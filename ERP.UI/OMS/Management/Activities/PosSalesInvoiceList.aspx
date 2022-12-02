<%@ Page Title="Sales Invoice (POS)" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="PosSalesInvoiceList.aspx.cs" Inherits="ERP.OMS.Management.Activities.PosSalesInvoiceList" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../../../assests/pluggins/DataTable/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="../../../assests/pluggins/DataTable/jquery.dataTables.min.js"></script>

    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchPopup.js"></script>
    <%--<script src="JS/SearchPopup.js"></script>--%>
    <script>
        var CopyInvoiceId;
        var cpSelectedKeys = [];
        var rets;
        var actionscheme = "";

        function onApprovalList() {
            cApproveRejectPopup.Show();
            cGridApproval.Refresh();
        }

        function onInfluencerScheme(id) {
            //alert(id);
            $("#invid").val(id)

            actionscheme = "add";



            $.ajax({
                type: "POST",
                url: "PosSalesInvoiceList.aspx/GetInfluencerSchemeDetails",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ invid: id }),
                dataType: "json",
                success: function (msg) {
                    var status = msg.d;
                    var str = "";
                    if (parseInt(status.RemainingBalance) > 0) {

                        if (status.INF_Inv_Details) {
                            $("#txtInvoiceNumberScheme").html(status.INF_Inv_Details.Inv_No);
                            $("#txtInvoiceAmountScheme").html(status.INF_Inv_Details.Amount);
                        }

                        str = "<thead>";
                        str = str + "<tr>";
                        str = str + "<th class='hide'>INFID</th>";
                        str = str + "<th>Influencer Name</th>";
                        str = str + "<th>Action</th>";
                        str = str + "</tr>";
                        str = str + "</thead>";
                        str = str + "<tbody id='infSchemebody'>";
                        for (var i = 0; i < status.Influencer_Details.length; i++) {
                            actionscheme = "Edit";
                            str = str + "<tr id='tr" + status.Influencer_Details[i].DET_INFLUENCER_ID + "'>";
                            str = str + "<td id='infid" + status.Influencer_Details[i].DET_INFLUENCER_ID + "' class='hide'>" + status.Influencer_Details[i].DET_INFLUENCER_ID + "</td>";
                            str = str + "<td id='infname" + status.Influencer_Details[i].DET_INFLUENCER_ID + "' >" + status.Influencer_Details[i].INF_Name + "</td>"
                            str = str + "<td><img onclick='infdeleteClick(" + JSON.stringify(status.Influencer_Details[i].DET_INFLUENCER_ID) + ")' src='../../../assests/images/crs.png' /></td>";
                            str = str + "</tr>";
                        }
                        str = str + "</tbody>";
                        $("#tableInfluencer").html('');
                        $("#tableInfluencer").html(str);




                    }
                    else {
                        jAlert('Return fully done . Can not add Influencer.', 'Alert');
                    }
                }

            });



            $("#InfluenceSchemeModel").modal('show');
        }


        function SaveInfluencerScheme() {
            var myTableArray = [];

            $("#tableInfluencer tr").each(function () {
                var arrayOfThisRow = [];
                var tableData = $(this).find('td');
                if (tableData.length > 0) {
                    tableData.each(function () {
                        arrayOfThisRow.push($(this).html());
                    });
                    myTableArray.push(arrayOfThisRow);
                }
            });


            var Infobj = [];
            for (var i = 0; i <= myTableArray.length - 1; i++) {

                var Myobj = {
                    INFID: myTableArray[i][0]
                };
                Infobj.push(Myobj);
            }

            console.log(Infobj);


            var obj = {};
            obj.PostingDate = new Date();
            obj.Schema = "0";
            obj.Billno = "";
            obj.Branch = $("#ddlBranch").val();
            obj.Invoice_Id = $("#invid").val();
            obj.Remarks = "";
            obj.Mainaccr = "";



            var proobj = [];
            obj.product = proobj;

            var myInfobj = JSON.stringify(Infobj);

            obj.Influencer = Infobj;

            obj.Action = actionscheme;


            if (Infobj.length == 0) {

                jAlert('Please add atleast one Influencer to proceed.', 'Alert');
                return;
            }


            $.ajax({
                type: "POST",
                url: "PosSalesInvoiceList.aspx/SaveInfluencerScheme",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ infsave: obj }),
                dataType: "json",
                async: false,
                success: function (response) {
                    actionscheme = "Add";
                    jAlert(response.d);
                    $("#InfluenceSchemeModel").modal('toggle');
                },
                error: function (response) {
                    jAlert("Please try again later");
                    //LoadingPanel.Hide();
                }
            });


        }




        var invoice_appId = "";
        function AddApproved(id) {
            invoice_appId = id;
            $("#ApprovalPopupModel").modal('show');
        }

        function SaveMainbill() {
            var obj = {};
            obj.SchemaID = $("#ddl_numberingScheme").val();
            obj.DocNo = ctxt_PLQuoteNo.GetText();
            obj.invoice_id = invoice_appId;
            obj.Challan_SchemaID = cchallanNoScheme.GetValue();
            obj.Challan_Doc = ctxtChallanNo.GetText();

            $.ajax({
                type: "POST",
                url: "PosSalesInvoiceList.aspx/SavePOSMainDoc",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(obj),
                dataType: "json",
                success: function (msg) {
                    var status = msg.d.split('~');

                    if (status[0] == "1") {

                        jAlert(status[1], "Alert", function () {
                            $("#ApprovalPopupModel").modal('hide');
                            cGridApproval.Refresh();
                            newInvoiceId = invoice_appId;


                            if (status[3] == "CGST") {
                                reportName = "POS-CGST~D";

                                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');
                                if (status[2] == "D") {
                                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + "GatePass~D" + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');
                                }
                            }
                            else if (status[3] == "IGST") {
                                reportName = "POS-IGST~D";
                                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');
                                if (status[2] == "D") {
                                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + "GatePass~D" + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=1', '_blank');
                                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + "GatePass~D" + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=2', '_blank');
                                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + "GatePass~D" + '&modulename=Invoice_POS&id=' + newInvoiceId + '&PrintOption=3', '_blank');
                                }
                            }


                        });
                    }
                    else {
                        jAlert(status[1], "Alert");
                    }

                },
                error: function (msg) {
                    var status = msg.d;
                    $("#ApprovalPopupModel").modal('show');
                }
            });
        }

        function UniqueCodeCheck() {

            var QuoteNo = ctxt_PLQuoteNo.GetText();
            if (QuoteNo != '') {
                var CheckUniqueCode = false;
                $.ajax({
                    type: "POST",
                    url: "SalesInvoice.aspx/CheckUniqueCode",
                    data: JSON.stringify({ QuoteNo: QuoteNo }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        CheckUniqueCode = msg.d;
                        if (CheckUniqueCode == true) {
                            jAlert('Please enter unique Invoice number');
                            ctxt_PLQuoteNo.SetValue('');
                            ctxt_PLQuoteNo.Focus();
                        }
                        else {
                            $('#duplicateQuoteno').attr('style', 'display:none');
                        }
                    }
                });
            }
        }


        function SetInfluencer(Id, Name) {
            if (Id) {

                var inf_id = Id.split('~')[0];
                var Ma_id = Id.split('~')[1];
                var Ma_name = Id.split('~')[2];

                if (Ma_id == "") {
                    jAlert('Please add a main account to influencer to proceed');
                    return;
                }

                if (Ma_id == "0") {
                    jAlert('Please add a main account to influencer to proceed');
                    return;
                }

                if (Ma_id == null) {
                    jAlert('Please add a main account to influencer to proceed');
                    return;
                }



                $("#infid").val(inf_id);
                ctxtInfluencerAdjustment.SetText(Name);
                $('#InfluencerModel').modal('toggle');
                onInfluencerReturn();
                // $('#VehicleModel').modal('toggle');
            }
        }

        function SetInfluencerScheme(Id, Name) {
            if (Id) {

                var inf_id = Id.split('~')[0];
                var Ma_id = Id.split('~')[1];
                var Ma_name = Id.split('~')[2];





                $("#infid").val(inf_id);
                ctxtInfluencerScheme.SetText(Name);
                $('#InfluencerSchemeModel').modal('toggle');
                //onInfluencerReturn();
                // $('#VehicleModel').modal('toggle');
            }
        }


        function onInfluencerReturn() {
            $("#CmbScheme").val('0');
            $("#txtBillNo").val("")
            $("#txtBillNo").prop('disabled', true)
            $.ajax({
                type: "POST",
                url: "PosSalesInvoiceList.aspx/GetInfluencerReturnDetails",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ invid: $("#infid").val() }),
                dataType: "json",
                success: function (msg) {
                    var status = msg.d;

                    var inv = status.Invoice_Data;
                    rets = status.Return_Data;
                    var ret = rets;
                    str = "<thead>";
                    str = str + "<tr>";

                    str = str + "<th  class='hide'>DOCID</th>";
                    str = str + "<th >Select</th>";
                    str = str + "<th  class='hide'>INFLUENCERID</th>";
                    str = str + "<th >Influencer</th>";
                    str = str + "<th>Return Number</th>";
                    str = str + "<th >Return Date</th>";
                    str = str + "<th style='width:200px;'>Total Comm. Amount</th>";
                    str = str + "<th>Unpaid Amount</th>";
                    str = str + "<th class='hide'>Actual Amount</th>";
                    str = str + "</tr>";
                    str = str + "</thead>";
                    str = str + "<tbody id='infadjretbodyRet'>";
                    for (var i = 0; i < ret.length; i++) {
                        str = str + "<tr id='trRet" + ret[i].DOC_ID + "'>";

                        str = str + "<td  class='hide' id='DocRet_" + ret[i].DOC_ID + "'>" + ret[i].DOC_ID + "</td>";
                        str = str + "<td  id='tdRetInfluencerIDSelect" + ret[i].DOC_ID + "'><input type='checkbox' id='chkInfIdRet" + ret[i].DOC_ID + "'></td>";
                        str = str + "<td  class='hide' id='tdRetInfluencerID" + ret[i].DOC_ID + "'>" + ret[i].CON_ID + "</td>";


                        str = str + "<td id='tdRetInfluencer" + ret[i].DOC_ID + "' >" + ret[i].NAME + "</td>";

                        str = str + "<td id='tdRetNumber" + ret[i].DOC_ID + "'>" + ret[i].DOC_NUMBER + "</td>";
                        str = str + "<td id='tdRetDate" + ret[i].DOC_ID + "' >" + ret[i].DOC_DATE + "</td>";
                        str = str + "<td id='tdRetComm" + ret[i].DOC_ID + "'  >" + ret[i].Total_Comm + "</td>";
                        str = str + "<td id='tdRetUnpaid" + ret[i].DOC_ID + "' ><input type='text' id='unpaidRet" + ret[i].DOC_ID + "' value= " + ret[i].Unpaid + " ></td>";
                        str = str + "<td class='hide' id='tdRetActualUnpaid" + ret[i].DOC_ID + "' >" + ret[i].Unpaid + " </td>";

                        str = str + "</tr>";
                    }
                    str = str + "</tbody>";
                    $("#tableProductAdjustmentReturn").html('');
                    $("#tableProductAdjustmentReturn").html(str);

                    $("#InfluencerPopupModel").modal('show');



                    str = "<thead>";
                    str = str + "<tr>";
                    str = str + "<th  class='hide'>DOC_ID</th>";
                    str = str + "<th >Select</th>";
                    str = str + "<th  class='hide'>INFLUENCERID</th>";
                    str = str + "<th >Influencer</th>";
                    str = str + "<th>Invoice Number</th>";
                    str = str + "<th >Return Date</th>";
                    str = str + "<th style='width:200px;'>Total Comm. Amount</th>";
                    str = str + "<th>Unpaid Amount</th>";
                    str = str + "</tr>";
                    str = str + "</thead>";
                    str = str + "<tbody id='infadjretbodyInv'>";
                    for (var i = 0; i < inv.length; i++) {
                        str = str + "<tr id='trInv" + inv[i].DOC_ID + "'>";

                        str = str + "<td class='hide' id='DocInv_" + inv[i].DOC_ID + "'>" + inv[i].DOC_ID + "</td>";

                        //str = str + "<td class='hide' id='tdInvInfluencerIDSelect" + inv[i].DOC_ID + "'><input type='radio' id='radInfId" + inv[i].CON_ID + "' name='InfSelection' onclick='handleClick(" + JSON.stringify(inv[i].CON_ID) + ");'></td>";
                        str = str + "<td  id='tdInvInfluencerIDSelect" + inv[i].DOC_ID + "'><input type='checkbox' id='chkInfIdInv" + inv[i].DOC_ID + "'></td>";

                        str = str + "<td  class='hide' id='tdInvInfluencerID" + inv[i].DOC_ID + "'>" + inv[i].CON_ID + "</td>";
                        str = str + "<td id='tdInvInfluencer" + inv[i].DOC_ID + "' >" + inv[i].NAME + "</td>";


                        str = str + "<td id='tdInvNumber" + inv[i].DOC_ID + "'>" + inv[i].DOC_NUMBER + "</td>";
                        str = str + "<td id='tdInvDate" + inv[i].DOC_ID + "' >" + inv[i].DOC_DATE + "</td>";
                        str = str + "<td id='tdInvComm" + inv[i].DOC_ID + "'  >" + inv[i].Total_Comm + "</td>";
                        str = str + "<td id='tdInvUnpaid" + inv[i].DOC_ID + "' ><input type='text' id='unpaidInv" + inv[i].DOC_ID + "' value= " + inv[i].Unpaid + " ></td>";
                        str = str + "</tr>";
                    }
                    str = str + "</tbody>";
                    $("#tableProductAdjustmentInvoice").html('');
                    $("#tableProductAdjustmentInvoice").html(str);

                    $("#InfluencerPopupModel").modal('show');

                }
            });

        }





        function SaveInfluencerdetailsAdjustment() {

            var InfobjInv = [];
            var InfobjRet = [];


            $("#tableProductAdjustmentInvoice tr").each(function () {
                var arrayOfThisRow = [];
                var tableData = $(this).find('td');
                if (tableData.length > 0) {

                    var id = $(tableData[0]).html().trim();
                    if ($('#chkInfIdInv' + id).prop("checked")) {
                        var Myobj = {
                            INF_ID: $("#tdInvInfluencerID" + id).html(),
                            DOC_ID: id,
                            AMOUNT: $("#unpaidInv" + id).val()
                        };
                        InfobjInv.push(Myobj);

                    }


                }
            });
            var myInfobjInv = JSON.stringify(InfobjInv);


            $("#tableProductAdjustmentReturn tr").each(function () {
                var arrayOfThisRow = [];
                var tableData = $(this).find('td');
                if (tableData.length > 0) {

                    var id = $(tableData[0]).html().trim();
                    if ($('#chkInfIdRet' + id).prop("checked")) {
                        var Myobj = {
                            INF_ID: $("#tdRetInfluencerID" + id).html(),
                            DOC_ID: id,
                            AMOUNT: $("#unpaidRet" + id).val()
                        };
                        InfobjRet.push(Myobj);
                    }



                }
            });
            var myInfobjRet = JSON.stringify(InfobjRet);







            $.ajax({
                type: "POST",
                url: "PosSalesInvoiceList.aspx/SaveInfluencerAdj",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ invoice: InfobjInv, returns: InfobjRet }),
                dataType: "json",
                async: false,
                success: function (response) {

                    jAlert(response.d);
                    $("#InfluencerPopupModel").modal('toggle');
                },
                error: function (response) {
                    jAlert("Please try again later");
                    //LoadingPanel.Hide();
                }
            });
        }


        function handleClick(infID) {

            var RetFilter = $.grep(rets, function (e) { return e.CON_ID == infID; });
            str = "<thead>";
            str = str + "<tr>";
            str = str + "<th  class='hide'>DOC_ID</th>";
            str = str + "<th  class='hide'>INFLUENCERID</th>";

            str = str + "<th >Influencer</th>";
            str = str + "<th>Return Number</th>";
            str = str + "<th >Return Date</th>";
            str = str + "<th style='width:200px;'>Total Comm. Amount</th>";
            str = str + "<th>Unpaid Amount</th>";
            str = str + "<th class='hide'>Actual Amount</th>";
            str = str + "</tr>";
            str = str + "</thead>";
            str = str + "<tbody id='infadjretbodyRet'>";
            for (var i = 0; i < RetFilter.length; i++) {
                str = str + "<tr id='trRet" + RetFilter[i].DOC_ID + "'>";
                str = str + "<td class='hide' id='DocRet_" + RetFilter[i].DOC_ID + "'>" + RetFilter[i].DOC_ID + "</td>";
                str = str + "<td  class='hide' id='tdRetInfluencerID" + RetFilter[i].DOC_ID + "'>" + RetFilter[i].CON_ID + "</td>";
                str = str + "<td id='tdRetInfluencer" + RetFilter[i].DOC_ID + "' >" + RetFilter[i].NAME + "</td>";

                str = str + "<td id='tdRetNumber" + RetFilter[i].DOC_ID + "'>" + RetFilter[i].DOC_NUMBER + "</td>";
                str = str + "<td id='tdRetDate" + RetFilter[i].DOC_ID + "' >" + RetFilter[i].DOC_DATE + "</td>";
                str = str + "<td id='tdRetComm" + RetFilter[i].DOC_ID + "'  >" + RetFilter[i].Total_Comm + "</td>";
                str = str + "<td id='tdRetUnpaid" + RetFilter[i].DOC_ID + "' ><input type='text' id='unpaidRet" + RetFilter[i].DOC_ID + "' value= " + RetFilter[i].Unpaid + " ></td>";
                str = str + "<td class='hide' id='tdRetActualUnpaid" + RetFilter[i].DOC_ID + "' >" + RetFilter[i].Unpaid + " </td>";

                str = str + "</tr>";
            }
            str = str + "</tbody>";
            $("#tableProductAdjustmentReturn").html('');
            $("#tableProductAdjustmentReturn").html(str);

            $("#InfluencerPopupModel").modal('show');

        }







        function VehicleSelectionChanged(s, e) {
            if (e.isChangedOnServer) return;
            globalindexcheck = e.visibleIndex;
            var key = s.GetRowKey(e.visibleIndex);
            if (e.isSelected) {
                cpSelectedKeys.push(key);
            }
            else {
                cpSelectedKeys = RemoveElementFromArray(cpSelectedKeys, key);

            }
            appcode = cpSelectedKeys;

        }
        function OnclickViewAttachment(obj) {
            //var URL = '/OMS/Management/Activities/SalesInvoice_Document.aspx?idbldng=' + obj + '&type=SalesInvoice';
            var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=SalesInvoice';
            window.location.href = URL;
        }

        function RemoveElementFromArray(array, element) {
            var index = array.indexOf(element);
            if (index < 0) return array;
            array[index] = null;
            var result = [];
            for (var i = 0; i < array.length; i++) {
                if (array[i] === null)
                    continue;
                result.push(array[i]);
            }
            return result;
        }

        function CustomerClick(s, e) {


            $("#hdnCustomerId").val(e);


            $("#<%=drdExport.ClientID%>").val('0');
            cOutstandingPopup.Show();
            var CustomerId = $("#<%=hdnCustomerId.ClientID%>").val();
            var BranchId = ccmbBranchfilter.GetValue();
            $("#<%=hddnBranchId.ClientID%>").val(BranchId);
            var AsOnDate = new Date().format('yyyy-MM-dd');
            $("#<%=hddnAsOnDate.ClientID%>").val(AsOnDate);
            $("#<%=hddnOutStandingBlock.ClientID%>").val('1');
            //Clear Row
            var rw = $("[id$='CustomerOutstanding_DXMainTable']").find("tr")
            for (var RowClount = 0; RowClount < rw.length; RowClount++) {
                rw[RowClount].remove();
            }



            //cCustomerOutstanding.Refresh();

            //cCustomerOutstanding.PerformCallback('BindOutStanding~' + CustomerId + '~' + BranchId + '~' + AsOnDate);
            var CheckUniqueCode = false;
            $.ajax({
                type: "POST",
                url: "SalesOrderAdd.aspx/GetCustomerOutStanding",
                data: JSON.stringify({ strAsOnDate: AsOnDate, strCustomerId: CustomerId, BranchId: BranchId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                //async:false,
                success: function (msg) {

                    CheckUniqueCode = msg.d;
                    if (CheckUniqueCode == true) {
                        cCustomerOutstanding.Refresh();

                    }
                }
            });


            //cCustomerOutstanding.Refresh();
            //cOutstandingPopup.Show();


        }



        var SelectedInvoiceId = 0;

        var GlobalRowIndex = 0;
        var BranchMassListByKeyValue = [];
        var isFirstTime = true;
        function AllControlInitilize() {
            if (isFirstTime) {
                // PopulateCurrentBankBalance(ccmbBranchfilter.GetValue());

                if (localStorage.getItem('PosListFromDate')) {
                    var fromdatearray = localStorage.getItem('PosListFromDate').split('-');
                    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                    cFormDate.SetDate(fromdate);
                }

                if (localStorage.getItem('PosListToDate')) {
                    var todatearray = localStorage.getItem('PosListToDate').split('-');
                    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                    ctoDate.SetDate(todate);
                }
                if (localStorage.getItem('PosListBranch')) {
                    if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('PosListBranch'))) {
                        ccmbBranchfilter.SetValue(localStorage.getItem('PosListBranch'));
                    }

                }


                if ($("#LoadGridData").val() == "ok")
                    updateGridByDate();

                isFirstTime = false;
            }
        }

        $(function () {
            BindERPSettings();
        });

        function BindERPSettings() {
            var OtherDetails = {}
            OtherDetails.Key = "ShowCreditInvoice";
            $.ajax({
                type: "POST",
                url: "Services/Master.asmx/GetMasterSettings",
                data: JSON.stringify(OtherDetails),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var returnObject = msg.d;

                    if (returnObject == "0") {
                        $("#btnCredit").addClass(" hide");
                    }
                    else {
                        $("#btnCredit").removeClass("hide");
                    }

                }
            });
            OtherDetails.Key = "ShowCashInvoice";
            $.ajax({
                type: "POST",
                url: "Services/Master.asmx/GetMasterSettings",
                data: JSON.stringify(OtherDetails),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var returnObject = msg.d;

                    if (returnObject == "0") {
                        $("#btnCash").addClass(" hide");
                    }
                    else {
                        $("#btnCash").removeClass("hide");
                    }

                }
            });


            OtherDetails.Key = "ShowFinInvoice";
            $.ajax({
                type: "POST",
                url: "Services/Master.asmx/GetMasterSettings",
                data: JSON.stringify(OtherDetails),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var returnObject = msg.d;

                    if (returnObject == "0") {
                        $("#btnFin").addClass(" hide");
                    }
                    else {
                        $("#btnFin").removeClass("hide");
                    }

                }
            });


            OtherDetails.Key = "ShowIST";
            $.ajax({
                type: "POST",
                url: "Services/Master.asmx/GetMasterSettings",
                data: JSON.stringify(OtherDetails),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var returnObject = msg.d;

                    if (returnObject == "0") {
                        $("#btnIST").addClass(" hide");
                    }
                    else {
                        $("#btnIST").removeClass("hide");
                    }

                }
            });


            OtherDetails.Key = "ShowCRP";
            $.ajax({
                type: "POST",
                url: "Services/Master.asmx/GetMasterSettings",
                data: JSON.stringify(OtherDetails),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var returnObject = msg.d;

                    if (returnObject == "0") {
                        $("#btnCRP").addClass(" hide");
                    }
                    else {
                        $("#btnCRP").removeClass("hide");
                    }

                }
            });

        }

        function updateMassBranchAssign() {

        }
        function PerformCallToRacpayGridBind() {
            CustomerRecpayPanel.PerformCallback('Bindsingledesign');
            cCustDocumentsPopup.Hide();
            return false;
        }

        function CustRacPayPanelEndCall(s, e) {
            debugger;
            if (CustomerRecpayPanel.cpSuccess != null) {
                var TotDocument = CustomerRecpayPanel.cpSuccess.split(',');
                var reportName = cCustCmbDesignName.GetValue();
                var module = 'CUSTRECPAY';
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + RecPayId + '&PrintOption=' + 1, '_blank')
            }
            CustomerRecpayPanel.cpSuccess = null
            if (CustomerRecpayPanel.cpSuccess == null) {
                CustomerRecpayPanel.SetSelectedIndex(0);
            }
        }

        function onCustomerReceiptPrint(id) {
            RecPayId = id;
            cCustDocumentsPopup.Show();
            cCustCmbDesignName.SetSelectedIndex(0);
            CustomerRecpayPanel.PerformCallback('Bindalldesignes');
            $('#btnOK').focus();
        }

        function OnCustReceiptViewClick(id) {
            uri = "CustomerReceiptPayment.aspx?key=" + id + "&req=V&IsTagged=Y&type=CRP";
            capcReciptPopup.SetContentUrl(uri);
            capcReciptPopup.SetHeaderText("View Money Receipt");
            capcReciptPopup.Show();
        }

        function ListingGridEndCallback(s, e) {
            IconChange();
            if (cGrdQuotation.cpCancelAssignMent) {
                if (cGrdQuotation.cpCancelAssignMent == "yes") {
                    jAlert("Branch Assignment Cancel Successfully.");
                    cGrdQuotation.cpCancelAssignMent = null;
                    cGrdQuotation.Refresh();
                }
            }


            if (cGrdQuotation.cpDelete) {
                jAlert(cGrdQuotation.cpDelete);
                cGrdQuotation.cpDelete = null;
                cGrdQuotation.Refresh();

            }
        }




        function CancelBranchToThisInvoice() {
            cAssignedBranch.SetValue('0');
            AssignedBranchSelectedIndexChanged(cAssignedBranch);
        }

        function onCancelBranchAssignment(invId) {
            cGrdQuotation.PerformCallback('CancelAssignment~' + invId);
        }

        var JVid = "";
        var Action = "Add";
        var AutoJVNumber = "";

        function onAddInfluencer(invId) {
            $("#CmbScheme").val('0');
            $("#txtBillNo").val("")
            $("#txtBillNo").prop('disabled', true)
            $.ajax({
                type: "POST",
                url: "PosSalesInvoiceList.aspx/GetInfluencerDetails",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ invid: invId }),
                dataType: "json",
                success: function (msg) {
                    var status = msg.d;
                    var str = "";
                    if (parseInt(status.RemainingBalance) > 0) {

                        if (status.INF_Inv_Details) {
                            $("#ddlBranch").val(status.INF_Inv_Details.Inv_BranchId);
                            $("#invid").val(status.INF_Inv_Details.Inv_Id);
                            $("#txtInvoiceNumber").html(status.INF_Inv_Details.Inv_No);
                            $("#txtInvoiceAmount").html(status.INF_Inv_Details.Amount);



                        }
                        if (status.Influencer.AUTOJV_NUMBER != "" && status.Influencer.AUTOJV_NUMBER != null) {
                            $("#div_Edit").addClass('hide');
                            AutoJVNumber = status.Influencer.AUTOJV_NUMBER
                            $("#txtBillNo").val(status.Influencer.AUTOJV_NUMBER);
                            $("#txtBillNo").prop('disabled', true);
                            cbtnMAdr.SetText(status.Influencer.MainAccount_AccountCode);
                            $("#txtCommAmt").html(status.Influencer.COMM_AMOUNT);
                            $("#mainacdrid").val(status.Influencer.MAINACCOUNT_DR);
                            ctxtInfRemarks.SetText(status.Influencer.Remarks);

                            if (parseInt(status.Influencer.IsTagged) > 0) {
                                $("#divSaveinf").addClass('hide')
                                $("#divDeleteinf").addClass('hide')
                                $("#divmsg").removeClass('hide')
                            }
                            else {
                                $("#divmsg").addClass('hide')
                                $("#divSaveinf").removeClass('hide')
                                $("#divDeleteinf").removeClass('hide')
                            }

                            tDate.SetDate(new Date(parseInt(status.Influencer.POSTING_DATE.substr(6))));
                            tDate.SetEnabled(false);
                            Action = "Edit";
                        }
                        else {
                            $("#div_Edit").removeClass('hide');

                            $("#txtBillNo").prop('disabled', false);
                            tDate.SetEnabled(true);
                            $("#divDeleteinf").addClass('hide')
                            Action = "Add";
                        }
                        str = "<thead>";
                        str = str + "<tr>";
                        str = str + "<th class='hide'>INFID</th>";
                        str = str + "<th class='hide'>INFMAID</th>";
                        str = str + "<th style='width:200px;'>Liability Ledger</th>";
                        str = str + "<th>Influencer Name</th>";
                        str = str + "<th style='width:150px;'>Amount</th>";
                        str = str + "<th>Action</th>";
                        str = str + "</tr>";
                        str = str + "</thead>";
                        str = str + "<tbody id='infbody'>";
                        for (var i = 0; i < status.Influencer_Details.length; i++) {
                            str = str + "<tr id='tr" + status.Influencer_Details[i].DET_INFLUENCER_ID + "'>";
                            str = str + "<td id='infid" + status.Influencer_Details[i].DET_INFLUENCER_ID + "' class='hide'>" + status.Influencer_Details[i].DET_INFLUENCER_ID + "</td>";
                            str = str + "<td id='infmaid" + status.Influencer_Details[i].DET_INFLUENCER_ID + "' class='hide' >" + status.Influencer_Details[i].DET_MAINACCOUNT_CR + "</td>";
                            str = str + "<td id='infmaname" + status.Influencer_Details[i].DET_INFLUENCER_ID + "' >" + status.Influencer_Details[i].DET_MAINACCOUNT_NAME + "</td>";
                            str = str + "<td id='infname" + status.Influencer_Details[i].DET_INFLUENCER_ID + "' >" + status.Influencer_Details[i].INF_Name + "</td>";

                            if (status.Influencer_Details.length > 1)
                                str = str + "<td id='infamt" + status.Influencer_Details[i].DET_INFLUENCER_ID + "' ><input  type='text' id='txtinfamt" + status.Influencer_Details[i].DET_INFLUENCER_ID + "' value='" + status.Influencer_Details[i].DET_AMOUNT_CR + "'  /></td>";
                            else
                                str = str + "<td id='infamt" + status.Influencer_Details[i].DET_INFLUENCER_ID + "' ><input  type='text' id='txtinfamt" + status.Influencer_Details[i].DET_INFLUENCER_ID + "' value='" + status.Influencer_Details[i].DET_AMOUNT_CR + "' disabled /></td>";


                            str = str + "<td><img onclick='infdeleteClick(" + JSON.stringify(status.Influencer_Details[i].DET_INFLUENCER_ID) + ")' src='../../../assests/images/crs.png' /></td>";
                            str = str + "</tr>";
                        }
                        str = str + "</tbody>";
                        $("#influencerGrid").html('');
                        $("#influencerGrid").html(str);

                        str = "<thead><tr>";
                        str = str + "<th class='hide'>Product Id</th>";
                        str = str + "<th style='width:250px !important;'>Product Names</th>";
                        str = str + "<th>Product Qty.</th>";
                        str = str + "<th>Sales Price</th>";
                        str = str + "<th>Amount(Before GST)</th>";
                        str = str + "<th>Amount(With GST)";
                        str = str + "<th class='hide'>prod det id</th>";
                        str = str + "<th>Basis</th>";
                        str = str + "<th>Commission Rate/Qty or Commission %</th>";
                        str = str + "<th>Comm. Amount</th>";
                        str = str + "</tr>";
                        str = str + "</thead>";
                        str = str + "<tbody id='prodbody'>";
                        for (var i = 0; i < status.INF_Inv_Products.length; i++) {
                            str = str + "<tr class='detINF'>";
                            str = str + "<td id='prodid" + status.INF_Inv_Products[i].Prod_id + "' class='hide'>" + status.INF_Inv_Products[i].Prod_id + "</td>";
                            str = str + "<td id='proddesc" + status.INF_Inv_Products[i].Prod_id + "' style='width:200px !important;'>" + status.INF_Inv_Products[i].Prod_description + "</td>";
                            str = str + "<td id='prodqty" + status.INF_Inv_Products[i].Prod_id + "' >" + status.INF_Inv_Products[i].prod_Qty + "</td>";
                            str = str + "<td id='prodsp" + status.INF_Inv_Products[i].Prod_id + "' >" + status.INF_Inv_Products[i].prod_Salesprice + "</td>";
                            str = str + "<td id='proddetamt" + status.INF_Inv_Products[i].Prod_id + "' >" + status.INF_Inv_Products[i].prod_amt + "</td>";
                            str = str + "<td id='proddetamtgst" + status.INF_Inv_Products[i].Prod_id + "' >" + status.INF_Inv_Products[i].prod_SalespriceWithGST + "</td>";

                            str = str + "<td id='proddetid" + status.INF_Inv_Products[i].Prod_id + "' class='hide'>" + status.INF_Inv_Products[i].prod_details_id + "</td>";

                            if (parseFloat(status.INF_Inv_Products[i].prod_Qty) > 0)
                                str = str + "<td><select id='prodappl" + status.INF_Inv_Products[i].Prod_id + "' onchange='ChangeType(" + status.INF_Inv_Products[i].Prod_id + ")'>";
                            else
                                str = str + "<td><select disabled id='prodappl" + status.INF_Inv_Products[i].Prod_id + "' onchange='ChangeType(" + status.INF_Inv_Products[i].Prod_id + ")'>";


                            if (status.INF_Inv_Products[i].Applicable_On == "1") {
                                str = str + "<option value='1' selected>On Qty.</option>";
                            }
                            else {
                                str = str + "<option value='1'>On Qty.</option>";
                            }



                            if (status.INF_Inv_Products[i].Applicable_On == "2") {
                                str = str + "<option value='2' selected>Amount before GST</option>";
                            }
                            else {
                                str = str + "<option value='2'>Amount before GST</option>";

                            }


                            if (status.INF_Inv_Products[i].Applicable_On == "3") {
                                str = str + "<option value='3' selected>Amount with GST</option>";
                            }
                            else {
                                str = str + "<option value='3'>Amount with GST</option>";
                            }


                            if (status.INF_Inv_Products[i].Applicable_On == "4") {
                                str = str + "<option value='4' selected>Flat Value</option>";
                            }
                            else {
                                str = str + "<option value='4'>Flat Value</option>";

                            }
                            str = str + "</select></td>";


                            if (parseFloat(status.INF_Inv_Products[i].prod_Qty) > 0) {
                                if (status.INF_Inv_Products[i].Applicable_On == "4") {
                                    str = str + "<td><input type='textbox' class='numeric'  onfocusout='percenLostfocus(" + status.INF_Inv_Products[i].Prod_id + ")' onkeypress='return isNumberKey(event)' id='prodpercen" + status.INF_Inv_Products[i].Prod_id + "' value='" + status.INF_Inv_Products[i].Prod_Percentage + "'disabled></td>";
                                    str = str + "<td><input type='textbox' class='numeric'  onfocusout='AmountLostfocus(" + status.INF_Inv_Products[i].Prod_id + ")' onkeypress='return isNumberKey(event)' id='prodcommamt" + status.INF_Inv_Products[i].Prod_id + "' value='" + status.INF_Inv_Products[i].PROD_COMM_AMOUNT + "' ></td>";
                                }
                                else {
                                    str = str + "<td><input type='textbox' class='numeric'  onfocusout='percenLostfocus(" + status.INF_Inv_Products[i].Prod_id + ")' onkeypress='return isNumberKey(event)' id='prodpercen" + status.INF_Inv_Products[i].Prod_id + "' value='" + status.INF_Inv_Products[i].Prod_Percentage + "' ></td>";
                                    str = str + "<td><input type='textbox' class='numeric'  onfocusout='AmountLostfocus(" + status.INF_Inv_Products[i].Prod_id + ")' onkeypress='return isNumberKey(event)' id='prodcommamt" + status.INF_Inv_Products[i].Prod_id + "' value='" + status.INF_Inv_Products[i].PROD_COMM_AMOUNT + "' disabled></td>";
                                }
                            }
                            else {
                                str = str + "<td><input type='textbox' class='numeric'  onfocusout='percenLostfocus(" + status.INF_Inv_Products[i].Prod_id + ")' onkeypress='return isNumberKey(event)' id='prodpercen" + status.INF_Inv_Products[i].Prod_id + "' value='" + status.INF_Inv_Products[i].Prod_Percentage + "' disabled></td>";
                                str = str + "<td><input type='textbox' class='numeric'  onfocusout='AmountLostfocus(" + status.INF_Inv_Products[i].Prod_id + ")' onkeypress='return isNumberKey(event)' id='prodcommamt" + status.INF_Inv_Products[i].Prod_id + "' value='" + status.INF_Inv_Products[i].PROD_COMM_AMOUNT + "' disabled></td>";

                            }


                            str = str + "</tr>";


                            //if( parseFloat($('#prodqty' + status.INF_Inv_Products[i].Prod_id).val())==0){
                            //    $('#prodqty' + status.INF_Inv_Products[i].Prod_id).closest(".detINF").find('input, select').attr('disabled', true)
                            //}
                        }
                        str = str + "</tbody>";
                        // $(".numeric").numeric({ decimal: ".", negative: false, scale: 3 });
                        $("#tableProduct").html('');
                        $("#tableProduct").html(str);



                        //$('#tableProduct').DataTable({
                        //    "iDisplayLength": 5,
                        //    "searching": false,
                        //    "lengthChange": false
                        //});

                        $("#myModal").modal('toggle');
                    }
                    else {
                        jAlert('Return fully done . Can not add Influencer.', 'Alert');
                    }
                }

            });


        }

        function CmbScheme_ValueChange() {

            var val = document.getElementById("CmbScheme").value;
            $("#MandatoryBillNo").hide();
            if (val != "0") {
                $.ajax({
                    type: "POST",
                    url: '../DailyTask/JournalEntry.aspx/getSchemeType',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{sel_scheme_id:\"" + val + "\"}",
                    success: function (type) {
                        console.log(type);

                        var schemetypeValue = type.d;
                        var schemetype = schemetypeValue.toString().split('~')[0];
                        var schemelength = schemetypeValue.toString().split('~')[1];
                        $('#txtBillNo').attr('maxLength', schemelength);
                        var branchID = schemetypeValue.toString().split('~')[2];
                        var branchStateID = schemetypeValue.toString().split('~')[3];

                        var fromdate = schemetypeValue.toString().split('~')[4];
                        var todate = schemetypeValue.toString().split('~')[5];

                        var dt = new Date();

                        tDate.SetDate(dt);

                        if (dt < new Date(fromdate)) {
                            tDate.SetDate(new Date(fromdate));
                        }

                        if (dt > new Date(todate)) {
                            tDate.SetDate(new Date(todate));
                        }




                        tDate.SetMinDate(new Date(fromdate));
                        tDate.SetMaxDate(new Date(todate));

                        if (schemetypeValue != "") {

                        }
                        if (schemetype == '0') {

                            document.getElementById('<%= txtBillNo.ClientID %>').disabled = false;
                            document.getElementById('<%= txtBillNo.ClientID %>').value = "";
                            //document.getElementById("txtBillNo").focus();
                            setTimeout(function () { $("#txtBillNo").focus(); }, 200);

                        }
                        else if (schemetype == '1') {

                            document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                            document.getElementById('<%= txtBillNo.ClientID %>').value = "Auto";
                            tDate.Focus();
                        }
                        else if (schemetype == '2') {

                            document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                            document.getElementById('<%= txtBillNo.ClientID %>').value = "Datewise";
                        }

                    }

                });
    }
    else {
        document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                document.getElementById('<%= txtBillNo.ClientID %>').value = "";
            }
        }
        function OnInfAddClick() {

            var str = "";
            var DET_INFLUENCER_ID = $("#infid").val();
            var trcount = $('#influencerGrid tr').length;
            var DET_AMOUNT_CR = DecimalRoundoff($("#txtCommAmt").html().trim(), 2) / parseInt(trcount);
            var DET_MAINACCOUNT_CR = $("#mainaccrid").val();
            var INF_Name = ctxtInfluencer.GetText();
            var DET_MAINACCOUNT_NAME = cbtnMainAccount.GetText();

            var valid = true;

            $("#infbody tr td:contains(" + DET_INFLUENCER_ID + ")")
            .next().text(function () {
                valid = false;
            });

            if (!valid) {
                jAlert('Influencer already added.', 'Alert');
                return;
            }

            str = str + "<tr id='tr" + DET_INFLUENCER_ID + "'>";
            str = str + "<td id='infid" + DET_INFLUENCER_ID + "' class='hide'>" + DET_INFLUENCER_ID + "</td>";
            str = str + "<td id='infmaid" + DET_INFLUENCER_ID + "' class='hide' >" + DET_MAINACCOUNT_CR + "</td>";
            str = str + "<td id='infmaname" + DET_INFLUENCER_ID + "' >" + DET_MAINACCOUNT_NAME + "</td>";
            str = str + "<td id='infname" + DET_INFLUENCER_ID + "' >" + INF_Name + "</td>";
            if (trcount > 2) {
                str = str + "<td id='infamt" + DET_INFLUENCER_ID + "'  class='asas' ><input type='text' id='txtinfamt" + DET_INFLUENCER_ID + "' value='" + DET_AMOUNT_CR + "' /></td>";
            }
            else {
                str = str + "<td id='infamt" + DET_INFLUENCER_ID + "'  class='asas' ><input  type='text' id='txtinfamt" + DET_INFLUENCER_ID + "' value='" + DET_AMOUNT_CR + "' disabled /></td>";

            }
            //str = str + "<td><img onclick='infdeleteClick(" + DET_INFLUENCER_ID + ")' src='../../../assests/images/crs.png' /></td>";
            str = str + '<td><img  onclick="infdeleteClick(' + JSON.stringify(DET_INFLUENCER_ID).replace(/"/g, '&quot;') + ');" src="../../../assests/images/crs.png" /></td>';

            str = str + "</tr>";
            $("#infbody").append(str);


            ReclaculateTotal();





        }
        function OnInfAddSchemeClick() {

            var str = "";
            var DET_INFLUENCER_ID = $("#infid").val();
            var trcount = $('#influencerGrid tr').length;
            var INF_Name = ctxtInfluencerScheme.GetText();

            var valid = true;

            $("#infSchemebody tr td:contains(" + DET_INFLUENCER_ID + ")")
            .next().text(function () {
                valid = false;
            });

            if (!valid) {
                jAlert('Influencer already added.', 'Alert');
                return;
            }

            str = str + "<tr id='tr" + DET_INFLUENCER_ID + "'>";
            str = str + "<td id='infid" + DET_INFLUENCER_ID + "' class='hide'>" + DET_INFLUENCER_ID + "</td>";
            str = str + "<td id='infname" + DET_INFLUENCER_ID + "' >" + INF_Name + "</td>";
            str = str + '<td><img  onclick="infSchemedeleteClick(' + JSON.stringify(DET_INFLUENCER_ID).replace(/"/g, '&quot;') + ');" src="../../../assests/images/crs.png" /></td>';
            str = str + "</tr>";

            $("#infSchemebody").append(str);




        }



        function ValueSelected(e, indexName) {
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var Id = e.target.parentElement.parentElement.cells[0].innerText;
                var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                if (Id) {
                    if (indexName == "MainAccountIndex") {
                        SetMainAccount(Id, name, e);
                    }
                    else if (indexName == "MainAccountIndexdr") {
                        SetMainAccountdr(Id, name, e);
                    }
                    else if (indexName == "InvoiceCustomerIndex") {
                        InvoiceSetCustomer(Id, name);
                    }
                    else if (indexName == "InfluencerIndex") {
                        SetInfluencer(Id, name);
                    }
                    else {
                        SetCustomer(Id, name);
                    }
                }

            }
            else if (e.code == "ArrowDown") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex++;
                if (thisindex < 10)
                    $("input[" + indexName + "=" + thisindex + "]").focus();
            }
            else if (e.code == "ArrowUp") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex--;
                if (thisindex > -1)
                    $("input[" + indexName + "=" + thisindex + "]").focus();
                else {
                    if (indexName == "MainAccountIndex")
                        $('#txtMainAccountSearch').focus();
                    else if (indexName == "MainAccountIndexdr")
                        $('#txtMainAccountSearchdr').focus();
                    else if (indexName == "InvoiceCustomerIndex")
                        $('#InvoicetxtCustsSearch').focus();
                    else
                        $('#txtCustSearch').focus();
                }
            }


        }
        function InvoiceSetCustomer(Id, Name) {
            $("#hdnInvoiceCustID").val(Id);
            ctxtCustName.SetValue(Name);
            $('#InvoiceCustsModel').modal('toggle');
            ctxtCustName.SetFocus()
        }

        function SetCustomer(Id, Name) {
            if (Id) {

                var inf_id = Id.split('~')[0];
                var Ma_id = Id.split('~')[1];
                var Ma_name = Id.split('~')[2];

                if (Ma_id == "") {
                    jAlert('Please add a main account to influencer to proceed');
                    return;
                }

                if (Ma_id == "0") {
                    jAlert('Please add a main account to influencer to proceed');
                    return;
                }

                if (Ma_id == null) {
                    jAlert('Please add a main account to influencer to proceed');
                    return;
                }


                cbtnMainAccount.SetText(Ma_name);
                $("#mainaccrid").val(Ma_id);

                $("#infid").val(inf_id);
                ctxtInfluencer.SetText(Name);
                $('#CustModel').modal('toggle');
                // $('#VehicleModel').modal('toggle');
            }
        }




        function SetMainAccount(Id, name, e) {

            $("#mainaccrid").val(Id);
            cbtnMainAccount.SetText(name);
            $('#MainAccountModel').modal('toggle');
        }
        function challanNoSchemeEndCallback() {
            if (lastChallan) {
                cchallanNoScheme.PerformCallback(lastChallan);
                lastChallan = null;
            }
        }

        function challanNoSchemeSelectedIndexChanged() {
            var schemeValue = cchallanNoScheme.GetValue();
            if (schemeValue == null) {
                ctxtChallanNo.SetEnabled(false);
                ctxtChallanNo.SetText('');
            }
            else if (schemeValue.split('~')[1] == '1') {
                ctxtChallanNo.SetEnabled(false);
                ctxtChallanNo.SetText('Auto');
            }
            else if (schemeValue.split('~')[1] == '0') {
                ctxtChallanNo.SetEnabled(true);
                ctxtChallanNo.SetText('');
            }
        }

        function SetMainAccountdr(Id, name, e) {

            $("#mainacdrid").val(Id);
            cbtnMAdr.SetText(name);
            $('#MainAccountModeldr').modal('toggle');
        }
        $(document).ready(function () {
            $('#MainAccountModel').on('shown.bs.modal', function () {
                $('#txtMainAccountSearch').val("");
                $('#txtMainAccountSearch').focus();
            })
            $('#SubAccountModel').on('shown.bs.modal', function () {
                $('#txtSubAccountSearch').val("");
                $('#txtSubAccountSearch').focus();
            })
            $('#CustModel').on('shown.bs.modal', function () {
                $('#txtCustSearch').focus();
            })

            $('#CustModel').on('hide.bs.modal', function () {
                cbtnSaveInfluencer.SetFocus();
            })

            $('#VehicleModel').on('shown.bs.modal', function () {
                $('#txtVechileSearch').focus();
            })

            $('#VehicleModel').on('hide.bs.modal', function () {
                cbtnSaveInfluencer.SetFocus();
            })

            //cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');

            //if (key != null && key != '' && type != "") {
            //    cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type + '~' + inventory);
            //}

            //Chinmoy added below line
            // cddl_PosGst.SetEnabled(false);

            var componentType = gridquotationLookup.GetValue();//gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());
            if (componentType != null && componentType != '') {
                grid.PerformCallback('GridBlank');
                cddl_PosGst.SetEnabled(true);
            }

            //$('#ddl_numberingScheme').change(function () {

            //    if ($('#ddl_numberingScheme').val() == "") {
            //        return;
            //    }

            //    var NoSchemeTypedtl = $(this).val();
            //    debugger;
            //    var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];
            //    var quotelength = NoSchemeTypedtl.toString().split('~')[2];



            //    if ($('#ddl_numberingScheme').val().split('~')[1] == "0") {
            //        //tstartdate.SetEnabled(true);
            //    } else {
            //        if ($("#ISAllowBackdatedEntry").val() == "No") {
            //            //tstartdate.SetEnabled(false);
            //        }
            //        // //tstartdate.SetDate(new Date);
            //        //cdeliveryDate.SetDate(//tstartdate.GetDate());
            //    }

            //    var fromdate = NoSchemeTypedtl.toString().split('~')[5];
            //    var todate = NoSchemeTypedtl.toString().split('~')[6];

            //    var dt = new Date();

            //    //tstartdate.SetDate(dt);

            //    if (dt < new Date(fromdate)) {
            //        //tstartdate.SetDate(new Date(fromdate));
            //    }

            //    if (dt > new Date(todate)) {
            //        //tstartdate.SetDate(new Date(todate));
            //    }




            //    //tstartdate.SetMinDate(new Date(fromdate));
            //    //tstartdate.SetMaxDate(new Date(todate));

            //    var branchID = (NoSchemeTypedtl.toString().split('~')[3] != null) ? NoSchemeTypedtl.toString().split('~')[3] : "";

            //    //  if (document.getElementById('HdPosType').value == "Fin") {
            //    //    if (ccmbFinancer.InCallback())
            //    //      lastFinancer = branchID;
            //    //    else
            //    //    ccmbFinancer.PerformCallback(branchID);
            //    //  }

            //    //if (branchID != "") document.getElementById('ddl_Branch').value = branchID;
            //    // if (document.getElementById('HdPosType').value != 'Crd') {
            //    // $('#HdSelectedBranch').val(branchID);
            //    // }




            //    if (NoSchemeType == '1') {
            //        ctxt_PLQuoteNo.SetText('Auto');
            //        ctxt_PLQuoteNo.SetEnabled(false);
            //        //ctxt_PLQuoteNo.SetClientEnabled(false);

            //        //tstartdate.Focus();
            //    }
            //    else if (NoSchemeType == '0') {
            //        ctxt_PLQuoteNo.SetEnabled(true);
            //        ctxt_PLQuoteNo.GetInputElement().maxLength = quotelength;
            //        //ctxt_PLQuoteNo.SetClientEnabled(true);
            //        ctxt_PLQuoteNo.SetText('');
            //        ctxt_PLQuoteNo.Focus();
            //    }
            //    else if (NoSchemeType == '2') {
            //        ctxt_PLQuoteNo.SetText('Datewise');
            //        ctxt_PLQuoteNo.SetEnabled(false);
            //        //ctxt_PLQuoteNo.SetClientEnabled(false);

            //        //tstartdate.Focus();
            //    }
            //    else {
            //        ctxt_PLQuoteNo.SetText('');
            //        ctxt_PLQuoteNo.SetEnabled(false);
            //        //ctxt_PLQuoteNo.SetClientEnabled(true);
            //    }
            //});

            $('#ddl_numberingScheme').change(function () {

                if ($('#ddl_numberingScheme').val() == "") {
                    return;
                }

                var NoSchemeTypedtl = $(this).val();
                debugger;
                var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];
                var quotelength = NoSchemeTypedtl.toString().split('~')[2];





                var fromdate = NoSchemeTypedtl.toString().split('~')[5];
                var todate = NoSchemeTypedtl.toString().split('~')[6];

                var dt = new Date();

                //tstartdate.SetDate(dt);







                var branchID = (NoSchemeTypedtl.toString().split('~')[3] != null) ? NoSchemeTypedtl.toString().split('~')[3] : "";

                //  if (document.getElementById('HdPosType').value == "Fin") {
                //    if (ccmbFinancer.InCallback())
                //      lastFinancer = branchID;
                //    else
                //    ccmbFinancer.PerformCallback(branchID);
                //  }

                //if (branchID != "") document.getElementById('ddl_Branch').value = branchID;
                //// if (document.getElementById('HdPosType').value != 'Crd') {
                //$('#HdSelectedBranch').val(branchID);
                //// }

                //if ($('#hdBasketId').val() != "") {
                //    document.getElementById('hdnPageStatus').value = 'Rebindbasketgrid';
                //    grid.PerformCallback('rebindgridFromBasket');
                //}

                //if (cddl_SalesAgent.InCallback())
                //    lastSalesman = branchID;
                //// else
                ////  cddl_SalesAgent.PerformCallback(branchID);
                //GetAllDetailsByBranch();

                // loadMainAccountByBranchIdForPayDet(branchID);

                if (cchallanNoScheme.InCallback())
                    lastChallan = 'BindChallanScheme~' + NoSchemeTypedtl.toString().split('~')[3];
                else
                    cchallanNoScheme.PerformCallback('BindChallanScheme~' + NoSchemeTypedtl.toString().split('~')[3])


                //ctxt_PLQuoteNo.SetMaxLength(quotelength);
                if (NoSchemeType == '1') {
                    ctxt_PLQuoteNo.SetText('Auto');
                    ctxt_PLQuoteNo.SetEnabled(false);
                    //ctxt_PLQuoteNo.SetClientEnabled(false);

                    //tstartdate.Focus();
                }
                else if (NoSchemeType == '0') {
                    ctxt_PLQuoteNo.SetEnabled(true);
                    ctxt_PLQuoteNo.GetInputElement().maxLength = quotelength;
                    //ctxt_PLQuoteNo.SetClientEnabled(true);
                    ctxt_PLQuoteNo.SetText('');
                    ctxt_PLQuoteNo.Focus();
                }
                else if (NoSchemeType == '2') {
                    ctxt_PLQuoteNo.SetText('Datewise');
                    ctxt_PLQuoteNo.SetEnabled(false);
                    //ctxt_PLQuoteNo.SetClientEnabled(false);

                    //tstartdate.Focus();
                }
                else {
                    ctxt_PLQuoteNo.SetText('');
                    ctxt_PLQuoteNo.SetEnabled(false);
                    //ctxt_PLQuoteNo.SetClientEnabled(true);
                }
            });


        });
        function percenLostfocus(id) {
            if ($("#prodpercen" + id).val() == "") {
                $("#prodpercen" + id).val('0.00')
            }

            if ($("#prodappl" + id).val() == "1") {

                var qty = $("#prodqty" + id).html();
                var salesprice = $("#prodsp" + id).html();
                var percen = $("#prodpercen" + id).val();

                if (DecimalRoundoff(percen, 2) > DecimalRoundoff(salesprice, 2)) {

                    jAlert('Amount per quantity can not be greater than sales price', 'Alert');
                    $("#prodpercen" + id).val("0.00");
                }
                else {
                    var amt = (DecimalRoundoff(qty, 2) * DecimalRoundoff(percen, 2)).toFixed(2);
                    $("#prodcommamt" + id).val(amt);
                }


            }
            else if ($("#prodappl" + id).val() == "2") {

                var qty = $("#prodqty" + id).html();
                var salesprice = $("#prodsp" + id).html();
                var percen = $("#prodpercen" + id).val();
                var invamt = $("#proddetamt" + id).html();



                if (DecimalRoundoff(percen, 2) > 100) {

                    jAlert('Amount per quantity can not be greater than sales price', 'Alert');
                    $("#prodpercen" + id).val("0.00");
                }
                else {
                    var amt = (DecimalRoundoff(invamt, 2) * DecimalRoundoff(percen, 2) * 0.01).toFixed(2);
                    $("#prodcommamt" + id).val(amt);
                }

            }
            else if ($("#prodappl" + id).val() == "3") {

                var qty = $("#prodqty" + id).html();
                var salesprice = $("#prodsp" + id).html();
                var percen = $("#prodpercen" + id).val();
                var invamt = DecimalRoundoff(qty, 2) * DecimalRoundoff(salesprice, 2);



                if (DecimalRoundoff(percen, 2) > 100) {

                    jAlert('Amount per quantity can not be greater than sales price', 'Alert');
                    $("#prodpercen" + id).val("0.00");
                }
                else {
                    var amt = (DecimalRoundoff(invamt, 2) * DecimalRoundoff(percen, 2) * 0.01).toFixed(2);
                    $("#prodcommamt" + id).val(amt);
                }

            }

            var sum = 0;
            $("#tableProduct tbody tr").each(function () {
                var obj = $(this).find("td")[0].innerHTML.trim();
                sum = sum + DecimalRoundoff($("#prodcommamt" + obj).val(), 2);
            })

            $("#txtCommAmt").html(sum);
            ReclaculateTotal();


        }

        function ReclaculateTotal() {
            var trcount = $('#influencerGrid tr').length - 1;
            var DET_AMOUNT_CR = (DecimalRoundoff($("#txtCommAmt").html().trim(), 2) / parseInt(trcount)).toFixed(2);
            var sum = 0;
            $("#infbody tr").each(function () {
                var obj = $(this).find("td")[0].innerHTML.trim();
                $("#txtinfamt" + obj).val(DET_AMOUNT_CR);
                if (trcount > 1) {
                    $("#txtinfamt" + obj).prop("disabled", false);
                }
                else {
                    $("#txtinfamt" + obj).prop("disabled", true);

                }
            })
        }


        function AmountLostfocus(id) {

            var sum = 0;
            $("#tableProduct tbody tr").each(function () {
                var obj = $(this).find("td")[0].innerHTML.trim();
                sum = sum + DecimalRoundoff($("#prodcommamt" + obj).val(), 2);
            })

            $("#txtCommAmt").html(sum);
            ReclaculateTotal();
            //if ($("#prodappl" + id).val() == "1") {

            //    var qty = $("#prodqty" + id).html();
            //    var salesprice = $("#prodsp" + id).html();
            //    var amt = $("#prodcommamt" + id).val();
            //    var lastamt = (DecimalRoundoff(amt, 2) / DecimalRoundoff(qty, 2)).toFixed(2);



            //    if (DecimalRoundoff(lastamt, 2) > DecimalRoundoff(salesprice, 2)) {

            //        jAlert('Amount per quantity can not be greater than sales price', 'Alert');
            //        $("#prodcommamt" + id).val("0.00");
            //        $("#prodpercen" + id).val("0.00");
            //    }
            //    else {

            //        $("#prodpercen" + id).val(lastamt);
            //    }


            //}
            //else if ($("#prodappl" + id).val() == "2") {

            //    var qty = $("#prodqty" + id).html();
            //    var salesprice = $("#prodsp" + id).html();
            //    var prodamt = $("#prodcommamt" + id).val();
            //    var invamt = $("#proddetamt" + id).html();

            //    var percent = ((DecimalRoundoff(invamt, 2) / DecimalRoundoff(prodamt, 2)) * 100).toFixed(2);


            //    if (DecimalRoundoff(prodamt, 2) > DecimalRoundoff(invamt, 2)) {

            //        jAlert('Amount per quantity can not be greater than sales price', 'Alert');
            //        $("#prodcommamt" + id).val("0.00");
            //        $("#prodpercen" + id).val("0.00");
            //    }
            //    else {

            //        $("#prodcommamt" + id).val(percent);
            //    }

            //}
            //else if ($("#prodappl" + id).val() == "3") {

            //    var qty = $("#prodqty" + id).html();
            //    //var salesprice = $("#prodsp" + id).html();
            //    var prodamt = $("#prodcommamt" + id).val();
            //    var invamt = $("#prodsp" + id).html();

            //    var percent = ((DecimalRoundoff(invamt, 2) / DecimalRoundoff(prodamt, 2)) * 100).toFixed(2);


            //    if (DecimalRoundoff(prodamt, 2) > DecimalRoundoff(invamt, 2)) {

            //        jAlert('Amount per quantity can not be greater than sales price', 'Alert');
            //        $("#prodcommamt" + id).val("0.00");
            //        $("#prodpercen" + id).val("0.00");
            //    }
            //    else {

            //        $("#prodcommamt" + id).val(percent);
            //    }

            //}






            //var sum = 0;
            //$("#tableProduct tbody tr").each(function () {
            //    var obj = $(this).find("td")[0].innerHTML.trim();
            //    sum = sum + DecimalRoundoff($("#prodcommamt" + obj).val(), 2);
            //})

            //$("#txtCommAmt").html(sum);


            //ReclaculateTotal();
        }

        function infdeleteClick(infid) {
            var row = document.getElementById("tr" + infid);
            row.parentNode.removeChild(row);


            var trcount = $('#influencerGrid tr').length - 1;
            var DET_AMOUNT_CR = DecimalRoundoff($("#txtCommAmt").html().trim(), 2) / parseInt(trcount);


            $("#infbody tr").each(function () {
                var obj = $(this).find("td")[0].innerHTML.trim();
                $("#infamt" + obj).html(DET_AMOUNT_CR);
            })
        }

        function infSchemedeleteClick(infid) {
            var row = document.getElementById("tr" + infid);
            row.parentNode.removeChild(row);


            var trcount = $('#tableInfluencer tr').length - 1;
            // var DET_AMOUNT_CR = DecimalRoundoff($("#txtCommAmt").html().trim(), 2) / parseInt(trcount);


            $("#infSchemebody tr").each(function () {
                var obj = $(this).find("td")[0].innerHTML.trim();
                //$("#infamt" + obj).html(DET_AMOUNT_CR);
            })
        }



        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            console.log(charCode);
            if (charCode == 46)
                return true;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        function ChangeType(id) {
            // alert(id);
            if ($("#prodappl" + id).val() == "4") {

                $("#prodpercen" + id).prop('disabled', true);
                $("#prodpercen" + id).val("0.00");
                $("#prodcommamt" + id).val("0.00");
                $("#prodcommamt" + id).prop('disabled', false);
            }
            else {

                $("#prodpercen" + id).prop('disabled', false);
                $("#prodpercen" + id).val("0.00");
                $("#prodcommamt" + id).val("0.00");
                $("#prodcommamt" + id).prop('disabled', true);
            }

            var sum = 0;
            $("#tableProduct tbody tr").each(function () {
                var obj = $(this).find("td")[0].innerHTML.trim();
                sum = sum + DecimalRoundoff($("#prodcommamt" + obj).val(), 2);
            })

            $("#txtCommAmt").html(sum);

            ReclaculateTotal();
        }

        function MassBranchCustomButtonClick(s, e) {
            if (e.buttonID == 'CancelAssignment') {
                GlobalRowIndex = e.visibleIndex;
                cmassBranch.batchEditApi.StartEdit(GlobalRowIndex);
                cmassBranch.GetEditor('pos_assignBranch').SetValue(0);
                BranchChangeOnMassChange(cmassBranch.GetEditor('pos_assignBranch'));
            }
            else if (e.buttonID == 'ShowStock') {
                cAssignmentGrid.PerformCallback('0~0');
                GlobalRowIndex = e.visibleIndex;
                SelectedInvoiceId = cmassBranch.GetRowKey(GlobalRowIndex);
                $('#BranchAssignmentHeader').hide();
                cAssignmentPopUp.SetHeaderText('Show Stock');
                cAssignmentPopUp.Show();
            }


        }

        function gridFocusedRowChanged(s, e) {
            GlobalRowIndex = e.visibleIndex;
        }





        function BranchChangeOnMassChange(s, e) {

            var AssignedBranch = {
                InvoiceId: '',
                BranchId: ''
            }
            AssignedBranch.InvoiceId = cmassBranch.GetRowKey(GlobalRowIndex);
            AssignedBranch.BranchId = s.GetValue();
            for (var ind = 0; ind < BranchMassListByKeyValue.length; ind++) {
                if (BranchMassListByKeyValue[ind].InvoiceId == AssignedBranch.InvoiceId) {
                    BranchMassListByKeyValue.pop(ind);
                }
            }


            BranchMassListByKeyValue.push(AssignedBranch);
        }


        function MassBranchAssign() {
            //cmassBranch.Refresh();
            //cmassBranchPopup.Show();
            var url = '/OMS/Management/Activities/PosMassBranch.aspx';
            cmassBranchPopup.SetContentUrl(url);
            cmassBranchPopup.Show();

            //}
            return true;
        }

        function BranchAssignmentBranchSelectedIndexChanged() {
            cAssignedBranch.SetValue(cBranchAssignmentBranch.GetValue());
            //    AssignedBranchSelectedIndexChanged(cBranchAssignmentBranch);
            cAssignedWareHouse.PerformCallback(cAssignedBranch.GetValue());
        }

        function AssignmentGridEndCallback() {
            if (cAssignmentGrid.cpMsg) {
                if (cAssignmentGrid.cpMsg != '') {
                    jAlert(cAssignmentGrid.cpMsg, 'Alert', function () {
                        cAssignmentPopUp.Hide();
                        if (page.activeTabIndex == 0) {
                            //cGrdQuotation.PerformCallback('RefreshGrid');
                            cGrdQuotation.Refresh();
                        }

                    });
                    cAssignmentGrid.cpMsg = null;
                }
            }
        }
        function AssignBranchToThisInvoice() {
            //$('#MandatoryBranchAssign').attr('style', 'display:none');
            //$('#mandetoryAssignedWareHouse').attr('style', 'display:none');


            //if (cAssignedBranch.GetValue() == null || cAssignedBranch.GetValue() == '0') {
            //    $('#MandatoryBranchAssign').attr('style', 'display:block');
            //}
            //else if (cAssignedWareHouse.GetValue() == null) {
            //    $('#mandetoryAssignedWareHouse').attr('style', 'display:block');
            //} else {
            //    cAssignmentGrid.PerformCallback('AssignBranch~' + SelectedInvoiceId + '~' + cAssignedBranch.GetValue() + '~' + cAssignedWareHouse.GetValue());
            //}

            cAssignmentGrid.PerformCallback('AssignBranch~' + SelectedInvoiceId + '~' + cAssignedBranch.GetValue() + '~' + cAssignedWareHouse.GetValue());
        }

        function watingInvoicegridEndCallback() {
            if (cwatingInvoicegrid.cpReturnMsg) {
                if (cwatingInvoicegrid.cpReturnMsg != "") {
                    jAlert(cwatingInvoicegrid.cpReturnMsg);
                    document.getElementById('waitingInvoiceCount').value = parseFloat(document.getElementById('waitingInvoiceCount').value) - 1;
                    cwatingInvoicegrid.cpReturnMsg = null;
                }
            }
        }


        function onViewBranchAssignment(obj) {
            SelectedInvoiceId = obj;
            cAssignmentGrid.PerformCallback('0~0');
            cBranchRequUpdatePanel.PerformCallback(SelectedInvoiceId);
            $('#BranchAssignmentHeader').show();
            cAssignmentPopUp.SetHeaderText('Branch Assignment');
            cAssignmentPopUp.Show();

        }

        function onCopyInvoice(obj, Inv) {
            debugger;
            $('#InvoiceCopyModel').modal('show');
            ctxt_InvoiceNo.SetEnabled(false);
            ctxt_InvoiceNo.SetValue(Inv);

            CopyInvoiceId = Inv;

            // SalesManLoad();
            cddl_SalesAgent.PerformCallback(CopyInvoiceId);

        }

        function SalesManLoad() {
            cddl_SalesAgent.ClearItems();
            var otherdetails = {};
            otherdetails.Inv = CopyInvoiceId;
            $.ajax({
                type: "POST",
                url: "PosSalesInvoiceList.aspx/bindSalesmanByBranch",
                data: JSON.stringify(otherdetails),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var returnObject = msg.d;
                    for (var i = 0; i < returnObject.SalesManDetails.length; i++) {
                        cddl_SalesAgent.AddItem(returnObject.SalesManDetails[i].Name, returnObject.SalesManDetails[i].CNTId);
                    }
                },
                error: function (msg) {
                    var ss = msg.d;
                    alert(msg);
                }
            });

        }


        function onAddVehicle(obj) {
            //SelectedInvoiceId = obj;
            //cAssignmentGrid.PerformCallback('0~0');
            //cBranchRequUpdatePanel.PerformCallback(SelectedInvoiceId);
            //$('#BranchAssignmentHeader').show();
            //cAssignmentPopUp.SetHeaderText('Branch Assignment');
            //cAssignmentPopUp.Show();
            $("#invid").val(obj);
            $.ajax({
                type: "POST",
                url: "PosSalesInvoiceList.aspx/viewDelavery",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ invoice: obj }),
                dataType: "json",
                success: function (msg) {
                    var status = msg.d;
                    var str = "";

                    if (status != null) {

                        $("#txtRemarks").val(status.Remarks);
                        ctxt_EwayValue.SetValue(status.EwayValu);
                        ctxt_EbillNo.SetValue(status.ENo);
                        ccmbOtehrChrgs.SetValue(status.CmbOtehrChrgs);
                        ccmbPaymentTrms.SetValue(status.cmbPaymentTrms);
                        //gridquotationLookup.grid.Selection.SelectRowByKey(1);
                        // cEwayDate.SetDate(status.PostingDate);
                        if (status.POSTING_DATE != null) {
                            cEwayDate.SetDate(new Date(parseInt(status.POSTING_DATE.substr(6))));
                        }
                    }
                }
            });

            $('#VehicleModel').modal('show');
            cQuotationComponentPanel.PerformCallback();
        }



        function AssignedBranchSelectedIndexChanged() {
            cAssignedWareHouse.PerformCallback(cAssignedBranch.GetValue());

            cBranchAssignmentBranch.SetValue(cAssignedBranch.GetValue());
            //updateAssignmentGrid();
        }

        function updateAssignmentGrid() {
            cAssignmentGrid.PerformCallback(SelectedInvoiceId + '~' + cBranchAssignmentBranch.GetValue());
        }

        function IconChange() {
            $(function () {
                var $tr = $("#GrdQuotation_DXMainTable > tbody > tr");
                $tr.each(function (index, value) {
                    //var $DelvStatus = $(this).find("td").eq(8).text();
                    //var $payType = $(this).find("td").eq(5).text();
                    //var $DelvType = $(this).find("td").eq(6).text();
                    //var $AssignedBranch = $(this).find("td").eq(15).text();
                    //var $ReturnNumber = $(this).find("td").eq(12).text();

                    var $DelvStatus = $(this).find("td").eq(7).text();
                    var $payType = $(this).find("td").eq(4).text();
                    var $DelvType = $(this).find("td").eq(5).text();
                    var $AssignedBranch = $(this).find("td").eq(14).text();
                    var $ReturnNumber = $(this).find("td").eq(11).text();
                    var $ChallanNumber = $(this).find("td").eq(8).text();



                    //var $a_Assignment = $(this).find("td").eq(17).find("#a_Assignment");
                    //var $Cancel_Assignment = $(this).find("td").eq(17).find("#Cancel_Assignment");
                    //var $a_editInvoice = $(this).find("td").eq(17).find("#a_editInvoice");
                    //var $a_delete = $(this).find("td").eq(17).find("#a_delete");]

                    var $a_Assignment = $(this).find("td").eq(17).find("#a_Assignment");
                    var $Cancel_Assignment = $(this).find("td").eq(17).find("#Cancel_Assignment");
                    var $a_editInvoice = $(this).find("td").eq(17).find("#a_editInvoice");
                    var $a_delete = $(this).find("td").eq(17).find("#a_delete");




                    //if ($DelvType.trim() == "Already Delivered") {
                    //    $a_delete.hide();
                    //}
                    //if ($ChallanNumber != "") {
                    //    $a_Assignment.hide();
                    //    $a_editInvoice.hide();
                    //    $a_delete.hide();
                    //}
                    //if ($ReturnNumber.trim() != "") {
                    //    $a_Assignment.hide();
                    //    $a_editInvoice.hide();
                    //    $a_delete.hide();
                    //}
                    //if ($DelvStatus == 'Pending') {
                    //    $a_Assignment.show();
                    //    $a_editInvoice.show();
                    //}
                    //else {
                    //    if ($payType != 'Finance') {
                    //        $a_Assignment.hide();
                    //        $a_editInvoice.hide();
                    //    }
                    //}

                    //if ($DelvStatus == 'Transfered') {
                    //    $Cancel_Assignment.show();
                    //} else {
                    //    $Cancel_Assignment.hide();
                    //}


                    switch ($DelvType.trim()) {
                        case "Our":
                            if ($ChallanNumber.trim() != "") {
                                $a_Assignment.hide();
                                $a_editInvoice.hide();
                                $a_delete.hide();
                                $Cancel_Assignment.hide();
                            }
                            else {
                                //$a_Assignment.show();
                                //$Cancel_Assignment.hide();
                                $a_editInvoice.show();
                                $a_delete.show();
                                if ($DelvStatus == 'Pending') {
                                    $a_Assignment.show();
                                    $a_editInvoice.show();
                                }
                                else {
                                    if ($payType != 'Finance') {
                                        $a_Assignment.hide();
                                        $a_editInvoice.hide();
                                    }
                                }

                                if ($DelvStatus == 'Transfered') {
                                    $Cancel_Assignment.show();
                                } else {
                                    $Cancel_Assignment.hide();
                                }
                            }
                            if ($ReturnNumber.trim() != "") {
                                $a_Assignment.hide();
                                $a_editInvoice.hide();
                                $a_delete.hide();
                                $Cancel_Assignment.hide();
                            }
                            break;
                        case "Self":
                            {
                                if ($ChallanNumber.trim() != "") {
                                    $a_Assignment.hide();
                                    $a_editInvoice.hide();
                                    $a_delete.hide();
                                    $Cancel_Assignment.hide();
                                }
                                else {
                                    //$a_Assignment.show();
                                    //$Cancel_Assignment.hide();
                                    $a_editInvoice.show();
                                    $a_delete.show();
                                    if ($DelvStatus == 'Pending') {
                                        $a_Assignment.show();
                                        $a_editInvoice.show();
                                    }
                                    else {
                                        if ($payType != 'Finance') {
                                            $a_Assignment.hide();
                                            $a_editInvoice.hide();
                                        }
                                    }

                                    if ($DelvStatus == 'Transfered') {
                                        $Cancel_Assignment.show();
                                    } else {
                                        $Cancel_Assignment.hide();
                                    }

                                }
                                if ($ReturnNumber.trim() != "") {
                                    $a_Assignment.hide();
                                    $a_editInvoice.hide();
                                    $a_delete.hide();
                                    $Cancel_Assignment.hide();
                                }
                                break
                            }
                        case "Already Delivered":
                            if ($ChallanNumber.trim() != "") {
                                $a_Assignment.hide();
                                //$a_editInvoice.hide();
                                $a_delete.hide();
                                $Cancel_Assignment.hide();
                            }
                            if ($ReturnNumber.trim() != "") {
                                $Cancel_Assignment.hide();
                                $a_Assignment.hide();
                                //$a_editInvoice.hide();
                                $a_delete.hide();
                            }
                            break;
                    }
                    if ($AssignedBranch.trim() != "") {
                        $a_delete.hide();
                        //$a_editInvoice.hide();
                    }

                });
            });
        }




        $(document).ready(function () {
            IconChange();


        });
        function OnBeginAfterCallback(s, e) {
            IconChange();
        }



        function updateGridByDate() {
            if (cFormDate.GetDate() == null) {
                jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
            }
            else if (ctoDate.GetDate() == null) {
                jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
            }
            else if (ccmbBranchfilter.GetValue() == null) {
                jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
            }
            else {

                localStorage.setItem("PosListFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("PosListToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("PosListBranch", ccmbBranchfilter.GetValue());

                $('#branchName').text(ccmbBranchfilter.GetText());
                PopulateCurrentBankBalance(ccmbBranchfilter.GetValue());
                if (page.activeTabIndex == 0) {
                    //cGrdQuotation.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
                    $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                    $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                    $("#hfBranchID").val(ccmbBranchfilter.GetValue());
                    $("#hfIsFilter").val("Y");
                    cGrdQuotation.Refresh();
                }
                else if (page.activeTabIndex == 1) {

                }
                if (page.activeTabIndex == 2) {

                }
            }
        }
        function InvoiceWattingOkClick() {
            var index = cwatingInvoicegrid.GetFocusedRowIndex();
            var listKey = cwatingInvoicegrid.GetRowKey(index);
            if (listKey) {
                if (cwatingInvoicegrid.GetRow(index).children[6].innerText != "Advance") {
                    var url = $("#hdnInvoiceType").val() + '?key=' + 'ADD&&BasketId=' + listKey;
                    LoadingPanel.Show();
                    window.location.href = url;
                } else {
                    ShowbasketReceiptPayment(listKey);
                }

            }
        }
        function cSelectPanelEndCall(s, e) {
            debugger;
            if (cSelectPanel.cpSuccess != null) {
                var TotDocument = cSelectPanel.cpSuccess.split(',');
                var reportName = cCmbDesignName.GetValue();
                var module = 'Invoice_POS';
                if (TotDocument.length > 0) {
                    for (var i = 0; i < TotDocument.length; i++) {
                        if (TotDocument[i] != "") {
                            //if (cCmbDesignName.GetValue() == 1) {
                            //    window.open("../../reports/XtraReports/Viewer/InvoiceReportViewer.aspx?id=" + InvoiceId + '&PrintOption=' + TotDocument[i], '_blank')
                            //}
                            //else if (cCmbDesignName.GetValue() == 2) {
                            //    window.open("../../reports/XtraReports/Viewer/TaxInvoiceReportViewer.aspx?id=" + InvoiceId + '&PrintOption=' + TotDocument[i], '_blank')
                            //}
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + InvoiceId + '&PrintOption=' + TotDocument[i], '_blank')
                        }
                    }
                }
            }
            //cSelectPanel.cpSuccess = null
            if (cSelectPanel.cpSuccess == "") {
                if (cSelectPanel.cpChecked != "") {
                    jAlert('Please check Original For Recipient and proceed.');
                }
                //CselectDuplicate.SetEnabled(false);
                //CselectTriplicate.SetEnabled(false);
                CselectOriginal.SetCheckState('UnChecked');
                CselectDuplicate.SetCheckState('UnChecked');
                CselectFDuplicate.SetCheckState('UnChecked');
                CselectTriplicate.SetCheckState('UnChecked');
                cCmbDesignName.SetSelectedIndex(0);
            }
        }
        function PerformCallToGridBind() {
            //cSelectPanel.PerformCallback();

            cSelectPanel.PerformCallback('Bindsingledesign~' + InvoiceId);
            cDocumentsPopup.Hide();
            return false;
        }
        //function OrginalCheckChange(s, e) {
        //    debugger;
        //    if (s.GetCheckState() == 'Checked') {
        //        CselectDuplicate.SetEnabled(true);
        //    }
        //    else {
        //        CselectDuplicate.SetCheckState('UnChecked');
        //        CselectDuplicate.SetEnabled(false);
        //        CselectTriplicate.SetCheckState('UnChecked');
        //        CselectTriplicate.SetEnabled(false);
        //    }

        //}
        //function DuplicateCheckChange(s, e) {
        //    if (s.GetCheckState() == 'Checked') {
        //        CselectTriplicate.SetEnabled(true);
        //    }
        //    else {
        //        CselectTriplicate.SetCheckState('UnChecked');
        //        CselectTriplicate.SetEnabled(false);
        //    }

        //}
        var InvoiceId = 0;
        function onPrintJv(id, RowIndex) {
            InvoiceId = id;
            cSelectPanel.cpSuccess = "";
            cDocumentsPopup.Show();
            //$('#HdInvoiceType').val(cGrdQuotation.GetRow(RowIndex).children[5].innerText);
            $('#HdInvoiceType').val(cGrdQuotation.GetRow(RowIndex).children[4].innerText);
            //CselectDuplicate.SetEnabled(false);
            //CselectTriplicate.SetEnabled(false);
            CselectOriginal.SetCheckState('UnChecked');
            CselectDuplicate.SetCheckState('UnChecked');
            CselectFDuplicate.SetCheckState('UnChecked');
            CselectTriplicate.SetCheckState('UnChecked');
            cCmbDesignName.SetSelectedIndex(0);
            cSelectPanel.PerformCallback('Bindalldesignes~' + InvoiceId);
            $('#btnOK').focus();
        }

        function onPrintJvIST(id, RowIndex) {
            InvoiceId = id;
            cSelectPanel.cpSuccess = "";
            cDocumentsPopup.Show();




            $('#HdInvoiceType').val('Stock Transfer');
            //CselectDuplicate.SetEnabled(false);
            //CselectTriplicate.SetEnabled(false);
            CselectOriginal.SetCheckState('UnChecked');
            CselectDuplicate.SetCheckState('UnChecked');
            CselectFDuplicate.SetCheckState('UnChecked');
            CselectTriplicate.SetCheckState('UnChecked');
            cCmbDesignName.SetSelectedIndex(0);
            cSelectPanel.PerformCallback('Bindalldesignes~' + InvoiceId);
            $('#btnOK').focus();
        }

        function OnWaitingGridKeyPress(e) {

            if (e.code == "Enter") {
                var index = cwatingInvoicegrid.GetFocusedRowIndex();
                var listKey = cwatingInvoicegrid.GetRowKey(index);
                if (listKey) {
                    if (cwatingInvoicegrid.GetRow(index).children[6].innerText != "Advance") {
                        var url = $("#hdnInvoiceType").val() + '?key=' + 'ADD&&BasketId=' + listKey;
                        LoadingPanel.Show();
                        window.location.href = url;
                    } else {
                        ShowbasketReceiptPayment(listKey);
                    }
                }
            }

        }
        function RemoveInvoice(obj) {
            if (obj) {
                jConfirm("Clicking on Delete will not allow to use this Billing request again. Are you sure?", "Alert", function (ret) {
                    if (ret) {
                        cwatingInvoicegrid.PerformCallback('Remove~' + obj);
                    }
                });

            }
        }


        function ShowReceiptPayment() {
            uri = "CustomerReceipt.aspx?key=ADD&IsTagged=Y";
            capcReciptPopup.SetContentUrl(uri);
            capcReciptPopup.SetHeaderText("Add Money Receipt");
            capcReciptPopup.Show();
        }

        function ShowbasketReceiptPayment(id) {
            uri = "CustomerReceiptPayment.aspx?key=ADD&IsTagged=Y&&basketId=" + id;
            capcReciptPopup.SetContentUrl(uri);
            capcReciptPopup.SetHeaderText("Add Money Receipt");
            capcReciptPopup.Show();
        }

        function timerTick() {
            //   cwatingInvoicegrid.Refresh();


            $.ajax({
                type: "POST",
                url: "PosSalesInvoiceList.aspx/GetTotalWatingInvoiceCount",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var status = msg.d;
                    console.log(status);
                    clblweatingCount.SetText(status);
                    var fetcheddata = parseFloat(document.getElementById('waitingInvoiceCount').value);
                    if (status != fetcheddata) {
                        cwatingInvoicegrid.Refresh();
                        document.getElementById('waitingInvoiceCount').value = status;
                    }
                }
            });

        }
        function InvoiceWatingClick() {

            waitingPopUp.Show();
            cwatingInvoicegrid.Focus();
        }
        function ListRowClicked(s, e) {

            var index = e.visibleIndex;
            var listKey = cwatingInvoicegrid.GetRowKey(index);
            if (e.htmlEvent.target.id != "CloseRemoveWattingBtn") {
                if (cwatingInvoicegrid.GetRow(index).children[6].innerText != "Advance") {
                    var url = $("#hdnInvoiceType").val() + '?key=' + 'ADD&&BasketId=' + listKey;
                    LoadingPanel.Show();
                    window.location.href = url;
                } else {
                    ShowbasketReceiptPayment(listKey);
                }
            }
        }
        document.onkeydown = function (e) {
            if (event.keyCode == 73 && event.altKey == true) {
                StopDefaultAction(e);
                InvoiceWatingClick();
            }
            else if (event.keyCode == 67 && event.altKey == true) {
                StopDefaultAction(e);
                OnAddInvoiceButtonClick('Cash');
            }
            else if (event.keyCode == 68 && event.altKey == true) {
                StopDefaultAction(e);
                OnAddInvoiceButtonClick('Crd');
            }
            else if (event.keyCode == 70 && event.altKey == true) {
                StopDefaultAction(e);
                OnAddInvoiceButtonClick('Fin');
            }
            else if (event.keyCode == 82 && event.altKey == true) {
                StopDefaultAction(e);
                ShowReceiptPayment();
            }
            else if (event.keyCode == 77 && event.altKey == true) {
                StopDefaultAction(e);
                MassBranchAssign();
            }
            else if (event.keyCode == 83 && event.altKey == true) {
                StopDefaultAction(e);
                if (cmassBranchPopup.IsVisible()) {
                    MassBranchAssignSaveClick();
                }
                else {
                    OnAddInvoiceButtonClick('IST');
                }
            }

            else if (event.keyCode == 79 && event.altKey == true) {
                if (waitingPopUp.IsVisible()) {
                    StopDefaultAction(e);
                    var index = cwatingInvoicegrid.GetFocusedRowIndex();
                    var listKey = cwatingInvoicegrid.GetRowKey(index);
                    if (listKey) {
                        if (cwatingInvoicegrid.GetRow(index).children[6].innerText != "Advance") {
                            var url = $("#hdnInvoiceType").val() + '?key=' + 'ADD&&BasketId=' + listKey;
                            LoadingPanel.Show();
                            window.location.href = url;
                        } else {
                            ShowReceiptPayment();
                        }
                    }
                }
            }
        }
        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }
        function OnAddButtonClick() {
            var url = 'blankpage.aspx?key=' + 'ADD';

            window.location.href = url;
        }
        function OnAddInvoiceButtonClick(obj) {
            eraseCookie('MenuCloseOpen');

            if (obj != "IST") {
                LoadingPanel.Show();
                var url = $("#hdnInvoiceType").val() + '?key=ADD&&type=' + obj;
            } else {
                if ($('#hdIsStockLedger').val() == "no") {
                    jAlert("Ledger for Interstate Stk-Out not mapped in Account Heads. Cannot Proceed.");
                    return false;
                } else {
                    LoadingPanel.Show();
                    var url = $("#hdnInvoiceType").val() + '?key=ADD&&type=' + obj;
                }
            }

            window.location.href = url;
        }
        function openAdvanceReceipt() {
            var url = 'CustomerReceiptPayment.aspx?key=ADD'
            window.location.href = url;
        }
        function OnMoreInfoClick(keyValue) {
            var ActiveUser = '<%=Session["userid"]%>'
            var url = 'PosSalesInvoiceEdit.aspx' + '?key=' + keyValue + '&Permission=3';
            window.location.href = url;

            //if (ActiveUser != null) {
            //    $.ajax({
            //        type: "POST",
            //        url: "SalesInvoiceList.aspx/GetEditablePermission",
            //        data: "{'ActiveUser':'" + ActiveUser + "'}",
            //        contentType: "application/json; charset=utf-8",
            //        dataType: "json",
            //        success: function (msg) {
            //            var status = msg.d;
            //            var url = 'posSalesInvoice.aspx?key=' + keyValue + '&Permission=' + status;
            //            window.location.href = url;
            //        }
            //    });
            //}
        }
        function viewDocument(keyValue) {

            var url = '/OMS/management/Activities/View/Invoice.html?v0.000044?id=' + keyValue;

            cPosView.SetContentUrl(url);
            cPosView.RefreshContentUrl();
            cPosView.Show();

        }



        function OnClickDelete(keyValue) {
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    cGrdQuotation.PerformCallback('Delete~' + keyValue);
                }
            });
        }




        function PopulateCurrentBankBalance(BranchId) {
            var frDate = cFormDate.GetDate().format('yyyy-MM-dd');
            var toDate = ctoDate.GetDate().format('yyyy-MM-dd');

            $.ajax({
                type: "POST",
                url: 'PosSalesInvoicelist.aspx/GetCurrentBankBalance',
                data: JSON.stringify({ BranchId: BranchId, fromDate: frDate, todate: toDate }),

                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;

                    if (msg.d.length > 0) {
                        document.getElementById("pageheaderContent").style.display = 'block';
                        if (msg.d.split('~')[0] != '') {

                            document.getElementById('<%=B_BankBalance.ClientID %>').innerHTML = msg.d.split('~')[0];
                            document.getElementById('<%=B_BankBalance.ClientID %>').style.color = "Black";
                        }
                        else {
                            document.getElementById('<%=B_BankBalance.ClientID %>').innerHTML = '0.0';
                            document.getElementById('<%=B_BankBalance.ClientID %>').style.color = "Black";

                        }
                    }

                },

            });

        }
        function disp_prompt(name) {
            if (name == "tab0") {
                //document.location.href="crm_sales.aspx"; 
            }
            if (name == "tab1") {
                document.location.href = "PosInvoiceList.aspx?tab=tab1";
            }
            if (name == "tab2") {
                document.location.href = "PosInvoiceList.aspx?tab=tab2";
            }

        }
        function InfluencerBtnClick(s, e) {
            $('#CustModel').modal('show');
        }

        function InfluencerBtnClickAdjustment(s, e) {
            $('#InfluencerModel').modal('show');
        }

        function InfluencerSchemebtnClick(s, e) {
            $('#InfluencerSchemeModel').modal('show');
        }

        function InfluencerKeyDownAdjustment(s, e) {
            if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
                $('#InfluencerModel').modal('show');
            }
        }
        function InfluencerSchemeKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
                $('#InfluencerSchemeModel').modal('show');
            }
        }



        function InfluencerKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
                $('#CustModel').modal('show');
            }
        }

        function VechileBtnClick(s, e) {
            $('#VechielModel').modal('show');
        }

        function VechileKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
                $('#VechielModel').modal('show');
            }
        }

        function btnMainAccountClick(s, e) {
            $('#MainAccountModel').modal('show');
        }

        function btnMainAccountKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
                $('#MainAccountModel').modal('show');
            }
        }


        function btnMAdrClick(s, e) {
            $('#MainAccountModeldr').modal('show');
        }

        function btnMAdrKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
                $('#MainAccountModeldr').modal('show');
            }
        }


        function CloseGridQuotationLookup() {

            gridquotationLookup.ConfirmCurrentSelection();
            gridquotationLookup.HideDropDown();
            // gridquotationLookup.Focus();
        }

        function LoadOldSelectedKeyvalue() {
            var x = gridquotationLookup.gridView.GetSelectedKeysOnPage();
            var Ids = "";
            for (var i = 0; i < x.length; i++) {
                Ids = Ids + ',' + x[i];
            }
            document.getElementById('OldSelectedKeyvalue').value = Ids;
        }

        function BeginComponentCallback() {
        }


        function componentEndCallBack(s, e) {

            gridquotationLookup.gridView.Refresh();
            //if (grid.GetVisibleRowsOnPage() == 0) {
            //    OnAddNewClick();
            //}

            if (cQuotationComponentPanel.cpRebindGridQuote && cQuotationComponentPanel.cpRebindGridQuote != "") {
                ctxt_InvoiceDate.SetText(cQuotationComponentPanel.cpRebindGridQuote);
                cQuotationComponentPanel.cpRebindGridQuote = null;
            }


            if (cQuotationComponentPanel.cpDetails != null) {
                var details = cQuotationComponentPanel.cpDetails;
                cQuotationComponentPanel.cpDetails = null;

                //var SpliteDetails = details.split("~");
                //var Reference = SpliteDetails[0];
                //var Currency_Id = SpliteDetails[1];
                //var SalesmanId = SpliteDetails[2];
                //var ExpiryDate = SpliteDetails[3];
                //var CurrencyRate = SpliteDetails[4];
                //var Type = SpliteDetails[5];
                //var CreditDays = SpliteDetails[6];
                //var DueDate = SpliteDetails[7];
                //var SalesmanName = SpliteDetails[8];
                //if (Type == "SO") {
                //    if (DueDate != "" && CreditDays != "") {
                //        ctxtCreditDays.SetValue(CreditDays);

                //        var Due_Date = new Date(DueDate);
                //        cdt_SaleInvoiceDue.SetDate(Due_Date);
                //    }
                //}

                //ctxt_Refference.SetValue(Reference);
                //ctxt_Rate.SetValue(CurrencyRate);
                //document.getElementById('ddl_Currency').value = Currency_Id;
                //document.getElementById('ddl_SalesAgent').value = SalesmanId;
                <%-- $("#<%=hdnSalesManAgentId.ClientID%>").val(SalesmanId);--%>
                //ctxtSalesManAgent.SetValue(SalesmanName);
                //if (ExpiryDate != "") {
                //    var myDate = new Date(ExpiryDate);
                //    var invoiceDate = new Date();
                //    var datediff = Math.round((myDate - invoiceDate) / (1000 * 60 * 60 * 24));

                //    ctxtCreditDays.SetValue(datediff);
                //    cdt_SaleInvoiceDue.SetDate(myDate);
                //}
            }
        }

        function QuotationNumberChanged() {
            var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();//gridquotationLookup.GetValue();
            quote_Id = quote_Id.join();

            var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";

            if (quote_Id != null) {
                var arr = quote_Id.split(',');

                if (arr.length > 1) {
                    if (type == "QO") {
                        ctxt_InvoiceDate.SetText('Multiple Select Quotation Dates');
                    }
                    else if (type == "SO") {
                        ctxt_InvoiceDate.SetText('Multiple Select Order Dates');
                    }
                    else if (type == "SC") {
                        ctxt_InvoiceDate.SetText('Multiple Select Challan Dates');
                    }
                }
                else {
                    if (arr.length == 1) {
                        // cComponentDatePanel.PerformCallback('BindComponentDate' + '~' + quote_Id + '~' + type);
                    }
                    else {
                        ctxt_InvoiceDate.SetText('');
                    }
                }
            }
            else { ctxt_InvoiceDate.SetText(''); }

            if (quote_Id != null) {
                // cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@');
                // cProductsPopup.Show();
            }
        }


    </script>
    <style>
        .smllpad > tbody > tr > td {
            padding-right: 25px;
        }

        .errorField {
            position: absolute;
            right: 5px;
            top: 9px;
        }

        .padTab {
            margin-bottom: 4px;
        }

            .padTab > tbody > tr > td {
                padding-right: 15px;
                vertical-align: middle;
            }

                .padTab > tbody > tr > td > label, .padTab > tbody > tr > td > input[type="button"] {
                    margin-bottom: 0 !important;
                }

        .backBranch {
            font-weight: 600;
            background: #75c1f5;
            padding: 5px;
        }

        #influencerGrid {
            width: 100% !important;
        }

        #tableProduct_wrapper .dataTables_scroll thead tr th, #influencerGrid_wrapper thead tr th {
            background: #1E6AB1;
            color: #fff;
        }

        .colorTable > thead > tr > th, #influencerGrid > thead > tr > th {
            background: #1E6AB1;
            color: #fff;
        }

        #colorTable, #influencerGrid {
            border-top: 1px solid #1E6AB1;
        }

            #colorTable > thead > tr > th, #influencerGrid > thead > tr > th {
                border-right: 1px solid #1E6AB1;
            }

            #colorTable > tbody > tr > td, #influencerGrid > tbody > tr > td {
                padding: 8px 8px 0 8px;
                vertical-align: middle;
            }

        .padTop25 {
            padding-top: 25px;
        }

        .dataTables_scrollHeadInner, .dataTables_scrollHeadInner > table {
            width: 100% !important;
            margin: 0 auto;
        }

        table.dataTable thead th {
            border-bottom: 0;
        }

        table.dataTable tfoot th {
            border-top: 0;
        }

        table.dataTable tbody th, table.dataTable tbody td {
            padding: 8px 15px;
        }

        .btn.onDxe {
            padding: 0px 5px 4px 5px !important;
            font-size: 18px !important;
        }

        .mroonText {
            color: red;
            background: #ffd3be;
            border: 1px solid #e47743;
            padding: 5px 15px;
            border-radius: 3px;
            margin-top: 26px;
        }

        .btn.btn-xs.typeNotificationBtn {
            position: relative;
            padding-right: 16px !important;
        }

        .typeNotification {
            position: absolute;
            width: 22px;
            height: 22px;
            background: #ff5140;
            display: block;
            font-size: 12px;
            border-radius: 50%;
            right: -7px;
            top: -9px;
            line-height: 22px;
        }
    </style>
    <script>

        function gridcrmCampaignclick(s, e) {
            //alert('hi');
            //IconChange();
            $('#gridcrmCampaign').find('tr').removeClass('rowActive');
            $('.floatedBtnArea').removeClass('insideGrid');
            //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
            $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).addClass('rowActive');
            setTimeout(function () {
                //alert('delay');
                var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
                //    setTimeout(function () {
                //        $(this).fadeIn();
                //    }, 100);
                //});    
                $.each(lists, function (index, value) {
                    //console.log(index);
                    //console.log(value);
                    setTimeout(function () {
                        console.log(value);
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }
    </script>

    <script>
        function InvoiceCustomersButnClick(s, e) {
            $('#InvoiceCustsModel').modal('show');
        }

        function CopyInvoiceCustomerKeydown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#InvoiceCustsModel').modal('show');
            }
        }



        function InvoiceCustomerskeydown(e) {
            var OtherDetails = {}

            if ($.trim($("#InvoicetxtCustsSearch").val()) == "" || $.trim($("#InvoicetxtCustsSearch").val()) == null) {
                return false;
            }
            OtherDetails.SearchKey = $("#InvoicetxtCustsSearch").val();
            OtherDetails.BranchID = $('#ddl_Branch').val();

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Customer Name");
                HeaderCaption.push("Unique Id");
                HeaderCaption.push("Address");

                if ($("#InvoicetxtCustsSearch").val() != "") {
                    callonServer("Services/Master.asmx/GetCustomer", OtherDetails, "InvoiceCustomersTable", HeaderCaption, "InvoiceCustomerIndex", "InvoiceSetCustomer");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[InvoiceCustomerIndex=0]"))
                    $("input[InvoiceCustomerIndex=0]").focus();
            }
        }

        function OnSalesAgentEndCallback(s, e) {
            //if (lastSalesman) {
            //    cddl_SalesAgent.PerformCallback(lastSalesman);
            //    lastSalesman = null;
            //} else {
            //if (!lastSalesman)
            //    cmbUcpaymentCashLedgerChanged(ccmbUcpaymentCashLedger);
            // }
            ctxt_NewInvoiceNo.SetFocus();
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cGrdQuotation.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cGrdQuotation.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cGrdQuotation.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cGrdQuotation.SetWidth(cntWidth);
                }

            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">Sales Invoice (POS)  
                 <%--<div  style="font-size: 14px;"> : <span class="backBranch"><asp:Label runat="server" ID="branchName" Text=""></asp:Label></span></div>--%></h3>
            <div id="pageheaderContent" class="scrollHorizontal pull-right wrapHolder content horizontal-images">
                <div class="Top clearfix">
                    <ul>
                        <li>
                            <div class="lblHolder" id="idCashbalanace">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>Cash Balance </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="width: 100%;">
                                                    <b style="text-align: center" id="B_BankBalance" runat="server">0.00</b>
                                                </div>

                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </li>
                        <li>
                            <div class="lblHolder">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>For Unit </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="width: 100%;">
                                                    <asp:Label runat="server" ID="branchName" Text=""></asp:Label>
                                                </div>

                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </div>



        <table class="padTab" style="margin-top: 5px;">
            <tr>
                <td>
                    <label>From Date</label></td>
                <td>
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>
                    <label>To Date</label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>

                </td>
                <td>Unit</td>
                <td>
                    <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                    </dxe:ASPxComboBox>
                </td>
                <td>
                    <input type="button" value="Show" class="btn btn-primary btn-radius" onclick="updateGridByDate()" />
                    <% if (rights.CanExport)
                       { %>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary pull-right btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLSX</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>
                    <% } %>
                </td>

            </tr>

        </table>


    </div>
    <div class="form_main">
        <div class="clearfix">
            <% if (rights.CanAdd)
               { %>

            <%--   <a href="javascript:void(0);" onclick="OnAddInvoiceButtonClick('Cash')" class="btn btn-success btn-xs hide" id="btnCash"><span>Add <u>C</u>ash Invoice</span> </a>
            <a href="javascript:void(0);" onclick="OnAddInvoiceButtonClick('Crd')" class="btn btn-success btn-xs hide" id="btnCredit"><span>Add Cre<u>d</u>it Invoice</span> </a>
            <a href="javascript:void(0);" onclick="OnAddInvoiceButtonClick('Fin')" class="btn btn-success btn-xs hide" id="btnFin"><span>Add <u>F</u>inance Invoice</span> </a>
            <a href="javascript:void(0);" onclick="OnAddInvoiceButtonClick('IST')" class="btn btn-success btn-xs hide" id="btnIST"><span>Interstate <u>S</u>tock Transfer</span> </a>
            <a href="javascript:void(0);" onclick="ShowReceiptPayment()" class="btn btn-success btn-xs hide" id="btnCRP"><span>Add <u>R</u>eceipt/Payment</span> </a>--%>

            <a href="javascript:void(0);" onclick="OnAddInvoiceButtonClick('Cash')" class="btn btn-success btn-radius" id="btnCash"><span class="btn-icon"><i class="fa fa-plus"></i></span><span>Add <u>C</u>ash Invoice</span> </a>

            <a href="javascript:void(0);" onclick="OnAddInvoiceButtonClick('Crd')" class="btn btn-success btn-radius" id="btnCredit"><span class="btn-icon"><i class="fa fa-plus"></i></span><span>Add Cre<u>d</u>it Invoice</span> </a>
            <a href="javascript:void(0);" onclick="OnAddInvoiceButtonClick('Fin')" class="btn btn-success btn-radius" id="btnFin"><span class="btn-icon"><i class="fa fa-plus"></i></span><span>Add <u>F</u>inance Invoice</span> </a>
            <a href="javascript:void(0);" onclick="OnAddInvoiceButtonClick('IST')" class="btn btn-success btn-radius" id="btnIST"><span class="btn-icon"><i class="fa fa-plus"></i></span><span>Interstate <u>S</u>tock Transfer</span> </a>
            <a href="javascript:void(0);" onclick="ShowReceiptPayment()" class="btn btn-success btn-radius " id="btnCRP"><span class="btn-icon"><i class="fa fa-plus"></i></span><span>Add <u>R</u>eceipt/Payment</span> </a>
            <a href="javascript:void(0);" onclick="onInfluencerReturn()" class="btn btn-success btn-radius " id="btnInfluencerReturn"><span class="btn-icon"><i class="fa fa-plus"></i></span><span>Add Influencer Adjustment</span> </a>
            <a href="javascript:void(0);" onclick="onApprovalList();" class="btn btn-success btn-radius "><span class="btn-icon"><i class="fa fa-plus"></i></span><span>Show Pending POS Status</span> </a>

            <%} %>

            <% if (rights.CanAssignbranch)
               { %>
            <a href="javascript:void(0);" onclick="MassBranchAssign()" class="btn btn-primary btn-radius "><span class="btn-icon"><i class="fa fa-user"></i></span><span><u>M</u>ass Unit Assign</span> </a>
            <%} %>
            <%--<a href="javascript:void(0);" class="btn btn-danger"><span>Delete</span> </a>--%>


            <a href="javascript:void(0);" onclick="InvoiceWatingClick()" class="btn btn-primary btn-radius relative typeNotificationBtn "><span class="btn-icon"><i class="fa fa-file-pdf-o"></i></span><span><u>I</u>nvoice Waiting </span>
                <span class="typeNotification">
                    <dxe:ASPxLabel runat="server" Text="" ID="lblweatingCount" ClientInstanceName="clblweatingCount"></dxe:ASPxLabel>
                </span>
            </a>




            <i class="fa fa-reply blink hide" style="font-size: 20px; margin-right: 10px;" aria-hidden="true"></i>



        </div>
    </div>
     <div id="spnEditLock" runat="server" style="display:none; color:red;text-align:center"></div>
     <div id="spnDeleteLock" runat="server" style="display:none; color:red;text-align:center"></div>

    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxDocumentsPopup" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentStyle VerticalAlign="Top"></ContentStyle>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--   <dxe:ASPxGridView runat="server" KeyFieldName="ID" ClientInstanceName="cgriddocuments" ID="grid_Documents"
                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridDocuments_CustomCallback" ClientSideEvents-EndCallback="cgridDocumentsEndCall"
                        Settings-ShowFooter="false" AutoGenerateColumns="False"
                        Settings-VerticalScrollableHeight="100" Settings-VerticalScrollBarMode="Hidden">
                                                      
                        <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                        <SettingsPager Visible="false"></SettingsPager>
                        <Columns>
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="10" Caption=" "  />




                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="ID" Width="0" ReadOnly="true" Caption="No." CellStyle-CssClass="hide" HeaderStyle-CssClass="hide">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="NAME" Width="" ReadOnly="true" Caption="Design(s)">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                      
                    </dxe:ASPxGridView>--%>
                    <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" ClientSideEvents-EndCallback="cSelectPanelEndCall">
                        <ClientSideEvents EndCallback="cSelectPanelEndCall"></ClientSideEvents>
                        <PanelCollection>
                            <dxe:PanelContent runat="server">



                                <%--   <dxe:ASPxCheckBox ID="selectOriginal" Text="Original" runat="server" ToolTip="Select Original"
                                    ClientInstanceName="CselectOriginal"  ClientSideEvents-CheckedChanged="function(s, e) { 
                                      grid.PerformCallback(s.GetChecked()+'^'+'stock'); }">
                                   </dxe:ASPxCheckBox>--%>

                                <%--<dxe:ASPxCheckBox ID="selectOriginal" Text="Original For Recipient" runat="server" ToolTip="Select Original" ClientSideEvents-CheckedChanged="OrginalCheckChange"
                                    ClientInstanceName="CselectOriginal">
                                </dxe:ASPxCheckBox>

                                <dxe:ASPxCheckBox ID="selectDuplicate" Text="Duplicate For Transporter" runat="server" ToolTip="Select Duplicate" ClientSideEvents-CheckedChanged="DuplicateCheckChange"
                                    ClientInstanceName="CselectDuplicate">
                                </dxe:ASPxCheckBox>--%>
                                <dxe:ASPxCheckBox ID="selectOriginal" Text="Original For Recipient" runat="server" ToolTip="Select Original"
                                    ClientInstanceName="CselectOriginal">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectDuplicate" Text="Duplicate For Transporter" runat="server" ToolTip="Select Duplicate"
                                    ClientInstanceName="CselectDuplicate">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectFDuplicate" Text="Duplicate For Financer" runat="server" ToolTip="Select Duplicate For Financer"
                                    ClientInstanceName="CselectFDuplicate">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectTriplicate" Text="Triplicate For Supplier" runat="server" ToolTip="Select Triplicate"
                                    ClientInstanceName="CselectTriplicate">
                                </dxe:ASPxCheckBox>

                                <dxe:ASPxComboBox ID="CmbDesignName" ClientInstanceName="cCmbDesignName" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                    <%--<Items>
                                        <dxe:ListEditItem Selected="True" Text="Default" Value="1"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Tax_Invoice" Value="2"></dxe:ListEditItem>
                                    </Items>--%>
                                    <%-- <ClientSideEvents ValueChanged="function(s,e){OnCmbCountryName_ValueChange()}"></ClientSideEvents>--%>
                                </dxe:ASPxComboBox>

                                <div class="text-center pTop10">
                                    <dxe:ASPxButton ID="btnOK" ClientInstanceName="cbtnOK" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind();}" />
                                    </dxe:ASPxButton>

                                </div>


                                <asp:HiddenField ID="HdInvoiceType" runat="server" />


                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>



                </dxe:PopupControlContentControl>

            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>

    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" ClientInstanceName="page"
        Font-Size="12px" Width="100%">
        <TabPages>
            <dxe:TabPage Name="POS" Text="POS">
                <ContentCollection>
                    <dxe:ContentControl runat="server">
                        <div class="GridViewArea relative">
                            <%--<dxe:ASPxGridView ID="GrdQuotation" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                Width="100%" ClientInstanceName="cGrdQuotation" OnCustomCallback="GrdQuotation_CustomCallback" SettingsBehavior-AllowFocusedRow="true"
                                SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" OnDataBinding="GrdQuotation_DataBinding" Settings-HorizontalScrollBarMode="Auto"
                                SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true" SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true" SettingsBehavior-ColumnResizeMode="Control"
                                OnSummaryDisplayText="ShowGrid_SummaryDisplayText">--%>
                            <dxe:ASPxGridView ID="GrdQuotation" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                Width="100%" ClientInstanceName="cGrdQuotation" OnCustomCallback="GrdQuotation_CustomCallback" SettingsBehavior-AllowFocusedRow="true"
                                SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
                                SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto"
                                OnSummaryDisplayText="ShowGrid_SummaryDisplayText" DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
                                <SettingsSearchPanel Visible="True" Delay="5000" />
                                <%--SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true" SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true" --%>
                                <ClientSideEvents BeginCallback="OnBeginAfterCallback" />
                                <Columns>
                                    <dxe:GridViewDataTextColumn Caption="Sl No." FieldName="SlNo" Width="50" Visible="false" SortOrder="Descending"
                                        VisibleIndex="0" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Document No." FieldName="InvoiceNo" Width="200"
                                        VisibleIndex="0" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>


                                    <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="Invoice_Date"
                                        VisibleIndex="1" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>



                                    <%--                                    <dxe:GridViewDataHyperLinkColumn Caption="Customer" FieldName="CustomerName"
                                        VisibleIndex="2" Width="300">
                                        <PropertiesHyperLinkEdit Target="_self" >
                                            <ClientSideEvents Click="CustomerClick" />
                                        </PropertiesHyperLinkEdit>
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataHyperLinkColumn>--%>

                                    <dxe:GridViewDataTextColumn FieldName="CustomerName" Caption="Customer" VisibleIndex="2" Width="300">
                                        <DataItemTemplate>
                                            <a href="javascript:void(0);" onclick="CustomerClick(this,'<%# Eval("Customer_Id") %>')">
                                                <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text='<%# Eval("CustomerName")%>'
                                                    ToolTip="Customer Outstanding">
                                                </dxe:ASPxLabel>
                                            </a>

                                        </DataItemTemplate>
                                        <EditFormSettings Visible="False" />
                                        <CellStyle Wrap="False" CssClass="text-center">
                                        </CellStyle>
                                        <%-- <HeaderTemplate>
                                                                                Status
                                                                            </HeaderTemplate>--%>
                                        <Settings AutoFilterCondition="Contains" />
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <%--<Settings AllowAutoFilter="False" />--%>
                                        <HeaderStyle Wrap="False" CssClass="text-center" />

                                    </dxe:GridViewDataTextColumn>




                                    <dxe:GridViewDataTextColumn Caption="Net Amount" FieldName="NetAmount"
                                        VisibleIndex="3">
                                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Type" FieldName="Pos_EntryType" Width="70"
                                        VisibleIndex="4">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>



                                    <dxe:GridViewDataTextColumn Caption="Delv. Type" FieldName="Pos_DeliveryType"
                                        VisibleIndex="4" Width="80">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Delv. Date" FieldName="Pos_DeliveryDate"
                                        VisibleIndex="4">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Delv. Status" FieldName="DelvStatus"
                                        VisibleIndex="4">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Challan No" FieldName="ChallanNo"
                                        VisibleIndex="4" Width="180">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Challan Date" FieldName="ChallanDate"
                                        VisibleIndex="4">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="SRN?" FieldName="isSRN"
                                        VisibleIndex="4" Width="50">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="SRN No." FieldName="SRNno"
                                        VisibleIndex="4">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="SRN Date" FieldName="SrnDate"
                                        VisibleIndex="4">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="CD Date" FieldName="cdDate"
                                        VisibleIndex="4">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Assigned Unit" FieldName="pos_assignBranch"
                                        VisibleIndex="4" Width="350">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Sales Person" FieldName="salesman" Width="150"
                                        VisibleIndex="4">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>


                                    <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="EnteredBy"
                                        VisibleIndex="4">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="0">
                                        <DataItemTemplate>
                                            <div class='floatedBtnArea'>
                                                <% if (rights.CanEdit)
                                                   { %>
                                                <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')" id="a_editInvoice" class="" style='<%#Eval("Editlock")%>' title="">
                                                    <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a><%} %>
                                                <% if (rights.CanDelete)
                                                   { %>
                                                <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="" title="" id="a_delete" style='<%#Eval("Deletelock")%>'>
                                                    <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                                <%} %>
                                                <%-- <a href="javascript:void(0);" onclick="OnClickCopy('<%# Container.KeyValue %>')" class="pad" title="Copy ">
                                                                            <i class="fa fa-copy"></i></a>--%>
                                                <% if (rights.CanView)
                                                   { %>
                                                <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%# Container.KeyValue %>')" class="pad" title="">
                                                    <span class='ico ColorThree'><i class='fa fa-paperclip'></i></span><span class='hidden-xs'>Add/View Attac.</span>
                                                </a><%} %>

                                                <% if (rights.CanView)
                                                   { %>
                                                <a href="javascript:void(0);" onclick="viewDocument('<%# Container.KeyValue %>')" class="pad" title="">
                                                    <span class='ico ColorFour'><i class='fa fa-file-text'></i></span><span class='hidden-xs'>View Doc.</span>
                                                </a><%} %>
                                                <% if (rights.CanPrint)
                                                   { %>
                                                <a href="javascript:void(0);" onclick="onPrintJv('<%# Container.KeyValue %>','<%# Container.VisibleIndex %>')" class="pad" title="">
                                                    <span class='ico ColorFive'><i class='fa fa-print'></i></span><span class='hidden-xs'>Print</span>
                                                </a><%} %>
                                                <% if (rights.CanAssignbranch)
                                                   { %>
                                                <a href="javascript:void(0);" class="pad" title="" id="a_Assignment" onclick="onViewBranchAssignment('<%# Container.KeyValue %>')" style="display: none;">
                                                    <span class='ico ColorSix'><i class='fa fa-industry'></i></span><span class='hidden-xs'>Branch Assig.</span>
                                                </a>
                                                <%} %>

                                                <% if (rights.Cancancelassignmnt)
                                                   { %>
                                                <a href="javascript:void(0);" class="pad" title="" id="Cancel_Assignment" onclick="onCancelBranchAssignment('<%# Container.KeyValue %>')" style="display: none;">
                                                    <span class='ico deleteColor'><i class='fa fa-times' aria-hidden='true'></i></span><span class='hidden-xs'>Cancel Branch Assig.</span>
                                                </a>
                                                <%} %>
                                                <% if (rights.Influencer)
                                                   { %>
                                                <a href="javascript:void(0);" class="pad" title="" id="Add_Influencer" onclick="onAddInfluencer('<%# Container.KeyValue %>')" style='<%#Eval("mainCommission_Display")%>'>
                                                    <span class='ico ColorSeven'><i class='fa fa-user-plus' aria-hidden='true'></i></span><span class='hidden-xs'>Add Influencer</span>
                                                </a>
                                                <%} %>
                                                <%if (rights.CanAssetDetails)
                                                  { %>
                                                <a href="javascript:void(0);" class="pad" title="" id="Add_Vechile" onclick="onAddVehicle('<%# Container.KeyValue %>')">
                                                    <span class='ico ColorSix'><i class='fa fa-truck out'></i></span><span class='hidden-xs'>Add Vehicle</span>
                                                </a>
                                                <%} %>
                                                <% if (rights.Influencer)
                                                   { %>
                                                <a href="javascript:void(0);" class="pad" title="" id="Add_InfluencerScheme" onclick="onInfluencerScheme('<%# Container.KeyValue %>')" style='<%#Eval("noCommission_Display")%>'>
                                                    <span class='ico ColorSix'><i class='fa fa-credit-card'></i></span><span class='hidden-xs'>Add Influencer</span>
                                                </a>
                                                <%} %>
                                                <%--       chinmoy comment 26-08-2019 start--%>
                                                <%--  <% if (rights.CanAdd)
                                                   { %>
                                                <a href="javascript:void(0);" class="pad" title="" id="CopyInvoice" onclick="onCopyInvoice('<%# Container.KeyValue %>','<%# Eval("InvoiceNo") %>')">
                                                    <span class='ico ColorSix'><i class='fa fa-truck out'></i></span><span class='hidden-xs'>Copy Invoice</span>
                                                </a>
                                                <%} %>--%>

                                                <%--                                                <a href="javascript:void(0);" class="pad" title="" onclick="onInfluencerReturn('<%# Container.KeyValue %>')">
                                                    <span class='ico ColorSix'><i class='fa fa-truck out'></i></span><span class='hidden-xs'></span>
                                                </a>--%>


                                                <%-- End--%>
                                            </div>
                                        </DataItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                        <HeaderTemplate><span>Actions</span></HeaderTemplate>
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                        <Settings AllowAutoFilterTextInputTimer="False" />

                                    </dxe:GridViewDataTextColumn>
                                </Columns>
                                <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                                <%-- <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                                                                <FirstPageButton Visible="True">
                                                                </FirstPageButton>
                                                                <LastPageButton Visible="True">
                                                                </LastPageButton>
                                                            </SettingsPager>--%>

                                <SettingsPager PageSize="10">
                                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                </SettingsPager>

                                <%--                                <SettingsSearchPanel Visible="True" />--%>
                                <Settings ShowGroupPanel="True" ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" ShowStatusBar="Hidden" ShowHorizontalScrollBar="true" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                <SettingsLoadingPanel Text="Please Wait..." />

                                <ClientSideEvents EndCallback="ListingGridEndCallback" RowClick="gridcrmCampaignclick" />

                                <TotalSummary>
                                    <dxe:ASPxSummaryItem FieldName="NetAmount" SummaryType="Sum" />
                                </TotalSummary>


                            </dxe:ASPxGridView>
                            <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                                ContextTypeName="ERPDataClassesDataContext" TableName="v_posList" />
                            <asp:HiddenField ID="hfIsFilter" runat="server" />
                            <asp:HiddenField ID="hfFromDate" runat="server" />
                            <asp:HiddenField ID="hfToDate" runat="server" />
                            <asp:HiddenField ID="hfBranchID" runat="server" />
                            <asp:HiddenField ID="hiddenedit" runat="server" />
                        </div>
                        <div style="display: none">
                            <dxe:ASPxGridViewExporter ID="exporter" GridViewID="GrdQuotation" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                            </dxe:ASPxGridViewExporter>
                        </div>

                    </dxe:ContentControl>
                </ContentCollection>
            </dxe:TabPage>
            <dxe:TabPage Name="AdvanceRec" Text="Money Receipt">
                <ContentCollection>
                    <dxe:ContentControl runat="server">
                        <div class="GridViewArea">
                        </div>
                    </dxe:ContentControl>
                </ContentCollection>
            </dxe:TabPage>
            <dxe:TabPage Name="IST" Text="Interstate Stock Transfer">
                <ContentCollection>
                    <dxe:ContentControl runat="server">
                        <div class="GridViewArea">
                            <asp:HiddenField ID="HiddenField1" runat="server" />
                        </div>


                    </dxe:ContentControl>
                </ContentCollection>
            </dxe:TabPage>
        </TabPages>
        <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
	                                            var Tab0 = page.GetTab(0);
	                                            var Tab1 = page.GetTab(1);
	                                            var Tab2 = page.GetTab(2);
	                                            if(activeTab == Tab0)
	                                            {
	                                                disp_prompt('tab0');
	                                            }
	                                            if(activeTab == Tab1)
	                                            {
	                                                disp_prompt('tab1');
	                                            }
                                                if(activeTab == Tab2)
	                                            {
	                                                disp_prompt('tab2');
	                                            }
	                                            }"></ClientSideEvents>
    </dxe:ASPxPageControl>


    <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" Width="1200"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="waitingPopUp"
        HeaderText="Invoice Waiting" AllowResize="false" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                <div onkeypress="OnWaitingGridKeyPress(event)">

                    <dxe:ASPxGridView ID="watingInvoicegrid" runat="server" KeyFieldName="SBMain_Id" AutoGenerateColumns="False"
                        Width="100%" ClientInstanceName="cwatingInvoicegrid" OnCustomCallback="watingInvoicegrid_CustomCallback" KeyboardSupport="true"
                        SettingsBehavior-AllowSelectSingleRowOnly="true" OnDataBinding="watingInvoicegrid_DataBinding" SettingsBehavior-AllowFocusedRow="true"
                        Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="320">
                        <Columns>
                            <%--<dxe:GridViewCommandColumn ShowSelectCheckbox="true" VisibleIndex="0" Width="60" Caption="Select" />--%>

                            <dxe:GridViewDataTextColumn Caption="Salesman" FieldName="Salesman"
                                VisibleIndex="0" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Unit" FieldName="Branch"
                                VisibleIndex="0" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn Caption="Customer" FieldName="Name"
                                VisibleIndex="1" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn Caption="Product List" FieldName="ProductList"
                                VisibleIndex="1" FixedStyle="Left" Width="180px">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>




                            <dxe:GridViewDataTextColumn Caption="Billing Amount" FieldName="finalAmount"
                                VisibleIndex="2" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn Caption="Billing Date" FieldName="Billingdate"
                                VisibleIndex="2" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Payment Type" FieldName="Paymenttype"
                                VisibleIndex="2" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="60">
                                <DataItemTemplate>
                                    <% if (rights.CanDelete)
                                       { %>
                                    <a href="javascript:void(0);" onclick="RemoveInvoice('<%# Container.KeyValue %>')" class="pad" title="Remove">
                                        <%--   <img src="../../../assests/images/Delete.png" />--%>
                                        <i class="fa fa-close" aria-hidden="true" id="CloseRemoveWattingBtn" style="font-size: 19px; color: #f35248;"></i>
                                    </a>
                                    <%} %>
                                </DataItemTemplate>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                <HeaderTemplate><span>Actions</span></HeaderTemplate>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                        </Columns>

                        <ClientSideEvents RowClick="ListRowClicked" EndCallback="watingInvoicegridEndCallback" />

                        <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="False" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsLoadingPanel Text="Please Wait..." />
                    </dxe:ASPxGridView>
                </div>


                <dxe:ASPxButton ID="InvoiceWattingOk" runat="server" AutoPostBack="false" Text="O&#818;k" CssClass="btn btn-primary okClass"
                    ClientSideEvents-Click="InvoiceWattingOkClick" UseSubmitBehavior="False" />
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

    <%--mass Branch --%>
    <dxe:ASPxPopupControl ID="massBranchPopup" runat="server" Width="1000"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cmassBranchPopup" Height="650"
        HeaderText="Branch Assignment" AllowResize="false" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

    <%--end of mass branch--%>

    <dxe:ASPxPopupControl ID="AssignmentPopUp" runat="server" Width="800"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cAssignmentPopUp" Height="500"
        HeaderText="Branch Assignment" AllowResize="false" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                <div id="BranchAssignmentHeader">
                    <dxe:ASPxCallbackPanel runat="server" ID="BranchRequUpdatePanel" ClientInstanceName="cBranchRequUpdatePanel" OnCallback="BranchRequUpdatePanel_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">

                                <table class="smllpad">
                                    <tr>
                                        <td style="width: 110px"></td>
                                        <td>
                                            <label>Unit</label></td>
                                        <td>
                                            <label>Warehouse</label></td>
                                        <td></td>

                                    </tr>
                                    <tr>
                                        <td style="padding-right: 10px">
                                            <label>Assigned To</label></td>
                                        <td class="relative">
                                            <dxe:ASPxComboBox ID="AssignedBranch" runat="server" ClientInstanceName="cAssignedBranch" Width="100%">
                                                <ClientSideEvents SelectedIndexChanged="AssignedBranchSelectedIndexChanged"></ClientSideEvents>
                                            </dxe:ASPxComboBox>
                                            <span id="MandatoryBranchAssign" style="display: none" class="errorField">
                                                <img id="MandatoryBranchAssignid" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                            </span>
                                        </td>
                                        <td class="relative">
                                            <dxe:ASPxComboBox ID="AssignedWareHouse" runat="server" OnCallback="AssignedWareHouse_Callback" ClientInstanceName="cAssignedWareHouse" SelectedIndex="1" Width="100%">
                                            </dxe:ASPxComboBox>
                                            <span id="mandetoryAssignedWareHouse" style="display: none" class="errorField">
                                                <img id="idmandetoryAssignedWareHouse" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                            </span>
                                        </td>
                                        <td>
                                            <input type="button" value="Assign" class="btn btn-primary" onclick="AssignBranchToThisInvoice()" />
                                            <input type="button" value="Cancel" class="btn btn-danger" onclick="CancelBranchToThisInvoice()" />
                                        </td>

                                    </tr>
                                </table>
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </div>
                <table class="smllpad" style="margin-top: 15px;">
                    <tr>

                        <td style="width: 110px">Select Unit To View Stock </td>
                        <td>
                            <dxe:ASPxComboBox ID="BranchAssignmentBranch" runat="server" ClientInstanceName="cBranchAssignmentBranch" Width="100%">
                                <ClientSideEvents SelectedIndexChanged="BranchAssignmentBranchSelectedIndexChanged"></ClientSideEvents>
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                            <a href="#" onclick="updateAssignmentGrid()">
                                <button type="button" class="btn btn-primary "><i class="fa fa-search" style="" aria-hidden="true"></i>View Stock</button></a>
                            <%--   <input type="button" value="Show Stock" class="btn btn-primary" onclick="updateAssignmentGrid()" />--%>
                        </td>

                    </tr>

                </table>


                <dxe:ASPxGridView ID="AssignmentGrid" runat="server" KeyFieldName="InvoiceDetails_Id" AutoGenerateColumns="False"
                    Width="100%" ClientInstanceName="cAssignmentGrid" OnCustomCallback="AssignmentGrid_CustomCallback" KeyboardSupport="true"
                    SettingsBehavior-AllowSelectSingleRowOnly="true" OnDataBinding="AssignmentGrid_DataBinding" SettingsBehavior-AllowFocusedRow="true">
                    <Columns>


                        <dxe:GridViewDataTextColumn Caption="Code" FieldName="sProducts_Code"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Name" FieldName="InvoiceDetails_ProductDescription"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Available Stock" FieldName="availableQty"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Curently Invoiced" FieldName="InvoicedBalance"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Actual Balance" FieldName="Actual_Balance"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                    </Columns>

                    <ClientSideEvents EndCallback="AssignmentGridEndCallback" />


                    <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>


                    <SettingsSearchPanel Visible="True" />
                    <Settings ShowGroupPanel="False" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                    <SettingsLoadingPanel Text="Please Wait..." />
                </dxe:ASPxGridView>




            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>








    <dxe:ASPxTimer runat="server" Interval="10000" ClientInstanceName="Timer1">
        <ClientSideEvents Tick="timerTick" />
    </dxe:ASPxTimer>
    <asp:HiddenField ID="waitingInvoiceCount" runat="server" />
    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel"
        Modal="True">
    </dxe:ASPxLoadingPanel>
    <dxe:ASPxPopupControl ID="apcReciptPopup" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="capcReciptPopup" Height="630px"
        Width="1200px" HeaderText="Customer Receipt/Payment" Modal="true" AllowResize="true" ResizingMode="Postponed">

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

    <asp:HiddenField ID="hdIsStockLedger" runat="server" />
    <asp:HiddenField ID="LoadGridData" runat="server" />

    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>



    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ClientInstanceName="cCustDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentStyle VerticalAlign="Top"></ContentStyle>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="CustomerRecpayPanel" ClientInstanceName="CustomerRecpayPanel" OnCallback="CustRacPayPanel_Callback" ClientSideEvents-EndCallback="CustRacPayPanelEndCall">
                        <ClientSideEvents EndCallback="CustRacPayPanelEndCall"></ClientSideEvents>
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxComboBox ID="CustCmbDesignName" ClientInstanceName="cCustCmbDesignName" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                </dxe:ASPxComboBox>
                                <div class="text-center pTop10">
                                    <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtnOK" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {return PerformCallToRacpayGridBind();}" />
                                    </dxe:ASPxButton>
                                </div>
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>



    <dxe:ASPxPopupControl ID="PosView" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cPosView" Height="650px"
        Width="1020px" HeaderText="POS (Invoice)" Modal="true">

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

    <!-- Button trigger modal -->

    <div class="modal fade pmsModal w80" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog" role="document" style="width: 95%;">
            <div class="modal-content">
                <div class="modal-header">

                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalLabel">Influencer</h4>
                </div>
                <div class="modal-body" style="overflow-y: scroll; max-height: 450px; overflow-x: hidden;">
                    <div class="row">
                        <div class="col-md-5">
                            <div class="Top clearfix">
                                <ul class="list-inline">
                                    <li>
                                        <div class="lblHolder" id="">
                                            <table>
                                                <tbody>
                                                    <tr>
                                                        <td>Invoice Number </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div style="width: 100%;">
                                                                <b id="txtInvoiceNumber" style="text-align: center">0.00</b>
                                                            </div>

                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </li>
                                    <li>
                                        <div class="lblHolder">
                                            <table>
                                                <tbody>
                                                    <tr>
                                                        <td>Amount</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div style="width: 100%;">
                                                                <span id="txtInvoiceAmount">Peekay Group</span>
                                                            </div>

                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </li>
                                    <li id="liJV" class="hide">
                                        <div class="lblHolder">
                                            <table>
                                                <tbody>
                                                    <tr>
                                                        <td>Auto Journal Number</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div style="width: 100%;">
                                                                <span id="txtAutoJV">Peekay Group</span>
                                                            </div>

                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <div class="col-md-7">
                            <ul class="smallList" style="color: red;">
                                <li>On Qty - Commission calculated on the Qty of the selected Item</li>
                                <li>Amount before GST - Commission calculated as percentage of the Amount before GST is charged.</li>
                                <li>Amount after GST - Commission calculated as percentage of the Amount with GST charged.</li>
                                <li>Flat Value - Flat commission amount irrespective of Qty and Value</li>
                            </ul>
                        </div>
                    </div>

                    <div class="clearfix padTopBot mBot10">
                        <table id="tableProduct" class="table table-striped table-bordered display colorTable" style="width: 100%">
                        </table>
                    </div>


                    <div style="background: #f5f4f3; padding: 8px 0; margin-bottom: 0px; border-radius: 4px; border: 1px solid #ccc; margin-bottom: 10px;" class="clearfix padTopBot">
                        <div class="col-md-3" id="div_Edit">
                            <label>Select Numbering Scheme For Journal</label>
                            <div>
                                <asp:DropDownList ID="CmbScheme" runat="server" DataSourceID="SqlSchematype"
                                    DataTextField="SchemaName" DataValueField="ID" Width="100%"
                                    onchange="CmbScheme_ValueChange()">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlSchematype" runat="server"
                                    SelectCommand="Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select ID,SchemaName  + 
                                (Case When (SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch) Is Null Then '' 
                                Else ' ('+(SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch)+')' End) From tbl_master_Idschema  Where TYPE_ID='1' AND IsActive=1 AND Isnull(Branch,'') in (select s FROM dbo.GetSplit(',',@userbranchHierarchy)) AND Isnull(comapanyInt,'')=@LastCompany AND financial_year_id=(Select Top 1 FinYear_ID FROM Master_FinYear WHERE FinYear_Code=@LastFinYear))) as x Order By ID asc">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="LastCompany" SessionField="LastCompany" />
                                        <asp:SessionParameter Name="LastFinYear" SessionField="LastFinYear" />
                                        <asp:SessionParameter Name="userbranchHierarchy" SessionField="userbranchHierarchy" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Document No.</label>
                            <div>
                                <asp:TextBox ID="txtBillNo" runat="server" Width="95%" meta:resourcekey="txtBillNoResource1" MaxLength="30" onchange="txtBillNo_TextChanged()"></asp:TextBox>
                                <span id="MandatoryBillNo" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                <span id="duplicateMandatoryBillNo" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Duplicate Journal No."></span>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label style="">Posting Date</label>
                            <div>
                                <dxe:ASPxDateEdit ID="tDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" DisplayFormatString="dd-MM-yyyy" ClientInstanceName="tDate"
                                    UseMaskBehavior="True" Width="100%" meta:resourcekey="tDateResource1">
                                </dxe:ASPxDateEdit>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label style="">Remarks</label>
                            <div>
                                <dxe:ASPxTextBox ID="txtInfRemarks" runat="server" EditFormat="Custom" ClientInstanceName="ctxtInfRemarks" MaxLength="500">
                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                        <div class="col-md-3 hide">
                            <div class="mroonText">Something goes here</div>
                        </div>


                    </div>

                    <div class="borderTopBottom clear padTopBot mBot10 ">

                        <div class="row mBot10 ">
                            <div class="col-md-2" style="padding-top: 8px;">
                                <label>Commission Expense Ledger</label>
                                <dxe:ASPxButtonEdit runat="server" ID="btnMAdr" ClientInstanceName="cbtnMAdr" class="form-control" Width="100%">
                                    <Buttons>
                                        <dxe:EditButton>
                                        </dxe:EditButton>
                                    </Buttons>
                                    <ClientSideEvents ButtonClick="btnMAdrClick" KeyDown="btnMAdrKeyDown" />
                                </dxe:ASPxButtonEdit>
                            </div>
                            <div class="col-md-2 hide" style="padding-top: 8px;">
                                <label>Posting Ledger Cr.</label>
                                <dxe:ASPxButtonEdit runat="server" ID="btnMainAccount" ClientInstanceName="cbtnMainAccount" class="form-control hide" Width="100%">

                                    <Buttons>

                                        <dxe:EditButton>
                                        </dxe:EditButton>
                                    </Buttons>
                                    <ClientSideEvents ButtonClick="btnMainAccountClick" KeyDown="btnMainAccountKeyDown" />
                                </dxe:ASPxButtonEdit>
                            </div>
                            <div class="col-md-3">
                                <label>Influencer</label>
                                <dxe:ASPxButtonEdit runat="server" ID="txtInfluencer" ClientInstanceName="ctxtInfluencer" class="form-control" Width="100%">
                                    <Buttons>
                                        <dxe:EditButton>
                                        </dxe:EditButton>
                                    </Buttons>
                                    <ClientSideEvents ButtonClick="InfluencerBtnClick" KeyDown="InfluencerKeyDown" />
                                </dxe:ASPxButtonEdit>
                            </div>
                            <div class="col-md-2 padTop25">
                                <dxe:ASPxButton runat="server" AutoPostBack="False" UseSubmitBehavior="false" ID="btnSaveInfluencer" ClientInstanceName="cbtnSaveInfluencer" CssClass="btn btn-primary onDxe" Text="+">
                                    <ClientSideEvents Click="OnInfAddClick" />
                                </dxe:ASPxButton>
                            </div>
                            <div class="col-md-5">
                                <ul class="list-inline pull-right" style="margin-top: 13px; margin-bottom: 0;">
                                    <li>
                                        <div class="lblHolder" id="">
                                            <table>
                                                <tbody>
                                                    <tr>
                                                        <td>Total Amount </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div style="width: 100%;">
                                                                <b id="txtCommAmt" style="text-align: center">0.00</b>
                                                            </div>

                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </li>

                                </ul>
                            </div>
                        </div>
                    </div>
                    <table id="influencerGrid" class="table table-striped table-bordered display ">
                    </table>
                </div>
                <div class="modal-footer">
                    <button type="button" id="divDeleteinf" onclick="DeleteInfluencerdetails();" class="btn btn-danger">Delete</button>
                    <button type="button" id="divSaveinf" onclick="SaveInfluencerdetails();" class="btn btn-success">Save and Exit</button>

                    <label class="red hide" id="divmsg">Payment for this commission has already been made. Edit/Delete is not allowed</label>

                </div>
            </div>
        </div>
    </div>


    <div class="modal fade pmsModal w60" id="CustModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Influencer Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search By Influencer Name or Unique Id" />

                    <div id="CustomerTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Customer Name</th>
                                <th>Unique Id</th>
                                <th>Address</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>

    <div class="modal fade pmsModal w60" id="InfluencerModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Influencer Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Influenecerkeydown(event)" id="txtInfluencerSearch" autofocus width="100%" placeholder="Search By Influencer Name or Unique Id" />

                    <div id="InfluencerTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Customer Name</th>
                                <th>Unique Id</th>
                                <th>Address</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>

    <div class="modal fade pmsModal w60" id="InfluencerSchemeModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Influencer Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="InfluenecerSchemekeydown(event)" id="txtInfluencerSchemeSearch" autofocus="autofocus" width="100%" placeholder="Search By Influencer Name or Unique Id" />

                    <div id="InfluencerSchemeTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Customer Name</th>
                                <th>Unique Id</th>
                                <th>Address</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>




    <div class="modal fade pmsModal w60" id="MainAccountModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <!-- Modal MainAccount-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Posting Ledger(Cr.) Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="MainAccountNewkeydown(event)" id="txtMainAccountSearch" autofocus width="100%" placeholder="Search by Main Account Name or Short Name" />

                    <div id="MainAccountTable">
                        <table border='1' width="100%">
                            <tr class="HeaderStyle">
                                <th>Main Account Name</th>
                                <th>Short Name</th>
                                <th>Subledger Type</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade pmsModal w60" id="MainAccountModeldr" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <!-- Modal MainAccount-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Posting Ledger(Dr.) Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="MainAccountNewkeydowndr(event)" id="txtMainAccountSearchdr" autofocus width="100%" placeholder="Search by Main Account Name or Short Name" />

                    <div id="MainAccountTabledr">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th>Main Account Name</th>
                                <th>Short Name</th>
                                <th>Subledger Type</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <div>
        <input type="hidden" id="ddlBranch" />
        <input type="hidden" id="invid" />
        <input type="hidden" id="infid" />
        <input type="hidden" id="mainaccrid" />
    </div>

    <div class="modal fade pmsModal w60" id="VehicleModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Vehicle Details</h4>
                </div>
                <div class="modal-body">

                    <div class="clearfix">
                        <div class="row">
                            <div class="col-md-4">
                                <label>Vehicle<span style="color: red;"> &nbsp;* </span></label>
                                <div class="relative">
                                    <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cQuotationComponentPanel" OnCallback="ComponentQuotation_Callback" Width="100%">
                                        <PanelCollection>
                                            <dxe:PanelContent runat="server">
                                                <asp:HiddenField runat="server" ID="OldSelectedKeyvalue" />
                                                <dxe:ASPxGridLookup ID="lookup_quotation" SelectionMode="Multiple" runat="server" TabIndex="7" ClientInstanceName="gridquotationLookup"
                                                    OnDataBinding="lookup_quotation_DataBinding" GridViewClientSideEvents-SelectionChanged="VehicleSelectionChanged"
                                                    KeyFieldName="vehicle_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                                    <Columns>
                                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                                        <dxe:GridViewDataColumn FieldName="vehicle_regNo" Visible="true" VisibleIndex="1" Caption="Vehicle No" Width="180" Settings-AutoFilterCondition="Contains">
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataColumn>
                                                        <dxe:GridViewDataColumn FieldName="vehicle_Id" Visible="false" VisibleIndex="2" Caption="ID" Width="100" Settings-AutoFilterCondition="Contains">
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataColumn>
                                                    </Columns>
                                                    <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                        <Templates>
                                                            <StatusBar>
                                                                <table class="OptionsTable" style="float: right">
                                                                    <tr>
                                                                        <td>
                                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" UseSubmitBehavior="False" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </StatusBar>
                                                        </Templates>
                                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                                        <SettingsPager Mode="ShowAllRecords">
                                                        </SettingsPager>
                                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                    </GridViewProperties>
                                                    <ClientSideEvents ValueChanged="function(s, e) { QuotationNumberChanged();}" GotFocus="function(s,e){gridquotationLookup.ShowDropDown();  }" DropDown="LoadOldSelectedKeyvalue" />
                                                </dxe:ASPxGridLookup>
                                            </dxe:PanelContent>
                                        </PanelCollection>
                                        <ClientSideEvents EndCallback="componentEndCallBack" BeginCallback="BeginComponentCallback" />
                                    </dxe:ASPxCallbackPanel>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <label>
                                    <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Payment Terms"></dxe:ASPxLabel>
                                </label>
                                <div class="relative">
                                    <dxe:ASPxComboBox ID="cmbPaymentTrms" ClientInstanceName="ccmbPaymentTrms" runat="server"
                                        ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                        <Items>
                                            <dxe:ListEditItem Text="By Us" Value="US" />
                                            <dxe:ListEditItem Text="By Party" Value="Pary" />
                                        </Items>
                                        <ClientSideEvents GotFocus="function(s,e){ccmbPaymentTrms.ShowDropDown();}" />
                                        <%--SelectedIndexChanged="isDeliveryTypeChanged2"--%>
                                    </dxe:ASPxComboBox>
                                </div>

                            </div>
                            <div class="col-md-4">
                                <label>
                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Other Charges">
                                    </dxe:ASPxLabel>
                                </label>
                                <div class="relative">

                                    <dxe:ASPxComboBox ID="CmbOtehrChrgs" ClientInstanceName="ccmbOtehrChrgs" runat="server"
                                        ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                        <Items>
                                            <dxe:ListEditItem Text="By Us" Value="US" />
                                            <dxe:ListEditItem Text="By Party" Value="Party" />
                                        </Items>
                                        <ClientSideEvents GotFocus="function(s,e){ccmbOtehrChrgs.ShowDropDown();}" />
                                        <%--SelectedIndexChanged="isDeliveryTypeChanged" --%>
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <label class="lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="E-Way Bill No.">
                                    </dxe:ASPxLabel>
                                </label>
                                <div class="relative">

                                    <dxe:ASPxTextBox ID="txtEbillNo" MaxLength="12" ClientInstanceName="ctxt_EbillNo" runat="server" Width="100%">
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <label class="lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel10" runat="server" Text="E-Way Bill Date">
                                    </dxe:ASPxLabel>
                                </label>
                                <div class="relative">

                                    <dxe:ASPxDateEdit ID="EwayDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cEwayDate" Width="100%">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                        <ClientSideEvents GotFocus="function(s,e){cEwayDate.ShowDropDown();}" />
                                    </dxe:ASPxDateEdit>
                                    <span id="MandatorysdeliveryDate" style="display: none" class="errorField">
                                        <img id="MandatorysdeliveryDateid" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"> </img>
                                    </span><span id="duplicateQuoteno" class="validclass" style="display: none">
                                        <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Duplicate number"> </img>
                                    </span>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <label class="lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="E-Way Bill Value">
                                    </dxe:ASPxLabel>
                                </label>
                                <div class="relative">
                                    <dxe:ASPxTextBox ID="txtEwayValue" CssClass="text-right text-right" Text="0.00" Style="text-align: right;" ClientInstanceName="ctxt_EwayValue" runat="server" Width="100%">
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <label class="lblmTop8">
                                    <dxe:ASPxLabel ID="lbl_Refference" runat="server" Text="Remarks ">
                                    </dxe:ASPxLabel>
                                </label>
                                <div class="relative">
                                    <asp:TextBox ID="txtRemarks" runat="server" MaxLength="1000" TabIndex="18" Width="100%" TextMode="MultiLine" Rows="5" Columns="10" Height="50px"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-success " onclick="VeichleSave()">Save</button>
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>

    <input type="hidden" id="mainacdrid" />
    <script>

        function LoadOldSelectedKeyvalue() {
            var x = gridquotationLookup.gridView.GetSelectedKeysOnPage();
            var Ids = "";
            for (var i = 0; i < x.length; i++) {
                Ids = Ids + ',' + x[i];
            }
            document.getElementById('OldSelectedKeyvalue').value = Ids;
        }

        function fn_Invoice_Name_TextChanged(s, e) {
            var otherdet = {};
            var INvnum = ctxt_NewInvoiceNo.GetText();
            otherdet.INvnum = INvnum;

            if (INvnum != "") {
                $.ajax({
                    type: "POST",
                    url: "PosSalesInvoiceList.aspx/CheckUniqueinvoiceName",
                    //data: "{'ProductName':'" + ProductName + "'}",
                    data: JSON.stringify(otherdet),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var data = msg.d;

                        if (data == 1) {
                            //jAlert("Please enter unique name", "Alert", function () { $("#PName").focus(); });
                            //ctxtPro_Code.SetText("");
                            jAlert("Please enter unique invoice number.");
                            ctxt_NewInvoiceNo.SetText("");
                            ctxt_NewInvoiceNo.Focus();
                            return false;
                        }
                    }

                });
            }
        }

        function fn_Challan_Name_TextChanged(s, e) {
            var otherdet = {};
            var challanNo = ctxtChallanNo.GetValue();
            otherdet.challanNo = challanNo;
            if (challanNo != "") {
                $.ajax({
                    type: "POST",
                    url: "PosSalesInvoiceList.aspx/CheckUniqueChallanName",
                    //data: "{'ProductName':'" + ProductName + "'}",
                    data: JSON.stringify(otherdet),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var data = msg.d;

                        if (data == 1) {
                            //jAlert("Please enter unique name", "Alert", function () { $("#PName").focus(); });
                            //ctxtPro_Code.SetText("");
                            jAlert("Please enter unique challan number.");
                            ctxtChallanNo.SetText("");
                            ctxtChallanNo.Focus();
                            return false;
                        }
                    }

                });

            }
        }

        function CopyInvoiceSave() {
            debugger;
            var otherdet = {};
            var INvnum = ctxt_InvoiceNo.GetText();
            var newInvoice = ctxt_NewInvoiceNo.GetText();
            var CustomerId = $("#hdnInvoiceCustID").val();
            var PostingDate = cPostingDate.GetDate();
            var RefNo = ctxt_RefNo.GetText();
            var vehicleNo = ctxtVehicleNo.GetValue();
            var challanNo = ctxtChallanNo.GetValue();
            var Salesman = cddl_SalesAgent.GetText();;

            otherdet.INvnum = INvnum;
            otherdet.newINvnum = newInvoice;
            otherdet.CntId = CustomerId;
            otherdet.PostingDate = PostingDate;
            otherdet.RfeNo = RefNo;
            otherdet.Salesman = Salesman;
            otherdet.Vehicle = vehicleNo;
            otherdet.uniquechallanNo = challanNo;

            if (newInvoice == "") {
                jAlert("Please enter unique invoice number.");
                ctxt_NewInvoiceNo.SetText("");
                ctxt_NewInvoiceNo.Focus();
                return false;
            }
            //if (INvnum != "") {
            //    $.ajax({
            //        type: "POST",
            //        url: "PosSalesInvoiceList.aspx/CheckUniqueinvoiceName",
            //        //data: "{'ProductName':'" + ProductName + "'}",
            //        data: JSON.stringify(INvnum),
            //        contentType: "application/json; charset=utf-8",
            //        dataType: "json",
            //        success: function (msg) {
            //            var data = msg.d;

            //            if (data == true) {
            //                //jAlert("Please enter unique name", "Alert", function () { $("#PName").focus(); });
            //                //ctxtPro_Code.SetText("");
            //                jAlert("Please enter unique invoice number.");
            //                ctxt_InvoiceNo.SetText("");
            //                ctxt_InvoiceNo.Focus();
            //                return false;
            //            }
            //        }

            //    });
            //}
            if (CustomerId == "") {
                jAlert("Customer can not be empty.");
                ctxtCustName.Focus();
                return false;
            }
            if (cPostingDate.GetDate() == null) {
                jAlert("Posting date can not be empty.");
                cPostingDate.Focus();
                return false;
            }


            //if (challanNo != "") {
            //    $.ajax({
            //        type: "POST",
            //        url: "PosSalesInvoiceList.aspx/CheckUniqueChallanName",
            //        //data: "{'ProductName':'" + ProductName + "'}",
            //        data: JSON.stringify(challanNo),
            //        contentType: "application/json; charset=utf-8",
            //        dataType: "json",
            //        success: function (msg) {
            //            var data = msg.d;

            //            if (data == true) {
            //                //jAlert("Please enter unique name", "Alert", function () { $("#PName").focus(); });
            //                //ctxtPro_Code.SetText("");
            //                jAlert("Please enter unique challan number.");
            //                ctxtChallanNo.SetText("");
            //                ctxtChallanNo.Focus();
            //                return false;
            //            }
            //        }

            //    });

            //}



            if (challanNo != "") {
                if (newInvoice == challanNo) {
                    jAlert("New invoice number and challan number not be same.");
                    ctxtChallanNo.SetText("");
                    ctxtChallanNo.Focus();
                    return false;

                }
            }



            $.ajax({
                type: "POST",
                url: "PosSalesInvoiceList.aspx/CopyInvoiceAndChallanSave",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(otherdet), //, VehicleNo: Gridr
                dataType: "json",
                //async: false,
                success: function (response) {
                    var returnVal = response.d;
                    if (returnVal == 1) {
                        jAlert("Data Save Successfully");
                        cGrdQuotation.Refresh();
                    }
                    $("#InvoiceCopyModel").modal('toggle');

                },
                error: function (response) {
                    jAlert("Please try again later");
                    //LoadingPanel.Hide();
                }
            });
            cGrdQuotation.Refresh();
        }


        function VeichleSave() {
            try {




                var vArr = cpSelectedKeys.join(",");

                debugger;
                var obj = {};

                var localproducts = [];
                var componentType = gridquotationLookup.GetValue();//gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());
                if (componentType != null && componentType != '') {
                    LoadingPanel.Show();
                    obj.Invoice_Id = $("#invid").val();
                    obj.PostingDate = cEwayDate.GetDate();
                    obj.Remarks = $("#txtRemarks").val();
                    if (ctxt_EwayValue.GetValue() == "0.00") {
                        obj.EwayValu = null;
                    }
                    else {
                        obj.EwayValu = ctxt_EwayValue.GetValue();//  $("#ctxt_EwayValue").val();
                    }
                    obj.ENo = ctxt_EbillNo.GetValue();//  $("#ctxt_EbillNo").val();
                    obj.CmbOtehrChrgs = ccmbOtehrChrgs.GetValue();
                    obj.cmbPaymentTrms = ccmbPaymentTrms.GetValue();
                    //Gridr.VECHICLE_NO = gridquotationLookup.GetValue();
                    //Gridr.VECHICLE_ID = vArr;//gridquotationLookup.gridView.GetSelectedKeysOnPage();
                    for (var i = 0; i < cpSelectedKeys.length; i++) {
                        var Gridr = {};
                        Gridr.VECHICLE_ID = cpSelectedKeys[i];
                        localproducts.push(Gridr);
                    }



                    obj.Grid = localproducts;
                    //LoadingPanel.Hide();
                    //alert(obj.gridID);
                    //alert(obj.gridValue);
                    $.ajax({
                        type: "POST",
                        url: "PosSalesInvoiceList.aspx/VeichleSave",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({ Delsave: obj }), //, VehicleNo: Gridr
                        dataType: "json",
                        //async: false,
                        success: function (response) {

                            jAlert(response.d);
                            $("#VehicleModel").modal('toggle');
                            LoadingPanel.Hide();
                            if (response.d == 'Data save successfully') {
                                $("#invid").val('');
                                $("#txtRemarks").val('');
                                ctxt_EwayValue.SetValue('0.00');
                                ctxt_EbillNo.SetValue('');
                                ccmbOtehrChrgs.SetValue('US');
                                ccmbPaymentTrms.SetValue('US');
                                gridquotationLookup.SetValue() = null;
                                cEwayDate.text = '';

                            }
                        },
                        error: function (response) {
                            jAlert("Please try again later");
                            LoadingPanel.Hide();
                        }
                    });
                }
                else {
                    jAlert("Please Select Vechile!!");
                    gridquotationLookup.Focus();
                }
            } catch (e) {
                alert(e);
            }
        }

        function Customerkeydown(e) {

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtCustSearch").val();

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Customer Name");
                HeaderCaption.push("Unique Id");
                HeaderCaption.push("Address");
                if ($("#txtCustSearch").val() != '') {
                    callonServer("Services/Master.asmx/GetInfluencerWithMainAccount", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[customerindex=0]"))
                    $("input[customerindex=0]").focus();
            }

        }


        function Influenecerkeydown(e) {

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtInfluencerSearch").val();

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Customer Name");
                HeaderCaption.push("Unique Id");
                HeaderCaption.push("Address");
                if ($("#txtInfluencerSearch").val() != '') {
                    callonServer("Services/Master.asmx/GetInfluencerWithMainAccount", OtherDetails, "InfluencerTable", HeaderCaption, "InfluencerIndex", "SetInfluencer");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[InfluencerIndex=0]"))
                    $("input[InfluencerIndex=0]").focus();
            }

        }
        function InfluenecerSchemekeydown(e) {

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtInfluencerSchemeSearch").val();

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Customer Name");
                HeaderCaption.push("Unique Id");
                HeaderCaption.push("Address");
                if ($("#txtInfluencerSchemeSearch").val() != '') {
                    callonServer("Services/Master.asmx/GetInfluencerWithMainAccount", OtherDetails, "InfluencerSchemeTable", HeaderCaption, "InfluencerSchemeIndex", "SetInfluencerScheme");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[InfluencerSchemeIndex=0]"))
                    $("input[InfluencerSchemeIndex=0]").focus();
            }

        }




        function MainAccountNewkeydown(e) {
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtMainAccountSearch").val();
            OtherDetails.branchId = $("#ddlBranch").val();
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                if ($("#txtMainAccountSearch").val() == "")
                    return;
                var HeaderCaption = [];
                HeaderCaption.push("Main Account Name");
                HeaderCaption.push("Short Name");

                HeaderCaption.push("Subledger Type");

                callonServer("/OMS/Management/Activities/Services/Master.asmx/GetMainAccountJournal", OtherDetails, "MainAccountTable", HeaderCaption, "MainAccountIndex", "SetMainAccount");
            }
            else if (e.code == "ArrowDown") {
                if ($("input[MainAccountIndex=0]"))
                    $("input[MainAccountIndex=0]").focus();
            }
            else if (e.code == "Escape") {
                //  
                $('#MainAccountModel').modal('hide');
                grid.batchEditApi.StartEdit(globalRowIndex, 1);
                var MainAccountID = grid.GetEditor("gvMainAcCode").GetValue();
                if (MainAccountID == 'Customers' || MainAccountID == 'Vendors') {
                    if ($("#hdnIsPartyLedger").val() == "") {
                        $("#hdnIsPartyLedger").val('1');
                    }
                    else {
                        $("#hdnIsPartyLedger").val(parseFloat($("#hdnIsPartyLedger").val()) + 1);
                    }

                }

            }
        }

        function MainAccountNewkeydowndr(e) {
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtMainAccountSearchdr").val();
            OtherDetails.branchId = $("#ddlBranch").val();
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                if ($("#txtMainAccountSearchdr").val() == "")
                    return;
                var HeaderCaption = [];
                HeaderCaption.push("Main Account Name");
                HeaderCaption.push("Short Name");

                HeaderCaption.push("Subledger Type");

                callonServer("/OMS/Management/Activities/Services/Master.asmx/GetMainAccountJournal", OtherDetails, "MainAccountTabledr", HeaderCaption, "MainAccountIndexdr", "SetMainAccountdr");
            }
            else if (e.code == "ArrowDown") {
                if ($("input[MainAccountIndexdr=0]"))
                    $("input[MainAccountIndexdr=0]").focus();
            }
            else if (e.code == "Escape") {
                //  
                $('#MainAccountModeldr').modal('hide');


            }
        }

        function DeleteInfluencerdetails() {
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    //var obj = {};
                    //obj.Invoice_Id = $("#invid").val();
                    $.ajax({
                        type: "POST",
                        url: "PosSalesInvoiceList.aspx/DeleteInfluencer",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({ Invoice_Id: $("#invid").val() }),
                        dataType: "json",
                        async: false,
                        success: function (response) {

                            jAlert(response.d);
                            $("#myModal").modal('toggle');
                        },
                        error: function (response) {
                            jAlert("Please try again later");
                            //LoadingPanel.Hide();
                        }
                    });
                }
            });


        }

        function SaveInfluencerdetails() {
            var obj = {};
            obj.PostingDate = tDate.GetDate();
            obj.Schema = $("#CmbScheme").val();
            obj.Billno = $("#txtBillNo").val();
            obj.Branch = $("#ddlBranch").val();
            obj.Invoice_Id = $("#invid").val();
            obj.Remarks = ctxtInfRemarks.GetText();


            obj.Mainaccr = $("#mainacdrid").val();



            var proobj = [];

            $("#tableProduct tr").each(function () {
                var arrayOfThisRow = [];
                var tableData = $(this).find('td');
                if (tableData.length > 0) {

                    var id = $(tableData[0]).html().trim();
                    var Myobj = {
                        PRODID: $("#prodid" + id).html(),
                        QTY: $("#prodqty" + id).html(),
                        SALESPRICE: $("#prodsp" + id).html(),
                        TOTALPRICE: $("#proddetamt" + id).html(),
                        DETID: $("#proddetid" + id).html(),
                        BASIS: $("#prodappl" + id).val(),
                        PERSENTAGE: $("#prodpercen" + id).val(),
                        AMOUNT: $("#prodcommamt" + id).val(),
                    };


                    proobj.push(Myobj);
                }
            });
            var myproobj = JSON.stringify(proobj);

            obj.product = proobj;


            myTableArray = [];

            $("#influencerGrid tr").each(function () {
                var arrayOfThisRow = [];
                var tableData = $(this).find('td');
                if (tableData.length > 0) {
                    tableData.each(function () {
                        arrayOfThisRow.push($(this).html());
                    });
                    myTableArray.push(arrayOfThisRow);
                }
            });
            //obj.Influencer = myTableArray;
            var totamount = 0;
            var Infobj = [];
            for (var i = 0; i <= myTableArray.length - 1; i++) {

                var Myobj = {
                    INFID: myTableArray[i][0],
                    MACRID: myTableArray[i][1],
                    AMT: $("#txtinfamt" + myTableArray[i][0]).val()
                };
                totamount = totamount + DecimalRoundoff($("#txtinfamt" + myTableArray[i][0]).val(), 2)
                Infobj.push(Myobj);
            }

            if (totamount > DecimalRoundoff($("#txtCommAmt").html().trim(), 2)) {
                jAlert('The total of the Individual Commission must be less than Total Calculated Commission', 'Alert');
                return;
            }




            var myInfobj = JSON.stringify(Infobj);

            obj.Influencer = Infobj;

            obj.Action = Action;

            if (Infobj.length == 0) {

                jAlert('Please add atleast one Influencer to proceed.', 'Alert');
                return;
            }

            if (tDate.GetText() == "") {
                jAlert('Please select a valid date to proceed.', 'Alert');
                return;
            }

            if (AutoJVNumber != "" && AutoJVNumber != null) {

            }
            else {
                if ($("#CmbScheme").val() == "") {
                    jAlert('Please select a valid schema to proceed.', 'Alert');
                    return;
                }
                if ($("#CmbScheme").val() == "0") {
                    jAlert('Please select a valid schema to proceed.', 'Alert');
                    return;
                }

            }
            if (parseFloat($("#txtCommAmt").html().trim()) == 0) {
                jAlert('Please add amount to proceed.', 'Alert');
                return;
            }

            if (cbtnMAdr.GetText() == "") {
                jAlert('Please select a valid main account to proceed.', 'Alert');
                return;
            }
            var output = "";
            var amt_msg = "";
            $.ajax({
                type: "POST",
                url: "PosSalesInvoiceList.aspx/GetInfluencerAmount",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ infsave: obj }),
                dataType: "json",
                async: false,
                success: function (msg) {
                    if (msg != null) {
                        output = msg.d.split('~')[1];
                        amt_msg = msg.d.split('~')[0];
                    }
                }
            });




            if (output == "b") {
                jAlert(amt_msg, 'Alert');
                return;
            }
            else if (output == "w") {
                jConfirm(amt_msg + " Do You want to proceed?", "Alert", function (ret) {
                    if (ret) {
                        $.ajax({
                            type: "POST",
                            url: "PosSalesInvoiceList.aspx/SaveInfluencer",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify({ infsave: obj }),
                            dataType: "json",
                            async: false,
                            success: function (response) {
                                Action = "Add";
                                jAlert(response.d);
                                $("#myModal").modal('toggle');
                            },
                            error: function (response) {
                                jAlert("Please try again later");
                                //LoadingPanel.Hide();
                            }
                        });
                    }
                });
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "PosSalesInvoiceList.aspx/SaveInfluencer",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ infsave: obj }),
                    dataType: "json",
                    async: false,
                    success: function (response) {

                        jAlert(response.d);
                        $("#myModal").modal('toggle');
                    },
                    error: function (response) {
                        jAlert("Please try again later");
                        //LoadingPanel.Hide();
                    }
                });


            }

            //$.ajax({
            //    type: "POST",
            //    url: "PosSalesInvoiceList.aspx/SaveInfluencer",
            //    contentType: "application/json; charset=utf-8",
            //    data: JSON.stringify({ infsave: obj }),
            //    dataType: "json",
            //    success: function (msg) {
            //    }
            //});




        }

        function ValidateInfluencerAmount(obj) {
            var output = "";
            $.ajax({
                type: "POST",
                url: "PosSalesInvoiceList.aspx/GetInfluencerAmount",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ infsave: obj }),
                dataType: "json",
                async: false,
                success: function (msg) {
                    output = msg.d.split('~')[1];
                }
            });

            return output;
        }






        function Vechilekeydown(e) {

            try {
                var OtherDetails = {}
                OtherDetails.SearchKey = $("#txtVechileSearch").val();

                if (e.code == "Enter" || e.code == "NumpadEnter") {
                    var HeaderCaption = [];
                    HeaderCaption.push("Registration No");
                    if ($("#txtVechileSearch").val() != '') {
                        //  callonServer("Services/Master.asmx/GetVehicleWithMainAccount", OtherDetails, "VechileTable", HeaderCaption, "customerIndex", "SetCustomer");
                        callonServerM("Services/Master.asmx/GetVehicleWithMainAccount", OtherDetails, "VechileTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "VehicleSource");
                    }
                }
                else if (e.code == "ArrowDown") {
                    if ($("input[dPropertyIndex=0]"))
                        $("input[dPropertyIndex=0]").focus();
                }
            } catch (e) {
                alert(e);
            }


        }
        function SetfocusOnseach(indexName) {
            if (indexName == "dPropertyIndex")
                $('#txtCustSearch').focus();
            else
                $('#txtCustSearch').focus();
        }



    </script>
    <script type="text/javascript">
        function SetSelectedValues(Id, Name, ArrName) {
            if (ArrName == 'VehicleSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#VechielModel').modal('hide');
                    txtVechileSearch.SetText(Name);
                    GetObjectID('hdnVehicleId').value = key;
                }
                else {
                    txtVechileSearch.SetText('');
                    GetObjectID('hdnVehicleId').value = '';
                }
            }

        }

    </script>
    <style>
        #InfluencerModel {
            z-index: 1041;
        }

        #InfluencerSchemeModel {
            z-index: 1041;
        }
    </style>
    <asp:HiddenField ID="hdnVehicleId" runat="server" />
    <%-- Subhra 30-04-2019 --%>
    <asp:HiddenField runat="server" ID="hdnPosDocPrintDesignBasedOnTaxCategory" />
    <%-- Subhra 30-04-2019 --%>
    <div class="modal fade pmsModal w60" id="VechielModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Vehicle Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Vechilekeydown(event)" id="txtVechileSearch" width="100%" placeholder="Search By Vechile" />
                    <div id="VechileTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size: small">
                                <th>Select</th>
                                <th class="hide">id</th>
                                <th>Vehicle No</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default" onclick="DeSelectAll('VehicleSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-success" data-dismiss="modal" onclick="OKPopup('VehicleSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>


    <%--Customer OutStanding Report--%>

    <dxe:ASPxPopupControl ID="ASPxPopupControl2" runat="server" ClientInstanceName="cOutstandingPopup"
        Width="1300px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <HeaderTemplate>
            <strong><span style="color: #fff">Customer Outstanding</span></strong>

            <dxe:ASPxImage ID="ASPxImage2" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                <ClientSideEvents Click="function(s, e){ 
                                                            cOutstandingPopup.Hide();
                                                        }" />
            </dxe:ASPxImage>
        </HeaderTemplate>
        <ContentCollection>

            <dxe:PopupControlContentControl runat="server">
                <dxe:ASPxGridView runat="server" KeyFieldName="SLNO" ClientInstanceName="cCustomerOutstanding" ID="CustomerOutstanding"
                    DataSourceID="LinqServerModeDataSourceCO" OnSummaryDisplayText="ShowGridCustOut_SummaryDisplayText"
                    Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" OnCustomCallback="cCustomerOutstanding_CustomCallback"
                    Settings-ShowFooter="true" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible"
                    OnHtmlFooterCellPrepared="ShowGridCustOut_HtmlFooterCellPrepared" OnHtmlDataCellPrepared="ShowGridCustOut_HtmlDataCellPrepared">

                    <SettingsBehavior AllowDragDrop="true" AllowSort="true"></SettingsBehavior>
                    <SettingsPager Visible="true"></SettingsPager>
                    <Columns>
                        <dxe:GridViewDataTextColumn Caption="Customer Name" FieldName="PARTYNAME" GroupIndex="0"
                            VisibleIndex="0">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="BRANCH_DESCRIPTION" Width="55%" ReadOnly="true" Caption="UNIT">
                        </dxe:GridViewDataTextColumn>
                        <%--<dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="PARTYNAME" Width="100" ReadOnly="true" Caption="Customer">
                                </dxe:GridViewDataTextColumn>--%>
                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="DOC_TYPE" ReadOnly="true" Caption="Doc. Type" Width="100%">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="ISOPENING" ReadOnly="true" Caption="Opening?" Width="30%">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="DOC_NO" ReadOnly="true" Width="95%" Caption="Document No">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="DOC_DATE" Width="50%" ReadOnly="true" Caption="Document Date">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="DUE_DATE" Width="50%" ReadOnly="true" Caption="Due Date">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="DOC_AMOUNT" ReadOnly="true" Caption="Document Amt." Width="50%">
                            <CellStyle HorizontalAlign="Right">
                            </CellStyle>
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Right" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="BAL_AMOUNT" ReadOnly="true" Caption="Balance Amount" Width="50%">
                            <CellStyle HorizontalAlign="Right">
                            </CellStyle>
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Right" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="DAYS" Width="20%" ReadOnly="true" Caption="Days">
                        </dxe:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" AllowSort="False" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                    <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                    <SettingsEditing Mode="EditForm" />
                    <SettingsContextMenu Enabled="true" />
                    <SettingsBehavior AutoExpandAllGroups="true" />
                    <Settings ShowGroupPanel="true" ShowStatusBar="Visible" ShowFilterRow="true" />
                    <SettingsSearchPanel Visible="false" />
                    <SettingsPager PageSize="10">
                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                    </SettingsPager>
                    <TotalSummary>
                        <dxe:ASPxSummaryItem FieldName="DOC_TYPE" SummaryType="Custom" Tag="Item_DocType" />
                        <dxe:ASPxSummaryItem FieldName="BAL_AMOUNT" SummaryType="Custom" Tag="Item_BalAmt"></dxe:ASPxSummaryItem>
                    </TotalSummary>

                    <SettingsDataSecurity AllowEdit="true" />

                </dxe:ASPxGridView>

                <dx:LinqServerModeDataSource ID="LinqServerModeDataSourceCO" runat="server" OnSelecting="EntityServerModeDataSourceCO_Selecting"
                    ContextTypeName="ERPDataClassesDataContext" TableName="PARTYOUTSTANDINGDET_REPORT" />
                <div style="display: none">
                    <dxe:ASPxGridViewExporter ID="exporter1" GridViewID="CustomerOutstanding" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                    </dxe:ASPxGridViewExporter>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>
    <asp:HiddenField ID="hdnCustomerId" runat="server" />
    <asp:HiddenField runat="server" ID="hddnBranchId" />
    <asp:HiddenField runat="server" ID="hddnAsOnDate" />
    <asp:HiddenField runat="server" ID="hddnOutStandingBlock" />
    <asp:HiddenField runat="server" ID="ISAllowBackdatedEntry" />
    <asp:HiddenField runat="server" ID="warehousestrProductID" />
    <%--End Customer Outstanding Reports--%>


    <%-- Tanmoy Rev Add Copy Invoice  --%>

    <div class="modal fade pmsModal w60" id="InvoiceCopyModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Invoice Details</h4>
                </div>
                <div class="modal-body">

                    <div class="clearfix">
                        <div class="row">
                            <div class="col-md-4">
                                <label>Invoice Number<span style="color: red;"> &nbsp;* </span></label>
                                <div class="relative">
                                    <dxe:ASPxTextBox ID="txt_InvoiceNo" MaxLength="16" ClientInstanceName="ctxt_InvoiceNo" runat="server" Width="100%">
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <label>
                                    <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="New Invoice Number"></dxe:ASPxLabel>
                                    <span style="color: red;">&nbsp;* </span>
                                </label>
                                <div class="relative">
                                    <dxe:ASPxTextBox ID="txt_NewInvoiceNo" MaxLength="16" ClientInstanceName="ctxt_NewInvoiceNo" runat="server" Width="100%">
                                        <ClientSideEvents TextChanged="function(s,e){fn_Invoice_Name_TextChanged()}" />
                                    </dxe:ASPxTextBox>
                                </div>

                            </div>
                            <div class="col-md-4">
                                <label>
                                    <dxe:ASPxLabel ID="ASPxLabel6" runat="server" Text="Customer Name">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">&nbsp;* </span>
                                </label>
                                <div class="relative">

                                    <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%">
                                        <Buttons>
                                            <dxe:EditButton>
                                            </dxe:EditButton>
                                        </Buttons>
                                        <ClientSideEvents ButtonClick="function(s,e){InvoiceCustomersButnClick();}" KeyDown="function(s,e){CopyInvoiceCustomerKeydown(s,e);}" />
                                    </dxe:ASPxButtonEdit>
                                    <asp:HiddenField ID="hdnInvoiceCustID" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <label class="lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel8" runat="server" Text="Posting Date">
                                    </dxe:ASPxLabel>
                                    <span style="color: red;">&nbsp;* </span>
                                </label>
                                <div class="relative">

                                    <dxe:ASPxDateEdit ID="PostingDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cPostingDate" Width="100%">
                                        <ButtonStyle Width="100%">
                                        </ButtonStyle>
                                        <ClientSideEvents GotFocus="function(s,e){cPostingDate.ShowDropDown();}" />
                                    </dxe:ASPxDateEdit>
                                    <span id="MandatorysdeliveryDate" style="display: none" class="errorField">
                                        <img id="MandatorysdeliveryDateid" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"> </img>
                                    </span><span id="duplicateQuoteno" class="validclass" style="display: none">
                                        <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Duplicate number"> </img>
                                    </span>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <label class="lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel7" runat="server" Text="Ref No">
                                    </dxe:ASPxLabel>
                                </label>
                                <div class="relative">

                                    <dxe:ASPxTextBox ID="txt_RefNo" MaxLength="16" ClientInstanceName="ctxt_RefNo" runat="server" Width="100%">
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <label class="lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel9" runat="server" Text="Salesman">
                                    </dxe:ASPxLabel>
                                </label>
                                <div class="relative">
                                    <%--<dxe:ASPxComboBox ID="ddl_SalesAgent" ClientInstanceName="cddl_SalesAgent" runat="server" ValueType="System.String" Width="100%" 
                                         EnableIncrementalFiltering="True" SelectedIndex="0">
                                        <ClientSideEvents GotFocus="function(s,e){cddl_SalesAgent.ShowDropDown();}"/>
                                    </dxe:ASPxComboBox>--%>
                                    <dxe:ASPxComboBox ID="ddl_SalesAgent" ClientInstanceName="cddl_SalesAgent" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True"
                                        OnCallback="ddl_SalesAgent_Callback" EnableIncrementalFiltering="True" SelectedIndex="0">
                                        <ClientSideEvents GotFocus="function(s,e){cddl_SalesAgent.ShowDropDown();}" EndCallback=" OnSalesAgentEndCallback" />
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <label class="lblmTop8">
                                    <dxe:ASPxLabel ID="ASPxLabel11" runat="server" Text="vehicle No ">
                                    </dxe:ASPxLabel>
                                </label>
                                <div class="relative">
                                    <dxe:ASPxTextBox ID="txtVehicleNo" ClientInstanceName="ctxtVehicleNo" runat="server" Width="100%" MaxLength="16">
                                    </dxe:ASPxTextBox>

                                </div>
                            </div>
                            <div class="col-md-4">
                                <label class="lblmTop8">
                                    <dxe:ASPxLabel ID="lblChallanNo" runat="server" Text="Challan No. ">
                                    </dxe:ASPxLabel>
                                </label>
                                <div class="relative">
                                    <dxe:ASPxTextBox ID="txtChallanNo" ClientInstanceName="ctxtChallanNo" runat="server" Width="100%" MaxLength="16">
                                        <ClientSideEvents TextChanged="function(s,e){fn_Challan_Name_TextChanged()}" />
                                    </dxe:ASPxTextBox>

                                </div>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-success " onclick="CopyInvoiceSave()">Save</button>
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>

    <%-- Tanmoy End Rev Add Copy Invoice  --%>

    <!--Customer Modal -->
    <div class="modal fade" id="InvoiceCustsModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Customer Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="InvoiceCustomerskeydown(event)" id="InvoicetxtCustsSearch" autofocus width="100%" placeholder="Search By Customer Name,Unique Id and Phone No." />

                    <div id="InvoiceCustomersTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Customer Name</th>
                                <th>Unique Id</th>
                                <th>Address</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>


    <div class="modal fade pmsModal" id="InfluencerPopupModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Influencer Adjustment</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-3">
                            <label>Influencer</label>
                            <dxe:ASPxButtonEdit runat="server" ID="txtInfluencerAdjustment" ClientInstanceName="ctxtInfluencerAdjustment" class="form-control" Width="100%">
                                <Buttons>
                                    <dxe:EditButton>
                                    </dxe:EditButton>
                                </Buttons>
                                <ClientSideEvents ButtonClick="InfluencerBtnClickAdjustment" KeyDown="InfluencerKeyDownAdjustment" />

                            </dxe:ASPxButtonEdit>
                        </div>
                    </div>
                    <div class="clearfix padTopBot mBot10 mTop5">
                        <table id="tableProductAdjustmentInvoice" class="table table-striped table-bordered display colorTable" style="width: 100%" border="0">
                        </table>
                    </div>

                    <div class="clearfix padTopBot mBot10">
                        <table id="tableProductAdjustmentReturn" class="table table-striped table-bordered display colorTable" style="width: 100%" border="0">
                        </table>
                    </div>


                </div>
                <div class="modal-footer">
                    <button type="button" id="" onclick="SaveInfluencerdetailsAdjustment();" class="btn btn-success">Save and Exit</button>
                </div>
            </div>

        </div>
    </div>
    <asp:HiddenField ID="hdnInvoiceType" runat="server" />



    <dxe:ASPxPopupControl ID="ApproveRejectPopup" runat="server" Width="1000"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cApproveRejectPopup"
        HeaderText="Invoice Approval/Rejection" AllowResize="false" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                <div onkeypress="OnWaitingGridKeyPress(event)">

                    <dxe:ASPxGridView ID="GridApproval" runat="server" KeyFieldName="invoice_id" AutoGenerateColumns="False"
                        Width="100%" ClientInstanceName="cGridApproval" OnCustomCallback="GridApproval_CustomCallback" KeyboardSupport="true"
                        SettingsBehavior-AllowSelectSingleRowOnly="true" OnDataBinding="GridApproval_DataBinding" SettingsBehavior-AllowFocusedRow="true"
                        Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="320">
                        <Columns>
                            <%--<dxe:GridViewCommandColumn ShowSelectCheckbox="true" VisibleIndex="0" Width="60" Caption="Select" />--%>

                            <dxe:GridViewDataTextColumn Caption="Salesman" FieldName="Salesman"
                                VisibleIndex="0" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn Caption="Customer" FieldName="Customer"
                                VisibleIndex="1" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Branch" FieldName="Branch"
                                VisibleIndex="2" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Billing Amount" FieldName="finalAmount"
                                VisibleIndex="3" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn Caption="Billing Date" FieldName="Billingdate"
                                VisibleIndex="4" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Status" FieldName="Status"
                                VisibleIndex="5" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Approved/Rejected By" FieldName="AppRejBy"
                                VisibleIndex="6" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>



                            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="60">
                                <DataItemTemplate>
                                    <% if (rights.CanAdd)
                                       { %>
                                    <a href="javascript:void(0);" onclick="AddApproved('<%# Container.KeyValue %>')" style='<%#Eval("Display")%>' class="pad" title="Remove">
                                        <%--   <img src="../../../assests/images/Delete.png" />--%>
                                        <i class="fa fa-edit" aria-hidden="true" id="CloseRemoveWattingBtn" style="font-size: 19px; color: #3E9CFC;"></i>
                                    </a>
                                    <%} %>
                                </DataItemTemplate>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                <HeaderTemplate><span>Actions</span></HeaderTemplate>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                        </Columns>

                        <ClientSideEvents RowClick="ListRowClicked" EndCallback="watingInvoicegridEndCallback" />

                        <SettingsPager NumericButtonCount="6" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="False" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsLoadingPanel Text="Please Wait..." />
                    </dxe:ASPxGridView>
                </div>


                <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="O&#818;k" CssClass="btn btn-primary okClass"
                    ClientSideEvents-Click="InvoiceWattingOkClick" UseSubmitBehavior="False" />
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>



    <div class="modal fade pmsModal" id="ApprovalPopupModel" role="dialog" style="z-index: 99999">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Approval</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-3">
                            <div id="divScheme" runat="server" style="padding-top: 10px">
                                <label>
                                    <dxe:ASPxLabel ID="lbl_NumberingScheme" runat="server" Text="Numbering Scheme">
                                    </dxe:ASPxLabel>
                                </label>
                                <div>
                                    <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="relative" style="padding-top: 10px">
                                <label>
                                    <dxe:ASPxLabel ID="lbl_SaleInvoiceNo" runat="server" Text="Document No.">
                                    </dxe:ASPxLabel>
                                </label>
                                <dxe:ASPxTextBox ID="txt_PLQuoteNo" runat="server" ClientInstanceName="ctxt_PLQuoteNo" Width="100%">
                                    <ClientSideEvents TextChanged="function(s, e) {UniqueCodeCheck();}" />
                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="relative" style="padding-top: 10px">
                                <label>
                                    <dxe:ASPxLabel ID="ASPxLabel12" runat="server" Text="Challan Document No.">
                                    </dxe:ASPxLabel>
                                </label>
                                <div>
                                    <dxe:ASPxComboBox ID="challanNoScheme" runat="server" ClientInstanceName="cchallanNoScheme" OnCallback="challanNoScheme_Callback" Width="100%">
                                        <ClientSideEvents EndCallback="challanNoSchemeEndCallback" SelectedIndexChanged="challanNoSchemeSelectedIndexChanged"
                                            GotFocus="function(s,e){cchallanNoScheme.ShowDropDown();}" />
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="relative" style="padding-top: 10px">
                                <label>
                                    <dxe:ASPxLabel ID="ASPxLabel13" runat="server" Text="Challan No.">
                                    </dxe:ASPxLabel>
                                </label>
                                <dxe:ASPxTextBox ID="ASPxTextBox1" ClientInstanceName="ctxtChallanNo" runat="server" Width="100%" ClientEnabled="false">
                                </dxe:ASPxTextBox>
                            </div>
                        </div>


                    </div>

                </div>
                <div class="modal-footer">
                    <button type="button" id="" onclick="SaveMainbill();" class="btn btn-success">Save and Exit</button>
                </div>
            </div>

        </div>
    </div>


    <div class="modal fade pmsModal" id="InfluenceSchemeModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Influencer Add</h4>
                </div>
                <div class="modal-body">

                    <div class="clearfix">
                        <div class="Top clearfix">
                            <ul class="list-inline">
                                <li>
                                    <div class="lblHolder" id="">
                                        <table>
                                            <tbody>
                                                <tr>
                                                    <td>Invoice Number </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div style="width: 100%;">
                                                            <b id="txtInvoiceNumberScheme" style="text-align: center">0.00</b>
                                                        </div>

                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </li>
                                <li>
                                    <div class="lblHolder">
                                        <table>
                                            <tbody>
                                                <tr>
                                                    <td>Amount</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div style="width: 100%;">
                                                            <span id="txtInvoiceAmountScheme">Peekay Group</span>
                                                        </div>

                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-3">
                            <label>Influencer</label>
                            <dxe:ASPxButtonEdit runat="server" ID="txtInfluencerScheme" ClientInstanceName="ctxtInfluencerScheme" class="form-control" Width="100%">
                                <Buttons>
                                    <dxe:EditButton>
                                    </dxe:EditButton>
                                </Buttons>
                                <ClientSideEvents ButtonClick="InfluencerSchemebtnClick" KeyDown="InfluencerSchemeKeyDown" />
                            </dxe:ASPxButtonEdit>
                        </div>
                    </div>
                    <div class="col-md-2 padTop25">
                        <dxe:ASPxButton runat="server" AutoPostBack="False" UseSubmitBehavior="false" ID="ASPxButton3" ClientInstanceName="cbtnSaveInfluencerScheme" CssClass="btn btn-primary onDxe" Text="+">
                            <ClientSideEvents Click="OnInfAddSchemeClick" />
                        </dxe:ASPxButton>
                    </div>

                    <div class="clearfix padTopBot mBot10">
                        <table id="tableInfluencer" class="table table-striped table-bordered display colorTable" style="width: 100%" border="0">
                            <thead>
                                <tr>
                                    <th class="hide">DET_INFLUENCER_ID</th>
                                    <th>Influencer Name</th>
                                    <th>Action</th>
                                </tr>
                            </thead>
                            <tbody id="infSchemebody">
                            </tbody>
                        </table>
                    </div>


                </div>
                <div class="modal-footer">
                    <button type="button" id="" onclick="SaveInfluencerScheme();" class="btn btn-success">Save and Exit</button>
                </div>
            </div>

        </div>
    </div>

    <asp:HiddenField ID="hdnLockFromDateedit" runat="server" />
<asp:HiddenField ID="hdnLockToDateedit" runat="server" />
 
 <asp:HiddenField ID="hdnLockFromDatedelete" runat="server" />
    <asp:HiddenField ID="hdnLockToDatedelete" runat="server" />
</asp:Content>


