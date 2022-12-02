<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_payoutposition" CodeBehind="payoutposition.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>

    

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


     function Page_Load()///Call Into Page Load
     {
         Hide('Tab_showFilter');
         Hide('Td_Filter');
         FnddlGeneration('1');
         FnddlGroupBy(document.getElementById('ddlGroupBy').value);
         document.getElementById('hiddencount').value = 0;
         height();
     }
     function height() {
         if (document.body.scrollHeight >= 350) {
             window.frameElement.height = document.body.scrollHeight;
         }
         else {
             window.frameElement.height = '350px';
         }
         window.frameElement.width = document.body.scrollwidth;
     }
     function Hide(obj) {
         document.getElementById(obj).style.display = 'none';
     }
     function Show(obj) {
         document.getElementById(obj).style.display = 'inline';
     }


     function FOExists(obj) {
         if (obj == 'FO')
             Show('Td_FO');
         else
             Hide('Td_FO');

     }
     function DpExists(obj) {
         if (obj == 'DP')
             Show('Td_DP');
         else
             Hide('Td_DP');
     }

     function FnddlGroupBy(obj) {
         if (obj == 'Group') {
             Hide('Td_OtherGroupBy');
             Show('Td_Group');
             document.getElementById('BtnGroup').click();
         }
         else {
             Show('Td_OtherGroupBy');
             Hide('Td_Group');
         }
     }
     function FnOtherGroupBy(obj) {
         if (obj == "a")
             Hide('Tab_showFilter');
         else {
             if (document.getElementById('ddlGroupBy').value == 'Clients') {
                 var cmb = document.getElementById('cmbsearchOption');
                 cmb.value = 'Clients';
             }
             if (document.getElementById('ddlGroupBy').value == 'Branch') {
                 var cmb = document.getElementById('cmbsearchOption');
                 cmb.value = 'Branch';
             }
             if (document.getElementById('ddlGroupBy').value == 'BranchGroup') {
                 var cmb = document.getElementById('cmbsearchOption');
                 cmb.value = 'BranchGroup';
             }
             Show('Tab_showFilter');
         }

     }
     function FnGroup(obj) {
         if (obj == "a")
             Hide('Tab_showFilter');
         else {
             var cmb = document.getElementById('cmbsearchOption');
             cmb.value = 'Group';
             Show('Tab_showFilter');
         }

     }
     function FunCallAjaxList(objID, objEvent) {
         var strQuery_Table = '';
         var strQuery_FieldName = '';
         var strQuery_WhereClause = '';
         var strQuery_OrderBy = '';
         var strQuery_GroupBy = '';
         var CombinedQuery = '';

         if (document.getElementById('cmbsearchOption').value == "Segment") {
             strQuery_Table = "(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName +\'-'\ + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE Where  TMCE.EXCH_COMPID=\'<%=Session["LastCompany"]%>'\) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID)as TAB";
                strQuery_FieldName = "distinct top 10 EXCHANGENAME,SEGMENTID";
                strQuery_WhereClause = " EXCHANGENAME like (\'%RequestLetter%')";
            }
            else if (document.getElementById('cmbsearchOption').value == "Branch") {
                strQuery_Table = "tbl_master_branch";
                strQuery_FieldName = "top 10 branch_description+'-'+branch_code,branch_id";
                strQuery_WhereClause = " (branch_description Like (\'%RequestLetter%') or branch_code like (\'%RequestLetter%')) and branch_id in (<%=Session["userbranchHierarchy"]%>)";
            }
            else if (document.getElementById('cmbsearchOption').value == "BranchGroup") {
                strQuery_Table = "master_branchgroups";
                strQuery_FieldName = "top 10 rtrim(ltrim(branchgroups_name))+' [ '+rtrim(ltrim(branchgroups_code))+' ]',branchgroups_id";
                strQuery_WhereClause = " (branchgroups_name Like (\'%RequestLetter%') or branchgroups_code like (\'%RequestLetter%'))";
            }
            else if (document.getElementById('cmbsearchOption').value == "EMail") {
                strQuery_Table = "tbl_master_email INNER JOIN tbl_master_contact ON tbl_master_email.eml_cntId = tbl_master_contact.cnt_internalId";
                strQuery_FieldName = "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'') + '< '+ tbl_master_email.eml_email + '>' as name,cnt_internalId as contactid";
                strQuery_WhereClause = " (tbl_master_email.eml_email <> '') AND cnt_contacttype ='EM'  and (cnt_firstName Like (\'%RequestLetter%') or cnt_ucc like (\'%RequestLetter%') or tbl_master_email.eml_email like (\'%RequestLetter%') )";
            }
            else if (document.getElementById('cmbsearchOption').value == "Group") {
                strQuery_Table = "tbl_master_groupmaster";
                strQuery_FieldName = "top 10 gpm_description+'-'+gpm_code ,gpm_id";
                strQuery_WhereClause = " (gpm_description Like (\'%RequestLetter%') or gpm_code like (\'%RequestLetter%')) and gpm_type='" + document.getElementById('ddlGroup').value + "'";
            }
            else {
                strQuery_Table = "tbl_master_contact,tbl_master_contactexchange,tbl_master_branch";
                strQuery_FieldName = "distinct top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID";
                strQuery_WhereClause = "  branch_id=cnt_branchid and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' and (crg_tcode Like (\'%RequestLetter%') or CNT_FIRSTNAME like (\'%RequestLetter%')) and cnt_branchid in (<%=Session["userbranchHierarchy"]%>)";
           }
    CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
    ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars(CombinedQuery));
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


function fnSegment(obj) {
    if (obj == "a")
        Hide('Tab_showFilter');
    else if (obj == "c") {
        Hide('Tab_showFilter');
        Show('Td_Specific');
    }
    else {
        var cmb = document.getElementById('cmbsearchOption');
        cmb.value = 'Segment';
        Show('Tab_showFilter');
    }
    selecttion();
}
function btnAddsubscriptionlist_click() {

    var cmb = document.getElementById('cmbsearchOption');
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

    Hide('Tab_showFilter');
    document.getElementById('btnScreen').disabled = false;
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




function heightlight(obj) {

    var colorcode = obj.split('&');

    if ((document.getElementById('hiddencount').value) == 0) {
        prevobj = '';
        prevcolor = '';
        document.getElementById('hiddencount').value = 1;
    }
    document.getElementById(obj).style.backgroundColor = '#ffe1ac';

    if (prevobj != '') {
        document.getElementById(prevobj).style.backgroundColor = prevcolor;
    }
    prevobj = obj;
    prevcolor = colorcode[1];

}
function selecttion() {
    var combo = document.getElementById('cmbExport');
    combo.value = 'Ex';
}

function fnRecord(obj) {
    if (obj == "1") {
        Hide('Td_Filter');
        Show('tab1');
        Hide('displayAll');
        Hide('Div_Checking');
        alert('No Record Found!!');
    }
    if (obj == "2") {
        Show('td_filter');
        Hide('tab1');
        Show('displayAll');
        Hide('Div_Checking');

    }
    if (obj == "3") {
        Hide('Td_Filter');
        Show('tab1');
        Hide('displayAll');
        Hide('Div_Checking');

    }
    if (obj == "9") {
        Show('td_filter');
        Hide('tab1');
        Hide('displayAll');
        Show('Div_Checking');

    }
    if (obj == '4' || obj == '5' || obj == '6' || obj == '7') {
        Hide('Td_Filter');
        Show('tab1');
        Hide('displayAll');
        Hide('Div_Checking');
        if (obj == '4')
            alert("Mail Sent Successfully !!");
        if (obj == '5')
            alert("Error on sending!Try again.. !!");
        if (obj == '6')
            alert("'Mail Sent Successfully !!'+'\n'+'Emails not Sent For Clients Without Email-Id ...'");
        if (obj == '7')
            alert("E-Mail Id Could Not Be Found !!");
    }
    document.getElementById('hiddencount').value = 0;
    Hide('Tab_showFilter');
    height();
    selecttion();
}
function FnddlGeneration(obj) {

    if (obj == "1")////////Screen
    {
        Show('td_Screen');
        Hide('td_Export');
        Hide('td_Email');
        Hide('Tr_RptTypeOptionEmail');
    }
    if (obj == "2")////////Export
    {
        Hide('td_Screen');
        Show('td_Export');
        Hide('td_Email');
        Hide('Tr_RptTypeOptionEmail');
    }
    if (obj == "3")///////Email
    {
        Hide('td_Screen');
        Hide('td_Export');
        Show('td_Email');
        Show('Tr_RptTypeOptionEmail');
    }

}
function fnddloptionformail(obj) {
    if (obj == '2') {
        document.getElementById('cmbsearchOption').value = 'EMail';
        Show('Tab_showFilter');
    }
    else
        Hide('Tab_showFilter');
}


FieldName = 'lstSlection';
</script>



    <script type="text/ecmascript">
      function ReceiveServerData(rValue) {
    var j = rValue.split('~');
    if (j[0] == 'Branch')
        document.getElementById('HiddenField_Branch').value = j[1];
    if (j[0] == 'Segment')
        document.getElementById('HiddenField_Segment').value = j[1];
    if (j[0] == 'BranchGroup')
        document.getElementById('HiddenField_BranchGroup').value = j[1];
    if (j[0] == 'Group')
        document.getElementById('HiddenField_Group').value = j[1];
    if (j[0] == 'Clients')
        document.getElementById('HiddenField_Client').value = j[1];
    if (j[0] == 'EMail')
        document.getElementById('HiddenField_emmail').value = j[1];
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
                <td class="EHEADER" colspan="0" style="text-align: center; height: 20px;">
                    <strong><span id="SpanHeader" style="color: #000099">PayOut Position </span></strong></td>

                <td class="EHEADER" width="15%" id="Td_Filter" style="height: 22px">
                    <a href="javascript:void(0);" onclick="fnRecord(3);"><span style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight: bold">Filter</span></a>
                    <asp:DropDownList ID="cmbExport" runat="server" AutoPostBack="True" Font-Size="12px" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                        <asp:ListItem Value="E">Excel</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table id="tab1" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <table>
                        <tr>
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">For Date :
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="DtFor" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="108px" ClientInstanceName="DtFor">
                                                <DropDownButton Text="For">
                                                </DropDownButton>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Segment :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbSegmentAll" runat="server" GroupName="bb" onclick="fnSegment('a')" />All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbSegmentSpecific" runat="server" Checked="True" GroupName="bb"
                                                onclick="fnSegment('c')" />Specific
                                        </td>
                                        <td>[ <span id="litSegmentMain" runat="server" style="color: Maroon"></span>]</td>
                                        <td id="Td_SegmentSelected">
                                            <asp:RadioButton ID="rdSegmentSelected" runat="server" GroupName="bb" onclick="fnSegment('b')" />Selected
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Group By</td>
                                        <td>
                                            <asp:DropDownList ID="ddlGroupBy" runat="server" Width="120px" Font-Size="12px" onchange="FnddlGroupBy(this.value)">
                                                <asp:ListItem Value="Clients">Clients</asp:ListItem>
                                                <asp:ListItem Value="Branch">Branch</asp:ListItem>
                                                <asp:ListItem Value="Group">Group</asp:ListItem>
                                                <asp:ListItem Value="BranchGroup">Branch Group</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="gridcellleft" id="Td_OtherGroupBy">
                                            <table border="10" cellpadding="1" cellspacing="1">
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="RadioBtnOtherGroupByAll" runat="server" Checked="True" GroupName="a"
                                                            onclick="FnOtherGroupBy('a')" />
                                                        All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioBtnOtherGroupBySelected" runat="server" GroupName="a" onclick="FnOtherGroupBy('b')" />Selected
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td class="gridcellleft" id="Td_Group">
                                            <table border="10" cellpadding="1" cellspacing="1">
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlGroup" runat="server" Font-Size="12px">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="BtnGroup" EventName="Click"></asp:AsyncPostBackTrigger>
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioBtnGroupAll" runat="server" Checked="True" GroupName="b"
                                                            onclick="FnGroup('a')" />
                                                        All
                                                        <asp:RadioButton ID="RadioBtnGroupSelected" runat="server" GroupName="b" onclick="FnGroup('b')" />Selected
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Obligation Type :</td>
                                        <td>
                                            <asp:DropDownList ID="DdlObligationType" runat="server" Width="200px" Font-Size="12px">
                                                <asp:ListItem Value="Both">Both</asp:ListItem>
                                                <asp:ListItem Value="Payable  Only">Payable  Only</asp:ListItem>
                                                <asp:ListItem Value="Receivables Only">Receivables Only</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC" id="Td_FO">
                                            <asp:CheckBox ID="ChkFODr" runat="server" />
                                            Consider FO Segment Debit Balance
                                        </td>
                                        <td class="gridcellleft" bgcolor="#B7CEEC" id="Td_DP">
                                            <asp:CheckBox ID="ChkDPDr" runat="server" />
                                            Consider DP Segment Debit Balance
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:CheckBox ID="ChkAppMrgn" runat="server" />
                                            Consider Applicable Margin
                                        </td>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:CheckBox ID="ChkCashMrgn" runat="server" />
                                            Show Cash Margin Balance
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Consider Clients With Payable Amount >=
                                        </td>
                                        <td>
                                            <dxe:ASPxTextBox ID="txtAmntGreaterThan" runat="server" HorizontalAlign="Right" Width="100px">
                                                <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                <ValidationSettings ErrorDisplayMode="None"></ValidationSettings>
                                            </dxe:ASPxTextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Generate Type :</td>
                                        <td>
                                            <asp:DropDownList ID="ddlGeneration" runat="server" Width="100px" Font-Size="12px"
                                                onchange="FnddlGeneration(this.value)">
                                                <asp:ListItem Value="1">Screen</asp:ListItem>
                                                <asp:ListItem Value="2">Export</asp:ListItem>
                                                <asp:ListItem Value="3">EMail</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="Tr_RptTypeOptionEmail">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Respective :</td>
                                        <td>
                                            <asp:DropDownList ID="ddloptionformail" runat="server" Width="100px" Font-Size="12px"
                                                onchange="fnddloptionformail(this.value)">
                                                <asp:ListItem Value="1">Group/Branch</asp:ListItem>
                                                <asp:ListItem Value="2">User</asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table>
                                    <tr>
                                        <td id="td_Screen">
                                            <asp:Button ID="btnScreen" runat="server" CssClass="btnUpdate" Height="20px" Text="Screen"
                                                Width="101px" OnClientClick="selecttion()" OnClick="btnScreen_Click" />
                                        </td>
                                        <td id="td_Export">
                                            <asp:Button ID="btnExcel" runat="server" CssClass="btnUpdate" Height="20px" Text="Export To Excel"
                                                Width="101px" OnClientClick="selecttion()" OnClick="btnExcel_Click" /></td>
                                        <td id="td_Email">
                                            <asp:Button ID="btnEmail" runat="server" CssClass="btnUpdate" Height="20px" Text="Send eMail"
                                                Width="101px" OnClientClick="selecttion()" OnClick="btnEmail_Click" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <table id="Tab_showFilter" border="10" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="300px" onkeyup="FunCallAjaxList(this,event)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                Enabled="false">
                                                <asp:ListItem>Clients</asp:ListItem>
                                                <asp:ListItem>Group</asp:ListItem>
                                                <asp:ListItem>Branch</asp:ListItem>
                                                <asp:ListItem>BranchGroup</asp:ListItem>
                                                <asp:ListItem>Segment</asp:ListItem>
                                                <asp:ListItem>EMail</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <a id="A3" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                                style="color: #2554C7; text-decoration: underline; font-size: 8pt;"><b>Add to List</b></span></a><span
                                                    style="color: #009900; font-size: 8pt;"> </span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ListBox ID="lstSlection" runat="server" Font-Size="12px" Height="100px" Width="400px"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <a id="A5" href="javascript:void(0);" onclick="clientselectionfinal()"><span style="color: #000099; text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <a id="A6" href="javascript:void(0);" onclick="btnRemovefromsubscriptionlist_click()">
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
        <table>
            <tr>
                <td style="display: none;">
                    <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                    <asp:Button ID="BtnGroup" runat="server" Text="BtnGroup" OnClick="BtnGroup_Click" />
                    <asp:HiddenField ID="HiddenField_Group" runat="server" />
                    <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                    <asp:HiddenField ID="HiddenField_emmail" runat="server" />
                    <asp:HiddenField ID="HiddenField_BranchGroup" runat="server" />
                    <asp:HiddenField ID="HiddenField_Client" runat="server" />
                    <asp:HiddenField ID="HiddenField_Segment" runat="server" />
                    <asp:DropDownList ID="cmbgrp" runat="server" Font-Size="12px" Width="300px" AutoPostBack="True">
                    </asp:DropDownList>
                    <asp:HiddenField ID="hiddencount" runat="server" />
                </td>
                <td>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                        <ProgressTemplate>
                            <div id='Div1' style='position: absolute; font-family: arial; font-size: 30; left: 50%; top: 50%; background-color: white; layer-background-color: white; height: 80; width: 150;'>
                                <table width='100' height='35' border='1' cellpadding='0' cellspacing='0' bordercolor='#C0D6E4'>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td height='25' align='center' bgcolor='#FFFFFF'>
                                                        <img src='/assests/images/progress.gif' width='18' height='18'></td>
                                                    <td height='10' width='100%' align='center' bgcolor='#FFFFFF'>
                                                        <font size='1' face='Tahoma'><strong align='center'>Please Wait..</strong></font></td>
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
        </table>
        <div id="displayAll" style="display: none;" width="100%">
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <table width="100%" border="1">
                        <tr>
                            <td>
                                <div id="DivHeader" runat="server">
                                </div>
                            </td>
                        </tr>
                        <tr bordercolor="Blue" id="tr_prvnxt">
                            <td align="left">
                                <table id="tblpage" cellspacing="0" cellpadding="0" runat="server" width="10%">
                                    <tr>
                                        <td width="20" style="padding: 5px">
                                            <asp:LinkButton ID="ASPxFirst" runat="server" Font-Bold="True" ForeColor="maroon"
                                                OnClientClick="javascript:selecttion();;" OnClick="ASPxFirst_Click">First</asp:LinkButton></td>
                                        <td width="25">
                                            <asp:LinkButton ID="ASPxPrevious" runat="server" Font-Bold="True" ForeColor="Blue"
                                                OnClientClick="javascript:selecttion();" OnClick="ASPxPrevious_Click">Previous</asp:LinkButton></td>
                                        <td width="20" style="padding: 5px">
                                            <asp:LinkButton ID="ASPxNext" runat="server" Font-Bold="True" ForeColor="maroon"
                                                OnClientClick="javascript:selecttion();" OnClick="ASPxNext_Click">Next</asp:LinkButton></td>
                                        <td width="20">
                                            <asp:LinkButton ID="ASPxLast" runat="server" Font-Bold="True" ForeColor="Blue" OnClientClick="javascript:selecttion();"
                                                OnClick="ASPxLast_Click">Last</asp:LinkButton></td>
                                        <td align="right" style="display: none;"></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="Divdisplay" runat="server">
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnScreen" EventName="Click"></asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div id="Div_Checking" style="display: none;" width="100%">
            <asp:UpdatePanel runat="server" ID="UpdatePanel7">
                <ContentTemplate>
                    <table width="50%" border="1">

                        <tr>
                            <td>
                                <div id="Div_Activity" runat="server">
                                </div>
                            </td>
                        </tr>
                        <br />

                        <tr>
                            <td>
                                <div id="Div_bill" runat="server">
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnScreen" EventName="Click"></asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
