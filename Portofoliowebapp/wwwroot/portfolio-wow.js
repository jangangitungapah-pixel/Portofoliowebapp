(function () {
    function initPointerGlow() {
        if (window.__portfolioPointerGlowReady) return;

        window.__portfolioPointerGlowReady = true;

        window.addEventListener("pointermove", function (event) {
            document.documentElement.style.setProperty("--mouse-x", event.clientX + "px");
            document.documentElement.style.setProperty("--mouse-y", event.clientY + "px");
        });
    }

    function initAos() {
        if (!window.AOS) return;

        window.AOS.init({
            duration: 850,
            easing: "ease-out-cubic",
            once: true,
            offset: 70
        });

        setTimeout(function () {
            window.AOS.refresh();
        }, 350);
    }

    function initTypedText() {
        const target = document.querySelector("#typed-role");

        if (!target || !window.Typed) return;

        if (window.__portfolioTypedInstance) {
            window.__portfolioTypedInstance.destroy();
        }

        window.__portfolioTypedInstance = new window.Typed("#typed-role", {
            strings: [
                "AI Web App Builder",
                "Prompt-Driven Creator",
                "Landing Page Designer",
                "Automation Explorer",
                "No-Code+ Builder"
            ],
            typeSpeed: 46,
            backSpeed: 24,
            backDelay: 1150,
            startDelay: 250,
            loop: true,
            smartBackspace: true
        });
    }

    function initTiltCards() {
        if (!window.VanillaTilt) return;

        document.querySelectorAll("[data-tilt-card]").forEach(function (card) {
            if (card.vanillaTilt) return;

            window.VanillaTilt.init(card, {
                max: 7,
                speed: 650,
                glare: true,
                "max-glare": 0.18,
                scale: 1.015,
                perspective: 1100
            });
        });
    }

    function initMagneticButtons() {
        document.querySelectorAll("[data-magnetic]").forEach(function (button) {
            if (button.dataset.magneticReady === "true") return;

            button.dataset.magneticReady = "true";

            button.addEventListener("mousemove", function (event) {
                const rect = button.getBoundingClientRect();
                const x = event.clientX - rect.left - rect.width / 2;
                const y = event.clientY - rect.top - rect.height / 2;

                button.style.transform = `translate(${x * 0.12}px, ${y * 0.18}px)`;
            });

            button.addEventListener("mouseleave", function () {
                button.style.transform = "translate(0, 0)";
            });
        });
    }

    function initPortfolioWow() {
        initPointerGlow();
        initAos();
        initTypedText();
        initTiltCards();
        initMagneticButtons();
    }

    document.addEventListener("DOMContentLoaded", initPortfolioWow);
    document.addEventListener("enhancedload", initPortfolioWow);

    window.portfolioWow = {
        init: initPortfolioWow
    };
})();