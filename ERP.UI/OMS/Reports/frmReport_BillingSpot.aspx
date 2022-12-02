<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_BillingSpot" Codebehind="frmReport_BillingSpot.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script language="javascript" type="text/javascript">
    
    function DateChange()
             {
                var sessionVal ="<%=Session["LastFinYear"]%>";
                var objsession=sessionVal.split('-');
                var MonthDate=dteDate.GetDate().getMonth()+1;
                var DayDate=dteDate.GetDate().getDate();
                var YearDate=dteDate.GetDate().getYear();
                var exd = new Date();
                if(YearDate>=objsession[0])
                {
                    if(MonthDate<4 && YearDate==objsession[0])
                    {
                        alert('Enter Date Is Outside Of Financial Year !!');
                        var datePost=((exd.getMonth()+1)+'-'+exd.getDate()+'-'+objsession[1]);
                        dteDate.SetDate(new Date(datePost));
                    }
                    else if(MonthDate>3 && YearDate==objsession[1])
                    {
                        alert('Enter Date Is Outside Of Financial Year !!');
                        var datePost=((exd.getMonth()+1)+'-'+exd.getDate()+'-'+objsession[0]);
                        dteDate.SetDate(new Date(datePost));
                    }
                    else if(YearDate!=objsession[0] && YearDate!=objsession[1])
                    {
                        alert('Enter Date Is Outside Of Financial Year !!');
                        var datePost;
                        if(MonthDate<4)
                            datePost=((exd.getMonth()+1)+'-'+exd.getDate()+'-'+objsession[1]);
                        else
                            datePost=((exd.getMonth()+1)+'-'+exd.getDate()+'-'+objsession[0]);
                        dteDate.SetDate(new Date(datePost));
                    }
                }
                else
                {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost;
                    if(MonthDate<4)
                        datePost=((exd.getMonth()+1)+'-'+exd.getDate()+'-'+objsession[1]);
                    else
                        datePost=((exd.getMonth()+1)+'-'+exd.getDate()+'-'+objsession[0]);
                    dteDate.SetDate(new Date(datePost));
                }
                
             }
    
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
          divscroll(sessionvalue);
   } 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"  AsyncPostBackTimeout="36000">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table class="TableMain100">
                        <tr>
                            <td align="center">
                                <table>
                                    <tr>
                                        <td class="EcoheadCon_">
                                            Date :</td>
                                        <td style="text-align: left">
                                            <dxe:ASPxDateEdit ID="dtDate" runat="server" EditFormat="Custom" ClientInstanceName="dteDate" UseMaskBehavior="True">
                                                <DropDownButton Text="From">
                                                </DropDownButton>
                                                <ClientSideEvents DateChanged="function(s,e){DateChange(); }" />
                                            </dxe:ASPxDateEdit>
                                        </td>
                                    </tr>
                                    <%--<tr>
                                        <td class="EcoheadCon_">
                                            Day Wise :</td>
                                        <td style="text-align: left">
                                            <asp:DropDownList ID="ddlDaywise" runat="server" Width="200px">
                                                <asp:ListItem Value="1">Consolidated Entry</asp:ListItem>
                                                <asp:ListItem Value="2">Separate Entries For Charges</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td class="EcoheadCon_">
                                            Generate/Delete :</td>
                                        <td style="text-align: left">
                                            <asp:DropDownList ID="ddlGenerate" runat="server" Width="200px">
                                                <asp:ListItem Value="1">Generate</asp:ListItem>
                                                <asp:ListItem Value="2">Delete</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align: left">
                                            <asp:Button ID="btnGenerate" runat="server" Text="Generate" CssClass="btnUpdate"
                                                OnClick="btnGenerate_Click" Height="24px" Width="70px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <table>
                <tr>
                    <td>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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
                                                            <font size='2' face='Tahoma'><strong align='center'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Loading..</strong></font></td>
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
        </div>
</asp:Content>
