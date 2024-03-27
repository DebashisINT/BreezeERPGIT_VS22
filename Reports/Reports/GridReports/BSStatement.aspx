<%--================================================== Revision History ======================================================================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                16-02-2023        V2.0.36           Pallab              25575 : Report pages design modification
2.0                01-06-2023        V2.0.37           Debashis            Running Balance Required in the Third Level Zooming report for PL - Horizontal & 
                                                                           BS Horizontal from any Ledger.
                                                                           The Running Balance of the 3rd Level zooming of any ledger from PL - Horizontal & 
                                                                           BS - Horizontal to be set as per Ledger Type
                                                                           Refer: 0026252 & 0026318
3.0                27-03-2024       V2.0.46            Sanchita            Contra Transactions showing as Opening Entry in BS. Refer: 0027336  
====================================================== Revision History ==========================================================================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="BSStatement.aspx.cs" Inherits="Reports.Reports.GridReports.BSStatement" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <style>
        /*Rev 2.0*/
        .colDisable {
        cursor:default !important;
        }
        /*End of Rev 2.0*/
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
            margin-top: 10px;
            background: #f7f7f7;
            padding: 15px;
            border: 1px solid #dfdddd;
            border-radius: 4px;
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

    </style>
    <%--<script src="https://cdnjs.cloudflare.com/ajax/libs/html2pdf.js/0.9.1/html2pdf.bundle.min.js" integrity="sha256-5WLU9Y9T0T1S7swCtH9WNzy3IQ77uNGW3cuTP65JSCo=" crossorigin="anonymous"></script>--%>
    <script src="/assests/pluggins/html2pdf/html2pdfbundle.min.js"></script>
    <script type="text/javascript">

        function PrintType(type)
        {
            $("#ExportTypeName").val(type);
            return false;
        } 




        function ClosePopUpstkDet(s,e) {
            $("#ddlExport6").val(0);
        }
        function ClosePopUpstkSum(s,e) {
            $("#ddlExport5").val(0);
        }
        function ClosePopUpstkDetLev1(s,e) {
            $("#ddlExport4").val(0);
        }
        function ClosePopUpstkDetLev2(s,e) {
            $("#ddlExport3").val(0);
        }

        function ddlExport3_chnage()
        {
          //  cShowGridDetails2Level.Refresh();
        }

        $(document).ready(function () {
            $("#ddlExport6").val(0);
            $("#ddlExport5").val(0);
            $("#ddlExport4").val(0);
            $("#ddlExport3").val(0);
        })


        function AllControlInitilize(s, e) {
            $("#ddlExport6").val(0);
            $("#ddlExport5").val(0);
            $("#ddlExport4").val(0);
            $("#ddlExport3").val(0);

            //if ($("#ExportTypeName").val() == "OpenPrintNewTab")
            //{
            //    $("#ExportTypeName").val("");
            //    window.location.href='PLBSViewer.aspx';
            //}



        }










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


            $("#compH1").text('<%=HttpContext.Current.Session["lablelCompanyName"]%>')
        }

        //Rev 2.0
        //function OnGetRowValuesLvl2(id) {
        //    $("#hdnEntity_Id").val(id);
        //    cCallbackPanelDetail.PerformCallback();
        //    cpopup2ndLevel.Show();
        //}
        function OnGetRowValuesLvl2(id,ledgertype) {
            $("#hdnEntity_Id").val(id);
            $("#hdnLedgertype").val(ledgertype);
            cCallbackPanelDetail.PerformCallback();
            cpopup2ndLevel.Show();
        }
        //End of Rev 2.0

        function RwoClick2ndLevel(s,e) {
            $("#hdnEntity_Id").val(s.GetRowKey(e.visibleIndex));
            cCallbackPanelDetail.PerformCallback();
            cpopup2ndLevel.Show();
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

            mywindow.document.write('<h1 align="center">'+'<%=HttpContext.Current.Session["lablelCompanyName"]%>'+'</h1>');
            mywindow.document.write('<h1 align="center">Balance Sheet</h1>');
            if (!document.getElementById('radPeriod').checked) {
                mywindow.document.write('<p  align="center">' + 'As at Date : ' + cxdeToDate.GetDate().toShortFormat() + '</p>');
            }
            else {
                mywindow.document.write('<p  align="center">' + 'Period : ' + cxdeFromDate.GetText() + ' - ' + cxdeToDate.GetText()  + '</p>');
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


        function popupHide(s, e) {
            cpopup2ndLevel.Hide();
            Grid.Focus();
            $("#drdExport").val(0);
        }
        function popupHide2(s, e) {
            popupdocument.Hide();
            cShowGridDetails2Level.Focus();
            $("#ddlExport3").val(0);
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
            document.getElementById("lblToDate").innerHTML = 'As On Date :'
            document.getElementById("dvFromdate").style.display = "none";
            document.getElementById("ASPxToDate").style.visibility = "visible";
            AsonWise = true;
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

        function OnRowClick(s,e) {

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
                async:false,
                success: function (msg) {
                    var returnObject = msg.d;
                    $("#hdn_LedgerCode").val(returnObject);
                    if (returnObject != null && returnObject != "") {
                        if (returnObject == "SYSTM000011" || returnObject == "SYSTM00007" || returnObject == "SYSTM00008") {
                            cclStockSummary.PerformCallback();
                            cpopupStockSummary.Show();
                        }
                        else
                        {
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


        function closePopup(s, e) {
            e.cancel = false;
            Grid.Focus();
            $("#drdExport").val(0);
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
            // Rev 3.0
            else if (type == 'TCB') {
                url = '/OMS/Management/dailytask/ContraVoucher.aspx?key=' + Uniqueid + '&IsTagged=1&req=' + docno;
            }
            // End of Rev 3.0
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

        function generatePDF() {
            $('#TreeList1_D .dxtl__Collapse, #TreeList_D .dxtl__Collapse').css({ 'background': '#a59f9f', 'border-radius': '50%' });
            $('#mainHeader').show();
            var seePercentage = $('#chkPercentage').is(':checked');
            console.log(seePercentage);

            var imgData = 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAABB8AAAAoCAIAAAA5asX5AAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6NzlGOTdFQzEwNzkyMTFFQTg3QTJEN0ZDMzZDNzYwNkIiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6NzlGOTdFQzIwNzkyMTFFQTg3QTJEN0ZDMzZDNzYwNkIiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDo3OUQzMUFBNDA3OTIxMUVBODdBMkQ3RkMzNkM3NjA2QiIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDo3OUQzMUFBNTA3OTIxMUVBODdBMkQ3RkMzNkM3NjA2QiIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/Pm1Le58AAAubSURBVHja7N19bFX1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';
            var imgDataLogo = 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAOQAAABHCAYAAAAX3+vNAAAACXBIWXMAAAsTAAALEwEAmpwYAAAKTWlDQ1BQaG90b3Nob3AgSUNDIHByb2ZpbGUAAHjanVN3WJP3Fj7f92UPVkLY8LGXbIEAIiOsCMgQWaIQkgBhhBASQMWFiApWFBURnEhVxILVCkidiOKgKLhnQYqIWotVXDjuH9yntX167+3t+9f7vOec5/zOec8PgBESJpHmomoAOVKFPDrYH49PSMTJvYACFUjgBCAQ5svCZwXFAADwA3l4fnSwP/wBr28AAgBw1S4kEsfh/4O6UCZXACCRAOAiEucLAZBSAMguVMgUAMgYALBTs2QKAJQAAGx5fEIiAKoNAOz0ST4FANipk9wXANiiHKkIAI0BAJkoRyQCQLsAYFWBUiwCwMIAoKxAIi4EwK4BgFm2MkcCgL0FAHaOWJAPQGAAgJlCLMwAIDgCAEMeE80DIEwDoDDSv+CpX3CFuEgBAMDLlc2XS9IzFLiV0Bp38vDg4iHiwmyxQmEXKRBmCeQinJebIxNI5wNMzgwAABr50cH+OD+Q5+bk4eZm52zv9MWi/mvwbyI+IfHf/ryMAgQAEE7P79pf5eXWA3DHAbB1v2upWwDaVgBo3/ldM9sJoFoK0Hr5i3k4/EAenqFQyDwdHAoLC+0lYqG9MOOLPv8z4W/gi372/EAe/tt68ABxmkCZrcCjg/1xYW52rlKO58sEQjFu9+cj/seFf/2OKdHiNLFcLBWK8ViJuFAiTcd5uVKRRCHJleIS6X8y8R+W/QmTdw0ArIZPwE62B7XLbMB+7gECiw5Y0nYAQH7zLYwaC5EAEGc0Mnn3AACTv/mPQCsBAM2XpOMAALzoGFyolBdMxggAAESggSqwQQcMwRSswA6cwR28wBcCYQZEQAwkwDwQQgbkgBwKoRiWQRlUwDrYBLWwAxqgEZrhELTBMTgN5+ASXIHrcBcGYBiewhi8hgkEQcgIE2EhOogRYo7YIs4IF5mOBCJhSDSSgKQg6YgUUSLFyHKkAqlCapFdSCPyLXIUOY1cQPqQ28ggMor8irxHMZSBslED1AJ1QLmoHxqKxqBz0XQ0D12AlqJr0Rq0Hj2AtqKn0UvodXQAfYqOY4DRMQ5mjNlhXIyHRWCJWBomxxZj5Vg1Vo81Yx1YN3YVG8CeYe8IJAKLgBPsCF6EEMJsgpCQR1hMWEOoJewjtBK6CFcJg4Qxwicik6hPtCV6EvnEeGI6sZBYRqwm7iEeIZ4lXicOE1+TSCQOyZLkTgohJZAySQtJa0jbSC2kU6Q+0hBpnEwm65Btyd7kCLKArCCXkbeQD5BPkvvJw+S3FDrFiOJMCaIkUqSUEko1ZT/lBKWfMkKZoKpRzame1AiqiDqfWkltoHZQL1OHqRM0dZolzZsWQ8ukLaPV0JppZ2n3aC/pdLoJ3YMeRZfQl9Jr6Afp5+mD9HcMDYYNg8dIYigZaxl7GacYtxkvmUymBdOXmchUMNcyG5lnmA+Yb1VYKvYqfBWRyhKVOpVWlX6V56pUVXNVP9V5qgtUq1UPq15WfaZGVbNQ46kJ1Bar1akdVbupNq7OUndSj1DPUV+jvl/9gvpjDbKGhUaghkijVGO3xhmNIRbGMmXxWELWclYD6yxrmE1iW7L57Ex2Bfsbdi97TFNDc6pmrGaRZp3mcc0BDsax4PA52ZxKziHODc57LQMtPy2x1mqtZq1+rTfaetq+2mLtcu0W7eva73VwnUCdLJ31Om0693UJuja6UbqFutt1z+o+02PreekJ9cr1Dund0Uf1bfSj9Rfq79bv0R83MDQINpAZbDE4Y/DMkGPoa5hpuNHwhOGoEctoupHEaKPRSaMnuCbuh2fjNXgXPmasbxxirDTeZdxrPGFiaTLbpMSkxeS+Kc2Ua5pmutG003TMzMgs3KzYrMnsjjnVnGueYb7ZvNv8jYWlRZzFSos2i8eW2pZ8ywWWTZb3rJhWPlZ5VvVW16xJ1lzrLOtt1ldsUBtXmwybOpvLtqitm63Edptt3xTiFI8p0in1U27aMez87ArsmuwG7Tn2YfYl9m32zx3MHBId1jt0O3xydHXMdmxwvOuk4TTDqcSpw+lXZxtnoXOd8zUXpkuQyxKXdpcXU22niqdun3rLleUa7rrStdP1o5u7m9yt2W3U3cw9xX2r+00umxvJXcM970H08PdY4nHM452nm6fC85DnL152Xlle+70eT7OcJp7WMG3I28Rb4L3Le2A6Pj1l+s7pAz7GPgKfep+Hvqa+It89viN+1n6Zfgf8nvs7+sv9j/i/4XnyFvFOBWABwQHlAb2BGoGzA2sDHwSZBKUHNQWNBbsGLww+FUIMCQ1ZH3KTb8AX8hv5YzPcZyya0RXKCJ0VWhv6MMwmTB7WEY6GzwjfEH5vpvlM6cy2CIjgR2yIuB9pGZkX+X0UKSoyqi7qUbRTdHF09yzWrORZ+2e9jvGPqYy5O9tqtnJ2Z6xqbFJsY+ybuIC4qriBeIf4RfGXEnQTJAntieTE2MQ9ieNzAudsmjOc5JpUlnRjruXcorkX5unOy553PFk1WZB8OIWYEpeyP+WDIEJQLxhP5aduTR0T8oSbhU9FvqKNolGxt7hKPJLmnVaV9jjdO31D+miGT0Z1xjMJT1IreZEZkrkj801WRNberM/ZcdktOZSclJyjUg1plrQr1zC3KLdPZisrkw3keeZtyhuTh8r35CP5c/PbFWyFTNGjtFKuUA4WTC+oK3hbGFt4uEi9SFrUM99m/ur5IwuCFny9kLBQuLCz2Lh4WfHgIr9FuxYji1MXdy4xXVK6ZHhp8NJ9y2jLspb9UOJYUlXyannc8o5Sg9KlpUMrglc0lamUycturvRauWMVYZVkVe9ql9VbVn8qF5VfrHCsqK74sEa45uJXTl/VfPV5bdra3kq3yu3rSOuk626s91m/r0q9akHV0IbwDa0b8Y3lG19tSt50oXpq9Y7NtM3KzQM1YTXtW8y2rNvyoTaj9nqdf13LVv2tq7e+2Sba1r/dd3vzDoMdFTve75TsvLUreFdrvUV99W7S7oLdjxpiG7q/5n7duEd3T8Wej3ulewf2Re/ranRvbNyvv7+yCW1SNo0eSDpw5ZuAb9qb7Zp3tXBaKg7CQeXBJ9+mfHvjUOihzsPcw83fmX+39QjrSHkr0jq/dawto22gPaG97+iMo50dXh1Hvrf/fu8x42N1xzWPV56gnSg98fnkgpPjp2Snnp1OPz3Umdx590z8mWtdUV29Z0PPnj8XdO5Mt1/3yfPe549d8Lxw9CL3Ytslt0utPa49R35w/eFIr1tv62X3y+1XPK509E3rO9Hv03/6asDVc9f41y5dn3m978bsG7duJt0cuCW69fh29u0XdwruTNxdeo94r/y+2v3qB/oP6n+0/rFlwG3g+GDAYM/DWQ/vDgmHnv6U/9OH4dJHzEfVI0YjjY+dHx8bDRq98mTOk+GnsqcTz8p+Vv9563Or59/94vtLz1j82PAL+YvPv655qfNy76uprzrHI8cfvM55PfGm/K3O233vuO+638e9H5ko/ED+UPPR+mPHp9BP9z7nfP78L/eE8/sl0p8zAAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAB9nSURBVHja7J15nJxVme+/511q7+o96aSTACFgEoghQfYdQVxABEdlQJkRvbgO6jjKZZwR584V4Qozep0ZB8XlqoCgqAiCCrIJIayBkMQEsm/d6aT32t/l3D/Oqe7qpKtrSW8k9ft86lPdVXXe97znPNt5zvM8R0gpqaGGGqYHrPG4SOTS274NfLTCZqcBa99sA+amUkTb22k75VT8XI5DQaDZ0Sj9WzbT9eKLWKEQCHFYMkPq/i8eGgwJtAB1ldJBTR7WUMNIGON0nUQVbbza8NdQw8QwZA011FBjyBpqqDFkDTXUUGPIGmqoMWQNNdRQY8gaaqgxZA011FBjyBpqqKHGkDXUUGPIQwS1gPwaJgjWm6CPNipO1gbSwMAU9qNB/71Xv7cBZwALgTlAc0FfXSAF9ACdwEZgJbB+gvo3Azhd92We7ktc98UDMgV92Qq8ALw8JGN8/80iaAJAGAgCUj9XEvBrDDkxqAPeDZwDnALM1oQV0AS+D1gDPAH8AVi3X/tlmijLoS4f+K2e1NHwFuBy3Z8FSNmIEATi8YXCMK6TUn5G96sSrAN+DfyqkCGqRBPwAeCvgDOBUIXttwAPAPcZpvUUhnE8sKTMsRsPCD2nvy1yz7yQOQF4GzBfP3NEP6uvhfSgFpKrgVeAF4Fnagx58Pgq8Fmgtcj3cf2aD7xXf/Zz3e4N/f8/aSYqF7OBjlHG5TbgulHtfDvQhBAXVcGMAIv16yuaMW/SBFQJIsD1wOeA+oMY76OA67xc7rpQY+NFdiSyyMtmv2XYk56Ik7co8jgHuBZ4n37WsRDUlstcYHnB568DdwPfA3bX1pDlScc8jtfS7V/GYMZiuALYANxQoIHKhcuBWSdLUHma1xVdQvp+bpwm+TJtOn6H8tPRrtTP+9WDZMYRa2JhGHMwjM4pooU8M56kNfYT+jkjB3HNY4Eb9VLhVqCxxpDlaecTgJc0IxwMc98EfB/4Z+CpMtvtv+44DVilJ3My8Vlthi8t8bv/Au7U69VxFI0CKWUSKaeCHn6q378CPA9cPM7XDwNf1ELsihpDFkfexHy6StNvNHwc+KE2eVaX2cYsMF3/XPD/ZONYLQzOK/L9H4FPcWjh34GrgUeA/z3B92rVJuy/19aQo2MQ+DcgOs7X/SiwXWsbp8QzGkCf/vuxKWTGQk3/GHDifg6fPwHnH2LM+F/A3wPPASdP4n0/r51FV9U05DC26YH5wgRd/0bgrDLNn7SW0m+ZRvPyZIHZ/J1DkBn/DHxGW0cnT8H9rwR+UmPIYcQnwUR5HLUt8qcxfrNJE/7t43hfqbX/wSCmTdcWvb48WEuk3C2gyXCvesDZWtCcMYW0/xHg72omq8JkeLxM1BbI31Hc85oDvkXl+3ej4T9RHsJ1qI3qJm02f1C/KkEfakvkwSr7chdwL/CavlYMOA61jXDtgYayAEi7qVS3m0p1GYGAPMCUltJHiLgVDkcM0yxWbW83MLOE6f8RPS7VCpqUfq5OlJf5eC24qsH/Re0H76qtIScH/wr8h3aIvGOU75dQvXfX0e89qMCB5/b7vgflcr9PM9fdFVz7OtRe6ylV9Ov9msj278t24GHgZ8BDmkmVSnddhGme3bho0X966fSlwrLkfjpfGLYt/Vx2U9+mTVHP97Gj0f2Zcq8WPN8bgyHX6XHYU8VzDaC2eu5EBYfkEdFLk68Bi6q47p3AuTWGHB1bgGeBzajN3mXjYNZciNqHekfFtqfvD72QEnwfN5XEdxxLCBHXPzuD0uFwP9fv5TBlFviNZppKcTHwuzLWb2cXOo2k54FhfKlp0eKPC2H0H2DhSsAQhjCML0faZt3T9dKLZAcGCMTjheF2JvD1EmbvPwDv1E6VStCl15rbimjMe4Ff6LX3WRVe+xxUQMHLNYYcxj7gS8D/G2W98za93ryoymv/T1RoWY4ytlek5+Hlcmqz3DRfs0Lhl8xQcLVhWV2GbQ/6vp8O1Ne/4rvuKdpELTc29efakXFmid/9BLXPeGaFz/lQGcyYxyqtya4dMlmlxE2lGkssJ74Ta59zb7CxUe5d9TLJjg7MQAA9rm+gto6KIalNzd9XMYfnF2HG/cXG27X5WWlwyZeAv64x5DBxvGsMM+ZFLVV/SOXV0dHSL661zuhrOSEUI2YyGIHAU9FZs34cbGxaZ8diBGKxsBkMRoRptgrTPEqYZlS67ke9TMZCiE9X2JdvlME092vCqhQ3V9GXayts05rt7zsp3NL6fMOCYxjcsR3DthFC/BEV0D4WvgfM0mvZSvB9yq9w76DCCe+q8B4fAD7B1CUuTBuG3AmcqrVXKVyDiupZVsV9jkYFMH9wNGb0nRxe1hmMzWl/X8OCBZtCzS3nGpb1ZSnlO6TnxaTnIaVEep56+T4I8aTuf6VarEc7fIphhyaqSrBHm6KVYCsqbO+kShoJwzjNy2WfT+zaifQlQogfaLOvFH5d5dx9v8Lf363bVLK/bWqlcM/hzpAXlsmMeXyY6s4DWYTaAjnQznFdDNNa1XjCcec0LjjmIkzjl1463ehls6Wu6ev1SiUHYWRRXs9iDPkGyuN7ToXP16UFWyURT141GsGwrDOdZPLbyc5OTNu+QQvKcjConV+VoFcLjUrxJ4aTDyoxiw9rhvwelecGrtNrkHdW2O5to5qKUuLlsjQcu/DDLccff5WTSHzXTzvlHjZzHsXD3KrFCs3olYYSLtHOsAmFlD5WNHrB4Nat5Pr63mXHYjdVIIji2plUCRxUHGql+6PtVTzeSUwDTCVDfrfKdj+ogiEXaSJPHmjKCAzLnOHncp/zHWeqT37azMFlOEwkN2IGQ/iOc0vfpo0NwjAq8QLfqU3kSvefZ6A85JOB9ukwzFMZXF7tYTvrqmjTqE3LvaPSmu/bUsrd0+AYtj4qP0VsUphRWBZWKHRv1wsv3Jzctes5K1r2Eq0f+BiVb3VMNmKHO0NWG8jdxfCmfLkI6/f0NCeKTEFfpw8MAzsWW7PvtdUf6n3j9d/asdixFZT7yJup50zzsfcPd4asVh3lqMwRlH9OwfSvu2IwMnN+WmjHYLw+MbB585J9r756ix2NXiLMsmXpJQynwZnTfOzl4c6QB2NaVKpFsnrAg0VEw3Q5MngW1Z21OUG8KAnE4yQ7O2Z2Prfyg2Yg8GXTtssthvVlRsbiOtOcrqaFwJhKp061TDC7CkEyWLCWHM1Y8aroj6c1rhhHgmileMGtUnDGsS8ARrC+3skNDJy2+5mnj5XSv8cORco9wv1HwDf3+6xzmjNk5HBnyO4q251ZRZtN2sxtHikSBNL38Z1cCiGsfAhZmegB3jqOTGDoPs6usv3ZKE/mQfVHSokVCmGFQiLd3d3R8eyKo7x0elMgHlcBEaWxgtH3JrdW2aVVqHjVibRihKaRw5ohj6B0fOJo+HwVbZ4fa40ghLARolIzuAEVITPea48+LayaK2zXOx5ayLRt3HSK7rVrGNi0Kei77oZAXV25zLgdFewxGl6vsktXHETbNx2mcg350yrMhBs1I1eK1aiSh6Pbnq7bge9XWkXOpvoyEMESpucjVVzz6oPti2FZSM+j45ln2PfqqyEM4w07GrXKNFNdVOZLagyLaGUV/XsLhxGmkiHnoXIIy90svgqV81YNXqdIOQ9hGjjJ5CbfcX4kKt+HvKGKvizXlsHDqIDmRUWEVaX4H1W0qUcFIzyNiog5T5hmSAJ2NPqIYVlzZfkm/AWUju39UxV9/EiVc34K02Sz/83CkKCyvdcCHxrjN62oSmE/q/Ie61EBAaPewzAt3HR6lpfJ/KYCd34ei1EVB8rFQlTWyUxUtNF/owIdnge+DbwHFRjwEOWXsiwcp0oqDDSiypzMBs5AylutSPSxdPe+jJdO32xYViVr9U+ichFL4a4q5u8DlC6PuT+O09p4Cypk8gaqC2w/7BgSlKv/56jarN/QWuNjehDv0oP6+YO4/teABRSJ/hemiZdKzckNDmLagTuquP7nUHmRp5dglutRtVfnjvL9SagqAQ9q7Xk7le+1ohn6BcYO4g6h0q7W5YlUSoldV0eme99xnStXfkD63vWGVbZ74V8pvybRuiq15KOoSnzl4CyGjxGw9VjchEpA3oCq5fMh1Lks0w5CjsMBK5FLb7udynPrJgtRzTBFK7flBgdOaz5uycrWE5Y15Ab6ew8ihG6F1jqbUNsi7agsjLcz/uUuS2GVJv4NmrlbtBl3diExSimxIxF8x7l0+6OPrHASib0VOHF+QeX1ghYwXJO3Uvwf4I4i7U/VNFhuzmxWM+4TqEoHfur+L045sR7qNXWu0wQwZhlFw7L99L69uOlUn7CsK6Xn3VXl/U4voSkrwe+0NP99le2XlWOmGZaF7zi3dq589g+5/v6+YH19ucwIw8Wdy3XO+aj6Nf8LVR+nUnxZv55HbaM4KG/30Xo5UKkz63yUd3raHPs1FQzZo03lhgm+T5cm6JdKDkIoRKZ7H5mebqKz2+92BgcvQoi/mcJ5SaH28rpQ5SW+OVE3MgNBUns6fpjcs+cLdiwWqtBiWlrFLR/Wjq2/0mvwanAy41PP9S+6H4f1GtJH1bnxJvg+S/VadHnpUTCQnsfgjh2AACH+tsq1znjhDM2MoNKPfjRhd5ISYYi5ZiDgTtL5kMuAf6TyUh7jjTTTMOB9KhiyRTsB/n4C7/Febc7cUS5RWuEIg9u2kdrTiR2NgpQXoOrbTLb1cBLqjMNCXIPywk6UK4FJEJCF+Lp2tjRPEd0n9Zpzb40hFdpRRWp7JuDaH9dOnM0VDYRl4bsOfRvfUEnKhgGqsPBNkzQmj6FC8YqdFfl54NMcOvgdw0WOJ5MxXkfVZlo9HQdlqhgyHzVz7jhf95Oo6nQ9qJIRFVhuEjsaJblrF6k9e7AjkXxc61e0afPCBI1FTq8T8yUMx8J3tfPi9+PcB2uKHBtPamvm2Al4ptFwtzaVN05XKTVVDJnfY3tNE/vBJg6vR8VQ3q5fVR1VIEwL6br0b9LzNbz98ZR2IlxB5dXdiqFTm25HU1mZig2oCmkXaUvg4InAtjyEmKpc0R+iNv/fpQVqxwTcY52+x5VMt3zTCWLIuRX+vtBN/pQ2XX5bxX03o9zgi1Cbx/k1KlX1R0qsaJTErp0kOnZjhQ+IN78HtY93Guq050co/2AdCbyqBcZVwJGos0d2Vjnmf9Ta5TiUk+xBRpbYL4exfywM45rcQOIp33EumMISJh/W77drAfUFqot73R+P6PX3ccAv3wx2/Hhte9xOGdsLBdg4CmNdqon9Sv2+kANTbny9BnhWa4dfj3LtO6i8VOTGYS1p4ucckrt2EZvVTpGUrJUFBFOvTfB2LZiaUBEiPipzo1ObotsrZT7peiAEwjRKSf91wC2oxO35ui/ztHAKamEwUNCXHcBWYRh4uRz9mzeD798pDOPVKaLDwnNR0qhwxG+hPOXv1NbJW7UQK0azaS1k1qE2/B+lVJaIEDBtctN1l8YpUmci+jYHFXKWj3BJoLYCdk/ogAhBpqeX+Ny5zDr7HNxsBvxJtuakRPo+RigIUuKnswjTBFOM60pPmCbS9+l45mly/QOYoeB0VyBtKM9sFBUC6KMSuge0dVDaSSh9sIKISJP6Ww7PbfKuvz1kNGQ1VAdmUA2IN2r5xZ2VaRRxcH4JIZCuS7avH2FbRI8+Ail9pOMiLGPyXB5SInMuZkscq7UB6Xn4/Snc7gFw5bj3RQihmH1aBKuUnMNOqsn5lFJtLwfjiHADMt2L+/qjeF3rwS0MGT5UGFIYxZnuAA0sQYKItSIzA2AFwLDASan3UdsUMTfMACIQRZgBpO8iU93ge2P0p1j3Bbm+ARDQuPR4mk9eTqi9Dae7D5FOI7MuImCNSj4500AKge15GHIscpKAgedF8bzw0PLdMLIYRhrDcEB6w8w4owHpKulttdYjbBNndzfSkQjbRPjgC+iPhkjbFiHHJeh6BFwPQ0p1NwmiyFgK4SNlECfbgJMNI4RXHmGPdHaNGyOKcAOYtpo7L4fM9GtBfZBuDumDFUJEmpB9O3HfeAx3y9P4/dPmSMjxZ0gRbhh9koQJnoNM9wxTqmFgNM7F3fQkzur7EPXtBM/9IsKcpSbBsED6yOQ+LdnE8MAKAxFtRpg20nOR6V68rvV4O15C2GHs5X+NTPWAky5vIrVAzuzZR/SoebS94zyi89qRno/TP4ARDmLPm6EYIZVFBCyEEPhAbyxEd12EpkQa0/PoidVRl8nR1pfEFwIpChlRkMupsqTh8BaCwV3Ydj++b5HNziGbbcPJNYGfJjTDxWxtUOtHXw2an8lhxCPYpoHYsY/OgE1PSz12zmHpti5m9ifYG4/SHQvTFw2RtUykgIxtEc06tPWpullSDGugbLYN03Zobn2Gvmg3fTubCLXlMAwfKcVIwSclzmAC6XnKzHU9rGgEMxiguiWPEroiFEcE4yB93F2r8LY+i4g0YrQdh9l6LBgmMrF3eK0nfSWEww1g7Jcq52YV/ahzV7THwUPEWsEO4ay6B2f978HNMp0xLmtIa/YSMOwR9jiACNVhzT8bc9YSZC4J0kfUzcT9y0NkVwxn7JizT8Bono/fvRERiGK2L8OafxbSSSGTqvSOiLYg7BDulmfwOtYg0734fbuUVtSwF7+HwCnXIJM9yHTv2It2IfBSKXI9fcQXv4UjrrgMKxYju697eFIliIAJEvyOHnKDKTa3tyAELN3WxQWvbeOC17YSzWVY2z6D7164nFeOmU1z7yCtgyl8AY7bjOtGqa9fSXPzo0QiG7GsPoRwAYHrxsnlZuBmwiT8s0hEL8eQA6OuW0XAJoFk/ssbOX3tVhbv6uXMDbtoSqToi4bZG4/QFY+QCAXwBfTGgtx55nE8ddwRxJIZZvcN4rl1ZN0W6mKraZtxF02tz5Hsa+PlB0+na107BB3C9SmEoabTGRjEdxzqjjmahuMXYobDpHbtpnfVazh9A9j1cYxgoMxaRAWMGGnC796Cu/kp/N7teB2vjfilOWsJ9vIrMVsW4PfvBgFGbCYyO4C38yVkNjFCshr17RhtixGGjZ/cC14Oo2Ee/kAHued/hLf71TIMAHloMGSpTHtr/tnYi98DZgBn3QO4bzxWmsmPOh178SUYrccCEn/v6zhrH8DdOvYRFtaC8wgsfT8iOgOZ7lFmcaG2FAIvmcJNJgm2tigT9dS3qcoBA4MIwziAhoRl4ARt/H39nP/Ea1y4ZhtnvL6LWX292sgQQJbuukZuf/tSfnD+8WxumUn7jiB1gc00z3iApqY/YRgZHKcZ3w8OqWchXEwzTcjcSI93BTuSt2CLLkYrISuFYMu8Vu74l7u46sH7gVaSwQgZ2yLguoQcD9tztRaUgEsiGONnZy3mvy9cyvrWI2nv7WNG8/20tNyPaSTJZGYTjmcRhmTH6iPY/MICerbNBr8by+wmMu8omk5cSnzRsZjhENLzEYZBancHvS+vpm/NetyBQez6OoxgsDhjSh8RqkeEG/D7d+Fuehxnw6PKmhljKRRYfgXWwneDm8Hd9hzexsfx9o2+r2+2n4B15OmYs9+KiLbgbnmG3Mrv78e8NYbUpqqFMCykW0GVQ8PEXvQeQOKse7DsinAiGMNacB7W0ediNM5FpnqVZE2lcRIpwjNbaThhCQ1LFhFsbcHpH8DLZA5kxgL0xqO09wyw4sO3Ybs9JEItDIQVAQrAMwyaE2nCuUE64jP5zjtP5P7L9zAn/gBhs5Ncrg3fDxbdf7dFB/3eJXQ612OJnqIMuaOtkdtuvY9rf/04HQ1jb7l6hiCezhFP95Oz2rj18nbuu/Zp5niv4WZb8bwYQnhIX2AGPKKNCbKJIDtemcHODScTWHA2TScciRUOk+vvRzrDe+pWXQwzFCS1czd9r66jb/UanIFBrTHzjJnXiPWISCN+7w7cjY/hbnxCWUzlkkHLAvBy+L3by/t9fTsi3oa346WK6Hg6MOTkeVl9F+lXGCThezhrK48XkNmE0qZvPIZ1zPlY88/CI4IZFrSccQoNSxYTmtGCM5gg26XWKGMxI4Dh+wjXY2dzHTO7JYOhwAiHien79EaC9ERDtPfm+Mc/Psjaa9YxEIwiBo5ACH/cgmH8MvfPTF+SDNoMhltp75Fcvu5JHoxsJdU9D8sXQ44cYUh812Cgqx7Ldll46jPUn3gue/wT8VOdZJMpZcIXCF53MIGbUFbG7IsvpOGti+lbvZa+tevJdu3FjtdhNrQiQg343VuGLCOZS1X+vPsqi3Tz+3fBNHXaTONtj0nYQcglcdY+gLPhERouu4F5n/oHLMvAGUyQ2bP3ACIbc5INge24hDIOfhGHkVAmB0mCdNQ14adm4lvlMaLAxyeEHIopGD8YvgQsusKtkO1FUMzzKvFck2RfnKwUOA4EhFvEYac+cwcGcQcGCbY2M/vii2hctoSeF1cxsGk3/mAv3ho1/rgZaihjrsbpOitQZf7WoPII9+j/rxyjzWmo4OLCc/k+hSqBMdZ5gHWoMLHVqCyBffp1TNEWboaAs51Qe4TMnr146cxoRDYflXHxhr7eKuCy/JehrENncz0dM+KE3UyBF3Xkc0gBYQwG6jz66z0sd8R93o2KJEmgokmGnt0ngC06McWAZsoh/AC4t/CDYM7df7/uD3q8X91v/C/O9wkEyahHJuwjhjtv6Ge+fuSy2UD4qdH2BK/R8xMZwZhC4A4myXbtJdDUyLyr3k/bucvJPPxVnLUPlGLG2boP6wvm8y8laOB8VOmN3fo5nx5z/oef9Wd6jHpQ0VlXHKoM+SwqtrJJD9YKVBzhWFE1R6JC5AordZ+MygAZq/xbCFXMaQ6qOtvDmiDHXJQYjXNxBsbcQ2tFHcCar4saB36Fiqskmsmxo62Rn15yCqb0tNYZ8RzzlDZSmvLh93TT3ewQzA7d7yOolCNfX/dUVCmKtwB4somo8TwRYxWuHHHA8odQgdEgJa5lkAnagIs/bGa/oMf/CD3+z+hn6AJQCtpgzZIEmaCPMayA36+f+eb9jF1s0YEhMsiRU5Gfn8Do5oFQpmwGLJEEWdYWQ4Pug6Xn8/ea2cZa0C1FJSW8qp/7FFToZkuJTa6rtOB9SNPL3ZQo7/JmNVnz1YE+iUoRuqyMNnnXV2Hkxc6Czbuivgr9/nWgrJg9YVrET78Yb2DM817yC9zP60lGE8a/Ad/1hcjEk2lWLZxDd6SeUM4lHbQLnyMJEEobdNTnWLU8Qd2gldekdcBPUPGvp+nfX42qZbpBUUsWR7bi+LMwRia/bEVnrwigLpHhlxcs44qHXqQumSYZDoIKUgdVYe9G4PJhx45kTk+IV47t5TeX7WXmnhHhcTfo6x+JClP5MYAvg1iiC5OEJpGhoIF9Qwq9uIcPP53FiLaWSztugXVUboHo/P3fpd8/U2AR/LhE228wnOMqUVXzHpsuDDne6Vcz9Xs5uYh57nhUD2Y3KnNjH6XjuNKobIttqGDpm8dcS3ou3mAvwhrzZOz8PecUfPZ1/X46gO16+IYgHbIxvdG7aLmCVMwjF/QJOEPaMV/46jr9HtUS/iVtVSCxMEhiiS7kGCeaz+hJ8Oe3LeAn7z2V+IHu/HwG/tDho7GUSW/U5ZvXb2WwziGaHNJ4R6PKaRyFShb/2jC1R7DZjc1u/CrOoHH79hE9YRn1Z19eCXP9AhWAvwuVZD4W8qr3ElTQ+Wf1/+vLoLnl+rnznXuSaYSJcuqICoTB8wwn5p5EedWmTVSpxRWakcbM/haWjRlrQLpuJQQCw8fe5Yrz7yj3k+o1CgHFChjne6jE3F8AH5QEscVObLENOWapF4mQkr8c3QYYGFJqr+vo49+QCfDQuV28snyAudvDeOZQx75QYLa2anO3EeiVBLHEXmyxnbT/VkzRW8xKKSYB8TM+M6/+J/qf+lW5tLJW+yAkpQ++yae8FbrgP0HplK1O/bzv1/8/AvzzobiGPBhh8Gltvl2tzbq6Mhg6gEqz+oQ2k8cu2ygEfjZN4eJpDBTmFN6i31cKIBewsD2fWDqLWyQlyrElsUGTcMokG/QL19h5Bw1aEyzRZm5jXg5IbORQEkMRWhcCy/OJpLOUmzq0dX56KLa1ABfr+/8HqqQFqKLPgMAgiy32IEcGfJuF5nnx8TZwOndTd+Iymt790XJp8CsF8/l4iTaxAssjXx2wnKPu21B1ks7QWvIdTG4toUlnyBn7TVw5g7qk4LPjy9Cw+Wt/S3tCV+vX8UWJ2MnR9+R9WPWBUkyOXui/orlioXaquEhJIhzizJc20ZDuJxu0C72+Q8+TCfnMHAxy0vNxElFvKHRUT/5R2mzKWwVRSlcoX8AoR9SJ0RX0rMJ5NXyBh8+u9iymN2Kz42NaI35It1msvcs3og/fkZgE2YQhsoXTaRYQ/yt63L9fzNDwMtB40dXlCuY/FMznC4xdJjRf6Pk5LcSfQ1VyWFxCE9tamKzgwEJih6TJ+rC+ZjklOdZos+0vBZ/9RjtJxrItU3rNM7uAAEubUb5famW6U/enXRPDXZrpXwRIRELM37mXKx5+EcdQsaKaKUY8hzTAQ3L+o4088L69ai2ZM/Lm0cnaAXMEqgp3iNLnZH6D8gsR/1JfLwtgO4L+kEvnrCyhzAjZG9GM9LuCz/5GO0cagU5fhrGNnZh+Dz4BDHWO7EOohOyFJbWlMJA5yskl7dLj11awBs6W8CM8qdvM0Gbou7SmXzCGpvR1m6eYxhDTIVyohhpqUPj/AwCXmRmyodlf1wAAAABJRU5ErkJggg==';
            var imgData2 = 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAABB8AAAA1CAIAAACGItS8AAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6RjU5Q0VBRDQwNzkwMTFFQUFGMUU5NDFGNTQ1N0UwMUQiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6RjU5Q0VBRDUwNzkwMTFFQUFGMUU5NDFGNTQ1N0UwMUQiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDpGNTdCOTc5QTA3OTAxMUVBQUYxRTk0MUY1NDU3RTAxRCIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDpGNTlDRUFEMzA3OTAxMUVBQUYxRTk0MUY1NDU3RTAxRCIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/Pmhp3i0AAAyHSURBVHja7N0NbFX1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';
            // Choose the element that our invoice is rendered in.
            var element = document.getElementById("CallbackPanel");
            // Choose the element and save the PDF for our user.
            //html2pdf(element, {
            //    margin: 0.5,
            //    filename: "my.pdf",
            //    image: { type: 'jpeg', quality: 1 },
            //    html2canvas: { dpi: 72, letterRendering: true },
            //    jsPDF: { unit: 'mm', format: 'a4', orientation: 'landscape' }
            //});
            var opt = {
                margin: [20, 10, 16, 10],
                enableLinks: true,
                filename: 'myfile.pdf',
                image: { type: 'jpeg', quality: 0.98 },
                html2canvas: { scale: 1 },
                jsPDF: { unit: 'mm', format: 'a4', orientation: 'landscape' }
            };

            // New Promise-based usage:
            html2pdf().from(element).set(opt).toPdf().get('pdf').then(function (pdf) {
                var totalPages = pdf.internal.getNumberOfPages();

                for (i = 1; i <= totalPages; i++) {
                    pdf.setPage(i);
                    //if (i == 1) {
                    //    pdf.addImage(imgDataLogo, "PNG", 10, 5, 60, 20);
                    //}
                    if (i > 1 && seePercentage == false) {
                        pdf.addImage(imgData, "PNG", 10, 5, 278, 10);
                    }
                    if (i > 1 && seePercentage == true) {
                        pdf.addImage(imgData2, "PNG", 10, 5, 278, 10);
                    }
                    
                    //pdf.text(50, 50, 'new text here');
                }
                window.open(pdf.output('bloburl'), '_blank');
                pdf.autoPrint();
            }).then(function () {
                $('#TreeList1_D .dxtl__Collapse, #TreeList_D .dxtl__Collapse').css({ 'background': 'transparent', 'border-radius': '0' });
                $('#mainHeader').hide();
            });

            //$('#TreeList1_D .dxtl__Collapse').css({ 'background': 'transparent', 'border-radius': '0' });
        }
    </script>
    <style type="text/css">
        #mainHeader {
            display:none;
        }

        .focus  
        {  
                background-color: Black;  
        }  
  
        .focus .dxtl  
        {  
                border: 10px solid yellow !important;  
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
            right: 6px;
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
            right: -2px;
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

        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/

    </style>
    <%--end 3rd level popup--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading">
        <div id="" class="panel-title">
            <h3>
                <span id="">Balance Sheet Statement</span>
            </h3>
        </div>

    </div>
    <%--Rev 1.0: "outer-div-main" class add: --%>
    <div class="outer-div-main">
        <div class="form_main">
        <div class="">
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
                                    <asp:RadioButton ID="radAsDate" runat="server" Checked="True" GroupName="a1" /><span class="ml-3">As On Date</span>
                                </td>
                                <td>
                                    <asp:RadioButton ID="radPeriod" runat="server" GroupName="a1" /><span class="ml-3">Period</span>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td id="dvFromdate">
                        <%--Rev 1.0: "for-cust-icon" class add--%>
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
                        <%--Rev 1.0: "for-cust-icon" class add--%>
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
                        <div class="simple-select">
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
                                        <dxe:ASPxGridLookup ID="lookup_branch" SelectionMode="Multiple"  runat="server" ClientInstanceName="gridbranchLookup"
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
                        <div class="simple-select">
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
                    <td style="width: 230px">

                        <div class="" style="color: #b5285f; font-weight: bold;">
                            <div style="padding-right: 10px; vertical-align: middle; padding-top: 0px" class="dis-flex">
                                <asp:CheckBox ID="chkZero" runat="server" Checked="false" Text="Show Zero Balance Account" />
                            </div>
                        </div>
                    </td>
                    <td>

                        <div class="" style="color: #b5285f; font-weight: bold;">
                            <div style="padding-right: 10px; vertical-align: middle; padding-top: 0px" class="dis-flex">
                                <asp:CheckBox ID="chkExpandAll" onclick="calc();" runat="server" Checked="false" Text="Expand All" />
                            </div>
                        </div>
                    </td>
                    <td>

                        <div class="" style="color: #b5285f; font-weight: bold;">
                            <div style="padding-right: 10px; vertical-align: middle; padding-top: 0px" class="dis-flex">
                                <asp:CheckBox ID="chkPercentage" onclick="calcPercentage();" runat="server" Checked="false" Text="Show Percentage" />
                            </div>
                        </div>
                    </td>
                    <td>

                        <div class="" style="color: #b5285f; font-weight: bold;">
                            <div style="padding-right: 10px; vertical-align: middle; padding-top: 0px" class="dis-flex">
                                <asp:CheckBox ID="chkClosingStock"  runat="server" Checked="true" Text="Consider Closing Stock" />
                            </div>
                        </div>
                    </td>
                   <td>

                        <div class="" style="color: #b5285f; font-weight: bold;">
                            <div style="padding-right: 10px; vertical-align: middle; padding-top: 0px" class="dis-flex">
                                <asp:CheckBox ID="chkConsiderOverhead"  runat="server" Checked="true" Text="Consider Overhead cost" />
                            </div>
                        </div>
                    </td>
                    <td colspan="2">

                        <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                       <%--   <button id="" class="btn btn-info " type="button" onclick="generatePDF()">Download PDF</button>
                      <asp:Button runat="server" CssClass="btn btn-primary "  OnClientClick="PrintType('Excel')" Text="Export To Excel"  OnClick="Unnamed_Click" ID="btnPrint" />
                        <asp:Button runat="server" CssClass="btn btn-primary"  OnClientClick="PrintType('Pdf')"   Text ="Export To PDF"   OnClick="Unnamed_Click" ID="Button4" />
                        <asp:Button runat="server" CssClass="btn btn-primary"  OnClientClick="PrintType('Csv')"   Text ="Export To CSV"   OnClick ="Unnamed_Click" ID="Button5" />--%>
                        <asp:Button runat="server" CssClass="btn btn-info" OnClientClick="PrintType('')"      Text="Print"            OnClick="Button6_Click" ID="Button6" />
<%--                        <dxe:ASPxButton ID="btnSaveRecords" OnClick="btnSaveRecords_Click" UseSubmitBehavior="false"  ClientInstanceName="cbtnSaveRecords" runat="server" AutoPostBack="True"  Text="Print" CssClass="btn btn-primary" >                           
                        </dxe:ASPxButton>--%>
                    <asp:HiddenField runat="server" ID="ExportTypeName" />
                    </td>
                    

                    <td></td>

                        <td></td>
                        <td></td>
                </tr>
            </table>
        </div>
    </div>
    

        <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="Component_Callback">
        <panelcollection>
            <dxe:PanelContent runat="server">
                <div class="text-center" id="mainHeader">
                    <img src="../../assests/images/logo.png" />
                    <h1 id="compH1">   </h1>
                    <p></p>
                </div>
    <div class="row no-gutters" id="printableArea">

        <div class="col-md-6" style="width:50%; float:left">
            <p class="text-center" style="margin: 0;
                        background: #efefef;
                        padding: 5px;
                        font-size: 14px;
                        border: 1px solid #ccc;
                        border-bottom: none;
                        font-weight: bold;
                        margin-right: 2px;">L I A B I L I T I E S</p>
            <dxeTreeList:ASPxTreeList ID="TreeList1" runat="server"  SettingsBehavior-AllowSort="false" AutoGenerateColumns="False" ClientInstanceName="treeList1" OnDataBinding="ASPxTreeList1_DataBinding"
                OnHtmlRowPrepared="ASPxTreeList1_HtmlRowPrepared"
                Width="100%" KeyFieldName="ID" ParentFieldName="ParentID">
                <columns>
            <dxeTreeList:TreeListDataColumn FieldName="Group"  Width="70%" Caption="Particulars" />
            <dxeTreeList:TreeListDataColumn FieldName="LedgerAmount"  Width="30%" Caption=" " DisplayFormat="#####,##,##,###0.00;(#####,##,##,###0.00)" />
            <dxeTreeList:TreeListDataColumn FieldName="AMOUNT"  Width="30%" Caption="Amount" DisplayFormat="#####,##,##,###0.00;(#####,##,##,###0.00)" />
            <dxeTreeList:TreeListDataColumn FieldName="Percentage"  Width="30%" Caption="Percentage" DisplayFormat="#####,##,##,###0.00;(#####,##,##,###0.00)" />

        </columns>
                <ClientSideEvents NodeClick="OnRowClick"></ClientSideEvents>
                <settingsbehavior autoexpandallnodes="true" />
                <settings gridlines="Both" suppressoutergridlines="true" />
                <settingsbehavior autoexpandallnodes="True" expandcollapseaction="NodeDblClick" />
                <border borderstyle="Solid" />
            </dxeTreeList:ASPxTreeList>
        </div>
        <div class="col-md-6 no-" style="width:50%; float:right">
            <p class="text-center" style="margin: 0;
                    background: #efefef;
                    padding: 5px;
                    font-size: 14px;
                    border: 1px solid #ccc;
                    border-bottom: none;
                    font-weight: bold;
                    margin-left: 2px;">A S S E T S</p>
            <dxeTreeList:ASPxTreeList ID="TreeList"  runat="server"  SettingsBehavior-AllowSort="false" AutoGenerateColumns="False" ClientInstanceName="treeList" OnDataBinding="ASPxTreeList1_DataBinding"
                OnHtmlRowPrepared="ASPxTreeList1_HtmlRowPrepared"
                Width="100%" KeyFieldName="ID" ParentFieldName="ParentID">
                <columns>
            <dxeTreeList:TreeListDataColumn FieldName="Group"  Width="70%" Caption="Particulars" />
            <dxeTreeList:TreeListDataColumn FieldName="LedgerAmount"  Width="30%" Caption=" " DisplayFormat="#####,##,##,###0.00;(#####,##,##,###0.00)" />
            <dxeTreeList:TreeListDataColumn FieldName="AMOUNT"  Width="30%" Caption="Amount" DisplayFormat="#####,##,##,###0.00;(#####,##,##,###0.00)" />
            <dxeTreeList:TreeListDataColumn FieldName="Percentage"  Width="30%" Caption="Percentage" DisplayFormat="#####,##,##,###0.00;(#####,##,##,###0.00)" />

        </columns>
                <ClientSideEvents NodeClick="OnRowClick"></ClientSideEvents>
                <settingsbehavior autoexpandallnodes="true" />
                <settings gridlines="Both" suppressoutergridlines="true" />
                <settingsbehavior autoexpandallnodes="True" expandcollapseaction="NodeDblClick" />
                <border borderstyle="Solid" />
                <Styles>  
                    <FocusedNode CssClass="focus">  
                    </FocusedNode>  
                </Styles>
            </dxeTreeList:ASPxTreeList>
        </div>



    </div>

                            </dxe:PanelContent>
        </panelcollection>
        <clientsideevents endcallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>
    </div>
    <dxeTreeList:ASPxTreeListExporter ID="ExpExporter" runat="server" TreeListID="TreeList" />
    <dxeTreeList:ASPxTreeListExporter ID="IncomeExporter" runat="server" TreeListID="TreeList1" />

    <div class="hide">
        <asp:HiddenField runat="server" ID="hfIsProfnlossDetails" />
    </div>





 <dxe:ASPxPopupControl ID="popup" runat="server" ClientInstanceName="cpopup2ndLevel"  CloseOnEscape="true"
        Width="1200px" Height="600px" ScrollBars="Vertical" HeaderText="Balance Sheet Statement - Detailed" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True" ClientSideEvents-Closing="ClosePopUpstkDetLev2"
        ContentStyle-CssClass="pad">
        <contentstyle verticalalign="Top" cssclass="pad">
            </contentstyle>
        <contentcollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel2" ClientInstanceName="cCallbackPanelDetail" OnCallback="cCallbackPanelDetail_Callback">
                    <panelcollection>
                            <dxe:PanelContent runat="server">
                            <div class="pull-right" style="padding-top: 26px;">
                                
<%--                                <asp:DropDownList ID="ddlExport3" OnChange="if(!AvailableExportOption()){return false;}" runat="server" CssClass="btn btn-sm btn-primary" AutoPostBack="true"  OnSelectedIndexChanged="ddlExport3_SelectedIndexChanged">
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLSX</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                                </asp:DropDownList>--%>


                                 <asp:Button ID="Button3"  CssClass="btn btn-sm btn-primary"  Text="Export To Excel" runat="server" OnClick="Button3_Click" />


                            </div>
                    <div class="clearfix">
                        <dxe:ASPxGridView runat="server" ID="ShowGridDetails2Level"  ClientInstanceName="cShowGridDetails2Level" KeyFieldName="SEQ" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                            OnDataBinding="ShowGridDetails2Level_DataBinding" KeyboardSupport="true" Settings-HorizontalScrollBarMode="Visible"
                            OnSummaryDisplayText="ShowGridDetails2Level_SummaryDisplayText"   >
                            <ClientSideEvents EndCallback="ClosePopUpstkDetLev2"  />
                            <Columns>
                                <%--OnCustomCallback="ShowGridDetails2Level_CustomCallback"--%>

                                <dxe:GridViewDataTextColumn FieldName="BRANCH_DESC" Caption="Unit" Width="30%" VisibleIndex="1" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable"/>
                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="DOC_NO" Caption="Document No." Width="15%" HeaderStyle-CssClass="colDisable" >
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
                                <dxe:GridViewDataTextColumn FieldName="DOC_TYPE" Caption="Document Type" Width="10%" VisibleIndex="3" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable"/>
                                 <dxe:GridViewDataDateColumn FieldName="DOC_DATE" Caption="Document Date" Width="10%" VisibleIndex="4" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                      <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"></PropertiesDateEdit> 
                                 </dxe:GridViewDataDateColumn>

                                <dxe:GridViewDataTextColumn FieldName="PARTY_NAME" Caption="Party" Width="15%" VisibleIndex="5" HeaderStyle-CssClass="colDisable" />

<%--                                <dxe:GridViewDataTextColumn FieldName="OP_DR_AMT" Caption="Opening (Dr.)" Width="8%" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)" Settings-AllowAutoFilter="False">
                                </dxe:GridViewDataTextColumn>
                           
                                <dxe:GridViewDataTextColumn FieldName="OP_CR_AMT" Caption="Opening (Cr.)" Width="8%" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)" Settings-AllowAutoFilter="False">
                                </dxe:GridViewDataTextColumn>--%>

                                <dxe:GridViewDataTextColumn FieldName="OPENING" Caption="Opening" Width="10%" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                </dxe:GridViewDataTextColumn>
                                
                                <dxe:GridViewDataTextColumn FieldName="PR_DR_AMT" Caption="Period (Dr.)" Width="10%" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PR_CR_AMT" Caption="Period (Cr.)" Width="10%" VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
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

                                <dxe:GridViewDataTextColumn FieldName="CLOSING" Caption="Closing" Width="10%" VisibleIndex="12" HeaderStyle-CssClass="colDisable" >
                                    <PropertiesTextEdit DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" ColumnResizeMode="Control" />
                            <Settings ShowFooter="true" ShowGroupPanel="true"  />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" AllowSort="false" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                            </SettingsPager>
                            <TotalSummary>
                                 <dxe:ASPxSummaryItem FieldName="OP_DR_AMT" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="OP_CR_AMT" SummaryType="Sum"/>
                                <%--Rev 2.0--%>
                                 <%--<dxe:ASPxSummaryItem FieldName="PR_DR_AMT" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="PR_CR_AMT" SummaryType="Sum"/>--%>
                                <dxe:ASPxSummaryItem FieldName="PR_DR_AMT" SummaryType="Sum" DisplayFormat="#####,##,##,###0.00;(#####,##,##,###0.00)"/>
                                <dxe:ASPxSummaryItem FieldName="PR_CR_AMT" SummaryType="Sum" DisplayFormat="#####,##,##,###0.00;(#####,##,##,###0.00)"/>
                                <%--End of Rev 2.0--%>
                                 <dxe:ASPxSummaryItem FieldName="CL_DR_AMT" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="CL_CR_AMT" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="NETT" SummaryType="Sum"/>
                                 <%--Rev 2.0--%>
                                 <%--<dxe:ASPxSummaryItem FieldName="CLOSING" SummaryType="Sum"/>--%>
                                 <%--<dxe:ASPxSummaryItem FieldName="OPENING" SummaryType="Sum"/>--%>
                                 <dxe:ASPxSummaryItem FieldName="CLOSING" SummaryType="Custom"/>
                                 <dxe:ASPxSummaryItem FieldName="OPENING" SummaryType="Sum" DisplayFormat="#####,##,##,###0.00;(#####,##,##,###0.00)"/>
                                <%--End of Rev 2.0--%>
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

        <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"  CloseOnEscape="true"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupdocument" Height="500px"
        Width="1200px" HeaderText="Details" Modal="true" AllowResize="true">
        <contentcollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
        </contentcollection>
    </dxe:ASPxPopupControl>
       <dxe:ASPxPopupControl ID="pop1" runat="server" ClientInstanceName="cpopup1ndLevel"  CloseOnEscape="true"
        Width="1200px" Height="600px" ScrollBars="Vertical" HeaderText="Balance Sheet Statement - Detailed" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" ClientSideEvents-Closing="ClosePopUpstkDetLev1"
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
                                
<%--                                <asp:DropDownList ID="ddlExport4" runat="server" OnChange="if(!AvailableExportOption()){return false;}" CssClass="btn btn-sm btn-primary" AutoPostBack="true"  OnSelectedIndexChanged="ddlExport4_SelectedIndexChanged" >
                                     <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                    <asp:ListItem Value="2">XLSX</asp:ListItem>
                                    <asp:ListItem Value="3">RTF</asp:ListItem>
                                    <asp:ListItem Value="4">CSV</asp:ListItem>
                                </asp:DropDownList>--%>
                                 <asp:Button ID="FirstImport"  CssClass="btn btn-sm btn-primary"  Text="Export To Excel" runat="server" OnClick="FirstImport_Click" />
                            </div>

                               
                       
                    <div class="clearfix" >
                        <%--Rev 2.0--%>
                        <%--OnSummaryDisplayText="ShowGridDetails3Level_SummaryDisplayText"--%>
                        <%--End of Rev 2.0--%>
                        <dxe:ASPxGridView runat="server" ID="ShowGridDetails1Level"  ClientInstanceName="cShowGridDetails1Level" KeyFieldName="Entity_Ids" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                            OnDataBinding="ShowGridDetails1Level_DataBinding" KeyboardSupport="true" Settings-HorizontalScrollBarMode="Auto"
                            OnSummaryDisplayText="ShowGridDetails3Level_SummaryDisplayText"   >
                            <ClientSideEvents EndCallback="ClosePopUpstkDetLev1"  />
                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="group_name" Caption="Group Name" Width="15%" VisibleIndex="2" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;(#####,##,##,###0.00)">
                                </dxe:GridViewDataTextColumn>
                           
                                <%--OnCustomCallback="ShowGridDetails2Level_CustomCallback"--%>
                                <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="PARTY_NAME" Caption="PARTY NAME" Width="15%" >
                                <CellStyle HorizontalAlign="Left" >
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                                <%--Rev 2.0--%>
                                <%--<DataItemTemplate>
                                      <a href="javascript:void(0)" onclick="OnGetRowValuesLvl2('<%#Eval("Entity_Ids") %>')" class="pad">
                                        <%#Eval("PARTY_NAME")%>
                                      </a> 
                                </DataItemTemplate>--%>
                                <DataItemTemplate>
                                    <a href="javascript:void(0)" onclick="OnGetRowValuesLvl2('<%#Eval("Entity_Ids") %>','<%#Eval("LEDGER_TYPE") %>')" class="pad">
                                        <%#Eval("PARTY_NAME")%>
                                      </a> 
                                </DataItemTemplate>
                                <%--End of Rev 2.0--%>
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
        Width="1200px" Height="600px" ScrollBars="Vertical" HeaderText="Balance Sheet Statement - Detailed" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
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
                                
<%--                                <asp:DropDownList ID="ddlExport5" OnChange="if(!AvailableExportOption()){return false;}" runat="server" CssClass="btn btn-sm btn-primary" AutoPostBack="true"  OnSelectedIndexChanged="ddlExport5_SelectedIndexChanged" >
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
                        <%--Rev 2.0--%>
                        <%--OnSummaryDisplayText="ShowGridDetails2Level_SummaryDisplayText"--%>
                        <%--End of Rev 2.0--%>
                        <dxe:ASPxGridView runat="server" ID="gvStockSummary" ClientInstanceName="cgvStockSummary" KeyFieldName="SLNO" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                            OnDataBinding="gvStockSummary_DataBinding"  Settings-HorizontalScrollBarMode="Auto" 
                            OnSummaryDisplayText="gvStockSummary_SummaryDisplayText" >
                             <ClientSideEvents EndCallback="ClosePopUpstkSum"  />
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
                            <Settings ShowFooter="true" ShowGroupPanel="true"  />
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
        Width="1200px" Height="600px" ScrollBars="Vertical" HeaderText="Balance Sheet Statement - Detailed" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" ClientSideEvents-Closing="ClosePopUpstkDet"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad" >
        <contentstyle verticalalign="Top" cssclass="pad">
            </contentstyle>
        <contentcollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="cpStockDetails" ClientInstanceName="ccpStockDetails" OnCallback="cclStockDetails_Callback">
                    <panelcollection>
                            <dxe:PanelContent runat="server">
<div class="pull-right" style="padding-top: 26px;">
                                
<%--                                <asp:DropDownList ID="ddlExport6" runat="server" CssClass="btn btn-sm btn-primary" AutoPostBack="true"  OnSelectedIndexChanged="ddlExport6_SelectedIndexChanged" OnChange="if(!AvailableExportOption()){return false;}">
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLSX</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                                </asp:DropDownList>--%>

                                 <asp:Button ID="Button2"  CssClass="btn btn-sm btn-primary"  Text="Export To Excel" runat="server" OnClick="Button2_Click" />


                            </div>


                    <div class="col-md-12">
                        <div class="row clearfix">
                            
                         
                        </div>
                    </div>
                    <div class="clearfix" onkeypress="OnWaitingGridKeyPress3(event)">
                        <%--Rev 2.0--%>
                        <%--OnSummaryDisplayText="ShowGridDetails2Level_SummaryDisplayText"--%>
                        <%--End of Rev 2.0--%>
                        <dxe:ASPxGridView runat="server" ID="gridStockDetials"  ClientInstanceName="cgridStockDetials" KeyFieldName="SEQ" Width="100%"  AutoGenerateColumns="False"
                            OnDataBinding="grivaluation_DataBinding" KeyboardSupport="true" Settings-HorizontalScrollBarMode="Auto" OnSummaryDisplayText="gridStockDetials_SummaryDisplayText">
                            <ClientSideEvents EndCallback="ClosePopUpstkDet"  />
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
    <%--Rev 2.0--%>
    <asp:HiddenField runat="server" ID="hdnLedgertype" />
    <%--End of Rev 2.0--%>
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
</asp:Content>
