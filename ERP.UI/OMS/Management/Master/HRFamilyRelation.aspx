<%@ Page Title="Family Relationship" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_HRFamilyRelation" CodeBehind="HRFamilyRelation.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function height() {
            if (document.body.scrollHeight >= 500)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '500px';
            window.frameElement.Width = document.body.scrollWidth;
        }
        function EndCall(obj) {
            if (grid.cpDelmsg != null)
                jAlert(grid.cpDelmsg);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading">
        <div class="panel-title">
            <h3>Family Relationship</h3>
        </div>

    </div>
    <div class="form_main">

        <table class="TableMain100">
            <%--    <tr> 
            <td class="EHEADER" style="text-align:center">
                <strong><span style="color: #000099">Family RelationShip</span></strong></td>
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
                                            <a href="javascript:void(0);" onclick="grid.AddNewRow()" class="btn btn-primary">
                                                <span>Add New</span>
                                            </a>
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
                            <%--<td></td>
                            <td class="gridcellright pull-right">
                                <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" Font-Bold="False" ForeColor="black" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" ValueType="System.Int32" Width="130px">
                                    <items>
                                        <dxe:ListEditItem Text="Select" Value="0" />
                                        <dxe:ListEditItem Text="PDF" Value="1" />
                                        <dxe:ListEditItem Text="XLS" Value="2" />
                                        <dxe:ListEditItem Text="RTF" Value="3" />
                                        <dxe:ListEditItem Text="CSV" Value="4" />
                                    </items>
                                    <buttonstyle>
                                    </buttonstyle>
                                    <itemstyle>
                                        <HoverStyle>
                                        </HoverStyle>
                                    </itemstyle>
                                    <border bordercolor="black" />
                                    <dropdownbutton text="Export">
                                    </dropdownbutton>
                                </dxe:ASPxComboBox>
                            </td>--%>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxGridView ID="FamilyGrid" runat="server" AutoGenerateColumns="False" DataSourceID="FamilySource"   ClientInstanceName="grid" KeyFieldName="fam_id" Width="100%" OnHtmlEditFormCreated="FamilyGrid_HtmlEditFormCreated" OnHtmlRowCreated="FamilyGrid_HtmlRowCreated" OnCustomCallback="FamilyGrid_CustomCallback" OnRowDeleting="FamilyGrid_RowDeleting"
                        OnCommandButtonInitialize="FamilyGrid_CommandButtonInitialize">
                        <SettingsSearchPanel Visible="true" Delay="6000" />
                        <clientsideevents endcallback="function(s, e) {
	  EndCall(s.cpEND);
}" />
                         <columns>
                            <dxe:GridViewDataTextColumn FieldName="fam_id" ReadOnly="True" VisibleIndex="0" Visible="False">
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="fam_familyRelationship" VisibleIndex="1" Caption="Family Relationship" Width="80%">
                                <EditFormSettings Visible="True" />

                                  <PropertiesTextEdit MaxLength="150">
                                    <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right">
                                       <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                        <RegularExpression ValidationExpression="^[^<>]+$" ErrorText="Invalid Input" />
                                 </ValidationSettings>
                                    </PropertiesTextEdit>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left" VerticalAlign="Top" Wrap="False">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="CreateDate" VisibleIndex="2" Visible="False">
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="CreateUser" VisibleIndex="3" Visible="False">
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="LastModifyDate" VisibleIndex="4" Visible="False">
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="LastModifyUser" VisibleIndex="5" Visible="False">
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewCommandColumn VisibleIndex="6" ShowDeleteButton="true" ShowEditButton="true" Width="150px" >
                     <%--        <DeleteButton Visible="True">
                </DeleteButton>
                                <EditButton Visible="True">
                </EditButton>--%>
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate >Action
                                    <%--<%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd"){ %>
                    <a href="javascript:void(0);" onclick="grid.AddNewRow()">
                      <span>Add New</span>
                    </a>
                    <%} %>--%>
                                </HeaderTemplate>
                                
                            </dxe:GridViewCommandColumn>
                        </columns>
                        <%--<SettingsContextMenu Enabled="true"></SettingsContextMenu>--%>
                        <settingscommandbutton>

                           
                            <EditButton Image-Url="/assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
<Image AlternateText="Edit" Url="/assests/images/Edit.png"></Image>
                            </EditButton>
                             <DeleteButton Image-Url="/assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete" Styles-Style-CssClass="pad">
<Image AlternateText="Delete" Url="/assests/images/Delete.png"></Image>
                            </DeleteButton>
                            <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary btn-xs">
<Styles>
<Style CssClass="btn btn-primary btn-xs"></Style>
</Styles>
                            </UpdateButton>
                            <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger btn-xs">
<Styles>
<Style CssClass="btn btn-danger btn-xs"></Style>
</Styles>
                            </CancelButton>
                        </settingscommandbutton>
                        <SettingsSearchPanel Visible="True" />
                        <settings showgrouppanel="True" showfooter="True" showstatusbar="Visible" showtitlepanel="True" showfilterrow="true" showfilterrowmenu="True" />
                        <settingsediting mode="PopupEditForm" popupeditformheight="200px" popupeditformhorizontalalign="Center"
                            popupeditformmodal="True" popupeditformverticalalign="WindowCenter" popupeditformwidth="400px" editformcolumncount="1" />
                        <styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </styles>
                        <settingstext popupeditformcaption="Add/Modify Family Relationship" confirmdelete="Confirm delete?" />
                        <settingspager numericbuttoncount="20" pagesize="20">
                        </settingspager>
                        <settingsbehavior columnresizemode="NextColumn" confirmdelete="True" />
                        <templates>
                            <EditForm>
                                <table style="width: 100%">
                                    <tr>
                                        
                                        <td style="width: 100%">
                                            <controls>
                                <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors">
                                </dxe:ASPxGridViewTemplateReplacement>                                                           
                            </controls>
                                            <div style="text-align: center; padding: 2px 2px 2px 28px">
                                                <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton" runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton" runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                            </div>
                                        </td>
                                        
                                    </tr>
                                </table>
                            </EditForm>

                        </templates>

                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="FamilySource" runat="server" ConflictDetection="CompareAllValues"
            DeleteCommand="DELETE FROM [tbl_master_familyRelationship] WHERE [fam_id] = @original_fam_id"
            InsertCommand="INSERT INTO [tbl_master_familyRelationship] ([fam_familyRelationship], [CreateDate], [CreateUser]) VALUES (@fam_familyRelationship, getdate(), @CreateUser)"
            OldValuesParameterFormatString="original_{0}"
            SelectCommand="SELECT * FROM [tbl_master_familyRelationship]"
            UpdateCommand="UPDATE [tbl_master_familyRelationship] SET [fam_familyRelationship] = @fam_familyRelationship, [LastModifyDate] = getdate(), [LastModifyUser] = @CreateUser WHERE [fam_id] = @original_fam_id">
            <DeleteParameters>
                <asp:Parameter Name="original_fam_id" Type="Decimal" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="fam_familyRelationship" Type="String" />
                <asp:Parameter Name="LastModifyDate" Type="String" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
                <asp:Parameter Name="original_fam_id" Type="Decimal" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="fam_familyRelationship" Type="String" />
                <asp:Parameter Name="CreateDate" Type="String" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
            </InsertParameters>
        </asp:SqlDataSource>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
</asp:Content>
