<%--=======================================================Revision History=====================================================    
    1.0   Pallab    V2.0.38   08-05-2023      25871: Add GRN (Inter State Stock Transfer) module design modification
=========================================================End Revision History===================================================--%>

<%@ Page Title="Stock Transfer" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="PurchaseChallan_OD.aspx.cs" Inherits="ERP.OMS.Management.Activities.PurchaseChallan_OD" %>

<%@ Register Src="~/OMS/Management/Activities/UserControls/BillingShippingControl.ascx" TagPrefix="ucBS" TagName="BillingShippingControl" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/TermsConditionsControl.ascx" TagPrefix="uc2" TagName="TermsConditionsControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Tax%20Details/Js/TaxDetailsItemlevel.js" type="text/javascript"></script>
    <%--Header Section Start--%>
    <script>
        function PerformCallToGridBind() {
            grid.PerformCallback('BindGridOnQuotation' + '~' + '@');
            cProductsPopup.Hide();

            document.getElementById("ddl_numberingScheme").disabled = true;

            //#### added by : Samrat Roy for Transporter & Billing/Shipping Doc Tagging #############
            var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();
            if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
        callTransporterControl(quote_Id[0], 'PO');
    }

    if (quote_Id.length > 0) {
        BSDocTagging(quote_Id[0], 'PO');
    }
            //#### END : Samrat Roy for Billing/Shipping Doc Tagging : END #############

            //#### added by Sayan Dutta for TC Control #############
    if ($("#btn_TermsCondition").is(":visible")) {
        callTCControl(quote_Id[0], 'PO');
    }
            //#### End : added by Sayan Dutta for TC Control : End #############

    return false;
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
                    url: "PurchaseChallan_OD.aspx/GetRate",
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
    </script>
    <script src="JS/PurchaseChallan_OD.js"></script>
    <link href="CSS/PurchaseChallanOD.css" rel="stylesheet" />

    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 1;
        }

        /*#grid {
            max-width: 98% !important;
        }*/
        #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_PLSales , #dt_SaleInvoiceDue , #dtPostingDate , #dt_PartyDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_PLSales_B-1 , #dt_SaleInvoiceDue_B-1 , #dtPostingDate_B-1 ,
        #dt_PartyDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
        #dt_PLSales_B-1 #dt_PLSales_B-1Img , #dt_SaleInvoiceDue_B-1 #dt_SaleInvoiceDue_B-1Img , #dtPostingDate_B-1 #dtPostingDate_B-1Img ,
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

    </style>
    <%--Rev end 1.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">
                <asp:Label ID="lblHeading" runat="server" Text="Add GRN (Inter State Stock Transfer)"></asp:Label>
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
                    <ul style="display: none;">
                        <li>
                            <div class="lblHolder">
                                <table>
                                    <tr>
                                        <td>Selected Unit</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label13" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                        <li>
                            <div class="lblHolder">
                                <table>

                                    <tr>
                                        <td>Selected Product</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblProduct" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                        <li>
                            <div class="lblHolder">
                                <table>
                                    <tr>
                                        <td>Stock Quantity</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblStkQty" runat="server" Text="0.00"></asp:Label>
                                            <asp:Label ID="lblStkUOM" runat="server" Text=" "></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            <div id="ApprovalCross" runat="server" class="crossBtn"><a href=""><i class="fa fa-times"></i></a></div>
            <div id="divcross" runat="server" class="crossBtn"><a href="PurchaseChallanList_OD.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
        <div class=" form_main row">
        <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
            <TabPages>
                <dxe:TabPage Name="General" Text="General">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                            <div class="row">
                                <div>
                                    <div class="col-md-2 lblmTop8">
                                        <dxe:ASPxLabel ID="lbl_Inventory" runat="server" Text="Inventory Item?">
                                        </dxe:ASPxLabel>
                                        <asp:DropDownList ID="ddlInventory" CssClass="backSelect" runat="server" Width="100%" onchange="ddlInventory_OnChange()">
                                            <asp:ListItem Text="Both" Value="B" />
                                            <asp:ListItem Text="Inventory Item" Value="Y" />
                                            <%-- <asp:ListItem Text="Non-Inventory Item" Value="N" />--%>
                                            <asp:ListItem Text="Capital Goods" Value="C" />
                                        </asp:DropDownList>
                                    </div>
                                    <%--Rev 1.0: "simple-select" class add --%>
                                    <div class="col-md-2 lblmTop8 simple-select" runat="server" id="divNumberingScheme">
                                        <dxe:ASPxLabel ID="lbl_NumberingScheme" Width="160px" runat="server" Text="Numbering Scheme">
                                        </dxe:ASPxLabel>
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
                                        <dxe:ASPxDateEdit ID="dt_PLQuote" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cPLQuoteDate" Width="100%">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                            <ClientSideEvents DateChanged="function(s, e) { GetIndentREquiNo(e)}" />
                                        </dxe:ASPxDateEdit>
                                        <%--Rev 1.0--%>
                                        <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                        <%--Rev end 1.0--%>
                                    </div>
                                    <%--Rev 1.0: "simple-select" class add --%>
                                    <div class="col-md-2 lblmTop8 simple-select">
                                        <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="To Unit">
                                        </dxe:ASPxLabel>
                                        <span style="color: red;">*</span>
                                        <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%" Enabled="false"
                                            DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID">
                                        </asp:DropDownList>
                                    </div>
                                    <%--Rev 1.0: "simple-select" class add --%>
                                    <div class="col-md-2 lblmTop8 simple-select">
                                        <dxe:ASPxLabel ID="lbl_ToBranch" runat="server" Text="From Unit">
                                        </dxe:ASPxLabel>
                                        <span style="color: red;">*</span>
                                        <asp:DropDownList ID="ddl_FromBranch" runat="server" Width="100%"
                                            DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" onchange="CmbBranch_ValueChange()">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-2 lblmTop8">
                                        <dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Vendor">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxGridLookup ID="lookup_Customer" runat="server" ClientInstanceName="gridLookup" DataSourceID="dsCustomer"
                                            KeyFieldName="cnt_internalid" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False">
                                            <Columns>
                                                <dxe:GridViewDataColumn FieldName="Name" Visible="true" VisibleIndex="1" Caption="Name" Width="150" Settings-AutoFilterCondition="Contains">
                                                </dxe:GridViewDataColumn>
                                                <dxe:GridViewDataColumn FieldName="shortname" Visible="true" VisibleIndex="0" Caption="Short Name" Width="150" Settings-AutoFilterCondition="Contains" />
                                                <dxe:GridViewDataColumn FieldName="cnt_internalid" Visible="false" VisibleIndex="2" Settings-AllowAutoFilter="False" Width="150">
                                                    <Settings AllowAutoFilter="False"></Settings>
                                                </dxe:GridViewDataColumn>
                                            </Columns>
                                            <GridViewProperties Settings-VerticalScrollBarMode="Auto">
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
                                                <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>
                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                            </GridViewProperties>
                                            <ClientSideEvents TextChanged="function(s, e) { GetContactPerson(e)}" />
                                            <ClearButton DisplayMode="Auto">
                                            </ClearButton>
                                        </dxe:ASPxGridLookup>
                                        <span id="MandatorysCustomer" class="customerno pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                    </div>
                                    <div class="col-md-2 lblmTop8">
                                        <dxe:ASPxLabel ID="lbl_ContactPerson" runat="server" Text="Contact Person">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxComboBox ID="cmbContactPerson" runat="server" OnCallback="cmbContactPerson_Callback" Width="100%"
                                            ClientInstanceName="cContactPerson" Font-Size="12px" ClientSideEvents-EndCallback="cmbContactPersonEndCall">
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div class="col-md-2 lblmTop8" style="display: none">
                                        <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Salesman/Agents">
                                        </dxe:ASPxLabel>
                                        <asp:DropDownList ID="ddl_SalesAgent" runat="server" Width="100%">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2 lblmTop8">
                                        <dxe:ASPxLabel ID="ASPxLabel9" runat="server" Text="Party Invoice No.">
                                        </dxe:ASPxLabel>
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
                                        </dxe:ASPxDateEdit>
                                        <%--Rev 1.0--%>
                                        <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                        <%--Rev end 1.0--%>
                                    </div>
                                    <div class="col-md-2 lblmTop8">
                                        <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Select Stock Transfer No.">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cQuotationComponentPanel" OnCallback="ComponentQuotation_Callback">
                                            <PanelCollection>
                                                <dxe:PanelContent runat="server">
                                                    <dxe:ASPxGridLookup ID="lookup_quotation" SelectionMode="Multiple" runat="server" ClientInstanceName="gridquotationLookup"
                                                        OnDataBinding="lookup_quotation_DataBinding" KeyFieldName="PurchaseOrder_Id" Width="100%"
                                                        TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                                        <Columns>
                                                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                                            <dxe:GridViewDataColumn FieldName="PurchaseOrder_Number" Visible="true" VisibleIndex="1" Caption="Document No." Width="150" Settings-AutoFilterCondition="Contains" />
                                                            <dxe:GridViewDataColumn FieldName="ComponentDate" Visible="true" VisibleIndex="2" Caption="Posting Date" Width="150" Settings-AutoFilterCondition="Contains" />
                                                            <dxe:GridViewDataColumn FieldName="CustomerName" Visible="true" VisibleIndex="3" Caption="Customer Name" Width="150" Settings-AutoFilterCondition="Contains">
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataColumn>
                                                            <dxe:GridViewDataColumn FieldName="ReferenceName" Visible="true" VisibleIndex="4" Caption="Reference" Width="150" Settings-AutoFilterCondition="Contains">
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataColumn>
                                                            <dxe:GridViewDataColumn FieldName="BranchName" Visible="true" VisibleIndex="5" Caption="Unit" Width="150" Settings-AutoFilterCondition="Contains">
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataColumn>
                                                        </Columns>
                                                        <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                            <Templates>
                                                                <StatusBar>
                                                                    <table class="OptionsTable" style="float: right">
                                                                        <tr>
                                                                            <td>
                                                                                <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </StatusBar>
                                                            </Templates>
                                                            <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                                            <SettingsPager Mode="ShowAllRecords">
                                                            </SettingsPager>
                                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                        </GridViewProperties>
                                                        <ClientSideEvents ValueChanged="function(s, e) { QuotationNumberChanged();}" />
                                                    </dxe:ASPxGridLookup>
                                                </dxe:PanelContent>
                                            </PanelCollection>
                                            <ClientSideEvents EndCallback="componentEndCallBack" />
                                        </dxe:ASPxCallbackPanel>
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
                                                    <dxe:ASPxGridView runat="server" KeyFieldName="ComponentDetailsID" ClientInstanceName="cgridproducts" ID="grid_Products"
                                                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                                                        SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridProducts_CustomCallback"
                                                        Settings-ShowFooter="false" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
                                                        <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                                                        <SettingsPager Visible="false"></SettingsPager>
                                                        <Columns>
                                                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="40" Caption=" " VisibleIndex="0" />
                                                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="SrlNo" Width="40" ReadOnly="true" Caption="Sl. No.">
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="ComponentNumber" Width="120" ReadOnly="true" Caption="Number">
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
                                                    </dxe:ASPxGridView>
                                                    <div class="text-center">
                                                        <asp:Button ID="Button2" runat="server" Text="OK" CssClass="btn btn-primary  mLeft mTop" OnClientClick="return PerformCallToGridBind();" />
                                                    </div>
                                                </dxe:PopupControlContentControl>
                                            </ContentCollection>
                                            <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
                                            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                                        </dxe:ASPxPopupControl>
                                    </div>
                                    <div class="col-md-2 lblmTop8">
                                        <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Stock Transfer Date">
                                        </dxe:ASPxLabel>
                                        <div style="width: 100%; height: 23px; border: 1px solid #e6e6e6;">
                                            <asp:Label ID="lbl_MultipleDate" runat="server" Text="Multiple Select Quotation Dates" Style="display: none"></asp:Label>
                                            <dxe:ASPxTextBox ID="dt_Quotation" runat="server" Width="100%" ClientEnabled="false" ClientInstanceName="cPLQADate">
                                            </dxe:ASPxTextBox>
                                        </div>
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
                                        <asp:DropDownList ID="ddl_Currency" runat="server" Width="100%" DataValueField="Currency_ID"
                                            DataTextField="Currency_AlphaCode" onchange="ddl_Currency_Rate_Change()">
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
                                        <dxe:ASPxComboBox ID="ddl_AmountAre" runat="server" SelectedIndex="0" ClientIDMode="Static" ClientInstanceName="cddl_AmountAre" Width="100%">
                                            <ClientSideEvents LostFocus="function(s, e) { SetFocusonDemand(e)}" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div class="col-md-2 lblmTop8  hide" style="margin-bottom: 5px">
                                        <dxe:ASPxLabel ID="lblVatGstCst" runat="server" Text="Select GST">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxComboBox ID="ddl_VatGstCst" runat="server" ClientInstanceName="cddlVatGstCst" Width="100%">
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <div>
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <dxe:ASPxGridView runat="server" KeyFieldName="OrderDetails_Id" ClientInstanceName="grid" ID="grid"
                                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                                        Settings-ShowFooter="false" SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="160"
                                        OnBatchUpdate="grid_BatchUpdate" OnCellEditorInitialize="grid_CellEditorInitialize"
                                        OnCustomCallback="grid_CustomCallback" OnDataBinding="grid_DataBinding"
                                        OnRowInserting="Grid_RowInserting" OnRowUpdating="Grid_RowUpdating"
                                        OnRowDeleting="Grid_RowDeleting" OnHtmlRowPrepared="grid_HtmlRowPrepared">
                                        <SettingsPager Visible="false"></SettingsPager>
                                        <Columns>
                                            <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="30" VisibleIndex="0" Caption="">
                                                <CustomButtons>
                                                    <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                                    </dxe:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                            </dxe:GridViewCommandColumn>
                                            <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl" ReadOnly="true" VisibleIndex="1" Width="20px">
                                                <PropertiesTextEdit>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Document No" FieldName="PoNumber" ReadOnly="true" Width="130" VisibleIndex="2">
                                                <PropertiesTextEdit>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product" VisibleIndex="3" Width="150">
                                                <PropertiesButtonEdit>
                                                    <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" GotFocus="ProductsGotFocusFromID" LostFocus="ProductsGotFocus" />
                                                    <Buttons>
                                                        <dxe:EditButton Text="..." Width="20px">
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                </PropertiesButtonEdit>
                                            </dxe:GridViewDataButtonEditColumn>
                                            <dxe:GridViewDataTextColumn FieldName="gvColProduct" Caption="hidden Field Id" VisibleIndex="22" ReadOnly="True" Width="0"
                                                EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="gvColDiscription" Caption="Description" VisibleIndex="4" Width="18%">
                                                <PropertiesTextEdit>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="gvColQuantity" Caption="Quantity" VisibleIndex="5" Width="6%" HeaderStyle-HorizontalAlign="Right">
                                                <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                    <ClientSideEvents LostFocus="QuantityTextChange" GotFocus="QuantityProductsGotFocus" />
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                                    <Style HorizontalAlign="Right">
                                                            </Style>
                                                </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="gvColUOM" Caption="UOM" VisibleIndex="6" Width="4%" ReadOnly="true">
                                                <PropertiesTextEdit>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewCommandColumn Width="4%" VisibleIndex="7" Caption="Stock">
                                                <CustomButtons>
                                                    <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomWarehouse" Image-Url="/assests/images/warehouse.png">
                                                    </dxe:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                            </dxe:GridViewCommandColumn>
                                            <dxe:GridViewDataTextColumn FieldName="gvColStockUOM" Caption="Stk UOM" VisibleIndex="14" Width="0">
                                                <PropertiesTextEdit>
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="gvColStockPurchasePrice" Caption="Price" VisibleIndex="8" Width="7%" HeaderStyle-HorizontalAlign="Right">
                                                <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="SalePriceTextChange" GotFocus="SalePriceTextFocus" />
                                                    <Style HorizontalAlign="Right">
                                                            </Style>
                                                </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataSpinEditColumn FieldName="gvColDiscount" Caption="Disc(%)" VisibleIndex="9" Width="4%" HeaderStyle-HorizontalAlign="Right">
                                                <PropertiesSpinEdit MinValue="0" MaxValue="100" AllowMouseWheel="false" DisplayFormatString="0.00" MaxLength="6" Style-HorizontalAlign="Right">
                                                    <SpinButtons ShowIncrementButtons="false"></SpinButtons>
                                                    <ClientSideEvents LostFocus="DiscountTextChange" GotFocus="DiscountTextFocus" />
                                                    <Style HorizontalAlign="Right">
                                                            </Style>
                                                </PropertiesSpinEdit>
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataSpinEditColumn>
                                            <dxe:GridViewDataTextColumn FieldName="gvColAmount" Caption="Amount" VisibleIndex="10" Width="9%" HeaderStyle-HorizontalAlign="Right">
                                                <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                <PropertiesTextEdit>
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="AmountTextChange" GotFocus="AmountTextFocus" />
                                                    <Style HorizontalAlign="Right">
                                                            </Style>
                                                </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataButtonEditColumn FieldName="gvColTaxAmount" Caption="Charges" VisibleIndex="11" Width="9%" HeaderStyle-HorizontalAlign="Right">
                                                <PropertiesButtonEdit Style-HorizontalAlign="Right">
                                                    <ClientSideEvents ButtonClick="taxAmtButnClick" GotFocus="taxAmtButnClick1" KeyDown="TaxAmountKeyDown" />
                                                    <Buttons>
                                                        <dxe:EditButton Text="..." Width="20px">
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                </PropertiesButtonEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataButtonEditColumn>
                                            <dxe:GridViewDataTextColumn FieldName="gvColTotalAmountINR" Caption="Net Amount" VisibleIndex="12" Width="9%" HeaderStyle-HorizontalAlign="Right">
                                                <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                                    <Style HorizontalAlign="Right">
                                                            </Style>
                                                </PropertiesTextEdit>
                                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="5%" VisibleIndex="13" Caption="Add New">
                                                <CustomButtons>
                                                    <dxe:GridViewCommandColumnCustomButton ID="CustomAddNewRow" Image-Url="/assests/images/add.png" Text=" ">
                                                    </dxe:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                            </dxe:GridViewCommandColumn>
                                            <dxe:GridViewDataTextColumn FieldName="gvColStockQty" Caption="Stk Qty" VisibleIndex="15" Width="0">
                                                <PropertiesTextEdit>
                                                    <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                                                </PropertiesTextEdit>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="TotalQty" Caption="Total Qty" VisibleIndex="16" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="BalanceQty" Caption="Balance Qty" VisibleIndex="17" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="IsComponentProduct" Caption="IsComponentProduct" VisibleIndex="19" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="IsLinkedProduct" Caption="IsLinkedProduct" VisibleIndex="20" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="ComponentID" Caption="ComponentID" VisibleIndex="21" ReadOnly="True" Width="0">
                                            </dxe:GridViewDataTextColumn>
                                        </Columns>
                                        <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" BatchEditStartEditing="gridFocusedRowChanged" />
                                        <SettingsDataSecurity AllowEdit="true" />
                                        <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                            <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                        </SettingsEditing>
                                    </dxe:ASPxGridView>
                                </div>
                                <div style="clear: both;">
                                    <br />
                                    <div style="display: none;">
                                        <dxe:ASPxLabel ID="txt_Charges" runat="server" Text="0.00" ClientInstanceName="ctxt_Charges" />
                                        <dxe:ASPxLabel ID="txt_cInvValue" runat="server" Text="0.00" ClientInstanceName="cInvValue" />
                                    </div>
                                </div>
                                <div class="content reverse horizontal-images clearfix" style="width: 100%; margin-right: 0; padding: 8px; height: auto; border-top: 1px solid #ccc; border-bottom: 1px solid #ccc; border-radius: 0;">
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
                                                                <dxe:ASPxLabel ID="bnrLblTaxAmt" runat="server" Text="Tax & Charges" ClientInstanceName="cbnrLblTaxAmt" />
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
                                                                <dxe:ASPxLabel ID="bnrLblOtherTaxAmt" runat="server" Text="Other Charges" ClientInstanceName="cbnrLblOtherTaxAmt" />
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
                                <div class="col-md-12" style="padding-top: 15px;">
                                    <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                    <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="S&#818;ave & New" CssClass="btn btn-success" meta:resourcekey="btnSaveRecordsResource1">
                                        <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                                    </dxe:ASPxButton>
                                    <dxe:ASPxButton ID="btn_SaveRecordsExit" ClientInstanceName="cbtn_SaveRecordsExit" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-success" meta:resourcekey="btnSaveRecordsResource1">
                                        <ClientSideEvents Click="function(s, e) {SaveExit_ButtonClick();}" />
                                    </dxe:ASPxButton>
                                    <dxe:ASPxButton ID="btn_SaveRecordsUDF" ClientInstanceName="cbtn_SaveRecordsUDF" runat="server" AutoPostBack="False" Text="UDF" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                        <ClientSideEvents Click="function(s, e) {OpenUdf();}" />
                                    </dxe:ASPxButton>
                                    <dxe:ASPxButton ID="btn_SaveTaxRecords" ClientInstanceName="cbtn_SaveTaxRecords" runat="server" AutoPostBack="False" Text="T&#818;ax & Charges" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                        <ClientSideEvents Click="function(s, e) {Save_TaxesClick();}" />
                                    </dxe:ASPxButton>
                                    <uc1:VehicleDetailsControl runat="server" ID="VehicleDetailsControl" />
                                    <asp:HiddenField ID="hfControlData" runat="server" />
                                    <uc2:TermsConditionsControl runat="server" ID="TermsConditionsControl" />
                                    <asp:HiddenField runat="server" ID="hfTermsConditionData" />
                                    <asp:HiddenField runat="server" ID="hfTermsConditionDocType" Value="PC" />
                                    <asp:Label ID="lbl_IsTagged" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                    <asp:CheckBox ID="chkmail" runat="server" Text="Send Mail" />
                                </div>
                            </div>
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>
                <dxe:TabPage Name="[B]illing/Shipping" Text="Our Billing/Shipping">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                            <ucBS:BillingShippingControl runat="server" ID="BillingShippingControl" />
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
    <%-- Vendor List --%>
    <asp:SqlDataSource runat="server" ID="dsCustomer" 
        SelectCommand="prc_Purchasechallan_Details" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter Type="String" Name="Action" DefaultValue="PopulateVendorsDetail" />
        </SelectParameters>
    </asp:SqlDataSource>
    <%-- Vendor List --%>

    <%-- Product List --%>
    <dxe:ASPxPopupControl ID="ProductpopUp" runat="server" ClientInstanceName="cProductpopUp"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
        Width="700" HeaderText="Select Product" AllowResize="true" ResizingMode="Postponed" Modal="true">
        <HeaderTemplate>
            <span>Select Product</span>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <label><strong>Search By Product Name</strong></label>
                <span style="color: red;">[Press ESC key to Cancel]</span>
                <dxe:ASPxGridLookup ID="productLookUp" runat="server" DataSourceID="ProductDataSource" ClientInstanceName="cproductLookUp"
                    KeyFieldName="Products_ID" Width="870" TextFormatString="{0}" MultiTextSeparator=", " ClientSideEvents-TextChanged="ProductSelected" ClientSideEvents-KeyDown="ProductlookUpKeyDown">
                    <Columns>
                        <dxe:GridViewDataColumn FieldName="Products_Description" Caption="Name" Width="360">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="Products_Name" Caption="Name" Width="0">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="IsInventory" Caption="Inventory" Width="60">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="HSNSAC" Caption="HSN/SAC" Width="80">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="ClassCode" Caption="Class" Width="230">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="BrandName" Caption="Brand" Width="130">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                    </Columns>
                    <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                    </GridViewProperties>
                </dxe:ASPxGridLookup>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
    </dxe:ASPxPopupControl>
    <asp:SqlDataSource runat="server" ID="ProductDataSource" 
        SelectCommand="prc_Purchasechallan_Details" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter Type="String" Name="Action" DefaultValue="ProductDetails" />
            <asp:SessionParameter Name="campany_Id" SessionField="LastCompany" Type="String" />
            <asp:SessionParameter Type="String" Name="FinYear" SessionField="LastFinYear" />
            <asp:ControlParameter DefaultValue="Y" Name="InventoryType" ControlID="ctl00$ContentPlaceHolder1$ASPxPageControl1$ddlInventory" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>
    <%-- Product List --%>


    <%--InlineTax--%>

    <dxe:ASPxPopupControl ID="aspxTaxpopUp" runat="server" ClientInstanceName="caspxTaxpopUp"
        Width="850px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
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

                                    <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="calCulatedOn" ReadOnly="true" Caption="Calculated On">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Percentage" FieldName="TaxField" VisibleIndex="4">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />
                                            <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Amount" Caption="Amount" ReadOnly="true">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <ClientSideEvents LostFocus="taxAmountLostFocus" GotFocus="taxAmountGotFocus" />
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                </Columns>
                                <%--  <SettingsPager Mode="ShowAllRecords"></SettingsPager>--%>
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
                            <div class="pull-left">
                                <asp:Button ID="Button1" runat="server" Text="Ok" TabIndex="5" CssClass="btn btn-primary mTop" OnClientClick="return BatchUpdate();" Width="85px" />
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
            Width="900px" Height="300px" HeaderText="GRN Taxes" PopupHorizontalAlign="WindowCenter"
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
                                <Settings VerticalScrollableHeight="150" VerticalScrollBarMode="Auto"></Settings>
                                <SettingsPager Visible="false"></SettingsPager>
                                <Columns>
                                    <dxe:GridViewDataTextColumn FieldName="TaxName" Caption="Tax" VisibleIndex="0" Width="40%" ReadOnly="true">
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="calCulatedOn" Caption="Calculated On" VisibleIndex="0" Width="20%" ReadOnly="true">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn FieldName="Percentage" Caption="Percentage" VisibleIndex="1" Width="20%">
                                        <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                            <ClientSideEvents LostFocus="PercentageTextChange" />
                                            <ClientSideEvents />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="2" Width="20%">
                                        <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
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
                                            <MaskSettings Mask="<-999999999..999999999>.<0..99>" AllowMouseWheel="false" />
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
                                            <dxe:ASPxTextBox ID="txtQuoteTaxTotalAmt" runat="server" Width="100%" ClientInstanceName="ctxtQuoteTaxTotalAmt" Text="0.00" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                <MaskSettings Mask="<-999999999..99999999999999999999>.<0..99>" AllowMouseWheel="false" />
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

    <%--Warehouse Details--%>

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
                    <div id="content-6" class="pull-right wrapHolder content horizontal-images" style="width: 100%; margin-right: 0px;">
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
                                    Batch/Lot
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
                                <dxe:GridViewDataTextColumn Caption="Batch/Lot Number" FieldName="BatchNo"
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
                                                        <a href="javascript:void(0);" id="ADelete" onclick="fn_DeleteWarehouse('<%# Container.KeyValue %>')" title="Delete">
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
    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <%--Warehouse Details--%>

    <%-- HiddenField List --%>

    <asp:HiddenField ID="hdfIsDelete" runat="server" />
    <asp:HiddenField ID="hdnPageStatus" runat="server" />
    <asp:HiddenField ID="hdfProductID" runat="server" />
    <asp:HiddenField ID="hdfProductType" runat="server" />
    <asp:HiddenField ID="hdfProductSerialID" runat="server" />
    <asp:HiddenField ID="hdnRefreshType" runat="server" />
    <asp:HiddenField ID="hdnCustomerId" runat="server" />
    <asp:HiddenField ID="hiddenOnGridBind" runat="server" Value="0" />
    <asp:HiddenField ID="hdnProductQuantity" runat="server" />
    <asp:HiddenField ID="IsPOTagged" runat="server" />
    <asp:HiddenField ID="hdfLookupCustomer" runat="server" />
    <asp:HiddenField ID="hdfChallanID" runat="server" />

    <asp:HiddenField runat="server" ID="HDItemLevelTaxDetails" />
    <asp:HiddenField runat="server" ID="HDHSNCodewisetaxSchemid" />
    <asp:HiddenField runat="server" ID="HDBranchWiseStateTax" />
    <asp:HiddenField runat="server" ID="HDStateCodeWiseStateIDTax" />

    <asp:HiddenField runat="server" ID="setCurrentProdCode" />
    <asp:HiddenField runat="server" ID="HdSerialNo" />
    <asp:HiddenField runat="server" ID="HdProdGrossAmt" />
    <asp:HiddenField runat="server" ID="HdProdNetAmt" />
    <asp:HiddenField ID="HdChargeProdAmt" runat="server" />
    <asp:HiddenField ID="HdChargeProdNetAmt" runat="server" />

    <%-- HiddenField List --%>

    <dxe:ASPxCallbackPanel runat="server" ID="taxUpdatePanel" ClientInstanceName="ctaxUpdatePanel" OnCallback="taxUpdatePanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="ctaxUpdatePanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divSubmitButton"
        Modal="True">
    </dxe:ASPxLoadingPanel>
</asp:Content>
