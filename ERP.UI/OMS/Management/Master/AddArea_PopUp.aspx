<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/PopUp.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_AddArea_PopUp" CodeBehind="AddArea_PopUp.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table border="0" class="TableMain100">
            <tr>
                <td class="grdcellright" style="padding-left:15px;padding-top:20px;">City &nbsp;Name</td>
                <td class="grdcellleft">
                    <asp:Label ID="lblCity" runat="server" Width="155px"></asp:Label></td>
            </tr>
            <tr>
                <td class="grdcellright" style="padding-left:15px">Area &nbsp;Name</td>
                <td class="grdcellleft">&nbsp;<asp:TextBox ID="txtArea" runat="server" Width="130px"></asp:TextBox>
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtArea" ValidationGroup="ar" SetFocusOnError="true" ToolTip="Please enter area name" class="pullrightClass fa fa-exclamation-circle abs" ErrorMessage="" ForeColor="red"></asp:RequiredFieldValidator>
                </td>

            </tr>
            <tr>
                <td></td>
                <td>&nbsp;
                    <asp:Button ID="btnNewSave" runat="server" Text="Save" OnClick="btnNewSave_Click" CssClass="btn btn-primary" ValidationGroup="ar" />
                    <%--<dxe:ASPxButton ID="btnSave" ClientInstanceName="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CausesValidation="false">
                    <ClientSideEvents Click="function(s, e) {	fnCLose();}" />
                </dxe:ASPxButton>--%>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                        SelectCommand="SELECT [city_id], [city_name] FROM [tbl_master_city] ORDER BY [city_name]"></asp:SqlDataSource>
                </td>
            </tr>
        </table>
    </div>

</asp:Content>
