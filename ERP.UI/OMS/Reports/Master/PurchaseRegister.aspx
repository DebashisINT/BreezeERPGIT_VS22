<%@ Page Title="Purchase Register" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="PurchaseRegister.aspx.cs" Inherits="ERP.OMS.Reports.Master.PurchaseRegister" %>

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
    </style>


    <script type="text/javascript">

        function fn_OpenDetails(keyValue) {
            //cPopup_Empcitys.SetHeaderText('Modify Products');
            Grid.PerformCallback('Edit~' + keyValue);
            // document.getElementById('Keyval_internalId').value = 'ProductMaster' + keyValue;
        }



        $(function () {

            //BindBranches(null);
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
            debugger;
            var data = "OnDateChanged";
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
            //CallServer(data, "");
            // Grid.PerformCallback('');
        }



        function btn_ShowRecordsClick(e) {
            e.preventDefault;
            var data = "OnDateChanged";

            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
            //CallServer(data, "");
            // alert( data);
            Grid.PerformCallback(data);
        }



        function OnContextMenuItemClick(sender, args) {
            if (args.item.name == "ExportToPDF" || args.item.name == "ExportToXLS") {
                args.processOnServer = true;
                args.usePostBack = true;
            } else if (args.item.name == "SumSelected")
                args.processOnServer = true;
        }


        function abc() {
            // alert();
            $("#drdExport").val(0);

        }


        function OpenBillDetails(branch) {

            $("#drdExport").val(0);
            cgridPendingApproval.PerformCallback('BndPopupgrid~' + branch);
            cpopupApproval.Show();
            return true;

        }

        function popupHide(s, e) {

            cpopupApproval.Hide();

        }


        function OpenPOSDetails(invoice, type) {

            // alert(type);
            if (type == 'PB') {


                url = '/OMS/Management/Activities/PurchaseInvoice.aspx?key=' + invoice + '&IsTagged=1&req=V&type=PB';


            }
            else if (type == 'PR') {

                url = '/OMS/Management/Activities/PReturn.aspx?key=' + invoice + '&IsTagged=1&req=V&type=PR';

                // alert(url);
            }


            popupbudget.SetContentUrl(url);
            popupbudget.Show();
        }

        function BudgetAfterHide(s, e) {
            popupbudget.Hide();
        }



        function selectAll() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAll() {
            gridbranchLookup.gridView.UnselectRows();
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


    </script>
    <style>
        
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div>

        <asp:HiddenField ID="hdnexpid" runat="server" />
    </div>
    <div class="panel-heading">
        <div class="panel-title">
            <%-- <h3>Contact Bank List</h3>--%>
            <h3>Purchase Register Report </h3>
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
                <asp:ListBox ID="ListBoxBranches" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="60px" Width="100%" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>
                <asp:HiddenField ID="hdnActivityType" runat="server" />




                <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cProductComponentPanel" OnCallback="Componentbranch_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookup_branch" SelectionMode="Multiple" runat="server" ClientInstanceName="gridbranchLookup"
                                OnDataBinding="lookup_branch_DataBinding"
                                KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="branch_code" Visible="true" VisibleIndex="1" Caption="Branch code" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>

                                    <dxe:GridViewDataColumn FieldName="branch_description" Visible="true" VisibleIndex="2" Caption="Branch Name" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>
                                </Columns>
                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                    <Templates>
                                        <StatusBar>
                                            <table class="OptionsTable" style="float: right">
                                                <tr>
                                                    <td>
                                                        <div class="hide">
                                                            <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll" />
                                                        </div>
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
                    <asp:ListItem Text="Only Inventory" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Only Non Inventory" Value="0"></asp:ListItem>

                </asp:DropDownList>
            </div>
            <div class="clear"></div>
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

            <div class="col-md-2" style="padding-top: 20px;">
                <label>&nbsp</label>
                <asp:CheckBox runat="server" ID="chkparty" Checked="true" Text="Search by Party Invoice Date" />
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
                        <asp:ListItem Value="2">XLS</asp:ListItem>
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
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                            OnCustomCallback="Grid_CustomCallback" OnDataBinding="grid2_DataBinding" Settings-HorizontalScrollBarMode="Auto"
                            OnSummaryDisplayText="ShowGrid_SummaryDisplayText" ClientSideEvents-BeginCallback="Callback_EndCallback">
                            <Columns>

                                <dxe:GridViewDataTextColumn FieldName="Serial" Caption="Serial" Width="50px" VisibleIndex="1" />


                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="Branch" Caption="Unit" Width="10%">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>

                                        <a href="javascript:void(0)" onclick="OpenBillDetails('<%#Eval("branch_id") %>')" class="pad">
                                            <%#Eval("Branch")%>
                                        </a>
                                    </DataItemTemplate>

                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>



                                <%--   <dxe:GridViewDataTextColumn FieldName="Branch" Caption="Branch" Width="10%" VisibleIndex="2" />--%>
                                <dxe:GridViewDataTextColumn FieldName="Invoice No" Caption="Total Bill Count" Width="10%" VisibleIndex="3">

                                    <PropertiesTextEdit DisplayFormatString="0"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Purchase Value" Caption="Purchase Value" Width="15%" VisibleIndex="4">

                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CGST_Amt" Caption="CGST Amount" Width="12%" VisibleIndex="6">

                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="SGST_Amt" Caption="SGST Amount" Width="12%" VisibleIndex="7">

                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="IGST_Amt" Caption="IGST Amount" Width="12%" VisibleIndex="8">

                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="UTGST_Amt" Caption="UTGST Amount" Width="12%" VisibleIndex="9">

                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--  <dxe:GridViewDataTextColumn FieldName="Total Value" Caption="Total Value" Width="12%" VisibleIndex="10" />--%>


                                <dxe:GridViewDataTextColumn FieldName="Others_Charges" Caption="Other Charges" Width="12%" VisibleIndex="10">

                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Tax Misc." Caption="Tax Misc." Width="12%" VisibleIndex="11">

                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>




                                <dxe:GridViewDataTextColumn FieldName="Total Value" Caption="Total Value" Width="15%" VisibleIndex="13">
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
                                <dxe:ASPxSummaryItem FieldName="Invoice No" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Sale Value" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Tax Misc." SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="CGST_Amt" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="SGST_Amt" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="IGST_Amt" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="UTGST_Amt" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Total Value" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="NTax Misc" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Others_Charges" SummaryType="Sum" />



                            </TotalSummary>

                        </dxe:ASPxGridView>

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
        Width="1300px" Height="600px" ScrollBars="Vertical" HeaderText="Purchase Register Report Details" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="TopSides" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                <asp:DropDownList ID="ddldetails" runat="server" CssClass="btn btn-sm btn-primary"
                    OnSelectedIndexChanged="cmbExport2_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">PDF</asp:ListItem>
                    <asp:ListItem Value="2">XLS</asp:ListItem>
                    <asp:ListItem Value="3">RTF</asp:ListItem>
                    <asp:ListItem Value="4">CSV</asp:ListItem>

                </asp:DropDownList>

                <div class="row">
                    <div>
                    </div>

                    <div class="col-md-12">


                        <dxe:ASPxGridView ID="gridPendingApproval" runat="server" KeyFieldName="SLNO" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="cgridPendingApproval" Settings-HorizontalScrollBarMode="Visible"
                            OnDataBinding="grid_DataBinding" ClientSideEvents-BeginCallback="Callback2_EndCallback"
                            OnCustomCallback="gridPendingApproval_CustomCallback" OnSummaryDisplayText="grid_SummaryDisplayText">
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Serial" FieldName="SLNO" Width="50px"
                                    VisibleIndex="0" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Unit" FieldName="Branch" Width="150px"
                                    VisibleIndex="1" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Vendor Name" Width="300px" FieldName="Customer Name"
                                    VisibleIndex="2" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="Bill No" Width="200px" Caption="Document No" FixedStyle="Left">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>

                                        <a href="javascript:void(0)" target="_blank" onclick="OpenPOSDetails('<%#Eval("Invoice_id") %>','<%#Eval("Module_Type") %>')">
                                            <%#Eval("Bill No")%>
                                        </a>
                                    </DataItemTemplate>

                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn Caption="Entry Date" Width="200px" FieldName="Invoice_Date"
                                    VisibleIndex="4">

                                    <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>



                                <dxe:GridViewDataTextColumn Caption="Vendor's Invoice No" FieldName="Vendor Invoice" Width="200px"
                                    VisibleIndex="6">
                                </dxe:GridViewDataTextColumn>




                                <dxe:GridViewDataTextColumn Caption="Invoice Date." Width="200px" FieldName="Vendor Date"
                                    VisibleIndex="7">

                                    <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>




                                <dxe:GridViewDataTextColumn Caption="Purchase Value" FieldName="Purchase Value" Width="200px"
                                    VisibleIndex="8">

                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>




                                <dxe:GridViewDataTextColumn FieldName="CGST_Amt" Caption="CGST Amount" Width="100px" VisibleIndex="9">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="SGST_Amt" Caption="SGST Amount" Width="100px" VisibleIndex="10">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="IGST_Amt" Caption="IGST Amount" Width="100px" VisibleIndex="11">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="UTGST_Amt" Caption="UTGST Amount" Width="100px" VisibleIndex="12">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%-- <dxe:GridViewDataTextColumn FieldName="Total Value" Caption="Total Value" Width="12%" VisibleIndex="9" />--%>



                                <dxe:GridViewDataTextColumn Caption="Others Charges" FieldName="Others_Charges" Width="100px"
                                    VisibleIndex="13">

                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn Caption="Tax Misc." FieldName="Tax Misc." Width="100px"
                                    VisibleIndex="14">

                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>




                                <dxe:GridViewDataTextColumn Caption="Bill Amount" FieldName="Total Value" Width="200px"
                                    VisibleIndex="15">

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
                            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsLoadingPanel Text="Please Wait..." />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />


                            <TotalSummary>

                                <dxe:ASPxSummaryItem FieldName="Sale Value" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Tax Misc." SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="CGST_Amt" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="SGST_Amt" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="IGST_Amt" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="UTGST_Amt" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Total Value" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Others_Charges" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Purchase Value" SummaryType="Sum" />


                            </TotalSummary>

                        </dxe:ASPxGridView>



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
</asp:Content>

