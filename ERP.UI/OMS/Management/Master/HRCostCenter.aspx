<%@ Page Title="Cost Centers/Departments" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_HRCostCenter" CodeBehind="HRCostCenter.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    


    <script language="javascript" type="text/javascript">
        //function SignOff() {
        //    window.parent.SignOff();
        //}
        //function height() {
        //    if (document.body.scrollHeight >= 500)
        //        window.frameElement.height = document.body.scrollHeight;
        //    else
        //        window.frameElement.height = '500px';
        //    window.frameElement.Width = document.body.scrollWidth;
        //}
        function ClickOnMoreInfo(keyValue) {
            var url = 'HRCost.aspx?id=' + keyValue;
            //OnMoreInfoClick(url, "Modify Cost Center", '940px', '450px', "Y");
            window.location.href = url;

        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function AddNewClick() {
            var url = "HRAddNewCostDept.aspx"
            //OnMoreInfoClick(url, "Modify Cost Center", '940px', '450px', "Y");
            window.location.href = url;
        }
        function gridcrmCampaignclick(s, e) {
            //alert('hi');
            $('#CostDepartmentGrid').find('tr').removeClass('rowActive');
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
                        console.log(value);
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Cost Centers/Departments</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100" width="100%">
            <%--<tr>
                <td class="EHEADER" style="text-align: center" colspan="2">
                    <strong><span style="color: #000099">Cost Centers/Departments</span></strong>
                </td>
            </tr>--%>
            <tr>
                <td style="text-align: left; vertical-align: top; width: 536px;">
                    <table>
                        <tr>
                            <td id="ShowFilter">
                                <%-- <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                  { %>--%>
                                <% if (rights.CanAdd)
                                   { %>
                                <a href="javascript:void(0);" onclick="AddNewClick();" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span>Add New </a>
                                <%} %>
                                <%-- <a href="javascript:ShowHideFilter('s');" class="btn btn-primary"><span>Show Filter</span></a>--%>
                              <% if (rights.CanExport)
                                               { %>
                                  <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true"  >
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                        <asp:ListItem Value="2">XLS</asp:ListItem>
                                        <asp:ListItem Value="3">RTF</asp:ListItem>
                                        <asp:ListItem Value="4">CSV</asp:ListItem>
                                </asp:DropDownList>
                                <% } %>
                            </td>
                            <td id="Td1">
                                <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%> <%--ag18102016 ---0011370--%>
                            </td>
                        </tr>
                    </table>
                </td>
                <%--<td class="gridcellright" style="float: right">
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
                        <Border BorderColor="black" />
                        <DropDownButton Text="Export">
                        </DropDownButton>
                    </dxe:ASPxComboBox>
                </td>--%>
            </tr>
            <tr>
                <%--ag18102016 ---0011370--%>
                <td colspan="2" class="relative">
                    <dxe:ASPxGridView ID="CostDepartmentGrid" runat="server" ClientInstanceName="grid"
                        AutoGenerateColumns="False" DataSourceID="DepartmentSource" KeyFieldName="cost_id" 
                        Width="100%" OnCustomCallback="CostDepartmentGrid_CustomCallback" OnRowUpdated="CostDepartmentGrid_RowUpdated" OnRowCommand="CostDepartmentGrid_RowCommand" SettingsBehavior-AllowFixedGroups="true"  SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"  >
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="cost_id" ReadOnly="True" VisibleIndex="0"
                                Visible="False">
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataComboBoxColumn Caption="Cost Center/Department Type" FieldName="cost_costCenterType"
                                VisibleIndex="1" Width="50%">
                                <PropertiesComboBox ValueType="System.String" EnableIncrementalFiltering="True">
                                    <Items>
                                        <dxe:ListEditItem Text="Department" Value="Department"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Employee" Value="Employee"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Branch" Value="Branch"></dxe:ListEditItem>
                                    </Items>
                                </PropertiesComboBox>
                                <EditFormSettings Visible="True" />
                                <CellStyle CssClass="gridcellleft" HorizontalAlign="Left">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataTextColumn Caption="Cost Centers/Departments" FieldName="cost_description" VisibleIndex="0"
                                Width="50%">
                                <EditFormSettings Visible="True" />
                                <CellStyle CssClass="gridcellleft" HorizontalAlign="Left">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataComboBoxColumn FieldName="cost_costCenterHead" Visible="False"
                                VisibleIndex="2">
                                <PropertiesComboBox DataSourceID="HeadSource" TextField="cnt_firstName" ValueField="cnt_internalId"
                                    ValueType="System.String" EnableIncrementalFiltering="True">
                                </PropertiesComboBox>
                                <EditFormSettings Visible="True" Caption="Head Of department" />
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataComboBoxColumn FieldName="cost_principlalDepartment" Visible="False"
                                VisibleIndex="3">
                                <PropertiesComboBox DataSourceID="ParentSource" TextField="cost_description" ValueField="cost_id"
                                    ValueType="System.String" EnableIncrementalFiltering="True">
                                </PropertiesComboBox>
                                <EditFormSettings Visible="True" Caption="Parent Cost  Center/Dept." />
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataCheckColumn FieldName="cost_operationIn" Visible="False" VisibleIndex="5">
                                <EditFormSettings Visible="True" Caption="Mutual Fund" />
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataCheckColumn>
                            <dxe:GridViewDataCheckColumn FieldName="cost_operationIn1" Visible="False" VisibleIndex="6">
                                <EditFormSettings Visible="True" Caption="Broking" />
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataCheckColumn>
                            <dxe:GridViewDataCheckColumn FieldName="cost_operationIn2" Visible="False" VisibleIndex="7">
                                <EditFormSettings Visible="True" Caption="Insurance" />
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataCheckColumn>
                            <dxe:GridViewDataCheckColumn FieldName="cost_operationIn3" Visible="False" VisibleIndex="8">
                                <EditFormSettings Visible="True" Caption="Depository" />
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataCheckColumn>
                            <dxe:GridViewDataTextColumn FieldName="cost_email" Visible="False" VisibleIndex="4">
                                <EditFormSettings Visible="True" Caption="Email-Id" />
                                <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Details" VisibleIndex="2" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="0">
                                <DataItemTemplate>
                                    <div class='floatedBtnArea'>
                                    <% if (rights.CanEdit)
                                               { %>
                                    <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Container.KeyValue %>')" title="" class="pad" style="text-decoration: none;">
                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span>
                                    </a>
                                     <% } %>
                                      <% if (rights.CanDelete)
                                               { %>
                                    <asp:LinkButton ID="btn_delete" runat="server" OnClientClick="return confirm('Confirm Delete?');" CommandArgument='<%# Container.KeyValue %>' CommandName="delete" ToolTip="" Font-Underline="false">
                                        <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span>
                                    </asp:LinkButton>
                                     <% } %>
                                        </div>
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    <span>Actions</span>
                                </HeaderTemplate>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <%--<dxe:GridViewCommandColumn VisibleIndex="3" ShowDeleteButton="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="5%">
                                <HeaderTemplate>
                                    <span>Actions</span>
                                </HeaderTemplate>
                            </dxe:GridViewCommandColumn>--%>
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
                        <%--<Styles>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                        </Styles>--%>
                    
                        <Settings ShowTitlePanel="false" ShowFooter="false" ShowStatusBar="Hidden" ShowFilterRow="true" ShowGroupPanel="true" ShowFilterRowMenu ="true" />
                        <%--<SettingsCommandButton>
                            <EditButton Text="Edit"></EditButton>
                            <DeleteButton ButtonType="Image" Image-Url="../../../assests/images/Delete.png"></DeleteButton>

                        </SettingsCommandButton>--%>
                        <ClientSideEvents RowClick="gridcrmCampaignclick" />
                        <SettingsText PopupEditFormCaption="Add/ Modify CostCenter" />
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True"
                            PopupEditFormVerticalAlign="TopSides" PopupEditFormWidth="900px" EditFormColumnCount="1" />
                        <SettingsBehavior AllowFocusedRow="false" ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                        <%--<SettingsPager AlwaysShowPager="True" PageSize="20" ShowSeparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>--%>
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>
        <asp:SqlDataSource ID="DepartmentSource" runat="server"
            SelectCommand="HrCostCenterSelect" DeleteCommand="delete from tbl_master_costCenter where cost_id=@cost_id"
            SelectCommandType="StoredProcedure" InsertCommand="HrCostCenterInsert" InsertCommandType="StoredProcedure"
            UpdateCommand="HrCostCenterUpdate" UpdateCommandType="StoredProcedure">
            <InsertParameters>
                <asp:Parameter Name="cost_costCenterType" Type="String" />
                <asp:Parameter Name="cost_description" Type="String" />
                <asp:Parameter Name="cost_costCenterHead" Type="String" />
                <asp:Parameter Name="cost_principlalDepartment" Type="String" />
                <asp:Parameter Name="cost_operationIn" Type="String" />
                <asp:Parameter Name="cost_operationIn1" Type="String" />
                <asp:Parameter Name="cost_operationIn2" Type="String" />
                <asp:Parameter Name="cost_operationIn3" Type="String" />
                <asp:Parameter Name="cost_email" Type="String" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="String" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="cost_id" Type="String" />
                <asp:Parameter Name="cost_costCenterType" Type="String" />
                <asp:Parameter Name="cost_description" Type="String" />
                <asp:Parameter Name="cost_costCenterHead" Type="String" />
                <asp:Parameter Name="cost_principlalDepartment" Type="String" />
                <asp:Parameter Name="cost_operationIn" Type="String" />
                <asp:Parameter Name="cost_operationIn1" Type="String" />
                <asp:Parameter Name="cost_operationIn2" Type="String" />
                <asp:Parameter Name="cost_operationIn3" Type="String" />
                <asp:Parameter Name="cost_email" Type="String" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="String" />
            </UpdateParameters>
            <DeleteParameters>
                <asp:Parameter Name="cost_id" Type="String" />
            </DeleteParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="ParentSource" runat="server"  SelectCommand="select cost_description,cost_id from tbl_master_costCenter where cost_costCenterType='Department' order by cost_description"></asp:SqlDataSource>
        <asp:SqlDataSource ID="HeadSource" runat="server" SelectCommand="select cnt_firstName+' '+isnull(cnt_middleName,'')+' '+isnull(cnt_lastName,'')+' ['+isnull(rtrim(cnt_shortName),'')+']' as cnt_firstName, cnt_internalId from tbl_master_contact where cnt_internalId LIKE '%em_%' order by cnt_firstName"></asp:SqlDataSource>
        <br />
    </div>
</asp:Content>
