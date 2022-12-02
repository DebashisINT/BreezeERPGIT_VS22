<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="GSTR_HSNcodeRate.aspx.cs" 
    Inherits="Reports.Reports.GridReports.GSTR_HSNcodeRate" %>

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

    </script>


    <script type="text/javascript">

        $(function () {


            //   Grid.PerformCallback('ListData~' + 0);
        });

        function cxdeToDate_OnChaged(s, e) {
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


        function Callback_EndCallback() {

            // alert('');
            $("#drdExport").val(0);
        }

    </script>

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


    <div class="panel-heading">
        <%--<div class="panel-title">
            <h3>GST Rate HSN/SAC Code Wise </h3>
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
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" ClientInstanceName="page"
                        Font-Size="12px" Width="100%">
                        <TabPages>
                            <dxe:TabPage Name="sales" Text="INPUT">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">



                                        <div onkeypress="OnWaitingGridKeyPress(event)">
                                            <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false"
                                                OnSummaryDisplayText="ShowGrid_SummaryDisplayText" OnDataBound="Showgrid_Datarepared" Settings-HorizontalScrollBarMode="Visible"
                                                OnCustomCallback="Grid_CustomCallback" OnDataBinding="grid_DataBinding" ClientSideEvents-BeginCallback="Callback_EndCallback">

                                                <Columns>
                                                    
                                                    <dxe:GridViewDataTextColumn FieldName="slno" Caption="Sl" Width="5%" VisibleIndex="1">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="sProducts_Code" Caption="Product/Ledger Short Name" Width="25%" VisibleIndex="2">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="ProductClass_Description" Caption="Product Class" Width="16%" VisibleIndex="5">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="STYPE" Caption="Type" Width="4%" VisibleIndex="5">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="sProducts_HsnCode" Caption="HSN/SAC Code" Width="10%" VisibleIndex="6">
                                                    </dxe:GridViewDataTextColumn>
                                                    
<%--                                                <dxe:GridViewDataTextColumn FieldName="INPUTCGST" Caption="INPUTCGST" VisibleIndex="7" Width="10%" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="INPUTSGST" Caption="INPUTSGST" VisibleIndex="8" Width="100px" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>--%>
                                                    
                                                    <dxe:GridViewDataTextColumn FieldName="Sale GST(CGST+SGST)" Caption="GST% (CGST+SGST)" VisibleIndex="11" Width="15%" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>
                                                    
                                                    <dxe:GridViewDataTextColumn FieldName="Sale GST(CGST+UTGST)" Caption="GST% (CGST+UTGST)" VisibleIndex="12" Width="15%" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>
                                                                                                     
                                                    <dxe:GridViewDataTextColumn FieldName="INPUTIGST" Caption="IGST%" VisibleIndex="13" Width="10%" PropertiesTextEdit-DisplayFormatString="0.00">
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
                                                </TotalSummary>

                                            </dxe:ASPxGridView>

                                        </div>

                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>

                            <dxe:TabPage Name="purchase" Text="OUTPUT">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">

                                        <div onkeypress="OnWaitingGridKeyPress(event)">
                                            <dxe:ASPxGridView runat="server" ID="ShowGrid2" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false"
                                              
                                                OnDataBinding="grid2_DataBinding" ClientSideEvents-BeginCallback="Callback_EndCallback">

                                                <Columns>
                                                    
                                                    <dxe:GridViewDataTextColumn FieldName="slno" Caption="Sl" Width="5%" VisibleIndex="1">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="sProducts_Code" Caption="Product/Ledger Short Name" Width="25%" VisibleIndex="2">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="ProductClass_Description" Caption="Product Class" Width="16%" VisibleIndex="5">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="STYPE" Caption="Type" Width="4%" VisibleIndex="5">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="sProducts_HsnCode" Caption="HSN Code" Width="10%" VisibleIndex="6">
                                                    </dxe:GridViewDataTextColumn>
                                                    
                                                    <dxe:GridViewDataTextColumn FieldName="Purchase GST(CGST+SGST)" Caption="GST% (CGST+SGST)" VisibleIndex="17" Width="15%" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>
                                                    
                                                    <dxe:GridViewDataTextColumn FieldName="Purchase GST(CGST+UTGST)" Caption="GST% (CGST+UTGST)" VisibleIndex="18" Width="15%" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>
                                                                                                        
                                                    <dxe:GridViewDataTextColumn FieldName="OUTPUTIGST" Caption="IGST%" VisibleIndex="19" Width="10%" PropertiesTextEdit-DisplayFormatString="0.00">
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
                                                </TotalSummary>

                                            </dxe:ASPxGridView>

                                        </div>

                                    </dxe:ContentControl>
                                </ContentCollection>

                            </dxe:TabPage>

                        </TabPages>
                    </dxe:ASPxPageControl>
                </td>
            </tr>
        </table>
    </div>

    <div>
    </div>

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
</asp:Content>

