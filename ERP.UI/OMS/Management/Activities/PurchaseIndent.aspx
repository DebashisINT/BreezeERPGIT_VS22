<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                13-04-2023        2.0.37           Pallab              25820: Purchase Indent/Requisition page design modification
2.0                18-05-2023        2.0.38           Pallab              26166: The Product Name and Description is too small in the Grid of Purchase Indent Module when the Screen Resolution is 1366X768
3.0                12-06-2023        2.0.38           Pallab              26325: Add Purchase Indent/Requisition grid columns visibility issue fix
4.0                11-07-2023        2.0.39           Priti               0026549: A setting is required to enter the backdated entries in Purchase Indent
                                                                          Mantis : 26871
5.0                02-01-2024       V2.0.42           Priti               Mantis : 0027050 A settings is required for the Duplicates Items Allowed or not in the Transaction Module.
6.0                23-01-2024       V2.0.43           Priti               0026947: "Clear Filter" is required in landing page of  Entry screens.

====================================================== Revision History =============================================--%>

<%@ Page Title="Purchase Indent/Requisition" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="PurchaseIndent.aspx.cs" Inherits="ERP.OMS.Management.Activities.PurchaseIndent" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/UOMConversion.ascx" TagPrefix="uc3" TagName="UOMConversionControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="https://cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css" rel="stylesheet" />
<script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
<script src="https://cdn.datatables.net/fixedcolumns/3.3.0/js/dataTables.fixedColumns.min.js"></script>
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
   <%-- <script src="JS/SearchPopup.js"></script>--%>
     <script src="JS/SearchPopupDatatable.js"></script>
    <%--Use for set focus on UOM after press ok on UOM--%>

    <script src="JS/PurchaseIndent.js?v=5.4"></script>
    <script>
        function PageLoad() {
            FinYearCheckOnPageLoad();
        }
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
        function OnEndCallback(s, e) {
            if (InsgridBatch.cpAddNewRow != null && InsgridBatch.cpAddNewRow != "") {
                InsgridBatch.cpAddNewRow = null;
                AddNewRow();
            }
            if (InsgridBatch.cpRtnMsg != null && InsgridBatch.cpRtnMsg != "") {
                InsgridBatch.cpRtnMsg = null;
                jAlert('Can not Tagged Duplicate Product in the Service Template.');

            }
            if (InsgridBatch.cpBtnVisible != null && InsgridBatch.cpBtnVisible != "") {
                InsgridBatch.cpBtnVisible = null;
                BtnVisible();
            }
            if (InsgridBatch.cpEdit != null) {
                var Indent_RequisitionNumber = InsgridBatch.cpEdit.split('~')[0];
                var Indent_RequisitionDate = InsgridBatch.cpEdit.split('~')[1];
                var Indent_BranchIdFor = InsgridBatch.cpEdit.split('~')[2];
                var Indent_Purpose = InsgridBatch.cpEdit.split('~')[3];
                var Indent_CurrencyId = InsgridBatch.cpEdit.split('~')[4];
                var Indent_ExchangeRtae = InsgridBatch.cpEdit.split('~')[5];
                var Indent_ID = InsgridBatch.cpEdit.split('~')[6];
                var Indent_ProjID = InsgridBatch.cpEdit.split('~')[7];
                var RevisionNo = InsgridBatch.cpEdit.split('~')[8];
                var ApproveProjectRem = InsgridBatch.cpEdit.split('~')[9];
                var ForBranch = InsgridBatch.cpEdit.split('~')[10];
                var ContactNos = InsgridBatch.cpEdit.split('~')[11];
                var ContactID = InsgridBatch.cpEdit.split('~')[12];
                var ContactDetailsID = InsgridBatch.cpEdit.split('~')[13];
                var TemplateDetailsID = InsgridBatch.cpEdit.split('~')[14];

                var ForDate = InsgridBatch.cpEdit.split('~')[15];
                var ToDate = InsgridBatch.cpEdit.split('~')[16];

                var ComponentIDs = InsgridBatch.cpEdit.split('~')[17];
                var ComponentDetailsIDs = InsgridBatch.cpEdit.split('~')[18];
                // Mantis Issue 25235
                var Indent_VendorId = InsgridBatch.cpEdit.split('~')[19];
                var Indent_VendorName = InsgridBatch.cpEdit.split('~')[20];
                // End of Mantis Issue 25235

                $("#hdnComponent").val(ComponentIDs);
                $("#hdnComponentDetailsIDs").val(ComponentDetailsIDs);
                

                var ForDatedt = new Date(ForDate);
              //  cdtFromDate.SetDate(ForDatedt);

                var ToDatedt = new Date(ToDate);
               // cdtToDate.SetDate(ToDatedt);

               // ctaggingGrid.SelectItemsByKey(ContactID);
              //  cgridTemplateproducts.SelectItemsByKey(ContactDetailsID);
               // cgridTemplateproducts.SelectItemsByKey(TemplateDetailsID);

                ctxtAppRejRemarks.SetText(ApproveProjectRem);
                ctxtRevisionNo.SetText(RevisionNo);
                ctaggingList.SetText(ContactNos);
                if (ContactNos != "")
                {
                    
                    var radio = $("[id*=rdl_Salesquotation] input[value=MRP]");
                    radio.attr("checked", "checked");
                    ctaggingList.SetEnabled(false);
                }
                //Mantis Issue 24912
                //$('#txtVoucherNo').val(Indent_RequisitionNumber);
                // Mantis Issue 25070
                //if ($("#hdnPageStatus").val() == "update") {
                //if ($("#hdnPageStatus").val() != "Copy") {
                if ($("#hdnPageStatus").val() != "Copy") {
                    // End of Mantis Issue 25070
                    // End of Mantis Issue 25070
                    $('#txtVoucherNo').val(Indent_RequisitionNumber);
                }
                else {
                    $('#txtVoucherNo').val("");
                }
                //End of Mantis Issue 24912
                document.getElementById('Keyval_internalId').value = "PurchaseIndent" + Indent_ID;

                var Transdt = new Date(Indent_RequisitionDate);
                ctDate.SetDate(Transdt);

                $("#txtVoucherNo").attr("disabled", "disabled");
                $("#ddlBranch").attr("disabled", "disabled");
                document.getElementById('hdnEditIndentID').value = Indent_ID;
                ctxtMemoPurpose.SetValue(Indent_Purpose);
                cCmbCurrency.SetValue(Indent_CurrencyId);
                document.getElementById('ddlBranch').value = Indent_BranchIdFor;
                ctxtRate.SetValue(Indent_ExchangeRtae);
                // Mantis Issue 25235
                ctxtVendorName.SetText(Indent_VendorName);
                GetObjectID('hdnCustomerId').value = Indent_VendorId;
                // End of Mantis Issue 25235
                //clookup_Project.gridView.Refresh();
                document.getElementById('ddlForBranch').value = ForBranch;

                InsgridBatch.batchEditApi.StartEdit(-1, 1);
                if ($('#hdnEditClick').val() == 'T') {
                    InsgridBatch.AddNewRow();
                    var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                    var tbQuotation = InsgridBatch.GetEditor("SrlNo");
                    tbQuotation.SetValue(noofvisiblerows);
                    $('#hdnEditClick').val("");
                }
                //Sandip Section For Approval Detail Start
                if (InsgridBatch.cpApproval != null) {
                    if (InsgridBatch.cpApproval == 'A') {
                        $('#lbl_quotestatusmsg').css('display', 'inline-block');
                        $('#lbl_quotestatusmsg').text('Document already approved.');
                        $('#btnnew').css('display', 'none');
                        $('#btnSaveExit').css('display', 'none');
                    }
                    else if (InsgridBatch.cpApproval == 'R') {
                        $('#lbl_quotestatusmsg').css('display', 'inline-block');
                        $('#lbl_quotestatusmsg').text('Document already rejected.');
                        $('#btnnew').css('display', 'none');
                        $('#btnSaveExit').css('display', 'none');
                    }
                    else {
                        $('#lbl_quotestatusmsg').css('display', 'none');
                        $('#btnnew').css('display', 'block');
                        $('#btnSaveExit').css('display', 'block');
                    }
                }
                //Sandip Section For Approval Detail End

                clookup_Project.gridView.Refresh();
                if ($("#hdnProjectSelectInEntryModule").val() == "1")
                    clookup_Project.gridView.SelectItemsByKey(Indent_ProjID);

                InsgridBatch.cpEdit = null;

            }
            if (InsgridBatch.cpSaveSuccessOrFail == "nullQuantity") {
                // Rev Mantis Issue 24428/24429
                InsgridBatch.batchEditApi.StartEdit(0, 2);
               
               // AddNewRow();
                // End of Rev Mantis Issue 24428/24429
                InsgridBatch.cpSaveSuccessOrFail = null;
                jAlert('Cannot save. Entered quantity must be greater then ZERO(0).');
                cLoadingPanelCRP.Hide();
            }
            else if (InsgridBatch.cpSaveSuccessOrFail == "duplicateProduct") {
                AddNewRow();
                InsgridBatch.cpSaveSuccessOrFail = null;
                jAlert('Can not Add Duplicate Product in the Purchase Indent.');
                InsgridBatch.cpSaveSuccessOrFail = '';
                cLoadingPanelCRP.Hide();
            }
            else if (InsgridBatch.cpSaveSuccessOrFail == "ExceedQuantity") {
                AddNewRow();
                InsgridBatch.cpSaveSuccessOrFail = null;
                jAlert('Tagged product quantity can not reduce.Update The quantity and Try Again.');
                InsgridBatch.cpSaveSuccessOrFail = '';
                cLoadingPanelCRP.Hide();
            }
            else if (InsgridBatch.cpSaveSuccessOrFail == "outrange") {
                InsgridBatch.batchEditApi.StartEdit(0, 2);
                InsgridBatch.cpSaveSuccessOrFail = null;
                jAlert('Can Not Add More Purchase Indent Number as Purchase Indent Scheme Exausted.<br />Update The Scheme and Try Again');
                cLoadingPanelCRP.Hide();
            }
            else if (InsgridBatch.cpSaveSuccessOrFail == "duplicate") {
                InsgridBatch.batchEditApi.StartEdit(0, 2);
                InsgridBatch.cpSaveSuccessOrFail = null;
                jAlert('Can Not Save as Duplicate Purchase Indent Number.');
                cLoadingPanelCRP.Hide();
            }

            else if (InsgridBatch.cpSaveSuccessOrFail == "checkMultiUOMData") {
                //debugger;
                InsgridBatch.batchEditApi.StartEdit(0, 2);
                var SrlNo = InsgridBatch.cpcheckMultiUOMData;
                var msg = "Please add Alt. Qty for SL No. " + SrlNo;
                InsgridBatch.cpcheckMultiUOMData = null;
                jAlert(msg);
                InsgridBatch.cpSaveSuccessOrFail = null;

                cLoadingPanelCRP.Hide();
            }


            else if (InsgridBatch.cpSaveSuccessOrFail == "errorInsert") {
                InsgridBatch.batchEditApi.StartEdit(0, 2);
                jAlert('Please try after sometime.');
                InsgridBatch.cpSaveSuccessOrFail = null;
            }
            else if (InsgridBatch.cpSaveSuccessOrFail == "EmptyProject") {
                InsgridBatch.batchEditApi.StartEdit(0, 2);
                jAlert('Please select project.');
            }
            else if (InsgridBatch.cpSaveSuccessOrFail == "transactionbeingused") {
                InsgridBatch.cpSaveSuccessOrFail = null;
                jAlert('Transaction exist. cannot be processed.');
                cLoadingPanelCRP.Hide();
            }
            else {
                if (InsgridBatch.cpVouvherNo != null) {
                    var JV_Number = InsgridBatch.cpVouvherNo;
                    var value = document.getElementById('hdnRefreshType').value;
                    var JV_Msg = "Purchase Indent Requisition No. " + JV_Number + " generated.";
                    var strSchemaType = document.getElementById('hdnSchemaType').value;

                    if (value == "E") {
                        if (JV_Number != "") {
                            if (strSchemaType == '1') {
                                jAlert(JV_Msg, 'Alert Dialog: [PurchaseIndent]', function (r) {
                                    if (r == true) {
                                        InsgridBatch.cpVouvherNo = null;
                                        window.location.assign("PurchaseIndent.aspx");
                                    }
                                });
                            }
                            else {
                                window.location.assign("PurchaseIndent.aspx");
                            }
                        }
                        else {
                            window.location.assign("PurchaseIndent.aspx");
                        }
                    }
                    else if (value == "S") {
                        if (JV_Number != "") {
                            if (strSchemaType == '1') {
                                jAlert(JV_Msg, 'Alert Dialog: [PurchaseIndent]', function (r) {
                                    if (r == true) {
                                        InsgridBatch.cpVouvherNo = null;
                                    }
                                });
                            }
                        }
                    }
                }
                if ($('#hdnSaveNew').val() == "Save_Exit") {

                    if (InsgridBatch.cpExitNew == "YES") {
                <%--Code Added By Sandip For Approval Detail Section Start--%>
                        if (InsgridBatch.cpApproverStatus == "approve") {
                            window.parent.popup.Hide();
                            window.parent.cgridPendingApproval.PerformCallback();
                        }
                <%--Code Above Added By Sandip For Approval Detail Section End--%>
                        deleteAllRows();
                    }

                }
                if ($('#hdnSaveNew').val() == "Save_New") {
                    ctxtMemoPurpose.SetValue("");
                    $("#divNumberingScheme").show();
                    var Campany_ID = '<%=Session["LastCompany"]%>';
                    var LocalCurrency = '<%=Session["LocalCurrency"]%>';
                    var basedCurrency = LocalCurrency.split("~");
                    cCmbCurrency.SetValue(basedCurrency[0]);
                    ctxtRate.SetValue("");
                    ctxtRate.SetEnabled(false);
                    $('#lblHeading').text("");
                    $('#lblHeading').text("Add Purchase Indent/Requisition");
                    // Mantis Issue 25235
                    ctxtVendorName.SetText("");
                    GetObjectID('hdnCustomerId').value = "";
                    // End of Mantis Issue 25235
                    deleteAllRows();
                    InsgridBatch.AddNewRow();
                    var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                    var tbQuotation = InsgridBatch.GetEditor("SrlNo");
                    tbQuotation.SetValue(noofvisiblerows);
                    $('#hdn_Mode').val('Entry');
                    if (document.getElementById('txtVoucherNo').value == "Auto") {
                        document.getElementById('txtVoucherNo').value = "Auto";
                    }
                    else {
                        document.getElementById('txtVoucherNo').value = "";
                        $('#txtVoucherNo').focus();
                    }

                    $('#txtMemoPurpose_I').focus();
                    cLoadingPanelCRP.Hide();
                }
            }
    if (InsgridBatch.cpView == "1") {
        viewOnly();
    }
        }

        // Mantis Issue 25394
        function CallbackPanelEndCall(s, e) {
            CgvPurchaseIndent.Refresh();
        }
        // End of Mantis Issue 25394

function ch_fnApproved() {
}
//Rev Debashis
function zoompurchaseindent(keyid, docno) {
    document.getElementById("divfromTo").style.display = 'none';
    $('#<%=hdnEditClick.ClientID %>').val('T'); //Edit
    $('#<%= lblHeading.ClientID %>').text("View Purchase Indent/Requisition");
    document.getElementById('DivEntry').style.display = 'block';
    document.getElementById('DivEdit').style.display = 'none';
    document.getElementById('btnAddNew').style.display = 'none';
    $('#<%=hdn_Mode.ClientID %>').val('View');
    InsgridBatch.PerformCallback("View~" + keyid);
    chkAccount = 1;
    document.getElementById('divNumberingScheme').style.display = 'none';
}
//End of Rev Debashis
function ProductsComboGotFocusChange(s, e) {
    var tbDescription = InsgridBatch.GetEditor("gvColDiscription");
    var tbUOM = InsgridBatch.GetEditor("gvColUOM");
    var tdRate = InsgridBatch.GetEditor("gvColRate");
    var AvailableStock = InsgridBatch.GetEditor("gvColAvailableStock");
    var ProductID = (InsgridBatch.GetEditor('gvColProduct').GetText() != null) ? InsgridBatch.GetEditor('gvColProduct').GetText() : "0";

    // var ProductID = s.GetValue();
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
                        // document.getElementById("B_AvailableStock").Value = data;
                        var AvailableStock = data + " " + strUOM;
                        $('#B_AvailableStock').text(AvailableStock);

                    }
                });
            }
        }
        function ProductsCombo_SelectedIndexChanged(s, e) {
            var tbDescription = InsgridBatch.GetEditor("gvColDiscription");
            var tbUOM = InsgridBatch.GetEditor("gvColUOM");
            var tdRate = InsgridBatch.GetEditor("gvColRate");

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
                url: 'PurchaseIndent.aspx/getAvilableStock',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ Campany_ID: Campany_ID, ProductID: strProductID, LastFinYear: LastFinYear, BranchFor: BranchFor }),
                success: function (msg) {
                    var data = msg.d;
                    document.getElementById("pageheaderContent").style.display = 'block';
                    var AvailableStock = data + " " + strUOM;
                    // document.getElementById("B_AvailableStock").Value = data;
                    $('#B_AvailableStock').text(AvailableStock);

                }
            });

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
                    url: "PurchaseIndent.aspx/GetRate",
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
        function SetProduct(Id, Name) {
            $('#ProductModel').modal('hide');
            var LookUpData = Id;
            var ProductCode = Name;
            if (!ProductCode) {
                LookUpData = null;
            }
            InsgridBatch.batchEditApi.StartEdit(globalRowIndex);
            InsgridBatch.GetEditor("gvColProduct").SetText(LookUpData);
            InsgridBatch.GetEditor("ProductName").SetText(ProductCode);
            var tbDescription = InsgridBatch.GetEditor("gvColDiscription");
            var tbUOM = InsgridBatch.GetEditor("gvColUOM");
            var tdRate = InsgridBatch.GetEditor("gvColRate");
            var ProductID = LookUpData;
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
            $('#<%= lblStkQty.ClientID %>').text("0.00");
            $('#<%= lblStkUOM.ClientID %>').text(strUOM);
            preColumn = "Product";
            cacpAvailableStock.PerformCallback(strProductID);
            InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 5);
        }



        //Rev Bapi
        $(document).ready(function () {

            $("#UOMQuantity").on('blur', function () {
                var currentObj = $(this);
                var currentVal = currentObj.val();
                if (!isNaN(currentVal)) {
                    var updatedVal = parseFloat(currentVal).toFixed(4);
                    currentObj.val(updatedVal);
                }
                else {
                    currentObj.val("");
                }
            })


        })
        //End Rev Bapi
    </script>


    <script>
        //Mantis Issue 25053
        //$(document).ready(function () {
        //    if ($('#hdnSettings').val() == "Yes" && $('#Keyval_internalId').val() == "Add") {
        //        $("#divIsDirector").removeClass('hide');
        //        //$("#onSmsClickJv").removeClass('hide');
        //    }
        //    else {
        //        $("#divIsDirector").addClass('hide');
        //        //$("#onSmsClickJv").addClass('hide');
        //    }
        //})
        function chkDirectorApprovalRequired_change() {
            var dropdownli = ""
            if ($('#chkDirectorApprovalRequired').is(':checked') == true) {
                $("#divEmployee, #divEmployeeIn").removeClass('hide');
            }
            else {
                $("#divEmployee, #divEmployeeIn").addClass('hide');
            }
        }
        //$(document).ready(function () {
            
        //})
        function BindModalEmployee() {
            var det = {};
            det.DBName = $('#hdDbName').val();
            var $select = $('#ddl_DirEmployee');
            $select.empty();
            //$select.append("<option value='0'>--Select--</option>");
            $.ajax({
                type: "POST",
                url: 'PurchaseIndent.aspx/AddModalEmployee',
                contentType: "application/json; charset=utf-8",
                async: false,
                dataType: "json",
                data: JSON.stringify(det),
                success: function (data) {
                    var arr = data.d;
                    console.log(arr)

                    var htm ='';
                    for (var i = 0; i < arr.length; i++) {
                        htm += '<option value="' + arr[i].cnt_internalId + '">' + arr[i].DirectorName + '</option>'
                    }
                    $('#ddl_DirEmployee').html(htm)
                    //$('<option>', {
                    //    value: data.d.cnt_internalId
                    //}).append(data.d.DirectorName).appendTo($select);
                    //alert(data.d[0].cnt_internalId)
                    //$.each(data.d, function (i, data) {
                    //    alert(data.d[i].cnt_internalId)
                    //    $('<option>', {
                    //        value: data.d[i].cnt_internalId
                    //    }).append(data.d[i].DirectorName).appendTo($select);
                    //});

                },
                error: function (mydata) { alert("error"); alert(mydata); },

            });
        }
        //End of Mantis Issue 25053
    </script>
    <link href="CSS/purchaseindent.css?v=1.0.0.21" rel="stylesheet" />
    <style>
        #gridBatch_DXMainTable > tbody > tr > td:last-child {
            display: none !important;
        }
    </style>
    <%-- Project Hierarchy End --%>


    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 0;
        }

        .simple-select::after
        {
            top: 6px;
            right: -2px;
        }

        #GrdSalesReturn {
            max-width: 98% !important;
        }
        #FormDate, #toDate, #dtTDate, #dt_PLQuote, #dt_PlQuoteExpiry {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        select
        {
            -webkit-appearance: auto;
        }

        .calendar-icon
        {
                right: 20px;
                bottom: 8px;
        }
        .padTabtype2 > tbody > tr > td
        {
            vertical-align: bottom;
        }

        input[disabled], select[disabled]
        {
            background: #eee;
        }

        .lblmTop8>span, .lblmTop8>label
        {
            margin-top: 0px !important;
        }
    </style>
    <%--Rev end 1.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left"><span class="">
                <asp:Label ID="lblHeading" runat="server" Text="Purchase Indent/Requisition"></asp:Label></span>

            </h3>
            <div id="pageheaderContent" class="pull-right wrapHolder reverse content horizontal-images" style="display: none; width: 617px;">
                <div class="Top clearfix">
                    <ul>
                        <li>
                            <div class="lblHolder">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>Available Stock </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="width: 100%;">
                                                    <%-- <b style="text-align: left" id="B_ImgSymbolBankBal" runat="server"></b>--%>
                                                    <b style="text-align: center" id="B_AvailableStock" runat="server">0.0</b>
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
                <table class="padTabtype2 pull-right  brnchreq" style="margin-top: 7px">
                    <tr>
                        <td>
                            <label>From Date</label></td>
                        <%--Rev 1.0: "for-cust-icon" class add --%>
                        <td class="for-cust-icon">
                            <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxDateEdit>
                            <%--Rev 1.0--%>
                            <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                            <%--Rev end 1.0--%>
                        </td>
                        <td>
                            <label>To Date</label>
                        </td>
                        <%--Rev 1.0: "for-cust-icon" class add --%>
                        <td class="for-cust-icon">
                            <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxDateEdit>
                            <%--Rev 1.0--%>
                            <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                            <%--Rev end 1.0--%>
                        </td>
                        <td>
                            <label>Unit</label></td>
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
            <%--Sandip Section for Show Hide Cross Button based on Condition Either for Normal Add/Edit or Approval Section--%>
            <div id="PODetailsCross" runat="server" class="crossBtn" style="display: none"><a href=""><i class="fa fa-times"></i></a></div>
            <div id="ApprovalCross" runat="server" class="crossBtn"><a><i class="fa fa-times"></i></a></div>
            <div id="btncross" runat="server" class="crossBtn" style="display: none; margin-left: 50px;"><a href="PurchaseIndent.aspx"><i class="fa fa-times"></i></a></div>
            <%--Sandip Section for Show Hide Cross Button based on Condition Either for Normal Add/Edit or Approval Section--%>
        </div>

    </div>
        <div class="form_main">
        <div class="clearfix mb-10" id="btnAddNew">
            <div style="float: left; padding-right: 5px;">
                <% if (rights.CanAdd)
                   { %>
                <a href="javascript:void(0);" onclick="AddButtonClick()" class="btn btn-success">
                    <span class="btn-icon"><i class="fa fa-plus"></i></span><span><u>I</u>ndent/Requisition</span> </a>
                <% } %>
                <% if (rights.CanExport)
                   { %>
                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="drdExport_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">PDF</asp:ListItem>
                    <asp:ListItem Value="2">XLS</asp:ListItem>
                    <asp:ListItem Value="3">RTF</asp:ListItem>
                    <asp:ListItem Value="4">CSV</asp:ListItem>
                </asp:DropDownList>
                <% } %>
                 <%--REV 6.0--%>
                 <dxe:ASPxButton ID="btnClearFilter" runat="server" Text="Clear Filter"  UseSubmitBehavior="false" CssClass="btn btn-primary btn-radius" AutoPostBack="False">
                 <ClientSideEvents Click="function(s, e) {
                 ASPxClientUtils.DeleteCookie('PurchaseIndentCookies');
                 location.reload(true);
                 }" />
                 </dxe:ASPxButton>
                <%--REV 6.0 END--%>
                <%--Sandip Section for Approval Section in Design Start --%>

                <span id="spanStatus" runat="server">
                    <a href="javascript:void(0);" onclick="OpenPopUPUserWiseQuotaion()" class="btn btn-primary">
                        <span>My Purchase Indent Status</span>
                        <%--<asp:Label ID="Label1" runat="server" Text=""></asp:Label>--%>                   
                    </a>
                </span>
                <span id="divPendingWaiting" runat="server">
                    <a href="javascript:void(0);" onclick="OpenPopUPApprovalStatus()" class="btn btn-primary">
                        <span>Approval Waiting</span>
                        <asp:Label ID="lblWaiting" runat="server" Text=""></asp:Label>
                    </a>
                    <i class="fa fa-reply blink" style="font-size: 20px; margin-right: 10px;" aria-hidden="true"></i>

                </span>

                <%--Sandip Section for Approval Section in Design End --%>
            </div>

        </div>
        <div id="DivEntry" style="display: none">
            <div style=" padding: 8px 0; margin-bottom: 15px; border-radius: 4px;" class="clearfix">
                <div class="col-md-3" id="divNumberingScheme">
                    <label style="">Numbering Scheme</label>
                    <div>
                        <dxe:ASPxComboBox ID="CmbScheme" EnableIncrementalFiltering="true" ClientInstanceName="cCmbScheme"
                            TextField="SchemaName" ValueField="ID" IncrementalFilteringMode="Contains"
                            runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                            <ClientSideEvents ValueChanged="function(s,e){CmbScheme_ValueChange()}" GotFocus="function(s,e){cCmbScheme.ShowDropDown();}" LostFocus="CmbScheme_LostFocus"></ClientSideEvents>
                        </dxe:ASPxComboBox>
                        <%--<span id="MandatoryNumberingScheme" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>--%>
                    </div>
                </div>
                <div class="col-md-3">
                    <label>Document No.<span style="color: red;">*</span></label>
                    <div>
                        <asp:TextBox ID="txtVoucherNo" runat="server" Width="100%" MaxLength="50" onchange="txtBillNo_TextChanged()">                             
                        </asp:TextBox>
                        <%-- <asp:TextBox ID="txtIndentId" runat="server" >                             
                        </asp:TextBox>--%>
                        <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                    </div>
                </div>
                <div class="col-md-3">
                    <label>Posting Date<span style="color: red;">*</span></label>
                    <div>
                        <dxe:ASPxDateEdit ID="tDate" runat="server" EditFormat="Custom" ClientInstanceName="ctDate" DisplayFormatString="dd-MM-yyyy"
                            UseMaskBehavior="True" Width="100%" EditFormatString="dd-MM-yyyy">
                            <%--  <ClientSideEvents Init="function(s,e){ s.SetDate(new Date());}" />--%>
                            <ClientSideEvents DateChanged="function(s,e){TDateChange();}" GotFocus="function(s,e){ctDate.ShowDropDown();}"></ClientSideEvents>
                            <ValidationSettings RequiredField-IsRequired="true" ErrorFrameStyle-CssClass="absolute"></ValidationSettings>
                        </dxe:ASPxDateEdit>
                        
                    </div>
                </div>
                
                <div class="col-md-3 ">

                    <label>Unit</label>
                    <%--Rev 1.0 : "simple-select" class add--%>
                    <div class="simple-select">
                        <asp:DropDownList ID="ddlBranch" runat="server" DataSourceID="dsBranch" onchange="ddlBranch_SelectedIndexChanged()"
                            DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%">
                        </asp:DropDownList>
                    </div>
                </div>
                <div style="clear: both"></div>
                <%--Rev 1.0 : "simple-select" class add--%>
                <div class="col-md-3 simple-select" id="DivForUnit" runat="server">

                    <label>For Unit</label>
                    <div>
                        <asp:DropDownList ID="ddlForBranch" runat="server" DataSourceID="dsBranch"
                            DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%">
                        </asp:DropDownList>
                    </div>
                </div>
                <%--Mantis Issue 25235  [class changed from "col-md-9" to "col-md-6"] --%>
                <div class="col-md-6">
                    <label style="margin-bottom: 5px; display: inline-block">Purpose</label>
                    <div>
                        <dxe:ASPxMemo ID="txtMemoPurpose" ClientInstanceName="ctxtMemoPurpose" runat="server" Height="50px" Width="100%" Font-Names="Arial"></dxe:ASPxMemo>
                    </div>
                </div>
                <%--Mantis Issue 25235--%>
                <div class="col-md-3 lblmTop8" id="DivForVendor" runat="server">
                    <dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Vendor">
                    </dxe:ASPxLabel>
                    <%--<span style="color: red;">*</span>--%>
                    <a href="#" onclick="AddVendorClick()" style="left: -12px; top: 20px; font-size: 16px;"><i id="openlink" runat="server" class="fa fa-plus-circle" aria-hidden="true"></i></a>
                    <dxe:ASPxButtonEdit ID="txtVendorName" runat="server" ReadOnly="true" ClientInstanceName="ctxtVendorName" Width="100%">
                        <Buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>

                        </Buttons>
                        <ClientSideEvents ButtonClick="function(s,e){VendorButnClick();}" KeyDown="function(s,e){VendorKeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>
                </div>
                <%--End of Mantis Issue 25235--%>
                <div class="col-md-2">
                    <label id="lblProject" runat="server">Project</label>
                    <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="ProjectServerModeDataSource"
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
                        <ClientSideEvents GotFocus="function(s,e){clookup_Project.ShowDropDown();}" LostFocus="clookupProjectLostFocus" ValueChanged="ProjectEndCallback" />
                        <ClearButton DisplayMode="Always">
                        </ClearButton>
                    </dxe:ASPxGridLookup>
                    <dx:LinqServerModeDataSource ID="ProjectServerModeDataSource" runat="server" OnSelecting="ProjectServerModeDataSource_Selecting"
                        ContextTypeName="ERPDataClassesDataContext" TableName="PurchaseIndentList" />
                </div>
                <div style="clear: both"></div>

                <div class="col-md-2 lblmTop8" id="dvRevision" style="display: none" runat="server">
                    <label>
                        <dxe:ASPxLabel ID="lblRevisionNo" runat="server" Text="Revision No." Width="120px" CssClass="inline">
                        </dxe:ASPxLabel>
                    </label>
                    <dxe:ASPxTextBox ID="txtRevisionNo" runat="server" Width="100%" MaxLength="50" ClientInstanceName="ctxtRevisionNo">
                        <%-- <ClientSideEvents LostFocus="Revision_LostFocus" />--%>
                    </dxe:ASPxTextBox>
                </div>
                <div class="col-md-2 lblmTop8" id="dvRevisionDate" style="display: none" runat="server">
                    <label>
                        <dxe:ASPxLabel ID="lblRevisionDate" runat="server" Text="Revision Date" Width="120px" CssClass="inline">
                        </dxe:ASPxLabel>
                    </label>
                    <dxe:ASPxDateEdit ID="txtRevisionDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" ClientInstanceName="ctxtRevisionDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                        <ClientSideEvents GotFocus="function(s,e){ctxtRevisionDate.ShowDropDown();}" />
                    </dxe:ASPxDateEdit>
                </div>

                <div class="col-md-6" id="dvAppRejRemarks" style="display: none" runat="server">
                    <asp:Label ID="lblAppRejRemarks" runat="server" Text="Approve/Reject Remarks"></asp:Label>
                    <dxe:ASPxMemo ID="txtAppRejRemarks" ClientInstanceName="ctxtAppRejRemarks" runat="server" Height="50px" Width="100%" Font-Names="Arial"></dxe:ASPxMemo>

                    <%--     <asp:Textbox ID="txtAppRejRemarks" runat="server" TabIndex="16" Width="100%" TextMode="MultiLine" Rows="2" Columns="8" Height="50px"></asp:Textbox>	--%>
                </div>

                <div class="col-md-4">
                    <label id="lblHierarchy" runat="server">Hierarchy </label>
                    <div>
                        <asp:DropDownList ID="ddlHierarchy" runat="server" DataSourceID="dsHierarchy"
                            DataTextField="HIERARCHY_NAME" DataValueField="Hierarchy_ID" Width="100%">
                        </asp:DropDownList>
                    </div>
                </div>

                <div style="clear: both"></div>



                <div class="col-md-3 hide" id="DivFromDate" runat="server">
                    <label>From Date</label>
                    <div>
                        <dxe:ASPxDateEdit ID="dtFromDate" runat="server" EditFormat="Custom" ClientInstanceName="cdtFromDate" DisplayFormatString="dd-MM-yyyy"
                            UseMaskBehavior="True" Width="100%" EditFormatString="dd-MM-yyyy">
                        </dxe:ASPxDateEdit>
                    </div>
                </div>
                <div class="col-md-3 hide" id="DivToDate" runat="server">
                    <label>To Date</label>
                    <div>
                        <dxe:ASPxDateEdit ID="dtToDate" runat="server" EditFormat="Custom" ClientInstanceName="cdtToDate" DisplayFormatString="dd-MM-yyyy"
                            UseMaskBehavior="True" Width="100%" EditFormatString="dd-MM-yyyy">
                        </dxe:ASPxDateEdit>
                    </div>
                </div>
                <div class="col-md-2 lblmTop8 " id="DivMRP" runat="server">
                    <asp:RadioButtonList ID="rdl_Salesquotation" runat="server" RepeatDirection="Horizontal" onchange="return selectValueForRadioBtn();" Width="85%">
                        <asp:ListItem Text="MRP" Value="MRP"></asp:ListItem>
                    </asp:RadioButtonList>

                    <dxe:ASPxButtonEdit ID="taggingList" ClientInstanceName="ctaggingList" runat="server" ReadOnly="true" Width="100%">
                        <Buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>
                        </Buttons>
                        <ClientSideEvents ButtonClick="taggingListButnClick" KeyDown="taggingListKeyDown" />
                    </dxe:ASPxButtonEdit>
                    <dxe:ASPxPopupControl ID="popup_taggingGrid" runat="server" ClientInstanceName="cpopup_taggingGrid"
                        HeaderText="Select Documents" PopupHorizontalAlign="WindowCenter"
                        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" Height="400px" Width="850px"
                        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                        ContentStyle-CssClass="pad">
                        <ContentStyle VerticalAlign="Top" CssClass="pad">
                        </ContentStyle>
                        <ContentCollection>
                            <dxe:PopupControlContentControl runat="server">
                                <div style="padding: 7px 0;">
                                    <input type="button" value="Select All Products" onclick="Tag_ChangeState('SelectAll')" class="btn btn-primary"></input>
                                    <input type="button" value="De-select All Products" onclick="Tag_ChangeState('UnSelectAll')" class="btn btn-primary"></input>
                                    <input type="button" value="Revert" onclick="Tag_ChangeState('Revart')" class="btn btn-primary"></input>
                                </div>
                                <div>
                                    <dxe:ASPxGridView ID="taggingGrid" ClientInstanceName="ctaggingGrid" runat="server" KeyFieldName="MRP_ID"
                                        Width="100%" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords"
                                        Settings-ShowFooter="false" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible"
                                        OnCustomCallback="taggingGrid_CustomCallback" OnDataBinding="taggingGrid_DataBinding">
                                        <SettingsBehavior AllowDragDrop="False"></SettingsBehavior>
                                        <SettingsPager Visible="false"></SettingsPager>
                                        <Columns>
                                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="40" Caption=" " VisibleIndex="0" />
                                            <dxe:GridViewDataTextColumn FieldName="MRP_No" Caption="Document Number" Width="150" VisibleIndex="1">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="BRANCHNAME" Caption="Unit" Width="100" VisibleIndex="2">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="MRP_Date" Caption="Date" Width="150" VisibleIndex="3">
                                            </dxe:GridViewDataTextColumn>

                                        </Columns>
                                        <SettingsDataSecurity AllowEdit="true" />
                                        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                        <ClientSideEvents EndCallback="taggingGrid_EndCallback" />
                                    </dxe:ASPxGridView>
                                </div>
                                <div class="text-center">
                                    <dxe:ASPxButton ID="btnTaggingSave" ClientInstanceName="cbtnTaggingSave" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                        <ClientSideEvents Click="function(s, e) {QuotationNumberChanged();}" />
                                    </dxe:ASPxButton>
                                </div>
                            </dxe:PopupControlContentControl>
                        </ContentCollection>
                        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                    </dxe:ASPxPopupControl>
                    <dxe:ASPxPopupControl ID="ASPxProductsPopup" runat="server" ClientInstanceName="cProductsPopup"
                        Width="900px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
                        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
                        <HeaderTemplate>
                            <strong><span style="color: #fff">Select Products</span></strong>
                            <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                                <ClientSideEvents Click="function(s, e){ 
                                                            cProductsPopup.Hide();
                                                        }" />
                            </dxe:ASPxImage>
                        </HeaderTemplate>
                        <ContentCollection>
                            <dxe:PopupControlContentControl runat="server">
                                <div style="padding: 7px 0;">
                                    <input type="button" value="Select All Products" onclick="ChangeState('SelectAll')" class="btn btn-primary"></input>
                                    <input type="button" value="De-select All Products" onclick="ChangeState('UnSelectAll')" class="btn btn-primary"></input>
                                    <input type="button" value="Revert" onclick="ChangeState('Revart')" class="btn btn-primary"></input>
                                </div>
                                <dxe:ASPxGridView runat="server" KeyFieldName="ComponentDetailsID" ClientInstanceName="cgridproducts" ID="grid_Products"
                                    Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords"
                                    OnCustomCallback="cgridProducts_CustomCallback" OnDataBinding="grid_Products_DataBinding"
                                    Settings-ShowFooter="false" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
                                    <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                                    <SettingsPager Visible="false"></SettingsPager>
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="40" Caption=" " VisibleIndex="0" />
                                        <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="SrlNo" Width="40" ReadOnly="true" Caption="Sl. No.">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="ComponentNumber" Width="120" ReadOnly="true" Caption="Number" Settings-AllowFilterBySearchPanel="True" Settings-AllowAutoFilter="True">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="ProductID" ReadOnly="true" Caption="Product" Width="0">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="ProductsName" ReadOnly="true" Width="100" Caption="Product Name">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="ProductDescription" Width="200" ReadOnly="true" Caption="Product Description">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity" Width="70" VisibleIndex="6">
                                            <PropertiesTextEdit>
                                                <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="ComponentID" ReadOnly="true" Caption="ComponentID" Width="0">
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                    <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                    <SettingsDataSecurity AllowEdit="true" />
                                     <ClientSideEvents EndCallback="gridProducts_EndCallback" />
                                </dxe:ASPxGridView>
                                <div class="text-center">
                                    <asp:Button ID="Button3" runat="server" Text="OK" CssClass="btn btn-primary  mLeft mTop" OnClientClick="return InventoryItemToGridBind();" UseSubmitBehavior="false" />
                                </div>
                            </dxe:PopupControlContentControl>
                        </ContentCollection>
                        <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
                        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                    </dxe:ASPxPopupControl>

                    <dxe:ASPxPopupControl ID="TemplateProductsPopup" runat="server" ClientInstanceName="cTemplateProductsPopup"
                        Width="900px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
                        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
                        <HeaderTemplate>
                            <strong><span style="color: #fff">Select Service Template Inventory Products</span></strong>
                            <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                                <ClientSideEvents Click="function(s, e){ 
                                                            cTemplateProductsPopup.Hide();
                                                        }" />
                            </dxe:ASPxImage>
                        </HeaderTemplate>
                        <ContentCollection>
                            <dxe:PopupControlContentControl runat="server">
                                <div style="padding: 7px 0;">
                                    <input type="button" value="Select All Products" onclick="TemplateChangeState('SelectAll')" class="btn btn-primary"></input>
                                    <input type="button" value="De-select All Products" onclick="TemplateChangeState('UnSelectAll')" class="btn btn-primary"></input>
                                    <input type="button" value="Revert" onclick="TemplateChangeState('Revart')" class="btn btn-primary"></input>
                                </div>
                                <dxe:ASPxGridView runat="server" KeyFieldName="ComponentDetailsID" ClientInstanceName="cgridTemplateproducts" ID="gridTemplateproducts"
                                    Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords"
                                    OnCustomCallback="gridTemplateproducts_CustomCallback" OnDataBinding="gridTemplateproducts_DataBinding"
                                    Settings-ShowFooter="false" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
                                    <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                                    <SettingsPager Visible="false"></SettingsPager>
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="40" Caption=" " VisibleIndex="0" />
                                        <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="SrlNo" Width="40" ReadOnly="true" Caption="Sl. No.">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="ComponentNumber" Width="120" ReadOnly="true" Caption="Number" Settings-AllowFilterBySearchPanel="True" Settings-AllowAutoFilter="True">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="ProductID" ReadOnly="true" Caption="Product" Width="0">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="ProductsName" ReadOnly="true" Width="100" Caption="Product Name">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="ProductDescription" Width="200" ReadOnly="true" Caption="Product Description">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity" Width="70" VisibleIndex="6">
                                            <PropertiesTextEdit>
                                                <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="ComponentID" ReadOnly="true" Caption="ComponentID" Width="0">
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                    <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                    <SettingsDataSecurity AllowEdit="true" />
                                    <ClientSideEvents EndCallback="gridTempProducts_EndCallback" />
                                </dxe:ASPxGridView>
                                <div class="text-center">
                                    <asp:Button ID="Button1" runat="server" Text="OK" CssClass="btn btn-primary  mLeft mTop" OnClientClick="return InventoryItemToGridBind();" UseSubmitBehavior="false" />
                                </div>
                            </dxe:PopupControlContentControl>
                        </ContentCollection>
                        <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
                        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                    </dxe:ASPxPopupControl>

                </div>
                <div style="clear: both"></div>

                <div>
                    <br />
                </div>
                <div style="display: none">
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
                <br />
            </div>
            <div>
                <div class="makeFullscreen ">
                    <span class="fullScreenTitle">Purchase Indent/Requisition</span>
                    <span class="makeFullscreen-icon half hovered " data-instance="InsgridBatch" title="Minimize Grid" id="expandEntryft67"><i class="fa fa-expand"></i></span>
                    <%--Rev 3.0: "Column width chsnge"--%>
                    <dxe:ASPxGridView runat="server" ClientInstanceName="InsgridBatch" ID="gridBatch" KeyFieldName="PurchaseIndentID"
                        OnBatchUpdate="gridBatch_BatchUpdate"
                        OnCellEditorInitialize="gridBatch_CellEditorInitialize"
                        OnDataBinding="gridBatch_DataBinding"
                        Width="100%" Settings-ShowFooter="false" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                        OnCustomCallback="gridBatch_CustomCallback"
                        OnRowInserting="Grid_RowInserting" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight="300" SettingsPager-Mode="ShowAllRecords"
                        OnRowUpdating="Grid_RowUpdating"
                        OnRowDeleting="Grid_RowDeleting">
                        <SettingsPager Visible="false"></SettingsPager>
                        <Columns>
                            <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="40" VisibleIndex="0" Caption="#" HeaderStyle-HorizontalAlign="Center">
                                <CustomButtons>
                                    <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                    </dxe:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                            </dxe:GridViewCommandColumn>
                            <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl" VisibleIndex="1" Width="30">
                                <PropertiesTextEdit>
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <%--<dxe:GridViewDataComboBoxColumn Caption="Product" FieldName="gvColProduct" VisibleIndex="2">
                            <PropertiesComboBox ValueField="ProductID" ClientInstanceName="ProductID" TextField="ProductName">
                                <ClientSideEvents SelectedIndexChanged="ProductsCombo_SelectedIndexChanged" GotFocus="ProductsComboGotFocusChange" />
                            </PropertiesComboBox>
                        </dxe:GridViewDataComboBoxColumn>--%>

                            <%--Batch Product Popup Start--%>
                            <%--Rev 2.0--%>
                            <%--<dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product Name" VisibleIndex="2">--%>
                            <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product Name" VisibleIndex="2" Width="90">
                            <%--Rev end 2.0--%>
                                <PropertiesButtonEdit>
                                    <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" LostFocus="ProductsGotFocus" GotFocus="ProductsGotFocusFromID" />
                                    <Buttons>
                                        <dxe:EditButton Text="..." Width="20px">
                                        </dxe:EditButton>
                                    </Buttons>
                                </PropertiesButtonEdit>
                            </dxe:GridViewDataButtonEditColumn>

                            <%--Batch Product Popup End--%>

                           
                            <%--Rev 2.0--%>
                            <%--<dxe:GridViewDataTextColumn VisibleIndex="3" Caption="Description" FieldName="gvColDiscription"  Width="">--%>
                            <dxe:GridViewDataTextColumn VisibleIndex="3" Caption="Description" FieldName="gvColDiscription"  Width="100">
                            <%--Rev end 2.0--%>
                                <PropertiesTextEdit>
                                </PropertiesTextEdit>
                                <CellStyle Wrap="true" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewCommandColumn VisibleIndex="4" Caption="Addl. Desc." Width="7%">
                                <CustomButtons>
                                    <dxe:GridViewCommandColumnCustomButton Text=" " ID="addlDesc" Image-Url="/assests/images/more.png" Image-ToolTip="Addl. Desc.">
                                        <Image ToolTip="Addl. Desc." Url="/assests/images/more.png">
                                        </Image>
                                    </dxe:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                            </dxe:GridViewCommandColumn>



                            <dxe:GridViewDataTextColumn VisibleIndex="5" Caption="Quantity" FieldName="gvColQuantity" Width="90" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                                <PropertiesTextEdit FocusedStyle-HorizontalAlign="Right" Style-HorizontalAlign="Right">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" />
                                    <%--For quantity user control Subhra 17-02-2019--%>
                                    <%-- <ClientSideEvents LostFocus="AutoCalValue" />--%>
                                    <ClientSideEvents LostFocus="AutoCalValue" GotFocus="QuantityProductsGotFocus" />
                                    <%--For quantity user control--%>
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="6" Caption="UOM(Pur.)" FieldName="gvColUOM" Width="90" HeaderStyle-HorizontalAlign="Right">
                                <PropertiesTextEdit>
                                    <ClientSideEvents LostFocus="UomLostFocus" />
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>

                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewCommandColumn VisibleIndex="7" Caption="Multi UOM" Width="60">
                                <CustomButtons>
                                    <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomMultiUOM" Image-Url="/assests/images/MultiUomIcon.png" Image-ToolTip="Multi UOM">
                                    </dxe:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                            </dxe:GridViewCommandColumn>

                                      <%--  Manis 24428--%> 
                                     <dxe:GridViewDataTextColumn Caption="Multi Qty" CellStyle-HorizontalAlign="Right" FieldName="Order_AltQuantity" PropertiesTextEdit-MaxLength="14" VisibleIndex="8" Width="60" ReadOnly="true">
                                                        <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right">
                                                           <%-- <ClientSideEvents GotFocus="QuantityGotFocus" LostFocus="QuantityTextChange" />--%>
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" />
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                     
                                                    <dxe:GridViewDataTextColumn Caption="Multi Unit" FieldName="Order_AltUOM" ReadOnly="true" VisibleIndex="9" Width="60" >
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>


                                                     <%--  Manis End 24428--%> 

                            <%--Mantis Issue 25235--%>
                            <dxe:GridViewDataTextColumn Caption="Rate" FieldName="gvColRate" Width="80" HeaderStyle-HorizontalAlign="Right" VisibleIndex="10">
                                <PropertiesTextEdit Style-HorizontalAlign="Right">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                    <ClientSideEvents LostFocus="AutoCalValueBtRate" />
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Value" FieldName="gvColValue" Width="80" HeaderStyle-HorizontalAlign="Right" VisibleIndex="11">
                                <PropertiesTextEdit Style-HorizontalAlign="Right">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <%--End of Mantis Issue 25235--%>
                            <dxe:GridViewDataDateColumn VisibleIndex="12" Caption="Expected Delivery Date" FieldName="ExpectedDeliveryDate" Width="140">
                                <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                    <ClientSideEvents DateChanged="function(s,e){InstrumentDateChange();}"></ClientSideEvents>
                                </PropertiesDateEdit>
                                <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataDateColumn>


                            <dxe:GridViewDataTextColumn FieldName="Remarks" Caption="Remarks" VisibleIndex="13" Width="9%" ReadOnly="false">
                                <PropertiesTextEdit Style-HorizontalAlign="Left">
                                    <Style HorizontalAlign="Left"></Style>
                                </PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>


                            <%--Mantis Issue 25235 --%>
                            <%--<dxe:GridViewDataTextColumn Caption="Rate" FieldName="gvColRate" Width="110" HeaderStyle-HorizontalAlign="Right" VisibleIndex="12">
                                <PropertiesTextEdit Style-HorizontalAlign="Right">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                    <ClientSideEvents LostFocus="AutoCalValueBtRate" />
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Value" FieldName="gvColValue" Width="110" HeaderStyle-HorizontalAlign="Right" VisibleIndex="14">
                                <PropertiesTextEdit Style-HorizontalAlign="Right">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>--%>
                            <%--End of Mantis Issue 25235--%>
                            <dxe:GridViewDataTextColumn FieldName="gvColIndentDetailsId" Caption="hidden Field Id" EditFormCaptionStyle-CssClass="hide" CellStyle-CssClass="hide" VisibleIndex="17" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                            </dxe:GridViewDataTextColumn>                            

                             <dxe:GridViewDataTextColumn FieldName="gvColProduct" Caption="hidden Field Id" VisibleIndex="18" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="ServiceTempDetails_ID" Caption="hidden Field Id" VisibleIndex="19" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="ServiceTemplate_ID" Caption="hidden Field Id" VisibleIndex="20" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="BalanceQty" Caption="Balance Qty" VisibleIndex="22" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="120" VisibleIndex="14" Caption="Add New" HeaderStyle-HorizontalAlign="Center">
                                <CustomButtons>
                                    <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomAddNewRow" Image-Url="/assests/images/add.png">
                                    </dxe:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                            </dxe:GridViewCommandColumn>

                        </Columns>
                        <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex"
                            BatchEditStartEditing="gridFocusedRowChanged" />
                        <SettingsDataSecurity AllowEdit="true" />
                        <%--Rev 3.0: "ColumnResizeMode add"--%>
                        <SettingsBehavior ColumnResizeMode="Control"/>
                        <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                            <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="Row" />
                        </SettingsEditing>

                        <Styles>
                            <StatusBar CssClass="statusBar">
                            </StatusBar>
                        </Styles>
                    </dxe:ASPxGridView>
                </div>
                <div>
                    <br />
                </div>
                <%--Mantis Issue 25053--%>
                <div class="row" id="divIsDirector">
                    <div>
                        <div class="typeHeader">Approval Details</div>
                        <%--<div class="col-md-3">
                            <label>Approval Action <span style="color: red;">*</span></label>
                            <div id="tdddlApprovalAction">
                                <select id="ddlApprovalAction" class="form-control">
                                    <option value="0">Select</option>
                                    <option value="1">Approve</option>
                                    <option value="2">Reject</option>
                                    <option value="3">Hold</option>
                                </select>
                            </div>
                        </div>--%>
                        <div class="col-md-3">
                            <div class="checkbox">
                                <label class="red">
                                    <input type="checkbox" id="chkDirectorApprovalRequired" onchange="chkDirectorApprovalRequired_change();" />
                                    Is Director Approval Required?</label>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label id="divEmployee" class="hide">
                                <div>Employee</div>
                            </label>
                            <div class="hide" id="divEmployeeIn">
                                <div class="dropDev">
                                    <dxe:ASPxComboBox ID="dddlApprovalEmployee" runat="server" ClientInstanceName="cdddlApprovalEmployee" Width="100%">
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                        <%--<div class="col-md-6">
                            <label>Remarks <span style="color: red;">*</span></label>
                            <div>
                                
                                <input type="text" class="form-control" id="txtApprovalRemarks" />
                            </div>
                        </div>--%>
                    </div>
                </div>
                <%--End of Mantis Issue 25053--%>
                <table style="float: left;">
                    <tr>
                        <td colspan="3">

                            <%--<asp:Button ID="btnnew" CssClass="btn btn-primary" runat="server" Text="S&#818;ave & New" OnClientClick="return  Save_ButtonClick();"   />--%>
                            <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                            <dxe:ASPxButton ID="btnnew" ClientInstanceName="cbtnnew" runat="server" AutoPostBack="False" Text="Save & N&#818;ew" CssClass="btn btn-primary hide" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                            </dxe:ASPxButton>


                            <%--<asp:Button ID="btnSaveExit" CssClass="btn btn-primary" runat="server" Text="Save & Ex&#818;it" OnClientClick="return  SaveExitButtonClick();" />--%>

                            <dxe:ASPxButton ID="btnSaveExit" ClientInstanceName="cbtnSaveExit" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {SaveExitButtonClick();}" />
                            </dxe:ASPxButton>
                            <span id="dvApprove" style="display: none" runat="server">
                                <dxe:ASPxButton ID="btn_Approve" ClientInstanceName="cbtn_Approve" CssClass="btn btn-success" runat="server" AutoPostBack="False" Text="Approve" UseSubmitBehavior="False">
                                    <ClientSideEvents Click="function(s, e) {Approve_ButtonClick();}" />
                                </dxe:ASPxButton>
                            </span>
                            <span id="dvReject" style="display: none" runat="server">
                                <dxe:ASPxButton ID="btn_Reject" ClientInstanceName="cbtn_Reject" runat="server" CssClass="btn btn-danger" AutoPostBack="False" Text="Reject" UseSubmitBehavior="False">
                                    <ClientSideEvents Click="function(s, e) {Reject_ButtonClick();}" />
                                </dxe:ASPxButton>
                            </span>

                            <dxe:ASPxButton ID="btnSaveUdf" ClientInstanceName="cbtn_SaveUdf" runat="server" AutoPostBack="False" Text="U&#818;DF" UseSubmitBehavior="False"
                                CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                <ClientSideEvents Click="function(s, e) {OpenUdf();}" />
                            </dxe:ASPxButton>
                            <uc3:UOMConversionControl runat="server" ID="UOMConversionControl" />
                        </td>
                    </tr>
                    <tr><b><span id="taggedApproval" style="display: none; color: red">************Already Approved.</span></b></tr>
                    <tr><b><span id="tagged" style="display: none; color: red">Tagged in Other Module. Cannot Modify</span></b></tr>
                </table>
            </div>
        </div>
        <div id="DivEdit">
            <%--<dxe:ASPxGridView ID="Grid_PurchaseIndent" runat="server" AutoGenerateColumns="False" OnCustomCallback="Grid_PurchaseIndent_CustomCallback"
                ClientInstanceName="CgvPurchaseIndent" KeyFieldName="Indent_Id" Width="100%" OnCustomButtonInitialize="Grid_PurchaseIndent_CustomButtonInitialize"
                SettingsBehavior-AllowFocusedRow="true" SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true"
                SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true">--%>
            <div class="makeFullscreen ">
                <span class="fullScreenTitle">Purchase Indent/Requisition List</span>
                <span class="makeFullscreen-icon half hovered " data-instance="CgvPurchaseIndent" title="Maximize Grid" id="expandCgvPurchaseIndent"><i class="fa fa-expand"></i></span>
                <dxe:ASPxGridView ID="Grid_PurchaseIndent" runat="server" AutoGenerateColumns="False" OnCustomCallback="Grid_PurchaseIndent_CustomCallback"
                    ClientInstanceName="CgvPurchaseIndent" KeyFieldName="Indent_Id" Width="100%" OnCustomButtonInitialize="Grid_PurchaseIndent_CustomButtonInitialize" Settings-HorizontalScrollBarMode="Auto"
                    SettingsBehavior-AllowFocusedRow="true" DataSourceID="EntityServerModeDataSource" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" SettingsBehavior-ColumnResizeMode="Control">
                    <SettingsSearchPanel Visible="True" Delay="5000" />
                    <%-- SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true"
                SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true" --%>
                    <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                    <ClientSideEvents CustomButtonClick="CustomButtonClick" />
                    <Columns>
                        <dxe:GridViewDataCheckColumn VisibleIndex="0" Visible="false">
                            <EditFormSettings Visible="True" />
                            <EditItemTemplate>
                                <dxe:ASPxCheckBox ID="ASPxCheckBox1" Text="" runat="server"></dxe:ASPxCheckBox>
                            </EditItemTemplate>
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataCheckColumn>
                        <dxe:GridViewDataTextColumn FieldName="Indent_Id" Visible="false" SortOrder="Descending">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="1" Caption="Document No." FixedStyle="Left" FieldName="Indent_RequisitionNumber" Width="120">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="2" Caption="Posting Date" FixedStyle="Left" FieldName="Indent_RequisitionDate" Width="90" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                            <%--<PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"
                            DisplayFormatInEditMode="True"></PropertiesTextEdit>--%>
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="3" Caption="From Unit" FieldName="Indent_branch">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <%-- <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Total Amount" FieldName="ValueInBaseCurrency" Width="150">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                    </dxe:GridViewDataTextColumn>--%>
                        <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Purchase Order No." FieldName="PurchaseOrder_Number">
                            <CellStyle CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="5" Caption="Purchase Order Date" FieldName="PurchaseOrder_Date">
                            <CellStyle CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="6" Caption="Purpose" FieldName="Indent_Purpose">
                            <CellStyle CssClass="gridcellleft" Wrap="True"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <%--Mantis Issue 25235--%>
                        <dxe:GridViewDataTextColumn VisibleIndex="7" Caption="Vendor" FieldName="Vendor_Name">
                            <CellStyle CssClass="gridcellleft" Wrap="True"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <%--End of Mantis Issue 25235--%>
                        <dxe:GridViewDataTextColumn VisibleIndex="8" Caption="Project Name" FieldName="Proj_Name">
                            <CellStyle CssClass="gridcellleft" Wrap="True"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>


                        <dxe:GridViewDataTextColumn VisibleIndex="9" Caption="Approval Status" FieldName="PurchaseIndent_ApproveStatus">
                            <CellStyle CssClass="gridcellleft" Wrap="True"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="10" Caption="Revision No." FieldName="Indent_RevisionNo">
                            <CellStyle CssClass="gridcellleft" Wrap="True"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn VisibleIndex="11" Caption="Revision Date" FieldName="Indent_RevisionDate">
                            <CellStyle CssClass="gridcellleft" Wrap="True"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>


                        <dxe:GridViewDataTextColumn VisibleIndex="12" Caption="Status" FieldName="Status">
                            <CellStyle CssClass="gridcellleft" Wrap="True"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="13" Caption="MRP" FieldName="MRP">
                            <CellStyle CssClass="gridcellleft" Wrap="True"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="14" Caption="Entered By" FieldName="EnteredBy">
                            <CellStyle CssClass="gridcellleft" Wrap="True"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="15" Caption="Last Update On" FieldName="LastUpdateOn">
                            <CellStyle CssClass="gridcellleft" Wrap="True"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="16" Caption="Updated By" FieldName="UpdatedBy">
                            <CellStyle CssClass="gridcellleft" Wrap="True"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <%--       <dxe:GridViewCommandColumn VisibleIndex="22" Width="180px" Caption="Actions" ButtonType="Image" HeaderStyle-HorizontalAlign="Center">
                            <CustomButtons>
                                <dxe:GridViewCommandColumnCustomButton ID="CustomBtnPO" Text="PO Details" Styles-Style-CssClass="pad">
                                    <Image Url="/assests/images/document.png" ToolTip="PO Details"></Image>
                                </dxe:GridViewCommandColumnCustomButton>
                                <dxe:GridViewCommandColumnCustomButton ID="CustomBtnView" Text="View" Styles-Style-CssClass="pad">
                                    <Image Url="/assests/images/viewIcon.png" ToolTip="View"></Image>
                                </dxe:GridViewCommandColumnCustomButton>
                                <dxe:GridViewCommandColumnCustomButton ID="CustomBtnEdit" Text="Edit" Styles-Style-CssClass="pad">
                                    <Image Url="/assests/images/Edit.png" ToolTip="Edit"></Image>
                                </dxe:GridViewCommandColumnCustomButton>
                                <dxe:GridViewCommandColumnCustomButton ID="CustomBtnDelete" Text="Delete" Styles-Style-CssClass="pad">
                                    <Image Url="/assests/images/Delete.png" ToolTip="Delete"></Image>
                                </dxe:GridViewCommandColumnCustomButton>
                                <dxe:GridViewCommandColumnCustomButton ID="CustomBtnPrint" Text="Print">
                                    <Image Url="/assests/images/Print.png" ToolTip="Print"></Image>
                                </dxe:GridViewCommandColumnCustomButton>
                            </CustomButtons>
                        </dxe:GridViewCommandColumn>--%>

                        <dxe:GridViewDataTextColumn VisibleIndex="17" Caption="Closed" FieldName="Closed" Width="0px">
                            <CellStyle CssClass="gridcellleft" Wrap="True"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="18" Width="188px">
                            <DataItemTemplate>

                                <a href="javascript:void(0);" onclick="OnPODetailsClick('<%#Eval("Indent_Id")%>')" class="pad" title="PO Details">
                                    <img src="../../../assests/images/document.png" /></a>

                                <% if (rights.CanView)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnclickView('<%# Container.KeyValue %>','<%# Container.VisibleIndex %>')" class="pad" title="View">
                                    <img src="../../../assests/images/viewIcon.png" />
                                </a><%} %>

                                <% if (rights.CanEdit)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnEditClick('<%# Container.KeyValue %>','<%# Container.VisibleIndex %>','<%#Eval("Closed")%>')" class="pad" title="Edit" style='<%#Eval("IndentLastEntryStaus")%>'>
                                    <img src="../../../assests/images/info.png" /></a><%} %>
                                <%--Mantis Issue 24912--%>
                                <% if (rights.CanAdd)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnCopyClick('<%# Container.KeyValue %>','<%# Container.VisibleIndex %>','<%#Eval("Closed")%>')" class="pad" title="Copy" style='<%#Eval("IndentLastEntryStaus")%>'>
                                    <img src="../../../assests/images/copy.png" /></a><%} %>
                                <%--End of Mantis Issue 24912--%>
                                <% if (rights.CanDelete)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>','<%# Container.VisibleIndex %>','<%#Eval("Closed")%>')" class="pad" title="Delete" style='<%#Eval("IndentLastEntryStaus")%>'>
                                    <img src="../../../assests/images/Delete.png" /></a><%} %>

                                <% if (rights.CanClose)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnClosedClick('<%# Container.KeyValue %>','<%# Container.VisibleIndex %>','<%#Eval("Closed")%>')" class="pad" title="Close" style='<%#Eval("IndentLastEntryStaus")%>'>
                                    <img src="../../../assests/images/closePop.png" /></a><%} %>

                                <% if (rights.CanApproved && isApprove)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnclickApprove('<%# Container.KeyValue %>','<%# Container.VisibleIndex %>','<%#Eval("Closed")%>')" class="pad" title="Approve/Reject" style='<%#Eval("IndentLastEntryStaus")%>'>
                                    <img src="../../../assests/images/verified.png" />
                                </a><%} %>

                                <% if (rights.CanPrint)
                                   { %>
                                <a href="javascript:void(0);" onclick="onPrintClickJv('<%#Eval("Indent_Id")%>')" class="pad" title="print">
                                    <img src="../../../assests/images/Print.png" />
                                </a><%} %>
                                <%--Mantis Issue 25053--%>
                                <% if (rights.SendSMS)
                               { %>
                                <a href="javascript:void(0);" onclick="onSmsClickJv('<%#Eval("Indent_Id")%>')" id="onSmsClickJv" class="pad" title="Send Sms">
                                    <img src="../../../assests/images/sms.png" />
                                </a>
                                <% } %>
                                <%--End of Mantis Issue 25053--%>
                                <%--Mantis Issue 25125--%>
                                <% if (rights.CanAddUpdateDocuments)
                               { %>
                                <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%#Eval("Indent_Id")%>')" class="pad" title="Add/View Attachment">
                                    <img src="../../../assests/images/upload.png" /></a>
                            <% } %>
                                <%--End of Mantis Issue 25125--%>
                            </DataItemTemplate>
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <CellStyle HorizontalAlign="Center"></CellStyle>
                            <HeaderTemplate><span>Actions</span></HeaderTemplate>
                            <EditFormSettings Visible="False"></EditFormSettings>
                        </dxe:GridViewDataTextColumn>


                    </Columns>
                    <SettingsCookies Enabled="true" StorePaging="true" Version="1.7" CookiesID="PurchaseIndentCookies" />
                    <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                    <ClientSideEvents EndCallback="function(s, e) {
	                                        ShowMsgLastCall();
                                        }" />
                    <SettingsBehavior AllowFocusedRow="false" ConfirmDelete="True" />
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
                    ContextTypeName="ERPDataClassesDataContext" TableName="V_PurchaseIndentList" />
                <asp:HiddenField ID="hfIsFilter" runat="server" />
                <asp:HiddenField ID="hfFromDate" runat="server" />
                <asp:HiddenField ID="hfToDate" runat="server" />
                <asp:HiddenField ID="hfBranchID" runat="server" />

                <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
                    <ClientSideEvents ControlsInitialized="AllControlInitilize" />
                </dxe:ASPxGlobalEvents>
                <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                </dxe:ASPxGridViewExporter>
            </div>
        </div>
        <asp:HiddenField ID="hdnRefreshType" runat="server" />
        <asp:HiddenField ID="hdnEditIndentID" runat="server" />
        <asp:HiddenField ID="hdfIsDelete" runat="server" />
        <asp:HiddenField ID="hdnSchemaType" runat="server" />
        <asp:HiddenField ID="hdnCurrenctId" runat="server" />
        <asp:HiddenField ID="hdnSaveNew" runat="server" />
        <asp:HiddenField ID="hdnEditClick" runat="server" />
        <asp:HiddenField ID="hdn_Mode" runat="server" />
        <asp:HiddenField ID="hdnPIID" runat="server" />
        <%--Mantis Issue 25053--%>
        <asp:HiddenField ID="hdnEmployee" runat="server" />
        <asp:HiddenField ID="hdnSettings" runat="server" />
        <asp:HiddenField ID="hdnIndentId" runat="server" />
        <%--End of Mantis Issue 25053--%>
         <%--Mantis Issue 25235--%>
        <%--Customer Popup--%>
            <dxe:ASPxPopupControl ID="DirectAddCustPopup" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="AspxDirectAddCustPopup" Height="750px"
            Width="1020px" HeaderText="Add New Customer" Modal="true" AllowResize="true" ResizingMode="Postponed">
            <HeaderTemplate>
                <span>Add New Customer</span>
            </HeaderTemplate>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
        <%--End of Mantis Issue 25235--%>

        <asp:HiddenField ID="hdnApprovalRemarksValue" runat="server" />
        <asp:SqlDataSource ID="dsBranch" runat="server"
            ConflictDetection="CompareAllValues"
            SelectCommand=""></asp:SqlDataSource>

        <asp:SqlDataSource ID="dsHierarchy" runat="server"
            ConflictDetection="CompareAllValues"
            SelectCommand=""></asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlSchematype" runat="server"
            SelectCommand="Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select ID,SchemaName From tbl_master_Idschema  Where TYPE_ID='15')) as X Order By ID ASC"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlCurrency" runat="server"
            SelectCommand="select Currency_ID,Currency_AlphaCode from Master_Currency Order By Currency_ID"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlCurrencyBind" runat="server"></asp:SqlDataSource>

        <%--Rev Subhra 0019337 23-01-2019--%>
        <dxe:ASPxPopupControl ID="ASPxDocumentsPopup" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentStyle VerticalAlign="Top"></ContentStyle>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" ClientSideEvents-EndCallback="cSelectPanelEndCall">
                        <ClientSideEvents EndCallback="cSelectPanelEndCall"></ClientSideEvents>
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
        <%--End of Rev Subhra 0019337 23-01-2019--%>

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
                                        <dxe:GridViewDataTextColumn Caption="Purchase Indent No." FieldName="Number"
                                            VisibleIndex="0" FixedStyle="Left">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Created On" FieldName="CreateDate"
                                            VisibleIndex="1" FixedStyle="Left">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Unit" FieldName="branch_description"
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


                            <%--<div class="col-md-12" style="padding-top: 10px;">
                            <dxe:ASPxButton ID="ASPxButton1" CausesValidation="true" ClientInstanceName="cbtn_PrpformaStatus" runat="server"
                                AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                                <ClientSideEvents Click="function (s, e) {SaveApprovalStatus();}" />
                            </dxe:ASPxButton>
                        </div>--%>
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
                Width="900px" HeaderText="User Wise Purchase Indent Status" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
                PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                ContentStyle-CssClass="pad">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <div class="row">
                            <div class="col-md-12">
                                <dxe:ASPxGridView ID="gridUserWiseQuotation" runat="server" KeyFieldName="ID" AutoGenerateColumns="False"
                                    Width="100%" ClientInstanceName="cgridUserWiseQuotation" OnCustomCallback="gridUserWiseQuotation_CustomCallback" OnPageIndexChanged="gridUserWiseQuotation_PageIndexChanged">
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

        <%-- Sandip Approval Dtl Section End--%>

        <%--Batch Product Popup Start--%>

        <%-- <dxe:ASPxPopupControl ID="ProductpopUp" runat="server" ClientInstanceName="cProductpopUp"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
            Width="700" HeaderText="Select Product" AllowResize="true" ResizingMode="Postponed" Modal="true">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <label><strong>Search By Product Name</strong></label>
                    <dxe:ASPxGridLookup ID="productLookUp" runat="server" DataSourceID="ProductDataSource" ClientInstanceName="cproductLookUp"
                        KeyFieldName="Products_ID" Width="800" TextFormatString="{0}" MultiTextSeparator=", " ClientSideEvents-TextChanged="ProductSelected" ClientSideEvents-KeyDown="ProductlookUpKeyDown">
                        <Columns>
                            <dxe:GridViewDataColumn FieldName="ProductsName" Caption="Name" Width="220">
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
                <asp:Parameter Type="String" Name="Action" DefaultValue="ProductDetailsPurchaseIndent" />
            </SelectParameters>
        </asp:SqlDataSource>--%>


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
                        <input type="text" onkeydown="prodkeydown(event)" id="txtProdSearch" autofocus width="100%" placeholder="Search By Product Name or Description" />

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
        <%--Mantis Issue 25053--%>
        <div class="modal fade pmsModal w30" id="assignEmployee" role="dialog" aria-labelledby="assignpop" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    
                    <h5 class="modal-title">Select Employee</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    
                </div>
                <div class="modal-body">
                    <div class="row ">
                        
                        <div class="col-md-12 mTop5">
                            <label class="deep">Employee </label>
                            <div class="fullWidth">
                                 <select class="form-control" id="ddl_DirEmployee" style="width:200px" >
                                    <option value="0">--Select--</option>
                                </select>
                                <%--<input type="hidden" id="hdDbName" runat="server" />--%>
                                
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer" id="divsave">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Cancel</button>
                   
                    <button type="button" class="btn btn-success" onclick="PhoneNoSend();">Confirm</button>
                    
                </div>
            </div>
        </div>
    </div>
       <%-- End of Mantis Issue 25053--%>
        <%--Mantis Issue 25235--%>
        <div class="modal fade" id="CustModel" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Vendor Search</h4>
                    </div>
                    <div class="modal-body">
                        <input type="text" onkeydown="VendorModekkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search By Vendor Name or Unique Id" />

                        <div id="CustomerTable">
                            <table border='1' width="100%" class="dynamicPopupTbl">
                                <tr class="HeaderStyle">
                                    <th class="hide">id</th>
                                    <th>Vendor Name</th>
                                    <th>Unique Id</th>
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
        <%--End of Mantis Issue 25235--%>
        <%--Batch Product Popup End--%>
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
        <%--UDF Popup End--%>
        <dxe:ASPxCallbackPanel runat="server" ID="acbpCrpUdf" ClientInstanceName="cacbpCrpUdf" OnCallback="acbpCrpUdf_Callback">
            <PanelCollection>
                <dxe:PanelContent runat="server">
                </dxe:PanelContent>
            </PanelCollection>
            <ClientSideEvents EndCallback="acbpCrpUdfEndCall" />
        </dxe:ASPxCallbackPanel>

        <dxe:ASPxCallbackPanel runat="server" ID="acpAvailableStock" ClientInstanceName="cacpAvailableStock" OnCallback="acpAvailableStock_Callback">
            <PanelCollection>
                <dxe:PanelContent runat="server">
                </dxe:PanelContent>
            </PanelCollection>
            <ClientSideEvents EndCallback="acpAvailableStockEndCall" />
        </dxe:ASPxCallbackPanel>
        <%--PurchaseOrderList Popup --%>

        <dxe:ASPxPopupControl ID="apcPurchaseOrderList" runat="server" ClientInstanceName="capcPurchaseOrderList"
            Width="900px" HeaderText="" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">

            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">

                    <div class="row">
                        <div class="col-md-12">
                            <dxe:ASPxGridView ID="gridPOLIst" runat="server" KeyFieldName="PurchaseOrder_Id" AutoGenerateColumns="False"
                                Width="100%" ClientInstanceName="cgridPOLIst" OnCustomCallback="gridPOLIst_CustomCallback" OnDataBinding="gridPOLIst_DataBinding">
                                <Columns>
                                    <dxe:GridViewDataTextColumn Caption="Purchase Order Number" FieldName="PurchaseOrder_Number"
                                        VisibleIndex="0" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="PurchaseOrder Date" FieldName="PurchaseOrder_Date"
                                        VisibleIndex="1" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Approval Status" FieldName="ApprovalStatus"
                                        VisibleIndex="2" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>


                                    <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="15%">
                                        <DataItemTemplate>
                                            <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')" class="pad" title="Details">
                                                <img src="../../../assests/images/viewIcon.png" />
                                            </a>
                                        </DataItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                        <HeaderTemplate><span>Actions</span></HeaderTemplate>
                                        <EditFormSettings Visible="False"></EditFormSettings>
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
                                <%--  <ClientSideEvents EndCallback="OnApprovalEndCall" />--%>
                            </dxe:ASPxGridView>
                        </div>
                        <div class="clear"></div>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
        <dxe:ASPxPopupControl ID="apcPoDetails" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="capcPoDetails" Height="630px"
            Width="1200px" HeaderText="Purchase Order Details" Modal="true" AllowResize="true" ResizingMode="Postponed">

            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
        <dxe:ASPxLoadingPanel ID="LoadingPanelCRP" runat="server" ClientInstanceName="cLoadingPanelCRP" ContainerElementID="divSubmitButton"
            Modal="True">
        </dxe:ASPxLoadingPanel>
        <%--PurchaseOrderList Popup END --%>
    </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnConvertionOverideVisible" />
    <asp:HiddenField runat="server" ID="hdnShowUOMConversionInEntry" />
    <asp:HiddenField ID="hdnProjectSelectInEntryModule" runat="server" />
    <%--for Project  --%>

    <%--for multiUOM start--%>

    <dxe:ASPxPopupControl ID="Popup_MultiUOM" runat="server" ClientInstanceName="cPopup_MultiUOM"
        Width="900px" HeaderText="Multi UOM Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ClientSideEvents Closing="function(s, e) {
	closeMultiUOM(s, e);}" />
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="Top clearfix">



                    <div class="clearfix col-md-12" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;">

                        <table class="eqTble">
                            <tr>
                                <td>
                                    <div>
                                        <div style="margin-bottom: 5px;">
                                            <div>
                                                <label>Base Quantity</label>
                                            </div>
                                            <div>
                                                  <%--Rev Mantis Issue 24428/24429--%>
                                              <%--  <input type="text" id="UOMQuantity" style=text-align: right;" maxlength="18" class="allownumericwithdecimal" />--%>
                                                  <input type="text" id="UOMQuantity" style="text-align: right;" maxlength="18" class="allownumericwithdecimal" onchange="CalcBaseRate()" />
                                                 <%--End of Rev Mantis Issue 24428/24429--%>
                                            </div>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="Left_Content" style="">
                                        <div>
                                            <label style="text-align: right;">Base UOM</label>
                                        </div>
                                        <div>
                                            <dxe:ASPxComboBox ID="cmbUOM" ClientInstanceName="ccmbUOM" runat="server" SelectedIndex="0" DataSourceID="UomSelect"
                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" ValueField="UOM_ID" TextField="UOM_Name">
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>
                                </td>
                                    <%--Mantis Issue 24428--%>
                                 <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label>Base Rate </label>
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="cmbBaseRate" runat="server" Width="80px" ClientInstanceName="ccmbBaseRate" DisplayFormatString="0.000" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right" ReadOnly="true" ></dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                <%--End of Mantis Issue 24428--%>
                                <td>
                                    <span style="font-size: 22px; padding-top: 15px; display: inline-block;">=</span>
                                </td>
                                <td>
                                    <div>
                                        <div>
                                            <label style="text-align: right;">Alt. UOM</label>
                                        </div>
                                        <div class="Left_Content" style="">
                                            <dxe:ASPxComboBox ID="cmbSecondUOM" ClientInstanceName="ccmbSecondUOM" runat="server" SelectedIndex="0" DataSourceID="AltUomSelect"
                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" ValueField="UOM_ID" TextField="UOM_Name">
                                                <ClientSideEvents TextChanged="function(s,e) { PopulateMultiUomAltQuantity();}" />
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label>Alt. Quantity </label>
                                        </div>
                                        <div>
                                            <%--  <input type="text" id="AltUOMQuantity" style="text-align:right;"  maxlength="18" class="allownumericwithdecimal"/> --%>
                                            <dxe:ASPxTextBox ID="AltUOMQuantity" runat="server" ClientInstanceName="cAltUOMQuantity" DisplayFormatString="0.0000" MaskSettings-Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right">
                                                  <%--Mantis Issue 24428--%>
                                                <ClientSideEvents TextChanged="function(s,e) { CalcBaseQty();}" />
                                                <%--End of Mantis Issue 24428--%>
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                   <%--Mantis Issue 24428--%>
                                 <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label>Alt Rate </label>
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="cmbAltRate" Width="80px" runat="server" ClientInstanceName="ccmbAltRate" DisplayFormatString="0.000" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right"  >
                                                <ClientSideEvents TextChanged="function(s,e) { CalcBaseRate();}" />
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            
                                        </div>
                                        <div>
                                            <%--<label class="checkbox-inline mlableWh">
                                                <input type="checkbox" id="chkUpdateRow"  />
                                              
                                            </label>--%>
                                            <label class="checkbox-inline mlableWh">
                                                <input type="checkbox" id="chkUpdateRow"  />
                                                <span style="margin: 0px 0; display: block">
                                                    <dxe:ASPxLabel ID="ASPxLabel18" runat="server" Text="Update Row">
                                                    </dxe:ASPxLabel>
                                                </span>
                                            </label>
                                        </div>
                                    </div>

                                    
                                </td>
                                <%--End of Mantis Issue 24428--%>
                                <td style="padding-top: 14px;">
                                    <dxe:ASPxButton ID="btnMUltiUOM" ClientInstanceName="cbtnMUltiUOM" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                        <ClientSideEvents Click="function(s, e) {SaveMultiUOM();}" />
                                    </dxe:ASPxButton>
                                </td>
                            </tr>
                        </table>

                    </div>
                    <div class="clearfix">
                        <dxe:ASPxGridView ID="grid_MultiUOM" runat="server" KeyFieldName="AltUomId;AltQuantity" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="cgrid_MultiUOM" OnCustomCallback="MultiUOM_CustomCallback" OnDataBinding="MultiUOM_DataBinding"
                            SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200" SettingsBehavior-AllowSort="false">
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity"
                                    VisibleIndex="0" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="UOM" FieldName="UOM"
                                    VisibleIndex="1">
                                </dxe:GridViewDataTextColumn>
                                     <%--Mantis Issue 24428 --%>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="BaseRate"
                                    VisibleIndex="2" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24428 --%>



                                <dxe:GridViewDataTextColumn Caption="Alt. UOM" FieldName="AltUOM"
                                    VisibleIndex="2">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Alt. Quantity" FieldName="AltQuantity"
                                    VisibleIndex="3" HeaderStyle-HorizontalAlign="Right">
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn Caption="" FieldName="UomId" Width="0px"
                                    VisibleIndex="3">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="" FieldName="AltUomId" Width="0px"
                                    VisibleIndex="5">
                                </dxe:GridViewDataTextColumn>

                                
                                   <%--Mantis Issue 24428--%>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="AltRate"
                                    VisibleIndex="7" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Update Row" FieldName="UpdateRow"
                                    VisibleIndex="7" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24428 --%>

                                <dxe:GridViewDataTextColumn VisibleIndex="10" Width="80px" Caption="Action">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" onclick="Delete_MultiUom('<%# Container.KeyValue %>','<%#Eval("SrlNo") %>')" title="Delete">
                                            <img src="/assests/images/crs.png" /></a>

                                           <%--Mantis Issue 24428 --%>

                                           <a href="javascript:void(0);" onclick="Edit_MultiUom('<%# Container.KeyValue %>','<%#Eval("MultiUOMSR") %>')" title="Edit">
                                            <img src="/assests/images/Edit.png" /></a>
                                          <%--End of Mantis Issue 24428 --%>
                                    </DataItemTemplate>
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <ClientSideEvents EndCallback="OnMultiUOMEndCallback" />
                            <SettingsPager Visible="false"></SettingsPager>
                            <SettingsLoadingPanel Text="Please Wait..." />
                        </dxe:ASPxGridView>
                    </div>
                    <div class="clearfix">
                        <br />
                        <div style="align-content: center">
                            <dxe:ASPxButton ID="ASPxButton7" ClientInstanceName="cbtnfinalUomSave" Width="50px" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                <ClientSideEvents Click="function(s, e) {FinalMultiUOM();}" />
                            </dxe:ASPxButton>
                        </div>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>


    <dxe:ASPxCallbackPanel runat="server" ID="callback_InlineRemarks" ClientInstanceName="ccallback_InlineRemarks" OnCallback="callback_InlineRemarks_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
                <dxe:ASPxPopupControl ID="Popup_InlineRemarks" runat="server" ClientInstanceName="cPopup_InlineRemarks"
                    Width="900px" HeaderText="Remarks" PopupHorizontalAlign="WindowCenter"
                    BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                    Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                    ContentStyle-CssClass="pad">
                    <%--<ClientSideEvents Closing="function(s, e) {
	                   closeRemarks(s, e);}" />--%>
                    <ContentStyle VerticalAlign="Top" CssClass="pad">
                    </ContentStyle>
                    <ContentCollection>

                        <dxe:PopupControlContentControl runat="server">
                            <div>
                                <asp:Label ID="lblInlineRemarks" runat="server" Text="Additional Description"></asp:Label>

                                <asp:TextBox ID="txtInlineRemarks" runat="server" ValidateRequestMode="Disabled" TabIndex="16" Width="100%" TextMode="MultiLine" Rows="5" Columns="10" Height="50px" MaxLength="5000"></asp:TextBox>
                            </div>

                            <div class="clearfix">
                                <br />
                                <div style="align-content: center">
                                    <dxe:ASPxButton ID="btnSaveInlineRemarks" ClientInstanceName="cbtnSaveInlineRemarks" Width="50px" runat="server" AutoPostBack="false" Text="OK" CssClass="btn btn-primary" UseSubmitBehavior="false">

                                        <ClientSideEvents Click="function(s, e) {FinalRemarks();}" />
                                    </dxe:ASPxButton>
                                </div>
                            </div>
                        </dxe:PopupControlContentControl>

                    </ContentCollection>
                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                </dxe:ASPxPopupControl>
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="callback_InlineRemarks_EndCall" />
    </dxe:ASPxCallbackPanel>


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

    <asp:SqlDataSource ID="UomSelect" runat="server"
        SelectCommand="select UOM_ID,UOM_Name from Master_UOM "></asp:SqlDataSource>

    <asp:SqlDataSource ID="AltUomSelect" runat="server"
        SelectCommand="select UOM_ID,UOM_Name from Master_UOM "></asp:SqlDataSource>

    <asp:HiddenField ID="hddnuomFactor" runat="server" />
    <asp:HiddenField ID="hddnMultiUOMSelection" runat="server" />
    <asp:HiddenField ID="hdnQuantitySL" runat="server" />
    <asp:HiddenField ID="hdnPageStatus" runat="server" />
    <asp:HiddenField ID="hdnPageStatForApprove" runat="server" />
    <asp:HiddenField ID="hdnEditOrderId" runat="server" />
    <asp:HiddenField ID="hdnApproveStatus" runat="server" />
    <asp:HiddenField ID="hdnApprovalReqInq" runat="server" />
    <asp:HiddenField ID="hdnForBranchTaggingPurchase" runat="server" />
    <%-- for MultiUOM End--%>
    <asp:HiddenField ID="hdnProjectMandatory" runat="server" />
    <asp:HiddenField ID="hdnShowPReqPestControl" runat="server" />
     <asp:HiddenField ID="hdnShowMRPTaggingRequiredPurchaseIndent" runat="server" />
    
     <%--    Mantis Issue 24428 --%>
     <asp:HiddenField ID="hdProductID" runat="server" />

    <asp:HiddenField ID="hdnComponent" runat="server" />
    <asp:HiddenField ID="hdnComponentDetailsIDs" runat="server" />
   <%--    End Mantis Issue 24428 --%>
    <%--Mantis Issue 25053--%>
    <asp:HiddenField  ID="hdDbName" runat="server" />
    <%--End of Mantis Issue 25053--%>
    <%--Mantis Issue 25235--%>
    <asp:HiddenField ID="hdnVendorRequiredInPurchaseIndent" runat="server" />
     <asp:HiddenField ID="hdnCustomerId" runat="server" />
    <%--End of Mantis Issue 25235--%>

    <%--Mantis Issue 25394--%>
     <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">           
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>
    <%--End of Mantis Issue 25394--%>
    <%--Rev 4.0 --%>
     <asp:HiddenField ID="HdnBackDatedEntryPurchaseIndent" runat="server" />
  <%--  Rev 4.0 End--%>
 <%-- Rev 5.0--%>
 <asp:HiddenField runat="server" ID="hdnIsDuplicateItemAllowedOrNot" />
 <%-- Rev 5.0 End--%>
</asp:Content>
