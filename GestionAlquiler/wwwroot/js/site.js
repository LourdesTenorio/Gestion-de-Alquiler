// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    var estadoValues = document.querySelectorAll("[id^='estado-value']");

    estadoValues.forEach(function (value) {
        var textoEstado = value.innerText.trim();

        if (textoEstado === "Pagado") {
            value.classList.add("btn", "btn-success");
        } else if (textoEstado === "Pendiente") {
            value.classList.add("btn", "btn-warning");
        } else if (textoEstado === "Ocupado") {
            value.classList.add("btn", "btn-secondary");

        } else if (textoEstado === "Disponible") {
            value.classList.add("btn", "btn-info");

        }
        else if (textoEstado === "En Mantenimiento") {
            value.classList.add("btn", "btn-dark");

        }
    });
});


