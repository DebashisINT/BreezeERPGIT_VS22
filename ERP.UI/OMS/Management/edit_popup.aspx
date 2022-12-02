<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.management_edit_popup" CodeBehind="edit_popup.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>--%>

<tent id="Content1" contentplaceholderid="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <%--<script type="text/javascript" src="/assests/js/ajaxList.js"></script>--%>

    <script type="text/javascript" src="/assests/js/ajaxList_wofolder.js"></script>

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
        //    function diffpage()
        //    {
        //     '<%=Session["transactiontype"] %>'='<%=Session["transactiontype"] %>'+'~'+'bind';
        //    }
        function SignOff() {
            window.parent.SignOff();
        }
        function PageLoad() {
            FieldName = 'dttran';
            var exdate1 = new Date('<%= exdate %>');

            if (document.getElementById('Hiddendateex').value != '1') {
                //alert(document.getElementById('Hiddendateex').value);
                dtexec1.SetDate(exdate1);
            }
            enabledisable();
        }
        var flag = 1;
        function enabledisable() {

            var trantype = '<%= tran %>';
       //document.getElementById('Button1').click();  

       if (flag == 1) {
           document.getElementById('Button1').click();
           flag = 2;
       }
       document.getElementById('hh').style.visibility = 'hidden';
       if ((trantype == 10) || (trantype == 11)) {
           //             document.getElementById('re').style.visibility='visible';
           //             document.getElementById('re1').style.visibility='visible';
           ////             document.getElementById('TextBox3').style.backgroundColor='#E0E0E0';           
           ////             document.getElementById('TextBox3').disabled=true;
           ////             document.getElementById('ddlmkt').style.backgroundColor='#E0E0E0';         
           ////             document.getElementById('ddlmkt').disabled=true; 
           //             document.getElementById('txtcmb').style.backgroundColor='#E0E0E0';
           //             document.getElementById('txtcmb').disabled=true; 
           //             document.getElementById('TextBox1').style.backgroundColor='#E0E0E0';
           //             document.getElementById('TextBox1').disabled=true; 
           //             document.getElementById('txtholding').style.backgroundColor='#E0E0E0';

           document.getElementById('re').style.visibility = 'visible';
           document.getElementById('re1').style.visibility = 'visible';
           document.getElementById('TextBox3').style.backgroundColor = '#E0E0E0';
           document.getElementById('TextBox3').disabled = true;
           document.getElementById('ddlmkt').style.backgroundColor = '#E0E0E0';
           document.getElementById('ddlmkt').disabled = true;
           document.getElementById('txtcmb').style.backgroundColor = '#E0E0E0';
           document.getElementById('txtcmb').disabled = true;
           document.getElementById('TextBox1').style.backgroundColor = '#E0E0E0';
           document.getElementById('TextBox1').disabled = true;
           document.getElementById('txtholding').style.backgroundColor = '#E0E0E0';
           document.getElementById('txtclient').style.backgroundColor = '#E0E0E0';
           document.getElementById('txtclient').disabled = true;
           document.getElementById('txtdpid').style.backgroundColor = '#E0E0E0';
           document.getElementById('txtdpid').disabled = true;
       }
       else if ((trantype != 10) || (trantype != 11)) {
           document.getElementById('re').style.visibility = 'hidden';
           document.getElementById('re1').style.visibility = 'hidden';
       }

       if (document.getElementById('txtcmb').value != '' && trantype == 4) {
           document.getElementById('txtdpid').style.backgroundColor = '#E0E0E0';
           document.getElementById('txtdpid').disabled = true;
           document.getElementById('txtclient').style.backgroundColor = '#E0E0E0';
           document.getElementById('txtclient').disabled = true;
           document.getElementById('ddlmkt').style.visibility = 'visible';
       }
       if (document.getElementById('txtdpid').value != '' && trantype == 4) {
           document.getElementById('TextBox4').value = '';
           //                        document.getElementById('ddlexchange').options[0].selected = true;
           //                        document.getElementById('ddlmktto').options[0].selected = true;
           document.getElementById('ddlexchange').style.backgroundColor = '#E0E0E0';
           document.getElementById('ddlexchange').disabled = true;
           document.getElementById('ddlmktto').style.backgroundColor = '#E0E0E0';
           document.getElementById('ddlmktto').disabled = true;
           document.getElementById('txtcmb').style.backgroundColor = '#E0E0E0';
           document.getElementById('txtcmb').disabled = true;
           document.getElementById('TextBox4').style.backgroundColor = '#E0E0E0';
           document.getElementById('TextBox4').disabled = true;
           document.getElementById('TextBox1').style.backgroundColor = '#E0E0E0';
           document.getElementById('TextBox1').disabled = true;

       }
       if (trantype == 1) {

           //           document.getElementById('TextBox3').style.backgroundColor='#E0E0E0';
           //           
           //           document.getElementById('TextBox3').disabled=true;
           //          
           //           document.getElementById('ddlmkt').style.backgroundColor='#E0E0E0';
           //         
           //           document.getElementById('ddlmkt').disabled=true; 

           document.getElementById('txtdpid').style.backgroundColor = '#E0E0E0';
           document.getElementById('txtdpid').disabled = true;
           document.getElementById('txtclient').style.backgroundColor = '#E0E0E0';
           document.getElementById('txtclient').disabled = true;

           //           document.getElementById('txtcmb').style.backgroundColor='#E0E0E0';
           //         
           //           document.getElementById('txtcmb').disabled=true; 

           document.getElementById('TextBox1').style.backgroundColor = '#E0E0E0';

           document.getElementById('TextBox1').disabled = true;

       }
       if (trantype == 2) {
           document.getElementById('txtcmb').style.backgroundColor = '#E0E0E0';
           document.getElementById('txtcmb').disabled = true;
           document.getElementById('ddlexchange').style.backgroundColor = '#E0E0E0';
           document.getElementById('ddlexchange').disabled = true;
           //           document.getElementById('ddlmkt').style.backgroundColor='#E0E0E0';
           //           document.getElementById('ddlmkt').disabled=true;     
           document.getElementById('ddlmktto').style.backgroundColor = '#E0E0E0';
           document.getElementById('ddlmktto').disabled = true;
           document.getElementById('TextBox1').style.backgroundColor = '#E0E0E0';
           document.getElementById('TextBox1').disabled = true;
           //           document.getElementById('TextBox3').style.backgroundColor='#E0E0E0';
           //           document.getElementById('TextBox3').disabled=true;
           document.getElementById('TextBox4').style.backgroundColor = '#E0E0E0';
           document.getElementById('TextBox4').disabled = true;
       }
       if (trantype == 3) {
           //           document.getElementById('TextBox3').style.backgroundColor='#E0E0E0';
           //           document.getElementById('TextBox3').disabled=true;
           document.getElementById('txtcmb').style.backgroundColor = '#E0E0E0';
           document.getElementById('txtcmb').disabled = true;
           //           document.getElementById('ddlmkt').style.backgroundColor='#E0E0E0';
           //           document.getElementById('ddlmkt').disabled=true; 
       }
       if (trantype == 4) {
           //           document.getElementById('TextBox3').style.backgroundColor='#E0E0E0';
           //           document.getElementById('TextBox3').disabled=true;
           //           document.getElementById('ddlmkt').style.backgroundColor='#E0E0E0';
           //           document.getElementById('ddlmkt').disabled=true; 
           document.getElementById('txtdpid3').style.visibility = 'hidden';
           document.getElementById('txtdpid3').value = '';
           document.getElementById('TextBox1').style.backgroundColor = '#E0E0E0';
           document.getElementById('TextBox1').disabled = true;
       }
       if (trantype == 5) {
           document.getElementById('txtcmb').style.backgroundColor = '#E0E0E0';
           document.getElementById('txtcmb').disabled = true;
           document.getElementById('TextBox1').style.backgroundColor = '#E0E0E0';
           document.getElementById('TextBox1').disabled = true;
           document.getElementById('txtdpid').style.backgroundColor = '#E0E0E0';
           document.getElementById('txtdpid').disabled = true;
           document.getElementById('txtclient').style.backgroundColor = '#E0E0E0';
           document.getElementById('txtclient').disabled = true;
       }
       if (trantype == 6) {
           document.getElementById('txtcmb').style.backgroundColor = '#E0E0E0';
           document.getElementById('txtcmb').disabled = true;
           document.getElementById('ddlmkt').style.backgroundColor = '#E0E0E0';
           document.getElementById('ddlmkt').disabled = true;

           document.getElementById('TextBox3').style.backgroundColor = '#E0E0E0';
           document.getElementById('TextBox3').disabled = true;
           document.getElementById('txtdpid').style.backgroundColor = '#E0E0E0';
           document.getElementById('txtdpid').disabled = true;
           document.getElementById('txtclient').style.backgroundColor = '#E0E0E0';
           document.getElementById('txtclient').disabled = true;

       }

       var con = new String(document.getElementById('Hiddenid').value);
       if (con.match(',8,') == null) {

           document.getElementById('txtqty').disabled = true;
           document.getElementById('txtqty').style.backgroundColor = '#E0E0E0';
       }
       //alert(con.match('9'));
       if (con.match(',9,') == null) {

           document.getElementById('txtdpid').style.backgroundColor = '#E0E0E0';
           document.getElementById('txtdpid').disabled = true;
           document.getElementById('txtclient').style.backgroundColor = '#E0E0E0';
           document.getElementById('txtclient').disabled = true;
           document.getElementById('txtcmb').style.backgroundColor = '#E0E0E0';
           document.getElementById('txtcmb').disabled = true;
       }
       if (con.match(',4,') == null) {

           document.getElementById('txtisin').style.backgroundColor = '#E0E0E0';
           document.getElementById('txtisin').disabled = true;

       }
       if (con.match(',13,') == null) {

           dtexec1.SetEnabled(false);
           //dtexec1.BackColor='#E0E0E0';
       }
   }

   function height() {
       if (document.body.scrollHeight >= 600)
           window.frameElement.height = document.body.scrollHeight;
       else
           window.frameElement.height = '600px';
       window.frameElement.Width = document.body.scrollWidth;
   }


   function DateChangeForFrom(s) {
       var currentTime = new Date();
       if (currentTime < s.GetValue()) {

       }
       else {
           s.SetDate(currentTime);

       }

   }
   function CallAjax(obj1, obj2, obj3) {
       var e = document.getElementById("ddlmkt");
       var ddlmkt1 = '';
       if (document.getElementById('ddlmkt').disabled == false) {
           ddlmkt1 = e.options[e.selectedIndex].value;
       }

       var ee = document.getElementById("ddlexchange");
       var ddlex1 = '<%=sourceex %>';//ee.options[ee.selectedIndex].value; 
           //alert(ddlmkt1+'~'+ddlex1+'~'+document.getElementById("txtdpid3").value);                                     
           ajax_showOptions(obj1, obj2, obj3, ddlmkt1 + '~' + ddlex1 + '~' + document.getElementById("txtdpid3").value, 'Main');
       }
       function CallAjaxto(obj1, obj2, obj3) {
           var e = document.getElementById("ddlmktto");
           var ddlmkt1 = e.options[e.selectedIndex].text;
           var ee = document.getElementById("ddlexchange");
           var ddlex1 = ee.options[ee.selectedIndex].value;
           ajax_showOptions(obj1, obj2, obj3, ddlmkt1 + '~' + ddlex1 + '~' + e.options[e.selectedIndex].value, 'Main');
       }
       function CallAjaxcmbpid(obj1, obj2, obj3) {
           // alert('a4');                                                        
           ajax_showOptions(obj1, obj2, obj3, 'nsdl', 'Main');
       }
       function CallAjaxclient(obj1, obj2, obj3) {

           var Variable1 = '<%= ServerVaraible1 %>';
                    // alert(Variable1+'~'+document.getElementById('hiddendpid').value);                                
                    ajax_showOptions(obj1, obj2, obj3, Variable1 + '~' + document.getElementById('hiddendpid').value, 'Main');
                }
                function keyVal(obj) {
                    //alert(obj);
                    var status = new String(obj);
                    status = status.split('~')[1];
                    //alert(status);
                    if (status == 'Suspended for Debit') {
                        alert(status);
                    }
                    if (status == 'To be Closed') {
                        alert(status);
                    }
                    // document.getElementById('hiddendpid').value='';
                    var a = new String(obj);

                    for (var i = 0; i < a.length; i++) {
                        if (a.substr(i, 1) == '~') {
                            // textBox.SetValue(obj.split('~')[0]);   
                            document.getElementById('Hiddenhold').value = obj.split('~')[0];
                            document.getElementById('Button1').click();
                        }
                        else if (a.substr(i, 1) == '!') {
                            //alert(obj);
                            document.getElementById('hiddendpid').value = a.split('!')[0];
                            if (('<%= tran %>' == 10) || ('<%= tran %>' == 11)) {
                                  document.getElementById('hiddenex').value = a.split('!')[0];
                                  document.getElementById('Hiddenhold').value = ' ';
                                  document.getElementById('Button1').click();
                              }
                          }
                      if (a.substr(i, 1) == 't') {
                          //alert(a.substr(0,a.length-1));
                          document.getElementById('hiddensettleto').value = a.substr(0, a.length - 1);
                      }
                      if (a.substr(i, 1) == 'f') {
                          document.getElementById('hiddensettlefrom').value = a.substr(0, a.length - 1);
                          document.getElementById('Button1').click();
                      }
                      if (a.substr(i, 1) == '$') {
                          //alert(a);
                          document.getElementById('hiddenex').value = a.split('$')[1];
                          document.getElementById('Hiddenhold').value = ' ';
                          //alert(document.getElementById('hiddenex').value); 
                          document.getElementById('Button1').click();
                      }
                      if (a.substr(i, 1) == '+') {

                          if (a.charAt(a.length - 1) == '+') {
                              document.getElementById('Hiddendpidm').value = obj.split('+')[1];
                          }

                          document.getElementById('hiddenex').value = obj.split('+')[0];
                          document.getElementById('Hiddenhold').value = ' ';
                          //alert(document.getElementById('hiddenex').value);
                          document.getElementById('Button1').click();
                      }
                  }
                      //alert(document.getElementById('hiddenex').value);
                      //                        alert(document.getElementById('hiddensettlefrom').value);      


              }

              function caps() {
                  var caps = new String(document.getElementById('txtcmb').value);
                  if (caps == 'null') {

                  }
                  else {
                      document.getElementById('txtcmb').value = caps.toUpperCase();
                      showdrop('txtcmb');
                  }


              }
              function hidedrop(obj) {

                  if (obj == 'txtdpid') {
                      document.getElementById('ddlmkt').style.visibility = 'hidden';

                  }
                  if (obj == 'txtcmb') {
                      document.getElementById('ddlmkt').style.visibility = 'hidden';

                  }
                  if (obj == 'txtclient') {

                      document.getElementById('ddlmktto').style.visibility = 'hidden';
                      document.getElementById('ddlexchange').style.visibility = 'hidden';

                  }
                  if (obj == 'txtisin') {

                      document.getElementById('ddlmkt').style.visibility = 'hidden';

                  }
                  if (obj == 'txtisin') {

                      document.getElementById('ddlmkt').style.visibility = 'hidden';

                  }
              }
              function showdrop(obj) {
                  var trantype = '<%= tran %>';
              if (obj == 'txtdpid') {
                  var dpid1 = new String(document.getElementById('txtdpid').value);
                  document.getElementById('ddlmkt').style.visibility = 'visible';
                  if (document.getElementById('txtdpid').value != '' && trantype == 4) {
                      document.getElementById('TextBox4').value = '';
                      //                        document.getElementById('ddlexchange').options[0].selected = true;
                      //                        document.getElementById('ddlmktto').options[0].selected = true;
                      document.getElementById('ddlexchange').style.backgroundColor = '#E0E0E0';
                      document.getElementById('ddlexchange').disabled = true;
                      document.getElementById('ddlmktto').style.backgroundColor = '#E0E0E0';
                      document.getElementById('ddlmktto').disabled = true;
                      document.getElementById('txtcmb').style.backgroundColor = '#E0E0E0';
                      document.getElementById('txtcmb').disabled = true;
                      document.getElementById('TextBox4').style.backgroundColor = '#E0E0E0';
                      document.getElementById('TextBox4').disabled = true;
                      document.getElementById('txtdpid').value = dpid1.toUpperCase();

                  }
                  else if (document.getElementById('txtdpid').value == '' && trantype == 4) {
                      document.getElementById('ddlexchange').style.backgroundColor = 'White';
                      document.getElementById('ddlexchange').disabled = false;
                      document.getElementById('ddlmktto').style.backgroundColor = 'White';
                      document.getElementById('ddlmktto').disabled = false;
                      document.getElementById('txtcmb').style.backgroundColor = 'White';
                      document.getElementById('txtcmb').disabled = false;
                      document.getElementById('TextBox4').style.backgroundColor = 'White';
                      document.getElementById('TextBox4').disabled = false;

                  }
                  else {

                  }

              }
              if (obj == 'txtcmb') {

                  if (document.getElementById('txtcmb').value != '' && trantype == 4) {
                      document.getElementById('txtdpid').style.backgroundColor = '#E0E0E0';
                      document.getElementById('txtdpid').disabled = true;
                      document.getElementById('txtclient').style.backgroundColor = '#E0E0E0';
                      document.getElementById('txtclient').disabled = true;
                      document.getElementById('ddlmkt').style.visibility = 'visible';
                  }
                  else if (document.getElementById('txtcmb').value == '' && trantype == 4) {
                      document.getElementById('txtdpid').style.backgroundColor = 'White';
                      document.getElementById('txtdpid').disabled = false;
                      document.getElementById('txtclient').style.backgroundColor = 'White';
                      document.getElementById('txtclient').disabled = false;
                      document.getElementById('ddlmkt').style.visibility = 'visible';
                  }
                  else {
                      document.getElementById('ddlmkt').style.visibility = 'visible';
                  }

              }
              if (obj == 'txtclient') {

                  document.getElementById('ddlmktto').style.visibility = 'visible';
                  document.getElementById('ddlexchange').style.visibility = 'visible';

              }
              //                  if(obj=='TextBox3')
              //                 {
              //                  
              //                  document.getElementById('ddlmkt').style.visibility='visible';                 
              //                  
              //                 }

          }
          function getdpid(s) {
              document.getElementById('hiddendpid').value = s;
          }

          function checkquan() {
              var trantype = '<%= tran %>';
              //alert(trantype);
              if (document.getElementById('txtisin').value == '') {
                  alert('ISIN can not be blank');
                  return false;
              }
              if (document.getElementById('txtdpid').disabled == false) {
                  var dpid = new String(document.getElementById('txtdpid').value);
                  if (trantype == 4 && document.getElementById('txtcmb').value == '') {
                      if (dpid == '') {
                          alert('DPID can not be blank');
                          return false;
                      }
                      //                      else
                      //                      {                        
                      ////                         if (dpid.length!=8)
                      ////                         {
                      ////                          alert('DPID length shold be 8');                      
                      ////                          return false;
                      ////                         }
                      //                         if(isNaN(dpid.substr(2,6)))
                      //                         {
                      //                          
                      //                            alert('DPID last 6 no should be digits');
                      //                            return false;
                      //                         
                      //                         }
                      //                         
                      //                         else if(dpid.substr(0,2)!='in' && dpid.substr(0,2)!='IN')
                      //                         {                                          
                      //                           alert('DPID first 2 letter should be IN');
                      //                           return false;
                      //                         }
                      //                      }
                  }
              }

              var a1 = new Number(document.getElementById('txtqty').value);
              var a2 = new Number(document.getElementById('txtholding').value);
              if (document.getElementById('txtqty').value == '') {
                  alert('Quantity can not be blank');
                  return false;
              }
              if (isNaN(a1)) {
                  alert('Not valid Quantity');
                  return false;
              }
              //                  if(a1 > a2)
              //                  {
              //                   alert('Quantity can not be greater than Holding');
              //                   return false;
              //                  }

              if (document.getElementById('txtclient').disabled == false) {
                  var vall = new String(document.getElementById('txtclient').value);

                  if (trantype != 4) {
                      if (vall == '') {
                          alert('Client name can not be blank');
                          return false;
                      }
                      if (vall.length < 8) {
                          alert('Client name not valid');
                          return false;
                      }
                      if (!isNaN(vall)) {
                          var d = new String(document.getElementById('txtdpid').value);

                          if (d.substr(0, 8) == 'IN301493') {
                              //alert(d.substr(0,5));
                              alert('Client name not valid');
                              return false;
                          }

                      }
                      if (isNaN(vall.substr(0, 8))) {
                          alert('Client name not valid');
                          return false;
                      }
                  }
                  else {
                      if (document.getElementById('txtdpid').value != '') {
                          if (vall == '') {
                              alert('Client name can not be blank');
                              return false;
                          }
                          if (vall.length < 8) {
                              alert('Client name not valid');
                              return false;
                          }
                          if (isNaN(vall.substr(0, 8))) {
                              alert('Client name not valid');
                              return false;
                          }
                      }
                      else {
                          if (vall != '') {
                              alert('Client name can not be given');
                              return false;
                          }
                      }
                  }

              }
              if (document.getElementById('TextBox1').disabled == false) {
                  var cmid = new String(document.getElementById('TextBox1').value);
                  if (cmid == '') {
                      alert('CMID can not be blank');
                      return false;
                  }
                  if (isNaN(cmid)) {
                      alert('CMID is not valid');
                      return false;
                  }
                  if (cmid.length < 8) {
                      alert('CMID is not valid');
                      return false;
                  }
              }
              if (document.getElementById('TextBox3').disabled == false) {
                  var setfrom = new String(document.getElementById('TextBox3').value);
                  if (setfrom == '') {
                      alert('Settlement from can not be blank');
                      return false;
                  }
              }
              if (document.getElementById('TextBox4').disabled == false) {
                  if (trantype != 4 || document.getElementById('txtcmb').value != '') {
                      var setto = new String(document.getElementById('TextBox4').value);
                      if (setto == '') {
                          alert('Settlement to can not be blank');
                          return false;
                      }
                  }
              }
              if (document.getElementById('ddlexchange').disabled == false) {
                  var exchange = document.getElementById("ddlexchange");
                  var ddlex1 = exchange.options[exchange.selectedIndex].value;
                  if (trantype != 4 || document.getElementById('txtcmb').value != '') {
                      if (ddlex1 == 'Select') {
                          alert('Select Exchange');
                          return false;
                      }
                  }
              }

              //                  enabledisable();
              if ((trantype == 10) || (trantype == 11)) {
                  var radioButtons = document.getElementsByName("RadioButtonList1");
                  //alert(radioButtons[1].checked);    
                  //alert(radioButtons[2].checked);   
                  if ((radioButtons[1].checked != true) && (radioButtons[2].checked != true)) {
                      alert('Instruction type should be given');
                      return false;
                  }
              }
              return checkvalid(trantype);
          }

          function checkvalid(obj) {

              if (obj != 4 || document.getElementById('txtdpid').value == '') {
                  if (document.getElementById('txtcmb').disabled == false) {

                      if (document.getElementById('txtcmb').value == '') {
                          alert('CMBPID can not be blank');
                          return false;
                      }
                      else
                          return true;


                  }
              }

          }
          function pageclose() {
              //                var x=window.confirm("Do you want to close the window?");
              //                if(x)
              //                {
              parent.editwin.close();
              //                }
              //                else
              //                {
              //                 document.getElementById('txtisin').focus();
              //                }

          }
          function DateChangeForFrom(s) {
              var currentTime = new Date()
              if (currentTime < s.GetValue()) {

              }
              else {
                  s.SetDate(currentTime);

              }

          }



                </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div align="center">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table style="width: 860px; height: 59px">
            <tr>
                <td colspan="3">&nbsp; &nbsp; &nbsp; Client id:&nbsp;
                        <asp:Label ID="lblid" runat="server" Text="Label"></asp:Label>
                    &nbsp; &nbsp; &nbsp; &nbsp; Transaction type: &nbsp;<asp:Label ID="lbltype" runat="server"
                        Text="Label"></asp:Label>
                    &nbsp; &nbsp;&nbsp; &nbsp; Slip no: &nbsp;<asp:Label ID="lblslip" runat="server"
                        Text="Label"></asp:Label>
                    &nbsp; &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 102px" align="left">Execution Date:</td>
                <td style="width: 358px" align="left">
                    <dxe:ASPxDateEdit ID="dtexec" runat="server" ClientInstanceName="dtexec1" DateOnError="Today"
                        EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                        TabIndex="5" UseMaskBehavior="True" Width="120px">
                        <ClientSideEvents ValueChanged="function(s, e){ DateChangeForFrom(dtexec1);}" />
                    </dxe:ASPxDateEdit>
                </td>
                <td style="width: 388px"></td>
                <td style="width: 371px"></td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div align="center">
                    <table id="Table2" border="0" style="width: 90%; background-color: #ddecfe">

                        <tr>
                            <td style="text-align: left">Mkt Type From:</td>
                            <td colspan="3" style="text-align: left">
                                <asp:DropDownList ID="ddlmkt" runat="server" Width="250px" EnableTheming="True" TabIndex="9">
                                </asp:DropDownList></td>
                        </tr>
                        <tr id="TrCostPrice">
                            <td style="text-align: left">Settlement No From:</td>
                            <td style="text-align: left">
                                <asp:TextBox ID="TextBox3" runat="server" Width="250px" TabIndex="11"></asp:TextBox></td>
                            <td style="text-align: left"></td>
                            <td style="text-align: left">&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="text-align: left">DPID<asp:TextBox ID="txtdpid3" runat="server" Width="50px" ReadOnly="True"></asp:TextBox></td>
                            <td colspan="6" style="height: 24px; text-align: left">
                                <asp:TextBox ID="txtdpid" runat="server" Width="450px" onkeydown="hidedrop('txtdpid');" onblur="showdrop('txtdpid');" TabIndex="4"></asp:TextBox>

                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left">CM Account no:
                            </td>
                            <td style="text-align: left">

                                <asp:TextBox ID="txtcmb" runat="server" Width="450px" onblur="caps();" onkeydown="hidedrop('txtcmb');" Wrap="False" TabIndex="5"></asp:TextBox>
                            </td>
                            <td style="text-align: left">Client ID :</td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtclient" runat="server" Width="250px" onkeydown="hidedrop('txtclient');" onblur="showdrop('txtclient');" TabIndex="6"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="text-align: left">CMID :
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="TextBox1" runat="server" Width="250px" MaxLength="8" TabIndex="7"></asp:TextBox></td>
                            <td style="text-align: left">Exchange
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="ddlexchange" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlexchange_SelectedIndexChanged" Width="250px" TabIndex="8">
                                </asp:DropDownList>
                            </td>
                        </tr>

                        <tr>
                            <td style="text-align: left">ISIN :</td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtisin" runat="server" onkeydown="hidedrop('txtisin');" onblur="showdrop('txtisin');"
                                    Width="450px" TabIndex="1"></asp:TextBox></td>
                            <td style="text-align: left">Mkt Type To:</td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="ddlmktto" runat="server" Width="250px" TabIndex="10">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left">Free Holding:</td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtholding" runat="server"
                                    Width="250px" TabIndex="2" Enabled="False"></asp:TextBox></td>
                            <td style="text-align: left">Settlement No To:</td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="TextBox4" runat="server" Width="250px" TabIndex="12"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="text-align: left">Quantity:
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtqty" runat="server"
                                    Width="250px" TabIndex="3"></asp:TextBox></td>
                            <td style="text-align: left"></td>
                            <td style="text-align: left"></td>
                        </tr>
                        <tr>
                            <td id="re1" style="text-align: left">Instruction type:</td>
                            <td id="re" style="text-align: left">
                                <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="906">Reversible</asp:ListItem>
                                    <asp:ListItem Value="912">Irreversible</asp:ListItem>
                                </asp:RadioButtonList></td>
                            <td style="text-align: left"></td>
                            <td style="text-align: left"></td>
                        </tr>
                        <tr>
                            <td style="height: 26px"></td>
                            <td style="height: 26px">
                                <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" OnClientClick="return checkquan();" TabIndex="13" /></td>
                            <td style="height: 26px"></td>
                            <td style="height: 26px">
                                <asp:Button ID="btncancel" runat="server" Text="Cancel" TabIndex="14" OnClick="btncancel_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 35px" id="hh">
                                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" BackColor="#DDECFE" BorderStyle="None" /></td>
                            <td style="height: 35px">
                                <dxe:ASPxTextBox ID="txtname" runat="server" BackColor="#DDECFE" ClientInstanceName="textBox"
                                    Height="0px" Width="0px">
                                    <Border BorderStyle="None" />
                                </dxe:ASPxTextBox>
                                &nbsp;
                            </td>
                            <td style="height: 35px"></td>
                            <td style="height: 35px">
                                <asp:HiddenField ID="hiddendpid" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 29px"></td>
                            <td style="height: 29px">
                                <asp:HiddenField ID="hiddensettleto" runat="server" />
                                <asp:HiddenField ID="hiddenex" runat="server" />
                                <asp:HiddenField ID="Hiddenhold" runat="server" />
                                <asp:HiddenField ID="Hiddendpidm" runat="server" />
                                <asp:HiddenField ID="Hiddendateex" runat="server" />
                            </td>
                            <td style="height: 29px"></td>
                            <td style="height: 29px">
                                <asp:HiddenField ID="hiddensettlefrom" runat="server" />
                                <asp:HiddenField ID="Hiddenid" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>

        </asp:UpdatePanel>
    </div>
</asp:Content>
