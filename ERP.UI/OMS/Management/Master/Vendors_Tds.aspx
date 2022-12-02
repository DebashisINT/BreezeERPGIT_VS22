<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Vendors_Tds.aspx.cs" Inherits="ERP.OMS.Management.Master.Vendors_Tds"
     MasterPageFile="~/OMS/MasterPage/Erp.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .savebtn {
        margin-top: 5px;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function disp_prompt(name) {
            if (name == "tab0") {
                //alert(name);
                document.location.href = "HRrecruitmentagent_general.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "HRrecruitmentagent_ContactPerson.aspx";
            }
            else if (name == "tab2") {
                //alert(name);
                document.location.href = "HRrecruitmentagent_Correspondence.aspx";
            }
            else if (name == "tab3") {
                //alert(name);
                document.location.href = "HRrecruitmentagent_BankDetails.aspx";
            }
            //else if (name == "tab4") {
            //    //alert(name);
            //    document.location.href = "HRrecruitmentagent_DPDetails.aspx";
            //}
            else if (name == "tab4") {
                //alert(name);
                document.location.href = "HRrecruitmentagent_Document.aspx";
            }
            else if (name == "tab5") {
                //alert(name);
                document.location.href = "HRrecruitmentagent_Registration.aspx";
            }
            else if (name == "tab6") {
                //alert(name);
                document.location.href="HRrecruitmentagent_GroupMember.aspx"; 
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title" >
           <h3>Vendors/Service Providers</h3>
         <div class="crossBtn"><a href="HRrecruitmentagent.aspx"><i class="fa fa-times"></i></a></div>
        </div>   
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td style="text-align: center">
                    <asp:Label ID="lblHeader" runat="server" Font-Bold="True" Font-Size="15px" ForeColor="Navy"
                        Width="819px" Height="18px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HiddenField ID="HdMode" runat="server" />
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="7" ClientInstanceName="page"
                        Font-Size="12px" Width="100%">
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
                            <dxe:TabPage Name="Documents" Text="Documents">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Registration" Text="Registration">
                                <contentcollection>
                                                            <dxe:ContentControl runat="server">
                                                            </dxe:ContentControl>
                                                        </contentcollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="GroupMember" Text="Group Member">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                      
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>

                             <dxe:TabPage Name="TDS" Text="TDS">
                                <contentcollection>
                                    <dxe:ContentControl runat="server">
                                        
                                    
                                        <div class="row">
                                            <div class="col-md-3 hide">
                                                <label>Deductees Type</label>

                                                <dxe:ASPxComboBox ID="aspxDeductees" ClientInstanceName="caspxDeductees" runat="server" SelectedIndex="0"    
                                                ValueType="System.String" Width="100%" EnableSynchronization="True" >
                                                   <Items>
                                                    <dxe:ListEditItem Text="(0020)Company Deductees" Value="(0020)Company Deductees" />
                                                    <dxe:ListEditItem Text="(0021)Non Company Deductees" Value="(0021)Non Company Deductees" />
                                                </Items>
                                            </dxe:ASPxComboBox>
                                            </div>
                                             <div class="col-md-3">
                                                <label>Deductees Type</label>

                                                <dxe:ASPxComboBox ID="aspxDeducteesNew" ClientInstanceName="caspxDeducteesNew" runat="server" SelectedIndex="0"    
                                                ValueType="System.String" Width="100%" EnableSynchronization="True" >
                                                  <%-- <Items>
                                                    <dxe:ListEditItem Text="(0020)Company Deductees" Value="(0020)Company Deductees" />
                                                    <dxe:ListEditItem Text="(0021)Non Company Deductees" Value="(0021)Non Company Deductees" />
                                                </Items>--%>
                                            </dxe:ASPxComboBox>
                                            </div>
                                            <div class="clear"></div>
                                             <div class="col-md-3">

                                                 <asp:Button ID="BtnSave" runat="server" Text="Save"  CssClass="btn btn-primary savebtn" OnClick="BtnSave_Click"  />
                                             </div>

                                        </div>
                                          

                                    </dxe:ContentControl>
                                </contentcollection>
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
