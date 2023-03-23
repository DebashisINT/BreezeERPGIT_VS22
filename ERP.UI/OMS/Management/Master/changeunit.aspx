<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                23-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="UOM" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_changeunit" CodeBehind="changeunit.aspx.cs" EnableEventValidation="false" %>


<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            ListBind();
            ChangeSource();

        });
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
        function lstconverttounit() {

            $('#lstconverttounit').fadeIn();

        }

        function FunCallAjaxList(objID, objEvent, ObjType) {
            //alert(ObjType);
            var strQuery_Table = '';
            var strQuery_FieldName = '';
            var strQuery_WhereClause = '';
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = '';

            if (ObjType == 'ProductFo') {

                strQuery_Table = "Master_uom";
                strQuery_FieldName = "distinct top 10 (isnull(Uom_Name,''))+'[' + Uom_Shortname+']',Uom_id";

                strQuery_WhereClause = "  ( Uom_Name like (\'%RequestLetter%') or Uom_Shortname like (\'%RequestLetter%') )";

            }
            CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);

            ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars(CombinedQuery));

        }
        function ChangeSource() {
            var fname = "%";
            var lconverttounit = $('select[id$=lstconverttounit]');
            lconverttounit.empty();


            $.ajax({
                type: "POST",
                url: "changeunit.aspx/GetUOM",
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

                            $('#lstconverttounit').append($('<option>').text(name).val(id));

                        }

                        $(lconverttounit).append(listItems.join(''));

                        lstconverttounit();
                        $('#lstconverttounit').trigger("chosen:updated");

                        Changeselectedvalue();


                    }
                    else {
                        //   alert("No records found");
                        //lstReferedBy();
                        $('#lstconverttounit').trigger("chosen:updated");

                    }
                }
            });
            // }
        }
        function lstconverttounit() {

            $('#lstconverttounit').fadeIn();

        }
        function setvalue() {

            if ($('#hdAddEdit').val() == "Edit") {
                document.getElementById("txtconverttounit_hidden").value = document.getElementById("lstconverttounit").value;
                if (document.getElementById("txtconverttounit_hidden").value != '') {

                    return false;
                }
            }
            else {
                //var ReturnValue = true;
                if (ctxtUnit.GetText().trim() == "") {
                    // $('#MandatoryUnit').show();
                    return false;
                } else {
                    $('#RequiredFieldValidator1').hide();
                }

                if (ctxtShortUnit.GetText().trim() == "") {
                    // $('#MandatoryShortUnit').show();
                    return false;
                } else {
                    //$('#MandatoryShortUnit').hide();
                }
                if (ctxtUseFor.GetText().trim() == "") {
                    //  $('#MandatoryUseFor').show();
                    return false;
                } else {
                    // $('#MandatoryUseFor').hide();
                }
            }
            //return ReturnValue;
        }

        function Changeselectedvalue() {
            var lstconverttounit = document.getElementById("lstconverttounit");
            if (document.getElementById("txtconverttounit_hidden").value != '') {

                for (var i = 0; i < lstconverttounit.options.length; i++) {
                    if (lstconverttounit.options[i].value == document.getElementById("txtconverttounit_hidden").value) {
                        lstconverttounit.options[i].selected = true;
                    }
                }
                $('#lstconverttounit').trigger("chosen:updated");
            }

        }

        function replaceChars(entry) {
            out = "+"; // replace this
            add = "--"; // with this
            temp = "" + entry; // temporary holder

            while (temp.indexOf(out) > -1) {
                pos = temp.indexOf(out);
                temp = "" + (temp.substring(0, pos) + add +
                temp.substring((pos + out.length), temp.length));
            }
            return temp;
        }
        function CallAjax(obj1, obj2, obj3) {

            // FieldName='ctl00_ContentPlaceHolder1_Headermain1_cmbCompany';
            ajax_showOptions(obj1, obj2, obj3);
            //alert (ajax_showOptions);
        }


        FieldName = 'abcd';
        //function SignOff() {
        //    window.parent.SignOff();
        //}


        //function height() {
        //    if (document.body.scrollHeight >= 300)
        //        window.frameElement.height = document.body.scrollHeight;
        //    else
        //        window.frameElement.height = '300px';
        //    window.frameElement.Width = document.body.scrollWidth;
        //}

        function FillValues(obj) {
            parent.editwin.close(obj);



        }

        function Changestatus() {
            var URL = "frm_UOM.aspx";
            window.location.href = URL;
        }

    </script>
    <style>
        #rfvComname {
            position: absolute;
            right: 80px;
            top: 8px;
        }

        .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }

        #lstconverttounit {
            width: 200px;
        }

        #lstconverttounit {
            display: none !important;
        }

        #lstconverttounit_chosen {
            width: 100% !important;
        }

        .ctcclass {
            position: absolute;
            top: 10px;
            right: -16px;
        }

        .Unitclass {
            position: absolute;
            top: 65px;
            right: 854px;
        }

        .UnitShortclass {
            position: absolute;
            top: 95px;
            right: 854px;
        }

        .Unituseclass {
            position: absolute;
            top: 127px;
            right: 854px;
        }

        /*Rev 1.0*/

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
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>
                <span class="">
                    <asp:Label ID="lblHeading" runat="server" Text="Add Units Of Measurement [UOM]"></asp:Label>
                </span>
            </h3>
            <div class="crossBtn"><a href="frm_UOM.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
        <div class="form_main">
        <%--  <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3600">
        </asp:ScriptManager>--%>
        <%--        <table class="TableMain100">
            <tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">Change Unit</span></strong>
                </td>
                <td style="text-align: right">
                    <div class="crossBtn"><a href="frm_UOM.aspx"><i class="fa fa-times"></i></a></div>
                </td>
            </tr>
        </table>--%>
        <%--<table width="400px" align="center" style="border: solid 1px white;">
                
            </table>--%>
        <table class="">
            <tr id="trunit" runat="server">
                <td style="width: 112px; text-align: left; padding-bottom: 10px">
                    <label style="">Unit Name<span style="color: red">*</span></label>
                </td>
                <td style="text-align: left; padding-bottom: 10px">

                    <dxe:ASPxTextBox runat="server" ID="txtUnit" ClientInstanceName="ctxtUnit">
                    </dxe:ASPxTextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="a" runat="server" ControlToValidate="txtUnit" Display="Dynamic"
                        CssClass="fa fa-exclamation-circle Unitclass " ToolTip="Mandatory."
                        ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="trshort" runat="server">
                <td style="width: 112px; text-align: left; padding-bottom: 10px">
                    <label style="">Short Name<span style="color: red">*</span></label>
                </td>
                <td style="text-align: left; padding-bottom: 10px">

                    <dxe:ASPxTextBox runat="server" ID="txtShortUnit" ClientInstanceName="ctxtShortUnit">
                    </dxe:ASPxTextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="a" runat="server" ControlToValidate="txtShortUnit" Display="Dynamic"
                        CssClass="fa fa-exclamation-circle UnitShortclass " ToolTip="Mandatory."
                        ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                </td>

                
            </tr>
            <tr id="truseFor" runat="server">
                <td style="width: 112px; text-align: left; padding-bottom: 10px">
                    <label style="">Use For<span style="color: red">*</span></label>
                </td>
                <td style="text-align: left; padding-bottom: 10px">

                    <dxe:ASPxTextBox runat="server" ID="txtUseFor" ClientInstanceName="ctxtUseFor">
                    </dxe:ASPxTextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="a" runat="server" ControlToValidate="txtUseFor" Display="Dynamic"
                        CssClass="fa fa-exclamation-circle Unituseclass " ToolTip="Mandatory."
                        ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                </td>

            </tr>
            <tr id="trExistingUnit" runat="server">
                <td style="width: 112px; text-align: left;">
                    <span id="Span2" class="Ecoheadtxt" style="text-align: right;">Existing Unit</span>
                </td>
                <td class="Ecoheadtxt" style="text-align: left; height: 37px; width: 300px">
                    <strong><span id="litSegment" runat="server" style="color: black"></span></strong>
                </td>
            </tr>
            <tr id="trChangeTo" runat="server">
                <td style="width: 80px; text-align: left;">
                    <span id="Span1" class="Ecoheadtxt" style="text-align: right;">Change to</span> <span style="color: red;">*</span>
                </td>
                <td class="Ecoheadtxt relative" style="text-align: left; height: 37px; width: 256px; position: relative">
                    <%-- <asp:TextBox runat="server" Width="200px" ID="txtproduct"></asp:TextBox>--%>
                    <asp:TextBox
                        ID="txtproduct_hidden" runat="server" Width="14px" Style="display: none">
                    </asp:TextBox>
                    <asp:HiddenField ID="txtconverttounit_hidden" runat="server" />
                    <asp:ListBox ID="lstconverttounit" CssClass="chsn" runat="server" Width="100%" TabIndex="8" data-placeholder="Select..."></asp:ListBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ValidationGroup="a" runat="server" ControlToValidate="lstconverttounit" Display="Dynamic"
                        CssClass="fa fa-exclamation-circle ctcclass " ToolTip="Mandatory."
                        ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <table>
                        <tr>
                            <td align="left" id="td_yes" runat="server" style="width: 40px;">
                                <asp:Button ID="btnYes" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnYes_Click" OnClientClick=" setvalue()" ValidationGroup="a" />
                            </td>
                            <td align="left" id="td_no" runat="server" style="width: 40px;">
                                <asp:Button ID="btnNo" runat="server" CssClass="btn btn-danger" Text="Cancel"
                                    OnClick="btnNo_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </div>
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:HiddenField ID="hdAddEdit" runat="server" />
</asp:Content>
