<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="StockValuation.aspx.cs" Inherits="Reports.Reports.GridReports.StockValuation" %>

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
        .hide {
            display: none;
        }
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
            bottom: 180px;
            background: #fff;
            box-shadow: 1px 1px 10px #0000001c;
            right: 55%;
        }
        /*rev end Pallab*/  
    </style>

    <%-- For multiselection when click on ok button--%>
    <script type="text/javascript">
        function SetSelectedValues(Id, Name, ArrName) {
            if (ArrName == 'ClassSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#ClassModel').modal('hide');
                    ctxtClass.SetText(Name);
                    GetObjectID('hdnClassId').value = key;
                    ctxtProdName.SetText('');
                    $('#txtProdSearch').val('')
                    var OtherDetailsProd = {}
                    OtherDetailsProd.SearchKey = 'undefined text';
                    OtherDetailsProd.ClassID = '';
                    var HeaderCaption = [];
                    HeaderCaption.push("Code");
                    HeaderCaption.push("Name");
                    HeaderCaption.push("Hsn");
                    callonServerM("Services/Master.asmx/GetClassWiseProduct", OtherDetailsProd, "ProductTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ProductSource");
                }
                else {
                    ctxtClass.SetText('');
                    GetObjectID('hdnClassId').value = '';
                }
            }
            else if (ArrName == 'ProductSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#ProdModel').modal('hide');
                    ctxtProdName.SetText(Name);
                    GetObjectID('hdncWiseProductId').value = key;
                }
                else {
                    ctxtProdName.SetText('');
                    GetObjectID('hdncWiseProductId').value = '';
                }
            }
            else if (ArrName == 'BrandSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#BrandModel').modal('hide');
                    ctxtBrandName.SetText(Name);
                    GetObjectID('hdnBranndId').value = key;
                }
                else {
                    ctxtBrandName.SetText('');
                    GetObjectID('hdnBranndId').value = '';
                }
            }
        }
    </script>
   <%-- For multiselection when click on ok button--%>

   <%-- For Class multiselection--%>
 
     <script type="text/javascript">
         $(document).ready(function () {
             $('#ClassModel').on('shown.bs.modal', function () {
                 $('#txtClassSearch').focus();
             })
         })
         var ClassArr = new Array();
         $(document).ready(function () {
             var ClassObj = new Object();
             ClassObj.Name = "ClassSource";
             ClassObj.ArraySource = ClassArr;
             arrMultiPopup.push(ClassObj);
         })
         function ClassButnClick(s, e) {
             $('#ClassModel').modal('show');
         }
         function Class_KeyDown(s, e) {
             if (e.htmlEvent.key == "Enter") {
                 $('#ClassModel').modal('show');
             }
         }
         function Classkeydown(e) {
             var OtherDetails = {}
             if ($.trim($("#txtClassSearch").val()) == "" || $.trim($("#txtClassSearch").val()) == null) {
                 return false;
             }
             OtherDetails.SearchKey = $("#txtClassSearch").val();
             if (e.code == "Enter" || e.code == "NumpadEnter") {
                 var HeaderCaption = [];
                 HeaderCaption.push("Class Name");

                 if ($("#txtClassSearch").val() != "") {
                     callonServerM("Services/Master.asmx/GetClass", OtherDetails, "ClassTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ClassSource");
                 }
             }
             else if (e.code == "ArrowDown") {
                 if ($("input[dPropertyIndex=0]"))
                     $("input[dPropertyIndex=0]").focus();
             }
         }
         function SetfocusOnseach(indexName) {
             if (indexName == "dPropertyIndex")
                 $('#txtClassSearch').focus();
             else
                 $('#txtClassSearch').focus();
         }
   </script>
 <%-- For Class multiselection--%>

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
             OtherDetails.ClassID = hdnClassId.value;
             if (e.code == "Enter" || e.code == "NumpadEnter") {
                 var HeaderCaption = [];
                 HeaderCaption.push("Code");
                 HeaderCaption.push("Name");
                 HeaderCaption.push("Hsn");
                 if ($("#txtProdSearch").val() != "") {
                     callonServerM("Services/Master.asmx/GetClassWiseProduct", OtherDetails, "ProductTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ProductSource");
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
   </script>
      <%-- For Product multiselection--%>

   <%-- For Brand multiselection--%>
 
     <script type="text/javascript">
         $(document).ready(function () {
             $('#BrandModel').on('shown.bs.modal', function () {
                 $('#txtBrandSearch').focus();
             })
         })
         var BrandArr = new Array();
         $(document).ready(function () {
             var BrandObj = new Object();
             BrandObj.Name = "BrandSource";
             BrandObj.ArraySource = BrandArr;
             arrMultiPopup.push(BrandObj);
         })
         function BrandButnClick(s, e) {
             $('#BrandModel').modal('show');
         }
         function Brand_KeyDown(s, e) {
             if (e.htmlEvent.key == "Enter") {
                 $('#BrandModel').modal('show');
             }
         }
         function Brandkeydown(e) {
             var OtherDetails = {}
             if ($.trim($("#txtBrandSearch").val()) == "" || $.trim($("#txtBrandSearch").val()) == null) {
                 return false;
             }
             OtherDetails.SearchKey = $("#txtBrandSearch").val();
             if (e.code == "Enter" || e.code == "NumpadEnter") {
                 var HeaderCaption = [];
                 HeaderCaption.push("Name");
                 if ($("#txtBrandSearch").val() != "") {
                     callonServerM("Services/Master.asmx/GetBrand", OtherDetails, "BrandTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "BrandSource");
                 }
             }
             else if (e.code == "ArrowDown") {
                 if ($("input[dPropertyIndex=0]"))
                     $("input[dPropertyIndex=0]").focus();
             }
         }
         function SetfocusOnseach(indexName) {
             if (indexName == "dPropertyIndex")
                 $('#txtBrandSearch').focus();
             else
                 $('#txtBrandSearch').focus();
         }
   </script>
      <%-- For Brand multiselection--%>

    <script type="text/javascript">
        function ClearGridLookup() {
            var grid = gridquotationLookup.GetGridView();
            grid.UnselectRows();
        }
        $(function () {
            ///  BindBranches(null);
            //    BindCustomerVendor(0);
            cProductbranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            //cProductComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);
            $("#ddlbranchHO").change(function () {
                var Ids = $(this).val();
                cProductbranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            })
        });
        //function BindProduct(type) {
        //    cProductComponentPanel.PerformCallback('BindComponentGrid' + '~' + type);
        //}
        function Callback_EndCallback() {
            $("#drdExport").val(0);
        }
        function CallbackPanelEndCall(s, e) {
             <%--Rev Subhra 20-12-2018   0017670--%>
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
            //End of Subhra
            Grid.Refresh();
        }
        function CallbackPanelDetEndCall(s, e) {
            cgridValuationdetails.Refresh();
        }

        function CloseGridQuotationLookupbranch() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }
    </script>


    <script type="text/javascript">
        //for Esc
        document.onkeyup = function (e) {
            if (event.keyCode == 27 && cpopupApproval.GetVisible() == true) { //run code for alt+N -- ie, Save & New  
                popupHide();
            }
        }
        function popupHide(s, e) {
            cpopupApproval.Hide();
            Grid.Focus();
            $("#drdExport").val(0);
        }
        function cxdeToDate_OnChaged(s, e) {
            var data = "OnDateChanged";
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();

            //CallServer(data, "");
            // Grid.PerformCallback('');
        }
        function btn_ShowRecordsClick(e) {
            e.preventDefault;
            var data = "OnDateChanged";
            $("#hfIsStockValFilter").val("Y");
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
            //CallServer(data, "");
            // alert( data);
            if (gridbranchLookup.GetValue() == null) {
                jAlert('Please select atleast one branch for generate the report.');
            }
            else {
                cCallbackPanel.PerformCallback(data);
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
        function OnContextMenuItemClick(sender, args) {
            if (args.item.name == "ExportToPDF" || args.item.name == "ExportToXLS") {
                args.processOnServer = true;
                args.usePostBack = true;
            } else if (args.item.name == "SumSelected")
                args.processOnServer = true;
        }
        function BudgetAfterHide(s, e) {
            popupbudget.Hide();
        }
        function CloseGridQuotationLookup() {
            gridquotationLookup.ConfirmCurrentSelection();
            gridquotationLookup.HideDropDown();
            gridquotationLookup.Focus();
        }
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
        function OpenBillDetails(branch, prodid) {
            $("#hfIsStockValDetFilter").val("Y");
            //cgridValuationdetails.PerformCallback('BndPopupgrid~' + branch + '~' + prodid);
            cCallbackPanelDetail.PerformCallback('BndPopupgrid~' + branch + '~' + prodid);
            cpopupApproval.Show();
            return true;
        }
        function popupHide(s, e) {
            cpopupApproval.Hide();
        }
        $(document).ready(function () {
            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                // BindLedgerType(Ids);                    
                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');
            })
        });
        function selectAll() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAll() {
            gridbranchLookup.gridView.UnselectRows();
        }
        //function selectAll_product() {
        //    gridquotationLookup.gridView.SelectRows();
        //}
        //function unselectAll_product() {
        //    gridquotationLookup.gridView.UnselectRows();
        //}
        //function selectAllclass() {
        //    clookupClass.gridView.SelectRows();
        //}
        //function unselectAllclass() {
        //    clookupClass.gridView.UnselectRows();
        //}
        //function _selectAll() {
        //    clookupBrand.gridView.SelectRows();
        //}
        //function _unselectAll() {
        //    clookupBrand.gridView.UnselectRows();
        //}
        //function GetAllClass() {
        //    //    alert(clookupClass.GetGridView().GetRowKey(clookupClass.GetGridView().GetFocusedRowIndex()));
        //    clookupClass.GetGridView().GetSelectedFieldValues("ProductClass_ID", GotSelectedValues);
        //}
        //function GotSelectedValues(selectedValues) {
        //    ///  alert(selectedValues);
        //    cProductComponentPanel.PerformCallback('BindComponentGrid' + '~' + selectedValues);
        //}
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
        .padTopbutton {
                padding-top: 15px;
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
        <%--<div class="panel-title">
            <h3>Stock Valuation</h3>
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
                   <%--Rev Subhra 20-12-2018   0017670--%>
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
        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />
        <div class="clearfix row">
            <div class="col-md-2">
                <div style="color: #b5285f;" class="clsTo">
                    <asp:Label ID="Label4" runat="Server" Text="Head Branch : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </div>
                <div>
                    <asp:DropDownList ID="ddlbranchHO" runat="server" Width="100%"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2 branch-selection-box">
                 <div style="color: #b5285f;" class="clsTo">
                    <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </div>
                <div>
                    <asp:ListBox ID="ListBoxBranches" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>
                    <asp:HiddenField ID="hdnActivityType" runat="server" />
                    <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                    <span id="MandatoryActivityType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                    <asp:HiddenField ID="hdnSelectedBranches" runat="server" />
                    <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cProductbranchComponentPanel" OnCallback="Componentbranch_Callback">
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
                                                                <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll"  UseSubmitBehavior="False" />
                                                            <%--</div>--%>
                                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll"  UseSubmitBehavior="False" />
                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookupbranch" UseSubmitBehavior="False" />
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
                <div style="color: #b5285f;" class="clsTo">
                    <asp:Label ID="Label7" runat="Server" Text="Valuation Technique : " CssClass="mylabel1"
                        ></asp:Label>
                </div>
                <div>
                    <asp:DropDownList ID="ddlValTech" runat="server" Width="100%">
                        <asp:ListItem Text="Average" Value="A"></asp:ListItem>
                        <asp:ListItem Text="FIFO" Value="F"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <div style="color: #b5285f;" class="clsFrom">
                    <asp:Label ID="Label5" runat="Server" Text="Class : " CssClass="mylabel1"></asp:Label>
                </div>
                <div>
                    <dxe:ASPxButtonEdit ID="txtClass" runat="server" ReadOnly="true" ClientInstanceName="ctxtClass" Width="100%" TabIndex="5">
                        <Buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>
                        </Buttons>
                        <ClientSideEvents ButtonClick="function(s,e){ClassButnClick();}" KeyDown="function(s,e){Class_KeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>
                </div>
            </div>
            <div class="col-md-2">
                <div style="color: #b5285f;" class="clsTo">
                    <asp:Label ID="Label3" runat="Server" Text="Product : " CssClass="mylabel1"
                        Width="110px"></asp:Label>
                </div>
                <div>
                    <dxe:ASPxButtonEdit ID="txtProdName" runat="server" ReadOnly="true" ClientInstanceName="ctxtProdName" Width="100%" TabIndex="5">
                        <Buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>
                        </Buttons>
                        <ClientSideEvents ButtonClick="function(s,e){ProductButnClick();}" KeyDown="function(s,e){Product_KeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>
                </div>
            </div>
            <div class="col-md-2">
                <div style="color: #b5285f;" class="clsFrom">
                    <asp:Label ID="Label6" runat="Server" Text="Brand : " CssClass="mylabel1"></asp:Label>
                </div>
                <div>
                    <dxe:ASPxButtonEdit ID="txtBrandName" runat="server" ReadOnly="true" ClientInstanceName="ctxtBrandName" Width="100%" TabIndex="5">
                        <Buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>
                        </Buttons>
                        <ClientSideEvents ButtonClick="function(s,e){BrandButnClick();}" KeyDown="function(s,e){Brand_KeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>
                </div>
            </div>
            <div class="col-md-2 hide">
                
                    <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                        <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                
                <td class="hide">
                    <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                
            </div>
            <div class="clear"></div>
            <div class="col-md-2">
                
                <div style="color: #b5285f;" class="clsTo">
                    <asp:Label ID="lblToDate" runat="Server" Text="As On Date : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </div>
                
                <div>
                    <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>

                    </dxe:ASPxDateEdit>
                </div>
            </div>
             <%--<div class="col-md-4">
                <div style="color: #b5285f; padding-top: 20px" class="clsTo">
                    <asp:CheckBox ID="chkOWMSTVT" runat="server" />
                    Override Product Valuation Technique in Master
                </div>
            </div>--%>
           
            <%--<div class="col-md-2">
                <div style="color: #b5285f; padding-top: 20px" class="clsTo">
                    <asp:CheckBox ID="chkConsopasondt" runat="server" />
                    Opening As On Date
                </div>
            </div>--%>
           <%-- <div class="col-md-2">
                <div style="color: #b5285f; padding-top: 20px" class="clsTo">
                    <asp:CheckBox ID="chkConsLandCost" runat="server" />
                    Consider Landed Cost
                </div>
            </div>--%>

            <div class="col-md-4" style="margin-top:17px;color: #b5285f; font-weight: bold;">
                <dxe:ASPxCheckBox runat="server" ID="chkOWMSTVT" Checked="false" Text="Override Product Valuation Technique in Master">
                </dxe:ASPxCheckBox>
            </div>
            <div class="col-md-2" style="margin-top:17px;color: #b5285f; font-weight: bold;">
                <dxe:ASPxCheckBox runat="server" ID="chkConsopasondt" Checked="false" Text="Opening As On Date">
                </dxe:ASPxCheckBox>
            </div>
            <div class="col-md-2" style="margin-top:17px;color: #b5285f; font-weight: bold;">
                <dxe:ASPxCheckBox runat="server" ID="chkConsLandCost" Checked="false" Text="Consider Landed Cost">
                </dxe:ASPxCheckBox>
            </div>
            <div class="col-md-2" style="margin-top:17px;color: #b5285f; font-weight: bold;">
                <dxe:ASPxCheckBox runat="server" ID="chkConsOverHeadCost" Checked="false" Text="Consider Overhead Cost">
                </dxe:ASPxCheckBox>
            </div>
            <div class="clear"></div> 
           <%-- <div class="col-md-2 padTopbutton">
                <button id="btnShow" class="btn btn-primary" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                     <% if (rights.CanExport)
                           { %>
                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                    OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">XLSX</asp:ListItem>
                    <asp:ListItem Value="2">PDF</asp:ListItem>
                    <asp:ListItem Value="3">CSV</asp:ListItem>
                    <asp:ListItem Value="4">RTF</asp:ListItem>
                </asp:DropDownList>
                 <% } %>
            </div>--%>

            <div class="col-md-2 pb-5 padTopbutton">
                <div class="">
                    <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                    <% if (rights.CanExport)
                        { %> 
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                        OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">XLSX</asp:ListItem>
                        <asp:ListItem Value="2">PDF</asp:ListItem>
                        <asp:ListItem Value="3">CSV</asp:ListItem>
                        <asp:ListItem Value="4">RTF</asp:ListItem>
                    </asp:DropDownList>
                    <% } %>
                </div>
           </div>
        </div>

        <table class="pull-left">
            <tr>
                

<%--                <td style="padding-left: 13px; width: 100px">
                    <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                        <asp:Label ID="Label5" runat="Server" Text="Class : " CssClass="mylabel1"></asp:Label>
                    </div>
                </td>
                <td>
                    <dxe:ASPxGridLookup ID="lookupClass" ClientInstanceName="clookupClass" SelectionMode="Multiple" runat="server"
                        OnDataBinding="lookupClass_DataBinding" KeyFieldName="ProductClass_ID" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
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
                                                <div class="hide">
                                                    <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAllclass" />
                                                </div>
                                                <dxe:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAllclass" />
                                                <dxe:ASPxButton ID="ASPxButton5" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseLookup" UseSubmitBehavior="False" />
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
                        <ClientSideEvents ValueChanged="GetAllClass" />
                    </dxe:ASPxGridLookup>
                </td>--%>
                 
              <%--  <td style="width: 254px">
                    <asp:ListBox ID="ListBoxCustomerVendor" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>
                    <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cProductComponentPanel" OnCallback="ComponentProduct_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="lookup_quotation" SelectionMode="Multiple" runat="server" ClientInstanceName="gridquotationLookup"
                                    OnDataBinding="lookup_quotation_DataBinding"
                                    KeyFieldName="ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                        <dxe:GridViewDataColumn FieldName="Name" Visible="true" VisibleIndex="1" Caption="Product Name" Width="180" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                        <dxe:GridViewDataColumn FieldName="Doc Code" Visible="true" VisibleIndex="2" Caption="Product Code" Width="100" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                        <dxe:GridViewDataColumn FieldName="Hsn" Visible="true" VisibleIndex="3" Caption="HSN Code" Width="100" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                    </Columns>
                                    <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                        <Templates>
                                            <StatusBar>
                                                <table class="OptionsTable" style="float: right">
                                                    <tr>
                                                        <td>
                                                            <div class="hide">
                                                                <dxe:ASPxButton ID="btn_prodtct" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_product" />
                                                            </div>
                                                            <dxe:ASPxButton ID="btn_product2" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_product" />                                                            
                                                            <dxe:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" UseSubmitBehavior="False" />
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
                    <asp:HiddenField ID="hdnSelectedCustomerVendor" runat="server" />
                    <span id="MandatoryCustomerType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                </td>--%>
                
            </tr>
            <tr>


                <td>
                    
                </td>
                <td>
                   
                </td>
              <%--  <td>
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
                                                <div class="hide">
                                                    <dxe:ASPxButton ID="ASPxButton7" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="_selectAll" />
                                                </div>
                                                <dxe:ASPxButton ID="ASPxButton8" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="_unselectAll" />
                                                <dxe:ASPxButton ID="ASPxButton9" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="_CloseLookup" UseSubmitBehavior="False" />
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
                </td>--%>
                <td style="padding-left: 10px; padding-top: 3px" colspan="2">                    
                    <%-- <% } %>--%>
                </td>
            </tr>


            <tr>
            </tr>
        </table>
        <div class="pull-right">
        </div>
        <table class="TableMain100">

            <tr>

                <td colspan="2">
                    <div>
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SLNO"
                            OnSummaryDisplayText="ShowGrid_SummaryDisplayText" ClientSideEvents-BeginCallback="Callback_EndCallback" 
                             DataSourceID="GenerateEntityServerModeDataSource" Settings-HorizontalScrollBarMode="Auto" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight ="300">
                            <%--OnDataBinding="gridvaluationsummary_DataBinding" --%>
                            <Columns>

                                <dxe:GridViewDataTextColumn FieldName="branch_description" Width="280" Caption="Unit" VisibleIndex="1" />
                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="sProducts_Description" Width="420" Caption="Product Name">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>
                                        <a href="javascript:void(0)" onclick="OpenBillDetails('<%#Eval("branch_id") %>','<%#Eval("sproducts_Id") %>')" class="pad">
                                            <%#Eval("sproducts_description")%>
                                        </a>
                                    </DataItemTemplate>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="STOCKUOM" Width="80" Caption="Unit" VisibleIndex="3" />
                                <dxe:GridViewDataTextColumn FieldName="ProductClass_Description" Width="280" Caption="Class" VisibleIndex="4" />
                                <dxe:GridViewDataTextColumn FieldName="Brand_Name" Width="220" Caption="Brand" VisibleIndex="5" />
                                <%--Rev Debashis--%>
                                <%--<dxe:GridViewDataTextColumn FieldName="IN_QTY_OP" Width="110" Caption="Quantity" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.00">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>--%>
                                <dxe:GridViewDataTextColumn FieldName="IN_QTY_OP" Width="110" Caption="Quantity" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.0000">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <%--End of Rev Debashis--%>

                                <dxe:GridViewDataTextColumn FieldName="IN_PRICE_OP" Width="110" Caption="Rate" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="0.00">
                                     <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="IN_TOTAL_OP" Width="110" Caption="Total" VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="0.00">
                                     <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                            </Columns>
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" ColumnResizeMode="Control" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>

                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="IN_QTY_OP" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="IN_TOTAL_OP" SummaryType="Sum" />
                            </TotalSummary>
                        </dxe:ASPxGridView>
                            <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="STOCKVALUATION_REPORT" ></dx:LinqServerModeDataSource>

                    </div>
                </td>
            </tr>
        </table>
    </div>
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

    <dxe:ASPxPopupControl ID="popupApproval" runat="server" ClientInstanceName="cpopupApproval"
        Width="1200px" Height="600px" ScrollBars="Vertical" HeaderText="Valuation Details" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="TopSides" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <% if (rights.CanExport)
                            { %> 
                <asp:DropDownList ID="ddldetails" runat="server" CssClass="btn btn-sm btn-primary"
                    AutoPostBack="true" OnSelectedIndexChanged="cmbExport1_SelectedIndexChanged">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">XLSX</asp:ListItem>
                    <asp:ListItem Value="2">PDF</asp:ListItem>
                    <asp:ListItem Value="3">CSV</asp:ListItem>
                    <asp:ListItem Value="4">RTF</asp:ListItem>
                </asp:DropDownList>
                <% } %>

                <div class="row">
                    <div>
                    </div>

                    <div class="col-md-12">


                        <dxe:ASPxGridView ID="grivaluation" runat="server" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="cgridValuationdetails" KeyFieldName="SLNO"
                            DataSourceID="GenerateEntityServerDetailsModeDataSource"  OnSummaryDisplayText="ShowGrid1_SummaryDisplayText" 
                            Settings-HorizontalScrollBarMode="Auto" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight ="300">
                          <%--  OnCustomCallback="gridPendingApproval_CustomCallback" OnDataBinding="grid_DataBinding"--%>
                            <Columns>
                                <%--VisibleIndex="0" FixedStyle="Left" Width="150px">--%>
                                <dxe:GridViewDataTextColumn Caption="Document Date" FieldName="Document_Date" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" 
                                    VisibleIndex="0" Width="150px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Document No" FieldName="Document_No" VisibleIndex="1" Width="120px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Document Type" FieldName="Trans_Type" VisibleIndex="2" Width="200px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Product" FieldName="sProducts_Description" VisibleIndex="3" Width="150px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                
                                <dxe:GridViewDataTextColumn FieldName="ProductClass_Description" Width="100px" Caption="Class" VisibleIndex="4" />

                                <dxe:GridViewDataTextColumn FieldName="Brand_Name" Width="100px" Caption="Brand" VisibleIndex="5" />

                                <dxe:GridViewDataTextColumn FieldName="STOCKUOM" Width="80px" Caption="Stock Unit" VisibleIndex="6" />

                                <dxe:GridViewDataTextColumn FieldName="ALTSTOCKUOM" Width="80px" Caption="Alt. Unit" VisibleIndex="7" />

                                <%--Rev Debashis--%>
                                <%--<dxe:GridViewDataTextColumn FieldName="IN_QTY_OP" Caption="Available Stock" Width="120px" VisibleIndex="8">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="ALTIN_QTY" Caption="Alt. Stock Qty." Width="120px" VisibleIndex="9">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>--%>

                                <dxe:GridViewDataTextColumn FieldName="IN_QTY_OP" Caption="Available Stock" Width="120px" VisibleIndex="8">
                                    <PropertiesTextEdit DisplayFormatString="0.0000"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="ALTIN_QTY" Caption="Alt. Stock Qty." Width="120px" VisibleIndex="9">
                                    <PropertiesTextEdit DisplayFormatString="0.0000"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <%--End of Rev Debashis--%>

                                <dxe:GridViewDataTextColumn FieldName="IN_PRICE_OP" Caption="Rate" Width="120px" VisibleIndex="10">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="LANDEDCOSTVALUE" Caption="Landed Cost" Width="120px" VisibleIndex="11">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="IN_TOTAL_OP" Caption="Value" Width="120px" VisibleIndex="12">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <%--Rev Debashis--%>
                                <%--<dxe:GridViewDataTextColumn FieldName="IN_QTY" Caption="Recd. Qty." Width="120px" VisibleIndex="13">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                     <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="ALTIN_QTY" Caption="Recd. Alt. Qty." Width="120px" VisibleIndex="14">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="OUT_QTY" Caption="Delv. Qty." Width="120px" VisibleIndex="15">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn FieldName="ALTOUT_QTY" Caption="Delv. Alt. Qty." Width="120px" VisibleIndex="16">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>--%>
                                <dxe:GridViewDataTextColumn FieldName="IN_QTY" Caption="Recd. Qty." Width="120px" VisibleIndex="13">
                                    <PropertiesTextEdit DisplayFormatString="0.0000"></PropertiesTextEdit>
                                     <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="ALTIN_QTY" Caption="Recd. Alt. Qty." Width="120px" VisibleIndex="14">
                                    <PropertiesTextEdit DisplayFormatString="0.0000"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="OUT_QTY" Caption="Delv. Qty." Width="120px" VisibleIndex="15">
                                    <PropertiesTextEdit DisplayFormatString="0.0000"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn FieldName="ALTOUT_QTY" Caption="Delv. Alt. Qty." Width="120px" VisibleIndex="16">
                                    <PropertiesTextEdit DisplayFormatString="0.0000"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <%--End of Rev Debashis--%>

                                <dxe:GridViewDataTextColumn FieldName="IN_RATE" Caption="Doc. Rate" Width="120px" VisibleIndex="17">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="IN_VALUE" Caption="Recd. Value" Width="120px" VisibleIndex="18">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="OUT_VALUE" Caption="Delv. Value" Width="120px" VisibleIndex="19">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <%--Rev Debashis--%>
                                <%--<dxe:GridViewDataTextColumn FieldName="CLOSE_QTY" Caption="Balance Qty." Width="120px" VisibleIndex="20">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>--%>
                                <dxe:GridViewDataTextColumn FieldName="CLOSE_QTY" Caption="Balance Qty." Width="120px" VisibleIndex="20">
                                    <PropertiesTextEdit DisplayFormatString="0.0000"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <%--End of Rev Debashis--%>

                                <dxe:GridViewDataTextColumn FieldName="CLOSE_RATE" Caption="Close Rate" Width="120px" VisibleIndex="21">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CLOSE_VAL" Caption="Balance Value" Width="120px" VisibleIndex="22">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                            </Columns>

                            <SettingsBehavior AllowFocusedRow="true" AllowGroup="true" />

                            <SettingsEditing Mode="Inline">
                            </SettingsEditing>
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>
                            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsLoadingPanel Text="Please Wait..." />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />

                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="IN_QTY_OP" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="LANDEDCOSTVALUE" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="IN_TOTAL_OP" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="IN_QTY" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="OUT_QTY" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="IN_VALUE" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="OUT_VALUE" SummaryType="Sum" />
                            </TotalSummary>

                        </dxe:ASPxGridView>
                          <dx:LinqServerModeDataSource ID="GenerateEntityServerDetailsModeDataSource" runat="server" OnSelecting="GenerateEntityServerDetailsModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="STOCKVALUATION_REPORT" ></dx:LinqServerModeDataSource>


                    </div>
                    <div class="clear"></div>



                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents CloseUp="popupHide" />

    </dxe:ASPxPopupControl>


    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupbudget" Height="500px"
        Width="1200px" HeaderText="Details" Modal="true" AllowResize="true">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>

        <ClientSideEvents CloseUp="BudgetAfterHide" />
    </dxe:ASPxPopupControl>
       <!--Class Modal -->
    <div class="modal fade" id="ClassModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Class Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Classkeydown(event)" id="txtClassSearch" width="100%" placeholder="Search By Class Name" />
                    <div id="ClassTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>Class Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('ClassSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('ClassSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
      <asp:HiddenField ID="hdnClassId" runat="server" />
    <!--Class Modal -->

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
                                <th>Hsn</th>
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
         <asp:HiddenField ID="hdncWiseProductId" runat="server" />
    
    <!--Product Modal -->
  
     <!--Brand Modal -->
    <div class="modal fade" id="BrandModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Brand Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Brandkeydown(event)" id="txtBrandSearch" width="100%" placeholder="Search By Brand Name" />
                    <div id="BrandTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('BrandSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('BrandSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
      <asp:HiddenField ID="hdnBranndId" runat="server" />
    <!--Brand Modal -->


<dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
    <PanelCollection>
        <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfIsStockValFilter" runat="server" />
        </dxe:PanelContent>
    </PanelCollection>
      <ClientSideEvents EndCallback="CallbackPanelEndCall" />
</dxe:ASPxCallbackPanel>

<dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelDetail" ClientInstanceName="cCallbackPanelDetail" OnCallback="CallbackPanelDetail_Callback">
    <PanelCollection>
        <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfIsStockValDetFilter" runat="server" />
        </dxe:PanelContent>
    </PanelCollection>
      <ClientSideEvents EndCallback="CallbackPanelDetEndCall" />
</dxe:ASPxCallbackPanel>

</asp:Content>