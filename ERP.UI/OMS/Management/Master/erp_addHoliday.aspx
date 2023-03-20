<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                20-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="erp_addHoliday.aspx.cs" Inherits="ERP.OMS.Management.Master.erp_addHoliday" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        $(document).ready(function () {
            //  cGrdHoliday.Refresh();
            $('.navbar-minimalize').click(function () {
                cGrdHoliday.Refresh();
            });

            var hdnValue = $("#Hidden_add_edit").val();
            if (hdnValue=="view") {
                $("#btnAllSave").addClass("hidden");
                $("#btnSave").addClass("hidden");
                $("#btnAddHoliday").addClass("hidden");
                $("#btnSaveExit").addClass("hidden");
            }
            else {
                $("#btnAllSave").removeClass("hidden");
                $("#btnSave").removeClass("hidden");
                $("#btnAddHoliday").removeClass("hidden");
                $("#btnSaveExit").removeClass("hidden");
            }
               
        });

        function grid_EndCallBack(s, e) {
            //var url = 'erp_addHoliday.aspx';
            //window.location.href = url;
        }

        function AddHoliday(value) {

            var FromMainDate = (cFormDate.GetValue() != null) ? cFormDate.GetValue() : "";
            var ToMainDate = (ctoDate.GetValue() != null) ? ctoDate.GetValue() : "";
            FromMainDate = GetDateFormat(FromMainDate);
            ToMainDate = GetDateFormat(ToMainDate);

            var holidayName = $("#txtHolidayName").val().trim();
            var FromDate = (cFrmDateDetl.GetValue() != null) ? cFrmDateDetl.GetValue() : "";
            var ToDate = (ctoDateDetail.GetValue() != null) ? ctoDateDetail.GetValue() : "";
            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);

            var Gu_id = $('#hdnGuid').val();

            var suc = true;
            if (ctxt_HolidayCode_nm.GetText() == "") {
                $("#HolidayCode_nm").show();
                $("#HoliDayDetail").modal('toggle');
                suc = false;
                return
            }
            else {
                $("#HolidayCode_nm").hide();
            }
            if (ctxt_HolidayDes_nm.GetText() == "") {
                $("#HolidayDesc_nm").show();
                $("#HoliDayDetail").modal('toggle');
                suc = false;
                return
            }
            else {
                $("#HolidayDesc_nm").hide();
            }
            if (FromMainDate == "") {
                suc = false;
                jAlert("Please Select From Date.", "Alert", function () {
                    setTimeout(function () {
                        cFormDate.Focus();
                        $("#HoliDayDetail").modal('toggle');
                        return
                    }, 200);
                });
            }
            else {
                if (ToMainDate == "") {
                    suc = false;
                    jAlert("Please Select To Date.", "Alert", function () {
                        setTimeout(function () {
                            ctoDate.Focus();
                            $("#HoliDayDetail").modal('toggle');
                            return
                        }, 200);
                    });
                }
                else {
                    if (ToMainDate < FromMainDate) {
                        suc = false;
                        jAlert("Please Select Valid To Date.", "Alert", function () {
                            setTimeout(function () {
                                ctoDate.Focus();
                                $("#HoliDayDetail").modal('toggle');
                                return
                            }, 200);
                        });
                    }
                    else {
                        if (holidayName != "") {
                            if (FromDate != "") {
                                if (FromMainDate > FromDate || FromDate > ToMainDate) {
                                    suc = false;
                                    jAlert("Please Select Valid From Date.", "Alert", function () {
                                        setTimeout(function () {
                                            cFrmDateDetl.Focus();
                                            $("#HoliDayDetail").modal('toggle');
                                            return
                                        }, 200);
                                    });
                                }
                                else {
                                    if (ToDate != "") {
                                        if (FromMainDate > ToDate || ToDate > ToMainDate) {
                                            suc = false;
                                            jAlert("Please Select Valid To Date.", "Alert", function () {
                                                setTimeout(function () {
                                                    ctoDateDetail.Focus();
                                                    $("#HoliDayDetail").modal('toggle');
                                                    return
                                                }, 200);
                                            });
                                        }
                                        else {
                                            if (ToDate >= FromDate) {
                                                $.ajax({
                                                    type: "POST",
                                                    url: "erp_addHoliday.aspx/AddData",
                                                    data: JSON.stringify({ Name: holidayName, FromDate: FromDate, todate: ToDate, Guids: Gu_id }),
                                                    contentType: "application/json; charset=utf-8",
                                                    dataType: "json",
                                                    success: function (msg) {
                                                        var list = msg.d;
                                                        //cGrdHoliday.Refresh();
                                                        jAlert(msg.d);
                                                        cGrdHoliday.PerformCallback();
                                                        $("#txtHolidayName").val('');
                                                        if (value=='Exit') {
                                                            $("#HoliDayDetail").modal('toggle');
                                                        }
                                                    }
                                                });
                                            }
                                            else {
                                                jAlert("Please Select valid ToDate.", "Alert", function () {
                                                    setTimeout(function () {
                                                        //  $('#toDetlDate').focus();
                                                        ctoDateDetail.Focus();
                                                    }, 200);
                                                });
                                            }
                                        }
                                    }
                                    else {
                                        jAlert("Please Select To Date.", "Alert", function () {
                                            setTimeout(function () {
                                                //  $('#toDetlDate').focus();
                                                ctoDateDetail.Focus();
                                            }, 200);
                                        });
                                    }
                                }
                            }
                            else {
                                jAlert("Please Select From Date.", "Alert", function () {
                                    setTimeout(function () {
                                        //  $('#FormDetlDate').focus();
                                        cFrmDateDetl.Focus();
                                    }, 200);
                                });
                            }
                        }
                        else {
                            jAlert("Please Enter Holiday Name.", "Alert", function () {
                                setTimeout(function () {
                                    $('#txtHolidayName').Focus();
                                }, 200);
                            });
                        }
                    }
                }
            }
        }
        function GetDateFormat(today) {
            if (today != "") {
                var dd = today.getDate();
                var mm = today.getMonth() + 1; //January is 0!

                var yyyy = today.getFullYear();
                if (dd < 10) {
                    dd = '0' + dd;
                }
                if (mm < 10) {
                    mm = '0' + mm;
                }
                //  today = dd + '-' + mm + '-' + yyyy;
                today = yyyy + '-' + mm + '-' + dd;
            }

            return today;
        }


        function apply() {
            if (ctxt_HolidayCode_nm.GetText().trim() == "") {
                $("#HolidayCode_nm").show();
                return
            }
            else {
                $("#HolidayCode_nm").hide();
            }
            if (ctxt_HolidayDes_nm.GetText().trim() == "") {
                $("#HolidayDesc_nm").show();
                return
            }
            else {
                $("#HolidayDesc_nm").hide();
            }
            if (cFormDate.GetText() == "") {
                jAlert("Please Select From Date.", "Alert", function () {
                    setTimeout(function () {
                        cFormDate.focus();
                        return
                    }, 200);
                });
            }
            if (ctoDate.GetText() == "") {
                jAlert("Please Select To Date.", "Alert", function () {
                    setTimeout(function () {
                        ctoDate.focus();
                        return
                    }, 200);
                });
            }
            var FromMainDate = (cFormDate.GetValue() != null) ? cFormDate.GetValue() : "";
            var ToMainDate = (ctoDate.GetValue() != null) ? ctoDate.GetValue() : "";
            FromMainDate = GetDateFormat(FromMainDate);
            ToMainDate = GetDateFormat(ToMainDate);

            if (ToMainDate >= FromMainDate) {
                jAlert("Please Select Valid To Date.", "Alert", function () {
                    setTimeout(function () {
                        ctoDate.focus();
                        return
                    }, 200);
                });
            }

            var frmDate = GetDateFormat(cFormDate.GetValue());
            var ToDate = GetDateFormat(ctoDate.GetValue());
            var Apply = {
                holidayCode: ctxt_HolidayCode_nm.GetText(),
                holidayName: ctxt_HolidayDes_nm.GetText(),
                branch: ccmbBranchfilter.GetValue(),
                fromdate: frmDate,
                todate: ToDate,
                holiday_ID: $("#Hidn_team_id").val(),
                Action: $("#Hidden_add_edit").val()
            }
            //HolidayCode, String HolidayName, String fromdate, String toDate,String BranchID,String holidayID,String Action
            $.ajax({
                type: "POST",
                url: "erp_addHoliday.aspx/save",
                data: "{apply:" + JSON.stringify(Apply) + "}",
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    console.log(response);
                    if (response.d) {
                        if (response.d == "true") {
                            jAlert("Saved Successfully.", "Alert", function () {
                                window.location.href = "erp_addHoliday.aspx?id=ADD";
                            });
                        }
                        else {
                            jAlert(response.d);
                            $("#HolidayCode_nm").show();
                            return
                        }
                    }

                },
                error: function (response) {

                    console.log(response);
                }

            });
        }

        function OnClickDelete(val) {
            jConfirm('Confirm Delete?', 'Alert', function (r) {
                if (r) {
                    $.ajax({
                        type: "POST",
                        url: "erp_addHoliday.aspx/DeleteData",
                        data: JSON.stringify({ HiddenID: val }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var list = msg.d;
                            //cGrdHoliday.Refresh();
                            jAlert(msg.d);
                            //  $("#HoliDayDetail").modal('toggle');
                            cGrdHoliday.PerformCallback();

                        }
                    });
                }
                else {

                }
            });
        }

        function ClickOnEdit(val) {
            $.ajax({
                type: "POST",
                url: "erp_addHoliday.aspx/EditData",
                data: JSON.stringify({ HiddenID: val }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    //cGrdHoliday.Refresh();
                    // jAlert(msg.holidayName);
                    $("#txtHolidayName").val(msg.d.holidayName);
                    //var fromdatearray = msg.d.fromdate.split('-');
                    //var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                    //cFrmDateDetl.SetDate(fromdate);
                    //var Todatearray = msg.d.fromdate.split('-');
                    //var todate = new Date(Todatearray[0], parseFloat(Todatearray[1]) - 1, Todatearray[2], 0, 0, 0, 0);
                    //ctoDateDetail.SetDate(todate);
                    var dtstrt = new Date(parseInt(msg.d.fromdate.substr(6)));
                    var dtEnd = new Date(parseInt(msg.d.todate.substr(6)));
                    cFrmDateDetl.SetDate(dtstrt);
                    ctoDateDetail.SetDate(dtEnd);
                    $('#hdnGuid').val(val);
                    $("#HoliDayDetail").modal('toggle');
                    cGrdHoliday.PerformCallback();

                }
            });
        }

        function cancel() {
            var url = "erp_addHoliday.aspx?id=ADD";
            window.location.href = url;
        }

        function Close() {
            $('#hdnGuid').val('');
            $("#txtHolidayName").val('');
            cFrmDateDetl.SetDate(new now.data);
            ctoDateDetail.SetDate(new now.data);
        }
    </script>
    <style>
        .boxModel {
            background: #f2f4fb;
            padding: 11px 15px;
        }

        .errorField {
            position: absolute;
            right: -19px;
            top: 3px;
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

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #EmployeeGrid , .TableMain100 #GrdEmployee,
        .TableMain100 #GrdHolidays
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
        <div class="panel-title clearfix">
            <h3 class="pull-left">
                <asp:Label runat="server" ID="HeaderName"></asp:Label></h3>

            <div class="crossBtn"><a href="erpHolidayList.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>

        <div class="form_main">
        <div class="row mTop5">
            <div class="col-md-12 mTop5">
                <div class=" clearfix">
                    <div class="row">
                        <div class="col-md-2">
                            <div class="form-group ">
                                <label for="" class=" col-form-label">Holiday Code<span style="color: red">*</span> </label>
                                <div class="">
                                    <div class="relative">
                                        <dxe:ASPxTextBox ID="txt_HolidayCode_nm" runat="server" ClientInstanceName="ctxt_HolidayCode_nm" Width="100%" MaxLength="50" CssClass="">
                                        </dxe:ASPxTextBox>
                                        <span id="HolidayCode_nm" style="display: none;" class="errorField">
                                            <img id="mandetorydelivery" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory" />
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group ">
                                <label for="" class="col-form-label">Holiday Description<span style="color: red">*</span> </label>
                                <div class="">
                                    <div class="relative">
                                        <dxe:ASPxTextBox ID="txt_HolidayDes_nm" runat="server" ClientInstanceName="ctxt_HolidayDes_nm" MaxLength="50" Width="100%" CssClass="">
                                        </dxe:ASPxTextBox>
                                        <span id="HolidayDesc_nm" style="display: none;" class="errorField">
                                            <img id="mandetorydeliverys" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory" />
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group ">
                                <label for="" class="col-form-label">Unit<span style="color: red">*</span> </label>
                                <div class="">
                                    <div class=" relative">
                                        <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group ">
                                <label for="" class="col-form-label">From Date <span style="color: red">*</span></label>
                                <div>
                                    <div class=" relative">
                                        <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                        </dxe:ASPxDateEdit>
                                        <%--Rev 1.0--%>
                                        <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                        <%--Rev end 1.0--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group ">
                                <label for="" class=" col-form-label">To Date <span style="color: red">*</span></label>
                                <div class="">
                                    <div class=" relative">
                                        <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                        </dxe:ASPxDateEdit>
                                        <%--Rev 1.0--%>
                                        <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                        <%--Rev end 1.0--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <button type="button" id="btnAddHoliday" class="btn btn-success btn-radius" style="margin-top: 18px;" data-toggle="modal" data-target="#HoliDayDetail" data-backdrop="static" data-keyboard="true">
                                <span class="btn-icon"><i class="fa fa-plus"></i></span>Holiday
                            </button>
                        </div>
                    </div>
                </div>
            </div>

        </div>
        <br />
        <div class="clear"></div>
        <div class="form-group ">
            <dxe:ASPxGridView ID="GrdHoliday" runat="server" KeyFieldName="HIddenID" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
                SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" OnDataBinding="grid_DataBinding" OnCustomCallback="GrdHoliday_CustomCallback"
                Width="100%" ClientInstanceName="cGrdHoliday">
                <SettingsSearchPanel Visible="True" Delay="5000" />
                <Columns>
                    <dxe:GridViewDataTextColumn Caption="ID" FieldName="HIddenID"
                        VisibleIndex="1" FixedStyle="Left" Width="200px" Visible="false">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Holiday Name" FieldName="Name"
                        VisibleIndex="2" Width="150px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Holiday Start Date" Width="150px" FieldName="FrmDate" VisibleIndex="3">
                        <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Holiday End Date" FieldName="ToDate" Width="150px" VisibleIndex="4">
                        <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="7" Width="240px">
                        <DataItemTemplate>

                            <a href="javascript:void(0);" onclick="ClickOnEdit('<%# Container.KeyValue %>')" id="a_editInvoice" class="pad" title="Edit">
                                <img src="../../../assests/images/info.png" /></a>

                            <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="pad" title="Delete" id="a_delete">
                                <img src="../../../assests/images/Delete.png" /></a>

                            <%-- <% if (rights.CanView)
                                       { %>
                                    <a href="javascript:void(0);" onclick="OnclickView('<%# Container.KeyValue %>')" class="pad" title="View Attachment">
                                        <img src="../../../assests/images/viewIcon.png" />
                                    </a><%} %>--%>
                        </DataItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <CellStyle HorizontalAlign="Center"></CellStyle>
                        <HeaderTemplate><span>Actions</span></HeaderTemplate>
                        <EditFormSettings Visible="False"></EditFormSettings>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                </Columns>
                <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                <ClientSideEvents EndCallback="grid_EndCallBack" />
                <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                    <FirstPageButton Visible="True">
                    </FirstPageButton>
                    <LastPageButton Visible="True">
                    </LastPageButton>
                </SettingsPager>

                <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                <SettingsLoadingPanel Text="Please Wait..." />
            </dxe:ASPxGridView>
            <%--    <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                        ContextTypeName="ERPDataClassesDataContext" TableName="ERP_TEAMVIEW" />--%>
        </div>

    </div>
    
        <div class="clear"></div>
        <div class="Row">
            <div class="col-md-12 " style="padding-left: 5px">
                <div class="Left_Content">
                    <button type="button" id="btnAllSave" class="btn btn-primary" onclick="apply()">Save</button>
                    <button type="button" class="btn btn-danger" onclick="cancel()">Cancel</button>
                </div>
            </div>
        </div>
    </div>



    <asp:HiddenField runat="server" ID="hdnGuid" />

    <div class="modal fade pmsModal w30" id="HoliDayDetail" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Holiday Details </h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" onclick="Close()">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="pmsForm">

                        <div class="form-group row">
                            <label for="" class="col-sm-4 col-form-label">Holiday Name <span style="color: red">*</span></label>
                            <div class="col-sm-8">
                                <input type="text" class="form-control" id="txtHolidayName" value="" />
                            </div>
                        </div>

                        <div class="formLine"></div>
                        <div class="form-group row">
                            <label for="" class="col-sm-4 col-form-label">From Date <span style="color: red">*</span></label>
                            <div class="col-sm-8">
                                <div class=" relative">
                                    <dxe:ASPxDateEdit ID="FormDetlDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFrmDateDetl"
                                        Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" CssClass="dateEditInput">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                    </dxe:ASPxDateEdit>
                                </div>
                            </div>
                        </div>
                        <div class="formLine"></div>
                        <div class="form-group row">
                            <label for="" class="col-sm-4 col-form-label">To Date <span style="color: red">*</span></label>
                            <div class="col-sm-8">
                                <div class=" relative">
                                    <dxe:ASPxDateEdit ID="toDetlDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDateDetail"
                                        Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" CssClass="dateEditInput">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                    </dxe:ASPxDateEdit>
                                </div>
                            </div>
                        </div>
                        <div class="formLine"></div>

                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger btn-radius" data-dismiss="modal" onclick="Close()">Close</button>
                    <button type="button" class="btn btn-success btn-radius" id="btnSave" onclick="AddHoliday('New')">Save & New</button>
                        <%--<span class="btn-icon"><i class="fa fa-plus"></i></span>--%>
                    <button type="button" class="btn btn-success btn-radius" id="btnSaveExit" onclick="AddHoliday('Exit')">Save & Exit</button>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="UserId" />
    <asp:HiddenField runat="server" ID="Hidden_add_edit" />
    <asp:HiddenField runat="server" ID="Hidn_team_id" />

</asp:Content>
