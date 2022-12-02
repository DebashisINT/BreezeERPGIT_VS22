<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="BOMRegisterDetails.aspx.cs" Inherits="Reports.Reports.GridReports.BOMRegisterDetails" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchMultiPopup.js"></script>
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <style>
        #pageControl, .dxtc-content {
            overflow: visible !important;
        }

        #MandatoryAssign {
            position: absolute;
            right: -17px;
            top: 6px;
        }

        .btnOkformultiselection {
            border-width: 1px;
            padding: 4px 10px;
            font-size: 13px !important;
            margin-right: 6px;
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

         #ListBoxWarehouse {
            width: 200px;
        }

        .hide {
            display: none;
        }

        .dxtc-activeTab .dxtc-link {
            color: #fff !important;
        }
    </style>


    <script type="text/javascript">

        $(function () {
            cBranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
        });

        $(function () {
            cWarehouseComponentPanel.PerformCallback('BindWarehouseGrid');
        });

        $(function () {
            cProjectPanel.PerformCallback('BindProjectGrid');
        });

        $(document).ready(function () {
            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');
            })

            $("#ListBoxWarehouse").chosen().change(function () {
                var Ids = $(this).val();

                $('#<%=hdnSelectedWarehouse.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');

            })

            var ProjectSelection = document.getElementById('hdnProjectSelection').value;
            if (ProjectSelection == "0") {
                $('#divProj').addClass('hidden');
            }
            else {
                $('#divProj').removeClass('hidden');
            }

            $("#ddlbranchHO").change(function () {
                var Ids = $(this).val();
                $('#MandatoryActivityType').attr('style', 'display:none');
                $("#hdnSelectedBranches").val('');
                cBranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            })
        })

    </script>

    <%-- For multiselection when click on ok button--%>
    <script type="text/javascript">
        function SetSelectedValues(Id, Name, ArrName) {
            if (ArrName == 'ProductSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#ProdModel').modal('hide');
                    ctxtProdName.SetText(Name);
                    GetObjectID('hdnProductId').value = key;
                }
                else {
                    ctxtProdName.SetText('');
                    GetObjectID('hdnProductId').value = '';
                }
            }
            else if (ArrName == 'BOMSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#BOMModel').modal('hide');
                    ctxtBOM.SetText(Name);
                    GetObjectID('hdnBOMId').value = key;
                }
                else {
                    ctxtBOM.SetText('');
                    GetObjectID('hdnBOMId').value = '';
                }
            }
        }
  </script>
  <%-- For multiselection when click on ok button--%>
  
  <%-- For BOM multiselection--%>
 
     <script type="text/javascript">
         $(document).ready(function () {
             $('#BOMModel').on('shown.bs.modal', function () {
                 $('#txtBOMSearch').focus();
             })

         })
         var BOMArr = new Array();
         $(document).ready(function () {
             var BOMObj = new Object();
             BOMObj.Name = "BOMSource";
             BOMObj.ArraySource = BOMArr;
             arrMultiPopup.push(BOMObj);
         })
         function BOMButnClick(s, e) {
             $('#BOMModel').modal('show');
         }

         function BOM_KeyDown(s, e) {
             if (e.htmlEvent.key == "Enter") {
                 $('#BOMModel').modal('show');
             }
         }

         function BOMkeydown(e) {
             var OtherDetails = {}

             if ($.trim($("#txtBOMSearch").val()) == "" || $.trim($("#txtBOMSearch").val()) == null) {
                 return false;
             }
             OtherDetails.SearchKey = $("#txtBOMSearch").val();

             if (e.code == "Enter" || e.code == "NumpadEnter") {

                 var HeaderCaption = [];
                 HeaderCaption.push("BOM No.");

                 if ($("#txtBOMSearch").val() != "") {
                     callonServerM("Services/Master.asmx/GetBOM", OtherDetails, "BOMTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "BOMSource");
                 }
             }
             else if (e.code == "ArrowDown") {
                 if ($("input[dPropertyIndex=0]"))
                     $("input[dPropertyIndex=0]").focus();
             }
         }

         function SetfocusOnseach(indexName) {
             if (indexName == "dPropertyIndex")
                 $('#txtBOMSearch').focus();
             else
                 $('#txtBOMSearch').focus();
         }
   </script>
   <%-- For BOM multiselection--%>

  <%-- For Product multiselection--%> 
     <script type="text/javascript">
         $(document).ready(function () {
             $('#ProdModel').on('shown.bs.modal', function () {
                 $('#txtProdSearch').focus();
             })
         })
         var ProdArr = new Array();
         $(document).ready(function () {
             var ProdObj = new Object();
             ProdObj.Name = "ProductSource";
             ProdObj.ArraySource = ProdArr;
             arrMultiPopup.push(ProdObj);
         })
         function ProductButnClick(s, e) {
             $('#ProdModel').modal('show');
         }

         function Product_KeyDown(s, e) {
             if (e.htmlEvent.key == "Enter") {
                 $('#ProdModel').modal('show');
             }
         }

         function Productkeydown(e) {
             var OtherDetails = {}

             if ($.trim($("#txtProdSearch").val()) == "" || $.trim($("#txtProdSearch").val()) == null) {
                 return false;
             }
             OtherDetails.SearchKey = $("#txtProdSearch").val();

             if (e.code == "Enter" || e.code == "NumpadEnter") {

                 var HeaderCaption = [];
                 HeaderCaption.push("Code");
                 HeaderCaption.push("Name");
                 HeaderCaption.push("Hsn");

                 if ($("#txtProdSearch").val() != "") {
                     callonServerM("Services/Master.asmx/GetNormalProduct", OtherDetails, "ProductTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ProductSource");
                 }
             }
             else if (e.code == "ArrowDown") {
                 if ($("input[dPropertyIndex=0]"))
                     $("input[dPropertyIndex=0]").focus();
             }
         }

         function SetfocusOnseach(indexName) {
             if (indexName == "dPropertyIndex")
                 $('#txtProdSearch').focus();
             else
                 $('#txtProdSearch').focus();
         }

         function CheckShowSum(s, e) {
             if (s.GetCheckState() == 'Checked') {
                 CchkShowCost.SetEnabled(true);
             }
             else {
                 CchkShowCost.SetCheckState('UnChecked');
                 CchkShowCost.SetEnabled(false);
             }
         }
   </script>
   <%-- For Product multiselection--%>

    <script type="text/javascript">

        function cxdeToDate_OnChaged(s, e) {
            var data = "OnDateChanged";
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
        }

        function btn_ShowRecordsClick(e) {
            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            var ProjectSelection = document.getElementById('hdnProjectSelection').value;
            var ProjectSelectionInReport = document.getElementById('hdnProjectSelectionInReport').value;
            e.preventDefault;
            var data = "OnDateChanged";
            $("#hfIsBOMRegDetSumFilter").val("Y");
            $("#drdExport").val(0);
            if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                jAlert('Please select atleast one Branch for generate the report.');
            }
            else if (ProjectSelection == "1") {
                if (ProjectSelectionInReport == "Yes" && gridprojectLookup.GetValue() == null) {
                    jAlert('Please select atleast one Project for generate the report.');
                }
                else {
                    cCallbackPanel.PerformCallback(data + '~' + $("#ddlbranchHO").val());
                }
            }
            else {
                cCallbackPanel.PerformCallback(data + '~' + $("#ddlbranchHO").val());
            }
            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "For the period: " + FromDate + " To " + ToDate;
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

        function OpenDocDetails(Uniqueid, doctype, docno) {
            var url = '';
            if (doctype == "Purchase Indent") {
                url = '/OMS/Management/Activities/PurchaseIndent.aspx?key=' + Uniqueid + '&IsTagged=1&req=' + docno;
            }
            else if (doctype == "Project Indent") {
                url = '/OMS/Management/Activities/PmsProjectIndent.aspx?key=' + Uniqueid + '&IsTagged=1&req=' + docno;
            }

            popupdetails.SetContentUrl(url);
            popupdetails.Show();
        }

        function DetailsAfterHide(s, e) {
            popupdetails.Hide();
        }

        function selectAll() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAll() {
            gridbranchLookup.gridView.UnselectRows();
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

        function selectAllWH() {
            gridwarehouseLookup.gridView.SelectRows();
        }
        function unselectAllWH() {
            gridwarehouseLookup.gridView.UnselectRows();
        }

        function CloseLookupwarehouse() {
            gridwarehouseLookup.ConfirmCurrentSelection();
            gridwarehouseLookup.HideDropDown();
            gridwarehouseLookup.Focus();
        }

    </script>
    <script>
        function Callback_EndCallback() {
            $("#drdExport").val(0);
        }

        function CloseGridBranchLookup() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }

    </script>
    <style>
        
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

    <div>

        <asp:HiddenField ID="hdnexpid" runat="server" />
    </div>
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
        <div class="row">
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">Head Branch</label>
                <div>
                    <asp:DropDownList ID="ddlbranchHO" runat="server" Width="100%"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"></asp:Label></label>
                <asp:ListBox ID="ListBoxBranches" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="60px" Width="100%" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>
                <asp:HiddenField ID="hdnActivityType" runat="server" />

                <dxe:ASPxCallbackPanel runat="server" ID="ComponentBranchPanel" ClientInstanceName="cBranchComponentPanel" OnCallback="Componentbranch_Callback">
                    <PanelCollection>
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
                                                        <%--<div class="hide">--%>
                                                            <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll" UseSubmitBehavior="False" />
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll" UseSubmitBehavior="False"/>                                                        
                                                        <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridBranchLookup" UseSubmitBehavior="False" />
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

                <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                <span id="MandatoryActivityType" style="display: none" class="validclass">
                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                <asp:HiddenField ID="hdnSelectedBranches" runat="server" />
            </div>

            <div class="col-md-2">
                 <div style="color: #b5285f;margin-bottom: 4px;" class="clsTo">
                    <asp:Label ID="Label5" runat="Server" Text="Warehouse : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </div>
                <div>
                    <asp:ListBox ID="ListBoxWarehouse" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>
                    <span id="MandatoryActivityType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                    <asp:HiddenField ID="hdnSelectedWarehouse" runat="server" />
                    <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel2" ClientInstanceName="cWarehouseComponentPanel" OnCallback="Componentwarehouse_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="lookup_warehouse" SelectionMode="Multiple" runat="server" ClientInstanceName="gridwarehouseLookup"
                                    OnDataBinding="lookup_warehouse_DataBinding"
                                    KeyFieldName="ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                        <dxe:GridViewDataColumn FieldName="Description" Visible="true" VisibleIndex="1" width="200px" Caption="Warehouse Name" Settings-AutoFilterCondition="Contains">
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
                                                                <dxe:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAllWH"  UseSubmitBehavior="False" />
                                                            <%--</div>--%>
                                                            <dxe:ASPxButton ID="ASPxButton7" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAllWH"  UseSubmitBehavior="False" />
                                                            <dxe:ASPxButton ID="ASPxButton8" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseLookupwarehouse" UseSubmitBehavior="False" />
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
                </div>
            </div>

            <div class="col-md-2">
                <div style="color: #b5285f;margin-bottom: 4px;" class="clsTo">
                    <asp:Label ID="Label7" runat="Server" Text="BOM Type : " CssClass="mylabel1"
                        ></asp:Label>
                </div>
                <div>
                    <asp:DropDownList ID="ddlBOMType" runat="server" Width="100%">
                        <asp:ListItem Text="All" Value="AL"></asp:ListItem>
                        <asp:ListItem Text="Production" Value="P"></asp:ListItem>
                        <asp:ListItem Text="Sales" Value="S"></asp:ListItem>
                        <asp:ListItem Text="Assembly" Value="A"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
           
            <%--<div class="clear"></div>--%>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;margin-bottom: 4px;" class="clsFrom">
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

            <%--<div class="clear"></div>--%>            
            <div class="clear"></div>
            <div class="col-md-2 " id="divProj" style="padding-bottom: 13px;">
                <div style="color: #b5285f" class="clsTo">
                    <asp:Label ID="Label2" runat="Server" Text="Project : " CssClass="mylabel1"></asp:Label>
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

            <div class="col-md-2">
                 <div style="color: #b5285f;" class="clsTo">
                        <asp:Label ID="Label4" runat="Server" Text="BOM : " CssClass="mylabel1"
                            Width="100px"></asp:Label>
                    </div>
                <div>
                    <dxe:ASPxButtonEdit ID="txtBOM" runat="server" ReadOnly="true" ClientInstanceName="ctxtBOM" Width="100%" TabIndex="6">
                        <Buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>
                        </Buttons>
                        <ClientSideEvents ButtonClick="function(s,e){BOMButnClick();}" KeyDown="function(s,e){BOM_KeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>
                </div>
            </div>
            
            <div class="col-md-2">
                 <div style="color: #b5285f;" class="clsTo">
                        <asp:Label ID="Label3" runat="Server" Text="Product : " CssClass="mylabel1"
                            Width="100px"></asp:Label>
                    </div>
                <div>
                    <dxe:ASPxButtonEdit ID="txtProdName" runat="server" ReadOnly="true" ClientInstanceName="ctxtProdName" Width="100%" TabIndex="7">
                        <Buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>
                        </Buttons>
                        <ClientSideEvents ButtonClick="function(s,e){ProductButnClick();}" KeyDown="function(s,e){Product_KeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>
                </div>
            </div>

            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold; padding-top: 15px" class="clsTo">
                    <dxe:ASPxCheckBox runat="server" ID="chkAddCost" Checked="false" Text="Show Additional Cost" ClientInstanceName="CchkAddCost">
                    </dxe:ASPxCheckBox>
                </div>
            </div>

            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold; padding-top: 15px" class="clsTo">
                    <dxe:ASPxCheckBox runat="server" ID="chkRevNoDt" Checked="false" Text="Show Revision No. & Date" ClientInstanceName="CchkRevNoDt">
                    </dxe:ASPxCheckBox>
                </div>
            </div>

            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold; padding-top: 15px" class="clsTo">
                    <dxe:ASPxCheckBox runat="server" ID="chkFGWH" Checked="false" Text="Show FG Warehouse" ClientInstanceName="CchkFGWH">
                    </dxe:ASPxCheckBox>
                </div>
            </div>

            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold; padding-top: 15px" class="clsTo">
                    <dxe:ASPxCheckBox runat="server" ID="chkHeadRem" Checked="false" Text="Show Header Remarks" ClientInstanceName="CchkHeadRem">
                    </dxe:ASPxCheckBox>
                </div>
            </div>
            <div class="clear"></div>

            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold; padding-top: 11px" class="clsTo">
                    <dxe:ASPxCheckBox runat="server" ID="chkStatus" Checked="false" Text="Show Status" ClientInstanceName="CchkStatus">
                    </dxe:ASPxCheckBox>
                </div>
            </div>

            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold; padding-top: 11px" class="clsTo">
                    <dxe:ASPxCheckBox runat="server" ID="chkCreateBy" Checked="false" Text="Show Created by" ClientInstanceName="CchkCreateby">
                    </dxe:ASPxCheckBox>
                </div>
            </div>

            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold; padding-top: 11px" class="clsTo">
                    <dxe:ASPxCheckBox runat="server" ID="chkShowSum" Checked="false" Text="Show Summary" ClientInstanceName="CchkShowSum">
                        <ClientSideEvents CheckedChanged="CheckShowSum" />
                    </dxe:ASPxCheckBox>
                </div>
            </div>

            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold; padding-top: 11px" class="clsTo">
                    <dxe:ASPxCheckBox runat="server" ID="chkShowCost" Checked="false" ClientEnabled="false" Text="Show Cost" ClientInstanceName="CchkShowCost">
                    </dxe:ASPxCheckBox>
                </div>
            </div>
            <%--<div class="clear"></div>--%>

            <div class="col-md-2" style="padding:0;padding-top: 7px;padding-bottom: 5px;">
            <table>
                <tr>
                    <td  style="padding-left:15px;">
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
                    </td>
                </tr>
            </table>
                
            </div>
            <div class="clear"></div>
        </div>
    </div>

    <div>
    </div>

    <!--BOM Modal -->
    <div class="modal fade" id="BOMModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">BOM Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="BOMkeydown(event)" id="txtBOMSearch" width="100%" placeholder="Search By BOM No" />
                    <div id="BOMTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>BOM No.</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default" onclick="DeSelectAll('BOMSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('BOMSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
      <asp:HiddenField ID="hdnBOMId" runat="server" />
    <!--BOM Modal -->

    <!--Product Modal -->
    <div class="modal fade" id="ProdModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Product Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Productkeydown(event)" id="txtProdSearch" width="100%" placeholder="Search By Product Name" />
                    <div id="ProductTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>Code</th>
                                 <th>Name</th>
                                <th>HSN</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('ProductSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('ProductSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
         <asp:HiddenField ID="hdnProductId" runat="server" />    
    <!--Product Modal -->

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <asp:HiddenField ID="hdnCustomerId" runat="server" />

     <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupdetails" Height="500px"
        Width="1200px" HeaderText="Details" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>

        <ClientSideEvents CloseUp="DetailsAfterHide" />
    </dxe:ASPxPopupControl>

     <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">

                 <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SEQ"
                            DataSourceID="GenerateEntityServerModeDataSource" Settings-HorizontalScrollBarMode="Visible"
                            OnSummaryDisplayText="ShowGrid_SummaryDisplayText" ClientSideEvents-BeginCallback="Callback_EndCallback"
                            Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Auto">
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Unit" FieldName="BRANCHDESC" Width="220px" VisibleIndex="1" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Project Name" Width="200px" FieldName="PROJ_NAME" VisibleIndex="2">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="BOM Type" Width="100px" FieldName="BOM_TYPE" VisibleIndex="3">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="BOM_NO" Width="130px" Caption="BOM#" >
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Center"/>
                                    <EditFormSettings Visible="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataDateColumn Caption="Date" Width="100px" FieldName="BOMDATE" VisibleIndex="5" PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataDateColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="BOMSTATUS" Width="130px" Caption="Status" >
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Left"/>
                                    <EditFormSettings Visible="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="REV_NO" Width="130px" Caption="Revision No." >
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <EditFormSettings Visible="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataDateColumn Caption="Revision Date" Width="100px" FieldName="REVDATE" VisibleIndex="8" PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataDateColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="FGCODE" Width="200px" Caption="FG Code" >
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <EditFormSettings Visible="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="FGDESC" Width="200px" Caption="FG Description" >
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <EditFormSettings Visible="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="FGQTY" Caption="FG Qty." Width="100px" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.0000;">
                                      <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="12" FieldName="FGWHDESC" Width="200px" Caption="FG Warehouse" >
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <EditFormSettings Visible="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="13" FieldName="HREMARKS" Width="130px" Caption="Header Remarks">
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <EditFormSettings Visible="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Part No." FieldName="PARTNO" Width="200px" VisibleIndex="14">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Description" FieldName="PARTCODE" Width="200px" VisibleIndex="15">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Drawing No." FieldName="DRAWINGNO" Width="130px" VisibleIndex="16">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Drawing Rev. No." FieldName="DREVISIONNO" Width="130px" VisibleIndex="17">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Warehouse" FieldName="PRODWHDESC" Width="200px" VisibleIndex="18">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Resource" FieldName="RCCODE" Width="200px" VisibleIndex="19">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Resource Desc." FieldName="RCDESC" Width="200px" VisibleIndex="20">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="STKQTY" Caption="Qty." Width="100px" VisibleIndex="21" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.000;">
                                      <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                
                                <dxe:GridViewDataTextColumn Caption="UOM" FieldName="STKUOM" Width="100px" VisibleIndex="22">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PRICE" Caption="Price" Width="100px" VisibleIndex="23" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;">
                                     <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="AMOUNT" Caption="Amount" Width="100px" VisibleIndex="24" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;">
                                     <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn VisibleIndex="25" FieldName="REMARKS" Width="130px" Caption="Remarks">
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <EditFormSettings Visible="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="ADDCOST" Caption="Additional Cost" Width="100px" VisibleIndex="26" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;">
                                     <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="STANDARDCOST" Caption="Standard Cost" Width="100px" VisibleIndex="27" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;">
                                     <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="ACTUALCOST" Caption="Actual Cost" Width="100px" VisibleIndex="28" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;">
                                     <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Created by" Width="130px" FieldName="CREATEDBY" VisibleIndex="29">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>
                            <settings showfilterrow="True" showfilterrowmenu="true" showstatusbar="Visible" usefixedtablelayout="true" />

                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="BRANCHDESC" SummaryType="Custom" Tag="Branch"/>
                                <dxe:ASPxSummaryItem FieldName="FGQTY" SummaryType="Sum" Tag="FGQty"/>
                                <dxe:ASPxSummaryItem FieldName="STKQTY" SummaryType="Sum" Tag="StkQty"/>
                                <dxe:ASPxSummaryItem FieldName="AMOUNT" SummaryType="Sum" Tag="Amt"/>
                                <dxe:ASPxSummaryItem FieldName="ADDCOST" SummaryType="Sum" Tag="AddCost"/>
                                <dxe:ASPxSummaryItem FieldName="STANDARDCOST" SummaryType="Sum" Tag="StandCost"/>
                                <dxe:ASPxSummaryItem FieldName="ACTUALCOST" SummaryType="Sum" Tag="ActualCost"/>
                            </TotalSummary>
                        </dxe:ASPxGridView>
                    <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                            ContextTypeName="ReportSourceDataContext" TableName="BOMREGISTERDETSUM_REPORT"></dx:LinqServerModeDataSource>

            <asp:HiddenField ID="hfIsBOMRegDetSumFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
    <asp:HiddenField ID="hdnProjectSelection" runat="server" />
    <asp:HiddenField ID="hdnProjectSelectionInReport" runat="server" />
</asp:Content>