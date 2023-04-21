<%--==========================================================Revision History ============================================================================================   
   1.0   Priti   V2.0.39     21-04-2023     25884: Error while trying to view Product Master Document Attachment
========================================== End Revision History =======================================================================================================--%>


<%@ Page Title="Document" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="Product_Document.aspx.cs" 
    Inherits="ERP.OMS.Management.Master.Product_Document" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        
    <script type="text/javascript">
        //function SignOff() {
        //window.parent.SignOff()
        //}
        function Show() {
            var url = "frmAddDocuments.aspx?id=Contact_Document.aspx&id1=<%=Session["requesttype"]%>&AcType=Add";
            popup.SetContentUrl(url);
            //alert (url);
            popup.Show();
            popup.SetHeaderText('Add Document');
        }
        function PopulateGrid(obj) {
            gridDocument.PerformCallback(obj);
        }
        function Changestatus(obj) {
            var URL = "../verify_documentremarks.aspx?id=" + obj;
            window.location.href = URL;
            //editwin = dhtmlmodal.open("Editbox", "iframe", URL, "Verify Remarks", "width=995px,height=300px,center=0,resize=1,top=-1", "recal");
            //editwin.onclose = function () {
            //    gridDocument.PerformCallback();
            //}
        }
        function ShowEditForm(KeyValue) {
            var url = 'frmAddDocuments.aspx?id=Contact_Document.aspx&id1=<%=Session["requesttype"]%>&AcType=Edit&docid=' + KeyValue;
            popup.SetContentUrl(url);
            //alert (url);
            popup.Show();
            popup.SetHeaderText('Modify Document');
        }
        function disp_prompt(name) {

            //var ID = document.getElementById(txtID);
            if (name == "tab0") {
                //alert(name);
                document.location.href = "rootcompany_general.aspx";//--//"Contact_general.aspx"; 
                //document.location.href = "Contact_general.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "rootComp_Correspondence.aspx";//////"Contact_Correspondence.aspx"; 
                //document.location.href = "Contact_Correspondence.aspx";
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
                //document.location.href="Contact_Document.aspx"; 
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
                document.location.href = "Contact_GroupMember.aspx";
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
        function OnDocumentView(obj1, obj2) {
           
            var docid = obj1;
            var filename;
            var chk = obj2.includes("~");
            if (chk) {
                filename = obj2.split('~')[1];
            }
            else {
                filename = obj2.split('/')[2];
            }
            if (filename != '' && filename != null) {
                var d = new Date();
                //Rev 1.0
                //var n = d.getFullYear();
                var n = obj2.split('/')[1];
                //Rev 1.0 End
                var url = '\\OMS\\Management\\Documents\\' + docid + '\\' + n + '\\' + filename;
               
               
                var seturl = '\\OMS\\Management\\DailyTask\\viewImage.aspx?id=' + url;
              
                popup.contentUrl = url;
                popup.Show();
                popup.SetHeaderText('View Document');
            }
            else {
                jAlert('File not found.')
            }

            //var url = 'Copy of viewImage.aspx?id=' + keyValue;
            //popup.contentUrl = url;
            //popup.Show();

        }

        function DeleteRow(keyValue) {           
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    gridDocument.PerformCallback('Delete~' + keyValue);
                   
                }
            });


        }
    </script>
    <style type="text/css">
        /*.dxpcLite_PlasticBlue .dxpc-contentWrapper, .dxdpLite_PlasticBlue .dxpc-contentWrapper {
            border-bottom:none !important;
        }*/
        input#txtFilepath {
            margin-bottom:3px !important;
        }
        #LinkButton1 {
            right: 9px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="panel-heading">
           
        <div class="panel-title">
            <%--<h3>
               Documents of Product 
            </h3>--%>
            <h4> <asp:Label ID="lblheader"  runat="server" Text="Documents of Product "></asp:Label></h4>
            <div class="crossBtn"><a href="/OMS/management/store/Master/sProducts.aspx"><i class="fa fa-times"></i></a></div>
            <%--<div class="crossBtn"><a href="<%=Session["PrePageRedirect"] %>"><i class="fa fa-times"></i></a></div>--%>
           <%-- <div class="crossBtn"><a href="frmContactMain.aspx?requesttype=<%= Session["Contactrequesttype"] %>""><i class="fa fa-times"></i></a></div>--%>
        </div>
    </div>
    <div class="form_main">
        <table width="100%">
            <tr>
                <td class="EHEADER" style="text-align: center">
                    <asp:Label ID="lblName" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <table class="TableMain100">
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="4" ClientInstanceName="page">
                        <TabPages>
                            <dxe:TabPage Text="General" Name="General">
                                <%--<TabTemplate>
                                    <span style="font-size: x-small">General</span>&nbsp;<span style="color: Red;">*</span>
                                </TabTemplate>--%>
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="CorresPondence" Text="CorresPondence">
                                <%--<TabTemplate>
                                    <span style="font-size: x-small">CorresPondence</span>&nbsp;<span style="color: Red;">*</span>
                                </TabTemplate>--%>
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Bank Details" Text="Bank">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="DP Details" Visible="false" Text="DP">

                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="Documents" Name="Documents">
                                <%--<TabTemplate>
                                    <span style="font-size: x-small">Documents</span>&nbsp;<span style="color: Red;">*</span>
                                </TabTemplate>--%>
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <% if (rights.CanAdd)
                                          { %>
                                        <div>  <a href="javascript:void(0);" onclick="Show();" class="btn btn-primary"><span>Add New</span> </a></div>
                                        <%} %>
                                        <dxe:ASPxGridView ID="EmployeeDocumentGrid" runat="server" AutoGenerateColumns="False"
                                            ClientInstanceName="gridDocument" KeyFieldName="Id" Width="100%" Font-Size="12px"
                                            OnCustomCallback="EmployeeDocumentGrid_CustomCallback" OnHtmlRowCreated="EmployeeDocumentGrid_HtmlRowCreated" OnRowCommand="EmployeeDocumentGrid_RowCommand">
                                            <SettingsSearchPanel Visible="True" />
                                            <Settings ShowFilterRow="true" ShowFilterRowMenu ="true"/>
                                            <Columns>
                                                <dxe:GridViewDataTextColumn FieldName="Id" ReadOnly="True" VisibleIndex="0" Visible="False">
                                                    <EditFormSettings Visible="False" />
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Type" VisibleIndex="0" Caption="Doc.Type">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="FileName" VisibleIndex="1" Caption="Doc.Name">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Src" VisibleIndex="2" Visible="False">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Note1" VisibleIndex="3" Visible="true" Caption="Note 1">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Note2" VisibleIndex="4" Visible="true" Caption="Note 2">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Fileno" VisibleIndex="5" Visible="true">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Bldng" VisibleIndex="6" Visible="true">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="FilePath" ReadOnly="True" VisibleIndex="7"
                                                    Caption="Location">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="createuser" ReadOnly="True" VisibleIndex="8"
                                                    Caption="Upload By">
                                                </dxe:GridViewDataTextColumn>
                                                <%--<dxe:GridViewDataHyperLinkColumn Caption="View" FieldName="Src" VisibleIndex="8"
                                                        Width="15%">
                                                        <DataItemTemplate>
                                                         <a onclick="OnDocumentView('<%#Eval("Src") %>')" style="cursor:pointer;">View</a>
                                                        </DataItemTemplate>
                                                    </dxe:GridViewDataHyperLinkColumn>--%>
                                                <dxe:GridViewDataTextColumn Caption="Verified By" FieldName="vrfy" ReadOnly="True"
                                                    VisibleIndex="9">
                                                    <EditFormSettings Visible="False" />
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="ReceiveDate" VisibleIndex="10" Visible="true">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="RenewDate" VisibleIndex="11" Visible="true">
                                                </dxe:GridViewDataTextColumn>
                                                <%-- <dxe:GridViewCommandColumn VisibleIndex="7">
                                                        <DeleteButton Visible="True">
                                                        </DeleteButton>
                                                        <HeaderTemplate>
                                                            <a href="javascript:void(0);" onclick="Show();"><span style="color: #000099; text-decoration: underline">
                                                                Add New</span> </a>
                                                        </HeaderTemplate>
                                                    </dxe:GridViewCommandColumn>--%>
                                                <dxe:GridViewDataTextColumn VisibleIndex="13" Width="8%" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <DataItemTemplate>
                                                        <a onclick="OnDocumentView('<%#Eval("doc") %>','<%#Eval("Src") %>')" style="text-decoration: none; cursor: pointer;" title="View Document" class="pad">
                                                            <img src="../../../assests/images/viewIcon.png" />
                                                        </a>
                                                        <%--<asp:LinkButton ID="btn_download" runat="server" OnClientClick="return confirm('Confirm delete?');" CommandArgument='<%#Eval("doc") %>','<%#Eval("Src") %>' CommandName="delete" ToolTip="Delete" CssClass="pad" Font-Underline="false">
                                        <img src="/assests/images/Delete.png" />
                                                        </asp:LinkButton>--%>
                                                           <%if (rights.CanEdit)
                                                             { %>
                                                        <a href="javascript:void(0);" onclick="ShowEditForm('<%# Container.KeyValue %>');" style="text-decoration: none;" title="Edit" class="pad">
                                                            <img src="/assests/images/Edit.png" />
                                                        </a>
                                                        <%} %>
                                                        <%if (rights.CanDelete)
                                                             { %>
                                                            <a href="javascript:void(0);" onclick="DeleteRow('<%# Container.KeyValue %>')" style="text-decoration: none;" title="Delete">
                                                                <img src="/assests/images/Delete.png" />
                                                            </a>
                                                       <%} %>
                                                    </DataItemTemplate>
                                                    <HeaderTemplate>
                                                        Action
                                                      <%--  <a href="javascript:void(0);" onclick="Show();"><span>Add New</span> </a>--%>
                                                    </HeaderTemplate>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <%-- <dxe:GridViewDataTextColumn VisibleIndex="8" Width="60px" Caption="Edit">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <DataItemTemplate>
                                                                                                        
                                                        </DataItemTemplate>
                                                            <CellStyle Wrap="False"></CellStyle>
                                                            <HeaderTemplate>
                                                                     <span style="color: #000099;">Edit</span>                                                                                                                
                                                            </HeaderTemplate>
                                                            <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>--%>
                                            </Columns>
                                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" />
                                            <SettingsEditing Mode="PopupEditForm"  PopupEditFormHorizontalAlign="Center"
                                                PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="700px"
                                                EditFormColumnCount="1" />
                                            <Styles>
                                                <LoadingPanel ImageSpacing="10px">
                                                </LoadingPanel>
                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                </Header>
                                            </Styles>
                                            <SettingsText PopupEditFormCaption="Add/Modify Documents" ConfirmDelete="Confirm delete?" />
                                            <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                                                <FirstPageButton Visible="True">
                                                </FirstPageButton>
                                                <LastPageButton Visible="True">
                                                </LastPageButton>
                                            </SettingsPager>
                                            <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                                            <%-- <Templates>
                                                    <TitlePanel>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td align="right">
                                                                    <table width="200">
                                                                        <tr>
                                                                            <td>
                                                                                <dxe:ASPxButton ID="ASPxButton1" runat="server" Text="ADD" ToolTip="Add New Data"
                                                                                    Height="18px" Width="88px" Font-Size="12px" AutoPostBack="False">
                                                                                  
                                                                                    <ClientSideEvents Click="function(s,e){Show();}" />
                                                                                </dxe:ASPxButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </TitlePanel>
                                                </Templates>--%>
                                        </dxe:ASPxGridView>
                                        <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ContentUrl="frmAddDocuments.aspx"
                                            CloseAction="CloseButton" Top="120" Left="300" ClientInstanceName="popup" Height="466px"
                                            Width="900px" HeaderText="Add/Modify Document" AllowResize="false" ContentStyle-Wrap="True"  ResizingMode="Live" Modal="true" >
                                            <ContentCollection>
                                                <dxe:PopupControlContentControl runat="server">
                                                </dxe:PopupControlContentControl>
                                            </ContentCollection>
                                            <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
                                        </dxe:ASPxPopupControl>
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
                                <%--  <TabTemplate>
                                    <span style="font-size: x-small">Other</span>&nbsp;<span style="color: Red;">*</span>
                                </TabTemplate>--%>
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Group Member" Text="Group">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Deposit" Visible="false" Text="Deposit">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>

                            <dxe:TabPage Name="Remarks" Text="Remarks">
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
	                                            
	                                            }"></ClientSideEvents>
                        <ContentStyle>
                            <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                        </ContentStyle>
                        <LoadingPanelStyle ImageSpacing="6px">
                        </LoadingPanelStyle>
                        <TabStyle Font-Size="12px">
                        </TabStyle>
                    </dxe:ASPxPageControl>
                </td>
                <td>
                    <asp:TextBox ID="txtID" runat="server" Visible="false"></asp:TextBox>
                </td>
            </tr>
        </table>
        <%--<asp:SqlDataSource ID="EmployeeDocumentData" runat="server" 
            SelectCommand=""
            DeleteCommand="delete from tbl_master_document where doc_id=@Id">
          <DeleteParameters>
             <asp:Parameter Name="Id" Type="decimal" />
          </DeleteParameters>  
          <SelectParameters>
            <asp:SessionParameter Name="doc_contactId" SessionField="KeyVal_InternalID" Type="string" />
          </SelectParameters>
        </asp:SqlDataSource>--%>
    </div>
</asp:Content>
