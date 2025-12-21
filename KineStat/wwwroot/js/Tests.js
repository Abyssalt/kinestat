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

    const valueInput = document.querySelector(`[name="test-${questionId}-value"]:checked`) ||
        document.querySelector(`[name="test-${questionId}-value"]`);
    const observationsInput = document.querySelector(`[name="test-${questionId}-observations"]`);

    if (!valueInput || !valueInput.value) {
        return;
    }

    const data = {
        PatientId: globalPatientId,
        AssessmentId: globalAssessmentId,
        Tests: [{
            Id: questionId,
            Value: valueInput.value,
            Observations: observationsInput?.value || null
        }]
    };

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


function loadExistingResponses(responses) {

    let loadedCount = 0;

    responses.forEach(response => {

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