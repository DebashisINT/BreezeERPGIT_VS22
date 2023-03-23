<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                23-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="Defective Product Marking" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="DefectiveProductMarking.aspx.cs" Inherits="ERP.OMS.Management.Activities.DefectiveProductMarking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function SearchStock() {
            cOpeningGrid.PerformCallback('BindGrid');
        }

        function SaveStock() {
            cOpeningGrid.PerformCallback('SaveBindGrid');
        }

        function OnDentSelectedChanged(s, e, itemIndex) {
            var obj = cOpeningGrid.GetRowKey(itemIndex);
            var globalChecked = s.GetChecked();
            var productList = globalChecked + '~' + obj;

            if (document.getElementById("hdnDentProduct").value == '') {
                document.getElementById("hdnDentProduct").value = productList;
            }
            else {
                document.getElementById("hdnDentProduct").value = document.getElementById("hdnDentProduct").value + ',' + productList;
            }
        }

        function OnDisplaySelectedChanged(s, e, itemIndex) {
            var obj = cOpeningGrid.GetRowKey(itemIndex);
            var globalChecked = s.GetChecked();
            var productList = globalChecked + '~' + obj;

            if (document.getElementById("hdnDisplayProduct").value == '') {
                document.getElementById("hdnDisplayProduct").value = productList;
            }
            else {
                document.getElementById("hdnDisplayProduct").value = document.getElementById("hdnDisplayProduct").value + ',' + productList;
            }
        }

        function OnStolenSelectedChanged(s, e, itemIndex) {
            var obj = cOpeningGrid.GetRowKey(itemIndex);
            var globalChecked = s.GetChecked();
            var productList = globalChecked + '~' + obj;

            if (document.getElementById("hdnStolenProduct").value == '') {
                document.getElementById("hdnStolenProduct").value = productList;
            }
            else {
                document.getElementById("hdnStolenProduct").value = document.getElementById("hdnStolenProduct").value + ',' + productList;
            }
        }

        function OnEndCallback(s, e) {
            if (cOpeningGrid.cpSaveSuccessOrFail == "successInsert") {
                document.getElementById("hdnDentProduct").value = "";
                document.getElementById("hdnDisplayProduct").value = "";
                document.getElementById("hdnStolenProduct").value = "";
                cOpeningGrid.cpSaveSuccessOrFail = null;

                jAlert('Product Marking successfully.');
            }
            else if (cOpeningGrid.cpSaveSuccessOrFail == "errorInsert") {
                document.getElementById("hdnDentProduct").value = "";
                document.getElementById("hdnDisplayProduct").value = "";
                document.getElementById("hdnStolenProduct").value = "";
                cOpeningGrid.cpSaveSuccessOrFail = null;

                jAlert('try again later.');
            }
        }
    </script>

    <style>
        /*Rev 1.0*/

        select
        {
            height: 30px !important;
            border-radius: 4px;
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

        .mt-27
        {
            margin-top: 27px !important;
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

        .dxeBase_PlasticBlue .dxichTextCellSys label
        {
            color: #fff !important;
        }

        #actv-warh label
        {
            color: #111 !important;
        }

        .btn.btn-xs
        {
                font-size: 14px !important;
        }

        /*#ShowFilter
        {
            padding-bottom: 3px !important;
        }*/

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
        <div class="panel-title clearfix">
            <h3 class="pull-left">Product Marking</h3>
        </div>
    </div>
        <div class="form_main">
        <div style="background: #f5f4f3; padding: 8px 0; margin-bottom: 0px; border-radius: 4px; border: 1px solid #ccc;" class="clearfix col-md-12">
            <div class="col-md-3">
                <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="Branch">
                </dxe:ASPxLabel>
                <dxe:ASPxComboBox ID="cmbbranch" runat="server" ClientInstanceName="ccmbbranch" TextField="branch_description" ValueField="branch_id" Width="100%">
                </dxe:ASPxComboBox>
            </div>
            <div class="col-md-3">
                <dxe:ASPxLabel ID="lbl_Product" runat="server" Text="Product">
                </dxe:ASPxLabel>
                <dxe:ASPxGridLookup ID="productLookUp" runat="server" DataSourceID="ProductDataSource" ClientInstanceName="cproductLookUp"
                    KeyFieldName="sProducts_ID" Width="100%" TextFormatString="{0}" MultiTextSeparator=", ">
                    <Columns>
                        <dxe:GridViewDataColumn FieldName="Products_Name" Caption="Code" Width="180">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="Products_Description" Caption="Name" Width="240">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="ClassCode" Caption="Class" Width="220">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="BrandName" Caption="Brand" Width="140">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                    </Columns>
                    <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                        <Templates>
                            <StatusBar>
                                <table class="OptionsTable" style="float: right">
                                    <tr>
                                        <td>
                                            <%--  <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />--%>
                                        </td>
                                    </tr>
                                </table>
                            </StatusBar>
                        </Templates>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                    </GridViewProperties>
                </dxe:ASPxGridLookup>
            </div>
            <div class="col-md-3 mt-27">
                <%--<div>
                    <br />
                </div>--%>
                <dxe:ASPxButton ID="btnSearch" ClientInstanceName="cbtnSearch" runat="server" Text="Search" AutoPostBack="False" CssClass="btn btn-info" UseSubmitBehavior="False">
                    <ClientSideEvents Click="function(s, e) {SearchStock();}" />
                </dxe:ASPxButton>
                &nbsp;&nbsp;
                 <% if (rights.CanExport)
                    { %>
                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnChange="if(!AvailableExportOption()){return false;}" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">PDF</asp:ListItem>
                    <asp:ListItem Value="2">XLS</asp:ListItem>
                    <asp:ListItem Value="3">RTF</asp:ListItem>
                    <asp:ListItem Value="4">CSV</asp:ListItem>
                </asp:DropDownList>
                <% } %>
            </div>
        </div>
        <div style="clear: both">
            <br />
        </div>
        <div class="clearfix">
            <dxe:ASPxGridView ID="OpeningGrid" runat="server" KeyFieldName="SerialID" AutoGenerateColumns="False" EnableRowsCache="true"
                Width="100%" ClientInstanceName="cOpeningGrid" OnDataBinding="OpeningGrid_DataBinding" OnCustomCallback="OpeningGrid_CustomCallback"  SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" >
                <Settings ShowGroupPanel="false" ShowStatusBar="Hidden" ShowTitlePanel="false" ShowFilterRow="true" ShowFilterRowMenu="true"   />
                <SettingsDataSecurity AllowDelete="False" />
                 <SettingsSearchPanel Visible="True" Delay="5000" />
                <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                    <FirstPageButton Visible="True">
                    </FirstPageButton>
                    <LastPageButton Visible="True">
                    </LastPageButton>
                </SettingsPager>
                <Columns>
                    <dxe:GridViewDataTextColumn Caption="Warehouse" VisibleIndex="0" Width="20%" FieldName="WarehouseName" CellStyle-VerticalAlign="Top">
                        <EditFormSettings Visible="False" />
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                    <%--<dxe:GridViewDataTextColumn Caption="Rate" VisibleIndex="1" Width="5%" FieldName="Rate" CellStyle-Wrap="True">
                        <EditFormSettings Visible="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Stock Qty" VisibleIndex="2" Width="5%" FieldName="IN_Quantity" CellStyle-VerticalAlign="Top">
                        <EditFormSettings Visible="False" />
                    </dxe:GridViewDataTextColumn>--%>

                    <dxe:GridViewDataTextColumn Caption="Doc Type" VisibleIndex="1" Width="10%" FieldName="DocType" CellStyle-Wrap="True">
                        <EditFormSettings Visible="False" />
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                  <%--  <dxe:GridViewDataTextColumn Caption="Stock IN Time" VisibleIndex="2" Width="15%" FieldName="Stock_IN_OUT_Date" CellStyle-VerticalAlign="Top">
                        <EditFormSettings Visible="False" />
                    </dxe:GridViewDataTextColumn>--%>

                    <dxe:GridViewDataTextColumn Caption="Batch" VisibleIndex="4" Width="20%" FieldName="BatchNo" CellStyle-VerticalAlign="Top" CellStyle-Wrap="True">
                        <EditFormSettings Visible="False" />
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Serial" VisibleIndex="5" Width="20%" FieldName="SerialNo" CellStyle-Wrap="True">
                        <EditFormSettings Visible="False" />
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Dent Marking" FieldName="IsDent" VisibleIndex="6" Settings-AllowAutoFilter="False">
                        <DataItemTemplate>
                            <div style="text-align: center">
                                <dxe:ASPxCheckBox ID="chkDent" ClientInstanceName="cchkDent" runat="server" Checked='<%# GetChecked(Eval("IsDent").ToString()) %>' OnInit="chkDent_Init">
                                </dxe:ASPxCheckBox>
                            </div>
                        </DataItemTemplate>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Display Marking" FieldName="IsDisplay" VisibleIndex="6" Settings-AllowAutoFilter="False">
                        <DataItemTemplate>
                            <div style="text-align: center">
                                <dxe:ASPxCheckBox ID="chkDisplay" ClientInstanceName="cchkDisplay" runat="server" Checked='<%# GetChecked(Eval("IsDisplay").ToString()) %>' OnInit="chkDisplay_Init">
                                </dxe:ASPxCheckBox>
                            </div>
                        </DataItemTemplate>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Stolen Marking" FieldName="IsStolen" VisibleIndex="6" Visible="false" Settings-AllowAutoFilter="False">
                        <DataItemTemplate>
                            <div style="text-align: center">
                                <dxe:ASPxCheckBox ID="chkStolen" ClientInstanceName="cchkStolen" runat="server" Checked='<%# GetChecked(Eval("IsStolen").ToString()) %>' OnInit="chkStolen_Init">
                                </dxe:ASPxCheckBox>
                            </div>
                        </DataItemTemplate>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                </Columns>
                <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                <ClientSideEvents EndCallback="OnEndCallback" />
               
            </dxe:ASPxGridView>
        </div>
        <div style="clear: both">
            <br />
        </div>
        <div class="clearfix">
            <% if (rights.CanAdd)
               { %>
            <dxe:ASPxButton ID="btnSave" ClientInstanceName="cbtnSave" runat="server" Text="Save & Exit" AutoPostBack="False" CssClass="btn btn-primary" UseSubmitBehavior="False">
                <ClientSideEvents Click="function(s, e) {SaveStock();}" />
            </dxe:ASPxButton>
            <% } %>
        </div>
    </div>
    </div>
    <div>
        <asp:SqlDataSource runat="server" ID="ProductDataSource" 
            SelectCommand="prc_CRMSalesInvoice_Details" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:Parameter Type="String" Name="Action" DefaultValue="AllProductDetails" />
                <asp:Parameter DefaultValue="Y" Name="InventoryType" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    <div>
        <asp:HiddenField runat="server" ID="hdnDentProduct" />
        <asp:HiddenField runat="server" ID="hdnDisplayProduct" />
        <asp:HiddenField runat="server" ID="hdnStolenProduct" />
    </div>
    <div style="display: none">
        <dxe:ASPxGridView ID="openingGridExport" runat="server" KeyFieldName="ProductID" AutoGenerateColumns="True"
            Width="100%" EnableRowsCache="true" SettingsBehavior-AllowFocusedRow="true"
            OnDataBinding="openingGridExport_DataBinding">
        </dxe:ASPxGridView>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
</asp:Content>
