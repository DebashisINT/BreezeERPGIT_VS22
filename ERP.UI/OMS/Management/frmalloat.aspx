<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.management_frmalloat" CodeBehind="frmalloat.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function closeWin() {
            parent.editwin.close();
        }
        function close() {
            parent.editwin.close();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td class="mylabel1" style="width: 13%">Department:</td>

                <td>
                    <asp:DropDownList ID="drpDepartment" runat="Server" AutoPostBack="True" Width="300px" OnSelectedIndexChanged="drpDepartment_SelectedIndexChanged"></asp:DropDownList></td>
            </tr>
            <tr>
                <td class="mylabel1" style="width: 13%">Branch:</td>

                <td>
                    <asp:DropDownList ID="drpBranch" runat="Server" AutoPostBack="True" Width="300px" OnSelectedIndexChanged="drpBranch_SelectedIndexChanged"></asp:DropDownList></td>
            </tr>
            <tr>
                <td class="mylabel1" style="width: 13%">User:</td>

                <td>
                    <asp:DropDownList ID="drpUser" runat="Server" Width="300px"></asp:DropDownList></td>
            </tr>
            <tr>
                <td class="mylabel1" style="width: 13%">Insturction:</td>

                <td>
                    <asp:TextBox ID="TxtInstruction" runat="Server" TextMode="MultiLine" Width="756px" Height="200px" Font-Size="12px"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="3" align="Center">
                    <asp:Button ID="btnSave" runat="Server" Text="Allot" CssClass="btnUpdate" OnClick="btnSave_Click" />
                    <input type="button" id="btnClose" name="btnClose" value="Close" onclick="closeWin();" class="btnUpdate" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

