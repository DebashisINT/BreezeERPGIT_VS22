<%@ Page Language="C#" AutoEventWireup="True" CodeFile="frm_attendance_Lock.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    CodeBehind="frm_attendance_Lock.aspx.cs" Inherits="ERP.OMS.Management.ToolsUtilities.management_utilities_frm_attendance_Lock" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--     
    --%>
    <script type="text/javascript" language="javascript">
        function pagecall(obj) {
            if (obj == 'A') {
                IfreameAtt.location = 'frm_attendance_Lock_iframe.aspx';
            }
            else {
                IfreameAtt.location = 'frm_attendance_PD_calculation.aspx';
            }
        }


        //function SignOff() {
        //    window.parent.SignOff()
        //}
        //function height() {
        //    if (document.body.scrollHeight > 500)
        //        window.frameElement.height = document.body.scrollHeight;
        //    else
        //        window.frameElement.height = '500px';
        //    window.frameElement.widht = document.body.scrollWidht;
        //}

    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>
                <asp:Label ID="lblHead" runat="server" Font-Bold="True"></asp:Label></h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td style="vertical-align: top; padding-left: 30px;">
                    <div class="crossBtn"><a href='<%= Page.ResolveUrl("~/OMS/Management/ProjectMainPage.aspx")%>'><i class="fa fa-times"></i></a></div>
                </td>
            </tr>
            <tr>
                <td>
                   <%-- <iframe id="IfreameAtt" name="IfreameAtt" scrolling="no" src="" frameborder="0" width="100%">
                </iframe>--%>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
