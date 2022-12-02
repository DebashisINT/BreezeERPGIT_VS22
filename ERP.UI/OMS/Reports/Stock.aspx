<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_Stock" CodeBehind="Stock.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:GridView ID="grdStocks" runat="server" Width="100%" BorderColor="CornflowerBlue"
            ShowFooter="True" AllowSorting="true" AutoGenerateColumns="false" BorderStyle="Solid"
            BorderWidth="2px" CellPadding="4" ForeColor="#0000C0">
            <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
            <Columns>
                <asp:TemplateField HeaderText="A/c Number">
                    <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("ACName")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Settlement">
                    <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Settlement")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ISIN">
                    <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblDeliveredFrom" runat="server" Text='<%# Eval("DematStocks_ISIN")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Quantity">
                    <ItemStyle BorderWidth="1px" HorizontalAlign="right"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblDeliveredFrom" runat="server" Text='<%# Eval("Qty")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Pledge/Lock In">
                    <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="right"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblDeliveredTo" runat="server" Text='<%# Eval("Pledge")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Free Balance">
                    <ItemStyle BorderWidth="1px" HorizontalAlign="right"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblDeliveredFrom" runat="server" Text='<%# Eval("FreeBalance")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <RowStyle BackColor="#FFFFFF" ForeColor="#330099" BorderColor="#BFD3EE" BorderStyle="Double"
                BorderWidth="1px"></RowStyle>
            <EditRowStyle BackColor="#E59930"></EditRowStyle>
            <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
            <PagerStyle ForeColor="White" HorizontalAlign="Center"></PagerStyle>
            <HeaderStyle ForeColor="Black" BorderWidth="1px" CssClass="EHEADER" BorderColor="AliceBlue"
                Font-Bold="False"></HeaderStyle>
            <%--<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>--%>
        </asp:GridView>
    </div>
</asp:Content>
