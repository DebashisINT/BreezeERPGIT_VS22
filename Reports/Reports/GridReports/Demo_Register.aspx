<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="Demo_Register.aspx.cs" 
    Inherits="Reports.Reports.GridReports.Demo_Register" %>

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

        #LBBranch {
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

        function ClearGridLookup() {
            var grid = gridquotationLookup.GetGridView();
            grid.UnselectRows();
        }

        $(function () {

            //BindBranches(null);
            //    BindCustomerVendor(0);


            cdeliverychallanComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);


            //    cProductComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);


            cPcategoryComponentPanel.PerformCallback('BindComponentGrid' + '~' + 2);
            cBranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + 2);

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


        function BindBranches(noteTilte) {
            var lBox = $('select[id$=LBBranch]');
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

                        $('#LBBranch').trigger("chosen:updated");
                        $('#LBBranch').prop('disabled', false).trigger("chosen:updated");
                    }
                    else {
                        lBox.empty();
                        $('#LBBranch').trigger("chosen:updated");
                        $('#LBBranch').prop('disabled', true).trigger("chosen:updated");

                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //  alert(textStatus);
                }
            });


        }






        function ListActivityType() {

            $('#LBBranch').chosen();
            $('#LBBranch').fadeIn();

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

                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');

            })

            $("#LBBranch").chosen().change(function () {
                var Ids = $(this).val();

                $('#<%=HFieldBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');

            })

                 <%-- $("#ListBoxLedgerType").chosen().change(function () {
                 var Ids = $(this).val();

                 $('#<%=hdnSelectedLedger.ClientID %>').val(Ids);
                 $('#MandatoryLedgerType').attr('style', 'display:none');

             })--%>

            $("#ListBoxCustomerVendor").chosen().change(function () {
                var Ids = $(this).val();

                $('#<%=hdnSelectedCustomerVendor.ClientID %>').val(Ids);
                $('#MandatoryCustomerType').attr('style', 'display:none');

            })

        })



        function BindCustomerVendor(type) {


            cProductComponentPanel.PerformCallback('BindComponentGrid' + '~' + 'CL');

        }




    </script>


    <script type="text/javascript">

        //Rev Subhra 18-12-2018  0017670
        function ShowGridEndCall() {
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = Grid.cpBranchNames
        }
        //End of Rev  

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

            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
            //CallServer(data, "");
            // alert( data);
            //Grid.PerformCallback(data);
            if (BranchSelection == "Yes" && gridLBBranchLookup.GetValue() == null) {
                jAlert('Please select atleast one branch for generate the report.');
            }
            else {
                Grid.PerformCallback(data);
            }

            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "For the period: " + FromDate + " To " + ToDate;

            //if (gridclassLookup.GetValue() == null) {
            //    jAlert('Please select atleast one Class.');
            //}
            //else if (gridcategoryLookup.GetValue() == null) {
            //    jAlert('Please select atleast one Category.');
            //}
            //else if (gridLBBranchLookup.GetValue() == null) {
            //    jAlert('Please select atleast one Branch.');
            //}
            //else {

            //    Grid.PerformCallback(data);
            //}
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

        function BudgetAfterHide(s, e) {
            popupbudget.Hide();
        }

        function CloseGridQuotationLookup() {
            gridcategoryLookup.ConfirmCurrentSelection();
            gridcategoryLookup.HideDropDown();
            gridcategoryLookup.Focus();
        }

        function CloseGridQuotationLookupbranch() {
            gridclassLookup.ConfirmCurrentSelection();
            gridclassLookup.HideDropDown();
            gridclassLookup.Focus();
        }


        function CloseGridQuotationLookupLBBranch() {
            gridLBBranchLookup.ConfirmCurrentSelection();
            gridLBBranchLookup.HideDropDown();
            gridLBBranchLookup.Focus();
        }

        function Callback2_EndCallback() {
            // alert('');
            $("#drdExport").val(0);
        }

        function selectAll() {
            gridclassLookup.gridView.SelectRows();
        }
        function unselectAll() {
            gridclassLookup.gridView.UnselectRows();
        }


        function selectAllLBBranch() {
            gridLBBranchLookup.gridView.SelectRows();
        }
        function unselectAllLBBranch() {
            gridLBBranchLookup.gridView.UnselectRows();
        }



        function selectAll_category() {
            gridcategoryLookup.gridView.SelectRows();
        }
        function unselectAll_category() {
            gridcategoryLookup.gridView.UnselectRows();
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
            <h3>Demo Register</h3>
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
                  <%--Rev Subhra 18-12-2018   0017670--%>
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
        <table class="pull-left">
            <tr>


                <td style="">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label1" runat="Server" Text="Class : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                </td>

                <td style="width: 234px">

                    <%--<asp:ListBox ID="ListBoxBranches" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>--%>
                    <asp:HiddenField ID="hdnActivityType" runat="server" />
                    <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                    <span id="MandatoryActivityType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                    <asp:HiddenField ID="hdnSelectedBranches" runat="server" />

                    <dxe:ASPxCallbackPanel runat="server" ID="panal_class" ClientInstanceName="cdeliverychallanComponentPanel" OnCallback="Componentchallan_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="lookup_class" SelectionMode="Multiple" runat="server" ClientInstanceName="gridclassLookup"
                                    OnDataBinding="lookup_class_DataBinding"
                                    KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                    <Columns>

                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                        <dxe:GridViewDataColumn FieldName="ProductClass_Code" Visible="true" VisibleIndex="1" Caption="Class code" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>

                                        <dxe:GridViewDataColumn FieldName="ProductClass_Name" Visible="true" VisibleIndex="2" Caption="Class" Settings-AutoFilterCondition="Contains">
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
                                                                <dxe:ASPxButton ID="ASPxButtonselect" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll" />
                                                            </div>
                                                            <dxe:ASPxButton ID="ASPxButton1unselect" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll" />                                                            
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


                <td style="padding-left: 15px">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label3" runat="Server" Text="Category : " CssClass="mylabel1"
                            Width="110px"></asp:Label>
                    </div>
                </td>
                <td style="width: 234px">
                    <%--<asp:ListBox ID="ListBoxCustomerVendor" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>--%>



                    <dxe:ASPxCallbackPanel runat="server" ID="ComponentcategoryPanel" ClientInstanceName="cPcategoryComponentPanel" OnCallback="Componentcategory_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="lookup_category" SelectionMode="Multiple" runat="server" TabIndex="7" ClientInstanceName="gridcategoryLookup"
                                    OnDataBinding="lookup_category_DataBinding"
                                    KeyFieldName="ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                        <dxe:GridViewDataColumn FieldName="Name" Visible="true" VisibleIndex="1" Caption="Name" Width="180" Settings-AutoFilterCondition="Contains">
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
                                                                <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_category" />
                                                            </div>
                                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_category" />                                                            
                                                            <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" UseSubmitBehavior="False" />
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



                    <asp:HiddenField ID="hdnSelectedCustomerVendor" runat="server" />

                    <span id="MandatoryCustomerType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>

                </td>


                <%--```````````````````````````````````````Suvankar```````````````````````````````````````--%>
                <td style="padding-left: 15px">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label2" runat="Server" Text="Branch : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                </td>

                <td style="width: 234px">

                    <%--<asp:ListBox ID="LBBranch" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnBranch  hide" data-placeholder="--- ALL ---"></asp:ListBox>--%>
                    <asp:HiddenField ID="HFieldActivityType" runat="server" />
                    <asp:HiddenField ID="HFieldActivityTypeText" runat="server" />
                    <span id="MandatoryActivityType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                    <asp:HiddenField ID="HFieldBranches" runat="server" />


                    <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cBranchComponentPanel" OnCallback="Branch_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="lookup_LBBranch" SelectionMode="Multiple" runat="server" ClientInstanceName="gridLBBranchLookup"
                                    OnDataBinding="lookup_LBBranch_DataBinding"
                                    KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="40" Caption=" " />
                                        <dxe:GridViewDataColumn FieldName="branch_code" Visible="true" VisibleIndex="1" Width="80" Caption="Branch code" Settings-AutoFilterCondition="Contains">
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
                                                                <dxe:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAllLBBranch" />
                                                            </div>
                                                            <dxe:ASPxButton ID="ASPxButton5" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAllLBBranch" />                                                            
                                                            <dxe:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookupLBBranch" UseSubmitBehavior="False" />
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
                <%--```````````````````````````````````````End```````````````````````````````````````--%>
            </tr>
            <tr>

                <td>
                    <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                        <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"
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
                <td style="padding-right: 10px;padding-left:10px; vertical-align: middle">
                 <asp:CheckBox ID="chkischallandate" runat="server" />
                 Search by Challan Date
                </td>
                
                <td style="padding-top: 3px">
                    <button id="btnShow" class="btn btn-primary" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
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
                </td>


            </tr>



            <tr>
            </tr>
        </table>
        <div class="pull-right">
        </div>
        <table class="TableMain100">


            <tr>

                <td colspan="2">
                    <div onkeypress="OnWaitingGridKeyPress(event)">
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                            OnSummaryDisplayText="ShowGrid_SummaryDisplayText" OnDataBound="Showgrid_Htmlprepared" OnDataBinding="ShowGrid_DataBinding" ClientSideEvents-BeginCallback="Callback2_EndCallback"
                            OnCustomCallback="Grid_CustomCallback" Settings-HorizontalScrollBarMode="Visible"
                            Settings-VerticalScrollableHeight="180" Settings-VerticalScrollBarMode="Auto">
                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="Sl.No." Caption="Sl.No." Width="60px" VisibleIndex="0" />
                                <dxe:GridViewDataTextColumn FieldName="BRANCH NAME" Caption="Branch Name" Width="120px" VisibleIndex="1" />
                                <dxe:GridViewDataTextColumn FieldName="INVOICE DATE" Caption="Invoice Date" Width="120px" VisibleIndex="2" />
                                <dxe:GridViewDataTextColumn FieldName="INVOICE NUMBER" Caption="Invoice Number" Width="120px" VisibleIndex="3" />
                                <dxe:GridViewDataTextColumn FieldName="BRAND" Caption="Brand" Width="120px" VisibleIndex="4" />
                                <dxe:GridViewDataTextColumn FieldName="PRODUCT DESCRIPTION" Caption="Product Description" Width="120px" VisibleIndex="5" />
                                <dxe:GridViewDataTextColumn FieldName="QUANTITY" Caption="Quantity" Width="120px" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="CUSTOMER NAME" Caption="Customer Name" Width="120px" VisibleIndex="7" />
                                <dxe:GridViewDataTextColumn FieldName="CUSTOMER BILLING ADDRESS" Caption="Customer Billing Address" Width="120px" VisibleIndex="8" />
                                <dxe:GridViewDataTextColumn FieldName="CUSTOMER SHIPPING ADDRESS" Caption="Customer Shipping Address" Width="120px" VisibleIndex="9" />
                                <dxe:GridViewDataTextColumn FieldName="Pincode" Caption="Pincode" Width="120px" VisibleIndex="10" />
                                <dxe:GridViewDataTextColumn FieldName="MOBILE NUMBER" Caption="Mobile Number" Width="120px" VisibleIndex="11" />
                                <dxe:GridViewDataTextColumn FieldName="ALTERNATE NUMBER" Caption="Alternate Number" Width="120px" VisibleIndex="12" />
                                <dxe:GridViewDataTextColumn FieldName="Email Id" Caption="Email Id" Width="120px" VisibleIndex="13" />
                                <dxe:GridViewDataTextColumn FieldName="DELIVERED DATE" Caption="Delivered Date" Width="120px" VisibleIndex="14" />
                                <dxe:GridViewDataTextColumn FieldName="ISD Name" Caption="ISD Name" Width="120px" VisibleIndex="15" />
                                <dxe:GridViewDataTextColumn FieldName="ISD CONTACT NUMBER" Caption="ISD Contact Number" Width="120px" VisibleIndex="16" />
                                <dxe:GridViewDataTextColumn FieldName="Reference" Caption="Reference" Width="120px" VisibleIndex="17" />
                                <dxe:GridViewDataTextColumn FieldName="Remarks" Caption="Remarks" Width="120px" VisibleIndex="18" />
                                <dxe:GridViewDataTextColumn FieldName="PRODUCT SERIAL NO." Caption="Product serial No." Width="120px" VisibleIndex="19" />
                                <dxe:GridViewDataTextColumn FieldName="CUSTOMER DELIVERY CHALLAN No" Caption="Customer Delivery Challan No." Width="120px" VisibleIndex="20" />
                                <dxe:GridViewDataTextColumn FieldName="CUSTOMER DELIVERY CHALLAN DATE" Caption="Customer Delivery Challan Date" Width="120px" VisibleIndex="21" />
                                <dxe:GridViewDataTextColumn FieldName="Financier Name" Caption="Financier Name" Width="120px" VisibleIndex="22" />
                                <dxe:GridViewDataTextColumn FieldName="Financier Approval Code" Caption="Financier Approval Code" Width="120px" VisibleIndex="23" />
                            </Columns>
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                             <%--Rev Subhra 18.12.2018  0017670 --%> 
                                <ClientSideEvents EndCallback="ShowGridEndCall" />
                            <%--End of Rev--%>
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" ColumnResizeMode="Control" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                            </SettingsPager>
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />

                            <TotalSummary>
                            </TotalSummary>
                        </dxe:ASPxGridView>

                    </div>
                </td>
            </tr>
        </table>
    </div>

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupbudget" Height="500px"
        Width="1200px" HeaderText="Details" Modal="true" AllowResize="true">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>

        <ClientSideEvents CloseUp="BudgetAfterHide" />
    </dxe:ASPxPopupControl>

    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
</asp:Content>


