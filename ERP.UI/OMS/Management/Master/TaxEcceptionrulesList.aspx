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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
    
    <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
    ContextTypeName="ERPDataClassesDataContext" TableName="v_hsnsac" />
</asp:Content>
