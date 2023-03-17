<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                17-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="rootcompany_deductorinfo.aspx.cs" Inherits="ERP.OMS.Management.Master.rootcompany_deductorinfo" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

     <script language="javascript" type="text/javascript">
         $(document).ready(function () {
            
             $("#txtAssyear").keypress(function (e) {
                
                 if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                   
                     return false;
                 }
             });
         });
         $(document).ready(function () {

             $("#txtfinyr").keypress(function (e) {

                 if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {

                     return false;
                 }
             });
         });
         
         $(document).ready(function () {

             $("#txtDeductpin").keypress(function (e) {

                 if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {

                     return false;
                 }
             });
             
             $("#txtdeductSTD").keypress(function (e) {

                 if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {

                     return false;
                 }
             });
             $("#txtPersSTD").keypress(function (e) {

                 if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {

                     return false;
                 }
             });
             $("#txtEmpaltSTD").keypress(function (e) {

                 if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {

                     return false;
                 }
             });
             $("#txtPersAltSTD").keypress(function (e) {

                 if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {

                     return false;
                 }
             });
             $("#txtDeductTelNo").keypress(function (e) {

                 if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {

                     return false;
                 }
             });
             $("#txtpersPin").keypress(function (e) {

                 if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {

                     return false;
                 }
             });
             
             $("#txtPersTel").keypress(function (e) {

                 if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {

                     return false;
                 }
             });
             
             $("#txtPaoNo").keypress(function (e) {

                 if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {

                     return false;
                 }
             });
             $("#txtPersAltTel").keypress(function (e) {

                 if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {

                     return false;
                 }
             });
             $("#txtPersAltSTD").keypress(function (e) {

                 if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {

                     return false;
                 }
             });
             $("#txtEmpAltTel").keypress(function (e) {

                 if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {

                     return false;
                 }
             });
             $("#txtEmpaltSTD").keypress(function (e) {

                 if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {

                     return false;
                 }
             });
             $("#txtPersSTD").keypress(function (e) {

                 if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {

                     return false;
                 }
             });
         });
         
         function fnValidatePAN(Obj) {
             if (Obj == null) Obj = window.event.srcElement;
             if (Obj.value != "") {
                 ObjVal = Obj.value;
                 var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
                 var code = /([C,P,H,F,A,T,B,L,J,G])/;
                 var code_chk = ObjVal.substring(3, 4);
                 if (ObjVal.search(panPat) == -1) {
                     alert("Invalid PAN No");
                     Obj.focus();
                     return false;
                 }
                 if (code.test(code_chk) == false) {
                     alert("Invaild PAN Card No.");
                     return false;
                 }
             }
         }



         function pan_validate(pan) {
             var regpan = /^([A-Z]){5}([0-9]){4}([A-Z]){1}?$/;
             if (regpan.test(pan) == false) {
                 document.getElementById("status").innerHTML = "PAN Number Not Valid.";
             } else {
                 document.getElementById("status").innerHTML = "";
             }
         }


         function IsDeductorDataValid() {
            
             var returnData = true;
             
             if($('#txtAssyear').val()=="")
             {
                 var assyr=document.getElementById('<%= txtAssyear.ClientID %>');
                 assyr.focus();
                 $('#spAssyr').css({ 'display': 'block' });
                 returnData = false;

             }
             if ($('#txtfinyr').val() == "") {
                 var assyr = document.getElementById('<%= txtfinyr.ClientID %>');
                 assyr.focus();
                 $('#spFinyr').css({ 'display': 'block' });
                 returnData = false;
             }
             if ($('#txtNamedeductor').val() == "") {
                 var assyr = document.getElementById('<%= txtNamedeductor.ClientID %>');
                 assyr.focus();
                 $('#spNamedeductor').css({ 'display': 'block' });
                 returnData = false;
             }
             if ($('#txtBranchdeduct').val() == "") {
                 var assyr = document.getElementById('<%= txtBranchdeduct.ClientID %>');
                 assyr.focus();
                 $('#spBranchdeduct').css({ 'display': 'block' });
                 returnData = false;
             }
             if ($('#txtDeductaddr1').val() == "") {
                 var assyr = document.getElementById('<%= txtDeductaddr1.ClientID %>');
                 assyr.focus();
                 $('#spDeductaddr1').css({ 'display': 'block' });
                 returnData = false;
             }
             if (ctxtDeductpin.GetText() == "") {
                 var assyr = document.getElementById('<%= txtDeductpin.ClientID %>');
                 assyr.focus();
                 $('#spDeductpin').css({ 'display': 'block' });
                 returnData = false;
             }
             if (CtxtDeductState.GetText() == "") {
                 var assyr = document.getElementById('<%= txtDeductState.ClientID %>');
                 assyr.focus();
                 $('#spDeductState').css({ 'display': 'block' });
                 returnData = false;
             }
             if ($('#txtDeductEmail').val() == "") {
                 var assyr = document.getElementById('<%= txtDeductEmail.ClientID %>');
                 assyr.focus();
                 $('#spDedEmail').css({ 'display': 'block' });
                 returnData = false;
             }
             //if (cChkdeductAddrReturn.GetChecked()==false) {
              
             //    $('#spdeductAddrReturn').css({ 'display': 'block' });
             //      returnData = false;
             //  }
             if ($('#txtResponsibleDeduct').val() == "") {
                 var assyr = document.getElementById('<%= txtResponsibleDeduct.ClientID %>');
                 assyr.focus();
                 $('#spResponsibleDeduct').css({ 'display': 'block' });
                 returnData = false;
             }

             if ($('#txtdeductdesig').val() == "") {
                 var assyr = document.getElementById('<%= txtdeductdesig.ClientID %>');
                 assyr.focus();
                 $('#spdeductdesig').css({ 'display': 'block' });
                 returnData = false;
             }

             if ($('#txtPersaddr1').val() == "") {
                 var assyr = document.getElementById('<%= txtdeductdesig.ClientID %>');
                 assyr.focus();
                 $('#spPersaddr1').css({ 'display': 'block' });
                 returnData = false;
             }
             if (CtxtpersPin.GetText() == "") {
                 var assyr = document.getElementById('<%= txtpersPin.ClientID %>');
                 assyr.focus();
                 $('#spPersPin').css({ 'display': 'block' });
                 returnData = false;
             }
             if (CtxtPersState.GetText() == "") {
                 var assyr = document.getElementById('<%= txtPersState.ClientID %>');
                 assyr.focus();
                 $('#spPersState').css({ 'display': 'block' });
                 returnData = false;
             }
             if ($('#txtPersemail').val() == "") {
                 var assyr = document.getElementById('<%= txtPersemail.ClientID %>');
                 assyr.focus();
                 $('#spPersEmail').css({ 'display': 'block' });
                 returnData = false;
             }
             if ($('#txtMobile').val() == "") {
                 var assyr = document.getElementById('<%= txtMobile.ClientID %>');
                 assyr.focus();
                 $('#spmobile').css({ 'display': 'block' });
                 returnData = false;
             }
             //if (CchkResPersaddr.GetChecked() == false) {

             //    $('#spResPersaddr').css({ 'display': 'block' });
             //    returnData = false;
             //}

             var panVal = $('#txtRePanPers').val();
             var regpan = /^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$/;
             if ($('#txtRePanPers').val()!="")
             {
             if (!regpan.test(panVal)) {
                 alert("Invaild PAN Card No."); // valid pan card number
                 returnData = false;
             } 
         }
             var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;


             if ($("#txtDeductEmail").val()!="")
             {
             if (reg.test($("#txtDeductEmail").val()) == false) {
                 alert('Invalid Email Address');
                 returnData = false;
               }
           }

             if ($("#txtPersemail").val()!="")
             {
             if (reg.test($("#txtPersemail").val()) == false) {
                 alert('Invalid Email Address');
                 returnData = false;
               }
             }


             if ($('#txtRePanPers').val() == "") {
                 var assyr = document.getElementById('<%= txtRePanPers.ClientID %>');
                   assyr.focus();
                   $('#spRePanPers').css({ 'display': 'block' });
                   returnData = false;
               }
             return returnData;
         }





         function DeductorPinChange()
         {

             var BBSPin = ctxtDeductpin.GetText();
             if ((BBSPin.length > 0) && (BBSPin.length <= 20)) {
                 CustomerDetailsByPin();
             }
         }
         function CustomerDetailsByPin()
         {

             var detailsByPin = ctxtDeductpin.GetText().trim();
             if (detailsByPin != '') {
                 //var details = {}

                 //details.PinCode = detailsByPin;
                 $.ajax({
                     type: "POST",
                     url: "UserControls/TDSdeduction.asmx/CustomAddressByPin",
                     data: JSON.stringify({ PinCode: detailsByPin }),
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     success: function (msg) {
                         var obj = msg.d;
                         var returnObj = obj[0];

                         //Billing HdndeductPin
                         if (returnObj) {

                             ctxtDeductpin.SetText(returnObj.PinCode);
                             $('#HdndeductPin').val(returnObj.PinId);
                             CtxtDeductState.SetText(returnObj.StateName);
                             $('#hdnDeductStateid').val(returnObj.StateId);
                             $('#hdnDeductStateCode').val(returnObj.StateCode);

                         }
                         else
                         {
                             ctxtDeductpin.SetText("");
                             $('#HdndeductPin').val("");
                             CtxtDeductState.SetText("");
                             $('#hdnDeductStateid').val("");
                             $('#hdnDeductStateCode').val("");
                         }
                     }
                 });
             }
         }

         function DeductorPersonPinChange() {

             var BBSPin = CtxtpersPin.GetText();
             if ((BBSPin.length > 0) && (BBSPin.length <= 20)) {
                 PersonsDetailsByPin();
             }
         }
         function PersonsDetailsByPin() {

             var detailsByPin = CtxtpersPin.GetText().trim();
             if (detailsByPin != '') {
                 //var details = {}

                 //details.PinCode = detailsByPin;
                 $.ajax({
                     type: "POST",
                     url: "UserControls/TDSdeduction.asmx/CustomAddressByPin",
                     data: JSON.stringify({ PinCode: detailsByPin }),
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     success: function (msg) {
                         var obj = msg.d;
                         var returnObj = obj[0];

                         //Billing HdndeductPin
                         if (returnObj) {

                             CtxtpersPin.SetText(returnObj.PinCode);
                             $('#hdnPersPinId').val(returnObj.PinId);
                             CtxtPersState.SetText(returnObj.StateName);
                             $('#hdnPersStateId').val(returnObj.StateId);
                             $('#hdnPersStateCode').val(returnObj.StateCode);

                         }
                         else
                         {
                             CtxtpersPin.SetText("");
                             $('#hdnPersPinId').val("");
                             CtxtPersState.SetText("");
                             $('#hdnPersStateId').val("");
                             $('#hdnPersStateCode').val("");
                         }
                     }
                 });
             }
         }


         function disp_prompt(name)
         {
             if (name == "tab0") {
                 //alert(name);
                 document.location.href="rootcompany_general.aspx"; 
             }
             if (name == "tab1") {
                 //alert(name);
                 document.location.href = "rootComp_Correspondence.aspx";
             }
             if (name == "tab2") {
                 //alert(name);
                 document.location.href = "rootComp_exchange.aspx";
             }
             else if (name == "tab3") {
                 //alert(name);
                 document.location.href = "rootComp_dpMembership.aspx";
             }
             else if (name == "tab4") {
                 //alert(name);<a href="">rootComp_document.aspx</a>
                 document.location.href = "rootComp_document.aspx";
             }
             else if (name == "tab5") {
                 //alert(name);<a href="">rootComp_document.aspx</a>
                 document.location.href = "rootcompany_deductorinfo.aspx";
             }
             else if (name == "tab6") {
                 //alert(name);<a href="">rootComp_document.aspx</a>
                 document.location.href = "rootComp_Remarks.aspx";
             }


             else if (name == "tab7") {
                 document.location.href = "rootcompany_logo.aspx";
             }
         }



       </script>
        <style>
            #ASPxPageControl1 {
                width:100%;
            }
            .boxLayout {
               background: #f3efd3;
                margin-bottom: 15px;
                padding: 23px  0 9px 0;
                border: 1px solid #a59d66;
                position:relative;
            }
            .hdBoxLayout {
                position: absolute;
                background: #d2cca0;
                display: block;
                width: 100%;
                top: 0;
                padding: 5px 15px;
                border-bottom: 1px solid #b9b38a;
                    font-weight: 600;
            }
            .iconRed {
                position: absolute;
                right: -18px;
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

        #Td1
        {
            vertical-align: baseline;
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
            <h3>Deductor Info(TDS)</h3>
            <div class="crossBtn"><a href="root_Companies.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
        <asp:HiddenField runat="server" ID="Keyval_internalId" />
        <div class="form_main">
        <table class="TableMain100" style="width: 100%">
                                          <tr>  
                                              <td>

                                                <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="5" ClientInstanceName="page">
                                                 <TabPages>

                                                      <dxe:TabPage Text="General" Name="General">
                                                         <ContentCollection>
                                                           <dxe:ContentControl runat="server">
                                                            </dxe:ContentControl>
                                                          </ContentCollection>
                                                     </dxe:TabPage>

                                                      <dxe:TabPage Text="Correspondence" Name="CorresPondence">
                                                         <ContentCollection>
                                                          <dxe:ContentControl runat="server">
                                                               </dxe:ContentControl>
                                                         </ContentCollection>
                                                       </dxe:TabPage>


                                                      <dxe:TabPage Name="Exchange Segment" Visible="False" Text="Exchange Segment">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>


                                                     <dxe:TabPage Name="DP Memberships" Visible="False" Text="DP Memberships">
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


                               <dxe:TabPage Name="Deductor Info(TDS)" Text="Deductor Info(TDS)">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">

                                              

                                             <div class="boxLayout clearfix">
                                                 <div class="hdBoxLayout">DEDUCTOR(s) INFORMATION</div>
                                                 <div class="col-md-3">
                                                <label>Financial Year<span style="color:red;"> *</span> </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtfinyr" runat="server" Width="100%" MaxLength="6"></asp:TextBox>
                                                    <asp:Label ID="lblfinyr" runat="server" style="color: blue">Value should be 201920 for Financial Year 2019-2020 </asp:Label>
                                                     <span id="spFinyr" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                </div>
                                              </div>
                                        <div class="col-md-3">
                                                <label>Assessment Year<span style="color:red;"> *</span> </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtAssyear"  runat="server" Width="100%" MaxLength="6"></asp:TextBox>

                                                    <asp:Label ID="lblasstyr" runat="server" style="color: blue">Value should be 201920 for Assessment Year 2019-2020</asp:Label>
                                                     <span id="spAssyr" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                </div>
                                              </div>

                                                 <div class="col-md-3">
                                                <label>Name of Deductor<span style="color:red;"> *</span>  </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtNamedeductor" runat="server" Width="100%" MaxLength="75"></asp:TextBox>  <%--Rev Maynak Changes max length to 75 0021268--%>
                                                     <span id="spNamedeductor" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                </div>
                                              </div>


                                               <div class="col-md-3">
                                                <label>Deductor's Branch <span style="color:red;"> *</span> </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtBranchdeduct" runat="server" Width="100%" MaxLength="75"></asp:TextBox> <%--Rev Maynak Changes max length to 75 0021268--%>
                                                    <span id="spBranchdeduct" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                </div>
                                              </div>
                                                <div class="clear"></div>

                                                  <div class="col-md-3">
                                                <label>Deductors Address1<span style="color:red;"> *</span> </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtDeductaddr1" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                     <span id="spDeductaddr1" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                </div>
                                              </div>


                                                   <div class="col-md-3">
                                                <label>Deductors Address2</label>
                                                <div>
                                                    <asp:TextBox ID="txtDeductaddr2" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                              </div>


                                                   <div class="col-md-3">
                                                <label>Deductors Address3</label>
                                                <div>
                                                    <asp:TextBox ID="txtDeductaddr3" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                              </div>


                                                   <div class="col-md-3">
                                                <label>Deductors Address4</label>
                                                <div>
                                                    <asp:TextBox ID="txtDeductaddr4" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                              </div>
                                                <div class="clear"></div>

                                                   <div class="col-md-3">
                                                <label>Deductors Address5</label>
                                                <div>
                                                    <asp:TextBox ID="txtDeductaddr5" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                              </div>


                                                


                                                   <div class="col-md-3">
                                                <label>Deductors Address - PINCODE<span style="color:red;"> *</span> </label>
                                                <div class="relative">
                                                    <dxe:ASPxTextBox ID="txtDeductpin" ClientInstanceName="ctxtDeductpin" Enabled="true" runat="server" Width="100%" MaxLength="6">
                                                        <ClientSideEvents LostFocus="DeductorPinChange" />
                                                           </dxe:ASPxTextBox>
                                                    <asp:HiddenField ID="HdndeductPin" runat="server" />
                                                    <span id="spDeductpin" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                  <%--  <asp:SqlDataSource runat="server" ID="drddepin" SelectCommand="select pin_code Pincode,pin_id pinId from tbl_master_pinzip"></asp:SqlDataSource>--%>

                                                </div>
                                              </div>

                                        <div class="col-md-3">
                                                <label>Deductors Address - State<span style="color:red;"> *</span> </label>
                                                <div class="relative">
                                                  <dxe:ASPxTextBox ID="txtDeductState" ClientInstanceName="CtxtDeductState" runat="server" Width="100%" MaxLength="50" ClientEnabled="false"></dxe:ASPxTextBox>
                                                    <asp:HiddenField ID="hdnDeductStateCode" runat="server" />
                                                    <asp:HiddenField ID="hdnDeductStateid" runat="server" />
                                                    <span id="spDeductState" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                </div>
                                              </div>

                                          <%-- <div class="col-md-3">
                                                <label>Deductors Address - State</label>
                                                <div>
                                                     <dxe:ASPxComboBox ID="drdDeductState" ClientInstanceName="cdrdDeductState" Enabled="true"
                                                          runat="server" Width="100%" MaxLength="20" DataSourceID="drddeState" ValueField="stateId" TextField="statename">
                                                           </dxe:ASPxComboBox>
                                                    <asp:SqlDataSource runat="server" ID="drddeState" SelectCommand="select id stateId,state statename  from tbl_master_state"></asp:SqlDataSource>
                                                </div>
                                              </div>--%>
                                                   <div class="col-md-3">
                                                <label>Deductors EMAIL ID<span style="color:red;"> *</span> </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtDeductEmail" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                    <span id="spDedEmail" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                </div>
                                              </div>
                                        <div class="clear"></div>

                                                   <div class="col-md-3">
                                                <label>Deductors STD Code</label>
                                                <div>
                                                    <asp:TextBox ID="txtdeductSTD" runat="server" Width="100%" MaxLength="5">
                                                     </asp:TextBox>
                                                </div>
                                              </div>


                                                   <div class="col-md-3">
                                                <label>Deductors Tel-Phone No</label>
                                                <div>
                                                    <asp:TextBox ID="txtDeductTelNo" runat="server" Width="100%" MaxLength="10"></asp:TextBox>
                                                </div>
                                              </div>


                                                   <div class="col-md-6">
                                                       <div class="checkbox relative" style="padding-left: 0px !important;padding-top: 16px;">
                                                              <table>
                                                                  <tr>
                                                                      <td><dxe:ASPxCheckBox ID="ChkdeductAddrReturn"  runat="server" ClientInstanceName="cChkdeductAddrReturn"></dxe:ASPxCheckBox></td>
                                                                      <td> <dxe:ASPxLabel ID="lbldeductAddrReturn" Width="100%" runat="server" Text="Change of Address of Deductor since last Return">
                                                              </dxe:ASPxLabel>
                                                                           <span id="spdeductAddrReturn" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                                      </td>
                                                                  </tr>
                                                              </table>

                                                                        
                                                            </div>
                                                   </div>
                                             </div>


                                            <div class="boxLayout clearfix">
                                                 <div class="hdBoxLayout">DEDUCTOR RESPONSIBLE PERSON(S) INFORMATION</div>
                                                 <div class="col-md-3">
                                                <label>Name of Person responsible for Deduction<span style="color:red;"> *</span> </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtResponsibleDeduct" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                     <span id="spResponsibleDeduct" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                </div>
                                              </div>


                                                    <div class="col-md-3">
                                                <label>Designation of the Person responsible for Deduction<span style="color:red;"> *</span> </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtdeductdesig" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                     <span id="spdeductdesig" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                </div>
                                              </div>

 
                                                      <div class="col-md-3">
                                                <label>Responsible Person's Address1<span style="color:red;"> *</span> </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtPersaddr1" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                     <span id="spPersaddr1" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                </div>
                                              </div>

                                                      <div class="col-md-3">
                                                <label>Responsible Person's Address2</label>
                                                <div>
                                                    <asp:TextBox ID="txtPersaddr2" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                              </div>
                                        <div class="clear"></div>
                                                      <div class="col-md-3">
                                                <label>Responsible Person's Address3</label>
                                                <div>
                                                    <asp:TextBox ID="txtPersaddr3" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                              </div>

                                                      <div class="col-md-3">
                                                <label>Responsible Person's Address4</label>
                                                <div>
                                                    <asp:TextBox ID="txtPersaddr4" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                              </div>

                                                      <div class="col-md-3">
                                                <label>Responsible Person's Address5</label>
                                                <div>
                                                    <asp:TextBox ID="txtPersaddr5" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                              </div>

                                          <div class="col-md-3">
                                                <label>Responsible Person's PIN<span style="color:red;"> *</span> </label>
                                                <div class="relative">

                                                    <dxe:ASPxTextBox ID="txtpersPin" ClientInstanceName="CtxtpersPin" Enabled="true" runat="server" Width="100%" MaxLength="6">
                                                        <ClientSideEvents LostFocus="DeductorPersonPinChange" />
                                                           </dxe:ASPxTextBox>
                                                    <span id="spPersPin" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                    <asp:HiddenField ID="hdnPersPinId" runat="server" />

                                                    <%--<asp:SqlDataSource runat="server" ID="PersPin" SelectCommand="select pin_code RespPincode,pin_id RespPinId from tbl_master_pinzip"></asp:SqlDataSource>--%>
                                                </div>
                                              </div>

                                        <div class="clear"></div>

                                                    <div class="col-md-3">
                                                <label>Responsible Person's State<span style="color:red;"> *</span> </label>
                                                <div class="relative">

                                                    <dxe:ASPxTextBox ID="txtPersState" ClientInstanceName="CtxtPersState" runat="server" Width="100%" MaxLength="50" ClientEnabled="false"></dxe:ASPxTextBox>
                                                    <asp:HiddenField ID="hdnPersStateId" runat="server" />
                                                    <asp:HiddenField ID="hdnPersStateCode" runat="server" />
                                                    <span id="spPersState" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>

                                                <%--    <dxe:ASPxComboBox ID="drdPersState" ClientInstanceName="cdrdPersState" Enabled="true"
                                                          runat="server" Width="100%" MaxLength="20" DataSourceID="persState"  ValueField="RespstateId" TextField="Respstatename">
                                                           </dxe:ASPxComboBox>
                                                    <asp:SqlDataSource runat="server" ID="persState" SelectCommand="select id RespstateId,state Respstatename  from tbl_master_state"></asp:SqlDataSource>--%>
                                                </div>
                                              </div>
                                        
                                                  

                                                    <div class="col-md-3">
                                                <label>Responsible Person's Email ID<span style="color:red;"> *</span> </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtPersemail" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                    <span id="spPersEmail" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                </div>
                                              </div>

                                                    <div class="col-md-3">
                                                <label>Mobile number<span style="color:red;"> *</span> </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtMobile" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                    <span id="spmobile" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                </div>
                                              </div>

                                                    <div class="col-md-3">
                                                <label>Responsible Person's STD CODE</label>
                                                <div>
                                                    <asp:TextBox ID="txtPersSTD" runat="server" Width="100%" MaxLength="5"></asp:TextBox>
                                                </div>
                                              </div>
                                        <div class="clear"></div>
                                                    <div class="col-md-3">
                                                <label>Responsible Person's Tel-Phone No:</label>
                                                <div>
                                                    <asp:TextBox ID="txtPersTel" runat="server" Width="100%" MaxLength="10"></asp:TextBox>
                                                </div>
                                              </div>

                                            <div class="col-md-6">
                                                <div class="checkbox relative" style="padding-left: 0px !important;padding-top: 16px;">
                                                              <table>
                                                                  <tr>
                                                                      <td> <dxe:ASPxCheckBox ID="chkResPersaddr" runat="server" ClientInstanceName="CchkResPersaddr" TabIndex="8"></dxe:ASPxCheckBox></td>
                                                                      <td>
                                                                          <dxe:ASPxLabel ID="lblResPersaddr"  runat="server" Text="Change of Address of Responsible person since last Return">
                                                              </dxe:ASPxLabel>
                                                                          <span id="spResPersaddr" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                                      </td>
                                                                  </tr>
                                                            
                                                             
                                                                        </table>
                                                    </div>
                                            </div>
                                            </div>
                                             <div class="boxLayout clearfix">
                                                  <div class="hdBoxLayout">OTHER INFORMATION</div>
                                                  <div class="col-md-3">
                                                <label>State Name</label>
                                                <div class="relative">
                                                    <dxe:ASPxComboBox ID="drdTDSState" ClientInstanceName="cdrdTDSState" Enabled="true"
                                                          runat="server" Width="100%" MaxLength="20" DataSourceID="drTDSState" ValueField="TDSstateCode" TextField="TDSstatename">
                                                           </dxe:ASPxComboBox>

                                                    <asp:SqlDataSource runat="server" ID="drTDSState" SelectCommand="select TDSState TDSstateCode,state TDSstatename  from tbl_master_state"></asp:SqlDataSource>
                                                </div>
                                              </div>

                                                    <div class="col-md-3">
                                                <label>PAO Code</label>
                                                <div>
                                                      <asp:TextBox ID="drdpao" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                              </div>

                                                    <div class="col-md-3">
                                                <label>DDO Code</label>
                                                <div class="relative">
                                                     <asp:TextBox ID="drdDDO" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                   
                                                </div>
                                              </div>


                                                   

                                                    <div class="col-md-3">
                                                <label>Ministry Name</label>
                                                <div class="relative">
                                                   <dxe:ASPxComboBox ID="drdMinstryName" ClientInstanceName="cdrdMinstryName" Enabled="true"
                                                          runat="server" Width="100%" MaxLength="20" DataSourceID="drdministry" ValueField="Minstry_Code" TextField="Minstry_Name">
                                                           </dxe:ASPxComboBox>
                                                    <asp:SqlDataSource runat="server" ID="drdministry" SelectCommand="select Minstry_Name,Minstry_Code  from master_Ministry"></asp:SqlDataSource>
                                                </div>
                                              </div>
                                        <div class="clear"></div>

                                                    <div class="col-md-3">
                                                <label>Ministry Name Other</label>
                                                <div class="relative">
                                                   <asp:TextBox ID="txtOtherMinstryName" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                              </div>


                                                    <div class="col-md-3">
                                                <label>PAN of Responsible Person<span style="color:red;"> *</span> </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtRePanPers" runat="server" Width="100%" MaxLength="10"></asp:TextBox>
                                                    <span id="spRePanPers" class="pullleftClass fa fa-exclamation-circle iconRed" style="color:red;display:none;" ></span>
                                                </div>
                                              </div>

                                                    <div class="col-md-3">
                                                <label>PAO Registration No</label>
                                                <div>
                                                    <asp:TextBox ID="txtPaoNo" runat="server" Width="100%" MaxLength="7"></asp:TextBox>
                                                </div>
                                              </div>

                                                    <div class="col-md-3">
                                                <label>DDO Registration No</label>
                                                <div>
                                                    <asp:TextBox ID="txtDdoNo" runat="server" Width="100%" MaxLength="10"></asp:TextBox>
                                                </div>
                                              </div>
                                        <div class="clear"></div>
                                                    <div class="col-md-3">
                                                <label>Employer / Deductor's STD code (Alternate)</label>
                                                <div>
                                                    <asp:TextBox ID="txtEmpaltSTD" runat="server" Width="100%" MaxLength="5"></asp:TextBox>
                                                </div>
                                              </div>

                                                    <div class="col-md-3">
                                                <label>Employer / Deductor 's Tel-Phone No. (Alternate)</label>
                                                <div>
                                                    <asp:TextBox ID="txtEmpAltTel" runat="server" Width="100%" MaxLength="10"></asp:TextBox>
                                                </div>
                                              </div>


                                                  <div class="col-md-3">
                                                <label>Employer / Deductor Email ID (Alternate)</label>
                                                <div>
                                                    <asp:TextBox ID="txtEmpAltEmail" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                              </div>


                                                   <div class="col-md-3">
                                                <label>Responsible Person's STD Code (Alternate)</label>
                                                <div>
                                                    <asp:TextBox ID="txtPersAltSTD" runat="server" Width="100%" MaxLength="5"></asp:TextBox>
                                                </div>
                                              </div>
                                        <div class="clear"></div>
                                                   <div class="col-md-3">
                                                <label>Responsible Person's Tel-Phone No. (Alternate)</label>
                                                <div>
                                                    <asp:TextBox ID="txtPersAltTel" runat="server" Width="100%" MaxLength="10"></asp:TextBox>
                                                </div>
                                              </div>

                                                   <div class="col-md-3">
                                                <label>Responsible Person's Email ID (Alternate)</label>
                                                <div>
                                                    <asp:TextBox ID="txtResPersEmail" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                                </div>
                                              </div>


                                                   <div class="col-md-3">
                                                <label>Account Office Identification Number (AIN) of PAO/ TO/ CDDO </label>
                                                <div>
                                                    <asp:TextBox ID="txtAcctAIN" runat="server" Width="100%" MaxLength="7"></asp:TextBox>
                                                </div>
                                              </div>

                                                   <div class="col-md-3">
                                                <label>Goods and Service Tax Number (GSTN)</label>
                                                <div>
                                                    <asp:TextBox ID="txtGST" runat="server" Width="100%" MaxLength="15"></asp:TextBox>
                                                </div>
                                              </div>
                                             </div>
                                                <div class="clear"></div>
                                             
                                                  
                                            <div class="clear"></div>

                                                   


                                        <div class="clear"></div>
                                        <div class="" style="margin-top: 8px;">
                                            <asp:Button ID="btndeductSave" runat="server" Text="Save"  CssClass="btn btn-primary"  OnClick="Save_deductorInfo"
                                              OnClientClick="if(!IsDeductorDataValid()){ return false;}"       Width="73px"  />
                                            
                                         </div>



                                        </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>


                                                      <dxe:TabPage Name="UDF" Text="UDF" Visible="false">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>


                                                     <dxe:TabPage Name="Logo" Text="Logo">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                                                 
</TabPages>
 <clientsideevents activetabchanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
	                                            var Tab0 = page.GetTab(0);
	                                            var Tab1 = page.GetTab(1);
	                                            var Tab2 = page.GetTab(2);
	                                            var Tab3 = page.GetTab(3);
	                                            var Tab4 = page.GetTab(4);
                                                 var Tab5 = page.GetTab(5);
                                                var Tab6 = page.GetTab(6);
                                                var Tab7 = page.GetTab(7);
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
	                                            }"></clientsideevents>



</dxe:ASPxPageControl>
                                             </td>

                                              
                                          </tr>
           
    </table>

     </div>
    </div>
</asp:Content>
