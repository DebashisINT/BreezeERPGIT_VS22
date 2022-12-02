<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    Inherits="ERP.OMS.Management.DailyTask.management_DailyTask_frm_ManualBRSwithGrid" CodeBehind="frm_ManualBRSwithGrid.aspx.cs" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe000001" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>--%>

    <%--  --%>
    <%-- <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>

    <script type="text/javascript" src="/assests/js/init.js"></script>


    

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>
  <script src="/assests/pluggins/choosen/choosen.min.js"></script>

    <%--<script type="text/javascript" src="/assests/js/jquery.meio.mask.js"></script>--%>

    <style type="text/css">
        .col-md-2s{
 float: right !important;
        }
           
        #rfvComname {
            position: absolute;
            right: 80px;
            top: 8px;
        }

        #RequiredFieldValidator9 {
            top: 25px !important;
        }

        .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }

        #lstconverttounit {
            width: 200px;
        }

        #lstconverttounit_chosen {
            width: 100% !important;
        }

        .ctcclass {
            position: absolute;
            top: 10px;
            right: -16px;
        }

        .grid_scrollNSEFO {
            overflow-y: scroll;
            overflow-x: no;
            width: 1000px;
            height: 300px;
            scrollbar-base-color: #C0C0C0;
        }
        /* Big box with list of options */
        #ajax_listOfOptions {
            position: absolute; /* Never change this one */
            width: 50px; /* Width of box */
            height: auto; /* Height of box */
            overflow: auto; /* Scrolling features */
            border: 1px solid Blue; /* Blue border */
            background-color: #FFF; /* White background color */
            text-align: left;
            font-size: 0.9em;
            z-index: 32767;
        }


            #ajax_listOfOptions div { /* General rule for both .optionDiv and .optionDivSelected */
                margin: 1px;
                padding: 1px;
                cursor: pointer;
                font-size: 0.9em;
            }

            #ajax_listOfOptions .optionDiv { /* Div for each item in list */
            }

            #ajax_listOfOptions .optionDivSelected { /* Selected item in the list */
                background-color: #DDECFE;
                color: Blue;
            }

        #ajax_listOfOptions_iframe {
            background-color: #F00;
            position: absolute;
            z-index: 3000;
        }

        form {
            display: inline;
        }

        #trsearch table {
            width: 100% !important;
        }

        td #txtAccName {
            margin-right: 15px;
        }

        #trsearch table th {
            padding-right: 15px;
            display: table-cell !important;
        }
    </style>

    <script type="text/javascript">
        (function ($) {
            // call setMask function on the document.ready event
            $(function () {
                //$('input:text').setMask(); sanjib due to 404 on console error
            }
          );
        })(jQuery);

        function Displaydate(s, e) {

            var data = s.GetDate();
            alert(data);
        }
    </script>

    <script language="javascript" type="text/javascript">

        //For chosen
        $(document).ready(function () {
            ListBind();
            ChangeSource();
            $("#RequiredFieldValidator9").css("display", "none");
            //setvaluechosen();

        });
        function ListBind() {

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
        function lstconverttounit() {

            $('#lstconverttounit').fadeIn();

        }
        function ChangeSource() {


            var fname = "%";
            var lconverttounit = $('select[id$=lstconverttounit]');
            lconverttounit.empty();
            var CurrentSegment = document.getElementById('hdn_CurrentSegment').value;
           <%-- var strPutSegment = " and (MainAccount_ExchangeSegment=" + CurrentSegment + " or MainAccount_ExchangeSegment=0)";
            var strQuery_Table = "(Select MainAccount_AccountCode+\'-\'+MainAccount_Name+\' [ \'+MainAccount_BankAcNumber+\' ]\' as IntegrateMainAccount,MainAccount_AccountCode as MainAccount_AccountCode,MainAccount_Name,MainAccount_BankAcNumber from Master_MainAccount where (MainAccount_BankCashType=\'Bank\' or MainAccount_BankCashType=\'Cash\') and (MainAccount_BankCompany=\'" + '<%=Session["LastCompany"] %>' + "\' Or IsNull(MainAccount_BankCompany,'')='')" + strPutSegment + ") as t1";
            var strQuery_FieldName = " Top 10 * ";
            var strQuery_WhereClause = "MainAccount_AccountCode like (\'%RequestLetter%\') or MainAccount_Name like (\'%RequestLetter%\') or MainAccount_BankAcNumber like (\'%RequestLetter%\')";
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';--%>
            //var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            var sql = "Select (MainAccount_AccountCode+'-'+MainAccount_Name+'[ '+MainAccount_BankAcNumber+' ]' )as IntegrateMainAccount,MainAccount_AccountCode as MainAccount_AccountCode,MainAccount_Name,MainAccount_BankAcNumber from Master_MainAccount where MainAccount_BankCashType='Bank' and (MainAccount_BankCompany='" + '<%=Session["LastCompany"] %>' + "' Or IsNull(MainAccount_BankCompany,'')='') and (MainAccount_ExchangeSegment=" + CurrentSegment + " or MainAccount_ExchangeSegment=0)and (MainAccount_AccountCode like ('" + fname + "%') or MainAccount_Name like ('" + fname + "%') or MainAccount_BankAcNumber like ('%" + fname + "%'))";

            $.ajax({
                type: "POST",
                url: "frm_ManualBRSwithGrid.aspx/Getbanks",
                data: JSON.stringify({ query: sql }),
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

                            $('#lstconverttounit').append($('<option>').text(name).val(id));

                        }

                        $(lconverttounit).append(listItems.join(''));

                        lstconverttounit();
                        $('#lstconverttounit').trigger("chosen:updated");

                        Changeselectedvalue();


                    }
                    else {
                        //   alert("No records found");
                        //lstReferedBy();
                        $('#lstconverttounit').trigger("chosen:updated");

                    }
                }
            });
            // }
        }
        function lstconverttounit() {

            $('#lstconverttounit').fadeIn();

        }
        function setvaluechosen() {
            console.log('setval');
            document.getElementById("txtBankName_hidden").value = document.getElementById("lstconverttounit").value;
            if (document.getElementById("txtBankName_hidden").value != '') {

               <%-- document.getElementById('<%= btnShow.ClientID %>').click();--%>
            } else {

                //jAlert("Select ");
            }

        }

        function Changeselectedvalue() {
            var lstconverttounit = document.getElementById("lstconverttounit");
            if (document.getElementById("txtBankName_hidden").value != '') {

                for (var i = 0; i < lstconverttounit.options.length; i++) {
                    if (lstconverttounit.options[i].value == document.getElementById("txtBankName_hidden").value) {
                        lstconverttounit.options[i].selected = true;
                    }
                }
                $('#lstconverttounit').trigger("chosen:updated");
            }

        }
        //end chosen

        function Page_Laod() {
            if (document.getElementById("ChkConsiderAllDate").checked == true) {
                tdConsiderAllDate.style.display = 'table-cell';
                tdConsiderAllDatelbl.style.display = 'table-cell';
                tdDateFrom.style.display = 'none';
                tdDateFromlbl.style.display = 'none';
                tdDateTo.style.display = 'none';
                tdDateTolbl.style.display = 'none';
            }
            else {
                tdConsiderAllDate.style.display = 'none';
                tdConsiderAllDatelbl.style.display = 'none';
                tdDateFrom.style.display = 'block';
                tdDateFromlbl.style.display = 'block';
                tdDateTo.style.display = 'block';
                tdDateTolbl.style.display = 'block';
            }
        }
        //function height() {

        //    if (document.body.scrollHeight >= 300)
        //        window.frameElement.height = document.body.scrollHeight;
        //    else
        //        window.frameElement.height = '300px';
        //    window.frameElement.Width = document.body.scrollWidth;
        //}

        function ShowBankName(obj1, obj2, obj3) {
            ajax_showOptions(obj1, obj2, obj3, 'Current');
        }
        FieldName = 'lblFieldName';


        function setvalue(obj1) {
            var controlid = obj1.id.split('_');
            var getintno = controlid[1].split('l')
            var trantest = document.getElementById(controlid[0] + '_' + getintno[0] + 'l' + getintno[1] + '_' + 'lblTransactionDate');
            var kk = trantest.innerText;
            var kksplit = kk.split('/');
            var years = kksplit[2].split(' ');
            if (kksplit[0] == 1) {
                kksplit[0] = 'Jan';

            }
            else if (kksplit[0] == 2) {
                kksplit[0] = 'Feb';

            }
            else if (kksplit[0] == 3) {
                kksplit[0] = 'Mar';

            }
            else if (kksplit[0] == 4) {
                kksplit[0] = 'Apr';

            }
            else if (kksplit[0] == 5) {
                kksplit[0] = 'May';

            }
            else if (kksplit[0] == 6) {
                kksplit[0] = 'Jun';

            }
            else if (kksplit[0] == 7) {
                kksplit[0] = 'Jul';

            }
            else if (kksplit[0] == 8) {
                kksplit[0] = 'Aug';

            }
            else if (kksplit[0] == 9) {
                kksplit[0] = 'Sep';

            }
            else if (kksplit[0] == 10) {
                kksplit[0] = 'Oct';

            }
            else if (kksplit[0] == 11) {
                kksplit[0] = 'Nov';

            }
            else if (kksplit[0] == 12) {
                kksplit[0] = 'Dec';

            }

            var newDatewFormat = kksplit[1] + ' ' + kksplit[0] + ' ' + years[0];
            //    alert(newDatewFormat);
            var FromDate = Date.parse(newDatewFormat);

            var objValueDate = document.getElementById(controlid[0] + '_' + getintno[0] + 'l' + getintno[1] + '_' + controlid[2]);
            //    var ValDate=Date.parse(objValueDate.value);
            //    alert(objValueDate.value);

            var Testalidity = objValueDate.value.split('-');

            if (Testalidity[1] == 1) {
                Testalidity[1] = 'Jan';

            }
            else if (Testalidity[1] == 2) {
                Testalidity[1] = 'Feb';

            }
            else if (Testalidity[1] == 3) {
                Testalidity[1] = 'Mar';

            }
            else if (Testalidity[1] == 4) {
                Testalidity[1] = 'Apr';

            }
            else if (Testalidity[1] == 5) {
                Testalidity[1] = 'May';

            }
            else if (Testalidity[1] == 6) {
                Testalidity[1] = 'Jun';

            }
            else if (Testalidity[1] == 7) {
                Testalidity[1] = 'Jul';

            }
            else if (Testalidity[1] == 8) {
                Testalidity[1] = 'Aug';

            }
            else if (Testalidity[1] == 9) {
                Testalidity[1] = 'Sep';

            }
            else if (Testalidity[1] == 10) {
                Testalidity[1] = 'Oct';

            }
            else if (Testalidity[1] == 11) {
                Testalidity[1] = 'Nov';

            }
            else if (Testalidity[1] == 12) {
                Testalidity[1] = 'Dec';

            }
            var ValDate = Date.parse(Testalidity[0] + ' ' + Testalidity[1] + ' ' + Testalidity[2]);
            if (parseInt(Testalidity[0]) > 31) {
                alert('Invalid Date Format');
                document.getElementById(controlid[0] + '_' + getintno[0] + 'l' + getintno[1] + '_' + controlid[2]).focus();
                document.getElementById(controlid[0] + '_' + getintno[0] + 'l' + getintno[1] + '_' + controlid[2]).select();
            }
            else if (parseInt(Testalidity[1]) > 12) {
                alert('Invalid Date Format');
                document.getElementById(controlid[0] + '_' + getintno[0] + 'l' + getintno[1] + '_' + controlid[2]).focus();
                document.getElementById(controlid[0] + '_' + getintno[0] + 'l' + getintno[1] + '_' + controlid[2]).select();
            }
            if (ValDate < FromDate) {

                //         alert(controlid[0]+'_'+getintno[0]+'l'+getintno[1]+'_'+controlid[2]);
                document.getElementById(controlid[0] + '_' + getintno[0] + 'l' + getintno[1] + '_' + controlid[2]).focus();
                document.getElementById(controlid[0] + '_' + getintno[0] + 'l' + getintno[1] + '_' + controlid[2]).select();
                alert('Value Date can not be less than Transaction Date');

            }
            else {

                var a = parseInt(getintno[1]) + 1;//For next Value Date
                if (getintno[1] == 8) {
                    a = "9";
                }
                if (getintno[1] == 9) {
                    a = "10";
                }
                if (a < 10) {

                    a = '0' + a;
                }
                //         alert(controlid[0]+'_'+getintno[0]+'l'+a+'_'+controlid[2]);
                //         alert(document.getElementById(controlid[0]+'_'+getintno[0]+'l'+a+'_'+controlid[2]).value);
                //        if(document.getElementById(controlid[0]+'_'+getintno[0]+'l'+a+'_'+controlid[2]).value =="")
                //        { 
                //            alert('forward previous date to next');
                //             document.getElementById(controlid[0]+'_'+getintno[0]+'l'+a+'_'+controlid[2]).value=document.getElementById(controlid[0]+'_'+getintno[0]+'l'+getintno[1]+'_'+controlid[2]).value;
                //        }

            }

        }

        function setvalueValuedate(obj1) {
            var controlid = obj1.id.split('_');
            var getintno = controlid[1].split('l')
            var trantest = document.getElementById(controlid[0] + '_' + getintno[0] + 'l' + getintno[1] + '_' + 'lblTransactionDate');
            var kk = trantest.innerText;
            var kksplit = kk.split('/');
            var years = kksplit[2].split(' ');
            if (kksplit[0] == 1) {
                kksplit[0] = 'Jan';

            }
            else if (kksplit[0] == 2) {
                kksplit[0] = 'Feb';

            }
            else if (kksplit[0] == 3) {
                kksplit[0] = 'Mar';

            }
            else if (kksplit[0] == 4) {
                kksplit[0] = 'Apr';

            }
            else if (kksplit[0] == 5) {
                kksplit[0] = 'May';

            }
            else if (kksplit[0] == 6) {
                kksplit[0] = 'Jun';

            }
            else if (kksplit[0] == 7) {
                kksplit[0] = 'Jul';

            }
            else if (kksplit[0] == 8) {
                kksplit[0] = 'Aug';

            }
            else if (kksplit[0] == 9) {
                kksplit[0] = 'Sep';

            }
            else if (kksplit[0] == 10) {
                kksplit[0] = 'Oct';

            }
            else if (kksplit[0] == 11) {
                kksplit[0] = 'Nov';

            }
            else if (kksplit[0] == 12) {
                kksplit[0] = 'Dec';

            }

            var newDatewFormat = kksplit[1] + ' ' + kksplit[0] + ' ' + years[0];
            //    alert(newDatewFormat);
            var FromDate = Date.parse(newDatewFormat);

            var objValueDate = document.getElementById(controlid[0] + '_' + getintno[0] + 'l' + getintno[1] + '_' + controlid[2]);
            //    var ValDate=Date.parse(objValueDate.value);
            //    alert(objValueDate.value);

            var Testalidity = objValueDate.value.split('-');

            if (Testalidity[1] == 1) {
                Testalidity[1] = 'Jan';

            }
            else if (Testalidity[1] == 2) {
                Testalidity[1] = 'Feb';

            }
            else if (Testalidity[1] == 3) {
                Testalidity[1] = 'Mar';

            }
            else if (Testalidity[1] == 4) {
                Testalidity[1] = 'Apr';

            }
            else if (Testalidity[1] == 5) {
                Testalidity[1] = 'May';

            }
            else if (Testalidity[1] == 6) {
                Testalidity[1] = 'Jun';

            }
            else if (Testalidity[1] == 7) {
                Testalidity[1] = 'Jul';

            }
            else if (Testalidity[1] == 8) {
                Testalidity[1] = 'Aug';

            }
            else if (Testalidity[1] == 9) {
                Testalidity[1] = 'Sep';

            }
            else if (Testalidity[1] == 10) {
                Testalidity[1] = 'Oct';

            }
            else if (Testalidity[1] == 11) {
                Testalidity[1] = 'Nov';

            }
            else if (Testalidity[1] == 12) {
                Testalidity[1] = 'Dec';

            }
            var ValDate = Date.parse(Testalidity[0] + ' ' + Testalidity[1] + ' ' + Testalidity[2]);
            if (parseInt(Testalidity[0]) > 31) {
                alert('Invalid Date Format');
                document.getElementById(controlid[0] + '_' + getintno[0] + 'l' + getintno[1] + '_' + controlid[2]).focus();
                document.getElementById(controlid[0] + '_' + getintno[0] + 'l' + getintno[1] + '_' + controlid[2]).select();
            }
            else if (parseInt(Testalidity[1]) > 12) {
                alert('Invalid Date Format');
                document.getElementById(controlid[0] + '_' + getintno[0] + 'l' + getintno[1] + '_' + controlid[2]).focus();
                document.getElementById(controlid[0] + '_' + getintno[0] + 'l' + getintno[1] + '_' + controlid[2]).select();
            }
            if (ValDate < FromDate) {

                //         alert(controlid[0]+'_'+getintno[0]+'l'+getintno[1]+'_'+controlid[2]);
                document.getElementById(controlid[0] + '_' + getintno[0] + 'l' + getintno[1] + '_' + controlid[2]).focus();
                document.getElementById(controlid[0] + '_' + getintno[0] + 'l' + getintno[1] + '_' + controlid[2]).select();
                alert('Value Date can not be less than Transaction Date');

            }
            else {
                if (document.getElementById(controlid[0] + '_' + getintno[0] + 'l' + getintno[1] + '_' + 'txtStatementDate').value != "") {
                    if (document.getElementById(controlid[0] + '_' + getintno[0] + 'l' + getintno[1] + '_' + 'txtValueDate').value == "") {
                        var StDate = document.getElementById(controlid[0] + '_' + getintno[0] + 'l' + getintno[1] + '_' + controlid[2]).value;
                        var SettlementDate = StDate.split('-');
                        var sdate = SettlementDate[1] + "/" + SettlementDate[0] + "/" + SettlementDate[2];
                        myDate = new Date(sdate);
                        myDate.setDate(myDate.getDate() + 2)
                        var date2 = myDate.getDate() + '-' + parseInt(myDate.getMonth() + 1) + '-' + myDate.getFullYear();
                        document.getElementById(controlid[0] + '_' + getintno[0] + 'l' + getintno[1] + '_' + 'txtValueDate').value = date2
                    }
                }
            }

        }
        function DateChangeBehaviourFrom() {
            var datediff = cFromDate.GetDate() - cDateTo.GetDate()
            if (datediff < -1296000000) {
                var d = new Date(cFromDate.GetDate());
                //d.setDate(d.getDate() + 15);
               // cDateTo.SetDate(d);
            }
            if (datediff > 0) {
                alert('From Date Can not Be More than To Date');
                cFromDate.SetDate(cDateTo.GetDate());
            }
        }
        function DateChangeBehaviourTo() {
            var datediff = cFromDate.GetDate() - cDateTo.GetDate()
            if (datediff < -1296000000) {
                var d = new Date(cFromDate.GetDate());
                //d.setDate(d.getDate() + 15);
                //cDateTo.SetDate(d);
            }
            if (datediff > 0) {
                alert('To Date Can not be less than From Date');
                cDateTo.SetDate(cFromDate.GetDate());
            }
        }

        function StatmentDateChangeBehaviourTo() {
            var datediff = cDatestatment.GetDate()
            var d = new Date(cDatestatment.GetDate());
            
            //ctxt_cashbankdate.SetDate(d);
            cgrdmanualBRS.PerformCallback('Setdate~' + d);
        }
        function SearchVisible(obj) {
            debugger;
            //cDatestatment.SetText('');
            if (obj == 'N') {
                document.getElementById("trsearch").style.display = 'none';
            }
            else {
                document.getElementById("trsearch").style.display = 'inline';
            }
        }

        function ValueDocAlert(obj1) { 
            if (obj1 == 'Y') {
                jAlert('Value date must be equal or greater than Document Date');
            }
        }


        function changeddate(s, e) {
            s.SetText("");
        }
    </script>

    <script type="text/javascript">
        $(document).ready(function () {

            $(".water").each(function () {
                if ($(this).val() == this.title) {
                    $(this).addClass("opaque");
                }
            });

            $(".water").focus(function () {
                if ($(this).val() == this.title) {
                    $(this).val("");
                    $(this).removeClass("opaque");
                }
            });

            $(".water").blur(function () {
                if ($.trim($(this).val()) == "") {
                    $(this).val(this.title);
                    $(this).addClass("opaque");
                }
                else {
                    $(this).removeClass("opaque");
                }
            });
        });

        function ChkConsiderAllDate_OnClick(Checked) {
            if (!Checked) {
                tdDateFrom.style.display = 'inline';
                tdDateFromlbl.style.display = 'block';
                tdDateTo.style.display = 'block';
                tdDateTolbl.style.display = 'block';
            }
            else {
                tdDateFrom.style.display = 'none';
                tdDateFromlbl.style.display = 'none';
                tdDateTo.style.display = 'none';
                tdDateTolbl.style.display = 'none';
            }
        }
        function SetDateRange(WhichCall) {
            if (WhichCall == "UNCLEAR") {
                document.getElementById("ChkConsiderAllDate").checked = true;
                tdConsiderAllDate.style.display = 'inline';
                tdConsiderAllDatelbl.style.display = 'inline';
                tdDateFrom.style.display = 'none';
                tdDateFromlbl.style.display = 'none';
                tdDateTo.style.display = 'none';
                tdDateTolbl.style.display = 'none';
            }
            if (WhichCall == "CLEAR" || WhichCall == "ALL") {
                document.getElementById("ChkConsiderAllDate").checked = false;
                tdConsiderAllDate.style.display = 'none';
                tdConsiderAllDatelbl.style.display = 'none';
                tdDateFrom.style.display = 'block';
                tdDateFromlbl.style.display = 'block';
                tdDateTo.style.display = 'block';
                tdDateTolbl.style.display = 'block';
            }
        }
        function CallBankAccount(obj1, obj2, obj3) {
            var CurrentSegment = document.getElementById('hdn_CurrentSegment').value;
            var strPutSegment = " and (MainAccount_ExchangeSegment=" + CurrentSegment + " or MainAccount_ExchangeSegment=0)";
            var strQuery_Table = "(Select MainAccount_AccountCode+\'-\'+MainAccount_Name+\' [ \'+MainAccount_BankAcNumber+\' ]\' as IntegrateMainAccount,MainAccount_AccountCode as MainAccount_AccountCode,MainAccount_Name,MainAccount_BankAcNumber from Master_MainAccount where (MainAccount_BankCashType=\'Bank\' or MainAccount_BankCashType=\'Cash\') and (MainAccount_BankCompany=\'" + '<%=Session["LastCompany"] %>' + "\' Or IsNull(MainAccount_BankCompany,'')='')" + strPutSegment + ") as t1";
            var strQuery_FieldName = " Top 10 * ";
            var strQuery_WhereClause = "MainAccount_AccountCode like (\'%RequestLetter%\') or MainAccount_Name like (\'%RequestLetter%\') or MainAccount_BankAcNumber like (\'%RequestLetter%\')";
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');
        }
        function replaceChars(entry) {
            out = "+"; // replace this
            add = "--"; // with this
            temp = "" + entry; // temporary holder

            while (temp.indexOf(out) > -1) {
                pos = temp.indexOf(out);
                temp = "" + (temp.substring(0, pos) + add +
                temp.substring((pos + out.length), temp.length));
            }
            return temp;

        }
        function ShowUpdateCancelButton() {
            td_CancelUpdate.style.display = "inline";
            //height(); sanjib due to 404 error on console
        }

    </script>
    <style>
        .checkbox {
            padding-left: 0px;
            padding-right: 20px;
        }

        #lstconverttounit {
            display: none !important;
        }

        .checkbox input[type="checkbox"] {
            margin-left: 0px;
            margin-right: 10px;
        }

        #grdDetails th, {
            padding: 4px 10px;
        }

        #grdDetails td {
            padding: 1px 10px;
        }

        #grdDetails th {
            cursor: pointer;
            white-space: nowrap;
            padding: 7px 6px;
            border-top: 1px none #2c4182;
            border: 1px solid #2c4182;
            background: #415698 url(/DXR.axd?r=0_4426-RqHhd) repeat-x top;
            overflow: hidden;
            font-weight: normal;
            text-align: left;
            color: #fff;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Manual BRS</h3>
        </div>

    </div>
    <div class="form_main">
        <table class="TableMain100" style="width: 100%">
            <%--<tr class="EHEADER">
            <td colspan="4" align="center" style="font-weight: bold; color: Blue">MANUAL BRS
            </td>
        </tr>--%>
            <tr>
                <td colspan="4">
                    <div class="row">
                        <div class="col-md-6">
                            <label>
                                <asp:Label ID="Label1" CssClass="mylabel1" runat="server" Text="Bank" /></label>
                            <div>
                                <%--<asp:TextBox ID="txtBankName" runat="server" Font-Size="12px" Width="100%" onkeyup="CallBankAccount(this,'GenericAjaxList',event)"></asp:TextBox>--%>
                           
                            <asp:Label ID="lblFieldName" runat="server" Text="Label" Visible="false"></asp:Label>
                                <%--  <asp:TextBox
                        ID="txtproduct_hidden" runat="server" Width="14px" Style="display: none">
                    </asp:TextBox>--%>
                  
                             <asp:HiddenField ID="txtBankName_hidden" runat="server"></asp:HiddenField>
                                <asp:ListBox ID="lstconverttounit" CssClass="chsn" runat="server" Width="100%" TabIndex="8" data-placeholder="Select..."></asp:ListBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ValidationGroup="a" runat="server" ControlToValidate="lstconverttounit" Display="Dynamic"
                                    CssClass="fa fa-exclamation-circle ctcclass " ToolTip="Mandatory."
                                    ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="col-md-2">
                            <label id="tdDateFromlbl">
                                <asp:Label ID="lblFromdate" CssClass="mylabel1" runat="server" Text="From:" Width="28px"></asp:Label></label>
                            <div id="tdDateFrom">
                                <dxe:ASPxDateEdit ID="FromDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                    ClientInstanceName="cFromDate" UseMaskBehavior="True" Width="100%">
                                    <ButtonStyle Width="13px"></ButtonStyle>
                                    <ClientSideEvents DateChanged="function(s, e){ DateChangeBehaviourFrom();}" />
                                </dxe:ASPxDateEdit>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label  id="tdDateTolbl">                                
                            <asp:Label ID="lblToDate" CssClass="mylabel1" runat="server" Text="To:"  Width="28px"></asp:Label>
                            </label>
                            <div id="tdDateTo">
                                <dxe:ASPxDateEdit ID="DateTo" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                    ClientInstanceName="cDateTo" UseMaskBehavior="True" Width="100%">
                                    <ButtonStyle Width="13px"></ButtonStyle>
                                    <ClientSideEvents ValueChanged="function(s, e){ DateChangeBehaviourTo(cDateTo);}" />
                                </dxe:ASPxDateEdit>
                            </div>
                        </div>

                        <div class="clear"></div>
                        <div class="col-md-6">
                            <table>
                                <tr>
                                    <td>
                                        <div class="checkbox">
                                            <label>
                                                <asp:RadioButton ID="RdUnCleared" runat="server" Text="" Checked="true" GroupName="R" onclick="SetDateRange('UNCLEAR');" />
                                                Uncleared
                                            </label>
                                        </div>
                                    </td>

                                    <td>
                                        <div class="checkbox">
                                            <label>
                                                <asp:RadioButton ID="RdCleared" runat="server" Text="" GroupName="R" onclick="SetDateRange('CLEAR');" />
                                                Cleared
                                            </label>
                                        </div>

                                    </td>

                                    <td>
                                        <div class="checkbox">
                                            <label>
                                                <asp:RadioButton ID="RdAll" runat="server" Text="" GroupName="R" onclick="SetDateRange('ALL');" />
                                                All
                                            </label>
                                        </div>

                                    </td>

                                    <td class="mylabel1" id="tdConsiderAllDate">
                                        <div class="checkbox">
                                            <label>
                                                <asp:CheckBox ID="ChkConsiderAllDate" runat="server" Checked="true" onclick="ChkConsiderAllDate_OnClick(this.checked)" />
                                                <span id="tdConsiderAllDatelbl">All Dates</span>
                                            </label>
                                        </div>
                                    </td>
                                    <%--<td class="mylabel1" id="tdConsiderAllDatelbl" style="vertical-align: top; height: 11px; text-align: left">
                                </td>--%>
                                </tr>
                            </table>
                        </div>
                        <div class="clear"></div>
                        <div class="col-md-12">
                            <asp:Button ID="btnShow" runat="server" CssClass="btn btn-primary" OnClick="btnShow_Click1" ValidationGroup="a" OnClientClick="setvaluechosen()" Text="Show" />
                            <%-- <button id="btnsave" onclick="setvaluechosen()">Show</button>--%>
                        </div>
                    </div>
                </td>
            </tr>

            <tr>
                <td colspan="4">
                    <table>
                        <tr id="trhypertext" runat="server" visible="false">
                            <td>
                                <span class="btn btn-primary" style="display:none">
                                    <asp:LinkButton ID="lnAllRecords" runat="server" ForeColor="#ffffff"
                                        NavigateUrl="javascript:void(0)" CssClass="myhypertext" OnClick="lnAllRecords_Click">All Records</asp:LinkButton></span>
                            </td>
                            <td style="height: 16px">
                                <span class="btn btn-primary" style="display:none">
                                    <asp:HyperLink ID="hpFilterRecords" runat="server" ForeColor="#ffffff"
                                        NavigateUrl="javascript:void(0)" CssClass="myhypertext" onclick="SearchVisible('Y')">Show Filter</asp:HyperLink></span>
                            </td>
                            <%--<td>
                                <b>Bank Balance :</b>
                            </td>
                            <td>
                                <asp:Literal ID="litBankBalance" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <b>Clear Balance :</b>
                            </td>
                            <td>
                                <asp:Literal ID="litClearBalance" runat="server"></asp:Literal>
                            </td>--%>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trsearch" visible="false" >
                <td colspan="4">
                    <table border="0" width="100%" style="display:none;">
                        <tr>
                            <th valign="middle">
                                <dxe:ASPxDateEdit ID="AspTranDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                    UseMaskBehavior="True" Width="120px" NullText="Tran Date">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </th>
                            <th valign="middle">
                                <asp:TextBox ID="txtVoucherNo" runat="server" Width="120px" CssClass="water" Text="Document No"
                                    ToolTip="Document No"></asp:TextBox>
                            </th>
                            <th valign="middle">
                                <dxe:ASPxDateEdit ID="AspInsDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                    UseMaskBehavior="True" Width="120px" NullText="Instrument Date">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </th>
                            <th>
                                <asp:TextBox ID="txtInsNo" runat="server" CssClass="water" Text="Instrument No" ToolTip="Instrument No"
                                    Width="100px"></asp:TextBox>
                            </th>
                            <th valign="middle">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtAccName" runat="server" Width="155px" CssClass="water" Text="Main Account"
                                                ToolTip="Main Account"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtSubName" runat="server" Width="80px" CssClass="water" Text="SubAccount"
                                                ToolTip="SubAccount"></asp:TextBox></td>
                                    </tr>
                                </table>
                            </th>
                            <th valign="middle">
                                <asp:TextBox ID="txtPayAmt" runat="server" Width="60px" CssClass="water" Text="Pmt Amount"
                                    ToolTip="Pmt Amount"></asp:TextBox>
                            </th>
                            <th valign="middle">
                                <asp:TextBox ID="txtReptAmt" runat="server" Width="60px" CssClass="water" Text="Rcpt Amount"
                                    ToolTip="Rcpt Amount"></asp:TextBox>
                            </th>
                            <th valign="middle">
                                <dxe:ASPxDateEdit ID="AspValueDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                    UseMaskBehavior="True" Width="120px" NullText="Value Date">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </th>
                            <th valign="middle">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" 
                                    CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                            </th>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr runat="server" id="MainContent" visible="false">
                <td>
                     <div class="col-md-2s hide" >
                            <label  id="lblStatmentDate">                                
                            <asp:Label ID="Label2" CssClass="mylabel1" runat="server" Text="Set All Statment Date :"  Width="400px"></asp:Label>
                            </label>
                            <div id="tdStatmentDate">
                                <dxe:ASPxDateEdit ID="dtdstatedateall" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy"  NullText="Date"
                                    ClientInstanceName="cDatestatment" UseMaskBehavior="True" Width="100%">
                                    <ButtonStyle Width="13px"></ButtonStyle>
                                    <ClientSideEvents ValueChanged="function(s, e){ StatmentDateChangeBehaviourTo();}"  />
                                </dxe:ASPxDateEdit>
                            </div>
                        </div>
                    <div  style="width: 100%"> <%-- onbeforecolumnsortinggrouping="grdmanualBRS_BeforeColumnSortingGrouping" OnBeforeHeaderFilterFillItems="grdmanualBRS_BeforeHeaderFilterFillItems" class="grid_scrollNSEFO"--%>
                        
                        <dxe:ASPxGridView ID="grdmanualBRS" runat="server" ClientInstanceName="cgrdmanualBRS" Width="100%" AutoGenerateColumns="False"
                             OnCustomCallback="grdmanualBRS_CustomCallback" SettingsBehavior-AllowDragDrop="false" SettingsBehavior-AllowSort="false"  Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="500">
                            <Settings ShowGroupPanel="false" ShowFilterRow="false" />
                            <SettingsSearchPanel Visible="false" />
                          <%--OnAfterPerformCallback="grdmanualBRS_AfterPerformCallback" <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />--%>
                            <SettingsPager PageSizeItemSettings-ShowAllItem="true" AlwaysShowPager="false" Mode="ShowAllRecords">
                           <PageSizeItemSettings ShowAllItem="True"></PageSizeItemSettings>
                            </SettingsPager>
                            <Columns>
                                <dxe:GridViewCommandColumn VisibleIndex="0" ShowClearFilterButton="True" width="0"></dxe:GridViewCommandColumn>
                           <%--     <dxe:GridViewDataTextColumn Caption="Trans Date" Visible="false" FieldName="cashbank_transactionDate_test">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Trans Date" Visible="false" FieldName="cashbankdetail_mainaccountid">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Trans Date" Visible="false" FieldName="Cashbank_ExchangeSegmentID">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Trans Date" Visible="false" FieldName="subaccount_code">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Trans Date" Visible="false" VisibleIndex="1" FieldName="cashbankdetail_id">
                                </dxe:GridViewDataTextColumn>--%>
                                <dxe:GridViewDataTextColumn Caption="Document Date" VisibleIndex="1" FieldName="cashbank_transactionDate">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Document No" VisibleIndex="2" FieldName="cashbank_vouchernumber">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Type" VisibleIndex="3" FieldName="Type">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Instrument Date" VisibleIndex="4" FieldName="cashbankdetail_instrumentdate">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Instrument No" VisibleIndex="5" FieldName="cashbankdetail_instrumentnumber">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Account Name" VisibleIndex="6" FieldName="AccountCode">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Payment Amt" VisibleIndex="7" FieldName="cashbankdetail_paymentamount">
                                      <PropertiesTextEdit DisplayFormatString="{0:0.00}" Width="100%">
                                                                <MaskSettings Mask="<0..999999999>.<0..99>" IncludeLiterals="DecimalSymbol" />
                                          </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Receipt Amt" VisibleIndex="8" FieldName="cashbankdetail_receiptamount">
                                      <PropertiesTextEdit DisplayFormatString="{0:0.00}" Width="100%">
                                                                <MaskSettings Mask="<0..999999999>.<0..99>" IncludeLiterals="DecimalSymbol" />
                                          </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataDateColumn FieldName="cashbankdetail_bankstatementdate" Width="14%" VisibleIndex="9"
                                    Caption="Statement Date (DD-MM-YYYY)">
                                    <Settings AllowAutoFilter="False" AllowGroup="False" />
                                    <DataItemTemplate>
                                       <%-- <dxe:ASPxTextBox ID="txt_cashbankdate" runat="server" Width="170px">
                                            <MaskSettings Mask="dd-MM-yyyy" IncludeLiterals="All"   />
                                            <ValidationSettings ErrorDisplayMode="None"></ValidationSettings>
                                        </dxe:ASPxTextBox>--%>
                                         <dxe:ASPxDateEdit ID="txt_cashbankdate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True" ClientInstanceName="ctxt_cashbankdate" AllowNull="true" DisplayFormatString="dd-MM-yyyy"  Date="<%#Setstatementdate(Container)%>" >
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                           <%--  <ClientSideEvents Init="Displaydate"/>--%>
                                        </dxe:ASPxDateEdit>
                                    </DataItemTemplate>
                                 <PropertiesDateEdit>
                                    
                                 </PropertiesDateEdit>
                                </dxe:GridViewDataDateColumn>
                                <dxe:GridViewDataDateColumn FieldName="cashbankdetail_bankvaluedate" Width="14%" VisibleIndex="10"
                                    Caption="Value Date (DD-MM-YYYY)">
                                    <Settings AllowAutoFilter="False" AllowGroup="False"  />
                                    
                                   <DataItemTemplate>
                                          <%--  <dxe:ASPxTextBox ID="txtvaluedate" runat="server" Width="170px" > <MaskSettings Mask="dd-MM-yyyy" IncludeLiterals="All"   />
                                              <ValidationSettings ErrorDisplayMode="None"></ValidationSettings>
                                                <ClientSideEvents Init="changeddate" />
                                            </dxe:ASPxTextBox>--%>
                                       <dxe:ASPxDateEdit ID="bankvaluedate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True" ClientInstanceName="cbankvaluedate" AllowNull="true" DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy"  Date="<%#Setbankvaluedate(Container)%>">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                           
                                        </dxe:ASPxDateEdit>
                                    </DataItemTemplate>
                                    <PropertiesDateEdit>
                                     

                                    </PropertiesDateEdit>
                                </dxe:GridViewDataDateColumn>
                             <%--   <dxe:GridViewDataTextColumn Caption="1"  FieldName="" VisibleIndex="12" Width="0%">
                                </dxe:GridViewDataTextColumn>--%>                                                               

                            </Columns>
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                            </SettingsPager>
                            
                        </dxe:ASPxGridView>
                                
                      <%--  Old Grid --%>
                        <asp:GridView ID="grdDetails" AutoGenerateColumns="false" runat="server" OnDataBound="grdDetails_DataBound" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="Document Date">
                                    <ItemTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTranDate" runat="server" Text='<%# Eval("cashbank_transactionDate") %>' /></td>
                                            </tr>
                                            <tr style="display: none">
                                                <td>
                                                    <asp:Label ID="lblTransactionDate" runat="server" Text='<%# Eval("cashbank_transactionDate_test") %>' />
                                                    <asp:Label ID="lblMainAcc" runat="server" Text='<%# Eval("cashbankdetail_mainaccountid") %>' />
                                                    <asp:Label ID="lblid" runat="server" Text='<%# Eval("cashbankdetail_id") %>' />
                                                    <asp:Label ID="lblSegID" runat="server" Text='<%# Eval("Cashbank_ExchangeSegmentID") %>' />
                                                    <asp:Label ID="lblSubCode" runat="server" Text='<%# Eval("subaccount_code") %>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="EHEADER" />
                                    <ItemStyle Width="8%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Document No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVoucherNo" runat="server" Text='<%# Eval("cashbank_vouchernumber") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="EHEADER" />
                                    <ItemStyle Width="8%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Instrument Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInsDate" runat="server" Text='<%# Eval("cashbankdetail_instrumentdate") %>' />

                                    </ItemTemplate>
                                    <HeaderStyle CssClass="EHEADER" />
                                    <ItemStyle Width="8%" />

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Instrument No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInsNo" runat="server" Text='<%# Eval("cashbankdetail_instrumentnumber") %>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" Width="8%" />
                                    <HeaderStyle CssClass="EHEADER" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Account Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAccName" runat="server" Text='<%# Eval("AccountCode") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="EHEADER" />
                                    <ItemStyle Width="30%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sub AccName" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubAcc" runat="server" Text='<%# Eval("subaccount_name") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="EHEADER" />
                                    <ItemStyle Width="0%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pymt Amt">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPmtAmt" runat="server" Text='<%# Eval("cashbankdetail_paymentamount") %>'
                                            CssClass="rt" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" Width="8%" />
                                    <HeaderStyle CssClass="EHEADER" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rcpt Amt">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRcptAmt" runat="server" Text='<%# Eval("cashbankdetail_receiptamount") %>'
                                            CssClass="rt" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" Width="8%" />
                                    <HeaderStyle CssClass="EHEADER" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Statement Date (DD-MM-YYYY)">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtStatementDate" Visible="true" runat="server" alt="date" Text='<%# Eval("cashbankdetail_bankstatementdate") %>'
                                            onblur="setvalueValuedate(this)" Width="90%" />
                                        <%--<input id="Text1" type="text" alt="date" />--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                    <HeaderStyle CssClass="EHEADER" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Value Date (DD-MM-YYYY)">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtValueDate" Visible="true" runat="server" alt="date" Text='<%# Eval("cashbankdetail_bankvaluedate") %>'
                                            onblur="setvalue(this)" Width="90%" />
                                        <%--<input id="Text1" type="text" alt="date" />--%>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="EHEADER" />
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <%--  Old Grid --%>
                    </div>
                </td>
            </tr>
            <tr>

                <td id="td_CancelUpdate"  text-align: center" colspan="4">&nbsp;<table border="0">
                    <tr>

                        <td >
                            <asp:Button ID="btnUpdate" runat="server" Text="Save" CssClass="btnUpdate btn btn-primary" OnClick="btnUpdate_Click" Width="" />

                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnUpdate btn btn-danger" OnClick="btnCancel_Click" Width="" /></td>
                    </tr>
                </table>
                </td>

            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hdn_CurrentSegment" runat="server" />

</asp:Content>
