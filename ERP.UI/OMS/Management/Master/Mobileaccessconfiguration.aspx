<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" CodeBehind="Mobileaccessconfiguration.aspx.cs" Inherits="ERP.OMS.Management.Master.Mobileaccessconfiguration" Title="Tab Configuration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        var status = "Add";
        var ReturnData = "";
        var uniqueid = "";
        $(document).ready(function () {
            debugger;
            cglSalesman.SetEnabled(false);
            cglManager.SetEnabled(false);
            cglTopApprovers.SetEnabled(false);
            cglFinancer.SetEnabled(false);
            //finalSaveButtonState();
        });

        function BranchIndexChange(s, e) {
            debugger;
            cglSalesman.SetEnabled(true);
            cglManager.SetEnabled(true);
            cglTopApprovers.SetEnabled(true);
            cglFinancer.SetEnabled(true);
            cCustPanel.PerformCallback(s.GetValue());

        }
        function CustPanelEndCallBack(s, e) {
            cglSalesman.gridView.Refresh();
            cglManager.gridView.Refresh();
            cglFinancer.gridView.Refresh();
            cglTopApprovers.gridView.Refresh();

        }

        function saveClientClick(s, e) {
            debugger;
            if (validAllData()) {
                if (status == 'Edit') {
                    cmobileconfigGrid.PerformCallback("EditDone" + "~" + uniqueid);
                }
                else {
                    status = "Add";
                    cmobileconfigGrid.PerformCallback(status);
                }


                //SetEnable(true);
                // ClearClientClick();
            }
        }

        function TabImei(s, e) {

            var url = 'TabIMEI.aspx';

            window.location.href = url;
        }


        function OpeningGridEndCallBack(s, e) {

            if (cmobileconfigGrid.cpRedirect) {
                if (cmobileconfigGrid.cpRedirect != "") {
                    window.location.href = cmobileconfigGrid.cpRedirect;
                    cmobileconfigGrid.cpRedirect = null;
                }
            }

            if (cmobileconfigGrid.cpBlankAlert) {
                if (cmobileconfigGrid.cpBlankAlert != "") {
                    jAlert(cmobileconfigGrid.cpBlankAlert);
                    cmobileconfigGrid.cpBlankAlert = null;
                }
            }

            if (cmobileconfigGrid.cpProfileDuplicacy) {
                if (cmobileconfigGrid.cpProfileDuplicacy != "") {
                    jAlert(cmobileconfigGrid.cpProfileDuplicacy);
                    cmobileconfigGrid.cpProfileDuplicacy = null;
                }
            }

            if (status == "Edit") {
                if (cmobileconfigGrid.cpBeforeEdit) {
                    if (cmobileconfigGrid.cpBeforeEdit != "") {
                        ReturnData = cmobileconfigGrid.cpBeforeEdit;
                        if (ReturnData.split('~')[1] == "No")
                            ccheckIsactive.SetChecked(false);
                        else
                            ccheckIsactive.SetChecked(true);

                        ccmbBranch.PerformCallback(ReturnData.split('~')[2]);
                        cCustPanel.PerformCallback(ReturnData.split('~')[2] + '~' + status);
                        cmobileconfigGrid.cpBeforeEdit = null;
                    }
                }
            }

            if (cmobileconfigGrid.cpClientMsg) {
                if (cmobileconfigGrid.cpClientMsg != "") {
                    jAlert(cmobileconfigGrid.cpClientMsg);
                    cmobileconfigGrid.cpClientMsg = null;
                }
            }
            ccmbBranch.Focus();
        }

        function onMobileConfigEdit(obj) {
            debugger;
            uniqueid = obj;
            $('#MandatoryBranch').css({ 'display': 'none' });
            $('#MandatorySalesman').css({ 'display': 'none' });
            status = "Edit";
            SetEnable(false);
            cmobileconfigGrid.PerformCallback(status + "~" + obj);
        }

        function DelMobileConfig(obj) {
            debugger;
            status = "Delete";
            SetEnable(false);
            cmobileconfigGrid.PerformCallback(status + "~" + obj);
        }

        function SetEnable(state) {
            //ccmbBranch.SetEnabled(state);

        }


        function FinalSaveClientClick(s, e) {
            cmobileconfigGrid.PerformCallback('SaveAllRecord');
        }

        function ClearClientClick(s, e) {
            debugger;
            $('#MandatoryBranch').css({ 'display': 'none' });
            $('#MandatorySalesman').css({ 'display': 'none' });
            $('#MandatoryManager').css({ 'display': 'none' });
            $('#MandatoryFinancer').css({ 'display': 'none' });
            ccmbBranch.SetSelectedIndex(-1);
            ccheckIsactive.SetChecked(false);
            status = "Add";
            cglSalesman.enabled = false;
            cglManager.enabled = false;
            cglFinancer.enabled = false;
            cglTopApprovers.enabled = false;
            cCustPanel.PerformCallback(0);
            ccmbBranch.PerformCallback(0);
            ccmbBranch.Focus();
        }
        function validAllData() {
            var retdata = true;
            $('#MandatoryBranch').css({ 'display': 'none' });
            $('#MandatoryAccount').css({ 'display': 'none' });

            if (ccmbBranch.GetValue() == null) {
                $('#MandatoryBranch').css({ 'display': 'block' });
                retdata = false;
            }
            //if (ccmbSalesman.GetValue() == null) {
            //    $('#MandatoryAccount').css({ 'display': 'block' });
            //    retdata = false;
            //}
            return retdata;

        }

        $(function () {
            var vAnotherKeyWasPressed = false;
            var ALT_CODE = 18;

            $(window).keydown(function (event) {
                var vKey = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
                vAnotherKeyWasPressed = vKey != ALT_CODE;
                if (event.altKey && (event.key == 's' || event.key == 'S')) {
                    if (parseFloat(ctotalDebit.GetValue()) == parseFloat(totalCredit.GetValue())) {
                        if (cFinalSave) {
                            FinalSaveClientClick();
                            return false;
                        }
                    }
                }

            });

            $(window).keyup(function (event) {

                var vKey = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;

            });
        });

        function CloseGridLookup() {
            cglSalesman.ConfirmCurrentSelection();
            cglSalesman.HideDropDown();
            cglSalesman.Focus();
        }
        function CloseManagerGridLookup() {
            cglManager.ConfirmCurrentSelection();
            cglManager.HideDropDown();
            cglManager.Focus();
        }
        function CloseTopApprovalGridLookup() {
            cglTopApprovers.ConfirmCurrentSelection();
            cglTopApprovers.HideDropDown();
            cglTopApprovers.Focus();
        }
        function CloseFinancerGridLookup() {
            cglFinancer.ConfirmCurrentSelection();
            cglFinancer.HideDropDown();
            cglFinancer.Focus();
        }
    </script>
    <style>
        .dxeErrorFrameSys.dxeErrorCellSys,
        .pullleftClass {
            position: absolute;
            right: 6px;
            top: 27px;
        }

        .lead {
            font-size: 18px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cmobileconfigGrid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cmobileconfigGrid.SetWidth(cntWidth);
            }
            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cmobileconfigGrid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cmobileconfigGrid.SetWidth(cntWidth);
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">Tab Configuration</h3>
            <div id="pageheaderContent" class="pull-right wrapHolder content horizontal-images" style="">
            </div>
        </div>
        <div class="crossBtn"><a href="../ProjectMainPage.aspx"><i class="fa fa-times"></i></a></div>
    </div>
    <div class="form_main">


        <table width="100%" style="margin-bottom: 5px;">

            <tr>
                <td style="padding-right: 25px; width: 120px" class="relative">
                    <label></label>
                    <dxe:ASPxCheckBox ID="checkIsactive" ClientInstanceName="ccheckIsactive" runat="server" Text="Is Active?">
                    </dxe:ASPxCheckBox>
                </td>

                <td style="padding-right: 25px" class="relative">
                    <label>Select Branch</label>
                    <dxe:ASPxComboBox ID="cmbBranch" ClientInstanceName="ccmbBranch" runat="server" DataSourceID="dsBranch" Width="100%"
                        ValueType="System.String" AutoPostBack="false" ValueField="BANKBRANCH_ID" TextField="BANKBRANCH_NAME"
                        EnableIncrementalFiltering="true" EnableSynchronization="False" OnCallback="cmbBranch_Callback">
                        <ClientSideEvents SelectedIndexChanged="BranchIndexChange" />
                    </dxe:ASPxComboBox>


                    <span id="MandatoryBranch" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; display: none; padding-left: 9px; right: 6px; top: 31px;" title="Mandatory"></span>
                </td>


                <td style="padding-right: 25px" class="relative">


                    <dxe:ASPxCallbackPanel runat="server" ID="CustPanel" ClientInstanceName="cCustPanel" OnCallback="CustPanel_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="padding-right: 25px" class="relative">
                                            <label>Select Salesman</label>
                                            <dxe:ASPxGridLookup ID="glSalesman" runat="server" SelectionMode="Multiple" ClientInstanceName="cglSalesman"
                                                KeyFieldName="Code" Width="100%" TextFormatString="{0}" MultiTextSeparator=", " OnDataBinding="glSalesman_DataBinding">
                                                <Columns>
                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption=" " />

                                                    <dxe:GridViewDataColumn FieldName="Name" Caption="Salesman" Width="300" />
                                                </Columns>
                                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">

                                                    <Templates>
                                                        <StatusBar>
                                                            <table class="OptionsTable" style="float: right">
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </StatusBar>
                                                    </Templates>
                                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                </GridViewProperties>
                                            </dxe:ASPxGridLookup>
                                        </td>
                                        <td style="padding-right: 25px" class="relative">
                                            <label>Select Manager</label>
                                            <dxe:ASPxGridLookup ID="glManager" runat="server" SelectionMode="Multiple" ClientInstanceName="cglManager"
                                                KeyFieldName="Code" Width="100%" TextFormatString="{0}" MultiTextSeparator=", " OnDataBinding="glManager_DataBinding">
                                                <Columns>
                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption=" " />

                                                    <dxe:GridViewDataColumn FieldName="Name" Caption="Manager" Width="300" />
                                                </Columns>
                                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">

                                                    <Templates>
                                                        <StatusBar>
                                                            <table class="OptionsTable" style="float: right">
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseManagerGridLookup" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </StatusBar>
                                                    </Templates>
                                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                </GridViewProperties>
                                            </dxe:ASPxGridLookup>
                                        </td>
                                        <td style="padding-right: 25px" class="relative">
                                            <label>Select Top Approver's</label>
                                            <dxe:ASPxGridLookup ID="glTopApproval" runat="server" SelectionMode="Multiple" ClientInstanceName="cglTopApprovers"
                                                KeyFieldName="Code" Width="100%" TextFormatString="{0}" MultiTextSeparator=", " OnDataBinding="glTopApproval_DataBinding">
                                                <Columns>
                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption="Top Approver's" />

                                                    <dxe:GridViewDataColumn FieldName="Name" Caption="" Width="300" />
                                                </Columns>
                                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">

                                                    <Templates>
                                                        <StatusBar>
                                                            <table class="OptionsTable" style="float: right">
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseTopApprovalGridLookup" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </StatusBar>
                                                    </Templates>
                                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                </GridViewProperties>
                                            </dxe:ASPxGridLookup>
                                        </td>
                                        <td style="" class="relative">
                                            <label>Select Financer</label>
                                            <dxe:ASPxGridLookup ID="glFinancer" runat="server" SelectionMode="Multiple" ClientInstanceName="cglFinancer"
                                                KeyFieldName="Code" Width="100%" TextFormatString="{0}" MultiTextSeparator=", " OnDataBinding="glFinancer_DataBinding">
                                                <Columns>
                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption=" " />

                                                    <dxe:GridViewDataColumn FieldName="Name" Caption="Financer" Width="300" />
                                                </Columns>
                                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">

                                                    <Templates>
                                                        <StatusBar>
                                                            <table class="OptionsTable" style="float: right">
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseFinancerGridLookup" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </StatusBar>
                                                    </Templates>
                                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                </GridViewProperties>
                                            </dxe:ASPxGridLookup>
                                        </td>
                                    </tr>
                                </table>

                            </dxe:PanelContent>
                        </PanelCollection>
                        <ClientSideEvents EndCallback="CustPanelEndCallBack" />
                    </dxe:ASPxCallbackPanel>
                    <%--<span id="MandatoryLookUp" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute;right:-9px;top:35px;display:none" title="Mandatory"></span>--%>

                </td>



                <td class="relative" style="width: 165px; padding-top: 19px;">
                    <label>&nbsp</label>
                    <dxe:ASPxButton ID="Button1" runat="server" AutoPostBack="false" CssClass="btn btn-primary" Text="Add" ClientInstanceName="cButton1"
                        VerticalAlign="Bottom">
                        <ClientSideEvents Click="saveClientClick" />
                    </dxe:ASPxButton>
                    <dxe:ASPxButton ID="ASPxButton1" runat="server" CssClass="btn btn-danger" Text="Clear" VerticalAlign="Bottom" AutoPostBack="false">
                        <ClientSideEvents Click="ClearClientClick" />
                    </dxe:ASPxButton>



                </td>

            </tr>

        </table>


        <div class="GridViewArea">
            <dxe:ASPxGridView ID="MobileConfigGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="cmobileconfigGrid" SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight="250"
                Width="100%" CssClass="pull-left" OnCustomCallback="MobileConfigGrid_CustomCallback" OnDataBinding="MobileConfigGrid_DataBinding">

                <Settings ShowGroupPanel="false" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu="True" />
                <Columns>
                    <dxe:GridViewDataTextColumn Caption="Active" FieldName="Active" ReadOnly="True" Visible="True" FixedStyle="Left" VisibleIndex="0" Width="60">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Branch" FieldName="Branch" ReadOnly="True" Visible="True" FixedStyle="Left" VisibleIndex="0">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Salesman" FieldName="Salesman" ReadOnly="True" Visible="True" FixedStyle="Left" VisibleIndex="0">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Manager" FieldName="Manager" ReadOnly="True" Visible="True" FixedStyle="Left" VisibleIndex="0">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Top Approver's" FieldName="TopApproval" ReadOnly="True" Visible="True" FixedStyle="Left" VisibleIndex="0">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Financer" FieldName="Financer" ReadOnly="True" Visible="True" FixedStyle="Left" VisibleIndex="0">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>



                    <dxe:GridViewDataTextColumn ReadOnly="True" Width="12%" CellStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" />

                        <CellStyle HorizontalAlign="Center"></CellStyle>
                        <HeaderTemplate>
                            Actions
                        </HeaderTemplate>
                        <DataItemTemplate>

                            <%--   <%if (rights.CanEdit)
                              { %>
                            <a href="javascript:void(0);" onclick="onOpeningEdit('<%#Eval("UniqueId")%>')" title="Edit" class="pad">
                                <img src="/assests/images/Edit.png" /></a>
                               <%} %>--%>


                            <a href="javascript:void(0);" onclick="onMobileConfigEdit('<%#Eval("UniqueId")%>')" title="update Details" class="pad">
                                <img src="../../../assests/images/Edit.png" />
                            </a>
                            <a href="javascript:void(0);" onclick="DelMobileConfig('<%#Eval("UniqueId")%>')" title="delete Details" class="pad">
                                <img src="../../../assests/images/crs.png" />
                            </a>

                        </DataItemTemplate>
                    </dxe:GridViewDataTextColumn>

                </Columns>
                <SettingsPager PageSize="10">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                </SettingsPager>
                <SettingsBehavior ColumnResizeMode="NextColumn" />
                <ClientSideEvents EndCallback="OpeningGridEndCallBack" />
            </dxe:ASPxGridView>
        </div>
        <div class="clear"></div>
        <div style="padding-top: 10px;">
            <table width="100%" style="margin-left: 50px;">
                <tr>
                    <td><span id="ErrorText" style="color: red; display: none" class="lead">Mismatch detected for Debit and Credit Total of the selected Branch.</span></td>
                    <td>&nbsp</td>

                    <td style="width: 250px; display: none"><span class="pull-left" style="margin: 5px 15px 0 0;">Total Dr.</span>
                        <dxe:ASPxTextBox ID="totalDebit" runat="server" ClientEnable="false" ClientInstanceName="ctotalDebit" Width="150">
                            <MaskSettings Mask="<0..99999999999>.<00..99>" IncludeLiterals="DecimalSymbol" ErrorText="None" />
                        </dxe:ASPxTextBox>
                    </td>
                    <td style="width: 250px; display: none"><span class="pull-left" style="margin: 5px 15px 0 0;">Total Cr.</span>
                        <dxe:ASPxTextBox ID="totalCredit" runat="server" ClientEnable="false" ClientInstanceName="totalCredit" Width="150">
                            <MaskSettings Mask="<0..99999999999>.<00..99>" IncludeLiterals="DecimalSymbol" ErrorText="None" />
                        </dxe:ASPxTextBox>
                    </td>
                    <td>&nbsp</td>
                </tr>
            </table>
        </div>


        <div class="clear"></div>
        <div class="col-md-3" style="padding-left: 0">

            <dxe:ASPxButton ID="FinalSave" runat="server" AutoPostBack="false" CssClass="btn btn-primary" Text="<u>S</u>ave" ClientInstanceName="cFinalSave" VerticalAlign="Bottom" EncodeHtml="false">
                <ClientSideEvents Click="FinalSaveClientClick" />
            </dxe:ASPxButton>


            <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" CssClass="btn btn-primary" Text="User IMEI" ClientInstanceName="cButton5"
                VerticalAlign="Bottom">
                <ClientSideEvents Click="TabImei" />
            </dxe:ASPxButton>

        </div>


    </div>
    <asp:SqlDataSource ID="dsBranch" runat="server"  ConflictDetection="CompareAllValues"
        SelectCommand=""></asp:SqlDataSource>
</asp:Content>
