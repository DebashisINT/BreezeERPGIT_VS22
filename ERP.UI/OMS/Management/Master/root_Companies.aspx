<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                16-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="Companies" Language="C#" AutoEventWireup="True" CodeBehind="root_Companies.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Managemnent.Master.management_master_root_Companies" %>

<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>
<%--<%@ Register Assembly="DevExpress.Web.v9.1.Export, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.Export" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.1, Version=9.1.2.0, Cultu7re=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.1, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.1, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.1, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--    <link type="text/css" href="../../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>--%>

    <script language="javascript" type="text/javascript">
        
        function NewPgae(cnt_id) {
            //alert('cnt_id');
        }
        function OnMoreInfoClick(keyValue) {
            var url = 'rootcompany_general.aspx?id=' + keyValue;
            //OnMoreInfoClick(url, "Modify Company Details", '940px', '450px', "Y");
            window.location.href = url;
        }

        function OnAddButtonClick() {
            var url = 'rootcompany_general.aspx?id=' + 'ADD';
            //OnMoreInfoClick(url, "Add Company Details", '940px', '450px', "Y");
            window.location.href = url;
        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function callback() {
            grid.PerformCallback('All');
        }
        //-->
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

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #txtDOB , #txtAnniversary
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1 , #txtDOB_B-1 , #txtAnniversary_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img ,
        #txtDOB_B-1 #txtDOB_B-1Img ,
        #txtAnniversary_B-1 #txtAnniversary_B-1Img
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

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #EmployeeGrid
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
            <h3>Companies</h3>

        </div>
    </div>
        <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td style="text-align: left; vertical-align: top">
                                <table>
                                    <tr>
                                        <td id="ShowFilter">
                                          
                                             <% if (rights.CanAdd)
                                               { %>
                                            <div class="hide">
                                            <a href="javascript:void(0);" onclick="OnAddButtonClick();" class="btn btn-primary"><span>Add New</span></a>
                                                </div>
                                              <%} %>
                                            <% if (rights.CanExport)
                                               { %>
                                            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true"  >
                                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                                 <asp:ListItem Value="2">XLS</asp:ListItem>
                                                 <asp:ListItem Value="3">RTF</asp:ListItem>
                                                 <asp:ListItem Value="4">CSV</asp:ListItem>
                                            </asp:DropDownList>
                                              <%} %>
                                            <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-primary"><span>Show Filter</span></a>--%>
                                        </td>
                                        <td id="Td1">
                                            <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td></td>
                            <td style="float: right; vertical-align: top">
                                <%--<dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true"
                                    Font-Bold="False" ForeColor="black" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                    ValueType="System.Int32" Width="130px">
                                    <Items>
                                        <dxe:ListEditItem Text="Select" Value="0" />
                                        <dxe:ListEditItem Text="PDF" Value="1" />
                                        <dxe:ListEditItem Text="XLS" Value="2" />
                                        <dxe:ListEditItem Text="RTF" Value="3" />
                                        <dxe:ListEditItem Text="CSV" Value="4" />
                                    </Items>
                                    <Border BorderColor="black" />
                                    <DropDownButton Text="Export">
                                    </DropDownButton>
                                </dxe:ASPxComboBox>--%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            
            <tr>
                <td style="vertical-align: top; text-align: left" colspan="2">
                    <dxe:ASPxGridView ID="CompanyGrid" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False"
                        DataSourceID="CompaniesDataSource"  SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" 
 KeyFieldName="cmp_id" Width="100%" OnCellEditorInitialize="CompanyGrid_CellEditorInitialize"
                        OnCustomCallback="CompanyGrid_CustomCallback" SettingsBehavior-AllowFocusedRow="true">
                        <SettingsSearchPanel Visible="True"  Delay="5000"/>
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true"/>
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="cmp_id" ReadOnly="True" Visible="False"
                                VisibleIndex="0">
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="cmp_internalid" Visible="False" VisibleIndex="0">
                           <Settings AllowAutoFilterTextInputTimer="False" />
                                 </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataComboBoxColumn FieldName="cmp_parentid" Visible="False" VisibleIndex="0">
                                <PropertiesComboBox DataSourceID="SqlParentComp" ValueField="cmp_internalid" TextField="cmp_Name"
                                    EnableSynchronization="False" EnableIncrementalFiltering="True" ValueType="System.String">
                                </PropertiesComboBox>
                                <EditFormSettings Caption="Parent Comp" Visible="True" VisibleIndex="0" />
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataTextColumn Caption="Company Name" FieldName="cmp_Name" VisibleIndex="0"
                                Width="30%">
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <PropertiesTextEdit Width="300px">
                                    <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                        <RequiredField ErrorText="Please Enter Company Name" IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="True" VisibleIndex="0" />
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="cmp_natureOfBusiness" Visible="False" VisibleIndex="1"
                                Caption="Nature Of Business">
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <PropertiesTextEdit Width="300px">
                                </PropertiesTextEdit>
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="True" Caption="Nature Of Business" VisibleIndex="1" />
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Directors" FieldName="cmp_directors" VisibleIndex="1"
                                Width="20%">
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <PropertiesTextEdit Width="300px">
                                </PropertiesTextEdit>
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="True" VisibleIndex="2" />
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Authorized Signatories" FieldName="cmp_authorizedSignatories"
                                VisibleIndex="2" Width="15%">
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <PropertiesTextEdit Width="300px">
                                </PropertiesTextEdit>
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="cmp_registrationNo" Visible="False" VisibleIndex="3"
                                Caption="Registration No.">
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <PropertiesTextEdit Width="300px">
                                </PropertiesTextEdit>
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="True" Caption="Registration No." VisibleIndex="3" />
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="PAN No." FieldName="cmp_panNo" VisibleIndex="3" Width="120px">
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <PropertiesTextEdit Width="300px">
                                </PropertiesTextEdit>
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="True" VisibleIndex="5" />
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="cmp_serviceTaxNo" Visible="False" VisibleIndex="3"
                                Caption="Service Tax No.">
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <PropertiesTextEdit Width="300px">
                                </PropertiesTextEdit>
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="True" Caption="Service Tax No." VisibleIndex="7" />
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="cmp_salesTaxNo" Visible="False" VisibleIndex="3"
                                Caption="Sales Tax No.">
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <PropertiesTextEdit Width="300px">
                                </PropertiesTextEdit>
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="True" Caption="Sales Tax No." VisibleIndex="8" />
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataDateColumn Visible="False" VisibleIndex="6" Caption="Date of Incorporation:"
                                FieldName="cmp_DateIncorporation">
                                <EditCellStyle Wrap="False" HorizontalAlign="Left">
                                </EditCellStyle>
                                <PropertiesDateEdit DisplayFormatString="" EditFormat="Custom" EditFormatString="dd MMMM yyyy"
                                    Width="300px" UseMaskBehavior="True">
                                </PropertiesDateEdit>
                                <EditFormSettings Caption="Date of Incorporation:" Visible="True" VisibleIndex="11" />
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataDateColumn>
                            <dxe:GridViewDataTextColumn FieldName="cmp_CIN" Caption="CIN" VisibleIndex="4">
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <PropertiesTextEdit Width="300px">
                                </PropertiesTextEdit>
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="True" VisibleIndex="9" />
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataDateColumn Caption="CIN Validity Date" FieldName="cmp_CINdt" Visible="False"
                                VisibleIndex="5">
                                <PropertiesDateEdit DisplayFormatString="" Width="300px" EditFormat="Custom" EditFormatString="dd MMMM yyyy"
                                    UseMaskBehavior="True">
                                </PropertiesDateEdit>
                                <EditFormSettings Visible="True" VisibleIndex="10" />
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataDateColumn>
                            <dxe:GridViewDataDateColumn Caption="Registration No Validity Date" FieldName="cmp_VregisNo"
                                Visible="False" VisibleIndex="5">
                                <PropertiesDateEdit DisplayFormatString="" Width="300px" EditFormat="Custom" EditFormatString="dd MMMM yyyy"
                                    UseMaskBehavior="True">
                                </PropertiesDateEdit>
                                <EditFormSettings Visible="True" VisibleIndex="4" />
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataDateColumn>
                            <dxe:GridViewDataDateColumn Caption="PanNo Validity Date" FieldName="cmp_VPanNo"
                                Visible="False" VisibleIndex="5">
                                <PropertiesDateEdit DisplayFormatString="" Width="300px" EditFormat="Custom" EditFormatString="dd MMMM yyyy"
                                    UseMaskBehavior="True">
                                </PropertiesDateEdit>
                                <EditFormSettings Visible="True" VisibleIndex="6" />
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataDateColumn>
                            <dxe:GridViewDataTextColumn Caption="Details" VisibleIndex="5" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="100px">
                                <DataItemTemplate>
                                     <% if (rights.CanEdit)
                                               { %>
                                    <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')" title="More Info">

                                        <img src="../../../assests/images/info.png" />
                                    </a>  <% } %>
                                </DataItemTemplate>
                                
                                <EditFormSettings Visible="False" />
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>Actions
                                    <%--<a href="javascript:void(0);" onclick="OnAddButtonClick()"><span style="color: #000099; text-decoration: underline">Add New</span> </a>--%>
                                </HeaderTemplate>
                            </dxe:GridViewDataTextColumn>

                        </Columns>
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <Templates>
                            <EditForm>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 25%"></td>
                                        <td style="width: 50%">

                                            <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors"></dxe:ASPxGridViewTemplateReplacement>

                                            <div style="text-align: right; padding: 2px 2px 2px 2px">
                                                <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                    runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                    runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                            </div>
                                        </td>
                                        <td style="width: 25%"></td>
                                    </tr>
                                </table>
                            </EditForm>
                        </Templates>
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu ="true" />
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="300px" PopupEditFormHorizontalAlign="Center"
                            PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="900px" />
                        <%--<Styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>--%>
                        <SettingsText PopupEditFormCaption="Add/Modify Company Name" ConfirmDelete="Confirm delete?" />
                        <%--<SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>--%>
                        <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                        <StylesEditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </StylesEditors>
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
        <%--========================================================== Master Grid DataSource =================================================--%>
        <asp:SqlDataSource ID="CompaniesDataSource" runat="server" InsertCommand="CompanyInsert" InsertCommandType="StoredProcedure" SelectCommand="CompanySelect"
            SelectCommandType="StoredProcedure" DeleteCommand="CompanyDelete" DeleteCommandType="StoredProcedure"
            UpdateCommand="CompanyUpdate" UpdateCommandType="StoredProcedure">
            <DeleteParameters>
                <asp:Parameter Name="cmp_internalid" Type="string" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="cmp_internalid" Type="String" />
                <asp:Parameter Name="cmp_Name" Type="String" />
                <asp:Parameter Name="cmp_natureOfBusiness" Type="String" />
                <asp:Parameter Name="cmp_directors" Type="String" />
                <asp:Parameter Name="cmp_authorizedSignatories" Type="String" />
                <asp:Parameter Name="cmp_registrationNo" Type="String" />
                <asp:Parameter Name="cmp_panNo" Type="String" />
                <asp:Parameter Name="cmp_serviceTaxNo" Type="String" />
                <asp:Parameter Name="cmp_salesTaxNo" Type="String" />
                <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" />
                <asp:Parameter Name="cmp_CIN" Type="String" />
                <asp:Parameter Name="cmp_DateIncorporation" Type="datetime" />
                <asp:Parameter Name="cmp_CINdt" Type="datetime" />
                <asp:Parameter Name="cmp_VregisNo" Type="datetime" />
                <asp:Parameter Name="cmp_VPanNo" Type="datetime" />
                <asp:Parameter Name="cmp_parentid" Type="string" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="cmp_Name" Type="String" />
                <asp:Parameter Name="cmp_natureOfBusiness" Type="String" />
                <asp:Parameter Name="cmp_directors" Type="String" />
                <asp:Parameter Name="cmp_authorizedSignatories" Type="String" />
                <asp:Parameter Name="cmp_registrationNo" Type="String" />
                <asp:Parameter Name="cmp_panNo" Type="String" />
                <asp:Parameter Name="cmp_serviceTaxNo" Type="String" />
                <asp:Parameter Name="cmp_salesTaxNo" Type="String" />
                <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" />
                <asp:Parameter Name="cmp_CIN" Type="String" />
                <asp:Parameter Name="cmp_DateIncorporation" Type="datetime" />
                <asp:Parameter Name="cmp_CINdt" Type="datetime" />
                <asp:Parameter Name="cmp_VregisNo" Type="datetime" />
                <asp:Parameter Name="cmp_VPanNo" Type="datetime" />
                <asp:Parameter Name="cmp_parentid" Type="string" />
            </InsertParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlParentComp" runat="server"      SelectCommand="SELECT [cmp_internalid], [cmp_Name] FROM [tbl_master_company]"></asp:SqlDataSource>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>
    </div>
    </div>
</asp:Content>
