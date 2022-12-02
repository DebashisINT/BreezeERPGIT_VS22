<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="Schema_master.aspx.cs" Inherits="ERP.OMS.Management.Master.Schema_master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function ShowError(obj) {

            if (SchemaGrid.cpMsg != null) {
                jAlert(SchemaGrid.cpMsg);
                SchemaGrid.cpMsg = null;
            }
        }
        function saveallsuccess() {
            jAlert("Saved Succesfully");
            //SchemaGrid.PerformCallback();
            window.location.reload(true);
        }

        
        function InitProcessed(s,e)
        {
            //debugger;
            //if (s.GetValue() == true) {
            //    cbCheck_CI1.SetChecked = false;
            //}
            //else if (s.GetValue() == false) {
            //    cbCheck_CI1.SetChecked = true;
            //}
        }
        function DeleteRow(keyValue) {

            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {

                    SchemaGrid.PerformCallback('Delete~' + keyValue);
                }
            });
        }
        window.CallParent = function () {
            SchemaGrid.PerformCallback();
        }

        function OnEdit(obj) {
            Action = 'edit';
            Status = obj;
            //alert(obj);
            var frm = 'Schemapopup.aspx?Schemaid=' + obj;
            window.location.href = frm;
            //editwin = dhtmlmodal.open("Editbox", "iframe", frm, "Edit Schema", "width=950px,height=500px,center=1,resize=1,scrolling=2,top=500", "recal");

            //cPopup_Schema.SetContentUrl(frm);
            //cPopup_Schema.Show();
        }
        function OnAdd() {
            Action = 'Add';
            var frm = 'Schemapopup.aspx';
            //editwin = dhtmlmodal.open("Editbox", "iframe", frm, "Add Schema", "width=950px,height=500px,center=1,resize=1,scrolling=2,top=500", "recal");
            cPopup_Schema.SetContentUrl("Schemapopup.aspx");
            cPopup_Schema.Show();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dxe:ASPxPopupControl ID="Popup_Schema" runat="server"
        CloseAction="CloseButton" Top="50" Left="600" ClientInstanceName="cPopup_Schema" Height="550px"
        Width="600px" HeaderText="Add/Modify Scheme" Modal="true" AllowResize="true" ResizingMode="Postponed" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <contentcollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </contentcollection>
    </dxe:ASPxPopupControl>
    <div class="panel-heading">
        <div class="panel-title" id="td_contact1" runat="server">
            <h3>
                <asp:Label ID="lblHeadTitle" runat="server" Text="Numbering Scheme "></asp:Label>
            </h3>
        </div>

    </div>


    <div class="form_main">
        <div class="SearchArea">
            <div class="FilterSide">
                <div style="float: left; padding-right: 5px;">
                    <% if (rights.CanAdd)
                       { %>
                    <a href="javascript:void(0);" onclick="javascript:OnAdd();" class="btn btn-primary"><span>Add New</span> </a>

                    <% } %>
                </div>

                <div class="pull-left">
                      <% if (rights.CanExport)
                                               { %>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnChange="if(!AvailableExportOption()){return false;}" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>

                    </asp:DropDownList>
                      <% } %>
                </div>
            </div>

        </div>
        <dxe:ASPxGridView ID="SchemaGrid" runat="server" KeyFieldName="schemaID" AutoGenerateColumns="False"
            Width="100%" ClientInstanceName="SchemaGrid" OnCustomCallback="SchemaGrid_CustomCallback" SettingsBehavior-AllowFocusedRow="true" 
            >
            <%-- <clientsideevents RowClick="FocusedRowChamnged"/>--%>
            <clientsideevents endcallback="function(s,e) { ShowError(s.cpInsertError); }" />
          <%--  <settingspager numericbuttoncount="10" showseparators="True" alwaysshowpager="True" pagesize="10">
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </settingspager>--%>
              <settingspager numericbuttoncount="10" showseparators="True" alwaysshowpager="True"  pagesize="10">
                             <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200"/>
                              
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </settingspager>

            <columns>
                <dxe:GridViewDataTextColumn FieldName="schtypeName" Caption="Type" VisibleIndex="0" Visible="false" Width="10%">
                    <EditFormSettings Visible="False" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataComboBoxColumn FieldName="schematype_id" Caption="Type" VisibleIndex="0" Visible="true">
                    <PropertiesComboBox DataSourceID="SqlSchematype" TextField="type_name"  ValueField="type_Id" ValueType="System.Int32" EnableSynchronization="False" EnableIncrementalFiltering="True">
                    </PropertiesComboBox>
                    <EditFormSettings Visible="True" VisibleIndex="0" />
                    <EditCellStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                    </EditCellStyle>
                </dxe:GridViewDataComboBoxColumn>

                <dxe:GridViewDataTextColumn FieldName="SchemaName" Caption="Scheme Name" VisibleIndex="1">
                    <EditFormSettings Visible="True" VisibleIndex="1" />
                    <EditCellStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                    </EditCellStyle>
                    <Settings AutoFilterCondition="Contains" />
                    <PropertiesTextEdit MaxLength="80">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right">
                            <RequiredField ErrorText="Mandatory" IsRequired="true" />
                        </ValidationSettings>
                    </PropertiesTextEdit>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="schema_typechar" Caption="Scheme type" VisibleIndex="2" Visible="false">
                    <EditFormSettings Visible="false" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataComboBoxColumn FieldName="schema_type" Caption="Scheme type" VisibleIndex="2"
                    Visible="false">
                    <EditFormSettings Visible="True" VisibleIndex="2" />
                    <EditCellStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                    </EditCellStyle>
                    <PropertiesComboBox
                        ValueType="System.String" EnableSynchronization="False" EnableIncrementalFiltering="True">
                        <Items>
                            <dxe:ListEditItem Text="Manual" Value="0"></dxe:ListEditItem>
                            <dxe:ListEditItem Text="Auto" Value="1"></dxe:ListEditItem>
                        </Items>
                        <%--<ClientSideEvents SelectedIndexChanged="function(s, e) {
	                                            var value = s.GetText().toUpperCase();	
                                                if(value == &quot;AUTO&quot;)
                                                { $('#GridContactPerson_DXEFL_6').attr('style','visible:true');
                                                     SchemaGrid.GetEditor(&quot;digit&quot;).SetEnabled(true);
                                                     SchemaGrid.GetEditor(&quot;start&quot;).SetEnabled(true);
                                                     SchemaGrid.GetEditor(&quot;prefix_with&quot;).SetEnabled(true);
                                                }
                                                else
                                                {$('#GridContactPerson_DXEFL_6').attr('style','visible:false');
                                                    SchemaGrid.GetEditor(&quot;digit&quot;).SetEnabled(false);
                                                     SchemaGrid.GetEditor(&quot;start&quot;).SetEnabled(false);
                                                     SchemaGrid.GetEditor(&quot;prefix_with&quot;).SetEnabled(false);
                                                }
                                            }"
                            Init="function(s, e) {
	                                            var value = s.GetText().toUpperCase();
                                                if(value == &quot;AUTO&quot;)
                                                {$('#GridContactPerson_DXEFL_6').attr('style','visible:true');
                                                      SchemaGrid.GetEditor(&quot;digit&quot;).SetEnabled(true);
                                                     SchemaGrid.GetEditor(&quot;start&quot;).SetEnabled(true);
                                                     SchemaGrid.GetEditor(&quot;prefix_with&quot;).SetEnabled(true);
                                                }
                                                else
                                                {$('#GridContactPerson_DXEFL_6').attr('style','visible:false');
                                                     SchemaGrid.GetEditor(&quot;digit&quot;).SetEnabled(false);
                                                     SchemaGrid.GetEditor(&quot;start&quot;).SetEnabled(false);
                                                     SchemaGrid.GetEditor(&quot;prefix_with&quot;).SetEnabled(false);
                                                }
                                            }" />--%>
                    </PropertiesComboBox>
                </dxe:GridViewDataComboBoxColumn>

                <dxe:GridViewDataComboBoxColumn FieldName="lenght" Caption="Length"
                    VisibleIndex="3" Visible="true">
                    <PropertiesComboBox
                        ValueField="lenght" ValueType="System.String" EnableSynchronization="False" EnableIncrementalFiltering="True">
                        <Items>
                            <dxe:ListEditItem Text="10" Value="10"></dxe:ListEditItem>
                            <dxe:ListEditItem Text="11" Value="11"></dxe:ListEditItem>
                            <dxe:ListEditItem Text="12" Value="12"></dxe:ListEditItem>
                            <dxe:ListEditItem Text="13" Value="13"></dxe:ListEditItem>
                            <dxe:ListEditItem Text="14" Value="14"></dxe:ListEditItem>
                            <dxe:ListEditItem Text="15" Value="15"></dxe:ListEditItem>
                            <dxe:ListEditItem Text="16" Value="16"></dxe:ListEditItem>
                           <%-- <dxe:ListEditItem Text="17" Value="17"></dxe:ListEditItem>
                            <dxe:ListEditItem Text="18" Value="18"></dxe:ListEditItem>
                            <dxe:ListEditItem Text="19" Value="19"></dxe:ListEditItem>
                            <dxe:ListEditItem Text="20" Value="20"></dxe:ListEditItem>
                            <dxe:ListEditItem Text="21" Value="21"></dxe:ListEditItem>
                            <dxe:ListEditItem Text="22" Value="22"></dxe:ListEditItem>
                            <dxe:ListEditItem Text="23" Value="23"></dxe:ListEditItem>
                            <dxe:ListEditItem Text="24" Value="24"></dxe:ListEditItem>
                            <dxe:ListEditItem Text="25" Value="25"></dxe:ListEditItem>
                            <dxe:ListEditItem Text="26" Value="26"></dxe:ListEditItem>
                            <dxe:ListEditItem Text="27" Value="27"></dxe:ListEditItem>
                            <dxe:ListEditItem Text="28" Value="28"></dxe:ListEditItem>
                            <dxe:ListEditItem Text="29" Value="29"></dxe:ListEditItem>
                            <dxe:ListEditItem Text="30" Value="30"></dxe:ListEditItem>--%>
                        </Items>
                    </PropertiesComboBox>
                    <EditFormSettings Visible="True" VisibleIndex="3" />
                    <EditCellStyle HorizontalAlign="Right">
                    </EditCellStyle>
                </dxe:GridViewDataComboBoxColumn>

                <dxe:GridViewDataTextColumn FieldName="prefix" Caption="Prefix" VisibleIndex="4">
                    <EditFormSettings Visible="True" VisibleIndex="4" />
                    <EditCellStyle HorizontalAlign="Right">
                    </EditCellStyle>
                    <Settings AutoFilterCondition="Contains" />
                    <PropertiesTextEdit>
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right">
                            <RequiredField ErrorText="Mandatory" IsRequired="true" />
                        </ValidationSettings>
                    </PropertiesTextEdit>

                    <PropertiesTextEdit MaxLength="20">
                    </PropertiesTextEdit>
                    
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn FieldName="suffix" Caption="Suffix" VisibleIndex="5">
                    <EditFormSettings Visible="True" VisibleIndex="5" />
                    <EditCellStyle HorizontalAlign="Right">
                    </EditCellStyle>
                    <Settings AutoFilterCondition="Contains" />
                    <PropertiesTextEdit>
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right">
                            <RequiredField ErrorText="Mandatory" IsRequired="true" />
                        </ValidationSettings>
                    </PropertiesTextEdit>

                    <PropertiesTextEdit MaxLength="20">
                    </PropertiesTextEdit>
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn FieldName="finyearcode" Caption="Financial year" VisibleIndex="6" Visible="false"
                    Width="10%">
                    <EditFormSettings Visible="False" />
                </dxe:GridViewDataTextColumn>
                
                 <dxe:GridViewDataComboBoxColumn FieldName="finyearid" Caption="Financial year"
                    VisibleIndex="6" Visible="true">
                    <PropertiesComboBox DataSourceID="Sqlfinyear" TextField="FinYear_Code"
                        ValueField="FinYear_ID" ValueType="System.String" EnableSynchronization="False" EnableIncrementalFiltering="True">
                    </PropertiesComboBox>
                    <EditFormSettings Visible="false" VisibleIndex="6" />
                    <EditCellStyle HorizontalAlign="Right">
                    </EditCellStyle>
                </dxe:GridViewDataComboBoxColumn>
                 <dxe:GridViewDataComboBoxColumn FieldName="Company" Caption="Company"
                    VisibleIndex="6" Visible="true">
                    <PropertiesComboBox DataSourceID="sqlcomp" TextField="cmp_Name"
                        ValueField="cmp_Name" ValueType="System.String" EnableSynchronization="False" EnableIncrementalFiltering="True">
                    </PropertiesComboBox>
                    <EditFormSettings Visible="false" VisibleIndex="6" />
                    <EditCellStyle HorizontalAlign="Right">
                    </EditCellStyle>
                </dxe:GridViewDataComboBoxColumn>
                <dxe:GridViewDataComboBoxColumn FieldName="branch" Caption="Branch"
                    VisibleIndex="6" Visible="true">
                    <PropertiesComboBox DataSourceID="sqlbranch" TextField="branch_description"
                        ValueField="branch_description" ValueType="System.String" EnableSynchronization="False" EnableIncrementalFiltering="True">
                    </PropertiesComboBox>
                    <EditFormSettings Visible="false" VisibleIndex="6" />
                    <EditCellStyle HorizontalAlign="Right">
                    </EditCellStyle>
                </dxe:GridViewDataComboBoxColumn>
                <dxe:GridViewDataTextColumn FieldName="digit" Caption="Digit" VisibleIndex="7">
                    <EditFormSettings Visible="True" VisibleIndex="7" />
                    <EditCellStyle HorizontalAlign="Right">
                    </EditCellStyle>
                    <PropertiesTextEdit>
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right">
                            <RequiredField ErrorText="Mandatory" IsRequired="false" />
                            <RegularExpression ValidationExpression="[0-9]+" />
                        </ValidationSettings>
                    </PropertiesTextEdit>

                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="start" Caption="Start No" VisibleIndex="8">
                    <EditFormSettings Visible="True" VisibleIndex="8" />
                    <EditCellStyle HorizontalAlign="Right">
                    </EditCellStyle>
                    <PropertiesTextEdit>
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right">
                            <RequiredField ErrorText="Mandatory" IsRequired="false" />
                            <RegularExpression ValidationExpression="[0-9]+" />
                        </ValidationSettings>
                    </PropertiesTextEdit>

                </dxe:GridViewDataTextColumn>
                
                <dxe:GridViewDataTextColumn FieldName="prefix_with" Caption="Prefill with" VisibleIndex="9" Visible="false">
                    <EditFormSettings Visible="false" VisibleIndex="9" />
                    <EditCellStyle HorizontalAlign="Right">
                    </EditCellStyle>
                    <PropertiesTextEdit MaxLength="5">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right">
                            <RequiredField ErrorText="Mandatory" IsRequired="false" />

                        </ValidationSettings>
                    </PropertiesTextEdit>

                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="IsActive" Caption="Is Active" VisibleIndex="10" Visible="true">
                    <EditFormSettings Visible="false" VisibleIndex="10" />
                    <EditCellStyle HorizontalAlign="Right">
                    </EditCellStyle>
                    <PropertiesTextEdit MaxLength="5">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right">
                            <RequiredField ErrorText="Mandatory" IsRequired="false" />

                        </ValidationSettings>
                    </PropertiesTextEdit>

                </dxe:GridViewDataTextColumn>
                 <dxe:GridViewDataComboBoxColumn  Caption="Voucher Type" FieldName="VoucherType"
                    VisibleIndex="10" Visible="true">
                    <PropertiesComboBox
                        ValueField="VoucherType" ValueType="System.String" EnableSynchronization="False" EnableIncrementalFiltering="True">
                        <Items>
                            <dxe:ListEditItem Text="None" Value="None"></dxe:ListEditItem>
                            <dxe:ListEditItem Text="Advance" Value="Advance"></dxe:ListEditItem>
                            <dxe:ListEditItem Text="Non Advance" Value="NonAdvance"></dxe:ListEditItem>
                        </Items>
                    </PropertiesComboBox>
                    <EditFormSettings Visible="True" VisibleIndex="11" />
                    <EditCellStyle HorizontalAlign="Right">
                    </EditCellStyle>
                </dxe:GridViewDataComboBoxColumn>
                   
                <%--<dxe:GridViewDataTextColumn FieldName="schemaID" Caption="schemaID" VisibleIndex="10" Visible="true">
                    <EditFormSettings Visible="false" VisibleIndex="10" />
                    <EditCellStyle HorizontalAlign="Right">
                    </EditCellStyle>
                    <PropertiesTextEdit MaxLength="5">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right">
                            <RequiredField ErrorText="Mandatory" IsRequired="false" />

                        </ValidationSettings>
                    </PropertiesTextEdit>

                </dxe:GridViewDataTextColumn>--%>
                <%--   <dxe:GridViewCommandColumn Caption="Actions" VisibleIndex="10" ShowEditButton="true" ShowDeleteButton="true" Width="70px">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <HeaderTemplate>
                        Actions
                    </HeaderTemplate>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                </dxe:GridViewCommandColumn>--%>
                <dxe:GridViewDataTextColumn Caption="" VisibleIndex="11" Width="8%">
                    <CellStyle HorizontalAlign="Center">
                    </CellStyle>
                    <HeaderStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        Actions
                                   
                    </HeaderTemplate>
                    <DataItemTemplate>
                        <% if (rights.CanEdit)
                           { %>
                        <a href="javascript:void(0);" onclick="OnEdit('<%#Eval("schemaID") %>')"  class="pad" >
                            <img src="/assests/images/Edit.png" alt="Edit"></a>
                        <% } %>
                        <% if (rights.CanDelete)
                           { %>
                        <a href="javascript:void(0);" onclick="DeleteRow('<%#Eval("schemaID") %>')" alt="Delete" class="pad">
                            <img src="/assests/images/Delete.png" /></a>
                        <% } %>
                    </DataItemTemplate>
                </dxe:GridViewDataTextColumn>

            </columns>
           

            <settingscommandbutton>
                <ClearFilterButton Text="Clear">
                </ClearFilterButton>

                <EditButton Image-Url="/assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit">
                </EditButton>
                <DeleteButton Image-Url="/assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                </DeleteButton>

                <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary"></UpdateButton>
                <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger"></CancelButton>


            </settingscommandbutton>


            <settingsediting mode="PopupEditForm" popupeditformhorizontalalign="Center" popupeditformmodal="True"
                popupeditformverticalalign="WindowCenter" popupeditformwidth="700px" editformcolumncount="1" popupeditformheight="500px" />
            <settingssearchpanel visible="True" />
            <settings showfilterrow="true" showgrouppanel="true" showfilterrowmenu="true" />

            <settingsbehavior confirmdelete="True" columnresizemode="NextColumn" filterrowmode="Auto" />

            <settingstext popupeditformcaption="Add/ Modify Schema" confirmdelete="Confirm delete?" />
            <stylespager>
                <Summary Width="100%">
                </Summary>
            </stylespager>

        </dxe:ASPxGridView>

        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>

        <asp:SqlDataSource ID="SqlSchematype" runat="server"
            SelectCommand="select type_Id,type_name from tbl_master_idschematype where type_Isactive=1"></asp:SqlDataSource>

        <asp:SqlDataSource ID="Sqlfinyear" runat="server"
            SelectCommand="select FinYear_ID,FinYear_Code from Master_FinYear"></asp:SqlDataSource>

        <asp:SqlDataSource ID="sqlcomp" runat="server" 
            SelectCommand="select cmp_internalid,cmp_Name from tbl_master_company"></asp:SqlDataSource>

         <asp:SqlDataSource ID="sqlbranch" runat="server" 
            SelectCommand="select branch_id,branch_description from tbl_master_branch"></asp:SqlDataSource>
    </div>
</asp:Content>

