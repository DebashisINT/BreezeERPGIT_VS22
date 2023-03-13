<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                13-03-2023        2.0.36           Pallab              25575 : Report pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="RptProductMasterReport.aspx.cs" Inherits="ERP.OMS.Reports.Master.RptProductMasterReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function ChangeState(value) {
            cProductCallbackPanel.PerformCallback(value);
        }
        function CloseGridLookup() {
            gridLookup.ConfirmCurrentSelection();
            gridLookup.HideDropDown();
            gridLookup.Focus();
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
            right: 20px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
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

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridLocationwiseStockStatus 
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

        select.btn-radius {
    padding: 4px 8px 6px 11px !important;
}
        .mt-30{
            margin-top: 30px;
        }
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
            <h3>Product Catalogue Report</h3>
        </div>
    </div>
        <div class="form_main">
        <div class="Main">

            <%--For gridview selection --%>
            <div class="GridViewArea" style="display: none">
                <dxe:ASPxGridView ID="ProductGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="cProductGrid" DataSourceID="ProductDataSource" KeyFieldName="sProducts_ID"
                    Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" OnCustomCallback="ProductGrid_CustomCallback" SettingsPager-Mode="ShowAllRecords"
                    Settings-ShowFooter="false" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight="400">
                    <SettingsSearchPanel Visible="false" />
                    <settings showgrouppanel="True" showstatusbar="Hidden" showfilterrow="true" showfilterrowmenu="True" />
                    <columns>

                    <dxe:GridViewCommandColumn ShowSelectCheckbox="true" Width="50" Caption="Select" />


                    <dxe:GridViewDataTextColumn Caption="Short Name (Unique)" FieldName="sProducts_Code" ReadOnly="True"
                        Visible="True" FixedStyle="Left"  >
                        <EditFormSettings Visible="True" />
                    </dxe:GridViewDataTextColumn>
                     
                    <dxe:GridViewDataTextColumn Caption="Product Name" FieldName="sProducts_Name" ReadOnly="True"
                        Visible="True" FixedStyle="Left"  >
                        <EditFormSettings Visible="True" />
                    </dxe:GridViewDataTextColumn>
                     

                    <dxe:GridViewDataTextColumn Caption="Description" FieldName="sProducts_Description" ReadOnly="True"
                        Visible="True" FixedStyle="Left"  >
                          <CellStyle Wrap="True" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle> 
                        <EditFormSettings Visible="True" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Product Type" FieldName="sProducts_TypeFull" ReadOnly="True"
                        Visible="True" FixedStyle="Left"  >
                        <EditFormSettings Visible="True" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Product Class" FieldName="ProductClass_Name" ReadOnly="True"
                        Visible="True" FixedStyle="Left" >
                        <EditFormSettings Visible="True" />
                    </dxe:GridViewDataTextColumn>

                </columns>
                    <settingsbehavior columnresizemode="NextColumn" />
                </dxe:ASPxGridView>
            </div>
            <%--For gridview selection --%>

            <%--For GridLookUp selection --%>
            <div class="col-md-3 pdLeft0">
                <dxe:ASPxCallbackPanel runat="server" id="ProductCallbackPanel" ClientInstanceName="cProductCallbackPanel" OnCallback="ProductCallbackPanel_Callback">
                    <panelcollection>
                                                       <dxe:PanelContent runat="server">

                                                      <dxe:ASPxGridLookup ID="GridLookup" runat="server" SelectionMode="Multiple" DataSourceID="ProductDataSource" ClientInstanceName="gridLookup" Height="30"
                                                                                                                KeyFieldName="sProducts_ID" Width="100%" TextFormatString="{0}" MultiTextSeparator=", " >
                                                                                                                <Columns>
                                                                                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption=" "/>
                                                                                                                    <dxe:GridViewDataColumn FieldName="sProducts_Code" Caption="Product Code" Width="150"/>
                                                                                                                    <dxe:GridViewDataColumn FieldName="sProducts_Name" Caption="Product Name" Width="300"/>
                                                                                                                    <dxe:GridViewDataColumn FieldName="ProductClass_Name" Caption="Product Class" Width="300"/>
                                                                                                                </Columns>
                                                                                                                <GridViewProperties  Settings-VerticalScrollBarMode="Auto"   >
                                                                             
                                                                                                                    <Templates>
                                                                                                                        <StatusBar>
                                                                                                                            <table class="OptionsTable" style="float: right">
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                       
                                                                                                                                  <%--      <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />--%>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </StatusBar>
                                                                                                                    </Templates>
                                                                                                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true"/>
                                                                                                                </GridViewProperties>
                                                                                                            </dxe:ASPxGridLookup>
                                                             </dxe:PanelContent>
                                                </panelcollection>
                </dxe:ASPxCallbackPanel>
                </div>

                <%--For GridLookUp selection --%>




                <div class="col-md-5 pdLeft0">
                    <input type="button" value="Select All Products" onclick="ChangeState('SelectAll')" class="btn btn-primary"></input>
                    <input type="button" value="De-select All Products" onclick="ChangeState('UnSelectAll')" class="btn btn-primary"></input>
                    <input type="button" value="Revert" onclick="ChangeState('Revart')" class="btn btn-primary"></input>

                    <asp:Button runat="server" Text="Preview" OnClick="Unnamed_Click" CssClass="btn btn-success" />
                </div>
            
        </div>
        <asp:SqlDataSource ID="ProductDataSource" runat="server"
            SelectCommand=" select sProducts_ID,sProducts_Code,sProducts_Name,sProducts_Description,CASE WHEN sProducts_Type ='A' THEN 'Raw Material'  
     WHEN sProducts_Type ='B' THEN 'Work-In-Process'    
     WHEN  sProducts_Type ='C' THEN 'Finished Goods' END AS sProducts_TypeFull,isnull((select ProductClass_Name from Master_ProductClass where ProductClass_ID=h.ProductClass_Code ),'')ProductClass_Name    from Master_sProducts h"></asp:SqlDataSource>

    </div>
    </div>
</asp:Content>
