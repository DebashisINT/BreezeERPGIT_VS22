<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                17-04-2023        2.0.37           Pallab              25836: Quotation Price Comparison module design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="NormalPurchaseQuotePriceCompare.aspx.cs" Inherits="ERP.OMS.Management.Activities.NormalPurchaseQuotePriceCompare" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchMultiPopup.js"></script>

       <script type="text/javascript">


           function MaterialsEndCallBack(s, e) {

               if (cMaterialsCallbackpanel.cpCloseMaterialsCallback == "CloseMaterialsCallback") {
                   MaterialsquotationLookup.ShowDropDown();
                   gridquotationLookup.HideDropDown();
               }

           }

           function QuotationEndCallBack(s, e) {
               if (cQuotationComponentPanel.cpCloseQupotation == "CloseQupotation") {
                   gridquotationLookup.HideDropDown();
               }

           }
           function SaveExit_ButtonClick() {
               //LoadingPanel.Show();

               flag = true;
               if ($("#hdncWiseVendorId").val() == "" || $("#hdncWiseVendorId").val() == null || $("#hdncWiseVendorId").val() == undefined) {
                   jAlert("Please select vendor.");
                   flag = false;
               }
               else {
                   var MaterialsTag_Id = MaterialsquotationLookup.gridView.GetSelectedKeysOnPage();

                   debugger;
                   if (MaterialsTag_Id.length > 0) {

                       var Materials_Id = "";
                       for (var i = 0; i < MaterialsTag_Id.length; i++) {
                           if (Materials_Id == "") {
                               Materials_Id = MaterialsTag_Id[i];
                           }
                           else {
                               Materials_Id += ',' + MaterialsTag_Id[i];
                           }
                       }


                       $("#hdnProductId").val(Materials_Id)
                   }



                   var quotetag_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();

                   debugger;
                   if (quotetag_Id.length > 0) {

                       var quote_Id = "";

                       for (var i = 0; i < quotetag_Id.length; i++) {
                           if (quote_Id == "") {
                               quote_Id = quotetag_Id[i];
                           }
                           else {
                               quote_Id += ',' + quotetag_Id[i];
                           }
                       }

                       $("#hdnQuoteId").val(quote_Id)
                   }

                   cShowgridPurchaseQuotationPrice.PerformCallback($("#hdncWiseVendorId").val() + '~' + $("#hdnProductId").val() + '~' + $("#hdnQuoteId").val());
                   //$("#hdncWiseVendorId").val("");
                   $("#hdnProductId").val("");
                   $("#hdnQuoteId").val("");
               }
           }
           var ProdArr = new Array();
           $(document).ready(function () {
               var ProdObj = new Object();
               ProdObj.Name = "VendorSource";
               ProdObj.ArraySource = ProdArr;
               arrMultiPopup.push(ProdObj);
           })


           $(document).ready(function () {
               $('#ProdModel').on('shown.bs.modal', function () {
                   $('#txtProdSearch').focus();
               })

           })


           function SetSelectedValues(Id, Name, ArrName) {

               if (ArrName == 'VendorSource') {
                   var key = Id;
                   if (key != null && key != '') {
                       $('#ProdModel').modal('hide');
                       ctxtVendorName.SetText(Name);
                       GetObjectID('hdncWiseVendorId').value = key;
                       if (cFromDate.GetText() != "" && cFromDate.GetText() != null && cFromDate.GetText() != "01-01-0100" && cToDate.GetText() != "" && cToDate.GetText() != null && cToDate.GetText() != "01-01-0100" && key != "") {
                           cQuotationComponentPanel.PerformCallback();
                           //cQuotationComponentPanel.gridView.Refresh();
                       }

                   }
                   else {
                       ctxtVendorName.SetText('');
                       GetObjectID('hdncWiseVendorId').value = '';
                   }
               }


           }

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
               //  OtherDetails.ClassID = hdnClassId.value;

               if (e.code == "Enter" || e.code == "NumpadEnter") {

                   var HeaderCaption = [];
                   HeaderCaption.push("Vendor Name");
                   HeaderCaption.push("Unique Id");

                   if ($("#txtProdSearch").val() != "") {
                       callonServerM("Services/Master.asmx/GetVendorWithQuotationPrice", OtherDetails, "ProductTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "VendorSource");
                   }
               }
               else if (e.code == "ArrowDown") {
                   if ($("input[dPropertyIndex=0]"))
                       $("input[dPropertyIndex=0]").focus();
               }
           }


           function MaterialsCloseGridQuotationLookup() {

               MaterialsquotationLookup.ConfirmCurrentSelection();
               //MaterialsquotationLookup.HideDropDown();
               MaterialsquotationLookup.Focus();

               var quotetag_Id = MaterialsquotationLookup.gridView.GetSelectedKeysOnPage();

               debugger;
               if (quotetag_Id.length > 0) {

                   var quote_Id = "";
                   // otherDets.quote_Id = quote_Id;
                   for (var i = 0; i < quotetag_Id.length; i++) {
                       if (quote_Id == "") {
                           quote_Id = quotetag_Id[i];
                       }
                       else {
                           quote_Id += ',' + quotetag_Id[i];
                       }
                   }
                   //if (quote_Id.length>0)
                   //{
                   //    $("#hdnProductId").val(quote_Id);
                   //}

               }

           }


           function CloseGridQuotationLookup() {

               gridquotationLookup.ConfirmCurrentSelection();
               gridquotationLookup.HideDropDown();
               gridquotationLookup.Focus();

               var quotetag_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();

               debugger;
               if (quotetag_Id.length > 0) {

                   var quote_Id = "";
                   // otherDets.quote_Id = quote_Id;
                   for (var i = 0; i < quotetag_Id.length; i++) {
                       if (quote_Id == "") {
                           quote_Id = quotetag_Id[i];
                       }
                       else {
                           quote_Id += ',' + quotetag_Id[i];
                       }
                   }

               }

               if (quotetag_Id.length > 0) {
                   $("#hdnQuoteId").val(quote_Id)
                   cMaterialsCallbackpanel.PerformCallback(quote_Id);
               }

           }



           function GetCheckBoxValue(value) {
               //var value = s.GetChecked();
               if (value == true) {
                   ctxtVendorName.SetText("");
                   $("#hdncWiseVendorId").val("AllVEND");
                   ctxtVendorName.SetEnabled(false);
                   if (cFromDate.GetText() != "" && cFromDate.GetText() != null && cFromDate.GetText() != "01-01-0100" && cToDate.GetText() != "" && cToDate.GetText() != null && cToDate.GetText() != "01-01-0100") {
                       cQuotationComponentPanel.PerformCallback();
                       //cQuotationComponentPanel.gridView.Refresh();
                   }
               } else {

                   $("#hdncWiseVendorId").val("");
                   $("#hdnQuoteId").val("");
                   $("#hdnProductId").val("");
                   ctxtVendorName.SetEnabled(true);
                   gridquotationLookup.SetEnabled(true);
                   ctxtVendorName.SetText("");
                   gridquotationLookup.Clear();
                   gridquotationLookup.gridView.UnselectRows();
                   MaterialsquotationLookup.Clear();
                   MaterialsquotationLookup.gridView.UnselectRows();
                   cQuotationComponentPanel.PerformCallback();
                   cMaterialsCallbackpanel.PerformCallback();
               }
           }


           function SetLostFocusonDemand(e) {
               if (cFromDate.GetText() != "" && cFromDate.GetText() != null && cFromDate.GetText() != "01-01-0100" && cToDate.GetText() != "" && cToDate.GetText() != null && cToDate.GetText() != "01-01-0100" && $("#hdncWiseVendorId").val() != "") {

                   // MaterialsquotationLookup.ShowDropDown();
                   cMaterialsCallbackpanel.PerformCallback();
               }
           }

           function GetQuotationCheckBoxValue(value) {
               //var value = s.GetChecked();
               if (value == true) {
                   $("#hdnQuoteId").val("AllQuotation");
                   gridquotationLookup.Clear();
                   gridquotationLookup.SetEnabled(false);
                   if (cFromDate.GetText() != "" && cFromDate.GetText() != null && cFromDate.GetText() != "01-01-0100" && cToDate.GetText() != "" && cToDate.GetText() != null && cToDate.GetText() != "01-01-0100" && $("#hdncWiseVendorId").val() != "") {
                       cMaterialsCallbackpanel.PerformCallback();
                       //cMaterialsCallbackpanel.gridView.Refresh();
                   }
               } else {
                   $("#hdnQuoteId").val("");
                   $("#hdnProductId").val("");
                   MaterialsquotationLookup.Clear();
                   MaterialsquotationLookup.gridView.UnselectRows();
                   gridquotationLookup.SetEnabled(true);
                   cMaterialsCallbackpanel.PerformCallback();

               }
           }

    </script>
    <style>
        .padTop {
            padding-top:5px;
        }
    </style>

    <%--<style>
        /*Rev 1.0*/

        select
        {
            height: 30px !important;
            border-radius: 4px !important;
            /*-webkit-appearance: none;
            position: relative;
            z-index: 1;*/
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , .dxeTextBox_PlasticBlue
        {
            height: 30px;
            border-radius: 4px;
        }

        .dxeButtonEditButton_PlasticBlue
        {
            background: #094e8c !important;
            border-radius: 4px !important;
            padding: 0 4px !important;
        }

        .calendar-icon {
            position: absolute;
            bottom: 7px;
            right: 20px;
            z-index: 0;
            cursor: pointer;
        }

        #FormDate , #toDate , #ASPxDateEditFrom , #ASPxDateEditTo , #FFromDate , #FToDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #FormDate_B-1 , #toDate_B-1 , #ASPxDateEditFrom_B-1 , #ASPxDateEditTo_B-1 , #FFromDate_B-1 , #FToDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #ASPxDateEditFrom_B-1 #ASPxDateEditFrom_B-1Img , #ASPxDateEditTo_B-1 #ASPxDateEditTo_B-1Img ,
        #FFromDate_B-1 #FFromDate_B-1Img , #FToDate_B-1 #FToDate_B-1Img
        {
            display: none;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        select.btn
        {
            padding-right: 10px !important;
        }

        .panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }

        .dxpLite_PlasticBlue .dxp-current
        {
            background-color: #1b5ea4;
            padding: 3px 5px;
            border-radius: 2px;
        }

        #accordion {
            margin-bottom: 20px;
            margin-top: 10px;
        }

        .dxgvHeader_PlasticBlue {
    background: #1b5ea4 !important;
    color: #fff !important;
}
        #ShowGrid
        {
            margin-top: 10px;
        }

        .pt-25{
                padding-top: 25px !important;
        }

        .styled-checkbox {
        position: absolute;
        opacity: 0;
        z-index: 1;
    }

        .styled-checkbox + label {
            position: relative;
            /*cursor: pointer;*/
            padding: 0;
            margin-bottom: 0 !important;
        }

            .styled-checkbox + label:before {
                content: "";
                margin-right: 6px;
                display: inline-block;
                vertical-align: text-top;
                width: 16px;
                height: 16px;
                /*background: #d7d7d7;*/
                margin-top: 2px;
                border-radius: 2px;
                border: 1px solid #c5c5c5;
            }

        .styled-checkbox:hover + label:before {
            background: #094e8c;
        }


        .styled-checkbox:checked + label:before {
            background: #094e8c;
        }

        .styled-checkbox:disabled + label {
            color: #b8b8b8;
            cursor: auto;
        }

            .styled-checkbox:disabled + label:before {
                box-shadow: none;
                background: #ddd;
            }

        .styled-checkbox:checked + label:after {
            content: "";
            position: absolute;
            left: 3px;
            top: 9px;
            background: white;
            width: 2px;
            height: 2px;
            box-shadow: 2px 0 0 white, 4px 0 0 white, 4px -2px 0 white, 4px -4px 0 white, 4px -6px 0 white, 4px -8px 0 white;
            transform: rotate(45deg);
        }

        .dxgvEditFormDisplayRow_PlasticBlue td.dxgv, .dxgvDataRow_PlasticBlue td.dxgv, .dxgvDataRowAlt_PlasticBlue td.dxgv, .dxgvSelectedRow_PlasticBlue td.dxgv, .dxgvFocusedRow_PlasticBlue td.dxgv
        {
            padding: 6px 6px 6px !important;
        }

        #lookupCardBank_DDD_PW-1
        {
                left: -182px !important;
        }
        .plhead a>i
        {
                top: 9px;
        }

        .clsTo
        {
            display: flex;
    align-items: flex-start;
        }

        .btn-info
        {
                background-color: #1da8d1 !important;
                background-image: none;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .dxeButtonDisabled_PlasticBlue
        {
            background: #b5b5b5 !important;
            border-color: #b5b5b5 !important;
        }

        #ddlValTech
        {
            width: 100% !important;
            margin-bottom: 0 !important;
        }

        .dis-flex
        {
            display: flex;
            align-items: baseline;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridLocationwiseStockStatus ,
        #GrdSalesReturn
        {
            max-width: 98% !important;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer
        {
            padding-top: 15px;
        }

        .mb-10 , .col-md-2{
            margin-bottom: 10px !important;
        }

        /*.btn
        {
            padding: 5px 10px;
        }*/


        /*Rev end 1.0*/

    </style>--%>

    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 0;
        }

        #GrdSalesReturn {
            max-width: 98% !important;
        }
        #FormDate , #toDate , #ASPxDateEditFrom , #ASPxDateEditTo , #FFromDate , #FToDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #FormDate_B-1 , #toDate_B-1 , #ASPxDateEditFrom_B-1 , #ASPxDateEditTo_B-1 , #FFromDate_B-1 , #FToDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #ASPxDateEditFrom_B-1 #ASPxDateEditFrom_B-1Img , #ASPxDateEditTo_B-1 #ASPxDateEditTo_B-1Img ,
        #FFromDate_B-1 #FFromDate_B-1Img , #FToDate_B-1 #FToDate_B-1Img
        {
            display: none;
        }

        select
        {
            -webkit-appearance: auto;
        }

        .calendar-icon
        {
                right: 18px;
        }
    </style>
    <%--Rev end 1.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">Quotation Price Comparison</h3>
            
        </div>
        
    </div>
        <div class="form_main">
        <div class="row">
      <div class="col-md-2">
                <div style="" class="clsTo padTop">
                    <asp:Label ID="lblFromDate" runat="Server" Text="From Date * : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </div>                
                <div>
                    <dxe:ASPxDateEdit ID="FFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cFromDate">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>

                    </dxe:ASPxDateEdit>
                    <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
                </div>
     </div>
   <div class="col-md-2">
                <div style="" class="clsTo padTop">
                    <asp:Label ID="lblToDate" runat="Server" Text="To Date * : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </div>                
                <div>
                    <dxe:ASPxDateEdit ID="FToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cToDate">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>

                    </dxe:ASPxDateEdit>
                    <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
                </div>
     </div>

     <div class="col-md-3">
                <div style="margin-top: 4px" class="clsTo">
                    <div>
                        <table style="width:100%">
                            <tr>
                                <td>Vendor * :</td>
                                
                            </tr>
                        </table>
                    </div>
                </div>
                <div>
                    <table style="width: 100%;">
                        <tr>

                              <td>
                                  <dxe:ASPxButtonEdit ID="txtVendorName" runat="server" ReadOnly="true" ClientInstanceName="ctxtVendorName" Width="100%" TabIndex="5">
                        <Buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>
                        </Buttons>
                        <ClientSideEvents ButtonClick="function(s,e){ProductButnClick();}" KeyDown="function(s,e){Product_KeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>
                            </td>

                            <td><dxe:ASPxCheckBox runat="server" ID="ChkAllVendor" ClientInstanceName="cChkAllVendor" Text="All Vendors" ToolTip="All Vendors">
                                        <ClientSideEvents CheckedChanged="function(s, e) { 
                                                        GetCheckBoxValue(s.GetChecked()); 
                                                    }" />
                            </dxe:ASPxCheckBox></td>
                          
                        </tr>
                    </table>
                  
                </div>
      </div>

               <div class="col-md-4 lblmTop8">
                    <div style="" class="clsTo padTop">
                        <asp:Label runat="server" ID="lblQuotation" Text="Quotation : " ToolTip="Quotation"></asp:Label>
                    </div>
                   <table>
                       <tr>
                           <td>
                               <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cQuotationComponentPanel" OnCallback="ComponentQuotation_Callback">
                                                    <PanelCollection>
                                                        <dxe:PanelContent runat="server">
                                                            <asp:HiddenField runat="server" ID="OldSelectedKeyvalue" />
                                                            <dxe:ASPxGridLookup ID="lookup_quotation" SelectionMode="Multiple" runat="server" TabIndex="7" ClientInstanceName="gridquotationLookup"
                                                                OnDataBinding="lookup_quotation_DataBinding"
                                                                KeyFieldName="PurchaseQuotation_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                                                <Columns>
                                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                                                    <dxe:GridViewDataColumn FieldName="Doc_number" Visible="true" VisibleIndex="1" Caption="Number" Width="180" Settings-AutoFilterCondition="Contains">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                    <dxe:GridViewDataColumn FieldName="Quote_Date" Visible="true" VisibleIndex="2" Caption="Date" Width="100" Settings-AutoFilterCondition="Contains">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                    <dxe:GridViewDataColumn FieldName="CustomerName" Visible="true" VisibleIndex="3" Caption="Customer Name" Width="150" Settings-AutoFilterCondition="Contains">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>

                                                                    <dxe:GridViewDataColumn FieldName="RevNo" Visible="true" VisibleIndex="4" Caption="Revision No." Width="100" Settings-AutoFilterCondition="Contains">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                    <dxe:GridViewDataColumn FieldName="RevDate" Visible="true" VisibleIndex="5" Caption="Revision Date" Width="80" Settings-AutoFilterCondition="Contains">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                 
                                                                    
                                                                </Columns>
                                                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                                    <Templates>
                                                                        <StatusBar>
                                                                            <table class="OptionsTable" style="float: right">
                                                                                <tr>
                                                                                    <td>
                                                                                        <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" UseSubmitBehavior="False" />
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
                                                                <ClientSideEvents  GotFocus="function(s,e){gridquotationLookup.ShowDropDown();  }" />
                                                            </dxe:ASPxGridLookup>
                                                        </dxe:PanelContent>
                                                    </PanelCollection>
                                                   <%-- <ClientSideEvents EndCallback="componentEndCallBack" BeginCallback="BeginComponentCallback" />--%>
                                                     <ClientSideEvents EndCallback="QuotationEndCallBack"/>
                                                </dxe:ASPxCallbackPanel>
                           </td>
                           <td>
                               <dxe:ASPxCheckBox runat="server" ID="chkAllQuotation" ClientInstanceName="cchkAllQuotation" Text="All Quotations" ToolTip="All Quotations">
                                        <ClientSideEvents CheckedChanged="function(s, e) { 
                                                        GetQuotationCheckBoxValue(s.GetChecked()); 
                                                    }" />
                                </dxe:ASPxCheckBox>
                           </td>
                       </tr>
                   </table>
                                                
                  </div>
            <div class="clear"></div>

    <div class="col-md-4 lblmTop8">
     <div style="" class="clsTo">
        <asp:Label runat="server" ID="lblProduct" Text="Materials : " ToolTip="Materials"></asp:Label>

    </div>
                                             <dxe:ASPxCallbackPanel runat="server" ID="MaterialsCallbackpanel" ClientInstanceName="cMaterialsCallbackpanel" OnCallback="cMaterialsCallbackpanel_Callback">
                                                    <PanelCollection>
                                                        <dxe:PanelContent runat="server">
                                                            <asp:HiddenField runat="server" ID="HiddenField1" />
                                                            <dxe:ASPxGridLookup ID="MaterialsLookup" SelectionMode="Multiple" runat="server" TabIndex="7" ClientInstanceName="MaterialsquotationLookup"
                                                                OnDataBinding="lookup_Materials_DataBinding"
                                                                KeyFieldName="sProducts_ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                                                <Columns>
                                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                                                    <dxe:GridViewDataColumn FieldName="ProductCode" Visible="true" VisibleIndex="1" Caption="Product Code" Width="150" Settings-AutoFilterCondition="Contains">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                    <dxe:GridViewDataColumn FieldName="ProductName" Visible="true" VisibleIndex="2" Caption="Product Name" Width="150" Settings-AutoFilterCondition="Contains">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                    <dxe:GridViewDataColumn FieldName="Doc_number" Visible="true" VisibleIndex="3" Caption="Document No." Width="130" Settings-AutoFilterCondition="Contains">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>

                                                                    <dxe:GridViewDataColumn FieldName="CustomerName" Visible="true" VisibleIndex="4" Caption="Customer" Width="150" Settings-AutoFilterCondition="Contains">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                  
                                                                    <dxe:GridViewDataColumn FieldName="RevNo" Visible="true" VisibleIndex="6" Caption="Revision No." Width="150" Settings-AutoFilterCondition="Contains">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                              
                                                                </Columns>
                                                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                                    <Templates>
                                                                        <StatusBar>
                                                                            <table class="OptionsTable" style="float: right">
                                                                                <tr>
                                                                                    <td>
                                                                                        <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="MaterialsCloseGridQuotationLookup" UseSubmitBehavior="False" />
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
                                                                <ClientSideEvents  GotFocus="function(s, e) { SetLostFocusonDemand(e)}" />
                                                            </dxe:ASPxGridLookup>
                                                        </dxe:PanelContent>
                                                    </PanelCollection>
                                                  <ClientSideEvents EndCallback="MaterialsEndCallBack"/>
                                                </dxe:ASPxCallbackPanel>
                                            </div>

        <div class="col-md-2" style="padding: 20px 15px;">
                    <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtn_SaveRecords_p" runat="server" AutoPostBack="False" Text="Submit" CssClass="btn btn-success" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {SaveExit_ButtonClick();}" />
                     </dxe:ASPxButton>


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
         <div class="clear"></div>
        </div>
          

        <table class="TableMain100">
            <tr>
                <td colspan="2">
                    <div>
                        <dxe:ASPxGridView ID="ShowgridPurchaseQuotationPrice" runat="server" ClientInstanceName="cShowgridPurchaseQuotationPrice" 
                            Width="100%" Settings-HorizontalScrollBarMode="Auto" KeyFieldName="SEQ" SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="300" 
                            Settings-VerticalScrollBarMode="Auto" Theme="PlasticBlue" OnCustomCallback="ShowgridPurchaseQuotationPrice_CustomCallback" 
                            OnDataBinding="ShowgridPurchaseQuotationPrice_DataBinding"
                            Settings-ShowFilterRow="true" Settings-ShowFilterRowMenu="true" OnHtmlFooterCellPrepared="ShowgridPurchaseQuotationPrice_HtmlFooterCellPrepared">
                            <Columns>

                            </Columns>
                             <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
                            <SettingsBehavior EnableCustomizationWindow="false" AllowSort="False"/>                       
                            <Settings ShowFooter="true"/>
                            <settingssearchpanel visible="False" />
                            <Settings ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" /> 
                            <SettingsContextMenu Enabled="false" />
                            <SettingsPager PageSize="50" NumericButtonCount="5">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>
                        </dxe:ASPxGridView>
                    </div>
                </td>
            </tr>
        </table>


    </div>
    </div>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server"  Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

     <div class="modal fade" id="ProdModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Vendor Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Productkeydown(event)" id="txtProdSearch" width="100%" placeholder="Search By Vendor Name" />
                    <div id="ProductTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>Vendor Name</th>
                                 <th>Unique Id</th>
                             <%--   <th>Address</th>--%>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('VendorSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('VendorSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdncWiseVendorId" runat="server" /> 
      <asp:HiddenField ID="hdnQuoteId" runat="server" /> 
      <asp:HiddenField ID="hdnProductId" runat="server" /> 

    <asp:HiddenField ID="hdnLCStkStatNoBandedColumn" runat="server" />
     <asp:HiddenField ID="hdnLCStkStatNoCaption" runat="server" />
    <asp:HiddenField ID="hdnLCStkStatNoFields" runat="server" />
</asp:Content>
