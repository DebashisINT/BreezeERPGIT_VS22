<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                19-05-2023        2.0.38           Pallab              26177: Add Employee module design modification & check in small device
====================================================== Revision History =============================================--%>

<%@ Page Language="C#" AutoEventWireup="True"
    Inherits="ERP.OMS.Management.Master.management_master_Employee_AddNew" MasterPageFile="~/OMS/MasterPage/ERP.Master" CodeBehind="Employee_AddNew.aspx.cs" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            ListBind();
            ChangeSource();

        });
        FieldName = 'btnSave';
        function CallList(obj1, obj2, obj3) {

            if (obj1.value == "") {
                obj1.value = "%";
            }
            var obj5 = '';
            if (obj5 != '18') {
                //ajax_showOptions(obj1, obj2, obj3, obj5);
                ajax_showOptionsTEST(obj1, obj2, obj3, obj5);
                if (obj1.value == "%") {
                    obj1.value = "";
                }
            }
        }
        function Pageload() {
            //TrGeneral.style.display = "inline";
            //TrJoin.style.display = "none";
            //TrCTC.style.display = "none";
            //TrEmpID.style.display = "none";

            //TrGeneral.style.display = "inline";
            //TrJoin.style.display = "inline";
            //TrCTC.style.display = "inline";
            btnEmpID.style.display = "none";
            btnSave.style.display = "none";
            //btnJoin.style.display = "none";
        }
        function lstReportTo() {

            // $('#lstReferedBy').chosen();
            $('#lstReportTo').fadeIn();
        }
        function setvalue() {
            document.getElementById("txtReportTo_hidden").value = document.getElementById("lstReportTo").value;
        }
        function startLoading() {
            LoadingPanel.Show();
        }
        function stopLoading() {
            LoadingPanel.Hide();
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
        function ChangeSource() {

            var InterId = "";
            var fname = "";
            var sname = "";
            var lReferBy = $('select[id$=lstReportTo]');
            lReferBy.empty();
            $.ajax({
                type: "POST",
                url: "Employee_AddNew.aspx/GetreportTo",
                data: JSON.stringify({ firstname: fname, shortname: sname }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {
                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];
                            $('#lstReportTo').append($('<option>').text(name).val(id));

                            //listItems.push('<option value="' +
                            //id + '">' + id
                            //+ '</option>');
                        }

                        $(lReferBy).append(listItems.join(''));

                        lstReportTo();
                        $('#lstReportTo').trigger("chosen:updated");

                    }
                    else {
                        alert("No records found");
                        lstReportTo();
                        $('#lstReportTo').trigger("chosen:updated");
                    }
                }
            });

        }

        function ForJoin() {
            TrGeneral.style.display = "none";
            //TrJoin.style.display = "inline";
            //TrCTC.style.display = "none";
            // TrEmpID.style.display = "none";

        }
        function ForCTC() {
            TrGeneral.style.display = "none";
            //TrJoin.style.display = "none";
            //TrCTC.style.display = "inline";
            //TrEmpID.style.display = "none";

        }
        function ForEMPID() {
            TrGeneral.style.display = "none";
            //TrJoin.style.display = "none";
            //TrCTC.style.display = "inline";
            TrEmpID.style.display = "inline";
        }

        function ValidateGeneral() {
            //if (document.getElementById("txtFirstNmae").value.trim() == '') {
            //    alert('Employee First Name is Required!..');
            //    return false;
            //}  
            var FirstName = document.getElementById('txtFirstNmae').value;
            if (FirstName.trim().length == 0) {
                $('#MandatoryFirstName').css({ 'display': 'block' });
                return false;
            }

        }
        function ValidateDOJ() {
            //if (cmbDOJ.GetText() == '01-01-0100' || cmbDOJ.GetText() == '01-01-1900' || cmbDOJ.GetText() == '' || cmbDOJ.GetText() == '01010100') {
            //    alert('Joining Date is Required!.');
            //    return false;
            //}

            if (cmbDOJ.GetText() == '01-01-0100' || cmbDOJ.GetText() == '01-01-1900' || cmbDOJ.GetText() == '' || cmbDOJ.GetText() == '01010100') {
                $('#MandatoryDOJ').css({ 'display': 'block' });
                return false;
            }
            //if (JoiningDate.GetText() == '01-01-0100' || JoiningDate.GetText() == '01-01-1900' || JoiningDate.GetText() == '' || JoiningDate.GetText() == '01010100') {
            //    $('#MandatoryDOJ2').css({ 'display': 'block' });
            //    return false;
            //}
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!

            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd
            }
            if (mm < 10) {
                mm = '0' + mm
            }
            var today = dd + '-' + mm + '-' + yyyy;

            if (cmbDOJ.GetText() > today) {
                alert('joining date can not be greater than today date');
                return false;
            }
        }
        function ValidateCTC() {

            if (document.getElementById("cmbOrganization").value == "0") {
                //alert('Please Select  Organization.');
                $('#MandatoryOrganization').css({ 'display': 'block' });
                return false;
            }
            else {
                $('#MandatoryOrganization').css({ 'display': 'none' });
            }
            if (document.getElementById("cmbJobResponse").value == "0") {
                //alert('Please Select Job Responsibility.');
                $('#MandatoryJobResponse').css({ 'display': 'block' });
                return false;
            }
            else {
                $('#MandatoryJobResponse').css({ 'display': 'none' });
            }
            if (document.getElementById("cmbBranch").value == "0") {
                $('#MandatoryBranch').css({ 'display': 'block' });
                return false;
            }
            else {
                $('#MandatoryBranch').css({ 'display': 'none' });
            }
            if (document.getElementById("cmbDesg").value == "0") {
                //alert('Please Select Designation.');
                $('#MandatoryDesignation').css({ 'display': 'block' });
                return false;
            }
            else {
                $('#MandatoryDesignation').css({ 'display': 'none' });
            }
            if (document.getElementById("EmpType").value == "0") {
                //alert('Please Select Employee Type.');
                $('#MandatoryEmpType').css({ 'display': 'block' });
                return false;
            }
            else {
                $('#MandatoryEmpType').css({ 'display': 'none' });
            }
            if (document.getElementById("cmbDept").value == "0") {
                //alert('Please Select Employee Dept..');
                $('#MandatoryDepartment').css({ 'display': 'block' });
                return false;
            }
            else {
                $('#MandatoryDepartment').css({ 'display': 'none' });
            }
            if (document.getElementById("txtReportTo_hidden").value == '') {
                //alert('Please Select Reporting Head.');
                $('#MandatoryReportTo').css({ 'display': 'block' });
                return false;
            }
            else {
                $('#MandatoryReportTo').css({ 'display': 'none' });
            }
            if (document.getElementById("cmbLeaveP").value == "0") {
                //alert('Please Select Leave Policy.');
                $('#MandatoryLeaveP').css({ 'display': 'block' });
                return false;
            }
            else {
                $('#MandatoryLeaveP').css({ 'display': 'none' });
            }
            if (document.getElementById("cmbWorkingHr").value == "0") {
                //alert('Please Select Working Hour.');
                $('#MandatoryWorkinghr').css({ 'display': 'block' });
                return false;
            }
            else {
                $('#MandatoryWorkinghr').css({ 'display': 'none' });
            }
            if (cmbLeaveEff.GetText() == '01-01-0100' || cmbLeaveEff.GetText() == '01-01-1900' || cmbLeaveEff.GetText() == '' || cmbLeaveEff.GetText() == '01010100') {
                //alert('Effective From Date is Required!.');
                $('#MandatoryLeaveEff').css({ 'display': 'block' });
                return false;
            }
            else {
                $('#MandatoryLeaveEff').css({ 'display': 'none' });
            }

            // alert(cmbLeaveEff.GetText() + '--' + cmbDOJ.GetText());



            var today = new Date();
            //var dd = today.getDate();
            //var mm = today.getMonth() + 1; //January is 0!

            //var yyyy = today.getFullYear();
            //if (dd < 10) {
            //    dd = '0' + dd
            //}
            //if (mm < 10) {
            //    mm = '0' + mm
            //}
            // today = dd + '-' + mm + '-' + yyyy;
            var joiningdate = cmbDOJ.GetValue();
            var joininginDate = new Date(joiningdate);
            //---------------------
            //var LeaveDate, JoinDate, CurrDate;
            //JoinDate = Date.parse(cmbDOJ.GetText());
            //LeaveDate = Date.parse(cmbLeaveEff.GetText());
            //CurrDate = Date.parse(cmbLeaveEff.GetText());

            //alert(JoinDate + '-----' + CurrDate);
            //if (cmbLeaveEff.GetText() < cmbDOJ.GetText()) {
            //    jAlert('Leave effective date can not be less than joining date');
            //    return false;
            //}

            if (joininginDate > today) {
                jAlert('Date of Joining should not be greater than current date.');
                return false;
            }

                //if (cmbDOJ.GetText() > today) {
                //    jAlert('Date of Joining should not be greater than current date.');
                //    return false;
                //}       
            else {
                if (cmbLeaveEff.GetText() < cmbDOJ.GetText()) {
                    jAlert('Leave effective date can not be less than joining date');
                    return false;
                }
            }

            //---------------------


            //if (cmbDOJ.GetText() > today) {
            //    alert('joining date can not be greater than today date');
            //    return false;
            //}

            startLoading();
        }
        function ValidateEMPID() {
            if (document.getElementById("txtAliasName").value == '') {
                alert('Employee Code is Required!..');
                return false;
            }
        }

        function Check() {
            //var txtBranch = document.getElementById('txtBranch').value;
            var lstBranches = document.getElementById('lstBranches');
            var Name = document.getElementById('txtName').value;
            var Code = document.getElementById('txtCode').value;
            var tLength = lstBranches.length;
            //var selectedValue = lstBranches.checked;


            if (Name.trim().length == 0) {
                $('#MandatoryName').css({ 'display': 'block' });
                return false;
            }
            else {
                $('#MandatoryName').css({ 'display': 'none' });
            }

            if (Code.trim().length == 0) {
                $('#MandatoryShortname').css({ 'display': 'block' });
                return false;
            }
            else {
                $('#MandatoryShortname').css({ 'display': 'none' });
            }
            //if (tLength > 0) {
            //    $('#MandatoryFileNo').attr('style', 'display:none;color:red; position:absolute;right:736px;top:240px');
            //}
            //else {
            //    $('#MandatoryFileNo').attr('style', 'display:block;color:red; position:absolute;right:736px;top:240px');
            //    return false;
            //}
            var count = 0;
            for (i = 0; i < tLength; i++) {
                if (lstBranches.options[i].selected == true) {
                    count++;
                }

            }
            if (count > 0) {
                $('#MandatoryFileNo').css({ 'display': 'none' });
            }
            else {
                $('#MandatoryFileNo').css({ 'display': 'block' });
                return false;
            }
        }

        function BindContactType(reqId) {
            $.ajax({
                type: "POST",
                url: "Employee_AddNew.aspx/BindContactType",
                data: JSON.stringify({ reqID: reqId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var list = data.d;
                    var listItems = [];
                    if (list.length > 0) {
                        $('#ContactType').append($('<option>').text("Select").val(""));
                        for (var i = 0; i < list.length; i++) {
                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];
                            $('#ContactType').append($('<option>').text(name).val(id));
                        }
                    }
                    else {
                        alert("No records found");
                        $('#ContactType').trigger("chosen:updated");
                    }
                }
            });
        }


    </script>
    <style>
        .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }

        #lstReportTo {
            width: 200px;
        }

        #lstReportTo {
            display: none !important;
        }

        .dxtcLite_PlasticBlue > .dxtc-content {
            overflow: visible !important;
        }
        /*#lstReportTo_chosen{
            width:39% !important;
        }*/
    </style>
    <%--  <link href="../../css/choosen.min.css" rel="stylesheet" />--%>
    
    <style>
        /*Rev 1.0*/
        .outer-div-main {
            background: #ffffff;
            padding: 10px;
            border-radius: 10px;
            box-shadow: 1px 1px 10px #11111154;
        }

        /*.form_main {
            overflow: hidden;
        }*/

        label , .mylabel1, .clsTo, .dxeBase_PlasticBlue
        {
            color: #141414 !important;
            font-size: 14px !important;
                font-weight: 500 !important;
                margin-bottom: 0 !important;
                    line-height: 20px;
        }

        #GrpSelLbl .dxeBase_PlasticBlue
        {
                line-height: 20px !important;
        }

        select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
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

        .calendar-icon {
            position: absolute;
            bottom: 6px;
            right: 20px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #cmbDOJ , #cmbLeaveEff
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1 , #cmbDOJ_B-1 , #cmbLeaveEff_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img ,
        #cmbDOJ_B-1 #cmbDOJ_B-1Img , #cmbLeaveEff_B-1 cmbLeaveEff_B-1Img
        {
            display: none;
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
            line-height: 18px;
            z-index: 0;
        }
        .simple-select {
            position: relative;
        }
        .simple-select:disabled::after
        {
            background: #1111113b;
        }
        /*select.btn
        {
            padding-right: 10px !important;
        }*/

        .panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }

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

        .styled-checkbox {
        position: absolute;
        opacity: 0;
        z-index: 1;
    }

        .styled-checkbox + label {
            position: relative;
            /*cursor: pointer;*/
            padding: 0;
            margin-bottom: 0 !important;
        }

            .styled-checkbox + label:before {
                content: "";
                margin-right: 6px;
                display: inline-block;
                vertical-align: text-top;
                width: 16px;
                height: 16px;
                /*background: #d7d7d7;*/
                margin-top: 2px;
                border-radius: 2px;
                border: 1px solid #c5c5c5;
            }

        .styled-checkbox:hover + label:before {
            background: #094e8c;
        }


        .styled-checkbox:checked + label:before {
            background: #094e8c;
        }

        .styled-checkbox:disabled + label {
            color: #b8b8b8;
            cursor: auto;
        }

            .styled-checkbox:disabled + label:before {
                box-shadow: none;
                background: #ddd;
            }

        .styled-checkbox:checked + label:after {
            content: "";
            position: absolute;
            left: 3px;
            top: 9px;
            background: white;
            width: 2px;
            height: 2px;
            box-shadow: 2px 0 0 white, 4px 0 0 white, 4px -2px 0 white, 4px -4px 0 white, 4px -6px 0 white, 4px -8px 0 white;
            transform: rotate(45deg);
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

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #EmployeeGrid , .TableMain100 #RootGrid
        {
            max-width: 98% !important;
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
                margin-top: 3px;
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

        .for-cust-icon {
            position: relative;
            z-index: 1;
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

        .mt-24
        {
            margin-top: 24px;
        }

        .col-md-3
        {
            margin-top: 8px;
        }

        #upldBigLogo , #upldSmallLogo
        {
            width: 100%;
        }

        .chosen-container-single .chosen-single span
        {
            min-height: 30px;
            line-height: 30px;
        }

        .chosen-container-single .chosen-single div {
        background: #094e8c;
        color: #fff;
        border-radius: 4px;
        height: 26px;
        top: 1px;
        right: 1px;
        /*position:relative;*/
    }

        .chosen-container-single .chosen-single div b {
            display: none;
        }

        .chosen-container-single .chosen-single div::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 2px;
            right: 5px;
            font-size: 17px;
            transform: rotate(269deg);
            font-weight: 500;
        }

    .chosen-container-active.chosen-with-drop .chosen-single div {
        background: #094e8c;
        color: #fff;
    }

        .chosen-container-active.chosen-with-drop .chosen-single div::after {
            transform: rotate(90deg);
            right: 5px;
        }

        .panel-title
        {
            padding-bottom: 20px;
        }

        .chosen-container-single .chosen-single
        {
            z-index: 1;
        }

        /*.dxeDisabled_PlasticBlue, .aspNetDisabled {
            opacity: 0.4 !important;
            color: #ffffff !important;
        }*/
                /*.padTopbutton {
            padding-top: 27px;
        }*/
        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%--   <script src="/assests/pluggins/choosen/choosen.min.js"></script>--%>

    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            //$(".chzn-select").chosen();
            //$(".chzn-select-deselect").chosen({ allow_single_deselect: true });
        });
    </script>
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Add Employee</h3>
            <div class="crossBtn"><a href="Employee.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
        <div class="form_main" style="border: 1px solid #ccc; padding: 5px 15px;">
        <table class="TableMain100">
            <tr id="TrGeneral" runat="server">
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="">
                                <%--Rev 1.0 : "simple-select" class add --%>
                                <div class="col-md-3 simple-select">
                                    <label>Salutation</label>
                                    <div>
                                        <asp:DropDownList ID="CmbSalutation" runat="server" Width="100%">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <label>First Name<span style="color: red">*</span></label>
                                    <div style="position: relative">
                                        <asp:TextBox ID="txtFirstNmae" runat="server" Width="100%" MaxLength="20"></asp:TextBox>
                                        <span id="MandatoryFirstName" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; right: -4px; top: 10px; display: none" title="Mandatory"></span>
                                        <%--  <dxe:ASPxTextBox ID="txtFirstNmae" runat="server" Width="225px" TabIndex="2">
                                                 </dxe:ASPxTextBox>--%>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <label>Middle Name</label>
                                    <div>
                                        <asp:TextBox ID="txtMiddleName" runat="server" Width="100%" MaxLength="20">
                                        </asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <label>Last Name</label>
                                    <div>
                                        <asp:TextBox ID="txtLastName" runat="server" Width="100%" MaxLength="20">
                                        </asp:TextBox>
                                    </div>
                                </div>
                                <%--Rev 1.0 : "simple-select" class add --%>
                                <div class="col-md-3 simple-select">
                                    <label>Gender</label>
                                    <div>
                                        <asp:DropDownList ID="cmbGender" runat="server" Width="100%">
                                            <asp:ListItem Value="1">Male</asp:ListItem>
                                            <asp:ListItem Value="0">Female</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-3" style="display: none">
                                    <div style="padding-left: 62px">
                                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary btnUpdate mTop5" Text="Click to Continue" OnClick="btnSave_Click" />
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <label>Date Of Joining<span style="color: red">*</span></label>
                                    <div style="position: relative">
                                        <%--         <dxe:ASPxDateEdit Width="250px" ID="cmbDOJ" TabIndex="7" runat="server" EditFormat="Custom" UseMaskBehavior="True">
                                            <dropdownbutton>
                                            </dropdownbutton>
                                        </dxe:ASPxDateEdit>--%>

                                        <dxe:ASPxDateEdit Width="100%" ID="cmbDOJ" runat="server">
                                        </dxe:ASPxDateEdit>
                                        <span id="MandatoryDOJ" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; right: -14px; top: 10px; display: none" title="Mandatory"></span>
                                    </div>
                                    <%--Rev 1.0--%>
                                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                    <%--Rev end 1.0--%>
                                </div>
                                <div class="col-md-3" style="display: none">
                                    <div>
                                        <asp:Button ID="btnJoin" Visible="false" CssClass="btn btn-primary btnUpdate" Text="Click to Continue" runat="server" OnClick="btnJoin_Click" />
                                    </div>
                                </div>
                                <%--Rev 1.0 : "simple-select" class add --%>
                                <div class="col-md-3 simple-select">
                                    <label>Organization<span style="color: red">*</span></label>
                                    <div style="position: relative">
                                        <asp:DropDownList ID="cmbOrganization" runat="server" Width="100%">
                                        </asp:DropDownList>
                                        <span id="MandatoryOrganization" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; right: -18px; top: 10px; display: none" title="Mandatory"></span>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div>

                                        <dxe:ASPxDateEdit ID="JoiningDate" ClientVisible="false" runat="server" DateOnError="Today" EditFormat="Custom"
                                            Width="100%">
                                        </dxe:ASPxDateEdit>

                                        <span id="MandatoryDOJ2" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; right: -14px; top: 10px; display: none" title="Mandatory"></span>
                                    </div>
                                </div>
                                <%--Rev 1.0 : "simple-select" class add --%>
                                <div class="col-md-3 simple-select">
                                    <label>Job Responsibility<span style="color: red">*</span> </label>
                                    <div style="position: relative">
                                        <asp:DropDownList ID="cmbJobResponse" runat="server" Width="100%">
                                        </asp:DropDownList>
                                        <span id="MandatoryJobResponse" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; right: -18px; top: 10px; display: none" title="Mandatory"></span>
                                    </div>
                                </div>
                                <%--Rev 1.0 : "simple-select" class add --%>
                                <div class="col-md-3 simple-select">
                                    <label>Branch<span style="color: red">*</span></label>
                                    <div style="position: relative">
                                        <asp:DropDownList ID="cmbBranch" runat="server" Width="100%">
                                        </asp:DropDownList>
                                        <span id="MandatoryBranch" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; right: -18px; top: 10px; display: none" title="Mandatory"></span>
                                    </div>
                                </div>
                                <%--Rev 1.0 : "simple-select" class add --%>
                                <div class="col-md-3 simple-select">
                                    <label>Designation<span style="color: red">*</span></label>
                                    <div style="position: relative">
                                        <asp:DropDownList ID="cmbDesg" runat="server" Width="100%">
                                        </asp:DropDownList>
                                        <span id="MandatoryDesignation" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; right: -18px; top: 10px; display: none" title="Mandatory"></span>
                                    </div>
                                </div>
                                <%--Rev 1.0 : "simple-select" class add --%>
                                <div class="col-md-3 simple-select">
                                    <label>Employee Type<span style="color: red">*</span></label>
                                    <div style="position: relative">
                                        <asp:DropDownList ID="EmpType" runat="server" Width="100%">
                                            <asp:ListItem Value="" Text="--Select--"></asp:ListItem>
                                        </asp:DropDownList>
                                        <span id="MandatoryEmpType" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; right: -18px; top: 10px; display: none" title="Mandatory"></span>
                                        <script type="text/javascript">
                                            $('#EmpType').change(function () {
                                                $('#ContactType').html(String(''));
                                                var empType = $('#EmpType').val();
                                                switch (empType) {
                                                    case "19":
                                                        $('#contactType').css('display', 'block');
                                                        BindContactType('DV');
                                                        $('#lblContactType').html('Vendor');
                                                        break;
                                                    default:
                                                        $('#contactType').css('display', 'none');
                                                }

                                            });
                                        </script>
                                    </div>
                                </div>
                                <div class="col-md-3" id="contactType" style="display: none">
                                    <label id="lblContactType">
                                    </label>
                                    <div style="position: relative">
                                        <asp:DropDownList ID="ContactType" runat="server" Width="100%">
                                        </asp:DropDownList>
                                        <%--<span id="MandatoryDepartment" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; right: -18px; top: 10px; display: none" title="Mandatory"></span>--%>
                                    </div>
                                </div>

                                <%--Rev 1.0 : "simple-select" class add --%>
                                <div class="col-md-3 simple-select">
                                    <label>
                                        Department<span style="color: red">*</span>
                                    </label>
                                    <div style="position: relative">
                                        <asp:DropDownList ID="cmbDept" runat="server" Width="100%">
                                        </asp:DropDownList>
                                        <span id="MandatoryDepartment" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; right: -18px; top: 10px; display: none" title="Mandatory"></span>
                                    </div>
                                </div>
                                <%--Rev 1.0 : "simple-select" class add --%>
                                <div class="col-md-3 simple-select">
                                    <label>Report To<span style="color: red">*</span></label>
                                    <div style="position: relative">
                                        <asp:DropDownList data-placeholder="Select or type here" Visible="false" runat="server" ID="ddlReportTo" class="chzn-select" Style="width: 255px;">
                                        </asp:DropDownList>
                                        <asp:ListBox ID="lstReportTo" CssClass="chsn" runat="server" Width="250px" data-placeholder="Select..."></asp:ListBox>
                                        <%--   <asp:TextBox ID="txtReportTo" runat="server" Width="225px" Visible="true" TabIndex="17"></asp:TextBox>--%>
                                        <span id="MandatoryReportTo" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; right: -18px; top: 5px; display: none" title="Mandatory"></span>
                                        <asp:HiddenField ID="txtReportTo_hidden" runat="server" />
                                    </div>
                                </div>
                                <%--Rev 1.0 : "simple-select" class add --%>
                                <div class="col-md-3 simple-select">
                                    <label>Working Hour<span style="color: red">*</span></label>
                                    <div style="position: relative">
                                        <asp:DropDownList ID="cmbWorkingHr" runat="server" Width="100%">
                                        </asp:DropDownList>
                                        <span id="MandatoryWorkinghr" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; right: -18px; top: 8px; display: none" title="Mandatory"></span>
                                    </div>
                                </div>
                                <%--Rev 1.0 : "simple-select" class add --%>
                                <div class="col-md-3 simple-select">
                                    <label>Leave Policy<span style="color: red">*</span></label>
                                    <div style="position: relative">
                                        <asp:DropDownList ID="cmbLeaveP" runat="server" Width="100%">
                                        </asp:DropDownList>
                                        <span id="MandatoryLeaveP" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; right: -18px; top: 8px; display: none" title="Mandatory"></span>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <label>Leave Effective From<span style="color: red">*</span> </label>
                                    <div style="position: relative">
                                        <dxe:ASPxDateEdit ID="cmbLeaveEff" runat="server" DateOnError="Today" EditFormat="Custom"
                                            Width="100%">
                                        </dxe:ASPxDateEdit>
                                        <span id="MandatoryLeaveEff" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; right: -18px; top: 8px; display: none" title="Mandatory"></span>
                                    </div>
                                    <%--Rev 1.0--%>
                                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                    <%--Rev end 1.0--%>
                                </div>
                                <div style="clear: both"></div>

                                <div class="col-md-3">
                                    <label>Use Hierarchy?</label>
                                    <div style="position: relative">
                                        <dxe:ASPxCheckBox ID="chkUsehierchy" runat="server" ClientInstanceName="cchkUsehierchy">
                                        </dxe:ASPxCheckBox>

                                    </div>
                                </div>

                                <div class="col-md-3 visF lblmTop8" id="divTDSDeductee" runat="server">
                                    <label class="labelt">
                                        <dxe:ASPxLabel ID="ASPxLabel36" runat="server" Text="Tds Deductee Type">
                                        </dxe:ASPxLabel>
                                    </label>
                                    <div class="visF">
                                        <dxe:ASPxComboBox ID="cmbTDS" ClientInstanceName="ccmbTDS" runat="server" Width="200px">
                                        </dxe:ASPxComboBox>
                                    </div>

                                    <%--Code Added By Sam on 09022018 for Mantis Issue 0015725 Section End--%>
                                </div>
                                <div class="col-md-3" id="DivIsDirector" runat="server">
                                    <label>Is Director</label>
                                    <div style="position: relative">
                                        <dxe:ASPxCheckBox ID="chkIsDirector" runat="server" ClientInstanceName="cchkIsDirector">
                                        </dxe:ASPxCheckBox>
                                    </div>
                                </div>
                                <div style="clear: both"></div>
                                <div class="col-md-12" style="padding-top: 15px;">
                                    <asp:Button ID="btnCTC" CssClass="btn btn-primary btnUpdate" Text="Save & Proceed" runat="server" OnClientClick="setvalue()" OnClick="btnCTC_Click" />
                                </div>
                            </div>

                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <%-- <tr id="TrJoin" runat="server">
                <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>--%>

            <%-- </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnJoin" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>--%>
            <tr id="TrCTC" runat="server">
                <td>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <table class="TableMain100">

                                <tr>

                                    <td></td>
                                    <td></td>
                                </tr>

                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCTC" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr id="TrEmpID" runat="server" style="display: none">
                <td>
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <table class="TableMain100">

                                <tr>
                                    <td style="width: 100px">ID Generated<span style="color: red">*</span>
                                    </td>
                                    <td>
                                        <dxe:ASPxTextBox ID="txtAliasName" runat="server" Width="225px" TabIndex="22">
                                        </dxe:ASPxTextBox>
                                        <!--<asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" Style="color: #cc3300; text-decoration: underline; font-size: 8pt;">Make System Employee Code</asp:LinkButton>-->

                                        <asp:Label ID="lblErr" Text="" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="padding: 5px 0px 0px 100px">
                                        <asp:Button ID="btnEmpID" CssClass="btn btn-primary btnUpdate" Text="Update & Continue" runat="server" TabIndex="23" OnClick="btnEmpID_Click" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnEmpID" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="btnCTC"
            Modal="True">
        </dxe:ASPxLoadingPanel>
    </div>
    </div>
    <style>
        a img {
            border: none;
        }

        ol li {
            list-style: decimal outside;
        }

        div#container {
            width: 780px;
            margin: 0 auto;
            padding: 1em 0;
        }

        div.side-by-side {
            width: 100%;
            margin-bottom: 1em;
        }

            div.side-by-side > div {
                float: left;
                width: 50%;
            }

                div.side-by-side > div > em {
                    margin-bottom: 10px;
                    display: block;
                }

        .clearfix:after {
            content: "\0020";
            display: block;
            height: 0;
            clear: both;
            overflow: hidden;
            visibility: hidden;
        }

        .chosen-container-active.chosen-with-drop .chosen-single div,
        .chosen-container-single .chosen-single div {
            display: none !important;
        }

        .chosen-container-single .chosen-single {
            border-radius: 0 !important;
            background: transparent !important;
        }
    </style>

</asp:Content>
