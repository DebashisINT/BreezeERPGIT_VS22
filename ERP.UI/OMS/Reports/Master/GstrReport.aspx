<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false" AutoEventWireup="true"
    CodeBehind="GstrReport.aspx.cs" Inherits="ERP.OMS.Reports.Master.GstrReport" %>

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


    </script>



</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="panel-heading">
        <div class="panel-title">
            <%-- <h3>GSTR Report</h3>--%>
            <h3>GSTR-1 </h3>

        </div>

    </div>
    <div class="form_main">
        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />
        <table class="pull-left">
            <tr>


                <%-- <td style="padding-left: 15px">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                </td>--%>
                <td style="width: 254px; display: none">

                    <%--                    <asp:ListBox ID="ListBoxBranches" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>--%>

                    <asp:HiddenField ID="hdnActivityType" runat="server" />
                    <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                    <span id="MandatoryActivityType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                    <asp:HiddenField ID="hdnSelectedBranches" runat="server" />


                </td>



                <td>
                    <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                        <asp:Label ID="lblFromDate" runat="Server" Text="GSTN : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                </td>
                <td>


                    <asp:DropDownList ID="ddlgstn" runat="server"></asp:DropDownList>

                </td>
                <td style="padding-left: 15px">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="lblToDate" runat="Server" Text="Month: " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                </td>
                <td>
                    <asp:DropDownList ID="ddlmonth" runat="server">

                        <asp:ListItem Text="ALL" Value="ALL"></asp:ListItem>
                        <asp:ListItem Text="JAN" Value="1"></asp:ListItem>
                        <asp:ListItem Text="FEB" Value="2"></asp:ListItem>
                        <asp:ListItem Text="MAR" Value="3"></asp:ListItem>
                        <asp:ListItem Text="APR" Value="4"></asp:ListItem>
                        <asp:ListItem Text="MAY" Value="5"></asp:ListItem>
                        <asp:ListItem Text="JUN" Value="6"></asp:ListItem>
                        <asp:ListItem Text="JULY" Value="7"></asp:ListItem>
                        <asp:ListItem Text="AUG" Value="8"></asp:ListItem>
                        <asp:ListItem Text="SEP" Value="9"></asp:ListItem>
                        <asp:ListItem Text="OCT" Value="10"></asp:ListItem>
                        <asp:ListItem Text="NOV" Value="11"></asp:ListItem>
                        <asp:ListItem Text="DEC" Value="12"></asp:ListItem>

                    </asp:DropDownList>
                </td>

          
                </td>
                <td style="padding-left: 10px; padding-top: 3px">
                    <button id="btnShow" class="btn btn-primary" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>

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
                <asp:ListItem Value="2">XLS</asp:ListItem>
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
                            OnSummaryDisplayText="ShowGrid_SummaryDisplayText"   Settings-HorizontalScrollBarMode="Visible"  OnDataBound="Showgrid_Datarepared"
                            OnCustomCallback="Grid_CustomCallback" OnDataBinding="grid_DataBinding" >

                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="GSTIN/UIN" Caption="GSTIN/UIN"  VisibleIndex="1" Width="25%">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Invoice No." Caption="Invoice No."  VisibleIndex="2" >
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn FieldName="Date" Caption="Date"  VisibleIndex="3">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Value" Caption="Value"  VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Goods/Services" Caption="Goods/Services"  VisibleIndex="5">
                                </dxe:GridViewDataTextColumn>




                                <dxe:GridViewDataTextColumn FieldName="HSN/SAC" Caption="HSN/SAC"  VisibleIndex="6">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Taxable value" Caption="Taxable value"  VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn FieldName="IGST Rate" Caption="IGST Rate"  VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn FieldName="IGST Amount" Caption="IGST Amount"  VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CGST Rate" Caption="CGST Rate"  VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CGST Amount" Caption="CGST Amount"  VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn FieldName="SGST Rate" Caption="SGST Rate"  VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="SGST Amount" Caption="SGST Amount"  VisibleIndex="13" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="POS" Caption="POS"  VisibleIndex="14" >
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Reverse Charge" Caption="Reverse Charge"  VisibleIndex="15" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Provisional Assessment" Caption="Provisional Assessment"  VisibleIndex="16">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="GSTIN E-Commerce" Caption="GSTIN E-Commerce"  VisibleIndex="17">
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
                                <dxe:ASPxSummaryItem FieldName="Value" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Taxable value" SummaryType="Sum"  />
                              
                                 <dxe:ASPxSummaryItem FieldName="IGST Rate" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="IGST Amount" SummaryType="Sum"  />



                                 <dxe:ASPxSummaryItem FieldName="CGST Rate" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="CGST Amount" SummaryType="Sum"  />

                                
                                 <dxe:ASPxSummaryItem FieldName="SGST Rate" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="SGST Amount" SummaryType="Sum"  />


                            </TotalSummary>

                        </dxe:ASPxGridView>

                    </div>
                </td>
            </tr>
        </table>
    </div>

    <div>
    </div>

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>




</asp:Content>

