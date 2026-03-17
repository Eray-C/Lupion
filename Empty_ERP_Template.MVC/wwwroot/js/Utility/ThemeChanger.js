(function () {
  
    const themeClass = localStorage.getItem("theme-class");
    const dxTheme = localStorage.getItem("dx-theme");

    if (themeClass) {
        document.documentElement.className = themeClass;
    }
    if (dxTheme) {
        const themes = ["generic.dark", "generic.light"];

        themes.forEach(theme => {
            const isActive = (theme === dxTheme);
            const link = document.querySelector(`link[data-theme="${theme}"]`);
            if (link) {
                link.setAttribute('data-active', isActive);
            }
        });
    }

})();