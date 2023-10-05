<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                17-04-2023        2.0.37           Pallab              25839: Add Purchase GRN module design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="Purchase Challan" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="PurchaseChallan.aspx.cs" Inherits="ERP.OMS.Management.Activities.PurchaseChallan" EnableEventValidation="false" %>

<%@ Register Src="~/OMS/Management/Activities/UserControls/Purchase_BillingShipping.ascx" TagPrefix="ucBS" TagName="Purchase_BillingShipping" %>
<%--<%@ Register Src="~/OMS/Management/Activities/UserControls/BillingShippingControl.ascx" TagPrefix="ucBS" TagName="BillingShippingControl" %>--%>
<%@ Register Src="~/OMS/Management/Activities/UserControls/VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/TermsConditionsControl.ascx" TagPrefix="uc2" TagName="TermsConditionsControl" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/UOMConversion.ascx" TagPrefix="uc3" TagName="UOMConversionControl" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="https://cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/fixedcolumns/3.3.0/js/dataTables.fixedColumns.min.js"></script>
    <script src="JS/SearchPopupDatatable.js"></script>   
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.9.0/css/all.css" rel="stylesheet" />    
    <script src="../../Tax%20Details/Js/TaxDetailsItemlevelPurchase.js"></script>
    <script type='text/javascript'>
        var SecondUOM = [];
        var SecondUOMProductId = "";
    </script>
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <%--  <script src="JS/SearchPopup.js"></script>--%>
    <script src="JS/ProductStockIN.js?v00000.007"></script>

    <script src="JS/PurchaseChallan.js?v=9.2"></script>

    <link href="CSS/PurchaseChallan.css" rel="stylesheet" />

    <%--Use for set focus on UOM after press ok on UOM--%>
    <script>
        var taxSchemeUpdatedDate = '<%=Convert.ToString(Cache["SchemeMaxDate"])%>';
        function PerformCallToGridBind() {
            var OrderTaggingData = cgridproducts.GetSelectedKeysOnPage();
            GetPurchaseForGstValue();
            cddlPosGstChallan.SetEnabled(false);
            if (OrderTaggingData == 0) {
                cProductsPopup.Hide();
            }
            else {
                grid.PerformCallback('BindGridOnQuotation' + '~' + '@');

                //#### Transporter & Billing/Shipping Tagging ####
                var quote_Id = ctaggingGrid.GetSelectedKeysOnPage();

                if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                    callTransporterControl(quote_Id[0], 'PO');
                }

                if (quote_Id.length > 0) {
                    //Chinmoy added below line

                    GetPurchaseOrderDocumentAddress(quote_Id[0]);
                    //BSDocTagging(quote_Id[0], 'PO');
                }


                if (quote_Id.length > 0) {
                    BindOrderProjectdata(quote_Id[0]);
                }

                //#### Transporter & Billing/Shipping Tagging ####

                //#### TC Control Tagging ####
                if ($("#btn_TermsCondition").is(":visible")) {
                    callTCControl(quote_Id[0], 'PO');
                }
                //#### TC Control Tagging ####

                if (quote_Id.length > 0) {
                    var ComponentDetails = _ComponentDetails.split("~");
                    cgridproducts.cpComponentDetails = null;

                    var ComponentNumber = ComponentDetails[0];
                    var ComponentDate = ComponentDetails[1];

                    ctaggingList.SetValue(ComponentNumber);
                    cPLQADate.SetValue(ComponentDate);
                }

                cProductsPopup.Hide();
            }
        }

        function ddl_Currency_Rate_Change() {
            var Campany_ID = '<%=Session["LastCompany"]%>';
            var LocalCurrency = '<%=Session["LocalCurrency"]%>';
            var basedCurrency = LocalCurrency.split("~");
            var Currency_ID = $("#ddl_Currency").val();


            if (Currency_ID == basedCurrency[0]) {
                ctxtRate.SetValue("0");
                ctxtRate.SetEnabled(false);
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "PurchaseChallan_Add.aspx/GetRate",
                    data: JSON.stringify({ Currency_ID: Currency_ID, Campany_ID: Campany_ID, basedCurrency: basedCurrency[0] }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var data = msg.d;
                        ctxtRate.SetValue(data);
                    }
                });
                ctxtRate.SetEnabled(true);
            }
        }

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
    <style>
        #grid_DXMainTable>tbody>tr>td:nth-child(26) {
            display:none
        }
    </style>

    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 1;
        }

        #GrdSalesReturn {
            max-width: 98% !important;
        }
        #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_PartyDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_PartyDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
        #dt_PartyDate_B-1 #dt_PartyDate_B-1Img
        {
            display: none;
        }

        /*select
        {
            -webkit-appearance: auto;
        }*/

        .calendar-icon
        {
                right: 20px;
                bottom: 8px;
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
            margin-top: 0 !important;
        }

        .col-md-2, .col-md-4 {
    margin-bottom: 10px;
}

        .simple-select::after
        {
                top: 26px;
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

        #ddlInventory, #ddlWarehouse
        {
                -webkit-appearance: auto;
        }



    </style>
    <%--Rev end 1.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">
                <asp:Label ID="lblHeading" runat="server" Text="Add Purchase GRN"></asp:Label>
            </h3>
            <div id="pageheaderContent" class="pull-right reverse wrapHolder content horizontal-images" style="display: none;">
                <div class="Top clearfix">
                    <ul>
                        <li>
                            <div class="lblHolder" id="divAvailableStk">
                                <table>
                                    <tr>
                                        <td>Available Stock</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblAvailableStkPro" runat="server" Text="0.0"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                        <li>
                            <div class="lblHolder" id="divPacking" style="display: none;">
                                <table>
                                    <tr>
                                        <td>Packing Quantity</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblPackingStk" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            <div id="ApprovalCross" runat="server" class="crossBtn"><a href=""><i class="fa fa-times"></i></a></div>
            <div id="divcross" runat="server" class="crossBtn"><a href="PurchaseChallanList.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
        <div class=" form_main row">
        <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
            <TabPages>
                <dxe:TabPage Name="General" Text="General">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                            <div class="row">
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="lbl_Inventory" runat="server" Text="Inventory Item?">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>
                                    <asp:DropDownList ID="ddlInventory" CssClass="backSelect" runat="server" Width="100%">
                                        <asp:ListItem Text="Both" Value="B" />
                                        <asp:ListItem Text="Inventory Item" Value="Y" />
                                        <%--  <asp:ListItem Text="Non Inventory Item" Value="N" />
                                        <asp:ListItem Text="Capital Goods" Value="C" />
                                        <asp:ListItem Text="Service Item" Value="S" />--%>
                                    </asp:DropDownList>
                                </div>
                                <%--Rev 1.0: "simple-select" class add --%>
                                <div class="col-md-2 lblmTop8 simple-select" runat="server" id="divNumberingScheme">
                                    <dxe:ASPxLabel ID="lbl_NumberingScheme" runat="server" Text="Numbering Scheme">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>
                                    <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%"
                                        DataTextField="SchemaName" DataValueField="ID" onchange="CmbScheme_ValueChange()">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="lbl_PIQuoteNo" runat="server" Text="Document No.">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>
                                    <asp:TextBox ID="txtVoucherNo" runat="server" Width="100%" MaxLength="50" onchange="txtBillNo_TextChanged()" Enabled="false">
                                    </asp:TextBox>
                                    <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Posting Date">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>
                                    <dxe:ASPxDateEdit ID="dt_PLQuote" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cPLQuoteDate" Width="100%" UseMaskBehavior="True">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                    </dxe:ASPxDateEdit>
                                    <%--Rev 1.0--%>
                                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                    <%--Rev end 1.0--%>
                                </div>
                                <%--Rev 1.0: "simple-select" class add --%>
                                <div class="col-md-2 lblmTop8 simple-select">
                                    <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="Unit">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>
                                    <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%" DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Enabled="false" onchange="onBranchItems()">
                                    </asp:DropDownList>
                                </div>
                               
                                <div class="col-md-2" id="DivForUnit" runat="server">
                                    <label>For Unit</label>
                                    <div>
                                        <asp:DropDownList ID="ddlForBranch" runat="server" 
                                            DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                 <div class="clear"></div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Vendor">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>
                                    <% if (rightsVendor.CanAdd)
                                       { %>
                                    <a href="#" onclick="AddVendorClick()" style="left: -12px; top: 20px; font-size: 16px;"><i id="openlink" runat="server" class="fa fa-plus-circle" aria-hidden="true"></i></a>
                                    <% } %>
                                    <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%">
                                        <Buttons>
                                            <dxe:EditButton>
                                            </dxe:EditButton>
                                        </Buttons>
                                        <ClientSideEvents ButtonClick="function(s,e){VendorButnClick();}" KeyDown="function(s,e){VendorKeyDown(s,e);}" />
                                    </dxe:ASPxButtonEdit>
                                    <span id="MandatorysCustomer" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                </div>

                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="lbl_ContactPerson" runat="server" Text="Contact Person">
                                    </dxe:ASPxLabel>
                                    <dxe:ASPxComboBox ID="cmbContactPerson" runat="server" OnCallback="cmbContactPerson_Callback" Width="100%" ClientInstanceName="cContactPerson"
                                        Font-Size="12px" ClientSideEvents-EndCallback="cmbContactPersonEndCall">
                                        <ClientSideEvents EndCallback="cmbContactPersonEndCall"></ClientSideEvents>
                                    </dxe:ASPxComboBox>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel9" runat="server" Text="Party Invoice No.">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>
                                    <dxe:ASPxTextBox ID="txtPartyInvoice" ClientInstanceName="ctxtPartyInvoice" runat="server" Width="100%">
                                    </dxe:ASPxTextBox>
                                    <span id="MandatorysPartyinvno" class="POVendor  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel10" runat="server" Text="Party Invoice Date">
                                    </dxe:ASPxLabel>
                                    <dxe:ASPxDateEdit ID="dt_PartyDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cPLPartyDate" Width="100%">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                        <ClientSideEvents LostFocus="function(s, e) {s.HideDropDown();}" GotFocus="function(s, e) {s.ShowDropDown();}" />
                                    </dxe:ASPxDateEdit>
                                    <%--Rev 1.0--%>
                                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                    <%--Rev end 1.0--%>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Select Purchase Order">
                                    </dxe:ASPxLabel>
                                    <dxe:ASPxButtonEdit ID="taggingList" ClientInstanceName="ctaggingList" runat="server" ReadOnly="true" Width="100%">
                                        <Buttons>
                                            <dxe:EditButton>
                                            </dxe:EditButton>
                                        </Buttons>
                                        <ClientSideEvents ButtonClick="taggingListButnClick" KeyDown="taggingListKeyDown" />
                                    </dxe:ASPxButtonEdit>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Purchase Date">
                                    </dxe:ASPxLabel>
                                    <dxe:ASPxTextBox ID="dt_Quotation" runat="server" Width="100%" ClientEnabled="false" ClientInstanceName="cPLQADate">
                                    </dxe:ASPxTextBox>
                                </div>
                                 <div class="clear"></div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="lbl_Refference" runat="server" Text="Reference">
                                    </dxe:ASPxLabel>
                                    <dxe:ASPxTextBox ID="txt_Refference" runat="server" Width="100%">
                                    </dxe:ASPxTextBox>
                                </div>
                               <%--Rev 1.0: "simple-select" class add --%>
                                <div class="col-md-2 lblmTop8 simple-select">
                                    <dxe:ASPxLabel ID="lbl_Currency" runat="server" Text="Currency">
                                    </dxe:ASPxLabel>
                                    <asp:DropDownList ID="ddl_Currency" runat="server" Width="100%"
                                        DataValueField="Currency_ID" DataTextField="Currency_AlphaCode" onchange="ddl_Currency_Rate_Change()">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="lbl_Rate" runat="server" Text="Rate">
                                    </dxe:ASPxLabel>
                                    <dxe:ASPxTextBox ID="txt_Rate" runat="server" Width="100%" ClientInstanceName="ctxtRate">
                                        <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                                        <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                                    </dxe:ASPxTextBox>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="lbl_AmountAre" runat="server" Text="Amounts are">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>
                                    <dxe:ASPxComboBox ID="ddl_AmountAre" runat="server" SelectedIndex="0" ClientIDMode="Static" ClientInstanceName="cddl_AmountAre" Width="100%">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateGSTCSTVAT(e)}" LostFocus="function(s, e) { SetFocusonDemand(e)}" />
                                    </dxe:ASPxComboBox>
                                </div>

                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Place Of Supply[GST]">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">*</span>
                                    <dxe:ASPxComboBox ID="ddlPosGstChallan" runat="server" ClientInstanceName="cddlPosGstChallan" Width="100%" ValueField="System.String">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateChallanPosGst(e)}" />
                                    </dxe:ASPxComboBox>
                                </div>
                                <div class="col-md-2 lblmTop8  hide" style="margin-bottom: 5px">
                                    <dxe:ASPxLabel ID="lblVatGstCst" runat="server" Text="Select GST">
                                    </dxe:ASPxLabel>
                                    <dxe:ASPxComboBox ID="ddl_VatGstCst" runat="server" ClientInstanceName="cddlVatGstCst" Width="100%">
                                    </dxe:ASPxComboBox>
                                </div>
                                <div class="col-md-2 lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel12" runat="server" Text="E-Way Bill Number">
                                    </dxe:ASPxLabel>
                                    <%--<span style="color: red;">*</span>--%>
                                    <asp:TextBox ID="txtEWayBillNumber" runat="server" Width="100%" MaxLength="20">                             
                                    </asp:TextBox>
                                    <span id="MandatoryEWayBillNumber" class="EWayBillNumber  pullleftClass fa fa-exclamation-circle iconRed "
                                        style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                </div>
                                <div class="clear"></div>
                                <div class="col-md-4 lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel11" runat="server" Text="Narration">
                                    </dxe:ASPxLabel>
                                    <dxe:ASPxTextBox ClientInstanceName="ctxtNarration" ID="txtNarration" runat="server" Width="100%" MaxLength="500">
                                    </dxe:ASPxTextBox>

                                </div>

                                <div class="col-md-2 lblmTop8">
                                    <%--<label id="lblProject" runat="server">Project</label>--%>
                                    <dxe:ASPxLabel ID="lblProject" runat="server" Text="Project">
                                    </dxe:ASPxLabel>
                                    <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="ProjectServerModeDataSource"
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
                                        <ClientSideEvents GotFocus="Project_gotFocus" LostFocus="Project_LostFocus" ValueChanged="ProjectValueChange" />
                                        <ClearButton DisplayMode="Always">
                                        </ClearButton>
                                    </dxe:ASPxGridLookup>
                                    <dx:LinqServerModeDataSource ID="ProjectServerModeDataSource" runat="server" OnSelecting="ProjectServerModeDataSource_Selecting"
                                        ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />

                                </div>
                                <div class="col-md-4 lblmTop8">

                                    <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="Hierarchy">
                                    </dxe:ASPxLabel>
                                    <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%" Enabled="false">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div>
                                <br />
                            </div>
                            <div class="col-md-12">
                                <div class="row">
                                    <dxe:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" KeyFieldName="SrlNo"
                                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                                        Settings-ShowFooter="false" SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="160"
                                        OnRowInserting="Grid_RowInserting" OnRowUpdating="Grid_RowUpdating"   OnRowDeleting="Grid_RowDeleting"
                                        OnCellEditorInitialize="grid_CellEditorInitialize" OnHtmlRowPrepared="grid_HtmlRowPrepared" OnDataBinding="grid_DataBinding"
                                        OnBatchUpdate="grid_BatchUpdate" OnCustomCallback="grid_CustomCallback" Settings-HorizontalScrollBarMode="Visible">
                                        <SettingsPager Visible="false"></SettingsPager>
                                        
                                        <Settings VerticalScrollableHeight="160" VerticalScrollBarMode="Auto"></Settings>
                                        
                                        <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                                        <Columns>
                                            <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="30" VisibleIndex="0" Caption="">
                                                <CustomButtons>
                                                    <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                                        <Image Url="/assests/images/crs.png"></Image>
                                                    </dxe:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                            </dxe:GridViewCommandColumn>
                                            <dxe:GridViewDataTextColumn FieldName="RowNo" Caption="Sl" ReadOnly="true" VisibleIndex="1" Width="30px">
                                                <PropertiesTextEdit>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="PO No" FieldName="DocNumber" ReadOnly="true" Width="130" VisibleIndex="2">
                                                <PropertiesTextEdit>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product" VisibleIndex="3" Width="150px" ReadOnly="true">
                                                <PropertiesButtonEdit>
                                                    <%--<ClientSideEvents LostFocus="ProductsGotFocus" GotFocus="ProductsGotFocus" />--%>
                                                    <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" />
                                                    <Buttons>
                                                        <dxe:EditButton Text="..." Width="20px">
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                </PropertiesButtonEdit>
                                            </dxe:GridViewDataButtonEditColumn>
                                            <dxe:GridViewDataTextColumn FieldName="ProductDiscription" Caption="Description" VisibleIndex="4" Width="200px" ReadOnly="true">
                                                <PropertiesTextEdit>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewCommandColumn Caption="Addl Desc." Width="40px" VisibleIndex="5">
                                                <CustomButtons>
                                                    <dxe:GridViewCommandColumnCustomButton ID="CustomaddDescRemarks" Image-ToolTip="Remarks" Image-Url="/assests/images/warehouse.png" Text=" ">
                                                        <Image ToolTip="Addl Desc." Url="/assests/images/more.png">
                                                        </Image>
                                                    </dxe:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                            </dxe:GridViewCommandColumn>

                                            <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Quantity" VisibleIndex="6" Width="100px" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                                                <PropertiesTextEdit>
                                                    <ClientSideEvents GotFocus="QuantityProductsGotFocus" LostFocus="QuantityTextChange" />
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                                    <ValidationSettings Display="None"></ValidationSettings>
                                                    <Style HorizontalAlign="Right"></Style>
                                                </PropertiesTextEdit>
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="PurchaseUOM" Caption="UOM" VisibleIndex="7" Width="100px" ReadOnly="true">
                                                <PropertiesTextEdit>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>


                                            <dxe:GridViewCommandColumn VisibleIndex="8" Caption="Multi UOM" Width="100px">
                                                <CustomButtons>
                                                    <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomMultiUOM" Image-Url="/assests/images/MultiUomIcon.png" Image-ToolTip="Multi UOM">
                                                    </dxe:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                            </dxe:GridViewCommandColumn>
                                             <%--Mantis Issue 24429--%>
                                            <dxe:GridViewDataTextColumn Caption="Multi Qty" CellStyle-HorizontalAlign="Right" FieldName="Order_AltQuantity" PropertiesTextEdit-MaxLength="14" VisibleIndex="9" Width="100px" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right">
                                                    <ClientSideEvents LostFocus="QuantityTextChange" />
                                                    <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" />
                                                    <Style HorizontalAlign="Right">
                                                    </Style>
                                                </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                                     
                                            <dxe:GridViewDataTextColumn Caption="Multi Unit" FieldName="Order_AltUOM" ReadOnly="true" VisibleIndex="10" Width="100px" >
                                                <PropertiesTextEdit>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>

                                            <%--VisibleIndex changed for below columns--%>
                                            <%--End of Mantis Issue 24429--%>


                                            <dxe:GridViewCommandColumn Width="80px" VisibleIndex="11" Caption="Stock">
                                                <CustomButtons>
                                                    <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomWarehouse" Image-Url="/assests/images/warehouse.png">
                                                        <Image Url="/assests/images/warehouse.png"></Image>
                                                    </dxe:GridViewCommandColumnCustomButton>
                                                </CustomButtons>

                                            </dxe:GridViewCommandColumn>
                                            <dxe:GridViewDataTextColumn FieldName="PurchasePrice" Caption="Price" VisibleIndex="12" Width="100px" HeaderStyle-HorizontalAlign="Right">
                                                <PropertiesTextEdit DisplayFormatString="0.000">
                                                    <ClientSideEvents GotFocus="PurchasePriceTextFocus" LostFocus="PurchasePriceTextChange" />
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                                    <ValidationSettings Display="None"></ValidationSettings>
                                                    <Style HorizontalAlign="Right"></Style>
                                                </PropertiesTextEdit>
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataSpinEditColumn FieldName="Discount" Caption="Disc(%)" VisibleIndex="13" Width="80px" HeaderStyle-HorizontalAlign="Right">
                                                <PropertiesSpinEdit MinValue="0" MaxValue="100" AllowMouseWheel="false" DisplayFormatString="0.00" MaxLength="6" DecimalPlaces="2">
                                                    <ClientSideEvents GotFocus="DiscountTextFocus" LostFocus="DiscountValueChange" />
                                                    <SpinButtons ShowIncrementButtons="false"></SpinButtons>
                                                    <Style HorizontalAlign="Right"></Style>
                                                </PropertiesSpinEdit>
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataSpinEditColumn>
                                            <dxe:GridViewDataTextColumn FieldName="TotalAmount" Caption="Amount" VisibleIndex="14" Width="200px" HeaderStyle-HorizontalAlign="Right">
                                                <PropertiesTextEdit DisplayFormatString="0.00">
                                                    <ClientSideEvents GotFocus="AmountTextFocus" LostFocus="AmountTextChange"></ClientSideEvents>
                                                    <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..99&gt;"></MaskSettings>
                                                    <ValidationSettings Display="None"></ValidationSettings>
                                                    <Style HorizontalAlign="Right"></Style>
                                                </PropertiesTextEdit>
                                                <%-- REV RAJDIP--%>
                                                <%--   <PropertiesTextEdit>
                                                    <ClientSideEvents GotFocus="AmountTextFocus" LostFocus="AmountTextChange" />
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <ValidationSettings Display="None"></ValidationSettings>
                                                    <Style HorizontalAlign="Right"></Style>
                                                </PropertiesTextEdit>--%>
                                                <%--END REV rAJDIP--%>
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataButtonEditColumn FieldName="TaxAmount" Caption="Charges" VisibleIndex="15" Width="200px" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                                                <PropertiesButtonEdit Style-HorizontalAlign="Right">
                                                    <ClientSideEvents ButtonClick="TaxAmountClick" GotFocus="TaxAmountFocus" KeyDown="TaxAmountKeyDown" />
                                                    <Buttons>
                                                        <dxe:EditButton Text="..." Width="20px">
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                    <Style HorizontalAlign="Right"></Style>
                                                </PropertiesButtonEdit>
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataButtonEditColumn>
                                            <%--Mantis Issue 24617 [Width changed to 150px from 9%] --%>
                                            <dxe:GridViewDataTextColumn FieldName="NetAmount" Caption="Net Amount" VisibleIndex="16" Width="150px" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="0.00">
                                                    <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..99&gt;"></MaskSettings>
                                                    <ValidationSettings Display="None"></ValidationSettings>
                                                    <Style HorizontalAlign="Right"></Style>
                                                    <%-- <ClientSideEvents LostFocus="NetAmountLostFocus" />--%>
                                                </PropertiesTextEdit>
                                                <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                    <%--Mantis Issue 24635--%>
                                                    <%--<MaskSettings Mask="&lt;0..999999999&gt;.&lt ;00..999&gt;" AllowMouseWheel="false" />--%>
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                                    <%--End of Mantis Issue 24635--%>
                                                    <ValidationSettings Display="None"></ValidationSettings>
                                                    <Style HorizontalAlign="Right"></Style>
                                                </PropertiesTextEdit>
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewDataTextColumn Caption="Remarks" FieldName="ChallanDetails_InlineRemarks" Width="150px" VisibleIndex="17" PropertiesTextEdit-MaxLength="5000">
                                                <PropertiesTextEdit Style-HorizontalAlign="Left">
                                                    <Style HorizontalAlign="Left">
                                                            </Style>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="100px" VisibleIndex="18" Caption="Add New">
                                                <CustomButtons>
                                                    <dxe:GridViewCommandColumnCustomButton ID="CustomAddNewRow" Image-Url="/assests/images/add.png" Text=" ">
                                                        <Image Url="/assests/images/add.png"></Image>
                                                    </dxe:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                            </dxe:GridViewCommandColumn>
                                            <dxe:GridViewDataTextColumn FieldName="TotalQty" Caption="Total Qty" VisibleIndex="19" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="BalanceQty" Caption="Balance Qty" VisibleIndex="20" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="IsComponentProduct" Caption="IsComponentProduct" VisibleIndex="21" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="IsLinkedProduct" Caption="IsLinkedProduct" VisibleIndex="22" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="DocID" Caption="DocID" VisibleIndex="23" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="SrlNo" VisibleIndex="24" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="ProductID" Caption="ProductID" VisibleIndex="25" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="DetailsId" Caption="Details ID" VisibleIndex="26" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                            <%--Rev Mantis Issue 24061--%>
                                            <dxe:GridViewDataTextColumn FieldName="Balance_Amount" Caption="Balance Amount" VisibleIndex="27" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                            <%-- End of Rev Mantis Issue 24061--%>
                                             <dxe:GridViewDataTextColumn FieldName="ChallanDetails_Id" Caption="ChallanDetails_Id" VisibleIndex="28" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>


                                        </Columns>
                                        <ClientSideEvents BatchEditStartEditing="gridFocusedRowChanged" EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" />
                                        <%--<ClientSideEvents  CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" BatchEditStartEditing="gridFocusedRowChanged" />--%>
                                        <SettingsDataSecurity AllowEdit="true" />
                                        <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                            <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                        </SettingsEditing>
                                    </dxe:ASPxGridView>
                                </div>
                            </div>
                            <div style="clear: both;">
                                <br />
                                <div style="display: none;">
                                    <dxe:ASPxLabel ID="txt_Charges" runat="server" Text="0.00" ClientInstanceName="ctxt_Charges" />
                                    <dxe:ASPxLabel ID="txt_cInvValue" runat="server" Text="0.00" ClientInstanceName="cInvValue" />
                                </div>
                            </div>
                            <div class="content reverse horizontal-images clearfix" style="width: 100%; margin-right: 0; padding: 8px 0; height: auto; border-top: 1px solid #ccc; border-bottom: 1px solid #ccc; border-radius: 0;">
                                <ul>
                                    <li class="clsbnrLblTaxableAmt">
                                        <div class="horizontallblHolder">
                                            <table>
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <dxe:ASPxLabel ID="bnrLblTotalQty" runat="server" Text="Total Quantity" ClientInstanceName="cbnrLblTotalQty" />
                                                        </td>
                                                        <td>
                                                            <dxe:ASPxLabel ID="txt_TotalQty" runat="server" Text="0.00" ClientInstanceName="cTotalQty" />
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
                                                            <dxe:ASPxLabel ID="bnrLblTaxableAmt" runat="server" Text="Taxable Amount" ClientInstanceName="cbnrLblTaxableAmt" />
                                                        </td>
                                                        <td>
                                                            <dxe:ASPxLabel ID="txt_TaxableAmtval" runat="server" Text="0.00" ClientInstanceName="cTaxableAmtval" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </li>
                                    <li class="clsbnrLblTaxAmt">
                                        <div class="horizontallblHolder">
                                            <table>
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <dxe:ASPxLabel ID="bnrLblTaxAmt" runat="server" Text="Tax & Charges Amt" ClientInstanceName="cbnrLblTaxAmt" />
                                                        </td>
                                                        <td>
                                                            <dxe:ASPxLabel ID="txt_TaxAmtval" runat="server" Text="0.00" ClientInstanceName="cTaxAmtval" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </li>

                                    <li class="clsbnrLblTaxAmt">
                                        <div class="horizontallblHolder">
                                            <table>
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <dxe:ASPxLabel ID="bnrLblOtherTaxAmt" runat="server" Text="Additional Amt" ClientInstanceName="cbnrLblOtherTaxAmt" />
                                                        </td>
                                                        <td>
                                                            <dxe:ASPxLabel ID="txt_OtherTaxAmtval" runat="server" Text="0.00" ClientInstanceName="cOtherTaxAmtval" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </li>
                                    <li class="clsbnrLblInvVal">
                                        <div class="horizontallblHolder">
                                            <table>
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <dxe:ASPxLabel ID="bnrLblInvVal" runat="server" Text="Total Amount" ClientInstanceName="cbnrLblInvVal" />
                                                        </td>
                                                        <td>
                                                            <dxe:ASPxLabel ID="txt_TotalAmt" runat="server" Text="0.00" ClientInstanceName="cTotalAmt" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </li>
                                </ul>
                            </div>

                            <div class="clearfix" style="padding-top: 3px;">
                                <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="S&#818;ave & New" CssClass="btn btn-success" UseSubmitBehavior="false">
                                    <ClientSideEvents Click="function(s, e) {SaveNew_Click();}" />
                                </dxe:ASPxButton>
                                <dxe:ASPxButton ID="btn_SaveRecordsExit" ClientInstanceName="cbtn_SaveRecordsExit" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-success" UseSubmitBehavior="false">
                                    <ClientSideEvents Click="function(s, e) {SaveExit_Click();}" />
                                </dxe:ASPxButton>
                                <dxe:ASPxButton ID="ASPxButton2" ClientInstanceName="cbtn_SaveRecordsUDF" runat="server" AutoPostBack="False" Text="UDF" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                    <ClientSideEvents Click="function(s, e) {OpenUdf();}" />
                                </dxe:ASPxButton>
                                <dxe:ASPxButton ID="ASPxButton3" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="T&#818;ax & Charges" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                    <ClientSideEvents Click="function(s, e) {Save_TaxesClick();}" />
                                </dxe:ASPxButton>
                                <uc1:VehicleDetailsControl runat="server" ID="VehicleDetailsControl" />
                                <asp:HiddenField ID="hfControlData" runat="server" />
                                <uc2:TermsConditionsControl runat="server" ID="TermsConditionsControl" />
                                <uc3:UOMConversionControl runat="server" ID="UOMConversionControl" />
                                <asp:HiddenField runat="server" ID="hfTermsConditionData" />
                                <asp:HiddenField runat="server" ID="hfTermsConditionDocType" Value="PC" />
                                <asp:HiddenField ID="hidIsLigherContactPage" runat="server" />
                                <asp:Label ID="lbl_IsTagged" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                <asp:CheckBox ID="chkmail" runat="server" Text="Send Mail" />
                            </div>
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>
                <dxe:TabPage Name="[B]illing/Shipping" Text="Our Billing/Shipping">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                            <ucBS:Purchase_BillingShipping runat="server" ID="Purchase_BillingShipping" />
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>
            </TabPages>
            <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
	                                            var Tab0 = page.GetTab(0);
	                                            var Tab1 = page.GetTab(1);
	                                            var Tab2 = page.GetTab(2);
                                                
                                               if(activeTab == Tab0)
	                                            {
	                                                disp_prompt('tab0');
	                                            }
	                                            if(activeTab == Tab1)
	                                            {
	                                                disp_prompt('tab1');
	                                            }
	                                           else if(activeTab == Tab2)
	                                            {
	                                                disp_prompt('tab2');
	                                            }

	                                            }"></ClientSideEvents>
        </dxe:ASPxPageControl>
    </div>
    </div>
    <dxe:ASPxPopupControl ID="aspxTaxpopUp" runat="server" ClientInstanceName="caspxTaxpopUp"
        Width="1000px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter" Height="500px"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True" AllowDragging="true">
        <HeaderTemplate>
            <span style="color: #fff"><strong>Select Tax</strong></span>
            <dxe:ASPxImage ID="ASPxImage1" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                <ClientSideEvents Click="function(s, e){ 
                                                            cgridTax.CancelEdit();
                                                            caspxTaxpopUp.Hide();
                                                        }" />
            </dxe:ASPxImage>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <asp:HiddenField runat="server" ID="HiddenField1" />
                <asp:HiddenField runat="server" ID="HiddenField2" />
                <asp:HiddenField runat="server" ID="HiddenField3" />
                <asp:HiddenField runat="server" ID="HiddenField4" />
                <div id="content-6">
                    <div class="col-sm-3">
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Gross Amount
                                                    <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="(Taxable)" ClientInstanceName="clblTaxableGross"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dxe:ASPxLabel ID="lblTaxProdGrossAmt" runat="server" Text="" ClientInstanceName="clblTaxProdGrossAmt"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div class="col-sm-3 gstGrossAmount hide">
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>GST</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dxe:ASPxLabel ID="lblGstForGross" runat="server" Text="" ClientInstanceName="clblGstForGross"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>

                    <div class="col-sm-3">
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Discount</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dxe:ASPxLabel ID="lblTaxDiscount" runat="server" Text="" ClientInstanceName="clblTaxDiscount"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div class="col-sm-3">
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Net Amount
                                                    <dxe:ASPxLabel ID="ASPxLabel6" runat="server" Text="(Taxable)" ClientInstanceName="clblTaxableNet"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dxe:ASPxLabel ID="lblProdNetAmt" runat="server" Text="" ClientInstanceName="clblProdNetAmt"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div class="col-sm-2 gstNetAmount hide">
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>GST</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dxe:ASPxLabel ID="lblGstForNet" runat="server" Text="" ClientInstanceName="clblGstForNet"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <%--Error Message--%>
                <div id="ContentErrorMsg">
                    <div class="col-sm-8">
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Status
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Tax Code/Charges Not defined.
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <table style="width: 100%;">
                    <tr>
                        <td colspan="2"></td>
                    </tr>

                    <tr>
                        <td colspan="2"></td>
                    </tr>


                    <tr style="display: none">
                        <td><span><strong>Product Basic Amount</strong></span></td>
                        <td>
                            <dxe:ASPxTextBox ID="txtprodBasicAmt" MaxLength="80" ClientInstanceName="ctxtprodBasicAmt" TabIndex="1" ReadOnly="true"
                                runat="server" Width="50%">
                                <MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" />
                            </dxe:ASPxTextBox>
                        </td>
                    </tr>
                    <tr class="cgridTaxClass">
                        <td colspan="3">
                            <dxe:ASPxGridView runat="server" OnBatchUpdate="taxgrid_BatchUpdate" KeyFieldName="Taxes_ID" ClientInstanceName="cgridTax" ID="aspxGridTax"
                                Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridTax_CustomCallback"
                                Settings-ShowFooter="false" AutoGenerateColumns="False" OnCellEditorInitialize="aspxGridTax_CellEditorInitialize"
                                OnRowInserting="taxgrid_RowInserting" OnRowUpdating="taxgrid_RowUpdating" OnRowDeleting="taxgrid_RowDeleting">
                                <Settings VerticalScrollableHeight="150" VerticalScrollBarMode="Auto"></Settings>
                                <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                                <SettingsPager Visible="false"></SettingsPager>
                                <Columns>
                                    <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="Taxes_Name" ReadOnly="true" Caption="Tax Component ID">
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="taxCodeName" ReadOnly="true" Caption="Tax Component Name">
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="calCulatedOn" ReadOnly="true" Caption="Calculated On" HeaderStyle-HorizontalAlign="Right">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Percentage" FieldName="TaxField" VisibleIndex="4" HeaderStyle-HorizontalAlign="Right">
                                        <PropertiesTextEdit DisplayFormatString="0.000" Style-HorizontalAlign="Right">
                                            <ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />
                                            <MaskSettings Mask="<0..999999999999>.<00..999>" AllowMouseWheel="false" />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Amount" Caption="Amount" ReadOnly="true" HeaderStyle-HorizontalAlign="Right">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <ClientSideEvents LostFocus="taxAmountLostFocus" GotFocus="taxAmountGotFocus" />
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                </Columns>
                                <SettingsDataSecurity AllowEdit="true" />
                                <SettingsEditing Mode="Batch">
                                    <BatchEditSettings EditMode="row" ShowConfirmOnLosingChanges="false" />
                                </SettingsEditing>
                                <ClientSideEvents EndCallback="cgridTax_EndCallBack " RowClick="GetTaxVisibleIndex" />
                            </dxe:ASPxGridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <table class="InlineTaxClass">
                                <tr class="GstCstvatClass" style="">
                                    <td style="padding-top: 10px; padding-bottom: 15px; padding-right: 25px"><span><strong>GST</strong></span></td>
                                    <td style="padding-top: 10px; padding-bottom: 15px;">
                                        <dxe:ASPxComboBox ID="cmbGstCstVat" ClientInstanceName="ccmbGstCstVat" runat="server" SelectedIndex="-1" TabIndex="2"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" TextFormatString="{0}"
                                            ClearButton-DisplayMode="Always">
                                            <Columns>
                                                <dxe:ListBoxColumn FieldName="Taxes_Name" Caption="Tax Component ID" Width="250" />
                                                <dxe:ListBoxColumn FieldName="TaxCodeName" Caption="Tax Component Name" Width="250" />
                                            </Columns>
                                        </dxe:ASPxComboBox>
                                    </td>
                                    <td style="padding-left: 15px; padding-top: 10px; padding-bottom: 15px; padding-right: 25px">
                                        <dxe:ASPxTextBox ID="txtGstCstVat" MaxLength="80" ClientInstanceName="ctxtGstCstVat" TabIndex="3" ReadOnly="true" Text="0.00"
                                            runat="server" Width="100%">
                                            <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        </dxe:ASPxTextBox>
                                    </td>
                                    <td>
                                        <input type="button" onclick="recalculateTax()" class="btn btn-info btn-small RecalculateInline" value="Recalculate GST" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <div class="pull-left">
                                <asp:Button ID="Button1" runat="server" Text="O&#818;K" TabIndex="5" CssClass="btn btn-primary mTop" OnClientClick="return BatchUpdate();" Width="85px" UseSubmitBehavior="false" />
                                <asp:Button ID="Button3" runat="server" Text="Cancel" TabIndex="5" CssClass="btn btn-danger mTop" Width="85px" OnClientClick="cgridTax.CancelEdit(); caspxTaxpopUp.Hide(); return false;" />
                            </div>
                            <table class="pull-right">
                                <tr>
                                    <td style="padding-top: 10px; padding-right: 5px"><strong>Total Charges</strong></td>
                                    <td>
                                        <dxe:ASPxTextBox ID="txtTaxTotAmt" MaxLength="80" ClientInstanceName="ctxtTaxTotAmt" Text="0.00" ReadOnly="true"
                                            runat="server" Width="100%" CssClass="pull-left mTop">
                                            <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        </dxe:ASPxTextBox>

                                    </td>
                                </tr>
                            </table>
                            <div class="clear"></div>
                        </td>
                    </tr>

                </table>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>
    <div class="PopUpArea">
        <%--ChargesTax--%>
        <dxe:ASPxPopupControl ID="Popup_Taxes" runat="server" ClientInstanceName="cPopup_Taxes"
            Width="1000px" Height="400px" HeaderText="GRN Taxes" PopupHorizontalAlign="WindowCenter"
            BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentStyle VerticalAlign="Top" CssClass="pad">
            </ContentStyle>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div class="Top clearfix">
                        <div id="content-5" class="col-md-12  wrapHolder content horizontal-images" style="width: 100%; margin-right: 0;">
                            <ul>
                                <li>
                                    <div class="lblHolder">
                                        <table>
                                            <tbody>
                                                <tr>
                                                    <td>Gross Amount Total
                                                                <dxe:ASPxLabel ID="ASPxLabel7" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesTaxableGross"></dxe:ASPxLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxLabel ID="txtProductAmount" runat="server" Text="ASPxLabel" ClientInstanceName="ctxtProductAmount">
                                                        </dxe:ASPxLabel>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </li>
                                <li class="lblChargesGSTforGross">
                                    <div class="lblHolder">
                                        <table>
                                            <tbody>
                                                <tr>
                                                    <td>GST</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxLabel ID="lblChargesGSTforGross" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesGSTforGross">
                                                        </dxe:ASPxLabel>
                                                    </td>
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
                                                    <td>Total Discount</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxLabel ID="txtProductDiscount" runat="server" Text="ASPxLabel" ClientInstanceName="ctxtProductDiscount">
                                                        </dxe:ASPxLabel>
                                                    </td>
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
                                                    <td>Total Charges</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxLabel ID="txtProductTaxAmount" runat="server" Text="ASPxLabel" ClientInstanceName="ctxtProductTaxAmount"></dxe:ASPxLabel>
                                                    </td>
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
                                                    <td>Net Amount Total
                                                                <dxe:ASPxLabel ID="ASPxLabel8" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesTaxableNet"></dxe:ASPxLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>
                                                            <dxe:ASPxLabel ID="txtProductNetAmount" runat="server" Text="ASPxLabel" ClientInstanceName="ctxtProductNetAmount"></dxe:ASPxLabel>
                                                        </strong>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </li>
                                <li class="lblChargesGSTforNet">
                                    <div class="lblHolder">
                                        <table>
                                            <tbody>
                                                <tr>
                                                    <td>GST</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxLabel ID="lblChargesGSTforNet" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesGSTforNet">
                                                        </dxe:ASPxLabel>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </li>
                            </ul>
                        </div>
                        <div class="clear">
                        </div>
                        <%--Error Msg--%>

                        <div class="col-md-8" id="ErrorMsgCharges">
                            <div class="lblHolder">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>Status
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Tax Code/Charges Not Defined.
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>

                        </div>

                        <div class="clear">
                        </div>
                        <div class="col-md-12 gridTaxClass" style="">
                            <dxe:ASPxGridView runat="server" KeyFieldName="TaxID" ClientInstanceName="gridTax" ID="gridTax"
                                Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                                Settings-ShowFooter="false" OnCustomCallback="gridTax_CustomCallback" OnBatchUpdate="gridTax_BatchUpdate"
                                OnRowInserting="gridTax_RowInserting" OnRowUpdating="gridTax_RowUpdating" OnRowDeleting="gridTax_RowDeleting"
                                OnDataBinding="gridTax_DataBinding">
                                <Settings VerticalScrollableHeight="300" VerticalScrollBarMode="Auto"></Settings>
                                <SettingsPager Visible="false"></SettingsPager>
                                <Columns>
                                    <dxe:GridViewDataTextColumn FieldName="TaxName" Caption="Tax" VisibleIndex="0" Width="40%" ReadOnly="true">
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="calCulatedOn" Caption="Calculated On" VisibleIndex="0" Width="20%" ReadOnly="true">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn FieldName="Percentage" Caption="Percentage" VisibleIndex="1" Width="20%">
                                        <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.000">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                            <ClientSideEvents LostFocus="PercentageTextChange" />
                                            <ClientSideEvents />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="2" Width="20%">
                                        <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ClientSideEvents LostFocus="QuotationTaxAmountTextChange" GotFocus="QuotationTaxAmountGotFocus" />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                </Columns>
                                <ClientSideEvents EndCallback="OnTaxEndCallback" />
                                <SettingsDataSecurity AllowEdit="true" />
                                <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                    <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                </SettingsEditing>
                            </dxe:ASPxGridView>
                        </div>
                        <div class="col-md-12">
                            <table style="" class="chargesDDownTaxClass">
                                <tr class="chargeGstCstvatClass">
                                    <td style="padding-top: 10px; padding-right: 25px"><span><strong>GST</strong></span></td>
                                    <td style="padding-top: 10px; width: 200px;">
                                        <dxe:ASPxComboBox ID="cmbGstCstVatcharge" ClientInstanceName="ccmbGstCstVatcharge" runat="server" SelectedIndex="0" TabIndex="2"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" TextFormatString="{0}">
                                            <Columns>
                                                <dxe:ListBoxColumn FieldName="Taxes_Name" Caption="Tax Component ID" Width="250" />
                                                <dxe:ListBoxColumn FieldName="TaxCodeName" Caption="Tax Component Name" Width="250" />
                                            </Columns>
                                        </dxe:ASPxComboBox>
                                    </td>
                                    <td style="padding-left: 15px; padding-top: 10px; width: 200px;">
                                        <dxe:ASPxTextBox ID="txtGstCstVatCharge" MaxLength="80" ClientInstanceName="ctxtGstCstVatCharge" TabIndex="3" ReadOnly="true" Text="0.00"
                                            runat="server" Width="100%">
                                            <MaskSettings Mask="&lt;-999999999..999999999g&gt;.&lt;00..99&gt;" />
                                        </dxe:ASPxTextBox>
                                    </td>
                                    <td style="padding-left: 15px; padding-top: 10px">
                                        <input type="button" onclick="recalculateTaxCharge()" class="btn btn-info btn-small RecalculateCharge" value="Recalculate GST" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="clear">
                            <br />
                        </div>
                        <div class="col-sm-3">
                            <div>
                                <dxe:ASPxButton ID="btn_SaveTax" ClientInstanceName="cbtn_SaveTax" runat="server" AccessKey="X" AutoPostBack="False" Text="Ok" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                    <ClientSideEvents Click="function(s, e) {Save_TaxClick();}" />
                                </dxe:ASPxButton>
                                <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtn_tax_cancel" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger">
                                    <ClientSideEvents Click="function(s, e) {cPopup_Taxes.Hide();}" />
                                </dxe:ASPxButton>
                            </div>
                        </div>
                        <div class="col-sm-9">
                            <table class="pull-right">
                                <tr>
                                    <td style="padding-right: 30px; width: 114px"><strong>Total Charges</strong></td>
                                    <td>
                                        <div>
                                            <dxe:ASPxTextBox ID="txtQuoteTaxTotalAmt" runat="server" Width="100%" ClientInstanceName="ctxtQuoteTaxTotalAmt" Text="0.0000" HorizontalAlign="Right" Font-Size="12px" ClientEnabled="false">
                                                <MaskSettings Mask="<-9999999..9999999g>.<00..99>" AllowMouseWheel="false" />
                                                <%-- <MaskSettings Mask="<0..999999999999g>.<0..99g>" />--%>
                                            </dxe:ASPxTextBox>
                                        </div>

                                    </td>
                                    <td style="padding-right: 30px; padding-left: 5px; width: 114px"><strong>Total Amount</strong></td>
                                    <td>
                                        <div>
                                            <dxe:ASPxTextBox ID="txtTotalAmount" runat="server" Width="100%" ClientInstanceName="ctxtTotalAmount" Text="0.00" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                <MaskSettings Mask="<-999999999..99999999999999999999>.<0..99>" AllowMouseWheel="false" />
                                                <%--<MaskSettings Mask="<0..999999999999g>.<0..99g>" />--%>
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="col-sm-2" style="padding-top: 8px;">
                            <span></span>
                        </div>
                        <div class="col-sm-4">
                        </div>
                        <div class="col-sm-2" style="padding-top: 8px;">
                            <span></span>
                        </div>
                        <div class="col-sm-4">
                        </div>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>
    </div>
    <div>
        <dxe:ASPxPopupControl ID="ASPxProductsPopup" runat="server" ClientInstanceName="cProductsPopup"
            Width="900px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
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
    <div>
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
                        <div class="clear">
                            <br />
                        </div>
                        <dxe:ASPxCallbackPanel runat="server" ID="WarehousePanel" ClientInstanceName="cWarehousePanel" OnCallback="WarehousePanel_Callback">
                            <PanelCollection>
                                <dxe:PanelContent runat="server">
                                    <div class="clearfix col-md-12" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;">
                                        <div>
                                            <div class="col-md-3" id="div_Warehouse">
                                                <div>
                                                    Warehouse
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <dxe:ASPxComboBox ID="CmbWarehouseID" EnableIncrementalFiltering="True" ClientInstanceName="cCmbWarehouseID" SelectedIndex="0"
                                                        TextField="WarehouseName" ValueField="WarehouseID" runat="server" Width="100%">
                                                    </dxe:ASPxComboBox>
                                                    <span id="spnCmbWarehouse" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                </div>
                                            </div>
                                            <div class="col-md-3" id="div_Batch">
                                                <div>
                                                    Batch/Lot
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <dxe:ASPxTextBox ID="txtBatchName" runat="server" Width="100%" ClientInstanceName="ctxtBatchName" HorizontalAlign="Left" Font-Size="12px">
                                                    </dxe:ASPxTextBox>
                                                    <span id="spntxtBatch" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                </div>
                                            </div>
                                            <div class="col-md-3" id="div_Manufacture">
                                                <div>
                                                    Manufacture Date
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <dxe:ASPxDateEdit ID="txtStartDate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True" ClientInstanceName="ctxtStartDate" AllowNull="true" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </div>
                                            </div>
                                            <div class="col-md-3" id="div_Expiry">
                                                <div>
                                                    Expiry Date
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <dxe:ASPxDateEdit ID="txtEndDate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True" ClientInstanceName="ctxtEndDate" AllowNull="true" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </div>
                                            </div>
                                            <div class="clear" id="div_Break"></div>
                                            <div class="col-md-3" id="div_Quantity">
                                                <div>
                                                    Quantity
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <dxe:ASPxTextBox ID="txtQuantity" runat="server" ClientInstanceName="ctxtQuantity" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" />
                                                        <%--<ClientSideEvents TextChanged="function(s, e) {SubmitWarehouse();}" />--%>
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                    </dxe:ASPxTextBox>
                                                    <span id="spntxtQuantity" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                </div>
                                            </div>
                                            <div class="col-md-3" id="Altdiv_Quantity">
                                                <div>
                                                    Alt. Quantity
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <dxe:ASPxTextBox ID="txtAltQuantity" runat="server" ClientInstanceName="ctxtAltQuantity" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" />

                                                    </dxe:ASPxTextBox>

                                                </div>
                                            </div>
                                            <div class="col-md-3" id="div_Serial">
                                                <div>
                                                    Serial No
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <dxe:ASPxTextBox ID="txtserialID" runat="server" Width="100%" ClientInstanceName="ctxtserialID" HorizontalAlign="Left" Font-Size="12px" MaxLength="49">
                                                        <ClientSideEvents LostFocus="SubmitWarehouse" />
                                                    </dxe:ASPxTextBox>
                                                    <span id="spntxtserialID" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                </div>
                                            </div>
                                            <div class="col-md-3" id="div_Upload">
                                                <div class="col-md-3">
                                                    <div>
                                                    </div>
                                                    <%-- <dxe:ASPxButton ID="btnUploadSerial" ClientInstanceName="cbtnUploadSerial" Width="50px" runat="server" AutoPostBack="False" Text="Upload Serial" CssClass="btn btn-primary">
                                                        <ClientSideEvents Click="UploadSerial" />
                                                    </dxe:ASPxButton>--%>
                                                </div>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="col-md-3">
                                                <div>
                                                </div>
                                                <div class="Left_Content" style="padding-top: 14px">
                                                    <dxe:ASPxButton ID="btnWarehouse" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary">
                                                        <ClientSideEvents Click="SubmitWarehouse" />
                                                    </dxe:ASPxButton>

                                                    <dxe:ASPxButton ID="btnClear" ClientInstanceName="cbtnClear" Width="50px" runat="server" AutoPostBack="False" Text="Clear" CssClass="btn btn-primary">
                                                        <ClientSideEvents Click="ClearWarehouse" />
                                                    </dxe:ASPxButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix">
                                        <dxe:ASPxGridView ID="GrdWarehouse" ClientInstanceName="cGrdWarehouse" runat="server" KeyFieldName="SrlNo" AutoGenerateColumns="False"
                                            Width="100%" SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200"
                                            SettingsBehavior-AllowSort="false" OnDataBinding="GrdWarehouse_DataBinding">
                                            <%--OnCustomCallback="GrdWarehouse_CustomCallback" --%>
                                            <Columns>
                                                <dxe:GridViewDataTextColumn Caption="Warehouse Name" FieldName="WarehouseName"
                                                    VisibleIndex="0">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn Caption="Batch/Lot Number" FieldName="BatchNo"
                                                    VisibleIndex="1">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn Caption="Mfg Date" FieldName="ViewMfgDate"
                                                    VisibleIndex="2">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn Caption="Expiry Date" FieldName="ViewExpiryDate"
                                                    VisibleIndex="3">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="SalesQuantity"
                                                    VisibleIndex="4">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn Caption="Serial Number" FieldName="SerialNo"
                                                    VisibleIndex="5">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="6" Width="120px">
                                                    <DataItemTemplate>
                                                        <a href="javascript:void(0);" onclick="fn_Edit('<%# Container.KeyValue %>')" title="Delete" style='<%#Eval("IsOutStatus")%>'>
                                                            <img src="../../../assests/images/Edit.png" /></a>
                                                        &nbsp;
                                        <a href="javascript:void(0);" onclick="fn_Delete('<%# Container.KeyValue %>')" title="Delete" style='<%#Eval("IsOutStatus")%>'>
                                            <img src="/assests/images/crs.png" /></a>
                                                        <a class="anchorclass" style='<%#Eval("IsOutStatusMsg")%>'>Already used</a>
                                                    </DataItemTemplate>
                                                </dxe:GridViewDataTextColumn>
                                            </Columns>
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
                                </dxe:PanelContent>
                            </PanelCollection>
                            <ClientSideEvents EndCallback="WarehousePanelEndCall" />
                        </dxe:ASPxCallbackPanel>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>
    </div>
    <div>
        <dxe:ASPxPopupControl ID="popup_taggingGrid" runat="server" ClientInstanceName="cpopup_taggingGrid"
            HeaderText="Select Purchase Order" PopupHorizontalAlign="WindowCenter"
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
                        <dxe:ASPxGridView ID="taggingGrid" ClientInstanceName="ctaggingGrid" runat="server" KeyFieldName="PurchaseOrder_Id"
                            Width="100%" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords"
                            Settings-ShowFooter="false" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible"
                            OnCustomCallback="taggingGrid_CustomCallback" OnDataBinding="taggingGrid_DataBinding">
                            <SettingsBehavior AllowDragDrop="False"></SettingsBehavior>
                            <SettingsPager Visible="false"></SettingsPager>
                            <Columns>
                                <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="40" Caption=" " VisibleIndex="0" />
                                <dxe:GridViewDataTextColumn FieldName="PurchaseOrder_Number" Caption="Purchase Order Number" Width="150" VisibleIndex="1">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="ComponentDate" Caption="Purchase Order Date" Width="100" VisibleIndex="2">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="CustomerName" Caption="Vendor Name" Width="150" VisibleIndex="3">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="ReferenceName" Caption="Reference" Width="150" VisibleIndex="4">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Tax_Option" Caption="Tax" Width="1" VisibleIndex="5">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="RevNo" Caption="Revision No" Width="150" VisibleIndex="6">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="RevDate" Caption="Rev. date" Width="150" VisibleIndex="7">
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <SettingsDataSecurity AllowEdit="true" />
                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                        </dxe:ASPxGridView>
                    </div>
                    <div class="text-center">
                        <dxe:ASPxButton ID="btnTaggingSave" ClientInstanceName="cbtnTaggingSave" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" UseSubmitBehavior="false">
                            <ClientSideEvents Click="function(s, e) {QuotationNumberChanged();}" />
                        </dxe:ASPxButton>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>
    </div>
    <asp:SqlDataSource ID="VendorDataSource" runat="server" />
    <asp:SqlDataSource ID="ProductDataSource" runat="server" />
    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divSubmitButton"
        Modal="True">
    </dxe:ASPxLoadingPanel>
    <div>
        <asp:HiddenField runat="server" ID="hdnBranchID" />
        <asp:HiddenField runat="server" ID="hdnCustomerId" />
        <asp:HiddenField runat="server" ID="hdfIsDelete" />
        <asp:HiddenField runat="server" ID="hfProduct_Json" />
        <asp:HiddenField runat="server" ID="hdnPageStatus" />
        <asp:HiddenField runat="server" ID="IsUdfpresent" />
        <asp:HiddenField runat="server" ID="Keyval_internalId" />
        <%--Subhra 14-03-2019--%>
        <asp:HiddenField runat="server" ID="Keyval_Id" />
        <%--End Subhra 14-03-2019--%>
        <asp:HiddenField runat="server" ID="hdnRefreshType" />
        <asp:HiddenField runat="server" ID="hdnJsonProductTax" />
        <asp:HiddenField runat="server" ID="hdnJsonProductStock" />
        <asp:HiddenField runat="server" ID="hdnJsonTempStock" />
        <asp:HiddenField runat="server" ID="hdndefaultWarehouse" />
        <asp:HiddenField runat="server" ID="IsBarcodeActive" />

        <asp:HiddenField runat="server" ID="hdfProductID" />
        <asp:HiddenField runat="server" ID="hdfWarehousetype" />
        <asp:HiddenField runat="server" ID="hdfProductSrlNo" />
        <asp:HiddenField runat="server" ID="hdnProductQuantity" />
        <asp:HiddenField runat="server" ID="hdfUOM" />
        <asp:HiddenField runat="server" ID="hdfServiceURL" />
        <asp:HiddenField runat="server" ID="hdfBranch" />
        <asp:HiddenField runat="server" ID="hdfIsRateExists" />
        <asp:HiddenField runat="server" ID="hdfIsBarcodeGenerator" />

        <asp:HiddenField runat="server" ID="setCurrentProdCode" />
        <asp:HiddenField runat="server" ID="HdSerialNo" />
        <asp:HiddenField runat="server" ID="HdProdGrossAmt" />
        <asp:HiddenField runat="server" ID="HdProdNetAmt" />
        <asp:HiddenField runat="server" ID="HdChargeProdAmt" />
        <asp:HiddenField runat="server" ID="HdChargeProdNetAmt" />

        <asp:HiddenField runat="server" ID="HDItemLevelTaxDetails" />
        <asp:HiddenField runat="server" ID="HDHSNCodewisetaxSchemid" />
        <asp:HiddenField runat="server" ID="HDBranchWiseStateTax" />
        <asp:HiddenField runat="server" ID="HDStateCodeWiseStateIDTax" />
        <asp:HiddenField ID="hdfEWayBillMendatory" runat="server" />
        <asp:HiddenField ID="hdnProjectSelectInEntryModule" runat="server" />
    </div>
    <!--Vendor Modal -->
    <div class="modal fade" id="CustModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Vendor Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search By Vendor Name or Unique Id" />
                    <div id="CustomerTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Unique Id</th>
                                <th>Vendor Name</th>
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
    <!--Vendor Modal -->

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
                                <th>GST Rate</th>
                                <th>Class</th>
                                <th>Brand</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <% if (rightsProd.CanAdd)
                       { %>
                    <button type="button" class="btn btn-success" onclick="fn_PopOpen();">
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

    <!--Product Stock Modal -->
    <dxe:ASPxPopupControl ID="PopupWarehouse" runat="server" ClientInstanceName="cPopupWarehouse"
        Width="850px" HeaderText="Stock Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ClientSideEvents Closing="function(s, e) {
	closeStockPopup(s, e);}" />
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div id="content-1" class="pull-right wrapHolder content horizontal-images" style="width: 100%; margin-right: 0px;">
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
                                                <asp:Label ID="lblProductName" runat="server"></asp:Label>
                                            </td>
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
                                                <asp:Label ID="lblEnteredAmount" runat="server"></asp:Label>
                                                <asp:Label ID="lblEnteredUOM" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </li>
                    </ul>
                </div>
                <div class="clear"></div>
                <div id="StockHeader">
                    <div class="clearfix  modal-body" style="padding: 8px 0 8px 0; margin-bottom: 15px; margin-top: 15px; border-radius: 4px; border: 1px solid #ccc;">
                        <div class="col-md-12">
                            <div class="clearfix  row">
                                <div class="col-md-3" id="_div_Warehouse">
                                    <div>
                                        Warehouse
                                    </div>
                                    <div class="Left_Content" style="">
                                        <asp:DropDownList ID="ddlWarehouse" runat="server" Width="100%">
                                        </asp:DropDownList>
                                        <span id="rfvWarehouse" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                    </div>
                                </div>
                                <div class="col-md-3" id="_div_Batch">
                                    <div>
                                        Batch
                                    </div>
                                    <div class="Left_Content" style="">
                                       <%-- onchange="BatchNoUniqueCheck()"--%>
                                        <input type="text" id="txtBatch" placeholder="Batch" onchange="BatchNoUniqueCheck()" />
                                        <span id="rfvBatch" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                    </div>
                                </div>
                                <div class="col-md-3" id="_div_Manufacture">
                                    <div>
                                        Manufacture Date
                                    </div>
                                    <div class="Left_Content" style="">
                                        <%--<input type="text" id="txtMfgDate" placeholder="Mfg Date" />--%>
                                        <dxe:ASPxDateEdit ID="txtMfgDate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True"
                                            ClientInstanceName="ctxtMfgDate" AllowNull="true" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                        </dxe:ASPxDateEdit>
                                    </div>
                                </div>
                                <div class="col-md-3" id="_div_Expiry">
                                    <div>
                                        Expiry Date
                                    </div>
                                    <div class="Left_Content" style="">
                                        <%-- <input type="text" id="txtExprieyDate" placeholder="Expiry Date" />--%>
                                        <dxe:ASPxDateEdit ID="txtExprieyDate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True"
                                            ClientInstanceName="ctxtExprieyDate" AllowNull="true" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                        </dxe:ASPxDateEdit>
                                    </div>
                                </div>
                                <div class="clear" id="_div_Break"></div>
                                <div class="col-md-3" id="_div_Rate">
                                    <div>
                                        Rate
                                    </div>
                                    <div class="Left_Content" style="">
                                        <dxe:ASPxTextBox ID="txtRate" runat="server" ClientInstanceName="ctxtRate" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                            <ValidationSettings Display="None"></ValidationSettings>
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>
                                <div class="col-md-3" id="_div_Quantity">
                                    <div>
                                        Quantity
                                    </div>
                                    <div class="Left_Content" style="">
                                        <dxe:ASPxTextBox ID="txtQty" runat="server" ClientInstanceName="ctxtQty" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" AllowMouseWheel="false" />
                                            <ClientSideEvents TextChanged="function(s, e) {SubmitWarehouse();}" LostFocus="ConversionFromUomToAltQuantity" />
                                            <ValidationSettings Display="None"></ValidationSettings>
                                        </dxe:ASPxTextBox>
                                        <span id="rfvQuantity" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                    </div>
                                </div>
                                <div class="col-md-3" id="_Altdiv_Quantity">
                                    <div>
                                        Alt. Quantity
                                    </div>
                                    <div class="Left_Content" style="">
                                        <dxe:ASPxTextBox ID="txtAltQty" runat="server" ClientInstanceName="ctxtAltQty" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" />
                                            <ClientSideEvents LostFocus="AltQuantityLostFocus" />
                                        </dxe:ASPxTextBox>

                                    </div>
                                    <div style="margin-bottom: 2px;">
                                        Alt. UOM
                                    </div>
                                    <dxe:ASPxComboBox ID="ASPxComboBox1" ClientEnabled="false" ClientInstanceName="ccmbAltUOM" runat="server" SelectedIndex="0" DataSourceID="AltUomSelect"
                                        ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" ValueField="UOM_ID" TextField="UOM_Name">
                                    </dxe:ASPxComboBox>
                                </div>
                                <div class="col-md-3" id="_div_Serial">
                                    <div>
                                        Serial No
                                    </div>
                                    <div class="Left_Content" style="">
                                        <input type="text" id="txtSerial" placeholder="Serial No" onkeyup="Serialkeydown(event)" />
                                        <span id="rfvSerial" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                    </div>
                                </div>
                                <div class="col-md-3" id="_div_Upload">
                                    <div class="col-md-3">
                                        <div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div>
                                    </div>
                                    <div class="Left_Content">
                                        <input type="button" id="btnAddStock" onclick="SaveStockPC()" value="Add" class="btn btn-primary" />
                                        <input id="btnSecondUOM" type="button" onclick="AlternateUOMDetails('PC')" value="Alt Unit Details" class="btn btn-success" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="showData" class="gridStatic">
                </div>
                <div class="clearfix  row">
                    <div class="col-md-3">
                        <div>
                        </div>
                        <div class="Left_Content" style="padding-top: 14px">
                            <%-- <input type="button" onclick="FullnFinalSave()" value="Ok" class="btn btn-primary" />--%>

                            <dxe:ASPxButton ID="ASPxButton4" ClientInstanceName="cbtn_CommitClose" runat="server" AutoPostBack="False" Text="OK&#818;" CssClass="btn btn-success" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {FullnFinalSave();}" />
                            </dxe:ASPxButton>

                        </div>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>
    <!--Product Stock Modal -->

    <%--Rev 2.0 Subhra 11-03-2019--%>
    <div>
        <asp:HiddenField runat="server" ID="hdnConvertionOverideVisible" />
        <asp:HiddenField runat="server" ID="hdnShowUOMConversionInEntry" />
    </div>
    <%--End of Rev 2.0 Subhra 11-03-2019--%>

    <dxe:ASPxCallback ID="DeletePanel" runat="server" OnCallback="DeletePanel_Callback" ClientInstanceName="cDeletePanel">
    </dxe:ASPxCallback>



    <dxe:ASPxPopupControl ID="DirectAddCustPopup" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="AspxDirectAddCustPopup" Height="650px"
        Width="1020px" HeaderText="Add New Vendor" Modal="true" AllowResize="false" ResizingMode="Postponed">

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>



    <dxe:ASPxPopupControl ID="SecondUOMpopup" runat="server" ClientInstanceName="cSecondUOM" ShowCloseButton="false"
        Width="850px" HeaderText="Second UOM Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="clearfix boxStyle">
                    <div class="col-md-3">
                        <label>Length (inch)</label>
                        <dxe:ASPxTextBox runat="server" ID="txtLength" ClientInstanceName="ctxtLength">
                            <ClientSideEvents LostFocus="SizeLostFocus" />

                            <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />

                        </dxe:ASPxTextBox>
                    </div>
                    <div class="col-md-3">
                        <label>Width (inch)</label>
                        <dxe:ASPxTextBox runat="server" ID="txtWidth" ClientInstanceName="ctxtWidth">
                            <ClientSideEvents LostFocus="SizeLostFocus" />
                            <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />

                        </dxe:ASPxTextBox>
                    </div>
                    <div class="col-md-3">
                        <label>Total (Sq. Feet)</label>
                        <dxe:ASPxTextBox runat="server" ID="txtTotal" ClientEnabled="false" ClientInstanceName="ctxtTotal">
                            <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />
                        </dxe:ASPxTextBox>
                    </div>
                    <div class="col-md-3 padTop23 pdLeft0">
                        <label></label>
                        <button type="button" onclick="AddSecondUOMDetails('PC');" class="btn btn-primary">Add</button>
                    </div>
                </div>
                <div class="clearfix"></div>
                <div class="col-md-12">
                    <table id="dataTbl" class="display nowrap" style="width: 100%">
                        <thead>
                            <tr>
                                <th class="hide">GUID</th>
                                <th class="hide">WarehouseID</th>
                                <th class="hide">ProductId</th>
                                <th>SL</th>
                                <th>Branch</th>
                                <th>Warehouse</th>
                                <th>Size</th>
                                <th>Total</th>
                                <th>Action</th>
                            </tr>
                        </thead>
                        <tbody id="tbodySecondUOM">
                        </tbody>
                    </table>
                </div>
                <div class="col-md-12 text-right pdTop15">
                    <button class="btn btn-success" type="button" onclick="SavePOESecondUOMDetails();">OK</button>
                    <button class="btn btn-danger hide" type="button" onclick="return cSecondUOM.Hide();">Cancel</button>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>

    </dxe:ASPxPopupControl>

    <asp:HiddenField ID="hdnAlternateProdId" runat="server" />


    <asp:HiddenField runat="server" ID="hdnChallanType" />
    <dxe:ASPxPopupControl ID="PosView" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cPosView" Height="650px"
        Width="1200px" HeaderText="Product" Modal="true">

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>


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
                                                 <%--Rev Mantis Issue 24429--%>
                                                <%--<input type="text" id="UOMQuantity" style="text-align: right;" maxlength="18"  class="allownumericwithdecimal" />--%>
                                                <input type="text" id="UOMQuantity" style="text-align: right;" maxlength="18"  class="allownumericwithdecimal" onchange="CalcBaseRate()" />
                                                <%--End of Rev Mantis Issue 24429--%>
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
                                
                                
                                <%--Mantis Issue 24429--%>
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
                                <%--End of Mantis Issue 24429--%>
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
                                            <%--  <input type="text" id="AltUOMQuantity" style="text-align:right;"  maxlength="18" class="allownumericwithdecimal"/> --%>
                                            <dxe:ASPxTextBox ID="AltUOMQuantity" runat="server" ClientInstanceName="cAltUOMQuantity" DisplayFormatString="0.0000" MaskSettings-Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right">
                                                 <%--Mantis Issue 24429--%>
                                                <ClientSideEvents TextChanged="function(s,e) { CalcBaseQty();}" />
                                                <%--End of Mantis Issue 24429--%>
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                 <%--Mantis Issue 24429--%>
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
                                            <%--Mantis Issue 24429--%>
                                           <%-- <label class="checkbox-inline mlableWh">
                                                <asp:CheckBox ID="chkUpdateRow" Checked="false" runat="server" ></asp:CheckBox>
                                                <span style="margin: 0px 0; display: block">
                                                    <dxe:ASPxLabel ID="ASPxLabel18" runat="server" Text="Update Row">
                                                    </dxe:ASPxLabel>
                                                </span>
                                            </label>--%>
                                             <label class="checkbox-inline mlableWh">
                                                <input type="checkbox" id="chkUpdateRow"  />
                                                <span style="margin: 0px 0; display: block">
                                                    <dxe:ASPxLabel ID="ASPxLabel19" runat="server" Text="Update Row">
                                                    </dxe:ASPxLabel>
                                                </span>
                                            </label>
                                            <%--Mantis Issue 24429--%>
                                        </div>
                                    </div>

                                    
                                </td>
                                <%--End of Mantis Issue 24429--%>
                                <td style="padding-top: 14px;">
                                    <dxe:ASPxButton ID="btnMUltiUOM" ClientInstanceName="cbtnMUltiUOM" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary">
                                        <ClientSideEvents Click="function(s, e) {SaveMultiUOM();}" />
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
                                <%--Mantis Issue 24429--%>
                                <dxe:GridViewDataTextColumn Caption="MultiUOMSR No" 
                                    VisibleIndex="0" HeaderStyle-HorizontalAlign="left" Width="0">
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24429--%>
                                <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity"
                                    VisibleIndex="0" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="UOM" FieldName="UOM"
                                    VisibleIndex="1">
                                </dxe:GridViewDataTextColumn>
                                 <%--Mantis Issue 24429--%>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="BaseRate"
                                    VisibleIndex="2" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24429--%>



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
                                <%--Mantis Issue 24429--%>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="AltRate"
                                    VisibleIndex="6" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Update Row" FieldName="UpdateRow"
                                    VisibleIndex="6" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24429--%>

                                <dxe:GridViewDataTextColumn VisibleIndex="10" Width="80px" Caption="Action">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" onclick="Delete_MultiUom('<%# Container.KeyValue %>','<%#Eval("SrlNo") %>','<%#Eval("DetailsId") %>')" title="Delete">
                                            <img src="/assests/images/crs.png" /></a>
                                        <%--Mantis Issue 24429 --%>

                                           <a href="javascript:void(0);" onclick="Edit_MultiUom('<%# Container.KeyValue %>','<%#Eval("MultiUOMSR") %>')" title="Edit">
                                            <img src="/assests/images/Edit.png" /></a>
                                          <%--End of Mantis Issue 24429 --%>
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
                            <dxe:ASPxButton ID="ASPxButton7" ClientInstanceName="cbtnfinalUomSave" Width="50px" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                                <ClientSideEvents Click="function(s, e) {FinalMultiUOM();}" />
                            </dxe:ASPxButton>
                        </div>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>

    <dxe:ASPxCallbackPanel runat="server" ID="callback_InlineRemarks" ClientInstanceName="ccallback_InlineRemarks" OnCallback="callback_InlineRemarks_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
                <dxe:ASPxPopupControl ID="Popup_InlineRemarks" runat="server" ClientInstanceName="cPopup_InlineRemarks"
                    Width="900px" HeaderText="Additional Description" PopupHorizontalAlign="WindowCenter"
                    BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                    Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                    ContentStyle-CssClass="pad">
                    <%--<ClientSideEvents Closing="function(s, e) {
	                   closeRemarks(s, e);}" />--%>
                    <ContentStyle VerticalAlign="Top" CssClass="pad">
                    </ContentStyle>
                    <ContentCollection>


                        <dxe:PopupControlContentControl runat="server">
                            <div>
                                <asp:Label ID="lblInlineRemarks" runat="server"></asp:Label>

                                <asp:TextBox ID="txtInlineRemarks" runat="server" TabIndex="16" Width="100%" TextMode="MultiLine" Rows="5" Columns="10" Height="50px" MaxLength="5000"></asp:TextBox>
                            </div>


                            <div class="clearfix">
                                <br />
                                <div style="align-content: center">

                                    <dxe:ASPxButton ID="btnSaveInlineRemarks" ClientInstanceName="cbtnSaveInlineRemarks" Width="50px" runat="server" AutoPostBack="false" Text="OK" CssClass="btn btn-primary" UseSubmitBehavior="false">

                                        <ClientSideEvents Click="function(s, e) {FinalRemarks();}" />
                                    </dxe:ASPxButton>

                                </div>
                            </div>
                        </dxe:PopupControlContentControl>



                    </ContentCollection>
                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                </dxe:ASPxPopupControl>
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="callback_InlineRemarks_EndCall" />
    </dxe:ASPxCallbackPanel>

    <asp:SqlDataSource ID="UomSelect" runat="server"
        SelectCommand="select UOM_ID,UOM_Name from Master_UOM "></asp:SqlDataSource>

    <asp:SqlDataSource ID="AltUomSelect" runat="server"
        SelectCommand="select UOM_ID,UOM_Name from Master_UOM "></asp:SqlDataSource>

    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>

    <asp:HiddenField ID="hddnuomFactor" runat="server" />
    <asp:HiddenField ID="hddnMultiUOMSelection" runat="server" />
    <asp:HiddenField ID="hdnQuantitySL" runat="server" />
    <asp:HiddenField ID="hdnEntityType" runat="server" />
    <asp:HiddenField runat="server" ID="hdnQty" />
    <asp:HiddenField ID="hdnProjectMandatory" runat="server" />
    <asp:HiddenField ID="hdnTaggedQuantity" runat="server" />
    <asp:HiddenField ID="hdnADDEditMode" runat="server" />
    <asp:HiddenField ID="hdnBackdateddate" runat="server" />
    <asp:HiddenField ID="hdnTagDateForbackdated" runat="server" />
    <asp:HiddenField ID="HdnBackDatedEntryPurchaseGRN" runat="server" />
    <asp:HiddenField ID="hdnForBranchTaggingPurchase" runat="server" />
    <%--Rev Mantis Issue 24061--%>
    <asp:HiddenField runat="server" ID="hdnPurchaseOrderItemNegative" />
    <%--End of Rev Mantis Issue 24061--%>
</asp:Content>
