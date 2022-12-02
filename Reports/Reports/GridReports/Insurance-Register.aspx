<%@ Page Title="Insurance Register" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="Insurance-Register.aspx.cs"
    Inherits="Reports.Reports.GridReports.Insurance_Register" %>


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
            //var data = "OnDateChanged";
            var v = $("#ddlgstn").val();
            var activeTab = page.GetActiveTab();
            if (activeTab.name == 'sales') {
                Grid.PerformCallback('ListData~' + v);
            }
            if (activeTab.name == 'purchase') {
                Gridpurchase.PerformCallback('ListData~' + v);
            }

            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "For the period: " + FromDate + " To " + ToDate;
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



        $(function () {
            $('body').on('change', '#ddlgstn', function () {


                ///  alert($("#ddlgstn").val());
                if ($("#ddlgstn").val()) {
                    cProductbranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlgstn").val());

                }

                else {

                    cProductbranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);
                }
            });
        });
        ///$("#ddlgstn").on('se')


        function selectAll_branch() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAll_branch() {
            gridbranchLookup.gridView.UnselectRows();
        }


        function CloseGridQuotationLookupbranch() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }



    </script>


    <style>
        .tablpad > tbody > tr > td {
            padding: 0 10px;
        }
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


    <div class="panel-heading">
       <%-- <div class="panel-title">
            <h3>Insurance Register </h3>
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
        <div>
            <%--<div class="col-md-3">
                <label>
                    <asp:Label ID="lblFromDate" runat="Server" Text="GSTIN : " CssClass="mylabel1"></asp:Label></label>
                <asp:DropDownList ID="ddlgstn" runat="server" Width="100%"></asp:DropDownList>
            </div>--%>


            <div class="col-md-2" >
                <label>
                    <asp:Label ID="lblbranch" runat="Server" Text="GSTIN : " CssClass="mylabel1"></asp:Label></label>
                <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cProductbranchComponentPanel" OnCallback="Componentbranch_Callback">
                    <PanelCollection>

                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookup_branch" SelectionMode="Multiple" runat="server" ClientInstanceName="gridbranchLookup"
                                OnDataBinding="lookup_branch_DataBinding"
                                KeyFieldName="branch_GSTIN" Width="100%"  AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <%--DataSourceID="BranchEntityServerModeDataSource" --%>
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="branch_GSTIN" Visible="true" VisibleIndex="1" Caption="GSTIN" Settings-AutoFilterCondition="Contains">
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
                                                            <dxe:ASPxButton ID="ASPxButtonselect" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_branch" />
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton1unselect" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_branch" />                                                            
                                                        <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookupbranch" UseSubmitBehavior="False" />
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
            </div>

            <div class="col-md-2">
                <label>From date</label>
                <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
            </div>

            <div class="col-md-2">
                <label>
                    <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"></asp:Label></label>
                <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
            </div>


            <div class="col-md-2">
                <label>&nbsp</label><br />
                <asp:CheckBox runat="server" ID="chkparty" Text="Search by party invoice date" />
            </div>


            <div class="col-md-3" style="padding-top: 15px;">
                <button id="btnShow" class="btn btn-primary" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                    OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">PDF</asp:ListItem>
                    <asp:ListItem Value="2">XLSX</asp:ListItem>
                    <asp:ListItem Value="3">RTF</asp:ListItem>
                    <asp:ListItem Value="4">CSV</asp:ListItem>

                </asp:DropDownList>
            </div>

        </div>
        <table class="pull-left tablpad">
            <tr>



                <td style="width: 254px; display: none">

                    <%--
                        <asp:ListBox ID="ListBoxBranches" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>--%>

                    <asp:HiddenField ID="hdnActivityType" runat="server" />
                    <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                    <span id="MandatoryActivityType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                    <asp:HiddenField ID="hdnSelectedBranches" runat="server" />


                </td>



                <td>
                    <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                    </div>
                </td>


            </tr>


        </table>
        <div class="clear"></div>
        <table class="tablpad">

            <tr>

                <td></td>
                <td style="padding-left: 15px">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    </div>
                </td>
                <td></td>

                <td style="padding-left: 10px; padding-top: 3px"></td>

            </tr>
        </table>
        <div class="pull-right">
        </div>
        <table class="TableMain100">

            <tr>
                <td colspan="2">

                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" ClientInstanceName="page"
                        Font-Size="12px" Width="100%">
                        <TabPages>


                            <dxe:TabPage Name="sales" Text="Insurance Sales Register">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">



                                        <div>
                                            <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false"
                                                OnSummaryDisplayText="ShowGrid_SummaryDisplayText" OnDataBound="Showgrid_Datarepared"
                                                Settings-HorizontalScrollBarMode="Visible"
                                                OnCustomSummaryCalculate="ASPxGridView1_CustomSummaryCalculate"
                                                OnCustomCallback="Grid_CustomCallback" OnDataBinding="grid_DataBinding" ClientSideEvents-BeginCallback="Callback_EndCallback"
                                                Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Auto">

                                                <Columns>

                                                    <dxe:GridViewDataTextColumn FieldName="Branchname" Caption="Branch Name" VisibleIndex="0">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Date" Caption="Doc Date" VisibleIndex="1" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Number" Caption="Doc Number" VisibleIndex="2" Width="120px">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Customer" Caption="Party" VisibleIndex="3">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="GSTIN" Caption="GSTIN" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="City" Caption="City" VisibleIndex="5">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="state" Caption="State" VisibleIndex="5">
                                                    </dxe:GridViewDataTextColumn>



                                                    <dxe:GridViewDataTextColumn FieldName="Qty" Caption="QTY" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>



                                                    <dxe:GridViewDataTextColumn FieldName="InvoiceDetails_ProductDescription" Caption="Product" VisibleIndex="7">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="invnoninv" Caption="Inventory/Non Inventory item" VisibleIndex="8">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="sProducts_HsnCode" Caption="HSN/SAC" VisibleIndex="9">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="TransporterName" Caption="Transporter Name" VisibleIndex="10">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="LRNo" Caption="CN no." VisibleIndex="11">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="TAxAmt" Caption="Taxable Value" CellStyle-HorizontalAlign="Right" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="CGSTRate" Caption="CGST" VisibleIndex="13" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="SGSTRate" Caption="SGST" VisibleIndex="14" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="IGSTRate" Caption="IGST" VisibleIndex="15" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Net" Caption="Net" CellStyle-HorizontalAlign="Right" VisibleIndex="16" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="othertax" Caption="Other tax" VisibleIndex="17" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="oldunitfet" Caption="Old Unit" VisibleIndex="18" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Totalnet" Caption="Total" Width="120px" VisibleIndex="19" PropertiesTextEdit-DisplayFormatString="0.00">
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
                                                <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                                <TotalSummary>
                                                    <dxe:ASPxSummaryItem FieldName="Net" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="CGSTRate" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="SGSTRate" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="IGSTRate" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="TAxAmt" SummaryType="Sum" />

                                                    <dxe:ASPxSummaryItem FieldName="Qty" SummaryType="Sum" />

                                                    <dxe:ASPxSummaryItem FieldName="othertax" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Totalnet" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="oldunitfet" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Number" SummaryType="Custom" DisplayFormat="Count" />

                                                </TotalSummary>

                                            </dxe:ASPxGridView>

                                        </div>




                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>


                            <dxe:TabPage Name="purchase" Text="Insurance Purchase Register">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">

                                        <div>
                                            <dxe:ASPxGridView runat="server" ID="grid_purchase" ClientInstanceName="Gridpurchase" Width="100%" EnableRowsCache="false"
                                                OnSummaryDisplayText="ShowGridPurchase_SummaryDisplayText" Settings-HorizontalScrollBarMode="Auto"
                                                OnCustomCallback="Grid_Purchase_CustomCallback" OnDataBinding="gridpurchase_DataBinding"
                                                ClientSideEvents-BeginCallback="Callback_EndCallback"
                                                OnCustomSummaryCalculate="ASPxGridpurchase_CustomSummaryCalculate">

                                                <Columns>



                                                    <dxe:GridViewDataTextColumn FieldName="Branchname" Caption="Branch Name" VisibleIndex="0">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Date" Caption="Doc. Date" VisibleIndex="1" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Number" Caption="Doc. Number" VisibleIndex="2">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="GSTIN" Caption="Supplier's GSTIN" VisibleIndex="3" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Customer" Caption="Supplier's  Name" VisibleIndex="4">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="VEND_CITY" Caption="City" VisibleIndex="4">
                                                    </dxe:GridViewDataTextColumn>
                                                    
                                                    <dxe:GridViewDataTextColumn FieldName="VEND_STATE" Caption="State" VisibleIndex="4">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="City" Caption="Destination City" VisibleIndex="4">
                                                    </dxe:GridViewDataTextColumn>
                                                    
                                                    <dxe:GridViewDataTextColumn FieldName="state" Caption="Destination State" VisibleIndex="4">
                                                    </dxe:GridViewDataTextColumn>




                                                    <dxe:GridViewDataTextColumn FieldName="Grn" Caption="GRN. No." VisibleIndex="5">
                                                    </dxe:GridViewDataTextColumn>



                                                    <dxe:GridViewDataTextColumn FieldName="grndate" Caption="GRN.Date" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                                                    </dxe:GridViewDataTextColumn>






                                                    <dxe:GridViewDataTextColumn FieldName="partyinvoice" Caption="Invoice No." VisibleIndex="7">
                                                    </dxe:GridViewDataTextColumn>



                                                    <dxe:GridViewDataTextColumn FieldName="Partyinvdate" Caption="Invoice Date" VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="branch_description" Caption="GRN Branch" VisibleIndex="9">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="invnoninv" Caption="Inventory/Non Inventory item" VisibleIndex="10">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="InvoiceDetails_ProductDescription" Caption="Product Description" VisibleIndex="13">
                                                    </dxe:GridViewDataTextColumn>



                                                    <dxe:GridViewDataTextColumn FieldName="TransporterName" Caption="Transporter Name" VisibleIndex="14">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="LRNo" Caption="CN no." VisibleIndex="15">
                                                    </dxe:GridViewDataTextColumn>





                                                    <dxe:GridViewDataTextColumn FieldName="Qty" Caption="QTY" VisibleIndex="16" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Rateunit" Caption="Rate Per Unit" VisibleIndex="17" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>




                                                    <dxe:GridViewDataTextColumn FieldName="TAxAmt" Caption="Purchase Price (Before tax)" CellStyle-HorizontalAlign="Right" VisibleIndex="18" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>



                                                    <dxe:GridViewDataTextColumn FieldName="dicount" Caption="Discount %" VisibleIndex="19" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>



                                                    <dxe:GridViewDataTextColumn FieldName="disamnt" Caption="Discount Amount" VisibleIndex="20" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>






                                                    <dxe:GridViewDataTextColumn FieldName="CGSTRate2" Caption="CGST%" VisibleIndex="21" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="CGSTRate" Caption="CGST Amount" VisibleIndex="22" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="SGSTRate2" Caption="SGST%" VisibleIndex="23" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="SGSTRate" Caption="SGST Amount" VisibleIndex="24" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="IGSTRate2" Caption="IGST%" VisibleIndex="25" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="IGSTRate" Caption="IGST" VisibleIndex="26" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Net" Caption="Amount with Tax" VisibleIndex="27" CellStyle-HorizontalAlign="Right" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="othertax" Caption="Other Charges" VisibleIndex="28" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Totalnet" Caption="Total Bill Amount" VisibleIndex="29" Width="120px" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                </Columns>

                                                <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                                                <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                                                <SettingsEditing Mode="EditForm" />
                                                <SettingsContextMenu Enabled="true" />
                                                <SettingsBehavior AutoExpandAllGroups="true" />
                                                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                                                <SettingsSearchPanel Visible="false" />
                                                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                                                <SettingsPager PageSize="10">
                                                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                                </SettingsPager>

                                                <TotalSummary>
                                                    <dxe:ASPxSummaryItem FieldName="TAxAmt" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Net" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="CGSTRate" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="SGSTRate" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="IGSTRate" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Qty" SummaryType="Sum" />

                                                    <dxe:ASPxSummaryItem FieldName="othertax" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Totalnet" SummaryType="Sum" />

                                                    <dxe:ASPxSummaryItem FieldName="Number" SummaryType="Custom" DisplayFormat="Count" />

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

