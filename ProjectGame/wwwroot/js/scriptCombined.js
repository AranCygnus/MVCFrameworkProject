const container = document.getElementById("container");
const registerBtn = document.getElementById("register");
const loginBtn = document.getElementById("login");

registerBtn.addEventListener("click", () => {
    container.classList.add("active");
});

loginBtn.addEventListener("click", () => {
    container.classList.remove("active");
});


/* Tooltip Error Message */
document.addEventListener("DOMContentLoaded", function () {
    const inputs = document.querySelectorAll("input");

    inputs.forEach(input => {
        const span = input.nextElementSibling;
        if (span && span.classList.contains("error-tooltip") && span.textContent.trim() !== "") {
            input.classList.add("input-error");

            setTimeout(() => {
                span.style.opacity = 0;
                span.style.visibility = "hidden";
            }, 3000);

            input.addEventListener("input", () => {
                input.classList.remove("input-error");
                if (span) {
                    span.style.opacity = 0;
                    span.style.visibility = "hidden";
                }
            });
        }
    });
});