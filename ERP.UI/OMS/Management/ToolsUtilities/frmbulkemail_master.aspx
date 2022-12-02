<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_utilities_frmbulkemail_master" CodeBehind="frmbulkemail_master.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/assests/js/init.js"></script>
    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>
    <script type="text/javascript">

        function AtTheTimePageLoad() {
            FieldName = 'cmbBodySource';


            //_____________________
            document.getElementById("FormSubscription").style.display = 'none';
            document.getElementById("FormTemplate").style.display = 'none';
            document.getElementById("FormGrid").style.display = 'inline';
            document.getElementById("trDateTime").style.display = 'none';
        }
        function frmOpenNewWindow_custom(location, v_height, v_weight, top, left) {
            window.open(location, "Search_Conformation_Box", "height=" + v_height + ",width=" + v_weight + ",top=" + top + ",left=" + left + ",location=no,directories=no,menubar=no,toolbar=no,status=no,scrollbars=yes,resizable=no,dependent=no'");
        }
        function frmOpenNewWindow() {
            $("#Div").show();
        } 
        function frmOpenNewWindow_Header() {
            $("#myDiv").show();
        }
        function BodySourceChange() {
            //alert('s');
            var sourcecombo = document.getElementById("cmbBodySource");
            if (sourcecombo.value == '0')
                document.getElementById("trMessageBody").style.display = '';
            else
                document.getElementById("trMessageBody").style.display = 'none';
        }


        //function is called on changing focused row
        function OnGridFocusedRowChanged() {
            // Query the server for the "bem_id" fields from the focused row 
            // The values will be returned to the OnGetRowValues() function     
            grid.GetRowValues(grid.GetFocusedRowIndex(), 'bem_id', OnGetRowValues);
        }
        //Value array contains "bem_id" field values returned from the server 
        function OnGetRowValues(values) {
            RowId = values;

        }

        function btnCreate_click() {
            visibleValidation();
            document.getElementById("hndRecipients_hidden").value = '';
            document.getElementById("FormSubscription").style.display = 'none';
            document.getElementById("FormTemplate").style.display = 'inline';
            document.getElementById("FormTemplate").style.display = 'table-row';
            document.getElementById("FormGrid").style.display = 'none';

            var cmbSendingOption = document.getElementById("cmbSendingOption");
            if (cmbSendingOption.value == '0')
                document.getElementById("tdSendingTime").style.display = 'none';
            else
                document.getElementById("tdSendingTime").style.display = 'inline';

            var ListBoxUserAll = document.getElementById("ListBoxUserAll");
            ListBoxUserAll.length = 0;
            document.getElementById("trMessageBody").style.display = '';

            var txtDescription = document.getElementById("txtDescription");
            txtDescription.value = '';
            var txtSubject = document.getElementById("txtSubject");
            txtSubject.value = '';
            var txtMessageHeader = document.getElementById("txtMessageHeader");
            txtMessageHeader.value = '';
            var cmbBodySource = document.getElementById("cmbBodySource");
            cmbBodySource.value = '0';
            var txtMessageBody = document.getElementById("txtMessageBody");
            txtMessageBody.value = '';
            var txtMessageFooter = document.getElementById("txtMessageFooter");
            txtMessageFooter.value = '';
            var txtReplyTo = document.getElementById("txtReplyTo");
            txtReplyTo.value = '';
            var txtSenderName = document.getElementById("txtSenderName");
            txtSenderName.value = '';
            var txtSenderEmail = document.getElementById("txtSenderEmail");
            txtSenderEmail.value = '';

            document.getElementById("txtDescription").focus();
            //document.getElementById("txtRecipients_hidden").style.display = 'none';
            document.getElementById("txtsubscriptionID_hidden").style.display = 'none';
            document.getElementById("btnupdate").style.display = 'none';
            document.getElementById("btnSave").style.display = 'inline';
            RowId = '0';

            ListBind();
            BindRecipients();

        }
        function btnModify_click() {
            document.getElementById("FormSubscription").style.display = 'none';
            document.getElementById("FormTemplate").style.display = 'inline';
            document.getElementById("FormTemplate").style.display = 'table-row';
            document.getElementById("FormGrid").style.display = 'none';
            document.getElementById("btnupdate").style.display = 'inline';
            document.getElementById("btnSave").style.display = 'none';
            var senddata = 'Edit~' + RowId;
            CallServer(senddata, "");
        }
        function btnDelete_click() {
            document.getElementById("FormSubscription").style.display = 'none';
            document.getElementById("FormTemplate").style.display = 'none';
            document.getElementById("FormGrid").style.display = 'inline';
            var senddata = 'Delete~' + RowId;
            CallServer(senddata, "");
        }

        function btnEditsubscription_click() {
            document.getElementById("FormSubscription").style.display = 'inline';
            document.getElementById("FormTemplate").style.display = 'none';
            document.getElementById("FormGrid").style.display = 'none';
        }
        function btnCalcel_click() {
            document.getElementById("FormSubscription").style.display = 'none';
            document.getElementById("FormTemplate").style.display = 'none';
            document.getElementById("FormGrid").style.display = 'inline';
            EmptyFields();
        }

        function btnRemovefromsubscriptionlist_click() {
            var listBox = document.getElementById("lstSuscriptions");
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
        function btnAddsubscriptionlist_click() {
            var userid = document.getElementById("txtsubscriptionID");
            if (userid.value != '') {
                var ids = userid.value.split(',');
                var listBox = document.getElementById("lstSuscriptions");
                var tLength = listBox.length;
                //alert(tLength);

                var no = new Option();
                no.value = ids[1];
                no.text = ids[0];
                listBox[tLength] = no;
                var recipient = document.getElementById("txtsubscriptionID");
                recipient.value = '';
            }
            else
                alert('Please search name and then Add!')
        }

        function btnAddToList_click() {
            var userid = document.getElementById("txtRecipients");
            var ids = userid.value.split(',');
            var listBox = document.getElementById("ListBoxUserAll");
            var tLength = listBox.length;
            //alert(tLength);

            var no = new Option();
            no.value = ids[1];
            no.text = ids[0];
            listBox[tLength] = no;
            var recipient = document.getElementById("txtRecipients");
            recipient.value = '';
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

        function btnSave_click() {
            save('New');
        }
        function save(obj) {

            visibleValidation();
            var txtDescription = document.getElementById("<%=txtDescription.ClientID%>");
            if (txtDescription.value == '') {
                $("#MandatoryDescription").show();
                //alert('Please Fill Description!');
                txtDescription.focus();
                return false;
            }
            else {
                var filter = /^.+"/;
                if (filter.test(txtDescription.value)) {
                    $("#MandatoryDescription").show();
                    //alert('Description can not have " character!');
                    document.getElementById("txtDescription").focus();
                    return false;
                }
            }
            var txtSubject = document.getElementById("txtSubject");
            if (txtSubject.value == '') {
                $("#MandatorySubject").show();
                //alert('Please Fill Email Subject!');
                txtSubject.focus();
                return false;
            }
            else {
                var filter = /^.+"/;
                if (filter.test(txtSubject.value)) {
                    $("#MandatorySubject").show();
                    //alert('Subject can not have " character!');
                    document.getElementById("txtSubject").focus();
                    return false;
                }
            }
            var txtMessageHeader = document.getElementById("txtMessageHeader");
            if (txtMessageHeader.value == '') {
                $("#MandatoryHeader").show();
                //alert('Please Fill Message Header!');
                txtMessageHeader.focus();
                return false;
            }
            else {
                var filter = /^(.|\n)+"/;
                if (filter.test(txtMessageHeader.value)) {
                    $("#MandatoryHeader").show();
                    //alert('Message Header can not have " character!');
                    document.getElementById("txtMessageHeader").focus();
                    return false;
                }
            }
            var cmbBodySource = document.getElementById("cmbBodySource");
            var txtMessageBody = document.getElementById("txtMessageBody");
            if (cmbBodySource.value == '0') {
                if (txtMessageBody.value == '') {
                    $("#MandatoryBody").show();
                    //alert('Please Fill Message Body!');
                    txtMessageBody.focus();
                    return false;
                }
                else {
                    var str = txtMessageBody.value;
                    //alert(str);
                    var filter = /^(.|\n)+"/;
                    if (filter.test(txtMessageBody.value)) {
                        $("#MandatoryBody").show();
                        //alert('Message Body can not have " character!');
                        document.getElementById("txtMessageBody").focus();
                        return false;
                    }
                }
            }
            var txtMessageFooter = document.getElementById("txtMessageFooter");
            if (txtMessageFooter.value == '') {
                $("#MandatoryFooter").show();
                //alert('Please Fill Message Footer!');
                txtMessageFooter.focus();
                return false;
            }
            else {
                var filter = /^(.|\n)+"/;
                if (filter.test(txtMessageFooter.value)) {
                    $("#MandatoryFooter").show();
                    //alert('Message Footer can not have " character!');
                    document.getElementById("txtMessageFooter").focus();
                    return false;
                }
            }
            var txtReplyTo = document.getElementById("txtReplyTo");
            if (txtReplyTo.value == '') {
                $("#MandatoryReplyTo").show();
                //alert('Please Fill Email ID for Reply To!');
                document.getElementById("txtReplyTo").focus();
                return false;
            }
            else {
                var filter = /^.+@.+\..{2,3}$/;
                if (filter.test(txtReplyTo.value)) {
                }
                else {
                    $("#MandatoryReplyTo").show();
                    //alert('Input valid \'reply To\' E-mail ID!');
                    txtReplyTo.value = '';
                    document.getElementById("txtReplyTo").focus();
                    return false;
                }
            }

            var txtSenderName = document.getElementById("txtSenderName");
            if (txtSenderName.value == '') {
                $("#MandatoryDisplay").show();
                //alert('Please Fill Sender Display Name!');
                txtSenderName.focus();
                return false;
            }
            else {
                var filter = /^.+"/;
                if (filter.test(txtSenderName.value)) {
                    $("#MandatoryDisplay").show();
                    //alert('Sender name can not have " character!');
                    document.getElementById("txtSenderName").focus();
                    return false;
                }
            }
            var txtSenderEmail = document.getElementById("txtSenderEmail");
            if (txtSenderEmail.value == '') {
                $("#MandatoryReplyFrom").show();
                //alert('Please Fill Sender Email ID!');
                txtSenderEmail.focus();
                return false;
            }
            else {
                var filter = /^.+@.+\..{2,3}$/;
                if (filter.test(txtSenderEmail.value)) {
                }
                else {
                    $("#MandatoryReplyFrom").show();
                    //alert('Input valid sender E-mail ID!');
                    txtSenderEmail.value = '';
                    txtSenderEmail.focus();
                    return false;
                }
            }
            var cmbSendingOption = document.getElementById("cmbSendingOption");
            var cmbhoure = document.getElementById("cmbhoure");
            var cmbminute = document.getElementById("cmbminute");
            var cmbampm = document.getElementById("cmbampm");
            var txtStartDate = document.getElementById("txtStartDate");
            if (txtStartDate.value == '') {
                $("#MandatoryStartDate").show();
                //alert('Please Fill Start Date!');
                txtStartDate.focus();
                return false;
            }
            var txtEndDate = document.getElementById("txtEndDate");
            if (txtEndDate.value == '') {
                $("#EndStartDate").show();
                //alert('Please Fill End Date!');
                txtEndDate.focus();
                return false;
            }

            var listBox = document.getElementById("ListBoxUserAll");

            var tLength = listBox.length;

            var userlist = '0';
            var i = 0;
            if (tLength > 0) {
                for (i = 0; i < tLength; i++) {
                    if (userlist == '0') {
                        userlist = listBox.options[i].value;
                    }
                    else {
                        userlist += ',' + listBox.options[i].value;
                    }
                }
            }

            var listData = document.getElementById("hndRecipients_hidden").value;

            //alert(userlist);
            if (obj == 'New') {
                var sendItems = 'Save~' + txtDescription.value + '~' + txtSubject.value + '~' + txtMessageHeader.value + '~' + cmbBodySource.value + '~' + txtMessageBody.value + '~' + txtMessageFooter.value + '~' + txtReplyTo.value + '~dummy~' + txtSenderName.value + '~' + txtSenderEmail.value + '~' + cmbSendingOption.value + '~' + cmbhoure.value + '~' + cmbminute.value + '~' + cmbampm.value + '~' + txtStartDate.value + '~' + txtEndDate.value + '~' + userlist + '~' + listData;
                //alert(sendItems);
                CallServer(sendItems, "");
            }
            if (obj == 'Edit') {
                var sendItems = 'Update~' + txtDescription.value + '~' + txtSubject.value + '~' + txtMessageHeader.value + '~' + cmbBodySource.value + '~' + txtMessageBody.value + '~' + txtMessageFooter.value + '~' + txtReplyTo.value + '~dummy~' + txtSenderName.value + '~' + txtSenderEmail.value + '~' + cmbSendingOption.value + '~' + cmbhoure.value + '~' + cmbminute.value + '~' + cmbampm.value + '~' + txtStartDate.value + '~' + txtEndDate.value + '~' + userlist + '~' + RowId + '~' + listData;
                //alert(sendItems);
                CallServer(sendItems, "");
            }
        }
        function EmptyFields() {
            //debugger;
            var txtDescription = document.getElementById("txtDescription");
            txtDescription.value = '';
            var txtSubject = document.getElementById("txtSubject");
            txtSubject.value = '';
            var txtMessageHeader = document.getElementById("txtMessageHeader");
            txtMessageHeader.value = '';
            var cmbBodySource = document.getElementById("cmbBodySource");
            cmbBodySource.value = '0';
            var txtMessageBody = document.getElementById("txtMessageBody");
            txtMessageBody.value = '';
            var txtMessageFooter = document.getElementById("txtMessageFooter");
            txtMessageFooter.value = '';
            var chkDefaultFooter = document.getElementById("chkDefaultFooter");
            chkDefaultFooter.checked = false;
            var txtReplyTo = document.getElementById("txtReplyTo");
            txtReplyTo.value = '';
            var txtUnsubscribeEmailid = document.getElementById("txtUnsubscribeEmailid");
            txtUnsubscribeEmailid = '';
            var txtSenderName = document.getElementById("txtSenderName");
            txtSenderName.value = '';

            var txtSenderEmail = document.getElementById("txtSenderEmail");
            txtSenderEmail.value = '';

            var cmbSendingOption = document.getElementById("cmbSendingOption");
            var cmbhoure = document.getElementById("cmbhoure");
            var cmbminute = document.getElementById("cmbminute");
            var cmbampm = document.getElementById("cmbampm");
            var listBox = document.getElementById("ListBoxUserAll");
            listBox.length = 0;
        }
        function SendingMailOption(obj) {
            if (obj == '0')
                document.getElementById("tdSendingTime").style.display = 'none';
            else
                document.getElementById("tdSendingTime").style.display = 'inline';
        }
        function btnUpdate_click() {
            visibleValidation();
            save('Edit');
        }

    </script>
    <script type="text/ecmascript">
        function FooterChange(obj) {
            if (obj == true) {
                CallServer('disclaimer', "");
            }
            if (obj == false) {
                document.getElementById("txtMessageFooter").disabled = false;
                var foot = document.getElementById("txtMessageFooter");
                foot.value = '';
            }

        }
        function btnEditsubscription_click(keyValue) {
            document.getElementById("FormSubscription").style.display = 'inline';
            document.getElementById("FormTemplate").style.display = 'none';
            document.getElementById("FormGrid").style.display = 'inline';
            document.getElementById("txtsubscriptionID_hidden").style.display = 'none';
            var sendData = 'Subscriptionlist~' + keyValue;
            //alert(sendData);
            CallServer(sendData, "");
        }
        function btnSaveSubscription_click() {
            var listBoxSubs = document.getElementById("lstSuscriptions");

            var listIDs = '';
            var i;
            if (listBoxSubs.length > 0) {
                for (i = 0; i < listBoxSubs.length; i++) {
                    if (listIDs == '')
                        listIDs = listBoxSubs.options[i].value;
                    else
                        listIDs += ',' + listBoxSubs.options[i].value;
                }
                var sendData = 'SaveSubscriptionlist~' + RowId + '~' + listIDs;
                CallServer(sendData, "");
            }
            else
                alert('Add User in listBox by searching from \"Subscribe user\" text box!');
        }


        function ReceiveServerData(rValue) {
            //debugger;
            var DATA = rValue.split('~');
            //alert(rValue); 
            if (DATA[0] == 'disclaimer') {
                if (DATA[1] != 'N') {
                    var foot = document.getElementById("txtMessageFooter");
                    foot.value = DATA[1];
                    document.getElementById("txtMessageFooter").disabled = true;
                }
                else {
                    alert('No disclaimer Present in the system!!');
                }
            }
            if (DATA[0] == 'Subscriptionlist') {
                var listBoxSubs = document.getElementById("lstSuscriptions");
                if (DATA[1] != 'N') {
                    listBoxSubs.length = 0;
                    var lenth = DATA.length;
                    //alert(lenth);
                    var i;
                    for (i = 1; i < lenth; i++) {
                        var idWname = DATA[i].split(',');
                        var opt = new Option();
                        opt.text = idWname[1];
                        opt.value = idWname[0];
                        listBoxSubs[i - 1] = opt;
                    }
                }
                else
                    listBoxSubs.length = 0;
            }
            if (DATA[0] == 'SaveSubscriptionlist') {
                alert(DATA[1]);
                grid.PerformCallback();
                document.getElementById("FormSubscription").style.display = 'none';
                var listBoxSubs = document.getElementById("lstSuscriptions");
                listBoxSubs.length = 0;
            }
            if (DATA[0] == 'Delete') {
                if (DATA[1] == 'Y') {
                    alert(DATA[2]);
                    grid.PerformCallback();
                }
                else
                    alert(DATA[1]);
            }
            if (DATA[0] == 'Edit') {
                var items = DATA[1].split('"');
                var txtDescription = document.getElementById("txtDescription");
                txtDescription.value = items[0];
                var txtSubject = document.getElementById("txtSubject");
                txtSubject.value = items[1];
                var txtMessageHeader = document.getElementById("txtMessageHeader");
                txtMessageHeader.value = items[2];
                var cmbBodySource = document.getElementById("cmbBodySource");
                cmbBodySource.value = items[3];
                var txtMessageBody = document.getElementById("txtMessageBody");
                if (items[3] == '0')
                    txtMessageBody.value = items[4];
                else
                    document.getElementById("trMessageBody").style.display = 'none';
                var txtMessageFooter = document.getElementById("txtMessageFooter");
                txtMessageFooter.value = items[5];
                var txtReplyTo = document.getElementById("txtReplyTo");
                txtReplyTo.value = items[6];
                var txtSenderName = document.getElementById("txtSenderName");
                txtSenderName.value = items[7];
                var txtSenderEmail = document.getElementById("txtSenderEmail");
                txtSenderEmail.value = items[8];
                var cmbSendingOption = document.getElementById("cmbSendingOption");
                cmbSendingOption.value = items[9];
                if (items[9] == '1')
                    document.getElementById("tdSendingTime").style.display = 'inline';
                else
                    document.getElementById("tdSendingTime").style.display = 'none';
                var am_pm = items[10].split(' ');
                var h_m = am_pm[0].split(':');
                var cmbhoure = document.getElementById("cmbhoure");
                cmbhoure.value = h_m[0];
                var cmbminute = document.getElementById("cmbminute");
                cmbminute.value = h_m[1];
                var cmbampm = document.getElementById("cmbampm");
                cmbampm.value = am_pm[1];
                //document.getElementById("txtRecipients_hidden").style.display = 'none';

                //var ListBoxUserAll = document.getElementById("ListBoxUserAll");
                //ListBoxUserAll.length = 0;
                //var DATAlist = DATA[2].split('!');
                //var lenth = DATAlist.length;
                //var i;
                //for (i = 0; i < lenth; i++) {
                //    var idWname = DATAlist[i].split(',');
                //    var opt = new Option();
                //    opt.text = idWname[1];
                //    opt.value = idWname[0];
                //    ListBoxUserAll[i] = opt;
                //}

                var test = DATA[2];
                document.getElementById("hndRecipients_hidden").value = DATA[2];
                BindRecipients();

            }
            if (DATA[0] == 'Save') {
                if (DATA[1] != 'Y') {
                    alert(DATA[1]);
                    EmptyFields();
                    AtTheTimePageLoad();
                    grid.PerformCallback();
                }
                else {
                    alert('Successfully Updated!');
                    EmptyFields();
                    AtTheTimePageLoad();
                    grid.PerformCallback();
                }
            }
            if (DATA[0] == 'Update') {
                if (DATA[1] != 'Y') {
                    alert(DATA[1]); EmptyFields();
                    AtTheTimePageLoad();
                    grid.PerformCallback();
                }

                else {
                    alert('Successfully Updated!');
                    EmptyFields();
                    AtTheTimePageLoad();
                    grid.PerformCallback();
                }
            }

        }
    </script>

    <%--Subscribing Email list is binded Sudip 19-12-2016--%>
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script type="text/javascript">
        //$(function () {
        //    ListBind();
        //    BindRecipients();
        //});

        function BindRecipients() {
            var lBox = $('select[id$=lstItems]');
            var listItems = [];

            lBox.empty();
            var firstParam = '';

            $.ajax({
                type: "POST",
                url: 'frmbulkemail_master.aspx/GetRecipients',
                data: "{Type:\"" + firstParam + "\"}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;

                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {

                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];

                            listItems.push('<option value="' +
                            id + '">' + name
                            + '</option>');
                        }

                        $(lBox).append(listItems.join(''));


                        setWithFromBind();
                        ListBind();
                        ListAssignTo();
                        $('#lstItems').trigger("chosen:updated");
                        $('#lstItems').prop('disabled', false).trigger("chosen:updated");
                    }
                    else {
                        lBox.empty();
                        $('#lstItems').trigger("chosen:updated");
                        $('#lstItems').prop('disabled', true).trigger("chosen:updated");

                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        }

        function ListBind() {
            var config = {
                '.chsn': {},
                '.chsn-deselect': { allow_single_deselect: true },
                '.chsn-no-single': { disable_search_threshold: 10 },
                '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
                '.chsn-width': { width: "100%" }
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }
        }

        function ListAssignTo() {
            $('#lstItems').chosen();
            $('#lstItems').fadeIn();
        }

        function changeFunc() {
            var ListValue = '';

            var techGroups = document.getElementById("lstItems");
            for (var i = 0; i < techGroups.options.length; i++) {
                if (techGroups.options[i].selected == true) {
                    if (ListValue == '') {
                        ListValue = techGroups.options[i].value;
                    }
                    else {
                        ListValue = ListValue + ',' + techGroups.options[i].value;
                    }
                }
            }

            document.getElementById("hndRecipients_hidden").value = ListValue;
        }

        function setWithFromBind() {
            var techGroups = document.getElementById("lstItems");
            var listValue = document.getElementById("hndRecipients_hidden").value;
            var str_array = listValue.split(',');

            for (var i = 0; i < techGroups.options.length; i++) {
                var selectedValue = techGroups.options[i].value;

                if (str_array.indexOf(selectedValue) > -1) {
                    techGroups.options[i].selected = true;
                }
                else {
                    techGroups.options[i].selected = false;
                }
            }
        }
    </script>
    <style type="text/css">
        .chosen-container.chosen-container-multi {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }
    </style>
    <script type="text/javascript">
        function visibleValidation() {
            $("#MandatoryDescription").hide();
            $("#MandatorySubject").hide();
            $("#MandatoryHeader").hide();
            $("#MandatoryBody").hide();
            $("#MandatoryFooter").hide();
            $("#MandatoryReplyTo").hide();
            $("#MandatoryReplyFrom").hide();
            $("#Div").hide();
            $("#myDiv").hide();
        }
        function PostReservedWord(obj) {
            document.getElementById("txtMessageBody").value = document.getElementById("txtMessageBody").value + '< ' + obj + '>'
        }
        function PostReservedWordHeader(obj) {
            document.getElementById("txtMessageHeader").value = document.getElementById("txtMessageHeader").value + '< ' + obj + '>'
        }
    </script>
    <style>
        #MandatoryFooter + span {
            display:inline !important;
        }
        #chkDefaultFooter {
            float:left;
        }
        #chkDefaultFooter + label {
            float:left;
            margin-left:8px;
        }
        #myDiv input, #Div input {
                margin-top: 20px;
                background: #0176c5;
                color: #fff !important;
                border: 1px solid #094b77 !important;
                margin-right: 2px;
        }
        #myDiv input:hover, #myDiv input:focus,
        #Div input:hover, #Div input:focus {
            background:#0a619c;
            box-shadow:none;
        }
        .pullleftClass  {
            position:absolute;
            right: -5px;
            top: 28px;
        }
    </style>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Bulk Email Template</h3>
        </div>
    </div>
    <div class="form_main">
        
        <table class="TableMain100">

            <tr>
                <td style="vertical-align: top; text-align: left">
                    <asp:Panel ID="Panel1" runat="server" Width="100%" BorderColor="white" BorderWidth="1px">
                        <table width="100%">
                            <tr id="FormGrid">
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <% if (rights.CanAdd)
                                                   { %><input id="btnAdd" type="button" value="Add" class="btn btn-primary" onclick="btnCreate_click()" /><%} %>

                                               
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <span class="Ecoheadtxt">Select RFA Category :</span>
                                                <asp:DropDownList ID="lst_requesttype" runat="server" Width="107px">
                                                    <asp:ListItem Value="1">New</asp:ListItem>
                                                    <asp:ListItem Value="2">Pending</asp:ListItem>
                                                    <asp:ListItem Value="3">Forward</asp:ListItem>
                                                    <asp:ListItem Value="4">Closed</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dxe:ASPxGridView ID="GridBulkTemplate" ClientInstanceName="grid" KeyFieldName="bem_id" runat="server" AutoGenerateColumns="False" Width="100%" OnCustomCallback="GridBulkTemplate_CustomCallback">
                                                    <Columns>
                                                        <dxe:GridViewDataTextColumn Caption="Description" FieldName="bem_description" VisibleIndex="0">
                                                            <CellStyle CssClass="gridcellleft"></CellStyle>
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Subject" FieldName="bem_subject" VisibleIndex="1">
                                                            <CellStyle CssClass="gridcellleft"></CellStyle>
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="In Use Date" FieldName="bem_inusedate" VisibleIndex="2" Width="20%">
                                                            <CellStyle CssClass="gridcellleft"></CellStyle>
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Use Until" FieldName="bem_useuntil" VisibleIndex="3" Visible="False">
                                                            <CellStyle CssClass="gridcellleft"></CellStyle>
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Subscription" FieldName="subscription" VisibleIndex="4" Visible="False">
                                                            <CellStyle CssClass="gridcellleft"></CellStyle>
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataColumn Caption="Subscription" FieldName="subs" VisibleIndex="5" Width="8%" Visible="false">
                                                            <DataItemTemplate>
                                                                <%--<a href="javascript:void(0);" onclick="subscription_click('<%# Container.KeyValue %>');"><%# Eval("subscription")%></a>--%>
                                                                <a href="javascript:void(0);" onclick="btnEditsubscription_click('<%# Container.KeyValue %>')">
                                                                    <img src="../../../assests/images/activity.png" /></a>
                                                            </DataItemTemplate>

                                                        </dxe:GridViewDataColumn>
                                                        <dxe:GridViewDataTextColumn VisibleIndex="6" Width="100px">
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                            <DataItemTemplate>
                                                                <% if (rights.CanEdit)
                                                                   { %><a href="javascript:void(0);" onclick="btnModify_click()">
                                                                       <img src="../../../assests/images/Edit.png" /></a><%} %>&nbsp;|&nbsp;

                                                                     <% if (rights.CanDelete)
                                                                        { %><a href="javascript:void(0);" onclick="btnDelete_click()"><img src="../../../assests/images/Delete.png" /></a><%} %>


                                                                <%--  <a href="javascript:void(0);" onclick="btnEditsubscription_click('<%# Container.KeyValue %>')" title="Subscription">--%>
                                                            </DataItemTemplate>
                                                            <HeaderTemplate>
                                                                <%-- <a href="javascript:void(0);" onclick="javascript:OnAddButtonClick();"><span style="color: #000099; text-decoration: underline">Add New</span> </a>--%>
                                                            </HeaderTemplate>
                                                            <EditFormSettings Visible="False"></EditFormSettings>
                                                        </dxe:GridViewDataTextColumn>
                                                    </Columns>
                                                    <Styles>
                                                        <LoadingPanel ImageSpacing="10px">
                                                        </LoadingPanel>
                                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                        </Header>
                                                    </Styles>
                                                    <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                                                        <FirstPageButton Visible="True">
                                                        </FirstPageButton>
                                                        <LastPageButton Visible="True">
                                                        </LastPageButton>
                                                    </SettingsPager>
                                                    <SettingsBehavior AllowFocusedRow="true" AllowSort="False" />
                                                    <ClientSideEvents FocusedRowChanged="function(s, e) { OnGridFocusedRowChanged(); }" />
                                                </dxe:ASPxGridView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr id="FormTemplate">
                                <td>
                                    <div class="crossBtn"><a href="../ToolsUtilities/frmbulkemail_master.aspx"><i class="fa fa-times"></i></a></div>
                                    <div class="row clearfix" style="width:100%">
                                        <div class="col-md-3 relative">
                                            <label>Description:</label>
                                            <asp:TextBox ID="txtDescription" runat="server" Font-Size="12px" Width="100%"></asp:TextBox>
                                            <span id="MandatoryDescription" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                        </div>
                                        <div class="col-md-3 relative">
                                            <label>Email Subject:</label>
                                            <asp:TextBox ID="txtSubject" runat="server" Font-Size="12px" Width="100%"></asp:TextBox>
                                            <span id="MandatorySubject" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                        </div>
                                        <div style="clear:both"></div>
                                        <div class="col-md-6 relative">
                                            <label>Message Header:</label>
                                            <asp:TextBox ID="txtMessageHeader" runat="server" Font-Size="12px" Height="50px" TextMode="MultiLine"
                                                    Width="100%"></asp:TextBox>

                                            <span id="MandatoryHeader" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                &nbsp;
                                    <%--   <a href="#" onclick="frmOpenNewWindow_custom('frmreservedword.aspx?type=receipent&control=window.opener.document.aspnetForm.txtMessageHeader.value','200','400','200','300')">
                                           <span style="color: #000099; text-decoration: underline">Use Reserved Word</span></a>--%>

                                                  <a href="#" onclick="frmOpenNewWindow_Header()" class="pull-right">
                                                    <span style="color: #000099; text-decoration: underline">Use Reserved Word</span></a>
                                        </div>
                                         <div class="col-md-3"><div id="myDiv" runat="server"></div></div>
                                        <div class="clear"></div>
                                        <div class="col-md-3 relative">
                                            <label>Body Content Source:</label>
                                            <asp:DropDownList ID="cmbBodySource" runat="server" Font-Size="12px" Width="100%">
                                                <asp:ListItem Value="0">Use Text</asp:ListItem>
                                                <asp:ListItem Value="1">Use File</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="clear"></div>
                                        <div class="col-md-6 relative" id="trMessageBody">
                                            <label>Message Body:</label>
                                            <asp:TextBox ID="txtMessageBody" runat="server" Font-Size="12px" Height="100px" TextMode="MultiLine"
                                                    Width="100%"></asp:TextBox>

                                                <span id="MandatoryBody" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                <%-- <a href="#" onclick="frmOpenNewWindow_custom('frmreservedword.aspx?type=receipent,sender&control=window.opener.document.aspnetForm.txtMessageBody.value','200','400','200','300')">
                                                    <span style="color: #000099; text-decoration: underline">Use Reserved Word</span></a>--%>
                                                <a href="#" onclick="frmOpenNewWindow()" class="pull-right">
                                                    <span style="color: #000099; text-decoration: underline">Use Reserved Word</span></a>
                                        </div>
                                        <div class="col-md-4"><div id="Div" runat="server"></div></div>
                                        <div class="clear"></div>
                                        <div class="col-md-6 relative">
                                             <label>Message Footer:</label>
                                            <asp:TextBox ID="txtMessageFooter" runat="server" Font-Size="12px" Height="50px" TextMode="MultiLine"
                                                    Width="100%"></asp:TextBox>

                                                <span id="MandatoryFooter" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                <asp:CheckBox ID="chkDefaultFooter" runat="server" Font-Underline="True" Text="Use Default Footer" ForeColor="#000099" Height="50px" Width="115px" />
                                        </div>
                                        <div class="clear"></div>
                                         <div class="col-md-2">
                                             <label>Reply To:</label>
                                             <asp:TextBox ID="txtReplyTo" runat="server" Font-Size="12px" Width="100%"></asp:TextBox>
                                             <span id="MandatoryReplyTo" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                         </div>
                                        <div class="col-md-2">
                                            <label>Subscribing Email Id:</label>
                                            <asp:ListBox ID="lstItems" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="100%" CssClass="mb0 chsn hide" data-placeholder="Select ..." onchange="changeFunc();"></asp:ListBox>
                                            <asp:HiddenField ID="hndRecipients_hidden" runat="server" />
                                        </div>
                                         <div class="col-md-2">
                                             <label>Sender Display Name:</label>
                                             <asp:TextBox ID="txtSenderName" runat="server" Font-Size="12px" Width="100%"></asp:TextBox>
                                                <span id="MandatoryDisplay" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                         </div>
                                        <div class="clear"></div>
                                        <div class="col-md-2">
                                            <label>Sender Email Id:</label>
                                            <asp:TextBox ID="txtSenderEmail" runat="server" Font-Size="12px" Width="100%"></asp:TextBox>
                                                <span id="MandatoryReplyFrom" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                        </div>
                                        <div class="col-md-2">
                                            <label>Sending Option:</label>
                                            <asp:DropDownList ID="cmbSendingOption" runat="server" Width="100%" Font-Size="12px">
                                                    <asp:ListItem Value="0">As an when</asp:ListItem>
                                                    <asp:ListItem Value="1">Specific Time Of Day</asp:ListItem>
                                                </asp:DropDownList>
                                        </div>
                                        <div class="col-md-4">
                                            <label>Sending Time:</label><br />
                                            <asp:DropDownList ID="cmbhoure" runat="server" Width="59px" Font-Size="12px"></asp:DropDownList>
                                                <asp:DropDownList ID="cmbminute" runat="server" Width="59px" Font-Size="12px"></asp:DropDownList>
                                                <asp:DropDownList ID="cmbampm" runat="server" Width="59px" Font-Size="12px">
                                                    <asp:ListItem>AM</asp:ListItem>
                                                    <asp:ListItem>PM</asp:ListItem>
                                                </asp:DropDownList>
                                        </div>
                                    </div>
                                    <table class="TableMain100">
                                        
                                        
                                       
                                        <tr>
                                            <td class="gridcellright" style="width: 111px">
                                                
                                            </td>
                                            <td class="gridcellleft">
                                                </td>
                                            <td class="gridcellright" style="width: 111px">
                                                
                                            </td>
                                            <td class="gridcellleft" id="tdSendingTime">
                                                
                                            </td>
                                        </tr>
                                        <tr id="trDateTime">
                                            <td class="gridcellright" style="width: 111px">
                                                <span style="color: #000099">Use From:</span>
                                            </td>
                                            <td class="gridcellleft">
                                                <asp:TextBox ID="txtStartDate" runat="server" Font-Size="12px" TabIndex="37" Width="228px"></asp:TextBox>&nbsp;<asp:Image
                                                    ID="imgStartDate" runat="server" ImageUrl="~/images/calendar.jpg" />

                                                <span id="MandatoryStartDate" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </td>
                                            <td class="gridcellright" style="width: 111px">
                                                <span style="color: #000099">Use Until:</span>
                                            </td>
                                            <td class="gridcellleft">
                                                <asp:TextBox ID="txtEndDate" runat="server" Font-Size="12px" TabIndex="37" Width="223px"></asp:TextBox>&nbsp;<asp:Image
                                                    ID="imgEnadDate" runat="server" ImageUrl="~/images/calendar.jpg" />

                                                <span id="MandatoryEndDate" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td class="gridcellright" style="width: 111px"></td>
                                            <td colspan="2" class="gridcellleft">
                                                <asp:ListBox ID="ListBoxUserAll" runat="server" Font-Size="12px" Height="64px" Width="250px" OnSelectedIndexChanged="ListBoxUserAll_SelectedIndexChanged"></asp:ListBox>
                                                <a id="A3" href="javascript:void(0);" onclick="btnRemoveToList_click()"><span style="color: #000099; text-decoration: underline">Remove</span></a></td>
                                            <td class="gridcellleft" style="vertical-align: bottom"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <input id="btnSave" type="button" value="Save" class="btn btn-primary" onclick="btnSave_click()" />
                                                <input id="btnupdate" type="button" value="Save" class="btn btn-primary" onclick="btnUpdate_click()" />
                                                <input id="Button1" type="button" value="Cancel" class="btn btn-danger" onclick="btnCalcel_click()" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr id="FormSubscription">
                <td>
                    <table class="TableMain100">
                        <tr>
                            <td class="gridcellright" style="width: 78px; vertical-align: top;">
                                <span style="color: #000099">Subscribe User:</span>
                            </td>
                            <td class="gridcellleft" style="vertical-align: top;">
                                <asp:TextBox ID="txtsubscriptionID" runat="server" Font-Size="12px" Width="237px"></asp:TextBox>
                                <asp:TextBox ID="txtsubscriptionID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span style="color: #000099; text-decoration: underline">Add to List</span></a>
                            </td>
                            <td class="gridcellright" style="text-align: left">
                                <asp:ListBox ID="lstSuscriptions" runat="server" Font-Size="12px" Height="64px" Width="250px"></asp:ListBox>
                                <a id="A1" href="javascript:void(0);" onclick="btnRemovefromsubscriptionlist_click()"><span style="color: #000099; text-decoration: underline">Remove</span></a>
                            </td>

                        </tr>
                        <tr>
                            <td colspan="2"></td>
                            <td class="gridcellleft">
                                <input id="btnSaveSubscription" type="button" value="Save" class="btn btn-primary" onclick="btnSaveSubscription_click()" />
                                <input id="btnCancelSubscription" type="button" value="Cancel" class="btn btn-danger" onclick="btnCalcel_click()" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
