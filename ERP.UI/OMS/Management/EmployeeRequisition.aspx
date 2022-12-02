<%@ Page Title="Requisition" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.management_EmployeeRequisition" EnableEventValidation="false" CodeBehind="EmployeeRequisition.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxpc" %>

<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxp" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--External Styles-->
    <link type="text/css" href="../CSS/GenericCss.css" rel="Stylesheet" />
    <!--External Scripts file-->
    <!-- Ajax List Requierd-->
    <link type="text/css" href="../CSS/AjaxStyle.css" rel="Stylesheet" />
    <script type="text/javascript" src="/assests/js/ajaxList_wofolder.js"></script>
    <script type="text/javascript" src="/assests/js/init.js"></script>
    <!--Other Script-->
    <script type="text/javascript" src="/assests/js/GenericJScript.js"></script>
    <!--JS Inline Method-->
    <!--Page and Filter Script -->
    <script type="text/javascript" language="javascript">
        function Page_Load() {
            HideShow('lblCheckDate', 'H');
            HideShow('Container2', 'H');
            HideShow('btnShowFilter', 'H');
            HideShow("BtnGenerateRequest", "H");
            HideShow("BtnAllGenerateRequest", "H");
            HideShow("BtnDeleteRequest", "H");
            HideShow("BtnAllDeleteRequest", "H");
            HideShow('Row2', 'H');
            SetValue('HDNCheckedEmpID', '');
            SetValue("HDNSearchBy", "ToReq");  //To Search By To Be Requisitioned                
        }         
        function Reset() {
            HideShow('Container1', 'H');
            HideShow('C1_Row10', 'S');
            SetValue('hdnSelectedOption', '');
            SetValue('HDNEmployee', '');
            SetValue('HDNCompany', '');
            SetValue('HDNBranch', '');
            SetValue('HDNReportTo', '');
            SetValue('HDNEmpType', '');
            cRblEmployee.SetSelectedIndex(0);
            cRblCompany.SetSelectedIndex(0);
            cRblBranch.SetSelectedIndex(0);
            cRblRptTo.SetSelectedIndex(0);
            cRblEmpType.SetSelectedIndex(0);
            cCmbType.SetSelectedIndex(0);
            cCmbExported.SetSelectedIndex(0);
            cCmbAuthorized.SetSelectedIndex(0);
            cCmbReportType.SetSelectedIndex(0);
            SetValue('HDNCheckedEmpID', '');
            height();
        }
        function DateChange(positionDate) {
            var FYS = '<%=Session["FinYearStart"]%>';
            var FYE = '<%=Session["FinYearEnd"]%>';
            var LFY = '<%=Session["LastFinYear"]%>';
            DevE_CheckForFinYear(positionDate, FYS, FYE, LFY);
        }
        function CompareFromDate() {
            var setFromDate = '<%=currentFromDate%>';
            CompareDate(cDtFrom.GetDate(), cDtTo.GetDate(), 'LE', 'From Date Can Not Be Greater Than To Date', cDtFrom, setFromDate);
        }
        function CompareToDate() {
            var setToDate = '<%=currentToDate%>';
            CompareDate(cDtFrom.GetDate(), cDtTo.GetDate(), 'LE', 'To Date Can Not Be Less Than From Date', cDtTo, setToDate);
        }
        function fn_OnMouseOverChkDate() {
            var ChkDate = GetObjectID('chkAllDate');
            if (ChkDate.checked == true) {
                ChkDate.setAttribute('title', 'Uncheck For Search Selected Date');
            }
            else {
                ChkDate.setAttribute('title', 'Check For Search All Date');
            }
        }
        function fn_OnClickAllDate() {
            var AllCheckedDate = GetObjectID('chkAllDate');
            var fromDate = ChangeDateFormat_SetCalenderValue('01-01-1900');
            var toDate = ChangeDateFormat_SetCalenderValue('12-12-9999');
            if (AllCheckedDate.checked == true) {
                cDtFrom.SetDate(fromDate);
                cDtTo.SetDate(toDate);
                HideShow('C1_Row0_Col2', 'H');
                HideShow('C1_Row0_Col3', 'H');
                HideShow('lblCheckDate', 'S');
            }
            else {
                cDtFrom.SetDate(ChangeDateFormat_SetCalenderValue('<%=currentFromDate%>'));
                cDtTo.SetDate(ChangeDateFormat_SetCalenderValue('<%=currentToDate%>'));
                HideShow('lblCheckDate', 'H');
                HideShow('C1_Row0_Col2', 'S');
                HideShow('C1_Row0_Col3', 'S');
            }
        }
        function ChangeDateFormat_SetCalenderValue(obj) {
            var SelectedDate = new Date(obj);
            var monthnumber = SelectedDate.getMonth();
            var monthday = SelectedDate.getDate();
            var year = SelectedDate.getYear();
            var changeDateValue = new Date(year, monthnumber, monthday);
            return changeDateValue;
        }
        function fn_Employee(obj) {
            if (obj == "A") {
                HideShow('Container2', 'H');
                HideShow('C1_Row10', 'S');
            }
            else if (obj == "S") {
                if (GetObjectID('Container2').style.display == "inline") {
                    cRblEmployee.SetSelectedIndex(0);
                    lnkBtnAddFinalSelection();
                }
                else {
                    HideShow('Container2', 'S');
                    SetValue('hdnSelectedOption', 'Employee');
                    HideShow('C1_Row10', 'H');
                    CallServer("CallAjax-Employee", "");
                    GetObjectID('txtSelectionID').focus();
                }
            }
        }
        function fn_Company(obj) {
            if (obj == "A") {
                HideShow('Container2', 'H');
                HideShow('C1_Row10', 'S');
            }
            else if (obj == "S") {
                if (GetObjectID('Container2').style.display == "inline") {
                    cRblCompany.SetSelectedIndex(0);
                    lnkBtnAddFinalSelection();
                }
                else {
                    HideShow('Container2', 'S');
                    SetValue('hdnSelectedOption', 'Company');
                    HideShow('C1_Row10', 'H');
                    CallServer("CallAjax-Company", "");
                    GetObjectID('txtSelectionID').focus();
                }
            }
        }
        function fn_Branch(obj) {
            if (obj == "A") {
                HideShow('Container2', 'H');
                HideShow('C1_Row10', 'S');
            }
            else if (obj == "S") {
                if (GetObjectID('Container2').style.display == "inline") {
                    cRblBranch.SetSelectedIndex(0);
                    lnkBtnAddFinalSelection();
                }
                else {
                    HideShow('Container2', 'S');
                    SetValue('hdnSelectedOption', 'Branch');
                    HideShow('C1_Row10', 'H');
                    CallServer("CallAjax-Branch", "");
                    GetObjectID('txtSelectionID').focus();
                }
            }
        }
        function fn_ReportTo(obj) {
            if (obj == "A") {
                HideShow('Container2', 'H');
                HideShow('C1_Row10', 'S');
            }
            else if (obj == "S") {
                if (GetObjectID('Container2').style.display == "inline") {
                    cRblRptTo.SetSelectedIndex(0);
                    lnkBtnAddFinalSelection();
                }
                else {
                    HideShow('Container2', 'S');
                    SetValue('hdnSelectedOption', 'ReportTo');
                    HideShow('C1_Row10', 'H');
                    CallServer("CallAjax-ReportTo", "");
                    GetObjectID('txtSelectionID').focus();
                }
            }
        }
        function fn_EmpType(obj) {
            if (obj == "A") {
                HideShow('Container2', 'H');
                HideShow('C1_Row10', 'S');
            }
            else if (obj == "S") {
                if (GetObjectID('Container2').style.display == "inline") {
                    cRblEmpType.SetSelectedIndex(0);
                    lnkBtnAddFinalSelection();
                }
                else {
                    HideShow('Container2', 'S');
                    SetValue('hdnSelectedOption', 'EmployeeType');
                    HideShow('C1_Row10', 'H');
                    CallServer("CallAjax-EmployeeType", "");
                    GetObjectID('txtSelectionID').focus();
                }
            }
        }
        function HideFilter() {
            HideShow('Row0', 'H');
            HideShow('btnShowFilter', 'S');
            height();
        }
    </script>
    <!-- CallAjax and Receive Server Script-->
    <script language="javascript" type="text/javascript">
        FieldName = 'none';
        function btnAddToList_click() {
            var txtName = GetObjectID('txtSelectionID');
            if (txtName != '') {
                var txtId = GetValue('txtSelectionID_hidden');
                var listBox = GetObjectID('lstSelection');
                var listLength = listBox.length;
                var opt = new Option();
                opt.value = txtId;
                opt.text = txtName.value;
                listBox[listLength] = opt;
                txtName.value = '';
            }
            else
                alert('Please Search Name And Then Add!');
            txtName.focus();
            txtName.select();
        }
        function lnkBtnAddFinalSelection() {
            var listBox = GetObjectID('lstSelection');
            var listID = '';
            var i;
            var hidden_SelectedCategory = GetValue('hdnSelectedOption');
            if (listBox.length > 0) {
                for (i = 0; i < listBox.length; i++) {
                    if (listID == '')
                        listID = listBox.options[i].value + '!' + listBox.options[i].text;
                    else
                        listID += '^' + listBox.options[i].value + '!' + listBox.options[i].text;
                }
                CallServer(listID, "");
                var j;
                for (j = listBox.options.length - 1; j >= 0; j--) {
                    listBox.remove(j);
                }
                HideShow('Container2', 'H');
                HideShow('C1_Row10', 'S');
            }
            else if ((GetObjectID('Container2').style.display == "inline") && (listBox.length == 0)) {
                if (cRblEmployee.GetSelectedIndex() == 1) {
                    alert("Please Select Atleast One Employee Item!!!");
                }
                else if (cRblCompany.GetSelectedIndex() == 1) {
                    alert("Please Select Atleast One Company Item!!!");
                }
                else if (cRblBranch.GetSelectedIndex() == 1) {
                    alert("Please Select Atleast One Branch Item!!!");
                }
                else if (cRblRptTo.GetSelectedIndex() == 1) {
                    alert("Please Select Atleast One Report To Item!!!");
                }
                else if (cRblEmpType.GetSelectedIndex() == 1) {
                    alert("Please Select Atleast One Employee Type Item!!!");
                }
            }
        }
        function lnkBtnRemoveFromSelection() {
            var listBox = GetObjectID('lstSelection');
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
            var Data = rValue.split('@');
            if (Data[1] != "undefined") {
                if (Data[0] == 'Employee') {
                    SetValue('HDNEmployee', Data[1]);
                }
                else if (Data[0] == 'Company') {
                    SetValue('HDNCompany', Data[1]);
                }
                else if (Data[0] == 'Branch') {
                    SetValue('HDNBranch', Data[1]);
                }
                else if (Data[0] == 'ReportTo') {
                    SetValue('HDNReportTo', Data[1]);
                }
                else if (Data[0] == 'EmployeeType') {
                    SetValue('HDNEmpType', Data[1]);
                }
            }
            if (Data[0] == 'AjaxQuery') {
                AjaxComQuery = Data[1];
                var AjaxList_TextBox = GetObjectID('txtSelectionID');
                AjaxList_TextBox.value = '';
                AjaxList_TextBox.attachEvent('onkeyup', CallGenericAjaxJS);
            }
        }
        function CallGenericAjaxJS(e) {
            var AjaxList_TextBox = GetObjectID('txtSelectionID');
            AjaxList_TextBox.focus();
            AjaxComQuery = AjaxComQuery.replace("\'", "'");
            ajax_showOptions(AjaxList_TextBox, 'GenericAjaxList', e, replaceChars(AjaxComQuery), 'Main');
        }
        function NORECORD(obj) {
            HideShow('Container2', 'H');
            HideShow('C1_Row10', 'S');
            if (obj == '2') {
                alert('No Record Found !! ');
                Reset();
            }
            height();
        }
    </script>
    <!-- GridView and Paging Script-->
    <script language="javascript" type="text/javascript">
        function OnLeftNav_Click() {
            var i = GetObjectID("A1").innerText;
            //GetObjectID('A1').className="number_box_selected";
            SetValue("hdn_GridBindOrNotBind", "False"); //To Stop Bind On Page Load
            if (parseInt(i) > 1)
                cGrdEmployeeRequisition.PerformCallback("SearchByNavigation~" + cDtFrom.GetValue() + '~' + cDtTo.GetValue() + "~" + GetObjectID("A1").innerText + "~LeftNav");
            else
                alert('No More Pages.');
        }
        function OnRightNav_Click() {
            var TestEnd = GetObjectID("A10").innerText;
            SetValue("hdn_GridBindOrNotBind", "False"); //To Stop Bind On Page Load
            var TotalPage = GetObjectID("B_TotalPage").innerText;
            if (TestEnd == "" || TestEnd == TotalPage) {
                alert('No More Records.');
                return;
            }
            var i = GetObjectID("A1").innerText;
            //GetObjectID('A1').className="number_box_selected";
            if (parseInt(i) < TotalPage)
                cGrdEmployeeRequisition.PerformCallback("SearchByNavigation~" + cDtFrom.GetValue() + '~' + cDtTo.GetValue() + "~" + GetObjectID("A1").innerText + "~RightNav");
            else
                alert('You are at the End');
        }
        function OnPageNo_Click(obj) {
            var i = GetObjectID(obj).innerText;
            //GetObjectID('A1').className="number_box";
            //GetObjectID(obj).className="number_box_selected";
            SetValue("hdn_GridBindOrNotBind", "False"); //To Stop Bind On Page Load
            cGrdEmployeeRequisition.PerformCallback("SearchByNavigation~" + cDtFrom.GetValue() + '~' + cDtTo.GetValue() + "~" + i + "~PageNav");
        }
        function BtnShow_Click() {
            SetValue("hdn_GridBindOrNotBind", "False"); //To Stop Bind On Page Load       
            if (GetObjectID("HDNSearchBy").value == "Req") {
                HideShow("BtnGenerateRequest", "H");
                HideShow("BtnAllGenerateRequest", "H");
                HideShow("BtnDeleteRequest", "S");
                HideShow("BtnAllDeleteRequest", "S");
                GetObjectID("spnReqType").innerHTML = " Already Requisitioned Of " + cCmbType.GetText();
            }
            else {
                HideShow("BtnGenerateRequest", "S");
                HideShow("BtnAllGenerateRequest", "S");
                HideShow("BtnDeleteRequest", "H");
                HideShow("BtnAllDeleteRequest", "H");
                GetObjectID("spnReqType").innerHTML = " Of To Be Requisitioned For " + cCmbType.GetText();
            }
            checkAll("frm_Employee_Requisition", false);// To UnCheck all selected Rows
            cGrdEmployeeRequisition.PerformCallback("Show~" + cDtFrom.GetValue() + '~' + cDtTo.GetValue());
        }
        function BtnShowFilter_Click() {
            HideShow('btnShowFilter', 'H');
            HideShow('Row0', 'S');
            height();
        }
        function BtnAllGenerateRequest_Click() {
            cGrdEmployeeRequisition.PerformCallback("GenerateAll~" + cDtFrom.GetValue() + '~' + cDtTo.GetValue());
            cBtnAllGenerateRequest.SetEnabled(false);
        }
        function BtnGenerateRequest_Click() {
            //alert('BtnGenerateRequest-'+GetObjectID('HDNCheckedEmpID').value);
            cGrdEmployeeRequisition.PerformCallback("Generate~" + cDtFrom.GetValue() + '~' + cDtTo.GetValue());
            cBtnGenerateRequest.SetEnabled(false);
        }
        function BtnAllDeleteRequest_Click() {
            cGrdEmployeeRequisition.PerformCallback("DeleteAll~" + cDtFrom.GetValue() + '~' + cDtTo.GetValue());
            cBtnAllDeleteRequest.SetEnabled(false);
        }
        function BtnDeleteRequest_Click() {
            //alert('BtnDeleteRequest-'+GetObjectID('HDNCheckedEmpID').value);
            cGrdEmployeeRequisition.PerformCallback("Delete~" + cDtFrom.GetValue() + '~' + cDtTo.GetValue());
            cBtnDeleteRequest.SetEnabled(false);
        }
        //==========GridView CheckBox Selection Script=======================        
        function OnGridSelectionChanged() {
            cGrdEmployeeRequisition.GetSelectedFieldValues('ContactID', OnGetRowValues);
        }
        function OnGetRowValues(values) {
            if (values != null || values != undefined)
                SetValue("HDNCheckedEmpID", values);
        }
        //===============End CheckBox Selection==========================    
        function GrdEmployeeRequisition_EndCallBack() {
            if (cGrdEmployeeRequisition.cpExcelExport != undefined) {
                GetObjectID('BtnForExportEvent').click();
            }
            if (cGrdEmployeeRequisition.cpRefreshNavPanel != undefined) {
                GetObjectID("B_PageNo").innerText = '';
                GetObjectID("B_TotalPage").innerText = '';
                GetObjectID("B_TotalRows").innerText = '';

                var NavDirection = cGrdEmployeeRequisition.cpRefreshNavPanel.split('~')[0];
                var PageNum = cGrdEmployeeRequisition.cpRefreshNavPanel.split('~')[1];
                var TotalPage = cGrdEmployeeRequisition.cpRefreshNavPanel.split('~')[2];
                var TotalRows = cGrdEmployeeRequisition.cpRefreshNavPanel.split('~')[3];

                if (NavDirection == "RightNav") {
                    PageNum = parseInt(PageNum) + 10;
                    GetObjectID("B_PageNo").innerText = PageNum;
                    GetObjectID("B_TotalPage").innerText = TotalPage;
                    GetObjectID("B_TotalRows").innerText = TotalRows;
                    var n = parseInt(TotalPage) - parseInt(PageNum) > 10 ? parseInt(11) : parseInt(TotalPage) - parseInt(PageNum) + 2;
                    for (r = 1; r < n; r++) {
                        var obj = "A" + r;
                        GetObjectID(obj).innerText = " " + PageNum++ + " ";
                        //GetObjectID(obj).className="number_box";
                    }
                    for (r = n; r < 11; r++) {
                        var obj = "A" + r;
                        GetObjectID(obj).innerText = "";
                    }
                }
                if (NavDirection == "LeftNav") {
                    if (parseInt(PageNum) > 1) {
                        PageNum = parseInt(PageNum) - 10;
                        GetObjectID("B_PageNo").innerText = PageNum;
                        GetObjectID("B_TotalPage").innerText = TotalPage;
                        GetObjectID("B_TotalRows").innerText = TotalRows;
                        for (l = 1; l < 11; l++) {
                            var obj = "A" + l;
                            GetObjectID(obj).innerText = " " + PageNum++ + " ";
                            //GetObjectID(obj).className="number_box";                       
                        }
                    }
                    else {
                        alert('No More Pages.');
                    }
                }
                if (NavDirection == "PageNav") {
                    GetObjectID("B_PageNo").innerText = PageNum;
                    GetObjectID("B_TotalPage").innerText = TotalPage;
                    GetObjectID("B_TotalRows").innerText = TotalRows;
                }
                if (NavDirection == "ShowBtnClick") {
                    GetObjectID("B_PageNo").innerText = PageNum;
                    GetObjectID("B_TotalPage").innerText = TotalPage;
                    GetObjectID("B_TotalRows").innerText = TotalRows;
                    var n = parseInt(TotalPage) - parseInt(PageNum) > 10 ? parseInt(11) : parseInt(TotalPage) - parseInt(PageNum) + 2;
                    for (r = 1; r < n; r++) {
                        var obj = "A" + r;
                        GetObjectID(obj).innerText = " " + PageNum++ + " ";
                    }
                    for (r = n; r < 11; r++) {
                        var obj = "A" + r;
                        GetObjectID(obj).innerText = "";
                    }
                    //GetObjectID('A1').className="number_box_selected";                
                }
            }
            if (cGrdEmployeeRequisition.cpSetGlobalFields != undefined) {
                SetValue("Hdn_PageSize", cGrdEmployeeRequisition.cpSetGlobalFields.split('~')[0]);
                SetValue("Hdn_PageNumber", cGrdEmployeeRequisition.cpSetGlobalFields.split('~')[1]);
                SetValue("Hdn_Emp_ContactID", cGrdEmployeeRequisition.cpSetGlobalFields.split('~')[2]);
                SetValue("Hdn_Exported", cGrdEmployeeRequisition.cpSetGlobalFields.split('~')[3]);
                SetValue("Hdn_Authorized", cGrdEmployeeRequisition.cpSetGlobalFields.split('~')[4]);
                SetValue("Hdn_DateFrom", cGrdEmployeeRequisition.cpSetGlobalFields.split('~')[5]);
                SetValue("Hdn_DateTo", cGrdEmployeeRequisition.cpSetGlobalFields.split('~')[6]);
                SetValue("Hdn_Company", cGrdEmployeeRequisition.cpSetGlobalFields.split('~')[7]);
                SetValue("Hdn_Branch", cGrdEmployeeRequisition.cpSetGlobalFields.split('~')[8]);
                SetValue("Hdn_ReportTo", cGrdEmployeeRequisition.cpSetGlobalFields.split('~')[9]);
                SetValue("Hdn_EmployeeType", cGrdEmployeeRequisition.cpSetGlobalFields.split('~')[10]);
                SetValue("Hdn_Type", cGrdEmployeeRequisition.cpSetGlobalFields.split('~')[11]);
                SetValue("Hdn_ReportType", cGrdEmployeeRequisition.cpSetGlobalFields.split('~')[12]);
                HideFilter();
            }
            if (cGrdEmployeeRequisition.cpInfoMsg != undefined) {
                alert(cGrdEmployeeRequisition.cpInfoMsg);

                cBtnAllGenerateRequest.SetEnabled(true);
                cBtnGenerateRequest.SetEnabled(true);
                cBtnAllDeleteRequest.SetEnabled(true);
                cBtnDeleteRequest.SetEnabled(true);
                SetValue('HDNCheckedEmpID', '');
                checkAll("frm_Employee_Requisition", false);// To UnCheck all selected Rows                              
                cGrdEmployeeRequisition.PerformCallback("Show~" + cDtFrom.GetValue() + '~' + cDtTo.GetValue());
            }
            //checkAll("frm_Employee_Requisition",false);// To UnCheck all selected Rows      
            //Now Reset GridBindOrNotBind to True for Next Page Load
            SetValue("hdn_GridBindOrNotBind", "True");
            height();
        }
        function OnCmbExcelExportChanged() {
            SetValue("hdn_GridBindOrNotBind", "False");
            cGrdEmployeeRequisition.PerformCallback("ExcelExport~" + cDtFrom.GetValue() + '~' + cDtTo.GetValue());
            cCmbExcelExport.SetSelectedIndex(0);
        }
        function checkAll(formname, checktoggle) {
            var checkboxes = new Array();
            checkboxes = document[formname].getElementsByTagName('input');
            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].type == 'checkbox') {
                    checkboxes[i].checked = checktoggle;
                }
            }
        }
    </script>
    <!-- Accordin Java Script-->
    <script type="text/javascript" language="javascript">
        var ContentHeight = 310;
        var TimeToSlide = 280.0;
        var openAccordion = '';
        function runAccordion(index) {
            var nID = "Accordion" + index + "Content";
            if (openAccordion == nID)
                nID = '';
            setTimeout("animate(" + new Date().getTime() + "," + TimeToSlide + ",'"
                + openAccordion + "','" + nID + "')", 33);
            openAccordion = nID;

        }
        function animate(lastTick, timeLeft, closingId, openingId) {
            var curTick = new Date().getTime();
            var elapsedTicks = curTick - lastTick;
            var opening = (openingId == '') ? null : GetObjectID(openingId);
            var closing = (closingId == '') ? null : GetObjectID(closingId);
            if (timeLeft <= elapsedTicks) {
                if (opening != null)
                    opening.style.height = ContentHeight + 'px';
                if (closing != null) {
                    closing.style.display = 'none';
                    closing.style.height = '0px';
                }
                if ((GetObjectID('T1').innerHTML == '[ + ]') && (GetObjectID('Accordion1Content').style.display == 'block')) {
                    GetObjectID('T1').innerHTML = "[ - ]";
                    cbtnAdd.SetText("Show Employee (Already Requisitioned)");
                    SetValue("HDNSearchBy", "Req");  //To Search By Already Requisitioned                            
                }
                else {
                    GetObjectID('T1').innerHTML = "[ + ]";
                    cbtnAdd.SetText("Show Employee (To Be Requisitioned)");
                    SetValue("HDNSearchBy", "ToReq");  //To Search By To Be Requisitioned     
                }
                return;
            }
            timeLeft -= elapsedTicks;
            var newClosedHeight = Math.round((timeLeft / TimeToSlide) * ContentHeight);
            if (opening != null) {
                if (opening.style.display != 'block')
                    opening.style.display = 'block';
                opening.style.height = (ContentHeight - newClosedHeight) + 'px';
            }
            if (closing != null)
                closing.style.height = newClosedHeight + 'px';

            setTimeout("animate(" + curTick + "," + timeLeft + ",'"
                + closingId + "','" + openingId + "')", 33);

            height();
        }
    </script>
    <!--Page Style-->
    <style type="text/css">
        #Container1 {
            width: 580px;
        }

        #Container2 {
            width: 420px;
            display: none;
        }

        .LableWidth {
            width: 150px !important;
        }

        .ContentWidth {
            width: 170px;
            height: 21px;
        }

        .inputCheck {
            width: 24px;
            height: 25px;
            margin: -2px;
        }

        .labelCont {
            font-size: 13px;
            margin-top: 7px;
        }

        .btnRight {
            margin-right: 10px;
            float: right;
            margin-top:15px;
        }

        .BtnPagingWidth {
            float: right;
            width: 37%;
        }
    </style>
    <!--- Accordin Style-->
    <style type="text/css">
        /**/.AccordionTitle, .AccordionContent, .AccordionContainer {
            position: relative;
        }

        .AccordionTitle {
            margin-right: 10px;
            height: 20px;
            overflow: hidden;
            cursor: pointer;
            font-size: 9pt;
            vertical-align: middle;
            -moz-user-select: none;
        }

        .AccordionContent {
            height: 0px;
            overflow: auto;
            display: block;
        }
        .LFloat_Lable,
        .Content, .LFloat_Content {
            border:none !important;
        }
        #Accordion1Content {
            float: left;
            width: 100%;
        }
        #Accordion1Content .LableWidth {
            margin-bottom:8px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Requisition</h3>
        </div>
    </div>
    <div class="form_main">
        <div id="Gmain">
            <!--<div id="header" class="Header">
                Requisition For Appt. Letter/I-Card/V-Card
            </div>-->
            <div id="Row0" class="row">
                <div class="col-md-2" id="C1_Row0_Col1">
                    <label>
                        Joining Date :
                    </label>
                    <div>
                        <dxe:ASPxDateEdit ID="DtFrom" runat="server" ClientInstanceName="cDtFrom" DateOnError="Today"
                            EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" TabIndex="0" Width="100%">
                            <ClientSideEvents DateChanged="function(s,e){CompareFromDate();}"></ClientSideEvents>
                            <DropDownButton Text="From">
                            </DropDownButton>
                        </dxe:ASPxDateEdit>
                    </div>
                </div>
                <div class="col-md-2" id="C1_Row0_Col3">
                    <label>
                        &nbsp
                    </label>
                    <div>
                        <dxe:ASPxDateEdit ID="DtTo" runat="server" ClientInstanceName="cDtTo" DateOnError="Today"
                            EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" TabIndex="0" Width="100%">
                            <ClientSideEvents DateChanged="function(s,e){CompareToDate();}"></ClientSideEvents>
                            <DropDownButton Text="To">
                            </DropDownButton>
                        </dxe:ASPxDateEdit>
                    </div>
                </div>
                <div class="col-md-2">
                    <label id="C1_Row1_Col1" >
                        Type :
                    </label>
                    <div id="C1_Row1_Col2">
                        <dxe:ASPxComboBox ID="CmbType" runat="server" ValueType="System.String" ClientInstanceName="cCmbType"
                            SelectedIndex="0" TabIndex="0" Width="100%">
                            <Items>
                                <dxe:ListEditItem Text="Appointment Letter" Value="A"></dxe:ListEditItem>
                                <dxe:ListEditItem Text="Identity Card" Value="I"></dxe:ListEditItem>
                                <dxe:ListEditItem Text="Visiting Card" Value="V"></dxe:ListEditItem>
                            </Items>
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div style="width:35px;float:left">
                    <label style="display:block;width:100%;">&nbsp</label>
                    <label id="C1_Row0_Col4">
                        <input type="checkbox" class="inputCheck" id="chkAllDate"
                            onclick="fn_OnClickAllDate()" onmouseover="fn_OnMouseOverChkDate()" />
                    </label>
                    <div id="lblCheckDate">Search All Date Wise</div>
                </div>
                <div class="col-md-2">
                    <label >&nbsp</label>
                    <div onclick="runAccordion(1);">
                        <div class="AccordionTitle green right" onselectstart="return false;">
                            <u>Advance Search</u> <span id="T1">[ + ]</span>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
                <div class="col-md-12">
                    <div id="AccordionContainer" class="AccordionContainer">
                        
                        <div id="Accordion1Content" class="AccordionContent">
                            <div id="C1_Row2" class="Row">
                                <div id="C1_Row2_Col1" class="LFloat_Lable LableWidth">
                                    Employee :
                                </div>
                                <div id="C1_Row2_Col2" class="LFloat_Content ContentWidth">
                                    <dxe:ASPxRadioButtonList ID="RblEmployee" runat="server" ClientInstanceName="cRblEmployee"
                                        SelectedIndex="0" ItemSpacing="20px" RepeatDirection="Horizontal" TextWrap="False"
                                        TabIndex="0" Paddings-PaddingTop="1px">
                                        <Items>
                                            <dxe:ListEditItem Text="All" Value="A" />
                                            <dxe:ListEditItem Text="Specific" Value="S" />
                                        </Items>
                                        <ClientSideEvents ValueChanged="function(s, e) {fn_Employee(s.GetValue());}" />
                                        <Border BorderWidth="0px" />
                                    </dxe:ASPxRadioButtonList>
                                </div>
                                <span class="clear"></span>
                            </div>
                            <div id="C1_Row3" class="Row">
                                <div id="C1_Row3_Col1" class="LFloat_Lable LableWidth">
                                    Company :
                                </div>
                                <div id="C1_Row3_Col2" class="LFloat_Content ContentWidth">
                                    <dxe:ASPxRadioButtonList ID="RblCompany" runat="server" ClientInstanceName="cRblCompany"
                                        SelectedIndex="0" ItemSpacing="20px" RepeatDirection="Horizontal" TextWrap="False"
                                        TabIndex="0" Paddings-PaddingTop="1px">
                                        <Items>
                                            <dxe:ListEditItem Text="All" Value="A" />
                                            <dxe:ListEditItem Text="Specific" Value="S" />
                                        </Items>
                                        <ClientSideEvents ValueChanged="function(s, e) {fn_Company(s.GetValue());}" />
                                        <Border BorderWidth="0px" />
                                    </dxe:ASPxRadioButtonList>
                                </div>
                            </div>
                            <div id="C1_Row4" class="Row">
                                <div id="C1_RolblGridHeaderw4_Col1" class="LFloat_Lable LableWidth">
                                    Branch :
                                </div>
                                <div id="C1_Row4_Col2" class="LFloat_Content ContentWidth">
                                    <dxe:ASPxRadioButtonList ID="RblBranch" runat="server" ClientInstanceName="cRblBranch"
                                        SelectedIndex="0" ItemSpacing="20px" RepeatDirection="Horizontal" TextWrap="False"
                                        TabIndex="0" Paddings-PaddingTop="1px">
                                        <Items>
                                            <dxe:ListEditItem Text="All" Value="A" />
                                            <dxe:ListEditItem Text="Specific" Value="S" />
                                        </Items>
                                        <ClientSideEvents ValueChanged="function(s, e) {fn_Branch(s.GetValue());}" />
                                        <Border BorderWidth="0px" />
                                    </dxe:ASPxRadioButtonList>
                                </div>
                            </div>
                            <div id="C1_Row5" class="Row">
                                <div id="C1_Row5_Col1" class="LFloat_Lable LableWidth">
                                    Report To :
                                </div>
                                <div id="C1_Row5_Col2" class="LFloat_Content ContentWidth">
                                    <dxe:ASPxRadioButtonList ID="RblRptTo" runat="server" ClientInstanceName="cRblRptTo"
                                        SelectedIndex="0" ItemSpacing="20px" RepeatDirection="Horizontal" TextWrap="False"
                                        TabIndex="0" Paddings-PaddingTop="1px">
                                        <Items>
                                            <dxe:ListEditItem Text="All" Value="A" />
                                            <dxe:ListEditItem Text="Specific" Value="S" />
                                        </Items>
                                        <ClientSideEvents ValueChanged="function(s, e) {fn_ReportTo(s.GetValue());}" />
                                        <Border BorderWidth="0px" />
                                    </dxe:ASPxRadioButtonList>
                                </div>
                            </div>
                            <div id="C1_Row6" class="Row">
                                <div id="C1_Row6_Col1" class="LFloat_Lable LableWidth">
                                    Employee Type :
                                </div>
                                <div id="C1_Row6_Col2" class="LFloat_Content ContentWidth">
                                    <dxe:ASPxRadioButtonList ID="RblEmpType" runat="server" ClientInstanceName="cRblEmpType"
                                        SelectedIndex="0" ItemSpacing="20px" RepeatDirection="Horizontal" TextWrap="False"
                                        TabIndex="0" Paddings-PaddingTop="1px">
                                        <Items>
                                            <dxe:ListEditItem Text="All" Value="A" />
                                            <dxe:ListEditItem Text="Specific" Value="S" />
                                        </Items>
                                        <ClientSideEvents ValueChanged="function(s, e) {fn_EmpType(s.GetValue());}" />
                                        <Border BorderWidth="0px" />
                                    </dxe:ASPxRadioButtonList>
                                </div>
                            </div>
                            <div id="C1_Row7" class="Row">
                                <div id="C1_Row7_Col1" class="LFloat_Lable LableWidth">
                                    Exported :
                                </div>
                                <div id="C1_Row7_Col2" class="LFloat_Content ContentWidth">
                                    <dxe:ASPxComboBox ID="CmbExported" runat="server" ValueType="System.String" ClientInstanceName="cCmbExported"
                                        SelectedIndex="0" TabIndex="0">
                                        <Items>
                                            <dxe:ListEditItem Text="All" Value="A"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Exported" Value="T"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="UnExported" Value="F"></dxe:ListEditItem>
                                        </Items>
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                            <div id="C1_Row8" class="Row">
                                <div id="C1_Row8_Col1" class="LFloat_Lable LableWidth">
                                    Authorized :
                                </div>
                                <div id="C1_Row8_Col2" class="LFloat_Content ContentWidth">
                                    <dxe:ASPxComboBox ID="CmbAuthorized" runat="server" ValueType="System.String" ClientInstanceName="cCmbAuthorized"
                                        SelectedIndex="0" TabIndex="0">
                                        <Items>
                                            <dxe:ListEditItem Text="All" Value="A"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Authorized" Value="T"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="UnAuthorized" Value="F"></dxe:ListEditItem>
                                        </Items>
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                            <div id="C1_Row9" class="Row">
                                <div id="C1_Row9_Col1" class="LFloat_Lable LableWidth">
                                    Report Type :
                                </div>
                                <div id="C1_Row9_Col2" class="LFloat_Content ContentWidth">
                                    <dxe:ASPxComboBox ID="CmbRptType" runat="server" ValueType="System.String" ClientInstanceName="cCmbRptType"
                                        SelectedIndex="0" TabIndex="0">
                                        <Items>
                                            <dxe:ListEditItem Text="Screen" Value="S"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Excel" Value="E"></dxe:ListEditItem>
                                        </Items>
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="" class="col-md-12">
                    <div id="C1_Row0" class="Row">
                        
                        
                        
                        <span class="clear"></span>
                    </div>
                    <div id="C1_Row1" class="Row">
                        
                    </div>
                    
                    
                    <div id="C1_Row10" class="">
                        <dxe:ASPxButton ID="btnShow" runat="server" CssClass="btn btn-primary" AutoPostBack="False" ClientInstanceName="cbtnAdd"
                            Text="Show Employee (To Be Requisitioned)">
                            <ClientSideEvents Click="function(s,e){BtnShow_Click();}" />
                        </dxe:ASPxButton>
                    </div>
                </div>
                <div id="Container2" class="container">
                    <div id="C2_Row0" class="Row">
                        <div id="C2_Row0_Col1" class="LFloat_Content">
                            <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="323px" TabIndex="0"></asp:TextBox>
                        </div>
                        <div id="C2_Row0_Col2" class="LFloat_Lable">
                            <a href="javascript:void(0);" tabindex="0" onclick="btnAddToList_click()"><span class="lnkBtnAjax green">Add to List</span></a>
                        </div>
                    </div>
                    <div id="C2_Row1" class="Row">
                        <div id="C2_Row1_Col1" class="LFloat_Content finalSelectedBox">
                            <asp:ListBox ID="lstSelection" runat="server" Font-Size="12px" Height="100px" Width="410px"
                                TabIndex="0"></asp:ListBox>
                        </div>
                    </div>
                    <div id="C2_Row2" class="Row">
                        <div id="C2_Row2_Col1" class="LFloat_Lable">
                            <a href="javascript:void(0);" tabindex="0" onclick="lnkBtnAddFinalSelection()"><span
                                class="lnkBtnAjax blue">Done</span></a>&nbsp;&nbsp; <a href="javascript:void(0);"
                                    tabindex="0" onclick="lnkBtnRemoveFromSelection()"><span class="lnkBtnAjax red">Remove</span></a>
                        </div>
                    </div>
                </div>
            </div>
            <div id="Row1" class="Row">
                <div id="lblGridHeader" class="paging textLeft">
                    <span class="pagingContent">Showing Record(s) <span id="spnReqType"></span></span>
                </div>
                <div class="paging textLeft clearfix">
                    <div class="BtnPagingWidth">
                        <dxe:ASPxButton ID="btnShowFilter" runat="server" AutoPostBack="False" ClientInstanceName="cbtnShowFilter"
                            Text="Show Filter" Font-Size="8" TabIndex="0" CssClass="btnRight">
                            <ClientSideEvents Click="function(s,e){BtnShowFilter_Click();}" />
                            <Paddings Padding="0px" />
                        </dxe:ASPxButton>
                        <dxe:ASPxComboBox ID="CmbExcelExport" runat="server" ValueType="System.String" CssClass="btnRight"
                            ClientInstanceName="cCmbExcelExport" Width="100px" Font-Size="8" SelectedIndex="0" TabIndex="0">
                            <Items>
                                <dxe:ListEditItem Text="Export" Value="Ex"></dxe:ListEditItem>
                                <dxe:ListEditItem Text="Excel" Value="1"></dxe:ListEditItem>
                            </Items>
                            <ClientSideEvents SelectedIndexChanged="OnCmbExcelExportChanged" />
                            <Paddings Padding="0px" />
                        </dxe:ASPxComboBox>
                        <dxe:ASPxButton ID="BtnAllGenerateRequest" runat="server" CssClass="btnRight" AutoPostBack="False" ClientInstanceName="cBtnAllGenerateRequest"
                            Text="Generate All" Font-Size="8" TabIndex="0">
                            <ClientSideEvents Click="function(s,e){BtnAllGenerateRequest_Click();}" />
                            <Paddings Padding="0px" />
                        </dxe:ASPxButton>
                        <dxe:ASPxButton ID="BtnGenerateRequest" runat="server" CssClass="btnRight" AutoPostBack="False" ClientInstanceName="cBtnGenerateRequest"
                            Text="Generate" Font-Size="8" TabIndex="0">
                            <ClientSideEvents Click="function(s,e){BtnGenerateRequest_Click();}" />
                            <Paddings Padding="0px" />
                        </dxe:ASPxButton>
                        <dxe:ASPxButton ID="BtnAllDeleteRequest" runat="server" CssClass="btnRight" AutoPostBack="False" ClientInstanceName="cBtnAllDeleteRequest"
                            Text="Delete All" Font-Size="8" TabIndex="0">
                            <ClientSideEvents Click="function(s,e){BtnAllDeleteRequest_Click();}" />
                            <Paddings Padding="0px" />
                        </dxe:ASPxButton>
                        <dxe:ASPxButton ID="BtnDeleteRequest" runat="server" CssClass="btnRight" AutoPostBack="False" ClientInstanceName="cBtnDeleteRequest"
                            Text="Delete" Font-Size="8" TabIndex="0">
                            <ClientSideEvents Click="function(s,e){BtnDeleteRequest_Click();}" />
                            <Paddings Padding="0px" />
                        </dxe:ASPxButton>
                    </div>
                    <div class="left pagingContent" style="vertical-align: bottom">
                        Page <b id="B_PageNo" runat="server"></b>Of <b id="B_TotalPage" runat="server"></b>
                        ( <b id="B_TotalRows" runat="server"></b>items ) 
                            <span class="textLeft">
                                <a id="A_LeftNav" runat="server" href="javascript:void(0);" onclick="OnLeftNav_Click()">
                                    <img src="/assests/images/LeftNav.gif" align="middle" class="paging_nav" alt="" width="16" />
                                </a>
                            </span>
                        <a id="A1" class="number_box" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A1')">1 </a>
                        <a id="A2" class="number_box" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A2')">2 </a>
                        <a id="A3" class="number_box" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A3')">3 </a>
                        <a id="A4" class="number_box" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A4')">4 </a>
                        <a id="A5" class="number_box" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A5')">5 </a>
                        <a id="A6" class="number_box" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A6')">6 </a>
                        <a id="A7" class="number_box" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A7')">7 </a>
                        <a id="A8" class="number_box" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A8')">8 </a>
                        <a id="A9" class="number_box" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A9')">9 </a>
                        <a id="A10" class="number_box" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A10')">10 </a>
                        <span class="textRight">
                            <a id="A_RightNav" runat="server" href="javascript:void(0);" onclick="OnRightNav_Click()">
                                <img src="../images/RightNav.gif" align="middle" class="paging_nav" width="16" alt="" />
                            </a>
                        </span>
                        <%-- <span class="clear"></span>--%>
                    </div>
                    <span class="clear"></span>
                </div>
                <dxe:ASPxGridView ID="GrdEmployeeRequisition" runat="server" AutoGenerateColumns="False" KeyFieldName="ContactID" Width="100%"
                    ClientInstanceName="cGrdEmployeeRequisition" OnCustomCallback="GrdEmployeeRequisition_CustomCallback">
                    <ClientSideEvents SelectionChanged="OnGridSelectionChanged" EndCallback="function(s, e){GrdEmployeeRequisition_EndCallBack();}" />
                    <SettingsBehavior AllowFocusedRow="True" AutoFilterRowInputDelay="1200" ConfirmDelete="True"
                        AllowMultiSelection="True" />
                    <Styles>
                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                        </Header>
                        <LoadingPanel ImageSpacing="10px">
                        </LoadingPanel>
                        <Row Wrap="False">
                        </Row>
                        <FocusedRow BackColor="#FCA977" HorizontalAlign="Left" VerticalAlign="Top">
                        </FocusedRow>
                        <AlternatingRow Enabled="True">
                        </AlternatingRow>
                    </Styles>
                    <Columns>
                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" FixedStyle="Left" VisibleIndex="0" Width="15px">
                            <%-- <HeaderTemplate>
                                <dxe:ASPxCheckBox ID="cbAll" runat="server" ClientInstanceName="cbAll" ToolTip="Select all rows"
                                    Checked="False" BackColor="White" OnInit="cbAll_Init">
                                    <clientsideevents checkedchanged="OnAllCheckedChanged" />
                                </dxe:ASPxCheckBox>                              
                            </HeaderTemplate>--%>
                        </dxe:GridViewCommandColumn>
                        <dxe:GridViewDataTextColumn Caption="Srl." FieldName="SRLNO" FixedStyle="Left"
                            VisibleIndex="1" Width="20px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Name" FieldName="Name" FixedStyle="Left" VisibleIndex="2"
                            Width="150px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="EmpCode" FieldName="EmpCode" FixedStyle="Left"
                            VisibleIndex="3" Width="120px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <%--Start For Employee Log Detail visible Index [4-15]--%>
                        <dxe:GridViewDataTextColumn Caption="Requisition Type" FieldName="EmpLog_Type" FixedStyle="Left"
                            VisibleIndex="4" Width="150px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Export" FieldName="EmpLog_IsExported" VisibleIndex="5" Width="150px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Exported By" FieldName="ExportedBy" VisibleIndex="6" Width="75px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Export Date" FieldName="EmpLog_ExportedDateTime" VisibleIndex="7" Width="75px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="First Authorized By" FieldName="Authorize1By" VisibleIndex="8" Width="150px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="First Authorized Date" FieldName="EmpLog_AuthorizeDateTime1" VisibleIndex="9" Width="150px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Second Authorized By" FieldName="Authorize2By" VisibleIndex="10" Width="150px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Second Authorized Date" FieldName="EmpLog_AuthorizeDateTime2" VisibleIndex="11" Width="150px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Third Authorized By" FieldName="Authorize3By" VisibleIndex="12" Width="150px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Third Authorized Date" FieldName="EmpLog_AuthorizeDateTime3" VisibleIndex="13" Width="150px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Created By" FieldName="CreatedBy" VisibleIndex="14" Width="150px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Create Date" FieldName="EmpLog_CreateDateTime" VisibleIndex="15" Width="150px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <%--End For Employee Log Detail--%>
                        <dxe:GridViewDataTextColumn Caption="Father's Name" FieldName="FatherName" VisibleIndex="16"
                            Width="150px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="DOB" FieldName="DOB" VisibleIndex="17" Width="75px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="DOJ" FieldName="DOJ" VisibleIndex="18" Width="75px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="DOL" FieldName="DOL" VisibleIndex="19" Width="75px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Department" FieldName="Department" VisibleIndex="20"
                            Width="120px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Branch" FieldName="BranchName" VisibleIndex="21"
                            Width="75px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="CTC" FieldName="CTC" VisibleIndex="22" Width="75px">
                            <CellStyle CssClass="gridcellleft" HorizontalAlign="Right" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="ReportTo" FieldName="ReportTo" VisibleIndex="23"
                            Width="150px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Designation" FieldName="Designation" VisibleIndex="24"
                            Width="150px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Company" FieldName="Company" VisibleIndex="25"
                            Width="150px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="EmpType" FieldName="EmpType" VisibleIndex="26"
                            Width="120px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="EmailId(s)" FieldName="Email_Ids" VisibleIndex="27"
                            Width="150px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Phone/Mobile" FieldName="PhoneMobile_Numbers"
                            VisibleIndex="28" Width="75px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="PanCard" FieldName="PanCardNumber" VisibleIndex="29"
                            Width="75px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Address" FieldName="Address" VisibleIndex="30"
                            Width="200px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Ac[BankName][Branch][AcType]" FieldName="Bank"
                            VisibleIndex="31" Width="200px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Emp CreatedBy" FieldName="CreatedBy" VisibleIndex="32">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="ContactID" FieldName="ContactID" Visible="False"
                            VisibleIndex="33" Width="75px">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewCommandColumn Visible="False" ShowDeleteButton="true">
                        </dxe:GridViewCommandColumn>
                    </Columns>
                    <SettingsEditing EditFormColumnCount="3" Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center"
                        PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="900px" />
                    <SettingsText ConfirmDelete="Are you sure to delete?" PopupEditFormCaption="Add / Modify Employee" />
                    <Settings ShowGroupPanel="True" ShowHorizontalScrollBar="True" ShowVerticalScrollBar="true" ShowStatusBar="Hidden" />
                    <SettingsLoadingPanel Text="Please Wait..." />
                    <SettingsPager Visible="False">
                    </SettingsPager>
                    <StylesPager EnableDefaultAppearance="False">
                    </StylesPager>
                </dxe:ASPxGridView>
                <span class="clear"></span>
            </div>
            <div id="Row2" class="Row">
                <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                <asp:HiddenField ID="hdnSelectedOption" runat="server" />
                <asp:HiddenField ID="HDNEmployee" runat="server" />
                <asp:HiddenField ID="HDNCompany" runat="server" />
                <asp:HiddenField ID="HDNBranch" runat="server" />
                <asp:HiddenField ID="HDNReportTo" runat="server" />
                <asp:HiddenField ID="HDNEmpType" runat="server" />
                <asp:HiddenField ID="HDNSearchBy" runat="server" />
                <asp:HiddenField ID="HDNCheckedEmpID" runat="server" />
                <asp:HiddenField ID="hdn_GridBindOrNotBind" runat="server" />
                <!--Create HiddenFields For filter Option -->
                <asp:HiddenField ID="Hdn_PageSize" runat="server" />
                <asp:HiddenField ID="Hdn_PageNumber" runat="server" />
                <asp:HiddenField ID="Hdn_Emp_ContactID" runat="server" />
                <asp:HiddenField ID="Hdn_DateFrom" runat="server" />
                <asp:HiddenField ID="Hdn_DateTo" runat="server" />
                <asp:HiddenField ID="Hdn_Company" runat="server" />
                <asp:HiddenField ID="Hdn_Branch" runat="server" />
                <asp:HiddenField ID="Hdn_ReportTo" runat="server" />
                <asp:HiddenField ID="Hdn_EmployeeType" runat="server" />
                <asp:HiddenField ID="Hdn_Type" runat="server" />
                <asp:HiddenField ID="Hdn_Exported" runat="server" />
                <asp:HiddenField ID="Hdn_Authorized" runat="server" />
                <asp:HiddenField ID="Hdn_ReportType" runat="server" />
                <!--End Create HiddenFields For filter Option -->
                <asp:Button ID="BtnForExportEvent" runat="server" OnClick="cmbExport_SelectedIndexChanged"
                    BackColor="#DDECFE" BorderStyle="None" />
            </div>
        </div>
    </div>
</asp:Content>
