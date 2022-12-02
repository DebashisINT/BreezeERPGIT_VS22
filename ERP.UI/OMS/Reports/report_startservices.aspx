<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_report_startservices" CodeBehind="report_startservices.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<%--    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>--%>
    <script language="javascript" type="text/javascript">
        //function SignOff() {
        //    window.parent.SignOff();
        //}
        var timer = ('<%=Session["time"] %>');
         var timeCont, t, timerOn = 0, timePanel
         function countTimer() {
             timeCont.value = timer;
             timePanel.innerHTML = hms(timer);
             timer--;
             t = setTimeout("countTimer()", 1000);
         }
         function playTimer(obj) {
             if (timeCont == null || timeCont == "undefined") timeCont = document.getElementById("timerval");
             if (timePanel == null || timePanel == "undefined") timePanel = document.getElementById("timepanel");
             if (!timerOn) {
                 timerOn = 1;
                 countTimer();
                 obj.value = timerLabel[1];
             } else {
                 timerOn = 0;
                 clearTimeout(t);
                 obj.value = timerLabel[0];
             }
         }
         function hms(secs) {
             time = [0, 0, secs];
             for (var i = 2; i > 0; i--) {
                 time[i - 1] = Math.floor(time[i] / 60);
                 time[i] = time[i] % 60;
                 if (time[i] < 10) time[i] = '0' + time[i];
             };
             return time.join(':');
         }
         function autoStartTimer() {
             playTimer(this);
         }
         //window.onload = autoStartTimer;
         //function height() {
         //    if (document.body.scrollHeight >= 300)
         //        window.frameElement.height = document.body.scrollHeight;
         //    else
         //        window.frameElement.height = '300px';
         //    window.frameElement.Width = document.body.scrollWidth;
         //}
         function Page_Load() {
             document.getElementById('Div1').style.display = "none";
         }

         function PassValues(appnameshow) {
             var txtValue = appnameshow;
             alert(txtValue);
         }
         function ajaxFunction() {
             // alert ('1');   

             var aa = "";
             var path = "";
             // aa= document.getElementById('hdnName').value;
             path = document.getElementById('hdnpath').value;
             //alert (path);
             var xmlhttp;
             if (window.XMLHttpRequest) {
                 //alert ('1');
                 // code for IE7+, Firefox, Chrome, Opera, Safari
                 xmlhttp = new XMLHttpRequest();
             }
             else {
                 //alert ('2');
                 // code for IE6, IE5
                 xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");

             }
             //alert(xmlhttp);
             xmlhttp.onreadystatechange = function () {
                 if (xmlhttp.readyState == 4) {
                     //alert ('3');  
                     var a = xmlhttp.responseText;
                     //'document.myForm.time.value=a; //xmlhttp.responseText;
                     if (a != "") {
                         document.getElementById('Div1').style.display = 'inline';
                         CreateFile(a);
                     }
                 }
             }
             xmlhttp.open("GET", "Defaultnew.aspx?path=" + path + "", true);
             xmlhttp.send(null);

         }
         function CreateFile(value) {
             //var source="C:\\Sachin.txt";
             //var destination="\\\\anil\\Test\\TestData.txt";
             var rand_no = Math.random();
             //alert(rand_no);           

             var now = new Date();
             var then = now.getFullYear() + '_' + (now.getMonth() + 1) + '_' + now.getDate();
             then += '_' + now.getHours() + '_' + now.getMinutes() + '_' + now.getSeconds();


             //var destination="C:\\Influx\\ContractNote"+ then +".txt";
             var destination = document.getElementById('hdnLocationPath').value;
             var destination1 = '<%=Session["deletepath"]%>';
        //alert (destination1);
        //////////filesystemobject.DeleteFile(destination1, force)

        var fSObj;
        try {
            fSObj = new ActiveXObject("Scripting.FileSystemObject");
            var enObj = new Enumerator(fSObj.GetFolder(destination1).Files);
            for (i = 0; !enObj.atEnd() ; enObj.moveNext()) {
                var fileName = new String(enObj.item(i).name);
                //alert(fileName);
                var test = destination1 + '/' + fileName
                // alert (test);
                // filesystemobject.DeleteFile(destination1 +'/'+ fileName, true);
                //		            fileName=fileName.toLowerCase();	
                //		            if(txtToSearch.substring(0,1)==".")
                //			            fileName=fileName.substring(fileName.lastIndexOf("."),fileName.length);
                //		            if(txtToSearch==fileName || fileName.search(txtToSearch)>-1)
                //		            {
                //			            ..........			}
            }
        }
        catch (err) {
            //alert("error creating file system object");
            //alert(err.description);
            alert("Please Enable The option\n'Initialize and Script Active X controls not marked as safe for scripting' \n Under 'Active X Controls & Piug-Ins' \n from Internet options -> Security Settings");

        }
        varFileObject = fso.OpenTextFile(destination, 2, true, 0);
        //alert (destination); // 2=overwrite, true=create if not exist, 0 = ASCII
        varFileObject.write(value);
        alert(value);
        varFileObject.close();
        alert("Print Send To Printer");
        document.getElementById('Div1').style.display = 'none';
        // Pageload(); 
    }
    //    function btn_Click()
    //    {
    //         document.getElementById('Div1').style.display="inline";
    //        combo.PerformCallback();
    //    }
    //    function ShowError(obj)
    //    {
    //        document.getElementById('Div1').style.display="none";
    //        if(obj=="b")
    //        {
    //            alert('Accounts Ledger Repost !!');
    //        }
    //        else
    //        {
    //            alert('No Data In This Company And Segment !!');
    //        }
    //         
    //    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="panel-heading">
       <div class="panel-title">
           <h3>Start / Stop Services</h3>
       </div>

   </div> 
  <div class="form_main">
        
        <%-- <div id='Div1' style='position:absolute; font-family:arial; font-size:30; left:40%; top:25%; background-color:white; layer-background-color:white; height:80; width:150;'>
                    <table class="TableMain100"> 
                      <tr><td><table><tr> 
                         <td height='25' align='center' bgcolor='#FFFFFF'> 
                           <img src='/assests/images/progress.gif' width='18' height='18'></td>  
                            <td height='10' width='100%' align='center' bgcolor='#FFFFFF'><font size='2' face='Tahoma'> 
 	                        <strong align='center'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Please Wait..</strong></font></td> 
                            </tr>  </table></td></tr>
                            </table> 
                    </div>--%>
        <table class="TableMain100" style="width:700px !important">
            <tr>
                <td style="vertical-align: top; padding-left: 30px; padding-bottom: 15px;">
                    <div class="crossBtn"><a href="../Management/ProjectMainPage.aspx"><i class="fa fa-times"></i></a></div>
                </td>
            </tr>
            <tr>
                <td align="right" id="td_service" runat="server">

                    <asp:DropDownList ID="ddlservices" runat="server" Width="140px" Font-Size="12px">
                        <asp:ListItem Value="1">Email Service</asp:ListItem>

                    </asp:DropDownList>
                </td>
                <%--</tr>
    <tr>--%>
                <td align="left" id="td_yes" runat="server">
                    <asp:Button ID="btnYes" runat="server" CssClass="btn btn btn-primary" Text="Start Service" Width="180px" OnClick="btnYes_Click" />
                </td>
                <td align="left" id="td_no" runat="server">
                    <asp:Button ID="btnNo" runat="server" CssClass="btn btn btn-danger" Text="Stop Service" Width="180px" OnClick="btnNo_Click" />
                </td>
                <td id="tr_time" runat="server" align="left" style="font-size: small; font-weight: bolder">Running Interval :
                                            <asp:DropDownList ID="DdlRptType" runat="server" Width="100px" Font-Size="12px">
                                                <asp:ListItem Value="300000">5 mint</asp:ListItem>
                                                <asp:ListItem Value="600000">10 mint</asp:ListItem>
                                                <asp:ListItem Value="1200000">20 mint</asp:ListItem>
                                                <asp:ListItem Value="1800000">30 mint</asp:ListItem>
                                                <asp:ListItem Value="3600000">1 hr</asp:ListItem>
                                                <asp:ListItem Value="7200000">2 hr</asp:ListItem>
                                                <asp:ListItem Value="18000000">5 hr</asp:ListItem>

                                                <asp:ListItem Value="864000000">24 hr</asp:ListItem>
                                            </asp:DropDownList>
                </td>
            </tr>
            <tr style="height: 10px;">
            </tr>


            <%-- <tr id="tr_time" runat="server">
   <%-- <td align="right">
    
    <asp:CheckBox ID="TIME" Checked="false" runat="server" />
    </td>--%>
            <%--<td class="gridcellleft" bgcolor="#B7CEEC">
                                            Report Type :</td>--%>
            <%--<td id="tr_time" runat="server" align="right" style="font-size:small; font-weight:bolder"> Run for how long :
                                            <asp:DropDownList ID="DdlRptType" runat="server" Width="100px" Font-Size="12px" >
                                                <asp:ListItem Value="300000">5 mint</asp:ListItem>
                                                <asp:ListItem Value="600000">10 mint</asp:ListItem>
                                                <asp:ListItem Value="1200000">20 mint</asp:ListItem>
                                                <asp:ListItem Value="1800000">30 mint</asp:ListItem>
                                                <asp:ListItem Value="3600000">1 hr</asp:ListItem>
                                                <asp:ListItem Value="7200000">2 hr</asp:ListItem>
                                                <asp:ListItem Value="18000000">5 hr</asp:ListItem>
                                                
                                                <asp:ListItem Value="864000000">24 hr</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
    </tr>--%>
            <%--<tr id="tr_timer">
    <asp:Label runat="server" ID="remaing" Text="Remaing Time" ></asp:Label>
    <asp:Literal ID="litTimerLabels" runat="server"></asp:Literal>
<input type="hidden" name="timerval" id="timerval" value=""/>
<div id="timepanel" style="font-weight:bold;font-size:12px;float:left;padding:2px 20px 0 0;color:Red;"></div>
</tr>--%>
        </table>
    </div>
    <asp:HiddenField ID="hdnpath" runat="server" />
    <asp:HiddenField ID="hdnLocationPath" runat="server" />
</asp:Content>


