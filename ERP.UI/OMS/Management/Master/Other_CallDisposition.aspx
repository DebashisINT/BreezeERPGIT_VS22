<%@ Page Title="Call Dispositions" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Master.management_master_Other_CallDisposition" CodeBehind="Other_CallDisposition.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        //function is called on changing country
        function OnDescriptionChanged(cmbDescription) {
            grid.GetEditor("Description").PerformCallback(cmbDescription.GetValue().toString());
        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        // Code  Added  By Priti on 14122016 to  stop the autopostback for Export To Text
        function showFilter() {
            var str = $('#drdExport').find('option:selected').text();
            if (str == 'Export to') {
                return false;
            }
            return true;
        }
        //................end.....................

        function ActiveCall(s,e,i)
        {
            grid.PerformCallback(i+'~'+s.GetValue());

        }

        function addNewRecord()
        {
            grid.AddNewRow();
            grid.GetEditor('IsActive').SetChecked(true);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Call Dispositions</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <%--<tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">Call Dispositions</span></strong></td>
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
                                            <a href="javascript:void(0);" onclick="addNewRecord()" class="btn btn-success btn-radius btn-xs">
                                                <span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Add New</span>
                                            </a>
                                            <%} %>

                                            <% if (rights.CanExport)
                                               { %>
                                             <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius btn-xs" onChange="if(!showFilter()){return false;}"
                                                 OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true"  >
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
                <td class="relative">
                    <dxe:ASPxGridView ID="CallDispositionGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid" DataSourceID="CallDisposition"      KeyFieldName="ID" Width="100%" OnHtmlEditFormCreated="CallDispositionGrid_HtmlEditFormCreated" OnHtmlRowCreated="CallDispositionGrid_HtmlRowCreated" 
                        OnCommandButtonInitialize="CallDispositionGrid_CommandButtonInitialize" OnCustomCallback="CallDispositionGrid_CustomCallback1" >
                        <SettingsSearchPanel Visible="True" Delay="5000" />

                        <columns>
                            <dxe:GridViewDataTextColumn FieldName="ID" ReadOnly="True" Visible="False" VisibleIndex="0">
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Call Dispositions" FieldName="Dispositions"
                                VisibleIndex="0" Width="40%">
                                <EditFormSettings Visible="True" />
                                <PropertiesTextEdit MaxLength="100">
                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">
                                        <RequiredField ErrorText="Mandatory." IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Call Dispositions Category" FieldName="Description"
                                VisibleIndex="1" Width="40%">
                                <EditFormSettings Visible="False" />
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                              <dxe:GridViewDataCheckColumn Caption="Active" FieldName="IsActive" VisibleIndex="4" Width="70px" Settings-AllowAutoFilter="False" HeaderStyle-HorizontalAlign="Center">
                                <DataItemTemplate>

                                      <dxe:ASPxCheckBox ID="chkActive" ClientInstanceName="CchkActive" Checked='<%# GetChecked(Eval("IsActive").ToString()) %>' runat="server" OnInit="chkActivateBatch_Init" >
                                      
                                    </dxe:ASPxCheckBox>
                                    <%--<dxe:ASPxCheckBox ID="chkActive" ClientInstanceName="CchkActive" runat="server" Checked='<%# GetChecked(Eval("IsActive").ToString()) %>' 
                                    ClientSideEvents-CheckedChanged="function(s, e) { grid.PerformCallback(s.GetChecked()+'^'+'serial'); }>
                                          </dxe:ASPxCheckBox>--%>
                                    
                                </DataItemTemplate>
                                   <CellStyle HorizontalAlign="Center"   >
                                                                    </CellStyle>
                                  <Settings AllowAutoFilterTextInputTimer="False" />
                                                                  
                           </dxe:GridViewDataCheckColumn>
                            <dxe:GridViewDataComboBoxColumn FieldName="Call_Category" Visible="False" VisibleIndex="1" Caption="Disposition Category">
                                <PropertiesComboBox DataSourceID="DispositionSelect" ValueField="Int_id" TextField="Description"
                                    EnableIncrementalFiltering="True" EnableSynchronization="False" ValueType="System.String">
                                    <ClientSideEvents SelectedIndexChanged="function(s,e){OnDescriptionChanged(s);}" />

                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">
                                        <RequiredField ErrorText="Mandatory." IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesComboBox>
                                <EditFormSettings Visible="True" Caption="Disposition Category" />
                                <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewCommandColumn VisibleIndex="2" ShowEditButton="true" ShowCancelButton="true" ShowDeleteButton="true" Width="6%" Visible="false">
                                <%-- <EditButton Visible="True">
                </EditButton>
                <DeleteButton Visible="True">
                </DeleteButton>--%>
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    Actions
                                    <%--<%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                      { %>
                                    <a href="javascript:void(0);" onclick="grid.AddNewRow()">
                                        <span>Add New</span>
                                    </a>
                                    <%} %>--%>
                                </HeaderTemplate>
                                
                            </dxe:GridViewCommandColumn>
                        </columns>
                        
                        <settingscommandbutton>
                            <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                            </DeleteButton>
                            <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit">
                            </EditButton>
                            <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary btn-xs"></UpdateButton>
                            <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger btn-xs"></CancelButton>
                        </settingscommandbutton>
                        <SettingsSearchPanel Visible="True" />
                        <settings showgrouppanel="True" showstatusbar="Visible" showfilterrow="true" showfilterrowmenu="true" />
                        <settingsediting mode="PopupEditForm"  popupeditformhorizontalalign="Center"
                            popupeditformmodal="True" popupeditformverticalalign="WindowCenter" popupeditformwidth="380px" popupeditformheight="220px" editformcolumncount="1" />
                        <styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </styles>
                        <settingstext popupeditformcaption="Add/Modify CallDisposition" confirmdelete="Confirm delete?" />
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
                                            <div style="padding: 2px 2px 2px 141px">
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
        <asp:SqlDataSource ID="DispositionSelect" runat="server" 
            SelectCommand="SELECT Int_id = CAST((CAST(Int_id as INT)) as nvarchar(max)), [Description] FROM [tbl_Master_DispositionCategory] order by Description"></asp:SqlDataSource>
        <asp:SqlDataSource ID="CallDisposition" runat="server"
            SelectCommand="SELECT cd.call_id AS ID, cd.IsActive AS IsActive,cd.call_dispositions AS Dispositions, d.Description as Description, cd.Call_Category FROM tbl_master_calldispositions AS cd INNER JOIN tbl_Master_DispositionCategory AS d ON cd.Call_Category = d.Int_id" ConflictDetection="CompareAllValues"
            DeleteCommand="DELETE FROM [tbl_master_calldispositions] WHERE [call_id] = @original_ID"
            InsertCommand="INSERT INTO [tbl_master_calldispositions] ([call_dispositions], [Call_Category], [CreateDate], [CreateUser],[IsActive]) VALUES (@Dispositions, @Call_Category, getdate(), @CreateUser,@IsActive)" OldValuesParameterFormatString="original_{0}"
            UpdateCommand="UPDATE [tbl_master_calldispositions] SET [call_dispositions] = @Dispositions, [Call_Category] = @Call_Category, [CreateDate] = getdate(), [CreateUser] = @CreateUser WHERE [call_id] = @ID">
            <DeleteParameters>
                <asp:Parameter Name="original_ID" Type="Decimal" />

            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="Dispositions" Type="String" />
                <asp:Parameter Name="Call_Category" Type="Decimal" />
                <asp:Parameter Name="CreateDate" Type="String" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
                <asp:Parameter Name="ID" Type="Decimal" />

            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="Dispositions" Type="String" />
                <asp:Parameter Name="Call_Category" Type="Decimal" />
                <asp:Parameter Name="CreateDate" Type="String" />
                 <asp:Parameter Name="IsActive" Type="Boolean"/>
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />

            </InsertParameters>
        </asp:SqlDataSource>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
</asp:Content>
