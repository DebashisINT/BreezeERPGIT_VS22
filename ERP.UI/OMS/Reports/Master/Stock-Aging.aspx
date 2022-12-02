<%@ Page Title="Stock Ageing" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false" AutoEventWireup="true"
    CodeBehind="Stock-Aging.aspx.cs" Inherits="ERP.OMS.Reports.Master.Stock_Aging" %>


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

            function OnWaitingGridKeyPress(e) {
                alert('1Hi');
                if (e.code == "Enter") {
                    alert('Hi');
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

        function Callback_agingBiginCallback() {


            $("#drdExport").val(0);
        }




        function btn_ShowRecordsClick(e) {
            e.preventDefault;
            var v = $("#ddlgstn").val();
            Grid.PerformCallback('ListData~' + v);
        }


        function OpenBillDetails(branch) {
            cgridPendingApproval.PerformCallback('BndPopupgrid~' + branch);
            cpopupApproval.Show();
            return true;
        }


        function popupHide(s, e) {
            cpopupApproval.Hide();
        }


    </script>


</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="panel-heading">
        <div class="panel-title">
            <h3>Stock Ageing</h3>

        </div>

    </div>
    <div class="form_main">
        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />
        <table class="pull-left">
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
                        <asp:Label ID="lblFromDate" runat="Server" Text="GSTIN : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                </td>
                <td>


                    <asp:DropDownList ID="ddlgstn" runat="server" Width="150px"></asp:DropDownList>

                </td>
                <td style="padding-left: 15px">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label2" runat="Server" Text="As On Date : " CssClass="mylabel1"
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


                <td></td>

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
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" ClientSideEvents-BeginCallback="Callback_agingBiginCallback"
                          OnSummaryDisplayText="ShowGrid_SummaryDisplayText"
                               Settings-HorizontalScrollBarMode="Auto" OnCustomCallback="Grid_CustomCallback" OnDataBinding="grid_DataBinding">

                            <Columns>

                                <dxe:GridViewDataTextColumn FieldName="sProducts_Description" Caption="Product Name" VisibleIndex="1" Width="200px">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Opening_Stock" Caption="Op. Stock" VisibleIndex="2" Width="70px">
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn FieldName="Purchase_During_Period" Caption="Purchase During Period" VisibleIndex="3" Width="150px" PropertiesTextEdit-DisplayFormatString="0.00" >
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn FieldName="Sales_Quantity_Sold" Caption="Qty Sold" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="0.00"   Width="70px">
                                </dxe:GridViewDataTextColumn>


                                 <dxe:GridViewDataTextColumn FieldName="Purchase_Quantity_Sold" Caption="Purc. Ret." VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="0.00"   Width="70px">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Stock_in_Hand" Caption="Stock in hand" VisibleIndex="6" Width="100px"  PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn FieldName="Serial_Number" Caption="Serial No." VisibleIndex="7"  Width="150px">
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn FieldName="Document_Date" Caption="GRN Date" VisibleIndex="8" Width="100px" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Document_No" Caption="GRN No." VisibleIndex="9" Width="150px" >
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Days_Ageing" Caption="Days Ageing" VisibleIndex="10" Width="100px" >
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="<30 Days" Caption="<30 Days" VisibleIndex="11" Width="100px" >
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="30-60 Days" Caption="30-60 Days" VisibleIndex="12" Width="100px" >
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="61-90 Days" Caption="61-90 Days" VisibleIndex="13" Width="100px" >
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="91-180 Days" Caption="91-180 Days" VisibleIndex="14" Width="100px" >
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="181 and Above" Caption="181 and Above" VisibleIndex="15"  Width="100px">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="branch_description" Caption="Branch Warehouse" VisibleIndex="16" PropertiesTextEdit-DisplayFormatString="0.00" Width="100px">
                                </dxe:GridViewDataTextColumn>



                            </Columns>

                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true"  />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                            </SettingsPager>

                              

                            <TotalSummary>
                                
                                  <dxe:ASPxSummaryItem FieldName="Stock_in_Hand" SummaryType="Sum" />
                            </TotalSummary>

                        </dxe:ASPxGridView>

                    </div>
                </td>
            </tr>
        </table>
    </div>

    <div>
    </div>

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A2" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>




</asp:Content>

