<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_CommissionReceivableReport_print" Codebehind="frmReport_CommissionReceivableReport_print.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" language="javascript">
        function printSpecial()
        {
            window.print();
        }



    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <table class="TableMain10">
            <tr>
                <td>
                    <table class="TableMain10">
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td class="gridcellleft">
                                            From date:&nbsp;<asp:Label ID="lblFromDate" runat="server" ForeColor="#0000C0"></asp:Label>
                                        </td>
                                        <td class="gridcellleft">
                                            To date:&nbsp;<asp:Label ID="lblToDate" runat="server" ForeColor="#0000C0"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="text-align: right">
                                <a id="print" href="javascript:printSpecial();"><span style="font-size: 8pt; color: #3300cc;
                                    text-decoration: underline; cursor: pointer">Print This Report</span></a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="printReady" runat="server">
                    </div>
                </td>
            </tr>
        </table>
</asp:Content>
