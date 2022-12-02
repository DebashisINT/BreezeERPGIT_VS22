function PactCountGenerate(e) {
    cgridpact.Refresh();
    e.preventDefault();
}

$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
    $('[data-toggle="popover"]').popover({ html: true });
});