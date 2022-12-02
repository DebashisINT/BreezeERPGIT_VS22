function CallCountGenerate(e) {
    cgridPhone.Refresh();
    e.preventDefault();
}
$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
    $('[data-toggle="popover"]').popover({ html: true });
});


function loadList(sid) {
    chideSalesman.SetText(sid);
    $('#phCall_hdSalesmanId').val(sid);
    $('#callditailsmod').modal('show');
    cgridPhoneDet.Refresh();
   

}