<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    Inherits="ERP.OMS.Management.Master.management_master_Employee_DPDetails" CodeBehind="Employee_DPDetails.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dxe" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web.ASPxTabControl" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web.ASPxClasses" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<title>DP Details</title>--%>
    <!--___________________These files are for List Items__________________________-->
    <link href="../../CSS/style.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">
        function disp_prompt(name) {
            //var ID = document.getElementById(txtID);
            if (name == "tab0") {
                //alert(name);
                document.location.href = "Employee_general.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "Employee_Correspondence.aspx";
            }
            else if (name == "tab2") {
                //alert(name);
                document.location.href = "Employee_BankDetails.aspx";
            }
            else if (name == "tab3") {
                //alert(name);
                //document.location.href="Employee_DPDetails.aspx"; 
            }
            else if (name == "tab4") {
                //alert(name);
                document.location.href = "Employee_Document.aspx";
            }
            else if (name == "tab5") {
                //alert(name);
                document.location.href = "Employee_FamilyMembers.aspx";
            }
            else if (name == "tab6") {
                //alert(name);
                document.location.href = "Employee_Registration.aspx";
            }
            else if (name == "tab7") {
                //alert(name);
                document.location.href = "Employee_GroupMember.aspx";
            }
            else if (name == "tab8") {
                //alert(name);
                document.location.href = "Employee_Employee.aspx";
            }
            else if (name == "tab9") {
                //alert(name);
                document.location.href = "Employee_EmployeeCTC.aspx";
            }
            else if (name == "tab10") {
                //alert(name);
                document.location.href = "Employee_Remarks.aspx";
            }
            else if (name == "tab11") {
                //alert(name);
                document.location.href = "Employee_Education.aspx";
            }
            else if (name == "tab12") {
                //alert(name);
                document.location.href = "Employee_Subscription.aspx";
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

     <div class="panel-heading">
        <div class="panel-title">
            <h3>Employee DP List</h3>
             <div class="crossBtn"><a href="Employee_general.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>

    <div class="form_main" >
        <table class="TableMain100">
            <tr>
                <td style="text-align: center">
                    <asp:Label ID="lblHeader" runat="server" Font-Bold="True" Font-Size="15px" ForeColor="Navy"
                        Width="819px" Height="18px"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="3" ClientInstanceName="page">
                        <TabPages>
                            <dxe:TabPage Text="General" Name="General">
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
                            <dxe:TabPage Name="Bank Details" Text="Bank">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="DP Details" Text="DP">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <dxe:ASPxGridView ID="DpDetailsGrid" runat="server" AutoGenerateColumns="False"
                                            DataSourceID="DpDetailsdata" KeyFieldName="Id" ClientInstanceName="gridDp" Width="100%"
                                            Font-Size="12px" OnHtmlEditFormCreated="DpDetailsGrid_HtmlEditFormCreated" OnRowInserting="DpDetailsGrid_RowInserting"
                                            OnRowUpdating="DpDetailsGrid_RowUpdating" OnRowValidating="DpDetailsGrid_RowValidating">
                                            <Templates>
                                                <EditForm>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="text-align: right; width: 30%">
                                                                <span class="Ecoheadtxt" style="color: Black">Category :</span>
                                                            </td>
                                                            <td style="text-align: left">
                                                                <dxe:ASPxComboBox ID="comboCategory" EnableIncrementalFiltering="True" EnableSynchronization="False"
                                                                    Value='<%#Bind("Category") %>' runat="server" ValueType="System.String" Width="285px">
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
                                                                <asp:TextBox ID="txtDPName" CssClass="EcoheadCon" Text='<%#Bind("DP") %>' runat="server"
                                                                    Width="279px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right; width: 30%">
                                                                <span class="Ecoheadtxt" style="color: Black">ClientID :</span>
                                                            </td>
                                                            <td style="text-align: left">
                                                                <asp:TextBox ID="txtClientID" CssClass="EcoheadCon" Text='<%#Bind("ClientId") %>'
                                                                    runat="server" Width="279px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right; width: 30%">
                                                                <span class="Ecoheadtxt" style="color: Black">POA :</span>
                                                            </td>
                                                            <td style="text-align: left">
                                                                <dxe:ASPxComboBox ID="comboPOA" EnableIncrementalFiltering="True" EnableSynchronization="False"
                                                                    Value='<%#Bind("POA") %>' runat="server" ValueType="System.String" Width="285px">
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
                                                                <asp:TextBox ID="txtPoaName" CssClass="EcoheadCon" Text='<%#Bind("POAName") %>' runat="server"
                                                                    Width="279px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" style="text-align: center">
                                                                <div style="text-align: right; padding: 2px 2px 2px 2px">
                                                                    <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                                        runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                    <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                                        runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" style="display: none">
                                                                <asp:TextBox ID="txtDPName_hidden" CssClass="EcoheadCon" Text='<%#Bind("DPName") %>'
                                                                    runat="server" Width="279px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EditForm>
                                                <TitlePanel>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td align="center">
                                                                <span class="Ecoheadtxt">Add/Modify DP Detail.</span>
                                                            </td>

                                                        </tr>
                                                    </table>
                                                </TitlePanel>
                                            </Templates>
                                            <SettingsDataSecurity AllowDelete="False" AllowEdit="False" />
                                            <SettingsText PopupEditFormCaption="Add/Modify DP Details" ConfirmDelete="Confirm delete?" />
                                            <Styles>
                                                <LoadingPanel ImageSpacing="10px">
                                                </LoadingPanel>
                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                </Header>
                                            </Styles>
                                            <Settings ShowGroupPanel="false" ShowFooter="True" ShowStatusBar="Visible" ShowTitlePanel="True" />
                                            <SettingsBehavior ConfirmDelete="True" />
                                            <SettingsPager NumericButtonCount="20" PageSize="20">
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
                                                    <PropertiesComboBox DataSourceID="SelectDp" TextField="DP" ValueField="DP_DepositoryID"
                                                        ValueType="System.String" EnableIncrementalFiltering="True" EnableSynchronization="False">
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
                                                <dxe:GridViewCommandColumn VisibleIndex="5" ShowDeleteButton="true" ShowEditButton="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                    <%-- <DeleteButton Visible="True">
                                                        </DeleteButton>
                                                        <EditButton Visible="True">
                                                        </EditButton>--%>
<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

<CellStyle HorizontalAlign="Center"></CellStyle>
                                                    <HeaderTemplate>
                                                        <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                                          { %>
                                                        <a href="javascript:void(0);" onclick="gridDp.AddNewRow();"><span>Add New</span> </a>
                                                        <%} %>
                                                    </HeaderTemplate>
                                                </dxe:GridViewCommandColumn>
                                            </Columns>
                                            <SettingsCommandButton>
                                                <EditButton Image-Url="/assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                                                    <Image AlternateText="Edit" Url="/assests/images/Edit.png"></Image>

                                                    <Styles>
                                                        <Style CssClass="pad"></Style>
                                                    </Styles>
                                                </EditButton>
                                                <DeleteButton Image-Url="/assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                    <Image AlternateText="Delete" Url="/assests/images/Delete.png"></Image>
                                                </DeleteButton>
                                                <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary">
                                                    <Styles>
                                                        <Style CssClass="btn btn-primary"></Style>
                                                    </Styles>
                                                </UpdateButton>
                                                <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger">
                                                    <Styles>
                                                        <Style CssClass="btn btn-danger"></Style>
                                                    </Styles>
                                                </CancelButton>
                                            </SettingsCommandButton>
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
                            <dxe:TabPage Name="Family Members" Text="Family">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Registration" Text="Registration">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Group Member" Text="Group">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Employee" Text="Employment">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Employee CTC" Text="CTC">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Remarks" Text="Remarks">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Education" Text="Education">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Subscription" Text="Subscription">
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
	                                            var Tab11 = page.GetTab(11);
	                                            var Tab12 = page.GetTab(12);
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
	                                            else if(activeTab == Tab11)
	                                            {
	                                                disp_prompt('tab11');
	                                            }
	                                             else if(activeTab == Tab12)
	                                            {
	                                                disp_prompt('tab12');
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
                <td>
                    <asp:TextBox ID="txtID" runat="server" Visible="false"></asp:TextBox></td>
            </tr>
        </table>
    </div>
    <asp:SqlDataSource ID="DpDetailsdata" runat="server"
        SelectCommand="select dpd_id AS Id,dpd_cntId as CntId,dpd_accountType AS Category,dpd_dpCode AS DPName,dpd_ClientId AS ClientId, CASE dpd_POA WHEN 1 THEN 'Yes' ELSE 'No' END AS POA,dpd_POAName AS POAName,CreateUser,(select DP_Name+' ['+DP_DepositoryID+']' from Master_DP where DP_DepositoryID=replace(tbl_master_contactDPDetails.dpd_dpCode,char(160),'')) as DP from tbl_master_contactDPDetails where dpd_cntId=@CntId"
        InsertCommand="insert into tbl_master_contactDPDetails(dpd_cntId,dpd_accountType,dpd_dpCode,dpd_clientId,dpd_POA,dpd_POAName,CreateDate,CreateUser) values(@CntId,@Category,@DPName,@ClientId,@POA,@POAName,getdate(),@CreateUser)"
        UpdateCommand="INSERT INTO tbl_master_contactDPDetails_Log(dpd_id, dpd_cntId, dpd_accountType, dpd_dpCode, dpd_ClientId, dpd_POA, dpd_POAName, CreateDate, CreateUser, LastModifyDate, LastModifyUser, LogModifyDate, LogModifyUser, LogStatus) select *,getdate(),@CreateUser,'M' from tbl_master_contactDPDetails where dpd_id=@Id update tbl_master_contactDPDetails set dpd_accountType=@Category,dpd_dpCode=@DPName,dpd_clientId=@ClientId,dpd_POA=@POA,dpd_POAName=@POAName,LastModifyDate=getdate(),LastModifyUser=@CreateUser where dpd_id=@Id"
        DeleteCommand="INSERT INTO tbl_master_contactDPDetails_Log(dpd_id, dpd_cntId, dpd_accountType, dpd_dpCode, dpd_ClientId, dpd_POA, dpd_POAName, CreateDate, CreateUser, LastModifyDate, LastModifyUser, LogModifyDate, LogModifyUser, LogStatus) select *,getdate(),@User,'D' from tbl_master_contactDPDetails where dpd_id=@Id delete from tbl_master_contactDPDetails where dpd_id=@Id">
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
            <asp:SessionParameter Name="User" SessionField="userid" Type="Int32" />
        </DeleteParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SelectDp" runat="server" 
        SelectCommand="select DP_DepositoryID,DP_Name+' ['+DP_DepositoryID+']' as DP from Master_DP order by DP_Name"></asp:SqlDataSource>
</asp:Content>

