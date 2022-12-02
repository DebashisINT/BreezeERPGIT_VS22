<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.management_CRMPhoneCalls" CodeBehind="CRMPhoneCalls.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />
    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>
    <script language="javascript" type="text/javascript">
        function SignOff() {
            window.parent.SignOff()
        }
        function height() {

            if (document.body.scrollHeight >= 550) {
                window.frameElement.height = document.body.scrollHeight;
            }
            else {
                window.frameElement.height = '550px';
            }
            window.frameElement.widht = document.body.scrollWidht;

        }

        function CallEmail(val) {

            var frm = "frmSendMailForPhoneCall.aspx?id=" + val;
            editwin = dhtmlmodal.open("Editbox", "iframe", frm, "Send Email", "width=950px,height=500px,center=1,resize=1,scrolling=2,top=500", "recal");
            editwin.onclose = function () {

                document.getElementById("f1").contentWindow.iframesource();
            }


        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td style="width: 100%">
                    <iframe id="f1" src="CRMPhoneCallWithFrame.aspx" style="width: 100%;" frameborder="0"
                        scrolling="no"></iframe>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
