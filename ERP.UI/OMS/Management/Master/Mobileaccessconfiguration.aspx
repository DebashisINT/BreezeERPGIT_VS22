<%--================================================== Revision History =============================================
1.0   Pallab    V2.0.38      24-05-2023          0026216: Tab Configuration module design modification & check in small device
====================================================== Revision History =============================================--%>

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

    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 1;
            margin-bottom: 0;
            -webkit-appearance: auto;
        }

        /*#grid {
            max-width: 98% !important;
        }*/
        #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_partyInvDt
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_partyInvDt_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
        #dt_partyInvDt_B-1 #dt_partyInvDt_B-1Img
        {
            display: none;
        }

        /*select
        {
            -webkit-appearance: auto;
        }*/

        .calendar-icon
        {
                right: 18px;
                bottom: 6px;
        }
        .padTabtype2 > tbody > tr > td
        {
            vertical-align: bottom;
        }
        #rdl_Salesquotation
        {
            margin-top: 0px;
        }

        .lblmTop8>span, .lblmTop8>label
        {
            margin-top: 8px !important;
        }

        .col-md-2, .col-md-4 {
    margin-bottom: 5px;
}

        .simple-select::after
        {
                top: 34px;
            right: 13px;
        }

        .dxeErrorFrameWithoutError_PlasticBlue.dxeControlsCell_PlasticBlue
        {
            padding: 0;
        }

        .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .backSelect {
    background: #42b39e !important;
}

        #ddlInventory
        {
                -webkit-appearance: auto;
        }

        /*.wid-90
        {
            width: 100%;
        }
        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content
        {
            width: 97%;
        }*/
        .newLbl
        {
                margin: 3px 0 !important;
        }

        .lblBot > span, .lblBot > label
        {
                margin-bottom: 3px !important;
        }

        .col-md-2 > label, .col-md-2 > span, .col-md-1 > label, .col-md-1 > span
        {
            margin-top: 8px !important;
            font-size: 14px;
        }

        .col-md-6 span
        {
            font-size: 14px;
        }

        #gridDEstination
        {
            width:99% !important;
        }

        #txtEntity , #txtCustName
        {
            width: 100%;
        }
        .col-md-6 span
        {
            margin-top: 8px !important;
        }

        .rds
        {
            margin-top: 10px !important;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , select
        {
            height: 30px !important;
            
        }
        select
        {
            background-color: transparent;
                padding: 0 20px 0 5px !important;
        }

        .newLbl
        {
            font-size: 14px;
            margin: 3px 0 !important;
            font-weight: 500 !important;
            margin-bottom: 0 !important;
            line-height: 20px;
        }

        .crossBtn {
            top: 25px !important;
            right: 25px !important;
        }

        .wrapHolder
        {
            height: 60px;
        }
        #rdl_SaleInvoice
        {
            margin-top: 12px;
        }

        .dxeRoot_PlasticBlue
        {
            width: 100% !important;
        }

        #ShowFilter {
            padding-bottom: 0px !important;
        }

        @media only screen and (max-width: 1380px) and (min-width: 1300px)
        {
            .col-xs-1, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9, .col-xs-10, .col-xs-11, .col-xs-12, .col-sm-1, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-sm-10, .col-sm-11, .col-sm-12, .col-md-1, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-md-10, .col-md-11, .col-md-12, .col-lg-1, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-lg-10, .col-lg-11, .col-lg-12
            {
                 padding-right: 10px;
                 padding-left: 10px;
            }
            .simple-select::after
        {
                top: 34px;
            right: 8px;
        }
            .calendar-icon
        {
                right: 14px;
                bottom: 6px;
        }
        }

    </style>
    <%--Rev end 1.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
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

            <dxe:ASPxButton ID="FinalSave" runat="server" AutoPostBack="false" CssClass="btn btn-success" Text="<u>S</u>ave" ClientInstanceName="cFinalSave" VerticalAlign="Bottom" EncodeHtml="false">
                <ClientSideEvents Click="FinalSaveClientClick" />
            </dxe:ASPxButton>


            <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" CssClass="btn btn-primary" Text="User IMEI" ClientInstanceName="cButton5"
                VerticalAlign="Bottom">
                <ClientSideEvents Click="TabImei" />
            </dxe:ASPxButton>

        </div>


    </div>
    </div>
    <asp:SqlDataSource ID="dsBranch" runat="server"  ConflictDetection="CompareAllValues"
        SelectCommand=""></asp:SqlDataSource>
</asp:Content>
