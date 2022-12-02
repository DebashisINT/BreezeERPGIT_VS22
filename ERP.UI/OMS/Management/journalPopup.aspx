<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.management_journalPopup" CodeBehind="journalPopup.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>--%>

    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajaxList_wofolder.js"></script>

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
            z-index: 100;
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
            z-index: 5;
        }

        form {
            display: inline;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function SignOff() {
            window.parent.SignOff();
        }
        var isCtrl = false;
        document.onkeyup = function (e) {
            if (event.keyCode == 17) {
                isCtrl = false;
            }
            if (event.keyCode == 27) {
                btnCancel_Click();
            }
        }
        document.onkeydown = function (e) {
            if (event.keyCode == 17) isCtrl = true;
            if (event.keyCode == 83 && isCtrl == true) {
                //run code for CTRL+S -- ie, save!
                var debit = document.getElementById('txtTDebit').value;
                var credit = document.getElementById('txtTCredit').value;
                if (debit == credit) {
                    document.getElementById('btnSave').click();
                    return false;
                }
                else {
                    alert('Credit And Debit Must Be Same');
                    return false;
                }
            }
        }
        function CallList(obj1, obj2, obj3) {
            ajax_showOptions(obj1, obj2, obj3);
        }
        function updateEditorText() {
            var code = txtAccountCode.GetText().toUpperCase();
            if (code == 'X' || code == 'Y' || code == 'Z') {
                alert('You Can not Enter This Code');
                txtAccountCode.SetText('');
            }
        }
        function CallMainAccount(obj1, obj2, obj3) {
            ajax_showOptions(obj1, obj2, obj3, null, 'Main');
        }
        function CallSubAccount(obj1, obj2, obj3) {
            var valSub;
            var HdVal = document.getElementById("hddnEdit").value;
            if (HdVal == 'Edit') {
                var BranchID = document.getElementById("ddlBranch").value;
                valSub = val + '~' + BranchID;
            }
            else
                valSub = val + '~' + 'N';
            ajax_showOptions(obj1, obj2, obj3, valSub, 'Sub');
        }
        function keyVal(obj) {
            var spObj = obj.split('~');
            val = spObj[0];
            valLedgerType = spObj[1];
            var testSubAcc = document.getElementById('<%=((HiddenField)grdAdd.FooterRow.FindControl("txtMainAccount_hidden")).ClientID%>');
            testSubAcc.value = val;
        }
        function SubAccountCheck(obj) {
            var obj1 = obj.split('_');
            if (valLedgerType.toUpperCase() != 'NONE') {
                var obj2 = 'grdAdd' + '_' + obj1[1] + '_' + 'txtSubAccount';
                var testSubAcc1 = document.getElementById(obj2);
                if (testSubAcc1.value == '') {
                    alert('SubAccount Name Required !!!');
                    testSubAcc1.focus();
                    testSubAcc1.select();
                    return false;
                }
            }
            var Withdraw = txtWithdraw.GetValue();
            var Receipt = txtReceipt.GetValue();
            var WithReceipt = parseFloat(Withdraw) + parseFloat(Receipt);
            if (WithReceipt == "0") {
                alert('Debit/Credit  Required !!!');
                return false;
            }
        }
        function SubAccountCheckUpdate(obj) {
            var obj1 = obj.split('_');
            if (valLedgerType.toUpperCase() != 'NONE') {
                var obj2 = 'grdAdd' + '_' + obj1[1] + '_' + 'txtEditSubAccount';
                var testSubAcc1 = document.getElementById(obj2);
                if (testSubAcc1.value == '') {
                    alert('SubAccount Name Required !!!');
                    testSubAcc1.focus();
                    testSubAcc1.select();
                    return false;
                }
            }
            var Withdraw = txtEditWithdraw.GetValue();
            var Receipt = txtEditRecpt.GetValue();
            var WithReceipt = parseFloat(Withdraw) + parseFloat(Receipt);
            if (WithReceipt == "0") {
                alert('Debit/Credit  Required !!!');
                return false;
            }
        }
        function btnCancel_Click() {
            var answer = confirm("Do you Want To Close This Window?");
            if (answer)
                parent.editwin.close();
        }
        function Page_Load() {
            document.getElementById("btnSave").disabled = true;
            document.getElementById("btnInsert").disabled = true;
        }
        function Page_Load1() {
            document.getElementById("tdSeg1").style.display = "none";
            document.getElementById("tdSeg2").style.display = "none";
        }
        function Button_Click() {
            document.getElementById("btnSave").disabled = false;
            document.getElementById("btnInsert").disabled = false;
        }
        function SetSubAcc1(obj) {
            var s = document.getElementById(obj);
            s.focus();
            s.select();
        }
        function keyVal1(obj) {
            val = obj
        }
        function PopulateData() {
            parent.RefreshGrid();
        }
        function Narration(obj) {
            document.getElementById("txtNarration1").value = obj;
        }
        function Narration1() {
            document.getElementById("txtNarration1").value = '';
        }
        function overChange(obj) {
            obj.style.backgroundColor = "#FFD497";
        }
        function OutChange(obj) {
            obj.style.backgroundColor = "#DDECFE";
        }
        function focusval(obj) {
            if (obj != '0.00') {
                var s = document.getElementById('txtNarration1');
                s.focus();
                s.select();
                txtReceipt.SetEnabled(false);
                txtReceipt.SetText('000000.00');
                OnlyPayment(obj, 'Dr');
            }
            else {
                txtReceipt.SetEnabled(true);
            }
        }
        function focusval1(obj) {
            if (obj != '0.00') {
                var s = document.getElementById('txtNarration1');
                s.focus();
                s.select();
                txtWithdraw.SetEnabled(false);
                txtWithdraw.SetText('000000.00');
                OnlyPayment(obj, 'Cr');
            }
            else {
                txtWithdraw.SetEnabled(true);
            }
        }

        function Efocusval(obj) {
            if (obj != '0.00') {
                var s = document.getElementById('txtNarration1');
                s.focus();
                s.select();
                txtEditRecpt.SetEnabled(false);
                txtEditRecpt.SetText('000000.00');
            }
            else {
                txtEditRecpt.SetEnabled(true);
            }
        }
        function Efocusval1(obj) {
            if (obj != '0.00') {
                var s = document.getElementById('txtNarration1');
                s.focus();
                s.select();
                txtEditWithdraw.SetEnabled(false);
                txtEditWithdraw.SetText('000000.00');
            }
            else {
                txtEditWithdraw.SetEnabled(true);
            }
        }
        function SelectSegment() {
            var obj = document.getElementById('ddlIntExchange').value;
            if (obj == "0") {
                document.getElementById("tdSeg1").style.display = "none";
                document.getElementById("tdSeg2").style.display = "none";
                txtAccountCode.SetText('JV');
                txtAccountCode.SetEnabled(true);
            }
            else {
                document.getElementById("tdSeg1").style.display = "inline";
                document.getElementById("tdSeg2").style.display = "inline";
                txtAccountCode.SetText('YF');
                txtAccountCode.SetEnabled(false);
            }
        }
        function SegmentName() {
            var obj = document.getElementById('ddlSegment').value;
            var obj1 = document.getElementById('ddlTntraExchange').value;
            if (obj == obj1) {
                alert('Segment And Segment2 Must Be Different');
                document.getElementById('ddlTntraExchange').selectedIndex = '0';
                return false;
            }
        }
        function Narration_Off() {
            document.getElementById('TrNarration').style.display = 'none';
        }
        function Narration_Val(obj) {
            document.getElementById('TrNarration').style.display = 'inline';
            document.getElementById('txtNarration1').value = obj;
        }
        function DateChange() {
            var sessionValFrom = "<%=Session["FinYearStart"]%>";
            var sessionValTo = "<%=Session["FinYearEnd"]%>";
            var sessionVal = "<%=Session["LastFinYear"]%>";
            var objsession = sessionVal.split('-');
            var MonthDate = tDate.GetDate().getMonth() + 1;
            var DayDate = tDate.GetDate().getDate();
            var YearDate = tDate.GetDate().getYear();
            var Cdate = MonthDate + "/" + DayDate + "/" + YearDate;
            var Sto = new Date(sessionValTo).getMonth() + 1;
            var SFrom = new Date(sessionValFrom).getMonth() + 1;

            var exd = new Date();
            if (YearDate >= objsession[0]) {
                if (Date.parse(Cdate) > Date.parse(sessionValTo) || Date.parse(Cdate) < Date.parse(sessionValFrom)) {
                    if (MonthDate < SFrom && YearDate == objsession[0]) {
                        alert('Enter Date Is Outside Of Financial Year !!');
                        var datePost = ((exd.getMonth() + 1) + '-' + exd.getDate() + '-' + objsession[1]);
                        tDate.SetDate(new Date(datePost));
                    }
                    else if (MonthDate > Sto && YearDate == objsession[1]) {
                        alert('Enter Date Is Outside Of Financial Year !!');
                        var datePost = ((exd.getMonth() + 1) + '-' + exd.getDate() + '-' + objsession[0]);
                        tDate.SetDate(new Date(datePost));
                    }
                    else if (YearDate != objsession[0] && YearDate != objsession[1]) {
                        alert('Enter Date Is Outside Of Financial Year !!');
                        var datePost;
                        if (MonthDate < 4)
                            datePost = ((exd.getMonth() + 1) + '-' + exd.getDate() + '-' + objsession[1]);
                        else
                            datePost = ((exd.getMonth() + 1) + '-' + exd.getDate() + '-' + objsession[0]);
                        tDate.SetDate(new Date(datePost));
                    }
                }
            }
            else {
                alert('Enter Date Is Outside Of Financial Year !!');
                var datePost;
                if (MonthDate < SFrom)
                    datePost = ((exd.getMonth() + 1) + '-' + exd.getDate() + '-' + objsession[1]);
                else
                    datePost = ((exd.getMonth() + 1) + '-' + exd.getDate() + '-' + objsession[0]);
                tDate.SetDate(new Date(datePost));
            }

        }
        function AcBalance(obj) {
            var comp = document.getElementById('ddlCompany').value;
            var segmnt = document.getElementById('ddlSegment').value;
            var date = document.getElementById('tDate_I').value;
            var dest = document.getElementById('lblAcBalance');
            var Suba = obj + "_hidden";
            var SubAcc = document.getElementById(Suba).value;
            var param = comp + '~' + segmnt + '~' + date + '~' + val + '~' + SubAcc;
            PageMethods.GetContactName(param, CallSuccess, CallFailed, dest);
        }
        function CallSuccess(res, destCtrl) {
            //destCtrl.innerText=res;
            var cc = res.substr(0, 1);
            if (cc == '-') {
                //cc=res * (-1);
                cc = res + ' (DR)';
                lbltype = 'DR';
                destCtrl.innerText = cc;
                destCtrl.style.color = 'red';
            }
            else {
                cc = res + ' (CR)';
                lbltype = 'CR';
                destCtrl.innerText = cc;
                destCtrl.style.color = 'blue';
            }
            lblval = res;
        }
        function CallFailed(res, destCtrl) {
            alert(res.get_message());
        }
        function alertMessage() {
            alert('This Voucher has multi branch enters.\n Please Provide a single account for counter entry !!')
        }
        function btnInsert_Click() {
            document.getElementById('Div1').style.display = 'inline';
            document.getElementById('btnInsert').disabled = true;
            document.getElementById('btnSave').click();
        }
        function AlertAfterInsert() {
            var answer = confirm("Do You Want To Print this page??");
            if (answer) {
                parent.editwin.close();
                document.getElementById('btnPrint').click();
            }
            else {
                parent.editwin.close();
            }
        }
        function OnlyPayment(obj, objType) {
            if (lbltype == 'DR' && objType == "Cr") {
                var str = lblval;
                str = str.replace(",", "");
                str = Math.abs(str);
                if (parseFloat(str) < parseFloat(obj)) {
                    alert('Credit Is Greater Than Debit');
                }
            }
            if (lbltype == 'CR' && objType == "Dr") {
                var str = lblval;
                str = str.replace(",", "");
                str = Math.abs(str);
                if (parseFloat(str) < parseFloat(obj)) {
                    alert('Debit Is Greater Than Credit');
                }
            }
        }
        function OnlyNarration(obj1, obj2, obj3) {
            ajax_showOptions(obj1, obj2, obj3);
        }
        FieldName = 'txtPrefix';
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="True" AsyncPostBackTimeout="36000">
        </asp:ScriptManager>
        <table class="TableMain100">
            <tr>
                <td>
                    <table class="TableMain100">
                        <tr>
                            <td style="text-align: left;">Company :
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlCompany" DataSourceID="dsCompany" TabIndex="1" AutoPostBack="true"
                                    DataTextField="cmp_Name" DataValueField="cmp_internalId" runat="server" Width="248px"
                                    Font-Size="12px" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left;">Segment :
                            </td>
                            <td style="text-align: left; width: 162px;">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlSegment" runat="server" Width="121px" TabIndex="2" Font-Size="12px">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlCompany" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <td style="text-align: left;">Branch :
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlBranch" runat="server" DataSourceID="dsBranch" TabIndex="3"
                                    DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="150px"
                                    Font-Size="12px">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left;">SettlementNo :
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtSettno" runat="server" Width="95px" TabIndex="4" Font-Size="12px"></asp:TextBox>
                                <asp:HiddenField ID="txtSettno_hidden" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">T. Date :
                            </td>
                            <td style="text-align: left;">
                                <dxe:ASPxDateEdit ID="tDate" runat="server" EditFormat="Custom" TabIndex="5" ClientInstanceName="tDate"
                                    UseMaskBehavior="True" Font-Size="12px" Width="247px">
                                    <ClientSideEvents DateChanged="function(s,e){DateChange(); }" />
                                </dxe:ASPxDateEdit>
                            </td>
                            <td style="text-align: left;">Prefix :
                            </td>
                            <td style="text-align: left; width: 162px;">
                                <dxe:ASPxTextBox ID="txtAccountCode" ClientInstanceName="txtAccountCode" TabIndex="6"
                                    runat="server" Width="124px" MaxLength="2">
                                    <ValidationSettings>
                                        <RequiredField IsRequired="True" ErrorText="Select Account Code" />
                                    </ValidationSettings>
                                    <ClientSideEvents KeyPress="function(s,e){window.setTimeout('updateEditorText()', 10);}" />
                                </dxe:ASPxTextBox>
                            </td>
                            <td style="text-align: left;">Bill No :
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtBillNo" runat="server" Width="120px" TabIndex="8" Font-Size="12px"></asp:TextBox>
                            </td>
                            <td style="text-align: left;">Voucher No. :
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtVoucherNo" runat="server" ReadOnly="true" Width="107px" TabIndex="7"
                                    Font-Size="12px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">Narration :
                            </td>
                            <td style="text-align: left;" colspan="4">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtNarration" runat="server" TextMode="MultiLine" Width="436px"
                                                TabIndex="8" Font-Size="15px" Height="60px" onkeyup="OnlyNarration(this,'Narration',event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <table width="100%">
                                                <tr>
                                                    <td style="text-align: left;">A/C Balance</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAcBalance" runat="server" Text=""></asp:Label></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlIntExchange" runat="server" Font-Size="12px" Width="124px">
                                    <asp:ListItem Value="0">Intra Segment</asp:ListItem>
                                    <asp:ListItem Value="1">Inter Segment</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: left;" id="tdSeg1">Segment2 :
                            </td>
                            <td style="text-align: left;" id="tdSeg2">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlTntraExchange" runat="server" Width="121px" TabIndex="2"
                                            Font-Size="12px">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlCompany" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td id='Div1' style="height: 20px; display: none">
                    <div style='position: absolute; font-family: arial; font-size: 30; left: 50%; top: 50%; background-color: white; layer-background-color: white; height: 80; width: 150;'>
                        <table width='100' height='35' border='1' cellpadding='0' cellspacing='0' bordercolor='#C0D6E4'>
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
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="grdAdd" runat="server" DataKeyNames="CashReportID" CssClass="gridcellleft"
                                Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None" BorderWidth="1px"
                                BorderColor="#507CD1" AutoGenerateColumns="False" OnRowEditing="grdAdd_RowEditing"
                                OnRowDeleting="grdAdd_RowDeleting" ShowFooter="True" OnRowCommand="grdAdd_RowCommand"
                                OnRowCreated="grdAdd_RowCreated" OnRowDataBound="grdAdd_RowDataBound" OnRowCancelingEdit="grdAdd_RowCancelingEdit"
                                OnRowUpdating="grdAdd_RowUpdating">
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                                <EditRowStyle BackColor="#E59930" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <HeaderStyle Font-Bold="False" HorizontalAlign="Center" ForeColor="Black" CssClass="EHEADER"
                                    BorderColor="AliceBlue" BorderWidth="1px" />
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:TemplateField HeaderText="MainAccount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMainAccount" runat="server" Text='<%# Eval("MainAccount1") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditMainAccount" TabIndex="9" Font-Size="12px" Text='<%# Eval("MainAccount1") %>'
                                                Width="180px" onkeyup="CallMainAccount(this,'MainAccountJournal',event)" runat="server"></asp:TextBox>
                                            <asp:HiddenField ID="txtEditMainAccount_hidden" runat="server" Value='<%# Eval("JournalVoucherDetail_MainAccountCode") %>' />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtMainAccount" TabIndex="9" Font-Size="12px" Width="180px" onkeyup="CallMainAccount(this,'MainAccountJournal',event)"
                                                runat="server"></asp:TextBox>
                                            <asp:HiddenField ID="txtMainAccount_hidden" runat="server" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SubAccount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubAccount" runat="server" Text='<%# Eval("SubAccount1") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditSubAccount" TabIndex="10" Font-Size="12px" Text='<%# Eval("SubAccount1") %>'
                                                Width="180px" onkeyup="CallSubAccount(this,'SubAccountMod',event)" runat="server"></asp:TextBox>
                                            <asp:HiddenField ID="txtEditSubAccount_hidden" runat="server" Value='<%# Eval("JournalVoucherDetail_SubAccountCode") %>' />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtSubAccount" TabIndex="10" Font-Size="12px" Width="180px" onkeyup="CallSubAccount(this,'SubAccountMod',event)"
                                                onblur="AcBalance(this.id)" runat="server"></asp:TextBox>
                                            <asp:HiddenField ID="txtSubAccount_hidden" runat="server" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false" HeaderText="Value Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("JournalVoucherDetail_ValueDate1") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <dxe:ASPxDateEdit ID="dtEditAspxDate" runat="server" Value='<%# Eval("JournalVoucherDetail_ValueDate") %>'
                                                EditFormat="Custom" TabIndex="11" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True"
                                                Font-Size="12px" Width="100px">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                            </dxe:ASPxDateEdit>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <dxe:ASPxDateEdit ID="dtAspxDate" runat="server" EditFormat="Custom" TabIndex="11"
                                                EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" Font-Size="12px" Width="100px">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                            </dxe:ASPxDateEdit>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Debit" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWithdraw" runat="server" Text='<%# Eval("WithDrawl") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <dxe:ASPxTextBox ID="txtEditWithdraw" runat="server" TabIndex="12" ClientInstanceName="txtEditWithdraw"
                                                HorizontalAlign="Right" Width="185px" Text='<%# Eval("JournalVoucherDetail_AmountDr") %>'>
                                                <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                <ValidationSettings ErrorDisplayMode="None">
                                                </ValidationSettings>
                                                <ClientSideEvents LostFocus="function(s,e){Efocusval(s.GetValue());}" />
                                            </dxe:ASPxTextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <dxe:ASPxTextBox ID="txtWithdraw" runat="server" Width="185px" ClientInstanceName="txtWithdraw"
                                                TabIndex="12" HorizontalAlign="Right">
                                                <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                <ValidationSettings ErrorDisplayMode="None">
                                                </ValidationSettings>
                                                <ClientSideEvents LostFocus="function(s,e){focusval(s.GetValue());}" />
                                            </dxe:ASPxTextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Credit" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReceipt" runat="server" Text='<%# Eval("Receipt") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <dxe:ASPxTextBox ID="txtEditRecpt" runat="server" TabIndex="13" ClientInstanceName="txtEditRecpt"
                                                HorizontalAlign="Right" Width="185px" Text='<%# Eval("JournalVoucherDetail_AmountCr") %>'>
                                                <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                <ValidationSettings ErrorDisplayMode="None">
                                                </ValidationSettings>
                                                <ClientSideEvents LostFocus="function(s,e){Efocusval1(s.GetValue());}" />
                                            </dxe:ASPxTextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <dxe:ASPxTextBox ID="txtReceipt" runat="server" Width="185px" ClientInstanceName="txtReceipt"
                                                TabIndex="13" HorizontalAlign="Right">
                                                <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                <ValidationSettings ErrorDisplayMode="None">
                                                </ValidationSettings>
                                                <ClientSideEvents LostFocus="function(s,e){focusval1(s.GetValue());}" />
                                            </dxe:ASPxTextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" Font-Size="12px" Text='<%# Eval("CashReportID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit" ShowHeader="False" ItemStyle-HorizontalAlign="Right">
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="True" CssClass="btnUpdate"
                                                OnClientClick="javascript:return SubAccountCheckUpdate(this.id);" CommandName="Update" Text="Save"></asp:LinkButton>|
                                                <asp:LinkButton ID="LinkButton4" runat="server" CausesValidation="False" CssClass="btnUpdate"
                                                    CommandName="Cancel" Text="Cancel"></asp:LinkButton>|
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CssClass="btnUpdate"
                                                CommandName="Edit" Text="Edit"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete" ShowHeader="False" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CssClass="btnUpdate"
                                                CommandName="Delete" OnClientClick="javascript:return confirm('Do You Want To Delete This Record ?')"
                                                Text="Delete">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Button ID="Button1" runat="server" Text="Add" CausesValidation="false" TabIndex="15"
                                                OnClientClick="javascript:return SubAccountCheck(this.id);" CommandName="Insert"
                                                onfocus="overChange(this)" onBlur="OutChange(this)" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <table width="100%">
                                <tr id="Tdebit" runat="server">
                                    <td style="width: 50%"></td>
                                    <td style="text-align: left;">Total
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:TextBox ID="txtTDebit" runat="server" ReadOnly="true" Font-Size="12px" Width="79px"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:TextBox ID="txtTCredit" runat="server" ReadOnly="true" Font-Size="12px" Width="79px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr id="TrNarration">
                            <td style="text-align: left;">Narration :
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtNarration1" runat="server" TextMode="MultiLine" TabIndex="14"
                                    Width="606px" Font-Size="15px" Height="61px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <input id="btnInsert" type="button" value="Save" class="btnUpdate" style="width: 62px; height: 25px"
                        onclick="btnInsert_Click()" />
                    <input id="btnCancel" type="button" value="Cancel" class="btnUpdate" style="width: 62px; height: 25px"
                        onclick="btnCancel_Click()" />
                    <asp:HiddenField ID="hddnEdit" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="display: none">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnUpdate" Height="25px"
                        Width="62px" OnClick="btnSave_Click" />
                    <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" />
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="dsCompany" runat="server"
            ConflictDetection="CompareAllValues" SelectCommand="select cmp_internalId,cmp_Name from tbl_master_company where cmp_internalId in(select exch_compId from tbl_master_companyExchange)"></asp:SqlDataSource>
        <asp:SqlDataSource ID="dsBranch" runat="server" 
            ConflictDetection="CompareAllValues" SelectCommand="SELECT BRANCH_id AS BANKBRANCH_ID , RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH"></asp:SqlDataSource>
    </div>
</asp:Content>
