<%@ Page Title="Purchase Invoice" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" 
    CodeBehind="AdjustmentOfPendingDocuments.aspx.cs" Inherits="ERP.OMS.Management.Activities.AdjustmentOfPendingDocuments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <%--Pending Document Grid For Adjustment Section Start By Sam--%>
    <script>
        var globalTaxRowIndex;
        function GetTaxVisibleIndex(s, e) {
            globalTaxRowIndex = e.visibleIndex;
        }

        function adjustedAmountLostFocus(s, e) {
            var AdjustedAmt = cgridTax.GetEditor("AdjustedAmt")
            var frontAmt = 0;
            var frontRow = 0;
            var backRow = -1;
            var backAmt = 0;
            for (var i = 0; i < cgridTax.GetVisibleRowsOnPage() ; i++) {
                frontAmt += parseFloat((cgridTax.batchEditApi.GetCellValue(backRow, 'AdjustedAmt') != null) ? (cgridTax.batchEditApi.GetCellValue(backRow, 'AdjustedAmt')) : '0.00');
                backAmt += parseFloat((cgridTax.batchEditApi.GetCellValue(frontRow, 'AdjustedAmt') != null) ? (cgridTax.batchEditApi.GetCellValue(frontRow, 'AdjustedAmt')) : '0.00');

                //if (frontProduct != "" || backProduct != "") {
                //    IsProduct = "Y";
                //    break;
                //}

                backRow--;
                frontRow++;
            }
            var adjsted = parseFloat($('#txt_AdjusttedAmt').text());
            if(adjsted>AdjustedAmt)
            {
                AdjustedAmt.SetValue('0.00');
                jAlert('Adjusting amount can not be greater then available amount');
                return;
            }
        }

        function cgridTax_EndCallBack(s, e) {
            //cgridTax.batchEditApi.StartEdit(0, 1);
            //$('.cgridTaxClass').show();

            //cgridTax.StartEditRow(0);


            ////check Json data
            //if (cgridTax.cpJsonData) {
            //    if (cgridTax.cpJsonData != "") {
            //        taxJson = JSON.parse(cgridTax.cpJsonData);
            //        cgridTax.cpJsonData = null;
            //    }
            //}
            //End Here

            //if (cgridTax.cpComboCode) {
            //    if (cgridTax.cpComboCode != "") {
            //        if (cddl_AmountAre.GetValue() == "1") {
            //            var selectedIndex;
            //            for (var i = 0; i < ccmbGstCstVat.GetItemCount() ; i++) {
            //                if (ccmbGstCstVat.GetItem(i).value.split('~')[0] == cgridTax.cpComboCode) {
            //                    selectedIndex = i;
            //                }
            //            }
            //            if (selectedIndex && ccmbGstCstVat.GetItem(selectedIndex) != null) {
            //                ccmbGstCstVat.SetValue(ccmbGstCstVat.GetItem(selectedIndex).value);
            //            }
            //            cmbGstCstVatChange(ccmbGstCstVat);
            //            cgridTax.cpComboCode = null;
            //        }
            //    }
            //}

            //if (cgridTax.cpUpdated.split('~')[0] == 'ok') {
            //    ctxtTaxTotAmt.SetValue(Math.round(cgridTax.cpUpdated.split('~')[1]));
            //    var gridValue = parseFloat(cgridTax.cpUpdated.split('~')[1]);
            //    var ddValue = parseFloat(ctxtGstCstVat.GetValue());
            //    ctxtTaxTotAmt.SetValue(Math.round(gridValue + ddValue));
            //    cgridTax.cpUpdated = "";
            //}

            //else {
            //    var totAmt = ctxtTaxTotAmt.GetValue();
            //    cgridTax.CancelEdit();
            //    caspxTaxpopUp.Hide();
            //    grid.batchEditApi.StartEdit(globalRowIndex, 13);
            //    grid.GetEditor("TaxAmount").SetValue(totAmt);
            //    grid.GetEditor("TotalAmount").SetValue(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()));
            //    if (cddl_AmountAre.GetValue() == '2') {
            //        var totalNetAmount = parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue());
            //        var totalRoundOffAmount = Math.round(totalNetAmount);
            //        grid.GetEditor("Amount").SetValue(parseFloat(grid.GetEditor("Amount").GetValue()) + (totalRoundOffAmount - totalNetAmount));
            //    }

            //}

            //if (cgridTax.GetVisibleRowsOnPage() == 0) {
            //    $('.cgridTaxClass').hide();
            //    ccmbGstCstVat.Focus();
            //}
            //Debjyoti Check where any Gst Present or not
            // If Not then hide the hole section

            //SetRunningTotal();
            //ShowTaxPopUp("IY");
            //RecalCulateTaxTotalAmountInline();
        }

        function SetRunningTotal() {
            var runningTot = parseFloat(clblProdNetAmt.GetValue());
            for (var i = 0; i < taxJson.length; i++) {
                cgridTax.batchEditApi.StartEdit(i, 3);
                if (taxJson[i].applicableOn == "R") {
                    cgridTax.GetEditor("calCulatedOn").SetValue(runningTot);
                    var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
                    var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
                    var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
                    var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
                    var thisRunningAmt = 0;
                    if (sign == '(+)') {
                        GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                        cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100);

                        ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) - GlobalCurTaxAmt);
                        GlobalCurTaxAmt = 0;
                    }
                    else {

                        GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                        cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100) * -1);

                        ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                        GlobalCurTaxAmt = 0;
                    }
                    SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
                }
                runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.batchEditApi.EndEdit();
            }
        }
        function RecalCulateTaxTotalAmountInline() {
            var totalInlineTaxAmount = 0;
            for (var i = 0; i < taxJson.length; i++) {
                cgridTax.batchEditApi.StartEdit(i, 3);
                var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
                var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
                if (sign == '(+)') {
                    totalInlineTaxAmount = totalInlineTaxAmount + parseFloat(cgridTax.GetEditor("Amount").GetValue());
                } else {
                    totalInlineTaxAmount = totalInlineTaxAmount - parseFloat(cgridTax.GetEditor("Amount").GetValue());
                }

                cgridTax.batchEditApi.EndEdit();
            }

            totalInlineTaxAmount = totalInlineTaxAmount + parseFloat(ctxtGstCstVat.GetValue());

            ctxtTaxTotAmt.SetValue(totalInlineTaxAmount);
        }
    </script>
    <%--Pending Document Grid For Adjustment Section Start By Sam--%>
     <%--Filteration Section Start By Sam--%>
    <script>
        function ClearField() {
            cFormDate.SetDate(null);
            ctoDate.SetDate(null) ;
            ccmbBranchfilter.SetSelectedIndex(0);
            
        }

        function updateGridByDate() {

            if (cFormDate.GetDate() == null) {
                jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
            }
            else if (ctoDate.GetDate() == null) {
                jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
            }
            else if (ccmbBranchfilter.GetValue() == null) {
                jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
            }
            else {
                var type = ($("[id$='rdl_Adjustment']").find(":checked").val() != null) ? $("[id$='rdl_Adjustment']").find(":checked").val() : "";
                var vendCust = cddl_type.GetValue();
                $('#branchName').text(ccmbBranchfilter.GetText());
                //PopulateCurrentBankBalance(ccmbBranchfilter.GetValue());
                //if (page.activeTabIndex == 0) {
                cGrdQuotation.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue() + '~' + type + '~' + vendCust);
                //}
                //else if (page.activeTabIndex == 1) {
                //    cCustomerReceiptGrid.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
                //}
            }
        }
    </script>

    <%-- Filteration Section Start By Sam--%>
    <script>
         
        function OnAddEditClick(e, obj) {
            var data = obj.split('~');
            if (data.length > 1)
                RowID = data[1];
            cproductpopup.Show();
            popproductPanel.PerformCallback(obj);
        }
    </script>
    <script>
        var PInvoice_id = 0;
        function onPrintJv(id) {
            PInvoice_id = id;
            cSelectPanel.cpSuccess = "";
            cDocumentsPopup.Show();
            CselectDuplicate.SetEnabled(false);
            CselectTriplicate.SetEnabled(false);
            CselectOriginal.SetCheckState('UnChecked');
            CselectDuplicate.SetCheckState('UnChecked');
            CselectTriplicate.SetCheckState('UnChecked');
            cCmbDesignName.SetSelectedIndex(0);
            cSelectPanel.PerformCallback('Bindalldesignes');
            $('#btnOK').focus();
        }

        function PerformCallToGridBind() {
            //cgriddocuments.PerformCallback('BindDocumentsGridOnSelection' + '~' + PInvoice_id);
            cSelectPanel.PerformCallback();
            cDocumentsPopup.Hide();
            return false;
        }

        //function cgridDocumentsEndCall(s, e) {
        //    debugger;
        //    if (cgriddocuments.cpSuccess != null) {
        //        var TotDocument = cgriddocuments.cpSuccess.split(',');
        //        if (TotDocument.length > 0) {
        //            for (var i = 0; i < TotDocument.length; i++) {
        //                if (TotDocument[i] != "") {
        //                    window.open("../../reports/XtraReports/Viewer/PurchaseInvoiceReportViewer.aspx?id=" + PInvoice_id + '&PrintOption=' + TotDocument[i], '_blank')
        //                }
        //            }
        //        }
        //    }
        //}



        //function cSelectPanelEndCall(s, e) {
        //    debugger;
        //    if (cSelectPanel.cpSuccess != null) {
        //        var TotDocument = cSelectPanel.cpSuccess.split(',');
        //        if (TotDocument.length > 0) {
        //            for (var i = 0; i < TotDocument.length; i++) {
        //                if (TotDocument[i] != "") {
        //                    if (cCmbDesignName.GetValue() == 1) {
        //                        window.open("../../reports/XtraReports/Viewer/PurchaseInvoiceReportViewer.aspx?id=" + PInvoice_id + '&PrintOption=' + TotDocument[i], '_blank')
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        function cSelectPanelEndCall(s, e) {
            debugger;
            if (cSelectPanel.cpSuccess != null) {
                var TotDocument = cSelectPanel.cpSuccess.split(',');
                var reportName = cCmbDesignName.GetValue();
                var module = 'PInvoice';
                if (TotDocument.length > 0) {
                    for (var i = 0; i < TotDocument.length; i++) {
                        if (TotDocument[i] != "") {
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + PInvoice_id + '&PrintOption=' + TotDocument[i], '_blank')
                        }
                    }
                }
            }
            //cSelectPanel.cpSuccess = null
            if (cSelectPanel.cpSuccess == "") {
                if (cSelectPanel.cpChecked != "") {
                    jAlert('Please check Original For Recipient and proceed.');
                }
                CselectDuplicate.SetEnabled(false);
                CselectTriplicate.SetEnabled(false);
                CselectOriginal.SetCheckState('UnChecked');
                CselectDuplicate.SetCheckState('UnChecked');
                CselectTriplicate.SetCheckState('UnChecked');
                cCmbDesignName.SetSelectedIndex(0);
            }
        }

        function OrginalCheckChange(s, e) {
            debugger;
            if (s.GetCheckState() == 'Checked') {
                CselectDuplicate.SetEnabled(true);
            }
            else {
                CselectDuplicate.SetCheckState('UnChecked');
                CselectDuplicate.SetEnabled(false);
                CselectTriplicate.SetCheckState('UnChecked');
                CselectTriplicate.SetEnabled(false);
            }

        }
        function DuplicateCheckChange(s, e) {
            if (s.GetCheckState() == 'Checked') {
                CselectTriplicate.SetEnabled(true);
            }
            else {
                CselectTriplicate.SetCheckState('UnChecked');
                CselectTriplicate.SetEnabled(false);
            }

        }

        function OpenPopUPUserWiseQuotaion() {
            cgridUserWiseQuotation.PerformCallback();
            cPopupUserWiseQuotation.Show();
        }
        document.onkeydown = function (e) {
            if (event.keyCode == 18) isCtrl = true;
            if (event.keyCode == 65 && isCtrl == true) { //run code for Ctrl+S -- ie, Save & New  
                StopDefaultAction(e);
                OnAddButtonClick();
            } 
        }

        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }

        

        function OnClickDelete(keyValue) {
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    cGrdQuotation.PerformCallback('Delete~' + keyValue);
                }
            });
        }
        function OnClickStatus(keyValue) {
            GetObjectID('hiddenedit').value = keyValue;
            cGrdQuotation.PerformCallback('Edit~' + keyValue);
        }

        function OpenPopUPApprovalStatus() {
            cgridPendingApproval.PerformCallback();
            cpopupApproval.Show();
        }

        //var globalRowIndex;
        //function GetVisibleIndex(s, e) {
        //    globalRowIndex = e.visibleIndex;
        //}
        //RowClick = "GetVisibleIndex"

        //function Approvalgrid_EndCallBack() {
        //    if (cgridPendingApproval.cpEdit != null) {
        //        cpopupApproval.Show(); 
        //    }
        //}

        function grid_EndCallBack() {
            if (cGrdQuotation.cpEdit != null) {
                GetObjectID('hiddenedit').value = cGrdQuotation.cpEdit.split('~')[0];
                cProforma.SetText(cGrdQuotation.cpEdit.split('~')[1]);
                cCustomer.SetText(cGrdQuotation.cpEdit.split('~')[4]);
                var pro_status = cGrdQuotation.cpEdit.split('~')[2]
                //cGrdQuotation.cpEdit = null;
                if (pro_status != null) {
                    var radio = $("[id*=rbl_QuoteStatus] label:contains('" + pro_status + "')").closest("td").find("input");
                    radio.attr("checked", "checked");
                    //return false;
                    //$('#rbl_QuoteStatus[type=radio][value=' + pro_status + ']').prop('checked', true); 
                    cQuotationRemarks.SetText(cGrdQuotation.cpEdit.split('~')[3]);

                    cQuotationStatus.Show();
                }
            }
            if (cGrdQuotation.cpUpdate != null) {
                GetObjectID('hiddenedit').value = '';
                cProforma.SetText('');
                cCustomer.SetText('');
                cQuotationRemarks.SetText('');
                var pro_status = 2;
                if (pro_status != null) {
                    var radio = $("[id*=rbl_QuoteStatus] label:contains('" + pro_status + "')").closest("td").find("input");
                    radio.attr("checked", "checked");
                    cQuotationStatus.Hide();
                }
                jAlert(cGrdQuotation.cpUpdate);
            }
            if (cGrdQuotation.cpDelete != null) {
                jAlert(cGrdQuotation.cpDelete);
                cGrdQuotation.cpDelete = null;
            }


        }
        function SavePrpformaStatus() {
            if (document.getElementById('hiddenedit').value == '') {
                cGrdQuotation.PerformCallback('save~');
            }
            else {
                var checked_radio = $("[id*=rbl_QuoteStatus] input:checked");
                var status = checked_radio.val();
                var remarks = cQuotationRemarks.GetText();
                cGrdQuotation.PerformCallback('update~' + GetObjectID('hiddenedit').value + '~' + status + '~' + remarks);
            }

        }

        function OnMoreInfoClick(keyValue) {
            var ActiveUser = '<%=Session["userid"]%>'
            if (ActiveUser != null) {
                $.ajax({
                    type: "POST",
                    url: "purchaseinvoicelist.aspx/GetEditablePermission",
                    data: "{'ActiveUser':'" + ActiveUser + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var status = msg.d;
                        var url = 'PurchaseInvoice.aspx?key=' + keyValue + '&Permission=' + status + '&type=PB';
                        window.location.href = url;
                    }
                });
            }
        }

        ////##### coded by Samrat Roy - 04/05/2017   
        ////Add an another param to define request type 
        function OnViewClick(keyValue) {
            var url = 'PurchaseInvoice.aspx?key=' + keyValue + '&req=V' + '&type=PB';
            window.location.href = url;
        }


        function OnclickViewAttachment(obj) {
            var URL = '/OMS/Management/Activities/PurchaseInvoice_Document.aspx?idbldng=' + obj + '&type=PurchaseInvoice';
            window.location.href = URL;
        }

        function OnAddButtonClick() {
            var url = 'PurchaseInvoice.aspx?key=' + 'ADD';
            window.location.href = url;
        }
        var keyval;
        //function FocusedRowChanged(s, e) {
        //    keyval=s.GetRowKey(s.GetFocusedRowIndex());
        //}

        //var globalRowIndex;

        //function GetVisibleIndex(s, e) {
        //    globalRowIndex = e.visibleIndex;
        //}
        //RowClick = "GetVisibleIndex"

        // User Approval Status Start

        function GetApprovedQuoteId(s, e, itemIndex) { 
            var rowvalue = cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetApprovedRowValues);
            //var currentRow = cgridPendingApproval.GetRow(0);
            //var col1 = currentRow.find("td:eq(0)").html();

            cgridPendingApproval.PerformCallback('Status~' + rowvalue);
            cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetApprovedRowValues);

        }
        function OnGetApprovedRowValues(obj) { 
            uri = "PurchaseInvoice.aspx?key=" + obj + "&status=2" + '&type=PB';
            popup.SetContentUrl(uri);
            popup.Show();
            //window.location.href = uri;

        }

        function ch_fnApproved() { 
        }


        function GetRejectedQuoteId(s, e, itemIndex) {
            debugger;
            cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetRejectedRowValues);

        }
        function OnGetRejectedRowValues(obj) {
            uri = "PurchaseInvoice.aspx?key=" + obj + "&status=3" + '&type=PB';
            popup.SetContentUrl(uri);
            popup.Show();
        }

        // User Approval Status End

        function OnApprovalEndCall(s, e) {
           <%-- $.ajax({
                type: "POST",
                url: "purchaseinvoicelist.aspx/GetPendingCase",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#<%= lblWaiting.ClientID %>').text(data.d);
                }
            });--%>
        }
    </script>
    <style>
        strong label {
            font-weight: bold !important;
        }
        .statusBar 
         {
            display: none;
        }

        input[type="radio"] {
            webkit-transform: translateY(3px);
            -moz-transform: translateY(3px);
            transform: translateY(3px);
        }
        .blink {
          animation: blink-animation 1s steps(5, start) infinite;
          -webkit-animation: blink-animation 1s steps(5, start) infinite;
          cursor:pointer;
          color:#128AC9;
        }
        @keyframes blink-animation {
          to {
            visibility: hidden;
          }
        }
        @-webkit-keyframes blink-animation {
          to {
            visibility: hidden;
          }
        }
        .padTab>tbody>tr>td {
            padding-right:15px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
           <ClientSideEvents ControlsInitialized="AllControlInitilize" />
        </dxe:ASPxGlobalEvents>--%>
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Adjustment of Pending Documents</h3>
        </div>
    </div>
     <%--Code Added by Sam For Filteration Section Start--%>
    <table class="padTab">
           <tr>
               <td colspan="3"> 
                   <asp:RadioButtonList ID="rdl_Adjustment" runat="server" RepeatDirection="Horizontal" onchange="return selectValue();" Width="190px">
                                            <asp:ListItem Text="Adjusted" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Un-Adjusted" Value="1"  Selected="True"></asp:ListItem>
                                        </asp:RadioButtonList>
               </td>
               <td> <label> Type:</label></td>
               <td colspan="2"> 
                     <dxe:ASPxComboBox ID="ddl_type" runat="server" ClientInstanceName="cddl_type" Width="100%">
                         <Items>
                             <dxe:ListEditItem Text="Customer" Value="0" />
                             <dxe:ListEditItem Text="Vendor" Value="1" Selected="true" />
                          </Items>
                    </dxe:ASPxComboBox>
               </td>
           </tr>
            <tr>
                <td>
                    From Date</td>
                <td>
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" OnInit="FormDate_Init" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>
                    To Date
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="toDate" runat="server" OnInit="toDate_Init" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>

                </td>
                <td>Branch</td>
                <td>
                    <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                    </dxe:ASPxComboBox>
                </td>
                <td>
                    <input type="button" value="Show" class="btn btn-primary" onclick="updateGridByDate()" />
                    <%--<input type="button" value="Clear" class="btn btn-primary" onclick="ClearField()" />--%>
                </td>

            </tr>

        </table>
    <%--Code Added by Sam For Filteration Section Start--%>



    <%--<div class="form_main">
        <div class="clearfix">--%>
             <%--<% if (rights.CanAdd)
                                   { %>
            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-primary"><span> <u>A</u>dd New</span> 

            </a><%} %>--%>

            <%--<dxe:ASPxButton ID="btn_Approval" runat="server" class="btn btn-primary" Text="Pending Approval" ClientInstanceName="cbtn_Approval">
                <ClientSideEvents Click="function (s, e) {OpenPopUPApprovalStatus();}" />
            </dxe:ASPxButton>--%>
            <%--<% if (rights.CanExport)
                                               { %>
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>
             <% } %>--%>
           <%-- <span id="spanStatus" runat="server">
            <a href="javascript:void(0);" onclick="OpenPopUPUserWiseQuotaion()" class="btn btn-primary">
                    <span>My Purchase Invoice Status</span>
                                    
                </a>
                </span>
            <span id="divPendingWaiting" runat="server">
                <a href="javascript:void(0);" onclick="OpenPopUPApprovalStatus()" class="btn btn-primary">
                    <span>Approval Waiting</span>
                    <asp:Label ID="lblWaiting" runat="server" Text=""></asp:Label>                   
                </a>
                 <i class="fa fa-reply blink" style="font-size: 20px;margin-right: 10px;" aria-hidden="true"></i>
                
            </span>--%>
            
       <%-- </div>
        
    </div>--%>

     
    <div class="GridViewArea">
         <dxe:ASPxGridView ID="GrdQuotation" runat="server" KeyFieldName="id" AutoGenerateColumns="False" Settings-HorizontalScrollBarMode="Visible"
            Width="100%" ClientInstanceName="cGrdQuotation" OnCustomCallback="GrdQuotation_CustomCallback" Settings-VerticalScrollableHeight="300"
              Settings-VerticalScrollBarMode="Visible"  OnPageIndexChanged="GrdQuotation_PageIndexChanged" OnDataBinding="GrdQuotation_DataBinding" OnSummaryDisplayText="GrdQuotation_SummaryDisplayText">
            <Columns>
                   
               
                <dxe:GridViewDataTextColumn Caption="Doc Type" FieldName="type" VisibleIndex="0"  Width="170px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                 <dxe:GridViewDataTextColumn Caption="Doc NO" FieldName="docno" VisibleIndex="1" Width="140px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Doc Date" FieldName="docdate" VisibleIndex="2" Width="80px" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Branch" FieldName="branch" VisibleIndex="3" Width="140px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                  
                  
                 
                <dxe:GridViewDataTextColumn Caption="Balance Amt" FieldName="balanceAmt" VisibleIndex="4"  Width="80px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                 <dxe:GridViewDataTextColumn Caption="Created By" FieldName="createdby" VisibleIndex="5"  Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                 <dxe:GridViewDataTextColumn Caption="Created On" FieldName="CreatedDate" VisibleIndex="6"  Width="80px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                 
                <dxe:GridViewDataTextColumn Caption="Updated By" FieldName="modifiedby" VisibleIndex="7" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                
                    
		 

                <dxe:GridViewDataTextColumn Caption="Updated On" FieldName="ModifiedDate" VisibleIndex="8"  Width="80px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                 

<%--                <dxe:GridViewDataTextColumn Caption="Amount" FieldName="TotalAmount"   VisibleIndex="11"  Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>--%>
                
                

                <dxe:GridViewDataTextColumn FieldName="Action"  Caption="" VisibleIndex="12" Width="100px">
                                                                            <DataItemTemplate>
                                                                                <a href="javascript:void(0);" onclick="OnAddEditClick(this,'Show~'+'<%# Eval("id")%>' +'~'+'<%# Eval("type")%>'+'~'+'<%# Eval("balanceAmt")%>')">
                                                                                    <dxe:ASPxLabel ID="ASPxTextBox2" runat="server" Text='<%# Eval("Adjustment")%>' 
                                                                                        ToolTip="Click to Change Status">
                                                                                    </dxe:ASPxLabel>
                                                                                </a>
                                                                            </DataItemTemplate>
                                                                            <EditFormSettings Visible="False" />
                                                                            <CellStyle Wrap="False" CssClass="text-center">
                                                                            </CellStyle>
                                                                           <%-- <HeaderTemplate>
                                                                                Status
                                                                            </HeaderTemplate>--%>
                                                                            <HeaderStyle Wrap="False" CssClass="text-center" />
                                                                        </dxe:GridViewDataTextColumn>
               
                 
            </Columns>
            <ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack();}" />
             
            <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </SettingsPager>
            <SettingsSearchPanel Visible="True" />
            <Settings ShowFooter="true"  ShowGroupFooter="VisibleIfExpanded" ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsLoadingPanel Text="Please Wait..." />
             <TotalSummary>
                   <dxe:ASPxSummaryItem FieldName="balanceAmt" SummaryType="Sum" /> 
             </TotalSummary>
        </dxe:ASPxGridView>
        <asp:HiddenField ID="hiddenedit" runat="server" />
    </div>
    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" GridViewID="GrdQuotation" runat="server" Landscape="true" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>

    <%--DEBASHIS--%>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxDocumentsPopup" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">


                   <%-- <dxe:ASPxGridView runat="server" KeyFieldName="ID" ClientInstanceName="cgriddocuments" ID="grid_Documents"
                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridDocuments_CustomCallback" ClientSideEvents-EndCallback="cgridDocumentsEndCall"
                        Settings-ShowFooter="false" AutoGenerateColumns="False"
                        Settings-VerticalScrollableHeight="100" Settings-VerticalScrollBarMode="Hidden">
                                                      
                        <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                        <SettingsPager Visible="false"></SettingsPager>
                        <Columns>
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="10" Caption=" " />
                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="ID" Width="0" ReadOnly="true" Caption="No." CellStyle-CssClass="hide" HeaderStyle-CssClass="hide">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="NAME" Width="" ReadOnly="true" Caption="Design(s)">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:ASPxGridView>
                    <div class="text-center pTop10">
                        <dxe:ASPxButton ID="btnOK" ClientInstanceName="cbtnOK" runat="server"  AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior ="False">
                            <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind();}" />                            
                        </dxe:ASPxButton>
                    </div>--%>

                      <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" ClientSideEvents-EndCallback="cSelectPanelEndCall">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxCheckBox ID="selectOriginal" Text="Original For Recipient" runat="server" ToolTip="Select Original" ClientSideEvents-CheckedChanged="OrginalCheckChange"
                                    ClientInstanceName="CselectOriginal">
                                </dxe:ASPxCheckBox>

                                <dxe:ASPxCheckBox ID="selectDuplicate" Text="Duplicate For Transporter" runat="server" ToolTip="Select Duplicate" ClientSideEvents-CheckedChanged="DuplicateCheckChange"
                                    ClientInstanceName="CselectDuplicate">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectTriplicate" Text="Triplicate For Supplier" runat="server" ToolTip="Select Triplicate"
                                    ClientInstanceName="CselectTriplicate">
                                </dxe:ASPxCheckBox>

                                <dxe:ASPxComboBox ID="CmbDesignName" ClientInstanceName="cCmbDesignName" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">                                    
                                </dxe:ASPxComboBox>

                               <div class="text-center pTop10">
                                <dxe:ASPxButton ID="btnOK" ClientInstanceName="cbtnOK" runat="server"  AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior ="False">
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


     <div class="PopUpArea">

        <%--Client Wise Quotation Status Section Start--%>

        <dxe:ASPxPopupControl ID="Popup_QuotationStatus" runat="server" ClientInstanceName="cQuotationStatus"
            Width="500px" HeaderText="Approvers Configuration" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="col-md-12">
                                <table width="">
                                    <tr>
                                        <td style="padding-right: 8px; vertical-align: top">
                                            <strong>
                                                <label style="margin-bottom: 5px">Proforma:-</label></strong>
                                        </td>
                                        <td style="padding-top: 0; vertical-align: top; padding-right: 35px">
                                            <%--<dxe:ASPxTextBox ID="txt_Proforma" MaxLength="80" ClientInstanceName="cProforma" TabIndex="1" 
                                                runat="server" Width="100%"> 
                                            </dxe:ASPxTextBox>--%>
                                            <strong>
                                                <dxe:ASPxLabel ID="lbl_Proforma" runat="server" ClientInstanceName="cProforma" Text="ASPxLabel"></dxe:ASPxLabel>
                                            </strong>
                                        </td>
                                        <td style="padding-right: 8px; padding-left: 8px; vertical-align: top">
                                            <strong>
                                                <label style="margin-bottom: 5px">Customer:-</label></strong>
                                        </td>
                                        <td style="padding-top: 0; vertical-align: top">
                                            <%-- <dxe:ASPxTextBox ID="txt_Customer" ClientInstanceName="cCustomer"  runat="server" MaxLength="100" TabIndex="2"
                                            Width="100%"> 
                                        </dxe:ASPxTextBox>--%>
                                            <strong>
                                                <dxe:ASPxLabel ID="lbl_Customer" runat="server" ClientInstanceName="cCustomer" Text="ASPxLabel"></dxe:ASPxLabel>
                                            </strong>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="col-md-6">
                            </div>
                            <div class="col-md-6">
                            </div>
                            <div class="clear"></div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <table>
                            <tr>
                                <td style="width: 70px; padding: 13px 0;"><strong>Status </strong></td>
                                <td>
                                    <asp:RadioButtonList ID="rbl_QuoteStatus" runat="server" Width="172px" CssClass="mTop5" RepeatDirection="Horizontal">

                                        <asp:ListItem Text="Accepted" Value="2" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Declined" Value="3"></asp:ListItem>
                                    </asp:RadioButtonList></td>
                            </tr>
                        </table>





                    </div>
                    <div class="clear"></div>
                    <div class="col-md-12">

                        <div class="" style="margin-bottom: 5px;">
                            <strong>Reason </strong>
                        </div>

                        <div>
                            <dxe:ASPxMemo ID="txt_QuotationRemarks" runat="server" ClientInstanceName="cQuotationRemarks" Height="71px" Width="100%"></dxe:ASPxMemo>
                        </div>
                    </div>

                    <div class="col-md-12" style="padding-top: 10px;">
                        <dxe:ASPxButton ID="btn_PrpformaStatus" CausesValidation="true" ClientInstanceName="cbtn_PrpformaStatus" runat="server"
                            AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                            <ClientSideEvents Click="function (s, e) {SavePrpformaStatus();}" />
                        </dxe:ASPxButton>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>

        <%--Client Wise Quotation Status Section END--%>

       <%-- Sandip Approval Dtl Section Start--%>


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
                            <dxe:ASPxGridView ID="gridPendingApproval" runat="server" KeyFieldName="ID" AutoGenerateColumns="False"  
                                Width="100%" ClientInstanceName="cgridPendingApproval"   OnCustomCallback="gridPendingApproval_CustomCallback">
                                <Columns>
                                    <dxe:GridViewDataTextColumn Caption="Purchase Invoice No." FieldName="Number"
                                        VisibleIndex="0" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Date" FieldName="CreateDate"
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
                                               <%-- <ClientSideEvents CheckedChanged="function (s, e) {ch_fnApproved();}" />--%>
                                            </dxe:ASPxCheckBox>
                                        </DataItemTemplate>
                                        <Settings ShowFilterRowMenu="False" AllowFilterBySearchPanel="False" AllowAutoFilter="False" />
                                    </dxe:GridViewDataCheckColumn>

                                    <dxe:GridViewDataCheckColumn UnboundType="Boolean" Caption="Rejected">
                                        <DataItemTemplate>
                                            <dxe:ASPxCheckBox ID="chkreject" runat="server" AllowGrayed="false" OnInit="chkreject_Init" ValueType="System.Boolean" ValueChecked="true" ValueUnchecked="false">
                                            <%-- <ClientSideEvents CheckedChanged="function (s, e) {ch_fnApproved();}" />--%>
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
                                <%--<ClientSideEvents EndCallback="OnApprovalEndCall" />--%>
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
            Width="900px" HeaderText="User Wise Purchase Invoice Status" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div class="row">
                        <div class="col-md-12">
                            <dxe:ASPxGridView ID="gridUserWiseQuotation" runat="server" KeyFieldName="ID" AutoGenerateColumns="False" 
                                Width="100%" ClientInstanceName="cgridUserWiseQuotation"  OnCustomCallback="gridUserWiseQuotation_CustomCallback">
                                <Columns>
                                    <dxe:GridViewDataTextColumn Caption="Purchase Invoice No." FieldName="number"
                                        VisibleIndex="0" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Approval User" FieldName="approvedby"
                                        VisibleIndex="1" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn> 
                                   
                                    <dxe:GridViewDataTextColumn Caption="User Level" FieldName="UserLevel"
                                        VisibleIndex="2" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                     <dxe:GridViewDataTextColumn Caption="Status" FieldName="status"
                                        VisibleIndex="3" FixedStyle="Left">
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

         <%-- Sandip Approval Dtl Section End--%>
        
    </div>

    <%--Product Name Detail Invoice Wise--%>
<dxe:ASPxPopupControl ID="productpopup" ClientInstanceName="cproductpopup" runat="server"
AllowDragging="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="Product Detail"
EnableHotTrack="False" BackColor="#DDECFE" Width="400px" CloseAction="CloseButton">
<ContentCollection>
<dxe:PopupControlContentControl runat="server">
<dxe:ASPxCallbackPanel ID="propanel" runat="server" Width="1200px" ClientInstanceName="popproductPanel"
    OnCallback="propanel_Callback" >
    <PanelCollection>
        <dxe:PanelContent runat="server">
                <div class="col-md-2">
                    <label >Doc Number</label>
                    <asp:TextBox ID="txt_docno" runat="server" Width="100%" MaxLength="50" Enabled="false">                             
                                        </asp:TextBox>

                    </div>
                <div class="col-md-2">
                    <label>Doc Date</label>
                    <dxe:ASPxDateEdit ID="dt_docdate" runat="server" OnInit="FormDate_Init" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cDocDate" Width="100%" Enabled="false">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                 </div>
                <div class="col-md-2">
                    <label>Branch</label>
                     <asp:TextBox ID="txt_docbranch" runat="server" Width="100%" MaxLength="50" Enabled="false">                             
                                        </asp:TextBox>
                </div>
                <div class="col-md-2">
                    <label id="lbltype" runat="server"></label>
                     <dxe:ASPxComboBox ID="ddl_custVend" runat="server" ClientInstanceName="cddl_custVend" Width="100%" Enabled="false">
                    </dxe:ASPxComboBox>
                </div>
                 <div class="col-md-2">
                    <label > Select Document</label>
                     <dxe:ASPxComboBox ID="ddl_Document" runat="server" ClientInstanceName="cddl_Document" Width="100%">
                         <Items>
                             <dxe:ListEditItem Text="Purchase Bill" Value="PB"  Selected="true"/>
                             <dxe:ListEditItem Text="Vendor Debit Note" Value="VDN"  />
                         </Items>
                         <%--<ClientSideEvents SelectedIndexChanged="DocumentChange" />--%>
                    </dxe:ASPxComboBox>
                </div>
                <div class="col-md-2">
                    <label>Amount for Adjustment</label>
                     <asp:TextBox ID="txt_AdjusttedAmt"   runat="server" Width="100%" MaxLength="50" Enabled="false">                             
                                        </asp:TextBox>
                </div>
              
            <div class="clear"></div>
          <%--  OnHtmlRowCreated="aspxGridTax_HtmlRowCreated"--%>
             <div class="col-md-12">
                    <dxe:ASPxGridView ID="aspxGridTax" runat="server"  ClientInstanceName="cgridTax"   KeyFieldName="id" Width="100%" SettingsBehavior-AllowSort="false" 
                        SettingsBehavior-AllowDragDrop="false" 
                                        SettingsPager-Mode="ShowAllRecords" Settings-ShowFooter="false" AutoGenerateColumns="False" 
                                        OnCellEditorInitialize="aspxGridTax_CellEditorInitialize" 
                                          OnCustomCallback="cgridTax_CustomCallback" OnBatchUpdate="taxgrid_BatchUpdate"
                                        OnRowInserting="taxgrid_RowInserting" OnRowUpdating="taxgrid_RowUpdating" OnRowDeleting="taxgrid_RowDeleting"
                              OnSummaryDisplayText="aspxGridTax_SummaryDisplayText">
                                    
                                    <Columns>
                                        <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="type" ReadOnly="true" Caption="Doc Type">
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="docno" ReadOnly="true" Caption="Doc Number">
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="docdate" ReadOnly="true" Caption="Doc Date">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                <%--<MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />--%>
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <%--<dxe:GridViewDataComboBoxColumn Caption="percentage" FieldName="TaxField" VisibleIndex="2">
                                                <PropertiesComboBox ClientInstanceName="cTaxes_Name" ValueField="Taxes_ID" TextField="Taxes_Name" DropDownStyle="DropDown">
                                                    <ClientSideEvents SelectedIndexChanged="cmbtaxCodeindexChange"
                                                        GotFocus="CmbtaxClick" />
                                                </PropertiesComboBox>
                                            </dxe:GridViewDataComboBoxColumn>--%>
                                        <dxe:GridViewDataTextColumn Caption="Branch Name" FieldName="branch" VisibleIndex="4" ReadOnly="true">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                               <%-- <ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />--%>
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="balanceAmt" Caption="Unpaid Amount" ReadOnly="true">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                <%--<ClientSideEvents LostFocus="taxAmountLostFocus" GotFocus="taxAmountGotFocus" />--%>
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                        <%--GotFocus="adjustedAmountGotFocus"--%>
                                        <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="AdjustedAmt" Caption="Adjusted Amount" ReadOnly="true">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                <ClientSideEvents LostFocus="adjustedAmountLostFocus"   />
                                                <%--GotFocus="taxAmountGotFocus"--%>
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                     <Settings ShowFooter="true"  ShowGroupFooter="VisibleIfExpanded" VerticalScrollableHeight="150" VerticalScrollBarMode="Auto"></Settings>
                                    <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                                    <SettingsPager Visible="false"></SettingsPager>
                                    <Styles>
                                       <StatusBar CssClass="statusBar">
                                      </StatusBar>
                                   </Styles>
                                    <SettingsDataSecurity AllowEdit="true" />
                                    <SettingsEditing Mode="Batch">
                                        <BatchEditSettings EditMode="row" />
                                    </SettingsEditing>
                                    <ClientSideEvents EndCallback=" cgridTax_EndCallBack " RowClick="GetTaxVisibleIndex" />
                                 <TotalSummary>
                                       <dxe:ASPxSummaryItem FieldName="AdjustedAmt" SummaryType="Sum" /> 
                                 </TotalSummary>

                                </dxe:ASPxGridView>
             </div>
                <%--<div>
                    <dxe:ASPxGridView ID="grdproduct" runat="server" KeyFieldName="id" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="cpbproduct">
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Product Name" FieldName="product" HeaderStyle-CssClass="text-center"
                                    VisibleIndex="0" FixedStyle="Left" Width="150px">
                                    <CellStyle CssClass="text-center" Wrap="true" >
                                    </CellStyle>
                                    <Settings AutoFilterCondition="Contains"  />
                                </dxe:GridViewDataTextColumn>
                                </Columns>
                        </dxe:ASPxGridView>
                </div>--%>
        </dxe:PanelContent>
    </PanelCollection> 
</dxe:ASPxCallbackPanel>
</dxe:PopupControlContentControl>
</ContentCollection>
<HeaderStyle HorizontalAlign="Left">
<Paddings PaddingRight="6px" />
</HeaderStyle>
<SizeGripImage Height="16px" Width="16px" />
<CloseButtonImage Height="12px" Width="13px" />
<ClientSideEvents CloseButtonClick="function(s, e) {
popup.Hide();
}" />
</dxe:ASPxPopupControl>
     <%--Product Name Detail Invoice Wise--%>

</asp:Content>
