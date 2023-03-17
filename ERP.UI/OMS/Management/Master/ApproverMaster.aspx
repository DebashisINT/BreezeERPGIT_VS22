<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                17-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ApproverMaster.aspx.cs" Inherits="ERP.OMS.Management.Master.ApproverMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script>
        function SaveApprover() {
            var module = cmodules.GetValue();
            if (module == "0") {
                jAlert('Please select module name.', 'Alert', function () {
                    return;
                });

            }
            cgrid.PerformCallback('Save~' + module);
        }
        function moduletypechange() {
            var module = cmodules.GetValue();
            cgrid.PerformCallback('ShowSelected~' + module);
        }

        function End_Call(s, e) {
            if (cgrid.cpSave != null) {
                jAlert(cgrid.cpSave, 'Alert');
                cgrid.cpSave = null;
            }
        }

    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            setTimeout(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cgrid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cgrid.SetWidth(cntWidth);
                }
            }, 200)

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cgrid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cgrid.SetWidth(cntWidth);
                }

            });
        });
        function goback(e) {
            //e.preventDefault();
            window.location.href = '/OMS/management/ProjectMainPage.aspx';
        }
    </script>

    <style>
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
            <h3>Approval Master</h3>
            <div id="crossdiv" class="crossBtn">
                <a href="#" onclick="goback()"><i class="fa fa-times"></i></a>
            </div>
        </div>
    </div>
        <div class="form_main">
        <div class="row">
        <div class="col-md-4" style="margin-bottom:15px">
            <dxe:ASPxComboBox SelectedIndex="0" runat="server" ID="modules" ClientInstanceName="cmodules" DataSourceID="dsModuleList" ValueField="ID" TextField="MODULE_NAME">
                <ClientSideEvents SelectedIndexChanged="moduletypechange" />
            </dxe:ASPxComboBox>
        </div>
        <div class="clear"></div>
   
    
    <div class="clearfix"></div>
    <div class="col-md-12">
        <dxe:ASPxGridView ID="grid" runat="server" KeyFieldName="id" AutoGenerateColumns="False" OnCustomCallback="grid_CustomCallback"
                                Width="100%" ClientInstanceName="cgrid"  SettingsBehavior-AllowFocusedRow="true"
                                SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
                                SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="220" Settings-VerticalScrollBarMode="Auto"
                                 DataSourceID="dsGvDS" ClientSideEvents-EndCallback="End_Call" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
                                <SettingsSearchPanel Visible="True" Delay="5000" />
                                <%--SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true" SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true" --%>
                                
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="true" VisibleIndex="0"></dxe:GridViewCommandColumn>
                                    <dxe:GridViewDataTextColumn Caption="Sl No." FieldName="sl" Width="50" Visible="false" SortOrder="Descending"
                                        VisibleIndex="1" FixedStyle="Left" >
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Name" FieldName="name" Width="50%"
                                        VisibleIndex="2" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>


                                    <dxe:GridViewDataTextColumn Caption="email" FieldName="email"
                                        VisibleIndex="3" Width="50%">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>




                                                                      
                                </Columns>
                                <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                                <SettingsPager PageSize="10">
                                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                </SettingsPager>
                                <Settings ShowGroupPanel="True" ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" ShowStatusBar="Hidden" ShowHorizontalScrollBar="true" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                <SettingsLoadingPanel Text="Please Wait..." />
                                </dxe:ASPxGridView>
    </div>
    <div class="col-md-12 mTop5" >
        <button type="button" class="btn btn-info">Close</button>
        <button type="button" class="btn btn-success" onclick="SaveApprover();">Save</button>
    </div>
            </div>
</div>
    </div>
    <asp:SqlDataSource ID="dsModuleList" SelectCommand="SELECT 0 AS ID ,'SELECT' AS MODULE_NAME UNION SELECT * FROM MODULELIST_APPROVER" runat="server">

    </asp:SqlDataSource>
    <asp:SqlDataSource ID="dsGvDS" SelectCommand="select * from v_approvers" runat="server">

    </asp:SqlDataSource>
</asp:Content>
