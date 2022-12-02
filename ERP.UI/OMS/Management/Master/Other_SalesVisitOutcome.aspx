<%@ Page Title="Sales Visit Outcome" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Master.management_master_Other_SalesVisitOutcome" CodeBehind="Other_SalesVisitOutcome.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <script type="text/javascript">

        //function is called on changing country
        function OnDescriptionChanged(cmbDescription) {
            grid.GetEditor("Description").PerformCallback(cmbDescription.GetValue().toString());
        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }

        function ActiveCall(s, e, i) {
            grid.PerformCallback(i + '~' + s.GetValue());

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Sales Visit Outcome</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <%--<tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">Sales Visit OutComes</span></strong></td>
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
                                            <a href="javascript:void(0);" onclick="grid.AddNewRow()" class="btn btn-success btn-radius btn-xs"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Add New</span></a>
                                            <%} %>
                                            <% if (rights.CanExport)
                                               { %>
                                             <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-xs" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true"  >
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
                           <%-- <td></td>
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
        </table>
        <dxe:ASPxGridView ID="OutComeGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid" DataSourceID="SalesVisitOutcomes"    KeyFieldName="ID" Width="100%" OnHtmlEditFormCreated="OutComeGrid_HtmlEditFormCreated" OnHtmlRowCreated="OutComeGrid_HtmlRowCreated" 
            OnCommandButtonInitialize="OutComeGrid_CommandButtonInitialize" OnCustomCallback="OutComeGrid_CustomCallback1" OnStartRowEditing="OutComeGrid_StartRowEditing">
            <SettingsSearchPanel Visible="True" Delay="5000" />
            <columns>
                <dxe:GridViewDataTextColumn FieldName="ID" ReadOnly="True" Visible="False" VisibleIndex="0">
                    <EditFormSettings Visible="False" />
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Sales Visit Outcome" FieldName="Outcome" VisibleIndex="0" Width="40%">
                    <EditFormSettings Caption="Sales Visit Outcome" Visible="True" />
                    <PropertiesTextEdit MaxLength="200">
                        <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" >
                            <RequiredField IsRequired="True" ErrorText="Mandatory." />

                        </ValidationSettings>
                    </PropertiesTextEdit>
                    <CellStyle CssClass="gridcellleft">
                    </CellStyle>
                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top">
                    </EditFormCaptionStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="Category" Visible="False" VisibleIndex="1">
                    <EditFormSettings Visible="False" />
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Sales Visit Outcome Category" FieldName="description"
                    VisibleIndex="1" Width="40%">
                    <EditFormSettings Caption="Sales Visit Outcome Category" Visible="False" />
                    <CellStyle CssClass="gridcellleft">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataComboBoxColumn FieldName="Category" Visible="False" VisibleIndex="1" Caption="Sales Visit Outcome Category">
                    <PropertiesComboBox DataSourceID="CategorySelect" ValueField="Int_id" TextField="Description" EnableIncrementalFiltering="True" EnableSynchronization="False" ValueType="System.String">
                        <ClientSideEvents SelectedIndexChanged="function(s,e){OnDescriptionChanged(s);}" />
                        <ValidationSettings SetFocusOnError="True" CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right">
                            <RequiredField ErrorText="Mandatory." IsRequired="True" />
                        </ValidationSettings>
                    </PropertiesComboBox>
                    <EditFormSettings Visible="True" Caption="Sales Visit Outcome Category" />
                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top">
                    </EditFormCaptionStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataComboBoxColumn>
                <dxe:GridViewDataCheckColumn Caption="Active"  FieldName="IsActive" VisibleIndex="2" Width="70px" Settings-AllowAutoFilter="False">
                                <DataItemTemplate>

                                      <dxe:ASPxCheckBox ID="chkActive" ClientInstanceName="CchkActive" Checked='<%# GetChecked(Eval("IsActive").ToString()) %>' runat="server" OnInit="chkActivateBatch_Init" >
                                      
                                    </dxe:ASPxCheckBox>
                                 
                                    
                                </DataItemTemplate>
                                   <CellStyle HorizontalAlign="Center"   >
                                                                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                                                                  
                           </dxe:GridViewDataCheckColumn>
                <dxe:GridViewCommandColumn VisibleIndex="4" ShowEditButton="true" ShowDeleteButton="true" Width="6%">
                    <HeaderStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        Actions
                        <%--<a href="javascript:void(0);" onclick="grid.AddNewRow()">
                            <span>Add New</span>
                        </a>--%>
                    </HeaderTemplate>
                
                </dxe:GridViewCommandColumn>
            </columns>
            <%--<SettingsContextMenu Enabled="true"></SettingsContextMenu>--%>
            <settingscommandbutton>
              
                <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                </EditButton>
                  <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                </DeleteButton>
                <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary btn-xs"></UpdateButton>
                <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger btn-xs"></CancelButton>
            </settingscommandbutton>
           
            <settings showgrouppanel="True" showstatusbar="Visible" showfilterrow="true" showfilterrowmenu="true" />
            <settingsediting mode="PopupEditForm"  popupeditformhorizontalalign="Center"
                popupeditformmodal="True" popupeditformverticalalign="WindowCenter" popupeditformwidth="450px" popupeditformheight="220" editformcolumncount="1" />
            <styles>
                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                </Header>
                <LoadingPanel ImageSpacing="10px">
                </LoadingPanel>
            </styles>
            <settingstext popupeditformcaption="Add/Modify Sales Visit Outcome" confirmdelete="Confirm delete?" />
            <settingspager numericbuttoncount="20" pagesize="20" showseparators="True">
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
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
                                <div style="text-align: center; padding: 2px 2px 2px 84px">
                                    <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton" runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                    <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton" runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                </div>
                            </td>
                            
                        </tr>
                    </table>
                </EditForm>
            </templates>
        </dxe:ASPxGridView>
        <asp:SqlDataSource ID="CategorySelect" runat="server"
            SelectCommand="SELECT [Int_id], [Description] FROM [tbl_Master_SalesVisitOutcomeCategory] order by Description"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SalesVisitOutcomes" runat="server"
            SelectCommand="SELECT sv.slv_Id AS ID, sv.IsActive AS IsActive,sv.slv_SalesVisitOutcome AS Outcome, sv.slv_Category AS Category, s.Description AS description FROM tbl_master_SalesVisitOutCome AS sv INNER JOIN tbl_Master_SalesVisitOutcomeCategory AS s ON sv.slv_Category = s.Int_id" ConflictDetection="CompareAllValues"
            DeleteCommand="DELETE FROM [tbl_master_SalesVisitOutCome] WHERE [slv_Id] = @original_ID"
            InsertCommand="INSERT INTO [tbl_master_SalesVisitOutCome] ([slv_SalesVisitOutcome], [slv_Category], [CreateDate], [CreateUser],[IsActive]) VALUES (@Outcome, @Category, getdate(), @CreateUser,@IsActive)" OldValuesParameterFormatString="original_{0}"
            UpdateCommand="UPDATE [tbl_master_SalesVisitOutCome] SET [slv_SalesVisitOutcome] = @Outcome, [slv_Category] = @Category, [CreateDate] = getdate(), [CreateUser] = @CreateUser WHERE [slv_Id] = @ID">
            <DeleteParameters>
                <asp:Parameter Name="original_ID" Type="Decimal" />

            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="Outcome" Type="String" />
                <asp:Parameter Name="Category" Type="Decimal" />
                <asp:Parameter Name="CreateDate" Type="String" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
                <asp:Parameter Name="ID" Type="Decimal" />

            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="Outcome" Type="String" />
                <asp:Parameter Name="Category" Type="Decimal" />
                <asp:Parameter Name="CreateDate" Type="String" />
                  <asp:Parameter Name="IsActive" Type="Boolean"/>
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />

            </InsertParameters>
        </asp:SqlDataSource>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
</asp:Content>
