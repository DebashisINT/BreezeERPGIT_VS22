<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                21-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="Currency Master" Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="frm_currency_master.aspx.cs"  MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Master.frm_currency_master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script language="javascript" type="text/javascript">

        function maxLengthCheck(object) {
            if (object.value.length > object.maxLength)
                object.value = object.value.slice(0, object.maxLength)
        }

        function isNumeric(evt) {
            var theEvent = evt || window.event;
            var key = theEvent.keyCode || theEvent.which;
            key = String.fromCharCode(key);
            var regex = /[0-9]|\./;
            if (!regex.test(key)) {
                theEvent.returnValue = false;
                if (theEvent.preventDefault) theEvent.preventDefault();
            }
        }
        //Code for UDF Control 
        function OpenUdf() {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                var url = 'frm_BranchUdfPopUp.aspx?Type=FI';
                popup.SetContentUrl(url);
                popup.Show();
            }
            return true;
        }
        // End Udf Code

        function popUpRedirect(obj) {
            jAlert('Saved successfully');
            window.location.href = obj;


        }

        function Submited(obj) {
            jAlert('Saved successfully');
            var url = "frm_currency_master.aspx?id=" + obj;
             window.location.href = url;
        }

        $(document).ready(function () {
           
            cell1.innerHTML = "<select style='width:150px'>  " + document.getElementById("CurrencyHD").value + " </select>";
            loadExecutiveNameFromField();
        });


        function loadExecutiveNameFromField() {

            var table = document.getElementById("executiveTable");
            var exeName = document.getElementById('executiveName_hidden').value;
            for (var i = 0 ; i < exeName.split(',').length; i++) {
                var j = 0;
                if (exeName.split(',')[i].trim() != '') {
                    var values = exeName.split(',');
                    if (table.rows[0].cells[2].children[0].value.trim() != '') {
                        for (j = 0; j < values[i].toString().split('~').length; j++) {
                            var row = table.insertRow(0);
                            var cell1 = row.insertCell(0);
                            var cell2 = row.insertCell(1);
                            var cell3 = row.insertCell(2);
                            var cell4 = row.insertCell(3);
                            var cell5 = row.insertCell(4);                           
                            var replce = document.getElementById("CurrencyHD").value;
                           // replce = replce.replace("value='" + values[i].toString().split('~')[j] + "'", "value='" + values[i].toString().split('~')[j] + "' selected='selected'");
                            replce = replce.replace("value=" + values[i].toString().split('~')[j] + "", "value=" + values[i].toString().split('~')[j] + " selected='selected'");
                            cell1.innerHTML = "<select style='width:150px'>  " + replce + " </select>";                        

                            j++;
                            cell2.innerHTML = "<input class='flatpickr' 'type='text'  value='" + values[i].toString().split('~')[j] + "'/>";
                            //cell2.innerHTML = "<input type='date'  value='" + values[i].toString().split('~')[j] + "'/>";
                            j++;
                            cell3.innerHTML = "<input type='number' step='0.00001' maxlength = '18' onkeypress='return isNumeric(event)' oninput='maxLengthCheck(this)' value='" + values[i].toString().split('~')[j] + "'/>";
                            j++;
                            cell4.innerHTML = "<input type='number' step='0.00001' maxlength = '18' onkeypress='return isNumeric(event)' oninput='maxLengthCheck(this)' value='" + values[i].toString().split('~')[j] + "'/>";
                            j++;
                            if ('<%= edit %>' == 'True') {
                                cell5.innerHTML = "<button type='button' value='' onclick='AddNewexecutive()' class='green' style='display:none'><i class='fa fa-plus-circle'></i></button> <button type='button' value='' class='red' onclick='removeExecutive(this.parentNode.parentNode)' style='display:none'><i class='fa fa-times-circle'></i></button>";
                            }
                            else {
                                cell5.innerHTML = "<button type='button' value='' onclick='AddNewexecutive()' class='green'><i class='fa fa-plus-circle'></i></button> <button type='button' value='' class='red' onclick='removeExecutive(this.parentNode.parentNode)'><i class='fa fa-times-circle'></i></button>";
                            }

                            cell2.children[0].flatpickr({
                                enableTime: true,
                                weekNumbers: true

                            });
                            //cell1.innerHTML = "<input type='text' maxlength='50' value='" + exeName.split('~')[i] + "'/>";

                        }
                    }
                    else {
                        for (j = 0; j < values[i].toString().split('~').length; j++) {
                            table.rows[0].cells[0].children[0].value = exeName.split('~')[i];

                            var replce = document.getElementById("CurrencyHD").value;
                           // replce = replce.replace("value='" + values[i].toString().split('~')[j] + "'", "value='" + values[i].toString().split('~')[j] + "' selected='selected'");
                            replce = replce.replace("value=" + values[i].toString().split('~')[j] + "", "value=" + values[i].toString().split('~')[j] + " selected='selected'");
                            table.rows[0].cells[0].innerHTML = "<select style='width:150px'>  " + replce + " </select>";

                            j++;
                            //var date = values[i].toString().split('~')[j];
                            //date = date.split('-');


                            table.rows[0].cells[1].innerHTML = "<input class='flatpickr' 'type='text'  value='" + values[i].toString().split('~')[j] + "'/>";
                            j++;
                            table.rows[0].cells[2].innerHTML = "<input type='number' step='0.00001' maxlength = '18' onkeypress='return isNumeric(event)' oninput='maxLengthCheck(this)' value='" + values[i].toString().split('~')[j] + "' novalidate />";
                            j++;
                            table.rows[0].cells[3].innerHTML = "<input type='number' step='0.00001' maxlength = '18' onkeypress='return isNumeric(event)' oninput='maxLengthCheck(this)' value='" + values[i].toString().split('~')[j] + "'/>";
                            j++;
                            if ('<%= edit %>' == 'True') {
                                table.rows[0].cells[4].innerHTML = "<button type='button' value='' onclick='AddNewexecutive()' class='green' style='display:none'><i class='fa fa-plus-circle'></i></button> <button type='button' value='' class='red' onclick='removeExecutive(this.parentNode.parentNode)' style='display:none'><i class='fa fa-times-circle'></i></button>";
                            }
                            else {
                                table.rows[0].cells[4].innerHTML = "<button type='button' value='' onclick='AddNewexecutive()' class='green'><i class='fa fa-plus-circle'></i></button> <button type='button' value='' class='red' onclick='removeExecutive(this.parentNode.parentNode)'><i class='fa fa-times-circle'></i></button>";
                            }
                            //cell1.innerHTML = "<input type='text' maxlength='50' value='" + exeName.split('~')[i] + "'/>";
                            //cell1.children[0].flatpickr({
                            //    enableTime: true,
                            //    weekNumbers: true

                            //});
                        }
                    }


                }
            }

        }



        function AddNewexecutive() {
         
         
             var table = document.getElementById("executiveTable");
            var row = table.insertRow(0);
            var cell1 = row.insertCell(0);
            var cell2 = row.insertCell(1);
            var cell3 = row.insertCell(2);
            var cell4 = row.insertCell(3);
            var cell5 = row.insertCell(4);
            cell1.innerHTML = "<select style='width:150px'>  " + document.getElementById("CurrencyHD").value + " </select>";
            cell2.innerHTML = "<input class='flatpickr' type='text' placeholder='Select Date..'/>";
            cell3.innerHTML = "<input type='number' step='0.00001' maxlength = '18' onkeypress='return isNumeric(event)' oninput='maxLengthCheck(this)'/>";
            cell4.innerHTML = "<input type='number' step='0.00001' maxlength = '18' onkeypress='return isNumeric(event)' oninput='maxLengthCheck(this)'/>";
            cell5.innerHTML = "<button type='button' value='' onclick='AddNewexecutive()' class='green'><i class='fa fa-plus-circle'></i></button> <button type='button' value='' class='red' onclick='removeExecutive(this.parentNode.parentNode)'><i class='fa fa-times-circle'></i></button>";
            cell2.children[0].flatpickr({
                enableTime: true,
                weekNumbers: true

            });

        }
        function removeExecutive(obj) {
            var rowIndex = obj.rowIndex;
            var table = document.getElementById("executiveTable");
            if (table.rows.length > 1) {
                table.deleteRow(rowIndex);
            } else {
                jAlert('Cannot delete all Executive.');
            }
        }
        function isValid() {
            if (document.getElementById('txtFinancerId').value.trim() == '') {
                return false;
            }
            if (document.getElementById('txtFinancerName').value.trim() == '') {
                return false;
            }
            return true;
        }

        function ClientSaveClick() {
            ////if (isValid()) {
            if (document.getElementById("DrpCompany").value=='0')
            {
                 jAlert("Company Required");
                 return false;
            }
            var table = document.getElementById("executiveTable");
            document.getElementById('executiveName_hidden').value = '';
            var data = '';
            //var arr[] = new Array();
            for (var i = 0, row; row = table.rows[i]; i++) {
                for (var j = 0, col; col = row.cells[j]; j++) {
                    if (col.children[0].type != 'button') {
                        if (data == '') {
                            if (col.children[0].value == '') {
                                jAlert("Base Currency Required");
                                return false;
                            }
                            else {
                                data = col.children[0].value;
                            }
                        }
                        else {
                            if (col.children[0].value == '') {
                                jAlert("Required");
                                return false;
                            }
                            else {
                                data = data + '~' + col.children[0].value;
                            }
                        }
                    }
                }
                //arr.push(data);
                if (document.getElementById('executiveName_hidden').value == '') {
                    document.getElementById('executiveName_hidden').value = data;
                    data = '';
                }
                else {
                    document.getElementById('executiveName_hidden').value = document.getElementById('executiveName_hidden').value + ',' + data;
                    data = '';
                }

            }

            return true;

        }

        function OnEndCallback(s, e) {

        }




    </script>
    <style>
        .nbackBtn button {
            background: transparent !important;
            border: none !important;
            font-size: 21px;
        }

            .nbackBtn button.green {
                color: #2db52d;
            }

            .nbackBtn button.red {
                color: #f53434;
            }

        .nbackBtn tr td:last-child {
            position: absolute;
        }

        .abs {
            position: absolute;
            top: 8px;
            right: -20px;
        }
        
        #executiveTable td, .pdri td {
            padding-right: 20px;
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
            right: 5px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #txtDOB , #txtAnniversary , #txtcstVdate , #txtLocalVdate ,
        #txtCINVdate , #txtincorporateDate , #txtErpValidFrom , #txtErpValidUpto , #txtESICValidFrom , #txtESICValidUpto , #FormDate , #toDate
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
        #txtESICValidUpto_B-1 , #FormDate_B-1 , #toDate_B-1
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
        #txtESICValidUpto_B-1 #txtESICValidUpto_B-1Img , #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img
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
            right: -2px;
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

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #EmployeeGrid , .TableMain100 #GrdEmployee,
        .TableMain100 #GrdHolidays , #gridFinancer
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

        button, input, select[multiple], textarea
        {
            height: 30px;
    border-radius: 4px;
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
    <link rel="stylesheet" href="https://unpkg.com/flatpickr/dist/flatpickr.min.css">
    <link href="../../../assests/bootstrap/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <script src="../../../assests/bootstrap/js/bootstrap-datetimepicker.min.js"></script>

    <script src="https://unpkg.com/flatpickr"></script>

    <script>
        $(document).ready(function () {
            $(".flatpickr").flatpickr({
                enableTime: true,
                weekNumbers: true
                
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Add/Edit Currency</h3>
            <div class="crossBtn"><a href="CurrencyMaster.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>

        <div class="form_main clearfix">
        <dxe:ASPxCallbackPanel runat="server" ID="ExecutiveCallbackPanel" ClientInstanceName="CallbackPanel" RenderMode="Table" OnCallback="ExecutiveCallbackPanel_Callback">
            <ClientSideEvents EndCallback="OnEndCallback"></ClientSideEvents>
            <PanelCollection>
                <dxe:PanelContent ID="PanelContent3" runat="server">
                </dxe:PanelContent>
            </PanelCollection>
        </dxe:ASPxCallbackPanel>

        <%--debjyoti 22-12-2016--%>
        <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
            Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>

        <asp:HiddenField runat="server" ID="IsUdfpresent" />
        <%--End debjyoti 22-12-2016--%>


        <table style="width: 100%">
            <tr>
                <td style="width: 100%">
                    
                                    <dxe:ContentControl runat="server">
                                        <div class="totalWrap">
                                            <table>
                                                <tr>

                                                    <td width="300px" style="position: relative;" class="simple-select">
                                                        <label>Company<span style="color: red">*</span></label>
                                                        <%-- <asp:TextBox ID="txtFinancerId" runat="server"   Width="100%"  onblur="onFinancerChange()"
                                                        MaxLength="80" />
                                                       
                                                        <asp:RequiredFieldValidator Display="Dynamic" runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtFinancerId"
                                                        SetFocusOnError="true" ErrorMessage="" class="pullrightClass fa fa-exclamation-circle abs iconRed" ToolTip="Mandatory" ValidationGroup="fingroup" >                                                        
                                                    </asp:RequiredFieldValidator>--%>
                                                        <asp:DropDownList ValidateRequestMode="Disabled" ID="DrpCompany" runat="server" Width="100%" OnSelectedIndexChanged="DrpCompany_SelectedIndexChanged1" AutoPostBack="true"></asp:DropDownList>

                                                    </td>
                                                </tr>
                                                <tr>

                                                    <td style="position: relative;">
                                                        <label>Base Currency</label>

                                                        <%-- <asp:DropDownList ID="DrpCurrency" runat="server" Width="100%"></asp:DropDownList>--%>
                                                        <%--<label id="lblcurrency" runat="server"></label>--%>

                                                        <asp:TextBox ID="txtcurrency" runat="server" Enabled="false"></asp:TextBox>
                                                        <asp:HiddenField runat="server" ID="CurrencyHD" />
                                                    </td>
                                                </tr>
                                                <%--<tr>

                                                    <td>
                                                        <asp:Button ID="Button1" runat="server" Text="Save" ValidationGroup="fingroup" CssClass="btn btn-primary dxbButton" OnClick="Button1_Click" />
                                                    </td>
                                                </tr>--%>
                                            </table>
                                            <table>


                                                <tr>

                                                    <td style="border-top:1px solid #ccc;padding-top:15px;">
                                                        <table style="width: 100%;" class="pdri">
                                                            <tr>
                                                                <td style="width: 180px">
                                                                    <label style="display: block;">Currency<span style="color: red">*</span> </label>
                                                                </td>
                                                                <td style="width: 163px">
                                                                    <label style="display: block;">Date<span style="color: red">*</span></label></td>
                                                                <td style="width: 170px">
                                                                    <label style="display: block;">Sale Rate<span style="color: red">*</span></label></td>
                                                                <td style="width: 180px">
                                                                    <label style="display: block;">Purchase Rate<span style="color: red">*</span></label></td>
                                                                <td></td>
                                                            </tr>
                                                        </table>
                                                        <table id="executiveTable" style="width: 100%;" class="nbackBtn">

                                                            <tr>

                                                                <td id="cell1" style="width: 150px">
                                                                    <%--  <label style=" display: block;">Currency </label>--%>
                                                                    <%-- <select id="Select1" runat="server" style="width:150px"> 
                                                                     <option></option>  
                                                                 </select>--%>


                                                                </td>

                                                                <td style="width: 150px">
                                                                    <%-- <label style=" display: block;">Date</label>--%>
                                                                    <input class="flatpickr" type="text" placeholder="Select Date.."/>
                                                                      <%--<input  type="date"/>--%>
                                                               
                                                                </td>

                                                                <td style="width: 150px">
                                                                    <%-- <label style=" display: block;">Sale Rate</label> --%>
                                                                    <input type="number" step='0.00001'  maxlength = "18" onkeypress="return isNumeric(event)" oninput="maxLengthCheck(this)"/>

                                                                </td>

                                                                <td style="width: 150px">
                                                                    <%-- <label style=" display: block;">Purchase Rate</label>--%>
                                                                    <input type="number" step='0.00001' maxlength = "18" onkeypress="return isNumeric(event)" oninput="maxLengthCheck(this)"/>

                                                                </td>

                                                                <td>
                                                                    <%-- <label style=" display: block;">&nbsp</label>--%>
                                                                    <button type="button" class="green" onclick="AddNewexecutive()"><i class="fa fa-plus-circle"></i></button>
                                                                    <button type="button" class="red" onclick="removeExecutive(this.parentNode.parentNode)"><i class="fa fa-times-circle"></i></button>
                                                                </td>

                                                            </tr>
                                                        </table>
                                                        <asp:HiddenField ID="executiveName_hidden" runat="server" />
                                                        <asp:HiddenField ID="currid_hidden" runat="server" />
                                                    </td>
                                                </tr>

                                            </table>
                                            <div class="clear"></div>
                                            <div class="" style="padding-top: 10px;">

                                                <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="fingroup" CssClass="btn btn-primary dxbButton" OnClick="btnSave_Click" OnClientClick="return ClientSaveClick()" />
                                                <asp:Button ID="Button2" runat="server" Text="Cancel" CssClass="btn btn-danger dxbButton" OnClick="Button2_Click" />
                                                <asp:Button ID="btnUdf" runat="server" Text="UDF" CssClass="btn btn-primary dxbButton hide" OnClientClick="if(OpenUdf()){ return false;}" />
                                            </div>
                                        </div>

                                    </dxe:ContentControl>
                            


                </td>
            </tr>
        </table>
    </div>
    </div>
</asp:Content>

