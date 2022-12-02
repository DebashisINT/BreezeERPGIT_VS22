<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.DailyTask.management_DailyTask_frm_Account_Transfer" CodeBehind="frm_Account_Transfer.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web" TagPrefix="dxpc" %>--%>
    

    

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajaxList.js"></script>


    <style type="text/css">
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
            z-index: 100;
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
            z-index: 5;
        }

        form {
            display: inline;
        }
    </style>
    <script language="javascript" type="text/javascript">

        var exist = 0;
        var slipvalid = '';
        var slipdate = '';

        function SignOff() {
            window.parent.SignOff();
        }
        function PageLoad() {
            dtexec1.SetDate(new Date());
            dttran1.SetDate(new Date());
        }
        function height() {
            if (document.body.scrollHeight >= 600)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '600px';
            window.frameElement.Width = document.body.scrollWidth;
        }
        function showholding_pop() {
            var BenID = document.getElementById('hiddenbenid').value;

            BenID = BenID.substr(BenID.indexOf('[') + 1, 10);

            //alert(dpid);
            //        var url='frm_InstrunctionEntryShowHolding_popup.aspx?BenAccNo='+BenID;
            var url = 'frm_AccountTransfer_PopUp.aspx?BenAccNo=' + BenID;

            OnMoreInfoClick(url, "Free Holding", '940px', '450px', "Y");
        }
        function Signature_PopUpCall(obj) {
            //OnMoreInfoClick(obj,"Large Image",'940px','450px',"N");     
            //alert(document.getElementById('txtClient').value);         
         <%--alert('<%= dp %>');--%>
            //alert(obj);
            var BenAccNo = document.getElementById('hiddenbenid').value;
            BenAccNo = BenAccNo.split('[')[1].toString().split(']')[0];
            //alert(BenAccNo);
            if ('<%= dp %>' == 'CDSL') {
                      var url = 'view_signature.aspx?id=' + BenAccNo + '[CDSL]';
                      OnMoreInfoClick(url, "View Signature", '940px', '450px', "Y");
                  }
                  else {
                      var url = 'view_signature.aspx?id=' + BenAccNo + '[NSDL]';
                      OnMoreInfoClick(url, "View Signature", '940px', '450px', "Y");
                  }
              }
              function OnAddButtonClick1() {
                  var dptype = '<%= dp %>';
        if (dptype == 'CDSL') {
            var url = 'display_xml.aspx?id=' + "cdsl";
            OnMoreInfoClick(url, "Show Seetlement Details", '940px', '450px', "Y");
        }
        else {
            var url = 'display_xml.aspx?id=' + "nsdl";
            OnMoreInfoClick(url, "Show Seetlement Details", '940px', '450px', "Y");
        }
    }
    </script>

    <%-- displaysignature_Cod--%>

    <script language="javascript" type="text/javascript">
        //    function diffpage()
        //    {
        //     '<%=Session["transactiontype"] %>'='<%=Session["transactiontype"] %>'+'~'+'bind';
        //    }

        function PageLoad1() {

            FieldName = 'abc';
            //enabledisable();
            OnSelectionRefresh();
            OnSelectionMarketType();
            FormInitialView();
            //alert('pageload1');

        }
        function height() {
            if (document.body.scrollHeight >= 600)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '600px';
            window.frameElement.Width = document.body.scrollWidth;
        }

        FieldName = 'abc';
        function CallAjax(obj1, obj2, obj3) {
            document.getElementById('hiddensettlefrom').value = '';
            var trantype = document.getElementById("hdntrantype").value;
            var ddlmkt1 = ccmbmktfrom.GetValue();
            var ddlex1 = '';<%--'<%=sourceex %>';--%>
            //alert(ddlmkt1+'~'+ddlex1+'~'+'IN3');                                     
            ajax_showOptions(obj1, obj2, obj3, ddlmkt1 + '~' + ddlex1 + '~' + trantype, 'Main');
            //alert(document.getElementById('hiddensettlefrom').value);
        }



        function CallAjaxcmbpid(obj1, obj2, obj3) {
            var EarlyOrNormal = '';
            var radioButtons = document.getElementsByName("rbtnNrmEarlyPayin");
            if (radioButtons[0].checked == true && tdrbtnNrmEarlyPayin.style.display == 'inline') {
                EarlyOrNormal = 'N';
                document.getElementById('hdnrbtnNrmErlyvalue').value = 'N';
            }
            else if (radioButtons[1].checked == true && tdrbtnNrmEarlyPayin.style.display == 'inline') {
                EarlyOrNormal = 'E';
                document.getElementById('hdnrbtnNrmErlyvalue').value = 'E';
            }
            else if (radioButtons[2].checked == true && tdrbtnNrmEarlyPayin.style.display == 'inline') {
                EarlyOrNormal = 'ON';
                document.getElementById('hdnrbtnNrmErlyvalue').value = 'OnMkt.';
            }
            else {
                EarlyOrNormal = 'N';
                document.getElementById('hdnrbtnNrmErlyvalue').value = 'N';
            }
            //alert(EarlyOrNormal);
            var trantype = document.getElementById("hdntrantype").value;
            ajax_showOptions(obj1, obj2, obj3, trantype + '~' + EarlyOrNormal, 'Main');
        }
        function CallAjaxMarkettodata(obj1, obj2) {
            // alert('a4');                                                        
            ajax_showOptions(obj1, obj2);
        }

        function CallAjaxclient(obj1, obj2, obj3) {

            var Variable1 = document.getElementById('hiddendpid').value;
            var trantype = document.getElementById("hdntrantype").value;
            //alert(Variable1+'~'+document.getElementById('hiddendpid').value+'~'+document.getElementById('hiddenbenid').value);                                
            ajax_showOptions(obj1, obj2, obj3, 'IN3' + '~' + Variable1 + '~' + trantype + '~' + document.getElementById('hiddenbenid').value, 'Main');
        }
        function keyVal(obj) {
            //alert(obj);
            var trantype = document.getElementById("hdntrantype").value;
            var WhichCode = new String(obj);
            var status = new String(obj);
            var bplistcode = new String(obj);
            var CHID = new String(obj);
            var CMID = new String(obj);
            var otherddpid = new String(obj);
            var selecteddpid = new String(obj);
            var SettlementToValue = new String(obj);
            WhichCode = WhichCode.split('+')[3];
            //alert(WhichCode);
            if (WhichCode == 'cmbpid') {
                status = status.split('+')[2];
                bplistcode = bplistcode.split('+')[0];
                otherddpid = otherddpid.split('+')[1];
                CHID = CHID.split('+')[4];
                CMID = CMID.split('+')[5];
                //alert(status+bplistcode);
                document.getElementById('hdnbplistcode').value = bplistcode;
                document.getElementById('hdnotherddpid').value = otherddpid;
                //                        document.getElementById('boldStuff2').innerHTML = status;
                document.getElementById('Hiddendpidm').value = bplistcode;
                document.getElementById('hdnCHID').value = CHID;
                document.getElementById('hdnCMID').value = CMID;
                cmbmarketto.PerformCallback(bplistcode + '~KeyVal');
                if (bplistcode == "IN001002") {
                    cmbmarketto.SetText('NORMAL');
                }
                else if (bplistcode == "IN001019") {
                    cmbmarketto.SetText('Rolling Market Lot');
                }
                else if (bplistcode == "IN001027") {
                    cmbmarketto.SetText('ROLLING MKT LOT');
                }
                else if (bplistcode == "12" || bplistcode == "11" || bplistcode == "13") {
                    cmbmarketto.SetText('Rolling Normal');
                }
                else {
                    cmbmarketto.SetText('Normal');
                }
            }
            if (WhichCode == 'isin') {
                status = status.split('+')[2];
                if (status == 'Suspended for Debit') {
                    alert(status);
                }
                if (status == 'To be Closed') {
                    alert(status);
                }
                document.getElementById('Hiddenhold').value = obj.split('+')[0];
                CIcmbholding.PerformCallback();
            }
            if (WhichCode == 'dpid') {
                //alert(selecteddpid.split('+')[0]);
                document.getElementById('hiddendpid').value = selecteddpid.split('+')[0];
                var DpId = document.getElementById('hiddendpid').value;
                CallServer1(DpId + '~' + trantype, "");
            }
            if (WhichCode == 'ClientID') {
                //alert(Whichcode);
                //document.getElementById('txtclient1').onkeyup = onlyNumbers;
            }
            if (WhichCode == 'settlementfrom') {
                if (trantype == '5') {
                    cmbmarketto.PerformCallback(' ~InterSettlement');
                }

            }
            if (WhichCode == 'settlementto') {
                SettlementToValue = SettlementToValue.split('+')[0];
                //alert(SettlementToValue);
                document.getElementById('hiddensettleto').value = SettlementToValue;
            }


        }
        function OnSelectedValueChanged(obj) {
            var ddlvalue = obj;
            //var ddlslipvalue=SlipType.GetValue();

            document.getElementById("hdntrantype").value = ddlvalue;
            tdadd.style.display = 'inline'
            tdnew.style.display = 'none'
            ////            alert(ddlslipvalue);
            //           

            //            var ddltopic = document.getElementById("<%=ddltran.ClientID%>");
                  //            var ddltext = ddltran.options[ddltran.selectedIndex].text; 

                  //alert(ddltext);
                  //            document.getElementById('spnheader').innerText = ddltext;
                  if (ddlvalue == '0') //Select
                  {
                      OnSelectionSelect();
                  }
                  else if (ddlvalue == '1') //Markettype
                  {
                      OnSelectionRefresh();
                      OnSelectionMarketType();
                      OnChangeViewClick('h');
                      RefreshControl();


                  }
                  else if (ddlvalue == '2')//OffMarkettype
                  {
                      OnSelectionRefresh();
                      OnSelectionOffMarketType();
                      OnChangeViewClick('h');
                      // RefreshControl();
                  }
                  else if (ddlvalue == '3')//InterDP-Mrk
                  {
                      OnSelectionRefresh();
                      OnSelectionInterDp_Mkt();
                      OnChangeViewClick('h');
                      RefreshControl();
                  }
                  else if (ddlvalue == '4')//InterDP-OffMrk
                  {
                      OnSelectionRefresh();
                      OnSelectionInterDp_OffMkt();
                      OnChangeViewClick('h');
                      // RefreshControl();
                  }
                  else if (ddlvalue == '5')//InterSettlement
                  {
                      //RefreshControl();
                      //document.getElementById('TextBox3').value='';
                      document.getElementById('txtdpid').value = '';
                      document.getElementById('txtclient1').value = '';
                      document.getElementById('txtdpid').value = '';
                      //                document.getElementById('txtcmb').value='';
                      // document.getElementById('TextBox4').value='';
                      //                document.getElementById('txtisin').value='';
                      //                document.getElementById('txtqty').value='';
                      //                document.getElementById('boldStuff2').innerHTML = '';
                      //                document.getElementById('bholding').innerHTML = '';
                      OnSelectionRefresh();
                      OnSelectionInterSet();
                      OnChangeViewClick('h');



                  }
                  else if (ddlvalue == '6')//Delevery Out
                  {
                      //RefreshControl();
                      //                document.getElementById('TextBox3').value='';
                      document.getElementById('txtdpid').value = '';
                      document.getElementById('txtclient1').value = '';
                      document.getElementById('txtdpid').value = '';

                      OnSelectionRefresh();
                      OnSelectionDeliveryOut();
                      OnChangeViewClick('h');
                  }
                  else if (ddlvalue == '7')//Cm Pool To Pool
                  {
                      //RefreshControl();
                      //               document.getElementById('TextBox3').value='';
                      document.getElementById('txtdpid').value = '';
                      document.getElementById('txtclient1').value = '';
                      document.getElementById('txtdpid').value = '';

                      OnSelectionRefresh();
                      OnSelectionCMPoolToPool();
                      OnChangeViewClick('h');
                  }
                  else {
                      OnSelectionRefresh();
                      RefreshControl();
                  }

              }
              function OnSelectionRefresh() {

                  trDPidClientID.style.display = 'inline';

                  trinsttype.style.display = 'inline';

                  tdrbtnNrmEarlyPayin.style.display = 'none';


              }
              function OnSelectionMarketType() {

                  var Length = document.getElementById("ddltran").options.length;
                  //alert(Length)
                  if (Length == '7') {
                      trDPidClientID.style.display = 'none';
                      trinsttype.style.display = 'none';
                      var DivOnMkt = document.getElementById("divOnMkt");
                      DivOnMkt.style.display = 'none';
                  }
                  else {
                      trDPidClientID.style.display = 'none';
                      trinsttype.style.display = 'none';

                      if ('<%= dp %>' == 'CDSL') {
                            //tdrbtnNrmEarlyPayin.style.display='inline';
                            var DivOnMkt = document.getElementById("divOnMkt");
                            DivOnMkt.style.display = 'inline';
                        }
                        else {
                            var DivOnMkt = document.getElementById("divOnMkt");
                            DivOnMkt.style.display = 'none';
                        }
                    }

                }
                function OnSelectionOffMarketType() {
                    var Length = document.getElementById("ddltran").options.length;
                    //alert(Length)
                    if (Length == '7') {

                        trinsttype.style.display = 'none';
                    }
                    else {

                        trinsttype.style.display = 'none';

                    }
                }
                function OnSelectionInterDp_Mkt() {
                    var Length = document.getElementById("ddltran").options.length;
                    //alert(Length)
                    if ('<%= dp %>' == 'NSDL') {
                tdrbtnNrmEarlyPayin.style.display = 'inline';
                var DivOnMkt = document.getElementById("divOnMkt");
                DivOnMkt.style.display = 'none';
            }
            if (Length == '7') {
                trDPidClientID.style.display = 'none';
                trinsttype.style.display = 'none';
            }
            else {
                trDPidClientID.style.display = 'none';
                trinsttype.style.display = 'none';

            }
        }
        function OnSelectionInterDp_OffMkt() {
            var Length = document.getElementById("ddltran").options.length;
            //alert(Length)
            if (Length == '7') {

                trinsttype.style.display = 'none';
            }
            else {

                trinsttype.style.display = 'none';

            }
        }
        function OnSelectionInterSet() {
            trDPidClientID.style.display = 'none';
            trinsttype.style.display = 'none';


        }
        function OnSelectionDeliveryOut() {
            trDPidClientID.style.display = 'none';

        }

        function OnSelectionCMPoolToPool() {
            trDPidClientID.style.display = 'none';
            trinsttype.style.display = 'none';
            if ('<%= dp %>' == 'CDSL') {
            tdrbtnNrmEarlyPayin.style.display = 'inline';
            var DivOnMkt = document.getElementById("divOnMkt");
            DivOnMkt.style.display = 'none';
        }
        RefreshControl();
        //  cmbmarketto.PerformCallback(' ~RefreshControl');

    }
    function OnSelectionSelect() {
        EntryTableInitialView();
    }
    function EntryTableInitialView() {

        trDPidClientID.style.display = 'none';

        trinsttype.style.display = 'none';


    }
    function FormInitialView() {
        trshowDetail.style.display = 'none';
        tdsignaturepanel.style.display = 'none';
        tblentry.style.display = 'none';
        tblDetailsGrid.style.display = 'none';
        document.getElementById('txtAccountNo').focus();
    }
    function AfterShowClickView_IfSlipExist() {

        trshowDetail.style.display = 'inline';
        tdsignaturepanel.style.display = 'inline';
        trshowDetail.style.display = 'inline';
        tblentry.style.display = 'inline';
        tblDetailsGrid.style.display = 'inline';
        EntryTableInitialView();

    }
    function RefreshControl() {
        // document.getElementById('TextBox3').value='';
        document.getElementById('txtdpid').value = '';
        document.getElementById('txtclient1').value = '';
        document.getElementById('txtdpid').value = '';





    }
    function SpecificRefreshControl() //When Transaction is Market or InterDP(Mkt)
    {
        //         document.getElementById('txtisin').value='';
        //         document.getElementById('txtqty').value='';
        //         document.getElementById('boldStuff2').innerHTML = '';
        //         document.getElementById('bholding').innerHTML = '';   
    }
    function ShowTrTd(obj) {
        obj.style.display = 'inline';
    }
    function HideTrTd(obj) {
        obj.style.display = 'none';
    }
    function getdpid(s) {
        document.getElementById('hiddendpid').value = s;
    }
    function pageclose() {
        var x = window.confirm("Do you want to close the window?");
        if (x) {
            parent.editwin.close();
        }
        //            else
        //            {
        //             document.getElementById('txtisin').focus();
        //            }

    }

    function ShowHideShowDetail(obj) {
        if (obj == 'h') {
            trshowDetail.style.display = 'none';
            tdsignaturepanel.style.display = 'none';
            document.getElementById("imgarrowdown").style.display = 'inline';
            document.getElementById("imgarrowup").style.display = 'none';
        }
        if (obj == 's') {
            trshowDetail.style.display = 'inline';
            tdsignaturepanel.style.display = 'inline';
            document.getElementById("imgarrowdown").style.display = 'none';
            document.getElementById("imgarrowup").style.display = 'inline';

        }
    }
    function OnChangeViewClick(obj) {
        if (obj == 'h') {
            TABLE1.style.display = 'none';
            tblshortdetail.style.display = 'inline';
        }
        if (obj == 's') {
            TABLE1.style.display = 'inline';
            tblshortdetail.style.display = 'none';
            document.getElementById('ddltran').selectedIndex = 0;
            EntryTableInitialView();
        }
    }



    function ShowHolding(s) {
        if (s != null) {
            //        document.getElementById('bholding').innerHTML = s;
            document.getElementById('hiddenbholding').value = s;

        }
    }
    function AddButtonClick() {

        if (CheckDebit()) {
            var Length = document.getElementById("ddltran").options.length;
            var control = document.getElementById("ddltran");
            var value = control.options[control.selectedIndex].value;
            //alert(Length + control + value);



            if (value == '0') {
                ddlcheck('ddltran', 'transaction')
            }
            if (value == '1') {

                AfterAddButtonValidation();


            }
            if (value == '2') {
                if (textboxcheck('txtdpid', 'DPID'))
                    if (CheckLegthTextBox('txtdpid', 'DPID'))
                        if (textboxcheck('txtclient1', 'Client'))
                            if (CheckLegthTextBox('txtclient1', 'Client'))
                                AfterAddButtonValidation();

            }
            if (value == '3') {

                AfterAddButtonValidation();
            }
            if (value == '4') {
                if (textboxcheck('txtdpid', 'DPID'))
                    if (CheckLegthTextBox('txtdpid', 'DPID'))
                        if (textboxcheck('txtclient1', 'Client'))
                            if (CheckLegthTextBox('txtclient1', 'Client'))
                                AfterAddButtonValidation();
            }



        }

    }
    function AfterAddButtonValidation() {
        // btnadd.SetEnabled(false);
        tdadd.style.display = 'none'
        tdnew.style.display = 'inline'
        document.getElementById('btnnew').focus();
        //settlegrid.PerformCallback('ConfirmHighTran');
        settlegrid.PerformCallback('Add');
    }
    function combocheck(obj, value, objname) {
        if (value == 'null') {
            var str = 'Please Choose ' + objname;
            alert(str);
            obj.focus();
            return false;

        }
        else {
            return true;
        }

    }
    function ddlcheck(obj, objname) {
        var control = document.getElementById(obj);
        var value = control.options[control.selectedIndex].value;
        if (value == '0') {
            var str = 'Please Choose ' + objname;
            alert(str);
            document.getElementById(obj).focus();

        }
        else {

        }

    }
    function textboxcheck(obj, objname) {
        var control = document.getElementById(obj);
        //alert(control.value);
        if (control.value == '' || control.value == 'No Record Found') {
            var str = 'Please Enter ' + objname;
            alert(str);
            document.getElementById(obj).focus();
            return false;
        }
        else {
            return true;
        }
    }
    function CheckLegthTextBox(obj, objname) {
        var control = document.getElementById(obj);
        var textvalue = (control.value);
        //alert(textvalue);
        if ((textvalue.length < 8)) {
            var str = 'Please Enter Eight Digit Account No in ' + objname + ' ID';
            alert(str);
            document.getElementById(obj).focus();
            return false;
        }
        else {
            return true;
        }
    }



    function NewButtonClick() {
        var ddlvalue = document.getElementById("hdntrantype").value;
        if (ddlvalue == '1' || ddlvalue == '2') {
            SpecificRefreshControl();
            //        document.getElementById('txtisin').focus();
        }
        else if (ddlvalue == '3' || ddlvalue == '4') {
            if ('<%= dp %>' == 'NSDL') {
                 RefreshControl();
                 document.getElementById('ddltran').focus();
             }
             else {
                 SpecificRefreshControl();
                 //            document.getElementById('txtisin').focus();
             }
         }
         else {
             RefreshControl();
             document.getElementById('ddltran').focus();
         }
     tdadd.style.display = 'inline'
     tdnew.style.display = 'none'

 }
 function SaveButtonClick() {

     var Length = document.getElementById("ddltran").options.length;
     var control = document.getElementById("ddltran");
     var value = control.options[control.selectedIndex].value;
     //alert(Length + control + value);



     if (value == '0') {
         ddlcheck('ddltran', 'transaction')
     }

     if (value == '2') {
         if (textboxcheck('txtdpid', 'DPID'))
             if (CheckLegthTextBox('txtdpid', 'DPID'))
                 if (textboxcheck('txtclient1', 'Client'))
                     if (CheckLegthTextBox('txtclient1', 'Client'))
                         settlegrid.PerformCallback('Save');

     }

     if (value == '4') {
         if (textboxcheck('txtdpid', 'DPID'))
             if (CheckLegthTextBox('txtdpid', 'DPID'))
                 if (textboxcheck('txtclient1', 'Client'))
                     if (CheckLegthTextBox('txtclient1', 'Client'))
                         settlegrid.PerformCallback('Save');
     }


 }
 function AfterRecordSaveMsg(s) {
     //    alert(s);
     if (s != null) {
         alert(s);
     }


 }

 function CancelButtonClick() {
     RefreshControl();
     tdadd.style.display = 'inline'
     tdnew.style.display = 'none'
     document.getElementById('ddltran').focus();
     settlegrid.PerformCallback('Cancel');
 }
 function CancelAllButtonClick() {
     //alert(settlegrid.GetVisibleRowsOnPage());
     if (settlegrid.GetVisibleRowsOnPage() != '0') {
         RefreshControl();
         tdadd.style.display = 'inline'
         tdnew.style.display = 'none'
         document.getElementById('ddltran').focus();
         settlegrid.PerformCallback('CancelAll');
     }
     else {
         alert('There is No Record to Discard');
         document.getElementById('ddltran').focus();
     }
 }
 function onlyNumbers(evt) {
     var e = event || evt; // for trans-browser compatibility
     var charCode = e.which || e.keyCode;

     if (charCode > 31 && (charCode < 48 || charCode > 57))
         return false;

     return true;

 }
 function letternumber(e) {
     var key;
     var keychar;

     if (window.event)
         key = window.event.keyCode;
     else if (e)
         key = e.which;
     else
         return true;
     keychar = String.fromCharCode(key);
     keychar = keychar.toLowerCase();

     // control keys
     if ((key == null) || (key == 0) || (key == 8) ||
         (key == 9) || (key == 13) || (key == 27))
         return true;

         // alphas and numbers
     else if ((("abcdefghijklmnopqrstuvwxyz0123456789").indexOf(keychar) > -1))
         return true;
     else
         return false;
 }


 function blinkIt() {
     if (!document.all) return;
     else {
         for (i = 0; i < document.all.tags('blink').length; i++) {
             s = document.all.tags('blink')[i];
             s.style.visibility = (s.style.visibility == 'visible') ? 'hidden' : 'visible';
         }
     }
 }
 function OnExecDateValueChanged() {
     var currentTime = new Date();
     var date = dtexec1.GetDate();
     document.getElementById('lbl_sdedate').innerHTML = date.format("dd MMM yyyy");
     if (currentTime < date) {
     }
     else {
         dtexec1.SetDate(currentTime);
     }
 }
 function ReceiveSvrData(rValue) {
     var Data = rValue.split('~');
     var fld = document.getElementById("txtclient1")
     if (Data[0] == 'NoClients') {
         fld.detachEvent('onkeypress', letternumber);
         fld.attachEvent('onkeypress', onlyNumbers);

     }
     else {
         fld.detachEvent('onkeypress', onlyNumbers);
         fld.attachEvent('onkeypress', letternumber);

     }
 }

 function ShowDebitExist() {
     trDebitExist.style.display = 'inline';

 }
 function HidDebitExist() {
     trDebitExist.style.display = 'none';


 }

 function CheckDebit() {
     if (trDebitExist.style.display == 'inline') {

         var aa = document.getElementById('chkDebtExist');
         if (aa.checked != true) {
             alert('Select debit exists!!');
             return false;
         }
         else
             return true;

     }
     else {
         return true;
     }


 }

 function ShowMsg(msg) {

     alert(msg);

 }

 function CallAjaxdpid(obj1, obj2, obj3) {

     var ddlex1 = document.getElementById("hdnbplistcode").value;
     var trantype = document.getElementById("hdntrantype").value;
     var BenAccNo = document.getElementById('hiddenbenid').value;

     //alert(ddlmkt1+'~'+ddlex1+'~'+document.getElementById("txtdpid3").value);                                     
     ajax_showOptions(obj1, obj2, obj3, '0~' + ddlex1 + '~' + 'IN3' + '~' + trantype + '~' + BenAccNo, 'Main');
     //alert(document.getElementById('hiddensettlefrom').value);
 }

 function CallAjaxclient(obj1, obj2, obj3) {

     //var Variable1 = document.getElementById('hiddendpid').value;
     var Variable1 = '<%=Session["usersegid"] %>';
     var trantype = 1;
     //                  var trantype=document.getElementById("hdntrantype").value;
     //alert(Variable1+'~'+document.getElementById('hiddendpid').value+'~'+document.getElementById('hiddenbenid').value);                                
     ajax_showOptions(obj1, obj2, obj3, 'IN3' + '~' + Variable1 + '~' + trantype + '~[ 00000000 ]', 'Main');
 }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>

        <div style="width: 100%; font-weight: bold; text-align: center">
            Account Transfer
        </div>
        <table border="1" style="width: 96%" id="TABLE1">
            <tr>
                <td style="width: 51px"></td>
                <td style="width: 32px"></td>
                <td colspan="2" style="font-weight: bold; text-transform: uppercase"></td>
                <td style="width: 7px"></td>
                <td rowspan="1" style="width: 129px" valign="top">
                    <img id="imgarrowdown" src="../images/arrow_down.gif" alt="Show Detail" onclick="ShowHideShowDetail('s')"
                        style="cursor: pointer; display: none" />
                    <img id="imgarrowup" src="../images/arrow_up.gif" alt="Hide Detail" onclick="ShowHideShowDetail('h')"
                        style="cursor: pointer" />
                </td>
            </tr>
            <tr>
                <td valign="top" style="width: 66px; height: 30px">Account No :</td>
                <td valign="top" style="width: 190px">
                    <asp:TextBox ID="txtAccountNo" Width="180px" runat="Server"></asp:TextBox>
                    <br />
                    <asp:RequiredFieldValidator ID="reqAccountNo" runat="server" ControlToValidate="txtAccountNo" ErrorMessage="Enter account no" Display="dynamic" SetFocusOnError="true" ValidationGroup="a"></asp:RequiredFieldValidator>
                </td>
                <td valign="top" style="width: 76px">
                    <dxe:ASPxButton ID="btnSave" runat="server" AutoPostBack="false" Text="Show" ValidationGroup="a"
                        OnClick="btnSave_Click">
                    </dxe:ASPxButton>
                </td>
                <td valign="top" style="width: 148px"></td>
                <td valign="top"></td>
                <td></td>
            </tr>
            <tr>
                <td valign="top"></td>
                <td valign="top"></td>
                <td valign="top"></td>
                <td></td>
                <td></td>
                <td id="tdsignaturepanel" style="display: none" rowspan="6" valign="top">
                    <asp:Panel ID="Panel1" runat="server" Height="179px" ScrollBars="Auto" Width="423px">
                        <dxe:ASPxGridView ID="gridSign" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
                            KeyFieldName="doc_source" Width="100%">
                            <SettingsPager Visible="False">
                            </SettingsPager>
                            <Border BorderColor="#DDECFE"></Border>
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Signature" VisibleIndex="0">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" onclick="Signature_PopUpCall('<%# Container.KeyValue %>')"
                                            tabindex="-1">
                                            <asp:Image ID="Image1" runat="server" TabIndex="-1" ImageUrl='<%# Eval("doc_source") %>' />
                                        </a>
                                    </DataItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                        </dxe:ASPxGridView>
                    </asp:Panel>
                </td>
            </tr>

            <tr id="trshowDetail">
                <td colspan="5" rowspan="5" valign="top">
                    <table style="height: 120px" border="1">
                        <tr>
                            <td style="width: 100px">Client ID</td>
                            <td colspan="2">
                                <asp:Label ID="txtClient" runat="server" Text="Label" Width="142%"></asp:Label></td>
                            <td style="width: 100px"></td>
                        </tr>
                        <tr>
                            <td style="width: 100px">Second Holder Name</td>
                            <td colspan="2">
                                <asp:Label ID="lblSecondHolderName" runat="server" Text="None" Width="142%" ForeColor="Blue"></asp:Label></td>
                            <td style="width: 100px"></td>
                        </tr>
                        <tr>
                            <td style="width: 100px">Third Holder Name</td>
                            <td colspan="2">
                                <asp:Label ID="lblThirdHolderName" runat="server" Text="None" Width="142%" ForeColor="Red"></asp:Label></td>
                            <td style="width: 100px"></td>
                        </tr>
                        <tr>
                            <td style="width: 100px; height: 16px;">Last Trans Date</td>
                            <td colspan="3" style="height: 16px">
                                <asp:Label ID="txtTransDate" runat="server" Text="Label" Width="20%"></asp:Label>
                                <blink><b id="lbldormantStatus" runat="server" style="width:78%;font-weight:bold;color:Red;"></b></blink>
                                <%--<asp:Label ID="lbldormantStatus" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="Larger" Visible="False" Font-Underline="True" Width="78%"></asp:Label>--%>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px">Ledger Balance</td>
                            <td colspan="3">
                                <asp:Label ID="txtBal" runat="server" Text="Label" Width="100%"></asp:Label></td>
                        </tr>
                        <tr id="trDebitExist">
                            <td></td>
                            <td colspan="3">
                                <asp:CheckBox ID="chkDebtExist" runat="server" TextAlign="Right" />Ignor Debit
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px; height: 14px;">
                                <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Show Free Holding"
                                    ValidationGroup="a" Width="121px" Height="3px" ToolTip="Click to See Holding Detail">
                                    <ClientSideEvents Click="function(s, e) {showholding_pop();}"></ClientSideEvents>
                                </dxe:ASPxButton>
                            </td>
                            <td style="width: 100px; height: 14px;" valign="top"></td>
                            <td style="width: 100px; height: 14px;" valign="top"></td>
                            <td style="width: 100px; height: 14px;"></td>
                        </tr>
                        <tr>
                            <td style="width: 100px">
                                <dxe:ASPxDateEdit ID="dttran" runat="server" ClientInstanceName="dttran1" DateOnError="Today"
                                    EditFormat="Custom" EditFormatString="dd-MM-yyyy" ReadOnly="True" TabIndex="-1"
                                    Width="140px">
                                    <DropDownButton Text="Trans.Date">
                                    </DropDownButton>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td style="width: 100px">
                                <dxe:ASPxDateEdit ID="dtexec" runat="server" ClientInstanceName="dtexec1" DateOnError="Today"
                                    EditFormat="Custom" EditFormatString="dd-MM-yyyy" OnDateChanged="dtexec_DateChanged"
                                    TabIndex="0" UseMaskBehavior="True" Width="140px">
                                    <DropDownButton Text="Exec.Date">
                                    </DropDownButton>
                                    <ClientSideEvents ValueChanged="OnExecDateValueChanged" />
                                </dxe:ASPxDateEdit>
                            </td>
                            <td style="width: 100px"></td>
                            <td style="width: 100px"></td>
                        </tr>
                    </table>
                    <a href="javascript:void(0);" onclick="OnAddButtonClick1( )"><span style="color: #000099; text-decoration: underline">Show unsavedrecords</span> </a>
                </td>
            </tr>
            <tr>
            </tr>
            <tr>
            </tr>
            <tr>
            </tr>
            <tr>
            </tr>
        </table>
        <table id="tblshortdetail" border="1" style="width: 912px; display: none">
            <tr>
                <td style="width: 168px; height: 18px;">Client ID</td>
                <td style="width: 186px; height: 18px">
                    <asp:Label ID="lbl_sdclientid" runat="server" Text="Label" Font-Bold="True" Width="250px"></asp:Label></td>
                <td style="width: 129px; height: 18px;"></td>
                <td style="width: 412px; height: 18px;"></td>
                <td style="width: 221px; height: 18px;"></td>
                <td style="width: 100px; height: 18px;"></td>
                <td style="width: 112px; height: 18px">
                    <a href="javascript:void(0);" onclick="OnChangeViewClick('s')"><span style="color: #000099; text-decoration: underline">ChangeView</span> </a>
                </td>
            </tr>
            <tr>
                <td style="width: 168px; height: 16px">Trans. Date</td>
                <td style="width: 186px; height: 16px">
                    <asp:Label ID="lbl_sdtdate" runat="server" Text="Label" Font-Bold="True"></asp:Label></td>
                <td style="width: 129px; height: 16px">Exec. Date</td>
                <td style="width: 412px; height: 16px">
                    <b id="lbl_sdedate" runat="server"></b>
                </td>
                <td style="width: 221px; height: 16px"></td>
                <td style="width: 100px; height: 16px"></td>
                <td style="width: 112px; height: 16px"></td>
            </tr>
        </table>

    </div>

    <table id="tblentry" border="1" style="width: 838px">
        <tr>
            <td style="width: 186px; height: 18px;" valign="top"></td>
            <td style="width: 309px; height: 18px;" valign="top"></td>
            <td style="width: 151px; height: 18px;" valign="top"></td>
            <td style="width: 309px; height: 18px;" valign="top"></td>
        </tr>
        <tr>
            <td style="width: 186px; height: 1px;" valign="top">Trans. Type</td>
            <td style="width: 309px;" valign="top">
                <asp:DropDownList ID="ddltran" runat="server" Width="166px" onchange="OnSelectedValueChanged(this.value);">
                </asp:DropDownList></td>
            <td id="tdrbtnNrmEarlyPayin" colspan="2" style="height: 1px; display: none" valign="top">
                <div style="float: left">
                    <input type="radio" name="rbtnNrmEarlyPayin" value="1" checked="checked" />Normal
                        <input type="radio" name="rbtnNrmEarlyPayin" value="2" />Early-Payin
                </div>
                <div id="divOnMkt">
                    <input type="radio" name="rbtnNrmEarlyPayin" value="3" />On-Market
                </div>
            </td>
        </tr>
        <tr>
            <td style="width: 186px" valign="top"></td>
            <td style="width: 309px" valign="top"></td>
            <td style="width: 151px" valign="top"></td>
            <td valign="top"></td>
        </tr>

        <tr id="trDPidClientID">
            <td style="width: 186px" valign="top">Dp ID
            </td>
            <td style="width: 309px" valign="top">
                <asp:TextBox ID="txtdpid" runat="server" MaxLength="8"
                    Width="300px"></asp:TextBox></td>
            <td style="width: 151px" valign="top">Client ID</td>
            <td style="width: 309px" valign="top">
                <asp:TextBox ID="txtclient1" runat="server" Width="300px" MaxLength="8"></asp:TextBox></td>
        </tr>


        <tr id="trinsttype">
            <td valign="top">Instruction type</td>
            <td valign="top">
                <asp:RadioButtonList ID="rdbtninsttype" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="906" Selected="True">Reversible</asp:ListItem>
                    <asp:ListItem Value="912">Irreversible</asp:ListItem>
                </asp:RadioButtonList></td>
            <td valign="top"></td>
            <td valign="top"></td>
        </tr>
        <tr>
            <td valign="top" colspan="4">
                <br />
                <table style="width: 819px">
                    <tr>
                        <td id="tdadd" style="width: 100px; height: 16px">
                            <dxe:ASPxButton ID="btnadd" runat="server" ClientInstanceName="btnadd" AutoPostBack="false" TabIndex="0" Text="Add Entry To [L]ist"
                                AccessKey="L" Width="165px" ToolTip="Add Current Entry( Above in  Form) into List Below.[Alt+L]">
                                <ClientSideEvents Click="function(s, e) {AddButtonClick();}" />
                            </dxe:ASPxButton>
                        </td>
                        <td id="tdnew" style="width: 100px; height: 16px; display: none">
                            <dxe:ASPxButton ID="btnnew" runat="server" AutoPostBack="false" TabIndex="0" Text="[N]ew Entry"
                                Width="150px" AccessKey="N" Font-Bold="False" Font-Underline="False" BackColor="Tan">
                                <ClientSideEvents Click="function(s, e) {NewButtonClick();}" />
                            </dxe:ASPxButton>
                        </td>
                        <td style="width: 100px; height: 16px">
                            <dxe:ASPxButton ID="btncancel" runat="server" AutoPostBack="false" TabIndex="0" Text="[C]ancel Entry"
                                AccessKey="C" Width="165px" ToolTip="Cancel & Reset Current Entry in Above Form.  [Alt+C]">
                                <ClientSideEvents Click="function(s, e) {CancelButtonClick();}" />
                            </dxe:ASPxButton>
                        </td>
                        <td style="width: 100px; height: 16px">
                            <dxe:ASPxButton ID="btnSaveRecords" runat="server" AutoPostBack="false" TabIndex="0"
                                Text="[S]ave Entered Records" AccessKey="S" Width="165px" ToolTip="Save All Records(if Exists) in List Below. [Alt+S]">
                                <ClientSideEvents Click="function(s, e) {SaveButtonClick();}" />
                            </dxe:ASPxButton>
                        </td>
                        <td style="width: 100px; height: 16px"></td>
                    </tr>
                </table>
                &nbsp; &nbsp;&nbsp;
            </td>
            <td valign="top"></td>
        </tr>
        <tr>
        </tr>
    </table>
    <table id="tblDetailsGrid" width="90%">
        <tr>
            <td colspan="6">
                <dxe:ASPxGridView ID="DetailsGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="settlegrid"
                    KeyFieldName="ID" OnCustomCallback="DetailsGrid_CustomCallback" OnRowDeleting="DetailsGrid_RowDeleting"
                    Width="100%" OnCustomJSProperties="DetailsGrid_CustomJSProperties1">
                    <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" AllowGroup="False"
                        AllowSort="False"></SettingsBehavior>
                    <Styles>
                        <Header SortingImageSpacing="5px" ImageSpacing="5px">
                        </Header>
                        <FocusedGroupRow CssClass="gridselectrow">
                        </FocusedGroupRow>
                        <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow">
                        </FocusedRow>
                        <Footer CssClass="gridfooter">
                        </Footer>
                        <LoadingPanel ImageSpacing="10px">
                        </LoadingPanel>
                    </Styles>
                    <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" PageSize="10" ShowSeparators="True">
                        <FirstPageButton Visible="True">
                        </FirstPageButton>
                        <LastPageButton Visible="True">
                        </LastPageButton>
                    </SettingsPager>
                    <ClientSideEvents EndCallback="function(s, e) {AfterRecordSaveMsg(s.cpsavemsg);}"></ClientSideEvents>
                    <Columns>
                        <dxe:GridViewDataTextColumn FieldName="ID" Caption="Sr.No" Visible="False" VisibleIndex="0">
                            <CellStyle Wrap="False" CssClass="gridcellleft">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="TranType" Caption="TransType"
                            VisibleIndex="1">
                            <CellStyle Wrap="False" CssClass="gridcellleft">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>


                        <dxe:GridViewDataTextColumn FieldName="Isin" Caption="ISIN" VisibleIndex="2">
                            <CellStyle Wrap="False" CssClass="gridcellleft">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Qty" VisibleIndex="3">
                            <CellStyle Wrap="False" CssClass="gridcellleft">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>

                    </Columns>
                </dxe:ASPxGridView>
            </td>
        </tr>
        <tr>
            <td style="display: none" colspan="6">
                <dxe:ASPxComboBox ID="cmbholding" runat="server" EnableIncrementalFiltering="True"
                    Font-Bold="False" Font-Size="12px" ValueType="System.String" ClientInstanceName="CIcmbholding"
                    Width="5px" OnCallback="cmbholding_Callback" OnCustomJSProperties="cmbholding_CustomJSProperties">
                    <ClientSideEvents EndCallback="function(s, e) {ShowHolding(s.cpretValue);}"></ClientSideEvents>
                </dxe:ASPxComboBox>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hdnbplistcode" runat="server" />
    <asp:HiddenField ID="hdnotherddpid" runat="server" />
    <asp:HiddenField ID="hdntrantype" runat="server" />
    <asp:HiddenField ID="hiddensettleto" runat="server" />
    <asp:HiddenField ID="Hiddenhold" runat="server" />
    <asp:HiddenField ID="Hiddendpidm" runat="server" />
    <asp:HiddenField ID="hiddendpid" runat="server" />
    <asp:HiddenField ID="hiddensettlefrom" runat="server" />
    <asp:HiddenField ID="hiddenbenid" runat="server" />
    <asp:HiddenField ID="hiddenbholding" runat="server" />
    <asp:HiddenField ID="hdnrbtnNrmErlyvalue" runat="server" />
    <asp:HiddenField ID="hdnCHID" runat="server" />
    <asp:HiddenField ID="hdnCMID" runat="server" />
</asp:Content>
