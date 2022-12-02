<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_mtest1" CodeBehind="mtest1.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:GridView ID="DepartmentsGridView" runat="server" AllowPaging="True" AllowSorting="True"
                    AutoGenerateColumns="False" CellPadding="4" DataSourceID="DepartmentDataSource"
                    ForeColor="#333333" GridLines="None" PageSize="3" OnRowDataBound="DepartmentsGridView_RowDataBound">
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <Columns>
                        <asp:BoundField DataField="GroupName" HeaderText="Division" SortExpression="GroupName">
                            <ItemStyle Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Name" HeaderText="Department Name" SortExpression="Name">
                            <ItemStyle Width="160px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Employees">
                            <ItemTemplate>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="EmployeesGridView" runat="server" AllowPaging="True" AllowSorting="True"
                                            AutoGenerateColumns="False" BackColor="White" BorderColor="#999999" BorderStyle="None"
                                            BorderWidth="1px" CellPadding="3" DataSourceID="EmployeesDataSource" GridLines="Vertical"
                                            PageSize="4">
                                            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                            <Columns>
                                                <asp:BoundField DataField="FirstName" HeaderText="First Name" SortExpression="FirstName">
                                                    <ItemStyle Width="80px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="LastName" HeaderText="Last Name" SortExpression="LastName">
                                                    <ItemStyle Width="160px" />
                                                </asp:BoundField>
                                            </Columns>
                                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                                            <AlternatingRowStyle BackColor="Gainsboro" />
                                        </asp:GridView>
                                        <asp:Label runat="server" ID="InnerTimeLabel"><%=DateTime.Now %></asp:Label>
                                        <asp:SqlDataSource ID="EmployeesDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:AdventureWorksConnectionString %>"
                                            SelectCommand="SELECT HumanResources.EmployeeDepartmentHistory.DepartmentID, 
                                                          HumanResources.vEmployee.EmployeeID, 
                                                          HumanResources.vEmployee.FirstName, 
                                                          HumanResources.vEmployee.LastName 
                                                   FROM HumanResources.EmployeeDepartmentHistory
                                                   INNER JOIN HumanResources.vEmployee 
                                                     ON HumanResources.EmployeeDepartmentHistory.EmployeeID = HumanResources.vEmployee.EmployeeID
                                                   WHERE HumanResources.EmployeeDepartmentHistory.DepartmentID = @DepartmentID
                                                   ORDER BY HumanResources.vEmployee.LastName ASC, HumanResources.vEmployee.FirstName ASC">
                                            <SelectParameters>
                                                <asp:Parameter Name="DepartmentID" DefaultValue="0" Type="int32" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ItemTemplate>
                            <ItemStyle Height="170px" Width="260px" />
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" VerticalAlign="Top" />
                    <EditRowStyle BackColor="#999999" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" VerticalAlign="Top" />
                </asp:GridView>
                <asp:Label runat="server" ID="OuterTimeLabel"><%=DateTime.Now %></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
        &nbsp;
        <asp:SqlDataSource ID="DepartmentDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:AdventureWorksConnectionString %>"
            SelectCommand="SELECT DepartmentID, Name, GroupName FROM HumanResources.Department ORDER BY Name"></asp:SqlDataSource>


    </div>
</asp:Content>
