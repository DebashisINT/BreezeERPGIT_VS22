<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_DematPosition" Codebehind="frmReport_DematPosition.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>

    

    <style type="text/css">
	 .tableClass {
    /* border: 0px; */
    border: 1px solid #aaa !important;
    border-collapse: collapse !important;
}
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
	.grid_scroll
    {
        overflow-y: scroll;  
        overflow-x: scroll; 
        width:80%;
        scrollbar-base-color: #C0C0C0;
    
    }

	</style>

    <script language="javascript" type="text/javascript">


        function Page_Load()///Call Into Page Load
        {
            document.getElementById('DdlSettlements').value = 'For';
            FnSearchBy('SettNo');
            Hide('TdFilter');
            document.getElementById('hiddencount').value = 0;
            document.getElementById('Divdisplay').innerHTML = "";
            FnGenerateType('Screen');
            height();
        }
        function height() {
            if (document.body.scrollHeight >= 480) {
                window.frameElement.height = document.body.scrollHeight;
            }
            else {
                window.frameElement.height = '480px';
            }
            window.frameElement.width = document.body.scrollwidth;
        }
        function Hide(obj) {
            document.getElementById(obj).style.display = 'none';
        }
        function Show(obj) {
            document.getElementById(obj).style.display = 'inline';
        }
        function FnSearchBy(obj) {
            if (obj == "SettNo") {
                Show('Td_Settlements');
                Hide('Tr_Period');
            }
            if (obj == "Payin") {
                Hide('Td_Settlements');
                Show('Tr_Period');
            }
            FnFinalShortage();
        }
        function FnSettlements(obj) {
            if (obj != "All")
                Show('Td_Settlemnets');
            else
                Hide('Td_Settlemnets');

            FnFinalShortage();
        }
        function FnGroupBy(obj) {
            if (obj == "Group") {
                Show('Td_Group');
                document.getElementById('BtnGroup').click();
            }
            else
                Hide('Td_Group');

            Hide('showFilter');
        }
        function FnAll(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                document.getElementById('cmbsearchOption').value = document.getElementById('DdlGroupBy').value;
                Show('showFilter');
            }

        }
        function FnAsset(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                document.getElementById('cmbsearchOption').value = "Asset";
                Show('showFilter');
            }

        }
        function FnScrip(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                document.getElementById('cmbsearchOption').value = "Scrip";
                Show('showFilter');
            }

        }
        function FunCallAjaxList(objID, objEvent, ObjType) {
            var strQuery_Table = '';
            var strQuery_FieldName = '';
            var strQuery_WhereClause = '';
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = '';

            if (ObjType == 'SettNo') {
                strQuery_Table = "Master_Settlements";
                strQuery_FieldName = "distinct top 10 RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix)+'  ' + REPLACE(CONVERT(VARCHAR(9), settlements_StartDateTime, 6), ' ', '-') AS [DD-Mon-YY],RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix)";
                var Exchange = document.getElementById("<%=DdlExchange.ClientID%>").options[document.getElementById("<%=DdlExchange.ClientID%>").selectedIndex].text;

                if (Exchange == "NSE")
                    Exchange = "1"
                else if (Exchange == "BSE")
                    Exchange = "4"
                else if (Exchange == "CSE")
                    Exchange = "15"

                if (Exchange != "ALL")
                    strQuery_WhereClause = " settlements_exchangesegmentid=" + Exchange + " and Settlements_FinYear='<%=Session["LastFinYear"]%>' and ((RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix)) like (\'%RequestLetter%') or CONVERT(VARCHAR(9), settlements_StartDateTime, 6) like (\'%RequestLetter%')) ";
                else
                    strQuery_WhereClause = " settlements_exchangesegmentid in (1,4,15) and Settlements_FinYear='<%=Session["LastFinYear"]%>' and ((RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix)) like (\'%RequestLetter%') or CONVERT(VARCHAR(9), settlements_StartDateTime, 6) like (\'%RequestLetter%')) ";

            }
            else {


                if (document.getElementById('cmbsearchOption').value == "Clients") {
                    var Exchange = document.getElementById("<%=DdlExchange.ClientID%>").options[document.getElementById("<%=DdlExchange.ClientID%>").selectedIndex].text;
                    Exchange = Exchange + ' - CM';

                    strQuery_Table = "tbl_master_contact,tbl_master_contactexchange,tbl_master_branch";
                    strQuery_FieldName = "distinct top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID";

                    if (Exchange != 'ALL - CM')
                        strQuery_WhereClause = "  branch_id=cnt_branchid and crg_exchange='" + Exchange + "' and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' and (crg_tcode Like (\'RequestLetter%') or CNT_FIRSTNAME like (\'RequestLetter%')) and cnt_branchid in (<%=Session["userbranchHierarchy"]%>)";
                      else
                          strQuery_WhereClause = "  branch_id=cnt_branchid and crg_exchange in ('NSE - CM','BSE - CM','CSE - CM') and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' and (crg_tcode Like (\'RequestLetter%') or CNT_FIRSTNAME like (\'RequestLetter%')) and cnt_branchid in (<%=Session["userbranchHierarchy"]%>)";

                  }
                  if (document.getElementById('cmbsearchOption').value == "Branch") {
                      strQuery_Table = "tbl_master_branch";
                      strQuery_FieldName = "top 10 branch_description+'-'+branch_code,branch_id";
                      strQuery_WhereClause = " (branch_description Like (\'%RequestLetter%') or branch_code like (\'%RequestLetter%')) and branch_id in (<%=Session["userbranchHierarchy"]%>)";
               }
               if (document.getElementById('cmbsearchOption').value == "Group") {
                   strQuery_Table = "tbl_master_groupmaster";
                   strQuery_FieldName = "top 10 gpm_description+'-'+gpm_code ,gpm_id";
                   strQuery_WhereClause = " (gpm_description Like (\'%RequestLetter%') or gpm_code like (\'%RequestLetter%')) and gpm_type='" + document.getElementById('DdlGrpType').value + "'";
               }
               if (document.getElementById('cmbsearchOption').value == "Asset") {
                   strQuery_Table = "master_products";
                   strQuery_FieldName = "distinct top 10 ltrim(rtrim(products_name))+\' [\'+rtrim(products_shortname)+\']\',rtrim(products_id)";
                   strQuery_WhereClause = " products_producttypeid in (1,5) and (products_name like (\'RequestLetter%') or products_shortname like (\'RequestLetter%')) ";
               }
               if (document.getElementById('cmbsearchOption').value == "Scrip") {
                   var ExchangeSegmentid = '<%=Session["ExchangeSegmentID"]%>';
                    strQuery_Table = "(select isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),Equity_TickerCode) as TickerSymbol,Equity_SeriesID from Master_Equity  WHERE Equity_ExchSegmentID='" + ExchangeSegmentid + "')as tb";
                    strQuery_FieldName = "distinct top 10 TickerSymbol,Equity_SeriesID";
                    strQuery_WhereClause = " (TickerSymbol like (\'RequestLetter%') )";
                }
                if (document.getElementById('cmbsearchOption').value == "Email") {
                    strQuery_Table = "tbl_master_email INNER JOIN tbl_master_contact ON tbl_master_email.eml_cntId = tbl_master_contact.cnt_internalId ";
                    strQuery_FieldName = "distinct top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'') + '< '+ tbl_master_email.eml_email + '>' as name,cnt_internalId as contactid ";
                    strQuery_WhereClause = " (tbl_master_email.eml_email <> '') AND cnt_contacttype ='EM'  and (cnt_firstName like (\'RequestLetter%') or cnt_ucc like (\'RequestLetter%') or tbl_master_email.eml_email like (\'RequestLetter%'))";
                }
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
            document.getElementById('BtnScreen').disabled = false;
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

        function FnGenerateType(obj) {
            if (obj == "Screen") {
                Show('Td_Screen');
                Hide('Td_Export');
                Hide('Td_Email');
                Hide('Tr_EmailGeneration');
            }
            if (obj == "Export") {
                Hide('Td_Screen');
                Show('Td_Export');
                Hide('Td_Email');
                Hide('Tr_EmailGeneration');
            }
            if (obj == "EMail") {

                Hide('Td_Screen');
                Hide('Td_Export');
                Show('Td_Email');
                Show('Tr_EmailGeneration');
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'Email';
                //Show('showFilter');
                document.getElementById('Button1').click();

            }
            Hide('showFilter');
            height();
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
        function mailoption(objval) {
            if (objval == '1')
                Show('showFilter');
            else
                Hide('showFilter');

        }
        //  function FnSendEmail(obj)
        //  {
        //        
        //        if(obj=='Branch/Group')
        //        {
        //          Hide('showFilter'); 
        //          }
        //          if(obj=='Client')
        //          {
        //          Hide('showFilter');
        //          }  
        //        if(obj=='User')
        //        {
        //          document.getElementById('cmbsearchOption').value='Email';
        //          Show('showFilter');   
        //        }
        //  }  
        function FnFinalShortage() {
            if (document.getElementById('RdbPositionTypeBoth').checked && document.getElementById('DdlSettlements').value == "For" && document.getElementById('DdlSearchBy').value == "SettNo")
                Show('Td_TransferTypeFinalShortage');
            else
                Hide('Td_TransferTypeFinalShortage');
        }
        function FnAlert(obj, colcount) {

            if (obj == 'DisPlay') {
                Hide('Tab_Selection');
                Show('Tab_Record');
                Show('TdFilter');

                if (colcount > 11) {
                    document.getElementById('Divdisplay').className = "grid_scroll";
                }

            }
            else {
                document.getElementById('Divdisplay').innerHTML = "";
                Show('Tab_Selection');
                Hide('Tab_Record');
                Hide('TdFilter');
                document.getElementById('ddlGeneration').value = 'Screen';


                if (obj != '3')
                    alert(obj);
                Page_Load();

            }

            Hide('showFilter');
            height();
        }
        FieldName = 'lstSlection';
    </script>

    <script type="text/ecmascript">
        function ReceiveServerData(rValue) {
            var j = rValue.split('~');

            if (j[0] == 'Scrip')
                document.getElementById('HiddenField_Scrip').value = j[1];
            else if (j[0] == 'Asset')
                document.getElementById('HiddenField_Asset').value = j[1];
            else if (j[0] == 'Email')
                document.getElementById('HiddenField_Email').value = j[1];
            else
                document.getElementById('HiddenField_ALL').value = j[1];


        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3600">
        </asp:ScriptManager>

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

        <div>
            <table class="TableMain100">
                <tr>
                    <td class="EHEADER" colspan="0" style="text-align: center; height: 20px;">
                        <strong><span id="SpanHeader" style="color: #000099">Delivery Position </span></strong>
                    </td>
                    <td class="EHEADER" width="15%" id="TdFilter" style="height: 22px">
                        <a href="javascript:void(0);" onclick="FnAlert(3,1);"><span style="color: Blue; text-decoration: underline;
                            font-size: 8pt; font-weight: bold">Filter</span></a>
                        <asp:DropDownList ID="cmbExport" runat="server" AutoPostBack="True" Font-Size="12px" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                            <asp:ListItem Value="E">Excel</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table id="Tab_Selection" border="0" cellpadding="0" cellspacing="0">
                <tr valign="top">
                    <td class="gridcellleft">
                        <table>
                            <tr>
                                <td class="gridcellleft">
                                    <table>
                                        <tr>
                                            <td valign="top">
                                                <table cellpadding="1" cellspacing="1" class="tableClass">
                                                    <tr valign="top">
                                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                                            Exchange :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="DdlExchange" Font-Size="12px" runat="server" Width="100px">
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
                                <td class="gridcellleft" style="width: 920px">
                                    <table>
                                        <tr>
                                            <td valign="top">
                                                <table  cellpadding="1" cellspacing="1" class="tableClass">
                                                    <tr valign="top">
                                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                                            Search By :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="DdlSearchBy" runat="server" Width="100px" Font-Size="12px"
                                                                onchange="FnSearchBy(this.value)">
                                                                <asp:ListItem Value="SettNo">Settlements</asp:ListItem>
                                                                <asp:ListItem Value="Payin"> Payin-Date</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="gridcellleft" id="Td_Settlements">
                                                            <table cellpadding="1" cellspacing="1" class="tableClass">
                                                                <tr valign="top">
                                                                    <td class="gridcellleft" bgcolor="#B7CEEC">
                                                                        Settlements:
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="DdlSettlements" runat="server" Width="100px" Font-Size="12px"
                                                                            onchange="FnSettlements(this.value)">
                                                                            <asp:ListItem Value="All">All</asp:ListItem>
                                                                            <asp:ListItem Value="For">For</asp:ListItem>
                                                                            <asp:ListItem Value="UpTo">UpTo</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td id="Td_Settlemnets">
                                                                        <asp:TextBox ID="txtSettlements" runat="server" Font-Size="12px" onkeyup="FunCallAjaxList(this,event,'SettNo')"
                                                                            Width="200Px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td class="gridcellleft" id="Tr_Period">
                                                            <table cellpadding="1" cellspacing="1" class="tableClass">
                                                                <tr>
                                                                    <td class="gridcellleft" bgcolor="#B7CEEC">
                                                                        For The Period :
                                                                    </td>
                                                                    <td class="gridcellleft">
                                                                        <dxe:ASPxDateEdit ID="DtFrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                                            Font-Size="12px" Width="108px" ClientInstanceName="DtFrom">
                                                                            <dropdownbutton text="From">
                                                </dropdownbutton>
                                                                        </dxe:ASPxDateEdit>
                                                                    </td>
                                                                    <td class="gridcellleft">
                                                                        <dxe:ASPxDateEdit ID="DtTo" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                                            Font-Size="12px" Width="108px" ClientInstanceName="DtTo">
                                                                            <dropdownbutton text="To">
                                                </dropdownbutton>
                                                                        </dxe:ASPxDateEdit>
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
                                <td class="gridcellleft" style="width: 920px">
                                    <table>
                                        <tr>
                                            <td valign="top">
                                                <table  cellpadding="1" cellspacing="1" class="tableClass">
                                                    <tr valign="top">
                                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                                            For:
                                                        </td>
                                                        <td class="gridcellleft">
                                                            <asp:CheckBox ID="ChkForClients" runat="server" Checked="true" />Clients
                                                        </td>
                                                        <td class="gridcellleft">
                                                            <asp:CheckBox ID="ChkForExchange" runat="server" />Exchange
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
                                    <table  cellpadding="1" cellspacing="1" class="tableClass">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Group By</td>
                                            <td>
                                                <asp:DropDownList ID="DdlGroupBy" runat="server" Width="100px" Font-Size="12px" onchange="FnGroupBy(this.value)">
                                                    <asp:ListItem Value="Clients">Clients</asp:ListItem>
                                                    <asp:ListItem Value="Branch">Branch</asp:ListItem>
                                                    <asp:ListItem Value="Group">Group</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td id="Td_Group" style="display: none;" colspan="2">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList ID="DdlGrpType" runat="server" Font-Size="12px">
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="BtnGroup" EventName="Click"></asp:AsyncPostBackTrigger>
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="rdAll" runat="server" Checked="True" GroupName="a" onclick="FnAll('a')" />
                                                            All
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdSelected" runat="server" GroupName="a" onclick="FnAll('b')" />Selected
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
                                    <table  cellpadding="1" cellspacing="1" class="tableClass">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Asset :</td>
                                            <td>
                                                <asp:RadioButton ID="RdAssetAll" runat="server" Checked="True" GroupName="b" onclick="FnAsset('a')" />
                                                All
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RdAssetSelected" runat="server" GroupName="b" onclick="FnAsset('b')" />Selected
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <table  cellpadding="1" cellspacing="1" class="tableClass">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Scrip :</td>
                                            <td>
                                                <asp:RadioButton ID="RdScripAll" runat="server" Checked="True" GroupName="c" onclick="FnScrip('a')" />
                                                All
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RdScripSelected" runat="server" GroupName="c" onclick="FnScrip('b')" />Selected
                                            </td>
                                            
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                           
                            <tr>
                                <td class="gridcellleft">
                                    <table  cellpadding="1" cellspacing="1" class="tableClass">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Position Type:</td>
                                            <td>
                                                <asp:RadioButton ID="RdbPositionTypeIncoming" runat="server" Checked="True" GroupName="e" onclick="FnFinalShortage()"/>
                                                Incoming
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RdbPositionTypeOutgoing" runat="server" GroupName="e" onclick="FnFinalShortage()"/>Outgoing
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RdbPositionTypeBoth" runat="server" GroupName="e" onclick="FnFinalShortage()"/>
                                                Both
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                             <tr>
                                <td class="gridcellleft">
                                    <table  cellpadding="1" cellspacing="1" class="tableClass">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Transfer Type:</td>
                                            <td>
                                                <asp:RadioButton ID="RdbTransferTypeAll" runat="server" GroupName="d" />
                                                All
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RdbTransferTypePending" runat="server" GroupName="d" Checked="True" />Pending
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RdbTransferTypeTransferred" runat="server" GroupName="d" />
                                                Transferred
                                            </td>
                                            <td id="Td_TransferTypeFinalShortage">
                                                <asp:RadioButton ID="RdbTransferTypeFinalShortage" runat="server" GroupName="d" />Final
                                                Shortage
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                             <tr>
                                <td class="gridcellleft">
                                    <table cellpadding="1" cellspacing="1" class="tableClass">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Order By:</td>
                                            <td>
                                                <asp:DropDownList ID="DdlOrderBy" runat="server" Width="200px" Font-Size="12px"
                                                    >
                                                    <asp:ListItem Value="Client">Client</asp:ListItem>
                                                    <asp:ListItem Value="Group">Group</asp:ListItem>
                                                    <asp:ListItem Value="Scrip">Scrip</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <table  cellpadding="1" cellspacing="1" class="tableClass">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Generate Type :</td>
                                            <td>
                                                <asp:DropDownList ID="ddlGeneration" runat="server" Width="200px" Font-Size="12px"
                                                    onchange="FnGenerateType(this.value)">
                                                    <asp:ListItem Value="Screen">Screen</asp:ListItem>
                                                    <asp:ListItem Value="Export">Export</asp:ListItem>
                                                    <asp:ListItem Value="EMail">E-Mail</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <%--<tr id="Tr_EmailGeneration">
                                <td class="gridcellleft">
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Send Email :</td>
                                            <td>
                                                <asp:DropDownList ID="DdlSendEmail" runat="server" Width="200px" Font-Size="12px"
                                                    onchange="FnSendEmail(this.value)">
                                                    <asp:ListItem Value="Branch/Group">Branch/Group</asp:ListItem>
                                                    <asp:ListItem Value="User">User</asp:ListItem>
                                                    <asp:ListItem Value="Client">Client</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>--%>
                             <tr id="Tr_EmailGeneration">
                            <td class="gridcellleft">
                               <asp:UpdatePanel ID="upanelRespective" runat="Server" UpdateMode="Conditional">
                               <ContentTemplate>
                                <table id="tabRespective" runat="Server" border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Respective :</td>
                                        <td>
                                            <asp:DropDownList ID="ddloptionformail" onchange="mailoption(this.value)" runat="server" Width="100px" Font-Size="12px"
                                                >
                                                
                                            </asp:DropDownList></td>
                                    </tr>
                                </table>
                                </ContentTemplate>
                                <Triggers> <asp:AsyncPostBackTrigger ControlID="Button1" /> </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <table>
                                        <tr>
                                            <td id="Td_Screen">
                                                <asp:Button ID="BtnScreen" runat="server" CssClass="btnUpdate" Height="20px" Text="Screen"
                                                    Width="101px" OnClientClick="selecttion()" OnClick="BtnScreen_Click" />
                                            </td>
                                            <td id="Td_Export">
                                                <asp:Button ID="BtnExcel" runat="server" CssClass="btnUpdate" Height="20px" Text="Export To Excel"
                                                    Width="101px" OnClientClick="selecttion()" OnClick="BtnExcel_Click" /></td>
                                            <td id="Td_Email">
                                                <asp:Button ID="BtnEmail" runat="server" CssClass="btnUpdate" Height="20px" Text="Send Email"
                                                    Width="101px" OnClientClick="selecttion()" OnClick="BtnEmail_Click" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top" align="left">
                        <table  cellpadding="1" cellspacing="1" id="showFilter" class="tableClass">
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="320px" onkeyup="FunCallAjaxList(this,event,'Other')"></asp:TextBox></td>
                                <td>
                                    <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                        Enabled="false">
                                        <asp:ListItem Value="Branch">Branch</asp:ListItem>
                                        <asp:ListItem Value="Clients">Clients</asp:ListItem>
                                        <asp:ListItem Value="Group">Group</asp:ListItem>
                                        <asp:ListItem Value="Asset">Asset</asp:ListItem>
                                        <asp:ListItem Value="Scrip">Scrip</asp:ListItem>
                                        <asp:ListItem Value="Email">Email</asp:ListItem>
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
                                    <asp:ListBox ID="lstSlection" runat="server" Font-Size="12px" Height="120px" Width="400px">
                                    </asp:ListBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <a id="P1" href="javascript:void(0);" onclick="clientselectionfinal()"><span style="color: #000099;
                                                    text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
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
            <table>
                <tr>
                    <td style="display: none;">
                        <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                        <asp:HiddenField ID="HiddenField_ALL" runat="server" />
                        <asp:HiddenField ID="hiddencount" runat="server" />
                        <asp:HiddenField ID="HiddenField_Scrip" runat="server" />
                        <asp:HiddenField ID="HiddenField_Asset" runat="server" />
                        <asp:HiddenField ID="HiddenField_Email" runat="server" />
                         <asp:Button ID="BtnGroup" runat="server" Text="BtnGroup" OnClick="BtnGroup_Click" />
                         <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
                        <asp:TextBox ID="txtSettlements_hidden" runat="server" Font-Size="12px" Width="200Px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <div id='Div1' style='position: absolute; font-family: arial; font-size: 30; left: 50%;
                                    top: 50%; background-color: white; layer-background-color: white; height: 80;
                                    width: 150;'>
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
            <table id="Tab_Record">
                <tr>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                            <ContentTemplate>
                                <table width="100%" border="1">
                                    <tr>
                                        <td>
                                            <div id="DivHeader" runat="server">
                                            </div>
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
                    </td>
                </tr>
            </table>
        </div>
</asp:Content>
