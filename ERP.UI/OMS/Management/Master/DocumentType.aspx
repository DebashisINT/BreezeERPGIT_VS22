<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                27-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="Document Types" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_DocumentType" CodeBehind="DocumentType.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        
        .dxeErrorFrameSys.dxeErrorCellSys {
            position:absolute;
        }

        /*Rev 1.0*/

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

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        /*select.btn
        {
            padding-right: 10px !important;
        }*/

        /*.panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }*/

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
        .TableMain100 #GrdHolidays , #gridudfGroup
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

        .btn.btn-xs
        {
                font-size: 14px !important;
        }
        /*Rev end 1.0*/
    </style>
    <script type="text/javascript">
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function EndCall(obj) {
            if (grid.cpDelmsg != null)
                alert(grid.cpDelmsg);
            grid.cpDelmsg = null;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
            <div class="panel-title">
                <h3>Document Types</h3>
            </div>

        </div>
        <div class="form_main">
            <table class="TableMain100">
                <%-- <tr>
                    <td class="EHEADER" style="text-align: center">
                        <strong><span style="color: #000099">Document Type</span></strong></td>
                </tr>--%>
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td style="text-align: left; vertical-align: top">
                                    <table>
                                        <tr>
                                            <td id="ShowFilter">
                                                <% if (rights.CanAdd)
                                                   { %>
                                                <a href="javascript:void(0);" onclick="grid.AddNewRow()" class="btn btn-success btn-radius "><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Add New</span> </a>
                                                <%} %>
                                                <% if (rights.CanExport)
                                                   { %>
                                                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius " OnChange="if(!AvailableExportOption()){return false;}" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                                    <asp:ListItem Value="2">XLS</asp:ListItem>
                                                    <asp:ListItem Value="3">RTF</asp:ListItem>
                                                    <asp:ListItem Value="4">CSV</asp:ListItem>
                                                </asp:DropDownList>
                                                 <%} %>
                                                <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-primary"><span>Show Filter</span></a>--%>
                                            </td>
                                            <td id="Td1">
                                                <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <%--<td></td>
                                <td class="gridcellright pull-right">
                                    <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true"
                                        Font-Bold="False" ForeColor="black" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                        ValueType="System.Int32" Width="130px">
                                        <Items>
                                            <dxe:ListEditItem Text="Select" Value="0" />
                                            <dxe:ListEditItem Text="PDF" Value="1" />
                                            <dxe:ListEditItem Text="XLS" Value="2" />
                                            <dxe:ListEditItem Text="RTF" Value="3" />
                                            <dxe:ListEditItem Text="CSV" Value="4" />
                                        </Items>
                                        <ButtonStyle>
                                        </ButtonStyle>
                                        <ItemStyle>
                                            <HoverStyle>
                                            </HoverStyle>
                                        </ItemStyle>
                                        <Border BorderColor="black" />
                                        <DropDownButton Text="Export">
                                        </DropDownButton>
                                    </dxe:ASPxComboBox>
                                </td>--%>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="relative">
                        <dxe:ASPxGridView ID="DocumentGrid" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False" OnRowDeleting="DocumentGrid_RowDeleting"
                            DataSourceID="DocumentType" KeyFieldName="dty_id" Width="100%" OnHtmlEditFormCreated="DocumentGrid_HtmlEditFormCreated"
                            OnHtmlRowCreated="DocumentGrid_HtmlRowCreated" OnCustomCallback="DocumentGrid_CustomCallback" OnCommandButtonInitialize="DocumentGrid_CommandButtonInitialize" >
                           <SettingsSearchPanel Visible="True" Delay="5000" />
                             <clientsideevents endcallback="function(s, e) {
                                EndCall(s.cpEND);
	
    }"></clientsideevents>
                            <columns>
                                <dxe:GridViewDataTextColumn FieldName="dty_id" ReadOnly="True" Visible="False" Caption="ID" CellStyle-Font-Bold="true"
                                    VisibleIndex="0" HeaderStyle-HorizontalAlign="Center" Width="10%">
                                    <%--
                                     <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                        <Paddings PaddingTop="15px" />
                                     
                                    </EditCellStyle>--%>
                                    <%-- <CellStyle Wrap="False">
                                    </CellStyle>--%>
                                    <EditFormSettings Visible="False" />
    <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn Caption="Document Type" FieldName="dty_documentType"
                                    VisibleIndex="0" >
                                    <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                        <Paddings PaddingTop="15px" />
                                    </EditCellStyle>
                                    <CellStyle Wrap="False">
                                    </CellStyle>
                                    <PropertiesTextEdit Width="280px" MaxLength="50">
                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                            <RequiredField  IsRequired="True" ErrorText="Mandatory" />
                                        </ValidationSettings>
                                    </PropertiesTextEdit>
                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                                    </EditFormCaptionStyle>
                                    <EditFormSettings Caption="Document Type" Visible="True" />
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>
                            
                                <dxe:GridViewDataComboBoxColumn FieldName="dty_applicableFor" Caption="Applicable For"
                                    VisibleIndex="1" >
                                    <PropertiesComboBox ValueType="System.String" EnableSynchronization="False" EnableIncrementalFiltering="True"
                                        Width="280px">
                                        <Items>
                                            <%--<dxe:ListEditItem Text="Products MF" Value="Products MF">
                                            </dxe:ListEditItem>--%>
                                            <%--        <dxe:ListEditItem Text="Products Insurance" Value="Products Insurance">
                                            </dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Products IPOs" Value="Products IPOs">
                                            </dxe:ListEditItem>--%>
                                            <dxe:ListEditItem Text="Customer/Client" Value="Customer/Client"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Product" Value="Product"></dxe:ListEditItem>
                                            <%-- <dxe:ListEditItem Text="Lead" Value="Lead">
                                            </dxe:ListEditItem>--%>
                                            <dxe:ListEditItem Text="Employee" Value="Employee"></dxe:ListEditItem>
                                            <%--<dxe:ListEditItem Text="Sub Brokers" Value="Sub Broker">
                                            </dxe:ListEditItem>--%>
                                            <dxe:ListEditItem Text="Franchisees" Value="Franchisees"></dxe:ListEditItem>
                                            <%--<dxe:ListEditItem Text="Data Vendors" Value="Data Vendor">
                                            </dxe:ListEditItem>--%>
                                            <dxe:ListEditItem Text="Relationship Partner" Value="Relationship Partner"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Business Partner" Value="Partner"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Recruitment Agents" Value="Recruitment Agent"></dxe:ListEditItem>
                                            <%-- <dxe:ListEditItem Text="AMCs" Value="AMCs">
                                            </dxe:ListEditItem>--%>
                                            <%-- <dxe:ListEditItem Text="Insurance Companies" Value="Insurance Companies">
                                            </dxe:ListEditItem>--%>
                                            <%--<dxe:ListEditItem Text="RTAs" Value="RTAs">
                                            </dxe:ListEditItem>--%>
                                            <dxe:ListEditItem Text="Branches" Value="Branches"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Companies" Value="Companies"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Building" Value="Building"></dxe:ListEditItem>
                                            <%--  <dxe:ListEditItem Text="ConsumerComp" Value="ConsumerComp">
                                            </dxe:ListEditItem>--%>
                                            <dxe:ListEditItem Text="Outsourcing Companies" Value="Outsourcing Companies"></dxe:ListEditItem>
                                                 <dxe:ListEditItem Text="Vendors / Service Providers" Value="HRrecruitmentagent">
                                            </dxe:ListEditItem>
                                            <%-- <dxe:ListEditItem Text="NSDL Clients" Value="NSDL Clients">
                                            </dxe:ListEditItem>
                                            <dxe:ListEditItem Text="CDSL Clients" Value="CDSL Clients">
                                            </dxe:ListEditItem>--%>
                                            <dxe:ListEditItem Text="Candidate" Value="Candidate"></dxe:ListEditItem>
                                            <%--<dxe:ListEditItem Text="Exchanges" Value="Exchanges">
                                            </dxe:ListEditItem>--%>
                                            <dxe:ListEditItem Text="Account Heads" Value="Account Heads"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Other Entity" Value="Other Entity"></dxe:ListEditItem>
                                            <%--<dxe:ListEditItem Text="Raw Materials" Value="Raw Materials"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Work-In-Process" Value="Work-In-Process"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Finished Goods" Value="Finished Goods"></dxe:ListEditItem>--%>
                                            <dxe:ListEditItem Text="Lead" Value="Lead"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Job Work Order" Value="JobWorkOrder"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Return FG" Value="ReturnFGReceived"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Material Issue" Value="MaterialIssue"></dxe:ListEditItem>
                                             <dxe:ListEditItem Text="FG Received" Value="FGReceived"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Material Issue Return" Value="ReturnMaterialsIssue"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Service Contract" Value="ServiceContract"></dxe:ListEditItem>
                                             <dxe:ListEditItem Text="Field Service" Value="FieldService"></dxe:ListEditItem>
                                             <dxe:ListEditItem Text="Cases" Value="Cases"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Agents" Value="Agents"></dxe:ListEditItem>
                                             <dxe:ListEditItem Text="Quotation" Value="Quotation"></dxe:ListEditItem>
                                             <dxe:ListEditItem Text="Sales Order" Value="SalesOrder"></dxe:ListEditItem>
                                             <dxe:ListEditItem Text="Sales Order" Value="SalesOrder"></dxe:ListEditItem>

                                             <dxe:ListEditItem Text="Purchase Invoice" Value="PurchaseInvoice"></dxe:ListEditItem>
                                             <dxe:ListEditItem Text="Transit Purchase Invoice" Value="TransitPurchaseInvoice"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Sales Return" Value="SalesReturn"></dxe:ListEditItem>
                                              <dxe:ListEditItem Text="Purchase Return" Value="PurchaseReturn"></dxe:ListEditItem>

                                            <dxe:ListEditItem Text="Sale Challan" Value="SaleChallan"></dxe:ListEditItem>
                                             <dxe:ListEditItem Text="Sale Return" Value="SaleReturn"></dxe:ListEditItem>
                                             <dxe:ListEditItem Text="Sale Invoice" Value="SalesInvoice"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Transit Sale Invoice" Value="TransitSalesInvoice"></dxe:ListEditItem>
                                             <dxe:ListEditItem Text="Customer Receipt/Payment" Value="CustomerReceiptPayment"></dxe:ListEditItem>
                                             <dxe:ListEditItem Text="Vendor Payment/Receipt" Value="VendorPaymentReceipt"></dxe:ListEditItem>

                                              <dxe:ListEditItem Text="Customer Debit/Credit Note" Value="CustomerDebitCreditNote"></dxe:ListEditItem>
                                             <dxe:ListEditItem Text="Vendor Debit/Credit Note" Value="VendorDebitCreditNote"></dxe:ListEditItem>
                                             <dxe:ListEditItem Text="Purchase Indent" Value="PurchaseIndent"></dxe:ListEditItem>
                                             <dxe:ListEditItem Text="Branch Requisition" Value="BranchRequisition"></dxe:ListEditItem>
                                             <dxe:ListEditItem Text="Purchase Order" Value="PurchaseOrder"></dxe:ListEditItem>

                                             <dxe:ListEditItem Text="Purchase Challan" Value="PurchaseChallan"></dxe:ListEditItem>
                                           <%-- Mantis Issue 25010--%>
                                             <%--<dxe:ListEditItem Text="Branch Tranfer Out" Value="BTO"></dxe:ListEditItem>
                                             <dxe:ListEditItem Text="Branch Tranfer In" Value="BTI"></dxe:ListEditItem>--%>
                                            <dxe:ListEditItem Text="Branch Tranfer Out" Value="BTOUT"></dxe:ListEditItem>
                                             <dxe:ListEditItem Text="Branch Tranfer In" Value="BTIN"></dxe:ListEditItem>
                                            <%--End of Mantis Issue 25010--%>
                                             <dxe:ListEditItem Text="Service Centre In" Value="ServiceCentreIn"></dxe:ListEditItem>
                                             <dxe:ListEditItem Text="Customer Return" Value="CustomerReturn"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Issue To Return" Value="IssueToReturn"></dxe:ListEditItem>
                                        
                                            <dxe:ListEditItem Text="Project Purchase Invoice" Value="ProjectPurchaseInvoice"></dxe:ListEditItem>

                                             <dxe:ListEditItem Text="Financer" Value="Financer"></dxe:ListEditItem>


                                              <dxe:ListEditItem Text="Transporter" Value="Transporter"></dxe:ListEditItem>

                                                <dxe:ListEditItem Text="Manual Sale Return" Value="ManualSaleReturn"></dxe:ListEditItem>


                                              <dxe:ListEditItem Text="Normal Sale Return" Value="NormalSaleReturn"></dxe:ListEditItem>
                                              <dxe:ListEditItem Text="Undelivery Return" Value="UndeliveryReturn"></dxe:ListEditItem>
                                              <dxe:ListEditItem Text="Purchase Return Manual" Value="PurchaseReturnManual"></dxe:ListEditItem>

                                             <dxe:ListEditItem Text="Rate Difference Entry Vendor" Value="RateDifferenceEntryVendor"></dxe:ListEditItem>
                                               <dxe:ListEditItem Text="Rate Difference Entry Customer" Value="RateDifferenceEntryCustomer"></dxe:ListEditItem>
                                             <dxe:ListEditItem Text="Bill Of Materials" Value="BillOfMaterials"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Campaign" Value="Campaign"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Estimate" Value="Estimate"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Project Quotation" Value="ProjectQuotation"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Bill of Quantities" Value="BOQ"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Project Purchase Order" Value="ProjectPurchaseOrder"></dxe:ListEditItem>
                                             <dxe:ListEditItem Text="Project Purchase GRN" Value="ProjectPurchaseChallan"></dxe:ListEditItem>
                                             <dxe:ListEditItem Text="Project Order" Value="ProjectSOrder"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Project Invoice" Value="ProjectInvoice"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="CRM Contact" Value="CRM Contact"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="CRM Contact" Value="CRM Contact"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Delivery" Value="SrvDelivery"></dxe:ListEditItem>
                                            <%--Mantis Issue 21677,21676,23104 (03/06/2021)--%>
                                            <dxe:ListEditItem Text="Contact" Value="Contact"></dxe:ListEditItem>
                                            <%--End of Mantis Issue 21677,21676,23104 (03/06/2021)--%>

                                            <%-- <dxe:ListEditItem Text="Products MF" Value="Products MF"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Products Insurance" Value="Products Insurance"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Products IPOs" Value="Products IPOs"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Customer/Client" Value="Customer/Client"></dxe:ListEditItem>
                                                                                    <dxe:ListEditItem Text="Employee" Value="Employee"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Sub Brokers" Value="Sub Broker"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Franchisses" Value="Franchisee"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Data Vendors" Value="Data Vendor"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Relationship Partner" Value="Relationship Partner"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Business Partner" Value="Partner"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Recruitment Agents" Value="Recruitment Agent"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="AMCs" Value="AMCs"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Insurance Companies" Value="Insurance Companies"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="RTAs" Value="RTAs"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Branches" Value="Branches"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Companies" Value="Companies"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Buildzsing" Value="Buildzsing"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="ConsumerComp" Value="ConsumerComp"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="OutsourcingComp" Value="OutsourcingComp"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="HRrecruitmentagent" Value="HRrecruitmentagent"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="NSDL Clients" Value="NSDL Clients"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="CDSL Clients" Value="CDSL Clients"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Candidate" Value="Candidate"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Exchanges" Value="Exchanges"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Account Heads" Value="Account Heads"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="OtherEntity" Value="OtherEntity"></dxe:ListEditItem>--%>
                                            <%--Mantis Issue 25010--%>
                                            <dxe:ListEditItem Text="Warehouse Wise Stock Transfer" Value="WarehousewiseStockTransfer"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Warehouse Wise Stock OUT" Value="WarehouseWiseStockOUT"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Warehouse Wise Stock IN" Value="WarehouseWiseStockIN"></dxe:ListEditItem>
                                            <%--End of Mantis Issue 25010--%>
                                            <%--Mantis Issue 25065--%>
                                            <dxe:ListEditItem Text="Project Indent/Requisition" Value="ProjectIndentRequisition"></dxe:ListEditItem>
                                            <%--End of Mantis Issue 25065--%>
                                            <%--Mantis Issue 25125--%>
                                            <dxe:ListEditItem Text="Purchase Indent/Requisition" Value="PurchaseIndent"></dxe:ListEditItem>
                                            <%--End of Mantis Issue 25125--%>
                                        </Items>
                                    </PropertiesComboBox>
                                    <EditFormSettings Visible="True" Caption="Applicable For"></EditFormSettings>
                                    <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                        <Paddings PaddingTop="15px" />
                                    </EditCellStyle>
                                    <CellStyle Wrap="False">
                                    </CellStyle>
                                    <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                                    </EditFormCaptionStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataComboBoxColumn>
                           
                                 <dxe:GridViewDataTextColumn FieldName="Mandatory1" Caption="Is mandatory?" VisibleIndex="2" Width="120px">
                                    <EditFormSettings Visible="False" />
                                     <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataCheckColumn Caption="Is this a mandatory document?" FieldName="Mandatory"
                                    VisibleIndex="2" Visible="False">
                                    <PropertiesCheckEdit ValueChecked="1" ValueType="System.String" ValueUnchecked="0">
                                        <Style HorizontalAlign="Left"></Style>
                                    </PropertiesCheckEdit>
                                    <EditFormSettings Visible="True" />
                                    <EditFormCaptionStyle Wrap="False" ForeColor="Red">
                                    </EditFormCaptionStyle>
                                    <CellStyle ForeColor="Red">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataCheckColumn>


                                <dxe:GridViewCommandColumn VisibleIndex="3" ShowEditButton="true" ShowDeleteButton="true" Width="6%">
                                    <%--<DeleteButton Visible="True">
                                    </DeleteButton>
                                    <EditButton Visible="True">
                                    </EditButton>--%>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <HeaderTemplate>
                                        Actions
                                        <%--<%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                          { %>
                                        <a href="javascript:void(0);" onclick="grid.AddNewRow()"><span style="text-decoration: underline">Add New</span> </a>
                                        <%} %>--%>
                                    </HeaderTemplate>
                                
                                </dxe:GridViewCommandColumn>
                            </columns>
                        
                            <settingscommandbutton>
                                <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                                </EditButton>
                                <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                </DeleteButton>
                                <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary "></UpdateButton>
                                <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger"></CancelButton>
                            </settingscommandbutton>
                            <styles>
                                <LoadingPanel ImageSpacing="10px">
                                </LoadingPanel>
                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                </Header>
                                <Cell CssClass="gridcellleft">
                                </Cell>
                            </styles>
                       
                            <settings showstatusbar="Visible" showgrouppanel="True" showfilterrow="true" ShowFilterRowMenu="true" />
                            <settingstext popupeditformcaption="Add/Modify Document" />
                            <settingspager numericbuttoncount="20" pagesize="20" alwaysshowpager="True" showseparators="True">
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </settingspager>
                            <settingsbehavior columnresizemode="NextColumn" confirmdelete="True" />
                            <settingsediting editformcolumncount="1" mode="PopupEditForm" popupeditformhorizontalalign="Center"
                                popupeditformmodal="True" popupeditformverticalalign="WindowCenter" popupeditformwidth="450px" />
                            <templates>
                                <EditForm>
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 5%"></td>
                                            <td style="width: 75%">
                                                <%--<controls></controls>--%>
                                                <dxe:ASPxGridViewTemplateReplacement ID="Editors" runat="server" ColumnID="" ReplacementType="EditFormEditors"></dxe:ASPxGridViewTemplateReplacement>
                                                <div style="padding: 2px 2px 2px 121px">
                                                    <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton" 
                                                        runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                    <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                        runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                </div>
                                            </td>
                                            <td style="width: 20%"></td>
                                        </tr>
                                    </table>
                                </EditForm>
                            </templates>

                        </dxe:ASPxGridView>
                    </td>
                </tr>
            </table>
            <asp:SqlDataSource ID="DocumentType" runat="server" ConflictDetection="CompareAllValues"
                DeleteCommand="DELETE FROM [tbl_master_documentType] WHERE [dty_id] = @original_dty_id"
                InsertCommand="INSERT INTO [tbl_master_documentType] ([dty_documentType], [dty_applicableFor],[dty_SearchBy],[dty_mandatory], [CreateDate], [CreateUser]) VALUES (@dty_documentType, @dty_applicableFor,0,@Mandatory,getdate(),@CreateUser)"
                OldValuesParameterFormatString="original_{0}"
                SelectCommand="SELECT [dty_id],[dty_documentType],[dty_applicableFor],dty_mandatory as Mandatory,case dty_mandatory when '1' then 'Yes' else 'No' end as Mandatory1 FROM [tbl_master_documentType]"
                UpdateCommand="UPDATE [tbl_master_documentType] SET [dty_documentType] = @dty_documentType, [dty_applicableFor] = @dty_applicableFor,dty_mandatory=@Mandatory, [LastModifyDate] = getdate(), [LastModifyUser] = @CreateUser WHERE [dty_id] = @original_dty_id">
                <DeleteParameters>
                    <asp:Parameter Name="original_dty_id" Type="Decimal" />
                </DeleteParameters>
                <UpdateParameters>
                    <asp:Parameter Name="dty_documentType" Type="String" />
                    <asp:Parameter Name="dty_applicableFor" Type="String" />
                    <asp:Parameter Name="Mandatory" Type="String" />
                    <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" />
                </UpdateParameters>
                <InsertParameters>
                    <asp:Parameter Name="dty_documentType" Type="String" />
                    <asp:Parameter Name="dty_applicableFor" Type="String" />
                    <asp:Parameter Name="Mandatory" Type="String" />
                    <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" />
                </InsertParameters>
            </asp:SqlDataSource>
            <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true" >
            </dxe:ASPxGridViewExporter>
            <br />
        </div>
    </div>
</asp:Content>

