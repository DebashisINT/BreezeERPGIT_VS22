<%--==========================================================Revision History ============================================================================================
    1.0   Priti    V2.0.36   23 - 01 - 2023    0025602: Available Stock & UOM Conversion tab is required in Warehouse wise Stock transfer module
    2.0   Pallab   V2.0.38   03 - 05 - 2023    0026006: In Warehouse wise Stock Transfer, multiple coloumns are not showing in the grid when the page resolution is set at 100%
========================================== End Revision History =======================================================================================================--%>


<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="WarehousewiseStockTransferAdd.aspx.cs" Inherits="ERP.OMS.Management.Activities.WarehousewiseStockTransferAdd" %>

<%@ Register Src="~/OMS/Management/Activities/UserControls/UOMConversion.ascx" TagPrefix="uc1" TagName="UOMConversionControl" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<%--check in--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
     <script src="JS/SearchPopupDatatable.js"></script>
    <%--<script src="JS/SearchPopup.js?v=0.03"></script>--%>
     <link href="https://cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/fixedcolumns/3.3.0/js/dataTables.fixedColumns.min.js"></script>
   <script src="JS/WarehousewiseStockTransferJS.js?v=20.4"></script>
     <%--<script src="https://cdn3.devexpress.com/jslib/20.2.3/js/dx.all.js"></script>
    <link href="https://cdn3.devexpress.com/jslib/20.2.3/css/dx.common.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/20.2.3/css/dx.light.css" />--%>
    <style type="text/css">
        .horizontallblHolder {
            height: auto;
            border: 1px solid #12a79b;
            border-radius: 3px;
            overflow: hidden;
        }

        #grid_DXMainTable > tbody > tr > td > a:focus {
            box-shadow: 0px 0px 3px rgb(39 93 175 / 91%);
            border: 1px solid #7c7cdc;
        }

        .horizontallblHolder > table > tbody > tr > td {
            padding: 8px 10px;
            background: #ffffff;
            background: -moz-linear-gradient(top, #ffffff 0%, #f3f3f3 50%, #ededed 51%, #ffffff 100%);
            background: -webkit-linear-gradient(top, #ffffff 0%,#f3f3f3 50%,#ededed 51%,#ffffff 100%);
            background: linear-gradient(to bottom, #ffffff 0%,#f3f3f3 50%,#ededed 51%,#ffffff 100%);
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#ffffff', endColorstr='#ffffff',GradientType=0 );
        }

            .horizontallblHolder > table > tbody > tr > td:first-child {
                background: #12a79b;
                color: #fff;
            }

            .horizontallblHolder > table > tbody > tr > td:last-child {
                font-weight: 500;
                text-transform: uppercase;
                color: #121212;
            }
    </style>
    <style type="text/css">
        #grid_DXMainTable > tbody > tr > td:last-child,
        #grid_DXMainTable > tbody > tr > td:nth-child(18),
        #productLookUp_DDD_gv_DXMainTable > tbody > tr > td:nth-child(2) {
            display: none !important;
        }

        .inline {
            display: inline !important;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content {
            overflow: visible !important;
        }

        #grid_DXStatus span > a {
            display: none;
        }

        .HeaderStyle {
            background-color: #180771d9;
            color: #f5f5f5;
        }

        .lblWarhouse {
            font-size: 10px !important;
            font-weight: 600;
            width: 100%;
            display: block;
            margin-bottom: 5px;
            color: #e29437;
            padding: 6px 2px;
        }

        .dxgvControl_PlasticBlue td.dxgvBatchEditModifiedCell_PlasticBlue {
            background: #fff;
        }

        .iconBranchTo {
            position: absolute;
            right: -17px;
            top: 5px;
        }

        .iconBranch {
            position: absolute;
            right: -17px;
            top: 5px;
        }

        .iconNumberScheme {
            position: absolute;
            right: -17px;
            top: 5px;
        }

        #grid_DXMainTable > tbody > tr > td:nth-child(19) {
            display: none !important;
        }
         .mlableWh{
            padding-top: 22px;
            display:inline-block
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
    <script>


        function SetLostFocusonDemand(e) {
            if ((new Date($("#hdnLockFromDate").val()) <= cdtTDate.GetDate()) && (cdtTDate.GetDate() <= new Date($("#hdnLockToDate").val()))) {
                jAlert("DATA is Freezed between   " + $("#hdnLockFromDateCon").val() + " to " + $("#hdnLockToDateCon").val() + " for Add.");
            }
        }

        function clookup_Project_LostFocus() {
            //grid.batchEditApi.StartEdit(-1, 2);

            var projID = clookup_Project.GetValue();
            if (projID == null || projID == "") {
                $("#ddlHierarchy").val(0);
            }
        }

        function ProjectValueChange(s, e) {

            var projID = clookup_Project.GetValue();

            $.ajax({
                type: "POST",
                url: 'WarehousewiseStockTransferAdd.aspx/getHierarchyID',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ ProjID: projID }),
                success: function (msg) {
                    var data = msg.d;
                    $("#ddlHierarchy").val(data);
                }
            });
        }

        function clookup_ToProject_LostFocus() {
            //grid.batchEditApi.StartEdit(-1, 2);

            var projID = clookup_ToProject.GetValue();
            if (projID == null || projID == "") {
                $("#ddlToHierarchy").val(0);
            }
        }

        function ToProjectValueChange(s, e) {

            var projID = clookup_ToProject.GetValue();

            $.ajax({
                type: "POST",
                url: 'WarehousewiseStockTransferAdd.aspx/getHierarchyID',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ ProjID: projID }),
                success: function (msg) {
                    var data = msg.d;
                    $("#ddlToHierarchy").val(data);
                }
            });
        }


        $(document).ready(function () {
            $('#grid_DXStatus').hide();
            $("#expandgrid").click(function (e) {
                e.preventDefault();

                var $this = $(this);

                if ($this.children('i').hasClass('fa-expand')) {
                    $this.removeClass('hovered half').addClass('full');
                    $this.attr('title', 'Minimize Grid');
                    $this.children('i').removeClass('fa-expand');
                    $this.children('i').addClass('fa-arrows-alt');
                    var gridId = $(this).attr('data-instance');
                    $(this).closest('.makeFullscreen').addClass('panel-fullscreen');
                    var cntWidth = $(this).parent('.makeFullscreen').width();
                    var browserHeight = document.documentElement.clientHeight;
                    var browserWidth = document.documentElement.clientWidth;


                    grid.SetHeight(browserHeight - 150);
                    grid.SetWidth(cntWidth);
                }
                else if ($this.children('i').hasClass('fa-arrows-alt')) {
                    $this.children('i').removeClass('fa-arrows-alt');
                    $this.removeClass('full').addClass('hovered half');
                    $this.attr('title', 'Maximize Grid');
                    $this.children('i').addClass('fa-expand');
                    var gridId = $(this).attr('data-instance');
                    $(this).closest('.makeFullscreen').removeClass('panel-fullscreen');

                    var browserHeight = document.documentElement.clientHeight;
                    var browserWidth = document.documentElement.clientWidth;
                    grid.SetHeight(450);

                    var cntWidth = $this.parent('.makeFullscreen').width();
                    grid.SetWidth(cntWidth);
                }
            });
        })
    </script>
    <script type="text/javascript">
        $(document).ready(function () {

            setTimeout(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    grid.SetWidth(cntWidth);
                }
            }, 200);
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


        //Rev Bapi
        $(document).ready(function () {

            $("#UOMQuantity").on('blur', function () {
                var currentObj = $(this);
                var currentVal = currentObj.val();
                if (!isNaN(currentVal)) {
                    var updatedVal = parseFloat(currentVal).toFixed(4);
                    currentObj.val(updatedVal);
                }
                else {
                    currentObj.val("");
                }
            })


        })
        //End Rev Bapi

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField runat="server" ID="hdnConvertionOverideVisible" />
    <asp:HiddenField runat="server" ID="hdnShowUOMConversionInEntry" />
    <uc1:UOMConversionControl runat="server" ID="UOMConversionControl" />
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
    <div class="panel-title clearfix" id="myDiv">
        <h3 class="pull-left">
            <asp:Label ID="lblHeading" runat="server" Text="Add Warehouse Wise Stock Transfer"></asp:Label>
        </h3>
    </div>
    <div id="ApprovalCross" runat="server" class="crossBtn"><a href="WarehousewiseStockTransferList.aspx"><i class="fa fa-times"></i></a></div>
    <div class="form_main">

        <div class="boxBorder">
            <div class="styledBox mTop5">
                <div class="row">
                    <div class="col-md-2 relative">
                        <label class="darkLabel mTop5">Numbering Scheme<span style="color: red">*</span></label>
                        <div class="relative">
                            <dxe:ASPxComboBox ID="CmbScheme" ClientInstanceName="cCmbScheme"
                                SelectedIndex="0" EnableCallbackMode="false"
                                TextField="SchemaName" ValueField="ID"
                                runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                <ClientSideEvents ValueChanged="CmbScheme_ValueChange"></ClientSideEvents>
                            </dxe:ASPxComboBox>
                            <span id="MandatoryNumberingScheme" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>

                        </div>
                    </div>
                    <div class="col-md-2">
                        <label class="darkLabel mTop5">Document No</label>
                        <div>
                            <dxe:ASPxTextBox runat="server" ID="txtVoucherNo" ClientInstanceName="ctxtVoucherNo" MaxLength="16" Text="Auto" ClientEnabled="false" Width="100%">
                            </dxe:ASPxTextBox>
                            <span id="MandatoryAdjNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <label class="darkLabel mTop5">Date<span style="color: red">*</span></label>
                        <div class="relative">
                            <dxe:ASPxDateEdit ID="dtTDate" runat="server" ClientInstanceName="cdtTDate" EditFormat="Custom" AllowNull="false"
                                Font-Size="12px" UseMaskBehavior="True" Width="100%" EditFormatString="dd-MM-yyyy" CssClass="pull-left">
                                <ButtonStyle Width="13px"></ButtonStyle>
                                <ClientSideEvents GotFocus="function(s,e){cdtTDate.ShowDropDown();}" LostFocus="function(s, e) { SetLostFocusonDemand(e)}"></ClientSideEvents>
                            </dxe:ASPxDateEdit>
                            <span id="MandatoryDate" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>

                        </div>
                    </div>
                    <div class="col-md-2">
                        <label class="darkLabel mTop5">From Unit<span style="color: red">*</span></label>
                        <div class="relative">
                            <asp:DropDownList ID="ddlBranch" runat="server" onchange="ddlBranch_SelectedIndexChanged()"
                                DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%">
                            </asp:DropDownList>
                            <span id="MandatoryBranch" class="iconBranch pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                        </div>
                    </div>
                    <div class="col-md-2">
                        <label class="darkLabel mTop5">To Unit<span style="color: red">*</span></label>
                        <div class="relative">
                            <asp:DropDownList ID="ddlBranchTo" runat="server" onchange="ddlBranchTo_SelectedIndexChanged()"
                                DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%">
                            </asp:DropDownList>
                            <span id="MandatoryBranchTo" class="iconBranchTo pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                        </div>
                    </div>
                    <div style="clear: both;"></div>
                    <div class="col-md-2">
                        <label class="darkLabel mTop5">Transportation Mode</label>
                        <div>
                            <dxe:ASPxTextBox runat="server" ID="txtTransportationMode" ClientInstanceName="ctxtTransportationMode" Width="100%">
                            </dxe:ASPxTextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <label class="darkLabel mTop5">Vehicle No</label>
                        <div>
                            <dxe:ASPxTextBox runat="server" ID="txtVehicleNo" ClientInstanceName="ctxtVehicleNo" Width="100%">
                            </dxe:ASPxTextBox>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <label class="darkLabel mTop5">Remarks</label>
                        <div>
                            <dxe:ASPxTextBox runat="server" ID="txRemarks" ClientInstanceName="ctxRemarks" MaxLength="500" Width="100%">
                            </dxe:ASPxTextBox>
                        </div>
                    </div>
                    <div class="col-md-4" id="DivTechnician" runat="server">
                        <label class="darkLabel mTop5">Technician</label>
                        <div>
                            <dxe:ASPxComboBox ID="ccmTechnician" runat="server" ClientInstanceName="cccmTechnician"
                                Width="100%">
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                    <div style="clear: both;"></div>
                    <div class="col-md-4" id="DivEmployee" runat="server">
                        <label class="darkLabel mTop5">Employee</label>
                        <div>
                            <dxe:ASPxComboBox ID="cmEmployee" runat="server" ClientInstanceName="cccmEmployee"
                                Width="100%">
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                    <div class="col-md-2" id="DivEntity" runat="server">
                        <label class="darkLabel mTop5">Entity
                            <a href="#" onclick="AddEntityClick()" style="left: -12px; top: 20px;"><i id="openlink" runat="server" class="fa fa-plus-circle" aria-hidden="true"></i></a>
                        </label>
                        <div>
                            <dxe:ASPxButtonEdit ID="txtEntity" runat="server" ReadOnly="true" ClientInstanceName="ctxtEntity">
                                <Buttons>
                                    <dxe:EditButton>
                                    </dxe:EditButton>
                                </Buttons>
                                <ClientSideEvents ButtonClick="function(s,e){EntityButnClick();}" KeyDown="function(s,e){EntityKeyDown(s,e);}" />
                            </dxe:ASPxButtonEdit>
                            <span id="MandatoryEntity" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>

                        </div>
                    </div>

                    <div class="col-md-2 lblmTop8">
                        <dxe:ASPxLabel ID="lblProject" runat="server" Text="From Project">
                        </dxe:ASPxLabel>
                        <%-- <label id="lblProject" runat="server">Project</label>--%>
                        <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="EntityServerModeDataStock"
                            KeyFieldName="Proj_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False">
                            <Columns>
                                <dxe:GridViewDataColumn FieldName="Proj_Code" Visible="true" VisibleIndex="1" Caption="Project Code" Settings-AutoFilterCondition="Contains" Width="200px">
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="Proj_Name" Visible="true" VisibleIndex="2" Caption="Project Name" Settings-AutoFilterCondition="Contains" Width="200px">
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="Customer" Visible="true" VisibleIndex="3" Caption="Customer" Settings-AutoFilterCondition="Contains" Width="200px">
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="HIERARCHY_NAME" Visible="true" VisibleIndex="4" Caption="Hierarchy" Settings-AutoFilterCondition="Contains" Width="200px">
                                </dxe:GridViewDataColumn>
                            </Columns>
                            <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                                <Templates>
                                    <StatusBar>
                                        <table class="OptionsTable" style="float: right">
                                            <tr>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </StatusBar>
                                </Templates>
                                <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                            </GridViewProperties>
                            <ClientSideEvents GotFocus="function(s,e){clookup_Project.ShowDropDown();}" LostFocus="clookup_Project_LostFocus" ValueChanged="ProjectValueChange" />

                        </dxe:ASPxGridLookup>
                        <dx:LinqServerModeDataSource ID="EntityServerModeDataStock" runat="server" OnSelecting="EntityServerModeDataStock_Selecting"
                            ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />
                    </div>
                    <div class="col-md-2 lblmTop8">
                        <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="From Hierarchy">
                        </dxe:ASPxLabel>
                        <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%" Enabled="false">
                        </asp:DropDownList>
                    </div>
                    <div style="clear: both;"></div>
                      <div class="col-md-2 lblmTop8">
                        <dxe:ASPxLabel ID="lblToProject" runat="server" Text="To Project">
                        </dxe:ASPxLabel>
                        
                        <dxe:ASPxGridLookup ID="lookup_ToProject" runat="server" ClientInstanceName="clookup_ToProject" DataSourceID="EntityServerModeDataStockToProject"
                            KeyFieldName="Proj_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False">
                            <Columns>
                                <dxe:GridViewDataColumn FieldName="Proj_Code" Visible="true" VisibleIndex="1" Caption="Project Code" Settings-AutoFilterCondition="Contains" Width="200px">
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="Proj_Name" Visible="true" VisibleIndex="2" Caption="Project Name" Settings-AutoFilterCondition="Contains" Width="200px">
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="Customer" Visible="true" VisibleIndex="3" Caption="Customer" Settings-AutoFilterCondition="Contains" Width="200px">
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="HIERARCHY_NAME" Visible="true" VisibleIndex="4" Caption="Hierarchy" Settings-AutoFilterCondition="Contains" Width="200px">
                                </dxe:GridViewDataColumn>
                            </Columns>
                            <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                                <Templates>
                                    <StatusBar>
                                        <table class="OptionsTable" style="float: right">
                                            <tr>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </StatusBar>
                                </Templates>
                                <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                            </GridViewProperties>
                            <ClientSideEvents GotFocus="function(s,e){clookup_ToProject.ShowDropDown();}" LostFocus="clookup_ToProject_LostFocus" ValueChanged="ToProjectValueChange" />

                        </dxe:ASPxGridLookup>
                        <dx:LinqServerModeDataSource ID="EntityServerModeDataStockToProject" runat="server" OnSelecting="EntityServerModeDataStockToProject_Selecting"
                            ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />
                    </div>
                    <div class="col-md-2 lblmTop8">
                        <dxe:ASPxLabel ID="lblToHierarchy" runat="server" Text="To Hierarchy">
                        </dxe:ASPxLabel>
                        <asp:DropDownList ID="ddlToHierarchy" runat="server" Width="100%" Enabled="false">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2 lblmTop8" id="DivBR" runat="server">
                        <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Select Branch Requisition">
                        </dxe:ASPxLabel>
                        <dxe:ASPxButtonEdit ID="taggingList" ClientInstanceName="ctaggingList" runat="server" ReadOnly="true" Width="100%">
                            <Buttons>
                                <dxe:EditButton>
                                </dxe:EditButton>
                            </Buttons>
                            <ClientSideEvents ButtonClick="taggingListButnClick" KeyDown="taggingListKeyDown" />
                        </dxe:ASPxButtonEdit>
                    </div>
                    <div class="col-md-2 lblmTop8" id="DivBRDate" runat="server">
                        <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Branch Requisition Date">
                        </dxe:ASPxLabel>
                        <dxe:ASPxTextBox ID="dt_BRDate" runat="server" Width="100%" ClientEnabled="false" ClientInstanceName="cdt_BRDate">
                        </dxe:ASPxTextBox>
                    </div>
                    <div style="clear: both;"></div>
                    <div class="col-md-2 hide">
                        <label class="mTop5">Source Warehouse<span style="color: red">*</span></label>
                        <div class="relative">
                            <dxe:ASPxComboBox ID="cmbSourceWarehouse" runat="server" ClientInstanceName="ccmbSourceWarehouse"
                                Width="100%">
                                <ClientSideEvents ValueChanged="SourceWH_ValueChange"></ClientSideEvents>
                            </dxe:ASPxComboBox>
                            <span id="MandatoryWarehouse" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            <dxe:ASPxLabel runat="server" ID="lblSourceWHAddress" ClientInstanceName="clblSourceWHAddress" Text="" CssClass="text-muted lblWarhouse"></dxe:ASPxLabel>
                        </div>
                    </div>

                    <div class="col-md-2 hide">
                        <label class="mTop5">Destination Warehouse<span style="color: red">*</span></label>
                        <div class="relative">
                            <dxe:ASPxComboBox ID="cmbDestWarehouse" runat="server" ClientInstanceName="ccmbDestWarehouse"
                                Width="100%">
                                <ClientSideEvents ValueChanged="DestWH_ValueChange"></ClientSideEvents>
                            </dxe:ASPxComboBox>
                            <span id="MandatorycmbDestWarehouse" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            <dxe:ASPxLabel runat="server" ID="lblDestWHAddress" ClientInstanceName="clblDestWHAddress" Text="" CssClass="text-muted lblWarhouse"></dxe:ASPxLabel>
                        </div>
                    </div>
                </div>
            </div>
            <div>
                <dxe:ASPxPopupControl ID="popup_taggingGrid" runat="server" ClientInstanceName="cpopup_taggingGrid"
                    HeaderText="Select Branch Requisition" PopupHorizontalAlign="WindowCenter"
                    BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" Height="400px" Width="850px"
                    Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                    ContentStyle-CssClass="pad">
                    <ContentStyle VerticalAlign="Top" CssClass="pad">
                    </ContentStyle>
                    <ContentCollection>
                        <dxe:PopupControlContentControl runat="server">
                            <div style="padding: 7px 0;">
                                <input type="button" value="Select All" onclick="Tag_ChangeState('SelectAll')" class="btn btn-primary"></input>
                                <input type="button" value="De-select All" onclick="Tag_ChangeState('UnSelectAll')" class="btn btn-primary"></input>
                                <input type="button" value="Revert" onclick="Tag_ChangeState('Revart')" class="btn btn-primary"></input>
                            </div>
                            <div>
                                <dxe:ASPxGridView ID="taggingGrid" ClientInstanceName="ctaggingGrid" runat="server" KeyFieldName="Indent_Id"
                                    Width="100%" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords"
                                    Settings-ShowFooter="false" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible"
                                    OnCustomCallback="taggingGrid_CustomCallback" OnDataBinding="taggingGrid_DataBinding">
                                    <SettingsBehavior AllowDragDrop="False"></SettingsBehavior>
                                    <SettingsPager Visible="false"></SettingsPager>
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="40" Caption=" " VisibleIndex="0" />
                                        <dxe:GridViewDataTextColumn FieldName="Indent_RequisitionNumber" Caption="Document Number" Width="150" VisibleIndex="1">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn FieldName="Indent_Date" Caption="Posting Date" Width="100" VisibleIndex="2">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn FieldName="Branch" Caption="To Branch" Width="150" VisibleIndex="3">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn FieldName="FromBranch" Caption="From Branch" Width="150" VisibleIndex="4">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn FieldName="Branch_Id" Caption="Branch_Id" Width="0" VisibleIndex="5">
                                        </dxe:GridViewDataTextColumn>

                                    </Columns>
                                    <SettingsDataSecurity AllowEdit="true" />
                                    <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                </dxe:ASPxGridView>
                            </div>
                            <div class="text-center">
                                <dxe:ASPxButton ID="btnTaggingSave" ClientInstanceName="cbtnTaggingSave" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                    <ClientSideEvents Click="function(s, e) {BRNumberChanged();}" />
                                </dxe:ASPxButton>
                            </div>
                        </dxe:PopupControlContentControl>
                    </ContentCollection>
                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                </dxe:ASPxPopupControl>
            </div>
            <div>
                <dxe:ASPxPopupControl ID="ASPxProductsPopup" runat="server" ClientInstanceName="cProductsPopup"
                    Width="900px" HeaderText="Branch Requisition" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                    Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
                    <HeaderTemplate>
                        <strong><span style="color: #fff">Select Products</span></strong>
                        <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                            <ClientSideEvents Click="function(s, e){ 
                                                                                        cProductsPopup.Hide();
                                                                                    }" />
                        </dxe:ASPxImage>
                    </HeaderTemplate>
                    <ContentCollection>
                        <dxe:PopupControlContentControl runat="server">
                            <div style="padding: 7px 0;">
                                <input type="button" value="Select All Products" onclick="ChangeState('SelectAll')" class="btn btn-primary"></input>
                                <input type="button" value="De-select All Products" onclick="ChangeState('UnSelectAll')" class="btn btn-primary"></input>
                                <input type="button" value="Revert" onclick="ChangeState('Revart')" class="btn btn-primary"></input>
                            </div>
                            <div>
                                <dxe:ASPxGridView runat="server" KeyFieldName="ComponentDetailsID" ClientInstanceName="cgridproducts" ID="grid_Products"
                                    Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridProducts_CustomCallback"
                                    Settings-ShowFooter="false" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
                                    <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                                    <SettingsPager Visible="false"></SettingsPager>
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="40" Caption=" " VisibleIndex="0" />
                                        <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="SrlNo" Width="40" ReadOnly="true" Caption="Sl. No.">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="ComponentNumber" Width="120" ReadOnly="true" Caption="Number">
                                            <Settings AllowAutoFilter="True" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="ProductID" ReadOnly="true" Caption="Product" Width="0">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="ProductsName" ReadOnly="true" Width="100" Caption="Product Name">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="ProductDescription" Width="200" ReadOnly="true" Caption="Product Description">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Bal Quantity" FieldName="Quantity" Width="70" VisibleIndex="6">
                                            <PropertiesTextEdit>
                                                <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="ComponentID" ReadOnly="true" Caption="ComponentID" Width="0">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="ComponentDetailsID" ReadOnly="true" Caption="ComponentDetailsID" Width="0">
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsDataSecurity AllowEdit="true" />
                                    <ClientSideEvents EndCallback="gridProducts_EndCallback" />
                                </dxe:ASPxGridView>
                            </div>
                            <div class="text-center">
                                <dxe:ASPxButton ID="btn_gridproducts" ClientInstanceName="cbtn_gridproducts" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                    <ClientSideEvents Click="function(s, e) {PerformCallToGridBind();}" />
                                </dxe:ASPxButton>
                            </div>
                        </dxe:PopupControlContentControl>
                    </ContentCollection>
                    <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                </dxe:ASPxPopupControl>
            </div>
            <div style="clear: both;"></div>
            <div class="row mTop5">
                <div class="col-md-12 mTop5">

                    <dxe:ASPxGridView runat="server" OnBatchUpdate="grid_BatchUpdate" KeyFieldName="ActualSL" ClientInstanceName="grid" ID="grid"
                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                        OnCellEditorInitialize="grid_CellEditorInitialize" OnCustomJSProperties="grid_CustomJSProperties"
                        Settings-ShowFooter="false" OnCustomCallback="grid_CustomCallback" OnDataBinding="grid_DataBinding"
                        OnRowInserting="grid_RowInserting" OnRowUpdating="grid_RowUpdating" OnRowDeleting="grid_RowDeleting"
                        OnHtmlRowPrepared="grid_HtmlRowPrepared" Settings-VerticalScrollableHeight="350" Settings-VerticalScrollBarMode="Visible"
                        SettingsPager-Mode="ShowAllRecords" Settings-HorizontalScrollBarMode="Visible">
                        <SettingsPager Visible="false"></SettingsPager>
                        <Columns>
                            <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="30px" VisibleIndex="0"
                                Caption=" ">
                                <CustomButtons>
                                    <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                    </dxe:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                            </dxe:GridViewCommandColumn>
                            <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl#" ReadOnly="true" VisibleIndex="1" Width="30px">
                                <PropertiesTextEdit>
                                </PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <%--Batch Product Popup Start--%>
                            <%--Rev 2.0--%>
                            <%--<dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product" VisibleIndex="2" Width="20%" ReadOnly="True">--%>
                            <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product" VisibleIndex="2" Width="130px" ReadOnly="True">
                            <%--Rev end 2.0--%>
                                <PropertiesButtonEdit>
                                    <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" />
                                    <Buttons>
                                        <dxe:EditButton Text="..." Width="20px">
                                        </dxe:EditButton>
                                    </Buttons>
                                </PropertiesButtonEdit>
                            </dxe:GridViewDataButtonEditColumn>
                            <dxe:GridViewDataTextColumn FieldName="ProductID" Caption="hidden Field Id" CellStyle-CssClass="hideContent"
                                PropertiesTextEdit-HelpTextStyle-CssClass="hideContent" EditFormCaptionStyle-CssClass="hideContent" VisibleIndex="23" CellStyle-Wrap="False"
                                ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide">
                                <PropertiesTextEdit>
                                    <FocusedStyle CssClass="hide">
                                    </FocusedStyle>
                                    <Style CssClass="hide">
                                                            </Style>
                                </PropertiesTextEdit>
                                <CellStyle Wrap="True" CssClass="hide myCellStyle"></CellStyle>
                                <EditCellStyle CssClass="hide">
                                </EditCellStyle>
                            </dxe:GridViewDataTextColumn>
                            <%--Batch Product Popup End--%>
                            <%--Rev 2.0--%>
                            <%--<dxe:GridViewDataTextColumn FieldName="Discription" Caption="Description" VisibleIndex="3" Width="15%" ReadOnly="true">--%>
                            <dxe:GridViewDataTextColumn FieldName="Discription" Caption="Description" VisibleIndex="3" Width="100px" ReadOnly="true">
                            <%--Rev end 2.0--%>
                                <CellStyle Wrap="True"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <%--Rev 2.0--%>
                            <%--<dxe:GridViewDataButtonEditColumn FieldName="SourceWarehouse" Caption="Source Warehouse" VisibleIndex="4" Width="15%" ReadOnly="True">--%>
                            <dxe:GridViewDataButtonEditColumn FieldName="SourceWarehouse" Caption="Source Warehouse" VisibleIndex="4" Width="130px" ReadOnly="True">
                            <%--Rev end 2.0--%>
                                <PropertiesButtonEdit>
                                    <ClientSideEvents ButtonClick="SourceWarehouseButnClick" KeyDown="SourceWarehouseKeyDown" />
                                    <Buttons>
                                        <dxe:EditButton Text="..." Width="20px">
                                        </dxe:EditButton>
                                    </Buttons>
                                </PropertiesButtonEdit>
                            </dxe:GridViewDataButtonEditColumn>
                            <dxe:GridViewDataTextColumn FieldName="SourceWarehouseID" Caption="hidden Field Id" VisibleIndex="22" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="AvlStkSourceWH" Caption="Avl. Stock" VisibleIndex="5" Width="80px" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                                <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.0000">
                                    <%-- <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />--%>
                                    <%--<MaskSettings Mask="<-999999999..999999999g>.<0..99>" AllowMouseWheel="false" />--%>
                                    <MaskSettings Mask="<-999999999..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                </PropertiesTextEdit>
                                <CellStyle HorizontalAlign="Right"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataButtonEditColumn FieldName="DestinationWarehouse" Caption="Destination Warehouse" VisibleIndex="6" Width="150px" ReadOnly="True">
                                <PropertiesButtonEdit>
                                    <ClientSideEvents ButtonClick="DestinationWarehouseButnClick" KeyDown="DestinationWarehouseKeyDown" />
                                    <Buttons>
                                        <dxe:EditButton Text="..." Width="20px">
                                        </dxe:EditButton>
                                    </Buttons>
                                </PropertiesButtonEdit>
                            </dxe:GridViewDataButtonEditColumn>
                            <dxe:GridViewDataTextColumn FieldName="DestinationWarehouseID" Caption="hidden Field Id" VisibleIndex="21" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="AvlStkDestWH" Caption="Avl. Stock" VisibleIndex="7" Width="80px" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                                <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.0000">
                                    <%--  <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />    --%>
                                </PropertiesTextEdit>
                                <CellStyle HorizontalAlign="Right"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="TransferQuantity" Caption="Transfer Qty" VisibleIndex="8" Width="92px" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                                <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.0000">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                    <ClientSideEvents LostFocus="QuantityTextChange" />
                                    <%--<ClientSideEvents  GotFocus="QuantityTextChangeGotFocus" />--%>
                                </PropertiesTextEdit>
                                <CellStyle HorizontalAlign="Right"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="SaleUOM" Caption="UOM" VisibleIndex="9" Width="100px" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                                <PropertiesTextEdit>
                                </PropertiesTextEdit>
                                <CellStyle HorizontalAlign="Right"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                             <%--  Manis 24428--%> 

                             <dxe:GridViewCommandColumn VisibleIndex="10" Caption="Multi UOM" Width="100px">
                                    <CustomButtons>
                                           <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomMultiUOM" Image-Url="/assests/images/MultiUomIcon.png" Image-ToolTip="Multi UOM">
                                            </dxe:GridViewCommandColumnCustomButton>
                                             </CustomButtons>
                                       </dxe:GridViewCommandColumn>

                                    <dxe:GridViewDataTextColumn Caption="Multi Qty" CellStyle-HorizontalAlign="Right" FieldName="Order_AltQuantity" PropertiesTextEdit-MaxLength="14" VisibleIndex="11" Width="100px" ReadOnly="true">
                                                        <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right">
                                                          <%--  <ClientSideEvents GotFocus="QuantityGotFocus" LostFocus="QuantityTextChange" />--%>
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" />
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                     
                                                    <dxe:GridViewDataTextColumn Caption="Multi Unit" FieldName="Order_AltUOM" ReadOnly="true" VisibleIndex="12" Width="100px" >
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>



                               <%--  Manis End 24428--%> 
                            <dxe:GridViewCommandColumn Width="90px" VisibleIndex="13" Caption="Stk Details">
                                <CustomButtons>
                                    <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomWarehouse" Image-Url="/assests/images/warehouse.png">
                                    </dxe:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                            </dxe:GridViewCommandColumn>
                            <dxe:GridViewDataTextColumn FieldName="Rate" Caption="Rate" VisibleIndex="14" Width="60px" HeaderStyle-HorizontalAlign="Right">
                                <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                    <ClientSideEvents LostFocus="RateTextChange" />
                                </PropertiesTextEdit>
                                <CellStyle HorizontalAlign="Right"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="Value" Caption="Value" VisibleIndex="15" Width="60px" HeaderStyle-HorizontalAlign="Right">
                                <PropertiesTextEdit Style-HorizontalAlign="Right">
                                    <ClientSideEvents LostFocus="AmountTextChange" />
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                </PropertiesTextEdit>
                                <CellStyle HorizontalAlign="Right"></CellStyle>
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn FieldName="Remarks" Caption="Remarks" VisibleIndex="16" Width="200px">
                                <PropertiesTextEdit>
                                    <ClientSideEvents KeyDown="AddBatchNew"></ClientSideEvents>
                                </PropertiesTextEdit>
                                <CellStyle></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="ActualSL" Width="0" VisibleIndex="17">
                            </dxe:GridViewDataTextColumn>

                      
                            <dxe:GridViewDataTextColumn FieldName="UpdateEdit" Caption="hidden Field Id" VisibleIndex="18" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="BRID" Caption="hidden Field Id" VisibleIndex="19" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="BRDetailsID" Caption="hidden Field Id" VisibleIndex="20" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                        </Columns>

                        <ClientSideEvents RowClick="GetVisibleIndex" BatchEditStartEditing="GetVisibleIndex" CustomButtonClick="gridCustomButtonClick" EndCallback="GridEndCallBack" />
                        <SettingsDataSecurity AllowEdit="true" />
                        <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                            <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                        </SettingsEditing>
                    </dxe:ASPxGridView>

                      <%--  Manis 24428--%> 
                      <dxe:ASPxPopupControl ID="Popup_MultiUOM" runat="server" ClientInstanceName="cPopup_MultiUOM"
        Width="900px" HeaderText="Multi UOM Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ClientSideEvents Closing="function(s, e) {
	closeMultiUOM(s, e);}" />
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="Top clearfix">



                    <div class="clearfix col-md-12" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;">

                        <table class="eqTble">
                            <tr>
                                <td>
                                    <div>
                                        <div style="margin-bottom: 5px;">
                                            <div>
                                                <label>Base Quantity</label>
                                            </div>
                                            <div>
                                                <%--rev Sanchita--%>
                                                <%--<input type="text" id="UOMQuantity" style="text-align: right;" maxlength="18" class="allownumericwithdecimal" />--%>
                                                <input type="text" id="UOMQuantity" style="text-align: right;" maxlength="18" class="allownumericwithdecimal" onchange="CalcBaseRate()" />
                                                <%--End of Rev SAnchita--%>
                                            </div>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="Left_Content" style="">
                                        <div>
                                            <label style="text-align: right;">Base UOM</label>
                                        </div>
                                        <div>
                                            <dxe:ASPxComboBox ID="cmbUOM" ClientInstanceName="ccmbUOM" runat="server" SelectedIndex="0" DataSourceID="UomSelect"
                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" ValueField="UOM_ID" TextField="UOM_Name">
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>
                                </td>
                                  <%--Mantis Issue 24428--%>
                                 <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label>Base Rate </label>
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="cmbBaseRate" runat="server" Width="80px" ClientInstanceName="ccmbBaseRate" DisplayFormatString="0.000" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right" ReadOnly="true" ></dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                <%--End of Mantis Issue 24428--%>
                                <td>
                                    <span style="font-size: 22px; padding-top: 15px; display: inline-block;">=</span>
                                </td>
                                <td>
                                    <div>
                                        <div>
                                            <label style="text-align: right;">Alt. UOM</label>
                                        </div>
                                        <div class="Left_Content" style="">
                                            <dxe:ASPxComboBox ID="cmbSecondUOM" ClientInstanceName="ccmbSecondUOM" runat="server" SelectedIndex="0" DataSourceID="AltUomSelect"
                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" ValueField="UOM_ID" TextField="UOM_Name">
                                                <ClientSideEvents TextChanged="function(s,e) { PopulateMultiUomAltQuantity();}" />
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label>Alt. Quantity </label>
                                        </div>
                                        <div>
                                            <%-- <input type="text" id="AltUOMQuantity" style="text-align:right;"  maxlength="18" class="allownumericwithdecimal"/>--%>
                                            <dxe:ASPxTextBox ID="AltUOMQuantity" runat="server" ClientInstanceName="cAltUOMQuantity" DisplayFormatString="0.0000" MaskSettings-Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right">
                                                <%--Mantis Issue 24428--%>
                                                <ClientSideEvents TextChanged="function(s,e) { CalcBaseQty();}" />
                                                <%--End of Mantis Issue 24428--%>
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                  <%--Mantis Issue 24428--%>
                                 <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label>Alt Rate </label>
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="cmbAltRate" Width="80px" runat="server" ClientInstanceName="ccmbAltRate" DisplayFormatString="0.000" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right"  >
                                                <ClientSideEvents TextChanged="function(s,e) { CalcBaseRate();}" />
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            
                                        </div>
                                        <div>
                                            <label class="checkbox-inline mlableWh">
                                                <input type="checkbox" id="chkUpdateRow"  />
                                                <span style="margin: 0px 0; display: block">
                                                    <dxe:ASPxLabel ID="ASPxLabel18" runat="server" Text="Update Row">
                                                    </dxe:ASPxLabel>
                                                </span>
                                            </label>
                                        </div>
                                    </div>

                                    
                                </td>
                                <%--End of Mantis Issue 24428--%>
                                <td style="padding-top: 14px;">
                                    <dxe:ASPxButton ID="btnMUltiUOM" ClientInstanceName="cbtnMUltiUOM" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                        <ClientSideEvents Click="function(s, e) { if(!document.getElementById('myCheck').checked)  {SaveMultiUOM();}}" />
                                    </dxe:ASPxButton>
                                </td>
                            </tr>
                        </table>

                    </div>
                    <div class="clearfix">
                        <dxe:ASPxGridView ID="grid_MultiUOM" runat="server" KeyFieldName="AltUomId;AltQuantity" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="cgrid_MultiUOM" OnCustomCallback="MultiUOM_CustomCallback" OnDataBinding="MultiUOM_DataBinding"
                            SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200" SettingsBehavior-AllowSort="false">
                            <Columns>
                                 <%--Mantis Issue 24428--%>
                                  <dxe:GridViewDataTextColumn Caption="MultiUOMSR No" 
                                    VisibleIndex="0" HeaderStyle-HorizontalAlign="left" Width="0px">
                                </dxe:GridViewDataTextColumn>
                                     <%--End of Mantis Issue 24428--%>
                                <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity"
                                    VisibleIndex="0" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="UOM" FieldName="UOM"
                                    VisibleIndex="1">
                                </dxe:GridViewDataTextColumn>


                                     <%--Mantis Issue 24428 --%>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="BaseRate"
                                    VisibleIndex="2" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24428 --%>



                                <dxe:GridViewDataTextColumn Caption="Alt. UOM" FieldName="AltUOM"
                                    VisibleIndex="2">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Alt. Quantity" FieldName="AltQuantity"
                                    VisibleIndex="3" HeaderStyle-HorizontalAlign="Right">
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn Caption="" FieldName="UomId" Width="0px"
                                    VisibleIndex="3">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="" FieldName="AltUomId" Width="0px"
                                    VisibleIndex="5">
                                </dxe:GridViewDataTextColumn>

                                   <%--Mantis Issue 24428--%>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="AltRate"
                                    VisibleIndex="7" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Update Row" FieldName="UpdateRow"
                                    VisibleIndex="7" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24428 --%>

                                <dxe:GridViewDataTextColumn VisibleIndex="10" Width="80px" Caption="Action">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" onclick="Delete_MultiUom('<%# Container.KeyValue %>','<%#Eval("SrlNo") %>')" title="Delete">
                                            <img src="/assests/images/crs.png" /></a>
                                          <%--Mantis Issue 24428 --%>

                                           <a href="javascript:void(0);" onclick="Edit_MultiUom('<%# Container.KeyValue %>','<%#Eval("MultiUOMSR") %>')" title="Edit">
                                            <img src="/assests/images/Edit.png" /></a>
                                          <%--End of Mantis Issue 24428 --%>
                                    </DataItemTemplate>
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <ClientSideEvents EndCallback="OnMultiUOMEndCallback" />
                            <SettingsPager Visible="false"></SettingsPager>
                            <SettingsLoadingPanel Text="Please Wait..." />
                        </dxe:ASPxGridView>
                    </div>
                    <div class="clearfix">
                        <br />
                        <div style="align-content: center">
                            <dxe:ASPxButton ID="ASPxButton7" ClientInstanceName="cbtnfinalUomSave" Width="50px" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                <ClientSideEvents Click="function(s, e) {FinalMultiUOM();}" />
                            </dxe:ASPxButton>
                        </div>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>


       <asp:SqlDataSource ID="UomSelect" runat="server"
        SelectCommand="select UOM_ID,UOM_Name from Master_UOM "></asp:SqlDataSource>

    <asp:SqlDataSource ID="AltUomSelect" runat="server"
        SelectCommand="select UOM_ID,UOM_Name from Master_UOM "></asp:SqlDataSource>

                         <%--End of Mantis Issue 24428--%>


                    <div style="clear: both;"></div>
                    <div class="clear"></div>
                    <div class="content reverse horizontal-images clearfix" style="width: 100%; margin-right: 0; padding: 8px; height: auto; border-top: 1px solid #ccc; border-bottom: 1px solid #ccc; border-radius: 0;">
                        <ul>
                            <li class="clsbnrLblTotalQty">
                                <div class="horizontallblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Total Quantity" ClientInstanceName="cbnrLblTotalQty" />
                                                </td>
                                                <td>
                                                    <dxe:ASPxLabel ID="bnrLblTotalQty" runat="server" Text="0.00" ClientInstanceName="cbnrLblTotalQty" />
                                                </td>
                                            </tr>

                                        </tbody>
                                    </table>
                                </div>
                            </li>
                            <li class="clsbnrLblTaxAmt " id="liAltQty" runat="server">
                                <div class="horizontallblHolder" id="">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxLabel ID="bnrLblTaxAmt" runat="server" Text="Total Alt. Quantity" ClientInstanceName="cbnrLblTaxAmt" />
                                                </td>
                                                <td>
                                                    <dxe:ASPxLabel ID="bnrLblAltQty" runat="server" Text="0.00" ClientInstanceName="cbnrLblAltQty" />
                                                </td>
                                            </tr>

                                        </tbody>
                                    </table>
                                </div>
                            </li>
                            <li class="clsbnrLblTaxableAmt">
                                <div class="horizontallblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxLabel ID="bnrLblTaxableAmt" runat="server" Text="Total Amount" ClientInstanceName="cbnrLblTaxableAmt" />
                                                </td>
                                                <td>
                                                    <dxe:ASPxLabel ID="bnrLblAmtval" runat="server" Text="0.00" ClientInstanceName="cbnrLblAmtval" />
                                                </td>
                                            </tr>

                                        </tbody>
                                    </table>
                                </div>
                            </li>


                        </ul>

                    </div>
                    <div style="clear: both;"></div>
                    <div class="clear"></div>
                    <div class="row">
                        <div class="col-md-12 mTop5">
                            <table style="float: left;" id="tblBtnSavePanel">
                                <tr>
                                    <td style="padding: 5px 0px;">
                                        <span id="tdSaveButton" runat="server">
                                            <%--<% if (rights.CanAdd)
                                       { %>--%>
                                            <dxe:ASPxButton ID="btnSaveRecords" ClientInstanceName="cbtnSaveRecords" runat="server" AutoPostBack="False" Text="S&#818;ave & New" ClientVisible="false"
                                                CssClass="btn btn-primary" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {SaveButtonClick();}" />
                                            </dxe:ASPxButton>
                                            <%--  <%} %>--%>
                                        </span>
                                        <span id="Span1" runat="server">
                                            <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" ClientVisible="false"
                                                CssClass="btn btn-primary" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {SaveExitButtonClick();}" />
                                            </dxe:ASPxButton>
                                        </span>
                                    </td>

                                </tr>
                            </table>
                        </div>
                    </div>
                </div>







            </div>
            <div class="clear"></div>
            <asp:HiddenField ID="hdAddEdit" runat="server" />
            <asp:HiddenField ID="hdAdjustmentId" runat="server" />
            <asp:HiddenField ID="HiddenSaveButton" runat="server" />
            <asp:HiddenField ID="HiddenRowCount" runat="server" />

            <asp:HiddenField ID="hdfProductID" runat="server" />
            <asp:HiddenField ID="hdfProductType" runat="server" />
            <asp:HiddenField ID="hdfProductSerialID" runat="server" />
            <asp:HiddenField ID="hdnProductQuantity" runat="server" />
            <asp:HiddenField ID="hdnProductTypeAssign" runat="server" />
            <asp:HiddenField ID="hdnStockTransfer" runat="server" />
             <asp:HiddenField ID="hddnMultiUOMSelection" runat="server" />

            <%-- Rev  Mantis Issue 24428--%>
            <asp:HiddenField runat="server" ID="hdnWSTAutoPrint" Value="" />
               <asp:HiddenField ID="hdProductID" runat="server" />
           <%--  End of Rev  Mantis Issue 24428--%>
            <!--Product Modal -->
            <div class="modal fade" id="ProductModel" role="dialog">
                <div class="modal-dialog">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Product Search</h4>
                        </div>
                        <div class="modal-body">
                            <input type="text" onkeydown="prodkeydown(event)" id="txtProdSearch" autofocus width="100%" placeholder="Search By Product Code or Name" />

                            <div id="ProductTable">
                                <table border='1' width="100%" class="dynamicPopupTbl">
                                    <tr class="HeaderStyle">
                                        <th class="hide">id</th>
                                        <th>Product Code</th>
                                        <th>Product Name</th>
                                        <th>Inventory</th>
                                        <th>HSN/SAC</th>
                                        <th>Class</th>
                                        <th>Brand</th>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <% if (rightsProd.CanAdd)
                               { %>
                            <button type="button" class="btn btn-success btn-radius" onclick="fn_PopOpen();">
                                <span class="btn-icon"><i class="fa fa-plus"></i></span>
                                Add New
                            </button>
                            <% } %>
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
            <!--Product Modal -->

            <dxe:ASPxPopupControl ID="Popup_Warehouse" runat="server" ClientInstanceName="cPopup_Warehouse"
                Width="900px" HeaderText="Warehouse Details" PopupHorizontalAlign="WindowCenter"
                BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                ContentStyle-CssClass="pad">
                <ClientSideEvents Closing="function(s, e) {
	closeWarehouse(s, e);}" />
                <ContentStyle VerticalAlign="Top" CssClass="pad">
                </ContentStyle>
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <div class="Top clearfix">
                            <div id="content-5" class="pull-right wrapHolder content horizontal-images" style="width: 100%; margin-right: 0px;">
                                <ul>
                                    <li>
                                        <div class="lblHolder">
                                            <table>
                                                <tbody>
                                                    <tr>
                                                        <td>Selected Product</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblProductName" runat="server"></asp:Label></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </li>
                                    <li>
                                        <div class="lblHolder">
                                            <table>
                                                <tbody>
                                                    <tr>
                                                        <td>Entered Quantity </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="txt_SalesAmount" runat="server"></asp:Label>
                                                            <asp:Label ID="txt_SalesUOM" runat="server"></asp:Label>

                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </li>
                                   <%--START REV 1.0--%>
                                    <li>
                                        <div class="lblHolder" id="divpopupAvailableStock" style="display: none;">
                                            <table>
                                                <tbody>
                                                    <tr>
                                                        <td>Available Stock</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblAvailableStock" runat="server"></asp:Label>
                                                            <asp:Label ID="lblAvailableStockUOM" runat="server"></asp:Label>

                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </li>
                                    <li>
                                        <div class="lblHolder" id="divuom">
                                            <table>
                                                <tbody>
                                                    <tr>
                                                        <td><span><b>UOM Conversion: </b></span></td>
                                                    </tr>
                                                    <tr>
                                                        <td>                                                           
                                                            <asp:Label ID="lbluomfactor1" runat="server"></asp:Label>                                                           
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </li>
                                  <%--  END REV 1.0--%>
                                    <li style="display: none;">
                                        <div class="lblHolder">
                                            <table>
                                                <tbody>
                                                    <tr>
                                                        <td>Stock Quantity </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="txt_StockAmount" runat="server"></asp:Label>
                                                            <asp:Label ID="txt_StockUOM" runat="server"></asp:Label></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </li>

                                </ul>
                            </div>

                            <div class="clear">
                                <br />
                            </div>
                            <div class="clearfix col-md-12" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;">
                                <div>
                                    <div class="col-md-3" id="div_Warehouse">
                                        <div style="margin-bottom: 5px;">
                                            Warehouse
                                        </div>
                                        <div class="Left_Content" style="">
                                            <dxe:ASPxComboBox ID="CmbWarehouse" EnableIncrementalFiltering="True" ClientInstanceName="cCmbWarehouse" SelectedIndex="0"
                                                TextField="WarehouseName" ValueField="WarehouseID" runat="server" Width="100%" OnCallback="CmbWarehouse_Callback">
                                                <ClientSideEvents ValueChanged="function(s,e){CmbWarehouse_ValueChange()}" EndCallback="CmbWarehouseEndCallback"></ClientSideEvents>
                                            </dxe:ASPxComboBox>
                                            <span id="spnCmbWarehouse" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3" id="div_Batch">
                                        <div style="margin-bottom: 5px;">
                                            Batch
                                        </div>
                                        <div class="Left_Content" style="">
                                            <dxe:ASPxComboBox ID="CmbBatch" EnableIncrementalFiltering="True" ClientInstanceName="cCmbBatch"
                                                TextField="BatchName" ValueField="BatchID" runat="server" Width="100%" OnCallback="CmbBatch_Callback">
                                                <ClientSideEvents ValueChanged="function(s,e){CmbBatch_ValueChange()}" EndCallback="CmbBatchEndCall"></ClientSideEvents>
                                            </dxe:ASPxComboBox>
                                            <span id="spnCmbBatch" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                        </div>
                                    </div>
                                    <div class="col-md-4" id="div_Serial">
                                        <div style="margin-bottom: 5px;">
                                            Serial No &nbsp;&nbsp; (
                                                <input type="checkbox" id="myCheck" name="BarCode" onchange="AutoCalculateMandateOnChange(this)">Barcode )
                                        </div>
                                        <div class="" id="divMultipleCombo">

                                            <dxe:ASPxDropDownEdit ClientInstanceName="checkComboBox" ID="ASPxDropDownEdit1" Width="85%" CssClass="pull-left" runat="server" AnimationType="None">
                                                <DropDownWindowStyle BackColor="#EDEDED" />
                                                <DropDownWindowTemplate>
                                                    <dxe:ASPxListBox Width="100%" ID="listBox" ClientInstanceName="checkListBox" SelectionMode="CheckColumn" OnCallback="CmbSerial_Callback"
                                                        runat="server">
                                                        <Border BorderStyle="None" />
                                                        <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />

                                                        <ClientSideEvents SelectedIndexChanged="OnListBoxSelectionChanged" EndCallback="listBoxEndCall" />
                                                    </dxe:ASPxListBox>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="padding: 4px">
                                                                <dxe:ASPxButton ID="ASPxButton4" AutoPostBack="False" runat="server" Text="Close" Style="float: right">
                                                                    <ClientSideEvents Click="function(s, e){ checkComboBox.HideDropDown(); }" />
                                                                </dxe:ASPxButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </DropDownWindowTemplate>
                                                <ClientSideEvents TextChanged="SynchronizeListBoxValues" DropDown="SynchronizeListBoxValues" GotFocus="function(s, e){ s.ShowDropDown(); }" />
                                            </dxe:ASPxDropDownEdit>
                                            <span id="spncheckComboBox" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            <div class="pull-left">
                                                <i class="fa fa-commenting" id="abpl" aria-hidden="true" style="font-size: 16px; cursor: pointer; margin: 3px 0 0 5px;" title="Serial No " data-container="body" data-toggle="popover" data-placement="right" data-content=""></i>
                                            </div>
                                        </div>
                                        <div class="" id="divSingleCombo" style="display: none;">
                                            <dxe:ASPxTextBox ID="txtserial" runat="server" Width="85%" ClientInstanceName="ctxtserial" HorizontalAlign="Left" Font-Size="12px" MaxLength="49">
                                                <ClientSideEvents TextChanged="txtserialTextChanged" />
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3" id="div_Quantity">
                                        <div style="margin-bottom: 2px;">
                                            Quantity
                                        </div>
                                        <div class="Left_Content" style="">
                                            <dxe:ASPxTextBox ID="txtQuantity" runat="server" ClientInstanceName="ctxtQuantity" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" />
                                                <ClientSideEvents TextChanged="function(s, e) {SaveWarehouse();}" />
                                            </dxe:ASPxTextBox>
                                            <span id="spntxtQuantity" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div>
                                        </div>
                                        <div class="Left_Content" style="padding-top: 14px">
                                            <dxe:ASPxButton ID="btnWarehouse" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" UseSubmitBehavior="True" Text="Add" CssClass="btn btn-primary">
                                                <ClientSideEvents Click="function(s, e) {if(!document.getElementById('myCheck').checked) SaveWarehouse();}" />
                                            </dxe:ASPxButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="clearfix">
                                <dxe:ASPxGridView ID="GrdWarehouse" runat="server" KeyFieldName="SrlNo" AutoGenerateColumns="False"
                                    Width="100%" ClientInstanceName="cGrdWarehouse" OnCustomCallback="GrdWarehouse_CustomCallback" OnDataBinding="GrdWarehouse_DataBinding"
                                    SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200" SettingsBehavior-AllowSort="false">
                                    <Columns>
                                        <dxe:GridViewDataTextColumn Caption="Warehouse Name" FieldName="WarehouseName"
                                            VisibleIndex="0">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Available Quantity" FieldName="AvailableQty" Visible="false"
                                            VisibleIndex="1">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="SalesQuantity"
                                            VisibleIndex="2">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Conversion Foctor" FieldName="ConversionMultiplier" Visible="false"
                                            VisibleIndex="3">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Stock Quantity" FieldName="StkQuantity" Visible="false"
                                            VisibleIndex="4">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Balance Stock" FieldName="BalancrStk" Visible="false"
                                            VisibleIndex="5">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Batch Number" FieldName="BatchNo"
                                            VisibleIndex="6">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Mfg Date" FieldName="MfgDate"
                                            VisibleIndex="7">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Expiry Date" FieldName="ExpiryDate"
                                            VisibleIndex="8">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Serial Number" FieldName="SerialNo"
                                            VisibleIndex="9">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="10" Width="80px">
                                            <DataItemTemplate>
                                                <a href="javascript:void(0);" onclick="fn_Edit('<%# Container.KeyValue %>')" title="Delete">
                                                    <img src="../../../assests/images/Edit.png" /></a>
                                                &nbsp;
                                                        <a href="javascript:void(0);" id="ADelete" onclick="fn_Deletecity('<%# Container.KeyValue %>')" title="Delete">
                                                            <img src="/assests/images/crs.png" /></a>
                                            </DataItemTemplate>
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                    <ClientSideEvents EndCallback="OnWarehouseEndCallback" />
                                    <SettingsPager Visible="false"></SettingsPager>
                                    <SettingsLoadingPanel Text="Please Wait..." />
                                </dxe:ASPxGridView>
                            </div>
                            <div class="clearfix">
                                <br />
                                <div style="align-content: center">
                                    <dxe:ASPxButton ID="btnWarehouseSave" ClientInstanceName="cbtnWarehouseSave" Width="50px" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary">
                                        <ClientSideEvents Click="function(s, e) {FinalWarehouse();}" />
                                    </dxe:ASPxButton>
                                </div>
                            </div>
                        </div>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="LightGray" ForeColor="Black" />
            </dxe:ASPxPopupControl>

            <dxe:ASPxCallbackPanel runat="server" ID="NCallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
                <PanelCollection>
                    <dxe:PanelContent runat="server">
                    </dxe:PanelContent>
                </PanelCollection>
                <ClientSideEvents EndCallback="CallbackPanelEndCall" />
            </dxe:ASPxCallbackPanel>

            <!--Source Wahehouse Modal -->
            <div class="modal fade" id="SourceWarehouseModel" role="dialog">
                <div class="modal-dialog">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Source Warehouse Search</h4>
                        </div>
                        <div class="modal-body">
                            <input type="text" onkeydown="SourceWarehousekeydown(event)" id="txtSourceWarehouseSearch" autofocus width="100%" placeholder="Search By Warehouse Name" />

                            <div id="SourceWarehouseTable">
                                <table border='1' width="100%" class="dynamicPopupTbl">
                                    <tr class="HeaderStyle">
                                        <th class="hide">id</th>
                                        <th>Source Warehouse</th>

                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
            <!--Product Modal -->

            <!--Destination Wahehouse Modal -->
            <div class="modal fade" id="DestinationWarehouseModel" role="dialog">
                <div class="modal-dialog">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Destination Warehouse Search</h4>
                        </div>
                        <div class="modal-body">
                            <input type="text" onkeydown="DestinationWarehousekeydown(event)" id="txtDestinationWarehouseSearch" autofocus width="100%" placeholder="Search By Warehouse Name" />

                            <div id="DestinationWarehouseTable">
                                <table border='1' width="100%" class="dynamicPopupTbl">
                                    <tr class="HeaderStyle">
                                        <th class="hide">id</th>
                                        <th>Destination Warehouse</th>

                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
            <!--Product Modal -->

            <!--Entity Modal -->
            <div class="modal fade" id="EntityModel" role="dialog">
                <div class="modal-dialog">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Entity Search</h4>
                        </div>
                        <div class="modal-body">
                            <input type="text" onkeydown="Entitykeydown(event)" id="txtEntitySearch" autofocus width="100%" placeholder="Search By Entity Name or Unique Id" />
                            <div id="EntityTable">
                                <table border='1' width="100%" class="dynamicPopupTbl">
                                    <tr class="HeaderStyle">
                                        <th class="hide">id</th>
                                        <th>Unique Id</th>
                                        <th>Entity Name</th>

                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>

                </div>
            </div>

            <dxe:ASPxPopupControl ID="PosView" runat="server"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cPosView" Height="650px"
                Width="1200px" HeaderText="Product" Modal="true">

                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                    </dxe:PopupControlContentControl>
                </ContentCollection>
            </dxe:ASPxPopupControl>

            <dxe:ASPxPopupControl ID="DirectAddCustPopup" runat="server"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="AspxDirectAddCustPopup" Height="650px"
                Width="1020px" HeaderText="Add New Entity" Modal="true" AllowResize="true" ResizingMode="Postponed">

                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                    </dxe:PopupControlContentControl>
                </ContentCollection>
            </dxe:ASPxPopupControl>
            <asp:HiddenField ID="hdnProjectSelectInEntryModule" runat="server" />
            <asp:HiddenField ID="hdnHierarchySelectInEntryModule" runat="server" />
            <asp:HiddenField ID="hdnValuationRateWHStockTransfer" runat="server" />
            <asp:HiddenField ID="hdnProjectMandatory" runat="server" />
            <asp:HiddenField ID="hdnEntityRequiredWarehouseStockTransfer" runat="server" />
            <asp:HiddenField ID="hdnLockFromDate" runat="server" />
            <asp:HiddenField ID="hdnLockToDate" runat="server" />
            <asp:HiddenField ID="hdnLockFromDateCon" runat="server" />
            <asp:HiddenField ID="hdnLockToDateCon" runat="server" />
            <asp:HiddenField ID="hdnMandatoryEntityWarehouseStockTransfer" runat="server" />
            <asp:HiddenField ID="hdnEntityId" runat="server" />
            <asp:HiddenField ID="hdnBranchReqTaggingWST" runat="server" />
            <asp:HiddenField ID="hdnTypeReturn" runat="server" />
            <asp:HiddenField ID="hdnWarehouseRepeatStockTransfer" runat="server" />
</asp:Content>
