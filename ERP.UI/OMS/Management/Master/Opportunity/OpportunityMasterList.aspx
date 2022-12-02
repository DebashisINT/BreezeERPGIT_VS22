<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="OpportunityMasterList.aspx.cs" Inherits="ERP.OMS.Management.Master.Opportunity.OpportunityMasterList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #EmployeeGrid_DXPagerBottom {
            min-width: 100% !important;
        }

        #EmployeeGrid {
            width: 100 % !important;
        }
        .myAssignTarget {
        margin-bottom: 0;
    }
         #cmbPriority {
            border-radius:3px;
        }
        .myAssignTarget > li {
            list-style-type: none;
            display: inline-block;
            font-size: 11px;
            text-align: center;
        }

            .myAssignTarget > li:not(:last-child) {
                margin-right: 15px;
            }

            .myAssignTarget > li.mainCircle {
                border: 1px solid #a2d3d8;
                border-radius: 8px;
                overflow: hidden;
            }

            .myAssignTarget > li .heading {
                padding: 2px 12px;
                background: #6d82c5;
                color: #fff;
            }

            .myAssignTarget > li .Num {
                font-size: 14px;
            }

            .myAssignTarget > li.mainHeadCenter {
                font-size: 12px;
                transform: translateY(-16px);
            }

            #myAssignTargetpopup {
                padding: 0;
            }

            #myAssignTargetpopup > li .heading {
                padding: 6px 12px;
                background: #7f96dc;
                font-weight: 600;
                color: #fff;
            }

            #myAssignTargetpopup li .Num {
                font-size: 14px;
                padding: 5px;
            }
            .modal-footer .btn {
                margin-top:0;
                margin-bottom:0;
            }
            .mleft15 {
                margin-left:15px;
            }

            #SalesActivityPopup_PW-1, #popupShowHistory_PW-1 {
                
                border-radius: 15px;
            }
            #SalesActivityPopup_PW-1 .dxpc-header, #popupShowHistory_PW-1 .dxpc-header {
                background: #3ca1e8 ;
                background-image: none !important;
                padding: 11px 20px;
                border: none;
                    border-radius: 15px 15px 0 0;
            }
            #SalesActivityPopup_PW-1 .dxpc-contentWrapper, #popupShowHistory_PW-1 .dxpc-contentWrapper {
                    background: #fff;
                    border-radius:0 0 15px 15px;
            }
             #SalesActivityPopup_PW-1 .dxpc-mainDiv, #popupShowHistory_PW-1 .dxpc-mainDiv {
                 background-color:transparent !important;
             }
             #SalesActivityPopup_PW-1 .modal-footer, #popupShowHistory_PW-1 .modal-footer {
                 text-align:left;
             }
             #SalesActivityPopup_PW-1 .dxpc-shadow, #popupShowHistory_PW-1 .dxpc-shadow {
                 box-shadow:none;
             }
    </style>
    <link href="../../../assests/css/custom/PMSStyles.css" rel="stylesheet" />
    <script type="text/javascript">

        $(document).ready(function () {
            $("#cmbPriority").change(function () {

                var value = $(this).val();
                if (value == '1') {
                    $("#cmbPriority").css({ 'background': '#66c19b', 'border-color': '#56a886', 'color': '#fff' }); // High
                } else if (value == '2') {
                    $("#cmbPriority").css({ 'background': '#35d667', 'border-color': '#2cc35b', 'color': '#555' });//Low
                }
                else if (value == '3') {
                    $("#cmbPriority").css({ 'background': '#f94747', 'border-color': '#f23c3c', 'color': '#fff' });  //Moderate
                }
                else if (value == '4') {
                    $("#cmbPriority").css({ 'background': '#f5dfc3', 'border-color': '#b8b8b8', 'color': '#555' });//Normal
                }

            });
        });

        function ddlPriorityChange() {
            var value = $("#cmbPriority").val();
            if (value == '1') {
                $("#cmbPriority").css({ 'background': '#66c19b', 'border-color': '#56a886', 'color': '#fff' }); // High
            } else if (value == '2') {
                $("#cmbPriority").css({ 'background': '#35d667', 'border-color': '#2cc35b', 'color': '#555' });//Low
            }
            else if (value == '3') {
                $("#cmbPriority").css({ 'background': '#f5dfc3', 'border-color': '#b8b8b8', 'color': '#555' });  //Moderate
            }
            else if (value == '4') {
                $("#cmbPriority").css({ 'background': '#f94747', 'border-color': '#f23c3c', 'color': '#fff' }); //Normal
            }
        }



        $(document).ready(function () {

            $(".water").each(function () {
                if ($(this).val() == this.title) {
                    $(this).addClass("opaque");
                }
            });

            $(".water").focus(function () {
                if ($(this).val() == this.title) {
                    $(this).val("");
                    $(this).removeClass("opaque");
                }
            });

            $(".water").blur(function () {
                if ($.trim($(this).val()) == "") {
                    $(this).val(this.title);
                    $(this).addClass("opaque");
                }
                else {
                    $(this).removeClass("opaque");
                }
            });
        });
        var olds = "";
        function ddlICRChange() {
            var s = $("#<%=cmbActivity.ClientID%> option:selected").val();
            if (olds != s) {
                cCallbackPanelLeadActivity.PerformCallback('Activity_Type~' + s);
                olds = s;
            }
        }

    </script>

    <script language="javascript" type="text/javascript">

        //AddedBy Chinmoy Date: 06-06-2018
        //Start

        function ClickOnView(keyValue) {
            cTransportView.SetWidth(window.screen.width - 50);
            cTransportView.SetHeight(window.innerHeight - 70);
            var url = '/OMS/management/Master/View/ViewTransport.html?id=' + keyValue;
            cTransportView.SetContentUrl(url);
            cTransportView.RefreshContentUrl();
            cTransportView.Show();
        }

        //End

        function ShowMissingData(obj, obj2) {
            var url = 'frmContactMissingData.aspx?id=' + obj;
            window.location.href = url;

        }
        function ShowError(obj) {

            if (grid.cpDelete != null) {
                if (grid.cpDelete == 'Success') {
                    jAlert('Deleted Successfully');
                    grid.cpDelete = null;
                }
                else {
                    jAlert('Used in other module.Can not delete');
                    grid.cpDelete = null;
                }

            }

            // height()
        }
        function NewPgae(cnt_id) {
            //alert('cnt_id');
        }
        function ClickOnMoreInfo(keyValue, IsConvert) {
            if (IsConvert == 1) {
                jAlert('Qualified Lead Can Not Be Edited');
            }
            else {
                //var url = 'Contact_general.aspx?contact_type=' + '<%=Session["Contactrequesttype"]%>' + '&id=' + keyValue;
                var url = 'Contact_general.aspx?id=' + keyValue;
                window.location.href = url;
            }

        }

        function OnDelete(keyValue, IsConvert) {
            if (IsConvert == 1) {
                jAlert('Qualified Lead Can Not Be Edited');
            }
            else {
                jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        grid.PerformCallback('Delete~' + keyValue);
                    }
                });
            }
        }

        //-------------Subhra 16-05-2019--------------------
        $(document).ready(function () {
            $("#cmbPriority").change(function () {

                var value = $(this).val();
                if (value == '0') {
                    $(this).css({ 'background': '#35d667', 'border-color': '#2cc35b', 'color': '#555' }); // High
                } else if (value == '1') {
                    $(this).css({ 'background': '#f5dfc3', 'border-color': '#b8b8b8', 'color': '#555' }); //Low
                }
                else if (value == '2') {
                    $(this).css({ 'background': '#f94747', 'border-color': '#f23c3c', 'color': '#fff' });  //Moderate
                }
                else if (value == '3') {
                    $(this).css({ 'background': '#66c19b', 'border-color': '#56a886', 'color': '#fff' }); //Normal
                }

            });
        });


        $(window).load(function () {

            if ($('#cmbPriority').val() == '0') {
                $('#cmbPriority').css({ 'background': '#35d667', 'border-color': '#2cc35b', 'color': '#555' }); // High
            } else if ($('#cmbPriority').val() == '1') {
                $('#cmbPriority').css({ 'background': '#f5dfc3', 'border-color': '#b8b8b8', 'color': '#555' });  //Low
            }
            else if ($('#cmbPriority').val() == '2') {
                $('#cmbPriority').css({ 'background': '#f94747', 'border-color': '#f23c3c', 'color': '#fff' }); //Moderate
            }
            else if ($('#cmbPriority').val() == '3') {
                $('#cmbPriority').css({ 'background': '#66c19b', 'border-color': '#56a886', 'color': '#fff' }); //Normal
            }
        });

        var keyvalueforassign = '';
        function OnAssignTo(keyValue, Name, EnterBy, Convert_To, Assign_Id, enteredby_id) {
            if (document.getElementById('hdnUserRestrictionForLeadAction').value == 1) {
                if ($('#hdnUserId').val() == Assign_Id || $('#hdnUserId').val() == enteredby_id) {
                    var OtherDetails = {}
                    OtherDetails.Key = keyValue;

                    $('#<%=hdnQualify.ClientID %>').val(Convert_To);
                        keyvalueforassign = keyValue;
                        cCallbackPanelAssign.PerformCallback('Load~' + keyvalueforassign + '~' + Name);
                        $('#AssignToModel').modal('show');
                        if (Convert_To == 1) {
                            $('#AssignToModel .modal-footer .btn').hide();
                        }
                        else {
                            $("#cmbAssignTo").focus();
                        }
                    }
                    else {
                        jAlert('You do not have authority to take this action.');
                    }
                }
                else {
                    var OtherDetails = {}
                    OtherDetails.Key = keyValue;

                    $('#<%=hdnQualify.ClientID %>').val(Convert_To);
                keyvalueforassign = keyValue;
                cCallbackPanelAssign.PerformCallback('Load~' + keyvalueforassign + '~' + Name);
                $('#AssignToModel').modal('show');
                if (Convert_To == 1) {
                    $('#AssignToModel .modal-footer .btn').hide();
                }
                else {
                    $("#cmbAssignTo").focus();
                }
            }

        }
        function Assign() {
            if ($("#txtRemarks").val() != "") {
                cCallbackPanelAssign.PerformCallback('Assign~' + keyvalueforassign);
                $('#AssignToModel').modal('hide');
            }
            else {
                jAlert('Remarks is mandatory.');
            }

        }

        function UnAssign(keyValue) {
            if ($("#txtRemarks").val() != "") {
                jConfirm('Confirm?', 'Confirmation Dialog', function (r) {
                    if (r == true) {

                        cCallbackPanelAssign.PerformCallback('Unassign~' + keyvalueforassign);
                        $('#AssignToModel').modal('hide');
                    }
                });
            }
            else {
                jAlert('Remarks is mandatory.', 'title', function () {
                    $("#txtRemarks").focus();
                });
            }
        }
        function CallbackPanelEndCall(s, e) {
            if (cCallbackPanelAssign.cpAssignSave == "Save") {
                grid.Refresh();
            }

            if (cCallbackPanelAssign.cpEnteredBy != 0) {
                $("#lblenteredby").text(cCallbackPanelAssign.cpName);
                $("#txtRemarks").val(cCallbackPanelAssign.cpRemarks);
                $("#cmbAssignTo").val(cCallbackPanelAssign.cpEnteredBy);
            }
            else {
                $("#lblenteredby").text(cCallbackPanelAssign.cpName);
                $("#cmbAssignTo").val(cCallbackPanelAssign.cpAssignTo);
                $("#txtRemarks").val(cCallbackPanelAssign.cpRemarks);
            }
            if ($("#txtRemarks").val() == '') {
                $('#btnUnAssign').hide();
            }
            else {
                $('#btnUnAssign').show();
            }


        }

        function OnConvertTo(keyValue, Name, Convert_To, Assign_Id, enteredby_id) {
            if (document.getElementById('hdnUserRestrictionForLeadAction').value == 1) {
                if ($('#hdnUserId').val() == Assign_Id || $('#hdnUserId').val() == enteredby_id) {
                    if (Convert_To == 'Qualified') {
                        $('#ConvertToModel .modal-footer .btn').hide();
                    }
                    else {
                        $('#ConvertToModel .modal-footer .btn').show()
                        $('#<%=hdnQualify.ClientID %>').val(Convert_To);
                            keyvalueforassign = keyValue;
                            cCallbackPanelConvertto.PerformCallback('Load~' + keyvalueforassign + '~' + Name);
                            $('#ConvertToModel').modal('show');
                        }
                    }
                    else {
                        jAlert('You do not have authority to take this action.');
                    }
                }
                else {
                    if (Convert_To == 'Qualified') {
                        $('#ConvertToModel .modal-footer .btn').hide();
                    }
                    else {
                        $('#ConvertToModel .modal-footer .btn').show()
                        $('#<%=hdnQualify.ClientID %>').val(Convert_To);
                    keyvalueforassign = keyValue;
                    cCallbackPanelConvertto.PerformCallback('Load~' + keyvalueforassign + '~' + Name);
                    $('#ConvertToModel').modal('show');
                }
            }


        }
        function SaveContact() {
            if ($("#cmbLeadstatus").val() != 0) {
                if ($("#txtConvertToRemarks").val() != "") {
                    cCallbackPanelConvertto.PerformCallback('Save~' + keyvalueforassign);
                    $('#ConvertToModel').modal('hide');
                }
                else {
                    jAlert('Remarks is mandatory.', 'title', function () {
                        $("#txtConvertToRemarks").focus();
                    });
                }
            }
            else {
                jAlert('Please Select Convert To');
            }
        }
        function CancelContact() {
            $("#txtConvertToRemarks").val('');
            $("#cmbLeadstatus").val(0);
            $('#ConvertToModel').modal('hide');
        }
        function CallbackPanelConverttoEndCall(s, e) {
            if (cCallbackPanelConvertto.cpConverttoSave == "Save") {
                grid.Refresh();
            }
            $("#lblName").text(cCallbackPanelConvertto.cpName);
            $("#cmbLeadstatus").val(cCallbackPanelConvertto.cpLeadStatus);
            $("#txtConvertToRemarks").val(cCallbackPanelConvertto.cpConverttoRemarks);
            if ($("#txtConvertToRemarks").val() == '') {
                $("#cmbLeadstatus").val(0);
            }
        }

        function OnSalesActivity(keyValue, Name, Convert_To, Assign_Id, enteredby_id) {

            $("#cmbActivity").val(0);
            $("#txtSubject").val('');
            $("#cmbType").val(0);
            $("#txtDetails").val('');
            $("#cmbSalesActivityAssignTo").val(0);
            $("#cmbDuration").val(0);
            $("#cmbPriority").val(0);


            keyvalueforassign = keyValue;
            $("#hdnEntityID").val(keyValue);
            if (document.getElementById('hdnUserRestrictionForLeadAction').value == 1) {
                if ($('#hdnUserId').val() == Assign_Id || $('#hdnUserId').val() == enteredby_id) {
                    cCallbackPanelLeadActivity.PerformCallback('Load~' + keyvalueforassign + '~' + Name);
                    cSalesActivityPopup.Show();
                }
                else {
                    jAlert('You do not have authority to take this action.');
                }
            }
            else {
                cCallbackPanelLeadActivity.PerformCallback('Load~' + keyvalueforassign + '~' + Name);
                cSalesActivityPopup.Show();
            }
        }



        $(document).ready(function () {
            <%--$('#<%=cmbActivity.ClientID%>').on('change', function () {
                var s = $("#<%=cmbActivity.ClientID%> option:selected").val();
                cCallbackPanelLeadActivity.PerformCallback('Activity_Type~' + s);
                //$("#Label1").text(s);
            });--%>
        })

        function validations() {
            var ismandatory = false;

            if (cdtActivityDate.date != null) {
                ismandatory = true;
            }
            else {
                ismandatory = false;
                jAlert('Select Activity Date');
                return;
            }

            if ($('#cmbActivity').val() != "0") {
                ismandatory = true;
            }
            else {
                ismandatory = false;
                jAlert('Select Activity');
                return;
            }

            if ($('#cmbType').val() != "0") {
                ismandatory = true;
            }
            else {
                ismandatory = false;
                jAlert('Select Type');
                return;
            }

            if ($('#txtSubject').val() != "") {
                ismandatory = true;
            }
            else {
                ismandatory = false;
                jAlert('Select Subject');
                return;
            }

            if ($('#txtDetails').val() != "") {
                ismandatory = true;
            }
            else {
                ismandatory = false;
                jAlert('Select Details');
                return;
            }

            if ($('#cmbSalesActivityAssignTo').val() != "0") {
                ismandatory = true;
            }
            else {
                ismandatory = false;
                jAlert('Select Assign To');
                return;
            }

            if ($('#cmbDuration').val() != "0") {
                ismandatory = true;
            }
            else {
                ismandatory = false;
                jAlert('Select Duration');
                return;
            }

            if ($('#cmbPriority').val() != "0") {
                ismandatory = true;
            }
            else {
                ismandatory = false;
                jAlert('Select Priority');
                return;
            }

            if (cDtxtDue.date != null) {
                ismandatory = true;
            }
            else {
                ismandatory = false;
                jAlert('Select Due Date');
                return;
            }

            return ismandatory;
        }
        function SaveSalesActivity() {
            if (validations()) {

                cCallbackPanelLeadActivity.PerformCallback('Save~' + keyvalueforassign + '~' + $("#cmbActivity").val() + '~' + $("#cmbType").val() + '~' + $("#txtSubject").val() + '~' + $("#txtDetails").val() + '~' + $("#cmbSalesActivityAssignTo").val() + '~' + $("#cmbDuration").val() + '~' + $("#cmbPriority").val());
            }

        }
        function CancelSalesActivity() {
            $("#cmbActivity").val(0);
            $("#txtSubject").text('');
            $("#cmbType").val(0);
            $("#txtDetails").text('');
            $("#cmbSalesActivityAssignTo").val(0);
            $("#cmbDuration").val(0);
            $("#cmbPriority").val(0);
            cSalesActivityPopup.Hide();
        }

        function CallbackPanelLeadActivityEndCall(s, e) {
            // $('#LeadActivityModel').modal('show');
            if (cCallbackPanelLeadActivity.cpStatusLeadActivity == "Save") {
                cSalesActivityPopup.Hide();
                grid.Refresh();
            }
            else if (cCallbackPanelLeadActivity.cpStatusLeadActivity == "Load") {
                $("#cmbSalesActivityAssignTo").val(cCallbackPanelLeadActivity.cpAssignto);
                $("#lblshowLeadName").text(cCallbackPanelLeadActivity.cpLeadName);
                $("#lblshowDueDate").text(cCallbackPanelLeadActivity.cpDueDate);
                $("#lblshowPriority").text(cCallbackPanelLeadActivity.cpPriority);
            }

        }

        function ShowHistory() {
            cCallbackPanelShowHistory.cpStatusLeadActivity = '';
            var lead_entityid = $("#hdnEntityID").val();
            cCallbackPanelShowHistory.PerformCallback('top10ShowHistory~' + lead_entityid);
            cpopupShowHistory.Show();
        }
        function btn_ShowHistory() {
            var lead_entityid = $("#hdnEntityID").val();
            cCallbackPanelShowHistory.PerformCallback('AllShowHistory~' + lead_entityid);
        }
        function btn_ShowTopTenHistory() {
            var lead_entityid = $("#hdnEntityID").val();
            cCallbackPanelShowHistory.PerformCallback('top10ShowHistory~' + lead_entityid);
        }
        function OnClickLeadActivityDelete(id) {
            var lead_entityid = $("#hdnEntityID").val();
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    cCallbackPanelShowHistory.PerformCallback('Delete~' + lead_entityid + '~' + id);
                }
            });
        }
        function CallbackPanelShowHistoryEndCall(s, e) {
            if (cCallbackPanelShowHistory.cpStatusLeadActivity == "Delete") {
                jAlert('Deleted Successfully');
                cshowhistorygrid.Refresh();
            }
        }
        //--------------------------------------------------

        function OnBudgetopen(Cusid, IsConvert) {
            //    if (document.getElementById('IsUdfpresent').value == '0') {
            //        jAlert("UDF not define.");
            //    }
            //    else {
            //        // var url = '../master/frm_BranchUdfPopUp.aspx?Type=SQO';

            //        var keyVal = document.getElementById('Keyval_internalId').value;
            if (IsConvert == 1) {
                jAlert('Qualified Lead Can Not Be Edited');
            }
            else {
                var url = '/OMS/Management/Master/BudgetCustomerAdd.aspx?Cusid=' + Cusid;
                popupbudget.SetContentUrl(url);
                popupbudget.Show();

                //}
                return true;
            }
        }

        function BudgetAfterHide(s, e) {
            popupbudget.Hide();
        }


        function ParentCustomerOnClose(newCustId, CustomerName, CustUniqueName, BillingStateText, BillingStateCode, ShippingStateText, ShippingStateCode) {
            AspxDirectAddCustPopup.Hide();
            var url = 'frmContactMain.aspx?requesttype=customer';
            window.location.href = url;

            //if (newCustId.trim() != '') {
            //   page.SetActiveTabIndex(0);
            //   GetObjectID('hdnCustomerId').value = newCustId;

            //  GetObjectID('lblBillingStateText').value = BillingStateText;
            //  GetObjectID('lblBillingStateValue').value = BillingStateCode;

            // GetObjectID('lblShippingStateText').value = ShippingStateText;
            //  GetObjectID('lblShippingStateValue').value = ShippingStateCode;

            // cCustomerCallBackPanel.PerformCallback('SetCustomer~' + newCustId + '~' + CustomerName);
            // var FullName = new Array(CustUniqueName, CustomerName);
            // cCustomerComboBox.AddItem(FullName, newCustId);
            //  cCustomerComboBox.SetValue(newCustId);
            // $('#DeleteCustomer').val("yes");
            // page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
            //  cddl_SalesAgent.Focus();

            //}
        }
        function ParentNewCustomerMasterOnClose(newCustId, CustomerName, CustUniqueName, BillingStateText, BillingStateCode, ShippingStateText, ShippingStateCode) {
            AspxDirectAddCustPopup.Hide();
            var url = 'CustomerMasterList.aspx';

            window.location.href = url;

        }
        function OnAddButtonClick() {


            var isLighterPage = $("#hidIsLigherContactPage").val();
            // alert(isLighterPage);
            if (isLighterPage == 1) {
                var url = '/OMS/management/Master/Opportunity/OpportunityMaster.aspx';
                // alert(url);
                AspxDirectAddCustPopup.SetContentUrl(url);
                AspxDirectAddCustPopup.RefreshContentUrl();
                AspxDirectAddCustPopup.Show();
            }
            else {
                var url = 'OpportunityMaster.aspx?id=' + 'ADD';
                window.location.href = url;
            }
        }
        function OnCreateActivityClick(KeyVal, cnt_id, status, IsConvert) {
            //kaushik
            // var url = "Lead_Activity.aspx?id=" + KeyVal + "&cnt_id=" + cnt_id;
            if (IsConvert == 1) {
                jAlert('Qualified Lead Can Not Be Edited');
            }
            else {
                if (status == "Converted") {
                    jAlert("Your Current Status is: Converted .Cannot Proceed");
                    return false;
                }
                else if (status == "Lost") {
                    jAlert("Your Current Status is: Lost .Cannot Proceed");
                    return false;
                }
                else {
                    var url = "../ActivityManagement/Sales_Activity.aspx?id=" + KeyVal + "&cnt_id=" + cnt_id;
                    window.location.href = url;
                }
            }
        }
        function ShowHideFilter(obj) {
            if (document.getElementById('TxtSeg').value == 'N') {
                document.getElementById('TxtTCODE').style.display = "none";
            }
            else {
                document.getElementById('TxtTCODE').style.display = "inline";
            }
            InitialTextVal();
            if (obj == "s")
                //document.getElementById('TrFilter').style.display="inline";
                grid.PerformCallback('ssss');
            else {
                document.getElementById('TrFilter').style.display = "none";
                grid.PerformCallback(obj);
            }
        }
        function callback() {
            grid.PerformCallback();
        }
        function OnContactInfoClick(keyValue, CompName, IsConvert) {
            if (IsConvert == 1) {
                jAlert('Qualified Lead Can Not Be Edited');
            }
            else {
                var url = 'insurance_contactPerson.aspx?id=' + keyValue;
                window.location.href = url;
            }
        }
        function OnHistoryInfoClick(keyValue, CompName) {
            var url = 'ShowHistory_Phonecall.aspx?id1=' + keyValue;
            //OnMoreInfoClick(url, "Lead  History", '940px', '450px', "Y");
            window.location.href = url;
        }
        function OnAddBusinessClick(keyValue, CompName, IsConvert) {
            if (IsConvert == 1) {
                jAlert('Qualified Lead Can Not Be Edited');
            }
            else {
                var url = 'AssignIndustry.aspx?id1=' + keyValue;
                window.location.href = url;
            }
        }
        function btnSearch_click() {
            document.getElementById('TrFilter').style.display = "none";
            grid.PerformCallback('s');
        }
        function InitialTextVal() {


            document.getElementById('txtName').value = "Name";
            document.getElementById('txtBranchName').value = "Branch Name";
            document.getElementById('txtCode').value = "Code";
            document.getElementById('txtRelationManager').value = "R. Manager";
            document.getElementById('txtReferedBy').value = "Email";
            document.getElementById('txtPhNumber').value = "Ph. Number";
            document.getElementById('txtContactStatus').value = "Contact Status";
            //        document.getElementById('txtStatus').value = "Status";

            document.getElementById('TxtTCODE').value = "Trade. Code";
            document.getElementById('txtPAN').value = "PAN No.";
        }
    </script>
    <%--<style>
        #EmployeeGrid_DXMainTable>tbody>tr>td:last-child {
            display:none;
        }
        #EmployeeGrid {
            width:100% !important;
        }
    </style>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title" id="td_contact1" runat="server">
            <%--<h3>Contact List</h3>--%>
            <h3>
                <asp:Label ID="lblHeadTitle" runat="server" CssClass=""></asp:Label>
            </h3>
        </div>
        <div class="panel-title" id="td_broker1" runat="server">
            <h3>Broker List</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <%--<tr id="td_contact" runat="server">
                <td style="text-align: center;">
                    <strong><span style="color: #000099">Contact List</span></strong>
                </td>
            </tr>
            <tr id="td_broker" runat="server">
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">Broker List</span></strong>
                </td>
            </tr>--%>
            <tr>
                <td>
                    <%--  <table width="100%">
                        <tr>
                            <td style="text-align: left; vertical-align: top">
                                <table>
                                    <tr>
                                        <td id="ShowFilter">
                                            <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">
                                                Show Filter</span></a>
                                        </td>
                                        <td id="Td1">
                                            <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">
                                                All Records</span></a>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                            </td>
                            <td class="gridcellright" align="right">
                                <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" BackColor="Navy"
                                    Font-Bold="False" ForeColor="White" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                    ValueType="System.Int32" Width="130px">
                                    <Items>
                                        <dxe:ListEditItem Text="Select" Value="0" />
                                        <dxe:ListEditItem Text="PDF" Value="1" />
                                        <dxe:ListEditItem Text="XLS" Value="2" />
                                        <dxe:ListEditItem Text="RTF" Value="3" />
                                        <dxe:ListEditItem Text="CSV" Value="4" />
                                    </Items>
                                    <ButtonStyle BackColor="#C0C0FF" ForeColor="Black">
                                    </ButtonStyle>
                                    <ItemStyle BackColor="Navy" ForeColor="White">
                                        <HoverStyle BackColor="#8080FF" ForeColor="White">
                                        </HoverStyle>
                                    </ItemStyle>
                                    <Border BorderColor="White" />
                                    <DropDownButton Text="Export">
                                    </DropDownButton>
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>--%>
                    <div class="SearchArea">
                        <div class="FilterSide">
                            <div style="float: left; padding-right: 5px;">


                                <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-primary"><span>Add New</span> </a>

                            </div>
                            <%--<div style="float: left; padding-right: 5px;">
                        <a href="javascript:ShowHideFilter('s');" class="btn btn-primary"><span>
                            Show Filter</span></a>
                    </div>--%>
                            <div class="pull-left">
                                <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>

                                <% if (rights.CanExport)
                                   { %>
                                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnChange="if(!AvailableExportOption()){return false;}" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                    <asp:ListItem Value="2">XLS</asp:ListItem>
                                    <asp:ListItem Value="5">XLSX</asp:ListItem>
                                    <asp:ListItem Value="3">RTF</asp:ListItem>
                                    <asp:ListItem Value="4">CSV</asp:ListItem>

                                </asp:DropDownList>
                                <% } %>
                            </div>
                        </div>
                        <div class="ExportSide pull-right">
                            <div>
                                <%-- <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true"
                                    Font-Bold="False" ForeColor="black" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                    ValueType="System.Int32" Width="130px">
                                    <items>
                                    <dxe:ListEditItem Text="Select" Value="0" />
                                    <dxe:ListEditItem Text="PDF" Value="1" />
                                    <dxe:ListEditItem Text="XLS" Value="2" />
                                    <dxe:ListEditItem Text="RTF" Value="3" />
                                    <dxe:ListEditItem Text="CSV" Value="4" />
                                </items>
                                    <buttonstyle>
                                </buttonstyle>
                                    <itemstyle>
                                    <HoverStyle>
                                    </HoverStyle>
                                </itemstyle>
                                    <border bordercolor="black" />
                                    <dropdownbutton text="Export">
                                </dropdownbutton>
                                </dxe:ASPxComboBox>--%>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
            <tr id="TrFilter" style="display: none">
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtName" runat="server" CssClass="water" Text="Name" ToolTip="Name"
                                    Font-Size="12px" Width="119px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBranchName" runat="server" CssClass="water" Text="Branch Name"
                                    ToolTip="Branch Name" Font-Size="12px" Width="100px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCode" runat="server" CssClass="water" Text="Code" ToolTip="Code"
                                    Font-Size="12px" Width="54px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TxtTCODE" runat="server" CssClass="water" Text="Trade.Code" ToolTip="Trade.Code"
                                    Font-Size="12px" Width="79px"></asp:TextBox>
                                <asp:HiddenField ID="TxtSeg" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtPAN" runat="server" CssClass="water" Text="PAN No." ToolTip="PAN No."
                                    Font-Size="12px" Width="79px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRelationManager" runat="server" CssClass="water" Text="R. Manager"
                                    ToolTip="R. Manager" Font-Size="12px" Width="85px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferedBy" runat="server" CssClass="water" Text="Email" ToolTip="Email"
                                    Font-Size="12px" Width="92px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPhNumber" runat="server" CssClass="water" Text="Ph. Number" ToolTip="Ph. Number"
                                    Font-Size="12px" Width="90px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtContactStatus" runat="server" CssClass="water" Text="Contact Status"
                                    ToolTip="Contact Status" Font-Size="12px" Width="79px"></asp:TextBox>
                            </td>
                            <%--  <td visible="false">
                                    <asp:TextBox ID="txtStatus" runat="server" CssClass="water" Text="Status" ToolTip="Status"
                                        Font-Size="12px" Width="97px"></asp:TextBox>
                                </td>--%>
                            <td>
                                <input id="btnSearch" type="button" value="Search" class="btnUpdate" style="height: 21px"
                                    onclick="btnSearch_click()" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxGridView ID="EmployeeGrid" runat="server" KeyFieldName="cnt_Id" AutoGenerateColumns="False" OnDataBound="EmployeeGrid_DataBound"
                        DataSourceID="EmployeeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"
                        Width="100%" ClientInstanceName="grid" OnCustomJSProperties="EmployeeGrid_CustomJSProperties"
                        OnCustomCallback="EmployeeGrid_CustomCallback" OnHtmlRowCreated="EmployeeGrid_HtmlRowCreated" SettingsBehavior-AllowFocusedRow="true" Settings-HorizontalScrollBarMode="Visible">
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <ClientSideEvents EndCallback="function(s,e) { ShowError(s.cpInsertError);
                                                                                                 }" />
                        <SettingsPager NumericButtonCount="10" ShowSeparators="True" AlwaysShowPager="True" PageSize="10">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />

                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>

                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True"
                            PopupEditFormVerticalAlign="WindowCenter" EditFormColumnCount="3" />
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowFilterRow="true" ShowGroupPanel="true" ShowFilterRowMenu="true" />

                        <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" FilterRowMode="Auto" />

                        <SettingsText PopupEditFormCaption="Add/ Modify Employee" ConfirmDelete="Confirm delete?" />
                        <StylesPager>
                            <Summary Width="100%">
                            </Summary>
                        </StylesPager>
                        <Columns>
                            <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="0" FieldName="Id">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="2" FieldName="Name" Width="220px">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="BranchName" Width="220px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="Code" Caption="Unique ID" Width="180px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <%-- <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="CRG_TCODE" Caption="Trade. Code" Visible="false">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>--%>

                            <%--     <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="PanNumber" Caption="PAN No." >
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>--%>

                            <%-- <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="RM" Caption="Relationship Manager">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>--%>
                            <%--<dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="eml_email" Caption="Email (Official)">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>--%>

                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="phf_phoneNumber" Caption="Phone" Width="150px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="PanNumber" Caption="PAN" Width="140px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                           <%-- <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="Status" Caption="Contact Status">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>--%>

                            <%--  <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Status" Caption="Status"
                                Visible="false">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>--%>

                            <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="Activetype" Caption="Status" Width="100px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="EnterBy" Caption="Entered By" Width="120px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="EnteredDate" Caption="Entered Date" Width="160px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="ModifyUser" Caption="Updated By" Width="120px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="ModifyDateTime" Caption="Last Updated On" Width="160px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            

                            <dxe:GridViewDataTextColumn VisibleIndex="11" FieldName="gstin" Caption="GSTIN" Width="140px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="12" FieldName="Assign_To" Caption="Assigned To" Width="120px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="13" FieldName="Convert_To" Caption="Lead Status" Width="120px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="14" FieldName="Source" Caption="Source" Width="120px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="15" FieldName="Rating" Caption="Rating" Width="120px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="16" CellStyle-HorizontalAlign="Center" Width="200px">

                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <HeaderTemplate>Actions</HeaderTemplate>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <DataItemTemplate>

                                    <% if (Convert.ToString(Session["requesttype"]) != "Lead")
                                       {
                                           if (rights.CanCreateActivity)
                                           { %>
                                    <a href="javascript:void(0);" onclick="OnCreateActivityClick('<%# Eval("Id") %>','<%# Eval("cnt_id") %>','<%# Eval("Status") %>','<%#Eval("IsConvert") %>')" title="Create Activity" class="pad" style="text-decoration: none;">
                                        <img src="../../../assests/images/activity.png" />
                                    </a>
                                    <% }
                                       if (rights.CanEdit)
                                       {%>
                                    <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Eval("cnt_id") %>','<%#Eval("IsConvert") %>')" title=" More Info" class="pad" style="text-decoration: none;">
                                        <img src="../../../assests/images/info.png" />
                                    </a>
                                    <% }

                                           //Added By Chinmoy Date: 06-06-2018
                                           //Start

                                           if (Session["Contactrequesttype"].ToString() == "Transporter" && rights.CanView)
                                           {%>
                                    <a href="javascript:void(0);" onclick="ClickOnView('<%# Eval("Id") %>')" title="View" class="pad" style="text-decoration: none;">
                                        <img src="../../../assests/images/viewIcon.png" />
                                    </a>
                                    <% }


                                        //End


                                        if (rights.CanContactPerson)
                                        {%>
                                    <%--........................................Code Added By Sam on 27092016........................................Add Industry...--%>
                                    <a href="javascript:void(0);" onclick="OnContactInfoClick('<%#Eval("Id") %>','<%#Eval("Name") %>','<%#Eval("IsConvert") %>')" title="Add Contact Person" class="pad" style="text-decoration: none;">
                                        <img src="../../../assests/images/show.png" />
                                    </a>
                                    <% }
                                        if (rights.CanIndustry)
                                        { %>
                                    <a href="javascript:void(0);" onclick="OnAddBusinessClick('<%#Eval("Id") %>','<%#Eval("Name") %>','<%#Eval("IsConvert") %>')" title="Map Industry" class="pad" style="text-decoration: none;">
                                        <img src="../../../assests/images/icoaccts.gif" />
                                    </a>
                                    <%--........................................Code Added By Sam on 27092016...........................................--%>
                                    <%  }

                                        if (rights.CanDelete)
                                        { %>
                                    <a href="javascript:void(0);" onclick="OnDelete('<%# Eval("Id") %>','<%#Eval("IsConvert") %>')" title="Delete" class="pad">
                                        <img src="/assests/images/Delete.png" /></a>
                                    <%   }%>

                                    <% if (rights.CanBudget)
                                       { %>

                                    <a href="javascript:void(0);" onclick="OnBudgetopen('<%# Eval("cnt_Id") %>','<%#Eval("IsConvert") %>')" title="Budget" class="pad">
                                        <img src="/assests/images/cashbudget.png" width="16" height="16" /></a>

                                    <%   }%>
                                    <%-------------------------Added by Subhra 20-05-2019----------------%>

                                    <% if (rights.CanAssignTo)
                                       { %>

                                    <a href="javascript:void(0);" onclick="OnAssignTo('<%# Eval("Id") %>','<%#Eval("Name") %>','<%#Eval("EnterBy") %>','<%#Eval("Assign_Id") %>','<%#Eval("enteredby_id") %>')" title="Assign To" class="pad">
                                        <img src="/assests/images/userASign.png" /></a>

                                    <%   }%>

                                    <%--------------------------------------------------------------------%>

                                    <% } %>


                                    <% else
                                       { %>
                                    <%--........................................Code Added By Sam on 07112016...........................................--%>
                                    <%-- <a href="javascript:void(0);" onclick="OnCreateActivityClick('<%# Eval("Id") %>')" title="Create Activity" class="pad" style="text-decoration: none;">--%>
                                    <% if (rights.CanCreateActivity)
                                       { %>
                                    <a href="javascript:void(0);" onclick="OnCreateActivityClick('<%# Eval("Id") %>','<%# Eval("cnt_id") %>','<%# Eval("Status") %>','<%#Eval("IsConvert") %>')" title="Create Activity" class="pad" style="text-decoration: none;">

                                        <%--........................................Code Above Added By Sam on 07112016...........................................--%>

                                        <img src="../../../assests/images/activity.png" />
                                    </a>
                                    <% }
                                       if (rights.CanEdit)
                                       {%>
                                    <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Eval("cnt_id") %>','<%#Eval("IsConvert") %>')" title="More Info" class="pad" style="text-decoration: none;">
                                        <img src="../../../assests/images/info.png" />
                                    </a>
                                    <% }
                                                if (rights.CanContactPerson)
                                                {%>
                                    <a href="javascript:void(0);" onclick="OnContactInfoClick('<%#Eval("Id") %>','<%#Eval("Name") %>','<%#Eval("IsConvert") %>')" title="Show" class="pad" style="text-decoration: none;">
                                        <img src="../../../assests/images/show.png" />
                                    </a>
                                    <% }
                                                 if (rights.CanHistory)
                                                 {%>
                                    <a href="javascript:void(0);" onclick="OnHistoryInfoClick('<%#Eval("Id") %>','<%#Eval("Name") %>')" title="History" class="pad" style="text-decoration: none;">
                                        <img src="../../../assests/images/history.png" />
                                        <% }
                                               if (rights.CanIndustry)
                                               { %>
                                        <a href="javascript:void(0);" onclick="OnAddBusinessClick('<%#Eval("Id") %>','<%#Eval("Name") %>','<%#Eval("IsConvert") %>')" title="Map Industry" class="pad" style="text-decoration: none;">
                                            <img src="../../../assests/images/icoaccts.gif" />
                                        </a>
                                        <%  }

                                                if (rights.CanDelete)
                                                { %>
                                        <a href="javascript:void(0);" onclick="OnDelete('<%# Eval("Id") %>','<%#Eval("IsConvert") %>')" title="Delete" class="pad">
                                            <img src="/assests/images/Delete.png" /></a>
                                        <%   }
                                            //-------------------Subhra 16-05-2019--------------------------
                                            if (rights.CanAssignTo)
                                            { %>
                                        <a href="javascript:void(0);" onclick="OnAssignTo('<%# Eval("Id") %>','<%#Eval("Name") %>','<%#Eval("EnterBy") %>','<%#Eval("IsConvert") %>','<%#Eval("Assign_Id") %>','<%#Eval("enteredby_id") %>')" title="Assign To" class="pad">
                                            <img src="/assests/images/userASign.png" /></a>
                                        <%   }
                                           if (rights.CanConvertTo)
                                           { %>
                                        <a href="javascript:void(0);" onclick="OnConvertTo('<%# Eval("Id") %>','<%#Eval("Name") %>','<%#Eval("IsConvert") %>','<%#Eval("Assign_Id") %>','<%#Eval("enteredby_id") %>')" title="Convert To" class="pad">
                                            <img src="/assests/images/filter.png" /></a>
                                        <%   }
                                           if (rights.CanSalesActivity)
                                           { %>
                                        <a href="javascript:void(0);" onclick="OnSalesActivity('<%# Eval("Id") %>','<%#Eval("Name") %>','<%#Eval("IsConvert") %>','<%#Eval("Assign_Id") %>','<%#Eval("enteredby_id") %>')" title="Lead Activity" class="pad">
                                            <img src="/assests/images/rating.png" /></a>
                                        <%   }
                                           //-------------------Subhra 16-05-2019--------------------------

                                       }%>
                                </DataItemTemplate>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="17" FieldName="user_name"
                                Caption="Created User">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                        </Columns>
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="EmployeeDataSource" runat="server" SelectCommand="">
            <SelectParameters>
                <asp:SessionParameter Name="userlist" SessionField="userchildHierarchy" Type="string" />
            </SelectParameters>


        </asp:SqlDataSource>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>


        <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupbudget" Height="500px"
            Width="1310px" HeaderText="Budget" Modal="true" AllowResize="true" ResizingMode="Postponed">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>

            <ClientSideEvents CloseUp="BudgetAfterHide" />
        </dxe:ASPxPopupControl>
        <%--------Assign to Subhra 16-05-2019------------------%>
        <div class="modal fade pmsModal w30" id="AssignToModel" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Assign To</h4>
                    </div>
                    <div class="modal-body">
                        <div>
                            <dxe:ASPxLabel ID="lblenteredby" runat="server" Text="">
                            </dxe:ASPxLabel>
                        </div>
                        <div class="clearfix">
                            <div class="visF">
                                <div id="td_lAssignto" class="labelt">
                                    <div class="visF">
                                        <dxe:ASPxLabel ID="lblAssignTo" runat="server" Text="Assign To">
                                        </dxe:ASPxLabel>
                                    </div>

                                </div>
                                <div id="td_dAssignto">
                                    <div class="visF">
                                        <asp:DropDownList ID="cmbAssignTo" runat="server" TabIndex="1" Width="100%">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="">
                                <label>
                                    <dxe:ASPxLabel ID="lblRemarks" runat="server" Text="Remarks" CssClass="pdl8"></dxe:ASPxLabel>
                                    <span style="color: red;">*</span>

                                </label>
                                <div style="position: relative;">
                                    <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Columns="20" Rows="4" CssClass="form-control" TabIndex="2" MaxLength="500" Width="100%">
                                    </asp:TextBox>
                                </div>
                            </div>
                        </div>


                    </div>
                    <div class="modal-footer">
                        <%--<button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('CustomerSource')">Assign</button>--%>
                        <button type="button" id="btnAssign" class="btnOkformultiselection btn btn-success" onclick="Assign()">Assign</button>
                        <button type="button" id="btnUnAssign" class="btnOkformultiselection btn btn-danger" onclick="UnAssign()">Unassign</button>
                    </div>
                </div>
            </div>
        </div>

        <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelAssign" ClientInstanceName="cCallbackPanelAssign" OnCallback="PopupAssign_Callback">
            <PanelCollection>
                <dxe:PanelContent runat="server">
                </dxe:PanelContent>
            </PanelCollection>
            <ClientSideEvents EndCallback="CallbackPanelEndCall" />
        </dxe:ASPxCallbackPanel>

        <div class="modal fade pmsModal w30" id="ConvertToModel" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Convert To</h4>
                    </div>
                    <div class="modal-body">
                        <div>
                            <dxe:ASPxLabel ID="lblName" runat="server" Text="">
                            </dxe:ASPxLabel>
                        </div>
                        <div class="clearfix">
                            <div class="visF">
                                <div id="td_lConvertto" class="labelt">
                                    <div class="visF">
                                        <dxe:ASPxLabel ID="lblConvertTo" runat="server" Text="Convert To">
                                        </dxe:ASPxLabel>
                                        <span style="color: red;">*</span>
                                    </div>

                                </div>
                                <div id="td_dAssignto">
                                    <div class="visF">
                                        <asp:DropDownList ID="cmbLeadstatus" runat="server" TabIndex="1" Width="100%">
                                        </asp:DropDownList>

                                    </div>
                                </div>
                            </div>
                            <div class="">
                                <label>
                                    <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Remarks" CssClass="pdl8"></dxe:ASPxLabel>
                                    <span style="color: red;">*</span>

                                </label>
                                <div style="position: relative;">
                                    <asp:TextBox ID="txtConvertToRemarks" runat="server" TextMode="MultiLine" Columns="20" Rows="4" CssClass="form-control" TabIndex="2" MaxLength="500" Width="100%">
                                    </asp:TextBox>
                                </div>
                            </div>
                        </div>


                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btnOkformultiselection btn btn-success" onclick="SaveContact()">Save</button>
                        <button type="button" class="btnOkformultiselection btn btn-danger" onclick="CancelContact()">Cancel</button>
                    </div>
                </div>
            </div>
        </div>


        <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelConvertto" ClientInstanceName="cCallbackPanelConvertto" OnCallback="CallbackPanelConvertto_Callback">
            <PanelCollection>
                <dxe:PanelContent runat="server">
                </dxe:PanelContent>
            </PanelCollection>
            <ClientSideEvents EndCallback="CallbackPanelConverttoEndCall" />
        </dxe:ASPxCallbackPanel>
        <dxe:ASPxPopupControl ID="SalesActivityPopup" runat="server" ClientInstanceName="cSalesActivityPopup"
            Width="650px" HeaderText="Lead Activity" PopupHorizontalAlign="WindowCenter" Height="500px"
            PopupVerticalAlign="WindowCenter" CssClass="DevPopTypeNew" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentStyle VerticalAlign="Top"></ContentStyle>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelLeadActivity" ClientInstanceName="cCallbackPanelLeadActivity" OnCallback="CallbackPanelLeadActivity_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                              
                                <div class="col-md-12">
                                        <ul class="myAssignTarget" id="myAssignTargetpopup">

                                            <li class="mainCircle">
                                                <div class="heading"><dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Lead Name ">
                                                    </dxe:ASPxLabel>
                                                </div>
                                                <div id="lblsource" class="Num">&nbsp;<dxe:ASPxLabel ID="lblshowLeadName" runat="server" Text=""></dxe:ASPxLabel></div>
                                            </li>
                                            <li class="mainCircle">
                                                <div class="heading">
                                                    <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Due Date "></dxe:ASPxLabel>
                                                 </div>
                                                <div id="lblIndustry" class="Num">&nbsp;<dxe:ASPxLabel ID="lblshowDueDate" runat="server" Text=""></dxe:ASPxLabel></div>
                                            </li>
                                            <li class="mainCircle">
                                                <div class="heading">
                                                    <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Priority ">
                                                    </dxe:ASPxLabel>
                                                     
                                                </div>
                                                <div id="lblMiscComments" class="Num">&nbsp;
                                                    <dxe:ASPxLabel ID="lblshowPriority" runat="server" Text="">
                                                    </dxe:ASPxLabel>
                                                </div>
                                            </li>
                                           
                                        </ul>
                                      
                                    </div>
                                <div class="clear"></div>
                                <div class="clearfix">


                                   <div class="col-md-6">
                                         <div class="visF">
                                            <div id="ltd_ActivityDate" class="labelt">
                                                <div class="visF">
                                                    <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="Activity Date">
                                                    </dxe:ASPxLabel>
                                                    <span style="color: red;">*</span>
                                                </div>
                                            </div>
                                            <div id="td_ActivityDate">
                                                <div class="visF">
                                                  <dxe:ASPxDateEdit ID="dtActivityDate" TabIndex="9" runat="server"  Date="" Width="100%"  ClientInstanceName="cdtActivityDate">
                                                    <TimeSectionProperties>
                                                        <TimeEditProperties EditFormatString="hh:mm tt" />
                                                    </TimeSectionProperties>
                                                  </dxe:ASPxDateEdit>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-6">
                                         <div class="visF">
                                            <div id="td_Activity" class="labelt">
                                                <div class="visF">
                                                    <dxe:ASPxLabel ID="lblActivity" runat="server" Text="Activity">
                                                    </dxe:ASPxLabel>
                                                    <span style="color: red;">*</span>
                                                </div>
                                            </div>
                                            <div id="td_Type">
                                                <div class="visF">
                                                    <asp:DropDownList ID="cmbActivity"  runat="server" TabIndex="2" Width="100%">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="visF">
                                            <div class="labelt">
                                                <label>
                                                    <dxe:ASPxLabel ID="blnActivityType" runat="server" Text="Type" CssClass="pdl8"></dxe:ASPxLabel>
                                                    <span style="color: red;">*</span>

                                                </label>
                                                <div id="td_Type">
                                                    <div class="">
                                                        <asp:DropDownList ID="cmbType" runat="server" TabIndex="3" Width="100%">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-12">
                                        <div class="">
                                        <label>
                                            <dxe:ASPxLabel ID="lblSubject" runat="server" Text="Subject" CssClass="pdl8"></dxe:ASPxLabel>
                                            <span style="color: red;">*</span>

                                        </label>
                                        <div id="td_Type">
                                            <div class="">
                                                <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" TabIndex="4" MaxLength="500" Width="100%">
                                                </asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-12">
                                        <div class="">
                                        <label>
                                            <dxe:ASPxLabel ID="lblDetails" runat="server" Text="Details" CssClass="pdl8"></dxe:ASPxLabel>
                                            <span style="color: red;">*</span>

                                        </label>
                                        <div id="td_Details">
                                            <div class="">
                                                <asp:TextBox ID="txtDetails" runat="server" TextMode="MultiLine" Columns="20" Rows="4" CssClass="form-control" TabIndex="5" MaxLength="500" Width="100%">
                                                </asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-12">
                                        <div class="">
                                            <div id="td_lAssignto" class="labelt">
                                                <dxe:ASPxLabel ID="lblSalesActivityAssignTo" runat="server" Text="Assign To">
                                                </dxe:ASPxLabel>
                                                <span style="color: red;">*</span>
                                            </div>
                                            <div id="td_dAssignto">
                                                <asp:DropDownList ID="cmbSalesActivityAssignTo" runat="server" TabIndex="6" Width="100%">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-6">
                                        <div class="">
                                            <div id="td_lDuration" class="labelt">
                                                <dxe:ASPxLabel ID="lblDuration" runat="server" Text="Duration">
                                                </dxe:ASPxLabel>
                                                <span style="color: red;">*</span>
                                            </div>
                                            <div id="td_dDuration">
                                                <asp:DropDownList ID="cmbDuration" runat="server" TabIndex="7" Width="100%">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="">
                                            <div id="td_lPriority" class="labelt">
                                                <dxe:ASPxLabel ID="lblPriority" runat="server" Text="Priority">
                                                </dxe:ASPxLabel>
                                                <span style="color: red;">*</span>
                                            </div>
                                            <div id="td_dPriority">
                                                <asp:DropDownList ID="cmbPriority" runat="server" TabIndex="8" Width="100%">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="">
                                            <div id="td_lDue" class="labelt">
                                                <dxe:ASPxLabel ID="lblDue" runat="server" Text="Due" CssClass="pdl8"></dxe:ASPxLabel>
                                            </div>
                                            <div>
                                               <%-- <dxe:ASPxDateEdit ID="DtxtDue" runat="server" EditFormatString="dd-MM-yyyy hh:mm:ss"  ClientInstanceName="cDtxtDue" EditFormat="Custom" UseMaskBehavior="True" TabIndex="20" Width="100%">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                    <TimeSectionProperties>
                                                        <TimeEditProperties EditFormatString="hh:mm tt" />
                                                    </TimeSectionProperties>
                                                </dxe:ASPxDateEdit>--%>

                                                <dxe:ASPxDateEdit ID="DtxtDue" TabIndex="9" runat="server"  Date="" Width="100%"  ClientInstanceName="cDtxtDue">
                                                <TimeSectionProperties>
                                                    <TimeEditProperties EditFormatString="hh:mm tt" />
                                                </TimeSectionProperties>
                                                  <%-- <ClientSideEvents DateChanged="Enddate" />--%>
                                        </dxe:ASPxDateEdit>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                               
                               
                                <div class="clearfix">
                                   
                                    
                                    
                                    
                                    
                                    
                                    
                                    
                                    <asp:HiddenField ID="hdnEntityID" runat="server" />
                                </div>

                        <div class="modal-footer">
                            <button type="button" class="btnOkformultiselection btn btn-success" onclick="SaveSalesActivity()">Save</button>
                            <button type="button" class="btnOkformultiselection btn btn-danger" onclick="CancelSalesActivity()">Cancel</button>
                            <button type="button" class="btnOkformultiselection btn btn-info" onclick="ShowHistory()">Show History</button>
                       </div>
              
                            </dxe:PanelContent>
                        </PanelCollection>
                        <ClientSideEvents EndCallback="CallbackPanelLeadActivityEndCall" />
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>

        </dxe:ASPxPopupControl>

        <asp:HiddenField ID="hdnQualify" runat="server" />



       <dxe:ASPxPopupControl ID="popupShowHistory" runat="server" ClientInstanceName="cpopupShowHistory"
            Width="1200px" HeaderText="Show History" PopupHorizontalAlign="WindowCenter" Height="600px"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentStyle VerticalAlign="Top"></ContentStyle>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelShowHistory" ClientInstanceName="cCallbackPanelShowHistory" OnCallback="CallbackPanelShowHistory_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                              <div>
                                  <label class="checkbox-inline">
                                    <%--<asp:CheckBox ID="chkShowAll" runat="server"></asp:CheckBox>--%>
                                  <%--  <span style="margin: 0px 0; display: block">
                                        <dxe:ASPxLabel ID="ASPxLabel9" runat="server" Text="Show All">
                                        </dxe:ASPxLabel>
                                    </span>--%>
                                 </label>
                                 <button type="button" class="btnOkformultiselection btn btn-success mleft15" onclick="btn_ShowHistory()">Show All</button>
                                 <button type="button" class="btnOkformultiselection btn btn-success mleft15" onclick="btn_ShowTopTenHistory()">Show Top 10</button>
                              </div>

                              
                               <div>
                                    <dxe:ASPxGridView ID="showhistorygrid" runat="server" KeyFieldName="SalesActivityId" AutoGenerateColumns="False" 
                                        DataSourceID="ShowHistoryLeadDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"
                                        Width="100%" ClientInstanceName="cshowhistorygrid"  SettingsBehavior-AllowFocusedRow="true" Settings-HorizontalScrollBarMode="Visible">
                                        <SettingsSearchPanel Visible="True" Delay="5000" />
                                        <ClientSideEvents EndCallback="function(s,e) { ShowError(s.cpInsertError);}" />
                                        <SettingsPager NumericButtonCount="10" ShowSeparators="True" AlwaysShowPager="True" PageSize="10">
                                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />

                                            <FirstPageButton Visible="True">
                                            </FirstPageButton>
                                            <LastPageButton Visible="True">
                                            </LastPageButton>
                                        </SettingsPager>

                                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True"
                                            PopupEditFormVerticalAlign="WindowCenter" EditFormColumnCount="3" />
                                        <SettingsSearchPanel Visible="True" />
                                        <Settings ShowFilterRow="true" ShowGroupPanel="true" ShowFilterRowMenu="true" />

                                        <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" FilterRowMode="Auto" />

                                        <SettingsText PopupEditFormCaption="Add/ Modify Employee" ConfirmDelete="Confirm delete?" />
                                        <StylesPager>
                                            <Summary Width="100%">
                                            </Summary>
                                        </StylesPager>
                                        <Columns>
                                            <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="1" Caption="Module Name" FieldName="ModuleName">
                                                <CellStyle CssClass="gridcellleft">
                                                </CellStyle>
                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                <EditFormCaptionStyle HorizontalAlign="Right">
                                                </EditFormCaptionStyle>
                                                <EditFormSettings Visible="False"></EditFormSettings>
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="2" Caption="Activity Date" FieldName="ActivityDate">
                                                <CellStyle CssClass="gridcellleft">
                                                </CellStyle>
                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                <EditFormCaptionStyle HorizontalAlign="Right">
                                                </EditFormCaptionStyle>
                                                <EditFormSettings Visible="False"></EditFormSettings>
                                            </dxe:GridViewDataTextColumn>

                                             <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="3" Caption="Contact Type" FieldName="ContactType">
                                                <CellStyle CssClass="gridcellleft">
                                                </CellStyle>
                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                <EditFormCaptionStyle HorizontalAlign="Right">
                                                </EditFormCaptionStyle>
                                                <EditFormSettings Visible="False"></EditFormSettings>
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="4" Caption="Activity Name" FieldName="ActivityName">
                                                <CellStyle CssClass="gridcellleft">
                                                </CellStyle>
                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                <EditFormCaptionStyle HorizontalAlign="Right">
                                                </EditFormCaptionStyle>
                                                <EditFormSettings Visible="False"></EditFormSettings>
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="5" Caption="Activity Type" FieldName="ActivityTypeName" Width="220px">
                                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                                </CellStyle>
                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                <EditFormCaptionStyle HorizontalAlign="Right">
                                                </EditFormCaptionStyle>
                                                <EditFormSettings Visible="False"></EditFormSettings>
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewDataTextColumn VisibleIndex="6" Caption="Subject" FieldName="Leadsubject" Width="220px">
                                                <CellStyle CssClass="gridcellleft">
                                                </CellStyle>
                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                <EditFormCaptionStyle HorizontalAlign="Right">
                                                </EditFormCaptionStyle>
                                                <EditFormSettings Visible="False"></EditFormSettings>
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="Leaddetails" Caption="Details" Width="180px">
                                                <CellStyle CssClass="gridcellleft">
                                                </CellStyle>
                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                <EditFormCaptionStyle HorizontalAlign="Right">
                                                </EditFormCaptionStyle>
                                                <EditFormSettings Visible="False"></EditFormSettings>
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="AssignTo_Name" Caption="Assign To" Width="150px">
                                                <CellStyle CssClass="gridcellleft">
                                                </CellStyle>
                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                <EditFormCaptionStyle HorizontalAlign="Right">
                                                </EditFormCaptionStyle>
                                                <EditFormSettings Visible="False"></EditFormSettings>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="PriorityName" Caption="Priority" Width="140px">
                                                <CellStyle CssClass="gridcellleft">
                                                </CellStyle>
                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                <EditFormCaptionStyle HorizontalAlign="Right">
                                                </EditFormCaptionStyle>
                                                <EditFormSettings Visible="False"></EditFormSettings>
                                            </dxe:GridViewDataTextColumn>
                          
                                            <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="DurationName" Caption="Duration" Width="100px">
                                                <CellStyle CssClass="gridcellleft">
                                                </CellStyle>
                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                <EditFormCaptionStyle HorizontalAlign="Right">
                                                </EditFormCaptionStyle>
                                                <EditFormSettings Visible="False"></EditFormSettings>
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewDataTextColumn VisibleIndex="11" FieldName="Duedate" Caption="Due Date" Width="120px">
                                                <CellStyle CssClass="gridcellleft">
                                                </CellStyle>
                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                <EditFormCaptionStyle HorizontalAlign="Right">
                                                </EditFormCaptionStyle>
                                                <EditFormSettings Visible="False"></EditFormSettings>
                                            </dxe:GridViewDataTextColumn>

                                          
                                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="12" CellStyle-HorizontalAlign="Center" Width="100px">
                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                <HeaderTemplate>Actions</HeaderTemplate>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <DataItemTemplate>
                                                            <% if (rights.CanDelete)
                                                               { %>
                                                            <a href="javascript:void(0);" onclick="OnClickLeadActivityDelete('<%# Container.KeyValue %>')" class="pad" title="Delete" id="a_delete">
                                                                <img src="../../../assests/images/Delete.png" /></a>
                                                            <%} %>
                                                </DataItemTemplate>
                                            </dxe:GridViewDataTextColumn>
                                            


                                        </Columns>
                                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                                    </dxe:ASPxGridView>
                               </div>
                                  <asp:SqlDataSource ID="ShowHistoryLeadDataSource" runat="server" SelectCommand="">
                                  </asp:SqlDataSource>

                            </dxe:PanelContent>
                        </PanelCollection>
                        <ClientSideEvents EndCallback="CallbackPanelShowHistoryEndCall" />
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>

        </dxe:ASPxPopupControl>
        <%--------------Assign to------------------%>
        <dxe:ASPxPopupControl ID="DirectAddCustPopup" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="AspxDirectAddCustPopup" Height="650px"
            Width="1020px" HeaderText="Add New Customer" Modal="true" AllowResize="true" ResizingMode="Postponed">

            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>

        <asp:HiddenField ID="hidIsLigherContactPage" runat="server" />
    </div>
    <asp:HiddenField ID="hdnUserId" runat="server" />
      <%-- Subhra 04-06-2019 --%>
    <asp:HiddenField runat="server" ID="hdnUserRestrictionForLeadAction" />
    <%-- Subhra 04-06-2019 --%>
    <dxe:ASPxPopupControl ID="TransportView" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cTransportView" Height="650px"
        Width="1020px" HeaderText="Transporter View" Modal="true">

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>
</asp:Content>
