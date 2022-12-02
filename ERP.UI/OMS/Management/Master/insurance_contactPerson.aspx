<%@ Page Title="Contact Person" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_insurance_contactPerson" CodeBehind="insurance_contactPerson.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function AddAddress(KeyVal) {
            var url = 'AddAddressForContactPerson.aspx?id=' + KeyVal;
            //frmOpenNewWindow1(url, 500, 500)//comment by sanjib 19122016 due to this popup has been discuonitue.
           
            Popup_AddAddress.SetContentUrl(url);
            Popup_AddAddress.Show();

        }
        function frmOpenNewWindow1(location, v_height, v_weight) {
            var y = (screen.availHeight - v_height) / 2;
            var x = (screen.availWidth - v_weight) / 2;
            window.open(location, "Search_Conformation_Box", "height=" + v_height + ",width=" + v_weight + ",top=" + y + ",left=" + x + ",location=no,directories=no,menubar=no,toolbar=no,status=yes,scrollbars=yes,resizable=no,dependent=no");
        }

    </script>
    <style>
        #GridContactPerson_col12 {
            text-decoration: none !important;
        }

        .dxflHARSys > table, .dxflHARSys > div {
            margin-left: 0;
            margin-right: auto;
            padding-left: 100px;
        }

        .dxgvEditForm_PlasticBlue {
            background-color: #f0f0f0 !important;
        }

        #GridContactPerson_DXPEForm_DXEFL_DXEditor5_EC, #GridContactPerson_DXPEForm_DXEFL_DXEditor0_EC {
            position: absolute;
        }
        #GridContactPerson_DXPEForm_DXEFT {
            width:97% !important;
        }
        .dxeErrorFrameSys.dxeErrorCellSys{
            position:absolute;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <%-- <h3>Insurance Contact Person List</h3>--%>
            <%--Code Commented and Added by Sam on 09122016 to change the header name. ................-%>
            <%--<h3><asp:Label ID="lblHeadTitle" runat="server"></asp:Label> Add/Edit Address </h3>--%>
            <h3>
                <asp:Label ID="lblHeadTitle" runat="server"></asp:Label>
                - Add Contact Person </h3>
        </div>
        <div class="crossBtn">
            <%--<a href="Lead.aspx" id="goBackCrossBtn"><i class="fa fa-times"></i></a>--%>
            <asp:HyperLink
                ID="goBackCrossBtn"
                NavigateUrl="#"
                runat="server">
        <i class="fa fa-times"></i>
            </asp:HyperLink>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td>
                    <table class="TableMain100">
                        <tr>
                            <td style="text-align: left; vertical-align: top">
                                <table>
                                    <tr>
                                        <td id="ShowFilter">
                                            <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-success"><span >Show Filter</span></a>--%>
                                            <a href="javascript:void(0);" class="btn btn-primary" onclick="GridContactPerson.AddNewRow();"><span>Add New</span> </a>
                                           <% if (rights.CanExport)
                                               { %>
                                              <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                                <asp:ListItem Value="2">XLS</asp:ListItem>
                                                <asp:ListItem Value="3">RTF</asp:ListItem>
                                                <asp:ListItem Value="4">CSV</asp:ListItem>

                                            </asp:DropDownList>
                                             <% } %>
                                            
                                        </td>
                                        <td id="Td1"></td>
                                    </tr>
                                </table>
                            </td>
                            <%--<td class="gridcellright pull-right">
                                <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true"
                                    Font-Bold="False" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                    ValueType="System.Int32" Width="130px">
                                    <Items>
                                        <dxe:ListEditItem Text="Select" Value="0" />
                                        <dxe:ListEditItem Text="PDF" Value="1" />
                                        <dxe:ListEditItem Text="XLS" Value="2" />
                                        <dxe:ListEditItem Text="RTF" Value="3" />
                                        <dxe:ListEditItem Text="CSV" Value="4" />
                                    </Items>
                                    <Border BorderColor="Black" />
                                    <DropDownButton Text="Export">
                                    </DropDownButton>
                                </dxe:ASPxComboBox>
                            </td>--%>
                        </tr>
                    </table>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxGridView ID="GridContactPerson" ClientInstanceName="GridContactPerson"
                        KeyFieldName="ContactId" runat="server" AutoGenerateColumns="False"
                        Width="100%" OnHtmlDataCellPrepared="GridContactPerson_HtmlDataCellPrepared"
                        OnInitNewRow="GridContactPerson_InitNewRow" OnRowUpdating="GridContactPerson_RowUpdating" OnRowInserting="GridContactPerson_RowInserting"
                        OnRowDeleting="GridContactPerson_RowDeleting" OnCellEditorInitialize="GridContactPerson_CellEditorInitialize">
                        <Styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="name" Caption="Name" VisibleIndex="0">
                                <EditFormSettings Visible="True" VisibleIndex="0" />
                                <EditCellStyle HorizontalAlign="Right">
                                </EditCellStyle>
                                <%-- 09122016 set postion and tooltrip text--%>
                                <PropertiesTextEdit>
                                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right">
                                        <RequiredField ErrorText="Mandatory" IsRequired="true" />

                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <%-- 09122016 set maxlength--%>
                                <PropertiesTextEdit MaxLength="150">
                                </PropertiesTextEdit>
                                <%--  end--%>
                                <%--  end--%>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="Phone" VisibleIndex="3">
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Officephone" Visible="False" VisibleIndex="1">
                                <EditCellStyle HorizontalAlign="Right">
                                </EditCellStyle>
                                <%-- 09122016 set maxlength--%>
                                <PropertiesTextEdit MaxLength="500">
                                </PropertiesTextEdit>
                                <%--  end--%>
                                <EditFormSettings Visible="True" Caption="Office Phone" VisibleIndex="1" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Residencephone" Visible="False" VisibleIndex="1" Width="200px">
                                <EditCellStyle HorizontalAlign="Right" CssClass="form-control">
                                </EditCellStyle>
                                <%-- 09122016 set maxlength--%>
                                <PropertiesTextEdit MaxLength="500">
                                </PropertiesTextEdit>
                                <%--  end--%>
                                <EditFormSettings Visible="True" Caption="Residence Phone" VisibleIndex="2" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Mobilephone" Visible="False" VisibleIndex="1" PropertiesTextEdit-MaxLength="11">
                                <EditCellStyle HorizontalAlign="Right">
                                </EditCellStyle>
                                <%-- 09122016 Set number validation--%>
                                <PropertiesTextEdit>
                                    <ValidationSettings Display="Dynamic" ErrorTextPosition="Right">
                                        <RegularExpression ErrorText="Invalid Mobile No!" ValidationExpression="[0-9]+" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>

                                <%--  end--%>
                                <EditFormSettings Visible="True" Caption="Mobile Phone" VisibleIndex="3" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="email" Caption="Email" VisibleIndex="4">
                                <%-- 09122016 set maxlength--%>
                                <PropertiesTextEdit MaxLength="50">
                                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right">
                                        <RequiredField ErrorText="Mandatory" IsRequired="true" />
                                        <RegularExpression ErrorText="Invalid Email ID!" ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <%--  end--%>
                                <EditCellStyle HorizontalAlign="Right">
                                </EditCellStyle>
                                <EditFormSettings Visible="True" VisibleIndex="4" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="cp_designation" Caption="Designation" VisibleIndex="2"
                                Width="10%">
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataComboBoxColumn FieldName="cp_designation_id" Caption="Designation"
                                VisibleIndex="2" Visible="false">
                                <PropertiesComboBox DataSourceID="SqlDesignation" TextField="deg_designation" ValueField="deg_id"
                                    ValueType="System.String" EnableSynchronization="False" EnableIncrementalFiltering="False">
                                </PropertiesComboBox>
                                <EditFormSettings Visible="True" VisibleIndex="6" />
                                <EditCellStyle HorizontalAlign="Right">
                                </EditCellStyle>
                            </dxe:GridViewDataComboBoxColumn>

                            <dxe:GridViewDataTextColumn FieldName="cp_relationShip" Caption="Relationship" VisibleIndex="1"
                                Width="10%">
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataComboBoxColumn FieldName="cp_relationShip_id" Caption="Relationship"
                                VisibleIndex="1" Visible="false">
                                <PropertiesComboBox DataSourceID="SqlFamRelationShip" TextField="fam_familyRelationship"
                                    ValueField="fam_id" ValueType="System.String" EnableSynchronization="False" EnableIncrementalFiltering="False">
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) {
	                                            var value = s.GetText().toUpperCase();	
                                                if(value == &quot;EMPLOYEE&quot;)
                                                { $('#GridContactPerson_DXEFL_6').attr('style','visible:true');
                                                     GridContactPerson.GetEditor(&quot;cp_designation_id&quot;).SetEnabled(true);
                                                }
                                                else
                                                {$('#GridContactPerson_DXEFL_6').attr('style','visible:false');
                                                     GridContactPerson.GetEditor(&quot;cp_designation_id&quot;).SetEnabled(false);
                                                }
                                            }"
                                        Init="function(s, e) {
	                                            var value = s.GetText().toUpperCase();
                                                if(value == &quot;EMPLOYEE&quot;)
                                                {$('#GridContactPerson_DXEFL_6').attr('style','visible:true');
                                                     GridContactPerson.GetEditor(&quot;cp_designation_id&quot;).SetEnabled(true);
                                                }
                                                else
                                                {$('#GridContactPerson_DXEFL_6').attr('style','visible:false');
                                                     GridContactPerson.GetEditor(&quot;cp_designation_id&quot;).SetEnabled(false);
                                                }
                                            }" />

                                </PropertiesComboBox>
                                <EditFormSettings Visible="True" VisibleIndex="5" />
                                <EditCellStyle HorizontalAlign="Right">
                                </EditCellStyle>
                            </dxe:GridViewDataComboBoxColumn>

                            <dxe:GridViewDataTextColumn FieldName="status" Caption="Status" VisibleIndex="5"
                                Width="10%">
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>

                             <%-- As per requirement it is Unnecessarily so therefor i have comment-out below point by sanjib 19122016  %>

                            <%-- <dxe:GridViewDataTextColumn FieldName="cp_relationShip_id" Caption="
                                Width="10%">
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>--%>

                            <dxe:GridViewDataComboBoxColumn FieldName="cp_status" Visible="False" VisibleIndex="4"
                                Width="10%">
                                <PropertiesComboBox ValueType="System.Char" EnableIncrementalFiltering="True">
                                    <Items>
                                        <dxe:ListEditItem Text="Active" Value="Y"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Suspended" Value="N"></dxe:ListEditItem>
                                    </Items>
                                </PropertiesComboBox>
                                <EditFormSettings Visible="True" Caption="Status" VisibleIndex="7" />
                                <EditCellStyle HorizontalAlign="Right">
                                </EditCellStyle>
                            </dxe:GridViewDataComboBoxColumn>


                            <dxe:GridViewDataTextColumn FieldName="cp_Pan" Caption="PAN Number" VisibleIndex="6" PropertiesTextEdit-MaxLength="20">
                                <EditFormSettings Visible="True" VisibleIndex="8" />
                                <EditCellStyle HorizontalAlign="Right">
                                </EditCellStyle>
                                <%-- 09122016 Set PANnumber validation--%>
                                <PropertiesTextEdit>
                                    <ValidationSettings Display="Dynamic" ErrorTextPosition="Right">
                                        <RegularExpression ErrorText="Invalid PAN Number!" ValidationExpression="^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>

                                <%--  end--%>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="cp_Din" Caption="Din" VisibleIndex="7" PropertiesTextEdit-MaxLength="20">
                                <EditFormSettings Visible="True" VisibleIndex="9" />
                                <EditCellStyle HorizontalAlign="Right">
                                </EditCellStyle>
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewCommandColumn VisibleIndex="8" Width="10%" ShowClearFilterButton="true" ShowDeleteButton="true" ShowEditButton="true">

                                <HeaderTemplate>Actions</HeaderTemplate>
                                <HeaderStyle Font-Underline="True" HorizontalAlign="Center" CssClass="textdecoration" />

                                <CustomButtons>
                                    <dxe:GridViewCommandColumnCustomButton Image-Url="../../../assests/images/show.png" Image-ToolTip="Add/Edit Address">
                                       
                                    </dxe:GridViewCommandColumnCustomButton>

                                </CustomButtons>

                            </dxe:GridViewCommandColumn>


                            <%-- As per requirement that all Action should be same column so therefor i have comment-out below point by sanjib 19122016  %>

                            <%--<dxe:GridViewDataTextColumn VisibleIndex="9">
                                <HeaderCaptionTemplate>
                                    <dxe:ASPxHyperLink ID="ASPxHyperLin" runat="server" Text="Address *"
                                        Font-Size="12px" Font-Color="Red" ForeColor="White">
                                    </dxe:ASPxHyperLink>
                                </HeaderCaptionTemplate>
                                <DataItemTemplate>

                                    <%--<asp:HyperLink ID="hyplnk" runat="server" NavigateUrl='<%#"AddAddressForContactPerson.aspx?id="+( Container.KeyValue) %>'>Add/Edit Address </asp:HyperLink>--%>
                                   <%-- <a href="javascript:void(0);" onclick="AddAddress('<%# Container.KeyValue %>')" title="Add/Edit Address">
                                        <img src="../../../assests/images/show.png" /></a>
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>--%>

                            <%--End --%>
                        </Columns>
                         <ClientSideEvents CustomButtonClick="function(s, e) {
                             var key = s.GetRowKey(e.visibleIndex);
                             AddAddress(key);
                            
                            }" />
                        <SettingsCommandButton>
                            <ClearFilterButton Text="Clear">
                            </ClearFilterButton>
                            <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                            </EditButton>
                            <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete" Styles-Style-CssClass="pad">
                            </DeleteButton>
                            
                         


                            <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary"></UpdateButton>
                            <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger"></CancelButton>


                        </SettingsCommandButton>

                        <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" />
                        <SettingsPager ShowSeparators="True">
                        </SettingsPager>
                        <SettingsEditing PopupEditFormHeight="520px" PopupEditFormHorizontalAlign="WindowCenter" Mode="PopupEditForm" EditFormColumnCount="1"
                            PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="520px" />
                        
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                    </dxe:ASPxGridView>
                    <br />
                    <%-- <asp:SqlDataSource ID="SqlContactPerson" runat="server" 
                            SelectCommand="" InsertCommand="ContactPersonInsertforInsCompany" InsertCommandType="StoredProcedure"
                            DeleteCommand="ContactPersonDelete" DeleteCommandType="StoredProcedure" UpdateCommand="ContactPersonUpdateforInsComp"
                            UpdateCommandType="StoredProcedure">
                            <InsertParameters>
                                <asp:Parameter Name="name" Type="String" />
                                <asp:Parameter Name="Officephone" Type="String" />
                                <asp:Parameter Name="Residencephone" Type="String" />
                                <asp:Parameter Name="Mobilephone" Type="String" />
                                <asp:Parameter Name="email" Type="String" />
                                <asp:Parameter Name="cp_designation" Type="String" />
                                <asp:Parameter Name="cp_relationShip" Type="String" />
                                <asp:Parameter Name="cp_status" Type="String" />
                                 <asp:Parameter Name="cp_Pan" Type="String" />
                                <asp:SessionParameter Name="userid" SessionField="userid" Type="Int32" />
                                <asp:SessionParameter Name="agentid" SessionField="KeyVal_InternalID" Type="String" />
                            </InsertParameters>
                            <DeleteParameters>
                                <asp:Parameter Name="ContactId" Type="String" />
                            </DeleteParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="name" Type="String" />
                                <asp:Parameter Name="Officephone" Type="String" />
                                <asp:Parameter Name="Residencephone" Type="String" />
                                <asp:Parameter Name="Mobilephone" Type="String" />
                                <asp:Parameter Name="email" Type="String" />
                                <asp:Parameter Name="cp_designation" Type="String" />
                                <asp:Parameter Name="cp_relationShip" Type="String" />
                                <asp:Parameter Name="cp_status" Type="String" />
                                <asp:Parameter Name="ContactId" Type="String" />
                                 <asp:Parameter Name="cp_Pan" Type="String" />
                                <asp:SessionParameter Name="userid" SessionField="userid" Type="Int32" />
                            </UpdateParameters>
                        </asp:SqlDataSource>--%>
                    <asp:SqlDataSource ID="SqlDesignation" runat="server" 
                        SelectCommand="select deg_id,deg_designation from tbl_master_designation"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlFamRelationShip" runat="server" 
                        SelectCommand="select fam_id,fam_familyRelationship from tbl_master_familyrelationship"></asp:SqlDataSource>
                </td>
            </tr>
        </table>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
    <dxe:ASPxPopupControl ID="Popup_AddAddress" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="Popup_AddAddress" Height="550px"
        Width="500px" HeaderText="Add/Modify Address" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>
</asp:Content>
