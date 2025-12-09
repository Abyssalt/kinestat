let allQuestionsData = {};
let currentCategory = 'Articulaire / Structurel';
let userResponses = {};
let globalPatientId = null;
let globalAssessmentId = null;
let saveTimeout = null;

const testSuggestions = {
    'Articulaire / Structurel': {
        'Oui': [
            'Test de mobilité active lombaire',
            'Test de mobilité passive lombaire',
            'Test de reproduction McKenzie',
            'Mesure expansion thoracique',
            'Test de Romberg',
            'Prone instability test'
        ]
    },
    'Myofascial': {
        'Oui': [
            'Palpation points gâchettes (Travell/Simons)',
            'Test de tension musculaire',
            'Test Sorensen',
            'Side-bridge test',
            'Test compression musculaire'
        ]
    },
    'Douleur Nociceptive': {
        'Oui': [
            'Test de provocation mécanique',
            'Évaluation douleur au repos vs mouvement',
            'Test de charge progressive'
        ]
    },
    'Douleur Neuropathique': {
        'Oui': [
            'Test SLR (Straight Leg Raise)',
            'Test Slump',
            'Évaluation réflexes ostéo-tendineux',
            'Test sensibilité (piqûre, toucher)',
            'Évaluation dermatomes L4/L5/S1',
            'Prone knee bending test',
            'Test Lasègue croisé'
        ]
    },
    'Douleur Nociplastique': {
        'Oui': [
            'Évaluation allodynie',
            'Test de sommation temporelle',
            'Évaluation seuil douleur'
        ]
    },
    'Controle Sensorimoteur': {
        'Oui': [
            'Tests proprioceptifs Luomajoki',
            'Test équilibre postural',
            'Test force isométrique',
            'Test endurance gainage',
            '6MWT (6-Minute Walk Test)',
            'Test pression biofeedback (40-44 mmHg)',
            'Règle prédiction clinique Hicks'
        ]
    },
    'Croyances & Cognition': {
        'Oui': [
            'Questionnaire Tampa (kinésiophobie)',
            'Questionnaire PCS (catastrophisme)',
            'Questionnaire PSEQ (auto-efficacité)',
            'Questionnaire Start Back / Orebro'
        ]
    },
    'Socio-environnemental': {
        'Oui': [
            'Questionnaire IPAQ (activité physique)',
            'Évaluation contraintes professionnelles',
            'Questionnaire MOS-SSS (soutien social)',
            'Évaluation satisfaction travail'
        ]
    },
    'Emotionnel / Affectif': {
        'Oui': [
            'Questionnaire HADS (anxiété/dépression)',
            'Questionnaire PSQI (qualité sommeil)',
            'Questionnaire SF-36 (qualité de vie)',
            'Évaluation tension émotionnelle'
        ]
    }
};

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

        requestAnimationFrame(() => {
            requestAnimationFrame(() => {
                refreshSuggestionsForCategory(currentCategory);
            });
        });
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

        await response.json();
    } catch (error) {
        console.error('Erreur sauvegarde automatique:', error);
    }
}

async function saveResponses() {
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

    if (responses.length === 0) {
        alert('Veuillez répondre à au moins une question avant d\'enregistrer.');
        return;
    }

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

        const result = await response.json();

        if (result.success) {
            alert(`✓ ${responses.length} réponse(s) enregistrée(s) avec succès!`);
            window.location.href = `/Tests/Tests/${globalPatientId}?assessmentId=${globalAssessmentId}`;
        } else {
            alert('❌ Erreur lors de l\'enregistrement: ' + result.message);
        }
    } catch (error) {
        console.error('Erreur:', error);
        alert('❌ Erreur de connexion au serveur');
    }
}

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

    refreshSuggestionsForCategory(category);
}

function createQuestionCard(question, uniqueId, category) {
    const card = document.createElement('div');
    card.className = 'card border-0 shadow-sm mb-3';
    card.dataset.questionId = question.id;
    card.dataset.category = category;

    const cardBody = document.createElement('div');
    cardBody.className = 'card-body p-3 p-md-4';

    const questionText = document.createElement('p');
    questionText.className = 'fw-medium mb-3';
    questionText.textContent = question.question;
    cardBody.appendChild(questionText);

    if (question.type === 'ladder') {
        const min = parseInt(question.options[0]) || 0;
        const max = parseInt(question.options[question.options.length - 1]) || 10;

        const ladderContainer = document.createElement('div');
        ladderContainer.className = 'mb-3';

        ladderContainer.innerHTML = `
            <label class="form-label fw-medium">Valeur (${min} - ${max})</label>
            <input type="number"
                   class="form-control form-control-lg mb-3"
                   name="q_${uniqueId}_value"
                   id="input_${uniqueId}"
                   min="${min}"
                   max="${max}"
                   placeholder="Entrez la valeur"
                   value="${min}">
            
            <label class="form-label small text-secondary">Ou utilisez le curseur :</label>
            <input type="range"
                   class="form-range"
                   id="range_${uniqueId}"
                   min="${min}"
                   max="${max}"
                   value="${min}"
                   step="1">
            <div class="d-flex justify-content-between align-items-center mt-2">
                <small class="text-secondary fw-semibold">${min}</small>
                <span class="badge bg-primary fs-5" id="display_${uniqueId}">${min}</span>
                <small class="text-secondary fw-semibold">${max}</small>
            </div>
        `;

        cardBody.appendChild(ladderContainer);
        card.appendChild(cardBody);

        const inputElem = card.querySelector(`#input_${uniqueId}`);
        const rangeElem = card.querySelector(`#range_${uniqueId}`);
        const displayElem = card.querySelector(`#display_${uniqueId}`);

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
        optionsDiv.className = 'btn-group w-100 mb-3';
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
            wrapper.className = 'flex-fill';
            wrapper.innerHTML = `
                <input type="radio" class="btn-check" name="q_${uniqueId}" id="q_${uniqueId}_${safeOpt}" value="${safeOpt}" autocomplete="off">
                <label class="btn btn-outline-${btnColor} fw-bold w-100" for="q_${uniqueId}_${safeOpt}">
                    <i class="bi ${iconClass} me-2"></i>${opt.toUpperCase()}
                </label>
            `;

            const radioInput = wrapper.querySelector('input[type="radio"]');

            if (userResponses[category] && userResponses[category][question.id] === opt) {
                radioInput.checked = true;
            }

            radioInput.addEventListener('change', function () {
                const cardCategory = card.dataset.category;
                updateSuggestions(cardCategory, opt, question.id);
                saveResponseLocally(question.id, opt, cardCategory);
            });

            optionsDiv.appendChild(wrapper);
        });

        cardBody.appendChild(optionsDiv);
    }

    const toggleBtn = document.createElement('button');
    toggleBtn.type = 'button';
    toggleBtn.className = 'btn btn-sm btn-outline-secondary fw-medium';
    toggleBtn.innerHTML = '<i class="bi bi-pencil me-1"></i>Ajouter une note';
    cardBody.appendChild(toggleBtn);

    const notesContainer = document.createElement('div');
    notesContainer.style.display = 'none';
    notesContainer.className = 'mt-3';

    const notesLabel = document.createElement('label');
    notesLabel.className = 'form-label fw-medium small text-secondary';
    notesLabel.textContent = 'Notes';
    notesContainer.appendChild(notesLabel);

    const textarea = document.createElement('textarea');
    textarea.className = 'form-control';
    textarea.name = `notes_${uniqueId}`;
    textarea.rows = 3;
    textarea.placeholder = 'Notes (optionnel)';
    notesContainer.appendChild(textarea);

    if (userResponses[category] && userResponses[category][question.id + '_notes']) {
        textarea.value = userResponses[category][question.id + '_notes'];
        notesContainer.style.display = 'block';
        toggleBtn.innerHTML = '<i class="bi bi-eye-slash me-1"></i>Masquer la note';
    }

    textarea.addEventListener('input', function () {
        if (!userResponses[category]) {
            userResponses[category] = {};
        }
        userResponses[category][question.id + '_notes'] = this.value;
    });

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

    cardBody.appendChild(notesContainer);

    toggleBtn.addEventListener('click', () => {
        if (notesContainer.style.display === 'none') {
            notesContainer.style.display = 'block';
            toggleBtn.innerHTML = '<i class="bi bi-eye-slash me-1"></i>Masquer la note';
        } else {
            notesContainer.style.display = 'none';
            toggleBtn.innerHTML = '<i class="bi bi-pencil me-1"></i>Ajouter une note';
        }
    });

    if (!card.contains(cardBody)) {
        card.appendChild(cardBody);
    }

    return card;
}

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

function updateSuggestions(category, response, questionId) {
    if (!userResponses[category]) {
        userResponses[category] = {};
    }
    userResponses[category][questionId] = response;

    const yesCount = Object.entries(userResponses[category])
        .filter(([key, value]) => !key.includes('_notes'))
        .filter(([key, value]) => typeof value === 'string' && value === 'Oui')
        .length;

    displaySuggestions(category, yesCount);

    autoSaveResponses();
}

function refreshSuggestionsForCategory(category) {
    if (userResponses[category]) {
        const yesCount = Object.entries(userResponses[category])
            .filter(([key, value]) => !key.includes('_notes'))
            .filter(([key, value]) => typeof value === 'string' && value === 'Oui')
            .length;

        displaySuggestions(category, yesCount);
    } else {
        displaySuggestions(category, 0);
    }
}

function displaySuggestions(category, yesCount) {
    const container = document.getElementById('suggestedTestsContainer');

    if (yesCount > 0 && testSuggestions[category] && testSuggestions[category]['Oui']) {
        const tests = testSuggestions[category]['Oui'];

        container.innerHTML = `
            <div class="mb-3">
                <p class="text-success fw-medium mb-2">
                    <i class="bi bi-check-circle me-2"></i>${yesCount} réponse(s) positive(s)
                </p>
                <p class="small text-muted mb-3">Tests physiques recommandés :</p>
            </div>
            <ul class="list-unstyled mb-0">
                ${tests.map(test => `
                    <li class="mb-2 pb-2 border-bottom">
                        <i class="bi bi-arrow-right-circle text-info me-2"></i>
                        <span class="small">${test}</span>
                    </li>
                `).join('')}
            </ul>
            <div class="mt-3 p-2 bg-light rounded">
                <small class="text-muted">
                    <i class="bi bi-info-circle me-1"></i>
                    Ces tests seront disponibles dans la page "Tests physiques"
                </small>
            </div>
        `;
    } else {
        container.innerHTML = `
            <p class="text-muted small mb-0">
                <i class="bi bi-info-circle me-2"></i>
                Répondez aux questions pour voir les tests recommandés
            </p>
        `;
    }
}

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