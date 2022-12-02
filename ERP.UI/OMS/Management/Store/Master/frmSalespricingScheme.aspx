<%@ Page Title="Sales Pricing Scheme" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" EnableEventValidation="false" Inherits="ERP.OMS.Management.Store.Master.management_master_frmSalesPricingScheme" CodeBehind="frmSalesPricingScheme.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        #grid_DXStatus span>a {
            display:none;
        }
        #scrollContent {
            max-height:457px;
            overflow-y:auto;
            
        }
        .mTop15 {
            margin-top:15px;
        }
        .dxgvControl_PlasticBlue td.dxgvBatchEditModifiedCell_PlasticBlue {
           background: #fff !important;
       }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if (ccmbUpdatefor.GetValue() == 'S')
            {
                $('#clsProdList').css({ 'display': 'block' });
            }
            if (ccmbUpdatefor.GetValue() == 'C') {
                $('#clsProdClass').css({ 'display': 'block' });
            }
        });
        function grid_EndCallBack(s, e) {
            if (grid.cpMsg != null) {
                jAlert(grid.cpMsg);
                grid.cpMsg = null;
            }
        }

        function fn_AllowonlyNumeric(s, e) {
            var theEvent = e.htmlEvent || window.event;
            var key = theEvent.keyCode || theEvent.which;
            var keychar = String.fromCharCode(key);
            if (key == 9 || key == 37 || key == 38 || key == 39 || key == 40 || key == 8 || key == 46) { //tab/ Left / Up / Right / Down Arrow, Backspace, Delete keys
                return;
            }
            var regex = /[0-9\b]/;

            if (!regex.test(keychar)) {
                theEvent.returnValue = false;
                if (theEvent.preventDefault)
                    theEvent.preventDefault();
            }
        }

        function updateforIndexChange(s, e) {
            $('#clsProdClass').css({ 'display': 'none' });
            $('#clsProdList').css({ 'display': 'none' });
            gridLookup.SetText('');

            if (s.GetValue() == "C") {
                $('#MandatoryProductClass').css({ 'display': 'none' });
                $('#clsProdClass').css({ 'display': 'block' });
            }
            if (s.GetValue() == "S") { 
                $('#clsProdList').css({ 'display': 'block' });
            }
        }
        function BatchUpdate() {
             var r = confirm("Are You Sure?");
            if(r==true){
            grid.UpdateEdit(); 
            }
            return false;
        }

        function validData()
        {
            var retVal = true;
            $('#MandatoryProductClass').css({ 'display': 'none' });
            $('#MandatorytxtNewValue').css({ 'display': 'none' });
            $('#MandatoryLookUp').css({ 'display': 'none' });
            if (ccmbUpdatefor.GetValue() == 'C') {
                if (ccmbProductClass.GetValue() == null) {
                    $('#MandatoryProductClass').css({ 'display': 'block' });
                    retVal = false;
                }
            }
            if (ccmbUpdatefor.GetValue() == 'S') {
                if (gridLookup.GetText().trim()=='') {
                    $('#MandatoryLookUp').css({ 'display': 'block' });
                    retVal = false;
                }
            }

            if (ctxtNewValue.GetText().trim() == '')
            {
                $('#MandatorytxtNewValue').css({ 'display': 'block' });
                retVal = false;
            }
            
            if(retVal==true){
                  var r = confirm("Are You Sure?");
                   if (r == true) {
                       return true;
                    } else {
                        return false;
                    }
            }

            return retVal;
           
        }

        function CloseGridLookup() {
            gridLookup.ConfirmCurrentSelection();
            gridLookup.HideDropDown();
            gridLookup.Focus();
        }

  function MarkupMinLostfocus(s, e) {
            var markMinVal = (s.GetValue() != null) ? s.GetValue() : "0";
            if(parseFloat(markMinVal)!=0){
            grid.GetEditor('markupPlus').SetValue("0");
             
            //var mrp =  (grid.GetEditor('sProduct_MRP').GetValue() != null) ? grid.GetEditor('sProduct_MRP').GetValue() : "0";
                //       grid.GetEditor('sProduct_MinSalePrice').SetValue( mrp - ((markMinVal * mrp) / 100));
            var mrp = (grid.GetEditor('sProduct_SalePrice').GetValue() != null) ? grid.GetEditor('sProduct_SalePrice').GetValue() : "0";
            grid.GetEditor('sProduct_MinSalePrice').SetValue(mrp - ((markMinVal * mrp) / 100));

            }
        }
  function MarkupPlusLostfocus(s, e) {
            var markPlusVal = (s.GetValue() != null) ? s.GetValue() : "0";
            if(parseFloat(markPlusVal)!=0){
            grid.GetEditor('markupmin').SetValue("0");
             
             //var mrp =  (grid.GetEditor('sProduct_MRP').GetValue() != null) ? grid.GetEditor('sProduct_MRP').GetValue() : "0";
                // grid.GetEditor('sProduct_MinSalePrice').SetValue( parseFloat(mrp) + ((markPlusVal * mrp) / 100));
            var mrp = (grid.GetEditor('sProduct_SalePrice').GetValue() != null) ? grid.GetEditor('sProduct_SalePrice').GetValue() : "0";
            grid.GetEditor('sProduct_MinSalePrice').SetValue(parseFloat(mrp) + ((markPlusVal * mrp) / 100));
            }
        }

 function MRPLostfocus(s, e) {
            var mrp = (s.GetValue() != null) ? s.GetValue() : "0";
            if(parseFloat(mrp)!=0){
                var markMinVal= grid.GetEditor('markupmin').GetValue();
                var markPlusVal= grid.GetEditor('markupPlus').GetValue();
             
                if(parseFloat(markMinVal)!=0){ 
                       grid.GetEditor('sProduct_MinSalePrice').SetValue( mrp - ((markMinVal * mrp) / 100));
                }
                if(parseFloat(markPlusVal)!=0){ 
                       grid.GetEditor('sProduct_MinSalePrice').SetValue( parseFloat(mrp) + ((markPlusVal * mrp) / 100));
                }
                if(parseFloat(markMinVal)==0 && parseFloat(markPlusVal) ==0){
                grid.GetEditor('sProduct_MinSalePrice').SetValue(mrp);
                }
            }else{
            grid.GetEditor('markupmin').SetValue("0");
            grid.GetEditor('markupPlus').SetValue("0");
            grid.GetEditor('sProduct_MinSalePrice').SetValue("0");
            }
            
        }
    </script>
    <style>
        #grid.dxgvControl_PlasticBlue,
        #grid.dxgvControl_PlasticBlue {
            width:100% !important;
        }
        #grid_DXPagerBottom_PSP .dxm-popup {
            width:60px !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Sales Pricing Scheme</h3>
        </div>

    </div>
    <div class="form_main clearfix" style="align-items: center;">
    <div class="row">
        <div class="col-md-3">
            <label>Update Value for</label>
            <div>
                <dxe:ASPxComboBox ID="cmbUpdatefor" ClientInstanceName="ccmbUpdatefor" runat="server" TabIndex="0"
                    ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                    <items>
                                            <dxe:ListEditItem Text="All Products" Value="A" />
                                             <dxe:ListEditItem Text="Selecttive Product" Value="S" />
                                             <dxe:ListEditItem Text="Product Class" Value="C" />
                                        </items>
                    <clientsideevents selectedindexchanged="updateforIndexChange"></clientsideevents>
                </dxe:ASPxComboBox>
            </div>
        </div>

        <div class="col-md-3" id="clsProdClass" style="display:none">
            <label>Product Class</label>
            <div>
                <dxe:ASPxComboBox ID="cmbProductClass" ClientInstanceName="ccmbProductClass" runat="server" TabIndex="0"
                    ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                </dxe:ASPxComboBox>
                <span id="MandatoryProductClass" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute;right:-9px;top:33px;display:none" title="Mandatory"></span>
            </div>
        </div>


        <%--GridLookUp--%>
         <div class="col-md-3" id="clsProdList" style="display:none">
            <label>Product</label>
            <div>
               

                 <dxe:ASPxGridLookup ID="GridLookup" runat="server" SelectionMode="Multiple" DataSourceID="ProductDataSource" ClientInstanceName="gridLookup"
                                                                        KeyFieldName="sProducts_ID" Width="100%" TextFormatString="{0}" MultiTextSeparator=", " >
                                                                        <Columns>
                                                                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption=" "/>
                                                                            <dxe:GridViewDataColumn FieldName="sProducts_Code" Caption="Product Code" Width="150"/>
                                                                            <dxe:GridViewDataColumn FieldName="sProducts_Name" Caption="Product Name" Width="300"/>
                                                                            <dxe:GridViewDataColumn FieldName="ProductClass_Code" Caption="Product Class" Width="300"/>
                                                                        </Columns>
                                                                        <GridViewProperties  Settings-VerticalScrollBarMode="Auto"   >
                                                                             
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
                                                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true"/>
                                                                        </GridViewProperties>
                                                                    </dxe:ASPxGridLookup>

                <span id="MandatoryLookUp" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute;right:-9px;top:35px;display:none" title="Mandatory"></span>

            </div>
        </div>
        <%--GridLookUp--%>


        
        <div class="col-md-3">
            <label>Enter Value for</label>
            <div>
                <dxe:ASPxComboBox ID="CmbValuefor" ClientInstanceName="cCmbValuefor" runat="server" TabIndex="0"
                    ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                    <items>
                                            <%--<dxe:ListEditItem Text="MRP" Value="MRP" />--%>
                                             <dxe:ListEditItem Text="Markup(-)%" Value="MARKUPMIN" />
                                             <dxe:ListEditItem Text="Markup(+)%" Value="MARKPLUS" />
                                             <dxe:ListEditItem Text="Sale Price" Value="SALEP" />
                                             <dxe:ListEditItem Text="Min Sale Price" Value="MSALEP" />
                                                <dxe:ListEditItem Text="Discount UpTo[Sales Manger(%)]" Value="SaleDisc" />
                                        </items> 
                </dxe:ASPxComboBox>
            </div>
        </div>
        <div class="col-md-3">
            <label>&nbsp</label>
            <div>
                <dxe:ASPxTextBox ID="txtNewValue" MaxLength="18" ClientInstanceName="ctxtNewValue" TabIndex="0"  
                    runat="server" Width="100%">
                    <validationsettings display="Dynamic" errordisplaymode="ImageWithTooltip" validationgroup="product" errortextposition="Right" errorimage-tooltip="Mandatory" setfocusonerror="True">
                                                <ErrorImage ToolTip="Mandatory"></ErrorImage>  
                                             </validationsettings>
                    <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                </dxe:ASPxTextBox>
                <span id="MandatorytxtNewValue" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute;right:-9px;top:35px;display:none" title="Mandatory"></span>
            </div>
        </div>
        <div class="clear"></div>
        <div class="col-md-3">
            <asp:Button ID="btnUploadRecord" runat="server" Text="Update & Save" CssClass="btn btn-primary" OnClick="btnUploadRecord_Click" OnClientClick="return validData()" />
        <% if (rights.CanExport)
                                               { %>
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary hide" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" OnChange="if(!AvailableExportOption()){return false;}" AutoPostBack="true"  >
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                                <asp:ListItem Value="2">XLS</asp:ListItem>
                                <asp:ListItem Value="3">RTF</asp:ListItem>
                                <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>
              <% } %>
      </div> 

        <div class="clear"></div>
        <div >
            <div class="col-md-12">
            <dxe:ASPxGridView runat="server" OnBatchUpdate="grid_BatchUpdate" KeyFieldName="sProducts_ID" ClientInstanceName="grid" ID="grid" DataSourceID="ProductDataSource"  SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" 
                Width="100%"   SettingsBehavior-AllowSort="true" SettingsBehavior-AllowDragDrop="false"  
                Settings-ShowFooter="false" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"> 
                <SettingsSearchPanel Visible="True" Delay="5000" />
                <SettingsBehavior AllowDragDrop="False"  ColumnResizeMode="NextColumn"></SettingsBehavior>
                 <Settings   ShowStatusBar="Hidden" ShowFilterRow="true"  ShowFilterRowMenu="true" />
                <columns>
                         <dxe:GridViewDataTextColumn  FieldName="sProducts_ID" ReadOnly="true" Visible="false">  
                              <CellStyle Wrap="False" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle> 
                             <editformsettings visible="False" />
                             <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>

                         <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="sProducts_Code" ReadOnly="True" Caption="Product Code">
                             <Settings AllowAutoFilterTextInputTimer="False" />  
                        </dxe:GridViewDataTextColumn>

                         <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="sProducts_Name" Caption="Product Name" ReadOnly="True"> 
                             <Settings AllowAutoFilterTextInputTimer="False" /> 
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="ProductClass_Code" ReadOnly="True" Caption="Product Class">
                            <Settings AllowAutoFilterTextInputTimer="False" />  
                        </dxe:GridViewDataTextColumn>

                          <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="sProduct_MRP" Caption="MRP" Visible="false"  Settings-ShowFilterRowMenu="False" Settings-AllowHeaderFilter="False" Settings-AllowAutoFilter="False"> 
                                <propertiestextedit displayformatstring="{0:0.00}">
                                     <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                      <ClientSideEvents    />
                                </propertiestextedit> 
                              <Settings AllowAutoFilterTextInputTimer="False" /> 
                        </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="sProduct_SalePrice" Caption="Sale Price(DP)"  Settings-ShowFilterRowMenu="False" Settings-AllowHeaderFilter="False" Settings-AllowAutoFilter="False"> 
                                <propertiestextedit displayformatstring="{0:0.00}">
                                     <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                      <ClientSideEvents  LostFocus="MRPLostfocus"  />
                                </propertiestextedit> 
                        <Settings AllowAutoFilterTextInputTimer="False" /> 
                        </dxe:GridViewDataTextColumn>

                         <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="markupmin"   Caption="Markup(-)%"  Settings-ShowFilterRowMenu="False" Settings-AllowHeaderFilter="False" Settings-AllowAutoFilter="False"> 
                             <propertiestextedit>
                                 <MaskSettings Mask="<0..999>.<0..99>" AllowMouseWheel="false" />
                                 <ClientSideEvents  LostFocus="MarkupMinLostfocus"  />
                             </propertiestextedit> 
                             <CellStyle Wrap="False" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle> 
                             <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>

                         <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="markupPlus"  Caption="Markup(+)%" Settings-ShowFilterRowMenu="False" Settings-AllowHeaderFilter="False" Settings-AllowAutoFilter="False">  
                             <propertiestextedit>
                                 <MaskSettings Mask="<0..999>.<0..99>" AllowMouseWheel="false" />
                                 <ClientSideEvents  LostFocus="MarkupPlusLostfocus"  />
                             </propertiestextedit> 
                             <CellStyle Wrap="False" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>
                             <Settings AllowAutoFilterTextInputTimer="False" /> 
                        </dxe:GridViewDataTextColumn>

                      <%--   <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="sProduct_SalePrice" Caption="Sale Price" Settings-ShowFilterRowMenu="False" Settings-AllowHeaderFilter="False" Settings-AllowAutoFilter="False"> 
                               <propertiestextedit displayformatstring="{0:0.00}">
                                    <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                               </propertiestextedit> 
                        </dxe:GridViewDataTextColumn>--%>
                    
                         <dxe:GridViewDataTextColumn FieldName="sProduct_MinSalePrice" VisibleIndex="7" Caption="Min Sale Price" Settings-ShowFilterRowMenu="False" Settings-AllowHeaderFilter="False" Settings-AllowAutoFilter="False">
                             <propertiestextedit displayformatstring="{0:0.00}">
                                  <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                             </propertiestextedit>
                             <Settings AllowAutoFilterTextInputTimer="False" />
                         </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="DiscountUpTo"  Caption="Discount UpTo(%)" Settings-ShowFilterRowMenu="False" Settings-AllowHeaderFilter="False" Settings-AllowAutoFilter="False">  
                             <propertiestextedit>
                                 <MaskSettings Mask="<0..999>.<0..99>" AllowMouseWheel="false" />
                               <%--  <ClientSideEvents  LostFocus="DiscountUpToLostfocus"  />--%>
                             </propertiestextedit> 
                             <CellStyle Wrap="False" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" /> 
                        </dxe:GridViewDataTextColumn>

                   </columns>
                <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                <settingspager pagesize="10" >
                            <PageSizeItemSettings Visible="true"  ShowAllItem="true" Items="10,50,100,150"/>
                </settingspager>
<%--<SettingsPager Mode="ShowAllRecords"></SettingsPager>--%>

                <settingsediting mode="Batch">
                          <BatchEditSettings EditMode="row"  ShowConfirmOnLosingChanges="false"/>
                    </settingsediting>
                 <ClientSideEvents EndCallback=" grid_EndCallBack " />
            </dxe:ASPxGridView>
        </div>
        </div>
        <div class="col-md-3 mTop15">
            <asp:Button ID="Button1" runat="server" Text="Update & Save" CssClass="btn btn-primary" OnClientClick="return BatchUpdate();" />
        </div>

        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
            </dxe:ASPxGridViewExporter>

    </div>
    </div>


    <asp:SqlDataSource ID="ProductDataSource" runat="server"
        SelectCommand="select h.sProducts_ID,h.sProducts_Code,h.sProducts_Name,(select d.ProductClass_Name from Master_ProductClass d where d.ProductClass_ID=h.ProductClass_Code)   as 'ProductClass_Code'
          ,sProduct_MRP,(select d.prodMarkup_min from tbl_master_productSalesPriceImport d where d.prod_id=h.sProducts_ID ) markupmin,
          (select d.prodMarkup_plus from tbl_master_productSalesPriceImport d where d.prod_id=h.sProducts_ID ) markupPlus,sProduct_SalePrice,sProduct_MinSalePrice, (select d.DiscountUpTo from tbl_master_productSalesPriceImport d where d.prod_id=h.sProducts_ID ) DiscountUpTo from Master_sProducts h"
        UpdateCommand="select null"
        ></asp:SqlDataSource>
</asp:Content>



