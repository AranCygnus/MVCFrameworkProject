// Toastr
$(document).ready(function () {
    const success = $('#toast-success').val();
    const error = $('#toast-error').val();
    const warning = $('#toast-warning').val();
    const info = $('#toast-info').val();

    toastr.options = {
        timeOut: "5000",
        extendedTimeOut: "1000",
        showDuration: "300",
        hideDuration: "1000",
    }

    if (success) {
        toastr.success(success);
    }
    if (error) {
        toastr.error(error);
    }
    if (warning) {
        toastr.warning(warning);
    }
    if (info) {
        toastr.info(info);
    }
});


// Sytle Switcher
document.addEventListener("DOMContentLoaded", () => {
    const dayNight = document.querySelector(".day-night");
    const icon = dayNight.querySelector(".material-symbols-outlined");

    if (localStorage.getItem("theme") === "dark") {
        document.body.classList.add("dark");
        icon.textContent = "dark_mode";
    } else {
        icon.textContent = "light_mode";
    }

    dayNight.addEventListener("click", () => {
        document.body.classList.toggle("dark");

        const isDark = document.body.classList.contains("dark");
        icon.textContent = isDark ? "dark_mode" : "light_mode";
        localStorage.setItem("theme", isDark ? "dark" : "light");
    });
});