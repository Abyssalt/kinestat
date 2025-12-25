/**
 * Admin Module
 * Centralized JavaScript for all admin views including password validation,
 * physiotherapist management, and questions management
 */

// ============================================================================
// PASSWORD VALIDATION MODULE
// ============================================================================

/**
 * Validates password strength against defined security requirements
 * @param {string} password - The password to validate
 * @returns {Object} Validation result containing isValid flag and array of error messages
 */
function validatePasswordStrength(password) {
    const minLength = 8;
    const hasUpperCase = /[A-Z]/.test(password);
    const hasLowerCase = /[a-z]/.test(password);
    const hasNumber = /\d/.test(password);
    const hasSpecialChar = /[!@#$%^&*(),.?":{}|<>_\-+=[\]\\/'`~;]/.test(password);

    const errors = [];

    if (password.length < minLength) {
        errors.push('Au moins ' + minLength + ' caractères');
    }
    if (!hasUpperCase) {
        errors.push('Au moins une majuscule');
    }
    if (!hasLowerCase) {
        errors.push('Au moins une minuscule');
    }
    if (!hasNumber) {
        errors.push('Au moins un chiffre');
    }
    if (!hasSpecialChar) {
        errors.push('Au moins un caractère spécial (!@#$%^&*...)');
    }

    return {
        isValid: errors.length === 0,
        errors: errors
    };
}

/**
 * Displays password validation errors in the UI
 * @param {string} inputId - The ID of the password input element
 * @param {string} errorContainerId - The ID of the error container element
 * @param {Array<string>} errors - Array of error messages to display
 */
function displayPasswordErrors(inputId, errorContainerId, errors) {
    const errorContainer = document.getElementById(errorContainerId);
    const input = document.getElementById(inputId);

    if (errors.length > 0) {
        input.classList.add('is-invalid');
        errorContainer.innerHTML = '<strong>Mot de passe faible :</strong><br>' + errors.join('<br>');
        errorContainer.style.display = 'block';
    } else {
        input.classList.remove('is-invalid');
        input.classList.add('is-valid');
        errorContainer.style.display = 'none';
    }
}

// ============================================================================
// PHYSIOTHERAPIST MANAGEMENT MODULE
// ============================================================================

/**
 * Loads physiotherapist data into the edit modal
 * @param {number} id - Physiotherapist ID
 * @param {string} firstName - Physiotherapist first name
 * @param {string} lastName - Physiotherapist last name
 * @param {string} email - Physiotherapist email address
 * @param {string} phone - Physiotherapist phone number
 * @param {number} inami - INAMI registration number
 */
function loadPhysioData(id, firstName, lastName, email, phone, inami) {
    document.getElementById('editId').value = id;
    document.getElementById('editFirstName').value = firstName;
    document.getElementById('editLastName').value = lastName;
    document.getElementById('editEmail').value = email;
    document.getElementById('editPhoneNumber').value = phone;
    document.getElementById('editPassword').value = '';
    document.getElementById('editPasswordConfirm').value = '';
    document.getElementById('editINAMI').value = inami;

    document.getElementById('editPassword').classList.remove('is-invalid', 'is-valid');
    document.getElementById('editPasswordConfirm').classList.remove('is-invalid', 'is-valid');
}

/**
 * Loads physiotherapist data into the delete confirmation modal
 * @param {number} id - Physiotherapist ID
 * @param {string} firstName - Physiotherapist first name
 * @param {string} lastName - Physiotherapist last name
 */
function loadDeletePhysioData(id, firstName, lastName) {
    document.getElementById('deleteId').value = id;
    document.getElementById('deletePhysioName').textContent = firstName + ' ' + lastName;
}

/**
 * Initializes event listeners for physiotherapist management
 * Sets up validation and form submission handlers
 */
function initializePhysioManagement() {
    const createPasswordInput = document.getElementById('createPassword');
    const editPasswordInput = document.getElementById('editPassword');
    const createModal = document.getElementById('createPhysioModal');
    const editModal = document.getElementById('editPhysioModal');

    // Check if elements exist (only on Index page)
    if (!createPasswordInput || !editPasswordInput) {
        return;
    }

    // Password validation on input for create form
    createPasswordInput.addEventListener('input', function () {
        const password = this.value;
        if (password.length > 0) {
            const validation = validatePasswordStrength(password);
            displayPasswordErrors('createPassword', 'createPasswordError', validation.errors);
        } else {
            this.classList.remove('is-invalid', 'is-valid');
            document.getElementById('createPasswordError').style.display = 'none';
        }
    });

    // Password validation on input for edit form
    editPasswordInput.addEventListener('input', function () {
        const password = this.value;
        if (password.length > 0) {
            const validation = validatePasswordStrength(password);
            displayPasswordErrors('editPassword', 'editPasswordError', validation.errors);
        } else {
            this.classList.remove('is-invalid', 'is-valid');
            document.getElementById('editPasswordError').style.display = 'none';
        }
    });

    // Form submission validation for create form
    createModal.querySelector('form').addEventListener('submit', function (e) {
        const password = document.getElementById('createPassword').value;
        const passwordConfirm = document.getElementById('createPasswordConfirm').value;

        const validation = validatePasswordStrength(password);
        if (!validation.isValid) {
            e.preventDefault();
            displayPasswordErrors('createPassword', 'createPasswordError', validation.errors);
            return false;
        }

        if (password !== passwordConfirm) {
            e.preventDefault();
            document.getElementById('createPassword').classList.add('is-invalid');
            document.getElementById('createPasswordConfirm').classList.add('is-invalid');
            document.getElementById('createPasswordError').innerHTML = '<strong>Les mots de passe ne correspondent pas.</strong>';
            document.getElementById('createPasswordError').style.display = 'block';
            return false;
        }

        document.getElementById('createPassword').classList.remove('is-invalid');
        document.getElementById('createPasswordConfirm').classList.remove('is-invalid');
        document.getElementById('createPasswordError').style.display = 'none';
    });

    // Form submission validation for edit form
    editModal.querySelector('form').addEventListener('submit', function (e) {
        const password = document.getElementById('editPassword').value;
        const passwordConfirm = document.getElementById('editPasswordConfirm').value;

        // Allow empty passwords (no change)
        if (password === '' && passwordConfirm === '') {
            return true;
        }

        const validation = validatePasswordStrength(password);
        if (!validation.isValid) {
            e.preventDefault();
            displayPasswordErrors('editPassword', 'editPasswordError', validation.errors);
            return false;
        }

        if (password !== passwordConfirm) {
            e.preventDefault();
            document.getElementById('editPassword').classList.add('is-invalid');
            document.getElementById('editPasswordConfirm').classList.add('is-invalid');
            document.getElementById('editPasswordError').innerHTML = '<strong>Les mots de passe ne correspondent pas.</strong>';
            document.getElementById('editPasswordError').style.display = 'block';
            return false;
        }

        document.getElementById('editPassword').classList.remove('is-invalid');
        document.getElementById('editPasswordConfirm').classList.remove('is-invalid');
        document.getElementById('editPasswordError').style.display = 'none';
    });

    // Remove validation errors on password confirmation input
    ['createPasswordConfirm', 'editPasswordConfirm'].forEach(function (id) {
        document.getElementById(id).addEventListener('input', function () {
            this.classList.remove('is-invalid');
        });
    });

    // Reset form when create modal is closed
    createModal.addEventListener('hidden.bs.modal', function () {
        this.querySelector('form').reset();
        document.getElementById('createPassword').classList.remove('is-invalid', 'is-valid');
        document.getElementById('createPasswordConfirm').classList.remove('is-invalid', 'is-valid');
        document.getElementById('createPasswordError').style.display = 'none';
    });
}

// ============================================================================
// PASSWORD CHANGE MODULE
// ============================================================================

/**
 * Initializes password change functionality
 * Sets up validation for new password and confirmation matching
 */
function initializePasswordChange() {
    const newPasswordInput = document.getElementById('newPassword');
    const confirmPasswordInput = document.getElementById('confirmNewPassword');
    const changePasswordForm = document.getElementById('changePasswordForm');

    // Check if elements exist (only on ChangePassword page)
    if (!newPasswordInput || !confirmPasswordInput || !changePasswordForm) {
        return;
    }

    // Validate new password strength on input
    newPasswordInput.addEventListener('input', function () {
        const password = this.value;
        if (password.length > 0) {
            const validation = validatePasswordStrength(password);
            displayPasswordErrors('newPassword', 'passwordStrengthError', validation.errors);
        } else {
            this.classList.remove('is-invalid', 'is-valid');
            document.getElementById('passwordStrengthError').style.display = 'none';
        }
    });

    // Check if passwords match on confirmation input
    confirmPasswordInput.addEventListener('input', function () {
        const newPassword = newPasswordInput.value;
        const confirmPassword = this.value;

        if (confirmPassword.length > 0) {
            if (newPassword !== confirmPassword) {
                this.classList.add('is-invalid');
                this.classList.remove('is-valid');
            } else {
                this.classList.remove('is-invalid');
                this.classList.add('is-valid');
            }
        } else {
            this.classList.remove('is-invalid', 'is-valid');
        }
    });

    // Validate form before submission
    changePasswordForm.addEventListener('submit', function (e) {
        const newPassword = newPasswordInput.value;
        const confirmPassword = confirmPasswordInput.value;

        const validation = validatePasswordStrength(newPassword);
        if (!validation.isValid) {
            e.preventDefault();
            displayPasswordErrors('newPassword', 'passwordStrengthError', validation.errors);
            return false;
        }

        if (newPassword !== confirmPassword) {
            e.preventDefault();
            newPasswordInput.classList.add('is-invalid');
            confirmPasswordInput.classList.add('is-invalid');
            alert('Les mots de passe ne correspondent pas.');
            return false;
        }

        return true;
    });
}

// ============================================================================
// QUESTIONS MANAGEMENT MODULE
// ============================================================================

/**
 * Displays a temporary alert message
 * @param {string} message - The message to display
 * @param {string} type - Alert type ('success' or 'danger')
 */
function showAlert(message, type) {
    const alertContainer = document.getElementById('alertContainer');
    const alertId = 'alert-' + Date.now();

    const alertHtml = `
        <div id="${alertId}" class="alert alert-${type} alert-dismissible fade show border-0" role="alert">
            <i class="bi bi-${type === 'success' ? 'check-circle-fill' : 'exclamation-triangle-fill'} me-2"></i>
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Fermer"></button>
        </div>
    `;

    alertContainer.insertAdjacentHTML('beforeend', alertHtml);

    // Auto-dismiss after 5 seconds
    setTimeout(() => {
        const alert = document.getElementById(alertId);
        if (alert) {
            const bsAlert = new bootstrap.Alert(alert);
            bsAlert.close();
        }
    }, 5000);
}

/**
 * Updates the RV values in the UI after successful save
 * @param {string} questionId - The ID of the question
 * @param {string} rvPositive - The new RV+ value
 * @param {string} rvNegative - The new RV- value
 * @param {string} sourceRv - The source reference
 */
function updateQuestionUI(questionId, rvPositive, rvNegative, sourceRv) {
    // Update table row (desktop view)
    const row = document.querySelector(`#questionsTable tr[data-question-id="${questionId}"]`);
    if (row) {
        row.querySelector('.rv-positive').textContent = parseFloat(rvPositive).toFixed(2);
        row.querySelector('.rv-negative').textContent = parseFloat(rvNegative).toFixed(2);
        row.querySelector('.source-text').innerHTML = `<small class="text-secondary">${sourceRv || 'Non spécifié'}</small>`;
    }

    // Update card (mobile view)
    const card = document.querySelector(`#questionsList .question-card[data-question-id="${questionId}"]`);
    if (card) {
        card.querySelector('.rv-positive').textContent = parseFloat(rvPositive).toFixed(2);
        card.querySelector('.rv-negative').textContent = parseFloat(rvNegative).toFixed(2);

        const sourceElement = card.querySelector('.source-text');
        if (sourceRv) {
            if (sourceElement) {
                sourceElement.innerHTML = `<small class="text-secondary"><i class="bi bi-book me-1"></i>${sourceRv}</small>`;
            } else {
                const sourceHtml = `<div class="source-text"><small class="text-secondary"><i class="bi bi-book me-1"></i>${sourceRv}</small></div>`;
                card.insertAdjacentHTML('beforeend', sourceHtml);
            }
        } else if (sourceElement) {
            sourceElement.remove();
        }
    }
}

/**
 * Initializes the search functionality for filtering questions
 */
function initializeSearch() {
    const searchInput = document.getElementById('searchInput');
    const tableBody = document.getElementById('questionsTableBody');
    const questionsList = document.getElementById('questionsList');
    const resultCount = document.getElementById('resultCount');

    // Check if elements exist (only on Questions page)
    if (!searchInput) {
        return;
    }

    searchInput.addEventListener('input', function () {
        const searchTerm = this.value.toLowerCase().trim();
        let visibleCount = 0;

        // Filter table rows (desktop view)
        if (tableBody) {
            const rows = tableBody.querySelectorAll('tr');
            rows.forEach(row => {
                const title = row.getAttribute('data-question-title');
                const category = row.getAttribute('data-category').toLowerCase();

                if (searchTerm === '' || title.includes(searchTerm) || category.includes(searchTerm)) {
                    row.style.display = '';
                    visibleCount++;
                } else {
                    row.style.display = 'none';
                }
            });
        }

        // Filter cards (mobile view)
        if (questionsList) {
            const cards = questionsList.querySelectorAll('.question-card');
            visibleCount = 0;
            cards.forEach(card => {
                const title = card.getAttribute('data-question-title');
                const category = card.getAttribute('data-category').toLowerCase();

                if (searchTerm === '' || title.includes(searchTerm) || category.includes(searchTerm)) {
                    card.style.display = '';
                    visibleCount++;
                } else {
                    card.style.display = 'none';
                }
            });
        }

        if (resultCount) {
            resultCount.textContent = `${visibleCount} question(s)`;
        }
    });
}

/**
 * Initializes the edit question modal
 * Populates form fields with question data when modal opens
 */
function initializeEditModal() {
    const editModal = document.getElementById('editQuestionModal');

    // Check if element exists (only on Questions page)
    if (!editModal) {
        return;
    }

    editModal.addEventListener('show.bs.modal', function (event) {
        // Clear previous validation states
        document.getElementById('editRVPositive').classList.remove('is-invalid');
        document.getElementById('editRVNegative').classList.remove('is-invalid');

        const button = event.relatedTarget;

        // Extract data from button attributes
        const questionId = button.getAttribute('data-question-id');
        const questionTitle = button.getAttribute('data-question-title');
        const rvPositive = button.getAttribute('data-question-rv-positive');
        const rvNegative = button.getAttribute('data-question-rv-negative');
        const sourceRv = button.getAttribute('data-question-source');

        // Populate modal form
        document.getElementById('editQuestionId').value = questionId;
        document.getElementById('editQuestionTitle').textContent = questionTitle;
        document.getElementById('editRVPositive').value = rvPositive;
        document.getElementById('editRVNegative').value = rvNegative;
        document.getElementById('editSourceRv').value = sourceRv || '';
    });
}

/**
 * Initializes the form submission handler for editing questions
 * @param {string} updateUrl - The URL for the update action
 */
function initializeFormSubmission(updateUrl) {
    const editForm = document.getElementById('editQuestionForm');

    // Check if element exists (only on Questions page)
    if (!editForm) {
        return;
    }

    editForm.addEventListener('submit', async function (e) {
        e.preventDefault();

        const id = document.getElementById('editQuestionId').value;
        const rvPositive = document.getElementById('editRVPositive').value.replace(',', '.');
        const rvNegative = document.getElementById('editRVNegative').value.replace(',', '.');
        const sourceRv = document.getElementById('editSourceRv').value;

        const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

        try {
            const response = await fetch(updateUrl, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: new URLSearchParams({
                    '__RequestVerificationToken': token,
                    'id': id,
                    'rvPositive': rvPositive,
                    'rvNegative': rvNegative,
                    'sourceRv': sourceRv
                })
            });

            const result = await response.json();

            if (result.success) {
                updateQuestionUI(id, rvPositive, rvNegative, sourceRv);

                const modal = bootstrap.Modal.getInstance(document.getElementById('editQuestionModal'));
                modal.hide();

                showAlert(result.message, 'success');
            } else {
                showAlert(result.message, 'danger');
            }
        } catch (error) {
            showAlert('Erreur de connexion au serveur : ' + error.message, 'danger');
        }
    });
}

/**
 * Initializes all question management functionality
 * @param {string} updateUrl - The URL for the update action
 */
function initializeQuestionsManagement(updateUrl) {
    initializeSearch();
    initializeEditModal();
    initializeFormSubmission(updateUrl);
}

// ============================================================================
// INITIALIZATION
// ============================================================================

/**
 * Initialize all admin modules when DOM is ready
 */
document.addEventListener('DOMContentLoaded', function () {
    initializePhysioManagement();
    initializePasswordChange();
    initializeSearch();
    initializeEditModal();
});