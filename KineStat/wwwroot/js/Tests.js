let customTestCounter = 1000;
let saveTimeout = null;
let globalPatientId = null;
let globalAssessmentId = null;

function autoSaveTest(questionId) {
    if (saveTimeout) {
        clearTimeout(saveTimeout);
    }

    saveTimeout = setTimeout(() => {
        saveTestData(questionId);
    }, 500);
}

async function saveTestData(questionId) {


    const card = document.getElementById('test-' + questionId);
    if (!card) {
      
        return;
    }

   

    const isCustom = card.getAttribute('data-custom') === 'true';
    const valueInput = document.querySelector(`[name="test-${questionId}-value"]:checked`) ||
        document.querySelector(`[name="test-${questionId}-value"]`);
    const observationsInput = document.querySelector(`[name="test-${questionId}-observations"]`);

    const hasValue = valueInput && valueInput.value;
    const hasObservations = observationsInput && observationsInput.value.trim();

    if (!valueInput || !valueInput.value) {
        return;
    }

    const data = {
        PatientId: globalPatientId,
        AssessmentId: globalAssessmentId,
        Tests: [{
            Id: questionId,
            Value: hasValue ? valueInput.value : '',
            Observations: observationsInput?.value || null,
            Custom: isCustom
        }]
    };


    if (isCustom) {
        const testName = card.querySelector('h5').textContent.replace(/\d+/, '').replace('Personnalisé', '').trim();
        let testType = 'text';
        const radioInput = card.querySelector('input[type="radio"]');
        const numberInput = card.querySelector('input[type="number"]');

        if (radioInput) testType = 'bool';
        else if (numberInput) testType = 'scale';

        data.Tests[0].Name = testName;
        data.Tests[0].Type = testType;
    }

    try {
        const response = await fetch('/Tests/SaveTestResults', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data)
        });

        const result = await response.json();

       
    } catch (error) {
        console.error('✗ Erreur sauvegarde automatique:', error);
    }
}

function toggleAddTestForm() {
    const form = document.getElementById('addTestForm');
    form.classList.toggle('show');
}

function addCustomTest() {
    const clusterId = document.getElementById('customTestCluster').value;
    const testName = document.getElementById('customTestName').value.trim();
    const testType = document.getElementById('customTestType').value;
    const testDescription = document.getElementById('customTestDescription').value.trim();

    if (!clusterId || !testName) {
        return;
    }

    const testId = customTestCounter++;
    const testIndex = document.querySelectorAll('.test-card').length + 1;

    let testHTML = `
    <div class="card border-0 shadow-sm mb-3 test-card" id="test-${testId}" data-test-id="${testId}" data-custom="true">
        <div class="card-header bg-light py-3">
            <div class="d-flex flex-column flex-md-row justify-content-between align-items-start align-items-md-center gap-2">
                <div class="flex-grow-1">
                    <h5 class="mb-2 fw-bold">
                        <span class="badge bg-primary me-2">${testIndex}</span>
                        ${testName}
                        <span class="badge bg-secondary ms-2">Personnalisé</span>
                    </h5>
                </div>
                <div class="d-flex flex-wrap gap-2 align-items-center">
                    ${testDescription ? `
                                <button type="button"
                                        class="btn btn-sm btn-outline-info fw-medium"
                                        onclick="toggleDescription(${testId})"
                                        title="Voir le protocole">
                                    <i class="bi bi-journal-medical me-1"></i>Protocole
                                </button>` : ''}
                    <button type="button"
                        class="btn btn-sm btn-outline-danger fw-medium"
                        onclick="removeCustomTest(${testId})"
                        title="Supprimer ce test">
                        <i class="bi bi-trash"></i>
                    </button>
                    <span class="badge bg-success answered-badge" id="badge-${testId}" style="display: none;">
                        <i class="bi bi-check-circle-fill me-1"></i>Complété
                    </span>
                </div>
            </div>
            ${testDescription ? `
                        <div class="alert alert-info border-0 mt-3 mb-0 collapse" id="description-${testId}">
                            <small>
                                <i class="bi bi-clipboard-check me-1"></i>
                                <strong>Protocole :</strong> ${testDescription}
                            </small>
                        </div>` : ''}
        </div>
        <div class="card-body p-3 p-md-4">
            `;

    if (testType === 'bool') {
        testHTML += `
                    <div class="mb-3">
                        <label class="form-label fw-medium">Résultat</label>
                        <div class="btn-group w-100" role="group">
                            <input type="radio"
                                   class="btn-check"
                                   name="test-${testId}-value"
                                   id="oui-${testId}"
                                   value="true"
                                   onchange="markAsAnswered(${testId}); autoSaveTest(${testId})">
                            <label class="btn btn-outline-success btn-lg fw-medium" for="oui-${testId}">
                                <i class="bi bi-check-circle-fill me-1"></i>Positif
                            </label>
                            <input type="radio"
                                   class="btn-check"
                                   name="test-${testId}-value"
                                   id="non-${testId}"
                                   value="false"
                                   onchange="markAsAnswered(${testId}); autoSaveTest(${testId})">
                            <label class="btn btn-outline-secondary btn-lg fw-medium" for="non-${testId}">
                                <i class="bi bi-x-circle-fill me-1"></i>Négatif
                            </label>
                        </div>
                    </div>
                `;
    } else if (testType === 'scale') {
        testHTML += `
                    <div class="mb-3">
                        <label class="form-label fw-medium">Valeur (0-10)</label>
                        <input type="number"
                               class="form-control form-control-lg mb-3"
                               name="test-${testId}-value"
                               min="0"
                               max="10"
                               placeholder="Entrez la valeur"
                               onchange="markAsAnswered(${testId}); updateRangeFromInput(this, 'range-${testId}', 'display-${testId}')"
                               oninput="autoSaveTest(${testId})"
                               id="input-${testId}">
                        <label class="form-label small text-secondary">Ou utilisez le curseur :</label>
                        <input type="range"
                               class="form-range"
                               id="range-${testId}"
                               min="0"
                               max="10"
                               value="0"
                               step="1"
                               oninput="syncInputs(this, 'input-${testId}', 'display-${testId}'); autoSaveTest(${testId})">
                        <div class="d-flex justify-content-between align-items-center mt-2">
                            <small class="text-secondary fw-semibold">0</small>
                            <span class="badge bg-primary fs-5" id="display-${testId}">0</span>
                            <small class="text-secondary fw-semibold">10</small>
                        </div>
                    </div>
                `;
    } else if (testType === 'text') {
        testHTML += `
                    <div class="mb-3">
                        <label class="form-label fw-medium">Observations</label>
                        <textarea class="form-control"
                                  name="test-${testId}-value"
                                  rows="4"
                                  placeholder="Notez vos observations..."
                                  onchange="markAsAnswered(${testId})"
                                  oninput="autoSaveTest(${testId})"></textarea>
                    </div>
                `;
    }

    testHTML += `
            <div class="mb-2">
                <button type="button"
                    class="btn btn-sm btn-outline-primary fw-medium"
                    onclick="toggleObservations(${testId})">
                    <i class="bi bi-chat-left-text me-1"></i>Ajouter des observations
                </button>
            </div>
            <div class="collapse" id="observations-${testId}">
                <div class="card border-0 bg-light mt-3">
                    <div class="card-body p-3">
                        <label class="form-label fw-medium">
                            <i class="bi bi-chat-left-dots me-1"></i>Observations complémentaires
                        </label>
                        <textarea class="form-control"
                            name="test-${testId}-observations"
                            rows="3"
                            placeholder="Notez ici vos observations..."
                            oninput="autoSaveTest(${testId})"></textarea>
                    </div>
                </div>
            </div>
        </div>
    </div>
    `;

    const clusterContainer = document.getElementById('testsContainer-' + clusterId);

    if (clusterContainer) {
        clusterContainer.insertAdjacentHTML('beforeend', testHTML);
        document.getElementById('customTestCluster').value = '';
        document.getElementById('customTestName').value = '';
        document.getElementById('customTestDescription').value = '';
        toggleAddTestForm();
        updateTestNumbers();
        updateProgress();
        alert(`✓ Test "${testName}" ajouté avec succès !`);
    } else {
        alert('Impossible de trouver le cluster.');
    }
}

function removeCustomTest(testId) {
    if (confirm('Êtes-vous sûr de vouloir supprimer ce test ?')) {
        document.getElementById('test-' + testId).remove();
        updateTestNumbers();
        updateProgress();
    }
}

function updateTestNumbers() {
    document.querySelectorAll('.test-card').forEach((card, index) => {
        const badge = card.querySelector('.badge.bg-primary');
        if (badge) badge.textContent = index + 1;
    });
}

function toggleDescription(questionId) {
    const description = document.getElementById('description-' + questionId);
    if (description) description.classList.toggle('show');
}

function markAsAnswered(questionId) {
    const badge = document.getElementById('badge-' + questionId);
    const card = document.getElementById('test-' + questionId);
    if (badge && card) {
        badge.style.display = 'inline-block';
        card.classList.add('border-success', 'border-2');
        updateProgress();
    }
}

function toggleObservations(questionId) {
    const observationsZone = document.getElementById('observations-' + questionId);
    if (observationsZone) observationsZone.classList.toggle('show');
}

function syncInputs(range, inputId, displayId) {
    const input = document.getElementById(inputId);
    const display = document.getElementById(displayId);
    if (input && display) {
        input.value = range.value;
        display.textContent = range.value;
    }
}

function updateRangeFromInput(input, rangeId, displayId) {
    const range = document.getElementById(rangeId);
    const display = document.getElementById(displayId);
    if (range && display) {
        range.value = input.value;
        display.textContent = input.value;
    }
}

function updateProgress() {
    const total = document.querySelectorAll('.test-card').length;
    const answered = document.querySelectorAll('.answered-badge[style*="inline-block"]').length;
    const percentage = total > 0 ? Math.round((answered / total) * 100) : 0;
    const progressBar = document.getElementById('progressBar');
    const progressBadge = document.getElementById('progressBadge');
    if (progressBar && progressBadge) {
        progressBar.style.width = percentage + '%';
        progressBar.textContent = percentage + '%';
        progressBar.setAttribute('aria-valuenow', percentage);
        progressBadge.textContent = answered + ' / ' + total;
    }
}

function resetForm() {
    if (confirm('Êtes-vous sûr de vouloir réinitialiser tous les tests ?')) {
        document.getElementById('formTests').reset();
        document.querySelectorAll('.answered-badge').forEach(badge => badge.style.display = 'none');
        document.querySelectorAll('.test-card').forEach(card => card.classList.remove('border-success', 'border-2'));
        document.querySelectorAll('.collapse').forEach(zone => zone.classList.remove('show'));
        updateProgress();
    }
}

function handleSubmit(event) {
    event.preventDefault();
    const answered = document.querySelectorAll('.answered-badge[style*="inline-block"]').length;
    if (answered === 0) {
        alert('⚠️ Veuillez répondre à au moins un test avant de valider.');
        return false;
    }
    window.location.href = `/Assessments/Resultat/${globalPatientId}?assessmentId=${globalAssessmentId}`;
    return false;
}

function saveData() {
    window.location.href = `/Assessments/Resultat/${globalPatientId}?assessmentId=${globalAssessmentId}`;
}

function loadExistingResponses(responses) {

    let loadedCount = 0;

    responses.forEach(response => {

        if (response.isCustom) {
            return;
        }

        const questionId = response.questionId;
        const card = document.getElementById('test-' + questionId);

        if (!card) {       
            return;
        }

        const valueString = String(response.value).toLowerCase();

        const radioInputTrue = card.querySelector(`input[name="test-${questionId}-value"][value="true"]`);
        const radioInputFalse = card.querySelector(`input[name="test-${questionId}-value"][value="false"]`);
        const numberInput = card.querySelector(`input[name="test-${questionId}-value"][type="number"]`);
        const rangeInput = card.querySelector(`input[type="range"]#range-${questionId}`);
        const selectInput = card.querySelector(`select[name="test-${questionId}-value"]`);
        const textareaInput = card.querySelector(`textarea[name="test-${questionId}-value"]`);

        if (radioInputTrue || radioInputFalse) {
           
            if (valueString === 'true' && radioInputTrue) {
                radioInputTrue.checked = true;
                markAsAnswered(questionId);
            } else if (valueString === 'false' && radioInputFalse) {
                radioInputFalse.checked = true;
                markAsAnswered(questionId);
            }
        } else if (numberInput) {
            if (response.value !== null && response.value !== '') {
                numberInput.value = response.value;
                if (rangeInput) {
                    rangeInput.value = response.value;
                    const displaySpan = card.querySelector(`#display-${questionId}`);
                    if (displaySpan) {
                        displaySpan.textContent = response.value;
                    }
                }
                markAsAnswered(questionId);
            }
        } else if (selectInput) {
            if (response.value !== null && response.value !== '') {
                selectInput.value = response.value;
                markAsAnswered(questionId);
            }
        } else if (textareaInput) {
            if (response.value !== null && response.value !== '') {
                textareaInput.value = response.value;
                markAsAnswered(questionId);
            }
        }

        if (response.observations) {
          
            const observationsTextarea = card.querySelector(`textarea[name="test-${questionId}-observations"]`);
            if (observationsTextarea) {
                observationsTextarea.value = response.observations;
                const observationsZone = document.getElementById('observations-' + questionId);
                if (observationsZone) {
                    observationsZone.classList.add('show');
                }
            }
        }

        loadedCount++;

    });

}