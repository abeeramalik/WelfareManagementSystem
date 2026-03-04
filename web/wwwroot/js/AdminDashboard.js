

(function () {
    'use strict';

    console.log('🚀 AdminDashboard.js loaded successfully');

    function qs(sel) { return document.querySelector(sel); }
    function qsa(sel) { return Array.from(document.querySelectorAll(sel)); }

    // ==================== VALIDATION ====================
    function validateEmail(email) {
        return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
    }

    function validatePhone(phone) {
        return /^(\+92|0)?[0-9]{10,11}$/.test(phone.replace(/[\s\-]/g, ''));
    }

    function validateRegistrationNumber(regNo) {
        return /^[A-Z0-9\-]{5,20}$/i.test(regNo);
    }

    function validateNGOId(ngoId) {
        return ngoId >= 100000 && ngoId <= 999999;
    }

    function validatePassword(password) {
        return password && password.length >= 6;
    }

    function validateNGOForm(formData) {
        console.log('🔍 Validating NGO form...');

        const ngoId = parseInt(formData.get('NgoId'));
        const email = formData.get('Email');
        const phone = formData.get('Phone');
        const password = formData.get('Password');
        const regNo = formData.get('RegistrationNumber');
        const orgName = formData.get('OrganizationName');

        console.log('📋 Form values:', { ngoId, orgName, email, phone, regNo });

        if (!orgName || orgName.trim().length < 3) {
            console.log('❌ Organization name validation failed');
            window.showAlertModal('⚠️ Validation Error', 'Organization name must be at least 3 characters.');
            return false;
        }
        if (!validateNGOId(ngoId)) {
            console.log('❌ NGO ID validation failed');
            window.showAlertModal('⚠️ Validation Error', 'NGO ID must be 6 digits (100000-999999).');
            return false;
        }
        if (!validateEmail(email)) {
            console.log('❌ Email validation failed');
            window.showAlertModal('⚠️ Validation Error', 'Please enter a valid email address.');
            return false;
        }
        if (!validatePassword(password)) {
            console.log('❌ Password validation failed');
            window.showAlertModal('⚠️ Validation Error', 'Password must be at least 6 characters.');
            return false;
        }
        if (!validatePhone(phone)) {
            console.log('❌ Phone validation failed');
            window.showAlertModal('⚠️ Validation Error', 'Please enter a valid phone number.');
            return false;
        }
        if (!validateRegistrationNumber(regNo)) {
            console.log('❌ Registration number validation failed');
            window.showAlertModal('⚠️ Validation Error', 'Registration number must be 5-20 alphanumeric characters.');
            return false;
        }

        console.log('✅ All validations passed!');
        return true;
    }

    function validateAdminForm(formData) {
        const adminId = parseInt(formData.get('AdminId'));
        const fullName = formData.get('FullName');
        const password = formData.get('Password');
        const dob = new Date(formData.get('Dob'));

        if (!fullName || fullName.trim().length < 3) {
            window.showAlertModal('⚠️ Validation Error', 'Full name must be at least 3 characters.');
            return false;
        }
        if (adminId <= 0) {
            window.showAlertModal('⚠️ Validation Error', 'Admin ID must be a positive number.');
            return false;
        }
        if (!validatePassword(password)) {
            window.showAlertModal('⚠️ Validation Error', 'Password must be at least 6 characters.');
            return false;
        }

        const today = new Date();
        let age = today.getFullYear() - dob.getFullYear();
        if (today.getMonth() < dob.getMonth() ||
            (today.getMonth() === dob.getMonth() && today.getDate() < dob.getDate())) {
            age--;
        }

        if (age < 18) {
            window.showAlertModal('⚠️ Validation Error', 'Admin must be at least 18 years old.');
            return false;
        }
        return true;
    }

    // ==================== TAB MANAGEMENT ====================
    function saveCurrentTab() {
        const activeTab = qs('.content-card.active');
        if (activeTab) {
            sessionStorage.setItem('adminActiveTab', activeTab.id);
        }
    }

    function restoreActiveTab() {
        const savedTab = sessionStorage.getItem('adminActiveTab');
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
            sessionStorage.setItem('adminActiveTab', sectionName);
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
            deactivate: {
                gradient: 'linear-gradient(135deg, #dc3545 0%, #c82333 100%)',
                confetti: ['#dc3545', '#c82333', '#ff6b6b']
            },
            activate: {
                gradient: 'linear-gradient(135deg, #28a745 0%, #218838 100%)',
                confetti: ['#28a745', '#218838', '#48c774']
            }
        };
        const theme = colors[actionType] || colors.activate;

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
            animation: 'popIn 0.3s ease-out forwards'
        });

        box.innerHTML = `
            <div style="font-size: 64px; margin-bottom: 20px; animation: bounce 0.6s ease-in-out;">${emoji}</div>
            <h2 style="color: white; margin: 0 0 15px 0; font-size: 28px;">${title}</h2>
            <p style="color: rgba(255,255,255,0.95); font-size: 16px;">${message}</p>
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
    window.approveRequest = function (requestId) {
        window.showConfirmModal('✓ Approve Request', 'Are you sure you want to approve this request?', () => {
            document.getElementById('approveRequestId').value = requestId;
            window.showActionSuccessAnimation('✅', 'Approved!', 'Request approved successfully!', 'activate');
            setTimeout(() => document.getElementById('approveForm').submit(), 2500);
        });
    };

    window.rejectRequest = function (requestId) {
        window.showInputModal('✗ Reject Request', 'Please provide a reason for rejection:', 'Enter rejection reason...', (reason) => {
            if (reason) {
                document.getElementById('rejectRequestId').value = requestId;
                document.getElementById('rejectReason').value = reason;
                window.showActionSuccessAnimation('❌', 'Rejected', 'Request has been rejected.', 'deactivate');
                setTimeout(() => document.getElementById('rejectForm').submit(), 2500);
            }
        });
    };

    window.fulfillRequest = function (requestId, canFulfill) {
        if (!canFulfill) {
            window.showAlertModal('⚠️ Error', 'Insufficient resources to fulfill this request.');
            return;
        }
        window.showConfirmModal('🎁 Fulfill Request', 'Are you sure you want to fulfill this request?', () => {
            document.getElementById('fulfillRequestId').value = requestId;
            document.getElementById('fulfillForm').submit();
        });
    };

    window.toggleUserStatus = function (userId, isActive) {
        const action = isActive ? 'Deactivate' : 'Activate';
        window.showConfirmModal(
            `🔄 ${action} User`,
            `Are you sure you want to ${action.toLowerCase()} this user?`,
            () => {
                window.showActionSuccessAnimation(
                    isActive ? '🚫' : '✅',
                    isActive ? 'Deactivated!' : 'Activated!',
                    `User has been ${isActive ? 'deactivated' : 'activated'} successfully.`,
                    isActive ? 'deactivate' : 'activate'
                );
                setTimeout(() => {
                    const f = document.createElement('form');
                    f.method = 'POST';
                    f.action = '/Admin/ToggleUserStatus';
                    const i = document.createElement('input');
                    i.type = 'hidden';
                    i.name = 'userId';
                    i.value = userId;
                    f.appendChild(i);
                    document.body.appendChild(f);
                    f.submit();
                }, 2500);
            }
        );
    };

    window.toggleNGOStatus = function (ngoId, isActive) {
        const action = isActive ? 'Deactivate' : 'Activate';
        window.showConfirmModal(
            `🔄 ${action} NGO`,
            `Are you sure you want to ${action.toLowerCase()} this NGO?`,
            () => {
                window.showActionSuccessAnimation(
                    isActive ? '🚫' : '✅',
                    isActive ? 'Deactivated!' : 'Activated!',
                    `NGO has been ${isActive ? 'deactivated' : 'activated'} successfully.`,
                    isActive ? 'deactivate' : 'activate'
                );
                setTimeout(() => {
                    const f = document.createElement('form');
                    f.method = 'POST';
                    f.action = '/Admin/ToggleNGOStatus';
                    const i = document.createElement('input');
                    i.type = 'hidden';
                    i.name = 'ngoId';
                    i.value = ngoId;
                    f.appendChild(i);
                    document.body.appendChild(f);
                    f.submit();
                }, 2500);
            }
        );
    };

    window.verifyNGO = function (ngoId, isVerified) {
        const action = isVerified ? 'Unverify' : 'Verify';
        window.showConfirmModal(
            `✓ ${action} NGO`,
            `Are you sure you want to ${action.toLowerCase()} this NGO?`,
            () => {
                window.showActionSuccessAnimation(
                    isVerified ? '⚠️' : '✅',
                    isVerified ? 'Unverified!' : 'Verified!',
                    `NGO has been ${isVerified ? 'unverified' : 'verified'} successfully.`,
                    isVerified ? 'deactivate' : 'activate'
                );
                setTimeout(() => {
                    const f = document.createElement('form');
                    f.method = 'POST';
                    f.action = '/Admin/VerifyNGO';
                    const i = document.createElement('input');
                    i.type = 'hidden';
                    i.name = 'ngoId';
                    i.value = ngoId;
                    f.appendChild(i);
                    document.body.appendChild(f);
                    f.submit();
                }, 2500);
            }
        );
    };

    window.cancelNGORequest = function (requestId) {
        window.showConfirmModal(
            '❌ Cancel Request',
            'Are you sure you want to cancel this NGO request?',
            () => {
                document.getElementById('cancelReqId').value = requestId;
                window.showActionSuccessAnimation('🚫', 'Cancelled!', 'Request has been cancelled.', 'deactivate');
                setTimeout(() => document.getElementById('cancelForm').submit(), 2500);
            }
        );
    };

    window.toggleNGOFields = function (type) {
        ['financialField', 'foodField', 'clothesField', 'shelterField'].forEach(id => {
            const el = document.getElementById(id);
            if (el) el.classList.add('hidden');
        });

        const fieldMap = {
            Financial: 'financialField',
            Food: 'foodField',
            Clothes: 'clothesField',
            Shelter: 'shelterField'
        };

        const target = document.getElementById(fieldMap[type]);
        if (target) target.classList.remove('hidden');
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
            ['financialField', 'foodField', 'clothesField', 'shelterField'].forEach(fieldId => {
                const field = document.getElementById(fieldId);
                if (field) field.classList.add('hidden');
            });
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

    // ==================== FORM SUBMISSION ====================
    document.addEventListener('submit', function (e) {
        const form = e.target;

        console.log('🔍 Form submit detected:', form.action);

        if (form.action && form.action.includes('/Admin/AddNGO')) {
            e.preventDefault();
            console.log('📝 Adding NGO...');

            if (!form.checkValidity()) {
                console.log('❌ Form validation failed');
                form.reportValidity();
                return;
            }

            const formData = new FormData(form);
            console.log('📦 Form data:', {
                NgoId: formData.get('NgoId'),
                OrganizationName: formData.get('OrganizationName'),
                Email: formData.get('Email')
            });

            if (!validateNGOForm(formData)) {
                console.log('❌ Custom validation failed');
                return;
            }

            console.log('✅ Validation passed, submitting...');
            saveCurrentTab();
            sessionStorage.setItem('pendingAnimation', 'addNGO');
            form.submit();
        }
        else if (form.action && form.action.includes('/Admin/AddAdmin')) {
            e.preventDefault();
            console.log('📝 Adding Admin...');

            if (!form.checkValidity()) {
                console.log('❌ Form validation failed');
                form.reportValidity();
                return;
            }

            const formData = new FormData(form);
            console.log('📦 Form data:', {
                AdminId: formData.get('AdminId'),
                FullName: formData.get('FullName')
            });

            if (!validateAdminForm(formData)) {
                console.log('❌ Custom validation failed');
                return;
            }

            console.log('✅ Validation passed, submitting...');
            saveCurrentTab();
            sessionStorage.setItem('pendingAnimation', 'addAdmin');
            form.submit();
        }
        else if (form.action && form.action.includes('/Admin/CreateNGORequest')) {
            e.preventDefault();
            console.log('📝 Creating NGO Request...');

            if (!form.checkValidity()) {
                form.reportValidity();
                return;
            }

            saveCurrentTab();
            sessionStorage.setItem('pendingAnimation', 'createRequest');
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
        .hidden {
            display: none !important;
        }
        
        .alert, .alert-success, .alert-danger, .alert-error, .alert-warning, .alert-info {
            display: none !important;
        }
    `;
    document.head.appendChild(style);

    // ==================== INIT ====================
    document.addEventListener('DOMContentLoaded', function () {
        console.log('📄 DOM Content Loaded');

        restoreActiveTab();

        qsa('[id$="Modal"]').forEach(m => {
            m.addEventListener('click', (e) => {
                if (e.target === m) {
                    m.style.display = 'none';
                }
            });
        });

        const ngoRequestModal = document.getElementById('ngoRequestModal');
        if (ngoRequestModal) {
            ngoRequestModal.addEventListener('click', (e) => {
                if (e.target === ngoRequestModal) {
                    window.closeModal('ngoRequestModal');
                }
            });
        }

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
                if (pendingAnimation === 'addNGO') {
                    if (tempDataSuccess) {
                        console.log('✅ Showing NGO success animation');
                        window.showActionSuccessAnimation('🏢', 'NGO Added!', 'NGO has been added successfully!', 'activate');
                    } else if (tempDataError) {
                        const errorType = tempDataError.dataset.errorType;
                        const errorMsg = tempDataError.dataset.errorMessage || '';

                        console.log('❌ NGO Error detected:', errorType, errorMsg);

                        if (errorType === 'NGO_ID_EXISTS') {
                            window.showActionSuccessAnimation('🆔', 'ID Already Exists!', errorMsg, 'deactivate');
                        } else if (errorType === 'NGO_EMAIL_EXISTS') {
                            window.showActionSuccessAnimation('📧', 'Email Already Exists!', errorMsg, 'deactivate');
                        } else {
                            window.showActionSuccessAnimation('❌', 'Error!', errorMsg, 'deactivate');
                        }
                    } else {
                        console.log('⚠️ No TempData found for NGO operation');
                    }
                } else if (pendingAnimation === 'addAdmin') {
                    if (tempDataSuccess) {
                        console.log('✅ Showing Admin success animation');
                        window.showActionSuccessAnimation('👨‍💼', 'Admin Added!', 'Administrator has been added successfully!', 'activate');
                    } else if (tempDataError) {
                        const errorMsg = tempDataError.dataset.errorMessage || 'Failed to add admin.';
                        console.log('❌ Showing Admin error animation:', errorMsg);
                        window.showActionSuccessAnimation('❌', 'Error!', errorMsg, 'deactivate');
                    } else {
                        console.log('⚠️ No TempData found for Admin operation');
                    }
                } else if (pendingAnimation === 'createRequest') {
                    if (tempDataSuccess) {
                        console.log('✅ Showing Request success animation');
                        window.showActionSuccessAnimation('📤', 'Request Sent!', 'Request has been sent to NGO successfully!', 'activate');
                    } else if (tempDataError) {
                        const errorMsg = tempDataError.dataset.errorMessage || 'Failed to send request.';
                        console.log('❌ Showing Request error animation');
                        window.showActionSuccessAnimation('❌', 'Error!', errorMsg, 'deactivate');
                    }
                }
            }, 300);
        }
    });

})();

console.log('✅ AdminDashboard.js fully loaded');