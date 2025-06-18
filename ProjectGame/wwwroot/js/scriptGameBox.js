
// Animate
document.addEventListener("DOMContentLoaded", function () {
    const bottom = document.getElementById("bottom");
    const right = document.getElementById("right");
    const left = document.getElementById("left");
    const up = document.getElementById("up");
    const bar = document.getElementById("bar");
    const G = document.getElementById("G");
    const ids = ["A", "M", "E", "B", "O", "X"];

    // progress Bar
    document.querySelector(".progress-fill").style.animation = "fillBar 3.2s linear forwards";


    // Icon Animate
    // Circle Drop
    setTimeout(() => {
        bottom.style.animation = "dropBounce .8s cubic-bezier(.8, 0, 1, 1) forwards ";
        right.style.animation = "dropBounce .8s cubic-bezier(.8, 0, 1, 1) forwards .2s";
        left.style.animation = "dropBounce .8s cubic-bezier(.8, 0, 1, 1) forwards .4s";
        up.style.animation = "dropBounce .8s cubic-bezier(.8, 0, 1, 1) forwards .6s";
    }, 500);

    // Bar Slide In
    setTimeout(() => {
        bar.style.animation = "slideInBar 1s cubic-bezier(.8, .35, 1, 1) forwards";
    }, 1000);

    // G Frame Line
    setTimeout(() => {
        G.style.animation = "drawPath 1.2s ease-out forwards";
    }, 1200);

    // G Fillout
    setTimeout(() => {
        G.style.fillOpacity = 1;
    }, 1350);

    // Typing
    setTimeout(() => {
        ids.forEach((id, index) => {
            setTimeout(() => {
                document.getElementById(id)?.classList.add("visible");
            }, 300 * index);
        });
    }, 1450);
    

});


// Delate
setTimeout(() => {
    window.location.href = "/Home/Index";
}, 3800);