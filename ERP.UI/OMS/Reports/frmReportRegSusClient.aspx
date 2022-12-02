<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_frmReportRegSusClient" Codebehind="frmReportRegSusClient.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

       
     <style type="text/css">
	
	/* Big box with list of options */
	#ajax_listOfOptions{
		position:absolute;	/* Never change this one */
		width:50px;	/* Width of box */
		height:auto;	/* Height of box */
		overflow:auto;	/* Scrolling features */
		border:1px solid Blue;	/* Blue border */
		background-color:#FFF;	/* White background color */
		text-align:left;
		font-size:0.9em;
		z-index:32767;
	}
	#ajax_listOfOptions div{	/* General rule for both .optionDiv and .optionDivSelected */
		margin:1px;		
		padding:1px;
		cursor:pointer;
		font-size:0.9em;
	}
	#ajax_listOfOptions .optionDiv{	/* Div for each item in list */
		
	}
	#ajax_listOfOptions .optionDivSelected{ /* Selected item in the list */
		background-color:#DDECFE;
		color:Blue;
	}
	#ajax_listOfOptions_iframe{
		background-color:#F00;
		position:absolute;
		z-index:3000;
	}
	
	form{
		display:inline;
	}
	
	</style>
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>
    <script language="javascript" type="text/javascript">
        FieldName = null;
        function Page_Load() {

            document.getElementById('showFilter').style.display = 'none';
            document.getElementById('btnfilter').style.display = 'none';

        }
        function height() {
            if (document.body.scrollHeight >= 350) {
                window.frameElement.height = document.body.scrollHeight + 20;
            }
            else {
                window.frameElement.height = '350px';
            }
            window.frameElement.width = document.body.scrollwidth;
            document.getElementById('hidScreenWd').value = screen.width - 20;
        }
        function GetCompanies(obj1, obj2, obj3) {
            var strQuery_Table = "Tbl_Master_Company";
            var strQuery_FieldName = "top 10 (isnull(cmp_name,\'\')+ \'-[\'+ isnull(cmp_internalid,\'\') + \']\') as Company,cmp_internalid";
            var strQuery_WhereClause = " (cmp_name Like (\'%RequestLetter%\') or cmp_internalid Like (\'%RequestLetter%\'))";
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery));

        }
        function GetClients(obj1, obj2, obj3) {
            if (document.getElementById('cmbsearchOption').value == 'Clients') {
                var strQuery_Table = "tbl_Master_Contact";
                var strQuery_FieldName = "top 10 (rtrim(ltrim(isnull(cnt_Firstname,\'\'))) + ' ' + rtrim(ltrim(isnull(cnt_Middlename,\'\'))) + ' ' + rtrim(ltrim(isnull(cnt_Lastname,\'\'))) + \'-[\'+ isnull(cnt_internalid,\'\') + \']\') as Client,cnt_internalId";
                var strQuery_WhereClause = " (cnt_Firstname Like (\'%RequestLetter%\') or cnt_Middlename Like (\'%RequestLetter%\') or cnt_Lastname Like (\'%RequestLetter%\'))";
                var strQuery_OrderBy = '';
                var strQuery_GroupBy = '';
                var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
                ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery));
            }
            else if (document.getElementById('cmbsearchOption').value == 'Branch') {
                var strQuery_Table = "tbl_master_branch";
                var strQuery_FieldName = "top 10 (isnull(branch_description,\'\')+ \'-[\'+ isnull(branch_code,\'\') + \']\') as Branch,branch_id";
                var strQuery_WhereClause = " (branch_description Like (\'%RequestLetter%\') or branch_code Like (\'%RequestLetter%\')) ";
                var strQuery_OrderBy = '';
                var strQuery_GroupBy = '';
                var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
                ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery));

            }
            else if (document.getElementById('cmbsearchOption').value == 'Group') {
                var grouptype = document.getElementById('ddlgrouptype').value;
                var strQuery_Table = "tbl_master_groupmaster";
                var strQuery_FieldName = " top 10 rtrim(ltrim(isnull(gpm_description,'')))+ ' ' + rtrim(ltrim(isnull(gpm_code,''))) as Groupdesc,rtrim(ltrim(cast(gpm_id as varchar))) as Groupid";
                var strQuery_WhereClause = " (gpm_description like (\'%RequestLetter%\') or gpm_code like (\'%RequestLetter%\')) and gpm_type='" + grouptype + "'";
                var strQuery_OrderBy = '';
                var strQuery_GroupBy = '';
                var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);

                ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');

            }
            else if (document.getElementById('cmbsearchOption').value == 'Segment') {
                // var grouptype=document.getElementById('ddlgrouptype').value;
                //var compval=document.getElementById('HiddenField_Company').value
                var compval = document.getElementById('txtCompany').value;
                compval = compval.split('[');
                compval = compval[1];

                compval = compval.substring(0, compval.length - 1);

                var strQuery_Table = " tbl_Master_Exchange,tbl_Master_CompanyExchange";
                var strQuery_FieldName = "top 10 Ltrim(Rtrim(Exh_ShortName))+' - '+ Exch_SegmentID as segment,Ltrim(Rtrim(Exh_ShortName))+' - '+ Exch_SegmentID as segment2";
                var strQuery_WhereClause = " Exh_CntID=Exch_ExchID and (Exh_ShortName like (\'%RequestLetter%\') or Exch_SegmentID like (\'%RequestLetter%\'))  and Exch_CompID='" + compval + "' and Exch_SegmentID is not null";
                var strQuery_OrderBy = '';
                var strQuery_GroupBy = '';
                var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);

                ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');


            }
            else if (document.getElementById('cmbsearchOption').value == 'ClientCategory') {
                //var compval=document.getElementById('HiddenField_Company').value

                var strQuery_Table = " tbl_master_legalStatus";
                var strQuery_FieldName = "top 10 Ltrim(Rtrim(lgl_legalStatus)) lgl_legalStatus,lgl_id as lgl_id";
                var strQuery_WhereClause = "  lgl_legalStatus like (\'%RequestLetter%\') ";
                var strQuery_OrderBy = '';
                var strQuery_GroupBy = '';
                var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);

                ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');

            }
            else if (document.getElementById('cmbsearchOption').value == 'BranchGroup') {
                //var userbranchhierarchy=document.getElementById('hiddenUserbranchierarchy').value;
                var strQuery_Table = "master_branchgroups";
                var strQuery_FieldName = " top 10 rtrim(ltrim(isnull(BranchGroups_Name,'')))+ ' [' + rtrim(ltrim(isnull(BranchGroups_Code,''))) +']' as BranchGroupDesc,rtrim(ltrim(cast(BranchGroups_ID as varchar))) as BranchGroupid";
                var strQuery_WhereClause = " (BranchGroups_Name like (\'%RequestLetter%\') or BranchGroups_Code like (\'%RequestLetter%\')) ";
                var strQuery_OrderBy = '';
                var strQuery_GroupBy = '';
                var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);

                ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');


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
        function Clients(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'Clients';
                Show('showFilter');
            }
            selection();
            height();
        }

        function Types(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                var cmb = document.getElementById('cmbsearchOption');
                var cmbtype = document.getElementById('ddlGroup').value;
                if (cmbtype == '0')
                    cmb.value = 'Branch';
                else
                    cmb.value = 'Group';
                Show('showFilter');
            }
            selection();
            height();
        }
        function Branch(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                if (document.getElementById('ddlGroup').value == "0") {
                    document.getElementById('cmbsearchOption').value = 'Branch';
                }
                if (document.getElementById('ddlGroup').value == "2") {
                    document.getElementById('cmbsearchOption').value = 'BranchGroup';
                }
                Show('showFilter');
                Show('divselectbranches');
                Hide('divselectclient');
            }
            selecttion();
            height();
        }
        function Group(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'Group';
                Show('showFilter');
                //                 Show('divselectbranches');
                //                 Hide('divselectclient');
            }
            selecttion();
            height();
        }
        function Segment(obj) {

            if (obj == "a")
                Hide('showFilter');
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'Segment';
                Show('showFilter');
            }
            selection();
            height();
        }
        function ClientCat(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'ClientCategory';
                Show('showFilter');
            }
            selection();
            height();
        }
        function selection() {

            var objddlexp = document.getElementById('ddlExport');
            objddlexp.value = 'Ex';

        }
        function Hide(obj) {
            document.getElementById(obj).style.display = 'none';
        }
        function Show(obj) {
            document.getElementById(obj).style.display = 'inline';
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

            Hide('showFilter');
            document.getElementById('btnshow').disabled = false;
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
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function fnddlGroup(obj) {

            if (obj == "0" || obj == "2") {
                Hide('tabGroup');
                Show('tabBranch');
                Hide('showfilter');
            }
            else {
                Show('tabGroup');
                Hide('tabBranch');
                Hide('showfilter');
                //                var btn = document.getElementById('btnhide');
                //                btn.click();
            }
            selecttion();
            height();
        }
        function fngrouptype(obj) {
            if (obj == "0") {
                Hide('td_allselect');
                alert('Please Select Group Type !');
            }
            else {
                Show('td_allselect');
            }
            selecttion();
            height();
        }
        function gridrowy() {
            document.getElementById('btnfilter').style.display = 'inline';
            Hide('showFilter');
            Hide('tabMainFilter');
        }
        function gridrown() {
            document.getElementById('btnfilter').style.display = 'none';
            Hide('showFilter');
        }
        function ShowFilters() {
            window.location = '../reports/frmReportRegSusClient.aspx';
            //             IFRAME_ForAllPages.location='../reports/frmReportRegSusClient.aspx';

            ////               selection();
            ////               Hide('btnfilter');
            ////               Show('tabMainFilter');
            ////               height(); 
        }
        function reset() {
            alert('No Record Found !');
            window.location = '../reports/frmReportRegSusClient.aspx';

        }
        function keyVal(obj) {
            alert(obj);

        }
    </script>
    
    <script type="text/ecmascript">
        function ReceiveServerData(rValue) {

            var j = rValue.split('~');
            var btn = document.getElementById('btnhide');


            if (j[0] == 'Clients') {
                document.getElementById('HiddenField_Client').value = j[1];
            }
            else if (j[0] == 'Segment') {
                var combo = document.getElementById('litSegment');
                var NoItems = j[1].split(',');
                var i;
                var val = '';
                var seg = '';
                for (i = 0; i < NoItems.length; i++) {
                    var items = NoItems[i].split(';');
                    if (val == '') {
                        seg = items[0];
                        val = items[1];

                    }
                    else {
                        seg += ',' + items[0];
                        val += ',' + items[1];

                    }
                }
                document.getElementById('HiddenField_Segment').value = seg;
                combo.innerText = val;
                document.getElementById('HiddenField_SegmentName').value = val;
                //document.getElementById('HiddenField_Segment').value = j[1];
            }
            else if (j[0] == 'ScripsExchange') {
                document.getElementById('HiddenField_Instrument').value = j[1];
            }
            else if (j[0] == 'SettlementNo') {
                document.getElementById('HiddenField_SettNo').value = j[1];
            }
            else if (j[0] == 'SettlementType') {
                document.getElementById('HiddenField_Setttype').value = j[1];
            }
            else if (j[0] == 'Branch') {
                document.getElementById('HiddenField_Branch').value = j[1];
                groupvalue = j[1];
                btn.click();
            }
            else if (j[0] == 'Group') {
                document.getElementById('HiddenField_Group').value = j[1];
                groupvalue = j[1];
                btn.click();
            }
            else if (j[0] == 'ClientCategory') {
                document.getElementById('HiddenField_ClientCategory').value = j[1];
            }
            else if (j[0] == 'BranchGroup') {

                document.getElementById('HiddenField_BranchGroup').value = j[1];

            }
        }

       </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
   <%-- frm_Report_ContactDetailsNew--%>
   <asp:ScriptManager ID="ScriptManager1" runat="Server"></asp:ScriptManager>
        <table class="TableMain100">
                <tr>
                    <td class="EHEADER" colspan="0" style="text-align: center;">
                        <strong><span style="color: #000099">Registered/Suspended Clients</span></strong>
                    </td>
                    
                </tr>
            </table>
            
    <table width="100%">
        <tr>
          <td class="gridcellleft">
                  
            <table id="tabMainFilter" cellspacing="1" cellpadding="2" style="background-color: #B7CEEC; border: solid 1px  #ffffff; width:500px"
                border="1">
                <tr>
                    <td style="height:28px; width:125px" class="gridcellleft">
                        Company :
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtCompany" Width="220px" onkeyup="GetCompanies(this,'GenericAjaxList',this)" runat="server"></asp:TextBox>
                        <asp:HiddenField ID="txtCompany_hidden" runat="Server" />
                    </td>
                 </tr>
                 <tr>
                    <td style="height:28px" class="gridcellleft">
                        Segment :
                    </td>
                    <td colspan="2">
                        <table border="0" >
                            <tr>
                                                
                            <td style="width:50px">
                                    <asp:RadioButton ID="rdbSegAll" runat="server" Checked="True" GroupName="s" onclick="Segment('a')" />
                                            All
                            </td>        
                            <td style="width:70px">
                                    <asp:RadioButton ID="rdbSegSelected" runat="server"  GroupName="s"
                                                onclick="Segment('b')" />
                                            Selected
                            
                            </td>
                            <td>
                                [<span id="litSegment" runat="server" style="color: Maroon"></span>]
                            </td>
                             </tr>
                        </table>
                    </td>
                 </tr>
                 <tr>
                    <td style="height:28px" class="gridcellleft">
                        Date :
                    </td>
                    
                    <td>
                        <dxe:ASPxDateEdit ID="dtFrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                        Font-Size="12px" Width="108px" EditFormatString="dd-MM-yyyy" ClientInstanceName="dtFrom">
                                        <DropDownButton Text="From">
                                        </DropDownButton>
                                    </dxe:ASPxDateEdit>
                    </td>
                   
                    <td>
                         <dxe:ASPxDateEdit ID="dtTo" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                        Font-Size="12px" Width="108px" EditFormatString="dd-MM-yyyy" ClientInstanceName="dtTo">
                                        <DropDownButton Text="To">
                                        </DropDownButton>
                                    </dxe:ASPxDateEdit>
                    </td>
                 </tr>
                 <tr>
                    <td style="height:28px" class="gridcellleft">
                        Type :
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="drpType" Width="100px" Font-Size="12px"  runat="Server">
                            <asp:ListItem Text="Registered" Value="r"></asp:ListItem>
                            <asp:ListItem Text="Suspended" Value="s"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                 </tr>
                 <tr>
                    <td style="height:28px" class="gridcellleft">
                        Group By :
                    </td>
                    <td colspan="2">
                    <table border="0">
                        <tr>
                        
                    <td>
                         <asp:DropDownList ID="ddlGroup" runat="server" Width="100px" Font-Size="12px" onchange="fnddlGroup(this.value)">
                                <asp:ListItem Value="0">Branch</asp:ListItem>
                                <asp:ListItem Value="1">Group</asp:ListItem>
                                <asp:ListItem Value="2">BranchGroup</asp:ListItem>
                              
                            </asp:DropDownList>
                    </td>
                    <td>
                        <table id="tabBranch">
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rdbranchAll" runat="server" Checked="True" GroupName="a" onclick="Branch('a')" />
                                    All
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdbranchSelected" runat="server" GroupName="a" onclick="Branch('b')" />Selected
                                </td>
                            </tr>
                        </table>
                         <table id="tabGroup" style="display:none" >
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="updatePanelGroup" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                    <asp:DropDownList ID="ddlgrouptype" runat="server" Font-Size="12px" onchange="fngrouptype(this.value)">
                                    </asp:DropDownList>
                                    </ContentTemplate>
                                   
                                    </asp:UpdatePanel>
                                        
                                </td>
                                <td id="td_allselect" style="display: none;">
                                    <asp:RadioButton ID="rdddlgrouptypeAll" runat="server" Checked="True" GroupName="b"
                                        onclick="Group('a')" />
                                    All
                                    <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="b" onclick="Group('b')" />Selected
                                </td>
                            </tr>
                        </table>
                                    
                    </td>
                    </tr>
                    </table>
                    </td>
                    
                 </tr>
                 <tr>
                    <td style="height:28px" class="gridcellleft">
                        Client Category :
                    </td>
                    
                    <td colspan="2">
                        <table border="0" width="160px">
                            <tr>
                            
                             <td>
                                    <asp:RadioButton ID="rbClientCategoryAll" runat="server" Checked="True" GroupName="cc" onclick="ClientCat('a')" />
                                            All
                            </td>        
                            <td>
                                    <asp:RadioButton ID="rbClientCategorySelected" runat="server"  GroupName="cc"
                                                onclick="ClientCat('b')" />
                                            Selected
                            
                            </td>
                        </tr>
                        </table>
                    </td>
                    
                 </tr>
                 <tr>
                    <td style="height:28px" class="gridcellleft">
                        Clients :
                    </td>
                    
                    <td colspan="2">
                    <table border="0" width="160px">
                        <tr>
                        
                         <td>
                                <asp:RadioButton ID="rbClientsAll" runat="server" Checked="True" GroupName="c" onclick="Clients('a')" />
                                        All
                        </td>        
                        <td>
                                <asp:RadioButton ID="rbClientSelected" runat="server"  GroupName="c"
                                            onclick="Clients('b')" />
                                        Selected
                        
                        </td>
                        </tr>
                    </table>
                    </td>
                 </tr>
                 <tr>
                    <td>
                        <asp:Button ID="btnshow" runat="server" CssClass="btnUpdate" Height="20px" Text="Show"
                                        Width="101px" OnClick="btnshow_Click" />
                    </td>
                     <td colspan="2"></td>
                 </tr>
                             
            </table>
                
        </td>
        <td valign="top">
             <table width="100%" id="showFilter">
                            <tr valign="top">
                                <td style="text-align:left; vertical-align: top; height: 134px;">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="gridcellleft" style="vertical-align: top; text-align: right; height: 21px;"
                                                id="TdFilter">
                                                <span id="spanunder"></span><span id="spanclient"></span>
                                                <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="155px" onkeyup="GetClients(this,'GenericAjaxList',this)"></asp:TextBox><asp:DropDownList
                                                    ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px" Enabled="false">
                                                    <asp:ListItem>Clients</asp:ListItem>
                                                    <asp:ListItem>Group</asp:ListItem>
                                                    <asp:ListItem>Branch</asp:ListItem>
                                                    <asp:ListItem>Segment</asp:ListItem>
                                                    <asp:ListItem>ClientCategory</asp:ListItem>
                                                     <asp:ListItem>BranchGroup</asp:ListItem>
                                                    
                                                    
                                                </asp:DropDownList>
                                                <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                                    style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                                        style="color: #009900; font-size: 8pt;"> </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:ListBox ID="lstSlection" runat="server" Font-Size="12px" Height="90px" Width="290px">
                                                </asp:ListBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <a id="A2" href="javascript:void(0);" onclick="clientselectionfinal()"><span style="color: #000099;
                                                                text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            <a id="A1" href="javascript:void(0);" onclick="btnRemovefromsubscriptionlist_click()">
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
            <td >
                <table>
                    <tr id="tr_filter" runat="server">
                        <td id="Td1">
                            <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">
                                Show Filter</span></a>
                        </td>
                        <td id="Td2">
                            <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">
                                All Records</span></a>
                        </td>
                    </tr>
                </table>
            </td>
           <td align="right" id="btnfilter" >
                        <asp:DropDownList ID="ddlExport" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged1">
                            <asp:ListItem Value="Ex" Selected="True">Export</asp:ListItem>
                            <asp:ListItem Value="E">Excel</asp:ListItem>
                            <asp:ListItem Value="P">PDF</asp:ListItem>
                           
                        </asp:DropDownList>||
                        <input id="Button1" type="button" value="Show Filter" class="btnUpdate" onclick="javascript: ShowFilters();"
                            style="width: 66px; height: 19px" />
                        
                                              
          </td>
           <%--<td class="gridcellright" align="right" id="td_export" runat="server" style="display:none">
                                                    <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" BackColor="Gray"
                                                        Font-Bold="False" ForeColor="White" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                                        ValueType="System.Int32" Width="80px">
                                                        <items>
                                                            <dxe:ListEditItem Text="Select" Value="0" />
                                                         <dxe:ListEditItem Text="PDF" Value="1" />
                                                            <dxe:ListEditItem Text="XLS" Value="2" />
                                                           
                                                            <dxe:ListEditItem Text="CSV" Value="4" />
                                                        </items>
                                                        <buttonstyle backcolor="#C0C0FF" forecolor="Black">
                                                        </buttonstyle>
                                                        <itemstyle backcolor="Navy" forecolor="White">
                                                            <HoverStyle BackColor="#8080FF" ForeColor="White">
                                                            </HoverStyle>
                                                        </itemstyle>
                                                        <border bordercolor="White" />
                                                        <dropdownbutton text="Export">
                                                        </dropdownbutton>
                                                    </dxe:ASPxComboBox>
                                                </td>--%>
       </tr>
       <tr>
             <td colspan="2" style="text-align: left; width: 990px; vertical-align: top;">
                   <%--<div id="divgrid" style="width:985px; overflow:scroll" >  --%>
                <dxe:ASPxGridView ID="gridClients" KeyFieldName="clientid"  Width="990px" runat="Server" ClientInstanceName="grid" OnCustomCallback="gridClients_CustomCallback">
                            <Columns>
                                <dxe:GridViewDataTextColumn  Caption="Srl No" VisibleIndex="1">
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <DataItemTemplate>
                                <%# Container.ItemIndex+1%>
                                </DataItemTemplate>
                                </dxe:GridViewDataTextColumn>
                                
                                <dxe:GridViewDataTextColumn  Caption="ClientName" FieldName="clientname" VisibleIndex="2">
                                </dxe:GridViewDataTextColumn>
                               
                                <dxe:GridViewDataTextColumn  Caption="UCC" FieldName="ucc" VisibleIndex="3">
                                </dxe:GridViewDataTextColumn>
                                  <dxe:GridViewDataTextColumn  Caption="Branch Name" FieldName="branchname" VisibleIndex="4">
                                </dxe:GridViewDataTextColumn>
                                  <dxe:GridViewDataTextColumn  Caption="Group Name" FieldName="groupname" VisibleIndex="5">
                                </dxe:GridViewDataTextColumn>
                                  <dxe:GridViewDataTextColumn  Caption="Phone" FieldName="phones" VisibleIndex="6">
                                </dxe:GridViewDataTextColumn>
                                  <dxe:GridViewDataTextColumn  Caption="Pan" FieldName="pancard" VisibleIndex="7">
                                </dxe:GridViewDataTextColumn>
                                  <dxe:GridViewDataTextColumn  Caption="Bank" FieldName="Bankdetails" VisibleIndex="8">
                                </dxe:GridViewDataTextColumn>
                                  <dxe:GridViewDataTextColumn  Caption="DPAccount" FieldName="DpAccount" VisibleIndex="9">
                                </dxe:GridViewDataTextColumn>
                                  <dxe:GridViewDataTextColumn  Caption="Email" FieldName="email" VisibleIndex="10">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Width="100px" Caption="Address" FieldName="address" VisibleIndex="11">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn  Caption="ContractDeliveryMode" FieldName="contractdeliverymode" VisibleIndex="12">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn  Caption="NSE-CM" FieldName="nsereg" VisibleIndex="13">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn  Caption="NSE-FO" FieldName="nforeg" VisibleIndex="14">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="NSE-CDX" FieldName="cdxreg" VisibleIndex="15">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="BSE-CM" FieldName="bsereg" VisibleIndex="16">
                                </dxe:GridViewDataTextColumn>
                                 <dxe:GridViewDataTextColumn Caption="BSE-FO" FieldName="bseforeg" VisibleIndex="17">
                                </dxe:GridViewDataTextColumn>
                                 <dxe:GridViewDataTextColumn Caption="MCXSX-CDX" FieldName="mcxcdxreg" VisibleIndex="18">
                                </dxe:GridViewDataTextColumn>
                                 <dxe:GridViewDataTextColumn Caption="MCX-COMM" FieldName="mcxcommreg" VisibleIndex="19">
                                </dxe:GridViewDataTextColumn>
                                 <dxe:GridViewDataTextColumn Caption="NCDEX-COMM" FieldName="ncdexcommreg" VisibleIndex="20">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="NMCE-COMM" FieldName="nmcereg" VisibleIndex="21">
                                </dxe:GridViewDataTextColumn>
                                 <dxe:GridViewDataTextColumn Caption="USE-CDX" FieldName="usecdxreg" VisibleIndex="22">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="ACCOUNTS" FieldName="accountsreg" VisibleIndex="23">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="NSEL-SPOT" FieldName="nselspotreg" VisibleIndex="24">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="MCXSX-CM" FieldName="mcxcmreg" VisibleIndex="25">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="MCXSX-FO" FieldName="mcxforeg" VisibleIndex="26">
                                </dxe:GridViewDataTextColumn>
                               
                            </Columns>
                            
                             <Styles>
                                                        <LoadingPanel ImageSpacing="10px">
                                                        </LoadingPanel>
                                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                        </Header>
                                                    </Styles>
                                                    <settingspager numericbuttoncount="20" pagesize="5" showseparators="True" alwaysshowpager="True">
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </settingspager>
                                                    <Settings ShowHorizontalScrollBar="true" />
                                                    <%--<SettingsPager AlwaysShowPager="True"  ShowSeparators="True">
                                                        <FirstPageButton Visible="True">
                                                        </FirstPageButton>
                                                        <LastPageButton Visible="True">
                                                        </LastPageButton>
                                                    </SettingsPager>--%>
                                                    <SettingsBehavior AllowFocusedRow="True" AllowSort="False" />
                           
                       </dxe:ASPxGridView>
                    
                      
           </td>
       </tr>
    </table>
    
     <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                                    </dxe:ASPxGridViewExporter>
    <div style="display:none">
     <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>           
            <asp:HiddenField ID="HiddenField_Group" runat="server" />
            <asp:HiddenField ID="HiddenField_Branch" runat="server" />
            <asp:HiddenField ID="HiddenField_Client" runat="server" />
            <asp:HiddenField ID="HiddenField_Segment" runat="server" />
            <asp:HiddenField ID="txtSignature_hidden" runat="server" />
            <asp:HiddenField ID="txtHeader_hidden" runat="server" />
            <asp:HiddenField ID="HiddenField_SegmentName" runat="server" /> 
            <asp:HiddenField ID="HiddenField_BranchGroup" runat="server" />
            <asp:HiddenField ID="HiddenField_Company" runat="server" />
            <asp:HiddenField ID="HiddenField_ClientCategory" runat="Server" />
            <asp:HiddenField ID="hidScreenWd" runat="server" />   
            </div>
    </div>
</asp:Content>
