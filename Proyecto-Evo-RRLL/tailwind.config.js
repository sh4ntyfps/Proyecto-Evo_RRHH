/** @type {import('tailwindcss').Config} */
// Tailwind convive con Bootstrap (grilla y JS de modales/menus de las 80 vistas):
// - prefijo "tw-" para que ninguna clase choque con las de Bootstrap (collapse, table, container...)
// - sin preflight: el reset de Bootstrap ya esta cargado
// - el modo oscuro sigue al mismo atributo que usa Bootstrap
module.exports = {
  prefix: 'tw-',
  content: ['./Views/**/*.cshtml', './wwwroot/js/**/*.js'],
  darkMode: ['selector', '[data-bs-theme="dark"]'],
  corePlugins: { preflight: false },
  theme: {
    extend: {
      colors: {
        marca: {
          50: '#eef4ff', 100: '#dce7ff', 200: '#bcd0ff', 300: '#8eaeff', 400: '#5b83ff',
          500: '#3461ff', 600: '#2447f0', 700: '#1d37cc', 800: '#1e31a3', 900: '#1e2f80', 950: '#151d4d',
        },
        tinta: { DEFAULT: '#0f172a', suave: '#475569' },
      },
      fontFamily: {
        sans: ['"Plus Jakarta Sans"', 'Inter', '"Segoe UI"', 'system-ui', 'sans-serif'],
      },
      boxShadow: {
        suave: '0 1px 2px rgba(15, 23, 42, .04), 0 4px 16px -4px rgba(15, 23, 42, .08)',
        elevada: '0 10px 30px -10px rgba(36, 71, 240, .35)',
        brillo: '0 0 0 1px rgba(255,255,255,.06), 0 8px 24px -8px rgba(52, 97, 255, .55)',
      },
      borderRadius: { '4xl': '2rem' },
      keyframes: {
        entrar: { '0%': { opacity: 0, transform: 'translateY(8px)' }, '100%': { opacity: 1, transform: 'none' } },
        flotar: { '0%,100%': { transform: 'translateY(0)' }, '50%': { transform: 'translateY(-8px)' } },
      },
      animation: {
        entrar: 'entrar .5s cubic-bezier(.2,.7,.2,1) both',
        flotar: 'flotar 6s ease-in-out infinite',
      },
    },
  },
  plugins: [],
};
