<%@ Page Title="Print tag" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Management_PrintTag" CodeBehind="PrintTag.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        Upload Excel file:
        <asp:FileUpload runat="server" ID="file" />
        <asp:Button runat="server" Text="Load" CssClass="btn btn-primary" ID="btnLoad" OnClick="btnLoad_Click" />
        <asp:GridView ID="grdActive" runat="server" Width="100%" Height="100%" BorderColor="CornflowerBlue"
            ShowFooter="True" AutoGenerateColumns="false" BorderStyle="Solid"
            BorderWidth="2px" CellPadding="4" ForeColor="#0000C0">
            <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
            <Columns>

                <asp:TemplateField HeaderText="Sl.No" Visible="true">
                    <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                    <ItemTemplate>
                        <asp:Label ID="Ids" runat="server" Text='<%# Eval("Id")%>'></asp:Label>
                        <asp:HiddenField ID="hidId" runat="server" Value='<%# Eval("Id")%>'></asp:HiddenField>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Pattern" Visible="true">
                    <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblPattern" runat="server" Text='<%# Eval("Pattern")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Quality" SortExpression="Quality">
                    <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblRepeat" runat="server" Text='<%# Eval("Quality")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Color" SortExpression="Color">
                    <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblColor" runat="server" Text='<%# Eval("Color")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Repeat" Visible="true">
                    <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblpRepeat" runat="server" Text='<%# Eval("Repeat")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Width" SortExpression="Width">
                    <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblWidth" runat="server" Text='<%# Eval("Width")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Content" SortExpression="Width">
                    <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblContent" runat="server" Text='<%# Eval("Content")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Weight_per_Mtr " SortExpression="Weight_per_Mtr" Visible="false">
                    <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                    <ItemTemplate>
                        <%--   <asp:Label ID="lblproduct" runat="server" Text='<%# Eval("Weight_per_Mtr")%>'></asp:Label>--%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Select" SortExpression="Select">
                    <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chkActive" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <RowStyle BackColor="White" ForeColor="#330099" BorderColor="#BFD3EE" BorderStyle="Double"
                BorderWidth="1px"></RowStyle>
            <EditRowStyle BackColor="#E59930"></EditRowStyle>
            <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
            <PagerStyle ForeColor="Blue" HorizontalAlign="Center"></PagerStyle>
            <HeaderStyle ForeColor="Black" BorderWidth="1px" CssClass="EHEADER" BorderColor="AliceBlue"
                Font-Bold="False"></HeaderStyle>
        </asp:GridView>
        <br />
        <div style="float: right">
            <asp:Button runat="server" ID="btnPrint" CssClass="btn btn-primary" OnClick="btnPrintAll_Click" Text="Print Selected" /></div>
    </div>
</asp:Content>

