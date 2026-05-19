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

    window.portfolioPrintInvoice = function () {
        setTimeout(function () {
            window.print();
        }, 150);

        return true;
    };
})();
(function () {
    function initScrollProgress() {
        let bar = document.querySelector(".scroll-progress-bar");

        if (!bar) {
            bar = document.createElement("div");
            bar.className = "scroll-progress-bar";
            document.body.prepend(bar);
        }

        if (window.__landingScrollProgressReady) return;
        window.__landingScrollProgressReady = true;

        let ticking = false;

        function updateProgress() {
            const scrollTop = window.scrollY || document.documentElement.scrollTop;
            const docHeight = document.documentElement.scrollHeight - window.innerHeight;
            const progress = docHeight <= 0 ? 0 : Math.min(scrollTop / docHeight, 1);

            document.documentElement.style.setProperty("--scroll-progress", progress.toString());
            ticking = false;
        }

        window.addEventListener("scroll", function () {
            if (ticking) return;

            ticking = true;
            window.requestAnimationFrame(updateProgress);
        }, { passive: true });

        updateProgress();
    }

    function initSpotlightCards() {
        const selector = [
            ".glass-card",
            ".service-card",
            ".project-card",
            ".timeline-item",
            ".demo-cta-card",
            ".app-window",
            ".creator-console",
            ".contact-section",
            ".hero-stats-youth div",
            ".demo-tab"
        ].join(",");

        document.querySelectorAll(selector).forEach(function (card) {
            if (card.dataset.spotlightReady === "true") return;

            card.dataset.spotlightReady = "true";

            card.addEventListener("pointermove", function (event) {
                const rect = card.getBoundingClientRect();
                const x = event.clientX - rect.left;
                const y = event.clientY - rect.top;

                card.style.setProperty("--spot-x", x + "px");
                card.style.setProperty("--spot-y", y + "px");
            });

            card.addEventListener("pointerleave", function () {
                card.style.setProperty("--spot-x", "50%");
                card.style.setProperty("--spot-y", "50%");
            });
        });
    }

    function initActiveNav() {
        if (window.__landingActiveNavObserver) {
            window.__landingActiveNavObserver.disconnect();
        }

        const sections = Array.from(document.querySelectorAll("section[id]"));
        const navLinks = Array.from(document.querySelectorAll(".nav-links a"));

        if (!sections.length || !navLinks.length) return;

        function setActive(id) {
            navLinks.forEach(function (link) {
                const href = link.getAttribute("href") || "";
                const isActive = href === "/#" + id || href === "#" + id;

                link.classList.toggle("active", isActive);
            });
        }

        const observer = new IntersectionObserver(function (entries) {
            const visible = entries
                .filter(function (entry) { return entry.isIntersecting; })
                .sort(function (a, b) { return b.intersectionRatio - a.intersectionRatio; })[0];

            if (visible && visible.target.id) {
                setActive(visible.target.id);
            }
        }, {
            root: null,
            threshold: [0.22, 0.35, 0.5],
            rootMargin: "-20% 0px -55% 0px"
        });

        sections.forEach(function (section) {
            observer.observe(section);
        });

        window.__landingActiveNavObserver = observer;
    }

    function initLandingPolish() {
        initScrollProgress();
        initSpotlightCards();
        initActiveNav();
    }

    document.addEventListener("DOMContentLoaded", initLandingPolish);
    document.addEventListener("enhancedload", initLandingPolish);

    window.landingPolish = {
        init: initLandingPolish
    };
})();