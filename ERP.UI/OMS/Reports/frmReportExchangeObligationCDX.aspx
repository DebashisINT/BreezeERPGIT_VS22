<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_frmReportExchangeObligationCDX" Codebehind="frmReportExchangeObligationCDX.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="../CSS/style.css" rel="stylesheet" type="text/css" />
  <style type="text/css">
  
	.grid_scroll
    {
        overflow-y: no;  
        overflow-x: scroll; 
        width:90%;
        scrollbar-base-color: #C0C0C0;
    
    }
	</style>
    <script language="javascript" type="text/javascript">
        function Page_Load()///Call Into Page Load
        {
            Hide('tr_grdexchange');
            Hide('tr_export');
            height();

        }
        function Hide(obj) {
            document.getElementById(obj).style.display = 'none'
        }
        function Show(obj) {
            document.getElementById(obj).style.display = 'inline'
        }
        function height() {
            if (document.body.scrollHeight >= 350) {
                window.frameElement.height = document.body.scrollHeight;
            }
            else {
                window.frameElement.height = '350px';
            }
            window.frameElement.width = document.body.scrollwidth;
        }
        function NORECORD(obj) {
            Show('tr_date');
            Show('tr_btn');
            Hide('tr_grdexchange');
            Hide('tr_export');
            if (obj == '1')
                alert('No Record Found !!');
            if (obj == '2')
                alert('Rates For This Date Does Not Exists !!');
            height();
        }
        function Display() {
            Show('tr_grdexchange');
            Show('tr_export');
            Hide('tr_date');
            Hide('tr_btn');
            document.getElementById('display').className = "grid_scroll";
            height();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
     <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3600">
            </asp:ScriptManager>

        <script language="javascript" type="text/javascript">
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_initializeRequest(InitializeRequest);
            prm.add_endRequest(EndRequest);
            var postBackElement;
            function InitializeRequest(sender, args) {
                if (prm.get_isInAsyncPostBack())
                    args.set_cancel(true);
                postBackElement = args.get_postBackElement();
                $get('UpdateProgress1').style.display = 'block';
            }
            function EndRequest(sender, args) {
                $get('UpdateProgress1').style.display = 'none';
            }
        </script>
<table class="TableMain100"> <tr>
            <td class="EHEADER" colspan="0" style="text-align:center; height: 22px;">
                <strong><span id="SpanHeader" style="color: #000099">Exchange Obligation</span></strong>
            </td>
            <td class="EHEADER" width="25%" id="tr_export" style="height: 22px">
                                    <a href="javascript:void(0);" onclick="NORECORD(3);"><span
                        style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight:bold ">Filter</span></a>
                &nbsp; &nbsp;<asp:DropDownList ID="ddlExport" Font-Size="Smaller" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                            <asp:ListItem Value="E">Excel</asp:ListItem>
                            <asp:ListItem Value="P">PDF</asp:ListItem>
                        </asp:DropDownList>

             </td>
             
        </tr></table>
        <table>
            <tr id="tr_date">
                <td valign="top" >
                    <table>
                        <tr>
                            <td valign="top" class="gridcellleft" bgcolor="#B7CEEC">
                                For :</td>
                            <td>
                                <dxe:ASPxDateEdit ID="dtFor" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Font-Size="12px" Width="108px" ClientInstanceName="dtfor">
                                    <DropDownButton Text="For">
                                    </DropDownButton>
                                    
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
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
            <tr id="tr_btn">
                <td>
                    <asp:Button ID="btn_show" runat="server" CssClass="btnUpdate" Height="20px" Text="Show"
                        Width="101px" OnClientClick="selecttion()" OnClick="btn_show_Click1" />
                </td>
            </tr>
           
            <tr id="tr_grdexchange">
                <td  >
                   <div id="displayAll" >
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <table width="100%" border="1">
                        
                        <tr id="tr_DIVdisplayPERIOD">
                            <td>
                                <div id="DIVdisplayPERIOD" runat="server">
                                </div>
                            </td>
                        </tr>
                    
                        <tr>
                            <td>
                                <div id="display" runat="server">
                                </div>
                            </td>
                        </tr>
                      
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btn_show" EventName="Click"></asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
        </div>
                </td>
                
            </tr>
        </table>
    </div>
</asp:Content>
