<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_CrossDeals" CodeBehind="CrossDeals.aspx.cs" %>


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

        .grid_scroll {
            overflow-y: scroll;
            overflow-x: scroll;
            width: 80%;
            scrollbar-base-color: #C0C0C0;
        }

        form {
            display: inline;
        }
    </style>
    <script language="javascript" type="text/javascript">


     function Page_Load()///Call Into Page Load
     {
         Hide('showFilter');
         FnGroupBy('Client');
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
     function FnScrip(obj) {
         if (obj == "a")
             Hide('showFilter');
         else {
             document.getElementById('cmbsearchOption').value = "Scrip";
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

         var date;
         date = new Date(DtTo.GetDate());
         date = parseInt(date.getMonth() + 1) + '-' + date.getDate() + '-' + date.getFullYear();

         if (document.getElementById('cmbsearchOption').value == "Client") {
             strQuery_Table = "tbl_master_contact,tbl_master_contactexchange,tbl_master_branch";
             strQuery_FieldName = "distinct top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID";
             strQuery_WhereClause = "  branch_id=cnt_branchid and crg_exchange='" + document.getElementById('HiddenField_SegmentName').value + "' and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' and (crg_tcode Like (\'RequestLetter%') or CNT_FIRSTNAME like (\'RequestLetter%')) and cnt_branchid in (<%=Session["userbranchHierarchy"]%>)";
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
           if (document.getElementById('cmbsearchOption').value == "Scrip") {
               var ExchangeSegmentid = '<%=Session["ExchangeSegmentID"]%>';

               if (ExchangeSegmentid == "1" || ExchangeSegmentid == "4" || ExchangeSegmentid == "19") {
                   strQuery_Table = "(select isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),Equity_TickerCode) as TickerSymbol,Equity_SeriesID from Master_Equity  WHERE Equity_ExchSegmentID='" + ExchangeSegmentid + "')as tb";
                   strQuery_FieldName = "distinct top 10 TickerSymbol,Equity_SeriesID";
                   strQuery_WhereClause = " (TickerSymbol like (\'RequestLetter%') )";
               }
               else if (ExchangeSegmentid == "2" || ExchangeSegmentid == "5" || ExchangeSegmentid == "20") {
                   strQuery_Table = "(select (case when isnull(Equity_StrikePrice,0)=0.0 then isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6) else isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6)+' '+cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar) end) as TickerSymbol,Equity_SeriesID from Master_Equity  WHERE Equity_ExchSegmentID='" + ExchangeSegmentid + "' and equity_effectuntil>='" + date + "' )as tb";
                   strQuery_FieldName = "distinct top 10 TickerSymbol,Equity_SeriesID";
                   strQuery_WhereClause = " (TickerSymbol like (\'RequestLetter%') )";
               }
               else {
                   strQuery_Table = "(select (case when isnull(commodity_StrikePrice,0)=0.0 then isnull(rtrim(ltrim(commodity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(commodity_Tickercode)),isnull(rtrim(ltrim(Commodity_Tickerseries)),''))+' '+isnull(convert(varchar(9),commodity_ExpiryDate,6),'') else isnull(rtrim(ltrim(commodity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(commodity_Tickercode)),isnull(rtrim(ltrim(Commodity_Tickerseries)),''))+' '+isnull(convert(varchar(9),commodity_ExpiryDate,6),'')+' '+cast(cast(round(commodity_StrikePrice,2) as numeric(28,2)) as varchar) end) as TickerSymbol,Commodity_ProductSeriesID from Master_commodity  WHERE commodity_ExchangeSegmentID='" + ExchangeSegmentid + "' and commodity_expirydate>='" + date + "' )as tb";
                   strQuery_FieldName = "distinct top 10 TickerSymbol,Commodity_ProductSeriesID";
                   strQuery_WhereClause = " (TickerSymbol like (\'RequestLetter%') )";
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

       function FnAlert(obj) {
           alert(obj);
           Page_Load();
       }

       FieldName = 'lstSlection';
       </script>



    <script type="text/ecmascript">
      function ReceiveServerData(rValue) {
    var j = rValue.split('~');
    if (j[0] == 'Scrip')
        document.getElementById('HiddenField_Scrip').value = j[1];
    else
        document.getElementById('HiddenField_ALL').value = j[1];


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
                    <strong><span id="SpanHeader" style="color: #000099">Cross Deals</span></strong></td>


            </tr>
        </table>
        <table id="tab1" border="0" cellpadding="0" cellspacing="0">
            <tr valign="top">
                <td>
                    <table>

                        <tr>
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Period :</td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="DtFrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="108px" ClientInstanceName="DtFrom">
                                                <DropDownButton Text="From">
                                                </DropDownButton>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td>
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
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Group By</td>
                                        <td>
                                            <asp:DropDownList ID="DdlGroupBy" runat="server" Width="100px" Font-Size="12px" onchange="FnGroupBy(this.value)">
                                                <asp:ListItem Value="Client">Client</asp:ListItem>
                                                <asp:ListItem Value="Branch">Branch</asp:ListItem>
                                                <asp:ListItem Value="Group">Group</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td id="Td_Group" style="display: none;" colspan="2">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
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
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Scrip :</td>
                                        <td>
                                            <asp:RadioButton ID="RdScripAll" runat="server" Checked="True" GroupName="b" onclick="FnScrip('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="RdScripSelected" runat="server" GroupName="b" onclick="FnScrip('b')" />Selected
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>






                        <tr>
                            <td class="gridcellleft">
                                <asp:Button ID="BtnExport" runat="server" CssClass="btnUpdate" Height="20px" Text="Export To Excel"
                                    Width="101px" OnClientClick="selecttion()" OnClick="BtnExport_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table border="10" cellpadding="1" cellspacing="1" id="showFilter">
                        <tr>
                            <td class="gridcellleft" style="vertical-align: top; text-align: right; height: 21px;"
                                id="TdFilter">
                                <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="FunCallAjaxList(this,event)"></asp:TextBox>
                                <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                    Enabled="false">
                                    <asp:ListItem>Client</asp:ListItem>
                                    <asp:ListItem>Branch</asp:ListItem>
                                    <asp:ListItem>Group</asp:ListItem>
                                    <asp:ListItem>Scrip</asp:ListItem>

                                </asp:DropDownList>
                                <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                    style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                        style="color: #009900; font-size: 8pt;"> </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ListBox ID="lstSlection" runat="server" Font-Size="12px" Height="90px" Width="290px"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <a id="A2" href="javascript:void(0);" onclick="clientselectionfinal()"><span style="color: #000099; text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
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
        <table>
            <tr>
                <td style="display: none;">
                    <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                    <asp:Button ID="BtnGroup" runat="server" Text="BtnGroup" OnClick="BtnGroup_Click" />
                    <asp:HiddenField ID="HiddenField_ALL" runat="server" />
                    <asp:HiddenField ID="HiddenField_Scrip" runat="server" />
                    <asp:HiddenField ID="HiddenField_SegmentName" runat="server" />

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

    </div>
</asp:Content>
