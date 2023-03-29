<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                29-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="Future Sales" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Activities.management_Activities_futuresale" CodeBehind="futuresale.aspx.cs" %>





<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <script type="text/javascript">

        function ZoomProduct(keyValue) {

            var url = '/OMS/management/master/View/ViewProduct.html?id=' + keyValue;

            CAspxDirectProductViewPopup.SetWidth(window.screen.width - 50);
            CAspxDirectProductViewPopup.SetHeight(window.innerHeight - 70);
            CAspxDirectProductViewPopup.SetContentUrl(url);
            CAspxDirectProductViewPopup.RefreshContentUrl();
            CAspxDirectProductViewPopup.Show();
        }

        function View_Customer(keyValue) {

            CAspxDirectCustomerViewPopup.SetWidth(window.screen.width - 50);
            CAspxDirectCustomerViewPopup.SetHeight(window.innerHeight - 70);
            var url = '/OMS/management/master/View/ViewCustomer.html?id=' + keyValue;
            CAspxDirectCustomerViewPopup.SetContentUrl(url);
            //AspxDirectAddCustPopup.ClearVerticalAlignedElementsCache();

            CAspxDirectCustomerViewPopup.RefreshContentUrl();
            CAspxDirectCustomerViewPopup.Show();
        }

        function SignOff() {
            window.parent.SignOff()
        }

        function ShowDetail(ProductType) {
            document.getElementById('GridDiv').style.display = 'none';
            document.getElementById('FrameDiv').style.display = '';
            document.getElementById("ASPxPageControl1_ShowDetails").src = ProductType;
        }

        function ShowHistory(LeadId) {
            var width = 800;
            var height = 300;
            var x = (screen.availHeight - height) / 2;
            var y = (screen.availWidth - width) / 2;
            window.open("ShowHistory_Phonecall.aspx?id1=" + LeadId, 'welcome', 'width=' + width + ',height=' + height + ',top=' + x + ',left=' + y + ',menubar=no,status=no,location=no,toolbar=no,scrollbars=yes');
        }


        function Showhistory(slsid) {

            var frmdate = cxdeFromDate.GetText();
            var todate = cxdeToDate.GetText();
            var stract = slsid + "&frmdate=" + frmdate + "&todate=" + todate;
            //  alert(stract)
            //   alert(stract)
            //    window.open("ShowHistory_Phonecall.aspx?id1=" + stract);
            document.location.href = "../Master/ShowHistory_Phonecall.aspx?id1=" + slsid + "&frmdate=" + frmdate + "&todate=" + todate;

        }

        function disp_prompt(name) {
            if (name == "tab0") {
                document.location.href = "crm_sales.aspx";
            }
            if (name == "tab1") {
                document.location.href = "frmDocument.aspx";
            }
            else if (name == "tab2") {
                //document.location.href="futuresale.aspx"; 
            }
            else if (name == "tab3") {
                document.location.href = "ClarificationSales.aspx";
            }
            else if (name == "tab4") {
                document.location.href = "ClosedSales.aspx";
            }
        }

        function ShowDetailProduct(actid) {

            cPopup_Product.Show();
            cAspxProductGrid.PerformCallback(actid);
            // cComponentPanel.PerformCallback(slsid);
        }
        function ShowDetailProductClass(actid) {

            cPopup_Product_Class.Show();
            cAspxProductClassGrid.PerformCallback(actid);
            // cComponentPanel.PerformCallback(slsid);
        }
        function Budget_open() {
            var SMid = '';
            var url = '/OMS/Management/Activities/SalesmanBudget.aspx?tid=1';
            popupbudget.SetContentUrl(url);
            popupbudget.Show();

            return false;
            //return true;
        }
        function BudgetAfterHide(s, e) {
            popupbudget.Hide();
        }
        function btn_ShowRecordsClick() {
            var startDate = new Date();
            var endDate = new Date();

            startDate = cxdeFromDate.GetDate();
            if (startDate != null) {
                endDate = cxdeToDate.GetDate();
                var startTime = startDate.getTime();
                var endTime = endDate.getTime();


                difference = (endTime - startTime) / (1000 * 60 * 60 * 24 * 30);

                if (difference > 12) {
                    jAlert('End date cannot  12 month later than Start date', 'Future Sales', function () {
                        return false;

                    });
                }
                else {
                    grid.PerformCallback('ShowGrid');
                }
            }
        }

        function ShowClosed(keyValue) {

            jConfirm('Confirm Closed Sales?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.PerformCallback('ClosedStatus~' + keyValue);
                }
            });
        }

        function LastActivityDetailsCall(obj) {


            if (grid.cpDelmsg != null) {
                if (grid.cpDelmsg.trim() != '') {
                    jAlert(grid.cpDelmsg);
                    grid.cpDelmsg = '';

                }


            }




            if (grid.cpSave == "1") {

                grid.cpSave = null;
                window.close();
                window.parent.popupCbudget.Hide();
                //grid.Refresh();
            }


        }

        function btMyPendingActivity() {
            // alert('66');
            var frmdate = cxdeFromDate.GetText();
            var todate = cxdeToDate.GetText();
            //document.location.href = "TodayTask.aspx?id=" + '3' + "&frmdate=" + frmdate + "&todate=" + todate;


            document.location.href = "TodayTask.aspx";
        }
        function OnBudgetCopen(Cusid, productclassid, slsid) {

            cacpCrossBtn.PerformCallback('BudgetClass~' + Cusid + '~' + productclassid + '~' + slsid);
            popupCbudget.Show();

            return true;
        }

        function BudgetCAfterHide(s, e) {
            popupCbudget.Hide();
        }


        function Save_ButtonClick() {

            grid.PerformCallback('InsertBudgetClass');
        }


        function acpCrossBtnEndCall() {
            // debugger;
            var custid = '';
            var productclassid = '';
            var slsid = '';

            if (cacpCrossBtn.cpcustid != null)
            { custid = cacpCrossBtn.cpcustid; }

            if (cacpCrossBtn.cpproductclassid != null) {
                productclassid = cacpCrossBtn.cpproductclassid;
            }

            if (cacpCrossBtn.cpslsid != null) {
                slsid = cacpCrossBtn.cpslsid;
            }
            $('#<%=hdncustid.ClientID %>').val(custid);
            $('#<%=hdnproductclassid.ClientID %>').val(productclassid);

            $('#<%=hdnslsid.ClientID %>').val(slsid);



            cacpCrossBtn.cpcustid = null;
            cacpCrossBtn.cpproductclassid = null;
            cacpCrossBtn.cpslsid = null;





        }
    </script>
    <style>
        .pull-right {
            float: right !important;
        }

        .mtop3 {
            margin-top: 3px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                grid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                grid.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    grid.SetWidth(cntWidth);
                }

            });
        });
    </script>

    <style>
        /*Rev 1.0*/

        /*select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }*/

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , .dxeTextBox_PlasticBlue
        {
            height: 30px;
            border-radius: 4px;
        }

        .dxeButtonEditButton_PlasticBlue
        {
            background: #094e8c !important;
            border-radius: 4px !important;
            padding: 0 4px !important;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        .simple-select::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 26px;
            right: 13px;
            font-size: 16px;
            transform: rotate(269deg);
            font-weight: 500;
            background: #094e8c;
            color: #fff;
            height: 18px;
            display: block;
            width: 26px;
            /* padding: 10px 0; */
            border-radius: 4px;
            text-align: center;
            line-height: 19px;
            z-index: 0;
        }
        .simple-select {
            position: relative;
        }
        .simple-select:disabled::after
        {
            background: #1111113b;
        }
        select.btn
        {
            padding-right: 10px !important;
        }

        /*select.btn
        {
            padding-right: 10px !important;
        }*/

        /*.panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }*/

        .dxpLite_PlasticBlue .dxp-current
        {
            background-color: #1b5ea4;
            padding: 3px 5px;
            border-radius: 2px;
        }

        #accordion {
            margin-bottom: 20px;
            margin-top: 10px;
        }

        .dxgvHeader_PlasticBlue {
    background: #1b5ea4 !important;
    color: #fff !important;
}
        #ShowGrid
        {
            margin-top: 10px;
        }

        .pt-25{
                padding-top: 25px !important;
        }

        .dxgvEditFormDisplayRow_PlasticBlue td.dxgv, .dxgvDataRow_PlasticBlue td.dxgv, .dxgvDataRowAlt_PlasticBlue td.dxgv, .dxgvSelectedRow_PlasticBlue td.dxgv, .dxgvFocusedRow_PlasticBlue td.dxgv
        {
            padding: 6px 6px 6px !important;
        }

        #lookupCardBank_DDD_PW-1
        {
                left: -182px !important;
        }
        .plhead a>i
        {
                top: 9px;
        }

        .clsTo
        {
            display: flex;
    align-items: flex-start;
        }

        input[type="radio"], input[type="checkbox"]
        {
            margin-right: 5px;
        }
        .dxeCalendarDay_PlasticBlue
        {
                padding: 6px 6px;
        }

        .modal-dialog
        {
            width: 50%;
        }

        .modal-header
        {
            padding: 8px 4px 8px 10px;
            background: #094e8c !important;
        }

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #EmployeeGrid , .TableMain100 #GrdEmployee,
        .TableMain100 #SalesDetailsGrid
        {
            max-width: 96% !important;
        }

        /*div.dxtcSys > .dxtc-content > div, div.dxtcSys > .dxtc-content > div > div
        {
            width: 95% !important;
        }*/

        .btn-info
        {
                background-color: #1da8d1 !important;
                background-image: none;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .dxeButtonDisabled_PlasticBlue
        {
            background: #b5b5b5 !important;
            border-color: #b5b5b5 !important;
        }

        #ddlValTech
        {
            width: 100% !important;
            margin-bottom: 0 !important;
        }

        .dis-flex
        {
            display: flex;
            align-items: baseline;
        }

        input + label
        {
            line-height: 1;
                margin-top: 7px;
        }

        .dxtlHeader_PlasticBlue
        {
            background: #094e8c !important;
        }

        .dxeBase_PlasticBlue .dxichCellSys
        {
            padding-top: 2px !important;
        }

        .pBackDiv
        {
            border-radius: 10px;
            box-shadow: 1px 1px 10px #1111112e;
        }
        .HeaderStyle th
        {
            padding: 5px;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer
        {
            padding-top: 15px;
        }

        .pt-2
        {
            padding-top: 5px;
        }
        .pt-10
        {
            padding-top: 10px;
        }

        .pt-15
        {
            padding-top: 15px;
        }

        .pb-10
        {
            padding-bottom: 10px;
        }

        .pTop10 {
    padding-top: 20px;
}
        .custom-padd
        {
            padding-top: 4px;
    padding-bottom: 10px;
        }

        input + label
        {
                margin-right: 10px;
        }

        .btn
        {
            margin-bottom: 0;
        }

        .pl-10
        {
            padding-left: 10px;
        }

        .col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }

        .devCheck
        {
            margin-top: 5px;
        }

        .mtc-5
        {
            margin-top: 5px;
        }

        .mtc-10
        {
            margin-top: 10px;
        }

        /*select.btn
        {
           position: relative;
           z-index: 0;
        }*/

        select
        {
            margin-bottom: 0;
        }

        .form-control
        {
            background-color: transparent;
        }

        select.btn-radius {
    padding: 4px 8px 6px 11px !important;
}
        .mt-30{
            margin-top: 30px;
        }

        .panel-title h3
        {
            padding-top: 0;
            padding-bottom: 0;
        }

        .btn-radius
        {
            padding: 4px 11px !important;
            border-radius: 4px !important;
        }

        .crossBtn
        {
             right: 30px;
             top: 25px;
        }

        .mb-10
        {
            margin-bottom: 10px;
        }

        .btn-cust
        {
            background-color: #108b47 !important;
            color: #fff;
        }

        .btn-cust:hover
        {
            background-color: #097439 !important;
            color: #fff;
        }

        .gHesder
        {
            background: #1b5ca0 !important;
            color: #ffffff !important;
            padding: 6px 0 6px !important;
        }

        .close
        {
             color: #fff;
             opacity: .5;
             font-weight: 400;
        }

        .mt-27
        {
            margin-top: 27px !important;
        }

        .col-md-3 , .col-md-2
        {
            margin-top: 8px;
        }

        #upldBigLogo , #upldSmallLogo
        {
            width: 100%;
        }

        #DivSetAsDefault
        {
            margin-top: 25px;
        }

        .btn.btn-xs
        {
                font-size: 14px !important;
        }

        .dxpc-content table
        {
             width: 100%;
        }

        input[type="text"], input[type="password"], textarea
        {
            margin-bottom: 0 !important;
        }
        #FromDate , #ToDate , #ASPxFromDate , #ASPxToDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #FromDate_B-1 , #ToDate_B-1 , #ASPxFromDate_B-1 , #ASPxToDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FromDate_B-1 #FromDate_B-1Img , #ToDate_B-1 #ToDate_B-1Img , #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img
        {
            display: none;
        }

        #lblToDate
        {
            padding-left: 10px;
        }

        .dxtc-activeTab:after {
            content: '';
            width: 0;
            height: 0;
            border-left: 8px solid transparent;
            border-right: 8px solid transparent;
            border-top: 9px solid #3e5395;
            position: absolute;
            /* left: 50%; */
            z-index: 3;
            /* bottom: -15px; */
            margin-left: -9px;
        }
        /*Rev end 1.0*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main"> 
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Sales Activity - Future Sales
            </h3>

            <div id="btncross" class="crossBtn" style="margin-left: 50px;"><a href="crm_sales.aspx"><i class="fa fa-times"></i></a></div>

        </div>
    </div>
        <div class="form_main">
        <div class="" id="divdetails">
            <div class="clearfix">
                <div style="padding-right: 5px;">


                    <table class="pull-left">
                        <tr>
                            <td>
                                <span id="spanBudget">
                                    <a href="javascript:void(0);" onclick="Budget_open()" title="Budget" class="btn btn-primary">Target Sale Of Product</a>
                                </span>
                                <span id="spanMyactivities" visible="false">
                                    <asp:Button ID="btn_myactivity"  Text="My Activities" runat="server" CssClass="btn btn-primary" OnClick="btMyactivities_Click" />
                                </span>

                                <asp:Button ID="btn_PendingTask"  Text="Pending Task" runat="server" CssClass="btn btn-primary" OnClick="btMyPendingTask_Click" />
                                <asp:Button ID="btn_PendingActivity"  Text="My Today's Task" runat="server" CssClass="btn btn-primary" OnClick="btMyPendingActivity_Click" />


                            </td>
                            <td>
                                <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                                    <asp:Label ID="lblFromDate" runat="Server" Text="Next Activity From : " CssClass="mylabel1"
                                        Width="135px"></asp:Label>
                                </div>
                            </td>
                            <%--Rev 1.0 : "for-cust-icon" class add--%>
                            <td class="for-cust-icon">
                                <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                                <%--Rev 1.0--%>
                                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                <%--Rev end 1.0--%>
                            </td>
                            <td style="padding-left: 1px">
                                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                                    <asp:Label ID="lblToDate" runat="Server" Text="To : " CssClass="mylabel1"
                                        Width="45px"></asp:Label>
                                </div>
                            </td>
                            <%--Rev 1.0 : "for-cust-icon" class add--%>
                            <td class="for-cust-icon">
                                <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                    <%-- <ClientSideEvents DateChanged="cxdeToDate_OnChaged"></ClientSideEvents>--%>
                                </dxe:ASPxDateEdit>
                                <%--Rev 1.0--%>
                                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                <%--Rev end 1.0--%>
                            </td>
                            <td style="padding-left: 5px; padding-top: 0px">
                                <button class="btn btn-success" onclick="btn_ShowRecordsClick()" type="button">Show</button>

                            </td>
                        </tr>

                    </table>

                    <% if (rights.CanExport)
                       { %>

                    <asp:DropDownList ID="drdSalesActivityDetails" runat="server" Height="34px" CssClass="btn btn-sm btn-primary  expad " OnSelectedIndexChanged="drdSalesActivityDetails_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>

                    <% } %>
                </div>
            </div>
        </div>
        <table class="TableMain100">
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="2" ClientInstanceName="page"
                        Width="100%">
                        <TabPages>
                            <dxe:TabPage Text="Open Activities" Name="Assigned Sales Activity" TabStyle-CssClass="tabOrg">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="Document Collection" Name="Document Collection" TabStyle-CssClass="tabSk">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="Future Sales" Name="Future Sales">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <div id="GridDiv">
                                            <table width="100%">

                                                <tr>
                                                    <td>
                                                        <%--Mantis Issue 24913  [ SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true" SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true"  removed from grid ]  --%>
                                                        <dxe:ASPxGridView ID="SalesDetailsGrid" KeyFieldName="sls_id" SettingsBehavior-AllowFocusedRow="true" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid" SettingsCookies-Enabled="true" SettingsCookies-StoreFiltering="true"
                                                            Width="100%" OnDataBinding="SalesDetailsGrid_DataBinding" OnCustomCallback="SalesDetailsGrid_CustomCallback"
                                                            OnHtmlRowCreated="SalesDetailsGrid_HtmlRowCreated" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" Settings-HorizontalScrollBarMode="Auto">
                                                            <Columns>
                                                                <%--  <dxe:GridViewCommandColumn VisibleIndex="0">cont
                                                                </dxe:GridViewCommandColumn>--%>
                                                                <dxe:GridViewDataTextColumn Caption="Activity" VisibleIndex="0" FieldName="LeadId" Visible="false">
                                                                    <DataItemTemplate>
                                                                        <dxe:ASPxCheckBox ID="chkDetail" runat="server">
                                                                        </dxe:ASPxCheckBox>

                                                                        <dxe:ASPxTextBox ID="lblActNo" runat="server" Width="100%" Visible="false"
                                                                            NullText="0" Value='<%# Eval("LeadId") %>'>
                                                                        </dxe:ASPxTextBox>


                                                                        <dxe:ASPxTextBox ID="lblSalesId" runat="server" Width="100%" Visible="false"
                                                                            NullText="0" Value='<%# Eval("sls_id") %>'>
                                                                        </dxe:ASPxTextBox>

                                                                        <dxe:ASPxTextBox ID="lblact_id" runat="server" Width="100%" Visible="false"
                                                                            NullText="0" Value='<%# Eval("sls_activity_id") %>'>
                                                                        </dxe:ASPxTextBox>
                                                                        <dxe:ASPxTextBox ID="lblAssignedTaskId" runat="server" Width="100%" Visible="false"
                                                                            NullText="0" Value='<%# Eval("act_assignedTo") %>'>
                                                                        </dxe:ASPxTextBox>

                                                                    </DataItemTemplate>
                                                                </dxe:GridViewDataTextColumn>
                                                                <%--      <dxe:GridViewDataTextColumn FieldName="Address" Caption="Address" VisibleIndex="1" Width="18%">
                                                                </dxe:GridViewDataTextColumn>--%>

                                                                <dxe:GridViewDataTextColumn FieldName="ContactNumber" Caption="Contact Details" VisibleIndex="2" Width="18%">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="sls_assignedBy" Caption="Assigned ID" VisibleIndex="0" Visible="false" Width="18%">
                                                                </dxe:GridViewDataTextColumn>

                                                                <dxe:GridViewDataTextColumn FieldName="Assigned To" VisibleIndex="3" Caption="Salesman" Settings-AllowAutoFilterTextInputTimer="False">
                                                                </dxe:GridViewDataTextColumn>

                                                                <dxe:GridViewDataTextColumn FieldName="Assigned By" VisibleIndex="4" Caption="Assigned By" Visible="false">
                                                                </dxe:GridViewDataTextColumn>

                                                                <dxe:GridViewDataTextColumn FieldName="Industry" VisibleIndex="5" Caption="Industry" Settings-AllowAutoFilterTextInputTimer="False">
                                                                </dxe:GridViewDataTextColumn>

                                                                <dxe:GridViewDataTextColumn FieldName="Name"   VisibleIndex="6" Caption="Customer/Lead Name" Settings-AllowAutoFilterTextInputTimer="False">

                                                                    <DataItemTemplate>
                                                                        <a href="javascript:void(0);" title="Click here to view Customer." onclick="View_Customer('<%# Eval("LeadId") %>');"><%#Eval("Name")%>
                                                                            <%--<img src="../../../assests/images/Delete.png" />--%>
                                                                        </a>
                                                                    </DataItemTemplate>

                                                                </dxe:GridViewDataTextColumn>

                                                                <%--   <dxe:GridViewDataTextColumn FieldName="ProductType" ReadOnly="True" Visible="False"
                                                                    VisibleIndex="4"  Width="18%">
                                                                </dxe:GridViewDataTextColumn>--%>
                                                                <dxe:GridViewDataTextColumn FieldName="Id" ReadOnly="True" Visible="False" VisibleIndex="7">
                                                                    <EditFormSettings Visible="False" />
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Amount" Visible="False" VisibleIndex="8">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Product" ReadOnly="True" Visible="False"
                                                                    VisibleIndex="9">
                                                                </dxe:GridViewDataTextColumn>

                                                                <dxe:GridViewDataTextColumn FieldName="ProductType" VisibleIndex="10" Caption="Product Type" Visible="false">
                                                                </dxe:GridViewDataTextColumn>

                                                                <dxe:GridViewDataTextColumn FieldName="ProductName" VisibleIndex="12" Caption="Product(s)" Settings-AllowAutoFilterTextInputTimer="False">
                                                                    <DataItemTemplate>
                                                                        <%--<asp:Label ID="lblProduct" runat="server" Text=""></asp:Label>--%>
                                                                        <asp:LinkButton runat="server" ID="lblProduct" OnClientClick='<%# string.Format("ZoomProduct(\"{0}\"); return false;", Eval("product_id").ToString().Trim()) %>'></asp:LinkButton>

                                                                        <asp:LinkButton runat="server" ID="lnkProduct" OnClientClick='<%# string.Format("ShowDetailProduct(\"{0}\"); return false", Eval("act_id")) %>'><img  src="/assests/images/Info.png"/  title="Details"></asp:LinkButton>
                                                                    </DataItemTemplate>
                                                                    <CellStyle HorizontalAlign="Center">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>


                                                                <dxe:GridViewDataTextColumn FieldName="ProductClasName" VisibleIndex="11" Caption="Product Class" Settings-AllowAutoFilterTextInputTimer="False">
                                                                    <DataItemTemplate>
                                                                        <asp:Label ID="lblProductClass" runat="server" Text=""></asp:Label>
                                                                        <asp:LinkButton runat="server" ID="lnkProductClass" OnClientClick='<%# string.Format("ShowDetailProductClass(\"{0}\"); return false", Eval("sls_id")) %>'><img  src="/assests/images/Viewt1.png"/  title="Details"></asp:LinkButton>
                                                                    </DataItemTemplate>
                                                                    <CellStyle HorizontalAlign="Center">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>




                                                                <dxe:GridViewDataTextColumn FieldName="ExpectedTime" VisibleIndex="13" Caption="Date Of Completion" Settings-AllowAutoFilterTextInputTimer="False">
                                                                    <CellStyle HorizontalAlign="Center">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="PriorityName" VisibleIndex="14" Caption="Priority" Settings-AllowAutoFilterTextInputTimer="False">
                                                                    <CellStyle HorizontalAlign="Center">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="NextVisit" VisibleIndex="15" Caption="Next Activity Date" Settings-AllowAutoFilterTextInputTimer="False">
                                                                    <CellStyle HorizontalAlign="Center">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>

                                                                <dxe:GridViewDataTextColumn FieldName="budget" VisibleIndex="16" Caption="Product:Budget " Settings-AllowAutoFilterTextInputTimer="False">
                                                                    <CellStyle HorizontalAlign="Center">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Remarks" VisibleIndex="17" Caption="Budget Remarks" Settings-AllowAutoFilterTextInputTimer="False">
                                                                    <CellStyle HorizontalAlign="Center">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn Visible="false" VisibleIndex="18" Caption="Reassign">
                                                                    <DataItemTemplate>
                                                                        <a href="javascript:void(0)" onclick="ShowDetailReassign('<%#Eval("sls_id") %>')">Reassign</a>
                                                                    </DataItemTemplate>
                                                                    <EditFormSettings Visible="False" />
                                                                </dxe:GridViewDataTextColumn>


                                                                <dxe:GridViewDataTextColumn Visible="false" VisibleIndex="19" Caption="Create Activity">
                                                                    <DataItemTemplate>
                                                                        <a href="javascript:void(0)" onclick="ShowCreateActivity('<%#Eval("LeadId") %>','<%#Eval("sls_id") %>','<%#Eval("sls_activity_id") %>','<%#Eval("act_assignedTo") %>','<%#Eval("act_activityNo") %>','<%#Eval("act_assign_task") %>')">Create Activity</a>
                                                                    </DataItemTemplate>
                                                                    <EditFormSettings Visible="False" />
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn VisibleIndex="20" Caption="History" Width="100px">
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
                                                                <dxe:GridViewDataTextColumn VisibleIndex="21" Caption="Actions" Width="160px">
                                                                    <CellStyle HorizontalAlign="Center">
                                                                    </CellStyle>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <DataItemTemplate>
                                                                        <a style="display: none;" href="javascript:void(0)" onclick="ShowClosed('<%#Eval("sls_id") %>')">
                                                                            <img src="/assests/images/CrIcon.png" width="24" height="24" title="Closed Sales"></a>

                                                                        <asp:HyperLink runat="server" ID="hpnPh" CssClass="pad"><img  src="/assests/images/phone_number.png"/ width="16" height="16" title="Phone call"></asp:HyperLink>
                                                                        <asp:HyperLink runat="server" ID="hpnSv" CssClass="pad"><img  src="/assests/images/sales.png" / width="16" height="16" title="Sales Visit"></asp:HyperLink>
                                                                        <asp:HyperLink runat="server" ID="hpnOtheractvSms" CssClass="pad"><img  src="/assests/images/sms.png" / width="16" height="16" title="Sms"></asp:HyperLink>
                                                                        <asp:HyperLink runat="server" ID="hpnOtheractvMeet" CssClass="pad"><img  src="/assests/images/meeting.png" / width="16" height="16" title="Meeting"></asp:HyperLink>
                                                                        <asp:HyperLink runat="server" ID="hpnOtheractvEmail" CssClass="pad"><img  src="/assests/images/email.png" / width="16" height="16" title="Email"></asp:HyperLink>
                                                                        <%  if (rights.CanBudget)
                                                                            { %>
                                                                        <a href="javascript:void(0);" onclick="OnBudgetCopen('<%# Eval("cnt_Id") %>','<%# Eval("ProductClass_ID") %>','<%#Eval("sls_id") %>')" title="Budget" class="pad">
                                                                            <img src="/assests/images/cashbudget.png" width="16" height="16" />
                                                                        </a>

                                                                        <%   }%>
                                                                        <asp:Label Text='<%#Eval("act_activityTypes") %>' ID="lblactivty" Visible="false" runat="server"></asp:Label>
                                                                        <asp:HiddenField ID="hdnActivity" Value='<%#Eval("act_activityTypes") %>' runat="server" />
                                                                    </DataItemTemplate>
                                                                    <EditFormSettings Visible="False" />
                                                                </dxe:GridViewDataTextColumn>
                                                            </Columns>
                                                            <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                                                            <SettingsPager PageSize="10">
                                                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                                            </SettingsPager>
                                                            <Styles>
                                                                <LoadingPanel ImageSpacing="10px">
                                                                </LoadingPanel>
                                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                </Header>
                                                                <Cell CssClass="gridcellleft">
                                                                </Cell>
                                                            </Styles>
                                                            <%--  <SettingsPager NumericButtonCount="20" ShowSeparators="True">
                                                                <FirstPageButton Visible="True">
                                                                </FirstPageButton>
                                                                <LastPageButton Visible="True">
                                                                </LastPageButton>
                                                            </SettingsPager>--%>
                                                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="True" />
                                                            <SettingsSearchPanel Visible="True" />
                                                            <ClientSideEvents EndCallback="function(s, e) {
	LastActivityDetailsCall(s.cpHeight);
}" />
                                                        </dxe:ASPxGridView>

                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="FrameDiv" style="display: none;">
                                            <iframe width="100%" id="ShowDetails" runat="server" scrolling="no"></iframe>
                                        </div>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="Clarification Required" Name="Clarification Required" TabStyle-CssClass="tabSg">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="Closed Sales" Name="Closed Sales" TabStyle-CssClass="tabG">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                        </TabPages>
                        <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
	                                            var Tab0 = page.GetTab(0);
	                                            var Tab1 = page.GetTab(1);
	                                            var Tab2 = page.GetTab(2);
	                                            var Tab3 = page.GetTab(3);
	                                              var Tab4 = page.GetTab(4);
	                                            if(activeTab == Tab0)
	                                            {
	                                                disp_prompt('tab0');
	                                            }
	                                            if(activeTab == Tab1)
	                                            {
	                                                disp_prompt('tab1');
	                                            }
	                                            else if(activeTab == Tab2)
	                                            {
	                                                disp_prompt('tab2');
	                                            }
	                                            else if(activeTab == Tab3)
	                                            {
	                                                disp_prompt('tab3');
	                                            }
                                                 else if(activeTab == Tab4)
	                                            {
	                                                disp_prompt('tab4');
	                                            }
	                                            }"></ClientSideEvents>
                        <ContentStyle>
                            <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                        </ContentStyle>
                        <LoadingPanelStyle ImageSpacing="6px">
                        </LoadingPanelStyle>
                        <TabStyle Font-Size="12px">
                        </TabStyle>
                    </dxe:ASPxPageControl>
                </td>
            </tr>
        </table>
        <%--  <asp:SqlDataSource ID="SalesDetailsGridDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:CRMConnectionString %>"></asp:SqlDataSource>--%>
        <dxe:ASPxPopupControl ID="Popup_Product" runat="server" ClientInstanceName="cPopup_Product"
            Width="400px" HeaderText="Product List" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">


                    <dxe:PanelContent runat="server">

                        <dxe:ASPxGridView ID="AspxProductGrid" ClientInstanceName="cAspxProductGrid" Width="100%" runat="server" OnCustomCallback="AspxProductGrid_CustomCallback"
                            AutoGenerateColumns="False">

                            <Columns>


                                <dxe:GridViewDataTextColumn Visible="True" FieldName="sProducts_Name" Caption="Product Name">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" title="Click here to view Product." onclick="ZoomProduct                                  ('<%# Eval("sProducts_Code") %>');"><%#Eval("sProducts_Name")%>
                                            <%--<img src="../../../assests/images/Delete.png" />--%>
                                        </a>
                                    </DataItemTemplate>

                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                </dxe:GridViewDataTextColumn>




                            </Columns>

                            <SettingsBehavior AllowSort="false" AllowGroup="false" />
                        </dxe:ASPxGridView>


                    </dxe:PanelContent>

                </dxe:PopupControlContentControl>

            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />


        </dxe:ASPxPopupControl>

        <dxe:ASPxPopupControl ID="Popup_ProductClass" runat="server" ClientInstanceName="cPopup_Product_Class"
            Width="400px" HeaderText="Product Class List" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">


                    <dxe:PanelContent runat="server">

                        <dxe:ASPxGridView ID="ASPxGridProductClass" ClientInstanceName="cAspxProductClassGrid" Width="100%" runat="server" OnCustomCallback="AspxProductclassGrid_CustomCallback"
                            AutoGenerateColumns="False">

                            <Columns>
                                <dxe:GridViewDataTextColumn Visible="True" FieldName="ProductClass_Code" Caption="Product Class">
                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                </dxe:GridViewDataTextColumn>



                            </Columns>

                            <SettingsBehavior AllowSort="false" AllowGroup="false" />
                        </dxe:ASPxGridView>


                    </dxe:PanelContent>

                </dxe:PopupControlContentControl>

            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />


        </dxe:ASPxPopupControl>

        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
        <%-- <asp:SqlDataSource ID="FutureSalesDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:CRMConnectionString %>">
            </asp:SqlDataSource>--%>

        <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupbudget" Height="500px"
            Width="1310px" HeaderText="Budget" Modal="true" AllowResize="true" ResizingMode="Postponed">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>

            <ClientSideEvents CloseUp="BudgetAfterHide" />
        </dxe:ASPxPopupControl>

        <dxe:ASPxPopupControl ID="ASPXPopupControl1" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupCbudget" Height="300px"
            Width="350px" HeaderText="Budget" Modal="true" AllowResize="true" ResizingMode="Postponed">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="acpCrossBtn" ClientInstanceName="cacpCrossBtn" OnCallback="acpCrossBtn_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">

                                <div id="divbudget" runat="server" class="Top clearfix">
                                    <div class="col-sm-12">
                                        <h5>Criteria : Customer's Industrywise, All Products </h5>
                                    </div>
                                    <div class="col-sm-12">
                                        <label>Product Class</label>
                                        <dxe:ASPxComboBox ClientInstanceName="gridcomboproductclass" runat="server" Enabled="false" ID="gridproductclass" Width="100%">
                                            <ClientSideEvents ValueChanged="ProductclassChanged" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <br />
                                    <div class="col-sm-12">
                                        <label>Input Quantity(Current Financial Year)</label>
                                        <dxe:ASPxTextBox ClientInstanceName="gridcomboproductclass" runat="server" ID="txt_qtyfinyr" Width="100%">
                                            <MaskSettings Mask="<0..999999999>.<0..99>" />
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <div class="col-sm-12">
                                        <label>Remarks</label>
                                        <dxe:ASPxMemo ID="txtRemarks" runat="server" Width="100%" Height="50px" ClientInstanceName="ctxtRemarks"></dxe:ASPxMemo>
                                    </div>
                                    <div style="padding-top: 8px" class="col-md-12">
                                        <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                            <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                                        </dxe:ASPxButton>

                                        <asp:HiddenField runat="server" ID="hdnchkgridbatch" />
                                    </div>
                                </div>

                                <div id="divmsg" runat="server" style="display: none">
                                    <div class="col-md-12">No class is mapped for the selected Customer. Cannot enter budget values.</div>
                                </div>

                            </dxe:PanelContent>
                        </PanelCollection>
                        <ClientSideEvents EndCallback="acpCrossBtnEndCall" />
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>



            </ContentCollection>

            <ClientSideEvents CloseUp="BudgetCAfterHide" />
        </dxe:ASPxPopupControl>




        <dxe:ASPxPopupControl ID="Popup_Budget" runat="server" ClientInstanceName="cPopup_Budget"
            Width="400px" HeaderText="Budget" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                    <%-- <div class="Top clearfix">--%>
                    <%--<table style="width: 94%">
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
                    </table>--%>




                    <%--  </div>--%>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />


        </dxe:ASPxPopupControl>


        <asp:HiddenField runat="server" ID="hdncustid" />
        <asp:HiddenField runat="server" ID="hdnproductclassid" />
        <asp:HiddenField runat="server" ID="hdnslsid" />

    </div>
    </div>

    <dxe:ASPxPopupControl ID="AspxDirectCustomerViewPopup" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="CAspxDirectCustomerViewPopup" Height="650px"
        Width="1020px" HeaderText="Customer View" Modal="true" AllowResize="false">

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>


    <dxe:ASPxPopupControl ID="AspxDirectProductViewPopup" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="CAspxDirectProductViewPopup" Height="650px"
        Width="1020px" HeaderText="View Product" Modal="true" AllowResize="false">

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

</asp:Content>
