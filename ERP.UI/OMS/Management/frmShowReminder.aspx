<%@ Import Namespace="System.Web.Services" %>
<%@ Page Language="C#" AutoEventWireup="true" Inherits="ERP.OMS.Management.management_frmShowReminder" MasterPageFile="~/OMS/MasterPage/ERP.Master" Codebehind="frmShowReminder.aspx.cs" %>
<%@ outputcache duration="1" varybyparam="none" Location="None" %>


<%--<script runat="server">
        [System.Web.Services.WebMethod()] 
        public string GetSessionValue(string key)
        {
            string strScrolling="";
            DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            try
            {    
                HtmlTableCell cellScrolling = new HtmlTableCell();
                strScrolling = "<Marquee OnMouseOut='this.start();' direction='horizental' scrollamount='5' border='1px #000000 solid' bgcolor='#FFFFFF' width='100%'> <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width: 100%\"><tr>";
                DataTable DT = oDBEngine.GetDataTable(" tbl_trans_reminder "," rem_id AS Rid, rem_createUser AS CreaterId, (SELECT tbl_master_user.user_name FROM tbl_master_user WHERE user_id = rem_createUser) AS CreateBy, rem_createDate AS CreateDate, rem_targetUser AS TargetId,(SELECT tbl_master_user.user_name FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, rem_startDate AS StartDate, rem_endDate AS EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'NotAttened' ELSE 'Attened' END AS Status "," CAST(rem_startDate AS DateTime) <= getdate()  AND CAST(rem_endDate AS DateTime) >= getdate()  AND ((rem_targetUser = " + HttpContext.Current.Session["userid"] + ")) and (rem_displayTricker = 1) and (rem_actiontaken = 0)");
                if (DT.Rows.Count != 0)
                {
                     for (int i = 0; i < DT.Rows.Count; i++)
                     {
                        if (i % 2 == 0)
                            strScrolling = strScrolling + "  <td style='color: blue;' nowrap='nowrap' id=" + DT.Rows[i][0].ToString() + " onclick='openNewPath147(" + DT.Rows[i][0].ToString() + ");' style=\"vertical-align: top;  text-align: left; font-size: 9pt; text-transform: capitalize; color: maroon; font-style: normal; font-family: Verdana, Arial; cursor: hand; \" >[" + (i + 1) + "] \" " + DT.Rows[i][8].ToString() + "  !!  </td>";  
                        else
                            strScrolling = strScrolling + "  <td style='color: red;' nowrap='nowrap' id=" + DT.Rows[i][0].ToString() + " onclick='openNewPath147(" + DT.Rows[i][0].ToString() + ");' style=\"vertical-align: top;  text-align: left; font-size: 9pt; text-transform: capitalize; color: maroon; font-style: normal; font-family: Verdana, Arial; cursor: hand; \" >[" + (i + 1) + "] \" " + DT.Rows[i][8].ToString() + "  !!  </td>";  
                     }  
                    strScrolling = strScrolling + "</tr></table></Marquee>";
                    strScrolling = strScrolling + "####" + DT.Rows.Count;
                    DT.Clear();
                }
                else
                {
                    strScrolling = "";   
                }
            } 
            catch (Exception ex)
            {
                //ex.message();
            }
            return strScrolling;  
        }
        
</script>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/ecmascript">
        function ParentCall(obj)
        {   
            CallServer(obj, "");
           
        }
        function openNewPath147(obj)
        {
            CallServer(obj, "");
           
        }
        function GetTempFromServer(TextBox2, context)
        {
            
            document.getElementById('SpnResultId').innerHTML=TextBox2;//DATA[0];
            
            parent.btnToday_click();

        }
       function SignOff()
       {
            parent.window.SignOff();
       }
    </script>
    
    <script language="JavaScript" type="text/javascript"> 
    var refreshinterval;//=10
     var cashJournal ;
    refreshinterval = <%=Session["TimeForTickerDisplay"] %>;
    cashJournal = <%=Session["cashJournal"] %>;    
    var displaycountdown="NO"
     var starttime
    var nowtime
    var reloadseconds=0
    var secondssinceloaded=0

    function starttime() {
	    starttime=new Date()
	    starttime=starttime.getTime()
	    if(cashJournal==1)
            countdown()
    }

    function countdown() {
	    nowtime= new Date()
	    nowtime=nowtime.getTime()
	    secondssinceloaded=(nowtime-starttime)/100
	    reloadseconds=Math.round(refreshinterval-secondssinceloaded)
	    if (refreshinterval>=secondssinceloaded) {
            var timer=setTimeout("countdown()",100)
		    if (displaycountdown=="yes") {
			    window.status="Page refreshing in "+reloadseconds+ " seconds"
		    }
        }
        else {
            clearTimeout(timer)
		    window.location.reload(true)
		    //ParentCall('Parent')
        } 
    }
    window.onload=starttime

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <table style="width:100%; height:20px;">
            <tr>
                <td align="right">
                    <div id="reminder" style="border-width:0px; width:100%; border:0px"  >               
                        <span runat="server" style=" width:100%; border:0px" id="SpnResultId">
                        </span>
                    </div> 
                </td>
           </tr>
       </table>
  </asp:Content>
