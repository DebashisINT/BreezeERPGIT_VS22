<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                28-04-2023        2.0.38           Pallab              25968: Add Customer Payment module design modification & check in small device
2.0                28-04-2023        2.0.38           Pallab              26397: Add Customer Payment module all bootstrap modal outside click event disable
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="CustomerPayment.aspx.cs" Inherits="ERP.OMS.Management.Activities.CustomerPayment" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<%@ Register Src="~/OMS/Management/Activities/UserControls/ucPaymentDetails.ascx" TagPrefix="uc1" TagName="ucPaymentDetails" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/Sales_BillingShipping.ascx" TagPrefix="ucBS" TagName="Sales_BillingShipping" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="JS/CustomerPayment.js?v2.0"></script>
    <link href="CSS/CustomerReceiptPayment.css?v1.00009" rel="stylesheet" />
    <script src="UserControls/Js/ucPaymentDetails.js"></script>
    <script src="JS/SearchPopup.js"></script>
    <link href="CSS/SearchPopup.css" rel="stylesheet" />

    <link href="CSS/PosSalesInvoice.css" rel="stylesheet" />

    <%-- Project Script start --%>
    <script>

        //$(function () {
        //    $("#lookup_Project").dxeLookup({
        //        onValueChanged: function (e) {
        //            var previousValue = e.previousValue;
        //            var newValue = e.value;
        //            // Event handling commands go here
        //        }
        //    });
        //});

       

    </script>
    <%-- Project Script End --%>
    <style>
        #lookup_Project .dxeButtonEditClearButton_PlasticBlue {
            display: table-cell;
        }
    </style>

    <script>
        //Hierarchy Start Tanmoy
      

           
           
           
       
        //Hierarchy End Tanmoy
    </script>

    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    

    <style>
            #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_PLSales , #dt_SaleInvoiceDue , #dtPostingDate , #InstDate
            {
                position: relative;
                z-index: 1;
                background: transparent;
            }

            #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_PLSales_B-1 , #dt_SaleInvoiceDue_B-1 , #dtPostingDate_B-1, #InstDate_B-1
            {
                background: transparent !important;
                border: none;
                width: 30px;
                padding: 10px !important;
            }

            #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
            #dt_PLSales_B-1 #dt_PLSales_B-1Img , #dt_SaleInvoiceDue_B-1 #dt_SaleInvoiceDue_B-1Img , #dtPostingDate_B-1 #dtPostingDate_B-1Img,
            #InstDate_B-1 #InstDate_B-1Img
            {
                display: none;
            }

        .calendar-icon
        {
                right: 18px !important;
        }

        /*select#ddlInventory
        {
            -webkit-appearance: auto;
        }*/

        .simple-select::after
        {
            top: 6px !important;
            right: -2px !important;
        }

        .col-sm-3 , .col-md-3 , .col-md-2{
            margin-bottom: 5px;
        }

        #rdl_Salesquotation
        {
            margin-top: 10px;
        }
        .col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }
        .lblmBot4 > span, .lblmBot4 > label
        {
                margin-bottom: 0px !important;
        }

        #drdTransCategory.aspNetDisabled {
    background: #f3f3f3 !important;
}

       /* #CustomerTableTbl.dynamicPopupTbl>tbody>tr>td
        {
            width: 33.33%;
        }*/

       .lblmTop8>span, .lblmTop8>label
        {
                margin-top: 0 !important;
        }

       input + label
       {
               margin-top: 3px;
               margin-right: 5px;
       }

       #txtVoucherNo
       {
               width: 100%;
       }

            @media only screen and (max-width: 1380px) and (min-width: 1300px)
            {

                .col-xs-1, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9, .col-xs-10, .col-xs-11, .col-xs-12, .col-sm-1, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-sm-10, .col-sm-11, .col-sm-12, .col-md-1, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-md-10, .col-md-11, .col-md-12, .col-lg-1, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-lg-10, .col-lg-11, .col-lg-12 {
                    padding-right: 10px;
                    padding-left: 10px;
                }

                /*.simple-select::after
                {
                    right: 8px !important;
                }*/
                .calendar-icon {
                    right: 13px !important;
                }

                input[type="radio"], input[type="checkbox"] {
                    margin-right: 0px;
                }
            }
        </style>
    <%--Rev end 1.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
    <div class="panel-title clearfix" id="myDiv">
        <div id="pageheaderContent" class=" pull-right wrapHolder content horizontal-images" style="width: auto !important" runat="server">
            <div class="Top clearfix">
                <ul>
                    <li>
                        <div class="lblHolder" id="divContactPhone" style="display: none;" runat="server">
                            <table>
                                <tr>
                                    <td>Contact Person's Phone</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblContactPhone" runat="server" Text="N/A" CssClass="classout"></asp:Label></td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder" id="divGSTIN" style="display: none;" runat="server">
                            <table>
                                <tr>
                                    <td>GST Registed?</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblGSTIN" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder" id="custBal" runat="server">
                            <table>
                                <tr>
                                    <td>Customer Balance</td>
                                </tr>
                                <tr>
                                    <td>
                                        <a href="#" id="idOutstanding">
                                            <asp:Label ID="lblOutstanding" runat="server" ToolTip="Click here to show details."></asp:Label>

                                        </a>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
        <h3 class="pull-left">
            <label id="TxtHeaded">Add Customer Payment</label>
        </h3>
    </div>



    <div id="ApprovalCross" runat="server" class="crossBtn"><a href="CustomerReceiptPaymentList.aspx"><i class="fa fa-times"></i></a></div>



    <%--        <div class="tab">
            <input type="button" class="tablinks" value="General" onclick="ShowHideTab(event, 'General'); return false" />
            <input type="button" class="tablinks" value="Billing/Shipping" onclick="ShowHideTab(event, 'BS'); return false" />
        </div>--%>

    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
        <TabPages>
            <dxe:TabPage Name="General" Text="General">
                <ContentCollection>
                    <dxe:ContentControl runat="server">
                        <div id="General" class="tabcontent" style="display: block;">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="">
                                        <div style=" padding: 5px 0 5px 0; margin-bottom: 10px; border-radius: 4px; ">
                                            <div class="col-md-2" id="divNumberingScheme" runat="server">
                                                <label style="">Numbering Scheme</label>
                                                <div>
                                                    <dxe:ASPxComboBox ID="CmbScheme" ClientInstanceName="cCmbScheme"
                                                        SelectedIndex="0" EnableCallbackMode="false"
                                                        TextField="SchemaName" ValueField="ID"
                                                        runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                                        <ClientSideEvents SelectedIndexChanged="CmbScheme_ValueChange"></ClientSideEvents>
                                                    </dxe:ASPxComboBox>
                                                    <span id="MandatoryNumberingScheme" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                </div>
                                            </div>

                                            <div class="col-md-2">
                                                <label style="">Document No.</label>
                                                <div>

                                                    <dxe:ASPxTextBox runat="server" ID="txtVoucherNo" ClientInstanceName="ctxtVoucherNo" MaxLength="16" Text="Auto" ClientEnabled="false">
                                                    </dxe:ASPxTextBox>

                                                    <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; margin-top: -21px; margin-left: 169px; display: none"
                                                        title="Mandatory"></span>

                                                </div>
                                            </div>


                                            <div class="col-md-2">
                                                <label style="">Posting Date</label>
                                                <div>
                                                    <dxe:ASPxDateEdit ID="dtTDate" runat="server" ClientInstanceName="cdtTDate" EditFormat="Custom" AllowNull="false"
                                                        Font-Size="12px" UseMaskBehavior="True" Width="100%" EditFormatString="dd-MM-yyyy" CssClass="pull-left">
                                                        <ButtonStyle Width="13px"></ButtonStyle>
                                                        <ClientSideEvents LostFocus="function(s, e) { SetLostFocusonDemand(e)}"></ClientSideEvents>
                                                    </dxe:ASPxDateEdit>
                                                    <span id="MandatoryTransDate" class="iconTransDate  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                    <%--Rev 1.0--%>
                                                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                                    <%--Rev end 1.0--%>
                                                </div>
                                            </div>

                                            <div class="col-md-2 lblmTop8">
                                                <label>For Unit <span style="color: red">*</span></label>
                                                <div>
                                                    <dxe:ASPxComboBox ID="ddlBranch" runat="server" ClientInstanceName="cddlBranch"
                                                        Width="100%">
                                                        <ClientSideEvents SelectedIndexChanged="ddlBranch_Change" />
                                                    </dxe:ASPxComboBox>
                                                    <span id="MandatoryBranch" class="iconBranch pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>


                                                </div>
                                            </div>

                                            <div class="col-md-2 lblmTop8 relative">
                                                <label class="btn-block" style="margin-bottom: 0;">
                                                    <%--<dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Customer">--%>
                                                    <%--</dxe:ASPxLabel>--%>

                                                    <asp:RadioButtonList ID="rdl_Contact" runat="server" RepeatDirection="Horizontal" onchange="return selectContactValue();" CssClass="pull-left">

                                                        <asp:ListItem Text="Customer" Value="CL" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Vendor" Value="DV" Enabled="false"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                    <span style="color: red">*</span>
                                                    <% if (false)
                                                       { %>
                                                    <i id="openlink" class="fa fa-plus-circle" aria-hidden="true"></i>

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

                                            <div class="col-md-2 lblmTop8">
                                                <label>Contact Person </label>
                                                <div>
                                                    <dxe:ASPxComboBox ID="ddlContactPerson" runat="server" ClientInstanceName="cddlContactPerson" Width="100%">
                                                    </dxe:ASPxComboBox>


                                                </div>
                                            </div>
                                            <div class="clear"></div>

                                            <div class="col-md-2 lblmTop8" id="tdCashBankLabel">
                                                <label>Cash Bank <span style="color: red">*</span></label>
                                                <div>
                                                    <dxe:ASPxComboBox ID="ddlCashBank" runat="server" ClientInstanceName="cddlCashBank" Width="100%">
                                                        <ClientSideEvents SelectedIndexChanged="CashBank_SelectedIndexChanged" />
                                                    </dxe:ASPxComboBox>

                                                    <span id="MandatoryCashBank" class="iconCashBank pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                                                </div>
                                            </div>


                                            <div class="col-md-2 lblmTop8">
                                                <label>Currency <span style="color: red">*</span></label>
                                                <div>
                                                    <dxe:ASPxComboBox ID="ddlCurrency" runat="server" ClientInstanceName="cddlCurrency" Width="100%">
                                                        <ClientSideEvents SelectedIndexChanged="Currency_Rate" />
                                                    </dxe:ASPxComboBox>


                                                </div>
                                            </div>

                                            <div class="col-md-2 lblmTop8">
                                                <label>Rate <span style="color: red">*</span></label>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtRate" runat="server" ClientInstanceName="ctxtRate" Width="100%" ClientEnabled="false" Text="0.00">
                                                    </dxe:ASPxTextBox>


                                                </div>
                                            </div>


                                            <div id="multipleredio" class="col-md-2" runat="server">
                                                <div style="padding-top: 13px; margin-top: 10px">
                                                    <asp:RadioButtonList ID="rdl_MultipleType" runat="server" Width="160px" RepeatDirection="Horizontal" onchange="return selectValue();">
                                                        <asp:ListItem Text="Single" Value="S" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Multiple" Value="M"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                            </div>

                                            <div class="clear" id="ClearSingle" style="display: none; !important"></div>
                                            <div id="singletype" runat="server">

                                                <div class="col-md-2 lblmTop8">
                                                    <label style="">Instrument Type</label>
                                                    <%--Rev 1.0: "simple-select" class add--%>
                                                    <div style="" class="simple-select">
                                                        <dxe:ASPxComboBox ID="cmbInstrumentType" runat="server" ClientInstanceName="cComboInstrumentTypee" Font-Size="12px"
                                                            ValueType="System.String" Width="100%" EnableIncrementalFiltering="True" Native="true">
                                                            <Items>

                                                                <dxe:ListEditItem Text="Cheque" Value="C" Selected />
                                                                <dxe:ListEditItem Text="Draft" Value="D" />
                                                                <dxe:ListEditItem Text="E.Transfer" Value="E" />
                                                                <dxe:ListEditItem Text="Cash" Value="CH" />
                                                                <dxe:ListEditItem Text="Card" Value="CRD" />
                                                                 <dxe:ListEditItem Text="Pay Order" Value="PO" />
                                                            </Items>
                                                            <ClientSideEvents SelectedIndexChanged="InstrumentTypeSelectedIndexChanged" />
                                                        </dxe:ASPxComboBox>

                                                    </div>
                                                </div>

                                                <div class="col-md-2 lblmTop8" id="divInstrumentNo" style="" runat="server">
                                                    <label id="" style="">Instrument No</label>
                                                    <div id="">
                                                        <dxe:ASPxTextBox runat="server" ID="txtInstNobth" ClientInstanceName="ctxtInstNobth" Width="100%" MaxLength="30">
                                                        </dxe:ASPxTextBox>
                                                    </div>
                                                </div>
                                                <div class="clear"></div>
                                                <div class="col-md-2 lblmTop8" id="tdIDateDiv" style="" runat="server">
                                                    <label id="tdIDateLable" style="">Instrument Date</label>
                                                    <div id="tdIDateValue" style="">
                                                        <dxe:ASPxDateEdit ID="InstDate" runat="server" EditFormat="Custom" ClientInstanceName="cInstDate"
                                                            UseMaskBehavior="True" Font-Size="12px" Width="100%" EditFormatString="dd-MM-yyyy">
                                                           <ClientSideEvents GotFocus="PutDate" />
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                        </dxe:ASPxDateEdit>
                                                        <%--Rev 1.0--%>
                                                        <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                                        <%--Rev end 1.0--%>
                                                    </div>
                                                </div>
                                                <div class="col-md-3" id="divDrawnOn" style="" runat="server">
                                                    <label id="" style="">Drawn on ( Party banker’s name )</label>
                                                    <div id="">
                                                        <dxe:ASPxTextBox runat="server" ID="txtDrawnOn" ClientInstanceName="ctxtDrawnOn" Width="100%" MaxLength="30">
                                                        </dxe:ASPxTextBox>
                                                    </div>
                                                </div>

                                            </div>


                                            <div class="col-md-5 lblmTop8">
                                                <label>Narration </label>
                                                <div>
                                                    <asp:TextBox ID="txtNarration" runat="server" MaxLength="500"
                                                        TextMode="MultiLine"
                                                        Width="100%"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2 lblmTop8">
                                                <label>Voucher Amount <span style="color: red">*</span> </label>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtVoucherAmount" runat="server" ClientInstanceName="ctxtVoucherAmount" Width="100%" CssClass="pull-left">
                                                        <ClientSideEvents TextChanged="function(s, e) { GetInvoiceMsg(e)}" LostFocus="lostFocusVoucherAmount" />
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                        <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>
                                                    </dxe:ASPxTextBox>
                                                    <span id="MandatoryVoucherAmount" class="iconVoucherAmount pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                </div>
                                            </div>

                                            <div class="clear"></div>
                                            <div class="col-md-2 lblmTop8">
                                                <label id="lblProject" runat="server">Project</label>
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
                                                    <ClientSideEvents GotFocus="cProject_GotFocus" CloseUp="Project_LostFocus" />
                                                  <%--  <ClearButton DisplayMode="Always">
                                                    </ClearButton>--%>
                                                   <%-- GotFocus="function(s,e){clookup_Project.ShowDropDown();}"--%>
                                                 <%--   ValueChanged="ProjectValueChange"--%>
                                                 <%--   ClientSideEvents-TextChanged="ProjectCodeinlineSelectedPayment"--%>
                                                </dxe:ASPxGridLookup>
                                                <dx:LinqServerModeDataSource ID="ProjectServerModeDataSource" runat="server" OnSelecting="ProjectServerModeDataSource_Selecting"
                                                    ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />

                                            </div>

                                            <div class="col-md-3">
                                                <label class="">
                                                    <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="Hierarchy">
                                                    </dxe:ASPxLabel>
                                                </label>
                                                <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%" Enabled="false">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2 lblmTop8 hide ">
                                                <label>Ref. Proforma </label>
                                                <div>
                                                    <dxe:ASPxComboBox ID="ddlProformaInvoice" runat="server" ClientInstanceName="cddlProformaInvoice" Width="100%">
                                                    </dxe:ASPxComboBox>
                                                </div>
                                            </div>
                                            <div class="clear"></div>
                                            <div id="Multipletype" style="display: none" class="bgrnd" runat="server">
                                                <uc1:ucPaymentDetails runat="server" ID="PaymentDetails" />
                                            </div>


                                            <div class="clear"></div>
                                        </div>
                                        <dxe:ASPxGridView runat="server" ClientInstanceName="grid" ID="grid"
                                            Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" Settings-ShowFooter="false"
                                            SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto"
                                            OnCellEditorInitialize="grid_CellEditorInitialize" OnCustomCallback="grid_CustomCallback"
                                            OnRowInserting="Grid_RowInserting"
                                            OnRowUpdating="Grid_RowUpdating"
                                            OnRowDeleting="Grid_RowDeleting"
                                            OnBatchUpdate="grid_BatchUpdate"
                                            OnDataBinding="grid_DataBinding"
                                            KeyFieldName="PaymentDetail_ID"
                                            Settings-VerticalScrollableHeight="170" CommandButtonInitialize="false" Settings-ShowStatusBar="Hidden">
                                            <SettingsPager Visible="false"></SettingsPager>
                                            <Styles>
                                                <StatusBar CssClass="statusBar">
                                                </StatusBar>
                                            </Styles>
                                            <Columns>
                                                <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="3%" VisibleIndex="0" Caption=" ">
                                                    <CustomButtons>
                                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                                        </dxe:GridViewCommandColumnCustomButton>
                                                    </CustomButtons>
                                                </dxe:GridViewCommandColumn>
                                                <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl#" ReadOnly="true" VisibleIndex="1" Width="5%">
                                                    <PropertiesTextEdit>
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataButtonEditColumn ReadOnly="true" FieldName="Type" Caption="Type" VisibleIndex="2">

                                                    <PropertiesButtonEdit>

                                                        <ClientSideEvents ButtonClick="TypeButnClick" KeyDown="TypeKeyDown" />
                                                        <Buttons>

                                                            <dxe:EditButton Text="..." Width="20px">
                                                            </dxe:EditButton>
                                                        </Buttons>
                                                    </PropertiesButtonEdit>
                                                </dxe:GridViewDataButtonEditColumn>

                                                <dxe:GridViewDataButtonEditColumn ReadOnly="true" FieldName="DocumentNo" Caption="Document No." VisibleIndex="3">

                                                    <PropertiesButtonEdit>

                                                        <ClientSideEvents ButtonClick="DocumentNoButnClick" KeyDown="DocumentNoKeyDown" />
                                                        <Buttons>

                                                            <dxe:EditButton Text="..." Width="20px">
                                                            </dxe:EditButton>
                                                        </Buttons>
                                                    </PropertiesButtonEdit>
                                                </dxe:GridViewDataButtonEditColumn>

                                                <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Payment" ReadOnly="false" FieldName="Payment" Width="130">
                                                    <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                        <ClientSideEvents LostFocus="Payment_LostFocus"></ClientSideEvents>
                                                        <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>

                                                    </PropertiesTextEdit>
                                                    <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                                </dxe:GridViewDataTextColumn>


                                                <%--  Chinmoy Added new column Project Code Start 13-12-2019--%>

                                                <dxe:GridViewDataButtonEditColumn FieldName="Project_Code" Caption="Project Code" VisibleIndex="5" Width="14%">
                                                    <PropertiesButtonEdit>
                                                        <ClientSideEvents ButtonClick="ProjectCodeButnClick" KeyDown="ProjectCodeKeyDown" />
                                                        <Buttons>
                                                            <dxe:EditButton Text="..." Width="20px">
                                                            </dxe:EditButton>
                                                        </Buttons>
                                                    </PropertiesButtonEdit>
                                                </dxe:GridViewDataButtonEditColumn>


                                                <%-- End--%>





                                                <dxe:GridViewDataTextColumn VisibleIndex="6" Caption="Remarks" ReadOnly="false" FieldName="Remarks" Width="200">
                                                    <PropertiesTextEdit>
                                                    </PropertiesTextEdit>
                                                    <CellStyle Wrap="False" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="ProjectId" Caption="ProjectId" VisibleIndex="16" ReadOnly="True" Width="0"
                                                    EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide"
                                                    PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                </dxe:GridViewDataTextColumn>


                                                <dxe:GridViewDataTextColumn FieldName="PaymentDetail_ID" Caption="Srl No" ReadOnly="true" Width="0">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="IsOpening" Caption="hidden Field Id" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="ActualAmount" Caption="hidden Field Id" VisibleIndex="17" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="TypeId" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide">
                                                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="DocId" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide">
                                                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="UpdateEdit" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" Width="0">
                                                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="2.5%" VisibleIndex="6" Caption=" ">
                                                    <CustomButtons>
                                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="AddNew" Image-Url="/assests/images/add.png">
                                                        </dxe:GridViewCommandColumnCustomButton>
                                                    </CustomButtons>
                                                </dxe:GridViewCommandColumn>


                                            </Columns>



                                            <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" BatchEditStartEditing="GetVisibleIndex" />
                                            <SettingsDataSecurity AllowEdit="true" />
                                            <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                                <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                            </SettingsEditing>
                                        </dxe:ASPxGridView>
                                        <div class="clear"></div>
                                        <div class="text-center">
                                            <table style="margin-left: 30%; margin-top: 10px">
                                                <tr>
                                                    <td style="padding-right: 50px"><b>Total Amount</b></td>
                                                    <td style="width: 203px;">
                                                        <dxe:ASPxTextBox ID="txtTotalAmount" runat="server" Width="105px" ClientInstanceName="c_txt_Debit" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">

                                                            <MaskSettings Mask="&lt;0..999999999999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                        </dxe:ASPxTextBox>
                                                    </td>
                                                </tr>
                                                <tr style="display: none">
                                                    <td style="padding-right: 30px">
                                                        <asp:Label ID="lbltaxAmountHeader" runat="server" Text="Total Taxable Amount" Font-Bold="true"></asp:Label></td>
                                                    <td>
                                                        <dxe:ASPxTextBox ID="txtTaxAmount" runat="server" Width="105px" ClientInstanceName="ctxtTaxAmount" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                            <MaskSettings Mask="&lt;0..999999999999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                        </dxe:ASPxTextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div class="content reverse horizontal-images clearfix" style="margin-top: 8px; width: 100%; margin-right: 0; padding: 8px; height: auto; border-top: 1px solid #ccc; border-bottom: 1px solid #ccc; border-radius: 0;">
                                                <ul>
                                                    <li class="clsbnrLblTaxableAmt" style="float: right;">
                                                        <div class="horizontallblHolder">
                                                            <table>
                                                                <tbody>
                                                                    <tr>
                                                                        <td>
                                                                            <dxe:ASPxLabel ID="ASPxLabel11" runat="server" Text="Running Balance" ClientInstanceName="cbnrLblInvVal" />
                                                                        </td>
                                                                        <td>
                                                                            <strong>
                                                                                <dxe:ASPxLabel ID="lblRunningBalanceCapsulCrp" runat="server" Text="0.00" ClientInstanceName="clblRunningBalanceCapsul" />
                                                                            </strong>
                                                                        </td>
                                                                    </tr>

                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                        <div id="divSubmitButton" runat="server">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="padding: 5px 0px;">
                                                        <span id="tdSaveButtonNew" runat="server">
                                                            <dxe:ASPxButton ID="btnSaveNew" ClientInstanceName="cbtnSaveNew" runat="server"
                                                                AutoPostBack="false" CssClass="btn btn-success" TabIndex="0" Text="S&#818;ave & New"
                                                                UseSubmitBehavior="False">
                                                                <ClientSideEvents Click="function(s, e) {SaveButtonClickNew();}" />
                                                            </dxe:ASPxButton>
                                                        </span>
                                                        <span id="tdSaveButton" runat="server">
                                                            <dxe:ASPxButton ID="btnSaveRecords" ClientInstanceName="cbtnSaveRecords" runat="server"
                                                                AutoPostBack="false" CssClass="btn btn-success" TabIndex="0" Text="Save & Ex&#818;it"
                                                                UseSubmitBehavior="False">
                                                                <ClientSideEvents Click="function(s, e) {SaveButtonClick();}" />
                                                            </dxe:ASPxButton>
                                                        </span>
                                                        <dxe:ASPxButton ID="btnSaveUdf" ClientInstanceName="cbtn_SaveUdf" runat="server" AutoPostBack="False" Text="U&#818;DF" UseSubmitBehavior="False"
                                                            CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                                            <ClientSideEvents Click="function(s, e) {OpenUdf();}" />
                                                        </dxe:ASPxButton>
                                                        <b><span id="tagged" runat="server" style="display: none; color: red">This Customer Payment is tagged in other modules. Cannot Modify data except UDF</span></b>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>




                                    </div>
                                </div>
                            </div>

                        </div>
                    </dxe:ContentControl>
                </ContentCollection>
            </dxe:TabPage>
            <dxe:TabPage Name="Billing/Shipping" Text="Billing/Shipping">
                <ContentCollection>
                    <dxe:ContentControl runat="server">


                        <ucBS:Sales_BillingShipping runat="server" ID="Sales_BillingShipping" />

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



    <%--Modal Section Start--%>

    <div class="modal fade" id="TypeModal" role="dialog" data-backdrop="static"
        data-keyboard="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" onclick="CloseSubModal();">&times;</button>
                    <h4 class="modal-title">Select Type</h4>
                </div>
                <div class="modal-body">
                    <div id="TypeTable">
                        <table border='1' width="100%">
                            <tr class="HeaderStyle">
                                <th>Type</th>

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





    <%--Rev 2.0--%>
    <%--<div class="modal fade" id="CustModel" role="dialog">--%>
    <div class="modal fade" id="CustModel" role="dialog" data-backdrop="static" data-keyboard="false">
        <%--Rev end 2.0--%>
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Customer Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search by Entity Name or Unique Id" />
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




    <%--Rev 2.0--%>
    <%--<div class="modal fade" id="DocModel" role="dialog">--%>
    <div class="modal fade" id="DocModel" role="dialog" data-backdrop="static" data-keyboard="false">
    <%--Rev end 2.0--%>
        <div class="modal-dialog">

            <!-- Modal content-->
            <!-- Modal MainAccount-->
            <div class="modal-content">
                <div class="modal-header">

                    <h4 class="modal-title">Select Document</h4>
                </div>
                <div class="modal-body" style="overflow-y: auto;">
                    <input type="text" onkeydown="DocNewkeydown(event)" id="txtDocSearch" autofocus="autofocus" width="100%" placeholder="Search by Document Number" />

                    <div id="DocTable">
                        <table border="1" width="100%" class="table table-hover">
                            <tr class="HeaderStyle">
                                <th>Document Number</th>
                                <th>Document Date</th>
                                <th>Document Unit</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="closeDocModal();">Close</button>
                </div>
            </div>
        </div>
    </div>




    <%--Customer Popup--%>
    <dxe:ASPxPopupControl ID="DirectAddCustPopup" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="AspxDirectAddCustPopup" Height="750px"
        Width="1020px" HeaderText="Add New Customer" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <HeaderTemplate>
            <span>Add New Customer</span>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>
    <%--Tax PopUP Start--%>

    <%--Rev 2.0--%>
    <%--<div class="modal fade" id="ProdModel" role="dialog">--%>
    <div class="modal fade" id="ProdModel" role="dialog" data-backdrop="static" data-keyboard="false">
        <%--Rev end 2.0--%>
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

                            <tr class="HeaderStyle" style="font-size: small">
                                <th>Select</th>
                                <th class="hide">id</th>
                                <th>Code</th>
                                <th>Name</th>
                                <th>Hsn</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="hide" onclick="DeSelectAll('ProductSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('ProductSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>









    <%--Modal Section End--%>

    <%--Chinmoy added inline Project code start 13-12-2019--%>

    <dxe:ASPxPopupControl ID="ProjectCodePopup" runat="server" ClientInstanceName="cProjectCodePopup"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
        Width="700" HeaderText="Select Document Number" AllowResize="true" ResizingMode="Postponed" Modal="true">
        <%--  <headertemplate>
                <span>Select Document Number</span>
            </headertemplate>--%>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <label><strong>Search By Document Number</strong></label>
                <%--   <span style="color: red;">[Press ESC key to Cancel]</span>--%>
                <dxe:ASPxCallbackPanel runat="server" ID="ProjectCodeCallback" ClientInstanceName="cProjectCodeCallback"
                    OnCallback="ProjectCodeCallback_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookupPopup_ProjectCode" runat="server" ClientInstanceName="clookupPopup_ProjectCode" Width="800"
                                KeyFieldName="ProjectId" TextFormatString="{0}" MultiTextSeparator=", " ClientSideEvents-TextChanged="ProjectCodeSelected"
                                ClientSideEvents-KeyDown="lookup_ProjectCodeKeyDown" OnDataBinding="lookup_ProjectCode_DataBinding">
                                <Columns>

                                    <%--   <dxe:GridViewDataColumn FieldName="Proj_Id" Visible="true" VisibleIndex="0" Caption="Project id" Settings-AutoFilterCondition="Contains" Width="200px">
                                                    </dxe:GridViewDataColumn>--%>
                                    <dxe:GridViewDataColumn FieldName="ProjectCode" Visible="true" VisibleIndex="1" Caption="Project Code" Settings-AutoFilterCondition="Contains" Width="200px">
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="Proj_Name" Visible="true" VisibleIndex="2" Caption="Project Name" Settings-AutoFilterCondition="Contains" Width="200px">
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="Customer" Visible="true" VisibleIndex="3" Caption="Customer" Settings-AutoFilterCondition="Contains" Width="200px">
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="HIERARCHY_NAME" Visible="true" VisibleIndex="4" Caption="Hierarchy" Settings-AutoFilterCondition="Contains" Width="200px">
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="Hierarchy_ID" Visible="true" VisibleIndex="5" Caption="Hierarchy_ID" Settings-AutoFilterCondition="Contains" Width="0">
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
                            <%--   <dx:LinqServerModeDataSource ID="EntityServerModeDataProjectQuotation" runat="server" OnSelecting="EntityServerModeDataProjectQuotation_Selecting"
                                                ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />--%>
                        </dxe:PanelContent>
                    </PanelCollection>
                    <ClientSideEvents EndCallback="ProjectCodeCallback_endcallback" />
                </dxe:ASPxCallbackPanel>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
    </dxe:ASPxPopupControl>



    <%--//End--%>






    <%--Customer Balance Add--%>

    <dxe:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ClientInstanceName="cOutstandingPopup"
        Width="1300px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <HeaderTemplate>
            <strong><span style="color: #fff">Customer Outstanding</span></strong>

            <dxe:ASPxImage ID="ASPxImage2" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                <ClientSideEvents Click="function(s, e){ 
                                                            cOutstandingPopup.Hide();
                                                        }" />
            </dxe:ASPxImage>
        </HeaderTemplate>
        <ContentCollection>

            <dxe:PopupControlContentControl runat="server">
                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport1_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">PDF</asp:ListItem>
                    <asp:ListItem Value="2">XLSX</asp:ListItem>
                    <asp:ListItem Value="3">RTF</asp:ListItem>
                    <asp:ListItem Value="4">CSV</asp:ListItem>
                </asp:DropDownList>
                <dxe:ASPxGridView runat="server" KeyFieldName="SLNO" ClientInstanceName="cCustomerOutstanding" ID="CustomerOutstanding"
                    DataSourceID="EntityServerModeDataSource" OnSummaryDisplayText="ShowGridCustOut_SummaryDisplayText"
                    Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" OnCustomCallback="cCustomerOutstanding_CustomCallback"
                    Settings-ShowFooter="true" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible"
                    OnHtmlFooterCellPrepared="ShowGridCustOut_HtmlFooterCellPrepared" OnHtmlDataCellPrepared="ShowGridCustOut_HtmlDataCellPrepared">

                    <SettingsBehavior AllowDragDrop="true" AllowSort="true"></SettingsBehavior>
                    <SettingsPager Visible="true"></SettingsPager>
                    <Columns>
                        <dxe:GridViewDataTextColumn Caption="Customer Name" FieldName="PARTYNAME" GroupIndex="0"
                            VisibleIndex="0">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="BRANCH_DESCRIPTION" Width="55%" ReadOnly="true" Caption="UNIT">
                        </dxe:GridViewDataTextColumn>
                        <%--<dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="PARTYNAME" Width="100" ReadOnly="true" Caption="Customer">
                                </dxe:GridViewDataTextColumn>--%>
                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="DOC_TYPE" ReadOnly="true" Caption="Doc. Type" Width="100%">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="ISOPENING" ReadOnly="true" Caption="Opening?" Width="30%">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="DOC_NO" ReadOnly="true" Width="95%" Caption="Document No">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="DOC_DATE" Width="50%" ReadOnly="true" Caption="Document Date">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="DUE_DATE" Width="50%" ReadOnly="true" Caption="Due Date">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="DOC_AMOUNT" ReadOnly="true" Caption="Document Amt." Width="50%">
                            <CellStyle HorizontalAlign="Right">
                            </CellStyle>
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Right" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="BAL_AMOUNT" ReadOnly="true" Caption="Balance Amount" Width="50%">
                            <CellStyle HorizontalAlign="Right">
                            </CellStyle>
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Right" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="DAYS" Width="20%" ReadOnly="true" Caption="Days">
                        </dxe:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" AllowSort="False" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                    <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                    <SettingsEditing Mode="EditForm" />
                    <SettingsContextMenu Enabled="true" />
                    <SettingsBehavior AutoExpandAllGroups="true" />
                    <Settings ShowGroupPanel="true" ShowStatusBar="Visible" ShowFilterRow="true" />
                    <SettingsSearchPanel Visible="false" />
                    <SettingsPager PageSize="10">
                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                    </SettingsPager>
                    <TotalSummary>
                        <dxe:ASPxSummaryItem FieldName="DOC_TYPE" SummaryType="Custom" Tag="Item_DocType" />
                        <dxe:ASPxSummaryItem FieldName="BAL_AMOUNT" SummaryType="Custom" Tag="Item_BalAmt"></dxe:ASPxSummaryItem>
                    </TotalSummary>

                    <SettingsDataSecurity AllowEdit="true" />

                </dxe:ASPxGridView>

                <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                    ContextTypeName="ERPDataClassesDataContext" TableName="PARTYOUTSTANDINGDET_REPORT" />
                <div style="display: none">
                    <dxe:ASPxGridViewExporter ID="exporter1" GridViewID="CustomerOutstanding" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                    </dxe:ASPxGridViewExporter>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>

    <asp:HiddenField runat="server" ID="hddnBranchId" />
    <asp:HiddenField runat="server" ID="hddnAsOnDate" />







    <%--Hidden Feild Section--%>

    <asp:HiddenField runat="server" ID="hdnEnterBranch" />
    <asp:HiddenField runat="server" ID="hdnInstrumentType" />
    <asp:HiddenField runat="server" ID="hdnCustomerId" />
    <div runat="server" id="hdnCompany" class="hide"></div>
    <div runat="server" id="hdnLocalCurrency" class="hide"></div>
    <div runat="server" id="ReceiptPaymentId" class="hide"></div>
    <asp:HiddenField runat="server" ID="hdAddEdit" />
    <asp:HiddenField runat="server" ID="SysSetting" />
    <asp:HiddenField runat="server" ID="hdnRefreshType" />
    <asp:HiddenField runat="server" ID="IsUdfpresent" />
    <asp:HiddenField runat="server" ID="Keyval_internalId" />
    <asp:HiddenField runat="server" ID="DoEdit" />
    <asp:HiddenField runat="server" ID="hddnOutStandingBlock" />
    <asp:HiddenField runat="server" ID="ISAllowBackdatedEntry" />
    <asp:HiddenField runat="server" ID="warehousestrProductID" />
    <asp:HiddenField ID="hdnProjectSelectInEntryModule" runat="server" />
     <asp:HiddenField ID="hdnAllowProjectInDetailsLevel" runat="server" />
      <asp:HiddenField ID="hdnEditProjId" runat="server" />
  <%--Rev Work Date:-21.03.2022 -Copy Function add--%>
                    <asp:HiddenField ID="hrCopy" runat="server" />  <%--Copy Mode--%>
                     <%--Close of Rev Work Date:-21.03.2022 -Copy Function add--%>
    <%--for Project  --%>
    <%--End Hidden Feild--%>


    <dxe:ASPxLoadingPanel ID="LoadingPanelCRP" runat="server" ClientInstanceName="cLoadingPanelCRP"
        Modal="True">
    </dxe:ASPxLoadingPanel>
    <asp:HiddenField ID="hdnProjectMandatory" runat="server" />

    <asp:HiddenField ID="hdnLockFromDate" runat="server" />
<asp:HiddenField ID="hdnLockToDate" runat="server" />
 <asp:HiddenField ID="hdnLockFromDateCon" runat="server" />
    <asp:HiddenField ID="hdnLockToDateCon" runat="server" />
</asp:Content>

