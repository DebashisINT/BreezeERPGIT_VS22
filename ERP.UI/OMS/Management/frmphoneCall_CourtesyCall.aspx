<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.management_frmphoneCall_CourtesyCall" Codebehind="frmphoneCall_CourtesyCall.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="stylesheet" href="../CSS/dhtmlgoodies_calendar.css?random=20051112" media="screen" />
    <script type="text/javascript" src="/assests/js/dhtmlgoodies_calendar.js?random=20060118"></script>
    <script language="javascript" type="text/javascript">
        function funJScript(obj) {
            var st = obj.value.split("!!")
            var ob = document.getElementById("tdvisitdate")
            if (st[1] == 8) {
                ob.disabled = false;
            }
            else {
                ob.disabled = true;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain">
            <tr>
                <td colspan="3" align="center" style="text-align: left">
                    <asp:Label ID="lblName" runat="Server" ForeColor="Red" Font-Bold="True" CssClass="Ecoheadtxt" Width="350px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="Ecoheadtxt" style="width: 134px">Courtesy Call Outcome</td>
                <td>:</td>
                <td><asp:DropDownList ID="drpCourtesy" runat="Server" Width="230px" CssClass="Ecoheadtxt"></asp:DropDownList></td>
            </tr>
             <tr>
                <td class="Ecoheadtxt" style="width: 134px">Sales Visit Outcome</td>
                <td>:</td>
                <td><asp:DropDownList ID="drpSalesVisitOutcome" runat="Server" Width="230px" AppendDataBoundItems="True" CssClass="Ecoheadtxt">
                    <asp:ListItem Value="0">None</asp:ListItem>
                </asp:DropDownList></td>
            </tr>
            <tr>
                <td class="Ecoheadtxt" style="width: 134px">Note</td>
                <td>:</td>
                <td><asp:TextBox ID="txtNote" runat="Server" TextMode="MultiLine" Width="226px" Height="100px" Font-Size="12px" ForeColor="blue"></asp:TextBox></td>
            </tr>
            <tr id="tdvisitdate" disabled="true">
                <td style="width: 134px" class="Ecoheadtxt">Next Visit</td>
                <td>:</td>
                <td><asp:TextBox ID="VisitDate" runat="server"></asp:TextBox>
                    <asp:Image ID="ImgVisit" runat="server" ImageUrl="~/images/calendar.jpg" />
                </td>
            </tr>
             <tr>
                <td colspan="3" align="Center">
                    <asp:Button ID="btnSave" runat="Server" Text="Save" CssClass="btnUpdate" OnClick="btnSave_Click" />
                    <input type="button" id="btnClose" name="btnClose" value="Close" onclick="window.close();" class="btnUpdate" />
                </td>
            </tr>
        </table>
    </div>
   </asp:Content>