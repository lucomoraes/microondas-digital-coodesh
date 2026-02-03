window.ProgramsManager = class ProgramsManager {
constructor(microwaveController) {
    this.microwave = microwaveController;
    this.programs = [];
    this.cacheKey = 'microwave_programs_cache';
        
    this.initializeElements();
    this.setupEventListeners();
    this.loadFromCache();
}
    
initializeElements() {
    this.programSelect = document.getElementById('programSelect');
    this.programDetails = document.getElementById('programDetails');
    this.startProgramBtn = document.getElementById('startProgramBtn');
        
    this.toggleCustomFormBtn = document.getElementById('toggleCustomFormBtn');
    this.customProgramForm = document.getElementById('customProgramForm');
    this.saveCustomProgramBtn = document.getElementById('saveCustomProgramBtn');
    this.cancelCustomFormBtn = document.getElementById('cancelCustomFormBtn');
    this.customProgramsList = document.getElementById('customProgramsList');
}

    setupEventListeners() {
        if (this.programSelect) {
            this.programSelect.addEventListener('change', () => this.handleProgramSelection());
        }
        if (this.startProgramBtn) {
            this.startProgramBtn.addEventListener('click', () => this.handleStartProgram());
        }
        if (this.toggleCustomFormBtn) {
            this.toggleCustomFormBtn.addEventListener('click', () => this.toggleCustomForm());
        }
        if (this.saveCustomProgramBtn) {
            this.saveCustomProgramBtn.addEventListener('click', () => this.handleSaveCustomProgram());
        }
        if (this.cancelCustomFormBtn) {
            this.cancelCustomFormBtn.addEventListener('click', () => this.hideCustomForm());
        }
    }

    loadFromCache() {
        try {
            const cached = localStorage.getItem(this.cacheKey);
            if (cached) {
                this.programs = JSON.parse(cached);
                this.renderProgramsDropdown();
                this.renderCustomProgramsList();
            }
        } catch (error) {
            console.error('Failed to load from cache:', error);
        }
    }

    saveToCache() {
        try {
            localStorage.setItem(this.cacheKey, JSON.stringify(this.programs));
        } catch (error) {
            console.error('Failed to save to cache:', error);
        }
    }

    async loadPrograms(forceRefresh = false) {
        if (!forceRefresh && this.programs.length > 0) {
            return;
        }
        
        try {
            this.programs = await api.getAllPrograms();
            this.saveToCache();
            this.renderProgramsDropdown();
            this.renderCustomProgramsList();
        } catch (error) {
            console.error('Failed to load programs:', error);
        }
    }

    renderProgramsDropdown() {
        if (!this.programSelect) {
            console.warn('programSelect element not found');
            return;
        }
        
        this.programSelect.innerHTML = '<option value="">Selecione um programa...</option>';
        
        this.programs.forEach(program => {
            const option = document.createElement('option');
            option.value = program.id;
            option.textContent = program.isPreset 
                ? `${program.name} (${program.food})` 
                : `${program.name} (${program.food}) - Customizado`;
            
            if (!program.isPreset) {
                option.style.fontStyle = 'italic';
            }
            
            this.programSelect.appendChild(option);
        });
    }

    handleProgramSelection() {
        if (!this.programSelect || !this.programDetails) {
            return;
        }
        
        const programId = this.programSelect.value;
        
        if (!programId) {
            this.programDetails.innerHTML = '';
            return;
        }
        
        const program = this.programs.find(p => p.id === programId);
        
        if (program) {
            this.displayProgramDetails(program);
        }
    }

    displayProgramDetails(program) {
        if (!this.programDetails) {
            return;
        }
        
        const minutes = Math.floor(program.timeInSeconds / 60);
        const seconds = program.timeInSeconds % 60;
        const timeStr = minutes > 0 
            ? `${minutes} minuto${minutes > 1 ? 's' : ''} e ${seconds} segundo${seconds !== 1 ? 's' : ''}`
            : `${seconds} segundo${seconds !== 1 ? 's' : ''}`;
        
        this.programDetails.innerHTML = `
            <h3>${program.name} ${program.isPreset ? '' : '(Customizado)'}</h3>
            <p><strong>Alimento:</strong> ${program.food}</p>
            <p><strong>Tempo:</strong> ${timeStr}</p>
            <p><strong>Pot&ecirc;ncia:</strong> ${program.power}/10</p>
            <p><strong>Caractere:</strong> ${program.character}</p>
            ${program.instructions ? `
                <div class="instructions">
                    <strong>Instru&ccedil;&otilde;es:</strong><br>
                    ${program.instructions}
                </div>
            ` : ''}
        `;
    }

    async handleStartProgram() {
        if (!this.programSelect) {
            return;
        }
        
        const programId = this.programSelect.value;
        
        if (!programId) {
            toast.warning('Por favor, selecione um programa.');
            return;
        }
        
        if (this.microwave.state && this.microwave.state.state === 'Running') {
            toast.warning('Micro-ondas já está em funcionamento. Use Pausar/Cancelar primeiro.');
            return;
        }
        
        try {
            this.microwave.state = await api.startProgram(programId);
            this.microwave.updateDisplay();
            this.microwave.startTimer();
            toast.success('Programa iniciado com sucesso!');
        } catch (error) {
            toast.error(`Erro ao iniciar programa: ${error.message}`);
        }
    }

    toggleCustomForm() {
        if (this.customProgramForm) {
            this.customProgramForm.classList.toggle('hidden');
        }
    }

    hideCustomForm() {
        if (this.customProgramForm) {
            this.customProgramForm.classList.add('hidden');
        }
        this.clearCustomForm();
    }

    clearCustomForm() {
        document.getElementById('customName').value = '';
        document.getElementById('customFood').value = '';
        document.getElementById('customTime').value = '';
        document.getElementById('customPower').value = '';
        document.getElementById('customChar').value = '';
        document.getElementById('customInstructions').value = '';
    }

    async handleSaveCustomProgram() {
        const name = document.getElementById('customName').value.trim();
        const food = document.getElementById('customFood').value.trim();
        const time = parseInt(document.getElementById('customTime').value);
        const power = parseInt(document.getElementById('customPower').value);
        const character = document.getElementById('customChar').value.trim();
        const instructions = document.getElementById('customInstructions').value.trim();

        if (!name || !food || !time || !power || !character) {
            toast.warning('Por favor, preencha todos os campos obrigatórios.');
            return;
        }

        if (time < 1 || time > 120) {
            toast.error('Tempo deve ser entre 1 e 120 segundos.');
            return;
        }

        if (power < 1 || power > 10) {
            toast.error('Potência deve ser entre 1 e 10.');
            return;
        }

        if (character.length !== 1) {
            toast.error('Caractere deve ter exatamente 1 caractere.');
            return;
        }

        const program = {
            name,
            food,
            timeInSeconds: time,
            power,
            character,
            instructions: instructions || null
        };

        try {
            const createdProgram = await api.createCustomProgram(program);
            toast.success('Programa customizado criado com sucesso!');
            this.hideCustomForm();
            
            if (createdProgram) {
                this.programs.push(createdProgram);
                this.saveToCache();
                this.renderProgramsDropdown();
                this.renderCustomProgramsList();
            } else {
                await this.loadPrograms(true);
            }
        } catch (error) {
            toast.error(`Erro ao criar programa: ${error.message}`);
        }
    }

    renderCustomProgramsList() {
        if (!this.customProgramsList) {
            console.warn('customProgramsList element not found');
            return;
        }
        
        const customPrograms = this.programs.filter(p => !p.isPreset);
        
        if (customPrograms.length === 0) {
            this.customProgramsList.innerHTML = '<p style="color: #666; text-align: center;">Nenhum programa customizado criado.</p>';
            return;
        }
        
        this.customProgramsList.innerHTML = customPrograms.map(program => `
            <div class="program-item custom">
                <div class="program-info">
                    <h4>${program.name}</h4>
                    <p><strong>Alimento:</strong> ${program.food}</p>
                    <p><strong>Tempo:</strong> ${program.timeInSeconds}s | <strong>Pot&ecirc;ncia:</strong> ${program.power}/10 | <strong>Char:</strong> ${program.character}</p>
                </div>
                <button class="btn btn-danger btn-small" onclick="programsManager.handleDeleteProgram('${program.id}')">Excluir</button>
            </div>
        `).join('');
    }

    async handleDeleteProgram(programId) {
        if (!confirm('Tem certeza que deseja excluir este programa?')) {
            return;
        }
        
        try {
            await api.deleteCustomProgram(programId);
            this.programs = this.programs.filter(p => p.id !== programId);
            this.saveToCache();
            this.renderProgramsDropdown();
            this.renderCustomProgramsList();
            toast.success('Programa excluído com sucesso!');
        } catch (error) {
            toast.error(`Erro ao excluir programa: ${error.message}`);
            await this.loadPrograms(true);
        }
    }
}
