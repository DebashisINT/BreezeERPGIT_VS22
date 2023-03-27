<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                27-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="TaxEcceptionrulesList.aspx.cs" Inherits="ERP.OMS.Management.Master.TaxEcceptionrulesList" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script>
        function gridRowclick(s, e) {
            $('#grid').find('tr').removeClass('rowActive');
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

        function OnEdit(Code,Type)
        {
            window.location.href = "TaxExceptionRules.aspx?Code=" + Code + "&Type=" + Type;
        }
     </script>
    <script type="text/javascript">
        $(document).ready(function () {
            setTimeout(function () {
                console.log('setTime');
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    grid.SetWidth(cntWidth);
                }
            }, 500);

            $('.navbar-minimalize').click(function () {
                console.log('Click');
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
        .TableMain100 #GrdHolidays , #gridudfGroup
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
        /*Rev end 1.0*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Tax Exception</h3>

        </div>
    </div>
        <div class="form_main clearfix">
        <div class="relative">
            <dxe:ASPxGridView ID="grid" runat="server" ClientInstanceName="grid" AutoGenerateColumns="False"
                    Width="100%" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
                    SettingsDataSecurity-AllowDelete="false" SettingsDataSecurity-AllowEdit="false" DataSourceID="EntityServerModeDataSource"
                    SettingsDataSecurity-AllowInsert="false" KeyFieldName="(None)" >

                    <SettingsPager PageSize="10">
                        <FirstPageButton Visible="True"></FirstPageButton>

                        <LastPageButton Visible="True"></LastPageButton>

                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                    </SettingsPager>
                    <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />

                    <ClientSideEvents EndCallback="function(s, e) {
	                                            LastCall(s.cpHeight);
                                            }" />

                    <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" AlwaysShowPager="True">
                        <FirstPageButton Visible="True">
                        </FirstPageButton>
                        <LastPageButton Visible="True">
                        </LastPageButton>

                        <PageSizeItemSettings Items="10,50, 100, 150, 200" Visible="True"></PageSizeItemSettings>
                    </SettingsPager>

                    <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="200px" PopupEditFormHorizontalAlign="Center"
                        PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="600px"
                        EditFormColumnCount="1" />
                    <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />

                    <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" />

                    <SettingsCommandButton>

                        <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                            <Image AlternateText="Edit" Url="../../../assests/images/Edit.png"></Image>
                        </EditButton>
                        <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete" Styles-Style-CssClass="pad">
                            <Image AlternateText="Delete" Url="../../../assests/images/Delete.png"></Image>
                        </DeleteButton>
                        <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary" Image-Width>
                            <Styles>
                                <Style CssClass="btn btn-primary"></Style>
                            </Styles>
                        </UpdateButton>
                        <CancelButton Text="Cancel" ButtonType="Button"></CancelButton>
                    </SettingsCommandButton>
                    <SettingsSearchPanel Visible="True" Delay="7000" />
                    <SettingsText PopupEditFormCaption="Add/Modify Category" ConfirmDelete="Confirm delete?" />
                    <StylesEditors>
                        <ProgressBar Height="25px">
                        </ProgressBar>
                    </StylesEditors>

                    <Columns>
                        <dxe:GridViewDataTextColumn FieldName="SrlNo" ReadOnly="True" Visible="False" VisibleIndex="0">
                            <EditFormSettings Visible="False" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>


                        <dxe:GridViewDataTextColumn FieldName="Code" Caption="Code" Width="200"
                            VisibleIndex="1" ShowInCustomizationForm="True">
                            <EditCellStyle Wrap="True">
                            </EditCellStyle>
                            <CellStyle CssClass="gridcellleft" Wrap="True">
                            </CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn FieldName="Description" Caption="Description" Width="50%"
                            VisibleIndex="2" ShowInCustomizationForm="True">
                            <EditCellStyle Wrap="True">
                            </EditCellStyle>
                            <CellStyle CssClass="gridcellleft" Wrap="True">
                            </CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn FieldName="Type" Caption="Type" Width="200"
                            VisibleIndex="3" ShowInCustomizationForm="True">
                            <EditCellStyle Wrap="True">
                            </EditCellStyle>
                            <CellStyle CssClass="gridcellleft" Wrap="True">
                            </CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="" VisibleIndex="7" Width="0" >
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                            <HeaderStyle HorizontalAlign="Center" />
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <DataItemTemplate>
                                <div class='floatedBtnArea'>

                                    <a href="javascript:void(0);" onclick="OnEdit('<%#Eval("Code") %>','<%#Eval("Type") %>')">
                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                                </div>
                            </DataItemTemplate>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                    </Columns>
                    <ClientSideEvents RowClick="gridRowclick" />
                    <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                    <Styles>
                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                        </Header>
                        <LoadingPanel ImageSpacing="10px">
                        </LoadingPanel>
                    </Styles>
                </dxe:ASPxGridView>
        </div>
    </div>
    </div>
    <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
    ContextTypeName="ERPDataClassesDataContext" TableName="v_hsnsac" />
</asp:Content>
