<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    Inherits="ERP.OMS.Management.Master.management_master_Custodian_general" Codebehind="Custodian_general.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dxe" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <%--  <title>Custodian</title>--%>
    

    

    <script language="javascript" type="text/javascript">
        function SignOff()
        {
            window.parent.SignOff();
        }
        function disp_prompt(name)
        {
            if ( name == "tab0")
            {
                //document.location.href="Custodian_general.aspx"; 
            }
            if ( name == "tab1")
            {
                document.location.href="Custodian_Correspondence.aspx"; 
            }
        }
        function Validate()
        {
            var name=document.getElementById('ASPxPageControl1$txtName').value;
            var shortname=document.getElementById('ASPxPageControl1$txtShortname').value;
            if(name=='')
            {
                alert('Name Required !');
                return false;
            }
            if(shortname=='')
            {
                alert('ShortName Required !');
                return false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>
            <table class="TableMain100">
                <tr>
                    <td>
                        <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" Width="100%" ClientInstanceName="page" Font-Size="12px">
                            <TabPages>
                                <dxe:TabPage Text="General" Name="General">
                                    <ContentCollection>
                                        <dxe:contentcontrol runat="server">
                                            <table style="width: 90%">
                                                <tr>
                                                    <td style="height: 92px">
                                                        <table>
                                                            <tr>
                                                                <td class="EcoheadCon_" style="width: 55px">
                                                                    Name :
                                                                </td>
                                                                <td style="text-align: left; width: 225px;">
                                                                    <asp:TextBox ID="txtName" runat="server" Width="160px" Font-Size="12px"></asp:TextBox>
                                                                </td>
                                                                <td style="width: 83px" class="EcoheadCon_">
                                                                    Short Name :
                                                                </td>
                                                                <td style="text-align: left; width: 202px;">
                                                                     <asp:TextBox ID="txtShortname" runat="server" Width="160px" Font-Size="12px"></asp:TextBox>
                                                                </td>
                                                                <td style="width: 84px" class="EcoheadCon_">
                                                                    SEBI No :
                                                                </td>
                                                                <td>
                                                                     <asp:TextBox ID="txtSebiNo" runat="server" Width="160px" Font-Size="12px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="EcoheadCon_" style="width: 55px">
                                                                    Map In :
                                                                </td>
                                                                <td style="text-align: left; width: 225px;">
                                                                     <asp:TextBox ID="txtMapin" runat="server" Width="160px" Font-Size="12px"></asp:TextBox>
                                                                </td>
                                                                <td style="width: 83px" class="EcoheadCon_">
                                                                    Pan No :
                                                                </td>
                                                                <td style="text-align: left; width: 202px;">
                                                                     <asp:TextBox ID="txtPanNo" runat="server" Width="160px" Font-Size="12px"></asp:TextBox>
                                                                </td>
                                                                <td style="width: 84px" >
                                                                    
                                                                </td>
                                                                <td style="text-align: left">
                                                                   
                                                                </td>
                                                            </tr> 
                                                            <tr>
                                                                <td colspan="6" style="text-align:center">
                                                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnUpdate" Height="24px" Width="59px" OnClick="btnSave_Click" />
                                                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnUpdate"  Height="24px" Width="59px"/>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </dxe:contentcontrol>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Text="CorresPondence" Name="CorresPondence">
                                    <ContentCollection>
                                        <dxe:contentcontrol runat="server">
                                        </dxe:contentcontrol>
                                    </ContentCollection>
                                </dxe:TabPage>
                            </TabPages>
                            <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
	                                            var Tab0 = page.GetTab(0);
	                                            var Tab1 = page.GetTab(1);
	                                            if(activeTab == Tab0)
	                                            {
	                                                disp_prompt('tab0');
	                                            }
	                                            if(activeTab == Tab1)
	                                            {
	                                                disp_prompt('tab1');
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

