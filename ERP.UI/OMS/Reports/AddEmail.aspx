<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_AddEmail" CodeBehind="AddEmail.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">


        function MailsendT() {
            alert("Mail Sent Successfully");
        }
        function MailsendF() {
            alert("Error on sending!Try again..");
        }
        function callAjax(obj1, obj2, obj3) {
            var combo = document.getElementById("cmbContactType");
            var set_value = combo.value
            if (set_value == '16') {
                ajax_showOptions(obj1, 'GetLeadId', obj3, set_value)
            }
            else {
                ajax_showOptions(obj1, obj2, obj3, set_value)
            }
        }
        FieldName = 'cmbTemplate';

        function AddProductCategory() {
            var email = document.getElementById("txtUserId");
            if (email.value == '') {
                alert("Please select email from list");
                return;
            }
            GridCategory.PerformCallback();
        }


        //THIS IS FOR EMAIL

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


        function callAjax1(obj1, obj2, obj3) {
            var combo = document.getElementById("cmbsearchOption");
            var set_value = combo.value
            if (set_value == '16') {
                ajax_showOptions(obj1, 'GetLeadId', obj3, set_value)
            }
            else {
                ajax_showOptions(obj1, obj2, obj3, set_value)
            }
        }

        function clientselectionfinal() {
            var listBoxSubs = document.getElementById('lstSlection');

            var cmb = document.getElementById('cmbsearchOption');
            alert(cmb.value);
            alert(listBoxSubs.value);
            var listIDs = '';
            var i;
            if (listBoxSubs.length > 0) {
                alert(listBoxSubs.length);
                for (i = 0; i < listBoxSubs.length; i++) {
                    if (listIDs == '')
                        listIDs = listBoxSubs.options[i].value + ';' + listBoxSubs.options[i].text;
                    else
                        listIDs += ',' + listBoxSubs.options[i].value + ';' + listBoxSubs.options[i].text;

                }
                var sendData = cmb.value + '~' + listIDs;
                alert(sendData);
                CallServer(sendData, "");


            }
            //	            var i;
            //                for(i=listBoxSubs.options.length-1;i>=0;i--)
            //                {
            //                    listBoxSubs.remove(i);
            //                }
            //              
            //                document.getElementById('showFilter').style.display='none';
            //                document.getElementById('TdFilter').style.display='none';
            //                document.getElementById('btn_show').disabled=false;
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

        function ReceiveServerData(rValue) {
            var Data = rValue.split('~');
            if (Data[0] == 'Clients') {
            }
        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td>User Type:
                        <asp:DropDownList ID="cmbContactType" runat="server" Width="150px" Font-Size="12px">
                        </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Select User :
                        <asp:TextBox ID="txtUserId" runat="server" Font-Size="12px" Width="400px" Height="18px"></asp:TextBox><asp:TextBox
                            ID="txtUserId_hidden" runat="server" Width="0px"></asp:TextBox>
                    <input id="Button1" type="button" runat="server" value="Add" onclick="AddProductCategory()" />
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:aspxgridview id="GridCategory" clientinstancename="GridCategory" runat="server"
                        width="100%" oncustomcallback="GridCategory_CustomCallback">
                            <Styles>
                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                </Header>
                                <LoadingPanel ImageSpacing="10px">
                                </LoadingPanel>
                            </Styles>
                            <Settings ShowGroupButtons="False" ShowStatusBar="Hidden" />
                            <SettingsBehavior AllowDragDrop="False" AllowGroup="False" AllowSort="False" />
                            <SettingsPager Mode="ShowAllRecords">
                            </SettingsPager>
                        </dxe:aspxgridview>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Button3" runat="server" Text="Send Email" CssClass="btnUpdate" OnClick="Button3_Click" />
                    <asp:Button ID="Button2" runat="server" Text="Cancel" CssClass="btnUpdate" OnClick="Button2_Click" />
                </td>
            </tr>
            <tr>
                <td style="display: none;">
                    <div id="displayALLCLIENT" runat="server">
                    </div>
                </td>
            </tr>
        </table>
        <table width="100%" id="showFilter">
            <tr>
                <td class="gridcellleft" style="vertical-align: top; text-align: right" id="TdFilter">
                    <span id="spanall">
                        <asp:DropDownList ID="cmbsearchOption" runat="server" Width="150px" Font-Size="12px">
                        </asp:DropDownList>
                        <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px"></asp:TextBox></span>
                    <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                        style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                            style="color: #009900; font-size: 8pt;"> </span>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; vertical-align: top; height: 134px;">
                    <table cellpadding="0" cellspacing="0">
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
            <tr style="display: none">
                <td style="height: 23px">
                    <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                    <asp:TextBox ID="dtFrom_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                    <asp:TextBox ID="dtTo_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
