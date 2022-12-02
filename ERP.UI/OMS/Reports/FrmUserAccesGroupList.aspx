<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_FrmUserAccesGroupList" Codebehind="FrmUserAccesGroupList.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajaxList_rootfile.js"></script>

    <%--    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>--%>
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript">
        function Page_Load()///Call Into Page Load
        {
            divAsOnDate.innerText = "User:";
            divAsOnDate1.innerText = "User:";
            document.getElementById('ShowTable').style.display = 'none';
          //  height();

        }
        function callheight(obj) {
           // height();
           // parent.CallMessage();
        }
        


        function btnAddEmailtolist_click() {


            var cmb = document.getElementById('cmbsearch');

            var userid = document.getElementById('txtSelectID');
            if (userid.value != '') {
                var ids = document.getElementById('txtSelectID_hidden');
                var listBox = document.getElementById('SelectionList');
                var tLength = listBox.length;


                var no = new Option();
                no.value = ids.value;
                no.text = userid.value;
                listBox[tLength] = no;
                var recipient = document.getElementById('txtSelectID');
                recipient.value = '';
            }
            else
                alert('Please search name and then Add!')
            var s = document.getElementById('txtSelectID');
            s.focus();
            s.select();

        }


        function callAjax1(obj1, obj2, obj3) {
            document.getElementById('SelectionList').style.display = 'none';
            var combo = document.getElementById("cmbGroup");
            var set_value = combo.value;
            var obj4 = 'Main';
            ajax_showOptions(obj1, obj2, obj3, set_value, obj4)


        }

        function clientselection() {
            var listBoxSubs = document.getElementById('SelectionList');

            // var cmb=document.getElementById('cmbsearch');
            var cmb = document.getElementById('cmbGroup');

            var listIDs = '';
            var i;

            if (listBoxSubs.length > 0) {

                for (i = 0; i < listBoxSubs.length; i++) {

                    if (listIDs == '')

                        listIDs = listBoxSubs.options[i].value + ';' + listBoxSubs.options[i].text;
                    else
                        listIDs += ',' + listBoxSubs.options[i].value + ';' + listBoxSubs.options[i].text;

                }
                //sendData = cmb.value + '~' + listIDs;
                var sendData;
                if (cmb.value == '0') {
                    sendData = 'User' + '~' + listIDs;
                }
                else {
                    sendData = 'Group' + '~' + listIDs;
                }

                CallServer1(sendData, "");

                document.getElementById('ShowTable').style.display = 'none';

            }
            else {
                alert("Please select from list.")
            }

            var i;
            for (i = listBoxSubs.options.length - 1; i >= 0; i--) {
                listBoxSubs.remove(i);
            }
            window.frameElement.height = document.body.scrollHeight;
        }

        function btnRemoveEmailFromlist_click() {

            var listBox = document.getElementById('SelectionList');
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

        function ReceiveSvrData(rValue) {
            var Data = rValue.split('~');
            if (Data[0] == 'Clients') {
            }
        }



        function keyVal(obj) {
            document.getElementById('SelectionList').style.display = 'inline';

        }

        function SelectUserClient(obj) {
            if (obj == 'Client') {


                document.getElementById('ShowTable').style.display = 'none';


            }
            else if (obj == 'User') {
                document.getElementById('ShowTable').style.display = 'inline';


            }


        }


        function ShowSelect(obj) {
            if (obj == '0') {
                divAsOnDate.innerText = "User :";
                divAsOnDate1.innerText = "User :";
            }
            else if (obj == '1') {
                divAsOnDate.innerText = "Access Group:";
                divAsOnDate1.innerText = "Access Group:";
            }
        }

        FieldName = 'SelectionList';

    </script>

    <script type="text/ecmascript">
        function ReceiveServerData(rValue) {
            var j = rValue.split('~');

            if (j[0] == 'Group') {

                // groupvalue=j[1];
                document.getElementById('HDNValue').value = j[1];

            }
            if (j[0] == 'User') {
                // groupvalue=j[1];
                document.getElementById('HDNValue').value = j[1];
            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
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
   </script> 
            <table class="TableMain100">
                <tr>
                    <td class="EHEADER" style="text-align: center; height: 20px;">
                        <strong><span style="color: #000099">User Access Group Members</span></strong>
                    </td>
                </tr>
                <tr >
                    <td>
                        <table class="TableMain100">
                            <tr>
                                <td valign="top">
                                    <table cellspacing="1" cellpadding="2" style="background-color: #B7CEEC; border: solid 1px  #ffffff"
                                        border="1">
                                        <tr>
                                            <td>
                                                View By:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbGroup" runat="server" Width="250px" onchange="ShowSelect(this.value)">
                                                    <asp:ListItem Value="0">User Wise</asp:ListItem>
                                                    <asp:ListItem Value="1"> Access Group Wise</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <div id="divAsOnDate">
                                                </div>
                                            </td>
                                            <td>
                                                <table id="ShowSelectUser">
                                                    <tr>
                                                        <td valign="top">
                                                            <asp:RadioButton ID="rbOnlyClient" runat="server" Checked="true" GroupName="h" />
                                                            ALL
                                                        </td>
                                                        <td valign="top">
                                                            <asp:RadioButton ID="rbClientUser" runat="server" GroupName="h" /><span style="font-size: 8pt;
                                                                color: #009900"> </span>Selected
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                           
                                            <td >
                                                <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" OnClick="Button1_Click" 
                                                    Text="Show" /></td>
                                                    <td>
                                                     <asp:Button ID="btnExoprt" runat="server" CssClass="btn btn-primary" OnClick="btnExoprt_Click"
                                                    Text="Export To Excel" />
                                                    </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <table id="ShowTable">
                                        <tr style="display: none;">
                                            <td width="70px" style="text-align: left;">
                                                Type:</td>
                                            <td class="gridcellleft" style="vertical-align: top; text-align: left" id="Td1">
                                                <span id="span1">
                                                    <asp:DropDownList ID="cmbsearch" runat="server" Width="150px" Font-Size="12px">
                                                    </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left;">
                                                <div id="divAsOnDate1">
                                                </div>
                                            </td>
                                            <td class="gridcellleft" style="vertical-align: top; text-align: left" id="TdFilter1">
                                                <span id="spanal2">
                                                    <asp:TextBox ID="txtSelectID" runat="server" Font-Size="12px" Width="285px"></asp:TextBox></span>
                                                <a id="A3" href="javascript:void(0);" onclick="btnAddEmailtolist_click()"><span style="color: #009900;
                                                    text-decoration: underline; font-size: 8pt;">Add to List</span></a><span style="color: #009900;
                                                        font-size: 8pt;"> </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="70px" style="text-align: left;">
                                            </td>
                                            <td style="text-align: left; vertical-align: top; height: 134px;">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            &nbsp;&nbsp;<asp:ListBox ID="SelectionList" runat="server" Font-Size="12px" Height="90px"
                                                                Width="290px"></asp:ListBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: left">
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <a id="AA2" href="javascript:void(0);" onclick="clientselection()"><span style="color: #000099;
                                                                            text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <a id="AA1" href="javascript:void(0);" onclick="btnRemoveEmailFromlist_click()"><span
                                                                            style="color: #cc3300; text-decoration: underline; font-size: 8pt;">Remove</span></a>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="display: none">
                                            <td width="70px" style="text-align: left;">
                                            </td>
                                            <td style="height: 23px">
                                                <asp:TextBox ID="txtSelectID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                                <asp:TextBox ID="dtFrom_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                                <asp:TextBox ID="dtTo_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
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
                                <div id='Div1' style='position: absolute; font-family: arial; font-size: 30; left: 100;
                                    top: 8; background-color: white; layer-background-color: white; height: 80; width: 150;'>
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
                </tr>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                           <ContentTemplate>
                           <div id="DisplayReport" runat="server">
                        </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <asp:HiddenField ID="HDNValue" runat="server" />
                        
                    </td>
                </tr>
            </table>
        </div>
</asp:Content>
