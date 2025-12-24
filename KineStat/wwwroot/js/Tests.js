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

        if (result.success && result.clinicalCategories) {
            if (window.ClinicalProfileStore) {
                ClinicalProfileStore.set(result.clinicalCategories);
                console.log('✓ Radar chart 9 mis à jour:', result.clinicalCategories);
            }
        }
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

function calculateClusterProgress(clusterId) {
    const container = document.getElementById('testsContainer-' + clusterId);
    if (!container) return { completed: 0, total: 0, percentage: 0 };

    const tests = container.querySelectorAll('.test-card');
    const total = tests.length;
    const completed = container.querySelectorAll('.answered-badge[style*="inline-block"]').length;
    const percentage = total > 0 ? Math.round((completed / total) * 100) : 0;

    return { completed, total, percentage };
}

function updateClusterCard(clusterId) {
    const progress = calculateClusterProgress(clusterId);
    const card = document.querySelector(`[data-cluster-id="${clusterId}"]`);

    if (!card) return;

    const progressBadge = card.querySelector('.cluster-progress-badge');
    if (progressBadge) {
        progressBadge.textContent = `${progress.completed}/${progress.total}`;
    }

    const progressFill = card.querySelector('.cluster-progress-fill');
    if (progressFill) {
        progressFill.style.width = `${progress.percentage}%`;
    }

    const statusBadge = card.querySelector('.cluster-status-badge');
    const actionBtn = card.querySelector('.btn-cluster-action');

    if (progress.percentage === 0) {
        if (statusBadge) statusBadge.textContent = 'Non commencé';
        if (actionBtn) {
            actionBtn.textContent = '▶ Commencer';
            actionBtn.className = 'btn-cluster-action btn-start btn-tablet-lg';
        }
        card.classList.remove('has-progress', 'completed');
    } else if (progress.percentage === 100) {
        if (statusBadge) statusBadge.textContent = '✓ Complété';
        if (actionBtn) {
            actionBtn.textContent = '✓ Revoir les tests';
            actionBtn.className = 'btn-cluster-action btn-review btn-tablet-lg';
        }
        card.classList.remove('has-progress');
        card.classList.add('completed');
    } else {
        if (statusBadge) statusBadge.textContent = 'En cours';
        if (actionBtn) {
            actionBtn.textContent = '→ Continuer';
            actionBtn.className = 'btn-cluster-action btn-continue btn-tablet-lg';
        }
        card.classList.add('has-progress');
        card.classList.remove('completed');
    }
}

function toggleClusterTests(clusterId) {
    const collapseId = 'collapse-cluster-' + clusterId;
    const collapse = document.getElementById(collapseId);

    if (!collapse) return;

    const bsCollapse = new bootstrap.Collapse(collapse, {
        toggle: true
    });
}

function scrollToClusterTop(clusterId) {
    const card = document.querySelector(`[data-cluster-id="${clusterId}"]`);
    if (card) {
        card.scrollIntoView({
            behavior: 'smooth',
            block: 'start'
        });
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

function initTests(patientId, assessmentId, existingResponses) {
    globalPatientId = patientId;
    globalAssessmentId = assessmentId;

    document.addEventListener('DOMContentLoaded', function () {
        loadExistingResponses(existingResponses);

        const originalUpdateProgress = window.updateProgress;
        window.updateProgress = function () {
            if (originalUpdateProgress) {
                originalUpdateProgress();
            }

            document.querySelectorAll('[data-cluster-id]').forEach(card => {
                const clusterId = card.getAttribute('data-cluster-id');
                updateClusterCard(clusterId);
            });
        };

        window.addEventListener('load', function () {
            setTimeout(function () {
                document.querySelectorAll('[data-cluster-id]').forEach(card => {
                    const clusterId = card.getAttribute('data-cluster-id');
                    updateClusterCard(clusterId);
                });
            }, 100);
        });
    });
}

window.Tests = {
    init: initTests
};