<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_OutsourcingComp_ContactPerson" CodeBehind="OutsourcingComp_ContactPerson.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function disp_prompt(name) {
            if (name == "tab0") {
                //alert(name);
                document.location.href = "OutsourcingComp_general.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                //document.location.href="OutsourcingComp_ContactPerson.aspx";         
            }
            else if (name == "tab2") {
                //alert(name);
                document.location.href = "OutsourcingComp_Correspondence.aspx";
            }
            else if (name == "tab3") {
                //alert(name);
                document.location.href = "OutsourcingComp_BankDetails.aspx";
            }
            else if (name == "tab4") {
                //alert(name);
                document.location.href = "OutsourcingComp_DPDetails.aspx";
            }
            else if (name == "tab5") {
                //alert(name);
                document.location.href = "OutsourcingComp_Document.aspx";
            }
            else if (name == "tab6") {
                //alert(name);
                document.location.href = "OutsourcingComp_GroupMember.aspx";
            }

        }
        function AddAddress(KeyVal) {
            var url = 'AddAddressForContactPerson.aspx?id=' + KeyVal;
            frmOpenNewWindow1(url, 300, 300)
        }
        function frmOpenNewWindow1(location, v_height, v_weight) {
            var y = (screen.availHeight - v_height) / 2;
            var x = (screen.availWidth - v_weight) / 2;
            window.open(location, "Search_Conformation_Box", "height=" + v_height + ",width=" + v_weight + ",top=" + y + ",left=" + x + ",location=no,directories=no,menubar=no,toolbar=no,status=yes,scrollbars=no,resizable=no,dependent=no");
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Outsourcing Agents/Companies</h3>
        </div>

    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td style="vertical-align: top; padding-left: 30px;">
                    <div class="crossBtn"><a href="OutsourcingComp.aspx"><i class="fa fa-times"></i></a></div>
                </td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <asp:Label ID="lblHeader" runat="server" Font-Bold="True" Font-Size="15px" ForeColor="Navy"
                        Width="819px" Height="18px"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 100%">
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="1" ClientInstanceName="page"
                        Font-Size="12px" Width="99%">
                        <TabPages>
                            <dxe:TabPage Text="General" Name="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="Contact Person" Name="ContactPreson">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <table class="TableMain100">
                                            <tr>
                                                <td id="ShowFilter">
                                                    <a href="javascript:void(0);" onclick="GridContactPerson.AddNewRow();" class="btn btn-success"><span>Add New</span> </a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 933px">
                                                    <dxe:ASPxGridView ID="GridContactPerson" ClientInstanceName="GridContactPerson"
                                                        KeyFieldName="ContactId" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1"
                                                        Width="100%" OnHtmlDataCellPrepared="GridContactPerson_HtmlDataCellPrepared">
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
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="Phone" VisibleIndex="3">
                                                                <EditFormSettings Visible="False" />
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="Officephone" Visible="false" VisibleIndex="1">
                                                                <EditCellStyle HorizontalAlign="Right">
                                                                </EditCellStyle>
                                                                <EditFormSettings Visible="True" Caption="OfficePhone" VisibleIndex="1" />
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="Residencephone" Visible="false" VisibleIndex="1">
                                                                <EditCellStyle HorizontalAlign="Right">
                                                                </EditCellStyle>
                                                                <EditFormSettings Visible="True" Caption="ResidencePhone" VisibleIndex="2" />
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="Mobilephone" Visible="false" VisibleIndex="1">
                                                                <EditCellStyle HorizontalAlign="Right">
                                                                </EditCellStyle>
                                                                <EditFormSettings Visible="True" Caption="MobilePhone" VisibleIndex="3" />
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="email" Caption="Email" VisibleIndex="4">
                                                                <PropertiesTextEdit>
                                                                    <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom">
                                                                        <RegularExpression ErrorText="Invali E-mail ID!" ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$" />
                                                                    </ValidationSettings>
                                                                </PropertiesTextEdit>
                                                                <EditCellStyle HorizontalAlign="Right">
                                                                </EditCellStyle>
                                                                <EditFormSettings Visible="True" VisibleIndex="4" />
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataComboBoxColumn FieldName="cp_designation" Caption="Designation"
                                                                VisibleIndex="2" Width="10%">
                                                                <PropertiesComboBox DataSourceID="SqlDesignation" TextField="deg_designation" ValueField="deg_id"
                                                                    ValueType="System.String" EnableSynchronization="False" EnableIncrementalFiltering="True">
                                                                </PropertiesComboBox>
                                                                <EditFormSettings Visible="True" VisibleIndex="6" />
                                                                <EditCellStyle HorizontalAlign="Right">
                                                                </EditCellStyle>
                                                            </dxe:GridViewDataComboBoxColumn>
                                                            <dxe:GridViewDataComboBoxColumn FieldName="cp_relationShip" Caption="RelationShip"
                                                                VisibleIndex="1" Width="10%">
                                                                <PropertiesComboBox DataSourceID="SqlFamRelationShip" TextField="fam_familyRelationship"
                                                                    ValueField="fam_id" ValueType="System.String" EnableSynchronization="False" EnableIncrementalFiltering="True">
                                                                    <ClientSideEvents Init="function(s, e) {
	var value = s.GetText().toUpperCase();	
    if(value == &quot;EMPLOYEE&quot;)
    {
         GridContactPerson.GetEditor(&quot;cp_designation&quot;).SetVisible(true);
    }
    else
    {
         GridContactPerson.GetEditor(&quot;cp_designation&quot;).SetVisible(false);
    }
}" />
                                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) {
	var value = s.GetText().toUpperCase();
    if(value == &quot;EMPLOYEE&quot;)
    {
         GridContactPerson.GetEditor(&quot;cp_designation&quot;).SetVisible(true);
    }
    else
    {
         GridContactPerson.GetEditor(&quot;cp_designation&quot;).SetVisible(false);
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
                                                            <dxe:GridViewCommandColumn VisibleIndex="5" Caption="Actions" Width="10%" ShowDeleteButton="true" ShowEditButton="true" ShowClearFilterButton="true">
                                                                <%--<ClearFilterButton Visible="True">
                                                                </ClearFilterButton>
                                                                <DeleteButton Visible="True">
                                                                </DeleteButton>
                                                                <EditButton Visible="True">
                                                                </EditButton>--%>
                                                                <HeaderCaptionTemplate>
                                                                    Actions
                                                                </HeaderCaptionTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </dxe:GridViewCommandColumn>
                                                            <dxe:GridViewDataTextColumn VisibleIndex="7">
                                                                <DataItemTemplate>
                                                                    <a href="javascript:void(0);" onclick="AddAddress('<%# Container.KeyValue %>')">Add
                                                                            Address</a>
                                                                </DataItemTemplate>
                                                                <EditFormSettings Visible="False" />
                                                            </dxe:GridViewDataTextColumn>
                                                        </Columns>
                                                        <SettingsCommandButton>
                                                            <DeleteButton Image-Url="/assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                            </DeleteButton>
                                                            <EditButton Image-Url="/assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit">
                                                            </EditButton>
                                                            <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary btn-xs"></UpdateButton>
                                                            <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger btn-xs"></CancelButton>
                                                        </SettingsCommandButton>
                                                        <SettingsBehavior ConfirmDelete="True" />
                                                        <SettingsPager ShowSeparators="True">
                                                        </SettingsPager>
                                                    </dxe:ASPxGridView>
                                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                                        SelectCommand="select A.cp_contactId as ContactId, A.cp_name as name,(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Office') as Officephone,(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Residence') as Residencephone,(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Mobile') as Mobilephone,isnull(('(O)'+(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Office')),'')+isnull(('(R)'+(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Residence')),'')+isnull(('(M)'+(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Mobile')),'') as Phone,(select eml_email from tbl_master_email where eml_cntId=A.cp_contactId) as email, (case when cp_status = 'Y' then 'Active' when cp_status = 'N' then 'Suspended' else 'N/A'  end) as status, ltrim(rtrim(cp_status)) as cp_status,ltrim(rtrim(cp_designation)) as cp_designation,ltrim(rtrim(cp_relationShip)) as cp_relationShip from tbl_master_contactperson A  where cp_agentInternalId=@insuId ORDER BY cp_status desc"
                                                        InsertCommand="ContactPersonInsertforInsCompany" InsertCommandType="StoredProcedure"
                                                        DeleteCommand="ContactPersonDelete" DeleteCommandType="StoredProcedure" UpdateCommand="ContactPersonUpdateforInsComp"
                                                        UpdateCommandType="StoredProcedure">
                                                        <SelectParameters>
                                                            <asp:SessionParameter Name="insuId" SessionField="KeyVal_InternalID" Type="String" />
                                                        </SelectParameters>
                                                        <InsertParameters>
                                                            <asp:Parameter Name="name" Type="String" />
                                                            <asp:Parameter Name="Officephone" Type="String" />
                                                            <asp:Parameter Name="Residencephone" Type="String" />
                                                            <asp:Parameter Name="Mobilephone" Type="String" />
                                                            <asp:Parameter Name="email" Type="String" />
                                                            <asp:Parameter Name="cp_designation" Type="String" />
                                                            <asp:Parameter Name="cp_relationShip" Type="String" />
                                                            <asp:Parameter Name="cp_status" Type="String" />
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
                                                            <asp:SessionParameter Name="userid" SessionField="userid" Type="Int32" />
                                                        </UpdateParameters>
                                                    </asp:SqlDataSource>
                                                    <asp:SqlDataSource ID="SqlDesignation" runat="server"
                                                        SelectCommand="select deg_id,deg_designation from tbl_master_designation"></asp:SqlDataSource>
                                                    <asp:SqlDataSource ID="SqlFamRelationShip" runat="server"
                                                        SelectCommand="select fam_id,fam_familyRelationship from tbl_master_familyrelationship"></asp:SqlDataSource>
                                                </td>
                                            </tr>
                                        </table>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="Correspondence" Name="CorresPondence">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="BankDetails" Text="Bank Details">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="DPDetails" Text="DP Details">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Documents" Text="Documents">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="GroupMember" Text="Group Member">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                        </TabPages>
                        <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                                var activeTab   = page.GetActiveTab();
	                                                var Tab0 = page.GetTab(0);
	                                                var Tab1 = page.GetTab(1);
	                                                var Tab2 = page.GetTab(2);
	                                                var Tab3 = page.GetTab(3);
	                                                var Tab4 = page.GetTab(4);
	                                                var Tab5 = page.GetTab(5);
	                                                var Tab6 = page.GetTab(6);
	                                                var Tab7 = page.GetTab(7);
    	                                            
	                                                if(activeTab == Tab0)
	                                                {
	                                                    disp_prompt('tab0');
	                                                }
	                                                if(activeTab == Tab1)
	                                                {
	                                                    disp_prompt('tab1');
	                                                }
	                                                else if(activeTab == Tab2)
	                                                {
	                                                    disp_prompt('tab2');
	                                                }
	                                                else if(activeTab == Tab3)
	                                                {
	                                                    disp_prompt('tab3');
	                                                }
	                                                else if(activeTab == Tab4)
	                                                {
	                                                    disp_prompt('tab4');
	                                                }
	                                                else if(activeTab == Tab5)
	                                                {
	                                                    disp_prompt('tab5');
	                                                }
	                                                else if(activeTab == Tab6)
	                                                {
	                                                    disp_prompt('tab6');
	                                                }
	                                                else if(activeTab == Tab7)
	                                                {
	                                                    disp_prompt('tab7');
	                                                }
    	                                            
	                                                }"></ClientSideEvents>
                        <ContentStyle>
                            <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                        </ContentStyle>
                        <LoadingPanelStyle ImageSpacing="6px">
                        </LoadingPanelStyle>
                    </dxe:ASPxPageControl>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
