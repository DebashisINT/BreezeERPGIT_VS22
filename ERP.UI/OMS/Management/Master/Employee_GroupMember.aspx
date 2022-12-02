<%@ Page Title="Group" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    Inherits="ERP.OMS.Management.Master.management_master_Employee_GroupMember" CodeBehind="Employee_GroupMember.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web.ASPxTabControl" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web.ASPxClasses" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">
        function disp_prompt(name) {

            if (name == "tab0") {
                //alert(name);
                document.location.href = "Employee_general.aspx?id=" + $("#hdKeyVal").val();
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "Employee_Correspondence.aspx";
            }
            else if (name == "tab2") {
                //alert(name);
                document.location.href = "Employee_Education.aspx";
            }
            else if (name == "tab3") {
                document.location.href = "Employee_Employee.aspx";
                //alert(name);
                //   document.location.href="Employee_DPDetails.aspx"; 
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
                //  document.location.href="Employee_Registration.aspx"; 
            }
            else if (name == "tab7") {
                //alert(name);
                document.location.href = "Employee_EmployeeCTC.aspx";
            }
            else if (name == "tab8") {
                //alert(name);
                document.location.href = "Employee_BankDetails.aspx";
            }
            else if (name == "tab9") {
                //alert(name);
                document.location.href = "Employee_Remarks.aspx";
            }
            else if (name == "tab10") {
                //alert(name);
                //  document.location.href="Employee_Remarks.aspx"; 
            }
            else if (name == "tab11") {
                //alert(name);
                //  document.location.href="Employee_Education.aspx"; 
            }
            else if (name == "tab12") {
                //alert(name);
                //  document.location.href="Employee_Subscription.aspx";
            }
            else if (name == "tab13") {
                //alert(name);
                var keyValue = $("#hdnlanguagespeak").val();
                document.location.href = 'frmLanguageProfi.aspx?id=' + keyValue + '&status=speak';
                //   document.location.href="Employee_Subscription.aspx"; 
            }

            else if (name == "tab14") {
                //alert(name);
                document.location.href = "StatutoryDocumnentDtls.aspx";
            }
            else if (name == "tab15") {
                //alert(name);
                document.location.href = "PFESIDtls.aspx";
            }
            else if (name == "tab16") {
                //alert(name);
                document.location.href = "OtherDetails.aspx";
            }
            else if (name == "tab17") {
                //alert(name);
                document.location.href = "Resignation.aspx";
            }
        }
    </script>

    <div class="panel-title">
        <h3>Employee Group Member</h3>
        <div class="crossBtn">
            <a href="employee.aspx" id="goBackCrossBtn"><i class="fa fa-times"></i></a>

        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td style="text-align: center">
                    <asp:Label ID="lblHeader" runat="server" Font-Bold="True" Font-Size="15px" ForeColor="Navy"
                        Width="819px" Height="18px"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="6" ClientInstanceName="page"
                        Font-Size="12px" Width="100%">
                        <TabPages>
                            <dxe:TabPage Text="General" Name="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="Correspondence" Name="CorresPondence">
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
                            <dxe:TabPage Name="Employee" Text="Employment">
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
                            <dxe:TabPage Name="Family Members" Text="Family">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>

                            <dxe:TabPage Name="Group Member" Text="Group">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 100%; text-align: left" align="left">
                                                    <% if (rights.CanAdd)
                                                       { %>
                                                    <asp:Button ID="BtnAdd" CssClass="btn btn-primary" runat="server" Text="Add New" OnClick="BtnAdd_Click" />
                                                    <%} %></td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Panel ID="GridPanel" runat="server" Width="100%">
                                                        <dxe:ASPxGridView ID="GroupMasterGrid" runat="server" AutoGenerateColumns="False"
                                                            Width="100%" DataSourceID="GroupMaster" KeyFieldName="ID" OnCommandButtonInitialize="GroupMasterGrid_CommandButtonInitialize">
                                                            <Columns>
                                                                <dxe:GridViewDataTextColumn FieldName="ID" VisibleIndex="0" Visible="False">
                                                                    <CellStyle CssClass="gridcellleft" HorizontalAlign="Left">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="GroupName" VisibleIndex="0">
                                                                    <CellStyle CssClass="gridcellleft" HorizontalAlign="Left">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="GroupType" VisibleIndex="1">
                                                                    <CellStyle CssClass="gridcellleft" HorizontalAlign="Left">
                                                                    </CellStyle>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewCommandColumn Width="10%" VisibleIndex="2" ShowDeleteButton="true" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">

                                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                                    <CellStyle HorizontalAlign="Center"></CellStyle>

                                                                    <HeaderTemplate>Actions</HeaderTemplate>
                                                                </dxe:GridViewCommandColumn>
                                                            </Columns>
                                                            <SettingsCommandButton>
                                                                <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                                    <Image AlternateText="Delete" Url="../../../assests/images/Delete.png"></Image>
                                                                </DeleteButton>

                                                                <%--   <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary"></UpdateButton>
                                                <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger"></CancelButton>--%>
                                                            </SettingsCommandButton>
                                                            <Styles>
                                                                <LoadingPanel ImageSpacing="10px">
                                                                </LoadingPanel>
                                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                </Header>
                                                            </Styles>
                                                            <SettingsBehavior ConfirmDelete="True" />
                                                            <SettingsText ConfirmDelete="Are you sure to delete this record?" />
                                                        </dxe:ASPxGridView>
                                                    </asp:Panel>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td style="width: 100%" align="center">
                                                    <asp:Panel ID="TablePanel" runat="server" Width="100%" Visible="False">
                                                        <table width="40%" style="">
                                                            <tr>
                                                                <td style="text-align: left; color: Blue">
                                                                    <asp:Table ID="TableBind" runat="server" Width="100%">
                                                                    </asp:Table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: center">
                                                                    <asp:Button ID="BtnSave" runat="server" OnClick="BtnSave_Click" Text="Save" CssClass="btn btn-primary" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Employee CTC" Text="CTC">
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
                            <dxe:TabPage Name="Remarks" Text="UDF" Visible="false">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="DP Details" Text="DP" Visible="false">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>


                            <dxe:TabPage Name="Registration" Text="Registration" Visible="false">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>



                            <dxe:TabPage Name="Subscription" Text="Subscriptions" Visible="false">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Language" Text="Language">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="StautoryDocumentDetails" Text="Statutory">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                               <dxe:TabPage Name="PFESI" Text="PF/ESI">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="othrdtls" Text="Other Details">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                              <dxe:TabPage Name="RESIGNATION" Text="Resignation">
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
                                                var Tab13 = page.GetTab(13);
                                                var Tab14 = page.GetTab(14);
                                                var Tab15 = page.GetTab(15);
                                                var Tab16 = page.GetTab(16);
                                                var Tab17 = page.GetTab(17);
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
                                    else if(activeTab == Tab13)
	                                            {
	                                                disp_prompt('tab13');
	                                            }
                             else if(activeTab == Tab14)
	                                            {
	                                                disp_prompt('tab14');
	                                            }
                            else if(activeTab == Tab15)
	                                            {
	                                                disp_prompt('tab15');
	                                            }
                             else if(activeTab == Tab16)
	                                            {
	                                                disp_prompt('tab16');
	                                            }
                            else if(activeTab == Tab17)
	                                            {
	                                                disp_prompt('tab17');
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
        <asp:SqlDataSource ID="GroupMaster" runat="server"
            SelectCommand="select tbl_trans_group.grp_id as ID,tbl_master_groupMaster.gpm_Description as GroupName, tbl_master_groupMaster.gpm_Type as GroupType from tbl_trans_group INNER JOIN tbl_master_groupMaster ON tbl_trans_group.grp_groupMaster = tbl_master_groupMaster.gpm_id where tbl_trans_group.grp_contactId = @ContactId"
            DeleteCommand="INSERT INTO tbl_trans_group_Log(grp_id, grp_contactId, grp_groupMaster, grp_groupType, CreateDate, CreateUser, LastModifyDate, LastModifyUser, LogModifyDate, LogModifyUser, LogStatus) SELECT *,getdate(),@User1,'D' FROM tbl_trans_group where grp_id=@ID delete from tbl_trans_group where grp_id=@ID">
            <SelectParameters>
                <asp:SessionParameter Name="ContactId" SessionField="KeyVal_InternalID" Type="string" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Type="string" Name="ID" />
                <asp:SessionParameter Name="User1" SessionField="userid" Type="Decimal" />
            </DeleteParameters>
        </asp:SqlDataSource>
        <asp:TextBox ID="Counter" runat="server" Visible="False"></asp:TextBox>
         <asp:HiddenField ID="hdKeyVal" runat="server" />
    </div>
</asp:Content>
