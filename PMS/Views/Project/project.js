
    var ProjectCodeCheck = {};
var ProcjectClose = {};
var ProjectstatusForTransaction = {};
var projectCode = [];
var MapBranch_id = [];
$(function () {
    $('#Proj_EstimatelabourCost').on('input', function () {
        this.value = this.value
          .replace(/[^\d.]/g, '')             // numbers and decimals only
          .replace(/(^[\d]{9})[\d]/g, '$1')   // not more than 2 digits at the beginning
          .replace(/(\..*)\./g, '$1')         // decimal can't exist more than once
          .replace(/(\.[\d]{2})./g, '$1');    // not more than 4 digits after decimal
    });
});
$(function () {
    $('#Proj_EstimateExpenseCost').on('input', function () {
        this.value = this.value
          .replace(/[^\d.]/g, '')             // numbers and decimals only
          .replace(/(^[\d]{9})[\d]/g, '$1')   // not more than 2 digits at the beginning
          .replace(/(\..*)\./g, '$1')         // decimal can't exist more than once
          .replace(/(\.[\d]{2})./g, '$1');    // not more than 4 digits after decimal
    });
});
$(function () {
    $('#proj_EstimateTotCost').on('input', function () {
        this.value = this.value
          .replace(/[^\d.]/g, '')             // numbers and decimals only
          .replace(/(^[\d]{9})[\d]/g, '$1')   // not more than 2 digits at the beginning
          .replace(/(\..*)\./g, '$1')         // decimal can't exist more than once
          .replace(/(\.[\d]{2})./g, '$1');    // not more than 4 digits after decimal
    });
});
$(function () {
    gridProjectList.Refresh();
    gridProjectList.Refresh();
    $('#ddlAppIds').on('change', function () {
        if ($("#ddlAppIds option:selected").index() > 0) {
            var selectedValue = $(this).val();
            $('#ddlAppIds').prop("selectedIndex", 0);
            var url = '@Url.Action("ExportProjectlist", "Project", new { type = "_type_" })'
            window.location.href = url.replace("_type_", selectedValue);
        }
    });
});

$(function () {
    $('#Terms_LiqDamage').on('input', function () {
        this.value = this.value
          .replace(/[^\d.]/g, '')             // numbers and decimals only
          .replace(/(^[\d]{9})[\d]/g, '$1')   // not more than 2 digits at the beginning
          .replace(/(\..*)\./g, '$1')         // decimal can't exist more than once
          .replace(/(\.[\d]{2})./g, '$1');    // not more than 4 digits after decimal
    });
});


$(function () {
    $('#BG_Percentage').on('input', function () {
        this.value = this.value
          .replace(/[^\d.]/g, '')             // numbers and decimals only
          .replace(/(^[\d]{9})[\d]/g, '$1')   // not more than 2 digits at the beginning
          .replace(/(\..*)\./g, '$1')         // decimal can't exist more than once
          .replace(/(\.[\d]{2})./g, '$1');    // not more than 4 digits after decimal
    });
});

$(function () {
    $('#BG_BGValue').on('input', function () {
        this.value = this.value
          .replace(/[^\d.]/g, '')             // numbers and decimals only
          .replace(/(^[\d]{9})[\d]/g, '$1')   // not more than 2 digits at the beginning
          .replace(/(\..*)\./g, '$1')         // decimal can't exist more than once
          .replace(/(\.[\d]{2})./g, '$1');    // not more than 4 digits after decimal
    });
});

function OnStartCallback(s, e) {


}

function OnStartTermsConitionsCallback(s, e) {

    //TermsConitionsPartial.Refresh();
}

   

function AddcustomerClick() {
       
    var url = '/OMS/management/Master/customerPopup.html?var=1.1.3.13';
       
    AspxDirectAddCustPopup.SetContentUrl(url);
     
    AspxDirectAddCustPopup.RefreshContentUrl();
    AspxDirectAddCustPopup.Show();
}

function ParentCustomerOnClose(InternalID, CustomerName, UniqueName) {
    AspxDirectAddCustPopup.Hide();
        
    $("#CustomerId").val(InternalID);
    ctxtShipToPartyShippingAdd.SetText('');
    if (InternalID != "") {
        CustomerTxt.SetText(CustomerName);
        SetCustomer(InternalID, CustomerName);
    }
        
}


function ProjectSelectionChanged(s, e) {
    ProjectGridLookup.gridView.GetSelectedFieldValues("OrderId", GetProjectSelectedFieldValuesCallback);
}
function BranchSelectionChanged(s, e) {
    BranchGridLookup.gridView.GetSelectedFieldValues("Br_id", GetBranchSelectedFieldValuesCallback);
}
function GetProjectSelectedFieldValuesCallback(values) {
    try {
        projectCode = [];
        for (var i = 0; i < values.length; i++) {
            projectCode.push(values[i]);
        }
    } finally {
        console.log(projectCode);
    }
}
function GetBranchSelectedFieldValuesCallback(values) {
    try {
        MapBranch_id = [];
        for (var i = 0; i < values.length; i++) {
            MapBranch_id.push(values[i]);
        }
    } finally {
        console.log(MapBranch_id);
    }
}

function ProjectStartCallback(s, e) {
    e.customArgs["Cnt_InternalId"] = $("#CustomerId").val();
    e.customArgs["Proj_Bracnchid"] = $("#ddlBranch").val();
    e.customArgs["Proj_Id"] = $("#ProjId").val();
}
function BranchStartCallback(s, e) {
    e.customArgs["Cnt_InternalId"] = $("#CustomerId").val();
    //e.customArgs["Proj_Bracnchid"] = $("#ddlBranch").val();
    //e.customArgs["Proj_Id"] = $("#ProjId").val();
}

function ProjectLookupValChange() {
    ProjectGridLookup.GetGridView().Refresh();
}

function BranchLookupValChange() {
    BranchGridLookup.GetGridView().Refresh();
}

function selectchangeOfBranch() {
    var BranchId = $("#ddlNumbscheme").val().split('~')[3];
    $("#ddlBranch").val(BranchId);
    $("#ddlBranch").prop("disabled", true);
    if ($("#ddlNumbscheme").val().split('~')[1] != "0") {

        $("#Proj_Code").val("Auto");
        $("#Proj_Code").prop("disabled", true);
    }
    else {
        $("#Proj_Code").val("");
        $("#Proj_Code").prop("disabled", false);
    }
}

 

$(document).ready(function () {

    $("#openlink").on("click", function () {
        AddcustomerClick();
    });
    var ProjectTermsCondition = '@ViewBag.ProjectTermsCondition';

    var MultipleBranchProject = '@ViewBag.MultipleBranchProject';
    if (MultipleBranchProject != null && MultipleBranchProject != "" && MultipleBranchProject != "No" && MultipleBranchProject != "NO")
    {
        $('#drdUnit').hide();
        $('#BranchGridLookup').show();

    }
    else {
        $('#drdUnit').show();
        $('#BranchGridLookup').hide();
          
    }
    BranchGridLookup.SetEnabled(false);
    if (ProjectTermsCondition == "No") {
        $("#btn_TermsCondition").hide();
    }
    else
    {
        $("#btn_TermsCondition").show();
    }


    $('#CustModel').on('shown.bs.modal', function () {
        $('#txtCustSearch').trigger('focus')
    })


    $('#proj_EstimateTotCost').prop("disabled", true);

    gridProjectList.Refresh();
    gridProjectList.Refresh();


    //$('.percent').mask('##0,00%', { reverse: true });
});
function CheckuniqueCode() {
    debugger;
    var det = {};
    var Code = $("#Proj_Code").val();
    det.Code = Code;
    $.ajax({
        type: "POST",
        url: "@Url.Action("UniqueCodeCheck", "Project")",
        data: JSON.stringify(det),
    contentType: "application/json; charset=utf-8",
    datatype: "JSON",

    success: function (data) {

        var val = data;

        debugger;
        if (val == 1) {
            jAlert("Already Exists");
            $("#Proj_Code").val("");
            $("#Proj_Code").focus();
        }

    },
    error: function (data) {

        jAlert("Please try again later");
    }
});
}

function validateNumbers(el, evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    var number = el.value.split('.');

    //if ((moduleNameSTk == "Stock Adjustment" && charCode == 45)) {
    //    return true;
    //}
    //else {
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    //just one dot (thanks ddlab)
    if (number.length > 1 && charCode == 46) {
        return false;
    }
    //get the carat position
    var caratPos = getSelectionStart(el);
    var dotPos = el.value.indexOf(".");
    if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
        return false;
    }
    return true;
    //}
}

function CustomerButnClick(s, e) {

    var htmlScript = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th class=\"hide\">id</th><th>Name</th><th>Unique Id</th><th>Address</th></tr>";

    document.getElementById('CustomerTable').innerHTML = htmlScript;
    $('#CustModel').modal('show');

}

function Customer_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        $('#CustModel').modal('show');
        $("#txtCustSearch").focus();
    }
}
function Customerkeydown(e) {
    var OtherDetails = {}

    if ($.trim($("#txtCustSearch").val()) == "" || $.trim($("#txtCustSearch").val()) == null) {
        return false;
    }
    OtherDetails.SearchKey = $("#txtCustSearch").val();
    // OtherDetails.BranchID = $('#ddl_Branch').val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Name");
        HeaderCaption.push("Unique Id");
        HeaderCaption.push("Address");

        if ($("#txtCustSearch").val() != "") {

            callonServer("../Models/CustomAddress.asmx/GetCustomer", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");

        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[customerindex=0]"))
            $("input[customerindex=0]").focus();
    }
}


function SetCustomer(id, Name) {

    var key = id;
    $('#CustomerId').val(id)

    if (key != null && key != '') {
        $('#CustModel').modal('hide');
        // ClstContactBy.SetText(Name);
        //$("#CustomerTxt").val(Name);
        CustomerTxt.SetText(Name);
        CustomerTxt.SetFocus();

    }
}

function ValueSelected(e, indexName) {
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var Id = e.target.parentElement.parentElement.cells[0].innerText;
        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
        if (Id) {
            if (indexName == "customerIndex") {
                SetCustomer(Id, name);
            }
        }
    }
    else if (e.code == "ArrowDown") {
        thisindex = parseFloat(e.target.getAttribute(indexName));
        thisindex++;
        if (thisindex < 10)
            $("input[" + indexName + "=" + thisindex + "]").focus();
    }
    else if (e.code == "ArrowUp") {
        thisindex = parseFloat(e.target.getAttribute(indexName));
        thisindex--;
        if (thisindex > -1)
            $("input[" + indexName + "=" + thisindex + "]").focus();
        else {
            if (indexName == "customerIndex") {
                $('#txtCustSearch').focus();
            }
        }
    }
}

function FinishClick() {

    ProcjectClose = "1";
}

function ProjectApproval(obj) {
    BranchGridLookup.gridView.Refresh();

    var MultipleBranchProject = '@ViewBag.MultipleBranchProject';
    if (MultipleBranchProject != null && MultipleBranchProject != "" && MultipleBranchProject != "No" && MultipleBranchProject != "NO") {
           
        $('#drdUnit').hide();
        $('#BranchGridLookup').show();
    }
    else {
        $('#drdUnit').show();
        $('#BranchGridLookup').hide();
    }
    BranchGridLookup.SetEnabled(false);
    var cancelProjectData = {};
    var cancelProjectResult = {};
    cancelProjectData.Code = obj;
    $.ajax({
        async: false,
        type: "POST",
        url: "@Url.Action("CancelProjectCheck", "Project")",
        data: JSON.stringify(cancelProjectData),
    contentType: "application/json; charset=utf-8",
    datatype: "JSON",

    success: function (data) {

        cancelProjectResult = data;


    },
    error: function (data) {

        jAlert("Please try again later");
    }
});
if (cancelProjectResult.split("~")[0] == "1" || cancelProjectResult.split("~")[1] == "Finish") {
    jAlert("Project cancelled/closed,Cannot approve data.");
}


else {
    document.getElementById("dvNumberscheme").style.display = "none";
    document.getElementById("exampleModalLabelForApproval").style.display = "block";
    document.getElementById("exampleModalLabelForAdd").style.display = "none";
    document.getElementById("exampleModalLabelForModify").style.display = "none";
    document.getElementById("exampleModalLabelForView").style.display = "none";
    document.getElementById("btnApproved").style.display = "inline-block";
    document.getElementById("btnReject").style.display = "inline-block";
    $("#btnSave").addClass('hide');
    $("#btncancel").addClass('hide');
    $("#btnApproved").removeClass('hide');
    $("#btnReject").removeClass('hide');
    $("#ProjId").val(obj);
    document.getElementById("dvApproval").style.display = "block";
    $("#projectMod").modal('toggle');

    $.ajax({
        type: "POST",
        url: "@Url.Action("ViewDataShow", "Project")",
        data: JSON.stringify({ Proj_Id: obj }),
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (response) {
        // LoadingPanel.Hide();
        debugger;
        var status = response;
        var str = "";

        if (status != null) {

            $("#Proj_Name").val(status.Proj_Name);
            $("#Proj_Description").val(status.Proj_Description);
            $("#Proj_Code").val(status.Proj_Code);
            $("#CustomerId").val(status.Cnt_InternalId);

            $("#ddlBranch").val(status.Proj_Bracnchid);
            $("#ddlPManager").val(status.Proj_Managerid);
            $("#drdStatus").val(status.Proj_Statuscolor);
            var startdt = new Date(parseInt(status.Proj_EstimateStartdate.substr(6)));
            var Enddt = new Date(parseInt(status.Proj_EstimateEnddate.substr(6)));
            CustomerTxt.SetText(status.Customer);
            StartDate_dt.SetDate(startdt);
            EndDate_dt.SetDate(Enddt);
            $("#drdStatus").val(status.Proj_Statuscolor);
            $("#drdCal").val(status.Proj_Calender);
            //$("#Proj_Estimatehours").val(status.Proj_Estimatehours);
            $("#estHrs").val(status.Proj_estimateHH);
            $("#estmm").val(status.Proj_estimateMM);
            $("#Proj_EstimatelabourCost").val(status.Proj_EstimatelabourCost);
            $("#Proj_EstimateExpenseCost").val(status.Proj_EstimateExpenseCost);
            $("#proj_EstimateTotCost").val(status.proj_EstimateTotCost);
            var Acstartdt = new Date(parseInt(status.Proj_ActualStartdate.substr(6)));
            var Acendtdt = new Date(parseInt(status.Proj_ActualEndDate.substr(6)));
            ActualStartDate_dt.SetDate(Acstartdt);
            ActualEndDate_dt.SetDate(Acendtdt);
            $("#ddlUser").val(status.Approved_by);
            var approvedt = new Date(parseInt(status.Approved_On.substr(6)));
            Approved_dt.SetDate(approvedt);
            $("#Remarks").val(status.Remarks);

            $('#projectProgress li').removeClass('on visited')
            $('.widgetDrop').hide();
            if (status.projStage_Desc == "New") {
                $('#liNew.parentLi').removeClass('on').addClass('visited');
                $('#liQualify.parentLi').addClass('ready on');
            }
            else if (status.projStage_Desc == "Qualify") {
                $('#liNew.parentLi').removeClass('on').addClass('visited');
                $('#liQualify.parentLi').addClass('ready on');
            }

            else if (status.projStage_Desc == "Planning") {
                $('#liNew.parentLi').removeClass('on').addClass('visited');
                $('#liQualify.parentLi').removeClass('on').addClass('visited');
                $('#liPlanning.parentLi').addClass('ready on');
            }
            else if (status.projStage_Desc == "Execution") {
                $('#liNew.parentLi').removeClass('on').addClass('visited');
                $('#liQualify.parentLi').removeClass('on').addClass('visited');
                $('#liPlanning.parentLi').removeClass('on').addClass('visited');
                $('#liExe.parentLi').addClass('ready on');
            }

            else if (status.projStage_Desc == "Deliver") {
                $('#liNew.parentLi').removeClass('on').addClass('visited');
                $('#liQualify.parentLi').removeClass('on').addClass('visited');
                $('#liPlanning.parentLi').removeClass('on').addClass('visited');
                $('#liExe.parentLi').removeClass('on').addClass('visited');
                $('#liDeliver.parentLi').addClass('ready on');
            }

            else if (status.projStage_Desc == "Complete") {
                $('#liNew.parentLi').removeClass('on').addClass('visited');
                $('#liQualify.parentLi').removeClass('on').addClass('visited');
                $('#liPlanning.parentLi').removeClass('on').addClass('visited');
                $('#liExe.parentLi').removeClass('on').addClass('visited');
                $('#liDeliver.parentLi').removeClass('on').addClass('visited');
                $('#liComplete.parentLi').addClass('ready on');
            }

            else if (status.projStage_Desc == "Close") {
                $('#liNew.parentLi').removeClass('on').addClass('visited');
                $('#liQualify.parentLi').removeClass('on').addClass('visited');
                $('#liPlanning.parentLi').removeClass('on').addClass('visited');
                $('#liExe.parentLi').removeClass('on').addClass('visited');
                $('#liDeliver.parentLi').removeClass('on').addClass('visited');
                $('#liComplete.parentLi').removeClass('on').addClass('visited');
                $('#liClose.parentLi').addClass('ready on');
            }





            $("#NewNm").val(status.Proj_Name);
            $("#NewDesc").val(status.Proj_Description);
            $("#NewCust").val(status.Customer);

            $("#NewNm").prop("disabled", true);
            $("#NewDesc").prop("disabled", true);
            $("#NewCust").prop("disabled", true);

            $("#QuaNm").val(status.Proj_Name);
            $("#QuaDesc").val(status.Proj_Description);
            $("#QuaCust").val(status.Customer);

            $("#QuaNm").prop("disabled", true);
            $("#QuaDesc").prop("disabled", true);
            $("#QuaCust").prop("disabled", true);


            $("#PlanningNm").val(status.Proj_Name);
            $("#PlanningDesc").val(status.Proj_Description);
            $("#PlanningCust").val(status.Customer);

            $("#PlanningNm").prop("disabled", true);
            $("#PlanningDesc").prop("disabled", true);
            $("#PlanningCust").prop("disabled", true);

            $("#executionNm").val(status.Proj_Name);
            $("#executionDesc").val(status.Proj_Description);
            $("#executionCust").val(status.Customer);

            $("#executionNm").prop("disabled", true);
            $("#executionDesc").prop("disabled", true);
            $("#executionCust").prop("disabled", true);


            $("#DeliverNm").val(status.Proj_Name);
            $("#DeliverDesc").val(status.Proj_Description);
            $("#DeliverCust").val(status.Customer);

            $("#DeliverNm").prop("disabled", true);
            $("#DeliverDesc").prop("disabled", true);
            $("#DeliverCust").prop("disabled", true);



            $("#CompleteNm").val(status.Proj_Name);
            $("#CompleteDesc").val(status.Proj_Description);
            $("#CompleteCust").val(status.Customer);

            $("#CompleteNm").prop("disabled", true);
            $("#CompleteDesc").prop("disabled", true);
            $("#CompleteCust").prop("disabled", true);



            $("#CloseNm").val(status.Proj_Name);
            $("#CloseDesc").val(status.Proj_Description);
            $("#CloseCust").val(status.Customer);

            $("#CloseNm").prop("disabled", true);
            $("#CloseDesc").prop("disabled", true);
            $("#CloseCust").prop("disabled", true);

            var OrderCount = [];



            ProjectGridLookup.gridView.Refresh();
            OrderCount = status.Order_Id.split(',');
            for (var i = 0; i < OrderCount.length; i++) {
                //ProjectGridLookup.gridView.Keyfield(OrderCount[i].trim());
                ProjectGridLookup.gridView.SelectItemsByKey(OrderCount);
            }

            var BranchmapCount = [];
                        
            BranchmapCount = status.BranchMap_Id.split(',');
            if (BranchmapCount !="")
            {
                for (var i = 0; i < BranchmapCount.length; i++) {
                    //ProjectGridLookup.gridView.Keyfield(OrderCount[i].trim());
                    BranchGridLookup.gridView.SelectItemsByKey(BranchmapCount);
                }
            }
            else
            {
                BranchGridLookup.Clear();
                BranchGridLookup.gridView.UnselectRows();
            }
            //var pm = document.getElementById("ddlPManager");
            //obj.Proj_Managerid = f.options[f.selectedIndex].value;

            //var pm = document.getElementById("ddlStatus");
            //obj.Proj_Statuscolor = f.options[f.selectedIndex].value;
            ////obj.Proj_EstimateStartdate = $("#StartDate_dt").val();
            ////obj.Proj_EstimateEnddate = $("#EndDate_dt").val();
            //obj.Proj_EstimateStartdate = StartDate_dt.GetDate();
            //obj.Proj_EstimateEnddate = EndDate_dt.GetDate();

            //obj.Proj_Estimatehours = $("#Proj_Estimatehours").val();
            //obj.Proj_EstimatelabourCost = $("#Proj_EstimatelabourCost").val();
            //obj.Proj_EstimateExpenseCost = $("#Proj_EstimateExpenseCost").val();
            //obj.proj_EstimateTotCost = $("#proj_EstimateTotCost").val();
            ////obj.ActualStartDate_dt = $("#ActualStartDate_dt").val();

            ////obj.ActualEndDate_dt = $("#ActualEndDate_dt").val();
            //obj.Proj_ActualStartdate = ActualStartDate_dt.GetDate();

            //obj.Proj_ActualEndDate = ActualEndDate_dt.GetDate();
            if (document.getElementById("exampleModalLabelForModify").style.display == "block") {
                $("#Proj_Code").prop('disabled', true);

                $("#Proj_Name").prop('disabled', true);
                $("#ddlBranch").prop('disabled', true);
                $("#CustomerTxt").prop('disabled', true);
            }

            // $("#projectMod").modal('toggle');
        }
    },
    error: function (response) {
        // alert(response);
        jAlert("Please try again later.");
        //LoadingPanel.Hide();
    }
});
}
}

function ApprovalProjectDetails() {
    var otherdt = {};
    otherdt.Action = "Approval";
    otherdt.Proj_Code = $("#Proj_Code").val();
    otherdt.Proj_Id = $("#ProjId").val();
    otherdt.Approved_by = $("#ddlUser").val();
    otherdt.Approved_On = Approved_dt.GetDate();
    otherdt.Remarks = $("#Remarks").val();
    $.ajax({
        type: "POST",
        url: "@Url.Action("ApprovalSaveData", "Project")",
        data: JSON.stringify(otherdt),
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (response) {

        var status = response;
        if (status == "Data Save") {
            jAlert(status);
            $("#ProjId").val("");
            // gridBookingStatusList.Refresh();
            $("#projectMod").modal('hide');
            gridProjectList.Refresh();
            gridProjectList.Refresh();
        }


    },
    error: function (response) {
        // alert(response);
        jAlert("Please try again later.");
        //LoadingPanel.Hide();
    }
});
}
function RejectProjectDetails() {
    var otherdt = {};
    otherdt.Action = "Reject";
    otherdt.Proj_Code = $("#Proj_Code").val();
    otherdt.Proj_Id = $("#ProjId").val();

    $.ajax({
        type: "POST",
        url: "@Url.Action("RejedctedSaveData", "Project")",
        data: JSON.stringify(otherdt),
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (response) {

        var status = response;
        if (status == "Data Save") {
            jAlert(status);
            $("#ProjId").val("");
            // gridBookingStatusList.Refresh();
            $("#projectMod").modal('hide');
            gridProjectList.Refresh();
            gridProjectList.Refresh();
        }


    },
    error: function (response) {
        // alert(response);
        jAlert("Please try again later.");
        //LoadingPanel.Hide();
    }
});
}


function ModelHide() {
    $('.widgetDrop').hide();
}


function OpenADDProject() {

    BranchGridLookup.SetEnabled(true);
    ProjectGridLookup.SetEnabled(false);

    $('#projectMod').on('shown.bs.modal', function () {
        $('#ddlNumbscheme').focus();
    })


    $('.widgetizeForm').find('.parentLi').removeClass('ready visited on');
    $('#liNew').addClass('ready on');



    $("#ProjId").val("");

    projectCode = [];
    projectCode.length = 0;

    document.getElementById("dvNumberscheme").disabled = true;
    LoadingPanel.Show();
    $("#ddlNumbscheme").val("0");
    $("#Proj_Name").val("");
    $("#Proj_Description").val("");
    $("#Proj_Code").val("");
    $("#CustomerId").val("");

    //chinmoy start


    $("#BG_BGGroup").val('');
    $("#BG_BGType").val('');
    $("#BG_Percentage").val('');
    $("#BG_BGValue").val('');
    $("#BG_BGStatus").val('');

    $("#Terms_DefectLibilityPeriodRemarks").val('');
    $("#Terms_LiqDamage").val('');
    $("#Terms_Payment").val('');
    $("#Terms_OrderType").val('');
    $("#Terms_NatureWork").val('');

    //End


    $("#ddlBranch").val(0);
    $("#ddlPManager").val(0);
    $("#drdStatus").val(0);
    var today = new Date();
    ProjectGridLookup.Clear();
    ProjectGridLookup.gridView.UnselectRows();

    BranchGridLookup.Clear();
    BranchGridLookup.gridView.UnselectRows();
      
    CustomerTxt.SetText("");
    StartDate_dt.SetDate(today);
    EndDate_dt.SetDate(today);
    $("#drdStatus").val(0);
    $("#drdCal").val(0);
    //$("#Proj_Estimatehours").val(0);
    $("#estHrs").val(0);
    $("#estmm").val(0);
    $("#Proj_EstimatelabourCost").val(0);
    $("#Proj_EstimateExpenseCost").val(0);
    $("#proj_EstimateTotCost").val(0);
    $("#Proj_Code").prop('disabled', false);

    $("#ddlHierarchy").prop('disabled', false);
    $("#Proj_Name").prop('disabled', false);
    $("#ddlBranch").prop('disabled', false);

    $("#NewNm").val("");
    $("#NewDesc").val("");
    $("#NewCust").val("");

    $("#QuaNm").val("");
    $("#QuaDesc").val("");
    $("#QuaCust").val("");
    $("#ddlHierarchy").val(0);

    CustomerTxt.SetEnabled(true);
    ActualStartDate_dt.SetDate(today);
    ActualEndDate_dt.SetDate(today);
    document.getElementById("dvNumberscheme").style.display = "block";
    document.getElementById("exampleModalLabelForApproval").style.display = "none";
    document.getElementById("dvApproval").style.display = "none";
    document.getElementById("exampleModalLabelForAdd").style.display = "block";
    document.getElementById("exampleModalLabelForModify").style.display = "none";
    document.getElementById("exampleModalLabelForView").style.display = "none";
    $("#btnSave").removeClass('hide');
    $("#btncancel").removeClass('hide');
    $("#btnApproved").addClass('hide');
    $("#btnReject").addClass('hide');
    LoadingPanel.Hide();

}

function OpenProjectforView(obj) {
    BranchGridLookup.gridView.Refresh();
    var MultipleBranchProject = '@ViewBag.MultipleBranchProject';
    if (MultipleBranchProject != null && MultipleBranchProject != "" && MultipleBranchProject != "No" && MultipleBranchProject != "NO") {
        $('#drdUnit').hide();
        $('#BranchGridLookup').show();
    }
    else {
        $('#drdUnit').show();
        $('#BranchGridLookup').hide();
    }
    BranchGridLookup.SetEnabled(false);
    document.getElementById("dvNumberscheme").style.display = "none";
    document.getElementById("exampleModalLabelForApproval").style.display = "none";
    document.getElementById("dvApproval").style.display = "block";
    document.getElementById("exampleModalLabelForView").style.display = "block";
    document.getElementById("exampleModalLabelForAdd").style.display = "none";
    document.getElementById("exampleModalLabelForModify").style.display = "none";
    $.ajax({
        type: "POST",
        url: "@Url.Action("ViewDataShow", "Project")",
        data: JSON.stringify({ Proj_Id: obj }),
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (response) {
        // LoadingPanel.Hide();
        debugger;
        var status = response;
        var str = "";

        if (status != null) {

            $("#Proj_Name").val(status.Proj_Name);
            $("#Proj_Description").val(status.Proj_Description);
            $("#Proj_Code").val(status.Proj_Code);
            $("#CustomerId").val(status.Cnt_InternalId);

            $("#ddlBranch").val(status.Proj_Bracnchid);
            $("#ddlPManager").val(status.Proj_Managerid);
            $("#drdStatus").val(status.Proj_Statuscolor);
            var startdt = new Date(parseInt(status.Proj_EstimateStartdate.substr(6)));
            var Enddt = new Date(parseInt(status.Proj_EstimateEnddate.substr(6)));
            CustomerTxt.SetText(status.Customer);
            StartDate_dt.SetDate(startdt);
            EndDate_dt.SetDate(Enddt);
            $("#drdStatus").val(status.Proj_Statuscolor);
            $("#drdCal").val(status.Proj_Calender);
            // $("#Proj_Estimatehours").val(status.Proj_Estimatehours);


            Terms_DefectLibilityPeriodDate.SetText(status.SaveEditTerms_DefectLibilityPeriodDate);
            Terms_DefectLibilityPeriodToDate.SetText(status.SaveEditTerms_DefectLibilityPeriodToDate);
            $("#Terms_DefectLibilityPeriodRemarks").val(status.SaveTerms_DefectLibilityPeriodRemarks);
            $("#Terms_LiqDamage").val(status.SaveTerms_LiqDamage);
            Terms_LiqDamageAppDate.SetText(status.SaveEditTerms_LiqDamageAppDate);
            $("#Terms_Payment").val(status.SaveTerms_Payment);
            $("#Terms_OrderType").val(status.SaveTerms_OrderType);
            $("#Terms_NatureWork").val(status.SaveTerms_NatureWork);



            $("#estHrs").val(status.Proj_estimateHH);
            $("#estmm").val(status.Proj_estimateMM);
            $("#Proj_EstimatelabourCost").val(status.Proj_EstimatelabourCost);
            $("#Proj_EstimateExpenseCost").val(status.Proj_EstimateExpenseCost);
            $("#proj_EstimateTotCost").val(status.proj_EstimateTotCost);
            var Acstartdt = new Date(parseInt(status.Proj_ActualStartdate.substr(6)));
            var Acendtdt = new Date(parseInt(status.Proj_ActualEndDate.substr(6)));
            ActualStartDate_dt.SetDate(Acstartdt);
            ActualEndDate_dt.SetDate(Acendtdt);
            $("#ddlUser").val(status.Approved_by);
            var approvedt = new Date(parseInt(status.Approved_On.substr(6)));
            Approved_dt.SetDate(approvedt);
            $("#Remarks").val(status.Remarks);
            $('#projectProgress li').removeClass('on visited')
            $('.widgetDrop').hide();
            if (status.projStage_Desc == "New") {
                $('#liNew.parentLi').removeClass('on').addClass('visited');
                $('#liQualify.parentLi').addClass('ready on');
            }
            else if (status.projStage_Desc == "Qualify") {
                $('#liNew.parentLi').removeClass('on').addClass('visited');
                $('#liQualify.parentLi').addClass('ready on');
            }

            else if (status.projStage_Desc == "Planning") {
                $('#liNew.parentLi').removeClass('on').addClass('visited');
                $('#liQualify.parentLi').removeClass('on').addClass('visited');
                $('#liPlanning.parentLi').addClass('ready on');
            }
            else if (status.projStage_Desc == "Execution") {
                $('#liNew.parentLi').removeClass('on').addClass('visited');
                $('#liQualify.parentLi').removeClass('on').addClass('visited');
                $('#liPlanning.parentLi').removeClass('on').addClass('visited');
                $('#liExe.parentLi').addClass('ready on');
            }

            else if (status.projStage_Desc == "Deliver") {
                $('#liNew.parentLi').removeClass('on').addClass('visited');
                $('#liQualify.parentLi').removeClass('on').addClass('visited');
                $('#liPlanning.parentLi').removeClass('on').addClass('visited');
                $('#liExe.parentLi').removeClass('on').addClass('visited');
                $('#liDeliver.parentLi').addClass('ready on');
            }

            else if (status.projStage_Desc == "Complete") {
                $('#liNew.parentLi').removeClass('on').addClass('visited');
                $('#liQualify.parentLi').removeClass('on').addClass('visited');
                $('#liPlanning.parentLi').removeClass('on').addClass('visited');
                $('#liExe.parentLi').removeClass('on').addClass('visited');
                $('#liDeliver.parentLi').removeClass('on').addClass('visited');
                $('#liComplete.parentLi').addClass('ready on');
            }

            else if (status.projStage_Desc == "Close") {
                $('#liNew.parentLi').removeClass('on').addClass('visited');
                $('#liQualify.parentLi').removeClass('on').addClass('visited');
                $('#liPlanning.parentLi').removeClass('on').addClass('visited');
                $('#liExe.parentLi').removeClass('on').addClass('visited');
                $('#liDeliver.parentLi').removeClass('on').addClass('visited');
                $('#liComplete.parentLi').removeClass('on').addClass('visited');
                $('#liClose.parentLi').addClass('ready on');
            }





            $("#NewNm").val(status.Proj_Name);
            $("#NewDesc").val(status.Proj_Description);
            $("#NewCust").val(status.Customer);

            $("#NewNm").prop("disabled", true);
            $("#NewDesc").prop("disabled", true);
            $("#NewCust").prop("disabled", true);

            $("#QuaNm").val(status.Proj_Name);
            $("#QuaDesc").val(status.Proj_Description);
            $("#QuaCust").val(status.Customer);

            $("#QuaNm").prop("disabled", true);
            $("#QuaDesc").prop("disabled", true);
            $("#QuaCust").prop("disabled", true);


            $("#PlanningNm").val(status.Proj_Name);
            $("#PlanningDesc").val(status.Proj_Description);
            $("#PlanningCust").val(status.Customer);

            $("#PlanningNm").prop("disabled", true);
            $("#PlanningDesc").prop("disabled", true);
            $("#PlanningCust").prop("disabled", true);

            $("#executionNm").val(status.Proj_Name);
            $("#executionDesc").val(status.Proj_Description);
            $("#executionCust").val(status.Customer);

            $("#executionNm").prop("disabled", true);
            $("#executionDesc").prop("disabled", true);
            $("#executionCust").prop("disabled", true);


            $("#DeliverNm").val(status.Proj_Name);
            $("#DeliverDesc").val(status.Proj_Description);
            $("#DeliverCust").val(status.Customer);

            $("#DeliverNm").prop("disabled", true);
            $("#DeliverDesc").prop("disabled", true);
            $("#DeliverCust").prop("disabled", true);



            $("#CompleteNm").val(status.Proj_Name);
            $("#CompleteDesc").val(status.Proj_Description);
            $("#CompleteCust").val(status.Customer);

            $("#CompleteNm").prop("disabled", true);
            $("#CompleteDesc").prop("disabled", true);
            $("#CompleteCust").prop("disabled", true);



            $("#CloseNm").val(status.Proj_Name);
            $("#CloseDesc").val(status.Proj_Description);
            $("#CloseCust").val(status.Customer);

            $("#CloseNm").prop("disabled", true);
            $("#CloseDesc").prop("disabled", true);
            $("#CloseCust").prop("disabled", true);



            var OrderCount = [];



            ProjectGridLookup.gridView.Refresh();
            OrderCount = status.Order_Id.split(',');
            for (var i = 0; i < OrderCount.length; i++) {
                //ProjectGridLookup.gridView.Keyfield(OrderCount[i].trim());
                ProjectGridLookup.gridView.SelectItemsByKey(OrderCount);
            }


            var BranchmapCount = [];
            // BranchGridLookup.gridView.Refresh();
            BranchmapCount = status.BranchMap_Id.split(',');
            if (BranchmapCount != "") {
                for (var i = 0; i < BranchmapCount.length; i++) {
                    //ProjectGridLookup.gridView.Keyfield(OrderCount[i].trim());
                    BranchGridLookup.gridView.SelectItemsByKey(BranchmapCount);
                }
            }
            else {
                BranchGridLookup.Clear();
                BranchGridLookup.gridView.UnselectRows();
            }
            //var pm = document.getElementById("ddlPManager");
            //obj.Proj_Managerid = f.options[f.selectedIndex].value;

            //var pm = document.getElementById("ddlStatus");
            //obj.Proj_Statuscolor = f.options[f.selectedIndex].value;
            ////obj.Proj_EstimateStartdate = $("#StartDate_dt").val();
            ////obj.Proj_EstimateEnddate = $("#EndDate_dt").val();
            //obj.Proj_EstimateStartdate = StartDate_dt.GetDate();
            //obj.Proj_EstimateEnddate = EndDate_dt.GetDate();

            //obj.Proj_Estimatehours = $("#Proj_Estimatehours").val();
            //obj.Proj_EstimatelabourCost = $("#Proj_EstimatelabourCost").val();
            //obj.Proj_EstimateExpenseCost = $("#Proj_EstimateExpenseCost").val();
            //obj.proj_EstimateTotCost = $("#proj_EstimateTotCost").val();
            ////obj.ActualStartDate_dt = $("#ActualStartDate_dt").val();

            ////obj.ActualEndDate_dt = $("#ActualEndDate_dt").val();
            //obj.Proj_ActualStartdate = ActualStartDate_dt.GetDate();

            //obj.Proj_ActualEndDate = ActualEndDate_dt.GetDate();
            if (document.getElementById("exampleModalLabelForModify").style.display == "block") {
                $("#Proj_Code").prop('disabled', true);

                $("#Proj_Name").prop('disabled', true);
                $("#ddlBranch").prop('disabled', true);

                CustomerTxt.SetEnabled(false);
            }


            $("#btnSave").addClass('hide');
            $("#btncancel").addClass('hide');
            $("#btnApproved").addClass('hide');
            $("#btnReject").addClass('hide');
            $("#projectMod").modal('toggle');
        }
    },
    error: function (response) {
        // alert(response);
        jAlert("Please try again later.");
        //LoadingPanel.Hide();
    }
});
}

function TermsConditionscancel() {
    $("#TermsConditionseModal").hide();
}

function TermsConditionsSave() {
    $("#TermsConditionseModal").hide();
}

function AddTermsDetails() {

    var bg = $("#BG_BGGroup").val();
    if (bg == "") {
        //ctxtBGGroup.SetFocus();
        $("#BG_BGGroup").focus();
        return;
    }

    var ob = {};
    ob.BG_BGGroup = $("#BG_BGGroup").val();
    ob.BG_BGType = $("#BG_BGType").val();
    ob.BG_Percentage = $("#BG_Percentage").val();
    ob.BG_BGValue = $("#BG_BGValue").val();
    ob.BG_BGStatus = $("#BG_BGStatus").val();

    var ValidityFromDate = BG_BGValidfrom.GetDate();
    var ValidityToDate = BG_BGValidUpTo.GetDate();
    if (ValidityFromDate != null) {
        ob.BG_BGValidfrom = GetDateFormat(ValidityFromDate);
    }
    if (ValidityToDate != null) {
        ob.BG_BGValidUpTo = GetDateFormat(ValidityToDate);
    }
    ob.Terms_BankGuaranteeSL = $('#hdnGuid').val();

    $.ajax({
        type: "POST",
        url: "@Url.Action("BankDetailsSave", "Project")",
        //data: { Project: obj },
        data: JSON.stringify({ bank: ob }),
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (response) {

        var BGdata = response;

        $("#BG_BGGroup").val('');
        $("#BG_BGType").val('');
        $("#BG_Percentage").val('');
        $("#BG_BGValue").val('');
        $("#BG_BGStatus").val('');


        TermsConitionsPartial.Refresh();

    },
    error: function (response) {
        jAlert("Please try again later.");
    }
});

}


function GetDateFormat(today) {
    if (today != "") {
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!

        var yyyy = today.getFullYear();
        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }
        //  today = dd + '-' + mm + '-' + yyyy;
        today = yyyy + '-' + mm + '-' + dd;
    }

    return today;
}

function OpenTermsConitionsforDelete(ob) {
    jConfirm('Confirm Delete?', 'Alert', function (r) {
        if (r) {
            $.ajax({
                type: "POST",
                url: "@Url.Action("DeleteTermsConditionsData", "Project")",
                data: JSON.stringify({ Terms_BankGuaranteeSL: ob }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var list = msg.d;

                jAlert("Bank guarantee  Remove Successfully.");

                TermsConitionsPartial.Refresh();

            }
        });
    }
    else {

}
});
}

function Termsconditions() {
    TermsConitionsPartial.Refresh();
    TermsConitionsPartial.Refresh();
}

function OpenProjectforDelete(obj) {
    jConfirm('Confirm Delete?', 'Alert', function (r) {
        if (r) {

            $.ajax({
                type: "POST",
                url: "@Url.Action("DeleteData", "Project")",
                data: JSON.stringify({ Proj_Id: obj }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                var status = response;
                jAlert(status);

                gridProjectList.Refresh();
                gridProjectList.Refresh();
            },
            error: function (response) {
                //alert(response);
                jAlert("Please try again later.");
            }
        });
    }
    else {
    // alert("false");
}
});

}

function ProjectCancel(obj) {
    var cancelcloseProjectResult = {};
    var cancelcloseProject = {};
    cancelcloseProject.Code = obj;
    $.ajax({
        async: false,
        type: "POST",
        url: "@Url.Action("CancelProjectCheck", "Project")",
        data: JSON.stringify(cancelcloseProject),
    contentType: "application/json; charset=utf-8",
    datatype: "JSON",

    success: function (data) {

        cancelcloseProjectResult = data;



    },
    error: function (data) {

        jAlert("Please try again later");
    }
});

if (cancelcloseProjectResult.split("~")[0] == "1" || cancelcloseProjectResult.split("~")[1] == "Finish") {
    jAlert("Project is already cancelled.");
}
else {
    jConfirm('Do you want to cancel the project ?', 'Confirm Dialog', function (r) {
        if (r == true) {
            $("#ProIdCanRemarks").val(obj);
            $("#CloseModal").modal('toggle');

        }
        else {
            return false;
        }
    });
}
}


function CancelRemarks_save() {
    var flag = true;
    if ($("#txtCanCelRemarks").val() == "") {
        flag = false;
        $("#txtCanCelRemarks").focus();
    }
    else {
        var obResult = {};
        obResult.Proj_Id = $("#ProIdCanRemarks").val();
        obResult.CancelRemarks = $("#txtCanCelRemarks").val();

        $.ajax({
            type: "POST",
            url: "@Url.Action("SaveCancelCloseRemarks", "Project")",
            data: JSON.stringify(obResult),
        contentType: "application/json; charset=utf-8",
        datatype: "JSON",

        success: function (data) {

            var CancelRemarks = data;

            $("#CloseModal").modal('hide');
            gridProjectList.Refresh();
            gridProjectList.Refresh();

        },
        error: function (data) {
            jAlert("Please try again later");
        }
    });
}

return flag;
}


function OpenProjectforEdit(obj) {
    BranchGridLookup.gridView.Refresh();
    var MultipleBranchProject = '@ViewBag.MultipleBranchProject';
    if (MultipleBranchProject != null && MultipleBranchProject != "" && MultipleBranchProject != "No" && MultipleBranchProject != "NO") {
        $('#drdUnit').hide();
        $('#BranchGridLookup').show();
    }
    else {
        $('#drdUnit').show();
        $('#BranchGridLookup').hide();
    }
    BranchGridLookup.SetEnabled(false);
    var cancelProjectResult = {};
    var cancelProjectData = {};
    cancelProjectData.Code = obj;
    // FinishClick();
    $.ajax({
        async: false,
        type: "POST",
        url: "@Url.Action("CancelProjectCheck", "Project")",
        data: JSON.stringify(cancelProjectData),
    contentType: "application/json; charset=utf-8",
    datatype: "JSON",

    success: function (data) {

        cancelProjectResult = data;



    },
    error: function (data) {

        jAlert("Please try again later");
    }
});





if (cancelProjectResult.split("~")[0] == "1" || cancelProjectResult.split("~")[1] == "Finish") {
    jAlert("Project cancelled/closed, Cannot modify data.");
}



else {
    debugger;
    $('#projectMod').on('shown.bs.modal', function () {
        $('#Proj_Description').focus();
    });
    $("#ProjId").val(obj);
    var CodeForTrans = {};
    CodeForTrans.Code = $("#ProjId").val();
    $.ajax({
        type: "POST",
        url: "@Url.Action("ProjectCodeTransactionCheck", "Project")",
        data: JSON.stringify(CodeForTrans),
    contentType: "application/json; charset=utf-8",
    datatype: "JSON",

    success: function (data) {

        ProjectCodeCheck = data;


    },
    error: function (data) {

        jAlert("Please try again later");
    }
});



$.ajax({
    type: "POST",
    url: "@Url.Action("ProjectstatusForTransaction", "Project")",
    data: JSON.stringify(CodeForTrans),
contentType: "application/json; charset=utf-8",
datatype: "JSON",

success: function (data) {

    ProjectstatusForTransaction = data;


},
error: function (data) {

    jAlert("Please try again later");
}
});



//alert(obj);
document.getElementById("dvNumberscheme").style.display = "none";
document.getElementById("exampleModalLabelForApproval").style.display = "none";
document.getElementById("dvApproval").style.display = "none";
document.getElementById("exampleModalLabelForAdd").style.display = "none";
document.getElementById("exampleModalLabelForModify").style.display = "block";
document.getElementById("exampleModalLabelForView").style.display = "none";
$("#btnSave").removeClass('hide');
$("#btncancel").removeClass('hide');
$("#btnApproved").addClass('hide');
$("#btnReject").addClass('hide');

$.ajax({
    type: "POST",
    url: "@Url.Action("ViewDataShow", "Project")",
    data: JSON.stringify({ Proj_Id: obj }),
contentType: "application/json; charset=utf-8",
dataType: "json",
success: function (response) {
    // LoadingPanel.Hide();
    debugger;
    var status = response;
    var str = "";

    if (status != null) {

        if (status.ProjectStatus == "Approved") {
            ProjectGridLookup.SetEnabled(true);
        }
        else {
            ProjectGridLookup.SetEnabled(false);
        }


        $("#Proj_Name").val(status.Proj_Name);
        $("#Proj_Description").val(status.Proj_Description);
        $("#Proj_Code").val(status.Proj_Code);
        $("#CustomerId").val(status.Cnt_InternalId);
        $("#HdnProjectapprovalstatus").val(status.ProjectStatus);
        $("#ddlBranch").val(status.Proj_Bracnchid);
        $("#ddlPManager").val(status.Proj_Managerid);
        $("#drdStatus").val(status.Proj_Statuscolor);
        $("#ddlHierarchy").val(status.Proj_Hierarchy);
        var startdt = new Date(parseInt(status.Proj_EstimateStartdate.substr(6)));
        var Enddt = new Date(parseInt(status.Proj_EstimateEnddate.substr(6)));
        CustomerTxt.SetText(status.Customer);
        StartDate_dt.SetDate(startdt);
        EndDate_dt.SetDate(Enddt);

        $("#drdStatus").val(status.Proj_Statuscolor);
        $("#drdCal").val(status.Proj_Calender);
        //$("#Proj_Estimatehours").val(status.Proj_Estimatehours);
        $("#estHrs").val(status.Proj_estimateHH);
        $("#estmm").val(status.Proj_estimateMM);
        $("#Proj_EstimatelabourCost").val(status.Proj_EstimatelabourCost);
        $("#Proj_EstimateExpenseCost").val(status.Proj_EstimateExpenseCost);
        $("#proj_EstimateTotCost").val(status.proj_EstimateTotCost);
        var Acstartdt = new Date(parseInt(status.Proj_ActualStartdate.substr(6)));
        var Acendtdt = new Date(parseInt(status.Proj_ActualEndDate.substr(6)));
        ActualStartDate_dt.SetDate(Acstartdt);
        ActualEndDate_dt.SetDate(Acendtdt);

        Terms_DefectLibilityPeriodDate.SetText(status.SaveEditTerms_DefectLibilityPeriodDate);
        Terms_DefectLibilityPeriodToDate.SetText(status.SaveEditTerms_DefectLibilityPeriodToDate);
        $("#Terms_DefectLibilityPeriodRemarks").val(status.SaveTerms_DefectLibilityPeriodRemarks);
        $("#Terms_LiqDamage").val(status.SaveTerms_LiqDamage);
        Terms_LiqDamageAppDate.SetText(status.SaveEditTerms_LiqDamageAppDate);
        $("#Terms_Payment").val(status.SaveTerms_Payment);
        $("#Terms_OrderType").val(status.SaveTerms_OrderType);
        $("#Terms_NatureWork").val(status.SaveTerms_NatureWork);


        //debugger;

        $('#projectProgress li').removeClass('on visited');
        $('.widgetDrop').hide();

        if (status.projStage_Desc == "New") {
            $('#liNew.parentLi').removeClass('on').addClass('visited');
            $('#liQualify.parentLi').addClass('ready on');
        }
        else if (status.projStage_Desc == "Qualify") {
            $('#liNew.parentLi').removeClass('on').addClass('visited');
            $('#liQualify.parentLi').addClass('ready on');
        }

        else if (status.projStage_Desc == "Planning") {
            $('#liNew.parentLi').removeClass('on').addClass('visited');
            $('#liQualify.parentLi').removeClass('on').addClass('visited');
            $('#liPlanning.parentLi').addClass('ready on');
        }
        else if (status.projStage_Desc == "Execution") {
            $('#liNew.parentLi').removeClass('on').addClass('visited');
            $('#liQualify.parentLi').removeClass('on').addClass('visited');
            $('#liPlanning.parentLi').removeClass('on').addClass('visited');
            $('#liExe.parentLi').addClass('ready on');
        }

        else if (status.projStage_Desc == "Deliver") {
            $('#liNew.parentLi').removeClass('on').addClass('visited');
            $('#liQualify.parentLi').removeClass('on').addClass('visited');
            $('#liPlanning.parentLi').removeClass('on').addClass('visited');
            $('#liExe.parentLi').removeClass('on').addClass('visited');
            $('#liDeliver.parentLi').addClass('ready on');
        }

        else if (status.projStage_Desc == "Complete") {
            $('#liNew.parentLi').removeClass('on').addClass('visited');
            $('#liQualify.parentLi').removeClass('on').addClass('visited');
            $('#liPlanning.parentLi').removeClass('on').addClass('visited');
            $('#liExe.parentLi').removeClass('on').addClass('visited');
            $('#liDeliver.parentLi').removeClass('on').addClass('visited');
            $('#liComplete.parentLi').addClass('ready on');
        }

        else if (status.projStage_Desc == "Close") {
            $('#liNew.parentLi').removeClass('on').addClass('visited');
            $('#liQualify.parentLi').removeClass('on').addClass('visited');
            $('#liPlanning.parentLi').removeClass('on').addClass('visited');
            $('#liExe.parentLi').removeClass('on').addClass('visited');
            $('#liDeliver.parentLi').removeClass('on').addClass('visited');
            $('#liComplete.parentLi').removeClass('on').addClass('visited');
            $('#liClose.parentLi').addClass('ready on');
        }


        $("#NewNm").val(status.Proj_Name);
        $("#NewDesc").val(status.Proj_Description);
        $("#NewCust").val(status.Customer);

        $("#NewNm").prop("disabled", true);
        $("#NewDesc").prop("disabled", true);
        $("#NewCust").prop("disabled", true);

        $("#QuaNm").val(status.Proj_Name);
        $("#QuaDesc").val(status.Proj_Description);
        $("#QuaCust").val(status.Customer);

        $("#QuaNm").prop("disabled", true);
        $("#QuaDesc").prop("disabled", true);
        $("#QuaCust").prop("disabled", true);

        $("#PlanningNm").val(status.Proj_Name);
        $("#PlanningDesc").val(status.Proj_Description);
        $("#PlanningCust").val(status.Customer);

        $("#PlanningNm").prop("disabled", true);
        $("#PlanningDesc").prop("disabled", true);
        $("#PlanningCust").prop("disabled", true);

        $("#executionNm").val(status.Proj_Name);
        $("#executionDesc").val(status.Proj_Description);
        $("#executionCust").val(status.Customer);

        $("#executionNm").prop("disabled", true);
        $("#executionDesc").prop("disabled", true);
        $("#executionCust").prop("disabled", true);


        $("#DeliverNm").val(status.Proj_Name);
        $("#DeliverDesc").val(status.Proj_Description);
        $("#DeliverCust").val(status.Customer);

        $("#DeliverNm").prop("disabled", true);
        $("#DeliverDesc").prop("disabled", true);
        $("#DeliverCust").prop("disabled", true);



        $("#CompleteNm").val(status.Proj_Name);
        $("#CompleteDesc").val(status.Proj_Description);
        $("#CompleteCust").val(status.Customer);

        $("#CompleteNm").prop("disabled", true);
        $("#CompleteDesc").prop("disabled", true);
        $("#CompleteCust").prop("disabled", true);



        $("#CloseNm").val(status.Proj_Name);
        $("#CloseDesc").val(status.Proj_Description);
        $("#CloseCust").val(status.Customer);

        $("#CloseNm").prop("disabled", true);
        $("#CloseDesc").prop("disabled", true);
        $("#CloseCust").prop("disabled", true);



        var OrderCount = [];



        ProjectGridLookup.gridView.Refresh();
        OrderCount = status.Order_Id.split(',');
        for (var i = 0; i < OrderCount.length; i++) {
            //ProjectGridLookup.gridView.Keyfield(OrderCount[i].trim());
            ProjectGridLookup.gridView.SelectItemsByKey(OrderCount);
        }

        var BranchmapCount = [];
        // BranchGridLookup.gridView.Refresh();
        BranchmapCount = status.BranchMap_Id.split(',');
        if (BranchmapCount != "") {
            for (var i = 0; i < BranchmapCount.length; i++) {
                //ProjectGridLookup.gridView.Keyfield(OrderCount[i].trim());
                BranchGridLookup.gridView.SelectItemsByKey(BranchmapCount);
            }
        }
        else {
            BranchGridLookup.Clear();
            BranchGridLookup.gridView.UnselectRows();
        }
        //var pm = document.getElementById("ddlPManager");
        //obj.Proj_Managerid = f.options[f.selectedIndex].value;

        //var pm = document.getElementById("ddlStatus");
        //obj.Proj_Statuscolor = f.options[f.selectedIndex].value;
        ////obj.Proj_EstimateStartdate = $("#StartDate_dt").val();
        ////obj.Proj_EstimateEnddate = $("#EndDate_dt").val();
        //obj.Proj_EstimateStartdate = StartDate_dt.GetDate();
        //obj.Proj_EstimateEnddate = EndDate_dt.GetDate();

        //obj.Proj_Estimatehours = $("#Proj_Estimatehours").val();
        //obj.Proj_EstimatelabourCost = $("#Proj_EstimatelabourCost").val();
        //obj.Proj_EstimateExpenseCost = $("#Proj_EstimateExpenseCost").val();
        //obj.proj_EstimateTotCost = $("#proj_EstimateTotCost").val();
        ////obj.ActualStartDate_dt = $("#ActualStartDate_dt").val();

        ////obj.ActualEndDate_dt = $("#ActualEndDate_dt").val();
        //obj.Proj_ActualStartdate = ActualStartDate_dt.GetDate();

        //obj.Proj_ActualEndDate = ActualEndDate_dt.GetDate();

        if (ProjectCodeCheck == 1 || ProjectstatusForTransaction == 1) {
            //$("#Proj_Name").prop('disabled', true);
            CustomerTxt.SetEnabled(false);
            $("#ddlHierarchy").prop("disabled", true);
        }
        else {
            //$("#Proj_Name").prop('disabled', false);
            CustomerTxt.SetEnabled(true);
            $("#ddlHierarchy").prop("disabled", false);
        }
        //if (ProjectstatusForTransaction == 1)
        //{
        //    CustomerTxt.SetEnabled(false);
        //}
        //else
        //{
        //    CustomerTxt.SetEnabled(true);
        //}
        $("#Proj_Code").prop('disabled', true);


        $("#ddlBranch").prop('disabled', true);




        $("#btnSave").removeClass('hide');
        $("#btncancel").removeClass('hide');
        $("#projectMod").modal('toggle');
    }
},
error: function (response) {
    // alert(response);
    jAlert("Please try again later.");
    //LoadingPanel.Hide();
}
});
}
}




$(document).ready(function () {
    $('.widgetizeForm li .parentLabel').click(function () {
        //alert($(this).attr('class'));
        if ($(this).parent('li').hasClass('ready')) {
            //$('.widgetDrop').hide();
            // $(this).parent('li').addClass('on');
            $('.widgetDrop').hide();
            $(this).parent('li').find('.widgetDrop').toggle();
        } else {
            $(this).parent('li').find('.widgetDrop').hide();
        }
    });
    $('.widgetDrop .nextStage').click(function () {
        //alert($(this).attr('class'));
        if ($(this).hasClass('act')) {
            $(this).closest('li.parentLi').next('.parentLi').addClass('ready');
            $(this).closest('li.parentLi').next('.parentLi').find('.widgetDrop').show();
            $(this).closest('li.parentLi').next('.parentLi').addClass('on');

            $(this).closest('li.parentLi').removeClass('on').addClass('visited');
            $(this).closest('li.parentLi').find('.widgetDrop').hide();
        } else {
            $(this).parent('li').find('.widgetDrop').hide();
        }
    });
    $('.closeDropCont').click(function () {
        $(this).closest('li.parentLi').find('.widgetDrop').hide();
    });
});



//Surojit

function CalculateTotalCost() {

    var Proj_EstimatelabourCost = $('#Proj_EstimatelabourCost').val();
    var Proj_EstimateExpenseCost = $('#Proj_EstimateExpenseCost').val();
    var proj_EstimateTotCost = 0;

    if (parseFloat(Proj_EstimatelabourCost) > 0 || parseFloat(Proj_EstimateExpenseCost) > 0) {
        proj_EstimateTotCost = parseFloat(Proj_EstimatelabourCost) + parseFloat(Proj_EstimateExpenseCost);
        $('#proj_EstimateTotCost').val(parseFloat(proj_EstimateTotCost).toFixed(2));
    }
    else {
        $('#proj_EstimateTotCost').val(parseFloat(0).toFixed(2));
    }

}


//Surojit




function SaveProjectDetails() {
    debugger;
    var obj = {};
    obj.Proj_Name = $("#Proj_Name").val();
    obj.Proj_Description = $("#Proj_Description").val();
    obj.Proj_Code = $("#Proj_Code").val().trim();
    obj.Cnt_InternalId = $("#CustomerId").val();
    var f = document.getElementById("ddlBranch");
    obj.Proj_Bracnchid = $("#ddlBranch").val();
    obj.NumberSchemaId = $("#ddlNumbscheme").val().split('~')[0];
    if ($("#ProjId").val() == "") {
        obj.Action = "Add"

    }
    else {
        obj.Action = "Edit"
        obj.Proj_Id = $("#ProjId").val();
    }

    //var pm = document.getElementById("ddlPManager");
    //obj.Proj_Managerid = f.options[f.selectedIndex].value;

    //var pm = document.getElementById("ddlStatus");
    //obj.Proj_Statuscolor = f.options[f.selectedIndex].value;
    obj.Proj_Managerid = $("#ddlPManager").val();
    obj.Proj_Statuscolor = $("#drdStatus").val();
    obj.Proj_Calender = $("#drdCal").val();
    //obj.Proj_EstimateStartdate = $("#StartDate_dt").val();
    //obj.Proj_EstimateEnddate = $("#EndDate_dt").val();
    obj.Proj_EstimateStartdate = StartDate_dt.GetDate();
    obj.Proj_EstimateEnddate = EndDate_dt.GetDate();

    //obj.Proj_Estimatehours = $("#Proj_Estimatehours").val();
    var estHrs = $("#estHrs").val();
    var estmm = $("#estmm").val();
    //var EstInMints = (estHrs * 60);
    //var EstInMintsTot = EstInMints + estmm
    obj.Proj_estimateHH = estHrs;
    obj.Proj_estimateMM = estmm;
    obj.Proj_EstimatelabourCost = $("#Proj_EstimatelabourCost").val();
    obj.Proj_EstimateExpenseCost = $("#Proj_EstimateExpenseCost").val();
    obj.proj_EstimateTotCost = $("#proj_EstimateTotCost").val();
    //obj.ActualStartDate_dt = $("#ActualStartDate_dt").val();

    //obj.ActualEndDate_dt = $("#ActualEndDate_dt").val();
    obj.Proj_ActualStartdate = ActualStartDate_dt.GetDate();

    obj.Proj_ActualEndDate = ActualEndDate_dt.GetDate();
    if ($("#ddlUser").val() != "") {
        obj.Approved_by = $("#ddlUser").val();
    }
    obj.Approved_On = Approved_dt.GetDate();
    obj.Remarks = $("#Remarks").val();

    if ($("#ddlNumbscheme").val().split('~')[1] != "0") {
        obj.Doc_No = "Auto"
    }
    else {
        obj.Doc_No = $("#Proj_Code").val();
    }
    if ($('#projectProgress li.parentLi.on').find('.clsProgressTle').text() != "") {
        obj.projStage_Desc = $('#projectProgress li.parentLi.on').find('.clsProgressTle').text();
    }
    else {
        obj.projStage_Desc = "Finish"
    }



        
    obj.Order_Id = projectCode.toString();
    if (obj.BranchMap_Id != null && obj.BranchMap_Id != "") {
        obj.BranchMap_Id = MapBranch_id.toString();
    }
    else
    {
        var quote_Id = "";
        var quotetag_Id = BranchGridLookup.gridView.GetSelectedKeysOnPage();
        if(quotetag_Id.length>0)
        {
                
              
            for (var i = 0; i < quotetag_Id.length; i++) {
                if (quote_Id == "") {
                    quote_Id = quotetag_Id[i];
                }
                else {
                    quote_Id += ',' + quotetag_Id[i];
                }
            }
        }
        obj.BranchMap_Id = quote_Id;
    }
    obj.Proj_Hierarchy = $("#ddlHierarchy").val();
    //{
    //    obj.Doc_No = "Auto"
    //}
    //if ($("#Proj_Code").val() == "")
    //{
    //    jAlert("Please Enter Project Code.");
    //    $("#Proj_Code").focus();
    //    return false;
    //}

    if (($("#ddlNumbscheme").val() == "0") && ($("#ProjId").val() == "")) {
        jAlert("Please Select Numbering Scheme.");
        $("#ddlNumbscheme").focus();
        return false;
    }

    if ($("#ddlNumbscheme").val().split('~')[1] == "0") {
        if ($("#Proj_Code").val() == "") {
            jAlert("Please Select Project Code.");
            $("#Proj_Code").focus();
            return false;
        }
    }


    if ($("#Proj_Name").val() == "") {
        jAlert("Please Enter Project Name.");
        $("#Proj_Name").focus();
        return false;
    }
    if ($("#CustomerId").val() == "") {
        jAlert("Please Enter Customer Name.");
        $("#CustomerId").focus();
        return false;
    }
    if ($("#ddlBranch").val() == "0") {
        jAlert("Please Select Unit.");
        $("#ddlBranch").focus();
        return false;
    }

    if (StartDate_dt.GetDate() > EndDate_dt.GetDate()) {
        jAlert("Estimated start date cannot be greater than end date.");
        EndDate_dt.SetFocus();
        return false;
    }
    if (ActualStartDate_dt.GetDate() > ActualEndDate_dt.GetDate()) {
        jAlert("Actual start date cannot be greater than end date.");
        ActualEndDate_dt.SetFocus();
        return false;
    }

    obj.ProjectStatus = $("#HdnProjectapprovalstatus").val();


    obj.SaveTerms_DefectLibilityPeriodDate = Terms_DefectLibilityPeriodDate.GetDate();
    obj.SaveTerms_DefectLibilityPeriodToDate = Terms_DefectLibilityPeriodToDate.GetDate();

    obj.SaveTerms_DefectLibilityPeriodRemarks = $("#Terms_DefectLibilityPeriodRemarks").val();
    obj.SaveTerms_LiqDamage = $("#Terms_LiqDamage").val();
    obj.SaveTerms_LiqDamageAppDate = Terms_LiqDamageAppDate.GetDate();
    obj.SaveTerms_Payment = $("#Terms_Payment").val();
    obj.SaveTerms_OrderType = $("#Terms_OrderType").val();
    obj.SaveTerms_NatureWork = $("#Terms_NatureWork").val();


    $.ajax({
        type: "POST",
        url: "@Url.Action("SaveData", "Project")",
        //data: { Project: obj },
        data: JSON.stringify({ Project: obj }),
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (response) {
        debugger;
        if (response != '') {
            var outputValue = response.split("~");

            if ((outputValue[1] == "1") || (outputValue[1] == "2")) {

                if (outputValue[1] == "1") {
                    jAlert(outputValue[0]);

                }
                else if (outputValue[1] == "2") {
                    jAlert("Project Modified Successfully.");
                }

                $("#Proj_Name").val('');
                $("#Proj_Description").val('');
                $("#CustomerId").val(0);
                $("#CustomerTxt").val('');
                $("#ddlBranch").val(0);
                $("#ddlPManager").val(0);
                $("#ddlStatus").val(0);
                $("#StartDate_dt").val('');
                $("#EndDate_dt").val('');
                $("#Proj_Estimatehours").val('');
                $("#Proj_EstimatelabourCost").val('');
                $("#Proj_EstimateExpenseCost").val('');
                $("#proj_EstimateTotCost").val('');
                $("#ActualStartDate_dt").val('');
                $("#ActualEndDate_dt").val('');
                $("#ProjId").val("");
                $("#ddlHierarchy").val(0);
                //projectCode.toString() = "";
                projectCode = [];
                projectCode.length = 0;
                MapBranch_id = [];
                MapBranch_id.length = 0;
                $("#projectMod").modal('hide');
                gridProjectList.Refresh();
                gridProjectList.Refresh();
            }
            else if (outputValue[1] == "-10") {
                jAlert("Document is used in other modules. It can't remove.");
            }

        }
    },
    error: function (response) {

        jAlert("Please try again later");

    }
});



}
function ChkPad2DigitCount(e) {
    var data = parseInt($(e).val());
    $(e).val((data < 10 ? '0' : '') + data);

}
var inputQuantity = [];
$(function () {
    $(".quantity").each(function (i) {
        inputQuantity[i] = this.defaultValue;
        $(this).data("idx", i); // save this field's index to access later
    });
    $(".quantity").on("keyup", function (e) {
        var $field = $(this),
            val = this.value,
            $thisIndex = parseInt($field.data("idx"), 10); // retrieve the index
        //        window.console && console.log($field.is(":invalid"));
        //  $field.is(":invalid") is for Safari, it must be the last to not error in IE8
        if (this.validity && this.validity.badInput || isNaN(val) || $field.is(":invalid")) {
            this.value = inputQuantity[$thisIndex];
            return;
        }
        if (val.length > Number($field.attr("maxlength"))) {
            val = val.slice(0, 5);
            $field.val(val);
        }
        inputQuantity[$thisIndex] = val;
    });
});

function gridRowclick(s, e) {
    $('#gridProjectList, TermsConitionsPartial').find('tr').removeClass('rowActive');
    $('.floatedBtnArea').removeClass('insideGrid');
    //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
    $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
    $(s.GetRow(e.visibleIndex)).addClass('rowActive');
    setTimeout(function () {
        //alert('delay');
        var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
        //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
        //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
        //    setTimeout(function () {
        //        $(this).fadeIn();
        //    }, 100);
        //});
        $.each(lists, function (index, value) {
            //console.log(index);
            //console.log(value);
            setTimeout(function () {
                $(value).css({ 'opacity': '1' });
            }, 100);
        });
    }, 200);
}
function gridRowclickforterms(s, e) {
    $('#TermsConitionsPartial').find('tr').removeClass('rowActive');
    $('.floatedBtnArea').removeClass('insideGrid');
    //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
    $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
    $(s.GetRow(e.visibleIndex)).addClass('rowActive');
    setTimeout(function () {
        //alert('delay');
        var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
        //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
        //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
        //    setTimeout(function () {
        //        $(this).fadeIn();
        //    }, 100);
        //});
        $.each(lists, function (index, value) {
            //console.log(index);
            //console.log(value);
            setTimeout(function () {
                $(value).css({ 'opacity': '1' });
            }, 100);
        });
    }, 200);
}

    $(document).ready(function () {
        if ($('body').hasClass('mini-navbar')) {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 90;
            gridProjectList.SetWidth(cntWidth);
        } else {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 220;
            gridProjectList.SetWidth(cntWidth);
        }

        $('.navbar-minimalize').click(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                gridProjectList.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                gridProjectList.SetWidth(cntWidth);
            }

        });
    });
