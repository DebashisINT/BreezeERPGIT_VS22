<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" 
    Inherits="ERP.OMS.Management.management_frm_Holiday_popup" Codebehind="frm_Holiday_popup.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <table style="border: solid 4px #DDECFE;" cellspacing="0px" cellpadding="0px">
            <tr>
                <td>
                    <table style="width: 100%" cellspacing="0px" style="border: solid 1px blue;">
                        <tr>
                            <td style="width: 90%; border-right: solid 1px blue;">
                                <%ShowList(); %>
                            </td>
                            <td style="vertical-align: top; text-align: center">
                                <asp:Button ID="BtnAdd" runat="server" Text="Add" OnClick="BtnAdd_Click" />
                            </td>
                        </tr>
                        <tr>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
</asp:Content>
