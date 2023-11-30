<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                10-04-2023        2.0.37           Pallab              25963: Customer Debit/Credit Note module design modification & check in small device
2.0                28/11/2023        V2.0.41          Priti               0027028: Customer code column is required in the listing module of Sales entry
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="CustomerNoteList.aspx.cs" Inherits="ERP.OMS.Management.Activities.CustomerNoteList" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        td {
            padding-left: 10px;
        }
    </style>
    <script>



        function CallbackPanelEndCall(s, e) {
            cGvJvSearch.Refresh();
        }
        function updateGridByDate() {
            if (cFormDate.GetDate() == null) {
            jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
        }
        else if (ctoDate.GetDate() == null) {
            jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
        }
        else if (ccmbBranchfilter.GetValue() == null) {
            jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
        }
        else {
            localStorage.setItem("FromDateCustomerCrDrNote", cFormDate.GetDate().format('yyyy-MM-dd'));
            localStorage.setItem("ToDateCustomerCrDrNote", ctoDate.GetDate().format('yyyy-MM-dd'));
            localStorage.setItem("BrancheCustomerCrDrNote", ccmbBranchfilter.GetValue());

            $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
            $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
            $("#hfBranchID").val(ccmbBranchfilter.GetValue());
            $("#hfIsFilter").val("Y");

           // cGvJvSearch.Refresh();
            $("#hFilterType").val("All");
            cCallbackPanel.PerformCallback("");
            //cGvJvSearch.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());
        }
                                       }

                                       function OnAddButtonClick() {
                                           window.location.href = "CustomerDebitCreditNote.aspx?key=Add";
                                       }
                                       function PerformCallToGridBind() {
                                           cSelectPanel.PerformCallback('Bindsingledesign');
                                           cDocumentsPopup.Hide();
                                           return false;
                                       }



                                       var isFirstTime = true;
                                       function AllControlInitilize() {
                                           ///  document.getElementById('AddButton').style.display = 'inline-block';
                                           if (isFirstTime) {

                                               if (localStorage.getItem('FromDateCustomerCrDrNote')) {
                                                   var fromdatearray = localStorage.getItem('FromDateCustomerCrDrNote').split('-');
                                                   var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                                                   cFormDate.SetDate(fromdate);
                                               }

                                               if (localStorage.getItem('ToDateCustomerCrDrNote')) {
                                                   var todatearray = localStorage.getItem('ToDateCustomerCrDrNote').split('-');
                                                   var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                                                   ctoDate.SetDate(todate);
                                               }
                                               if (localStorage.getItem('BrancheCustomerCrDrNote')) {
                                                   if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('BrancheCustomerCrDrNote'))) {
                                                       ccmbBranchfilter.SetValue(localStorage.getItem('BrancheCustomerCrDrNote'));
                                                   }

                                               }
                                               //updateGridByDate();

                                               isFirstTime = false;
                                           }
                                       }

                                       function CustomBtnEdit(keyValue) {
                                           window.location.href = "CustomerDebitCreditNote.aspx?key=Edit&id=" + keyValue;
                                       }
                                       function CustomBtnView(keyValue)
                                       {
                                           window.location.href = "CustomerDebitCreditNote.aspx?key=View&id=" + keyValue;
                                       }
                                       function CustomBtnDelete(keyValue)
                                       {
                                           jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                                               if (r == true) {
                                                   cGvJvSearch.PerformCallback('PCB_DeleteBtnOkE~' + keyValue);

                                               }
                                           });
                                       }
                                       function CustomBtnPrint(keyValue, NoteType)
                                       {
                                           onPrintJv(keyValue, NoteType);
                                       }
                                       function CustomButtonClick(s, e) {
                                           //console.log(e);
                                           debugger;
                                           if (e.buttonID == 'CustomBtnEdit') {
                                    
                                               VisibleIndexE = e.visibleIndex;
                                               window.location.href = "CustomerDebitCreditNote.aspx?key=Edit&id=" + s.GetRowKey(e.visibleIndex);

                                           }


                                           if (e.buttonID == 'CustomBtnView') {
                                               VisibleIndexE = e.visibleIndex;
                                               window.location.href = "CustomerDebitCreditNote.aspx?key=View&id=" + s.GetRowKey(e.visibleIndex);
                                           }
                                           else if (e.buttonID == 'CustomBtnDelete') {
                                               VisibleIndexE = e.visibleIndex;

                                               jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                                                   if (r == true) {
                                                       cGvJvSearch.PerformCallback('PCB_DeleteBtnOkE~' + s.GetRowKey(e.visibleIndex));

                                                   }
                                               });
                                           }
                                           else if (e.buttonID == 'CustomBtnPrint') {
                                               var keyValueindex = s.GetRowKey(e.visibleIndex);
                                               var index = s.GetRow(e.visibleIndex);
                                               var NoteType = index.children[0].innerHTML;
                                               onPrintJv(keyValueindex, NoteType);

                                           }
                                       }

                                       var DCNoteID = 0;
                                       function onPrintJv(id, NoteType) {
                                           debugger;
                                           DCNoteID = id;
                                           cDocumentsPopup.Show();
                                           $('#HdCrDrNoteType').val(NoteType);
                                           cCmbDesignName.SetSelectedIndex(0);
                                           cSelectPanel.PerformCallback('Bindalldesignes');
                                           $('#btnOK').focus();
                                       }

                                       function cSelectPanelEndCall(s, e) {

                                           if (cSelectPanel.cpSuccess != null) {
                                               var reportName = cCmbDesignName.GetValue();
                                               var module = 'CUSTDRCRNOTE';
                                               window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + DCNoteID, '_blank')
                                           }
                                           cSelectPanel.cpSuccess = null
                                           if (cSelectPanel.cpSuccess == null) {
                                               cCmbDesignName.SetSelectedIndex(0);
                                           }
                                       }
                                       function GvJvSearch_EndCallBack() {
                                           if (cGvJvSearch.cpJVDelete != undefined && cGvJvSearch.cpJVDelete != null) {
                                               jAlert(cGvJvSearch.cpJVDelete);
                                               cGvJvSearch.cpJVDelete = null;
                                               updateGridByDate();
                                               //cGvJvSearch.PerformCallback('PCB_BindAfterDelete');
                                           }
                                       }


                                       document.onkeydown = function (e) {
                                           if (event.keyCode == 65 && event.altKey == true) {
                                               OnAddButtonClick();//........Alt+A
                                           }
                                           
                                       }
                                       function gridRowclick(s, e) {
                                           //alert('hi');
                                           $('#GvJvSearch').find('tr').removeClass('rowActive');
                                           $('.floatedBtnArea').removeClass('insideGrid');
                                           //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
                                           $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
                                           $(s.GetRow(e.visibleIndex)).addClass('rowActive');
                                           setTimeout(function () {
                                               //alert('delay');
                                               var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
                                               //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
                                               //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
                                               //    setTimeout(function () {
                                               //        $(this).fadeIn();
                                               //    }, 100);
                                               //});    
                                               $.each(lists, function (index, value) {
                                                   ////console.log(index);
                                                   //console.log(value);
                                                   setTimeout(function () {
                                                       //console.log(value);
                                                       $(value).css({ 'opacity': '1' });
                                                   }, 100);
                                               });
                                           }, 200);
                                       }


                                       </script>
    <script type="text/javascript">
                                            $(document).ready(function () {
                                                if ($('body').hasClass('mini-navbar')) {
                                                    var windowWidth = $(window).width();
                                                    var cntWidth = windowWidth - 90;
                                                    cGvJvSearch.SetWidth(cntWidth);
                                                } else {
                                                    var windowWidth = $(window).width();
                                                    var cntWidth = windowWidth - 220;
                                                    cGvJvSearch.SetWidth(cntWidth);
                                                }
                                                $('.navbar-minimalize').click(function () {
                                                    if ($('body').hasClass('mini-navbar')) {
                                                        var windowWidth = $(window).width();
                                                        var cntWidth = windowWidth - 220;
                                                        cGvJvSearch.SetWidth(cntWidth);
                                                    } else {
                                                        var windowWidth = $(window).width();
                                                        var cntWidth = windowWidth - 90;
                                                        cGvJvSearch.SetWidth(cntWidth);
                                                    }

                                                });
                                            });
                                        </script>

    

    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    

        <style>
            select
            {
                -webkit-appearance: auto;
            }
            #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_PLSales , #dt_SaleInvoiceDue , #dtPostingDate
            {
                position: relative;
                z-index: 1;
                background: transparent;
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

            .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridLocationwiseStockStatus ,
        #GvJvSearch
        {
            max-width: 98% !important;
        }

        .calendar-icon
        {
                right: 5px !important;
        }

        /*select#ddlInventory
        {
            -webkit-appearance: auto;
        }*/

        .simple-select::after
        {
            top: 26px !important;
            right: 13px !important;
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

            @media only screen and (max-width: 1380px) and (min-width: 1300px)
            {

                /*.col-xs-1, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9, .col-xs-10, .col-xs-11, .col-xs-12, .col-sm-1, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-sm-10, .col-sm-11, .col-sm-12, .col-md-1, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-md-10, .col-md-11, .col-md-12, .col-lg-1, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-lg-10, .col-lg-11, .col-lg-12 {
                    padding-right: 10px;
                    padding-left: 10px;
                }

                .simple-select::after
                {
                    right: 8px !important;
                }
                .calendar-icon {
                    right: 13px !important;
                }*/

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
        <div class="panel-heading clearfix ">
    <div class="panel-title " id="myDiv">
        <h3 class="pull-left">
            <label id="TxtHeaded">Customer Debit/Credit Note</label>
        </h3>
    </div>
     <table class="padTabtype2 pull-right" id="gridFilter">
        <tr>
            <td>
                <label>From Date</label></td>
            <%--Rev 1.0: "for-cust-icon" class add --%>
            <td class="for-cust-icon">
                <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
                <%--Rev 1.0--%>
                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                <%--Rev end 1.0--%>
            </td>
            <td>
                <label>To Date</label>
            </td>
            <%--Rev 1.0: "for-cust-icon" class add --%>
            <td class="for-cust-icon">
                <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
                <%--Rev 1.0--%>
                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                <%--Rev end 1.0--%>
            </td>
            <td>Unit</td>
            <td>
                <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                </dxe:ASPxComboBox>
            </td>
            <td>
                <input type="button" value="Show" class="btn btn-primary" onclick="updateGridByDate()" />
            </td>
        </tr>
    </table>
</div>
        <div class="form_main">
        <div id="TblSearch" class="clearfix">
            <div class="clearfix">
                <div class="mb-10" style="padding-right: 5px;">
                    <% if (rights.CanAdd)
                       { %>
                    <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span><u>A</u>dd New</span> </a>
                    <% } %>

                    <% if (rights.CanExport)
                       { %>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>
                    <% } %>

                    
                </div>
            </div>

             <div id="spnEditLock" runat="server" style="display:none; color:red;text-align:center"></div>
              <div id="spnDeleteLock" runat="server" style="display:none; color:red;text-align:center"></div>

            <div class="clearfix relative">
                <dxe:ASPxGridView ID="GvJvSearch" runat="server" AutoGenerateColumns="False" SettingsBehavior-AllowSort="true"
                    ClientInstanceName="cGvJvSearch" KeyFieldName="DCNote_ID" Width="100%" Settings-HorizontalScrollBarMode="Auto" DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"
                    OnCustomCallback="GvJvSearch_CustomCallback" OnCustomButtonInitialize="GvJvSearch_CustomButtonInitialize"
                    OnSummaryDisplayText="ShowGrid_SummaryDisplayText">
                    <SettingsSearchPanel Visible="true" Delay="5000" />
                    <ClientSideEvents CustomButtonClick="CustomButtonClick" EndCallback="function(s, e) {GvJvSearch_EndCallBack();}" RowClick="gridRowclick" />
                    <SettingsBehavior ConfirmDelete="True" />
                    <Styles>
                        <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
                        <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>
                        <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                        <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
                        <Footer CssClass="gridfooter"></Footer>
                    </Styles>
                    <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" AlwaysShowPager="True">
                        <FirstPageButton Visible="True">
                        </FirstPageButton>
                        <LastPageButton Visible="True">
                        </LastPageButton>
                    </SettingsPager>

                    <Columns>
                        <%-- <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="NoteType" Caption="Type">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataTextColumn>--%>
                      <%--  Rev Sayantani--%>
                      <%--  <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="DCNote_ID" Caption="DCNote_ID" SortOrder="Descending">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>--%>
                         <dxe:GridViewDataTextColumn Visible="False" ShowInCustomizationForm="false"  VisibleIndex="0" FieldName="DCNote_ID" Caption="DCNote_ID" SortOrder="Descending">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                       <%-- End of Rev Sayantani--%>
                        <dxe:GridViewDataTextColumn Caption="Type" FieldName="NoteType" VisibleIndex="1">
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="TransDate" Caption="Posting Date" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="NoteNumber" Caption="Document Number" Width="150px">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="BranchName" Caption="Unit" >
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Currency" Caption="Currency" Settings-AllowAutoFilter="False">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="ImportType" Caption="Type">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                         <%--Rev  2.0--%>
                          <dxe:GridViewDataTextColumn Caption="Customer Code" FieldName="CustomerCode" Width="160px"
                          VisibleIndex="7">
                          <CellStyle CssClass="gridcellleft" Wrap="true">
                          </CellStyle>
                          <Settings AutoFilterCondition="Contains" />
                         </dxe:GridViewDataTextColumn>
                        <%-- Rev  2.0 End--%>

                        <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="CustomerName" Caption="Customer Name" Width="180px">
                            <CellStyle CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="Amount" Caption="Amount" CellStyle-HorizontalAlign="Right" Width="150px">
                            <HeaderStyle HorizontalAlign="Right" />
                            <CellStyle Wrap="False">
                            </CellStyle>
                            <PropertiesTextEdit DisplayFormatString="0.00">
                            </PropertiesTextEdit>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="Total_CGST" Caption="CGST" CellStyle-HorizontalAlign="Right" Width="150px">
                            <HeaderStyle HorizontalAlign="Right" />
                            <CellStyle Wrap="False"></CellStyle>
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="11" FieldName="Total_SGST" Caption="SGST" CellStyle-HorizontalAlign="Right">
                            <HeaderStyle HorizontalAlign="Right" />
                            <CellStyle Wrap="False"></CellStyle>
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="12" FieldName="Total_UTGST" Caption="UTGST" CellStyle-HorizontalAlign="Right">
                            <HeaderStyle HorizontalAlign="Right" />
                            <CellStyle Wrap="False"></CellStyle>
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="13" FieldName="Total_IGST" Caption="IGST" CellStyle-HorizontalAlign="Right">
                            <HeaderStyle HorizontalAlign="Right" />
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="14" FieldName="Total_taxable_amount" Caption="Taxable Amount" CellStyle-HorizontalAlign="Right" Width="130px">
                            <HeaderStyle HorizontalAlign="Right" />
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                         <dxe:GridViewDataTextColumn VisibleIndex="15" FieldName="Invoice_Number" Caption="Ref.Sale Invoice No."  Width="130px">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="16" FieldName="EnteredBy" Caption="Entered On" Settings-AllowAutoFilter="False">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="17" FieldName="UpdateOn" Caption="Last Update On" Width="130px" Settings-AllowAutoFilter="False">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy hh:mm:ss"></PropertiesTextEdit>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="18" FieldName="UpdatedBy" Caption="Updated By" Settings-AllowAutoFilter="False">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                         
                       <%-- Rev Sayantani--%>
                       <%-- <dxe:GridViewDataTextColumn Visible="False" FieldName="DCNote_ID"></dxe:GridViewDataTextColumn>--%>
                         <%--<dxe:GridViewDataTextColumn Visible="False" ShowInCustomizationForm="false"  FieldName="DCNote_ID"></dxe:GridViewDataTextColumn>--%>
                        <dxe:GridViewDataTextColumn Caption="Project Name" FieldName="Proj_Name" Width="150px"  VisibleIndex="19" Settings-AllowAutoFilter="True">
                           <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="true" />
                           <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                     <%--   End of Rev Sayantani--%>
                 <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="20" Width="130px">
                    <DataItemTemplate>
                        <% if (rights.CanView)
                           { %>
                        <a href="javascript:void(0);" onclick="CustomBtnView('<%# Container.KeyValue %>')" class="pad" title="View">
                            <img src="../../../assests/images/doc.png" /></a>
                        <% } %>
                        <% if (rights.CanEdit)
                           { %>
                        <a href="javascript:void(0);" onclick="CustomBtnEdit('<%# Container.KeyValue %>')" class="pad" title="Edit" style='<%#Eval("Editlock")%>'>
                            <img src="../../../assests/images/info.png" /></a><%} %>
                        <% if (rights.CanDelete)
                           { %>
                        <a href="javascript:void(0);" onclick="CustomBtnDelete('<%# Container.KeyValue %>')" class="pad" title="Delete" style='<%#Eval("Deletelock")%>'>
                            <img src="../../../assests/images/Delete.png" /></a><%} %>
                    
                    
                            <% if (rights.CanPrint)
                               { %>
                            <a href="javascript:void(0);" onclick="CustomBtnPrint('<%# Container.KeyValue %>','<%#Eval("NoteType") %>')" class="pad" title="print">
                                <img src="../../../assests/images/Print.png" />
                            </a><%} %>
                    </DataItemTemplate>
                      <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <HeaderTemplate><span>Actions</span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                </dxe:GridViewDataTextColumn>
                       <%-- <dxe:GridViewCommandColumn VisibleIndex="19" Width="130px" ButtonType="Image" Caption="Actions" HeaderStyle-HorizontalAlign="Center">
                            <CustomButtons>
                                <dxe:GridViewCommandColumnCustomButton ID="CustomBtnView" meta:resourcekey="GridViewCommandColumnCustomButtonResource1" Image-ToolTip="View" Styles-Style-CssClass="pad">
                                    <Image Url="/assests/images/doc.png"></Image>
                                </dxe:GridViewCommandColumnCustomButton>
                                <dxe:GridViewCommandColumnCustomButton ID="CustomBtnEdit" meta:resourcekey="GridViewCommandColumnCustomButtonResource1" Image-ToolTip="Edit" Styles-Style-CssClass="pad">
                                    <Image Url="/assests/images/Edit.png"></Image>
                                </dxe:GridViewCommandColumnCustomButton>
                                <dxe:GridViewCommandColumnCustomButton ID="CustomBtnDelete" meta:resourcekey="GridViewCommandColumnCustomButtonResource2" Image-ToolTip="Delete" Styles-Style-CssClass="pad">
                                    <Image Url="/assests/images/Delete.png"></Image>
                                </dxe:GridViewCommandColumnCustomButton>

                                <dxe:GridViewCommandColumnCustomButton ID="CustomBtnPrint" meta:resourcekey="GridViewCommandColumnCustomButtonResource3" Image-ToolTip="Print" Styles-Style-CssClass="pad">
                                    <Image Url="/assests/images/Print.png"></Image>

                                </dxe:GridViewCommandColumnCustomButton>
                            </CustomButtons>
                        </dxe:GridViewCommandColumn>--%>
                    </Columns>
                     <%-- --Rev Sayantani--%>
                     <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
                     <SettingsCookies Enabled="true" StorePaging="true" StoreColumnsWidth="false" StoreColumnsVisiblePosition="false" />
                     <%-- -- End of Rev Sayantani --%>
                    <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                    <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" HorizontalScrollBarMode="Visible" ShowFooter="true" />
                    <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                        <FirstPageButton Visible="True">
                        </FirstPageButton>
                        <LastPageButton Visible="True">
                        </LastPageButton>
                    </SettingsPager>
                    <TotalSummary>
                        <dxe:ASPxSummaryItem FieldName="Amount" SummaryType="Sum" />
                        <dxe:ASPxSummaryItem FieldName="Total_CGST" SummaryType="Sum" />
                        <dxe:ASPxSummaryItem FieldName="Total_SGST" SummaryType="Sum" />
                        <dxe:ASPxSummaryItem FieldName="Total_UTGST" SummaryType="Sum" />
                        <dxe:ASPxSummaryItem FieldName="Total_IGST" SummaryType="Sum" />
                        <dxe:ASPxSummaryItem FieldName="Total_taxable_amount" SummaryType="Sum" />
                    </TotalSummary>
                </dxe:ASPxGridView>
                <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                    ContextTypeName="ERPDataClassesDataContext" TableName="CustomerNoteList" />
            </div>
        </div>

    </div>
    </div>
    <div>
        <asp:HiddenField ID="hfIsFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfBranchID" runat="server" />
        <asp:HiddenField ID="hdnBranchId" runat="server" />
         <asp:HiddenField ID="hFilterType" runat="server" />

    </div>

    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>


    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxDocumentsPopup" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" ClientSideEvents-EndCallback="cSelectPanelEndCall">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxComboBox ID="CmbDesignName" ClientInstanceName="cCmbDesignName" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                </dxe:ASPxComboBox>
                                <div class="text-center pTop10">
                                    <dxe:ASPxButton ID="btnOK" ClientInstanceName="cbtnOK" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind();}" />
                                    </dxe:ASPxButton>
                                </div>
                                <asp:HiddenField ID="HdCrDrNoteType" runat="server" />
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>
    <%--DEBASHIS--%>

<asp:HiddenField ID="hdnLockFromDateedit" runat="server" />
<asp:HiddenField ID="hdnLockToDateedit" runat="server" />
 
<asp:HiddenField ID="hdnLockFromDatedelete" runat="server" />
<asp:HiddenField ID="hdnLockToDatedelete" runat="server" />
<asp:HiddenField ID="hdnDocLocTYpe" runat="server" />

     <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">           
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>
</asp:Content>
