<%@ Control Language="C#" AutoEventWireup="true" Inherits="Headermain" Codebehind="Headermain.ascx.cs" %>

<script type="text/javascript" language="javascript">
    function OnSegmentChange(keyValue) 
    {
         var url='../management/frm_selectCompFinYrSett.aspx?id='+keyValue;
         //alert(url);
         editwin=dhtmlmodal.open("Editbox", "iframe", url, "", "width=400px,height=245px,center=1,resize=0,scrolling=2,top=500", "recal")
         document.getElementById('ctl00_ContentPlaceHolder1_Headermain1_cmbSegment').style.visibility='hidden';
         editwin.onclose=function()
         {
             document.getElementById('ctl00_ContentPlaceHolder1_Headermain1_cmbSegment').style.visibility='visible';
             window.location='/management/welcome.aspx';
         }
    }
    function CloseW()
    {//alert('CloseW');
        editwin.close();
    }
    function showpage(obj)
    {
        OnSegmentChange(obj);
    }
    var isCtrl = false;
    document.onkeyup=function(e)
    {
	   if(event.keyCode == 17) 
	   {
	       isCtrl=false;
	   }
    }
    document.onkeydown=function(e)
    {
	    if(event.keyCode == 17) isCtrl=true;
	    if(event.keyCode == 81 && isCtrl == true) 
	    {
		    //run code for CTRL+Q -- ie, open!
		    window.location='/management/CashBank.aspx';
		    return false;
	    }
	    if(event.keyCode == 74 && isCtrl == true) 
	    {
		    //run code for CTRL+J -- ie, open!
		    window.location='/management/journal.aspx';
		    return false;
	    }
    } 
</script>

<link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />

<script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>

<link rel="stylesheet" href="../modalfiles/modal.css" type="text/css" />

<script type="text/javascript" src="../modalfiles/modal.js"></script>

<asp:Panel ID="Panel1" runat="server" Width="100%">

            
    <table style="width: 100%" cellpadding="0" cellspacing="0">
        <tr>
            <td style="vertical-align: top;">
                <table style="width: 100%;" cellpadding="0" cellspacing="0">
                    <%--<tr style="background-image: url(../images/topheadbg.jpg); background-repeat:repeat-x; background-color: #A3BFEE"> --%>
                    <tr style="background-color: white">
                        <td colspan="2" style="height: 50px; padding-bottom: 5; padding-left: 5; padding-bottom: 5;
                            padding-right: 5; padding-top: 5;">
                            <table style="width: 100%;" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td style="width: 10px;">
                                    </td>
                                    <%--<td style="width:150px; vertical-align:middle;background-color:#FFFFFF; border-top:solid 1px #4B6CA1; border-bottom:solid 1px #4b6ca1;"><img src="/assests/images/logo.jpg" width="261" height="61" alt="A" /></td>--%>
                                    <td style="width: 100px; vertical-align: middle; background-color: #FFFFFF;">
                                        <img src="/assests/images/logo.jpg" width="261" height="61" alt="A" /></td>
                                    <td style="text-align: right; vertical-align: top; width: 350px;">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="text-align: left">
                                                    <iframe id="Iframe1" src="../management/frmnewmessage.aspx" width="40" height="30"
                                                        frameborder="0" scrolling="no" style="width: 60px; height: 57px"></iframe>
                                                </td>
                                                <td style="padding:5px 5px 5px 5px">
                                                    <div style="border: solid 1px #F8B187;">
                                                        <table id="tblSegment" runat="server">
                                                            <tr>
                                                                <td class="gridcelleft" style="text-align: left" colspan="2">
                                                                    <asp:Label ID="lblSCompName" runat="server" Font-Size="12px" ForeColor="Navy" Font-Bold="True"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="gridcelleft"  style="text-align: left" colspan="2">
                                                                    <asp:Label ID="lblSettNo" runat="server" Font-Size="12px" ForeColor="Navy" Font-Bold="True"></asp:Label>
                                                                    <span style="color: #ff9966">|</span>
                                                                    <asp:Label ID="lblStartDate" runat="server" Font-Size="12px" ForeColor="Navy" Font-Bold="True"></asp:Label>
                                                                    <span style="color: #ff9966">|</span>
                                                                    <asp:Label ID="lblfundPayeeDate" runat="server" Font-Size="12px" ForeColor="Navy" Font-Bold="True"></asp:Label>
                                                                </td>
                                                                
                                                            </tr>
                                                            <tr>
                                                                <td class="gridcelleft" style="text-align: left">
                                                                    <asp:Label ID="lblFinYear" runat="server" Font-Size="12px" ForeColor="Navy" Font-Bold="True"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <a href="#" id="lnkSelectCompanySettFinYear" class="Headlink" runat="server"><strong>
                                                                        <span style="color: #336699; text-decoration:underline"><em>Change</em></span></strong></a>
                                                                </td>
                                                            </tr>
                                                            
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="text-align: right; vertical-align: top;">
                                        <table style="width: 100%;" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td class="coheadtxt" style="text-align: right;">
                                                    <asp:Label ID="lblCompHead" runat="server" ForeColor="Navy"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <%-- <td style="text-align: right; height: 19px;" class="coheadtxt">  
                                            
                                        </td>--%>
                                                <td>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <%--<td style="text-align:right; height: 58px; width: 80px;" align="right">
                                                  </td>--%>
                                                            <td style="text-align: right; width: 40%; vertical-align: top" align="center">
                                                                <%--<table style="width:100%; vertical-align:top">--%>
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td class="mt" style="width: 10%; height: 18px; text-align: right;">
                                                                            <!-- Time dispaly file will be here -->
                                                                            <asp:DropDownList ID="cmbSegment" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbSegment_SelectedIndexChanged"
                                                                                Width="100px" BackColor="#FCA977">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td style="width: 3%; height: 18px; text-align: right;" id="td1" class="mt" onmousemove="chgtxt(this,'link1','#A4BAE8','white');"
                                                                            onmouseout="chgtxt(this,'link1','white','black');">
                                                                            <a href="#" id="link1" class="Headlink">Settings</a>
                                                                        </td>
                                                                        <td style="width: 3%; height: 18px; text-align: left; padding-left: 7px" id="td2"
                                                                            class="mt" onmousemove="chgtxt(this,'link2','#A4BAE8','white');" onmouseout="chgtxt(this,'link2','white','black');">
                                                                            <a href="../SignOff.aspx" id="link2" class="Headlink">Sign off</a>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <asp:Label ID="Label2" runat="server" Text="Welcome, " ForeColor="Navy"></asp:Label>
                                                                <asp:Label ID="lblName" runat="server" ForeColor="Navy"></asp:Label>
                                                                <a href="#" class="WhiteLink">
                                                                    <asp:Label ID="Label1" runat="server" Text="| Last Logged On: " ForeColor="Navy"></asp:Label>
                                                                    <asp:Label ID="lblLastTime" runat="server" ForeColor="Navy"></asp:Label>
                                                                </a>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <iframe id="iFrmReminder" style="vertical-align: top;" src="../management/frmShowReminder.aspx"
                    width="100%" height="20" marginheight="0" marginwidth="0" frameborder="0" scrolling="no">
                </iframe>
            </td>
        </tr>
        <tr>
            <td>
                <table class="width100per" cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr style="width: 100%; background-image: url(../images/menu_strip_bg.jpg); background-repeat: repeat-x;">
                        <td style="width: 100%;">
                            <asp:Menu ID="Menumain" runat="server" Orientation="Horizontal" StaticEnableDefaultPopOutImage="true"
                                StaticTopSeparatorImageUrl="../images/menu_strip_bg1.JPG">
                                <StaticMenuStyle VerticalPadding="2px" HorizontalPadding="1px" />
                                <StaticMenuItemStyle HorizontalPadding="2px" ItemSpacing="2px" VerticalPadding="1px"
                                    Width="80px" />
                                <DynamicHoverStyle BackColor="#FFE0C0" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                <StaticHoverStyle BackColor="#FFE0C0" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                <DynamicMenuStyle HorizontalPadding="0px" VerticalPadding="0px" BorderColor="Black"
                                    BorderStyle="Solid" BorderWidth="1px" />
                                <DynamicMenuItemStyle Width="200px" VerticalPadding="0px" CssClass="SubMenu" />
                            </asp:Menu>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    
</asp:Panel>
