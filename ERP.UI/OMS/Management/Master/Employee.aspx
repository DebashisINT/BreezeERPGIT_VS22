<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                17-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="Employee" Language="C#" AutoEventWireup="True" Inherits="ERP.OMS.Management.Master.management_master_Employee" CodeBehind="Employee.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        function Pageload() {

            document.getElementById('td1').style.display = "inline";
            document.getElementById('td1').style.display = "tablee-cell";
            document.getElementById('td2').style.display = "inline";
            document.getElementById('td2').style.display = "table-cell";
            document.getElementById('td3').style.display = "inline";
            document.getElementById('td3').style.display = "table-cell";
            document.getElementById('td4').style.display = "inline";
            document.getElementById('td4').style.display = "table-cell";
            HideTrTd("Tr_EmployeeName");
            HideTrTd("Tr_EmployeeCode");

            cGrdEmployee.PerformCallback("Show~~~");
        }
        function fn_DeleteEmp(keyValue) {
            //var result=confirm('Confirm delete?');
            //if(result)
            //{
            //    grid.PerformCallback('Delete~' + keyValue);
            //}

            if (keyValue != "EMA0000001") {
                jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        cGrdEmployee.PerformCallback('Delete~' + keyValue);
                    }
                    else {
                        return false;
                    }
                });
            } else {
                jAlert("Sorry, you can not delete the Admin.");
            }


        }

        function ShowTrTd(obj) {
            document.getElementById(obj).style.display = 'inline';
        }
        function HideTrTd(obj) {
            //document.getElementById(obj).style.display = 'none';
        }

        function OnMoreInfoClick(keyValue) {

            if (keyValue != '') {
                var url = 'employee_general.aspx?id=' + keyValue;
                //parent.OnMoreInfoClick(url, "Modify Employee Details", '980px', '500px', "Y");
                window.location.href = url;
            }
        }
        function OnAddButtonClick() {
            var url = 'Employee_AddNew.aspx?id=' + 'ADD';
            //parent.OnMoreInfoClick(url,"Add Employee Details",'980px','400px',"Y");
            window.location.href = url;
        }
        function OnAddBusinessClick(keyValue, CompName) {
            var url = 'AssignIndustry.aspx?id1=' + keyValue + '&EntType=Employee';
            window.location.href = url;
        }
        //Rev Work Start 22.04.2022 MantiseID:0024850: Copy feature add in Employee master
        function OnCopyClick(keyValue) {

            if (keyValue != '') {
                var url = 'Employee_AddNew.aspx?key=Copy&id=' + keyValue;
                window.location.href = url;
            }
        }
        //Rev Work Close 22.04.2022 MantiseID:0024850: Copy feature add in Employee master
        function OnLeftNav_Click() {
            var i = document.getElementById("A1").innerText;
            document.getElementById("hdn_GridBindOrNotBind").value = "False"; //To Stop Bind On Page Load
            if (parseInt(i) > 1) {
                if (crbDOJ_Specific_All.GetValue() == "S")
                    cGrdEmployee.PerformCallback("SearchByNavigation~" + cDtFrom.GetValue() + '~' + cDtTo.GetValue() + "~" + document.getElementById("A1").innerText + "~LeftNav");
                else
                    cGrdEmployee.PerformCallback("SearchByNavigation~~~" + document.getElementById("A1").innerText + "~LeftNav");
            }
            else {
                alert('No More Pages.');
            }
        }
        function OnRightNav_Click() {
            var TestEnd = document.getElementById("A10").innerText;
            document.getElementById("hdn_GridBindOrNotBind").value = "False"; //To Stop Bind On Page Load
            var TotalPage = document.getElementById("B_TotalPage").innerText;
            if (TestEnd == "" || TestEnd == TotalPage) {
                alert('No More Records.');
                return;
            }
            var i = document.getElementById("A1").innerText;
            if (parseInt(i) < TotalPage) {
                if (crbDOJ_Specific_All.GetValue() == "S")
                    cGrdEmployee.PerformCallback("SearchByNavigation~" + cDtFrom.GetValue() + '~' + cDtTo.GetValue() + "~" + document.getElementById("A1").innerText + "~RightNav");
                else
                    cGrdEmployee.PerformCallback("SearchByNavigation~~~" + document.getElementById("A1").innerText + "~RightNav");
            }
            else {
                alert('You are at the End');
            }
        }
        function OnPageNo_Click(obj) {
            var i = document.getElementById(obj).innerText;
            document.getElementById("hdn_GridBindOrNotBind").value = "False"; //To Stop Bind On Page Load
            if (crbDOJ_Specific_All.GetValue() == "S")
                cGrdEmployee.PerformCallback("SearchByNavigation~" + cDtFrom.GetValue() + '~' + cDtTo.GetValue() + "~" + i + "~PageNav");
            else
                cGrdEmployee.PerformCallback("SearchByNavigation~~~" + i + "~PageNav");

        }
        function BtnShow_Click() {
            document.getElementById("hdn_GridBindOrNotBind").value = "False"; //To Stop Bind On Page Load
            if (crbDOJ_Specific_All.GetValue() == "S") {

                cGrdEmployee.PerformCallback("Show~" + cDtFrom.GetValue() + '~' + cDtTo.GetValue());
            }
            else {

                cGrdEmployee.PerformCallback("Show~~~");
            }
        }
        function GridEmployee_EndCallBack() {

            if (cGrdEmployee.cpDelete != null && cGrdEmployee.cpDelete != "") {
                if (cGrdEmployee.cpDelete = "Existinpayroll") {
                    jAlert('This employee is attached to a Pay Structure. If you still wish to Delete this employee, Please remove its Pay Structure Attachment and then Proceed.');
                }
            }
        }

        function GrdEmployee_EndCallBack() {
            if (cGrdEmployee.cpExcelExport != undefined) {
                document.getElementById('BtnForExportEvent').click();
            }
            if (cGrdEmployee.cpRefreshNavPanel != undefined) {
                document.getElementById("B_PageNo").innerText = '';
                document.getElementById("B_TotalPage").innerText = '';
                document.getElementById("B_TotalRows").innerText = '';

                var NavDirection = cGrdEmployee.cpRefreshNavPanel.split('~')[0];
                var PageNum = cGrdEmployee.cpRefreshNavPanel.split('~')[1];
                var TotalPage = cGrdEmployee.cpRefreshNavPanel.split('~')[2];
                var TotalRows = cGrdEmployee.cpRefreshNavPanel.split('~')[3];

                if (NavDirection == "RightNav") {
                    PageNum = parseInt(PageNum) + 10;
                    document.getElementById("B_PageNo").innerText = PageNum;
                    document.getElementById("B_TotalPage").innerText = TotalPage;
                    document.getElementById("B_TotalRows").innerText = TotalRows;
                    var n = parseInt(TotalPage) - parseInt(PageNum) > 10 ? parseInt(11) : parseInt(TotalPage) - parseInt(PageNum) + 2;
                    for (r = 1; r < n; r++) {
                        var obj = "A" + r;
                        document.getElementById(obj).innerText = PageNum++;
                    }
                    for (r = n; r < 11; r++) {
                        var obj = "A" + r;
                        document.getElementById(obj).innerText = "";
                    }
                }
                if (NavDirection == "LeftNav") {
                    if (parseInt(PageNum) > 1) {
                        PageNum = parseInt(PageNum) - 10;
                        document.getElementById("B_PageNo").innerText = PageNum;
                        document.getElementById("B_TotalPage").innerText = TotalPage;
                        document.getElementById("B_TotalRows").innerText = TotalRows;
                        for (l = 1; l < 11; l++) {
                            var obj = "A" + l;
                            document.getElementById(obj).innerText = PageNum++;
                        }
                    }
                    else {
                        alert('No More Pages.');
                    }
                }
                if (NavDirection == "PageNav") {
                    document.getElementById("B_PageNo").innerText = PageNum;
                    document.getElementById("B_TotalPage").innerText = TotalPage;
                    document.getElementById("B_TotalRows").innerText = TotalRows;
                }
                if (NavDirection == "ShowBtnClick") {
                    document.getElementById("B_PageNo").innerText = PageNum;
                    document.getElementById("B_TotalPage").innerText = TotalPage;
                    document.getElementById("B_TotalRows").innerText = TotalRows;
                    var n = parseInt(TotalPage) - parseInt(PageNum) > 10 ? parseInt(11) : parseInt(TotalPage) - parseInt(PageNum) + 2;

                    for (r = 1; r < n; r++) {
                        var obj = "A" + r;
                        document.getElementById(obj).innerText = PageNum++;
                    }

                    for (r = n; r < 11; r++) {
                        var obj = "A" + r;
                        document.getElementById(obj).innerText = "";
                    }

                }
            }
            if (cGrdEmployee.cpCallOtherWhichCallCondition != undefined) {
                if (cGrdEmployee.cpCallOtherWhichCallCondition == "Show") {
                    if (crbDOJ_Specific_All.GetValue() == "S")
                        cGrdEmployee.PerformCallback("Show~" + cDtFrom.GetValue() + '~' + cDtTo.GetValue());
                    else
                        cGrdEmployee.PerformCallback("Show~~~");
                }
            }
            //Now Reset GridBindOrNotBind to True for Next Page Load
            document.getElementById("hdn_GridBindOrNotBind").value = "True";
            //height();

            if (cGrdEmployee.cpDelete != null) {
                if (cGrdEmployee.cpDelete == 'Success')
                    jAlert('Record deleted successfully');
                else
                    jAlert('Error on deletio/n Please Try again!!')
            }
        }
        function selecttion() {
            var combo = document.getElementById('cmbExport');
            combo.value = 'Ex';
        }

        function OnContactInfoClick(keyValue, CompName) {
            var url = 'insurance_contactPerson.aspx?id=' + keyValue;
            // OnMoreInfoClick(url, "Employee Name : " + CompName + "", '980px', '550px', "Y");
            window.location.href = url;
        }
        function Callheight() {
            //height();
        }

        function ShowEmployeeFilterForm(obj) {
            if (obj == 'A') {
                document.getElementById('td1').style.display = "none";
                document.getElementById('td2').style.display = "none";
                document.getElementById('td3').style.display = "none";
                document.getElementById('td4').style.display = "none";
            }
            if (obj == 'S') {

                document.getElementById('td1').style.display = "inline";
                document.getElementById('td1').style.display = "table-cell";

                document.getElementById('td2').style.display = "inline";
                document.getElementById('td2').style.display = "table-cell";

                document.getElementById('td3').style.display = "inline";
                document.getElementById('td3').style.display = "table-cell";

                document.getElementById('td4').style.display = "inline";
                document.getElementById('td4').style.display = "table-cell";
            }

        }
        function ShowFindOption() {
            if (cRb_SearchBy.GetValue() == "N") {
                HideTrTd("Tr_EmployeeName")
                HideTrTd("Tr_EmployeeCode")
            }
            else if (cRb_SearchBy.GetValue() == "EN") {
                ShowTrTd("Tr_EmployeeName")
                HideTrTd("Tr_EmployeeCode")
            }
            else if (cRb_SearchBy.GetValue() == "EC") {
                HideTrTd("Tr_EmployeeName")
                ShowTrTd("Tr_EmployeeCode")
            }
        }
      <%--  function ddlExport_OnChange() {
            var ddlExport = document.getElementById("<%=ddlExport.ClientID%>");
            if (crbDOJ_Specific_All.GetValue() == "S")
                cGrdEmployee.PerformCallback("ExcelExport~" + cDtFrom.GetValue() + '~' + cDtTo.GetValue());
            else
                cGrdEmployee.PerformCallback("ExcelExport~~~");
            ddlExport.options[0].selected = true;
        }--%>

        function ShowHideFilter(obj) {
            document.getElementById("hdn_GridBindOrNotBind").value = "False"; //To Stop Bind On Page Load
            cGrdEmployee.PerformCallback("ShowHideFilter~" + cDtFrom.GetValue() + '~' + cDtTo.GetValue() + '~' + obj);
        }

        function ViewLogData() {
            cGvJvSearch.Refresh();
        }

        function ShowLogData(haslog) {
            debugger;
            $('#btnViewLog').click();
            // cGvJvSearch.Refresh();
        }

        $(document).ready(function () {
            $('#modalSS').on('hidden.bs.modal', function () {
                cGrdEmployee.Refresh();
            })
        })

        function ImportUpdatePopOpenEmployeesTarget(e) {

            $("#modalimport").modal('show');
        }
        function gridRowclick(s, e) {
            $('#GrdEmployee').find('tr').removeClass('rowActive');
            $('.floatedBtnArea').removeClass('insideGrid');
            //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
            $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).addClass('rowActive');
            setTimeout(function () {
                //alert('delay');
                var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
                //    setTimeout(function () {
                //        $(this).fadeIn();
                //    }, 100);
                //});    
                $.each(lists, function (index, value) {
                    //console.log(index);
                    //console.log(value);
                    setTimeout(function () {
                        console.log(value);
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }



    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            setTimeout(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cGrdEmployee.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cGrdEmployee.SetWidth(cntWidth);
                }
            }, 1000)

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cGrdEmployee.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cGrdEmployee.SetWidth(cntWidth);
                }

            });
        });
    </script>

    <script>
        var Branchlist = []
        function BranchPushPop() {
            var empID = $("#hdnEMPID").val();
            let a = [];

            $(".statecheckall:checked").each(function () {
                a.push(this.value);
            });

            $(".statecheck:checked").each(function () {
                a.push(this.value);
            });
            var str1
            //  alert(a);

            str1 = { EMPID: empID, Branchlist: a }
            $.ajax({
                type: "POST",
                url: "Employee.aspx/GetBranchListSubmit",
                data: JSON.stringify(str1),
                contentType: "application/json; charset=utf-8",
                datatype: "json",
                success: function (responseFromServer) {
                    // alert(responseFromServer.d)
                    $("#myModal").modal('hide');
                    jAlert('Branch assigned successfully');
                }
            });
        }

        function CheckParticular(v) {
            if (v == false) {
                $(".statecheckall").prop('checked', false);
            }
        }

        function CheckAll(id) {
            var ischecked = $(".statecheckall").is(':checked');
            if (ischecked == true) {
                $('input:checkbox.statecheck').each(function () {
                    $(this).prop('checked', true);
                });

            }
            else {
                $('input:checkbox.statecheck').each(function () {
                    $(this).prop('checked', false);
                });

            }


        }

        function fn_BranchMap(empID) {
            $("#hdnEMPID").val(empID);
            var str
            str = { EMPID: empID }
            var html = "";
            // alert();
            $.ajax({
                type: "POST",
                url: "Employee.aspx/GetBranchList",
                data: JSON.stringify(str),
                contentType: "application/json; charset=utf-8",
                datatype: "json",
                success: function (responseFromServer) {
                    for (i = 0; i < responseFromServer.d.length; i++) {
                        if (responseFromServer.d[i].IsChecked == true) {
                            html += "<li><input type='checkbox' id=" + responseFromServer.d[i].branch_id + "  class='statecheck' onclick=CheckParticular($(this).is(':checked')) value=" + responseFromServer.d[i].branch_id + " checked  /><a href='#'><label id='lblstatename' class='lblstate' for=" + responseFromServer.d[i].branch_id + " >" + responseFromServer.d[i].branch_description + "</label></a></li>";
                        }
                        else {
                            html += "<li><input type='checkbox' id=" + responseFromServer.d[i].branch_id + " class='statecheck'  onclick=CheckParticular($(this).is(':checked')) value=" + responseFromServer.d[i].branch_id + "   /><a href='#'><label id='lblstatename' class='lblstate' for=" + responseFromServer.d[i].branch_id + ">" + responseFromServer.d[i].branch_description + "</label></a></li>";
                        }
                    }
                    $("#divModalBody").html(html);
                    $("#myModal").modal('show');
                }
            });
        }
    </script>

    <style>
        .VerySmall {
            width: 320px;
        }
    </style>

    <style>
        .listStyle > li {
            list-style-type: none;
            padding: 5px;
        }

        .listStyle {
            height: 450px;
            overflow-y: auto;
        }

            .listStyle > li > input[type="checkbox"] {
                -webkit-transform: translateY(3px);
                -moz-transform: translateY(3px);
                transform: translateY(3px);
            }

        #divModalBody li a:hover:not(.header) {
            background-color: none;
        }

        .modal-backdrop {
            z-index: auto !important;
        }

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

        /*select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }*/

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

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #txtDOB , #txtAnniversary , #txtcstVdate , #txtLocalVdate ,
        #txtCINVdate , #txtincorporateDate , #txtErpValidFrom , #txtErpValidUpto , #txtESICValidFrom , #txtESICValidUpto
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1 , #txtDOB_B-1 , #txtAnniversary_B-1 , #txtcstVdate_B-1 ,
        #txtLocalVdate_B-1 , #txtCINVdate_B-1 , #txtincorporateDate_B-1 , #txtErpValidFrom_B-1 , #txtErpValidUpto_B-1 , #txtESICValidFrom_B-1 ,
        #txtESICValidUpto_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img ,
        #txtDOB_B-1 #txtDOB_B-1Img ,
        #txtAnniversary_B-1 #txtAnniversary_B-1Img ,
        #txtcstVdate_B-1 #txtcstVdate_B-1Img ,
        #txtLocalVdate_B-1 #txtLocalVdate_B-1Img , #txtCINVdate_B-1 #txtCINVdate_B-1Img , #txtincorporateDate_B-1 #txtincorporateDate_B-1Img ,
        #txtErpValidFrom_B-1 #txtErpValidFrom_B-1Img , #txtErpValidUpto_B-1 #txtErpValidUpto_B-1Img , #txtESICValidFrom_B-1 #txtESICValidFrom_B-1Img ,
        #txtESICValidUpto_B-1 #txtESICValidUpto_B-1Img
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

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #EmployeeGrid , .TableMain100 #GrdEmployee
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
            font-size: 18px;
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
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Employees</h3>
        </div>
    </div>
        <div class="form_main">

        <div class="clearfix pTop10">
            <div class="pb-10">
                <table>
                    <tr>
                        <td>
                            <% if (rights.CanAdd)
                               { %>
                            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus"></i></span><span>Add New</span> </a>
                            <% } %>

                            <% if (rights.CanExport)
                               { %>
                            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                <asp:ListItem Value="2">XLS</asp:ListItem>
                                <asp:ListItem Value="3">RTF</asp:ListItem>
                                <asp:ListItem Value="4">CSV</asp:ListItem>

                            </asp:DropDownList>
                            <% } %>
                            
                        </td>
                        <td>
                            <asp:LinkButton ID="lnlDownloaderexcel" runat="server" OnClick="lnlDownloaderexcel_Click" CssClass="btn btn-info btn-radius pull-right mBot0">Download Format</asp:LinkButton>
                        </td>

                        <td>
                            <label>&nbsp;</label>
                            <button type="button" onclick="ImportUpdatePopOpenEmployeesTarget();" class="btn btn-primary btn-radius">Import(Add/Update)</button>



                            <button type="button" class="btn btn-warning btn-radius" data-toggle="modal" data-target="#modalSS" id="btnViewLog" onclick="ViewLogData();">View Log</button>

                        </td>
                    </tr>
                </table>
            </div>


            <div class="col-md-5">
            </div>

        </div>
        <table class="TableMain100">
            <%--            <tr>
                <td class="EHEADER" style="text-align: center; height: 20px;">
                    <strong><span style="color: #000099">Employee Details</span></strong></td>
            </tr>--%>

            <tr>
                <td style="text-align: left; vertical-align: top">
                    <%-- <table>
                        <tr>
                            <td id="ShowFilter">
                                <% if (rights.CanAdd)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-primary"><span>Add New</span> </a>
                                <% } %>

                                <% if (rights.CanExport)
                                   { %>
                                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                    <asp:ListItem Value="2">XLS</asp:ListItem>
                                    <asp:ListItem Value="3">RTF</asp:ListItem>
                                    <asp:ListItem Value="4">CSV</asp:ListItem>

                                </asp:DropDownList>
                                <% } %>
                            </td>
                            <td id="Td1">
                                <div class="col-md-3">
                                    <label>Choose File</label>
                                    <div>
                                        <asp:FileUpload ID="OFDBankSelect" runat="server" Width="100%" />
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <label class="lblBlock">&nbsp;</label>
                                    <asp:LinkButton ID="lnlDownloaderexcel" runat="server" OnClick="lnlDownloaderexcel_Click" CssClass="btn btn-info">Download Format</asp:LinkButton>
                                </div>

                                <div class="col-md-3">
                                    <label>&nbsp;</label>
                                    <div>
                                        <asp:Button ID="BtnSaveexcel" runat="server" Text="Import File" OnClick="BtnSaveexcel_Click1" CssClass="btn btn-primary" />

                                    </div>
                                </div>
                            </td>
                            <td><button type="button" class="btn btn-sm btn-primary" data-toggle="modal" data-target="#modalSS" id="btnViewLog" onclick="ViewLogData();" >View Log</button></td>
                        </tr>
                    </table>--%>
                </td>

            </tr>
            <tr style="display: none">
                <td>
                    <div style="padding: 15px; background: #f9f9f9; border-radius: 3px; margin-bottom: 12px;">
                        <table cellpadding="1" cellspacing="1" style="display: none">
                            <tr id="trSpecific">
                                <td class="gridcellleft" style="vertical-align: top">Date Of Joining :</td>
                                <td valign="top" style="vertical-align: top">
                                    <dxe:ASPxRadioButtonList ID="rbDOJ_Specific_All" runat="server" SelectedIndex="0" ItemSpacing="10px"
                                        ClientInstanceName="crbDOJ_Specific_All" RepeatDirection="Horizontal" TextWrap="False">
                                        <Items>

                                            <dxe:ListEditItem Text="Specific" Value="S" />
                                            <dxe:ListEditItem Text="All" Value="A" />
                                        </Items>
                                        <ClientSideEvents ValueChanged="function(s, e) {ShowEmployeeFilterForm(s.GetValue());}" />
                                        <Border BorderWidth="0px" />
                                    </dxe:ASPxRadioButtonList>
                                </td>
                                <td align="right" valign="middle" id="td1" class="gridcellleft" style="vertical-align: top">&nbsp;From :</td>
                                <td valign="middle" class="gridcellleft" id="td2" style="vertical-align: top">
                                    <dxe:ASPxDateEdit ID="DtFrom" ClientInstanceName="cDtFrom" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                        <ButtonStyle Width="13px"></ButtonStyle>
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td valign="middle" align="right" id="td3" class="gridcellleft" style="vertical-align: top">To:</td>
                                <td valign="middle" class="gridcellleft" id="td4" style="vertical-align: top">
                                    <dxe:ASPxDateEdit ID="DtTo" ClientInstanceName="cDtTo" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                        <ButtonStyle Width="13px"></ButtonStyle>
                                    </dxe:ASPxDateEdit>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft" style="vertical-align: top">Search By :</td>
                                <td style="vertical-align: top" valign="top">
                                    <dxe:ASPxRadioButtonList ID="Rb_SearchBy" runat="server" ItemSpacing="10px" RepeatDirection="Horizontal"
                                        TextWrap="False" ClientInstanceName="cRb_SearchBy" SelectedIndex="0">
                                        <Border BorderWidth="0px" />
                                        <ClientSideEvents ValueChanged="function(s, e) {ShowFindOption();}" />
                                        <Items>
                                            <dxe:ListEditItem Text="None" Value="N"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Employee Name" Value="EN"></dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Employee Code" Value="EC"></dxe:ListEditItem>
                                        </Items>
                                    </dxe:ASPxRadioButtonList>
                                </td>
                                <td align="right" class="gridcellleft" style="vertical-align: top"
                                    valign="middle"></td>
                                <td class="gridcellleft" style="vertical-align: top" valign="middle">
                                    <dxe:ASPxButton ID="BtnShow" runat="server" AutoPostBack="False" Text="Show" CssClass="btn btn-primary">
                                        <ClientSideEvents Click="function (s, e) {BtnShow_Click();}" />
                                    </dxe:ASPxButton>
                                </td>
                                <td align="right" class="gridcellleft" style="vertical-align: top"
                                    valign="middle"></td>
                                <td class="gridcellleft" style="vertical-align: top" valign="middle"></td>
                            </tr>
                            <tr id="tr_EmployeeName">
                                <td class="gridcellleft" style="vertical-align: top">Employee Name :</td>
                                <td style="vertical-align: top" valign="top">
                                    <asp:TextBox ID="txtEmpName" onFocus="this.select()" runat="server"></asp:TextBox></td>
                                <td align="right" class="gridcellleft" style="vertical-align: top"
                                    valign="middle">Find Option</td>
                                <td class="gridcellleft" style="vertical-align: top" valign="middle">
                                    <dxe:ASPxComboBox ID="cmbEmpNameFindOption" runat="server"
                                        ClientInstanceName="exp" Font-Bold="False" ForeColor="black"
                                        SelectedIndex="0" ValueType="System.Int32" Width="170px">
                                        <Items>
                                            <dxe:ListEditItem Value="0" Text="Like"></dxe:ListEditItem>
                                            <dxe:ListEditItem Value="1" Text="Whole Word"></dxe:ListEditItem>
                                        </Items>
                                        <ButtonStyle>
                                        </ButtonStyle>
                                        <ItemStyle>
                                            <HoverStyle>
                                            </HoverStyle>
                                        </ItemStyle>
                                        <Border BorderColor="black"></Border>

                                    </dxe:ASPxComboBox>
                                </td>
                                <td align="right" class="gridcellleft" style="vertical-align: top"
                                    valign="middle"></td>
                                <td class="gridcellleft" style="vertical-align: top" valign="middle"></td>
                            </tr>
                            <tr id="tr_EmployeeCode">
                                <td class="gridcellleft" style="vertical-align: top">Employee Code :</td>
                                <td style="vertical-align: top" valign="top">
                                    <asp:TextBox ID="txtEmpCode" onFocus="this.select()" runat="server"></asp:TextBox></td>
                                <td align="right" class="gridcellleft" style="vertical-align: top"
                                    valign="middle">Find Option</td>
                                <td class="gridcellleft" style="vertical-align: top" valign="middle">
                                    <dxe:ASPxComboBox ID="cmbEmpCodeFindOption" runat="server"
                                        ClientInstanceName="exp" Font-Bold="False" ForeColor="black"
                                        SelectedIndex="0" ValueType="System.Int32" Width="170px">
                                        <Items>
                                            <dxe:ListEditItem Value="0" Text="Like"></dxe:ListEditItem>
                                            <dxe:ListEditItem Value="1" Text="Whole Word"></dxe:ListEditItem>
                                        </Items>
                                        <ButtonStyle>
                                        </ButtonStyle>
                                        <ItemStyle>
                                            <HoverStyle>
                                            </HoverStyle>
                                        </ItemStyle>
                                        <Border BorderColor="black"></Border>

                                    </dxe:ASPxComboBox>
                                </td>
                                <td align="right" class="gridcellleft" style="vertical-align: top"
                                    valign="middle"></td>
                                <td class="gridcellleft" style="vertical-align: top" valign="middle"></td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td id="Td5">
                                    <% if (rights.CanAdd)
                                       { %>
                                    <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success"><span class="btn-icon"><i class="fa fa-plus"></i></span><span>Add New</span> </a><% } %>
                                    <%-- <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">Show Filter</span></a>--%>
                                </td>
                                <td id="Td6">
                                    <%-- <a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr style="display: none">
                <td>


                    <table style="width: 100%" border="0">
                        <tr>
                            <td valign="top" style="vertical-align: top; width: 34px; text-align: left">Page </td>
                            <td valign="top" style="width: 4px">
                                <b style="text-align: right" id="B_PageNo" runat="server"></b>
                            </td>
                            <td valign="top" style="vertical-align: top; text-align: left;">Of
                            </td>
                            <td valign="top">
                                <b style="text-align: right" id="B_TotalPage" runat="server"></b>
                            </td>
                            <td valign="top" style="vertical-align: top; text-align: left">( <b style="text-align: right" id="B_TotalRows" runat="server"></b>&nbsp;items )
                            </td>
                            <td valign="top">
                                <table width="100%">
                                    <tr>
                                        <td valign="top" style="vertical-align: top; text-align: left">
                                            <a id="A_LeftNav" runat="server" href="javascript:void(0);" onclick="OnLeftNav_Click()">
                                                <img src="/assests/images/LeftNav.gif" width="10" />
                                            </a>
                                        </td>
                                        <td valign="top" style="vertical-align: top; text-align: left">
                                            <a id="A1" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A1')">1</a>
                                        </td>
                                        <td valign="top" style="vertical-align: top; text-align: left">
                                            <a id="A2" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A2')">2</a>
                                        </td>
                                        <td valign="top" style="vertical-align: top; text-align: left">
                                            <a id="A3" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A3')">3</a>
                                        </td>
                                        <td valign="top" style="vertical-align: top; text-align: left">
                                            <a id="A4" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A4')">4</a>
                                        </td>
                                        <td valign="top" style="vertical-align: top; text-align: left">
                                            <a id="A5" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A5')">5</a>
                                        </td>
                                        <td valign="top" style="vertical-align: top; text-align: left">
                                            <a id="A6" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A6')">6</a>
                                        </td>
                                        <td valign="top" style="vertical-align: top; text-align: left">
                                            <a id="A7" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A7')">7</a>
                                        </td>
                                        <td valign="top" style="vertical-align: top; text-align: left">
                                            <a id="A8" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A8')">8</a>
                                        </td>
                                        <td valign="top" style="vertical-align: top; text-align: left">
                                            <a id="A9" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A9')">9</a>
                                        </td>
                                        <td valign="top" style="vertical-align: top; text-align: left">
                                            <a id="A10" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A10')">10</a>
                                        </td>
                                        <td style="text-align: right; vertical-align: top;" valign="top">
                                            <a id="A_RightNav" runat="server" href="javascript:void(0);" onclick="OnRightNav_Click()">
                                                <img src="../images/RightNav.gif" width="10" />
                                            </a>
                                        </td>
                                        <td style="vertical-align: top; text-align: right" valign="top">
                                            <%--<asp:DropDownList ID="ddlExport" Onchange="ddlExport_OnChange()" runat="server"
                                                Width="100px">
                                                <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                                                <asp:ListItem Value="1">Excel</asp:ListItem>
                                            </asp:DropDownList>--%></td>

                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="relative">
                    <dxe:ASPxGridView ID="GrdEmployee" runat="server" KeyFieldName="cnt_id" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto"
                        Width="100%" ClientInstanceName="cGrdEmployee" OnCustomCallback="GrdEmployee_CustomCallback" SettingsBehavior-AllowFocusedRow="true" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <%--<ClientSideEvents EndCallback="function(s, e) {GrdEmployee_EndCallBack();}" />--%>
                        <ClientSideEvents EndCallback="function(s, e) {GridEmployee_EndCallBack();}" />
                        <SettingsBehavior AllowFocusedRow="true" ConfirmDelete="True" ColumnResizeMode="NextColumn" />
                        <Styles>
                            <Header SortingImageSpacing="5px" ImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Row Wrap="true">
                            </Row>
                            <%-- <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" ></FocusedRow>
                            <AlternatingRow Enabled="True"></AlternatingRow>--%>
                        </Styles>

                        <Columns>
                            <dxe:GridViewDataTextColumn Caption="Code" Visible="False" FieldName="ContactID"
                                VisibleIndex="0" FixedStyle="Left">
                                <PropertiesTextEdit DisplayFormatInEditMode="True">
                                </PropertiesTextEdit>
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Name" FieldName="Name"
                                VisibleIndex="2" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataDateColumn Caption="Joining On" FieldName="DOJ"
                                VisibleIndex="7" Width="100px" ReadOnly="True">
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataDateColumn>
                            <dxe:GridViewDataDateColumn Caption="Date Of Confirmation" FieldName="Date_Of_Confirmation"
                                VisibleIndex="8" Width="100px" ReadOnly="True">
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataDateColumn>
                            <dxe:GridViewDataTextColumn Caption="Department" FieldName="Department"
                                VisibleIndex="5" Width="120px">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Branch" FieldName="BranchName"
                                VisibleIndex="4" Width="75px">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="CTC" FieldName="CTC"
                                VisibleIndex="6" Width="75px" Visible="false">
                                <CellStyle CssClass="gridcellleft" Wrap="False" HorizontalAlign="Left">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Report To" FieldName="ReportTo"
                                VisibleIndex="9" Width="150px">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Designation" FieldName="Designation"
                                VisibleIndex="6" Width="150px">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Company" FieldName="Company"
                                VisibleIndex="3" Width="150px">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewCommandColumn Visible="False" ShowDeleteButton="true" VisibleIndex="16">
                                <%--<DeleteButton Visible="True" Text="Delete">
                                </DeleteButton>--%>
                            </dxe:GridViewCommandColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="0">
                                <DataItemTemplate>
                                    <div class='floatedBtnArea'>
                                        <% if (rights.CanContactPerson)
                                           { %>
                                        <a href="javascript:void(0);" onclick="OnContactInfoClick('<%#Eval("ContactID") %>','<%#Eval("Name") %>')" title="" class="">
                                            <span class='ico ColorSeven'><i class='fa fa-user'></i></span><span class='hidden-xs'>Show contact person</span>
                                        </a><% } %>
                                        <% if (rights.CanEdit)
                                           { %>
                                        <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')" class="" title="">
                                            <span class='ico ColorSix'><i class='fa fa-pencil'></i></span><span class='hidden-xs'>Edit</span></a><% } %>
                                        <%--Rev Work Start 22.04.2022 MantiseID:0024850: Copy feature add in Employee master--%>
                                        <% if (rights.CanAdd)
                                           { %>
                                        <a href="javascript:void(0);" onclick="OnCopyClick('<%# Container.KeyValue %>')" class="" title="">
                                            <span class='ico ColorSix'><i class='fa fa-pencil'></i></span><span class='hidden-xs'>Copy</span></a><% } %>
                                        <%--Rev Work Close 22.04.2022 MantiseID:0024850: Copy feature add in Employee master--%>
                                        <% if (rights.CanIndustry)
                                           { %>
                                        <a href="javascript:void(0);" onclick="OnAddBusinessClick('<%#Eval("ContactID") %>','<%#Eval("Name") %>')" title="" class="" style="text-decoration: none;">
                                            <span class='ico editColor'><i class='fa fa-industry' aria-hidden='true'></i></span><span class='hidden-xs'>Add Industry</span><% } %>
                                            <% if (rights.CanDelete)
                                               { %>

                                            <a href="javascript:void(0);" onclick="fn_DeleteEmp('<%#Eval("ContactID") %>')" title="">
                                                <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                            <% } %>

                                            <%-- <asp:LinkButton ID="btn_delete" runat="server" OnClientClick="return confirm('Confirm delete?');" CommandArgument='<%# Container.KeyValue %>' CommandName="delete" ToolTip="Delete" Font-Underline="false">
                                                <img src="/assests/images/Delete.png" />
                                                </asp:LinkButton>--%>
                                            <% if (rights.CanAssignbranch && SrvBranchMap)
                                               { %>
                                            <a href="javascript:void(0);" onclick="fn_BranchMap('<%#Eval("ContactID") %>')" title="">
                                                <span class='ico deleteColor'><i class='fa fa-sitemap' aria-hidden='true'></i></span><span class='hidden-xs'>Branch Mapping</span></a>
                                            <% } %>
                                    </div>
                                </DataItemTemplate>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />

                                <HeaderTemplate><span>Actions</span></HeaderTemplate>

                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Employee Code" FieldName="Code"
                                VisibleIndex="1" FixedStyle="Left" Width="150px">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                        </Columns>
                        <ClientSideEvents RowClick="gridRowclick" />
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <SettingsPager PageSize="10" ShowSeparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                        </SettingsPager>
                        <SettingsCommandButton>
                            <DeleteButton ButtonType="Image" Image-Url="/assests/images/Delete.png">
                            </DeleteButton>
                        </SettingsCommandButton>
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True"
                            PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="900px" EditFormColumnCount="3" />
                        <SettingsText PopupEditFormCaption="Add/ Modify Employee" ConfirmDelete="Are you sure to delete?" />

                        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsLoadingPanel Text="Please Wait..." />
                    </dxe:ASPxGridView>

                </td>
            </tr>
        </table>
        <br />
        <asp:HiddenField ID="hdn_GridBindOrNotBind" runat="server" />
        <asp:Button ID="BtnForExportEvent" runat="server" OnClick="cmbExport_SelectedIndexChanged" BackColor="#DDECFE" BorderStyle="None" Visible="false" />
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" GridViewID="GrdEmployee" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>

    </div>
    </div>
    <div class="modal fade" id="modalSS" role="dialog">
        <div class="modal-dialog fullWidth">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Employee Log</h4>
                </div>
                <div class="modal-body">

                    <dxe:ASPxGridView ID="GvJvSearch" runat="server" AutoGenerateColumns="False" SettingsBehavior-AllowSort="true"
                        ClientInstanceName="cGvJvSearch" KeyFieldName="EmpLogId" Width="100%" OnDataBinding="GvJvSearch_DataBinding" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight="400">

                        <SettingsBehavior ConfirmDelete="false" ColumnResizeMode="NextColumn" />
                        <Styles>
                            <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
                            <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>
                            <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                            <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
                            <Footer CssClass="gridfooter"></Footer>
                        </Styles>
                        <Columns>
                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="EmpLogId" Caption="LogID" SortOrder="Descending">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="CreatedDatetime" Caption="Date" Width="10%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="EmployeeCode" Caption="Employee Code" Width="10%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="LoopNumber" Caption="Row Number" Width="13%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="EmpName" Width="8%" Caption="Employee Name">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="FileName" Width="14%" Caption="File Name">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Description" Caption="Description" Width="10%" Settings-AllowAutoFilter="False">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Status" Caption="Status" Width="14%" Settings-AllowAutoFilter="False">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsSearchPanel Visible="false" />
                        <SettingsPager NumericButtonCount="200" PageSize="200" ShowSeparators="True" Mode="ShowPager">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="200,400,600" />
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                    </dxe:ASPxGridView>

                </div>

            </div>
        </div>
    </div>


    <div class="modal fade" id="modalimport" role="dialog">
        <div class="modal-dialog VerySmall">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Select File to Import (Add/Update)</h4>
                </div>
                <div class="modal-body">

                    <div class="col-md-12">
                        <div id="divproduct">

                            <div>
                                <asp:FileUpload ID="OFDBankSelect" accept=".xls,.xlsx" runat="server" Width="100%" />
                                <div class="pTop10  mTop5">
                                    <asp:Button ID="BtnSaveexcel" runat="server" Text="Import(Add/Update)" OnClick="BtnSaveexcel_Click1" CssClass="btn btn-primary" />
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

            </div>
        </div>
    </div>


    <div id="myModal" class="modal fade" data-backdrop="static" role="dialog">
        <div class="modal-dialog" style="width: 450px;">

            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Branch List</h4>
                </div>
                <div class="modal-body">
                    <div>

                        <input type="text" id="myInput" onkeyup="myFunction()" placeholder="Search for Branch.">

                        <ul id="divModalBody" class="listStyle">
                            <%--<input type="checkbox" id="idstate" class="statecheck" /><label id="lblstatename" class="lblstate"></label>--%>
                        </ul>
                    </div>
                    <input type="button" id="btnsatesubmit" title="SUBMIT" value="SUBMIT" class="btn btn-primary" onclick="BranchPushPop()" />
                    <input type="hidden" id="hdnstatelist" class="btn btn-primary" />
                    <input type="hidden" id="hdnEMPID" class="btn btn-primary" />
                </div>
            </div>

        </div>
    </div>

    <style>
        #myInput {
            background-image: url('/css/searchicon.png'); /* Add a search icon to input */
            background-position: 10px 12px; /* Position the search icon */
            background-repeat: no-repeat; /* Do not repeat the icon image */
            width: 100%; /* Full-width */
            font-size: 16px; /* Increase font-size */
            padding: 12px 20px 12px 40px; /* Add some padding */
            border: 1px solid #ddd; /* Add a grey border */
            margin-bottom: 12px; /* Add some space below the input */
        }

        #divModalBody {
            /* Remove default list styling */
            list-style-type: none;
            padding: 0;
            margin: 0;
            margin-bottom: 8px;
        }

            #divModalBody li {
                padding: 5px 10px;
            }

                #divModalBody li a {
                    margin-top: -1px; /* Prevent double borders */
                    padding: 0 12px; /* Add some padding */
                    text-decoration: none; /* Remove default text underline */
                    font-size: 14px; /* Increase the font-size */
                    color: black; /* Add a black text color */
                    display: inline-block; /* Make it into a block element to fill the whole list */
                    cursor: pointer;
                }
    </style>

    <script>
        function myFunction() {
            // Declare variables
            var input, filter, ul, li, a, i, txtValue;
            input = document.getElementById('myInput');
            filter = input.value.toUpperCase();
            ul = document.getElementById("divModalBody");
            li = ul.getElementsByTagName('li');

            // Loop through all list items, and hide those who don't match the search query
            for (i = 0; i < li.length; i++) {
                a = li[i].getElementsByTagName("a")[0];
                txtValue = a.textContent || a.innerText;

                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    li[i].style.display = "";
                } else {
                    li[i].style.display = "none";
                }

            }
        }
    </script>

</asp:Content>
