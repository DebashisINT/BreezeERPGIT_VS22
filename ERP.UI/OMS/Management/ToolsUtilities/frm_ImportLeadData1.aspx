<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_ToolsUtilities_frm_ImportLeadData1" CodeBehind="frm_ImportLeadData1.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--    <script language="javascript" type="text/javascript">
        function SignOff() {
            window.parent.SignOff();
        }
        function height() {
            if (document.body.scrollHeight >= 500)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '500px';
            window.frameElement.Width = document.body.scrollWidth;
        }
    </script>--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Import Leads</h3>
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
                <td class="Ecoheadtxt" style="width: 78px; height: 23px">Select File :
                </td>
                <td style="width: 243px; height: 23px; text-align: left">
                    <asp:FileUpload ID="FileUpload" runat="server" class="form-control" />
                </td>
                <td style="height: 23px; text-align: left; padding-left: 15px;">
                    <asp:Button ID="BtnSave" runat="server" Text="Import File" class="btn btn-success" OnClick="BtnSave_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
