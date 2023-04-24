<%--=======================================================Revision History=======================================    
    1.0   Pallab    V2.0.38   20-04-2023      25885: Cash / Fund Requisition Add module design modification
=========================================================End Revision History=====================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="PaymentRequisition.aspx.cs" Inherits="ERP.OMS.Management.Activities.PaymentRequisition" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    
    <script>
        $(document).ready(function () {

            setTimeout(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 218;
                    grid.SetWidth(cntWidth);
                }
            }, 1000);

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    grid.SetWidth(cntWidth);
                }
            });
        });
    </script>
    <link href="CSS/PaymentRequisition.css" rel="stylesheet" />
    <style>
        /*Rev 1.0*/

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
            right: 18px;
            z-index: 0;
            cursor: pointer;
        }

        .right-20{
            right: 18px !important;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #FormDate , #toDate , #dtTDate , #InstDate, #dt_date
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1 , #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #InstDate_B-1, #dt_date_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img ,
        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #InstDate_B-1 #InstDate_B-1Img, #dt_date #dt_date_B-1Img
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
            top: 34px;
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

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridLocationwiseStockStatus ,
        #GvCBSearch
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

        .mb-10{
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

        .col-md-2
        {
            padding-right: 12px;
            padding-left: 12px;
        }
        /*Rev end 1.0*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="JS/Paymentrequision.js"></script>
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading clearfix">
         <div class="panel-title clearfix">
        <h3>
          
         <dxe:ASPxLabel ID="lblcashfundreq" Width="" runat="server" Text="Cash / Fund Requisition Add">
                                                </dxe:ASPxLabel>
        </h3>
        <div id="divcross" class="crossBtn" visible="false"><a href="PaymentRequisitionList.aspx"><i class="fa fa-times"></i></a></div>
    </div>
   </div>
        <div class="form_main" >
         <div class="row">
             <div class="clearfix" style="padding-top:5px;">
                 <%--Rev 1.0: "simple-select" class add--%>
                  <div class="col-md-3 simple-select" id="ddl_Num" runat="server">

                        <label>
                            <dxe:ASPxLabel ID="lbl_NumberingScheme" Width="160px" runat="server" Text="Numbering Scheme">
                            </dxe:ASPxLabel>
                        </label>
                        <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%" onchange="ddl_numberingSchemechange();">
                        </asp:DropDownList>

             
                    </div>
   
                  
                      
                    <div class="col-md-3">
                            <label>
                                <dxe:ASPxLabel ID="lbl_SlOrderNo" runat="server" Text="Document No" Width="">
                                </dxe:ASPxLabel>
                                <span style="color: red">*</span>
                            </label>

                            <div class="relative">
                                <dxe:ASPxTextBox ID="txt_SlOrderNo" runat="server" ClientInstanceName="ctxt_SlOrderNo" Width="100%" MaxLength="30">
                                    <ClientSideEvents TextChanged="function(s, e) {UniqueCodeCheck();}" />
                                </dxe:ASPxTextBox>
                                <span id="MandatorySlOrderNo" style="display: none" class="validclass">
                                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"/></span>
                            </div>

                        </div>
                        <div class="col-md-3">
                            <label>Date</label>
                            <div>
                            <dxe:ASPxDateEdit ID="dt_date" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" ClientInstanceName="cDate" Width="100%">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
    <%--                                                <ClientSideEvents DateChanged="DateCheck" />
                                                    <ClientSideEvents GotFocus="function(s,e){cPLSalesOrderDate.ShowDropDown();}" />--%>
                                                </dxe:ASPxDateEdit>
                                <span id="Mandatorydate" style="display: none" class="validclass">
                                                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI1" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"/></span>
                            <%--Rev 1.0--%>
                            <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                            <%--Rev end 1.0--%>
                            </div>
                        </div>    
                        <%--Rev 1.0: "simple-select" class add--%>
                            <div class="col-md-3 simple-select">
                                <label>
                                    <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="Unit">
                                    </dxe:ASPxLabel>
                                </label>
                                <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%">
                                </asp:DropDownList>
                            </div>
                        <div class="clear"></div>
                         <div class="col-md-3">
                             <label>Name</label>
                             <span style="color: red">*</span>
                             <div class="relative">
                                <dxe:ASPxTextBox ID="txtname" runat="server" AutoCompleteType="None" ClientInstanceName="ctxtname" Width="100%" MaxLength="30">
                                                       <%-- <ClientSideEvents TextChanged="function(s, e) {UniqueCodeCheck();}" />--%>
                                </dxe:ASPxTextBox>
                                    <span id="Mandatoryname" style="display: none" class="validclass">
                                        <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory" />
                                    </span>
                             </div>
                             
                         </div>
                 <div class="col-md-3" runat="server" id="divjob">
                     <label>Job No <span style="color: red" id="validjob" runat="server">*</span></label>
                     <div class="relative">
                          <div>
                                            <%--<dxe:ASPxLabel ID="lblProject" runat="server" Text="Project"></dxe:ASPxLabel>--%>
                                            <%--<label id="lblProject" runat="server">Project</label>--%>

                            <dxe:ASPxGridLookup ID="lookup_Project"  runat="server" ClientInstanceName="clookup_Project" DataSourceID="EntityServerModeDataSalesOrder"
                                KeyFieldName="Proj_Id" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False">
                                <Columns>
                                        <dxe:GridViewDataColumn FieldName="Proj_Id" Visible="true" VisibleIndex="1" Caption="Project Id" EditFormSettings-Visible="False" Settings-AutoFilterCondition="Contains" Width="0px">
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="Proj_Code" Visible="true" VisibleIndex="1" Caption="Project Code" Settings-AutoFilterCondition="Contains" Width="200px">
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="Proj_Name" Visible="true" VisibleIndex="2" Caption="Project Name" Settings-AutoFilterCondition="Contains" Width="200px">
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="Customer" Visible="true" VisibleIndex="3" Caption="Customer" Settings-AutoFilterCondition="Contains" Width="200px">
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="HIERARCHY_NAME" Visible="true" VisibleIndex="4" Caption="Hierarchy" EditFormSettings-Visible="False" Settings-AutoFilterCondition="Contains" Width="200px">
                                    </dxe:GridViewDataColumn>
                                </Columns>
                                <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                                    <Templates>
                                        <StatusBar>
                                            <table class="OptionsTable" style="float: right">
                                                <tr>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </StatusBar>
                                    </Templates>
                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>

                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                </GridViewProperties>
                                <%--<ClientSideEvents GotFocus="function(s,e){clookup_Project.ShowDropDown();}" LostFocus="clookup_Project_LostFocus" ValueChanged="ProjectValueChange" />--%>

                                <ClearButton DisplayMode="Always">
                                </ClearButton>
                            </dxe:ASPxGridLookup>
                            <dx:LinqServerModeDataSource ID="EntityServerModeDataSalesOrder" runat="server" OnSelecting="EntityServerModeDataSalesOrder_Selecting"
                                ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />
                        </div>
                          <span id="Mandatoryjobno" style="display: none" class="validclass">
                              <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI2" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"/>
                          </span>
                     </div>
                 </div>
                
                 <div class="col-md-3">
                     <label>Mode</label>
                     <div>
                          <dxe:ASPxComboBox ID="ddl_mode" runat="server"  Width="100%" ClientInstanceName="cddl_mode" Font-Size="12px">
                          </dxe:ASPxComboBox>
                     </div>
                 </div>
                 <div class="col-md-3">
                     <label>Service Name</label>
                     <div>
                        <dxe:ASPxTextBox ID="txtservicename" runat="server" ClientInstanceName="ctxtservicename" Width="100%" MaxLength="30">
                            <%-- <ClientSideEvents TextChanged="function(s, e) {UniqueCodeCheck();}" />--%>
                        </dxe:ASPxTextBox>
                     </div>
                 </div>
             </div>
         </div> 

        <div class="gridwraper" style="margin-top:20px">
               <dxe:ASPxGridView runat="server" 
                        OnCustomUnboundColumnData="grid_CustomUnboundColumnData" ClientInstanceName="grid" KeyFieldName="SrlNo" ID="grid"
                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" Settings-ShowFooter="false"
                        OnBatchUpdate="grid_BatchUpdate"
                        OnCustomCallback="grid_CustomCallback"
                        OnDataBinding="grid_DataBinding"
                        OnCellEditorInitialize="grid_CellEditorInitialize"
                        OnRowInserting="Grid_RowInserting"
                        OnRowUpdating="Grid_RowUpdating"
                        OnRowDeleting="Grid_RowDeleting"
                        OnHtmlRowCreated="grid_HtmlRowCreated"
                        OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared"
                        Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200" SettingsPager-Mode="ShowAllRecords">
                                                <SettingsPager Visible="false"></SettingsPager>
                                                <Settings VerticalScrollBarMode="Auto" />
                                                <SettingsBehavior AllowDragDrop="False" AllowSort="False" />
                    <Settings ShowGroupPanel="false" ShowStatusBar="Hidden" ShowFilterRow="false" />
                                                <Columns>
                                                    <dxe:GridViewCommandColumn Caption=" " ShowDeleteButton="false" ShowNewButtonInHeader="false" VisibleIndex="0" Width="30">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton ID="CustomDelete" Image-Url="/assests/images/crs.png" Text=" ">
                                                                <Image Url="/assests/images/crs.png">
                                                                </Image>
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                        <HeaderCaptionTemplate>
                                                            <dxe:ASPxHyperLink ID="btnNew" runat="server" ForeColor="White" Text=" ">
                                                                <ClientSideEvents Click="function (s, e) { OnAddNewClick();}"  />

                                                            </dxe:ASPxHyperLink>
                                                        </HeaderCaptionTemplate>
                                                    </dxe:GridViewCommandColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Sl"  FieldName="SrlNo"   VisibleIndex="1" Width="0">
                                                        <PropertiesTextEdit>
                                                            <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Particulars" FieldName="Particulars"  VisibleIndex="2" Width="80">
                                                        <PropertiesTextEdit>
                                                             <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                     <dxe:GridViewDataTextColumn Caption="Amount" CellStyle-HorizontalAlign="Right" FieldName="Amount"   VisibleIndex="3" Width="100">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                                <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Remarks" FieldName="Remarks"  VisibleIndex="4" Width="200">
                                                        <PropertiesTextEdit>
                                                                <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>       
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewCommandColumn Caption=" " ShowDeleteButton="false" ShowNewButtonInHeader="false" VisibleIndex="5" Width="40">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton ID="AddNew" Image-Url="/assests/images/add.png" Text=" ">
                                                                <Image Url="/assests/images/add.png">
                                                                </Image>
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>
                                             
                                              
                                                </Columns>
                                               
                                                <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" BatchEditStartEditing="gridFocusedRowChanged" />
                                                <SettingsDataSecurity AllowEdit="true" />
                                                <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                                    <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row"/>
                                                </SettingsEditing>
                                                 <Settings VerticalScrollableHeight="150" VerticalScrollBarMode="Auto" />
                                                <SettingsBehavior ColumnResizeMode="Disabled" />
                                            </dxe:ASPxGridView>
        </div>
        <div>
                 <asp:HiddenField runat="server" ID="hddnSaveOrExitButton" />
                <asp:HiddenField ID="hddnDocumentIdTagged" runat="server" />
                <asp:HiddenField ID="hdnnproductIds" runat="server" />
               <asp:HiddenField ID="hdnhidejobno" runat="server" />
                <asp:HiddenField ID="hdfIsDelete" runat="server" />
                <asp:HiddenField ID="hdfSerialDetails" runat="server" />
                <asp:HiddenField ID="hdfBatchDetails" runat="server" />
                <asp:HiddenField ID="hdfLookupCustomer" runat="server" />
                <asp:HiddenField ID="hddnOrderNumber" runat="server" />
                <asp:HiddenField ID="hdfProductID" runat="server" />
            <asp:HiddenField ID="hdnflag" runat="server" />
                <asp:HiddenField ID="hdfProductSerialID" runat="server" />
                <asp:HiddenField ID="hdnRefreshType" runat="server" />
                <asp:HiddenField ID="hdfProductType" runat="server" />
                <asp:HiddenField ID="hdnProductQuantity" runat="server" />
                <asp:HiddenField ID="hdntab2" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hdnCustomerId" runat="server" />
                <asp:HiddenField ID="hdnSalesManAgentId" runat="server" />
                <asp:HiddenField ID="hdnAddressDtl" runat="server" />
                <asp:HiddenField ID="hdnPageStatus" runat="server" />
                <asp:HiddenField ID="hddnActionFieldOnStockBlock" runat="server" />
                <asp:HiddenField ID="hdnSchemaLength" runat="server" />
                <asp:HiddenField runat="server" ID="HDItemLevelTaxDetails" />
                <asp:HiddenField runat="server" ID="HDHSNCodewisetaxSchemid" />
                <asp:HiddenField runat="server" ID="HDBranchWiseStateTax" />
                <asp:HiddenField runat="server" ID="HDStateCodeWiseStateIDTax" />
                <asp:HiddenField runat="server" ID="hddnCustIdFromCRM" />
                <asp:HiddenField runat="server" ID="hdAddOrEdit" />
                <asp:HiddenField ID="hddnuomFactor" runat="server" />
                    <asp:HiddenField runat="server" ID="hdnIsDistanceCalculate" />
                <asp:HiddenField ID="hidIsLigherContactPage" runat="server" />
        <asp:HiddenField runat="server" ID="hdnConvertionOverideVisible" />
        <asp:HiddenField runat="server" ID="hdnShowUOMConversionInEntry" />
        <asp:HiddenField runat="server" ID="hdnGuid" />
        <asp:HiddenField runat="server" ID="hdnPlaceShiptoParty" />
    <asp:HiddenField ID="hddnMultiUOMSelection" runat="server" />
                <asp:HiddenField runat="server" ID="hddnBranchId" />
                <asp:HiddenField runat="server" ID="hddnAsOnDate" />
                <%--kaushik 24-2-2017 --%>
                <asp:HiddenField runat="server" ID="IsUdfpresent" />
                <asp:HiddenField ID="hdnIsFromActivity" runat="server" />
                <asp:HiddenField ID="IsDiscountPercentage" runat="server" />
                <asp:HiddenField runat="server" ID="hdnJsonProductTax" />
                <asp:HiddenField runat="server" ID="hdnConfigValueForTagging" />
                <asp:HiddenField runat="server" ID="hddnOutStandingBlock" />
                <asp:HiddenField runat="server" ID="hddnCustomers" />
                <asp:HiddenField runat="server" ID="uniqueId" />
                <asp:HiddenField runat="server" ID="hdnSalesIrderId" />
            <asp:HiddenField runat="server" ID="hdnsavestat" />
                <asp:HiddenField runat="server" ID="hdnProjectSelectInEntryModule" />
                 <asp:HiddenField runat="server" ID="hdnnumschema" />
        </div>
        <%-- BUTTON SECTION --%>
          <div style="clear: both;"></div>
        <br />
        <div class="clearfix">
            <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
            <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="Save & N&#818;ew" CssClass="btn btn-success" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
            </dxe:ASPxButton>
            <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtn_SaveRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                <ClientSideEvents Click="function(s, e) {SaveExit_ButtonClick();}" />
            </dxe:ASPxButton>
              <b><h4><span id="tagged" style=" color: red" runat="server">This Cash/Fund Req. No. is already tagged in Cash/Bank Voucher,Cannot Modify!</span></h4></b>
        <%-- END BUTTON SECTION --%>
    </div>
        </div>
    </div>
</asp:Content>
