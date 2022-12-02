<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_Billing" Codebehind="frmReport_Billing.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="/assests/js/GenericJScript.js"></script>
<script language="javascript" type="text/javascript">

    function Page_Load()///Call Into Page Load
            {
                
                 Hide('Tab_DisplayNoAction');
                 Hide('Tab_BtnRemainingBill');
                 height();
            }
   function height()
        {
            if(document.body.scrollHeight>=350)
            {
                window.frameElement.height = document.body.scrollHeight;
            }
            else
            {
                window.frameElement.height = '350px';
            }
            window.frameElement.width = document.body.scrollWidth;
        }
    function Hide(obj)
            {
             document.getElementById(obj).style.display='none';
            }
    function Show(obj)
            {
             document.getElementById(obj).style.display='inline';
            }
        function DateChangeForFrom()
        {
            var FYS ='<%=Session["FinYearStart"]%>';
            var FYE ='<%=Session["FinYearEnd"]%>'; 
            var LFY ='<%=Session["LastFinYear"]%>';
            DevE_CheckForFinYear(dtFrom,FYS,FYE,LFY);
            DevE_CheckForFinYear(dtTo,FYS,FYE,LFY);
            var Msg="To Date Can Not Be Less Than From Date!!!";
            DevE_CompareDateForMin(dtFrom,dtTo,Msg);
           
        }
        function DateChangeForTo()
        {
            var FYS ='<%=Session["FinYearStart"]%>';
            var FYE ='<%=Session["FinYearEnd"]%>'; 
            var LFY ='<%=Session["LastFinYear"]%>';
            DevE_CheckForFinYear(dtFrom,FYS,FYE,LFY);
            DevE_CheckForFinYear(dtTo,FYS,FYE,LFY);
            var Msg="To Date Can Not Be Less Than From Date!!!";
            DevE_CompareDateForMin(dtFrom,dtTo,Msg);
        }
   function fnAlert(obj)
   {
       if(obj=="1")
        {
            Show('Tab_DisplayNoAction');
            Hide('Tab_BtnRemainingBill');
//            alert('Please Complete The Process For Above Days and Generate Bills...');
        }
        if(obj=="2")
        {
            Show('Tab_DisplayNoAction');
            Show('Tab_BtnRemainingBill');
//            alert('Generate For Remaining Bills...');
        }
        if(obj=="3")
        {
            Hide('Tab_DisplayNoAction');
            Hide('Tab_BtnRemainingBill');
            alert('Bill Generate Successfully !!');
        }
        if(obj=="4")
        {
            Hide('Tab_DisplayNoAction');
            Hide('Tab_BtnRemainingBill');
            alert('Bill Delete Successfully !!');
        }
        if(obj=="5")
        {
            alert('Nothing Found For Process.');
            window.location = "../Reports/frmReport_Billing.aspx";
        }
        
         
   }
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="36000">
            </asp:ScriptManager>

           <script language="javascript" type="text/javascript"> 
   var prm = Sys.WebForms.PageRequestManager.getInstance(); 
   prm.add_initializeRequest(InitializeRequest); 
   prm.add_endRequest(EndRequest); 
   var postBackElement; 
   function InitializeRequest(sender, args) 
   { 
      if (prm.get_isInAsyncPostBack()) 
         args.set_cancel(true); 
            postBackElement = args.get_postBackElement(); 
         $get('UpdateProgress1').style.display = 'block'; 
   } 
   function EndRequest(sender, args) 
   { 
          $get('UpdateProgress1').style.display = 'none'; 
 
   } 
            </script>

             <table class="TableMain100">
            <tr>
                <td class="EHEADER" colspan="0" style="text-align: center; height: 20px;">
                    <strong><span id="SpanHeader" style="color: #000099">Billing</span></strong></td>

           
            </tr>
        </table>
            <table class="TableMain100">
                <tr>
                    <td align="center">
                        <table>
                            <tr>
                                <td>
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Period :
                                            </td>
                                            <td>
                                                <dxe:ASPxDateEdit ID="dtFrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                    Font-Size="12px" Width="108px" ClientInstanceName="dtFrom">
                                                    <dropdownbutton text="From">
                                                    </dropdownbutton>
                                                    <ClientSideEvents DateChanged="function(s,e){DateChangeForFrom();}" />
                                                </dxe:ASPxDateEdit>
                                            </td>
                                            <td>
                                                <dxe:ASPxDateEdit ID="dtTo" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                    Font-Size="12px" Width="108px" ClientInstanceName="dtTo">
                                                    <dropdownbutton text="To">
                                                    </dropdownbutton>
                                                    <ClientSideEvents DateChanged="function(s,e){DateChangeForTo();}" />
                                                </dxe:ASPxDateEdit>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Generate Type :
                                            </td>
                                            <td colspan="2" align="left">
                                                <asp:DropDownList ID="dllGenerate" runat="server" Width="100px" Font-Size="12px">
                                                    <asp:ListItem Value="G">Generate</asp:ListItem>
                                                    <asp:ListItem Value="D">Delete</asp:ListItem>
                                                </asp:DropDownList></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" colspan="3">
                                    <asp:Button ID="btnGenerate" runat="server" CssClass="btnUpdate" Height="20px" Text="Generate"
                                        Width="101px" OnClick="btnGenerate_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                            <ProgressTemplate>
                                <div id='Div1' style='position: absolute; font-family: arial; font-size: 30; left: 50%;
                                    top: 50%; background-color: white; layer-background-color: white; height: 80;
                                    width: 150;'>
                                    <table width='100' height='35' border='1' cellpadding='0' cellspacing='0' bordercolor='#C0D6E4'>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td height='25' align='center' bgcolor='#FFFFFF'>
                                                            <img src='/assests/images/progress.gif' width='18' height='18'></td>
                                                        <td height='10' width='100%' align='center' bgcolor='#FFFFFF'>
                                                            <font size='1' face='Tahoma'><strong align='center'>Please Wait..</strong></font></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
            
              
            </table>
            <table>
                <tr>
                    <td>
                        <table style="display: none;" id="Tab_DisplayNoAction">
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                        <ContentTemplate>
                                            <table width="100%" border="1">
                                                <tr>
                                                    <td>
                                                        <div id="Divdisplay" runat="server">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnGenerate" EventName="Click"></asp:AsyncPostBackTrigger>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table id="Tab_BtnRemainingBill" style="display: none;">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <tr>
                                        <td>
                                            <asp:Button ID="BtnRemainingBill" runat="server" CssClass="btnUpdate" Height="20px"
                                                Text="Generate Remaining Bill" Width="150px" OnClick="BtnRemainingBill_Click" />
                                        </td>
                                    </tr>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="BtnRemainingBill" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </table>
                    </td>
                </tr>
            </table>
            
        </div>
</asp:Content>
