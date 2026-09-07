// EVO RRHH - utilidades del sitio

(function () {
    'use strict';

    document.addEventListener('DOMContentLoaded', function () {
        initTheme();
        initSearch();
        initBackToTop();
        initCookieBanner();
        initPasswordToggle();
        initCopyButtons();
        initAutoAlerts();
    });

    /* ---------- Modo oscuro ---------- */
    function initTheme() {
        var STORE = 'evo-theme';
        var root = document.documentElement;
        var toggle = document.getElementById('themeToggle');
        if (!toggle) return;

        var saved = localStorage.getItem(STORE);
        var prefiereOscuro = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
        var tema = saved || (prefiereOscuro ? 'dark' : 'light');
        aplicar(tema);

        toggle.addEventListener('click', function () {
            aplicar(root.getAttribute('data-bs-theme') === 'dark' ? 'light' : 'dark');
        });

        function aplicar(t) {
            root.setAttribute('data-bs-theme', t);
            localStorage.setItem(STORE, t);
            var icono = toggle.querySelector('svg use');
            if (icono) icono.setAttribute('href', t === 'dark' ? '#i-sun' : '#i-moon');
        }
    }

    /* ---------- Buscador del sitio ---------- */
    function initSearch() {
        var input = document.getElementById('siteSearch');
        if (!input) return;
        input.addEventListener('input', function () {
            var termino = input.value.trim().toLowerCase();
            document.querySelectorAll('[data-search-target]').forEach(function (target) {
                var esTabla = target.matches('table');
                if (esTabla) {
                    var filas = target.querySelectorAll('tbody tr');
                    filas.forEach(function (fila) {
                        var texto = fila.textContent.toLowerCase();
                        fila.style.display = texto.indexOf(termino) === -1 ? 'none' : '';
                    });
                    // Mostrar u ocultar aviso de "sin resultados"
                    var tbody = target.querySelector('tbody');
                    var aviso = target.querySelector('.search-no-results');
                    var visibles = Array.prototype.filter.call(filas, function (f) { return f.style.display !== 'none'; }).length;
                    if (aviso) aviso.style.display = (esTabla && filas.length > 0 && visibles === 0) ? '' : 'none';
                } else {
                    // Filtra tarjetas/registros que tengan data-search-target
                    var items = target.children || [];
                    var total = 0;
                    for (var i = 0; i < items.length; i++) {
                        if (!items[i].matches || !items[i].matches('[data-search-item]')) continue;
                        total++;
                        var txt = items[i].textContent.toLowerCase();
                        items[i].style.display = txt.indexOf(termino) === -1 ? 'none' : '';
                    }
                }
            });
        });
    }

    /* ---------- Botón volver arriba ---------- */
    function initBackToTop() {
        var btn = document.getElementById('backToTop');
        if (!btn) return;

        function onScroll() {
            if (window.scrollY > 300) btn.classList.add('show');
            else btn.classList.remove('show');
        }
        window.addEventListener('scroll', onScroll, { passive: true });
        onScroll();
        btn.addEventListener('click', function () {
            window.scrollTo({ top: 0, behavior: 'smooth' });
        });
    }

    /* ---------- Banner de cookies ---------- */
    function initCookieBanner() {
        var banner = document.getElementById('cookieBanner');
        var aceptar = document.getElementById('cookieAccept');
        if (!banner) return;
        if (localStorage.getItem('evo-cookies') === 'aceptadas') return;
        banner.classList.add('show');
        if (aceptar) aceptar.addEventListener('click', function () {
            localStorage.setItem('evo-cookies', 'aceptadas');
            banner.classList.remove('show');
        });
    }

    /* ---------- Mostrar/ocultar contraseña ---------- */
    function initPasswordToggle() {
        document.querySelectorAll('[data-toggle-password]').forEach(function (btn) {
            btn.addEventListener('click', function () {
                var input = document.querySelector(btn.getAttribute('data-toggle-password'));
                if (!input) return;
                var visible = input.type === 'text';
                input.type = visible ? 'password' : 'text';
                btn.textContent = visible ? 'Mostrar' : 'Ocultar';
            });
        });
    }

    /* ---------- Botón copiar ---------- */
    function initCopyButtons() {
        if (!navigator.clipboard || !window.isSecureContext) return;

        document.querySelectorAll('[data-copy-target]').forEach(function (btn) {
            btn.addEventListener('click', function () {
                var origen = document.querySelector(btn.getAttribute('data-copy-target'));
                if (!origen) return;
                var texto = origen.tagName === 'INPUT' || origen.tagName === 'TEXTAREA' ? origen.value : origen.textContent;
                navigator.clipboard.writeText(texto.trim()).then(function () {
                    var original = btn.textContent;
                    btn.textContent = '\u2713 Copiado';
                    setTimeout(function () { btn.textContent = original; }, 1500);
                });
            });
        });
    }

    /* ---------- Cerrar alerts automáticamente ---------- */
    function initAutoAlerts() {
        document.querySelectorAll('.alert-auto').forEach(function (alerta) {
            setTimeout(function () {
                if (alerta.parentNode) {
                    alerta.classList.add('fade');
                    setTimeout(function () { if (alerta.parentNode) alerta.remove(); }, 300);
                }
            }, 5000);
        });
    }

    /* ---------- Spinner de carga (uso: window.evoShowLoading()) ---------- */
    window.evoShowLoading = function (visible) {
        var spinner = document.getElementById('loadingSpinner');
        if (spinner) spinner.classList.toggle('show', visible);
    };
})();