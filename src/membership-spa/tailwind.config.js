module.exports = {
  purge: {
    mode: 'all',
    content: ["./src/**/*.{js,jsx,ts,tsx}", "./public/index.html"]
  },
  darkMode: false, // or 'media' or 'class'
  theme: {
    extend: {},
  },
  variants: {
    extend: {
      backgroundColor:['disabled']
    },
  },
  plugins: [
    require('@tailwindcss/forms')
  ],
}
