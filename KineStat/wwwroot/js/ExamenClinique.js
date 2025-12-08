// ========================================
// EXAMEN CLINIQUE - JavaScript Complet
// Support : assessmentId + QuestionLadder avec slider
// ========================================

let allQuestionsData = {};
let currentCategory = 'Articulaire / Structurel';
let userResponses = {};
let globalPatientId = null;
let globalAssessmentId = null;

// Mapping des suggestions de tests selon les réponses
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

// ========================================
// INITIALISATION DES ONGLETS
// ========================================

function initializeTabs() {
    document.querySelectorAll('button[data-category]').forEach(btn => {
        btn.addEventListener('click', async function (e) {
            e.preventDefault();

            // Sauvegarder automatiquement avant de changer de catégorie
            await autoSaveResponses();

            // Changer le style des boutons
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

// ========================================
// CHARGEMENT DES DONNÉES
// ========================================

async function loadQuestionnaire() {
    try {
        const response = await fetch(`/ClinicalExam/GetQuestionsClinique/${globalPatientId}`);
        if (!response.ok) {
            throw new Error('Erreur lors du chargement des questions');
        }
        allQuestionsData = await response.json();
        console.log('=== Questions chargées ===');
        console.log('Catégories disponibles:', Object.keys(allQuestionsData));
        for (const cat in allQuestionsData) {
            console.log(`  ${cat}: ${allQuestionsData[cat].length} questions`);
        }

        // Charger les réponses existantes pour ce bilan (assessmentId)
        await loadExistingResponses();

        // Masquer les catégories vides
        hideEmptyCategories();

        // Trouver la première catégorie disponible
        const firstCategory = Object.keys(allQuestionsData)[0] || 'Articulaire / Structurel';

        // ✅ Mettre à jour la catégorie courante
        currentCategory = firstCategory;

        // Activer le bouton de la première catégorie
        document.querySelectorAll('button[data-category]').forEach(btn => {
            if (btn.dataset.category === firstCategory) {
                btn.classList.remove('btn-outline-secondary');
                btn.classList.add('btn-primary');
            }
        });

        displayQuestions(firstCategory);

        // ✅ SOLUTION : Forcer le refresh après que le DOM soit complètement rendu
        requestAnimationFrame(() => {
            requestAnimationFrame(() => {
                console.log('=== Refresh forcé après rendu DOM ===');
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
        // ✅ Charger les réponses pour ce patient ET ce bilan spécifique
        const url = `/ClinicalExam/GetExistingResponses/${globalPatientId}?assessmentId=${globalAssessmentId}`;
        const response = await fetch(url);
        if (!response.ok) return;

        const existingData = await response.json();
        console.log('=== Réponses existantes chargées ===', existingData);
        console.log('AssessmentId:', globalAssessmentId);
        console.log('Nombre de réponses:', existingData.length);

        // Organiser les réponses par catégorie
        existingData.forEach(resp => {
            const category = resp.categoryName;
            console.log(`Chargement réponse: QuestionId=${resp.questionId}, Catégorie="${category}", Valeur="${resp.responseValue}"`);

            if (!userResponses[category]) {
                userResponses[category] = {};
            }
            userResponses[category][resp.questionId] = resp.responseValue;
            if (resp.observations) {
                userResponses[category][resp.questionId + '_notes'] = resp.observations;
            }
        });

        console.log('Réponses organisées par catégorie:', userResponses);
        console.log('Catégories avec réponses:', Object.keys(userResponses));
    } catch (error) {
        console.warn('Impossible de charger les réponses existantes:', error);
    }
}

function hideEmptyCategories() {
    console.log('=== Masquage des catégories vides ===');

    const loadedCategories = Object.keys(allQuestionsData);
    console.log('Catégories avec questions:', loadedCategories);

    document.querySelectorAll('button[data-category]').forEach(btn => {
        const category = btn.dataset.category;

        if (!loadedCategories.includes(category)) {
            const parentCol = btn.closest('.col-6, .col-md-3, .col-xl');
            if (parentCol) {
                parentCol.style.display = 'none';
                console.log(`Catégorie "${category}" masquée (aucune question)`);
            }
        } else {
            const parentCol = btn.closest('.col-6, .col-md-3, .col-xl');
            if (parentCol) {
                parentCol.style.display = '';
                console.log(`Catégorie "${category}" affichée (${allQuestionsData[category].length} questions)`);
            }
        }
    });
}

// ========================================
// SAUVEGARDE AUTOMATIQUE
// ========================================

async function autoSaveResponses() {
    const responses = [];

    document.querySelectorAll('.card[data-question-id]').forEach(card => {
        const questionId = parseInt(card.dataset.questionId);
        const selectedRadio = card.querySelector('input[type="radio"]:checked');
        const numberInput = card.querySelector('input[type="number"]');
        const notesTextarea = card.querySelector('textarea');

        let responseValue = null;

        // Vérifier d'abord si c'est un radio button (bool/qcm)
        if (selectedRadio) {
            responseValue = selectedRadio.value;
        }
        // Sinon vérifier si c'est un input numérique (ladder)
        else if (numberInput && numberInput.value !== '') {
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
                assessmentId: globalAssessmentId,  // ✅ Inclure assessmentId
                responses: responses
            })
        });

        const result = await response.json();

        if (result.success) {
            console.log(`✓ ${responses.length} réponse(s) sauvegardée(s) automatiquement`);
        }
    } catch (error) {
        console.error('Erreur sauvegarde automatique:', error);
    }
}

// ========================================
// SAUVEGARDE MANUELLE
// ========================================

async function saveResponses() {
    const responses = [];

    document.querySelectorAll('.card[data-question-id]').forEach(card => {
        const questionId = parseInt(card.dataset.questionId);
        const selectedRadio = card.querySelector('input[type="radio"]:checked');
        const numberInput = card.querySelector('input[type="number"]');
        const notesTextarea = card.querySelector('textarea');

        let responseValue = null;

        // Vérifier d'abord si c'est un radio button (bool/qcm)
        if (selectedRadio) {
            responseValue = selectedRadio.value;
        }
        // Sinon vérifier si c'est un input numérique (ladder)
        else if (numberInput && numberInput.value !== '') {
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
                assessmentId: globalAssessmentId,  // ✅ Inclure assessmentId
                responses: responses
            })
        });

        const result = await response.json();

        if (result.success) {
            alert(`✓ ${responses.length} réponse(s) enregistrée(s) avec succès!`);
            // Redirection avec assessmentId
            window.location.href = `/Tests/Tests/${globalPatientId}?assessmentId=${globalAssessmentId}`;
        } else {
            alert('❌ Erreur lors de l\'enregistrement: ' + result.message);
        }
    } catch (error) {
        console.error('Erreur:', error);
        alert('❌ Erreur de connexion au serveur');
    }
}

// ========================================
// AFFICHAGE DES QUESTIONS
// ========================================

function displayQuestions(category) {
    console.log(`=== displayQuestions appelée avec: "${category}" ===`);

    const container = document.getElementById('questionnaireContainer');
    container.innerHTML = '';

    let questionsToDisplay = [];

    if (allQuestionsData[category]) {
        questionsToDisplay.push({ category: category, questions: allQuestionsData[category] });
    } else {
        console.warn(`Catégorie "${category}" non trouvée. Catégories disponibles:`, Object.keys(allQuestionsData));
    }

    if (questionsToDisplay.length === 0) {
        container.innerHTML = `
            <div class="alert alert-info" role="alert">
                <i class="bi bi-info-circle me-2"></i>
                Aucune question disponible pour cette catégorie.
                <br><small>Catégories disponibles: ${Object.keys(allQuestionsData).join(', ')}</small>
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

    console.log('Questions affichées, userResponses avant refresh:', userResponses);
    console.log('userResponses pour cette catégorie:', userResponses[category]);

    // Recharger les suggestions pour cette catégorie
    refreshSuggestionsForCategory(category);
}

// ========================================
// CRÉATION DES CARTES DE QUESTIONS
// ========================================

function createQuestionCard(question, uniqueId, category) {
    const card = document.createElement('div');
    card.className = 'card border-0 shadow-sm mb-3';
    card.dataset.questionId = question.id;
    card.dataset.category = category;

    const cardBody = document.createElement('div');
    cardBody.className = 'card-body p-3 p-md-4';

    // Titre de la question
    const questionText = document.createElement('p');
    questionText.className = 'fw-medium mb-3';
    questionText.textContent = question.question;
    cardBody.appendChild(questionText);

    // ✅ NOUVEAU : Détection du type de question
    if (question.type === 'ladder') {
        // ========================================
        // QUESTION À ÉCHELLE (SLIDER)
        // ========================================
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

        // Event listeners pour synchroniser input et slider
        const inputElem = card.querySelector(`#input_${uniqueId}`);
        const rangeElem = card.querySelector(`#range_${uniqueId}`);
        const displayElem = card.querySelector(`#display_${uniqueId}`);

        // Vérifier si une réponse existe déjà
        if (userResponses[category] && userResponses[category][question.id]) {
            const savedValue = userResponses[category][question.id];
            inputElem.value = savedValue;
            rangeElem.value = savedValue;
            displayElem.textContent = savedValue;
        }

        inputElem.addEventListener('input', function () {
            rangeElem.value = this.value;
            displayElem.textContent = this.value;
            // ✅ FIX FINAL: Capturer depuis le DOM
            const cardCategory = card.dataset.category;
            saveResponseLocally(question.id, this.value, cardCategory);
        });

        rangeElem.addEventListener('input', function () {
            inputElem.value = this.value;
            displayElem.textContent = this.value;
            // ✅ FIX FINAL: Capturer depuis le DOM
            const cardCategory = card.dataset.category;
            saveResponseLocally(question.id, this.value, cardCategory);
        });

    } else if (question.options && question.options.length > 0) {
        // ========================================
        // QUESTIONS OUI/NON OU QCM (BOUTONS)
        // ========================================
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

            // Vérifier si cette question a déjà une réponse stockée
            if (userResponses[category] && userResponses[category][question.id] === opt) {
                radioInput.checked = true;
            }

            // Ajouter event listener
            radioInput.addEventListener('change', function () {
                // ✅ FIX FINAL: Capturer la catégorie depuis la carte elle-même
                const cardCategory = card.dataset.category;
                console.log('Radio button changé, catégorie de la carte:', cardCategory);
                console.log('currentCategory globale:', currentCategory);
                updateSuggestions(cardCategory, opt, question.id);
                saveResponseLocally(question.id, opt, cardCategory);
            });

            optionsDiv.appendChild(wrapper);
        });

        cardBody.appendChild(optionsDiv);
    }

    // Bouton pour ajouter une note
    const toggleBtn = document.createElement('button');
    toggleBtn.type = 'button';
    toggleBtn.className = 'btn btn-sm btn-outline-secondary fw-medium';
    toggleBtn.innerHTML = '<i class="bi bi-pencil me-1"></i>Ajouter une note';
    cardBody.appendChild(toggleBtn);

    // Conteneur des notes
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

    // Pré-remplir les notes si elles existent
    if (userResponses[category] && userResponses[category][question.id + '_notes']) {
        textarea.value = userResponses[category][question.id + '_notes'];
        notesContainer.style.display = 'block';
        toggleBtn.innerHTML = '<i class="bi bi-eye-slash me-1"></i>Masquer la note';
    }

    // Sauvegarder les notes lors de la saisie
    textarea.addEventListener('input', function () {
        if (!userResponses[category]) {
            userResponses[category] = {};
        }
        userResponses[category][question.id + '_notes'] = this.value;
    });

    cardBody.appendChild(notesContainer);

    // Toggle des notes
    toggleBtn.addEventListener('click', () => {
        if (notesContainer.style.display === 'none') {
            notesContainer.style.display = 'block';
            toggleBtn.innerHTML = '<i class="bi bi-eye-slash me-1"></i>Masquer la note';
        } else {
            notesContainer.style.display = 'none';
            toggleBtn.innerHTML = '<i class="bi bi-pencil me-1"></i>Ajouter une note';
        }
    });

    // Si la carte n'a pas encore été ajoutée (pour les types non-ladder)
    if (!card.contains(cardBody)) {
        card.appendChild(cardBody);
    }

    return card;
}

// ========================================
// GESTION DES RÉPONSES LOCALES
// ========================================

function saveResponseLocally(questionId, response, category) {
    if (!userResponses[category]) {
        userResponses[category] = {};
    }
    userResponses[category][questionId] = response;
}

// ========================================
// SUGGESTIONS DE TESTS
// ========================================

function updateSuggestions(category, response, questionId) {
    console.log(`=== updateSuggestions appelée ===`);
    console.log('Catégorie:', category);
    console.log('Réponse:', response);
    console.log('QuestionId:', questionId);
    console.log('currentCategory (globale):', currentCategory);

    if (!userResponses[category]) {
        userResponses[category] = {};
    }
    userResponses[category][questionId] = response;

    console.log('userResponses après sauvegarde:', userResponses[category]);

    // Compter uniquement les "Oui" (exclure les notes)
    const yesCount = Object.entries(userResponses[category])
        .filter(([key, value]) => !key.includes('_notes'))
        .filter(([key, value]) => typeof value === 'string' && value === 'Oui')
        .length;

    console.log('Nombre de "Oui" calculé:', yesCount);
    displaySuggestions(category, yesCount);
}

function refreshSuggestionsForCategory(category) {
    console.log(`=== Refresh suggestions pour: ${category} ===`);
    console.log('userResponses[category]:', userResponses[category]);

    if (userResponses[category]) {
        // Filtrer les réponses (exclure les clés "_notes")
        const yesCount = Object.entries(userResponses[category])
            .filter(([key, value]) => !key.includes('_notes')) // Exclure les notes
            .filter(([key, value]) => typeof value === 'string' && value === 'Oui')
            .length;

        console.log('Nombre de "Oui":', yesCount);
        displaySuggestions(category, yesCount);
    } else {
        console.log('Aucune réponse trouvée pour cette catégorie');
        displaySuggestions(category, 0);
    }
}

function displaySuggestions(category, yesCount) {
    console.log(`=== displaySuggestions appelée ===`);
    console.log('Catégorie:', category);
    console.log('YesCount:', yesCount);
    console.log('testSuggestions pour cette catégorie:', testSuggestions[category]);

    const container = document.getElementById('suggestedTestsContainer');

    if (yesCount > 0 && testSuggestions[category] && testSuggestions[category]['Oui']) {
        const tests = testSuggestions[category]['Oui'];

        console.log('✅ Affichage des suggestions:', tests.length, 'tests');

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
        console.log('❌ Pas de suggestions à afficher');
        if (yesCount === 0) console.log('Raison: yesCount = 0');
        if (!testSuggestions[category]) console.log('Raison: testSuggestions[category] undefined');
        if (testSuggestions[category] && !testSuggestions[category]['Oui']) console.log('Raison: testSuggestions[category]["Oui"] undefined');

        container.innerHTML = `
            <p class="text-muted small mb-0">
                <i class="bi bi-info-circle me-2"></i>
                Répondez aux questions pour voir les tests recommandés
            </p>
        `;
    }
}

// ========================================
// EXPORT ET INITIALISATION
// ========================================

window.ExamenClinique = {
    init: function (patientId, assessmentId) {
        globalPatientId = patientId;
        globalAssessmentId = assessmentId;

        document.addEventListener('DOMContentLoaded', function () {
            initializeTabs();
            loadQuestionnaire();
        });

        // Exposer les fonctions pour les boutons HTML
        window.saveResponses = saveResponses;
        window.autoSaveResponses = autoSaveResponses;
    }
};