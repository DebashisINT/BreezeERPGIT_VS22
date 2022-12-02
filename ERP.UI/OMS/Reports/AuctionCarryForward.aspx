<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_AuctionCarryForward" CodeBehind="AuctionCarryForward.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
        function InterSettlementFunc(objID, objListFun, objEvent) {
            ajax_showOptions(objID, objListFun, objEvent);
        }
        function CalcKeyCode(aChar) {
            var character = aChar.substring(0, 1);
            var code = aChar.charCodeAt(0);
            return code;
        }
        function checkNumber(val) {
            var strPass = val.value;
            var strLength = strPass.length;
            var lchar = val.value.charAt((strLength) - 1);
            var cCode = CalcKeyCode(lchar);

            /* Check if the keyed in character is a number
               do you want alphabetic UPPERCASE only ?
               or lower case only just check their respective
               codes and replace the 48 and 57 */

            if (cCode < 48 || cCode > 57) {
                var myNumber = val.value.substring(0, (strLength) - 1);
                val.value = myNumber;
            }
            return false;
        }
        function DeliverableValue(objVal, ProductID, TotalExchQty) {
            if (parseInt(TotalExchQty) < parseInt(objVal.value)) {
                alert('You Can Deliver Maximum ' + TotalExchQty + ' Share');
                objVal.value = TotalExchQty;
            }
            document.getElementById('hdnProduct').value = TotalExchQty;
            //document.getElementById('btnCheck').click();
        }
        function ButtonShow() {
            document.getElementById('TrProcessing').style.display = 'inline';
            height();
        }
        function Page_Load() {
            document.getElementById('TrProcessing').style.display = 'none';
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
        }
        function checkValForTextBox(obj) {
            var objText = obj.split('_');
            var objMainText;
            objMainText = objText[0] + '_' + objText[1] + '_' + 'txtStock';
            var TextStockVal = document.getElementById(objMainText).value;
            if (TextStockVal != '')
                document.getElementById(obj).checked = true;
            else {
                document.getElementById(obj).checked = false;
            }
        }
        function GiveAlert(Obj) {
            alert('Closing Rates For All/Some Dates  ' + '\n' + ' For The Period ' + Obj + ' are/is Missing. ' + '\n' + ' Please Update The Rates and Re-Run The Auction Process !!');
        }
        FieldName = 'btnShow'
    </script>
    <script language="javascript" type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
        var postBackElement;
        function InitializeRequest(sender, args) {
            if (prm.get_isInAsyncPostBack())

                args.set_cancel(true);
            postBackElement = args.get_postBackElement();
            $get('UpdateProgress1').style.display = 'block';

        }
        function EndRequest(sender, args) {
            $get('UpdateProgress1').style.display = 'none';
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">Auction CarryForward</span></strong>
                </td>
            </tr>
        </table>
        <table class="TableMain100">
            <tr>
                <td colspan="2">
                    <table>
                        <tr>
                            <td class="gridcellleft" style="width: 130px">Source Settlement
                            </td>
                            <td>
                                <asp:TextBox ID="txtSourceSett" runat="server" Font-Size="12px" onkeyup="InterSettlementFunc(this,'settNoForAuction',event)"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="gridcellleft" style="width: 130px">Target Settlement
                            </td>
                            <td>
                                <asp:TextBox ID="txtTargetSett" runat="server" Font-Size="12px" onkeyup="InterSettlementFunc(this,'settNoForAuction',event)"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="btnUpdate" Width="119px"
                        OnClick="btnShow_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                        <ProgressTemplate>
                            <div id='Div2' style='position: absolute; font-family: arial; font-size: 30; background-color: white; layer-background-color: white;'>
                                <table border='1' cellpadding='0' cellspacing='0' bordercolor='#C0D6E4'>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td height='25' align='center' bgcolor='#FFFFFF'>
                                                        <img src='/assests/images/progress.gif' width='18' height='18'></td>
                                                    <td height='10' width='100%' align='center' bgcolor='#FFFFFF'>
                                                        <font size='2' face='Tahoma'><strong align='center'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Loading..</strong></font></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="grdDematProcessing" runat="server" Width="100%" BorderColor="CornflowerBlue"
                                ShowFooter="True" AllowSorting="true" AutoGenerateColumns="false" BorderStyle="Solid"
                                BorderWidth="2px" CellPadding="4" ForeColor="#0000C0" OnRowDataBound="grdDematProcessing_RowDataBound">
                                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                                <RowStyle BackColor="White" ForeColor="#330099" BorderColor="#BFD3EE" BorderStyle="Double"
                                    BorderWidth="1px"></RowStyle>
                                <Columns>
                                    <asp:TemplateField HeaderText="Client/Exchange">
                                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblClientName" runat="server" Text='<%# Eval("ClientName")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ProductName">
                                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("ProductName")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Pending Incoming">
                                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblInComing" runat="server" Text='<%# Eval("InComing")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Out Going">
                                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblOutGoing" runat="server" Text='<%# Eval("OutGoing")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="C/F To Auction">
                                        <ItemStyle Font-Size="12px" BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtStock" Font-Size="12px" Width="60px" runat="server" Text='<%# Eval("Adjust") %>' onKeyUp="javascript:checkNumber(this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CUSID" Visible="false">
                                        <ItemStyle Font-Size="12px" BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCusID" runat="server" Text='<%# Eval("DematPosition_CustomerID")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Product ID" Visible="False">
                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblProID" runat="server" Text='<%# Eval("DematPosition_ProductSeriesID")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="DelPos" Visible="False">
                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDelPos" runat="server" Text='<%# Eval("DelPos")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ISIN" Visible="False">
                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblISIN" runat="server" Text='<%# Eval("ISIN")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CType" Visible="False">
                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCType" runat="server" Text='<%# Eval("CType")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkDelivery" runat="server" Checked='<%# Eval("ChkUnchk") %>' onclick="checkValForTextBox(this.id)" />
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="cbSelectAll" runat="server" />
                                        </HeaderTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EditRowStyle BackColor="#E59930"></EditRowStyle>
                                <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
                                <PagerStyle ForeColor="White" HorizontalAlign="Center"></PagerStyle>
                                <HeaderStyle ForeColor="Black" BorderWidth="1px" CssClass="EHEADER" BorderColor="AliceBlue"
                                    Font-Bold="False"></HeaderStyle>
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCheck" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr id="TrProcessing">
                <td>
                    <asp:Button ID="btnCheck" runat="server" Text="Processing" CssClass="btnUpdate" OnClick="btnCheck_Click" />
                    <%--<asp:HiddenField ID="hdnProduct" runat="server" />--%>
                </td>
                <td style="display: none"></td>
            </tr>
        </table>
    </div>
</asp:Content>
