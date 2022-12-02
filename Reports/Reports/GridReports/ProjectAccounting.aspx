<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ProjectAccounting.aspx.cs" Inherits="Reports.Reports.GridReports.ProjectAccounting" %>

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
        #ListBoxProjects{
            width: 200px;
        }
        .hide {
            display: none;
        }
        .dxtc-activeTab .dxtc-link {
            color: #fff !important;
        }
        .pad32 {
        padding-left:32px;
        }
        .pad52 {
        padding-left:52px;
        }
        .makebold label{
        font-weight:bold !important;
        }
    </style>


    <script type="text/javascript">
        $(function () {
            cProductbranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            $("#ddlbranchHO").change(function () {
                var Ids = $(this).val();
                cProductbranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            })
        });

        $(function () {
            cProjectPanel.PerformCallback('BindProjectGrid');
        });
        //for Esc        
        
        var AsonWise = false;
        var ason = 'Y';
        $(document).ready(function () {
            document.getElementById("lblToDate").innerHTML = 'As On Date :'
            document.getElementById("dvFromdate").style.display = "none";
            document.getElementById("ASPxToDate").style.visibility = "visible";
            AsonWise = true;
        })

        function btn_ShowRecordsClick(e) {
            var data = "Grid";
            $("#hfIsProjectAccountingSumFilter").val("Y");
            $("#drdExport").val(0);
            var branchid = $('#ddlbranchHO').val();
            var ProjectSelection = document.getElementById('hdnProjectSelection').value;
            var ProjectSelectionInReport = document.getElementById('hdnProjectSelectionInReport').value;
            if (gridbranchLookup.GetValue() == null) {
                jAlert('Please select atleast one Branch for generate the report.');
            }
            else if (ProjectSelection == "1") {
                if (ProjectSelectionInReport == "Yes" && gridprojectLookup.GetValue() == null) {
                    jAlert('Please select atleast one Project for generate the report.');
                }
                else {
                    cCallbackPanel.PerformCallback(data + '~' + branchid);
                }
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

            var ProjectSelection = document.getElementById('hdnProjectSelection').value;
            if (ProjectSelection == "0") {
                $('#divProj').addClass('hidden');
            }
            else {
                $('#divProj').removeClass('hidden');
            }
        })
        function DateAll(obj) {
            if (obj == 'all') {
                document.getElementById("lblToDate").innerHTML = 'As On Date :'
                document.getElementById("dvFromdate").style.display = "none";
                document.getElementById("ASPxToDate").style.visibility = "visible";
                CchkHierarchy.SetEnabled(true);
                CchkGroup.SetEnabled(true);
                CchkOpAsOnDt.SetEnabled(true);
                CchkNetDrCrOpCl.SetEnabled(false);
                CchkNetDrCrOpCl.SetCheckState('UnChecked');
                CchkPGRP.SetEnabled(true);
                AsonWise = true;
            }
            else {
                document.getElementById("lblToDate").style.visibility = "visible";
                document.getElementById("lblToDate").innerHTML = 'To Date :'
                document.getElementById("dvFromdate").style.display = "block";
                document.getElementById("ASPxToDate").style.visibility = "visible";
                CchkHierarchy.SetCheckState('UnChecked');
                CchkHierarchy.SetEnabled(false);
                CchkGroup.SetCheckState('UnChecked');
                CchkGroup.SetEnabled(false);
                CchkOpAsOnDt.SetCheckState('UnChecked');
                CchkOpAsOnDt.SetEnabled(false);
                CchkNetDrCrOpCl.SetEnabled(true);
                CchkPGRP.SetEnabled(true);
                AsonWise = false;
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

        
        function selectAll_Branch() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAll_Branch() {
            gridbranchLookup.gridView.UnselectRows();
        }
        function CloseGridLookupbranch() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }

        function selectAll_Project() {
            gridprojectLookup.gridView.SelectRows();
        }
        function unselectAll_Project() {
            gridprojectLookup.gridView.UnselectRows();
        }
        function CloseGridProjectLookup() {
            gridprojectLookup.ConfirmCurrentSelection();
            gridprojectLookup.HideDropDown();
            gridprojectLookup.Focus();
        }

    </script>
    <%--end 3rd level popup--%>
    <script type="text/javascript">
        function DateChangeForFrom() {
            var sessionValFrom = "<%=Session["FinYearStart"]%>";
            var sessionValTo = "<%=Session["FinYearEnd"]%>";
            var sessionVal = "<%=Session["LastFinYear"]%>";
            var objsession = sessionVal.split('-');
            var MonthDate = cxdeFromDate.GetDate().getMonth() + 1;
            var DayDate = cxdeFromDate.GetDate().getDate();
            var YearDate = cxdeFromDate.GetDate().getYear();
            var Cdate = MonthDate + "/" + DayDate + "/" + YearDate;
            var Sto = new Date(sessionValTo).getMonth() + 1;
            var SFrom = new Date(sessionValFrom).getMonth() + 1;
            var SDto = new Date(sessionValTo).getDate();
            var SDFrom = new Date(sessionValFrom).getDate();
            if (YearDate >= objsession[0]) {
                if (MonthDate < SFrom && YearDate == objsession[0]) {
                    jAlert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                    cxdeFromDate.SetDate(new Date(datePost));
                }
                else if (MonthDate > Sto && YearDate == objsession[1]) {
                    jAlert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                    cxdeFromDate.SetDate(new Date(datePost));
                }
                else if (YearDate != objsession[0] && YearDate != objsession[1]) {
                    jAlert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                    cxdeFromDate.SetDate(new Date(datePost));
                }
            }
            else {
                jAlert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                cxdeFromDate.SetDate(new Date(datePost));
            }
        }
        function DateChangeForTo() {
            var sessionValFrom = "<%=Session["FinYearStart"]%>";
            var sessionValTo = "<%=Session["FinYearEnd"]%>";
            var sessionVal = "<%=Session["LastFinYear"]%>";
            var objsession = sessionVal.split('-');
            var MonthDate = cxdeToDate.GetDate().getMonth() + 1;
            var DayDate = cxdeToDate.GetDate().getDate();
            var YearDate = cxdeToDate.GetDate().getYear();
            var Cdate = MonthDate + "/" + DayDate + "/" + YearDate;
            var Sto = new Date(sessionValTo).getMonth() + 1;
            var SFrom = new Date(sessionValFrom).getMonth() + 1;
            var SDto = new Date(sessionValTo).getDate();
            var SDFrom = new Date(sessionValFrom).getDate();
            if (YearDate <= objsession[1]) {
                if (MonthDate < SFrom && YearDate == objsession[0]) {
                    jAlert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
                    cxdeToDate.SetDate(new Date(datePost));
                }
                else if (MonthDate > Sto && YearDate == objsession[1]) {
                    jAlert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
                    cxdeToDate.SetDate(new Date(datePost));
                }
                else if (YearDate != objsession[0] && YearDate != objsession[1]) {
                    jAlert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
                    cxdeToDate.SetDate(new Date(datePost));
                }
            }
            else {
                jAlert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
                cxdeToDate.SetDate(new Date(datePost));
            }
        }
    </script>
    <%--end 3rd level popup--%>
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
        table.dxeDisabled_PlasticBlue{
            width:100%;
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
        <div class="clearfix" style=" background: #f1f1f1;border: 1px solid #d2d2d2;padding: 14px 0px;border-radius: 5px;">
            <div class="col-md-2" style="padding-top: 23px;color: #b5285f; font-weight: bold;" >
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
                <label style="color: #b5285f; font-weight: bold;" class="clsFrom">
                    <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"
                        Width="92px" Font-Bold="True"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                    <buttonstyle width="13px">
                        </buttonstyle>
                </dxe:ASPxDateEdit>
            </div>
            <div class="col-md-2" id="dvtodate">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"
                        Width="92px" Font-Bold="True"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                    <buttonstyle width="13px">
                    </buttonstyle>
                </dxe:ASPxDateEdit>
            </div>

            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label2" runat="Server" Text="Head Branch : " CssClass="mylabel1"
                            Width="92px" Font-Bold="True"></asp:Label>
                    </label>
                </div>
                <asp:DropDownList ID="ddlbranchHO" runat="server" Width="100%"></asp:DropDownList>
            </div>

            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"
                        Width="92px" Font-Bold="True"></asp:Label>
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

            <div class="col-md-2" id="divProj">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label3" runat="Server" Text="Project : " CssClass="mylabel1"></asp:Label></label>
                <asp:ListBox ID="ListBoxProjects" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="60px" Width="100%" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>

                <dxe:ASPxCallbackPanel runat="server" ID="ProjectPanel" ClientInstanceName="cProjectPanel" OnCallback="Project_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookup_project" SelectionMode="Multiple" runat="server" ClientInstanceName="gridprojectLookup"
                                OnDataBinding="lookup_project_DataBinding"
                                KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="project_code" Visible="true" VisibleIndex="1" width="200px" Caption="Project code" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>

                                    <dxe:GridViewDataColumn FieldName="project_name" Visible="true" VisibleIndex="2" width="200px" Caption="Project Name" Settings-AutoFilterCondition="Contains">
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
                                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_Project" UseSubmitBehavior="False"/>
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_Project" UseSubmitBehavior="False"/>                                                        
                                                        <dxe:ASPxButton ID="ASPxButton5" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridProjectLookup" UseSubmitBehavior="False" />
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

                <span id="MandatoryActivityType" style="display: none" class="validclass">
                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                <asp:HiddenField ID="hdnSelectedProjects" runat="server" />
            </div>

            <div class="clear"></div>
            <div class="col-md-2" style="padding-top: 8px">
                <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>               
                 <% if (rights.CanExport)
                       { %>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                        OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">EXCEL</asp:ListItem>
                        <asp:ListItem Value="2">PDF</asp:ListItem>
                        <asp:ListItem Value="3">CSV</asp:ListItem>
                    </asp:DropDownList>
                 <% } %>
            </div>
            <div class="clear"></div>
            <div class="col-md-2"></div>
        </div>

        <table class="TableMain100" style="margin-top:10px">
            <tr>
                <td colspan="2">
                    <div>
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyboardSupport="true" KeyFieldName="SEQ"
                             DataSourceID="GenerateEntityServerModeDataSource" OnSummaryDisplayText="ShowGrid_SummaryDisplayText" 
                            SettingsBehavior-AllowFocusedRow="true" SettingsBehavior-AllowSelectSingleRowOnly="true" Settings-HorizontalScrollBarMode="Visible">
                            <columns>
                                <dxe:GridViewDataTextColumn FieldName="BRANCHDESC" Caption="Unit" Width="300px" VisibleIndex="1" HeaderStyle-CssClass="colDisable">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PROJ_CODE" Caption ="Project Code" Width="200px" VisibleIndex="2" HeaderStyle-CssClass="colDisable">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PROJ_NAME" Caption ="Project Name" Width="300px" VisibleIndex="3" HeaderStyle-CssClass="colDisable">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="ACCCODE" Caption ="Account Code" Width="200px" VisibleIndex="4" HeaderStyle-CssClass="colDisable">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="ACCNAME" Caption ="Account Name" Width="300px" VisibleIndex="5" HeaderStyle-CssClass="colDisable">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="AMOUNT" Caption="Amount" Width="120px" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                            </columns>
                            <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
                            <settingsbehavior confirmdelete="true" enablecustomizationwindow="true" enablerowhottrack="true" />
                            <settings showfooter="true" showgrouppanel="false" showgroupfooter="VisibleIfExpanded" showstatusbar="Visible" showfilterrow="true"/>
                            <settingsediting mode="EditForm" />
                            <settingscontextmenu enabled="true" />
                            <settingsbehavior AutoExpandAllGroups="true" AllowSort="False" columnresizemode="Control" />
                            <settingssearchpanel visible="False" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>
                            <settings showfilterrow="True" showfilterrowmenu="true" showstatusbar="Visible" usefixedtablelayout="true" />

                            <totalsummary>
                                <dxe:ASPxSummaryItem FieldName="BRANCHDESC" SummaryType="Custom" Tag="Branch"/>
                                <dxe:ASPxSummaryItem FieldName="AMOUNT" SummaryType="Sum" Tag="Amount"/>
                            </totalsummary>
                        </dxe:ASPxGridView>
                         <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                            ContextTypeName="ReportSourceDataContext" TableName="PROJECTACCOUNTING_REPORT"></dx:LinqServerModeDataSource>
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
            <asp:HiddenField ID="hfIsProjectAccountingSumFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>
    <asp:HiddenField ID="hdnProjectSelection" runat="server" />
    <asp:HiddenField ID="hdnProjectSelectionInReport" runat="server" />
</asp:Content>
