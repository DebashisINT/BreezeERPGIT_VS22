<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Title="Sales Visit"
    EnableEventValidation="false" Inherits="ERP.OMS.Management.Activities.management_Activities_CRMSalesVisitWithIFrame" CodeBehind="CRMSalesVisitWithIFrame.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script language="javascript" type="text/javascript">
        function Btnsalesorder() {

            var SalesId = '<%=Convert.ToString(Request.QueryString["TransSale"])%>';
            var TypeId = '<%=Int32.Parse(Request.QueryString["type"])%>';
            // alert(SalesId + ' ' + TypeId)


            //string strCustomer = oDBEngine.ExeSclar("select sls_contactlead_id from tbl_trans_sales where sls_id=" + Request.QueryString["SalId"]);

            $.ajax({
                type: "POST",
                url: "CRMSalesVisitWithIFrame.aspx/GetBOIsExistInBI",
                data: "{'keyValue':'" + SalesId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,//Added By:Subhabrata
                success: function (msg) {
                    debugger;
                    var url = '';
                    var status = msg.d;
                    if (status == "D") {
                        jAlert('Customer is marked as Dormant. Cannot proceed.', 'Alert Dialog', function (r) {
                            if (r == true) {
                                return false;
                            }
                        });
                    }
                    else {
                        url = "/OMS/Management/Activities/SalesOrderAdd.aspx?key=ADD" + "&type=" + TypeId + "&SalId=" + SalesId;
                        popup.SetContentUrl(url);
                        popup.Show();
                    }


                }
            });
           <%-- var SalesId = '<%=Convert.ToString(Request.QueryString["TransSale"])%>';
            var TypeId = '<%=Int32.Parse(Request.QueryString["type"])%>';
            // alert(SalesId + ' ' + TypeId)


         

            var url = "/OMS/Management/Activities/SalesOrderAdd.aspx?key=ADD" + "&type=" + TypeId + "&SalId=" + SalesId;
            window.open(url);--%>


        }

        function Btnsalesquotation() {
            var DisposionIdCat = $('#<%=hdnCallDisposion.ClientID %>').val().split('~');
            var DisposionId = DisposionIdCat[0, 0];


            var SalesId = '<%=Convert.ToString(Request.QueryString["TransSale"])%>';
             var TypeId = '<%=Int32.Parse(Request.QueryString["type"])%>';
            //  alert(SalesId + ' ' + TypeId)

            //   var url = "http://localhost:7665/OMS/Management/Activities/SalesQuotation.aspx?key=ADD" + "&type=" + TypeId + "&SalId=" + SalesId;
            var url = '';


            $.ajax({
                type: "POST",
                url: "CRMSalesVisitWithIFrame.aspx/GetBOIsExistInBI",
                data: "{'keyValue':'" + SalesId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,//Added By:Subhabrata
                success: function (msg) {
                    debugger;
                    var url = '';
                    var status = msg.d;

                    if (DisposionId == '34') {

                        url = "/OMS/Management/Activities/SalesQuotation.aspx?key=ADD" + "&type=" + TypeId + "&SalId=" + SalesId;
                        popupProforma.SetContentUrl(url);
                        popupProforma.Show();
                    }
                    else {
                        if (status == "D") {
                            jAlert('Customer is marked as Dormant. Cannot proceed.', 'Alert Dialog', function (r) {
                                if (r == true) {
                                    return false;
                                }
                            });
                        }
                        else {
                            url = "/OMS/Management/Activities/SalesQuotation.aspx?key=ADD" + "&type=" + TypeId + "&SalId=" + SalesId;
                            popupProforma.SetContentUrl(url);
                            popupProforma.Show();
                        }

                    }



                }
            });

          <%--  var SalesId = '<%=Convert.ToString(Request.QueryString["TransSale"])%>';
             var TypeId = '<%=Int32.Parse(Request.QueryString["type"])%>';
             //  alert(SalesId + ' ' + TypeId)
           
             var url = "/OMS/Management/Activities/SalesQuotation.aspx?key=ADD" + "&type=" + TypeId + "&SalId=" + SalesId;

             window.open(url);--%>


         }
        function popUpRedirect(obj, obj1) {
            //   jAlert('Saved successfully');
            //   alert('Saved successfully');
            // alert(obj);
            //  window.open(obj);

            var msg = '';

            var msgpage = '';

            if (obj1 == "1")
            { msgpage = "Document"; }
            else if (obj1 == "2")
            { msgpage = "Closed"; }
            else if (obj1 == "3")
            { msgpage = "Future"; }
            else if (obj1 == "5")
            { msgpage = "Clarification"; }

            msg = 'Saved successfully. ' + 'Record has been moved to ' + msgpage + ' Section';
            jAlert(msg, 'Sales Activity', function () {

                window.location.href = obj;
                //  window.open(obj);
            });

        }



        function SameDate() {
            jAlert('Next date cannot be same date.', 'Sales Activity');
            return false;
        }

        function MailFetchAssignto() {

            return $("mailassignfield").val();
        }

        function DateCompletedChangedNextCall(s, e) {
            var CurrDate = new Date();
            var NextcallDate = s.GetDate();
            var datediff = (NextcallDate - CurrDate) / (1000 * 60 * 60 * 24)
            if (datediff > 30) {

                jAlert('Selected date is 1 month later than current date.Cannot Proceed', 'Sales Activity', function () {
                    s.SetDate(CurrDate);
                });
            }
            else {
                //alert('No');
            }

        }

        $(document).ready(function () {

            $("#btnUpdateVisit").click(function () {
                //$(this).addClass('active');
                //$('#btnPhoneFollowUP').removeClass('active');
                EnableChoosen();
            })
            $("#btnPhoneFollowUP").click(function () {
                 EnableChoosen();
                //$(this).addClass('active');
                //$('#btnUpdateVisit').removeClass('active');
            })
            $("#btnSavePhoneCallDetails").click(function () {
                var flag = true;
                var Notes = txtNotes.GetText();

                var drp = document.getElementById("TxtOut")
                var st = drp.value;
                var st = drp.value.split("|");
                if (Notes == "") {
                    flag = false;
                    $('#MandatoryNotes').attr('style', 'display:block');
                }
                else { $('#MandatoryNotes').attr('style', 'display:none'); }


                var noneChecked = false;
                var visitStstbl = document.getElementById("ddl_activitystatus");
                for (var inp = 0 ; inp < visitStstbl.tBodies[0].children[0].children.length; inp++) {
                    if (visitStstbl.tBodies[0].children[0].children[inp].children[0].checked) {
                        noneChecked = true;
                    }
                }

                if (noneChecked == false) {
                    jAlert("Please select Sales Visit Status");
                    flag = false;
                }


               


                //var drpVal1 = drpVal.split("|");              
                if ((st[1] == 1) || (st[1] == 2) || (st[1] == 3) || (st[1] == 4) || (st[1] == 5) || (st[1] == 9) || (st[1] == 10) || (st[1] == 11) || (st[1] == 12) || (st[1] == 13)) {
                    CallDate = tnextdate.GetValue();

                    if (CallDate == null || CallDate == "") {
                        flag = false;
                        $('#MandatoryVisitDate').attr('style', 'display:block');
                    }
                    else { $('#MandatoryVisitDate').attr('style', 'display:none'); }
                }

                document.getElementById('txtCountry_hidden').value = document.getElementById('lstCountry').value;
                document.getElementById('txtState_hidden').value = document.getElementById('lstState').value;
                document.getElementById('txtCity_hidden').value = document.getElementById('lstCity').value;
                //alert(flag);

                return flag;
            })
        });


        function SearchByBranchName(obj1, obj2, obj3) {
            ajax_showOptions(obj1, obj2, obj3);
        }
        FieldName = 'lblFieldName';

        function OnExpensesButtonClick() {
            var url = 'sales_conveyence.aspx';
            OnMoreInfoClick(url, "Modify Lead Details", '940px', '450px', "Y");
        }

        function OnMoreInfoClick1(Id) {
            frmOpenNewWindow1("ShowHistory_Phonecall.aspx?id1=" + Id, 300, 800);
        }


        function OnButton1ClickModify(url, title) {
            OnMoreInfoClick(url, title, '940px', '450px', "Y");
        }

        function OnButton3ClickModify(title) {
            var url = "ShowHistory_Phonecall.aspx?id1=" + Id;
            OnMoreInfoClick(url, title, '940px', '450px', "Y");

        }


        function frmOpenNewWindow1(location, v_height, v_weight) {
            var x = (screen.availHeight - v_height) / 2;
            var y = (screen.availWidth - v_weight) / 2
            window.open(location, "Search_Conformation_Box", "height=" + v_height + ",width=" + v_weight + ",top=" + x + ",left=" + y + ",location=no,directories=no,menubar=no,toolbar=no,status=yes,scrollbars=no,resizable=no,dependent=no");
        }
        var chkobj;
        var objchk = null;
        function showPanel(obj, msg) {
            // alert(obj + msg)
        }
        function chkclicked(obj, msg12) {
            var txt = document.getElementById("hiddenleadid")
            if (objchk == null) {
                objchk = obj;
                objchk.checked = true;
            }
            else {
                objchk.checked = false;
                objchk = obj;
                objchk.checked = true;
            }
            txt.value = msg12;
        }
        function adddatetime(obj1, adddays, addtime) {
            var t
            var t1
            var old_date
            var lst_id
            var new_date
            var txt_date
            txt_date = adddays + " " + addtime;
        }

        function Changedata(txtdate, txttime, nowdate, nowtime, cdate, ctime) {

            var a = document.getElementById("txtStartDate")
            a.value = nowdate + " " + nowtime
            var b = document.getElementById("txtEndTime")
            b.value = cdate + " " + ctime
            var drp = document.getElementById("TxtOut")
            var drpVal = drp.value;

            var drpVal1 = drpVal.split("|");

            if ((drpVal1[1] == 1) || (drpVal1[1] == 2) || (drpVal1[1] == 3) || (drpVal1[1] == 4) || (drpVal1[1] == 5)) {
                document.getElementById("lblNextVisitDate").style.display = "block";
                document.getElementById("lblNextVsDate").style.display = "block";

                document.getElementById("lblNextVisitPlace").style.display = "block";
                document.getElementById("ASPxNextVisit").style.display = "block";
            }
            else {
                if ((drpVal1[1] == 9) || (drpVal1[1] == 10)) {
                    document.getElementById("txtStartDate").disabled = false
                }
                else {
                    if ((drpVal1[1] == 11) || (drpVal1[1] == 12)) {
                        document.getElementById("txtStartDate").disabled = false

                    }
                    else {
                        if (drpVal1[1] == 13) {
                            document.getElementById("txtStartDate").disabled = false


                        }
                        else {
                            document.getElementById("lblNextVisitDate").style.display = "none";
                            document.getElementById("lblNextVsDate").style.display = "none";
                            document.getElementById("lblNextVisitPlace").style.display = "none";
                            document.getElementById("ASPxNextVisit").style.display = "none";

                        }
                    }
                }
            }
        }


        function FillValues(id) {

            noofproduct = id;
        }
        function calldispose(Obj, val) {
            var str = "../frmSalesVisitOutCome.aspx?call=" + val + "&obj=" + Obj
            frmOpenNewWindow1(str, 400, 900)


        }
        function funChangeNext(obj) {
            var o = document.getElementById("lblNextVisitDate")
            if (obj.id == 'rdrCall') {
                o.innerText = "Next Call Date:"
                document.getElementById("tdnextvisit").style.display = 'none';
                document.getElementById("tdnextvisit1").style.display = 'none';
                document.getElementById("TdNextvisitplace").style.display = 'none';
                document.getElementById("TdnBranch").style.display = 'none';

            }
            else {

                o.innerText = "Next Visit Date:"
                $("#rdNClient").prop("checked", true);
                BranchOrClientN('C');
                document.getElementById("tdnextvisit").style.display = 'inline';
                document.getElementById("tdnextvisit1").style.display = 'inline';
                document.getElementById("TdNextvisitplace").style.display = 'inline';


                //  $('#rdNClient').checked()
                // document.getElementById("TdnBranch").style.display = 'inline';
            }
        }
        function BranchOrClient(obj) {

            if (obj == 'C') {
                $('#drpVisitPlace').show();
                $('#TdVisitplace').show();
                $('#TdBranch').hide();
            }
            else if (obj == 'B') {
                $('#drpVisitPlace').hide();
                $('#TdVisitplace').hide();
                $('#TdBranch').show();
            }
        }
        function BranchOrClientN(obj) {
            if (obj == 'C') {
                $('#drpNextVisitPlace').show();
                $('#TdNextvisitplace').show();
                $('#TdnBranch').hide();
                //document.getElementById("drpNextVisitPlace").style.display = 'inline';
                //document.getElementById("TdNextvisitplace").style.display = 'inline';
                //document.getElementById("TdnBranch").style.display = 'none';
            }

            else if (obj == 'B') {
                $('#drpNextVisitPlace').hide();
                $('#TdNextvisitplace').hide();
                $('#TdnBranch').show();
                //document.getElementById("drpNextVisitPlace").style.display = 'none';
                //document.getElementById("TdNextvisitplace").style.display = 'none';
                //document.getElementById("TdnBranch").style.display = 'inline';
            }
        }
        function chkOnSaveClick123() {

            var drp = document.getElementById("TxtOut");
            var st = drp.value.split("!");
            if ((st[1] == 4) || (st[1] == 12)) {
                //var sel = document.getElementById('txtProductCount');
                if (noofproduct == 0) {
                    jAlert("For confirm sale choose atleast one product");
                    return false;
                }
            }
            if ((st[1] == 1) || (st[1] == 2) || (st[1] == 3) || (st[1] == 4) || (st[1] == 8)) {
                var sel = document.getElementById("drpVisitPlace");
                var sel1 = document.getElementById("TdVisitplace")
                ;
                if (sel1.style.display == 'inline') {

                    if (sel.value == 0) //|| (sel.options.length == 0)
                    {
                        jAlert("Please select the visit address");
                        return false;
                    }
                }
            }

            

            if ((st[1] == 1) || (st[1] == 2) || (st[1] == 3) || (st[1] == 4) || (st[1] == 8) || (st[1] == 11) || (st[1] == 12)) {
                var sel = document.getElementById("drpNextVisitPlace");
                var sel1 = document.getElementById("txtNBranch");
                var obrdr = document.getElementById("rdrVisit");
                var objClient = document.getElementById("rdNClient");
                var objBranch = document.getElementById("rdNBranch");
                if (obrdr.checked == true) {
                    if (objClient.checked == true) {
                        if (sel.value == 0) //|| (sel.options.length == 0)
                        {
                            jAlert("Please select the next visit address");
                            return false;
                        }
                    }
                    else if (objBranch.checked == true) {

                        if (sel1.value == '') //|| (sel.options.length == 0)
                        {
                            jAlert("Please select the next visit address");
                            return false;
                        }
                    }
                }
            }

           

            return true;
        }
        function All_CheckedChanged() {
            grid.PerformCallback()
        }
        function Specific_CheckedChanged() {
            grid.PerformCallback()
        }

        function callback() {
        }
        function callDhtmlFormsParent(val, val1) {
            if (val == "ADD") {
                OnMoreInfoClick("frmOfferedProduct_New.aspx", "ADD PRODUCT", "950px", "500px", "Y");
            }
            else if (val == "UPDATE") {
                var url = "../UpdateOfferedProduct.aspx" + "?" + val1;
                document.location.href = url;
                //OnMoreInfoClick(url, "UPDATE PRODUCT", "950px", "500px", "Y");
            }
            else if (val == "PHONE/ADD") {
                var url = "../Master/Contact_Correspondence.aspx" + "?" + val1;
                document.location.href = url;
                //OnMoreInfoClick(url, "Modify Phone/Address", "950px", "500px", "Y");
            }
            else if (val == "LEAD") {
                var url = "../Master/Contact_general.aspx" + "?id=" + <%= Convert.ToString(Session["cntId"]) %> +"";
                    document.location.href = url;
                //OnMoreInfoClick(url, "Modify Lead Details", "950px", "500px", "Y");
                }
                else if (val == "Expences") {
                    var url = "sales_conveyence.aspx";
                    document.location.href = url;
                    //OnMoreInfoClick(url, "Expences", "950px", "500px", "N");
                }
                else if (val == "HISTORY") {
                    var url = "ShowHistory_Phonecall.aspx" + "?" + "id1=" + val1;
                    OnMoreInfoClick(url, "HISTORY", "950px", "500px", "N");
                }
                else if (val == "HISTORY1") {
                    var url = "ShowHistory_Phonecall.aspx";
                    OnMoreInfoClick(url, "HISTORY", "950px", "500px", "N");
                }

}
function OnMoreInfoClick(KeyValue, Id, SalesVisitId) {
    //document.location.href = "CRMSalesVisitWithIFrame.aspx?id=" + KeyValue + "&id1=" + Id + "&id2=" + SalesVisitId;
    document.location.href = "CRMSalesVisitWithIFrame.aspx?id=" + KeyValue + "&id1=" + Id + "&id2=" + SalesVisitId;
}
function Budget_open() {

    var Cid = '<%=Convert.ToString(Request.QueryString["Cid"])%>';
     var SalesId = '<%=Convert.ToString(Request.QueryString["TransSale"])%>';
     var TypeId = '<%=Int32.Parse(Request.QueryString["type"])%>';

     var url = '/OMS/Management/Activities/SalesmanBudget.aspx?tid=3' + '&Cid=' + Cid + "&type=" + TypeId + "&SalId=" + SalesId;
     popupbudget.SetContentUrl(url);
     popupbudget.Show();

     return false;
     //return true;
 }
 function BudgetAfterHide(s, e) {
     popupbudget.Hide();
 }
    </script>
    <style type="text/css">
        .chosen-container.chosen-container-multi,
        .chosen-container.chosen-container-single {
            width:100% !important;
        }
        .chosen-choices {
            width:100% !important;
        }
        #lstCallDisposion {
            width:200px;
        }
        .hide {
            display:none;
        }
                #MandatoryNotes {
            position:absolute;right:-20px;top:7px
        }
        .btn.active {
            background-color: #057979 !important;
            color:#fff !important;
        }
        #lstCountry_chosen .chosen-results 
        {
            height:80px
        }
        #ddl_activitystatus>tbody>tr>td {
            padding-right:20px;
        }
#lstState_chosen .chosen-results,
#lstCity_chosen .chosen-results {
    height:80px
}
    </style>

    <script type="text/javascript">

        function ClientSaveClick() {
            //  alert(document.getElementById('lstState').value);
            document.getElementById('txtCountry_hidden').value = document.getElementById('lstCountry').value;
            document.getElementById('txtState_hidden').value = document.getElementById('lstState').value;
            document.getElementById('txtCity_hidden').value = document.getElementById('lstCity').value;

        }
        $(document).ready(function () {

            //  DisableChoosen();
            //$('#lstItems').chosen();

            //for choosen drop down kaushik  17-11-2016
            BindBranchDtl();
            BindCallDisposion();
            ListBind();

         //   DisableChoosen();
            $("#lstBranch").chosen().change(function () {
                var branchval = $(this).val();
                $('#<%=hdnBranchVal.ClientID %>').val(branchval);

            });



            $("#lstNBranch").chosen().change(function () {
                var nbranchval = $(this).val();
                $('#<%=hdnNBranchVal.ClientID %>').val(nbranchval);

            });

            //$("#lstCallDisposion").chosen().ready(function () {
            //    DisableChoosen();
            //});
            $("#lstCallDisposion").chosen().change(function () {

                var callDisposionIdDtl = $(this).val();
                if (callDisposionIdDtl != null) {
                    var callDisposionIdsplit = callDisposionIdDtl.toString().split('!');
                    var callDisposionTimedtl = callDisposionIdsplit[0, 0];

                    var nowdate = callDisposionTimedtl.toString().split(',')[0];

                    var nowtime = callDisposionTimedtl.toString().split(',')[1];

                    var cdate = callDisposionTimedtl.toString().split(',')[2];

                    var ctime = callDisposionTimedtl.toString().split(',')[3];

                    var callDisposionId = callDisposionIdsplit[0, 1];
                    var DisposionIdCat = callDisposionId.toString().split('~');
                    var DisposionId = DisposionIdCat[0, 0];
                    var DisposionCat = DisposionIdCat[0, 1];
                    $('#TxtOut').val(DisposionId + '|' + DisposionCat);
                    $("#btnSavePhoneCallDetails").prop("disabled", false);
                    $('#btnSavePhoneCallDetails').removeClass("aspNetDisabled");
                    //$('#btnSavePhoneCallDetails').disabled = false;
                    Changedata('txtNextVisitDate1', 'txtNextVisitTime', nowdate, nowtime, cdate, ctime);
                    //document.getElementById("TxtOut").va


                    //if (DisposionCat == 1 || DisposionCat == 2) {
                    //    ///   alert(1);
                    //    var date = new Date();
                    //    var date2 = addDays(date, 1);
                    //    tnextdate.SetDate(date2);


                    //}
                    //else {
                    //    var date = new Date();
                    //    tnextdate.SetDate(date);
                    //}
                    // txtCallDispose:calldisposition text,  TxtOut:calldisposition id  , txtNextVisitDate1,txtNextVisitTime,


                    if (DisposionCat == 4 && $("#idrightsquotation").val() == 'True') {

                        $("#Button2").attr('style', 'display:inline-block');
                      

                    }
                    else {

                        $("#Button2").attr('style', 'display:none');
                    


                    }
                    if (DisposionCat == 4 && $("#idrightsSaleorder").val() == 'True') {
                        $("#Button3").attr('style', 'display:inline-block');
                      
                    }
                    else {

                        $("#Button3").attr('style', 'display:none');
                    


                    }

                    if (DisposionId == 32) {

                        $("#Button2").attr('style', 'display:none');



                    }

                    if (DisposionId ==34) {

                        $("#Button3").attr('style', 'display:none');
                        $("#Button2").attr('style', 'display:inline-block');

                    }


                    $('#<%=hdnCallDisposion.ClientID %>').val(callDisposionId);
                }
                //var productText = $("#ListBoxProduct").find("option:selected").text();

            })

        });

        function addDays(date, amount) {
            var tzOff = date.getTimezoneOffset() * 60 * 1000,
                t = date.getTime(),
                d = new Date(),
                tzOff2;

            t += (1000 * 60 * 60 * 24) * amount;
            d.setTime(t);

            tzOff2 = d.getTimezoneOffset() * 60 * 1000;
            if (tzOff != tzOff2) {
                var diff = tzOff2 - tzOff;
                t += diff;
                d.setTime(t);
            }

            return d;
        }

        $(function () {
            $("#Button2").attr('style', 'display:none');
            $("#Button3").attr('style', 'display:none');
          
        });


        function DisableChoosen() {
            //  alert('kkkkk');
            debugger;
            // $('#lstCallDisposion').trigger("chosen:updated");
            //alert($('#hdndisableChoosen').val());
          
            if ($('#hdndisableChoosen').val() == 'Yes') {
               // alert('kkk');
                $('#lstCallDisposion').prop('disabled', true).trigger("chosen:updated");
                $('#lstCountry').prop('disabled', true).trigger("chosen:updated");
                $('#lstState').prop('disabled', true).trigger("chosen:updated");
                $('#lstCity').prop('disabled', true).trigger("chosen:updated");
                $('#hdndisableChoosen').val('No');
            }
            else {
         $('#lstCallDisposion').prop('disabled', false).trigger("chosen:updated");
            }
          
        
            //$('#lstCountry').prop('disabled', true).trigger("chosen:updated");
            //$('#lstState').prop('disabled', true).trigger("chosen:updated");
            //$('#lstCity').prop('disabled', true).trigger("chosen:updated");
        }

        function EnableChoosen() {
       $('#lstCallDisposion').prop('disabled', false).trigger("chosen:updated");
            $('#lstCountry').prop('disabled', false).trigger("chosen:updated");
            $('#lstState').prop('disabled', false).trigger("chosen:updated");
            $('#lstCity').prop('disabled', false).trigger("chosen:updated");
        }
        function BindCallDisposion() {
            debugger;
            var lBox = $('select[id$=lstCallDisposion]');
            var listItems = [];
            lBox.empty();
            $.ajax({
                type: "POST",
                url: 'CRMSalesVisitWithIFrame.aspx/GetCallDispositionList',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    if (list.length > 0) {
                        DisableChoosen

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

                        ListBind();
                        $('#lstCallDisposion').trigger("chosen:updated");
                   $('#lstCallDisposion').prop('disabled', false).trigger("chosen:updated");
                       DisableChoosen();
                    }
                    else {
                        lBox.empty();
                        $('#lstCallDisposion').trigger("chosen:updated");
                        $('#lstCallDisposion').prop('disabled', true).trigger("chosen:updated");

                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    // alert(textStatus);
                }
            });


        }
        $(document).ready(function () {
            ListBind();

            var cntry = document.getElementById('txtCountry_hidden').value;
            document.getElementById('txtCountry_hidden').value = "";
            setCountry(cntry);
        });
        function ListBind() {
            $('#lstCallDisposion').chosen();
            $('#lstCallDisposion').fadeIn();
            $('#lstBranch').chosen();
            $('#lstBranch').fadeIn();
            $('#lstNBranch').chosen();
            $('#lstNBranch').fadeIn();

            var config = {
                '.chsn': {},
                '.chsn-deselect': { allow_single_deselect: true },
                '.chsn-no-single': { disable_search_threshold: 10 },
                '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
                '.chsn-width': { width: "100%" }
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }

        }

        //Populate  Branch detail by ajax

        function BindBranchDtl() {

            var lBox = $('select[id$=lstBranch]');
            var nlBox = $('select[id$=lstNBranch]');
            var listItems = [];
            lBox.empty();
            nlBox.empty();
            $.ajax({
                type: "POST",
                url: 'CRMSalesVisitWithIFrame.aspx/GetBranchList',
                contentType: "application/json; charset=utf-8",
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
                        $(nlBox).append(listItems.join(''));
                        ListBind();
                        $('#lstBranch').trigger("chosen:updated");
                        $('#lstBranch').prop('disabled', false).trigger("chosen:updated");
                        $('#lstNBranch').trigger("chosen:updated");
                        $('#lstNBranch').prop('disabled', false).trigger("chosen:updated");

                    }
                    else {
                        lBox.empty();
                        $('#lstBranch').trigger("chosen:updated");
                        $('#lstBranch').prop('disabled', true).trigger("chosen:updated");
                        $('#lstNBranch').trigger("chosen:updated");
                        $('#lstNBranch').prop('disabled', true).trigger("chosen:updated");

                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //  alert(textStatus);
                }
            });


        }



    </script>
    <style>
            .strong {
                font-weight:bold;
            }
            
            #TdNextvisitplace {
                display:block;
                margin:0;
            }
            #MandatoryVisitDate {
                position: absolute;
                right: -19px;
                top: 5px;
            }
               #lstState_chosen, #lstCountry_chosen, #lstCity_chosen,#lstPin_chosen,#lstBranchHead_chosen {
            margin-bottom:5px;
        }
 
        .chosen-choices {
            width:100% !important;
        }
                .chosen-container.chosen-container-single {
            width:100% !important;
        }
        .chosen-choices {
            width:100% !important;
        }
         #lstCountry_chosen,#lstState_chosen,#lstCity_chosen,#lstArea_chosen,#lstPin_chosen,#lstBranchHead_chosen{
            width:100% !important;
        }
        </style>

    <script language="javascript" type="text/javascript">


        function onCountryChange() {
            var CountryId = "";
            if (document.getElementById('lstCountry').value) {
                CountryId = document.getElementById('lstCountry').value;
            } else {
                return;
            }
            var lState = $('select[id$=lstState]');
            var lCity = $('select[id$=lstCity]');

            lState.empty();
            lCity.empty();

            $('#lstCity').trigger("chosen:updated");

            $.ajax({
                type: "POST",
                url: "CRMSalesVisitWithIFrame.aspx/GetStates",
                data: JSON.stringify({ CountryCode: CountryId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
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

                        $(lState).append(listItems.join(''));

                        $('#lstState').fadeIn();
                        $('#lstState').trigger("chosen:updated");
                        if (document.getElementById('txtState_hidden').value) {
                            var stateVal = document.getElementById('txtState_hidden').value;
                            document.getElementById('txtState_hidden').value = "";
                            setState(stateVal);
                        }
                    }
                    else {
                        $('#lstState').fadeIn();
                        $('#lstState').trigger("chosen:updated");
                    }
                }
            });
        }

        function onStateChange() {
            var StateId = "";
            if (document.getElementById('lstState').value) {
                StateId = document.getElementById('lstState').value;
            }
            else {
                return;
            }
            var lCity = $('select[id$=lstCity]');


            lCity.empty();


            $.ajax({
                type: "POST",
                url: "CRMSalesVisitWithIFrame.aspx/GetCities",
                data: JSON.stringify({ StateCode: StateId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
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

                        $(lCity).append(listItems.join(''));

                        $('#lstCity').fadeIn();
                        $('#lstCity').trigger("chosen:updated");
                        if (document.getElementById('txtCity_hidden').value) {
                            var cityVal = document.getElementById('txtCity_hidden').value;
                            document.getElementById('txtCity_hidden').value = "";
                            setCity(cityVal);
                        }
                    }
                    else {
                        $('#lstCity').fadeIn();
                        $('#lstCity').trigger("chosen:updated");
                    }
                }
            });
        }

        function setCountry(obj) {
            if (obj) {
                var lstCntry = document.getElementById("lstCountry");

                for (var i = 0; i < lstCntry.options.length; i++) {
                    if (lstCntry.options[i].value == obj) {
                        lstCntry.options[i].selected = true;
                    }
                }
                $('#lstCountry').trigger("chosen:updated");
                onCountryChange();
            }
        }
        function setState(obj) {
            if (obj) {
                var lstStae = document.getElementById("lstState");

                for (var i = 0; i < lstStae.options.length; i++) {
                    if (lstStae.options[i].value == obj) {
                        lstStae.options[i].selected = true;
                    }
                }
                $('#lstState').trigger("chosen:updated");
                onStateChange();
            }
        }
        function setCity(obj) {
            if (obj) {
                var lstCity = document.getElementById("lstCity");

                for (var i = 0; i < lstCity.options.length; i++) {
                    if (lstCity.options[i].value == obj) {
                        lstCity.options[i].selected = true;
                    }
                }
                $('#lstCity').trigger("chosen:updated");

            }
        }
    </script>
    <style>
       .horizontal-images.content li:first-child {
            padding-left:0;
       }
     .totalWrap {
        margin-top: 12px;
        background: #c3d8d8;
        border: 1px solid #b6d0d0;
        padding: 9px 0;
    }
    table.dxeDisabled_PlasticBlue {
         border: 1px solid #b8b8b8;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Sales Visit</h3>
            <div class="crossBtn"><a href="crm_sales.aspx"><i class="fa fa-times"></i></a></div>
        </div>

    </div>
    <div class="form_main">
          <%--   <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
           <ClientSideEvents ControlsInitialized="DisableChoosen" />
        </dxe:ASPxGlobalEvents>--%>
        <table class="TableMain100">


            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                        <tr id="activityRow" runat="server" visible="false">
                            <td>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 70px">
                                            <asp:Label ID="LBLActivity" runat="server" Text="Activities Of :" CssClass="myhypertext"
                                                Width="70px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDLActivity" runat="server" Width="250px" OnSelectedIndexChanged="DDLActivity_SelectedIndexChanged"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="grdtbl" runat="server" visible="false">
                            <td style="width: 100%">
                                <table style="width: 100%">
                                    <tr>
                                        <td colspan="2" style="text-align: left">
                                            <table class="TableMain100" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width: 30px;">
                                                        <asp:RadioButton ID="Lrd" runat="server" GroupName="a" Checked="True" />
                                                    </td>
                                                    <td style="width: 150px;">
                                                        <asp:Label ID="Label4" runat="server" Text="From Lead Data" Font-Size="X-Small" ForeColor="Blue"></asp:Label>
                                                    </td>
                                                    <td style="width: 30px;">
                                                        <asp:RadioButton ID="Erd" runat="server" GroupName="a" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label5" runat="server" Text="From Existing Customer Data" Font-Size="X-Small"
                                                            ForeColor="Blue"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%">
                                            <dxe:ASPxGridView ID="AspxActivity" runat="server" ClientInstanceName="grid" KeyFieldName="ActId"
                                                AutoGenerateColumns="False" Width="100%" OnCustomCallback="AspxActivity_CustomCallback">
                                                <Columns>
                                                    <dxe:GridViewDataTextColumn FieldName="Id" VisibleIndex="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Name" ReadOnly="True" VisibleIndex="1">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="ActId" Visible="False" VisibleIndex="2">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="SalesVisitId" Visible="False" VisibleIndex="2">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="AssignBy" Visible="False" VisibleIndex="2">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="NextVisitDate" VisibleIndex="2">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Address1" ReadOnly="True" VisibleIndex="3">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="LastOutcome" VisibleIndex="4">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Details" VisibleIndex="5" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <DataItemTemplate>
                                                            <a href="#" onclick="OnMoreInfoClick('<%# Container.KeyValue %>','<%#Eval("Id") %>','<%#Eval("SalesVisitId") %>')" title="show">
                                                                <img src="/OMS/images/show.png" />
                                                            </a>
                                                            <%--<a href="#" onclick="callDhtmlFormsParent('HISTORY','<%#Eval("Id") %>')" title="history">
                                                             <img src="/assests/images/history.png" />
                                                         </a>--%>
                                                        </DataItemTemplate>
                                                        <HeaderTemplate>Actions</HeaderTemplate>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%-- <dxe:GridViewDataTextColumn Caption="Details" VisibleIndex="5">
                                                        <DataItemTemplate>
                                                            <a href="#" onclick="OnMoreInfoClick('<%# Container.KeyValue %>','<%#Eval("Id") %>','<%#Eval("SalesVisitId") %>')">Show</a>
                                                        </DataItemTemplate>
                                                        <EditFormSettings Visible="False" />
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="History" VisibleIndex="6">
                                                        <DataItemTemplate>
                                                            <a href="#" onclick="callDhtmlFormsParent('HISTORY','<%#Eval("Id") %>')">History</a>
                                                        </DataItemTemplate>
                                                        <EditFormSettings Visible="False" />
                                                    </dxe:GridViewDataTextColumn>--%>
                                                </Columns>
                                                <Styles>
                                                    <LoadingPanel ImageSpacing="10px">
                                                    </LoadingPanel>
                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                    </Header>
                                                </Styles>
                                                <SettingsPager ShowSeparators="True">
                                                    <FirstPageButton Visible="True">
                                                    </FirstPageButton>
                                                    <LastPageButton Visible="True">
                                                    </LastPageButton>
                                                </SettingsPager>
                                            </dxe:ASPxGridView>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="showdetailstbl" runat="server" visible="true">
                            <td colspan="2">
                                <asp:Panel ID="PnlShowDetails" runat="server" Width="100%" Visible="true" CssClass="TableMain100">




                                    <table class="TableMain100" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 100%;">

                                                <div id="content-6" class="wrapHolder reverse insidePage content horizontal-images" style="width: 100%; margin-right: 0px;">
                                                    <ul>
                                                        <li>
                                                            <div class="lblHolder">
                                                                <table>
                                                                    <tbody>
                                                                        <tr>
                                                                            <td>Assigned By </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="txtAlloatedBy" runat="server" CssClass="EcoheadCon" Width="122px"></asp:Label>

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
                                                                            <td>Assigned On </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="txtDateOfAllottment" runat="server" CssClass="EcoheadCon" Width="130px"></asp:Label>
                                                                                <asp:Label ID="txtTotalNumberofCalls" runat="server" Visible="False" CssClass="EcoheadCon"
                                                                                    Width="130px"></asp:Label>

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
                                                                            <td>Priority </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="txtPriority" runat="server" CssClass="EcoheadCon"></asp:Label>

                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </div>

                                                        </li>

                                                        <li>
                                                            <div class="lblHolder" runat="server" id="lilastvisitstatus">
                                                                <table>
                                                                    <tbody>
                                                                        <tr>
                                                                            <td>Last Visit </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblLastVisit" runat="server" CssClass="EcoheadCon"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </div>

                                                        </li>



                                                        <li >
                                                            <div   class="lblHolder" id="lilLastOutcome" runat="server">
                                                                <table>
                                                                    <tbody>
                                                                        <tr>
                                                                            <td>Sales Visit Status </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblLastOutcome" runat="server" CssClass="EcoheadCon"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </div>

                                                        </li>

                                                        <li runat="server" id="li1">
                                                            <div class="lblHolder">
                                                                <table>
                                                                    <tbody>
                                                                        <tr>
                                                                            <td>Customer </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblcustomer1" runat="server"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </div>

                                                        </li>


                                                        <li runat="server" id="liproductclass">
                                                            <div class="lblHolder">
                                                                <table>
                                                                    <tbody>
                                                                        <tr>
                                                                            <td>Product Class </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblproductclass" runat="server"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </div>

                                                        </li>

                                                        <li>
                                                            <div class="lblHolder" runat="server" id="idNoteRemarks">
                                                                <table>
                                                                    <tbody>
                                                                        <tr>
                                                                            <td>Last Note/Remarks </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblShortNote" runat="server" CssClass="EcoheadCon"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </div>

                                                        </li>
                                                          <li>

                                                            <span id="spanBudget" >

                          <a href="javascript:void(0);" onclick="Budget_open()" title="Budget"  class="btn btn-primary">
                               Target Sale Of Product</a>
                        
                    </span>
                                                        </li>
                                                    </ul>

                                                </div>




                                                <table class="maintable10" style="width: 100%; display: none;">
                                                    <tr>
                                                        <td class="strong">
                                                            <asp:Label ID="Label6" runat="server" CssClass="mylabel">Assigned By :</asp:Label>
                                                        </td>
                                                        <td></td>
                                                        <td class="strong">
                                                            <asp:Label ID="Label8" runat="server" CssClass="mylabel">Assigned On :</asp:Label>
                                                        </td>
                                                        <td></td>
                                                        <td class="strong">
                                                            <asp:Label ID="Label10" runat="server" CssClass="mylabel">Priority :</asp:Label>
                                                        </td>
                                                        <td></td>
                                                        <td class="strong">
                                                            <asp:Label ID="Label12" runat="server" CssClass="mylabel">Start By :</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="txtSeheduleStart" runat="server" CssClass="EcoheadCon"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="strong">
                                                            <asp:Label ID="Label7" runat="server" CssClass="mylabel">End By :</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="txtSeheduleEnd" runat="server" CssClass="EcoheadCon"></asp:Label>
                                                        </td>
                                                        <td class="strong">
                                                            <asp:Label ID="Label9" runat="server" CssClass="mylabel">Started On:</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="txtAcutalStart" runat="server" CssClass="EcoheadCon" Width="130px"></asp:Label>
                                                        </td>

                                                        <td class="strong">
                                                            <asp:Label ID="Label1" runat="server" CssClass="mylabel">Next Visit :</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblNextVisit" runat="server" CssClass="EcoheadCon"></asp:Label>
                                                        </td>
                                                        <td class="strong">
                                                            <asp:Label ID="Label13" runat="server" CssClass="mylabel">Last Visit :</asp:Label>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="strong">
                                                            <asp:Label ID="Label14" runat="server" CssClass="mylabel">Sales Visit Status  :</asp:Label></td>
                                                        <td></td>
                                                        <td class="strong">
                                                            <asp:Label ID="Label11" runat="server" CssClass="mylabel">Instruction :</asp:Label>
                                                        </td>
                                                        <td></td>


                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="10">

                                                <table style="margin-top: 2px;">
                                                    <tr>
                                                        <td class="gridcellleft">
                                                            <asp:Button ID="btnUpdateVisit" Text="Update Visit" CssClass="btnUpdate btn btn-primary" runat="server"
                                                                OnClick="btnUpdateVisit_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnPhoneFollowUP" Text="Sales Visit Follow Up" CssClass="btnUpdate btn btn-primary" runat="server"
                                                                OnClick="btnPhoneFollowUP_Click" />
                                                        </td>
                                                        <td>
                                                            <input type="button" value="Modify Phone/Address" id="btn_UpdateAddress" class="btnUpdate btn btn-primary" style="visibility: hidden"
                                                                onclick="callDhtmlFormsParent('PHONE/ADD', 'type=modify&requesttype=lead&formtype=lead')" />
                                                        </td>
                                                        <td>
                                                            <input type="button" value="Modify Contact Details" id="btn_UpdateDetails" class="btnUpdate btn btn-primary" style="visibility: hidden"
                                                                onclick="callDhtmlFormsParent('LEAD', 'type=modify&requesttype=lead&formtype=lead')" />
                                                        </td>
                                                        <%--<td>
                                                                <input type="button" id="Button1" name="btnHistory" value="History" class="btnUpdate"
                                                                    onclick="frmOpenNewWindow1('../management/ShowHistory_Phonecall.aspx',300,800);"
                                                                    style="height: 21px" />
                                                            </td>--%>
                                                        <%--   <td>
                                                                <input type="button" value="Modify Phone/Address" id="btn_UpdateAddress" class="btnUpdate"
                                                                    style="width: 139px; height: 21px;" onclick="frmOpenNewWindow1('../management/Contact_Correspondence.aspx?type=modify&requesttype=lead&formtype=lead',550,800)" />
                                                            </td>
                                                            <td>
                                                                <input type="button" value="Modify Contact Details" id="btn_UpdateDetails" class="btnUpdate"
                                                                    style="width: 139px; height: 21px;" onclick="frmOpenNewWindow1('../management/Lead_general.aspx?type=modify&requesttype=lead&formtype=lead',400,900)" />
                                                            </td>--%>
                                                        <td>
                                                            <input type="button" id="Button1" name="btnHistory" value="History" class="btnUpdate btn btn-primary" style="visibility: hidden"
                                                                onclick="callDhtmlFormsParent('HISTORY1', 'Optional')" />
                                                            <%-- <input type="button" id="Button1" name="btnHistory" value="History" class="btnUpdate"
                                                                    onclick="OnMoreInfoClick1('<%#Eval("Id") %>')"
                                                                    style="height: 21px" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                        <tr id="pnldatatbl" runat="server" visible="true">
                            <td style="width: 100%">
                                <asp:Panel ID="pnlData" Visible="true"  runat="server" Width="100%">
                                    <div class="clearfix" style="background: #c1e2cf; padding-bottom: 8px; border: 1px solid #9dc5ae;">
                                        <div class="col-md-3">
                                            <label>
                                                <asp:Label ID="Label2" runat="server" CssClass="mylabel1">Visit Outcome :</asp:Label><span style="color: red">*</span></label>
                                            <div>
                                                <asp:ListBox ID="lstCallDisposion" runat="server" TabIndex="4" SelectionMode="Single" CssClass="mb0 chsn hide" data-placeholder="Select ..."></asp:ListBox>
                                                <asp:HiddenField ID="hdnCallDisposion" runat="server" />
                                                <asp:HiddenField ID="hdnCallDisposionText" runat="server" />


                                                <asp:TextBox ID="TxtOut" runat="server" BackColor="Transparent" BorderColor="Transparent" CssClass="hide"
                                                    BorderStyle="None" ForeColor="#DDECFE" Width="100%"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-4">

                                            <div class="clearfix">
                                                <label class="pull-left" style="margin-top: 8px">
                                                    <asp:Label ID="Label16" runat="server" Text="Meeting at:" CssClass="mylabel1"></asp:Label></label>
                                                <table style="width: 200px; margin: 8px 0 0 10px;" class="pull-left">
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="rdClient" runat="server" GroupName="k" Text="" Checked="True" /></td>
                                                        <td>
                                                            <asp:Label ID="Label19" runat="server" Text="Client Place" CssClass="ColorOption"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdBranch" runat="server" GroupName="k" Text="" CssClass="ColorOption" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label20" runat="server" Text="Branch Name" CssClass="ColorOption"></asp:Label>
                                                        </td>
                                                        <td class="hide">
                                                            <asp:Label ID="lblVisitPlace" runat="server" CssClass="mylabel1" Text="Visit Place :"></asp:Label></td>

                                                    </tr>

                                                </table>
                                                <div>
                                                    <div id="TdVisitplace">
                                                        <asp:DropDownList ID="drpVisitPlace" runat="server" Width="100%">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div id="TdBranch" style="display: none">
                                                        <%--<asp:TextBox ID="txtbranch" runat="server" Width="100%"></asp:TextBox><asp:HiddenField
                                                            ID="txtbranch_hidden" runat="server"></asp:HiddenField>--%>
                                                        <asp:ListBox ID="lstBranch" runat="server" TabIndex="4" SelectionMode="Single" CssClass="mb0 chsn hide" data-placeholder="Select ..."></asp:ListBox>
                                                        <asp:HiddenField ID="hdnBranchVal" runat="server" />
                                                        <asp:HiddenField ID="hdnBranchText" runat="server" />
                                                    </div>
                                                    <asp:HiddenField ID="hdcleint" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <label>&nbsp</label>

                                        </div>
                                        <div class="col-md-3">
                                            <%-- <label><asp:Label ID="lblVisitDateTime" runat="server" CssClass="mylabel1">Visit Date Time:</asp:Label></label>--%>
                                            <div>
                                            </div>
                                        </div>
                                        <div class="clear"></div>

                                        <div class="col-md-3">
                                            <label>
                                                <asp:Label ID="Label15" runat="server" CssClass="mylabel1">Next ActivityType:</asp:Label></label>
                                            <div>
                                                <asp:RadioButton ID="rdrCall" runat="server" GroupName="rdr" Text="Phone FollowUp"
                                                    Width="125px" />
                                                <asp:RadioButton ID="rdrVisit" runat="server" GroupName="rdr" Text="Visit" Checked="True"
                                                    Width="50px" />
                                                <a class="decoration" href="../sales_conveyence.aspx" onclick="window.open(this.href,'popupwindow','left=120,top=170,height=450,width=900,scrollbars=no,toolbar=no,location=center,menubar=no'); return false; "
                                                    style="font-size: 8pt; color: purple; visibility: hidden">Expenses </a>
                                                <%--  <a class="decoration" href="sales_conveyence.aspx" onclick="window.open(this.href,'popupwindow','left=120,top=170,height=450,width=900,scrollbars=no,toolbar=no,location=center,menubar=no'); return false; "
                                                            style="font-size: 8pt; color: purple"> Expenses </a>--%>
                                                <%--<a href="javascript:void(0);" onclick="callDhtmlFormsParent('Expences','Optional');" >
                                                    <span style="color: #000099; text-decoration: underline">Expenses</span> </a>--%>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <label id="lblNextVsDate">
                                                <asp:Label ID="lblNextVisitDate" runat="server" CssClass="mylabel1 pull-left">Next Visit Date:</asp:Label>
                                                <span style="color: red" class="">*</span>
                                            </label>
                                            <div class="relative">
                                                <dxe:ASPxDateEdit ID="ASPxNextVisit" runat="server" EditFormat="Custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                                                    Width="100%" UseMaskBehavior="True" ClientInstanceName="tnextdate" ClientSideEvents-DateChanged="DateCompletedChangedNextCall">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>
                                                <span id="MandatoryVisitDate" style="display: none; position: absolute; right: -10px;">
                                                    <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor2111_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                            </div>
                                        </div>

                                        <div class="col-md-4" id="tdnextvisit">

                                            <div id="tdnextvisit1" class="clearfix">
                                                <label class="pull-left" style="margin-top: 8px;">
                                                    <asp:Label ID="lblNextVisitPlace" Text="Next Visit Place : " runat="server"
                                                        CssClass="mylabel1"></asp:Label></label>
                                                <table style="width: 200px; margin: 8px 0 0 10px;" class="pull-left">
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="rdNClient" runat="server" GroupName="kN" Text="" Checked="True"
                                                                CssClass="ColorOption" /></td>
                                                        <td>
                                                            <asp:Label ID="Label18" runat="server" Text="Client Place" CssClass="ColorOption"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdNBranch" runat="server" GroupName="kN" Text="" /></td>
                                                        <td>
                                                            <asp:Label ID="Label17" runat="server" Text="Branch Name" CssClass="ColorOption"></asp:Label></td>

                                                    </tr>

                                                </table>
                                                <div>

                                                    <label id="TdNextvisitplace" style="display: none">
                                                        <asp:DropDownList ID="drpNextVisitPlace" runat="server" Width="100%">
                                                        </asp:DropDownList></label>
                                                    <div id="TdnBranch" style="display: none">
                                                        <asp:ListBox ID="lstNBranch" runat="server" TabIndex="4" SelectionMode="Single" CssClass="mb0 chsn hide" data-placeholder="Select ..."></asp:ListBox>
                                                        <asp:HiddenField ID="hdnNBranchVal" runat="server" />
                                                        <asp:HiddenField ID="hdnNBranchText" runat="server" />
                                                        <%--<asp:TextBox ID="txtNBranch" runat="server" Width="100%"></asp:TextBox><asp:HiddenField
                                                            ID="txtNBranch_hidden" runat="server"></asp:HiddenField>--%>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                        </div>
                                        <div class="clear"></div>
                                        <div class="col-md-6">
                                            <label>
                                                <%--<asp:Label ID="Label3" runat="server" CssClass="mylabel1">Note/Remarks :</asp:Label>--%>
                                                <label>Note/Remarks :<span style="color: red">*</span></label>
                                            </label>
                                            <div style="position: relative;">
                                                <%-- <asp:TextBox ID="txtNotes" TextMode="MultiLine" runat="server" Width="100%" Height="47px"></asp:TextBox>--%>
                                                <dxe:ASPxMemo ID="txtNotes" runat="server" Width="100%" Height="30px" MaxLength="1000" ClientInstanceName="txtNotes"></dxe:ASPxMemo>
                                                <span id="MandatoryNotes" style="display: none;">
                                                    <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <label>
                                                <asp:Label ID="Label21" runat="server" CssClass="mylabel1">Sales Visit Status :</asp:Label></label>
                                            <div style="padding-top: 5px;">

                                                <%--Abhisek
                                                <asp:DropDownList ID="ddl_activitystatus" runat="server" Width="100%">--%>
                                                    <%-- <asp:ListItem Value="0" Text="Open" Selected="True"></asp:ListItem>
                                                                     <asp:ListItem Value="1" Text="Document Collection" ></asp:ListItem>
                                                                     <asp:ListItem Value="2" Text="Closed Sales" ></asp:ListItem>
                                                                     <asp:ListItem Value="3" Text="Future Sales" ></asp:ListItem>
                                                                       <asp:ListItem Value="5" Text="Clarification Required" ></asp:ListItem>--%>
                                                <%--Abhisek
                                                    </asp:DropDownList> --%>
                                                <asp:RadioButtonList runat="server" ID="ddl_activitystatus" RepeatDirection="Horizontal">
                                              </asp:RadioButtonList>
                                                
                                              
                                            </div>
                                        </div>
                                    </div>



                                    <%--     CRM NEW ADD --%>

                                    <div class="totalWrap" style="">
                                        <div class="col-md-2">
                                            <span style="margin: 3px; display: inline-block;">
                                                <asp:Label ID="lblVisitDateTime" runat="server">Visit Date Time</asp:Label>
                                            </span>
                                            <div class="relative">
                                                <dxe:ASPxDateEdit ID="ASPxDateEdit" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy hh:mm tt"
                                                    UseMaskBehavior="True" Width="100%">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <label>Customer </label>
                                            <div class="relative">

                                                <asp:TextBox ID="lblcustomer" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-md-2">
                                            <label>Email Id </label>
                                            <div class="relative">

                                                <asp:TextBox ID="txtemailids" runat="server" Width="100%" ></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-md-2">
                                            <label>Person met and Designation</label>
                                            <div>
                                                <asp:TextBox ID="txtpersonmetdesignation" runat="server" Width="100%"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <label>Phone Nos </label>
                                            <div class="relative">
                                                <asp:TextBox ID="txtphonenos" runat="server" Width="100%" MaxLength="50"></asp:TextBox>

                                            </div>
                                        </div>

                                        <div class="col-md-2">
                                            <label>Country </label>
                                            <div class="relative">

                                                <asp:ListBox ID="lstCountry" CssClass="chsn" runat="server" Width="100%" data-placeholder="Select..." onchange="onCountryChange()"></asp:ListBox>
                                                <asp:HiddenField ID="txtCountry_hidden" runat="server" />
                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                        <div class="col-md-2">
                                            <label>State </label>
                                            <div class="relative">

                                                <asp:ListBox ID="lstState" CssClass="chsn hide" runat="server" Width="100%" data-placeholder="Select State.." onchange="onStateChange()"></asp:ListBox>
                                                <asp:HiddenField ID="txtState_hidden" runat="server" />
                                            </div>
                                        </div>

                                        <div class="col-md-2">
                                            <label>City / District </label>
                                            <div class="relative">

                                                <asp:ListBox ID="lstCity" CssClass="chsn hide" runat="server" Width="100%" data-placeholder="Select City.."></asp:ListBox>
                                                <asp:HiddenField ID="txtCity_hidden" runat="server" />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <label>Product Mfg. & Capacity </label>
                                            <div class="relative">
                                                <asp:TextBox ID="txtproductmanufacture" runat="server" Width="100%"></asp:TextBox>

                                            </div>
                                        </div>

                                        <div class="col-md-2">
                                            <label>Raw Materials reqd./month </label>
                                            <div class="relative">
                                                <asp:TextBox ID="txtrawmaterial" runat="server" Width="100%"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <label>Current Sourcing and Pricing </label>
                                            <div class="relative">
                                                <asp:TextBox ID="txtcurrsourceprice" runat="server" Width="100%"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <label>Discussion and future action </label>
                                            <div class="relative">
                                                <asp:TextBox ID="txtdisscussonfuture" runat="server" Width="100%"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                        <div class="col-md-2">
                                            <label>Other Business if any</label>
                                            <div class="relative">
                                                <asp:TextBox ID="txtotherbusiness" runat="server" Width="100%"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <label>Ref. of other Customers</label>
                                            <div class="relative">
                                                <asp:TextBox ID="txtContPhone" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                        </div>
                                        <div class="clear"></div>


                                    </div>












                                    <table width="100%">
                                        
                                        <tr>
                                            <td colspan="8" align="center" style="padding-top: 10px;">
                                               <span class="pull-left"> <asp:CheckBox ID="chkMail" runat="server" />
                                                Send Mail</span>
                                            
                                                <asp:Button ID="btnSavePhoneCallDetails" runat="server" Text="Save" CssClass="btnUpdate btn btn-primary" Enabled="true"
                                                    OnClick="btnSave1_Click" />
                                                <asp:Button ID="btnSetReminder" runat="server" Text="Reminder" CssClass="btnUpdate btn btn-primary hide"
                                                    OnClick="btnSetReminder_Click" />
                                                <asp:Button ID="BtnSCancel" runat="server" Text="Cancel" CssClass="btnUpdate btn btn-danger" OnClick="BtnSCancel_Click" />
                                                <asp:HiddenField runat="server" ID="mailassignfield" />

                                                <%--   <a href="mailto:sudipk.pal@indusnet.co.in">Send mail to  customer</a>--%>
                                                <a href="mailto:javascript:MailFetchAssignto();" id="idmail" runat="server" class="btn btn-primary">Send mail to  customer</a>
                                                <asp:Label ID="lblFieldName" runat="server" Text="Label" Visible="false"></asp:Label>
                                                 <%-- <% if (rightsQuotation.CanAdd)
                                                                            { %>--%>
                                                  <input type="hidden" id="idrightsquotation" runat="server"  />
                                                       <input type="hidden" id="idrightsSaleorder" runat="server" />
                                              <%--  <asp:Button ID="Button2" runat="server" CssClass="btnUpdate btn btn-success"   OnClick="btnSaleQuotation_Click"
                                                    Text="Create  Quotation"  style="display:none;"  />--%>
                                                  <a href="javascript:void(0)" id="Button2" class="btnUpdate btn btn-success"  onclick="Btnsalesquotation();" style="display:none;">Create  Proforma</a>


                                                <%-- <% } %>
                                                  <% if (rightsSalesOrder.CanAdd)
                                                                            { %>--%>
                                                  <a href="javascript:void(0)" id="Button3" class="btnUpdate btn btn-success"  onclick="Btnsalesorder();" style="display:none;">Create Sale Order</a>
                                                                    
                                              <%--  <asp:Button ID="Button3" runat="server" CssClass="btnUpdate btn btn-success" OnClick="btnSaleOrder_Click"
                                                    Text="Create Sale Order"  style="display:none;"  />--%>
                                                <%--  <% } %>--%>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:TextBox ID="txtStartDate" runat="server" BackColor="Transparent" BorderColor="Transparent" CssClass="hide"
                                    BorderStyle="None" ForeColor="#DDECFE"></asp:TextBox>
                                <asp:TextBox ID="txtEndTime" runat="server" BackColor="Transparent" BorderColor="Transparent" CssClass="hide"
                                    BorderStyle="None" ForeColor="#DDECFE"></asp:TextBox>
                                <asp:TextBox ID="txtExp" Text="0" runat="server" Visible="False" BackColor="Transparent"
                                    BorderColor="Transparent" BorderStyle="None" ForeColor="#DDECFE"></asp:TextBox>
                                <asp:Label ID="lblVisitExp" runat="server" Text="Visit Expenses:" BackColor="Transparent"
                                    BorderColor="Transparent" BorderStyle="None" ForeColor="#DDECFE" Visible="false"
                                    Width="65px"></asp:Label>
                                <input type="hidden" id="txtProductCount" name="txtProductCount" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
     <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupbudget" Height="500px"
                Width="1310px" HeaderText="Budget" Modal="true" AllowResize="true" ResizingMode="Postponed">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                
                 <ClientSideEvents CloseUp="BudgetAfterHide" />
            </dxe:ASPxPopupControl>
     <asp:HiddenField ID="hdndisableChoosen" runat="server" />

      <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
            Width="1300px" HeaderText="Sales Order" Modal="true" AllowResize="true" ResizingMode="Postponed" >
            <%--<HeaderTemplate>
                <span>CRM Sales Order</span> 
            </HeaderTemplate>--%>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl> 

     <dxe:ASPxPopupControl ID="ASPXPopupControl1" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupProforma" Height="630px"
            Width="1300px" HeaderText="Proforma Invoice/Quotation" Modal="true" AllowResize="true" ResizingMode="Postponed" >
            <%--<HeaderTemplate>
                <span>CRM Sales Order</span> 
            </HeaderTemplate>--%>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl> 
</asp:Content>
