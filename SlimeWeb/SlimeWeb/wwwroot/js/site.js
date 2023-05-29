// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.onload += fixquillthemeissues();
function fixquillthemeissues() {
    let centelems = document.getElementsByClassName("ql-align-center");
    let righttelems = document.getElementsByClassName("ql-align-right");
    let justifyelems = document.getElementsByClassName("ql-align-justify");


    if (centelems != null) {
        for (var i = 0; i < centelems.length; i++) {
           // centelems[i].className = "text-center";
            centelems[i].className += " text-center";
        }
    }
    if (righttelems != null) {
        for (var i = 0; i < righttelems.length; i++) {
            righttelems[i].className += " text-right";
        }
    }
    if (justifyelems != null) {
        for (var i = 0; i < justifyelems.length; i++) {
            justifyelems[i].className += " text-justify";
        }
    }
}


