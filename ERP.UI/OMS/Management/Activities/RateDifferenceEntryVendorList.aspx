<%--=======================================================Revision History=====================================================    
    1.0   Pallab    V2.0.38   09-05-2023      26067: Rate Difference Entry - Vendor module design modification & check in small device
    2.0   Sanchita  V2.0.43   15-02-2024      27249: Views to be converted to Procedures in the Listing Page - Rate Difference Entry Vendor 
=========================================================End Revision History===================================================--%>

<%@ Page Language="C#" AutoEventWireup="true"   MasterPageFile="~/OMS/MasterPage/ERP.Master" CodeBehind="RateDifferenceEntryVendorList.aspx.cs" Inherits="ERP.OMS.Management.Activities.RateDifferenceEntryVendorList" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
     Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style>

        .fwidth{width:350px !important;}

           .padTabtype2 > tbody > tr > td {
            padding: 0px 5px;
        }

            .padTabtype2 > tbody > tr > td > label {
                margin-bottom: 0 !important;
                margin-right: 15px;
            }
      
    </style>
       <%--Subhra--%>
    <script>
        // Rev 2.0
        function CallbackPanelEndCall(s, e) {
            cGrdPurchaseReturn.Refresh();
        }
        // End of Rev 2.0
        var ReturnId = 0;
        function onPrintJv(id) {
            // debugger;
            ReturnId = id;
            cSelectPanel.cpSuccess = "";
            cDocumentsPopup.Show();
            cCmbDesignName.SetSelectedIndex(0);
            cSelectPanel.PerformCallback('Bindalldesignes');
            $('#btnOK').focus();
        }

        function PerformCallToGridBind() {
            cSelectPanel.PerformCallback('Bindsingledesign');
            cDocumentsPopup.Hide();
            return false;
        }
        function cSelectPanelEndCall(s, e) {
            debugger;
            if (cSelectPanel.cpSuccess != "") {
                var TotDocument = cSelectPanel.cpSuccess.split(',');
                var reportName = cCmbDesignName.GetValue();
                var module = 'RateDiff_Entry_Vend';
                if (TotDocument.length > 0) {
                    for (var i = 0; i < TotDocument.length; i++) {
                        if (TotDocument[i] != "") {
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + ReturnId + '&PrintOption=' + TotDocument[i], '_blank')
                        }
                    }
                }
            }
            if (cSelectPanel.cpSuccess == "") {
                if (cSelectPanel.cpChecked != "") {
                    jAlert('Please check Original For Recipient and proceed.');
                }
                CselectOriginal.SetCheckState('UnChecked');
                CselectDuplicate.SetCheckState('UnChecked');
                CselectTriplicate.SetCheckState('UnChecked');
                cCmbDesignName.SetSelectedIndex(0);
            }
        }


    </script>
    <%--Subhra--%>
    <script>
        //function onPrintJv(id) {
        //    window.location.href = "../../reports/XtraReports/Viewer/PurchaseReturnReportViewer.aspx?id=" + id
        //    ////window.location.href = "../../reports/XtraReports/Viewer/TaxInvoiceReportViewer.aspx?id=" + id

        //}


        function OnclickViewAttachment(obj) {
            //var URL = '/OMS/Management/Activities/PurchaseReturn_Document.aspx?idbldng=' + obj + '&type=PurchaseReturn';
            var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=RateDifferenceEntryVendor';
            window.location.href = URL;
        }
        document.onkeydown = function (e) {
            if (event.keyCode == 18) isCtrl = true;


            if (event.keyCode == 65 && isCtrl == true) { //run code for alt+a -- ie, Add

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
            var url = 'RateDifferenceEntryVendor.aspx?key=' + 'ADD';
            window.location.href = url;
        }
        function OnMoreInfoClick(keyValue) {
            var ActiveUser = '<%=Session["userid"]%>'
            if (ActiveUser != null) {
                $.ajax({
                    type: "POST",
                    url: "RateDifferenceEntryVendorList.aspx/GetEditablePermission",
                    data: "{'ActiveUser':'" + ActiveUser + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var status = msg.d;
                        var url = 'RateDifferenceEntryVendor.aspx?key=' + keyValue + '&Permission=' + status + '&type=PR';
                        window.location.href = url;
                    }
                });
            }
        }

        ////##### coded by Samrat Roy - 17/04/2017 - ref IssueLog(P Order - 70) 
        ////Add an another param to define request type 
        function OnViewClick(keyValue) {
            var url = 'RateDifferenceEntryVendor.aspx?key=' + keyValue + '&req=V' + '&type=RDEV';
            window.location.href = url;
        }

        function OnClickDelete(keyValue) {
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    cGrdPurchaseReturn.PerformCallback('Delete~' + keyValue);
                }
            });
        }
        function OnEndCallback(s, e) {

            if (cGrdPurchaseReturn.cpDelete != null) {
                jAlert(cGrdPurchaseReturn.cpDelete);

                cGrdPurchaseReturn.cpDelete = null;
                cGrdPurchaseReturn.Refresh();
                //  window.location.href = "PReturnList.aspx";
            }
        }
        //function OnClickDelete(keyValue) {
        //    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
        //        if (r == true) {
        //            cGrdQuotation.PerformCallback('Delete~' + keyValue);
        //        }
        //    });
        //}

        var isFirstTime = true;

        function AllControlInitilize() {
            if (isFirstTime) {
                if (localStorage.getItem('ReturnList_FromDate')) {
                    var fromdatearray = localStorage.getItem('ReturnList_FromDate').split('-');
                    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                    cFormDate.SetDate(fromdate);
                }

                if (localStorage.getItem('ReturnList_ToDate')) {
                    var todatearray = localStorage.getItem('ReturnList_ToDate').split('-');
                    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                    ctoDate.SetDate(todate);
                }

                if (localStorage.getItem('ReturnList_Branch')) {
                    if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('ReturnList_Branch'))) {
                        ccmbBranchfilter.SetValue(localStorage.getItem('ReturnList_Branch'));
                    }

                }

                //if ($("#LoadGridData").val() == "ok")
                //    updateGridByDate();
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
                localStorage.setItem("ReturnList_FromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("ReturnList_ToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("ReturnList_Branch", ccmbBranchfilter.GetValue());
                $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                $("#hfBranchID").val(ccmbBranchfilter.GetValue());
                $("#hfIsFilter").val("Y");
                // Rev 2.0
               //cGrdPurchaseReturn.Refresh();
                cCallbackPanel.PerformCallback("");
               // End of Rev 2.0
                //cGrdPurchaseReturn.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
            }
        }
    </script>

    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 0;
        }

        #Grid_PurchaseChallan {
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
            right: 10px;
        }

        .panel-title h3
        {
            padding-top: 0px !important;
        }
        
    </style>
    <%--Rev end 1.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Rate Difference Entry - Vendor</h3>
        </div>
    </div>
        <div class="form_main">
        <div class="clearfix">
             <% if (rights.CanAdd)
                                   { %>
            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success"><span><u>A</u>dd New</span> </a><%} %>

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

             <table class="padTabtype2 pull-right" id="gridFilter">
                        <tr>
                            <td>
                                <label>From Date</label></td>
                            <%--Rev 1.0: "for-cust-icon" class add --%>
                            <td class="for-cust-icon">
                                <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                                    <buttonstyle width="13px">
                        </buttonstyle>
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
                                    <buttonstyle width="13px">
                        </buttonstyle>
                                </dxe:ASPxDateEdit>
                                <%--Rev 1.0--%>
                                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                <%--Rev end 1.0--%>
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
    </div>
     <%--  SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true" SettingsCookies-StoreFiltering="true" 
            SettingsCookies-StoreGroupingAndSorting="true" --%>
        <div class="GridViewArea">
        <dxe:ASPxGridView ID="GrdPurchaseReturn" runat="server" KeyFieldName="SrlNo" AutoGenerateColumns="False"  
            Width="100%" ClientInstanceName="cGrdPurchaseReturn" OnCustomCallback="GrdPurchaseReturn_CustomCallback" 
          SettingsBehavior-AllowFocusedRow="true" 
           DataSourceID="EntityServerModeDataSource" >
            <Columns>
                <dxe:GridViewDataTextColumn Caption="Document Number" FieldName="SalesReturnNo"
                    VisibleIndex="0" FixedStyle="Left" Width="170px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                 <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="Return_Date" Caption="Posting Date" SortOrder="Descending" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                                               <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                               <EditFormSettings Visible="True"></EditFormSettings>
                                           </dxe:GridViewDataTextColumn>

                

                 <%--<dxe:GridViewDataTextColumn Caption="Date" FieldName="Return_Date"
                    VisibleIndex="1" FixedStyle="Left" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>--%>
                 <dxe:GridViewDataTextColumn Caption="Purchase Invoice Number(s)" FieldName="PurchaseInvoice"
                    VisibleIndex="2" FixedStyle="Left" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Vendor" FieldName="CustomerName"
                    VisibleIndex="3" FixedStyle="Left"  Width="210px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Amount" FieldName="Amount"
                    VisibleIndex="4" FixedStyle="Left" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>               
               
                  <dxe:GridViewDataTextColumn VisibleIndex="5" Caption="Project Name" FieldName="Proj_Name" Width="120px">
                        <CellStyle CssClass="gridcellleft" Wrap="True"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="15%">
                    <DataItemTemplate>
                         <% if (rights.CanView)
                            { %>
                        <a href="javascript:void(0);" onclick="OnViewClick('<%#Eval("Return_Id")%>')" class="pad" title="View">
                            <img src="../../../assests/images/doc.png" /></a>
                           <% } %>
                        <% if (rights.CanEdit)
                                       { %>
                        <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%#Eval("Return_Id")%>')" class="pad" title="Edit">
                            <img src="../../../assests/images/info.png" /></a><%} %>
                        <% if (rights.CanDelete)
                                       { %>
                        <a href="javascript:void(0);" onclick="OnClickDelete('<%#Eval("Return_Id")%>')" class="pad" title="Delete">
                            <img src="../../../assests/images/Delete.png" /></a><%} %>
                        <%-- <a href="javascript:void(0);" onclick="OnClickCopy('<%# Container.KeyValue %>')" class="pad" title="Copy ">
                            <i class="fa fa-copy"></i></a>--%>
                      <%--   <% if (rights.CanEdit)
                                       { %>
                        <a href="javascript:void(0);" onclick="OnClickStatus('<%# Container.KeyValue %>')" class="pad" title="Status">
                            <img src="../../../assests/images/verified.png" /></a><%} %>--%>
                         <% if (rights.CanView)
                                       { %>
                        <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%#Eval("Return_Id")%>')" class="pad" title="View Attachment">
                            <img src="../../../assests/images/attachment.png" />
                        </a><%} %>
                        <% if (rights.CanPrint)
                           { %>
                        <a href="javascript:void(0);" onclick="onPrintJv('<%#Eval("Return_Id")%>')" class="pad" title="print">
                            <img src="../../../assests/images/Print.png" />
                        </a><%} %>
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <HeaderTemplate><span>Actions</span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                </dxe:GridViewDataTextColumn>
            </Columns>
            <SettingsCookies Enabled="true" StorePaging="true"  Version="2.0" />
            <ClientSideEvents EndCallback="OnEndCallback" />
            <%--<SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </SettingsPager>--%>
            <settingspager pagesize="10">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200"/>
                              </settingspager>
          <%--  <SettingsSearchPanel Visible="True" />--%>
            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsLoadingPanel Text="Please Wait..." />
        </dxe:ASPxGridView>
          <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="v_RateDifferenceEntryVendorList" />
        <asp:HiddenField ID="hiddenedit" runat="server" />
    </div>
    </div>
    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" GridViewID="GrdPurchaseReturn" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>



     <%--SUBHRA--%>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" ClientSideEvents-EndCallback="cSelectPanelEndCall">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
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
      <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>

     <div>
        <asp:HiddenField ID="hfIsFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfBranchID" runat="server" />
    </div>

    <%-- Rev 2.0 --%>
    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">           
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>
    <%-- End of Rev 2.0 --%>
</asp:Content>
