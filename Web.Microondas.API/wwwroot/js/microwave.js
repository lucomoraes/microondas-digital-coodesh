class MicrowaveController {
constructor() {
    this.state = null;
    this.timer = null;
    this.isTicking = false;
    this.currentPower = 10;
        
    this.displayTime = document.getElementById('displayTime');
    this.powerCycleBtn = document.getElementById('powerCycleBtn');
        
    this.quickStartBtn = document.getElementById('quickStartBtn');
    this.startBtn = document.getElementById('startBtn');
    this.pauseCancelBtn = document.getElementById('pauseCancelBtn');
        
    this.setupEventListeners();
    this.setupKeypad();
}

    playBeepSound() {
        const audioContext = new (window.AudioContext || window.webkitAudioContext)();
        
        const beep = (frequency, duration, delay) => {
            setTimeout(() => {
                const oscillator = audioContext.createOscillator();
                const gainNode = audioContext.createGain();
                
                oscillator.connect(gainNode);
                gainNode.connect(audioContext.destination);
                
                oscillator.frequency.value = frequency;
                oscillator.type = 'sine';
                
                gainNode.gain.setValueAtTime(0.3, audioContext.currentTime);
                gainNode.gain.exponentialRampToValueAtTime(0.01, audioContext.currentTime + duration);
                
                oscillator.start(audioContext.currentTime);
                oscillator.stop(audioContext.currentTime + duration);
            }, delay);
        };
        
        beep(800, 0.15, 0);   
        beep(800, 0.15, 200); 
        beep(800, 0.15, 400);    
    }

    setupEventListeners() {
        this.quickStartBtn.addEventListener('click', () => this.handleQuickStart());
        this.startBtn.addEventListener('click', () => this.handleManualStart());
        this.pauseCancelBtn.addEventListener('click', () => this.handlePauseCancel());
        this.powerCycleBtn.addEventListener('click', () => this.cyclePower());
    }

    setupKeypad() {
        const keys = document.querySelectorAll('.key[data-number]');
        const clearKey = document.querySelector('.key-clear');
        
        keys.forEach(key => {
            key.addEventListener('click', () => {
                const number = key.dataset.number;
                this.appendToTimeInput(number);
            });
        });
        
        if (clearKey) {
            clearKey.addEventListener('click', () => {
                this.displayTime.textContent = '0:00';
            });
        }
    }

    appendToTimeInput(number) {
        let currentDigits = this.displayTime.textContent.replace(/[^0-9]/g, '');
        
        currentDigits = currentDigits.replace(/^0+/, '') || '0';
        
        let newDigits = currentDigits + number;
        
        if (newDigits.length > 4) {
            return;
        }
        
        newDigits = newDigits.padStart(4, '0');
        
        const minutes = parseInt(newDigits.substring(0, 2));
        const seconds = parseInt(newDigits.substring(2, 4));
        
        if (seconds >= 60) {
            return; 
        }
        
        const totalSeconds = (minutes * 60) + seconds;
        
        if (totalSeconds > 120) {
            return; 
        }
        
        this.displayTime.textContent = `${minutes}:${seconds.toString().padStart(2, '0')}`;
    }

    cyclePower() {
        this.currentPower = this.currentPower >= 10 ? 1 : this.currentPower + 1;
        this.powerCycleBtn.textContent = `🔥 ${this.currentPower}`;
    }

    async handleQuickStart() {
        try {
            this.state = await api.quickStart();
            this.updateDisplay();
            this.startTimer();
        } catch (error) {
            this.showError(error.message);
        }
    }

    async handleManualStart() {
        if (this.state && this.state.state === 'Running') {
            try {
                this.state = await api.quickStart();
                this.updateDisplay();
            } catch (error) {
                this.showError(error.message);
            }
            return;
        }
        
        const displayText = this.displayTime.textContent.replace(/[^0-9]/g, '');
        const seconds = parseInt(displayText) || 30;
        const power = this.currentPower;

        if (seconds < 1 || seconds > 120) {
            this.showError('Tempo deve ser entre 1 e 120 segundos (2 minutos)');
            return;
        }

        try {
            this.state = await api.startManual(seconds, power);
            this.updateDisplay();
            this.startTimer();
        } catch (error) {
            this.showError(error.message);
        }
    }

    async handlePauseCancel() {
        try {
            this.state = await api.pauseCancel();
            
            if (this.state.state === 'Paused') {
                this.stopTimer();
            } else if (this.state.state === 'Idle') {
                this.stopTimer();
                this.resetDisplay();
            }
            
            this.updateDisplay();
        } catch (error) {
            this.showError(error.message);
        }
    }

    async tick() {
        if (this.isTicking) {
            console.log('Tick already in progress, skipping...');
            return;
        }
        
        this.isTicking = true;
        
        try {
            this.state = await api.tick();
            this.updateDisplay();
            
            if (this.state.state === 'Completed') {
                this.stopTimer();
                this.playBeepSound();
            } else if (this.state.state === 'Idle' || this.state.remainingSeconds <= 0) {
                this.stopTimer();
            }
        } catch (error) {
            this.stopTimer();
            this.showError(error.message);
        } finally {
            this.isTicking = false;
        }
    }

    startTimer() {
        this.stopTimer();
        this.updateDisplay();
        this.timer = setInterval(() => this.tick(), 1000);
    }

    stopTimer() {
        if (this.timer) {
            clearInterval(this.timer);
            this.timer = null;
        }
    }

    updateDisplay() {
        if (!this.state) return;

        const minutes = Math.floor(this.state.remainingSeconds / 60);
        const seconds = this.state.remainingSeconds % 60;
        this.displayTime.textContent = `${minutes}:${seconds.toString().padStart(2, '0')}`;
        
        this.updateButtonStates();
    }

    updateButtonStates() {
        if (!this.state) return;
        
        this.startBtn.disabled = false;
        this.quickStartBtn.disabled = false;
    }

    resetDisplay() {
        this.displayTime.textContent = '0:00';
        this.startBtn.disabled = false;
        this.quickStartBtn.disabled = false;
    }

    showError(message) {
        toast.error(message);
    }

    async refreshState() {
        try {
            this.state = await api.getMicrowaveState();
            this.updateDisplay();
        } catch (error) {
            console.error('Failed to refresh state:', error);
        }
    }
}
