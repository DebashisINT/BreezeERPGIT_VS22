<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_ChequePrintingintermediateother" CodeBehind="ChequePrintingintermediateother.aspx.cs" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link href="/aspnet_client/system_web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/system_web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/system_web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/system_web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />  
    <script language="javascript" type="text/javascript">

        function height() {
            if (document.body.scrollHeight >= 500) {
                window.frameElement.height = document.body.scrollHeight;
            }
            else {
                window.frameElement.height = '500';
            }
            window.frameElement.width = document.body.scrollWidth;
        }

        function chkRectify() {
            if (chkRectify.checked) {

                alert(document.getElementById('Rectify').id);
            }
            else {
                alert(document.getElementById('Rectify').id);

            }

        }

        function SelectSingle() {
            //alert('a');
            var gridview = document.getElementById('gridview');
            var rCount = gridview.rows.length;
            var SumAmt = 0;
            var SumChq = 0;
            var chqNo1;
            var j = 0;
            var chequeID = 0;
            var chequenumber = 0;
            var bank = 0;
            for (i = 2; i < rCount; i++) {
                var leni;
                var a = new String(i)
                j = i - 1;
                var b = new String(j)
                if (a.length == 1)
                    leni = "0" + i;
                else
                    leni = i;
                if (b.length == 1)
                    j = "0" + j;
                var obj = 'gridview' + '_ctl' + leni + '_' + 'ChkDelivery';
                //                     var chqNo='gridview'+'_ctl'+leni+'_'+'txtChequeNumber';
                var chqid = 'gridview' + '_ctl' + leni + '_' + 'CashBankDetail_ID';
                //                     var bnkid='gridview'+'_ctl'+leni+'_'+'ddlClientAccount';
                //                     if(i!=2)
                //                         chqNo1='gridCheque'+'_ctl'+j+'_'+'txtChequeNumber';
                //alert(chqid);
                if (document.getElementById(obj).checked == true) {

                    if (document.getElementById(chqid).value != '') {
                        if (chequeID == 0) {
                            chequeID = document.getElementById(chqid).innerText;
                            bank = document.getElementById(bnkid).value;
                        }
                        else {
                            chequeID = chequeID + ',' + document.getElementById(chqid).innerText;
                            bank = bank + ',' + document.getElementById(bnkid).value;
                        }
                    }
                }
            }

        }

        function SelectAll(id) {
            var frm = document.forms[0];
            var val;
            for (i = 0; i < frm.elements.length; i++) {
                if (frm.elements[i].type == "text") {
                    val = '';
                    val = frm.elements[i].value
                }
                if (frm.elements[i].type == "checkbox") {
                    if (val != '')
                        frm.elements[i].checked = document.getElementById(id).checked;
                }
            }
            SelectSingle();
        }

        function SelectAllInterSegment(id) {
            var frm = document.forms[0];
            for (i = 0; i < frm.elements.length; i++) {
                if (frm.elements[i].type == "checkbox") {
                    frm.elements[i].checked = document.getElementById(id).checked;
                }
            }
            SelectSingle();
        }

        function Hide() {
            document.getElementById('CrystalReportViewer1').style.display = 'none';
            document.getElementById('CrystalReportViewer2').style.display = 'none';
            document.getElementById('ddlPrintMode').value = "0";
        }
        function show() {
            document.getElementById('CrystalReportViewer1').style.display = 'inline';
            document.getElementById('CrystalReportViewer2').style.display = 'inline';
        }
        function HideTr() {
            document.getElementById('trYesNo').style.display = 'none';

        }
        function ShowTr() {
            document.getElementById('trYesNo').style.display = 'inline';

        }
    </script>

        <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table>
            <tr>
                <td>
                    <table border="1">
                        <tr>
                            <%--<td>
                            <asp:DropDownList id="ddlExport" runat="server">
                                                     <asp:ListItem>Select </asp:ListItem>
                                                     <asp:ListItem>PDF</asp:ListItem>
                                                 </asp:DropDownList>
                <asp:Button ID="btnExport" CssClass="btn" runat="server" OnClick="btnExport_Click" Text="Report" /></td>--%>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblPrint" runat="server" Text=" Select Print Type:" Width="37px" Height="22px"></asp:Label>
                                            <asp:DropDownList ID="ddlPrintMode" AutoPostBack="true" runat="server" Width="104px" OnSelectedIndexChanged="ddlPrintMode_SelectedIndexChanged">
                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                <asp:ListItem Value="1">Dos Print</asp:ListItem>
                                                <asp:ListItem Value="2">Lazer Print</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 14px"></td>
                                    </tr>
                                    <tr>
                                        <td>

                                            <asp:Button ID="btnUpdateCheque" runat="server" CssClass="btn" OnClick="btnUpdateCheque_Click" Text="Update Cheques"
                                                Width="123px" />
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 14px">
                                            <asp:Label ID="lblerror" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trYesNo" runat="server">

                                        <td>
                                            <asp:ScriptManager ID="ScriptManager1" runat="server">
                                            </asp:ScriptManager>
                                            <table width="100%">
                                                <tr>
                                                    <td align="center">
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblYesNo" runat="server" Font-Bold="True" ForeColor="Red">Did All Cheques Print Successfully??</asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td align="right">
                                                                                <asp:Button ID="btnYes" runat="server" CssClass="btn" Text="Yes - Update" OnClick="btnYes_Click" Width="92px" />
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:Button ID="btnNo" runat="server" CssClass="btn" Text="No - Cancel" OnClick="btnNo_Click" Width="122px" />
                                                                            </td>
                                                                            <td align="left" style="display: none">
                                                                                <asp:Button ID="btnUpdateprint" runat="server" CssClass="btn" Text="Update & PrintSummary" OnClick="btnUpdateprint_Click" Width="153px" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>

                                            </table>
                                        </td>
                                    </tr>

                                </table>
                            </td>
                        </tr>

                    </table>
                </td>
            </tr>

            <tr>
                <td style="width: 100%">
                    <asp:GridView ID="gridview" runat="server" AutoGenerateColumns="False" OnPageIndexChanging="gridview_PageIndexChanging" ForeColor="#333333" Width="100%" OnRowDataBound="gridview_RowDataBound" OnRowUpdating="gridview_RowUpdating">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="ChkDelivery" runat="server" onclick="SelectSingle();" />
                                </ItemTemplate>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="cbSelectAll" runat="server" />
                                </HeaderTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CashBankDetail_ID" HeaderText="ID">
                                <HeaderStyle Font-Size="Small" ForeColor="White" />
                                <ItemStyle Font-Size="Small" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CashBankDetail_InstrumentNumber" HeaderText="Cheque Number">
                                <ItemStyle Font-Size="Small" />
                                <HeaderStyle Font-Size="Small" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MainAccount_Name" HeaderText="Account Name">
                                <ItemStyle Font-Size="Small" />
                                <HeaderStyle Font-Size="Small" />
                            </asp:BoundField>
                            <asp:BoundField DataField="cashbank_vouchernumber" HeaderText="Voucher Number">
                                <ItemStyle Font-Size="Small" />
                                <HeaderStyle Font-Size="Small" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CashBankDetail_InstrumentDate" HeaderText="Instrument Date" Visible="true">
                                <ItemStyle Font-Size="Small" />
                                <HeaderStyle Font-Size="Small" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CashBank_TransactionDate" HeaderText="Transaction Date">
                                <ItemStyle Font-Size="Small" />
                                <HeaderStyle Font-Size="Small" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Payment" HeaderText="Payment">
                                <ItemStyle Font-Size="Small" />
                                <HeaderStyle Font-Size="Small" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CashBankDetail_Subaccountid" HeaderText="Sub AccountID" Visible="true">
                                <ItemStyle Font-Size="Small" />
                                <HeaderStyle Font-Size="Small" />
                            </asp:BoundField>
                            <asp:BoundField DataField="cashbankdetail_mainaccountid" HeaderText="MainAccountID" Visible="true">
                                <ItemStyle Font-Size="Small" />
                                <HeaderStyle Font-Size="Small" />
                            </asp:BoundField>

                            <asp:BoundField DataField="cnt_branchID" HeaderText="BranchId" Visible="false">
                                <ItemStyle Font-Size="Small" />
                                <HeaderStyle Font-Size="Small" />
                            </asp:BoundField>

                            <asp:BoundField DataField="BranchDescription" HeaderText="Branch Description" Visible="false">
                                <ItemStyle Font-Size="Small" />
                                <HeaderStyle Font-Size="Small" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BankID" HeaderText="BankID" Visible="false">
                                <ItemStyle Font-Size="Small" />
                                <HeaderStyle Font-Size="Small" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ClientBankID" HeaderText="Client BankID" Visible="false">
                                <ItemStyle Font-Size="Small" />
                                <HeaderStyle Font-Size="Small" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ClientBankName" HeaderText="Client BankName" Visible="false">
                                <ItemStyle Font-Size="Small" />
                                <HeaderStyle Font-Size="Small" />
                            </asp:BoundField>

                        </Columns>
                        <FooterStyle BackColor="#507CD1" BorderColor="AliceBlue" ForeColor="White" />
                        <HeaderStyle BackColor="#507CD1" BorderColor="AliceBlue" ForeColor="White" />
                    </asp:GridView>


                </td>
            </tr>



            <tr>
                <td valign="top">
                    <br />
                    <asp:Label ID="lbllist" runat="server" Text="List Of ChequeNumber(s)" ForeColor="Blue" Visible="False"></asp:Label>
                    <asp:CheckBoxList ID="chkRectify" runat="server" Width="158px" Enabled="False" Visible="False">
                    </asp:CheckBoxList>
                </td>
            </tr>

        </table>

        <cr:crystalreportviewer id="CrystalReportViewer1" runat="server" autodatabind="true"
            reportsourceid="CrystalReportSource1" printmode="ActiveX" bestfitpage="False" height="3055px" separatepages="False" width="901px" />
        <cr:crystalreportsource id="CrystalReportSource1" runat="server">
            <Report FileName="ChequePrint1.rpt">
            </Report>
        </cr:crystalreportsource>
        <cr:crystalreportviewer id="CrystalReportViewer2" runat="server" autodatabind="true"
            reportsourceid="CrystalReportSource2" printmode="ActiveX" bestfitpage="False" height="3055px" separatepages="False" width="901px" />
        <cr:crystalreportsource id="CrystalReportSource2" runat="server">
            <Report FileName="ChequePrintUTI.rpt">
            </Report>
        </cr:crystalreportsource>
        <asp:HiddenField ID="hdnID1" runat="server" />
        <asp:HiddenField ID="hdnbank1" runat="server" />
        <asp:HiddenField ID="hdnFromdate" runat="server" />
        <asp:HiddenField ID="hdnTodate" runat="server" />
        <asp:HiddenField ID="hdnstrBank1" runat="server" />
        <asp:HiddenField ID="hdnChequeNumber1" runat="server" />
        <asp:HiddenField ID="hdnBankType1" runat="server" />
    </div>
</asp:Content>
