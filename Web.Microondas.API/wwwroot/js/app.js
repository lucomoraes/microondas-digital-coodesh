let authManager;
let microwaveController;
let programsManager;

window.switchToTab = function (tabName) {
    const tabButtons = document.querySelectorAll('.main-tab');
    const tabContents = document.querySelectorAll('.tab-content');

    tabButtons.forEach(btn => btn.classList.remove('active'));
    tabContents.forEach(content => content.classList.remove('active'));

    const targetButton = document.querySelector(`.main-tab[data-tab="${tabName}"]`);
    const targetTab = document.getElementById(`${tabName}Tab`);

    if (targetButton) {
        targetButton.classList.add('active');
    }
    if (targetTab) {
        targetTab.classList.add('active');
    }
}

function setupTabs() {
    const tabButtons = document.querySelectorAll('.main-tab');
    const tabContents = document.querySelectorAll('.tab-content');

    tabButtons.forEach(button => {
        button.addEventListener('click', () => {
            const tabName = button.dataset.tab;

            tabButtons.forEach(btn => btn.classList.remove('active'));
            tabContents.forEach(content => content.classList.remove('active'));

            button.classList.add('active');
            const targetTab = document.getElementById(`${tabName}Tab`);
            if (targetTab) {
                targetTab.classList.add('active');
            }
        });
    });
}

document.addEventListener('DOMContentLoaded', async () => {
    setupTabs();

    authManager = new AuthManager();
    microwaveController = new MicrowaveController();
    programsManager = new ProgramsManager(microwaveController);

    window.programsManager = programsManager;
    window.microwaveController = microwaveController;

    await authManager.initialize();
});

window.addEventListener('unhandledrejection', (event) => {
    console.error('Unhandled promise rejection:', event.reason);

    if (event.reason.message && event.reason.message.includes('autenticado')) {
        authManager.showLoginScreen();
    }
});
