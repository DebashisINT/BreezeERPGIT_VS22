<%@ Page Title="Industry/Sectors" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_industrySector" CodeBehind="industrySector.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/javascript">
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function ClickOnMap(keyValue,KeyValueName) {

            var url = 'IndustryMap.aspx?id=' + keyValue + '&iname=' + KeyValueName;
           
            window.location.href = url;
        }
        function LastCall() {
            if (grid.cpErrorMsg != null) {
                jAlert(grid.cpErrorMsg);
                grid.cpErrorMsg = null;
            }
        }
        function gridRowclick(s, e) {
            $('#Industrygrid').find('tr').removeClass('rowActive');
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
            <h3>Industry/Sectors</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <%--   <tr>
                <td class="EHEADER" style="text-align: center">
                    <strong><span style="color: #000099">IndustrySector</span></strong></td>
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
                                            <a href="javascript:void(0);" onclick="grid.AddNewRow()" class="btn btn-success btn-radius "><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Add New</span> </a>
                                            <%} %>

                                            <% if (rights.CanExport)
                                               { %>
                                            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius " OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true"  >
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
                           <%-- <td></td>
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
                    <dxe:ASPxGridView ID="Industrygrid" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False"
                        DataSourceID="IndustrySector" KeyFieldName="ind_id" Width="100%" OnHtmlEditFormCreated="Industrygrid_HtmlEditFormCreated" OnRowDeleting="Industrygrid_RowDeleting"
                        OnHtmlRowCreated="Industrygrid_HtmlRowCreated" OnCustomCallback="Industrygrid_CustomCallback" OnCommandButtonInitialize="Industrygrid_CommandButtonInitialize"   >
                        <Templates>
                            <EditForm>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 5%"></td>
                                        <td style="width: 90%">
                                            <controls>
                                <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors">
                                </dxe:ASPxGridViewTemplateReplacement>                                                           
                            </controls>
                                            <div style="padding: 2px 2px 2px 147px">
                                                <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                    runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                    runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                            </div>
                                        </td>
                                        <td style="width: 5%"></td>
                                    </tr>
                                </table>
                            </EditForm>
                        </Templates>
                        <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" AllowFocusedRow="false"  />
                        <Styles>
                            <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
                            <Cell CssClass="gridcellleft"></Cell>
                            <FocusedRow CssClass="gridselectrow"></FocusedRow>
                            <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                            <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
                        </Styles>
                        <SettingsPager NumericButtonCount="20" PageSize="20" AlwaysShowPager="True" ShowSeparators="True">
                            <FirstPageButton Visible="True"></FirstPageButton>
                            <LastPageButton Visible="True"></LastPageButton>
                        </SettingsPager>
                        <Columns>
                            <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="0" FieldName="ind_id">
                                <EditCellStyle>
                                    <Paddings PaddingTop="15px"></Paddings>
                                </EditCellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="ind_industry" Width="40%" Caption="Industry Sector" PropertiesTextEdit-MaxLength="50">
                                <EditCellStyle Wrap="False" HorizontalAlign="Left"></EditCellStyle>
                                <PropertiesTextEdit Width="300px">
                                    <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right">
                                        <RequiredField IsRequired="True" ErrorText="Mandatory."></RequiredField>
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Left"></EditFormCaptionStyle>
                                <EditFormSettings Caption="Industry/Sector Name" Visible="True"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataComboBoxColumn FieldName="ind_principalIndustry" Caption="Under" VisibleIndex="1" >
                                <EditCellStyle Wrap="False" HorizontalAlign="Left"></EditCellStyle>

                                <PropertiesComboBox ClearButton-DisplayMode="Always" Width="300px" ValueType="System.String"  TextField="ind_industry" ValueField="ind_id" EnableIncrementalFiltering="True" EnableSynchronization="False" DataSourceID="PrincipalSelect">
                                    <Items>
                                        <dxe:ListEditItem Value="N/A" Text="N/A"></dxe:ListEditItem>
                                    </Items>
                                </PropertiesComboBox>

                                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>

                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right"></EditFormCaptionStyle>

                                <EditFormSettings Caption="Principal Sector" Visible="True"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewCommandColumn VisibleIndex="2" ShowDeleteButton="true" ShowEditButton="true" Width="6%">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                 
                                <HeaderTemplate>
                                    Actions
                                </HeaderTemplate>

                              
                            </dxe:GridViewCommandColumn>
                             <dxe:GridViewDataTextColumn VisibleIndex="2" Width="6%" CellStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                 <HeaderTemplate>
                                    Actions
                                </HeaderTemplate>
                                  <DataItemTemplate>
                                      
                                       <% if (rights.CanIndustry)
                                               { %>
                                               <a href="javascript:void(0);" onclick="ClickOnMap('<%# Eval("ind_id") %>','<%# Eval("ind_industry") %>')" title="Map other Entities" class="pad" style="text-decoration:none;">

                                                <img src="../../../assests/images/icoaccts.gif" />
                                            </a>
                                              <% } %>
                                          </div>
                                      </DataItemTemplate>
                                <%--<DeleteButton Visible="True"></DeleteButton>--%>
                                <HeaderTemplate>
                                    Map
                                </HeaderTemplate>
                                  <EditFormSettings Caption="Parent Sector" Visible="false"></EditFormSettings>
                                <%--<EditButton Visible="True"></EditButton>--%>
                                 <Settings AllowAutoFilterTextInputTimer="False" />

                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        
                        <SettingsCommandButton>
                            <%--<EditButton Text="Edit"></EditButton>
                            <DeleteButton Text="Delete"></DeleteButton>--%>

                            <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                            </EditButton>
                            <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                            </DeleteButton>
                            <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary btn-xs"></UpdateButton>
                            <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger btn-xs"></CancelButton>
                        </SettingsCommandButton>
                        <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center"
                            PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="400px" />
                        <SettingsText PopupEditFormCaption="Add/Modify Industry" />
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <Settings ShowStatusBar="Visible" ShowGroupPanel="True" ShowFilterRow="true" ShowFilterRowMenu="true" />
                          <clientsideevents endcallback="function(s, e) {	LastCall( );}" />
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>

        <asp:SqlDataSource ID="IndustrySector" runat="server" ConflictDetection="CompareAllValues"
            DeleteCommand="DELETE FROM [tbl_master_industry] WHERE [ind_id] = @original_ind_id"
            InsertCommand="INSERT INTO [tbl_master_industry] ([ind_industry], [ind_principalIndustry],[CreateDate],[CreateUser]) VALUES (@ind_industry, @ind_principalIndustry,getdate(),@CreateUser)"
            OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [ind_id], [ind_industry], [ind_principalIndustry] FROM [tbl_master_industry]"
            UpdateCommand="UPDATE [tbl_master_industry] SET [ind_industry] = @ind_industry, [ind_principalIndustry] = @ind_principalIndustry, [LastModifyDate]=getdate(), [LastModifyUser]=@CreateUser WHERE [ind_id] = @original_ind_id">
            <DeleteParameters>
                <asp:Parameter Name="original_ind_id" Type="Decimal" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="ind_industry" Type="String" />
                <asp:Parameter Name="ind_principalIndustry" Type="String" />
                <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="ind_industry" Type="String" />
                <asp:Parameter Name="ind_principalIndustry" Type="String" />
                <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" />
            </InsertParameters>
        </asp:SqlDataSource>
        <%--====================================================== PRINCIPAL SELECT =======================================================--%>
        <asp:SqlDataSource ID="PrincipalSelect" runat="server"
            SelectCommand="SELECT [ind_id], [ind_industry]  FROM  [tbl_master_industry] order by ind_industry"></asp:SqlDataSource>
        <%--==========================================================================================================================--%>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server"  Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
    <br />
</asp:Content>
