/**
 * Melvyn
 * Script pour la page Anamnèse. 
 * Permet de gérer dynamiquement les flèches affichant si la catégorie est déroulée ou non.
 */
document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.card-header[data-bs-toggle="collapse"], .card-header div[data-bs-toggle="collapse"]').forEach(header => {
        const target = document.querySelector(header.getAttribute('data-bs-target'));
        const title = header.querySelector('h4');
        if (!title) return;

        const arrow = document.createElement('i');
        arrow.classList.add('bi', 'bi-chevron-down', 'ms-2', 'arrow-icon');
        title.appendChild(arrow);

        if (!target.classList.contains('show')) {
            arrow.classList.replace('bi-chevron-down', 'bi-chevron-right');
        }

        target.addEventListener('show.bs.collapse', () => {
            arrow.classList.replace('bi-chevron-right', 'bi-chevron-down');
        });
        target.addEventListener('hide.bs.collapse', () => {
            arrow.classList.replace('bi-chevron-down', 'bi-chevron-right');
        });
    });
});

function updateFieldVisualState(input) {
    const isRequired = input.hasAttribute('required');
    const value = input.value.trim();

    input.classList.remove('is-valid-custom', 'is-invalid-custom');

    if (isRequired && !value) {
        input.classList.add('is-invalid-custom');
        return false;
    }

    if (value) {
        const isValid = validateField(input);
        if (isValid) {
            input.classList.add('is-valid-custom');
        } else {
            input.classList.add('is-invalid-custom');
        }
        return isValid;
    }

    return true;
}


