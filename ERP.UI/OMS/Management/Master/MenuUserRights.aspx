<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                20-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="MenuUserRights.aspx.cs" Inherits="ERP.OMS.Management.Master.MenuUserRights" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        #CustomNewNotify {
            visibility: hidden;
            min-width: 250px;
            margin-left: -125px;
            background-color: #54749d;
            color: #fff;
            text-align: center;
            border-radius: 2px;
            padding: 16px;
            position: fixed;
            z-index: 1;
            right: 5%;
            bottom: 30px;
            font-size: 17px;
        }

            #CustomNewNotify.show {
                visibility: visible;
                -webkit-animation: fadein 0.5s, fadeout 0.5s 5.5s;
                animation: fadein 0.5s, fadeout 0.5s 5.5s;
            }

        @-webkit-keyframes fadein {
            from {
                bottom: 0;
                opacity: 0;
            }

            to {
                bottom: 30px;
                opacity: 1;
            }
        }

        @keyframes fadein {
            from {
                bottom: 0;
                opacity: 0;
            }

            to {
                bottom: 30px;
                opacity: 1;
            }
        }

        @-webkit-keyframes fadeout {
            from {
                bottom: 30px;
                opacity: 1;
            }

            to {
                bottom: 0;
                opacity: 0;
            }
        }

        @keyframes fadeout {
            from {
                bottom: 30px;
                opacity: 1;
            }

            to {
                bottom: 0;
                opacity: 0;
            }
        }

        .modal-content {
            width: 500px !important;
            left: 25% !important;
        }

        .rightTable {
            width: 100%;
        }

            /*.rightTable > tbody > tr >td {
           border-top: 1px solid #e2dbdb
            }*/
            .rightTable > tbody > tr > td {
                padding: 2px 5px;
            }

            .rightTable > tbody > tr:hover {
                /*border: 1px solid #e2dbdb;*/
                background: #d3e5e8;
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
        .TableMain100 #GrdHolidays , #StateGrid
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


    <script src="Js/UserRights.js?v=1.0"></script>
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">User Rights [<asp:Label ID="lblmodName" runat="server" Text=""></asp:Label>]</h3>
        </div>
        <div id="divcross" runat="server" class="crossBtn"><a href="root_UserGroups.aspx"><i class="fa fa-times"></i></a></div>
    </div>
        <div class="form_main">

        <button type="button" onclick="TaggedAll()" class="btn btn-success mb-10">Assign All Rights</button>
        <button type="button" onclick="UnTaggedAll()" class="btn btn-info mb-10">Revoke All Rights</button>
          <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius mb-10" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
          </asp:DropDownList>

        <dxe:ASPxGridView ID="GridRights" runat="server" ClientInstanceName="cGridRights" KeyFieldName="id"
            Width="100%" Settings-HorizontalScrollBarMode="Auto"
            SettingsBehavior-ColumnResizeMode="Control" OnDataBinding="GridRights_DataBinding"
            Settings-VerticalScrollableHeight="300" SettingsBehavior-AllowSelectByRowClick="true"
            Settings-VerticalScrollBarMode="Auto" Settings-ShowGroupPanel="true" KeyboardSupport="true"
            Settings-ShowGroupFooter="VisibleAlways" SettingsBehavior-AllowFocusedRow="true"
            Settings-ShowFilterRow="true" Settings-ShowFilterRowMenu="true">
            <Columns>



                <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Module" Width="10%" FieldName="Level1">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>



                <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Menu" Width="10%" FieldName="Level2">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>



                <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Sub-Menu" Width="20%" FieldName="Level3">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Right(s)" Width="50%" FieldName="hasRights">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" Width="10%">
                    <DataItemTemplate>

                        <a href="javascript:void(0);" onclick="AddEditRights('<%# Eval("id") %>','<%# Eval("Level2") %>','<%# Eval("Level3") %>')" class="pad" title="Edit">
                            <img src="../../../assests/images/edit.png" /></a>

                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" CssClass="gridHeader"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <HeaderTemplate><span>Actions</span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                    <Settings AllowAutoFilterTextInputTimer="False" />

                </dxe:GridViewDataTextColumn>

            </Columns>

            <SettingsContextMenu Enabled="true"></SettingsContextMenu>
            <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </SettingsPager>
        </dxe:ASPxGridView>

         <div style="display: none">
            <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
    </div>
    </div>
    <asp:HiddenField ID="HdGroupId" runat="server" />


    <div class="modal fade" id="RightsModel" role="dialog">
        <div class="modal-dialog">

            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">User Rights</h4>
                </div>
                <div class="modal-body">
                    <h4 class="clearfix">
                        <span id="modName"></span>
                        
                        <span class="btn btn-small btn-info" onclick="SelectAll()" style="margin-left:100px">Select All</span> 
                        <span class="btn btn-small btn-info" onclick="deselectALl()">Deselect</span></h4>
                    <table class="rightTable">
                        <tr>
                            <td>
                                <label id="lAddNew">Add New</label>
                            </td>
                            <td>
                                <input type="checkbox" id="AddNew" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lView">View</label>
                            </td>
                            <td>
                                <input type="checkbox" id="View" /></td>
                        </tr>

                        <tr>
                            <td>
                                <label id="lModify">Modify </label>
                            </td>
                            <td>
                                <input type="checkbox" id="Modify" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lDelete">Delete </label>
                            </td>
                            <td>
                                <input type="checkbox" id="Delete" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lCreateActivity">Create Activity </label>
                            </td>
                            <td>
                                <input type="checkbox" id="CreateActivity" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lAddIndustry">Add Industry </label>
                            </td>
                            <td>
                                <input type="checkbox" id="AddIndustry" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lContactPerson">Contact Person </label>
                            </td>
                            <td>
                                <input type="checkbox" id="ContactPerson" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lHistory">History </label>
                            </td>
                            <td>
                                <input type="checkbox" id="History" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lAddUpdateDocuments">Add/Update Documents </label>
                            </td>
                            <td>
                                <input type="checkbox" id="AddUpdateDocuments" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lMembers">Members </label>
                            </td>
                            <td>
                                <input type="checkbox" id="Members" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lOpeningUpdate">Opening Add/Update </label>
                            </td>
                            <td>
                                <input type="checkbox" id="OpeningUpdate" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lAssetDetails">Delivery </label>
                            </td>
                            <td>
                                <input type="checkbox" id="AssetDetails" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lExport">Export </label>
                            </td>
                            <td>
                                <input type="checkbox" id="Export" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lPrint">Print </label>
                            </td>
                            <td>
                                <input type="checkbox" id="Print" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lBudget">Budget </label>
                            </td>
                            <td>
                                <input type="checkbox" id="Budget" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lAssignBranch">Assign Branch </label>
                            </td>
                            <td>
                                <input type="checkbox" id="AssignBranch" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lCancelAssignment">Cancel Assignment</label></td>
                            <td>
                                <input type="checkbox" id="CancelAssignment" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lReassignSupervisor">Reassign Supervisor</label></td>
                            <td>
                                <input type="checkbox" id="ReassignSupervisor" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lClose">Close</label></td>
                            <td>
                                <input type="checkbox" id="Close" /></td>
                        </tr>
                        <tr>
                            <td>

                                <label id="lSpecialEdit">Special Edit</label></td>
                            <td>
                                <input type="checkbox" id="SpecialEdit" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lCancel">Cancel</label></td>
                            <td>
                                <input type="checkbox" id="Cancel" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lImageUpload">Image Upload</label></td>
                            <td>
                                <input type="checkbox" id="ImageUpload" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lRePrintBarcode">Re-Print Barcode</label></td>
                            <td>
                                <input type="checkbox" id="RePrintBarcode" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lDocumentCollection">Document Collection</label></td>
                            <td>
                                <input type="checkbox" id="DocumentCollection" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lClosedSales">Closed Sales</label></td>
                            <td>
                                <input type="checkbox" id="ClosedSales" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lFutureSales">Future Sales</label></td>
                            <td>
                                <input type="checkbox" id="FutureSales" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lClarificationRequired">Clarification Required</label></td>
                            <td>
                                <input type="checkbox" id="ClarificationRequired" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lReassignSalesman">Reassign Salesman</label></td>
                            <td>
                                <input type="checkbox" id="ReassignSalesman" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lViewAdjustment">View Adjustment</label></td>
                            <td>
                                <input type="checkbox" id="ViewAdjustment" /></td>
                        </tr>






                        <tr>
                            <td>
                                <label id="lblSupervisorFeedback">Supervisor Feedback</label></td>
                            <td>
                                <input type="checkbox" id="SupervisorFeedback" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lblSalesmanFeedback">Salesman Feedback</label></td>
                            <td>
                                <input type="checkbox" id="SalesmanFeedback" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lblVerified">Verify</label></td>
                            <td>
                                <input type="checkbox" id="Verified" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lblInfluencer">Influencer</label></td>
                            <td>
                                <input type="checkbox" id="Influencer" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lblRestore">Restore</label></td>
                            <td>
                                <input type="checkbox" id="Restore" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lblAssignTo">Assign To</label></td>
                            <td>
                                <input type="checkbox" id="AssignTo" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lblConvertTo">Convert To</label></td>
                            <td>
                                <input type="checkbox" id="ConvertTo" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lblSalesActivity">Sales Activity</label></td>
                            <td>
                                <input type="checkbox" id="SalesActivity" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lblApproved">Approved</label></td>
                            <td>
                                <input type="checkbox" id="Approved" /></td>
                        </tr>

                        <tr>
                            <td>
                                <label id="lblCreateOrder">Create Order</label></td>
                            <td>
                                <input type="checkbox" id="CreateOrder" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lblQualifyk">Qualify</label></td>
                            <td>
                                <input type="checkbox" id="Qualify" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lblCancelLost">Cancel/Lost</label></td>
                            <td>
                                <input type="checkbox" id="CancelLost" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lblSharing">Sharing</label></td>
                            <td>
                                <input type="checkbox" id="Sharing" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lblProducts">Products</label></td>
                            <td>
                                <input type="checkbox" id="Products" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lblLiterature">Literature</label></td>
                            <td>
                                <input type="checkbox" id="Literature" /></td>
                        </tr>

                         <tr>
                            <td>
                                <label id="lblReadyToInvoice">Ready To Invoice</label></td>
                            <td>
                                <input type="checkbox" id="ReadyToInvoice" /></td>
                        </tr>
                         <tr>
                            <td>
                                <label id="lblMakeInvoice">Make Invoice</label></td>
                            <td>
                                <input type="checkbox" id="MakeInvoice" /></td>
                        </tr>
                          <tr>
                            <td>
                                <label id="lblUpdateTransporter">Update Transporter</label></td>
                            <td>
                                <input type="checkbox" id="UpdateTransporter" /></td>
                        </tr>

                         <tr>
                            <td>
                                <label id="lblIRN">IRN</label></td>
                            <td>
                                <input type="checkbox" id="IRN" /></td>
                        </tr>
                         <tr>
                            <td>
                                <label id="lblEWayBill">E-Way Bill</label></td>
                            <td>
                                <input type="checkbox" id="EWayBill" /></td>
                        </tr>
                         <tr>
                            <td>
                                <label id="lblMRCancellation">MR Cancellation</label></td>
                            <td>
                                <input type="checkbox" id="MRCancellation" /></td>
                        </tr>
                         <tr>
                            <td>
                                <label id="lblWRCancellation">WR Cancellation</label></td>
                            <td>
                                <input type="checkbox" id="WRCancellation" /></td>
                        </tr>
                         <tr>
                            <td>
                                <label id="lblSTBRequisition">STB Requisition</label></td>
                            <td>
                                <input type="checkbox" id="STBRequisition" /></td>
                        </tr>
                         <tr>
                            <td>
                                <label id="lblHolds">Holds</label></td>
                            <td>
                                <input type="checkbox" id="Holds" /></td>
                        </tr>
                         <tr>
                            <td>
                                <label id="lblDirectorApproval">Director Approval</label></td>
                            <td>
                                <input type="checkbox" id="DirectorApproval" /></td>
                        </tr>
                         <tr>
                            <td>
                                <label id="lblInventoryCancellation">Inventory Cancellation</label></td>
                            <td>
                                <input type="checkbox" id="InventoryCancellation" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lblAddReturn">Add Return</label></td>
                            <td>
                                <input type="checkbox" id="AddReturn" /></td>
                        </tr>
                         <tr>
                            <td>
                                <label id="lblPendingDispatch">Pending Dispatch</label></td>
                            <td>
                                <input type="checkbox" id="PendingDispatch" /></td>
                        </tr>
                         <tr>
                            <td>
                                <label id="lblDispatchAcknowledgment">Dispatch Acknowledgment</label></td>
                            <td>
                                <input type="checkbox" id="DispatchAcknowledgment" /></td>
                        </tr>
                        <%--Mantis Issue 24211--%>
                        <tr>
                            <td>
                                <label id="lblCreateOpportunities">Create Opportunities</label></td>
                            <td>
                                <input type="checkbox" id="CreateOpportunities" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lblAutoCloseOpportunities">Auto Close Opportunities</label></td>
                            <td>
                                <input type="checkbox" id="AutoCloseOpportunities" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lblCloseOpportunities">Close Opportunities</label></td>
                            <td>
                                <input type="checkbox" id="CloseOpportunities" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lblReopenOpportunities">Reopen Opportunities</label></td>
                            <td>
                                <input type="checkbox" id="ReopenOpportunities" /></td>
                        </tr>
                        <%--End of Mantis Issue 24211--%>
                        <%--Mantis Issue 24893--%>
                        <tr>
                            <td>
                                <label id="lblTotalAssigned">Total Assigned</label></td>
                            <td>
                                <input type="checkbox" id="TotalAssigned" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lblRepairingPending">Repairing Pending</label></td>
                            <td>
                                <input type="checkbox" id="RepairingPending" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lblServiceEntered">Service Entered</label></td>
                            <td>
                                <input type="checkbox" id="ServiceEntered" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lblServicePending">Service Pending</label></td>
                            <td>
                                <input type="checkbox" id="ServicePending" /></td>
                        </tr>
                        <%--End of Mantis Issue 24893--%>

                        <tr>
                            <td>
                                <label id="lblQuotationStatus">Quotation Status</label></td>
                            <td>
                                <input type="checkbox" id="QuotationStatus" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lblReOpen">ReOpen</label></td>
                            <td>
                                <input type="checkbox" id="ReOpen" /></td>
                        </tr>
                        <%--Mantis Issue 25083--%>
                        <tr>
                            <td>
                                <label id="lblSendSMS">Send SMS</label></td>
                            <td>
                                <input type="checkbox" id="SendSMS" /></td>
                        </tr>
                        <%--End of Mantis Issue 25083--%>
                        <%--Mantis Issue 0024702--%>
                        <tr>
                            <td>
                                <label id="lblPartyInv">Update Party Invoice No. and Date</label></td>
                            <td>
                                <input type="checkbox" id="UpdatePartyInvNoDT" /></td>
                        </tr>
                        <%--End of Mantis Issue 0024702--%>
                        <%--Mantis Issue 25503--%>
                         <tr>
                            <td>
                                <label id="lblUnassign">Unassign</label></td>
                            <td>
                                <input type="checkbox" id="Unassign" /></td>
                        </tr>
                        <%--End of Mantis Issue 25503--%>
                    </table>



                </div>
                <div class="modal-footer">
                    <%--<input type="button" onclick="OnSaveClick()" value="Save" class="btn btn-primary" />--%>
                    <button type="button" class="btn btn-primary mTop5" onclick="OnSaveClick()">Save</button>
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>










    <div id="CustomNewNotify">Some text some message..</div>



    <asp:HiddenField ID="hdnExportValue" runat="server" />

</asp:Content>
