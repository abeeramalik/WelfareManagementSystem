(function () {
    'use strict';

    function qs(sel) { return document.querySelector(sel); }
    function qsa(sel) { return Array.from(document.querySelectorAll(sel)); }

    // ==================== TAB NAVIGATION ====================
    window.showSection = function (evt, sectionName) {
        if (evt && evt.preventDefault) {
            evt.preventDefault();
        }

        // Update nav links
        if (evt && evt.currentTarget) {
            qsa('.nav-link').forEach(n => n.classList.remove('active'));
            evt.currentTarget.classList.add('active');
        }

        // Switch content cards
        qsa('.content-card').forEach(c => c.classList.remove('active'));
        const target = document.getElementById(sectionName);
        if (target) target.classList.add('active');
    };

    // ==================== FOOD DONATION ====================
    function initFoodDonation() {
        const peopleButtons = qsa('.people-btn');
        const peopleInput = qs('#numberOfPeople');
        const peopleError = qs('#peopleError');
        const foodForm = qs('#foodForm');

        // Handle people button clicks
        peopleButtons.forEach(btn => {
            btn.addEventListener('click', function () {
                const value = this.getAttribute('data-value') || this.textContent.trim();

                // Remove active from all
                peopleButtons.forEach(b => {
                    b.classList.remove('active');
                    b.classList.remove('selected');
                });

                // Add active to clicked
                this.classList.add('active');
                this.classList.add('selected');

                // Set value
                if (peopleInput) {
                    peopleInput.value = value;
                }

                // Hide error
                if (peopleError) {
                    peopleError.style.display = 'none';
                }
            });
        });

        // Form validation
        if (foodForm) {
            foodForm.addEventListener('submit', function (e) {
                const people = peopleInput ? peopleInput.value : '0';
                const dt = qs('#pickupDateTime') ? qs('#pickupDateTime').value : '';

                if (!people || people === '0') {
                    e.preventDefault();
                    if (peopleError) peopleError.style.display = 'block';
                    alert('Please select number of people');
                    return false;
                }

                if (!dt) {
                    e.preventDefault();
                    alert('Please select pickup date and time');
                    return false;
                }

                if (peopleError) peopleError.style.display = 'none';
                return true;
            });
        }
    }

    // ==================== CLOTHES DONATION ====================
    function initClothesDonation() {
        const categoryCards = qsa('.category-card');
        const categoryInput = qs('#clothesCategory');
        const categoryError = qs('#categoryError');
        const quantityInput = qs('#clothesQuantity');
        const minusBtn = qs('.qty-btn-minus');
        const plusBtn = qs('.qty-btn-plus');
        const clothesForm = qs('#clothesForm');

        // Handle category selection
        categoryCards.forEach(card => {
            card.addEventListener('click', function () {
                const category = this.getAttribute('data-category');

                // Remove active from all
                categoryCards.forEach(c => {
                    c.classList.remove('active');
                    c.classList.remove('selected');
                });

                // Add active to clicked
                this.classList.add('active');
                this.classList.add('selected');

                // Set value
                if (categoryInput) {
                    categoryInput.value = category;
                }

                // Hide error
                if (categoryError) {
                    categoryError.style.display = 'none';
                }
            });
        });

        // Handle quantity buttons
        if (minusBtn && quantityInput) {
            minusBtn.addEventListener('click', function () {
                const current = parseInt(quantityInput.value) || 1;
                quantityInput.value = Math.max(1, current - 1);
            });
        }

        if (plusBtn && quantityInput) {
            plusBtn.addEventListener('click', function () {
                const current = parseInt(quantityInput.value) || 1;
                quantityInput.value = current + 1;
            });
        }

        // Form validation
        if (clothesForm) {
            clothesForm.addEventListener('submit', function (e) {
                const category = categoryInput ? categoryInput.value : '';
                const clothType = qs('#clothesType') ? qs('#clothesType').value : '';
                const location = qs('#pickupLocation') ? qs('#pickupLocation').value : '';
                const datetime = qs('#clothesPickupDateTime') ? qs('#clothesPickupDateTime').value : '';

                if (!category) {
                    e.preventDefault();
                    if (categoryError) categoryError.style.display = 'block';
                    alert('Please select a clothes category');
                    return false;
                }

                if (!clothType) {
                    e.preventDefault();
                    alert('Please select cloth type');
                    return false;
                }

                if (!location) {
                    e.preventDefault();
                    alert('Please enter pickup location');
                    return false;
                }

                if (!datetime) {
                    e.preventDefault();
                    alert('Please select pickup date and time');
                    return false;
                }

                if (categoryError) categoryError.style.display = 'none';
                return true;
            });
        }
    }

    // ==================== MONEY DONATION ====================
    function initMoneyDonation() {
        const moneyForm = qs('#moneyForm');
        const amountInput = qs('#moneyAmount');
        const amountError = qs('#amountError');

        if (moneyForm) {
            moneyForm.addEventListener('submit', function (e) {
                const amount = amountInput ? parseFloat(amountInput.value) : 0;

                if (!amount || amount < 100) {
                    e.preventDefault();
                    if (amountError) amountError.style.display = 'block';
                    alert('Minimum donation amount is PKR 100');
                    return false;
                }

                if (amountError) amountError.style.display = 'none';
                return true;
            });
        }
    }

    // ==================== HOME TAB QUICK ACTIONS ====================
    function initHomeQuickActions() {
        // Wire up action cards on home tab to navigate to donation forms
        const actionCards = qsa('.action-card');

        actionCards.forEach(card => {
            card.addEventListener('click', function () {
                let sectionName = '';

                if (this.classList.contains('card-food')) {
                    sectionName = 'food';
                } else if (this.classList.contains('card-clothes')) {
                    sectionName = 'clothes';
                } else if (this.classList.contains('card-money')) {
                    sectionName = 'money';
                }

                if (sectionName) {
                    // Find the corresponding nav link and activate it
                    const navLink = qs(`.nav-link[onclick*="${sectionName}"]`);
                    if (navLink) {
                        qsa('.nav-link').forEach(n => n.classList.remove('active'));
                        navLink.classList.add('active');
                    }

                    // Show the section
                    qsa('.content-card').forEach(c => c.classList.remove('active'));
                    const target = document.getElementById(sectionName);
                    if (target) target.classList.add('active');
                }
            });
        });
    }

    // ==================== SUCCESS ANIMATION (BLUE THEME) ====================
    function createConfetti() {
        // Blue theme confetti colors
        const colors = ['#667eea', '#764ba2', '#98a0f8', '#8a6ab8', '#7f8ff1'];
        for (let i = 0; i < 50; i++) {
            const c = document.createElement('div');
            c.style.position = 'fixed';
            c.style.left = (Math.random() * 100) + '%';
            c.style.top = '-10px';
            c.style.width = '8px';
            c.style.height = '12px';
            c.style.background = colors[Math.floor(Math.random() * colors.length)];
            c.style.transform = 'rotate(' + (Math.random() * 360) + 'deg)';
            c.style.opacity = '0.95';
            c.style.zIndex = '9999';
            c.style.borderRadius = '2px';
            c.style.pointerEvents = 'none';

            // Animate with CSS
            const duration = 2 + Math.random() * 2;
            const delay = Math.random() * 0.5;
            c.style.animation = `fall ${duration}s linear ${delay}s forwards`;

            document.body.appendChild(c);
            setTimeout(() => c.remove(), (duration + delay) * 1000 + 100);
        }
    }

    window.showSuccessAnimation = function (type) {
        // Create overlay
        const overlay = document.createElement('div');
        overlay.style.position = 'fixed';
        overlay.style.inset = '0';
        overlay.style.background = 'rgba(0,0,0,0.7)';
        overlay.style.zIndex = '9998';
        overlay.style.backdropFilter = 'blur(4px)';
        document.body.appendChild(overlay);

        // Create success box with BLUE THEME
        const box = document.createElement('div');
        box.style.position = 'fixed';
        box.style.left = '50%';
        box.style.top = '50%';
        box.style.transform = 'translate(-50%,-50%) scale(0.8)';
        box.style.zIndex = '9999';
        box.style.background = 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)';
        box.style.padding = '50px';
        box.style.borderRadius = '20px';
        box.style.border = '2px solid rgba(255, 255, 255, 0.3)';
        box.style.boxShadow = '0 20px 60px rgba(102, 126, 234, 0.5)';
        box.style.textAlign = 'center';
        box.style.minWidth = '350px';
        box.style.animation = 'popIn 0.3s ease-out forwards';

        const emoji = type === 'food' ? '🍲' : type === 'clothes' ? '👕' : '💰';

        box.innerHTML = `
            <div style="font-size:80px;margin-bottom:20px;animation: bounce 0.6s ease-in-out infinite;">${emoji}</div>
            <h2 style="color:#ffffff;margin-bottom:15px;font-size:28px;font-weight:600;text-shadow: 0 2px 10px rgba(0,0,0,0.2);">Donation Successful!</h2>
            <p style="color:#ffffff;font-size:18px;line-height:1.6;opacity:0.95;">Thank you for your generous <strong style="color:#fff;text-shadow: 0 1px 5px rgba(0,0,0,0.2);">${type}</strong> donation.<br>You're making a real difference! ❤️</p>
        `;
        document.body.appendChild(box);

        // Create confetti
        setTimeout(() => createConfetti(), 100);

        // Remove after 3 seconds
        setTimeout(() => {
            box.style.animation = 'popOut 0.3s ease-in forwards';
            setTimeout(() => {
                box.remove();
                overlay.remove();
            }, 300);
        }, 3000);
    };

    // ==================== INITIALIZATION ====================
    document.addEventListener('DOMContentLoaded', function () {
        // Set default active nav if none set
        if (!qs('.nav-link.active')) {
            const firstNav = qs('.nav-link');
            if (firstNav) firstNav.classList.add('active');
        }

        // Initialize all donation forms
        initFoodDonation();
        initClothesDonation();
        initMoneyDonation();
        initHomeQuickActions();

        // Add CSS animations if not already present
        if (!document.getElementById('donor-animations')) {
            const style = document.createElement('style');
            style.id = 'donor-animations';
            style.textContent = `
                @keyframes fall {
                    to {
                        transform: translateY(100vh) rotate(720deg);
                        opacity: 0;
                    }
                }
                @keyframes popIn {
                    to {
                        transform: translate(-50%,-50%) scale(1);
                    }
                }
                @keyframes popOut {
                    to {
                        transform: translate(-50%,-50%) scale(0.8);
                        opacity: 0;
                    }
                }
                @keyframes bounce {
                    0%, 100% { transform: translateY(0); }
                    50% { transform: translateY(-10px); }
                }
            `;
            document.head.appendChild(style);
        }
    });

})();