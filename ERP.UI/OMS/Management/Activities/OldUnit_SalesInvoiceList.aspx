<%--=======================================================Revision History=====================================================    
    1.0   Pallab    V2.0.38   15-05-2023      26127: Second Hand Sales module design modification & check in small device
=========================================================End Revision History===================================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="OldUnit_SalesInvoiceList.aspx.cs" Inherits="ERP.OMS.Management.Activities.OldUnit_SalesInvoiceList" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
      <%--Subhra--%>
    <script>
        var SecondHandId = 0;
        var invoiceId;
        function onPrintJv(id) {
            debugger;
            SecondHandId = id;
            cSelectPanel.cpSuccess = "";
            cDocumentsPopup.Show();
            cCmbDesignName.SetSelectedIndex(0);
            cSelectPanel.PerformCallback('Bindalldesignes');
            $('#btnOK').focus();
        }
        function OnBeginAfterCallback(s, e) {
            IconChange();
        }
         
        $(document).ready(function () {
            IconChange(); 

        });
        function IconChange() {
            $(function () {

                var $tr = $("#GrdQuotation_DXMainTable > tbody > tr:gt(1)");

                $tr.each(function (index, value) {
                    var $ChallanNumber = $(this).find("td").eq(2).text();
                    var $AssignBranch = $(this).find("td").eq(11).text();
                    
                    var a_Assignment = $(this).find("td").eq(12).find("#a_Assignment");
                    var Cancel_Assignment = $(this).find("td").eq(12).find("#Cancel_Assignment");
                    var a_Edit = $(this).find("td").eq(12).find("#a_Edit");

                    if ($AssignBranch.trim() != '') {
                        a_Assignment.hide();
                        Cancel_Assignment.show();
                    } else {
                        a_Assignment.show();
                        Cancel_Assignment.hide();
                    }
                    
                    if ($ChallanNumber.trim() != '') {
                        Cancel_Assignment.hide();
                        a_Assignment.hide();
                        a_Edit.hide();
                    }

                });
            });
        }


        function onCancelBranchAssignment(inv) {
            cBranchRequUpdatePanel.PerformCallback('Cancel_Assignment~' + inv);
        }

        function onViewBranchAssignment(obj) {
            invoiceId = obj;
            cAssignmentPopUp.Show();
        }
        function AssignBranchToThisInvoice(){
            cBranchRequUpdatePanel.PerformCallback('AssignBranch~' + invoiceId);
        }
        function BranchRequUpdatePanelEndCallBack() {
            if (cBranchRequUpdatePanel.cpAssignUpdated) {
                if (cBranchRequUpdatePanel.cpAssignUpdated == 'y') {
                    jAlert("Branch Assigned Successfully.", 'Alert', function () { cAssignmentPopUp.Hide(); });
                    updateGridByDate();
                    cBranchRequUpdatePanel.cpAssignUpdated = null;
                }
                if (cBranchRequUpdatePanel.cpAssignUpdated == 'CancelAssign') {
                    jAlert("Assignment Cancel Successfully.");
                    updateGridByDate();
                    cBranchRequUpdatePanel.cpAssignUpdated = null;
                }
            }
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
                var module = 'Second_Hand';
                if (TotDocument.length > 0) {
                    for (var i = 0; i < TotDocument.length; i++) {
                        if (TotDocument[i] != "") {
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + SecondHandId + '&PrintOption=' + TotDocument[i], '_blank')
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
        var isFirstTime = true;

        function AllControlInitilize() {
            if (isFirstTime) {
                if (localStorage.getItem('Old_InvoiceList_FromDate')) {
                    var fromdatearray = localStorage.getItem('Old_InvoiceList_FromDate').split('-');
                    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                    cFormDate.SetDate(fromdate);
                }

                if (localStorage.getItem('Old_InvoiceList_ToDate')) {
                    var todatearray = localStorage.getItem('Old_InvoiceList_ToDate').split('-');
                    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                    ctoDate.SetDate(todate);
                }

                if (localStorage.getItem('Old_InvoiceList_Branch')) {
                    if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('Old_InvoiceList_Branch'))) {
                        ccmbBranchfilter.SetValue(localStorage.getItem('Old_InvoiceList_Branch'));
                    }

                }

                //updateGridByDate();
                isFirstTime = false;
            }
        }





        function ChallanClick(keyValue, vissibleInx) {
            cGridChallan.SetFocusedRowIndex(vissibleInx);
            var ActiveUser = '<%=Session["userid"]%>'
            if (ActiveUser != null) {
                $.ajax({
                    type: "POST",
                    url: "CustomerDeliveryPendingList.aspx/GetEditablePermission",
                    data: "{'ActiveUser':'" + ActiveUser + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,//Added By:Subhabrata
                    success: function (msg) {

                        var status = msg.d;
                        $.ajax({
                            type: "POST",
                            url: "CustomerDeliveryPendingList.aspx/GetChallanIdIsExistInSalesInvoice",
                            data: "{'keyValue':'" + keyValue + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,//Added By:Subhabrata
                            success: function (msg) {

                                var status1 = msg.d;
                                $.ajax({
                                    type: "POST",
                                    url: "CustomerDeliveryPendingList.aspx/GetCustomerId",
                                    data: "{'KeyVal':'" + keyValue + "'}",
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    async: false,//Added By:Subhabrata
                                    success: function (msg) {
                                        //debugger;
                                        var ID = msg.d;
                                        var Customer_Id = ID.split('~')[0];
                                        var BillDate = ID.split('~')[1];
                                        var ChallanDocNo = cGridChallan.GetRow(cGridChallan.GetFocusedRowIndex()).children[2].innerText;
                                        if (status1 == "1") {
                                            jAlert('Already Delivered.You can only view !', 'Confirmation Dialog', function (r) {
                                                if (r == true) {
                                                    //debugger;
                                                    var url = 'SalesChallanAdd.aspx?key=' + ChallanDocNo + '&type=SC' + '&DlvTyeId=' + '4';
                                                    window.location.href = url;
                                                }
                                            });
                                        }
                                        else {
                                            var url = 'SalesChallanAdd.aspx?key=' + keyValue + '&Permission=' + status + '&CustID=' + Customer_Id + '&Flag=' + 'SecondHandSale' + '&BillDate=' + BillDate;
                                            window.location.href = url;
                                        }



                                    }
                                });
                            }
                        });


                    }
                });
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
                localStorage.setItem("Old_InvoiceList_FromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("Old_InvoiceList_ToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("Old_InvoiceList_Branch", ccmbBranchfilter.GetValue());
                    if (page.activeTabIndex == 0) {
                        //cGrdQuotation.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
                        $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                        $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                        $("#hfBranchID").val(ccmbBranchfilter.GetValue());
                        $("#hfIsFilter").val("Y");
                        cGrdQuotation.Refresh();
                    }
                    else if (page.activeTabIndex == 1) {
                        cGridChallan.PerformCallback('bindchallan~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
                        
                    }

                }
        }

        function grid_EndCallBack(s, e) {
            IconChange();
            if (cGrdQuotation.cpDelete) {
                var message = cGrdQuotation.cpDelete;
                cGrdQuotation.cpDelete = null;
                jAlert(message);
                cGrdQuotation.Refresh();
            }
            
        }

        function OnViewClick(keyValue) {
            var url = 'SalesInvoice.aspx?key=' + keyValue + '&req=V' + '&type=SI';
            window.location.href = url;
        }

        function OnClickDelete(keyValue) {
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    cGrdQuotation.PerformCallback('Delete~' + keyValue);
                   
                }
            });
        }

        function OnAddButtonClick() {
            var url = 'OldUnit_SalesInvoice.aspx?key=' + 'ADD';
            window.location.href = url;
        }

        document.onkeydown = function (e) {
            if (event.keyCode == 18) isCtrl = true;
            if (event.keyCode == 65 && isCtrl == true) {
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

        function OnMoreInfoClick(keyValue) {
            var ActiveUser = '<%=Session["userid"]%>'
            if (ActiveUser != null) {
                $.ajax({
                    type: "POST",
                    url: "SalesInvoiceList.aspx/GetEditablePermission",
                    data: "{'ActiveUser':'" + ActiveUser + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var status = msg.d;
                        var url = 'OldUnit_SalesInvoice.aspx?key=' + keyValue + '&Permission=' + status + '&type=SI';
                        window.location.href = url;
                    }
                });
            }
        }
        function gridRowclick(s, e) {
            $('#GrdQuotation').find('tr').removeClass('rowActive');
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

    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 0;
        }

        #gridAdvanceAdj {
            max-width: 99% !important;
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
        }

        .panel-title h3
        {
            padding-top: 0px !important;
        }

        .fakeInput
        {
                min-height: 30px;
    border-radius: 4px;
        }
        
    </style>
    <%--Rev end 1.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">Second Hand Sales</h3>
        </div>
        <table class="padTab">
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
            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success "><span class="btn-icon"><i class="fa fa-plus" ></i></span>
<span><u>A</u>dd New</span> </a><%} %>

            <% if (rights.CanExport)
               { %>
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary " OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>
            <% } %>
        </div>
    </div>
    

        <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" ClientInstanceName="page"
        Font-Size="12px" Width="100%">
        <TabPages>
            <dxe:TabPage Name="SecondHandSale" Text="Second Hand Sale">
                <ContentCollection>
                    <dxe:ContentControl runat="server">

  

<div class="GridViewArea relative">
        <%--<dxe:ASPxGridView ID="GrdQuotation" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
            Width="100%" ClientInstanceName="cGrdQuotation" OnCustomCallback="GrdQuotation_CustomCallback" OnDataBinding="GrdQuotation_DataBinding"
            SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true" SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true"
            Settings-HorizontalScrollBarMode="Auto" >--%>
        <dxe:ASPxGridView ID="GrdQuotation" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
            Width="100%" ClientInstanceName="cGrdQuotation" OnCustomCallback="GrdQuotation_CustomCallback" 
            Settings-HorizontalScrollBarMode="Auto" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto" DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"  >
            <%--SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true" SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true" --%>
           <SettingsSearchPanel Visible="True" Delay="5000" />
             <ClientSideEvents BeginCallback="OnBeginAfterCallback"  />
            <Columns>
                <dxe:GridViewDataTextColumn Caption="Document No." FieldName="InvoiceNo" 
                    VisibleIndex="0" FixedStyle="Left" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="Invoice_Date" VisibleIndex="1"  >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                    <%--<PropertiesTextEdit DisplayFormatString="dd-MM-yyyy" 
                                DisplayFormatInEditMode="True"></PropertiesTextEdit>--%>
                </dxe:GridViewDataTextColumn>
                <%--<dxe:GridViewDataTextColumn Caption="Challan No" FieldName="SalesChallan_id" Width="0"
                    VisibleIndex="2" > 
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="SO Date" FieldName="SalesOrder_Date" Width="150px"
                    VisibleIndex="3">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>--%>
                <dxe:GridViewDataTextColumn Caption="Sales Challan No." FieldName="SalesChallan_Number" Width="150px"
                    VisibleIndex="4">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Challan Date" FieldName="SalesChallan_Date" Width="150px"
                    VisibleIndex="5">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Ageing Today" FieldName="AgeingDate"
                    VisibleIndex="6" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Customer" FieldName="CustomerName" Width="150px"
                    VisibleIndex="7">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Gross Amount" FieldName="GrossAmount"
                    VisibleIndex="8" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Tax & Charges" FieldName="ChargesAmount"
                    VisibleIndex="9" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Net Amount" FieldName="NetAmount"
                    VisibleIndex="10" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Amount Received" FieldName="AmountReceived"
                    VisibleIndex="11" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Balance Amount" FieldName="BalanceAmount"
                    VisibleIndex="12" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Assigned Unit" FieldName="assignedBranch"
                    VisibleIndex="12" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="13" Width="0">
                    <DataItemTemplate>

                        <div class='floatedBtnArea'>
                        <% if (rights.CanView)
                           { %>
                        <a href="javascript:void(0);" onclick="OnViewClick('<%# Container.KeyValue %>')" class="" title="">
                            <span class='ico ColorFive'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                        <% } %>
                        <% if (rights.CanEdit)
                           { %>
                        <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')" id="a_Edit" class="" title="">
                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a><%} %>

                    <%--    <a href="javascript:void(0);" onclick="ChallanClick('<%# Container.KeyValue %>',<%# Container.VisibleIndex %>)" class="pad" title="Create Delivery Challan">
                            
                            <i class="fa fa-truck out" ></i></a>--%>


                        <% if (rights.CanDelete)
                           { %>
                        <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="" title="">
                            <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a><%} %>
                         
                         <% if (rights.CanPrint)
                           { %>
                        <a href="javascript:void(0);" onclick="onPrintJv('<%# Container.KeyValue %>')" class="" title="">
                            <span class='ico ColorSeven'><i class='fa fa-print'></i></span><span class='hidden-xs'>Print</span>
                        </a><%} %>

                         <a href="javascript:void(0);" class="" title="" id="a_Assignment"  onclick="onViewBranchAssignment('<%# Container.KeyValue %>')" >
                                                <span class='ico ColorSix'><i class='fa fa-truck out'></i></span><span class='hidden-xs'>Branch Assignment</span>
                                            </a>

                          <a href="javascript:void(0);" class="" title="" id="Cancel_Assignment" onclick="onCancelBranchAssignment('<%# Container.KeyValue %>')" style="display: none;">
                                                <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Cancel Branch Assignment</span>
                                            </a>


                       <%-- <% if (rights.CanView)
                           { %>
                        <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%# Container.KeyValue %>')" class="pad" title="View Attachment">
                            <img src="../../../assests/images/attachment.png" />
                        </a><%} %>
                        <% if (rights.CanPrint)
                           { %>
                        <a href="javascript:void(0);" onclick="onPrintJv('<%# Container.KeyValue %>')" class="pad" title="print">
                            <img src="../../../assests/images/Print.png" />
                        </a><%} %>--%>
                       </div>
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <HeaderTemplate><span>Actions</span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                </dxe:GridViewDataTextColumn>
            </Columns>
               <SettingsContextMenu Enabled="true"></SettingsContextMenu>
            <ClientSideEvents EndCallback="grid_EndCallBack" RowClick="gridRowclick" />
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
            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsLoadingPanel Text="Please Wait..." />
        </dxe:ASPxGridView>
     <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                ContextTypeName="ERPDataClassesDataContext" TableName="V_SalesInvoiceOldUnitList" />
        <asp:HiddenField ID="hfIsFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfBranchID" runat="server" />
    </div>

                     </dxe:ContentControl>
                </ContentCollection>
            </dxe:TabPage>


            
            <dxe:TabPage Name="PendingDelivery" Text="Pending Delivery">
                <ContentCollection>
                    <dxe:ContentControl runat="server">

                                <div class="GridViewArea">
        <dxe:ASPxGridView ID="GridChallan" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
            Width="100%" ClientInstanceName="cGridChallan" OnCustomCallback="cGridChallan_CustomCallback" OnDataBinding="cGridChallan_DataBinding" 
            Settings-HorizontalScrollBarMode="Auto">
            <Columns>
                <dxe:GridViewDataTextColumn Caption="Document No." FieldName="InvoiceNo"
                    VisibleIndex="0" FixedStyle="Left" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="Invoice_Date"
                    VisibleIndex="1">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                
                  <dxe:GridViewDataTextColumn Caption="ChallanId" FieldName="ChallanId" Width="0"
                    VisibleIndex="4">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn> 

                <dxe:GridViewDataTextColumn Caption="Sales Challan No." FieldName="SalesChallan_Number" Width="150px"
                    VisibleIndex="4">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Challan Date" FieldName="SalesChallan_Date" Width="150px"
                    VisibleIndex="5">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Ageing Today" FieldName="AgeingDate"
                    VisibleIndex="6" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Customer" FieldName="CustomerName" Width="150px"
                    VisibleIndex="7">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Gross Amount" FieldName="GrossAmount"
                    VisibleIndex="8" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Tax & Charges" FieldName="ChargesAmount"
                    VisibleIndex="9" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Net Amount" FieldName="NetAmount"
                    VisibleIndex="10" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Amount Received" FieldName="AmountReceived"
                    VisibleIndex="11" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Balance Amount" FieldName="BalanceAmount"
                    VisibleIndex="12" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Assigned Unit" FieldName="assignedBranch"
                    VisibleIndex="12" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="13" Width="240px">
                    <DataItemTemplate>
                         

                        <a href="javascript:void(0);" onclick="ChallanClick('<%# Container.KeyValue %>',<%# Container.VisibleIndex %>)" class="pad" title="Create Delivery Challan">
                            
                            <i class="fa fa-truck out" ></i></a>


                     
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <HeaderTemplate><span>Actions</span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                </dxe:GridViewDataTextColumn>
            </Columns>
            <ClientSideEvents   />
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
            <SettingsSearchPanel Visible="True" />
            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsLoadingPanel Text="Please Wait..." />
        </dxe:ASPxGridView>
    </div>

                       </dxe:ContentControl>
                </ContentCollection>
            </dxe:TabPage>



        </TabPages>
    </dxe:ASPxPageControl>


        <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
    </div>
    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" GridViewID="GrdQuotation" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
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



     <dxe:ASPxPopupControl ID="AssignmentPopUp" runat="server" Width="600"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cAssignmentPopUp" Height="150"
        HeaderText="Branch Assignment" AllowResize="false" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                <div id="BranchAssignmentHeader">
                    <dxe:ASPxCallbackPanel runat="server" ID="BranchRequUpdatePanel" ClientInstanceName="cBranchRequUpdatePanel" OnCallback="BranchRequUpdatePanel_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">

                                <table class="smllpad">
                                    <tr>
                                        <td style="width: 110px"></td>
                                        <td>
                                            <label>Unit</label></td>
                                        
                                        <td></td>

                                    </tr>
                                    <tr>
                                        <td style="padding-right: 10px">
                                            <label>Assigned To</label></td>
                                        <td class="relative">
                                            <dxe:ASPxComboBox ID="AssignedBranch" runat="server" ClientInstanceName="cAssignedBranch" Width="100%">
                                               
                                            </dxe:ASPxComboBox>
                                            <span id="MandatoryBranchAssign" style="display: none" class="errorField">
                                                <img id="MandatoryBranchAssignid" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                            </span>
                                        </td>
                                        
                                        <td>
                                            <input type="button" value="Assign" class="btn btn-primary" onclick="AssignBranchToThisInvoice()" />
                                            <%--<input type="button" value="Cancel" class="btn btn-danger" onclick="CancelBranchToThisInvoice()" />--%>
                                        </td>

                                    </tr>
                                </table>
                            </dxe:PanelContent>
                        </PanelCollection>
                        <ClientSideEvents EndCallback="BranchRequUpdatePanelEndCallBack" />
                    </dxe:ASPxCallbackPanel>
                </div>
                


                




            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

</asp:Content>
