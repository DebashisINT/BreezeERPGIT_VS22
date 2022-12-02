<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.management_crm_sales_withiframe" CodeBehind="crm_sales_withiframe.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Src="Headermain.ascx" TagName="Headermain" TagPrefix="uc1" %>
<%@ Register assembly="DevExpress.Web.v15.1" namespace="DevExpress.Web" tagprefix="dx" %>--%>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="Javascript">
        function ShowDetails(ProductTypePath) {

            document.getElementById('GridDiv').style.display = 'none';
            document.getElementById('FrameDiv').style.display = 'inline';
            document.getElementById("ASPxPageControl1_ShowDetails_").style.display = 'inline';
            document.getElementById("ASPxPageControl1_ShowDetails_").src = ProductTypePath;


        }
        function AfterSave() {

            document.getElementById('GridDiv').style.display = 'inline';
            document.getElementById('FrameDiv').style.display = 'none';
            document.getElementById("ASPxPageControl1_ShowDetails_").style.display = 'none';
            //height();
        }

        //function ShowHistory(LeadId) 
        //{
        //  var width=800;
        //  var height=300;
        //  var x=(screen.availHeight-height)/2;
        //  var y=(screen.availWidth-width)/2;
        //  window.open("ShowHistory_Phonecall.aspx?id1="+LeadId,'welcome','width='+ width +',height=' + height + ',top='+x+',left='+y+',menubar=no,status=no,location=no,toolbar=no,scrollbars=yes');
        //}
        function disp_prompt(name) {

            if (name == "tab0") {
                //document.location.href="crm_sales.aspx"; 
            }
            if (name == "tab1") {
                document.location.href = "frmDocument.aspx";
            }
            else if (name == "tab2") {
                document.location.href = "futuresale.aspx";
            }
            else if (name == "tab3") {
                document.location.href = "ClosedSales.aspx";
            }
        }
        function All_CheckedChanged() {
            grid.PerformCallback();
        }
        function Specific_CheckedChanged() {
            grid.PerformCallback();
        }
        //function height() {
        //    window.frameElement.height = document.body.scrollHeight;
        //    window.frameElement.Width = document.body.scrollWidth;
        //}
        function CallForms(val) {

            parent.CallForms1(val);

        }
        function ClosingDHTML() {

            document.getElementById("ASPxPageControl1_ShowDetails_").contentWindow.ClosingDHTML();
        }
    </script>

    <!-- THis code will help us to open the pages in the Modal DHTML window -->
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />

    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>

    <link rel="stylesheet" href="../modalfiles/modal.css" type="text/css" />

    <script type="text/javascript" src="../modalfiles/modal.js"></script>

    <!-- ------------------------------------------------------------->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="TableMain100">
        <tr>
            <td>
                <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" Width="100%"
                    ClientInstanceName="page" CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css"
                    CssPostfix="Office2003_Blue" ImageFolder="~/App_Themes/Office2003 Blue/{0}/">
                    <LoadingPanelStyle ImageSpacing="6px"></LoadingPanelStyle>
                    <ContentStyle>
                        <Border BorderWidth="1px" BorderStyle="Solid" BorderColor="#002D96"></Border>
                    </ContentStyle>
                    <TabStyle Font-Size="12px"></TabStyle>
                    <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
	                                            var Tab0 = page.GetTab(0);
	                                            var Tab1 = page.GetTab(1);
	                                            var Tab2 = page.GetTab(2);
	                                            var Tab3 = page.GetTab(3);
	                                            
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
	                                            }"></ClientSideEvents>
                    <TabPages>
                        <dxe:TabPage Name="New Sales" Text="New Sales">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                    <div id="GridDiv">
                                        <table class="TableMain100">
                                            <tbody>
                                                <tr>
                                                    <td style="text-align: left" colspan="2">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <asp:RadioButton runat="server" GroupName="a" Visible="False" ID="Lrd" __designer:wfdid="w3"></asp:RadioButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label runat="server" Text="From Lead Data" Font-Size="X-Small" ForeColor="Blue" ID="Label4" Visible="False" __designer:wfdid="w4"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton runat="server" GroupName="a" Visible="False" ID="Erd" Checked="True" __designer:wfdid="w5"></asp:RadioButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label runat="server" Text="From Existing Customer Data" Font-Size="X-Small" ForeColor="Blue" ID="Label5" Visible="False" __designer:wfdid="w6"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxGridView runat="server" CssPostfix="Office2003_Blue" Width="99%" ID="SalesGrid" DataSourceID="SalesDataSource" AutoGenerateColumns="False" ClientInstanceName="grid" CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css" __designer:wfdid="w7" OnCustomCallback="SalesGrid_CustomCallback">
                                                            <Images ImageFolder="~/App_Themes/Office2003 Blue/{0}/">
                                                                <ExpandedButton Height="12px" Width="11px" Url="~/App_Themes/Office2003 Blue/GridView/gvExpandedButton.png"></ExpandedButton>

                                                                <CollapsedButton Height="12px" Width="11px" Url="~/App_Themes/Office2003 Blue/GridView/gvCollapsedButton.png"></CollapsedButton>

                                                                <DetailCollapsedButton Height="12px" Width="11px" Url="~/App_Themes/Office2003 Blue/GridView/gvCollapsedButton.png"></DetailCollapsedButton>

                                                                <DetailExpandedButton Height="12px" Width="11px" Url="~/App_Themes/Office2003 Blue/GridView/gvExpandedButton.png"></DetailExpandedButton>

                                                                <FilterRowButton Height="13px" Width="13px"></FilterRowButton>
                                                            </Images>

                                                            <Styles CssPostfix="Office2003_Blue" CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css">
                                                                <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>

                                                                <Cell CssClass="gridcellleft"></Cell>

                                                                <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                                                            </Styles>

                                                            <Settings ShowGroupPanel="True" ShowFooter="True" ShowStatusBar="Visible"></Settings>

                                                            <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True">
                                                                <FirstPageButton Visible="True"></FirstPageButton>

                                                                <LastPageButton Visible="True"></LastPageButton>
                                                            </SettingsPager>
                                                            <Columns>
                                                                <dxe:GridViewCommandColumn Visible="False" VisibleIndex="0"></dxe:GridViewCommandColumn>
                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="Status"></dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="LeadId" Width="18%"></dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="1" FieldName="Name" Width="18%"></dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="2" FieldName="Address" Width="18%"></dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="3" FieldName="Phone" Width="18%"></dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="4" FieldName="ProductType" Width="14%"></dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="4" FieldName="ProductTypePath"></dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="4" FieldName="Id">
                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="5" FieldName="Amount"></dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="4" FieldName="Product"></dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="NextVisit" Width="18%"></dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn VisibleIndex="7">
                                                                    <DataItemTemplate>
                                                                        <a href="#" onclick="ShowDetails('<%#Eval("ProductTypePath") %>')">Show</a>

                                                                    </DataItemTemplate>

                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn VisibleIndex="6" Caption="History">
                                                                    <DataItemTemplate>
                                                                        <a href="#" onclick="CallForms('History')">History</a>

                                                                    </DataItemTemplate>

                                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                </dxe:GridViewDataTextColumn>
                                                            </Columns>
                                                        </dxe:ASPxGridView>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div style="display: none" id="FrameDiv">
                                        <iframe runat="server" id="ShowDetails_" frameborder="0" width="100%" scrolling="no"></iframe>
                                    </div>
                                </dxe:ContentControl>
                            </ContentCollection>
                        </dxe:TabPage>
                        <dxe:TabPage Name="Document Collection" Text="Document Collection">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                </dxe:ContentControl>
                            </ContentCollection>
                        </dxe:TabPage>
                        <dxe:TabPage Name="Future Sales" Text="Future Sales">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                </dxe:ContentControl>
                            </ContentCollection>
                        </dxe:TabPage>
                        <dxe:TabPage Name="Closed Sales" Text="Closed Sales">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                </dxe:ContentControl>
                            </ContentCollection>
                        </dxe:TabPage>
                    </TabPages>
                </dxe:ASPxPageControl>
            </td>
        </tr>
    </table>
    <asp:SqlDataSource ID="SalesDataSource" runat="server" ></asp:SqlDataSource>
</asp:Content>
