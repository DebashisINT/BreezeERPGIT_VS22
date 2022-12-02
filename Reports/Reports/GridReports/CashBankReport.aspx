<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="CashBankReport.aspx.cs" 
    Inherits="Reports.Reports.GridReports.CashBankReport" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .colDisable {
        cursor:default !important;
        }
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

        #ListBoxProjects{
            width: 200px;
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

    <script>
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
            clookupCashBank.gridView.SelectRows();
        }

        function unselectAll() {
            clookupCashBank.gridView.UnselectRows();
        }

        function CloseLookup() {
            clookupCashBank.ConfirmCurrentSelection();
            clookupCashBank.HideDropDown();
            clookupCashBank.Focus();
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

        //function select_All() {
        //    clookupUser.gridView.SelectRows();
        //}

        //function unselect_All() {
        //    clookupUser.gridView.UnselectRows();
        //}

        //function Close_Lookup() {
        //    clookupUser.ConfirmCurrentSelection();
        //    clookupUser.HideDropDown();
        //    clookupUser.Focus();
        //}

        //function _select_All() {
        //    clookupLedger.gridView.SelectRows();
        //}

        //function _unselect_All() {
        //    clookupLedger.gridView.UnselectRows();
        //}

        //function _Close_Lookup() {
        //    clookupLedger.ConfirmCurrentSelection();
        //    clookupLedger.HideDropDown();
        //    clookupLedger.Focus();
        //}

        function BindOtherDetails(e) {
            clookupCashBank.gridView.SetFocusedRowIndex(-1);
            //clookupUser.gridView.SetFocusedRowIndex(-1);
            //clookupLedger.gridView.SetFocusedRowIndex(-1);

            //cLedgerPanel.PerformCallback();
            //cUserPanel.PerformCallback();
            cCashBankPanel.PerformCallback();
        }

        //function ddlType_Change() {
        //    clookupCashBank.gridView.SetFocusedRowIndex(-1);
        //    cCashBankPanel.PerformCallback();
        //}
        function ddlType_Change(opts) {
            if (opts.value == 'Bank') {
                chkchqno.disabled = false;
                chkchqdt.disabled = false;
                chkchqonbnk.disabled = false;
            }
            else {
                chkchqno.disabled = true;
                chkchqno.checked = false;
                chkchqdt.disabled = true;
                chkchqdt.checked = false;
                chkchqonbnk.disabled = true;
                chkchqonbnk.checked = false;
            }
            clookupCashBank.gridView.SetFocusedRowIndex(-1);
            cCashBankPanel.PerformCallback();
        }

        $(function () {
            cBranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());

            $("#ddlbranchHO").change(function () {
                var Ids = $(this).val();
                cBranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());

            })
        });

        $(function () {
            cProjectPanel.PerformCallback('BindProjectGrid');
        });

        $(document).ready(function () {
            var ProjectSelection = document.getElementById('hdnProjectSelection').value;
            if (ProjectSelection == "0") {
                $('#divProj').addClass('hidden');
            }
            else {
                $('#divProj').removeClass('hidden');
            }
        })

        function btn_ShowRecordsClick(e) {
            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            var ProjectSelection = document.getElementById('hdnProjectSelection').value;
            var ProjectSelectionInReport = document.getElementById('hdnProjectSelectionInReport').value;
            $("#drdExport").val(0);
            $("#hfIsCashBank").val("Y");
            //if (clookupBranch.GetValue() == null) {
            //    jAlert('Please select atleast one branch');
            //}
            //else {
            //
            //cCallbackPanel.PerformCallback($('#ddlbranchHO').val());
            <%--}--%>

            if (BranchSelection == "Yes" && clookupBranch.GetValue() == null) {
                jAlert('Please select atleast one branch for generate the report.');
            }
            else if (ProjectSelection == "1") {
                if (ProjectSelectionInReport == "Yes" && gridprojectLookup.GetValue() == null) {
                    jAlert('Please select atleast one Project for generate the report.');
                }
                else if (BranchSelection == "Yes" && clookupBranch.GetValue() == null) {
                    jAlert('Please select atleast one branch for generate the report.');
                }
                else {
                    cCallbackPanel.PerformCallback($('#ddlbranchHO').val());
                }
            }
            else {
                cCallbackPanel.PerformCallback($('#ddlbranchHO').val());
            }

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
        function CallbackPanelEndCall(s, e) {
             <%--Rev Subhra 13-12-2018   0017670--%>
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
            //End of Subhra
            cShowGrid.Refresh();
        }
    </script>
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
            <h3>Cash/Bank Book - Detail</h3>
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
                   <%--Rev Subhra 13-12-2018   0017670--%>
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
    <div class="form_main">
        <div class="row">
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsFrom">
                    <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
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
            </div>

            <div class="col-md-2">
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
            <%--<div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label5" runat="Server" Text="User : " CssClass="mylabel1"></asp:Label></label>--%>
                <%--<dxe:ASPxCallbackPanel runat="server" ID="UserPanel" ClientInstanceName="cUserPanel" OnCallback="UserPanel_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookupUser" ClientInstanceName="clookupUser" SelectionMode="Multiple" runat="server"
                                OnDataBinding="lookupUser_DataBinding" KeyFieldName="ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>--%>
                                   <%-- <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="User" Visible="true" VisibleIndex="1" Caption="User Name" Width="250px" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="ID" Visible="true" VisibleIndex="2" Caption="ID" Settings-AutoFilterCondition="Contains" Width="0">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>
                                </Columns>--%>
                               <%-- <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                    <Templates>
                                        <StatusBar>
                                            <table class="OptionsTable" style="float: right">
                                                <tr>
                                                    <td>
                                                        <%--<div class="hide">--%>
                                                            <%--<dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="select_All" />--%>
                                                       <%-- </div>--%>
                                                        <%--<dxe:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselect_All" />--%>
                                                        <%--<dxe:ASPxButton ID="ASPxButton5" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="Close_Lookup" UseSubmitBehavior="False" />--%>
                                                   <%-- </td>
                                                </tr>
                                            </table>
                                        </StatusBar>--%>
                                    <%--</Templates>--%>
                                    <%--<SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                    <SettingsPager Mode="ShowPager">
                                    </SettingsPager>--%>
                                    <%--<SettingsPager PageSize="20">
                                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                    </SettingsPager>
                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                <%--</GridViewProperties>
                            </dxe:ASPxGridLookup>
                        </dxe:PanelContent>
                    </PanelCollection>
                </dxe:ASPxCallbackPanel>
                <span id="MandatorClass" style="display: none" class="validclass" />
            </div>--%>

            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label4" runat="Server" Text="Cash/Bank Type : " CssClass="mylabel1"></asp:Label>
                </label>
                <%--<asp:DropDownList ID="ddlType" runat="server" Width="100%" onchange="ddlType_Change()">--%>
                <asp:DropDownList ID="ddlType" runat="server" Width="100%" onchange="ddlType_Change(this)">
                    <asp:ListItem Text="Bank" Value="Bank"></asp:ListItem>
                    <asp:ListItem Text="Cash" Value="Cash"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label1" runat="Server" Text="Cash/Bank : " CssClass="mylabel1"></asp:Label></label>
                <dxe:ASPxCallbackPanel runat="server" ID="CashBankPanel" ClientInstanceName="cCashBankPanel" OnCallback="CashBankPanel_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookupCashBank" ClientInstanceName="clookupCashBank" SelectionMode="Multiple" runat="server"
                                OnDataBinding="lookupCashBank_DataBinding" KeyFieldName="ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
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
            <div class="clear"></div>
            <%--<div class="col-md-2" style="padding-top: 10px;">
                <div style="color: #b5285f;" class="clsTo">
                    <asp:CheckBox ID="chkledger" runat="server" Checked="false"></asp:CheckBox>
                    <asp:Label ID="Label6" runat="Server" Text="Show Ledger Description" CssClass="mylabel1"></asp:Label>
                </div>
            </div>--%>
            <div class="col-md-2" style="padding-top: 10px;">
                <div style="color: #b5285f;" class="clsTo">
                    <asp:CheckBox ID="chksubledger" runat="server" Checked="false"></asp:CheckBox>
                    <asp:Label ID="Label2" runat="Server" Text="Show Sub Ledger" CssClass="mylabel1"></asp:Label>
                </div>
            </div>
            <div class="col-md-2" style="padding-top: 10px;">
                <div style="color: #b5285f;" class="clsTo">
                    <asp:CheckBox ID="chkchqno" runat="server" Checked="false"></asp:CheckBox>
                    <asp:Label ID="Label7" runat="Server" Text="Show Cheque No." CssClass="mylabel1"></asp:Label>
                </div>
            </div>
            <div class="col-md-2" style="padding-top: 10px;">
                <div style="color: #b5285f;" class="clsTo">
                    <asp:CheckBox ID="chkchqdt" runat="server" Checked="false"></asp:CheckBox>
                    <asp:Label ID="Label8" runat="Server" Text="Show Cheque Date" CssClass="mylabel1"></asp:Label>
                </div>
            </div>
            <div class="col-md-2" style="padding-top: 10px;">
                <div style="color: #b5285f;" class="clsTo">
                    <asp:CheckBox ID="chkchqonbnk" runat="server" Checked="false"></asp:CheckBox>
                    <asp:Label ID="Label9" runat="Server" Text="Show Cheque on Bank" CssClass="mylabel1"></asp:Label>
                </div>
            </div>
            <div class="col-md-2" style="padding-top: 10px;">
                <div style="color: #b5285f;" class="clsTo">
                    <asp:CheckBox ID="chkhdnarra" runat="server" Checked="false"></asp:CheckBox>
                    <asp:Label ID="Label10" runat="Server" Text="Show Header Narration" CssClass="mylabel1"></asp:Label>
                </div>
            </div>
            <div class="col-md-2" style="padding-top: 10px;">
                <div style="color: #b5285f;" class="clsTo">
                    <asp:CheckBox ID="chksortondate" runat="server" Checked="false"></asp:CheckBox>
                    <asp:Label ID="Label6" runat="Server" Text="Sort On Date" CssClass="mylabel1"></asp:Label>
                </div>
            </div>
            <%--<div class="col-md-2" style="display: none;">--%>
                <%--<label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label6" runat="Server" Text="Ledger : " CssClass="mylabel1"></asp:Label></label>--%>
                <%--<dxe:ASPxCallbackPanel runat="server" ID="LedgerPanel" ClientInstanceName="cLedgerPanel" OnCallback="LedgerPanel_Callback">
                    <PanelCollection>--%>
                        <%--<dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookupLedger" ClientInstanceName="clookupLedger" SelectionMode="Multiple" runat="server"
                                OnDataBinding="lookupLedger_DataBinding" KeyFieldName="ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>--%>
                                    <%--<dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="Doc_Code" Visible="true" VisibleIndex="1" Caption="Doc Code" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="Description" Visible="true" VisibleIndex="2" Caption="Description" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="ID" Visible="true" VisibleIndex="3" Caption="ID" Settings-AutoFilterCondition="Contains" Width="0">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>--%>
                                <%--</Columns>--%>
                                <%--<GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                    <Templates>
                                        <StatusBar>--%>
                                           <%-- <table class="OptionsTable" style="float: right">
                                                <tr>
                                                    <td>--%>
                                                       <%-- <div class="hide">--%>
                                                            <%--<dxe:ASPxButton ID="ASPxButton9" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="_select_All" />--%>
                                                        <%--</div>--%>
                                                        <%--<dxe:ASPxButton ID="ASPxButton10" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="_unselect_All" />
                                                        <dxe:ASPxButton ID="ASPxButton11" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="_Close_Lookup" UseSubmitBehavior="False" />--%>
                                                   <%-- </td>
                                                </tr>
                                            </table>
                                        </StatusBar>
                                    </Templates>--%>
                                    <%--<SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                    <SettingsPager Mode="ShowPager">
                                    </SettingsPager>--%>
                                    <%--<SettingsPager PageSize="20">
                                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                    </SettingsPager>--%>
                                    <%--<Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                </GridViewProperties>--%>
                            <%--</dxe:ASPxGridLookup>
                        </dxe:PanelContent>
                    </PanelCollection>
                </dxe:ASPxCallbackPanel>--%>
                <%--<span id="MandatorClass" style="display: none" class="validclass" />--%>
            <%--</div>--%>
            <div class="clear mBot10"></div>
           <%-- <div class="col-md-2">--%>
            <div class="col-md-2" id="divProj">
                <div style="color: #b5285f; /*font-weight: bold;*/" class="clsTo">
                    <asp:Label ID="lblProj" runat="Server" Text="Project : " CssClass="mylabel1"></asp:Label>
                </div>
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
                                                            <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_Project" UseSubmitBehavior="False"/>
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_Project" UseSubmitBehavior="False"/>                                                        
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
            <div class="col-md-4" style="padding: 12px 15px 7px 15px;">
                <%--<label style="margin-bottom: 0">&nbsp</label>--%>
                <div class="">
                    <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                    <% if (rights.CanExport)
                       { %>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="drdExport_SelectedIndexChanged"
                        AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">XLSX</asp:ListItem>
                        <asp:ListItem Value="2">PDF</asp:ListItem>
                        <asp:ListItem Value="3">CSV</asp:ListItem>
                        <asp:ListItem Value="4">RTF</asp:ListItem>
                    </asp:DropDownList>
                    <% } %>
                </div>
            </div>
               <%-- </div>--%>
        </div>
        <div class="clearfix mtop5">
            <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="cShowGrid" Width="100%" EnableRowsCache="False" AutoGenerateColumns="False" KeyFieldName="SLNO"
                DataSourceID="GenerateEntityServerModeDataSource" 
                OnSummaryDisplayText="ShowGrid_SummaryDisplayText" Settings-HorizontalScrollBarMode="Visible"  >
                <Columns>
                    <dxe:GridViewDataTextColumn FieldName="CASHBANKNAME" Caption="Cash/Bank Name" Width="150" VisibleIndex="0" GroupIndex="0">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="SLNO" Caption="Sl." Width="0" VisibleIndex="1" HeaderStyle-CssClass="colDisable">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataComboBoxColumn FieldName="BRANCH_DESC" Caption="Unit" Width="200px" VisibleIndex="2" HeaderStyle-CssClass="colDisable">
                        <PropertiesComboBox DataSourceID="sqlbranch" TextField="branch_description"
                            ValueField="branch_description" ValueType="System.String" EnableSynchronization="False" EnableIncrementalFiltering="True">
                        </PropertiesComboBox>
                    </dxe:GridViewDataComboBoxColumn>
                    <dxe:GridViewDataTextColumn FieldName="MAINACCOUNT" Caption="Ledger Desc." Width="250px" VisibleIndex="3" HeaderStyle-CssClass="colDisable">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="DOCUMENT_NO" Caption="Voucher No." Width="170px" VisibleIndex="4" HeaderStyle-CssClass="colDisable">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="TRAN_DATE" Caption="Voucher Date" Width="120px" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" HeaderStyle-CssClass="colDisable">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="INSTRUMENT_NO" Caption="Cheque Number" Width="130px" VisibleIndex="6" HeaderStyle-CssClass="colDisable">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="INSTRUMENT_DATE" Caption="Cheque Date" Width="80px" VisibleIndex="7" HeaderStyle-CssClass="colDisable">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="DRAWEE_BANK" Caption="Cheque On Bank" Width="200px" VisibleIndex="8" HeaderStyle-CssClass="colDisable">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="TRAN_TYPE" Caption="Doc. Type" Width="200px" VisibleIndex="9" HeaderStyle-CssClass="colDisable">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="PROJ_NAME" Caption="Project Name" Width="200px" VisibleIndex="10" HeaderStyle-CssClass="colDisable">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="PARTICULARS" Caption="Particulars" Width="470px" VisibleIndex="11" HeaderStyle-CssClass="colDisable">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="HEADER_NARRATION" Caption="Header Narration" Width="300px" VisibleIndex="12" HeaderStyle-CssClass="colDisable">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="SUBLEDGER" Caption="Subledger" Width="250px" VisibleIndex="13" HeaderStyle-CssClass="colDisable">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="DEBIT" Caption="Debit" Width="130px" VisibleIndex="14" HeaderStyle-CssClass="colDisable">
                        <%--<PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>--%>
                        <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                        <CellStyle HorizontalAlign="Right"></CellStyle>
                        <HeaderStyle HorizontalAlign="Right" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="CREDIT" Caption="Credit" Width="130px" VisibleIndex="15" HeaderStyle-CssClass="colDisable">
                        <%--<PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>--%>
                         <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                        <CellStyle HorizontalAlign="Right"></CellStyle>
                        <HeaderStyle HorizontalAlign="Right" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="CLOSING_BALANCE" Caption="Balance" Width="130px" VisibleIndex="16" HeaderStyle-CssClass="colDisable" FooterCellStyle-HorizontalAlign="Right" GroupFooterCellStyle-HorizontalAlign="Right">
                        <%--<PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>--%>
                         <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                        <CellStyle HorizontalAlign="Right"></CellStyle>
                        <HeaderStyle HorizontalAlign="Right" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="CLOSEBAL_DBCR" Caption="DbCr." Width="90px" VisibleIndex="17" HeaderStyle-CssClass="colDisable">
                    </dxe:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                <SettingsEditing Mode="EditForm" />
                <SettingsContextMenu Enabled="true" />
                <SettingsBehavior AutoExpandAllGroups="true" AllowSort="False"/>
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
                    <dxe:ASPxSummaryItem FieldName="DEBIT" ShowInGroupFooterColumn="DEBIT" SummaryType="SUM" Tag="Item_Debit" DisplayFormat="C" />
                    <dxe:ASPxSummaryItem FieldName="CREDIT" ShowInGroupFooterColumn="CREDIT" SummaryType="SUM" Tag="Item_Credit" />
                    <dxe:ASPxSummaryItem FieldName="CLOSING_BALANCE" ShowInGroupFooterColumn="CLOSING_BALANCE" SummaryType="Custom" Tag="Item_Balance" />
                    <dxe:ASPxSummaryItem FieldName="CLOSEBAL_DBCR" ShowInGroupFooterColumn="CLOSEBAL_DBCR" SummaryType="Custom" Tag="Item_DBCR" />
                </GroupSummary>
            </dxe:ASPxGridView>
             <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="CASHCARDBANKBOOK_REPORT"></dx:LinqServerModeDataSource>
        </div>
    </div>
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <asp:SqlDataSource ID="sqlbranch" runat="server" 
        SelectCommand="select branch_id,branch_description from tbl_master_branch"></asp:SqlDataSource>

  <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
    <PanelCollection>
        <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfIsCashBank" runat="server" />
        </dxe:PanelContent>
    </PanelCollection>
    <ClientSideEvents EndCallback="CallbackPanelEndCall" />
 </dxe:ASPxCallbackPanel>
    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
    <asp:HiddenField ID="hdnProjectSelection" runat="server" />
    <asp:HiddenField ID="hdnProjectSelectionInReport" runat="server" />
    </span>

</asp:Content>
