
       

function ZoomProduct(keyValue) {

    var url = '/OMS/management/master/View/ViewProduct.html?id=' + keyValue;

    CAspxDirectProductViewPopup.SetWidth(window.screen.width - 50);
    CAspxDirectProductViewPopup.SetHeight(window.innerHeight - 70);
    CAspxDirectProductViewPopup.SetContentUrl(url);
    CAspxDirectProductViewPopup.RefreshContentUrl();
    CAspxDirectProductViewPopup.Show();
}

function View_Customer(keyValue) {

    CAspxDirectCustomerViewPopup.SetWidth(window.screen.width - 50);
    CAspxDirectCustomerViewPopup.SetHeight(window.innerHeight - 70);
    var url = '/OMS/management/master/View/ViewCustomer.html?id=' + keyValue;
    CAspxDirectCustomerViewPopup.SetContentUrl(url);
    //AspxDirectAddCustPopup.ClearVerticalAlignedElementsCache();

    CAspxDirectCustomerViewPopup.RefreshContentUrl();
    CAspxDirectCustomerViewPopup.Show();
}



$(document).ready(function () {
    //BindAssignSupervisor();
    //ListAssignTo();

    $("#lstAssignTo").chosen().change(function () {
        var assignId = $(this).val();
        $('#hdnAssign').val(assignId);
    })

    $("#DropDownListCallDisposition").change(function () {
        var dispositionval = this.value;

        grid.PerformCallback('Disposition~' + dispositionval);
        //var firstDropVal = $('#pick').val();
    });

    $("#DropDownSalesVisit").change(function () {
        var SalesVisitval = this.value;

        grid.PerformCallback('SalesVisit~' + SalesVisitval);
        //var firstDropVal = $('#pick').val();
    });
})


function ShowDetailProduct(actid) {

    cPopup_Product.Show();
    cAspxProductGrid.PerformCallback(actid);
    // cComponentPanel.PerformCallback(slsid);
}

function ShowDetailProductClass(actid) {

    cPopup_Product_Class.Show();
    cAspxProductClassGrid.PerformCallback(actid);
    // cComponentPanel.PerformCallback(slsid);
}


function Call_save() {
    var uid = $('#hdnAssign').val();
    grid.PerformCallback('Ressign~' + reassignvar + '~' + uid);

}
function ShowDetailReassign(slsid) {
    reassignvar = slsid;
    cPopup_Reassign.Show();
}

function ShowHistory(sls_id) {
    //alert(cxdeFromDate.GetDate());
    var frmdate = cxdeFromDate.GetText();
    var todate = cxdeToDate.GetText();




    document.location.href = "../Master/ShowHistory_Phonecall.aspx?id1=" + sls_id + "&frmdate=" + frmdate + "&todate=" + todate;
}


function ShowCreateActivity(LeadId, sls_id, sls_activity_id, act_assignedTo, act_activityNo, act_assign_task) {
    grid.PerformCallback('CreateActivity~' + LeadId + '~' + sls_id + '~' + sls_activity_id + '~' + act_assignedTo + '~' + act_activityNo + '~' + act_assign_task);
}
function AfterSave() {
    document.getElementById('GridDiv').style.display = 'inline';
    document.getElementById('FrameDiv').style.display = 'none';
    //height();
}


function disp_prompt(name) {
    if (name == "tab0") {
        //document.location.href="crm_sales.aspx"; 
    }
    if (name == "tab1") {
        document.location.href = "frmDocument.aspx";
    }
    else if (name == "tab2") {
        document.location.href = "futuresale.aspx";
    }
    else if (name == "tab3") {
        document.location.href = "ClarificationSales.aspx";
    }
    else if (name == "tab4") {
        document.location.href = "ClosedSales.aspx";
    }
}

function CallForms(val) {

    if (val == "Expences") {
        var frm = "sales_Sconveyence.aspx";
        var Title = "Expences";
    }
    else if (val == "Address") {
        var frm = "Contact_Correspondence.aspx?type=modify&requesttype=lead&formtype=lead";
        var Title = "Address/Phone";

    }
    else if (val == "Contact") {
        var frm = "Lead_general.aspx?type=modify&requesttype=lead&formtype=lead123";
        var Title = "Contact";
    }
    else if (val == "Bank") {
        var frm = "Contact_BankDetails.aspx?type=modify&requesttype=lead&formtype=lead123";
        Title = "Bank Details";
    }
    else if (val == "Registration") {
        var frm = "Contact_Registration.aspx?type=modify&requesttype=lead&formtype=lead123";
        var Title = "Registration";
    }
    else if (val == "Document") {
        var frm = "SalesDocument.aspx?type=modify&requesttype=lead&formtype=lead";
        var Title = "Document Details";
    }
    else if (val == "History") {
        var frm = "../ShowHistory_Phonecall.aspx";
        var Title = "History";
    }
    OnMoreInfoClick(frm, Title, '950px', '450px', 'Y');
}
function callback() {
    document.getElementById("ShowDetails").contentWindow.ClosingDHTML();
}
//function ShowDetail(keyValue) {
//    $('#SaleListID').attr('style', 'display:none');
//    $('#SaleDetailsListID').attr('style', 'display:block');
//    grid.PerformCallback('Details~' + keyValue);
//}
function ShowDetail(keyValue, AssignedID) {
    $('#SaleListID').attr('style', 'display:none');
    $('#SaleDetailsListID').attr('style', 'display:block');
    $('#divadd').attr('style', 'display:none');
    $('#btncross').attr('style', 'display:block');
    $('#divdetails').attr('style', 'display:block');
    $('#sapnId').attr('style', 'display:inline-block');

    grid.PerformCallback('Details~' + keyValue + '~' + AssignedID);
}
function DeleteRow(keyValue) {

    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
        if (r == true) {
            Sgrid.PerformCallback('Delete~' + keyValue);
        }
    });
}

function LastCall(obj) {
    if (Sgrid.cpDelmsg != null) {

        if (Sgrid.cpDelmsg.trim() != '') {
            jAlert(Sgrid.cpDelmsg);
            Sgrid.cpDelmsg = '';

        }
    }
}


function LastActivityDetailsCall(obj) {
    if (grid.cpSave != null) {
        if (grid.cpSave == 'Y') {
            grid.cpSave = '';
            // jAlert("Saved Successfully");
            cPopup_Reassign.Hide();
        }

    }
    if (grid.cpredirect != null) {
        var rurl = grid.cpredirect;
        grid.cpredirect = null;

        document.location.href = rurl;

    }


    if (grid.cpDelmsg != null) {
        if (grid.cpDelmsg.trim() != '') {
            jAlert(grid.cpDelmsg);
            grid.cpDelmsg = '';

        }


    }

    if (grid.cpSave == "1") {

        grid.cpSave = null;
        window.close();
        window.parent.popupCbudget.Hide();
        //grid.Refresh();
    }
}
function AddButtonClick() {

    document.location.href = "../ActivityManagement/Sales_Activity.aspx";
}



function BindAssignSupervisor() {

    var lAssignTo = $('select[id$=lstAssignTo]');


    $.ajax({
        type: "POST",
        url: 'crm_sales.aspx/GetAllUserListBeforeSelect',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var list = msg.d;
            var listItems = [];
            if (list.length > 0) {

                for (var i = 0; i < list.length; i++) {

                    var id = '';
                    var name = '';
                    id = list[i].split('|')[1];
                    name = list[i].split('|')[0];

                    listItems.push('<option value="' +
                    id + '">' + name
                    + '</option>');
                }

                $(lAssignTo).append(listItems.join(''));


                ListAssignTo();
                $('#lstAssignTo').trigger("chosen:updated");


            }
            else {
                $('#lstAssignTo').trigger("chosen:updated");
                $('#lstAssignTo').prop('disabled', true).trigger("chosen:updated");
                // alert("No records found");
            }
            //else {
            //    alert("No records found");
            //}
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(textStatus);
        }
    });


}
function ListAssignTo() {

    $('#lstAssignTo').chosen(); Settings
    $('#lstAssignTo').fadeIn();

    var config = {
        '.chsnProduct': {},
        '.chsnProduct-deselect': { allow_single_deselect: true },
        '.chsnProduct-no-single': { disable_search_threshold: 10 },
        '.chsnProduct-no-results': { no_results_text: 'Oops, nothing found!' },
        '.chsnProduct-width': { width: "100%" }
    }
    for (var selector in config) {
        $(selector).chosen(config[selector]);
    }
}


function Budget_open() {
    var SMid = '';
    var url = '/OMS/Management/Activities/SalesmanBudget.aspx?tid=1';
    popupbudget.SetContentUrl(url);
    popupbudget.Show();

    return false;
    //return true;
}
function BudgetAfterHide(s, e) {
    popupbudget.Hide();
}


function btn_ShowRecordsClick() {

    var startDate = new Date();
    var endDate = new Date();

    startDate = cxdeFromDate.GetDate();
    if (startDate != null) {
        endDate = cxdeToDate.GetDate();
        var startTime = startDate.getTime();
        var endTime = endDate.getTime();


        difference = (endTime - startTime) / (1000 * 60 * 60 * 24 * 30);

        if (difference > 12) {
            jAlert('End date cannot  12 month later than Start date', 'Open Activities', function () {
                return false;

            });
        }
        else {
            grid.PerformCallback('ShowGrid');
        }
    }


}
function ShowClosed(keyValue) {

    jConfirm('Confirm Closed Sales?', 'Confirmation Dialog', function (r) {
        if (r == true) {
            grid.PerformCallback('ClosedStatus~' + keyValue);
        }
    });
}
function OnBudgetCopen(Cusid, productclassid, slsid) {

    cacpCrossBtn.PerformCallback('BudgetClass~' + Cusid + '~' + productclassid + '~' + slsid);
    popupCbudget.Show();

    return true;
}

function BudgetCAfterHide(s, e) {
    popupCbudget.Hide();
}
function Save_ButtonClick() {

    grid.PerformCallback('InsertBudgetClass');
}


function acpCrossBtnEndCall() {
    // debugger;
    var custid = '';
    var productclassid = '';
    var slsid = '';

    if (cacpCrossBtn.cpcustid != null)
    { custid = cacpCrossBtn.cpcustid; }

    if (cacpCrossBtn.cpproductclassid != null) {
        productclassid = cacpCrossBtn.cpproductclassid;
    }

    if (cacpCrossBtn.cpslsid != null) {
        slsid = cacpCrossBtn.cpslsid;
    }
    $('#hdncustid').val(custid);
    $('#hdnproductclassid').val(productclassid);

    $('#hdnslsid').val(slsid);



    cacpCrossBtn.cpcustid = null;
    cacpCrossBtn.cpproductclassid = null;
    cacpCrossBtn.cpslsid = null;


}
