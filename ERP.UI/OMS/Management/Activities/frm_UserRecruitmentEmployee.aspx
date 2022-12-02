<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Activities.management_activities_frm_UserRecruitmentEmployee" CodeBehind="frm_UserRecruitmentEmployee.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">

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
                page.SetActiveTab(page.GetTab(3));
            }
            else if (name == "tab4") {
                document.location.href = "frm_UserRecruitmentEmployee.aspx?id=SelectedCandidate";
            }
            else if (name == "tab5") {
                document.location.href = "frm_UserRecruitmentEmployee.aspx?id=EliminatedCandidate";
            }

        }



        function ClickOnMoreInfo(keyValue) {
            var url = 'frm_UserRecruitmentEmployee_Detail.aspx?id=' + keyValue;
            OnMoreInfoClick(url, "Activity", '950px', '450px', "Y");

        }
        function OnMoreInfoClickCandiDetail(keyValue) {
            var url = 'frm_CandidateDetail.aspx?id=' + keyValue;
            OnMoreInfoClick(url, "Activity", '950px', '450px', "N");

        }
        function callback() {
            window.location.reload();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" ClientInstanceName="page"
        Width="100%" OnActiveTabChanged="ASPxPageControl1_ActiveTabChanged">
        <TabPages>
            <dxe:TabPage Name="Summary" Text="Summary">
                <ContentCollection>
                    <dxe:ContentControl runat="server">
                    </dxe:ContentControl>
                </ContentCollection>
            </dxe:TabPage>
            <dxe:TabPage Name="NewActivity" Text="New Activity">
                <ContentCollection>
                    <dxe:ContentControl runat="server">
                        <dxe:ASPxGridView ID="GridNewActivity" runat="server" Width="100%" AutoGenerateColumns="False"
                            KeyFieldName="Id">
                            <SettingsEditing Mode="PopupEditForm"></SettingsEditing>
                            <Styles>
                                <LoadingPanel ImageSpacing="10px">
                                </LoadingPanel>
                                <Header SortingImageSpacing="5px" ImageSpacing="5px">
                                </Header>
                            </Styles>
                            <Settings ShowGroupPanel="True"></Settings>
                            <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn"></SettingsBehavior>
                            <SettingsPager PageSize="20" NumericButtonCount="20" AlwaysShowPager="True">
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </SettingsPager>
                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="Id" ReadOnly="True" Visible="False" VisibleIndex="0">
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="ActNo" VisibleIndex="0">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Company" VisibleIndex="1">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Branch" VisibleIndex="2">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Department" VisibleIndex="3">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Vacancies" VisibleIndex="4">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="UserName" VisibleIndex="5">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="JobResponsibility" VisibleIndex="6">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Details" VisibleIndex="7">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Container.KeyValue %>')">More Info...</a>
                                    </DataItemTemplate>
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                        </dxe:ASPxGridView>
                    </dxe:ContentControl>
                </ContentCollection>
            </dxe:TabPage>
            <dxe:TabPage Name="PendingInterview" Text="Pending Interview">
                <ContentCollection>
                    <dxe:ContentControl runat="server">
                        <dxe:ASPxGridView ID="GridNewActivity1" runat="server" Width="100%" AutoGenerateColumns="False"
                            KeyFieldName="Id">
                            <SettingsEditing Mode="PopupEditForm"></SettingsEditing>
                            <Styles>
                                <LoadingPanel ImageSpacing="10px">
                                </LoadingPanel>
                                <Header SortingImageSpacing="5px" ImageSpacing="5px">
                                </Header>
                            </Styles>
                            <Settings ShowGroupPanel="True"></Settings>
                            <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn"></SettingsBehavior>
                            <SettingsPager PageSize="20" NumericButtonCount="20" ShowSeparators="True" AlwaysShowPager="True">
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </SettingsPager>
                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="Id" ReadOnly="True" Visible="False" VisibleIndex="0">
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="ActNo" VisibleIndex="0">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Company" VisibleIndex="1">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Branch" VisibleIndex="2">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Department" VisibleIndex="3">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Vacancies" VisibleIndex="4">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="UserName" VisibleIndex="5">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="JobResponsibility" VisibleIndex="6">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Details" VisibleIndex="7">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')">More Info...</a>
                                    </DataItemTemplate>
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                        </dxe:ASPxGridView>
                    </dxe:ContentControl>
                </ContentCollection>
            </dxe:TabPage>
            <dxe:TabPage Name="CompletedInterview" Text="Completed Interview">
                <ContentCollection>
                    <dxe:ContentControl runat="server">
                        <dxe:ASPxGridView ID="GridCompltIntervw" runat="server" Width="100%" AutoGenerateColumns="False"
                            KeyFieldName="Id">
                            <SettingsEditing Mode="PopupEditForm"></SettingsEditing>
                            <Styles>
                                <LoadingPanel ImageSpacing="10px">
                                </LoadingPanel>
                                <Header SortingImageSpacing="5px" ImageSpacing="5px">
                                </Header>
                            </Styles>
                            <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn"></SettingsBehavior>
                            <SettingsPager PageSize="20" NumericButtonCount="20" ShowSeparators="True" AlwaysShowPager="True">
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </SettingsPager>
                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="Id" ReadOnly="True" Visible="False" VisibleIndex="0">
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="ActNo" VisibleIndex="0">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="CandidateName" Caption="Candidate Name"
                                    VisibleIndex="1">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Company" VisibleIndex="2">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Branch" VisibleIndex="3">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Department" VisibleIndex="4">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="UserName" Caption="Assigned By" VisibleIndex="5">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="JobResponsibility" VisibleIndex="6">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Details" VisibleIndex="7">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')">More Info...</a>
                                    </DataItemTemplate>
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                        </dxe:ASPxGridView>
                    </dxe:ContentControl>
                </ContentCollection>
            </dxe:TabPage>
            <dxe:TabPage Name="SelectedCandidate" Text="Selected Candidate">
                <ContentCollection>
                    <dxe:ContentControl runat="server">
                        <dxe:ASPxGridView ID="GridCompltIntervw1" runat="server" Width="100%" AutoGenerateColumns="False"
                            KeyFieldName="RecruitmentId">
                            <SettingsEditing Mode="PopupEditForm"></SettingsEditing>
                            <Styles>
                                <LoadingPanel ImageSpacing="10px">
                                </LoadingPanel>
                                <Header SortingImageSpacing="5px" ImageSpacing="5px">
                                </Header>
                            </Styles>
                            <Settings ShowGroupPanel="True"></Settings>
                            <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn"></SettingsBehavior>
                            <SettingsPager PageSize="20" NumericButtonCount="20" ShowSeparators="True" AlwaysShowPager="True">
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </SettingsPager>
                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="Id" ReadOnly="True" Visible="False" VisibleIndex="0">
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="ActNo" VisibleIndex="0">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="CandidateName" Caption="Candidate Name"
                                    VisibleIndex="1">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Company" VisibleIndex="2">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Branch" VisibleIndex="3">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Department" VisibleIndex="4">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="UserName" Caption="Assigned By" VisibleIndex="5">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="JobResponsibility" VisibleIndex="6">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Details" VisibleIndex="7">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" onclick="OnMoreInfoClickCandiDetail('<%# Container.KeyValue %>')">More Info...</a>
                                    </DataItemTemplate>
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                        </dxe:ASPxGridView>
                    </dxe:ContentControl>
                </ContentCollection>
            </dxe:TabPage>
            <dxe:TabPage Name="EliminatedCandidate" Text="Eliminated Candidate">
                <ContentCollection>
                    <dxe:ContentControl runat="server">
                        <dxe:ASPxGridView ID="GridEliminated" runat="server" Width="100%" AutoGenerateColumns="False"
                            KeyFieldName="Id">
                            <SettingsEditing Mode="PopupEditForm"></SettingsEditing>
                            <Styles>
                                <LoadingPanel ImageSpacing="10px">
                                </LoadingPanel>
                                <Header SortingImageSpacing="5px" ImageSpacing="5px">
                                </Header>
                            </Styles>
                            <Settings ShowGroupPanel="True"></Settings>
                            <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn"></SettingsBehavior>
                            <SettingsPager PageSize="20" NumericButtonCount="20" ShowSeparators="True" AlwaysShowPager="True">
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </SettingsPager>
                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="Id" ReadOnly="True" Visible="False" VisibleIndex="0">
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="ActNo" VisibleIndex="0">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="CandidateName" Caption="Candidate Name"
                                    VisibleIndex="1">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Company" VisibleIndex="2">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Branch" VisibleIndex="3">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Department" VisibleIndex="4">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="UserName" VisibleIndex="5">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="JobResponsibility" VisibleIndex="6">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Details" VisibleIndex="7">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')">More Info...</a>
                                    </DataItemTemplate>
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                        </dxe:ASPxGridView>
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
</asp:Content>
