<%@ Page Title="GSTR  Vendor Payment receipt" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false" AutoEventWireup="true"
    CodeBehind="GSTR-Vendorpaymetreceipt.aspx.cs" Inherits="ERP.OMS.Reports.Master.GSTR_Vendorpaymetreceipt" %>

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

        function CloseGridQuotationLookupbranch() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }

        function selectAll() {
            cBranchGridLookup.gridView.SelectRows();
        }
        function unselectAll() {
            cBranchGridLookup.gridView.UnselectRows();
        }

        function selectAll_branch() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAll_branch() {
            gridbranchLookup.gridView.UnselectRows();
        }
        function BindBranches(noteTilte) {
            var lBox = $('select[id$=ListBoxBranches]');
            var listItems = [];
            var selectedNoteId = '';
            if (noteTilte) {

                selectedNoteId = noteTilte;
            }
            lBox.empty();


            $.ajax({
                type: "POST",
                url: 'GstrReport.aspx/GetBranchesList',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ NoteId: selectedNoteId }),
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;

                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {

                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];

                            listItems.push('<option value="' +
                            id + '">' + name
                            + '</option>');
                        }



                        $(lBox).append(listItems.join(''));
                        ListActivityType();

                        $('#ListBoxBranches').trigger("chosen:updated");
                        $('#ListBoxBranches').prop('disabled', false).trigger("chosen:updated");
                    }
                    else {
                        lBox.empty();
                        $('#ListBoxBranches').trigger("chosen:updated");
                        $('#ListBoxBranches').prop('disabled', true).trigger("chosen:updated");

                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //  alert(textStatus);
                }
            });


        }

        $(function () {

            cProductbranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);

            $("#drp_partytype").change(function () {
                var end = $("#drp_partytype").val();

                if (end == '1') {

                    $("#Label3").text('Customer');
                }
                else if (end == '2') {

                    $("#Label3").text('Vendor');
                }
                else if (end == '0') {


                    $("#Label3").text('Customer/Vendor');
                }

                BindCustomerVendor(end);
            });

            function OnWaitingGridKeyPress(e) {
                alert('1Hi');
                if (e.code == "Enter") {
                    alert('Hi');
                    //var index = cwatingInvoicegrid.GetFocusedRowIndex();
                    //var listKey = cwatingInvoicegrid.GetRowKey(index);
                    //if (listKey) {
                    //    if (cwatingInvoicegrid.GetRow(index).children[6].innerText != "Advance") {
                    //        var url = 'PosSalesInvoice.aspx?key=' + 'ADD&&BasketId=' + listKey;
                    //        LoadingPanel.Show();
                    //        window.location.href = url;
                    //    } else {
                    //        ShowReceiptPayment();
                    //    }
                    //}
                }

            }


        });

        $(function () {

            BindBranches(null);
            //BindCustomerVendor();



            function OnWaitingGridKeyPress(e) {
                alert('1Hi');
                if (e.code == "Enter") {
                    alert('Hi');
                    //var index = cwatingInvoicegrid.GetFocusedRowIndex();
                    //var listKey = cwatingInvoicegrid.GetRowKey(index);
                    //if (listKey) {
                    //    if (cwatingInvoicegrid.GetRow(index).children[6].innerText != "Advance") {
                    //        var url = 'PosSalesInvoice.aspx?key=' + 'ADD&&BasketId=' + listKey;
                    //        LoadingPanel.Show();
                    //        window.location.href = url;
                    //    } else {
                    //        ShowReceiptPayment();
                    //    }
                    //}
                }

            }


        });


        function BindBranches(noteTilte) {
            var lBox = $('select[id$=ListBoxBranches]');
            var listItems = [];
            var selectedNoteId = '';
            if (noteTilte) {

                selectedNoteId = noteTilte;
            }
            lBox.empty();


            $.ajax({
                type: "POST",
                url: 'GstrReport.aspx/GetBranchesList',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ NoteId: selectedNoteId }),
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;

                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {

                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];

                            listItems.push('<option value="' +
                            id + '">' + name
                            + '</option>');
                        }



                        $(lBox).append(listItems.join(''));
                        ListActivityType();

                        $('#ListBoxBranches').trigger("chosen:updated");
                        $('#ListBoxBranches').prop('disabled', false).trigger("chosen:updated");
                    }
                    else {
                        lBox.empty();
                        $('#ListBoxBranches').trigger("chosen:updated");
                        $('#ListBoxBranches').prop('disabled', true).trigger("chosen:updated");

                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //  alert(textStatus);
                }
            });


        }






        function ListActivityType() {

            $('#ListBoxBranches').chosen();
            $('#ListBoxBranches').fadeIn();

            var config = {
                '.chsnProduct': {},
                '.chsnProduct-deselect': { allow_single_deselect: true },
                '.chsnProduct-no-single': { disable_search_threshold: 10 },
                '.chsnProduct-no-results': { no_results_text: 'Oops, nothing found!' },
                '.chsnProduct-width': { width: "100%" }
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }
        }

        //function ListLedgerType() {

        //    $('#ListBoxLedgerType').chosen();
        //    $('#ListBoxLedgerType').fadeIn();

        //    var config = {
        //        '.chsnProduct': {},
        //        '.chsnProduct-deselect': { allow_single_deselect: true },
        //        '.chsnProduct-no-single': { disable_search_threshold: 10 },
        //        '.chsnProduct-no-results': { no_results_text: 'Oops, nothing found!' },
        //        '.chsnProduct-width': { width: "100%" }
        //    }
        //    for (var selector in config) {
        //        $(selector).chosen(config[selector]);
        //    }
        //}
        //function ListCustomerVendor() {

        //    $('#ListBoxCustomerVendor').chosen();
        //    $('#ListBoxCustomerVendor').fadeIn();

        //    var config = {
        //        '.chsnProduct': {},
        //        '.chsnProduct-deselect': { allow_single_deselect: true },
        //        '.chsnProduct-no-single': { disable_search_threshold: 10 },
        //        '.chsnProduct-no-results': { no_results_text: 'Oops, nothing found!' },
        //        '.chsnProduct-width': { width: "100%" }
        //    }
        //    for (var selector in config) {
        //        $(selector).chosen(config[selector]);
        //    }
        //}



        $(document).ready(function () {
            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                // BindLedgerType(Ids);                    

                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');

                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryBranch').attr('style', 'display:none');

            })



            <%-- $("#ListBoxLedgerType").chosen().change(function () {
                 var Ids = $(this).val();

                 $('#<%=hdnSelectedLedger.ClientID %>').val(Ids);
                 $('#MandatoryLedgerType').attr('style', 'display:none');

             })--%>

            <%-- $("#ListBoxCustomerVendor").chosen().change(function () {
                 var Ids = $(this).val();

                 $('#<%=hdnSelectedCustomerVendor.ClientID %>').val(Ids);
                 $('#MandatoryCustomerType').attr('style', 'display:none');

             })--%>

        })


        //function BindLedgerType(Ids) {
        //    var lBox = $('select[id$=ListBoxLedgerType]');
        //    var listItems = [];
        //    $.ajax({
        //        type: "POST",
        //        url: 'LedgerPostingReport.aspx/BindLedgerType',
        //        data: "{'Ids':'" + Ids + "'}",
        //        //  data: JSON.stringify({ Ids: Ids }),
        //        contentType: "application/json; charset=utf-8",

        //        dataType: "json",
        //        success: function (msg) {
        //            var list = msg.d;

        //            if (list.length > 0) {

        //                for (var i = 0; i < list.length; i++) {

        //                    var id = '';
        //                    var name = '';
        //                    id = list[i].split('|')[1];
        //                    name = list[i].split('|')[0];

        //                    listItems.push('<option value="' +
        //                    id + '">' + name
        //                    + '</option>');

        //                }


        //                $(lBox).append(listItems.join(''));
        //                ListLedgerType();

        //                $('#ListBoxLedgerType').trigger("chosen:updated");
        //                $('#ListBoxLedgerType').prop('disabled', false).trigger("chosen:updated");
        //            }
        //            else {
        //                lBox.empty();
        //                $('#ListBoxLedgerType').trigger("chosen:updated");
        //                $('#ListBoxLedgerType').prop('disabled', true).trigger("chosen:updated");

        //            }
        //        },
        //        error: function (XMLHttpRequest, textStatus, errorThrown) {
        //            //  alert(textStatus);
        //        }
        //    });


        //}


        //function BindCustomerVendor() {

        //    var lBox = $('select[id$=ListBoxCustomerVendor]');
        //    var listItems = [];
        //    $.ajax({
        //        type: "POST",
        //        url: 'LedgerPostingReport.aspx/BindCustomerVendor',
        //        //data: "{'Ids':'" + Ids + "'}",                   
        //        contentType: "application/json; charset=utf-8",

        //        dataType: "json",
        //        success: function (msg) {
        //            var list = msg.d;
        //            if (list.length > 0) {
        //                for (var i = 0; i < list.length; i++) {

        //                    var id = '';
        //                    var name = '';
        //                    id = list[i].split('|')[1];
        //                    name = list[i].split('|')[0];

        //                    listItems.push('<option value="' +
        //                    id + '">' + name
        //                    + '</option>');
        //                }


        //                $(lBox).append(listItems.join(''));
        //                ListCustomerVendor();

        //                $('#ListBoxCustomerVendor').trigger("chosen:updated");
        //                $('#ListBoxCustomerVendor').prop('disabled', false).trigger("chosen:updated");
        //            }
        //            else {
        //                lBox.empty();
        //                $('#ListBoxCustomerVendor').trigger("chosen:updated");
        //                $('#ListBoxCustomerVendor').prop('disabled', true).trigger("chosen:updated");

        //            }
        //        },
        //        error: function (XMLHttpRequest, textStatus, errorThrown) {
        //            //  alert(textStatus);
        //        }
        //    });


        //}




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
            //var data = "OnDateChanged";

            var v = $("#ddlgstn").val();

            //data += '~' + cxdeFromDate.GetDate();
            //data += '~' + cxdeToDate.GetDate();
            //CallServer(data, "");
            // alert( data);
            //   Grid.PerformCallback('Te');

            Grid.PerformCallback('ListData~' + v);
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

            cgridPendingApproval.PerformCallback('BndPopupgrid~' + branch);
            cpopupApproval.Show();
            return true;

        }

        function popupHide(s, e) {

            cpopupApproval.Hide();

        }


        function OpenPOSDetails(invoice, type) {

            // alert(type);
            if (type == 'POS') {

                //  window.location.href = '/OMS/Management/Activities/posSalesInvoice.aspx?key=' + invoice + '&Viemode=1';

                window.open('/OMS/Management/Activities/posSalesInvoice.aspx?key=' + invoice + '&Viemode=1', '_blank')
            }
            else if (type == 'SI') {

                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                window.open('/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type, '_blank')

            }


        }

        function Callback_EndCallback() {

            // alert('');
            $("#drdExport").val(0);
        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading">
        <div class="panel-title">

            <%-- <h3>GSTR Report</h3>--%>

            <h3>GSTR  Vendor Payment Receipt </h3>

        </div>

    </div>
    <div class="form_main">
        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />
        <table class="pull-left">


            <tr>


                <td style="width: 254px; display: none">


                    <asp:HiddenField ID="hdnActivityType" runat="server" />
                    <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                    <span id="MandatoryActivityType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                    <asp:HiddenField ID="hdnSelectedBranches" runat="server" />



                </td>


                <td>
                    <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                        <asp:Label ID="lblFromDate" runat="Server" Text="GSTIN : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                </td>
                <td>


                    <asp:DropDownList ID="ddlgstn" runat="server" Width="150px"></asp:DropDownList>

                </td>

                <%-- new added 18-09-2017 --%>
                <td style="">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label2" runat="Server" Text="Branch : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                </td>

                <td style="width: 254px">

                    <asp:ListBox ID="ListBoxBranches" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                    <asp:HiddenField ID="HiddenField2" runat="server" />
                    <span id="MandatoryBranch" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EII" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                    <asp:HiddenField ID="HiddenField3" runat="server" />


                    <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cProductbranchComponentPanel" OnCallback="ASPxCallbackPanel1_Callback">
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
                                                                <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_branch" />
                                                            </div>
                                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_branch" />                                                            
                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookupbranch" UseSubmitBehavior="False" />
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

                </td>
                <%--end new added --%>

                <td>

                    <table>

                        <tr>

                            <td>
                                <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                                    <asp:Label ID="Label1" runat="Server" Text="From Date : " CssClass="mylabel1"
                                        Width="92px"></asp:Label>
                                </div>
                            </td>
                            <td>
                                <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td style="padding-left: 15px">
                                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                                    <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"
                                        Width="92px"></asp:Label>
                                </div>
                            </td>
                            <td>
                                <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>

                            <td style="padding-left: 10px; padding-top: 3px">
                                <button id="btnShow" class="btn btn-primary" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>

                            </td>

                        </tr>
                    </table>
                </td>



            </tr>



            <tr>
            </tr>
        </table>
        <div class="pull-right">

            <%-- <% if (rights.CanExport)
               { %>--%>

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
        <table class="TableMain100">

            <tr>
                <td colspan="2">
                    <div onkeypress="OnWaitingGridKeyPress(event)">
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false"
                            OnSummaryDisplayText="ShowGrid_SummaryDisplayText" OnDataBound="Showgrid_Datarepared"
                            Settings-HorizontalScrollBarMode="Visible" OnCustomSummaryCalculate="ASPxGridView1_CustomSummaryCalculate"
                            OnCustomCallback="Grid_CustomCallback" OnDataBinding="grid_DataBinding" ClientSideEvents-BeginCallback="Callback_EndCallback">

                            <Columns>


                                <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="SrlNo" VisibleIndex="0">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="branch_description" Caption="Unit" VisibleIndex="1" Width="140px" FixedStyle="Left">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="ReceiptPayment_TransactionDate" Caption="Doc Date" VisibleIndex="1" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="ReceiptPayment_VoucherNumber" Caption="Doc No." VisibleIndex="2">
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn FieldName="Doc Type" Caption="Doc Type" VisibleIndex="3">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Transaction Type" Caption="Transaction Type" VisibleIndex="4">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="HSN" Caption="HSN" VisibleIndex="5">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Customer" Caption="Vendor" VisibleIndex="6">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="GSTIN" Caption="GSTIN" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn FieldName="State" Caption="State" VisibleIndex="8">
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn FieldName="Taxable_Amount" Caption="Taxable Amount" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>



                                <dxe:GridViewDataTextColumn FieldName="Tax_Amount" Caption="Total Tax Amount" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn FieldName="Total_CGST" Caption="CGST" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn FieldName="Total_SGST" Caption="SGST" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn FieldName="Total_UTGST" Caption="UTGST" VisibleIndex="13" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Total_IGST" Caption="IGST" VisibleIndex="14" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Voucher_Amount" Caption="Voucher Amount" VisibleIndex="15" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>

                            </Columns>

                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                            <SettingsSearchPanel Visible="false" />

                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                            </SettingsPager>

                            <TotalSummary>


                                <dxe:ASPxSummaryItem FieldName="Taxable_Amount" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Tax_Amount" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Total_CGST" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Total_SGST" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Total_UTGST" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Total_IGST" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Voucher_Amount" SummaryType="Sum" />
                                
                                <dxe:ASPxSummaryItem FieldName="ReceiptPayment_VoucherNumber" SummaryType="Custom"  />


                            </TotalSummary>

                        </dxe:ASPxGridView>

                    </div>
                </td>
            </tr>
        </table>
    </div>

    <div>
    </div>

    <dxe:ASPxGridViewExporter ID="exporter" GridViewID="ShowGrid" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>




</asp:Content>

