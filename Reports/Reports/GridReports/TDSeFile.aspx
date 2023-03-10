<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                09-03-2023        2.0.36           Pallab              25575 : Report pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="TDSeFile.aspx.cs" Inherits="Reports.Reports.GridReports.TDSeFile" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <style>
        #pageControl, .dxtc-content {
            overflow: visible !important;
        }

        #MandatoryAssign {
            position: absolute;
            right: -17px;
            top: 6px;
        }

        #MandatorySupervisorAssign {
            position: absolute;
            right: 1px;
            top: 27px;
        }

        .chosen-container.chosen-container-multi,
        .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }

        #ListBoxBranches {
            width: 200px;
        }

        .hide {
            display: none;
        }

        .dxtc-activeTab .dxtc-link {
            color: #fff !important;
        }
    </style>


    <style>
        .plhead a {
            font-size:16px;
            padding-left:10px;
            position:relative;
            width:100%;
            display:block;
            padding:9px 10px 5px 10px;
        }
        .plhead a>i {
            position:absolute;
            top:11px;
            right:15px;
        }
        #accordion {
            margin-bottom:10px;
        }
        .companyName {
            font-size:16px;
            font-weight:bold;
            margin-bottom:15px;
        }
        
        .plhead a.collapsed .fa-minus-circle{
            display:none;
        }
        .bxStyle2 {
            background: #f9f9f9;
            border-radius: 6px;
            padding: 8px 0px;
            border: 1px solid #d6d6d6;
            }
        .padTbl>tbody>tr>td {
            padding:10px 0;
            padding-right:15px;
        }

        table#statementType td {
            width: 70px;
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
            bottom: 7px;
            right: 19px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img
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

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridCustOut  
        /*#B2B , 
        #grid_B2BUR , 
        #grid_IMPS , 
        #grid_IMPG ,
        #grid_CDNR ,
        #grid_CDNUR ,
        #grid_EXEMP ,
        #grid_ITCR ,
        #grid_HSNSUM*/
        {
            max-width: 99% !important;
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
        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/
    </style>

    <script>
        $(document).ready(function () {
            $("#rdl_SaleInvoice").click(function () {

                if ($("#rdl_SaleInvoice_1").is(":checked")) {
                    $("#txt_tokenNo").attr("disabled", "disabled");
                    $("#txt_tokenNo").val('');
                   
                } else {
                    $("#txt_tokenNo").removeAttr("disabled");
                    $("#txt_tokenNo").focus();
                }
            });

            if ($("#rdl_SaleInvoice_1").is(":checked")) {
                $("#txt_tokenNo").attr("disabled", "disabled");
                $("#txt_tokenNo").val('');
            }


        })

        function CustomerButnClick(s, e) {
           
        }

        function Validate() {
            var TokenNo = $("#txt_tokenNo").val();
            var EPeriod = $("#rdl_SaleInvoice input:checked").val();
            if ((TokenNo == '') && (EPeriod=='Y')) {
                jAlert('Token no. of previous regular statement is blank. Cannot proceed');
                return false;
            }
            else {
                return true;
            }
        }

        $(function () {
        $("#ddl_FormNo").change(function () {
            var end = $("#ddl_FormNo").val();
            if (end == '24Q') {
                $("#lblFormTxt").text('Whether regular statement for Form 24Q filed for earlier period?');
                $("#lblTokenTxt").text('Token no. of previous regular statement (Form No. 24Q)');
                }
            else if (end == '26Q') {
                $("#lblFormTxt").text('Whether regular statement for Form 26Q filed for earlier period?');
                $("#lblTokenTxt").text('Token no. of previous regular statement (Form No. 26Q)');
                }
        });
        });

        function Showalert() {
            jAlert('There are no sufficient data to generate the eFile');
        }


        function ddlAssesYR_SelectedIndexChanged() {
            $("#hdnAssessmentValue").val($("#ddlAssesYR").val())
            $.ajax({
                type: "POST",
                url: 'TDSeFile.aspx/ddlAssesYR_SelectedIndexChanged',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                data: "{ddlAssesYR:\"" + $("#ddlAssesYR").val() + "\"}",
                success: function (response) {

                    var obj = response.d.split('~');
                    $("#txt_finyr").val(obj[0]);
                    $("#hdnFinValue").val(obj[1]);

                    //console.log(response);
                }
            });
        }

    </script>
    <style>
        .devCheck>table>tbody>tr>td:not(:first-child){
            padding-left:22px;
        }
        .mBot0{
            margin-bottom:0px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="panel-heading">
    <div id="td_contact1" class="panel-title">
        <h3>
            <span id="lblHeadTitle">e TDS File Generation</span>
        </h3>
    </div>

</div>
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="form_main">
        <div class="clearfix bxStyle2">
            <div class="col-md-2">
                <label>Form Number</label>
                <%--Rev 1.0 : "simple-select" class add--%>
                <div class="relative simple-select">
                    <select name="ctl00$ContentPlaceHolder1$PageControl1$cmbBranchType" id="ddl_FormNo" style="width:100%;" runat="server">
			            <option value="24Q">24Q</option>
			            <option value="26Q">26Q</option>
			            <%--<option value="27Q">27Q</option>
			            <option value="27EQ">27EQ</option>--%>
		            </select>
                </div>
            </div>
            <div class="col-md-6">

                <div><label>Select Type of Statement to be Prepared</label></div>

                <div class="radio-inline devCheck">
                    <asp:RadioButtonList id="statementType" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True" Value="R">Regular</asp:ListItem>
                        <asp:ListItem Value="C">Correction</asp:ListItem>
                     </asp:RadioButtonList>
                </div>

            </div>
            <div class="clear"></div>
            <div class="col-md-2">
                <label>Assessment Year </label>
                <%--<div class="relative">                   
                    <asp:TextBox runat="server" ID="txt_assementyr" ReadOnly></asp:TextBox>
                </div>--%>
                 <%--Rev 1.0 : "simple-select" class add--%>
                <div class="relative simple-select">
                    <asp:DropDownList ID="ddlAssesYR" runat="server" Width="100%" AutoPostBack="false" onchange="javascript:ddlAssesYR_SelectedIndexChanged()"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <label>Financial Year </label>
                <div class="relative">

                    <asp:TextBox runat="server" ID="txt_finyr" ReadOnly></asp:TextBox>

                </div>
            </div>
            <div class="col-md-2">
                <label>Quarter </label>
                <%--Rev 1.0 : "simple-select" class add--%>
                <div class="relative simple-select">
                    <select name="ctl00$ContentPlaceHolder1$PageControl1$cmbBranchType" id="ddl_QuaterType" style="width:100%;" runat="server">
			            <option value="Q1">Q1</option>
			            <option value="Q2">Q2</option>
			            <option value="Q3">Q3</option>
			            <option value="Q4">Q4</option>

		            </select>
                </div>
            </div>
            <div class="col-md-2">
                <label>Minor Head Challan </label>
                <%--Rev 1.0 : "simple-select" class add--%>
                <div class="relative simple-select">
                    <select name="ctl00$ContentPlaceHolder1$PageControl1$cmbBranchType" id="ddl_HeadChallan" style="width:100%;" runat="server">
			            <option value="200">200</option>
			            <option value="400">400</option>
		            </select>
                </div>
            </div>
            <div class="clear"></div>
            <div class="col-md-6">
                <table class="padTbl" width="607px">
                    <tr>
                        <%--<td>Whether regular statement for Form 26Q filed for earlier period? </td>--%>
                        <td>
                            <asp:Label ID="lblFormTxt" runat="Server" Text="Whether regular statement for Form 24Q filed for earlier period?" Width="470px" ></asp:Label>
                        </td>
                        <td>
                        <div class="devCheck">
                            <asp:RadioButtonList ID="rdl_SaleInvoice" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                <asp:ListItem Text="No" Value="N" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        
                        </td>
                    </tr>
                        <tr>
                            <%--<td>Token no. of previous regular statement (Form No. 26Q)</td>--%>
                            <td>
                                <asp:Label ID="lblTokenTxt" runat="Server" Text="Token no. of previous regular statement (Form No. 24Q)" Width="470px" ></asp:Label>
                            </td>
                            <td><input type="text" class="form-control" id="txt_tokenNo" runat="server" /></td>
                        </tr>
                </table>
            </div>

            <div class="clear"></div>
            <div class="col-md-12 mTop5">
                <asp:LinkButton ID="lnbGenerateFile" runat="server" OnClientClick="if (!Validate()) return false;" OnClick="lnbGenerateFile_Click" CssClass="btn btn-info">Generate eFile</asp:LinkButton>
                
                <button class="btn btn-danger ">Close</button>
            </div>
        </div>
    </div>
    </div>
    <asp:HiddenField ID="hdnAssessmentValue" runat="server"/>
    <asp:HiddenField ID="hdnFinValue" runat="server"/>
    <asp:HiddenField ID="hdnDSColCount" runat="server"/>
</asp:Content>



