<%@ Page Title="Employee" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_HRCostEmployee" CodeBehind="HRCostEmployee.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">




    <script language="javascript" type="text/javascript">
        function disp_prompt(name) {
            if (name == "tab0") {
                //alert(name);
                document.location.href = "HrCost.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "HRCostBranch.aspx";
            }
            else if (name == "tab2") {
                //alert(name);
                //document.location.href="HRCostEmployee.aspx"; 
            }
        }
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Cost Centers/Departments Information</h3>
            <div class="crossBtn"><a href="HRCostCenter.aspx"><i class="fa fa-times"></i></a></div>
        </div>

    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="2" Width="100%"
                        ClientInstanceName="page" Font-Size="12px">
                        <TabPages>
                            <dxe:TabPage Text="Update" Name="Update">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="Branch" Name="Branch">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Employee" Text="Employee">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <dxe:ASPxGridView ID="EmployeeGrid" runat="server" AutoGenerateColumns="False"
                                            DataSourceID="EmployeeSource" Width="100%">
                                            <Columns>
                                                <dxe:GridViewDataTextColumn FieldName="id" Visible="False" VisibleIndex="0">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Costcenter" VisibleIndex="0" Width="50%">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="CostCenterType" VisibleIndex="1" Width="50%">
                                                </dxe:GridViewDataTextColumn>
                                            </Columns>
                                            <SettingsSearchPanel Visible="True" />
                                            <Settings ShowFilterRow="true" ShowFilterRowMenu ="true" />
                                            <Styles>
                                                <LoadingPanel ImageSpacing="10px">
                                                </LoadingPanel>
                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                </Header>
                                            </Styles>

                                        </dxe:ASPxGridView>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                        </TabPages>
                        <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
	                                            var Tab0 = page.GetTab(0);
	                                            var Tab1 = page.GetTab(1);
	                                            var Tab2 = page.GetTab(2);	                                            
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
	                                            
	                                            }"></ClientSideEvents>
                        <ContentStyle>
                            <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                        </ContentStyle>
                        <LoadingPanelStyle ImageSpacing="6px">
                        </LoadingPanelStyle>
                    </dxe:ASPxPageControl>
                </td>
            </tr>
            <tr>
                <td style="height: 8px">
                    <table style="width: 100%;">
                        <tr>
                            <td align="right" style="width: 843px"></td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 843px"></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="EmployeeSource" runat="server" 
            SelectCommand="select cnt_internalId as id,cnt_firstName  as Costcenter,cost_costCenterType as CostCenterType from tbl_master_contact ,tbl_master_costCenter where cnt_internalId=cost_description"></asp:SqlDataSource>
    </div>
</asp:Content>
