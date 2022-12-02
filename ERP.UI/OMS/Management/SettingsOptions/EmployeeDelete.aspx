<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.SettingsOptions.management_SettingsOptions_EmployeeDelete" CodeBehind="EmployeeDelete.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script  type="text/javascript">
        FieldName = 'btnSave';

        function CallList(obj1, obj2, obj3) {
            var obj5 = '';
            if (obj5 != '18') {
                ajax_showOptions(obj1, obj2, obj3, obj5);
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Delete Employee</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td style="vertical-align: top; padding-left: 30px; padding-bottom: 15px;">
                    <div class="crossBtn"><a href="../ProjectMainPage.aspx"><i class="fa fa-times"></i></a></div>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:TextBox ID="txtReportTo" runat="server" Width="300px" TabIndex="7"></asp:TextBox>
                    <asp:HiddenField ID="txtReportTo_hidden" runat="server" />
                </td>
            </tr>
            <tr>
                <td align="left" valign="top">
                    <asp:Button ID="btnSave" runat="server" Text="Delete Employee" CssClass="btnUpdate btn btn-primary" OnClick="btnSave_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
