<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                21-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_frm_FinancialYearAdd" CodeBehind="frm_FinancialYearAdd.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>--%>




    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var MaxLength = 100;
            $('#txtRemarks').keypress(function (e) {
                if ($(this).val().length >= MaxLength) {
                    e.preventDefault();
                }
            });
        });

        function numbersonly(myfield, e) {
            var key;
            var keychar;

            if (window.event)
                key = window.event.keyCode;
            else if (e)
                key = e.which;
            else
                return true;

            keychar = String.fromCharCode(key);

            // control keys
            if ((key == null) || (key == 0) || (key == 8) || (key == 9) || (key == 13) || (key == 27))
                return true;

                // numbers
            else if ((("0123456789").indexOf(keychar) > -1))
                return true;

                // only one decimal point
            else if ((keychar == ".")) {
                if (myfield.value.indexOf(keychar) > -1)
                    return false;
            }
            else
                return false;
        }

        function ValidatePage() {
            var returnValue = true;

            if (document.getElementById("txtFinYear").value.trim() == '') {
                $('#redtxtFinYear').css({ 'display': 'block' });               
                returnValue = false;
            } else {
                $('#redtxtFinYear').css({ 'display': 'none' });
            }
            if (document.getElementById("txtFinYear1").value.trim() == '') {
                $('#redtxtFinYear1').css({ 'display': 'block' });
                returnValue = false;
            } else {
                $('#redtxtFinYear1').css({ 'display': 'none' });
            }

            if (document.getElementById("txtFinYear").value.trim().length < 4) {
                jAlert('Enter a Valid Financial year');
                return false;
            }

            if (document.getElementById("txtFinYear1").value.trim().length < 4) {
                jAlert('Enter a Valid Financial year');
                return false;
            } 

           
            if (document.getElementById("txtFinYear").value.trim() != '' && document.getElementById("txtFinYear1").value.trim() != '') {
                
                var Year1 = document.getElementById("txtFinYear").value;
                var Year2 = document.getElementById("txtFinYear1").value;
                var Diff = parseInt(Year2) - parseInt(Year1)               
                if (Diff != 1) {
                    jAlert('Enter a Valid Financial year');
                    return false;
                }
            }


            if (txtStart.GetText() == '01-01-0100' || txtStart.GetText() == '01-01-1900') {
                $('#redtxtStart').css({ 'display': 'block' });
                // $("#redtxtStart").removeClass("hide");
                returnValue = false;
            } else {
                $('#redtxtStart').css({ 'display': 'none' });
            }

            if (txtEnd.GetText() == '01-01-0100' || txtEnd.GetText() == '01-01-1900') {

                $('#redtxtEnd').css({ 'display': 'block' });
                // $("#redtxtFinYear").removeClass("hide");
                returnValue = false;
            } else {
                $('#redtxtEnd').css({ 'display': 'none' });
            }

            return returnValue;
        }
        function Close() {
            // parent.editwin.close();
            window.location.href = "frm_FinancialYear.aspx";
        }
    </script>
    <style>
        .nestedinput {
            padding: 0;
            margin: 0;
        }
        .nestedinput li {
            list-style-type: none;
            display: inline-block;
            float: left;
        }
        .nestedinput li.dash {
            width: 26px;
            text-align: center;
            padding: 6px;
        }
        table.pad>tbody>tr>td {
            padding:5px 0px !important;
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
        .TableMain100 #GrdHolidays , #gridStatus
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
            <h3><%Response.Write((Request.QueryString["id"] != null ? Request.QueryString["id"].ToString() : "").ToString()); %> Finacial Year</h3>
            <div class="crossBtn"><a href="frm_FinancialYear.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
        <div class="form_main">
        <table width="500px" class="pad">
            <%--            <tr>
                <td colspan="2">
                    <h3><%Response.Write((Request.QueryString["id"] != null ? Request.QueryString["id"].ToString() : "").ToString()); %> Finacial Year</h3>
                </td>
            </tr>--%>

            <tr>
                <td class="gridcellleft">
                    <span class="Ecoheadtxt">Financial Year:<span style="color: red">*</span></span>
                </td>
                <td class="gridcellleft">
               <ul class="nestedinput">
                   <li><asp:TextBox ID="txtFinYear" runat="server" Width="90px" MaxLength="4" TabIndex="1" placeholder="YYYY" onkeypress="return numbersonly(this, event)"></asp:TextBox></li>
                   <li class="dash"> - </li>
                   <li><asp:TextBox ID="txtFinYear1" runat="server" Width="90px" MaxLength="4" TabIndex="2" placeholder="YYYY" onkeypress="return numbersonly(this, event)"></asp:TextBox></li>
               </ul>                
                   
                    <%--<div style="text-align: right; width: 202PX;">(Ex.2016-2017)</div>--%>
                    <%--<div id="redtxtFinYear" class="red hide">Mandatory</div>--%>
                    
                    <span id="redtxtFinYear" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute;top: 62px;left: 400px;display:none" title="Mandatory/Invalid"></span>
                <span id="redtxtFinYear1" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute;top: 62px;left: 400px;display:none" title="Mandatory/Invalid"></span>
                </td>
            </tr>
            <tr>
                <td class="gridcellleft">
                    <span class="Ecoheadtxt">Start Date:<span style="color: red">*</span></span>
                </td>
                <td class="gridcellleft">
                    <dxe:ASPxDateEdit ID="txtStart" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                        TabIndex="3" Width="208px">
                        <buttonstyle width="13px">
                        </buttonstyle>
                    </dxe:ASPxDateEdit>
                    <%--<div id="redtxtStart" class="red hide">Mandatory</div>--%> 
                    <span id="redtxtStart" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute; top:118px; left: 400px; display:none" title="Mandatory"></span>
                </td>
            </tr>
            <tr>
                <td class="gridcellleft">
                    <span class="Ecoheadtxt">End Date:<span style="color: red">*</span></span>
                </td>
                <td class="gridcellleft">
                    <dxe:ASPxDateEdit ID="txtEnd" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                        TabIndex="4" Width="208px">
                        <buttonstyle width="13px">
                        </buttonstyle>
                    </dxe:ASPxDateEdit>
                    <%--<div id="redtxtEnd" class="red hide">Mandatory</div>--%>
                    <span id="redtxtEnd" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute; top: 155px;left: 400px;display:none" title="Mandatory"></span>
                </td>
            </tr>
            <tr>
                <td class="gridcellleft">
                    <span class="Ecoheadtxt">Remarks:</span>
                </td>
                <td class="gridcellleft">
                    <asp:TextBox TextMode="MultiLine" ID="txtRemarks" MaxLength="100" runat="server" Width="208px" TabIndex="5"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td></td>
                <td colspan="2" class="gridcellleft">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary btnUpdate" OnClick="btnSave_Click"
                        TabIndex="6" ValidationGroup="a" />
                  
                    <input type="button" id="btnCancel" value="Cancel" class="btn btn-danger btnUpdate" onclick="Close()" />
                   

                </td>
            </tr>
        </table>
    </div>
    </div>
    <asp:HiddenField ID="hdFinYearOld" runat="server" />
</asp:Content>

