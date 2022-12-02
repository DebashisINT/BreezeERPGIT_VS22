<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ReSchedule.aspx.cs"
    Inherits="ERP.OMS.Management.Activities.ReSchedule" EnableEventValidation="false" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchPopup.js?v=0.02"></script>
    <script>
        var isFirstTime = true;
        document.onkeydown = function (e) {
            if (event.keyCode == 65 && event.altKey == true) {

                if (document.getElementById('AddId'))
                    OnAddClick();
            }

        }
        function OnDocumentView(obj1, obj2) {
            var docid = obj1;
            //var filename;
            //var chk = obj2.includes("~");
            //if (chk) {
            //    filename = obj2.split('~')[1];
            //}
            //else {
            //    filename = obj2.split('/')[2];
            //}
            if (obj2 != '' && obj2 != null) {
                var d = new Date();
                var n = d.getFullYear();
                var url = obj2 + obj1;

                var seturl = '\\OMS\\Management\\DailyTask\\viewImage.aspx?id=' + url;
                popup.contentUrl = url;
                popup.Show();
            }
            else {
                alert('File not found.')
            }
        }

        function ShowHold()
        {
            cgridAdvanceAdj.Refresh();
        }
        function OnChangeFileInProgress() {
            $("#Attachmentdiv").removeClass('hide');
            $('#divfile').attr('style', 'display:none');
        }

        function OnChangeFileCompAttach() {
            $("#CompAttachmentdiv").removeClass('hide');
            $('#divCompAttachView').attr('style', 'display:none');
        }

        function ChangeFileOnHoldAttach() {
            $("#DivFileUnHold").removeClass('hide');
            $('#DivViewUnhold').attr('style', 'display:none');
        }

        function ChangeFileReleaseHoldAttach()
        {
            $("#DivfileOnHold").removeClass('hide');
            $('#divOnHoldAttachView').attr('style', 'display:none');
        }
        function OnChangeFileCanAttach()
        {
            $("#DivfileCancelled").removeClass('hide');
            $('#divCanAttachView').attr('style', 'display:none');
        }
        function OnChangeFileReSehedule()
        {
            $("#FileReSeheduleFile").removeClass('hide');
            $('#divReSeheduleFile').attr('style', 'display:none');
        }
        function InProgressPanelEndCall(s,e)
        {
            if (cInProPanel.cpInProTime != null)
            {
                var time = cInProPanel.cpInProTime;
                $('#timepicker1').timepicker('setTime', time);
                cInProPanel.cpInProTime = null

            } 
        }
        function CompletePanelEndCall(s,e)
        {
            if (cCompletePanel.cpCompleateTime != null)
            {
                var time = cCompletePanel.cpCompleateTime;
                $('#timepicker3').timepicker('setTime', time);
                cInProPanel.cpInProTime = null

            } 
        }

        function OnHoldPanelEndCall(s, e) {
            $("#OnHoldModal").modal('hide');
            jAlert('Update Successfully');
        }
        function CancelledPanelEndCall(s, e) {
            $("#CancelledModal").modal('hide');
            jAlert('Update Successfully');
        }
        function SaveOnHold(s, e) {
            var len = $('#fileOnHold').get(0).files.length;
            if (cOnHoldddlTechnician.GetText() == "") {
                e.processOnServer = false;
                jAlert("Please Enter Technician.");
                return false;
            }
            if (ctxtHoldRemarks.GetText() == "") {
                e.processOnServer = false;
                jAlert("Please Enter Remarks.");
                return false;
            }
            if ($("#hidFilenameOnHold").val() == "") {
                if (len == "0") {
                    e.processOnServer = false;
                    jAlert("Please Attach a File.");
                    return false;
                }
            }
            //else
            //{
            //    cOnHoldPanel.PerformCallback("OnHold");
            //}         

        }
        
        function SaveUnHold(s, e) {
            var len = $('#FileReleaseHold').get(0).files.length;
            if (cReleaseHoldddlTechnician.GetText() == "") {
                e.processOnServer = false;
                jAlert("Please Enter Technician.");
                return false;
            }
            if (ctxtReleaseHoldRemarks.GetText() == "") {
                e.processOnServer = false;
                jAlert("Please Enter Remarks.");
                return false;
            }
            if ($("#hddnReleaseHoldFile").val() == "") {
                if (len == "0") {
                    e.processOnServer = false;
                    jAlert("Please Attach a File.");
                    return false;
                }
            }
            //else
            //{
            //    cOnHoldPanel.PerformCallback("OnHold");
            //}         

        }


        function SaveCancelled(s, e) {
            var len = $('#fileCancelled').get(0).files.length;
            if (cCanddlTechnician.GetText() == "") {
                e.processOnServer = false;
                jAlert("Please Enter Technician.");
                return false;
            }
            if (ctxtReason.GetText() == "") {
                e.processOnServer = false;
                jAlert("Please Enter Reason.");
                return false;
            }
            if ($("#hddnCanAttach").val() == "") {
                if (len == "0") {
                    e.processOnServer = false;
                    jAlert("Please Attach a File.");
                    return false;
                }
            }
            //else
            //{
            //    cCancelledPanel.PerformCallback("Cancelled");
            //}         

        }


        function fn_btnValidateInProgress(s, e) {
            var InprogressTime = $('#timepicker1').val();
            $('#hdnInprogressTime').val(InprogressTime);
            
            var len = $('#InProAttachment').get(0).files.length;
            var ret = true;
            if (cddlTechnician.GetText() == "") {
                e.processOnServer = false;
                jAlert("Please Enter Technician.");
                ret = false;
                return false;
            }
            if (InprogressTime == "") {
                e.processOnServer = false;
                jAlert("Please Enter Time.");
                ret = false;
                return;
            }

            if (ctxtRemarks.GetText() == "") {
                e.processOnServer = false;
                jAlert("Please Enter Remarks.");
                ret = false;
                return false;
            }


            if ($("#hidFilenameInProgress").val() == "") {
                if (len == "0") {
                    e.processOnServer = false;
                    jAlert("Please Attach a File.");
                    ret = false;
                    return false;
                }
            }

        }
        function fn_btnValidate(s, e) {
            var InprogressTime2 = $('#timepicker2').val();
            $('#hdnComStartTime').val(InprogressTime2);
            var InprogressTime3 = $('#timepicker3').val();
            $('#hdnComEndTime').val(InprogressTime3);

            var ret = true;
            var len = $('#CompAttachment').get(0).files.length;

            if (cCompddlTechnician.GetText() == "") {
                e.processOnServer = false;
                jAlert("Please Enter Technician.");
                ret = false;
                return false;
            }
            if (InprogressTime3 == "") {
                e.processOnServer = false;
                jAlert("Please Enter Complete Time.");
                ret = false;
                return;
            }
            if (ctxtCompleteRemarks.GetText() == "") {
                e.processOnServer = false;
                jAlert("Please Enter Remarks.");
                ret = false;
                return false;
            }
            if ($("#hidFilenameCompAttach").val() == "") {
                if (len == "0") {
                    e.processOnServer = false;
                    jAlert("Please Attach a File.");
                    ret = false;
                    return false;
                }
            }

        }
        $(document).ready(function () {
            $('#CustModel').on('shown.bs.modal', function () {
                $('#txtCustSearch').focus();
            })


        });
        function ShowInProgress() {
            cgridAdvanceAdj.Refresh();
        }

        function onlyNumbers(evt) {
            var e = event || evt; // for trans-browser compatibility
            var charCode = e.which || e.keyCode;

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;

        }
        function GetFileSize() {
            var maxFileSize = 2; // 2MB

            //if ($("#InProAttachment")[0].files[0].size < maxFileSize) {
            //    $('#MandatoryFileSize').attr('style', 'display:none');
            //} else {
            //    $('#MandatoryFileSize').attr('style', 'display:block');
            //    $("#InProAttachment").val('');
            //    return false;
            //}


            var files = $('#InProAttachment')[0].files;
            var len = $('#InProAttachment').get(0).files.length;

            //for (var i = 0; i < len; i++) {

            //    f = files[i];

            //    var ext = f.name.split('.').pop().toLowerCase();
            //    if ($.inArray(ext, ['exe']) == 1) {
            //        $('#MandatoryFileType').attr('style', 'display:block');
            //        return false;
            //    }
            //    else { $('#MandatoryFileType').attr('style', 'display:none'); }
            //}
        }
        function OnRescheduleClick(id, status) {
            $("#RescheDuleModal").modal('show');
            $("#hdnScheduleId").val(id);

            

            if (status == "Work Cancelled" || status == "Work Completed") {
                cbtnReAssign.SetVisible(false);
            }
            else
            {
                cbtnReAssign.SetVisible(true);
            }

            cReSchedulePanel.PerformCallback('ShowData');
            checkListBox.PerformCallback('ShowEditData');

        }
        function OnInProgressClick(id, TechId, status) {
            $("#INProgessModal").modal('show');
            $("#hdnScheduleId").val(id);
            cddlTechnician.SetValue(TechId);

            if (status == "Work Cancelled" || status == "Work Completed") {
                cbtneInProgress.SetVisible(false);
            }
            else
            {
                cbtneInProgress.SetVisible(true);
            }
            cInProPanel.PerformCallback('ShowData');

        }
        function OnHoldClick(id, TechId, status) {
            $("#OnHoldModal").modal('show');
            $("#hdnScheduleId").val(id);
            cOnHoldddlTechnician.SetValue(TechId);
            if (status == "Work Cancelled" || status == "Work Completed") {
                cbtnOnHold.SetVisible(false);
            }
            else
            {
                cbtnOnHold.SetVisible(true);
            }

             cOnHoldPanel.PerformCallback('ShowData');
        }

        
        function ReleaseHoldClick(id, TechId, status) {
            $("#ReleaseHoldModal").modal('show');
            $("#hdnScheduleId").val(id);
            cReleaseHoldddlTechnician.SetValue(TechId);
            if (status == "Work Cancelled" || status == "Work Completed") {
                cbtnReleaseHold.SetVisible(false);
            }
            else {
                cbtnReleaseHold.SetVisible(true);
            }

            cReleaseHoldPanel.PerformCallback('ShowData');
        }



        function OnCancelledClick(id, TechId, status) {
            $("#CancelledModal").modal('show');
            $("#hdnScheduleId").val(id);
             
            cCanddlTechnician.SetValue(TechId);
            if (status == "Work Cancelled" || status == "Work Completed") {
                cbtnCancelled.SetVisible(false);
            }
            else
            {
                cbtnCancelled.SetVisible(true);
            }
            cCancelledPanel.PerformCallback('ShowData');

        }
        function OnCompleteClick(id, TechId, status) {
            $("#CompleteModal").modal('show');
            $("#hdnScheduleId").val(id);
            cCompddlTechnician.SetValue(TechId);
            

            if (status == "Work Cancelled" || status == "Work Completed") {
                cbtnOnCompleate.SetVisible(false);
            }
            else
            {
                cbtnOnCompleate.SetVisible(true);
            }
            cCompletePanel.PerformCallback('ShowData');
        }

        function SaveReAssign() {
            cReSchedulePanel.PerformCallback('ReAssign');
        }
        function SaveInProgress() {
            cInProPanel.PerformCallback('InProgress');
        }


        function BranchChange(s, e) {
            cddlTechnician.PerformCallback();
            checkListBox.PerformCallback();
            grid.PerformCallback('BlankGrid');
        }



        var textSeparator = ";";
        function updateText() {
            var selectedItems = checkListBox.GetSelectedItems();
            checkComboBox.SetText(getSelectedItemsText(selectedItems));

            if (selectedItems.length > 0) {
                for (var i = 0; i < selectedItems.length; i++) {
                    if (i == 0)
                        $("#hdnSubTech").val(selectedItems[i].value)
                    else
                        $("#hdnSubTech").val($("#hdnSubTech").val() + "," + selectedItems[i].value)
                }
            }

        }
        function synchronizeListBoxValues(dropDown, args) {
            checkListBox.UnselectAll();
            var texts = dropDown.GetText().split(textSeparator);
            var values = getValuesByTexts(texts);
            checkListBox.SelectValues(values);
            updateText(); // for remove non-existing texts
        }
        function getSelectedItemsText(items) {
            var texts = [];
            for (var i = 0; i < items.length; i++)
                texts.push(items[i].text);
            return texts.join(textSeparator);
        }
        function getValuesByTexts(texts) {
            var actualValues = [];
            var item;
            for (var i = 0; i < texts.length; i++) {
                item = checkListBox.FindItemByText(texts[i]);
                if (item != null)
                    actualValues.push(item.value);
            }
            return actualValues;
        }



        function AllControlInitilize() {
            if (isFirstTime) {

                if (localStorage.getItem('ScheduleDetailsFromDate')) {
                    var fromdatearray = localStorage.getItem('ScheduleDetailsFromDate').split('-');
                    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                    cFormDate.SetDate(fromdate);
                }

                if (localStorage.getItem('ScheduleDetailsToDate')) {
                    var todatearray = localStorage.getItem('ScheduleDetailsToDate').split('-');
                    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                    ctoDate.SetDate(todate);
                }
                if (localStorage.getItem('ScheduleDetailsBranch')) {
                    if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('ScheduleDetailsBranch'))) {
                        ccmbBranchfilter.SetValue(localStorage.getItem('ScheduleDetailsBranch'));
                    }

                }
                if ($("#LoadGridData").val() == "ok")
                    updateGridByDate();

                isFirstTime = false;
            }

        }
        function OnViewClick(keyValue) {
            var url = 'ServiceMaterialIssueAdd.aspx?key=' + keyValue + '&req=V';
            window.location.href = url;
        }
        function onEditClick(id) {
            window.location.href = 'ServiceMaterialIssueAdd.aspx?Key=' + id;
        }

        function OnClickDelete(id) {
            jConfirm("Confirm Delete?", "Alert", function (ret) {
                if (ret)
                { cgridAdvanceAdj.PerformCallback("Del~" + id); }
            });

        }

        function GridEndCallBack() {
            if (cgridAdvanceAdj.cpReturnMesg) {
                jAlert(cgridAdvanceAdj.cpReturnMesg, "Alert", function () { cgridAdvanceAdj.Refresh(); });
                cgridAdvanceAdj.cpReturnMesg = null;
            }
        }


        function updateGridByDate() {

            if (cFormDate.GetDate() == null) {
                jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
            }
            else if (ctoDate.GetDate() == null) {
                jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
            }
            else
                if (cddlBranch.GetValue() == null) {
                    jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
                }
                else {

                    localStorage.setItem("ScheduleDetailsFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
                    localStorage.setItem("ScheduleDetailsToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
                    localStorage.setItem("ScheduleDetailsBranch", cddlBranch.GetValue());

                    $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                    $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                    $("#hfBranchID").val(cddlBranch.GetValue());
                    $("#hfIsFilter").val("Y");
                    cgridAdvanceAdj.Refresh();
                }


        }


        function CustomerButnClick(s, e) {
            //if (ccmbBranchfilter.GetValue() == "0") {
            //    ccmbBranchfilter.Focus();

            //}
            //else {
            //    $('#CustModel').modal('show');
            //}
            $('#CustModel').modal('show');
        }
        function CustomerKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
                //if (ccmbBranchfilter.GetValue() == "0") {
                //    jAlert("Please Select Branch.", "Alert", function () {
                //        ccmbBranchfilter.Focus();
                //    });
                //}
                //else {
                //    $('#CustModel').modal('show');
                //}
                $('#CustModel').modal('show');
            }
        }
        function Customerkeydown(e) {

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtCustSearch").val();

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Customer Name");
                HeaderCaption.push("Unique Id");
                HeaderCaption.push("Address");
                if ($("#txtCustSearch").val() != '') {
                    callonServer("Services/Master.asmx/GetCustomer", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[customerindex=0]"))
                    $("input[customerindex=0]").focus();
            }

        }
        function SetCustomer(Id, Name) {
            if (Id) {
                $('#CustModel').modal('hide');
                ctxtCustName.SetText(Name);
                GetObjectID('hdnCustomerId').value = Id;

            }
        }

        function ValueSelected(e, indexName) {
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var Id = e.target.parentElement.parentElement.cells[0].innerText;
                var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                if (Id) {
                    if (indexName == "customerIndex")
                        SetCustomer(Id, name);

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
                    if (indexName == "customerIndex")
                        $('#txtCustSearch').focus();
                }
            }

        }
        function OnAddClick() {
            window.location.href = 'ServiceMaterialIssueAdd.aspx?Key=Add';
        }
        function gridRowclick(s, e) {
            $('#gridAdvanceAdj').find('tr').removeClass('rowActive');
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
                        console.log(value);
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }
    </script>
    <style>
        .padTab {
            margin-bottom: 4px;
            margin-top: 8px;
        }

            .padTab > tbody > tr > td {
                padding-right: 15px;
                vertical-align: middle;
            }

                .padTab > tbody > tr > td > label, .padTab > tbody > tr > td > input[type="button"] {
                    margin-bottom: 0 !important;
                    font-size: 14px;
                }

            .padTab > tbody > tr > td {
                font-size: 14px;
            }

        .pdtble > tbody > tr > td {
            padding: 0 5px 0 0;
        }

            .pdtble > tbody > tr > td > select {
                margin: 0;
            }

        .pdtbleCom > tbody > tr > td {
            padding: 0 5px 0 0;
        }

            .pdtbleCom > tbody > tr > td > select {
                margin: 0;
            }
            .bootstrap-timepicker-widget table td input{
                margin:0 auto !important;
            }
            .bootstrap-timepicker-meridian {
                font-size: 11px;
            }
    </style>
    <link href="/assests/pluggins/TimePicker/bootstrap-timepicker.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/TimePicker/bootstrap-timepicker.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cgridAdvanceAdj.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cgridAdvanceAdj.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cgridAdvanceAdj.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cgridAdvanceAdj.SetWidth(cntWidth);
                }

            });
          
            $('#INProgessModal').on('shown.bs.modal', function (e) {
                //alert("hi");
                setTimeout(function () {
                    $('#timepicker1').timepicker({
                        minuteStep: 5
                    });
                    //salert("hi");
                }, 700)
            });

            $('#CompleteModal').on('shown.bs.modal', function (e) {
                //alert("hi");
                setTimeout(function () {
                    $('#timepicker2').timepicker({
                        minuteStep: 1
                    });
                    $('#timepicker3').timepicker({
                        minuteStep: 1
                    });
                    //salert("hi");
                }, 700)
            });
                                        
        });
        
        
                                        
      
        
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>


    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">Schedule Details</h3>
        </div>
    </div>
    <table class="padTab">
        <tr>
            <td>
                <label>From Date</label></td>
            <td>
                <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
            </td>
            <td>
                <label>To Date</label>
            </td>
            <td>
                <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>

            </td>
            <td>Unit</td>
            <td>
                <dxe:ASPxComboBox ID="ddlBranch" runat="server" ClientInstanceName="cddlBranch" Width="100%">
                </dxe:ASPxComboBox>
            </td>
            <td>Customer</td>
            <td>
                <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName">
                    <Buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </Buttons>
                    <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){CustomerKeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>
            </td>
            <td>
                <input type="button" value="Show" class="btn btn-primary" onclick="updateGridByDate()" />
            </td>

        </tr>

    </table>

    <div class="form_main">

        <div class="GridViewArea relative">

            <dxe:ASPxGridViewExporter ID="exporter" GridViewID="gridAdvanceAdj" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
            </dxe:ASPxGridViewExporter>

            <dxe:ASPxGridView ID="gridAdvanceAdj" runat="server" KeyFieldName="DETAILS_ID" AutoGenerateColumns="False"
                Width="100%" ClientInstanceName="cgridAdvanceAdj" SettingsBehavior-AllowFocusedRow="true"
                SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto"
                SettingsBehavior-ColumnResizeMode="Control" DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" OnCustomCallback="gridAdvanceAdj_CustomCallback">

                <SettingsSearchPanel Visible="True" Delay="5000" />
                <Columns>
                    <dxe:GridViewDataTextColumn Caption="Schedule Code" FieldName="SCH_CODE" Width="200"
                        VisibleIndex="0" FixedStyle="Left">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataDateColumn Caption="SCHEDULE DATE" FieldName="SCHEDULE_DATE" Width="200"
                        VisibleIndex="0" PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataDateColumn>
                    <dxe:GridViewDataTextColumn Caption="Contract Number" FieldName="CONTRACT_NO" Width="200"
                        VisibleIndex="0" FixedStyle="Left">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Customer" FieldName="CUSTOMER" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Service Code" FieldName="sProducts_Code" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Service Name" FieldName="sProducts_Name" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Service Description" FieldName="sProducts_Description" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="SEGMENT1" FieldName="SEGMENT1" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="SEGMENT1" FieldName="SEGMENT1" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="SEGMENT2" FieldName="SEGMENT2" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="SEGMENT3" FieldName="SEGMENT3" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="SEGMENT4" FieldName="SEGMENT4" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="SEGMENT5" FieldName="SEGMENT5" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="SERVICE" FieldName="SERVICE" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="QUANTITY" FieldName="QUANTITY" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="UOM" FieldName="UOM" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="ASSIGNED BRANCH" FieldName="ASSIGNEDBRANCH" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="BRANCH ASSIGNED BY" FieldName="BRANCHASSIGNEDBY" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="BRANCH ASSIGNED ON" FieldName="BRANCH_ASSIGNED_ON" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="ACTUAL ASSIGNED BRANCH" FieldName="ACTUALASSIGNEDBRANCH" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="BRANCH UNASSIGNED BY" FieldName="BRANCHUNASSIGNEDBY" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="BRANCH UNASSIGNED ON" FieldName="BRANCH_UNASSIGNED_ON" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="ASSIGNED TECHNICIAN" FieldName="ASSIGNEDTECHNICIAN" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="TECHNICIAN ASSIGNED BY" FieldName="TECHNICIANASSIGNEDBY" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="TECHNICIAN ASSIGNED ON" FieldName="TECHNICIAN_ASSIGNED_ON" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="ACTUAL ASSIGNED TECHNICIAN" FieldName="ACTUALASSIGNED_ECHNICIAN" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="TECHNICIAN UNASSIGNED BY" FieldName="TECHNICIANUNASSIGNEDBY" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="TECHNICIAN UNASSIGNED ON" FieldName="TECHNICIAN_UNASSIGNED_ON" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="SUB TECHNICIAN" FieldName="SUB_TECHNICIAN" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="STATUS" FieldName="STATUS" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="0">
                        <DataItemTemplate>
                            <div class='floatedBtnArea'>
                                <a href="javascript:void(0);" onclick="OnRescheduleClick('<%# Container.KeyValue %>','<%#Eval("STATUS") %>')" class="" title="">
                                    <span class='ico ColorFour'><i class='fa fa-reply-all'></i></span><span class='hidden-xs'>Re-Schedule</span></a>

                                <a href="javascript:void(0);" onclick="OnInProgressClick('<%# Container.KeyValue %>','<%#Eval("ASSIGNED_TECHNICIAN") %>','<%#Eval("STATUS") %>')" class="" title="">
                                    <span class='ico ColorFive'><i class='fa fa-long-arrow-right'></i></span><span class='hidden-xs'>In Progress</span></a>

                                <a href="javascript:void(0);" onclick="OnHoldClick('<%# Container.KeyValue %>','<%#Eval("ASSIGNED_TECHNICIAN") %>','<%#Eval("STATUS") %>')" class="" title="" style='<%#Eval("HoldStyle")%>'>
                                    <span class='ico ColorThree'><i class='fa fa-map-signs'></i></span><span class='hidden-xs'>On Hold</span></a>
                                 
                                 <a href="javascript:void(0);" onclick="ReleaseHoldClick('<%# Container.KeyValue %>','<%#Eval("ASSIGNED_TECHNICIAN") %>','<%#Eval("STATUS") %>')" class="" title="" style='<%#Eval("UnHoldStyle")%>'>
                                    <span class='ico ColorTwo'><i class='fa fa-check-square'></i></span><span class='hidden-xs'>Release Hold</span></a>

                                <a href="javascript:void(0);" onclick="OnCancelledClick('<%# Container.KeyValue %>','<%#Eval("ASSIGNED_TECHNICIAN") %>','<%#Eval("STATUS") %>')" class="" title="">
                                    <span class='ico deleteColor'><i class='fa fa-times'></i></span><span class='hidden-xs'>Cancelled</span></a>

                                <a href="javascript:void(0);" onclick="OnCompleteClick('<%# Container.KeyValue %>','<%#Eval("ASSIGNED_TECHNICIAN") %>','<%#Eval("STATUS") %>')" class="" title="">
                                    <span class='ico ColorSix'><i class='fa fa-check'></i></span><span class='hidden-xs'>Complete</span></a>
                            </div>
                        </DataItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <CellStyle HorizontalAlign="Center"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <HeaderTemplate><span></span></HeaderTemplate>
                        <EditFormSettings Visible="False"></EditFormSettings>

                    </dxe:GridViewDataTextColumn>
                </Columns>
                <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                <SettingsPager PageSize="10">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                </SettingsPager>

                <Settings ShowGroupPanel="True" ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" ShowStatusBar="Hidden" ShowHorizontalScrollBar="true" ShowFilterRow="true" ShowFilterRowMenu="true" />
                <SettingsLoadingPanel Text="Please Wait..." />
                <ClientSideEvents EndCallback="GridEndCallBack" RowClick="gridRowclick" />

            </dxe:ASPxGridView>
            <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                ContextTypeName="ERPDataClassesDataContext" TableName="v_SCHEDULEDETAILS_List" />

            <div class="modal fade" id="CustModel" role="dialog">
                <div class="modal-dialog">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Customer Search</h4>
                        </div>
                        <div class="modal-body">
                            <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search By Customer Name or Unique Id" />

                            <div id="CustomerTable">
                                <table border='1' width="100%" class="dynamicPopupTbl">
                                    <tr class="HeaderStyle">
                                        <th class="hide">id</th>
                                        <th>Customer Name</th>
                                        <th>Unique Id</th>
                                        <th>Address</th>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>

                </div>
            </div>
            <asp:HiddenField ID="hfIsFilter" runat="server" />
            <asp:HiddenField ID="hfFromDate" runat="server" />
            <asp:HiddenField ID="hfToDate" runat="server" />
            <asp:HiddenField ID="hfBranchID" runat="server" />
            <asp:HiddenField ID="hiddenedit" runat="server" />
            <asp:HiddenField ID="hdnCustomerId" runat="server" />
        </div>
    </div>


    <!-- Modal -->
    <div id="RescheDuleModal" class="modal fade" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Re-Schedule</h4>
                </div>
                <div class="modal-body">
                    <dxe:ASPxCallbackPanel runat="server" ID="ReSchedulePanel" ClientInstanceName="cReSchedulePanel" OnCallback="ReSchedulePanel_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <div class="row">
                                    <div class="col-md-3">
                                        <label>Branch</label>
                                        <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                                            <ClientSideEvents SelectedIndexChanged="BranchChange" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label>Head Technician</label>
                                        <dxe:ASPxComboBox ID="ddlTechnician" OnCallback="ddlTechnician_Callback" ClientInstanceName="cddlTechnician" runat="server" Width="100%">
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label>Subordinate Technician</label>
                                        <dxe:ASPxDropDownEdit ClientInstanceName="checkComboBox" ID="ASPxDropDownEdit1" Width="100%" runat="server" AnimationType="None">
                                            <DropDownWindowStyle BackColor="#EDEDED" />
                                            <DropDownWindowTemplate>
                                                <dxe:ASPxListBox OnCallback="listBox_Callback" Width="100%" ID="listBox" ClientInstanceName="checkListBox" SelectionMode="CheckColumn"
                                                    runat="server" Height="200" EnableSelectAll="true">
                                                    <Border BorderStyle="None" />
                                                    <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />

                                                    <ClientSideEvents SelectedIndexChanged="updateText" Init="updateText" />
                                                </dxe:ASPxListBox>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="padding: 4px">
                                                            <dxe:ASPxButton ID="ASPxButton1" AutoPostBack="False" runat="server" Text="Close" Style="float: right">
                                                                <ClientSideEvents Click="function(s, e){ checkComboBox.HideDropDown(); }" />
                                                            </dxe:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </DropDownWindowTemplate>
                                            <ClientSideEvents TextChanged="synchronizeListBoxValues" DropDown="synchronizeListBoxValues" />
                                        </dxe:ASPxDropDownEdit>

                                    </div>
                                    <div class="col-md-3">
                                        <label>Date</label>
                                        <dxe:ASPxDateEdit ID="dtAssign" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdtAssign" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                        </dxe:ASPxDateEdit>
                                    </div>
                                    <div class="col-md-3">
                                        <label>Attachment</label>
                                        <div id="FileReSeheduleFile" runat="server" >
                                             <asp:FileUpload ID="fileReSchedule" runat="server" Width="100%" />
                                        </div>
                                       
                                        <div id="divReSeheduleFile" runat="server" visible="false" class="pull-left mRight10">
                                            <a onclick="OnDocumentView('<%=filedoc %>','<%=filesrc %>')" style="text-decoration: none; cursor: pointer;" title="View" class="pad btn btn-default btn-xs">View file
                                            </a>

                                            <a onclick="OnChangeFileReSehedule()" style="text-decoration: none; cursor: pointer;" title="Change File" class="pad btn btn-default btn-xs">Change File
                                            </a>
                                        </div>
                                        <asp:HiddenField ID="hddnReSeheduleFile" runat="server" />
                                    </div>
                                    <div class="col-md-3">
                                        <label>User</label>
                                        <dxe:ASPxComboBox ID="ddlReSchedule" ClientInstanceName="cddlReSchedule" runat="server" Width="100%">
                                        </dxe:ASPxComboBox>
                                    </div>

                                </div>
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </div>
                <div class="modal-footer">
                    <%--<button type="button" class="btn btn-primary" onclick="SaveReAssign()">Re-Assign</button>--%>
                    <dxe:ASPxButton ID="btnReAssign" runat="server" Text="Re-Assign" CssClass="btn btn-primary" ClientInstanceName="cbtnReAssign" AutoPostBack="false"
                        OnClick="btnReAssign_Click" UseSubmitBehavior="False">
                        <ClientSideEvents Click="SaveReAssign" />
                    </dxe:ASPxButton>
                </div>
            </div>

        </div>
    </div>
    <asp:HiddenField ID="hdnScheduleId" runat="server" />
    <asp:HiddenField ID="hdnSubTech" runat="server" />

    <div id="INProgessModal" class="modal fade" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">In Progress</h4>
                </div>
                <div class="modal-body">
                    <dxe:ASPxCallbackPanel runat="server" ID="InProPanelPanel" ClientInstanceName="cInProPanel" OnCallback="InProPanelPanel_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <div class="row">
                                    <div class="col-md-3">
                                        <label>Technician<span style="color: Red;">*</span></label>
                                        <dxe:ASPxComboBox ID="InProddlTechnician" ClientInstanceName="cddlTechnician" runat="server" Width="100%">
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label>Start Date<span style="color: Red;">*</span></label>
                                        <dxe:ASPxDateEdit ID="InProStartDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cInProStartDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                        </dxe:ASPxDateEdit>
                                    </div>
                                    <div class="col-md-2 lblmTop8">
                                        <label>Start Time <span style="color: Red;">*</span></label>
                                        <div>
                                            <div class="input-group bootstrap-timepicker timepicker">
                                                <input id="timepicker1" type="text" class="form-control input-small" />
                                                <span class="input-group-addon" style="padding: 2px 5px;"><i class="fa fa-clock-o"></i></span>
                                            </div>
                                        </div>
                                    </div>
                                    
                                    <%--<div class="col-md-4 hide">
                                        <div>Start Time<span style="color: Red;">*</span></div>
                                        <table class="pdtble">
                                            <tr>
                                                <td style="width: 100px">
                                                    <dxe:ASPxTextBox ID="txtHrs" runat="server" ClientInstanceName="ctxtHrs" RightToLeft="True" Width="100%" MaxLength="2" Text="0" onkeypress="return onlyNumbers();">
                                                    </dxe:ASPxTextBox>
                                                </td>
                                                <td style="width: 12px">:</td>
                                                <td style="width: 100px">
                                                    <dxe:ASPxTextBox ID="txtMin" runat="server" ClientInstanceName="ctxtMin" RightToLeft="True" Width="100%" MaxLength="2" Text="0" onkeypress="return onlyNumbers();">
                                                    </dxe:ASPxTextBox>
                                                </td>
                                                <td style="width: 120px">
                                                    <asp:DropDownList ID="ddlTime" runat="server" Width="100%">
                                                        <asp:ListItem Value="AM">AM</asp:ListItem>
                                                        <asp:ListItem Value="PM">PM</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>--%>

                                    
                                    <div class="col-md-3">
                                        <label>Attachment</label>
                                        <div id="Attachmentdiv" runat="server">
                                            <asp:FileUpload ID="InProAttachment" runat="server" Width="100%" />
                                        </div>
                                        <span id="MandatoryFileSize" style="display: none">
                                            <img id="imgProAttachment" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="File Size More Than 2 MB" /></span>

                                        <div id="divfile" runat="server" visible="false" class="pull-left mRight10">
                                            <a onclick="OnDocumentView('<%=filedoc %>','<%=filesrc %>')" style="text-decoration: none; cursor: pointer;" title="View" class="pad btn btn-default btn-xs">View file
                                            </a>

                                            <a onclick="OnChangeFileInProgress()" style="text-decoration: none; cursor: pointer;" title="Change File" class="pad btn btn-default btn-xs">Change File
                                            </a>
                                        </div>
                                        <asp:HiddenField ID="hidFilenameInProgress" runat="server" />
                                        <div style="clear: both"></div>
                                    </div>
                                    <div style="clear: both"></div>
                                    <div class="col-md-3">
                                        <label>User</label>
                                        <dxe:ASPxComboBox ID="InProgressUser" ClientInstanceName="cInProgressUser" runat="server" Width="100%">
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div style="clear: both"></div>
                                    <div class="col-md-12 lblmTop8">
                                        <label>Remarks<span style="color: Red;">*</span></label>
                                        <dxe:ASPxMemo ID="txtRemarks" ClientInstanceName="ctxtRemarks" runat="server" Height="50px" Width="100%" Font-Names="Arial"></dxe:ASPxMemo>
                                    </div>
                                </div>
                            </dxe:PanelContent>
                            
                        </PanelCollection>
                           <ClientSideEvents EndCallback="InProgressPanelEndCall" />
                    </dxe:ASPxCallbackPanel>
                </div>
                <div class="modal-footer">
                    <dxe:ASPxButton ID="BtnSaveInProgress" runat="server" Text="Save" CssClass="btn btn-primary" ClientInstanceName="cbtneInProgress" AutoPostBack="false"
                        OnClick="BtnSaveInProgress_Click" UseSubmitBehavior="False">
                        <ClientSideEvents Click="fn_btnValidateInProgress" />
                    </dxe:ASPxButton>
                </div>
            </div>

        </div>
    </div>

    <div id="OnHoldModal" class="modal fade" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">On Hold</h4>
                </div>
                <div class="modal-body">
                    <dxe:ASPxCallbackPanel runat="server" ID="OnHoldPanel" ClientInstanceName="cOnHoldPanel" OnCallback="OnHoldPanel_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <div class="row">
                                    <div class="col-md-3">
                                        <div>Technician<span style="color: Red;">*</span></div>
                                        <dxe:ASPxComboBox ID="OnHoldddlTechnician" ClientInstanceName="cOnHoldddlTechnician" runat="server" Width="100%">
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div class="col-md-3">
                                        <div>Schedule Date</div>
                                        <dxe:ASPxDateEdit ID="OnHoldScheduleDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cOnHoldScheduleDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                        </dxe:ASPxDateEdit>
                                    </div>
                                    <div class="col-md-3">
                                        <div>Hold Upto<span style="color: Red;">*</span></div>
                                        <dxe:ASPxDateEdit ID="OnHoldHoldUpto" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cOnHoldHoldUpto" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                        </dxe:ASPxDateEdit>
                                    </div>
                                    <div style="clear: both"></div>
                                    <div class="col-md-4 lblmTop8">
                                        <div>Attachment</div>
                                        <div id="DivfileOnHold" runat="server">
                                            <asp:FileUpload ID="fileOnHold" runat="server" Width="250px" onchange="GetFileSize()" />
                                        </div>

                                        <div id="divOnHoldAttachView" runat="server" visible="false" class="pull-left mRight10">
                                            <a onclick="OnDocumentView('<%=filedoc %>','<%=filesrc %>')" style="text-decoration: none; cursor: pointer;" title="View" class="pad btn btn-default btn-xs">View file
                                            </a>
                                            <a onclick="ChangeFileOnHoldAttach()" style="text-decoration: none; cursor: pointer;" title="Change File" class="pad btn btn-default btn-xs">Change File
                                            </a>
                                        </div>
                                        <asp:HiddenField ID="hidFilenameOnHold" runat="server" />
                                    </div>
                                    <div class="col-md-3 lblmTop8">
                                        <div>User</div>
                                        <dxe:ASPxComboBox ID="OnHoldUser" ClientInstanceName="cOnHoldUser" runat="server" Width="100%">
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div style="clear: both"></div>

                                    <div class="col-md-12 lblmTop8">
                                        <div>Remarks<span style="color: Red;">*</span></div>
                                        <dxe:ASPxMemo ID="txtHoldRemarks" ClientInstanceName="ctxtHoldRemarks" runat="server" Height="50px" Width="100%" Font-Names="Arial"></dxe:ASPxMemo>

                                    </div>
                                </div>
                            </dxe:PanelContent>
                        </PanelCollection>
                      <%--  <ClientSideEvents EndCallback="OnHoldPanelEndCall" />--%>
                    </dxe:ASPxCallbackPanel>
                </div>
                <div class="modal-footer">                    
                    <dxe:ASPxButton ID="btnOnHold" runat="server" Text="Save" CssClass="btn btn-primary" ClientInstanceName="cbtnOnHold" AutoPostBack="false"
                        UseSubmitBehavior="False" OnClick="btnOnHold_Click">
                        <ClientSideEvents Click="SaveOnHold" />
                    </dxe:ASPxButton>
                </div>
            </div>

        </div>
    </div>


     <div id="ReleaseHoldModal" class="modal fade" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Release Hold</h4>
                </div>
                <div class="modal-body">
                    <dxe:ASPxCallbackPanel runat="server" ID="ReleaseHoldPanel" ClientInstanceName="cReleaseHoldPanel" OnCallback="ReleaseHoldPanel_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <div class="row">
                                    <div class="col-md-3">
                                        <div>Technician<span style="color: Red;">*</span></div>
                                        <dxe:ASPxComboBox ID="ReleaseHoldddlTechnician" ClientInstanceName="cReleaseHoldddlTechnician" runat="server" Width="100%">
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div class="col-md-3">
                                        <div>Schedule Date</div>
                                        <dxe:ASPxDateEdit ID="ReleaseHoldScheduleDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cReleaseHoldScheduleDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                        </dxe:ASPxDateEdit>
                                    </div>
                                    <div class="col-md-3">
                                        <div>Hold Upto<span style="color: Red;">*</span></div>
                                        <dxe:ASPxDateEdit ID="ReleaseHoldHoldUpto" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cReleaseHoldHoldUpto" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                        </dxe:ASPxDateEdit>
                                    </div>
                                    <div style="clear: both"></div>
                                    <div class="col-md-4 lblmTop8">
                                        <div>Attachment</div>
                                        <div id="DivFileUnHold" runat="server">
                                            <asp:FileUpload ID="FileReleaseHold" runat="server" Width="250px" onchange="GetFileSize()" />
                                        </div>

                                        <div id="DivViewUnhold" runat="server" visible="false" class="pull-left mRight10">
                                            <a onclick="OnDocumentView('<%=filedoc %>','<%=filesrc %>')" style="text-decoration: none; cursor: pointer;" title="View" class="pad btn btn-default btn-xs">View file
                                            </a>
                                            <a onclick="ChangeFileReleaseHoldAttach()" style="text-decoration: none; cursor: pointer;" title="Change File" class="pad btn btn-default btn-xs">Change File
                                            </a>
                                        </div>
                                        <asp:HiddenField ID="hddnReleaseHoldFile" runat="server" />
                                    </div>
                                    <div class="col-md-3 lblmTop8">
                                        <div>User</div>
                                        <dxe:ASPxComboBox ID="ReleaseHoldUser" ClientInstanceName="cReleaseHoldUser" runat="server" Width="100%">
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div style="clear: both"></div>

                                    <div class="col-md-12 lblmTop8">
                                        <div>Remarks<span style="color: Red;">*</span></div>
                                        <dxe:ASPxMemo ID="txtReleaseHoldRemarks" ClientInstanceName="ctxtReleaseHoldRemarks" runat="server" Height="50px" Width="100%" Font-Names="Arial"></dxe:ASPxMemo>

                                    </div>
                                </div>
                            </dxe:PanelContent>
                        </PanelCollection>
                      <%--  <ClientSideEvents EndCallback="OnHoldPanelEndCall" />--%>
                    </dxe:ASPxCallbackPanel>
                </div>
                <div class="modal-footer">                    
                    <dxe:ASPxButton ID="btnReleaseHold" runat="server" Text="Save" CssClass="btn btn-primary" ClientInstanceName="cbtnReleaseHold" AutoPostBack="false"
                        UseSubmitBehavior="False" OnClick="btnReleaseHold_Click">
                        <ClientSideEvents Click="SaveUnHold" />
                    </dxe:ASPxButton>
                </div>
            </div>

        </div>
    </div>

    <div id="CancelledModal" class="modal fade" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Cancelled</h4>
                </div>
                <div class="modal-body">
                    <dxe:ASPxCallbackPanel runat="server" ID="CancelledPanel" ClientInstanceName="cCancelledPanel" OnCallback="CancelledPanel_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <div class="row">
                                    <div class="col-md-3">
                                        <div>Technician<span style="color: Red;">*</span></div>
                                        <dxe:ASPxComboBox ID="CanddlTechnician" ClientInstanceName="cCanddlTechnician" runat="server" Width="100%">
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div class="col-md-3">
                                        <div>Schedule Date<span style="color: Red;">*</span></div>
                                        <dxe:ASPxDateEdit ID="CanScheduleDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cCanScheduleDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                        </dxe:ASPxDateEdit>
                                    </div>
                                    <div class="col-md-3">
                                        <div>Re-Schedule By</div>
                                        <asp:RadioButtonList ID="rdl_SaleInvoice" runat="server" RepeatDirection="Vertical">
                                            <asp:ListItem Text="US" Value="US"></asp:ListItem>
                                            <asp:ListItem Text="Customer" Value="Customer"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div style="clear: both"></div>
                                    <div class="col-md-4">
                                        <div>Attachment</div>
                                        <div id="DivfileCancelled" runat="server">
                                            <asp:FileUpload ID="fileCancelled" runat="server" Width="250px" onchange="GetFileSize()" />
                                        </div>
                                        <div id="divCanAttachView" runat="server" visible="false" class="pull-left mRight10">
                                            <a onclick="OnDocumentView('<%=filedoc %>','<%=filesrc %>')" style="text-decoration: none; cursor: pointer;" title="View" class="pad btn btn-default btn-xs">View file
                                            </a>
                                            <a onclick="OnChangeFileCanAttach()" style="text-decoration: none; cursor: pointer;" title="Change File" class="pad btn btn-default btn-xs">Change File
                                            </a>
                                        </div>
                                        <asp:HiddenField ID="hddnCanAttach" runat="server" />
                                    </div>
                                    <div class="col-md-3">
                                        <div>User</div>
                                        <dxe:ASPxComboBox ID="CancelledUser" ClientInstanceName="cOnHoldUser" runat="server" Width="100%">
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div style="clear: both"></div>
                                    <div class="col-md-12 lblmTop8">
                                        <div>Reason<span style="color: Red;">*</span></div>
                                        <dxe:ASPxMemo ID="txtReason" ClientInstanceName="ctxtReason" runat="server" Height="50px" Width="100%" Font-Names="Arial"></dxe:ASPxMemo>

                                    </div>
                                </div>
                            </dxe:PanelContent>
                        </PanelCollection>
                       <%-- <ClientSideEvents EndCallback="CancelledPanelEndCall" />--%>
                    </dxe:ASPxCallbackPanel>
                </div>
                <div class="modal-footer">
                    <dxe:ASPxButton ID="btnCancelled" runat="server" Text="Save" CssClass="btn btn-primary" ClientInstanceName="cbtnCancelled" AutoPostBack="false"
                        UseSubmitBehavior="False" OnClick="btnCancelled_Click">
                        <ClientSideEvents Click="SaveCancelled" />
                    </dxe:ASPxButton>
                </div>
            </div>

        </div>
    </div>

    <div id="CompleteModal" class="modal fade" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Complete</h4>
                </div>
                <div class="modal-body">
                    <dxe:ASPxCallbackPanel runat="server" ID="CompletePanel" ClientInstanceName="cCompletePanel" OnCallback="CompletePanel_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <div class="row">
                                    <div class="col-md-3">
                                        <div>Technician<span style="color: Red;">*</span></div>
                                        <dxe:ASPxComboBox ID="CompddlTechnician" ClientInstanceName="cCompddlTechnician" runat="server" Width="100%">
                                        </dxe:ASPxComboBox>
                                        <span id="MandatoryCompddlTechnician" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                    </div>
                                    <div class="col-md-3">
                                        <div>Start Date</div>
                                        <dxe:ASPxDateEdit ID="CompStartDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cCompStartDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                        </dxe:ASPxDateEdit>
                                    </div>
                                    <div class="col-md-2">
                                        <div>Start Time</div>
                                        <div>
                                            <div class="input-group bootstrap-timepicker timepicker">
                                                <input id="timepicker2" type="text" class="form-control input-small" />
                                                <span class="input-group-addon" style="padding: 2px 5px;"><i class="fa fa-clock-o"></i></span>
                                            </div>
                                        </div>
                                    </div>

                                    <div style="clear: both"></div>
                                    <div class="col-md-4">
                                        <div>Attachment</div>
                                        <div id="CompAttachmentdiv" runat="server">
                                            <asp:FileUpload ID="CompAttachment" runat="server" Width="250px" onchange="GetFileSize()" />
                                        </div>
                                        <span id="MandatoryCompAttachment" style="display: none">
                                            <img id="imgCompAttachment" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="File Size More Than 2 MB" /></span>

                                        <div id="divCompAttachView" runat="server" visible="false" class="pull-left mRight10">
                                            <a onclick="OnDocumentView('<%=filedoc %>','<%=filesrc %>')" style="text-decoration: none; cursor: pointer;" title="View" class="pad btn btn-default btn-xs">View file
                                            </a>
                                            <a onclick="OnChangeFileCompAttach()" style="text-decoration: none; cursor: pointer;" title="Change File" class="pad btn btn-default btn-xs">Change File
                                            </a>
                                        </div>
                                        <asp:HiddenField ID="hidFilenameCompAttach" runat="server" />
                                        <div style="clear: both"></div>

                                    </div>

                                    <div class="col-md-3">
                                        <div>End Date<span style="color: Red;">*</span></div>
                                        <dxe:ASPxDateEdit ID="CompEndDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cCompEndDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                        </dxe:ASPxDateEdit>
                                        <span id="MandatoryCompEndDate" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                    </div>
                                    <div class="col-md-2">
                                        <div>End Time<span style="color: Red;">*</span></div>
                                       <div>
                                            <div class="input-group bootstrap-timepicker timepicker">
                                                <input id="timepicker3" type="text" class="form-control input-small" />
                                                <span class="input-group-addon" style="padding: 2px 5px;"><i class="fa fa-clock-o"></i></span>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="col-md-3">
                                        <div>User</div>
                                        <dxe:ASPxComboBox ID="CompleteUser" ClientInstanceName="cCompleteUser" runat="server" Width="100%">
                                        </dxe:ASPxComboBox>
                                    </div>

                                    <div style="clear: both"></div>
                                    <div class="col-md-12">
                                        <div>Remarks<span style="color: Red;">*</span></div>
                                        <dxe:ASPxMemo ID="txtCompleteRemarks" ClientInstanceName="ctxtCompleteRemarks" runat="server" Height="50px" Width="100%" Font-Names="Arial"></dxe:ASPxMemo>

                                    </div>
                                </div>
                            </dxe:PanelContent>
                        </PanelCollection>
                        <ClientSideEvents EndCallback="CompletePanelEndCall" />
                    </dxe:ASPxCallbackPanel>
                </div>
                <div class="modal-footer">

                    <dxe:ASPxButton ID="btnOnCompleate" runat="server" Text="Save" CssClass="btn btn-primary" ClientInstanceName="cbtnOnCompleate" AutoPostBack="false"
                        OnClick="btnOnCompleate_Click1" UseSubmitBehavior="False">
                        <ClientSideEvents Click="fn_btnValidate" />
                    </dxe:ASPxButton>
                </div>
            </div>

        </div>
    </div>
    <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ContentUrl="ReSchedule.aspx"
        CloseAction="CloseButton" Top="120" Left="300" ClientInstanceName="popup" Height="400px"
        Width="850px" HeaderText="Attachment" AllowResize="true" ResizingMode="Postponed" Modal="true">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
    </dxe:ASPxPopupControl>

     <asp:HiddenField ID="hdnInprogressTime" runat="server" />
     <asp:HiddenField ID="hdnComStartTime" runat="server" />
     <asp:HiddenField ID="hdnComEndTime" runat="server" />
</asp:Content>
