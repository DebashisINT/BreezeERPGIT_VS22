<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DailySalesReport.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.DailySalesReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">



    <script type="text/javascript">
        //Done by:Subhabrata
        //$(document).ready(function () {
        //    debugger;
        //    var Fromdate = new Date(Date.now());
        //    var ToDate = new Date(Date.now());
        //    cxdeFromDate.SetDate(Fromdate);
        //    cxdeToDate.SetDate(ToDate);
        //    Grid.PerformCallback('');
        //});

        $(document).ready(function () {
            cNxtactivtyDate.SetEnabled(false);
        });

        var isFirstTime = true;

        function AllControlInitilize() {
            if (isFirstTime) {

                if (localStorage.getItem('cxdeFromDate')) {
                    var fromdatearray = localStorage.getItem('cxdeFromDate').split('-');
                    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                    cxdeFromDate.SetDate(fromdate);
                }

                if (localStorage.getItem('cxdeToDate')) {
                    var todatearray = localStorage.getItem('cxdeToDate').split('-');
                    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                    cxdeToDate.SetDate(todate);
                }


                isFirstTime = false;
            }
        }

        function cxdeToDate_OnChaged(s, e) {
            debugger;
            var data = "OnDateChanged";
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
            //CallServer(data, "");
            Grid.PerformCallback('');
        }

        function btn_ShowRecordsClick() {
            var data = "OnDateChanged";
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
            //CallServer(data, "");
            localStorage.setItem("cxdeFromDate", cxdeFromDate.GetDate().format('yyyy-MM-dd'));
            localStorage.setItem("cxdeToDate", cxdeToDate.GetDate().format('yyyy-MM-dd'));
            Grid.PerformCallback('');


        }


        function OnContextMenuItemClick(sender, args) {
            if (args.item.name == "ExportToPDF" || args.item.name == "ExportToXLS") {
                args.processOnServer = true;
                args.usePostBack = true;
            } else if (args.item.name == "SumSelected")
                args.processOnServer = true;
        }

        function abc() {
            // alert();
            $("#drdExport").val(0);

        }

        function chnagenextdateenableprop(param) {
            if (param.checked == true) {
                cNxtactivtyDate.SetEnabled(true);
            }
            else {
                cNxtactivtyDate.SetEnabled(false);
            }
        }
    </script>


    <script>
        function Exportfunctionality(s) {
            ///   alert(s.value);

            var exportvalue = s.value;
            // alert(exportvalue);
            if (exportvalue != 0) {

                $.ajax({
                    type: "POST",
                    url: "DailySalesReport.aspx/Export",
                    data: JSON.stringify({ Exportvalue: exportvalue }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {

                        if (msg.d == true) {

                            $("#drdExport").val(0);
                        }
                        else {

                        }
                    }
                });
            }
        }
        var vartype;
        var nxtdate;
        function InsertFeedback(sid, dtid, tid, nxtcall, type, CustomerLead, ProductClassName, Industry, CallDate, NextCall, Outcome, detailsid, Tid, slno) {
            var datearr = [];
            var datearr1 = [];
            //  alert(slsid+' '+tid)
            $('#MandatoryRemarksFeedback').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
            txtFeedback.SetValue();
            $('#chkmailfeedback').prop('checked', false);
            stid = sid;
            refeedbackvar = dtid;
            $("#hdtid").val(tid);
            cPopup_Feedback.Show();


            CustomerLeadvar = CustomerLead;
            ProductClassNamevar = ProductClassName;
            Industryvar = Industry;
            CallDatevar = CallDate;
            NextCallvar = NextCall;
            Outcomevar = Outcome;
            Notevar = '';
            detailsidvar = detailsid;
            Tidvar = Tid;
            slnovar = slno;
            $("#hdndaily").val(sid);
            datearr = nxtcall.split('-')
            datearr1 = datearr[2].split(' ');
            nxtdate = new Date(datearr1[0], parseFloat(datearr[1]) - 1, datearr[0], 0, 0, 0, 0);
            cNxtactivtyDate.SetDate(nxtdate);
            vartype = type;
        }


        function CallFeedback_save() {
            cPopup_Feedback.Hide();

            var flag = true;
            var Remarks = txtFeedback.GetValue();
            if (Remarks == "" || Remarks == null) {
                $('#MandatoryRemarksFeedback').attr('style', 'display:block;position: absolute; right: -20px; top: 8px;');
                flag = false;
            }
            else {
                $('#MandatoryRemarksFeedback').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');

                Grid.PerformCallback('Feedback~' + refeedbackvar + '~' + Remarks + '~' + $("#hdtid").val() + '~' + stid + '~' + vartype + '~' + nxtdate + '~' + CustomerLeadvar + '~' + ProductClassNamevar + '~' + Industryvar + '~' + CallDatevar + '~' + NextCallvar + '~' + Outcomevar + '~' + Notevar + '~' + detailsidvar + '~' + Tidvar + '~' + slnovar);
                //  Grid.Refresh();
            }
            cNxtactivtyDate.SetEnabled(false);
            $("#chkchnageDate").attr("checked", false);
            $("#chkchnageDate").prop("checked", false);
            return flag;


        }




        function CancelFeedback_save() {


            txtFeedback.SetValue();
            cPopup_Feedback.Hide();
            $('#chkmailfeedback').prop('checked', false);
        }


        function ShowDetailFeedBack(stid, actid, Typeid) {


            cAspxAspxFeedGrid.PerformCallback('Feedbackdetails~' + actid + '~' + Typeid);
            cPopup_Feddbackdetails.Show();
            // cComponentPanel.PerformCallback(slsid);

        }


        function LastCall(obj) {


            if (Grid.cpMainSendmsg != null) {
                if (Grid.cpMainSendmsg.trim() != '') {

                    jAlert(Grid.cpMainSendmsg);
                    Grid.cpMainSendmsg = '';

                }


            }
        }
        function Showhistory(slsid) {

            var frmdate = cxdeFromDate.GetText();
            var todate = cxdeToDate.GetText();
            var stract = slsid + "&frmdate=" + frmdate + "&todate=" + todate;
            //document.location.href = "../Master/ShowHistory_Phonecall.aspx?id1=" + slsid + "&frmdate=" + frmdate + "&todate=" + todate;

            var url = "../Master/ShowHistory_Phonecall.aspx?id1=" + slsid + "&frmdate=" + frmdate + "&todate=" + todate + "&status=1";
            chistoryPopup.SetContentUrl(url);
            chistoryPopup.Show();

            //}
            return true;

        }
    </script>
    <link href="../../../assests/css/RES.css" rel="stylesheet" />

    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                Grid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                Grid.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    Grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    Grid.SetWidth(cntWidth);
                }

            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="panel-heading">
        <div class="panel-title">
            <%-- <h3>Contact Bank List</h3>--%>
            <h3>Daily Salesmen Report </h3>
            <div id="dvnormal" runat="server" visible="false" class="crossBtn"><a href="/OMS/management/ProjectMainPage.aspx"><i class="fa fa-times"></i></a></div>
            <div id="dvnfrmsales" runat="server" visible="false" class="crossBtn"><a href="/OMS/management/Activities/crm_sales.aspx"><i class="fa fa-times"></i></a></div>
            <div id="dvdocuments" runat="server" visible="false" class="crossBtn"><a href="/OMS/management/Activities/frmDocument.aspx"><i class="fa fa-times"></i></a></div>
            <div id="dvfutue" runat="server" visible="false" class="crossBtn"><a href="/OMS/management/Activities/futuresale.aspx"><i class="fa fa-times"></i></a></div>
            <div id="dvclarification" runat="server" visible="false" class="crossBtn"><a href="/OMS/management/Activities/ClarificationSales.aspx"><i class="fa fa-times"></i></a></div>
            <div id="dvclosed" runat="server" visible="false" class="crossBtn"><a href="/OMS/management/Activities/ClosedSales.aspx"><i class="fa fa-times"></i></a></div>

        </div>

    </div>
    <div class="form_main">
        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />
        <table class="pull-left responsive-table">
            <tr>
                <td>
                    <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                        <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td class="padLeftbig">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                        <%-- <ClientSideEvents DateChanged="cxdeToDate_OnChaged"></ClientSideEvents>--%>
                    </dxe:ASPxDateEdit>
                </td>
                <td class="padLeftbig" style="padding-top: 3px">
                    <button class="btn btn-primary" onclick="btn_ShowRecordsClick()" type="button">Show</button>
                    <% if (rights.CanExport)
                       { %>

                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLSX</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>

                    </asp:DropDownList>
                    <% } %>
                    <%--<dxe:ASPxButton ID="btn_ShowRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Show" CssClass="btn btn-primary" UseSubmitBehavior="False">
                        <ClientSideEvents Click="function(s, e) {btn_ShowRecordsClick();}" />
                    </dxe:ASPxButton>--%>
                </td>
            </tr>

        </table>
        <div class="pull-right">

            

            <%--     OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true"--%>
        </div>
        <table class="TableMain100">


            <tr>

                <td colspan="2">
                    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
                        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
                    </dxe:ASPxGlobalEvents>


                    <dxe:ASPxGridView ID="GridSalesReport"  ClientInstanceName="Grid" runat="server" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
            Width="100%"  OnCustomCallback="Grid_CustomCallback"  OnDataBinding="GridSalesReport_DataBinding" KeyFieldName="slno"
            Settings-HorizontalScrollBarMode="Auto" >
             
                      <%--  <dxe:ASPxGridView runat="server" ID="GridSalesReport" ClientInstanceName="Grid" Width="100%" AutoGenerateColumns="False"
                        OnCustomCallback="Grid_CustomCallback" OnDataBinding="GridSalesReport_DataBinding" KeyFieldName="slno"
                        OnSummaryDisplayText="ShowGrid_SummaryDisplayText" SettingsBehavior-AllowSelectByRowClick="true"
                        SettingsBehavior-ColumnResizeMode="Control" Settings-HorizontalScrollBarMode="Auto"  Settings-VerticalScrollableHeight="350" Settings-VerticalScrollBarMode="Auto">--%>
                        <Columns>
                            
                                <%-- <PropertiesComboBox
                                    ValueField="AssignTo" TextField="AssignTo"  />--%>

                                <%--<PropertiesComboBox EnableSynchronization="False" EnableIncrementalFiltering="True"
                                    ValueType="System.String" DataSourceID="SMDataSource" TextField="AssignTo" ValueField="AssignTo">
                                </PropertiesComboBox>
                                <%-- <PropertiesComboBox EnableSynchronization="False" EnableIncrementalFiltering="True"
                                            ValueType="System.String"  TextField="AssignTo" ValueField="AssignTo">
                                         </PropertiesComboBox>
                            </dxe:GridViewDataComboBoxColumn>--%>

                            <%--<dxe:GridViewDataTextColumn FieldName="SalesMan" Caption="SalesManType" Visible="false" />--%>

                            <%--  <dxe:GridViewDataTextColumn FieldName="EntityName" Caption="Entity" />--%>



                            <dxe:GridViewDataTextColumn FieldName="Customer_Lead" Caption="Customer/Lead" Width="200" VisibleIndex="0"  />
                            <dxe:GridViewDataTextColumn FieldName="ProductClass_Name" Caption="Product Class" Width="120" VisibleIndex="1" />
                            <dxe:GridViewDataTextColumn FieldName="Note" Caption="Activity Note" VisibleIndex="2" />
                            <dxe:GridViewDataTextColumn FieldName="Status" Caption="Status" Width="150" VisibleIndex="3" />
                            <dxe:GridViewDataTextColumn FieldName="NextCall" Caption="Next Activity Date" Width="130" VisibleIndex="4" />
                            <dxe:GridViewDataTextColumn FieldName="LASTDT" Caption="Last Order Confirm Date" Width="300" VisibleIndex="5" />
                            <dxe:GridViewDataTextColumn FieldName="Invoice_Number" Caption="Last Sale Date/Qty" Width="200" VisibleIndex="6" />
                           <%-- Rev Sayantani--%>
                           <%-- <dxe:GridViewDataTextColumn FieldName="Product" Caption="Product(s)" Width="250" VisibleIndex="6" Visible="false" />--%>
                             <dxe:GridViewDataTextColumn FieldName="Product" Caption="Product(s)" Width="250" VisibleIndex="7" Visible="false" ShowInCustomizationForm="false" />
                           <%-- End of Rev Sayantani--%>
                            <dxe:GridViewDataTextColumn FieldName="Industry" Caption="Industry" Width="150" VisibleIndex="8" />
                            <dxe:GridViewDataTextColumn FieldName="activitystatus" Caption="Outcome" Width="250" VisibleIndex="9" />

                            <dxe:GridViewDataTextColumn FieldName="feed_remarks" Caption="Feedback" Width="250" VisibleIndex="10" />


                            <dxe:GridViewDataTextColumn FieldName="Date" Caption="Activity Done On" Width="130" VisibleIndex="11" />

                            <dxe:GridViewDataTextColumn FieldName="AssignTo" Caption="Salesmen" VisibleIndex="12" Width="150" ></dxe:GridViewDataTextColumn>




                            <dxe:GridViewDataTextColumn FieldName="budget" Caption="Product:Budget Rate/Month" Width="100" VisibleIndex="13" />
                            <dxe:GridViewDataTextColumn VisibleIndex="14" Caption="History" Width="50" >
                                <CellStyle HorizontalAlign="Center">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                                <DataItemTemplate>
                                    <% if (rights.CanHistory)
                                       { %>


                                    <%--                                                                            <asp:HyperLink runat="server" ID="hpnhis" CssClass="pad"><img  src="/assests/images/history.png" width="16" height="16" title="History"></asp:HyperLink>
                                    --%>

                                    <a href="javascript:void(0)" onclick="Showhistory('<%#Eval("sls_id") %>')">

                                        <img src="../../../assests/images/history.png" />


                                    </a>

                                    <% } %>
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="15" Caption="Actions" Width="50" >
                                <CellStyle HorizontalAlign="Center">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                                <DataItemTemplate>
                                    <a href="javascript:void(0)" onclick="InsertFeedback('<%#Eval("sls_id") %>','<%#Eval("detailsid") %>','<%#Eval("Tid") %>','<%#Eval("NextCall") %>','<%#Eval("Type") %>','<%#Eval("Customer_Lead") %>','<%#Eval("ProductClass_Name") %>','<%#Eval("Industry") %>','<%#Eval("CallDate") %>','<%#Eval("NextCall") %>','<%#Eval("activitystatus") %>','<%#Eval("detailsid") %>','<%#Eval("Tid") %>','<%#Eval("slno") %>')" class="pad">
                                        <img src="/assests/images/feedback.png" title="Feedback" width="20" height="20"></a>
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <%-- --Rev Sayantani--%>
                      <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
                      <SettingsCookies Enabled="true" StorePaging="true" StoreColumnsVisiblePosition="true" Version="2.0" />
                      <%-- -- End of Rev Sayantani --%>
                      <%--  Rev Sayantani--%>
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                       <%-- End of Rev Sayantani--%>
                     
                       <%-- <SettingsPager PageSize="50">
                                                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                                            </SettingsPager>--%>
                       <%-- <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
                        <SettingsPager Mode="ShowPager">
                                    </SettingsPager>
                                    <SettingsPager PageSize="20">
                                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                    </SettingsPager>
                        <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                        <SettingsEditing Mode="EditForm" />
                        <SettingsContextMenu Enabled="true" />
                        <SettingsBehavior AutoExpandAllGroups="true" />
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                        <SettingsSearchPanel Visible="True" />
                        <ClientSideEvents EndCallback="function(s, e) {	LastCall(s.cpHeight);}" />--%>

                         <ClientSideEvents EndCallback="function(s, e) {	LastCall(s.cpHeight);}" />
            <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </SettingsPager>
            
            <SettingsSearchPanel Visible="false" />
            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
           



                       <%-- <TotalSummary>
                            <dxe:ASPxSummaryItem FieldName="Date" SummaryType="Count" />
                        </TotalSummary>--%>


                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
    </div>

    <%-- <asp:SqlDataSource ID="SalesDataSource" runat="server" 
        SelectCommand="sp_DailySales_Report" SelectCommandType="StoredProcedure"></asp:SqlDataSource>--%>
  <%--  <asp:SqlDataSource ID="SMDataSource" runat="server"
        SelectCommand="select *  FROM(select distinct isnull(MCSls.cnt_firstName,'')+' ' +isnull(MCSls.cnt_middleName,'')+' ' +isnull(MCSls.cnt_lastName,'') AssignTo from tbl_trans_sales trs   left join tbl_master_contact  MCSls on trs.sls_assignedTo=MCSls.cnt_id  )t WHERE   t.AssignTo is not null AND t.AssignTo<>''"></asp:SqlDataSource>--%>

    <%--<asp:SqlDataSource ID="EntityDataSource" runat="server"
        SelectCommand="sp_DailySales_Report" SelectCommandType="StoredProcedure"></asp:SqlDataSource>--%>
    <%-- <asp:SqlDataSource ID="EntityDataSource" runat="server" 
        SelectCommand="sp_DailySales_Report" SelectCommandType="StoredProcedure"></asp:SqlDataSource>--%>
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>


    <dxe:ASPxPopupControl ID="Popup_Feedback" runat="server" ClientInstanceName="cPopup_Feedback"
        Width="400px" HeaderText="Feedback" PopupHorizontalAlign="WindowCenter"
        BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                <div class="Top clearfix">
                    <table style="width: 94%">
                        <tr>
                            <td>Feedback<span style="color: red">*</span></td>
                            <td class="relative" style="padding-bottom: 8px;">
                                <dxe:ASPxMemo ID="txtInstFeedback" runat="server" Width="100%" Height="50px" ClientInstanceName="txtFeedback"></dxe:ASPxMemo>
                                <span id="MandatoryRemarksFeedback" style="display: none">
                                    <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor1234_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkchnageDate" ClientIDMode="Static" runat="server" onclick="chnagenextdateenableprop(this);" />
                                Change Next Activity Date
                            </td>
                            <td>
                                <dxe:ASPxDateEdit ID="ASPxNxtactivtyDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cNxtactivtyDate">
                                    <ButtonStyle Width="13px"></ButtonStyle>
                                    <%-- <ClientSideEvents DateChanged="cxdeToDate_OnChaged"></ClientSideEvents>--%>
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkmailfeedback" runat="server" ClientIDMode="Static" />
                                Send Mail
                            </td>
                            <td colspan="2" style="padding-left: 121px;"></td>
                        </tr>
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
    <dxe:ASPxPopupControl ID="historyPopup" runat="server" Width="900"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="chistoryPopup" Height="650"
        HeaderText="History" AllowResize="false" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

</asp:Content>
