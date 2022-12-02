<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_utilities_ChangeFavPicture" Codebehind="ChangeFavPicture.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/javascript">
        function PicChange(obj)
        {
            var URL;
            //URL='../management/ShowPicture.aspx?id='+obj;
            URL = 'ShowPicture.aspx?id=' + obj;
            OnMoreInfoClick(URL,"Change Picture",'480px','300px',"Y");
            //editwin=dhtmlmodal.open("Editbox", "iframe", URL,"Change Picture" , "width=480px,height=300px,center=1,resize=1,scrolling=2,top=500", "recal")                     
        }  
        function callback()
        {
            document.getElementById('btnChangePic').click();
        }   
        function height()
        {
            if(document.body.scrollHeight>=500)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '500px';
            window.frameElement.Width = document.body.scrollWidth;
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="panel-heading">
       <div class="panel-title">
           <h3>Manage Favourite Menu</h3>
       </div>

   </div> 
     <div class="form_main">
          
            <table class="TableMain100">
                
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="grdUpdateFavMenu" runat="server" DataKeyNames="FavouriteMenu_ID"
                                    CssClass="gridcellleft" CellPadding="4" ForeColor="#333333" GridLines="None"
                                    BorderWidth="1px" BorderColor="#507CD1" Width="100%" AutoGenerateColumns="False"
                                    ShowFooter="True" OnRowDeleting="grdUpdateFavMenu_RowDeleting">
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                                    <EditRowStyle BackColor="#E59930" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <HeaderStyle Font-Bold="False" ForeColor="Black" CssClass="EHEADER" BorderColor="AliceBlue"
                                        BorderWidth="1px" />
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="RowID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRowID" runat="server" Text='<%# Eval("FavouriteMenu_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Menu Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMainAccount" runat="server" Text='<%# Eval("mnu_menuname") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Image">
                                            <ItemTemplate>
                                                <asp:Image ID="lblImage" runat="server" ImageUrl='<%# Eval("FavouriteMenu_Image") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Change Picture">
                                            <ItemTemplate>
                                                <a href="javascript:PicChange('<%# Eval("FavouriteMenu_ID")%>')">Change Picture</a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remove">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    OnClientClick='javascript:return confirm("Do You Want To Remove This Favourite Menu ??");'
                                                    Text="Remove"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnChangePic" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr style="display: none">
                    <td>
                        <asp:Button ID="btnChangePic" runat="server" Text="Button" OnClick="btnChangePic_Click" />
                    </td>
                </tr>
            </table>
        </div>
</asp:Content>
