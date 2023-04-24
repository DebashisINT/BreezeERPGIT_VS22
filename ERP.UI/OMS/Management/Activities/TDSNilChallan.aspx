<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                05-04-2023        2.0.37           Pallab              25886: TDS Nil Challan module design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="TDSNilChallan.aspx.cs" Inherits="ERP.OMS.Management.Activities.TDSNilChallan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchPopup.js?v=0.01"></script>
    <link href="CSS/CustomerReceiptAdjustment.css" rel="stylesheet" />
    <script>
        $(document).ready(function () {
            if ($("#hdAddEdit").val() == "Add") {

                var Quarter = document.getElementById('ddlQuater').value;
                var LastFinYear = '<%=Session["LastFinYear"]%>';
                var year = LastFinYear.split('-')[0];
                var Q1Date = year + "-06-30";
                if (Quarter == "Q1") {
                    ctdsDate.SetDate(new Date(Q1Date));
                }
            }

        });
        //rev srijeeta
        function AllControlInitilize() 
        {
            if (localStorage.getItem('BranchCashBank')) 
            {
                if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('BranchCashBank'))) 
                {
                    ccmbBranchfilter.SetValue(localStorage.getItem('BranchCashBank'));
                }

            }
            //updateGridByDate();
            isFirstTime = false;
        }
        
        //end of rev srijeeta
        function selectTDSChange(s, e) {
            //document.getElementById("chkall").checked = false;
            document.getElementById("lblcount").innerHTML = 0;
            var Quarter = document.getElementById('ddlQuater').value;
            var LastFinYear = '<%=Session["LastFinYear"]%>';
            var FinYearEnd = '<%=Session["FinYearEnd"]%>';
            var YearEnd = FinYearEnd.split('/')[2];
            var year = LastFinYear.split('-')[0];

            if (Quarter == "Q1") {
                var Q1Date = year + "-06-30";
                ctdsDate.SetDate(new Date(Q1Date));
            }
            else if (Quarter == "Q2") {
                var Q2Date = year + "-09-30";
                ctdsDate.SetDate(new Date(Q2Date));
            }
            else if (Quarter == "Q3") {
                var Q3Date = year + "-12-31";
                ctdsDate.SetDate(new Date(Q3Date));
            }
            else if (Quarter == "Q4") {
                var Q4Date = YearEnd + "-03-31";
                ctdsDate.SetDate(new Date(Q4Date));
            }
            ctdsDate.SetEnabled(false);

            var ID = ctdsSection.GetValue();
            var desc = ID.split('~')[1].trim();
            var code = ID.split('~')[0].trim();
            ctxtDeductionON.SetText(desc);

            grid.PerformCallback('TDSPayment~' + ctdsDate.GetDate() + '~' + code + '~' + $("#ddlQuater").val() + '~' + $("#ddlFinYear").val() + '~' + $("#ddlEntityType").val());

            //$.ajax({
            //    type: "POST",
            //    url: "CashBankEntry.aspx/GETTDSDOCDETAILS",
            //    data: JSON.stringify({ TDSPaymentDate: cdtTDate.GetDate(), TDSCode: code, TDSQuater: $("#ddlQuater").val(), TDSYear: $("#ddlFinYear").val(), Type: $("#ddlEntityType").val() }),
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    async: false,
            //    success: function (msg) {
            //        var data = msg.d;
            //        tdspay = data;
            //        // debugger;
            //        for (var i = 0; i < data.length; i++) {
            //            document.getElementById("chkall").disabled = false;

            //            var obj = {};
            //            obj.newId = data[i].DETID;


            //            str += "<tr>";
            //            str += "<td><input onclick='iCheckClick(this,\"" + data[i].DETID + "\");'  type='checkbox' id='chk" + data[i].DETID + "'/></td>";
            //            str += "<td class='hide'>" + data[i].DETID + "</td>";
            //            str += "<td class='hide'>" + data[i].VendorId + "</td>";
            //            str += "<td class='hide'>" + data[i].MainAccountID + "</td>";
            //            str += "<td>" + data[i].DocumentNo + "</td>";
            //            str += "<td>" + data[i].PartyID + "</td>";
            //            str += "<td>" + data[i].TDSTCS_Code + "</td>";
            //            str += "<td>" + data[i].PaymentDate + "</td>";
            //            str += "<td>" + data[i].Total_Tax + "</td>";
            //            str += "<td>" + data[i].Tax_Amount + "</td>";
            //            str += "<td>" + data[i].Surcharge + "</td>";
            //            str += "<td>" + data[i].EduCess + "</td>";
            //            str += "<td>" + data[i].IsOpening + "</td>";

            //            str += "</tr>";

            //        }


            //    }
            //});



            // ctdsDate.SetDate(cdtTDate.GetDate());
            ctxtSurcharge.SetText(0);
            ctxteduCess.SetText(0);
            ctxtTotal.SetText(0);
            ctxtTax.SetText(0);

        }
    </script>
    <style>
        /*Rev 1.0*/

        select
        {
            height: 30px !important;
            border-radius: 4px !important;
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

        .calendar-icon-2 {
            position: absolute;
            bottom: 7px;
            right: 4px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #FormDate , #toDate , #dtTDate , #tdsDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1 , #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #tdsDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img ,
        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #tdsDate_B-1 #tdsDate_B-1Img
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
            line-height: 18px;
            z-index: 0;
        }
        .simple-select {
            position: relative;
                z-index: 0;
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

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridLocationwiseStockStatus 
        
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

        /*.col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }*/

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

        #massrecdt
        {
            width: 100%;
        }

        .col-sm-3{
            margin-bottom: 10px;
        }

        .crossBtn
        {
            top: 25px;
                right: 25px;
        }

        input[type="text"], input[type="password"], textarea
        {
                margin-bottom: 0;
        }

        #CallbackPanel_LPV
        {
                top: 410px !important;
        }
        /*Rev end 1.0*/
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="JS/TDSNilChallan.js?v=2.3"></script>


    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-title clearfix" id="myDiv">
        <h3 class="pull-left">
            <asp:Label ID="lblHeading" runat="server" Text="TDS Nil Challan"></asp:Label>

        </h3>
    </div>
        <div id="ApprovalCross" runat="server" class="crossBtn"><a href="TDSNilChallanList.aspx"><i class="fa fa-times"></i></a></div>

    
        <div class="form_main">
        <div class="row">
            <div class="col-md-12">
                <div class="row">
                    <div class="col-sm-3">
                        <label id="lblsection">Section</label>
                        <div class="relative">
                            <dxe:ASPxComboBox runat="server" SelectedIndex="0" ID="tdsSection" Width="100%" ClientInstanceName="ctdsSection" ValueField="ID" TextField="Section_Code" ValueType="System.String">
                                <ClientSideEvents SelectedIndexChanged="selectTDSChange" />
                            </dxe:ASPxComboBox>
                            <span id="MandatorySection" class="Section  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            <%-- <asp:SqlDataSource runat="server" ID="dsTDS" SelectCommand="select '0~' as ID ,'Select' Section_Code Union ALL  Select Section_Code +'~'+Section_Description  ID,  Section_Code from tbl_master_TDS_Section  SEC inner join Master_TDSTCS TDS on TDS.TDSTCS_Code=SEC.Section_Code"></asp:SqlDataSource>
                            --%>
                        </div>
                    </div>

                    <div class="col-sm-3">
                        <label>TDS Deducted on</label>
                        <div class="relative">
                            <dxe:ASPxTextBox ID="txtDeductionON" ClientEnabled="false" Width="100%" runat="server" ClientInstanceName="ctxtDeductionON"></dxe:ASPxTextBox>
                        </div>
                    </div>
                    <div class="col-sm-3 for-cust-icon">
                        <label>Date of Deposit</label>
                        <div class="">
                            <dxe:ASPxDateEdit ID="tdsDate" ClientEnabled="false" runat="server" ClientInstanceName="ctdsDate" EditFormat="Custom"
                                Font-Size="12px" UseMaskBehavior="True" Width="100%" EditFormatString="dd-MM-yyyy" CssClass="pull-left">
                                <ButtonStyle Width="13px"></ButtonStyle>

                            </dxe:ASPxDateEdit>
                            <%--Rev 1.0--%>
                            <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                            <%--Rev end 1.0--%>
                        </div>
                    </div>
                    <div class="col-sm-3">
                        <label>Financial Year</label>
                        <%--Rev 1.0: "simple-select" class add --%>
                        <div class="relative simple-select">
                            <select id="ddlFinYear" runat="server" onchange="selectTDSChange(this,this);" class="form-control">
                                <option value="2019-20">2019-20</option>
                                <option value="2020-21">2020-21</option>
                                <option value="2021-22">2021-22</option>
                                <option value="2022-23">2022-23</option>
                                <option value="2023-24">2024-25</option>

                            </select>
                        </div>
                    </div>


                    <div class="col-sm-3">
                        <label>Quarter</label>
                        <%--Rev 1.0: "simple-select" class add --%>
                        <div class="relative simple-select">
                            <select id="ddlQuater" runat="server" onchange="selectTDSChange(this,this);" class="form-control">
                                <option value="Q1">Q1</option>
                                <option value="Q2">Q2</option>
                                <option value="Q3">Q3</option>
                                <option value="Q4">Q4</option>

                            </select>
                        </div>
                    </div>


                    <div class="col-sm-3">
                        <label>Type</label>
                        <%--Rev 1.0: "simple-select" class add --%>
                        <div class="relative simple-select">
                            <select id="ddlEntityType" runat="server" onchange="selectTDSChange(this,this);" class="form-control">
                                <option value="0">All</option>
                                <option value="01">Company</option>
                                <option value="02">Other than Company</option>

                            </select>
                        </div>
                    </div>

                    <div class="col-sm-3">
                        <label>Surcharge</label>
                        <div class="relative">
                            <dxe:ASPxTextBox ID="txtSurcharge" DisplayFormatString="0.00" Width="100%" runat="server" ClientEnabled="false" ClientInstanceName="ctxtSurcharge">

                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />

                            </dxe:ASPxTextBox>

                        </div>
                    </div>
                    <div class="col-sm-3">
                        <label>Education Cess</label>
                        <div class="relative">
                            <dxe:ASPxTextBox ID="txteduCess" DisplayFormatString="0.00" Width="100%" runat="server" ClientEnabled="false" ClientInstanceName="ctxteduCess">
                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                            </dxe:ASPxTextBox>

                        </div>
                    </div>
                    <div class="clear"></div>
                    <div class="col-sm-3">
                        <label>Interest</label>
                        <div class="relative ">
                            <dxe:ASPxTextBox ID="txtInterest" DisplayFormatString="0.00" runat="server" ClientInstanceName="ctxtInterest" Width="100%">
                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                <ClientSideEvents LostFocus="RecalculateTotal" />

                            </dxe:ASPxTextBox>

                        </div>
                    </div>

                    <div class="col-sm-3">
                        <label>Late Fees (u/s 234E)</label>
                        <div class="relative ">
                            <dxe:ASPxTextBox ID="txtLateFees" DisplayFormatString="0.00" runat="server" ClientInstanceName="ctxtLateFees " Width="100%">
                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                <ClientSideEvents LostFocus="RecalculateTotal" />
                            </dxe:ASPxTextBox>

                        </div>
                    </div>

                    <div class="col-sm-3">
                        <label><b>Total</b></label>
                        <div class="relative">
                            <dxe:ASPxTextBox ID="txtTotal" DisplayFormatString="0.00" ClientEnabled="false" runat="server" ClientInstanceName="ctxtTotal" Width="100%">
                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                            </dxe:ASPxTextBox>

                        </div>
                    </div>

                    <div class="col-sm-3">
                        <label><b>Tax</b></label>
                        <div class="relative">
                            <dxe:ASPxTextBox ID="txtTax" DisplayFormatString="0.00" ClientEnabled="false" runat="server" ClientInstanceName="ctxtTax" Width="100%">
                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                            </dxe:ASPxTextBox>

                        </div>
                    </div>

                    <div class="col-sm-3">
                        <label>Others</label>
                        <div class="relative noerrormask">
                            <dxe:ASPxTextBox ID="txtOthers" DisplayFormatString="0.00" runat="server" ClientInstanceName="ctxtOthers" Width="100%">
                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                <ClientSideEvents LostFocus="RecalculateTotal" />

                            </dxe:ASPxTextBox>

                        </div>
                    </div>

                    <%--<rev srijeeta--%>
                    <div class="col-sm-3">
                        <label> Branch </label>
                        <div class="relative noerrormask">
                            <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                            </dxe:ASPxComboBox>
                       </div>
                    </div>
                    <%--<end of rev srijeeta--%>
                   
                    <div class="clear"></div>
                    <%--<div class="bdbox clearfix">
                        <div class="headingTypeblo">Bank Details</div>--%>
                    <%-- <div class="bdboxContent row">--%>
                    <div class="col-sm-3">
                        <label>Bank Name</label>
                        <div class="relative">
                            <dxe:ASPxTextBox ID="txtBankName" runat="server" ClientInstanceName="ctxtBankName" Width="100%"></dxe:ASPxTextBox>

                        </div>
                    </div>

                    <div class="col-sm-3">
                         <%--rev srijeeta --%>
                        <%--<label>Branch</label>--%>
                        <label>Bank Branch</label>
                        <%--end of rev srijeeta--%>
                        <div class="relative">
                            <dxe:ASPxTextBox ID="txtBankBrach" runat="server" ClientInstanceName="ctxtBankBranch" Width="100%"></dxe:ASPxTextBox>

                        </div>
                    </div>

                    <div class="col-sm-3">
                        <label>BSR Code</label>
                        <div class="relative">
                            <dxe:ASPxTextBox ID="txtBRS" runat="server" ClientInstanceName="ctxtBRS" Width="100%"></dxe:ASPxTextBox>

                        </div>
                    </div>

                    <div class="col-sm-3">
                        <label>Challan No</label>
                        <div class="relative">
                            <dxe:ASPxTextBox ID="txtChallanNo" runat="server" ClientInstanceName="ctxtChallanNo" Width="100%"></dxe:ASPxTextBox>
                            <span id="MandatoryChallanNo" class="ChallanNo  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                        </div>
                    </div>
                    <%--  </div>--%>
                    <br />
                    <%--<div class="col-sm-3">
                            <label>Select/Deselect All</label>
                            <input type='checkbox' onclick="chechall()" id="chkall" />
                            <input type="hidden" id="hddnflag" />
                        </div>--%>
                    <div class="col-sm-4"></div>
                    <div class="col-sm-2"></div>
                    <div class="col-sm-2">
                        <label><font color="red">Items Selected:</font></label>
                        <label id="lblcount" style="color: red;"></label>
                    </div>
                    <%-- </div>--%>
                    <div class="clear"></div>





                    <div class="clear"></div>
                    <div class="clear"></div>
                    <div class="clear"></div>



                    <div class="col-md-12" style="top: 20px;">
                        <dxe:ASPxGridView runat="server" ClientInstanceName="grid" ID="grid"
                            OnCellEditorInitialize="grid_CellEditorInitialize"
                            OnBatchUpdate="grid_BatchUpdate"
                            OnDataBinding="grid_DataBinding"
                            OnCustomCallback="grid_CustomCallback"
                            OnRowInserting="Grid_RowInserting"
                            OnRowUpdating="Grid_RowUpdating"
                            OnRowDeleting="Grid_RowDeleting"
                            OnCustomJSProperties="grid_CustomJSProperties"
                            KeyFieldName="DETID"
                            SettingsBehavior-AllowSort="false"
                            SettingsPager-Mode="ShowAllRecords"
                            Settings-VerticalScrollBarMode="auto"
                            Settings-VerticalScrollableHeight="250"
                            Settings-HorizontalScrollBarMode="Visible"
                            Width="100%">
                            <SettingsPager Visible="false"></SettingsPager>
                            <Columns>
                                <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="50px" Caption=" " VisibleIndex="1" />

                                <dxe:GridViewDataTextColumn Caption="Document No." FieldName="Document_No" VisibleIndex="2" Width="200px" ReadOnly="true">
                                    <CellStyle Wrap="False" HorizontalAlign="Left"></CellStyle>

                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Party ID" VisibleIndex="3" FieldName="PartyID" Width="250px" ReadOnly="true">
                                    <CellStyle Wrap="False" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Section" FieldName="TDSTCS_Code" Width="160px" ReadOnly="true">
                                    <CellStyle Wrap="False" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>

                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn FieldName="PaymentDate" Caption="Payment/Credit Date" VisibleIndex="5" Width="180px" ReadOnly="true">
                                    <CellStyle Wrap="False" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>

                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Total_Tax" Caption="Total Tax" VisibleIndex="6" Width="120px" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                                    <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>

                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="7" Caption="Amount of Tax" ReadOnly="true" FieldName="Tax_Amount" Width="120px" HeaderStyle-HorizontalAlign="Right">
                                    <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="8" Caption="Surcharge" FieldName="Surcharge" Width="120px" ReadOnly="true" HeaderStyle-HorizontalAlign="Right">
                                    <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="9" Caption="Education Cess" FieldName="EduCess" Width="120px" ReadOnly="true" HeaderStyle-HorizontalAlign="Right">
                                    <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="10" Caption="Is Opening" FieldName="IsOpening" Width="150px" ReadOnly="true">
                                    <CellStyle Wrap="False" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <%--<dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="8%" VisibleIndex="10" Caption=" ">
                                    <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                    <CustomButtons>
                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="AddNew" Image-Url="/assests/images/add.png">
                                            <Image Url="/assests/images/add.png">
                                            </Image>
                                        </dxe:GridViewCommandColumnCustomButton>
                                    </CustomButtons>
                                </dxe:GridViewCommandColumn>--%>
                            </Columns>
                            <ClientSideEvents RowClick="GetVisibleIndex" BatchEditStartEditing="gridFocusedRowChanged"
                                EndCallback="GridEndCallBack" SelectionChanged="grid_SelectionChanged" />
                            <SettingsDataSecurity AllowEdit="true" />
                            <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="Row" />
                            </SettingsEditing>

                            <Settings ShowStatusBar="Hidden" />
                            <Styles>
                                <StatusBar CssClass="statusBar">
                                </StatusBar>
                            </Styles>
                        </dxe:ASPxGridView>
                    </div>

                </div>
            </div>
            <div class="clear"></div>
            <div class="row">
                <div class="col-md-12" style="top: 60px; left: 13px;">
                    <table style="float: left;" id="tblBtnSavePanel">
                        <tr>
                            <td style="width: 100px;" id="tdSaveButton" runat="Server">
                                <% if (rights.CanAdd)
                                   { %>
                                <dxe:ASPxButton ID="btnSaveRecords" ClientInstanceName="cbtnSaveRecords" runat="server" AutoPostBack="False" Text="S&#818;ave & New" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                    <ClientSideEvents Click="function(s, e) {SaveButtonClick();}" />
                                </dxe:ASPxButton>
                                <%} %>
                            </td>
                            <td style="width: 100px;" id="td_SaveButton" runat="Server">
                                <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                    <ClientSideEvents Click="function(s, e) {SaveExitButtonClick();}" />
                                </dxe:ASPxButton>
                            </td>


                        </tr>
                    </table>
                </div>
            </div>


        </div>


    </div>
        <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="tblBtnSavePanel"
        Modal="True">
    </dxe:ASPxLoadingPanel>
    </div>
    <!--Customer Modal -->
    <div class="modal fade" id="CustModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Customer Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search By Customer Name or Unique Id" />

                    <div id="CustomerTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Customer Name</th>
                                <th>Unique Id</th>
                                <th>Address</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>


    <%--Advance Receipt Selection Model--%>
    <div class="modal fade" id="AdvanceModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Advance Receipt Search</h4>
                </div>
                <div class="modal-body">
                    <div id="AdvRecDocTbl">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Receipt Number</th>
                                <th>Receipt Date</th>
                                <th>Amount</th>
                                <th>Balance</th>

                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>


    <%--Document Selection Model--%>
    <div class="modal fade" id="DocumentModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Document Search</h4>
                </div>
                <div class="modal-body">
                    <div id="DocNoDocTbl">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Document Number</th>
                                <th>Document Date</th>
                                <th>Document Type</th>
                                <th>Document Amount</th>
                                <th>Balance Amount</th>

                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>


    <%-- HiddenField Feild  --%>
    <asp:HiddenField ID="hdnCustomerId" runat="server" />
    <asp:HiddenField ID="hdAddEdit" runat="server" />
    <asp:HiddenField ID="hdAdvanceDocNo" runat="server" />
    <asp:HiddenField ID="hdAdjustmentId" runat="server" />
    <asp:HiddenField ID="hdAdjustmentType" runat="server" />
    <asp:HiddenField ID="hddnProjectId" runat="server" />
    <asp:HiddenField ID="hdnProjectSelectInEntryModule" runat="server" />

    <asp:HiddenField ID="hdnLockFromDate" runat="server" />
    <asp:HiddenField ID="hdnLockToDate" runat="server" />
    <asp:HiddenField ID="hdnLockFromDateCon" runat="server" />
    <asp:HiddenField ID="hdnLockToDateCon" runat="server" />

</asp:Content>
