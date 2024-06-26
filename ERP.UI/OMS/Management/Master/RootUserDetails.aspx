<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                20-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="Users" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false"
    Inherits="ERP.OMS.Management.Master.management_master_RootUserDetails" CodeBehind="RootUserDetails.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script type="text/javascript">

        FieldName = 'test';
        /*Code  Added  By Priti on 20122016 to use jquery Choosen*/
        $(document).ready(function () {
            ListBind();
            ChangeSource();


            $("#txtpassword").keyup(function () {
                check_pass();
            });

            $(<%=txtuserid.ClientID%>).blur(function () {


                ($(<%=txtuserid.ClientID%>).val(($(<%=txtuserid.ClientID%>).val().trim())));

                if ($("#hdnEmailMadatory").val() == "Yes") {
                    if (!validateEmail($(<%=txtuserid.ClientID%>).val())) {
                        if ($("#emailmandate").hasClass("hide")) {
                            $("#emailmandate").removeClass("hide");

                        }
                        $(<%=txtuserid.ClientID%>).focus();
                        $(<%=btnUpdate.ClientID%>).prop("disabled", true);
                    }
                    else {
                        if (!$("#emailmandate").hasClass("hide")) {
                            $("#emailmandate").addClass("hide");
                            $(<%=btnUpdate.ClientID%>).prop("disabled", false);
                        }
                    }
                }
            });

        });


        function validateEmail(emailField) {
            var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;

            if (reg.test(emailField) == false) {

                return false;
            }

            return true;

        }


        function check_pass() {
            var val = document.getElementById("txtpassword").value;
            var meter = document.getElementById("meter");
            var no = 0;
            if (val != "") {
                // If the password length is less than or equal to 6
                if (val.length <= 6) no = 1;

                // If the password length is greater than 6 and contain any lowercase alphabet or any number or any special character
                if (val.length > 6 && (val.match(/[a-z]/) || val.match(/\d+/) || val.match(/.[!,@,#,$,%,^,&,*,?,_,~,-,(,)]/))) no = 2;

                // If the password length is greater than 6 and contain alphabet,number,special character respectively
                if (val.length > 6 && ((val.match(/[a-z]/) && val.match(/\d+/)) || (val.match(/\d+/) && val.match(/.[!,@,#,$,%,^,&,*,?,_,~,-,(,)]/)) || (val.match(/[a-z]/) && val.match(/.[!,@,#,$,%,^,&,*,?,_,~,-,(,)]/)))) no = 3;

                // If the password length is greater than 6 and must contain alphabets,numbers and special characters
                if (val.length > 6 && val.match(/[a-z]/) && val.match(/\d+/) && val.match(/.[!,@,#,$,%,^,&,*,?,_,~,-,(,)]/)) no = 4;

                $('#hdPassstrength').val(no);

                if (no == 1) {
                    $("#meter").animate({ width: '50px' }, 300);
                    meter.style.backgroundColor = "red";
                    document.getElementById("pass_type").innerHTML = "Very Weak";
                    document.getElementById("pass_type").style.color = "red";
                }

                if (no == 2) {
                    $("#meter").animate({ width: '100px' }, 300);
                    meter.style.backgroundColor = "#F5BCA9";
                    document.getElementById("pass_type").innerHTML = "Weak";
                    document.getElementById("pass_type").style.color = "#F5BCA9";
                }

                if (no == 3) {
                    $("#meter").animate({ width: '150px' }, 300);
                    meter.style.backgroundColor = "#FF8000";
                    document.getElementById("pass_type").innerHTML = "Good";
                    document.getElementById("pass_type").style.color = "#FF8000";
                }

                if (no == 4) {
                    $("#meter").animate({ width: '200px' }, 300);
                    meter.style.backgroundColor = "#00FF40";
                    document.getElementById("pass_type").innerHTML = "Strong";
                    document.getElementById("pass_type").style.color = "#00FF40";
                }
            }

            else {
                meter.style.backgroundColor = "white";
                document.getElementById("pass_type").style.color = "white";
                document.getElementById("pass_type").innerHTML = "";

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
        function lstAssociatedEmployee() {

            $('#lstAssociatedEmployee').fadeIn();

        }
        function setvalue() {
          //  document.getElementById("txtReportTo_hidden").value = document.getElementById("lstAssociatedEmployee").value;
            var userid = $(<%=txtuserid.ClientID%>).val();
            if ($("#txtusername").val().trim() != "") {
                if ($(<%=txtuserid.ClientID%>).val().trim() != "") {
                    if ($("#hdnEntryMode").val() == "Add") {
                        if ($("#txtpassword").val().trim() != "") {
                            if ($("#lstAssociatedEmployee").val() != null) {
                                if ($("#ddlGroups").val() != "Select Group") {

                                    document.getElementById("txtReportTo_hidden").value = document.getElementById("lstAssociatedEmployee").value;
                                }
                                else {
                                    jAlert("Please select group.", "Alert", function () {
                                        $("#ddlGroups").focus();
                                    });
                                    return false;
                                }
                            }
                            else {
                                jAlert("Please select associated employee.", "Alert", function () {
                                    $("#lstAssociatedEmployee").focus();
                                });
                                return false;
                            }
                        }
                        else {
                            jAlert("Please enter password.", "Alert", function () {
                                $("#txtpassword").focus();
                            });
                            return false;
                        }
                    }
                    else {
                        if ($("#lstAssociatedEmployee").val() != null) {
                            if ($("#ddlGroups").val() != "Select Group") {

                                document.getElementById("txtReportTo_hidden").value = document.getElementById("lstAssociatedEmployee").value;

                            }
                            else {
                                jAlert("Please select group.", "Alert", function () {
                                    $("#ddlGroups").focus();
                                });
                                return false;
                            }
                        }
                        else {
                            jAlert("Please select associated employee.", "Alert", function () {
                                $("#lstAssociatedEmployee").focus();
                            });
                            return false;
                        }
                    }
                }
                else {
                    jAlert("Please enter login id.", "Alert", function () {
                        $(<%=txtuserid.ClientID%>).focus();
                    });
                    return false;
                }
            }
            else {
                jAlert("Please enter user name.", "Alert", function () {
                    $("#txtusername").focus();
                });
                return false;
            }
        }
        function Changeselectedvalue() {
            var lstAssociatedEmployee = document.getElementById("lstAssociatedEmployee");
            if (document.getElementById("txtReportTo_hidden").value != '') {
                for (var i = 0; i < lstAssociatedEmployee.options.length; i++) {
                    if (lstAssociatedEmployee.options[i].value == document.getElementById("txtReportTo_hidden").value) {
                        lstAssociatedEmployee.options[i].selected = true;
                    }
                }
                $('#lstAssociatedEmployee').trigger("chosen:updated");
            }

        }
        function ChangeSource() {
            var fname = "%";
            var lAssociatedEmployee = $('select[id$=lstAssociatedEmployee]');
            lAssociatedEmployee.empty();


            $.ajax({
                type: "POST",
                url: "RootUserDetails.aspx/ALLEmployee",
                data: JSON.stringify({ reqStr: fname }),
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

                            $('#lstAssociatedEmployee').append($('<option>').text(name).val(id));

                        }

                        $(lAssociatedEmployee).append(listItems.join(''));

                        lstAssociatedEmployee();
                        $('#lstAssociatedEmployee').trigger("chosen:updated");

                        Changeselectedvalue();

                    }
                    else {
                        //   alert("No records found");
                        //lstReferedBy();
                        $('#lstAssociatedEmployee').trigger("chosen:updated");

                    }
                }
            });
            // }
        }
        //....end......
        function CallList(obj1, obj2, obj3) {
            //var obj5 = '';
            //if (obj5 != '18') {
            //    ajax_showOptions(obj1, obj2, obj3, obj5);
            //}
            if (obj1.value == "") {
                obj1.value = "%";
            }
            var obj5 = '';
            if (obj5 != '18') {
                ajax_showOptionsTEST(obj1, obj2, obj3, obj5);
                if (obj1.value == "%") {
                    obj1.value = "";
                }
            }
        }
        function Close() {
            parent.editwin.close();
        }
        function Cancel_Click() {
            //parent.editwin.close();
            location.href = '/OMS/Management/Master/root_user.aspx';
        }
    </script>

    <script type="text/javascript" language="javascript">
        // WRITE THE VALIDATION SCRIPT IN THE HEAD TAG.
        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
                return false;

            return true;
        }


    </script>
    <style>
        .reltv {
            position: relative;
        }

        .spl {
            position: absolute;
            right: -17px;
            top: 5px;
        }

        .inb {
            display: inline-block !important;
            width: 62px !important;
        }
        /*Code  Added  By Priti on 20122016 to use jquery Choosen*/
        .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }

        #lstAssociatedEmployee {
            width: 100%;
            display: none !important;
        }

        #display:none !important;_chosen {
            width: 100% !important;
        }

        .dxtcLite_PlasticBlue > .dxtc-content {
            overflow: visible !important;
        }
        /*...end....*/

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
        .TableMain100 #GrdHolidays , #StateGrid
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
            <h3>Add/Edit User </h3>
        </div>
        <div class="crossBtn"><a href="root_user.aspx"><i class="fa fa-times"></i></a></div>
    </div>
        <div class="form_main">
        <div class="row">
            <%--<asp:Button id="brnChangeUsersPassword" runat="server" Text="Change Password" OnClick="lnkChangePassword_Click" CssClass="btn btn-primary" />--%>
        </div>
        <div class="row">
            <div class="col-md-3">
                <label>User Name&nbsp;<em style="color: red">*</em> :</label>
                <div class="reltv">
                    <asp:TextBox ID='txtusername' runat="server" Width="100%" ValidationGroup="a" MaxLength="50"></asp:TextBox>
                   <%-- <asp:RequiredFieldValidator ID="rfvuserName" runat="server" ControlToValidate="txtusername" CssClass="spl fa fa-exclamation-circle iconRed"
                        ErrorMessage="" ValidationGroup="a" ToolTip="Mandatory."></asp:RequiredFieldValidator>--%>

                </div>
            </div>
            <div class="col-md-3">
                <label>Login Id&nbsp;<em style="color: red">*</em> :</label>
                  <asp:Label ID="lblLoginId" runat="server"></asp:Label>
                <div class="reltv">
                    <asp:TextBox ID='txtuserid' ClientIDMode="AutoID" runat="server" Width="100%" ValidationGroup="a" value="" MaxLength="50" autocomplete="off"></asp:TextBox>
                    <span id="emailmandate" class="danger hide">Please enter a valid Email id</span>
                  <%--  <asp:RequiredFieldValidator ID="rfvLiginId" runat="server" ControlToValidate="txtuserid" CssClass="spl fa fa-exclamation-circle iconRed"
                        ErrorMessage="" ToolTip="Mandatory." ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                </div>
            </div>
            <div class="col-md-3" id="user_password" runat="server" visible="true">
                <asp:HiddenField ID="hdPassstrength" runat="server" />
                <label>Password&nbsp;<em style="color: red">*</em> :</label><span id="pass_type" style="font-weight: 800; padding-left: 15px;"></span>
                <div class="reltv">
                    <asp:TextBox ID='txtpassword' runat="server" Width="100%" TextMode="Password" ValidationGroup="a" MaxLength="50"></asp:TextBox>
                   <%-- <asp:RequiredFieldValidator ID="rfvPass" runat="server" ControlToValidate="txtpassword" CssClass="spl fa fa-exclamation-circle iconRed"
                        ErrorMessage="" ToolTip="Mandatory." ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                </div>
                <div id="meter" style="height: 5px;"></div>
            </div>
            <div class="col-md-3">
                <label>Associated Employee <em style="color: red">*</em> :</label>
                <div class="reltv">
                    <%--  Code  Added and Commented By Priti on 20122016 to use ListBox instead of TextBox--%>
                    <%--  <asp:TextBox ID="txtReportTo" runat="server" Width="200px" autocomplete="off"></asp:TextBox>--%>
                    <asp:ListBox ID="lstAssociatedEmployee" CssClass="chsn" runat="server" Width="100%" data-placeholder="Select..."></asp:ListBox>
                  <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="lstAssociatedEmployee" CssClass="spl fa fa-exclamation-circle iconRed"
                        ErrorMessage="" ToolTip="Mandatory." ValidationGroup="a"></asp:RequiredFieldValidator>--%>

                    <asp:HiddenField ID="txtReportTo_hidden" runat="server" />
                </div>
            </div>
            <div class="clear"></div>
            <div class="col-md-3" style="display: none">
                <label>Data Entry Profile :</label>
                <div>
                    <asp:DropDownList ID="ddDataEntry" runat="server" Width="100%">
                        <asp:ListItem Value="F">Final</asp:ListItem>
                        <asp:ListItem Value="P">Provisional</asp:ListItem>
                        <asp:ListItem Value="M">Maker</asp:ListItem>
                        <asp:ListItem Value="C">Checker</asp:ListItem>
                        <asp:ListItem Value="R">Read Only</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-3" style="display: none;">
                <label>
                    Allow Access From(IP) :
                </label>
                <div>
                    <asp:TextBox ID="txtIp1" runat="server" CssClass="inb" MaxLength="3" onkeypress="javascript:return isNumber (event)"></asp:TextBox><font>.</font>
                    <asp:TextBox ID="txtIp2" runat="server" CssClass="inb" MaxLength="3" onkeypress="javascript:return isNumber (event)"></asp:TextBox><font>.</font>
                    <asp:TextBox ID="txtIp3" runat="server" CssClass="inb" MaxLength="3" onkeypress="javascript:return isNumber (event)"></asp:TextBox><font>.</font>
                    <asp:TextBox ID="txtIp4" runat="server" CssClass="inb" MaxLength="3" onkeypress="javascript:return isNumber (event)"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-3" style="display: none;">
                <label>&nbsp</label>
                <div>
                    <dxe:ASPxCheckBox ID="cbSuperUser" runat="server" Text="Super User">
                    </dxe:ASPxCheckBox>
                </div>
            </div>
            <div class="col-md-3" style="display: none">

                <asp:GridView ID="grdUserAccess" runat="Server" Visible="false" AutoGenerateColumns="False" BorderColor="CornflowerBlue"
                    BackColor="White" BorderStyle="Solid" BorderWidth="2px" CellPadding="4" Width="100%"
                    OnRowDataBound="grdUserAccess_RowDataBound">
                    <RowStyle BackColor="LightSteelBlue" ForeColor="#330099"></RowStyle>
                    <SelectedRowStyle BackColor="LightSteelBlue" ForeColor="SlateBlue" Font-Bold="True"></SelectedRowStyle>
                    <PagerStyle BackColor="LightSteelBlue" ForeColor="SlateBlue" HorizontalAlign="Center"></PagerStyle>
                    <HeaderStyle BackColor="LightSteelBlue" ForeColor="Black" Font-Bold="True"></HeaderStyle>
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSegmentId" runat="server" />
                                <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id")%>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle />
                            <HeaderStyle />
                        </asp:TemplateField>
                        <asp:BoundField DataField="SegmentName" HeaderText="Segment">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Security Role">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle />
                            <ItemTemplate>
                                <asp:DropDownList ID="drpUserGroup" runat="server" AppendDataBoundItems="True" Width="250px">
                                    <asp:ListItem Value="0">None</asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:DropDownList ID='dropdownlistbranch' runat="Server" AppendDataBoundItems="True"
                    Visible="false">
                    <asp:ListItem Value="0">None</asp:ListItem>
                </asp:DropDownList>

            </div>
            <div class="col-md-3">
                <label>Security Role<em style="color: red">*</em> :</label>
                <div class="reltv">
                    <asp:DropDownList ID="ddlGroups" runat="server" Width="100%"></asp:DropDownList>
                   <%-- <asp:RequiredFieldValidator ID="rfGroup" runat="server" ControlToValidate="ddlGroups" InitialValue="Select Security Role"
                        ErrorMessage="" ToolTip="Mandatory." ValidationGroup="a" CssClass="spl fa fa-exclamation-circle iconRed"></asp:RequiredFieldValidator>--%>
                </div>
            </div>
            <div class="col-md-1 lblmTop8">
                <label>Inactive?</label>
                <div>
                    <dxe:ASPxCheckBox ID="chkIsActive" runat="server" Text="">
                    </dxe:ASPxCheckBox>
                </div>
            </div>


            <div class="col-md-2 lblmTop8" style="display: none;">
                <label>Is Mac Lock?</label>
                <div>
                    <dxe:ASPxCheckBox ID="chkmac" runat="server" Text="">
                    </dxe:ASPxCheckBox>
                </div>
            </div>
            <div class="col-md-2 lblmTop8">
                <label>Userwise Data Display?</label>
                <div>
                    <dxe:ASPxCheckBox ID="chkUserWiseView" runat="server" Text="">
                    </dxe:ASPxCheckBox>
                </div>
            </div>
           <%-- Rev work start 12.05.2022 Show in Assign To in Lead
               Mantise Issue :0024839: Enhancement in LEAD module (C. Provide a Check Box in the User Master "Show in Assign To in Lead".)--%>
             <div class="col-md-2 lblmTop8">
                <label>Show in Assign To in Lead?</label>
                <div>
                    <dxe:ASPxCheckBox ID="chkLeadAssign" runat="server" Text="">
                    </dxe:ASPxCheckBox>
                </div>
            </div>
            <%-- Rev work close 12.05.2022 Show in Assign To in Lead
                Mantise Issue :0024839: Enhancement in LEAD module (C. Provide a Check Box in the User Master "Show in Assign To in Lead".)--%>
            <%--Start new settings for service management Tanmoy --%>
            <div class="col-md-2 lblmTop8" id="divServiceManagemnet" runat="server">
                <label>Default Service Dashboard?</label>
                <div>
                    <dxe:ASPxCheckBox ID="chkServiceManagement" runat="server" Text="">
                    </dxe:ASPxCheckBox>
                </div>
            </div>
            <%--End new settings for service management Tanmoy --%>

             <%--Start new settings for STB management Tanmoy --%>
            <div class="col-md-2 lblmTop8" id="divSTBManagemnet" runat="server">
                <label>Default STB Dashboard?</label>
                <div>
                    <dxe:ASPxCheckBox ID="chkSTBManagement" runat="server" Text="">
                    </dxe:ASPxCheckBox>
                </div>
            </div>
            <%--End new settings for STB management Tanmoy --%>

            <div class="clear"></div>
            <div class="col-md-12 mtc-10">
                <asp:Button ID="btnUpdate" runat="server" ClientIDMode="AutoID" CssClass="btn btn-primary" Text="Save" OnClick="btnUpdate_Click" OnClientClick="return setvalue()"
                    ValidationGroup="a" />
                <input id="btnCancel" type="button" class="btn btn-danger" value="Cancel" onclick="Cancel_Click()" />
            </div>
        </div>
        <table class="TableMain100" style="width: 550px">
            <%-- <tr>
            <td class="EHEADER" colspan="2" style="text-align: center;">
                <span style="color: #0000cc"><span style="text-decoration: underline"><strong>User Information</strong></span>&nbsp;</span></td>
        </tr>--%>
            <tr>
            </tr>
            <tr>
            </tr>

            <tr>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <table id="test" width="100%">
                        <tr>
                        </tr>
                        <tr>
                        </tr>
                        <tr style="display: none;">
                        </tr>
                        <tr>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
            </tr>
            <tr>
            </tr>
            <tr>
                <td style="">&nbsp;</td>

            </tr>
        </table>
    </div>
    </div>
    <asp:HiddenField ID="hdnEmailMadatory" runat="server" />
    <asp:HiddenField ID="hdnServicemanagement" runat="server" />
     <asp:HiddenField ID="hdnEntryMode" runat="server" />

     <asp:HiddenField ID="hdnSTBmanagement" runat="server" />
    <asp:HiddenField ID="hdnSyncUsertoWhileCreating" runat="server" />
</asp:Content>
