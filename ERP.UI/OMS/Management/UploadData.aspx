<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.management_UploadData" CodeBehind="UploadData.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script lang="javascript" type="text/javascript">
        function validation_OK() {
            if (document.getElementById('txtTarget').value == "") {
                alert("Target DataBase field should not be blank");
                document.getElementById('txtTarget').focus();
                return false;
            }
            return true;
        }
        function validation_write_OK() {
            if (document.getElementById('txtSource').value == "") {
                alert("Source DataBase field should not be blank");
                document.getElementById('txtSource').focus();
                return false;
            }

            return true;
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <br />
        <table class="TablaMain">
            <tr>
                <td style="color: White">Create XML</td>
                <td>
                    <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="xml Create" /></td>
            </tr>
            <tr>
                <td style="color: White">Insert Main Account</td>
                <td>
                    <asp:FileUpload ID="FileUpload1" runat="server" />
                    <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="Data Enter" /></td>
            </tr>
            <tr>
                <td style="color: White">Insert Sub Account</td>
                <td>
                    <asp:FileUpload ID="FileUpload2" runat="server" />
                    <asp:Button ID="Button1" runat="server" Text="Data Enter" OnClick="Button1_Click" /></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td></td>
            </tr>
        </table>
        <table class="TableMain">
            <tr>
                <td class="EHEADER" colspan="4" style="text-align: center">
                    <strong><span id="spanHeader" style="color: #000099">Create XML For Incremental Data Upgrade</span></strong>
                </td>
            </tr>
            <tr>
                <td width="10%" align="right">Create XML File:
                </td>
                <td width="40%">&nbsp;<asp:Button ID="Button4" OnClientClick="javascript:return validation_write_OK()" runat="server" OnClick="Create_IncrementalXML" Text=" XML Create" /></td>
                <td width="10%" align="right">&nbsp;</td>
                <td width="40%">&nbsp;</td>
            </tr>
            <%-- <tr  visible="false">
            <td align="right" width="10%"  visible="false">
                Enter Target DataBase name :</td>
            <td width="40%">
                <asp:TextBox ID="txtTarget" TextMode="MultiLine"  Rows="1" runat="server" width="95%"></asp:TextBox></td>
            <td align="right" width="10%">
            </td>
            <td width="40%">
                <asp:Button ID="Button5" runat="server" OnClientClick="javascript:return validation_OK()" OnClick="Read_IncrementalXML" Text="Incremental XML Read" /></td>
        </tr>--%>
            <tr>
                <td colspan="4" align="center">
                    <asp:TextBox ID="txtSource" TextMode="MultiLine" Rows="1" runat="server" Width="95%" Visible="False"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="4" align="center"></td>
            </tr>
            <%--<tr>
            <td colspan="4" align="center"><asp:Button ID="btnTest" runat="server" OnClick="Test" Text="Test" /></td>
        </tr>--%>
            <%--<tr>
            <td colspan = "4" align="center">
                <asp:GridView ID="GridView1" runat="server" Width="90%">
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan = "4" align="center">
                <asp:GridView ID="GridView2" runat="server" Width="90%">
                </asp:GridView>
            </td>
        </tr>--%>
        </table>
    </div>
</asp:Content>
