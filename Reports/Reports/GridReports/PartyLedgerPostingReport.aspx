<%--================================================== Revision History ============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                17-02-2023        V2.0.36           Pallab              25575 : Report pages design modification
====================================================== Revision History ================================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="PartyLedgerPostingReport.aspx.cs"
    Inherits="Reports.Reports.GridReports.PartyLedgerPostingReport" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchMultiPopup.js"></script>
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

         .btnOkformultiselection {
            border-width: 1px;
            padding: 4px 10px;
            font-size: 13px !important;
            margin-right: 6px;
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

    <%-- For multiselection when click on ok button--%>
    <script type="text/javascript">
        function SetSelectedValues(Id, Name, ArrName) {
            if (ArrName == 'GroupSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#GroupModel').modal('hide');
                    ctxtGroup.SetText(Name);
                    GetObjectID('hdnSelectedGroups').value = key;

                    ctxtCustVendName.SetText('');
                    $('#txtCustVendSearch').val('')

                    //when vendor tagged with group and want to de select and clear text previously selected vendor.
                    var OtherDetailsvend = {}
                    OtherDetailsvend.SearchKey = "NoVendorWithAnyGroup";
                    OtherDetailsvend.GroupIDs = "";
                    OtherDetailsvend.Type = "0";

                    var HeaderCaptionVend = [];
                    HeaderCaptionVend.push("Name");
                    HeaderCaptionVend.push("Unique Id");
                    callonServerM("Services/Master.asmx/GetGroupWiseVendor", OtherDetailsvend, "CustomerVendorTable", HeaderCaptionVend, "dPropertyIndex", "SetSelectedValues", "CustomerVendorSource");
                }
                else {
                    ctxtGroup.SetText('');
                    GetObjectID('hdnSelectedGroups').value = '';
                }
            }
            else if (ArrName == 'CustomerVendorSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#CustVendModel').modal('hide');
                    ctxtCustVendName.SetText(Name);
                    GetObjectID('hdnSelectedCustomerVendor').value = key;
                }
                else {
                    ctxtCustVendName.SetText('');
                    GetObjectID('hdnSelectedCustomerVendor').value = '';
                }
            }

        }

    </script>
    <%-- For multiselection when click on ok button--%>

    <%-- For Group multiselection--%>
    <script type="text/javascript">

        $(document).ready(function () {
            $('#GroupModel').on('shown.bs.modal', function () {
                $('#txtGroupSearch').focus();
            })
        })

        var GroupArr = new Array();
        $(document).ready(function () {
            var GroupObj = new Object();
            GroupObj.Name = "GroupSource";
            GroupObj.ArraySource = GroupArr;
            arrMultiPopup.push(GroupObj);
        })
        function GroupButnClick(s, e) {
            $('#GroupModel').modal('show');
        }

        function Group_KeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#GroupModel').modal('show');
            }
        }

        function Groupkeydown(e) {
            var OtherDetails = {}

            if ($.trim($("#txtGroupSearch").val()) == "" || $.trim($("#txtGroupSearch").val()) == null) {
                return false;
            }
            OtherDetails.SearchKey = $("#txtGroupSearch").val();

            if (e.code == "Enter" || e.code == "NumpadEnter") {

                var HeaderCaption = [];
                HeaderCaption.push("Group Description");

                if ($("#txtGroupSearch").val() != "") {
                    callonServerM("Services/Master.asmx/GetGroup", OtherDetails, "GroupTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "GroupSource");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[dPropertyIndex=0]"))
                    $("input[dPropertyIndex=0]").focus();
            }
        }

        function SetfocusOnseach(indexName) {
            if (indexName == "dPropertyIndex")
                $('#txtGroupSearch').focus();
            else
                $('#txtGroupSearch').focus();
        }
    </script>
    <%-- For Group multiselection--%>

      <%-- For Customer/Vendor multiselection--%>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#CustVendModel').on('shown.bs.modal', function () {
                $('#txtCustVendSearch').focus();
            })
        })

        var CustVendArr = new Array();
        $(document).ready(function () {
            var CustVendObj = new Object();
            CustVendObj.Name = "CustomerVendorSource";
            CustVendObj.ArraySource = CustVendArr;
            arrMultiPopup.push(CustVendObj);
        })
        function CustomerVendorButnClick(s, e) {
            $('#CustVendModel').modal('show');
        }

        function CustomerVendor_KeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#CustVendModel').modal('show');
            }
        }

        function CustomerVendorkeydown(e) {
            var OtherDetails = {}

            if ($.trim($("#txtCustVendSearch").val()) == "" || $.trim($("#txtCustVendSearch").val()) == null) {
                return false;
            }
            OtherDetails.SearchKey = $("#txtCustVendSearch").val();
            OtherDetails.GroupIDs = hdnSelectedGroups.value;
            

            if (e.code == "Enter" || e.code == "NumpadEnter") {

                var HeaderCaption = [];
                HeaderCaption.push("Name");
                HeaderCaption.push("Unique Id");
                HeaderCaption.push("Party Type");

                if (drp_partytype.value == 0)
                {
                    OtherDetails.Type = "0";
                    callonServerM("Services/Master.asmx/GetGroupWiseVendor", OtherDetails, "CustomerVendorTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "CustomerVendorSource");
                }
                else if (drp_partytype.value == 1)
                {
                    OtherDetails.Type = "3";
                    callonServerM("Services/Master.asmx/GetGroupWiseVendor", OtherDetails, "CustomerVendorTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "CustomerVendorSource");
                }
                else if (drp_partytype.value == 2) {
                    OtherDetails.Type = "4";
                    callonServerM("Services/Master.asmx/GetGroupWiseVendor", OtherDetails, "CustomerVendorTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "CustomerVendorSource");
                }
                    //Rev Debashis 0025193
                else if (drp_partytype.value == 3) {
                    OtherDetails.Type = "6";
                    callonServerM("Services/Master.asmx/GetGroupWiseVendor", OtherDetails, "CustomerVendorTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "CustomerVendorSource");
                }
                //End of Rev Debashis 0025193
            }
            else if (e.code == "ArrowDown") {
                if ($("input[dPropertyIndex=0]"))
                    $("input[dPropertyIndex=0]").focus();
            }

        }

        function SetfocusOnseach(indexName) {
            if (indexName == "dPropertyIndex")
                $('#txtCustVendSearch').focus();
            else
                $('#txtCustVendSearch').focus();
        }
    </script>
    <%-- For Customer/Vendor multiselection--%>


    <script type="text/javascript">

        function ClearGridLookup() {
            var grid = gridquotationLookup.GetGridView();
            grid.UnselectRows();
        }
        function CallbackPanelEndCall(s, e) {
            <%--Rev Subhra 20-12-2018   0017670--%>
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
            //End of Subhra
            Grid.Refresh();
        }
        $(function () {

            // BindBranches(null);
            //    BindCustomerVendor(0);


            //cProductbranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);
            // cGroupComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);
            //cProductComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);

            cBranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);
            //cGroupComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);
            //cCustomerVendorComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);

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
                $("#ckpar").attr('style', 'display:inline-block; padding-left: 0px; padding-top: 3px;color: #b5285f; font-weight: bold;" class="clsTo;');


                if (end == '1') {

                    $("#Label3").text('Customer:');
                    $("#ckpar").attr('style', 'display:none; padding-left: 0px; padding-top: 3px;color: #b5285f; font-weight: bold;" class="clsTo;');
                    $("#GrpSelLbl").attr('style', 'display:none;');
                    $("#GrpSel").attr('style', 'display:none;');
                    //gridquotationLookup.gridView.UnselectRows();
                    //gridGroupLookup.gridView.UnselectRows();
                    $('#<%=hdfVendor.ClientID %>').val('2');
                    //$("#gridGroupLookup").text('');
                }
                else if (end == '2') {

                    $("#Label3").text('Vendor:');
                    $("#GrpSelLbl").attr('style', 'color: #b5285f; font-weight: bold;" class="clsTo;display:block;');
                    $("#GrpSel").attr('style', 'color: #b5285f; font-weight: bold;" class="clsTo;display:block;');
                    //$("#gridGroupLookup").text('');
                    //gridquotationLookup.gridView.UnselectRows();
                    $('#<%=hdfVendor.ClientID %>').val('1');
                    
                }
                else if (end == '0') {
                    //Rev Debashis 0025193
                    //$("#Label3").text('Customer/Vendor');
                    $("#Label3").text('Customer/Vendor/Transporter:');
                    //End of Rev Debashis 0025193
                    $("#GrpSelLbl").attr('style', 'color: #b5285f; font-weight: bold;" class="clsTo;display:block;');
                    $("#GrpSel").attr('style', 'color: #b5285f; font-weight: bold;" class="clsTo;display:block;');
                    //$("#gridGroupLookup").text('');
                }
                //Rev Debashis 0025193
                if (end == '3') {
                    $("#Label3").text('Transporter:');
                    $("#GrpSelLbl").attr('style', 'display:none;');
                    $("#GrpSel").attr('style', 'display:none;');
                    $('#<%=hdfVendor.ClientID %>').val('2');
                }
                //End of Rev Debashis 0025193
                //gridGroupLookup.ClearGridLookup;
                
                //gridGroupLookup.SetValue(null);
                //BindCustomerVendor(end);
                //gridquotationLookup.gridView.Refresh();
            });

            function OnWaitingGridKeyPress(e) {

                if (e.code == "Enter") {

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
                // BindLedgerType(Ids);                    
                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');

            })

            $("#ListBoxGroup").chosen().change(function () {   //Suvankar
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


        //function BindCustomerVendor(type) {

        //    //cProductComponentPanel.PerformCallback('BindComponentGrid' + '~' + type);
        //    cCustomerVendorComponentPanel.PerformCallback('BindComponentGrid' + '~' + type);
        //}


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
            $("#drdExport").val(0);
            $("#hfIsCustFilter").val("Y");
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();          

            if (ctxtCustVendName.GetValue() == null) {
                jAlert('Please select atleast one Party.');
            }
            else {

                //Grid.PerformCallback(data);
                //cCallbackPanel.PerformCallback();
                if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                    jAlert('Please select atleast one branch for generate the report.');
                }
                else {
                    cCallbackPanel.PerformCallback();
                }
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
        <%--function CloseGridQuotationLookupGroupLostFocus()
        {
            var type = $("#drp_partytype").val();
            if(type == 1)
            {
                $('#<%=hdfVendor.ClientID %>').val('2');
                gridquotationLookup.gridView.Refresh();
            }
            else if (type == 2) {
                $('#<%=hdfVendor.ClientID %>').val('1');
                gridquotationLookup.gridView.Refresh();
            }
          
            
        }--%>

        <%--   function Groupwiseledger() {
            //var type = $("#drp_partytype").val();
            var key = gridGroupLookup.GetGridView().GetRowKey(gridGroupLookup.GetGridView().GetFocusedRowIndex());
            //cProductComponentPanel.PerformCallback('BindComponentGrid' + '~' + 'GrpWs');
            cCustomerVendorComponentPanel.PerformCallback('BindComponentGrid' + '~' + 'GrpWs');
         //if (type == 1) {
                //gridquotationLookup.gridView.UnselectRows();
                //$('#<%=hdfVendor.ClientID %>').val('2');
               // gridquotationLookup.gridView.Refresh();
          //  }
            //else if (type == 2) {
                //gridquotationLookup.gridView.UnselectRows();
               // $('#<%=hdfVendor.ClientID %>').val('1');
                 // gridquotationLookup.gridView.Refresh();
            //}
        }--%>

        //function custvendEndCallBack(s, e)
        //{
            <%--if ($('#<%=hdfVendor.ClientID %>').val() == '1')
            { $('#<%=hdfVendor.ClientID %>').val('0'); }
            ridquotationLookup.gridView.Refresh();--%>
            //alert("hi");
         
        //}

       
        function OpenPOSDetails(Uniqueid, type, docno) {
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

            else if (type == 'CBV') {
                url = '/OMS/Management/dailytask/CashBankEntry.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }
            else if (type == 'JV') {
                url = '/OMS/Management/dailytask/JournalEntry.aspx?key=' + Uniqueid + '&IsTagged=1&req=' + docno;
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


            popupbudget.SetContentUrl(url);
            popupbudget.Show();
        }
        function BudgetAfterHide(s, e) {
            popupbudget.Hide();
        }
       <%-- function CloseGridQuotationLostFocus() {

            if ($('#<%=hdfVendor.ClientID %>').val() == '1')
            { $('#<%=hdfVendor.ClientID %>').val('0'); }

            if ($('#<%=hdfVendor.ClientID %>').val() == '2')
            { $('#<%=hdfVendor.ClientID %>').val('0'); }
        }--%>
        //function CloseGridQuotationLookup() {
        //    gridquotationLookup.ConfirmCurrentSelection();
        //    gridquotationLookup.HideDropDown();
        //    gridquotationLookup.Focus();
        //}
        //function CloseGridQuotationLookupGroup() {  //Suvankar
        //    gridGroupLookup.ConfirmCurrentSelection();
        //    gridGroupLookup.HideDropDown();
        //    gridGroupLookup.Focus();
        //}
        function CloseGridLookupbranch() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }
        //function componentEndCallBack() {
           
        //}
        function Callback2_EndCallback() {
            // alert('');
            $("#drdExport").val(0);
        }


        function selectAll() {
            gridquotationLookup.gridView.SelectRows();
        }
        function unselectAll() {
            gridquotationLookup.gridView.UnselectRows();
        }
        //function selectAllGroup() {  //Suvankar
        //    gridGroupLookup.gridView.SelectRows();
        //}
        //function unselectAllGroup() {  //Suvankar
        //    gridGroupLookup.gridView.UnselectRows();
        //}
        function selectAll_branch() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAll_branch() {
            gridbranchLookup.gridView.UnselectRows();
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

        /*Rev 1.0*/
        .outer-div-main {
            background: #ffffff;
            padding: 10px;
            border-radius: 10px;
            box-shadow: 1px 1px 10px #11111154;
        }

        /*.form_main {
            overflow: hidden;
        }*/

        label , .mylabel1, .clsTo, .dxeBase_PlasticBlue
        {
            color: #141414 !important;
            font-size: 14px !important;
                font-weight: 500 !important;
                margin-bottom: 0 !important;
        }

        #GrpSelLbl .dxeBase_PlasticBlue
        {
                line-height: 20px !important;
        }

        select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue
        {
            height: 30px;
            border-radius: 4px;
        }

        .dxeButtonEditButton_PlasticBlue
        {
            background: #094e8c !important;
            border-radius: 4px !important;
            padding: 0 4px !important;
        }

        .calendar-icon {
            position: absolute;
            bottom: 6px;
            right: 20px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img
        {
            display: none;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        .simple-select::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 26px;
            right: 13px;
            font-size: 16px;
            transform: rotate(269deg);
            font-weight: 500;
            background: #094e8c;
            color: #fff;
            height: 18px;
            display: block;
            width: 26px;
            /* padding: 10px 0; */
            border-radius: 4px;
            text-align: center;
            line-height: 19px;
            z-index: 0;
        }
        .simple-select {
            position: relative;
        }
        .simple-select:disabled::after
        {
            background: #1111113b;
        }
        select.btn
        {
            padding-right: 10px !important;
        }

        .panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }

        .dxpLite_PlasticBlue .dxp-current
        {
            background-color: #1b5ea4;
            padding: 3px 5px;
            border-radius: 2px;
        }

        #accordion {
            margin-bottom: 20px;
            margin-top: 10px;
        }

        .dxgvHeader_PlasticBlue {
    background: #1b5ea4 !important;
    color: #fff !important;
}
        #ShowGrid
        {
            margin-top: 10px;
        }

        .pt-25{
                padding-top: 25px !important;
        }

        .styled-checkbox {
        position: absolute;
        opacity: 0;
        z-index: 1;
    }

        .styled-checkbox + label {
            position: relative;
            /*cursor: pointer;*/
            padding: 0;
            margin-bottom: 0 !important;
        }

            .styled-checkbox + label:before {
                content: "";
                margin-right: 6px;
                display: inline-block;
                vertical-align: text-top;
                width: 16px;
                height: 16px;
                /*background: #d7d7d7;*/
                margin-top: 2px;
                border-radius: 2px;
                border: 1px solid #c5c5c5;
            }

        .styled-checkbox:hover + label:before {
            background: #094e8c;
        }


        .styled-checkbox:checked + label:before {
            background: #094e8c;
        }

        .styled-checkbox:disabled + label {
            color: #b8b8b8;
            cursor: auto;
        }

            .styled-checkbox:disabled + label:before {
                box-shadow: none;
                background: #ddd;
            }

        .styled-checkbox:checked + label:after {
            content: "";
            position: absolute;
            left: 3px;
            top: 9px;
            background: white;
            width: 2px;
            height: 2px;
            box-shadow: 2px 0 0 white, 4px 0 0 white, 4px -2px 0 white, 4px -4px 0 white, 4px -6px 0 white, 4px -8px 0 white;
            transform: rotate(45deg);
        }

        .dxgvEditFormDisplayRow_PlasticBlue td.dxgv, .dxgvDataRow_PlasticBlue td.dxgv, .dxgvDataRowAlt_PlasticBlue td.dxgv, .dxgvSelectedRow_PlasticBlue td.dxgv, .dxgvFocusedRow_PlasticBlue td.dxgv
        {
            padding: 6px 6px 6px !important;
        }

        #lookupCardBank_DDD_PW-1
        {
                left: -182px !important;
        }
        .plhead a>i
        {
                top: 9px;
        }

        .clsTo
        {
            display: flex;
    align-items: flex-start;
        }

        input[type="radio"], input[type="checkbox"]
        {
            margin-right: 5px;
        }
        .dxeCalendarDay_PlasticBlue
        {
                padding: 6px 6px;
        }

        .modal-dialog
        {
            width: 50%;
        }

        .modal-header
        {
            padding: 4px 4px 4px 10px;
            background: #094e8c !important;
        }

        .TableMain100 #ShowGrid
        {
            max-width: 98% !important;
        }

        .btn-info
        {
                background-color: #1da8d1 !important;
                background-image: none;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled
        {
            background: #f7f7f7;
        }

        #ddlValTech
        {
            width: 100% !important;
            margin-bottom: 0 !important;
        }

        .dis-flex
        {
            display: flex;
            align-items: baseline;
        }

        input + label
        {
            line-height: 1;
        }

        .dxtlHeader_PlasticBlue
        {
            background: #094e8c !important;
        }

        #ckpar , #ckHNarration
        {
            padding-top: 9px;
        }

        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/
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
            <h3>Consolidated Party Ledger</h3>
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

    <%--Rev 1.0: "outer-div-main" class add: --%>
    <div class="outer-div-main">
        <div class="form_main">
        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />
        <div class="row">
            <div class="col-md-2 branch-selection-box">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </div>
                <div>

                    <%--<asp:ListBox ID="ListBoxBranches" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>--%>
                    <asp:HiddenField ID="hdnActivityType" runat="server" />
                    <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                    <span id="MandatoryActivityType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                    <asp:HiddenField ID="hdnSelectedBranches" runat="server" />


                    <dxe:ASPxCallbackPanel runat="server" ID="ComponentBranchPanel" ClientInstanceName="cBranchComponentPanel" OnCallback="Componentbranch_Callback">
                    <%--<dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cProductbranchComponentPanel">--%>
                        <panelcollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="lookup_branch" SelectionMode="Multiple" runat="server" ClientInstanceName="gridbranchLookup"
                                    OnDataBinding="lookup_branch_DataBinding"
                                    KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                     <%--DataSourceID="BranchEntityServerModeDataSource" --%>
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                        <dxe:GridViewDataColumn FieldName="branch_code" Visible="true" VisibleIndex="1" width="200px" Caption="Branch code" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>

                                        <dxe:GridViewDataColumn FieldName="branch_description" SortOrder="Ascending" Visible="true" VisibleIndex="2" width="200px" Caption="Branch Name" Settings-AutoFilterCondition="Contains">
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
                                                                <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_branch" UseSubmitBehavior="False" />
                                                            </div>
                                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_branch" UseSubmitBehavior="False" />
                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookupbranch" UseSubmitBehavior="False" />
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
                        </panelcollection>

                    </dxe:ASPxCallbackPanel>
                    <%--<dx:LinqServerModeDataSource ID="BranchEntityServerModeDataSource" runat="server" OnSelecting="BranchEntityServerModeDataSource_Selecting"
                        ContextTypeName="ERPDataClassesDataContext" TableName="v_BranchList" />--%>
                </div>
            </div>
            <%--Rev 1.0--%>
            <%--<div class="col-md-2">--%>
            <div class="col-md-2 simple-select">
                <%--Rev end 1.0--%>
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label2" runat="Server" Text="Party Type : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </div>
                <div>

                    <asp:DropDownList ID="drp_partytype" runat="server" Width="100%">

                        <asp:ListItem Text="All" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Customer" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Vendor" Value="2"></asp:ListItem>
                        <%--Rev Debashis 0025193--%>
                        <asp:ListItem Text="Transporter" Value="3"></asp:ListItem>
                        <%--End of Rev Debashis 0025193--%>
                    </asp:DropDownList>

                </div>
            </div>
            <div class="col-md-2">
                <div id="GrpSelLbl" style="color: #b5285f; font-weight: bold; font-size: 14px" class="clsTo">
                     <dxe:ASPxLabel ID="ASPxLabel1" style="color: #b5285f; font-weight: bold;" runat="server" Text="Group:">
                </dxe:ASPxLabel>
                </div>
                <div>
                    <div id="GrpSel">
                       
                      <%--  <asp:HiddenField ID="HiddenField2" runat="server" />
                        <span id="MandatoryActivityType" style="display: none" class="validclass">
                            <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                        <asp:HiddenField ID="HiddenField3" runat="server" />

                        <dxe:ASPxCallbackPanel runat="server" ID="GroupComponentPanel" ClientInstanceName="cGroupComponentPanel" OnCallback="ComponentGroup_Callback">
                            <panelcollection>
                                <dxe:PanelContent runat="server">                                     
                                    <dxe:ASPxGridLookup ID="lookup_Group" SelectionMode="Multiple" runat="server" ClientInstanceName="gridGroupLookup"
                                        OnDataBinding="lookup_Group_DataBinding"
                                        KeyFieldName="GroupCode" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                        <Columns>
                                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                            <dxe:GridViewDataColumn FieldName="GroupCode" Visible="true" Width="0" VisibleIndex="1" Caption="Group Code" Settings-AutoFilterCondition="Contains">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>

                                            <dxe:GridViewDataColumn FieldName="GroupDescription" SortOrder="Ascending" Visible="true" Width="160" VisibleIndex="2" Caption="Group Description" Settings-AutoFilterCondition="Contains">
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
                                        <ClientSideEvents  ValueChanged="Groupwiseledger"/>
                                    </dxe:ASPxGridLookup>
                                </dxe:PanelContent>
                            </panelcollection>
                        </dxe:ASPxCallbackPanel>--%>

                <dxe:ASPxButtonEdit ID="txtGroup" runat="server" ReadOnly="true" ClientInstanceName="ctxtGroup" Width="100%" TabIndex="5">
                    <buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </buttons>
                    <clientsideevents buttonclick="function(s,e){GroupButnClick();}" keydown="function(s,e){Group_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>


                    </div>
                </div>
            </div>
            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label3" runat="Server" Text="Customer/Vendor/Transporter:" CssClass="mylabel1"
                        Width="120px"></asp:Label>
                </div>
                <div>
                   <%-- <dxe:ASPxCallbackPanel runat="server" ID="ComponentCustomerVendorPanel" ClientInstanceName="cCustomerVendorComponentPanel" >
                        <panelcollection>
                            <dxe:PanelContent runat="server">                                 
                                <dxe:ASPxGridLookup ID="lookup_custvend" SelectionMode="Multiple" runat="server" TabIndex="7" ClientInstanceName="gridquotationLookup"
                                    OnDataBinding="lookup_vendor_DataBinding"
                                    KeyFieldName="ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                        <dxe:GridViewDataColumn FieldName="Name" Visible="true" SortOrder="Ascending" VisibleIndex="1" Caption="Name" Width="180" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                        <dxe:GridViewDataColumn FieldName="Contact" Visible="true" VisibleIndex="2" Caption="Contact No." Width="180" Settings-AutoFilterCondition="Contains">
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
                                </dxe:ASPxGridLookup>
                            </dxe:PanelContent>
                        </panelcollection>

                    </dxe:ASPxCallbackPanel>--%>
                    
                   <%-- <span id="MandatoryCustomerType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>--%>

                    <dxe:ASPxButtonEdit ID="txtCustVendName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustVendName" Width="100%" TabIndex="6">
                        <buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>
                        </buttons>
                        <clientsideevents buttonclick="function(s,e){CustomerVendorButnClick();}" keydown="function(s,e){CustomerVendor_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>

                </div>
            </div>
            <%--<div class="clear"></div>--%>
            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                    <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </div>
                <div>
                    <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                        <buttonstyle width="13px">
                        </buttonstyle>
                    </dxe:ASPxDateEdit>
                </div>
                <%--Rev 1.0--%>
                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                <%--Rev end 1.0--%>
            </div>
            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </div>
                <div>
                    <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                        <buttonstyle width="13px">
                        </buttonstyle>

                    </dxe:ASPxDateEdit>
                </div>
                <%--Rev 1.0--%>
                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                <%--Rev end 1.0--%>
            </div>
            <div class="clear"></div>
            <div class="col-md-2">
                <div id="ckpar" style="padding-top: 7px; color: #b5285f; font-weight: bold;">
                    <asp:CheckBox runat="server" ID="chkparty" Checked="false" Text="Search by Party Inv. date" />
                </div>
            </div>
            <div class="col-md-2">
                <div id="ckHNarration" style="padding-top: 7px; color: #b5285f; font-weight: bold;">
                    <asp:CheckBox runat="server" ID="chkShowHNarratn" Checked="false" Text="Show Header Narration" />
                </div>
            </div>
            <div class="col-md-2">
                <div>
                    <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
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
            </div>
        </div>
        <div class="pull-right">
        </div>
        <table class="TableMain100">
            <tr>
                <td colspan="2">
                    <div onkeypress="OnWaitingGridKeyPress(event)">
                        
                        <%--<dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                            OnCustomSummaryCalculate="dgvVIEW_CustomSummaryCalculate"
                            OnSummaryDisplayText="ShowGrid_SummaryDisplayText" OnDataBound="Showgrid_Htmlprepared" ClientSideEvents-BeginCallback="Callback2_EndCallback"
                            OnCustomCallback="Grid_CustomCallback" Settings-HorizontalScrollBarMode="Visible">--%>

                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                            OnSummaryDisplayText="ShowGrid_SummaryDisplayText" ClientSideEvents-BeginCallback="Callback2_EndCallback"
                             Settings-HorizontalScrollBarMode="Visible" DataSourceID="GenerateEntityServerModeDataSource" >
                            <columns>
                                <%--<dxe:GridViewDataTextColumn FieldName="RID" Caption="RID" Width="50px" VisibleIndex="1" />--%>
                                <dxe:GridViewDataTextColumn FieldName="BRANCH_DESC" Caption="Unit" Width="20%" VisibleIndex="2" />
                                <dxe:GridViewDataTextColumn FieldName="CustomerVendor" Caption="Customer/Vendor/Transporter" Width="18%" VisibleIndex="3" />
                                <dxe:GridViewDataTextColumn FieldName="TRAN_DATE" Caption="Transaction Date" Width="13%" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" />


                                <%--<dxe:GridViewDataTextColumn FieldName="DOCUMENT_NO" Caption="DOCUMENT NO." Width="12%" VisibleIndex="5" />--%>



                                <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="DOCUMENT_NO" Caption="Document No." Width="17%">
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>

                                        <a href="javascript:void(0)" onclick="OpenPOSDetails('<%#Eval("DOC_ID") %>','<%#Eval("MODULE_TYPE") %>','<%#Eval("DOCUMENT_NO") %>')" class="pad">
                                            <%#Eval("DOCUMENT_NO")%>
                                        </a>

                                    </DataItemTemplate>

                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="TRAN_TYPE" Caption="Transaction Type" Width="15%" VisibleIndex="6" />

                                <dxe:GridViewDataTextColumn FieldName="party_date" Caption="Invoice Date" Width="13%" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" />

                                <dxe:GridViewDataTextColumn FieldName="party_inv" Caption="Invoice No." Width="15%" VisibleIndex="8" />

                                <%--<dxe:GridViewDataTextColumn FieldName="MODULE_TYPE" Caption="MODULE TYPE" VisibleIndex="7" />--%>
                                <dxe:GridViewDataTextColumn FieldName="Particulars" Caption="Header Narration" Width="15%" VisibleIndex="9" />

                               <%-- <dxe:GridViewDataTextColumn FieldName="DEBIT" Caption="Debit" VisibleIndex="10" Width="15%" PropertiesTextEdit-DisplayFormatString="0.00" />--%>
                                <dxe:GridViewDataTextColumn FieldName="DEBIT" Caption="Debit" VisibleIndex="10" >
                                <%--PropertiesTextEdit-DisplayFormatString="0.00" /--%>
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <%--<dxe:GridViewDataTextColumn FieldName="CREDIT" Caption="Credit" Width="15%" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="0.00" />--%>
                                 <dxe:GridViewDataTextColumn FieldName="CREDIT" Caption="Credit" VisibleIndex="11" >
                                <%--PropertiesTextEdit-DisplayFormatString="0.00" /--%>
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <%--<dxe:GridViewDataTextColumn FieldName="Closing_Balance" Caption="Closing Balance" Width="15%" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="0.00" />--%>
                                <dxe:GridViewDataTextColumn FieldName="Closing_Balance" Caption="Balance" VisibleIndex="12" FooterCellStyle-HorizontalAlign="Right">
                                    <%--PropertiesTextEdit-DisplayFormatString="0.00" /--%>
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <%--<dxe:GridViewDataTextColumn FieldName="Closebal_DBCR" Caption="DbCrType" Width="10%" VisibleIndex="13" PropertiesTextEdit-DisplayFormatString="0.00" />--%>
                                <dxe:GridViewDataTextColumn FieldName="Closebal_DBCR" Caption="Type" Width="10%" VisibleIndex="13" />
                            </columns>
                            <settingsbehavior confirmdelete="true" enablecustomizationwindow="true" enablerowhottrack="true" />
                            <settings showfooter="true" showgrouppanel="true" showgroupfooter="VisibleIfExpanded" />
                            <settingsediting mode="EditForm" />
                            <settingscontextmenu enabled="true" />
                            <settingsbehavior autoexpandallgroups="true" columnresizemode="Control" />
                            <settings showgrouppanel="True" showstatusbar="Visible" showfilterrow="true" />
                            <settingssearchpanel visible="false" />
                            <settingspager pagesize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </settingspager>
                            <settings showgrouppanel="True" showstatusbar="Hidden" showhorizontalscrollbar="False" showfilterrow="true" showfilterrowmenu="true" />
                            <totalsummary>
                                  <dxe:ASPxSummaryItem FieldName="DEBIT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="CREDIT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Closing_Balance" SummaryType="Custom" >
                                </dxe:ASPxSummaryItem>
                                <dxe:ASPxSummaryItem FieldName="Closebal_DBCR" SummaryType="Custom" />

                            </totalsummary>
                        </dxe:ASPxGridView>
                         <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="CONSPARTYLEDGER_REPORT" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </div>

     <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfIsCustFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

   <!--Customer/Vendor Modal -->
    <div class="modal fade" id="CustVendModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Customer/Vendor/Transporter Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="CustomerVendorkeydown(event)" id="txtCustVendSearch" width="100%" placeholder="Search By Customer/Vendor/Transporter Name/Alternate No." />
                    <div id="CustomerVendorTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size: small">
                                <th>Select</th>
                                <th class="hide">id</th>
                                <th>Name</th>
                                <th>Unique Id</th>
                                <th>Party Type</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default" onclick="DeSelectAll('CustomerVendorSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('CustomerVendorSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
   <!--Customer/Vendor Modal -->

   <!--Group Modal -->
    <div class="modal fade" id="GroupModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Group Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Groupkeydown(event)" id="txtGroupSearch" width="100%" placeholder="Search By Group Name" />
                    <div id="GroupTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size: small">
                                <th>Select</th>
                                <th class="hide">id</th>
                                <th>Group Description</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default" onclick="DeSelectAll('GroupSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('GroupSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
    <!--Group Modal -->

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupbudget" Height="500px"
        Width="1200px" HeaderText="Details" Modal="true" AllowResize="true">
        <contentcollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </contentcollection>

        <clientsideevents closeup="BudgetAfterHide" />
    </dxe:ASPxPopupControl>

    <asp:HiddenField ID="hdfVendor" runat="server" />
    <asp:HiddenField ID="hdnSelectedGroups" runat="server" />
    <asp:HiddenField ID="hdnSelectedCustomerVendor" runat="server" />
    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
</asp:Content>
