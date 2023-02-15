<%--================================================== Revision History ============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                15-02-2023        V2.0.36           Pallab              25575 : Report pages design modification
====================================================== Revision History ================================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="TradingandPLStatement.aspx.cs" Inherits="Reports.Reports.GridReports.TradingandPLStatement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <style>
        .row.no-gutters {
            margin-right: 0;
            margin-left: 0;
        }

        #TreeList td {
            white-space: pre-wrap;
        }

        .no-gutters > div {
            padding: 0;
        }

        .padtbl > tbody > tr > td {
            padding-right: 15px;
        }

        .ml-3 {
            margin-left: 3px;
        }

        .m0 {
            margin: 0 !important;
        }

        .TradingandPLStatement > tbody > tr > td {
            vertical-align: middle;
            padding-right: 10px;
            padding-bottom: 10px;
        }

        .shadowBox {
            /*margin-top: 10px;
            background: #f7f7f7;
            padding: 15px;
            border: 1px solid #dfdddd;
            border-radius: 4px;*/
            /* box-shadow: 0 0 4px rgba(1, 0, 0, 0.15); */
            margin-bottom: 10px;
        }

        .mylabel1 {
            display: block;
        }

        #TreeList_D>tbody>tr>td:last-child,
        #TreeList_D>thead>tr>th:last-child,
        #TreeList1_D>tbody>tr>td:last-child,
        #TreeList1_D>thead>tr>th:last-child{
            text-align:right;
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

        label , .mylabel1, .clsTo
        {
            color: #141414 !important;
            font-size: 14px;
                font-weight: 500 !important;
                margin-bottom: 0 !important;
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
            right: 5px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img
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
            right: 7px;
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

        /*table
        {
            max-width: 99% !important;
        }*/

        .btn-info
        {
                background-color: #1da8d1 !important;
                background-image: none;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/

        #ddlValTech
        {
            margin-bottom: 0;
        }

    </style>


    <script type="text/javascript">


        function ClosePopUpstkDet(s, e) {
            $("#ddlExport6").val(0);
        }
        function ClosePopUpstkSum(s, e) {
            $("#ddlExport5").val(0);
        }
        function ClosePopUpstkDetLev1(s, e) {
            $("#ddlExport4").val(0);
        }
        function ClosePopUpstkDetLev2(s, e) {
            $("#ddlExport3").val(0);
        }

        $(document).ready(function () {
            $("#ddlExport6").val(0);
            $("#ddlExport5").val(0);
            $("#ddlExport4").val(0);
            $("#ddlExport3").val(0);
        })








        $(function () {
            cProductbranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            $("#ddlbranchHO").change(function () {
                var Ids = $(this).val();
                cProductbranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            })
        });

        document.onkeyup = function (e) {
            if (event.keyCode == 27 && cpopup2ndLevel.GetVisible() == true && popupdocument.GetVisible() == false) {
                popupHide();
            }
            else if (event.keyCode == 27 && popupdocument.GetVisible() == true) {
                popupHide2();
            }
        }
        function popupHide(s, e) {
            cpopup2ndLevel.Hide();
            
            $("#drdExport").val(0);
        }

        function OnGetRowValuesLvl2(id) {
            $("#hdnEntity_Id").val(id);
            cCallbackPanelDetail.PerformCallback();
            cpopup2ndLevel.Show();
        }

        function RwoClick2ndLevel(s, e) {
            $("#hdnEntity_Id").val(s.GetRowKey(e.visibleIndex));
            cCallbackPanelDetail.PerformCallback();
            cpopup2ndLevel.Show();
        }


        function popupHide2(s, e) {
            popupdocument.Hide();
            cShowGridDetails2Level.Focus();
            $("#ddlExport3").val(0);
        }


        function OnRowClick(s, e) {

            $("#hdnLedger").val(e.nodeKey);
            var branchid = $('#ddlbranchHO').val();

            var OtherDetails = {}
            OtherDetails.LedgerId = e.nodeKey;
            $.ajax({
                type: "POST",
                url: "BSStatement.aspx/GetLedgerCode",
                data: JSON.stringify(OtherDetails),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var returnObject = msg.d;
                    $("#hdn_LedgerCode").val(returnObject);
                    if (returnObject != null && returnObject != "") {
                        if (returnObject == "SYSTM00011" || returnObject == "SYSTM00007" || returnObject == "SYSTM00008") {
                            cclStockSummary.PerformCallback();
                            cpopupStockSummary.Show();
                        }
                        else {
                            cCallbackPanelDetail1.PerformCallback();
                            cpopup1ndLevel.Show();
                        }
                    }



                },
                error: function (msg) {

                }
            });




        }

        function OpenBillDetails(branch, prodid) {
            $("#hfIsStockValDetFilter").val("Y");
            $("#hdnProductiD").val(prodid);
            $("#hdnBranchId").val(branch);
            ccpStockDetails.PerformCallback();
            cpopStockDetails.Show();
            //return true;

        }


        function EndShowGridDetails3Level() {

            cShowGridDetails3Level.Focus();
            ctxtProductCode3rdLevel.SetText(ctxtProductCode2ndLevel.GetText());
            ctxtProductDesc3rdLevel.SetText(ctxtProductDesc2ndLevel.GetText());
            $("#lblFromDate3rdLevel")[0].innerHTML = $("#lblFromDate2ndLevel")[0].innerHTML;
            $("#lblToDate3rdLevel")[0].innerHTML = $("#lblToDate2ndLevel")[0].innerHTML;
        }

        function OnGetRowValuesLvl3(Uniqueid, type, docno) {
            //alert(Uniqueid + ' ' + type);
            var url = '';
            if (type == 'POS') {
                url = '/OMS/Management/Activities/posSalesInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&Viemode=1';
            }
            else if (type == 'SI') {
                url = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }

            else if (type == 'PC') {
                url = '/OMS/Management/Activities/PurchaseChallan.aspx?key=' + Uniqueid + '&req=V&IsTagged=1&type=' + type;
            }

            else if (type == 'SR') {
                url = '/OMS/Management/Activities/SalesReturn.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }

            else if (type == 'SRM') {
                url = '/OMS/Management/Activities/ReturnManual.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }

            else if (type == 'SRN') {
                url = '/OMS/Management/Activities/ReturnNormal.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;

            }

            else if (type == 'PI') {
                url = '/OMS/Management/Activities/PurchaseInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=PB';
            }


            else if (type == 'VP' || type == 'VR') {
                url = '/OMS/Management/Activities/VendorPaymentReceipt.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=VPR';
            }

            else if (type == 'PR') {
                url = '/OMS/Management/Activities/PReturn.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=PR';
            }

            else if (type == 'SC') {
                url = '/OMS/Management/Activities/CustomerReturn.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }

            else if (type == 'CP' || type == 'CR') {
                url = '/OMS/Management/Activities/CustomerReceiptPayment.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=CRP';
            }

            else if (type == 'JV') {
                url = '/OMS/Management/dailytask/JournalEntry.aspx?key=' + Uniqueid + '&IsTagged=1&req=' + docno;
            }
            else if (type == 'CBV') {
                url = '/OMS/Management/dailytask/CashBankEntry.aspx?key=' + Uniqueid + '&IsTagged=1&req=V';
            }

            else if (type == 'CNC' || type == 'DNC') {
                url = '/OMS/Management/Activities/CustomerNote.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=CDCN';
            }

            else if (type == 'CNV' || type == 'DNV') {
                url = '/OMS/Management/Activities/VendorDebitCreditNote.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=CDCN';
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
            popupdocument.SetContentUrl(url);
            popupdocument.Show();
        }

        
        Date.prototype.toShortFormat = function () {

            var month_names = ["Jan", "Feb", "Mar",
                              "Apr", "May", "Jun",
                              "Jul", "Aug", "Sep",
                              "Oct", "Nov", "Dec"];

            var day = this.getDate();
            var month_index = this.getMonth();
            var year = this.getFullYear();

            return "" + day + "-" + month_names[month_index] + "-" + year;
        }


        function printDiv(elem) {

            var mywindow = window.open('', 'PRINT', 'height=400,width=600');

            mywindow.document.write('<html><head><title>' + '' + '</title>');

            mywindow.document.write('<h1 align="center">' + '<%=HttpContext.Current.Session["lablelCompanyName"]%>' + '</h1>');
            mywindow.document.write('<h1 align="center">Trading and PL</h1>');
            if (!document.getElementById('radPeriod').checked) {
                mywindow.document.write('<p  align="center">' + 'As at Date : ' + cxdeToDate.GetDate().toShortFormat() + '</p>');
            }
            else {
                mywindow.document.write('<p  align="center">' + 'Period : ' + cxdeFromDate.GetText() + ' - ' + cxdeToDate.GetText() + '</p>');
            }

            mywindow.document.write('<style> @media print { #TreeList_D, #TreeList1_D {border-collapse: collapse;} tr#TreeList_HDR, tr#TreeList1_HDR, .MakeFooter { background-color: #1a4567 !important;color: #fff !important; -webkit-print-color-adjust: exact; } .MakeGreen { background-color: #D3EBB7 !important;color: #fff !important; -webkit-print-color-adjust: exact; }body {border: 3px solid #ddd }} ');
            mywindow.document.write('@media print {#TreeList_HDR th, #TreeList1_HDR th, .MakeFooter td { color: white !important; }.MakeGreen td { color: #333 !important; }}');
            mywindow.document.write(' @media print { @page { size:  auto; margin: 50px;  } ');
            mywindow.document.write('</style>');

            mywindow.document.write('</head><body >');
            mywindow.document.write(document.getElementById(elem).innerHTML);
            mywindow.document.write('</body></html>');
            mywindow.document.close(); // necessary for IE >= 10
            mywindow.focus(); // necessary for IE >= 10*/
            mywindow.print();
            mywindow.close();

            return true;
        }



        function Callback2_EndCallback() {
            if (Grid.cpErrorFinancial == 'ErrorFinancial') {
                jAlert('Date Range should be within Financial Year');
            }
            else {
                var Amount = parseFloat(Grid.cpSummary);
                ctxtdiffcalculation.SetText(Amount);
                ctxtdiffcalculationText.SetText('Mismatch Defeated');
                Grid.cpSummary = null;
                $("#drdExport").val(0);
                Grid.Focus();
                Grid.SetFocusedRowIndex(2);
            }
            Grid.cpErrorFinancial = null;
        }
        var AsonWise = false;
        var ason = 'Y';


        function calc() {
            if (document.getElementById('chkExpandAll').checked) {
                cCallbackPanel.PerformCallback("ExpandAll");
                

            } else {
                cCallbackPanel.PerformCallback("CollapseAll");
                
            }
        }

        function calcPercentage() {
            if (document.getElementById('chkPercentage').checked) {
                cCallbackPanel.PerformCallback("ShowPercentage");
                

            } else {
                cCallbackPanel.PerformCallback("HidePercentage");
               
            }
        }



        function btn_ShowRecordsClick(e) {
            $("#hfIsProfnloss").val("Y");
            var data = "OnDateChanged";
            var branchid = $('#ddlbranchHO').val();
            var layoutid = $('#ddlLayout').val();
            //Grid.PerformCallback(data + '~' + AsonWise + '~' + branchid + '~' + layoutid);
            if (gridbranchLookup.GetValue() == null) {
                jAlert('Please select atleast one Branch for generate the report.');
            }
            else {

                cCallbackPanel.PerformCallback("BindGrid");
                //treeList1.PerformCallback("BindGrid");
            }

            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
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



        function OnGetRowValuesCallback(values) {
            alert(values);
        }

        $(document).ready(function () {
            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                 $('#MandatoryActivityType').attr('style', 'display:none');
             });
        });
         function DateAll(obj) {

             if (obj == 'all') {
                 //document.getElementById("lblFromDate").innerHTML = 'As On Date'
                 //document.getElementById("dvtodate").style.display = "none";
                 //document.getElementById("ASPxFromDate").style.visibility = "visible";

                 document.getElementById("lblToDate").innerHTML = 'As On Date :'
                 document.getElementById("dvFromdate").style.display = "none";
                 document.getElementById("ASPxToDate").style.visibility = "visible";
                 AsonWise = true;
             }
             else {
                 //document.getElementById("lblFromDate").style.visibility = "visible";
                 //document.getElementById("lblFromDate").innerHTML = 'From Date'
                 //document.getElementById("dvtodate").style.display = "block";
                 //document.getElementById("ASPxFromDate").style.visibility = "visible";

                 document.getElementById("lblToDate").style.visibility = "visible";
                 document.getElementById("lblToDate").innerHTML = 'To Date :'
                 document.getElementById("dvFromdate").style.display = "table-cell";
                 document.getElementById("ASPxToDate").style.visibility = "visible";
                 AsonWise = false;
             }
         }

         function CheckConsPL(s, e) {

         }
         $(document).ready(function () {
             //document.getElementById("lblToDate").innerHTML = 'As On Date :'
             //document.getElementById("dvFromdate").style.display = "none";
             //document.getElementById("ASPxToDate").style.visibility = "visible";
             //AsonWise = true;
             document.getElementById("lblToDate").style.visibility = "visible";
             document.getElementById("lblToDate").innerHTML = 'To Date :'
             document.getElementById("dvFromdate").style.display = "table-cell";
             document.getElementById("ASPxToDate").style.visibility = "visible";
             AsonWise = false;
         });
    </script>

    <script type="text/javascript">
        function CallbackPanelEndCall() {

        }

        function cxdeToDate_OnChaged(s, e) {
            var data = "OnDateChanged";
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
        }

        function OnContextMenuItemClick(sender, args) {
            if (args.item.name == "ExportToPDF" || args.item.name == "ExportToXLS") {
                args.processOnServer = true;
                args.usePostBack = true;
            } else if (args.item.name == "SumSelected")
                args.processOnServer = true;
        }

        function abc() {
            $("#drdExport").val(0);
        }

        function BudgetAfterHide(s, e) {
            popupbudget.Hide();
        }

        function CloseGridQuotationLookup() {
            gridquotationLookup.ConfirmCurrentSelection();
            gridquotationLookup.HideDropDown();
            gridquotationLookup.Focus();
        }

        function CloseGridQuotationLookupbranch() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }

        function selectAll_Branch() {
            gridbranchLookup.gridView.SelectRows();
        }

        function unselectAll_Branch() {
            gridbranchLookup.gridView.UnselectRows();
        }

    </script>
    <%--for 2nd level popup--%>
    <script type="text/javascript">
        function OnWaitingGridKeyPress(e) {
            //if (e.code == "Enter" || e.code == "NumpadEnter") {
            if (e.key == "Enter" || e.key == "NumpadEnter") {
                var index = Grid.GetFocusedRowIndex();
                if (AsonWise == false) {
                    ason = 'N';
                }
                else {
                    ason = 'Y';
                }
                $("#ddlExport3").val(0);
                var index = Grid.GetFocusedRowIndex();
                //var ledger = Grid.GetRowKey(index);
                var ledger = Grid.GetRow(index).children[8].innerHTML;
                var branchid = $('#ddlbranchHO').val();
                $("#hfIsProfnlossDetails").val("Y");
                var ledgertype = Grid.GetRow(index).children[7].innerHTML;

                //cCallbackPanelDetail.PerformCallback(ledger + "~" + ason + "~" + branchid + "~" + ledgertype);
                //cpopup2ndLevel.Show();
                if (ledger.trim() == "0") {
                    jAlert('Zooming from Total is not possible.');
                    cpopup2ndLevel.Hide();
                }
                else {
                    cCallbackPanelDetail.PerformCallback(ledger + "~" + ason + "~" + branchid + "~" + ledgertype);
                    cpopup2ndLevel.Show();
                }
            }
        }


        //function EndShowGridDetails2Level() {
        //    cShowGridDetails2Level.Focus();
        //    cShowGridDetails2Level.SetFocusedRowIndex(0);
        //    ctxtLedger2ndLevel.SetText(cShowGridDetails2Level.cpLedger);

        //    if (document.getElementById('radAsDate').checked) {
        //        $("#lblFromDate2ndLevel")[0].innerHTML = "As on : " + cShowGridDetails2Level.cpFromDate;
        //        $("#lblToDate2ndLevel")[0].innerHTML = "";
        //    }
        //    else if (document.getElementById('radPeriod').checked) {
        //        $("#lblFromDate2ndLevel")[0].innerHTML = "From " + cShowGridDetails2Level.cpFromDate;
        //        $("#lblToDate2ndLevel")[0].innerHTML = " To " + cShowGridDetails2Level.cpToDate;
        //    }

        //    if (cShowGridDetails2Level.cpLedgerType != "FOR BRANCH") {
        //        $("#Label3")[0].innerHTML = "Ledger :";
        //    }
        //    else {
        //        $("#Label3")[0].innerHTML = "Branch :";
        //    }
        //}



        function CallbackPanelDetailEndCall() {
            cShowGridDetails2Level.Refresh();
            cShowGridDetails2Level.Focus();
            cShowGridDetails2Level.SetFocusedRowIndex(0);
            ctxtLedger2ndLevel.SetText(cCallbackPanelDetail.cpLedger);

            if (document.getElementById('radAsDate').checked) {
                $("#lblFromDate2ndLevel")[0].innerHTML = "As on : " + cCallbackPanelDetail.cpFromDate;
                $("#lblToDate2ndLevel")[0].innerHTML = "";
            }
            else if (document.getElementById('radPeriod').checked) {
                $("#lblFromDate2ndLevel")[0].innerHTML = "From " + cCallbackPanelDetail.cpFromDate;
                $("#lblToDate2ndLevel")[0].innerHTML = " To " + cCallbackPanelDetail.cpToDate;
            }

            if (cShowGridDetails2Level.cCallbackPanelDetail != "FOR BRANCH") {
                $("#Label3")[0].innerHTML = "Ledger :";
            }
            else {
                $("#Label3")[0].innerHTML = "Branch :";
            }
            if (cCallbackPanelDetail.cpBlankLedger == "0") {
                ctxtLedger2ndLevel.SetText('');
            }
        }

    </script>
    <%--end 2nd level popup--%>

    <%--end 3rd level popup--%>
    <script type="text/javascript">
        function OnWaitingGridKeyPress2(e) {
            if (e.code == "Enter" || e.code == "NumpadEnter") {
            }
        }

        function EndShowGridDetails3Level() {

            cShowGridDetails3Level.Focus();
            ctxtProductCode3rdLevel.SetText(ctxtProductCode2ndLevel.GetText());
            ctxtProductDesc3rdLevel.SetText(ctxtProductDesc2ndLevel.GetText());
            $("#lblFromDate3rdLevel")[0].innerHTML = $("#lblFromDate2ndLevel")[0].innerHTML;
            $("#lblToDate3rdLevel")[0].innerHTML = $("#lblToDate2ndLevel")[0].innerHTML;
        }

        function OnGetRowValuesLvl3(Uniqueid, type, docno) {
            //alert(Uniqueid + ' ' + type);
            var url = '';
            if (type == 'POS') {
                url = '/OMS/Management/Activities/posSalesInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&Viemode=1';
            }
            else if (type == 'SI') {
                url = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }

            else if (type == 'PC') {
                url = '/OMS/Management/Activities/PurchaseChallan.aspx?key=' + Uniqueid + '&req=V&IsTagged=1&type=' + type;
            }

            else if (type == 'SR') {
                url = '/OMS/Management/Activities/SalesReturn.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }

            else if (type == 'SRM') {
                url = '/OMS/Management/Activities/ReturnManual.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }

            else if (type == 'SRN') {
                url = '/OMS/Management/Activities/ReturnNormal.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;

            }

            else if (type == 'PI') {
                url = '/OMS/Management/Activities/PurchaseInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=PB';
            }


            else if (type == 'VP' || type == 'VR') {
                url = '/OMS/Management/Activities/VendorPaymentReceipt.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=VPR';
            }

            else if (type == 'PR') {
                url = '/OMS/Management/Activities/PReturn.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=PR';
            }

            else if (type == 'SC') {
                url = '/OMS/Management/Activities/CustomerReturn.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }

            else if (type == 'CP' || type == 'CR') {
                url = '/OMS/Management/Activities/CustomerReceiptPayment.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=CRP';
            }

            else if (type == 'JV') {
                url = '/OMS/Management/dailytask/JournalEntry.aspx?key=' + Uniqueid + '&IsTagged=1&req=' + docno;
            }
            else if (type == 'CBV') {
                url = '/OMS/Management/dailytask/CashBankEntry.aspx?key=' + Uniqueid + '&IsTagged=1&req=V';
            }

            else if (type == 'CNC' || type == 'DNC') {
                url = '/OMS/Management/Activities/CustomerNote.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=CDCN';
            }

            else if (type == 'CNV' || type == 'DNV') {
                url = '/OMS/Management/Activities/VendorDebitCreditNote.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=CDCN';
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
            popupdocument.SetContentUrl(url);
            popupdocument.Show();
        }

        function DateChangeForFrom() {

            var sessionValFrom = "<%=Session["FinYearStart"]%>";
            var sessionValTo = "<%=Session["FinYearEnd"]%>";
            var sessionVal = "<%=Session["LastFinYear"]%>";
            var objsession = sessionVal.split('-');
            var MonthDate = cxdeFromDate.GetDate().getMonth() + 1;
            var DayDate = cxdeFromDate.GetDate().getDate();
            var YearDate = cxdeFromDate.GetDate().getYear();
            var Cdate = MonthDate + "/" + DayDate + "/" + YearDate;
            var Sto = new Date(sessionValTo).getMonth() + 1;
            var SFrom = new Date(sessionValFrom).getMonth() + 1;
            var SDto = new Date(sessionValTo).getDate();
            var SDFrom = new Date(sessionValFrom).getDate();

            if (YearDate >= objsession[0]) {
                if (MonthDate < SFrom && YearDate == objsession[0]) {
                    jAlert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                    cxdeFromDate.SetDate(new Date(datePost));
                }
                else if (MonthDate > Sto && YearDate == objsession[1]) {
                    jAlert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                    cxdeFromDate.SetDate(new Date(datePost));
                }
                else if (YearDate != objsession[0] && YearDate != objsession[1]) {
                    jAlert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                    cxdeFromDate.SetDate(new Date(datePost));
                }
            }
            else {
                jAlert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                cxdeFromDate.SetDate(new Date(datePost));
            }
        }
        function DateChangeForTo() {
            var sessionValFrom = "<%=Session["FinYearStart"]%>";
            var sessionValTo = "<%=Session["FinYearEnd"]%>";
            var sessionVal = "<%=Session["LastFinYear"]%>";
            var objsession = sessionVal.split('-');
            var MonthDate = cxdeToDate.GetDate().getMonth() + 1;
            var DayDate = cxdeToDate.GetDate().getDate();
            var YearDate = cxdeToDate.GetDate().getYear();
            var Cdate = MonthDate + "/" + DayDate + "/" + YearDate;
            var Sto = new Date(sessionValTo).getMonth() + 1;
            var SFrom = new Date(sessionValFrom).getMonth() + 1;
            var SDto = new Date(sessionValTo).getDate();
            var SDFrom = new Date(sessionValFrom).getDate();

            if (YearDate <= objsession[1]) {
                if (MonthDate < SFrom && YearDate == objsession[0]) {
                    jAlert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
                    cxdeToDate.SetDate(new Date(datePost));
                }
                else if (MonthDate > Sto && YearDate == objsession[1]) {
                    jAlert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
                    cxdeToDate.SetDate(new Date(datePost));
                }
                else if (YearDate != objsession[0] && YearDate != objsession[1]) {
                    jAlert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
                    cxdeToDate.SetDate(new Date(datePost));
                }
            }
            else {
                jAlert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
                cxdeToDate.SetDate(new Date(datePost));
            }
        }
    </script>
    <%--end 3rd level popup--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading">
        <div id="" class="panel-title">
            <h3>
                <span id="">Trading and PL Statement</span>
            </h3>
        </div>

    </div>
    <%--Rev 1.0: "outer-div-main" class add: --%>
    <div class="outer-div-main">
        <div class="form_main">
        <div class="shadowBox">
            <table class="TradingandPLStatement">
                <tr>
                    <td width="200px">
                        <label style="color: #b5285f; font-weight: bold;" class="clsFrom">
                            <asp:Label ID="Label3" runat="Server" Text="Choose Type : " CssClass="mylabel1"
                                Width="92px" Font-Bold="True"></asp:Label>
                        </label>
                        <table class="padtbl">
                            <tr>
                                <td>
                                    <asp:RadioButton ID="radPeriod" runat="server" Checked="True" GroupName="a1" /><span class="ml-3">Period</span>
                                </td>
                                <td>
                                    <asp:RadioButton ID="radAsDate" runat="server"  GroupName="a1" /><span class="ml-3">As On Date</span>
                                </td>
                                
                            </tr>
                        </table>
                    </td>
                    <td id="dvFromdate">
                        <%--Rev 1.0 : "for-cust-icon" class add--%>
                        <div class="for-cust-icon">
                            <label style="color: #b5285f; font-weight: bold;" class="clsFrom">
                                <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"
                                    Width="92px" Font-Bold="True"></asp:Label>
                            </label>
                            <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                                UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                                <buttonstyle width="13px">
                                </buttonstyle>
                                <%--  <ClientSideEvents DateChanged="function(s,e){DateChangeForFrom();}" />--%>
                            </dxe:ASPxDateEdit>
                            <%--Rev 1.0--%>
                            <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                            <%--Rev end 1.0--%>
                        </div>
                    </td>
                    <td id="dvtodate">
                        <%--Rev 1.0 : "for-cust-icon" class add--%>
                        <div class="for-cust-icon">
                            <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                                <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"
                                    Width="92px" Font-Bold="True"></asp:Label>
                            </label>
                            <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                                UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                                <buttonstyle width="13px">
                            </buttonstyle>
                                <%--   <ClientSideEvents DateChanged="function(s,e){DateChangeForTo();}" />--%>
                            </dxe:ASPxDateEdit>
                            <%--Rev 1.0--%>
                            <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                            <%--Rev end 1.0--%>
                        </div>
                    </td>
                    <td>
                          <%--Rev 1.0--%>
                          <%--<div class="">--%>
                          <div class="simple-select">
                          <%--Rev end 1.0--%>
                            <div style="color: #b5285f; font-weight: bold;" class="">
                                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                                    <asp:Label ID="Label2" runat="Server" Text="Head Branch : " CssClass="mylabel1"
                                        Width="" Font-Bold="True"></asp:Label>
                                </label>
                            </div>
                            <asp:DropDownList ID="ddlbranchHO" runat="server" Width="170px" CssClass="m0"></asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <div class="">
                            <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                                <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"
                                    Width="92px" Font-Bold="True"></asp:Label>
                            </label>
                            <%--<asp:ListBox ID="ListBoxBranches" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="100%" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>--%>
                            <asp:HiddenField ID="hdnActivityType" runat="server" />
                            <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                            <span id="MandatoryActivityType" style="display: none" class="validclass">
                                <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                            <asp:HiddenField ID="hdnSelectedBranches" runat="server" />

                            <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cProductbranchPanel" OnCallback="Componentbranch_Callback">
                                <panelcollection>
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
                                                                   <%-- <div class="hide">--%>
                                                                        <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_Branch" />
                                                                   <%-- </div>--%>
                                                                    <dxe:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_Branch" />                                                            
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
                                            <%--<ClientSideEvents ValueChanged="BranchValuewiseledger" />--%>
                                        </dxe:ASPxGridLookup>
                                    </dxe:PanelContent>
                                </panelcollection>

                            </dxe:ASPxCallbackPanel>
                        </div>
                    </td>
                    <td>
                        <div>
                            <div style="color: #b5285f; font-weight: bold;" class="">
                                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                                    <asp:Label ID="Label4" runat="Server" Text="Valuation Technique : " CssClass="mylabel1"
                                        Width="" Font-Bold="True"></asp:Label>
                                </label>
                            </div>
                            <asp:DropDownList ID="ddlValTech" runat="server" Width="170px">
                                <asp:ListItem Text="Average" Value="A"></asp:ListItem>
                                <asp:ListItem Text="FIFO" Selected="True" Value="F"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>

                </tr>
                <tr>
                    <td style="width: 230px;">

                        <div class="" style="color: #b5285f; font-weight: bold;">
                            <div style="padding-right: 10px; vertical-align: middle; padding-top: 6px">
                                <asp:CheckBox ID="chkZero" runat="server" Checked="false" Text="Show Zero Balance Account" />
                            </div>
                        </div>
                    </td>
                    <td>

                        <div class="" style="color: #b5285f; font-weight: bold;">
                            <div style="padding-right: 10px; vertical-align: middle; padding-top: 6px">
                                <asp:CheckBox ID="chkExpandAll" onclick="calc();" runat="server" Checked="false" Text="Expand All" />
                            </div>
                        </div>
                    </td>
                    <td>

                        <div class="" style="color: #b5285f; font-weight: bold;">
                            <div style="padding-right: 10px; vertical-align: middle; padding-top: 6px">
                                <asp:CheckBox ID="chkPercentage" onclick="calcPercentage();" runat="server" Checked="false" Text="Show Percentage" />
                            </div>
                        </div>
                    </td>

                     <td>

                        <div class="" style="color: #b5285f; font-weight: bold;">
                            <div style="padding-right: 10px; vertical-align: middle; padding-top: 6px">
                                <asp:CheckBox ID="chkClosingStock"  runat="server" Checked="true" Text="Consider Closing Stock" />
                            </div>
                        </div>
                    </td>
                     <td>

                        <div class="" style="color: #b5285f; font-weight: bold;">
                            <div style="padding-right: 10px; vertical-align: middle; padding-top: 6px">
                                <asp:CheckBox ID="chkConsiderOverhead"  runat="server" Checked="true" Text="Consider Overhead cost" />
                            </div>
                        </div>
                    </td>

                    <td colspan="2" style="padding-top: 12px;">

                        <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                         <asp:Button runat="server" CssClass="btn btn-info" OnClientClick="PrintType('')"      Text="Print"            OnClick="Button6_Click" ID="Button6" />

                       <%-- <button id="btnPrint" class="btn btn-info" type="button" onclick="printDiv('printableArea')">Print</button>
                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="drdExport_SelectedIndexChanged"
                            AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">XLSX</asp:ListItem>
                            <asp:ListItem Value="3">RTF</asp:ListItem>
                            <asp:ListItem Value="4">CSV</asp:ListItem>

                        </asp:DropDownList>--%>

                    </td>

                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
        </div>
    </div>
    

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="Component_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
         <asp:HiddenField ID="hfIsTransFilter" runat="server" />
             <div class="row no-gutters" id="printableArea" >
        <div class="col-md-6 no-" style="width:50%; float:left">
            <p class="text-center" style="margin: 0;
                        background: #efefef;
                        padding: 5px;
                        font-size: 14px;
                        border: 1px solid #ccc;
                        font-weight: bold;
                        border-bottom: none;
                        margin-right: 2px;">D E B I T</p>
            <dxeTreeList:ASPxTreeList ID="TreeList"  runat="server"  SettingsBehavior-AllowSort="false" AutoGenerateColumns="False" on ClientInstanceName="treeList" OnDataBinding="ASPxTreeList1_DataBinding"
                OnHtmlRowPrepared="ASPxTreeList1_HtmlRowPrepared"
                Width="100%" KeyFieldName="ID" ParentFieldName="ParentID">
                <columns>
            <dxeTreeList:TreeListDataColumn FieldName="Group"   Width="70%" Caption="Particulars" />
            <dxeTreeList:TreeListDataColumn FieldName="LedgerAmount"  Width="30%" Caption=" "  DisplayFormat="#####,##,##,###0.00;(#####,##,##,###0.00)" />
            <dxeTreeList:TreeListDataColumn FieldName="AMOUNT"  Width="30%" Caption="Amount"  DisplayFormat="#####,##,##,###0.00;(#####,##,##,###0.00)" />
            <dxeTreeList:TreeListDataColumn FieldName="Percentage"  Width="30%" Caption="Percentage"  DisplayFormat="#####,##,##,###0.00;(#####,##,##,###0.00)" />

        </columns>
                <ClientSideEvents NodeClick="OnRowClick"></ClientSideEvents>
                <settingsbehavior autoexpandallnodes="true" />
                <settings gridlines="Both" suppressoutergridlines="true" />
                <settingsbehavior autoexpandallnodes="True" expandcollapseaction="NodeDblClick" />
                <border borderstyle="Solid" />
            </dxeTreeList:ASPxTreeList>
        </div>
        <div class="col-md-6" style="width:50%;  float:right">
            <p class="text-center" style="margin: 0;
                    background: #efefef;
                    padding: 5px;
                    font-size: 14px;
                    border: 1px solid #ccc;
                    border-bottom: none;
                    font-weight: bold;
                    margin-left: 2px;">C R E D I T</p>
            <dxeTreeList:ASPxTreeList ID="TreeList1"  runat="server"  SettingsBehavior-AllowSort="false" AutoGenerateColumns="False" ClientInstanceName="treeList1" OnDataBinding="ASPxTreeList1_DataBinding"
                OnHtmlRowPrepared="ASPxTreeList1_HtmlRowPrepared"
                Width="100%" KeyFieldName="ID" ParentFieldName="ParentID">
                <columns>
            <dxeTreeList:TreeListDataColumn FieldName="Group"  Width="70%" Caption="Particulars" />
            <dxeTreeList:TreeListDataColumn FieldName="LedgerAmount"  Width="30%" Caption=" "  DisplayFormat="#####,##,##,###0.00;(#####,##,##,###0.00)"/>
            <dxeTreeList:TreeListDataColumn FieldName="AMOUNT"  Width="30%" Caption="Amount"  DisplayFormat="#####,##,##,###0.00;(#####,##,##,###0.00)"/>
            <dxeTreeList:TreeListDataColumn FieldName="Percentage"  Width="30%" Caption="Percentage"  DisplayFormat="#####,##,##,###0.00;(#####,##,##,###0.00)"/>

        </columns>
                <ClientSideEvents NodeClick="OnRowClick"></ClientSideEvents>
                <settingsbehavior autoexpandallnodes="true" />
                <settings gridlines="Both" suppressoutergridlines="true" />
                <settingsbehavior autoexpandallnodes="True" expandcollapseaction="NodeDblClick" />
                <border borderstyle="Solid" />
            </dxeTreeList:ASPxTreeList>
        </div>
    </div>

            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

</div>
        <dxeTreeList:ASPxTreeListExporter  ID="ExpExporter" EnableTheming="true" runat="server" TreeListID="TreeList" />
        <dxeTreeList:ASPxTreeListExporter  ID="IncomeExporter" EnableTheming="true"  runat="server" TreeListID="TreeList1" />

    <div class="hide">
        <asp:HiddenField runat="server" ID="hfIsProfnlossDetails" />
    </div>

     <dxe:ASPxPopupControl ID="popup" runat="server" ClientInstanceName="cpopup2ndLevel"  CloseOnEscape="true"
        Width="1200px" Height="600px" ScrollBars="Vertical" HeaderText="Trading P/L Statement - Detailed" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"  ClientSideEvents-Closing="ClosePopUpstkDetLev2"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <contentstyle verticalalign="Top" cssclass="pad">
            </contentstyle>
        <contentcollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel2" ClientInstanceName="cCallbackPanelDetail" OnCallback="cCallbackPanelDetail_Callback">
                    <panelcollection>
                            <dxe:PanelContent runat="server">
                            <div class="pull-right" style="padding-top: 26px;">
                                
<%--                                <asp:DropDownList ID="ddlExport3" runat="server" CssClass="btn btn-sm btn-primary" AutoPostBack="true"  OnSelectedIndexChanged="ddlExport3_SelectedIndexChanged">
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                    <asp:ListItem Value="2">XLSX</asp:ListItem>
                                    <asp:ListItem Value="3">RTF</asp:ListItem>
                                    <asp:ListItem Value="4">CSV</asp:ListItem>
                                </asp:DropDownList>--%>
                                <asp:Button ID="Button3"  CssClass="btn btn-sm btn-primary"  Text="Export To Excel" runat="server" OnClick="Button3_Click" />
                            </div>

                    <div class="clearfix" onkeypress="OnWaitingGridKeyPress3(event)">
                        <dxe:ASPxGridView runat="server" ID="ShowGridDetails2Level"  ClientInstanceName="cShowGridDetails2Level" KeyFieldName="SEQ" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                            OnDataBinding="ShowGridDetails2Level_DataBinding" KeyboardSupport="true" Settings-HorizontalScrollBarMode="Visible"
                            OnSummaryDisplayText="ShowGridDetails2Level_SummaryDisplayText" >

                            <Columns>
                                <%--OnCustomCallback="ShowGridDetails2Level_CustomCallback"--%>

                                <dxe:GridViewDataTextColumn FieldName="BRANCH_DESC" Caption="Unit" Width="30%" VisibleIndex="1" Settings-AllowAutoFilter="False"/>
                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="DOC_NO" Caption="Document No." Width="30%" >
                                <CellStyle HorizontalAlign="Left" >
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                                <DataItemTemplate>
                                      <a href="javascript:void(0)" onclick="OnGetRowValuesLvl3('<%#Eval("DOC_ID") %>','<%#Eval("MODULE_TYPE") %>' ,'<%#Eval("DOC_NO") %>')" class="pad">
                                        <%#Eval("DOC_NO")%>
                                      </a> 
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="DOC_TYPE" Caption="Document Type" Width="10%" VisibleIndex="3" Settings-AllowAutoFilter="False"/>
                                 <dxe:GridViewDataDateColumn FieldName="DOC_DATE" Caption="Document Date" Width="10%" VisibleIndex="3" Settings-AllowAutoFilter="False">
                                      <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"></PropertiesDateEdit> 
                                 </dxe:GridViewDataDateColumn>

                                <dxe:GridViewDataTextColumn FieldName="PARTY_NAME" Caption="Party" Width="15%" VisibleIndex="3" />

<%--                                <dxe:GridViewDataTextColumn FieldName="OP_DR_AMT" Caption="Opening (Dr.)" Width="8%" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)" Settings-AllowAutoFilter="False">
                                </dxe:GridViewDataTextColumn>
                           
                                <dxe:GridViewDataTextColumn FieldName="OP_CR_AMT" Caption="Opening (Cr.)" Width="8%" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)" Settings-AllowAutoFilter="False">
                                </dxe:GridViewDataTextColumn>--%>

                                <dxe:GridViewDataTextColumn FieldName="OPENING" Caption="Opening" Width="8%" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)" Settings-AllowAutoFilter="False">
                                </dxe:GridViewDataTextColumn>
                                
                                <dxe:GridViewDataTextColumn FieldName="PR_DR_AMT" Caption="Period (Dr.)" Width="8%" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)" Settings-AllowAutoFilter="False">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PR_CR_AMT" Caption="Period (Cr.)" Width="8%" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)" Settings-AllowAutoFilter="False">
                                </dxe:GridViewDataTextColumn>

                               <%-- <dxe:GridViewDataTextColumn FieldName="CL_DR_AMT" Caption="Closing (Dr.)" Width="8%" VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)" Settings-AllowAutoFilter="False">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CL_CR_AMT" Caption="Closing (Cr.)" Width="8%" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)" Settings-AllowAutoFilter="False">
                                </dxe:GridViewDataTextColumn>--%>

                                <dxe:GridViewDataTextColumn FieldName="Doc_Id" Caption="Doc_Id" Width="15%" VisibleIndex="10" Visible="false">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="MODULE_TYPE" Caption="MODULE_TYPE" Width="8%" VisibleIndex="11" Visible="false">
                                </dxe:GridViewDataTextColumn>
                                <%--<dxe:GridViewDataTextColumn FieldName="NETT" Caption="Nett." Width="8%" VisibleIndex="12" >
                                    <PropertiesTextEdit DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>--%>

                                <dxe:GridViewDataTextColumn FieldName="CLOSING" Caption="Closing" Width="8%" VisibleIndex="12" >
                                    <PropertiesTextEdit DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" ColumnResizeMode="Control" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                            </SettingsPager>
                            <TotalSummary>
                                 <dxe:ASPxSummaryItem FieldName="OP_DR_AMT" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="OP_CR_AMT" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="PR_DR_AMT" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="PR_CR_AMT" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="CL_DR_AMT" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="CL_CR_AMT" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="NETT" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="CLOSING" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="OPENING" SummaryType="Sum"/>
                            </TotalSummary>
                           <%-- <ClientSideEvents EndCallback="EndShowGridDetails2Level" />--%>

                        </dxe:ASPxGridView>
                            <dxe:ASPxGridViewExporter ID="exporterDetails" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
                      </div>
                               
                   </dxe:PanelContent>
                         </panelcollection>
              </dxe:ASPxCallbackPanel>
                   

                </dxe:PopupControlContentControl>
            </contentcollection>

    </dxe:ASPxPopupControl>

        <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server" CloseOnEscape="true"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupdocument" Height="500px"
        Width="1200px" HeaderText="Details" Modal="true" AllowResize="true">
        <contentcollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
        </contentcollection>
    </dxe:ASPxPopupControl>
       <dxe:ASPxPopupControl ID="pop1" runat="server" ClientInstanceName="cpopup1ndLevel"  CloseOnEscape="true"
        Width="1200px" Height="600px" ScrollBars="Vertical" HeaderText="Trading P/L Statement - Detailed" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"  ClientSideEvents-Closing="ClosePopUpstkDetLev1"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <contentstyle verticalalign="Top" cssclass="pad">
            </contentstyle>
        <contentcollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel3" ClientInstanceName="cCallbackPanelDetail1" OnCallback="cCallbackPanelDetail1_Callback">
                    <panelcollection>
                            <dxe:PanelContent runat="server">
                            <div class="pull-right" style="padding-top: 26px;">
                                
<%--                                <asp:DropDownList ID="ddlExport4" runat="server" CssClass="btn btn-sm btn-primary" AutoPostBack="true"   OnSelectedIndexChanged="ddlExport4_SelectedIndexChanged" >
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                    <asp:ListItem Value="2">XLSX</asp:ListItem>
                                    <asp:ListItem Value="3">RTF</asp:ListItem>
                                    <asp:ListItem Value="4">CSV</asp:ListItem>
                                </asp:DropDownList>--%>

                                 <asp:Button ID="FirstImport"  CssClass="btn btn-sm btn-primary"  Text="Export To Excel" runat="server" OnClick="FirstImport_Click" />
                            </div>

                    <div class="clearfix" >
                        <dxe:ASPxGridView runat="server" ID="ShowGridDetails1Level"  ClientInstanceName="cShowGridDetails1Level" KeyFieldName="Entity_Ids" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                            OnDataBinding="ShowGridDetails1Level_DataBinding" KeyboardSupport="true" Settings-HorizontalScrollBarMode="Auto"
                            OnSummaryDisplayText="ShowGridDetails2Level_SummaryDisplayText" >

                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="group_name" Caption="Group Name" Width="15%" VisibleIndex="2" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)">
                                </dxe:GridViewDataTextColumn>
                           
                                <%--OnCustomCallback="ShowGridDetails2Level_CustomCallback"--%>
                                <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="PARTY_NAME" Caption="PARTY NAME" Width="15%" >
                                <CellStyle HorizontalAlign="Left" >
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                                <DataItemTemplate>
                                      <a href="javascript:void(0)" onclick="OnGetRowValuesLvl2('<%#Eval("Entity_Ids") %>')" class="pad">
                                        <%#Eval("PARTY_NAME")%>
                                      </a> 
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>       

                              <%--  <dxe:GridViewDataTextColumn FieldName="OP_DR_AMT" Caption="Opening (Dr.)" Width="12%" VisibleIndex="2" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)" Settings-AllowAutoFilter="False">
                                </dxe:GridViewDataTextColumn>
                           
                                <dxe:GridViewDataTextColumn FieldName="OP_CR_AMT" Caption="Opening (Cr.)" Width="12%" VisibleIndex="3" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)" Settings-AllowAutoFilter="False">
                                </dxe:GridViewDataTextColumn>
                                --%>
                                <dxe:GridViewDataTextColumn FieldName="OPENING" Caption="Opening" Width="20%" VisibleIndex="3" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)" Settings-AllowAutoFilter="False">
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn FieldName="PR_DR_AMT" Caption="Period (Dr.)" Width="15%" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)" Settings-AllowAutoFilter="False">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PR_CR_AMT" Caption="Period (Cr.)" Width="15%" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)" Settings-AllowAutoFilter="False">
                                </dxe:GridViewDataTextColumn>

<%--                                <dxe:GridViewDataTextColumn FieldName="CL_DR_AMT" Caption="Closing (Dr.)" Width="12%" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)" Settings-AllowAutoFilter="False">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CL_CR_AMT" Caption="Closing (Cr.)" Width="12%" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)" Settings-AllowAutoFilter="False">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="NETT" Caption="Nett." Width="12%" VisibleIndex="10" >
                                    <PropertiesTextEdit DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                   --%>      
                                 <dxe:GridViewDataTextColumn FieldName="CLOSING" Caption="Closing" Width="20%" VisibleIndex="10" >
                                    <PropertiesTextEdit DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                            </Columns>
                            <SettingsBehavior  EnableCustomizationWindow="true" ColumnResizeMode="Control" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="8,50,100,150,200" />
                            </SettingsPager>
                             <TotalSummary>
                                 <dxe:ASPxSummaryItem FieldName="OP_DR_AMT" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="OP_CR_AMT" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="PR_DR_AMT" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="PR_CR_AMT" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="CL_DR_AMT" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="CL_CR_AMT" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="NETT" SummaryType="Sum"/>
                                <dxe:ASPxSummaryItem FieldName="CLOSING" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="OPENING" SummaryType="Sum"/>
                            </TotalSummary>
                            <ClientSideEvents RowClick="RwoClick2ndLevel" />

                        </dxe:ASPxGridView>
                            <dxe:ASPxGridViewExporter ID="Exporter1" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
                      </div>
                               
                   </dxe:PanelContent>
                         </panelcollection>
              </dxe:ASPxCallbackPanel>
                   

                </dxe:PopupControlContentControl>
            </contentcollection>

    </dxe:ASPxPopupControl>

         <dxe:ASPxPopupControl ID="StockSummary" runat="server" ClientInstanceName="cpopupStockSummary"  CloseOnEscape="true"
        Width="1200px" Height="600px" ScrollBars="Vertical" HeaderText="Trading P/L Statement - Detailed" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" ClientSideEvents-Closing="ClosePopUpstkSum"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <contentstyle verticalalign="Top" cssclass="pad">
            </contentstyle>
        <contentcollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="clStockSummary" ClientInstanceName="cclStockSummary" OnCallback="cclStockSummary_Callback">
                    <panelcollection>
                    <dxe:PanelContent runat="server">
                    <div class="col-md-12">
                        <div class="row clearfix">
                            
                           <div class="pull-right" style="padding-top: 26px;">
                                
<%--                                <asp:DropDownList ID="ddlExport5" runat="server" CssClass="btn btn-sm btn-primary" AutoPostBack="true"  OnSelectedIndexChanged="ddlExport5_SelectedIndexChanged" >
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                    <asp:ListItem Value="2">XLSX</asp:ListItem>
                                    <asp:ListItem Value="3">RTF</asp:ListItem>
                                    <asp:ListItem Value="4">CSV</asp:ListItem>
                                </asp:DropDownList>--%>

                                <asp:Button ID="Button1"  CssClass="btn btn-sm btn-primary"  Text="Export To Excel" runat="server" OnClick="Button1_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="clearfix">
                        <dxe:ASPxGridView runat="server" ID="gvStockSummary"  ClientInstanceName="cgvStockSummary" KeyFieldName="SLNO" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                            OnDataBinding="gvStockSummary_DataBinding"  Settings-HorizontalScrollBarMode="Auto"
                            OnSummaryDisplayText="ShowGridDetails2Level_SummaryDisplayText" >

                            <Columns>

                                <dxe:GridViewDataTextColumn FieldName="branch_description" Width="280" Caption="Unit" VisibleIndex="1" />
                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="sProducts_Description" Width="420" Caption="Product Name">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>
                                        <a href="#" onclick="OpenBillDetails('<%#Eval("branch_id") %>','<%#Eval("sproducts_Id") %>')" class="pad">
                                            <%#Eval("sproducts_description")%>
                                        </a>
                                    </DataItemTemplate>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="STOCKUOM" Width="80" Caption="Unit" VisibleIndex="3" />
                                <dxe:GridViewDataTextColumn FieldName="ProductClass_Description" Width="280" Caption="Class" VisibleIndex="4" />
                                <dxe:GridViewDataTextColumn FieldName="Brand_Name" Width="220" Caption="Brand" VisibleIndex="5" />
                                <dxe:GridViewDataTextColumn FieldName="IN_QTY_OP" Width="110" Caption="Quantity" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="IN_PRICE_OP" Width="110" Caption="Rate" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)">
                                     <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="IN_TOTAL_OP" Width="110" Caption="Total" VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)">
                                     <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                            </Columns>
                            <SettingsBehavior EnableCustomizationWindow="true" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>

                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="IN_QTY_OP" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="IN_TOTAL_OP" SummaryType="Sum" />
                            </TotalSummary>

                        </dxe:ASPxGridView>
                        <dxe:ASPxGridViewExporter ID="SummaryExporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
                      </div>
                               
                   </dxe:PanelContent>
                         </panelcollection>
              </dxe:ASPxCallbackPanel>
                   

                </dxe:PopupControlContentControl>
            </contentcollection>

    </dxe:ASPxPopupControl>

   


    <dxe:ASPxPopupControl ID="popStockDetails" runat="server" ClientInstanceName="cpopStockDetails"  CloseOnEscape="true"
        Width="1200px" Height="600px" ScrollBars="Vertical" HeaderText="Trading P/L Statement - Detailed" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"  ClientSideEvents-Closing="ClosePopUpstkDet"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <contentstyle verticalalign="Top" cssclass="pad">
            </contentstyle>
        <contentcollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="cpStockDetails" ClientInstanceName="ccpStockDetails" OnCallback="cclStockDetails_Callback">
                    <panelcollection>
                            <dxe:PanelContent runat="server">
                            <div class="pull-right" style="padding-top: 26px;">
                                
                                <%--<asp:DropDownList ID="ddlExport6" runat="server" CssClass="btn btn-sm btn-primary" AutoPostBack="true"  OnSelectedIndexChanged="ddlExport6_SelectedIndexChanged" >
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                    <asp:ListItem Value="2">XLSX</asp:ListItem>
                                    <asp:ListItem Value="3">RTF</asp:ListItem>
                                    <asp:ListItem Value="4">CSV</asp:ListItem>
                                </asp:DropDownList>--%>
                                <asp:Button ID="Button2"  CssClass="btn btn-sm btn-primary"  Text="Export To Excel" runat="server" OnClick="Button2_Click" />
                            </div>



                    <div class="clearfix" onkeypress="OnWaitingGridKeyPress3(event)">
                        <dxe:ASPxGridView runat="server" ID="gridStockDetials"  ClientInstanceName="cgridStockDetials" KeyFieldName="SEQ" Width="100%"  AutoGenerateColumns="False"
                            OnDataBinding="grivaluation_DataBinding" KeyboardSupport="true" Settings-HorizontalScrollBarMode="Auto" OnSummaryDisplayText="ShowGridDetails2Level_SummaryDisplayText"
                              >
                            <Columns>
                            <dxe:GridViewDataTextColumn Caption="Document Date" FieldName="Document_Date" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy"
                                    VisibleIndex="0" FixedStyle="Left" Width="150px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Document No" FieldName="Document_No" VisibleIndex="1" FixedStyle="Left" Width="120px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Document Type" FieldName="Trans_Type" VisibleIndex="2" FixedStyle="Left" Width="200px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Product" FieldName="sProducts_Description" VisibleIndex="3" FixedStyle="Left" Width="150px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                
                                <dxe:GridViewDataTextColumn FieldName="ProductClass_Description" Width="100px" Caption="Class" VisibleIndex="4" />

                                <dxe:GridViewDataTextColumn FieldName="Brand_Name" Width="100px" Caption="Brand" VisibleIndex="5" />

                                <dxe:GridViewDataTextColumn FieldName="STOCKUOM" Width="80px" Caption="Stock Unit" VisibleIndex="6" />

                                <dxe:GridViewDataTextColumn FieldName="ALTSTOCKUOM" Width="80px" Caption="Alt. Unit" VisibleIndex="7" />

                                <dxe:GridViewDataTextColumn FieldName="IN_QTY_OP" Caption="Available Stock" Width="120px" VisibleIndex="8">
                                    <PropertiesTextEdit DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="ALTIN_QTY" Caption="Alt. Stock Qty." Width="120px" VisibleIndex="9">
                                    <PropertiesTextEdit DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="IN_PRICE_OP" Caption="Rate" Width="120px" VisibleIndex="10">
                                    <PropertiesTextEdit DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="IN_TOTAL_OP" Caption="Value" Width="120px" VisibleIndex="11">
                                    <PropertiesTextEdit DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn FieldName="IN_QTY" Caption="Recd. Qty." Width="120px" VisibleIndex="12">
                                    <PropertiesTextEdit DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)"></PropertiesTextEdit>
                                     <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="ALTIN_QTY" Caption="Recd. Alt. Qty." Width="120px" VisibleIndex="13">
                                    <PropertiesTextEdit DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="OUT_QTY" Caption="Delv. Qty." Width="120px" VisibleIndex="14">
                                    <PropertiesTextEdit DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn FieldName="ALTOUT_QTY" Caption="Delv. Alt. Qty." Width="120px" VisibleIndex="15">
                                    <PropertiesTextEdit DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="IN_RATE" Caption="Doc. Rate" Width="120px" VisibleIndex="16">
                                    <PropertiesTextEdit DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="IN_VALUE" Caption="Recd. Value" Width="120px" VisibleIndex="17">
                                    <PropertiesTextEdit DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="OUT_VALUE" Caption="Delv. Value" Width="120px" VisibleIndex="18">
                                    <PropertiesTextEdit DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CLOSE_QTY" Caption="Balance Qty." Width="120px" VisibleIndex="19">
                                    <PropertiesTextEdit DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CLOSE_RATE" Caption="Close Rate" Width="120px" VisibleIndex="20">
                                    <PropertiesTextEdit DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CLOSE_VAL" Caption="Balance Value" Width="120px" VisibleIndex="21">
                                    <PropertiesTextEdit DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                </Columns>
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" ColumnResizeMode="Control" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />                           
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="8,50,100,150,200" />
                            </SettingsPager>

                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="IN_QTY_OP" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="IN_TOTAL_OP" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="IN_QTY" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="OUT_QTY" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="IN_VALUE" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="OUT_VALUE" SummaryType="Sum" />
                            </TotalSummary>

                           <%-- <ClientSideEvents EndCallback="EndShowGridDetails2Level" />--%>

                        </dxe:ASPxGridView>
                                                <dxe:ASPxGridViewExporter ID="DetailsExporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
                      </div>
                               
                   </dxe:PanelContent>
                         </panelcollection>
              </dxe:ASPxCallbackPanel>
                   

                </dxe:PopupControlContentControl>
            </contentcollection>

    </dxe:ASPxPopupControl>



    <asp:HiddenField runat="server" ID="hfIsStockValDetFilter" />
        <asp:HiddenField runat="server" ID="hdnProductiD" />
        <asp:HiddenField runat="server" ID="hdnBranchId" />

    <asp:HiddenField runat="server" ID="hdn_LedgerCode" />
    <asp:HiddenField runat="server" ID="hdnEntity_Id" />
    <asp:HiddenField runat="server" ID="hdnLedger" />







</asp:Content>


