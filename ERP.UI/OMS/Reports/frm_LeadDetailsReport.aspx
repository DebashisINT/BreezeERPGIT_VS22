<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frm_LeadDetailsReport" Codebehind="frm_LeadDetailsReport.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div class="panel-heading">
        <div class="panel-title">
            <h3> Lead Generation Report</h3>
        </div>
    </div>
     <div class="crossBtn"><a href="../Management/ProjectMainPage.aspx"><i class="fa fa-times"></i></a></div>
    <div class="form_main">
        <table class="TableMain100" style="width:596px">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Form" CssClass="mylabel1"></asp:Label></td>
                <td>
                    <dxe:aspxdateedit id="ASPxFromDate" runat="server" editformat="custom" usemaskbehavior="True">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:aspxdateedit>
                </td>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="To" CssClass="mylabel1"></asp:Label></td>
                <td>
                    <dxe:aspxdateedit id="ASPxToDate" runat="server" editformat="custom" usemaskbehavior="True">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:aspxdateedit>
                </td>
                <td>
                    <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="btn btn-success" OnClick="btnShow_Click" />
                    <asp:DropDownList ID="ddlExport" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged"
                        Width="113px">
                        <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                        <asp:ListItem Value="E">Excel</asp:ListItem>
                        <asp:ListItem Value="P">PDF</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td colspan="5">
                    <div id="MainContainer" runat="server" visible="false" style="border: solid 1px black">
                    </div>
                </td>
            </tr>
        </table>
        </div>
</asp:Content>