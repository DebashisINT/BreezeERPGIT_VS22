<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false" Title="Phone Calls"
    Inherits="ERP.OMS.Management.management_CRMPhoneCallWithFrame" CodeBehind="CRMPhoneCallWithFrame.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <style type="text/css">
        .tblhd {
            color: white !important;
            background: #415698 !important;
            border-width: 1px;
            border-style: solid; 
            font-weight: normal;
            border-top: 1px none #2c4182 !important;
            border: 1px solid #2c4182 !important;
        }


        .chosen-container.chosen-container-multi,
        .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }

        #lstCallDisposion {
            width: 200px;
        }

        .hide {
            display: none;
        }

        #MandatoryNotes {
            position: absolute;
            right: 0px;
            top: 7px;
        }

          #MandatoryPhoneStatus {
            position: absolute;
            right: 0px;
            top: 7px;
        }
        
        #MandatoryCallDate {
            position: absolute;
            right: 0;
            top: 10px;
        }

        #chkStage {
            -webkit-transform: translateY(2px);
            transform: translateY(2px);
        }
    </style>
    <script language="javascript" type="text/javascript">

        function Btnsalesorder() {

            debugger;
            var SalesId = '<%=Convert.ToString(Request.QueryString["TransSale"])%>';
            var TypeId = '<%=Int32.Parse(Request.QueryString["type"])%>';
            // alert(SalesId + ' ' + TypeId)
            // Mantis Issue 24913
            var AssignedToName = '<%=Convert.ToString(Request.QueryString["AssignedToName"])%>';
            var AssignedToId = '<%=Convert.ToString(Request.QueryString["AssignedToId"])%>';
            // End of Mantis Issue 24913


            //string strCustomer = oDBEngine.ExeSclar("select sls_contactlead_id from tbl_trans_sales where sls_id=" + Request.QueryString["SalId"]);

            $.ajax({
                type: "POST",
                url: "CRMPhoneCallWithFrame.aspx/GetBOIsExistInBI",
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
                        // Mantis Issue 24913 [ AssignedToName and AssignedToId passed as parameter ]
                        url = "/OMS/Management/Activities/SalesOrderAdd.aspx?key=ADD" + "&type=" + TypeId + "&SalId=" + SalesId + "&AssignedToName=" + AssignedToName + "&AssignedToId=" + AssignedToId;
                        //window.location.href = url;
                        popup.SetContentUrl(url);
                        popup.Show();
                    }


                }
            });

            // var url = "/OMS/Management/Activities/SalesOrderAdd.aspx?key=ADD" + "&type=" + TypeId + "&SalId=" + SalesId;
            //window.open(url);



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
                url: "CRMPhoneCallWithFrame.aspx/GetBOIsExistInBI",
                data: "{'keyValue':'" + SalesId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,//Added By:Subhabrata
                success: function (msg) {
                    debugger;
                    var url = '';
                    var status = msg.d;

                    if (DisposionId == '56' || DisposionId == '66') {
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

            //window.open(url);
            //popupProforma.SetContentUrl(url);
            //popupProforma.Show();

        }
        function SameDate() {
            jAlert('Next date cannot be same date.', 'Sales Activity');
            return false;
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

        function CallDispositionChange(s) {
            // alert();

        }

        function MailFetchAssignto() {

            return $("mailassignfield").val();
        }
        $(document).ready(function () {
            $("#Button2").hide();
            $("#btnSavePhoneCallDetails").click(function () {
                var flag = true;
                //var Notes = $('#txtNotes').text();
                var Notes = txtNotes.GetText();
                var drp = document.getElementById("TxtOut")
                var st = drp.value;

                if (st != '' && st != null)
                { st = drp.value.split("|"); }

                var CallDate = '';

                $("#Button2").attr('style', 'display:none');
                $("#Button1").attr('style', 'display:none');

                var checked_radio = $('#<%=RdActivityList.ClientID %> input:checked').val();
                // var checked_radio = $('#<%=RdActivityList.ClientID %>').val();

                if (checked_radio == undefined) {

                    flag = false;
                    $('#MandatoryPhoneStatus').attr('style', 'display:block');
                }
                else {


                    $('#MandatoryPhoneStatus').attr('style', 'display:none');
                }

                if (Notes == "") {
                    flag = false;
                    $('#MandatoryNotes').attr('style', 'display:block');
                }
                else { $('#MandatoryNotes').attr('style', 'display:none'); }



                if ((st[1] == 1) || (st[1] == 2)) {
                    CallDate = tnextdate.GetValue()
                    if (CallDate == null || CallDate == "") {
                        flag = false;
                        $('#MandatoryCallDate').attr('style', 'display:block');
                    }
                    else { $('#MandatoryCallDate').attr('style', 'display:none'); }
                }
                else if ((st[1] == 3) || (st[1] == 6))
                { }
                else
                {
                    CallDate = tnextdate.GetValue()
                    if (CallDate == null || CallDate == "") {
                        flag = false;
                        $('#MandatoryCallDate').attr('style', 'display:block');
                    }
                    else { $('#MandatoryCallDate').attr('style', 'display:none'); }
                }

                return flag;
            })
        });
        var chkobj;
        var objchk = null;
        var obGenral = null;
        function chkGenral(objGenral, val12) {
            var st = document.getElementById("txtGrdContact")

            if (obGenral == null) {
                obGenral = objGenral;
            }
            else {
                obGenral.checked = false;
                obGenral = objGenral;
                obGenral.checked = true;
            }
            st = val12;
            //st.value = val12;
        }
        function height() {
            if (document.body.scrollHeight >= 500)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '500px';
            window.frameElement.Width = document.body.scrollWidth;

            $('.btnGrp .btn').click(function () {
                $('.btnGrp .btn').removeClass('btn-success').addClass('btn-primary');
                $(this).removeClass('btn-primary').addClass('btn-primary');
            });

        }

        function OnMoreInfoClick(KeyValue) {
            document.location.href = "CRMPhoneCalls.aspx?id=" + KeyValue;

        }

        function frmOpenNewWindow1(url, title, v_height, v_weight) {
            OnMoreInfoClick(url, title, v_height, v_weight, "Y");
        }
        function callback() {
            iframesource();
        }
        function calldispose(Obj, val) {
            var y = (screen.availHeight - 450) / 2;
            var x = (screen.availWidth - 900) / 2;
            var str = "SalesOutCome1.aspx?call=" + val + "&obj=" + Obj
            window.open(str, "Search_Conformation_Box", "height=450,width=900,top=" + y + ",left=" + x + ",location=no,directories=no,menubar=no,toolbar=no,status=yes,scrollbars=no,resizable=no,dependent=no");

            //        var str = "SalesOutCome1.aspx?call="+val+"&obj="+Obj
            //        frmOpenNewWindow1(str,400,900)
        }


        function CallEmail1(val) {
            parent.CallEmail(val);

        }



        function Changedata(txtdate, txttime, nowdate, nowtime, cdate, ctime) {
            //var drp = document.getElementById("txtOutCome_id")
            var drp = document.getElementById("TxtOut")
            var a = document.getElementById("chkStage");
            if (a != null) {
                a.checked = false
                a.disabled = true
            }
            $('#btnCallForward').prop("disabled", true);
            $('#btnSendEmail').prop("disabled", true);
            //document.getElementById("btnCallForward").disabled = true
            //document.getElementById("btnSendEmail").disabled = true
            var st = drp.value;
            var st = drp.value.split("|")
            if ((st[1] == 1) || (st[1] == 2)) {

                var nextdate = document.getElementById("trnextdate");
                nextdate.style.display = "block";
                var nexttime = document.getElementById("trnexttime");
                document.getElementById("lblNextDate").style.display = "block";
                document.getElementById("ASPxNextDate").style.display = "block";
                document.getElementById("lblNextTime").style.display = "block";
                //document.getElementById("lblNextDate1").style.display = "none";
                //document.getElementById("tdnext").style.display = "none";
                document.getElementById("tdvisibility").style.width = "";
                document.getElementById("tdvisibility2").style.width = "";
                document.getElementById("tdvisibility2").style.textAlign = "right";
                document.getElementById("idtime").style.display = "table-cell";
                document.getElementById("tdNextDateLbl").style.display = "table-cell";
                document.getElementById("lblNextTime1").style.display = "none";
                $('#drpvisitplace').prop("disabled", false);
                //document.getElementById("drpvisitplace").disabled = true;
                if ((st[1] == 1)) {
                    $('#btnCallForward').prop("disabled", false);
                    $('#btnSendEmail').prop("disabled", false);
                    //document.getElementById("btnCallForward").disabled = false;
                    //document.getElementById("btnSendEmail").disabled = false;
                }
            }
            else {
                if ((st[1] == 3) || (st[1] == 6)) {
                    var nextdate = document.getElementById("trnextdate");
                    nextdate.style.display = "none";
                    var nexttime = document.getElementById("trnexttime");
                    var lbldate = document.getElementById("lblNextDate").style.display = "none";
                    //document.getElementById("lblNextDate1").style.display = "none";
                    console.log('none');
                    document.getElementById("idtime").style.display = "none";
                    document.getElementById("tdNextDateLbl").style.display = "none";
                    $('#drpvisitplace').prop("disabled", true);
                    //document.getElementById("drpvisitplace").disabled = true;
                    if (a != null) {
                        if ('<%=Session["contactInternalId"]%>' != null) {
                            var leadtype = '<%=Session["contactInternalId"]%>'
                            var res = leadtype.substr(0, 2);
                            if (res == 'LD') {
                                a.checked = true;
                                //kaushik
                                a.disabled = true;
                            }
                            else {
                                a.visible = false;
                            }

                        }
                    }

                }
                else {
                    var nextdate = document.getElementById("trnextdate");
                    nextdate.style.display = "block";
                    var nexttime = document.getElementById("trnexttime");
                    document.getElementById("lblNextDate").style.display = "block";
                    document.getElementById("ASPxNextDate").style.display = "block";
                    document.getElementById("lblNextTime").style.display = "none";
                    //document.getElementById("lblNextDate1").style.display = "block";
                    //document.getElementById("lblNextTime1").style.display = "block";
                    document.getElementById("idtime").style.display = "table-cell";
                    document.getElementById("tdNextDateLbl").style.display = "table-cell";
                    $('#drpvisitplace').prop("disabled", false);
                    $('#btnSendEmail').prop("disabled", false);
                    //document.getElementById("drpvisitplace").disabled = false;
                    //document.getElementById("btnSendEmail").disabled = false;
                    //adddatetime(txtdate,nowdate,ctime)
                }
            }
            var savecall = $("#btnSavePhoneCallDetails").disabled = false;
            //var savecall = document.getElementById("btnSavePhoneCallDetails").disabled = false;
        }

        // Code ended above

        function callforward() {
            var startcall = document.getElementById("txtEndTime").value;
            var nextcall = document.getElementById("txtStartDate").value;
            var note = document.getElementById("txtNotes").value;
            var CallDispose = document.getElementById("TxtOut").value;
            frmOpenNewWindow1('frmcallforward.aspx?callstarttime=' + startcall + '&ASPxNextDate=' + nextcall + '&txtNote=' + note + '&txtCallDispose_id=' + CallDispose, '300', '600');
        }
        function FillValues(id) {
            //        var sel = document.getElementById('txtProductCount');
            //        sel.value=id;
            noofproduct = id;
        }

        function chkOnSaveClick123() {
            var drp = document.getElementById("TxtOut");
            var st = drp.value.split("|");
            if ((st[1] == 5) || (st[1] == 4)) {
                //var sel = document.getElementById('txtProductCount');
                if (noofproduct == 0) {
                    alert("For confirm sale/Request Meeting choose alleast one product");
                    return false;
                }

            }
            if ((st[1] == 5) || (st[1] == 4)) {
                var sel = document.getElementById("drpVisitPlace");
                if (sel.value == 0) //|| (sel.options.length == 0)
                {
                    alert("Please Add Address");
                    return false;
                }
            }
            if ((st[1] == 1) || (st[1] == 2)) {
                var flag = checkDateFunction12()
                if (flag == false) {
                    alert("Time must greater than current time");
                    return false;
                }
            }
            return true;
        }
        function checkDateFunction12() {
            var now = new Date();
            var year = now.getYear();
            var month = now.getMonth() + 1;
            var date = now.getDate();//today
            var hh = now.getHours();
            if (hh == 13) {
                hh = "01 PM";
            }
            if (hh == 14) {
                hh = "02 PM";
            }
            if (hh == 15) {
                hh = "03 PM";
            }
            if (hh == 16) {
                hh = "04 PM";
            }
            if (hh == 17) {
                hh = "05 PM";
            }
            if (hh == 18) {
                hh = "06 PM";
            }
            if (hh == 19) {
                hh = "07 PM";
            }
            if (hh == 20) {
                hh = "08 PM";
            }
            if (hh == 21) {
                hh = "09 PM";
            }
            if (hh == 22) {
                hh = "10 PM";
            }
            if (hh == 23) {
                hh = "11 PM";
            }
            if (hh == 24) {
                hh = "12 PM";
            }
            else {
                if (hh == 12) {
                    hh = hh + " PM";
                }
                else {
                    hh = hh + " AM";
                }
            }
            hh = hh.split(' ');
            var mm = now.getMinutes() + 5;
            var ss = now.getSeconds();

            var d = document.getElementById("ASPxNextDate")

            var inputdate = d.value.split(' ');
            var da = inputdate[0]
            if (date == 1 || date == 2 || date == 3 || date == 4 || date == 5 || date == 6 || date == 7 || date == 8 || date == 9) {
                date = "0" + date;
            }
            if (month == 1 || month == 2 || month == 3 || month == 4 || month == 5 || month == 6 || month == 7 || month == 8 || month == 9) {
                month = "0" + month;
            }
            var mo = inputdate[1];
            var inputmonth;
            if (mo == 'January') {
                inputmonth = "01";
            }
            if (mo == 'February') {
                inputmonth = "02";
            }
            if (mo == 'March') {
                inputmonth = "03";
            }
            if (mo == 'April') {
                inputmonth = "04";
            }
            if (mo == 'May') {
                inputmonth = "05";
            }
            if (mo == 'June') {
                inputmonth = "06";
            }
            if (mo == 'July') {
                inputmonth = "07";
            }
            if (mo == 'August') {
                inputmonth = "08";
            }
            if (mo == 'September') {
                inputmonth = "09";
            }
            if (mo == 'October') {
                inputmonth = "10";
            }
            if (mo == 'November') {
                inputmonth = "11";
            }
            if (mo == 'December') {
                inputmonth = "12";
            }
            var y = inputdate[2];
            var m = inputdate[3].split(':')

            if (y >= year) {
                if (y > year) {
                    return true
                }
                else {
                    if (inputmonth >= month) {

                        if (inputmonth > month) {
                            return true
                        }
                        else {
                            if (da >= date) {
                                if (da > date) {
                                    return true
                                }
                                else {
                                    if (inputdate[4] == hh[1]) {
                                        if (m[0] >= hh[0]) {
                                            if (m[0] > hh[0]) {
                                                return true
                                            }
                                            else {
                                                if (m[1] < mm) {
                                                    return false
                                                }
                                            }
                                        }
                                        else {
                                            return false

                                        }
                                    }
                                    else {
                                        return true;
                                    }
                                }
                            }
                            else {
                                return false
                            }
                        }
                    }
                    else {
                        return false
                    }
                }




            }
            else {
                return false
            }

            return true
        }

        function iframesource() {
            //alert('calluserinformation');
            iFrmInformation.location = "calluserinformation.aspx";
        }

        function SignOff() {
            window.parent.SignOff()
        }
        //function height() {

        //    if (document.body.scrollHeight >= 550) {
        //        window.frameElement.height = document.body.scrollHeight;
        //    }
        //    else {
        //        window.frameElement.height = '550px';
        //    }
        //    window.frameElement.widht = document.body.scrollWidht;

        //}
        function callDhtmlFormsParent(val, val1) {
            if (val == "ADD") {
                OnMoreInfoClick("frmOfferedProduct_New.aspx", "ADD PRODUCT", "950px", "500px", "Y");
            }
            else if (val == "UPDATE") {
                var url = "UpdateOfferedProduct.aspx" + "?" + val1;
                OnMoreInfoClick(url, "UPDATE PRODUCT", "950px", "500px", "Y");
            }
        }
    </script>

    <%--   .............................Code Commented and Added by Sam on 29122016. to bind Call disposition list ..................................... --%>
    <script type="text/javascript">
        $(document).ready(function () {
            //$('#lstItems').chosen();

            //for choosen drop down kaushik  17-11-2016

            BindCallDisposion();
            ListBind();
            $("#lstCallDisposion").chosen().change(function () {
                debugger;
                //alert($("#idrightsquotation").val());
                $("#Button2").attr('style', 'display:none');
                $("#Button1").attr('style', 'display:none');

                var callDisposionIdDtl = $(this).val();
                if (callDisposionIdDtl != null) {
                    var callDisposionIdsplit = callDisposionIdDtl.toString().split('!');
                    var callDisposionTimedtl = callDisposionIdsplit[0, 0];

                    var nowdate = callDisposionTimedtl.toString().split(',')[0];

                    var nowtime = callDisposionTimedtl.toString().split(',')[1];

                    var cdate = callDisposionTimedtl.toString().split(',')[2];

                    var ctime = callDisposionTimedtl.toString().split(',')[3];

                    var callDisposionId = callDisposionIdsplit[0, 1];

                    //  alert(callDisposionId);

                    var DisposionIdCat = callDisposionId.toString().split('~');
                    var DisposionId = DisposionIdCat[0, 0];
                    var DisposionCat = DisposionIdCat[0, 1];
                    $('#TxtOut').val(DisposionId + '|' + DisposionCat);
                    if (DisposionCat == 5) {
                        $("#btnSavePhoneCallDetails").prop("disabled", true);
                    }
                    else {
                        $("#btnSavePhoneCallDetails").prop("disabled", false);
                    }

                    $('#btnSavePhoneCallDetails').removeClass("aspNetDisabled");
                    Changedata('txtNextVisitDate1', 'txtNextVisitTime', nowdate, nowtime, cdate, ctime);
                    //document.getElementById("TxtOut").va

                    if (DisposionCat == 1 || DisposionCat == 2) {
                        ///   alert(1);
                        var date = new Date();
                        var date2 = addDays(date, 1);
                        tnextdate.SetDate(date2);


                    }
                    else {

                        var Pid = '<%=Convert.ToString(Request.QueryString["Pid"])%>';
                        var date = new Date();

                        var date2 = addDays(date, 1);
                        if (Pid == "3")
                        { tnextdate.SetDate(date2); }
                        else
                        { tnextdate.SetDate(date); }

                    }

                    if (DisposionCat == 5 && $("#idrightsSaleordFromActivity").val() == 'True') {
                        $("#Button2").attr('style', 'display:block');
                    }
                    else { 

                        $("#Button1").attr('style', 'display:none');

                    }
                     
                    if (DisposionId == 46) {

                        $("#Button1").attr('style', 'display:none');



                    }

                    if (DisposionId == 56 || DisposionId == 66) {
                        
                        $("#Button2").attr('style', 'display:none');

                        $("#Button1").attr('style', 'display:block');


                    }
                    //else
                    //{ $('#Button1').prop("disabled", false); }

                    // txtCallDispose:calldisposition text,  TxtOut:calldisposition id  , txtNextVisitDate1,txtNextVisitTime,


                    $('#<%=hdnCallDisposion.ClientID %>').val(callDisposionId);
                }
                else {
                    $("#btnSavePhoneCallDetails").prop("disabled", true);
                }
                //var productText = $("#ListBoxProduct").find("option:selected").text();

            })

        });
        $(function () {

            // <%-- document.getElementById('<%=Button1.ClientID %>').style.display = 'none';--%>

            $("#Button1").attr('style', 'display:none');
            <%-- //   document.getElementById('<%=Button2.ClientID %>').style.display = 'none';--%>
            //$("#Button2").attr('style', 'display:none');
            // $("#Button2").style.display = 'none';
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

        function EnabledSaveBtn() {
            $("#btnSavePhoneCallDetails").prop("disabled", false);
        }

        function BindCallDisposion() {
            var lBox = $('select[id$=lstCallDisposion]');
            var listItems = [];
            lBox.empty();
            $.ajax({
                type: "POST",
                url: 'CRMPhoneCallWithFrame.aspx/GetCallDispositionList',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {
                            //debugger;
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

                    }
                    else {
                        lBox.empty();
                        $('#lstCallDisposion').trigger("chosen:updated");
                        $('#lstCallDisposion').prop('disabled', true).trigger("chosen:updated");

                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });


        }
        function ListBind() {
            $('#lstCallDisposion').chosen();
            $('#lstCallDisposion').fadeIn();
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



    </script>
    <script>
        $(document).ready(function () {
            $("#grdUserInfo th").addClass('dxgvHeader_PlasticBlue');

            //$('.btnGrp .btn').click(function () {
            //    $('.btnGrp .btn').removeClass('btn-success').addClass('btn-primary');
            //    $(this).removeClass('btn-primary').addClass('btn-primary');
            //});



        });


        function DateCompletedChangedNextCall(s, e) {
             var CurrDate = new Date();
            var NextcallDate = s.GetDate();
            var datediff = (NextcallDate - CurrDate) / (1000 * 60 * 60 * 24); 
            var checked_radio = $('#<%=RdActivityList.ClientID %> input:checked').val();
            if (datediff > 15 && checked_radio!="1") {
            <%--if (datediff > 15) {--%>

                jAlert('Selected date is 15 days later than current date.Cannot Proceed', 'Sales Activity', function () {
                 
                });
                   tnextdate.SetDate(CurrDate);
            }
            else {
                //alert('No');
            }

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
    <style>
        #grdUserInfo {
            border: 1px solid #d9d9d9;
        }

            #grdUserInfo td {
                padding: 5px 8px;
                border-right: 1px solid #d9d9d9;
            }

        .dxgvHeader_PlasticBlue {
            cursor: pointer;
            white-space: nowrap;
            padding: 7px 6px;
            border-top: 1px none #2c4182;
            border: 1px solid #2c4182;
            background: #415698 url(/DXR.axd?r=0_4426-RqHhd) repeat-x top;
            overflow: hidden;
            font-weight: normal;
            text-align: left;
        }

        .strong {
            font-weight: 600;
        }

        .afftbl {
            width: 70%;
            margin-top: 20px;
        }

            .afftbl > tbody > tr > td {
                padding-right: 20px;
            }

        #lstCallDisposion + .chosen-container-single {
            width: 300px !important;
            margin-bottom: 5px;
            margin-top: 3px;
        }

        #content-6.horizontal-images.content li {
            float: left;
        }

        .wrapHolder {
            height: auto;
        }
        #ddl_activitystatus {
            display:none;
        }
        #RdActivityList{
            border: 1px solid #afadad;
            margin-bottom: 5px;
            width:100%;
        }
        #RdActivityList>tbody>tr>td {
            padding-right:10px;
            padding-top:5px;
        }
        #RdActivityList>tbody>tr>td:first-child {
            padding-left:8px;
        }
    </style>

    <%-- .............................Code Above Commented and Added by Sam on 29122016...................................... --%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Phone Calls</h3>
            <%--<div class="crossBtn" runat="server" id="dccrossaccess"><a href="Activities/crm_sales.aspx"><i class="fa fa-times"></i></a></div>--%>
            <div class="crossBtn" runat="server" id="dccrossaccess"></div>
        </div>

    </div>
    <div class="form_main">
        <table class="TableMain100">

            <tr id="pnlFButton" runat="server">
                <td style="text-align: left; width: 100%">
                    <table>
                        <asp:Panel ID="pnlFButton1" runat="server" Visible="false">
                            <tr>
                                <td colspan="10" class="btnGrp">
                                    <div class="btn-group" role="group" aria-label="...">
                                        <asp:Button ID="BtnCalls" runat="server" Text="New Calls" CssClass="btnUpdate btn btn-success" OnClick="BtnCalls_Click" />
                                        <asp:Button ID="BtnBack" runat="server" Text="Call Back" CssClass=" btn btn-primary" OnClick="BtnBack_Click" /><asp:Button ID="BtnContact" runat="server" Text="Non Contactable" CssClass="btnUpdate btn btn-primary"
                                            OnClick="BtnContact_Click" /><asp:Button ID="BtnUsable" runat="server" Text="Non Usable" CssClass="btnUpdate btn btn-primary"
                                                OnClick="BtnUsable_Click" /><asp:Button ID="BtnWon" runat="server" Text="Won/ConfirmSale" CssClass="btnUpdate btn btn-primary"
                                                    OnClick="BtnWon_Click" /><asp:Button ID="BtnLost" runat="server" Text="Lost/Not Interested" CssClass="btnUpdate btn btn-primary"
                                                        OnClick="BtnLost_Click" /><asp:Button ID="BtnPipeline" runat="server" Text="Pipeline/Sales Visit" CssClass="btnUpdate btn btn-primary"
                                                            OnClick="BtnPipeline_Click" /><asp:Button ID="BtnCourtesy" runat="server" Text="Courtesy Calls" CssClass="btnUpdate btn btn-primary"
                                                                OnClick="BtnCourtesy_Click" /><asp:Button ID="BtnForward" runat="server" Text="Forward Calls" CssClass="btnUpdate btn btn-primary"
                                                                    OnClick="BtnForward_Click" />
                                    </div>

                                </td>

                            </tr>
                        </asp:Panel>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 100%">
                    <table style="width: 100%">
                        <tr id="pnlBtn" runat="server">
                            <td style="text-align: left;">
                                <table>
                                    <tr>
                                        <asp:Panel ID="pnlBtn1" runat="server" Visible="false">
                                            <td colspan="5" class="btnGrp">
                                                <div class="btn-group" role="group" aria-label="...">
                                                    <asp:Button ID="btnCall" runat="server" Text="Call" CssClass="btnUpdate btn btn-primary" OnClick="BtnCall_Click" />
                                                    <asp:Button ID="BtnService" runat="server" Text="Temp. Out of Service" CssClass="btnUpdate btn btn-primary"
                                                        OnClick="BtnService_Click" />
                                                    <asp:Button ID="BtnReachable" runat="server" Text="Not Reachable" CssClass="btnUpdate btn btn-primary"
                                                        OnClick="BtnReachable_Click" />
                                                    <asp:Button ID="BtnResponse" runat="server" Text="No Response" CssClass="btnUpdate btn btn-primary"
                                                        OnClick="BtnResponse_Click" />
                                                    <asp:Button ID="BtnExist" runat="server" Text="Number Doesnot Exist" CssClass="btnUpdate btn btn-primary"
                                                        OnClick="BtnExist_Click1" />
                                                </div>

                                            </td>

                                        </asp:Panel>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="height: auto" id="TotalNoOfCalls" runat="server">
                            <td>
                                <table class="TableMain100">
                                    <tr>
                                        <td class="Ecoheadtxt" style="width: 47%; text-align: left">
                                            <asp:Label ID="lblTotalRecord" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                        <td class="Ecoheadtxt" style="text-align: left">
                                            <asp:Label ID="lblTotalNumberofCalls" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">

                                <asp:GridView ID="grdUserInfo" runat="server" OnPageIndexChanging="grdUserInfo_PageIndexChanging" overflow="auto"
                                    Width="100%" CssClass="gridcellleft" CellPadding="4" ForeColor="#333333" GridLines="None" Height="400px"
                                    BorderWidth="1px" BorderColor="" OnRowDataBound="grdUserInfo_RowDataBound"
                                    OnRowCreated="grdUserInfo_RowCreated" AllowPaging="True" PageSize="20">
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <RowStyle BackColor="" BorderColor="#d9d9d9" BorderWidth="1px" />
                                    <EditRowStyle BackColor="" BorderColor="#d9d9d9" />
                                    <SelectedRowStyle BackColor="" BorderColor="#d9d9d9" Font-Bold="True" ForeColor="#333333" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <HeaderStyle Font-Bold="False" ForeColor="Black" CssClass="EHEADER " BorderColor="AliceBlue"
                                        BorderWidth="1px" />
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSel" runat="Server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="Server" Text='<%# Eval("Id") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="gridheader tblhd" />
                                </asp:GridView>








                                <%--<asp:Button ID="TxtNext" runat="server" Visible="false" Text="Next" OnClick="TxtNext_Click" CssClass="btnUpdate"
                                    Height="21px" />
                                <asp:Button ID="TxtPrevious" Visible="false" runat="server" Text="Previous" CssClass="btnUpdate"
                                    OnClick="TxtPrevious_Click" Height="21px" />--%>
                                <asp:GridView ID="grdForwardCall" runat="server" CssClass="gridcellleft" CellPadding="4"
                                    ForeColor="#333333" GridLines="None" BorderWidth="1px" BorderColor="#507CD1"
                                    Width="100%" OnRowCreated="grdForwardCall_RowCreated">
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <HeaderStyle Font-Bold="False" ForeColor="Black" CssClass="EHEADER" BorderColor="AliceBlue"
                                        BorderWidth="1px" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" id="tblshowdetails" runat="server" style="width: 100%">
                                <asp:Panel ID="PnlShowDetails" runat="server" Width="100%" Visible="False">
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                        <tr>
                                            <td style="width: 100%;">
                                                <div id="content-6" class="wrapHolder content horizontal-images" style="width: 100%; margin-right: 0px;">
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
                                                                                <asp:Label ID="txtAlloatedBy" runat="server"></asp:Label></td>
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
                                                                                <asp:Label ID="txtDateOfAllottment" runat="server"></asp:Label>
                                                                                <asp:Label ID="txtTotalNumberofCalls" runat="server" Visible="False"></asp:Label>

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
                                                                                <asp:Label ID="txtPriority" runat="server"></asp:Label>

                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </div>

                                                        </li>

                                                        <li>
                                                            <div class="lblHolder" runat="server" id="lastcallliid">
                                                                <table>
                                                                    <tbody>
                                                                        <tr>
                                                                            <td>Last Call </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblLastVisit" runat="server"></asp:Label>
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
                                                                            <td>Call Status </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label CssClass="EcoheadCon" ID="lblLastOutcome" runat="server"></asp:Label>
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
                                                                                <asp:Label ID="lblcustomer" runat="server"></asp:Label>
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


                                                        <li runat="server" id="lilastnote">
                                                            <div class="lblHolder">
                                                                <table>
                                                                    <tbody>
                                                                        <tr>
                                                                            <td>Last Note/Remarks </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblShortNote" runat="server"></asp:Label>
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


                                                <table style="width: 100%; display: none;">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label4" runat="server" CssClass="mylabel"><strong>Assigned By :</strong></asp:Label></td>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="Label5" runat="server" CssClass="mylabel"><strong>Assigned On :</strong></asp:Label></td>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="Label8" runat="server" CssClass="mylabel"><strong>Priority :</strong></asp:Label></td>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="Label10" runat="server" CssClass="mylabel"><strong>Start By :</strong></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="txtSeheduleStart" runat="server"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label6" runat="server" CssClass="mylabel"><strong>End By :</strong></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="txtSeheduleEnd" runat="server"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="Label7" runat="server" CssClass="mylabel"><strong>Started On :</strong></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="txtAcutalStart" runat="server"></asp:Label></td>
                                                        <td>
                                                            <asp:Label CssClass="mylabel strong" ID="Label3" runat="server" Text="Next Call :" wrap="false"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblNextVisit" runat="server" CssClass="EcoheadCon"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="Label1" runat="server" Text="Last Call :" CssClass="mylabel strong"></asp:Label></td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label2" runat="server" Text="Call Status :" CssClass="mylabel strong"></asp:Label></td>
                                                        <td>/td>
                                                        <td>
                                                            <asp:Label ID="Label9" runat="server" CssClass="mylabel"><strong>Instruction :</strong></asp:Label></td>
                                                            <td colspan="5">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <%-- <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="javascript:void(0)" CssClass="btn btn-primary btn-small"
                                                                >Phone /Address</asp:HyperLink>--%>
                                                        </td>
                                                        <td>
                                                            <%--<asp:HyperLink ID="HyperLink2" runat="server"
                                                                NavigateUrl="javascript:void(0)" CssClass="btn btn-primary btn-small">Details </asp:HyperLink>--%>

                                                        </td>
                                                        <td colspan="9" align="left">
                                                            <%-- <asp:HyperLink ID="aa" runat="server" CssClass="btn btn-primary btn-small" NavigateUrl="javascript:void(0)"
                                                               >Show History</asp:HyperLink>--%>

                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <%--  <tr>
                                            <td style="border: solid 1px #b8b8b8">
                                               <iframe id="iFrmInformation" src="" width="100%" height="200" frameborder="0"></iframe>
                                            </td>
                                        </tr>--%>
                                        <tr id="pnlData1">
                                            <td class="gridcellleft">
                                                <table class="afftbl" style="width: 73%">
                                                    <tr>
                                                        <td style="width: 150px">
                                                            <asp:Label ID="Label21" runat="server" CssClass="mylabel1">Phone Status :</asp:Label>
                                                        </td>
                                                        <td colspan="3" style="position:relative">

                                                             <asp:RadioButtonList ID="RdActivityList" runat="server" RepeatDirection="horizontal">
                                                                 </asp:RadioButtonList>

                                                            <span id="MandatoryPhoneStatus" style="display: none; position: absolute; right: -10px;">
                                                                <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor2111_EI11" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                                            <asp:DropDownList ID="ddl_activitystatus" runat="server" Width="298px">
                                                                <%--    <asp:ListItem Value="0" Text="Open" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="Document Collection"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="Closed Sales"></asp:ListItem>
                                                                <asp:ListItem Value="3" Text="Future Sales"></asp:ListItem>
                                                                <asp:ListItem Value="5" Text="Clarification Required"></asp:ListItem>--%>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td style="display: none;">
                                                                        <asp:CheckBox ID="chkStage" runat="Server" Text="Lost" Checked="false" ForeColor="Black" /></td>
                                                                    <td id="tdvisibility" runat="server" class="mylabelr">
                                                                        <asp:Label ID="lblVisitPlace" runat="server" Text="Visit Place : " Font-Bold="True" Visible="false"
                                                                            CssClass="mylabel1"></asp:Label></td>
                                                                    <td>
                                                                        <asp:DropDownList ID="drpVisitPlace" runat="Server" Enabled="False" AppendDataBoundItems="True" Visible="false">
                                                                            <asp:ListItem Value="0">Select Address</asp:ListItem>
                                                                        </asp:DropDownList></td>
                                                                </tr>
                                                            </table>
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td style="width: 150px">
                                                            <label>
                                                                <asp:Label ID="Label11" runat="server" CssClass="mylabel1" Text="Call Outcome:"></asp:Label></label></td>
                                                        <td>
                                                            <div>
                                                                <input id="txtOutCome_id" type="hidden" name="txtOutCome_id" style="width: 7px; height: 13px;" />
                                                                <asp:TextBox ID="txtCallDispose" runat="server" Width="100%" CssClass="hide"></asp:TextBox>


                                                                <asp:ListBox ID="lstCallDisposion" runat="server" TabIndex="4" SelectionMode="Single" Font-Size="12px" Height="90px" Width="100%" CssClass="mb0 chsn hide" data-placeholder="Select ..."></asp:ListBox>
                                                                <asp:HiddenField ID="hdnCallDisposion" runat="server" />
                                                                <asp:HiddenField ID="hdnCallDisposionText" runat="server" />
                                                            </div>
                                                        </td>
                                                        <td id="tdNextDateLbl" runat="server">
                                                            <div class="">
                                                                <asp:Label ID="lblNextDate" runat="Server" Text="Next Call Date : " CssClass="mylabel1 pull-left"
                                                                    Width="86px"></asp:Label><span style="color: red" class="pull-left">*</span>
                                                            </div>
                                                        </td>
                                                        <td id="idtime" runat="server" class="relative">
                                                            <div class="">
                                                                <div runat="server">
                                                                    <dxe:ASPxDateEdit ID="ASPxNextDate" runat="server" EditFormat="Custom"  DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                                                                        UseMaskBehavior="True" Width="100%" ClientInstanceName="tnextdate" ClientSideEvents-DateChanged="DateCompletedChangedNextCall" >
                                                                        <ButtonStyle Width="13px">
                                                                        </ButtonStyle>
                                                                    </dxe:ASPxDateEdit>
                                                                </div>
                                                            </div>
                                                            <span id="MandatoryCallDate" style="display: none; position: absolute; right: -10px;">
                                                                <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor2111_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>

                                                        </td>



                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <%--<asp:Label ID="Label12" runat="server" CssClass="mylabel1" Text="Note/Remarks :"> 
                                                                   </asp:Label>--%>
                                                            <label>
                                                                <label>Note/Remarks :<span style="color: red">*</span></label>
                                                            </label>
                                                        </td>
                                                        <td colspan="3" style="position: relative">
                                                            <div>
                                                                <dxe:ASPxMemo ID="txtNotes" runat="server" MaxLength="500" Height="45px" Width="100%" ClientInstanceName="txtNotes">
                                                                </dxe:ASPxMemo>
                                                                <span id="MandatoryNotes" style="display: none">
                                                                    <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>


                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            <asp:CheckBox ID="chkMail" runat="server" />
                                                            Send Mail</td>
                                                    </tr>
                                                </table>
                                                <div class="row">
                                                    <div class="col-md-2">
                                                    </div>


                                                    <div class="col-md-4">
                                                    </div>
                                                    <div class="clear"></div>
                                                    <div class="col-md-2">
                                                    </div>
                                                </div>
                                                <table style="width: 100%">

                                                    <tr>
                                                        <td style="width: 117px"></td>
                                                        <td>

                                                            <%--<asp:TextBox ID="txtNotes" TextMode="MultiLine" runat="server" Height="45px" Width="421px"></asp:TextBox>--%></td>

                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            <table>
                                                                <tr>
                                                                    <td style="padding-left: 150px">
                                                                        <%--Sam made btnSavePhoneCallDetails button enabled true for testing purpose on 29122016--%>
                                                                        <%-- Enabled="False"--%>
                                                                        <asp:Button ID="btnSavePhoneCallDetails" runat="server" Text="Save" Enabled="false"
                                                                            CssClass="btnUpdate btn btn-primary" OnClick="btnSavePhoneCallDetails_Click" /></td>
                                                                    <td>
                                                                        <asp:Button ID="btnSCancel" runat="server" CssClass="btnUpdate btn btn-danger" OnClick="btnSCancel_Click"
                                                                            Text="Cancel" />
                                                                    </td>
                                                                    <td>

                                                                        <asp:HiddenField runat="server" ID="mailassignfield" />
                                                                    
                                                                        <a href="mailto:javascript:MailFetchAssignto();" id="idmail" runat="server" class="btn btn-primary">Send mail to customer</a>

                                                                             
                                                                    </td>

                                                                    <td>
                                                                       <%-- <% if (rightsQuotation.CanAdd)
                                                                           { %>--%>
                                                                    <%--    <asp:Button ID="Button1" runat="server" CssClass="btnUpdate btn btn-success" OnClick="btnSaleQuotation_Click"
                                                                            Text="Create  Quotation" style="display:none;" />--%>
          
                                                                        
                                                                <a href="javascript:void(0)" id="Button1" class="btnUpdate btn btn-success"  onclick="Btnsalesquotation();" style="display:none;">Create  Proforma</a>

                                                                      <%--  <% } %>--%>
                                                                    </td>


                                                                    <td>
                                                                        <% if (rightsSalesActivity.CanCreateOrder)
                                                                           { %>
                                                                       <%-- <asp:Button ID="Button2"  CssClass="btnUpdate btn btn-success"
                                                                       onclientclick="Btnsalesorder();"     Text="Create Sale Order"  style="display:none;" />--%>
                                                                        <a href="javascript:void(0)" id="Button2" class="btnUpdate btn btn-success"  onclick="Btnsalesorder();" >Create Sale Order</a>
                                                                       <% } %>
                                                                    </td>

                                                                    <td>
                                                                        <input type="hidden" id="idrightsquotation" runat="server"  />
                                                                        <input type="hidden" id="idrightsSaleorder" runat="server" />
                                                                        <input type="hidden" id="idrightsSaleordFromActivity" runat="server" />

                                                                        <asp:Button ID="btnSendEmail" runat="Server" Visible="false" Text="Send Email" Enabled="false" CssClass="btnUpdate btn btn-primary"
                                                                            OnClick="btnSendEmail_Click" />
                                                                        <input type="hidden" id="txtProductCount" name="txtProductCount" /></td>
                                                                    <td>
                                                                        <input type="button" id="btnCallForward" name="btnCallForward" style="visibility: hidden" value="Call Forward"
                                                                            onclick="callforward()" disabled="disabled" class="btnUpdate btn btn-primary" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td id="tdvisibility2" runat="server">
                                                            <asp:TextBox ID="TxtOut" runat="server" BackColor="Transparent" BorderColor="Transparent" CssClass="hide"
                                                                BorderStyle="None" ForeColor="#DDECFE" Width="86px"></asp:TextBox>
                                                            <asp:TextBox ID="txtEndTime" runat="server" BackColor="Transparent" BorderColor="Transparent" CssClass="hide"
                                                                BorderStyle="None" ForeColor="#DDECFE"></asp:TextBox>
                                                            <asp:TextBox ID="txtStartDate" runat="server" BackColor="Transparent" BorderColor="Transparent" CssClass="hide"
                                                                BorderStyle="None" ForeColor="#DDECFE"></asp:TextBox>
                                                        </td>

                                                    </tr>
                                                    <tr id="trnexttime" style="display: none">
                                                        <td class="Ecoheadtxt" style="text-align: left; width: 117px;">
                                                            <asp:Label ID="lblNextTime" runat="server" Text="Next Call Time : " Font-Bold="False"
                                                                CssClass="mylabel1"></asp:Label>
                                                            <asp:Label ID="lblNextTime1" runat="server" Text="Next Visit Time : " Font-Bold="False"
                                                                CssClass="mylabel1"></asp:Label></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" id="tbldata" runat="server" style="width: 100%">
                                <asp:Panel ID="pnlData" Visible="false" Enabled="true" runat="server" Width="100%">
                                    <table width="100%">
                                        <tr>
                                            <td class="Ecoheadtxt" colspan="2" style="text-align: left;">
                                                <table id="nextcall" runat="Server">
                                                    <tr id="trnextdate" style="display: none">
                                                        <td class="Ecoheadtxt" style="text-align: left">&nbsp;</td>
                                                        <td class="Ecoheadtxt" style="text-align: left;">&nbsp;<%--<asp:TextBox ID="ASPxNextDate" runat="server"></asp:TextBox>
                                               <asp:Image ID="Image1" runat="server" ImageUrl="~/images/calendar.jpg" />--%></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div class="PopUpArea"> 
            <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupbudget" Height="500px"
                Width="1310px" HeaderText="Budget" Modal="true" AllowResize="true" ResizingMode="Postponed">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                
                 <ClientSideEvents CloseUp="BudgetAfterHide" />
            </dxe:ASPxPopupControl>
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
            </div>
    </div>

    <asp:HiddenField ID="hdncatsts" runat="server" />
</asp:Content>
