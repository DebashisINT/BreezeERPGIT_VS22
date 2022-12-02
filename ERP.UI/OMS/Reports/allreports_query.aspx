<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_allreports_query" Codebehind="allreports_query.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">
    function SignOff()
    {
        window.parent.SignOff();
    }
    function height()
    {
        if(document.body.scrollHeight>=350)
            window.frameElement.height = document.body.scrollHeight;
        else
            window.frameElement.height = '350px';
        window.frameElement.Width = document.body.scrollWidth;
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="text-align:center">
    <asp:Panel ID="Panelmain" runat="server" Visible="true">
        <table id="tbl_main" class="login" cellspacing="0" cellpadding="0" width="410">
            <tbody>
                <tr>
                    <td class="lt1" style="height: 22px">
                        <h5>
                            Export Report As You Need
                        </h5>
                    </td>
                </tr>
                <tr>
                    <td class="lt brdr" style="height: 280px">
                        <table cellspacing="0" cellpadding="0" align="center">
                            <tbody>
                                <tr>
                                    <td class="lt">
                                        <table class="width100per" cellspacing="12" cellpadding="0">
                                            <tbody>
                                              
                                                
                                                <tr>
                                                     <td style="height: 22px">
                                                                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                                                                    </asp:ScriptManager> 
                                                    </td>
                                                    <tr>
                                                    <td>
                                                     <tr>
                                                                <td style="width: 6px">
                                                                    &nbsp;<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:DropDownList ID="ddlBank" runat="server" Width="345px" OnSelectedIndexChanged="ddlBank_SelectedIndexChanged1"
                                                                                AutoPostBack="True">
                                                                            </asp:DropDownList><br />
                                                                            
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                    </td>
                                                    
                                                    </tr>
                                                    <td class="lt" style="width: 278px; height: 22px;">
                                                        <table>
                                                           
                                                            <tr>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="BtnSave" runat="server" Text="Export" CssClass="btn" 
                                                                        Width="84px" OnClick="BtnSave_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                
                                               
                                                
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
    </div>
</asp:Content>