$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
    $('[data-toggle="popover"]').popover({ html: true });
});


function SVCountGenerate(e) {
    cgridSV.Refresh();
    e.preventDefault();
}