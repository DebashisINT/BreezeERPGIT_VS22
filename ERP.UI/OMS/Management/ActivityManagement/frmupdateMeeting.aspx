<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ActivityManagement.management_activitymanagement_frmupdateMeeting" CodeBehind="frmupdateMeeting.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../CSS/style.css" rel="stylesheet" type="text/css" />
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
                <td colspan="3" align="center">
                    <asp:Label ID="lblName" runat="Server" ForeColor="Navy" Font-Bold="True" CssClass="Ecoheadtxt"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="mylabel1" style="width: 18%" align="left">Next Visit DateTime:</td>
                <td class="lt">&nbsp;&nbsp;<dxe:ASPxDateEdit ID="VisitDate" runat="server"
                    EditFormat="Custom"
                    UseMaskBehavior="True" Width="180px">
                    <ButtonStyle Width="13px" />
                </dxe:ASPxDateEdit>
                </td>
            </tr>
            <tr>
                <td class="mylabel1" style="width: 18%">Note:</td>
                <td class="lt">
                    <asp:TextBox ID="txtNote" runat="Server" TextMode="multiLine" Width="739px"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="3" align="Center">
                    <asp:Button ID="btnSave" runat="Server" Text="Save" CssClass="btnUpdate" OnClick="btnSave_Click" />
                    <input type="button" id="btnClose" name="btnClose" class="btnUpdate"
                        value="Close" onclick="closeWin();" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
