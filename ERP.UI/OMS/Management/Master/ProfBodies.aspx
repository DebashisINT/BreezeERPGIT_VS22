<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                28-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="Professional/Technical Bodies" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Master.management_master_ProfBodies" CodeBehind="ProfBodies.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function ClickOnMoreInfo(keyValue) {
            var url = 'ProfBodies_general.aspx?id=' + keyValue;
            //OnMoreInfoClick(url, "Modify Professional/Technical Details", '940px', '450px', "Y");
            window.location.href = url;
        }
        function OnAddButtonClick() {
            var url = 'ProfBodies_general.aspx?id=' + 'ADD';
            //OnMoreInfoClick(url, "Add Professional/Technical Details", '940px', '450px', "Y");
            window.location.href = url;
        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function callback() {
            grid.PerformCallback();
        }

    </script>

    <style>
        /*Rev 1.0*/

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
        .TableMain100 #GrdHolidays 
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

        .col-md-3
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
        /*Rev end 1.0*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Professional/Technical Bodies</h3>
        </div>
    </div>
        <div class="form_main">
        <table class="TableMain100" cellpadding="0px" cellspacing="0px">
            <%--<tr>
                <td class="EHEADER" colspan="2" style="text-align: center">
                    <strong><span style="color: #000099">Professional/Technical Bodies</span></strong>
                </td>
            </tr>--%>
            <tr>
                <td style="text-align: left; vertical-align: top" class="gridcellleft">
                    <table>
                        <tr>
                            <td id="ShowFilter">
                                <% if (rights.CanAdd)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success"><span>Add New</span> </a>
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
                                <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-success"><span>Show Filter</span></a>--%>
                            </td>
                            <td id="Td1">
                                <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>
                            </td>
                        </tr>
                    </table>
                </td>
                <%--<td class="gridcellright pull-right">
                    <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true"
                        Font-Bold="False" ForeColor="black" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                        ValueType="System.Int32" Width="130px">
                        <Items>
                            <dxe:ListEditItem Text="Select" Value="0" />
                            <dxe:ListEditItem Text="PDF" Value="1" />
                            <dxe:ListEditItem Text="XLS" Value="2" />
                            <dxe:ListEditItem Text="RTF" Value="3" />
                            <dxe:ListEditItem Text="CSV" Value="4" />
                        </Items>
                        <ButtonStyle>
                        </ButtonStyle>
                        <ItemStyle>
                            <HoverStyle>
                            </HoverStyle>
                        </ItemStyle>
                        <Border BorderColor="black" />
                        <DropDownButton Text="Export">
                        </DropDownButton>
                    </dxe:ASPxComboBox>
                </td>--%>
            </tr>
            <tr>
                <td style="vertical-align: top; text-align: left" colspan="2" class="gridcellcenter">
                    <dxe:ASPxGridView ID="gridProf" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False"
                        DataSourceID="Sqlprof" KeyFieldName="prof_id" Width="100%" OnCustomCallback="gridProf_CustomCallback"
                        OnCustomJSProperties="gridProf_CustomJSProperties">
                        <Styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="prof_id" ReadOnly="True" Visible="False"
                                VisibleIndex="0">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Name" FieldName="prof_name" VisibleIndex="0">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Short Name" FieldName="prof_shortname" VisibleIndex="1">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Registration Number" FieldName="prof_regnNumber"
                                VisibleIndex="2">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <%--<dxe:GridViewDataTextColumn Caption="Details" VisibleIndex="3" Width="5%">
                                <DataItemTemplate>
                                    <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Container.KeyValue %>')">More Info...</a>
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                </HeaderTemplate>
                            </dxe:GridViewDataTextColumn>--%>
                            <dxe:GridViewDataTextColumn VisibleIndex="6" Width="6%">
                                <DataItemTemplate>
                                    <% if(rights.CanView)
                                       { %>
                                    <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Container.KeyValue %>')" title="More Info">
                                        <img src="../../../assests/images/info.png" />
                                    </a>
                                    <% } %>
                                </DataItemTemplate>
                                <CellStyle HorizontalAlign="Center" Wrap="False">
                                </CellStyle>
                                <HeaderTemplate>Actions</HeaderTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <EditFormSettings Visible="False" />                                
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <StylesEditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </StylesEditors>
                        <SettingsText PopupEditFormCaption="Add/ Modify Professional/Technical Bodies" />
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowTitlePanel="True" ShowStatusBar="Visible" ShowFilterRow="true" showfilterrowmenu="true"/>
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True"
                            PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="900px" EditFormColumnCount="3" />
                        <SettingsBehavior ConfirmDelete="True" />
                        <SettingsPager ShowSeparators="True" AlwaysShowPager="True" NumericButtonCount="20"
                            PageSize="20">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <ClientSideEvents EndCallback="function(s, e) {
	                                                        LastCall(s.cpHeight);
                                                        }" />
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="Sqlprof" runat="server" 
            SelectCommand="SELECT [prof_id], [prof_name], [prof_shortname], [prof_regnNumber] FROM [tbl_master_profTechBodies]"></asp:SqlDataSource>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
        <br />
    </div>
    </div>
</asp:Content>
