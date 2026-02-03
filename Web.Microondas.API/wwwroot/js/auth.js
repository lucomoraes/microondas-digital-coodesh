async function updateAuthStatusIndicator() {
    const indicator = document.getElementById('authStatusIndicator');
    if (!indicator) return;
    const isValid = await api.checkAuthStatus();
    indicator.classList.remove('green', 'grey');
    indicator.classList.add(isValid ? 'green' : 'grey');
}

class AuthManager {
    constructor() {
        this.loginForm = document.getElementById('loginForm');
        this.registerForm = null;
        this.loginError = document.getElementById('loginError');
        this.authStatus = document.getElementById('authStatus');
        this.logoutBtn = document.getElementById('logoutBtn');
        this.userInfo = document.getElementById('userInfo');

        this.setupEventListeners();
    }

    setupEventListeners() {
        this.loginForm.addEventListener('submit', (e) => this.handleLogin(e));
        this.logoutBtn.addEventListener('click', () => this.handleLogout());
        setTimeout(() => {
            this.registerForm = document.getElementById('registerForm');
            if (this.registerForm) {
                this.registerForm.addEventListener('submit', (e) => this.handleRegister(e));
            }
            this.setupUserManagement();
        }, 100);
    }

    setupUserManagement() {
        this.usersListDiv = document.getElementById('usersList');
        this.editUserForm = document.getElementById('editUserFormSubmit');
        this.editUserFormDiv = document.getElementById('editUserForm');
        this.userFormDiv = document.getElementById('userForm');
        this.toggleUserFormBtn = document.getElementById('toggleUserFormBtn');
        this.cancelUserFormBtn = document.getElementById('cancelUserFormBtn');
        this.cancelEditUserBtn = document.getElementById('cancelEditUserBtn');
        this.editUsername = document.getElementById('editUsername');
        this.editPassword = document.getElementById('editPassword');
        this.editUserError = document.getElementById('editUserError');
        this.editUserSuccess = document.getElementById('editUserSuccess');
        this.currentEditUserId = null;
        
        if (this.toggleUserFormBtn) {
            this.toggleUserFormBtn.addEventListener('click', () => this.showUserForm());
        }
        if (this.cancelUserFormBtn) {
            this.cancelUserFormBtn.addEventListener('click', () => this.hideUserForm());
        }
        if (this.cancelEditUserBtn) {
            this.cancelEditUserBtn.addEventListener('click', () => this.hideEditUserForm());
        }
        if (this.editUserForm) {
            this.editUserForm.addEventListener('submit', (e) => this.handleEditUser(e));
        }
        this.loadUsers();
    }

    async loadUsers() {
        if (!this.usersListDiv) return;

        try {
            const users = await api.getAllUsers();
            this.renderUsersList(users);
        } catch (error) {
            console.error('Error loading users:', error);
            this.usersListDiv.innerHTML = '<p style="color: #666; text-align: center;">Erro ao carregar usuários.</p>';
        }
    }

    renderUsersList(users) {
        if (!Array.isArray(users) || users.length === 0) {
            this.usersListDiv.innerHTML = '<p style="color: #666; text-align: center;">Nenhum usuário registrado.</p>';
            return;
        }

        this.usersListDiv.innerHTML = users.map(user => `
            <div class="user-item">
                <div class="user-info">
                    <h4>${user.username}</h4>
                    <p><strong>ID:</strong> ${user.id}</p>
                </div>
                <div style="display: flex; gap: 10px;">
                    <button class="btn btn-small btn-secondary" data-id="${user.id}">Editar</button>
                    <button class="btn btn-small btn-danger" data-id="${user.id}">Excluir</button>
                </div>
            </div>
        `).join('');

        this.usersListDiv.querySelectorAll('.btn-secondary').forEach(btn => {
            btn.addEventListener('click', (e) => this.showEditUserForm(e.target.dataset.id));
        });
        this.usersListDiv.querySelectorAll('.btn-danger').forEach(btn => {
            btn.addEventListener('click', (e) => this.handleDeleteUser(e.target.dataset.id));
        });
    }

    showUserForm() {
        this.hideEditUserForm();
        this.userFormDiv.classList.remove('hidden');
        this.toggleUserFormBtn.textContent = '❌ Cancelar';
        this.toggleUserFormBtn.onclick = () => this.hideUserForm();
    }

    hideUserForm() {
        this.userFormDiv.classList.add('hidden');
        this.toggleUserFormBtn.textContent = '➕ Criar Novo Usuário';
        this.toggleUserFormBtn.onclick = () => this.showUserForm();
        if (this.registerForm) {
            this.registerForm.reset();
        }
        const errorDiv = document.getElementById('registerError');
        const successDiv = document.getElementById('registerSuccess');
        if (errorDiv) errorDiv.textContent = '';
        if (successDiv) successDiv.textContent = '';
    }

    showEditUserForm(id) {
        this.hideUserForm();
        this.currentEditUserId = id;
        this.editUserError.textContent = '';
        this.editUserSuccess.textContent = '';
        api.getAllUsers().then(users => {
            const user = users.find(u => u.id === id);
            if (user) {
                this.editUsername.value = user.username;
                this.editPassword.value = '';
                this.editUserFormDiv.classList.remove('hidden');
                
                const usersList = document.getElementById('usersList');
                const userItem = usersList.querySelector(`[data-id="${id}"]`).closest('.user-item');
                if (userItem && this.editUserFormDiv.parentNode !== userItem.parentNode) {
                    userItem.after(this.editUserFormDiv);
                }
            }
        });
    }

    hideEditUserForm() {
        this.editUserFormDiv.classList.add('hidden');
        this.currentEditUserId = null;
        this.editUserError.textContent = '';
        this.editUserSuccess.textContent = '';
    }

    async handleEditUser(e) {
        e.preventDefault();
        const username = this.editUsername.value.trim();
        const password = this.editPassword.value.trim();
        this.editUserError.textContent = '';
        this.editUserSuccess.textContent = '';
        if (!username) {
            this.editUserError.textContent = 'Usuário é obrigatório.';
            return;
        }
        try {
            await api.updateUser(this.currentEditUserId, password ? { username, password } : { username });
            this.editUserSuccess.textContent = 'Usuário atualizado com sucesso!';
            this.loadUsers();
            setTimeout(() => {
                this.hideEditUserForm();
            }, 1200);
        } catch (e) {
            this.editUserError.textContent = e.message || 'Erro ao atualizar usuário.';
        }
    }

    async handleLogin(e) {
        e.preventDefault();

        const username = document.getElementById('username').value;
        const password = document.getElementById('password').value;

        this.loginError.textContent = '';
        this.loginError.classList.add('hidden');

        try {
            const response = await api.login(username, password);
            if (response.token) {
                this.showAuthSuccess('Login realizado com sucesso!');
                this.showMainScreen();
                this.updateUserInfo(response.username || username);
                updateAuthStatusIndicator();
                this.loadUsers();

                if (window.programsManager) {
                    window.programsManager.loadPrograms(true);
                }
                if (window.microwaveController) {
                    window.microwaveController.refreshState();
                }
            }
        } catch (error) {
            this.showAuthError(error.message || 'Usuário ou senha inválidos.');
        }
    }

    async handleRegister(e) {
        e.preventDefault();
        const username = document.getElementById('regUsername').value;
        const password = document.getElementById('regPassword').value;
        const confirmPassword = document.getElementById('regPasswordConfirm').value;
        const errorDiv = document.getElementById('registerError');
        const successDiv = document.getElementById('registerSuccess');
        errorDiv.textContent = '';
        successDiv.textContent = '';
        if (password !== confirmPassword) {
            errorDiv.textContent = 'As senhas não coincidem.';
            return;
        }
        if (password.length < 4) {
            errorDiv.textContent = 'A senha deve ter pelo menos 4 caracteres.';
            return;
        }
        try {
            await api.createUser({ username, password });
            successDiv.textContent = `✅ Usuário "${username}" registrado com sucesso!`;
            this.registerForm.reset();
            this.loadUsers();
            setTimeout(() => {
                successDiv.textContent = '';
                this.hideUserForm();
            }, 1500);
        } catch (error) {
            errorDiv.textContent = error.message || 'Erro ao registrar usuário.';
        }
    }

    async handleDeleteUser(id) {
        if (!confirm('Tem certeza que deseja excluir este usuário?')) return;
        try {
            await api.deleteUser(id);
            toast.success('Usuário excluído com sucesso!');
            this.loadUsers();
        } catch (e) {
            toast.error('Erro ao excluir usuário.');
        }
    }

    handleLogout() {
        api.clearToken();
        this.showLoginScreen();
        this.loginForm.reset();
        if (this.registerForm) {
            this.registerForm.reset();
        }
        this.authStatus.textContent = '';
        this.authStatus.className = 'auth-status';
        updateAuthStatusIndicator();
    }

    async checkAuthStatus() {
        await updateAuthStatusIndicator();
        return await api.checkAuthStatus();
    }

    updateUserInfo(username) {
        this.userInfo.textContent = `👤 ${username}`;
        updateAuthStatusIndicator();
    }

    showAuthSuccess(message) {
        this.authStatus.textContent = `✅ ${message}`;
        this.authStatus.className = 'auth-status success';
    }

    showAuthError(message) {
        this.loginError.textContent = `❌ ${message}`;
        this.loginError.classList.remove('hidden');
    }

    showLoginScreen() {
        document.getElementById('loginScreen').classList.add('active');
        document.getElementById('mainScreen').classList.remove('active');
    }

    showMainScreen() {
        document.getElementById('loginScreen').classList.remove('active');
        document.getElementById('mainScreen').classList.add('active');
    }

    async initialize() {
        const isAuthenticated = await this.checkAuthStatus();

        if (isAuthenticated) {
            this.showMainScreen();

            if (window.programsManager) {
                window.programsManager.loadPrograms(true);
            }
            if (window.microwaveController) {
                window.microwaveController.refreshState();
            }
        } else {
            this.showLoginScreen();
        }
    }
}

if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => {
        updateAuthStatusIndicator();
    });
} else {
    updateAuthStatusIndicator();
}


