<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="SerialwiseDetails.aspx.cs" Inherits="Reports.Reports.GridReports.SerialwiseDetails" %>

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
        .branch-selection-box .dxlpLoadingPanel_PlasticBlue tbody, .branch-selection-box .dxlpLoadingPanelWithContent_PlasticBlue tbody, #divProj .dxlpLoadingPanel_PlasticBlue tbody, #divProj .dxlpLoadingPanelWithContent_PlasticBlue tbody
        {
            bottom: 0 !important;
        }
        .dxlpLoadingPanel_PlasticBlue tbody, .dxlpLoadingPanelWithContent_PlasticBlue tbody
        {
            position: absolute;
            bottom: 150px;
            background: #fff;
            box-shadow: 1px 1px 10px #0000001c;
            right: 55%;
        }
        /*rev end Pallab*/  
    </style>


    <script type="text/javascript">

        function ClearGridLookup() {
            var grid = gridquotationLookup.GetGridView();
            grid.UnselectRows();
        }

        $(function () {

            //  BindBranches(null);
            //    BindCustomerVendor(0);
            ///   cProductComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);


            $("#drp_partytype").change(function () {
                var end = $("#drp_partytype").val();
                //var lBox = $('select[id$=ListBoxCustomerVendor]');
                //lBox.append('');
                //$('#ListBoxCustomerVendor').empty();
                //$('#ListBoxCustomerVendor').trigger("chosen:updated");
                //$('#ListBoxCustomerVendor').val('').trigger("chosen:updated");
                //$('#hdnSelectedCustomerVendor').val('');
                //  alert(end);
                //  gridquotationLookup.UnselectRows();

                //   ClearGridLookup();

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
                url: 'PartyLedgerPostingReport.aspx/GetBranchesList',
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

        function ListCustomerVendor() {

            $('#ListBoxCustomerVendor').chosen();
            $('#ListBoxCustomerVendor').fadeIn();

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

            $("#ListBoxCustomerVendor").chosen().change(function () {
                var Ids = $(this).val();


                $('#MandatoryCustomerType').attr('style', 'display:none');

            })

        })




        function BindCustomerVendor(type) {


            cProductComponentPanel.PerformCallback('BindComponentGrid' + '~' + type);



        }


        function OpenPOSDetails(Uniqueid, type) {

            //    alert(type);

            type = type.trim();
            var url = '';
            if (type == 'BI') {

                url = '/OMS/Management/Activities/BranchTransferIn.aspx?key=' + Uniqueid + '&IsTagged=1&req=V';
            }
            else if (type == 'BO') {

                url = '/OMS/Management/Activities/BranchTransferOut.aspx?key=' + Uniqueid + '&IsTagged=1&req=V';
            }


            else if (type == 'PC') {


                //url = '/OMS/Management/Activities/PurchaseChallan.aspx?key=' + Uniqueid + '&req=V&IsTagged=1&type=' + type;
                url = '/OMS/Management/Activities/PurchaseChallan.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=PC';
            }

            else if (type == 'PI') {

                url = '/OMS/Management/Activities/PurchaseInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=PB';

            }
            else if (type == 'PR') {
                url = '/OMS/Management/Activities/PReturn.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=PR';
            }


            else if (type == 'SC') {
                //   alert('');
                url = '/OMS/Management/Activities/SalesChallanAdd.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }


            else if (type == 'SR') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                // window.open('/OMS/Management/Activities/SalesReturn.aspx?key=' + Uniqueid + '&req=V&type=' + type, '_blank')
                url = '/OMS/Management/Activities/SalesReturn.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }

            else if (type == 'SISRM') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                //  window.open('/OMS/Management/Activities/ReturnManual.aspx?key=' + Uniqueid + '&req=V&type=' + type, '_blank')
                url = '/OMS/Management/Activities/ReturnManual.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }

            else if (type == 'SISRN') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                //  window.open('/OMS/Management/Activities/ReturnNormal.aspx?key=' + Uniqueid + '&req=V&type=' + type, '_blank')
                url = '/OMS/Management/Activities/ReturnNormal.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;

            }

            else if (type == 'OUR') {

                url = '/OMS/Management/Activities/OldUnitReceivedFromServiceCenter.aspx?key=' + Uniqueid + '&IsTagged=1&ViewMode=yes';

            }


            else if (type == 'CRI') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                // window.open('/OMS/Management/Activities/SalesReturn.aspx?key=' + Uniqueid + '&req=V&type=' + type, '_blank')
                url = '/OMS/Management/Activities/SalesReturn.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }



            else if (type == 'CR') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                // window.open('/OMS/Management/Activities/SalesReturn.aspx?key=' + Uniqueid + '&req=V&type=' + type, '_blank')
                url = '/OMS/Management/Activities/CustomerReturn.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }


            popupbudget.SetContentUrl(url);
            popupbudget.Show();
        }
        function BudgetAfterHide(s, e) {
            popupbudget.Hide();
        }


        function Callback_EndCallback() {

            // alert('');
            $("#drdExport").val(0);
        }

        function CallbackPanelEndCall(s, e) {
            Grid.Refresh();
        }

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
            e.preventDefault;
            $("#hfIsSerialDetailsFilter").val("Y");
            var data = "OnDateChanged";

            if ($("#txtserial").val() == '') {
                jAlert('Please input Serial Number');
            }

            else {
                //Grid.PerformCallback(data);
                cCallbackPanel.PerformCallback(data);
            }
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




        //function BudgetAfterHide(s, e) {
        //    popupbudget.Hide();
        //}

        function CloseGridQuotationLookup() {
            gridquotationLookup.ConfirmCurrentSelection();
            gridquotationLookup.HideDropDown();
            gridquotationLookup.Focus();
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
    <script>
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
       <%-- <div class="panel-title">
            <h3>Serial No. Details</h3>
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
        <div class="row">
           <div class="col-md-2">
               <label>
                   <asp:Label ID="Label2" runat="Server" Text="Serial Number : " CssClass="from-label"
                            Width="100%"></asp:Label>
               </label>
               <div>
                   <asp:TextBox ID="txtserial" runat="server" Width="100%"></asp:TextBox>
               </div>
           </div>
            <div class="col-md-4 mBot10" style="padding-top: 18px;">
                <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                    <%-- <% if (rights.CanExport)
                           { %>--%>

                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary"
                        OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>

                    </asp:DropDownList>
                    <%-- <% } %>--%>
            </div>
            <div class="col-md-2"></div>
        </div>


        
        <div class="pull-right">
        </div>
        <table class="TableMain100">


            <tr>

                <td colspan="2">
                    <div onkeypress="OnWaitingGridKeyPress(event)">
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SEQ"
                            ClientSideEvents-BeginCallback="Callback_EndCallback" DataSourceID="GenerateEntityServerModeDataSource"
                            Settings-VerticalScrollableHeight="200" Settings-VerticalScrollBarMode="Auto">
                            <%--OnCustomCallback="Grid_CustomCallback" OnDataBinding="grid2_DataBinding"--%>
                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="branch" Caption="Unit" VisibleIndex="1" />

                                <dxe:GridViewDataTextColumn FieldName="Stock_IN_OUT_Date" Caption="Stock In/Out Date" VisibleIndex="2" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" />

                              <%--  <dxe:GridViewDataTextColumn FieldName="DOC_Date" Caption="Doc. Date" VisibleIndex="3" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" />--%>

                                <%--       <dxe:GridViewDataTextColumn FieldName="DOC_Number" Caption="Doc. No."  VisibleIndex="4"  />--%>


                                <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="DOC_Number" Caption="Doc. No." Width="20%">
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>

                                        <a href="javascript:void(0)" onclick="OpenPOSDetails('<%#Eval("DOC_ID") %>','<%#Eval("type1") %>')" class="pad">
                                            <%#Eval("DOC_Number")%>
                                        </a>
                                    </DataItemTemplate>

                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Doc_Type" Caption="Doc Type" VisibleIndex="4" />

                                <dxe:GridViewDataTextColumn FieldName="SerialNo" Caption="Serial No" VisibleIndex="5" />

                            </Columns>
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" ColumnResizeMode="Control" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>

                        </dxe:ASPxGridView>
                        <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="SERIALDETAILS_REPORT" ></dx:LinqServerModeDataSource>

                    </div>
                </td>
            </tr>
        </table>
    </div>



    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
    <PanelCollection>
        <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfIsSerialDetailsFilter" runat="server" />
        </dxe:PanelContent>
    </PanelCollection>
      <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupbudget" Height="500px"
        Width="1200px" HeaderText="Details" Modal="true" AllowResize="true">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>

        <ClientSideEvents CloseUp="BudgetAfterHide" />
    </dxe:ASPxPopupControl>
</asp:Content>
