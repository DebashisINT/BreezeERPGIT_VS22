<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_frmLegendReport_popup" Codebehind="frmLegendReport_popup.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .headerSt
        {
            BORDER-RIGHT: Blue 1px solid; 
	        PADDING-RIGHT: 0px; 
	        BORDER-TOP: Blue 1px solid; 
	        PADDING-LEFT: 0px; 
	        BACKGROUND: url(../images/EHeaderBack.gif) repeat-x 0px 0px; 
	        PADDING-BOTTOM: 0px;
	        FONT: bold 10px/14px Verdana, Arial, Helvetica, sans-serif; 
	        BORDER-LEFT: Blue 1px solid;
	        PADDING-TOP: 0px; 
	        BORDER-BOTTOM: Blue 1px solid;	
	        height:28px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table border="1" style="background-color:#F4F4E1;font-size:small; width: 100%;" cellpadding="0" cellspacing="0" class="TableMain">
            <tr>
                <td class="headerSt" align="center">
                    Colour</td>
                <td class="headerSt" align="center">
                    Status</td>
            </tr>
            <tr>
                <td style="background-color:#00B050;width:1px">
                </td>
                <td style="width: 100%" align="left">
                    <strong><span style="color: #800000">P</span></strong> – present</td>
            </tr>
            <tr>
                <td style="background-color:#92D050;width:1px">
                </td>
                <td style="width: 100%" align="left">
                    <strong><span style="color: #800000">OD</span></strong>-Official Delay</td>
            </tr>
            <tr>
                <td style="background-color:#AAFFAA;width:1px">
                </td>
                <td style="width: 100%" align="left">
                    <strong><span style="color: #800000">OV</span></strong>-Official Visit</td>
            </tr>
            <tr>
                <td style="background-color:#F87156;width:1px">
                </td>
                <td style="width: 100%" align="left">
                    <strong><span style="color: #800000">PD</span></strong>-Personal Delay</td>
            </tr>
            <tr>
                <td style="background-color:#FF0000;width:1px">
                </td>
                <td style="width: 100%" align="left">
                    <strong><span style="color: #800000">A</span></strong> – Absent or Leave without
                    Pay</td>
            </tr>
            <tr>
                <td style="background-color:#538ED5;width:1px">
                </td>
                <td style="width: 100%" align="left">
                    <strong>PL</strong>– Privilege Leave</td>
            </tr>
            <tr>
                <td style="background-color:#8DB4E3;width:1px">
                </td>
                <td style="width: 100%" align="left">
                    <strong><span style="color: #800000">CL</span></strong> – Casual Leave</td>
            </tr>
            <tr>
                <td style="background-color:#17375D;width:1px">
                </td>
                <td style="width: 100%" align="left">
                    <strong><span style="color: #800000">SL</span></strong> – Sick Leave</td>
            </tr>
            <tr>
                <td style="background-color:#C2D69A;width:1px">
                </td>
                <td style="width: 100%" align="left">
                    <strong><span style="color: #800000">HC </span></strong>– Half day(Casual)</td>
            </tr>
            <tr>
                <td style="background-color:#D7E4BC;width:1px">
                </td>
                <td style="width: 100%" align="left">
                    <strong><span style="color: #800000">HS </span></strong>– Half day(Sick)</td>
            </tr>
            <tr>
                <td style="background-color:#953735;width:1px">
                </td>
                <td style="width: 100%" align="left">
                    <strong><span style="color: #800000">WO</span></strong> – weekly Off</td>
            </tr>
            <tr>
                <td style="background-color:#BC4914;width:1px">
                </td>
                <td style="width: 100%;">
                    <strong><span style="color: #800000">CO</span></strong>– Compensatory off</td>
            </tr>
            <tr>
                <td style="background-color:#E46D0A;width:1px">
                </td>
                <td style="width: 100%;">
                    <strong><span style="color: #800000">PH</span></strong>– Paid holiday</td>
            </tr>
            
        </table>
    
    </div>
        <strong><span style="color: #800000"></span></strong>
</asp:Content>