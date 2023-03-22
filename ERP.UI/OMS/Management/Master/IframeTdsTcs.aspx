<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                22-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page title="TDS/TCS" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_IframeTdsTcs" CodeBehind="IframeTdsTcs.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Js/IframeTdsTcs.js"></script>   
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
            <h3>TDS/TCS</h3>
        </div>

    </div>
        <div class="form_main">
        <%--Rev 1.0: "clearfix mb-10" class add --%>
            <div class="FilterSide clearfix mb-10">
                <div style="float: left; padding-right: 5px;">
                    <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                    { %>
                    <a href="javascript:void(0);" onclick="OnAddButtonClick( )" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Add New</span> </a>
                    <%} %>
                </div>
                <div class="pull-left">
              <% if (rights.CanExport)
                                               { %>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnChange="if(!AvailableExportOption()){return false;}" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>

                    </asp:DropDownList>
               <%} %>
             
                </div>
        </div>
        <table class="TableMain100">
            <%--    <tr>
                    <td class="EHEADER" style="text-align: center;">
                        <strong><span style="color: #000099">TDS/TCS</span></strong>
                    </td>
                </tr>--%>
            <tr>
                <td class="relative">
                    <dxe:ASPxGridView ID="gridTdsTcs" runat="server" AutoGenerateColumns="False" Width="100%" Settings-ShowFilterRow="true" Settings-ShowFilterRowMenu ="true" 
                        ClientInstanceName="gridTdsTcs" DataSourceID="SqlTdsCts" KeyFieldName="TDSTCS_ID" Settings-ShowFilterBar="Visible" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
                        OnCustomCallback="gridTdsTcs_CustomCallback" OnCustomJSProperties="gridTdsTcs_CustomJSProperties"  SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"  >
                        <SettingsSearchPanel Visible="true" Delay="7000" />
                        <Styles>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <FocusedGroupRow CssClass="gridselectrow">
                            </FocusedGroupRow>
                            <FocusedRow CssClass="gridselectrow">
                            </FocusedRow>
                        </Styles>
                        <Settings ShowStatusBar="Visible" ShowGroupPanel="True" ShowFilterBar="Hidden" />
                        <SettingsBehavior ColumnResizeMode="NextColumn" AllowFocusedRow="true" ConfirmDelete="True" />
                        <SettingsPager ShowSeparators="True" AlwaysShowPager="True" NumericButtonCount="20"
                            PageSize="20">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="TDSTCS_ID" Visible="False" ReadOnly="True"
                                VisibleIndex="0">
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="TDSTCS_Type" Caption="Type" VisibleIndex="1">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="TDSTCS_Code" Caption="Short Name" ReadOnly="True"
                                VisibleIndex="2">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="TDSTCS_Description" Caption="Description"
                                VisibleIndex="3">
                                <EditFormCaptionStyle Wrap="True">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="TDSTCS_MainAccountCode" Caption="Main Account"
                                VisibleIndex="4">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="TDSTCS_SubAccountCode" Caption="Sub Account"
                                VisibleIndex="5">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="6" Width="0">
                                <HeaderStyle HorizontalAlign="Center" />
                                 <HeaderTemplate>
                                     <span ></span>
                                    <%--<%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                      { %>
                                    <a href="javascript:void(0);" onclick="OnAddButtonClick( )"><span>Add New</span> </a>
                                    <%} %>--%>
                                </HeaderTemplate>
                                <DataItemTemplate>
                                    <%-- <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                      { %>
                                    <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Container.KeyValue %>')">
                                        <img src="/assests/images/Edit.png" /></a>
                                    <%} %>--%>
                                    <div class='floatedBtnArea'>
                                    <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Container.KeyValue %>')" class="">
                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                                    <a href="javascript:void(0);" onclick="fn_DeleteTDSTCS('<%# Container.KeyValue %>')" title=""  class="">
                                        <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                     </div>
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                                <CellStyle Wrap="true" HorizontalAlign="Center">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                          <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack();}" RowClick="gridRowclick" />
                       <%-- <ClientSideEvents EndCallback="function(s, e) {EndCall(s);}" />--%>
                     <%--   <ClientSideEvents EndCallback="function(s, e) {EndCall(s.cpEND);}" />--%>
                    </dxe:ASPxGridView>
                    <%--                        SelectCommand="select TDSTCS_ID,TDSTCS_Type,TDSTCS_Code,TDSTCS_Description,(select mainaccount_name from master_mainaccount where mainaccount_accountcode=Master_TDSTCS.TDSTCS_MainAccountCode) as TDSTCS_MainAccountCode,(isnull((select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'') from tbl_master_contact where cnt_internalid=Master_TDSTCS.TDSTCS_SubAccountCode),isnull((select top 1 subaccount_name from master_subaccount where cast(subaccount_code as varchar)=Master_TDSTCS.TDSTCS_SubAccountCode),isnull((select subaccount_name from master_subaccount where cast(subaccount_referenceid as varchar)=Master_TDSTCS.TDSTCS_SubAccountCode),isnull((select top 1 cdslclients_firstholdername from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=Master_TDSTCS.TDSTCS_SubAccountCode),(select nsdlclients_benfirstholdername from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=Master_TDSTCS.TDSTCS_SubAccountCode)))))) as TDSTCS_SubAccountCode from Master_TDSTCS order by TDSTCS_CreateDateTime desc"--%>

                    <%--SelectCommand ="select TDSTCS_ID,TDSTCS_Type,TDSTCS_Code,TDSTCS_Description,(select mainaccount_name from master_mainaccount where mainaccount_accountcode=Master_TDSTCS.TDSTCS_MainAccountCode) as TDSTCS_MainAccountCode,(isnull((select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'') from tbl_master_contact where cnt_internalid=Master_TDSTCS.TDSTCS_SubAccountCode),isnull((select top 1 subaccount_name from master_subaccount where cast(subaccount_code as varchar)=Master_TDSTCS.TDSTCS_SubAccountCode),isnull((select subaccount_name from master_subaccount where cast(subaccount_referenceid as varchar)=Master_TDSTCS.TDSTCS_SubAccountCode),'' ))))  as TDSTCS_SubAccountCode from Master_TDSTCS order by TDSTCS_CreateDateTime desc"--%>
                    <asp:SqlDataSource ID="SqlTdsCts" runat="server"
                        DeleteCommand="DELETE FROM [Master_TDSTCS] WHERE [TDSTCS_ID] = @TDSTCS_ID AND [TDSTCS_Code] = @TDSTCS_Code"
                        InsertCommand="INSERT INTO [Master_TDSTCS] ([TDSTCS_Type], [TDSTCS_Code], [TDSTCS_Description], [TDSTCS_MainAccountCode], [TDSTCS_SubAccountCode], [TDSTCS_CreateUser], [TDSTCS_CreateDateTime], [TDSTCS_ModifyUser], [TDSTCS_ModifyDateTime]) VALUES (@TDSTCS_Type, @TDSTCS_Code, @TDSTCS_Description, @TDSTCS_MainAccountCode, @TDSTCS_SubAccountCode, @TDSTCS_CreateUser, @TDSTCS_CreateDateTime, @TDSTCS_ModifyUser, @TDSTCS_ModifyDateTime)"
                         
                        SelectCommand ="select TDSTCS_ID,TDSTCS_Type,TDSTCS_Code,TDSTCS_Description,(select mainaccount_name from master_mainaccount where mainaccount_accountcode=Master_TDSTCS.TDSTCS_MainAccountCode) as TDSTCS_MainAccountCode,(select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=Master_TDSTCS.TDSTCS_SubAccountCode) as TDSTCS_SubAccountCode from Master_TDSTCS order by TDSTCS_CreateDateTime desc"

                        UpdateCommand="UPDATE [Master_TDSTCS] SET [TDSTCS_Type] = @TDSTCS_Type, [TDSTCS_Description] = @TDSTCS_Description, [TDSTCS_MainAccountCode] = @TDSTCS_MainAccountCode, [TDSTCS_SubAccountCode] = @TDSTCS_SubAccountCode, [TDSTCS_CreateUser] = @TDSTCS_CreateUser, [TDSTCS_CreateDateTime] = @TDSTCS_CreateDateTime, [TDSTCS_ModifyUser] = @TDSTCS_ModifyUser, [TDSTCS_ModifyDateTime] = @TDSTCS_ModifyDateTime WHERE [TDSTCS_ID] = @TDSTCS_ID AND [TDSTCS_Code] = @TDSTCS_Code">
                        <DeleteParameters>
                            <asp:Parameter Name="TDSTCS_ID" Type="Int32" />
                            <asp:Parameter Name="TDSTCS_Code" Type="String" />
                        </DeleteParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="TDSTCS_Type" Type="String" />
                            <asp:Parameter Name="TDSTCS_Description" Type="String" />
                            <asp:Parameter Name="TDSTCS_MainAccountCode" Type="String" />
                            <asp:Parameter Name="TDSTCS_SubAccountCode" Type="String" />
                            <asp:Parameter Name="TDSTCS_ModifyUser" Type="Int32" />
                            <asp:Parameter Name="TDSTCS_ModifyDateTime" Type="DateTime" />
                            <asp:Parameter Name="TDSTCS_ID" Type="Int32" />
                            <asp:Parameter Name="TDSTCS_Code" Type="String" />
                        </UpdateParameters>
                        <InsertParameters>
                            <asp:Parameter Name="TDSTCS_Type" Type="String" />
                            <asp:Parameter Name="TDSTCS_Code" Type="String" />
                            <asp:Parameter Name="TDSTCS_Description" Type="String" />
                            <asp:Parameter Name="TDSTCS_MainAccountCode" Type="String" />
                            <asp:Parameter Name="TDSTCS_SubAccountCode" Type="String" />
                            <asp:Parameter Name="TDSTCS_CreateUser" Type="Int32" />
                            <asp:Parameter Name="TDSTCS_CreateDateTime" Type="DateTime" />
                        </InsertParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>
        </table>
    </div>
    </div>
     <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
</asp:Content>
