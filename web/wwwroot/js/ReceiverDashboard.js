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

    // ==================== FOOD REQUEST ====================
    function initFoodRequest() {
        const foodForm = qs('#foodRequestForm');
        const familyInput = qs('#familyMembers');
        const quantityInput = qs('#foodQuantity');
        const minusBtn = qs('.food-qty-btn-minus');
        const plusBtn = qs('.food-qty-btn-plus');

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
        if (foodForm) {
            foodForm.addEventListener('submit', function (e) {
                const family = familyInput ? parseInt(familyInput.value) : 0;
                const quantity = quantityInput ? parseInt(quantityInput.value) : 0;

                if (!family || family < 1) {
                    e.preventDefault();
                    alert('Please enter number of family members');
                    return false;
                }

                if (!quantity || quantity < 1) {
                    e.preventDefault();
                    alert('Please enter food quantity');
                    return false;
                }

                return true;
            });
        }
    }

    // ==================== CLOTHES REQUEST ====================
    function initClothesRequest() {
        const clothesForm = qs('#clothesRequestForm');
        const maleInput = qs('#maleClothes');
        const femaleInput = qs('#femaleClothes');
        const kidsInput = qs('#kidsClothes');
        const clothesTypeInput = qs('#clothesType');

        // Quantity buttons for male
        const maleMinus = qs('.male-qty-minus');
        const malePlus = qs('.male-qty-plus');
        if (maleMinus && maleInput) {
            maleMinus.addEventListener('click', () => {
                const val = parseInt(maleInput.value) || 0;
                maleInput.value = Math.max(0, val - 1);
            });
        }
        if (malePlus && maleInput) {
            malePlus.addEventListener('click', () => {
                const val = parseInt(maleInput.value) || 0;
                maleInput.value = val + 1;
            });
        }

        // Quantity buttons for female
        const femaleMinus = qs('.female-qty-minus');
        const femalePlus = qs('.female-qty-plus');
        if (femaleMinus && femaleInput) {
            femaleMinus.addEventListener('click', () => {
                const val = parseInt(femaleInput.value) || 0;
                femaleInput.value = Math.max(0, val - 1);
            });
        }
        if (femalePlus && femaleInput) {
            femalePlus.addEventListener('click', () => {
                const val = parseInt(femaleInput.value) || 0;
                femaleInput.value = val + 1;
            });
        }

        // Quantity buttons for kids
        const kidsMinus = qs('.kids-qty-minus');
        const kidsPlus = qs('.kids-qty-plus');
        if (kidsMinus && kidsInput) {
            kidsMinus.addEventListener('click', () => {
                const val = parseInt(kidsInput.value) || 0;
                kidsInput.value = Math.max(0, val - 1);
            });
        }
        if (kidsPlus && kidsInput) {
            kidsPlus.addEventListener('click', () => {
                const val = parseInt(kidsInput.value) || 0;
                kidsInput.value = val + 1;
            });
        }

        // Form validation
        if (clothesForm) {
            clothesForm.addEventListener('submit', function (e) {
                const male = parseInt(maleInput?.value) || 0;
                const female = parseInt(femaleInput?.value) || 0;
                const kids = parseInt(kidsInput?.value) || 0;
                const type = clothesTypeInput?.value;

                const total = male + female + kids;

                if (total === 0) {
                    e.preventDefault();
                    alert('Please specify at least one clothing item');
                    return false;
                }

                if (!type) {
                    e.preventDefault();
                    alert('Please select clothes type');
                    return false;
                }

                return true;
            });
        }
    }

    // ==================== LOAN REQUEST ====================
    function initLoanRequest() {
        const loanForm = qs('#loanRequestForm');
        const amountInput = qs('#loanAmount');
        const purposeInput = qs('#loanPurpose');
        const repaymentInput = qs('#repaymentMonths');

        if (loanForm) {
            loanForm.addEventListener('submit', function (e) {
                const amount = parseFloat(amountInput?.value) || 0;
                const purpose = purposeInput?.value;
                const repayment = parseInt(repaymentInput?.value) || 0;

                if (amount < 100) {
                    e.preventDefault();
                    alert('Minimum loan amount is PKR 100');
                    return false;
                }

                if (!purpose) {
                    e.preventDefault();
                    alert('Please specify loan purpose');
                    return false;
                }

                if (repayment < 1) {
                    e.preventDefault();
                    alert('Please specify repayment months');
                    return false;
                }

                return true;
            });
        }
    }

    // ==================== SHELTER REQUEST ====================
    function initShelterRequest() {
        const shelterForm = qs('#shelterRequestForm');
        const durationInput = qs('#shelterDuration');
        const roomsInput = qs('#requiredRooms');

        if (shelterForm) {
            shelterForm.addEventListener('submit', function (e) {
                const duration = parseInt(durationInput?.value) || 0;
                const rooms = parseInt(roomsInput?.value) || 0;

                if (duration < 1) {
                    e.preventDefault();
                    alert('Please specify duration in days');
                    return false;
                }

                if (rooms < 1) {
                    e.preventDefault();
                    alert('Please specify required rooms');
                    return false;
                }

                return true;
            });
        }
    }

    // ==================== HOME TAB QUICK ACTIONS ====================
    function initHomeQuickActions() {
        const actionCards = qsa('.action-card');

        actionCards.forEach(card => {
            card.addEventListener('click', function () {
                let sectionName = '';

                if (this.classList.contains('card-food')) {
                    sectionName = 'food';
                } else if (this.classList.contains('card-clothes')) {
                    sectionName = 'clothes';
                } else if (this.classList.contains('card-loan')) {
                    sectionName = 'loan';
                } else if (this.classList.contains('card-shelter')) {
                    sectionName = 'shelter';
                }

                if (sectionName) {
                    const navLink = qs(`.nav-link[onclick*="${sectionName}"]`);
                    if (navLink) {
                        qsa('.nav-link').forEach(n => n.classList.remove('active'));
                        navLink.classList.add('active');
                    }

                    qsa('.content-card').forEach(c => c.classList.remove('active'));
                    const target = document.getElementById(sectionName);
                    if (target) target.classList.add('active');
                }
            });
        });
    }

    // ==================== SUCCESS ANIMATION (GREEN THEME) ====================
    function createConfetti() {
        const colors = ['#43A047', '#2E7D32', '#66BB6A', '#81C784', '#4CAF50'];
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

            const duration = 2 + Math.random() * 2;
            const delay = Math.random() * 0.5;
            c.style.animation = `fall ${duration}s linear ${delay}s forwards`;

            document.body.appendChild(c);
            setTimeout(() => c.remove(), (duration + delay) * 1000 + 100);
        }
    }

    window.showSuccessAnimation = function (type) {
        const overlay = document.createElement('div');
        overlay.style.position = 'fixed';
        overlay.style.inset = '0';
        overlay.style.background = 'rgba(0,0,0,0.7)';
        overlay.style.zIndex = '9998';
        overlay.style.backdropFilter = 'blur(4px)';
        document.body.appendChild(overlay);

        const box = document.createElement('div');
        box.style.position = 'fixed';
        box.style.left = '50%';
        box.style.top = '50%';
        box.style.transform = 'translate(-50%,-50%) scale(0.8)';
        box.style.zIndex = '9999';
        box.style.background = 'linear-gradient(135deg, #43A047 0%, #2E7D32 100%)';
        box.style.padding = '50px';
        box.style.borderRadius = '20px';
        box.style.border = '2px solid rgba(255, 255, 255, 0.3)';
        box.style.boxShadow = '0 20px 60px rgba(67, 160, 71, 0.5)';
        box.style.textAlign = 'center';
        box.style.minWidth = '350px';
        box.style.animation = 'popIn 0.3s ease-out forwards';

        const emoji = type === 'food' ? '🍲' : type === 'clothes' ? '👕' : type === 'loan' ? '💰' : '🏠';

        box.innerHTML = `
            <div style="font-size:80px;margin-bottom:20px;animation: bounce 0.6s ease-in-out infinite;">${emoji}</div>
            <h2 style="color:#ffffff;margin-bottom:15px;font-size:28px;font-weight:600;text-shadow: 0 2px 10px rgba(0,0,0,0.2);">Request Submitted!</h2>
            <p style="color:#ffffff;font-size:18px;line-height:1.6;opacity:0.95;">Your <strong style="color:#fff;text-shadow: 0 1px 5px rgba(0,0,0,0.2);">${type}</strong> request has been submitted successfully.<br>We'll review it shortly! 🙏</p>
        `;
        document.body.appendChild(box);

        setTimeout(() => createConfetti(), 100);

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
        if (!qs('.nav-link.active')) {
            const firstNav = qs('.nav-link');
            if (firstNav) firstNav.classList.add('active');
        }

        initFoodRequest();
        initClothesRequest();
        initLoanRequest();
        initShelterRequest();
        initHomeQuickActions();

        if (!document.getElementById('receiver-animations')) {
            const style = document.createElement('style');
            style.id = 'receiver-animations';
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