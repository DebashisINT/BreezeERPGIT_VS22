<%@ Page Language="C#" AutoEventWireup="true" Title="Group" MasterPageFile="~/OMS/MasterPage/Erp.Master" Inherits="ERP.OMS.Management.Master.management_master_Contact_GroupMember" CodeBehind="Contact_GroupMember.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">
        function SignOff() {
            window.parent.SignOff()
        }
        function AddNewRow() {

            GroupMasterGrid.PerformCallback();
        }


        function disp_prompt(name) {
            if (name == "tab0") {
                //alert(name);
                document.location.href = "Contact_general.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "Contact_Correspondence.aspx";
            }
            else if (name == "tab2") {
                //alert(name);
                document.location.href = "Contact_BankDetails.aspx";
            }
            else if (name == "tab3") {
                //alert(name);
                document.location.href = "Contact_DPDetails.aspx";
            }
            else if (name == "tab4") {
                //alert(name);
                document.location.href = "Contact_Document.aspx";
            }
            else if (name == "tab12") {
                //alert(name);
                document.location.href = "Contact_FamilyMembers.aspx";
            }
            else if (name == "tab5") {
                //alert(name);
                document.location.href = "Contact_Registration.aspx";
            }
            else if (name == "tab7") {
                //alert(name);
                //document.location.href="Contact_GroupMember.aspx"; 
            }
            else if (name == "tab8") {
                //alert(name);
                document.location.href = "Contact_Deposit.aspx";
            }
            else if (name == "tab9") {
                //alert(name);
                document.location.href = "Contact_Remarks.aspx";
            }
            else if (name == "tab10") {
                //alert(name);
                document.location.href = "Contact_Education.aspx";
            }
            else if (name == "tab11") {
                //alert(name);
                document.location.href = "contact_brokerage.aspx";
            }
            else if (name == "tab6") {
                //alert(name);
                document.location.href = "contact_other.aspx";
            }
            else if (name == "tab13") {
                document.location.href = "contact_Subscription.aspx";
            }

        }

        $(document).ready(function () {
            var mod = '<%= Session["Contactrequesttype"] %>';
             if (mod == 'customer') {
                 document.getElementById("lnkClose").href = 'CustomerMasterList.aspx';
             }
             else {
                 document.getElementById("lnkClose").href = 'frmContactMain.aspx?requesttype=<%= Session["Contactrequesttype"] %>';
             }

         });

    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <%--<h3>Contact Group Member List</h3>--%>
            <h3>
                <asp:Label ID="lblHeadTitle" runat="server"></asp:Label>
            </h3>
          <%--  <div class="crossBtn"><a href="frmContactMain.aspx?requesttype=<%= Session["Contactrequesttype"] %>"><i class="fa fa-times"></i></a></div>--%>
            <div class="crossBtn"><a id="lnkClose" ><i class="fa fa-times"></i></a></div>
        </div>

    </div>
    <div class="form_main">
        <table width="100%">
            <tr>
                <td class="EHEADER" style="text-align: center">
                    <asp:Label ID="lblName" runat="server" Font-Bold="True"></asp:Label>
                </td>
            </tr>
        </table>
        <table class="TableMain100">
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="7" ClientInstanceName="page" Font-Size="12px" Width="100%">
                        <tabpages>
                            <dxe:TabPage Name="General" Text="General">

                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="CorresPondence" Text="Correspondence">

                                <ContentCollection>
                                    <dxe:ContentControl runat="server"></dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Bank Details" Text="Bank">

                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Visible="false" Name="DP Details" Text="DP">

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


                            <dxe:TabPage Name="Registration" Text="Registration">

                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Other" Visible="false" Text="Other">

                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Group Member" Text="Group">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <table style="width: 100%">
                                          <%-- ....................... Code Added by Sam on 28092016..................................--%>
                                            <tr>
                                                <td style="text-align:left">
                                                    <% if(rights.CanAdd)
                                                       { %>
                                                    <asp:LinkButton ID="LinkButton1" CssClass="btn btn-primary" runat="server" OnClick="LinkButton1_Click">Add New</asp:LinkButton>
                                                    <% } %>
                                                </td>

                                            </tr>


                                         <%-- ....................... Code Added by Sam on 28092016..................................--%>
                                            <tr>
                                                <td align="center">
                                                    <asp:Panel ID="GridPanel" runat="server" Width="100%">
                                                        <dxe:ASPxGridView ID="GroupMasterGrid" ClientInstanceName="GroupMasterGrid" runat="server" AutoGenerateColumns="False" Width="100%"
                                                            DataSourceID="GroupMaster" KeyFieldName="ID" OnCommandButtonInitialize="GroupMasterGrid_CommandButtonInitialize">
                                                            <Settings ShowFilterRow="true" ShowGroupPanel="true" ShowFilterRowMenu="true"/>
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
                                                                <dxe:GridViewCommandColumn VisibleIndex="2" ShowDeleteButton="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="6%">
                                                                    <%-- <DeleteButton Visible="True">
                                                                </DeleteButton>--%>
<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

<CellStyle HorizontalAlign="Center"></CellStyle>
                                                                    <HeaderTemplate>
                                                                        Action
                                                                        <%--<% if (rights.CanAdd)
                                                                                     { %>--%>
                                                                        <%--<asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">Add New</asp:LinkButton>--%>
                                                                        <%--  <% } %>--%>
                                                                    </HeaderTemplate>
                                                                </dxe:GridViewCommandColumn>
                                                            </Columns>
                                                            <SettingsCommandButton>

                                                                <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
<Image AlternateText="Delete" Url="../../../assests/images/Delete.png"></Image>
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
                                                            <Styles>
                                                                <LoadingPanel ImageSpacing="10px">
                                                                </LoadingPanel>
                                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                </Header>
                                                            </Styles>
                                                            <SettingsBehavior ConfirmDelete="True" />
                                                            <SettingsText ConfirmDelete="Confirm delete?" />
                                                            <SettingsPager ShowSeparators="True">
                                                                <FirstPageButton Visible="True">
                                                                </FirstPageButton>
                                                                <LastPageButton Visible="True">
                                                                </LastPageButton>
                                                            </SettingsPager>
                                                        </dxe:ASPxGridView>


                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>

                                                <td align="center">
                                                    <asp:Panel ID="TablePanel" runat="server" Width="100%" Visible="False">
                                                        <table runat="server" width="40%">
                                                            <tr runat="server">
                                                                <td style="text-align: left;" runat="server">
                                                                    <asp:Table ID="TableBind" runat="server" Width="100%"></asp:Table>
                                                                </td>
                                                            </tr>
                                                            <tr runat="server">
                                                                <td style="padding-left: 170px;" runat="server">
                                                                    <asp:Button ID="BtnSave" runat="server" OnClick="BtnSave_Click" Text="Save" CssClass="btn btn-primary" />
                                                                    <asp:Button ID="BtnCancel" runat="server"  Text="Cancel" OnClick="BtnCancel_Click" CssClass="btn btn-danger" />
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
                            <dxe:TabPage Name="Deposit" Visible="false" Text="Deposit">
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
                            <dxe:TabPage Name="Education" Visible="false" Text="Education">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Trad. Prof." Visible="false" Text="Trad.Prof">
                                <%--<TabTemplate ><span style="font-size:x-small">Trad.Prof</span>&nbsp;<span style="color:Red;">*</span> </TabTemplate>--%>
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="FamilyMembers" Visible="false" Text="Family">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Subscription" Visible="false" Text="Subscription">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Cont.Person" Visible="false" Text="Cont.Person">

                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                        </tabpages>
                        <clientsideevents activetabchanged="function(s, e) {
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
	                                            var Tab13=page.GetTab(13);
	                                            
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
	                                            
	                                            }"></clientsideevents>
                        <contentstyle>
                            <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                        </contentstyle>
                        <loadingpanelstyle imagespacing="6px">
                        </loadingpanelstyle>
                    </dxe:ASPxPageControl>
                </td>
            </tr>


        </table>
        <asp:SqlDataSource ID="GroupMaster" runat="server" 
            SelectCommand="select tbl_trans_group.grp_id as ID,tbl_master_groupMaster.Group_Name as GroupName, tbl_master_groupMaster.gpm_Type as GroupType 
                           from tbl_trans_group INNER JOIN V_MultiGroup tbl_master_groupMaster ON tbl_trans_group.grp_groupMaster = tbl_master_groupMaster.Group_Id where tbl_trans_group.grp_contactId = @ContactId"
            DeleteCommand="delete from tbl_trans_group where grp_id=@ID">
            <SelectParameters>
                <asp:SessionParameter Name="ContactId" SessionField="KeyVal_InternalID_New" Type="string" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Type="string" Name="ID" />
            </DeleteParameters>
        </asp:SqlDataSource>
        <asp:TextBox ID="Counter" runat="server" Visible="False"></asp:TextBox>
    </div>
</asp:Content>

