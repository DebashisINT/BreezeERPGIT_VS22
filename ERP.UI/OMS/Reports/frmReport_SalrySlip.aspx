<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.frmReport_SalrySlip" Codebehind="frmReport_SalrySlip.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script language="javascript" type="text/javascript">
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

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
     <table class="TableMain100">
                <tr>
                    <td class="EHEADER" style="text-align: center;" colspan="3">
                        <strong><span style="color: #000099">Salary & Reimbursment</span></strong>
                    </td>
                </tr>
                <tr>
                <td align="right" style="height: 37px">
                    &nbsp;<dxe:ASPxDateEdit ID="txtDOB" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                            TabIndex="9" Width="202px" Date="2010-09-01" EditFormatString="MMMM-yyyy">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                        <CalendarProperties DayNameFormat="Shortest">
                        </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                        </td>
                                        <td style="height: 37px"><asp:Button ID="btnSalarySlip" runat="server" Text="Get SalarySlip" CssClass="btnUpdate" OnClick="btnSalarySlip_Click" />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnReimbursment" runat="server" Text="Get Reimbursment" CssClass="btnUpdate" OnClick="btnReimbursment_Click"/></td>
                                           
                                            
                
                </tr>
               
                </table>
    </div>
</asp:Content>
