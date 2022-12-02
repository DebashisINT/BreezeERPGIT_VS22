<%@ Import Namespace="System.Web.Services" %>
<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.management_frmshowreminder1" Codebehind="frmshowreminder1.aspx.cs" %>
<%@ outputcache duration="1" varybyparam="none" Location="None" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">
        function SignOff()
           {
                parent.window.SignOff();
           }
    </script>
    <script type="text/javascript" language="javascript">
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

    function countdown() 
    {
	    nowtime= new Date()
	    nowtime=nowtime.getTime()
	    secondssinceloaded=(nowtime-starttime)/10
	    reloadseconds=Math.round(refreshinterval-secondssinceloaded)
	    if (refreshinterval>=secondssinceloaded) {
            var timer=setTimeout("countdown()",10)
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
</asp:Content>
