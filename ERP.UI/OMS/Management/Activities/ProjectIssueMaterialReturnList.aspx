<%--=======================================================Revision History=========================================================================
 1.0     Sanchita    V2.0.41  14-11-2023     	0026953: Data Freeze Required for Project Purchase Return Manual, Material Issue & Material Received
=========================================================End Revision History========================================================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ProjectIssueMaterialReturnList.aspx.cs" Inherits="ERP.OMS.Management.Activities.ProjectIssueMaterialReturnList" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .fwidth {
            width: 350px !important;
        }

        .padTabtype2 > tbody > tr > td {
            padding: 0px 5px;
        }

            .padTabtype2 > tbody > tr > td > label {
                margin-bottom: 0 !important;
                margin-right: 15px;
            }

        .dxeErrorFrameWithoutError_PlasticBlue .dxeControlsCell_PlasticBlue, .dxeErrorFrameWithoutError_PlasticBlue.dxeControlsCell_PlasticBlue {
            padding: 0px !important;
        }
    </style>
    <%--Subhra--%>

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

        #tableProduct > thead > tr > th, #influencerGrid > thead > tr > th {
            background: #1E6AB1;
            color: #fff;
        }

        #tableProduct, #influencerGrid {
            border-top: 1px solid #1E6AB1;
        }

            #tableProduct > thead > tr > th, #influencerGrid > thead > tr > th {
                border-right: 1px solid #1E6AB1;
            }

            #tableProduct > tbody > tr > td, #influencerGrid > tbody > tr > td {
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
        var ReturnId = 0;
        var JVid = "";
        var Action = "Add";
        var AutoJVNumber = "";
        function onPrintJv(id) {
            debugger;
            ReturnId = id;
            cSelectPanel.cpSuccess = "";
            cDocumentsPopup.Show();
            cCmbDesignName.SetSelectedIndex(0);
            cSelectPanel.PerformCallback('Bindalldesignes');
            $('#btnOK').focus();
        }

        function onInfluencerCommissionReturn(id) {
            $("#CmbScheme").val('0');
            $("#txtBillNo").val("")
            $("#txtBillNo").prop('disabled', true)
            $.ajax({
                type: "POST",
                url: "ProjectIssueMaterialReturnList.aspx/GetInfluencerDetails",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ invid: id }),
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

                        cbtnMAdr.SetText(status.Influencer.MainAccount_AccountCode);
                        $("#txtCommAmt").html(status.Influencer.COMM_AMOUNT);
                        $("#mainacdrid").val(status.Influencer.MAINACCOUNT_DR);

                        if (status.Influencer.AUTOJV_NUMBER != "" && status.Influencer.AUTOJV_NUMBER != null) {
                            $("#div_Edit").addClass('hide');
                            AutoJVNumber = status.Influencer.AUTOJV_NUMBER
                            $("#txtBillNo").val(status.Influencer.AUTOJV_NUMBER);
                            $("#txtBillNo").prop('disabled', true);


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
                        //str = str + "<th>Action</th>";
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


                            //str = str + "<td><img onclick='infdeleteClick(" + JSON.stringify(status.Influencer_Details[i].DET_INFLUENCER_ID) + ")' src='../../../assests/images/crs.png' /></td>";
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
                        //str = str + "<th>Basis</th>";
                        //str = str + "<th>Commission Rate/Qty or Commission %</th>";
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



                            if (parseFloat(status.INF_Inv_Products[i].prod_Qty) > 0) {
                                if (status.INF_Inv_Products[i].Applicable_On == "4") {
                                    //str = str + "<td><input type='textbox' class='numeric'  onfocusout='percenLostfocus(" + status.INF_Inv_Products[i].Prod_id + ")' onkeypress='return isNumberKey(event)' id='prodpercen" + status.INF_Inv_Products[i].Prod_id + "' value='" + status.INF_Inv_Products[i].Prod_Percentage + "'disabled></td>";
                                    str = str + "<td><input type='textbox' class='numeric'  onfocusout='AmountLostfocus(" + status.INF_Inv_Products[i].Prod_id + ")' onkeypress='return isNumberKey(event)' id='prodcommamt" + status.INF_Inv_Products[i].Prod_id + "' value='" + status.INF_Inv_Products[i].PROD_COMM_AMOUNT + "' ></td>";
                                }
                                else {
                                    //str = str + "<td><input type='textbox' class='numeric'  onfocusout='percenLostfocus(" + status.INF_Inv_Products[i].Prod_id + ")' onkeypress='return isNumberKey(event)' id='prodpercen" + status.INF_Inv_Products[i].Prod_id + "' value='" + status.INF_Inv_Products[i].Prod_Percentage + "' ></td>";
                                    str = str + "<td><input type='textbox' class='numeric'  onfocusout='AmountLostfocus(" + status.INF_Inv_Products[i].Prod_id + ")' onkeypress='return isNumberKey(event)' id='prodcommamt" + status.INF_Inv_Products[i].Prod_id + "' value='" + status.INF_Inv_Products[i].PROD_COMM_AMOUNT + "' disabled></td>";
                                }
                            }
                            else {
                                // str = str + "<td><input type='textbox' class='numeric'  onfocusout='percenLostfocus(" + status.INF_Inv_Products[i].Prod_id + ")' onkeypress='return isNumberKey(event)' id='prodpercen" + status.INF_Inv_Products[i].Prod_id + "' value='" + status.INF_Inv_Products[i].Prod_Percentage + "' disabled></td>";
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
                        jAlert('Influencer booking not found . Can not add Influencer.', 'Alert');
                    }
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


            $.ajax({
                type: "POST",
                url: "ProjectIssueMaterialReturnList.aspx/SaveInfluencer",
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

          function PerformCallToGridBind() {
              cSelectPanel.PerformCallback('Bindsingledesign');
              cDocumentsPopup.Hide();
              return false;
          }
          function cSelectPanelEndCall(s, e) {
              debugger;
              if (cSelectPanel.cpSuccess != "") {
                  var TotDocument = cSelectPanel.cpSuccess.split(',');
                  var reportName = cCmbDesignName.GetValue();
                  var module = 'Sales_Return';
                  if (TotDocument.length > 0) {
                      for (var i = 0; i < TotDocument.length; i++) {
                          if (TotDocument[i] != "") {
                              window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + ReturnId + '&PrintOption=' + TotDocument[i], '_blank')
                          }
                      }
                  }
              }
              if (cSelectPanel.cpSuccess == "") {
                  if (cSelectPanel.cpChecked != "") {
                      jAlert('Please check Original For Recipient and proceed.');
                  }
                  CselectOriginal.SetCheckState('UnChecked');
                  CselectDuplicate.SetCheckState('UnChecked');
                  CselectTriplicate.SetCheckState('UnChecked');
                  cCmbDesignName.SetSelectedIndex(0);
              }
          }
          var isFirstTime = true;

          function AllControlInitilize() {
              if (isFirstTime) {
                  if (localStorage.getItem('ReturnList_FromDate')) {
                      var fromdatearray = localStorage.getItem('ReturnList_FromDate').split('-');
                      var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                      cFormDate.SetDate(fromdate);
                  }

                  if (localStorage.getItem('ReturnList_ToDate')) {
                      var todatearray = localStorage.getItem('ReturnList_ToDate').split('-');
                      var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                      ctoDate.SetDate(todate);
                  }

                  if (localStorage.getItem('ReturnList_Branch')) {
                      if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('ReturnList_Branch'))) {
                          ccmbBranchfilter.SetValue(localStorage.getItem('ReturnList_Branch'));
                      }

                  }

                  //if ($("#LoadGridData").val() == "ok")
                  //    updateGridByDate();
                  isFirstTime = false;
              }
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
                  localStorage.setItem("ReturnList_FromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
                  localStorage.setItem("ReturnList_ToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
                  localStorage.setItem("ReturnList_Branch", ccmbBranchfilter.GetValue());



                  $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                  $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                  $("#hfBranchID").val(ccmbBranchfilter.GetValue());
                  $("#hfIsFilter").val("Y");
                  cGrdSalesReturn.Refresh();
                  // cGrdSalesReturn.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
              }
          }

    </script>
    <%--Subhra--%>
    <script>
        function OnEWayBillClick(id, EWayBillNumber, EWayBillDate, EWayBillValue) {

            if (EWayBillNumber.trim() != "") {
                ctxtEWayBillNumber.SetText(EWayBillNumber);
            }
            else {
                ctxtEWayBillNumber.SetText("");
            }

            if (EWayBillDate.trim() != "" && EWayBillDate.trim() != "01-01-1970" && EWayBillDate.trim() != "01-01-1900") {
                cdt_EWayBill.SetText(EWayBillDate);
            }
            else {
                cdt_EWayBill.SetText("");
            }
            if (EWayBillValue.trim() != "0.00" && EWayBillValue.trim() != "") {
                ctxtEWayBillValue.SetText(EWayBillValue);
            }
            else {
                ctxtEWayBillValue.SetText("0.0");
            }
            $('#hddnSalesReturnID').val(id);
            cPopup_EWayBill.Show();
            ctxtEWayBillNumber.Focus();
        }
        function GetEWayBillDateFormat(today) {
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
                today = yyyy + '-' + mm + '-' + dd;
            }

            return today;
        }
        function CallEWayBill_save() {

            var ReturnID = $("#<%=hddnSalesReturnID.ClientID%>").val();
            var UpdateEWayBill = ctxtEWayBillNumber.GetValue();
            if (UpdateEWayBill == "0") {
                UpdateEWayBill = "";
            }
            if (cdt_EWayBill.GetValue() == "" && cdt_EWayBill.GetValue() == null) {
                var EWayBillDate = "1990-01-01";
            }
            else {
                var EWayBillDate = GetEWayBillDateFormat(new Date(cdt_EWayBill.GetValue()));
            }

            var EWayBillValue = ctxtEWayBillValue.GetValue();

            $.ajax({
                type: "POST",
                url: "ProjectIssueMaterialReturnList.aspx/UpdateEWayBill",
                data: JSON.stringify({
                    ReturnID: ReturnID, UpdateEWayBill: UpdateEWayBill, EWayBillDate: EWayBillDate, EWayBillValue: EWayBillValue
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var status = msg.d;
                    if (status == "1") {
                        jAlert("Saved successfully.");
                        //ctxtEWayBillNumber.SetText("");
                        cPopup_EWayBill.Hide();
                        cGrdSalesReturn.Refresh();
                    }
                    else if (status == "-10") {
                        jAlert("Data not saved.");
                        cPopup_EWayBill.Hide();
                    }
                }
            });
        }
        function CancelEWayBill_save() {
            cPopup_EWayBill.Hide();
        }
        function OnclickViewAttachment(obj) {
            //var URL = '/OMS/Management/Activities/SalesReturn_Document.aspx?idbldng=' + obj + '&type=SalesReturn';
            var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=IssueMaterialReturn';
            window.location.href = URL;
        }
        document.onkeydown = function (e) {
            if (event.keyCode == 18) isCtrl = true;


            if (event.keyCode == 65 && isCtrl == true) { //run code for alt+a -- ie, Add

                StopDefaultAction(e);
                OnAddButtonClick();
            }

        }

        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }
        function OnAddButtonClick() {
            var url = 'ProjectIssueMaterialReturn.aspx?key=' + 'ADD';
            window.location.href = url;
        }
        function OnMoreInfoClick(keyValue) {


            var ActiveUser = '<%=Session["userid"]%>'
            if (ActiveUser != null) {
                $.ajax({
                    type: "POST",
                    url: "ProjectIssueMaterialReturnList.aspx/GetEditablePermission",
                    data: "{'ActiveUser':'" + ActiveUser + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var status = msg.d;
                        var url = 'ProjectIssueMaterialReturn.aspx?key=' + keyValue + '&Permission=' + status + '&type=MR';
                        window.location.href = url;
                    }
                });
            }
        }

        ////##### coded by Samrat Roy - 17/04/2017 - ref IssueLog(P Order - 70) 
        ////Add an another param to define request type 
        function OnViewClick(keyValue) {
            var url = 'ProjectIssueMaterialReturn.aspx?key=' + keyValue + '&req=V' + '&type=MR';
            window.location.href = url;
        }

        function OnClickDelete(keyValue) {
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    cGrdSalesReturn.PerformCallback('Delete~' + keyValue);
                }
            });
        }
        function OnEndCallback(s, e) {

            if (cGrdSalesReturn.cpDelete != null) {
                jAlert(cGrdSalesReturn.cpDelete);

                cGrdSalesReturn.cpDelete = null;
                cGrdSalesReturn.Refresh();
                // window.location.href = "SalesReturnList.aspx";
            }
        }
        //function OnClickDelete(keyValue) {
        //    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
        //        if (r == true) {
        //            cGrdQuotation.PerformCallback('Delete~' + keyValue);
        //        }
        //    });
        //}
        function gridRowclick(s, e) {
            $('#GrdSalesReturn').find('tr').removeClass('rowActive');
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
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Material Received</h3>
        </div>
    </div>
    <div class="form_main">
        <div class="clearfix">
            <% if (rights.CanAdd)
               { %>
            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus"></i></span><span> Material Received</span> </a>
            <%} %>

            <% if (rights.CanExport)
               { %>
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>
            <% } %>

            <table class="padTabtype2 pull-right" id="gridFilter">
                <tr>
                    <td>
                        <label>From Date</label></td>
                    <td>
                        <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dxe:ASPxDateEdit>
                    </td>
                    <td>
                        <label>To Date</label>
                    </td>
                    <td>
                        <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
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
                        <input type="button" value="Show" class="btn btn-primary" onclick="updateGridByDate()" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxDocumentsPopup" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
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
                    <%-- <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" ClientSideEvents-EndCallback="cSelectPanelEndCall">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">


                                <dxe:ASPxCheckBox ID="selectOriginal" Text="Original For Recipient" runat="server" ToolTip="Select Original" ClientSideEvents-CheckedChanged="OrginalCheckChange"
                                    ClientInstanceName="CselectOriginal">
                                </dxe:ASPxCheckBox>

                                <dxe:ASPxCheckBox ID="selectDuplicate" Text="Duplicate For Transporter" runat="server" ToolTip="Select Duplicate" ClientSideEvents-CheckedChanged="DuplicateCheckChange"
                                    ClientInstanceName="CselectDuplicate">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectTriplicate" Text="Triplicate For Supplier" runat="server" ToolTip="Select Triplicate"
                                    ClientInstanceName="CselectTriplicate">
                                </dxe:ASPxCheckBox>

                                <dxe:ASPxComboBox ID="CmbDesignName" ClientInstanceName="cCmbDesignName" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                   
                                </dxe:ASPxComboBox>

                                <div class="text-center pTop10">
                                    <dxe:ASPxButton ID="btnOK" ClientInstanceName="cbtnOK" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind();}" />
                                    </dxe:ASPxButton>

                                </div>
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>--%>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>
     <%--Rev 1.0--%>
    <div id="spnEditLock" runat="server" style="display:none; color:red;text-align:center"></div>
    <div id="spnDeleteLock" runat="server" style="display:none; color:red;text-align:center"></div>
    <%--End of Rev 1.0--%>
    <div class="GridViewArea relative">
        <dxe:ASPxGridView ID="GrdSalesReturn" runat="server" KeyFieldName="SrlNo" AutoGenerateColumns="False"
            Width="100%" ClientInstanceName="cGrdSalesReturn" OnCustomCallback="GrdSalesReturn_CustomCallback" SettingsBehavior-AllowFocusedRow="true"
            DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
            <SettingsSearchPanel Visible="true" Delay="5000" />
            <Columns>
                <dxe:GridViewDataTextColumn Caption="Document Number" FieldName="IssueReturnNo"
                    VisibleIndex="0" FixedStyle="Left" Width="140px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="Return_Date" Caption="Posting Date" SortOrder="Descending">
                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <EditFormSettings Visible="True"></EditFormSettings>
                </dxe:GridViewDataTextColumn>
                <%--<dxe:GridViewDataTextColumn Caption="Date" FieldName="Return_Date"
                    VisibleIndex="1" FixedStyle="Left" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>--%>

                <dxe:GridViewDataTextColumn Caption="Issue Number(s)" FieldName="Invoice"
                    VisibleIndex="2" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Date(s)" FieldName="InvoiceDate"
                    VisibleIndex="3">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Customer" FieldName="CustomerName"
                    VisibleIndex="4" FixedStyle="Left" Width="210px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Amount" FieldName="Amount"
                    VisibleIndex="5" FixedStyle="Left" Width="110px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="E-Way Bill No" FieldName="EWayBillNumber" VisibleIndex="6" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="" FieldName="PlaceOfSupply" VisibleIndex="7" Width="0"> <%--Place of Supply[GST]--%>

                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="Return_CreateUser"
                    VisibleIndex="8">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Last Update On" FieldName="Return_CreateDateTime"
                    VisibleIndex="9">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Updated By" FieldName="Return_ModifyUser"
                    VisibleIndex="10">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Project Name" FieldName="Proj_Name" Width="150px" VisibleIndex="11" Settings-AllowAutoFilter="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="true" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="12" Width="0">
                    <DataItemTemplate>
                        <div class='floatedBtnArea'>

                            <% if (rights.CanView)
                               { %>
                            <a href="javascript:void(0);" onclick="OnViewClick('<%#Eval("Return_Id")%>')" class="" title="">
                                <span class='ico ColorFive'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                            <% } %>
                            <% if (rights.CanEdit)
                               { %>
                            <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%#Eval("Return_Id")%>')" class="" title="">
                                <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a><%} %>
                            <% if (rights.CanDelete)
                               { %>
                            <a href="javascript:void(0);" onclick="OnClickDelete('<%#Eval("Return_Id")%>')" class="" title="">
                                <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a><%} %>
                            <%-- <a href="javascript:void(0);" onclick="OnClickCopy('<%# Container.KeyValue %>')" class="pad" title="Copy ">
                            <i class="fa fa-copy"></i></a>--%>
                            <%--   <% if (rights.CanEdit)
                                       { %>
                        <a href="javascript:void(0);" onclick="OnClickStatus('<%# Container.KeyValue %>')" class="pad" title="Status">
                            <img src="../../../assests/images/verified.png" /></a><%} %>--%>
                            <% if (rights.CanView)
                               { %>
                            <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%#Eval("Return_Id")%>')" class="" title="">
                                <span class='ico ColorSix'><i class='fa fa-paperclip'></i></span><span class='hidden-xs'>View Attachment</span>
                            </a><%} %>
                            <a href="javascript:void(0);" onclick="OnEWayBillClick('<%#Eval("Return_Id")%>','<%#Eval("EWayBillNumber") %>','<%#Eval("EWayBillDate") %>','<%#Eval("EWayBillValue") %>')" class="" title="">
                                <span class='ico ColorFour'><i class='fa fa-file-text-o'></i></span><span class='hidden-xs'>Update E-Way Bill</span>
                                <% if (rights.CanPrint)
                                   { %>
                                <a href="javascript:void(0);" onclick="onPrintJv('<%#Eval("Return_Id")%>')" class="" title="">
                                    <span class='ico ColorSeven'><i class='fa fa-print'></i></span><span class='hidden-xs'>Print</span>
                                </a><%} %>
                        </div>
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <HeaderTemplate><span></span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
            </Columns>
            <SettingsContextMenu Enabled="true"></SettingsContextMenu>
            <ClientSideEvents EndCallback="OnEndCallback" RowClick="gridRowclick" />
            <%-- <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </SettingsPager>--%>
            <SettingsPager PageSize="10">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
            </SettingsPager>
            <%-- <SettingsSearchPanel Visible="True" />--%>
            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsLoadingPanel Text="Please Wait..." />
        </dxe:ASPxGridView>
        <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="v_ProjectIssueMaterialReturnList" />
        <asp:HiddenField ID="hiddenedit" runat="server" />
    </div>
    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" GridViewID="GrdSalesReturn" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>

    <%--SUBHRA--%>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" ClientSideEvents-EndCallback="cSelectPanelEndCall">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxCheckBox ID="selectOriginal" Text="Original For Recipient" runat="server" ToolTip="Select Original"
                                    ClientInstanceName="CselectOriginal">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectDuplicate" Text="Duplicate For Transporter" runat="server" ToolTip="Select Duplicate"
                                    ClientInstanceName="CselectDuplicate">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectTriplicate" Text="Triplicate For Supplier" runat="server" ToolTip="Select Triplicate"
                                    ClientInstanceName="CselectTriplicate">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxComboBox ID="CmbDesignName" ClientInstanceName="cCmbDesignName" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                </dxe:ASPxComboBox>
                                <div class="text-center pTop10">
                                    <dxe:ASPxButton ID="btnOK" ClientInstanceName="cbtnOK" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind();}" />
                                    </dxe:ASPxButton>
                                </div>
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>

    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
    <div>
        <asp:HiddenField ID="hfIsFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfBranchID" runat="server" />
        <asp:HiddenField ID="hddnSalesReturnID" runat="server" />
         <%--Rev 1.0--%>
        <asp:HiddenField ID="hdnLockFromDate" runat="server" />
        <asp:HiddenField ID="hdnLockToDate" runat="server" />
        <asp:HiddenField ID="hdnLockFromDateCon" runat="server" />
        <asp:HiddenField ID="hdnLockToDateCon" runat="server" />
   
        <asp:HiddenField ID="hdnLockFromDateedit" runat="server" />
        <asp:HiddenField ID="hdnLockToDateedit" runat="server" /> 
        <asp:HiddenField ID="hdnLockFromDatedelete" runat="server" />
        <asp:HiddenField ID="hdnLockToDatedelete" runat="server" />
        <%--End of Rev 1.0--%>
    </div>
    <dxe:ASPxPopupControl ID="Popup_EWayBill" runat="server" ClientInstanceName="cPopup_EWayBill"
        Width="400px" HeaderText="Update E-Way Bill" PopupHorizontalAlign="WindowCenter"
        BackColor="white" Height="150px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="Top clearfix">
                    <table style="width: 100%; margin: 0 auto; margin-top: 5px;">
                        <tr>
                            <label>
                                <dxe:ASPxLabel ID="ASPxLabel12" runat="server" Text="E-Way Bill Number">
                                </dxe:ASPxLabel>
                            </label>

                            <dxe:ASPxTextBox ID="txtEWayBillNumber" MaxLength="12" ClientInstanceName="ctxtEWayBillNumber"
                                runat="server" Width="100%">
                                <MaskSettings Mask="<0..999999999999>" AllowMouseWheel="false" />
                                <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                            </dxe:ASPxTextBox>
                        </tr>
                        <tr>
                            <td>
                                <label style="margin-top: 6px">
                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="E-Way Bill Date">
                                    </dxe:ASPxLabel>
                                </label>
                                <dxe:ASPxDateEdit ID="dt_EWayBill" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdt_EWayBill" Width="100%" UseMaskBehavior="True">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label style="margin-top: 6px">
                                    <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="E-Way Bill Value">
                                    </dxe:ASPxLabel>
                                </label>
                                <dxe:ASPxTextBox ID="txtEWayBillValue" ClientInstanceName="ctxtEWayBillValue"
                                    runat="server" Width="100%">
                                    <MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" />
                                    <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                                </dxe:ASPxTextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="margin-top: 10px;">
                        <input id="btnEWayBillSave" class="btn btn-primary" onclick="CallEWayBill_save()" type="button" value="Save" />
                        <input id="btnEWayBillCancel" class="btn btn-danger" onclick="CancelEWayBill_save()" type="button" value="Cancel" />
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>

    <%-------Influencer Return------%>

    <div class="modal fade pmsModal w80" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog" role="document" style="width: 95%;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalLabel">Influencer</h4>
                </div>
                <div class="modal-body" style="overflow-y: scroll; max-height: 450px; overflow-x: hidden;">
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
                    <div class="clearfix">
                        <ul class="smallList  hide">
                            <li>On Qty - Commission calculated on the Qty of the selected Item</li>
                            <li>Amount before GST - Commission calculated as percentage of the Amount before GST is charged.</li>
                            <li>Amount after GST - Commission calculated as percentage of the Amount with GST charged.</li>
                            <li>Flat Value - Flat commission amount irrespective of Qty and Value</li>
                        </ul>
                    </div>

                    <div class="clearfix padTopBot mBot10">
                        <table id="tableProduct" class="table table-striped table-bordered display " style="width: 100%">
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
                                </dxe:ASPxButtonEdit>
                            </div>
                            <div class="col-md-2 hide" style="padding-top: 8px;">
                                <label>Posting Ledger Cr.</label>
                                <dxe:ASPxButtonEdit runat="server" ID="btnMainAccount" ClientEnabled="false" ClientInstanceName="cbtnMainAccount" class="form-control hide" Width="100%">
                                    <Buttons>
                                        <dxe:EditButton>
                                        </dxe:EditButton>
                                    </Buttons>
                                </dxe:ASPxButtonEdit>
                            </div>
                            <div class="col-md-3 hide">
                                <label>Influencer</label>
                                <dxe:ASPxButtonEdit runat="server" ID="txtInfluencer" ClientInstanceName="ctxtInfluencer" class="form-control" Width="100%">
                                    <Buttons>
                                        <dxe:EditButton>
                                        </dxe:EditButton>
                                    </Buttons>
                                </dxe:ASPxButtonEdit>
                            </div>
                            <div class="col-md-2 padTop25 hide">
                                <dxe:ASPxButton runat="server" AutoPostBack="False" UseSubmitBehavior="false" ID="btnSaveInfluencer" ClientInstanceName="cbtnSaveInfluencer" CssClass="btn btn-primary onDxe" Text="+">
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

    <input type="hidden" id="ddlBranch" />
    <input type="hidden" id="invid" />
    <input type="hidden" id="infid" />
    <input type="hidden" id="mainaccrid" />
    <input type="hidden" id="mainacdrid" />

</asp:Content>
