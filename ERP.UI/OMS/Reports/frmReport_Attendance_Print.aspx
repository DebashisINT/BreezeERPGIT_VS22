<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_Attendance_Print" Codebehind="frmReport_Attendance_Print.aspx.cs" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <table>
            <tr>
                <td>
                    <cr:crystalreportviewer id="CrystalReportViewer1" runat="server" autodatabind="true"
                        displaygrouptree="False" />
                    <cr:crystalreportpartsviewer id="CrystalReportPartsViewer1" runat="server" autodatabind="True"
                        reportsourceid="CrystalReportSource1" />
                    <cr:crystalreportsource id="CrystalReportSource1" runat="server">
                                    <Report FileName="Reports\AttendenceRport.rpt">
                                    </Report>
                                </cr:crystalreportsource>
                </td>
            </tr>
        </table>
</asp:Content>