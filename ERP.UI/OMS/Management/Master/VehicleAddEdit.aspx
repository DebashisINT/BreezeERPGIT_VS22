<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                24-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="VehicleAddEdit.aspx.cs"
    Inherits="ERP.OMS.Management.Master.VehicleAddEdit" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="/assests/js/init.js"></script>
    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>

    <script lang="javascript" type="text/javascript">
        $(document).ready(function () {
            var MaxLength = 150;
            $('#txtRemarks').keypress(function (e) {
                if ($(this).val().length >= MaxLength) {
                    e.preventDefault();
                }
            });

            $('#<%= lblBranch.ClientID %>').attr('style', 'display:none');
        });


        function branchGridEndCallBack() {
            if (cbranchGrid.cpReceviedString) {
                if (cbranchGrid.cpReceviedString == 'SetAllRecordToDataTable') {
                    cBranchSelectPopup.Hide();
                }
            }

            if (cbranchGrid.cpBrselected) {
                // cbranchGrid.cpBrselected = null;

                // alert(cbranchGrid.cpBrselected);
                if (cbranchGrid.cpBrselected == '1') {


                    jAlert("Individual branch selection not allowed when all branch option is checked.");
                    cbranchGrid.cpBrselected = null;
                    cBranchSelectPopup.Show();
                }

                else { cbranchGrid.cpBrselected = null; }

            }
            //else if (cbranchGrid.cpBrselected == '0') {


            //    jAlert("Please select atleast  one branch.");
            //    cbranchGrid.cpBrselected = null;
            //    cBranchSelectPopup.Show();
            //}
            if (cbranchGrid.cpBrChecked) {
                if (cbranchGrid.cpBrChecked == '1') {
                    $('#<%= lblBranch.ClientID %>').attr('style', 'display:inline');
                    $('#<%=chkAllBranch.ClientID %>').prop('checked', true)
                    cbranchGrid.cpBrChecked = null;
                    //  hdnBranchAllSelected
                    $('#<%= hdnBranchAllSelected.ClientID %>').val('0');
                }
                else {
                    $('#<%= lblBranch.ClientID %>').attr('style', 'display:none');
                    $('#<%=chkAllBranch.ClientID %>').prop('checked', false)
                    cbranchGrid.cpBrChecked = null;
                    $('#<%= hdnBranchAllSelected.ClientID %>').val('1');
                }
            }
        }

        function selectAll() {

            cbranchGrid.PerformCallback('SelectAllBranchesFromList');
            cBranchSelectPopup.Show();
        }
        function unselectAll() {
            cbranchGrid.PerformCallback('ClearSelectedBranch');
            cBranchSelectPopup.Show();
        }

        function SelectAllBranches(e) {

            if (e.checked == true) {

                ClearSelectedBranch();
                $('#<%= hdnBranchAllSelected.ClientID %>').val('0');
                $('#<%= lblBranch.ClientID %>').attr('style', 'display:inline');

            }
            else {
                $('#<%= hdnBranchAllSelected.ClientID %>').val('1');
                $('#<%= lblBranch.ClientID %>').attr('style', 'display:none');

            }
        }


        function CmbBranchChanged() {

            var branchCode = CmbBranch.GetValue();
            if (branchCode == 0) {
                $('#MultiBranchButton').show();
            }
            else {
                $('#MultiBranchButton').hide();
            }
        }

        function MultiBranchClick() {

            cbranchGrid.PerformCallback('SetAllSelectedRecord');
            cBranchSelectPopup.Show();
        }

        function SaveSelectedBranch() {

            // var allbranch = $('#<%= hdnBranchAllSelected.ClientID %>').val();
            ///alert(allbranch);
            cbranchGrid.PerformCallback('SetAllRecordToDataTable');
        }

        function ClearSelectedBranch() {
            cbranchGrid.PerformCallback('ClearSelectedBranch');
        }
        function ClientSaveClick() {
            //if (isVehicleRegistraionNoAvailable()) {
            //    return true;
            //}
            //else {
            //    return false;
            //}
            //if (isVehicleChassisNoAvailable()) {
            //    return true;
            //}
            //else {
            //    return false;
            //}
            //if (isVehicleEngineNoAvailable()) {
            //    return true;
            //}
            //else {
            //    return false;
            //}

        }


        //Debjyoti Code for UDF
        function OpenUdf() {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                var keyVal = document.getElementById('Keyval_internalId').value;
                var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=VCH&&KeyVal_InternalID=' + keyVal;
                //alert("OpenUdf() -- else -- keyVal" + keyVal);
                popup.SetContentUrl(url);
                popup.Show();
            }
            return true;
        }



        function popUpRedirect(obj) {
            jAlert("Saved successfully");
            window.location.href = obj;


        }



        var pinCodeWithAreaId = [];
        //////////$(document).ready(function () {
        //////////    ListBind();
        //////////    /*Code  Added  By Priti on 06122016 to use jquery Choosen for BranchHead*/
        //////////    ChangeSourceBranchHead();
        //////////    //.............end........
        //////////    ////debugger;
        //////////    ////var cntry = document.getElementById('txtCountry_hidden').value;
        //////////    ////document.getElementById('txtCountry_hidden').value = "";
        //////////    ////setCountry(cntry);
        //////////});

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
        function lstCountry() {
            $('#lstCountry').fadeIn();
        }

        function disp_prompt(name) {

            if (name == "tab0") {
                //alert(name);
                document.location.href = "VehicleAddEdit.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "Vehicle_Document.aspx";
            }



        }

        function Close() {
            editwin.close();
        }

        function DeleteRow(keyValue) {
            doIt = confirm('Confirm delete?');
            if (doIt) {
                grid.PerformCallback('Delete~' + keyValue);
                //height();
            }
            else {

            }
        }


        function ShowHideFilter1(obj) {

            gridTerminal.PerformCallback(obj);
        }

        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function setvaluetovariable(obj1) {
            combo1.PerformCallback(obj1);
        }
        function setvaluetovariable1(obj1) {
            combo2.PerformCallback(obj1);
        }
        function CallList(obj1, obj2, obj3) {
            //ajax_showOptions(obj1, obj2, obj3);
            if (obj1.value == "") {
                obj1.value = "%";
            }
            var obj5 = '';
            ajax_showOptionsTEST(obj1, obj2, obj3, obj5);
            if (obj1.value == "%") {
                obj1.value = "";
            }
        }
        function hide_show(obj) {
            if (obj == 'All') {
                document.getElementById("client_pro").style.display = "none";
            }

        }
        function GetClick() {
            btnC.PerformCallback();
        }
        function Page_Load() {
            document.getElementById("TdCombo").style.display = "none";
        }

        function CheckLengthNote1() {
            var textbox = document.getElementById('txtRemarks').value;
            if (textbox.trim().length >= 150) {
                return false;
            }
            else {
                return true;
            }
        }
        function CheckingTD(obj) {

            var gridstat = gridTerminal.cpCompCombo;
            if (gridstat == 'anew')
                combo.SetFocus();
        }
        FieldName = "cmbExport_DDDWS";
        function LastCall(obj) {
            //height();
        }


        function fn_checkRegnNumber_TextChanged() {
            var clientName = txtVehRegNo.GetText();
            //alert(clientName);
            $.ajax({
                type: "POST",
                url: "VehicleAddEdit.aspx/checkRegNoAvailability",
                data: JSON.stringify({ RegistrationNo: clientName }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;

                    if (data == true) {
                        jAlert("Registration Number Already Exist!");
                        txtVehRegNo.SetText("");
                        document.getElementById("txtVehRegNo").focus();
                        return false;
                    }
                }
            });
        }


        function fn_checkEngineNumber_TextChanged() {

            var clientName = txtEngineNo.GetText();
            $.ajax({
                type: "POST",
                url: "VehicleAddEdit.aspx/checkEngineNoAvailability",
                data: JSON.stringify({ EngineNo: clientName }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;

                    if (data == true) {
                        jAlert("Engine Number Already Exist!");
                        txtEngineNo.SetText("");
                        document.getElementById("txtEngineNo").focus();
                        return false;
                    }

                }
            });
        }



        function fn_checkChassisNo_TextChanged() {

            var clientName = txtChassisNo.GetText();
            $.ajax({
                type: "POST",
                url: "VehicleAddEdit.aspx/checkChassNoAvailability",
                data: JSON.stringify({ ChassisNo: clientName }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;

                    if (data == true) {
                        jAlert("Chassis Number Already Exist!");
                        txtChassisNo.SetText("");
                        document.getElementById("txtChassisNo").focus();
                        return false;
                    }

                }
            });

        }





        function fn_btnValidate(s, e) {
            debugger;
            e.processOnServer = false;
            var ret = true;
            var contype = '<%= Session["Contactrequesttype"] %>';
            //alert(contype);
            if (contype == 'customer') {

            }

            if (contype != 'Lead') {


            }

            if (contype == 'Lead') {

            }

            if (contype != 'Lead') {

            }

            e.processOnServer = ret;
        }




    </script>

    <style type="text/css">
        .vehiclecls {
            display: inline;
            color: red;
            font-size: 14px;
            padding-left: 15px;
        }

        .abs {
            position: absolute;
            right: -19px;
            top: 4px;
        }

        .abs1 {
            position: absolute;
            right: -19px;
            top: 4px;
        }

        .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }

        #lstCountry, #lstState, #lstCity, #lstArea, #lstPin, #lstBranchHead {
            width: 100%;
        }

        #lstCountry, #lstState, #lstCity, #lstArea, #lstPin, #lstBranchHead {
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

        .divControlClass > span.controlClass {
            margin-top: 8px;
        }

        /*Rev 1.0*/

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

        #txtInsValidUpto , #ASPxDateEditPolluValidUpto , #ASPxDateAuthLttrValidUpto , #ASPxDateCFValidUpto , #ASPxDateEditTaxValidUpto
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #txtInsValidUpto_B-1 , #ASPxDateEditPolluValidUpto_B-1 , #ASPxDateAuthLttrValidUpto_B-1 , #ASPxDateCFValidUpto_B-1 , #ASPxDateEditTaxValidUpto_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #txtInsValidUpto_B-1 #txtInsValidUpto_B-1Img , #ASPxDateEditPolluValidUpto_B-1 #ASPxDateEditPolluValidUpto_B-1Img,
        #ASPxDateAuthLttrValidUpto_B-1 #ASPxDateAuthLttrValidUpto_B-1Img , #ASPxDateCFValidUpto_B-1 #ASPxDateCFValidUpto_B-1Img , 
        #ASPxDateEditTaxValidUpto_B-1 #ASPxDateEditTaxValidUpto_B-1Img
        {
            display: none;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
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
        .TableMain100 #GrdHolidays , #cityGrid
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

        .col-md-3
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

        /*#ShowFilter
        {
            padding-bottom: 3px !important;
        }*/

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

    <!-- Validations for Vehicle Page-->
    <script lang="javascript" type="text/javascript">
        $(document).ready(function () {
            $("#txtRegnYear").keydown(function (e) {
                // Allow: backspace, delete, tab, escape, enter and .
                if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                    // Allow: Ctrl+A, Command+A
                    (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                    // Allow: home, end, left, right, down, up
                    (e.keyCode >= 35 && e.keyCode <= 40)) {
                    // let it happen, don't do anything
                    return;
                }
                // Ensure that it is a number and stop the keypress
                if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                    e.preventDefault();
                }
            });
        });

        //Checking for Vehicle Registration Year.
        $(document).ready(function () {
            $("#txtRegnYear").blur(function (e) {
                var regYearVal = $("#txtRegnYear").val();
                var currentYr = '<%= Session["CurrYear"] %>';
                if (regYearVal <= 2017) {
                    if (regYearVal.length >= 1 && regYearVal.length < 4) {
                        jAlert("Please provide a proper Year value!");
                        $("#txtRegnYear").val("");
                        $("#txtRegnYear").focus();
                    }
                    else {
                        return;
                    }
                }
                else {
                    jAlert("Registration Year should be on or before 2017!");
                    $("#txtRegnYear").val("");
                    $("#txtRegnYear").focus();
                }
            });
        });

        function popUpRedirect(obj) {
            jAlert('Vehicle Saved Successfully!', 'Alert', function () { window.location.href = obj; });

        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>
                <asp:Label ID="lblAddEdit" runat="server"></asp:Label>
            </h3>
            <div class="crossBtn"><a href="Vehicle.aspx"><i class="fa fa-times"></i></a></div>
        </div>

    </div>

        <div class="form_main">

        <%--debjyoti 21-03-2016--%>
        <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
            Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
        <asp:HiddenField runat="server" ID="IsUdfpresent" />
        <asp:HiddenField runat="server" ID="Keyval_internalId" />
        <%--End debjyoti 21-03-2016--%>

        <table class="TableMain100">
            <tr>
                <td style="text-align: center">
                    <asp:Label ID="lblHeader" runat="server" Font-Bold="True" Font-Size="15px" ForeColor="Navy" Width="100%" Height="18px"></asp:Label>
                </td>

            </tr>
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" ClientInstanceName="page" Font-Size="12px" Width="100%">
                        <TabPages>
                            <dxe:TabPage Text="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <div class="totalWrap">

                                            <div class="col-md-3">
                                                <label>Vehicle Registration No. <span style="color: red">*</span></label>
                                                <div class="relative">

                                                    <div class="relative">
                                                        <dxe:ASPxTextBox ID="txtVehRegNo" runat="server" TabIndex="1" Width="100%" MaxLength="50" CssClass="upper">
                                                            <ClientSideEvents TextChanged="function(s,e){fn_checkRegnNumber_TextChanged()}" />
                                                        </dxe:ASPxTextBox>
                                                        <asp:RequiredFieldValidator Display="Dynamic" runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtVehRegNo"
                                                            SetFocusOnError="true" ErrorMessage="" class="pullrightClass fa fa-exclamation-circle abs iconRed" ToolTip="Mandatory" ValidationGroup="drivegrp">                                                        
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator CssClass="pullrightClass" ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtVehRegNo" ForeColor="Red"
                                                            ValidationExpression="[a-zA-Z0-9]*$" ErrorMessage="Special characters not allowed." SetFocusOnError="true" ValidationGroup="drivegrp" />
                                                    </div>



                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Engine No.</label>
                                                <div class="relative">
                                                    <div class="relative">
                                                        <dxe:ASPxTextBox ID="txtEngineNo" runat="server" TabIndex="2" Width="100%" MaxLength="50" CssClass="upper">
                                                            <ClientSideEvents TextChanged="function(s,e){fn_checkEngineNumber_TextChanged()}" />
                                                        </dxe:ASPxTextBox>
                                                        <%--<asp:RequiredFieldValidator Display="Dynamic" runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtEngineNo"
                                                            SetFocusOnError="true" ErrorMessage="" class="pullrightClass fa fa-exclamation-circle abs iconRed" ToolTip="Mandatory" >                                                        
                                                        </asp:RequiredFieldValidator>--%>
                                                    </div>

                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Chassis No. </label>
                                                <div class="relative">
                                                    <%--<asp:TextBox ID="txtChassisNo" runat="server"   Width="100%" MaxLength="50"  >                                                                   
                                                    </asp:TextBox>--%>
                                                    <dxe:ASPxTextBox ID="txtChassisNo" runat="server" TabIndex="3" Width="100%" MaxLength="50" CssClass="upper">
                                                        <ClientSideEvents TextChanged="function(s,e){fn_checkChassisNo_TextChanged()}" />
                                                    </dxe:ASPxTextBox>
                                                    <%-- <asp:RequiredFieldValidator Display="Dynamic" runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtEngineNo"
                                                            SetFocusOnError="true" ErrorMessage="" class="pullrightClass fa fa-exclamation-circle abs iconRed" ToolTip="Mandatory" >                                                        
                                                        </asp:RequiredFieldValidator>--%>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Vehicle Type</label>
                                                <%--Rev 1.0: "simple-select" class add --%>
                                                <div class="relative simple-select">
                                                    <asp:DropDownList ID="ddlVehicleType" runat="server" Width="100%" TabIndex="4">
                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                        <asp:ListItem Value="1">HCV</asp:ListItem>
                                                        <asp:ListItem Value="2">MCV</asp:ListItem>
                                                        <asp:ListItem Value="3">LCV</asp:ListItem>
                                                        <asp:ListItem Value="4">SUV</asp:ListItem>
                                                        <asp:ListItem Value="5">MUV</asp:ListItem>
                                                        <asp:ListItem Value="6">Car</asp:ListItem>
                                                        <asp:ListItem Value="7">Others</asp:ListItem>
                                                    </asp:DropDownList>

                                                </div>
                                            </div>
                                            <div class="clear"></div>

                                            <div class="col-md-3">
                                                <label>Vehicle Maker</label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtVehicleMaker" runat="server" ClientIDMode="Static" Width="100%" MaxLength="50" TabIndex="5">        
                                                    </asp:TextBox>

                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Model </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtVehicleModel" runat="server" ClientIDMode="Static" Width="100%"
                                                        MaxLength="50" TabIndex="6">        
                                                    </asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Year of Registration </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtRegnYear" runat="server" ClientIDMode="Static" Width="100%"
                                                        MaxLength="4" TabIndex="7">        
                                                    </asp:TextBox>

                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Ownership</label>
                                                <%--Rev 1.0: "simple-select" class add --%>
                                                <div class="relative simple-select">
                                                    <asp:DropDownList ID="ddlOwnershipType" runat="server" Width="100%" TabIndex="8">
                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                        <asp:ListItem Value="1">Owned</asp:ListItem>
                                                        <asp:ListItem Value="2">Hired</asp:ListItem>
                                                    </asp:DropDownList>

                                                </div>
                                            </div>
                                            <div class="clear"></div>

                                            <div class="col-md-3">
                                                <label>Engine CC</label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtEngineCC" runat="server" ClientIDMode="Static" Width="100%"
                                                        MaxLength="4" TabIndex="9">        
                                                    </asp:TextBox>

                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Log Book Status</label>
                                                <%--Rev 1.0: "simple-select" class add --%>
                                                <div class="relative simple-select">
                                                    <asp:DropDownList ID="ddlLogBookStatus" runat="server" Width="100%" TabIndex="10">
                                                        <asp:ListItem Value="0">Yes</asp:ListItem>
                                                        <asp:ListItem Value="1">No</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>GPS Device Enabled ? </label>
                                                <%--Rev 1.0: "simple-select" class add --%>
                                                <div class="relative simple-select">
                                                    <asp:DropDownList ID="ddlGPSInstallationStatus" runat="server" Width="100%" TabIndex="11">
                                                        <asp:ListItem Value="0">Yes</asp:ListItem>
                                                        <asp:ListItem Value="1">No</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <%--  <div class="col-md-3">
                                                <label>Vehicle Alloted to</label>
                                                <div class="relative">  
                                                    <asp:DropDownList ID="cmbBranch" runat="server" Width="100%" TabIndex="12">
                                                    </asp:DropDownList>

                                                </div>
                                            </div>--%>



                                            <div class="col-md-3" id="divBranch" style="display: block;">
                                                <label>
                                                    Vehicle Alloted to :<%--<span style="color: Red;">*</span> --%>
                                                </label>
                                                <div>
                                                    <div>
                                                        <asp:Label ID="lblSelectedBranch" runat="server"></asp:Label></div>
                                                    <dxe:ASPxComboBox ID="cmbMultiBranches" ClientInstanceName="CmbBranch" runat="server" Visible="false"
                                                        ValueType="System.String" DataSourceID="branchdtl" ValueField="branch_id"
                                                        TextField="branch_description" EnableIncrementalFiltering="true"
                                                        Width="90%" AutoPostBack="false">
                                                        <ClientSideEvents SelectedIndexChanged="CmbBranchChanged" Init="CmbBranchChanged" />
                                                    </dxe:ASPxComboBox>
                                                    <input type="button" onclick="MultiBranchClick()" class="btn btn-small btn-primary" value="Select Branch(s)" id="MultiBranchButton" />
                                                </div>
                                            </div>




                                            <div class="clear"></div>

                                            <div class="col-md-3">
                                                <label>Fleet Card Applied?</label>
                                                <%--Rev 1.0: "simple-select" class add --%>
                                                <div class="relative simple-select">
                                                    <asp:DropDownList ID="ddlFleetCardApplied" runat="server" Width="100%" TabIndex="13">
                                                        <asp:ListItem Value="0">Yes</asp:ListItem>
                                                        <asp:ListItem Value="1" Selected="True">No</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Fleet Card No.</label>
                                                <div>
                                                    <asp:TextBox ID="txtFleetCardNo" runat="server" ClientIDMode="Static" Width="100%" MaxLength="100" TabIndex="14">        
                                                    </asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Happy Card</label>
                                                <div>
                                                    <asp:TextBox ID="txtHappyCard" runat="server" ClientIDMode="Static" Width="100%" MaxLength="100" TabIndex="15">        
                                                    </asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="clear"></div>

                                            <div class="col-md-3">
                                                <label>Insurer Name</label>
                                                <div>
                                                    <asp:TextBox ID="txtInsName" runat="server" ClientIDMode="Static" Width="100%" MaxLength="100" TabIndex="16">        
                                                    </asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Policy No.</label>
                                                <div>
                                                    <asp:TextBox ID="txtInsPolicyNo" runat="server" ClientIDMode="Static" Width="100%" MaxLength="50" TabIndex="17">        
                                                    </asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <%--Insurance Valid upto--%>
                                                <div class="">
                                                    <dxe:ASPxLabel ID="ASPxLabelInsValidUpto" runat="server" Text="Valid upto" TabIndex="18">
                                                    </dxe:ASPxLabel>

                                                </div>
                                                <%--Insurance Valid upto--%>
                                                <%--Rev 1.0: "for-cust-icon" class add --%>
                                                <div class="for-cust-icon">
                                                    <dxe:ASPxDateEdit ID="txtInsValidUpto" runat="server" Width="100%" EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                                        UseMaskBehavior="True" TabIndex="19">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                    <%--Rev 1.0--%>
                                                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                                    <%--Rev end 1.0--%>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Insurance Given To</label>
                                                <div>
                                                    <asp:TextBox ID="txtInsGivenTo" runat="server" ClientIDMode="Static" Width="100%" MaxLength="200" TabIndex="20">        
                                                    </asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="clear"></div>

                                            <div class="col-md-3">
                                                <label>Tax Token No.</label>
                                                <div>
                                                    <asp:TextBox ID="txtTaxTokenNo" runat="server" ClientIDMode="Static" Width="100%" MaxLength="50" TabIndex="21">        
                                                    </asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <%--Tax Token Valid upto--%>
                                                <div class="">
                                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Tax Token Valid upto" TabIndex="22">
                                                    </dxe:ASPxLabel>
                                                </div>
                                                <%--Tax Token  Valid upto--%>
                                                <%--Rev 1.0: "for-cust-icon" class add --%>
                                                <div class="for-cust-icon">
                                                    <dxe:ASPxDateEdit ID="ASPxDateEditTaxValidUpto" runat="server" Width="100%" EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                                        UseMaskBehavior="True" TabIndex="23">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                    <%--Rev 1.0--%>
                                                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                                    <%--Rev end 1.0--%>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Pollution</label>
                                                <div>
                                                    <asp:TextBox ID="txtPollution" runat="server" ClientIDMode="Static" Width="100%" MaxLength="50" TabIndex="24">        
                                                    </asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <%--Pollution Valid upto--%>
                                                <div class="">
                                                    <dxe:ASPxLabel ID="ASPxLblPollutionValidUpto" runat="server" Text="Pollution Valid upto" TabIndex="25">
                                                    </dxe:ASPxLabel>

                                                </div>
                                                <%--Pollution Valid upto--%>
                                                <%--Rev 1.0: "for-cust-icon" class add --%>
                                                <div class="for-cust-icon">
                                                    <dxe:ASPxDateEdit ID="ASPxDateEditPolluValidUpto" runat="server" Width="100%" EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                                        UseMaskBehavior="True" TabIndex="26">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                    <%--Rev 1.0--%>
                                                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                                    <%--Rev end 1.0--%>
                                                </div>
                                            </div>
                                            <div class="clear"></div>

                                            <div class="col-md-3">
                                                <label>Pollution Case Details</label>
                                                <div>
                                                    <asp:TextBox ID="txtPollutionCaseDtl" runat="server" ClientIDMode="Static" Width="100%" MaxLength="1000" TabIndex="27">        
                                                    </asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Authorised Letter </label>
                                                <%--Rev 1.0: "simple-select" class add --%>
                                                <div class="relative simple-select">
                                                    <asp:DropDownList ID="ddlAuthLettr" runat="server" Width="100%" TabIndex="28">
                                                        <asp:ListItem Value="0">YES</asp:ListItem>
                                                        <asp:ListItem Value="1">NO</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <%--Authorised Letter Valid upto--%>
                                                <div class="">
                                                    <dxe:ASPxLabel ID="ASPxLabelAuthLttrValidUpto" runat="server" Text="Authorised Letter Valid upto">
                                                    </dxe:ASPxLabel>

                                                </div>
                                                <%--Authorised Letter Valid upto--%>
                                                <%--Rev 1.0: "for-cust-icon" class add --%>
                                                <div class="for-cust-icon">
                                                    <dxe:ASPxDateEdit ID="ASPxDateAuthLttrValidUpto" runat="server" Width="100%" EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                                        UseMaskBehavior="True" TabIndex="29">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                    <%--Rev 1.0--%>
                                                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                                    <%--Rev end 1.0--%>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Blue Book </label>
                                                <%--Rev 1.0: "simple-select" class add --%>
                                                <div class="relative simple-select">
                                                    <asp:DropDownList ID="ddlBlueBook" runat="server" Width="100%" TabIndex="30">
                                                        <asp:ListItem Value="0">YES</asp:ListItem>
                                                        <asp:ListItem Value="1">NO</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="clear"></div>

                                            <div class="col-md-3">
                                                <label>CF Case Details</label>
                                                <div>
                                                    <asp:TextBox ID="txtCFCaseDetails" runat="server" ClientIDMode="Static" Width="100%" MaxLength="1000" TabIndex="30">        
                                                    </asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <%--CF Valid upto--%>
                                                <div class="">
                                                    <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="CF Paper Valid upto">
                                                    </dxe:ASPxLabel>

                                                </div>
                                                <%--CF Valid upto--%>
                                                <%--Rev 1.0: "for-cust-icon" class add --%>
                                                <div class="for-cust-icon">
                                                    <dxe:ASPxDateEdit ID="ASPxDateCFValidUpto" runat="server" Width="100%" EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                                        UseMaskBehavior="True" TabIndex="31">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                    <%--Rev 1.0--%>
                                                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                                    <%--Rev end 1.0--%>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Fuel Type</label>
                                                <%--Rev 1.0: "simple-select" class add --%>
                                                <div class="relative simple-select">
                                                    <asp:DropDownList ID="ddlFuelType" runat="server" Width="100%" TabIndex="32">
                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                        <asp:ListItem Value="1">Petrol</asp:ListItem>
                                                        <asp:ListItem Value="2">Diesel</asp:ListItem>
                                                        <asp:ListItem Value="3">LPG</asp:ListItem>
                                                        <asp:ListItem Value="4">CNG</asp:ListItem>
                                                    </asp:DropDownList>

                                                </div>
                                            </div>
                                            <%--                                            <div class="col-md-3">
                                                <label>Is Active?</label>
                                                <div>
                                                    <dxe:ASPxCheckBox ID="chkIsActive" runat="server" Text="">
                                                    </dxe:ASPxCheckBox>
                                                </div>
                                            </div>--%>
                                            <div class="col-md-3">
                                                <label>Is Active?</label>
                                                <%--Rev 1.0: "simple-select" class add --%>
                                                <div class="relative simple-select">
                                                    <asp:DropDownList ID="ddlIsActive" runat="server" Width="100%" TabIndex="33">
                                                        <asp:ListItem Value="0">YES</asp:ListItem>
                                                        <asp:ListItem Value="1">NO</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="clear"></div>

                                            <div class="col-md-3">
                                                <label>Type</label><span style="color: red">*</span>
                                                <div class="relative">
                                                    BY Hand&nbsp;
                                                    <asp:CheckBox ID="chkByHand" runat="server" />
                                                </div>
                                            </div>

                                            <div class="clear"></div>

                                            <div class="col-md-12" style="padding-top: 10px;">

                                                <%--<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary dxbButton" OnClick="btnSave_Click" ValidationGroup="drivegrp"  OnClientClick="return ValidationCheck();" />--%>
                                                <dxe:ASPxButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="drivegrp" ClientInstanceName="cbtnSave" AutoPostBack="false" TabIndex="35">
                                                    <ClientSideEvents Click="fn_btnValidate" />
                                                </dxe:ASPxButton>
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger dxbButton" OnClick="btnCancel_Click" TabIndex="36" />
                                                <asp:Button ID="btnUdf" runat="server" Text="UDF" CssClass="btn btn-primary dxbButton" OnClientClick="if(OpenUdf()){ return false;}" TabIndex="37" />

                                            </div>
                                        </div>

                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>

                            <%-- DocumentAttach_110517 --%>
                            <dxe:TabPage Name="Documents" Text="Documents">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <%-- DocumentAttach_110517 --%>
                        </TabPages>
                        <ClientSideEvents ActiveTabChanged="function(s, e) {
                                    var activeTab   = page.GetActiveTab();
                                    var Tab0 = page.GetTab(0);
                                    var Tab1 = page.GetTab(1);
                                                                       
                                    if(activeTab == Tab0)
                                    {
                                    disp_prompt('tab0');
                                    }
                                    if(activeTab == Tab1)
                                    {
                                    disp_prompt('tab1');
                                    }
	                                           
                    }"></ClientSideEvents>
                        <ContentStyle>
                            <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                        </ContentStyle>
                        <LoadingPanelStyle ImageSpacing="6px">
                        </LoadingPanelStyle>
                    </dxe:ASPxPageControl>
                </td>
            </tr>
            <tr>
                <td style="height: 8px">
                    <table style="width: 100%;">
                        <tr>
                            <td align="right" style="width: 843px">
                                <asp:HiddenField ID="HdId" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

        </table>


        <asp:SqlDataSource ID="branchdtl" runat="server"
            SelectCommand="select '0' as branch_id ,  'Select' as branch_description union all   select branch_id,branch_description from tbl_master_branch order by branch_description"></asp:SqlDataSource>

        <asp:SqlDataSource ID="BranchdataSource" runat="server"
            SelectCommand="select branch_id,branch_code,branch_description from tbl_master_branch"></asp:SqlDataSource>

        <asp:HiddenField ID="hdnBranchAllSelected" runat="server" />
    </div>
    </div>

    <dxe:ASPxPopupControl ID="BranchSelectPopup" runat="server" Width="700"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cBranchSelectPopup"
        HeaderText="Select Branch" AllowResize="false" ResizingMode="Postponed" Modal="true">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                <%--<div>Selected Branches: <asp:Label ID="popSelectdBranches" runat="server"></asp:Label></div>--%>

                <div style="margin-bottom: 10px; margin-top: 10px;">
                    Alloted for All Branch &nbsp; 
                     <asp:CheckBox ID="chkAllBranch" runat="server" OnClick="SelectAllBranches(this);" />
                    <asp:Label ID="lblBranch" runat="server" Text="All Branch Selected, No need to select individual Branch" CssClass="vehiclecls"></asp:Label>
                </div>


                <dxe:ASPxGridView ID="branchGrid" runat="server" KeyFieldName="branch_id" AutoGenerateColumns="False" DataSourceID="BranchdataSource"
                    Width="100%" ClientInstanceName="cbranchGrid" OnCustomCallback="branchGrid_CustomCallback"
                    SelectionMode="Multiple" SettingsBehavior-AllowFocusedRow="true">
                    <Columns>

                        <dxe:GridViewCommandColumn ShowSelectCheckbox="true" VisibleIndex="0" Width="60" Caption="Select" />


                        <dxe:GridViewDataTextColumn Caption="Branch Code" FieldName="branch_code"
                            VisibleIndex="1" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Branch Description" FieldName="branch_description"
                            VisibleIndex="1" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>





                    </Columns>


                    <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                        <FirstPageButton Visible="True">
                        </FirstPageButton>
                        <LastPageButton Visible="True">
                        </LastPageButton>
                    </SettingsPager>
                    <SettingsSearchPanel Visible="True" />
                    <Settings ShowGroupPanel="False" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                    <SettingsLoadingPanel Text="Please Wait..." />
                    <ClientSideEvents EndCallback="branchGridEndCallBack" />
                </dxe:ASPxGridView>
                <br />
                <input type="button" value="Ok" class="btn btn-primary" onclick="SaveSelectedBranch()" />
                <div style="float: right;">
                    <input type="button" runat="server" value="Select All" onclick="selectAll()" />
                    <input type="button" runat="server" value="Deselect All" onclick="unselectAll()" />
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

</asp:Content>
