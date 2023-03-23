<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                23-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

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

        /*Rev 1.0*/

        /*select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }*/

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

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #txtDOB , #txtAnniversary , #txtcstVdate , #txtLocalVdate ,
        #txtCINVdate , #txtincorporateDate , #txtErpValidFrom , #txtErpValidUpto , #txtESICValidFrom , #txtESICValidUpto , #FormDate , #toDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1 , #txtDOB_B-1 , #txtAnniversary_B-1 , #txtcstVdate_B-1 ,
        #txtLocalVdate_B-1 , #txtCINVdate_B-1 , #txtincorporateDate_B-1 , #txtErpValidFrom_B-1 , #txtErpValidUpto_B-1 , #txtESICValidFrom_B-1 ,
        #txtESICValidUpto_B-1 , #FormDate_B-1 , #toDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img ,
        #txtDOB_B-1 #txtDOB_B-1Img ,
        #txtAnniversary_B-1 #txtAnniversary_B-1Img ,
        #txtcstVdate_B-1 #txtcstVdate_B-1Img ,
        #txtLocalVdate_B-1 #txtLocalVdate_B-1Img , #txtCINVdate_B-1 #txtCINVdate_B-1Img , #txtincorporateDate_B-1 #txtincorporateDate_B-1Img ,
        #txtErpValidFrom_B-1 #txtErpValidFrom_B-1Img , #txtErpValidUpto_B-1 #txtErpValidUpto_B-1Img , #txtESICValidFrom_B-1 #txtESICValidFrom_B-1Img ,
        #txtESICValidUpto_B-1 #txtESICValidUpto_B-1Img , #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img
        {
            display: none;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        /*select.btn
        {
            padding-right: 10px !important;
        }*/

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

        input[type="radio"], input[type="checkbox"]
        {
            margin-right: 5px;
        }
        .dxeCalendarDay_PlasticBlue
        {
                padding: 6px 6px;
        }

        .modal-dialog
        {
            width: 50%;
        }

        .modal-header
        {
            padding: 8px 4px 8px 10px;
            background: #094e8c !important;
        }

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #EmployeeGrid , .TableMain100 #GrdEmployee,
        .TableMain100 #GrdHolidays , #cityGrid
        {
            max-width: 98% !important;
        }

        /*div.dxtcSys > .dxtc-content > div, div.dxtcSys > .dxtc-content > div > div
        {
            width: 95% !important;
        }*/

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

        input + label
        {
            line-height: 1;
                margin-top: 7px;
        }

        .dxtlHeader_PlasticBlue
        {
            background: #094e8c !important;
        }

        .dxeBase_PlasticBlue .dxichCellSys
        {
            padding-top: 2px !important;
        }

        .pBackDiv
        {
            border-radius: 10px;
            box-shadow: 1px 1px 10px #1111112e;
        }
        .HeaderStyle th
        {
            padding: 5px;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer
        {
            padding-top: 15px;
        }

        .pt-2
        {
            padding-top: 5px;
        }
        .pt-10
        {
            padding-top: 10px;
        }

        .pt-15
        {
            padding-top: 15px;
        }

        .pb-10
        {
            padding-bottom: 10px;
        }

        .pTop10 {
    padding-top: 20px;
}
        .custom-padd
        {
            padding-top: 4px;
    padding-bottom: 10px;
        }

        input + label
        {
                margin-right: 10px;
        }

        .btn
        {
            margin-bottom: 0;
        }

        .pl-10
        {
            padding-left: 10px;
        }

        .col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }

        .devCheck
        {
            margin-top: 5px;
        }

        .mtc-5
        {
            margin-top: 5px;
        }

        .mtc-10
        {
            margin-top: 10px;
        }

        /*select.btn
        {
           position: relative;
           z-index: 0;
        }*/

        select
        {
            margin-bottom: 0;
        }

        .form-control
        {
            background-color: transparent;
        }

        select.btn-radius {
    padding: 4px 8px 6px 11px !important;
}
        .mt-30{
            margin-top: 30px;
        }

        .panel-title h3
        {
            padding-top: 0;
            padding-bottom: 0;
        }

        .btn-radius
        {
            padding: 4px 11px !important;
            border-radius: 4px !important;
        }

        .crossBtn
        {
             right: 30px;
             top: 25px;
        }

        .mb-10
        {
            margin-bottom: 10px;
        }

        .btn-cust
        {
            background-color: #108b47 !important;
            color: #fff;
        }

        .btn-cust:hover
        {
            background-color: #097439 !important;
            color: #fff;
        }

        .gHesder
        {
            background: #1b5ca0 !important;
            color: #ffffff !important;
            padding: 6px 0 6px !important;
        }

        .close
        {
             color: #fff;
             opacity: .5;
             font-weight: 400;
        }

        .mt-24
        {
            margin-top: 24px;
        }

        .col-md-3
        {
            margin-top: 8px;
        }

        #upldBigLogo , #upldSmallLogo
        {
            width: 100%;
        }

        #DivSetAsDefault
        {
            margin-top: 25px;
        }

        /*.dxeDisabled_PlasticBlue, .aspNetDisabled {
            opacity: 0.4 !important;
            color: #ffffff !important;
        }*/
                /*.padTopbutton {
            padding-top: 27px;
        }*/
        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
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
        <div class="col-md-3 mb-10">
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
    </div>

    <asp:SqlDataSource ID="ProductDataSource" runat="server"
        SelectCommand="select h.sProducts_ID,h.sProducts_Code,h.sProducts_Name,(select d.ProductClass_Name from Master_ProductClass d where d.ProductClass_ID=h.ProductClass_Code)   as 'ProductClass_Code'
          ,sProduct_MRP,(select d.prodMarkup_min from tbl_master_productSalesPriceImport d where d.prod_id=h.sProducts_ID ) markupmin,
          (select d.prodMarkup_plus from tbl_master_productSalesPriceImport d where d.prod_id=h.sProducts_ID ) markupPlus,sProduct_SalePrice,sProduct_MinSalePrice, (select d.DiscountUpTo from tbl_master_productSalesPriceImport d where d.prod_id=h.sProducts_ID ) DiscountUpTo from Master_sProducts h"
        UpdateCommand="select null"
        ></asp:SqlDataSource>
</asp:Content>



