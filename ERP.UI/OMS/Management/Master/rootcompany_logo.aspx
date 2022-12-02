<%@ Page Title="Logo" Language="C#" AutoEventWireup="True" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_rootcompany_logo" CodeBehind="rootcompany_logo.aspx.cs" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">


        function disp_prompt(name) {
            if (name == "tab0") { 
                document.location.href="rootcompany_general.aspx"; 
            }
            if (name == "tab1") {
                document.location.href = "rootComp_Correspondence.aspx";
            }
            if (name == "tab2") {
                document.location.href = "rootComp_document.aspx";
            }
            if (name == "tab3") {
                document.location.href = "rootcompany_deductorinfo.aspx";
            }
        }

       function btnBigLogoDeleteClick()
        { 
            document.getElementById('<%= btnBigLogoDelete.ClientID %>').click();
        }

        function btnSmallLogoDeleteClick() {
            document.getElementById('<%= btnSmallLogoDelete.ClientID %>').click();
        }
    </script>
    <style>
        .myImageBig {
            max-height: 300px;
            max-width: 300px;
        }
        .myImageSmall {
            max-height: 150px;
            max-width: 300px;
        }
        .mleft10 {
            margin-left:10px
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>&nbsp;<asp:Label ID="Label1" runat="server"></asp:Label>
                &nbsp;Logo Upload</h3>
            <div class="crossBtn"><a href="root_Companies.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>

    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="4" ClientInstanceName="page"
                        Width="100%">
                        <tabpages>
                             <dxe:TabPage Name="General" Text="General"  Visible="true" >
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

                             <dxe:TabPage Name="Documents" Text="Documents">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>

                             <dxe:TabPage Name="Deductor Info(TDS)" Text="Deductor Info(TDS)">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>

                            <dxe:TabPage Text="Logo" Name="Logo">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                       
                                          <div class="row">
                                               <div class="col-md-4">
                                                    <div   style="height: auto;">
                                                     <dxe:ASPxImage ID="CompImageBig" runat="server"  ClientInstanceName="cCompImageBig" CssClass="myImageBig" ></dxe:ASPxImage>
                                                     </div>
                                                    <div class="Left_Content" style="padding-top:10px">
                                    
                                                           <dxe:ASPxUploadControl ID="upldBigLogo" runat="server" ClientInstanceName="cupldBigLogo" 
                                                             ShowProgressPanel="True" CssClass="pull-left">
                                                              <ValidationSettings MaxFileSize="2194304" AllowedFileExtensions=".jpg, .jpeg, .gif, .png" ErrorStyle-CssClass="validationMessage" />
                                                
                                                            </dxe:ASPxUploadControl>
                                                         <%-- Rev Sayantani--%>
                                                        <%--<button class="btn btn-danger mleft10" onclick="btnBigLogoDeleteClick();return false;"><i class="fa fa-trash"></i></button>--%>
                                                        <%--   End of Rev Sayantani--%>
                                                        <asp:Button ID="btnBigLogoDelete" runat="server" Text="Delete" ValidationGroup="rqCompany" CssClass="btn btn-danger mleft10" OnClick="btnBigLogoDelete_Click"
                                                        Width="73px" style="display:none" />
                                                    
                                                    </div>
                                               </div>
                                              <div class="col-md-4">
                                                    <div class="cityDiv" style="height: auto;">
                                                       <dxe:ASPxImage ID="CompImageSmall" runat="server"  ClientInstanceName="cCompImageSmall" CssClass="myImageSmall" ></dxe:ASPxImage>
                                                         </div>


                                                    <div class="Left_Content" style="padding-top:10px">
                                                   <dxe:ASPxUploadControl ID="upldSmallLogo" runat="server" ClientInstanceName="cupldBigSmall" 
                                                         ShowProgressPanel="True" CssClass="pull-left">
                                                          <ValidationSettings MaxFileSize="2194304" AllowedFileExtensions=".jpg, .jpeg, .gif, .png" ErrorStyle-CssClass="validationMessage" />
                                                
                                                        </dxe:ASPxUploadControl>
                                                         <%-- Rev Sayantani--%>
                                                        <%--<button class="btn btn-danger mleft10"><i class="fa fa-trash"  onclick="btnSmallLogoDeleteClick();return false;"></i></button>--%>
                                                      <%--   End of Rev Sayantani--%>
                                                           <asp:Button ID="btnSmallLogoDelete" runat="server" Text="Delete" ValidationGroup="rqCompany" CssClass="btn btn-danger mleft10" OnClick="btnSmallLogoDelete_Click"
                                                        Width="73px" style="display:none" />
                                                    
                                                    </div>
                                               </div>
                                           </div>
                                         
                                        <div style="padding-top:15px">
                                         <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="rqCompany" CssClass="btn btn-primary" OnClick="btnSave_Click"
                                                        Width="73px"  />
                                        </div>
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
	                                            if(activeTab == Tab0)
	                                            {
	                                                disp_prompt('tab0');
	                                            }
                                                else if(activeTab == Tab1)
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
	                                            }"></clientsideevents>
                        <contentstyle>
                            <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                        </contentstyle>
                        <loadingpanelstyle imagespacing="6px">
                        </loadingpanelstyle>
                        <tabstyle font-size="12px">
                        </tabstyle>
                    </dxe:ASPxPageControl>
                </td>
            </tr>
            <tr>
                <td></td>
            </tr>
        </table>
    </div>
</asp:Content>
