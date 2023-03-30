<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                30-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ModuleDetails.aspx.cs" Inherits="ERP.OMS.Management.UserForm.ModuleDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #formuladiv {
            background: #f3f3f3;
            border: 1px solid #ccc;
            border-radius: 4px;
            padding: 0 10px 10px 10px;
            margin-bottom: 15px;
        }

        #OrderBy_EC {
            display: none;
        }

        .p-19 {
            padding-top: 19px;
        }

        .pt-15 {
            padding-top: 15px;
        }

        .hd {
            font-size: 18px;
            font-weight: 500;
        }

        .popover-content {
            width: 600px !important;
        }

        .popover {
            max-width: 600px !important;
        }
    </style>

    <style>
        /*Rev 1.0*/

        select
        {
            height: 30px !important;
            border-radius: 4px;
           /* -webkit-appearance: none;
            position: relative;
            z-index: 1;*/
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

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        .simple-select::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 41px;
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
            z-index: 1;
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

        /*select.btn
        {
            padding-right: 10px !important;
        }*/

        /*.panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }*/

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
        .TableMain100 #SalesDetailsGrid, #ShowGrid
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

        .mt-27
        {
            margin-top: 27px !important;
        }

        .col-md-3 , .col-md-2
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

        .btn.btn-xs
        {
                font-size: 14px !important;
        }

        .dxpc-content table
        {
             width: 100%;
        }

        input[type="text"], input[type="password"], textarea
        {
            margin-bottom: 0 !important;
        }
        #FromDate , #ToDate , #ASPxFromDate , #ASPxToDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #FromDate_B-1 , #ToDate_B-1 , #ASPxFromDate_B-1 , #ASPxToDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FromDate_B-1 #FromDate_B-1Img , #ToDate_B-1 #ToDate_B-1Img , #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img
        {
            display: none;
        }

        #lblToDate
        {
            padding-left: 10px;
        }

        .dxtc-activeTab:after {
            content: '';
            width: 0;
            height: 0;
            border-left: 8px solid transparent;
            border-right: 8px solid transparent;
            border-top: 9px solid #3e5395;
            position: absolute;
            /* left: 50%; */
            z-index: 3;
            /* bottom: -15px; */
            margin-left: -9px;
        }

        .calendar-icon
        {
            right: 14px;
        }
        /*Rev end 1.0*/
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="JS/ModuleDetails.js?v=0.2"></script>
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">
                <asp:Label ID="ModuleName" runat="server" Text="" />
            </h3>
        </div>

        <div id="divcross" runat="server" class="crossBtn"><a href="createModuleList.aspx"><i class="fa fa-times"></i></a></div>
    </div>

        <div class="form_main clearfix">
        <div class="clearfix">

            <dxe:ASPxCallbackPanel runat="server" ID="ComponentPanel" ClientInstanceName="cComponentPanel"
                OnCallback="ComponentPanel_Callback">
                <PanelCollection>
                    <dxe:PanelContent runat="server">


                        <div class="row">
                            <div class="col-md-2">
                                <label>Field Type</label>
                                <dxe:ASPxComboBox ID="FiledType" runat="server" ValueType="System.String" ClientInstanceName="cFiledType" Width="100%">
                                    <ClientSideEvents ValueChanged="fieldTypeChange" />
                                </dxe:ASPxComboBox>
                            </div>

                            <div class="col-md-2">
                                <label>Name</label>
                                <dxe:ASPxTextBox ID="txtName" ClientInstanceName="ctxtName"
                                    MaxLength="100" runat="server" Width="100%">
                                </dxe:ASPxTextBox>
                            </div>

                            <div class="col-md-2">
                                <label>Grid(Sequence)</label>
                                <dxe:ASPxTextBox ID="OrderBy" ClientInstanceName="cOrderBy" runat="server" Width="100%">
                                    <MaskSettings Mask="<0..99>" />
                                </dxe:ASPxTextBox>

                            </div>


                            <div class="col-md-2 p-19 ">
                                <label>Visible in List</label>
                                <dxe:ASPxCheckBox ID="vissibleinList" runat="server" ClientInstanceName="cvissibleinList"></dxe:ASPxCheckBox>

                            </div>


                            <div class="col-md-2 p-19">
                                <label>Mandatory</label>
                                <dxe:ASPxCheckBox ID="Mandetory" runat="server" ClientInstanceName="cMandetory"></dxe:ASPxCheckBox>
                            </div>
                            <div class="col-md-2 p-19 hide" id="chkFormula">
                                <label>Formula</label>
                                <dxe:ASPxCheckBox ID="chkIsFormula" runat="server" ClientInstanceName="cchkIsFormula">
                                    <ClientSideEvents ValueChanged="cchkIsFormulaChange" />
                                </dxe:ASPxCheckBox>
                            </div>
                            <div class="clear" />

                            <div class="col-md-4" id="commaSepearedtedDiv" style="display: none">
                                <label>Comma Separated Values</label>
                                <dxe:ASPxTextBox ID="txtValues" ClientInstanceName="ctxtValues"
                                    MaxLength="300" runat="server" Width="100%">
                                </dxe:ASPxTextBox>
                            </div>



                        </div>




                        <div class="clear"></div>
                        <div class="col-md-12 pt-15">
                            <div id="formuladiv" style="display: none">

                                <div class="row">
                                    <h4 class="hd col-md-12">Define Formula</h4>
                                    <div class="col-md-2">
                                        <dxe:ASPxComboBox ID="columnList" ClientInstanceName="ccolumnList" Width="100%"
                                            OnCallback="columnList_Callback" runat="server" ValueType="System.String">
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div class="col-md-3">
                                        <input type="button" class="btn btn-xs btn-success" value="Add" onclick="FormulaAdd()" />

                                        <span data-toggle="popover" data-html="true" id="formulapopover"
                                            data-content=""><i class="fa fa-question-circle"></i></span>
                                    </div>

                                    <div class="clear"></div>
                                    <div class="col-md-12">
                                        <dxe:ASPxMemo ID="memoFormula" runat="server" ClientInstanceName="cmemoFormula"
                                            Height="71px" Width="100%">
                                        </dxe:ASPxMemo>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12 mb-10">
                            <input type="button" value="Save" class="btn brn-sm btn-primary" onclick="SaveNew()" />
                            <input type="button" value="Cancel" class="btn brn-sm btn-danger" onclick="Cancel()" />
                        </div>


                        <asp:HiddenField ID="AddEdit" runat="server" />
                    </dxe:PanelContent>
                </PanelCollection>
                <ClientSideEvents EndCallback="PanelEndCallBack" />
            </dxe:ASPxCallbackPanel>


        </div>
    </div>
    </div>

    <dxe:ASPxGridView ID="Grid" runat="server" KeyFieldName="id"
        Width="100%" ClientInstanceName="cGrid"
        OnDataBinding="Grid_DataBinding"
        SettingsBehavior-AllowFocusedRow="true">
        <Columns>

            <dxe:GridViewDataTextColumn Caption="Name" FieldName="FieldName" Width="50%"
                VisibleIndex="0">
                <CellStyle CssClass="gridcellleft" Wrap="true">
                </CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>


            <dxe:GridViewDataTextColumn Caption="Grid(Sequence)" FieldName="OrderBy" Width="10%"
                VisibleIndex="0">
                <CellStyle CssClass="gridcellleft" Wrap="true">
                </CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>


            <dxe:GridViewDataTextColumn Caption="Mandatory" FieldName="Mandatory" Width="10%"
                VisibleIndex="0">
                <CellStyle CssClass="gridcellleft" Wrap="true">
                </CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>



            <dxe:GridViewDataTextColumn Caption="Field Type" FieldName="VissibleText" Width="20%"
                VisibleIndex="0">
                <CellStyle CssClass="gridcellleft" Wrap="true">
                </CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>

            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center"
                VisibleIndex="17" Width="20%">
                <DataItemTemplate>

                    <a href="javascript:void(0);" class="pad" title="Edit" onclick="onEditClick('<%#Container.KeyValue %>')">
                        <img src="../../../assests/images/info.png" /></a>
                    </a>

                            <a href="javascript:void(0);" class="pad" title="Edit" onclick="onDeleteClick('<%#Container.KeyValue %>')">
                                <img src="../../../assests/images/Delete.png" /></a>
                    </a>
                                              
                </DataItemTemplate>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                <HeaderTemplate><span>Actions</span></HeaderTemplate>
                <EditFormSettings Visible="False"></EditFormSettings>

            </dxe:GridViewDataTextColumn>
        </Columns>
        <SettingsPager PageSize="10">
            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
        </SettingsPager>
    </dxe:ASPxGridView>
    <asp:HiddenField ID="ModuleId" runat="server" />
</asp:Content>
