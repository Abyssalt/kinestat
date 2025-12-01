let customTestCounter = 1000;

    function toggleAddTestForm() {
            const form = document.getElementById('addTestForm');
    form.classList.toggle('show');
        }

    function addCustomTest() {
            const clusterId = document.getElementById('customTestCluster').value;
    const testName = document.getElementById('customTestName').value.trim();
    const testType = document.getElementById('customTestType').value;
    const testDescription = document.getElementById('customTestDescription').value.trim();

    if (!clusterId) {
        alert('⚠️ Veuillez sélectionner un cluster.');
    return;
            }

    if (!testName) {
        alert('⚠️ Veuillez entrer un nom pour le test.');
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
                                   onchange="markAsAnswered(${testId})">
                            <label class="btn btn-outline-success btn-lg fw-medium" for="oui-${testId}">
                                <i class="bi bi-check-circle-fill me-1"></i>Positif
                            </label>
                            <input type="radio"
                                   class="btn-check"
                                   name="test-${testId}-value"
                                   id="non-${testId}"
                                   value="false"
                                   onchange="markAsAnswered(${testId})">
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
                               id="input-${testId}">
                        <label class="form-label small text-secondary">Ou utilisez le curseur :</label>
                        <input type="range"
                               class="form-range"
                               id="range-${testId}"
                               min="0"
                               max="10"
                               value="0"
                               step="1"
                               oninput="syncInputs(this, 'input-${testId}', 'display-${testId}')">
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
                                  onchange="markAsAnswered(${testId})"></textarea>
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
                            placeholder="Notez ici vos observations..."></textarea>
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
        alert('❌ Erreur : Impossible de trouver le cluster.');
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

    function calculateRedFlags() {
        let scoreTotal = 0;
    const testsEffectues = [];

            // Parcours toutes les cartes de test
            document.querySelectorAll('.test-card').forEach(card => {
                const testId = card.getAttribute('data-test-id');
    const testTitle = card.getAttribute('data-test-title');
    const testRv = parseInt(card.getAttribute('data-test-rv'));
    const isCustom = card.getAttribute('data-custom') === 'true';

    // Ne pas inclure les tests personnalisés dans les red flags
    if (isCustom) return;

    const input = document.querySelector(`input[name="test-${testId}-value"]:checked, input[name="test-${testId}-value"]:not([type="radio"]), select[name="test-${testId}-value"]`);

    if (input && input.value) {
                    const value = input.value;
    let isPositive = false;

    // Détermine si le test est positif selon son type
    if (input.type === 'radio') {
        isPositive = value === 'true';
                    } else if (input.type === 'number') {
        isPositive = parseInt(value) >= 3;
                    } else if (input.tagName === 'SELECT') {
        isPositive = value !== 'Démarche normale' && value !== '';
                    }

    if (isPositive) {
        scoreTotal += testRv;
    testsEffectues.push({
        title: testTitle,
    score: testRv,
    value: value
                        });
                    }
                }
            });

    const redFlags = [
    {
        nom: "Syndrome de la Queue de Cheval",
    seuil: 15,
    niveau: "Urgence",
    couleur: "danger",
    description: "Compression médullaire nécessitant une intervention chirurgicale urgente",
    action: "ORIENTATION IMMÉDIATE VERS LES URGENCES"
                },
    {
        nom: "Atteinte Neurologique Sévère",
    seuil: 10,
    niveau: "Critique",
    couleur: "warning",
    description: "Déficit neurologique significatif nécessitant une évaluation médicale",
    action: "Consultation neurologique urgente sous 24h"
                },
    {
        nom: "Déficit Neurologique Modéré",
    seuil: 5,
    niveau: "Élevé",
    couleur: "info",
    description: "Signes neurologiques nécessitant une surveillance",
    action: "Consultation médicale sous 48-72h"
                }
    ];

            const redFlagsDetected = redFlags.filter(rf => scoreTotal >= rf.seuil);
    displayRedFlags(redFlagsDetected, scoreTotal, testsEffectues);
        }

    function displayRedFlags(redFlags, scoreTotal, testsEffectues) {
            const container = document.getElementById('redFlagsContainer');
    if (!container) return;

    let html = `
    <div class="alert ${redFlags.length > 0 ? 'alert-danger' : 'alert-success'} border-0">
        <h6 class="fw-bold">
            <i class="bi bi-${redFlags.length > 0 ? 'exclamation-triangle-fill' : 'check-circle-fill'} me-2"></i>
            Score total: <strong>${scoreTotal} points</strong>
        </h6>
        <p class="mb-0 small">Basé sur ${testsEffectues.length} test(s) positif(s)</p>
    </div>
    `;

            if (redFlags.length > 0) {
        html += '<div class="list-group list-group-flush mb-3">';
                redFlags.forEach(rf => {
                    const badgeClass = rf.niveau === 'Urgence' ? 'bg-danger' : rf.niveau === 'Critique' ? 'bg-warning text-dark' : 'bg-info';
    html += `
    <div class="list-group-item list-group-item-danger border-0 border-bottom px-0 py-3">
        <div class="d-flex w-100 justify-content-between align-items-start mb-2">
            <h6 class="mb-1 fw-bold"><i class="bi bi-flag-fill me-2"></i>${rf.nom}</h6>
            <span class="badge ${badgeClass}">${rf.niveau}</span>
        </div>
        <p class="mb-1 small">${rf.description}</p>
        <small class="text-danger">
            <i class="bi bi-arrow-right-circle me-1"></i><strong>Action:</strong> ${rf.action}
        </small>
    </div>
    `;
                });
    html += '</div>';
            } else {
    html += '<div class="alert alert-success border-0 mt-3"><i class="bi bi-check-circle me-2"></i>Aucun red flag détecté.</div>';
}

if (testsEffectues.length > 0) {
    html += '<h6 class="mt-3 fw-bold">Tests contributeurs:</h6><ul class="small text-secondary">';
    testsEffectues.forEach(test => html += `<li>${test.title}: +${test.score} points</li>`);
    html += '</ul>';
}

container.innerHTML = html;
        }

function handleSubmit(event) {
    event.preventDefault();
    const answered = document.querySelectorAll('.answered-badge[style*="inline-block"]').length;
    if (answered === 0) {
        alert('⚠️ Veuillez répondre à au moins un test avant de valider.');
        return false;
    }
    saveData();
    alert(`✓ Évaluation validée!\n\n${answered} test(s) complété(s).`);
    return false;
}
