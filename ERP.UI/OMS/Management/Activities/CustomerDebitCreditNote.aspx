<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                10-04-2023        2.0.37           Pallab              Transactions pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="CustomerDebitCreditNote.aspx.cs" Inherits="ERP.OMS.Management.Activities.CustomerDebitCreditNote" %>

<%@ Register Src="~/OMS/Management/Activities/UserControls/Sales_BillingShipping.ascx" TagPrefix="ucbs" TagName="Sales_BillingShipping" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="JS/CustomerNote.js?v=2.1"></script>
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchPopup.js"></script>
    <script src="../../Tax%20Details/Js/TaxDetailsItemlevel.js"></script>

    <script>


        //Hierarchy Start Tanmoy
        function clookup_Project_LostFocus() {
            // grid.batchEditApi.StartEdit(-1, 2);  
            var gridVal = "";
            if ($("#hdAddEdit").val() == "Add") {
                grid.batchEditApi.StartEdit(-1);
                gridVal = grid.GetEditor("MainAccount").GetValue();
                grid.batchEditApi.EndEdit();
            }
            else {
                grid.batchEditApi.StartEdit(0);
                gridVal = grid.GetEditor("MainAccount").GetValue();
                grid.batchEditApi.StartEdit(0);
            }
            var ProjectId = (clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()));
            if (grid.GetVisibleRowsOnPage() > 0 && gridVal != "" && gridVal != null && ProjectId != $("#hdnGotProjectVal").val()) {
                debugger;

                jAlert("Project Change will  blank  the grid.");
                cddlInvoice.ClearItems();
                deleteAllRows();

            }


            if (ProjectId != null && clookup_Project.GetText() != "") {
                getSaleInvoiceForCustomerWithProject(GetObjectID('hdnCustomerId').value, ProjectId);
            }
            else {
                getSaleInvoiceForCustomer(GetObjectID('hdnCustomerId').value);
            }



            var projID = clookup_Project.GetValue();
            if (projID == null || projID == "") {
                $("#ddlHierarchy").val(0);
            }
            var projID = clookup_Project.GetValue();

            $.ajax({
                type: "POST",
                url: 'CustomerDebitCreditNote.aspx/getHierarchyID',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ ProjID: projID }),
                async: false,
                success: function (msg) {
                    var data = msg.d;
                    $("#ddlHierarchy").val(data);
                }
            });


        }


        function clookup_Project_GotFocus() {
            $("#hdnGotProjectVal").val(clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()));
        }

        function ProjectValueChange(s, e) {
            //debugger;
            var projID = clookup_Project.GetValue();

            $.ajax({
                type: "POST",
                url: 'CustomerDebitCreditNote.aspx/getHierarchyID',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ ProjID: projID }),
                async: false,
                success: function (msg) {
                    var data = msg.d;
                    $("#ddlHierarchy").val(data);
                }
            });
            var ProjectId = (clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()));
            if (ProjectId != null && clookup_Project.GetText() != "") {
                getSaleInvoiceForCustomerWithProject(GetObjectID('hdnCustomerId').value, ProjectId);
            }
            else {
                getSaleInvoiceForCustomer(GetObjectID('hdnCustomerId').value);
            }
        }
        //Hierarchy End Tanmoy
    </script>
    <link href="CSS/CustomerDebitCreditNote.css" rel="stylesheet" />

    <style>
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
            bottom: 6px;
            right: 20px;
            z-index: 0;
            cursor: pointer;
        }

        #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_PLSales , #dt_SaleInvoiceDue , #dtPostingDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_PLSales_B-1 , #dt_SaleInvoiceDue_B-1 , #dtPostingDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
        #dt_PLSales_B-1 #dt_PLSales_B-1Img , #dt_SaleInvoiceDue_B-1 #dt_SaleInvoiceDue_B-1Img , #dtPostingDate_B-1 #dtPostingDate_B-1Img
        {
            display: none;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        .simple-select::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 26px;
            right: 13px;
            font-size: 16px;
            transform: rotate(269deg);
            font-weight: 500;
            background: #094e8c;
            color: #fff;
            height: 18px;
            display: block;
            width: 26px;
            /* padding: 10px 0; */
            border-radius: 4px;
            text-align: center;
            line-height: 18px;
            z-index: 0;
        }
        .simple-select {
            position: relative;
                z-index: 0;
        }
        .simple-select:disabled::after
        {
            background: #1111113b;
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

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridLocationwiseStockStatus ,
        #GvJvSearch
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
                margin-top: 3px;
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

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer
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

        /*.col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }*/

        .devCheck
        {
            margin-top: 5px;
        }

        .mtc-5
        {
            margin-top: 5px;
        }

        #txtProdSearch
        {
            margin-bottom: 10px;
        }

        select.btn
        {
           position: relative;
           z-index: 0;
        }

        select
        {
            margin-bottom: 0;
        }

        .form-control
        {
            background-color: transparent;
        }

        #massrecdt
        {
            width: 100%;
        }

        .col-sm-3 , .col-md-3{
            margin-bottom: 10px;
        }

        .crossBtn
        {
            top: 25px;
                right: 25px;
        }

        input[type="text"], input[type="password"], textarea
        {
                margin-bottom: 0;
        }

        .typeNotification span
        {
             color: #ffffff !important;
        }

        #rdl_Salesquotation
        {
            margin-top: 8px;
    line-height: 20px;
        }

        #ASPxLabel8
        {
            line-height: 16px;
        }

        .lblmTop8>span, .lblmTop8>label
        {
                margin-top: 0 !important;
        }

        #OFDBankSelect
        {
            height: 30px;
            border-radius: 4px;
        }

        .mt-28{
            margin-top: 28px;
        }

        .mb-10{
            margin-bottom: 10px;
        }

        /*.col-md-3>label, .col-md-3>span
        {
            margin-top: 0;
        }*/

        #CallbackPanel_LPV
        {
            top: 450px !important;
        }

        /*.GridViewArea
        {
            z-index: 0;
        }*/

        select.btn
        {
            height: 34px !important;
        }

        .makeFullscreen >table
        {
            z-index: 0;
        }
        .makeFullscreen .makeFullscreen-icon.half
        {
                z-index: 0;
        }

        .lblmBot4 > span, .lblmBot4 > label
        {
                margin-bottom: 0px !important;
        }

        .col-md-3>label, .col-md-3>span
        {
            margin-top: 0px !important;
        }

        /*Rev end 1.0*/
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-title clearfix" id="myDiv">
        <h3 class="pull-left">
            <label id="TxtHeaded">Add Customer Debit/Credit Note</label>
        </h3>
    </div>
        <div id="ApprovalCross" runat="server" class="crossBtn"><a href="CustomerNoteList.aspx"><i class="fa fa-times"></i></a></div>

        <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
        <TabPages>
            <dxe:TabPage Name="General" Text="General">
                <ContentCollection>
                    <dxe:ContentControl runat="server">
                        <div style=" padding: 8px 0; margin-bottom: 0px; border-radius: 4px;" class="clearfix">
                            <div class="col-md-3">
                                <label>Note Type</label>
                                <div>
                                    <dxe:ASPxComboBox ID="ddlNoteType" runat="server" TabIndex="1" Width="100%" ClientInstanceName="cddlNoteType">
                                        <Items>
                                            <dxe:ListEditItem Text="Debit Note" Value="Dr" Selected="true" />
                                            <dxe:ListEditItem Text="Credit Note" Value="Cr" />
                                        </Items>
                                        <ClientSideEvents SelectedIndexChanged="Type_Changed" />
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                            <div id="divNumberingScheme" class="col-md-3">
                                <label>Select Numbering Scheme</label>
                                <div id="div_Edit">
                                    <div>
                                        <dxe:ASPxComboBox ID="CmbScheme" runat="server" TabIndex="2" ClientInstanceName="cCmbScheme"
                                            Width="100%">
                                            <ClientSideEvents SelectedIndexChanged="CmbScheme_ValueChange" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2 lblmTop8">
                                <label>Document No.</label>
                                <div>
                                    <dxe:ASPxTextBox ID="txtVoucherNo" ClientInstanceName="ctxtVoucherNo" runat="server" Width="95%" meta:resourcekey="txtBillNoResource1" TabIndex="3" MaxLength="30" onchange="txtBillNo_TextChanged()"></dxe:ASPxTextBox>
                                    <span id="MandatoryBillNo" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                    <span id="duplicateMandatoryBillNo" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Duplicate Journal No."></span>
                                </div>
                            </div>
                            <div class="col-md-2 lblmTop8">
                                <label style="">Posting Date</label>
                                <div>
                                    <dxe:ASPxDateEdit ID="dtTDate" runat="server" EditFormat="Custom" ClientInstanceName="cdtTDate" TabIndex="4" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                                        UseMaskBehavior="True" Width="100%" meta:resourcekey="tDateResource1">
                                        <%--  <ClientSideEvents DateChanged="function(s,e){DateChange()}" />--%>
                                        <ClientSideEvents LostFocus="Posting_LostFocus" />
                                    </dxe:ASPxDateEdit>
                                    <%--Rev 1.0--%>
                                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                    <%--Rev end 1.0--%>
                                </div>
                            </div>
                            <div class="col-md-2 lblmTop8">
                                <label>Branch</label>
                                <div>
                                    <dxe:ASPxComboBox ID="ddlBranch" runat="server" TabIndex="5" ClientInstanceName="cddlBranch"
                                        Width="100%">
                                        <ClientSideEvents SelectedIndexChanged="ddlBranch_Change" />
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>


                            <div style="clear: both;"></div>
                            <div class="col-md-3 lblmTop8 relative">
                                <label>
                                    Customer<span style="color: red">*</span>
                                    <% if (false)
                                       { %>
                                    <i id="openlink" class="fa fa-plus-circle ml5" aria-hidden="true"></i>

                                    <% 
                                       } 
                                    %>
                                </label>
                                <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%">
                                    <Buttons>
                                        <dxe:EditButton>
                                        </dxe:EditButton>
                                    </Buttons>
                                    <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){Customer_KeyDown(s,e);}" />
                                </dxe:ASPxButtonEdit>
                                <span id="MandatorysCustomer" class="iconCustomer pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            </div>
                            <div class="col-md-3" id="div_InvoiceNo" style="display: none">
                                <label>
                                    <dxe:ASPxLabel ID="txtPartyInvoiceNo" runat="server" Text="Party Debit Note Number">
                                    </dxe:ASPxLabel>
                                </label>
                                <dxe:ASPxTextBox ID="txtPartyInvoice" runat="server" Width="100%">
                                </dxe:ASPxTextBox>
                            </div>
                            <div class="col-md-3" id="div_InvoiceDate" style="display: none">

                                <label>
                                    <dxe:ASPxLabel ID="ASPxLabel10" runat="server" Text="Party Debit Note Date">
                                    </dxe:ASPxLabel>

                                </label>

                                <dxe:ASPxDateEdit ID="dt_PartyDate" runat="server" EditFormat="Custom" DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy" ClientInstanceName="cPLPartyDate" Width="100%">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </div>
                            <div class="col-md-3">
                                <div class="row">
                                    <div class="col-md-6 lblmTop8 lblmBot4">
                                        <dxe:ASPxLabel ID="lbl_Currency" runat="server" Text="Currency">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxComboBox ID="ddlCurrency" ClientInstanceName="cddlCurrency" runat="server" Width="95%">
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div class="col-md-6 lblmTop8 lblmBot4">
                                        <dxe:ASPxLabel ID="lbl_Rate" runat="server" Text="Exch. Rate">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxTextBox ID="txtRate" ClientInstanceName="ctxt_Rate" runat="server" Width="100%">
                                            <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>
                                            <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                                        </dxe:ASPxTextBox>
                                    </div>

                                </div>
                            </div>
                            <div class="col-md-3 ">
                                <label>Reason For Issuing document</label>
                                <div>
                                    <dxe:ASPxComboBox ID="ddl_Reason" runat="server" ClientInstanceName="cddl_Reason"
                                        Width="100%">
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                             <div class="col-md-3">
                                <label>
                                    <dxe:ASPxLabel ID="lblInvoiceNo" ClientInstanceName="clblInvoiceNo" runat="server" Text="Ref. Sale Invoice No.">
                                    </dxe:ASPxLabel>

                                </label>
                                <dxe:ASPxComboBox ID="ddlInvoice" runat="server" ClientInstanceName="cddlInvoice" Width="100%">
                                </dxe:ASPxComboBox>
                            </div>
                            <div class="clear"></div>
                            <div class="col-md-3" id="dvProject">
                                <label id="lblProject" runat="server">Project</label>
                                <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="EntityServerModeDataCustDbCr"
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
                                    <ClientSideEvents GotFocus="clookup_Project_GotFocus" CloseUp="clookup_Project_LostFocus" />
                                    <%--  ValueChanged="ProjectValueChange"--%>

                                    <%--  <ClearButton DisplayMode="Always">
                                    </ClearButton>--%>
                                </dxe:ASPxGridLookup>
                                <dx:LinqServerModeDataSource ID="EntityServerModeDataCustDbCr" runat="server" OnSelecting="EntityServerModeDataCustDbCr_Selecting"
                                    ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />
                            </div>
                            <div class="col-md-4">
                                <label>
                                    <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="Hierarchy">
                                    </dxe:ASPxLabel>
                                </label>
                                <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%" Enabled="false">
                                </asp:DropDownList>
                            </div>                           

                            <%--<div class="clear pdbot30"></div>--%>
                            <div class="clear"></div>
                            <div class="col-md-2  " id="DivSegment1" runat="server">
                                <dxe:ASPxLabel ID="ASPxLabel14" runat="server" Text="Segment1">
                                </dxe:ASPxLabel>

                                <dxe:ASPxButtonEdit ID="txtSegment1" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment1" Width="100%" TabIndex="5">
                                    <Buttons>
                                        <dxe:EditButton>
                                        </dxe:EditButton>
                                    </Buttons>
                                    <ClientSideEvents ButtonClick="function(s,e){Segment1ButnClick();}" KeyDown="function(s,e){Segment1_KeyDown(s,e);}" />
                                </dxe:ASPxButtonEdit>

                            </div>
                            <div class="col-md-2  " id="DivSegment2" runat="server">
                                <dxe:ASPxLabel ID="ASPxLabel15" runat="server" Text="Segment2">
                                </dxe:ASPxLabel>
                                <dxe:ASPxButtonEdit ID="txtSegment2" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment2" Width="100%" TabIndex="5">
                                    <Buttons>
                                        <dxe:EditButton>
                                        </dxe:EditButton>
                                    </Buttons>
                                    <ClientSideEvents ButtonClick="function(s,e){Segment2ButnClick();}" KeyDown="function(s,e){Segment2_KeyDown(s,e);}" />
                                </dxe:ASPxButtonEdit>
                            </div>
                            <div class="col-md-2  " id="DivSegment3" runat="server">
                                <dxe:ASPxLabel ID="ASPxLabel16" runat="server" Text="Segment3">
                                </dxe:ASPxLabel>

                                <dxe:ASPxButtonEdit ID="txtSegment3" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment3" Width="100%" TabIndex="5">
                                    <Buttons>
                                        <dxe:EditButton>
                                        </dxe:EditButton>
                                    </Buttons>
                                    <ClientSideEvents ButtonClick="function(s,e){Segment3ButnClick();}" KeyDown="function(s,e){Segment3_KeyDown(s,e);}" />
                                </dxe:ASPxButtonEdit>

                            </div>
                            <div class="col-md-2  " id="DivSegment4" runat="server">
                                <dxe:ASPxLabel ID="ASPxLabel17" runat="server" Text="Segment4">
                                </dxe:ASPxLabel>

                                <dxe:ASPxButtonEdit ID="txtSegment4" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment4" Width="100%" TabIndex="5">
                                    <Buttons>
                                        <dxe:EditButton>
                                        </dxe:EditButton>
                                    </Buttons>
                                    <ClientSideEvents ButtonClick="function(s,e){Segment4ButnClick();}" KeyDown="function(s,e){Segment4_KeyDown(s,e);}" />
                                </dxe:ASPxButtonEdit>

                            </div>
                            <div class="col-md-2  " id="DivSegment5" runat="server">
                                <dxe:ASPxLabel ID="ASPxLabel18" runat="server" Text="Segment5">
                                </dxe:ASPxLabel>

                                <dxe:ASPxButtonEdit ID="txtSegment5" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment5" Width="100%" TabIndex="5">
                                    <Buttons>
                                        <dxe:EditButton>
                                        </dxe:EditButton>
                                    </Buttons>
                                    <ClientSideEvents ButtonClick="function(s,e){Segment5ButnClick();}" KeyDown="function(s,e){Segment5_KeyDown(s,e);}" />
                                </dxe:ASPxButtonEdit>

                            </div>
                            <div class="clear"></div>
                        </div>




                        <div class="relative">
                            <dxe:ASPxGridView runat="server" ClientInstanceName="grid" ID="grid" KeyFieldName="Note_Id"
                                OnBatchUpdate="grid_BatchUpdate"
                                OnCellEditorInitialize="grid_CellEditorInitialize"
                                OnDataBinding="grid_DataBinding"
                                Width="100%" Settings-ShowFooter="true"
                                SettingsBehavior-AllowSort="false"
                                SettingsBehavior-AllowDragDrop="false"
                                SettingsPager-Mode="ShowAllRecords"
                                Settings-VerticalScrollBarMode="auto"
                                Settings-VerticalScrollableHeight="170"
                                OnRowInserting="Grid_RowInserting"
                                OnRowUpdating="Grid_RowUpdating"
                                OnRowDeleting="Grid_RowDeleting">
                                <SettingsPager Visible="false"></SettingsPager>

                                <Settings ShowStatusBar="Hidden" />

                                <Styles>
                                    <Cell Wrap="False"></Cell>
                                </Styles>

                                <Columns>
                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="4%" VisibleIndex="0" Caption="">
                                        <CustomButtons>
                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                            </dxe:GridViewCommandColumnCustomButton>
                                        </CustomButtons>
                                    </dxe:GridViewCommandColumn>
                                    <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl" ReadOnly="true" VisibleIndex="1" Width="3%">
                                        <PropertiesTextEdit>
                                        </PropertiesTextEdit>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataButtonEditColumn FieldName="MainAccount" Caption="Main Account" VisibleIndex="2">
                                        <PropertiesButtonEdit>
                                            <ClientSideEvents ButtonClick="MainAccountButnClick" KeyDown="MainAccountKeyDown" />
                                            <Buttons>
                                                <dxe:EditButton Text="..." Width="20px">
                                                </dxe:EditButton>
                                            </Buttons>
                                        </PropertiesButtonEdit>
                                    </dxe:GridViewDataButtonEditColumn>


                                    <dxe:GridViewDataButtonEditColumn FieldName="bthSubAccount" Caption="Sub Account" VisibleIndex="3">
                                        <PropertiesButtonEdit>
                                            <ClientSideEvents ButtonClick="SubAccountButnClick" KeyDown="SubAccountKeyDown" />
                                            <Buttons>
                                                <dxe:EditButton Text="..." Width="20px">
                                                </dxe:EditButton>
                                            </Buttons>
                                        </PropertiesButtonEdit>
                                    </dxe:GridViewDataButtonEditColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Amount" FieldName="btnRecieve" Width="130" HeaderStyle-HorizontalAlign="Right">
                                        <PropertiesTextEdit Style-HorizontalAlign="Right">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                            <ClientSideEvents KeyDown="OnKeyDown" LostFocus="DebitLostFocus"
                                                GotFocus="function(s,e){
                                                                        DebitGotFocus(s,e); 
                                                                        }" />
                                            <ClientSideEvents />
                                            <ValidationSettings Display="Dynamic">
                                            </ValidationSettings>
                                        </PropertiesTextEdit>

                                        <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataButtonEditColumn FieldName="TaxAmount" Caption="Charges" VisibleIndex="5" Width="10%" HeaderStyle-HorizontalAlign="Right">
                                        <PropertiesButtonEdit Style-HorizontalAlign="Right">
                                            <ClientSideEvents ButtonClick="taxAmtButnClick" KeyDown="TaxAmountKeyDown" />
                                            <Buttons>
                                                <dxe:EditButton Text="..." Width="20px">
                                                </dxe:EditButton>
                                            </Buttons>
                                            <MaskSettings Mask="&lt;-999999999999..999999999999&gt;.&lt;00..99&gt;" />
                                            <ValidationSettings Display="Dynamic">
                                            </ValidationSettings>
                                        </PropertiesButtonEdit>
                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataButtonEditColumn>

                                    <dxe:GridViewDataTextColumn FieldName="NetAmount" Caption="Net Amount" VisibleIndex="6" Width="12%" HeaderStyle-HorizontalAlign="Right">
                                        <PropertiesTextEdit Style-HorizontalAlign="Right">
                                            <MaskSettings Mask="&lt;-999999999999..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ValidationSettings Display="Dynamic">
                                            </ValidationSettings>
                                        </PropertiesTextEdit>
                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn VisibleIndex="7" Caption="Remarks" FieldName="btnLineNarration" Width="160">
                                        <PropertiesTextEdit>
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="30" VisibleIndex="8" Caption=" ">
                                        <CustomButtons>
                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="AddNew" Image-Url="/assests/images/add.png">
                                                <Image Url="/assests/images/add.png">
                                                </Image>
                                            </dxe:GridViewCommandColumnCustomButton>
                                        </CustomButtons>
                                    </dxe:GridViewCommandColumn>
                                    <dxe:GridViewDataTextColumn FieldName="HSNCODE" Caption="HSN" ReadOnly="true" HeaderStyle-CssClass="hide" Width="0">
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="Note_Id" Caption="Srl No" ReadOnly="true" HeaderStyle-CssClass="hide" Width="0">
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="gvColMainAccount" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="gvColSubAccount" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="IsSubledger" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="UpdateEdit" Caption="hidden Field Id" Width="0" HeaderStyle-CssClass="hide">
                                    </dxe:GridViewDataTextColumn>
                                </Columns>
                                <ClientSideEvents Init="OnInit" EndCallback="OnEndCallback" BatchEditStartEditing="OnBatchEditStartEditing"
                                    CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" />
                                <SettingsDataSecurity AllowEdit="true" />
                                <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                    <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="Row" />
                                </SettingsEditing>
                                <Styles>
                                    <StatusBar CssClass="statusBar">
                                    </StatusBar>
                                </Styles>
                            </dxe:ASPxGridView>
                        </div>


                        <div class="text-center">
                            <table style="margin-left: 193px; margin-top: 5px; margin-bottom: 5px">

                                <tr>
                                    <td style="padding-right: 6px; width: 150px;">Taxable Amount</td>
                                    <td>
                                        <dxe:ASPxTextBox ID="txtTaxableAmount" runat="server" Width="105px" ClientInstanceName="c_txtTaxableAmount" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                        </dxe:ASPxTextBox>
                                    </td>
                                    <td style="padding-right: 16px; width: 150px;">Tax Amount</td>
                                    <td>
                                        <dxe:ASPxTextBox ID="txtTaxAmount" runat="server" Width="105px" ClientInstanceName="c_txtTaxAmount" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                        </dxe:ASPxTextBox>
                                    </td>
                                    <td style="padding-right: 16px; width: 150px;">Total Amount</td>
                                    <td>
                                        <dxe:ASPxTextBox ID="txt_Debit" runat="server" Width="105px" ClientInstanceName="c_txt_Debit" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                        </dxe:ASPxTextBox>
                                    </td>
                                    <td style="display: none;">
                                        <dxe:ASPxTextBox ID="txt_Credit" runat="server" Width="105px" ClientInstanceName="c_txt_Credit" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                        </dxe:ASPxTextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="clearfix" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;">
                            <div class="col-md-12">
                                <label>Main Narration</label>
                                <div>
                                    <asp:TextBox ID="txtNarration" Font-Names="Arial" runat="server" TextMode="MultiLine"
                                        Width="100%" meta:resourcekey="txtNarrationResource1" Height="40px"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div>

                            <b><span id="tagged" runat="server" style="display: none; color: red">This Customer Debit/Credit Note is tagged with Document : <span id="spanTaggedDocNo"></span>. Cannot Modify data!!</span></b>
                            <dxe:ASPxButton ID="btnSaveRecords" ClientInstanceName="cbtnSaveNew" runat="server" AutoPostBack="False" Text="S&#818;ave & New" CssClass="btn btn-success" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {SaveButtonClickNew();}" />
                            </dxe:ASPxButton>
                            <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtnSaveRecords" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-success" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {SaveButtonClick();}" />
                            </dxe:ASPxButton>
                            <dxe:ASPxButton ID="btnUDF" ClientInstanceName="cbtnUDF" runat="server" AutoPostBack="False" Text="U&#818;DF" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {if(OpenUdf()){ return false}}" />
                            </dxe:ASPxButton>
                        </div>








                    </dxe:ContentControl>
                </ContentCollection>

            </dxe:TabPage>
            <dxe:TabPage Name="Billing/Shipping" Text="Billing/Shipping">
                <ContentCollection>
                    <dxe:ContentControl runat="server">
                        <ucbs:Sales_BillingShipping runat="server" ID="BillingShippingControl" />
                        <asp:HiddenField runat="server" ID="hfTermsConditionDocType" Value="CN" />
                    </dxe:ContentControl>
                </ContentCollection>
            </dxe:TabPage>
        </TabPages>
        <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab = page.GetActiveTab();
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
    <div class="modal fade" id="Segment1Model" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment1header"></h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Segment1keydown(event)" id="txtSegment1Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />
                    <div id="Segment1Table">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Code</th>
                                <th>Name</th>
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
    <div class="modal fade" id="Segment2Model" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment2Header"></h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Segment2keydown(event)" id="txtSegment2Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />
                    <div id="Segment2Table">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Code</th>
                                <th>Name</th>
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
    <div class="modal fade" id="Segment3Model" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment3Header"></h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Segment3keydown(event)" id="txtSegment3Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />
                    <div id="Segment3Table">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Code</th>
                                <th>Name</th>
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
    <div class="modal fade" id="Segment4Model" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment4Header">Segment4 Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Segment4keydown(event)" id="txtSegment4Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />
                    <div id="Segment4Table">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Code</th>
                                <th>Name</th>
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
    <div class="modal fade" id="Segment5Model" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment5Header"></h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Segment5keydown(event)" id="txtSegment5Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />
                    <div id="Segment5Table">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Code</th>
                                <th>Name</th>
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




    <%--Modal Section--%>

    <div class="modal fade" id="CustModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Customer Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search by Entity Name,Unique Id and Phone No." />
                    <div id="CustomerTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Customer Name</th>
                                <th>Unique Id</th>
                                <th>Address</th>
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




    <div class="modal fade" id="MainAccountModel" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" onclick="closeModal();">&times;</button>
                    <h4 class="modal-title">Main Account Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="MainAccountNewkeydown(event)" id="txtMainAccountSearch" autofocus width="100%" placeholder="Search by Main Account Name or Short Name" />

                    <div id="MainAccountTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th>Main Account Name</th>
                                <th>Short Name</th>
                                <th>Subledger Type</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="closeModal();">Close</button>
                </div>
            </div>
        </div>
    </div>


    <div class="modal fade" id="SubAccountModel" role="dialog" data-backdrop="static"
        data-keyboard="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" onclick="CloseSubModal();">&times;</button>
                    <h4 class="modal-title">Sub Account Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="SubAccountNewkeydown(event)" id="txtSubAccountSearch" autofocus width="100%" placeholder="Search By Sub Account Name or Code" />

                    <div id="SubAccountTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th>Sub Account Name [Unique ID]</th>
                                <th>Sub Account Code</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="CloseSubModal();">Close</button>
                </div>
            </div>

        </div>
    </div>


    <%--End Modal Structure--%>




    <%--Tax PopUp Start--%>
    <dxe:ASPxPopupControl ID="aspxTaxpopUp" runat="server" ClientInstanceName="caspxTaxpopUp"
        Width="850px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <HeaderTemplate>
            <span style="color: #fff"><strong>Select Tax</strong></span>
            <dxe:ASPxImage ID="ASPxImage31" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                <ClientSideEvents Click="function(s, e){ 
                                                            caspxTaxpopUp.Hide();
                                                        }" />
            </dxe:ASPxImage>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <asp:HiddenField runat="server" ID="setCurrentProdCode" />
                <asp:HiddenField runat="server" ID="HdSerialNo" />
                <asp:HiddenField runat="server" ID="HdSerialNo1" />
                <asp:HiddenField runat="server" ID="hdnDeleteSrlNo" Value="0" />
                <asp:HiddenField runat="server" ID="HdProdGrossAmt" />
                <asp:HiddenField runat="server" ID="HdProdNetAmt" />


                <input type="hidden" id="IsTaxApplicable" value="" />

                <div id="content-6">
                    <div class="col-sm-3">
                        <div class="lblHolder" style="margin-bottom: 8px">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Gross Amount
                                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="(Taxable)" ClientInstanceName="clblTaxableGross"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 28px">
                                            <dxe:ASPxLabel ID="lblTaxProdGrossAmt" runat="server" Text="" ClientInstanceName="clblTaxProdGrossAmt"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>

                    <div class="col-sm-3 gstGrossAmount">
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>GST</td>
                                    </tr>
                                    <tr>
                                        <td style="height: 28px">
                                            <dxe:ASPxLabel ID="lblGstForGross" runat="server" Text="" ClientInstanceName="clblGstForGross"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>

                    <div class="col-sm-3" style="display: none">
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Discount</td>
                                    </tr>
                                    <tr>
                                        <td style="height: 28px">
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
                                                    <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="(Taxable)" ClientInstanceName="clblTaxableNet"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 28px">
                                            <dxe:ASPxLabel ID="lblProdNetAmt" runat="server" Text="" ClientInstanceName="clblProdNetAmt"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>

                    <div class="col-sm-2 gstNetAmount">
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>GST</td>
                                    </tr>
                                    <tr>
                                        <td style="height: 28px">
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
                            <dxe:ASPxTextBox ID="txtprodBasicAmt" MaxLength="80" ClientInstanceName="ctxtprodBasicAmt" ReadOnly="true"
                                runat="server" Width="50%">
                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                            </dxe:ASPxTextBox>
                        </td>
                    </tr>
                    <tr class="cgridTaxClass">
                        <td colspan="3">

                            <dxe:ASPxGridView runat="server" OnBatchUpdate="taxgrid_BatchUpdate" KeyFieldName="Taxes_ID" ClientInstanceName="cgridTax" ID="aspxGridTax"
                                Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords"
                                OnCustomCallback="cgridTax_CustomCallback"
                                Settings-ShowFooter="false" AutoGenerateColumns="False"
                                OnCellEditorInitialize="aspxGridTax_CellEditorInitialize"
                                OnRowInserting="taxgrid_RowInserting" OnRowUpdating="taxgrid_RowUpdating" OnRowDeleting="taxgrid_RowDeleting">
                                <Settings VerticalScrollableHeight="150" VerticalScrollBarMode="Auto" ShowStatusBar="Hidden"></Settings>
                                <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                                <SettingsPager Visible="false"></SettingsPager>

                                <Styles>
                                    <Cell Wrap="False"></Cell>
                                </Styles>

                                <Columns>
                                    <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="Taxes_Name" ReadOnly="true" Caption="Tax Component ID">
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="taxCodeName" ReadOnly="true" Caption="Tax Component Name">
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="calCulatedOn" ReadOnly="true" Caption="Calculated On">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ValidationSettings Display="Dynamic">
                                            </ValidationSettings>
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>

                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Percentage" FieldName="TaxField" VisibleIndex="4">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ValidationSettings Display="Dynamic">
                                            </ValidationSettings>
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Amount" Caption="Amount" ReadOnly="true">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <ClientSideEvents LostFocus="taxAmountLostFocus" GotFocus="taxAmountGotFocus" />
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ValidationSettings Display="Dynamic">
                                            </ValidationSettings>
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
                        <td colspan="3" style="padding-top: 5px">
                            <div class="pull-left" id="calculateTotalAmountOK">
                                <input type="button" onclick="calculateTotalAmount()" class="btn btn-primary" value="Ok" />
                            </div>
                            <table class="pull-right">
                                <tr>
                                    <td style="padding-right: 5px"><strong>Total Charges</strong></td>
                                    <td>
                                        <dxe:ASPxTextBox ID="txtTaxTotAmt" MaxLength="80" ClientInstanceName="ctxtTaxTotAmt" Text="0.00" ReadOnly="true"
                                            runat="server" Width="100%" CssClass="pull-left mTop">
                                            <MaskSettings Mask="&lt;-999999999999..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
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

    <%--Tax popup End--%>

    <div>
        <asp:HiddenField runat="server" ID="hdAddEdit" />
        <div runat="server" id="ReceiptPaymentId" visible="false"></div>
        <asp:HiddenField runat="server" ID="hdnCustomerId" />
        <asp:HiddenField runat="server" ID="hdnRefreshType" />
        <asp:HiddenField runat="server" ID="HDItemLevelTaxDetails" />
        <asp:HiddenField runat="server" ID="HDHSNCodewisetaxSchemid" />
        <asp:HiddenField runat="server" ID="HDBranchWiseStateTax" />
        <asp:HiddenField runat="server" ID="HDStateCodeWiseStateIDTax" />
        <asp:HiddenField runat="server" ID="TaxAmountOngrid" />
        <asp:HiddenField runat="server" ID="VisibleIndexForTax" />
        <asp:HiddenField runat="server" ID="DoEdit" />
        <asp:HiddenField runat="server" ID="hdnTagCount" />
        <asp:HiddenField runat="server" ID="hdnProjectSelectInEntryModule" />

        <asp:HiddenField runat="server" ID="hdnSegment1" />
        <asp:HiddenField runat="server" ID="hdnSegment2" />
        <asp:HiddenField runat="server" ID="hdnSegment3" />
        <asp:HiddenField runat="server" ID="hdnSegment4" />
        <asp:HiddenField runat="server" ID="hdnSegment5" />

        <asp:HiddenField runat="server" ID="hdnValueSegment1" />
        <asp:HiddenField runat="server" ID="hdnValueSegment2" />
        <asp:HiddenField runat="server" ID="hdnValueSegment3" />
        <asp:HiddenField runat="server" ID="hdnValueSegment4" />
        <asp:HiddenField runat="server" ID="hdnValueSegment5" />
        <asp:HiddenField runat="server" ID="hdnDocumentSegmentSettings" />
    </div>

    <dxe:ASPxCallbackPanel runat="server" ID="deleteTax" ClientInstanceName="cdeleteTax" OnCallback="deleteTax_Callback">
        <ClientSideEvents EndCallback="deleteTaxEndCallBack" />
    </dxe:ASPxCallbackPanel>

    <dxe:ASPxLoadingPanel ID="LoadingPanelCRP" runat="server" ClientInstanceName="cLoadingPanelCRP"
        Modal="True">
    </dxe:ASPxLoadingPanel>
    <%--Rev v1.0.101  subhra  04-01-2019  0019425--%>
    <asp:HiddenField ID="hdnAutoPrint" runat="server" />
    <%--End of Rev--%>
    <asp:HiddenField ID="hdnGotProjectVal" runat="server" />
    <asp:HiddenField ID="hdnProjectMandatory" runat="server" />
    <asp:HiddenField ID="hdnLockFromDate" runat="server" />
    <asp:HiddenField ID="hdnLockToDate" runat="server" />
    <asp:HiddenField ID="hdnLockFromDateCon" runat="server" />
    <asp:HiddenField ID="hdnLockToDateCon" runat="server" />
</asp:Content>
