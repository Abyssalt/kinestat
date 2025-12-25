let allQuestionsData = {};
let currentCategory = 'Articulaire / Structurel';
let userResponses = {};
let globalPatientId = null;
let globalAssessmentId = null;
let saveTimeout = null;


/**
 * Initializes category tab buttons with click handlers.
 * Switches active tab styling and displays corresponding questions when clicked.
 */
function initializeTabs() {
    document.querySelectorAll('button[data-category]').forEach(btn => {
        btn.addEventListener('click', async function (e) {
            e.preventDefault();

            document.querySelectorAll('button[data-category]').forEach(el => {
                el.classList.remove('btn-primary');
                el.classList.add('btn-outline-secondary');
            });

            this.classList.remove('btn-outline-secondary');
            this.classList.add('btn-primary');

            const category = this.dataset.category;
            currentCategory = category;
            displayQuestions(category);
        });
    });
}

/**
 * Fetches clinical exam questions from the server and initializes the questionnaire.
 * Loads existing responses, hides empty categories, and displays the first category.
 * Shows error message if fetch fails.
 * @returns {Promise<void>}
 */
async function loadQuestionnaire() {
    try {
        const response = await fetch(`/ClinicalExam/GetQuestionsClinique/${globalPatientId}`);
        if (!response.ok) {
            throw new Error('Erreur lors du chargement des questions');
        }
        allQuestionsData = await response.json();

        await loadExistingResponses();
        hideEmptyCategories();

        const firstCategory = Object.keys(allQuestionsData)[0] || 'Articulaire / Structurel';
        currentCategory = firstCategory;

        document.querySelectorAll('button[data-category]').forEach(btn => {
            if (btn.dataset.category === firstCategory) {
                btn.classList.remove('btn-outline-secondary');
                btn.classList.add('btn-primary');
            }
        });

        displayQuestions(firstCategory);

    } catch (error) {
        console.error('Erreur:', error);
        document.getElementById('questionnaireContainer').innerHTML = `
            <div class="alert alert-danger" role="alert">
                <i class="bi bi-exclamation-triangle me-2"></i>
                Erreur lors du chargement des questions. Veuillez réessayer.
            </div>
        `;
    }
}

/**
 * Loads previously saved responses from the server for the current assessment.
 * Populates userResponses object with values and notes by category and question ID.
 * Silently fails if no responses exist or fetch fails.
 * @returns {Promise<void>}
 */
async function loadExistingResponses() {
    try {
        const url = `/ClinicalExam/GetExistingResponses/${globalPatientId}?assessmentId=${globalAssessmentId}`;
        const response = await fetch(url);
        if (!response.ok) return;

        const existingData = await response.json();

        existingData.forEach(resp => {
            const category = resp.categoryName;

            if (!userResponses[category]) {
                userResponses[category] = {};
            }
            userResponses[category][resp.questionId] = resp.responseValue;
            if (resp.observations) {
                userResponses[category][resp.questionId + '_notes'] = resp.observations;
            }
        });
    } catch (error) {
        console.warn('Impossible de charger les réponses existantes:', error);
    }
}

/**
 * Hides category buttons that have no questions loaded from the server.
 * Compares loaded categories against all category buttons and hides empty ones.
 */
function hideEmptyCategories() {
    const loadedCategories = Object.keys(allQuestionsData);

    document.querySelectorAll('button[data-category]').forEach(btn => {
        const category = btn.dataset.category;

        if (!loadedCategories.includes(category)) {
            const parentCol = btn.closest('.col-6, .col-md-3, .col-xl');
            if (parentCol) {
                parentCol.style.display = 'none';
            }
        } else {
            const parentCol = btn.closest('.col-6, .col-md-3, .col-xl');
            if (parentCol) {
                parentCol.style.display = '';
            }
        }
    });
}


/**
 * Saves all answered questions to the server via POST request.
 * Collects all responses and notes from the DOM, sends them in batch.
 * Updates clinical profile chart if server returns updated categories.
 * @returns {Promise<void>}
 */
async function autoSaveResponses() {
    const responses = [];

    document.querySelectorAll('.card[data-question-id]').forEach(card => {
        const questionId = parseInt(card.dataset.questionId);
        const selectedRadio = card.querySelector('input[type="radio"]:checked');
        const numberInput = card.querySelector('input[type="number"]');
        const notesTextarea = card.querySelector('textarea');

        let responseValue = null;

        if (selectedRadio) {
            responseValue = selectedRadio.value;
        } else if (numberInput && numberInput.value !== '') {
            responseValue = numberInput.value;
        }

        if (responseValue !== null) {
            responses.push({
                questionId: questionId,
                response: responseValue,
                notes: notesTextarea?.value || null
            });
        }
    });

    if (responses.length === 0) return;

    try {
        const response = await fetch('/ClinicalExam/SaveExamenClinique', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                patientId: globalPatientId,
                assessmentId: globalAssessmentId,
                responses: responses
            })
        });

        const data = await response.json();

        if (data.success) {         

            if (data.clinicalCategories && window.ClinicalProfileStore) {
                window.ClinicalProfileStore.set(data.clinicalCategories);               
            }
        }
    } catch (error) {
        console.error('Erreur:', error);
    }
}

/**
 * Redirects user to the physical tests page for the current patient and assessment.
 * Called when user clicks "Next" or "Save and Continue" button.
 * @returns {Promise<void>}
 */
async function saveResponses() {
    window.location.href = `/Patients/Tests/${globalPatientId}?assessmentId=${globalAssessmentId}`;
}


/**
 * Renders all questions for a given category into the question container.
 * Clears existing content and creates question cards dynamically.
 * Shows info message if no questions exist for the category.
 * @param {string} category - The name of the category to display
 */
function displayQuestions(category) {

    const container = document.getElementById('questionnaireContainer');
    container.innerHTML = '';

    let questionsToDisplay = [];

    if (allQuestionsData[category]) {
        questionsToDisplay.push({ category: category, questions: allQuestionsData[category] });
    }

    if (questionsToDisplay.length === 0) {
        container.innerHTML = `
            <div class="alert alert-info" role="alert">
                <i class="bi bi-info-circle me-2"></i>
                Aucune question disponible pour cette catégorie.
            </div>
        `;
        return;
    }

    questionsToDisplay.forEach(({ category: subCategory, questions }) => {
        questions.forEach((q, index) => {

            const questionCard = createQuestionCard(q, `${subCategory.replace(/\s+/g, '_')}_${index}`, subCategory);
            container.appendChild(questionCard);
        });
    });
}


/**
 * Creates a single question card DOM element with horizontal layout.
 * Generates different input types based on question type (ladder/radio).
 * Includes question text on left, response controls in middle, and notes on right.
 * Loads saved responses if they exist.
 * @param {Object} question - Question object containing id, question text, type, and options
 * @param {string} uniqueId - Unique identifier for DOM elements (sanitized category + index)
 * @param {string} category - Category name for saving responses
 * @returns {HTMLElement} Complete question card element ready to append to DOM
 */
function createQuestionCard(question, uniqueId, category) {
    uniqueId = uniqueId.replace(/[&\/]/g, '');
    const card = document.createElement('div');
    card.className = 'card border shadow-sm mb-3';
    card.style.borderWidth = '2px';
    card.dataset.questionId = question.id;
    card.dataset.category = category;

    const cardBody = document.createElement('div');
    cardBody.className = 'card-body p-3 p-md-4';

    const mainContainer = document.createElement('div');
    mainContainer.className = 'd-flex flex-column flex-lg-row align-items-start align-items-lg-center gap-3';

    const questionText = document.createElement('div');
    questionText.className = 'fw-medium flex-grow-1';
    questionText.textContent = question.question;
    mainContainer.appendChild(questionText);

    const responseContainer = document.createElement('div');
    responseContainer.className = 'flex-shrink-0';
    responseContainer.style.minWidth = '300px';

    if (question.type === 'ladder') {
        const min = parseInt(question.options[0]) || 0;
        const max = parseInt(question.options[question.options.length - 1]) || 10;

        responseContainer.innerHTML = `
            <div class="d-flex align-items-center gap-2">
                <input type="number"
                       class="form-control"
                       style="width: 80px;"
                       name="q_${uniqueId}_value"
                       id="input_${uniqueId}"
                       min="${min}"
                       max="${max}"
                       placeholder="${min}"
                       value="${min}">
                
                <input type="range"
                       class="form-range flex-grow-1"
                       id="range_${uniqueId}"
                       min="${min}"
                       max="${max}"
                       value="${min}"
                       step="1"
                       style="max-width: 200px;">
                
                <span class="badge bg-primary" id="display_${uniqueId}" style="min-width: 40px;">${min}</span>
            </div>
        `;

        mainContainer.appendChild(responseContainer);

        const inputElem = responseContainer.querySelector(`#input_${uniqueId}`);
        const rangeElem = responseContainer.querySelector(`#range_${uniqueId}`);
        const displayElem = responseContainer.querySelector(`#display_${uniqueId}`);

        if (userResponses[category] && userResponses[category][question.id]) {
            const savedValue = userResponses[category][question.id];
            inputElem.value = savedValue;
            rangeElem.value = savedValue;
            displayElem.textContent = savedValue;
        }

        inputElem.addEventListener('input', function () {
            rangeElem.value = this.value;
            displayElem.textContent = this.value;
            const cardCategory = card.dataset.category;
            saveResponseLocally(question.id, this.value, cardCategory);
        });

        rangeElem.addEventListener('input', function () {
            inputElem.value = this.value;
            displayElem.textContent = this.value;
            const cardCategory = card.dataset.category;
            saveResponseLocally(question.id, this.value, cardCategory);
        });

    } else if (question.options && question.options.length > 0) {
        const optionsDiv = document.createElement('div');
        optionsDiv.className = 'btn-group';
        optionsDiv.setAttribute('role', 'group');

        question.options.forEach(opt => {
            const safeOpt = opt.replace(/"/g, '&quot;');

            let iconClass = 'bi-circle';
            let btnColor = 'secondary';

            if (opt === 'Oui') {
                iconClass = 'bi-check-circle';
                btnColor = 'success';
            } else if (opt === 'Non') {
                iconClass = 'bi-x-circle';
                btnColor = 'danger';
            }

            const wrapper = document.createElement('div');
            wrapper.innerHTML = `
                <input type="radio" class="btn-check" name="q_${uniqueId}" id="q_${uniqueId}_${safeOpt}" value="${safeOpt}" autocomplete="off">
                <label class="btn btn-outline-${btnColor} fw-bold" for="q_${uniqueId}_${safeOpt}">
                    <i class="bi ${iconClass} me-2"></i>${opt.toUpperCase()}
                </label>
            `;

            const radioInput = wrapper.querySelector('input[type="radio"]');

            if (userResponses[category] && userResponses[category][question.id] === opt) {
                radioInput.checked = true;
            }

            radioInput.addEventListener('change', function () {
                const cardCategory = card.dataset.category;
                saveResponseLocally(question.id, opt, cardCategory);
            });

            optionsDiv.appendChild(wrapper);
        });

        responseContainer.appendChild(optionsDiv);
        mainContainer.appendChild(responseContainer);
    }

    const notesSection = document.createElement('div');
    notesSection.className = 'flex-shrink-0 d-flex align-items-center gap-2';
    notesSection.style.minWidth = '200px';

    const toggleBtn = document.createElement('button');
    toggleBtn.type = 'button';
    toggleBtn.className = 'btn btn-sm btn-outline-secondary';
    toggleBtn.innerHTML = '<i class="bi bi-pencil"></i>';
    toggleBtn.title = 'Ajouter/Modifier une note';

    const textarea = document.createElement('textarea');
    textarea.className = 'form-control form-control-sm';
    textarea.name = `notes_${uniqueId}`;
    textarea.rows = 1;
    textarea.placeholder = 'Note...';
    textarea.style.display = 'none';
    textarea.style.resize = 'vertical';
    textarea.style.minWidth = '150px';

    if (userResponses[category] && userResponses[category][question.id + '_notes']) {
        textarea.value = userResponses[category][question.id + '_notes'];
        textarea.style.display = 'block';
        toggleBtn.innerHTML = '<i class="bi bi-eye-slash"></i>';
    }

    textarea.addEventListener('input', function () {
        if (!userResponses[category]) {
            userResponses[category] = {};
        }
        userResponses[category][question.id + '_notes'] = this.value;

        if (saveTimeout) {
            clearTimeout(saveTimeout);
        }

        saveTimeout = setTimeout(() => {
            autoSaveResponses();
        }, 1000);
    });

    toggleBtn.addEventListener('click', () => {
        if (textarea.style.display === 'none') {
            textarea.style.display = 'block';
            toggleBtn.innerHTML = '<i class="bi bi-eye-slash"></i>';
            textarea.focus();
        } else {
            textarea.style.display = 'none';
            toggleBtn.innerHTML = '<i class="bi bi-pencil"></i>';
        }
    });

    notesSection.appendChild(toggleBtn);
    notesSection.appendChild(textarea);
    mainContainer.appendChild(notesSection);

    cardBody.appendChild(mainContainer);
    card.appendChild(cardBody);
    return card;
}


/**
 * Stores a question response locally and triggers auto-save after 500ms delay.
 * Debounces rapid changes to avoid excessive server calls.
 * @param {number} questionId - The ID of the question being answered
 * @param {string} response - The answer value (yes/no/number)
 * @param {string} category - The category name for organizing responses
 */
function saveResponseLocally(questionId, response, category) {
    if (!userResponses[category]) {
        userResponses[category] = {};
    }
    userResponses[category][questionId] = response;


    if (saveTimeout) {
        clearTimeout(saveTimeout);
    }

    saveTimeout = setTimeout(() => {
        autoSaveResponses();
    }, 500);
}




/**
 * Initializes the Clinical Exam module by setting global IDs and loading the questionnaire.
 * Sets up event listeners and exposes save functions to window object.
 * Should be called once on page load.
 */
window.ExamenClinique = {
    init: function (patientId, assessmentId) {
        globalPatientId = patientId;
        globalAssessmentId = assessmentId;

        document.addEventListener('DOMContentLoaded', function () {
            initializeTabs();
            loadQuestionnaire();
        });

        window.saveResponses = saveResponses;
        window.autoSaveResponses = autoSaveResponses;
    }
};