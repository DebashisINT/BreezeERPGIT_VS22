<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Activities.management_activities_frm_EmployeeRecruitmentSummary" CodeBehind="frm_EmployeeRecruitmentSummary.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function disp_prompt(name) {
            //alert(name);
            if (name == "tab0") {
                document.location.href = "frm_EmployeeRecruitmentSummary.aspx";
            }
            if (name == "tab1") {
                document.location.href = "frm_UserRecruitmentEmployee.aspx?id=NewActivity";
            }
            else if (name == "tab2") {
                document.location.href = "frm_UserRecruitmentEmployee.aspx?id=PendingInterview";
            }
            else if (name == "tab3") {
                document.location.href = "frm_UserRecruitmentEmployee.aspx?id=CompletedInterview";
            }
            else if (name == "tab4") {
                document.location.href = "frm_UserRecruitmentEmployee.aspx?id=SelectedCandidate";
            }
            else if (name == "tab5") {
                document.location.href = "frm_UserRecruitmentEmployee.aspx?id=EliminatedCandidate";
            }

        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_main">
        <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" ClientInstanceName="page"
            Width="100%">
            <TabPages>
                <dxe:TabPage Name="Summary" Text="Summary">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                            <asp:Table ID="TblSummary" runat="server" Width="100%" BorderColor="CornflowerBlue"
                                BorderWidth="1px" CellPadding="0" CellSpacing="0">
                            </asp:Table>
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>
                <dxe:TabPage Name="NewActivity" Text="New Activity">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>
                <dxe:TabPage Name="PendingInterview" Text="Pending Interview">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>
                <dxe:TabPage Name="CompletedInterview" Text="Completed Interview">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>
                <dxe:TabPage Name="SelectedCandidate" Text="Selected Candidate">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>
                <dxe:TabPage Name="EliminatedCandidate" Text="Eliminated Candidate">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>
            </TabPages>
            <ContentStyle>
                <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
            </ContentStyle>
            <LoadingPanelStyle ImageSpacing="6px">
            </LoadingPanelStyle>
            <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                        var activeTab   = page.GetActiveTab();
	                                            var Tab0 = page.GetTab(0);
	                                            var Tab1 = page.GetTab(1);
	                                            var Tab2 = page.GetTab(2);
	                                            var Tab3 = page.GetTab(3);
	                                            var Tab4 = page.GetTab(4);
	                                            var Tab5 = page.GetTab(5);
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
                                            }" />
        </dxe:ASPxPageControl>
    </div>
</asp:Content>
