<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_rpt_BankStatement" CodeBehind="rpt_BankStatement.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <script type="text/javascript" src="/assests/js/jquery.meio.mask.js"></script>

    <style type="text/css">
        .grid_scrollNSEFO {
            overflow-y: scroll;
            overflow-x: no;
            width: 1000px;
            height: 300px;
            scrollbar-base-color: #C0C0C0;
        }
        /* Big box with list of options */
    </style>

    <script type="text/javascript">
        (function ($) {
            // call setMask function on the document.ready event
            $(function () {
                $('input:text').setMask();
            }
          );
        })(jQuery);
    </script>

    <script language="javascript" type="text/javascript">
        //function height() {

        //    if (document.body.scrollHeight >= 300)
        //        window.frameElement.height = document.body.scrollHeight;
        //    else
        //        window.frameElement.height = '300px';
        //    window.frameElement.Width = document.body.scrollWidth;
        //}

        function ShowBankName(obj1, obj2, obj3) {
            ajax_showOptions(obj1, obj2, obj3, 'Current');
        }
        FieldName = 'lblFieldName';


        function setvalue(obj1) {

            var controlid = obj1.id.split('_');
            var getintno = controlid[1].split('l')
            var trantest = document.getElementById(controlid[0] + '_' + getintno[0] + 'l' + getintno[1] + '_' + 'lblTransactionDate');
            var kk = trantest.innerText;
            var kksplit = kk.split('/');
            var years = kksplit[2].split(' ');
            if (kksplit[0] == 1) {
                kksplit[0] = 'Jan';

            }
            else if (kksplit[0] == 2) {
                kksplit[0] = 'Feb';

            }
            else if (kksplit[0] == 3) {
                kksplit[0] = 'Mar';

            }
            else if (kksplit[0] == 4) {
                kksplit[0] = 'Apr';

            }
            else if (kksplit[0] == 5) {
                kksplit[0] = 'May';

            }
            else if (kksplit[0] == 6) {
                kksplit[0] = 'Jun';

            }
            else if (kksplit[0] == 7) {
                kksplit[0] = 'Jul';

            }
            else if (kksplit[0] == 8) {
                kksplit[0] = 'Aug';

            }
            else if (kksplit[0] == 9) {
                kksplit[0] = 'Sep';

            }
            else if (kksplit[0] == 10) {
                kksplit[0] = 'Oct';

            }
            else if (kksplit[0] == 11) {
                kksplit[0] = 'Nov';

            }
            else if (kksplit[0] == 12) {
                kksplit[0] = 'Dec';

            }

            var newDatewFormat = kksplit[1] + ' ' + kksplit[0] + ' ' + years[0];
            //    alert(newDatewFormat);
            var FromDate = Date.parse(newDatewFormat);

            var objValueDate = document.getElementById(controlid[0] + '_' + getintno[0] + 'l' + getintno[1] + '_' + controlid[2]);
            //    var ValDate=Date.parse(objValueDate.value);
            //    alert(objValueDate.value);

            var Testalidity = objValueDate.value.split('-');

            if (Testalidity[1] == 1) {
                Testalidity[0] = 'Jan';

            }
            else if (Testalidity[1] == 2) {
                Testalidity[1] = 'Feb';

            }
            else if (Testalidity[1] == 3) {
                Testalidity[1] = 'Mar';

            }
            else if (Testalidity[1] == 4) {
                Testalidity[1] = 'Apr';

            }
            else if (Testalidity[1] == 5) {
                Testalidity[1] = 'May';

            }
            else if (Testalidity[1] == 6) {
                Testalidity[1] = 'Jun';

            }
            else if (Testalidity[1] == 7) {
                Testalidity[1] = 'Jul';

            }
            else if (Testalidity[1] == 8) {
                Testalidity[1] = 'Aug';

            }
            else if (Testalidity[1] == 9) {
                Testalidity[1] = 'Sep';

            }
            else if (Testalidity[1] == 10) {
                Testalidity[1] = 'Oct';

            }
            else if (Testalidity[1] == 11) {
                Testalidity[1] = 'Nov';

            }
            else if (Testalidity[1] == 12) {
                Testalidity[1] = 'Dec';

            }

            var ValDate = Date.parse(Testalidity[0] + ' ' + Testalidity[1] + ' ' + Testalidity[2]);
            if (parseInt(Testalidity[0]) > 31) {
                alert('Invalid Date Format');
                document.getElementById(controlid[0] + '_' + getintno[0] + 'l' + getintno[1] + '_' + controlid[2]).focus();
                document.getElementById(controlid[0] + '_' + getintno[0] + 'l' + getintno[1] + '_' + controlid[2]).select();
            }
            else if (parseInt(Testalidity[1]) > 12) {
                alert('Invalid Date Format');
                document.getElementById(controlid[0] + '_' + getintno[0] + 'l' + getintno[1] + '_' + controlid[2]).focus();
                document.getElementById(controlid[0] + '_' + getintno[0] + 'l' + getintno[1] + '_' + controlid[2]).select();
            }
            if (ValDate < FromDate) {

                //         alert(controlid[0]+'_'+getintno[0]+'l'+getintno[1]+'_'+controlid[2]);
                document.getElementById(controlid[0] + '_' + getintno[0] + 'l' + getintno[1] + '_' + controlid[2]).focus();
                document.getElementById(controlid[0] + '_' + getintno[0] + 'l' + getintno[1] + '_' + controlid[2]).select();
                alert('Value Date can not be less than Transaction Date');

            }
            else {

                var a = parseInt(getintno[1]) + 1;//For next Value Date
                if (getintno[1] == 8) {
                    a = "9";
                }
                if (getintno[1] == 9) {
                    a = "10";
                }
                if (a < 10) {

                    a = '0' + a;
                }
                //         alert(controlid[0]+'_'+getintno[0]+'l'+a+'_'+controlid[2]);
                //         alert(document.getElementById(controlid[0]+'_'+getintno[0]+'l'+a+'_'+controlid[2]).value);
                //        if(document.getElementById(controlid[0]+'_'+getintno[0]+'l'+a+'_'+controlid[2]).value =="")
                //        { 
                //            alert('forward previous date to next');
                //             document.getElementById(controlid[0]+'_'+getintno[0]+'l'+a+'_'+controlid[2]).value=document.getElementById(controlid[0]+'_'+getintno[0]+'l'+getintno[1]+'_'+controlid[2]).value;
                //        }

            }

        }
        function SearchVisible(obj) {

            if (obj == 'N') {

                document.getElementById("trsearch").style.display = 'none';
            }
            else {

                document.getElementById("trsearch").style.display = 'inline';

            }
        }
        function CallBankAccount(obj1, obj2, obj3) {
            var CurrentSegment = '<%=Session["usersegid"]%>'
            var strPutSegment = " and (MainAccount_ExchangeSegment=" + CurrentSegment + " or MainAccount_ExchangeSegment=0)";
            var strQuery_Table = "(Select MainAccount_AccountCode+\'-\'+MainAccount_Name+\' [ \'+MainAccount_BankAcNumber+\' ]\' as IntegrateMainAccount,MainAccount_AccountCode as MainAccount_AccountCode,MainAccount_Name,MainAccount_BankAcNumber from Master_MainAccount where (MainAccount_BankCashType=\'Bank\' or MainAccount_BankCashType=\'Cash\') and (MainAccount_BankCompany=\'" + '<%=Session["LastCompany"] %>' + "\' Or IsNull(MainAccount_BankCompany,'')='')" + strPutSegment + ") as t1";
            var strQuery_FieldName = " Top 10 * ";
            var strQuery_WhereClause = "MainAccount_AccountCode like (\'%RequestLetter%\') or MainAccount_Name like (\'%RequestLetter%\') or MainAccount_BankAcNumber like (\'%RequestLetter%\')";
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');
        }
        function replaceChars(entry) {
            out = "+"; // replace this
            add = "--"; // with this
            temp = "" + entry; // temporary holder

            while (temp.indexOf(out) > -1) {
                pos = temp.indexOf(out);
                temp = "" + (temp.substring(0, pos) + add +
                temp.substring((pos + out.length), temp.length));
            }
            return temp;

        }
    </script>

    <script type="text/javascript">
        $(document).ready(function () {

            $(".water").each(function () {
                if ($(this).val() == this.title) {
                    $(this).addClass("opaque");
                }
            });

            $(".water").focus(function () {
                if ($(this).val() == this.title) {
                    $(this).val("");
                    $(this).removeClass("opaque");
                }
            });

            $(".water").blur(function () {
                if ($.trim($(this).val()) == "") {
                    $(this).val(this.title);
                    $(this).addClass("opaque");
                }
                else {
                    $(this).removeClass("opaque");
                }
            });
        });

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Bank Statement</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="pull-right">
            <tr>
               <td>
                    <asp:DropDownList ID="ddlExport" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                        <asp:ListItem Value="E">Excel</asp:ListItem>
                        <asp:ListItem Value="P">PDF</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
        </table>
        <table class="">
            <%--<tr class="EHEADER">
                <td colspan="5" align="center" style="font-weight: bold; color: Blue">BANK STATEMENT
                </td>
            </tr>--%>
            <tr>
                <td width="120px">
                    <asp:Label ID="Label1" CssClass="mylabel1" runat="server" Text="Select Bank :" />
                </td>
                <td>
                    <asp:TextBox ID="txtBankName" runat="server" CssClass="form-control"
                        onkeyup="CallBankAccount(this,'GenericAjaxList',event)"></asp:TextBox>
                    <asp:HiddenField ID="txtBankName_hidden" runat="server"></asp:HiddenField>
                    <asp:Label ID="lblFieldName" runat="server" Text="Label" Visible="false"></asp:Label>
                </td>
                <td class="rt" style="width:120px;padding-left:15px">
                    <asp:Label ID="lblConsider" CssClass="mylabel1" runat="server" Text="Consider"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlConsider" CssClass="form-control" runat="server">
                        <asp:ListItem Value="VD">Value Date</asp:ListItem>
                        <asp:ListItem Value="SD">Statement Date</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="rt" width="120px">
                    <asp:Label ID="lblFromdate" CssClass="mylabel1" runat="server" Text="From:" ></asp:Label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="FromDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td class="rt" style="width:120px;padding-left:15px">
                    <asp:Label ID="lblToDate" CssClass="mylabel1" runat="server" Text="To:"></asp:Label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="DateTo" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Button ID="btnShow" runat="server" CssClass="btn btn-primary" OnClick="btnShow_Click1" Text="Show" />
                </td>
            </tr>
            <tr> 
               
                <td style="visibility: hidden">
                    <table style="border: solid 1px white">
                        <tr>
                            <td>
                                <asp:RadioButton ID="RdUnCleared" runat="server" Text="" GroupName="R" /></td>
                            <td class="mylabel1">Uncleared</td>
                            <td>
                                <asp:RadioButton ID="RdCleared" runat="server" Text="" GroupName="R" Checked="true" />
                            </td>
                            <td class="mylabel1">Cleared
                            </td>
                            <td>
                                <asp:RadioButton ID="RdAll" runat="server" Text="" GroupName="R" />
                            </td>
                            <td class="mylabel1">All
                            </td>
                        </tr>
                    </table>
                </td>

            </tr>
            <tr>
                <td colspan="5">
                    <table>
                        <tr id="trhypertext" runat="server" visible="false">
                            <td>
                                <asp:LinkButton ID="lnAllRecords" runat="server" NavigateUrl="javascript:void(0)" CssClass="btn btn-primary" OnClick="lnAllRecords_Click">All Records</asp:LinkButton>
                            </td>
                            <td>
                              <%--  <asp:HyperLink ID="hpFilterRecords" runat="server" Font-Underline="True" ForeColor="#8080FF"
                                    NavigateUrl="javascript:void(0)" CssClass="myhypertext" onclick="SearchVisible('Y')">Show Filter</asp:HyperLink>--%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trsearch" visible="false">
                <td colspan="5">
                    <table>
                        <tr>
                            <td valign="middle">
                                <dxe:ASPxDateEdit ID="AspTranDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                    UseMaskBehavior="True" Width="90px" NullText="Tran Date">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td valign="middle">
                                <asp:TextBox ID="txtVoucherNo" runat="server" Width="70px" CssClass="water form-control" Text="Voucher No"
                                    ToolTip="Voucher No"></asp:TextBox>
                            </td>
                            <td valign="middle">
                                <dxe:ASPxDateEdit ID="AspInsDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                    UseMaskBehavior="True" Width="90px" NullText="Ins Date">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInsNo" runat="server" CssClass="water form-control" Text="Ins No" ToolTip="Ins No"
                                    Width="60px"></asp:TextBox>
                            </td>
                            <td valign="middle">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtAccName" runat="server" CssClass="water form-control" Text="Main Account"
                                                ToolTip="Main Account"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtSubName" runat="server" CssClass="water form-control" Text="SubAccount"
                                                ToolTip="SubAccount"></asp:TextBox></td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="middle">
                                <asp:TextBox ID="txtPayAmt" runat="server" Width="60px" CssClass="water form-control" Text="Pmt Amount"
                                    ToolTip="Pmt Amount"></asp:TextBox>
                            </td>
                            <td valign="middle">
                                <asp:TextBox ID="txtReptAmt" runat="server" Width="60px" CssClass="water form-control" Text="Rcpt Amount"
                                    ToolTip="Rcpt Amount"></asp:TextBox>
                            </td>
                            <td valign="middle">
                                <%--<dxe:ASPxDateEdit ID="AspValueDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                    UseMaskBehavior="True" Width="84px" NullText="Value Date">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>--%>
                            </td>
                            <td valign="middle">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr runat="server" id="MainContent" visible="false">
                <td colspan="5">
                    <div class="grid_scrollNSEFO" style="width: 1104px">
                        <asp:GridView ID="grdDetails" AutoGenerateColumns="false" ShowFooter="True" runat="server" OnDataBound="grdDetails_DataBound" Width="1192px">
                            <Columns>
                                <asp:TemplateField HeaderText="Trans Date">
                                    <ItemTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTranDate" runat="server" Text='<%# Eval("cashbank_transactionDate") %>'
                                                        Width="80px" /></td>
                                            </tr>
                                            <tr style="display: none">
                                                <td>
                                                    <asp:Label ID="lblTransactionDate" runat="server" Text='<%# Eval("cashbank_transactionDate_test") %>' />
                                                    <asp:Label ID="lblMainAcc" runat="server" Text='<%# Eval("cashbankdetail_mainaccountid") %>' />
                                                    <asp:Label ID="lblid" runat="server" Text='<%# Eval("cashbankdetail_id") %>' Width="0px" />
                                                    <asp:Label ID="lblSubCode" runat="server" Text='<%# Eval("subaccount_code") %>' Width="0px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="EHEADER" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Voucher No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVoucherNo" runat="server" Text='<%# Eval("cashbank_vouchernumber") %>'
                                            Width="60px" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="EHEADER" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ins Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInsDate" runat="server" Text='<%# Eval("cashbankdetail_instrumentdate") %>'
                                            Width="80px" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="EHEADER" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ins No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInsNo" runat="server" Text='<%# Eval("cashbankdetail_instrumentnumber") %>'
                                            Width="50px" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="EHEADER" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblType" runat="server" Text='<%# Eval("type1") %>'
                                            Width="70px" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="EHEADER" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Account Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAccName" runat="server" Text='<%# Eval("AccountCode") %>' Width="400px" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="EHEADER" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pmt Amt">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPmtAmt" runat="server" Width="80px" Text='<%# Eval("cashbankdetail_paymentamount") %>'
                                            CssClass="rt" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <HeaderStyle CssClass="EHEADER" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rcpt Amt">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRcptAmt" runat="server" Text='<%# Eval("cashbankdetail_receiptamount") %>'
                                            Width="80px" CssClass="rt" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <HeaderStyle CssClass="EHEADER" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Value Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblValueDate" runat="server" Text='<%# Eval("cashbankdetail_bankvaluedate") %>' Width="80px"
                                            CssClass="rt" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="EHEADER" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Statement Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblstDate" runat="server" Text='<%# Eval("bankstatementdate") %>' Width="80px"
                                            CssClass="rt" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="EHEADER" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr style="visibility: hidden">
                <td class="rt">
                    <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btnUpdate" OnClick="btnUpdate_Click" /></td>
                <td>
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnUpdate" OnClick="btnCancel_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

