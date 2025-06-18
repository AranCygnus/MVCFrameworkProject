const cardInput = document.querySelector('.card-number-input');
const cardPreview = document.querySelector('.card-number-box');
const cardError = document.querySelector('.card-error');
const brandImg = document.querySelector('.card-brand-img');

const cardTypes = {
    visa: { regex: /^4/, name: 'visa' },
    mastercard: { regex: /^5[1-5]/, name: 'mastercard' },
    jcb: { regex: /^35/, name: 'jcb' },
};

brandImg.src = "";
brandImg.style.display = "none";


function detectBrand(cardNum) {
    for (let key in cardTypes) {
        if (cardTypes[key].regex.test(cardNum)) return cardTypes[key].name;
    }
    return null;
}

function luhnCheck(cardNumber) {
    let sum = 0, shouldDouble = false;
    for (let i = cardNumber.length - 1; i >= 0; i--) {
        let digit = parseInt(cardNumber[i]);
        if (shouldDouble) {
            digit *= 2;
            if (digit > 9) digit -= 9;
        }
        sum += digit;
        shouldDouble = !shouldDouble;
    }
    return sum % 10 === 0;
}

function showError(msg) {
    cardError.style.display = "block";
    cardError.innerText = msg;
    cardPreview.innerText = "#### #### #### ####";
    brandImg.src = "";
    brandImg.style.display = "none";
}

function clearError() {
    cardError.style.display = "none";
    cardError.innerText = "";
    brandImg.style.display = 'inline';
}

cardInput.addEventListener('input', (e) => {
    const raw = e.target.value.replace(/\D/g, "").slice(0, 16);
    const formatted = raw.replace(/(.{4})/g, "$1 ").trim();
    cardInput.value = formatted;

    const brand = detectBrand(raw);
    if (brand) {
        brandImg.src = `/images/card/${brand}.png`;
        brandImg.style.display = "inline";
    } else {
        brandImg.src = "";
        brandImg.style.display = "none";
    }

    const masked = raw.padEnd(16, "#")
        .replace(/^(.{4})(.{4})(.{4})(.{4})$/, (_, a, b, c, d) => `${a} **** **** ${d}`);
    cardPreview.innerText = masked;

    clearError();

    if (raw.length < 13) return;
    if (!/^\d+$/.test(raw)) return showError("Card number must be digits only");
    if (!luhnCheck(raw)) return showError("Invalid card number (Luhn check failed)");
    if (!brand) return showError("Only Visa / MasterCard / JCB accepted");
});

// Card Holder
document.querySelector('.card-holder-input').addEventListener('input', (e) => {
    document.querySelector('.card-holder-name').innerText = e.target.value;
});

// Expiration
const monthSelect = document.querySelector('.month-input');
const yearSelect = document.querySelector('.year-input');
const expiration = document.querySelector('.expiration');

const now = new Date().getFullYear();
for (let i = 1; i <= 12; i++) {
    const mm = i.toString().padStart(2, '0');
    monthSelect.innerHTML += `<option value="${mm}">${mm}</option>`;
}
for (let i = 0; i <= 10; i++) {
    const yy = (now + i).toString().slice(-2);
    yearSelect.innerHTML += `<option value="${yy}">${yy}</option>`;
}
const updateExp = () => {
    const mm = monthSelect.value;
    const yy = yearSelect.value;
    if (mm && yy) expiration.innerText = `${mm}/${yy}`;
};
monthSelect.addEventListener('change', updateExp);
yearSelect.addEventListener('change', updateExp);

// CVV Flip
const cvvInput = document.querySelector('.cvv-input');
cvvInput.addEventListener('mouseenter', () => {
    document.querySelector('.front').style.transform = 'perspective(1000px) rotateY(-180deg)';
    document.querySelector('.back').style.transform = 'perspective(1000px) rotateY(0deg)';
});
cvvInput.addEventListener('mouseleave', () => {
    document.querySelector('.front').style.transform = 'perspective(1000px) rotateY(0deg)';
    document.querySelector('.back').style.transform = 'perspective(1000px) rotateY(180deg)';
});
cvvInput.addEventListener('input', (e) => {
    document.querySelector('.cvv-box').innerText = e.target.value;
});


