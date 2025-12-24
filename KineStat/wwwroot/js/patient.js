//This file contains all functions related to patient

//Securtiy Social Number functions : 
/**
  * Update phone input with the country's prefix 
  */
function updatePhonePrefix() {
    const countrySelect = document.querySelector('.cache-field[data-field="Country"]');
    const phoneInput = document.querySelector('.cache-field[data-field="PhoneNumber"]');
    if (!countrySelect || !phoneInput) return;

    countrySelect.addEventListener('change', function () {
        const selectedCountry = this.value;
        const prefix = phoneCountryCodes[selectedCountry] || '';

        let currentNumber = phoneInput.value.trim();
        currentNumber = currentNumber.replace(/^\+\d+/, '').trim();
        phoneInput.value = prefix ? `${prefix} ${currentNumber}`.trim() : currentNumber;
    });
    if (countrySelect.value) {
        countrySelect.dispatchEvent(new Event('change'));
    }
}
/**
 * Validates a social security number based on the selected country
 * param :  value is the social security number to validate
 * param : country is the  country code ('0'=Belgium, '1'=France, '2'=Luxembourg, '3'=Switzerland)
 * return : True if the number is valid for the given country
 */
function validateSocialSecurityNumber(value, country) {
    if (!value) return false;

    const cleanNumber = value.replace(/[\s\.\-]/g, '');

    switch (country) {
        case '0': // Belgium
            return validateBelgianNISS(cleanNumber);
        case '1': // France
            return validateFrenchNIR(cleanNumber);
        case '2': // Luxembourg
            return validateLuxembourgNSS(cleanNumber);
        case '3': // Suisse
            return validateSwissAVS(cleanNumber);
        default:
            return false;
    }
}

/**
 * Validates Belgian National Register Number
 * Format: YY.MM.DD-XXX.XX (11 digits)
 * parma : vlue : clean number
 * return : True if valid
 *
 * Special cases: month can be 20 for unknown month (21) , 40 for adopted children or unknown gender (41-52)
 */
function validateBelgianNISS(value) {
    if (!/^\d{11}$/.test(value)) return false;
    const month = parseInt(value.substring(2, 4)), day = parseInt(value.substring(4, 6));
    const validMonth = (month >= 1 && month <= 12) || (month >= 21 && month <= 32) || (month >= 41 && month <= 52);
    if (!validMonth || day < 1 || day > 31) return false;
    let base = parseInt(value.substring(0, 9)), check = parseInt(value.substring(9, 11)), calc = 97 - (base % 97);
    if (calc !== check) calc = 97 - (parseInt('2' + value.substring(0, 9)) % 97);
    return calc === check;
}

/**
  * Validates French Social Security Number
  * Format: S YY MM DD XXX XXX XX (15 digits)
  * param : number cleaned number (digits only, or with 2A/2B for Corsica)
  * return : True if valid
  *
  * Special cases:
  * Sex: 1=male, 2=female, 7=temporary waiting for assignment
  * Month can be:
  * 01-12: Standard months
  * 20: Unknown month (foreign birth)
  * 30-42: Exceptional administrative cases
  * Department: 2A or 2B for Corsica (Corse-du-Sud and Haute-Corse)
  */
function validateFrenchNIR(number) {
    if (!/^\d{15}$/.test(number)) return false;

    const sex = parseInt(number.substring(0, 1));
    const year = parseInt(number.substring(1, 3));
    const month = parseInt(number.substring(3, 5));
    const department = number.substring(5, 7);

    if (![1, 2, 7].includes(sex)) return false;

    const validMonth = (month >= 1 && month <= 12) ||
        month === 20 ||
        (month >= 30 && month <= 42);

    if (!validMonth) return false;

    // Validate department (2 digits or 2A/2B for Corsica)
    if (!/^\d{2}$/.test(department) && department !== '2A' && department !== '2B') {
        return false;
    }

    return true;
}

/**
  * validates Luxembourg Social Security Number
  * format: YYYY MM DD XXX (13 digits)
  * param : number : cleaned number (digits only)
  * return : True if valid
  *
  * Structure: Birth date (YYYYMMDD) + sequential number (XXX) + check digits (XX)
  * Year range: 1800 to current year + 1 (to allow for future births)
  */
function validateLuxembourgNSS(number) {
    if (!/^\d{13}$/.test(number)) return false;

    const year = parseInt(number.substring(0, 4));
    const month = parseInt(number.substring(4, 6));
    const day = parseInt(number.substring(6, 8));
    const currentYear = new Date().getFullYear();

    if (year < 1800 || year > currentYear + 1) return false;
    if (month < 1 || month > 12) return false;
    if (day < 1 || day > 31) return false;

    return true;
}

/**
  * Validates Swiss AVS Number
  * format: 756.XXXX.XXXX.XX (13 digits)
  * param : number : cleaned number (digits only)
  * return : True if valid
  *
  * Special cases:
  * All Swiss AVS numbers start with the country code 756
  */
function validateSwissAVS(number) {
    if (!/^\d{13}$/.test(number)) return false;
    if (!number.startsWith('756')) return false;

    return true;
}


/**
 * this function returns the appropriate placeholder text based on the selected country
 * param : country :  country code ('0'=Belgium, '1'=France, '2' =Luxembourg, '3'= Suisse)
 * return : placeholder text for the input field
 */
function getSocialSecurityPlaceholder(country) {
    switch (country) {
        case '0':
            return 'AA.MM.JJ-XXX.XX';
        case '1':
            return 'S MM AA JJ XXX XXX XX';
        case '2':
            return 'AAAA MM JJ XXX';
        case '3':
            return '756.XXXX.XXXX.XX';
        default:
            return 'Numéro de sécurité sociale';
    }
}

/**
 * returns the appropriate label text based on the selected country
 * param : country : he country code ('0'=Belgium, '1'=France, '2'=Luxembourg, '3'=Switzerland)
 * return : True if is valid
 */
function getSocialSecurityLabel(country) {
    switch (country) {
        case '0': // Belgium
            return 'N° de registre national (NISS)';
        case '1': // France
            return 'N° de sécurité sociale (NIR)';
        case '2': // Luxembourg
            return 'N° de sécurité sociale';
        case '3': // Switzerland
            return 'N° AVS';
        default:
            return 'N° Sécurité sociale';
    }
}

function setUpSocialSecurityByCountry() {
    const countrySelect = document.querySelector('.cache-field[data-field="Country"]');
    const ssnInput = document.querySelector('.cache-field[data-field="SocialSecurityNumber"]');
    const ssnLabel = document.querySelector('label[for="SocialSecurityNumber"]') || ssnInput?.previousElementSibling;

    if (countrySelect && ssnInput) {
        countrySelect.addEventListener('change', function () {
            const country = this.value;

            ssnInput.placeholder = getSocialSecurityPlaceholder(country);

            if (ssnLabel && ssnLabel.tagName === 'LABEL') {
                const requiredMark = ssnLabel.textContent.includes('*') ? ' *' : '';
                ssnLabel.textContent = getSocialSecurityLabel(country) + requiredMark;
            }
            if (ssnInput.value.trim()) {
                updateFieldState(ssnInput);
                updateCreateButton();
            }
        });

        // Trigger change event on load if country is already selected
        if (countrySelect.value) {
            countrySelect.dispatchEvent(new Event('change'));
        }
    }
}
/**
 * Parse Belgian NISS (YYMMDD-XXX.XX) to YYYY-MM-DD
 */
function parseBelgianNISS(niss) {
    const value = niss.replace(/\D/g, '');
    if (!/^\d{11}$/.test(value)) return null;

    let yy = parseInt(value.substring(0, 2), 10);
    let mm = parseInt(value.substring(2, 4), 10);
    let dd = parseInt(value.substring(4, 6), 10);

    if (mm > 12 && mm < 21) mm -= 20;
    if (mm > 32 && mm < 41) mm -= 20;
    if (mm > 52 && mm < 60) mm -= 40;

    const now = new Date();
    const currentYY = now.getFullYear() % 100;
    const century = yy <= currentYY ? 2000 : 1900;
    const year = century + yy;

    return `${year}-${String(mm).padStart(2, '0')}-${String(dd).padStart(2, '0')}`;
}

/**
 * Parse French NIR (SYYMMDDXXX...) to YYYY-MM-DD
 */
function parseFrenchNIR(nir) {
    const value = nir.replace(/\D/g, '');
    if (!/^\d{15}$/.test(value)) return null;

    let yy = parseInt(value.substring(1, 3), 10);
    let mm = parseInt(value.substring(3, 5), 10);
    let dd = parseInt(value.substring(5, 7), 10);

    if (mm === 20 || (mm >= 30 && mm <= 42)) mm = 1;

    const now = new Date();
    const currentYY = now.getFullYear() % 100;
    const century = yy <= currentYY ? 2000 : 1900;
    const year = century + yy;

    return `${year}-${String(mm).padStart(2, '0')}-${String(dd).padStart(2, '0')}`;
}

/**
 * Parse Luxembourg NSS (YYYYMMDDXXX) to YYYY-MM-DD
 */
function parseLuxembourgNSS(nss) {
    const value = nss.replace(/\D/g, '');
    if (!/^\d{13}$/.test(value)) return null; // 8 digits date + 5 chiffres séquentiels

    const year = parseInt(value.substring(0, 4), 10);
    const month = parseInt(value.substring(4, 6), 10);
    const day = parseInt(value.substring(6, 8), 10);

    if (month < 1 || month > 12 || day < 1 || day > 31) return null;

    return `${year}-${String(month).padStart(2, '0')}-${String(day).padStart(2, '0')}`;
}

/**
 * Determine birth date from SSN and country
 */
function getBirthDateFromSSN(country, ssn) {
    if (!ssn) return null;
    switch (country) {
        case '0': return parseBelgianNISS(ssn);
        case '1': return parseFrenchNIR(ssn);
        case '2': return parseLuxembourgNSS(ssn);
        case '3': return parseSwissAVS(ssn); // returns null
        default: return null;
    }
}

/**
 * Update the BirthDate input based on current SSN and country
 */
function updateBirthDate() {
    const countrySelect = document.querySelector('.cache-field[data-field="Country"]');
    const ssnInput = document.querySelector('.cache-field[data-field="SocialSecurityNumber"]');
    const birthDateInput = document.querySelector('.cache-field[data-field="BirthDate"]');

    if (!countrySelect || !ssnInput || !birthDateInput) return;

    const country = countrySelect.value;
    const ssn = ssnInput.value.trim();
    const birthDate = getBirthDateFromSSN(country, ssn);

    birthDateInput.value = birthDate || ''; // clear if not available
}

/**
 * Initialize automatic birth date filling
 */
function autoFillBirthDate() {
    const countrySelect = document.querySelector('.cache-field[data-field="Country"]');
    const ssnInput = document.querySelector('.cache-field[data-field="SocialSecurityNumber"]');

    if (!countrySelect || !ssnInput) return;

    ssnInput.addEventListener('input', updateBirthDate);
    countrySelect.addEventListener('change', updateBirthDate);

    // Initial fill if values exist
    updateBirthDate();
}
