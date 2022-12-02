<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    Inherits="ERP.OMS.Management.management_frm_WelcomeLetter" CodeBehind="frm_WelcomeLetter.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
        function height() {
            //         alert(document.body.scrollHeight);
            if (document.body.scrollHeight > 385) {
                window.frameElement.height = document.body.scrollHeight;
            }
            else {
                window.frameElement.height = "385";
            }
            window.frameElement.Width = document.body.scrollWidth;
        }
        function callDhtmlFormsParent() {
            OnMoreInfoClick("frm_ReservedWordsForMessage.aspx", "ADD RESERVED WORDS", "950px", "500px", "Y");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="ColumnBackColor" style="width: 50%; border: solid 1px maroon">
        <tr>
            <td class="MylabelMaroon">
                <asp:Label ID="Label1" runat="server" Text="Template For:"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="dpTemplate" runat="server" Width="300px">
                    <asp:ListItem Text="Employee" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Customer" Value="1"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="MylabelMaroon">
                <asp:Label ID="Label2" runat="server" Text="Message:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtMessage" runat="server" Height="174px" Width="298px"></asp:TextBox>
            </td>
        </tr>
        <tr class="rt">
            <td colspan="2" style="text-decoration: underline">
                <asp:HyperLink ID="HpWord" runat="server" ForeColor="blue" href="javascript:void(0);"
                    onclick="callDhtmlFormsParent()">Use Reserved Words</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td class="lt">
                <asp:Button ID="btnSave" runat="server" Text="Save" class="btnUpdate" />
            </td>
        </tr>
    </table>
</asp:Content>

