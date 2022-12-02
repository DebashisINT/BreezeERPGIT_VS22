<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    Inherits="ERP.OMS.Management.management_JournalRegister" CodeBehind="JournalRegister.aspx.cs" %>

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
        function clientselectionfinal() {
       var listBoxSubs = document.getElementById('lstSuscriptions');
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
       document.getElementById('TdFilter').style.display = 'none';
   }
               function btnRemovefromsubscriptionlist_click() {

                   var listBox = document.getElementById('lstSuscriptions');
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
                   var userid = document.getElementById('txtsubscriptionID');
                   if (userid.value != '') {
                       var ids = document.getElementById('txtsubscriptionID_hidden');
                       var listBox = document.getElementById('lstSuscriptions');
                       var tLength = listBox.length;
                       //alert(tLength);

                       var no = new Option();
                       no.value = ids.value;
                       no.text = userid.value;
                       listBox[tLength] = no;
                       var recipient = document.getElementById('txtsubscriptionID');
                       recipient.value = '';
                   }
                   else
                       alert('Please search name and then Add!')
                   var s = document.getElementById('txtsubscriptionID');
                   s.focus();
                   s.select();
               }
               function ForFilter(objSelected, objType) {
                   if (objSelected == 'A')
                       document.getElementById('TdFilter').style.display = 'none';
                   else {
                       document.getElementById('TdFilter').style.display = 'inline';
                       if (objType == "Seg")
                           document.getElementById('cmbsearchOption').value = 'Segment';
                       else if (objType == "Branch")
                           document.getElementById('cmbsearchOption').value = 'Branch';
                       else if (objType == "EUser")
                           document.getElementById('cmbsearchOption').value = 'EntryUser';
                   }
               }
               function Page_Load() {
                   document.getElementById('TdFilter').style.display = 'none';
                   document.getElementById('TrFilter').style.display = 'none';
                   document.getElementById('TdSpecific').style.display = 'none';

                   height();
               }
               function ForSpecific(obj) {
                   if (obj == 'A')
                       document.getElementById('TdSpecific').style.display = 'none';
                   else
                       document.getElementById('TdSpecific').style.display = 'inline';
               }
               function height() {
                   if (document.body.scrollHeight >= 500) {
                       window.frameElement.height = document.body.scrollHeight;
                   }
                   else {
                       window.frameElement.height = '500';
                   }
                   window.frameElement.width = document.body.scrollWidth;
               }
               function ShowAjax(obj1, obj2, obj3) {
                   var obj4 = document.getElementById("cmbsearchOption");
                   ajax_showOptions(obj1, obj2, obj3, obj4.value, 'Sub');
               }
               function HeightCall() {
                   document.getElementById('TrSelect').style.display = 'none';
                   document.getElementById('TrFilter').style.display = 'inline';
                   height();
               }
               function ForFilter1() {
                   document.getElementById('TrSelect').style.display = 'inline';
                   height();
               }
               function selecttion() {
                   var combo = document.getElementById('ddlExport');
                   combo.value = 'Ex';
               }
               function DateChangeForFrom() {
                   //var datePost=(dtFrom.GetDate().getMonth()+2)+'-'+dtFrom.GetDate().getDate()+'-'+dtFrom.GetDate().getYear();
                   var sessionVal = "<%=Session["LastFinYear"]%>";
            var objsession = sessionVal.split('-');
            var MonthDate = dtFrom.GetDate().getMonth() + 1;
            var DayDate = dtFrom.GetDate().getDate();
            var YearDate = dtFrom.GetDate().getYear();
            if (YearDate >= objsession[0]) {
                if (MonthDate < 4 && YearDate == objsession[0]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (4 + '-' + 1 + '-' + objsession[0]);
                    dtFrom.SetDate(new Date(datePost));
                }
                else if (MonthDate > 3 && YearDate == objsession[1]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (4 + '-' + 1 + '-' + objsession[0]);
                    dtFrom.SetDate(new Date(datePost));
                }
                else if (YearDate != objsession[0] && YearDate != objsession[1]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (4 + '-' + 1 + '-' + objsession[0]);
                    dtFrom.SetDate(new Date(datePost));
                }
            }
            else {
                alert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (4 + '-' + 1 + '-' + objsession[0]);
                dtFrom.SetDate(new Date(datePost));
            }
        }
        function DateChangeForTo() {
            var sessionVal = "<%=Session["LastFinYear"]%>";
            var objsession = sessionVal.split('-');
            var MonthDate = dtTo.GetDate().getMonth() + 1;
            var DayDate = dtTo.GetDate().getDate();
            var YearDate = dtTo.GetDate().getYear();

            if (YearDate <= objsession[1]) {
                if (MonthDate < 4 && YearDate == objsession[0]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (3 + '-' + 31 + '-' + objsession[1]);
                    dtTo.SetDate(new Date(datePost));
                }
                else if (MonthDate > 3 && YearDate == objsession[1]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (3 + '-' + 31 + '-' + objsession[1]);
                    dtTo.SetDate(new Date(datePost));
                }
                else if (YearDate != objsession[0] && YearDate != objsession[1]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (3 + '-' + 31 + '-' + objsession[1]);
                    dtTo.SetDate(new Date(datePost));
                }
            }
            else {
                alert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (3 + '-' + 31 + '-' + objsession[1]);
                dtTo.SetDate(new Date(datePost));
            }
        }
        FieldName = 'lstSuscriptions';
        </script>

    <script type="text/ecmascript">
        function ReceiveServerData(rValue) {

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
    <table class="TableMain100">
        <tr>
            <td class="EHEADER" style="text-align: center;">
                <strong><span style="color: #000099">Journal Register</span></strong>
            </td>
        </tr>
    </table>
    <table class="TableMain100">
        <tr id="TrSelect">
            <td>
                <table cellspacing="1" cellpadding="2" style=" border: solid 1px  #ffffff"
                    >
                    <tr>
                        <td class="gridcellleft">For A Date Range :
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtFrom" runat="server" ClientInstanceName="dtFrom" EditFormat="Custom"
                                            UseMaskBehavior="True" Font-Size="12px" Width="108px">
                                            <DropDownButton Text="From">
                                            </DropDownButton>
                                            <ClientSideEvents ValueChanged="function(s,e){DateChangeForFrom();}" />
                                        </dxe:ASPxDateEdit>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtTo" runat="server" ClientInstanceName="dtTo" EditFormat="Custom"
                                            UseMaskBehavior="True" Font-Size="12px" Width="98px">
                                            <DropDownButton Text="To">
                                            </DropDownButton>
                                            <ClientSideEvents ValueChanged="function(s,e){DateChangeForTo();}" />
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkIgnoreSystem" runat="server" Checked="true" />
                                    </td>
                                    <td class="gridcellleft">Ignore system generated accounting JVs
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="gridcellleft" style="width: 77px">Voucher Type
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="radAll" Checked="true" runat="server" GroupName="a" onclick="ForSpecific('A')" />
                                    </td>
                                    <td>All
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="radSpecific" runat="server" GroupName="a" onclick="ForSpecific('B')" />
                                    </td>
                                    <td>Specific
                                    </td>
                                    <td id="TdSpecific">
                                        <dxe:ASPxTextBox ID="txtAccountCode" ClientInstanceName="txtAccountCode"
                                            runat="server" Width="50px" MaxLength="2">
                                            <%--<ValidationSettings>
                                            <RequiredField IsRequired="True" ErrorText="Select Prefix" />
                                        </ValidationSettings>--%>
                                            <%--<ClientSideEvents KeyPress="function(s,e){window.setTimeout('updateEditorText()', 10);}" />--%>
                                        </dxe:ASPxTextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="gridcellleft">Segment :
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RadSegmentAll" runat="server" Checked="True" onclick="ForFilter('A','Seg');"
                                            GroupName="c" />
                                    </td>
                                    <td>All
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="RadSegmentSelected" runat="server" onclick="ForFilter('S','Seg');"
                                            GroupName="c" />
                                    </td>
                                    <td>Selected
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="gridcellleft">Branch :
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RadBranchAll" runat="server" onclick="ForFilter('A','Branch');"
                                            Checked="True" GroupName="d" />
                                    </td>
                                    <td>All
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="RadBranchSelected" runat="server" onclick="ForFilter('S','Branch');"
                                            GroupName="d" />
                                    </td>
                                    <td>Selected
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="gridcellleft">Entry User :
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RadEntryAll" runat="server" Checked="True" onclick="ForFilter('A','EUser');"
                                            GroupName="e" />
                                    </td>
                                    <td>All
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="RadEntrySelected" runat="server" onclick="ForFilter('S','EUser');"
                                            GroupName="e" />
                                    </td>
                                    <td>Selected
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="vertical-align: top;" id="TdFilter">
                <table>
                    <tr>
                        <td class="gridcellleft" style="vertical-align: top; text-align: right">
                            <asp:TextBox ID="txtsubscriptionID" runat="server" Font-Size="12px" onkeyup="ShowAjax(this,'SearchMainAccountBranchSegment',event);"
                                Width="200px"></asp:TextBox>
                            <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                Enabled="false">
                                <asp:ListItem>Segment</asp:ListItem>
                                <asp:ListItem>Branch</asp:ListItem>
                                <asp:ListItem>EntryUser</asp:ListItem>
                            </asp:DropDownList>
                            <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                    style="color: #009900; font-size: 8pt;"> </span>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 5px;">
                            <asp:ListBox ID="lstSuscriptions" runat="server" Font-Size="12px" Height="90px" Width="270px"></asp:ListBox>
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
                                <tr>
                                    <td colspan="2">
                                        <asp:HiddenField ID="txtsubscriptionID_hidden" runat="server" />
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
                            <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="btnUpdate" Height="26px" OnClientClick="selecttion()"
                                OnClick="btnShow_Click" Width="96px" />
                        </td>
                        <td style="text-align: left">
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                <ProgressTemplate>
                                    <div id='Div2' style='position: absolute; font-family: arial; font-size: 30; background-color: white; layer-background-color: white;'>
                                        <table border='1' cellpadding='0' cellspacing='0' bordercolor='#C0D6E4'>
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
                        <td style="text-align: right" id="TrFilter">
                            <span style="font-weight: bold; color: Blue; cursor: pointer" onclick="javascript:ForFilter1();">Filter</span>
                            <asp:DropDownList ID="ddlExport" runat="server" AutoPostBack="True" Font-Size="12px" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged">
                                <asp:ListItem Value="Ex" Selected="True">Export</asp:ListItem>
                                <asp:ListItem Value="E">Excel</asp:ListItem>
                                <asp:ListItem Value="P">PDF</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
            <td></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="DivShow" runat="server">
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>

