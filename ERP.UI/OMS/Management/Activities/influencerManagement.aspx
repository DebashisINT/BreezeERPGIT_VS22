<%--================================================== Revision History =============================================
1.0   Pallab    V2.0.38      23-05-2023          0026199: Influencer Opening module design modification & check in small device
2.0   Pallab    V2.0.39      07-08-2023          0026686: Influencer Opening module all bootstrap modal outside click event disable
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="influencerManagement.aspx.cs" Inherits="ERP.OMS.Management.Activities.influencerManagement" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchPopup.js"></script>

    <style>
         #InfluencerModel {
            z-index:1041;
        }
        /* Tab Navigation */
        .newtab .nav-tabs {
            margin: 0;
            padding: 0;
            border: 0;    
        }
        .newtab .nav-tabs > li > a {
            background: #eeeeee;
            border-radius: 0;
        }
        .newtab .nav-tabs > li > a:hover {
            background: #e1e1e1 !important;
                border-color: #e1e1e1;
                COLOR: #499aea;
        }
        .newtab .nav-tabs > li.active > a,
        .newtab .nav-tabs > li.active > a:hover {
                    border-color: #6b94ca !important;
                    COLOR: #FFF;
                    background: linear-gradient(#699dde, #3d6ca9) !important;
                    border-radius: 2px 2px 0 0;
                        text-transform: uppercase;
        }

        /* Tab Content */
        .newtab .tab-pane {
                background: #ffffff;
                /* box-shadow: 0 0 4px rgba(0,0,0,.4); */
                border-radius: 0;
                padding: 10px;
                border: 1px solid #dadada;
        }
        .modal {
            z-index:1050;
        }
        .padTab>tbody>tr>td{
            padding-right:15px
        }
    </style>
    <script>
        var JVid = "";
        var Action = "Add";
        var AutoJVNumber = "";

        function onAddInfluencer(invId) {
            $("#CmbScheme").val('0');
            $("#txtBillNo").val("")
            $("#txtBillNo").prop('disabled', true)
            $.ajax({
                type: "POST",
                url: "influencerManagement.aspx/GetInfluencerDetails",
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
        function btnMainAccountClick(s, e) {
            $('#MainAccountModel').modal('show');
        }
        function InfluencerBtnClick(s, e) {
            $('#CustModel').modal('show');
        }

        function SetMainAccountdr(Id, name, e) {

            $("#mainacdrid").val(Id);
            cbtnMAdr.SetText(name);
            $('#MainAccountModeldr').modal('toggle');
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


        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            //console.log(charCode);
            if (charCode == 46)
                return true;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

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

        function btnMainAccountKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
                $('#MainAccountModel').modal('show');
            }
        }

        function InfluencerKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
                $('#CustModel').modal('show');
            }
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
            obj.IsOpening = "1";

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
                        //console.log(type);

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

        function btnMAdrClick(s, e) {
            $('#MainAccountModeldr').modal('show');
        }

        function btnMAdrKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
                $('#MainAccountModeldr').modal('show');
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

                localStorage.setItem("InfluencerListFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("InfluencerListToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("InfluencerListBranch", ccmbBranchfilter.GetValue());

                $('#branchName').text(ccmbBranchfilter.GetText());
               
                
                    //cGrdQuotation.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
                    $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                    $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                    $("#hfBranchID").val(ccmbBranchfilter.GetValue());
                    $("#hfIsFilter").val("Y");
                    cGrdQuotation.Refresh();
                
            }
        }

        $(document).ready(function () {

            if (localStorage.getItem('InfluencerListFromDate')) {
                var fromdatearray = localStorage.getItem('InfluencerListFromDate').split('-');
                var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                cFormDate.SetDate(fromdate);
            }

            if (localStorage.getItem('InfluencerListToDate')) {
                var todatearray = localStorage.getItem('InfluencerListToDate').split('-');
                var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                ctoDate.SetDate(todate);
            }
            if (localStorage.getItem('InfluencerListBranch')) {
                if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('InfluencerListBranch'))) {
                    ccmbBranchfilter.SetValue(localStorage.getItem('InfluencerListBranch'));
                }

            }
        });


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
                        //console.log(value);
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }

    </script>

    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 0;
        }

        #gridAdvanceAdj {
            max-width: 99% !important;
        }
        #FormDate, #toDate, #dtTDate, #dt_PLQuote, #dt_PlQuoteExpiry {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        select
        {
            -webkit-appearance: auto;
        }

        .calendar-icon
        {
            right: 20px;
        }

        .panel-title h3
        {
            padding-top: 0px !important;
        }

        .fakeInput
        {
                min-height: 30px;
    border-radius: 4px;
        }
        
    </style>
    <%--Rev end 1.0--%>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>Influencer Opening</h3>
        </div>
    </div>
        <div class="form_main newtab">
        <!-- Nav tabs -->
        <ul class="nav nav-tabs" role="tablist">
          <li class="active">
              <a href="#home" role="tab" data-toggle="tab">
                  <icon class="fa fa-home"></icon> Influencer
              </a>
          </li>
          <%--<li><a href="#profile" role="tab" data-toggle="tab">
              <i class="fa fa-user"></i> Profile
              </a>
          </li>
          <li>
              <a href="#messages" role="tab" data-toggle="tab">
                  <i class="fa fa-envelope"></i> Messages
              </a>
          </li>
          <li>
              <a href="#settings" role="tab" data-toggle="tab">
                  <i class="fa fa-cog"></i> Settings
              </a>
          </li>--%>
        </ul>
    
        <!-- Tab panes -->
        <div class="tab-content">
          <div class="tab-pane fade active in" id="home">
              <table class="padTab" style="margin-top: 5px;">
            <tr>
                <td>
                    <label>From Date</label></td>
                <%--Rev 1.0: "for-cust-icon" class add --%>
                <td class="for-cust-icon">
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                    <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
                </td>
                <td>
                    <label>To Date</label>
                </td>
                <%--Rev 1.0: "for-cust-icon" class add --%>
                <td class="for-cust-icon">
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                    <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
                </td>
                <td>Unit</td>
                <td>
                    <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                    </dxe:ASPxComboBox>
                </td>
                <td class="">
                    <input type="button" value="Show" class="btn btn-primary " onclick="updateGridByDate()" />
                    
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary pull-right btn-radius hide" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLSX</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>
                    
                </td>

            </tr>

        </table>

                  <div class="GridViewArea relative">
                            <%--<dxe:ASPxGridView ID="GrdQuotation" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                Width="100%" ClientInstanceName="cGrdQuotation" OnCustomCallback="GrdQuotation_CustomCallback" SettingsBehavior-AllowFocusedRow="true"
                                SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" OnDataBinding="GrdQuotation_DataBinding" Settings-HorizontalScrollBarMode="Auto"
                                SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true" SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true" SettingsBehavior-ColumnResizeMode="Control"
                                OnSummaryDisplayText="ShowGrid_SummaryDisplayText">--%>
                            <dxe:ASPxGridView ID="GrdQuotation" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                Width="100%" ClientInstanceName="cGrdQuotation"  SettingsBehavior-AllowFocusedRow="true"
                                SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
                                SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto"
                                DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
                                <SettingsSearchPanel Visible="True" Delay="5000" />
                                <%--SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true" SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true" --%>
                                <ClientSideEvents  RowClick="gridcrmCampaignclick" />
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
                                        VisibleIndex="1">
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
                                               
<%--                                                <% if (rights.Influencer)
                                                   { %>--%>
                                                <a href="javascript:void(0);" class="pad" title="" id="Add_Influencer" onclick="onAddInfluencer('<%# Container.KeyValue %>')">
                                                    <span class='ico ColorSeven'><i class='fa fa-user-plus' aria-hidden='true'></i></span><span class='hidden-xs'>Add Influencer</span>
                                                </a>
                                                <%--<%} %>--%>
                                                
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

      <%--Rev 2.0--%>
      <%--<div class="modal fade pmsModal w80" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">--%>
      <div class="modal fade pmsModal w80" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" data-backdrop="static" data-keyboard="false">
      <%--Rev end 2.0--%>
        <div class="modal-dialog" role="document" style="width: 95%;">
            <div class="modal-content">
                <div class="modal-header">

                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalLabel">Influencer</h4>
                </div>
                <div class="modal-body" style="overflow-y: scroll; max-height: 450px; overflow-x: hidden;">

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
                        <ul class="smallList" style="color:red">
                            <li>On Qty - Commission calculated on the Qty of the selected Item</li>
                            <li>Amount before GST - Commission calculated as percentage of the Amount before GST is charged.</li>
                            <li>Amount after GST - Commission calculated as percentage of the Amount with GST charged.</li>
                            <li>Flat Value - Flat commission amount irrespective of Qty and Value</li>
                        </ul>
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
                        <div class="col-md-3 hide">
                            <div class="mroonText">Something goes here</div>
                        </div>

                    </div>

                    <div class="borderTopBottom clear padTopBot mBot10 ">

                        <div class="row mBot10 ">
                            <div class="col-md-3">
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
                            <div class="col-md-1" style="padding-top: 25px;">
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
    <%--Rev 2.0--%>
    <%--<div class="modal fade pmsModal w60" id="MainAccountModel" role="dialog">--%>
    <div class="modal fade pmsModal w60" id="MainAccountModel" role="dialog" data-backdrop="static" data-keyboard="false">
      <%--Rev end 2.0--%>
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
    <%--Rev 2.0--%>
    <%--<div class="modal fade pmsModal w60" id="MainAccountModeldr" role="dialog">--%>
    <div class="modal fade pmsModal w60" id="MainAccountModeldr" role="dialog" data-backdrop="static" data-keyboard="false">
      <%--Rev end 2.0--%>
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
        <input type="hidden" id="mainacdrid" />
    </div>




    </div>
    <%--Rev 2.0--%>
     <%--<div class="modal fade pmsModal w60" id="CustModel" role="dialog">--%>
     <div class="modal fade pmsModal w60" id="CustModel" role="dialog" data-backdrop="static" data-keyboard="false">
      <%--Rev end 2.0--%>
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

          
              
              
          </div>
          <div class="tab-pane fade" id="profile">
              <h2>Profile Content Goes Here</h2>
             
          </div>
          <div class="tab-pane fade" id="messages">
              <h2>Messages Content Goes Here</h2>
              
          </div>
          <div class="tab-pane fade" id="settings">
              <h2>Settings Content Goes Here</h2>
              
          </div>
        </div>

    </div>
    </div>
</asp:Content>
