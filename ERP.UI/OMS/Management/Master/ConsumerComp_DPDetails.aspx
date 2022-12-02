<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_ConsumerComp_DPDetails" CodeBehind="ConsumerComp_DPDetails.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">
        function disp_prompt(name) {
            if (name == "tab0") {
                //alert(name);
                document.location.href = "ConsumerComp_general.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "ConsumerComp_ContactPerson.aspx";
            }
            else if (name == "tab2") {
                //alert(name);
                document.location.href = "ConsumerComp_Correspondence.aspx";
            }
            else if (name == "tab3") {
                //alert(name);
                document.location.href = "ConsumerComp_BankDetails.aspx";
            }
            else if (name == "tab4") {
                //alert(name);
                //document.location.href="ConsumerComp_DPDetails.aspx";         
            }
            else if (name == "tab5") {
                //alert(name);
                document.location.href = "ConsumerComp_Document.aspx";
            }
            else if (name == "tab6") {
                //alert(name);
                document.location.href = "ConsumerComp_GroupMember.aspx";
            }
        }
        function CallList(obj, obj1, obj2) {
            ajax_showOptions(obj, obj1, obj2);
        }
        FieldName = 'ASPxPageControl1_DpDetailsGrid_DXSelInput';
        function setvaluetovariable(obj) {
            if (obj == '1') {
                document.getElementById("TrPoaName").style.display = "none";
            }
            else {
                document.getElementById("TrPoaName").style.display = "inline";
            }
        }
    </script>

    <div>
        <table class="TableMain100">
            <tr>
                <td class="EHEADER"></td>
            </tr>
            <tr>
                <td colspan="2">
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="4" ClientInstanceName="page" Width="100%">

                        <TabPages>
                            <dxe:TabPage Text="General" Name="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Contact Person" Text="Contact Person">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="CorresPondence" Name="CorresPondence">
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
                                        <dxe:ASPxGridView ID="DpDetailsGrid" runat="server" AutoGenerateColumns="False" DataSourceID="DpDetailsdata" KeyFieldName="Id" ClientInstanceName="gridDp" Width="100%" Font-Size="12px" OnHtmlEditFormCreated="DpDetailsGrid_HtmlEditFormCreated" OnRowInserting="DpDetailsGrid_RowInserting" OnRowUpdating="DpDetailsGrid_RowUpdating" OnRowValidating="DpDetailsGrid_RowValidating">
                                            <Templates>
                                                <EditForm>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="text-align: right; width: 30%">
                                                                <span class="Ecoheadtxt" style="color: Black">Category :</span>
                                                            </td>
                                                            <td style="text-align: left">
                                                                <dxe:ASPxComboBox ID="comboCategory" EnableIncrementalFiltering="True" EnableSynchronization="False" Value='<%#Bind("Category") %>' runat="server" ValueType="System.String" Width="285px">
                                                                    <Items>
                                                                        <dxe:ListEditItem Text="Default" Value="Default" />
                                                                        <dxe:ListEditItem Text="Secondary" Value="Secondary" />
                                                                    </Items>
                                                                    <ButtonStyle Width="13px">
                                                                    </ButtonStyle>
                                                                </dxe:ASPxComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right; width: 30%">
                                                                <span class="Ecoheadtxt" style="color: Black">DPName :</span>
                                                            </td>
                                                            <td style="text-align: left">
                                                                <asp:TextBox ID="txtDPName" CssClass="EcoheadCon" Text='<%#Bind("DP") %>' runat="server" Width="279px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right; width: 30%">
                                                                <span class="Ecoheadtxt" style="color: Black">ClientID :</span>
                                                            </td>
                                                            <td style="text-align: left">
                                                                <asp:TextBox ID="txtClientID" CssClass="EcoheadCon" Text='<%#Bind("ClientId") %>' runat="server" Width="279px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right; width: 30%">
                                                                <span class="Ecoheadtxt" style="color: Black">POA :</span>
                                                            </td>
                                                            <td style="text-align: left">
                                                                <dxe:ASPxComboBox ID="comboPOA" EnableIncrementalFiltering="True" EnableSynchronization="False" Value='<%#Bind("POA") %>' runat="server" ValueType="System.String" Width="285px">
                                                                    <Items>
                                                                        <dxe:ListEditItem Text="Yes" Value="1" />
                                                                        <dxe:ListEditItem Text="No" Value="0" />
                                                                    </Items>
                                                                    <ClientSideEvents ValueChanged="function(s,e){
                                                                                                    var indexr = s.GetSelectedIndex();
                                                                                                    setvaluetovariable(indexr)
                                                                                                    }" />
                                                                    <ButtonStyle Width="13px">
                                                                    </ButtonStyle>
                                                                </dxe:ASPxComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr id="TrPoaName">
                                                            <td style="text-align: right; width: 30%">
                                                                <span class="Ecoheadtxt" style="color: Black">POAName :</span>
                                                            </td>
                                                            <td style="text-align: left">
                                                                <asp:TextBox ID="txtPoaName" CssClass="EcoheadCon" Text='<%#Bind("POAName") %>' runat="server" Width="279px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" style="text-align: center">
                                                                <%--<controls>
                                                       <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors">
                                                       </dxe:ASPxGridViewTemplateReplacement>                                                           
                                                     </controls>--%>
                                                                <div style="text-align: right; padding: 2px 2px 2px 2px">
                                                                    <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton" runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                    <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton" runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" style="display: none">
                                                                <asp:TextBox ID="txtDPName_hidden" CssClass="EcoheadCon" Text='<%#Bind("DPName") %>' runat="server" Width="279px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EditForm>
                                            </Templates>
                                            <SettingsText PopupEditFormCaption="Add/Modify DP Details" ConfirmDelete="Confirm delete?" />
                                            <Styles>
                                                <LoadingPanel ImageSpacing="10px">
                                                </LoadingPanel>
                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                </Header>
                                            </Styles>
                                            <Settings ShowStatusBar="Visible" />
                                            <SettingsBehavior ConfirmDelete="True" />
                                            <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                                                <FirstPageButton Visible="True">
                                                </FirstPageButton>
                                                <LastPageButton Visible="True">
                                                </LastPageButton>
                                            </SettingsPager>
                                            <Columns>
                                                <dxe:GridViewDataTextColumn FieldName="Id" VisibleIndex="0" Visible="False">
                                                    <EditFormSettings Visible="False" />
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="CntId" VisibleIndex="0" Visible="False">
                                                    <EditFormSettings Visible="False" />
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataComboBoxColumn FieldName="Category" VisibleIndex="0">
                                                    <PropertiesComboBox ValueType="System.String">
                                                        <Items>
                                                            <dxe:ListEditItem Text="Default" Value="Default"></dxe:ListEditItem>
                                                            <dxe:ListEditItem Text="Secondary" Value="Secondary"></dxe:ListEditItem>
                                                        </Items>

                                                        <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                            <RequiredField ErrorText="Select Category" IsRequired="True" />
                                                        </ValidationSettings>
                                                    </PropertiesComboBox>
                                                    <EditFormSettings Visible="True" />
                                                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataComboBoxColumn FieldName="DP" VisibleIndex="1">
                                                    <PropertiesComboBox DataSourceID="SelectDp" TextField="DP" ValueField="DP_DepositoryID" ValueType="System.String" EnableIncrementalFiltering="True" EnableSynchronization="False">
                                                        <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                            <RequiredField ErrorText="Select DPName" IsRequired="True" />
                                                        </ValidationSettings>
                                                    </PropertiesComboBox>
                                                    <EditFormSettings Visible="True" />
                                                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataComboBoxColumn FieldName="POA" VisibleIndex="3">
                                                    <PropertiesComboBox ValueType="System.String">
                                                        <Items>
                                                            <dxe:ListEditItem Text="Yes" Value="1"></dxe:ListEditItem>
                                                            <dxe:ListEditItem Text="No" Value="0"></dxe:ListEditItem>
                                                        </Items>
                                                        <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                            <RequiredField ErrorText="Select POA" IsRequired="True" />
                                                        </ValidationSettings>
                                                    </PropertiesComboBox>
                                                    <EditFormSettings Visible="True" />
                                                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataTextColumn FieldName="ClientId" VisibleIndex="2">
                                                    <EditFormSettings Visible="True" />
                                                    <PropertiesTextEdit>
                                                        <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                            <RequiredField ErrorText="ClientId Required" IsRequired="True" />
                                                        </ValidationSettings>
                                                    </PropertiesTextEdit>
                                                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="POAName" VisibleIndex="4">
                                                    <EditFormSettings Visible="True" />
                                                    <PropertiesTextEdit>
                                                        <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                            <RequiredField ErrorText="POAName Required" IsRequired="True" />
                                                        </ValidationSettings>
                                                    </PropertiesTextEdit>
                                                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="CreateUser" VisibleIndex="5" Visible="False">
                                                    <EditFormSettings Visible="False" />
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="DPName" VisibleIndex="6" Visible="False">
                                                    <EditFormSettings Visible="False" />
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewCommandColumn VisibleIndex="5" ShowDeleteButton="True" ShowEditButton="True">
                                                    <HeaderTemplate>
                                                        <a href="javascript:void(0);" onclick="gridDp.AddNewRow();">
                                                            <span style="color: #000099; text-decoration: underline">Add New</span>
                                                        </a>
                                                    </HeaderTemplate>
                                                </dxe:GridViewCommandColumn>
                                            </Columns>
                                            <StylesPager>
                                                <Summary Width="100%">
                                                </Summary>
                                            </StylesPager>

                                        </dxe:ASPxGridView>
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
	                                            var Tab8 = page.GetTab(8);
	                                            var Tab9 = page.GetTab(9);
	                                            var Tab10 = page.GetTab(10);
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
	                                            else if(activeTab == Tab8)
	                                            {
	                                                disp_prompt('tab8');
	                                            }
	                                            else if(activeTab == Tab9)
	                                            {
	                                                disp_prompt('tab9');
	                                            }
	                                            else if(activeTab == Tab10)
	                                            {
	                                                disp_prompt('tab10');
	                                            }
	                                            
	                                            }"></ClientSideEvents>
                        <ContentStyle>
                            <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                        </ContentStyle>
                        <LoadingPanelStyle ImageSpacing="6px">
                        </LoadingPanelStyle>
                        <TabStyle Font-Size="12px">
                        </TabStyle>
                    </dxe:ASPxPageControl>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtID" runat="server" Visible="false"></asp:TextBox></td>
            </tr>
        </table>
    </div>
    <asp:SqlDataSource ID="DpDetailsdata" runat="server"
        SelectCommand="select dpd_id AS Id,dpd_cntId as CntId,dpd_accountType AS Category,dpd_dpCode AS DPName,dpd_ClientId AS ClientId, CASE dpd_POA WHEN 1 THEN 'Yes' ELSE 'No' END AS POA,dpd_POAName AS POAName,CreateUser,(select DP_Name+' ['+DP_DepositoryID+']' from Master_DP where DP_DepositoryID=replace(tbl_master_contactDPDetails.dpd_dpCode,char(160),'')) as DP from tbl_master_contactDPDetails where dpd_cntId=@CntId"
        InsertCommand="insert into tbl_master_contactDPDetails(dpd_cntId,dpd_accountType,dpd_dpCode,dpd_clientId,dpd_POA,dpd_POAName,CreateDate,CreateUser) values(@CntId,@Category,@DPName,@ClientId,@POA,@POAName,getdate(),@CreateUser)"
        UpdateCommand="update tbl_master_contactDPDetails set dpd_accountType=@Category,dpd_dpCode=@DPName,dpd_clientId=@ClientId,dpd_POA=@POA,dpd_POAName=@POAName,LastModifyDate=getdate(),LastModifyUser=@CreateUser where dpd_id=@Id"
        DeleteCommand="delete from tbl_master_contactDPDetails where dpd_id=@Id">
        <SelectParameters>
            <asp:SessionParameter Name="CntId" SessionField="KeyVal_InternalID" Type="String" />
        </SelectParameters>
        <InsertParameters>
            <asp:SessionParameter Name="CntId" SessionField="KeyVal_InternalID" Type="String" />
            <asp:Parameter Name="Category" />
            <asp:Parameter Name="DPName" />
            <asp:Parameter Name="ClientId" />
            <asp:Parameter Name="POA" />
            <asp:Parameter Name="POAName" />
            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="Category" />
            <asp:Parameter Name="DPName" />
            <asp:Parameter Name="ClientId" />
            <asp:Parameter Name="POA" />
            <asp:Parameter Name="POAName" />
            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
            <asp:Parameter Name="Id" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" />
        </DeleteParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SelectDp" runat="server" 
        SelectCommand="select DP_DepositoryID,DP_Name+' ['+DP_DepositoryID+']' as DP from Master_DP order by DP_Name"></asp:SqlDataSource>
</asp:Content>
