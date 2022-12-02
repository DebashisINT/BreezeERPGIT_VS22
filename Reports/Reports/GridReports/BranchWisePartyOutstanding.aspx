<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false" 
    AutoEventWireup="true" CodeBehind="BranchWisePartyOutstanding.aspx.cs" Inherits="Reports.Reports.GridReports.BranchWisePartyOutstanding" %>

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
            bottom: 50px;
            background: #fff;
            box-shadow: 1px 1px 10px #0000001c;
            right: 50%;
        }
        /*rev end Pallab*/
    </style>


    <script type="text/javascript">
        //function EndShowGrid() {
        //    //alert('');
        //    $("#drdExport").val(0);
        //    Grid.Focus();
        //    Grid.SetFocusedRowIndex(0);
        //}
        function EndShowCustdetails() {
            //$("#ddlExport2").val(0);
            //ctxtBranch2ndLevel.SetText(cCustdetails.cpBranchDesc);
            //ctxtcustvend2ndLevel.SetText(cCustdetails.cpCustVendDesc);
            //$("#lblFromDate2ndLevel")[0].innerHTML = "From " + cCustdetails.cpFromDate;
            //$("#lblToDate2ndLevel")[0].innerHTML = " To " + cCustdetails.cpToDate;

            //if (cCustdetails.cpCustVendType == 'ALL') {
            //    $("#Label7")[0].innerHTML = 'Customer/Vendor : '
            //}
            //else if (cCustdetails.cpCustVendType == 'CL') {
            //    $("#Label7")[0].innerHTML = 'Customer : '
            //}
            //else if (cCustdetails.cpCustVendType == 'DV') {
            //    $("#Label7")[0].innerHTML = 'Vendor : '
            //}


            //cCustdetails.Focus();
            //cCustdetails.SetFocusedRowIndex(0);


            $("#ddlExport2").val(0);
           
        }
        $(function () {
            $("#ddlbranchHO").change(function () {
                var Ids = $(this).val();
                cbranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            })
        });

        $(function () {
            //cbranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);
            //cGroupComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);


            cbranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            cGroupComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);

            $("#drp_partytype").change(function () {
                var end = $("#drp_partytype").val();
                $("#ckpar").attr('style', 'display:inline-block; padding-left: 15px; padding-top: 3px;font-weight: bold;" class="clsTo;')

                if (end == 'CL') {

                    //$("#Label3").text('Customer');
                    //$("#ckpar").attr('style', 'display:none; padding-left: 15px; padding-top: 3px;color: #b5285f; font-weight: bold;" class="clsTo;');
                    $("#GrpSelLbl").attr('style', 'display:none;');
                    $("#GrpSel").attr('style', 'display:none;');
                    $("#ghgf").attr('style', 'display:none;');
                    $("#ckpar").attr('style', 'display:none;');
                }
                else if (end == 'DV') {

                    //$("#Label3").text('Vendor');
                    $("#GrpSelLbl").attr('style', 'font-weight:bold; display :block;' );
                    $("#GrpSel").attr('style', ' font-weight: bold;display:block;');
                    $("#ghgf").attr('style', 'display:block;');
                    $("#ckpar").attr('style', 'display:block;');
                }
                else if (end == 'ALL') {

                    //$("#Label3").text('Customer/Vendor');
                    $("#GrpSelLbl").attr('style', 'font-weight: bold;display:block;');
                    $("#GrpSel").attr('style', 'font-weight: bold;display:block;');
                    $("#ghgf").attr('style', 'display:block;');
                    $("#ckpar").attr('style', 'display:block;');
                }
                gridGroupLookup.SetValue(null);
                //BindCustomerVendor(end);
            });

        });


        function OnWaitingGridKeyPress(e) {
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                $("#hfIsBranchWisePartyOutstandingCustDet").val("Y");
                var index = Grid.GetFocusedRowIndex();
                //var CustCode = Grid.GetRowKey(index);
                //cShowGridDetails2Level.PerformCallback(ledger + "~" + ason + "~" + branchid);
                //cpopup2ndLevel.Show();
                //var branchdesc = Grid.GetRow(index).children[0].innerHTML;
                //var custvenddesc = Grid.GetRow(index).children[2].innerHTML;
                //var branchid=Grid.GetRow(index).children[10].innerHTML;
                //var CustVendCode = Grid.GetRow(index).children[11].innerHTML;
                //var CustVendType = Grid.GetRow(index).children[12].innerHTML;
                var branchdesc = Grid.GetRow(index).children[0].innerHTML;
                var custvenddesc = Grid.GetRow(index).children[1].innerHTML;
                var branchid = Grid.GetRow(index).children[9].innerHTML;
                var CustVendCode = Grid.GetRow(index).children[10].innerHTML;
                var CustVendType = Grid.GetRow(index).children[11].innerHTML;


                cCallbackPaneCustDet.PerformCallback(branchdesc + "~" + custvenddesc + "~" + branchid + "~" + CustVendCode + "~" + CustVendType);
                cpopup2ndLevel.Show();
            }

        }




        //for Esc
        document.onkeyup = function (e) {
            if (event.keyCode == 27 && cpopup2ndLevel.GetVisible() == true && popupcustvend.GetVisible() == false) { //run code for alt+N -- ie, Save & New  
                popupHide();
            }
            else if (event.keyCode == 27 && popupcustvend.GetVisible() == true) { //run code for alt+N -- ie, Save & New  
                popupHide2();
            }
        }
        function popupHide(s, e) {
            cpopup2ndLevel.Hide();
            Grid.Focus();
            $("#drdExport").val(0);
        }
        function popupHide2(s, e) {
            popupcustvend.Hide();
            cCustdetails.Focus();
            $("#ddlExport2").val(0);
            $("#drdExport").val(0);
        }

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

        function Callback_EndCallback() {
            $("#drdExport").val(0);
        }
        function CallbackPanelEndCall(s, e) {
             <%--Rev Subhra 20-12-2018   0017670--%>
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
            //End of Subhra
            Grid.Refresh();
        }
        function CallbackPanelCustDetEndCall(s, e) {
            cCustdetails.Refresh();

            ctxtBranch2ndLevel.SetText(cCallbackPaneCustDet.cpBranchDesc);
            ctxtcustvend2ndLevel.SetText(cCallbackPaneCustDet.cpCustVendDesc);
            $("#lblFromDate2ndLevel")[0].innerHTML = "From " + cCallbackPaneCustDet.cpFromDate;
            $("#lblToDate2ndLevel")[0].innerHTML = " To " + cCallbackPaneCustDet.cpToDate;

            if (cCallbackPaneCustDet.cpCustVendType == 'ALL') {
                $("#Label7")[0].innerHTML = 'Customer/Vendor : '
            }
            else if (cCallbackPaneCustDet.cpCustVendType == 'CL') {
                $("#Label7")[0].innerHTML = 'Customer : '
            }
            else if (cCallbackPaneCustDet.cpCustVendType == 'DV') {
                $("#Label7")[0].innerHTML = 'Vendor : '
            }
            cCustdetails.Focus();
            cCustdetails.SetFocusedRowIndex(0);
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

            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
            data += '~' + $('#ddlbranchHO').val();

            //if (gridquotationLookup.GetValue() == null) {
            //    jAlert('Please select atleast one Party.');
            //}
            //else {
            $("#hfIsBranchWisePartyOutstanding").val("Y");
            //cCallbackPanel.PerformCallback(data);
            if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                jAlert('Please select atleast one branch for generate the report.');
            }
            else {
                cCallbackPanel.PerformCallback(data);
            }
            //}
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

        function closePopup(s, e) {
            e.cancel = false;
            Grid.Focus();
            $("#drdExport").val(0);
            $("#ddlExport2").val(0);
        }

        function OpenCUSTDetails(Uniqueid, type) {

            // alert(type);
            var url = '';
            if (type == 'POS') {
                //  window.location.href = '/OMS/Management/Activities/posSalesInvoice.aspx?key=' + invoice + '&Viemode=1';
                //   window.open('/OMS/Management/Activities/posSalesInvoice.aspx?key=' + Uniqueid + '&Viemode=1', '_blank')

                url = '/OMS/Management/Activities/posSalesInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&Viemode=1';
            }
            else if (type == 'SI') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                //   window.open('/OMS/Management/Activities/SalesInvoice.aspx?key=' + Uniqueid + '&req=V&type=' + type, '_blank')

                url = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }


            else if (type == 'PC') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                //  window.open('/OMS/Management/Activities/PurchaseChallan.aspx?key=' + Uniqueid + '&req=V&status=1&type=' + type, '_blank')

                url = '/OMS/Management/Activities/PurchaseChallan.aspx?key=' + Uniqueid + '&req=V&IsTagged=1&type=' + type;
            }

            else if (type == 'SR') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                // window.open('/OMS/Management/Activities/SalesReturn.aspx?key=' + Uniqueid + '&req=V&type=' + type, '_blank')
                url = '/OMS/Management/Activities/SalesReturn.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }

            else if (type == 'SRM') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                //  window.open('/OMS/Management/Activities/ReturnManual.aspx?key=' + Uniqueid + '&req=V&type=' + type, '_blank')
                url = '/OMS/Management/Activities/ReturnManual.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }

            else if (type == 'SRN') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                //  window.open('/OMS/Management/Activities/ReturnNormal.aspx?key=' + Uniqueid + '&req=V&type=' + type, '_blank')
                url = '/OMS/Management/Activities/ReturnNormal.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;

            }


            else if (type == 'PI') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                ///   window.open('/OMS/Management/Activities/PurchaseInvoice.aspx?key=' + Uniqueid + '&req=V&type=PB', '_blank')
                url = '/OMS/Management/Activities/PurchaseInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=PB';

            }


            else if (type == 'VP' || type == 'VR') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                /// window.open('/OMS/Management/Activities/VendorPaymentReceipt.aspx?key=' + Uniqueid + '&req=V&type=VPR', '_blank')
                url = '/OMS/Management/Activities/VendorPaymentReceipt.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=VPR';

            }

            else if (type == 'PR') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                ///    window.open('/OMS/Management/Activities/BranchRequisitionReturn.aspx?key=' + Uniqueid + '&req=V', '_blank')
                url = '/OMS/Management/Activities/PReturn.aspx.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=PR';

            }


            else if (type == 'SC') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                /// window.open('/OMS/Management/Activities/CustomerReturn.aspx?key=' + Uniqueid + '&req=V&type=' + type, '_blank')

                url = '/OMS/Management/Activities/CustomerReturn.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }


            else if (type == 'CP' || type == 'CR') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                ///    window.open('/OMS/Management/Activities/CustomerReceiptPayment.aspx?key=' + Uniqueid + '&req=V&type=CRP', '_blank')
                url = '/OMS/Management/Activities/CustomerReceiptPayment.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=CRP';
            }



            else if (type == 'CDN' || type == 'CCN') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                ///    window.open('/OMS/Management/Activities/CustomerReceiptPayment.aspx?key=' + Uniqueid + '&req=V&type=CRP', '_blank')
                url = '/OMS/Management/Activities/CustomerNote.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=CDCN';
            }





            else if (type == 'VCN' || type == 'VDN') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                ///    window.open('/OMS/Management/Activities/CustomerReceiptPayment.aspx?key=' + Uniqueid + '&req=V&type=CRP', '_blank')
                //url = '/OMS/Management/Activities/VendorDebitCreditNote.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=CDCN';
                url = '/OMS/Management/Activities/VendorDebitCreditNote.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;;
            }


            else if (type == 'TPB') {

                url = '/OMS/Management/Activities/TPurchaseInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }


            else if (type == 'TSI') {

                url = '/OMS/Management/Activities/TSalesInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }
            else if (type == 'IPB') {
                url = '/Import/PurchaseInvoice-Import.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }

            popupcustvend.SetContentUrl(url);
            popupcustvend.Show();
        }

        function BudgetAfterHide(s, e) {
            popupbudget.Hide();
        }

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

     


        //function selectAll() {
        //    gridquotationLookup.gridView.SelectRows();
        //}
        //function unselectAll() {
        //    gridquotationLookup.gridView.UnselectRows();
        //}
        function selectAllGroup() {  //Suvankar
            gridGroupLookup.gridView.SelectRows();
        }
        function unselectAllGroup() {  //Suvankar
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
<%--        <div class="panel-title">
            <h3>Branch Wise Party Outstanding</h3>
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
                 <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label5" runat="Server" Text="Head Branch : " CssClass="mylabel1"
                            Width="92px" Font-Bold="True"></asp:Label>
                    </label>
                </div>
                <asp:DropDownList ID="ddlbranchHO" runat="server" Width="100%"></asp:DropDownList>
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label1" runat="Server" Text="Unit : " CssClass="mylabel1"
                        Width="92px" Font-Bold="True"></asp:Label>
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
                                        <dxe:GridViewDataColumn FieldName="branch_code" Visible="true"  VisibleIndex="1" width="200px" Caption="Branch code" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>

                                        <dxe:GridViewDataColumn FieldName="branch_description" Visible="true" VisibleIndex="2" width="200px"  Caption="Branch Name" Settings-AutoFilterCondition="Contains">
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
                            Width="92px" Font-Bold="True"></asp:Label>
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
                        Width="92px" Font-Bold="True"></asp:Label>
                </label>
                <asp:DropDownList ID="drp_balancetype" runat="server" Width="100%">
                    <asp:ListItem Text="All" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Only Balance" Value="1"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-2" id="ghgf">
                <label id="GrpSelLbl" style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label4" runat="Server" Text="Group : " CssClass="mylabel1"
                        Width="92px" Font-Bold="True"></asp:Label>
                </label>
                <div id="GrpSel">
                        <asp:ListBox ID="ListBoxGroup" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>
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
            <div class="col-md-2 padTop30">
                <div id="ckpar" >
                    <asp:CheckBox runat="server" ID="chkparty" style="color: #b5285f" Checked="true" Font-Bold="True" Text="Consider Party Inv. date" />
                </div>
            </div>
            <div class="clear"></div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsFrom">
                    <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"
                        Width="92px" Font-Bold="True"></asp:Label>
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
                        Width="92px" Font-Bold="True"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>

                    </dxe:ASPxDateEdit>
            </div>
            
            <div class="col-md-3 " style="padding-top:20px;">
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
        <table class="pull-left">
            
            <tr>
                <%--<td >
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label3" runat="Server" Text="Customer/Vendor : " CssClass="mylabel1"
                            Width="110px"></asp:Label>
                    </div>
                </td>--%>
                <td style="width: 254px">
                    <asp:ListBox ID="ListBoxCustomerVendor" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>



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
                         DataSourceID="GenerateEntityServerModeDataSource" OnSummaryDisplayText="ShowGrid_SummaryDisplayText" OnDataBound="Showgrid_Htmlprepared"  KeyFieldName="SLNO" 
                            ClientSideEvents-BeginCallback="Callback_EndCallback" Settings-HorizontalScrollBarMode="Visible">
                            <Columns>
                                <%-- <dxe:GridViewDataTextColumn FieldName="branch_description" Caption="Branch" Width="30%" VisibleIndex="1" >
                                    <Settings ShowFilterRowMenu="True"></Settings>
                                </dxe:GridViewDataTextColumn>--%>
                                <dxe:GridViewDataTextColumn FieldName="For_BranchDescription" Caption="Unit" Width="30%" VisibleIndex="1" >
                                    <Settings ShowFilterRowMenu="True"></Settings>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CustomerVendor" Caption="Customer/Vendor" Width="30%" VisibleIndex="2" >
                                    <Settings ShowFilterRowMenu="True"></Settings>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="UNIQUEID" Caption="Unique ID" Width="15%" VisibleIndex="3" >
                                    <Settings ShowFilterRowMenu="True"></Settings>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Opening_Dr" Caption="Opening Dr." VisibleIndex="4" Width="15%" FooterCellStyle-HorizontalAlign="Right" >
                                     <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Opening_Cr" Caption="Opening Cr." Width="15%" VisibleIndex="5"  FooterCellStyle-HorizontalAlign="Right" >
                                      <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="During_Dr" Caption="Period Dr." Width="15%" VisibleIndex="6"  FooterCellStyle-HorizontalAlign="Right" >
                                      <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="During_Cr" Caption="Period Cr." Width="15%" VisibleIndex="7" FooterCellStyle-HorizontalAlign="Right" >
                                      <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Closing_Dr" Caption="Closing Dr." Width="15%" VisibleIndex="8"  FooterCellStyle-HorizontalAlign="Right" >
                                     <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Closing_Cr" Caption="Closing Cr." Width="15%" VisibleIndex="9" FooterCellStyle-HorizontalAlign="Right" >
                                      <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="branch_id" Caption="branch_id" Width="0px" VisibleIndex="10" />
                                <dxe:GridViewDataTextColumn FieldName="CODE" Caption="CODE" Width="0px" VisibleIndex="11" />
                                <dxe:GridViewDataTextColumn FieldName="CustVend" Caption="CustVend" Width="0px" VisibleIndex="12" />

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
                            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="false" />
                             <TotalSummary>
                                 
                                <dxe:ASPxSummaryItem FieldName="Opening_Dr" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Opening_Cr" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="During_Dr" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="During_Cr" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Closing_Dr" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Closing_Cr" SummaryType="Sum" />

                            </TotalSummary>
                            <%--<ClientSideEvents EndCallback="EndShowGrid" />--%>
                        </dxe:ASPxGridView>
                        <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="BRANCHWISEPARTYOUTSTANDINGSUM_REPORT" ></dx:LinqServerModeDataSource>
                    </div>
                </td>
            </tr>
        </table>
    </div>

      <dxe:ASPxPopupControl ID="popup" runat="server" ClientInstanceName="cpopup2ndLevel"
        Width="1100px" Height="600px" ScrollBars="Vertical" HeaderText="Branch Wise Party Details Report" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <clientsideevents closing="function(s, e) {
	        closePopup(s, e);}" />
        <contentstyle verticalalign="Top" cssclass="pad">
        </contentstyle>
        <contentcollection>
             <dxe:PopupControlContentControl runat="server">
                   <input id="hfProductID2" type="hidden" />
                    <input id="hfBranchID3" type="hidden" />
                 <div class="col-md-12">
                        <div class="row clearfix">
                            <table class="pdbot" style="margin: 4px 0 16px 10px; float: left;">
                            <tr>
                                <td style="padding-top: 10px">
                                    <label style="color: #b5285f; font-weight: bold; margin-top: 5px;" class="clsTo">
                                        <asp:Label ID="Label6" runat="Server" Text="Branch : " CssClass="mylabel1"></asp:Label>
                                    </label>
                                </td>
                                <td style="padding-top: 10px">
                                    <dxe:ASPxTextBox ID="txtBranch2ndLevel" ClientInstanceName="ctxtBranch2ndLevel" runat="server" ReadOnly="true" Width="600px"></dxe:ASPxTextBox>
                                </td>

                            </tr>
                            <tr>
                                  <td style="padding-top: 10px">
                                    <label style="color: #b5285f; font-weight: bold; margin-top: 5px;" class="clsTo">
                                        <asp:Label ID="Label7" runat="Server"  CssClass="mylabel1"></asp:Label>
                                    </label>
                                </td>
                                <td style="padding-top: 10px">
                                    <dxe:ASPxTextBox ID="txtcustvend2ndLevel" ClientInstanceName="ctxtcustvend2ndLevel" runat="server" ReadOnly="true" Width="600px"></dxe:ASPxTextBox>
                                </td>
                            </tr>

                            <tr>
                                <td></td>
                                <td>
                                    <label style="color: #b5285f; font-weight: bold; margin-top: 5px;" class="clsTo">
                                        <asp:Label ID="lblFromDate2ndLevel" runat="Server" Font-Bold="true" CssClass="mylabel1"></asp:Label>
                                    </label>
                                    <label style="color: #b5285f; font-weight: bold; margin-top: 5px;" class="clsTo">
                                        <asp:Label ID="lblToDate2ndLevel" runat="Server" Font-Bold="true"  CssClass="mylabel1"></asp:Label>
                                    </label>
                                    <span style="padding-left: 10px;color: #b5285f; display: inline-block"><strong>Press < Esc > Key to Close</strong></span></td>
                                </tr>
                        </table>
                            <div class="pull-right" style="padding-top: 26px;">
                                
                                <asp:DropDownList ID="ddlExport2" runat="server" CssClass="btn btn-sm btn-primary" AutoPostBack="true" OnSelectedIndexChanged="ddlExport2_SelectedIndexChanged">
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                    <asp:ListItem Value="2">XLSX</asp:ListItem>
                                    <asp:ListItem Value="3">RTF</asp:ListItem>
                                    <asp:ListItem Value="4">CSV</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                      <div onkeypress="OnWaitingGridKeyPress2(event)">
                        <dxe:ASPxGridView runat="server" ID="Custdetails" ClientInstanceName="cCustdetails" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                            DataSourceID="GenerateEntityServerModeCustDetailsDataSource" OnCustomSummaryCalculate="Custdetails_CustomSummaryCalculate"  KeyFieldName="SLNO"
                            OnSummaryDisplayText="Custdetails_SummaryDisplayText" OnDataBound="Custdetails_Htmlprepared" ClientSideEvents-BeginCallback="EndShowCustdetails"
                            Settings-HorizontalScrollBarMode="Visible">
                            <Columns>

                                <dxe:GridViewDataTextColumn FieldName="BRANCH_DESC" Caption="Unit" Width="20%" VisibleIndex="1" />
                                <dxe:GridViewDataTextColumn FieldName="CustomerVendor" Caption="Customer/Vendor" Width="20%" VisibleIndex="3" />
                                <dxe:GridViewDataTextColumn FieldName="TRAN_DATE" Caption="Transaction Date" Width="25%" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" />

                                <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="DOCUMENT_NO" Caption="Doument No." Width="20%">
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>

                                        <a href="javascript:void(0)" onclick="OpenCUSTDetails('<%#Eval("DOC_ID") %>','<%#Eval("MODULE_TYPE") %>')" class="pad">
                                            <%#Eval("DOCUMENT_NO")%>
                                        </a>
                                    </DataItemTemplate>

                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="TRAN_TYPE" Caption="Transaction Type" Width="25%" VisibleIndex="6" />

                                <dxe:GridViewDataTextColumn FieldName="party_date" Caption="Invoice Date" Width="20%" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" />

                                <dxe:GridViewDataTextColumn FieldName="party_inv" Caption="Invoice No." Width="20%" VisibleIndex="8" />

                                <dxe:GridViewDataTextColumn FieldName="Particulars" Caption="Particulars" Width="25%" VisibleIndex="9" />

                                <dxe:GridViewDataTextColumn FieldName="DEBIT" Caption="Debit" VisibleIndex="10" Width="15%" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="CREDIT" Caption="Credit" Width="15%" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="0.00" />

                                <dxe:GridViewDataTextColumn FieldName="Closing_Balance" Caption="Closing Balance" Width="25%" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="0.00" />

                                <dxe:GridViewDataTextColumn FieldName="Closebal_DBCR" Caption="Dbcrtype" Width="16%" VisibleIndex="13" />
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
                            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                          
                            
                              <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="DEBIT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="CREDIT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Closing_Balance" SummaryType="Custom" />

                            </TotalSummary>
                           <%--   <ClientSideEvents EndCallback="EndShowCustdetails" />--%>
                        </dxe:ASPxGridView>
                            <dx:LinqServerModeDataSource ID="GenerateEntityServerModeCustDetailsDataSource" runat="server" OnSelecting="GenerateEntityServerModeCustDetailsDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="BRANCHWISEPARTYOUTSTANDINGDET_REPORT" ></dx:LinqServerModeDataSource>
                    </div>
                    </div>
             </dxe:PopupControlContentControl>
        </contentcollection>
    </dxe:ASPxPopupControl>

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

      <dxe:ASPxPopupControl ID="ASPXPopupControl1" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupcustvend" Height="500px"
        Width="1200px" HeaderText="Details" Modal="true" AllowResize="true">
        <contentcollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </contentcollection>

        <%--   <ClientSideEvents CloseUp="DocumentAfterHide" />--%>
    </dxe:ASPxPopupControl>

     <dxe:ASPxGridViewExporter ID="exporterDetails" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>


    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            <asp:HiddenField ID="hfIsBranchWisePartyOutstanding" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPaneCustDet" ClientInstanceName="cCallbackPaneCustDet" OnCallback="CallbackPaneCustDet_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            <asp:HiddenField ID="hfIsBranchWisePartyOutstandingCustDet" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelCustDetEndCall" />
    </dxe:ASPxCallbackPanel>

    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
</asp:Content>