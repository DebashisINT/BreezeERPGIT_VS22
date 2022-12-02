<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_PoaAccountPayIN" CodeBehind="PoaAccountPayIN.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajaxList_rootfile.js"></script>

    <style type="text/css">
        /* Big box with list of options */
        #ajax_listOfOptions {
            position: absolute; /* Never change this one */
            width: 50px; /* Width of box */
            height: auto; /* Height of box */
            overflow: auto; /* Scrolling features */
            border: 1px solid Blue; /* Blue border */
            background-color: #FFF; /* White background color */
            text-align: left;
            font-size: 0.9em;
            z-index: 32767;
        }

            #ajax_listOfOptions div { /* General rule for both .optionDiv and .optionDivSelected */
                margin: 1px;
                padding: 1px;
                cursor: pointer;
                font-size: 0.9em;
            }

            #ajax_listOfOptions .optionDiv { /* Div for each item in list */
            }

            #ajax_listOfOptions .optionDivSelected { /* Selected item in the list */
                background-color: #DDECFE;
                color: Blue;
            }

        #ajax_listOfOptions_iframe {
            background-color: #F00;
            position: absolute;
            z-index: 3000;
        }

        form {
            display: inline;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function FunSettNumber(objID, objListFun, objEvent) {
       ajax_showOptions(objID, objListFun, objEvent, 'SettType', 'Main');
   }
               function keyVal(obj) {
                   document.getElementById('Button1').click();
                   height();
               }
               function height() {
                   if (document.body.scrollHeight >= 500) {
                       window.frameElement.height = document.body.scrollHeight;
                   }
                   else {
                       window.frameElement.height = '500';
                   }
                   window.frameElement.width = document.body.scrollWidth;
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
               FieldName = 'btnGenerate';
               document.body.style.cursor = 'pointer';
               var oldColor = '';
               function ChangeRowColor(rowID, rowNumber) {
                   var gridview = document.getElementById('grdPOAClient');
                   var rCount = gridview.rows.length;
                   var rowIndex = 1;
                   var rowCount = 0;
                   if (rCount == 28)
                       rowCount = 25;
                   else
                       rowCount = rCount - 2;
                   if (rowNumber > 25 && rCount < 28)
                       rowCount = rCount - 3;
                   for (rowIndex; rowIndex <= rowCount; rowIndex++) {
                       var rowElement = gridview.rows[rowIndex];
                       rowElement.style.backgroundColor = '#FFFFFF'
                   }
                   var color = document.getElementById(rowID).style.backgroundColor;
                   if (color != '#ffe1ac') {
                       oldColor = color;
                   }
                   if (color == '#ffe1ac') {
                       document.getElementById(rowID).style.backgroundColor = oldColor;
                   }
                   else
                       document.getElementById(rowID).style.backgroundColor = '#ffe1ac';
               }

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
                <td class="EHEADER" style="text-align: center;" colspan="2">
                    <strong><span style="color: #000099">POA Account PayIN</span></strong>
                </td>
            </tr>
            <tr>
                <td style="width: 369px">
                    <table style="border: solid 1px blue">
                        <tr>
                            <td class="gridcellleft">Sett. No.
                            </td>
                            <td>
                                <asp:TextBox ID="txtSettNo" Font-Size="12px" runat="server" onkeyup="FunSettNumber(this,'ShowClientScrip',event)"
                                    Width="232px"></asp:TextBox>
                                <asp:HiddenField ID="txtSettNo_hidden" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">Source DP
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlSourceDp" Font-Size="12px" runat="server" Width="235px"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddlSourceDp_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">Target A/C
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTargetAc" Font-Size="12px" runat="server" Width="236px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">Transfer Type
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTransferType" Font-Size="12px" runat="server" Width="235px">
                                    <asp:ListItem Value="TP">Transfer To Pool</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">Slip Number
                            </td>
                            <td>
                                <asp:TextBox ID="txtSlipNumber" Font-Size="12px" runat="server" Width="231px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">Execution Date
                            </td>
                            <td>
                                <dxe:ASPxDateEdit ID="DtTransferDate" runat="server" Font-Size="12px" Width="234px"
                                    EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>
                    </table>
                </td>
                <td></td>
            </tr>
            <tr>
                <td style="text-align: center">
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
                <td></td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="Panel1" runat="server" Height="420px" ScrollBars="Vertical">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:GridView ID="grdPOAClient" runat="server" Width="100%" BorderColor="CornflowerBlue"
                                                ShowFooter="True" AllowSorting="true" AutoGenerateColumns="false" BorderStyle="Solid"
                                                BorderWidth="2px" CellPadding="4" ForeColor="#0000C0" OnRowCreated="grdPOAClient_RowCreated"
                                                OnRowDataBound="grdPOAClient_RowDataBound">
                                                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Client Name">
                                                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblClientID" runat="server" Text='<%# Eval("ClientName")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Branch">
                                                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBranch" runat="server" Text='<%# Eval("Branch")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sett.Num">
                                                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSettNumber" runat="server" Text='<%# Eval("SettNumber")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Scrip">
                                                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblScrip" runat="server" Text='<%# Eval("Scrip")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ISIN">
                                                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblISIN" runat="server" Text='<%# Eval("ISIN")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Qty To Recv.">
                                                        <ItemStyle Font-Size="12px" BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIncomingPending" runat="server" Text='<%# Eval("IncomingPending")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Hold in POA">
                                                        <ItemStyle Font-Size="12px" BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblHoldOnPoa" runat="server" Text='<%# Eval("HoldOnPoa")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Transfer Qty">
                                                        <ItemStyle Font-Size="12px" BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtReceived" Font-Size="12px" Width="60px" runat="server" Text='<%# Eval("Received") %>'
                                                                onKeyUp="javascript:checkNumber(this);"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Client ID">
                                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbldpClient" runat="server" Text='<%# Eval("dpClient")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="CustomerID" Visible="false">
                                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCustomerID" runat="server" Text='<%# Eval("CustomerID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ProductSeriesID" Visible="false">
                                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProductSeriesID" runat="server" Text='<%# Eval("ProductSeriesID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="dpCode" Visible="false">
                                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbldpCode" runat="server" Text='<%# Eval("dpCode")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="BranchID" Visible="false">
                                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBranchID" runat="server" Text='<%# Eval("BranchID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="dpdID" Visible="false">
                                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbldpdID" runat="server" Text='<%# Eval("dpdID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="SNumber" Visible="false">
                                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSNumber" runat="server" Text='<%# Eval("SNumber")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="SType" Visible="false">
                                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSType" runat="server" Text='<%# Eval("SType")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="ChkDelivery" runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="cbSelectAll" runat="server" />
                                                        </HeaderTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <RowStyle BackColor="White" ForeColor="#330099" BorderColor="#BFD3EE" BorderStyle="Double"
                                                    BorderWidth="1px"></RowStyle>
                                                <EditRowStyle BackColor="#E59930"></EditRowStyle>
                                                <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
                                                <PagerStyle ForeColor="White" HorizontalAlign="Center"></PagerStyle>
                                                <HeaderStyle ForeColor="Black" BorderWidth="1px" CssClass="EHEADER" BorderColor="AliceBlue"
                                                    Font-Bold="False"></HeaderStyle>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGenerate" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ddlSourceDp" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnGenerate" runat="server" Text="Generate" CssClass="btnUpdate"
                        OnClick="btnGenerate_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="display: none">
                    <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
