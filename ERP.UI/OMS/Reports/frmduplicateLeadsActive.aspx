<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_frmduplicateLeadsActive" CodeBehind="frmduplicateLeadsActive.aspx.cs" %>



<%--<%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--     <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />
      <script type="text/javascript" src="/assests/js/loaddata1.js"></script>--%>
    <%--<script language="javascript" type="text/javascript">
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
            <h3>Duplicate Lead Report</h3>
        </div>
    </div>
     <div class="crossBtn"><a href="../Management/ProjectMainPage.aspx"><i class="fa fa-times"></i></a></div>
    <div class="form_main">
        <table class="TableMain100">
            <%--            <tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">Duplicate Lead Report</span></strong>
                </td>
            </tr>--%>
            <tr>
                <td align="center">
                    <%--<CR:crystalreportviewer id="CrystalReportViewer1" runat="server" autodatabind="true" displaygrouptree="False" />--%>

                </td>
            </tr>
        </table>
    </div>
</asp:Content>
