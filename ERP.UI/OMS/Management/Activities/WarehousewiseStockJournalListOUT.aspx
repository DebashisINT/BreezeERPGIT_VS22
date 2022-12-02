<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="WarehousewiseStockJournalListOUT.aspx.cs" Inherits="ERP.OMS.Management.Activities.WarehousewiseStockJournalListOUT" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        var isFirstTime = true;
        function OnAddClick() {
            window.location.href = 'WarehousewiseStockJournalAddOUT.aspx?Key=Add';
        }
        document.onkeydown = function (e) {
            if (event.keyCode == 65 && event.altKey == true) {

                if (document.getElementById('AddId'))
                    OnAddClick();
            }
        }
        function AllControlInitilize() {
            if (isFirstTime) {

                if (localStorage.getItem('WHSJFromDate')) {
                    var fromdatearray = localStorage.getItem('WHSJFromDate').split('-');
                    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                    cFormDate.SetDate(fromdate);
                }

                if (localStorage.getItem('WHSJToDate')) {
                    var todatearray = localStorage.getItem('WHSJToDate').split('-');
                    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                    ctoDate.SetDate(todate);
                }
                if (localStorage.getItem('WHSJListBranch')) {
                    if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('WHSJListBranch'))) {
                        ccmbBranchfilter.SetValue(localStorage.getItem('WHSJListBranch'));
                    }

                }
                if ($("#LoadGridData").val() == "ok")
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

                localStorage.setItem("WHSJFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("WHSJToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("WHSJListBranch", ccmbBranchfilter.GetValue());

                $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                $("#hfBranchID").val(ccmbBranchfilter.GetValue());
                $("#hfIsFilter").val("Y");
                cgridAdvanceAdj.Refresh();
            }


        }
        function GridEndCallBack() {
            if (cgridAdvanceAdj.cpReturnMesg) {

                if (cgridAdvanceAdj.cpReturnMesg == -2) {
                    jConfirm("Product is going negative do you want to delete?", "Alert", function (ret) {
                        if (ret) {
                            $("#hdnWDelete").val("Y");
                            cgridAdvanceAdj.PerformCallback("Del~" + $("#hdnDelete").val());
                        }
                    });
                }
                else {
                    jAlert(cgridAdvanceAdj.cpReturnMesg, "Alert", function () { cgridAdvanceAdj.Refresh(); });
                    cgridAdvanceAdj.cpReturnMesg = null;
                }

            }
        }
        function OnViewClick(id) {
            var url = 'WarehousewiseStockJournalAddOUT.aspx?key=' + id + '&req=V';
            window.location.href = url;
        }
        function onEditClick(id) {
            window.location.href = 'WarehousewiseStockJournalAddOUT.aspx?Key=' + id;
        }

        function OnClickDelete(id) {
            $("#hdnDelete").val(id);
            jConfirm("Confirm Delete?", "Alert", function (ret) {
                if (ret)
                { cgridAdvanceAdj.PerformCallback("Del~" + id); }
            });

        }
        var StockJournalId = 0;
        function onPrintJv(id) {
           
            StockJournalId = id;
            cSelectPanel.cpSuccess = "";
            cDocumentsPopup.Show();           
            cCmbDesignName.SetSelectedIndex(0);
            cSelectPanel.PerformCallback('Bindalldesignes');
            $('#btnOK').focus();
        }
        function cSelectPanelEndCall(s, e) {
            if (cSelectPanel.cpSuccess != "") {
                var TotDocument = cSelectPanel.cpSuccess.split(',');
                var reportName = cCmbDesignName.GetValue();
                var module = 'WarehouseStockOUT';
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + StockJournalId + '&PrintOption=' + 1, '_blank')
            }          
            if (cSelectPanel.cpSuccess == "") {
                cCmbDesignName.SetSelectedIndex(0);
            }
        }
        function PerformCallToGridBind() {
            cSelectPanel.PerformCallback('Bindsingledesign');
            cDocumentsPopup.Hide();
            return false;
        }
        function gridRowclick(s, e) {
            $('#gridAdvanceAdj').find('tr').removeClass('rowActive');
            $('.floatedBtnArea').removeClass('insideGrid');           
            $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).addClass('rowActive');
            setTimeout(function () {               
                var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');               
                $.each(lists, function (index, value) {                 
                    setTimeout(function () {
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }
        //Mantis Issue 25010
        function OnclickViewAttachment(obj) {
            var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=WarehouseWiseStockOUT';
            window.location.href = URL;
        }
        //End of Mantis Issue 25010
        //Rev work start 12.07.2022 mantise no :0025011: Update E-way Bill
        function OnEWayBillClick(id, VisibleIndex, EWayBillNumber, EWayBillDate) {
            
            cgridAdvanceAdj.SetFocusedRowIndex(VisibleIndex);
            if (EWayBillNumber.trim() != "") {
                ctxtEWayBillNumber.SetText(EWayBillNumber);
            }
            else {
                ctxtEWayBillNumber.SetText("");
            }
            if (EWayBillDate.trim() != "" && EWayBillDate.trim() != "01-01-1970" && EWayBillDate.trim() != "01-01-1900" && EWayBillDate.trim() != "01-01-0100") {
                var d = new Date(EWayBillDate.split('-')[2].trim(), EWayBillDate.split('-')[1].trim() - 1, EWayBillDate.split('-')[0].trim(), 0, 0, 0, 0);
                cdt_EWayBill.SetDate(d);
            }
            else {
                cdt_EWayBill.SetText("");
            }
            $('#hddnInvoiceID').val(id);
            cPopup_EWayBill.Show();
            ctxtEWayBillNumber.Focus();
        }
        function CancelEWayBill_save() {
            cPopup_EWayBill.Hide();
        }
        function GetEWayBillDateFormat(today) {
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
                today = yyyy + '-' + mm + '-' + dd;
            }

            return today;
        }
        function CallEWayBill_save() {
            var Stk_Id = $("#<%=hddnInvoiceID.ClientID%>").val();
            var UpdateEWayBill = ctxtEWayBillNumber.GetValue();
            if (UpdateEWayBill == "0") {
                UpdateEWayBill = "";
            }
            if (cdt_EWayBill.GetValue() == "" && cdt_EWayBill.GetValue() == null) {
                var EWayBillDate = "1990-01-01";
            }
            else {
                //var EWayBillDate = GetEWayBillDateFormat(new Date(cdt_EWayBill.GetValue()));
                //Rev Subhra  0019106  11/12/2018
                if (cdt_EWayBill.GetValue() == null) {
                    var EWayBillDate = null;
                }
                else {
                    var EWayBillDate = GetEWayBillDateFormat(new Date(cdt_EWayBill.GetValue()));
                }
                //End of Rev 
            }
            $.ajax({
                type: "POST",
                url: "WarehousewiseStockJournalListOUT.aspx/UpdateEWayBill",
                data: JSON.stringify({
                    Stk_Id: Stk_Id, UpdateEWayBill: UpdateEWayBill, EWayBillDate: EWayBillDate
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var status = msg.d;
                    if (status == "1") {
                        jAlert("Saved successfully.");
                        //ctxtEWayBillNumber.SetText("");
                        cPopup_EWayBill.Hide();
                        //Rev work start 03.08.2022 mantise no :0025011: Update E-way Bill
                        //cGrdQuotation.Refresh();
                        cgridAdvanceAdj.Refresh();
                        //Rev work close 03.08.2022 mantise no :0025011: Update E-way Bill
                    }
                    else if (status == "-10") {
                        jAlert("Data not saved.");
                        cPopup_EWayBill.Hide();
                    }
                }
            });
            //}           
        }
        //Rev work close 12.07.2022 mantise no :0025011: Update E-way Bill
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
             /*Mantis Issue 24428*/
        .mlableWh>input +span {
            white-space: nowrap;
        }
         .eqTble > tbody>tr>td {
            padding:0 7px;
            vertical-align:top;
        }
        /*End of Mantis Issue 24428*/
    </style>
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
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>

    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">Warehouse Wise Stock - OUT </h3>
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
                <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                </dxe:ASPxComboBox>
            </td>
            <td>
                <input type="button" value="Show" class="btn btn-primary" onclick="updateGridByDate()" />
            </td>

        </tr>

    </table>
    <div class="form_main">
        <% if (rights.CanAdd)
           { %>
        <a href="javascript:void(0);" onclick="OnAddClick()" id="AddId" class="btn btn-success  btn-radius"><span class="btn-icon"><i class="fa fa-plus"></i></span><u>A</u>dd  </a>
        <%} %>
        <% if (rights.CanExport)
           { %>
        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
            <asp:ListItem Value="0">Export to</asp:ListItem>
            <asp:ListItem Value="1">PDF</asp:ListItem>
            <asp:ListItem Value="2">XLS</asp:ListItem>
            <asp:ListItem Value="3">RTF</asp:ListItem>
            <asp:ListItem Value="4">CSV</asp:ListItem>
        </asp:DropDownList>
        <% } %>

          <div id="spnEditLock" runat="server" style="display:none; color:red;text-align:center"></div>
     <div id="spnDeleteLock" runat="server" style="display:none; color:red;text-align:center"></div>

        <div class="GridViewArea relative">

            <dxe:ASPxGridViewExporter ID="exporter" GridViewID="gridAdvanceAdj" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
            </dxe:ASPxGridViewExporter>
            <dxe:ASPxGridView ID="gridAdvanceAdj" runat="server" KeyFieldName="StockJournal_ID" AutoGenerateColumns="False"
                Width="100%" ClientInstanceName="cgridAdvanceAdj" SettingsBehavior-AllowFocusedRow="true"
                SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
                SettingsBehavior-ColumnResizeMode="Control" DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false"
                SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" OnSummaryDisplayText="gridAdvanceAdj_SummaryDisplayText"
                OnCustomCallback="gridAdvanceAdj_CustomCallback" Settings-VerticalScrollBarMode="Auto">

                <SettingsSearchPanel Visible="True" Delay="5000" />
                <Columns>
                    <dxe:GridViewDataTextColumn FieldName="StockJournal_ID" Visible="false" SortOrder="Descending">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Document No." FieldName="StockTransfer_No" Width="200"
                        VisibleIndex="0" FixedStyle="Left">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>


                    <dxe:GridViewDataDateColumn Caption="Date" FieldName="Stock_Date" Width="200"
                        VisibleIndex="0" PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataDateColumn>

                    <dxe:GridViewDataTextColumn Caption="Unit" FieldName="branch_description" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Project Name" FieldName="Proj_Name" Width="160"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Technician" FieldName="Technician_Name" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                     <dxe:GridViewDataTextColumn Caption="Entity" FieldName="EntityName" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                       <dxe:GridViewDataTextColumn Caption="Transportation Mode" FieldName="TransportationMode" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                       <dxe:GridViewDataTextColumn Caption="Vehicle No." FieldName="VehicleNo" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                       <dxe:GridViewDataTextColumn Caption="Remarks" FieldName="Remarks" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                     <dxe:GridViewDataTextColumn Caption="Type" FieldName="ReplaceableType" Width="100"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                     <dxe:GridViewDataTextColumn Caption="Employee Name" FieldName="Employee_Name" Width="250"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="CreatedBy" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="EnteredOn" Settings-AllowAutoFilter="False"
                        Caption="Entered On" Width="200">
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Updated By" FieldName="LastUpdatedBy" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                     <dxe:GridViewDataTextColumn Caption="Updated On" FieldName="LastUpdateOn" Width="200"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <%--Rev work start 12.07.2022 mantise no :0025011: Update E-way Bill--%>
                    <dxe:GridViewDataTextColumn Caption="E-Way Bill No" FieldName="EWayBillNumber" Width="100"
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <%--Rev work close 12.07.2022 mantise no :0025011: Update E-way Bill--%>

                    <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="0">
                        <DataItemTemplate>
                            <div class='floatedBtnArea'>

                            <% if (rights.CanView)
                               { %>
                            <a href="javascript:void(0);" onclick="OnViewClick('<%# Container.KeyValue %>')" class="" title="">
                                <span class='ico ColorFive'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                            <% } %>
                            <% if (rights.CanEdit)
                               { %>
                            <a href="javascript:void(0);" class="" title="" onclick="onEditClick('<%# Container.KeyValue %>')" style='<%#Eval("EditStockJournalOUTlock")%>'>
                                <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                            </a>
                                <%} %>

                            <% if (rights.CanDelete)
                               { %>
                            <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="" title="" id="a_delete" style='<%#Eval("DeleteStockJournalOUTlock")%>'>
                                <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                            <%} %>
                           <% if (rights.CanPrint)
                               { %>
                            <a href="javascript:void(0);" onclick="onPrintJv('<%# Container.KeyValue %>')" class="" title="">
                                <span class='ico ColorSeven'><i class='fa fa-print'></i></span><span class='hidden-xs'>Print</span>
                            </a>
                          <%} %>
                                <%--Mantis Issue 25010--%>
                                <%--Mantis Issue 25127--%>
                            <% if (rights.CanAddUpdateDocuments)
                               { %>
                                <%--End of Mantis Issue 25127--%>
                                <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%# Container.KeyValue %>')" class="" title="">
                                    <span class='ico ColorSix'><i class='fa fa-paperclip'></i></span><span class='hidden-xs'>Add/View Attachment</span>
                                </a>
                                <%} %>
                                <%--End of Mantis Issue 25010--%>
                                 <%--Rev work start 12.07.2022 mantise no :0025011: Update E-way Bill--%>
                                <% if (rights.CanEdit)
                                   { %>
                            <a href="javascript:void(0);" onclick="OnEWayBillClick('<%# Container.KeyValue %>','<%# Container.VisibleIndex %>','<%#Eval("EWayBillNumber") %>','<%#Eval("EWayBillDate") %>')" class="" title="">
                                <span class='ico ColorFour'><i class='fa fa-file-text-o'></i></span><span class='hidden-xs'>Update E-Way Bill</span></a>
                                <%} %>
                            <%--Rev work close 12.07.2022 mantise no :0025011: Update E-way Bill--%>
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
                <Styles>
                    <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
                    <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>
                    <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                    <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
                    <Footer CssClass="gridfooter"></Footer>
                </Styles>
                <SettingsPager PageSize="10">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                </SettingsPager>

                <Settings ShowGroupPanel="True" ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" ShowStatusBar="Hidden" ShowHorizontalScrollBar="true" ShowFilterRow="true" ShowFilterRowMenu="true" />
                <TotalSummary>
                    <dxe:ASPxSummaryItem FieldName="TotalStockInHand" SummaryType="Sum" />
                </TotalSummary>
                <SettingsLoadingPanel Text="Please Wait..." />
                <ClientSideEvents EndCallback="GridEndCallBack" RowClick="gridRowclick" />

            </dxe:ASPxGridView>
            <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                ContextTypeName="ERPDataClassesDataContext" TableName="v_WarehouseWiseStockJournalList" />
            <asp:HiddenField ID="hfIsFilter" runat="server" />
            <asp:HiddenField ID="hfFromDate" runat="server" />
            <asp:HiddenField ID="hfToDate" runat="server" />
            <asp:HiddenField ID="hfBranchID" runat="server" />
            <asp:HiddenField ID="hiddenedit" runat="server" />
            <asp:HiddenField ID="hdnWDelete" runat="server" Value="N" />
            <asp:HiddenField ID="hdnDelete" runat="server" />
        </div>


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
                               <%-- <dxe:ASPxCheckBox ID="selectNone" Text="None" runat="server" ToolTip="Select None" ClientSideEvents-CheckedChanged="NoneCheckChange"
                                    ClientInstanceName="CselectNone">
                                </dxe:ASPxCheckBox>--%>

                               <%-- <dxe:ASPxCheckBox ID="selectOriginal" Text="Original For Recipient" runat="server" ToolTip="Select Original" ClientSideEvents-CheckedChanged="OrginalCheckChange"
                                    ClientInstanceName="CselectOriginal">
                                </dxe:ASPxCheckBox>--%>

                               <%-- <dxe:ASPxCheckBox ID="selectDuplicate" Text="Duplicate For Transporter" runat="server" ToolTip="Select Duplicate" ClientSideEvents-CheckedChanged="DuplicateCheckChange"
                                    ClientInstanceName="CselectDuplicate">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectTriplicate" Text="Triplicate For Supplier" runat="server" ToolTip="Select Triplicate"
                                    ClientInstanceName="CselectTriplicate">
                                </dxe:ASPxCheckBox>--%>

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

        <%--Rev work start 12.07.2022 mantise no :0025011: Update E-way Bill--%>
    <dxe:ASPxPopupControl ID="Popup_EWayBill" runat="server" ClientInstanceName="cPopup_EWayBill"
        Width="400px" HeaderText="Update E-Way Bill" PopupHorizontalAlign="WindowCenter"
        BackColor="white" Height="150px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="Top clearfix">
                    <table style="width: 100%; margin: 0 auto; margin-top: 5px;">
                        <tr>
                            <label>
                                <dxe:ASPxLabel ID="ASPxLabel12" runat="server" Text="E-Way Bill Number">
                                </dxe:ASPxLabel>
                            </label>
                            <dxe:ASPxTextBox ID="txtEWayBillNumber" MaxLength="12" ClientInstanceName="ctxtEWayBillNumber"
                                runat="server" Width="100%">
                                <%-- <MaskSettings Mask="<0..999999999999>" AllowMouseWheel="false" />--%>
                                <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                            </dxe:ASPxTextBox>
                        </tr>
                        <tr>
                            <td>
                                <label style="margin-top: 6px">
                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Exp. Date">
                                    </dxe:ASPxLabel>
                                </label>
                                <dxe:ASPxDateEdit ID="dt_EWayBill" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdt_EWayBill" Width="100%" UseMaskBehavior="True">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>                       
                    </table>
                    <div style="margin-top: 10px;">
                        <input id="btnEWayBillSave" class="btn btn-primary" onclick="CallEWayBill_save()" type="button" value="Save" />
                        <input id="btnEWayBillCancel" class="btn btn-danger" onclick="CancelEWayBill_save()" type="button" value="Cancel" />
                        <dxe:ASPxLabel ID="lblEwayBillStatus" runat="server" Text="" Style="color: red; font-size: large"></dxe:ASPxLabel>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>
    <asp:HiddenField ID="hddnInvoiceID" runat="server" />
    <%--Rev work close 12.07.2022 mantise no :0025011: Update E-way Bill--%>
    </div>

    <asp:HiddenField ID="hdnLockFromDateedit" runat="server" />
<asp:HiddenField ID="hdnLockToDateedit" runat="server" />
 
 <asp:HiddenField ID="hdnLockFromDatedelete" runat="server" />
    <asp:HiddenField ID="hdnLockToDatedelete" runat="server" />

</asp:Content>
