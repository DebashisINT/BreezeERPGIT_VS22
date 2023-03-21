<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                21-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkinhHourAddEdit.aspx.cs"
    Inherits="ERP.OMS.Management.Master.WorkinhHourAddEdit" MasterPageFile="~/OMS/MasterPage/ERP.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        th {
            width: 190px !important;
        }

        .mTop15 {
            margin-top: 15px;
        }

        .w400 {
            width: 400px;
        }
        .table thead>tr>th, .table tbody>tr>th, .table tfoot>tr>th, .table thead>tr>td, .table tbody>tr>td, .table tfoot>tr>td {
    padding: 0px;
    line-height: 1.428571429;
    vertical-align: top;
    border-top: 1px solid #ddd;
}
        .table-bordered>thead > tr > th, 
        .table-bordered>tbody > tr > th, 
        .table-bordered>tfoot > tr > th, 
        .table-bordered thead > tr > td, 
        .table-bordered >tbody > tr > td, 
        .table-bordered >tfoot > tr > td {
            padding:8px;
        }
        .minput>table>tbody>tr>td {
            border-top:none !important;
        }
        .minput>table>tbody>tr>td >div {
            padding:2px 4px;
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
        .TableMain100 #GrdHolidays , #WorkingHourGrid
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

        #cmbBranch
        {
            margin-bottom: 5px;
        }

        .w850 {
            width: 850px;
        }

        .table thead>tr>th, .table tbody>tr>th, .table tfoot>tr>th, .table thead>tr>td, .table tbody>tr>td, .table tfoot>tr>td
        {
                vertical-align: middle;
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
    <script src="Js/WorkinhHourAddEdit.js?v=0.02"></script>


    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>&nbsp;Add/Edit Working Roster</h3>
            <div class="crossBtn"><a href="frm_workingShedule.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
        <div class="form_main">
        <label>Roster Name</label>
        <dxe:ASPxTextBox ID="txtName" runat="server" Width="170px" ClientInstanceName="ctxtName"></dxe:ASPxTextBox>



        <table class="table table-bordered mTop15 w850">
            <tr>

                <th></th>
                <th>Is Working Day?</th>
                <th style="width: 190px">Day Begin</th>
                <th style="width: 190px">Day End</th>
                <th style="width: 190px; display: none">Total Break</th>
                <th style="width: 190px">Grace Time (Minute)</th>
            </tr>
            <tr>

                <td>Monday:</td>
                <td>
                    <dxe:ASPxCheckBox ID="chkMonday" ClientInstanceName="cchkMonday" runat="server" ClientSideEvents-CheckedChanged="mondayChkChange"></dxe:ASPxCheckBox>
                </td>
                <td class="tdMonday minput">
                    <dxe:ASPxTimeEdit ID="beginMonday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cbeginMonday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdMonday minput">
                    <dxe:ASPxTimeEdit ID="Endmonday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cEndmonday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdMonday hide minput">
                    <dxe:ASPxTimeEdit ID="brkMonday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cbrkMonday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdMonday minput">
                    <dxe:ASPxTextBox ID="graceMonday" runat="server" Width="170px">
                        <MaskSettings Mask="&lt;0..999&gt;" AllowMouseWheel="false" />
                    </dxe:ASPxTextBox>
                </td>

            </tr>

            <tr>

                <td>Tuesday:</td>
                <td>
                    <dxe:ASPxCheckBox ID="chktuesday" ClientInstanceName="cchktuesday" runat="server" 
                        ClientSideEvents-CheckedChanged="tuesdayCheckChange"></dxe:ASPxCheckBox>
                </td>
                <td class="tdTuesday minput">
                    <dxe:ASPxTimeEdit ID="BeginTuesDay" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cBeginTuesDay">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdTuesday minput">
                    <dxe:ASPxTimeEdit ID="endTuesDay" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cendTuesDay">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td> 
                <td class="tdTuesday hide minput">
                    <dxe:ASPxTimeEdit ID="brkTuesDay" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cbrkTuesDay">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>

                 <td class="tdTuesday minput">
                    <dxe:ASPxTextBox ID="graceTuesday" runat="server" Width="170px">
                        <MaskSettings Mask="&lt;0..999&gt;" AllowMouseWheel="false" />
                    </dxe:ASPxTextBox>
                </td>






            </tr>



            <tr>

                <td>Wednesday:</td>
                <td>
                    <dxe:ASPxCheckBox ID="chkWednesday" ClientInstanceName="cchkWednesday" runat="server" ClientSideEvents-CheckedChanged="WednesdayCheckChange"></dxe:ASPxCheckBox>
                </td>
                <td class="tdWednesday minput">
                    <dxe:ASPxTimeEdit ID="beginWednesday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cbeginWednesday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdWednesday minput">
                    <dxe:ASPxTimeEdit ID="endWednesday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cendWednesday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdWednesday hide minput">
                    <dxe:ASPxTimeEdit ID="brkWednesday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cbrkWednesday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                 <td class="tdWednesday minput">
                    <dxe:ASPxTextBox ID="graceWednesday" runat="server" Width="170px">
                        <MaskSettings Mask="&lt;0..999&gt;" AllowMouseWheel="false" />
                    </dxe:ASPxTextBox>
                </td>



            </tr>


            <tr>

                <td>Thursday:</td>
                <td>
                    <dxe:ASPxCheckBox ID="chkThursday" ClientInstanceName="cchkThursday" runat="server" ClientSideEvents-CheckedChanged="thursdayCheckChange"></dxe:ASPxCheckBox>
                </td>
                <td class="tdthursday minput">

                    <dxe:ASPxTimeEdit ID="beginThursday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cbeginThursday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdthursday minput">
                    <dxe:ASPxTimeEdit ID="endThursday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cendThursday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdthursday hide minput">
                    <dxe:ASPxTimeEdit ID="brkThursday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cbrkThursday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                 <td class="tdthursday minput">
                    <dxe:ASPxTextBox ID="gracethursday" runat="server" Width="170px">
                        <MaskSettings Mask="&lt;0..999&gt;" AllowMouseWheel="false" />
                    </dxe:ASPxTextBox>
                </td>



            </tr>


            <tr>

                <td>Friday:</td>
                <td>
                    <dxe:ASPxCheckBox ID="chkFriday" ClientInstanceName="cchkFriday" runat="server"
                        ClientSideEvents-CheckedChanged="friDayCheckChange">
                    </dxe:ASPxCheckBox>
                </td>
                <td class="tdFriday minput">
                    <dxe:ASPxTimeEdit ID="beginFriday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cbeginFriday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdFriday minput">
                    <dxe:ASPxTimeEdit ID="endFriday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cendFriday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdFriday hide minput">
                    <dxe:ASPxTimeEdit ID="brkFriday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cbrkFriday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdFriday minput">
                    <dxe:ASPxTextBox ID="graceFridDay" runat="server" Width="170px">
                        <MaskSettings Mask="&lt;0..999&gt;" AllowMouseWheel="false" />
                    </dxe:ASPxTextBox>
                </td>


            </tr>

            <tr>

                <td>Saturday:</td>
                <td>
                    <dxe:ASPxCheckBox ID="chkSaturday" ClientInstanceName="cchkSaturday" runat="server"
                        ClientSideEvents-CheckedChanged="SaturdayCheckChange">
                    </dxe:ASPxCheckBox>
                </td>
                <td class="tdSaturday minput">
                    <dxe:ASPxTimeEdit ID="beginSaturday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cbeginSaturday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdSaturday minput">
                    <dxe:ASPxTimeEdit ID="endSaturday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cendSaturday" Height="22">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdSaturday hide minput">
                    <dxe:ASPxTimeEdit ID="brkSaturday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cbrkSaturday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdSaturday minput">
                    <dxe:ASPxTextBox ID="graceSaturday" runat="server" Width="170px">
                        <MaskSettings Mask="&lt;0..999&gt;" AllowMouseWheel="false" />
                    </dxe:ASPxTextBox>
                </td>
            </tr>


            <tr>

                <td>Sunday:</td>
                <td>
                    <dxe:ASPxCheckBox ID="chkSunday" ClientInstanceName="cchkSunday" runat="server"
                        ClientSideEvents-CheckedChanged="sundayCheckChange">
                    </dxe:ASPxCheckBox>
                </td>
                <td class="tdSunday minput">
                    <dxe:ASPxTimeEdit ID="beginSunday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cbeginSunday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdSunday minput">
                    <dxe:ASPxTimeEdit ID="endSunday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cendSunday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                <td class="tdSunday hide minput">
                    <dxe:ASPxTimeEdit ID="brkSunday" runat="server" Width="190" EditFormat="Custom"
                        EditFormatString="h:mm tt" DisplayFormatString="h:mm tt" ClientInstanceName="cbrkSunday">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="None" />
                    </dxe:ASPxTimeEdit>
                </td>
                  <td class="tdSunday minput">
                    <dxe:ASPxTextBox ID="graceSunday" runat="server" Width="170px">
                        <MaskSettings Mask="&lt;0..999&gt;" AllowMouseWheel="false" />
                    </dxe:ASPxTextBox>
                </td>
            </tr>


        </table>
    </div>
    


        <dxe:ASPxButton ID="Save" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="Save_Click" >
        <ClientSideEvents Click="Validate" />
    </dxe:ASPxButton>
    </div>
    <asp:HiddenField ID="hdAddedit" runat="server" />
    <asp:HiddenField ID="hdId" runat="server" />
</asp:Content>





