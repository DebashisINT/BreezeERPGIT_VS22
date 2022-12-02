<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.management_InterestRateSlabPopup" CodeBehind="InterestRateSlabPopup.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>--%>
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script language="javascript" type="text/javascript">

        function noNumbers(e) {
            var keynum
            var keychar
            var numcheck

            if (window.event)//IE
            {
                keynum = e.keyCode
                if (keynum >= 48 && keynum <= 57 || keynum == 46) {
                    return true;
                }
                else {
                    alert("Please Insert Numeric Only");
                    return false;
                }
            }

            else if (e.which) // Netscape/Firefox/Opera
            {
                keynum = e.which

                if (keynum >= 48 && keynum <= 57 || keynum == 46) {
                    return true;
                }
                else {
                    alert("Please Insert Numeric Only");
                    return false;
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table border="10" cellpadding="1" cellspacing="1">

            <tr>
                <td class="gridcellleft" bgcolor="#B7CEEC">Slab Code:</td>
                <td>
                    <dxe:ASPxTextBox runat="server" Width="150px" ID="txtslabcode">
                    </dxe:ASPxTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <table border="10" cellpadding="1" cellspacing="1">
                        <tr>
                            <td class="gridcellleft" bgcolor="#B7CEEC">Amnt From:</td>
                            <td>
                                <asp:TextBox runat="server" Width="150px" ID="txtmin" onkeypress="return noNumbers(event)"
                                    MaxLength="19"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table border="10" cellpadding="1" cellspacing="1">
                        <tr>
                            <td class="gridcellleft" bgcolor="#B7CEEC">Amnt To:</td>
                            <td>
                                <asp:TextBox runat="server" Width="150px" MaxLength="19" ID="txtmax" onkeypress="return noNumbers(event)"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="gridcellleft" bgcolor="#B7CEEC">Rate:</td>
                <td>
                    <asp:TextBox runat="server" Width="150px" ID="txtrate" onkeypress="return noNumbers(event)"
                        MaxLength="19"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnSave" runat="server" Style="cursor: pointer" CssClass="btnupdate"
                        Text="Save" OnClientClick="return aa();" OnClick="btnSave_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
