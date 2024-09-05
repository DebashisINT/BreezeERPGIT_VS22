<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                16-03-2023        2.0.36           Pallab              25733 : Master pages design modification
2.0                23-08-2024        2.0.44           Priti               0027615:Auto email of the Sales Invoice should consider CC email recipients too from the Customer.
====================================================== Revision History =============================================--%>

<%@ Page Title="Correspondence" Language="C#" AutoEventWireup="True" MasterPageFile="~/OMS/MasterPage/Erp.Master"
    Inherits="ERP.OMS.Management.Master.management_Master_Contact_Correspondence" CodeBehind="Contact_Correspondence.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /*.dxeErrorCell_PlasticBlue  {
            position: absolute;
            margin-top: 6px;
        }*/
        .dxeErrorFrameSys.dxeErrorCellSys, .dxeErrorFrameSys.dxeErrorCellSys {
            position: absolute !important;
        }

        .dxgvEditingErrorRow_PlasticBlue {
            background-color: #0820b7 !important;
            color: #f4f4f4 !important;
        }
    </style>
    <script lang="javascript" type="text/javascript">
        function DateValidateBirth() {
            var DOB = new Date(gridAddress.GetEditor("DateofBirth").GetDate());
            var monthnumber = DOB.getMonth();
            var monthday = DOB.getDate();
            var year = DOB.getYear();
            var SelectedDateValue = new Date(year, monthnumber, monthday);

            var Anniversary = new Date(gridAddress.GetEditor("AnniversaryDate").GetDate());
            var monthnumber = Anniversary.getMonth();
            var monthday = Anniversary.getDate();
            var year = Anniversary.getYear();
            var SelectedAnniversary = new Date(year, monthnumber, monthday);

            if (DOB != "") {
                if (Anniversary != "") {
                    if (SelectedDateValue.getTime() >= SelectedAnniversary.getTime()) {
                        gridAddress.GetEditor("AnniversaryDate").SetText("");
                        gridAddress.GetEditor("AnniversaryDate").Focus();
                    }
                }
            }
        }

        function DistanceEnableDisable() {
            var AddressType = gridAddress.GetEditor("Type").GetValue();
            if (AddressType == "Billing" || AddressType == "Shipping") {
                var Distance = gridAddress.GetEditor("Distance");
                Distance.SetEnabled(true);
            }
            else {
                var Distance = gridAddress.GetEditor("Distance");
                Distance.SetValue("0.00");
                Distance.SetEnabled(false);
            }
        }
        function init_distance() {
            DistanceEnableDisable();
        }
        function onChangeType() {
            DistanceEnableDisable();
        }

        function ul() {
            window.opener.document.getElementById('iFrmInformation').setAttribute('src', 'CallUserInformation.aspx')
        }

        function OnCountryChanged(cmbCountry) {

            gridAddress.GetEditor("City").PerformCallback('0');
            gridAddress.GetEditor("area").PerformCallback('0');
            gridAddress.GetEditor("PinCode").PerformCallback('0');// change cmdcountry.getvalue() to 0 on 07122016
            gridAddress.GetEditor("State").PerformCallback(cmbCountry.GetValue().toString());

            // alert("asda");

        }
        function OnStateChanged(cmbState) {

            gridAddress.GetEditor("area").PerformCallback('0');
            //gridAddress.GetEditor("City").PerformCallback('0');
            gridAddress.GetEditor("PinCode").PerformCallback('0');
            gridAddress.GetEditor("City").PerformCallback(cmbState.GetValue().toString());

        }
        function OnCityChanged(cmbCity) {
            gridAddress.GetEditor("area").PerformCallback(cmbCity.GetValue().toString());
            gridAddress.GetEditor("PinCode").PerformCallback(cmbCity.GetValue().toString());
        }

        //................ Code Added by Sam on 20102016..................
        function OnEmilTypeChanged(emiltype) {
            var tes = emiltype.GetValue().toString();
            gridEmail.PerformCallback(emiltype.GetValue().toString());
        }

        function OnPhoneTypeChanged(phonetype) {

            //var ph = phf_type.GetValue().toString();

            //alert(tes);

            gridPhone.PerformCallback(phf_type.GetValue().toString());
        }

        function HidePopupAndShowInfo() {
            popupan.Hide();

        }





        //<dxe:ListEditItem Text="Official (For sending Emails)" Value="Official"></dxe:ListEditItem>
        //                                                                            <dxe:ListEditItem Text="Personal" Value="Personal"></dxe:ListEditItem>
        //                                                                            <dxe:ListEditItem Text="Web Site" Value="Web Site"></dxe:ListEditItem>




        //................ Code Added by Sam on 20102016..................
        function OnChildCall(cmbCity) {
            OnCityChanged(gridAddress.GetEditor("City"));
        }
        function openAreaPage() {
            var left = (screen.width - 300) / 2;
            var top = (screen.height - 250) / 2;
            var cityid = gridAddress.GetEditor("City").GetValue();
            var cityname = gridAddress.GetEditor("City").GetText();

            if (cityid == null) {
                jAlert('Please select City to add an area');
            }
            else {
                var URL = 'AddArea_PopUp.aspx?id=' + cityid + '&name=' + cityname + '';
                popupan.SetContentUrl(URL);
                popupan.Show();
            }


        }
        function disp_prompt(name) {
            if (name == "tab0") {
                document.location.href = "Contact_general.aspx";
            }
            if (name == "tab1") {
                //document.location.href="Contact_Correspondence.aspx"; 
            }
            else if (name == "tab2") {
                //alert(name);
                document.location.href = "Contact_BankDetails.aspx";
            }
            else if (name == "tab3") {
                //alert(name);
                document.location.href = "Contact_DPDetails.aspx";
            }
            else if (name == "tab4") {
                //alert(name);
                document.location.href = "Contact_Document.aspx";
            }
            else if (name == "tab12") {
                //alert(name);
                document.location.href = "Contact_FamilyMembers.aspx";
            }
            else if (name == "tab5") {
                //alert(name);
                document.location.href = "Contact_Registration.aspx";
            }
            else if (name == "tab7") {
                //alert(name);
                document.location.href = "Contact_GroupMember.aspx";
            }
            else if (name == "tab8") {
                //alert(name);
                document.location.href = "Contact_Deposit.aspx";
            }
            else if (name == "tab9") {
                //alert(name);
                document.location.href = "Contact_Remarks.aspx";
            }
            else if (name == "tab10") {
                //alert(name);
                document.location.href = "Contact_Education.aspx";
            }
            else if (name == "tab11") {
                //alert(name);
                document.location.href = "contact_brokerage.aspx";
            }
            else if (name == "tab6") {
                //alert(name);
                document.location.href = "contact_other.aspx";
            }
            else if (name == "tab13") {
                document.location.href = "contact_Subscription.aspx";
            }

        }
        function OnPhoneClick() {

            gridPhone.UpdateEdit();

        }
        function Errorcheckingonendcallback(s, e) {

            if (gridAddress.cperror != null) {
                jAlert(gridAddress.cperror);
                gridAddress.cperror = null
                return false;
            }
        }
        function OnEmailClick() {
            if (gridEmail.GetEditor('eml_type').GetValue() == 'Web Site') {
                if (gridEmail.GetEditor('eml_website').GetValue() == null)
                    jAlert('Url Required');
                else
                    gridEmail.UpdateEdit();
            }
            else {
                if (gridEmail.GetEditor('eml_email').GetValue() == null)
                    jAlert('Email Required');
                else
                    gridEmail.UpdateEdit();
            }
        }
        function Emailcheck(obj) {
            //chinmoy  comment 12-04-2019
            //start

            //if (obj == 'c') {
            //    jAlert("This email id has already exists for other contacts.");
            //}

            //end
        }


        //-----------For Address Status -------------------
        function btnSave_Click() {
            var obj = 'SaveOld~' + RowID;
            popPanel.PerformCallback(obj);

        }

        function OnAddEditClick(e, obj) {
            var data = obj.split('~');
            if (data.length > 1)
                RowID = data[1];
            popup.Show();
            popPanel.PerformCallback(obj);
        }
        function EndCallBack(obj) {
            if (obj == 'Y') {
                popup.Hide();
                jAlert("Successfully update");
                gridAddress.PerformCallback();
            }
            else if (obj == 'Y1') {
                var msg = 'Cannot proceed. Registered/Permanent Address is already exist as "Active".\n You can set only one Registered/Permanent Address as "Active". ';
                alert(msg);//added by sanjib 20122016

            }
            else if (obj == 'Y2') {
                var msg = 'Cannot proceed. Residence Address is already exist as "Active".\n You can set only one Residence Address as "Active".';
                alert(msg);//added by sanjib 20122016
            }
            else if (obj == 'Y3') {
                var msg = 'Cannot proceed. Office Address is already exist as "Active".\n You can set only one Office Address as "Active".';
                alert(msg);//added by sanjib 20122016
            }



        }
        function btnCancel_Click() {
            popup.Hide();
        }

        //-----------For Phone Status -------------------
        function btnSave_ClickP() {
            var obj = 'SaveOld~' + RowIDP;
            popPanelP.PerformCallback(obj);

        }

        function OnAddEditClickP(e, obj) {

            var data = obj.split('~');
            if (data.length > 1)
                RowIDP = data[1];
            popupP.Show();
            popPanelP.PerformCallback(obj);
        }
        function EndCallBackP(obj) {
            if (obj == 'Y') {
                popupP.Hide();
                jAlert("Successfully Updated");
                gridPhone.PerformCallback();
            }


        }
        function btnCancel_ClickP() {
            popupP.Hide();
        }


        //-----------For Email Status -------------------
        function btnSave_ClickE() {
            var obj = 'SaveOld~' + RowIDE;
            popPanelE.PerformCallback(obj);

        }

        function OnAddEditClickE(e, obj) {

            var data = obj.split('~');
            if (data.length > 1)
                RowIDE = data[1];
            popupE.Show();
            popPanelE.PerformCallback(obj);
        }
        function EndCallBackE(obj) {
            if (obj == 'Y') {
                popupE.Hide();
                jAlert("Successfully Updated");
                gridEmail.PerformCallback();
            }


        }
        function btnCancel_ClickE() {
            popupE.Hide();
        }
        function AddressUpdate() {
            gridAddress.UpdateEdit();
        }

        function IsNumeric(strString)
            //  check for valid numeric strings	
        {
            var strValidChars = "0123456789";
            var strChar;
            var blnResult = true;

            if (strString.length == 0) return false;

            //  test strString consists of valid characters listed above
            for (i = 0; i < strString.length && blnResult == true; i++) {
                strChar = strString.charAt(i);
                if (strValidChars.indexOf(strChar) == -1) {
                    blnResult = false;
                }
            }
            return blnResult;
        }

        function fn_AllowonlyNumeric(s, e) {
            var theEvent = e.htmlEvent || window.event;
            var key = theEvent.keyCode || theEvent.which;
            var keychar = String.fromCharCode(key);
            if (key == 9 || key == 37 || key == 38 || key == 39 || key == 40 || key == 8 || key == 46) { //tab/ Left / Up / Right / Down Arrow, Backspace, Delete keys
                return;
            }
            var regex = /[0-9\b]/;

            if (!regex.test(keychar)) {
                theEvent.returnValue = false;
                if (theEvent.preventDefault)
                    theEvent.preventDefault();
            }
        }
        function fn_chekFbtRate(s, e) {

            //var type = gridAddress.GetEditor("Type").GetValue();
            //alert("type"+type);
            ////var ProductName = ctxtPro_Name.GetText();
            //var ischeck = gridAddress.GetEditor("Isdefault").GetValue();
            //alert(ischeck);
            //if (ischeck) {
            //    $.ajax({
            //        type: "POST",
            //        url: "Contact_Correspondence.aspx/Checkdefault",
            //        //data: "{'ProductName':'" + ProductName + "'}",
            //        data: JSON.stringify({ ischeck: ischeck, type: type, actiontype: "customerAddress" }),
            //        contentType: "application/json; charset=utf-8",
            //        dataType: "json",
            //        success: function (msg) {
            //            var data = msg.d;

            //            if (data == true) {
            //                jAlert("Default already set.");

            //                txtClentUcc.SetText("");
            //                //document.getElementById("Popup_Empcitys_ctxtPro_Code_I").focus();
            //                document.getElementById("txtClentUcc").focus();

            //                return false;
            //            }
            //        }

            //    });
            //} else {
            //    return true;
            //}
        }

        $(document).ready(function () {
            var mod = '<%= Session["Contactrequesttype"] %>';

            if (mod == 'customer') {
                document.getElementById("lnkClose").href = 'CustomerMasterList.aspx';
            }
            else if (mod == 'Transporter') {
                document.getElementById("lnkClose").href = 'TransporterMasterList.aspx?requesttype=<%= Session["Contactrequesttype"] %>';
            }
            else {
                document.getElementById("lnkClose").href = 'frmContactMain.aspx?requesttype=<%= Session["Contactrequesttype"] %>';
            }

        });
    </script>

    <style type="text/css">
        .dxeValidStEditorTable td.dxeErrorFrameSys.dxeErrorCellSys {
            position: absolute !important;
        }

        .dxeValidStEditorTable[errorframe="errorFrame"] {
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

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #txtDOB , #txtAnniversary
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1 , #txtDOB_B-1 , #txtAnniversary_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img ,
        #txtDOB_B-1 #txtDOB_B-1Img ,
        #txtAnniversary_B-1 #txtAnniversary_B-1Img
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
        select.btn
        {
            padding-right: 10px !important;
        }

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

        /*.TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridLocationwiseStockStatus 
        #B2B , 
        #grid_B2BUR , 
        #grid_IMPS , 
        #grid_IMPG ,
        #grid_CDNR ,
        #grid_CDNUR ,
        #grid_EXEMP ,
        #grid_ITCR ,
        #grid_HSNSUM
        {
            max-width: 98% !important;
        }*/

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

        select.btn
        {
           position: relative;
           z-index: 0;
        }

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
            <%--<h3>Contact Correspondence List</h3>--%>
            <h3>
                <asp:Label ID="lblHeadTitle" runat="server"></asp:Label>
            </h3>
            <%-- <div class="crossBtn"><a href="frmContactMain.aspx?requesttype=<%= Session["Contactrequesttype"] %>"><i class="fa fa-times"></i></a></div>--%>
            <div class="crossBtn"><a id="lnkClose"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
        <div class="form_main">
        <div>
            <table width="100%">
                <tr>
                    <td class="EHEADER" style="text-align: center">
                        <asp:Label ID="lblName" runat="server" Font-Size="12px" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
            </table>
            <table class="TableMain100">
                <tr>
                    <td>
                        <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="1" ClientInstanceName="page"
                            OnActiveTabChanged="ASPxPageControl1_ActiveTabChanged">
                            <TabPages>
                                <dxe:TabPage Name="General" Text="General">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Correspondence" Text="Correspondence">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                            <dxe:ASPxPageControl ID="ASPxPageControl2" runat="server" ActiveTabIndex="0" ClientInstanceName="page">
                                                <TabPages>
                                                    <dxe:TabPage Name="Adress" Text="Address">
                                                        <ContentCollection>
                                                            <dxe:ContentControl runat="server">
                                                                <div style="float: left;">
                                                                    <% if (rights.CanAdd)
                                                                       { %>
                                                                    <a href="javascript:void(0);" class="btn btn-primary" onclick="gridAddress.AddNewRow();"><span>Add New</span> </a>
                                                                    <% } %>
                                                                </div>

                                                                <div class="pull-left">
                                                                </div>

                                                                <dxe:ASPxGridView ID="AddressGrid" runat="server" DataSourceID="Address" ClientInstanceName="gridAddress"
                                                                    KeyFieldName="Id" AutoGenerateColumns="False" OnCellEditorInitialize="AddressGrid_CellEditorInitialize"
                                                                    Width="100%" OnCustomCallback="AddressGrid_CustomCallback" OnRowValidating="AddressGrid_RowValidating"
                                                                    OnCommandButtonInitialize="AddressGrid_CommandButtonInitialize" EnableRowsCache="false">
                                                                    <Columns>

                                                                        <dxe:GridViewDataTextColumn FieldName="Id" Visible="False" VisibleIndex="0" Caption="Id">
                                                                            <EditFormSettings Visible="False" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataCheckColumn FieldName="Isdefault" Visible="False" VisibleIndex="1" Caption="Default">
                                                                            <EditFormSettings Visible="True" />
                                                                            <PropertiesCheckEdit>
                                                                            </PropertiesCheckEdit>
                                                                        </dxe:GridViewDataCheckColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="contactperson" VisibleIndex="2" Caption="Contact Person">
                                                                            <EditFormSettings Visible="true" VisibleIndex="2" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataComboBoxColumn Caption="Address Type" FieldName="Type" Visible="False"
                                                                            VisibleIndex="3" Width="100%">
                                                                            <PropertiesComboBox ValueType="System.String">
                                                                                <Items>
                                                                                    <dxe:ListEditItem Text="Billing" Value="Billing" Selected="true"></dxe:ListEditItem>
                                                                                    <dxe:ListEditItem Text="Shipping" Value="Shipping"></dxe:ListEditItem>
                                                                                    <dxe:ListEditItem Text="Registered/Permanent Address" Value="Registered"></dxe:ListEditItem>
                                                                                    <dxe:ListEditItem Text="Residence" Value="Residence"></dxe:ListEditItem>
                                                                                    <dxe:ListEditItem Text="Office" Value="Office"></dxe:ListEditItem>
                                                                                    <dxe:ListEditItem Text="Factory/Work/Branch" Value="FactoryWorkBranch"></dxe:ListEditItem>
                                                                                </Items>
                                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { onChangeType(s); }"></ClientSideEvents>
                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">

                                                                                    <RequiredField IsRequired="True" ErrorText="Mandatory" />

                                                                                </ValidationSettings>

                                                                            </PropertiesComboBox>
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormSettings Visible="True" VisibleIndex="3" />
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataComboBoxColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="Type" VisibleIndex="4" Caption="Type">
                                                                            <EditFormSettings Visible="False" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="Address1" VisibleIndex="5" Caption="Address1" PropertiesTextEdit-MaxLength="100">
                                                                            <EditFormSettings Visible="True" VisibleIndex="5" />
                                                                            <CellStyle CssClass="gridcellleft abc">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                            <PropertiesTextEdit Width="100%"></PropertiesTextEdit>
                                                                        </dxe:GridViewDataTextColumn>

                                                                        <dxe:GridViewDataTextColumn FieldName="Address2" VisibleIndex="6" Caption="Address2" PropertiesTextEdit-MaxLength="100">
                                                                            <EditFormSettings Visible="True" VisibleIndex="6" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                            <PropertiesTextEdit Width="100%"></PropertiesTextEdit>
                                                                        </dxe:GridViewDataTextColumn>

                                                                        <dxe:GridViewDataTextColumn FieldName="Address3" VisibleIndex="6" Caption="Address3" PropertiesTextEdit-MaxLength="50" Visible="false">
                                                                            <EditFormSettings Visible="True" VisibleIndex="6" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                            <PropertiesTextEdit Width="100%"></PropertiesTextEdit>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="Address4" VisibleIndex="6" Caption="Address4" PropertiesTextEdit-MaxLength="50" Visible="false">
                                                                            <EditFormSettings Visible="True" VisibleIndex="6" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                            <PropertiesTextEdit Width="100%"></PropertiesTextEdit>
                                                                        </dxe:GridViewDataTextColumn>

                                                                        <dxe:GridViewDataTextColumn FieldName="LandMark" VisibleIndex="7" Caption="Landmark" PropertiesTextEdit-MaxLength="100"
                                                                            Visible="false">
                                                                            <EditFormSettings Visible="True" VisibleIndex="7" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                            <PropertiesTextEdit Width="100%"></PropertiesTextEdit>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="Phone" VisibleIndex="8" Caption="Phone" Visible="false">
                                                                            <EditFormSettings Visible="True" VisibleIndex="8" />
                                                                            <PropertiesTextEdit MaxLength="100">
                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">
                                                                                    <RequiredField IsRequired="True" ErrorText="Mandatory"></RequiredField>
                                                                                </ValidationSettings>

                                                                            </PropertiesTextEdit>

                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="add_Email" Caption="Email" Visible="false">
                                                                            <EditFormSettings Visible="True" VisibleIndex="9" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                            <PropertiesTextEdit MaxLength="200" Width="100%">
                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">
                                                                                    <RegularExpression ErrorText="Enter valid Email ID" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                                                </ValidationSettings>
                                                                            </PropertiesTextEdit>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataDateColumn FieldName="DateofBirth" Caption="Date of Birth" Visible="false">
                                                                            <EditFormSettings Visible="True" VisibleIndex="10" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                            <PropertiesDateEdit Width="100%" EditFormatString="dd-MM-yyyy">
                                                                                <ClientSideEvents  DateChanged="DateValidateBirth"></ClientSideEvents>
                                                                            </PropertiesDateEdit>
                                                                        </dxe:GridViewDataDateColumn>

                                                                        <dxe:GridViewDataDateColumn FieldName="AnniversaryDate" Caption="Anniversary date" Visible="false">
                                                                            <EditFormSettings Visible="True" VisibleIndex="11" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                            <PropertiesDateEdit Width="100%" EditFormatString="dd-MM-yyyy">
                                                                                   <ClientSideEvents  DateChanged="DateValidateBirth"></ClientSideEvents>
                                                                            </PropertiesDateEdit>
                                                                        </dxe:GridViewDataDateColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="add_Website" Caption="Website" Visible="false">
                                                                            <EditFormSettings Visible="True" VisibleIndex="12" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>

                                                                        <dxe:GridViewDataComboBoxColumn Caption="Designation" FieldName="add_Designation" Visible="False">
                                                                            <PropertiesComboBox DataSourceID="DesignationSelect" TextField="deg_designation" ValueField="deg_id"
                                                                                EnableSynchronization="False" EnableIncrementalFiltering="True" ValueType="System.String" ClearButton-DisplayMode="Always" ClearButton-ImagePosition="Right">
                                                                                <%-- <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                    <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                                                                </ValidationSettings>       --%>
                                                                            </PropertiesComboBox>
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormSettings Visible="True" VisibleIndex="13" />
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataComboBoxColumn>


                                                                        <dxe:GridViewDataComboBoxColumn Caption="Country" FieldName="Country" Visible="False">
                                                                            <PropertiesComboBox DataSourceID="CountrySelect" TextField="Country" ValueField="cou_id"
                                                                                EnableSynchronization="False" EnableIncrementalFiltering="True" ValueType="System.String">
                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                    <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                                                                </ValidationSettings>
                                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCountryChanged(s); }"></ClientSideEvents>
                                                                            </PropertiesComboBox>
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormSettings Visible="True" VisibleIndex="14" />
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataComboBoxColumn>

                                                                        <dxe:GridViewDataComboBoxColumn Caption="District" FieldName="City" Visible="False" Width="0">
                                                                            <PropertiesComboBox DataSourceID="SelectCity" TextField="City" ValueField="CityId" Width="100%"
                                                                                EnableSynchronization="False" EnableIncrementalFiltering="True" ValueType="System.String">
                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                    <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                                                                </ValidationSettings>
                                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCityChanged(s); }"></ClientSideEvents>
                                                                            </PropertiesComboBox>
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                            <EditFormSettings Visible="True" VisibleIndex="16" />
                                                                        </dxe:GridViewDataComboBoxColumn>
                                                                        <dxe:GridViewDataComboBoxColumn Caption="State" FieldName="State" Visible="False">
                                                                            <PropertiesComboBox DataSourceID="StateSelect" TextField="State" ValueField="ID" Width="100%"
                                                                                EnableSynchronization="False" EnableIncrementalFiltering="True" ValueType="System.String">
                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                    <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                                                                </ValidationSettings>
                                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { OnStateChanged(s); }"></ClientSideEvents>
                                                                            </PropertiesComboBox>
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormSettings Visible="True" VisibleIndex="15" />
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataComboBoxColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="Country1" VisibleIndex="17" Caption="Country" Visible="False">
                                                                            <EditFormSettings Visible="False" VisibleIndex="17" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="State1" VisibleIndex="19" Caption="State">
                                                                            <EditFormSettings Visible="False" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="City1" VisibleIndex="18" Caption="District">
                                                                            <EditFormSettings Visible="False" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataTextColumn Caption="Phone" FieldName="Phone" VisibleIndex="20" Visible="true">
                                                                            <EditFormSettings Visible="False" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataHyperLinkColumn Visible="false">
                                                                            <EditFormSettings VisibleIndex="21" Visible="true" />
                                                                            <EditItemTemplate>
                                                                                <a href="javascript:void(0);" onclick="openAreaPage();"><span class="Ecoheadtxt">
                                                                                    <strong>Add New Area</strong></span></a>
                                                                            </EditItemTemplate>
                                                                        </dxe:GridViewDataHyperLinkColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="PinCode1" VisibleIndex="22" Caption="Pincode / Zip" Visible="true">
                                                                            <EditFormSettings Visible="false" VisibleIndex="22" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <PropertiesTextEdit>
                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                    <RequiredField IsRequired="True" />
                                                                                </ValidationSettings>
                                                                            </PropertiesTextEdit>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataComboBoxColumn Caption="Pincode / Zip" FieldName="PinCode" Visible="false">
                                                                            <PropertiesComboBox DataSourceID="SelectPin" TextField="pin_code" ValueField="pin_id" Width="100%"
                                                                                EnableSynchronization="False" EnableIncrementalFiltering="True" ValueType="System.String" ClearButton-DisplayMode="Always" ClearButton-ImagePosition="Right">
                                                                                <ClearButton DisplayMode="Always" ImagePosition="Right"></ClearButton>
                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                    <RequiredField ErrorText="Mandatory" IsRequired="True" />
                                                                                </ValidationSettings>
                                                                            </PropertiesComboBox>
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormSettings VisibleIndex="23" Visible="True" />
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataComboBoxColumn>
                                                                        <dxe:GridViewDataComboBoxColumn FieldName="area" Visible="false">
                                                                            <PropertiesComboBox ValueType="System.Int32" DataSourceID="SelectArea" EnableSynchronization="False" Width="100%"
                                                                                EnableIncrementalFiltering="True" ValueField="area_id" TextField="areaname">
                                                                            </PropertiesComboBox>
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                            <EditFormSettings VisibleIndex="24" Visible="True" Caption="Area" />
                                                                        </dxe:GridViewDataComboBoxColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="Distance" VisibleIndex="25" Caption="Distance(Km)">
                                                                            <EditFormSettings Visible="true" VisibleIndex="25" />
                                                                            <PropertiesTextEdit>
                                                                                <ClientSideEvents Init="init_distance" />
                                                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                                            </PropertiesTextEdit>
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="status" VisibleIndex="26" Caption="Status">
                                                                            <DataItemTemplate>
                                                                                <a href="javascript:void(0);" onclick="OnAddEditClick(this,'Edit~'+'<%# Container.KeyValue %>')">
                                                                                    <dxe:ASPxLabel ID="ASPxTextBox2" runat="server" Text='<%# Eval("status")%>'
                                                                                        ToolTip="Click to Change Status">
                                                                                    </dxe:ASPxLabel>
                                                                                </a>
                                                                            </DataItemTemplate>
                                                                            <EditFormSettings Visible="False" />
                                                                            <CellStyle Wrap="False">
                                                                            </CellStyle>
                                                                            <HeaderTemplate>
                                                                                Status
                                                                            </HeaderTemplate>
                                                                            <HeaderStyle Wrap="False" />
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewCommandColumn VisibleIndex="27" ShowDeleteButton="true" ShowEditButton="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                            <HeaderTemplate>
                                                                                Actions                                                                               
                                                                            </HeaderTemplate>
                                                                        </dxe:GridViewCommandColumn>
                                                                    </Columns>
                                                                    <SettingsCommandButton>
                                                                        <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                                                                            <Image AlternateText="Edit" Url="../../../assests/images/Edit.png"></Image>
                                                                            <Styles>
                                                                                <Style CssClass="pad"></Style>
                                                                            </Styles>
                                                                        </EditButton>
                                                                        <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                                            <Image AlternateText="Delete" Url="../../../assests/images/Delete.png"></Image>
                                                                        </DeleteButton>
                                                                        <UpdateButton Text="Save" ButtonType="Button"></UpdateButton>
                                                                        <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger">
                                                                            <Styles>
                                                                                <Style CssClass="btn btn-danger"></Style>
                                                                            </Styles>
                                                                        </CancelButton>
                                                                    </SettingsCommandButton>
                                                                    <SettingsSearchPanel Visible="True" />
                                                                    <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                                                    <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="WindowCenter"
                                                                        PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="450px"
                                                                        EditFormColumnCount="1" />
                                                                    <Styles>
                                                                        <LoadingPanel ImageSpacing="10px">
                                                                        </LoadingPanel>
                                                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                        </Header>
                                                                    </Styles>
                                                                    <SettingsText PopupEditFormCaption="Add/Modify Address" ConfirmDelete="Confirm delete?" />
                                                                    <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                                                                        <FirstPageButton Visible="True">
                                                                        </FirstPageButton>
                                                                        <LastPageButton Visible="True">
                                                                        </LastPageButton>
                                                                    </SettingsPager>
                                                                    <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                                                                    <%--<ClientSideEvents EndCallback="function(s,e){addgrid_endcallback()}" />--%>
                                                                    <Templates>
                                                                        <TitlePanel>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td align="center" style="width: 50%">
                                                                                        <span class="Ecoheadtxt" style="color: Black">Add/Modify Address.</span>
                                                                                    </td>

                                                                                </tr>
                                                                            </table>
                                                                        </TitlePanel>
                                                                        <EditForm>
                                                                            <div style="color: red; margin-top: 5px; margin-left: 5px;">* Denotes the mandatory field.</div>
                                                                            <div style="margin: 8px 8px 0px 8px">
                                                                                <table style="width: 100%" style="">
                                                                                    <tr>
                                                                                        <td style="width: 5%;"></td>
                                                                                        <td style="width: 90%;">
                                                                                            <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors"></dxe:ASPxGridViewTemplateReplacement>
                                                                                            <div style="text-align: left; padding: 2px 2px 2px 110px">
                                                                                                <a id="update" href="#" onclick="AddressUpdate()" class="btn btn-primary " style="color: white; padding: 6px 18px !important;">Save</a>
                                                                                                <div class="dxbButton" style="display: inline-block; padding: 3px">
                                                                                                    <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                                                                        runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                                </div>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td style="width: 5%;"></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </EditForm>
                                                                    </Templates>
                                                                    <ClientSideEvents EndCallback="Errorcheckingonendcallback" />
                                                                </dxe:ASPxGridView>

                                                                <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ContentUrl="AddArea_PopUp.aspx"
                                                                    CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupan" Height="250px"
                                                                    Width="300px" HeaderText="Add New Area" AllowResize="true" ResizingMode="Postponed" Modal="true">
                                                                    <ContentCollection>
                                                                        <dxe:PopupControlContentControl runat="server">
                                                                        </dxe:PopupControlContentControl>
                                                                    </ContentCollection>
                                                                    <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
                                                                </dxe:ASPxPopupControl>
                                                                <dxe:ASPxPopupControl ID="ASPxPopupControl1" ClientInstanceName="popup" runat="server"
                                                                    AllowDragging="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="Set Address Status"
                                                                    EnableHotTrack="False" BackColor="#DDECFE" Width="400px" CloseAction="CloseButton">
                                                                    <ContentCollection>
                                                                        <dxe:PopupControlContentControl runat="server">
                                                                            <dxe:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="400px" ClientInstanceName="popPanel"
                                                                                OnCallback="ASPxCallbackPanel1_Callback" OnCustomJSProperties="ASPxCallbackPanel1_CustomJSProperties">
                                                                                <PanelCollection>
                                                                                    <dxe:PanelContent runat="server">
                                                                                        <table style="width: 100%">
                                                                                            <tr>
                                                                                                <td style="width: 25%">Status:
                                                                                                </td>
                                                                                                <td style="width: 50%">
                                                                                                    <asp:DropDownList ID="cmbStatus" runat="server" Width="180px" TabIndex="0">
                                                                                                        <asp:ListItem Text="Active" Value="Y"></asp:ListItem>
                                                                                                        <asp:ListItem Text="Deactive" Value="N"></asp:ListItem>
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td style="width: 25%">Date:
                                                                                                </td>
                                                                                                <td style="width: 50%">
                                                                                                    <dxe:ASPxDateEdit ID="StDate" runat="server" ClientInstanceName="StDate" EditFormat="Custom"
                                                                                                        UseMaskBehavior="True" Width="179px" TabIndex="1">
                                                                                                        <ButtonStyle Width="13px">
                                                                                                        </ButtonStyle>
                                                                                                    </dxe:ASPxDateEdit>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td style="width: 25%">Reason:
                                                                                                </td>
                                                                                                <td style="width: 75%">
                                                                                                    <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Width="250px" TabIndex="2" MaxLength="50"></asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td style="width: 25%"></td>
                                                                                                <td colspan="2" class="gridcellleft" style="width: 75%">
                                                                                                    <%-- <input id="Button1" type="button" value="Save" class="btnUpdate dxbButton" onclick="btnSave_Click()"
                                                                                                        style="width: 60px" tabindex="41" />--%>
                                                                                                    <input id="Button1" type="button" value="Save" class="btnUpdate btn btn-primary" onclick="btnSave_Click()"
                                                                                                        style="width: 60px" tabindex="3" />

                                                                                                    <%--<input id="Button2" type="button" value="Cancel" class="btnUpdate dxbButton" onclick="btnCancel_Click()"
                                                                                                        style="width: 60px" tabindex="42" />--%>
                                                                                                    <input id="Button2" type="button" value="Cancel" class="btnUpdate btn btn-danger" onclick="btnCancel_Click()"
                                                                                                        style="width: 60px" tabindex="4" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </dxe:PanelContent>
                                                                                </PanelCollection>
                                                                                <ClientSideEvents EndCallback="function(s, e) {
	                                                        EndCallBack(s.cpLast);
                                                        }" />
                                                                            </dxe:ASPxCallbackPanel>
                                                                        </dxe:PopupControlContentControl>
                                                                    </ContentCollection>
                                                                    <HeaderStyle HorizontalAlign="Left">
                                                                        <Paddings PaddingRight="6px" />
                                                                    </HeaderStyle>
                                                                    <SizeGripImage Height="16px" Width="16px" />
                                                                    <CloseButtonImage Height="12px" Width="13px" />
                                                                    <ClientSideEvents CloseButtonClick="function(s, e) {
	     popup.Hide();
    }" />
                                                                </dxe:ASPxPopupControl>
                                                            </dxe:ContentControl>
                                                        </ContentCollection>
                                                    </dxe:TabPage>




                                                    <dxe:TabPage Name="Phone" Text="Phone">

                                                        <ContentCollection>
                                                            <dxe:ContentControl runat="server">
                                                                <div style="float: left;">
                                                                    <% if (rights.CanAdd)
                                                                       { %>
                                                                    <a href="javascript:void(0);" class="btn btn-primary" onclick="gridPhone.AddNewRow();"><span>Add New</span> </a>

                                                                    <% } %>
                                                                </div>

                                                                <dxe:ASPxGridView ID="PhoneGrid" ClientInstanceName="gridPhone" DataSourceID="Phone"
                                                                    KeyFieldName="phf_id" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                    OnRowValidating="PhoneGrid_RowValidating" OnCustomCallback="PhoneGrid_CustomCallback"
                                                                    OnBeforeGetCallbackResult="PhoneGrid_BeforeGetCallbackResult" OnCommandButtonInitialize="PhoneGrid_CommandButtonInitialize">
                                                                    <SettingsSearchPanel Visible="True" />
                                                                    <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                                                    <Columns>
                                                                        <dxe:GridViewDataTextColumn FieldName="phf_id" ReadOnly="True" VisibleIndex="0"
                                                                            Visible="False">
                                                                            <EditFormSettings Visible="False" />
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="phf_cntId" ReadOnly="True" VisibleIndex="0"
                                                                            Visible="False">
                                                                            <EditFormSettings Visible="False" />
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="phf_entity" ReadOnly="True" VisibleIndex="0"
                                                                            Visible="False">
                                                                            <EditFormSettings Visible="False" />
                                                                        </dxe:GridViewDataTextColumn>

                                                                        <dxe:GridViewDataCheckColumn FieldName="Isdefault" Visible="False" Caption="Default">
                                                                            <EditFormSettings Visible="True" />
                                                                            <PropertiesCheckEdit>
                                                                                <%--  <ClientSideEvents CheckedChanged="function(s,e){fn_chekFbtRate(s,e);}"/>--%>
                                                                            </PropertiesCheckEdit>
                                                                        </dxe:GridViewDataCheckColumn>

                                                                        <dxe:GridViewDataTextColumn FieldName="phf_ContactPerson" VisibleIndex="1" Caption="Contact Person">
                                                                            <EditFormSettings Caption="Contact Person" Visible="True" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <PropertiesTextEdit MaxLength="100">

                                                                                <ClientSideEvents />
                                                                            </PropertiesTextEdit>

                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>

                                                                        <dxe:GridViewDataTextColumn FieldName="phf_ContactPersonDesignation" VisibleIndex="2" Caption="Designation">
                                                                            <EditFormSettings Visible="True" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <PropertiesTextEdit MaxLength="100">
                                                                            </PropertiesTextEdit>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <%--//////////////////////////////--%>
                                                                        <dxe:GridViewDataTextColumn FieldName="phf_type2" VisibleIndex="3" Caption="Phone Type">
                                                                            <EditFormSettings Visible="False" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <PropertiesTextEdit MaxLength="100">
                                                                            </PropertiesTextEdit>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>

                                                                        <%--//////////////////////////////--%>




                                                                        <dxe:GridViewDataComboBoxColumn Caption="Phone Type" FieldName="phf_type" Visible="false">
                                                                            <PropertiesComboBox ValueType="System.String">
                                                                                <%--<ClientSideEvents SelectedIndexChanged="function(s, e) { OnPhoneTypeChanged(s); }"></ClientSideEvents>--%>







                                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) {
	    var value = s.GetValue();
        
       if(value == &quot;Mobile&quot;)
    {
             gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetEnabled(false);
             gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetEnabled(false);
             gridPhone.GetEditor(&quot;phf_extension&quot;).SetEnabled(false);
       
    }
    else
    {
             gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetEnabled(true);
             gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetEnabled(true);
             gridPhone.GetEditor(&quot;phf_extension&quot;).SetEnabled(true);
        
    }
                                                                                  
    
    }" />







                                                                                <ClientSideEvents Init="function(s, e) {
                                                                               var value = s.GetValue();
                                                                                     
        if(value == &quot;Mobile&quot;)
        {
             gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetEnabled(false);
             gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetEnabled(false);
             gridPhone.GetEditor(&quot;phf_extension&quot;).SetEnabled(false);
            
             
        }
        else
        {
             gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetEnabled(true);
             gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetEnabled(true);
             gridPhone.GetEditor(&quot;phf_extension&quot;).SetEnabled(true);
             
        }
    }" />







                                                                                <Items>

                                                                                    <dxe:ListEditItem Text="Residence" Value="Residence"></dxe:ListEditItem>


                                                                                    <dxe:ListEditItem Text="Office" Value="Office"></dxe:ListEditItem>



                                                                                    <dxe:ListEditItem Text="Correspondence" Value="Correspondence"></dxe:ListEditItem>


                                                                                    <dxe:ListEditItem Text="Mobile" Value="Mobile"></dxe:ListEditItem>


                                                                                    <dxe:ListEditItem Text="Fax" Value="Fax"></dxe:ListEditItem>

                                                                                    <dxe:ListEditItem Text="Personal" Value="Personal"></dxe:ListEditItem>

                                                                                    <dxe:ListEditItem Text="Alternate" Value="Alternate"></dxe:ListEditItem>

                                                                                    <dxe:ListEditItem Text="Whatsapp" Value="Whatsapp"></dxe:ListEditItem>

                                                                                </Items>


                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">


                                                                                    <RequiredField ErrorText="Mandatory" IsRequired="True" />

                                                                                </ValidationSettings>


                                                                            </PropertiesComboBox>
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormSettings Visible="True" VisibleIndex="3" />
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataComboBoxColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="phf_countryCode" Visible="False">
                                                                            <EditFormSettings Caption="Country Code" Visible="True" VisibleIndex="4" />
                                                                            <PropertiesTextEdit MaxLength="5">


                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">


                                                                                    <RegularExpression ErrorText="Mandatory" ValidationExpression="[0-9+]+" />



                                                                                </ValidationSettings>



                                                                            </PropertiesTextEdit>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="phf_areaCode" VisibleIndex="5" Visible="False">
                                                                            <EditFormSettings Caption="Area Code" Visible="True" />
                                                                            <PropertiesTextEdit MaxLength="5">



                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">



                                                                                    <RegularExpression ErrorText="Enter Valid Area Code" ValidationExpression="[0-9]+" />

                                                                                </ValidationSettings>


                                                                                <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />

                                                                            </PropertiesTextEdit>

                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="phf_phoneNumber" VisibleIndex="6" Caption="Number"
                                                                            Visible="True">
                                                                            <EditFormSettings Visible="True" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <PropertiesTextEdit MaxLength="100">

                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">

                                                                                    <RequiredField IsRequired="True" ErrorText="Mandatory"></RequiredField>

                                                                                    <RegularExpression ErrorText="Enter Valid Phone Number" ValidationExpression="[0-9]+" />

                                                                                </ValidationSettings>

                                                                                <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />

                                                                            </PropertiesTextEdit>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="Number" VisibleIndex="7" Caption="Phone Number"
                                                                            Width="40%">
                                                                            <EditFormSettings Visible="False" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="status" VisibleIndex="8" Caption="Status">
                                                                            <DataItemTemplate>
                                                                                <a href="javascript:void(0);" onclick="OnAddEditClickP(this,'Edit~'+'<%# Container.KeyValue %>')">
                                                                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text='<%# Eval("status")%>' Width="100%"
                                                                                        ToolTip="Click to Change Status">
                                                                                    </dxe:ASPxLabel>
                                                                                </a>
                                                                            </DataItemTemplate>
                                                                            <EditFormSettings Visible="false" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataTextColumn VisibleIndex="9" Caption="Change Status" Visible="false">
                                                                            <DataItemTemplate>
                                                                                <a href="javascript:void(0);" onclick="OnAddEditClickP(this,'Edit~'+'<%# Container.KeyValue %>')">
                                                                                    <u>Change Status</u> </a>
                                                                            </DataItemTemplate>
                                                                            <EditFormSettings Visible="false" VisibleIndex="4" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <%-- <dxe:GridViewDataComboBoxColumn Caption="Sms alert by Stock Exchange" FieldName="phf_SMSFacility"
                                                                        VisibleIndex="9" Visible="false">
                                                                        <PropertiesComboBox ValueType="System.String">
                                                                            <Items>
                                                                                <dxe:ListEditItem Text="Yes" Value="1"></dxe:ListEditItem>
                                                                                <dxe:ListEditItem Text="No" Value="2"></dxe:ListEditItem>
                                                                            </Items>
                                                                        </PropertiesComboBox>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="True" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>--%>
                                                                        <dxe:GridViewDataTextColumn FieldName="phf_extension" VisibleIndex="10" Caption="Extension"
                                                                            Visible="False">
                                                                            <EditFormSettings Visible="True" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <PropertiesTextEdit MaxLength="50">

                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">

                                                                                    <RegularExpression ErrorText="Enter Valid Extension" ValidationExpression="[0-9]+" />

                                                                                </ValidationSettings>

                                                                                <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />

                                                                            </PropertiesTextEdit>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewCommandColumn VisibleIndex="11" ShowDeleteButton="true" ShowEditButton="true" HeaderStyle-HorizontalAlign="Center" Width="6%">

                                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                                            <HeaderTemplate>
                                                                                Actions
                                                                                <%-- <% if (rights.CanAdd)
                                                                                     { %>--%>
                                                                                <%--<a href="javascript:void(0);" onclick="gridPhone.AddNewRow();"><span>Add New</span> </a>--%>
                                                                                <%--  <% } %>--%>
                                                                            </HeaderTemplate>
                                                                        </dxe:GridViewCommandColumn>
                                                                    </Columns>
                                                                    <SettingsCommandButton>

                                                                        <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                                                                            <Image AlternateText="Edit" Url="../../../assests/images/Edit.png"></Image>

                                                                            <Styles>
                                                                                <Style CssClass="pad"></Style>
                                                                            </Styles>
                                                                        </EditButton>
                                                                        <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                                            <Image AlternateText="Delete" Url="../../../assests/images/Delete.png"></Image>
                                                                        </DeleteButton>
                                                                        <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger ">
                                                                            <Styles>
                                                                                <Style CssClass="btn btn-danger "></Style>
                                                                            </Styles>
                                                                        </CancelButton>
                                                                    </SettingsCommandButton>
                                                                    <Settings ShowStatusBar="Visible" ShowTitlePanel="True" ShowGroupPanel="true" />
                                                                    <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="WindowCenter"
                                                                        PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="450px"
                                                                        EditFormColumnCount="1" />
                                                                    <Styles>
                                                                        <LoadingPanel ImageSpacing="10px">
                                                                        </LoadingPanel>
                                                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                        </Header>
                                                                    </Styles>
                                                                    <SettingsText PopupEditFormCaption="Add/Modify Phone" ConfirmDelete="Confirm delete?" />
                                                                    <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                                                                        <FirstPageButton Visible="True">
                                                                        </FirstPageButton>
                                                                        <LastPageButton Visible="True">
                                                                        </LastPageButton>
                                                                    </SettingsPager>
                                                                    <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                                                                    <Templates>
                                                                        <EditForm>
                                                                            <div style="color: red; margin-top: 5px; margin-left: 5px;">* Denotes the mandatory field.</div>
                                                                            <div style="color: #000; margin: 5px 5px 0px 5px">
                                                                                <table style="width: 100%">
                                                                                    <tr>
                                                                                        <td style="width: 5%"></td>
                                                                                        <td style="width: 90%" align="">
                                                                                            <controls>
                                                    <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors">
                                                    </dxe:ASPxGridViewTemplateReplacement>                                                           
                                                  </controls>
                                                                                            <%--<hr />--%>

                                                                                            <div style="padding: 2px 2px 2px 93px">
                                                                                                <%-- <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                                                runat="server">
                                                                            </dxe:ASPxGridViewTemplateReplacement>--%>
                                                                                                <%-- onclick="OnPhoneClick()"--%>
                                                                                                <a id="update" href="#" class="btn btn-primary " onclick="OnPhoneClick()" style="color: white; padding: 6px 18px !important;">Save</a> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                            <div class="dxbButton" style="display: inline-block; padding: 3px">
                                                                                                <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                                                                    runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                            </div>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td style="width: 5%"></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </EditForm>
                                                                        <TitlePanel>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td align="center" style="width: 50%">
                                                                                        <span class="Ecoheadtxt" style="color: Black">Add/Modify Phone.</span>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </TitlePanel>
                                                                    </Templates>
                                                                </dxe:ASPxGridView>
                                                                <dxe:ASPxPopupControl ID="ASPxPopupControlP" ClientInstanceName="popupP" runat="server"
                                                                    AllowDragging="True" PopupHorizontalAlign="WindowCenter" HeaderText="Set Phone Status"
                                                                    EnableHotTrack="False" BackColor="#DDECFE" Width="400px" CloseAction="CloseButton">
                                                                    <ContentCollection>
                                                                        <dxe:PopupControlContentControl runat="server">
                                                                            <dxe:ASPxCallbackPanel ID="ASPxCallbackPanelP" runat="server" Width="400px" ClientInstanceName="popPanelP"
                                                                                OnCallback="ASPxCallbackPanelP_Callback" OnCustomJSProperties="ASPxCallbackPanelP_CustomJSProperties">
                                                                                <PanelCollection>
                                                                                    <dxe:PanelContent runat="server">
                                                                                        <div style="color: #000;">
                                                                                            <table style="width: 100%">
                                                                                                <tr>
                                                                                                    <td>Status:
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:DropDownList ID="cmbStatusP" runat="server" Width="180px">
                                                                                                            <asp:ListItem Text="Active" Value="Y"></asp:ListItem>
                                                                                                            <asp:ListItem Text="Deactive" Value="N"></asp:ListItem>
                                                                                                        </asp:DropDownList>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Date:
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <dxe:ASPxDateEdit ID="StDateP" runat="server" ClientInstanceName="StDate" Width="179px" EditFormat="Custom"
                                                                                                            UseMaskBehavior="True" TabIndex="21">
                                                                                                            <ButtonStyle Width="13px">
                                                                                                            </ButtonStyle>
                                                                                                        </dxe:ASPxDateEdit>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Reason:
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtReasonP" runat="server" TextMode="MultiLine" Width="250px"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td colspan="2" class="gridcellleft">
                                                                                                        <input id="Button3" type="button" value="Save" class="btnUpdate btn btn-primary" onclick="btnSave_ClickP()"
                                                                                                            style="width: 60px" tabindex="41" />
                                                                                                        <input id="Button4" type="button" value="Cancel" class="btnUpdate btn btn-danger" onclick="btnCancel_ClickP()"
                                                                                                            style="width: 60px" tabindex="42" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </dxe:PanelContent>
                                                                                </PanelCollection>
                                                                                <ClientSideEvents EndCallback="function(s, e) {
	                                                        EndCallBackP(s.cpLast);
                                                        }" />
                                                                            </dxe:ASPxCallbackPanel>
                                                                        </dxe:PopupControlContentControl>
                                                                    </ContentCollection>
                                                                    <HeaderStyle HorizontalAlign="Left">
                                                                        <Paddings PaddingRight="6px" />
                                                                    </HeaderStyle>
                                                                    <SizeGripImage Height="16px" Width="16px" />
                                                                    <CloseButtonImage Height="12px" Width="13px" />
                                                                    <ClientSideEvents CloseButtonClick="function(s, e) {
	     popup.Hide();
    }" />
                                                                </dxe:ASPxPopupControl>
                                                            </dxe:ContentControl>
                                                        </ContentCollection>
                                                    </dxe:TabPage>
                                                    <dxe:TabPage Name="Email" Text="Email">

                                                        <ContentCollection>
                                                            <dxe:ContentControl runat="server">
                                                                <div style="float: left;">
                                                                    <% if (rights.CanAdd)
                                                                       { %>
                                                                    <a href="javascript:void(0);" class="btn btn-primary" onclick="gridEmail.AddNewRow();"><span>Add New</span> </a>
                                                                    <% } %>
                                                                </div>
                                                                <dxe:ASPxGridView ID="EmailGrid" runat="server" ClientInstanceName="gridEmail"
                                                                    DataSourceID="Email" KeyFieldName="eml_id" AutoGenerateColumns="False" Width="100%"
                                                                    OnCustomCallback="EmailGrid_CustomCallback" OnRowValidating="EmailGrid_RowValidating"
                                                                    OnBeforeGetCallbackResult="EmailGrid_BeforeGetCallbackResult" OnCancelRowEditing="EmailGrid_CancelRowEditing"
                                                                    OnCustomJSProperties="EmailGrid_CustomJSProperties" OnCommandButtonInitialize="EmailGrid_CommandButtonInitialize">
                                                                    <SettingsSearchPanel Visible="True" />
                                                                    <Settings ShowFilterRow="true" ShowGroupPanel="true" ShowFilterRowMenu="true" />
                                                                    <Columns>
                                                                        <dxe:GridViewDataTextColumn FieldName="eml_id" VisibleIndex="0" Visible="False">
                                                                            <EditFormSettings Visible="False" />
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataCheckColumn FieldName="Isdefault" Visible="false" Caption="Default">
                                                                            <EditFormSettings Visible="True" />
                                                                            <PropertiesCheckEdit>
                                                                                <%--  <ClientSideEvents CheckedChanged="function(s,e){fn_chekFbtRate(s,e);}"/>--%>
                                                                            </PropertiesCheckEdit>
                                                                        </dxe:GridViewDataCheckColumn>
                                                                        <dxe:GridViewDataComboBoxColumn Caption="Email Type" FieldName="eml_type" Visible="False"
                                                                            VisibleIndex="1">
                                                                            <PropertiesComboBox ValueType="System.String" Width="100%">

                                                                                <%--  <ClientSideEvents SelectedIndexChanged="function(s, e) { OnEmilTypeChanged(s); }"></ClientSideEvents>--%>

                                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { gridEmail.PerformCallback(); }" />


                                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) {
	    var value = s.GetValue();
	    
        if(value == &quot;Web Site&quot;)
        {
             gridEmail.GetEditor(&quot;eml_email&quot;).SetEnabled(false);
             gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetEnabled(false);
             gridEmail.GetEditor(&quot;eml_website&quot;).SetEnabled(true);
        }
        else
        {
             gridEmail.GetEditor(&quot;eml_email&quot;).SetEnabled(true);
             gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetEnabled(true);
             gridEmail.GetEditor(&quot;eml_website&quot;).SetEnabled(false);
        }
        
    }" />








                                                                                <ClientSideEvents Init="function(s, e) {
	    var value = s.GetValue();
	    
        if(value == &quot;Web Site&quot;)
        {
             gridEmail.GetEditor(&quot;eml_email&quot;).SetEnabled(false);
             gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetEnabled(false);
             gridEmail.GetEditor(&quot;eml_website&quot;).SetEnabled(true);
        }
        else
        {
             gridEmail.GetEditor(&quot;eml_email&quot;).SetEnabled(true);
             gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetEnabled(true);
             gridEmail.GetEditor(&quot;eml_website&quot;).SetEnabled(false);
        }
       
    }" />








                                                                                <Items>
                                                                                    <dxe:ListEditItem Text="Official (For sending Emails)" Value="Official"></dxe:ListEditItem>
                                                                                    <dxe:ListEditItem Text="Personal" Value="Personal"></dxe:ListEditItem>
                                                                                    <dxe:ListEditItem Text="Web Site" Value="Web Site"></dxe:ListEditItem>
                                                                                </Items>



                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">







                                                                                    <RequiredField IsRequired="True" ErrorText="Mandatory"></RequiredField>







                                                                                </ValidationSettings>







                                                                            </PropertiesComboBox>
                                                                            <EditFormSettings Visible="True" />
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataComboBoxColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="eml_type" VisibleIndex="2" Caption="Type"
                                                                            Width="27%">
                                                                            <EditFormSettings Caption="Email Type" Visible="False" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="eml_email" VisibleIndex="3" Caption="Email">
                                                                            <EditFormSettings Caption="Email ID" Visible="True" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                            <PropertiesTextEdit MaxLength="200" Width="100%">
                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">
                                                                                    <%--<RequiredField IsRequired="true" ErrorText="Mandatory" />--%>
                                                                                   <%-- REV 2.0--%>
                                                                                <%--   <RegularExpression ErrorText="Enter valid Email ID" ValidationExpression="\w+([-+.,']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />--%>

                                                                                <RegularExpression ErrorText="Enter valid Email ID" ValidationExpression="(([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)(\s*(;|,)\s*|\s*$))*" />
                                                                                    <%-- REV 2.0 END--%>
                                                                                </ValidationSettings>
                                                                            </PropertiesTextEdit>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="eml_ccEmail" VisibleIndex="4" Visible="False">
                                                                            <EditFormSettings Caption="CC Email ID" Visible="True" />
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                            <PropertiesTextEdit MaxLength="200" Width="100%">
                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">
                                                                                    <%-- REV 2.0--%>
                                                                                   <%-- <RegularExpression ErrorText="Enter Valid CC Email ID" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />--%>
                                                                                     <RegularExpression ErrorText="Enter Valid CC Email ID" ValidationExpression="(([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)(\s*(;|,)\s*|\s*$))*" />
                                                                                     <%-- REV 2.0 END--%>
                                                                                </ValidationSettings>
                                                                            </PropertiesTextEdit>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="status" VisibleIndex="6" Caption="Status">
                                                                            <DataItemTemplate>
                                                                                <a href="javascript:void(0);" onclick="OnAddEditClickE(this,'Edit~'+'<%# Container.KeyValue %>')">
                                                                                    <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text='<%# Eval("status")%>' Width="100%"
                                                                                        ToolTip="Click to Change Status">
                                                                                    </dxe:ASPxLabel>
                                                                                </a>
                                                                            </DataItemTemplate>
                                                                            <EditFormSettings Visible="false" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="eml_website" Caption="Website" VisibleIndex="5"
                                                                            Visible="true">
                                                                            <EditFormSettings Caption="Website" Visible="true" />
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                            <PropertiesTextEdit MaxLength="50">
                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="true">
                                                                                    <RegularExpression ValidationExpression="(http(s)?://)?([\w-]+\.)+[\w-]+[\w-]+[\.]+[\.com]+([./?%&=]*)?" ErrorText="Enter valid url" />

                                                                                </ValidationSettings>
                                                                            </PropertiesTextEdit>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <%-- <dxe:GridViewDataComboBoxColumn Caption="" FieldName="eml_facility"
                                                                        VisibleIndex="9" Visible="false">
                                                                        <PropertiesComboBox ValueType="System.String">
                                                                            <Items>
                                                                                <dxe:ListEditItem Text="Yes" Value="1"></dxe:ListEditItem>
                                                                                <dxe:ListEditItem Text="No" Value="2"></dxe:ListEditItem>
                                                                            </Items>
                                                                        </PropertiesComboBox>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="True" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>--%>
                                                                        <dxe:GridViewDataTextColumn VisibleIndex="7" Caption="Change Status" Visible="false">
                                                                            <DataItemTemplate>
                                                                                <a href="javascript:void(0);" onclick="OnAddEditClickE(this,'Edit~'+'<%# Container.KeyValue %>')">
                                                                                    <u>Change Status</u> </a>
                                                                            </DataItemTemplate>
                                                                            <EditFormSettings Visible="false" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewCommandColumn VisibleIndex="8" ShowDeleteButton="true" ShowEditButton="true" HeaderStyle-HorizontalAlign="Center" Width="6%" CellStyle-HorizontalAlign="Center">

                                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                                            <CellStyle HorizontalAlign="Center"></CellStyle>

                                                                            <HeaderTemplate>
                                                                                Actions
                                                                                <%-- <% if (rights.CanAdd)
                                                                                     { %>--%>
                                                                                <%-- <a href="javascript:void(0);" onclick="gridEmail.AddNewRow();"><span>Add New</span> </a>--%>
                                                                                <%-- <% } %>--%>
                                                                            </HeaderTemplate>
                                                                        </dxe:GridViewCommandColumn>
                                                                        <%-- <dxe:GridViewCommandColumn VisibleIndex="3">
                                                            <EditButton Visible="True">
                                                            </EditButton>
                                                        </dxe:GridViewCommandColumn>--%>
                                                                    </Columns>
                                                                    <SettingsCommandButton>

                                                                        <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                                                                            <Image AlternateText="Edit" Url="../../../assests/images/Edit.png"></Image>

                                                                            <Styles>
                                                                                <Style CssClass="pad"></Style>
                                                                            </Styles>
                                                                        </EditButton>
                                                                        <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                                            <Image AlternateText="Delete" Url="../../../assests/images/Delete.png"></Image>
                                                                        </DeleteButton>
                                                                        <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger ">
                                                                            <Styles>
                                                                                <Style CssClass="btn btn-danger "></Style>
                                                                            </Styles>
                                                                        </CancelButton>
                                                                    </SettingsCommandButton>
                                                                    <SettingsSearchPanel Visible="True" />
                                                                    <Settings ShowStatusBar="Visible" ShowTitlePanel="True" ShowGroupPanel="true" ShowFilterRow="true" ShowFilterRowMenu="true" />

                                                                    <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center"
                                                                        PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="500px"
                                                                        EditFormColumnCount="1" />
                                                                    <Styles>
                                                                        <LoadingPanel ImageSpacing="10px">
                                                                        </LoadingPanel>
                                                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                        </Header>
                                                                    </Styles>
                                                                    <SettingsText PopupEditFormCaption="Add/Modify Email" ConfirmDelete="Confirm delete?" />
                                                                    <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                                                                        <FirstPageButton Visible="True">
                                                                        </FirstPageButton>
                                                                        <LastPageButton Visible="True">
                                                                        </LastPageButton>
                                                                    </SettingsPager>
                                                                    <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                                                                    <Templates>
                                                                        <EditForm>
                                                                            <div style="color: red; margin-top: 5px; margin-left: 5px;">* Denotes the mandatory field.</div>
                                                                            <div style="color: #000; margin: 8px 8px 0px 8px">
                                                                                <table style="width: 100%">
                                                                                    <tr>
                                                                                        <td style="width: 5%"></td>
                                                                                        <td style="width: 90%">

                                                                                            <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors"></dxe:ASPxGridViewTemplateReplacement>

                                                                                            <div style="padding: 2px 2px 2px 89px;">
                                                                                                <%-- <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                                                runat="server">
                                                                            </dxe:ASPxGridViewTemplateReplacement>--%>
                                                                                                <a id="update1" href="#" class="btn btn-primary " style="color: white; padding: 6px 18px !important;" onclick="OnEmailClick()">Save</a> &nbsp;&nbsp;&nbsp;&nbsp;
                                                                                            <div class="dxbButton" style="display: inline-block; padding: 3px">
                                                                                                <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                                                                    runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                            </div>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td style="width: 5%"></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </EditForm>
                                                                        <TitlePanel>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td align="center" style="width: 50%">
                                                                                        <span class="Ecoheadtxt" style="color: black">Add/Modify Email.</span>
                                                                                    </td>
                                                                                    <%-- <td align="right">
                                                  <table >
                                                     <tr>
    <td>
                                                           <dxe:ASPxButton ID="ASPxButton1" runat="server" Text="ADD" ToolTip="Add New Data"   Height="18px" Width="88px" AutoPostBack="False" Font-Size="12px">
                                                                <clientsideevents click="function(s, e) {gridEmail.AddNewRow();}" />
                                                           </dxe:ASPxButton>
                                                        </td>
                                                    </tr>
                                                  </table>
                                                </td>   --%>
                                                                                </tr>
                                                                            </table>
                                                                        </TitlePanel>
                                                                    </Templates>
                                                                    <ClientSideEvents EndCallback="function(s, e) {
	    Emailcheck(s.cpHeight);
    }" />
                                                                </dxe:ASPxGridView>
                                                                <dxe:ASPxPopupControl ID="ASPxPopupControlE" ClientInstanceName="popupE" runat="server"
                                                                    AllowDragging="True" PopupHorizontalAlign="WindowCenter" HeaderText="Set Email Status"
                                                                    EnableHotTrack="False" BackColor="#DDECFE" Width="400px" CloseAction="CloseButton">
                                                                    <ContentCollection>
                                                                        <dxe:PopupControlContentControl runat="server">
                                                                            <dxe:ASPxCallbackPanel ID="ASPxCallbackPanelE" runat="server" Width="400px" ClientInstanceName="popPanelE"
                                                                                OnCallback="ASPxCallbackPanelE_Callback" OnCustomJSProperties="ASPxCallbackPanelE_CustomJSProperties">
                                                                                <PanelCollection>
                                                                                    <dxe:PanelContent runat="server">
                                                                                        <table style="width: 100%">
                                                                                            <tr>
                                                                                                <td>Status:
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:DropDownList ID="cmbStatusE" runat="server" Width="180px">
                                                                                                        <asp:ListItem Text="Active" Value="Y"></asp:ListItem>
                                                                                                        <asp:ListItem Text="Deactive" Value="N"></asp:ListItem>
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>Date:
                                                                                                </td>
                                                                                                <td>
                                                                                                    <dxe:ASPxDateEdit ID="StDateE" runat="server" ClientInstanceName="StDate" EditFormat="Custom"
                                                                                                        UseMaskBehavior="True" Width="179px" TabIndex="21">
                                                                                                        <ButtonStyle Width="13px">
                                                                                                        </ButtonStyle>
                                                                                                    </dxe:ASPxDateEdit>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>Reason:
                                                                                                </td>
                                                                                                <td>
                                                                                                    <%-- <asp:TextBox ID="txtReasonE" runat="server" TextMode="MultiLine" Width="250px" Height="80px"></asp:TextBox>--%>
                                                                                                    <dxe:ASPxMemo ID="txtReasonE" runat="server" Width="250px" Height="80px" MaxLength="50"></dxe:ASPxMemo>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td></td>
                                                                                                <td colspan="2" class="gridcellleft">
                                                                                                    <input id="Button5" type="button" value="Save" class="btnUpdate btn btn-primary" onclick="btnSave_ClickE()"
                                                                                                        tabindex="41" />
                                                                                                    <input id="Button6" type="button" value="Cancel" class="btnUpdate btn btn-danger" onclick="btnCancel_ClickE()"
                                                                                                        tabindex="42" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </dxe:PanelContent>
                                                                                </PanelCollection>
                                                                                <ClientSideEvents EndCallback="function(s, e) {
	                                                        EndCallBackE(s.cpLast);
                                                        }" />
                                                                            </dxe:ASPxCallbackPanel>
                                                                        </dxe:PopupControlContentControl>
                                                                    </ContentCollection>
                                                                    <HeaderStyle HorizontalAlign="Left">
                                                                        <Paddings PaddingRight="6px" />
                                                                    </HeaderStyle>
                                                                    <SizeGripImage Height="16px" Width="16px" />
                                                                    <CloseButtonImage Height="12px" Width="13px" />
                                                                    <ClientSideEvents CloseButtonClick="function(s, e) {
	     popup.Hide();
    }" />
                                                                </dxe:ASPxPopupControl>
                                                            </dxe:ContentControl>
                                                        </ContentCollection>
                                                    </dxe:TabPage>
                                                </TabPages>
                                            </dxe:ASPxPageControl>
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Bank Details" Text="Bank">

                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="DP Details" Visible="false" Text="DP">

                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Documents" Text="Documents">

                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Registration" Text="Registration">

                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Other" Visible="false" Text="Other">

                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Group Member" Text="Group">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Deposit" Visible="false" Text="Deposit">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Remarks" Text="UDF" Visible="false">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Education" Visible="false" Text="Education">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Trad. Prof." Visible="false" Text="Trad.Prof">
                                    <%--<TabTemplate ><span style="font-size:x-small">Trad.Prof</span>&nbsp;<span style="color:Red;">*</span> </TabTemplate>--%>
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="FamilyMembers" Visible="false" Text="Family">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Subscription" Visible="false" Text="Subscription">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                            </TabPages>
                            <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                                var activeTab   = page.GetActiveTab();
	                                                var Tab0 = page.GetTab(0);
	                                                var Tab1 = page.GetTab(1);
	                                                var Tab2 = page.GetTab(2);
	                                                var Tab3 = page.GetTab(3);
	                                                var Tab4 = page.GetTab(4);
	                                                var Tab5 = page.GetTab(5);
	                                                var Tab6 = page.GetTab(6);
	                                                var Tab7 = page.GetTab(7);
	                                                var Tab8 = page.GetTab(8);
	                                                var Tab9 = page.GetTab(9);
	                                                var Tab10 = page.GetTab(10);
	                                                var Tab11 = page.GetTab(11);
	                                                var Tab12 = page.GetTab(12);
	                                                var Tab13=page.GetTab(13);
	                                               
	                                                if(activeTab == Tab0)
	                                                {
	                                                    disp_prompt('tab0');
	                                                }
	                                                if(activeTab == Tab1)
	                                                {
	                                                    disp_prompt('tab1');
	                                                }
	                                                else if(activeTab == Tab2)
	                                                {
	                                                    disp_prompt('tab2');
	                                                }
	                                                else if(activeTab == Tab3)
	                                                {
	                                                    disp_prompt('tab3');
	                                                }
	                                                else if(activeTab == Tab4)
	                                                {
	                                                    disp_prompt('tab4');
	                                                }
	                                                else if(activeTab == Tab5)
	                                                {
	                                                    disp_prompt('tab5');
	                                                }
	                                                else if(activeTab == Tab6)
	                                                {
	                                                    disp_prompt('tab6');
	                                                }
	                                                else if(activeTab == Tab7)
	                                                {
	                                                    disp_prompt('tab7');
	                                                }
	                                                else if(activeTab == Tab8)
	                                                {
	                                                    disp_prompt('tab8');
	                                                }
	                                                else if(activeTab == Tab9)
	                                                {
	                                                    disp_prompt('tab9');
	                                                }
	                                                else if(activeTab == Tab10)
	                                                {
	                                                    disp_prompt('tab10');
	                                                }
	                                                else if(activeTab == Tab11)
	                                                {
	                                                    disp_prompt('tab11');
	                                                }
	                                                else if(activeTab == Tab12)
	                                                {
	                                                    disp_prompt('tab12');
	                                                }
	                                                else if(activeTab == Tab13)
	                                                {
	                                                   disp_prompt('tab13');
	                                                }
	                                                
	                                                }"></ClientSideEvents>
                            <ContentStyle>
                                <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                            </ContentStyle>
                            <LoadingPanelStyle ImageSpacing="6px">
                            </LoadingPanelStyle>
                            <TabStyle>
                            </TabStyle>
                        </dxe:ASPxPageControl>
                        <asp:TextBox ID="txtID" runat="server" Visible="false"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
            </table>
        </div>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>


        <asp:SqlDataSource ID="Address" runat="server"
            SelectCommand="select DISTINCT  tbl_master_address.add_id AS Id,tbl_master_address.Isdefault as Isdefault, tbl_master_address.contactperson as contactperson,tbl_master_address.add_addressType AS Type,
                        tbl_master_address.add_address1 AS Address1,  tbl_master_address.add_address2 AS Address2, 
                        tbl_master_address.add_address3 AS Address3,tbl_master_address.add_landMark AS LandMark, 
                        tbl_master_address.add_country AS Country, 
                        tbl_master_address.add_state AS State,tbl_master_address.add_city AS City,CASE add_pin WHEN '' THEN '' ELSE(SELECT pin_code FROM tbl_master_pinzip WHERE pin_id = add_pin) END AS PinCode1,
                        CASE add_country WHEN '' THEN '' ELSE(SELECT cou_country FROM tbl_master_country WHERE cou_id = add_country) END AS Country1, 
                        CASE add_state WHEN '' THEN '' ELSE(SELECT state FROM tbl_master_state WHERE id = add_state) END AS State1,
                        CASE add_city WHEN '' THEN '' ELSE(SELECT city_name FROM tbl_master_city WHERE city_id = add_city) END AS City1,
                        CASE add_area WHEN '' THEN '' Else(select area_name From tbl_master_area Where area_id = add_area) End AS add_area, area = CAST(add_area as int),
                        tbl_master_address.add_pin AS PinCode, tbl_master_address.add_landMark AS LankMark ,
                        case when add_status='N' then 'Deactive' else 'Active' end as status ,add_Phone as Phone,add_Email,add_Website,add_Designation,add_address4  as  Address4,Convert(decimal(18,2),Convert(decimal(18,2)
                        ,isnull(Distance,0.00))) Distance   
                        ,CAST(DateofBirth AS DATE) AS DateofBirth 
                        ,CAST(AnniversaryDate AS DATE) AS AnniversaryDate                 
                        from tbl_master_address where add_cntId=@insuId"
            DeleteCommand="contactDelete"
            DeleteCommandType="StoredProcedure" InsertCommand="insert_correspondence"
            InsertCommandType="StoredProcedure" UpdateCommand="Update_correspondence" UpdateCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="insuId" SessionField="KeyVal_InternalID_New" Type="String" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="Id" Type="int32" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:SessionParameter Name="insuId" SessionField="KeyVal_InternalID_New" Type="String" />
                <asp:SessionParameter Name="contacttype" SessionField="ContactType" Type="string" />
                <asp:Parameter Name="Id" Type="int32" />

                <asp:Parameter Name="Type" Type="string" />
                <asp:Parameter Name="Address1" Type="string" />
                <asp:Parameter Name="Address2" Type="string" />
                <asp:Parameter Name="Address3" Type="string" />
                <asp:Parameter Name="City" Type="int32" />
                <asp:Parameter Name="area" Type="int32" />
                <asp:Parameter Name="LandMark" Type="string" />
                <asp:Parameter Name="Country" Type="int32" />
                <asp:Parameter Name="State" Type="int32" />
                <asp:Parameter Name="PinCode" Type="string" />
                <asp:Parameter Name="contactperson" Type="string" />
                <asp:Parameter Name="Isdefault" Type="int32" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
                <asp:Parameter Name="Id" Type="decimal" />
                <asp:Parameter Name="Phone" Type="string" />
                <asp:Parameter Name="add_Email" Type="string" />
                <asp:Parameter Name="add_Website" Type="string" />
                <asp:Parameter Name="add_Designation" Type="int32" />
                <asp:Parameter Name="Address4" Type="string" />
                <asp:Parameter Name="Distance" Type="decimal" />
                <asp:Parameter Name="DateofBirth" Type="datetime" />
                <asp:Parameter Name="AnniversaryDate" Type="datetime" />
            </UpdateParameters>
            <InsertParameters>
                <asp:SessionParameter Name="insuId" SessionField="KeyVal_InternalID_New" Type="String" />
                <asp:Parameter Name="Type" Type="string" />
                <asp:SessionParameter Name="contacttype" SessionField="ContactType" Type="string" />
                <asp:Parameter Name="Address1" Type="string" />
                <asp:Parameter Name="Address2" Type="string" />
                <asp:Parameter Name="Address3" Type="string" />
                <asp:Parameter Name="City" Type="int32" />
                <asp:Parameter Name="area" Type="int32" />
                <asp:Parameter Name="LandMark" Type="string" />
                <asp:Parameter Name="Country" Type="int32" />
                <asp:Parameter Name="State" Type="int32" />
                <asp:Parameter Name="PinCode" Type="string" />
                <asp:Parameter Name="contactperson" Type="string" />
                <asp:Parameter Name="Isdefault" Type="int32" />
                <asp:Parameter Name="Phone" Type="string" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
                <asp:Parameter Name="add_Email" Type="string" />
                <asp:Parameter Name="add_Website" Type="string" />
                <asp:Parameter Name="add_Designation" Type="int32" />
                <asp:Parameter Name="Address4" Type="string" />
                <asp:Parameter Name="Distance" Type="decimal" />
                <asp:Parameter Name="DateofBirth" Type="datetime" />
                <asp:Parameter Name="AnniversaryDate" Type="datetime" />
            </InsertParameters>
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="DesignationSelect" runat="server"
            SelectCommand="SELECT deg_id, deg_designation FROM tbl_master_Designation order by deg_designation"></asp:SqlDataSource>

        <asp:SqlDataSource ID="CountrySelect" runat="server"
            SelectCommand="SELECT cou_id, cou_country as Country FROM tbl_master_country order by cou_country"></asp:SqlDataSource>
        <asp:SqlDataSource ID="StateSelect" runat="server"
            SelectCommand="SELECT s.id as ID,s.state as State from tbl_master_state s where (s.countryId = @State) ORDER BY s.state">
            <SelectParameters>
                <asp:Parameter Name="State" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SelectCity" runat="server"
            SelectCommand="SELECT c.city_id AS CityId, c.city_name AS City FROM tbl_master_city c where c.state_id=@City order by c.city_name">

            <SelectParameters>
                <asp:Parameter Name="City" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SelectArea" runat="server"
            SelectCommand="SELECT area_id = CAST(area_id as int), area_name as areaname from tbl_master_area where (city_id = @Area) ORDER BY area_name">
            <SelectParameters>
                <asp:Parameter Name="Area" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>
        &nbsp;
    <asp:SqlDataSource ID="Phone" runat="server"
        DeleteCommand="PhoneDelete" DeleteCommandType="StoredProcedure" InsertCommand="insert_correspondence_phone" UpdateCommand="update_correspondence_phone"
        SelectCommand="select DISTINCT Isdefault,phf_id,phf_cntId,phf_entity,phf_type,phf_type as phf_type1,phf_type as phf_type2,phf_countryCode,phf_areaCode,phf_phoneNumber,phf_extension, ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' + ISNULL(phf_phoneNumber, '') +' '+ ISNULL(phf_extension, '') + ISNULL(phf_faxNumber, '') AS Number , case when phf_Status='N' then 'Deactive' else 'Active' end as status ,isnull(phf_SMSFacility,'') as  phf_SMSFacility
                        ,phf_ContactPerson,phf_ContactPersonDesignation  from tbl_master_phonefax where phf_cntId=@PhfId"
        UpdateCommandType="StoredProcedure"
       
        InsertCommandType="StoredProcedure">
        <SelectParameters>
            <asp:SessionParameter Name="PhfId" SessionField="KeyVal_InternalID_New" Type="String" />
        </SelectParameters>
        <InsertParameters>
            <asp:SessionParameter Name="PhfId" SessionField="KeyVal_InternalID_New" Type="String" />
            <asp:Parameter Name="phf_type" Type="string" />
            <asp:SessionParameter Name="contacttype" SessionField="ContactType" Type="string" />
            <asp:Parameter Name="phf_countryCode" Type="string" />
            <asp:Parameter Name="phf_areaCode" Type="string" />
            <asp:Parameter Name="phf_phoneNumber" Type="string" />
            <asp:Parameter Name="phf_extension" Type="string" />
            <asp:Parameter Name="phf_Availablefrom" Type="string" />
            <asp:Parameter Name="phf_AvailableTo" Type="string" />
            <asp:Parameter Name="phf_SMSFacility" Type="int32" />
            <asp:Parameter Name="Isdefault" Type="int32" />
            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
            <asp:Parameter Name="phf_ContactPerson" Type="string" />
            <asp:Parameter Name="phf_ContactPersonDesignation" Type="string" />
        </InsertParameters>
        <UpdateParameters>
            <asp:SessionParameter Name="Phf_cntId" SessionField="KeyVal_InternalID_New" Type="String" />
            <asp:Parameter Name="phf_type" Type="string" />
            <asp:Parameter Name="phf_countryCode" Type="string" />
            <asp:Parameter Name="phf_areaCode" Type="string" />
            <asp:Parameter Name="phf_phoneNumber" Type="string" />
            <asp:Parameter Name="phf_extension" Type="string" />
            <asp:Parameter Name="phf_Availablefrom" Type="string" />
            <asp:Parameter Name="phf_AvailableTo" Type="string" />
            <asp:Parameter Name="phf_SMSFacility" Type="int32" />
            <asp:Parameter Name="Isdefault" Type="int32" />

            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
            <asp:Parameter Name="phf_id" Type="decimal" />
            <asp:Parameter Name="phf_ContactPerson" Type="string" />
            <asp:Parameter Name="phf_ContactPersonDesignation" Type="string" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:Parameter Name="phf_id" Type="int32" />
            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="int32" />
        </DeleteParameters>
    </asp:SqlDataSource>


        <%--debjyoti 02-12-2016--%>
        <asp:SqlDataSource ID="SelectPin" runat="server"
            SelectCommand="select pin_id,pin_code from tbl_master_pinzip where city_id=@City order by pin_code">
            <SelectParameters>
                <asp:Parameter Name="City" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>
        <%--End Debjyoti 02-12-2016--%>

        <asp:SqlDataSource ID="Email" runat="server"
            DeleteCommand="EmailDelete" DeleteCommandType="StoredProcedure" InsertCommand="insert_correspondence_email"
            InsertCommandType="StoredProcedure" 
            SelectCommand="select Isdefault,eml_id,eml_cntId,eml_entity,eml_type,eml_email,eml_ccEmail,eml_website,CreateDate,CreateUser,case when eml_Status='N' then 'Deactive' else 'Active' end as status,(case when eml_facility=1 then '1' when eml_facility=2 then '2' else null end) as eml_facility from tbl_master_email where eml_cntId=@EmlId"
            UpdateCommand="update tbl_master_email set Isdefault=@Isdefault,eml_type=@eml_type,eml_email=@eml_email,eml_ccEmail=@eml_ccEmail,eml_website=@eml_website,LastModifyDate=getdate(),LastModifyUser=@CreateUser,eml_facility=(case when ltrim(rtrim(@eml_type))='Official' then @eml_facility else '2' end) where eml_id=@eml_id">

            <DeleteParameters>
                <asp:Parameter Name="eml_id" Type="int32" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:SessionParameter Name="EmlId" SessionField="KeyVal_InternalID_New" Type="string" />
                <asp:Parameter Name="eml_type" Type="string" />
                <asp:Parameter Name="eml_email" Type="string" />
                <asp:Parameter Name="eml_ccEmail" Type="string" />
                <asp:Parameter Name="eml_website" Type="string" />
                <asp:Parameter Name="eml_id" Type="decimal" />
                <asp:Parameter Name="eml_facility" Type="int32" />
                <asp:Parameter Name="Isdefault" Type="int32" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
            </UpdateParameters>
            <SelectParameters>
                <asp:SessionParameter Name="EmlId" SessionField="KeyVal_InternalID_New" Type="string" />
            </SelectParameters>
            <InsertParameters>
                <asp:SessionParameter Name="EmlId" SessionField="KeyVal_InternalID_New" Type="string" />
                <asp:Parameter Name="eml_type" Type="string" />
                <asp:SessionParameter Name="contacttype" SessionField="ContactType" Type="string" />
                <asp:Parameter Name="eml_email" Type="string" />
                <asp:Parameter Name="eml_ccEmail" Type="string" />
                <asp:Parameter Name="eml_website" Type="string" />
                <asp:Parameter Name="eml_facility" Type="int32" />
                <asp:Parameter Name="Isdefault" Type="int32" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
            </InsertParameters>
        </asp:SqlDataSource>
    </div>
    </div>
</asp:Content>
