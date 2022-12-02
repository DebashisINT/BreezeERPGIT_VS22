<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.DailyTask.Management_DailyTask_frm_NSECDXTRADES" CodeBehind="frm_NSECDXTRADES.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../CSS/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/assests/js/init.js"></script>
    <%-- <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>--%>
    <script type="text/javascript" src="/assests/js/ajaxList.js"></script>
    <script type="text/javascript" src="/assests/js/jquery-1.3.2.js"></script>

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

        .grid_scroll {
            overflow-y: no;
            overflow-x: scroll;
            width: 55%;
            scrollbar-base-color: #C0C0C0;
        }
    </style>
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
               FieldName = 'lstSlection';
               function selectfile() {
                   var i = document.getElementById('cmbTrade').value;
                   document.getElementById('txtType').value = i;

               }
               function hide() {
                   document.getElementById('tdyes').style.display = 'none';
                   document.getElementById('tdno').style.display = 'none';
                   document.getElementById('tr_CustCheck').style.display = 'none';
                   document.getElementById('tr_CustTxt').style.display = 'none';
                   document.getElementById('tr_CustBtn').style.display = 'none';
               }
               function show() {
                   document.getElementById('tdyes').style.display = 'inline';
                   document.getElementById('tdno').style.display = 'inline';
               }
               function showtr_Cust() {
                   document.getElementById('tr_CustCheck').style.display = 'inline';
               }

               function fnClientcallajax(objID, objListFun, objEvent, ObjCriteria) {
                   ajax_showOptions(objID, 'ShowClientFORMarginStocks', objEvent, 'Clients');
               }
               function ddlAddClient(obj) {
                   if (obj == 1) {
                       document.getElementById('tr_CustTxt').style.display = 'inline';
                       document.getElementById('tr_CustBtn').style.display = 'none';
                   }
                   else if (obj == 2) {
                       document.getElementById('tr_CustTxt').style.display = 'none';
                       document.getElementById('tr_CustBtn').style.display = 'inline';
                   }
                   else {
                       document.getElementById('tr_CustTxt').style.display = 'none';
                       document.getElementById('tr_CustBtn').style.display = 'none';
                   }
               }
               function OnLinkButtonClick() {
                   var url = 'TradeImportDetailsCom.aspx';
                   OnMoreInfoClick(url, "Trade Import Details", '800px', '450px');
               }

               </script>
    <script type="text/javascript" src="/assests/js/init.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:Panel ID="Panelmain" runat="server" HorizontalAlign="Center" Style="z-index: 100; top: 13px"
            Visible="true">
            <table id="tbl_main" cellpadding="0" cellspacing="0" class="login" style="height: 153px; display: inline-table;" width="410">
                <tbody>
                    <tr>
                        <td class="lt1">
                            <h5>Imports NSE-CDX Trades
                            </h5>
                        </td>
                    </tr>

                    <tr>
                        <td class="lt">
                            <table cellpadding="0" cellspacing="12" class="width100per">
                                <tbody>
                                    <tr>
                                        <td class="lt" colspan="2">
                                            <div id="divStatus" runat="server">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lt" colspan="2">
                                            <div id="divCust" runat="server">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="tr_CustCheck" runat="server">
                                        <td>
                                            <asp:DropDownList ID="ddlClient" runat="server" Width="191px" onchange="ddlAddClient(this.value)">
                                                <asp:ListItem Value="0">Select Action</asp:ListItem>
                                                <asp:ListItem Value="1">Update all Unidentified Clients</asp:ListItem>
                                                <asp:ListItem Value="2">Delete all unIdentified Clients</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="tr_CustTxt">
                                        <td class="lt">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtClient" runat="server" Width="200px" Font-Size="11px" onkeyup="fnClientcallajax(this,'chkfn',event,'Clients')"></asp:TextBox>
                                                    </td>
                                                    <td style="height: 22px">
                                                        <asp:Button ID="btnCust" runat="server" CssClass="btn" Text="Save" OnClick="btnCust_Click" />
                                                    </td>

                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="tr_CustBtn">
                                        <td style="width: 283px">
                                            <asp:Button ID="btnRemove" runat="server" CssClass="btn" Text="Remove" OnClick="btnRemove_Click" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="lt" colspan="2" style="height: 57px">
                                            <table>
                                                <tr>
                                                    <td colspan="4">
                                                        <div id="divterminalID" runat="server">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td id="tdyes">
                                                        <asp:Button ID="btnYes" runat="server" CssClass="btn" OnClick="btnYes_Click" Text="Yes"
                                                            Width="84px" />
                                                    </td>
                                                    <td id="tdno">
                                                        <asp:Button ID="btnNo" runat="server" CssClass="btn" OnClick="btnNo_Click" Text="No"
                                                            Width="84px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lt">
                                            <asp:Label ID="importstatus" runat="server" Font-Size="XX-Small" Font-Names="Arial"
                                                Font-Bold="True" ForeColor="Red" />
                                        </td>
                                    </tr>
                                    <tr>

                                        <td>
                                            <asp:DropDownList ID="cmbTrade" runat="server" OnSelectedIndexChanged="cmbTrade_SelectedIndexChanged" AutoPostBack="True" Width="188px">
                                                <asp:ListItem Value="0">FINAL TXT</asp:ListItem>
                                                <asp:ListItem Value="1">OMNI TXT</asp:ListItem>
                                                <asp:ListItem Value="2">FINAL CSV</asp:ListItem>
                                                <asp:ListItem Value="3">Trade OP</asp:ListItem>
                                                <asp:ListItem Value="4">Trade Now</asp:ListItem>
                                                <asp:ListItem Value="5">FINAL TXT FOR IRF</asp:ListItem>
                                                <asp:ListItem Value="6">GREEK TXT</asp:ListItem>
                                                <asp:ListItem Value="7">ODIN TXT</asp:ListItem>
                                                <asp:ListItem Value="8">Flash Trade CSV</asp:ListItem>
                                                <asp:ListItem Value="9">Opening Trades From PS03 Files</asp:ListItem>

                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="2" valign="middle">
                                            <asp:Label ID="Label1" runat="server">EG: X_DDMMYYYY_MemberCode.txt</asp:Label></td>
                                    </tr>
                                    <tr>

                                        <td>
                                            <asp:FileUpload ID="McxTradeSelectFile" runat="server" Height="21px" Width="272px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table cellpadding="0" cellspacing="0">
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="BtnSave" runat="server" CssClass="btn" OnClick="BtnSave_Click" Text="Import File"
                                                                Width="84px" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>

        </asp:Panel>
        <table>
            <tr>
                <td style="display: none;">
                    <asp:HiddenField ID="hdfname" runat="server" />
                    <asp:HiddenField ID="hdnDate" runat="server" />
                    <asp:HiddenField ID="hdnTerminalID" runat="server" />
                    <asp:TextBox ID="txtClient_hidden" runat="server" Width="5px"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
