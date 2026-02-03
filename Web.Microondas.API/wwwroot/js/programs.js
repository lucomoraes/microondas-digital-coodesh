window.ProgramsManager = class ProgramsManager {
    constructor(microwaveController) {
        this.microwave = microwaveController;
        this.programs = [];
        
        this.programSelect = document.getElementById('programSelect');
        this.programDetails = document.getElementById('programDetails');
        this.startProgramBtn = document.getElementById('startProgramBtn');
        
        this.toggleCustomFormBtn = document.getElementById('toggleCustomFormBtn');
        this.customProgramForm = document.getElementById('customProgramForm');
        this.saveCustomProgramBtn = document.getElementById('saveCustomProgramBtn');
        this.cancelCustomFormBtn = document.getElementById('cancelCustomFormBtn');
        this.customProgramsList = document.getElementById('customProgramsList');
        
        this.setupEventListeners();
    }

    setupEventListeners() {
        this.programSelect.addEventListener('change', () => this.handleProgramSelection());
        this.startProgramBtn.addEventListener('click', () => this.handleStartProgram());
        
        this.toggleCustomFormBtn.addEventListener('click', () => this.toggleCustomForm());
        this.saveCustomProgramBtn.addEventListener('click', () => this.handleSaveCustomProgram());
        this.cancelCustomFormBtn.addEventListener('click', () => this.hideCustomForm());
    }

    async loadPrograms() {
        try {
            this.programs = await api.getAllPrograms();
            this.renderProgramsDropdown();
            this.renderCustomProgramsList();
        } catch (error) {
            console.error('Failed to load programs:', error);
        }
    }

    renderProgramsDropdown() {
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
        this.customProgramForm.classList.toggle('hidden');
    }

    hideCustomForm() {
        this.customProgramForm.classList.add('hidden');
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
                this.renderProgramsDropdown();
                this.renderCustomProgramsList();
            } else {
                await this.loadPrograms();
            }
        } catch (error) {
            toast.error(`Erro ao criar programa: ${error.message}`);
        }
    }

    renderCustomProgramsList() {
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
            toast.success('Programa excluído com sucesso!');
            await this.loadPrograms();
        } catch (error) {
            toast.error(`Erro ao excluir programa: ${error.message}`);
        }
    }
}
