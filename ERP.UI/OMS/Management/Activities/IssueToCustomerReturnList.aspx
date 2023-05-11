<%--=======================================================Revision History=====================================================    
    1.0   Pallab    V2.0.38   11-05-2023      26096: Issue To Customer module design modification & check in small device
=========================================================End Revision History===================================================--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IssueToCustomerReturnList.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master"  Inherits="ERP.OMS.Management.Activities.IssueToCustomerReturnList" %>

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
    <script>
        function onPrintJv(id) {
            window.location.href = "../../reports/XtraReports/Viewer/SalesCustomerReturnReportViewer.aspx?id=" + id
            ////window.location.href = "../../reports/XtraReports/Viewer/TaxInvoiceReportViewer.aspx?id=" + id

        }

        function OnclickViewAttachment(obj) {
            //var URL = '/OMS/Management/Activities/SalesReturn_Document.aspx?idbldng=' + obj + '&type=SalesReturn';
            var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=IssueToReturn';
            window.location.href = URL;
        }
        document.onkeydown = function (e) {
            if (event.keyCode == 18) isCtrl = true;


            if (event.keyCode == 78 && isCtrl == true) { //run code for alt+a -- ie, Add

                StopDefaultAction(e);
                OnAddButtonClick();
            }
            else if (event.keyCode == 77 && isCtrl == true) {
                StopDefaultAction(e);
                OnAddManualButtonClick();
            }

        }

        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }
        function OnAddButtonClick() {
            var url = 'IssuetoCustomerReturn.aspx?key=' + 'ADD';
            window.location.href = url;
        }

        function OnAddManualButtonClick() {
            var url = 'IssuetoCustomerReturnManual.aspx?key=' + 'ADD';
            window.location.href = url;
        }
        function OnMoreInfoClick(keyValue) {
            var ActiveUser = '<%=Session["userid"]%>'
            if (ActiveUser != null) {
                $.ajax({
                    type: "POST",
                    url: "CustomerReturnList.aspx/GetEditablePermission",
                    data: "{'ActiveUser':'" + ActiveUser + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var status = msg.d;
                        var url = 'IssuetoCustomerReturn.aspx?key=' + keyValue + '&Permission=' + status + '&type=CR';
                        window.location.href = url;
                    }
                });
            }
        }
        function OnclickIssueToCustomerReturn(keyValue, IsManualFlag) {
            var ActiveUser = '<%=Session["userid"]%>'
            if (ActiveUser != null) {
                $.ajax({
                    type: "POST",
                    url: "IssueToCustomerReturnList.aspx/GetEditablePermission",
                    data: "{'ActiveUser':'" + ActiveUser + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async:false,
                    success: function (msg) {
                        var status = msg.d;
                        debugger;
                        if (IsManualFlag == "False")
                        {
                            var url = 'IssueToCustomerReturn.aspx?key=' + keyValue + '&Permission=' + status + '&type=CR';
                            window.location.href = url;
                        }
                        else if (IsManualFlag == "True") {
                            var url = 'IssueToCustomerReturnManual.aspx?key=' + keyValue + '&Permission=' + status + '&type=CR';
                            window.location.href = url;
                        }
                        
                       
                    }
                });
            }
        }
        ////##### coded by Samrat Roy - 17/04/2017 - ref IssueLog(P Order - 70) 
        ////Add an another param to define request type 
        function OnViewClick(keyValue) {
            var url = 'IssuetoCustomerReturn.aspx?key=' + keyValue + '&req=V' + '&type=CR';
            window.location.href = url;
        }

        function OnClickDelete(keyValue) {
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    cGrdCustomerReturn.PerformCallback('Delete~' + keyValue);
                }
            });
        }
        function OnEndCallback(s, e) {

            if (cGrdCustomerReturn.cpDelete != null) {
                jAlert(cGrdCustomerReturn.cpDelete);

                cGrdCustomerReturn.cpDelete = null;
                cGrdCustomerReturn.Refresh();
                // window.location.href = "CustomerReturnList.aspx";
            }
        }
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
                cGrdCustomerReturn.Refresh();
                //  cGrdCustomerReturn.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
            }
        }
        function gridRowclick(s, e) {
            $('#GrdCustomerReturn').find('tr').removeClass('rowActive');
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
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cGrdCustomerReturn.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cGrdCustomerReturn.SetWidth(cntWidth);
            }
            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cGrdCustomerReturn.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cGrdCustomerReturn.SetWidth(cntWidth);
                }

            });
        });
    </script>

    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 0;
        }

        #GrdCustomerReturn {
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
            <h3>Issue To Customer </h3>
        </div>
    </div>
        <div class="form_main">
        <div class="clearfix">
             <% if (rights.CanAdd)
                                   { %>
            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success "><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Add <u>N</u>ormal</span> </a>
            <a href="javascript:void(0);" onclick="OnAddManualButtonClick()" class="btn btn-success "><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Add <u>M</u>anual</span> </a>
            <%} %>

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
        <div class="GridViewArea relative">
        <dxe:ASPxGridView ID="GrdCustomerReturn" runat="server" KeyFieldName="SrlNo" AutoGenerateColumns="False"  
            Width="100%" ClientInstanceName="cGrdCustomerReturn" OnCustomCallback="GrdCustomerReturn_CustomCallback" SettingsBehavior-AllowFocusedRow="true" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto"
           DataSourceID="EntityServerModeDataSource"  SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" 
            >
            <SettingsSearchPanel Visible="True" Delay="5000" />
            <Columns>
                <dxe:GridViewDataTextColumn Caption="Document Number" FieldName="SalesReturnNo"
                    VisibleIndex="0" FixedStyle="Left"  Width="140px" >
                    <CellStyle CssClass="gridcellleft" Wrap="true" >
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                  <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="Return_Date" Caption="Posting Date" SortOrder="Descending">
                                               <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                      <Settings AllowAutoFilterTextInputTimer="False" />
                                               <EditFormSettings Visible="True"></EditFormSettings>
                                           </dxe:GridViewDataTextColumn>
               <%--  <dxe:GridViewDataTextColumn Caption="Date" FieldName="Return_Date"
                    VisibleIndex="1" FixedStyle="Left" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>--%>

                 <dxe:GridViewDataTextColumn Caption="Customer Return Number(s)" FieldName="CustomerReturn"
                    VisibleIndex="2" FixedStyle="Left" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Customer" FieldName="CustomerName"
                    VisibleIndex="3" FixedStyle="Left" Width="210px" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Amount" FieldName="Amount"
                    VisibleIndex="4" FixedStyle="Left" Width="110px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>               
               
                
                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="0">
                    <DataItemTemplate>
                        <div class='floatedBtnArea'>
                        <% if (rights.CanView)
                            { %>
                        <a href="javascript:void(0);" onclick="OnViewClick('<%#Eval("Return_Id")%>')" class="" title="">
                            <span class='ico ColorFive'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                           <% } %>
                         <% if (rights.CanEdit)
                                       { %>
                        <a href="javascript:void(0);" onclick="OnclickIssueToCustomerReturn('<%#Eval("Return_Id")%>','<%#Eval("IsManual")%>')" class="" title="">
                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a><%} %>
                        <% if (rights.CanDelete)
                                       { %>
                        <a href="javascript:void(0);" onclick="OnClickDelete('<%#Eval("Return_Id")%>')" class="" title="">
                           <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a><%} %>
                        <%-- <a href="javascript:void(0);" onclick="OnClickCopy('<%# Container.KeyValue %>')" class="pad" title="Copy ">
                            <i class="fa fa-copy"></i></a>--%>
                      <%--   <% if (rights.CanEdit)
                                       { %>
                        <a href="javascript:void(0);" onclick="OnClickStatus('<%# Container.KeyValue %>')" class="pad" title="Status">
                            <img src="../../../assests/images/verified.png" /></a><%} %>--%>
                         <% if (rights.CanView)
                                       { %>
                        <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%#Eval("Return_Id")%>')" class="" title="">
                            <span class='ico ColorSix'><i class='fa fa-paperclip'></i></span><span class='hidden-xs'>Add/View Attachment</span>
                        </a><%} %>

                          <% if (rights.CanPrint)
                           { %>
                        <a href="javascript:void(0);" onclick="onPrintJv('<%#Eval("Return_Id")%>')" class="" title="">
                            <span class='ico editColor'><i class='fa fa-print' aria-hidden='true'></i></span><span class='hidden-xs'>Print</span>
                        </a><%} %>
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
            <ClientSideEvents EndCallback="OnEndCallback" RowClick="gridRowclick" />
           <%-- <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </SettingsPager>--%>
            <settingspager pagesize="10">
                 <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200"/>
            </settingspager>
            <SettingsSearchPanel Visible="True" />
            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsLoadingPanel Text="Please Wait..." />
        </dxe:ASPxGridView>
        <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="v_IssueToCustomerReturnList" />
        <asp:HiddenField ID="hiddenedit" runat="server" />
    </div>
    </div>
    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" GridViewID="GrdCustomerReturn" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
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
</asp:Content>


