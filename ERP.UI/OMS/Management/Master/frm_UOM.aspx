<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                22-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="UOM" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Master.management_master_frm_UOM" CodeBehind="frm_UOM.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        function PopulateGrid(obj) {

            grid.PerformCallback(obj);
        }
        function Changestatus(obj, obj2) {
            var URL = "changeunit.aspx?id=" + obj + "(" + obj2;
            window.location.href = URL;
        }
        function OnAddClick() {
            window.location.href = 'changeunit.aspx?id=Add';
        }
        function OnClickDelete(id) {
            jConfirm("Confirm Delete?", "Alert", function (ret) {
                if (ret)
                { grid.PerformCallback("Del~" + id); }
            });
        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function callback() {
            grid.PerformCallback();
        }
        function callheight(obj) {
            //height();
            // parent.CallMessage();
        }
        document.onkeydown = function (e) {
            if (event.keyCode == 65 && event.altKey == true) {

                if (document.getElementById('AddId'))
                    OnAddClick();
            }
        }
        function GridEndCallBack() {
            if (grid.cpReturnMesg) {
                jAlert(grid.cpReturnMesg, "Alert", function () {
                    // grid.PerformCallback();
                    var URL = "frm_UOM.aspx";
                    window.location.href = URL;
                });
                grid.cpReturnMesg = null;
            }
        }
        function gridRowclick(s, e) {
            $('#grdDocuments').find('tr').removeClass('rowActive');
            $('.floatedBtnArea').removeClass('insideGrid');
            //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
            $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).addClass('rowActive');
            setTimeout(function () {
                //alert('delay');
                var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
                //    setTimeout(function () {
                //        $(this).fadeIn();
                //    }, 100);
                //});    
                $.each(lists, function (index, value) {
                    //console.log(index);
                    //console.log(value);
                    setTimeout(function () {
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }
    </script>

    <style>
        /*Rev 1.0*/

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
        .TableMain100 #GrdHolidays , #cityGrid
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
                <h3>Units Of Measurement [UOM]</h3>
            </div>

        </div>
    <div class="form_main">
        <table class="TableMain100">
            <%-- <tr>
                    <td class="EHEADER" style="text-align: center;">
                        <strong><span style="color: #000099">Units Of Measurement [UOM] </span></strong>
                    </td>
                </tr>--%>
            <tr>
                <td style="text-align: left; vertical-align: top">
                    <table width="100%">
                        <tr>
                            <td id="ShowFilter" class="pull-left">
                                <%-- <a href="javascript:ShowHideFilter('s');" class="btn btn-success"><span>Show Filter</span></a>--%>
                                <% if (rights.CanAdd)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnAddClick()" id="AddId" class="btn btn-success btn-radius "><span class="btn-icon"><i class="fa fa-plus"></i></span><u>A</u>dd New </a>
                                <%} %>

                                <% if (rights.CanExport)
                                   { %>
                                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn  btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                    <asp:ListItem Value="2">XLS</asp:ListItem>
                                    <asp:ListItem Value="3">RTF</asp:ListItem>
                                    <asp:ListItem Value="4">CSV</asp:ListItem>
                                </asp:DropDownList>
                                <% } %>
                            </td>
                            <td id="Td1" class="pull-left">
                                <%-- <a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>
                            </td>


                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="relative">
                    <dxe:ASPxGridView ID="grdDocuments" runat="server" AutoGenerateColumns="False"
                        KeyFieldName="tmp_id" Width="100%" OnRowDeleting="grdDocuments_RowDeleting" ClientInstanceName="grid"
                        OnCustomCallback="grdDocuments_CustomCallback" OnHtmlRowCreated="grdDocuments_HtmlRowCreated" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
                        <SettingsSearchPanel Visible="True" Delay="5000"  />
                        <%--<SettingsBehavior AllowFocusedRow="True" ColumnResizeMode="NextColumn" />--%>
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" />
                        <%-- <ClientSideEvents EndCallback="function(s, e) {
	callheight(s.cpHeight);
}" />--%>
                        <%-- <Templates>
                            <EditForm>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 25%">
                                        </td>
                                        <td style="width: 50%">
                                            <controls>
                                <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors">
                                </dxe:ASPxGridViewTemplateReplacement>                                                           
                            </controls>
                                            <div style="text-align: right; padding: 2px 2px 2px 2px">
                                                <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                    runat="server">
                                                </dxe:ASPxGridViewTemplateReplacement>
                                                <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                    runat="server">
                                                </dxe:ASPxGridViewTemplateReplacement>
                                            </div>
                                        </td>
                                        <td style="width: 25%">
                                        </td>
                                    </tr>
                                </table>
                            </EditForm>
                        </Templates>--%>
                        <Templates>
                            <TitlePanel>
                                <table style="width: 100%">
                                    <tr>
                                        <td align="right">
                                            <table width="200px">
                                                <tr>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </TitlePanel>
                            <EditForm>
                            </EditForm>
                        </Templates>
                        <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="200px" PopupEditFormHorizontalAlign="Center"
                            PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="600px"
                            EditFormColumnCount="1" />

                        <Settings ShowGroupPanel="True" ShowFilterRow="true" ShowFilterRowMenu="True" />
                        <SettingsBehavior ConfirmDelete="True" AllowFocusedRow="true" />
                        <SettingsText PopupEditFormCaption="Add/Modify " ConfirmDelete="Confirm delete?" />
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="tmp_id" ReadOnly="True" VisibleIndex="0" SortOrder="Descending"
                                Visible="false">
                                <CellStyle HorizontalAlign="Left">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="tmp_name" Caption="Name" Width="25%" ReadOnly="True"
                                VisibleIndex="0">
                                <CellStyle HorizontalAlign="left">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <HeaderStyle HorizontalAlign="left" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="tmp_usedfor" Caption="Used For" Width="22%"
                                VisibleIndex="1" Visible="true">
                                <CellStyle HorizontalAlign="left">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <HeaderStyle HorizontalAlign="left" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="tmp_shortname" Caption="Short Name" Width="22%"
                                VisibleIndex="2">
                                <CellStyle HorizontalAlign="left">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <HeaderStyle HorizontalAlign="left" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="tmp_convuomname" Caption="Converted To"
                                Width="22%" VisibleIndex="3">
                                <CellStyle HorizontalAlign="left">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <HeaderStyle HorizontalAlign="left" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Width="0" VisibleIndex="4">
                                <HeaderTemplate>
                                    <span></span>
                                </HeaderTemplate>
                                <DataItemTemplate>
                                    <div class='floatedBtnArea'>
                                    <% if (rights.CanEdit)
                                       { %>
                                    <a href="javascript:void(0);" onclick="Changestatus('<%# Container.KeyValue %>',<%#Eval("tmp_convuom") %>)" title="" class="" style="cursor: pointer;">
                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                                    <%} %>
                                    <% if (rights.CanDelete)
                                       { %>
                                    <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="" title="" id="a_delete">
                                        <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                    <%} %>
                                    </div>
                                </DataItemTemplate>
                                <CellStyle HorizontalAlign="Center">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <%--<dxe:GridViewCommandColumn VisibleIndex="4">
                                <EditButton Visible="True">
                                </EditButton>
                                <DeleteButton Visible="True">
                                </DeleteButton>
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                      { %>
                                    <a href="javascript:void(0);" onclick="grid.AddNewRow()"><span style="color: #000099;
                                        text-decoration: underline">Add New</span> </a>
                                    <%} %>
                                </HeaderTemplate>
                            </dxe:GridViewCommandColumn>--%>
                        </Columns>
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <Styles>
                            <Header SortingImageSpacing="5px" ImageSpacing="5px" CssClass="gridheader"></Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <FocusedGroupRow CssClass="gridselectrow">
                            </FocusedGroupRow>
                            <FocusedRow CssClass="gridselectrow">
                            </FocusedRow>
                        </Styles>
                        <ClientSideEvents EndCallback="GridEndCallBack" RowClick="gridRowclick" />
                    </dxe:ASPxGridView>
                    <%--   <asp:SqlDataSource ID="grddoc" runat="server" 
                            SelectCommand="" DeleteCommand="DELETE FROM [tbl_master_forms] WHERE [frm_id] = @formID">
                            <DeleteParameters>
                                <asp:Parameter Name="formID" Type="int32" />
                            </DeleteParameters>
                            <SelectParameters>
                                <asp:SessionParameter Name="userlist" SessionField="userchildHierarchy" Type="string" />
                            </SelectParameters>
                        </asp:SqlDataSource>--%>
                </td>
            </tr>
            <tr>
                <td style="display: none;">
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></td>
            </tr>
        </table>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
  </div>
</asp:Content>
