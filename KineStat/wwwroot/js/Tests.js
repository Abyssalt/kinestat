let saveTimeout = null;
let globalPatientId = null;
let globalAssessmentId = null;

/**
 * Triggers an automatic save after a 500ms delay when user interacts with a test question.
 * Debounces rapid changes to avoid excessive API calls.
 * @param {number} questionId - The ID of the question being answered
 */
function autoSaveTest(questionId) {
    if (saveTimeout) {
        clearTimeout(saveTimeout);
    }

    saveTimeout = setTimeout(() => {
        saveTestData(questionId);
    }, 500);
}

/**
 * Saves a single test response to the server via POST request.
 * Validates input exists before sending. Updates clinical profile chart on success.
 * @param {number} questionId - The ID of the question to save
 * @returns {Promise<void>}
 */
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


/**
 * Marks a test question as answered by showing success badge and green border.
 * Also triggers global progress bar update.
 * @param {number} questionId - The ID of the question to mark as answered
 */
function markAsAnswered(questionId) {
    const badge = document.getElementById('badge-' + questionId);
    const card = document.getElementById('test-' + questionId);
    if (badge && card) {
        badge.style.display = 'inline-block';
        card.classList.add('border-success', 'border-2');
        updateProgress();
    }
}

/**
 * Toggles the visibility of the observations textarea for a specific question.
 * Uses Bootstrap's 'show' class for collapse animation.
 * @param {number} questionId - The ID of the question whose observations to toggle
 */
function toggleObservations(questionId) {
    const observationsZone = document.getElementById('observations-' + questionId);
    if (observationsZone) observationsZone.classList.toggle('show');
}

/**
 * Synchronizes a range slider with its corresponding number input and display badge.
 * Ensures all three elements show the same value when slider is moved.
 * @param {HTMLInputElement} range - The range input element being moved
 * @param {string} inputId - DOM ID of the number input to sync
 * @param {string} displayId - DOM ID of the display badge to sync
 */
function syncInputs(range, inputId, displayId) {
    const input = document.getElementById(inputId);
    const display = document.getElementById(displayId);
    if (input && display) {
        input.value = range.value;
        display.textContent = range.value;
    }
}

/**
 * Synchronizes a range slider from a number input value change.
 * Reverse operation of syncInputs - updates slider when number is typed.
 * @param {HTMLInputElement} input - The number input element being changed
 * @param {string} rangeId - ID of the range slider to sync
 * @param {string} displayId -  ID of the display badge to sync
 */
function updateRangeFromInput(input, rangeId, displayId) {
    const range = document.getElementById(rangeId);
    const display = document.getElementById(displayId);
    if (range && display) {
        range.value = input.value;
        display.textContent = input.value;
    }
}

/**
 * Updates the global progress bar showing overall test completion percentage.
 * Counts answered tests vs total tests and updates UI elements accordingly.
 */
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

/**
 * Calculates completion statistics for a specific test cluster.
 * Returns object with completed count, total count, and percentage.
 * @param {number} clusterId - The ID of the cluster to calculate progress for
 * @returns {{completed: number, total: number, percentage: number}} Progress statistics
 */
function calculateClusterProgress(clusterId) {
    const container = document.getElementById('testsContainer-' + clusterId);
    if (!container) return { completed: 0, total: 0, percentage: 0 };

    const tests = container.querySelectorAll('.test-card');
    const total = tests.length;
    const completed = container.querySelectorAll('.answered-badge[style*="inline-block"]').length;
    const percentage = total > 0 ? Math.round((completed / total) * 100) : 0;

    return { completed, total, percentage };
}

/**
 * Updates a cluster card's visual state based on completion progress.
 * Changes badges, progress bar, button text/color according to 0%, 1-99%, or 100% completion.
 * @param {number} clusterId - The ID of the cluster card to update
 */
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

/**
 * Toggles the collapse/expand state of tests within a cluster using Bootstrap Collapse.
 * Opens or closes the list of tests for a given cluster card.
 * @param {number} clusterId - The ID of the cluster to toggle
 */
function toggleClusterTests(clusterId) {
    const collapseId = 'collapse-cluster-' + clusterId;
    const collapse = document.getElementById(collapseId);

    if (!collapse) return;

    const bsCollapse = new bootstrap.Collapse(collapse, {
        toggle: true
    });
}

/**
 * Smoothly scrolls the page to bring a cluster card to the top of viewport.
 * Used by "back to top" buttons within expanded clusters.
 * @param {number} clusterId - The ID of the cluster card to scroll to
 */
function scrollToClusterTop(clusterId) {
    const card = document.querySelector(`[data-cluster-id="${clusterId}"]`);
    if (card) {
        card.scrollIntoView({
            behavior: 'smooth',
            block: 'start'
        });
    }
}

/**
 * Populates the test form with previously saved responses from the database.
 * Handles different input types (radio, number, select, textarea) and marks them as answered.
 * Also loads saved observations if present.
 * @param {Array<Object>} responses - Array of response objects containing questionId, value, and observations
 */
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

/**
 * Initializes the Tests module by setting global IDs and attaching event listeners.
 * Loads existing responses, sets up progress update hooks, and initializes cluster cards.
 * Should be called once on page load.
 * @param {number} patientId - The ID of the current patient
 * @param {number} assessmentId - The ID of the current assessment
 * @param {Array<Object>} existingResponses - Array of previously saved test responses
 */
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