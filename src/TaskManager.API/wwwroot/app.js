// API-клиент и состояние приложения.
// Токен живёт в HttpOnly-купе, поэтому JS его не читает и не прикладывает
// вручную: браузер сам отправляет куку при credentials: 'include'.
// Статика отдаётся тем же origin, что и API, поэтому CORS не задействован.

const STATUS = { NEW: 0, IN_PROGRESS: 1, COMPLETED: 2 };
const PRIORITY = { LOW: 0, MEDIUM: 1, HIGH: 2 };

const STATUS_LABEL = { 0: 'Новая', 1: 'В работе', 2: 'Выполнено' };
const STATUS_CLASS = { 0: 'badge-new', 1: 'badge-in-progress', 2: 'badge-completed' };
const PRIORITY_LABEL = { 0: 'Низкий', 1: 'Средний', 2: 'Высокий' };
const PRIORITY_CLASS = { 0: 'badge-low', 1: 'badge-medium', 2: 'badge-high' };

const PAGE_SIZE = 5;

const state = {
    user: null,          // { id, email, name }
    projects: [],        // ProjectDto[]
    selectedId: null,    // id выбранного проекта
    page: 1,
    filters: { status: '', priority: '' }
};

// ---------- HTTP ----------

// Единственный «сырой» fetch — сюда ходят все запросы.
// credentials: 'include' прикладывает HttpOnly-куки (accessToken + refreshToken)
// автоматически, JS их не читает.
async function request(path, { method = 'GET', body, _retry = false } = {}) {
    const response = await fetch(path, {
        method,
        credentials: 'include',
        headers: body !== undefined ? { 'Content-Type': 'application/json' } : {},
        body: body !== undefined ? JSON.stringify(body) : undefined
    });

    // accessToken живёт 2 минуты. Если он истёк, но refreshToken — ещё валиден
    // (7 дней), пробуем обновить куки один раз.
    if (response.status === 401 && !_retry) {
        try {
            const refreshResponse = await fetch('/api/v1/auth/refresh', {
                method: 'POST',
                credentials: 'include'
            });

            if (refreshResponse.ok) {
                // Куки обновились (Set-Cookie через TokenCookieBehavior).
                // Повторяем оригинальный запрос с новыми токенами.
                return request(path, { method, body, _retry: true });
            }
        } catch {
            // Refresh не удался — сессия утеряна.
        }

        throw Object.assign(new Error('Сессия истекла. Войдите заново.'), { unauthorized: true });
    }

    if (response.status === 401) {
        throw Object.assign(new Error('Сессия истекла. Войдите заново.'), { unauthorized: true });
    }

    if (response.status === 204) return null;

    const text = await response.text();
    const payload = text ? JSON.parse(text) : null;

    if (!response.ok) {
        throw new Error(readableError(payload));
    }

    return payload;
}

// Контроллеры отдают либо ApiResponse { data, message },
// либо форму ошибки middleware { status, title, detail, errors }.
function readableError(payload) {
    if (!payload) return 'Неизвестная ошибка';
    if (payload.detail) {
        const fields = payload.errors
            ? ' ' + Object.values(payload.errors).flat().join('; ')
            : '';
        return payload.detail + fields;
    }
    return payload.message || payload.title || 'Ошибка запроса';
}

const api = {
    login: (body) => request('/api/v1/auth/login', { method: 'POST', body }),
    register: (body) => request('/api/v1/auth/register', { method: 'POST', body }),
    logout: () => request('/api/v1/auth/logout', { method: 'POST' }),

    projects: (userId, page) =>
        request(`/api/Projects/${userId}?offset=${page}&limit=${PAGE_SIZE}`),
    createProject: (body) => request('/api/Projects', { method: 'POST', body }),
    updateProject: (id, body) => request(`/api/Projects/${id}`, { method: 'PUT', body }),
    deleteProject: (id) => request(`/api/Projects/${id}`, { method: 'DELETE' }),

    createTask: (body) => request('/api/Task', { method: 'POST', body }),
    updateTask: (id, body) => request(`/api/Task/${id}`, { method: 'PATCH', body }),
    deleteTask: (id) => request(`/api/Task/${id}`, { method: 'DELETE' }),
    clearCompleted: () => request('/api/Task/completed', { method: 'DELETE' })
};

// ---------- DOM ----------

const $ = (selector) => document.querySelector(selector);

const el = {
    authScreen: $('#auth-screen'),
    appScreen: $('#app-screen'),
    loginForm: $('#login-form'),
    registerForm: $('#register-form'),
    currentUser: $('#current-user'),
    projectList: $('#project-list'),
    pageInfo: $('#page-info'),
    projectEmpty: $('#project-empty'),
    projectDetail: $('#project-detail'),
    projectTitle: $('#project-title'),
    projectDescription: $('#project-description'),
    taskList: $('#task-list'),
    filterStatus: $('#filter-status'),
    filterPriority: $('#filter-priority'),
    projectModal: $('#project-modal'),
    projectForm: $('#project-form'),
    projectModalTitle: $('#project-modal-title'),
    taskModal: $('#task-modal'),
    taskForm: $('#task-form'),
    toast: $('#toast')
};

function showScreen(name) {
    el.authScreen.classList.toggle('hidden', name !== 'auth');
    el.appScreen.classList.toggle('hidden', name !== 'app');
}

let toastTimer;
function toast(message, kind = 'success') {
    el.toast.textContent = message;
    el.toast.className = `toast toast-${kind}`;
    clearTimeout(toastTimer);
    toastTimer = setTimeout(() => el.toast.classList.add('hidden'), 4000);
}

// Эндпоинта /me в API нет, а список проектов требует userId.
// Профиль (не токен!) храним в localStorage — куки остаются единственным
// доказательством подлинности, здесь это только удобство навигации.
const PROFILE_KEY = 'tm.profile';

function saveProfile(user) {
    localStorage.setItem(PROFILE_KEY, JSON.stringify(user));
}

function loadProfile() {
    try {
        return JSON.parse(localStorage.getItem(PROFILE_KEY));
    } catch {
        return null;
    }
}

function clearProfile() {
    localStorage.removeItem(PROFILE_KEY);
}

// Обработка потери сессии для всех мутаций и загрузок.
function guard(error) {
    if (error.unauthorized) {
        state.user = null;
        clearProfile();
        showScreen('auth');
    }
    toast(error.message, 'error');
}

// ---------- Рендер ----------

function renderProjects() {
    el.projectList.innerHTML = '';

    if (state.projects.length === 0) {
        const li = document.createElement('li');
        li.className = 'project-item muted';
        li.textContent = 'Пока нет проектов';
        el.projectList.appendChild(li);
        return;
    }

    for (const project of state.projects) {
        const li = document.createElement('li');
        li.className = 'project-item' + (project.id === state.selectedId ? ' active' : '');
        li.dataset.id = project.id;

        const done = (project.tasks || []).filter((t) => t.status === STATUS.COMPLETED).length;
        const total = (project.tasks || []).length;

        li.innerHTML = `
            <div class="project-item-name"></div>
            <div class="project-item-date"></div>`;
        li.querySelector('.project-item-name').textContent = project.title;
        li.querySelector('.project-item-date').textContent =
            `${fmtDate(project.createdAt)} · ${done}/${total} готово`;

        el.projectList.appendChild(li);
    }

    el.pageInfo.textContent = String(state.page);
}

function visibleTasks(project) {
    const { status, priority } = state.filters;
    return (project.tasks || []).filter((task) => {
        if (status !== '' && task.status !== Number(status)) return false;
        if (priority !== '' && task.priority !== Number(priority)) return false;
        return true;
    });
}

function renderProjectDetail() {
    const project = selectedProject();

    if (!project) {
        el.projectDetail.classList.add('hidden');
        el.projectEmpty.classList.remove('hidden');
        return;
    }

    el.projectEmpty.classList.add('hidden');
    el.projectDetail.classList.remove('hidden');
    el.projectTitle.textContent = project.title;
    el.projectDescription.textContent = project.description;

    el.taskList.innerHTML = '';

    const tasks = visibleTasks(project);
    if (tasks.length === 0) {
        const li = document.createElement('li');
        li.className = 'muted';
        li.textContent = 'Задач нет';
        el.taskList.appendChild(li);
        return;
    }

    for (const task of tasks) {
        const li = document.createElement('li');
        li.className = 'task-item';
        li.dataset.id = task.id;

        const nextStatus = task.status === STATUS.COMPLETED ? STATUS.NEW : STATUS.COMPLETED;
        const nextLabel = task.status === STATUS.COMPLETED ? 'Вернуть' : 'Готово';

        li.innerHTML = `
            <div class="task-item-header">
                <div class="task-item-title"></div>
                <div class="task-item-desc"></div>
                <div class="task-item-meta">
                    <span class="badge ${STATUS_CLASS[task.status]}">${STATUS_LABEL[task.status]}</span>
                    <span class="badge ${PRIORITY_CLASS[task.priority]}">${PRIORITY_LABEL[task.priority]}</span>
                    ${task.dueDate ? `<span class="muted">до ${fmtDate(task.dueDate)}</span>` : ''}
                </div>
            </div>
            <div class="task-item-actions">
                <button class="btn btn-ghost" data-action="toggle" data-next="${nextStatus}">${nextLabel}</button>
                <button class="btn btn-ghost" data-action="delete">Удалить</button>
            </div>`;

        // textContent вместо вставки в HTML — защита от XSS в названиях задач.
        li.querySelector('.task-item-title').textContent = task.title;
        li.querySelector('.task-item-desc').textContent = task.description;

        el.taskList.appendChild(li);
    }
}

function selectedProject() {
    return state.projects.find((p) => p.id === state.selectedId) || null;
}

function fmtDate(value) {
    if (!value) return '';
    const date = new Date(value);
    return Number.isNaN(date.getTime())
        ? ''
        : date.toLocaleDateString('ru-RU', { day: '2-digit', month: 'short', year: 'numeric' });
}

// ---------- Данные ----------

async function loadProjects() {
    const response = await api.projects(state.user.id, state.page);
    state.projects = response.data.content || [];

    // Если выбранный проект ушёл на другую страницу — снимаем выделение.
    if (!selectedProject()) state.selectedId = state.projects[0]?.id ?? null;

    renderProjects();
    renderProjectDetail();
}

async function refresh() {
    try {
        await loadProjects();
    } catch (error) {
        guard(error);
    }
}

async function enterApp(user) {
    state.user = user;
    state.page = 1;
    state.selectedId = null;
    saveProfile(user);
    el.currentUser.textContent = `${user.name} (${user.email})`;
    showScreen('app');
    await refresh();
}

// ---------- События ----------

// Вкладки входа и регистрации
document.querySelectorAll('[data-auth-tab]').forEach((tab) => {
    tab.addEventListener('click', () => {
        document.querySelectorAll('[data-auth-tab]').forEach((t) => t.classList.remove('active'));
        tab.classList.add('active');
        const isLogin = tab.dataset.authTab === 'login';
        el.loginForm.classList.toggle('hidden', !isLogin);
        el.registerForm.classList.toggle('hidden', isLogin);
    });
});

el.loginForm.addEventListener('submit', async (event) => {
    event.preventDefault();
    const data = Object.fromEntries(new FormData(el.loginForm));
    try {
        const response = await api.login(data);
        await enterApp(response.data);
    } catch (error) {
        guard(error);
    }
});

// Регистрация не выдаёт куки, поэтому сразу входим под созданным аккаунтом.
el.registerForm.addEventListener('submit', async (event) => {
    event.preventDefault();
    const data = Object.fromEntries(new FormData(el.registerForm));
    try {
        await api.register(data);
        toast('Аккаунт создан', 'success');
        const response = await api.login({ email: data.email, password: data.password });
        await enterApp(response.data);
    } catch (error) {
        guard(error);
    }
});

$('#logout-btn').addEventListener('click', async () => {
    try {
        await api.logout();
    } catch (error) {
        // Выход работает даже при истёкшем accessToken.
        console.warn(error);
    }
    state.user = null;
    state.projects = [];
    state.selectedId = null;
    clearProfile();
    showScreen('auth');
});

// Выбор проекта
el.projectList.addEventListener('click', (event) => {
    const item = event.target.closest('.project-item[data-id]');
    if (!item) return;
    state.selectedId = Number(item.dataset.id);
    renderProjects();
    renderProjectDetail();
});

// Пагинация
$('#prev-page').addEventListener('click', () => {
    if (state.page <= 1) return;
    state.page -= 1;
    refresh();
});

$('#next-page').addEventListener('click', () => {
    if (state.projects.length < PAGE_SIZE) return;
    state.page += 1;
    refresh();
});

// Фильтры задач — клиентские, данные уже приходят внутри проекта
el.filterStatus.addEventListener('change', (event) => {
    state.filters.status = event.target.value;
    renderProjectDetail();
});

el.filterPriority.addEventListener('change', (event) => {
    state.filters.priority = event.target.value;
    renderProjectDetail();
});

// Модалка проекта
function openProjectModal(project = null) {
    el.projectForm.reset();
    el.projectModalTitle.textContent = project ? 'Изменить проект' : 'Новый проект';
    el.projectForm.elements.id.value = project ? project.id : '';
    if (project) {
        el.projectForm.elements.title.value = project.title;
        el.projectForm.elements.description.value = project.description;
    }
    el.projectModal.classList.remove('hidden');
}

$('#new-project-btn').addEventListener('click', () => openProjectModal());

$('#edit-project-btn').addEventListener('click', () => {
    const project = selectedProject();
    if (project) openProjectModal(project);
});

el.projectForm.addEventListener('submit', async (event) => {
    event.preventDefault();
    const form = el.projectForm;
    const id = form.elements.id.value;
    const payload = {
        title: form.elements.title.value,
        description: form.elements.description.value
    };

    try {
        // PUT обновляет оба поля целиком. PATCH (частичное обновление) тоже
        // работает, но PUT надёжнее: он не оставит project с null-полями.
        if (id) {
            await api.updateProject(Number(id), payload);
        } else {
            await api.createProject(payload);
        }
        el.projectModal.classList.add('hidden');
        toast(id ? 'Проект обновлён' : 'Проект создан', 'success');
        await refresh();
    } catch (error) {
        guard(error);
    }
});

$('#delete-project-btn').addEventListener('click', async () => {
    const project = selectedProject();
    if (!project) return;
    if (!confirm(`Удалить проект «${project.title}» со всеми задачами?`)) return;

    try {
        await api.deleteProject(project.id);
        state.selectedId = null;
        toast('Проект удалён', 'success');
        await refresh();
    } catch (error) {
        guard(error);
    }
});

// Модалка задачи
$('#new-task-btn').addEventListener('click', () => {
    if (!selectedProject()) return;
    el.taskForm.reset();
    el.taskModal.classList.remove('hidden');
});

el.taskForm.addEventListener('submit', async (event) => {
    event.preventDefault();
    const form = el.taskForm;
    const project = selectedProject();
    const due = form.elements.dueDate.value;

    try {
        await api.createTask({
            projectId: project.id,
            title: form.elements.title.value,
            description: form.elements.description.value || '—',
            status: STATUS.NEW,
            priority: Number(form.elements.priority.value),
            dueDate: due ? new Date(due).toISOString() : null
        });
        el.taskModal.classList.add('hidden');
        toast('Задача создана', 'success');
        await refresh();
    } catch (error) {
        guard(error);
    }
});

// Действия внутри списка задач
el.taskList.addEventListener('click', async (event) => {
    const button = event.target.closest('[data-action]');
    if (!button) return;

    const id = Number(button.closest('.task-item').dataset.id);
    const action = button.dataset.action;

    try {
        if (action === 'toggle') {
            await api.updateTask(id, { status: Number(button.dataset.next) });
        } else if (action === 'delete') {
            if (!confirm('Удалить задачу?')) return;
            await api.deleteTask(id);
        }
        await refresh();
    } catch (error) {
        guard(error);
    }
});

$('#clear-completed-btn').addEventListener('click', async () => {
    if (!confirm('Удалить все выполненные задачи?')) return;
    try {
        const response = await api.clearCompleted();
        toast(`Удалено задач: ${response.data?.deletedCount ?? 0}`, 'success');
        await refresh();
    } catch (error) {
        guard(error);
    }
});

// Закрытие модалок
document.querySelectorAll('[data-close-modal]').forEach((button) => {
    button.addEventListener('click', () => {
        el.projectModal.classList.add('hidden');
        el.taskModal.classList.add('hidden');
    });
});

[el.projectModal, el.taskModal].forEach((modal) => {
    modal.addEventListener('click', (event) => {
        if (event.target === modal) modal.classList.add('hidden');
    });
});

document.addEventListener('keydown', (event) => {
    if (event.key === 'Escape') {
        el.projectModal.classList.add('hidden');
        el.taskModal.classList.add('hidden');
    }
});

// ---------- Старт ----------
// accessToken живёт 2 минуты, refreshToken — 7 дней.
// При 401 фронтенд автоматически вызывает /auth/refresh и повторяет запрос,
// так что пользователю не нужно перелогиниться каждый раз.
(async function init() {
    const profile = loadProfile();
    if (!profile?.id) {
        showScreen('auth');
        return;
    }

    state.user = profile;
    el.currentUser.textContent = `${profile.name} (${profile.email})`;
    showScreen('app');
    await refresh();
})();
