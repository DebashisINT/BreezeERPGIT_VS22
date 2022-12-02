<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_utilities_welcome" CodeBehind="welcome.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
        function popupClose2() {
            //         document.getElementById('groupid').value="";
            //         rbgrp.SetValue('A');
            editwin.close();
        }
        function ActiveBtn(obj) {
            //            if(obj.substr(0,3) == "btn")
            //            {
            document.getElementById(obj).removeAttribute("btnUpdate");
            document.getElementById(obj).className = "btnPinkActive";
            //}            
        }
        function InActiveBtn(obj) {
            //             if(obj.substr(0,3) == "btn")
            //            {
            document.getElementById(obj).removeAttribute("btnPinkActive");
            document.getElementById(obj).className = "btnUpdate";
            //}            
        }
        function ShowError(obj) {
        }
    </script>

    <script type="text/javascript">


        FieldName = 'btnFilter';
        //function is called on changing focused row
        //    function OnGridFocusedRowChanged() {
        //        // Query the server for the Row ID "Rid" fields from the focused row 
        //        // The values will be returned to the OnGetRowValues() function     
        //        grid.GetRowValues(grid.GetFocusedRowIndex(), 'Rid', OnGetRowValues);
        //        //alert();
        //    }
        //    //Value array contains Row ID "Rid" field values returned from the server 
        //    function OnGetRowValues(values) {
        //        RowID = values;
        //        //alert(RowID);
        //        //GridForR = document.getElementById("GridReminder");
        //    }
        //________End here on changing focused row
        //////////////////////////////////////////
        function OnGridFocusedRowChanged() {
            //        var noofrow=grid.GetSelectedRowCount().toString();
            //        alert(noofrow);
            grid.GetSelectedFieldValues('Rid', OnGetRowValues);

        }
        function OnGetRowValues(values) {
            //     cbAll.checked = true;
            //        cbAll.SetChecked=true;
            //RowID = values;

            RowID = 'n';
            for (var j = 0; j < values.length; j++) {
                if (RowID != 'n')
                    RowID += ',' + values[j];
                else
                    RowID = values[j];
                //alert (RowID);
            }
            //alert(counter+'test');
        }



        /////////////////////////////////////////////

        function OnGridSelectionChanged() {
            //        var noofrow=grid.GetSelectedRowCount().toString();
            //        alert(noofrow);
            gridM.GetSelectedFieldValues('Mid', OnGridSelectionComplete);
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
        function OnAllCheckedChanged(s, e) {

            if (s.GetChecked()) {

                grid.SelectRows();

            }

            else {

                grid.UnselectRows();
            }

        }

        function OnGridSelectionChanged(s, e) {

            cbAll.SetChecked(s.GetSelectedRowCount() == s.cpVisibleRowCount);
            alert(cpVisibleRowCount);


        }
        var _handle = true;



        function OnPageCheckedChanged(s, e) {

            _handle = false;

            if (s.GetChecked())

                GridReminder.SelectAllRowsOnPage();

            else

                GridReminder.UnselectAllRowsOnPage();

        }
        var _selectNumber = 0;  // the number of selected rows within the page



        function OnGridSelectionChanged(s, e) {

            cbAll.SetChecked(s.GetSelectedRowCount() == s.cpVisibleRowCount);

            if (e.isChangedOnServer == false) {
                if (e.isAllRecordsOnPage && e.isSelected)
                    _selectNumber = s.GetVisibleRowsOnPage();
                else if (e.isAllRecordsOnPage && !e.isSelected)
                    _selectNumber = 0;
                else if (!e.isAllRecordsOnPage && e.isSelected)
                    _selectNumber++;
                else if (!e.isAllRecordsOnPage && !e.isSelected)
                    _selectNumber--;

                if (_handle) {
                    cbPage.SetChecked(_selectNumber == s.GetVisibleRowsOnPage());
                    _handle = false;
                }
                _handle = true;

            }
            else {
                cbPage.SetChecked(cbAll.GetChecked());

            }
        }



        function OnGridEndCallback(s, e) {

            _selectNumber = s.cpSelectedRowsOnPage; // get the number of selected rows within the page
            alert(_selectNumber);

        }
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
        function btnPending_click() {
            document.getElementById("FormId").style.display = 'none';
            document.getElementById("filterForm").style.display = 'none';
            document.getElementById("ShowFilter").style.display = 'inline';
            document.getElementById("Td1").style.display = 'inline';

            document.getElementById("filterForm1").style.display = 'inline';
            document.getElementById("filterForm2").style.display = 'none';
            document.getElementById("btnAttend").style.visibility = 'visible';
            document.getElementById("FormGrid").style.display = 'inline';
            document.getElementById("tdtxtincharge").style.display = 'none';
            var SelectedDate = new Date('<%=Session["fromdate"]%>');
            txtStart1.SetDate(new Date(SelectedDate));
            var SelectedDate1 = new Date('<%=Session["todate"]%>');
            txtEnd1.SetDate(new Date(SelectedDate1));
            grid.PerformCallback('Pending');
            ActiveBtn("Pending");
            InActiveBtn("btnCreate");
            InActiveBtn("All");
            InActiveBtn("btnshow");
            InActiveBtn("Attended");
            InActiveBtn("btnEdit");
            InActiveBtn("btnDelete");
            InActiveBtn("Today");
        }
        function btnAll_click() {
            document.getElementById("FormId").style.display = 'none';
            document.getElementById("filterForm").style.display = 'none';
            document.getElementById("filterForm1").style.display = 'none';
            document.getElementById("filterForm2").style.display = 'inline';
            document.getElementById("btnAttend").style.visibility = 'visible';
            document.getElementById("FormGrid").style.display = 'inline';
            document.getElementById("ShowFilter").style.display = 'inline';
            document.getElementById("tdtxtincharge").style.display = 'none';
            document.getElementById("Td1").style.display = 'inline';
            var SelectedDate = new Date('<%=Session["fromdate"]%>');
        txtStart2.SetDate(new Date(SelectedDate));
        var SelectedDate1 = new Date('<%=Session["todate"]%>');
            txtEnd2.SetDate(new Date(SelectedDate1));
            grid.PerformCallback('All');
            ActiveBtn("All");
            InActiveBtn("btnCreate");
            InActiveBtn("Pending");
            InActiveBtn("btnshow");
            InActiveBtn("Attended");
            InActiveBtn("btnEdit");
            InActiveBtn("btnDelete");
            InActiveBtn("Today");
        }
        function btnToday_click() {
            document.getElementById("filterForm").style.display = 'none';
            document.getElementById("filterForm1").style.display = 'none';
            document.getElementById("filterForm2").style.display = 'none';
            document.getElementById("FormId").style.display = 'none';
            document.getElementById("btnAttend").style.visibility = 'visible';
            document.getElementById("FormGrid").style.display = 'inline';
            document.getElementById("Tr1").style.display = 'none';
            document.getElementById("tr_button").style.display = 'none';
            document.getElementById("ShowFilter").style.display = 'inline';
            document.getElementById("Td1").style.display = 'inline';
            document.getElementById("tdtxtincharge").style.display = 'none';
            document.getElementById("tdtxtincharge").style.display = 'none';
            ActiveBtn("Today");
            InActiveBtn("btnCreate");
            InActiveBtn("Pending");
            InActiveBtn("btnshow");
            InActiveBtn("Attended");
            InActiveBtn("btnEdit");
            InActiveBtn("btnDelete");
            InActiveBtn("All");
            grid.PerformCallback('Today');
            counter = 'n';
            document.getElementById("trRepluForm").style.display = 'none';
            FieldName = 'btnFilter';

        }
        function btnAttended_click() {
            document.getElementById("filterForm").style.display = 'inline';
            document.getElementById("filterForm1").style.display = 'none';
            document.getElementById("FormId").style.display = 'none';
            document.getElementById("filterForm2").style.display = 'none';
            document.getElementById("btnAttend").style.visibility = 'hidden';
            document.getElementById("FormGrid").style.display = 'inline';
            document.getElementById("ShowFilter").style.display = 'inline';
            document.getElementById("Td1").style.display = 'inline';
            document.getElementById("tdtxtincharge").style.display = 'none';
            var SelectedDate = new Date('<%=Session["fromdate"]%>');
        txtStart.SetDate(new Date(SelectedDate));
        var SelectedDate1 = new Date('<%=Session["todate"]%>');
            txtEnd.SetDate(new Date(SelectedDate1));
            grid.PerformCallback('Attended');
            ActiveBtn("Attended");
            InActiveBtn("btnCreate");
            InActiveBtn("Pending");
            InActiveBtn("btnshow");
            InActiveBtn("Today");
            InActiveBtn("btnEdit");
            InActiveBtn("btnDelete");
            InActiveBtn("All");
        }
        function btnFilter_click() {
            var startFilterdate = document.getElementById("txtStart");
            var endFilaterdate = document.getElementById("txtEnd");
            var stdate = startFilterdate.value;
            var enddate = endFilaterdate.value;

            if (stdate != "" && enddate != "") {
                //alert(stdate + '--' + enddate);
                grid.PerformCallback('Filter');
            }
            else {
                alert('Please Select Appropriate Date !!');
            }
            //CallServer('Today', "");
            //alert(RowID);
            //hidden-
        }
        function ShowHideFilter(obj) {
          grid.PerformCallback(obj);
            //alert(obj);
            //height();
        }
        function frmOpenNewWindow1(location, v_height, v_weight) {
            var y = (screen.availHeight - v_height) / 2;
            var x = (screen.availWidth - v_weight) / 2;
            editwin = dhtmlmodal.open("Editbox", "iframe", location, "Memberdetail", "width=800px,height=400px,center=1,resize=1,top=0", "recal");

            editwin.onclose = function () {
                grid.PerformCallback();
            }
        }
        function ParentWindowShow(objURL, title) {
            OnMoreInfoClick(objURL, title, '940px', '450px', "Y");
        }
        function OnMoreInfoClick1(url, HeaderText, Width, Height, anyKey) //AnyKey will help to call back event to child page, if u have to fire more that one function 
        {
            editwin = dhtmlmodal.open("Editbox", "iframe", url, HeaderText, "width=" + Width + ",height=" + Height + ",center=1,resize=1,scrolling=2,top=500", "recal")
            editwin.onclose = function () {
                if (anyKey == 'Y') {
                    document.getElementById('IFRAME_ForAllPages').contentWindow.callback();
                }
            }
        }
        function selectsubject() {
            var URL = 'subjectselection_reminder.aspx';
            //frmOpenNewWindow1(url,540,350);
            //OnMoreInfoClick(url,"Record not available for  " ,'650px','500px',"Y");
            //ParentWindowShow(URL,'Charge Groups');
            window.open('subjectselection_reminder.aspx', '50', 'resizable=1,height=500px,width=500px');
            //window.open(location,"template","height="+ v_height +",width="+ v_weight +",top="+ x +",left="+ y +",location=no,directories=no,menubar=no,toolbar=no,status=yes,scrollbars=yes,resizable=no,dependent=no'");       
            //grid.PerformCallback(obj);
        }
        function btnFilter2_click() {
            var startFilterdate = document.getElementById("txtStart2");
            var endFilaterdate = document.getElementById("txtEnd2");
            var stdate = startFilterdate.value;
            var enddate = endFilaterdate.value;

            if (stdate != "" && enddate != "") {
                //alert(stdate + '--' + enddate);
                grid.PerformCallback('Filter2');
            }
            else {
                alert('Please Select Appropriate Date !!');
            }
            //CallServer('Today', "");
            //alert(RowID);
            //hidden-
        }
        function btnFilter1_click() {
            var startFilterdate = document.getElementById("txtStart1");
            var endFilaterdate = document.getElementById("txtEnd1");
            var stdate = startFilterdate.value;
            var enddate = endFilaterdate.value;

            if (stdate != "" && enddate != "") {
                //alert(stdate + '--' + enddate);
                grid.PerformCallback('Filter1');
            }
            else {
                alert('Please Select Appropriate Date !!');
            }
            //CallServer('Today', "");
            //alert(RowID);
            //hidden-
        }
        function btnAttend_click() {
            document.getElementById("filterForm").style.display = 'none';
            document.getElementById("filterForm1").style.display = 'none';
            document.getElementById("FormId").style.display = 'none';
            document.getElementById("FormGrid").style.display = 'inline';
            document.getElementById("filterForm2").style.display = 'none';
            document.getElementById("ShowFilter").style.display = 'none';
            document.getElementById("Td1").style.display = 'none';
            var data = 'Attend';
            var combo = document.getElementById("td_reply").value;
            var attnd = document.getElementById("ddlattend").value;
            // combo = document.getElementById("td_reply")  ;
            //            if(combo.value != "")
            //            {
            //                data +=+ '~'+ RowID+ '~' + combo.value;
            //            }
            // CallServer(data, ""); 
            CallServer('Attend' + '~' + RowID + '~' + combo + '~' + attnd, "");
            //alert('btnAttend_click');
            //document.getElementById('iFrmReminder').setAttribute('src','frmShowReminder.aspx');
        }
        function gettime(DateObject) {

            //alert ('1');
            var SelectedDate = new Date(DateObject.GetDate());
            var monthnumber = SelectedDate.getMonth();
            var monthday = SelectedDate.getDate();
            var year = SelectedDate.getYear();


            var SelectedDateValue = new Date(year, monthnumber, monthday).getTime();
            //alert (SelectedDateValue);
            ///Checking of Transaction Date For MaxLockDate
            //    var MaxLockDate=new Date(LockDate);
            //    monthnumber = MaxLockDate.getMonth();
            //    monthday    = MaxLockDate.getDate();
            //    year        = MaxLockDate.getYear();
            //    
            //    var MaxLockDateNumeric=new Date(year, monthnumber, monthday).getTime();
            //    return 
            //                alert('TransactionDate='+TransactionDate+'\n'+'MaxLockDate= '+MaxLockDate);
            //alert(ValueDate+'~'+ValueDateNumeric+'~'+VisibleIndexE);
            ////    if(SelectedDateValue<=MaxLockDateNumeric)
            ////    {
            ////        alert('Any Transaction Below ['+monthday+'-'+monthtext[monthnumber]+'-'+year+'] Has Been Locked!!!');
            ////        MaxLockDate.setDate(MaxLockDate.getDate() + 1);
            ////        DateObject.SetDate(MaxLockDate);
            ////        return;
            ////    }
            ///End Checking of Date For MaxLockDate
        }
        function btnCreate_click() {
            document.getElementById('cmbcompany').value = "";
            document.getElementById('cmbsubject').value = "";
            document.getElementById('cmbinchargefor').value = "";
            document.getElementById('cmbCreatedFor').value = "";
            document.getElementById('chkincharge').checked = false
            document.getElementById('chkrepeat').checked = false
            document.getElementById('delterms').value = 'Unchk';
            var priority = document.getElementById("cmbpriority");
            priority.value = 2;


            document.getElementById("td_chkid").style.display = 'inline';
            document.getElementById("td_reapetlabel").style.display = 'inline';
            document.getElementById("filterForm").style.display = 'none';
            document.getElementById("filterForm1").style.display = 'none';
            document.getElementById("FormId").style.display = 'inline';
            document.getElementById("FormGrid").style.display = 'inline';
            document.getElementById("td_date").style.display = 'none';
            document.getElementById("td_date1").style.display = 'none';
            document.getElementById("td_date2").style.display = 'none';
            document.getElementById("td_date3").style.display = 'none';
            document.getElementById("td_date4").style.display = 'none';
            document.getElementById("filterForm2").style.display = 'none';
            document.getElementById("ShowFilter").style.display = 'none';
            document.getElementById("Td1").style.display = 'none';
            document.getElementById("tdtxtincharge").style.display = 'none';


            document.getElementById("td_frequency").style.display = 'none';
            document.getElementById("td_daytofre").style.display = 'none';
            document.getElementById("txt_fordays").style.display = 'none';

            document.getElementById("td_days").style.display = 'none';
            document.getElementById("td_week").style.display = 'none';
            document.getElementById("td_quarter").style.display = 'none';
            document.getElementById("td_fort").style.display = 'none';
            document.getElementById("td_month").style.display = 'none';
            document.getElementById("td_fortext").style.display = 'none';
            document.getElementById("td_semi").style.display = 'none';
            document.getElementById("td_annuall").style.display = 'none';

            document.getElementById("Tr1").style.display = 'none';
            document.getElementById("tr_button").style.display = 'none';
            ActiveBtn("btnCreate");
            InActiveBtn("Attended");
            InActiveBtn("Pending");
            InActiveBtn("btnshow");
            InActiveBtn("Today");
            InActiveBtn("btnEdit");
            InActiveBtn("btnDelete");
            InActiveBtn("All");
            var combo = document.getElementById("cmbCreatedFor");
            combo = document.getElementById("cmbinchargefor");
            //combo.value=0;
            combo = document.getElementById("cmbTicker");
            combo.value = 1;
            // txtStartDate.SetText(tim);
            var SelectedDate = new Date('<%=Session["fromdate"]%>');
        txtStartDate.SetDate(new Date(SelectedDate));
        var SelectedDate1 = new Date('<%=Session["todate"]%>');
            txtEndDate.SetDate(new Date(SelectedDate1));
        //txtEndDate.SetDate('01-01-0100 12:00 AM');
            combo = document.getElementById("txtcontent");
            combo.value = "";
            combo = document.getElementById("hdID");
            combo.value = "";

        //alert(combo);
        }
        function btnEdit_click() {
            document.getElementById('delterms').value = 'Unchk';
            document.getElementById("tdtxtincharge").style.display = 'inline';
            document.getElementById("td_date").style.display = 'none';
            document.getElementById("td_date1").style.display = 'none';
            document.getElementById("td_date2").style.display = 'none';
            document.getElementById("td_date3").style.display = 'none';
            document.getElementById("td_date4").style.display = 'none';
            document.getElementById("filterForm2").style.display = 'none';

            document.getElementById("td_frequency").style.display = 'none';
            document.getElementById("td_daytofre").style.display = 'none';
            document.getElementById("txt_fordays").style.display = 'none';
            document.getElementById("td_semi").style.display = 'none';
            document.getElementById("td_annuall").style.display = 'none';

            document.getElementById("td_days").style.display = 'none';
            document.getElementById("td_week").style.display = 'none';
            document.getElementById("td_quarter").style.display = 'none';
            document.getElementById("td_fort").style.display = 'none';
            document.getElementById("td_month").style.display = 'none';
            document.getElementById("td_fortext").style.display = 'none';
            document.getElementById("td_chkid").style.display = 'none';
            document.getElementById("td_reapetlabel").style.display = 'none';
            document.getElementById("ShowFilter").style.display = 'none';
            document.getElementById("Td1").style.display = 'none';
            ActiveBtn("btnEdit");
            InActiveBtn("Attended");
            InActiveBtn("Pending");
            InActiveBtn("btnshow");
            InActiveBtn("Today");
            InActiveBtn("btnCreate");
            InActiveBtn("btnDelete");
            InActiveBtn("All");
            //__serverCall____//
            CallServer('Edit' + '~' + RowID, "");
        }
        function btnDelete_click() {
            //alert('d');
            document.getElementById("FormId").style.display = 'none';
            document.getElementById("FormGrid").style.display = 'inline';
            ActiveBtn("btnDelete");
            InActiveBtn("Attended");
            InActiveBtn("Pending");
            InActiveBtn("btnshow");
            InActiveBtn("Today");
            InActiveBtn("btnCreate");
            InActiveBtn("btnEdit");
            InActiveBtn("All");
            var con = confirm('Are you sure to Delete this record?');
            if (con) {
                CallServer('Delete' + '~' + RowID, "");
            }

        }
        function btnSave_click() {
            //alert('s');
            // document.getElementById("FormId").style.display = 'none';
            var data = 'Save';
            var combo = document.getElementById("cmbCreatedFor_hidden");
            if (combo.value != "") {
                data += '~' + combo.value;
            }
            else {
                alert('Select Target User!');
                return false;
            }
            //            alert (combo);
            //            alert (data);
            combo = document.getElementById("cmbTicker");
            if (combo.value != "") {
                data += '~' + combo.value;
            }
            else {
                alert('Select Tricker!');
                return false;
            }
            combo = txtStartDate.GetDate();
            if (combo != "") {
                var date = (combo.getMonth() + 1) + '/' + combo.getDate() + '/' + combo.getYear() + ' ' + combo.getHours() + ':' + combo.getMinutes();
                //alert(date);
                data += '~' + date;
            }
            else {
                alert('Fill Start Date!');
                return false;
            }
            combo = txtEndDate.GetDate();
            if (combo.value != "") {
                var date = (combo.getMonth() + 1) + '/' + combo.getDate() + '/' + combo.getYear() + ' ' + combo.getHours() + ':' + combo.getMinutes();

                data += '~' + date;
            }
            else {
                alert('Fill End Date!');
                return false;
            }
            combo = document.getElementById("txtcontent");
            if (combo.value != "") {
                data += '~' + combo.value;
            }
            else {
                alert('Fill Content Data!');
                return false;
            }

            combo = document.getElementById("cmbpriority");
            if (combo.value != "") {
                data += '~' + combo.value;
            }
            combo = document.getElementById("delterms")
            data += '~' + combo.value;
            combo = document.getElementById("hdnfrequency")
            data += '~' + combo.value;
            combo = document.getElementById("txt_fordays")
            data += '~' + combo.value;

            combo = document.getElementById("hdnsatusday")
            data += '~' + combo.value;
            combo = document.getElementById("hdnsunday")
            data += '~' + combo.value;


            combo = document.getElementById("hdID");
            data += '~' + combo.value;
            combo = document.getElementById("cmbinchargefor_hidden")
            data += '~' + combo.value;
            //            combo = document.getElementById("cmbsubject_hidden") 
            combo = document.getElementById("cmbsubject")
            if (combo.value != "") {
                data += '~' + combo.value;
            }
            else {
                alert('Fill Subject');
                return false;
            }

            combo = document.getElementById("cmbcompany")
            if (combo.value != "") {
                data += '~' + combo.value;
            }
            else {
                alert('Fill Company Shortname');
                return false;
            }
            //alert(data);
            //__serverCall____//
            //document.getElementById("FormId").style.display = 'none';
            CallServer(data, "");
            //alert (CallServer);
            // alert (data);

        }
        function btnCancel_click() {
            //alert('Ca');
            document.getElementById("FormId").style.display = 'none';
            document.getElementById("FormGrid").style.display = 'inline';
        }
        function btnCancel2_click() {
            document.getElementById("Tr1").style.display = 'none';
            document.getElementById("tr_button").style.display = 'none';
        }

        function btnshow_click() {
            document.getElementById("Tr1").style.display = 'inline';
            document.getElementById("tr_button").style.display = 'inline';
            document.getElementById("filterForm").style.display = 'none';
            document.getElementById("filterForm1").style.display = 'none';
            document.getElementById("filterForm2").style.display = 'none';
            document.getElementById("ShowFilter").style.display = 'none';
            document.getElementById("Td1").style.display = 'none';
            ActiveBtn("btnshow");
            InActiveBtn("btnCreate");
            InActiveBtn("All");
            InActiveBtn("Pending");
            InActiveBtn("Attended");
            InActiveBtn("btnEdit");
            InActiveBtn("btnDelete");
            InActiveBtn("Today");
        }
        function btnInbox_Click() {
            gridM.PerformCallback('inBox');
            //document.getElementById("FormCreate").style.display = 'none';
            document.getElementById("FormGridM").style.display = 'inline';
            document.getElementById("trRepluForm").style.display = 'none';
            gridM.UnselectAllRowsOnPage();
        }
        function btnRead_click() {
            if (counter != 'n') {
                //document.getElementById("FormCreate").style.display = 'none';
                var ReadIDs = 'read~' + counter;
                CallServer(ReadIDs, "");
            }
            else
                alert('Plase Select a message!');

        }
        function btnReply_Click() {
            if (counter != 'n') {
                var ReadIDs = 'reply~' + counter;
                CallServer(ReadIDs, "");
            }
            else
                alert('Plase Select a message!');
        }
        function btnShowTemplate_click() {
            document.getElementById("TDtemplate").style.display = 'inline';
            document.getElementById("TDshow").style.display = 'none';
        }
        function btnHideTemplate_click() {
            document.getElementById("TDtemplate").style.display = 'none';
            document.getElementById("TDshow").style.display = 'inline';
        }
        function btnCancelM_click() {
            document.getElementById("trRepluForm").style.display = 'none';
            document.getElementById("FormGridM").style.display = 'inline';
            gridM.UnselectAllRowsOnPage();
            counter = 'n';
        }
        function PopulateGrid(obj) {
            grid.PerformCallback(obj);
        }
        function btnSend_click() {
            var replyText = document.getElementById("txtContentM");
            var ReadIDs = "send~" + counter + '~' + replyText.value;
            CallServer(ReadIDs, "");
        }

        function FunCallAjaxList(objID, objEvent, ObjType) {

            var strQuery_Table = '';
            var strQuery_FieldName = '';
            var strQuery_WhereClause = '';
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = '';

            if (ObjType == 'Digital') {
                //var alert1=document.getElementById('HiddenField1').value;
                //alert(alert1);
                strQuery_Table = "Master_Remindercategory";
                strQuery_FieldName = "distinct top 10 Remindercategory_shortname AS shortname,Remindercategory_id";
                //strQuery_WhereClause = "Remindercategory_id is not null";
                strQuery_WhereClause = "  ( Remindercategory_shortname like (\'%RequestLetter%') or Remindercategory_description like (\'%RequestLetter%'))";

            }

            if (ObjType == 'company') {

                strQuery_Table = "tbl_master_company";
                strQuery_FieldName = "distinct top 10 ltrim(rtrim(cmp_Name)) + '  '+' [ ' + cmp_onroleshortname + ' ] ' AS compname,cmp_id";
                strQuery_WhereClause = "( cmp_name like (\'%RequestLetter%') or cmp_onroleshortname like (\'%RequestLetter%'))";


            }
            CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            //alert (CombinedQuery);
            ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars(CombinedQuery));
            //alert (CombinedQuery);
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
        function CallAjax(obj1, obj2, obj3) {
            //alert ('1');
            // FieldName='ctl00_ContentPlaceHolder1_Headermain1_cmbCompany';
            ajax_showOptions(obj1, obj2, obj3);
            //alert (ajax_showOptions);
        }

        function ShowMissingData(obj) {
            var URL = 'Addreply_reminder.aspx?id=' + obj;
            //OnMoreInfoClick(url,"Insert Reply  "  ,'450px','350px',"N");
            window.location.href = URL;
            //editwin = dhtmlmodal.open("Editbox", "iframe", URL, "Change Task Updation Log/Status", "width=1000px,height=500px,center=0,resize=1,top=-1", "recal");
            //editwin.onclose = function () {
            //    grid.PerformCallback();
            //}
        }
        //    function Changestatus(obj)
        //    {
        //      var url='changestatus_reminder.aspx?id=' +obj;
        //      OnMoreInfoClick(url,"Change Status  "  ,'450px','350px',"N");
        //    }
        function showhistory(obj) {

            var URL = 'showhistory_reminder.aspx?id=' + obj;

            //OnMoreInfoClick(URL,"Modify Contact Details",'10px','10px',"Y");
            window.location.href = URL;
            //editwin = dhtmlmodal.open("Editbox", "iframe", URL, "View Task Updation Log", "width=1000px,height=400px,center=0,resize=1,top=-1", "recal");
            //editwin.onclose = function () {
            //    grid.PerformCallback();
            //}

        }
        function Changestatus(obj) {
            var URL = "changestatus_reminder.aspx?id=" + obj;
            window.location.href = URL;
            //editwin = dhtmlmodal.open("Editbox", "iframe", URL, "Change Task Updation Log/Status", "width=1000px,height=300px,center=0,resize=1,top=-1", "recal");
            //editwin.onclose = function () {
            //    grid.PerformCallback();
            //}
        }
        function fncolumnsdisplay(obj) {

            if (obj.checked == true) {
                document.getElementById('td_date').style.display = 'inline';
                document.getElementById("td_date1").style.display = 'inline';
                document.getElementById("td_date2").style.display = 'inline';
                document.getElementById("td_date3").style.display = 'inline';
                document.getElementById("td_date4").style.display = 'inline';

                document.getElementById("td_frequency").style.display = 'inline';
                document.getElementById("td_daytofre").style.display = 'inline';
                document.getElementById("txt_fordays").style.display = 'inline';

                document.getElementById("td_days").style.display = 'inline';
                // document.getElementById("td_week").style.display='none';
                //document.getElementById("td_month").style.display='none';
                document.getElementById("td_fortext").style.display = 'inline';
                document.getElementById('delterms').value = 'Chk';

            }
            else {
                document.getElementById('td_date').style.display = 'none';
                document.getElementById("td_date1").style.display = 'none';
                document.getElementById("td_date2").style.display = 'none';
                document.getElementById("td_date3").style.display = 'none';
                document.getElementById("td_date4").style.display = 'none';
                document.getElementById("td_frequency").style.display = 'none';
                document.getElementById("td_daytofre").style.display = 'none';
                document.getElementById("txt_fordays").style.display = 'none';

                document.getElementById("td_days").style.display = 'none';
                // document.getElementById("td_week").style.display='none';
                //document.getElementById("td_month").style.display='none';
                document.getElementById("td_fortext").style.display = 'none';
                document.getElementById('delterms').value = 'Unchk';

            }
            //height();
        }
        function fnshowclient(obj) {
            if (obj.checked == true) {
                document.getElementById('tdtxtincharge').style.display = 'inline'
            }
            else {
                document.getElementById('tdtxtincharge').style.display = 'none'
            }
        }
        function saturday(obj) {
            if (obj.checked == true) {
                document.getElementById('hdnsatusday').value = 'satryes';
            }
            else {
                document.getElementById('hdnsatusday').value = 'satrno';
            }
        }

        function sunday(obj) {
            if (obj.checked == true) {
                document.getElementById('hdnsunday').value = 'sunyes';
            }
            else {
                document.getElementById('hdnsunday').value = 'sunrno';
            }
        }
        function forfrequency(obj) {

            if (obj == "0") {
                document.getElementById('td_days').style.display = 'none';
                document.getElementById('td_week').style.display = 'none';
                document.getElementById('td_month').style.display = 'none';
                document.getElementById("td_quarter").style.display = 'none';
                document.getElementById("td_fort").style.display = 'none';
                document.getElementById("td_semi").style.display = 'none';
                document.getElementById("td_annuall").style.display = 'none';

                document.getElementById('hdnfrequency').value = 'day';
            }
            if (obj == "D") {
                document.getElementById('td_days').style.display = 'inline';
                document.getElementById('td_week').style.display = 'none';
                document.getElementById('td_month').style.display = 'none';
                document.getElementById("td_quarter").style.display = 'none';
                document.getElementById("td_fort").style.display = 'none';
                document.getElementById("td_semi").style.display = 'none';
                document.getElementById("td_annuall").style.display = 'none';

                document.getElementById('hdnfrequency').value = 'day';
            }
            if (obj == "W") {
                document.getElementById('td_days').style.display = 'none';
                document.getElementById('td_week').style.display = 'inline';
                document.getElementById('td_month').style.display = 'none';
                document.getElementById("td_quarter").style.display = 'none';
                document.getElementById("td_fort").style.display = 'none';
                document.getElementById("td_semi").style.display = 'none';
                document.getElementById("td_annuall").style.display = 'none';
                document.getElementById('hdnfrequency').value = 'week';
            }
            if (obj == "M") {
                document.getElementById('td_days').style.display = 'none';
                document.getElementById('td_week').style.display = 'none';
                document.getElementById('td_month').style.display = 'inline';
                document.getElementById("td_quarter").style.display = 'none';
                document.getElementById("td_fort").style.display = 'none';
                document.getElementById('hdnfrequency').value = 'month';
            }

            if (obj == "F") {
                document.getElementById('td_days').style.display = 'none';
                document.getElementById('td_week').style.display = 'none';
                document.getElementById('td_month').style.display = 'none';
                document.getElementById("td_quarter").style.display = 'none';
                document.getElementById("td_fort").style.display = 'inline';
                document.getElementById("td_semi").style.display = 'none';
                document.getElementById("td_annuall").style.display = 'none';
                document.getElementById('hdnfrequency').value = 'fortnight';
            }
            if (obj == "Q") {
                document.getElementById('td_days').style.display = 'none';
                document.getElementById('td_week').style.display = 'none';
                document.getElementById('td_month').style.display = 'none';
                document.getElementById("td_quarter").style.display = 'inline';
                document.getElementById("td_fort").style.display = 'none';
                document.getElementById("td_semi").style.display = 'none';
                document.getElementById("td_annuall").style.display = 'none';
                document.getElementById('hdnfrequency').value = 'quarter';
            }
            if (obj == "S") {
                document.getElementById('td_days').style.display = 'none';
                document.getElementById('td_week').style.display = 'none';
                document.getElementById('td_month').style.display = 'none';
                document.getElementById("td_quarter").style.display = 'none';
                document.getElementById("td_fort").style.display = 'none';
                document.getElementById("td_semi").style.display = 'inline';
                document.getElementById("td_annuall").style.display = 'none';
                document.getElementById('hdnfrequency').value = 'semi';
            }
            if (obj == "A") {
                document.getElementById('td_days').style.display = 'none';
                document.getElementById('td_week').style.display = 'none';
                document.getElementById('td_month').style.display = 'none';
                document.getElementById("td_quarter").style.display = 'none';
                document.getElementById("td_fort").style.display = 'none';
                document.getElementById("td_semi").style.display = 'none';
                document.getElementById("td_annuall").style.display = 'inline';
                document.getElementById('hdnfrequency').value = 'annual';
            }


        }
        function ReceiveServerData(rValue) {
            // alert(rValue);
            var DATA = rValue.split('~');
            if (DATA[0] == "Edit") {
                if (DATA[1] != "nauth") {
                    document.getElementById("FormId").style.display = 'inline';
                    var combo = document.getElementById("cmbCreatedFor");
                    combo.value = DATA[1];
                    combo = document.getElementById("cmbTicker");
                    combo.value = DATA[2];
                    var data1 = DATA[3].split('/');
                    var Ytime = data1[2].split(' ');
                    //alert(Ytime[1]+'-'+Ytime[2]);
                    var time = Ytime[1].split(':');
                    var hour = 0;
                    if (Ytime[2] == 'PM')
                        //hour=parseInt(time[0])+12;
                        hour = parseInt(time[0]);
                    else
                        hour = time[0];
                    var newDate = new Date(Ytime[0], data1[0] - 1, data1[1], hour, time[1], 0);
                    txtStartDate.SetDate(newDate);

                    var datepart = DATA[4].split('/');
                    var YearTime = datepart[2].split(' ');
                    var hourmin = YearTime[1].split(':');
                    var HR = 0;
                    if (YearTime[2] == 'PM')
                        //HR=parseInt(hourmin[0])+12;
                        HR = parseInt(hourmin[0]);
                    else
                        HR = hourmin[0];

                    var new_date = new Date(YearTime[0], datepart[0] - 1, datepart[1], HR, hourmin[1], hourmin[2]);
                    txtEndDate.SetDate(new_date);
                    combo = document.getElementById("txtcontent");
                    combo.value = DATA[5];

                    combo = document.getElementById("cmbpriority");
                    combo.value = DATA[6];
                    combo = document.getElementById("cmbCreatedFor_hidden");
                    combo.value = DATA[7];


                    combo = document.getElementById("hdID");
                    combo.value = DATA[8];
                    combo = document.getElementById("cmbsubject");
                    combo.value = DATA[9];
                    combo = document.getElementById("cmbinchargefor");
                    combo.value = DATA[10];
                    combo = document.getElementById("cmbinchargefor_hidden");
                    combo.value = DATA[11];
                    combo = document.getElementById("cmbcompany");
                    combo.value = DATA[12];


                    // alert(DATA);

                }
                else {
                    alert('You are not Authorise to Change data!!');
                    document.getElementById("FormId").style.display = 'none';
                }
            }
            if (DATA[0] == "Save") {
                if (DATA[1] != "Y") {
                    alert('Update Unsuccessful!');
                    //alert ('1');
                }
                else {
                    // alert ('2');
                    grid.PerformCallback(DATA[2]);
                    alert('Update Successful!');
                    document.getElementById("FormId").style.display = 'none';
                    //_____Getting latest data in iframe by calling function of reminder page without refreshing page___//
                    x = top.frames['iFrmReminder'].ParentCall('Parent');
                }
            }
            if (DATA[0] == "Delete") {
                if (DATA[1] != "Y") {
                    alert('You are not Authorise to Change data!!');
                    grid.PerformCallback(DATA[2]);
                }
                else {
                    grid.PerformCallback(DATA[2]);
                    alert('Deleted Successfully!');
                    //x = top.frames['iFrmReminder'].location = "frmShowReminder.aspx";
                    x = top.frames['iFrmReminder'].ParentCall('Parent');
                }
            }
            if (DATA[0] == "Attend") {
                if (DATA[1] != "Y")
                    alert('You are not Authorise to Change data!!');
                else {
                    grid.PerformCallback(DATA[2]);
                    document.getElementById("tr_button").style.display = 'none';
                    document.getElementById("Tr1").style.display = 'none';
                    alert('Attended Successfully!');
                    x = top.frames['iFrmReminder'].ParentCall('Parent');
                }
            }
            if (DATA[0] == "read") {
                if (DATA[1] == "Y") {
                    alert('Read Successfully!');
                    gridM.PerformCallback('read');
                    gridM.UnselectAllRowsOnPage();
                }
                else if (DATA[1] == "S")
                    alert('Please Select a message!');
            }
            if (DATA[0] == "reply") {
                if (DATA[1] != "M") {
                    if (DATA[1] != "") {
                        document.getElementById("trRepluForm").style.display = 'inline';
                        document.getElementById("FormGridM").style.display = 'none';
                        var replyText = document.getElementById("txtContentM");
                        replyText.value = '';

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
            if (DATA[0] == "send") {
                if (DATA[1] == "Y") {
                    alert('Message sent Successfully!');
                    document.getElementById("trRepluForm").style.display = 'none';
                    document.getElementById("FormGridM").style.display = 'inline';
                    gridM.PerformCallback('inBox');
                    gridM.UnselectAllRowsOnPage();
                    counter = 'n';
                    var replyText = document.getElementById("txtContentM");
                    replyText.value = '';
                }
            }
        }
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Reminders / Tasks</h3>
        </div>

    </div>
    <div class="form_main">
        <table class="TableMain100">
            <%--<tr>
                    <td class="EHEADER" style="text-align: center">
                        <strong><span style="color: Blue; font-size: 10pt;">Reminders / Tasks</span></strong></td>
                </tr>--%>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td style="vertical-align: top; text-align: left;">
                                <div style="float: right; width: 3.5%">
                                    <asp:ImageButton OnClick="test_Click" ImageUrl="/assests/images/refresh1.jpg" ID="test"
                                        runat="server" Width="40px" Height="40px" ToolTip="Refresh" />
                                </div>
                                <div style="border: 2px solid Gray; padding: 4px 2px 4px 2px; background-color: #eee; width: 95.5%">
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td>
                                                <div style="border: 1px solid #999; padding: 2px; background-color: #ddd;">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td id="ShowFilter">
                                                              <%--  <a href="javascript:ShowHideFilter('s');"><span style="color: Gray; text-decoration: underline; font-size: 12px">Search</span></a>--%>
                                                            </td>
                                                            <td id="Td1">
                                                                <a href="javascript:ShowHideFilter('All');"><span style="color: Gray; text-decoration: underline; font-size: 12px">ShowAll</span></a>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                            <td style="width: 10px">&nbsp;
                                            </td>
                                            <td>
                                                <div style="border: 1px solid #999; padding: 2px; background-color: #ddd;">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl_show" runat="server" Text="Show" ForeColor="navy" Font-Bold="true"
                                                                    Font-Size="12px" BackColor="ActiveBorder" BorderStyle="Groove"></asp:Label>
                                                            </td>
                                                            <td style="width: 5px;">&nbsp;
                                                            </td>
                                                            <td style="vertical-align: top; text-align: left;">
                                                                <input id="Today" type="button" value="Today`s" class="btnUpdate" onclick="btnToday_click();"
                                                                    style="width: 80px; height: 19px;" runat="server" />
                                                            </td>
                                                            <td style="vertical-align: top; text-align: left;">
                                                                <input id="All" type="button" value="All" class="btnUpdate" onclick="btnAll_click();"
                                                                    style="width: 80px; height: 19px;" />
                                                            </td>
                                                            <td style="vertical-align: top; text-align: left;">
                                                                <input id="Pending" type="button" value="UnAttended" class="btnUpdate" onclick="btnPending_click();"
                                                                    style="width: 80px; height: 19px;" />
                                                            </td>
                                                            <td style="vertical-align: top; text-align: left;">
                                                                <input id="Attended" type="button" value="Attended" class="btnUpdate" onclick="btnAttended_click();"
                                                                    style="width: 80px; height: 19px;" />
                                                            </td>
                                                            <td style="width: 1px;">&nbsp;<asp:HiddenField ID="hdUserList" runat="server" />
                                                            </td>
                                                            <td style="vertical-align: top; text-align: left;">
                                                                <asp:TextBox ID="Text1" runat="server" Visible="false" Width="2px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                            <td style="width: 10px">&nbsp;
                                            </td>
                                            <td>
                                                <div style="border: 1px solid #999; padding: 2px; background-color: #ddd;">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl_Action" runat="server" Text="Action" ForeColor="navy" Font-Bold="true"
                                                                    Font-Size="12px" BackColor="ActiveBorder" BorderStyle="Groove"></asp:Label>
                                                            </td>
                                                            <td style="width: 5px;">&nbsp;
                                                            </td>
                                                            <td style="vertical-align: top; text-align: left;">
                                                                <input id="btnshow" type="button" value="Attend" class="btnUpdate" onclick="btnshow_click();"
                                                                    style="width: 80px; height: 19px; display: none" />
                                                            </td>
                                                            <td style="vertical-align: top; text-align: left;">
                                                                <input id="btnCreate" type="button" value="Create" class="btnUpdate" onclick="btnCreate_click();"
                                                                    style="width: 80px; height: 19px;" />
                                                            </td>
                                                            <td style="vertical-align: top; text-align: left;">
                                                                <input id="btnEdit" type="button" value="Edit" class="btnUpdate" onclick="btnEdit_click();"
                                                                    style="width: 80px; height: 19px;" />
                                                            </td>
                                                            <td style="vertical-align: top; text-align: left;">
                                                                <input id="btnDelete" type="button" value="Delete" class="btnUpdate" onclick="btnDelete_click();"
                                                                    style="width: 80px; height: 19px;" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td class="gridcellright" align="right" id="td_export" runat="server">
                                                <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" BackColor="Gray"
                                                    Font-Bold="False" ForeColor="White" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                                    ValueType="System.Int32" Width="80px">
                                                    <Items>
                                                        <dxe:ListEditItem Text="Select" Value="0" />
                                                        <%--<dxe:ListEditItem Text="PDF" Value="1" />--%>
                                                        <dxe:ListEditItem Text="XLS" Value="2" />
                                                        <%--<dxe:ListEditItem Text="RTF" Value="3" />--%>
                                                        <dxe:ListEditItem Text="CSV" Value="4" />
                                                    </Items>
                                                    <ButtonStyle BackColor="#C0C0FF" ForeColor="Black">
                                                    </ButtonStyle>
                                                    <ItemStyle BackColor="Navy" ForeColor="White">
                                                        <HoverStyle BackColor="#8080FF" ForeColor="White">
                                                        </HoverStyle>
                                                    </ItemStyle>
                                                    <Border BorderColor="White" />
                                                    <DropDownButton Text="Export">
                                                    </DropDownButton>
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <%--<br style="clear:both" />--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%; vertical-align: top">
                                    <tr id="FormID" style="display: none">
                                        <td colspan="2" style="background-color: #D1E1F8;">
                                            <asp:Panel ID="pnlForm" runat="server" Width="100%" BorderColor="blue" BorderWidth="1px"
                                                BackColor="#D1E1F8">
                                                <table style="height: 9px">
                                                    <tr>
                                                        <td style="text-align: left; width: 80px;">
                                                            <span id="Span2" class="Ecoheadtxt">Task For:</span>
                                                        </td>
                                                        <td id="tdtxtcustomer">
                                                            <asp:TextBox ID="cmbCreatedFor" runat="server" Width="400px" Font-Size="11px"></asp:TextBox><asp:TextBox
                                                                ID="cmbCreatedFor_hidden" runat="server" Width="14px" Style="display: none;"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: left; width: 98px;">
                                                            <span id="Span3" class="Ecoheadtxt">Display as Ticker:</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbTicker" runat="server" Width="50px" Font-Size="12px">
                                                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                <asp:ListItem Value="0">No</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="text-align: left;">
                                                            <span id="Span5" class="Ecoheadtxt">Priority:</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbpriority" runat="server" Width="100px">
                                                                <asp:ListItem Value="2"> Normal</asp:ListItem>
                                                                <asp:ListItem Value="3">High</asp:ListItem>
                                                                <asp:ListItem Value="1">Low</asp:ListItem>
                                                                <asp:ListItem Value="4">Urgent</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table>
                                                    <tr>
                                                        <td style="text-align: left; width: 80px;">
                                                            <span id="Span16" class="Ecoheadtxt">Follow-up By:</span>
                                                        </td>
                                                        <td style="text-align: left;" width="20px" runat="server" id="td_chkin">
                                                            <asp:CheckBox ID="chkincharge" runat="server" Checked="false" onclick="fnshowclient(this)" />
                                                        </td>
                                                        <td id="tdtxtincharge">
                                                            <asp:TextBox ID="cmbinchargefor" runat="server" Width="400px" Font-Size="11px"></asp:TextBox>
                                                            <asp:TextBox ID="cmbinchargefor_hidden" runat="server" Width="14px" Style="display: none;"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table>
                                                    <tr>
                                                        <%-- <td style="text-align: left; width:200px; " id="td_subject">
                                                           <a href="javascript:selectsubject('subject');" onclick="selectsubject()"><span style="color: #000099; text-decoration: underline; font-size:small">
                                                    Choose Subject</span></a>
                                                         
                                                         </td>--%>
                                                        <td style="text-align: left; width: 80px;" id="td_subject">
                                                            <span id="Span17" class="Ecoheadtxt">Select Subject:</span>
                                                        </td>
                                                        <td id="tdtxtsubject">
                                                            <asp:TextBox ID="cmbsubject" runat="server" Width="500px" Font-Size="11px" onkeyup="FunCallAjaxList(this,event,'Digital')"></asp:TextBox>
                                                            <asp:TextBox ID="cmbsubject_hidden" runat="server" Width="14px" Style="display: none;"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table>
                                                    <tr>
                                                        <td style="text-align: left; width: 80px;" id="td_company">
                                                            <span id="Span19" class="Ecoheadtxt">Company name:</span>
                                                        </td>
                                                        <td id="tdtxtcompany">
                                                            <asp:TextBox ID="cmbcompany" runat="server" Width="500px" Font-Size="11px" onkeyup="FunCallAjaxList(this,event,'company')"></asp:TextBox>
                                                            <asp:TextBox ID="cmbcompany_hidden" runat="server" Width="14px" Style="display: none;"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table>
                                                    <tr>
                                                        <td style="text-align: left; width: 80px;">
                                                            <span id="spHoldUntillDate" class="Ecoheadtxt">Start date:</span>
                                                        </td>
                                                        <td>
                                                            <dxe:ASPxDateEdit ID="txtStartDate" ClientInstanceName="txtStartDate" runat="server"
                                                                UseMaskBehavior="True" EditFormat="Custom">
                                                                <ButtonStyle Width="13px">
                                                                </ButtonStyle>
                                                                <%--<clientsideevents datechanged="function(s,e){gettime(txtStartDate);}"></clientsideevents>--%>
                                                            </dxe:ASPxDateEdit>
                                                        </td>
                                                        <td style="text-align: left;">
                                                            <span id="Span11" class="Ecoheadtxt">End Date:</span>
                                                        </td>
                                                        <td>
                                                            <dxe:ASPxDateEdit ID="txtEndDate" ClientInstanceName="txtEndDate" runat="server"
                                                                UseMaskBehavior="True" EditFormat="Custom">
                                                                <ButtonStyle Width="13px">
                                                                </ButtonStyle>
                                                            </dxe:ASPxDateEdit>
                                                        </td>
                                                        <%--<td>
                                                            <dxe:ASPxComboBox runat="server" ID="cmbpriority" Width="100px" DropDownStyle="DropDown"
                                                                        EnableSynchronization="False" EnableIncrementalFiltering="True">
                                                                        <Items>
                                                                            <dxe:ListEditItem Text="Nearest" Value="1"></dxe:ListEditItem>
                                                                            <dxe:ListEditItem Text="Higher" Value="2"></dxe:ListEditItem>
                                                                            <dxe:ListEditItem Text="Lower" Value="3"></dxe:ListEditItem>
                                                                            <dxe:ListEditItem Text="Truncate" Value="4"></dxe:ListEditItem>
                                                                            <%--<dxe:ListEditItem Text="Nearest 5 Paisa" Value="5"></dxe:ListEditItem>
                                                                            <dxe:ListEditItem Text="Lower 5 Paisa" Value="6"></dxe:ListEditItem>
                                                                            <dxe:ListEditItem Text="Higher 5 Paisa" Value="7"></dxe:ListEditItem>
                                                                        </Items>
                                                                    </dxe:ASPxComboBox>
                                                            </td>--%>
                                                        <%--<td>
                                         <asp:DropDownList ID="cmbpriority" runat="server" Width="100px">
                                        <asp:ListItem  Value="2"> Normal</asp:ListItem>
                                        <asp:ListItem  Value="3">High</asp:ListItem>
                                        <asp:ListItem  Value="1">Low</asp:ListItem>
                                        <asp:ListItem  Value="4">Urgent</asp:ListItem>
                                       
                                    </asp:DropDownList>
                                                            </td>--%>
                                                    </tr>
                                                </table>
                                                <table>
                                                    <tr>
                                                        <td style="text-align: left; width: 80px;">
                                                            <span id="Span4" class="Ecoheadtxt">Content:</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtcontent" runat="server" Font-Size="12px" Height="60px" TextMode="MultiLine"
                                                                Width="725px"></asp:TextBox>
                                                        </td>
                                                        <%--<td style="text-align: right;">
                                                            </td>
                                                            <td class="gridcellleft">
                                                            </td>
                                                            <td style="text-align: right;">
                                                            </td>
                                                            <td style="text-align: right;" >
                                                            </td>--%>
                                                    </tr>
                                                </table>
                                                <table>
                                                    <tr id="tr_rpt">
                                                        <td style="text-align: left;" id="td_reapetlabel" runat="server">
                                                            <span id="Span111" class="Ecoheadtxt">Repeat This Reminder:</span>
                                                        </td>
                                                        <td style="text-align: right;" width="20px" runat="server" id="td_chkid">
                                                            <asp:CheckBox ID="chkrepeat" runat="server" Checked="false" onclick="fncolumnsdisplay(this)" />
                                                            <asp:HiddenField ID="delterms" runat="server" />
                                                        </td>
                                                        <td style="text-align: left;" id="td_frequency" runat="server">
                                                            <span id="Span1111" class="Ecoheadtxt">Frequency:</span>
                                                        </td>
                                                        <td id="td_daytofre" runat="server" style="width: 70px;">
                                                            <asp:DropDownList ID="cmbfrequency" runat="server" Width="100px" onchange="forfrequency(this.value)">
                                                                <asp:ListItem Value="0"> --Select--</asp:ListItem>
                                                                <asp:ListItem Value="D"> Daily</asp:ListItem>
                                                                <asp:ListItem Value="W">Weekly</asp:ListItem>
                                                                <asp:ListItem Value="F"> Fortnightly</asp:ListItem>
                                                                <asp:ListItem Value="M">Monthly</asp:ListItem>
                                                                <asp:ListItem Value="Q"> Quarterly</asp:ListItem>
                                                                <asp:ListItem Value="S"> Semi Annually</asp:ListItem>
                                                                <asp:ListItem Value="A"> Annually</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:HiddenField ID="hdnfrequency" runat="server" />
                                                        </td>
                                                        <td id="td_fortext" runat="server" style="width: 40px;">
                                                            <span id="Span7" class="Ecoheadtxt">For</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_fordays" runat="server" Width="40px"></asp:TextBox>
                                                            <asp:HiddenField ID="txt_fordays_hidden" runat="server" />
                                                        </td>
                                                        <td style="text-align: left;" id="td_days" runat="server">
                                                            <span id="Span8" class="Ecoheadtxt">Days</span>
                                                        </td>
                                                        <td style="text-align: left;" id="td_week" runat="server">
                                                            <span id="Span9" class="Ecoheadtxt">Week</span>
                                                        </td>
                                                        <td style="text-align: left;" id="td_month" runat="server">
                                                            <span id="Span10" class="Ecoheadtxt">Month</span>
                                                        </td>
                                                        <td style="text-align: left;" id="td_fort" runat="server">
                                                            <span id="Span12" class="Ecoheadtxt">Fortnight</span>
                                                        </td>
                                                        <td style="text-align: left;" id="td_quarter" runat="server">
                                                            <span id="Span13" class="Ecoheadtxt">Quarter</span>
                                                        </td>
                                                        <td style="text-align: left;" id="td_semi" runat="server">
                                                            <span id="Span14" class="Ecoheadtxt">Semi Annuall</span>
                                                        </td>
                                                        <td style="text-align: left;" id="td_annuall" runat="server">
                                                            <span id="Span15" class="Ecoheadtxt">Annuall</span>
                                                        </td>
                                                        <%--</tr>
                                                        <tr id="td_date" runat="server">--%>
                                                        <%--</tr>--%>
                                                        <%-- </table>
                                                         <table>
                                                        <tr>--%>
                                                        <td style="width: 100px;"></td>
                                                        <td style="text-align: right;" id="td_date" runat="server">
                                                            <span id="Span123" class="Ecoheadtxt">Exclude the day of:</span>
                                                        </td>
                                                        <%--<td style="text-align: right;" width="40px" >--%>
                                                        <td style="text-align: left;" id="td_date1" runat="server">
                                                            <span id="Span1" class="Ecoheadtxt" style="text-align: right;">Saturday</span>
                                                        </td>
                                                        <td style="text-align: left;" id="td_date2" runat="server">
                                                            <asp:CheckBox ID="CheckBoxstr" runat="server" Checked="false" onclick="saturday(this)" />
                                                            <asp:HiddenField ID="hdnsatusday" runat="server" />
                                                            <%-- </td>
                                                            <td>--%>
                                                        </td>
                                                        <td style="text-align: left;" id="td_date3" runat="server">
                                                            <%--</td>
                                                            <td style="text-align: right;" width="40px">--%>
                                                            <span id="Span6" class="Ecoheadtxt">Sunday</span>
                                                        </td>
                                                        <td style="text-align: left;" id="td_date4" runat="server">
                                                            <asp:CheckBox ID="CheckBoxsun" runat="server" Checked="false" onclick="sunday(this)" />
                                                            <asp:HiddenField ID="hdnsunday" runat="server" />
                                                            <%--</td>
                                                            <td>--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <input id="btnSave" type="button" value="Save" class="btnUpdate" onclick="btnSave_click();"
                                                                style="width: 80px; height: 18px;" />
                                                            &nbsp;&nbsp;
                                                                <input id="btnCancel" type="button" value="Cancel" class="btnUpdate" onclick="btnCancel_click();"
                                                                    style="width: 80px; height: 18px;" />
                                                        </td>
                                                        <td style="text-align: right;">
                                                            <input id="hdID" type="hidden" style="width: 151px; height: 7px" />
                                                        </td>
                                                        <td class="gridcellleft"></td>
                                                        <td style="text-align: right;"></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr id="Tr1" runat="server">
                                                    <td style="background-color: #D1E1F8;">
                                                        <asp:TextBox ID="td_reply" runat="server" Font-Size="12px" Height="30px" TextMode="MultiLine"
                                                            Width="500px"></asp:TextBox>
                                                        <asp:HiddenField ID="hdnreply" runat="server" />
                                                    </td>
                                                    <td style="text-align: left;" id="td2" runat="server">
                                                        <span id="Span18" class="Ecoheadtxt" style="text-align: right;">Attend Status :</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlattend" runat="server">
                                                            <asp:ListItem Value="1" Text="Attend Completely"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="Attend Open"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="tr_button" runat="server">
                                        <td>
                                            <input id="btnAttend" type="button" value="Attend" class="btnUpdate" onclick="btnAttend_click();"
                                                style="width: 80px; height: 19px;" />
                                            &nbsp;&nbsp;
                                                <input id="btnCancel2" type="button" value="Cancel" class="btnUpdate" onclick="btnCancel2_click();"
                                                    style="width: 80px; height: 18px;" />
                                        </td>
                                    </tr>
                                    <tr style="width: 100%">
                                        <td style="vertical-align: top; text-align: left; width: 80px;" id="filterForm">
                                            <table style="width: 182px">
                                                <tr>
                                                    <td class="Ecoheadtxt" style="text-align: left; width: 63%;">
                                                        <dxe:ASPxDateEdit ID="txtStart" ClientInstanceName="txtStart" runat="server" UseMaskBehavior="True"
                                                            EditFormat="Custom">
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                            <DropDownButton Text="Start">
                                                            </DropDownButton>
                                                        </dxe:ASPxDateEdit>
                                                    </td>
                                                    <%--</tr>
                                                    <tr>--%>
                                                    <td class="Ecoheadtxt" style="text-align: left; width: 63%;">
                                                        <dxe:ASPxDateEdit ID="txtEnd" ClientInstanceName="txtEnd" runat="server" UseMaskBehavior="True"
                                                            EditFormat="Custom">
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                            <DropDownButton Text="End">
                                                            </DropDownButton>
                                                        </dxe:ASPxDateEdit>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="1" style="text-align: left;">
                                                        <input id="Filter" type="button" value="Show" class="btnUpdate" onclick="btnFilter_click();"
                                                            style="width: 80px" runat="server" validationgroup="d" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="vertical-align: top; text-align: left; width: 80px;" id="filterForm1">
                                            <table style="width: 182px">
                                                <tr>
                                                    <td class="Ecoheadtxt" style="text-align: left; width: 63%;">
                                                        <dxe:ASPxDateEdit ID="txtStart1" ClientInstanceName="txtStart1" runat="server" UseMaskBehavior="True"
                                                            EditFormat="Custom">
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                            <DropDownButton Text="Start">
                                                            </DropDownButton>
                                                        </dxe:ASPxDateEdit>
                                                    </td>
                                                    <%-- </tr>
                                                    <tr>--%>
                                                    <td class="Ecoheadtxt" style="text-align: left; width: 63%;">
                                                        <dxe:ASPxDateEdit ID="txtEnd1" ClientInstanceName="txtEnd1" runat="server" UseMaskBehavior="True"
                                                            EditFormat="Custom">
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                            <DropDownButton Text="End">
                                                            </DropDownButton>
                                                        </dxe:ASPxDateEdit>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="1" style="text-align: left;">
                                                        <input id="Filter1" type="button" value="Show" class="btnUpdate" onclick="btnFilter1_click();"
                                                            style="width: 80px" runat="server" validationgroup="d" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="vertical-align: top; text-align: left; width: 80px;" id="filterForm2">
                                            <table style="width: 182px">
                                                <tr>
                                                    <td class="Ecoheadtxt" style="text-align: left; width: 63%;">
                                                        <dxe:ASPxDateEdit ID="txtStart2" ClientInstanceName="txtStart2" runat="server" UseMaskBehavior="True"
                                                            EditFormat="Custom">
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                            <DropDownButton Text="Start">
                                                            </DropDownButton>
                                                        </dxe:ASPxDateEdit>
                                                    </td>
                                                    <%--</tr>
                                                    <tr>--%>
                                                    <td class="Ecoheadtxt" style="text-align: left; width: 63%;">
                                                        <dxe:ASPxDateEdit ID="txtEnd2" ClientInstanceName="txtEnd2" runat="server" UseMaskBehavior="True"
                                                            EditFormat="Custom">
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                            <DropDownButton Text="End">
                                                            </DropDownButton>
                                                        </dxe:ASPxDateEdit>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="1" style="text-align: left;">
                                                        <input id="Filter2" type="button" value="Show" class="btnUpdate" onclick="btnFilter2_click();"
                                                            style="width: 80px" runat="server" validationgroup="d" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--<dxe:ASPxButton ID="btnAuthorize" runat="server" AutoPostBack="False" Text="Athorize Selected Record">
                                                    <ClientSideEvents Click="function (s, e) {AuthorizeRecords(); }" />
                                                </dxe:ASPxButton>--%>
                                        <td style="text-align: left; width: 995px; vertical-align: top;" id="FormGrid">
                                            <dxe:ASPxGridView ID="GridReminder" ClientInstanceName="grid" runat="server" KeyFieldName="Rid"
                                                AutoGenerateColumns="False" OnCustomCallback="GridReminder_CustomCallback" OnCustomJSProperties="GridReminder_CustomJSProperties"
                                                OnHtmlRowCreated="GridReminder_HtmlRowCreated" Width="100%">
                                                <Styles>
                                                    <LoadingPanel ImageSpacing="10px">
                                                    </LoadingPanel>
                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                    </Header>
                                                </Styles>
                                                <Settings ShowHorizontalScrollBar="true" />
                                                <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True">
                                                    <FirstPageButton Visible="True">
                                                    </FirstPageButton>
                                                    <LastPageButton Visible="True">
                                                    </LastPageButton>
                                                </SettingsPager>
                                                <SettingsBehavior AllowFocusedRow="True" AllowSort="False" />
                                                <%--<ClientSideEvents FocusedRowChanged="function(s, e) { OnGridFocusedRowChanged(); }" />--%>
                                                <ClientSideEvents SelectionChanged="function(s, e) { OnGridFocusedRowChanged(); }" />
                                                <Columns>
                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="10px" VisibleIndex="0">
                                                        <HeaderTemplate>
                                                            <dxe:ASPxCheckBox ID="cbAll" runat="server" ClientInstanceName="cbAll" ToolTip="Select all rows"
                                                                Checked="true" BackColor="White" OnInit="cbAll_Init">
                                                                <ClientSideEvents CheckedChanged="OnAllCheckedChanged" />
                                                            </dxe:ASPxCheckBox>
                                                            <%-- <dxe:ASPxCheckBox ID="cbPage" runat="server" ClientInstanceName="cbPage" ToolTip="Select all rows within the page"
                                                        OnInit="cbPage_Init">
                                                        <ClientSideEvents CheckedChanged="OnPageCheckedChanged" />
                                                    </dxe:ASPxCheckBox>--%>
                                                        </HeaderTemplate>
                                                    </dxe:GridViewCommandColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Create By" FieldName="CreateBy" ReadOnly="True"
                                                        VisibleIndex="1">
                                                        <EditFormSettings Visible="False" />
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Create Date" FieldName="CreateDate" ReadOnly="True"
                                                        VisibleIndex="2">
                                                        <EditFormSettings Visible="False" />
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Token #" FieldName="Rid" ReadOnly="True" VisibleIndex="3">
                                                        <EditFormSettings Visible="False" />
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Task For" FieldName="Target" ReadOnly="True"
                                                        VisibleIndex="4">
                                                        <EditFormSettings Visible="False" />
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Follow-up By" FieldName="incharge" ReadOnly="True"
                                                        VisibleIndex="5">
                                                        <EditFormSettings Visible="False" />
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn Caption="Comp" FieldName="company" ReadOnly="True"
                                                        VisibleIndex="6">
                                                        <EditFormSettings Visible="False" />
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Subject" FieldName="shortname" ReadOnly="True"
                                                        VisibleIndex="7">
                                                        <EditFormSettings Visible="False" />
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn Caption="Task" FieldName="Content" ReadOnly="True"
                                                        VisibleIndex="8">
                                                        <EditFormSettings Visible="False" />
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Task updates" FieldName="replycontent" ReadOnly="True"
                                                        VisibleIndex="9">
                                                        <EditFormSettings Visible="False" />
                                                        <%-- <DataItemTemplate>
                                        <a href="javascript:void(0);" onclick="OnCreateActivityClick('<%# Eval("Rid") %>')">
                                            View</a>
                                    
</DataItemTemplate>--%>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Prty" FieldName="priority" ReadOnly="True"
                                                        VisibleIndex="10">
                                                        <EditFormSettings Visible="False" />
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Start By" FieldName="StartDate" ReadOnly="True"
                                                        VisibleIndex="11">
                                                        <EditFormSettings Visible="False" />
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Finish By" FieldName="EndDate" ReadOnly="True"
                                                        VisibleIndex="12">
                                                        <EditFormSettings Visible="False" />
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Status" FieldName="Status" ReadOnly="True"
                                                        VisibleIndex="13">
                                                        <EditFormSettings Visible="False" />
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Attend Time" FieldName="attenddate" ReadOnly="True"
                                                        VisibleIndex="14">
                                                        <EditFormSettings Visible="False" />
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Type" FieldName="flag" ReadOnly="True" VisibleIndex="15">
                                                        <EditFormSettings Visible="False" />
                                                    </dxe:GridViewDataTextColumn>
                                                </Columns>
                                            </dxe:ASPxGridView>
                                        </td>
                                    </tr>
                                </table>
                                <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                                </dxe:ASPxGridViewExporter>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <%--<tr>
                    <td class="EHEADER" style="text-align: center">
                        <strong><span style="color: #F96410; font-size: 10pt;">Messages</span></strong></td>
                </tr>--%>
            <%-- <tr>
                    <td style="text-align: left">
                        <input id="btnInbox" type="button" value="Inbox" class="btnUpdate" onclick="btnInbox_Click();"
                            style="width: 80px; height: 19px" tabindex="4" />
                        <input id="btnRead" type="button" value="Read" class="btnUpdate" onclick="btnRead_click();"
                            style="width: 80px; height: 19px" tabindex="1" />&nbsp;
                        <input id="btnReply" type="button" value="Reply" class="btnUpdate" onclick="btnReply_Click();"
                            style="width: 80px; height: 19px" tabindex="1" />&nbsp;
                    </td>
                </tr>
                <tr id="FormGridM">
                    <td>
                        <dxe:ASPxGridView ID="GridMessage" ClientInstanceName="gridM" runat="server" Width="100%"
                            KeyFieldName="Mid" OnCustomCallback="GridMessage_CustomCallback" AutoGenerateColumns="False">
                            <Styles>
                                <LoadingPanel ImageSpacing="10px">
                                </LoadingPanel>
                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                </Header>
                            </Styles>
                            <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True">
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </SettingsPager>
                            <SettingsBehavior AllowMultiSelection="True" />
                            <ClientSideEvents SelectionChanged="function(s, e) { OnGridSelectionChanged(); }" />
                            <Columns>
                                <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="3%">
                                    <HeaderTemplate>
                                        <input type="checkbox" onclick="gridM.SelectAllRowsOnPage(this.checked);" style="vertical-align: middle;"
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
                                <dxe:GridViewDataTextColumn Caption="Status" FieldName="Status" ReadOnly="True"
                                    VisibleIndex="4">
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                        </dxe:ASPxGridView>
                    </td>
                </tr>
                <tr id="trRepluForm">
                    <td>
                        <table>
                            <tr>
                                <td class="gridcellright" style="width: 15%">
                                    <span style="font-size: 10px; color: #000099">Use Templates:</span></td>
                                <td style="display: none;" id="TDtemplate" class="gridcellleft">
                                    <asp:DropDownList ID="cmbTemplate" runat="server" Width="151px" Font-Size="12px">
                                    </asp:DropDownList>
                                    <a id="btnHideTemplate" href="javascript:void(0);" onclick="btnHideTemplate_click()">
                                        <span style="color: #000099; text-decoration: underline">Hide</span></a>&nbsp;&nbsp;
                                </td>
                                <td class="gridcellleft" id="TDshow">
                                    <a id="btnShowTemplate" href="javascript:void(0);" onclick="btnShowTemplate_click()">
                                        <span style="color: #000099; text-decoration: underline">Show</span></a>
                                </td>
                            </tr>
                            <tr id="ReplyUserName">
                                <td class="gridcellright" style="width: 15%">
                                    <span style="font-size: 10px; color: #000099">Created For:</span></td>
                                <td colspan="2" class="gridcellleft">
                                    <asp:TextBox ID="txtRelplyUser" runat="server" Width="269px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="TRcontent">
                                <td class="gridcellright" style="width: 15%">
                                    <span style="font-size: 10px; color: #000099">Content:</span></td>
                                <td colspan="2" class="gridcellleft">
                                    <asp:TextBox ID="txtContentM" runat="server" TextMode="MultiLine" Width="700px" Height="48px"
                                        Font-Size="12px"></asp:TextBox></td>
                            </tr>
                            <tr id="TRreplied">
                                <td>
                                </td>
                                <td class="gridcellleft">
                                    <asp:TextBox ID="txtReply" runat="server" TextMode="MultiLine" Width="700px" Font-Size="12px"
                                        ReadOnly="true" Height="48px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="TRbutton">
                                <td>
                                </td>
                                <td colspan="2" class="gridcellleft">
                                    <input id="btnReplydata" type="button" value="Send" class="btnUpdate" onclick="btnSend_click();"
                                        style="width: 66px; height: 19px" tabindex="4" />
                                    <input id="Button1" type="button" value="Cancel" class="btnUpdate" onclick="btnCancelM_click();"
                                        style="width: 66px; height: 19px" tabindex="4" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>--%>
        </table>
        <table>
            <tr>
                <td>
                    <div id="display" runat="server">
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
