<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                22-03-2023        2.0.36           Pallab              25733 : Master pages design modification
2.0                16-05-2023        2.0.38           Priti               0025893 : Import Module Required for Importing Ledger/Subledger Opening
3.0                30-05-2023        2.0.39           Priti               0026462: Unable to enter Ledger Opening
====================================================== Revision History =============================================--%>

<%@ Page Title="Opening Balance" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/Erp.Master"
    Inherits="ERP.OMS.Management.Master.management_master_frm_ActOpeningBalance" CodeBehind="frm_ActOpeningBalance.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        var status = "Add";
        var ReturnData = "";
        $(document).ready(function () {
            finalSaveButtonState();

            $('#SubAct').on('shown.bs.modal', function () {
                clearPopup();
                $('#txtSubActSearch').focus();
            })

        });

        function clearPopup() {// Arindam mantis id:0019467

            var rowsArr = $('.dynamicPopupTbl')[0].rows;
            var len = rowsArr.length;
            while (rowsArr.length > 1) {
                rowsArr[rowsArr.length - 1].remove();
            }

            $('#txtSubActSearch').val('');

        }

        function BranchIndexChange(s, e) {
            //  ccmbAccount.PerformCallback(s.GetValue());
            //  ccmbSubAccount.PerformCallback('');
        }

        function AccountIndexChange(s, e) {

            // ccmbSubAccount.PerformCallback(s.GetValue());
            if (s.GetValue() != null) {
                ctxtSubName.SetText("");
                $('#hdSubAccountId').val("0");
                cAmountPanel.PerformCallback("MainAccount~" + s.GetValue() + "~" + cmbBranch.GetValue());
            }
        }
        function SubAccountIndexChange(s, e) {
            cAmountPanel.PerformCallback("SubMainAccount~" + ccmbAccount.GetValue() + "~" + s.GetValue() + "~" + cmbBranch.GetValue());
        }

        function saveClientClick(s, e) {
            if (validAllData()) {
                cOpeningGrid.PerformCallback(status);
                status = "Add";
                SetEnable(true);
                ClearClientClick();
            }
        }
        function OpeningGridEndCallBack(s, e) {
            if (cOpeningGrid.cpTotalAmount) {
                ctotalDebit.SetText(cOpeningGrid.cpTotalAmount.split('~')[1]);
                clblTotDebit.SetText(cOpeningGrid.cpTotalAmount.split('~')[1]);
                totalCredit.SetText(cOpeningGrid.cpTotalAmount.split('~')[3]);
                clblTotCredit.SetText(cOpeningGrid.cpTotalAmount.split('~')[3]);
                cOpeningGrid.cpTotalAmount = null;
            }
            if (status == "Edit") {
                if (cOpeningGrid.cpBeforeEdit) {
                    if (cOpeningGrid.cpBeforeEdit != "") {
                        ReturnData = cOpeningGrid.cpBeforeEdit;
                        cmbBranch.SetValue(ReturnData.split('~')[1]);
                        ccmbAccount.PerformCallback(cmbBranch.GetValue());
                        cOpeningGrid.cpBeforeEdit = null;
                    }
                }
            }

            if (cOpeningGrid.cpClientMsg) {
                if (cOpeningGrid.cpClientMsg != "") {
                    jAlert(cOpeningGrid.cpClientMsg);
                    cOpeningGrid.cpClientMsg = null;
                }
            }

            finalSaveButtonState();
            //  cmbBranch.Focus();
        }

        function finalSaveButtonState() {
            if (parseFloat(ctotalDebit.GetValue()) == parseFloat(totalCredit.GetValue())) {
                cFinalSave.SetVisible(true);
                $('#ErrorText').hide();
            }
            else {
                // cFinalSave.SetVisible(false);
                // $('#ErrorText').show();
                var totcredit = parseFloat(clblTotCredit.GetText());
                var totDebit = parseFloat(clblTotDebit.GetText());

                var totalDiff = totcredit - totDebit;
                cdiffAmt.SetText(parseFloat(Math.round(Math.abs(totalDiff) * 100) / 100).toFixed(2));



            }
        }

        function onOpeningEdit(obj) {
            status = "Edit";
            SetEnable(false);
            cOpeningGrid.PerformCallback(status + "~" + obj);
        }

        function SetEnable(state) {
            cmbBranch.SetEnabled(state);
            ccmbAccount.SetEnabled(state);
            //  ccmbSubAccount.SetEnabled(state);
            ctxtSubName.SetEnabled(state);
        }

        function cmbAccountEndCallBack(s, e) {
            if (status == "Edit") {
                ccmbAccount.SetValue(ReturnData.split('~')[2]);
                // ccmbSubAccount.PerformCallback(ccmbAccount.GetValue());

                $('#hdSubAccountId').val(ReturnData.split('~')[3]);
                ccmbDrorCr.SetValue(ReturnData.split('~')[4]);
                cAmmount.SetValue(ReturnData.split('~')[5]);
                ctxtSubName.SetText(ReturnData.split('~')[6]);
                status = "EditDone~" + ReturnData.split('~')[0];
                ccmbDrorCr.Focus();
                cOpeningGrid.Refresh();
            }
        }
        function cmbSubAccountEndCallBack(s, e) {
            if (status == "Edit") {
                ccmbSubAccount.SetValue(ReturnData.split('~')[3]);
                ccmbDrorCr.SetValue(ReturnData.split('~')[4]);
                cAmmount.SetValue(ReturnData.split('~')[5]);
                status = "EditDone~" + ReturnData.split('~')[0];
                ccmbDrorCr.Focus();
                cOpeningGrid.Refresh();
            }
        }
        function FinalSaveClientClick(s, e) {
            cOpeningGrid.PerformCallback('SaveAllRecord');
        }
        function ClearClientClick(s, e) {
            $('#MandatoryBranch').css({ 'display': 'none' });
            $('#MandatoryAccount').css({ 'display': 'none' });
            cmbBranch.SetSelectedIndex(-1);
            ccmbAccount.PerformCallback('0');
            //ccmbSubAccount.PerformCallback('0');
            ctxtSubName.SetText("");
            $('#hdSubAccountId').val("0");
            cAmmount.SetValue(0);
            status = "Add";
            SetEnable(true);
            cmbBranch.Focus();
        }
        function validAllData() {
            var retdata = true;
            $('#MandatoryBranch').css({ 'display': 'none' });
            $('#MandatoryAccount').css({ 'display': 'none' });

            if (cmbBranch.GetValue() == null) {
                $('#MandatoryBranch').css({ 'display': 'block' });
                retdata = false;
            }
            if (ccmbAccount.GetValue() == null) {
                $('#MandatoryAccount').css({ 'display': 'block' });
                retdata = false;
            }
            return retdata;

        }

        function AmountPanelEndCallBack(s, e) {
            //  alert(cAmountPanel.cpAmount);
            if (cAmountPanel.cpAmount) {
                var panelRetData = cAmountPanel.cpAmount;
                ccmbDrorCr.SetValue(panelRetData.split('~')[0]);
                cAmmount.SetValue(panelRetData.split('~')[1]);
            }
        }


        $(function () {
            var vAnotherKeyWasPressed = false;
            var ALT_CODE = 18;

            $(window).keydown(function (event) {
                var vKey = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
                vAnotherKeyWasPressed = vKey != ALT_CODE;
                if (event.altKey && (event.key == 's' || event.key == 'S')) {
                    if (parseFloat(ctotalDebit.GetValue()) == parseFloat(totalCredit.GetValue())) {
                        if (cFinalSave) {
                            FinalSaveClientClick();
                            return false;
                        }
                    }
                }

            });

            $(window).keyup(function (event) {

                var vKey = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;

            });
        });


        function SubActkeydown(e) {

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtSubActSearch").val();
            OtherDetails.MainAccountCode = ccmbAccount.GetValue();
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Sub Account Name");
                HeaderCaption.push("Type");
                if ($("#txtCustSearch").val() != '') {
                    callonServer("../Activities/Services/Master.asmx/GetSubAccount", OtherDetails, "SubActTable", HeaderCaption, "SubActIndex", "SetSubAct");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[SubActIndex=0]"))
                    $("input[SubActIndex=0]").focus();
            }

        }


        function SetSubAct(Id, Name) {
            if (Id) {
                $('#SubAct').modal('hide');
                ctxtSubName.SetText(Name);
                $('#hdSubAccountId').val(Id);
                cAmountPanel.PerformCallback("SubMainAccount~" + ccmbAccount.GetValue() + "~" + Id + "~" + cmbBranch.GetValue());
                ccmbDrorCr.Focus();
            }
        }

        function SubActButnClick(s, e) {
            if (ccmbAccount.GetText() != "") {
                $('#SubAct').modal('show');
                $(".dynamicPopupTbl tr").remove();
                document.getElementById("SubActTable").innerHTML = "<table border='1' width='100%' class='dynamicPopupTbl'> <tr class='HeaderStyle'><th>Sub Account Name</th><th>Type</th></tr> </table>";
            }
            else
                jAlert("Please Select a Main Account.");
        }


        function subActKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
                $(".dynamicPopupTbl tr").remove();
                document.getElementById("SubActTable").innerHTML = "<table border='1' width='100%' class='dynamicPopupTbl'> <tr class='HeaderStyle'><th>Sub Account Name</th><th>Type</th></tr> </table>";
                if (ccmbAccount.GetText() != "")
                    $('#SubAct').modal('show');
                else
                    jAlert("Please Select a Main Account.");
            }
        }

        function ValueSelected(e, SubActIndex) {
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var Id = e.target.parentElement.parentElement.cells[0].innerText;
                var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                if (Id) {
                    SetSubAct(Id, name);
                }

            }
            else if (e.code == "ArrowDown") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex++;
                if (thisindex < 10)
                    $("input[" + indexName + "=" + thisindex + "]").focus();
            }
            else if (e.code == "ArrowUp") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex--;
                if (thisindex > -1)
                    $("input[" + indexName + "=" + thisindex + "]").focus();
                else {
                    $('#txtSubActSearch').focus();

                }
            }

        }
    </script>
    <script src="../Activities/JS/SearchPopup.js"></script>
    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />
    <style>
        .dxeErrorFrameSys.dxeErrorCellSys,
        .pullleftClass {
            position: absolute;
            right: 6px;
            top: 8px;
        }

        .lead {
            font-size: 18px;
        }

        #diffAmt {
        }

        #OpeningGrid, #OpeningGrid .dxgvHSDC > div,
        #OpeningGrid .dxgvHSDC + div {
            width: 100% !important;
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
    <script>
        function gridRowclick(s, e) {
            $('#OpeningGrid').find('tr').removeClass('rowActive');
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
     <%-- Rev 2.0--%>
     <script type="text/javascript">

         function ImportUpdatePopOpen(e) {
             $("#modalimport").modal('show');
         }
         function ViewLogData() {
             cGvLogSearch.Refresh();
         }
         function ShowLogData(haslog) {
             ;
             $('#btnViewLog').click();
         }
     </script>
  <%--  Rev 2.0 End--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">Opening Balance  </h3>
            <div id="pageheaderContent" class=" pull-left wrapHolder content horizontal-images" style="">
                <ul>
                    <li>
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Total Debit</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dxe:ASPxLabel ID="lblTotDebit" runat="server" Text="ASPxLabel" ClientInstanceName="clblTotDebit"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Total Credit</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dxe:ASPxLabel ID="lblTotCredit" runat="server" Text="ASPxLabel" ClientInstanceName="clblTotCredit"></dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
        <div class="crossBtn"><a href="../ProjectMainPage.aspx"><i class="fa fa-times"></i></a></div>
    </div>
        <div class="form_main">


        <table width="100%">
            <tr>
                <td>Branch</td>
                <td>Account</td>
                <td style="140px">Sub Account</td>
                <td>DR/CR</td>
                <td style="width:80px">Amount</td>
                <td style="width: 300px"></td>
            </tr>
            <tr>

                <td style="padding-right: 25px" class="relative">
                    <dxe:ASPxComboBox ID="cmbBranch" ClientInstanceName="cmbBranch" runat="server" DataSourceID="dsBranch" Width="100%"
                        ValueType="System.String" AutoPostBack="false" ValueField="BANKBRANCH_ID" TextField="BANKBRANCH_NAME"
                        EnableIncrementalFiltering="true" EnableSynchronization="False">
                        <ClientSideEvents SelectedIndexChanged="BranchIndexChange" />
                    </dxe:ASPxComboBox>
                    <span id="MandatoryBranch" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; display: none; padding-left: 9px;" title="Mandatory"></span>
                </td>


                <td style="padding-right: 15px" class="relative">
                    <dxe:ASPxComboBox ID="cmbAccount" ClientInstanceName="ccmbAccount" runat="server" Width="100%" OnCallback="cmbAccount_CustomCallback" ValueType="System.String" AutoPostBack="false">
                        <ClientSideEvents SelectedIndexChanged="AccountIndexChange" EndCallback="cmbAccountEndCallBack" />
                    </dxe:ASPxComboBox>
                    <span id="MandatoryAccount" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; display: none; padding-left: 9px;" title="Mandatory"></span>
                </td>


                <td style="padding-right:15px" class="relative">
                    <%-- <dxe:ASPxComboBox ID="cmbSubAccount" ClientInstanceName="ccmbSubAccount" runat="server" Width="100%"  ValueType="System.String" AutoPostBack="false" OnCallback="cmbSubAccount_CustomCallback"   EnableCallbackMode="true"  > 
                         <ClientSideEvents SelectedIndexChanged="SubAccountIndexChange"  EndCallback="cmbSubAccountEndCallBack" />                        
                           </dxe:ASPxComboBox>--%>

                    <dxe:ASPxButtonEdit ID="txtSubName" runat="server" ReadOnly="true" ClientInstanceName="ctxtSubName" Width="100%">

                        <Buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>

                        </Buttons>
                        <ClientSideEvents ButtonClick="function(s,e){SubActButnClick();}" KeyDown="function(s,e){subActKeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>


                </td>

                <td style="padding-right: 15px" class="relative">
                    <dxe:ASPxComboBox ID="cmbDrorCr" ClientInstanceName="ccmbDrorCr" runat="server" Width="100%"
                        ValueType="System.String" AutoPostBack="false" SelectedIndex="0">
                        <Items>
                            <dxe:ListEditItem Text="Debit" Value="D" />
                            <dxe:ListEditItem Text="Credit" Value="C" />
                        </Items>
                    </dxe:ASPxComboBox>
                </td>

                <td style="padding-right: 15px; vertical-align: top; padding-top: 3px; width: 100px" class="relative">
                    <dxe:ASPxCallbackPanel runat="server" ID="AmountPanel" ClientInstanceName="cAmountPanel" OnCallback="AmountPanel_Callback" Width="100%">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxTextBox ID="txtAmmount" runat="server" Width="100%" ClientInstanceName="cAmmount">
                                    <MaskSettings Mask="<0..99999999999g>.<00..99>" IncludeLiterals="DecimalSymbol" AllowMouseWheel="false"
                                        ErrorText="None" />
                                </dxe:ASPxTextBox>
                            </dxe:PanelContent>
                        </PanelCollection>
                        <ClientSideEvents EndCallback="AmountPanelEndCallBack" />
                    </dxe:ASPxCallbackPanel>

                </td>
                <td class="relative" >
                    <dxe:ASPxButton ID="Button1" runat="server" AutoPostBack="false" CssClass="btn btn-success btn-radius" Text="Add to Row" ClientInstanceName="cButton1" UseSubmitBehavior="False"
                        VerticalAlign="Bottom">
                        <ClientSideEvents Click="saveClientClick" />
                    </dxe:ASPxButton>
                    <dxe:ASPxButton ID="ASPxButton1" runat="server" CssClass="btn btn-info btn-radius" Text="Clear" VerticalAlign="Bottom" AutoPostBack="false" UseSubmitBehavior="False">
                        <ClientSideEvents Click="ClearClientClick" />
                    </dxe:ASPxButton>

                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>

                </td>

            </tr>
            <tr>
                 <%-- Rev 2.0--%>
                <td><asp:LinkButton ID="lnlDownloaderexcel" runat="server" OnClick="lnlDownloaderexcel_Click" CssClass="btn btn-info btn-radius pull-rigth mBot0">Download Format</asp:LinkButton></td>
                <td><button type="button" onclick="ImportUpdatePopOpen();" class="btn btn-primary btn-radius">Import(Add/Update)</button></td>
                <td><button type="button" class="btn btn-warning btn-radius" data-toggle="modal" data-target="#modalSS" id="btnViewLog" onclick="ViewLogData();">View Log</button></td>
               <%-- Rev 2.0 End--%>
            </tr>

        </table>


        <div class="GridViewArea relative">
            <dxe:ASPxGridView ID="OpeningGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="cOpeningGrid" SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight="350"
                Width="100%" OnCustomCallback="OpeningGrid_CustomCallback" OnDataBinding="grid_DataBinding" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
                <SettingsSearchPanel Visible="True" Delay="5000"  />

                <Settings ShowGroupPanel="false" ShowFooter="true" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu="True" />
                <Columns>

                    <dxe:GridViewDataTextColumn Caption="Branch" FieldName="Branch" ReadOnly="True" Visible="True" FixedStyle="Left" VisibleIndex="0">
                        <Settings AutoFilterCondition="Contains" />
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Account" FieldName="Account" ReadOnly="True" Visible="True" FixedStyle="Left" VisibleIndex="0">
                        <Settings AutoFilterCondition="Contains" />
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Sub Account" FieldName="SubAccount" ReadOnly="True" Visible="True" FixedStyle="Left" VisibleIndex="0">
                        <Settings AutoFilterCondition="Contains" />
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Debit" FieldName="DebitAmount" ReadOnly="True" Visible="True" FixedStyle="Left" VisibleIndex="0">
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                        <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                        <PropertiesTextEdit DisplayFormatString="{0:0.00}">
                            <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                        </PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Credit" FieldName="CreditAmount" ReadOnly="True" Visible="True" FixedStyle="Left" VisibleIndex="0">
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                        <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                        <PropertiesTextEdit DisplayFormatString="{0:0.00}">
                            <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                        </PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>

                     <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="CreateUser" 
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Entered On" FieldName="CreateDate" 
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                     <dxe:GridViewDataTextColumn Caption="Updated By" FieldName="ModifyUser" 
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Updated On" FieldName="ModifyDate" 
                        VisibleIndex="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn ReadOnly="True" Width="0" CellStyle-HorizontalAlign="Center">
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <HeaderStyle HorizontalAlign="Center" />

                        <CellStyle HorizontalAlign="Center"></CellStyle>
                        <HeaderTemplate>
                            
                        </HeaderTemplate>
                        <DataItemTemplate>
                            <div class='floatedBtnArea'>
                            <%if (rights.CanEdit)
                              { %>
                            <a href="javascript:void(0);" onclick="onOpeningEdit('<%#Eval("UniqueId")%>')" title="" class="">
                                <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                            <%} %>
                            </div>
                        </DataItemTemplate>
                    </dxe:GridViewDataTextColumn>

                </Columns>
                <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                <SettingsPager PageSize="10">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                </SettingsPager>
                <SettingsBehavior ColumnResizeMode="NextColumn" />
                <ClientSideEvents EndCallback="OpeningGridEndCallBack" RowClick="gridRowclick" />

                <TotalSummary>
                    <dxe:ASPxSummaryItem FieldName="DebitAmount" SummaryType="Sum" DisplayFormat="0.00" ShowInColumn="DebitAmount" />
                    <dxe:ASPxSummaryItem FieldName="CreditAmount" SummaryType="Sum" DisplayFormat="0.00" ShowInColumn="CreditAmount" />

                </TotalSummary>

            </dxe:ASPxGridView>
        </div>
        <div class="clear"></div>
        <div style="padding-top: 10px;">
            <table width="100%" style="margin-left: 50px;">
                <tr>
                    <td><span id="ErrorText" style="color: red; display: none" class="lead">Mismatch detected for Debit and Credit Total of the selected Branch is :  
                        <dxe:ASPxLabel ID="diffAmt" runat="server" Text="" CssClass="lead" ClientInstanceName="cdiffAmt"></dxe:ASPxLabel>
                    </span></td>
                    <td>&nbsp</td>
                    <td style="width: 250px; display: none"><span class="pull-left" style="margin: 5px 15px 0 0;">Total Dr.</span>
                        <dxe:ASPxTextBox ID="totalDebit" runat="server" ClientEnable="false" ClientInstanceName="ctotalDebit" Width="150">
                            <MaskSettings Mask="<0..99999999999>.<00..99>" IncludeLiterals="DecimalSymbol" ErrorText="None" />
                        </dxe:ASPxTextBox>
                    </td>
                    <td style="width: 250px; display: none"><span class="pull-left" style="margin: 5px 15px 0 0;">Total Cr.</span>
                        <dxe:ASPxTextBox ID="totalCredit" runat="server" ClientEnable="false" ClientInstanceName="totalCredit" Width="150">
                            <MaskSettings Mask="<0..99999999999>.<00..99>" IncludeLiterals="DecimalSymbol" ErrorText="None" />
                        </dxe:ASPxTextBox>
                    </td>
                    <td>&nbsp</td>
                </tr>
            </table>
        </div>


        <div class="clear"></div>
        <div class="">
            <%if (rights.CanAdd)
              { %>
            <dxe:ASPxButton ID="FinalSave" runat="server" AutoPostBack="false" CssClass="btn btn-primary" Text="<u>S</u>ave" ClientInstanceName="cFinalSave" VerticalAlign="Bottom" EncodeHtml="false" UseSubmitBehavior="false">
                <ClientSideEvents Click="FinalSaveClientClick" />
            </dxe:ASPxButton>
            <%} %>
        </div>


    </div>
    </div>
    <asp:SqlDataSource ID="dsBranch" runat="server"  ConflictDetection="CompareAllValues"
        SelectCommand=""></asp:SqlDataSource>




    <div class="modal fade" id="SubAct" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Sub Account Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="SubActkeydown(event)" id="txtSubActSearch" autofocus width="100%" placeholder="Search By Sub Account Name" />

                    <div id="SubActTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Sub Account Name</th>
                                <th>Type</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>



    <dxe:ASPxGridViewExporter ID="exporter" GridViewID="OpeningGrid" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <asp:HiddenField ID="hdSubAccountId" runat="server"></asp:HiddenField>
     <%--  Rev 2.0--%>
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
                                   <%-- Rev 3.0--%>
                                     <asp:Button ID="BtnSaveexcel" runat="server" Text="Import(Add/Update)"  OnClick="BtnSaveexcel_Click" CssClass="btn btn-primary" UseSubmitBehavior="false"/>
                                   <%-- <asp:Button ID="BtnSaveexcel" runat="server" Text="Import(Add/Update)"  OnClick="BtnSaveexcel_Click" CssClass="btn btn-primary"  />--%>
                                   <%-- Rev 3.0 End--%>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

            </div>
        </div>
    </div>
      <div class="modal fade" id="modalSS" role="dialog">
        <div class="modal-dialog fullWidth">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Customer Import Log</h4>
                </div>
                <div class="modal-body">

                    <dxe:ASPxGridView ID="GvLogSearch" runat="server" AutoGenerateColumns="False" SettingsBehavior-AllowSort="true"
                        ClientInstanceName="cGvLogSearch" KeyFieldName="LOG_ID" Width="100%" OnDataBinding="GvLogSearch_DataBinding" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight="400">

                        <SettingsBehavior ConfirmDelete="false" ColumnResizeMode="NextColumn" />
                        <Styles>
                            <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
                            <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>
                            <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                            <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
                            <Footer CssClass="gridfooter"></Footer>
                        </Styles>
                        <Columns>
                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="LOG_ID" Caption="LogID" SortOrder="Descending">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="CREATEDON" Caption="Date" Width="10%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="LOG_Account" Caption="Account" Width="10%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>                                
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="LOG_SubAccount" Caption="Sub Account" Width="10%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>                               
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="LOG_Unit" Caption="Unit" Width="10%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>                               
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="LOG_LOOPNUMBER" Caption="Row Number" Width="13%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>                            
                            <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="LOG_FILENAME" Width="14%" Caption="File Name">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="LOG_DESCRIPTION" Caption="Description" Width="10%" Settings-AllowAutoFilter="False">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="LOG_STASTUS" Caption="Status" Width="14%" Settings-AllowAutoFilter="False">
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
     <%-- Rev 2.0 End--%>

</asp:Content>

