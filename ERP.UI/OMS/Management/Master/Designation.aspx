<%@ Page Title="Designation" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_Designation" CodeBehind="Designation.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/javascript">
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function EndCall(obj) {
            if (grid.cpDelmsg != null)
                jAlert(grid.cpDelmsg);
            grid.cpDelmsg = null;
        }
    </script>
    <script type="text/javascript">
        function UniqueCodeCheck() {
            var proclassid = '0';
            var id = '<%= Convert.ToString(Session["Desigid"]) %>';
            var ProductClassCode = grid.GetEditor('deg_designation').GetValue();
            if ((id != null) && (id != '')) {
                proclassid = id;
                '<%=Session["Desigid"]=null %>'
            }
            var CheckUniqueCode = false;
            $.ajax({
                type: "POST",
                url: "Designation.aspx/CheckUniqueCode",
                data: JSON.stringify({ ProductClassCode: ProductClassCode, proclassid: proclassid }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    CheckUniqueCode = msg.d;
                    if (CheckUniqueCode == true) {
                        jAlert('Please enter unique Designation');
                        grid.GetEditor('deg_designation').SetValue('');
                        grid.GetEditor('deg_designation').Focus();
                    }
                }
            });
        }
        function gridRowclick(s, e) {
            $('#DesigGrid').find('tr').removeClass('rowActive');
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Designation</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <%--<tr>
                <td class="EHEADER" style="text-align: center">
                    <strong><span style="color: #000099">Designations List</span></strong></td>
            </tr>--%>
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
                                            <a href="javascript:void(0);" onclick="grid.AddNewRow();" class="btn btn-success btn-radius btn-xs"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Add New</span> </a>
                                            <%} %>
                                            <% if (rights.CanExport)
                                               { %>
                                            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius btn-xs" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
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
                            <%--<td></td>
                            <td class="gridcellright pull-right">
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
                    </table>
                </td>
            </tr>
            <tr>
                <td class="relative">
                    <dxe:ASPxGridView ID="DesigGrid" runat="server" AutoGenerateColumns="False" DataSourceID="designation"  
                        KeyFieldName="deg_id" ClientInstanceName="grid" Width="100%" OnHtmlEditFormCreated="DesigGrid_HtmlEditFormCreated"
                        OnHtmlRowCreated="DesigGrid_HtmlRowCreated" OnCustomCallback="DesigGrid_CustomCallback"
                        OnStartRowEditing="DesigGrid_StartRowEditing" OnCommandButtonInitialize="DesigGrid_CommandButtonInitialize"
                        OnRowDeleting="DesigGrid_RowDeleting">
                        <SettingsSearchPanel Visible="true" Delay="6000" />
                        <ClientSideEvents EndCallback="function(s, e) {EndCall(s.cpEND);}"></ClientSideEvents>
                        <Columns>
                            <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="0" FieldName="deg_id">
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="deg_designation" Width="80%"
                                Caption="Designation">
                                <PropertiesTextEdit Width="300px" MaxLength="50">
                                    <ValidationSettings SetFocusOnError="True" ErrorTextPosition="Right" ErrorDisplayMode="ImageWithTooltip">
                                        <RequiredField IsRequired="True" ErrorText="Mandatory"></RequiredField>
                                    </ValidationSettings>
                                    <ClientSideEvents TextChanged="function(s, e) {UniqueCodeCheck();}" Init="function (s,e) {s.Focus(); }" />
                                </PropertiesTextEdit>
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                    <Paddings PaddingTop="15px" />
                                </EditCellStyle>
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewCommandColumn VisibleIndex="1" ShowEditButton="true" ShowDeleteButton="true" HeaderStyle-HorizontalAlign="Center" Width="6%">
                                <%-- <DeleteButton Visible="True">
                                </DeleteButton>
                                <EditButton Visible="True">
                                </EditButton>--%>
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    Actions
                                    <%--<%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                      { %>
                                    <a href="javascript:void(0);" onclick="grid.AddNewRow();"><span>Add New</span> </a>
                                    <%} %>--%>
                                </HeaderTemplate>
                                
                            </dxe:GridViewCommandColumn>
                        </Columns>
                        <%--<SettingsContextMenu Enabled="true"></SettingsContextMenu>--%>
                        <SettingsCommandButton>

                            <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                            </EditButton>
                            <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                            </DeleteButton>
                            <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary btn-xs"></UpdateButton>
                            <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger btn-xs"></CancelButton>
                        </SettingsCommandButton>
                       
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true"></Settings>
                        <Styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <Cell CssClass="gridcellleft">
                            </Cell>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>
                        <SettingsText PopupEditFormCaption="Add/Modify Designation" ConfirmDelete="Confirm delete?" />
                        <SettingsPager NumericButtonCount="20" PageSize="20" AlwaysShowPager="True" ShowSeparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>




                        <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                        <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm"
                            PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True" PopupEditFormVerticalAlign="Windowcenter"
                            PopupEditFormWidth="400px" />
                        <Templates>
                            <EditForm>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 25%"></td>
                                        <td style="width: 50%">
                                            <controls>
                                <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors">
                                </dxe:ASPxGridViewTemplateReplacement>                                                           
                            </controls>
                                            <div style="text-align: left; padding: 2px 2px 2px 94px">
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
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>

        <asp:SqlDataSource ID="designation" runat="server" ConflictDetection="CompareAllValues"
            DeleteCommand="DELETE FROM [tbl_master_designation] WHERE [deg_id] = @original_deg_id"
            InsertCommand="IF NOT EXISTS (SELECT 'Y' FROM tbl_master_designation WHERE deg_designation = @deg_designation) BEGIN INSERT INTO [tbl_master_designation] ([deg_designation], [CreateDate], [CreateUser]) VALUES (@deg_designation, getdate(), @CreateUser) End"
            OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [deg_id],[deg_designation] FROM [tbl_master_designation]"
            UpdateCommand="IF NOT EXISTS (SELECT 'Y' FROM tbl_master_designation WHERE deg_designation = @deg_designation AND [deg_id] <> @original_deg_id) BEGIN UPDATE [tbl_master_designation] SET [deg_designation] = @deg_designation,[LastModifyDate]=getdate(),[LastModifyUser]=@CreateUser WHERE [deg_id] = @original_deg_id END">
            <DeleteParameters>
                <asp:Parameter Name="original_deg_id" Type="Decimal" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="deg_designation" Type="String" />
                <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="deg_designation" Type="String" />
                <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" />
            </InsertParameters>
        </asp:SqlDataSource>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
        <br />
    </div>
</asp:Content>

