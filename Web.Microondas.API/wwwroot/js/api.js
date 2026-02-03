const API_BASE_URL = 'https://localhost:7219/api';

class ApiClient {
    constructor() {
        this.token = localStorage.getItem('jwt_token');
    }

    setToken(token) {
        this.token = token;
        localStorage.setItem('jwt_token', token);
    }

    clearToken() {
        this.token = null;
        localStorage.removeItem('jwt_token');
    }

    getHeaders() {
        const headers = {
            'Content-Type': 'application/json'
        };

        if (this.token) {
            headers['Authorization'] = `Bearer ${this.token}`;
        }

        return headers;
    }

    async request(endpoint, options = {}) {
        const url = `${API_BASE_URL}${endpoint}`;
        const config = {
            ...options,
            headers: this.getHeaders()
        };

        try {
            const response = await fetch(url, config);
            
            if (response.status === 401) {
                this.clearToken();
                throw new Error('Falha de login. Tente novamente.');
            }

            if (!response.ok) {
                const error = await response.json().catch(() => ({ message: 'Erro desconhecido' }));
                throw new Error(error.message || `Erro HTTP: ${response.status}`);
            }

            if (response.status === 204) {
                return null;
            }

            const contentType = response.headers.get('content-type');
            if (contentType && contentType.includes('application/json')) {
                return await response.json();
            }

            return null;
        } catch (error) {
            console.error('API Request Error:', error);
            throw error;
        }
    }


    async login(username, password) {
        const response = await this.request('/auth/login', {
            method: 'POST',
            body: JSON.stringify({ username, password })
        });
        
        if (response.token) {
            this.setToken(response.token);
        }
        
        return response;
    }

    async register(username, password) {
        return await this.request('/auth/createuser', {
            method: 'POST',
            body: JSON.stringify({ username, password })
        });
    }

    async getAuthStatus() {
        return await this.request('/auth/status');
    }

    async getMicrowaveState() {
        return await this.request('/microwave/state');
    }

    async quickStart() {
        return await this.request('/microwave/quickstart', { method: 'POST' });
    }

    async startManual(seconds, power) {
        return await this.request(`/microwave/manualstart?seconds=${seconds}&power=${power}`, {
            method: 'POST'
        });
    }

    async startProgram(programId) {
        return await this.request(`/microwave/programstart/${programId}`, {
            method: 'POST'
        });
    }

    async tick() {
        return await this.request('/microwave/tick', { method: 'POST' });
    }

    async pauseCancel() {
        return await this.request('/microwave/pause-cancel', { method: 'POST' });
    }

    async resume() {
        return await this.request('/microwave/resume', { method: 'POST' });
    }

    async reset() {
        return await this.request('/microwave/reset', { method: 'POST' });
    }

    async getAllPrograms() {
        return await this.request('/heatingprograms');
    }

    async getProgramById(id) {
        return await this.request(`/heatingprograms/${id}`);
    }

    async createCustomProgram(program) {
        return await this.request('/heatingprograms', {
            method: 'POST',
            body: JSON.stringify(program)
        });
    }

    async deleteCustomProgram(id) {
        return await this.request(`/heatingprograms/${id}`, {
            method: 'DELETE'
        });
    }

    async checkAuthStatus() {
        try {
            await this.request('/auth/check');
            return true;
        } catch (e) {
            return false;
        }
    }

    async createUser(user) {
        return await this.request('/user', {
            method: 'POST',
            body: JSON.stringify(user)
        });
    }

    async updateUser(id, user) {
        return await this.request(`/user/${id}`, {
            method: 'PUT',
            body: JSON.stringify(user)
        });
    }

    async deleteUser(id) {
        return await this.request(`/user/${id}`, {
            method: 'DELETE'
        });
    }

    async getAllUsers() {
        return await this.request('/user');
    }
}

const api = new ApiClient();
