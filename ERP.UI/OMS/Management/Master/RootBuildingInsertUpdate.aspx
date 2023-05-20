<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                19-05-2023        2.0.38           Pallab              26175: Add Building Details module design modification & check in small device
====================================================== Revision History =============================================--%>

<%@ Page Title="Building/Warehouses" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false"
    Inherits="ERP.OMS.Management.Master.management_master_RootBuildingInsertUpdate" CodeBehind="RootBuildingInsertUpdate.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script type="text/javascript">
        function CheckUniqueCode(s, e) {
            var WHID = 0;
            if (GetObjectID('hdnEditID').value != 'ADD') {
                WHID = GetObjectID('hdnEditID').value;
            }
            var ProductName = ctxtCode.GetText().trim();
            $.ajax({
                type: "POST",
                url: "RootBuildingInsertUpdate.aspx/CheckUniqueName",
                data: JSON.stringify({ ProductName: ProductName, WHID: WHID }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;
                    if (data == true) {
                        jAlert("Please enter unique code", "Alert", function () { ctxtCode.SetFocus(); });
                        ctxtCode.SetText("");
                        return false;
                    }
                }

            });
        }

    </script>
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            ListBind();
            if (document.getElementById('txtCountry_hidden')) {
                var cntry = document.getElementById('txtCountry_hidden').value;
                document.getElementById('txtCountry_hidden').value = "";
                setCountry(cntry);
                setLevel($("#hdnLevel").val());
            }
        });

        function selectAll() {
            cBranchGridLookup.gridView.SelectRows();
        }
        function unselectAll() {
            cBranchGridLookup.gridView.UnselectRows();
        }
        function CloseGridLookup() {
            cBranchGridLookup.ConfirmCurrentSelection();
            cBranchGridLookup.HideDropDown();
        }

        function ClientSaveClick() {
            document.getElementById('txtCountry_hidden').value = document.getElementById('lstCountry').value;
            document.getElementById('txtState_hidden').value = document.getElementById('lstState').value;
            document.getElementById('txtCity_hidden').value = document.getElementById('lstCity').value;
            document.getElementById('HdPin').value = document.getElementById('lstPin').value;
            document.getElementById('hdnWarehouse').value = document.getElementById('ddl_ParentWarehouse').value;

            var level = document.getElementById('ddl_level').value;

            if (level != null && level != "") {
                if (level != "1") {
                    if (document.getElementById('ddl_ParentWarehouse').value == "" || document.getElementById('ddl_ParentWarehouse').value == null) {
                        jAlert('Please select parent warehouse.', 'Alert');
                        return false;
                    }
                }
            }
            else {
                if (!$("#dvMultiwarehouse").hasClass('hide')) {
                    jAlert('Please select warehouse level.', 'Alert');

                    return false;
                }
            }


            if ($("#TxtBuilding").val() == '') {
                return false;

            }
            if (document.getElementById('txtCountry_hidden').value == '') {
                // alert();
                return false;
            }

            else if (document.getElementById('txtState_hidden').value == '') {
                // alert();
                return false;
            }
            else {

                return true;
            }
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
        function Show() {

            var url = "frmAddDocuments.aspx";

            popup.SetContentUrl(url);

            popup.Show();
        }


        function setCountry(obj) {
            if (obj) {
                var lstCntry = document.getElementById("lstCountry");

                for (var i = 0; i < lstCntry.options.length; i++) {
                    if (lstCntry.options[i].value == obj) {
                        lstCntry.options[i].selected = true;
                    }
                }
                $('#lstCountry').trigger("chosen:updated");
                onCountryChange();
            }
        }


        function setLevel(obj) {
            if (obj) {
                var lstCntry = document.getElementById("ddl_level");

                for (var i = 0; i < lstCntry.options.length; i++) {
                    if (lstCntry.options[i].value == obj) {
                        lstCntry.options[i].selected = true;
                    }
                }
                $('#ddl_level').trigger("chosen:updated");
                level_change();
            }
        }


        function setState(obj) {
            if (obj) {
                var lstStae = document.getElementById("lstState");

                for (var i = 0; i < lstStae.options.length; i++) {
                    if (lstStae.options[i].value == obj) {
                        lstStae.options[i].selected = true;
                    }
                }
                $('#lstState').trigger("chosen:updated");
                onStateChange();
            }
        }
        function setCity(obj) {
            if (obj) {
                var lstCity = document.getElementById("lstCity");

                for (var i = 0; i < lstCity.options.length; i++) {
                    if (lstCity.options[i].value == obj) {
                        lstCity.options[i].selected = true;
                    }
                }
                $('#lstCity').trigger("chosen:updated");
                onCityChange();
            }
        }
        function setArea(obj) {
            if (obj) {
                var lstArea = document.getElementById("lstArea");

                for (var i = 0; i < lstArea.options.length; i++) {
                    if (lstArea.options[i].value == obj) {
                        lstArea.options[i].selected = true;
                    }
                }
                $('#lstArea').trigger("chosen:updated");

            }

        }
        function setPin(obj) {
            if (obj) {
                var lstPin = document.getElementById("lstPin");

                for (var i = 0; i < lstPin.options.length; i++) {
                    if (lstPin.options[i].value == obj) {
                        lstPin.options[i].selected = true;
                    }
                }
                $('#lstPin').trigger("chosen:updated");

            }
        }

        function setWarehouse(obj) {
            if (obj) {
                var ddl_ParentWarehouse = document.getElementById("ddl_ParentWarehouse");

                for (var i = 0; i < ddl_ParentWarehouse.options.length; i++) {
                    if (ddl_ParentWarehouse.options[i].value == obj) {
                        ddl_ParentWarehouse.options[i].selected = true;
                    }
                }
                $('#ddl_ParentWarehouse').trigger("chosen:updated");

            }
        }


        function onCountryChange() {
            var CountryId = "";
            if (document.getElementById('lstCountry').value) {
                CountryId = document.getElementById('lstCountry').value;
            } else {
                return;
            }
            var lState = $('select[id$=lstState]');
            var lCity = $('select[id$=lstCity]');
            var lArea = $('select[id$=lstArea]');
            var lPin = $('select[id$=lstPin]');
            lState.empty();
            lCity.empty();
            lArea.empty();
            lPin.empty();
            $('#lstCity').trigger("chosen:updated");
            $('#lstArea').trigger("chosen:updated");
            $('#lstPin').trigger("chosen:updated");
            $.ajax({
                type: "POST",
                url: "BranchAddEdit.aspx/GetStates",
                data: JSON.stringify({ CountryCode: CountryId }),
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

                            listItems.push('<option value="' +
                            id + '">' + name
                            + '</option>');
                        }

                        $(lState).append(listItems.join(''));

                        $('#lstState').fadeIn();
                        $('#lstState').trigger("chosen:updated");
                        if (document.getElementById('txtState_hidden').value) {
                            var stateVal = document.getElementById('txtState_hidden').value;
                            document.getElementById('txtState_hidden').value = "";
                            setState(stateVal);
                        }
                    }
                    else {
                        $('#lstState').fadeIn();
                        $('#lstState').trigger("chosen:updated");
                    }
                }
            });
        }

        function onStateChange() {
            var StateId = "";
            if (document.getElementById('lstState').value) {
                StateId = document.getElementById('lstState').value;
            }
            else {
                return;
            }
            var lCity = $('select[id$=lstCity]');
            var lArea = $('select[id$=lstArea]');
            var lPin = $('select[id$=lstPin]');
            lArea.empty();
            lCity.empty();
            lPin.empty();
            $('#lstArea').trigger("chosen:updated");
            $('#lstPin').trigger("chosen:updated");
            $.ajax({
                type: "POST",
                url: "BranchAddEdit.aspx/GetCities",
                data: JSON.stringify({ StateCode: StateId }),
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

                            listItems.push('<option value="' +
                            id + '">' + name
                            + '</option>');
                        }

                        $(lCity).append(listItems.join(''));

                        $('#lstCity').fadeIn();
                        $('#lstCity').trigger("chosen:updated");
                        if (document.getElementById('txtCity_hidden').value) {
                            var cityVal = document.getElementById('txtCity_hidden').value;
                            document.getElementById('txtCity_hidden').value = "";
                            setCity(cityVal);
                        }
                    }
                    else {
                        $('#lstCity').fadeIn();
                        $('#lstCity').trigger("chosen:updated");
                    }
                }
            });
        }
        function onCityChange() {
            getPinList();
        }

        function level_change() {
            var ddl_level = "";
            if (document.getElementById('ddl_level').value) {
                ddl_level = document.getElementById('ddl_level').value;
            }
            else {
                return;
            }
            var ddl_ParentWarehouse = $('select[id$=ddl_ParentWarehouse]');
            ddl_ParentWarehouse.empty();
            $.ajax({
                type: "POST",
                url: "RootBuildingInsertUpdate.aspx/GetWarehouse",
                data: JSON.stringify({ ddl_level: ddl_level }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {
                            var id = '';
                            var name = '';
                            id = list[i].id;
                            name = list[i].Name;

                            listItems.push('<option value="' +
                            id + '">' + name
                            + '</option>');
                        }

                        $(ddl_ParentWarehouse).append(listItems.join(''));

                        $('#ddl_ParentWarehouse').fadeIn();
                        $('#ddl_ParentWarehouse').trigger("chosen:updated");
                        if (document.getElementById('hdnWarehouse').value) {
                            setWarehouse(document.getElementById('hdnWarehouse').value);
                        }

                    }
                    else {
                        $('#ddl_ParentWarehouse').fadeIn();
                        $('#ddl_ParentWarehouse').trigger("chosen:updated");
                    }
                }
            });
        }


        function getPinList() {
            var CityId = "";
            if (document.getElementById('lstCity').value) {
                CityId = document.getElementById('lstCity').value;
            }
            else {
                return;
            }
            var lPin = $('select[id$=lstPin]');
            lPin.empty();
            $.ajax({
                type: "POST",
                url: "BranchAddEdit.aspx/GetPin",
                data: JSON.stringify({ CityCode: CityId }),
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

                            listItems.push('<option value="' +
                            id + '">' + name
                            + '</option>');
                        }

                        $(lPin).append(listItems.join(''));

                        $('#lstPin').fadeIn();
                        $('#lstPin').trigger("chosen:updated");
                        if (document.getElementById('HdPin').value) {
                            setPin(document.getElementById('HdPin').value);
                        }

                    }
                    else {
                        $('#lstPin').fadeIn();
                        $('#lstPin').trigger("chosen:updated");
                    }
                }
            });
        }
    </script>
    <style>
        #RequiredFieldValidator9 {
            position: absolute;
            right: 620px;
            top: 70px;
        }

        .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }

        #lstCountry, #lstState, #lstCity, #lstArea, #lstPin, #lstBranchHead {
            width: 200px;
        }

        #lstCountry, #lstState, #lstCity, #lstArea, #lstPin, #lstBranchHead, #ddl_ParentWarehouse {
            display: none !important;
        }

        #lstCountry_chosen, #lstState_chosen, #lstCity_chosen, #lstArea_chosen, #lstPin_chosen, #lstBranchHead_chosen {
            width: 100% !important;
        }

        #PageControl1_CC {
            overflow: visible !important;
        }

        #lstState_chosen, #lstCountry_chosen, #lstCity_chosen, #lstPin_chosen, #lstBranchHead_chosen {
            margin-bottom: 5px;
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
            <h3>
                <asp:Label ID="Label12" runat="server"></asp:Label>
                Building Details</h3>
        </div>
    </div>
        <div class="crossBtn"><a href="RootBuilding.aspx"><i class="fa fa-times"></i></a></div>
        <div class="form_main" style="border: 1px solid #ccc; padding: 10px 15px">

        <%--<asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>--%>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="col-md-4" id="DivWHCode" runat="server">
                        <label>
                            <asp:Label ID="Label1" runat="server" Text="Warehouse Code"></asp:Label>
                        </label>
                        <div>
                            <dxe:ASPxTextBox ID="txtCode" MaxLength="100" ClientInstanceName="ctxtCode" runat="server" Width="100%" CssClass="upper">
                                <ClientSideEvents TextChanged="function(s,e){CheckUniqueCode()}" />
                            </dxe:ASPxTextBox>
                             <asp:HiddenField runat="server" ID="hdnEditID" />
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="a" runat="server" ControlToValidate="txtCode"
                                CssClass="pullleftClass fa fa-exclamation-circle ctcclass " ToolTip="Mandatory."
                                ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <label>
                            <asp:Label ID="Label2" runat="server" Text="Building/Warehouse Name "></asp:Label><span style="color: red">*</span>
                        </label>
                        <div>
                            <asp:TextBox ID="TxtBuilding" runat="server" Width="100%" MaxLength="100"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ValidationGroup="a" runat="server" ControlToValidate="TxtBuilding"
                                CssClass="pullleftClass fa fa-exclamation-circle ctcclass " ToolTip="Mandatory."
                                ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <%--Rev 1.0 : "simple-select" class add --%>
                    <div class="col-md-4 simple-select">
                        <label>
                            <asp:Label ID="Label3" runat="server" Text="Caretaker/Contact Person" Width="100%"></asp:Label></label>
                        <div>
                            <asp:DropDownList ID="DDLCareTaker" runat="server" Width="100%">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <label>
                            <asp:Label ID="Label4" runat="server" Text="Address1"></asp:Label></label>
                        <div>
                            <asp:TextBox ID="TxtAdd1" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <label>
                            <asp:Label ID="Label5" runat="server" Text="Address2"></asp:Label></label>
                        <div>
                            <asp:TextBox ID="TxtAdd2" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <label>
                            <asp:Label ID="Label6" runat="server" Text="Address3 "></asp:Label></label>
                        <div>
                            <asp:TextBox ID="TxtAdd3" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <label>
                            <asp:Label ID="Label7" runat="server" Text="Landmark"></asp:Label></label>
                        <div>
                            <asp:TextBox ID="TxtLand" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <label>
                            <asp:Label ID="Label8" runat="server" Text="Country "></asp:Label><span style="color: red">*</span>
                        </label>
                        <div>
                            <%--<asp:DropDownList ID="DDLCountry" runat="server" Width="100%" AutoPostBack="True"
                                OnSelectedIndexChanged="DDLCountry_SelectedIndexChanged">
                            </asp:DropDownList>--%>

                            <asp:ListBox ID="lstCountry" CssClass="chsn" runat="server" Font-Size="12px" Width="253px" data-placeholder="Select..." onchange="onCountryChange()"></asp:ListBox>
                            <asp:HiddenField ID="txtCountry_hidden" runat="server" />
                            <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator4" InitialValue="0" ValidationGroup="a" runat="server" CssClass="pullrightClass fa fa-exclamation-circle r591" SetFocusOnError="true" ControlToValidate="DDLCountry" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <label>
                            <asp:Label ID="Label9" runat="server" Text="State "></asp:Label><span style="color: red">*</span>
                        </label>
                        <div>
                            <%--<asp:DropDownList ID="DDLState" runat="server" Width="100%" AutoPostBack="True"
                                OnSelectedIndexChanged="DDLState_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" InitialValue="0" ValidationGroup="a" runat="server" CssClass="pullrightClass fa fa-exclamation-circle r591" SetFocusOnError="true" ControlToValidate="DDLState" ForeColor="Red"></asp:RequiredFieldValidator>
                            <asp:TextBox ID="TxtState" runat="server" Visible="False" Width="100%"></asp:TextBox>--%>

                            <asp:ListBox ID="lstState" CssClass="chsn" runat="server" Font-Size="12px" Width="253px" data-placeholder="Select State.." onchange="onStateChange()"></asp:ListBox>
                            <asp:HiddenField ID="txtState_hidden" runat="server" />

                        </div>
                    </div>
                    <div class="col-md-4">
                        <label>
                            <asp:Label ID="Label10" runat="server" Text="City / District"></asp:Label></label>
                        <div>
                            <%--<asp:DropDownList ID="DDLCity" runat="server" Width="100%">
                            </asp:DropDownList>
                            <asp:TextBox ID="TxtCity" runat="server" Visible="False" Width="100%"></asp:TextBox>--%>

                            <asp:ListBox ID="lstCity" CssClass="chsn" runat="server" Font-Size="12px" Width="253px" data-placeholder="Select City.." onchange="onCityChange()"></asp:ListBox>
                            <asp:HiddenField ID="txtCity_hidden" runat="server" />
                        </div>
                    </div>
                    <div class="col-md-4">
                        <label>
                            <asp:Label ID="Label11" runat="server" Text="Pin"></asp:Label></label>
                        <div>
                            <%--<asp:TextBox ID="TxtPin" runat="server" Width="100%" MaxLength="50"></asp:TextBox>--%>

                            <asp:ListBox ID="lstPin" CssClass="chsn" runat="server" Font-Size="12px" Width="253px" data-placeholder="Select..."></asp:ListBox>
                            <asp:HiddenField ID="HdPin" runat="server" />
                        </div>
                    </div>
                    <div class="col-md-4">
                        <label>
                            <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="Branch">
                            </dxe:ASPxLabel>
                        </label>
                        <div>
                            <%--   <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%" TabIndex="5">
                            </asp:DropDownList>--%>
                            <dxe:ASPxGridLookup ID="BranchGridLookup" runat="server" SelectionMode="Multiple" ClientInstanceName="cBranchGridLookup" KeyFieldName="branch_id" TextFormatString="{1}" MultiTextSeparator=", "
                                DataSourceID="BranchdataSource" Width="100%">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption=" " SelectAllCheckboxMode="Page">
                                    </dxe:GridViewCommandColumn>

                                    <dxe:GridViewDataColumn FieldName="branch_code" Caption="Branch Code" Width="150">
                                        <Settings AutoFilterCondition="Contains" />

                                    </dxe:GridViewDataColumn>

                                    <dxe:GridViewDataColumn FieldName="branch_description" Caption="Branch Name" Width="150">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>



                                </Columns>
                                <GridViewProperties Settings-VerticalScrollBarMode="Auto">

                                    <Templates>
                                        <StatusBar>
                                            <table class="OptionsTable" style="float: right">
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxButton ID="btn_select_all" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll" UseSubmitBehavior="false" />
                                                        <dxe:ASPxButton ID="btn_deselect_all" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll" UseSubmitBehavior="false" />
                                                        <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" UseSubmitBehavior="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </StatusBar>
                                    </Templates>
                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />

                                </GridViewProperties>
                            </dxe:ASPxGridLookup>
                        </div>
                    </div>


                    <div class="clear"></div>

                    <div id="dvMultiwarehouse" runat="server" class="hide">
                        <div class="col-md-4">
                            <label>
                                <dxe:ASPxLabel ID="lbl_Level" runat="server" Text="Level"></dxe:ASPxLabel>
                            </label>
                            <font color="red">*</font>
                            <div>
                                <asp:ListBox ID="ddl_level" onchange="level_change();" DataTextField="Level_Name" DataValueField="Level_id" DataSourceID="sdLevel" runat="server" CssClass="chsn" Font-Size="12px" Width="253px" data-placeholder="Select..."></asp:ListBox>
                            </div>
                            <asp:SqlDataSource ID="sdLevel" runat="server" SelectCommand="select Level_id,Level_Name from Master_WarehouseLayout where ISNULL(Level_Name,'')<>'' order by Level_id"></asp:SqlDataSource>
                        </div>

                        <div class="col-md-4">
                            <label>
                                <dxe:ASPxLabel ID="lbl_ParentWarehouse" runat="server" Text="Parent Warehouse"></dxe:ASPxLabel>
                            </label>
                            <div>
                                <asp:ListBox ID="ddl_ParentWarehouse" runat="server" CssClass="chsn" Font-Size="12px" Width="253px" data-placeholder="Select..."></asp:ListBox>
                                <asp:HiddenField ID="hdnWarehouse" runat="server" />
                            </div>
                        </div>
                    </div>


                    <div class="clear"></div>
                    <div class="col-md-12  pdTop15">
                        <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="btn btn-primary btnUpdate" OnClick="BtnSave_Click" OnClientClick="return ClientSaveClick()" ValidationGroup="a" />
                        <asp:Button ID="BtnAdd" runat="server" Text="Add Files" Visible="false" OnClientClick="Show()" />
                    </div>
                </div>

                <%--   <dxe:ASPxPopupControl runat="server" ClientInstanceName="popup" CloseAction="CloseButton"
                        ContentUrl="frmAddDocuments.aspx" HeaderText="Add Document" Left="150" Top="10"
                        Width="700px" Height="400px" ID="ASPXPopupControl">
                        <ContentCollection>
                            <dxe:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            </dxe:PopupControlContentControl>
                        </ContentCollection>
                    </dxe:ASPxPopupControl>--%>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--<table>
                <tr>
                    <td >
                    </td>
                    <td>

                    </td>
                </tr>
            </table>--%>
    </div>
   </div>
    <asp:HiddenField runat="server" ID="hdnLevel" />
    
    <asp:SqlDataSource ID="BranchdataSource" runat="server"
        SelectCommand="select branch_id,branch_code,branch_description from tbl_master_branch"></asp:SqlDataSource>
</asp:Content>
