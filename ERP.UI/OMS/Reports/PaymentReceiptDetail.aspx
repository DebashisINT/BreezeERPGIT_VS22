<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_PaymentReceiptDetail" CodeBehind="PaymentReceiptDetail.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   

    <script type="text/javascript" src="/assests/js/ajaxList_rootfile.js"></script>

    


    <script language="javascript" type="text/javascript">


        function Page_Load()///Call Into Page Load
        {
            Hide('Td_Filter');
            Hide('Tab_Grid');
            document.getElementById('TxtShowUnclearedformorethan_I').disabled = false;
            Hide('Tr_SubAccount');
            Show('Tab_Selection');
            Hide('showFilter');
            selecttion();
          //  height();
        }
      

        function Hide(obj) {
            document.getElementById(obj).style.display = 'none';
        }
        function Show(obj) {
            document.getElementById(obj).style.display = 'inline';
        }

        function btnAddsubscriptionlist_click() {

            var userid = document.getElementById('txtSelectionID');
            if (userid.value != '') {
                var ids = document.getElementById('txtSelectionID_hidden');
                var listBox = document.getElementById('lstSlection');
                var tLength = listBox.length;


                var no = new Option();
                no.value = ids.value;
                no.text = userid.value;
                listBox[tLength] = no;
                var recipient = document.getElementById('txtSelectionID');
                recipient.value = '';
            }
            else
                alert('Please search name and then Add!')
            var s = document.getElementById('txtSelectionID');
            s.focus();
            s.select();

        }
        function clientselectionfinal() {
            var listBoxSubs = document.getElementById('lstSlection');

            var cmb = document.getElementById('cmbsearchOption');
            var listIDs = '';
            var i;
            if (listBoxSubs.length > 0) {

                for (i = 0; i < listBoxSubs.length; i++) {
                    if (listIDs == '')
                        listIDs = listBoxSubs.options[i].value + ';' + listBoxSubs.options[i].text;
                    else
                        listIDs += ',' + listBoxSubs.options[i].value + ';' + listBoxSubs.options[i].text;
                }
                var sendData = cmb.value + '~' + listIDs;
                CallServer(sendData, "");

            }
            var i;
            for (i = listBoxSubs.options.length - 1; i >= 0; i--) {
                listBoxSubs.remove(i);
            }

            Hide('showFilter');
            //document.getElementById('btnScreen').disabled=false;
        }

        function btnRemovefromsubscriptionlist_click() {

            var listBox = document.getElementById('lstSlection');
            var tLength = listBox.length;

            var arrTbox = new Array();
            var arrLookup = new Array();
            var i;
            var j = 0;
            for (i = 0; i < listBox.options.length; i++) {
                if (listBox.options[i].selected && listBox.options[i].value != "") {

                }
                else {
                    arrLookup[listBox.options[i].text] = listBox.options[i].value;
                    arrTbox[j] = listBox.options[i].text;
                    j++;
                }
            }
            listBox.length = 0;
            for (i = 0; i < j; i++) {
                var no = new Option();
                no.value = arrLookup[arrTbox[i]];
                no.text = arrTbox[i];
                listBox[i] = no;
            }
        }
        function FnAccountType(obj)/////Account Type 
        {
            if (obj == 'Customers' || obj == 'NSDL Clients' || obj == 'CDSL Clients') {
                Hide('Tr_MainAc');
                Show('Tr_SubAccount');
            }
            else {
                Show('Tr_MainAc');
                Hide('Tr_SubAccount');
            }
        }
        function FnMainAc(obj) {
            if (obj == 'a') {
                Hide('Td_MainAcSpecific');
            }
            else {
                Show('Td_MainAcSpecific');
            }
        }

        function FunCallAjaxList(objID, objEvent, ObjType) {
            var strQuery_Table = '';
            var strQuery_FieldName = '';
            var strQuery_WhereClause = '';
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = '';

            if (ObjType == 'MainAc') {
                strQuery_Table = "Master_MainAccount";
                strQuery_FieldName = "distinct top 10 ltrim(rtrim(MainAccount_Name))+\' [\'+rtrim(MainAccount_AccountCode)+\']\',rtrim(MainAccount_AccountCode)";
                strQuery_WhereClause = " (MainAccount_Name like (\'%RequestLetter%') or MainAccount_AccountCode like (\'%RequestLetter%') or MainAccount_AccountType like (\'%RequestLetter%') ) and MainAccount_SubLedgerType in ('Customers','CDSL Clients','NSDL Clients','None','Brokers','Custom','Sub Brokers')";
                CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
                ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars(CombinedQuery), 'Main');
            }
            else {
                if (document.getElementById('cmbsearchOption').value == "Company") {
                    strQuery_Table = "tbl_master_company";
                    strQuery_FieldName = "distinct top 10 cmp_Name,cmp_internalid";
                    strQuery_WhereClause = " cmp_Name like (\'%RequestLetter%')";
                }
                else if (document.getElementById('cmbsearchOption').value == "Segment") {
                    strQuery_Table = "(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName +\'-'\ + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE Where  TMCE.EXCH_COMPID=\'<%=Session["LastCompany"]%>'\) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID)as TAB";
                    strQuery_FieldName = "distinct top 10 EXCHANGENAME,SEGMENTID";
                    strQuery_WhereClause = " EXCHANGENAME like (\'%RequestLetter%')";
                }
                else if (document.getElementById('cmbsearchOption').value == "Bank") {
                    strQuery_Table = "master_mainaccount";
                    strQuery_FieldName = "distinct top 10 ltrim(rtrim(MainAccount_Name))+\' [\'+rtrim(MainAccount_AccountCode)+\']\',rtrim(MainAccount_AccountCode)";

                    if (document.getElementById('RdbCurrentCompany').checked) {
                        strQuery_WhereClause = " (MainAccount_Name like (\'%RequestLetter%') or MainAccount_AccountCode like (\'%RequestLetter%') or MainAccount_AccountType like (\'%RequestLetter%') ) and MainAccount_BankCashType='Bank' and MainAccount_BankCompany='<%=Session["LastCompany"]%>'";
                    }
                    else if (document.getElementById('RdbSelectedCompany').checked) {
                        strQuery_WhereClause = " (MainAccount_Name like (\'%RequestLetter%') or MainAccount_AccountCode like (\'%RequestLetter%') or MainAccount_AccountType like (\'%RequestLetter%') ) and MainAccount_BankCashType='Bank' and MainAccount_BankCompany in (" + document.getElementById('HiddenField_Company').value + ")";
                    }
                    else {
                        strQuery_WhereClause = " (MainAccount_Name like (\'%RequestLetter%') or MainAccount_AccountCode like (\'%RequestLetter%') or MainAccount_AccountType like (\'%RequestLetter%') ) and MainAccount_BankCashType='Bank'";
                    }

                }
                else if (document.getElementById('cmbsearchOption').value == "SubAccount") {

                    if (document.getElementById('DdlAccountType').value == "Customers") {
                        strQuery_Table = "tbl_master_contact,tbl_master_branch ,tbl_master_contactexchange";
                        strQuery_FieldName = "distinct top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID )";
                        strQuery_WhereClause = " cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' and (crg_tcode like (\'%RequestLetter%') or CNT_FIRSTNAME like (\'%RequestLetter%')) and branch_id in (<%=Session["userbranchHierarchy"]%>)";
                    }
                    else if (document.getElementById('DdlAccountType').value == "NSDL Clients") {
                        strQuery_Table = "master_nsdlclients,tbl_master_branch";
                        strQuery_FieldName = "distinct top 10 rtrim(nsdlclients_benfirstholdername)+' ['+isnull(rtrim(nsdlclients_benaccountid),'')+']'+'['+isnull(rtrim(branch_code),'')+']' ,rtrim(nsdlclients_DPID)+rtrim(nsdlclients_benaccountid)";
                        strQuery_WhereClause = " nsdlclients_branchid=branch_id and (nsdlclients_benaccountid like (\'%RequestLetter%') or nsdlclients_benfirstholdername like (\'%RequestLetter%')) and nsdlclients_branchid in (<%=Session["userbranchHierarchy"]%>)";
                     }
                     else if (document.getElementById('DdlAccountType').value == "CDSL Clients") {
                         strQuery_Table = "master_cdslclients,tbl_master_branch";
                         strQuery_FieldName = "distinct top 10 rtrim(cdslclients_firstholdername)+' ['+isnull(rtrim(cdslclients_benaccountnumber),'')+']'+'['+isnull(rtrim(branch_code),'')+']' ,cdslclients_BOID";
                         strQuery_WhereClause = " cdslclients_branchid=branch_id and (cdslclients_benaccountnumber like (\'%RequestLetter%') or cdslclients_firstholdername like (\'%RequestLetter%')) and cdslclients_branchid in (<%=Session["userbranchHierarchy"]%>)";
                      }
                      else {
                          strQuery_Table = "master_subaccount";
                          strQuery_FieldName = "distinct  distinct top 10 SubAccount_Name+' ['+SubAccount_Code+']' as SubAccount_Name,SubAccount_Code";
                          strQuery_WhereClause = " (SubAccount_Name like (\'%RequestLetter%') or SubAccount_Code like (\'%RequestLetter%')) and SubAccount_MainAcReferenceID='" + document.getElementById('HiddenField_txtMainAccountCode').value + "'";
                      }

          }
    CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
    ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars(CombinedQuery));
}

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
function keyVal(obj) {
    var obj1 = obj.split('~');
    document.getElementById('HiddenField_txtMainAccountCode').value = obj1[0];
    document.getElementById('HiddenField_txtSubLedgerType').value = obj1[1];

    if (obj1[1] == 'Custom' || obj1[1] == 'Brokers' || obj1[1] == 'Sub Brokers') {
        Show('Tr_SubAccount');
    }
    else if (obj1[1] == 'None') {
        Hide('Tr_SubAccount');
    }
    else {
        Show('Tr_SubAccount');
    }

}
function FnSubAc(obj) {
    if (obj == "a")
        Hide('showFilter');
    else {
        Show('showFilter');
        document.getElementById('cmbsearchOption').value = 'SubAccount';
    }

}

function fnCompany(obj) {
    if (obj == "a")
        Hide('showFilter');
    else {
        Show('showFilter');
        document.getElementById('cmbsearchOption').value = 'Company';
    }

}
function fnSegment(obj) {
    if (obj == "a")
        Hide('showFilter');
    else if (obj == "c") {
        Hide('showFilter');
        Show('Td_Specific');
    }
    else {
        var cmb = document.getElementById('cmbsearchOption');
        cmb.value = 'Segment';
        Show('showFilter');
    }

}
function fnBank(obj) {
    if (obj == "a")
        Hide('showFilter');
    else {
        Show('showFilter');
        document.getElementById('cmbsearchOption').value = 'Bank';
    }

}
function FnChkShowUnclearedformorethan(obj) {
    if (obj.checked == true) {
        document.getElementById('TxtShowUnclearedformorethan_I').disabled = false;
    }
    else {
        document.getElementById('TxtShowUnclearedformorethan_I').disabled = true;
    }

}
function BtnShow_Click() {
    cGridPaymentReceiptQuery.PerformCallback("Show~" + document.getElementById("A1").innerText);
}
function OnPageNo_Click(obj) {
    selecttion();
    var i = document.getElementById(obj).innerText;
    cGridPaymentReceiptQuery.PerformCallback("SearchByNavigation~" + i);

}
function GridPaymentReceiptQuery_EndCallBack() {

    var strUndefined = new String(cGridPaymentReceiptQuery.cpIsEmptyDsSearch);
    if (strUndefined != "NoRecord") {
        document.getElementById("B_PageNo").innerText = strUndefined.split('~')[1];
        document.getElementById("B_TotalPage").innerText = strUndefined.split('~')[2];
        document.getElementById("B_TotalRows").innerText = strUndefined.split('~')[3];

        var i = document.getElementById("A1").innerText;
        var TotalPage = strUndefined.split('~')[2];
        if (parseInt(i) <= TotalPage && parseInt(i) == 1) {

            n = (parseInt(TotalPage) - parseInt(i) > 10) ? parseInt(11) : parseInt(TotalPage) - parseInt(i) + 2;

            for (a = 1; a < n; a++) {

                var obj = "A" + a;
                document.getElementById(obj).innerText = a;
            }
            for (a = n; a < 11; a++) {

                var obj = "A" + a;
                document.getElementById(obj).innerText = "";
            }
        }

        Hide('Tab_Selection');
        Show('Tab_Grid');
        Hide('showFilter');
        Show('Td_Filter');

    }
    else if (strUndefined == "NoRecord") {
        Hide('Tab_Grid');
        Show('Tab_Selection');
        Hide('showFilter');
        Hide('Td_Filter');
        alert('No Record Found');
    }


    height();
}
function OnLeftNav_Click() {
    var i = document.getElementById("A1").innerText;
    if (parseInt(i) > 1) {
        i = parseInt(i) - 10;
        for (l = 1; l < 11; l++) {
            var obj = "A" + l;
            document.getElementById(obj).innerText = i++;
        }
        cGridPaymentReceiptQuery.PerformCallback("SearchByNavigation~" + document.getElementById("A1").innerText);
    }
    else {
        alert('You are on the Beginning');
    }
}
function OnRightNav_Click() {
    var TestEnd = document.getElementById("A10").innerText;
    var TotalPage = document.getElementById("B_TotalPage").innerText;
    if (TestEnd == "" || TestEnd == TotalPage) {
        alert('You are at the End');
        return;
    }
    var i = document.getElementById("A1").innerText;
    if (parseInt(i) < TotalPage) {
        i = parseInt(i) + 10;
        var n = parseInt(TotalPage) - parseInt(i) > 10 ? parseInt(11) : parseInt(TotalPage) - parseInt(i) + 2;
        for (r = 1; r < n; r++) {
            var obj = "A" + r;
            document.getElementById(obj).innerText = i++;
        }
        for (r = n; r < 11; r++) {
            var obj = "A" + r;
            document.getElementById(obj).innerText = "";
        }
        cGridPaymentReceiptQuery.PerformCallback("SearchByNavigation~" + document.getElementById("A1").innerText);
    }
    else {
        alert('You are at the End');
    }
}
function selecttion() {
    var combo = document.getElementById('cmbExport');
    combo.value = 'Ex';
}
FieldName = 'lstSlection';
    </script>

    <script type="text/ecmascript">
        function ReceiveServerData(rValue) {

            var j = rValue.split('~');

            if (j[0] == 'Company') {
                document.getElementById('HiddenField_Company').value = j[1];
            }
            if (j[0] == 'Segment') {
                document.getElementById('HiddenField_Segment').value = j[1];
            }
            if (j[0] == 'Bank') {
                document.getElementById('HiddenField_BRSAC').value = j[1];
            }
            if (j[0] == 'SubAccount') {
                if (document.getElementById('DdlAccountType').value == "Customers") {
                    document.getElementById('HiddenField_SubAC').value = j[1];
                }
                else if (document.getElementById('DdlAccountType').value == "NSDL Clients") {
                    document.getElementById('HiddenField_SubAC').value = j[1];
                }
                else if (document.getElementById('DdlAccountType').value == "CDSL Clients") {
                    document.getElementById('HiddenField_SubAC').value = j[1];
                }
                else {
                    document.getElementById('HiddenField_SubAC').value = j[1];
                }
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Query Payment/Receipt Transactions</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <%--<td class="EHEADER" colspan="0" style="text-align: center; height: 20px;">
                    <strong><span id="SpanHeader" style="color: #000099">Query Payment/Receipt Transactions</span></strong></td>--%>
                <td class="EHEADER" width="15%" id="Td_Filter" style="height: 22px">
                    <a href="javascript:void(0);" onclick="Page_Load();"><span style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight: bold">Filter</span></a>

                </td>
            </tr>
        </table>
        <table>
            <tr valign="top">
                <td>
                    <table id="Tab_Selection">
                        <tr>
                            <td valign="top">
                                <table>
                                    <tr>
                                        <td class="gridcellleft" valign="top">
                                            <table>
                                                <tr>
                                                    <td valign="top">
                                                        <table cellpadding="1" cellspacing="1" class="tableClass">
                                                            <tr>
                                                                <td class="gridcellleft" width="150px">Account Type :</td>
                                                                <td>
                                                                    <asp:DropDownList ID="DdlAccountType" runat="server" Width="100px" Font-Size="12px"
                                                                        onchange="FnAccountType(this.value)">
                                                                        <asp:ListItem Value="ALL">ALL</asp:ListItem>
                                                                        <asp:ListItem Value="Customers">Customers</asp:ListItem>
                                                                        <asp:ListItem Value="NSDL Clients">NSDL Clients</asp:ListItem>
                                                                        <asp:ListItem Value="CDSL Clients">CDSL Clients</asp:ListItem>
                                                                        <asp:ListItem Value="Other Accounts">Other Accounts</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="Tr_MainAc">
                                                    <td>
                                                        <table cellpadding="1" cellspacing="1" class="tableClass">
                                                            <tr>
                                                                <td class="gridcellleft" width="150px">Main Account :
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="RdMainAcAll" Checked="true" runat="server" GroupName="a" onclick="FnMainAc('a')" />
                                                                    All</td>
                                                                <td>
                                                                    <asp:RadioButton ID="RdMainAcSpecific" runat="server" GroupName="a" onclick="FnMainAc('b')" />
                                                                    Specific
                                                                </td>
                                                                <td style="display: none;" id="Td_MainAcSpecific">
                                                                    <asp:TextBox ID="txtMainAccount" TabIndex="0" runat="server" Width="250px" Font-Size="12px"
                                                                        onkeyup="FunCallAjaxList(this,event,'MainAc')"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="Tr_SubAccount">
                                                    <td>
                                                        <table cellpadding="1" cellspacing="1" class="tableClass">
                                                            <tr>
                                                                <td class="gridcellleft" width="150px">Sub-Account :</td>
                                                                <td>
                                                                    <asp:RadioButton ID="RdbSubAcAll" runat="server" Checked="True" GroupName="b" onclick="FnSubAc('a')" />
                                                                    All
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="RdbSubAcSelected" runat="server" GroupName="b" onclick="FnSubAc('b')" />Selected
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table cellpadding="1" cellspacing="1" class="tableClass">
                                                            <tr>
                                                                <td class="gridcellleft" width="150px">Company :</td>
                                                                <td>
                                                                    <asp:RadioButton ID="RdbAllCompany" runat="server" GroupName="c" onclick="fnCompany('a')" />
                                                                    All
                                                                        <asp:RadioButton ID="RdbCurrentCompany" runat="server" Checked="True" GroupName="c"
                                                                            onclick="fnCompany('a')" />
                                                                    Current
                                                                        <asp:RadioButton ID="RdbSelectedCompany" runat="server" GroupName="c" onclick="fnCompany('b')" />Selected
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table cellpadding="1" cellspacing="1" class="tableClass">
                                                            <tr>
                                                                <td class="gridcellleft" width="150px">Segment:</td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbSegmentAll" runat="server" Checked="True" GroupName="d" onclick="fnSegment('a')" />
                                                                    All
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbSegmentSpecific" runat="server" GroupName="d"
                                                                        onclick="fnSegment('c')" />Current
                                                                </td>
                                                                <td id="Td_Specific">[ <span id="litSegmentMain" runat="server" style="color: Maroon"></span>]
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdSegmentSelected" runat="server" GroupName="d" onclick="fnSegment('b')" />Selected
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table cellpadding="1" cellspacing="1" class="tableClass">
                                                            <tr>
                                                                <td class="gridcellleft" width="150px">Bank Name :</td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbBankAll" runat="server" Checked="True" GroupName="e" onclick="fnBank('a')" />
                                                                    All
                                                                        <asp:RadioButton ID="rdBankSelected" runat="server" GroupName="e" onclick="fnBank('b')" />Selected
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td valign="top">
                                            <table cellpadding="1" cellspacing="1" id="showFilter">
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="320px" onkeyup="FunCallAjaxList(this,event,'Other')"></asp:TextBox></td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                            Enabled="false">
                                                            <asp:ListItem Value="Company">Company</asp:ListItem>
                                                            <asp:ListItem Value="Segment">Segment</asp:ListItem>
                                                            <asp:ListItem Value="Bank">Bank</asp:ListItem>
                                                            <asp:ListItem Value="SubAccount">SubAccount</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <a id="P4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                                            style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                                                style="color: #009900; font-size: 8pt;"> </span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:ListBox ID="lstSlection" runat="server" Font-Size="12px" Height="120px" Width="400px"></asp:ListBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: center">
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <a id="P1" href="javascript:void(0);" onclick="clientselectionfinal()"><span style="color: #000099; text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
                                                                </td>
                                                                <td>
                                                                    <a id="P2" href="javascript:void(0);" onclick="btnRemovefromsubscriptionlist_click()">
                                                                        <span style="color: #cc3300; text-decoration: underline; font-size: 8pt;">Remove</span></a>
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
                            <td>
                                <table>
                                    <tr>
                                        <td class="gridcellleft">
                                            <table cellpadding="1" cellspacing="1" class="tableClass">
                                                <tr>
                                                    <td class="gridcellleft" width="150px">Transaction Date
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxDateEdit ID="DtFromDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                            Font-Size="12px" Width="120px" ClientInstanceName="DtFromDate">
                                                            <DropDownButton Text="From">
                                                            </DropDownButton>
                                                        </dxe:ASPxDateEdit>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxDateEdit ID="DtToDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                            Font-Size="12px" Width="120px" ClientInstanceName="DtToDate">
                                                            <DropDownButton Text="To">
                                                            </DropDownButton>
                                                        </dxe:ASPxDateEdit>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>

                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td class="gridcellleft">
                                            <table cellpadding="1" cellspacing="1" class="tableClass">
                                                <tr>
                                                    <td class="gridcellleft" width="150px">Amount:
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxTextBox ID="TxtAmntFrom" runat="server" HorizontalAlign="Right" Width="150px"
                                                            Text="00.01" Height="10px">
                                                            <ValidationSettings ErrorDisplayMode="None">
                                                            </ValidationSettings>
                                                            <MaskSettings Mask="&lt;0..999999999999999999g&gt;.&lt;0000..999&gt;" IncludeLiterals="DecimalSymbol" />
                                                        </dxe:ASPxTextBox>
                                                    </td>
                                                    <td>To
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxTextBox ID="TxtAmntTo" runat="server" HorizontalAlign="Right" Width="150px"
                                                            Text="9999999999.99" Height="10px">
                                                            <ValidationSettings ErrorDisplayMode="None">
                                                            </ValidationSettings>
                                                            <MaskSettings Mask="&lt;0..999999999999999999g&gt;.&lt;0000..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                        </dxe:ASPxTextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td class="gridcellleft">
                                            <table cellpadding="1" cellspacing="1" class="tableClass">
                                                <tr>
                                                    <td class="gridcellleft" >
                                                        <asp:CheckBox ID="ChkShowOnlyThirdParty" runat="server" />
                                                        Show Only Third Party Receipts
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td class="gridcellleft">
                                            <table cellpadding="1" cellspacing="1" class="tableClass">
                                                <tr>
                                                    <td >
                                                        <asp:CheckBox ID="ChkShowUnclearedformorethan" runat="server" onclick="FnChkShowUnclearedformorethan(this)" />
                                                    </td>
                                                    <td >Show Uncleared For More Than
                                                    </td>

                                                    <td  valign="top">
                                                        <dxe:ASPxTextBox ID="TxtShowUnclearedformorethan" runat="server" HorizontalAlign="Right"
                                                            Width="50px">
                                                            <MaskSettings Mask="&lt;0..9999999999g&gt;" IncludeLiterals="DecimalSymbol" />
                                                            <ValidationSettings ErrorDisplayMode="None"></ValidationSettings>
                                                        </dxe:ASPxTextBox>
                                                    </td>

                                                    <td >Days
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td class="gridcellleft">
                                            <table cellpadding="1" cellspacing="1" class="tableClass">
                                                <tr>
                                                    <td class="gridcellleft" width="150px">Instrument Number :</td>
                                                    <td>
                                                        <asp:TextBox ID="txt1stInstrumentNumber" Width="100px" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt2ndInstrumentNumber" Width="100px" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td class="gridcellleft">
                                            <table cellpadding="1" cellspacing="1" class="tableClass">
                                                <tr>
                                                    <td class="gridcellleft">Instrument Type :</td>
                                                    <td>
                                                        <asp:CheckBox ID="ChkInstrumentTypeCheque" runat="server" Checked="true" />
                                                        Cheque</td>
                                                    <td>
                                                        <asp:CheckBox ID="ChkInstrumentTypeETrf" runat="server" Checked="true" />
                                                        E-Trf</td>
                                                    <td>
                                                        <asp:CheckBox ID="ChkInstrumentTypeDraft" runat="server" Checked="true" />
                                                        Draft</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td class="gridcellleft">
                                            <table cellpadding="1" cellspacing="1" class="tableClass">
                                                <tr>
                                                    <td class="gridcellleft" width="150px">Type :</td>
                                                    <td>
                                                        <asp:CheckBox ID="ChkTypePayments" runat="server" Checked="true" />
                                                        Payments</td>
                                                    <td>
                                                        <asp:CheckBox ID="ChkTypeReceipts" runat="server" Checked="true" />
                                                        Receipts</td>

                                                </tr>
                                            </table>
                                        </td>
                                        <td class="gridcellleft">
                                            <table cellpadding="1" cellspacing="1" class="tableClass">
                                                <tr>
                                                    <td class="gridcellleft" >Status  :</td>
                                                    <td>
                                                        <asp:CheckBox ID="ChkStatusUncleared" runat="server" Checked="true" />
                                                        Uncleared</td>
                                                    <td>
                                                        <asp:CheckBox ID="ChkStatuscleared" runat="server" Checked="true" />
                                                        Cleared</td>

                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td class="gridcellleft">
                                            <table cellpadding="1" cellspacing="1" class="tableClass">
                                                <tr>
                                                    <td class="gridcellleft" width="150px">Sort Order :</td>
                                                    <td style="width: 255px">
                                                        <asp:DropDownList ID="DdlSortOrder" runat="server" Width="250px" Font-Size="12px">
                                                            <asp:ListItem Value="1">Account Name + Instrument Number</asp:ListItem>
                                                            <asp:ListItem Value="2">Instrument Number</asp:ListItem>
                                                            <asp:ListItem Value="3">Bank A/c + Transaction Date</asp:ListItem>
                                                            <asp:ListItem Value="4">Bank A/c + Instrument Number</asp:ListItem>
                                                            <asp:ListItem Value="5">Age (Uncleared)</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
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
                                            <dxe:ASPxButton ID="BtnShow" runat="server" AutoPostBack="False" Text="Show" CssClass="btn btn-primary">
                                                <ClientSideEvents Click="function (s, e) {BtnShow_Click();}" />
                                            </dxe:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="display: none;" colspan="2">
                                <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                                <asp:HiddenField ID="HiddenField_BRSAC" runat="server" />
                                <asp:HiddenField ID="HiddenField_Company" runat="server" />
                                <asp:HiddenField ID="HiddenField_Segment" runat="server" />
                                <asp:HiddenField ID="HiddenField_SubAC" runat="server" />
                                <asp:TextBox ID="txtMainAccount_hidden" runat="server" Width="5px"></asp:TextBox>
                                <asp:TextBox ID="HiddenField_txtMainAccountCode" runat="server" Width="5px"></asp:TextBox>
                                <asp:TextBox ID="HiddenField_txtSubLedgerType" runat="server" Width="5px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table id="Tab_Grid">
            <tr>
                <td>
                    <table style="width: 60%" border="1">
                        <tr>
                            <td valign="top" style="vertical-align: top; width: 34px; height: 11px; background-color: #b7ceec; text-align: left">Page</td>
                            <td valign="top" style="width: 4px">
                                <b style="text-align: right" id="B_PageNo" runat="server"></b>
                            </td>
                            <td valign="top" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left;">Of
                            </td>
                            <td valign="top">
                                <b style="text-align: right" id="B_TotalPage" runat="server"></b>
                            </td>
                            <td valign="top" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">( <b style="text-align: right" id="B_TotalRows" runat="server"></b>&nbsp;items )
                            </td>
                            <td valign="top">
                                <table width="100%">
                                    <tr>
                                        <td valign="top" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                            <a id="A_LeftNav" runat="server" href="javascript:void(0);" onclick="OnLeftNav_Click()">
                                                <img src="/assests/images/LeftNav.gif" width="10" />
                                            </a>
                                        </td>
                                        <td valign="top" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                            <a id="A1" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A1')">1</a>
                                        </td>
                                        <td valign="top" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                            <a id="A2" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A2')">2</a>
                                        </td>
                                        <td valign="top" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                            <a id="A3" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A3')">3</a>
                                        </td>
                                        <td valign="top" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                            <a id="A4" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A4')">4</a>
                                        </td>
                                        <td valign="top" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                            <a id="A5" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A5')">5</a>
                                        </td>
                                        <td valign="top" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                            <a id="A6" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A6')">6</a>
                                        </td>
                                        <td valign="top" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                            <a id="A7" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A7')">7</a>
                                        </td>
                                        <td valign="top" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                            <a id="A8" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A8')">8</a>
                                        </td>
                                        <td valign="top" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                            <a id="A9" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A9')">9</a>
                                        </td>
                                        <td valign="top" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                            <a id="A10" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A10')">10</a>
                                        </td>
                                        <td style="text-align: right; vertical-align: top; height: 11px; background-color: #b7ceec;"
                                            valign="top">
                                            <a id="A_RightNav" runat="server" href="javascript:void(0);" onclick="OnRightNav_Click()">
                                                <img src="../images/RightNav.gif" width="10" />
                                            </a>
                                        </td>
                                        <td align="right">
                                            <asp:DropDownList ID="cmbExport" runat="server" AutoPostBack="True" Font-Size="12px" Width="100px" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged">
                                                <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                                                <asp:ListItem Value="E">Excel</asp:ListItem>
                                                <asp:ListItem Value="P">PDF</asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxGridView ID="GridPaymentReceiptQuery" runat="server" AutoGenerateColumns="False"
                        ClientInstanceName="cGridPaymentReceiptQuery" Width="1200px" Font-Size="12px" KeyFieldName="Srl. No"
                        OnCustomCallback="GridPaymentReceiptQuery_CustomCallback">
                        <ClientSideEvents EndCallback="function(s, e) {GridPaymentReceiptQuery_EndCallBack();}" />
                        <Columns>
                            <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="Srl. No" Width="25px"
                                Caption="Srl.No">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="Branch ID" Width="150px"
                                Caption="Branch ID">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="Main A/c (Code)" Width="200px"
                                Caption="Main A/c (Code)">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="Sub A/c (Code)" Width="200px"
                                Caption="Sub A/c (Code)">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="Bank Name (Code)"
                                Width="200px" Caption="Bank Name (Code)">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="Segment" Width="45px"
                                Caption="Segment">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Instr.Type" Width="45px"
                                Caption="Instr Type">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="Instr.No." Width="45px"
                                Caption="Instr.No">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="Tran. Date" Width="50px"
                                Caption="Tran.Date">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="Voucher No." Width="50px"
                                Caption="Voucher No">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="Payment" Width="100px"
                                Caption="Payment">
                                <CellStyle HorizontalAlign="Right">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="Receipt" Width="100px"
                                Caption="Receipt">
                                <CellStyle HorizontalAlign="Right">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="11" FieldName="Value Date" Width="50px"
                                Caption="Value Date">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="12" FieldName="Client Bnk" Width="100px"
                                Caption="Client Bnk">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="13" FieldName="Third Party" Width="100px"
                                Caption="Third Party">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="14" FieldName="Age(Days)" Width="45px"
                                Caption="Age(Days)">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
                        <SettingsPager NumericButtonCount="30" ShowSeparators="True" Mode="ShowAllRecords"
                            PageSize="20">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <Settings ShowHorizontalScrollBar="True" ShowVerticalScrollBar="True" VerticalScrollableHeight="450" />
                        <Styles>
                            <FocusedRow BackColor="#FFC080" Font-Bold="False">
                            </FocusedRow>
                            <Header BackColor="ControlLight" Font-Bold="True" ForeColor="Black" HorizontalAlign="Center">
                            </Header>
                        </Styles>
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
