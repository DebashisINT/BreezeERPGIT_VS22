<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_utilities_frmmessage" CodeBehind="frmmessage.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .gridcellright {
            margin-right: 10px;
        }
    </style>
    <%--    <script language="javascript" type="text/javascript">
    function SignOff()
    {
        window.parent.SignOff();
    }
    function height()
    {
        if(document.body.scrollHeight>=500)
            window.frameElement.height = document.body.scrollHeight;
        else
            window.frameElement.height = '500px';
        window.frameElement.Width = document.body.scrollWidth;
    }
    </script>--%>

    <script language="javascript" type="text/javascript">
        FieldName = 'btnFilter';
        //function is called on changing Selection
        function OnGridSelectionChanged() {
            //        var noofrow=grid.GetSelectedRowCount().toString();
            //        alert(noofrow);
            grid.GetSelectedFieldValues('Mid', OnGridSelectionComplete);
        }
        function OnGridSelectionComplete(values) {
            counter = 'n';
            for (var i = 0; i < values.length; i++) {
                if (counter != 'n')
                    counter += ',' + values[i];
                else
                    counter = values[i];
            }
            //alert(counter+'test');
        }
        //________End here on changing focused row
        function frmOpenNewWindow1(location, v_height, v_weight) {

            var cmb = document.getElementById("cmbTemplate");
            if (cmb.value != '') {
                var x = (screen.availHeight - v_height) / 2;
                var y = (screen.availWidth - v_weight) / 2
                window.open(location, "template", "height=" + v_height + ",width=" + v_weight + ",top=" + x + ",left=" + y + ",location=no,directories=no,menubar=no,toolbar=no,status=yes,scrollbars=yes,resizable=no,dependent=no'");
            }
        }
    </script>

    <script type="text/ecmascript">
        function PageLoadFirst() {
            document.getElementById("FormCreate").style.display = 'none';
            document.getElementById("FormFilter").style.display = 'none';
            document.getElementById("FormGrid").style.display = 'inline';
            document.getElementById("FormOutbox").style.display = 'none';
            counter = 'n';
            //--FieldName = "drp_accesslevel";
            document.getElementById("txtRecipients_hidden").style.display = 'none';
            document.getElementById("FormHistory").style.display = 'none';
        }

        function HistoryExport() {
            document.getElementById("FormFilter").style.display = 'none';
            document.getElementById("FormCreate").style.display = 'none';
            document.getElementById("FormGrid").style.display = 'none';
            document.getElementById("FormOutbox").style.display = 'none';
            document.getElementById("txtUserName_hidden").style.visibility = 'hidden';
        }
        function btnRead_click() {
            //alert(counter);
            if (counter != 'n') {
                document.getElementById("FormCreate").style.display = 'none';
                document.getElementById("FormFilter").style.display = 'none';
                var ReadIDs = 'read~' + counter;
                CallServer(ReadIDs, "");
            }
            else
                alert('Please Select a message!');

        }
        function btnReply_Click() {
            if (counter != 'n') {
                var ReadIDs = 'reply~' + counter;
                CallServer(ReadIDs, "");
            }
            else
                alert('Please Select a message!');
        }
        function btnDelete_click() {
            if (counter != 'n') {
                var ReadIDs = 'delete~' + counter;
                CallServer(ReadIDs, "");
            }
            else
                alert('Please Select a message!');
        }
        function btnInbox_Click() {
            grid.PerformCallback('inBox');
            document.getElementById("FormFilter").style.display = 'inline';
            document.getElementById("FormCreate").style.display = 'none';
            document.getElementById("FormGrid").style.display = 'inline';
            document.getElementById("FormOutbox").style.display = 'none';
        }
        function btnOutbox_Click() {
            document.getElementById("FormGrid").style.display = 'none';
            document.getElementById("FormOutbox").style.display = 'inline';
            document.getElementById("FormFilter").style.display = 'none';
            gridoutbox.PerformCallback();
        }
        function btnFilter_click() {
            var StartDate = document.getElementById("txtStart");
            var EndDate = document.getElementById("txtEnd");
            if (StartDate.value != "") {
                if (EndDate.value != "") {
                    grid.PerformCallback('filter');
                }
                else
                    alert('Please Enter End Date!');
            }
            else
                alert('Please Enter Start Date!');
        }
        function btnCancel_click() {
            document.getElementById("FormCreate").style.display = 'none';
        }
        function btnSave_click() {
            var replyText = document.getElementById("txtContent");
            //alert(replyText.value);
            var txtUserNameId = document.getElementById("txtUser_hidden");
            //alert(txtUserNameId.value);
            var userIDname = txtUserNameId.value.split(',');

            var ListBox = document.getElementById('ListBoxUserAll');
            txtUserNameId = '';

            for (var i = 0; i < ListBox.options.length; i++) {
                if (i == 0) {
                    txtUserNameId = ListBox.options[i].value;
                }
                else {
                    txtUserNameId = txtUserNameId + ',' + ListBox.options[i].value;
                }

            }

            if (replyText.value != '') {
                var ReadIDs = "create~" + txtUserNameId + '~' + replyText.value;
                //alert(ReadIDs);
                CallServer(ReadIDs, "");
            }
            else
                alert('Please fill content you want to send message!');


            //        if(userIDname[0] != '' )
            //        {
            //            if(replyText.value != '')
            //            {
            //                var ReadIDs = "create~" + userIDname[1] + '~' + replyText.value;
            //                //alert(ReadIDs);
            //                CallServer(ReadIDs,"");
            //            }
            //            else
            //                alert('Please fill content you want to send message!');
            //        }
            //        else
            //            alert('Please fill Name to whoom you want to send message!');
        }
        function btnSend_click() {
            var replyText = document.getElementById("txtContent");
            var ReadIDs = "send~" + counter + '~' + replyText.value;
            //alert(ReadIDs);
            CallServer(ReadIDs, "");
        }
        function btnCreate_Click() {
            document.getElementById("FormHistory").style.display = 'none';
            document.getElementById("TDtemplate").style.display = 'inline';
            document.getElementById("trRecipient").style.display = '';
            document.getElementById("FormCreate").style.display = 'inline';
            document.getElementById("CreateUserName").style.display = 'none';
            document.getElementById("btnSavedata").style.display = 'inline';
            document.getElementById("TRreplied").style.display = 'none';
            document.getElementById("FormFilter").style.display = 'none';
            document.getElementById("ReplyUserName").style.display = 'none';
            document.getElementById("btnReplydata").style.display = 'none';

            document.getElementById("TDshow").style.display = 'inline';
            var replyText = document.getElementById("txtContent");
            replyText.value = '';
            replyText = document.getElementById("txtUser");
            replyText.value = '';
            document.getElementById("txtUser_hidden").style.visibility = 'hidden'
            //-- document.getElementById("TRaddTemplate").style.display = 'none';
            document.getElementById("txtUser").disabled = false;
        }
        //    function btnShowTemplate_click()
        //    {
        //        document.getElementById("TDtemplate").style.display = 'inline';
        //        document.getElementById("TDshow").style.display = 'none';
        //    }
        function btnHideTemplate_click() {
            document.getElementById("TDtemplate").style.display = 'none';
            document.getElementById("TDshow").style.display = 'inline';

            //-- document.getElementById("TRaddTemplate").style.display = 'none';
            document.getElementById("FormCreate").style.display = 'inline';

            document.getElementById("CreateUserName").style.display = 'inline';
            document.getElementById("ReplyUserName").style.display = 'none';
            document.getElementById("TRcontent").style.display = 'inline';
            document.getElementById("TRbutton").style.display = 'inline';
        }
        function btnAddTemplate_click() {
            //--  FieldName = "drp_accesslevel";
            //-- document.getElementById("TRaddTemplate").style.display = 'inline';

            document.getElementById("CreateUserName").style.display = 'none';
            document.getElementById("ReplyUserName").style.display = 'none';
            document.getElementById("TRcontent").style.display = 'none';
            document.getElementById("TRbutton").style.display = 'none';
            document.getElementById("TRreplied").style.display = 'none';
            var listBox = document.getElementById("ListBoxUserAll");
            listBox.length = 0;
        }
        function btnAddToList_click() {
            var userid = document.getElementById("txtRecipients_hidden");
            var ids = userid.value.split(',');
            var listBox = document.getElementById("ListBoxUserAll");
            var tLength = listBox.length;



            //alert(tLength);

            var no = new Option();
            no.value = ids[1];
            no.text = ids[0];
            listBox[tLength] = no;

            document.getElementById("txtRecipients_hidden").value = '';
            document.getElementById("txtRecipients").value = '';

        }
        function btnRemoveToList_click() {
            var listBox = document.getElementById("ListBoxUserAll");
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
            //var listBox1 = document.getElementById("ListBoxUserAll");
            listBox.length = 0;
            for (i = 0; i < j; i++) {
                var no = new Option();
                no.value = arrLookup[arrTbox[i]];
                no.text = arrTbox[i];
                listBox[i] = no;
            }
        }
        function btnHistory_Click() {
            document.getElementById("FormHistory").style.display = 'inline';
            document.getElementById("TRHistory").style.display = 'none';
            document.getElementById("FormFilter").style.display = 'none';
            document.getElementById("FormCreate").style.display = 'none';
            document.getElementById("FormGrid").style.display = 'none';
            document.getElementById("FormOutbox").style.display = 'none';
            document.getElementById("txtUserName_hidden").style.visibility = 'hidden';
        }
        function btnGetHistory_click() {
            document.getElementById("TRHistory").style.display = 'inline';
            document.getElementById("TRHistory").style.display = 'table-row';

            var hiddenfield = document.getElementById("txtUserName_hidden");
            if (hiddenfield.value != '') {
                var id = hiddenfield.value.split(',');
                //alert(id[1]);
            }
            else {
                alert('Please Select Name!');
                return false;
            }
            var StartDate = document.getElementById("txtFromDate");
            var EndDate = document.getElementById("txtToDate");

            if (StartDate.value != "") {
                if (EndDate.value != "") {
                    gridhistory.PerformCallback('GetHistory');
                }
                else {
                    alert('Please Enter End Date!');
                    return false;
                }
            }
            else {
                alert('Please Enter Start Date!');
                return false;

            }



        }
        //    function btnSaveTemp_click()
        //    {
        //        //-- var dhortDescription = document.getElementById("txtShortDescription");
        //         if(dhortDescription.value == '')
        //         {
        //            alert('Please Fill ShortDescription!');
        //            return false;
        //         }
        //    //--     var message = document.getElementById("txtMessage");
        //         if(message.value == '')
        //         {
        //            alert('Please Fill Message!');
        //            return false;
        //         }
        //    //--     var Accslevel = document.getElementById("drp_accesslevel");
        //         
        //         var listBox = document.getElementById("ListBoxUserAll");
        //         var tLength = listBox.length;
        //         var userlist = 0;
        //         var i = 0;
        //         if(tLength > 0)
        //         {
        //            for (i = 0; i < tLength; i++) 
        //            {
        //                if( userlist == 0)
        //                    userlist = listBox.options[i].value;
        //                else
        //                    userlist += ',' + listBox.options[i].value;
        //            }
        //         }
        //         
        //        var ReadIDs = "saveTemp~" + dhortDescription.value + '~' + userlist + '~' + Accslevel.value + '~' + message.value;
        //        alert(ReadIDs);
        //        CallServer(ReadIDs,"");
        //         
        //    }
        function Export_change() {

            //var comboexport = document.getElementById("ASPxComboBox1");
            var ReadIDs = 'Export~';// + gridHistoryexport.value;
            //alert(ReadIDs);
            CallServer(ReadIDs, "");
        }
        //    function btnCancelTemp_click()
        //    {
        //     //--   document.getElementById("TRaddTemplate").style.display = 'none';
        //        document.getElementById("FormCreate").style.display = 'inline';
        //        document.getElementById("TRreplied").style.display = 'inline';
        //        document.getElementById("CreateUserName").style.display = 'none';
        //        document.getElementById("ReplyUserName").style.display = 'inline';
        //        document.getElementById("TRcontent").style.display = 'inline';
        //        document.getElementById("TRbutton").style.display = 'inline';
        //    }
        function ReceiveServerData(rValue) {
            var DATA = rValue.split('~');

            if (DATA[0] == "BindCombo") {
                if (DATA[1] == "Y") {
                    document.getElementById("txtContent").value = DATA[2];
                    var DtUser = DATA[3].split(';');
                    var len = DtUser.length;
                    //document.getElementById("ListBoxUserAll").items.clear();
                    var listBoxSubs = document.getElementById("ListBoxUserAll");
                    for (i = listBoxSubs.options.length - 1; i >= 0; i--) {
                        listBoxSubs.remove(i);
                    }
                    for (var i = 0; i < len; i++) {
                        var ids = DtUser[i].split(',');
                        var listBox = document.getElementById("ListBoxUserAll");
                        var tLength = listBox.length;
                        var no = new Option();
                        no.value = ids[1];
                        no.text = ids[0];
                        listBox[tLength] = no;
                    }

                }
                else {
                    document.getElementById("txtContent").value = '';
                    var listBoxSubs = document.getElementById("ListBoxUserAll");
                    for (i = listBoxSubs.options.length - 1; i >= 0; i--) {
                        listBoxSubs.remove(i);
                    }

                }
            }


            if (DATA[0] == "read") {
                if (DATA[1] == "Y") {
                    alert('Read Successfully!');
                    grid.PerformCallback('read');
                    grid.UnselectAllRowsOnPage();
                }
                else if (DATA[1] == "S")
                    alert('Please Select a message!');
            }
            if (DATA[0] == "reply") {
                if (DATA[1] != "M") {
                    if (DATA[1] != "") {
                        document.getElementById("FormHistory").style.display = 'none';
                        document.getElementById("TDtemplate").style.display = 'inline';
                        document.getElementById("trRecipient").style.display = 'none';
                        document.getElementById("btnReplydata").style.display = 'inline';
                        document.getElementById("btnSavedata").style.display = 'none';
                        var replyText = document.getElementById("txtContent");
                        replyText.value = '';
                        document.getElementById("FormCreate").style.display = 'inline';
                        document.getElementById("TRreplied").style.display = 'inline';
                        document.getElementById("FormFilter").style.display = 'none';
                        document.getElementById("CreateUserName").style.display = 'none';
                        document.getElementById("ReplyUserName").style.display = 'inline';
                        //--  document.getElementById("TRaddTemplate").style.display = 'none';
                        var txtUserNameId = document.getElementById("txtRelplyUser");
                        txtUserNameId.value = DATA[1] + '[' + DATA[6] + ']';
                        document.getElementById("txtRelplyUser").disabled = true;

                        var replyText = document.getElementById("txtReply");
                        replyText.value = 'On ' + DATA[2] + ', \" ' + DATA[1] + ' \" Wrote: \n\t' + DATA[3];
                    }
                    else
                        alert('You Can not Reply System Generated message!');
                }
                else
                    alert('You Can not reply more than one message at a time!');
            }
            if (DATA[0] == "create") {
                if (DATA[1] == "Y") {
                    alert('Message sended Successfully!');
                    document.getElementById("FormCreate").style.display = 'none';
                    grid.PerformCallback('unRead');
                    counter = 'n';
                }
                else
                    alert('Unseccessful! Try Again');
            }
            if (DATA[0] == "send") {
                if (DATA[1] == "Y") {
                    alert('Message sended Successfully!');
                    document.getElementById("FormCreate").style.display = 'none';
                    grid.PerformCallback('unRead');
                    grid.UnselectAllRowsOnPage();
                    counter = 'n';
                }
            }
            if (DATA[0] == "delete") {
                if (DATA[1] == "Y") {
                    alert('Deleted Successfully!');
                    grid.PerformCallback('inBox');
                    grid.UnselectAllRowsOnPage();
                }
                else
                    alert('Unseccessful! Try Again');
            }
            if (DATA[0] == "saveTemp") {
                if (DATA[1] == "Y") {
                    alert('Updated Successfully!');
                    btnCancelTemp_click();
                }
                else if (DATA[1] == "N")
                    alert('Unseccessful! Try Again');
                else if (DATA[1] == "E")
                    alert('Short Description Already exist!');
            }
        }

        function ShowContent() {

            var cmb = document.getElementById('cmbTemplate').value;
            var obj = 'BindCombo~' + cmb;
            CallServer(obj, "");

            //      var btn = document.getElementById('btnhide');
            //      btn.click();
            // cmbTemplate.Attributes.Add("onchange", "frmOpenNewWindow1('frmshowtemplate.aspx?tem_id='+ window.document.aspnetForm.cmbTemplate.options[window.document.aspnetForm.cmbTemplate.selectedIndex].value,'250','1000')");
            //frmOpenNewWindow1('frmshowtemplate.aspx?tem_id='+ document.getElementById('cmbTemplate').value ,'250','1000')

        }

    </script>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Messages</h3>
        </div>

    </div>
    <div class="form_main">
        <table class="TableMain100">

            <tr>
                <td style="text-align: left">
                    <input id="btnRead" type="button" value="Read" class="btn btn-primary" onclick="btnRead_click();"
                        tabindex="1" />
                    <input id="btnCreate" type="button" value="Create" class="btn btn-primary" onclick="btnCreate_Click();"
                        tabindex="1" />
                    <input id="btnReply" type="button" value="Reply" class="btn btn-info" onclick="btnReply_Click();"
                        tabindex="1" />
                    <input id="btnDelete" type="button" value="Delete" class="btn btn-danger" onclick="btnDelete_click();"
                        tabindex="4" />
                    <input id="btnInbox" type="button" value="Inbox" class="btn btn-success" onclick="btnInbox_Click();"
                        tabindex="4" />
                    <input id="btnDeliver" type="button" value="Outbox" class="btn btn-warning" onclick="btnOutbox_Click();"
                        tabindex="4" />
                    <input id="btnHistory" type="button" value="History" class="btn btn-info" onclick="btnHistory_Click();"
                        tabindex="4" />
                </td>
            </tr>
            <tr id="FormFilter">
                <td>
                    <table>
                        <tr>
                            <td>
                                <span style="font-size: 10px; color: #000099"></span>
                                <%-- <asp:TextBox ID="txtStart" runat="server" TabIndex="1" Width="88px" ValidationGroup="d" Font-Size="12px"></asp:TextBox>
                            <asp:Image ID="imgStart" runat="server" ImageUrl="~/images/calendar.jpg" />--%>
                                <dxe:ASPxDateEdit ID="txtStart" runat="server" EditFormat="Custom" UseMaskBehavior="true">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                    <DropDownButton Text="From Date">
                                    </DropDownButton>
                                </dxe:ASPxDateEdit>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtStart"
                                    ErrorMessage="Required!"></asp:RequiredFieldValidator></td>
                            <td>
                                <span style="font-size: 10px; color: #000099"></span>
                                <%--<asp:TextBox ID="txtEnd" runat="server" TabIndex="2" Width="87px" ValidationGroup="d" Font-Size="12px"></asp:TextBox>
                            <asp:Image ID="imgEnd" runat="server" ImageUrl="~/images/calendar.jpg" />--%>
                                <dxe:ASPxDateEdit ID="txtEnd" runat="server" EditFormat="Custom" UseMaskBehavior="true">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                    <DropDownButton Text="End Date">
                                    </DropDownButton>
                                </dxe:ASPxDateEdit>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEnd"
                                    ErrorMessage="Required!"></asp:RequiredFieldValidator></td>
                            <td valign="top">
                                <input id="btnFilter" type="button" value="Get" class="btnUpdate btn btn-primary" onclick="btnFilter_click();"
                                    tabindex="1" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="FormCreate">
                <td>
                    <table class="TableMain100">
                        <tr>
                            <td class="gridcellright">Use Templates:</td>
                            <td style="display: none;" id="TDtemplate" class="gridcellleft">
                                <asp:DropDownList ID="cmbTemplate" runat="server" Width="151px"
                                    onchange="ShowContent();">
                                </asp:DropDownList>
                                <%--   <a id="btnHideTemplate" href="javascript:void(0);" onclick="btnHideTemplate_click()">
                                        <span style="color: #000099; text-decoration: underline">Hide</span></a>&nbsp;&nbsp;
                                    <a id="A1" href="javascript:void(0);" onclick="btnAddTemplate_click()"><span style="color: #000099;
                                        text-decoration: underline">Add</span></a>--%>
                            </td>
                            <td class="gridcellleft" id="TDshow">
                                <%-- <a id="btnShowTemplate" href="javascript:void(0);" onclick="btnShowTemplate_click()">
                                        <span style="color: #000099; text-decoration: underline">Show</span></a>--%>
                            </td>
                        </tr>
                        <%-- <tr id="TRaddTemplate">
                                <td colspan="2">
                                    <table class="TableMain100">
                                        <tr>
                                            <td colspan="2">
                                                <table class="TableMain100">
                                                    <tr>
                                                        <td style="vertical-align: top; width: 480px;">
                                                            <table class="TableMain100">
                                                                <tr>
                                                                    <td class="gridcellright" style="width: 26%">
                                                                        <span style="font-size: 10px; color: #000099">Short description:</span></td>
                                                                    <td colspan="2" class="gridcellleft">
                                                                        <asp:TextBox ID="txtShortDescription" runat="server" Width="269px" Font-Size="12px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="gridcellright" style="width: 26%">
                                                                        <span style="font-size: 10px; color: #000099">Recipients:</span></td>
                                                                    <td colspan="2" class="gridcellleft">
                                                                        <asp:TextBox ID="txtRecipients" runat="server" Width="269px" Font-Size="12px"></asp:TextBox>
                                                                        <asp:TextBox ID="txtRecipients_hidden" runat="server" Width="2px" Font-Size="12px"></asp:TextBox>
                                                                        <a id="A2" href="javascript:void(0);" onclick="btnAddToList_click()"><span style="color: #000099;
                                                                            text-decoration: underline">Add to List</span></a>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="gridcellright" style="width: 26%">
                                                                        <span style="font-size: 10px; color: #000099">Access Level:</span></td>
                                                                    <td colspan="2" class="gridcellleft">
                                                                        <asp:DropDownList ID="drp_accesslevel" runat="server" Width="156px" Font-Size="12px">
                                                                            <asp:ListItem Value="1">Public</asp:ListItem>
                                                                            <asp:ListItem Value="0">Private</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            <table class="TableMain100">
                                                                <tr>
                                                                    <td class="gridcellleft">
                                                                        <asp:ListBox ID="ListBoxUserAll" runat="server" Width="61%" Height="64px" Font-Size="12px">
                                                                        </asp:ListBox>
                                                                        <a id="A3" href="javascript:void(0);" onclick="btnRemoveToList_click()"><span style="color: #000099;
                                                                            text-decoration: underline">Remove</span></a>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:HiddenField ID="HREC" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="gridcellright" style="width: 15%">
                                                <span style="font-size: 10px; color: #000099">Message:</span></td>
                                            <td colspan="2" class="gridcellleft">
                                                <asp:TextBox ID="txtMessage" runat="server" Width="700px" TextMode="MultiLine" Font-Size="12px"
                                                    Height="48px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="gridcellright" style="width: 15%">
                                            </td>
                                            <td colspan="2" class="gridcellleft">
                                                <input id="txtSaveTemplate" type="button" value="Send" class="btnUpdate" onclick="btnSaveTemp_click();"
                                                    style="width: 66px; height: 19px" tabindex="4" />
                                                <input id="txtcanceltemplate" type="button" value="Cancel" class="btnUpdate" onclick="btnCancelTemp_click();"
                                                    style="width: 66px; height: 19px" tabindex="4" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>--%>
                        <tr id="CreateUserName">
                            <td class="gridcellright">Created For:</td>
                            <td class="gridcellleft">
                                <asp:TextBox ID="txtUser" runat="server" Width="269px"></asp:TextBox>
                                <asp:TextBox ID="txtUser_hidden" runat="server"></asp:TextBox>
                            </td>
                            <td></td>
                        </tr>
                        <tr id="trRecipient">
                            <td class="gridcellright" valign="top">Recipients:</td>
                            <td class="gridcellleft">
                                <table>
                                    <tr>
                                        <td valign="top">
                                            <asp:TextBox ID="txtRecipients" runat="server" Width="269px"></asp:TextBox><br />
                                        </td>
                                        <td align="left" valign="bottom">
                                            <a id="A2" href="javascript:void(0);" onclick="btnAddToList_click()"><span style="color: #000099; text-decoration: underline">Add to List</span></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:ListBox ID="ListBoxUserAll" runat="server" Width="272px" Height="64px"></asp:ListBox>
                                        </td>
                                        <td align="left" valign="bottom">
                                            <a id="A3" href="javascript:void(0);" onclick="btnRemoveToList_click()"><span style="color: #000099; text-decoration: underline">Remove</span></a>
                                        </td>
                                    </tr>
                                </table>
                                <asp:TextBox ID="txtRecipients_hidden" runat="server" Width="2px"></asp:TextBox><br />
                                <asp:HiddenField ID="HREC" runat="server" />
                            </td>
                            <td></td>
                        </tr>
                        <tr id="ReplyUserName">
                            <td class="gridcellright">Created For:></td>
                            <td class="gridcellleft">
                                <asp:TextBox ID="txtRelplyUser" runat="server" Width="269px"></asp:TextBox>
                            </td>
                            <td></td>
                        </tr>
                        <tr id="TRcontent">
                            <td class="gridcellright">Content:</td>
                            <td class="gridcellleft">
                                <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Width="700px" Height="100px"
                                   ></asp:TextBox></td>
                            <td></td>
                        </tr>
                        <tr id="TRreplied">
                            <td></td>
                            <td class="gridcellleft">
                                <asp:TextBox ID="txtReply" runat="server" TextMode="MultiLine" Width="700px"
                                    ReadOnly="true" Height="48px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="TRbutton">
                            <td></td>
                            <td colspan="2" class="gridcellleft">
                                <input id="btnSavedata" type="button" value="Send" class="btn btn-primary" onclick="btnSave_click();"
                                    tabindex="4" />
                                <input id="btnReplydata" type="button" value="Send" class="btn btn-success" onclick="btnSend_click();"
                                    tabindex="4" />
                                <input id="btnCancel" type="button" value="Cancel" class="btn btn-danger" onclick="btnCancel_click();"
                                    tabindex="4" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="FormGrid">
                <td>
                    <dxe:ASPxGridView ID="GridMessage" ClientInstanceName="grid" runat="server" Width="100%"
                        KeyFieldName="Mid" OnCustomCallback="GridMessage_CustomCallback" AutoGenerateColumns="False">
                        <%--<Styles>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                        </Styles>--%>
                        <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="true" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsBehavior AllowMultiSelection="True" />
                        <ClientSideEvents SelectionChanged="function(s, e) { OnGridSelectionChanged(); }" />
                        <Columns>
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="3%">
                                <HeaderTemplate>
                                    <input type="checkbox" onclick="grid.SelectAllRowsOnPage(this.checked);" style="vertical-align: middle;"
                                        title="Select/Unselect all rows on the page"></input>
                                </HeaderTemplate>
                                <HeaderStyle HorizontalAlign="Center">
                                    <Paddings PaddingBottom="1px" PaddingTop="1px" />
                                </HeaderStyle>
                            </dxe:GridViewCommandColumn>
                            <dxe:GridViewDataTextColumn Caption="Created By" FieldName="CreateBy" ReadOnly="True"
                                VisibleIndex="1">
                                <EditFormSettings Visible="False" />
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Created For" FieldName="Target" ReadOnly="True"
                                VisibleIndex="2">
                                <EditFormSettings Visible="False" />
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Content" FieldName="content" ReadOnly="True"
                                VisibleIndex="3">
                                <EditFormSettings Visible="False" />
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Status" FieldName="Status" ReadOnly="True" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                VisibleIndex="4">
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:ASPxGridView>
                </td>
            </tr>
            <tr id="FormOutbox">
                <td>
                    <dxe:ASPxGridView ID="gridOutBox" runat="server" Width="100%" KeyFieldName="Mid"
                        ClientInstanceName="gridoutbox" OnCustomCallback="gridOutBox_CustomCallback">
                        <Styles>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                        </Styles>
                        <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <Columns>
                            <dxe:GridViewDataTextColumn Caption="Created For" FieldName="Target" ReadOnly="True"
                                VisibleIndex="2">
                                <EditFormSettings Visible="False" />
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Content" FieldName="content" ReadOnly="True"
                                VisibleIndex="3">
                                <EditFormSettings Visible="False" />
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Read Date" FieldName="ReadDate" ReadOnly="True"
                                VisibleIndex="3">
                                <EditFormSettings Visible="False" />
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Status" FieldName="Status" ReadOnly="True"
                                VisibleIndex="4">
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:ASPxGridView>
                </td>
            </tr>
            <tr id="FormHistory">
                <td>
                    <table class="TableMain100">
                        <tr>
                            <td valign="top">User:</td>
                            <td class="gridcellleft" valign="top">
                                    <asp:TextBox ID="txtUserName" runat="server" Width="300px" TabIndex="1"></asp:TextBox>
                            </td>
                            <td>
                                <%--<asp:TextBox ID="txtFromDate" runat="server" TabIndex="2" Width="88px" ValidationGroup="d" Font-Size="12px"></asp:TextBox>
                            <asp:Image ID="imgFromDate" runat="server" ImageUrl="~/images/calendar.jpg" />--%>
                                <dxe:ASPxDateEdit ID="txtFromDate" runat="server" EditFormat="Custom" UseMaskBehavior="true">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                    <DropDownButton Text="From Date">
                                    </DropDownButton>
                                </dxe:ASPxDateEdit>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFromDate"
                                    ErrorMessage="Required!"></asp:RequiredFieldValidator></td>
                            <td>
                                <span style="font-size: 10px; color: #000099"></span>
                                <%--    <asp:TextBox ID="txtToDate" runat="server" TabIndex="3" Width="87px" ValidationGroup="d" Font-Size="12px"></asp:TextBox>
                            <asp:Image ID="imgToDate" runat="server" ImageUrl="~/images/calendar.jpg" />--%>
                                <dxe:ASPxDateEdit ID="txtToDate" runat="server" EditFormat="Custom" UseMaskBehavior="true">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                    <DropDownButton Text="To Date">
                                    </DropDownButton>
                                </dxe:ASPxDateEdit>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtToDate"
                                    ErrorMessage="Required!"></asp:RequiredFieldValidator></td>
                            <td valign="top">
                                <input id="btnGetHistory" type="button" value="Get History" class="btnUpdate btn btn-primary" onclick="btnGetHistory_click();"
                                    tabindex="4" />
                            </td>
                        </tr>
                        <tr id="TRHistory">
                            <td colspan="4">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="text-align: right">
                                            <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" 
                                                Font-Bold="False" ForeColor="black" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                                ValueType="System.Int32" Width="130px">
                                                <Items>
                                                    <dxe:ListEditItem Text="Select" Value="0" />
                                                    <dxe:ListEditItem Text="PDF" Value="1" />
                                                    <dxe:ListEditItem Text="XLS" Value="2" />
                                                    <dxe:ListEditItem Text="RTF" Value="3" />
                                                    <dxe:ListEditItem Text="CSV" Value="4" />
                                                </Items>
                                                <ButtonStyle >
                                                </ButtonStyle>
                                                <ItemStyle >
                                                    <HoverStyle >
                                                    </HoverStyle>
                                                </ItemStyle>
                                                <Border BorderColor="black" />
                                                <DropDownButton Text="Export">
                                                </DropDownButton>
                                            </dxe:ASPxComboBox>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dxe:ASPxGridView ID="GridHistory" runat="server" Width="100%" KeyFieldName="Mid"
                                                ClientInstanceName="gridhistory" OnCustomCallback="GridHistory_CustomCallback">
                                                <Styles>
                                                    <LoadingPanel ImageSpacing="10px">
                                                    </LoadingPanel>
                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                    </Header>
                                                </Styles>
                                                <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                                                    <FirstPageButton Visible="True">
                                                    </FirstPageButton>
                                                    <LastPageButton Visible="True">
                                                    </LastPageButton>
                                                </SettingsPager>
                                                <Columns>
                                                    <dxe:GridViewDataTextColumn Caption="Created By" FieldName="sender" ReadOnly="True"
                                                        VisibleIndex="2">
                                                        <EditFormSettings Visible="False" />
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Created For" FieldName="Target" ReadOnly="True"
                                                        VisibleIndex="2">
                                                        <EditFormSettings Visible="False" />
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Content" FieldName="content" ReadOnly="True"
                                                        VisibleIndex="3">
                                                        <EditFormSettings Visible="False" />
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Created Date" FieldName="CreatedDate" ReadOnly="True"
                                                        VisibleIndex="3">
                                                        <EditFormSettings Visible="False" />
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Read Date" FieldName="ReadDate" ReadOnly="True"
                                                        VisibleIndex="3">
                                                        <EditFormSettings Visible="False" />
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Delete Date" FieldName="DeleteDate" ReadOnly="True"
                                                        VisibleIndex="4">
                                                        <EditFormSettings Visible="False" />
                                                    </dxe:GridViewDataTextColumn>
                                                </Columns>
                                            </dxe:ASPxGridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dxe:ASPxGridViewExporter ID="exporter" runat="server" GridViewID="GridHistory">
                                            </dxe:ASPxGridViewExporter>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="display: none">
                    <asp:Button ID="btnhide" runat="server" Text="Button" OnClick="btnhide_Click" />
                    <asp:HiddenField ID="HREC_Hidden" runat="server" />
                    <asp:TextBox ID="txtUserName_hidden" runat="server" Font-Size="12px" Width="11px"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
