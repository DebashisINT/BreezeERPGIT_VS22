<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                22-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="BalanceSheetSchedule.aspx.cs" Inherits="ERP.OMS.Management.Master.BalanceSheetSchedule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .container {
            display: table;
        }

        .contentButtons {
            padding-top: 20px;
            padding-bottom: 10px;
        }

        .button {
            width: 100% !important;
        }

        @media(min-width:790px) {
            .contentEditors, .contentButtons {
                display: table-cell;
                width: 33.33333333%;
            }

            .button {
                width: 170px !important;
            }

            .contentEditors {
                vertical-align: top;
            }

            .contentButtons {
                vertical-align: middle;
                text-align: center;
            }
        }

        .borderNone, .borderNone > tbody > tr > td {
            border: none !important;
            padding-left: 33px;
        }

        #ViewLayoutModel .modal-body {
            background-color: #fff;
        }

        .dxgvHEC {
            width:0px; !important;
        }
        #bsgrid_DXDataRow2 > td.dxgvHEC{
            width:0px; !important;
        }

         #dxgvHeader_PlasticBlue > .dxgvHEC {
            width:0px; !important;
        }

         #lbChoosen_+_D{
             display: none !important;
         }

         #lbChoosen_-_D{
             display: none !important;
         }
         span[id^="lbChoosen_+"] {
             display:none;
         }
         span[id^="lbChoosen_-"] {
             display:none;
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
            top: 6px;
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
        .TableMain100 #GrdHolidays , #AccountGroup
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

        #DivSetAsDefault
        {
            margin-top: 25px;
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



    <script type="text/javascript">


        function cmbType_Change() {
            //cGroupCallBackPanel.PerformCallback();
            var obj = {};
            obj.Type = ccmbType.GetValue();
            var CheckUniqueCodee = false;
            $.ajax({
                type: "POST",
                url: "BalanceSheetSchedule.aspx/FilldllData",
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var returnObject = msg.d;
                    if (returnObject.listSubGroup) {
                        SetDataSourceOnComboBox(ccmbCategory, returnObject.listSubGroup);
                    }
                    //if (returnObject.listGroup) {
                    //    SetDataSourceOnComboBox(ccmbGroup, returnObject.listGroup);
                    //}
                    //if (returnObject.listSubGroup1) {
                    //    SetDataSourceOnComboBox(ccmbSubGroup1, returnObject.listSubGroup1);
                    //}
                    //if (returnObject.listSubGroup2) {
                    //    SetDataSourceOnComboBox(ccmbSubGroup2, returnObject.listSubGroup2);
                    //}
                    //if (returnObject.listSubGroup3) {
                    //    SetDataSourceOnComboBox(ccmbSubGroup3, returnObject.listSubGroup3);
                    //}
                }
            });

            ccmbGroup.ClearItems();
            ccmbGroup.AddItem("-Select-", "0");
            ccmbGroup.SetSelectedIndex(0);

            ccmbSubGroup1.ClearItems();
            ccmbSubGroup1.AddItem("-Select-", "0");
            ccmbSubGroup1.SetSelectedIndex(0);

            ccmbSubGroup2.ClearItems();
            ccmbSubGroup2.AddItem("-Select-", "0");
            ccmbSubGroup2.SetSelectedIndex(0);

            ccmbSubGroup3.ClearItems();
            ccmbSubGroup3.AddItem("-Select-", "0");
            ccmbSubGroup3.SetSelectedIndex(0);

        }

        function SchedulePopUpEndCall() {

        }

        function CloseFormulaWindow() {
            cFormulaPopUp.Hide();
        }

        function ccmbCategory_Change() {
            //cGroupCallBackPanel.PerformCallback();
            var obj = {};
            obj.ParentId = ccmbCategory.GetValue();
            obj.orderId = "0";
            var CheckUniqueCodee = false;
            $.ajax({
                type: "POST",
                url: "BalanceSheetSchedule.aspx/FilldllChildData",
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var returnObject = msg.d;
                    if (returnObject.listSubGroup) {
                        SetDataSourceOnComboBox(ccmbGroup, returnObject.listSubGroup);
                    }

                }
            });

            ccmbSubGroup1.ClearItems();
            ccmbSubGroup1.AddItem("-Select-", "0");
            ccmbSubGroup1.SetSelectedIndex(0);

            ccmbSubGroup2.ClearItems();
            ccmbSubGroup2.AddItem("-Select-", "0");
            ccmbSubGroup2.SetSelectedIndex(0);

            ccmbSubGroup3.ClearItems();
            ccmbSubGroup3.AddItem("-Select-", "0");
            ccmbSubGroup3.SetSelectedIndex(0);


        }


        function ccmbGroup_Change() {
            //cGroupCallBackPanel.PerformCallback();
            var obj = {};
            obj.ParentId = ccmbGroup.GetValue();
            obj.orderId = "1";
            var CheckUniqueCodee = false;
            $.ajax({
                type: "POST",
                url: "BalanceSheetSchedule.aspx/FilldllChildData",
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var returnObject = msg.d;
                    if (returnObject.listSubGroup) {
                        SetDataSourceOnComboBox(ccmbSubGroup1, returnObject.listSubGroup);
                    }

                }
            });

            ccmbSubGroup2.ClearItems();
            ccmbSubGroup2.AddItem("-Select-", "0");
            ccmbSubGroup2.SetSelectedIndex(0);




        }

        function ccmbSubGroup1_Change() {
            //cGroupCallBackPanel.PerformCallback();
            var obj = {};
            obj.ParentId = ccmbSubGroup1.GetValue();
            obj.orderId = "2";
            var CheckUniqueCodee = false;
            $.ajax({
                type: "POST",
                url: "BalanceSheetSchedule.aspx/FilldllChildData",
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var returnObject = msg.d;
                    if (returnObject.listSubGroup) {
                        SetDataSourceOnComboBox(ccmbSubGroup2, returnObject.listSubGroup);
                    }

                }
            });


            ccmbSubGroup3.ClearItems();
            ccmbSubGroup3.AddItem("-Select-", "0");
            ccmbSubGroup3.SetSelectedIndex(0);

        }




        function ccmbSubGroup2_Change() {
            //cGroupCallBackPanel.PerformCallback();
            var obj = {};
            obj.ParentId = ccmbSubGroup2.GetValue();
            obj.orderId = "3";
            var CheckUniqueCodee = false;
            $.ajax({
                type: "POST",
                url: "BalanceSheetSchedule.aspx/FilldllChildData",
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var returnObject = msg.d;
                    if (returnObject.listSubGroup) {
                        SetDataSourceOnComboBox(ccmbSubGroup3, returnObject.listSubGroup);
                    }

                }
            });




        }













        function Delete_Row(ID) {

            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    cbsgrid.PerformCallback("Delete~" + ID);
                }
            });

        }

        function SetDataSourceOnComboBox(ControlObject, Source) {
            ControlObject.ClearItems();
            for (var count = 0; count < Source.length; count++) {
                ControlObject.AddItem(Source[count].ScheduleGroup_Name, Source[count].ScheduleGroup_Id);
            }
            ControlObject.SetSelectedIndex(0);
        }
        
        function AddData() {

            var valid = true;
            if (ctxtSl.GetText() == "") {
                valid = false;
                jAlert('Please enter a valid serial no. to proceed.', 'Error');
                return;
            }
            if (ccmbSubGroup2.GetValue() == 0 && ccmbSubGroup3.GetValue() != 0) {
                valid = false;
                jAlert('Please select a valid sub group 2 to proceed.', 'Error');
                return;
            }
            if (ccmbSubGroup1.GetValue() == 0 && ccmbSubGroup2.GetValue() != 0) {
                valid = false;
                jAlert('Please select a valid sub group 1 to proceed.', 'Error');
                return;
            }
            if (ccmbGroup.GetValue() == 0 && ccmbSubGroup1.GetValue() != 0) {
                valid = false;
                jAlert('Please select a valid group to proceed.', 'Error');
                return;
            }
            if (ccmbCategory.GetValue() == 0 && ccmbGroup.GetValue() != 0) {
                valid = false;
                jAlert('Please select a valid category to proceed.', 'Error');
                return;
            }
            if (ccmbType.GetValue() == 0 || ccmbType.GetText() == "") {
                valid = false;
                jAlert('Please select a valid type to proceed.', 'Error');
                return;
            }



            if (valid)
                cbsgrid.PerformCallback("AddData");
        }
        function SaveData() {
            cbsgrid.PerformCallback("SaveData");
        }

        function ViewData() {
            var obj = {};
            var CheckUniqueCodee = false;
            $.ajax({
                type: "POST",
                url: "BalanceSheetSchedule.aspx/ViewLayout",
                data: JSON.stringify(),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var returnObject = msg.d;

                    //var txt = '';
                    //var count = 0;
                    //txt = "<table border='1' width='100%' class='borderNone'><tr class='HeaderStyle'>";
                    //for (x in returnObject) {
                    //    txt += "<tr>";
                    //    var PropertyCount = 0;

                    //    for (key in returnObject[0]) {
                    //        txt += "<td>" + returnObject[x][key] + "</td>"
                    //        PropertyCount++;
                    //    }
                    //    txt += "</tr>";
                    //    count++;
                    //}
                    //txt += "</table>"

                    LayoutTable.innerHTML = returnObject;
                    $("#ViewLayoutModel").modal('show');
                }
            });
        }

        function ViewLayoutModelClose() {
            $("#ViewLayoutModel").modal('hide');
        }

        document.onkeydown = function (e) {
            //event.preventDefault();
            if (event.keyCode == 65 && event.altKey) {
                document.getElementById("btnAdd").click();
            }
            if (event.keyCode == 83 && event.altKey) {
                document.getElementById("btnSave").click();
            }
            if (event.keyCode == 80 && event.altKey) {
                document.getElementById("btnView").click();
            }
        }


        function Ledger_Map(Type, Id) {
            Row_id = Id;
            Row_Type = Type;
            cLedgerPopUp.Show();
            cLedgerGrid.PerformCallback('ShowDetails~' + Row_id + '~' + Row_Type);
        }

        var returnObject;
        function LedgerNewkeydown(e) {

            var newobj = [];
            if (e.code == "Enter" || e.code == "NumpadEnter") {

                var SearchObj = $("#txtLedgerSearch").val();
                if (SearchObj != "") {
                    for (var i = 0; i < returnObject.length; i++) {

                        var obj = {};
                        var NewCode = returnObject[i]["LedgerCode"];
                        var NewName = returnObject[i]["LedgerName"];
                        if (NewCode.toUpperCase().includes(SearchObj.toUpperCase()) || NewName.toUpperCase().includes(SearchObj.toUpperCase())) {
                            NewCode = NewCode.toUpperCase().replace(SearchObj.toUpperCase(), '<mark><b>' + SearchObj.toUpperCase() + '</b></mark>');
                            NewName = NewName.toUpperCase().replace(SearchObj.toUpperCase(), '<mark><b>' + SearchObj.toUpperCase() + '</b></mark>');

                            obj.LedgerCode = NewCode;
                            obj.LedgerName = NewName;
                            newobj.push(obj);

                        }



                    }
                    MainAccountTable.innerHTML = "<table border='1' width=\"100%\"><tr class=\"HeaderStyle\"><th>Ledger Code</th><th>Ledger Name</th></tr><table>";
                    MainAccountTable.innerHTML = MakeTableFromArray(newobj);
                }
                else {
                    MainAccountTable.innerHTML = "<table border='1' width=\"100%\"><tr class=\"HeaderStyle\"><th>Ledger Code</th><th>Ledger Name</th></tr><table>";
                    MainAccountTable.innerHTML = MakeTableFromArray(returnObject);
                }
            }

        }


        function View_LedgerMap(Id) {


            var obj = {};
            obj.id = Id;
            var CheckUniqueCodee = false;
            $.ajax({
                type: "POST",
                url: "BalanceSheetSchedule.aspx/ShowLedger",
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    returnObject = msg.d;



                    MainAccountTable.innerHTML = MakeTableFromArray(returnObject);



                    //console.log(returnObject);
                }
            });

            $("#LedgerModel").modal('show');
        }



        function MakeTableFromArray(myObj) {
            var txt = '';
            var count = 0;
            txt = "<table border='1' width='100%' class='dynamicPopupTbl'><tr class='HeaderStyle'><th>Main Account Name</th><th>Short Name</th></tr>";
            for (x in myObj) {
                txt += "<tr>";
                var PropertyCount = 0;

                for (key in myObj[0]) {
                    txt += "<td style='background-color: #FADBD8;'>" + myObj[x][key] + "</td>"
                    PropertyCount++;
                }
                txt += "</tr>";
                count++;
            }
            txt += "</table>"

            return txt;
        }





        var Row_id, Row_Type;
        function bsgrid_Customclick(s, e) {
            if (e.buttonID == 'CustomBtnLedgerMap') {

                Row_id = cbsgrid.GetDataRow(e.visibleIndex).children[0].innerHTML;
                Row_Type = cbsgrid.GetDataRow(e.visibleIndex).children[2].innerHTML;
                cLedgerPopUp.Show();
                cLedgerGrid.PerformCallback('ShowDetails~' + Row_id + '~' + Row_Type);

            }
        }
        function LedgerPopUpClosing() {
            cLedgerGridLookUp.ClearFilter();
        }
        function LdgEnCall() {
            if (cLedgerGrid.cpAutoID != "") {
                if (cLedgerGrid.cpAutoID == "Sucsess") {
                    cLedgerPopUp.Hide();
                    jAlert("Ledger Mapped Successfully. Please click on save to keep the changes.");

                }
                else {
                    cLedgerPopUp.Hide();
                    jAlert("Please Try Again");
                }
                cLedgerGridLookUp.ClearFilter();

            }


        }

        function LedgerPopUpClosing() {
            cLedgerGridLookUp.ClearFilter();
        }
        function LdgEnCall() {
            if (cLedgerGrid.cpAutoID != "") {
                if (cLedgerGrid.cpAutoID == "Sucsess") {
                    cLedgerPopUp.Hide();
                    jAlert("Ledger Mapped Successfully. Please click on save to keep the changes.");
                    Row_id = '';
                    Row_Type = '';
                }
                else {
                    cLedgerPopUp.Hide();
                    jAlert("Please Try Again");
                }
                cLedgerGridLookUp.ClearFilter();

            }


        }
        function SaveMap() {

            cLedgerGrid.PerformCallback('SaveMap~' + Row_id + '~' + Row_Type);
        }
        function CloseLedgerWindow() {
            cLedgerPopUp.Hide();

        }
        function bsgrid_Endcallback() {
            if (cbsgrid.cpSuccess == "SucsessAdd") {

                var oldVal = ctxtSl.GetText();
                var NewSl = parseInt(oldVal) + 1;
                ctxtSl.SetText(NewSl.toString());
                jAlert('Record added. Please Click on Save to permanently keep the Record.');
            }
            else if (cbsgrid.cpSuccess == "DataSaved") {
                //setTimeout(function () {
                //    alert('Record Saved.');
                //}, 200
                //    )
               

                //setTimeout(function () { document.location.reload(); }, 600);

                jAlert('Record Saved.', 'Alert', function () {
                    document.location.reload();
                });

                //alert('Record Saved.');
                //document.location.reload();
                
            }
            else if (cbsgrid.cpSuccess == "NoDataFound") {
                jAlert('No Records Found.');
            }
            else if (cbsgrid.cpSuccess == "ParentNotFound") {
                jAlert('Please Create Upper Level to Create This Level.');
            }
            else if (cbsgrid.cpSuccess == "DuplicateRow") {
                jAlert('Duplicate Record Found.Can not Proceed.');
            }
            else if (cbsgrid.cpSuccess == "DataDeleted") {
                jAlert('Record deleted. Please Click on Save to permanently delete the Record.');
            }
            else if (cbsgrid.cpSuccess == 'FormulaDetected') {
                jAlert('Formula detected with this item. Please Delete Formula first to delete this item.');
            }
            else {

            }
        }


        ////For Formula /////
        function Assign_Formula(ID) {
            Row_id = ID;
            cFormulaPopupCallBack.PerformCallback("ShowDetails~" + Row_id);
            cFormulaPopUp.Show();
        }

        var ScheduleParentId;
        function Add_Schedule(ID) {
            ScheduleParentId=ID;
           cSchedulePopUpCallBack.PerformCallback("ShowDetails~"+ID);
           cSchedulePopUp.Show();
        }

        function SaveSchedule() {
            cSchedulePopUpCallBack.PerformCallback("SaveDetails~" + ScheduleParentId);
        }
        function cSchedulePopUpEndCallBack() {
            if (cSchedulePopUpCallBack.cpSuccess == 'SaveSuccess') {
                ScheduleParentId = 0;
                cSchedulePopUp.Hide();
            }
        }


        function AddSelectedItems() {
            MoveSelectedItems(lbAvailable, lbChoosen);
            UpdateButtonState();
        }
        function AddAllItems() {
            MoveAllItems(lbAvailable, lbChoosen);
            UpdateButtonState();
        }



        function MinusSelectedItems() {
            MoveMinusSelectedItems(lbAvailable, lbChoosen);
            UpdateButtonState();
        }
        function MinusAllItems() {
            MoveAllMinusItems(lbAvailable, lbChoosen);
            UpdateButtonState();
        }

        function RemoveSelectedItems() {
            MoveSelectedItems(lbChoosen, lbAvailable);
            UpdateButtonState();
        }
        function RemoveAllItems() {
            MoveAllItems(lbChoosen, lbAvailable);
            UpdateButtonState();
        }
        function MoveSelectedItems(srcListBox, dstListBox) {
            srcListBox.BeginUpdate();
            dstListBox.BeginUpdate();
            if (dstListBox == lbChoosen) {
                var items = srcListBox.GetSelectedItems();
                for (var i = items.length - 1; i >= 0; i = i - 1) {

                    if (dstListBox.GetItemCount() > 0) {
                        dstListBox.AddItem("+", "+" + i);
                    }
                    dstListBox.AddItem(items[i].text, items[i].value);


                    if (i != 0 && dstListBox.GetItemCount() == 0) {
                        dstListBox.AddItem("+", "+" + i);
                    }


                    


                    srcListBox.RemoveItem(items[i].index);
                }
            }
            else {
                //var items = srcListBox.GetSelectedItems();
                //for (var i = 0 ; i <= items.length - 1; i = i + 1) {
                //    if (items[i].text != "+" && items[i].text != "-") {
                //        dstListBox.AddItem(items[i].text, items[i].value);

                //    }

                //    if (srcListBox.GetItem(items[i].index - 1) != null) {
                //        if (srcListBox.GetItem(items[i].index - 1).text.trim() == "+" || srcListBox.GetItem(items[i].index - 1).text.trim() == "-") {
                //            srcListBox.RemoveItem(items[i].index - 1);
                //            srcListBox.RemoveItem(items[i].index - 1);
                //        }
                //        else {
                //            srcListBox.RemoveItem(items[i].index);
                //        }
                //    }
                //    else {
                //        srcListBox.RemoveItem(items[i].index);
                //    }
                //}
                var itemscount = srcListBox.GetSelectedItems().length;
                for (var i = 0 ; i < itemscount ; i++) {
                    var tempObj = srcListBox.GetSelectedItems();
                    if (srcListBox.GetItem(tempObj[0].index - 1) != null) {
                        if (srcListBox.GetItem(tempObj[0].index - 1).text.trim() == "+" || srcListBox.GetItem(tempObj[0].index - 1).text.trim() == "-") {
                            srcListBox.RemoveItem(tempObj[0].index - 1);
                                    }
                    }

                     tempObj = srcListBox.GetSelectedItems();
                    if (tempObj.length>0)
                        srcListBox.RemoveItem(tempObj[0].index);
                }

            }

            srcListBox.EndUpdate();
            dstListBox.EndUpdate();
        }

        function MoveMinusSelectedItems(srcListBox, dstListBox) {
            srcListBox.BeginUpdate();
            dstListBox.BeginUpdate();
            if (dstListBox == lbChoosen) {
                var items = srcListBox.GetSelectedItems();
                for (var i = items.length - 1; i >= 0; i = i - 1) {
                    if (dstListBox.GetItemCount() > 0) {
                        dstListBox.AddItem("-", "-" + i);
                    }
                    dstListBox.AddItem(items[i].text, items[i].value);


                    if (i != 0 && dstListBox.GetItemCount() == 0) {
                        dstListBox.AddItem("-", "-" + i);
                    }
                    srcListBox.RemoveItem(items[i].index);
                }
            }
            else {
                var items = srcListBox.GetSelectedItems();
                for (var i = items.length - 1; i >= 0; i = i - 1) {
                    if (items[i].text != "-") {
                        dstListBox.AddItem(items[i].text, items[i].value);

                    }
                    srcListBox.RemoveItem(items[i].index);
                }
            }

            srcListBox.EndUpdate();
            dstListBox.EndUpdate();
        }

        function MoveAllMinusItems(srcListBox, dstListBox) {
            srcListBox.BeginUpdate();
            var count = srcListBox.GetItemCount();
            for (var i = 0; i < count; i++) {
                var item = srcListBox.GetItem(i);
                dstListBox.AddItem(item.text, item.value);
            }
            srcListBox.EndUpdate();
            srcListBox.ClearItems();
        }

        function MoveAllItems(srcListBox, dstListBox) {
            srcListBox.BeginUpdate();
            var count = srcListBox.GetItemCount();
            for (var i = 0; i < count; i++) {
                var item = srcListBox.GetItem(i);
                if (item.text != "+" && item.text != "-") {
                    dstListBox.AddItem(item.text, item.value);
                }
            }
            srcListBox.EndUpdate();
            srcListBox.ClearItems();
        }
        function UpdateButtonState(s, e) {

            var arrindex = [];
            if (s && (s.GetItem(e.index).text.trim() == "+" || s.GetItem(e.index).text.trim() == "-")) {
                arrindex.push(e.index);
                s.UnselectIndices(arrindex);
                return;
            }

            btnMoveAllItemsToRight.SetEnabled(lbAvailable.GetItemCount() > 0);
            cbtnMoveAllItemsToRight.SetEnabled(lbAvailable.GetItemCount() > 0);
            btnMoveAllItemsToLeft.SetEnabled(lbChoosen.GetItemCount() > 0);
            btnMoveSelectedItemsToRight.SetEnabled(lbAvailable.GetSelectedItems().length > 0);
            cbtnMoveSelectedItemsToRight.SetEnabled(lbAvailable.GetSelectedItems().length > 0);
            btnMoveSelectedItemsToLeft.SetEnabled(lbChoosen.GetSelectedItems().length > 0);
        }

        function SaveFormula() {
            var lstitem = lbChoosen.items;

            var i = 0;
            var arr = [];
            var objarr = {};
            for (i = 0 ; i < lbChoosen.GetItemCount() ; i++) {
                objarr = new Object();

                //var item = lbChoosen.GetItem(i);
                //arr[i].itemValues = item.value;
                //arr[i].itemTexts = item.text;
                var item = lbChoosen.GetItem(i);
                objarr.ScheduleGroup_Id = item.value;
                objarr.ScheduleGroup_Name = item.text;
                arr.push(objarr);
            }
            var myJsonString = JSON.stringify(arr);
            $("#hdnChoosen").val(myJsonString);


            cFormulaPopupCallBack.PerformCallback("SaveDetails~" + Row_id);
        }

        function cFormulaPopupCallBack_EndCallBAck() {
            if (cFormulaPopupCallBack.cpSuccess != "") {
                if (cFormulaPopupCallBack.cpSuccess == "Sucsess") {

                    jAlert("Formula Mapped Successfully. Please click on save to keep the changes.");
                    cFormulaPopUp.Hide();
                    Row_id = '';
                    Row_Type = '';
                    lbChoosen.ClearItems();
                    lbAvailable.ClearItems();
                }


            }
            if (cFormulaPopupCallBack.cpBind != null) {
                var cpAvailable = cFormulaPopupCallBack.cpBind.split('~')[0];
                var cpChoosen = cFormulaPopupCallBack.cpBind.split('~')[1];
                var objcpAvailable = JSON.parse(cpAvailable);
                var objcpChoosen = JSON.parse(cpChoosen);

                if (objcpAvailable) {

                    SetDataSourceOnListBoxAvailable(lbAvailable, objcpAvailable);
                }
                if (objcpChoosen) {

                    SetDataSourceOnListBoxChoosen(lbChoosen, objcpChoosen);
                }


            }
        }
        function SetDataSourceOnListBoxAvailable(ControlObject, Source) {
            ControlObject.ClearItems();
            for (var count = 0; count < Source.length; count++) {
                ControlObject.AddItem(Source[count].Text, Source[count].ID);
            }
            //ControlObject.SetSelectedIndex(0);
        }
        function SetDataSourceOnListBoxChoosen(ControlObject, Source) {
            ControlObject.ClearItems();
            for (var count = 0; count < Source.length; count++) { 
                ControlObject.AddItem(Source[count].TEXT, Source[count].SCHEDULE_ID);
            }
            //ControlObject.SetSelectedIndex(0);
        }

        function closeModal() {
            $("#LedgerModel").modal('hide');
        }





    </script>
    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />
    <style>
        .dxeTextBoxSys, .dxeButtonEditSys {
            width: auto !important;
        }

        .padtbl > tbody > tr > td {
            padding-right: 20px;
        }

        .padTop15 {
            padding-top: 15px;
        }

        .maskInput table > tbody > tr > td.dxeErrorCell_PlasticBlue {
            display: none;
        }

        #LayoutTable ul, #LayoutTable ol {
            list-style-type: none;
        }

            #LayoutTable ul > li > ul {
                border-left: 1px solid #ccc;
            }

                #LayoutTable ul > li > ul > li {
                    position: relative;
                    padding: 3px 0;
                }

                    #LayoutTable ul > li > ul > li:before {
                        content: '';
                        width: 28px;
                        height: 1px;
                        background: #cccccc;
                        position: absolute;
                        top: 14px;
                        left: -40px;
                    }


    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3>Balance Sheet - Schedule VI
            </h3>
        </div>
    </div>
        <div class="form_main">
        <dxe:ASPxCallbackPanel runat="server" ID="GroupCallBackPanel" ClientInstanceName="cGroupCallBackPanel">
            <PanelCollection>
                <dxe:PanelContent runat="server">
                    <div>
                        <table class="padtbl">
                            <tr>
                                <td style="width: 50px">

                                    <label>Sl#</label>
                                    <div class="relative maskInput">
                                        <dxe:ASPxTextBox runat="server" ID="txtSl" Text="1" ClientInstanceName="ctxtSl">
                                            <MaskSettings Mask="<0..999>" AllowMouseWheel="false" />
                                        </dxe:ASPxTextBox>
                                    </div>
                                </td>
                                <td style="width: 130px">
                                    <label>Type</label>
                                    <div>
                                        <dxe:ASPxComboBox runat="server" ID="cmbType" ClientInstanceName="ccmbType">
                                            <Items>
                                                <dxe:ListEditItem Text="Asset" Value="Asset" />
                                                <dxe:ListEditItem Text="Equity and Liability" Value="Equity and Liability" />
                                            </Items>
                                            <ClientSideEvents SelectedIndexChanged="cmbType_Change" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                </td>
                                <td>
                                    <label>Category</label>
                                    <div>
                                        <dxe:ASPxComboBox runat="server" ID="cmbCategory" ClientInstanceName="ccmbCategory"
                                            ClientSideEvents-SelectedIndexChanged="ccmbCategory_Change">
                                        </dxe:ASPxComboBox>
                                    </div>
                                </td>
                                <td>
                                    <label>Group</label>
                                    <div>
                                        <dxe:ASPxComboBox runat="server" ID="cmbGroup" ClientInstanceName="ccmbGroup"
                                            ClientSideEvents-SelectedIndexChanged="ccmbGroup_Change">
                                        </dxe:ASPxComboBox>
                                    </div>
                                </td>
                                <td>
                                    <label>Sub-Group 1</label>
                                    <div>
                                        <dxe:ASPxComboBox runat="server" ID="cmbSubGroup1" ClientInstanceName="ccmbSubGroup1"
                                            ClientSideEvents-SelectedIndexChanged="ccmbSubGroup1_Change">
                                        </dxe:ASPxComboBox>
                                    </div>
                                </td>
                                <td>
                                    <label>Sub-Group 2</label>
                                    <div>
                                        <dxe:ASPxComboBox runat="server" ID="cmbSubGroup2" ClientInstanceName="ccmbSubGroup2"
                                            ClientSideEvents-SelectedIndexChanged="ccmbSubGroup2_Change">
                                        </dxe:ASPxComboBox>
                                    </div>
                                </td>
                                <td>
                                    <label>Sub-Group 3</label>
                                    <div>
                                        <dxe:ASPxComboBox runat="server" ID="cmbSubGroup3" ClientInstanceName="ccmbSubGroup3"></dxe:ASPxComboBox>
                                    </div>
                                </td>
                                <td style="width: 218px">
                                    <div class="padTop15">
                                        <button id="btnAdd" type="button" class="btn btn-primary" title="Click to Add and then click on 'Save'." onclick="AddData();"><u>A</u>dd</button>
                                        <button type="button" id="btnSave" class="btn btn-success" onclick="SaveData();"><u>S</u>ave</button>
                                        <button type="button" id="btnView" class="btn btn-info" onclick="ViewData();"><u>P</u>review</button>
                                    </div>
                                </td>

                            </tr>
                        </table>
                    </div>
                    <div class="container">
                    </div>




                </dxe:PanelContent>
            </PanelCollection>

        </dxe:ASPxCallbackPanel>
        <div class="clearfix"></div>
        <div style="margin-top: 15px;">
            <div class="row">
                <div class="col-sm-12">
                    <div class="relative">
                        <dxe:ASPxGridView runat="server" ID="bsgrid" Width="100%" ClientInstanceName="cbsgrid"
                            ClientSideEvents-CustomButtonClick="bsgrid_Customclick" KeyFieldName="ID" OnCustomCallback="bsgrid_CustomCallback" OnDataBinding="bsgrid_DataBinding"
                            ClientSideEvents-EndCallback="bsgrid_Endcallback" Settings-HorizontalScrollBarMode="Auto"
                             Settings-VerticalScrollableHeight="320" Settings-VerticalScrollBarMode="Auto">

                            <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="Control" />
                            <Styles>
                                <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
                                <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>
                                <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                                <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
                                <Footer CssClass="gridfooter"></Footer>
                            </Styles>
                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="ID" Width="0px" FixedStyle="Left">
                                    <Settings AllowGroup="False" AllowSort="False" AllowDragDrop="False" AllowAutoFilterTextInputTimer="True" />
                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="SL" Caption="Sl No." Width="40px" SortOrder="Ascending" FixedStyle="Left">
                                    <Settings AllowGroup="False" AllowSort="False" AllowDragDrop="False" AllowAutoFilterTextInputTimer="True" />
                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Type" Caption="Type" Width="200px" FixedStyle="Left">
                                    <Settings AllowGroup="False" AllowSort="False" AllowDragDrop="False" AllowAutoFilterTextInputTimer="True" />
                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                    <%-- <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy"></PropertiesTextEdit>--%>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="CAT_ID" Caption="Category" Width="0px" Visible="false" FixedStyle="Left">
                                    <Settings AllowGroup="False" AllowSort="False" AllowDragDrop="False" AllowAutoFilterTextInputTimer="True" />
                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="CAT_NAME" Width="250px" Caption="Category" FixedStyle="Left">
                                    <Settings AllowGroup="False" AllowSort="False" AllowDragDrop="False" AllowAutoFilterTextInputTimer="True" />
                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="GROUP_ID" Width="0px" Visible="false" FixedStyle="Left">
                                    <Settings AllowGroup="False" AllowSort="False" AllowDragDrop="False" AllowAutoFilterTextInputTimer="True" />
                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="GROUP_NAME" Caption="Group Name" Width="250px" Settings-AllowAutoFilter="False" FixedStyle="Left">
                                    <Settings AllowGroup="False" AllowSort="False" AllowDragDrop="False" AllowAutoFilterTextInputTimer="True" />
                                    <CellStyle CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="SUBGROUP1_ID" Width="0px" Visible="false">
                                    <Settings AllowGroup="False" AllowSort="False" AllowDragDrop="False" AllowAutoFilterTextInputTimer="True" />
                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="SUBGROUP1_NAME" Width="250px" Caption="Subgroup 1">
                                    <Settings AllowGroup="False" AllowSort="False" AllowDragDrop="False" AllowAutoFilterTextInputTimer="True" />
                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Visible="False" FieldName="SUBGROUP2_ID">
                                    <Settings AllowGroup="False" AllowSort="False" AllowDragDrop="False" AllowAutoFilterTextInputTimer="True" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="SUBGROUP2_NAME" Width="250px" Caption="Subgroup 2">
                                    <Settings AllowGroup="False" AllowSort="False" AllowDragDrop="False" AllowAutoFilterTextInputTimer="True" />
                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Visible="False" Width="0px" FieldName="SUBGROUP3_ID">
                                    <Settings AllowGroup="False" AllowSort="False" AllowDragDrop="False" AllowAutoFilterTextInputTimer="True" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="SUBGROUP3_NAME" Width="250px" Caption="Subgroup 3">
                                    <Settings AllowGroup="False" AllowSort="False" AllowDragDrop="False" AllowAutoFilterTextInputTimer="True" />
                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Schedule_Number" Width="100px" Caption="Schedule No.">
                                    <Settings AllowGroup="False" AllowSort="False" AllowDragDrop="False" AllowAutoFilterTextInputTimer="True" />
                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="100">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" title="Click here to view ledger map." onclick="View_LedgerMap('<%# Container.KeyValue%>');" style='<%#Eval("Ledger_Visible")%>'>View Ledger
                                            <%--<img src="../../../assests/images/Delete.png" />--%>
                                        </a>
                                    </DataItemTemplate>

                                    <HeaderTemplate>Ledger Map</HeaderTemplate>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                    <Settings AllowAutoFilterTextInputTimer="False" />

                                </dxe:GridViewDataTextColumn>





                                <dxe:GridViewDataTextColumn CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="0">
                                    <DataItemTemplate>
                                        <div class='floatedBtnArea'>
                                            <a href="javascript:void(0);" onclick="Ledger_Map('<%# Eval("Type") %>', '<%# Container.KeyValue%>');" title="" class="" style='<%#Eval("Ledger_Visible")%>'>
                                                <span class='ico editColor'><i class='fa fa-map-o' aria-hidden='true'></i></span><span class='hidden-xs'>Map Ledger</span>
                                            </a>
                                            <a href="javascript:void(0);" title="Click on  Delete and then click on 'Save'." onclick="Delete_Row('<%# Container.KeyValue%>');" style='<%#Eval("Delete_Visible")%>'>
                                                <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span>
                                            </a>
                                            <a href="javascript:void(0);" onclick="Assign_Formula('<%# Container.KeyValue%>');" class="" title="" style='<%#Eval("isFormula")%>'>
                                                <span class='ico ColorSeven'><i class='fa fa-calculator'></i></span><span class='hidden-xs'>Define Formula</span>
                                            </a>
                                            <a href="javascript:void(0);" onclick="Add_Schedule('<%# Container.KeyValue%>');" class="" title="" style='<%#Eval("ISScheduleVisible")%>'>
                                                <span class='ico ColorFour'><i class='fa fa-calendar'></i></span><span class='hidden-xs'>Add Schedule</span>
                                            </a>
                                       </div>
                                    </DataItemTemplate>
                                    <HeaderTemplate></HeaderTemplate>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>



















                                <%--                                <dxe:GridViewCommandColumn Width="13%" ButtonType="Image" Caption="Actions" >
                                    <CustomButtons>
                                        <dxe:GridViewCommandColumnCustomButton ID="CustomBtnDelete" Image-ToolTip="Ledger Map" image-di >
                                            <Image Url="/assests/images/Delete.png"></Image>
                                        </dxe:GridViewCommandColumnCustomButton>
                                        <dxe:GridViewCommandColumnCustomButton ID="CustomBtnLedgerMap" Image-ToolTip="Delete">
                                            <Image Url="/assests/images/popuparrow.jpg"></Image>
                                        </dxe:GridViewCommandColumnCustomButton>
                                        
                                    </CustomButtons>
                                </dxe:GridViewCommandColumn>--%>
                            </Columns>
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsSearchPanel Visible="false" />
                            <ClientSideEvents RowClick="gridRowclick" />
                            <SettingsPager NumericButtonCount="10" PageSize="100" ShowSeparators="True" Mode="ShowPager">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </SettingsPager>
                        </dxe:ASPxGridView>

                    </div>
                </div>
            </div>
            <%--            <div class="row">
                <div class="col-sm-12 padTop15">
                    <button type="button" id="btnSave" class="btn btn-success" onclick="SaveData();">Save</button>
                </div>
            </div>--%>
        </div>
        <div>
            <dxe:ASPxPopupControl ID="LedgerPopUp" runat="server" ClientInstanceName="cLedgerPopUp"
                Width="600px" HeaderText="Ledger Map" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="TopSides" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True" Opacity="100">
                <ClientSideEvents Closing="function(s, e) {LedgerPopUpClosing();}" />
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <dxe:ASPxCallbackPanel runat="server" ID="LedgerPanel" ClientInstanceName="cLedgerGrid" OnCallback="LedgerPanel_Callback">
                            <ClientSideEvents EndCallback="function(s, e) {LdgEnCall();}" />
                            <PanelCollection>
                                <dxe:PanelContent runat="server">
                                    <dxe:ASPxGridView ID="LedgerGrid" runat="server" ClientInstanceName="cLedgerGridLookUp"
                                        KeyFieldName="Ledger_id" Width="100%" SelectionMode="Multiple" AutoGenerateColumns="False"
                                        OnDataBinding="LedgerGrid_DataBinding">
                                        <SettingsPager Mode="ShowPager">
                                        </SettingsPager>

                                        <SettingsPager PageSize="10">
                                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                        </SettingsPager>

                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                        <Columns>
                                            <dxe:GridViewCommandColumn ShowSelectCheckbox="true" VisibleIndex="0" Width="40" Caption="Select">
                                            </dxe:GridViewCommandColumn>
                                            <dxe:GridViewDataColumn FieldName="Ledger_id" Caption="Code" Width="100" Visible="false">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>
                                            <dxe:GridViewDataColumn FieldName="Ledger_Name" Caption="Ledger Code" Width="100">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>
                                            <dxe:GridViewDataColumn FieldName="Ledger_Code" Caption="Ledger Name" Width="180">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>
                                            <dxe:GridViewDataColumn FieldName="Group_Name" Visible="false" Caption="GroupCode" Width="80">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>

                                        </Columns>




                                    </dxe:ASPxGridView>

                                    <div>
                                        <dxe:ASPxButton ID="LedgerSave" ClientInstanceName="cbtnLedgerSave" runat="server" AutoPostBack="False" Text="Ok" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                            <ClientSideEvents Click="function(s, e) {return SaveMap(); }" />

                                        </dxe:ASPxButton>
                                        <dxe:ASPxButton ID="LedgerCancel" ClientInstanceName="cbtnLedgerCancel" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                            <ClientSideEvents Click="function(s, e) {return CloseLedgerWindow(); }" />

                                        </dxe:ASPxButton>
                                    </div>
                                </dxe:PanelContent>
                            </PanelCollection>
                        </dxe:ASPxCallbackPanel>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
            </dxe:ASPxPopupControl>
            <asp:HiddenField ID="CallStatus" runat="server" EnableViewState="true" />
            <asp:HiddenField ID="GroupId" runat="server" EnableViewState="true" />
            <asp:HiddenField ID="ParentId" runat="server" EnableViewState="true" />
            <asp:HiddenField ID="ItemList" runat="server" EnableViewState="true" />

        </div>



        <div>

            <div class="modal fade" id="LedgerModel" role="dialog">
                <div class="modal-dialog" style="height:400px;">

                    <!-- Modal content-->
                    <!-- Modal MainAccount-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" onclick="closeModal();">&times;</button>
                            <h4 class="modal-title">View Ledger</h4>
                        </div>
                        <div class="modal-body" style="height:400px;  overflow-y: auto;">
                            <input type="text" onkeydown="LedgerNewkeydown(event)" id="txtLedgerSearch" autofocus width="100%" placeholder="Search by Ledger Name or Ledger Code" />

                            <div id="MainAccountTable">
                                <table border="1" width="100%" class="table table-hover">
                                    <tr class="HeaderStyle">
                                        <th>Ledger Code</th>
                                        <th>Ledger Name</th>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" onclick="closeModal();">Close</button>
                        </div>
                    </div>
                </div>
            </div>

        </div>

</div>
    </div>


<div class="modal fade" id="ViewLayoutModel" role="dialog">
                    <!-- Modal content-->
  <div class="modal-dialog" style="">                  <!-- Modal MainAccount-->
        <div class="modal-content">
            <div class="modal-header" style="height: 22px; !important;">
                <button type="button" class="close" onclick="ViewLayoutModelClose();">&times;</button>
                <h4 class="modal-title">Balance Sheet - Schedule VI Preview</h4>
            </div>
            <div class="modal-body" style="height: 500px; overflow-y: auto;">
                <div id="LayoutTable">
                    <table width="100%">
                        <tr class="HeaderStyle">
                            <th></th>

                        </tr>
                    </table>
                </div>
            </div>

        </div>
   </div>
</div>


















        <div>
            <dxe:ASPxPopupControl ID="FormulaPopUp" runat="server" ClientInstanceName="cFormulaPopUp"
                Width="800px" Height="500px" HeaderText="Define Formula" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True" Opacity="100">

                <ContentStyle VerticalAlign="Top"></ContentStyle>

                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <dxe:ASPxCallbackPanel runat="server" ID="FormulaPopupCallBack" ClientSideEvents-EndCallback="cFormulaPopupCallBack_EndCallBAck" ClientInstanceName="cFormulaPopupCallBack" OnCallback="FormulaPopup_Callback">
                            <ClientSideEvents EndCallback="cFormulaPopupCallBack_EndCallBAck"></ClientSideEvents>
                            <PanelCollection>
                                <dxe:PanelContent runat="server">
                                    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
                                        <ClientSideEvents ControlsInitialized="function(s, e) { UpdateButtonState(); }" />
                                    </dxe:ASPxGlobalEvents>
                                    <div class="container horizontal-center-aligned">
                                        <div class="contentEditors">
                                            <dxe:ASPxListBox ID="lbAvailable" runat="server" ClientInstanceName="lbAvailable"
                                                Width="285" Height="400px" SelectionMode="CheckColumn" Caption="Available" ValueField="system.string" EnableSynchronization="True">
                                                <CaptionSettings Position="Top" />
                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonState(); }" />
                                            </dxe:ASPxListBox>
                                        </div>
                                        <div class="contentButtons">
                                            <div>
                                                <dxe:ASPxButton ID="btnMoveSelectedItemsToRight" runat="server" ClientInstanceName="btnMoveSelectedItemsToRight" CssClass="button"
                                                    AutoPostBack="False" Text="Add >" ClientEnabled="False"
                                                    ToolTip="Add selected items">
                                                    <ClientSideEvents Click="function(s, e) { AddSelectedItems(); }" />
                                                </dxe:ASPxButton>
                                            </div>
                                            <div class="TopPadding" style="visibility: hidden">
                                                <dxe:ASPxButton ID="btnMoveAllItemsToRight" runat="server" ClientInstanceName="btnMoveAllItemsToRight" CssClass="button"
                                                    AutoPostBack="False" Text="Add All >>" ToolTip="Add all items">
                                                    <ClientSideEvents Click="function(s, e) { AddAllItems(); }" />
                                                </dxe:ASPxButton>
                                            </div>
                                            <div style="height: 32px">
                                            </div>

                                            <div>
                                                <dxe:ASPxButton ID="ASPxbtnMoveSelectedItemsToRight" runat="server" ClientInstanceName="cbtnMoveSelectedItemsToRight" CssClass="button"
                                                    AutoPostBack="False" Text="Minus >" ClientEnabled="False"
                                                    ToolTip="Add selected items">
                                                    <ClientSideEvents Click="function(s, e) { MinusSelectedItems(); }" />
                                                </dxe:ASPxButton>
                                            </div>
                                            <div class="TopPadding" style="visibility: hidden">
                                                <dxe:ASPxButton ID="ASPxbtnMoveAllItemsToRight" runat="server" ClientInstanceName="cbtnMoveAllItemsToRight" CssClass="button"
                                                    AutoPostBack="False" Text="Minus All >>" ToolTip="Add all items">
                                                    <ClientSideEvents Click="function(s, e) { MinusAllItems(); }" />
                                                </dxe:ASPxButton>
                                            </div>
                                            <div style="height: 32px">
                                            </div>




                                            <div>
                                                <dxe:ASPxButton ID="btnMoveSelectedItemsToLeft" runat="server" ClientInstanceName="btnMoveSelectedItemsToLeft" CssClass="button"
                                                    AutoPostBack="False" Text="< Remove" ClientEnabled="False"
                                                    ToolTip="Remove selected items">
                                                    <ClientSideEvents Click="function(s, e) { RemoveSelectedItems(); }" />
                                                </dxe:ASPxButton>
                                            </div>
                                            <div class="TopPadding" style="visibility: hidden">
                                                <dxe:ASPxButton ID="btnMoveAllItemsToLeft" runat="server" ClientInstanceName="btnMoveAllItemsToLeft" CssClass="button"
                                                    AutoPostBack="False" Text="<< Remove All" ClientEnabled="False"
                                                    ToolTip="Remove all items">
                                                    <ClientSideEvents Click="function(s, e) { RemoveAllItems(); }" />
                                                </dxe:ASPxButton>
                                            </div>
                                        </div>

                                        <div class="contentEditors">
                                            <dxe:ASPxListBox ID="lbChoosen" runat="server" ClientInstanceName="lbChoosen" Width="285" ValueField="system.string"  EnableViewState="true" EnableSynchronization="False"
                                                Height="400px" SelectionMode="CheckColumn"  Caption="Definition">
                                                <CaptionSettings Position="Top" />
                                                <ClientSideEvents  SelectedIndexChanged="function(s, e) { UpdateButtonState(s,e); }"></ClientSideEvents>
                                            </dxe:ASPxListBox>
                                            <asp:HiddenField runat="server" ID="hdnChoosen" />
                                        </div>
                                    </div>

                                    <div class="col-md-12 mTop5">
                                        <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtnFormulaSave" runat="server" AutoPostBack="False" Text="Ok" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                            <ClientSideEvents Click="function(s, e) {return SaveFormula(); }" />

                                        </dxe:ASPxButton>
                                        <dxe:ASPxButton ID="ASPxButton2" ClientInstanceName="cbtnLedgerCancel" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                            <ClientSideEvents Click="function(s, e) {return CloseFormulaWindow(); }" />

                                        </dxe:ASPxButton>
                                    </div>
                                </dxe:PanelContent>
                            </PanelCollection>
                        </dxe:ASPxCallbackPanel>

                    </dxe:PopupControlContentControl>
                </ContentCollection>
            </dxe:ASPxPopupControl>
            <asp:HiddenField ID="HiddenField1" runat="server" EnableViewState="true" />
            <asp:HiddenField ID="HiddenField2" runat="server" EnableViewState="true" />
            <asp:HiddenField ID="HiddenField3" runat="server" EnableViewState="true" />
            <asp:HiddenField ID="HiddenField4" runat="server" EnableViewState="true" />

        </div>




<div>
            <dxe:ASPxPopupControl ID="SchedulePopUp" runat="server" ClientInstanceName="cSchedulePopUp"
                Width="200px" HeaderText="Schedule Number" PopupHorizontalAlign="WindowCenter" 
                PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True" Opacity="100">
               
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <dxe:ASPxCallbackPanel runat="server" ID="SchedulePopUpCallBack" ClientInstanceName="cSchedulePopUpCallBack" OnCallback="SchedulePopUpCallBack_Callback" ClientSideEvents-EndCallback="cSchedulePopUpEndCallBack">
                            
                            <PanelCollection>
                                <dxe:PanelContent runat="server">
                                   <div>
                                       <dxe:ASPxTextBox runat="server" ID="txtSchedule" MaxLength="10" ClientInstanceName="ctxtSchedule"></dxe:ASPxTextBox>
                                   </div>

                                    <div>
                                        <dxe:ASPxButton ID="btnScheduleSave" ClientInstanceName="cbtnLedgerSave" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                            <ClientSideEvents Click="function(s, e) {return SaveSchedule(); }" />

                                        </dxe:ASPxButton>
                                        <dxe:ASPxButton ID="btnScheduleClose" ClientInstanceName="cbtnLedgerCancel" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                            <ClientSideEvents Click="function(s, e) {return cSchedulePopUp.Hide(); }" />

                                        </dxe:ASPxButton>
                                    </div>
                                </dxe:PanelContent>
                            </PanelCollection>
                        </dxe:ASPxCallbackPanel>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
            </dxe:ASPxPopupControl>
            <asp:HiddenField ID="HiddenField5" runat="server" EnableViewState="true" />
            <asp:HiddenField ID="HiddenField6" runat="server" EnableViewState="true" />
            <asp:HiddenField ID="HiddenField7" runat="server" EnableViewState="true" />
            <asp:HiddenField ID="HiddenField8" runat="server" EnableViewState="true" />

        </div>










    <div><div>
     <asp:Button ID="drdCashBank" runat="server" Text="Export to Excel" CssClass="btn btn-sm btn-primary expad" OnClick="drdCashBankExport_SelectedIndexChanged" AutoPostBack="true">
     </asp:Button>
     <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    </div>
    </div>
</asp:Content>
