<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_contractregistercomm" CodeBehind="contractregistercomm.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>
    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>

    
    <style type="text/css">
        /* Big box with list of options */
        #ajax_listOfOptions {
            position: absolute; /* Never change this one */
            width: 10px; /* Width of box */
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
        function SignOff() {
       window.parent.SignOff();
   }

               function Page_Load()///Call Into Page Load
               {

                   height();
                   document.getElementById('tr_filter').style.display = 'none';
                   document.getElementById('td_span').style.display = 'none';
                   document.getElementById('showFilter').style.display = 'none';
                   document.getElementById('TrFilter').style.display = 'none';


               }

               function height() {
                   if (document.body.scrollHeight >= 320) {
                       window.frameElement.height = document.body.scrollHeight;
                   }
                   else {
                       window.frameElement.height = '320px';
                   }
                   window.frameElement.width = document.body.scrollWidth;
                   document.getElementById('hidScreenWd').value = screen.width - 20;
               }

               function Show() {
                   document.getElementById('tr_filter').style.display = 'inline';
               }
               function CallList(obj1, obj2, obj3) {
                   var cmb = document.getElementById('cmbsearchOption');

                   ajax_showOptions(obj1, obj2, obj3, cmb.value);


               }
               function rdbtnSegAll(obj) {

                   document.getElementById('TrFilter').style.display = 'none';
                   document.getElementById('showFilter').style.display = 'none';

                   if (obj == 'other') {

                   }
                   else {
                       document.getElementById('btn_show').disabled = false;
                   }
               }
               function rdbtnSelected(obj) {

                   document.getElementById('TrFilter').style.display = 'inline';
                   document.getElementById('showFilter').style.display = 'inline';
                   if (obj == 'Client') {
                       document.getElementById('cmbsearchOption').value = 'Clients';
                       document.getElementById('btn_show').disabled = true;
                       document.getElementById('spanall').style.display = "inline";
                   }
                   else if (obj == 'Branch') {
                       document.getElementById('cmbsearchOption').value = 'Branch';
                       document.getElementById('btn_show').disabled = true;
                       document.getElementById('spanall').style.display = "inline";
                   }


               }


               function Disable(obj) {

                   var gridview = document.getElementById('grdContractRegister');
                   var rCount = gridview.rows.length;

                   if (rCount < 10)
                       rCount = '0' + rCount;

                   if (obj == 'P') {
                       document.getElementById("grdContractRegister_ctl" + rCount + "_FirstPage").style.display = 'none';
                       document.getElementById("grdContractRegister_ctl" + rCount + "_PreviousPage").style.display = 'none';
                       document.getElementById("grdContractRegister_ctl" + rCount + "_NextPage").style.display = 'inline';
                       document.getElementById("grdContractRegister_ctl" + rCount + "_LastPage").style.display = 'inline';
                   }
                   else {
                       document.getElementById("grdContractRegister_ctl" + rCount + "_NextPage").style.display = 'none';
                       document.getElementById("grdContractRegister_ctl" + rCount + "_LastPage").style.display = 'none';
                   }
               }
               function NoRecord() {

                   alert('No Record Found');
                   Page_Load();
               }
               function ALLSELECTED() {
                   document.getElementById('td_span').style.display = 'inline';
                   document.getElementById('tr_date').style.display = 'none';
                   document.getElementById('tr_branchid').style.display = 'none';
                   document.getElementById('tr_Clientid').style.display = 'none';
                   document.getElementById('btn_show').style.display = 'none';
                   document.getElementById('td_grid').style.display = 'inline';
                   document.getElementById('tr_branchdisplay').style.display = 'none';

                   height();
               }
               function btnCancel_Click() {
                   document.getElementById('td_span').style.display = 'none';
                   document.getElementById('tr_filter').style.display = 'none';
                   document.getElementById('tr_date').style.display = 'inline';
                   document.getElementById('tr_branchid').style.display = 'inline';
                   document.getElementById('tr_Clientid').style.display = 'inline';
                   document.getElementById('td_grid').style.display = 'none';
                   document.getElementById('btn_show').style.display = 'inline';
                   document.getElementById('tr_branchdisplay').style.display = 'inline';


                   height();
               }
               function displaydate(obj) {
                   document.getElementById('spanshow2').innerText = obj;
               }
               function ChangeRowColor(rowID, rowNumber) {
                   var gridview = document.getElementById('grdContractRegister');

                   var rCount = gridview.rows.length;
                   var rowIndex = 1;
                   var rowCount = 0;
                   if (rCount == 18)
                       rowCount = 15;
                   else
                       rowCount = rCount - 2;
                   if (rowNumber > 15 && rCount < 18)
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


               function ReceiveServerData(rValue) {
                   var Data = rValue.split('~');

                   if (Data[0] == 'Branch') {
                       var combo = document.getElementById('litBranchMain');
                       var NoItems = Data[1].split(',');
                       var i;
                       var val = '';
                       for (i = 0; i < NoItems.length; i++) {

                           var items = NoItems[i].split(';');

                           if (val == '') {
                               val = '(' + items[1];
                           }
                           else {
                               val += ',' + items[1];
                           }
                       }
                       val = val + ')';
                       combo.innerText = val;
                   }
                   if (Data[0] == 'Clients') {


                   }

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


                   document.getElementById('TrFilter').style.display = 'none';
                   document.getElementById('showFilter').style.display = 'none';
                   document.getElementById('btn_show').disabled = false;
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

               FieldName = 'lstSlection';
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



                   //if (postBackElement.id == 'ctl00_ContentPlaceHolder3_btnShow') 

                   $get('UpdateProgress1').style.display = 'block';
                   // document.getElementById('btn_show').disabled=true;

               }



               function EndRequest(sender, args) {

                   // if (postBackElement.id == 'ctl00_ContentPlaceHolder3_btnShow') 

                   $get('UpdateProgress1').style.display = 'none';
                   //document.getElementById('btn_show').disabled=false;




               }
               </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" colspan="0" style="text-align: center; height: 22px;">
                    <strong><span id="SpanHeader" style="color: #000099">Contract Register&nbsp;<asp:Label ID="lblSegment" runat="server"></asp:Label></span></strong>
                </td>
                <td class="EHEADER" width="25%" id="tr_filter" style="height: 22px">
                    <a href="javascript:void(0);"
                        onclick="btnCancel_Click();"><span style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight: bold">Filter</span></a>
                    <asp:DropDownList ID="ddlExport" runat="server" AutoPostBack="True" Font-Size="12px" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                        <asp:ListItem Value="E">Excel</asp:ListItem>
                        <asp:ListItem Value="P">PDF</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>


        <table>
            <tr>
                <td colspan="4" valign="top" style="width: 428px">
                    <table border="0">
                        <tr id="tr_date">
                            <td style="width: 110px">

                                <dxe:ASPxDateEdit ID="dtFrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Font-Size="12px" Width="108px" ClientInstanceName="dtfrom">
                                    <DropDownButton Text="From">
                                    </DropDownButton>
                                    <ClientSideEvents ValueChanged="function(s, e) {dateassign(s.GetValue());}" />

                                </dxe:ASPxDateEdit>
                            </td>
                            <td style="width: 110px">
                                <dxe:ASPxDateEdit ID="dtTo" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Font-Size="12px" Width="98px" ClientInstanceName="dtto">
                                    <DropDownButton Text="To">
                                    </DropDownButton>
                                    <ClientSideEvents ValueChanged="function(s, e) {dateassign(s.GetValue());}" />

                                </dxe:ASPxDateEdit>
                            </td>
                            <td colspan="1" style="text-align: left"></td>
                        </tr>
                        <tr id="tr_branchid">
                            <td class="gridcellleft" style="width: 73px">Branch
                            </td>
                            <td colspan="2" style="text-align: left">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdBranchAll" runat="server" Checked="True" GroupName="b" />
                                        </td>
                                        <td>All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdBranchSelected" runat="server" GroupName="b" />
                                        </td>
                                        <td>Selected
                                        </td>
                                        <td style="width: 3px"></td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="1" style="text-align: left"></td>
                        </tr>
                        <tr id="tr_branchdisplay">
                            <td class="gridcellleft" colspan="3">
                                <span id="litBranchMain" runat="server" style="color: maroon"></span>
                            </td>
                            <td class="gridcellleft" colspan="1"></td>
                        </tr>
                        <tr id="tr_Clientid">
                            <td class="gridcellleft" style="width: 73px">Clients
                            </td>
                            <td colspan="2" style="text-align: left">
                                <table>
                                    <tr>
                                        <td style="height: 36px">
                                            <asp:RadioButton ID="rdClientAll" runat="server" Checked="True" GroupName="a" />
                                        </td>
                                        <td style="width: 14px; height: 36px">All
                                        </td>
                                        <td style="height: 36px">
                                            <asp:RadioButton ID="rdClientSelected" runat="server" GroupName="a" />
                                        </td>
                                        <td style="width: 44px; height: 36px">Selected
                                        </td>
                                        <td style="width: 3px; height: 36px">
                                            <span id="litClientMain" runat="server" style="color: maroon"></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="1" style="text-align: left"></td>
                        </tr>

                        <tr>

                            <td style="width: 104px" colspan="3">
                                <asp:Button ID="btn_show" OnClick="btn_show_Click" runat="server" Width="101px" Height="23px"
                                    Text="Show" CssClass="btnUpdate"></asp:Button></td>
                            <td></td>
                        </tr>
                    </table>
                </td>
                <td class="gridcellleft" style="width: 100px;"></td>



                <td class="gridcellleft" style="vertical-align: top; text-align: right" id="TdFilter">
                    <table width="100%">
                        <tr id="TrFilter">
                            <td>
                                <span id="spanall">
                                    <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="225px"></asp:TextBox>
                                </span>
                                <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                    Enabled="false">
                                    <asp:ListItem>Clients</asp:ListItem>
                                    <asp:ListItem>Branch</asp:ListItem>
                                    <asp:ListItem>other</asp:ListItem>
                                </asp:DropDownList>
                                <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                    style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; vertical-align: top;">
                                <table cellpadding="0" cellspacing="0" id="showFilter">
                                    <tr>
                                        <td style="width: 291px">
                                            <asp:ListBox ID="lstSlection" runat="server" Font-Size="12px" Height="90px" Width="358px"></asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center; width: 291px;">
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
                        <tr>
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
                            <td style="display: none;">
                                <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                <asp:TextBox ID="dtFrom_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                <asp:TextBox ID="dtTo_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox></td>
                        </tr>

                    </table>
                </td>

            </tr>
        </table>
        <table>
            <tr>
                <td id="td_span">
                    <span id="spanshow1" style="color: Blue; font-weight: bold">Period :
                    </span>&nbsp;&nbsp;<span id="spanshow2"></span></td>

            </tr>
            <tr>
                <td colspan="2" style="height: 10px;"></td>
            </tr>
            <tr>
                <td colspan="2" id="td_grid">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="divgrid" runat="Server">
                                <asp:GridView ID="grdContractRegister" runat="server" BorderColor="CornflowerBlue"
                                    ShowFooter="True" AllowSorting="True" AutoGenerateColumns="False" BorderStyle="Solid"
                                    BorderWidth="2px" AllowPaging="True" PageSize="15" ForeColor="#0000C0" OnRowCreated="grdContractRegister_RowCreated"
                                    OnRowDataBound="grdContractRegister_RowDataBound" OnSorting="grdContractRegister_Sorting">
                                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Date" SortExpression="ContractNotes_TradeDate">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lbldate" runat="server" Text='<%# Eval("ContractNotes_TradeDate", "{0:dd MMM yy}") %>'
                                                    CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                Total
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Left" VerticalAlign="Top" Font-Bold="True" ForeColor="White" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CntNo." SortExpression="ContractNotes_Number">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCntNo" runat="server" Text='<%# Eval("ContractNotes_Number")%>'
                                                    CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name" SortExpression="Name">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name")%>' CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="UCC" SortExpression="UCC">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblUCC" runat="server" Text='<%# Eval("UCC")%>' CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Branch" SortExpression="Branch">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblBranch" runat="server" Text='<%# Eval("Branch")%>' CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Future To" SortExpression="ContractNotes_DelFutTO_Sorting">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDel" runat="server" Text='<%# Eval("ContractNotes_DelFutTO")%>'
                                                    CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lbldel_sum" runat="server" Text='<%# Eval("del")%>' ForeColor="white"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" VerticalAlign="Top" Font-Bold="True" ForeColor="White" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Option To" SortExpression="ContractNotes_SqrOptPrmTO_sorting">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSqr" runat="server" Text='<%# Eval("ContractNotes_SqrOptPrmTO")%>'
                                                    CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblsqr_sum" runat="server" Text='<%# Eval("sqr")%>' ForeColor="white"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" VerticalAlign="Top" Font-Bold="True" ForeColor="White" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total To" SortExpression="ContractNotes_TotalTO_Sorting">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("ContractNotes_TotalTO")%>'
                                                    CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lbltotalto_sum" runat="server" Text='<%# Eval("totalto")%>' ForeColor="white"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" VerticalAlign="Top" Font-Bold="True" ForeColor="White" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Brkg" SortExpression="ContractNotes_TotalBrokerage_sorting">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblBrkg" runat="server" Text='<%# Eval("ContractNotes_TotalBrokerage")%>'
                                                    CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lbltotalbrkg_sum" runat="server" Text='<%# Eval("totalbrkg")%>' ForeColor="white"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" VerticalAlign="Top" Font-Bold="True" ForeColor="White" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TranCharge" SortExpression="ContractNotes_TransactionCharges_Sorting">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblTran" runat="server" Text='<%# Eval("ContractNotes_TransactionCharges")%>'
                                                    CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lbltrancharge_sum" runat="server" Text='<%# Eval("trancharge")%>'
                                                    ForeColor="white"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" VerticalAlign="Top" Font-Bold="True" ForeColor="White" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="StampDuty" SortExpression="ContractNotes_StampDuty_Sorting">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblStampDuty" runat="server" Text='<%# Eval("ContractNotes_StampDuty")%>'
                                                    CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblstamp_sum" runat="server" Text='<%# Eval("stamp")%>' ForeColor="white"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" VerticalAlign="Top" Font-Bold="True" ForeColor="White" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="STT" SortExpression="ContractNotes_STTAmount_Sorting">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSTTTax" runat="server" Text='<%# Eval("ContractNotes_STTAmount")%>'
                                                    CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblStt_sum" runat="server" Text='<%# Eval("STTAmount")%>' ForeColor="white"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" VerticalAlign="Top" Font-Bold="True" ForeColor="White" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Sebi Fee" SortExpression="ContractNotes_SEBIFee">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Wrap="false" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSebifee" runat="server" Text='<%# Eval("ContractNotes_SEBIFee")%>'
                                                    CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblSebifee_sum" runat="server" ForeColor="white"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" VerticalAlign="Top" Font-Bold="True" ForeColor="White" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delivery Charge" SortExpression="ContractNotes_DeliveryCharges">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Wrap="false" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeliveryCharge" runat="server" Text='<%# Eval("ContractNotes_DeliveryCharges")%>'
                                                    CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblDeliveryCharge_sum" runat="server" ForeColor="white"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" VerticalAlign="Top" Font-Bold="True" ForeColor="White" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Other Charge" SortExpression="ContractNotes_OtherCharges">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Wrap="false" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblOtherCharge" runat="server" Text='<%# Eval("ContractNotes_OtherCharges")%>'
                                                    CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblOtherCharge_sum" runat="server" ForeColor="white"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" VerticalAlign="Top" Font-Bold="True" ForeColor="White" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Srv Tax & Cess" SortExpression="TotalTax_Sorting">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Wrap="false" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalTax" runat="server" Text='<%# Eval("TotalTax")%>' CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotal_sum" runat="server" Text='<%# Eval("Total")%>' ForeColor="white"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" VerticalAlign="Top" Font-Bold="True" ForeColor="White" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Net Charges" SortExpression="ContractNotes_NetAmount_Sorting">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Wrap="false" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblnetamount" runat="server" Text='<%# Eval("ContractNotes_NetAmount")%>'
                                                    CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblnetamount_sum" runat="server" Text='<%# Eval("netamount")%>' ForeColor="white"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" VerticalAlign="Top" Font-Bold="True" ForeColor="White" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Round Amount" SortExpression="ContractNotes_RoundAmount">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Wrap="false" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblRoundAmount" runat="server" Text='<%# Eval("ContractNotes_RoundAmount")%>'
                                                    CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblRoundamount_sum" runat="server" ForeColor="white"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" VerticalAlign="Top" Font-Bold="True" ForeColor="White" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Remarks" SortExpression="ContractNotes_Remarks">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Wrap="false" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("ContractNotes_Remarks")%>'
                                                    CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cancel Reason" SortExpression="ContractNotes_CancellationReason">
                                            <ItemStyle Wrap="false" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Wrap="false" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCancelReason" runat="server" Text='<%# Eval("ContractNotes_CancellationReason")%>'
                                                    CssClass="gridstyleheight1"></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="10">
                                                    <asp:LinkButton ID="FirstPage" runat="server" Font-Bold="true" CommandName="First"
                                                        OnCommand="NavigationLink_Click" Text="[First Page]"> </asp:LinkButton>
                                                    <asp:LinkButton ID="PreviousPage" runat="server" Font-Bold="true" CommandName="Prev"
                                                        OnCommand="NavigationLink_Click" Text="[Previous Page]">  </asp:LinkButton>
                                                    <asp:LinkButton ID="NextPage" runat="server" Font-Bold="true" CommandName="Next"
                                                        OnCommand="NavigationLink_Click" Text="[Next Page]">
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="LastPage" runat="server" Font-Bold="true" CommandName="Last"
                                                        OnCommand="NavigationLink_Click" Text="[Last Page]">
                                                    </asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </PagerTemplate>
                                    <RowStyle BackColor="White" ForeColor="#330099" BorderColor="#BFD3EE" BorderStyle="Double"
                                        BorderWidth="1px"></RowStyle>
                                    <EditRowStyle BackColor="#E59930"></EditRowStyle>
                                    <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
                                    <PagerStyle ForeColor="White" HorizontalAlign="Center"></PagerStyle>
                                    <HeaderStyle ForeColor="Black" BorderWidth="1px" CssClass="EHEADER" BorderColor="AliceBlue"
                                        Font-Bold="False"></HeaderStyle>
                                </asp:GridView>
                            </div>
                            <asp:HiddenField ID="CurrentPage" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="TotalPages" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="TotalClient" runat="server" />

                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btn_show" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hidScreenWd" runat="server" />
    </div>
</asp:Content>

