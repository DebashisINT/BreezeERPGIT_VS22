function NewPgae(cnt_id) {
    //alert('cnt_id');
}
function ClickOnMoreInfo(keyValue) {
    var url = 'rootcompany_general.aspx?id=' + keyValue;
    OnMoreInfoClick(url, "Modify Company Details", '940px', '450px', "Y");
}

function OnAddButtonClick() {
    var url = 'rootcompany_general.aspx?id=' + 'ADD';
    OnMoreInfoClick(url, "Add Company Details", '940px', '450px', "Y");
}
function ShowHideFilter(obj) {
    grid.PerformCallback(obj);
}
function callback() {
    grid.PerformCallback('All');
}