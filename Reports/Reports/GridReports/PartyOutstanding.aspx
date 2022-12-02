<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false" 
    AutoEventWireup="true" CodeBehind="PartyOutstanding.aspx.cs" Inherits="Reports.Reports.GridReports.PartyOutstanding" %>
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
            bottom: 50px;
            background: #fff;
            box-shadow: 1px 1px 10px #0000001c;
            right: 50%;
        }
        /*rev end Pallab*/
    </style>


    <script type="text/javascript">

        //function ClearGridLookup() {
        //    var grid = gridquotationLookup.GetGridView();
        //    grid.UnselectRows();
        //}
        //function cbtnPreviewClick() {
        //    LoadingPanel.Show();
        //}

        $(function () {
            cbranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);
            cGroupComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);

            //cProductComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);

            $("#drp_partytype").change(function () {
                var end = $("#drp_partytype").val();
                $("#ckpar").attr('style', 'display:inline-block; padding-left: 15px; padding-top: 3px;color: #b5285f; font-weight: bold;" class="clsTo;')

                if (end == 'CL') {

                    //$("#Label3").text('Customer');
                    //$("#ckpar").attr('style', 'display:none; padding-left: 15px; padding-top: 3px;color: #b5285f; font-weight: bold;" class="clsTo;');
                    $("#GrpSelLbl").attr('style', 'display:none;');
                    $("#GrpSel").attr('style', 'display:none;');
                    $("#ckpar").attr('style', 'display:none;');
                }
                else if (end == 'DV') {

                    //$("#Label3").text('Vendor');
                    $("#GrpSelLbl").attr('style', 'color: #b5285f; font-weight: bold;" class="clsTo;display:block;');
                    $("#GrpSel").attr('style', 'color: #b5285f; font-weight: bold;" class="clsTo;display:block;');
                    $("#ckpar").attr('style', 'display:block;');
                }
                else if (end == 'ALL') {

                    //$("#Label3").text('Customer/Vendor');
                    $("#GrpSelLbl").attr('style', 'color: #b5285f; font-weight: bold;" class="clsTo;display:block;');
                    $("#GrpSel").attr('style', 'color: #b5285f; font-weight: bold;" class="clsTo;display:block;');
                    $("#ckpar").attr('style', 'display:block;');
                }
                gridGroupLookup.SetValue(null);
                //BindCustomerVendor(end);
            });

            function OnWaitingGridKeyPress(e) {

                if (e.code == "Enter" || e.code == "NumpadEnter") {
                    var index = Grid.GetFocusedRowIndex();
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




        $(document).ready(function () {
            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');
            })

            $("#ListBoxGroup").chosen().change(function () {  
                var Ids = $(this).val();

                $('#<%=hdnSelectedGroups.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');

            })


            $("#ListBoxCustomerVendor").chosen().change(function () {
                var Ids = $(this).val();

                $('#<%=hdnSelectedCustomerVendor.ClientID %>').val(Ids);
                $('#MandatoryCustomerType').attr('style', 'display:none');

            })

        })


        function BindCustomerVendor(type) {

            //cProductComponentPanel.PerformCallback('BindComponentGrid' + '~' + type);

        }

        function CallbackPanelEndCall(s, e) {
            <%--Rev Subhra 18-12-2018   0017670--%>
                document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
                //End of Subhra
            Grid.Refresh();
    }
    </script>


    <script type="text/javascript">
        function cxdeToDate_OnChaged(s, e) {
            var data = "OnDateChanged";
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
        }

        function btn_ShowRecordsClick(e) {
            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            e.preventDefault;
            var data = "OnDateChanged";
            $("#hfIsPartyOutstanding").val("Y");
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();

            //if (gridquotationLookup.GetValue() == null) {
            //    jAlert('Please select atleast one Party.');
            //}
            //else {
            //LoadingPanel.Show();
            //Grid.PerformCallback(data);
            //}
            //cCallbackPanel.PerformCallback(data);
            if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                jAlert('Please select atleast one branch for generate the report.');
            }
            else {
                cCallbackPanel.PerformCallback(data);
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


        function Groupwiseledger() {
            var key = gridGroupLookup.GetGridView().GetRowKey(gridGroupLookup.GetGridView().GetFocusedRowIndex());
            //cProductComponentPanel.PerformCallback('BindComponentGrid' + '~' + 'GrpWs');

        }


        //function OpenPOSDetails(Uniqueid, type) {

        //    // alert(type);
        //    var url = '';
        //    if (type == 'POS') {
        //        url = '/OMS/Management/Activities/posSalesInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&Viemode=1';
        //    }
        //    else if (type == 'SI') {
        //        url = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
        //    }

        //    else if (type == 'PC') {
        //        url = '/OMS/Management/Activities/PurchaseChallan.aspx?key=' + Uniqueid + '&req=V&IsTagged=1&type=' + type;
        //    }
        //    else if (type == 'SR') {
        //        url = '/OMS/Management/Activities/SalesReturn.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
        //    }

        //    else if (type == 'SRM') {
        //        url = '/OMS/Management/Activities/ReturnManual.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
        //    }

        //    else if (type == 'SRN') {
        //        url = '/OMS/Management/Activities/ReturnNormal.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
        //    }
        //    else if (type == 'PI') {
        //        url = '/OMS/Management/Activities/PurchaseInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=PB';

        //    }
        //    else if (type == 'VP' || type == 'VR') {
        //        url = '/OMS/Management/Activities/VendorPaymentReceipt.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=VPR';

        //    }

        //    else if (type == 'PR') {
        //        url = '/OMS/Management/Activities/PReturn.aspx.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=PR';

        //    }
        //    else if (type == 'SC') {
        //        url = '/OMS/Management/Activities/CustomerReturn.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
        //    }

        //    else if (type == 'CP' || type == 'CR') {
        //        url = '/OMS/Management/Activities/CustomerReceiptPayment.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=CRP';
        //    }

        //    else if (type == 'CDN' || type == 'CCN') {
        //        url = '/OMS/Management/Activities/CustomerNote.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=CDCN';
        //    }
        //    else if (type == 'VCN' || type == 'VDN') {
        //        url = '/OMS/Management/Activities/VendorDebitCreditNote.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;;
        //    }
        //    else if (type == 'TPB') {
        //        url = '/OMS/Management/Activities/TPurchaseInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
        //    }
        //    else if (type == 'TSI') {
        //        url = '/OMS/Management/Activities/TSalesInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
        //    }

        //    popupbudget.SetContentUrl(url);
        //    popupbudget.Show();
        //}
        //function BudgetAfterHide(s, e) {
        //    popupbudget.Hide();
        //}

        //function CloseGridQuotationLookup() {
        //    gridquotationLookup.ConfirmCurrentSelection();
        //    gridquotationLookup.HideDropDown();
        //    gridquotationLookup.Focus();
        //}
        function CloseGridQuotationLookupGroup() {  
            gridGroupLookup.ConfirmCurrentSelection();
            gridGroupLookup.HideDropDown();
            gridGroupLookup.Focus();
        }
        function CloseGridQuotationLookupbranch() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }

        function Callback2_EndCallback() {
             //alert('');
            $("#drdExport").val(0);
            Grid.Focus();
            Grid.SetFocusedRowIndex(0);
            //LoadingPanel.Hide();
            //cCallbackPanel.Focus();
            //cCallbackPanel.SetFocusedRowIndex(1);
        }


        //function selectAll() {
        //    gridquotationLookup.gridView.SelectRows();
        //}
        //function unselectAll() {
        //    gridquotationLookup.gridView.UnselectRows();
        //}
        function selectAllGroup() {  
            gridGroupLookup.gridView.SelectRows();
        }
        function unselectAllGroup() {  
            gridGroupLookup.gridView.UnselectRows();
        }
        function selectAll_branch() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAll_branch() {
            gridbranchLookup.gridView.UnselectRows();
        }



    </script>
    <style>
        .padTop30 {
            padding-top:25px;
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
        <%--<div class="panel-title">
            <h3>Party Outstanding</h3>
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
                    <%--Rev Subhra 11-12-2018   0017670--%>
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
            <div class="col-md-2 branch-selection-box">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </label>
                <%--<asp:ListBox ID="ListBoxBranches" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>--%>
                    <asp:HiddenField ID="hdnActivityType" runat="server" />
                    <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                    <span id="MandatoryActivityType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                    <asp:HiddenField ID="hdnSelectedBranches" runat="server" />


                    <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cbranchComponentPanel" OnCallback="Componentbranch_Callback">
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
                                                                <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_branch" />
                                                            <%--</div>--%>
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
            </div>
            <div class="col-md-2">
                 <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label2" runat="Server" Text="Criteria : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                   </label>
                <asp:DropDownList ID="drp_partytype" runat="server" Width="100%">

                        <asp:ListItem Text="All" Value="ALL"></asp:ListItem>
                        <asp:ListItem Text="Customer" Value="CL"></asp:ListItem>
                        <asp:ListItem Text="Vendor" Value="DV"></asp:ListItem>

                    </asp:DropDownList>
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label3" runat="Server" Text="Balance Sort : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </label>
                <asp:DropDownList ID="drp_balancetype" runat="server" Width="100%">
                    <asp:ListItem Text="All" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Only Balance" Value="1"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-2 branch-selection-box">
                <label id="GrpSelLbl" style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label4" runat="Server" Text="Group : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </label>
                <div id="GrpSel">
                        <%--<asp:ListBox ID="ListBoxGroup" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>--%>
                        <asp:HiddenField ID="hdnSelectedGroups" runat="server" />
                        <asp:HiddenField ID="HiddenField2" runat="server" />
                        <span id="MandatoryActivityType" style="display: none" class="validclass">
                            <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                        <asp:HiddenField ID="HiddenField3" runat="server" />


                        <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel2" ClientInstanceName="cGroupComponentPanel" OnCallback="ComponentGroup_Callback">
                            <PanelCollection>
                                <dxe:PanelContent runat="server">
                                    <dxe:ASPxGridLookup ID="lookup_Group" SelectionMode="Multiple" runat="server" ClientInstanceName="gridGroupLookup"
                                        OnDataBinding="lookup_Group_DataBinding"
                                        KeyFieldName="GroupCode" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                        <Columns>
                                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                            <dxe:GridViewDataColumn FieldName="GroupCode" Visible="true" Width="0" VisibleIndex="1" Caption="Group Code" Settings-AutoFilterCondition="Contains">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>

                                            <dxe:GridViewDataColumn FieldName="GroupDescription" Visible="true" Width="160" VisibleIndex="2" Caption="Group Description" Settings-AutoFilterCondition="Contains">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>
                                        </Columns>
                                        <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                            <Templates>
                                                <StatusBar>
                                                    <table class="OptionsTable" style="float: right">
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxButton ID="ASPxButtonselect" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAllGroup" />
                                                                <dxe:ASPxButton ID="ASPxButton1unselect" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAllGroup" />
                                                                <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookupGroup" UseSubmitBehavior="False" />
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
                                        <ClientSideEvents ValueChanged="Groupwiseledger" />
                                    </dxe:ASPxGridLookup>
                                </dxe:PanelContent>
                            </PanelCollection>

                        </dxe:ASPxCallbackPanel>
                    </div>
            </div>
            <div class="col-md-4 padTop30">
                <div id="ckpar" style="padding-left: 15px;">
                    <%-- Search by party invoice date --%>
                    <asp:CheckBox runat="server" ID="chkparty" style="color: #b5285f" Checked="true" Text="Consider Party Inv. date" />
                </div>
            </div>
            <div class="clear"></div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsFrom">
                    <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
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
            
            <div class="col-md-3 " style="padding-top:20px;">
                    <%--<button id="btnShow" class="btn btn-primary" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>--%>

                    <dxe:ASPxButton ID="btnShow" runat="server" ToolTip="Click on Show the report" AutoPostBack="false" Text="Show" CssClass="btn btn-success" >
                        <ClientSideEvents Click="btn_ShowRecordsClick"></ClientSideEvents>
                    </dxe:ASPxButton>
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
        <table class="pull-left">
            
            <tr>
                <%--<td >
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label3" runat="Server" Text="Customer/Vendor : " CssClass="mylabel1"
                            Width="110px"></asp:Label>
                    </div>
                </td>--%>
                <td style="width: 254px">
                    <%--<asp:ListBox ID="ListBoxCustomerVendor" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>--%>



                   <%-- <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cProductComponentPanel" OnCallback="ComponentProduct_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="lookup_quotation" SelectionMode="Multiple" runat="server" TabIndex="7" ClientInstanceName="gridquotationLookup"
                                    OnDataBinding="lookup_quotation_DataBinding"
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
                                                                <dxe:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll" />
                                                            </div>
                                                            <dxe:ASPxButton ID="ASPxButton5" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll" />                                                            
                                                            <dxe:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" UseSubmitBehavior="False" />
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
                                    <%--  <ClientSideEvents ValueChanged="function(s, e) { InvoiceNumberChanged();}" />--%>
                               <%-- </dxe:ASPxGridLookup>
                            </dxe:PanelContent>
                        </PanelCollection>--%>
                        <%--<ClientSideEvents EndCallback="componentEndCallBack" />--%>
                    <%--</dxe:ASPxCallbackPanel>--%>





                    <asp:HiddenField ID="hdnSelectedCustomerVendor" runat="server" />


                    <span id="MandatoryCustomerType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>

                </td>
                <td style="padding-left: 15px">
                    
                </td>
                <td>
                    
                </td>
                <td style="padding-left: 15px">
                    
                </td>
                <td>
                    
                </td>
                
                <td style="padding-left: 10px; padding-top: 3px">
                    
                </td>
            </tr>



            </table>
        <div class="pull-right">
        </div>
        <table class="TableMain100">


            <tr>

                <td colspan="2">
                    <div onkeypress="OnWaitingGridKeyPress(event)">
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyboardSupport="true" 
                             DataSourceID="GenerateEntityServerModeDataSource" KeyFieldName="SLNO" Settings-HorizontalScrollBarMode="Visible">
                            <%--OnDataBound="Showgrid_Htmlprepared" OnDataBinding="ShowGrid_DataBinding" OnCustomCallback="Grid_CustomCallback"--%>
                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="CustomerVendor" Caption="Customer/Vendor" Width="220px" VisibleIndex="1" >
                                    <Settings ShowFilterRowMenu="True"></Settings>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="UNIQUEID" Caption="Unique ID" Width="150px" VisibleIndex="2" >
                                    <Settings ShowFilterRowMenu="True"></Settings>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="CVGRP_NAME" Caption="Party Group" Width="200px" VisibleIndex="3" >
                                    <Settings ShowFilterRowMenu="True"></Settings>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Opening_Dr" Caption="Opening Dr." VisibleIndex="4" Width="130px" >
                                    <%--PropertiesTextEdit-DisplayFormatString="0.00" />--%>
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Opening_Cr" Caption="Opening Cr." Width="130px" VisibleIndex="5" >
                                    <%--PropertiesTextEdit-DisplayFormatString="0.00" />--%>
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="During_Dr" Caption="Period Dr." Width="130px" VisibleIndex="6" >
                                    <%--PropertiesTextEdit-DisplayFormatString="0.00" />--%>
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="During_Cr" Caption="Period Cr." Width="130px" VisibleIndex="7" >
                                    <%--PropertiesTextEdit-DisplayFormatString="0.00" />--%>
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Closing_Dr" Caption="Closing Dr." Width="130px" VisibleIndex="8" >
                                    <%--PropertiesTextEdit-DisplayFormatString="0.00" />--%>
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Closing_Cr" Caption="Closing Cr." Width="130px" VisibleIndex="9" >
                                    <%--PropertiesTextEdit-DisplayFormatString="0.00" />--%>
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" ColumnResizeMode="Control" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <%--<PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />--%>
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>
                            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="false" />
                            <ClientSideEvents EndCallback="Callback2_EndCallback" />
                        </dxe:ASPxGridView>
                        <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="PARTYOUTSTANDING_REPORT"></dx:LinqServerModeDataSource>
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
     <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
    <PanelCollection>
        <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfIsPartyOutstanding" runat="server" />
        </dxe:PanelContent>
    </PanelCollection>
    <ClientSideEvents EndCallback="CallbackPanelEndCall" />
 </dxe:ASPxCallbackPanel>
    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
     <%--<dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divSubmitButton" 
        Modal="True" >     
    </dxe:ASPxLoadingPanel>--%>
</asp:Content>