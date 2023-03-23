<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                23-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="Product Sales Price Import" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" EnableEventValidation="false" Inherits="ERP.OMS.Management.Store.Master.management_master_frmProductSalesPriceImport" CodeBehind="frmProductSalesPriceImport.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <style>
        #ghhhtbl td {
            padding-right: 15px;
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
            <h3>Product Sales Price Import</h3>
        </div>

    </div>
        <div class="form_main" style="align-items: center;">

        <div class="col-md-3 mb-10">
                    <label>Choose File</label>
                    <div>
                        <asp:FileUpload ID="uploadProdSalesPrice" runat="server" Width="100%"/>
                    </div>
                </div>

       <div class="col-md-3">
                    <label>&nbsp;</label>
                    <div>
                        <asp:Button ID="BtnSave" runat="server" Text="Import File" CssClass="btn btn-primary" OnClick="BtnSave_Click" />
                        <asp:LinkButton ID="lnlDownloader" runat="server" OnClick="lnlDownloader_Click" CssClass="btn btn-info">Download Format</asp:LinkButton>
                    </div>
                </div>

        <div class="clear"></div>
        <div class="col-md-12">
             <dxe:ASPxGridView runat="server" Width="100%" ID="gridprodSalesPrice"  AutoGenerateColumns="False" ClientInstanceName="grid" OnDataBinding="grid_DataBinding"  SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"   >
                                                    <SettingsPager Mode="ShowAllRecords" Visible="False" ></SettingsPager>
                                                  <SettingsSearchPanel Visible="True" Delay="5000" />
                                                    <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                                    <Columns>
                                                        <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="Product Code" Caption="Product Code">
                                                            <Settings AutoFilterCondition="Contains"></Settings>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                   <%--     <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="Product Name" Caption="Product Name">
                                                            <Settings AutoFilterCondition="Contains"></Settings>
                                                        </dxe:GridViewDataTextColumn>--%>
                                                       <%-- <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="Product Class" Caption="Product Class">
                                                            <Settings AutoFilterCondition="Contains"></Settings>
                                                        </dxe:GridViewDataTextColumn>--%>
                                                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="MRP" Caption="MRP">
                                                            <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle> 
                                                            <Settings AutoFilterCondition="Contains"></Settings>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="Markup(-)%" Caption="Markup(-)%">
                                                            <Settings AutoFilterCondition="Contains"></Settings>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Markup(+)%" Caption="Markup(+)%">
                                                            <Settings AutoFilterCondition="Contains"></Settings>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn  VisibleIndex="6" FieldName="Sale Price" Caption="Sale Price">
                                                            <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle> 
                                                            <Settings AutoFilterCondition="Contains"></Settings>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn   VisibleIndex="7" FieldName="Min Sale Price" Caption="Min Sale Price">
                                                            <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle> 
                                                             <PropertiesTextEdit DisplayFormatString="f2">
                                                              </PropertiesTextEdit>
                                                            <Settings AutoFilterCondition="Contains"></Settings>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                    </Columns>
                 <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                                                </dxe:ASPxGridView>

        </div>
         <div class="clear"></div>
          <div class="col-md-3">
                    <label>&nbsp;</label>
                    <div>
                        <asp:Button ID="btnUploadRecord" runat="server" Text="Save & Update" CssClass="btn btn-success" OnClick="btnUploadRecord_Click" />
                        
                    </div>
                </div>
    </div>
    </div>
</asp:Content>



