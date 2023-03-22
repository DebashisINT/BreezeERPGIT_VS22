<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                22-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="Account Head" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_MainAccountHead" CodeBehind="MainAccountHead.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Js/MainAccountHead.js?v=1.0.0.1"></script>
    <script>
        function BankCashType(obj) {
            var actype = ASPxComboBox1.GetText();
            var asettype = '';




            if (actype == 'Asset') {
                if (obj == '0') {
                    //$(function () {

            <%-- asettype = '<%=Session["AssetType"]%>';--%>
            //asettype = document.getElementById('hdneditassettype').value;
            //if(asettype=='')
            //{
            //    asettype = obj;
            //}
            //});
            //document.getElementById('hdneditassettype').value = '';
           <%-- '<%Session["AssetType"] = null; %>';--%>
            document.getElementById("tdBankAccountType").style.display = 'none';
            document.getElementById("tdBankAccountType").style.display = 'none';
            document.getElementById("tdBankAccountType1").style.display = 'none';
            document.getElementById("tdBankAccountType1").style.display = 'none';
            document.getElementById("tdBankAccountNo").style.display = 'block';
            document.getElementById("clsPaymentType").style.display = 'block';
            document.getElementById("tdSubledgertype").style.display = 'none';
            ItemsBank();
            if (asettype == '0') {
                document.getElementById("tdtdsapprate").style.display = 'none';
                document.getElementById("tdtdsapprate1").style.display = 'none';
                $('#tdfbtapprate').closest('div.col-md-6').css({ 'display': 'none' });
                document.getElementById("tdfbtapprate").style.display = 'none';
                document.getElementById("tdfbtapprate1").style.display = 'none';
                document.getElementById("tdroi").style.display = 'none';
                document.getElementById("tdroi1").style.display = 'none';
                document.getElementById("tddepretion").style.display = 'none';
            }
            else if (asettype == '1') {
                document.getElementById("tdtdsapprate").style.display = 'none';
                document.getElementById("tdtdsapprate1").style.display = 'none';
                $('#tdfbtapprate').closest('div.col-md-6').css({ 'display': 'none' });
                document.getElementById("tdfbtapprate").style.display = 'none';
                document.getElementById("tdfbtapprate1").style.display = 'none';
                document.getElementById("tdroi").style.display = 'none';
                document.getElementById("tdroi1").style.display = 'none';
                document.getElementById("tddepretion").style.display = 'none';
                document.getElementById("tdBankAccountNo").style.display = 'none';
                // document.getElementById("clsPaymentType").style.display = 'none';//Priti
                // ItemaOther();
            }
            else if (asettype == '2') {

                document.getElementById("tddepretion").style.display = 'inline';
                document.getElementById("tdSubledgertype").style.display = 'block';
                // ItemaOther();
            }
            else if (asettype == '3') {
                document.getElementById("tdtdsapprate").style.display = 'block';
                document.getElementById("tdtdsapprate1").style.display = 'block';
                $('#tdfbtapprate').closest('div.col-md-6').css({ 'display': 'block' });
                document.getElementById("tdfbtapprate").style.display = 'block';
                document.getElementById("tdfbtapprate1").style.display = 'block';
                document.getElementById("tdroi").style.display = 'block';
                document.getElementById("tdroi1").style.display = 'block';
                document.getElementById("tddepretion").style.display = 'block';
            }
            document.getElementById("trCompanyName").style.display = 'block';
            // ItemaOther();
            //GetObjectID('hdnAssetType').value = '';
        }
        else {
            ItemaOther();
            if (obj == '1') {
                document.getElementById("tdBankAccountType").style.display = 'none';
                document.getElementById("tdBankAccountType1").style.display = 'none';
                document.getElementById("tdBankAccountNo").style.display = 'none';
                // document.getElementById("clsPaymentType").style.display = 'none';//Priti
                //document.getElementById("tdBankAccountNo1").style.display = 'none';
                document.getElementById("tdSubledgertype").style.display = 'none';
                //document.getElementById("tdSubledgertype1").style.display = 'none';
                //Added Later
                document.getElementById("tdtdsapprate").style.display = 'none';
                document.getElementById("tdtdsapprate1").style.display = 'none';
                $('#tdfbtapprate').closest('div.col-md-6').css({ 'display': 'none' });
                document.getElementById("tdfbtapprate").style.display = 'none';
                document.getElementById("tdfbtapprate1").style.display = 'none';
                document.getElementById("tdroi").style.display = 'none';
                document.getElementById("tdroi1").style.display = 'none';

                //document.getElementById("tdExchangeSeg").style.display = 'none';
                //document.getElementById("tdExchangeSeg1").style.display = 'none';
                document.getElementById("tddepretion").style.display = 'none';
                document.getElementById("trCompanyName").style.display = 'block';
                //ItemaOther();
            }
            else if (obj == '2') {

                $('#tddepretion').removeClass('hide');
                document.getElementById("tdBankAccountType").style.display = 'table-cell';// modified by atish for showing depreciation
                document.getElementById("tdBankAccountType1").style.display = 'table-cell';// modified by atish for showing depreciation
                document.getElementById("tdBankAccountNo").style.display = 'none';
                // document.getElementById("clsPaymentType").style.display = 'none';//Priti

                document.getElementById("tdSubledgertype").style.display = 'none';// modified by atish for not showing subledger type
                document.getElementById("tdtdsapprate").style.display = 'none';
                document.getElementById("tdtdsapprate1").style.display = 'none';
                $('#tdfbtapprate').closest('div.col-md-6').css({ 'display': 'none' });
                document.getElementById("tdfbtapprate").style.display = 'none';
                document.getElementById("tdfbtapprate1").style.display = 'none';
                document.getElementById("tdroi").style.display = 'none';
                document.getElementById("tdroi1").style.display = 'none';
                document.getElementById("tddepretion").style.display = 'inline';
                document.getElementById("trCompanyName").style.display = 'block';
                document.getElementById("tdSubledgertype").style.display = 'block';
                //ItemaOther();
            }
            else {
                document.getElementById("tdBankAccountType").style.display = 'none';
                document.getElementById("tdBankAccountType1").style.display = 'none';
                document.getElementById("tdBankAccountNo").style.display = 'none';
                //  document.getElementById("clsPaymentType").style.display = 'none';//Priti
                //document.getElementById("tdBankAccountNo1").style.display = 'none';
                document.getElementById("tdSubledgertype").style.display = 'inline';
                //document.getElementById("tdSubledgertype1").style.display = 'block';
                //Added Later
                document.getElementById("tdtdsapprate").style.display = 'inline';
                document.getElementById("tdtdsapprate1").style.display = 'inline';
                $('#tdfbtapprate').closest('div.col-md-6').css({ 'display': 'none' });
                document.getElementById("tdfbtapprate").style.display = 'inline';
                document.getElementById("tdfbtapprate1").style.display = 'inline';
                document.getElementById("tdroi").style.display = 'inline';
                document.getElementById("tdroi1").style.display = 'inline';
                //document.getElementById("tdExchangeSeg").style.display = 'none';
                //document.getElementById("tdExchangeSeg1").style.display = 'none';
                document.getElementById("tddepretion").style.display = 'none';
                document.getElementById("trCompanyName").style.display = 'block';
                // ItemaOther();
            }

        }
    }

        }
        function UniqueCodeCheck() {
            var strAccountCode = txtAccountCode.GetText()
            if (strAccountCode != null && strAccountCode != '') {
                if (strAccountCode.toLowerCase().indexOf("systm") > 0) {
                    txtAccountCode.Focus();
                    txtAccountCode.SetText();
                    alert('SYSTM can not use in unique short name');

                }
                else {
                    var Accountid = '0';
                    var id = '<%= Convert.ToString(Session["id"]) %>';
            //var strAccountCode = grid.GetEditor('AccountCode').GetValue();

            if ((id != null) && (id != '')) {
                Accountid = id;
                '<%=Session["id"]=null %>'
            }
            var CheckUniqueCode = false;
            $.ajax({
                type: "POST",
                url: "MainAccountHead.Aspx/CheckUniqueCode",
                data: JSON.stringify({ strAccountCode: strAccountCode, Accountid: Accountid }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    CheckUniqueCode = msg.d;
                    if (CheckUniqueCode == true) {
                        txtAccountCode.Focus();
                        txtAccountCode.SetText();
                        alert('Please enter unique short name');
                        //jAlert('Please enter unique short name');

                    }
                }
            });
        }
    }
}
    </script>
    <style>
    .truncated {
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
    }

        #MainAccountGrid_DXPEForm_efnew_ASPxComboBox1_ETC {
    display: none;
        }
         #txtNumber input{
             text-transform:uppercase;
         }
</style>

    <style>
        #MainAccountGrid_DXPEForm_DXEditingErrorRow {
            display: none;
        }

        #MainAccountGrid_DXPEForm_efnew_ASPxComboBox1_EC {
            position: absolute;
        }

        #mylay {
            background: red;
            width: 500px;
            height: 500px;
        }

        .hide {
            display: none;
        }

        #chkSysAccount + label.emph {
            font-weight: 600 !important;
        }

        #trAccountGroup .dxeErrorCellSys {
            position: absolute;
        }

        #MainAccountGrid_DXPEForm_efnew_txtRateofIntrest_EC, #MainAccountGrid_DXPEForm_efnew_txtDepreciation_EC,
        #tdroi .dxeErrorCellSys, #tddepretion .dxeErrorCellSys {
            display: none;
        }

        #cmb_tdstcs .dxeListBoxItem {
            height: 50px;
        }

        .dxgvSelectedRow_PlasticBlue td.dxgvCommandColumn_PlasticBlue a, .dxgvFocusedRow_PlasticBlue td.dxgvCommandColumn_PlasticBlue a, .dxgvPreviewRow_PlasticBlue td.dxgvCommandColumn_PlasticBlue a {
            color: #5A83D0 !important;
        }

        a.dxbButton_PlasticBlue {
            text-decoration: none;
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
    <div id="mylay" class="hide"></div>
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <%--<h3>&nbsp;&nbsp;Main Account Details</h3>--%>
            <h3>Account Head</h3>
        </div>
    </div>
        <div class="form_main">
        <table class="TableMain100">
            <%--   <tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099;">Main Account Details</span></strong>
                </td>
            </tr>--%>
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td style="text-align: left; vertical-align: top">
                                <table>
                                    <tr>
                                        <td id="ShowFilter">
                                            <% if (rights.CanAdd)
                                               { %>
                                            <a href="javascript:void(0);" id="btnAddNew" runat="server" onclick="AddButtonClick()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Add New</span></a>
                                            <%} %>
                                            <%--   <a href="javascript:ShowHideFilter('s');" class="btn btn-success"><span >
                                                Show Filter</span></a>--%>
                                            <% if (rights.CanExport)
                                               { %>
                                            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnChange="if(!AvailableExportOption()){return false;}"
                                                OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="Static">
                                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                                <asp:ListItem Value="2">XLS</asp:ListItem>
                                                <asp:ListItem Value="3">RTF</asp:ListItem>
                                                <asp:ListItem Value="4">CSV</asp:ListItem>
                                            </asp:DropDownList>
                                            <% } %>
                                            <div id="Td1" style="display: none">
                                                <a href="javascript:ShowHideFilter('All');" class="btn btn-primary btn-radius"><span>All Records</span></a>
                                            </div>
                                            <asp:CheckBox ID="chkSysAccount" runat="server" Checked="True" Text="Hide System Accounts" Font-Bold="True" ForeColor="Blue" /><span>
                                            </span>
                                            <%--<a href="javascript:void(0);" id="A2" runat="server" onclick="VerifyButtonClick();" class="btn btn-primary"><span>Verify</span></a>--%>
                                            
                                        </td>

                                    </tr>
                                </table>
                            </td>
                            <td></td>
                            <%--<td class="gridcellright pull-right">
                                <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true"
                                    Font-Bold="False" ForeColor="black" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                    ValueType="System.Int32" Width="130px">
                                    <items>
                                        <dxe:ListEditItem Text="Select" Value="0" />
                                        <dxe:ListEditItem Text="PDF" Value="1" />
                                        <dxe:ListEditItem Text="XLS" Value="2" />
                                        <dxe:ListEditItem Text="RTF" Value="3" />
                                        <dxe:ListEditItem Text="CSV" Value="4" />
                                    </items>
                                    <buttonstyle>
                                    </buttonstyle>
                                    <itemstyle>
                                        <HoverStyle>
                                        </HoverStyle>
                                    </itemstyle>
                                    <border bordercolor="black" />
                                    <dropdownbutton text="Export">
                                    </dropdownbutton>
                                </dxe:ASPxComboBox>
                            </td>--%>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="relative">
                    <%--   <dxe:ASPxCallbackPanel ID="ASPxCallbackPanel1" ClientInstanceName="panel" runat="server" Width="411px">
                        <PanelCollection>
                            <dxe:PanelContent runat="server" SupportsDisabledAttribute="True">&nbsp;<br /> &nbsp;<br /> &nbsp;<br /> &nbsp;<br /> &nbsp;<br />
    <br />>

                            </dxe:PanelContent>

                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>--%>
                    <dxe:ASPxGridView ID="MainAccountGrid" runat="server" AutoGenerateColumns="False" Images-ContextMenuShowFooter-AlternateText=""
                        KeyFieldName="MainAccount_ReferenceID" ClientInstanceName="grid" DataSourceID="MainAccount" OnInitNewRow="MainAccountGrid_InitNewRow"
                        Width="100%" OnRowUpdating="MainAccountGrid_OnRowUpdating" OnCustomCallback="MainAccountGrid_CustomCallback"
                        OnRowValidating="MainAccountGrid_OnRowValidating" OnHtmlDataCellPrepared="MainAccountGrid_OnHtmlDataCellPrepared"
                        OnRowInserting="MainAccountGrid_OnRowInserting" OnHtmlEditFormCreated="MainAccountGrid_HtmlEditFormCreated"
                        OnStartRowEditing="MainAccountGrid_StartRowEditing" OnHtmlRowCreated="MainAccountGrid_HtmlRowCreated"
                        OnCustomJSProperties="MainAccountGrid_CustomJSProperties" OnRowCommand="MainAccountGrid_RowCommand"
                        OnCommandButtonInitialize="MainAccountGrid_CommandButtonInitialize"
                        OnRowDeleting="MainAccountGrid_RowDeleting">
                        <%--SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" --%>
                        <SettingsSearchPanel Visible="True" Delay="7000" />
                        <%-- <Settings ShowHorizontalScrollBar="true" />
                        <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True">
                        </SettingsPager>--%>



                        <ClientSideEvents EndCallback="function(s,e) { OnGridEndCallback(s,e);}" CustomButtonClick="function(s,e) {OpenMappingLedgerPopup(s,e);}" />



                        <Settings ShowGroupedColumns="True" ShowGroupPanel="True" />
                        <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="true" />
                        <SettingsText PopupEditFormCaption="Add Main Account" ConfirmDelete="Confirm delete?" />
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="True" />

                        <Styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>

                            <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>

                            <FocusedRow CssClass="gridselectrow"></FocusedRow>

                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>

                        <Templates>
                            <EditForm>
                                <div id="main">
                                    <div style="padding: 10px;"></div>

                                    <%--Account Name and Account Code or Short Name--%>
                                    <div class="col-md-12 clearfix" id="trAccountName">
                                        <div class="col-md-6" style="margin-bottom: 5px">
                                            <label>Account Name :<span style="color: Red;">*</span></label>
                                            <dxe:ASPxTextBox ID="txtAccountNo" ClientInstanceName="txtAccountNo" runat="server"
                                                Text='<%#Bind("AccountName") %>' Width="100%" MaxLength="50" ValidationSettings-ValidationGroup="<%# Container.ValidationGroup %>">
                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right">
                                                    <RequiredField IsRequired="true" ErrorText="Mandatory" />
                                                </ValidationSettings>
                                            </dxe:ASPxTextBox>

                                            <%--<div id="valid" style="position: absolute; right: -4px; top: 30px;" class="hide">
                                                <img id="grid_DXPEForm_DXEFL_DXEditor2_EI" title="Mandatory" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-YRohc" alt="Required" />
                                            </div>--%>
                                        </div>



                                        <div class="col-md-6" style="margin-bottom: 5px">
                                            <label>Short Name:<span style="color: Red;">*</span></label>
                                            <dxe:ASPxTextBox ID="txtAccountCode" ClientInstanceName="txtAccountCode" runat="server"
                                                Text='<%#Bind("AccountCode") %>' Width="100%" MaxLength="50" CssClass="gridcellleft" ValidationSettings-ValidationGroup="<%# Container.ValidationGroup %>">
                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right">
                                                    <RequiredField IsRequired="true" ErrorText="Mandatory" />
                                                </ValidationSettings>

                                                <ClientSideEvents TextChanged="function(s, e) {UniqueCodeCheck();}" />
                                                <ClientSideEvents KeyPress="function(s,e){window.setTimeout('updateEditorText()', 10);}" />
                                                <ClientSideEvents Init="OntxtAccountCodeInit" />
                                            </dxe:ASPxTextBox>
                                            <%--<div id="validaccode" style="position: absolute; right: -4px; top: 30px;" class="hide">
                                                <img id="grid_DXPEForm_DXEFL_DXEditor2_EI" title="Mandatory" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-YRohc" alt="Required" />
                                            </div>--%>
                                        </div>
                                    </div>


                                    <div class="col-md-12 clearfix" id="trAccountGroup">
                                        <div class="col-md-6">
                                            <label>Account Type :<span style="color: Red;">*</span></label>
                                            <dxe:ASPxComboBox ID="ASPxComboBox1" ClientInstanceName="ASPxComboBox1" runat="server"
                                                ValueType="System.String" Value='<%#Bind("AccountType") %>' Width="90%" EnableIncrementalFiltering="true"
                                                ValidationSettings-ValidationGroup='<%# Container.ValidationGroup %>' ValidationSettings-RequiredField-ErrorText="Mandatory">
                                                <Items>

                                                    <dxe:ListEditItem Text="Asset" Value="Asset" Selected="true"></dxe:ListEditItem>
                                                    <dxe:ListEditItem Text="Liability" Value="Liability"></dxe:ListEditItem>
                                                    <dxe:ListEditItem Text="Income" Value="Income"></dxe:ListEditItem>
                                                    <%-- <dxe:ListEditItem Text="Expenses" Value="Expences"></dxe:ListEditItem>--%>
                                                    <dxe:ListEditItem Text="Expense" Value="Expense"></dxe:ListEditItem>
                                                </Items>
                                                <ValidationSettings ErrorImage-ToolTip="Mandatory">
                                                    <RequiredField IsRequired="true" />
                                                </ValidationSettings>
                                                <%--<ValidationSettings CausesValidation="True" SetFocusOnError="True" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right">
                                                                                        <RequiredField IsRequired="True" ErrorText="Select category"  />
                                                                                    </ValidationSettings>--%>
                                                <ClientSideEvents ValueChanged="function(s,e){
                                                                                          var indexr = s.GetSelectedIndex();
                                                                                          AccopuntType(indexr)
                                                                                         }" />
                                                <ClientSideEvents Init="function(s,e){
                                                                                           var indexr = s.GetSelectedIndex();
                                                                                           AccopuntType(indexr);
                                                      }" />
                                            </dxe:ASPxComboBox>
                                        </div>
                                        <div class="col-md-6">
                                            <label>Account Group :</label>
                                            <dxe:ASPxComboBox ID="combAccountGroup" ClientInstanceName="combAccountGroup" runat="server" OnCallback="combAccountGroup_Callback"
                                                ValueType="System.String" EnableIncrementalFiltering="true" Width="90%" DataSourceID="AllAccountGroup"
                                                TextField="AccountGroup" ValueField="ID" Value='<%#Bind("AccountGroup") %>'>
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>
                                    <div style="clear: both"></div>
                                    <div class="col-md-12 clearfix" id="trBankCashType">
                                        <div class="col-md-6">
                                            <label>
                                                Asset Type :
                                                <span style="color: Red;">*</span>
                                            </label>
                                            <div>
                                                <dxe:ASPxComboBox ID="ASPxComboBox2" ClientInstanceName="ASPxComboBox2" runat="server"
                                                    ValueType="System.String" Value='<%#Bind("BankCashType") %>' EnableIncrementalFiltering="true"
                                                    Width="90%" SelectedIndex="0">

                                                    <Items>
                                                        <dxe:ListEditItem Text="Bank" Value="Bank" Selected="true" />
                                                        <dxe:ListEditItem Text="Cash" Value="Cash" />
                                                        <dxe:ListEditItem Text="Fixed Asset" Value="Fixed Asset" />
                                                        <dxe:ListEditItem Text="Other" Value="Other" />
                                                    </Items>

                                                    <ClientSideEvents Init="function(s,e){
                                                                                           var indexr = s.GetSelectedIndex();
                                                                                           BankCashType(indexr);
                                                   }" />
                                                    <ClientSideEvents ValueChanged="function(s,e){
                                                                                           var indexr = s.GetSelectedIndex();
                                                                                           BankCashType(indexr);
                                                                                        
                                                   }" />
                                                    <%--Init="function(s,e){
                                                    
                                                            if(s.GetText()=='')
                                                                {
                                                    
                                                                    $('#tdBankAccountType').attr('style','display:none');
                                                                    $('#tdBankAccountType1').attr('style','display:none');
                                                                }
                                                            else
                                                                {
                                                                     $('#tdBankAccountType').attr('style','display:block');
                                                                     $('#tdBankAccountType1').attr('style','display:block');
                                                                }
                                                            }" />--%>
                                                </dxe:ASPxComboBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6" id="tdBankAccountNo">
                                            <label>Bank Account No :</label>
                                            <div>
                                                <dxe:ASPxTextBox ID="txtBankAccountNo" ClientInstanceName="txtBankAccountNo" runat="server"
                                                    Text='<%#Bind("BankAccountNo") %>' Width="90%" MaxLength="20">
                                                </dxe:ASPxTextBox>
                                            </div>
                                            <%-- <div id="validbankaccno" style="position: absolute; right: -4px; top: 30px;" class="hide">
                                                <img id="grid_DXPEForm_DXEFL_DXEditor2_EI" title="Mandatory" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-YRohc" alt="Required" />
                                            </div>--%>
                                        </div>
                                    </div>
                                    <div class="col-md-12 clearfix" id="trCompanyName">
                                        <div class="col-md-6">
                                            <label>
                                                Company Name :<span style="color: Red;">*</span>
                                                <%-- <span style="color: Red;">*</span>--%>
                                            </label>
                                            <div>
                                                <dxe:ASPxComboBox ID="comboCompanyName" ClientInstanceName="comboCompanyName" runat="server"
                                                    ValueType="System.String" DataSourceID="SqlCompany" ValueField="cmp_internalId"
                                                    TextField="cmp_name" EnableIncrementalFiltering="true" Width="90%">
                                                    <%--<ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right"     >
                                                  <RequiredField IsRequired="true" ErrorText="Mandatory"  />
                                                  </ValidationSettings>--%>

                                                    <%--<ClientSideEvents ValueChanged="function(s,e){CompanyExchange(s.GetValue());
                                                                                          }" />--%>
                                                </dxe:ASPxComboBox>
                                                <span id="MandatoryProduct" style="display: none" class="validclass">
                                                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                            </div>
                                        </div>







                                        <div class="col-md-6" id="divBranch">
                                            <label>
                                                Branch :<span style="color: Red;">*</span>
                                                <%-- <span style="color: Red;">*</span>--%>
                                            </label>
                                            <div>
                                                <dxe:ASPxComboBox ID="CmbBranch" ClientInstanceName="CmbBranch" runat="server"
                                                    ValueType="System.String" DataSourceID="branchdtl" ValueField="branch_id"
                                                    TextField="branch_description" EnableIncrementalFiltering="true"
                                                    Width="90%" AutoPostBack="false">
                                                    <ClientSideEvents SelectedIndexChanged="CmbBranchChanged" Init="CmbBranchChanged" />
                                                </dxe:ASPxComboBox>
                                                <input type="button" onclick="MultiBranchClick()" class="btn btn-small btn-primary" value="Select Specific Branch" id="MultiBranchButton"></input>
                                            </div>
                                            <%-- Value='<%#Bind("branchname") %>'--%>
                                            <%--<div id="validsubledgtyp" style="position: absolute; right: -4px; top: 30px;" class="hide">
                                                <img id="grid_DXPEForm_DXEFL_DXEditor2_EI" title="Mandatory" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-YRohc" alt="Required" />
                                            </div>--%>
                                        </div>
                                        <%--<div class="col-md-6">
                                            <div style="text-align: left;" id="td4"></div>
                                            <div style="text-align: left;" id="td5"></div>
                                        </div>--%>
                                        <div class="col-md-6" id="tdSubledgertype" style="display: none">
                                            <label>
                                                Sub-Ledger Type :
                                               <%-- <span style="color: Red;">*</span>--%>
                                            </label>
                                            <div>
                                                <dxe:ASPxComboBox ID="CmbSubLedgerType" ClientInstanceName="CmbSubLedgerType" runat="server"
                                                    ValueType="System.String" EnableIncrementalFiltering="true" Value='<%#Bind("SubLedgerType") %>'
                                                    Width="90%" AutoPostBack="false">
                                                    <Items>
                                                        <dxe:ListEditItem Text="None" Value="None"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="Customers" Value="Customers "></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="Employees" Value="Employees"></dxe:ListEditItem>
                                                        <%-- <dxe:ListEditItem Text="Sub Brokers" Value="Sub Brokers "></dxe:ListEditItem>--%>
                                                        <%--  <dxe:ListEditItem Text="Relationship Partners" Value="Relationship Partners"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="Business Partners" Value="Business Partners"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="Franchisees" Value="Franchisees"></dxe:ListEditItem>--%>
                                                        <dxe:ListEditItem Text="Vendors" Value="Vendors "></dxe:ListEditItem>
                                                        <%-- <dxe:ListEditItem Text="Data Vendors" Value="Data Vendors"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="Recruitment Agents" Value="Recruitment Agents "></dxe:ListEditItem>--%>
                                                        <dxe:ListEditItem Text="Agents" Value="Agents"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="Custom" Value="Custom"></dxe:ListEditItem>
                                                        <%-- <dxe:ListEditItem Text="Products-Equity" Value="Products-Equity"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="Products-Commodity " Value="Products-Commodity  "></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="Products-MF" Value="Products-MF"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="Products-Insurance" Value="Products-Insurance "></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="Products-ConsumerFinance" Value="Products-ConsumerFinance"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="RTAs" Value="RTAs "></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="MFs" Value="MFs"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="AMCs" Value="AMCs "></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="Insurance Cos" Value=" Insurance Cos"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="Consumer Finance Cos " Value="Consumer Finance Cos  "></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="Custodians" Value="Custodians "></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="NSDL Clients" Value="NSDL Clients"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="CDSL Clients" Value="CDSL Clients"></dxe:ListEditItem>--%>
                                                        <%--<dxe:ListEditItem Text="Consultants" Value="Consultants"></dxe:ListEditItem>--%>
                                                        <dxe:ListEditItem Text="Driver/Transporter" Value="DriverTransporter"></dxe:ListEditItem>
                                                        <%--<dxe:ListEditItem Text="Share Holder" Value="Share Holder"></dxe:ListEditItem>--%>
                                                        <%-- <dxe:ListEditItem Text="Debtors" Value="Debtors"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="Creditors" Value="Creditors"></dxe:ListEditItem>--%>
                                                        <%--<dxe:ListEditItem Text="Brokers" Value="Brokers"></dxe:ListEditItem>--%>
                                                    </Items>

                                                    <%--<ClientSideEvents ValueChanged="function(s,e){
                                                                                                                    var indexr = s.GetSelectedIndex();
                                                                                                                    SubLedgerTypeFun(indexr)
                                                                                                                    }" />--%>
                                                </dxe:ASPxComboBox>
                                            </div>
                                            <%--<div id="validsubledgtyp" style="position: absolute; right: -4px; top: 30px;" class="hide">
                                                <img id="grid_DXPEForm_DXEFL_DXEditor2_EI" title="Mandatory" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-YRohc" alt="Required" />
                                            </div>--%>
                                        </div>
                                    </div>
                                    <div class="col-md-12" id="trBaAccountType" style="display: none">
                                        <div class="col-md-6">
                                            <label id="tdBankAccountType">
                                                Depreciation :<div class="hidden">Bank Account Type :</div>
                                                <%-- <span style="color: Red;">*</span>--%>
                                            </label>
                                            <div id="tdBankAccountType1">
                                                <div class="hidden">
                                                    <dxe:ASPxComboBox ID="ASPxComboBox3" ClientInstanceName="ASPxComboBox3" runat="server"
                                                        ValueType="System.String" Width="90%"
                                                        SelectedIndex="0" EnableIncrementalFiltering="true">
                                                        <Items>
                                                            <dxe:ListEditItem Text="Clearing" Value="Clearing"></dxe:ListEditItem>
                                                            <dxe:ListEditItem Text="Client" Value="Client"></dxe:ListEditItem>
                                                            <dxe:ListEditItem Text="Own" Value="Own"></dxe:ListEditItem>
                                                        </Items>

                                                        <ClientSideEvents ValueChanged="function(s,e){
                                                                                            var indexr = s.GetSelectedIndex();
                                                                                            BankAccountType(indexr)
                                                                                          }" />
                                                    </dxe:ASPxComboBox>
                                                </div>
                                                <%-- <table>--%>
                                                <%--<tr id="trDepreciationtext">
                                                        <td>
                                                            <dxe:ASPxTextBox ID="txtDepreciation" ClientInstanceName="txtDepreciation" runat="server"
                                                                Text='<%#Bind("Depreciation") %>' Width="100%" MaskSettings-Mask="<0..9999g>.<00..99>"
                                                                ValidationSettings-ErrorDisplayMode="None" MaskSettings-IncludeLiterals="DecimalSymbol">
                                                            </dxe:ASPxTextBox>
                                                        </td>
                                                        <td>%
                                                        </td>
                                                    </tr>--%>
                                                <%-- </table>--%>
                                            </div>
                                        </div>
                                    </div>

                                    <div style="clear: both"></div>
                                    <div class="col-md-12" id="trsubledgertype">
                                        <%--// .............................Code Commented and Added by Sam on 15122016.due to unnecessary Code because segment id is always 1 .....................................--%>
                                        <%-- <div class="col-md-6">
                                            <label style="text-align: left;" id="tdExchangeSeg">
                                                <div class="hidden">Exchange Segment :</div>
                                            </label>
                                            <div id="tdExchangeSeg1">
                                                <dxe:ASPxRadioButtonList ID="rbSegment" ClientInstanceName="rbSegment" runat="server"
                                                    SelectedIndex="0" ItemSpacing="0px" RepeatDirection="Horizontal" TextWrap="False"
                                                    Font-Size="12px" ValueField='<%#Bind("ExchengSegment")%>' ValueType="System.String">
                                                    <Items>
                                                        <dxe:ListEditItem Text="All" Value="A" />
                                                        <dxe:ListEditItem Text="Specific" Value="S" />
                                                    </Items>
                                                    <ClientSideEvents ValueChanged="function(s,e){   var indexr = s.GetValue();
                                                                                                       ExchangeSegment(indexr)
                                                                                                     }" />
                                                    <Border BorderWidth="0px" />
                                                </dxe:ASPxRadioButtonList>
                                            </div>
                                        </div>--%>
                                        <%--// .............................Code Above Commented and Added by Sam on 15122016...................................... --%>
                                        <div class="col-md-6" id="tdroi">
                                            <label style="text-align: left;">Rate Of Interest (P/a) :</label>
                                            <div style="text-align: left;" id="tdroi1">
                                                <dxe:ASPxTextBox ID="txtRateofIntrest" ClientInstanceName="txtRateofIntrest" runat="server"
                                                    Text='<%#Bind("RateOfIntrest") %>' Width="90%">
                                                    <MaskSettings Mask="<0..9999g>.<00..99>" ErrorText="None" IncludeLiterals="DecimalSymbol" />
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <%--// .............................Code Commented and Added by Sam on 15122016.due to unnecessary Code because segment id is always 1 .....................................--%>
                                        <%--<div class="col-md-6" id="trExchengSegment11">
                                            <label style="display: none; margin-top: 5px;" id="trExchange">Segment Name :
                                             <span style="color: Red;">*</span> 

                                            </label>
                                            <div style="display: none;" id="trExchange1">
                                                <dxe:ASPxComboBox ID="comboSegment" ClientInstanceName="comboSegment" runat="server"
                                                    ValueType="System.String" DataSourceID="SqlSegment" ValueField="exch_internalId"
                                                    TextField="Exchange" EnableIncrementalFiltering="true" Width="100%" OnCallback="comboSegment_Callback">

                                                    <ClientSideEvents ValueChanged="function(s,e){
                                                                                            SegmentID1(s.GetValue());
                                                                                          }"
                                                        EndCallback="function(s,e){SetSegValue(s.GetValue());}" />

                                                </dxe:ASPxComboBox>
                                                <asp:TextBox ID="txtSpefificExchange_hidden" runat="server" Visible="false"></asp:TextBox>
                                            </div>
                                        </div>--%>
                                        <%--// .............................Code Above Commented and Added by Sam on 15122016......................................--%>
                                        <div class="col-md-6">
                                            <label id="tdfbtapprate" style="display: none">
                                                <div class="hidden">FBT Applicable :</div>
                                            </label>
                                            <div id="tdfbtapprate1" style="display: none">
                                                <div style="display: none">
                                                    <dxe:ASPxCheckBox ID="FBTApplicable" ClientInstanceName="FBTApplicable" runat="server"
                                                        Width="50px" Checked='<%# Container.Grid.IsNewRowEditing ? false : Container.Grid.GetRowValues(Container.VisibleIndex, "FBTApplicable") %>' />
                                                </div>
                                                <label id="fbtrate1">
                                                    <div style="display: none">FBT Rate :</div>
                                                </label>
                                                <div id="fbtrate2">
                                                    <%--ValidationSettings-ErrorDisplayMode="None"--%>
                                                    <div style="display: none">
                                                        <dxe:ASPxTextBox ID="txtFBTRate" ClientInstanceName="txtFBTRate" runat="server" Text='<%#Bind("FBTRate") %>'
                                                            Width="90%" MaskSettings-Mask="<0..9999g>.<00..99>"
                                                            MaskSettings-IncludeLiterals="DecimalSymbol">
                                                        </dxe:ASPxTextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6" id="tdtdsapprate">
                                            <label style="margin-top: 5px;">TDS Section:</label>
                                            <div id="tdtdsapprate1">


                                                <%-- <asp:TextBox ID="txtTdsType" runat="server" CssClass="form-control" Text='<%#Bind("TDSApplicable") %>'
                                                    onkeyup="CallTdsAccount(this,'SearchTdsTcsCode',event)" Width="100%"></asp:TextBox>
                                                <asp:HiddenField ID="txtTdsType_hidden" runat="server" Value='<%#Bind("TDSRate") %>' />--%>

                                                <%--// .............................Code Commented and Added by Sam on 09122016. ..................................... --%>

                                                <dxe:ASPxComboBox ID="cmb_tdstcs" ClientInstanceName="cmb_tdstcs" DataSourceID="tdstcs" Width="90%" Value='<%#Bind("TDSRate") %>' ItemStyle-Wrap="True"
                                                    ClearButton-DisplayMode="Always" runat="server" TextField="tdsdescription" ValueField="tdscode">
                                                </dxe:ASPxComboBox>


                                                <%--// .............................Code Above Commented and Added by Sam on 09122016...................................... --%>
                                            </div>
                                        </div>
                                        <div class="col-md-6 hide" id="tddepretion">
                                            <label style="text-align: left;">Depreciation :</label>
                                            <%--ValidationSettings-ErrorDisplayMode="None"--%>
                                            <div>
                                                <dxe:ASPxTextBox ID="txtDepreciation" ClientInstanceName="txtDepreciation" runat="server"
                                                    Text='<%#Bind("Depreciation") %>' Width="90%" MaskSettings-Mask="<0..9999g>.<00..99>"
                                                    MaskSettings-IncludeLiterals="DecimalSymbol">
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>

                                        <div class=" clear"></div>
                                        <div class="col-md-6" id="clsPaymentType">
                                            <label style="text-align: left;">Select Posting Type</label>
                                            <div>
                                                <dxe:ASPxComboBox ID="cPaymenttype" ClientInstanceName="cPaymenttype" runat="server"
                                                    ValueType="System.String" Value='<%#Bind("MainAccount_PaymentType") %>' EnableIncrementalFiltering="true"
                                                    Width="90%" SelectedIndex="0">

                                                    <Items>
                                                        <dxe:ListEditItem Text="None" Value="None" Selected="true" />
                                                        <dxe:ListEditItem Text="Card" Value="Card" />
                                                        <dxe:ListEditItem Text="Coupon" Value="Coupon" />
                                                        <dxe:ListEditItem Text="Etransfer" Value="Etransfer" />
                                                        <dxe:ListEditItem Text="Ledger for Interstate Stk-Out" Value="LedgOut" />
                                                        <dxe:ListEditItem Text="Ledger for Interstate Stk-In" Value="LedgIn" />

                                                        <dxe:ListEditItem Text="Finance Processing Fee" Value="PrcFee" />
                                                        <dxe:ListEditItem Text="Finance Other Charges Emi" Value="EmiCharge" />
                                                    </Items>
                                                    <ClientSideEvents Init="function(s,e){ 
                                                          if(s.GetValue() ==null){
                                                                s.SetValue('None');
                                                          }
                                                          
                                                   }" />
                                                </dxe:ASPxComboBox>
                                            </div>
                                        </div>

                                        <div class="col-md-3">
                                            <label style="text-align: left;">Old Unit Ledger</label>
                                            <div>
                                                <dxe:ASPxComboBox ID="cmbOldUnitLedger" ClientInstanceName="ccmbOldUnitLedger" runat="server"
                                                    ValueType="System.String" Value='<%#Bind("MainAccount_OldUnitLedger")%>' EnableIncrementalFiltering="true"
                                                    Width="90%">

                                                    <Items>
                                                        <dxe:ListEditItem Text="Yes" Value="1" Selected="true" />
                                                        <dxe:ListEditItem Text="No" Value="0" />


                                                    </Items>
                                                    <ClientSideEvents Init="function(s,e){ 
                                                           if(s.GetText()==''){
                                                           s.SetValue(0);
                                                          }else{
                                                          s.SetValue(s.GetText());
                                                          }
                                                   }" />
                                                </dxe:ASPxComboBox>
                                            </div>
                                        </div>

                                        <div class=" clear"></div>
                                        <div class="col-md-3">
                                            <label style="text-align: left;">Reverse Applicable</label>
                                            <div>
                                                <dxe:ASPxComboBox ID="cmbReverseApplicable" ClientInstanceName="ccmbReverseApplicable" runat="server"
                                                    ValueType="System.String" Value='<%#Bind("MainAccount_ReverseApplicable")%>' EnableIncrementalFiltering="true"
                                                    Width="90%">

                                                    <Items>
                                                        <dxe:ListEditItem Text="Yes" Value="1" Selected="true" />
                                                        <dxe:ListEditItem Text="No" Value="0" />


                                                    </Items>
                                                    <ClientSideEvents Init="function(s,e){ 
                                                           if(s.GetText()==''){
                                                           s.SetValue(0);
                                                          }else{
                                                          s.SetValue(s.GetText());
                                                          }
                                                   }" />
                                                </dxe:ASPxComboBox>
                                            </div>
                                        </div>


                                    </div>
                                </div>
                                <%--main end--%>
                                <table style="text-align: left; width: 100%;" border="0" id="main">

                                    <tr>
                                    </tr>

                                </table>
                                <div class="col-md-12">
                                    <div class="col-md-12">
                                        <controls></controls>
                                        <dxe:ASPxGridViewTemplateReplacement ID="Editors" runat="server" ColumnID="" ReplacementType="EditFormEditors"></dxe:ASPxGridViewTemplateReplacement>
                                        <div style="padding: 2px 2px 2px 2px; font-weight: bold;">
                                            <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton" class="btn btn-primary"
                                                runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                            <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton" class="btn btn-danger"
                                                runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                            <dxe:ASPxButton ID="btnSaveUdf" ClientInstanceName="cbtn_SaveUdf" runat="server" AutoPostBack="False" Text="UDF" UseSubmitBehavior="False"
                                                CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                                <ClientSideEvents Click="function(s, e) {OpenUdf();}" />
                                            </dxe:ASPxButton>
                                        </div>
                                    </div>
                                </div>

                            </EditForm>
                        </Templates>
                        <%--<SettingsContextMenu Enabled="true"></SettingsContextMenu>--%>
                        <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" AlwaysShowPager="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                        </SettingsPager>
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="450px" PopupEditFormHorizontalAlign="WindowCenter"
                            PopupEditFormModal="False" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="600px" />


                        <%--<settingsbehavior confirmdelete="True" allowfocusedrow="False" />--%>
                        <SettingsBehavior AllowFocusedRow="true" />


                        <%--  <settingscommandbutton>
                            <EditButton Image-Url="/assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                                <Image AlternateText="Edit" Url="/assests/images/Edit.png"></Image>
                                
                            </EditButton>

                            <DeleteButton Image-Url="/assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete" Styles-Style-CssClass="pad">
                                <Image AlternateText="Edit" Url="/assests/images/Delete.png"></Image>

                            </DeleteButton>

                            <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary">
                                <Styles>
                                    <Style CssClass="btn btn-primary"></Style>
                                </Styles>
                            </UpdateButton>
                            <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger">
                                <Styles>
                                    <Style CssClass="btn btn-danger"></Style>
                                </Styles>
                            </CancelButton>
                        </settingscommandbutton>--%>


                        <StylesEditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </StylesEditors>

                        <Columns>
                            <dxe:GridViewDataComboBoxColumn FieldName="AccountType" VisibleIndex="1" ShowInCustomizationForm="True">


                                <PropertiesComboBox ValueType="System.String" EnableIncrementalFiltering="True">
                                    <Items>

                                        <dxe:ListEditItem Text="Asset" Value="Asset" Selected="true"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Liability" Value="Liability"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Income" Value="Income"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Expense" Value="Expense"></dxe:ListEditItem>

                                    </Items>
                                </PropertiesComboBox>
                                <CellStyle CssClass="gridcellright">
                                </CellStyle>
                                <EditFormSettings Visible="False" VisibleIndex="7"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>

                            <dxe:GridViewDataComboBoxColumn Visible="False" FieldName="BankCashType" VisibleIndex="3" ShowInCustomizationForm="True">
                                <PropertiesComboBox ValueType="System.String" EnableIncrementalFiltering="True">
                                </PropertiesComboBox>
                                <CellStyle CssClass="gridcellright">
                                </CellStyle>
                                <EditFormSettings Visible="False" VisibleIndex="9"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataComboBoxColumn Visible="False" FieldName="BankAccountType" VisibleIndex="7" ShowInCustomizationForm="True">
                                <PropertiesComboBox ValueType="System.String" EnableIncrementalFiltering="True">
                                </PropertiesComboBox>
                                <CellStyle CssClass="gridcellright">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False" VisibleIndex="11"></EditFormSettings>
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataComboBoxColumn Visible="False" FieldName="ExchengSegment" VisibleIndex="10" ShowInCustomizationForm="True">
                                <PropertiesComboBox ValueType="System.String" EnableIncrementalFiltering="True">
                                </PropertiesComboBox>
                                <EditFormSettings Visible="False" VisibleIndex="4"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>


                            <%-- Account Code Section--%>
                            <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="AccountCode"
                                Caption="Short Name" ShowInCustomizationForm="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False" VisibleIndex="3"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>


                            <%-- <dxe:GridViewDataComboBoxColumn FieldName="branchname" Caption="Branch"
                                VisibleIndex="2" ShowInCustomizationForm="True">
                                <PropertiesComboBox ValueType="System.String" TextField="branch_description" ValueField="branch_id"
                                    EnableIncrementalFiltering="True" DataSourceID="branchdtl">
                                </PropertiesComboBox>
                                <Settings FilterMode="DisplayText"></Settings>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False" VisibleIndex="5"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>--%>

                            <dxe:GridViewDataTextColumn VisibleIndex="2" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="True">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <HeaderTemplate>
                                    <span>Branch</span>
                                </HeaderTemplate>
                                <DataItemTemplate>

                                    <a href="javascript:selectbranch('<%# Container.KeyValue %>');" title="Branch">
                                        <label title="Branch" style="cursor: pointer !important;">Branch</label>
                                </DataItemTemplate>
                                <CellStyle Wrap="False">
                                </CellStyle>

                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>








                            <dxe:GridViewDataComboBoxColumn FieldName="AccountGroup" Caption="Account Group"
                                VisibleIndex="2" ShowInCustomizationForm="True">
                                <PropertiesComboBox ValueType="System.String" TextField="AccountGroup" ValueField="ID"
                                    EnableIncrementalFiltering="True" DataSourceID="AllAccountGroup">
                                </PropertiesComboBox>
                                <Settings FilterMode="DisplayText"></Settings>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False" VisibleIndex="5"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="AccountName" ShowInCustomizationForm="True" Caption="Account Name">
                                <CellStyle Wrap="true" CssClass="gridcellleft">
                                </CellStyle>
                                <PropertiesTextEdit Width="200px" MaxLength="50">
                                </PropertiesTextEdit>
                                <EditFormSettings Visible="False" VisibleIndex="1"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="BankAccountNo"
                                Caption="Bank Account No" ShowInCustomizationForm="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False" VisibleIndex="13"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataComboBoxColumn FieldName="SubLedgerType" VisibleIndex="8" ShowInCustomizationForm="True">
                                <PropertiesComboBox ValueType="System.String" EnableIncrementalFiltering="True">
                                </PropertiesComboBox>
                                <Settings FilterMode="DisplayText"></Settings>
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <EditFormSettings Visible="False" VisibleIndex="2"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>

                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="6" FieldName="TDSApplicable" ShowInCustomizationForm="True">
                                <EditFormSettings Visible="False" VisibleIndex="6"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="9" FieldName="TDSRate"
                                Caption="TDS Rate" ShowInCustomizationForm="True">
                                <EditFormSettings Visible="False" VisibleIndex="10"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataCheckColumn Visible="False" VisibleIndex="12" FieldName="FBTApplicable" ShowInCustomizationForm="True">
                                <EditFormSettings Visible="False" VisibleIndex="8"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataCheckColumn>

                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="14" FieldName="FBTRate"
                                Caption="FBT Rate" ShowInCustomizationForm="True">
                                <EditFormSettings Visible="False" VisibleIndex="12"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <%-- ShowInCustomizationForm="True"--%>
                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="18" FieldName="RateOfIntrest"
                                Caption="Rate Of Interest (P/a)">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False" VisibleIndex="15"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="19" FieldName="Depreciation"
                                Caption="Depretiation" ShowInCustomizationForm="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False" VisibleIndex="15"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="20" FieldName="BankCompany"
                                Caption="BankCompany" ShowInCustomizationForm="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False" VisibleIndex="15"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <%--ShowInCustomizationForm="True"--%>
                            <%--   <dxe:GridViewCommandColumn ShowEditButton="True" Width="110px" ShowDeleteButton="true" VisibleIndex="17">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <HeaderTemplate>
                                    <span>Actions</span>
                                </HeaderTemplate>

                            </dxe:GridViewCommandColumn>--%>

                            <%-- <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="openingBalance" Caption="Action" HeaderStyle-HorizontalAlign="Center"  CellStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                              
                                 <DataItemTemplate>
                                        <a href="javascript:void(0);" title="Edit" onclick="Show('<%#Eval("MainAccount_ReferenceID") %>')">
                                       <img src="/assests/images/Edit.png" /></a>
                                  
                                    <a href="javascript:void(0);"  title="Delete" onclick="GridDelete('<%#Eval("AccountCode") %>','<%#Eval("SubLedgerType") %>')">
                                       <img src="/assests/images/Delete.png" /></a>
                                </DataItemTemplate>
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                             
                             <dxe:GridViewDataTextColumn VisibleIndex="8" Visible="false"  CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                               
                                <DataItemTemplate>
                                    <a href="javascript:void(0);"  title="Delete" onclick="GridDelete('<%#Eval("AccountCode") %>','<%#Eval("SubLedgerType") %>')">
                                       <img src="/assests/images/Delete.png" /></a>
                                </DataItemTemplate>
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>--%>


                            <dxe:GridViewDataTextColumn VisibleIndex="11" Visible="false" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="True">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <HeaderTemplate>
                                    <span>Opening DR/CR</span>
                                </HeaderTemplate>
                                <DataItemTemplate>
                                    <%-- onclick="Show('<%#Eval("MainAccount_ReferenceID") %>')"--%>
                                    <% if (rights.CanAdd && rights.CanEdit)
                                       { %>
                                    <a href="javascript:void(0);" title="Edit Opening DR/CR">
                                        <label title="Add/Edit" style="cursor: pointer !important;">Add/Edit</label>
                                        <% } %>
                                </DataItemTemplate>
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>


                            <%--<dxe:GridViewDataTextColumn VisibleIndex="16" HeaderStyle-HorizontalAlign="Center" Width="50px" CellStyle-HorizontalAlign="left" ShowInCustomizationForm="True">
                                <HeaderTemplate>
                                    Delete
                                </HeaderTemplate>
                                  <DataItemTemplate>
                                    <% if (rights.CanDelete)
                                       { %>
                                    <a href="javascript:void(0);" title="Delete" onclick="GridDelete('<%#Eval("AccountCode") %>','<%#Eval("SubLedgerType") %>')">
                                        <img src="/assests/images/Delete.png" /></a><%} %>
                                </DataItemTemplate>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <CellStyle Wrap="False">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>--%>


                            <dxe:GridViewDataTextColumn VisibleIndex="13" FieldName="openingBalance" Caption="Asset Detail" ShowInCustomizationForm="True">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <EditCellStyle HorizontalAlign="Center">
                                </EditCellStyle>
                                <Settings ShowFilterRowMenu="False" AllowAutoFilter="False" />
                                <DataItemTemplate>
                                    <%-- <a href="javascript:void(0);" id="aaa" style="color:#000099;" runat="server">Add/Edit </a>--%>
                                    <dxe:ASPxHyperLink ID="AviewLink" runat="server" Text="Asset Detail">
                                    </dxe:ASPxHyperLink>
                                </DataItemTemplate>
                                <CellStyle Wrap="False" CssClass="gridcellright" HorizontalAlign="Center">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="15" FieldName="openingBalance" Caption="Document" ShowInCustomizationForm="True">
                                <Settings ShowFilterRowMenu="False" AllowAutoFilter="False" />
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <EditCellStyle HorizontalAlign="Center">
                                </EditCellStyle>
                                <DataItemTemplate>

                                    <dxe:ASPxHyperLink ID="hlink2" runat="server" Text="Document" CommandName="DocSelect">
                                    </dxe:ASPxHyperLink>
                                    <%--   <a href="javascript:void(0);" title="Document" onclick="showhistory('<%#Eval("MainAccount_ReferenceID")+ "^" + Eval("AccountCode") %>')">
                                     <label title="Document"  style="cursor:pointer !important; color:#000099; text-align:left">Document</label>   
                                       </a>--%>
                                </DataItemTemplate>
                                <CellStyle Wrap="False" CssClass="gridcellright" HorizontalAlign="Center">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewCommandColumn ShowNewButton="false" ShowEditButton="false" Caption="Statutory Details" ShowInCustomizationForm="True" Width="130">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <CustomButtons>
                                    <dxe:GridViewCommandColumnCustomButton ID="MapLedger" Text="Statutory Details">
                                    </dxe:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                                <CellStyle Wrap="False" CssClass="gridcellright" HorizontalAlign="Center">
                                </CellStyle>

                            </dxe:GridViewCommandColumn>




                            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="0">
                                <DataItemTemplate>
                                    <div class='floatedBtnArea'>
                                    <% if (rights.CanEdit)
                                       { %>
                                    <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')" class="" title="">
                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a><%} %>
                                        <%--Rev work start 21.06.2022 Mantise no:0024974: Copy in Account Head Master module with Rights based on 'ADD' rights.--%>
                                            <% if (rights.CanAdd)
                                       { %>
                                    <a href="javascript:void(0);" onclick="OnCopyClick('<%# Container.KeyValue %>')" class="" title=""> 
                                        <span class='ico copyColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Copy</span>                                        
                                    </a><%} %>
                                        <%--Rev work close 21.06.2022 Mantise no:0024974: Copy in Account Head Master module with Rights based on 'ADD' rights.--%>
                                    <% if (rights.CanDelete)
                                       { %>
                                    <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="" title="">                                        
                                        <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>
                                            <%--Rev work start 21.06.2022 Mantise no:0024974: Copy in Account Head Master module with Rights based on 'ADD' rights.--%>
                                            <%--Edit--%>
                                            Delete
                                            <%--Rev work close 21.06.2022 Mantise no:0024974: Copy in Account Head Master module with Rights based on 'ADD' rights.--%>
                                        </span></a><%} %>
                                        <%--Mantis Issue 24953--%>
                                    <% if (rights.CanEdit)
                                       { %>
                                    <a href="javascript:void(0);" onclick="OnPostingTypeClick('<%# Container.KeyValue %>')" class="" title="">
                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Posting type in Party Ledger</span></a><%} %>
                                        <%--End of Mantis Issue 24953--%>
                                    </div>
                                </DataItemTemplate>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                <HeaderTemplate><span></span></HeaderTemplate>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>






                        </Columns>
                        <ClientSideEvents RowClick="gridRowclick" />

                        <Styles>
                            <Header CssClass="gridheader" SortingImageSpacing="5px" ImageSpacing="5px">
                            </Header>
                            <FocusedRow CssClass="gridselectrow">
                            </FocusedRow>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <FocusedGroupRow CssClass="gridselectrow">
                            </FocusedGroupRow>
                        </Styles>

                    </dxe:ASPxGridView>
                    &nbsp;
                     <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ContentUrl="AssetDetail.aspx"
                         CloseAction="CloseButton" Top="120" Left="300" ClientInstanceName="popup" Height="680px"
                         Width="1050px" HeaderText="Asset Details" AllowResize="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ContentStyle-Wrap="True" ResizingMode="Live" Modal="true">
                         <ContentCollection>
                             <dxe:PopupControlContentControl runat="server">
                             </dxe:PopupControlContentControl>
                         </ContentCollection>
                         <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
                     </dxe:ASPxPopupControl>

                    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server" ContentUrl="Account_Document.aspx"
                        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupdoc" Height="560px"
                        Width="1000px" HeaderText="Add/Modify Document" AllowResize="false" ContentStyle-Wrap="True" ResizingMode="Live" Modal="true">
                        <ContentCollection>
                            <dxe:PopupControlContentControl runat="server">
                            </dxe:PopupControlContentControl>
                        </ContentCollection>
                        <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
                    </dxe:ASPxPopupControl>
                    <%-- <dxe:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ContentUrl="frm_OpeningBalance.aspx"
                        CloseAction="CloseButton" Top="100" Left="250" ClientInstanceName="popup" Height="350px"
                        Width="430px" HeaderText="Add Opening Balance">
                    </dxe:ASPxPopupControl>--%>
                    <asp:HiddenField ID="hdSegment" runat="server" />
                </td>
            <%--Rev work star 21.06.2022 mantise issue:0024974:  BUG (what 'mo' is showing? in the page--%>
            <%--mo--%>
            <%--Rev work close 21.06.2022 mantise issue:0024974:  BUG (what 'mo' is showing? in the page--%>
            </tr>
            <tr>
                <td style="display: none">
                    <dxe:ASPxComboBox ID="ASPxComboBox4" ClientInstanceName="combo" runat="server" OnCallback="ASPxComboBox4_Callback"
                        OnCustomJSProperties="ASPxComboBox4_CustomJSProperties">
                        <ClientSideEvents EndCallback="function(s,e) { ShowError1(s.cpInsertError1); }" />
                    </dxe:ASPxComboBox>
                </td>
                <asp:HiddenField ID="hdnAssetType" runat="server" />
                <asp:HiddenField ID="hdneditassettype" runat="server" />
                <%--Mantis Issue 24953--%>
                <asp:HiddenField ID="hdnAccountTypeID" runat="server" />
                <%--End of Mantis Issue 24953--%>
            </tr>
        </table>


        <asp:SqlDataSource ID="MainAccount" runat="server" ConflictDetection="CompareAllValues"
            DeleteCommand="" SelectCommand=""></asp:SqlDataSource>

        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>


        <%--        <asp:SqlDataSource ID="AllAccountGroup" runat="server"
            SelectCommand="Select AccountGroup_Name as Name, cast([AccountGroup_ReferenceID]  as varchar(100)) as ID ,AccountGroup_Name as AccountGroup  from Master_AccountGroup"></asp:SqlDataSource>--%>
        <%--kaushik 16-2-2017 initially account group will be populated with respect to selected account type start --%>

        <asp:SqlDataSource ID="AllAccountGroup" runat="server" SelectCommand="Select AccountGroup_Name as Name, cast([AccountGroup_ReferenceID]  as varchar(100)) as ID ,AccountGroup_Name as AccountGroup  from Master_AccountGroup "></asp:SqlDataSource>
        <%--kaushik 16-2-2017 initially account group will be populated with respect to selected account type end --%>
        <asp:SqlDataSource ID="branchdtl" runat="server" SelectCommand="select '0' as branch_id ,  'Select' as branch_description union all   select branch_id,branch_description from tbl_master_branch order by branch_description"></asp:SqlDataSource>
        <%--     <asp:SqlDataSource ID="SqlCompany" runat="server"
            SelectCommand="select cmp_internalId,cmp_name from tbl_master_company where cmp_internalId in(select distinct exch_compId from tbl_master_companyExchange)">
        </asp:SqlDataSource>--%>

        <asp:SqlDataSource ID="SqlCompany" runat="server" SelectCommand=""></asp:SqlDataSource>


        <asp:SqlDataSource ID="SqlSegment" runat="server" ConflictDetection="CompareAllValues"
            SelectCommand=""></asp:SqlDataSource>

        <asp:SqlDataSource ID="tdstcs" runat="server" SelectCommand="prc_Subledger" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="action" DefaultValue="PopulateDropDownFortdstcs" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="BranchdataSource" runat="server" SelectCommand="select branch_id,branch_code,branch_description from tbl_master_branch"></asp:SqlDataSource>
    </div>
    </div>
    <dxe:ASPxPopupControl ID="BranchSelectPopup" runat="server" Width="700"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cBranchSelectPopup"
        HeaderText="Select Branch" AllowResize="false" ResizingMode="Postponed" Modal="true">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                <dxe:ASPxGridView ID="branchGrid" runat="server" KeyFieldName="branch_id" AutoGenerateColumns="False" DataSourceID="BranchdataSource"
                    Width="100%" ClientInstanceName="cbranchGrid" OnCustomCallback="branchGrid_CustomCallback"
                    SelectionMode="Multiple" SettingsBehavior-AllowFocusedRow="true" OnCommandButtonInitialize="branchGrid_CommandButtonInitialize">
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



            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>
    <%--UDF Popup --%>
    <dxe:ASPxPopupControl ID="ASPXPopupControl1" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
        Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed">

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>
    <asp:HiddenField runat="server" ID="IsUdfpresent" />
    <asp:HiddenField runat="server" ID="Keyval_internalId" />
    <%--UDF Popup End--%>


    <%--HSN/SAC Mapping To Ledger Popup --%>

    <dxe:ASPxPopupControl ID="MappingLedgerPopup" runat="server" ClientInstanceName="cMappingLedgerPopup"
        Width="400px" HeaderText="Statutory Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div style="clear: both"></div>
                <asp:HiddenField runat="server" ID="hfLedgerID" />
                <asp:HiddenField runat="server" ID="hfHSNSCAkey" />
                <asp:HiddenField runat="server" ID="hfHSNSCAType" />
                <div class="cityDiv" style="height: auto;">
                    <asp:Label ID="Label1" runat="server" Text="HSN Code" CssClass="newLbl"></asp:Label>
                </div>
                <div class="Left_Content">
                    <dxe:ASPxGridLookup ID="HsnLookUp" runat="server" ClientInstanceName="cHsnLookUp"
                        KeyFieldName="HSN_id" Width="100%" TextFormatString="{0}" MultiTextSeparator=", "
                        OnDataBinding="HsnLookUp_DataBinding">
                        <Columns>
                            <dxe:GridViewDataColumn FieldName="Code" Caption="Code" Width="50" />
                            <dxe:GridViewDataColumn FieldName="Description" Caption="Description" Width="350" />
                        </Columns>
                        <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                            <Templates>
                                <StatusBar>
                                    <table class="OptionsTable" style="float: right">
                                        <tr>
                                            <td>
                                                <%--<dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseMappingPopup" />--%>
                                            </td>
                                        </tr>
                                    </table>
                                </StatusBar>
                            </Templates>

                            <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>

                            <Settings ShowStatusBar="Visible" UseFixedTableLayout="true" />
                        </GridViewProperties>
                        <ClientSideEvents TextChanged="function(s, e) { HsnLookUp_SelectedChange(e)}" />
                        <ClearButton DisplayMode="Auto">
                        </ClearButton>
                    </dxe:ASPxGridLookup>
                </div>
                <div class="clear"></div>
                <div class="cityDiv" style="height: auto;">
                    <asp:Label ID="Label4" runat="server" Text="Service Category" CssClass="newLbl"></asp:Label>
                </div>
                <div class="Left_Content">
                    <dxe:ASPxGridLookup ID="ScaLookUp" runat="server" ClientInstanceName="cScaLookUp"
                        KeyFieldName="TAX_ID" Width="100%" TextFormatString="{0}" MultiTextSeparator=", "
                        OnDataBinding="ScaLookUp_DataBinding">
                        <Columns>
                            <dxe:GridViewDataColumn FieldName="SERVICE_CATEGORY_CODE" Caption="Code" Width="50" />
                            <dxe:GridViewDataColumn FieldName="SERVICE_TAX_NAME" Caption="Name" Width="250" />
                            <dxe:GridViewDataColumn FieldName="ACCOUNT_HEAD_TAX_RECEIPTS" Caption="Receipts" Width="0" />
                            <dxe:GridViewDataColumn FieldName="ACCOUNT_HEAD_OTHERS_RECEIPTS" Caption="Oth Receipts" Width="0" />
                            <dxe:GridViewDataColumn FieldName="ACCOUNT_HEAD_PENALTIES" Caption="Penalties" Width="0" />
                            <dxe:GridViewDataColumn FieldName="ACCOUNT_HEAD_DeductRefund" Caption="A/C Head (Deduct Refund)" Width="0" />
                        </Columns>
                        <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                            <Templates>
                                <StatusBar>
                                    <table class="OptionsTable" style="float: right">
                                        <tr>
                                            <td>
                                                <%--<dxe:ASPxButton ID="ASPxButton7" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseMappingPopup" />--%>
                                            </td>
                                        </tr>
                                    </table>
                                </StatusBar>
                            </Templates>

                            <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>

                            <Settings ShowStatusBar="Visible" UseFixedTableLayout="true" />
                        </GridViewProperties>
                        <ClientSideEvents TextChanged="function(s, e) { ScaLookUp_SelectedChange(e)}" />
                        <ClearButton DisplayMode="Auto">
                        </ClearButton>
                    </dxe:ASPxGridLookup>
                </div>
                <div class="clear"></div>
                <div class="cityDiv" style="height: auto;margin-top:8px">
                    <label>GSTIN   </label>
                    <div class="Left_Content">
                        <ul class="nestedinput">
                            <li>
                                <dxe:ASPxTextBox ID="txtGSTIN1" ClientInstanceName="ctxtGSTIN111" MaxLength="2" TabIndex="10" runat="server" Width="50px">
                                    <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                </dxe:ASPxTextBox>
                            </li>
                            <li class="dash">- </li>
                            <li>
                                <dxe:ASPxTextBox ID="txtGSTIN2" ClientInstanceName="ctxtGSTIN222" MaxLength="10" TabIndex="11" runat="server" Width="150px">
                                    <ClientSideEvents KeyUp="Gstin2TextChanged" />
                                </dxe:ASPxTextBox>
                            </li>
                            <li class="dash">- </li>
                            <li class="relative">
                                <dxe:ASPxTextBox ID="txtGSTIN3" ClientInstanceName="ctxtGSTIN333" MaxLength="3" TabIndex="12" runat="server" Width="50px">
                                </dxe:ASPxTextBox>
                                <span id="invalidGst" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; display: none; padding-left: 9px; right: -22px" title="Invalid GSTIN"></span>
                            </li>
                        </ul>
                    </div>
                </div>
                <div class="clear"></div>
                <%-- REV RAJDIP FOR PAN & Deductee Type --%>
                <div class="row">
                     <div class="col-sm-6" style="height: auto;">    
                         <div id="Number" style="width:100%">           
                        <label style="margin-top: 7px;">PAN</label>
                        <div >
                            <dxe:ASPxTextBox ID="txtNumber" ClientInstanceName="ctxtNumber" ClientEnabled="true" MaxLength="10"
                                runat="server" Height="18px" Width="100%" >   
                                <ClientSideEvents LostFocus="DeducteStatusBasedOnPan" />                             
                            </dxe:ASPxTextBox>
                                <dxe:ASPxLabel ID="labelformat" Style="text-transform:capitalize;font-size:11px;" Width="100px" runat="server" ForeColor="blueviolet" CssClass="formatcss" ClientInstanceName="lbformat" Text="Sample:AAAAA9999A"></dxe:ASPxLabel>
                                 
                        </div>
                         </div>
                    </div>
                    <div class="col-sm-6" style="height: auto;"> 
                        <label style="margin-top: 7px;">Deductee Type</label>
                        <select class="form-control" id="cmbDeducteeType" style="width:100%;">
                            <option value="0" selected="selected">Select</option>
                        </select>
                    </div>
                </div>
                        <%-- END REV RAJDIP --%>                 
                 <div class="clear"></div>
                <div class="clear"></div>
                 <div class="Left_Content" id="dvNameAsPerPan" runat="server">
                            <asp:Label ID="Label3" runat="server" Text="Name as per PAN Card"></asp:Label>
                            <div>
                                <dxe:ASPxTextBox ID="txtNameAsPerPan"  ClientInstanceName="ctxtNameAsPerPan" MaxLength="250" HorizontalAlign="Left"
                                    runat="server" Width="100%">

                                 </dxe:ASPxTextBox>
                            </div>
                 </div>
                 <div class="clear"></div>

                <div style="margin-top: 7px;">
                    <span><dxe:ASPxCheckBox ID="chbFurtherenceOfBusiness" ClientInstanceName="cchbFurtherenceOfBusiness" runat="server"></dxe:ASPxCheckBox></span>
                    <span><asp:Label ID="Label2" runat="server" Text="Furtherence of Business" CssClass="newLbl"></asp:Label></span>
                </div>

                <div style="clear: both"></div>
                <div class="" style="margin-top: 5px;margin-bottom:5px;">
                    <div class="cityDiv" style="height: auto;">
                    </div>
                    <div class="Left_Content">
                        <button type="button" class="btn btn-success" onclick="MappingLedgerSaveClick()">Save</button>
                        <button type="button" class="btn btn-danger" onclick="CloseMappingPopup()">Cancel</button>

                    </div>
                </div>

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>


    <dxe:ASPxPopupControl ID="VerifyPopup" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cVerifyPopup" Height="750px"
        Width="1020px" HeaderText="Add New Customer" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <HeaderTemplate>
            <span>Verify</span>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

    <%--Mantis Issue 24953--%>
    <dxe:ASPxPopupControl ID="PostingTypePopup" runat="server" ClientInstanceName="cPostingTypePopup"
        Width="400px" HeaderText="Posting type in Party Ledger" PopupHorizontalAlign="WindowCenter"
        BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div style="clear: both"></div>
                  <div class="cityDiv" style="height: auto;">
                    <asp:Label ID="Label5" runat="server" Text="" CssClass="newLbl"></asp:Label>
                </div>
                <div class="Left_Content">
                    <dxe:ASPxComboBox ID="cmbPostingType" runat="server" ClientInstanceName="ccmbPostingType" Width="100%">
                    </dxe:ASPxComboBox>
                </div>

                <div style="clear: both"></div>
                <div class="" style="margin-top: 15px;margin-bottom:5px;">
                    <div class="cityDiv" style="height: auto;">
                    </div>
                    <div class="Left_Content">
                        <button type="button" class="btn btn-success" onclick="PostingTypeSaveClick()">Save</button>
                        <button type="button" class="btn btn-danger" onclick="ClosePostingTypePopup()">Cancel</button>

                    </div>
                </div>

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>

    <%--End of Mantis Issue 24953--%>

    <asp:HiddenField ID="hdnflag" runat="server" />
    <%--HSN/SAC Mapping To Ledger Popup End--%>
</asp:Content>
