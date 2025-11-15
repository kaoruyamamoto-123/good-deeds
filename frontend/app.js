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

  
function renderFilters(container){
    container.innerHTML = '';
    filters.forEach(f => {
      // wrapper for each grid item
      const item = document.createElement('div');
      item.className = 'filter-item';

      // button that holds only the icon
      const b = document.createElement('button');
      b.type = 'button';
      b.className = 'filter-btn';
      b.innerHTML = `<span class="material-icons">${f.icon}</span>`;
      b.onclick = () => toggleFilter(f.name, b);

      // label below the button (outside the button)
      const lbl = document.createElement('div');
      lbl.className = 'filter-label';
      lbl.textContent = f.name;

      item.appendChild(b);
      item.appendChild(lbl);
      container.appendChild(item);
    });
  }


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
      orgListMobile.appendChild(card.cloneNode(true));
    });

    foundCount.textContent = list.length;
    foundCountMobile.textContent = list.length;
  }

  function applyFilters(){
    const q = query.trim().toLowerCase();
    let list = orgs.filter(o =>
      o.name.toLowerCase().includes(q) ||
      o.address.toLowerCase().includes(q)
    );

    if(activeFilters.size){
      list = list.filter(o =>
        Array.from(activeFilters)
             .some(f => o.tags.map(t=>t.toLowerCase()).includes(f.toLowerCase()))
      );
    }

    renderOrgs(list);
  }

  // Search sync
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

  // Mobile panel
  panelHeader.addEventListener('click', ()=> mobilePanel.classList.toggle('open'));
  mobileOpenBtn?.addEventListener('click', ()=> mobilePanel.classList.add('open'));
  mobileCloseBtn?.addEventListener('click', ()=> mobilePanel.classList.remove('open'));

  // Initialize
  renderFilters(filtersEl);
  renderFilters(filtersMobileEl);
  applyFilters();
})();

/* --- AUTOCOMPLETE ГОРОДОВ --- */

const rosatomCities = [
  "Железногорск",
  "Саров",
  "Озерск",
  "Заречный",
  "Снежинск",
  "Нововоронеж",
  "Северск",
  "Трехгорный",
  "Зеленогорск",
  "Лесной",
  "Межгорье",
  "Полярные Зори"
];

const cityInput = document.getElementById("city-input");
const citySuggestions = document.getElementById("city-suggestions");
const selectedCity = document.getElementById("selected-city");

function updateCitySuggestions() {
  const query = cityInput.value.trim().toLowerCase();

  if (!query) {
    citySuggestions.style.display = "none";
    return;
  }

  const list = rosatomCities.filter(city =>
    city.toLowerCase().startsWith(query)
  );

  if (list.length === 0) {
    citySuggestions.style.display = "none";
    return;
  }

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

// события
cityInput.addEventListener("input", updateCitySuggestions);

// скрытие списка при клике вне
document.addEventListener("click", (e) => {
  if (!citySuggestions.contains(e.target) && e.target !== cityInput) {
    citySuggestions.style.display = "none";
  }
});
