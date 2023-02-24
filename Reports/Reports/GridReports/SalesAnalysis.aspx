<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                24-02-2023        2.0.36           Pallab              25575 : Report pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="SalesAnalysis.aspx.cs" Inherits="Reports.Reports.GridReports.SalesAnalysis" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function CloseLookup() {
            clookupClass.ConfirmCurrentSelection();
            clookupClass.HideDropDown();
            clookupClass.Focus();
        }

        function _CloseLookup() {
            clookupBrand.ConfirmCurrentSelection();
            clookupBrand.HideDropDown();
            clookupBrand.Focus();
        }

        function btn_ShowRecordsClick(e) {
            $("#drdExport").val(0);
            $("#hfIsSalesAnaSumm").val("Y");
            $("#ddldetails").val(0);
            cCallbackPanelSumm.PerformCallback();
            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "For the period: " + FromDate + " To " + ToDate;
        }

        function GetDateFormat(today) {
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
                today = dd + '-' + mm + '-' + yyyy;
            }

            return today;
        }

        function selectAll() {
            clookupClass.gridView.SelectRows();
            document.getElementById("hflookupClassAllFlag").value = "ALL";
        }

        function unselectAll() {
            clookupClass.gridView.UnselectRows();
            document.getElementById("hflookupClassAllFlag").value = "";
        }

        function _selectAll() {
            clookupBrand.gridView.SelectRows();
            document.getElementById("hflookupBrandAllFlag").value = "ALL";

        }

        function _unselectAll() {
            clookupBrand.gridView.UnselectRows();
            document.getElementById("hflookupClassAllFlag").value = "";
        }

        function OpenAnalysisDetails(brandID, classID, productID, Rate) {
            $("#hfIsSalesAnaDet").val("Y");
            $("#drdExport").val(0);
            $("#ddldetails").val(0);
            $('#<%=hdnCategoryID.ClientID %>').val(brandID);
            $('#<%=hdnClassID.ClientID %>').val(classID);
            $('#<%=hdnProductID.ClientID %>').val(productID);
            $('#<%=hdnRate.ClientID %>').val(Rate);

            cCallbackPanel.PerformCallback(productID);
            //cShowGridDetails.PerformCallback();
            cCallbackPanelDet.PerformCallback();
            cpopup.Show();
        }

        function popupHide(s, e) {
            cpopup.Hide();
        }

        function CallbackPanelEndCall(s, e) {
            if (cCallbackPanel.cpProductValue != null) {
                var ProductValue = cCallbackPanel.cpProductValue;
                cCallbackPanel.cpProductValue = null;

                var productCode = ProductValue.split('||@||')[0].replace("squot", "'").replace("squot", "'").replace("coma", ",").replace("slash", "/");
                var productName = ProductValue.split('||@||')[1].replace("squot", "'").replace("squot", "'").replace("coma", ",").replace("slash", "/");

                ctxtProductCode.SetValue(productCode);
                ctxtProductName.SetValue(productName);
            }
        }

        function closePopup(s, e) {
            e.cancel = false;
            $("#drdExport").val(0);
            $("#ddldetails").val(0);
            ctxtProductCode.SetValue("");
            ctxtProductName.SetValue("");
            $('#<%=hdnCategoryID.ClientID %>').val("");
            $('#<%=hdnClassID.ClientID %>').val("");
            $('#<%=hdnProductID.ClientID %>').val("");
            $('#<%=hdnRate.ClientID %>').val("");

            //cShowGridDetails.PerformCallback();
        }

        $(document).keydown(function (e) {
            if (e.keyCode == 27) {
                cpopup.Hide();
                cShowGrid.Focus();
            }
        });

        function OnWaitingGridKeyPress(e) {

            //if (e.code == "Enter" || e.code == "NumpadEnter") {
            if (e.key == "Enter" || e.key == "NumpadEnter") {
                cShowGrid.GetRowValues(cShowGrid.GetFocusedRowIndex(), 'CategoryID;ClassID;sProducts_ID;Pu_Rate', OnGetRowValues);
            }

        }
        function OnGetRowValues(values) {
            OpenAnalysisDetails(values[0], values[1], values[2], values[3])
        }

        function Endgridanalysis() {

            cShowGrid.Focus();
        }
        function CallbackPanelSummEndCall(s, e) {
            cShowGrid.Refresh();
        }
        function CallbackPanelDetEndCall(s, e) {
            cShowGridDetails.Refresh();
        }
    </script>

    <style>
        .pdbot > tbody > tr > td {
            padding-bottom: 10px;
        }
    </style>

    <style>
        .plhead a {
            font-size:16px;
            padding-left:10px;
            position:relative;
            width:100%;
            display:block;
            padding:9px 10px 5px 10px;
        }
        .plhead a>i {
            position:absolute;
            top:11px;
            right:15px;
        }
        #accordion {
            margin-bottom:10px;
        }
        .companyName {
            font-size:16px;
            font-weight:bold;
            margin-bottom:15px;
        }
        
        .plhead a.collapsed .fa-minus-circle{
            display:none;
        }
        /*rev Pallab*/
        .branch-selection-box .dxlpLoadingPanel_PlasticBlue tbody, .branch-selection-box .dxlpLoadingPanelWithContent_PlasticBlue tbody, #divProj .dxlpLoadingPanel_PlasticBlue tbody, #divProj .dxlpLoadingPanelWithContent_PlasticBlue tbody
        {
            bottom: 0 !important;
        }
        .dxlpLoadingPanel_PlasticBlue tbody, .dxlpLoadingPanelWithContent_PlasticBlue tbody
        {
            position: absolute;
            bottom: 80px;
            background: #fff;
            box-shadow: 1px 1px 10px #0000001c;
            right: 50%;
        }
        /*rev end Pallab*/

        /*Rev 1.0*/
        .outer-div-main {
            background: #ffffff;
            padding: 10px;
            border-radius: 10px;
            box-shadow: 1px 1px 10px #11111154;
        }

        /*.form_main {
            overflow: hidden;
        }*/

        label , .mylabel1, .clsTo, .dxeBase_PlasticBlue
        {
            color: #141414 !important;
            font-size: 14px !important;
                font-weight: 500 !important;
                margin-bottom: 0 !important;
                    line-height: 20px;
        }

        #GrpSelLbl .dxeBase_PlasticBlue
        {
                line-height: 20px !important;
        }

        select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue
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

        .calendar-icon {
            position: absolute;
            bottom: 6px;
            right: 20px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img
        {
            display: none;
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

        .panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }

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

        .styled-checkbox {
        position: absolute;
        opacity: 0;
        z-index: 1;
    }

        .styled-checkbox + label {
            position: relative;
            /*cursor: pointer;*/
            padding: 0;
            margin-bottom: 0 !important;
        }

            .styled-checkbox + label:before {
                content: "";
                margin-right: 6px;
                display: inline-block;
                vertical-align: text-top;
                width: 16px;
                height: 16px;
                /*background: #d7d7d7;*/
                margin-top: 2px;
                border-radius: 2px;
                border: 1px solid #c5c5c5;
            }

        .styled-checkbox:hover + label:before {
            background: #094e8c;
        }


        .styled-checkbox:checked + label:before {
            background: #094e8c;
        }

        .styled-checkbox:disabled + label {
            color: #b8b8b8;
            cursor: auto;
        }

            .styled-checkbox:disabled + label:before {
                box-shadow: none;
                background: #ddd;
            }

        .styled-checkbox:checked + label:after {
            content: "";
            position: absolute;
            left: 3px;
            top: 9px;
            background: white;
            width: 2px;
            height: 2px;
            box-shadow: 2px 0 0 white, 4px 0 0 white, 4px -2px 0 white, 4px -4px 0 white, 4px -6px 0 white, 4px -8px 0 white;
            transform: rotate(45deg);
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

        /*.TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet
        {
            max-width: 98% !important;
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
                margin-top: 3px;
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


        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cShowGrid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cShowGrid.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cShowGrid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cShowGrid.SetWidth(cntWidth);
                }

            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <%--<div class="panel-title">
            <h3>Sales Analysis</h3>
        </div>--%>
        <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
          <div class="panel panel-info">
            <div class="panel-heading" role="tab" id="headingOne">
              <h4 class="panel-title plhead">
                <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne" class="collapsed">
                  <asp:Label ID="RptHeading" runat="Server" Text="" Width="470px" style="font-weight:bold;"></asp:Label>
                    <i class="fa fa-plus-circle" ></i>
                    <i class="fa fa-minus-circle"></i>
                </a>
              </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
              <div class="panel-body">
                    <div class="companyName">
                        <asp:Label ID="CompName" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="CompAdd" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="CompOth" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="CompPh" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>       
                    <div>
                        <asp:Label ID="CompAccPrd" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="DateRange" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
              </div>
            </div>
          </div>
        </div>
    </div>
    <%--Rev 1.0: "outer-div-main" class add: --%>
    <div class="outer-div-main">
        <div class="form_main">
        <div class="row">
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label1" runat="Server" Text="Class : " CssClass="mylabel1"></asp:Label></label>
                <asp:HiddenField ID="hflookupClassAllFlag" runat="server" Value="" />
                <dxe:ASPxGridLookup ID="lookupClass" ClientInstanceName="clookupClass" SelectionMode="Multiple" runat="server" 
                    OnDataBinding="lookupClass_DataBinding" KeyFieldName="ProductClass_ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                    <Columns>
                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                        <dxe:GridViewDataColumn FieldName="ProductClass_Name" Visible="true" VisibleIndex="1" Caption="Class Name" Settings-AutoFilterCondition="Contains">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="ProductClass_ID" Visible="true" VisibleIndex="2" Caption="Class ID" Settings-AutoFilterCondition="Contains" Width="0">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                    </Columns>
                    <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                        <Templates>
                            <StatusBar>
                                <table class="OptionsTable" style="float: right">
                                    <tr>
                                        <td>
                                            <%--<div class="hide">--%>
                                                <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll" />
                                            <%--</div>--%>
                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll" />
                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseLookup" UseSubmitBehavior="False" />
                                        </td>
                                    </tr>
                                </table>
                            </StatusBar>
                        </Templates>
                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                        <SettingsPager Mode="ShowPager">
                        </SettingsPager>
                        <SettingsPager PageSize="20">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                        </SettingsPager>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                    </GridViewProperties>
                    
                </dxe:ASPxGridLookup>
                <span id="MandatorClass" style="display: none" class="validclass" />
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label3" runat="Server" Text="Brand : " CssClass="mylabel1"></asp:Label></label>
                <asp:HiddenField ID="hflookupBrandAllFlag" runat="server" Value="" />
                <dxe:ASPxGridLookup ID="lookupBrand" ClientInstanceName="clookupBrand" SelectionMode="Multiple" runat="server"
                    OnDataBinding="lookupBrand_DataBinding" KeyFieldName="Brand_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                    <Columns>
                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                        <dxe:GridViewDataColumn FieldName="Brand_Name" Visible="true" VisibleIndex="1" Caption="Brand Name" Settings-AutoFilterCondition="Contains">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="Brand_Id" Visible="true" VisibleIndex="1" Caption="Brand ID" Settings-AutoFilterCondition="Contains" Width="0">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                    </Columns>
                    <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                        <Templates>
                            <StatusBar>
                                <table class="OptionsTable" style="float: right">
                                    <tr>
                                        <td>
                                            <%--<div class="hide">--%>
                                                <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="_selectAll" />
                                            <%--</div>--%>
                                            <dxe:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="_unselectAll" />                                            
                                            <dxe:ASPxButton ID="ASPxButton5" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="_CloseLookup" UseSubmitBehavior="False" />
                                        </td>
                                    </tr>
                                </table>
                            </StatusBar>
                        </Templates>
                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                        <SettingsPager Mode="ShowPager">
                        </SettingsPager>
                        <SettingsPager PageSize="20">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                        </SettingsPager>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                    </GridViewProperties>
                </dxe:ASPxGridLookup>
                <span id="MandatoryActivityType" style="display: none" class="validclass" />
            </div>
            <%--Rev 1.0--%>
            <%--<div class="col-md-2">--%>
            <div class="col-md-2 simple-select">
                <%--Rev end 1.0--%>
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label2" runat="Server" Text="Rate Criteria : " CssClass="mylabel1"></asp:Label>
                </label>
                <asp:DropDownList ID="ddlRateCriteria" runat="server" Width="100%">
                    <asp:ListItem Text="Without GST" Value="False"></asp:ListItem>
                    <asp:ListItem Text="With GST" Value="True"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsFrom">
                    <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
                <%--Rev 1.0--%>
                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                <%--Rev end 1.0--%>
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>

                </dxe:ASPxDateEdit>
                <%--Rev 1.0--%>
                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                <%--Rev end 1.0--%>
            </div>
             <div class="col-md-2">
                <label style="margin-bottom: 0">&nbsp</label>
                <div class="">
                    <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                    <% if (rights.CanExport)
                        { %> 
                            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="drdExport_SelectedIndexChanged"
                                AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                <asp:ListItem Value="2">XLSX</asp:ListItem>
                                <asp:ListItem Value="3">RTF</asp:ListItem>
                                <asp:ListItem Value="4">CSV</asp:ListItem>
                            </asp:DropDownList>
                    <% } %>
                </div>
            </div>
        </div>

        <div class="clearfix" onkeypress="OnWaitingGridKeyPress(event)">
            <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="cShowGrid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="Srlno"
                DataSourceID="GenerateEntityServerModeDataSource"  OnSummaryDisplayText="ShowGrid_SummaryDisplayText" KeyboardSupport="true" ClientSideEvents-EndCallback="Endgridanalysis">
                <Columns>
                    <dxe:GridViewDataTextColumn FieldName="Srlno" Caption="Sl." Width="4%" VisibleIndex="0" CellStyle-HorizontalAlign="Left">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="Category" Caption="Category" Width="12%" VisibleIndex="1">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="Class" Caption="Class" Width="12%" VisibleIndex="2">
                    </dxe:GridViewDataTextColumn>

                     <dxe:GridViewDataTextColumn FieldName="Particulars" Caption="Particulars" Width="24%" VisibleIndex="2">
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="sProducts_ID" Caption="Net Sale" Width="8%" Visible="false">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="Pu_Rate" Caption="Net Sale" Width="8%" Visible="false">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="Net_Sale" Caption="Net Sale" Width="8%" VisibleIndex="4">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="Rate" Caption="Rate" Width="8%" VisibleIndex="5">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="Sale_Value" Caption="Sale Value" Width="8%" VisibleIndex="6">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="Pu_Rate" Caption="Pur.Rate" Width="9%" VisibleIndex="7">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="Pu_Value" Caption="Pur.Value" Width="8%" VisibleIndex="8">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="Diff_Value" Caption="Diff. Value" Width="8%" VisibleIndex="9">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="Diff_P" Caption="Diff %" Width="8%" VisibleIndex="10" CellStyle-HorizontalAlign="Right">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                <SettingsEditing Mode="EditForm" />
                <SettingsContextMenu Enabled="true" />
                <SettingsBehavior AutoExpandAllGroups="true" />
                <Settings   ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" /> 
                <SettingsPager PageSize="10">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                </SettingsPager>
                <TotalSummary>
                    <dxe:ASPxSummaryItem FieldName="Net_Sale" SummaryType="Sum" />
                    <%--<dxe:ASPxSummaryItem FieldName="Rate" SummaryType="Sum" />--%>
                    <dxe:ASPxSummaryItem FieldName="Sale_Value" SummaryType="Sum" />
                    <%--<dxe:ASPxSummaryItem FieldName="Pu_Rate" SummaryType="Sum" />--%>
                    <dxe:ASPxSummaryItem FieldName="Pu_Value" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Diff_Value" SummaryType="Sum" />
                  <%--  <dxe:ASPxSummaryItem FieldName="Diff_P" SummaryType="Sum" />--%>
                </TotalSummary>
            </dxe:ASPxGridView>
             <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="SALESANALYSISSUMM_REPORT"></dx:LinqServerModeDataSource>
        </div>
    </div>
    </div>

    <dxe:ASPxPopupControl ID="popup" runat="server" ClientInstanceName="cpopup"
        Width="1000px" Height="600px" ScrollBars="Vertical" HeaderText="Sales Analysis Details" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ClientSideEvents Closing="function(s, e) {
	closePopup(s, e);}" />
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="col-md-12">
                    <div class="row clearfix">
                        <table class="pdbot" style="margin: 4px 0 16px 10px; float: left;">
                            <tr>
                                <td>
                                    <label style="color: #b5285f; font-weight: bold; margin-top: 5px;" class="clsTo">
                                        <asp:Label ID="Label5" runat="Server" Text="Product Code : " CssClass="mylabel1"></asp:Label>
                                    </label>
                                </td>
                                <td>
                                    <dxe:ASPxTextBox ID="txtProductCode" ClientInstanceName="ctxtProductCode" runat="server" ReadOnly="true" Width="600px"></dxe:ASPxTextBox>
                                </td>

                            </tr>
                            <tr>
                                <td>
                                    <label style="color: #b5285f; font-weight: bold; margin-top: 5px;" class="clsTo">
                                        <asp:Label ID="Label4" runat="Server" Text="Product Name : " CssClass="mylabel1"></asp:Label>
                                    </label>
                                </td>
                                <td>
                                    <dxe:ASPxTextBox ID="txtProductName" ClientInstanceName="ctxtProductName" runat="server" ReadOnly="true" Width="600px"></dxe:ASPxTextBox>
                                </td>

                            </tr>
                        </table>
                        <div class="pull-right" style="padding-top: 26px;">
                            <td style="float: right">Press Esc to Close</td>
                            <asp:DropDownList ID="ddldetails" runat="server" CssClass="btn btn-sm btn-primary" AutoPostBack="true" OnSelectedIndexChanged="ddldetails_SelectedIndexChanged">
                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                <asp:ListItem Value="2">XLSX</asp:ListItem>
                                <asp:ListItem Value="3">RTF</asp:ListItem>
                                <asp:ListItem Value="4">CSV</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class=" clearfix">
                    <dxe:ASPxGridView runat="server" ID="ShowGridDetails" ClientInstanceName="cShowGridDetails" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                         DataSourceID="GenerateEntityServerModeDetailsDataSource" 
                         OnSummaryDisplayText="ShowGridDetails_SummaryDisplayText">
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="Branch" Caption="Branch" Width="19%" VisibleIndex="0">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Sale" Caption="Sale" Width="9%" VisibleIndex="1">
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Return" Caption="Return" Width="9%" VisibleIndex="2">
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Net_Sale" Caption="Net Sale" Width="9%" VisibleIndex="3">
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Rate" Caption="Rate" Width="9%" VisibleIndex="4">
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Sale_Value" Caption="Sale Value" Width="9%" VisibleIndex="5">
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Pu_Rate" Caption="Pur.Rate" Width="9%" VisibleIndex="6">
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Pu_Value" Caption="Pur.Value" Width="9%" VisibleIndex="7">
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Diff_Value" Caption="Diff. Value" Width="9%" VisibleIndex="8">
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Diff_P" Caption="Diff. %" Width="9%" VisibleIndex="9" CellStyle-HorizontalAlign="Right">
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                        <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                        <SettingsEditing Mode="EditForm" />
                        <SettingsContextMenu Enabled="true" />
                        <SettingsBehavior AutoExpandAllGroups="true" />
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsSearchPanel Visible="false" />
                        <SettingsPager PageSize="10">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                        </SettingsPager>

                        <TotalSummary>
                            <dxe:ASPxSummaryItem FieldName="Sale" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Return" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Net_Sale" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Rate" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Sale_Value" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Pu_Rate" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Pu_Value" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Diff_Value" SummaryType="Sum" />
                            <%--<dxe:ASPxSummaryItem FieldName="Diff_P" SummaryType="Sum" />--%>
                        </TotalSummary>
                    </dxe:ASPxGridView>
                      <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDetailsDataSource" runat="server" OnSelecting="GenerateEntityServerModeDetailsDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="SALESANALYSISDET_REPORT"></dx:LinqServerModeDataSource>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents CloseUp="popupHide" />
    </dxe:ASPxPopupControl>
    <asp:HiddenField ID="hdnCategoryID" runat="server" />
    <asp:HiddenField ID="hdnClassID" runat="server" />
    <asp:HiddenField ID="hdnProductID" runat="server" />
    <asp:HiddenField ID="hdnRate" runat="server" />
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <dxe:ASPxGridViewExporter ID="exporterDetails" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel> 


  <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanelSumm" ClientInstanceName="cCallbackPanelSumm" OnCallback="CallbackPanelSumm_Callback">
    <PanelCollection>
        <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfIsSalesAnaSumm" runat="server" />
        </dxe:PanelContent>
    </PanelCollection>
    <ClientSideEvents EndCallback="CallbackPanelSummEndCall" />
 </dxe:ASPxCallbackPanel>

 <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanelDet" ClientInstanceName="cCallbackPanelDet" OnCallback="CallbackPanelDet_Callback">
    <PanelCollection>
        <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfIsSalesAnaDet" runat="server" />
        </dxe:PanelContent>
    </PanelCollection>
    <ClientSideEvents EndCallback="CallbackPanelDetEndCall" />
 </dxe:ASPxCallbackPanel>

</asp:Content>

