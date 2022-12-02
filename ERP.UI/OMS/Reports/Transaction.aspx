<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_Transaction" CodeBehind="Transaction.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:GridView ID="grdTransaction" runat="server" Width="100%" BorderColor="CornflowerBlue"
            ShowFooter="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="TranID" BorderStyle="Solid"
            BorderWidth="2px" CellPadding="4" ForeColor="#0000C0">
            <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
            <Columns>
                <asp:TemplateField HeaderText="Tran ID">
                    <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblTranID" runat="server" Text='<%# Eval("TranID")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Type">
                    <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblType" runat="server" Text='<%# Eval("Type")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Date">
                    <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Quantity">
                    <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quantity")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Delivered From">
                    <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblDeliveredFrom" runat="server" Text='<%# Eval("DeliveredFrom")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Settl.From ">
                    <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblSettNo" runat="server" Text='<%# Eval("DematTransactions_SettlementNumberS")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Delivered To">
                    <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblDeliveredTo" runat="server" Text='<%# Eval("DeliveredTo")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Settl.To">
                    <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblSettTarget" runat="server" Text='<%# Eval("DematTransactions_SettlementNumberT")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Slip Number">
                    <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblSlipNumber" runat="server" Text='<%# Eval("DematTransactions_SlipNumber")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Remarks">
                    <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("DematTransactions_Remarks")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <%-- <asp:TemplateField ShowHeader="False" Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete" OnClientClick="javascript:return confirm('Are You Want To Delete This Transaction');"
                                Text="Delete"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
            </Columns>
            <RowStyle BackColor="White" ForeColor="#330099" BorderColor="#BFD3EE" BorderStyle="Double"
                BorderWidth="1px"></RowStyle>
            <EditRowStyle BackColor="#E59930"></EditRowStyle>
            <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
            <PagerStyle ForeColor="White" HorizontalAlign="Center"></PagerStyle>
            <HeaderStyle ForeColor="Black" BorderWidth="1px" CssClass="EHEADER" BorderColor="AliceBlue"
                Font-Bold="False"></HeaderStyle>
        </asp:GridView>
    </div>
</asp:Content>

