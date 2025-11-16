/* =========================
     ГЛОБАЛЬНЫЕ ДАННЫЕ
========================= */

let filteredOrgs = []; // <-- обязательно!

(function(){
  const filters = [
    { name: "образование", icon: "school" },
    { name: "спорт", icon: "sports_soccer" },
    { name: "соц. поддержка", icon: "volunteer_activism" },
    { name: "благотворительность", icon: "favorite" },
    { name: "медицина", icon: "medical_services" },
    { name: "сообщество", icon: "groups" },
    { name: "религия", icon: "church" },
    { name: "экология", icon: "eco" },
    { name: "право", icon: "gavel" },
    { name: "наука", icon: "science" },
    { name: "культура", icon: "theaters" },
    { name: "бизнес", icon: "business_center" }
  ];

  const orgs = [
    { name: "Фонд развития школы №98", address: "Железногорск, Ленина, 48", tags: ["образование","бизнес","экология"] },
    { name: "Фонд поддержки семьи", address: "Железногорск, Гагарина, 12", tags: ["соц. поддержка","медицина"] },
    { name: "Спортивный центр 'Актив'", address: "Железногорск, Мира, 5", tags: ["спорт"] }
  ];

  const filtersEl = document.getElementById('filters');
  const filtersMobileEl = document.getElementById('filtersMobile');
  const orgListLeft = document.getElementById('orgListLeft');
  const orgListMobile = document.getElementById('orgListMobile');
  const searchInput = document.getElementById('searchInput');
  const searchInputMobile = document.getElementById('searchInputMobile');
  const mobilePanel = document.getElementById('mobilePanel');
  const panelHeader = document.getElementById('panelHeader');
  const mobileOpenBtn = document.getElementById('mobile-open-filters');
  const mobileCloseBtn = document.getElementById('mobileCloseBtn');
  const foundCount = document.getElementById('found-count');
  const foundCountMobile = document.getElementById('found-count-mobile');

  let activeFilters = new Set();
  let query = '';

/* =========================
      РЕНДЕР ФИЛЬТРОВ
========================= */

function renderFilters(container){
    container.innerHTML = '';
    filters.forEach(f => {
      const item = document.createElement('div');
      item.className = 'filter-item';

      const b = document.createElement('button');
      b.type = 'button';
      b.className = 'filter-btn';
      b.innerHTML = `<span class="material-icons">${f.icon}</span>`;
      b.onclick = () => toggleFilter(f.name, b);

      const lbl = document.createElement('div');
      lbl.className = 'filter-label';
      lbl.textContent = f.name;

      item.appendChild(b);
      item.appendChild(lbl);
      container.appendChild(item);
    });
}

/* =========================
      ПЕРЕКЛЮЧЕНИЕ ФИЛЬТРА
========================= */

function toggleFilter(name, node){
    if(activeFilters.has(name)){
      activeFilters.delete(name);
      node.classList.remove('active');
    } else {
      activeFilters.add(name);
      node.classList.add('active');
    }
    applyFilters();
}

/* =========================
      РЕНДЕР ОРГАНИЗАЦИЙ
========================= */

function renderOrgs(list){
    orgListLeft.innerHTML = '';
    orgListMobile.innerHTML = '';

    list.forEach(o => {
      const card = document.createElement('div');
      card.className = 'org-card';
      card.innerHTML =
        `<div class="tags">
           ${o.tags.map(t=>`<div class="tag">${t}</div>`).join('')}
         </div>
         <div class="org-name">${o.name}</div>
         <div class="org-address">${o.address}</div>`;

      orgListLeft.appendChild(card);

      const clone = card.cloneNode(true);
      orgListMobile.appendChild(clone);
    });

    foundCount.textContent = list.length;
    foundCountMobile.textContent = list.length;
}

/* =========================
         ФИЛЬТРАЦИЯ
========================= */

function applyFilters(){
    const q = query.trim().toLowerCase();

    filteredOrgs = orgs.filter(o =>
      o.name.toLowerCase().includes(q) ||
      o.address.toLowerCase().includes(q)
    );

    if(activeFilters.size){
      filteredOrgs = filteredOrgs.filter(o =>
        Array.from(activeFilters)
             .some(f =>
               o.tags.map(t => t.toLowerCase()).includes(f.toLowerCase())
             )
      );
    }

    renderOrgs(filteredOrgs);
    attachOrgClickHandlers();
}

/* =========================
          ПОИСК
========================= */

searchInput?.addEventListener('input', e=>{
    query = e.target.value;
    searchInputMobile.value = query;
    applyFilters();
});

searchInputMobile?.addEventListener('input', e=>{
    query = e.target.value;
    searchInput.value = query;
    applyFilters();
});

/* =========================
     МОБИЛЬНАЯ ПАНЕЛЬ
========================= */

panelHeader.addEventListener('click', ()=> mobilePanel.classList.toggle('open'));
mobileOpenBtn?.addEventListener('click', ()=> mobilePanel.classList.add('open'));
mobileCloseBtn?.addEventListener('click', ()=> mobilePanel.classList.remove('open'));

/* =========================
      ИНИЦИАЛИЗАЦИЯ
========================= */

renderFilters(filtersEl);
renderFilters(filtersMobileEl);
applyFilters();

})();


/* =========================
    АВТОКОМПЛИТ ГОРОДОВ
========================= */

const rosatomCities = [
  "Железногорск","Саров","Озерск","Заречный","Снежинск",
  "Нововоронеж","Северск","Трехгорный","Зеленогорск",
  "Лесной","Межгорье","Полярные Зори"
];

const cityInput = document.getElementById("city-input");
const citySuggestions = document.getElementById("city-suggestions");

function updateCitySuggestions() {
  const query = cityInput.value.trim().toLowerCase();
  if (!query) return citySuggestions.style.display = "none";

  const list = rosatomCities.filter(c =>
    c.toLowerCase().startsWith(query)
  );

  if (!list.length) return citySuggestions.style.display = "none";

  citySuggestions.innerHTML = "";
  list.forEach(city => {
    const li = document.createElement("li");
    li.textContent = city;
    li.onclick = () => {
      cityInput.value = city;
      citySuggestions.style.display = "none";
    };
    citySuggestions.appendChild(li);
  });

  citySuggestions.style.display = "block";
}

cityInput.addEventListener("input", updateCitySuggestions);
document.addEventListener("click", (e) => {
  if (!citySuggestions.contains(e.target) && e.target !== cityInput) {
    citySuggestions.style.display = "none";
  }
});


/* =========================
       СТАНДАРТНЫЕ МОДАЛКИ
========================= */

const loginModal = document.getElementById("login-modal");
const registerModal = document.getElementById("register-modal");
const overlay = document.getElementById("modal-overlay");

function openModal(modal) {
  overlay.classList.add("active");
  modal.classList.add("active");
}
function closeModals() {
  overlay.classList.remove("active");
  loginModal.classList.remove("active");
  registerModal.classList.remove("active");
}

document.getElementById("login-btn").onclick = () => openModal(loginModal);

document.querySelectorAll("[data-close]").forEach(btn => {
  btn.onclick = closeModals;
});

overlay.onclick = closeModals;

document.getElementById("open-register-link").onclick = () => {
  loginModal.classList.remove("active");
  registerModal.classList.add("active");
};

document.getElementById("open-login-link").onclick = () => {
  registerModal.classList.remove("active");
  loginModal.classList.add("active");
};


/* =========================
   МОДАЛКА ДОБАВЛЕНИЯ ТОЧКИ
========================= */

document.addEventListener('DOMContentLoaded', () => {
  const addBtn = document.getElementById('add-point-btn');
  const modal = document.getElementById('create-point-modal');
  const overlay = document.getElementById('modal-overlay');

  if (!addBtn || !modal || !overlay) return;

  const openM = () => {
    overlay.classList.add('active');
    modal.classList.add('active');
    document.documentElement.style.overflow = 'hidden';
  };
  const closeM = () => {
    overlay.classList.remove('active');
    modal.classList.remove('active');
    document.documentElement.style.overflow = '';
  };

  addBtn.addEventListener('click', (e) => {
    e.preventDefault();
    openM();
  });

  overlay.addEventListener('click', (e) => {
    if (e.target === overlay) closeM();
  });

  document.querySelectorAll('[data-close]').forEach(el => {
    el.addEventListener('click', (ev) => { ev.preventDefault(); closeM(); });
  });

  document.addEventListener('keydown', (ev) => {
    if (ev.key === 'Escape') closeM();
  });
});


/* =========================
     ТЕГИ В ФОРМЕ НКО
========================= */

document.addEventListener("DOMContentLoaded", () => {
    const container = document.getElementById("tagsSelect");
    if (!container) return;

    const tags = Array.from(container.children);

    tags.forEach((tag, i) => tag.dataset.originalIndex = i);

    function reorder() {
        const all = Array.from(container.children);
        const active = all.filter(t => t.classList.contains("active"));
        const inactive = all.filter(t => !t.classList.contains("active"));
        inactive.sort((a, b) => a.dataset.originalIndex - b.dataset.originalIndex);
        [...active, ...inactive].forEach(el => container.appendChild(el));
    }

    tags.forEach(tag => {
        tag.addEventListener("click", () => {
            tag.classList.toggle("active");
            reorder();
        });
    });
});


/* =========================
    ПРЕВЬЮ ЛОГО
========================= */

document.addEventListener("DOMContentLoaded", () => {
    const input = document.getElementById("nko-logo");
    const status = document.getElementById("file-status");
    if (!input) return;

    input.addEventListener("change", () => {
        if (input.files.length > 0) {
            status.textContent = input.files[0].name;
            status.className = "file-status success";
        } else {
            status.textContent = "";
            status.className = "file-status";
        }
    });
});


/* =========================
   МОДАЛКА ПРОСМОТРА ОРГ.
========================= */

function openOrgModal(org) {
    const modal = document.getElementById("org-view-modal");
    const overlay = document.getElementById("modal-overlay");

    document.getElementById("org-view-title").textContent = org.name;
    document.getElementById("org-view-logo").src = org.logo || "img/default.png";
    document.getElementById("org-view-address").textContent = org.address;
    document.getElementById("org-view-description").textContent = org.description || "Описание отсутствует";

    document.getElementById("org-view-contacts").innerHTML = `
        ${org.phone ? `<div><b>Телефон:</b> ${org.phone}</div>` : ""}
        ${org.email ? `<div><b>Email:</b> ${org.email}</div>` : ""}
        ${org.site ? `<div><b>Сайт:</b> <a href="${org.site}" target="_blank">${org.site}</a></div>` : ""}
    `;

    document.getElementById("org-view-events").innerHTML =
        org.events?.length
            ? org.events.map(ev => `<div style="margin-bottom:6px;">${ev}</div>`).join("")
            : "Нет мероприятий";

    overlay.classList.add("active");
    modal.classList.add("active");
}


/* =========================
     ПРИВЯЗКА КАРТОЧЕК
========================= */

function attachOrgClickHandlers() {
    document.querySelectorAll(".org-card").forEach((card, index) => {
        card.onclick = () => openOrgModal(filteredOrgs[index]);
    });
}
// Закрытие модалки по крестику и overlay
document.addEventListener("click", function (e) {
    if (e.target.hasAttribute("data-close")) {
        closeModal();
    }

    if (e.target.id === "modal-overlay") {
        closeModal();
    }
});

// Закрытие по Esc
document.addEventListener("keydown", function (e) {
    if (e.key === "Escape") {
        closeModal();
    }
});

function closeModal() {
    document.querySelectorAll(".modal.active").forEach(m => m.classList.remove("active"));
    const overlay = document.getElementById("modal-overlay");
    overlay.classList.remove("active");
}

function openOrgModal(org) {
    const modal = document.getElementById("org-view-modal");
    const overlay = document.getElementById("modal-overlay");

    // 1. Фото
    document.getElementById("org-view-logo").src = org.logo || "img/default.png";

    // 2. Тэги
    document.getElementById("org-view-tags").innerHTML =
        org.tags?.length
            ? org.tags.map(t => `<div class="tag">${t}</div>`).join("")
            : "";

    // 3. Название жирным
    document.getElementById("org-view-title").textContent = org.name;

    // 4. Адрес
    document.getElementById("org-view-address").textContent = org.address;

    // Описание
    document.getElementById("org-view-description").textContent =
        org.description || "Описание отсутствует";

    // Контакты
    document.getElementById("org-view-contacts").innerHTML = `
        ${org.phone ? `<div><b>Телефон:</b> ${org.phone}</div>` : ""}
        ${org.email ? `<div><b>Email:</b> ${org.email}</div>` : ""}
        ${org.site ? `<div><b>Сайт:</b> <a href="${org.site}" target="_blank">${org.site}</a></div>` : ""}
    `;

    // Мероприятия
    document.getElementById("org-view-events").innerHTML =
        org.events?.length
            ? org.events.map(ev => `<div style="margin-bottom:6px;">${ev}</div>`).join("")
            : "Нет мероприятий";

    overlay.classList.add("active");
    modal.classList.add("active");
}
