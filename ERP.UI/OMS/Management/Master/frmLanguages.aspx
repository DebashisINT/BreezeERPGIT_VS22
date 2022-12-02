<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/PopUp.Master" Inherits="ERP.OMS.Management.Master.management_master_frmLanguages" Codebehind="frmLanguages.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div>
        <table style="width: 123px">
            <tr>
                <td>
                     <%ShowList(); %>
                </td>
                <td style="vertical-align:top; text-align:center">
                    <asp:Button ID="BtnAdd" runat="server" Text="Add" OnClick="BtnAdd_Click" />
                </td>
            </tr>
            <tr>
                
            </tr>
        </table>
    </div>
  </asp:Content>