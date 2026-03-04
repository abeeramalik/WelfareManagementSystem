
(function () {
    'use strict';

    console.log('🚀 NGODashboard.js loaded successfully');

    function qs(sel) { return document.querySelector(sel); }
    function qsa(sel) { return Array.from(document.querySelectorAll(sel)); }

    // ==================== VALIDATION ====================
    function validateEmail(email) {
        return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
    }

    function validatePhone(phone) {
        return /^(\+92|0)?[0-9]{10,11}$/.test(phone.replace(/[\s\-]/g, ''));
    }

    function validatePassword(password) {
        return password && password.length >= 6;
    }

    // ==================== TAB MANAGEMENT ====================
    function saveCurrentTab() {
        const activeTab = qs('.content-card.active');
        if (activeTab) {
            sessionStorage.setItem('ngoActiveTab', activeTab.id);
        }
    }

    function restoreActiveTab() {
        const savedTab = sessionStorage.getItem('ngoActiveTab');
        if (savedTab) {
            qsa('.content-card').forEach(c => c.classList.remove('active'));
            qsa('.nav-link').forEach(n => n.classList.remove('active'));
            const target = document.getElementById(savedTab);
            if (target) {
                target.classList.add('active');
                const navLink = qs(`.nav-link[onclick*="${savedTab}"]`);
                if (navLink) navLink.classList.add('active');
            }
        }
    }

    window.showSection = function (evt, sectionName) {
        if (evt?.preventDefault) evt.preventDefault();
        if (evt?.currentTarget) {
            qsa('.nav-link').forEach(n => n.classList.remove('active'));
            evt.currentTarget.classList.add('active');
        }
        qsa('.content-card').forEach(c => c.classList.remove('active'));
        const target = document.getElementById(sectionName);
        if (target) {
            target.classList.add('active');
            sessionStorage.setItem('ngoActiveTab', sectionName);
        }
    };

    // ==================== ANIMATIONS ====================
    function createConfetti(colors) {
        const defaultColors = ['#667eea', '#764ba2', '#98a0f8', '#8a6ab8', '#7f8ff1', '#28a745'];
        const confettiColors = colors || defaultColors;

        for (let i = 0; i < 50; i++) {
            const c = document.createElement('div');
            Object.assign(c.style, {
                position: 'fixed',
                left: (Math.random() * 100) + '%',
                top: '-10px',
                width: '8px',
                height: '12px',
                background: confettiColors[Math.floor(Math.random() * confettiColors.length)],
                transform: 'rotate(' + (Math.random() * 360) + 'deg)',
                opacity: '0.95',
                zIndex: '99999',
                borderRadius: '2px',
                pointerEvents: 'none',
                animation: `fall ${2 + Math.random() * 2}s linear ${Math.random() * 0.5}s forwards`
            });
            document.body.appendChild(c);
            setTimeout(() => c.remove(), 5000);
        }
    }

    window.showActionSuccessAnimation = function (emoji, title, message, actionType = 'success') {
        console.log('🎬 Showing animation:', { emoji, title, message, actionType });

        const colors = {
            error: {
                gradient: 'linear-gradient(135deg, #dc3545 0%, #c82333 100%)',
                confetti: ['#dc3545', '#c82333', '#ff6b6b', '#e74c3c']
            },
            reject: {
                gradient: 'linear-gradient(135deg, #ff6b6b 0%, #ff8e53 100%)',
                confetti: ['#ff6b6b', '#ff8e53', '#ff9a76', '#ffa94d']
            },
            success: {
                gradient: 'linear-gradient(135deg, #28a745 0%, #218838 100%)',
                confetti: ['#28a745', '#218838', '#48c774', '#51cf66']
            },
            donate: {
                gradient: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
                confetti: ['#667eea', '#764ba2', '#98a0f8', '#8a6ab8']
            },
            fulfill: {
                gradient: 'linear-gradient(135deg, #43e97b 0%, #38f9d7 100%)',
                confetti: ['#43e97b', '#38f9d7', '#51cf66', '#20c997']
            },
            accept: {
                gradient: 'linear-gradient(135deg, #4facfe 0%, #00f2fe 100%)',
                confetti: ['#4facfe', '#00f2fe', '#4fc3f7', '#29b6f6']
            }
        };
        const theme = colors[actionType] || colors.success;

        const overlay = document.createElement('div');
        Object.assign(overlay.style, {
            position: 'fixed',
            inset: '0',
            background: 'rgba(0,0,0,0.7)',
            zIndex: '99998',
            backdropFilter: 'blur(4px)'
        });

        const box = document.createElement('div');
        Object.assign(box.style, {
            position: 'fixed',
            left: '50%',
            top: '50%',
            transform: 'translate(-50%,-50%) scale(0.8)',
            zIndex: '99999',
            background: theme.gradient,
            padding: '50px',
            borderRadius: '20px',
            textAlign: 'center',
            minWidth: '350px',
            animation: 'popIn 0.3s ease-out forwards',
            boxShadow: '0 25px 70px rgba(0,0,0,0.5)'
        });

        box.innerHTML = `
            <div style="font-size: 64px; margin-bottom: 20px; animation: bounce 0.6s ease-in-out;">${emoji}</div>
            <h2 style="color: white; margin: 0 0 15px 0; font-size: 28px; font-weight: 800; text-shadow: 0 2px 10px rgba(0,0,0,0.3);">${title}</h2>
            <p style="color: rgba(255,255,255,0.95); font-size: 16px; font-weight: 500;">${message}</p>
        `;

        document.body.appendChild(overlay);
        document.body.appendChild(box);
        setTimeout(() => createConfetti(theme.confetti), 100);

        setTimeout(() => {
            box.style.animation = 'popOut 0.3s ease-in forwards';
            setTimeout(() => {
                box.remove();
                overlay.remove();
            }, 300);
        }, 2500);
    };

    // ==================== MODALS ====================
    function createModalOverlay() {
        const overlay = document.createElement('div');
        overlay.className = 'custom-modal-overlay';
        return overlay;
    }

    function createModalBox(title, message) {
        const modal = document.createElement('div');
        modal.className = 'custom-modal-box';
        modal.innerHTML = `
            <h3 class="modal-title">${title}</h3>
            <p class="modal-message">${message}</p>
        `;
        return modal;
    }

    function closeModalBox(overlay, modal) {
        overlay.style.opacity = '0';
        modal.style.opacity = '0';
        setTimeout(() => {
            overlay.remove();
            modal.remove();
        }, 300);
    }

    window.showAlertModal = function (title, message) {
        const overlay = createModalOverlay();
        const modal = createModalBox(title, message);

        const btn = document.createElement('button');
        btn.className = 'modal-btn modal-btn-primary';
        btn.textContent = 'OK';
        btn.onclick = () => closeModalBox(overlay, modal);

        const container = document.createElement('div');
        container.className = 'modal-btn-container';
        container.appendChild(btn);
        modal.appendChild(container);

        document.body.appendChild(overlay);
        document.body.appendChild(modal);

        setTimeout(() => {
            overlay.style.opacity = '1';
            modal.style.opacity = '1';
            modal.style.transform = 'translate(-50%, -50%) scale(1)';
        }, 10);
    };

    window.showConfirmModal = function (title, message, onConfirm) {
        const overlay = createModalOverlay();
        const modal = createModalBox(title, message);
        const container = document.createElement('div');
        container.className = 'modal-btn-container';

        const cancelBtn = document.createElement('button');
        cancelBtn.className = 'modal-btn modal-btn-secondary';
        cancelBtn.textContent = 'Cancel';
        cancelBtn.onclick = () => closeModalBox(overlay, modal);

        const confirmBtn = document.createElement('button');
        confirmBtn.className = 'modal-btn modal-btn-primary';
        confirmBtn.textContent = 'Confirm';
        confirmBtn.onclick = () => {
            closeModalBox(overlay, modal);
            if (onConfirm) onConfirm();
        };

        container.appendChild(cancelBtn);
        container.appendChild(confirmBtn);
        modal.appendChild(container);

        document.body.appendChild(overlay);
        document.body.appendChild(modal);

        setTimeout(() => {
            overlay.style.opacity = '1';
            modal.style.opacity = '1';
            modal.style.transform = 'translate(-50%, -50%) scale(1)';
        }, 10);
    };

    window.showInputModal = function (title, message, placeholder, onSubmit) {
        const overlay = createModalOverlay();
        const modal = createModalBox(title, message);

        const input = document.createElement('textarea');
        input.className = 'modal-input';
        input.placeholder = placeholder;
        input.rows = 4;
        input.style.marginTop = '20px';
        modal.appendChild(input);

        const container = document.createElement('div');
        container.className = 'modal-btn-container';
        container.style.marginTop = '20px';

        const cancelBtn = document.createElement('button');
        cancelBtn.className = 'modal-btn modal-btn-secondary';
        cancelBtn.textContent = 'Cancel';
        cancelBtn.onclick = () => closeModalBox(overlay, modal);

        const submitBtn = document.createElement('button');
        submitBtn.className = 'modal-btn modal-btn-primary';
        submitBtn.textContent = 'Submit';
        submitBtn.onclick = () => {
            const val = input.value.trim();
            closeModalBox(overlay, modal);
            if (onSubmit) onSubmit(val);
        };

        container.appendChild(cancelBtn);
        container.appendChild(submitBtn);
        modal.appendChild(container);

        document.body.appendChild(overlay);
        document.body.appendChild(modal);

        setTimeout(() => {
            overlay.style.opacity = '1';
            modal.style.opacity = '1';
            modal.style.transform = 'translate(-50%, -50%) scale(1)';
            input.focus();
        }, 10);
    };

    // ==================== ACTION FUNCTIONS ====================
    window.acceptRequest = function (requestId) {
        window.showConfirmModal('✓ Accept Request', 'Are you sure you want to accept this welfare request?', () => {
            // Save pending animation BEFORE submitting
            saveCurrentTab();
            sessionStorage.setItem('pendingAnimation', 'acceptRequest');

            const f = document.createElement('form');
            f.method = 'POST';
            f.action = '/NGO/AcceptRequest';
            const i = document.createElement('input');
            i.type = 'hidden';
            i.name = 'requestId';
            i.value = requestId;
            f.appendChild(i);
            document.body.appendChild(f);
            f.submit();
        });
    };

    window.rejectRequest = function (requestId) {
        window.showInputModal('✗ Reject Request', 'Please provide a reason for rejection:', 'Enter rejection reason...', (reason) => {
            if (reason) {
                // Save pending animation BEFORE submitting
                saveCurrentTab();
                sessionStorage.setItem('pendingAnimation', 'rejectRequest');

                const f = document.createElement('form');
                f.method = 'POST';
                f.action = '/NGO/RejectRequest';
                const i1 = document.createElement('input');
                i1.type = 'hidden';
                i1.name = 'requestId';
                i1.value = requestId;
                const i2 = document.createElement('input');
                i2.type = 'hidden';
                i2.name = 'reason';
                i2.value = reason;
                f.appendChild(i1);
                f.appendChild(i2);
                document.body.appendChild(f);
                f.submit();
            }
        });
    };

    window.fulfillRequest = function (requestId) {
        window.showConfirmModal('🎁 Fulfill Request', 'Are you sure you want to fulfill this request? Resources will be sent to the welfare fund.', () => {
            // Save pending animation BEFORE submitting
            saveCurrentTab();
            sessionStorage.setItem('pendingAnimation', 'fulfillRequest');

            const f = document.createElement('form');
            f.method = 'POST';
            f.action = '/NGO/FulfillRequest';
            const i = document.createElement('input');
            i.type = 'hidden';
            i.name = 'requestId';
            i.value = requestId;
            f.appendChild(i);
            document.body.appendChild(f);
            f.submit();
        });
    };

    window.openModal = function (id) {
        const m = document.getElementById(id);
        if (m) m.style.display = 'flex';
    };

    window.closeModal = function (id) {
        const m = document.getElementById(id);
        if (m) {
            m.style.display = 'none';
            const form = m.querySelector('form');
            if (form) form.reset();
        }
    };

    window.searchTable = function (inputId, tableId) {
        const input = document.getElementById(inputId);
        const table = document.getElementById(tableId);
        if (!input || !table) return;

        const filter = input.value.toLowerCase();
        const rows = table.getElementsByTagName('tr');

        for (let i = 1; i < rows.length; i++) {
            const cells = rows[i].getElementsByTagName('td');
            let found = false;
            for (let j = 0; j < cells.length; j++) {
                if ((cells[j].textContent || '').toLowerCase().indexOf(filter) > -1) {
                    found = true;
                    break;
                }
            }
            rows[i].style.display = found ? '' : 'none';
        }
    };

    // ==================== FORM VALIDATION ====================
    function validateDonationForm(formData, type) {
        if (type === 'money') {
            const amount = parseFloat(formData.get('amount'));
            if (!amount || amount <= 0) {
                window.showAlertModal('⚠️ Validation Error', 'Please enter a valid donation amount greater than zero.');
                return false;
            }
        } else if (type === 'food') {
            const quantity = parseInt(formData.get('quantity'));
            if (!quantity || quantity <= 0) {
                window.showAlertModal('⚠️ Validation Error', 'Please enter a valid food quantity greater than zero.');
                return false;
            }
        } else if (type === 'clothes') {
            const male = parseInt(formData.get('maleQty')) || 0;
            const female = parseInt(formData.get('femaleQty')) || 0;
            const kids = parseInt(formData.get('kidsQty')) || 0;
            if (male === 0 && female === 0 && kids === 0) {
                window.showAlertModal('⚠️ Validation Error', 'Please specify at least one type of clothes to donate.');
                return false;
            }
        } else if (type === 'shelter') {
            const beds = parseInt(formData.get('beds'));
            if (!beds || beds <= 0) {
                window.showAlertModal('⚠️ Validation Error', 'Please enter a valid number of shelter beds greater than zero.');
                return false;
            }
        }
        return true;
    }

    function validateProfileForm(formData, type) {
        if (type === 'email') {
            const email = formData.get('newEmail');
            if (!validateEmail(email)) {
                window.showAlertModal('⚠️ Validation Error', 'Please enter a valid email address.');
                return false;
            }
        } else if (type === 'password') {
            const currentPassword = formData.get('currentPassword');
            const newPassword = formData.get('newPassword');
            const confirmPassword = formData.get('confirmPassword');

            if (!currentPassword) {
                window.showAlertModal('⚠️ Validation Error', 'Please enter your current password.');
                return false;
            }
            if (!validatePassword(newPassword)) {
                window.showAlertModal('⚠️ Validation Error', 'New password must be at least 6 characters long.');
                return false;
            }
            if (newPassword !== confirmPassword) {
                window.showAlertModal('⚠️ Validation Error', 'New password and confirm password do not match.');
                return false;
            }
        } else if (type === 'phone') {
            const phone = formData.get('phone');
            if (!validatePhone(phone)) {
                window.showAlertModal('⚠️ Validation Error', 'Please enter a valid phone number.');
                return false;
            }
        }
        return true;
    }

    // ==================== FORM SUBMISSION ====================
    document.addEventListener('submit', function (e) {
        const form = e.target;
        const action = form.action;

        console.log('🔍 Form submit detected:', action);

        // Donation forms
        if (action && action.includes('/NGO/DonateMoney')) {
            e.preventDefault();
            const formData = new FormData(form);
            if (!validateDonationForm(formData, 'money')) return;

            saveCurrentTab();
            sessionStorage.setItem('pendingAnimation', 'donateMoney');
            form.submit();
        }
        else if (action && action.includes('/NGO/DonateFood')) {
            e.preventDefault();
            const formData = new FormData(form);
            if (!validateDonationForm(formData, 'food')) return;

            saveCurrentTab();
            sessionStorage.setItem('pendingAnimation', 'donateFood');
            form.submit();
        }
        else if (action && action.includes('/NGO/DonateClothes')) {
            e.preventDefault();
            const formData = new FormData(form);
            if (!validateDonationForm(formData, 'clothes')) return;

            saveCurrentTab();
            sessionStorage.setItem('pendingAnimation', 'donateClothes');
            form.submit();
        }
        else if (action && action.includes('/NGO/DonateShelter')) {
            e.preventDefault();
            const formData = new FormData(form);
            if (!validateDonationForm(formData, 'shelter')) return;

            saveCurrentTab();
            sessionStorage.setItem('pendingAnimation', 'donateShelter');
            form.submit();
        }
        // Profile update forms
        else if (action && action.includes('/NGO/UpdateProfile')) {
            e.preventDefault();
            saveCurrentTab();
            sessionStorage.setItem('pendingAnimation', 'updateProfile');
            form.submit();
        }
        else if (action && action.includes('/NGO/UpdateEmail')) {
            e.preventDefault();
            const formData = new FormData(form);
            if (!validateProfileForm(formData, 'email')) return;

            saveCurrentTab();
            sessionStorage.setItem('pendingAnimation', 'updateEmail');
            form.submit();
        }
        else if (action && action.includes('/NGO/UpdatePassword')) {
            e.preventDefault();
            const formData = new FormData(form);
            if (!validateProfileForm(formData, 'password')) return;

            saveCurrentTab();
            sessionStorage.setItem('pendingAnimation', 'updatePassword');
            form.submit();
        }
    });

    // ==================== CSS ====================
    const style = document.createElement('style');
    style.textContent = `
        @keyframes fall {
            to {
                transform: translateY(100vh) rotate(360deg);
                opacity: 0;
            }
        }
        @keyframes popIn {
            from {
                opacity: 0;
                transform: translate(-50%, -50%) scale(0.8);
            }
            to {
                opacity: 1;
                transform: translate(-50%, -50%) scale(1);
            }
        }
        @keyframes popOut {
            from {
                opacity: 1;
                transform: translate(-50%, -50%) scale(1);
            }
            to {
                opacity: 0;
                transform: translate(-50%, -50%) scale(0.8);
            }
        }
        @keyframes bounce {
            0%, 100% {
                transform: translateY(0);
            }
            50% {
                transform: translateY(-20px);
            }
        }
        .custom-modal-overlay {
            position: fixed;
            inset: 0;
            background: rgba(0,0,0,0.5);
            backdrop-filter: blur(4px);
            z-index: 99998;
            opacity: 0;
            transition: opacity 0.3s;
        }
        .custom-modal-box {
            position: fixed;
            left: 50%;
            top: 50%;
            transform: translate(-50%, -50%) scale(0.9);
            background: white;
            padding: 40px;
            border-radius: 20px;
            box-shadow: 0 20px 60px rgba(0,0,0,0.3);
            z-index: 99999;
            min-width: 400px;
            max-width: 90%;
            opacity: 0;
            transition: all 0.3s;
        }
        .modal-title {
            font-size: 24px;
            font-weight: 600;
            margin: 0 0 15px 0;
            color: #333;
        }
        .modal-message {
            font-size: 16px;
            line-height: 1.6;
            color: #666;
            margin: 0;
        }
        .modal-btn-container {
            display: flex;
            gap: 12px;
            margin-top: 30px;
            justify-content: flex-end;
        }
        .modal-btn {
            padding: 12px 24px;
            border: none;
            border-radius: 10px;
            font-size: 15px;
            font-weight: 500;
            cursor: pointer;
            transition: all 0.3s;
            font-family: 'Poppins', sans-serif;
        }
        .modal-btn-primary {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
        }
        .modal-btn-primary:hover {
            transform: translateY(-2px);
            box-shadow: 0 10px 25px rgba(102, 126, 234, 0.4);
        }
        .modal-btn-secondary {
            background: #f0f0f0;
            color: #333;
        }
        .modal-btn-secondary:hover {
            background: #e0e0e0;
            transform: translateY(-2px);
        }
        .modal-input {
            width: 100%;
            padding: 12px 16px;
            border: 2px solid rgba(102,126,234,0.2);
            border-radius: 12px;
            font-size: 15px;
            font-family: 'Poppins', sans-serif;
            transition: all 0.3s;
            background: white;
            resize: vertical;
        }
        .modal-input:focus {
            outline: none;
            border-color: #667eea;
            box-shadow: 0 5px 20px rgba(102,126,234,0.2);
        }
    `;
    document.head.appendChild(style);

    // ==================== INIT ====================
    document.addEventListener('DOMContentLoaded', function () {
        console.log('📄 DOM Content Loaded');

        restoreActiveTab();

        // Close modals on outside click
        qsa('[id$="Modal"]').forEach(m => {
            m.addEventListener('click', (e) => {
                if (e.target === m) {
                    m.style.display = 'none';
                }
            });
        });

        const pendingAnimation = sessionStorage.getItem('pendingAnimation');
        console.log('🎬 Pending animation:', pendingAnimation);

        if (pendingAnimation) {
            sessionStorage.removeItem('pendingAnimation');

            const tempDataSuccess = document.querySelector('[data-success-message]');
            const tempDataError = document.querySelector('[data-error-type]');

            console.log('📊 TempData markers:', {
                success: tempDataSuccess?.dataset.successMessage,
                errorType: tempDataError?.dataset.errorType,
                errorMessage: tempDataError?.dataset.errorMessage
            });

            setTimeout(() => {
                if (tempDataSuccess) {
                    console.log('✅ Showing success animation');

                    if (pendingAnimation === 'donateMoney') {
                        window.showActionSuccessAnimation('💰', 'Donation Sent!', 'Your monetary donation has been added to the welfare fund!', 'donate');
                    } else if (pendingAnimation === 'donateFood') {
                        window.showActionSuccessAnimation('🍲', 'Food Donated!', 'Your food donation has been added to the welfare fund!', 'donate');
                    } else if (pendingAnimation === 'donateClothes') {
                        window.showActionSuccessAnimation('👕', 'Clothes Donated!', 'Your clothing donation has been added to the welfare fund!', 'donate');
                    } else if (pendingAnimation === 'donateShelter') {
                        window.showActionSuccessAnimation('🏠', 'Shelter Donated!', 'Your shelter donation has been added to the welfare fund!', 'donate');
                    } else if (pendingAnimation === 'updateProfile') {
                        window.showActionSuccessAnimation('✅', 'Profile Updated!', 'Your profile has been updated successfully!', 'success');
                    } else if (pendingAnimation === 'updateEmail') {
                        window.showActionSuccessAnimation('📧', 'Email Updated!', 'Your email has been updated successfully!', 'success');
                    } else if (pendingAnimation === 'updatePassword') {
                        window.showActionSuccessAnimation('🔒', 'Password Updated!', 'Your password has been changed successfully!', 'success');
                    } else if (pendingAnimation === 'fulfillRequest') {
                        window.showActionSuccessAnimation('🎁', 'Request Fulfilled!', 'Welfare request has been fulfilled successfully!', 'fulfill');
                    } else if (pendingAnimation === 'acceptRequest') {
                        window.showActionSuccessAnimation('✅', 'Request Accepted!', 'Welfare request has been accepted successfully!', 'accept');
                    } else if (pendingAnimation === 'rejectRequest') {
                        window.showActionSuccessAnimation('❌', 'Request Rejected', 'The welfare request has been rejected.', 'reject');
                    }
                } else if (tempDataError) {
                    const errorType = tempDataError.dataset.errorType;
                    const errorMsg = tempDataError.dataset.errorMessage || 'An error occurred.';

                    console.log('❌ Error detected:', errorType, errorMsg);

                    if (errorType === 'INVALID_AMOUNT') {
                        window.showActionSuccessAnimation('⚠️', 'Invalid Amount', errorMsg, 'error');
                    } else if (errorType === 'INVALID_QUANTITY') {
                        window.showActionSuccessAnimation('⚠️', 'Invalid Quantity', errorMsg, 'error');
                    } else if (errorType === 'NO_CLOTHES') {
                        window.showActionSuccessAnimation('⚠️', 'No Clothes Specified', errorMsg, 'error');
                    } else if (errorType === 'INVALID_BEDS') {
                        window.showActionSuccessAnimation('⚠️', 'Invalid Beds', errorMsg, 'error');
                    } else if (errorType === 'EMAIL_EXISTS') {
                        window.showActionSuccessAnimation('📧', 'Email Exists', errorMsg, 'error');
                    } else if (errorType === 'WRONG_PASSWORD') {
                        window.showActionSuccessAnimation('🔒', 'Wrong Password', errorMsg, 'error');
                    } else if (errorType === 'WEAK_PASSWORD') {
                        window.showActionSuccessAnimation('🔒', 'Weak Password', errorMsg, 'error');
                    } else {
                        window.showActionSuccessAnimation('❌', 'Error!', errorMsg, 'error');
                    }
                }
            }, 300);
        }
    });

})();

console.log('✅ NGODashboard.js fully loaded');