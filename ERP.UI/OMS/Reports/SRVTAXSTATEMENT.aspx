<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_SRVTAXSTATEMENT" CodeBehind="SRVTAXSTATEMENT.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>--%>
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/assests/js/init.js"></script>
    <script type="text/javascript" src="../CentralData/JSScript/GenericJScript.js"></script>
    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>
    

    <style type="text/css">
        .tableClass {
            /* border: 0px; */
            border: 1px solid #aaa !important;
            border-collapse: collapse !important;
        }
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
            overflow-y: scroll;
            overflow-x: scroll;
            width: 80%;
            scrollbar-base-color: #C0C0C0;
        }
    </style>

    <script language="javascript" type="text/javascript">


        function Page_Load()///Call Into Page Load
        {
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

        function FnBranch(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                document.getElementById('cmbsearchOption').value = 'Branch';
                Show('showFilter');
            }

        }
        function FnSegment(obj) {
            if (obj == "a")
                Hide('showFilter');
            else if (obj == "c") {
                Hide('Tab_showFilter');
                Show('Td_Specific');
            }
            else {
                var cmb = document.getElementById('cmbsearchOption').value = 'Segment';
                Show('showFilter');
            }

        }
        function FunCallAjaxList(objID, objEvent) {
            var strQuery_Table = '';
            var strQuery_FieldName = '';
            var strQuery_WhereClause = '';
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = '';

            if (document.getElementById('cmbsearchOption').value == "Branch") {
                strQuery_Table = "tbl_master_branch";
                strQuery_FieldName = "top 10 branch_description+'-'+branch_code,branch_id";
                strQuery_WhereClause = " (branch_description Like (\'RequestLetter%') or branch_code like (\'RequestLetter%')) and branch_id in (<%=Session["userbranchHierarchy"]%>)";
            }
            if (document.getElementById('cmbsearchOption').value == "Segment") {
                strQuery_Table = "(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName +\'-'\ + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE Where  TMCE.EXCH_COMPID=\'<%=Session["LastCompany"]%>'\) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID)as TAB";
               strQuery_FieldName = "distinct top 10 EXCHANGENAME,SEGMENTID";
               strQuery_WhereClause = " EXCHANGENAME like (\'%RequestLetter%')";
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

           }
           if (obj == "Export") {
               Hide('Td_Screen');
               Show('Td_Export');

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

       function FnAlert(obj) {

           if (obj == 'DisPlay') {
               Hide('Tab_Selection');
               Show('Tab_Record');
               Show('TdFilter');


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
       function ddlReportType_OnChange(ObjValue) {
           if (ObjValue == "Contract")//// When ContractWise
           {
               //ddlAsPer alway Will Be CntrNo
               DDL_SetIndex("ddlAsPer", 0);
               Chk_TrueOrFalse("ChkSegmentBreakUp", "False");
               EnableDisableControl("ddlAsPer", 'D');
               EnableDisableControl("ChkSegmentBreakUp", 'D');
           }
           else {
               DDL_SetIndex("ddlAsPer", 0);
               EnableDisableControl("ddlAsPer", 'E');
               EnableDisableControl("ChkSegmentBreakUp", 'E');
           }
       }


       FieldName = 'lstSlection';
    </script>

    <script type="text/ecmascript">
        function ReceiveServerData(rValue) {
            var j = rValue.split('~');

            if (j[0] == 'Segment')
                document.getElementById('HiddenField_Segment').value = j[1];
            else if (j[0] == 'Branch')
                document.getElementById('HiddenField_Branch').value = j[1];



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
                    <strong><span id="SpanHeader" style="color: #000099">Service Tax Statement </span></strong>
                </td>
                <td class="EHEADER" width="15%" id="TdFilter" style="height: 22px">
                    <a href="javascript:void(0);" onclick="FnAlert(3);"><span style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight: bold">Filter</span></a>
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
                                <table cellpadding="1" cellspacing="1" class="tableClass">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">For The Period :
                                        </td>
                                        <td class="gridcellleft">
                                            <dxe:ASPxDateEdit ID="DtFrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="108px" ClientInstanceName="DtFrom">
                                                <DropDownButton Text="From">
                                                </DropDownButton>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td class="gridcellleft">
                                            <dxe:ASPxDateEdit ID="DtTo" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="108px" ClientInstanceName="DtTo">
                                                <DropDownButton Text="To">
                                                </DropDownButton>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table>
                                    <tr>
                                        <td valign="top">
                                            <table cellpadding="1" cellspacing="1" class="tableClass">
                                                <tr valign="top">
                                                    <td class="gridcellleft" bgcolor="#B7CEEC">Report Type :</td>
                                                    <td>
                                                        <asp:DropDownList ID="DdlReportType" runat="server" Width="100px" Font-Size="12px" onchange="ddlReportType_OnChange(this.value);">
                                                            <asp:ListItem Value="Date">Date Wise</asp:ListItem>
                                                            <asp:ListItem Value="Month">Month Wise</asp:ListItem>
                                                            <asp:ListItem Value="Contract">Contract Wise</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td valign="top" class="gridcellleft">
                                            <table cellpadding="1" cellspacing="1" class="tableClass">
                                                <tr valign="top">
                                                    <td class="gridcellleft" bgcolor="#B7CEEC">Consider :</td>
                                                    <td>
                                                        <asp:DropDownList ID="DdlConsider" runat="server" Width="100px" Font-Size="12px">
                                                            <asp:ListItem Value="Trade Date">Trade Date</asp:ListItem>
                                                            <asp:ListItem Value="Payout Date">Payout Date</asp:ListItem>
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
                            <td class="gridcellleft">
                                <table>
                                    <tr>
                                        <td valign="top">
                                            <table cellpadding="1" cellspacing="1" class="tableClass">
                                                <tr valign="top">
                                                    <td class="gridcellleft" bgcolor="#B7CEEC">As Per :</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlAsPer" runat="server" Width="150px" Font-Size="12px">
                                                            <asp:ListItem Value="CntrNo">Contract Note</asp:ListItem>
                                                            <asp:ListItem Value="Bill">Bills</asp:ListItem>
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
                            <td class="gridcellleft">
                                <table cellpadding="1" cellspacing="1" class="tableClass">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Segment :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbSegmentAll" runat="server" GroupName="bb" onclick="FnSegment('a')" />All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbSegmentSpecific" runat="server" Checked="True" GroupName="bb"
                                                onclick="FnSegment('c')" />Specific
                                        </td>
                                        <td>[ <span id="litSegmentMain" runat="server" style="color: Maroon"></span>]</td>
                                        <td id="Td_SegmentSelected">
                                            <asp:RadioButton ID="rdSegmentSelected" runat="server" GroupName="bb" onclick="FnSegment('b')" />Selected
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table cellpadding="1" cellspacing="1" class="tableClass">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Branch :</td>
                                        <td>
                                            <asp:RadioButton ID="rdBranch" runat="server" Checked="True" GroupName="b" onclick="FnBranch('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdBranchSelected" runat="server" GroupName="b" onclick="FnBranch('b')" />Selected
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:CheckBox ID="ChkSegmentBreakUp" runat="server" />Show Segment BreakUp
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <tr>
                            <td class="gridcellleft">
                                <table cellpadding="1" cellspacing="1" class="tableClass">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Generate Type :</td>
                                        <td>
                                            <asp:DropDownList ID="ddlGeneration" runat="server" Width="200px" Font-Size="12px"
                                                onchange="FnGenerateType(this.value)">
                                                <asp:ListItem Value="Screen">Screen</asp:ListItem>
                                                <asp:ListItem Value="Export">Export</asp:ListItem>

                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
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

                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top" align="left">
                    <table cellpadding="1" cellspacing="1" id="showFilter">
                        <tr>
                            <td>
                                <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="320px" onkeyup="FunCallAjaxList(this,event)"></asp:TextBox></td>
                            <td>
                                <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                    Enabled="false">
                                    <asp:ListItem Value="Branch">Branch</asp:ListItem>
                                    <asp:ListItem Value="Segment">Segment</asp:ListItem>

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
        <table>
            <tr>
                <td style="display: none;">
                    <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                    <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                    <asp:HiddenField ID="HiddenField_Segment" runat="server" />
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
                            <asp:AsyncPostBackTrigger ControlID="BtnScreen" EventName="Click"></asp:AsyncPostBackTrigger>
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

