<%@ Page Title="Sales Invoice for Transporter" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" 
    AutoEventWireup="true" CodeBehind="SalesInvoiceforTransporterList.aspx.cs" Inherits="ERP.OMS.Management.Activities.SalesInvoiceforTransporterList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--Code Added By Sandip For Approval Detail Section Start--%>
    <script>
        //function onPrintJv(id) {
        //    window.location.href = "../../reports/XtraReports/Viewer/OrderReportViewer.aspx?id=" + id;
        //}

        //This function is called to show the Status of All Sales Order Created By Login User Start
        function OpenPopUPUserWiseQuotaion() {
            cgridUserWiseQuotation.PerformCallback();
            cPopupUserWiseQuotation.Show();
        }
        // function above  End 

        //This function is called to show all Pending Approval of Sales Order whose Userid has been set LevelWise using Approval Configuration Module 
        function OpenPopUPApprovalStatus() {
            cgridPendingApproval.PerformCallback();
            cpopupApproval.Show();
        }
        // function above  End


        // Status 2 is passed If Approved Check box is checked by User Both Below function is called and used to show in POPUP,  the Add Page of Respective Segment(like Page for Adding Quotation ,Sale Order ,Challan)
        function GetApprovedQuoteId(s, e, itemIndex) {
            var rowvalue = cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetApprovedRowValues);
            //cgridPendingApproval.PerformCallback('Status~' + rowvalue);
            //cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetApprovedRowValues);

        }
        function OnGetApprovedRowValues(obj) {
            uri = "SalesInvoiceforTransporter.aspx?key=" + obj + "&status=2" + '&type=SI';
            popup.SetContentUrl(uri);
            popup.Show();
        }
        // function above  End For Approved

        // Status 3 is passed If Approved Check box is checked by User Both Below function is called and used to show in POPUP,  the Add Page of Respective Segment(like Page for Adding Quotation ,Sale Order ,Challan)
        function GetRejectedQuoteId(s, e, itemIndex) {
            debugger;
            cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetRejectedRowValues);

        }
        function OnGetRejectedRowValues(obj) {
            uri = "SalesInvoiceforTransporter.aspx?key=" + obj + "&status=3" + '&type=SI';
            popup.SetContentUrl(uri);
            popup.Show();
        }
        // function above  End For Rejected

        // To Reflect the Data in Pending Waiting Grid and Pending Waiting Counting if the user approve or Rejecte the Order and Saved 

        function OnApprovalEndCall(s, e) {
            $.ajax({
                type: "POST",
                url: "SalesInvoiceforTransporterList.aspx/GetPendingCase",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#<%= lblWaiting.ClientID %>').text(data.d);
                }
            });
            }

            // function above  End 

    </script>
    <%-- Code Added By Sandip For Approval Detail Section End--%>

    <script>



        var InvoiceId = 0;
        function onPrintJv(id) {
            //window.location.href = "../../reports/XtraReports/Viewer/InvoiceReportViewer.aspx?id=" + id
            //  window.location.href = "../../reports/XtraReports/Viewer/TaxInvoiceReportViewer.aspx?id=" + id
            debugger;
            InvoiceId = id;
            cSelectPanel.cpSuccess = "";
            cDocumentsPopup.Show();
            //cSelectPanel.PerformCallback('BindDocumentsDetails');
            //CselectDuplicate.SetEnabled(false);
            //CselectTriplicate.SetEnabled(false);
            CselectOriginal.SetCheckState('UnChecked');
            CselectDuplicate.SetCheckState('UnChecked');
            CselectTriplicate.SetCheckState('UnChecked');
            cCmbDesignName.SetSelectedIndex(0);
            //cComponentPanel.PerformCallback();
            cSelectPanel.PerformCallback('Bindalldesignes');
            $('#btnOK').focus();
        }

        //function OrginalCheckChange(s, e) {
        //    debugger;
        //    if (s.GetCheckState() == 'Checked') {
        //        CselectDuplicate.SetEnabled(true);
        //    }
        //    else {
        //        CselectDuplicate.SetCheckState('UnChecked');
        //        CselectDuplicate.SetEnabled(false);
        //        CselectTriplicate.SetCheckState('UnChecked');
        //        CselectTriplicate.SetEnabled(false);
        //    }

        //}
        //function DuplicateCheckChange(s, e) {
        //    if (s.GetCheckState() == 'Checked') {
        //        CselectTriplicate.SetEnabled(true);
        //    }
        //    else {
        //        CselectTriplicate.SetCheckState('UnChecked');
        //        CselectTriplicate.SetEnabled(false);
        //    }

        //}

        function PerformCallToGridBind() {
            //cgriddocuments.PerformCallback('BindDocumentsGridOnSelection' + '~' + InvoiceId);
            //cDocumentsPopup.Hide();
            //return false;
            cSelectPanel.PerformCallback('Bindsingledesign');
            cDocumentsPopup.Hide();
            return false;
        }

        //function cgridDocumentsEndCall(s,e)
        //{
        //    debugger;
        //    if (cgriddocuments.cpSuccess !=null)
        //    {
        //        var TotDocument = cgriddocuments.cpSuccess.split(',');
        //        if (TotDocument.length > 0)
        //        {
        //            for (var i = 0; i <TotDocument.length; i++) {
        //                if (TotDocument[i] != "") {
        //                    if (cCmbDesignName.GetValue() == 1)
        //                        {
        //                        window.open("../../reports/XtraReports/Viewer/InvoiceReportViewer.aspx?id=" + InvoiceId + '&PrintOption=' + TotDocument[i], '_blank')
        //                    }
        //                    else if (cCmbDesignName.GetValue() == 2)
        //                    {
        //                        window.open("../../reports/XtraReports/Viewer/TaxInvoiceReportViewer.aspx?id=" + InvoiceId + '&PrintOption=' + TotDocument[i], '_blank')
        //                    }
        //                }                       
        //            }                    
        //        }                
        //    }
        //}


        function cSelectPanelEndCall(s, e) {
            debugger;
            if (cSelectPanel.cpSuccess != "") {
                var TotDocument = cSelectPanel.cpSuccess.split(',');
                var reportName = cCmbDesignName.GetValue();
                var module = 'Invoice';
                if (TotDocument.length > 0) {
                    for (var i = 0; i < TotDocument.length; i++) {
                        if (TotDocument[i] != "") {
                            //if (cCmbDesignName.GetValue() == 1) {
                            //    window.open("../../reports/XtraReports/Viewer/InvoiceReportViewer.aspx?id=" + InvoiceId + '&PrintOption=' + TotDocument[i], '_blank')
                            //}
                            //else if (cCmbDesignName.GetValue() == 2) {
                            //    window.open("../../reports/XtraReports/Viewer/TaxInvoiceReportViewer.aspx?id=" + InvoiceId + '&PrintOption=' + TotDocument[i], '_blank')
                            //}
                            //else {
                            //    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + InvoiceId + '&PrintOption=' + TotDocument[i], '_blank')
                            //    //window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + InvoiceId + '&PrintOption=' + TotDocument[i], 'PrintingFrame')
                            //}
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + InvoiceId + '&PrintOption=' + TotDocument[i], '_blank')
                        }
                    }
                }
            }
            //cSelectPanel.cpSuccess = null
            if (cSelectPanel.cpSuccess == "") {
                if (cSelectPanel.cpChecked != "") {
                    jAlert('Please check Original For Recipient and proceed.');
                }
                //CselectDuplicate.SetEnabled(false);
                //CselectTriplicate.SetEnabled(false);
                CselectOriginal.SetCheckState('UnChecked');
                CselectDuplicate.SetCheckState('UnChecked');
                CselectTriplicate.SetCheckState('UnChecked');
                cCmbDesignName.SetSelectedIndex(0);
            }
        }

        function OnclickViewAttachment(obj) {
            //var URL = '/OMS/Management/Activities/SalesInvoice_Document.aspx?idbldng=' + obj + '&type=SalesInvoice';
            var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=SalesInvoice';
            window.location.href = URL;
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
        function OnAddButtonClick() {
            var url = 'SalesInvoiceforTransporter.aspx?key=' + 'ADD';
            window.location.href = url;
        }
        function OnMoreInfoClick(keyValue) {
            var ActiveUser = '<%=Session["userid"]%>'
            if (ActiveUser != null) {
                $.ajax({
                    type: "POST",
                    url: "SalesInvoiceforTransporterList.aspx/GetEditablePermission",
                    data: "{'ActiveUser':'" + ActiveUser + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var status = msg.d;
                        var url = 'SalesInvoiceforTransporter.aspx?key=' + keyValue + '&Permission=' + status + '&type=SI';
                        window.location.href = url;
                    }
                });
            }
        }

        ////##### coded by Samrat Roy - 04/05/2017  
        ////Add an another param to define request type 
        function OnViewClick(keyValue) {
            var url = 'SalesInvoiceforTransporter.aspx?key=' + keyValue + '&req=V' + '&type=SI';
            window.location.href = url;
        }

        function OnClickDelete(keyValue) {
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    cGrdQuotation.PerformCallback('Delete~' + keyValue);
                }
            });
        }
    </script>

    <script>
        function OpenPopUPApproveSO() {
            cgridApprovedSO.PerformCallback();
            cPOPUPApprovedSO.Show();
        }
    </script>

    <style>
        .smllpad > tbody > tr > td {
            padding-right: 25px;
        }

        .errorField {
            position: absolute;
            right: 5px;
            top: 9px;
        }

        .padTab {
            margin-bottom: 4px;
        }

            .padTab > tbody > tr > td {
                padding-right: 15px;
                vertical-align: middle;
            }

                .padTab > tbody > tr > td > label, .padTab > tbody > tr > td > input[type="button"] {
                    margin-bottom: 0 !important;
                }

        .backBranch {
            font-weight: 600;
            background: #75c1f5;
            padding: 5px;
        }
    </style>
    <script>
        var isFirstTime = true;

        function AllControlInitilize() {
            if (isFirstTime) {
                if (localStorage.getItem('InvoiceList_FromDate')) {
                    var fromdatearray = localStorage.getItem('InvoiceList_FromDate').split('-');
                    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                    cFormDate.SetDate(fromdate);
                }

                if (localStorage.getItem('InvoiceList_ToDate')) {
                    var todatearray = localStorage.getItem('InvoiceList_ToDate').split('-');
                    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                    ctoDate.SetDate(todate);
                }

                if (localStorage.getItem('InvoiceList_Branch')) {
                    if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('InvoiceList_Branch'))) {
                        ccmbBranchfilter.SetValue(localStorage.getItem('InvoiceList_Branch'));
                    }

                }

                updateGridByDate();
                isFirstTime = false;
            }
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
                localStorage.setItem("InvoiceList_FromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("InvoiceList_ToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("InvoiceList_Branch", ccmbBranchfilter.GetValue());

                cGrdQuotation.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
            }
        }
        function grid_EndCallBack(s, e) {
            if (cGrdQuotation.cpDelete) {
                var message = cGrdQuotation.cpDelete;
                cGrdQuotation.cpDelete = null;

                jAlert(message);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">Sales Invoice for Transporter</h3>
            <div id="pageheaderContent" class="scrollHorizontal pull-right wrapHolder content horizontal-images"><%--class="pull-right wrapHolder reverse content horizontal-images">--%>
                <div class="Top clearfix">
                    <ul>
                        <li>
                            <div class="lblHolder">
                                <table>
                                    <tr>
                                        <td>Customer Balance</td>
                                    </tr>
                                    <tr>
                                        <td class="lower">
                                            <asp:Label ID="lblAvailableDues" runat="server" Text="0.0"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
        <table class="padTab">
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
                <td>Unit</td>
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
    <div class="form_main">        
        <div class="clearfix">
            <% if (rights.CanAdd)
               { %>
            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-primary"><span><u>A</u>dd New</span> </a><%} %>

            <% if (rights.CanExport)
               { %>
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>
            <% } %>
            <%--Sandip Section for Approval Section in Design Start --%>

            <span id="spanStatus" runat="server">
                <a href="javascript:void(0);" onclick="OpenPopUPUserWiseQuotaion()" class="btn btn-primary">
                    <span>My Sales Invoice Status</span>
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
            <span id="span1" runat="server">
                <a href="javascript:void(0);" onclick="OpenPopUPApproveSO()" class="btn btn-primary">
                    <span>Approved Sales Order</span>
                </a>
            </span>

            <%--Sandip Section for Approval Section in Design End --%>
        </div>
    </div>
    <%--<dxe:ASPxCallbackPanel runat="server" id="ComponentPanel" ClientInstanceName="cComponentPanel" OnCallback="Component_Callback">

         <PanelCollection>

             <dxe:PanelContent runat="server">

             </dxe:PanelContent>
         </PanelCollection>
     </dxe:ASPxCallbackPanel>--%>
    <div class="GridViewArea">
        <dxe:ASPxGridView ID="GrdQuotation" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
            Width="100%" ClientInstanceName="cGrdQuotation" OnCustomCallback="GrdQuotation_CustomCallback" OnDataBinding="GrdQuotation_DataBinding"
            SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true" SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true"
            Settings-HorizontalScrollBarMode="Auto" SettingsDataSecurity-AllowDelete="false" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false">
            <SettingsSearchPanel Visible="True" Delay="5000" />
            <Columns>
                <dxe:GridViewDataTextColumn Caption="Document No." FieldName="InvoiceNo"
                    VisibleIndex="0" FixedStyle="Left" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="Invoice_Date"
                    VisibleIndex="1">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Sales Order No." FieldName="SalesOrder_Number"
                    VisibleIndex="2" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle><Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="SO Date" FieldName="SalesOrder_Date" Width="150px"
                    VisibleIndex="3">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle><Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Sales Challan No." FieldName="SalesChallan_Number" Width="150px"
                    VisibleIndex="4">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle><Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Challan Date" FieldName="SalesChallan_Date" Width="150px"
                    VisibleIndex="5">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle><Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Ageing Today" FieldName="AgeingDate"
                    VisibleIndex="6" Width="100px" Visible="false">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle><Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Transporter" FieldName="CustomerName"  Width="250px"
                    VisibleIndex="7">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle><Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Gross Amount" FieldName="GrossAmount"
                    VisibleIndex="8" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true"> 
                    </CellStyle><Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Tax & Charges" FieldName="ChargesAmount"
                    VisibleIndex="9" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle><Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Net Amount" FieldName="NetAmount"
                    VisibleIndex="10" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle><Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Amount Received" FieldName="AmountReceived"
                    VisibleIndex="11" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle><Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Balance Amount" FieldName="BalanceAmount"
                    VisibleIndex="12" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle><Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Place of Supply[GST]" FieldName="PosState"
                    VisibleIndex="12" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle><Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="13" Width="240px">
                    <DataItemTemplate>
                        <% if (rights.CanView)
                           { %>
                        <a href="javascript:void(0);" onclick="OnViewClick('<%# Container.KeyValue %>')" class="pad" title="View">
                            <img src="../../../assests/images/doc.png" /></a>
                        <% } %>
                        <% if (rights.CanEdit)
                           { %>
                        <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')" class="pad" title="Edit">
                            <img src="../../../assests/images/info.png" /></a><%} %>
                        <% if (rights.CanDelete)
                           { %>
                        <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="pad" title="Delete">
                            <img src="../../../assests/images/Delete.png" /></a><%} %>
                        <%-- <a href="javascript:void(0);" onclick="OnClickCopy('<%# Container.KeyValue %>')" class="pad" title="Copy ">
                            <i class="fa fa-copy"></i></a>--%>
                        <% if (rights.CanView)
                           { %>
                        <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%# Container.KeyValue %>')" class="pad" title="View Attachment">
                            <img src="../../../assests/images/attachment.png" />
                        </a><%} %>
                        <% if (rights.CanPrint)
                           { %>
                        <a href="javascript:void(0);" onclick="onPrintJv('<%# Container.KeyValue %>')" class="pad" title="print">
                            <img src="../../../assests/images/Print.png" />
                        </a><%} %>
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle><Settings AllowAutoFilterTextInputTimer="False" />
                    <HeaderTemplate><span>Actions</span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                </dxe:GridViewDataTextColumn>
            </Columns>
            <SettingsContextMenu Enabled="true"></SettingsContextMenu>
            <ClientSideEvents EndCallback="grid_EndCallBack" />
            <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </SettingsPager>
            <GroupSummary>
                <dxe:ASPxSummaryItem FieldName="GrossAmount" SummaryType="Sum" DisplayFormat="Total Gross Amount : {0}" />
                <dxe:ASPxSummaryItem FieldName="ChargesAmount" SummaryType="Sum" DisplayFormat="Total Tax & Charges : {0}" />
                <dxe:ASPxSummaryItem FieldName="NetAmount" SummaryType="Sum" DisplayFormat="Total Net Amount : {0}" />
                <dxe:ASPxSummaryItem FieldName="AmountReceived" SummaryType="Sum" DisplayFormat="Total Amount Received : {0}" />
                <dxe:ASPxSummaryItem FieldName="BalanceAmount" SummaryType="Sum" DisplayFormat="Total Balance Amount : {0}" />
            </GroupSummary>
           <%-- <SettingsSearchPanel Visible="True" />--%>
            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsLoadingPanel Text="Please Wait..." />
        </dxe:ASPxGridView>
        <asp:HiddenField ID="hiddenedit" runat="server" />
    </div>
    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" GridViewID="GrdQuotation" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
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
                    <%--   <dxe:ASPxGridView runat="server" KeyFieldName="ID" ClientInstanceName="cgriddocuments" ID="grid_Documents"
                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridDocuments_CustomCallback" ClientSideEvents-EndCallback="cgridDocumentsEndCall"
                        Settings-ShowFooter="false" AutoGenerateColumns="False"
                        Settings-VerticalScrollableHeight="100" Settings-VerticalScrollBarMode="Hidden">
                                                      
                        <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                        <SettingsPager Visible="false"></SettingsPager>
                        <Columns>
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="10" Caption=" "  />




                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="ID" Width="0" ReadOnly="true" Caption="No." CellStyle-CssClass="hide" HeaderStyle-CssClass="hide">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="NAME" Width="" ReadOnly="true" Caption="Design(s)">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                      
                    </dxe:ASPxGridView>--%>
                    <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" ClientSideEvents-EndCallback="cSelectPanelEndCall">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <%--   <dxe:ASPxCheckBox ID="selectOriginal" Text="Original" runat="server" ToolTip="Select Original"
                                    ClientInstanceName="CselectOriginal"  ClientSideEvents-CheckedChanged="function(s, e) { 
                                      grid.PerformCallback(s.GetChecked()+'^'+'stock'); }">
                                   </dxe:ASPxCheckBox>--%>

                                <%--<dxe:ASPxCheckBox ID="selectOriginal" Text="Original For Recipient" runat="server" ToolTip="Select Original" ClientSideEvents-CheckedChanged="OrginalCheckChange"
                                    ClientInstanceName="CselectOriginal">
                                </dxe:ASPxCheckBox>--%>

                                <%--<dxe:ASPxCheckBox ID="selectDuplicate" Text="Duplicate For Transporter" runat="server" ToolTip="Select Duplicate" ClientSideEvents-CheckedChanged="DuplicateCheckChange"
                                    ClientInstanceName="CselectDuplicate">
                                </dxe:ASPxCheckBox>--%>
                                <dxe:ASPxCheckBox ID="selectOriginal" Text="Original For Recipient" runat="server" ToolTip="Select Original" 
                                    ClientInstanceName="CselectOriginal">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectDuplicate" Text="Duplicate For Transporter" runat="server" ToolTip="Select Duplicate" 
                                    ClientInstanceName="CselectDuplicate">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectTriplicate" Text="Triplicate For Supplier" runat="server" ToolTip="Select Triplicate"
                                    ClientInstanceName="CselectTriplicate">
                                </dxe:ASPxCheckBox>

                                <dxe:ASPxComboBox ID="CmbDesignName" ClientInstanceName="cCmbDesignName" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                    <%-- <Items>
                                        <dxe:ListEditItem Selected="True" Text="Default" Value="1"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Tax_Invoice" Value="2"></dxe:ListEditItem>
                                    </Items>--%>
                                    <%-- <ClientSideEvents ValueChanged="function(s,e){OnCmbCountryName_ValueChange()}"></ClientSideEvents>--%>
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

    <%-- Sandip Approval Dtl Section Start--%>
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
                            <dxe:ASPxGridView ID="gridPendingApproval" runat="server" KeyFieldName="ID" AutoGenerateColumns="False"
                                Width="100%" ClientInstanceName="cgridPendingApproval" OnCustomCallback="gridPendingApproval_CustomCallback">
                                <Columns>
                                    <dxe:GridViewDataTextColumn Caption="Sale Document No." FieldName="Number"
                                        VisibleIndex="0" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="CreateDate"
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
                                                <%--<ClientSideEvents CheckedChanged="function (s, e) {ch_fnApproved();}" />--%>
                                            </dxe:ASPxCheckBox>
                                        </DataItemTemplate>
                                        <Settings ShowFilterRowMenu="False" AllowFilterBySearchPanel="False" AllowAutoFilter="False" />
                                    </dxe:GridViewDataCheckColumn>

                                    <dxe:GridViewDataCheckColumn UnboundType="Boolean" Caption="Rejected">
                                        <DataItemTemplate>
                                            <dxe:ASPxCheckBox ID="chkreject" runat="server" AllowGrayed="false" OnInit="chkreject_Init" ValueType="System.Boolean" ValueChecked="true" ValueUnchecked="false">
                                                <%--<ClientSideEvents CheckedChanged="function (s, e) {ch_fnApproved();}" />--%>
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
            Width="900px" HeaderText="User Wise Sales Invoice Status" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div class="row">
                        <div class="col-md-12">
                            <dxe:ASPxGridView ID="gridUserWiseQuotation" runat="server" KeyFieldName="ID" AutoGenerateColumns="False"
                                Width="100%" ClientInstanceName="cgridUserWiseQuotation" OnCustomCallback="gridUserWiseQuotation_CustomCallback">
                                <Columns>
                                    <dxe:GridViewDataTextColumn Caption="Sale Document No." FieldName="number"
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
    </div>

    <%-- Sandip Approval Dtl Section End--%>

    <%-- Approved Sales Order --%>

    <dxe:ASPxPopupControl ID="POPUPApprovedSO" runat="server" ClientInstanceName="cPOPUPApprovedSO"
        Width="900px" HeaderText="Approved Sales Order" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="row">
                    <div class="col-md-12">
                        <dxe:ASPxGridView ID="gridApprovedSO" runat="server" KeyFieldName="Order_Id" AutoGenerateColumns="False" OnDataBinding="gridApprovedSO_DataBinding"
                            Width="100%" ClientInstanceName="cgridApprovedSO" OnCustomCallback="gridApprovedSO_CustomCallback">
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="SO No." FieldName="Order_Number"
                                    VisibleIndex="0" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="SO Date" FieldName="Order_Date"
                                    VisibleIndex="1" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Approved By" FieldName="ApproverName"
                                    VisibleIndex="2" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Approved On" FieldName="ApprovedOn"
                                    VisibleIndex="3" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                 <dxe:GridViewDataTextColumn Caption="Customer Name" FieldName="CustomerName"
                                    VisibleIndex="4" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                 <dxe:GridViewDataTextColumn Caption="GSTIN" FieldName="GSTIN"
                                    VisibleIndex="5" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                 <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="EnteredBy"
                                    VisibleIndex="6" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                 <dxe:GridViewDataTextColumn Caption="Unit" FieldName="Unit"
                                    VisibleIndex="7" FixedStyle="Left">
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

    <%-- Approved Sales Order --%>

     <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
</asp:Content>
