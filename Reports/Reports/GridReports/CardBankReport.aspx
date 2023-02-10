<%--====================================================== Revision History ==========================================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                10-02-2023        2.0.36           Pallab              25575 : Report pages modification
====================================================== Revision History ==========================================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="CardBankReport.aspx.cs" 
    Inherits="Reports.Reports.GridReports.CardBankReport" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        $(document).ready(function () {
            document.getElementById('lblDate').innerHTML = cdtASondate.date.format("dd-MM-yyyy");
        });

        function _selectAll() {
            clookupBranch.gridView.SelectRows();
        }

        function _unselectAll() {
            clookupBranch.gridView.UnselectRows();
        }

        function _CloseLookup() {
            clookupBranch.ConfirmCurrentSelection();
            clookupBranch.HideDropDown();
            clookupBranch.Focus();
        }

        function selectAll() {
            clookupCardBank.gridView.SelectRows();
        }

        function unselectAll() {
            clookupCardBank.gridView.UnselectRows();
        }

        function CloseLookup() {
            clookupCardBank.ConfirmCurrentSelection();
            clookupCardBank.HideDropDown();
            clookupCardBank.Focus();
        }

        function BindOtherDetails(e) {
            clookupCardBank.gridView.SetFocusedRowIndex(-1);
            cCardBankPanel.PerformCallback();
        }

        function ddlType_Change() {
            clookupCardBank.gridView.SetFocusedRowIndex(-1);
            cCardBankPanel.PerformCallback();
        }

        function DateChangeForAsOn() {
            var data = "";
            data = new Date(cdtASondate.date)
            //data = cdtASondate.date.format("dd/MM/yyyy");
            document.getElementById('lblDate').innerHTML = cdtASondate.date.format("dd-MM-yyyy");
            cxdeFromDate.SetDate(data);
            cxdeToDate.SetDate(data);
        }

        $(function () {
            cBranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            $("#ddlbranchHO").change(function () {
                var Ids = $(this).val();
                cBranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            })
        });

        function btn_ShowRecordsClick(e) {
            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            $("#drdExport").val(0);
            $("#hfIsCardBank").val("Y");
            
            //if (clookupBranch.GetValue() == null) {
            //    jAlert('Please select atleast one branch');
            //}
            //else {
            //    cShowGrid.PerformCallback();
            //}
            //cCallbackPanel.PerformCallback($('#ddlbranchHO').val());

            if (BranchSelection == "Yes" && clookupBranch.GetValue() == null) {
                jAlert('Please select atleast one branch for generate the report.');
            }
            else {
                cCallbackPanel.PerformCallback($('#ddlbranchHO').val());
            }

            var OnDate = (cdtASondate.GetValue() != null) ? cdtASondate.GetValue() : "";

            OnDate = GetDateFormat(OnDate);
            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "On Date: " + OnDate;
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

        function PrevDate(e) {
            var prevdate = new Date(cxdeToDate.date);
            //var prevdate = new Date(cdtASondate.date);
            prevdate.setDate(prevdate.getDate() - 1);

            if (cdtASondate.GetMinDate() <= prevdate) {
                //cxdeFromDate.SetDate(prevdate);
                cxdeToDate.SetDate(prevdate);
                document.getElementById('lblDate').innerHTML = cxdeToDate.GetDate().format("dd-MM-yyyy");
                return true;
            }
            else {
                return false;
            }
        }

        function NextDate(e) {

            var nextdate = new Date(cxdeToDate.date);
            //var nextdate = new Date(cdtASondate.date);
            nextdate.setDate(nextdate.getDate() + 1);
            if (cdtASondate.GetMaxDate() >= nextdate) {
                cxdeToDate.SetDate(nextdate);
                //cxdeToDate.SetDate(nextdate);
                document.getElementById('lblDate').innerHTML = cxdeToDate.date.format("dd-MM-yyyy");
                return true;
            }
            else {
                return false;
            }
           

        }
        function CallbackPanelEndCall(s, e) {
              <%--Rev Subhra 11-12-2018   0017670--%>
                document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
                //End of Subhra
            cShowGrid.Refresh();
        }
    </script>

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
            bottom: 180px;
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

        .form_main {
            overflow: hidden;
        }

        label , .mylabel1
        {
            color: #141414 !important;
            font-size: 14px;
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

        #ASPxFromDate , #ASPxToDate , #ASPxASondate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img
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
            top: 31px;
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
       <%-- <div class="panel-title">
            <h3>Card/Bank Book</h3>
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
                    <%--Rev Subhra 11-12-2018   0017670--%>
                    <div>
                        <asp:Label ID="CompBranch" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                  <%--End of Rev--%>
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
    <%--Rev 1.0 : "outer-div-main" class add--%>
    <div class="outer-div-main">
    <div class="form_main">
        <div class="row">
            <%--Rev 1.0--%>
            <%--<div class="col-md-1">--%>
              <div class="col-md-1 pt-25">
                  <%--Rev end 1.0--%>
                <%-- <label style="color: #b5285f; font-weight: bold;" class="clsFrom">
                    <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"></asp:Label>
                </label>--%>
                <div style="display: none;">
                    <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </div>
             <%--   <button id="btnPrevDate" class="btn btn-primary" type="button" onclick="PrevDate(this);">Prev Day</button>--%>
                <%--<button id="btnPrevDate" class="btn btn-primary" type="button" onclick="PrevDate(this);btn_ShowRecordsClick(this);">Prev Day</button>--%>
                <button id="btnPrevDate" class="btn btn-primary" type="button" onclick="if(PrevDate(this)){btn_ShowRecordsClick(this);}">Prev Day</button>

            </div>
            <%--Rev 1.0--%>
            <%--<div class="col-md-1">--%>
              <div class="col-md-1 pt-25">
                  <%--Rev end 1.0--%>
                <%--  <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </label>--%>
                <div style="display: none;">
                    <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </div>
                <%--<button id="btnNextDate" class="btn btn-primary" type="button" onclick="NextDate(this);">Next Day</button>--%>
               <%-- <button id="btnNextDate" class="btn btn-primary" type="button" onclick="NextDate(this);btn_ShowRecordsClick(this);">Next Day</button>--%>
                   <button id="btnNextDate" class="btn btn-primary" type="button" onclick="if(NextDate(this)){btn_ShowRecordsClick(this);}">Next Day</button>
            </div>
            <div class="col-md-1">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="lblToDate" runat="Server" Text="Date : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </label>
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="lblDate" runat="Server" CssClass="mylabel1"></asp:Label>
                </label>
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label6" runat="Server" Text="On Date : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </label>
               <dxe:ASPxDateEdit ID="ASPxASondate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cdtASondate">
                    <buttonstyle width="13px">
                        </buttonstyle>
                         <ClientSideEvents DateChanged="function(s,e){DateChangeForAsOn();}" />
                </dxe:ASPxDateEdit>
                <%--Rev 1.0--%>
                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                <%--Rev end 1.0--%>
            </div>
            <%--Rev 1.0--%>
            <%--<div class="col-md-2">--%>
            <div class="col-md-2 simple-select">
                <%--Rev end 1.0--%>
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label5" runat="Server" Text="Head Branch : " CssClass="mylabel1"></asp:Label></label>
                </div>
                <asp:DropDownList ID="ddlbranchHO" runat="server" Width="100%"></asp:DropDownList>
            </div>
            <div class="col-md-2 branch-selection-box">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label3" runat="Server" Text="Branch : " CssClass="mylabel1"></asp:Label></label>
                <asp:HiddenField ID="hdnSelectedBranches" runat="server" />
                <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cBranchPanel" OnCallback="Componentbranch_Callback">
                    <panelcollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookupBranch" ClientInstanceName="clookupBranch" SelectionMode="Multiple" runat="server"
                                OnDataBinding="lookupBranch_DataBinding" KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                        <dxe:GridViewDataColumn FieldName="branch_code" Visible="true" VisibleIndex="1" width="200px" Caption="Branch code" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>

                                        <dxe:GridViewDataColumn FieldName="branch_description" Visible="true" VisibleIndex="2" width="200px" Caption="Branch Name" Settings-AutoFilterCondition="Contains">
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
                                                            <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="_selectAll" />
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="_unselectAll" />
                                                        <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="_CloseLookup" UseSubmitBehavior="False" />
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
                                <ClientSideEvents TextChanged="function(s, e) { BindOtherDetails(e)}" />
                            </dxe:ASPxGridLookup>
                            <span id="MandatoryActivityType" style="display: none" class="validclass" />
                        </dxe:PanelContent>
                    </panelcollection>
                </dxe:ASPxCallbackPanel>
            </div>
           
            <%--Rev 1.0--%>
            <%--<div class="col-md-2">--%>
            <div class="col-md-2 simple-select">
                <%--Rev end 1.0--%>
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label4" runat="Server" Text="Card/Bank Type : " CssClass="mylabel1"></asp:Label>
                </label>
                <asp:DropDownList ID="ddlType" runat="server" Width="100%" onchange="ddlType_Change()">
                    <asp:ListItem Text="Bank" Value="Bank"></asp:ListItem>
                    <asp:ListItem Text="Card" Value="Card"></asp:ListItem>
                </asp:DropDownList>
            </div>
             <div class="clear"></div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label1" runat="Server" Text="Card/Bank : " CssClass="mylabel1"></asp:Label></label>
                <dxe:ASPxCallbackPanel runat="server" ID="CardBankPanel" ClientInstanceName="cCardBankPanel" OnCallback="CardBankPanel_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookupCardBank" ClientInstanceName="clookupCardBank" SelectionMode="Multiple" runat="server"
                                OnDataBinding="lookupCardBank_DataBinding" KeyFieldName="ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="Name" Visible="true" VisibleIndex="1" Caption="Name" Settings-AutoFilterCondition="Contains" Width="250px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="ID" Visible="true" VisibleIndex="2" Caption="ID" Settings-AutoFilterCondition="Contains" Width="0">
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
                                                            <dxe:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll" />
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton7" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll" />                                                        
                                                        <dxe:ASPxButton ID="ASPxButton8" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseLookup" UseSubmitBehavior="False" />
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
                        </dxe:PanelContent>
                    </PanelCollection>
                </dxe:ASPxCallbackPanel>
                <span id="MandatorClass" style="display: none" class="validclass" />
            </div>
            
            <div class="col-md-2" style="padding-top: 25px;">
                <div style="color: #b5285f;" class="clsTo">
                    <asp:CheckBox ID="chksubledger" runat="server" Checked="false"></asp:CheckBox>
                    <asp:Label ID="Label2" runat="Server" Text="Show Sub Ledger" CssClass="mylabel1"></asp:Label>
                </div>
            </div>

            <div class="col-md-2">
                <label style="margin-bottom: 0">&nbsp</label>
                <div class="">
                    <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="drdExport_SelectedIndexChanged"
                        AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLSX</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="clearfix">
            <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="cShowGrid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                DataSourceID="GenerateEntityServerModeDataSource" KeyFieldName="SEQ"
                OnSummaryDisplayText="ShowGrid_SummaryDisplayText" Settings-HorizontalScrollBarMode="Visible"  Settings-VerticalScrollBarMode="Auto"  
                Settings-VerticalScrollableHeight="400">
                
                <Columns>
                    <dxe:GridViewDataTextColumn FieldName="CASHBANKNAME" Caption="Card/Bank Name" Width="150" VisibleIndex="0" GroupIndex="0">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="SLNO" Caption="Sl." Width="50" VisibleIndex="1">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataComboBoxColumn FieldName="BRANCH_DESC" Caption="Branch" Width="100" VisibleIndex="2">
                        <PropertiesComboBox DataSourceID="sqlbranch" TextField="branch_description"
                            ValueField="branch_description" ValueType="System.String" EnableSynchronization="False" EnableIncrementalFiltering="True">
                        </PropertiesComboBox>
                    </dxe:GridViewDataComboBoxColumn>
                    <dxe:GridViewDataTextColumn FieldName="MAINACCOUNT" Caption="Ledger Desc." Width="350" VisibleIndex="3">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="DOCUMENT_NO" Caption="Voucher No." Width="130" VisibleIndex="4">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="TRAN_DATE" Caption="Voucher Date" Width="80" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="INSTRUMENT_NO" Caption="Cheque Number" Width="130" VisibleIndex="6">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="INSTRUMENT_DATE" Caption="Cheque Date" Width="80" VisibleIndex="7">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="DRAWEE_BANK" Caption="Cheque On Bank" Width="200" VisibleIndex="8">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="TRAN_TYPE" Caption="Doc. Type" Width="200" VisibleIndex="9">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="PARTICULARS" Caption="Particulars" Width="300" VisibleIndex="10">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="HEADER_NARRATION" Caption="Header Narration" Width="300" VisibleIndex="11">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="SUBLEDGER" Caption="Subledger" Width="250" VisibleIndex="12">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="DEBIT" Caption="Debit" Width="100" VisibleIndex="13">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                         <CellStyle HorizontalAlign="Right"></CellStyle>
                        <HeaderStyle HorizontalAlign="Right" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="CREDIT" Caption="Credit" Width="100" VisibleIndex="14">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                         <CellStyle HorizontalAlign="Right"></CellStyle>
                        <HeaderStyle HorizontalAlign="Right" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="CLOSING_BALANCE" Caption="Balance" Width="100" VisibleIndex="15" FooterCellStyle-HorizontalAlign="Right" GroupFooterCellStyle-HorizontalAlign="Right">
                     <%--   <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>--%>
                           <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                          <CellStyle HorizontalAlign="Right"></CellStyle>
                        <HeaderStyle HorizontalAlign="Right" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="CLOSEBAL_DBCR" Caption="DBCR" Width="50" VisibleIndex="16">
                    </dxe:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                <SettingsEditing Mode="EditForm" />
                <SettingsContextMenu Enabled="true" />
                <SettingsBehavior AutoExpandAllGroups="true" />
                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                <SettingsSearchPanel Visible="false" />
                <SettingsPager PageSize="50">
                    <%--<PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />--%>
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                </SettingsPager>
                <TotalSummary>
                    <dxe:ASPxSummaryItem FieldName="PARTICULARS" SummaryType="Custom" />
                    <dxe:ASPxSummaryItem FieldName="DEBIT" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="CREDIT" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="CLOSING_BALANCE" SummaryType="Custom" />
                    <dxe:ASPxSummaryItem FieldName="CLOSEBAL_DBCR" SummaryType="Custom" />
                </TotalSummary>
                <GroupSummary>
                    <%--<dxe:ASPxSummaryItem FieldName="Particulars" ShowInGroupFooterColumn="Particulars" SummaryType="Custom" Tag="Item_Particulars" />--%>
                    <dxe:ASPxSummaryItem FieldName="DEBIT" ShowInGroupFooterColumn="DEBIT" SummaryType="Sum" Tag="Item_Debit" />
                    <dxe:ASPxSummaryItem FieldName="CREDIT" ShowInGroupFooterColumn="CREDIT" SummaryType="Sum" Tag="Item_Credit" />
                    <dxe:ASPxSummaryItem FieldName="CLOSING_BALANCE" ShowInGroupFooterColumn="CLOSING_BALANCE" SummaryType="Custom" Tag="Item_Balance" />
                    <dxe:ASPxSummaryItem FieldName="CLOSEBAL_DBCR" ShowInGroupFooterColumn="CLOSEBAL_DBCR" SummaryType="Custom" Tag="Item_DBCR" />
                </GroupSummary>
            </dxe:ASPxGridView>
            <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="CASHCARDBANKBOOK_REPORT"></dx:LinqServerModeDataSource>
        </div>
    </div>
        </div>
    <%--Rev end 1.0--%>
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <asp:SqlDataSource ID="sqlbranch" runat="server" 
        SelectCommand="select branch_id,branch_description from tbl_master_branch"></asp:SqlDataSource>

      <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            <asp:HiddenField ID="hfIsCardBank" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
     </dxe:ASPxCallbackPanel>

      <asp:HiddenField ID="hfIsMaxDate" runat="server" />
      <asp:HiddenField ID="hfIsMinDate" runat="server" />
      <asp:HiddenField ID="hdnBranchSelection" runat="server" />
</asp:Content>
