<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    Inherits="ERP.OMS.Management.Master.management_master_AddNewProduct" CodeBehind="AddNewProduct.aspx.cs" %>

<%-- <title>Product Details</title>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
        function OnMoreInfoClick(keyValue, FrmDt, ToDt) {
            var url = 'DStatChart.aspx?Flag=SR&id=' + keyValue + '&FrmDt=' + FrmDt + '&ToDt=' + ToDt;
        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function infoclick(productid, seriesid) {
            var url = 'isin_productdetails.aspx?productid=' + productid + '&seriesid=' + seriesid;
            editwin = dhtmlmodal.open("Editbox", "iframe", url, "ISIN Details", "width=910px,height=400px,center=1,resize=1,top=500", "recal")

        }
    </script>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table id="tblShowDt" runat="server" width="100%">
        <tr>
            <td colspan="5" class="EHEADER"></td>
        </tr>

    </table>
    <table style="width: 100%">
        <tr>
            <td style="width: 100px"></td>
            <td colspan="2" rowspan="2">

                <table id="tblAddNew" runat="server">
                    <tr>
                        <td style="width: 128px; font-weight: bold; color: blue;">Delivery Mode:</td>
                        <td style="font-weight: bold; width: 14px; color: blue"></td>
                        <td>
                            <asp:DropDownList ID="ddlDeliveryMode" runat="server" Width="163px">
                                <asp:ListItem Value="0">Demat</asp:ListItem>
                                <asp:ListItem Value="2">Physical</asp:ListItem>
                            </asp:DropDownList></td>
                        <td rowspan="2" style="width: 31px" valign="bottom">
                            <asp:Button ID="btnCheck" runat="server" Font-Bold="True" ForeColor="Blue"
                                Text="Check..." OnClick="btnCheck_Click" ValidationGroup="a" /></td>
                    </tr>

                    <tr>
                        <td style="width: 128px; font-weight: bold; color: blue; height: 24px;">ISIN No.:</td>
                        <td style="font-weight: bold; width: 14px; color: blue; height: 24px"></td>
                        <td style="height: 24px">
                            <asp:TextBox ID="txtISIN" runat="server" Width="160px" MaxLength="12"></asp:TextBox></td>
                    </tr>
                </table>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtISIN"
                    ErrorMessage="ISIN No Can't be blank" ValidationGroup="a" Width="381px"></asp:RequiredFieldValidator></td>
            <td style="width: 100px"></td>
        </tr>
        <tr>
            <td style="width: 100px; height: 36px;"></td>
            <td style="width: 100px; height: 36px;"></td>
        </tr>
        <tr>
            <td style="width: 100px; height: 72px"></td>
            <td colspan="2" rowspan="1" style="height: 72px">
                <table id="tblAddProduct" runat="server">
                    <tr>
                        <td style="width: 129px; font-weight: bold; color: blue;">Product Name:</td>
                        <td style="font-weight: bold; width: 11px; color: blue"></td>
                        <td style="width: 3px">
                            <asp:TextBox ID="txtProductName" runat="server" Width="160px"></asp:TextBox></td>
                        <td rowspan="2" style="width: 31px" valign="bottom">
                            <asp:Button ID="btnContinue" runat="server" Font-Bold="True" ForeColor="Blue" OnClick="btnContinue_Click"
                                Text="Continue..." ValidationGroup="b" /></td>
                    </tr>
                    <tr>
                        <td style="width: 129px; font-weight: bold; color: blue; height: 25px;">Product Short Name:</td>
                        <td style="font-weight: bold; width: 11px; color: blue; height: 25px">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtProductShortName"
                                ErrorMessage="Insert  Product's Short Name" ValidationGroup="b">*</asp:RequiredFieldValidator></td>
                        <td style="height: 25px; width: 3px;">
                            <asp:TextBox ID="txtProductShortName" runat="server" Width="160px"></asp:TextBox></td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" Width="305px" ValidationGroup="b" />
            </td>
            <td style="width: 100px; height: 72px"></td>
        </tr>
        <tr>
            <td style="width: 100px"></td>
            <td colspan="2" rowspan="7">
                <table id="tblAddProductEquity" runat="server">
                    <tr>
                        <td style="font-weight: bold; width: 131px; color: blue; font-style: normal; height: 16px">Ticker Code:</td>
                        <td style="font-weight: bold; width: 13px; color: blue; font-style: normal; height: 16px">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtTickerCode"
                                ErrorMessage="Insert  Product's Ticker Code" ValidationGroup="c">*</asp:RequiredFieldValidator></td>
                        <td style="width: 100px; height: 16px">
                            <asp:TextBox ID="txtTickerCode" runat="server" Width="160px"></asp:TextBox></td>
                        <td style="width: 100px; height: 16px"></td>
                    </tr>
                    <tr>
                        <td style="width: 131px; font-weight: bold; color: blue;">
                            <asp:Label ID="lblSeries" runat="server" Text="Series" Width="68px"></asp:Label></td>
                        <td style="font-weight: bold; width: 13px; color: blue"></td>
                        <td style="width: 100px">
                            <asp:TextBox ID="txtSeries" runat="server" Width="160px"></asp:TextBox></td>
                        <td style="width: 100px"></td>
                    </tr>
                    <tr>
                        <td style="width: 131px; font-weight: bold; color: blue;">Trading Lot:</td>
                        <td style="font-weight: bold; width: 13px; color: blue">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtTradingLot"
                                ErrorMessage="Insert  Product's Trading Lot" ValidationGroup="c">*</asp:RequiredFieldValidator></td>
                        <td style="width: 100px">
                            <asp:TextBox ID="txtTradingLot" runat="server" Width="160px"></asp:TextBox></td>
                        <td style="width: 100px"></td>
                    </tr>
                    <tr>
                        <td style="width: 131px; font-weight: bold; color: blue; height: 11px;">Face Value:</td>
                        <td style="font-weight: bold; width: 13px; color: blue; height: 11px">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtFaceValue"
                                ErrorMessage="Insert  Product's Face Value" ValidationGroup="c">*</asp:RequiredFieldValidator></td>
                        <td style="width: 100px; height: 11px;">
                            <asp:TextBox ID="txtFaceValue" runat="server" Width="160px"></asp:TextBox></td>
                        <td style="width: 100px; height: 11px;"></td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; width: 131px; color: blue; height: 11px">Group:</td>
                        <td style="font-weight: bold; width: 13px; color: blue; height: 11px"></td>
                        <td style="width: 100px; height: 11px">
                            <asp:TextBox ID="txtGroup" runat="server" Width="160px"></asp:TextBox></td>
                        <td style="width: 100px; height: 11px"></td>
                    </tr>
                    <tr>
                        <td style="width: 131px"></td>
                        <td style="width: 13px"></td>
                        <td style="width: 100px">
                            <asp:Button ID="btnSave" runat="server" Font-Bold="True" ForeColor="Blue"
                                Text="Save" Width="68px" OnClick="btnSave_Click" ValidationGroup="c" /></td>
                        <td style="width: 100px"></td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="c"
                    Width="429px" />
            </td>
            <td style="width: 100px"></td>
        </tr>
        <tr>
            <td style="width: 100px"></td>
            <td style="width: 100px"></td>
        </tr>
        <tr>
            <td style="width: 100px"></td>
            <td style="width: 100px"></td>
        </tr>
        <tr>
            <td style="width: 100px"></td>
            <td style="width: 100px"></td>
        </tr>
        <tr>
            <td style="width: 100px"></td>
            <td style="width: 100px"></td>
        </tr>
        <tr>
            <td style="width: 100px"></td>
            <td style="width: 100px"></td>
        </tr>
        <tr>
            <td style="width: 100px"></td>
            <td style="width: 100px"></td>
        </tr>
        <tr>
            <td style="width: 100px"></td>
            <td style="width: 47px"></td>
            <td style="width: 100px"></td>
            <td style="width: 100px"></td>
        </tr>
    </table>
    <asp:HiddenField ID="hdnProductID" runat="server" />
</asp:Content>
