<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.management_frm_AccountSummary" CodeBehind="frm_AccountSummary.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .floatLeft {
            float: left;
        }

        .floatRight {
            float: right;
        }
    </style>
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript">
        function SignOff()
        {
            window.parent.SignOff();
        }
        function height()
        {
            if(document.body.scrollHeight>=500)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '500px';
            window.frameElement.Width = document.body.scrollWidth;
        }
        function aaa(obj,evt,uid)
        {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :((evt.which) ? evt.which : 0));
            if((charCode != 9)&& (charCode != 8))
            {      
                var CurrentTextBox_Value=uid.id.value;
                var CurrentTextBox_id = uid.id;
                var txtF_Char=CurrentTextBox_id.charAt(0);
                if(txtF_Char=='L')
                {
                    var RespectiveTextBox_id=CurrentTextBox_id.replace(/\L/,"R");
                    alert(RespectiveTextBox_id);
                } 
                else
                { 
                    var RespectiveTextBox_id=CurrentTextBox_id.replace(/\R/,"L");
                    alert(RespectiveTextBox_id);
                    var dd=document.getElementById(RespectiveTextBox_id);
                    alert(dd);
                    alert(dd.);
                } 
                var respValue=document.getElementById(RespectiveTextBox_id + '_Raw').value; 
                var blankvalue='Rs. 0.20';
                if(respValue!='Rs. 0.00')
                {
                    alert(at1.GetValue());
                    at1.SetValue('000000.00');
                    alert(Dtxt2.GetValue());
                    Dtxt2.SetValue('11.00');
                    var rr='D'+'txt2';
                    alert(rr); 
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:HiddenField ID="hdnNOR" runat="server" />
        <asp:HiddenField ID="hdnOpeningCR" runat="server" />
        <asp:HiddenField ID="hdnOpeningDR" runat="server" />
        <table border="2" id="MainDataTable" runat="server" class="TableMain100">
            <tr>
                <td class="EHEADER" style="text-align: center">
                    <strong><span style="color: #F96410; font-size: 10pt;">Account Summary</span></strong></td>
            </tr>
            <%-- <tr>
                    <td style="vertical-align: top; text-align: right; width: 900px;">
                        &nbsp;</td>
                </tr>--%>
            <tr style="width: 100%">
                <td style="height: auto; background-color: #ffffff; width: 100%;">
                    <table class="gridheader">
                        <tr style="width: 100%">
                            <td style="width: 25%; height: 15px; text-align: left;">
                                <strong><span>Account Name</span></strong>
                            </td>
                            <td style="text-align: left">
                                <strong><span>Account Code</span></strong>
                            </td>
                            <td style="text-align: left;">
                                <strong><span>Opening Dr.</span></strong>
                            </td>
                            <td style="text-align: left;">
                                <strong><span style="padding-left: 40px">Opening Cr.</span></strong>
                            </td>
                            <td style="text-align: left;">
                                <asp:LinkButton ID="BtnAdd" runat="server" OnClick="BtnAdd_Click"><span style="color: #000099;
                                    text-decoration: underline"><strong>Save</strong></span> </asp:LinkButton>
                                &nbsp; &nbsp;
                                    <asp:LinkButton ID="BtnCancel" runat="server" OnClick="BtnCancel_Click"><span style="color: #000099;
                                    
                                    text-decoration: underline"><strong>Cancel</strong></span> </asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Total Dr." Width="100px" CssClass="floatLeft" Font-Bold="true">
                                </dxe:ASPxLabel>
                                <dxe:ASPxTextBox ID="txtTotalDr" runat="server" Width="130px" Style="background-color: #ddecfe; padding-right: 20px;">
                                    <MaskSettings Mask="<Rs. |*Rs. ><0..99999g>.<00..99>" IncludeLiterals="DecimalSymbol"
                                        ErrorText="None" />
                                </dxe:ASPxTextBox>
                            </td>
                            <td>
                                <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Total Dr." Width="100px" CssClass="floatLeft" Font-Bold="true">
                                </dxe:ASPxLabel>
                                <dxe:ASPxTextBox ID="txtTotalCr" runat="server" Width="130px" Style="background-color: #ddecfe">
                                    <MaskSettings Mask="<Rs. |*Rs. ><0..99999g>.<00..99>" IncludeLiterals="DecimalSymbol"
                                        ErrorText="None" />
                                </dxe:ASPxTextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Table ID="tblData" runat="server" BackColor="White" CellSpacing="0" CellPadding="0">
                    </asp:Table>
                    <%-- <%ShowList(); %>--%>
                </td>
            </tr>
            <tr>
            </tr>
        </table>
    </div>
</asp:Content>
