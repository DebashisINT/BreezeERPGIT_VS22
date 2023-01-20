<%--==========================================================Revision History ============================================================================================   
 1.0   Priti   V2.0.36   19-01-2023    0025371: Listing view upgradation required of Branch Requisition of Inventory
========================================== End Revision History =======================================================================================================--%>


<%@ Page Title="Branch Requisition" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="BranchRequisition.aspx.cs"
    Inherits="ERP.OMS.Management.Activities.BranchRequisition" EnableEventValidation="false" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchPopup.js"></script>
    <%--Code Added By Sandip For Approval Detail Section Start--%>


    <%-- Code Added By Sandip For Approval Detail Section End--%>
    <style>
        .BranchTo {
            position: absolute;
            right: -2px;
            top: 29px;
        }

        .brnchreq label {
            font-size: 13px;
            font-weight: 300;
        }

        .voucherno {
            position: absolute;
            right: -3px;
            top: 29px;
        }

        /*.dxgv {
            display: none;
        }*/

        .dxgvControl_PlasticBlue td.dxgvBatchEditModifiedCell_PlasticBlue {
            background: #fff !important;
        }

        #gridBatch_DXMainTable tr td:first-child {
            display: table-cell !important;
        }



        #gridBatch_DXStatus span > a {
            display: none;
        }

        #gridBatch_DXEditingErrorRow-1 {
            display: none;
        }



        #gridBatch .dxeButtonEditSys.dxeButtonEdit_PlasticBlue {
            margin-bottom: 0px !important;
            height: 27px;
        }

        .dxeTextBoxSys.dxeTextBox_PlasticBlue {
            height: 25PX;
        }

        .padTabtype2 > tbody > tr > td {
            padding-right: 15px;
        }

        padTabtype2 > tbody > tr > td:last-child {
            padding-right: 0px;
        }
    </style>

    <style>
        #gridBatch_DXMainTable > tbody > tr > td:last-child {
            display: none !important;
        }
    </style>
    <%--Batch Product Popup Start--%>
    <script src="JS/BranchRequisition.js?v=2.0"></script>
    <script>
        function FinYearCheckOnPageLoad() {
            var SelectedDate = new Date(ctDate.GetDate());
            var monthnumber = SelectedDate.getMonth();
            var monthday = SelectedDate.getDate();
            var year = SelectedDate.getYear();

            var SelectedDateValue = new Date(year, monthnumber, monthday);

            var FYS = "<%=Session["FinYearStart"]%>";
            var FYE = "<%=Session["FinYearEnd"]%>";
            var LFY = "<%=Session["LastFinYear"]%>";
            var FinYearStartDate = new Date(FYS);
            var FinYearEndDate = new Date(FYE);
            var LastFinYearDate = new Date(LFY);

            monthnumber = FinYearStartDate.getMonth();
            monthday = FinYearStartDate.getDate();
            year = FinYearStartDate.getYear();
            var FinYearStartDateValue = new Date(year, monthnumber, monthday);


            monthnumber = FinYearEndDate.getMonth();
            monthday = FinYearEndDate.getDate();
            year = FinYearEndDate.getYear();
            var FinYearEndDateValue = new Date(year, monthnumber, monthday);

            var SelectedDateNumericValue = SelectedDateValue.getTime();
            var FinYearStartDateNumericValue = FinYearStartDateValue.getTime();
            var FinYearEndDatNumbericValue = FinYearEndDateValue.getTime();
            if (SelectedDateNumericValue >= FinYearStartDateNumericValue && SelectedDateNumericValue <= FinYearEndDatNumbericValue) {
                //                   alert('Between');
            }
            else {
                if (SelectedDateNumericValue < FinYearStartDateNumericValue) {
                    ctDate.SetDate(new Date(FinYearStartDate));
                }
                if (SelectedDateNumericValue > FinYearEndDatNumbericValue) {
                    ctDate.SetDate(new Date(FinYearEndDate));
                }
            }
        }
        function TDateChange() {
            var SelectedDate = new Date(ctDate.GetDate());
            var monthnumber = SelectedDate.getMonth();
            var monthday = SelectedDate.getDate();
            var year = SelectedDate.getYear();

            var SelectedDateValue = new Date(year, monthnumber, monthday);
            ///Checking of Transaction Date For MaxLockDate
            var MaxLockDate = new Date('<%=Session["LCKBNK"]%>');
            monthnumber = MaxLockDate.getMonth();
            monthday = MaxLockDate.getDate();
            year = MaxLockDate.getYear();
            var MaxLockDateNumeric = new Date(year, monthnumber, monthday).getTime();

            if (SelectedDateValue <= MaxLockDateNumeric) {
                jAlert('This Entry Date has been Locked.');
                MaxLockDate.setDate(MaxLockDate.getDate() + 1);
                ctDate.SetDate(MaxLockDate);
                return;
            }
            ///End Checking of Transaction Date For MaxLockDate

            ///Date Should Between Current Fin Year StartDate and EndDate
            var FYS = "<%=Session["FinYearStart"]%>";
            var FYE = "<%=Session["FinYearEnd"]%>";
            var LFY = "<%=Session["LastFinYear"]%>";
            var FinYearStartDate = new Date(FYS);
            var FinYearEndDate = new Date(FYE);
            var LastFinYearDate = new Date(LFY);

            monthnumber = FinYearStartDate.getMonth();
            monthday = FinYearStartDate.getDate();
            year = FinYearStartDate.getYear();
            var FinYearStartDateValue = new Date(year, monthnumber, monthday);


            monthnumber = FinYearEndDate.getMonth();
            monthday = FinYearEndDate.getDate();
            year = FinYearEndDate.getYear();
            var FinYearEndDateValue = new Date(year, monthnumber, monthday);


            var SelectedDateNumericValue = SelectedDateValue.getTime();
            var FinYearStartDateNumericValue = FinYearStartDateValue.getTime();
            var FinYearEndDatNumbericValue = FinYearEndDateValue.getTime();
            if (SelectedDateNumericValue >= FinYearStartDateNumericValue && SelectedDateNumericValue <= FinYearEndDatNumbericValue) {

            }
            else {
                jAlert('Enter Date Is Outside Of Financial Year !!');
                if (SelectedDateNumericValue < FinYearStartDateNumericValue) {
                    ctDate.SetDate(new Date(FinYearStartDate));
                }
                if (SelectedDateNumericValue > FinYearEndDatNumbericValue) {
                    ctDate.SetDate(new Date(FinYearEndDate));
                }
            }
            ///End OF Date Should Between Current Fin Year StartDate and EndDate
        }

        //Rev Debashis
        function zoombranchreq(keyid, docno) {
            document.getElementById("divfromTo").style.display = 'none';
            $('#<%=hdnEditClick.ClientID %>').val('T'); //Edit
            //VisibleIndexE = e.visibleIndex;
            $('#<%= lblHeading.ClientID %>').text("View Branch Requisition");
            document.getElementById('DivEntry').style.display = 'block';
            document.getElementById('DivEdit').style.display = 'none';
            document.getElementById('btnAddNew').style.display = 'none';
            //btncross.style.display = "block";
            $('#<%=hdn_Mode.ClientID %>').val('View');
            InsgridBatch.PerformCallback("View~" + keyid);
            chkAccount = 1;
            document.getElementById('divNumberingScheme').style.display = 'none';
        }
        //End of Rev Debashis

        function CustomBtnView(keyValue) {
            document.getElementById("divfromTo").style.display = 'none';

            $('#<%=hdnEditClick.ClientID %>').val('T'); //Edit

            //VisibleIndexE = e.visibleIndex;
            $('#<%= lblHeading.ClientID %>').text("View Branch Requisition");
            document.getElementById('DivEntry').style.display = 'block';

            document.getElementById('DivEdit').style.display = 'none';
            document.getElementById('btnAddNew').style.display = 'none';

            btncross.style.display = "block";
            $('#<%=hdn_Mode.ClientID %>').val('View');
            InsgridBatch.PerformCallback("View~" + keyValue);

            chkAccount = 1;
            document.getElementById('divNumberingScheme').style.display = 'none';
            //Mantis Issue 25087
            CheckAddOrEdit();
            //End of Mantis Issue 25087
        }


        function CustomBtnEdit(keyValue, ClosedVal) {

            if (ClosedVal == "False") {
                document.getElementById("divfromTo").style.display = 'none';
                var userbranchID = '<%=Session["userbranchID"]%>';


                $('#<%=hdnEditClick.ClientID %>').val('T'); //Edit
                $('#<%=hdn_Mode.ClientID %>').val('Edit'); //Edit
                //VisibleIndexE = e.visibleIndex;

                $('#<%= lblHeading.ClientID %>').text("Modify Branch Requisition");
                document.getElementById('DivEntry').style.display = 'block';
                document.getElementById('DivEdit').style.display = 'none';
                document.getElementById('btnAddNew').style.display = 'none';
                btncross.style.display = "block";
                chkAccount = 1;

                InsgridBatch.PerformCallback("Edit~" + keyValue);

                document.getElementById('divNumberingScheme').style.display = 'none';
            }
            else {
                jAlert("Branch Requisition is already closed.Edit is not allowed");
            }
            //Mantis Issue 25087
            CheckAddOrEdit();
            //End of Mantis Issue 25087
        }


        function CustomBtnDelete(keyValue) {
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    // VisibleIndexE = e.visibleIndex;
                    CgvPurchaseIndent.PerformCallback("Delete~" + keyValue);

                }
                else {
                    return false;
                }
            });
        }


        function CustomBtnPrint(keyValue) {
            // var keyValueindex = s.GetRowKey(e.visibleIndex);
            onPrintJv(keyValue);
        }



        function CustomBtnCancel(keyValue, visibleIndex) {

            CgvPurchaseIndent.GetRowValues(visibleIndex, 'IsCancel', function (value) {
                if (value == true) {
                    jAlert("Branch requisition is already cancelled");
                }
                else {
                    jConfirm('Do you want to cancel the Branch Requisition?', 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            // VisibleIndexE = e.visibleIndex;
                            $("#<%=hddnKeyValue.ClientID%>").val(keyValue);

                            $('#MandatoryRemarksFeedback').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
                            cPopup_Feedback.Show();

                        }
                        else {
                            return false;
                        }
                    });
                }
            });
        }

        function CustomButtonClick(s, e) {
            if (e.buttonID == 'CustomBtnView') {
                document.getElementById("divfromTo").style.display = 'none';

                $('#<%=hdnEditClick.ClientID %>').val('T'); //Edit

                        VisibleIndexE = e.visibleIndex;
                        $('#<%= lblHeading.ClientID %>').text("View Branch Requisition");
                document.getElementById('DivEntry').style.display = 'block';

                document.getElementById('DivEdit').style.display = 'none';
                document.getElementById('btnAddNew').style.display = 'none';

                btncross.style.display = "block";
                $('#<%=hdn_Mode.ClientID %>').val('View');
                InsgridBatch.PerformCallback("View~" + VisibleIndexE);

                chkAccount = 1;
                document.getElementById('divNumberingScheme').style.display = 'none';
            }
            else if (e.buttonID == 'CustomBtnEdit') {

                document.getElementById("divfromTo").style.display = 'none';
                var userbranchID = '<%=Session["userbranchID"]%>';


               $('#<%=hdnEditClick.ClientID %>').val('T'); //Edit
               $('#<%=hdn_Mode.ClientID %>').val('Edit'); //Edit
               VisibleIndexE = e.visibleIndex;

               $('#<%= lblHeading.ClientID %>').text("Modify Branch Requisition");
                    document.getElementById('DivEntry').style.display = 'block';
                    document.getElementById('DivEdit').style.display = 'none';
                    document.getElementById('btnAddNew').style.display = 'none';
                    btncross.style.display = "block";
                    chkAccount = 1;

                    InsgridBatch.PerformCallback("Edit~" + VisibleIndexE);

                    document.getElementById('divNumberingScheme').style.display = 'none';
                }
                else if (e.buttonID == 'CustomBtnDelete') {
                    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            VisibleIndexE = e.visibleIndex;
                            CgvPurchaseIndent.PerformCallback("Delete~" + VisibleIndexE);

                        }
                        else {
                            return false;
                        }
                    });
                }
                else if (e.buttonID == 'CustomBtnPrint') {
                    var keyValueindex = s.GetRowKey(e.visibleIndex);
                    onPrintJv(keyValueindex);
                }
                else if (e.buttonID == 'CustomBtnCancel') {

                    CgvPurchaseIndent.GetRowValues(e.visibleIndex, 'IsCancel', function (value) {
                        if (value == true) {
                            jAlert("Branch requisition is already cancelled");
                        }
                        else {
                            jConfirm('Do you want to cancel the Branch Requisition?', 'Confirmation Dialog', function (r) {
                                if (r == true) {
                                    VisibleIndexE = e.visibleIndex;
                                    $("#<%=hddnKeyValue.ClientID%>").val(VisibleIndexE);

                                $('#MandatoryRemarksFeedback').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
                                cPopup_Feedback.Show();

                            }
                            else {
                                return false;
                            }
                        });
                    }
                });

            }
}

function OnEndCallback(s, e) {
    if (InsgridBatch.cpAddNewRow != null && InsgridBatch.cpAddNewRow != "") {
        InsgridBatch.cpAddNewRow = null;
        AddNewRow();
    }

    if (InsgridBatch.cpBtnVisible != null && InsgridBatch.cpBtnVisible != "") {
        InsgridBatch.cpBtnVisible = null;
        BtnVisible();
    }
    if (InsgridBatch.cpModifyOrNot != null && InsgridBatch.cpModifyOrNot != "") {

        document.getElementById('btnSaveExit').style.display = 'none'
        document.getElementById('btnnew').style.display = 'none'
        document.getElementById('taggModify').style.display = 'block'
        InsgridBatch.cpModifyOrNot = null;
    }
    if (InsgridBatch.cpEdit != null) {
        //Sandip Section For Approval Detail Start

        //Sandip Section For Approval Detail End
        var Indent_RequisitionNumber = InsgridBatch.cpEdit.split('~')[0];
        var Indent_RequisitionDate = InsgridBatch.cpEdit.split('~')[1];
        var Indent_BranchIdFor = InsgridBatch.cpEdit.split('~')[2];
        var Indent_Purpose = InsgridBatch.cpEdit.split('~')[3];
        var Indent_CurrencyId = InsgridBatch.cpEdit.split('~')[4];
        var Indent_ExchangeRtae = InsgridBatch.cpEdit.split('~')[5];
        var Indent_ID = InsgridBatch.cpEdit.split('~')[6];
        var Indent_ProjID = InsgridBatch.cpEdit.split('~')[8];
        document.getElementById('Keyval_internalId').value = "BranchRequisition" + Indent_ID;
        var Indent_BranchIdTo = InsgridBatch.cpEdit.split('~')[7];
        var Transdt = new Date(Indent_RequisitionDate);
        ctDate.SetDate(Transdt);
        document.getElementById('txtVoucherNo').value = Indent_RequisitionNumber;
        $("#txtVoucherNo").attr("disabled", "disabled");
        $("#ddlBranch").attr("disabled", "disabled");
        cddlBranchTo.SetEnabled(true);
        document.getElementById('hdnEditIndentID').value = Indent_ID;
        ctxtMemoPurpose.SetValue(Indent_Purpose);
        cCmbCurrency.SetValue(Indent_CurrencyId);
        document.getElementById('ddlBranch').value = Indent_BranchIdFor;



        cddlBranchTo.PerformCallback(Indent_BranchIdFor + '~' + Indent_BranchIdTo);

        ctxtRate.SetValue(Indent_ExchangeRtae);
        InsgridBatch.batchEditApi.StartEdit(-1, 1);

        if ($('#<%=hdnEditClick.ClientID %>').val() == 'T') {
            InsgridBatch.AddNewRow();
            var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
            var tbQuotation = InsgridBatch.GetEditor("SrlNo");
            tbQuotation.SetValue(noofvisiblerows);
            $('#<%=hdnEditClick.ClientID %>').val("");
        }

        //if ($("#hdnProjectSelectInEntryModule").val() == "1")
        //    clookup_Project.gridView.Refresh();
        if ($("#hdnProjectSelectInEntryModule").val() == "1")
            clookup_Project.gridView.SelectItemsByKey(Indent_ProjID);


        if (InsgridBatch.cpApproval != null) {
            if (InsgridBatch.cpApproval == 'A') {


                $('#lbl_quotestatusmsg').css('display', 'block');
                $('#lbl_quotestatusmsg').text('Document already approved');
                $('#btnnew').css('display', 'none');
                $('#btnSaveExit').css('display', 'none');
            }
            else if (InsgridBatch.cpApproval == 'R') {
                $('#lbl_quotestatusmsg').css('display', 'block');
                $('#lbl_quotestatusmsg').text('Document already rejected');
                $('#btnnew').css('display', 'none');
                $('#btnSaveExit').css('display', 'none');
            }
            else {
                $('#lbl_quotestatusmsg').css('display', 'none');
                $('#btnnew').css('display', 'block');
                $('#btnSaveExit').css('display', 'block');
            }

            //if ($("#hdnProjectSelectInEntryModule").val() == "1")
            //    clookup_Project.gridView.Refresh();
            if ($("#hdnProjectSelectInEntryModule").val() == "1")
                clookup_Project.gridView.SelectItemsByKey(Indent_ProjID);

        }



    }
    if (InsgridBatch.cpSaveSuccessOrFail == "nullQuantity") {

        InsgridBatch.AddNewRow();
        var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var tbQuotation = InsgridBatch.GetEditor("SrlNo");
        tbQuotation.SetValue(noofvisiblerows);
        InsgridBatch.cpSaveSuccessOrFail = null;
        $('#<%=hdnSaveNew.ClientID %>').val('');
        jAlert('Cannot save. Entered quantity must be greater then ZERO(0).');
        cLoadingPanelCRP.Hide();
    }
    else if (InsgridBatch.cpSaveSuccessOrFail == "duplicateProduct") {
        AddNewRow();
        InsgridBatch.cpSaveSuccessOrFail = null;
        $('#<%=hdnSaveNew.ClientID %>').val('');
        jAlert('Can not Add Duplicate Product in the Branch Requisition.');
        InsgridBatch.cpSaveSuccessOrFail = '';
        cLoadingPanelCRP.Hide();
    }
    else if (InsgridBatch.cpSaveSuccessOrFail == "outrange") {
        InsgridBatch.batchEditApi.StartEdit(0, 2);
        InsgridBatch.cpSaveSuccessOrFail = null;
        $('#<%=hdnSaveNew.ClientID %>').val('');
        jAlert('Can Not Add More Quotation Number as Quotation Scheme Exausted.<br />Update The Scheme and Try Again');
        cLoadingPanelCRP.Hide();

    }
    else if (InsgridBatch.cpSaveSuccessOrFail == "duplicate") {
        InsgridBatch.batchEditApi.StartEdit(0, 2);
        InsgridBatch.cpSaveSuccessOrFail = null;
        $('#<%=hdnSaveNew.ClientID %>').val('');
        jAlert('Can Not Save as Duplicate Quotation Numbe No. Found');
        cLoadingPanelCRP.Hide();

    }
    else if (InsgridBatch.cpSaveSuccessOrFail == "errorInsert") {
        InsgridBatch.batchEditApi.StartEdit(0, 2);
        InsgridBatch.cpSaveSuccessOrFail = null;
        $('#<%=hdnSaveNew.ClientID %>').val('');
        jAlert('Please try after sometime.');
        cLoadingPanelCRP.Hide();
    }
    else if (InsgridBatch.cpSaveSuccessOrFail == "EmptyProject") {
        InsgridBatch.batchEditApi.StartEdit(0, 2);
        InsgridBatch.cpSaveSuccessOrFail = null;
        $('#<%=hdnSaveNew.ClientID %>').val('');
                jAlert('Please select project.');
                cLoadingPanelCRP.Hide();
            }
            else if (InsgridBatch.cpSaveSuccessOrFail == "AddLock") {
                InsgridBatch.batchEditApi.StartEdit(0, 2);
                InsgridBatch.cpSaveSuccessOrFail = null;
                $('#<%=hdnSaveNew.ClientID %>').val('');
            jAlert('DATA is Freezed between ' + InsgridBatch.cpAddLockStatus + ' for Add.');
            cLoadingPanelCRP.Hide();
        }
        else if (InsgridBatch.cpSaveSuccessOrFail == "DocumentNumberAlreadyExists") {
            InsgridBatch.batchEditApi.StartEdit(0, 2);
            InsgridBatch.cpSaveSuccessOrFail = null;
            $('#<%=hdnSaveNew.ClientID %>').val('');
            jAlert('Document Number Already Exists');
            cLoadingPanelCRP.Hide();
        }

        else {
            if (InsgridBatch.cpVouvherNo != null) {
                var JV_Number = InsgridBatch.cpVouvherNo;

                var value = document.getElementById('hdnRefreshType').value;

                var JV_Msg = "Branch Requisition No. " + JV_Number + " generated.";
                var strSchemaType = document.getElementById('hdnSchemaType').value;

                if (value == "E") {

                    if (JV_Number != "") {
                        if (strSchemaType == '1') {

                            jAlert(JV_Msg, 'Alert Dialog: [BranchRequisition]', function (r) {
                                if (r == true) {
                                    InsgridBatch.cpVouvherNo = null;
                                    window.location.assign("BranchRequisition.aspx");
                                }
                            });

                        }
                        else {
                            window.location.assign("BranchRequisition.aspx");
                        }
                    }
                    else {
                        window.location.assign("BranchRequisition.aspx");
                    }

                }
                else if (value == "S") {

                    if (JV_Number != "") {
                        if (strSchemaType == '1') {
                            jAlert(JV_Msg, 'Alert Dialog: [BranchRequisition]', function (r) {
                                if (r == true) {
                                    InsgridBatch.cpVouvherNo = null;

                                }
                            });

                        }
                    }
                }
            }
            
            if ($('#<%=hdnSaveNew.ClientID %>').val() == "Save_Exit") {

                if (InsgridBatch.cpExitNew == "YES") {

                    if (InsgridBatch.cpApproverStatus == "approve") {
                        window.parent.popup.Hide();
                        window.parent.cgridPendingApproval.PerformCallback();
                        window.parent.parent.CgvPurchaseIndent.Refresh();
                    }

                    deleteAllRows();

                }
                else {
                    InsgridBatch.AddNewRow();
                    var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                    var tbQuotation = InsgridBatch.GetEditor("SrlNo");
                    tbQuotation.SetValue(noofvisiblerows);
                }
                var newInvoiceId = InsgridBatch.cpAutoID;
                //Rev Tanmoy Online Print Stop in Edit Mode
                if ($("#Keyval_internalId").val() == "Add") {
                    <%--Rev work start 28.07.2022 mantise no:0025074: A setting required "Is Online Printing Require For Branch Requisition ?"--%>
                    if ($("#hdnPrintingBranchRequisition").val() == "1") {
                        <%--Rev work close 28.07.2022 mantise no:0025074: A setting required "Is Online Printing Require For Branch Requisition ?--%>
                        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=BR-Default~D&modulename=BranchReq&id=" + newInvoiceId, '_blank');
                        <%--Rev work start 28.07.2022 mantise no:0025074: A setting required "Is Online Printing Require For Branch Requisition ?"--%>
                    }
                    <%--Rev work close 28.07.2022 mantise no:0025074: A setting required "Is Online Printing Require For Branch Requisition ?--%>
                }
                //Rev Tanmoy Online Print Stop in Edit Mode
            }
            if ($('#<%=hdnSaveNew.ClientID %>').val() == "Save_New") {

                ctxtMemoPurpose.SetValue("");
                $("#divNumberingScheme").show();
                var Campany_ID = '<%=Session["LastCompany"]%>';
                var LocalCurrency = '<%=Session["LocalCurrency"]%>';
                var basedCurrency = LocalCurrency.split("~");
                cCmbCurrency.SetValue(basedCurrency[0]);
                ctxtRate.SetValue("");
                ctxtRate.SetEnabled(false);
                $('#<%=lblHeading.ClientID %>').text("");
                $('#<%=lblHeading.ClientID %>').text("Add Branch Requisition");
                deleteAllRows();
                InsgridBatch.AddNewRow();
                var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                var tbQuotation = InsgridBatch.GetEditor("SrlNo");
                $('#<%=hdn_Mode.ClientID %>').val('Entry');
                tbQuotation.SetValue(noofvisiblerows);
                if (document.getElementById('txtVoucherNo').value == "Auto") {
                    document.getElementById('txtVoucherNo').value = "Auto";
                    $("#txtMemoPurpose_I").focus();
                }
                else {
                    document.getElementById('txtVoucherNo').value = "";

                    $('#txtVoucherNo').focus();
                }


                cCmbScheme.Focus();
                var newInvoiceId = InsgridBatch.cpAutoID;
                //Rev Tanmoy Online Print Stop in Edit Mode
                if ($("#Keyval_internalId").val() == "Add") {
                    <%--Rev work start 28.07.2022 mantise no:0025074: A setting required "Is Online Printing Require For Branch Requisition ?"--%>
                    if ($("#hdnPrintingBranchRequisition").val() == "1") {
                        <%--Rev work close 28.07.2022 mantise no:0025074: A setting required "Is Online Printing Require For Branch Requisition ?--%>
                        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=BR-Default~D&modulename=BranchReq&id=" + newInvoiceId, '_blank');
                        <%--Rev work start 28.07.2022 mantise no:0025074: A setting required "Is Online Printing Require For Branch Requisition ?"--%>
                    }
                    <%--Rev work close 28.07.2022 mantise no:0025074: A setting required "Is Online Printing Require For Branch Requisition ?--%>
                }
                //Rev Tanmoy Online Print Stop in Edit Mode
            }
        }
    if (InsgridBatch.cpView == "1") {
        viewOnly();
    }
}

function ProductsComboGotFocusChange(s, e) {

    var tbDescription = InsgridBatch.GetEditor("gvColDiscription");
    var tbUOM = InsgridBatch.GetEditor("gvColUOM");
    var tdRate = InsgridBatch.GetEditor("gvColRate");
    var AvailableStock = InsgridBatch.GetEditor("gvColAvailableStock");
    var ProductID = (InsgridBatch.GetEditor('gvColProduct').GetValue() != null) ? InsgridBatch.GetEditor('gvColProduct').GetValue() : "0";


    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strRate = SpliteDetails[6];
    chkAccount = 1;
    tbDescription.SetValue(strDescription);
    tbUOM.SetValue(strUOM);
    tdRate.SetValue(strRate);
    var Campany_ID = '<%=Session["LastCompany"]%>';
    var LastFinYear = '<%=Session["LastFinYear"]%>';
    var BranchFor = $("#ddlBranch").val();
    if (ProductID != "0" && ProductID != "") {

        $.ajax({
            type: "POST",
            url: 'BranchRequisition.aspx/getAvilableStock',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ Campany_ID: Campany_ID, ProductID: strProductID, LastFinYear: LastFinYear, BranchFor: BranchFor }),
            success: function (msg) {
                var data = msg.d;

                document.getElementById("pageheaderContent").style.display = 'block';


                var AvailableStock = data + " " + strUOM;
                $('#<%=B_AvailableStock.ClientID %>').text(AvailableStock);

            }
        });
        var BranchTo = $("#ddlBranchTo").val();

        if (BranchTo != "0" && ProductID != "") {

            $.ajax({
                type: "POST",
                url: 'BranchRequisition.aspx/getAvilableStock',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ Campany_ID: Campany_ID, ProductID: strProductID, LastFinYear: LastFinYear, BranchFor: BranchTo }),
                success: function (msg) {
                    var data = msg.d;

                    document.getElementById("liToBranch").style.display = 'block';


                    var AvailableStock = data + " " + strUOM;
                    $('#<%=B_AvailableStockToBranch.ClientID %>').text(AvailableStock);

                }
            });
        }

    }

}
function ProductsCombo_SelectedIndexChanged(s, e) {
    var tbDescription = InsgridBatch.GetEditor("gvColDiscription");
    var tbUOM = InsgridBatch.GetEditor("gvColUOM");
    var tdRate = InsgridBatch.GetEditor("gvColRate");
    var AvailableStock = InsgridBatch.GetEditor("gvColAvailableStock");
    var ProductID = s.GetValue();
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strRate = SpliteDetails[6];
    chkAccount = 1;
    tbDescription.SetValue(strDescription);
    tbUOM.SetValue(strUOM);
    tdRate.SetValue(strRate);
    var Campany_ID = '<%=Session["LastCompany"]%>';
    var LastFinYear = '<%=Session["LastFinYear"]%>';
    var BranchFor = $("#ddlBranch").val();
    $.ajax({
        type: "POST",
        url: 'BranchRequisition.aspx/getAvilableStock',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ Campany_ID: Campany_ID, ProductID: strProductID, LastFinYear: LastFinYear, BranchFor: BranchFor }),
        success: function (msg) {
            var data = msg.d;

            document.getElementById("pageheaderContent").style.display = 'block';

            var AvailableStock = data + " " + strUOM;
            $('#<%=B_AvailableStock.ClientID %>').text(AvailableStock);

        }
    });
    var BranchTo = $("#ddlBranchTo").val();

    if (BranchTo != "0") {
        $.ajax({
            type: "POST",
            url: 'BranchRequisition.aspx/getAvilableStock',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ Campany_ID: Campany_ID, ProductID: strProductID, LastFinYear: LastFinYear, BranchFor: BranchTo }),
            success: function (msg) {
                var data = msg.d;

                document.getElementById("pageheaderAvToBranch").style.display = 'block';

                var AvailableStock = data + " " + strUOM;
                $('#<%=B_AvailableStockToBranch.ClientID %>').text(AvailableStock);

            }
        });
    }

}

function Currency_Rate() {

    var Campany_ID = '<%=Session["LastCompany"]%>';
    var LocalCurrency = '<%=Session["LocalCurrency"]%>';
    var basedCurrency = LocalCurrency.split("~");
    var Currency_ID = cCmbCurrency.GetValue();
    $('#<%=hdnCurrenctId.ClientID %>').val(Currency_ID);


            if (cCmbCurrency.GetText().trim() == basedCurrency[1]) {
                ctxtRate.SetValue("");
                ctxtRate.SetEnabled(false);
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "BranchRequisition.aspx/GetRate",
                    data: JSON.stringify({ Currency_ID: Currency_ID, Campany_ID: Campany_ID, basedCurrency: basedCurrency[0] }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var data = msg.d;
                        ctxtRate.SetValue(data);


                    }
                });

                ctxtRate.SetEnabled(true);
            }

        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            setTimeout(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    InsgridBatch.SetWidth(cntWidth);
                    CgvPurchaseIndent.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    InsgridBatch.SetWidth(cntWidth);
                    CgvPurchaseIndent.SetWidth(cntWidth);
                }
            }, 200);
            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    InsgridBatch.SetWidth(cntWidth);
                    CgvPurchaseIndent.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    InsgridBatch.SetWidth(cntWidth);
                    CgvPurchaseIndent.SetWidth(cntWidth);
                }
            });
        });
        function goback(e) {
            //e.preventDefault();
            window.location.href = '/OMS/management/ProjectMainPage.aspx';
        }
        //Mantis Issue 25010
        function OnclickViewAttachment(obj) {
            var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=BranchRequisition';
            window.location.href = URL;
        }
        //End of Mantis Issue 25010
    </script>
    <%--Mantis Issue 25087--%>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('#hdnSettings').val() == "1" ) {
                $('#chkBranchRequisition').prop('checked', true);
            }
            else {
                $('#chkBranchRequisition').prop('checked', false);
            }
        })
    </script>
    <%--End of Mantis Issue 25087--%>
    <%--Batch Product Popup End--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading clearfix">
        <div class="panel-title clearfix">
            <div style="padding-right: 5px;">
                <h3 class="pull-left"><span class="">
                    <asp:Label ID="lblHeading" runat="server" Text="Branch Requisition"></asp:Label></span>
                </h3>
                <div id="pageheaderContent" class="pull-right wrapHolder reverse content horizontal-images" style="display: none;">
                    <div class="Top clearfix">
                        <ul>
                            <li id="liToBranch" style="display: none;">
                                <div class="lblHolder" style="max-width: 350px">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Available Balance of Request To Branch </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div style="width: 100%;">

                                                        <asp:Label ID="B_AvailableStockToBranch" runat="server" Text="0.0"></asp:Label>
                                                    </div>

                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>
                            <li>
                                <div class="lblHolder" style="max-width: 350px">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Available Balance of Request From Branch </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div style="width: 100%;">

                                                        <asp:Label ID="B_AvailableStock" runat="server" Text="0.0"></asp:Label>
                                                    </div>

                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>
                            <li>
                                <div class="lblHolder" style="display: none;">
                                    <table>
                                        <tr>
                                            <td>Stock Quantity</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblStkQty" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblStkUOM" runat="server" Text=" "></asp:Label>
                                                <asp:Label ID="lblStkUOMTo" runat="server" Text=" "></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>

                <%--Abhisek--%>
                <div id="divfromTo">
                    <table class="padTabtype2 pull-right brnchreq" style="margin-top: 7px">
                        <tr>
                            <td>
                                <label>From Date</label></td>
                            <td>
                                <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td>
                                <label>To Date</label>
                            </td>
                            <td>
                                <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td>
                                <label>Branch</label></td>
                            <td>
                                <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                                </dxe:ASPxComboBox>
                            </td>
                            <td>
                                <input type="button" value="Show" class="btn btn-primary" onclick="updateGridByDate()" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="ApprovalCross" runat="server" class="crossBtn"><a><i class="fa fa-times"></i></a></div>
                <div id="btncross" runat="server" class="crossBtn" style="display: none; margin-left: 50px;"><a href="BranchRequisition.aspx"><i class="fa fa-times"></i></a></div>
            </div>

        </div>

    </div>
    <div class="form_main">
        <div class="clearfix" id="btnAddNew">
            <div style="float: left; padding-right: 5px;">
                <% if (rights.CanAdd)
                   { %>
                <a href="javascript:void(0);" onclick="AddButtonClick()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus"></i></span><span><u>A</u>dd New</span> </a>
                <% } %>
                <% if (rights.CanExport)
                   { %>
                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="drdExport_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">PDF</asp:ListItem>
                    <asp:ListItem Value="2">XLS</asp:ListItem>
                    <asp:ListItem Value="3">RTF</asp:ListItem>
                    <asp:ListItem Value="4">CSV</asp:ListItem>
                </asp:DropDownList>
                <% } %>


                <%--Sandip Section for Approval Section in Design Start --%>

                <span id="spanStatus" runat="server">
                    <a href="javascript:void(0);" onclick="OpenPopUPUserWiseQuotaion()" class="btn btn-primary btn-radius">
                        <span>My Branch Requisition Status</span>

                    </a>
                </span>
                <span id="divPendingWaiting" runat="server">
                    <a href="javascript:void(0);" onclick="OpenPopUPApprovalStatus()" class="btn btn-primary btn-radius">
                        <span>Branch Requisition Waiting</span>
                        <asp:Label ID="lblWaiting" runat="server" Text=""></asp:Label>
                    </a>
                    <i class="fa fa-reply blink" style="font-size: 20px; margin-right: 10px;" aria-hidden="true"></i>

                </span>

                <%--Sandip Section for Approval Section in Design End --%>
            </div>
        </div>

        <div id="spnEditLock" runat="server" style="display: none; color: red; text-align: center"></div>
        <div id="spnDeleteLock" runat="server" style="display: none; color: red; text-align: center"></div>

        <div id="DivEntry" style="display: none">
            <div style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;" class="clearfix">
                <div class="col-md-2" id="divNumberingScheme">
                    <label style="">Numbering Scheme</label>
                    <div>
                        <dxe:ASPxComboBox ID="CmbScheme" EnableIncrementalFiltering="True" ClientInstanceName="cCmbScheme"
                            TextField="SchemaName" ValueField="ID" IncrementalFilteringMode="Contains"
                            runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                            <ClientSideEvents ValueChanged="function(s,e){CmbScheme_ValueChange()}" GotFocus="function(s,e){cCmbScheme.ShowDropDown();}" LostFocus="CmbScheme_LostFocus"></ClientSideEvents>
                        </dxe:ASPxComboBox>
                        <span id="MandatoryNumberingScheme" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                    </div>
                </div>
                <div class="col-md-2">
                    <label>Document No.<span style="color: red;">*</span></label>
                    <div>
                        <asp:TextBox ID="txtVoucherNo" runat="server" Width="100%" MaxLength="50" onchange="txtBillNo_TextChanged()">                             
                        </asp:TextBox>

                        <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                    </div>
                </div>
                <div class="col-md-2">
                    <label>Posting Date<span style="color: red;">*</span></label>
                    <div>
                        <dxe:ASPxDateEdit ID="tDate" runat="server" EditFormat="Custom" ClientInstanceName="ctDate" DisplayFormatString="dd-MM-yyyy"
                            UseMaskBehavior="True" Width="100%" EditFormatString="dd-MM-yyyy">

                            <ClientSideEvents DateChanged="function(s,e){TDateChange();}" GotFocus="function(s,e){ctDate.ShowDropDown();}" LostFocus="function(s, e) { SetLostFocusonDemand(e)}"></ClientSideEvents>
                            <ValidationSettings RequiredField-IsRequired="true" ErrorFrameStyle-CssClass="absolute"></ValidationSettings>
                        </dxe:ASPxDateEdit>
                    </div>
                </div>
                <div class="col-md-2">

                    <label>From Branch</label>
                    <div>
                        <asp:DropDownList ID="ddlBranch" runat="server" DataSourceID="dsBranch" onchange="ddlBranchFor_SelectedIndexChanged()"
                            DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%">
                        </asp:DropDownList>

                    </div>
                </div>
                <div class="col-md-2">

                    <label>Request To Branch<span style="color: red;">*</span></label>
                    <div>

                        <dxe:ASPxComboBox ID="ddlBranchTo" runat="server" ClientInstanceName="cddlBranchTo" Width="100%"
                            OnCallback="ddlBranchTo_Callback">
                            <ClientSideEvents SelectedIndexChanged="ChangeBranchTo" />
                        </dxe:ASPxComboBox>
                        <span id="MandatoryBranchTo" class="BranchTo  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                    </div>
                </div>


                <div style="clear: both"></div>
                <div class="col-md-8">
                    <label style="margin-bottom: 5px; display: inline-block">Purpose</label>
                    <div>
                        <dxe:ASPxMemo ID="txtMemoPurpose" ClientInstanceName="ctxtMemoPurpose" runat="server" Height="50px" Width="100%" Font-Names="Arial"></dxe:ASPxMemo>
                    </div>
                </div>



                <div class="col-md-2 lblmTop8">
                    <dxe:ASPxLabel ID="lblProject" runat="server" Text="Project">
                    </dxe:ASPxLabel>
                    <%-- <label id="lblProject" runat="server">Project</label>--%>
                    <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="EntityServerModeDataStock"
                        KeyFieldName="Proj_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False">
                        <Columns>
                            <dxe:GridViewDataColumn FieldName="Proj_Code" Visible="true" VisibleIndex="1" Caption="Project Code" Settings-AutoFilterCondition="Contains" Width="200px">
                            </dxe:GridViewDataColumn>
                            <dxe:GridViewDataColumn FieldName="Proj_Name" Visible="true" VisibleIndex="2" Caption="Project Name" Settings-AutoFilterCondition="Contains" Width="200px">
                            </dxe:GridViewDataColumn>
                            <dxe:GridViewDataColumn FieldName="Customer" Visible="true" VisibleIndex="3" Caption="Customer" Settings-AutoFilterCondition="Contains" Width="200px">
                            </dxe:GridViewDataColumn>
                            <dxe:GridViewDataColumn FieldName="HIERARCHY_NAME" Visible="true" VisibleIndex="4" Caption="Hierarchy" Settings-AutoFilterCondition="Contains" Width="200px">
                            </dxe:GridViewDataColumn>
                        </Columns>
                        <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                            <Templates>
                                <StatusBar>
                                    <table class="OptionsTable" style="float: right">
                                        <tr>
                                            <td></td>
                                        </tr>
                                    </table>
                                </StatusBar>
                            </Templates>
                            <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                        </GridViewProperties>
                        <ClientSideEvents GotFocus="function(s,e){clookup_Project.ShowDropDown();}" LostFocus="clookup_Project_LostFocus" ValueChanged="ProjectValueChange" />

                    </dxe:ASPxGridLookup>
                    <dx:LinqServerModeDataSource ID="EntityServerModeDataStock" runat="server" OnSelecting="EntityServerModeDataStock_Selecting"
                        ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />
                </div>
                <div class="col-md-2 lblmTop8">
                    <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="Hierarchy">
                    </dxe:ASPxLabel>
                    <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%" Enabled="false">
                    </asp:DropDownList>
                </div>



                <div style="clear: both"></div>
                <div>
                    <br />
                </div>
                <div style="display: none;">
                    <div class="col-md-1">
                        <label>Currency:  </label>
                        <div>
                            <dxe:ASPxComboBox ID="CmbCurrency" EnableIncrementalFiltering="True" ClientInstanceName="cCmbCurrency"
                                TextField="Currency_AlphaCode" ValueField="Currency_ID" DataSourceID="SqlCurrency"
                                runat="server" ValueType="System.String" EnableSynchronization="True" Width="100%" CssClass="pull-left">
                                <ClientSideEvents ValueChanged="function(s,e){Currency_Rate()}"></ClientSideEvents>
                            </dxe:ASPxComboBox>

                        </div>
                    </div>
                    <div class="col-md-2">
                        <label>Rate:  </label>
                        <div>
                            <dxe:ASPxTextBox runat="server" ID="txtRate" ClientInstanceName="ctxtRate" Width="100%" CssClass="pull-left">
                                <MaskSettings Mask="<0..9999>.<0..99999>" IncludeLiterals="DecimalSymbol" />

                            </dxe:ASPxTextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div>
                <div>
                    <br />
                </div>
                <dxe:ASPxGridView runat="server" ClientInstanceName="InsgridBatch" ID="gridBatch" KeyFieldName="PurchaseIndentID" OnBatchUpdate="gridBatch_BatchUpdate"
                    OnCellEditorInitialize="gridBatch_CellEditorInitialize" OnDataBinding="gridBatch_DataBinding"
                    Width="100%" Settings-ShowFooter="false" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                    OnRowInserting="Grid_RowInserting" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight="200" SettingsPager-Mode="ShowAllRecords"
                    OnRowUpdating="Grid_RowUpdating"
                    OnRowDeleting="Grid_RowDeleting"
                    OnCustomCallback="gridBatch_CustomCallback" >
                    <SettingsPager Visible="false"></SettingsPager>
                    <Columns>
                        <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="50" VisibleIndex="0" Caption="Action" HeaderStyle-HorizontalAlign="Center">
                            <CustomButtons>
                                <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDeleteIDS" Image-Url="/assests/images/crs.png">
                                </dxe:GridViewCommandColumnCustomButton>
                            </CustomButtons>
                        </dxe:GridViewCommandColumn>
                        <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl" VisibleIndex="1" Width="30">
                            <PropertiesTextEdit>
                            </PropertiesTextEdit>
                            <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <%-- <dxe:GridViewDataComboBoxColumn Caption="Product" FieldName="gvColProduct" VisibleIndex="2">
                            <PropertiesComboBox ValueField="ProductID" ClientInstanceName="ProductID" TextField="ProductName">
                                <ClientSideEvents SelectedIndexChanged="ProductsCombo_SelectedIndexChanged" GotFocus="ProductsComboGotFocusChange" />
                            </PropertiesComboBox>
                        </dxe:GridViewDataComboBoxColumn>--%>
                        <%--Batch Product Popup Start--%>

                        <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product" VisibleIndex="2">
                            <PropertiesButtonEdit>
                                <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" LostFocus="ProductsGotFocus" GotFocus="ProductsGotFocusFromID" />
                                <Buttons>
                                    <dxe:EditButton Text="..." Width="20px">
                                    </dxe:EditButton>
                                </Buttons>
                            </PropertiesButtonEdit>
                        </dxe:GridViewDataButtonEditColumn>
                        <dxe:GridViewDataTextColumn FieldName="gvColProduct" Caption="hidden Field Id" VisibleIndex="15" ReadOnly="True" Width="0"
                            EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide">
                            <CellStyle CssClass="hide"></CellStyle>
                        </dxe:GridViewDataTextColumn>

                        <%--Batch Product Popup End--%>
                        <dxe:GridViewDataTextColumn VisibleIndex="3" Caption="Description" FieldName="gvColDiscription">
                            <PropertiesTextEdit>
                            </PropertiesTextEdit>
                            <CellStyle Wrap="true" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Quantity" FieldName="gvColQuantity" Width="110" HeaderStyle-HorizontalAlign="Right">
                            <PropertiesTextEdit>
                                <MaskSettings Mask="<0..999999999>.<0..9999>" />
                                <ClientSideEvents LostFocus="AutoCalValue" />
                            </PropertiesTextEdit>

                            <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="5" Caption="UOM(Stock)" FieldName="gvColUOM" Width="110">
                            <PropertiesTextEdit>
                            </PropertiesTextEdit>
                            <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataDateColumn VisibleIndex="6" Caption="Expected Delivery Date" FieldName="ExpectedDeliveryDate" Width="140">
                            <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                <ClientSideEvents DateChanged="function(s,e){InstrumentDateChange();}"></ClientSideEvents>
                            </PropertiesDateEdit>
                            <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataDateColumn>

                        <dxe:GridViewDataTextColumn VisibleIndex="7" Caption="Remarks" FieldName="btnLineNarration" Width="160">
                            <PropertiesTextEdit>
                            </PropertiesTextEdit>
                            <CellStyle Wrap="False" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="80" VisibleIndex="8" Caption="Action" HeaderStyle-HorizontalAlign="Center">
                            <CustomButtons>
                                <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomAddNewRow" Image-Url="/assests/images/add.png">
                                </dxe:GridViewCommandColumnCustomButton>

                            </CustomButtons>
                        </dxe:GridViewCommandColumn>
                        <dxe:GridViewDataTextColumn Caption="Rate" FieldName="gvColRate" Width="0" HeaderStyle-HorizontalAlign="Right">
                            <PropertiesTextEdit>
                                <MaskSettings Mask="<0..999999999>.<0..99>" />
                                <ClientSideEvents LostFocus="AutoCalValueBtRate" />
                            </PropertiesTextEdit>
                            <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Value" FieldName="gvColValue" Width="0" HeaderStyle-HorizontalAlign="Right">
                            <PropertiesTextEdit>
                                <MaskSettings Mask="<0..999999999999999999>.<0..99>" />
                            </PropertiesTextEdit>
                            <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="10" Caption="Available Stock" FieldName="gvColAvailableStock" Width="0">
                            <PropertiesTextEdit NullTextStyle-CssClass="hide" ReadOnlyStyle-CssClass="hide" Style-CssClass="hide">
                                <MaskSettings Mask="<0..999999999999>.<0..99999>" />
                            </PropertiesTextEdit>
                            <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                        </dxe:GridViewDataTextColumn>


                    </Columns>

                    <ClientSideEvents EndCallback="OnEndCallback" RowClick="GetVisibleIndex"
                        CustomButtonClick="OnCustomButtonClick" BatchEditStartEditing="gridFocusedRowChanged" />
                    <SettingsDataSecurity AllowEdit="true" />
                    <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                        <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="Row" />
                    </SettingsEditing>

                    <Styles>
                        <StatusBar CssClass="statusBar">
                        </StatusBar>
                    </Styles>
                </dxe:ASPxGridView>
                <div>
                    <br />
                </div>
                <%--Mantis Issue 25087--%>
                <div class="row" id="divIsRequired" runat="server">
                    <div>
                        <div class="col-md-3">
                            <div class="checkbox">
                                <label class="red">
                                    <input type="checkbox" id="chkBranchRequisition" />
                                    Is SMS Required In Branch Requisition?</label>
                            </div>
                        </div>
                    </div>
                </div>
        <%--End of Mantis Issue 25087--%>
                <div>
                    <table style="float: left;">
                        <tr>

                            <td>
                                <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                <dxe:ASPxButton ID="btnnew" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & N&#818;ew"
                                    CssClass="btn btn-primary  hide"
                                    meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                    <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                                </dxe:ASPxButton>

                            </td>
                            <td>
                                <dxe:ASPxButton ID="btnSaveExit" ClientInstanceName="cbtn_SaveRecordsExit" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-primary"
                                    meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                    <ClientSideEvents Click="function(s, e) {SaveExitButtonClick();}" />
                                </dxe:ASPxButton>

                            </td>
                            <td>
                                <dxe:ASPxButton ID="btnSaveUdf" ClientInstanceName="cbtn_SaveUdf" runat="server" AutoPostBack="False" Text="U&#818;DF" UseSubmitBehavior="False"
                                    CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                    <ClientSideEvents Click="function(s, e) {OpenUdf();}" />
                                </dxe:ASPxButton>
                            </td>


                        </tr>
                        <tr><b><span id="tagged" style="display: none; color: red">Tagged in Branch Transfer Out. Cannot Modify</span></b></tr>
                        <tr><b><span id="taggModify" style="display: none; color: red">Requested By Other Branch Cannot Modify</span></b></tr>
                    </table>
                </div>
                <%--Batch Product Popup Start--%>

                <%--<dxe:ASPxPopupControl ID="ProductpopUp" runat="server" ClientInstanceName="cProductpopUp"
                    CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
                    Width="700" HeaderText="Select Product" AllowResize="true" ResizingMode="Postponed" Modal="true">
                    <ContentCollection>
                        <dxe:PopupControlContentControl runat="server">
                            <label><strong>Search By Product Name</strong></label>
                            <dxe:ASPxGridLookup ID="productLookUp" runat="server" DataSourceID="ProductDataSource" ClientInstanceName="cproductLookUp"
                                KeyFieldName="Products_ID" Width="800" TextFormatString="{0}" MultiTextSeparator=", " ClientSideEvents-TextChanged="ProductSelected" ClientSideEvents-KeyDown="ProductlookUpKeyDown">
                                <Columns>
                                    <dxe:GridViewDataColumn FieldName="Products_Name" Caption="Name" Width="220">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="IsInventory" Caption="Inventory" Width="60">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="HSNSAC" Caption="HSN/SAC" Width="80">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="ClassCode" Caption="Class" Width="200">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="BrandName" Caption="Brand" Width="100">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="sProducts_isInstall" Caption="Installation Reqd." Width="120">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>
                                </Columns>
                                <GridViewProperties Settings-VerticalScrollBarMode="Auto">

                                    <Templates>
                                        <StatusBar>
                                            <table class="OptionsTable" style="float: right">
                                                <tr>
                                                    <td>                                                      
                                                    </td>
                                                </tr>
                                            </table>
                                        </StatusBar>
                                    </Templates>
                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                </GridViewProperties>
                            </dxe:ASPxGridLookup>

                        </dxe:PopupControlContentControl>
                    </ContentCollection>
                    <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
                </dxe:ASPxPopupControl>

                <asp:SqlDataSource runat="server" ID="ProductDataSource" 
                    SelectCommand="prc_PurchaseIndentDetailsList" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:Parameter Type="String" Name="Action" DefaultValue="ProductDetails" />
                    </SelectParameters>
                </asp:SqlDataSource>--%>

                <%--Batch Product Popup End--%>
            </div>

        </div>
        <div id="DivEdit">
            <dxe:ASPxGridView ID="Grid_PurchaseIndent" runat="server" AutoGenerateColumns="False" OnCustomCallback="Grid_PurchaseIndent_CustomCallback"
                ClientInstanceName="CgvPurchaseIndent" KeyFieldName="Indent_Id" Width="100%" OnCustomButtonInitialize="Grid_PurchaseIndent_CustomButtonInitialize"
                SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true" SettingsBehavior-AllowFocusedRow="true"
                SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true" DataSourceID="EntityServerModeDataSource"
                SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"
                Settings-VerticalScrollableHeight="350" Settings-VerticalScrollBarMode="Auto" Settings-HorizontalScrollBarMode="Auto">
                <%--OnHtmlRowPrepared="Grid_PurchaseIndent_HtmlRowPrepared"--%>
                <%-- SettingsBehavior-ColumnResizeMode="Control"--%>

                <SettingsSearchPanel Visible="True" Delay="5000" />
                <%--  <SettingsSearchPanel Visible="false" />--%>
                <ClientSideEvents CustomButtonClick="CustomButtonClick" />
                <Columns>

                    <dxe:GridViewDataTextColumn VisibleIndex="1" Caption="Document No." FieldName="Indent_RequisitionNumber" Width="130px" FixedStyle="Left">

                        <CellStyle Wrap="False"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="2" Caption="Posting Date" FieldName="Indent_RequisitionDate" FixedStyle="Left" Width="100px">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <%-- <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"
                            DisplayFormatInEditMode="True">
                        </PropertiesTextEdit>--%>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="3" Caption="Requisition sent From" FieldName="Indent_branch" Width="250px">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Requested to Branch" FieldName="Indent_branch_to" Width="250px">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="5" Caption="Project Name" FieldName="Proj_Name" Width="180px">
                        <CellStyle CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <%--<dxe:GridViewDataTextColumn VisibleIndex="5" Caption="Total Amount" FieldName="ValueInBaseCurrency">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                    </dxe:GridViewDataTextColumn>--%>
                    <%-- <dxe:GridViewDataTextColumn VisibleIndex="6" Caption="Purpose" FieldName="Indent_Purpose">
                        <CellStyle CssClass="gridcellleft"></CellStyle>
                    </dxe:GridViewDataTextColumn>--%>
                    <dxe:GridViewDataTextColumn VisibleIndex="6" Caption="Transfer Out No." FieldName="Stk_TransferNumber" Width="100px">
                        <CellStyle CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="7" Caption="Transfer Out Date" FieldName="Stk_TransferDate" Width="120px">
                        <CellStyle CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="8" Caption="Transfer In No." FieldName="Stk_TransferNumberIN" Width="100px">
                        <CellStyle CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="9" Caption="Transfer In Date" FieldName="Stk_TransferDateIN" Width="100px">
                        <CellStyle CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="10" Caption="Pending" FieldName="Pending" Width="180px">
                        <CellStyle CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="11" Caption="Approval Status" FieldName="ApprovalStatus" Width="180px">
                        <CellStyle CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="12" Caption="Status" FieldName="DOC_STATUS" Width="180px">
                        <CellStyle CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="13" Caption="Purpose" FieldName="Indent_Purpose" Width="200px">
                        <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="14" Caption="Entered By" FieldName="EnteredBy" Width="100px">
                        <CellStyle CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="15" Caption="Entered On" FieldName="EnteredOn" Width="180px">
                        <CellStyle CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="16" Caption="Updated By" FieldName="UpdatedBy" Width="100px">
                        <CellStyle CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="17" Caption="Updated On" FieldName="Updated" Width="180px">
                        <CellStyle CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="18" Caption="Approved By" FieldName="ApprovedBy" Width="100px">
                        <CellStyle CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="19" Caption="Approved On" FieldName="ApprovedDate" Width="180px">
                        <CellStyle CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="20" Visible="false" FieldName="Indent_BranchIdFor" Width="0">
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="21" Visible="false" FieldName="IsCancel" Width="0">
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="22" Caption="IsClosed" FieldName="IsClosed" Width="0px">                        
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>



                    <dxe:GridViewDataCheckColumn VisibleIndex="23" Visible="false" Width="0">
                        <EditFormSettings Visible="True" />
                        <EditItemTemplate>
                            <dxe:ASPxCheckBox ID="ASPxCheckBox1" Text="" runat="server"></dxe:ASPxCheckBox>
                        </EditItemTemplate>
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataCheckColumn>
                    <dxe:GridViewDataTextColumn FieldName="Indent_Id" Visible="false" SortOrder="Descending" VisibleIndex="23" Width="0">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                    <%-- <dxe:GridViewCommandColumn VisibleIndex="23" Width="150px" Caption="Actions" ButtonType="Image" HeaderStyle-HorizontalAlign="Center">

                        <CustomButtons>
                            <dxe:GridViewCommandColumnCustomButton ID="CustomBtnView" Text="View" Styles-Style-CssClass="pad">
                                <Image Url="/assests/images/viewIcon.png" ToolTip="View"></Image>
                            </dxe:GridViewCommandColumnCustomButton>
                            <dxe:GridViewCommandColumnCustomButton ID="CustomBtnEdit" Text="Edit" Styles-Style-CssClass="pad">
                                <Image Url="/assests/images/Edit.png" ToolTip="Edit"></Image>
                            </dxe:GridViewCommandColumnCustomButton>
                            <dxe:GridViewCommandColumnCustomButton ID="CustomBtnDelete" Text="Delete">
                                <Image Url="/assests/images/Delete.png" ToolTip="Delete"></Image>
                            </dxe:GridViewCommandColumnCustomButton>
                            <dxe:GridViewCommandColumnCustomButton ID="CustomBtnPrint" Text="Print">
                                <Image Url="/assests/images/Print.png" ToolTip="Print"></Image>
                            </dxe:GridViewCommandColumnCustomButton>
                            <dxe:GridViewCommandColumnCustomButton ID="CustomBtnCancel" Text="Cancel">
                                <Image Url="/assests/images/not-verified.png" ToolTip="Cancel"></Image>
                            </dxe:GridViewCommandColumnCustomButton>
                        </CustomButtons>
                    </dxe:GridViewCommandColumn>--%>
                    <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="24" Width="200px">
                        <DataItemTemplate>
                            <% if (rights.CanView)
                               { %>
                            <a href="javascript:void(0);" onclick="CustomBtnView('<%#Eval("Indent_Id")%>')" class="pad" title="View">
                                <img src="../../../assests/images/doc.png" /></a>
                            <% } %>
                            <% if (rights.CanEdit)
                               { %>
                            <a href="javascript:void(0);" onclick="CustomBtnEdit('<%#Eval("Indent_Id")%>','<%#Eval("IsClosed")%>')" class="pad" title="Edit" style='<%#Eval("Editlock")%>'>
                                <img src="../../../assests/images/info.png" /></a><%} %>
                            <% if (rights.CanDelete)
                               { %>
                            <a href="javascript:void(0);" onclick="CustomBtnDelete('<%#Eval("Indent_Id")%>')" class="pad" title="Delete" style='<%#Eval("Deletelock")%>'>
                                <img src="../../../assests/images/Delete.png" /></a><%} %>


                            <% if (rights.CanPrint)
                               { %>
                            <a href="javascript:void(0);" onclick="CustomBtnPrint('<%#Eval("Indent_Id")%>')" class="pad" title="print">
                                <img src="../../../assests/images/Print.png" />
                            </a><%} %>

                            <% if (rights.CanCancel)
                               { %>
                            <a href="javascript:void(0);" onclick="CustomBtnCancel('<%#Eval("Indent_Id")%>','<%# Container.VisibleIndex %>')" class="pad" title="Cancel">
                                <img src="../../../assests/images/not-verified.png" />
                            </a><%} %>
                            <%-- <% if (rights.CanClose)
                               { %>--%>
                            <a href="javascript:void(0);" onclick="OnClosedClick('<%# Container.KeyValue %>','<%# Container.VisibleIndex %>')" class="pad" title="Close">
                                <img src="../../../assests/images/closePop.png" /></a>
                            <%--  <% } %>--%>
                            <%--Mantis Issue 25010--%>
                            <%--Mantis Issue 25127--%>
                            <%--<% if (rights.CanView)
                               { %>--%>
                            <% if (rights.CanAddUpdateDocuments)
                               { %>
                            <%--End of Mantis Issue 25127--%>
                            <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%#Eval("Indent_Id")%>')" class="pad" title="Add/View Attachment">
                                <img src="../../../assests/images/upload.png" /></a>
                            <% } %>
                            <%--End of Mantis Issue 25010--%>
                            <%--Mantis Issue 25087--%>
                            <% if (rights.SendSMS)
                               { %>
                            <a href="javascript:void(0);" onclick="onSmsClickJv('<%#Eval("Indent_Id")%>')" id="onSmsClickJv" class="pad" title="Send Sms">
                                <img src="../../../assests/images/sms.png" />
                            </a>
                            <% } %>
                            <%--End of Mantis Issue 25087--%>
                        </DataItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <CellStyle HorizontalAlign="Center"></CellStyle>
                        <HeaderTemplate><span>Actions</span></HeaderTemplate>
                        <EditFormSettings Visible="False"></EditFormSettings>
                    </dxe:GridViewDataTextColumn>

                </Columns>

                <SettingsCookies Enabled="true" StorePaging="true" Version="9" />

                <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                <ClientSideEvents EndCallback="function(s, e) {
	                                        ShowMsgLastCall();
                                        }" />
                <SettingsBehavior ConfirmDelete="True" />
                <Styles>
                    <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
                    <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>
                    <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                    <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
                    <Footer CssClass="gridfooter"></Footer>
                </Styles>
                <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                </SettingsPager>

            </dxe:ASPxGridView>
            <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                ContextTypeName="ERPDataClassesDataContext" TableName="V_BranchRequisitionList" />
            <asp:HiddenField ID="hfIsFilter" runat="server" />
            <asp:HiddenField ID="hfFromDate" runat="server" />
            <asp:HiddenField ID="hfToDate" runat="server" />
            <asp:HiddenField ID="hfBranchID" runat="server" />
            <dxe:ASPxGridViewExporter ID="exporter" runat="server">
            </dxe:ASPxGridViewExporter>
            <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
                <ClientSideEvents ControlsInitialized="AllControlInitilize" />
            </dxe:ASPxGlobalEvents>
            <dxe:ASPxPopupControl ID="Popup_Feedback" runat="server" ClientInstanceName="cPopup_Feedback"
                Width="400px" HeaderText="Reason For Cancel" PopupHorizontalAlign="WindowCenter"
                BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">

                        <div class="Top clearfix">
                            <table style="width: 94%">
                                <tr>
                                    <td>Reason<span style="color: red">*</span></td>
                                    <td class="relative">
                                        <dxe:ASPxMemo ID="txtInstFeedback" runat="server" Width="100%" Height="30px" ClientInstanceName="txtFeedback"></dxe:ASPxMemo>
                                        <span id="MandatoryRemarksFeedback" style="display: none">
                                            <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor1234_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                    </td>
                                </tr>
                                <tr></tr>
                                <tr>
                                    <td colspan="3" style="padding-left: 121px;">
                                        <input id="btnFeedbackSave" class="btn btn-primary" onclick="CallFeedback_save()" type="button" value="Save" />
                                        <input id="btnFeedbackCancel" class="btn btn-danger" onclick="CancelFeedback_save()" type="button" value="Cancel" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="LightGray" ForeColor="Black" />
            </dxe:ASPxPopupControl>
        </div>
        <asp:HiddenField ID="hdnRefreshType" runat="server" />
        <asp:HiddenField ID="hdnEditIndentID" runat="server" />
        <asp:HiddenField ID="hdfIsDelete" runat="server" />
        <asp:HiddenField ID="hdnSchemaType" runat="server" />
        <asp:HiddenField ID="hdnCurrenctId" runat="server" />
        <asp:HiddenField ID="hdnSaveNew" runat="server" />
        <asp:HiddenField ID="hdn_Mode" runat="server" />

        <asp:HiddenField ID="hdnEditClick" runat="server" />
        <asp:SqlDataSource ID="dsBranchTo" runat="server"
            ConflictDetection="CompareAllValues"
            SelectCommand=""></asp:SqlDataSource>
        <asp:SqlDataSource ID="dsBranch" runat="server"
            ConflictDetection="CompareAllValues"
            SelectCommand=""></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlSchematype" runat="server"
            SelectCommand="Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select ID,SchemaName From tbl_master_Idschema  Where TYPE_ID='16')) as X Order By ID ASC"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlCurrency" runat="server"
            SelectCommand="select Currency_ID,Currency_AlphaCode from Master_Currency Order By Currency_ID"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlCurrencyBind" runat="server"></asp:SqlDataSource>

        <%-- Sandip Approval Dtl Section Start--%>
        <asp:HiddenField ID="hdngridkeyval" runat="server" />
        <div class="PopUpArea">
            <dxe:ASPxPopupControl ID="popupApproval" runat="server" ClientInstanceName="cpopupApproval"
                Width="900px" HeaderText="Pending Approvals" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
                PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                ContentStyle-CssClass="pad">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                        <div class="row">
                            <div class="col-md-12">
                                <dxe:ASPxGridView ID="gridPendingApproval" runat="server" KeyFieldName="ID" AutoGenerateColumns="False" OnPageIndexChanged="gridPendingApproval_PageIndexChanged"
                                    Width="100%" ClientInstanceName="cgridPendingApproval" OnCustomCallback="gridPendingApproval_CustomCallback">
                                    <Columns>
                                        <dxe:GridViewDataTextColumn Caption="Document No." FieldName="Number"
                                            VisibleIndex="0" FixedStyle="Left">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Created On" FieldName="CreateDate"
                                            VisibleIndex="1" FixedStyle="Left">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Branch" FieldName="branch_description"
                                            VisibleIndex="2" FixedStyle="Left">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="craetedby"
                                            VisibleIndex="3" FixedStyle="Left">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataCheckColumn UnboundType="Boolean" Caption="Approved">
                                            <DataItemTemplate>
                                                <dxe:ASPxCheckBox ID="chkapprove" runat="server" AllowGrayed="false" OnInit="chkapprove_Init" ValueType="System.Boolean" ValueChecked="true" ValueUnchecked="false">
                                                    <ClientSideEvents CheckedChanged="function (s, e) {ch_fnApproved();}" />
                                                </dxe:ASPxCheckBox>
                                            </DataItemTemplate>
                                            <Settings ShowFilterRowMenu="False" AllowFilterBySearchPanel="False" AllowAutoFilter="False" />
                                        </dxe:GridViewDataCheckColumn>

                                        <dxe:GridViewDataCheckColumn UnboundType="Boolean" Caption="Rejected">
                                            <DataItemTemplate>
                                                <dxe:ASPxCheckBox ID="chkreject" runat="server" AllowGrayed="false" OnInit="chkreject_Init" ValueType="System.Boolean" ValueChecked="true" ValueUnchecked="false">
                                                    <ClientSideEvents CheckedChanged="function (s, e) {ch_fnApproved();}" />
                                                </dxe:ASPxCheckBox>
                                            </DataItemTemplate>
                                            <Settings ShowFilterRowMenu="False" AllowFilterBySearchPanel="False" AllowAutoFilter="False" />
                                        </dxe:GridViewDataCheckColumn>
                                    </Columns>
                                    <SettingsBehavior AllowFocusedRow="true" />
                                    <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True">
                                        <FirstPageButton Visible="True">
                                        </FirstPageButton>
                                        <LastPageButton Visible="True">
                                        </LastPageButton>
                                    </SettingsPager>
                                    <SettingsEditing Mode="Inline">
                                    </SettingsEditing>
                                    <SettingsSearchPanel Visible="True" />
                                    <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                    <SettingsLoadingPanel Text="Please Wait..." />
                                    <ClientSideEvents EndCallback="OnApprovalEndCall" />
                                </dxe:ASPxGridView>
                            </div>
                            <div class="clear"></div>



                        </div>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
            </dxe:ASPxPopupControl>
            <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
                Width="1200px" HeaderText="Quotation Approval" Modal="true" AllowResize="true" ResizingMode="Postponed">
                <HeaderTemplate>
                    <span>User Approval</span>
                </HeaderTemplate>
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                    </dxe:PopupControlContentControl>
                </ContentCollection>
            </dxe:ASPxPopupControl>
            <dxe:ASPxPopupControl ID="PopupUserWiseQuotation" runat="server" ClientInstanceName="cPopupUserWiseQuotation"
                Width="900px" HeaderText="Branch Requisition Status" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
                PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                ContentStyle-CssClass="pad">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <div class="row">
                            <div class="col-md-12">
                                <dxe:ASPxGridView ID="gridUserWiseQuotation" runat="server" KeyFieldName="ID" AutoGenerateColumns="False" OnPageIndexChanged="gridUserWiseQuotation_PageIndexChanged"
                                    Width="100%" ClientInstanceName="cgridUserWiseQuotation" OnCustomCallback="gridUserWiseQuotation_CustomCallback">
                                    <Columns>
                                        <dxe:GridViewDataTextColumn Caption="Document No." FieldName="number"
                                            VisibleIndex="0" FixedStyle="Left">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Entered On" FieldName="createddate"
                                            VisibleIndex="1" FixedStyle="Left">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Approval User" FieldName="approvedby"
                                            VisibleIndex="2" FixedStyle="Left">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="User Level" FieldName="UserLevel"
                                            VisibleIndex="3" FixedStyle="Left">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Status" FieldName="status"
                                            VisibleIndex="4" FixedStyle="Left">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Approved On" FieldName="ApprovedOn"
                                            VisibleIndex="5" FixedStyle="Left">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsBehavior AllowFocusedRow="true" />
                                    <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True">
                                        <FirstPageButton Visible="True">
                                        </FirstPageButton>
                                        <LastPageButton Visible="True">
                                        </LastPageButton>
                                    </SettingsPager>
                                    <SettingsEditing Mode="Inline">
                                    </SettingsEditing>
                                    <SettingsSearchPanel Visible="True" />
                                    <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                    <SettingsLoadingPanel Text="Please Wait..." />

                                </dxe:ASPxGridView>
                            </div>
                            <div class="clear"></div>
                        </div>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
            </dxe:ASPxPopupControl>
        </div>

        <%--DEBASHIS--%>
        <div class="PopUpArea">
            <dxe:ASPxPopupControl ID="ASPxDocumentsPopup" runat="server" ClientInstanceName="cDocumentsPopup"
                Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" ClientSideEvents-EndCallback="cSelectPanelEndCall">
                            <PanelCollection>
                                <dxe:PanelContent runat="server">
                                    <dxe:ASPxComboBox ID="CmbDesignName" ClientInstanceName="cCmbDesignName" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                    </dxe:ASPxComboBox>
                                    <div class="text-center pTop10">
                                        <dxe:ASPxButton ID="btnOK" ClientInstanceName="cbtnOK" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                            <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                </dxe:PanelContent>
                            </PanelCollection>
                        </dxe:ASPxCallbackPanel>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
            </dxe:ASPxPopupControl>
        </div>
        <%--DEBASHIS--%>

        <%-- Sandip Approval Dtl Section End--%>
        <dxe:ASPxCallbackPanel runat="server" ID="acpAvailableStock" ClientInstanceName="cacpAvailableStock" OnCallback="acpAvailableStock_Callback">
            <PanelCollection>
                <dxe:PanelContent runat="server">
                </dxe:PanelContent>
            </PanelCollection>
            <ClientSideEvents EndCallback="acpAvailableStockEndCall" />
        </dxe:ASPxCallbackPanel>
        <%--UDF Popup --%>
        <dxe:ASPxPopupControl ID="ASPXPopupControl1" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cUDFpopup" Height="630px"
            Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed">

            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
        <asp:HiddenField runat="server" ID="IsUdfpresent" />
        <asp:HiddenField runat="server" ID="Keyval_internalId" />
        <asp:HiddenField ID="hddnKeyValue" runat="server" />
        <asp:HiddenField ID="hddnIsSavedFeedback" runat="server" />
        <%--UDF Popup End--%>

        <!--Product Modal -->
        <div class="modal fade" id="ProductModel" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Product Search</h4>
                    </div>
                    <div class="modal-body">
                        <input type="text" onkeydown="prodkeydown(event)" id="txtProdSearch" autofocus width="100%" placeholder="Search By Product Name or Product Code" />

                        <div id="ProductTable">
                            <table border='1' width="100%" class="dynamicPopupTbl">
                                <tr class="HeaderStyle">
                                    <th class="hide">id</th>
                                    <th>Product Code</th>
                                    <th>Product Name</th>
                                    <th>Inventory</th>
                                    <th>HSN/SAC</th>
                                    <th>Class</th>
                                    <th>Brand</th>
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
        <!--Product Modal -->
        <dxe:ASPxPopupControl ID="Popup_Closed" runat="server" ClientInstanceName="cPopup_Closed"
            Width="400px" HeaderText="Reason For Close" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                    <div class="Top clearfix">

                        <table style="width: 94%">

                            <tr>
                                <td>Reason<span style="color: red">*</span></td>
                                <td class="relative">
                                    <dxe:ASPxMemo ID="ASPxMemo1" runat="server" Width="100%" Height="50px" ClientInstanceName="txtClosed"></dxe:ASPxMemo>
                                    <span id="MandatoryRemarksFeedback1" style="display: none">
                                        <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor1234_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                </td>
                            </tr>

                            <tr>
                                <td></td>
                                <td colspan="2" style="padding-top: 10px;">
                                    <input id="btnClosedSave" class="btn btn-primary" onclick="CallClosed_save()" type="button" value="Save" />
                                    <input id="btnClosedCancel" class="btn btn-danger" onclick="CancelClosed_save()" type="button" value="Cancel" />
                                </td>

                            </tr>
                        </table>


                    </div>

                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />


        </dxe:ASPxPopupControl>
        <dxe:ASPxCallbackPanel runat="server" ID="acbpCrpUdf" ClientInstanceName="cacbpCrpUdf" OnCallback="acbpCrpUdf_Callback">
            <PanelCollection>
                <dxe:PanelContent runat="server">
                </dxe:PanelContent>
            </PanelCollection>
            <ClientSideEvents EndCallback="acbpCrpUdfEndCall" />
        </dxe:ASPxCallbackPanel>
        <dxe:ASPxLoadingPanel ID="LoadingPanelCRP" runat="server" ClientInstanceName="cLoadingPanelCRP" ContainerElementID="divSubmitButton"
            Modal="True">
        </dxe:ASPxLoadingPanel>
        
    </div>
    <asp:HiddenField ID="hdnProjectSelectInEntryModule" runat="server" />
    <asp:HiddenField ID="hdnProjectMandatory" runat="server" />
    <asp:HiddenField ID="hdnHierarchySelectInEntryModule" runat="server" />
    <asp:HiddenField ID="ProjectForBranch" runat="server" />
    <asp:HiddenField ID="hdnBranchReqApprovalToBranch" runat="server" />
    <asp:HiddenField ID="hdnNoninventoryItemBranchReqBTOBTI" runat="server" />
    <asp:HiddenField ID="hdnEditID" runat="server" />


    <asp:HiddenField ID="hdnLockFromDate" runat="server" />
    <asp:HiddenField ID="hdnLockToDate" runat="server" />
    <asp:HiddenField ID="hdnLockFromDateCon" runat="server" />
    <asp:HiddenField ID="hdnLockToDateCon" runat="server" />

    <asp:HiddenField ID="hdnLockFromDateedit" runat="server" />
    <asp:HiddenField ID="hdnLockToDateedit" runat="server" />

    <asp:HiddenField ID="hdnLockFromDatedelete" runat="server" />
    <asp:HiddenField ID="hdnLockToDatedelete" runat="server" />

    <%--Mantis Issue 25087--%>
    <asp:HiddenField ID="hdnSettings" runat="server" />
    <asp:HiddenField ID="hdnIsBranchRequisition" runat="server" />
    <asp:HiddenField ID="hdnBranchRequisitionId" runat="server" />
    <%--End of Mantis Issue 25087--%>

    <%--Rev work start 28.07.2022 mantise no:0025074: A setting required "Is Online Printing Require For Branch Requisition ?"--%>
    <asp:HiddenField ID="hdnPrintingBranchRequisition" runat="server" />
    <%--Rev work close 28.07.2022 mantise no:0025074: A setting required "Is Online Printing Require For Branch Requisition ?--%>
    <%-- REV 1.0--%>
    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
    <PanelCollection>
    <dxe:PanelContent runat="server">           
    </dxe:PanelContent>
    </PanelCollection>
    <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>
    <asp:HiddenField ID="hFilterType" runat="server" />
    <%--END REV 1.0--%>

</asp:Content>
