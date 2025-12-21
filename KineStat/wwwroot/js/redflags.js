let hasDisplayedCriticalModal = false;
let criticalRedflagThreshold = 20;
let globalPatientId = null;
let globalAssessmentId = null;
let globalFolderId = null;

async function loadQuestionsByCategory(categoryId) {
    const response = await fetch(`/RedFlags/${globalPatientId}/Assessment/${globalAssessmentId}/Questions/${categoryId}`);
    const html = await response.text();
    document.getElementById("questionnaireContainer").innerHTML = html;

    transformQuestionsToCompactFormat();
    attachAnswerListeners();
    initializeTooltips();
}

function transformQuestionsToCompactFormat() {
    const container = document.getElementById("questionnaireContainer");
    const cards = container.querySelectorAll('.card');

    cards.forEach(card => {
        const cardBody = card.querySelector('.card-body');
        if (!cardBody) return;

        card.classList.add('border', 'shadow-sm', 'mb-3');
        card.style.borderWidth = '2px';

        const questionText = cardBody.querySelector('p, .fw-medium');
        const btnGroup = cardBody.querySelector('.btn-group');
        const collapseDiv = cardBody.querySelector('.collapse');

        if (!questionText || !btnGroup) return;

        const mainContainer = document.createElement('div');
        mainContainer.className = 'd-flex flex-column flex-lg-row align-items-start align-items-lg-center gap-3 w-100';

        const questionContainer = document.createElement('div');
        questionContainer.className = 'fw-medium flex-grow-1';
        questionContainer.innerHTML = questionText.innerHTML;


        const tooltip = questionText.querySelector('[data-bs-toggle="tooltip"]');
        if (tooltip) {
            const icon = document.createElement('i');
            icon.className = 'bi bi-info-circle ms-2 text-primary';
            icon.setAttribute('data-bs-toggle', 'tooltip');
            icon.setAttribute('title', tooltip.getAttribute('title'));
            questionContainer.appendChild(icon);
        }


        const responseContainer = document.createElement('div');
        responseContainer.className = 'flex-shrink-0';


        btnGroup.classList.remove('w-100', 'd-grid');
        btnGroup.classList.add('btn-group');

        responseContainer.appendChild(btnGroup);


        const notesSection = document.createElement('div');
        notesSection.className = 'flex-shrink-0 d-flex align-items-center gap-2';
        notesSection.style.minWidth = '200px';


        const toggleBtn = document.createElement('button');
        toggleBtn.type = 'button';
        toggleBtn.className = 'btn btn-sm btn-outline-secondary';
        toggleBtn.innerHTML = '<i class="bi bi-pencil"></i>';
        toggleBtn.title = 'Ajouter/Modifier une note';

        const textarea = collapseDiv ? collapseDiv.querySelector('textarea') : null;
        if (textarea) {
            const newTextarea = document.createElement('textarea');
            newTextarea.className = 'form-control form-control-sm';
            newTextarea.name = textarea.name;
            newTextarea.dataset.questionId = textarea.dataset.questionId;
            newTextarea.rows = 1;
            newTextarea.placeholder = 'Note...';
            newTextarea.value = textarea.value || '';
            newTextarea.style.display = textarea.value ? 'block' : 'none';
            newTextarea.style.resize = 'vertical';
            newTextarea.style.minWidth = '150px';

            if (textarea.value) {
                toggleBtn.innerHTML = '<i class="bi bi-eye-slash"></i>';
            }

            toggleBtn.addEventListener('click', () => {
                if (newTextarea.style.display === 'none') {
                    newTextarea.style.display = 'block';
                    toggleBtn.innerHTML = '<i class="bi bi-eye-slash"></i>';
                    newTextarea.focus();
                } else {
                    newTextarea.style.display = 'none';
                    toggleBtn.innerHTML = '<i class="bi bi-pencil"></i>';
                }
            });

 
            newTextarea.addEventListener('change', handleTextareaInput);

            notesSection.appendChild(toggleBtn);
            notesSection.appendChild(newTextarea);
        }


        mainContainer.appendChild(questionContainer);
        mainContainer.appendChild(responseContainer);
        mainContainer.appendChild(notesSection);


        cardBody.innerHTML = '';
        cardBody.className = 'card-body p-3 p-md-4';
        cardBody.appendChild(mainContainer);
    });
}

function initializeTooltips() {
    const tooltipElements = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    tooltipElements.forEach(el => {
        new bootstrap.Tooltip(el);
    });
}

function showCriticalRedFlagModal(patientId) {
    if (hasDisplayedCriticalModal) return;
    hasDisplayedCriticalModal = true;

    const btn = document.getElementById("redirectToSummaryBtn");
    btn.href = `/Patient/${patientId}/Dossier/${globalFolderId}/Resultat/${globalAssessmentId}`;

    const modal = new bootstrap.Modal(document.getElementById("redFlagCriticalModal"));
    modal.show();
}

function fetchInitialState() {
    fetch(`/Patient/${globalPatientId}/Assessment/${globalAssessmentId}/CategoryPercentages`)
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                if (data.totalPercentage !== undefined) {
                    RedFlagsStore.set(data.totalPercentage);
                } else if (data.redflags !== undefined) {
                    RedFlagsStore.set(data.redflags);
                }

                if (data.categories) {
                    RedFlagsStore.setCategories(data.categories);
                }
            }
        });
}

function attachAnswerListeners() {
    const radios = document.querySelectorAll('input[name^="q_"]');
    radios.forEach(radio => {
        radio.addEventListener('change', handleRadioChange);
    });

    const textareas = document.querySelectorAll('textarea[name^="note_"]');
    textareas.forEach(textarea => {
        textarea.addEventListener('change', handleTextareaInput);
    });
}

function handleRadioChange(event) {
    const radio = event.target;
    const questionId = radio.dataset.questionId;
    const value = radio.value === "true";
    const comment = getCommentForQuestion(questionId);
    saveAnswer(globalPatientId, questionId, value, comment);
}

function handleTextareaInput(event) {
    const textarea = event.target;
    const questionId = textarea.dataset.questionId;
    const comment = textarea.value;
    const value = getRadioValueForQuestion(questionId);
    saveAnswer(globalPatientId, questionId, value, comment);
}

function getCommentForQuestion(questionId) {
    const textarea = document.querySelector(`textarea[name="note_${questionId}"]`);
    return textarea ? textarea.value : "";
}

function getRadioValueForQuestion(questionId) {
    const radio = document.querySelector(`input[name="q_${questionId}"]:checked`);
    return radio ? (radio.value === "true") : null;
}

function saveAnswer(patientId, questionId, boolValue, comment) {
    const dto = {
        PatientId: patientId,
        QuestionId: questionId,
        BoolValue: boolValue,
        Comment: comment,
        AssessmentId: globalAssessmentId
    };

    fetch(`/Patient/SaveOrUpdateAnswer`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(dto)
    })
        .then(async response => {
            if (!response.ok) {
                throw new Error('Erreur serveur');
            }
            return response.json();
        })
        .then(data => {
            RedFlagsStore.set(data.redflags);
            if (data.categories) {
                RedFlagsStore.setCategories(data.categories);
            }
            if (data.redflags >= criticalRedflagThreshold) {
                showCriticalRedFlagModal(patientId);
            }
        });
}

function updateGauge(newValue) {
    if (typeof moveNeedle === 'function') {
        moveNeedle(newValue);
    }
    const elem = document.getElementById('probabilityValue');
    if (elem) elem.textContent = newValue.toFixed(2) + '%';

    const eyeBadge = document.getElementById("eyeBadge");
    if (eyeBadge) {
        if (newValue >= criticalRedflagThreshold) {
            eyeBadge.classList.remove("d-none");
        } else {
            eyeBadge.classList.add("d-none");
        }
    }
}

function initializeTabs() {
    document.querySelectorAll('#redflagsTabs button[data-category]').forEach(btn => {
        btn.addEventListener('click', function (e) {
            e.preventDefault();

            document.querySelectorAll('#redflagsTabs button')
                .forEach(el => {
                    el.classList.remove('btn-primary');
                    el.classList.add('btn-outline-primary');
                });
            this.classList.remove('btn-outline-primary');
            this.classList.add('btn-primary');

            const categoryId = this.dataset.category;
            loadQuestionsByCategory(categoryId);
        });
    });
}

function initBackToTop() {
    const backToTopBtn = document.getElementById('backToTop');

    if (!backToTopBtn) return;

    window.addEventListener('scroll', function () {
        if (window.pageYOffset > 300) {
            backToTopBtn.style.opacity = '1';
            backToTopBtn.style.pointerEvents = 'auto';
        } else {
            backToTopBtn.style.opacity = '0';
            backToTopBtn.style.pointerEvents = 'none';
        }
    });

    backToTopBtn.addEventListener('click', function () {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    });
}

window.RedFlags = {
    init: function (patientId, assessmentId, folderId) {
        globalPatientId = patientId;
        globalAssessmentId = assessmentId;
        globalFolderId = folderId;

        document.addEventListener("DOMContentLoaded", function () {
            if (window.RedFlagsStore) {
                RedFlagsStore.clear();
            }

            initializeTabs();
            loadQuestionsByCategory(1);
            initializeGauge();
            fetchInitialState();
            initBackToTop();

            RedFlagsStore.subscribe(function (newValue) {
                updateGauge(newValue);
            });
        });
    }
};