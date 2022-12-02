<%@ Page Language="C#" AutoEventWireup="true" Inherits="Reset" Codebehind="Reset.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Reset LogIN</title>
    <link type="text/css" href="CSS/style.css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td class="lt" width="25%" style="height: 22px">
                    LogIn ID :
                </td>
                <td>
                    <asp:TextBox ID="txtLogIN" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="lt" width="25%" style="height: 22px">
                    Password :
                </td>
                <td>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="149px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btnUpdate" OnClick="btnReset_Click"/>
                </td>
            </tr>
            <%--<tr>
                <td colspan="2">
                    <iframe id="Iframe2" src="management/frmshowreminder1.aspx" width="1" height="1"
                    frameborder="0" scrolling="no" style="width: 1px; height: 1px"></iframe>
                </td>
            </tr>--%>
        </table>
    </div>
    </form>
</body>
</html>
