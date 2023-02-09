<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="SalesRegisterReport.aspx.cs" 
    Inherits="Reports.Reports.GridReports.SalesRegisterReport" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <style>
        #pageControl, .dxtc-content {
            overflow: visible !important;
        }

        #MandatoryAssign {
            position: absolute;
            right: -17px;
            top: 6px;
        }

        #MandatorySupervisorAssign {
            position: absolute;
            right: 1px;
            top: 27px;
        }

        .chosen-container.chosen-container-multi,
        .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }

        #ListBoxBranches {
            width: 200px;
        }

        .hide {
            display: none;
        }

        .dxtc-activeTab .dxtc-link {
            color: #fff !important;
        }
        /*rev Pallab*/
        .dxlpLoadingPanel_PlasticBlue tbody, .dxlpLoadingPanelWithContent_PlasticBlue tbody
        {
            position: absolute;
            bottom: 200px;
            background: #fff;
            box-shadow: 1px 1px 10px #0000001c;
            right: 50%;
        }
        /*rev end Pallab*/
    </style>


    <script type="text/javascript">
        document.onkeyup = function (e) {
            if (event.keyCode == 27 && cpopupApproval.GetVisible() == true && popupbudget.GetVisible() == false) { //run code for alt+N -- ie, Save & New  
                popupHide();
            }
            else if (event.keyCode == 27 && popupbudget.GetVisible() == true) { //run code for alt+N -- ie, Save & New  
                popupHide2();
            }
        }
        function popupHide(s, e) {
            cpopupApproval.Hide();
            Grid.Focus();
            $("#drdExport").val(0);
        }
        function popupHide2(s, e) {
            popupbudget.Hide();
            cgridSalesRegDetail.Focus();
            $("#ddldetails").val(0);
        }

        function fn_OpenDetails(keyValue) {
            //cPopup_Empcitys.SetHeaderText('Modify Products');
            Grid.PerformCallback('Edit~' + keyValue);
            // document.getElementById('Keyval_internalId').value = 'ProductMaster' + keyValue;
        }



        $(function () {

            // BindBranches(null);
            //BindCustomerVendor();


            cProductComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());


            function OnWaitingGridKeyPress(e) {

                if (e.code == "Enter") {


                }

            }


        });



        $(document).ready(function () {
            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                // BindLedgerType(Ids);                    

                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');

            })

            $("#ddlbranchHO").change(function () {
                var Ids = $(this).val();
                // BindLedgerType(Ids);                    

           <%--     $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);--%>
                $('#MandatoryActivityType').attr('style', 'display:none');
                $("#hdnSelectedBranches").val('');
                //  BindBranches(null);
                cProductComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            })
        })

    </script>

    <script type="text/javascript">
        function cxdeToDate_OnChaged(s, e) {

            var data = "OnDateChanged";
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
            //CallServer(data, "");
            // Grid.PerformCallback('');
        }

        function btn_ShowRecordsClick(e) {
            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            e.preventDefault;
            var data = "OnDateChanged";
            $("#hfIsSalesRegSummFilter").val("Y");
            //data += '~' + cxdeFromDate.GetDate();
            //data += '~' + cxdeToDate.GetDate();

            //Grid.PerformCallback(data);            
            //cCallbackPanel.PerformCallback(data + '~' + $("#ddlbranchHO").val());
            if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                jAlert('Please select atleast one branch for generate the report.');
            }
            else {
                cCallbackPanel.PerformCallback(data + '~' + $("#ddlbranchHO").val());
            }
            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "For the period: " + FromDate + " To " + ToDate;
        }
        function CallbackPanelEndCall(s, e) {
            <%--Rev Subhra 20-12-2018   0017670--%>
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
            //End of Subhra
            Grid.Refresh();
        }
        function GetDateFormat(today) {
            if (today != "") {
                var dd = today.getDate();
                var mm = today.getMonth() + 1; //January is 0!

                var yyyy = today.getFullYear();
                if (dd < 10) {
                    dd = '0' + dd;
                }
                if (mm < 10) {
                    mm = '0' + mm;
                }
                today = dd + '-' + mm + '-' + yyyy;
            }

            return today;
        }

        function OnContextMenuItemClick(sender, args) {
            if (args.item.name == "ExportToPDF" || args.item.name == "ExportToXLS") {
                args.processOnServer = true;
                args.usePostBack = true;
            } else if (args.item.name == "SumSelected")
                args.processOnServer = true;
        }


        //function abc() {
        //    // alert();
        //    $("#drdExport").val(0);

        //}


        function OpenBillDetails(branch) {

            $("#drdExport").val(0);
            $("#hfIsSaleRegdetFilter").val("Y");
            cCallbackPanelDetails.PerformCallback('BndPopupgrid~' + branch);
            cpopupApproval.Show();
            return true;

        }
        function CallbackPanelDetailsEndCall(s, e) {
            cgridSalesRegDetail.Refresh();
        }
        function popupHide(s, e) {

            cpopupApproval.Hide();

        }


        function OpenPOSDetails(invoice, type) {

            // alert(type);
            if (type == 'POS') {
                //  window.location.href = '/OMS/Management/Activities/posSalesInvoice.aspx?key=' + invoice + '&Viemode=1';

                // window.open('/OMS/Management/Activities/posSalesInvoice.aspx?key=' + invoice + '&Viemode=1', '_blank')
                url = '/OMS/Management/Activities/posSalesInvoice.aspx?key=' + invoice + '&IsTagged=1&Viemode=1';
            }
            else if (type == 'SI') {

                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                ///     window.open('/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type, '_blank')
                url = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&IsTagged=1&req=V&type=' + type;
            }

            else if (type == 'SRN') {

                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                ///     window.open('/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type, '_blank')


                url = '/OMS/Management/Activities/ReturnNormal.aspx?key=' + invoice + '&IsTagged=1&req=V&type=' + type;
            }


            else if (type == 'SRM') {

                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                ///     window.open('/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type, '_blank')
                url = '/OMS/Management/Activities/ReturnManual.aspx?key=' + invoice + '&IsTagged=1&req=V&type=' + type;
            }

            else if (type == 'USR') {

                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                ///     window.open('/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type, '_blank')
                // url = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&IsTagged=1&req=V&type=' + type;

                url = '/OMS/Management/Activities/UndeliveryReturn.aspx?key=' + invoice + '&IsTagged=1&req=V&type=' + type;
            }



            popupbudget.SetContentUrl(url);
            popupbudget.Show();
        }

        function BudgetAfterHide(s, e) {
            popupbudget.Hide();
        }

    </script>
    <script>

        function GethiddenSalesregister() {

            var value1 = '1';
            alert(value1);

            if (value1 != "0") {
                //  var selectedValue = $(this).val();

                $("#hdnexpid").val(value1);

                //$('#drdExport').prop("selectedIndex", 0);


                return true;
            }
            else {

                //   return false;
            }
        }

        function Callback_EndCallback() {

            // alert('');
            $("#drdExport").val(0);
        }

        function Callback2_EndCallback() {
            // alert('');
            $("#ddldetails").val(0);
        }


        function CloseGridQuotationLookup() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }


        function selectAll() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAll() {
            gridbranchLookup.gridView.UnselectRows();
        }


    </script>
    <style>
        
    </style>

    <style>
        .plhead a {
            font-size:16px;
            padding-left:10px;
            position:relative;
            width:100%;
            display:block;
            padding:9px 10px 5px 10px;
        }
        .plhead a>i {
            position:absolute;
            top:11px;
            right:15px;
        }
        #accordion {
            margin-bottom:10px;
        }
        .companyName {
            font-size:16px;
            font-weight:bold;
            margin-bottom:15px;
        }
        
        .plhead a.collapsed .fa-minus-circle{
            display:none;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                Grid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                Grid.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    Grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    Grid.SetWidth(cntWidth);
                }

            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div>

        <asp:HiddenField ID="hdnexpid" runat="server" />
    </div>
    <div class="panel-heading">
        <%--<div class="panel-title">
            <h3>Sales Register</h3>
        </div>--%>
        <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
          <div class="panel panel-info">
            <div class="panel-heading" role="tab" id="headingOne">
              <h4 class="panel-title plhead">
                <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne" class="collapsed">
                  <asp:Label ID="RptHeading" runat="Server" Text="" Width="470px" style="font-weight:bold;"></asp:Label>
                    <i class="fa fa-plus-circle" ></i>
                    <i class="fa fa-minus-circle"></i>
                </a>
              </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
              <div class="panel-body">
                    <div class="companyName">
                        <asp:Label ID="CompName" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <%--Rev Subhra 20-12-2018   0017670--%>
                    <div>
                        <asp:Label ID="CompBranch" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                  <%--End of Rev--%>
                    <div>
                        <asp:Label ID="CompAdd" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="CompOth" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="CompPh" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>       
                    <div>
                        <asp:Label ID="CompAccPrd" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="DateRange" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
              </div>
            </div>
          </div>
        </div>
    </div>
    <div class="form_main">
        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />
        <div class="row">
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">Head Branch</label>
                <div>
                    <asp:DropDownList ID="ddlbranchHO" runat="server" Width="100%"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"></asp:Label></label>
                <%--<asp:ListBox ID="ListBoxBranches" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="60px" Width="100%" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>--%>
                <asp:HiddenField ID="hdnActivityType" runat="server" />




                <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cProductComponentPanel" OnCallback="Componentbranch_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookup_branch" SelectionMode="Multiple" runat="server" ClientInstanceName="gridbranchLookup"
                                OnDataBinding="lookup_branch_DataBinding"
                                KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="branch_code" Visible="true" VisibleIndex="1" width="200px" Caption="Branch code" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>

                                    <dxe:GridViewDataColumn FieldName="branch_description" Visible="true" VisibleIndex="2" width="200px" Caption="Branch Name" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>
                                </Columns>
                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                    <Templates>
                                        <StatusBar>
                                            <table class="OptionsTable" style="float: right">
                                                <tr>
                                                    <td>
                                                        <%--<div class="hide">--%>
                                                            <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll" />
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll" />
                                                        <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" UseSubmitBehavior="False" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </StatusBar>
                                    </Templates>
                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                    <SettingsPager Mode="ShowPager">
                                    </SettingsPager>

                                    <SettingsPager PageSize="20">
                                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                    </SettingsPager>

                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                </GridViewProperties>

                            </dxe:ASPxGridLookup>
                        </dxe:PanelContent>
                    </PanelCollection>

                </dxe:ASPxCallbackPanel>






                <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                <span id="MandatoryActivityType" style="display: none" class="validclass">
                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                <asp:HiddenField ID="hdnSelectedBranches" runat="server" />
            </div>

            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label2" runat="Server" Text="Show Invoice Type: " CssClass="mylabel1"></asp:Label>
                </label>
                <asp:DropDownList ID="ddlisinventory" runat="server" Width="100%">
                    <asp:ListItem Text="All" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Only Inventory" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Only Non Inventory" Value="0"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsFrom">
                    <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>

                </dxe:ASPxDateEdit>
            </div>
            <div class="col-md-2">
                <label style="margin-bottom: 0">&nbsp</label>
                <div class="">

                    <button id="btnShow" class="btn btn-primary" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                    <%-- <% if (rights.CanExport)
               { %>--%>
                    <%--    
                onchange="javascript:GethiddenSalesregister($(this).val())"--%>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                        OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">

                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLSX</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>

                    </asp:DropDownList>
                    <%-- <% } %>--%>
                </div>
            </div>
        </div>





        <table class="TableMain100">

            <tr>
                <td colspan="2">
                    <div onkeypress="OnWaitingGridKeyPress(event)">
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SEQ"
                             DataSourceID="GenerateEntityServerModeDataSource" OnSummaryDisplayText="ShowGrid_SummaryDisplayText" ClientSideEvents-BeginCallback="Callback_EndCallback"
                            Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Auto">
                            <Columns>
                               <%--OnCustomCallback="Grid_CustomCallback" OnDataBinding="grid2_DataBinding"--%>

                                <dxe:GridViewDataTextColumn FieldName="SEQ" Caption="Serial" Width="50px" VisibleIndex="1" />


                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="BRANCH" Caption="Unit" Width="10%">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>

                                        <a href="javascript:void(0)" onclick="OpenBillDetails('<%#Eval("BRANCH_ID") %>')" class="pad">
                                            <%#Eval("BRANCH")%>
                                        </a>
                                    </DataItemTemplate>

                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>



                                <%--   <dxe:GridViewDataTextColumn FieldName="Branch" Caption="Branch" Width="10%" VisibleIndex="2" />--%>
                                <dxe:GridViewDataTextColumn FieldName="INVOICE_NO" Caption="Total Bill Count" Width="10%" VisibleIndex="3">

                                    <PropertiesTextEdit DisplayFormatString="0"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="SALE_VALUE" Caption="Sale Value" Width="15%" VisibleIndex="4">

                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CGST_AMT" Caption="CGST Amount" Width="12%" VisibleIndex="6">

                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="SGST_AMT" Caption="SGST Amount" Width="12%" VisibleIndex="7">

                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="IGST_AMT" Caption="IGST Amount" Width="12%" VisibleIndex="8">

                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="UTGST_AMT" Caption="UTGST Amount" Width="12%" VisibleIndex="9">

                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--  <dxe:GridViewDataTextColumn FieldName="Total Value" Caption="Total Value" Width="12%" VisibleIndex="10" />--%>


                                <dxe:GridViewDataTextColumn FieldName="OTHERS_CHARGES" Caption="Other Charges" Width="12%" VisibleIndex="10">

                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="TAX_MISC" Caption="Tax Misc." Width="12%" VisibleIndex="11">

                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn FieldName="OLD_UNIT" Caption="Old Unit" Width="12%" VisibleIndex="12">

                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn FieldName="TOTAL_VALUE" Caption="Total Value" Width="15%" VisibleIndex="13">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="50">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                            </SettingsPager>

                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="INVOICE_NO" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="SALE_VALUE" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="TAX_MISC" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="CGST_AMT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="SGST_AMT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="IGST_AMT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="UTGST_AMT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="TOTAL_VALUE" SummaryType="Sum" />
                            <%--    <dxe:ASPxSummaryItem FieldName="NTax Misc" SummaryType="Sum" />--%>
                                <dxe:ASPxSummaryItem FieldName="OTHERS_CHARGES" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="OLD_UNIT" SummaryType="Sum" />


                            </TotalSummary>

                        </dxe:ASPxGridView>
                             <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                            ContextTypeName="ReportSourceDataContext" TableName="PURCHASEREGISTERSUMMARY_REPORT"></dx:LinqServerModeDataSource>
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <div>
    </div>

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>


    <dxe:ASPxPopupControl ID="popupApproval" runat="server" ClientInstanceName="cpopupApproval"
        Width="1300px" Height="600px" ScrollBars="Vertical" HeaderText="Sales Register - Details" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="TopSides" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                <asp:DropDownList ID="ddldetails" runat="server" CssClass="btn btn-sm btn-primary"
                    OnSelectedIndexChanged="cmbExport2_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">PDF</asp:ListItem>
                    <asp:ListItem Value="2">XLSX</asp:ListItem>
                    <asp:ListItem Value="3">RTF</asp:ListItem>
                    <asp:ListItem Value="4">CSV</asp:ListItem>

                </asp:DropDownList>

                <div class="row">
                    <div>
                    </div>

                    <div class="col-md-12">


                        <dxe:ASPxGridView ID="gridSalesRegDetail" runat="server"  AutoGenerateColumns="False" KeyFieldName="SEQ"
                            Width="100%" ClientInstanceName="cgridSalesRegDetail"  ClientSideEvents-BeginCallback="Callback2_EndCallback"
                             DataSourceID="GenerateEntityServerModeDetailsDataSource" OnSummaryDisplayText="grid_SummaryDisplayText"
                            Settings-VerticalScrollableHeight="350" Settings-VerticalScrollBarMode="Auto">
                            <Columns>
                              <%--  OnDataBinding="grid_DataBinding" OnCustomCallback="gridSalesRegDetail_CustomCallback" --%>

                                <dxe:GridViewDataTextColumn Caption="Serial" FieldName="SEQ" VisibleIndex="0" Width="0" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Unit" FieldName="BRANCH" Width="180px" VisibleIndex="1" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <%--Rev 1.0 Mantis: 0025618--%>
                                <dxe:GridViewDataTextColumn Caption="Customer Code" Width="200px" FieldName="UCC" VisibleIndex="2" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <%--End of Rev 1.0 Mantis: 0025618--%>
                                <dxe:GridViewDataTextColumn Caption="Customer Name" Width="160px" FieldName="CUSTOMER_NAME" VisibleIndex="3" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="City" Width="120px" FieldName="CITY_NAME" VisibleIndex="4" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="State" Width="120px" FieldName="STATE" VisibleIndex="5" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Country" Width="100px" FieldName="COU_COUNTRY" VisibleIndex="6" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Ship to Party Name" Width="250px" FieldName="SHIP_TO_PARTY" VisibleIndex="7" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Address" Width="300px" FieldName="CUST_ADDRESS" VisibleIndex="8" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Salesman Name" Width="160px" FieldName="SALESMAN_NAME" VisibleIndex="9">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="BILL_NO" Width="120px" Caption="Invoice No" >
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>
                                        <a href="javascript:void(0)" onclick="OpenPOSDetails('<%#Eval("INVOICE_ID") %>','<%#Eval("MODULE_TYPE") %>')">
                                            <%#Eval("BILL_NO")%>
                                        </a>
                                    </DataItemTemplate>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Inventory Type" Width="150px" FieldName="INVENTORYTYPE" VisibleIndex="11" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Invoice Date" Width="100px" FieldName="INVOICE_DATE" VisibleIndex="12">
                                    <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Sale Value" FieldName="SALE_VALUE" Width="100px" VisibleIndex="13">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CGST_AMT" Caption="CGST Amount" Width="100px" VisibleIndex="14">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="SGST_AMT" Caption="SGST Amount" Width="100px" VisibleIndex="15">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="IGST_AMT" Caption="IGST Amount" Width="100px" VisibleIndex="16">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="UTGST_AMT" Caption="UTGST Amount" Width="100px" VisibleIndex="17">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%-- <dxe:GridViewDataTextColumn FieldName="Total Value" Caption="Total Value" Width="12%" VisibleIndex="9" />--%>

                                <dxe:GridViewDataTextColumn Caption="Others Charges" FieldName="OTHERS_CHARGES" Width="100px" VisibleIndex="18">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Tax Misc." FieldName="TAX_MISC" Width="100px" VisibleIndex="19">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Old Unit" FieldName="OLD_UNIT" Width="100px" VisibleIndex="20">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Bill Amount" FieldName="TOTAL_VALUE" Width="100px" VisibleIndex="21">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <SettingsBehavior AllowFocusedRow="true" ColumnResizeMode="Control" />
                            <%--     <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True">
                                    <FirstPageButton Visible="True">
                                    </FirstPageButton>
                                    <LastPageButton Visible="True">
                                    </LastPageButton>
                                </SettingsPager>--%>
                            <SettingsEditing Mode="Inline">
                            </SettingsEditing>
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                            </SettingsPager>
                            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="true" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsLoadingPanel Text="Please Wait..." />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />


                            <TotalSummary>

                                <dxe:ASPxSummaryItem FieldName="SALE_VALUE" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="TAX_MISC" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="CGST_AMT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="SGST_AMT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="IGST_AMT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="UTGST_AMT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="TOTAL_VALUE" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="OTHERS_CHARGES" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="OLD_UNIT" SummaryType="Sum" />

                            </TotalSummary>

                        </dxe:ASPxGridView>
                         <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDetailsDataSource" runat="server" OnSelecting="GenerateEntityServerModelDetailDataSource_Selecting"
                            ContextTypeName="ReportSourceDataContext" TableName="SALESREGISTERDETAILS_REPORT"></dx:LinqServerModeDataSource>


                    </div>
                    <div class="clear"></div>



                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents CloseUp="popupHide" />

    </dxe:ASPxPopupControl>


    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupbudget" Height="500px"
        Width="1200px" HeaderText="Details" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>

        <ClientSideEvents CloseUp="BudgetAfterHide" />
    </dxe:ASPxPopupControl>

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            <asp:HiddenField ID="hfIsSalesRegSummFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

     <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelDetails" ClientInstanceName="cCallbackPanelDetails" OnCallback="CallbackPanelDetails_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            <asp:HiddenField ID="hfIsSaleRegdetFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelDetailsEndCall" />
    </dxe:ASPxCallbackPanel>

    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
</asp:Content>

