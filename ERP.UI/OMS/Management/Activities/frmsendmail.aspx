<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                30-03-2023        2.0.36           Pallab              25768: CRM pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Activities.management_activities_frmsendmail" CodeBehind="frmsendmail.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #panel1 {
            padding:0 15px 15px 15px;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function VisibleServer() {
            document.getElementById("FormServerFile").style.display = 'none';
        }
        function GetServerFilePath(path) {

            //document. getElementById("lblServerFile").innerText = path;
            var senddata = "AttachServer";
            var senddata = "AttachServer~" + path;
            //CallServer(senddata,"");
            gridServer.PerformCallback(senddata);
            document.getElementById("FormServerFile").style.display = 'inline';

        }

        function OnGridServerSelectAll(obj) {
            OnGridServerSelectionChanged();
        }
        function OnGridServerSelectionChanged() {
            gridServer.GetSelectedFieldValues('Serverfilename', OnGridServerSelectionComplete);
        }

        function OnGridServerSelectionComplete(values) {
            counterServer = 'n';
            for (var i = 0; i < values.length; i++) {
                if (counterServer != 'n')
                    counterServer += ',' + values[i];
                else
                    counterServer = values[i];
            }
            //alert(counterLocal);
        }
        function btnCancelAttachServer_click() {
            document.getElementById("FormServerFile").style.display = 'none';
            //document.getElementById("FormLocalFile").style.display = 'none';
            var senddata = 'canServer';
            gridServer.PerformCallback(senddata);
        }
        function btnRemoveAttachServer_click() {

            var senddata = 'remvServer~' + counterServer;
            gridServer.PerformCallback(senddata);
        }

        function SetSendDateTime() {
            var chkBOXT = document.getElementById("ChkTime");
            if (chkBOXT.checked == true) {

                document.getElementById('ASPxNextDate').style.display = "inline";
                document.getElementById('ASPxNextDate').style.display = "table-row";

            }
            else
                document.getElementById('ASPxNextDate').style.display = "none";
                
        }
        function PageLoad() {

            document.getElementById('ASPxNextDate').style.display = "none";

        }

    </script>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>


    <script language="javascript" type="text/javascript">

        //function is called on changing Selection
        function OnGridServerSelectAll(obj) {

            OnGridSelectionChanged();
        }
        function OnGridSelectionChanged() {
            //        var noofrow=grid.GetSelectedRowCount().toString();
            //        alert(noofrow);
            gridServer.GetSelectedFieldValues('FilePath', OnGridSelectionComplete);
        }
        function OnGridSelectionComplete(values) {
            counter = 'n';
            for (var i = 0; i < values.length; i++) {
                if (counter != 'n')
                    counter += ',' + values[i];
                else
                    counter = values[i];
            }
        }

        function OnGridLocalSelectAll(obj) {
            OnGridLocalSelectionChanged();
        }
        function OnGridLocalSelectionChanged() {
            //        var noofrow=gridLocal.GetSelectedRowCount().toString();
            //        alert(noofrow);
            gridLocal.GetSelectedFieldValues('filepathServer', OnGridLocalSelectionComplete);
        }
        function OnGridLocalSelectionComplete(values) {
            counterLocal = 'n';
            for (var i = 0; i < values.length; i++) {
                if (counterLocal != 'n')
                    counterLocal += ',' + values[i];
                else
                    counterLocal = values[i];
            }
            //alert(counterLocal);
        }
        //________End here on changing focused row
        function GroupChecked() {
            document.getElementById("trGroupWise").style.display = 'inline';
            document.getElementById("trGroupWise").style.display = 'table-row';
            document.getElementById("trContactWise").style.display = 'none';
            document.getElementById("TR1").style.display = 'none';
            document.getElementById("TR2").style.display = 'none';
            document.getElementById("TR3").style.display = 'none';
            document.getElementById("TR4").style.display = 'none';
            document.getElementById("TR5").style.display = 'none';
        }
        function contactChecked() {
            document.getElementById("trContactWise").style.display = 'inline';
            document.getElementById("trContactWise").style.display = 'table-row';
            document.getElementById("trGroupWise").style.display = 'none';
            document.getElementById("TR1").style.display = 'none';
            document.getElementById("TR2").style.display = 'none';
            document.getElementById("TR3").style.display = 'none';
            document.getElementById("TR4").style.display = 'none';
            document.getElementById("TR5").style.display = 'none';
        }
        function specificChecked() {
            document.getElementById("trContactWise").style.display = 'none';
            document.getElementById("trGroupWise").style.display = 'none';
            document.getElementById("TR1").style.display = 'inline';
            document.getElementById("TR1").style.display = 'table-row';

            document.getElementById("TR2").style.display = 'inline';
            document.getElementById("TR2").style.display = 'table-row';

            document.getElementById("TR3").style.display = 'inline';
            document.getElementById("TR3").style.display = 'table-row';

            document.getElementById("TR4").style.display = 'inline';
            document.getElementById("TR4").style.display = 'table-row';

            document.getElementById("TR5").style.display = 'inline';
            document.getElementById("TR5").style.display = 'table-row';
        }
        function AttachmentCall() {
            var chkBOX = document.getElementById("rdbSpecific");
            if (chkBOX.checked == true) {
                specificChecked();
            }
            else
                contactChecked();
        }
    </script>

    <script type="text/javascript" language="javascript">
        function AtTheTimePageLoad() {
            //alert('t');
            counterLocal = 'n';
            counter = 'n';
            FieldName = 'cmbFrom';
            document.getElementById("FormServerFile").style.display = 'none';
            document.getElementById("FormLocalFile").style.display = 'none';
            document.getElementById("txtUserId_hidden").style.visibility = 'hidden';
        }
        function ServerLocalCall(obj) {
            FieldName = 'cmbFrom';
            document.getElementById("txtUserId_hidden").style.visibility = 'hidden';
            if (obj == 'LocalCall') {
                document.getElementById("FormServerFile").style.display = 'none';
                document.getElementById("FormLocalFile").style.display = '';
            }
            if (obj == 'ServerCall') {
                document.getElementById("FormServerFile").style.display = 'none';
                document.getElementById("FormLocalFile").style.display = 'none';
            }
        }
        function btnCompose_click() {

        }
        function AddEmailID(mailid, cntid, EType) {
            var textlstmail = document.getElementById("txtlist");
            if (textlstmail.value == '')
                textlstmail.value = mailid + '-' + cntid + '-' + EType;
            else
                textlstmail.value += ',' + mailid + '-' + cntid + '-' + EType;


        }
        function btnAdd_click() {
            var texcntid = document.getElementById("txtUserId_hidden");
            var textmail = document.getElementById("txtUserId");
            var textmaillist = document.getElementById("txtUsermailIDs");
            if (textmail.value != '') {
                var filter = /^.+@.+\..{2,3}$/;
                var data = textmail.value;
                var idWname = data.split('<');
                var idlength = idWname.length;
                var mailID = '';
                if (idlength == '1') {
                    mailID = idWname[0];
                }
                else {
                    mailID = idWname[1].substring(0, idWname[1].length - 1);
                }
                //alert(mailID);
                if (filter.test(mailID)) {
                    if (texcntid.value != '') {
                        var EType = "TO";
                        AddEmailID(mailID, texcntid.value, EType);
                    }
                    if (textmaillist.value == '')
                        textmaillist.value = textmail.value;
                    else
                        textmaillist.value += ',' + textmail.value;

                    textmail.value = '';
                    texcntid.value = '';
                }
                else {
                    alert('Input valid E-mail ID!');
                    textmail.value = '';
                }
            }
            else
                alert('Please fill ID first then Add to List of ID!');
        }

        function btnAddCC_click() {
            var texcntid = document.getElementById("txtUserId_hidden");
            var textmail = document.getElementById("txtUserId");
            var textmaillist = document.getElementById("txtCC");
            if (textmail.value != '') {
                var data = textmail.value;
                var idWname = data.split('<');
                var idlength = idWname.length;
                var mailID = '';
                if (idlength == '1') {
                    mailID = idWname[0];
                }
                else {
                    mailID = idWname[1].substring(0, idWname[1].length - 1);
                }

                if (texcntid.value != '') {
                    var EType = "CC";
                    AddEmailID(mailID, texcntid.value, EType);
                }
                if (textmaillist.value == '')
                    textmaillist.value = textmail.value;
                else
                    textmaillist.value += ',' + textmail.value;

                textmail.value = '';
                texcntid.value = '';
            }
            else
                alert('Please fill E-mail ID first then Add to respective List of ID!');
        }
        function btnAddBCC_click() {
            var texcntid = document.getElementById("txtUserId_hidden");
            var textmail = document.getElementById("txtUserId");
            var textmaillist = document.getElementById("txtBCc");
            if (textmail.value != '') {
                var data = textmail.value;
                var idWname = data.split('<');
                var idlength = idWname.length;
                var mailID = '';
                if (idlength == '1') {
                    mailID = idWname[0];
                }
                else {
                    mailID = idWname[1].substring(0, idWname[1].length - 1);
                }

                if (texcntid.value != '') {
                    var EType = "BC";
                    AddEmailID(mailID, texcntid.value, EType);
                }
                if (textmaillist.value == '')
                    textmaillist.value = textmail.value;
                else
                    textmaillist.value += ',' + textmail.value;

                textmail.value = '';
                texcntid.value = '';
            }
            else
                alert('Please fill ID first then Add to List of ID!');
        }
        function btnServerAtt_click() {
            var url = 'frmAttchServerFile.aspx';
            OnMoreInfoClick(url, "Add Server Files.", '940px', '450px', "Y");
            //        document.getElementById("FormServerFile").style.display = 'inline';
            //        document.getElementById("FormLocalFile").style.display = 'none';
            //        document.getElementById("TRServerGrid").style.display = 'none';
            //        CallServer("ServerCall","");
            //        var url='frmAttchServerFile.aspx?id='+keyValue;
            //        OnMoreInfoClick(url,"Modify Lead Details",'940px','450px',"Y");
        }
        function btnLocalAtt_click() {
            // document.getElementById("FormServerFile").style.display = 'none';
            document.getElementById("FormLocalFile").style.display = '';
            document.getElementById("TRLocalGrid").style.display = 'none';
            document.getElementById("TRServerGrid").style.display = 'none';
            CallServer("LocalCall", "");
        }


        function btnCancelAttach_click() {
            //  document.getElementById("FormServerFile").style.display = 'none';
            document.getElementById("FormLocalFile").style.display = 'none';
            grid.PerformCallback('cancel');
        }
        function btnremoveAttach_click() {

        }
        function btnAddLocal_click() {
            var fileupload = document.getElementById("FileUpload1");
            document.getElementById("TRLocalGrid").style.display = 'inline';
            var filename = fileupload.value;
            var file = filename.split('\\');
            var length = file.length;
            var data = 'addLocal' + '~' + filename + '~' + file[length - 1];
            //alert(filename);
            var senddata = "AttachLocal~" + filename;
            CallServer(senddata, "");
            gridLocal.PerformCallback(data);
        }
        function btnCancelAddLocal_click() {

            //document.getElementById("FormServerFile").style.display = 'inline';
            document.getElementById("FormLocalFile").style.display = 'none';
        }

        function btnCancelAttachLocal_click() {

            // document.getElementById("FormServerFile").style.display = 'none';
            document.getElementById("FormLocalFile").style.display = 'none';
            var senddata = 'Canloc';
            gridLocal.PerformCallback(senddata);
        }
        function btnRemoveAttachLocal_click() {
            var senddata = 'remvloc~' + counterLocal;
            gridLocal.PerformCallback(senddata);
        }

        //    function btnCancelMail_click()
        //    {
        //    
        //    }
        function btnSearch_click() {
            var textboxDocu = document.getElementById("txtName");
            if (textboxDocu.value != '') {
                document.getElementById("TRServerGrid").style.display = 'inline';
                var combo = document.getElementById("drpDocumentEntity");
                var combo1 = document.getElementById("drpDocumentType");
                if (combo1.value == '') {
                    alert('Please select Document Type!');
                    return false;
                }
                var chek = document.getElementById("chkSearch");
                FieldName = 'cmbTemplate';
                var data = 'search~' + combo.value + '~' + combo1.value + '~' + chek.checked + '~' + textboxDocu.value;
                //CallServer(data,"");
                //alert(data);
                gridServer.PerformCallback(data);
                //alert('C');
            }
            else
                alert('Please Fill Document Name!');
        }

    </script>

    <script type="text/ecmascript">
        function callDoc() {
            var combo = document.getElementById("drpDocumentEntity");
            var data = 'Combo~' + combo.value;
            //alert(data);
            CallServer(data, "");
        }

        function callTemplateChange() {
            var combo = document.getElementById("cmbTemplate");
            var messageBox = document.getElementById("txtmailMessage");
            var senddata = '';
            if (combo.value != '') {
                var mailto = document.getElementById("txtUsermailIDs");
                var mailtolist = mailto.value;
                //alert(mailtolist);
                if (mailtolist == '') {
                    alert('Please First Fill E-mail ID in Recipient!');
                    var comboTemplate = document.getElementById("cmbTemplate");
                    comboTemplate.selectedIndex = 0;
                    return false;
                }

                var maillistarray = mailtolist.split(',');
                //alert(maillistarray.length);
                //return false;
                if (maillistarray.length == 1) {
                    senddata = 'template~' + mailto.value + '~' + combo.value;
                }
                else {
                    senddata = 'template~all~' + combo.value;

                }

                CallServer(senddata, "");
            }
            else {
                messageBox.value = '';
            }
        }

        function btnSendmail_click() {
            var messageBox = document.getElementById("txtmailMessage");
            var chkBOX = document.getElementById("rdbSpecific");
            var Subject = document.getElementById("txtSubject");
            var comboTemplate = document.getElementById("cmbTemplate");
            var textlstmail = document.getElementById("txtlist");
            if (Subject.value == '') {
                alert('Please Fill Subject!');
                return false;
            }
            if (chkBOX.checked == true) {
                var mailto = document.getElementById("txtUsermailIDs");
                var mailCC = document.getElementById("txtCC");
                var mailBCc = document.getElementById("txtBCc");
                var chkBOXT = document.getElementById("ChkTime");
                var sentDt = ASPxNextDate.GetText();
                if (mailto.value == '') {
                    alert('Please Fill E-mail Id of Recipient!');
                    return false;
                }

                if (chkBOXT.checked == true) {
                    var senddata = "sendmail~" + mailto.value + '~' + messageBox.value + '~' + Subject.value + '~' + mailCC.value + '~' + mailBCc.value + '~' + comboTemplate.value + '~' + textlstmail.value + '~' + sentDt;
                    CallServer(senddata, "");
                }
                else {
                    var senddata = "sendmail~" + mailto.value + '~' + messageBox.value + '~' + Subject.value + '~' + mailCC.value + '~' + mailBCc.value + '~' + comboTemplate.value + '~' + textlstmail.value + '~' + '';
                    CallServer(senddata, "");
                }

            }
            chkBOX = document.getElementById("rdbContact");
            if (chkBOX.checked == true) {
                var chkBOXT = document.getElementById("ChkTime");
                var sentDt = ASPxNextDate.GetText();
                var cmb = document.getElementById("drpContactWise");

                if (chkBOXT.checked == true) {
                    var senddata = "sendmailGroup~" + cmb.value + '~' + messageBox.value + '~' + Subject.value + '~' + comboTemplate.value + '~' + sentDt;
                    CallServer(senddata, "");
                }
                else {
                    var senddata = "sendmailGroup~" + cmb.value + '~' + messageBox.value + '~' + Subject.value + '~' + comboTemplate.value + '~' + '';
                    CallServer(senddata, "");
                }
            }
            chkBOX = document.getElementById("rdbGroup");
            if (chkBOX.checked == true) {
                var chkBOXT = document.getElementById("ChkTime");
                var GroupMaster = document.getElementById("txtGroup_hidden");
                var GroupName = document.getElementById("txtGroup");
                var sentDt = ASPxNextDate.GetText();
                if (GroupName.value == '') {
                    alert('Please Select Group!');
                    return false;
                }
                if (chkBOXT.checked == true) {
                    var senddata = "sendGroupwise~" + GroupName.value + '~' + messageBox.value + '~' + Subject.value + '~' + comboTemplate.value + '~' + GroupMaster.value + '~' + sentDt;
                    CallServer(senddata, "");
                }
                else {
                    var senddata = "sendGroupwise~" + GroupName.value + '~' + messageBox.value + '~' + Subject.value + '~' + comboTemplate.value + '~' + GroupMaster.value + '~' + '';
                    CallServer(senddata, "");
                }
            }
        }
        function btnAttach_click() {

        }
        function btnAttachLocal_click() {
            var senddata = "AttachLocal";
            CallServer(senddata, "");
        }



        function ReceiveServerData(rValue) {
            var DATA = rValue.split('~');
            //alert(rValue); 
            if (DATA[0] == "Combo") {
                var combo = document.getElementById("drpDocumentType");
                combo.length = 0;
                var NoItems = DATA[1].split(';');
                var i;
                for (i = 0; i < NoItems.length; i++) {
                    var items = NoItems[i].split(',');
                    var opt = document.createElement("option");
                    opt.text = items[1];
                    opt.value = items[0];
                    combo.options.add(opt);
                }
            }
            if (DATA[0] == "template") {
                if (DATA[1] == 'Y') {
                    var messageBox = document.getElementById("txtmailMessage");
                    messageBox.value = DATA[2];
                }
                else
                    alert(DATA[1]);
            }
            if (DATA[0] == "sendmail") {
                if (DATA[1] == 'Y') {
                    alert(DATA[2]);
                    // AtTheTimePageLoad();
                    var messageBox = document.getElementById("txtmailMessage");
                    messageBox.value = '';
                    var mailto = document.getElementById("txtUsermailIDs");
                    mailto.value = '';
                    var mailCC = document.getElementById("txtCC");
                    mailCC.value = '';
                    var mailBCc = document.getElementById("txtBCc");
                    mailBCc.value = '';
                    var Subject = document.getElementById("txtSubject");
                    Subject.value = '';
                    var comboTemplate = document.getElementById("cmbTemplate");
                    comboTemplate.selectedIndex = 0;
                    var textlstmail = document.getElementById("txtlist");
                    textlstmail.value = ''

                }
                else
                    alert(DATA[1]);
            }
        }

    </script>

    <script type="text/javascript" language="javascript">
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
        function callAjaxDoc(obj1, obj2, obj3) {
            var ob = document.getElementById("drpDocumentEntity");
            var ob1 = document.getElementById("drpDocumentType");
            var ob2 = document.getElementById("chkSearch");
            var set_value = ob.value + '~' + ob1.value + '~' + ob2.checked;
            FieldName = 'cmbTemplate';
            ajax_showOptions(obj1, obj2, obj3, set_value);

        }

        function callAjaxGroup(obj1, obj2, obj3) {
            FieldName = 'cmbTemplate';
            ajax_showOptions(obj1, obj2, obj3);

        }

    </script>
    <style>
        #txtUserId_hidden {
            display:none;
        }
    </style>

    <style>
        /*Rev 1.0*/

        select
        {
            height: 30px !important;
            border-radius: 4px !important;
            /*-webkit-appearance: none;
            position: relative;
            z-index: 1;*/
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , .dxeTextBox_PlasticBlue
        {
            height: 30px;
            border-radius: 4px;
        }

        .dxeButtonEditButton_PlasticBlue
        {
            background: #094e8c !important;
            border-radius: 4px !important;
            padding: 0 4px !important;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        .simple-select::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 26px;
            right: 13px;
            font-size: 16px;
            transform: rotate(269deg);
            font-weight: 500;
            background: #094e8c;
            color: #fff;
            height: 18px;
            display: block;
            width: 26px;
            /* padding: 10px 0; */
            border-radius: 4px;
            text-align: center;
            line-height: 19px;
            z-index: 0;
        }
        .simple-select {
            position: relative;
        }
        .simple-select:disabled::after
        {
            background: #1111113b;
        }
        select.btn
        {
            padding-right: 10px !important;
        }

        /*select.btn
        {
            padding-right: 10px !important;
        }*/

        /*.panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }*/

        .dxpLite_PlasticBlue .dxp-current
        {
            background-color: #1b5ea4;
            padding: 3px 5px;
            border-radius: 2px;
        }

        #accordion {
            margin-bottom: 20px;
            margin-top: 10px;
        }

        .dxgvHeader_PlasticBlue {
    background: #1b5ea4 !important;
    color: #fff !important;
}
        #ShowGrid
        {
            margin-top: 10px;
        }

        .pt-25{
                padding-top: 25px !important;
        }

        .dxgvEditFormDisplayRow_PlasticBlue td.dxgv, .dxgvDataRow_PlasticBlue td.dxgv, .dxgvDataRowAlt_PlasticBlue td.dxgv, .dxgvSelectedRow_PlasticBlue td.dxgv, .dxgvFocusedRow_PlasticBlue td.dxgv
        {
            padding: 6px 6px 6px !important;
        }

        #lookupCardBank_DDD_PW-1
        {
                left: -182px !important;
        }
        .plhead a>i
        {
                top: 9px;
        }

        .clsTo
        {
            display: flex;
    align-items: flex-start;
        }

        input[type="radio"], input[type="checkbox"]
        {
            margin-right: 5px;
        }
        .dxeCalendarDay_PlasticBlue
        {
                padding: 6px 6px;
        }

        .modal-dialog
        {
            width: 50%;
        }

        .modal-header
        {
            padding: 8px 4px 8px 10px;
            background: #094e8c !important;
        }

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #EmployeeGrid , .TableMain100 #GrdEmployee,
        .TableMain100 #SalesDetailsGrid, #ShowGrid
        {
            max-width: 99% !important;
        }

        /*div.dxtcSys > .dxtc-content > div, div.dxtcSys > .dxtc-content > div > div
        {
            width: 95% !important;
        }*/

        .btn-info
        {
                background-color: #1da8d1 !important;
                background-image: none;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .dxeButtonDisabled_PlasticBlue
        {
            background: #b5b5b5 !important;
            border-color: #b5b5b5 !important;
        }

        #ddlValTech
        {
            width: 100% !important;
            margin-bottom: 0 !important;
        }

        .dis-flex
        {
            display: flex;
            align-items: baseline;
        }

        input + label
        {
            line-height: 1;
                margin-top: 7px;
        }

        .dxtlHeader_PlasticBlue
        {
            background: #094e8c !important;
        }

        .dxeBase_PlasticBlue .dxichCellSys
        {
            padding-top: 2px !important;
        }

        .pBackDiv
        {
            border-radius: 10px;
            box-shadow: 1px 1px 10px #1111112e;
        }
        .HeaderStyle th
        {
            padding: 5px;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer
        {
            padding-top: 15px;
        }

        .pt-2
        {
            padding-top: 5px;
        }
        .pt-10
        {
            padding-top: 10px;
        }

        .pt-15
        {
            padding-top: 15px;
        }

        .pb-10
        {
            padding-bottom: 10px;
        }

        .pTop10 {
    padding-top: 20px;
}
        .custom-padd
        {
            padding-top: 4px;
    padding-bottom: 10px;
        }

        input + label
        {
                margin-right: 10px;
        }

        .btn
        {
            margin-bottom: 0;
        }

        .pl-10
        {
            padding-left: 10px;
        }

        .col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }

        .devCheck
        {
            margin-top: 5px;
        }

        .mtc-5
        {
            margin-top: 5px;
        }

        .mtc-10
        {
            margin-top: 10px;
        }

        /*select.btn
        {
           position: relative;
           z-index: 0;
        }*/

        select
        {
            margin-bottom: 0;
        }

        .form-control
        {
            background-color: transparent;
        }

        select.btn-radius {
    padding: 4px 8px 6px 11px !important;
}
        .mt-30{
            margin-top: 30px;
        }

        .panel-title h3
        {
            padding-top: 0;
            padding-bottom: 0;
        }

        .btn-radius
        {
            padding: 4px 11px !important;
            border-radius: 4px !important;
        }

        .crossBtn
        {
             right: 30px;
             top: 25px;
        }

        .mb-10
        {
            margin-bottom: 10px;
        }

        .btn-cust
        {
            background-color: #108b47 !important;
            color: #fff;
        }

        .btn-cust:hover
        {
            background-color: #097439 !important;
            color: #fff;
        }

        .gHesder
        {
            background: #1b5ca0 !important;
            color: #ffffff !important;
            padding: 6px 0 6px !important;
        }

        .close
        {
             color: #fff;
             opacity: .5;
             font-weight: 400;
        }

        .mt-27
        {
            margin-top: 27px !important;
        }

        .col-md-3 , .col-md-2
        {
            margin-top: 8px;
        }

        #upldBigLogo , #upldSmallLogo
        {
            width: 100%;
        }

        #DivSetAsDefault
        {
            margin-top: 25px;
        }

        .btn.btn-xs
        {
                font-size: 14px !important;
        }

        .dxpc-content table
        {
             width: 100%;
        }

        input[type="text"], input[type="password"], textarea
        {
            margin-bottom: 0 !important;
        }
        #FromDate , #ToDate , #ASPxFromDate , #ASPxToDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #FromDate_B-1 , #ToDate_B-1 , #ASPxFromDate_B-1 , #ASPxToDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FromDate_B-1 #FromDate_B-1Img , #ToDate_B-1 #ToDate_B-1Img , #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img
        {
            display: none;
        }

        #lblToDate
        {
            padding-left: 10px;
        }

        .dxtc-activeTab:after {
            content: '';
            width: 0;
            height: 0;
            border-left: 8px solid transparent;
            border-right: 8px solid transparent;
            border-top: 9px solid #3e5395;
            position: absolute;
            /* left: 50%; */
            z-index: 3;
            /* bottom: -15px; */
            margin-left: -9px;
        }

        table td
        {
            padding-bottom: 10px !important;
        }

        #cmbContactType , #txtUserId
        {
            margin-bottom: 10px !important;
        }
        /*Rev end 1.0*/
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3><asp:Label ID="lblHeader" runat="server" ></asp:Label></h3>
        </div>

    </div>
        <div class="form_main ">

        <table class="TableMain100">
            <tr>
                <td class="gridcellleft" style="vertical-align: top; width: 70px;">
                    <table>
                        <tr>
                            <td>
                                <input id="btnCompose" type="button" value="Compose" class="btnUpdate btn btn-success" onclick="btnCompose_click();"
                                    tabindex="1" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                
                <td>
                    <asp:Panel ID="panel1" runat="server" Width="100%">
                        <table class="TableMain100">
                            <tr>
                                <td></td>
                                <td class="gridcellleft">
                                    <asp:RadioButton ID="rdbSpecific" runat="server" Checked="True" GroupName="a" />Specfic
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:RadioButton ID="rdbContact" runat="server" GroupName="a" /><asp:Label ID="lblcnt"
                                        runat="server" Text="Contact Wise"></asp:Label>
                                    <asp:RadioButton ID="rdbGroup" runat="server" GroupName="a" />Group Wise</td>
                            </tr>
                            <tr style="display: none">
                                <td></td>
                                <td class="gridcellleft">
                                    <asp:TextBox ID="txtlist" runat="server" Width="300px"></asp:TextBox>
                                    <asp:TextBox ID="txtGroup_hidden" runat="server" Width="14px"></asp:TextBox></td>
                            </tr>
                            <tr style="display: none" id="trContactWise">
                                <td></td>
                                <td class="gridcellleft">
                                    <asp:DropDownList ID="drpContactWise" runat="server" Width="214px" Font-Size="12px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="display: none" id="trGroupWise">
                                <td class="gridcellright">
                                    <span style="color: #000099">Group Name:</span></td>
                                <td class="gridcellleft">
                                    <asp:TextBox ID="txtGroup" runat="server" Font-Size="12px" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="TR1">
                                <td class="gridcellright" style="width: 111px">
                                    <span style="color: #000099">To:<strong> </strong></span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:DropDownList ID="cmbContactType" runat="server" Width="250px" Font-Size="12px">
                                    </asp:DropDownList>
                                    <%--  <asp:ListItem Value="EM">Employees</asp:ListItem>
                                        <asp:ListItem Value="CL">Customers</asp:ListItem>
                                        <asp:ListItem Value="LD">Lead</asp:ListItem>
                                        <asp:ListItem Value="CD">CDSL Client</asp:ListItem>
                                        <asp:ListItem Value="ND">NSDL Client</asp:ListItem>
                                        <asp:ListItem Value="BP">Business Partner</asp:ListItem>
                                        <asp:ListItem Value="RA">Relationship Partner</asp:ListItem>
                                        <asp:ListItem Value="SB">Sub Broker</asp:ListItem>
                                        <asp:ListItem Value="FR">Franchisees</asp:ListItem>--%>
                                    <asp:TextBox ID="txtUserId" runat="server" Font-Size="12px" Width="250px"></asp:TextBox>
                                    <asp:TextBox ID="txtUserId_hidden" runat="server" Width="14px"></asp:TextBox>
                                    <input id="btnAdd" type="button" value="Add To" class="btnUpdate btn btn-default btn-xs" onclick="btnAdd_click();"
                                         tabindex="1" />
                                    <input id="btnAddCC" type="button" value="Add Cc" class="btnUpdate btn btn-default btn-xs" onclick="btnAddCC_click();"
                                         tabindex="1" />
                                    <input id="btnAddBc" type="button" value="Add Bcc" class="btnUpdate btn btn-default btn-xs" onclick="btnAddBCC_click();"
                                         tabindex="1" />
                                </td>
                            </tr>
                            <tr id="TR2">
                                <td style="width: 111px"></td>
                                <td class="gridcellleft">
                                    <asp:TextBox ID="txtUsermailIDs" runat="server" TextMode="MultiLine" Width="90%"
                                        Font-Size="12px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="TR3">
                                <td class="gridcellright" style="width: 111px">
                                    <span style="color: #000099">Cc:<strong> </strong></span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:TextBox ID="txtCC" runat="server" TextMode="MultiLine" Width="90%" Font-Size="12px"></asp:TextBox></td>
                            </tr>
                            <tr id="TR4">
                                <td class="gridcellright" style="width: 111px">
                                    <span style="color: #000099">Bcc:<strong> </strong></span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:TextBox ID="txtBCc" runat="server" TextMode="MultiLine" Width="90%" Font-Size="12px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="gridcellright" style="width: 111px">
                                    <span style="color: #000099">From:<strong> </strong></span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:DropDownList ID="cmbFrom" runat="server" Font-Size="12px" Width="304px">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td class="gridcellright" style="width: 111px">
                                    <span style="color: #000099">Subject:<strong> </strong></span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:TextBox ID="txtSubject" runat="server" Width="300px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="gridcellright" style="width: 111px">
                                    <span style="color: #000099">Attachment:<strong> </strong></span>
                                </td>
                                <td class="gridcellleft">
                                    <input id="btnServerAttach" type="button" value="Attach Server File" class="btnUpdate btn btn-primary btn-xs"
                                        onclick="btnServerAtt_click();" tabindex="1" />

                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellright" style="width: 111px"></td>
                                <td class="gridcellleft"></td>
                            </tr>
                            <tr id="FormServerFile">
                                <td colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <td class="gridcellleft">
                                                <dxe:ASPxGridView ID="GridAttachmentServer" ClientInstanceName="gridServer" runat="server"
                                                    Width="100%" KeyFieldName="Serverfilename" AutoGenerateColumns="False" OnCustomCallback="GridAttachmentServer_CustomCallback">
                                                    <Styles>
                                                        <LoadingPanel ImageSpacing="10px">
                                                        </LoadingPanel>
                                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                        </Header>
                                                    </Styles>
                                                    <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True"
                                                        Visible="False">
                                                        <FirstPageButton Visible="True">
                                                        </FirstPageButton>
                                                        <LastPageButton Visible="True">
                                                        </LastPageButton>
                                                    </SettingsPager>
                                                    <SettingsBehavior AllowMultiSelection="True" />
                                                    <ClientSideEvents SelectionChanged="function(s, e) { OnGridServerSelectionChanged(); }" />

                                                    <Columns>
                                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="3%">
                                                            <HeaderStyle HorizontalAlign="Center">
                                                                <Paddings PaddingBottom="1px" PaddingTop="1px" />
                                                            </HeaderStyle>
                                                            <HeaderTemplate>
                                                                <input type="checkbox" onclick="gridServer.SelectAllRowsOnPage(this.checked); OnGridServerSelectAll(this.checked);"
                                                                    style="vertical-align: middle;" title="Select/Unselect all rows on the page"></input>
                                                            </HeaderTemplate>
                                                        </dxe:GridViewCommandColumn>
                                                        <dxe:GridViewDataTextColumn Caption="File Name" FieldName="Serverfilename" ReadOnly="True"
                                                            VisibleIndex="1">
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <%--  <dxe:GridViewDataTextColumn Caption="File Path" FieldName="filepath" ReadOnly="True"
                                                                        VisibleIndex="2">
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                  <dxe:GridViewDataTextColumn Caption="Server File Path" FieldName="filepathServer"
                                                                        ReadOnly="True" VisibleIndex="2">
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn> --%>
                                                    </Columns>
                                                </dxe:ASPxGridView>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="gridcellleft">

                                                <input id="btnR" type="button" value="Remove" class="btnUpdate" onclick="btnRemoveAttachServer_click();"
                                                    style="width: 62px; height: 19px" tabindex="1" />
                                                <input id="btnC" type="button" value="Cancel" class="btnUpdate" onclick="btnCancelAttachServer_click();"
                                                    style="width: 62px; height: 19px" tabindex="1" />

                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="2">
                                    <%--<table class="TableMain100">
                                        <tr>
                                            <td class="gridcellright" style="width: 111px">
                                                <span style="color: #000099">Document Entity:<strong> </strong></span>
                                            </td>
                                            <td class="gridcellleft">
                                                <asp:DropDownList ID="drpDocumentEntity" runat="server" AutoPostBack="false" Width="304px"
                                                    Font-Size="12px">
                                                    <asp:ListItem>Products MF</asp:ListItem>
                                                    <asp:ListItem>Products Insurance</asp:ListItem>
                                                    <asp:ListItem>Products IPOs</asp:ListItem>
                                                    <asp:ListItem>Customer</asp:ListItem>
                                                    <asp:ListItem>Lead</asp:ListItem>
                                                    <asp:ListItem>Employee</asp:ListItem>
                                                    <asp:ListItem>Sub Brokers</asp:ListItem>
                                                    <asp:ListItem>Franchisees</asp:ListItem>
                                                    <asp:ListItem>Data Vendors</asp:ListItem>
                                                    <asp:ListItem>Referral Agents</asp:ListItem>
                                                    <asp:ListItem>Recruitment Agents</asp:ListItem>
                                                    <asp:ListItem>AMCs</asp:ListItem>
                                                    <asp:ListItem>Insurance Companies</asp:ListItem>
                                                    <asp:ListItem>RTAs</asp:ListItem>
                                                    <asp:ListItem>Branches</asp:ListItem>
                                                    <asp:ListItem>Companies</asp:ListItem>
                                                    <asp:ListItem>Building</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="gridcellright" style="width: 111px">
                                                <span style="color: #000099">Document Type:<strong> </strong></span>
                                            </td>
                                            <td class="gridcellleft">
                                                <asp:DropDownList ID="drpDocumentType" runat="Server" Width="304px" Font-Size="12px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="gridcellright" style="width: 111px">
                                                <span style="color: #000099">Search By Document Name:<strong> </strong></span>
                                            </td>
                                            <td class="gridcellleft">
                                                <asp:CheckBox ID="chkSearch" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="gridcellright" style="width: 111px">
                                                <span style="color: #000099">Search By Name:<strong> </strong></span>
                                            </td>
                                            <td class="gridcellleft">
                                                <asp:TextBox ID="txtName" runat="Server" AutoCompleteType="Disabled" Width="300px"
                                                    Font-Size="12px"></asp:TextBox>
                                                <asp:TextBox ID="txtName_hidden" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="gridcellright" style="width: 111px">
                                            </td>
                                            <td class="gridcellleft">
                                                <input id="btnSearch" type="button" value="Search" class="btnUpdate" onclick="btnSearch_click();"
                                                    style="width: 62px; height: 19px" tabindex="1" />
                                            </td>
                                        </tr>
                                        <tr id="TRServerGrid">
                                            <td colspan="2" class="gridcellleft">
                                                <table class="TableMain100">
                                                    <tr>
                                                        <td colspan="2">
                                                            <dxe:ASPxGridView ID="GridAttachment" ClientInstanceName="gridServer" runat="server"
                                                                Width="100%" KeyFieldName="FilePath" AutoGenerateColumns="False" OnCustomCallback="GridAttachment_CustomCallback">
                                                                <Styles>
                                                                    <LoadingPanel ImageSpacing="10px">
                                                                    </LoadingPanel>
                                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                    </Header>
                                                                </Styles>
                                                                <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True"
                                                                    Visible="False">
                                                                    <FirstPageButton Visible="True">
                                                                    </FirstPageButton>
                                                                    <LastPageButton Visible="True">
                                                                    </LastPageButton>
                                                                </SettingsPager>
                                                                <SettingsBehavior AllowMultiSelection="True" AllowSort="False" />
                                                                <ClientSideEvents SelectionChanged="function(s, e) { OnGridSelectionChanged(); }" />
                                                                <Columns>
                                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="3%">
                                                                        <HeaderStyle HorizontalAlign="Center">
                                                                            <Paddings PaddingBottom="1px" PaddingTop="1px" />
                                                                        </HeaderStyle>
                                                                        <HeaderTemplate>
                                                                            <input type="checkbox" onclick="gridServer.SelectAllRowsOnPage(this.checked);OnGridServerSelectAll(this.checked);"
                                                                                style="vertical-align: middle;" title="Select/Unselect all rows on the page"></input>
                                                                        </HeaderTemplate>
                                                                    </dxe:GridViewCommandColumn>
                                                                    <dxe:GridViewDataTextColumn Caption="Document Type" FieldName="Type" ReadOnly="True"
                                                                        VisibleIndex="1">
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn Caption="Document Name" FieldName="FileName" ReadOnly="True"
                                                                        VisibleIndex="2">
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn Caption="Document Physical Location" FieldName="FilePath"
                                                                        ReadOnly="True" VisibleIndex="3">
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataHyperLinkColumn Caption="View Doc." VisibleIndex="4" Width="5%">
                                                                        <PropertiesHyperLinkEdit Target="_blank" Text="View" NavigateUrlFormatString="viewImage.aspx?id={3}">
                                                                        </PropertiesHyperLinkEdit>
                                                                    </dxe:GridViewDataHyperLinkColumn>
                                                                </Columns>
                                                            </dxe:ASPxGridView>
                                                            &nbsp; &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 111px">
                                                        </td>
                                                        <td class="gridcellleft">
                                                            <input id="btnAttach" type="button" value="Attach" class="btnUpdate" onclick="btnAttach_click();"
                                                                style="width: 62px; height: 19px" tabindex="1" />
                                                            <input id="btnCancelAttach" type="button" value="Cancel" class="btnUpdate" onclick="btnCancelAttach_click();"
                                                                style="width: 62px; height: 19px" tabindex="1" />
                                                            <input id="btnremoveAttach" type="button" value="Remove" class="btnUpdate" onclick="btnremoveAttach_click();"
                                                                style="width: 62px; height: 19px" tabindex="1" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellright" style="width: 111px">
                                    <span style="color: #000099"><strong></strong></span>
                                </td>
                                <td class="gridcellleft">

                                    <input id="btnLocalAttach" type="button" value="Attach Local File" class="btnUpdate btn btn-primary btn-xs"
                                        onclick="btnLocalAtt_click();" tabindex="1" />
                                </td>
                            </tr>
                            <tr id="FormLocalFile">
                                <td></td>
                                <td class="gridcellleft">
                                    <table class="TableMain100">
                                        <tr>
                                            <td style="width: auto">
                                                <asp:FileUpload ID="FileUpload1" runat="server" Width="314px" Font-Size="12px" />
                                                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Upload" CssClass="btnUpdate btn btn-success btn-xs mTop5"
                                                     />
                                                <input id="btnCancelAddLocal" type="button" value="Cancel" class="btnUpdate btn btn-danger btn-xs mTop5" onclick="btnCancelAddLocal_click();"
                                                     tabindex="1" />
                                            </td>
                                        </tr>
                                        <tr id="TRLocalGrid">
                                            <td colspan="2">
                                                <table class="TableMain100">
                                                    <tr>
                                                        <td>
                                                            <dxe:ASPxGridView ID="GridAttachmentLocal" ClientInstanceName="gridLocal" runat="server"
                                                                Width="100%" KeyFieldName="filepathServer" AutoGenerateColumns="False" OnCustomCallback="GridAttachmentLocal_CustomCallback">
                                                                <Styles>
                                                                    <LoadingPanel ImageSpacing="10px">
                                                                    </LoadingPanel>
                                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                    </Header>
                                                                </Styles>
                                                                <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True"
                                                                    Visible="False">
                                                                    <FirstPageButton Visible="True">
                                                                    </FirstPageButton>
                                                                    <LastPageButton Visible="True">
                                                                    </LastPageButton>
                                                                </SettingsPager>
                                                                <SettingsBehavior AllowMultiSelection="True" />
                                                                <ClientSideEvents SelectionChanged="function(s, e) { OnGridLocalSelectionChanged(); }" />

                                                                <Columns>
                                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="3%">
                                                                        <HeaderStyle HorizontalAlign="Center">
                                                                            <Paddings PaddingBottom="1px" PaddingTop="1px" />
                                                                        </HeaderStyle>
                                                                        <HeaderTemplate>
                                                                            <input type="checkbox" onclick="gridLocal.SelectAllRowsOnPage(this.checked); OnGridLocalSelectAll(this.checked);"
                                                                                style="vertical-align: middle;" title="Select/Unselect all rows on the page"></input>
                                                                        </HeaderTemplate>
                                                                    </dxe:GridViewCommandColumn>
                                                                    <dxe:GridViewDataTextColumn Caption="File Name" FieldName="filename" ReadOnly="True"
                                                                        VisibleIndex="1">
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn Caption="File Path" FieldName="filepath" ReadOnly="True"
                                                                        VisibleIndex="2">
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn Caption="Server File Path" FieldName="filepathServer"
                                                                        ReadOnly="True" VisibleIndex="2">
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                </Columns>
                                                            </dxe:ASPxGridView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="gridcellleft">
                                                            <input id="btnRemoveAttachLocal" type="button" value="Remove" class="btnUpdate btn btn-primary" onclick="btnRemoveAttachLocal_click();"
                                                                tabindex="1" />
                                                            <input id="btnCancelAttachLocal" type="button" value="Cancel" class="btnUpdate btn btn-danger" onclick="btnCancelAttachLocal_click();"
                                                                 tabindex="1" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id='TR5'>
                                <td class="gridcellright" style="width: 111px">
                                    <span style="color: #000099">Select Template:<strong> </strong></span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:DropDownList ID="cmbTemplate" runat="server" Font-Size="12px" Width="200px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellright" style="width: 111px; vertical-align: top">
                                    <span style="color: #000099">Body:<strong> </strong></span>
                                </td>
                                <td class="gridcellleft" style="vertical-align: top">
                                    <asp:TextBox ID="txtmailMessage" runat="server" TextMode="MultiLine" Width="90%"
                                        Height="200px" Font-Size="12px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellright">
                                    <%--  <input type="CheckBox" id="ChkTime" value="Send on different DateTime"  onclick="SetSendDateTime(ChkTime.GetValue);" />--%>
                                    <asp:CheckBox ID="ChkTime" runat="server" onclick="SetSendDateTime();" />
                                </td>
                                <td class="gridcellleft" style="height: 50px;">
                                    <table>
                                        <tr>
                                            <td>Send On Different Date:</td>
                                            <td>
                                                <dxe:ASPxDateEdit ID="ASPxNextDate" runat="server" EditFormat="custom" UseMaskBehavior="True" ClientInstanceName="ASPxNextDate">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                            <tr>
                                <td style="width: 111px"></td>
                                <td class="gridcellleft">
                                    <input id="btnSendmail" type="button" value="Send Mail" class="btnUpdate btn btn-primary" onclick="btnSendmail_click();"
                                         tabindex="1" />
                                    <%-- <input id="btnCancelMail" type="button" value="Cancel" class="btnUpdate" onclick="btnCancelMail_click();"
                                        style="width: 62px; height: 19px" tabindex="1" />&nbsp;--%>
                                    <asp:Button ID="btnCancelall" runat="server" class="btnUpdate btn btn-danger" Text="Cancel" OnClick="btnCancelall_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    </div>
</asp:Content>
