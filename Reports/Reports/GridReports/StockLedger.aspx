<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="StockLedger.aspx.cs" Inherits="Reports.Reports.GridReports.StockLedger" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
     Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<%--debashis--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchMultiPopup.js"></script>
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

        .dxlpLoadingPanel_PlasticBlue tbody, .dxlpLoadingPanelWithContent_PlasticBlue tbody
        {
            position: absolute;
            bottom: 150px;
            background: #fff;
            box-shadow: 1px 1px 10px #0000001c;
            right: -55px;
        }
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
                    GetObjectID('hdnProductId').value = key;
                }
                else {
                    ctxtProdName.SetText('');
                    GetObjectID('hdnProductId').value = '';
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
             OtherDetails.ClassID = "";

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

        $(function () {
            cBranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            $("#ddlbranchHO").change(function () {
                var Ids = $(this).val();
                cBranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            })
        });

        function Callback_EndCallback() {
            $("#drdExport").val(0);
        }
        function CallbackPanelEndCall(s, e) {
            <%--Rev Subhra 24-12-2018   0017670--%>
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
            //End of Subhra
            Grid.Refresh();
        }

        function selectAll() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAll() {
            gridbranchLookup.gridView.UnselectRows();
        }

        function CloseLookupbranch() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
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
    </script>

    <script type="text/javascript">
        //for Esc
        document.onkeyup = function (e) {
            if (event.keyCode == 27 && popupdocument.GetVisible() == true) { //run code for alt+N -- ie, Save & New  
                popupHide();
            }
        }
        function popupHide(s, e) {
            popupdocument.Hide();
            Grid.Focus();
            $("#drdExport").val(0);
        }

        function btn_ShowRecordsClick(e) {
            e.preventDefault;
            var data = "OnDateChanged";
            $("#hfIsStockLedgerFilter").val("Y");
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();

            if (gridbranchLookup.GetValue() == null) {
                jAlert('Please select atleast one branch to generate the report.');
            }
            else if (cxdeFromDate.GetValue() == null || cxdeToDate.GetValue() == null) {
                jAlert('Date can not be blank.');
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

        function OpenDetailsDocuments(Doc_ID, TransType) {
            if (TransType == 'PB') {
                url = '/OMS/Management/Activities/PurchaseInvoice.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'PR') {
                url = '/OMS/Management/Activities/PReturn.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'SC') {
                url = '/OMS/Management/Activities/SalesChallanAdd.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'SD' || TransType == 'OD') {
                url = '/OMS/Management/Activities/CustomerDeliveryPendingOurDelv.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'OUR') {
                url = '/OMS/Management/Activities/OldUnitReceivedFromServiceCenter.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'GRN') {
                url = '/OMS/Management/Activities/PurchaseChallan.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'SR') {
                url = '/OMS/Management/Activities/SalesReturn.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'SRM') {
                url = '/OMS/Management/Activities/ReturnManual.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'SRN') {
                url = '/OMS/Management/Activities/ReturnNormal.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'RFS') {
                url = '/OMS/Management/Activities/ReceiveFromServiceCenter.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'BI') {
                url = '/OMS/Management/Activities/BranchTransferIn.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'RSI') {

            }
            else if (TransType == 'ITS') {
                url = '/OMS/Management/Activities/IssueToServiceCenter.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'BO') {
                url = '/OMS/Management/Activities/BranchTransferOut.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'RSO') {
            }
            else if (TransType == 'CRI') {
                url = '/OMS/Management/Activities/IssuetoCustomerReturn.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'SI') {
                url = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'POS') {
                url = '/OMS/Management/Activities/posSalesInvoice.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'SAIN' || TransType == 'SAOUT') {
                url = '/OMS/Management/Activities/StockAdjustmentAdd.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'WHSTIN' || TransType == 'WHSTOUT') {
                url = '/OMS/Management/Activities/WarehousewiseStockTransferAdd.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'WHSJIN' || TransType == 'WHSJOUT') {
                url = '/OMS/Management/Activities/WarehousewiseStockJournalAdd.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'StkReceipt') {
                url = '/Import/GoodsReceivedNoteAdd_Import.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }

            popupdocument.SetContentUrl(url);
            popupdocument.Show();

        }

        function DocumentAfterHide(s, e) {
            popupdocument.Hide();
        }

        $(document).ready(function () {
            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');
            })
        });
        
    </script>

    <style>
        .pl-10{
            padding-left: 10px;
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
        .padTbl>tbody>tr>td {
            padding:8px 20px 8px 0;
            
        }
    </style>

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
                   <%--Rev Subhra 24-12-2018   0017670--%>
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
        <div class="row">
            <div class="col-md-2">
                <div style="color: #b5285f;" class="clsTo">
                    <asp:Label ID="Label4" runat="Server" Text="Head Branch : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </div>
                <div>
                    <asp:DropDownList ID="ddlbranchHO" runat="server" Width="100%"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <div style="color: #b5285f;" class="clsTo">
                        <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                <div>
                    <span id="MandatoryActivityType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                    <asp:HiddenField ID="hdnSelectedBranches" runat="server" />
                    <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cBranchComponentPanel" OnCallback="Componentbranch_Callback" Width="100%">
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
                                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll" UseSubmitBehavior="False" />
                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseLookupbranch" UseSubmitBehavior="False" />
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
            <div class="clear"></div>
            <div class="col-md-2">
                 <div style="color: #b5285f;" class="clsFrom">
                        <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                 <div>
                    <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </div>
            </div>
            <div class="col-md-2">
                <div style="color: #b5285f;" class="clsTo">
                        <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"
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
            <div class="col-md-8">
                <table width="100%">
                    <tr>
                        <td>
                            <div style="color: #b5285f; padding-top: 20px" class="clsTo">
                                <%--<asp:CheckBox ID="chkOWMSTVT" runat="server" Checked="false" Text="Override Product Valuation Technique in Master"/>--%>
                                <dxe:ASPxCheckBox runat="server" ID="chkOWMSTVT" Checked="false" Text="Override Product Valuation Technique in Master">
                                </dxe:ASPxCheckBox>
                            </div>
                        </td>
                        <td>
                            <div style="color: #b5285f; padding-top: 20px" class="clsTo">
                                <%--<asp:CheckBox runat="server" ID="chkRateValue" Checked="false" Text="Show Valuation Rate & Value" />--%>
                                <dxe:ASPxCheckBox runat="server" ID="chkRateValue" Checked="false" Text="Show Valuation Rate & Value">
                                </dxe:ASPxCheckBox>
                            </div>
                        </td>
                        <td>
                            <div style="color: #b5285f; padding-top: 20px" class="clsTo">
                                <%--<asp:CheckBox runat="server" ID="chkparty" Checked="false" Text="Show Party Name" />--%>
                                <dxe:ASPxCheckBox runat="server" ID="chkparty" Checked="false" Text="Show Party Name">
                                </dxe:ASPxCheckBox>
                            </div>
                        </td>
                        <td>
                            <div style="color: #b5285f; padding-top: 20px" class="clsTo">
                                <%--<asp:CheckBox runat="server" ID="chkDocRate" Checked="false" Text="Show Doc. Rate" />--%>
                                <dxe:ASPxCheckBox runat="server" ID="chkDocRate" Checked="false" Text="Show Doc. Rate">
                                </dxe:ASPxCheckBox>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="clear"></div>
            <div class="col-md-2 pTop10 mBot10 pl-10" style="margin-top:3px;color: #b5285f;">
                <dxe:ASPxCheckBox runat="server" ID="chkConsLandCost" Checked="false" Text="Consider Landed Cost">
                </dxe:ASPxCheckBox>
            </div>
            <div class="col-md-2 pTop10 mBot10 pl-10" style="margin-top:3px;color: #b5285f;">
                <dxe:ASPxCheckBox runat="server" ID="chkConsOverHeadCost" Checked="false" Text="Consider Overhead Cost">
                </dxe:ASPxCheckBox>
            </div>
            <div class="col-md-2 pTop10 mBot10 pl-20" style="margin-top:3px;color: #b5285f;">
                <dxe:ASPxCheckBox runat="server" ID="chkShowMultiUOM" Checked="false" Text="Show Multiple UOM">
                </dxe:ASPxCheckBox>
            </div>
            <div class="col-md-2 pTop10 mBot10 pl-20" style="margin-top:3px;color: #b5285f;">
                <dxe:ASPxCheckBox runat="server" ID="chkPartyInvNoDt" Checked="false" Text="Show Party Inv. No. & Date">
                </dxe:ASPxCheckBox>
            </div>
            <div class="col-md-3 pTop10 mBot10">
                <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                <% if (rights.CanExport)
                    { %> 
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                        OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLSX</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>
                <% } %>
            </div>
        </div>
        <div class="pull-right">
        </div>
        <table class="TableMain100">
            <tr>
                <td colspan="2">
                    <div>
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SEQ"
                            OnSummaryDisplayText="ShowGrid_SummaryDisplayText" ClientSideEvents-BeginCallback="Callback_EndCallback" DataSourceID="GenerateEntityServerModeDataSource"
                            OnHtmlFooterCellPrepared="ShowGrid_HtmlFooterCellPrepared" OnHtmlDataCellPrepared="ShowGrid_HtmlDataCellPrepared" Settings-HorizontalScrollBarMode="Auto"
                            Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight ="300">
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Product" Width="150px" FieldName="SPRODUCTS_DESCRIPTION" VisibleIndex="0" GroupIndex="0">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="ISBOLD" Caption="ISBOLD" Width="0%" VisibleIndex="1" HeaderStyle-CssClass="colDisable">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Unit" FieldName="BRANCH_DESCRIPTION" Width="250px" VisibleIndex="2" FixedStyle="Left" HeaderStyle-CssClass="colDisable">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"> </CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Doc. Date" FieldName="DOCUMENT_DATE" Width="120px" VisibleIndex="3" FixedStyle="Left" HeaderStyle-CssClass="colDisable">
                                    <CellStyle CssClass="gridcellleft"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                    <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Doc. No" FieldName="DOCUMENT_NO" Width="150px" VisibleIndex="4" FixedStyle="Left" HeaderStyle-CssClass="colDisable">
                                   <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <Settings AutoFilterCondition="Contains" />
                                    <DataItemTemplate>
                                        <a href="javascript:void(0)" onclick="OpenDetailsDocuments('<%#Eval("DOC_ID") %>','<%#Eval("MODULE_TYPE") %>')" class="pad">
                                            <%#Eval("DOCUMENT_NO")%>
                                        </a>
                                    </DataItemTemplate>

                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Doc. Type" FieldName="TRANS_TYPE" Width="150px" VisibleIndex="5" FixedStyle="Left" HeaderStyle-CssClass="colDisable">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PARTY" Width="200px" Caption="Party" VisibleIndex="6" HeaderStyle-CssClass="colDisable">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PARTYINVNO" Width="130px" Caption="Party Inv. No." VisibleIndex="7" HeaderStyle-CssClass="colDisable">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PARTYINVDT" Width="90px" Caption="Party Inv. Date" VisibleIndex="8" HeaderStyle-CssClass="colDisable">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PRODUCTCLASS_DESCRIPTION" Width="150px" Caption="Class" VisibleIndex="9" HeaderStyle-CssClass="colDisable">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="BRAND_NAME" Width="150px" Caption="Brand" VisibleIndex="10" HeaderStyle-CssClass="colDisable">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="STOCKUOM" Width="80px" Caption="Stock UOM" VisibleIndex="11" HeaderStyle-CssClass="colDisable">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="MULTIUOM" Width="80px" Caption="Multi UOM" VisibleIndex="12" HeaderStyle-CssClass="colDisable">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="IN_QTY" Caption="Recd. Qty." Width="100px" VisibleIndex="13" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="IN_MULTQTY" Caption="Recd. Multi Qty." Width="100px" VisibleIndex="14" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="OUT_QTY" Caption="Delv. Qty." Width="100px" VisibleIndex="15" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="OUT_MULTQTY" Caption="Delv. Multi Qty." Width="100px" VisibleIndex="16" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="DOC_RATE" Caption="Doc. Rate" Width="100px" VisibleIndex="17" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                 
                                <dxe:GridViewDataTextColumn FieldName="RATE" Caption="Valuation Rate" Width="100px" VisibleIndex="18" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="IN_TOTAL" Caption="Recd. Value" Width="100px" VisibleIndex="19" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="OUT_TOTAL" Caption="Delv. Value" Width="100px" VisibleIndex="20" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CLOSE_VAL" Caption="Balance Value" Width="100px" VisibleIndex="21" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CLOSE_QTY" Caption="Balance Qty." Width="100px" VisibleIndex="22" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                               <dxe:GridViewDataTextColumn FieldName="MULTCLOSE_QTY" Caption="Balance Multi Qty." Width="100px" VisibleIndex="23" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                               </dxe:GridViewDataTextColumn>
                            </Columns>
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" AllowSort="False" ColumnResizeMode="Control" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>

                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="BRAND_NAME" SummaryType="Custom" Tag="Item_Brand"/>
                                <dxe:ASPxSummaryItem FieldName="IN_QTY" SummaryType="Custom" Tag="Item_InQty"/>
                                <dxe:ASPxSummaryItem FieldName="IN_MULTQTY" SummaryType="Custom" Tag="Item_InMQty"/>
                                <dxe:ASPxSummaryItem FieldName="OUT_QTY" SummaryType="Custom" Tag="Item_OutQty"/>
                                <dxe:ASPxSummaryItem FieldName="OUT_MULTQTY" SummaryType="Custom" Tag="Item_OutMQty"/>
                                <dxe:ASPxSummaryItem FieldName="IN_TOTAL" SummaryType="Custom" Tag="Item_InValue"/>
                                <dxe:ASPxSummaryItem FieldName="OUT_TOTAL" SummaryType="Custom" Tag="Item_OutValue"/>                                
                                <dxe:ASPxSummaryItem FieldName="CLOSE_QTY" SummaryType="Custom" Tag="Item_Close"/>
                                <dxe:ASPxSummaryItem FieldName="MULTCLOSE_QTY" SummaryType="Custom" Tag="Item_MClose"/>
                                <dxe:ASPxSummaryItem FieldName="CLOSE_VAL" SummaryType="Custom" Tag="Val_Close"/>
                            </TotalSummary>
                        </dxe:ASPxGridView>
                        <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="STOCKLEDGERRUNNINGBALANCE_REPORT" ></dx:LinqServerModeDataSource>

                    </div>
                </td>
            </tr>
        </table>
    </div>

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupdocument" Height="500px"
        Width="1200px" HeaderText="Details" Modal="true" AllowResize="true">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>

        <ClientSideEvents CloseUp="DocumentAfterHide" />
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
        <asp:HiddenField ID="hfIsStockLedgerFilter" runat="server" />
        </dxe:PanelContent>
    </PanelCollection>
      <ClientSideEvents EndCallback="CallbackPanelEndCall" />
</dxe:ASPxCallbackPanel>
</asp:Content>