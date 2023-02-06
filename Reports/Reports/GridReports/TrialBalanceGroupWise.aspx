<%--======================================Revision History=========================================================================
1.0   V2.0.35     Debashis    06/02/2023      Enhancement required in Trial balance(Group wise) Report.
                                              Refer: 0025608
===================================End of Revision History=====================================================================--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="TrialBalanceGroupWise.aspx.cs" Inherits="Reports.Reports.GridReports.TrialBalanceGroupWise" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
     Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <style>
        .colDisable {
        cursor:default !important;
        }
        #pageControl, .dxtc-content {
            overflow: visible !important;
        }

        #MandatoryAssign {
            position: absolute;
            right: -17px;
            top: 6px;
        }
        .B {
          font-weight: bold;
        }

        #MandatorySupervisorAssign {
            position: absolute;
            right: 1px;
            top: 27px;
        }

        .chosen-container.chosen-container-multi,
        .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }

        #ListBoxBranches {
            width: 200px;
        }

        .hide {
            display: none;
        }

        /*.makebold label{
        font-weight:bold !important;
        }*/

        .dxtc-activeTab .dxtc-link {
            color: #fff !important;
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
    </style>


    <script type="text/javascript">
        $(function () {
            cProductbranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());

            $("#ddlbranchHO").change(function () {
                var Ids = $(this).val();
                cProductbranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            })
        });

        function Callback2_EndCallback() {
            if (Grid.cpErrorFinancial == 'ErrorFinancial') {
                jAlert('Date Range should be within Financial Year');
            }
            else {
                var Amount = parseFloat(Grid.cpSummary);
                Grid.cpSummary = null;
                $("#drdExport").val(0);
                Grid.Focus();
                Grid.SetFocusedRowIndex(0);
            }
            Grid.cpErrorFinancial = null;
        }
        var AsonWise = false;
        var ason = 'Y';
        $(document).ready(function () {
            document.getElementById("lblToDate").innerHTML = 'As On Date :'
            document.getElementById("dvFromdate").style.display = "none";
            document.getElementById("ASPxToDate").style.visibility = "visible";
            AsonWise = true;
        })

        function btn_ShowRecordsClick(e) {
            var data = "OnDateChanged";
            $("#hfIsTrialBalanceGrpWiseFilter").val("Y");
            $("#drdExport").val(0);
            var branchid = $('#ddlbranchHO').val();
            if (gridbranchLookup.GetValue() == null) {
                jAlert('Please select atleast one Branch for generate the report.');
            }
            else {
                cCallbackPanel.PerformCallback(data + '~' + AsonWise + '~' + branchid);
            }

            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
            if (AsonWise == false) {
                document.getElementById('<%=DateRange.ClientID %>').innerHTML = "For the period: " + FromDate + " To " + ToDate;
            }
            else {
                document.getElementById('<%=DateRange.ClientID %>').innerHTML = "As on: " + FromDate;
            }
        }
        function CallbackPanelEndCall(s, e) {
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
            Grid.Refresh();
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

        $(document).ready(function () {

            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');

            })
        })



        function DateAll(obj) {

            if (obj == 'all') {
                document.getElementById("lblToDate").innerHTML = 'As On Date :'
                document.getElementById("dvFromdate").style.display = "none";
                document.getElementById("ASPxToDate").style.visibility = "visible";
                AsonWise = true;
            }
            else {
                document.getElementById("lblToDate").style.visibility = "visible";
                document.getElementById("lblToDate").innerHTML = 'To Date :'
                document.getElementById("dvFromdate").style.display = "block";
                document.getElementById("ASPxToDate").style.visibility = "visible";
                AsonWise = false;
            }
        }

        function CheckConsPL(s, e) {
            if (s.GetCheckState() == 'Checked') {
                CchkOWMSTVT.SetEnabled(true);
                CchkConsLandCost.SetEnabled(true);
                CchkConsOverHeadCost.SetEnabled(true)
                $(<%=ddlValTech.ClientID%>).prop("disabled", false)
            }
            else {
                CchkOWMSTVT.SetCheckState('UnChecked');
                CchkOWMSTVT.SetEnabled(false);
                CchkConsLandCost.SetCheckState('UnChecked');
                CchkConsLandCost.SetEnabled(false);
                CchkConsOverHeadCost.SetCheckState('UnChecked');
                CchkConsOverHeadCost.SetEnabled(false);
                $(<%=ddlValTech.ClientID%>).prop("disabled", true)
                $("#ddlValTech").val("A");
            }
        }
    </script>

    <script type="text/javascript">

        function cxdeToDate_OnChaged(s, e) {

            var data = "OnDateChanged";
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
        }

        function OnContextMenuItemClick(sender, args) {
            if (args.item.name == "ExportToPDF" || args.item.name == "ExportToXLS") {
                args.processOnServer = true;
                args.usePostBack = true;
            } else if (args.item.name == "SumSelected")
                args.processOnServer = true;
        }

        function CloseGridLookupbranch() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }

        function selectAll_Branch() {
            gridbranchLookup.gridView.SelectRows();
        }

        function unselectAll_Branch() {
            gridbranchLookup.gridView.UnselectRows();
        }
        function selectAll_Group() {
            clookupGroup.gridView.SelectRows();
        }

        function unselectAll_Group() {
            clookupGroup.gridView.UnselectRows();
        }
        function CloseLookup_Group() {
            clookupGroup.ConfirmCurrentSelection();
            clookupGroup.HideDropDown();
            clookupGroup.Focus();
        }


        function AddCSSClassToCell() {
            for (var i = 0; i < 1000; i++) {
                if (ShowGrid.GetRow(i)) {
                    if (ShowGrid.GetRow(i).style.display != "none") {
                        if (ShowGrid.GetRow(i)) {

                        }
                    }
                }
            }

            for (i = -1; i > -1000; i--) {
                if (ShowGrid.GetRow(i)) {
                    if (ShowGrid.GetRow(i).style.display != "none") {
                        if (ShowGrid.GetRow(i)) {

                        }
                    }
                }
            }
        }

        function OnEndCallback(){
            AddCSSClassToCell()
}

    </script>   

    <style>
        .padtbl > tbody > tr > td {
            padding-right: 10px;
        }
        input[type="checkbox"] {
            -webkit-transform: translateY(2px);
            -moz-transform: translateY(2px);
            transform: translateY(2px);
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
    </style>
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
                        <asp:Label ID="CompBranch" runat="Server" Text="" Width="470px" ></asp:Label>
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

    <div class="form_main">
        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />
        <div class="row">
            <div class="col-md-2" style="padding-top: 23px;color: #b5285f;">
                <table class="padtbl">
                    <tr >
                        <td>
                            <asp:RadioButton ID="radAsDate" runat="server" Checked="True" GroupName="a1" />
                        </td>
                        <td>As On Date
                        </td>
                        <td>
                            <asp:RadioButton ID="radPeriod" runat="server" GroupName="a1" />
                        </td>
                        <td>Period
                        </td>
                    </tr>
                </table>
            </div>
            <div class="col-md-2" id="dvFromdate">
                <label style="color: #b5285f;" class="clsFrom">
                    <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                    <buttonstyle width="13px">
                        </buttonstyle>
                </dxe:ASPxDateEdit>
            </div>
            <div class="col-md-2" id="dvtodate">
                <label style="color: #b5285f;" class="clsTo">
                    <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                    <buttonstyle width="13px">
                    </buttonstyle>
                </dxe:ASPxDateEdit>
            </div>

            <div class="col-md-2">
                <div style="color: #b5285f;" class="clsTo">
                    <label style="color: #b5285f;" class="clsTo">
                        <asp:Label ID="Label2" runat="Server" Text="Head Branch : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </label>
                </div>
                <asp:DropDownList ID="ddlbranchHO" runat="server" Width="100%"></asp:DropDownList>
            </div>


            <div class="col-md-2 branch-selection-box">
                <label style="color: #b5285f;" class="clsTo">
                    <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </label>
                <asp:HiddenField ID="hdnActivityType" runat="server" />
                <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                <span id="MandatoryActivityType" style="display: none" class="validclass">
                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                <asp:HiddenField ID="hdnSelectedBranches" runat="server" />

                <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cProductbranchPanel" OnCallback="Componentbranch_Callback">
                    <panelcollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="lookup_branch" SelectionMode="Multiple" runat="server" ClientInstanceName="gridbranchLookup"
                                    OnDataBinding="lookup_branch_DataBinding"
                                    KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
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
                                                           <%-- <div class="hide">--%>
                                                                <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_Branch" />
                                                           <%-- </div>--%>
                                                            <dxe:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_Branch" />                                                            
                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookupbranch" UseSubmitBehavior="False" />
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
                        </panelcollection>

                </dxe:ASPxCallbackPanel>
            </div>
            

            <div class="col-md-2">
                <label style="color: #b5285f;" class="clsTo">
                    <asp:Label ID="Label4" runat="Server" Text="Group : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </label>
                <dxe:ASPxCallbackPanel runat="server" ID="GroupPanel" ClientInstanceName="cGroupPanel" OnCallback="GroupPanel_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookupGroup" ClientInstanceName="clookupGroup" SelectionMode="Multiple" runat="server"
                                OnDataBinding="lookupGroup_DataBinding" KeyFieldName="ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
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
                                                            <dxe:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_Group" UseSubmitBehavior="False"/>
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton7" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_Group" UseSubmitBehavior="False"/>
                                                        <dxe:ASPxButton ID="ASPxButton8" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseLookup_Group" UseSubmitBehavior="False" />
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
            <div class="clear"></div>
            <div class="col-md-2" style="padding-top: 15px;color: #b5285f;">
                <div style="padding-right: 10px; vertical-align: middle; padding-top: 6px">
                    <asp:CheckBox ID="chkZero" runat="server" Checked="false"/>
                    Show Zero Value Account
                </div>
            </div>

            <%--<div class="col-md-2" style="padding-top: 16px">
                <div style="padding-right: 10px; vertical-align: middle; padding-top: 6px">
                    <asp:CheckBox ID="chkPL" runat="server" />
                    Consider Closing Stock
                </div>
            </div>--%>            

            <div class="col-md-2" style="margin-top:15px;color: #b5285f; font-weight: bold;">
             <dxe:ASPxCheckBox runat="server" ID="chkPL" Checked="false" Text="Consider Closing Stock" >
                 <ClientSideEvents CheckedChanged="CheckConsPL" />
             </dxe:ASPxCheckBox>
            </div>

             <div class="col-md-2" style="padding-top: 1px;color: #b5285f;">
                <div style="padding-right: 10px; vertical-align: middle; padding-top: 1px">
                    <asp:Label ID="Label7" runat="Server" Text="Valuation Technique : " CssClass="mylabel1"></asp:Label>
                </div>
                <div>
                    <asp:DropDownList ID="ddlValTech" runat="server" Width="100%" Enabled="false">
                        <asp:ListItem Text="Average" Value="A"></asp:ListItem>
                        <asp:ListItem Text="FIFO" Value="F"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="col-md-3" style="margin-top:15px;color: #b5285f; font-weight: bold;">
                <dxe:ASPxCheckBox runat="server" ID="chkOWMSTVT" Checked="false" ClientEnabled="false" Text="Override Product Valuation Technique in Master" ClientInstanceName="CchkOWMSTVT">
                </dxe:ASPxCheckBox>
            </div> 

            <%--<div class="col-md-3" style="margin-top:15px;color: #b5285f; font-weight: bold;">
             <dxe:ASPxCheckBox runat="server" ID="chkSLOPBal" Checked="false" Text="Consider Subledger wise Opening" >
             </dxe:ASPxCheckBox>
            </div>--%>
            <div class="col-md-3" style="margin-top:15px;color: #b5285f; font-weight: bold;">
            <dxe:ASPxCheckBox runat="server" ID="chkConsLandCost" Checked="false" ClientEnabled="false" Text="Consider Landed Cost" ClientInstanceName="CchkConsLandCost">
                </dxe:ASPxCheckBox>
            </div> 
            <div class="clear"></div>
            <div class="col-md-2" style="margin-top:8px;color: #b5285f; font-weight: bold;">
                <dxe:ASPxCheckBox runat="server" ID="chkConsOverHeadCost" Checked="false" ClientEnabled="false" Text="Consider Overhead Cost" ClientInstanceName="CchkConsOverHeadCost">
                </dxe:ASPxCheckBox>
            </div>
            <div class="col-md-3" style="padding-top: 1px;color: #b5285f;">
                <div style="padding-right: 10px; vertical-align: middle; padding-top: 6px">
                    <%--<asp:CheckBox ID="chkSLOPBal" runat="server" Checked="false" Text="Consider Subledger wise Opening"/>--%>
                    <dxe:ASPxCheckBox runat="server" ID="chkSLOPBal" Checked="false" Text="Consider Subledger wise Opening">
                    </dxe:ASPxCheckBox>
                </div>
            </div>
           <div class="col-md-2" style="padding-top: 1px">
                <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                <% if (rights.CanExport)
                    { %> 
                   <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                        OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                       <%--Rev 1.0 Mantis: 0025608--%>
                        <%--<asp:ListItem Value="1">XLSX</asp:ListItem>
                        <asp:ListItem Value="2">PDF</asp:ListItem>
                        <asp:ListItem Value="3">CSV</asp:ListItem>
                        <asp:ListItem Value="4">RTF</asp:ListItem>--%>
                        <asp:ListItem Value="1">EXCEL</asp:ListItem>
                        <asp:ListItem Value="2">PDF</asp:ListItem>
                        <asp:ListItem Value="3">CSV</asp:ListItem>
                       <%--End of Rev 1.0 Mantis: 0025608--%>
                   </asp:DropDownList>
                <% } %>
            </div>
            <div class="clear"></div>
            <div class="col-md-2"></div>
        </div>
        <table class="pull-left">
            <tr>
                <td></td>
                <td style="width: 254px"></td>
                <asp:HiddenField ID="HiddenField1" runat="server" />
                <span id="MandatoryCustomerType" style="display: none" class="validclass">
                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                </td>
            </tr>
        </table>

        <table class="TableMain100">
            <tr>
                <td colspan="2">
                    <div onkeypress="OnWaitingGridKeyPress(event)">
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyboardSupport="true" KeyFieldName="SEQ"
                             DataSourceID="GenerateEntityServerModeDataSource"  OnDataBinding="ShowGrid_DataBinding" OnSummaryDisplayText="ShowGrid_SummaryDisplayText" OnHtmlDataCellPrepared="ShowGrid_HtmlDataCellPrepared"
                            SettingsBehavior-AllowFocusedRow="true" SettingsBehavior-AllowSelectSingleRowOnly="true" Settings-HorizontalScrollBarMode="Visible" OnHtmlFooterCellPrepared="ShowGrid_HtmlFooterCellPrepared">
                            <%--OnDataBound="ShowGrid_DataBound" --%>
                            <columns>
                                <dxe:GridViewDataTextColumn FieldName="BOLD_GRP" Caption="BOLD_GRP" Width="0%" VisibleIndex="1" HeaderStyle-CssClass="colDisable"/>                                 
                                
                                <dxe:GridViewDataTextColumn FieldName="LEDGER" Caption="Group/Ledger/Sub Ledger" Width="60%" VisibleIndex="1" HeaderStyle-CssClass="colDisable">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="OP_DR" Caption="Dr.(Opening)" Width="25%" VisibleIndex="3" PropertiesTextEdit-DisplayFormatString="0.00" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="OP_CR" Caption="Cr.(Opening)" Width="25%" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="0.00" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                
                                <dxe:GridViewDataTextColumn FieldName="PR_DR" Caption="Dr.(Period)" Width="25%" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="0.00" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PR_CR" Caption="Cr.(Period)" Width="25%" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.00" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CLOSE_DR" Caption="Dr.(Closing)" Width="25%" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="0.00" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CLOSE_CR" Caption="Cr.(Closing)" Width="25%" VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="0.00" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                            </columns>
                            <settingsbehavior confirmdelete="true" enablecustomizationwindow="true" enablerowhottrack="true" />
                            <settings showfooter="true" showgrouppanel="false" showgroupfooter="VisibleIfExpanded" />
                            <settingsediting mode="EditForm" />
                            <settingscontextmenu enabled="true" />
                            <settingsbehavior autoexpandallgroups="true" columnresizemode="Control" AllowSort="False"/>
                            <settings showgrouppanel="false" showstatusbar="Visible" showfilterrow="true" />
                            <settingssearchpanel visible="False" />
                            <settingspager mode="ShowAllRecords"></settingspager>
                            <settings showfilterrow="True" showfilterrowmenu="true" showstatusbar="Visible" usefixedtablelayout="true" />

                            <totalsummary>
                                <dxe:ASPxSummaryItem FieldName="LEDGER" SummaryType="Custom" Tag="Item_Ledger"/>
                                <dxe:ASPxSummaryItem FieldName="OP_DR" SummaryType="Custom" Tag="Item_OpDr"/>
                                <dxe:ASPxSummaryItem FieldName="OP_CR" SummaryType="Custom" Tag="Item_OpCr"/>
                                <dxe:ASPxSummaryItem FieldName="PR_DR" SummaryType="Custom" Tag="Item_PrDr"/>
                                <dxe:ASPxSummaryItem FieldName="PR_CR" SummaryType="Custom" Tag="Item_PrCr"/>
                                <dxe:ASPxSummaryItem FieldName="CLOSE_DR" SummaryType="Custom" Tag="Item_ClDr"/>
                                <dxe:ASPxSummaryItem FieldName="CLOSE_CR" SummaryType="Custom" Tag="Item_ClCr"/>
                            </totalsummary>
                            <clientsideevents endcallback="Callback2_EndCallback" />
                        </dxe:ASPxGridView>
                         <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                            ContextTypeName="ReportSourceDataContext" TableName="TRIALBALANCEGROUPWISE_REPORT"></dx:LinqServerModeDataSource>
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

<dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
    <PanelCollection>
        <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfIsTrialBalanceGrpWiseFilter" runat="server" />
        </dxe:PanelContent>
    </PanelCollection>
    <ClientSideEvents EndCallback="CallbackPanelEndCall" />
</dxe:ASPxCallbackPanel>

</asp:Content>