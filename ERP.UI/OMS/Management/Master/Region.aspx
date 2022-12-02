<%@ Page Title="Regions" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Master.management_master_Region" CodeBehind="Region.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/javascript">
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

        //function is called on changing country
        //function OnCountryChanged(cmbCountry) {
        //    grid.GetEditor("cou_country").PerformCallback(cmbCountry.GetValue().toString());
        //}
        //function ShowHideFilter(obj) {
        //    grid.PerformCallback(obj);
        //}
        function LastCall(obj) {
            if (grid.cpErrorMsg) {
                if (grid.cpErrorMsg.trim != "") {
                    jAlert(grid.cpErrorMsg);
                    grid.cpErrorMsg = '';
                    return;
                }
            }
            if (grid.cpDelmsg != null) {
                if (grid.cpDelmsg.trim() != '') {
                    jAlert(grid.cpDelmsg);
                    grid.cpDelmsg = '';
                    
                }
            }


        }
        function DeleteRow(keyValue) {

            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.PerformCallback('Delete~' + keyValue);
                }
            });
        }
        function gridRowclick(s, e) {
            $('#RegionGrid').find('tr').removeClass('rowActive');
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
            <h3>Regions</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <%--            <tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">Regions</span></strong>
                </td>
            </tr>--%>
            <tr>
                <td style="">
                    <table width="100%">
                        <tr>
                            <td style="text-align: left; vertical-align: top">
                                <table>
                                    <tr>
                                        <td id="ShowFilter">
                                         <% if (rights.CanAdd)
                                               { %>
                                            <a href="javascript:void(0);" onclick="grid.AddNewRow()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Add New</span></a><%} %>
                                          <% if (rights.CanExport)
                                               { %>
                                              <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}" >
                                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                                 <asp:ListItem Value="2">XLS</asp:ListItem>
                                                 <asp:ListItem Value="3">RTF</asp:ListItem>
                                                 <asp:ListItem Value="4">CSV</asp:ListItem>
                                            </asp:DropDownList>
                                             <% } %>
                                            <%-- <% } %>--%>
                                            <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-primary"><span>Show Filter</span></a>--%>
                                        </td>

                                       <%--..................................... Code Commented By Sam On 29092016.......................................--%>
                                        <td id="Td1">
                                            <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>
                                        </td>
                                        <%-- ..................................... Code Above Commented By Sam On 29092016.......................................--%>
                                    </tr>
                                </table>
                            </td>
                            <td></td>
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
                    </table>
                </td>
            </tr>
            <tr>
                <td class="relative">
                    <dxe:ASPxGridView ID="RegionGrid" runat="server" AutoGenerateColumns="False" DataSourceID="insertupdatedelete" 
                        KeyFieldName="regid" ClientInstanceName="grid" Width="100%" OnHtmlRowCreated="RegionGrid_HtmlRowCreated"
                        OnHtmlEditFormCreated="RegionGrid_HtmlEditFormCreated" OnCustomCallback="RegionGrid_CustomCallback" OnCommandButtonInitialize="RegionGrid_CommandButtonInitialize" SettingsBehavior-AllowFocusedRow="true">
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <Columns>

                            <dxe:GridViewDataTextColumn FieldName="reg_id" ReadOnly="True" Visible="False"
                                VisibleIndex="0">
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Country" FieldName="country" VisibleIndex="0"
                                Width="45%">
                                <EditFormSettings Visible="False" />
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Regions" FieldName="region" VisibleIndex="1"
                                Width="45%">
                                <EditFormSettings Visible="True" />
                                <PropertiesTextEdit MaxLength="50">
                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                        <RequiredField ErrorText="Mandatory" IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                </EditFormCaptionStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataComboBoxColumn FieldName="Id" Visible="False" VisibleIndex="2"
                                Caption="Country Name">
                                <PropertiesComboBox DataSourceID="SelectCountry" ValueField="cou_id" TextField="cou_country"
                                    EnableIncrementalFiltering="True" EnableSynchronization="False" ValueType="System.String">
                                    <%--<ClientSideEvents SelectedIndexChanged="function(s,e){OnCountryChanged(s);}" />--%>
                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                        <RequiredField ErrorText="Mandatory" IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesComboBox>
                                <EditFormSettings Visible="True" />
                                <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                </EditFormCaptionStyle>
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewCommandColumn ShowEditButton="true" ShowDeleteButton="false" VisibleIndex="2" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="6%">
                                
                                <HeaderTemplate>
                                    <span>Actions</span>
                                </HeaderTemplate>
                                <CustomButtons>
                                    <dxe:GridViewCommandColumnCustomButton Image-Url="../../../assests/images/Delete.png" Image-ToolTip="Delete">
                                       
                                    </dxe:GridViewCommandColumnCustomButton>

                                </CustomButtons>
                         
                            </dxe:GridViewCommandColumn>
                        </Columns>
                       <%-- <SettingsContextMenu Enabled="true"></SettingsContextMenu>--%>
                         <ClientSideEvents CustomButtonClick="function(s, e) {
                             var key = s.GetRowKey(e.visibleIndex);
                             DeleteRow(key);
                            
                            }" />
                        <SettingsCommandButton>

                            <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                            </EditButton>
                            <%--<DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                            </DeleteButton>--%>
                            
                            <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary"></UpdateButton>
                            <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger"></CancelButton>
                        </SettingsCommandButton>
                        <%--<SettingsCommandButton DeleteButton-Styles-Style-HorizontalAlign="Left" EditButton-Styles-FocusRectStyle-HorizontalAlign="Left">
                            <DeleteButton Text="Delete"></DeleteButton>
                            <EditButton Text="Edit"></EditButton>
                        </SettingsCommandButton>--%>
                      
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center"
                            PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="500px"
                            EditFormColumnCount="1" />
                       <%-- <Styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>--%>
                        <SettingsText PopupEditFormCaption="Add/Modify Region" ConfirmDelete="Confirm delete?" />
                        <SettingsPager NumericButtonCount="20" PageSize="20" AlwaysShowPager="True" ShowSeparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                        <Templates>
                            <EditForm>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 5%"></td>
                                        <td style="width: 90%">

                                            <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors"></dxe:ASPxGridViewTemplateReplacement>

                                            <div style=" padding: 2px 2px 2px 111px;">
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

                        <clientsideevents endcallback="function(s, e) {
	LastCall(s.cpHeight);
}" />
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table><%--DeleteCommand="DELETE FROM [tbl_master_regions] WHERE [reg_id] = @original_regid"
         <DeleteParameters>
                <asp:Parameter Name="original_regid" Type="Int32" />
            </DeleteParameters>--%>
        <asp:SqlDataSource ID="insertupdatedelete" runat="server" ConflictDetection="CompareAllValues"
            
            InsertCommand="INSERT INTO [tbl_master_regions] ([reg_countryId], [reg_region], [CreateDate], [CreateUser]) VALUES (@Id, @region, getdate(), @CreateUser)"
            OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT r.reg_id as regid,r.reg_region as region,c.cou_country as country,r.reg_countryId as Id FROM [tbl_master_regions] r,[tbl_master_country] c where (c.cou_id=r.reg_countryId)"
            UpdateCommand="UPDATE [tbl_master_regions] SET [reg_countryId] = @Id, [reg_region] = @region, [LastModifyDate] = getdate(), [LastModifyUser] = @CreateUser WHERE [reg_id] = @regid">
           
            <UpdateParameters>
                <asp:Parameter Name="Id" Type="Int32" />
                <asp:Parameter Name="region" Type="String" />
                <asp:Parameter Name="CreateDate" Type="String" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
                <asp:Parameter Name="regid" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <%-- <asp:Parameter Name="reg_countryId" Type="Int32" />
                <asp:Parameter Name="reg_region" Type="String" />
                <asp:Parameter Name="CreateDate" Type="String" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />--%>
                <asp:Parameter Name="Id" Type="Int32" />
                <asp:Parameter Name="region" Type="String" />
                <asp:Parameter Name="CreateDate" Type="String" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
            </InsertParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SelectRegion" runat="server" 
            SelectCommand="SELECT r.reg_id as regid,r.reg_region as region,c.cou_country as country,r.reg_countryId as Id FROM [tbl_master_regions] r,[tbl_master_country] c where (c.cou_id=r.reg_countryId) order by c.cou_country"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SelectCountry" runat="server" 
            SelectCommand="SELECT [cou_id], [cou_country] FROM [tbl_master_country] order by cou_country"></asp:SqlDataSource>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server"  Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
</asp:Content>
