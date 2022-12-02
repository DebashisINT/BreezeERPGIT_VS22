<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_Master_Lead_Document" CodeBehind="Lead_Document.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function Show() {

            var url = "frmAddDocuments.aspx?id=Lead_Document.aspx&id1=Lead";

            popup.SetContentUrl(url);

            popup.Show();

        }
        function disp_prompt(name) {
            if (name == "tab0") {
                //alert(name);
                document.location.href = "Lead_general.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "Lead_Correspondence.aspx";
            }
            else if (name == "tab2") {
                //alert(name);
                document.location.href = "Lead_BankDetails.aspx";
            }
                else if (name == "tab3") {
                    //alert(name);
                    document.location.href = "Lead_DPDetails.aspx";
                }
            else if (name == "tab4") {
                //alert(name);
                //document.location.href="Lead_Document.aspx"; 
            }
            else if (name == "tab5") {
                //alert(name);
                document.location.href = "Lead_FamilyMembers.aspx";
            }
            else if (name == "tab6") {
                //alert(name);
                document.location.href = "Lead_Registration.aspx";
            }
            else if (name == "tab7") {
                //alert(name);
                document.location.href = "Lead_GroupMember.aspx";
            }
            else if (name == "tab8") {
                //alert(name);
                document.location.href = "Lead_Remarks.aspx";
            }
        }
        //function OnDocumentView(keyValue) {
        //    var url = 'viewImage.aspx?id=' + keyValue;
        //    popup.contentUrl = url;
        //    popup.Show();

        //}
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
                var n = d.getFullYear();
                var url = '\\OMS\\Management\\Documents\\' + docid + '\\' + n + '\\' + filename;
                //window.open(url, '_blank');
                var seturl = '\\OMS\\Management\\DailyTask\\viewImage.aspx?id=' + url;
                popup.contentUrl = url;
                popup.Show();
            }
            else {
                alert('File not found.')
            }

            //var url = 'Copy of viewImage.aspx?id=' + keyValue;
            //popup.contentUrl = url;
            //popup.Show();

        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Lead Documents</h3>
        </div>
        <div class="crossBtn"><a href="Lead.aspx"><i class="fa fa-times"></i></a></div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="4" ClientInstanceName="page">
                        <ContentStyle>
                            <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px"></Border>
                        </ContentStyle>
                        <TabPages>
                            <dxe:TabPage Name="General" Text="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="CorresPondence" Text="CorresPondence">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Bank Details" Text="Bank Details">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="DP Details" Text="DP Details"  Visible="false">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Documents" Text="Documents">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <a href="javascript:void(0);" onclick="Show();" class="btn btn-primary"><span>Add New</span> </a>

                                        <dxe:ASPxGridView runat="server" ClientInstanceName="gridDocument" KeyFieldName="Id"
                                            AutoGenerateColumns="False" Width="100%" Font-Size="12px" ID="EmployeeDocumentGrid"
                                            OnRowDeleting="EmployeeDocumentGrid_RowDeleting">
                                            <Columns>
                                                <dxe:GridViewDataTextColumn FieldName="Id" ReadOnly="True" Visible="False" VisibleIndex="0">
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Type" Width="25%" Caption="Document Type"
                                                    VisibleIndex="0">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="FileName" Width="25%" Caption="Document Name"
                                                    VisibleIndex="1">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Src" Visible="False" VisibleIndex="2">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="FilePath" ReadOnly="True" Width="25%" Caption="Document Physical Location"
                                                    VisibleIndex="2">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataHyperLinkColumn Width="5%" Caption="View" VisibleIndex="3" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                    <DataItemTemplate>
                                                        <%--<a href='viewImage.aspx?id=<%#Eval("Src") %>' target="_blank">View</a>--%>
                                                        <%--<a onclick="OnDocumentView('<%#Eval("Src") %>')" style="cursor: pointer;">View</a>--%>
                                                        <a onclick="OnDocumentView('<%#Eval("doc") %>','<%#Eval("Src") %>')" style="text-decoration: none; cursor: pointer;" title="View">
                                                            <img src="../../../assests/images/viewIcon.png" />
                                                        </a>
                                                    </DataItemTemplate>
                                                </dxe:GridViewDataHyperLinkColumn>
                                                <dxe:GridViewCommandColumn VisibleIndex="4" ShowDeleteButton="true" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" Width="5%">

                                                    <HeaderTemplate>
                                                        <span>Actions</span>
                                                    </HeaderTemplate>
                                                </dxe:GridViewCommandColumn>
                                            </Columns>
                                            <SettingsCommandButton>
                                                <DeleteButton Image-Url="/assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete"></DeleteButton>

                                                <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary btn-xs"></UpdateButton>
                                                <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger btn-xs"></CancelButton>
                                            </SettingsCommandButton>
                                            <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True"></SettingsBehavior>
                                            <SettingsPager PageSize="20" AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True">
                                                <FirstPageButton Visible="True">
                                                </FirstPageButton>
                                                <LastPageButton Visible="True">
                                                </LastPageButton>
                                            </SettingsPager>
                                            <SettingsEditing Mode="PopupEditForm" PopupEditFormWidth="500px" PopupEditFormHeight="250px"
                                                PopupEditFormHorizontalAlign="Center" PopupEditFormVerticalAlign="WindowCenter"
                                                PopupEditFormModal="True" EditFormColumnCount="1">
                                            </SettingsEditing>
                                            <SettingsSearchPanel Visible="True" />
                                            <Settings ShowStatusBar="Visible" ShowFilterRow="true" ShowGroupPanel="true" ShowFilterRowMenu ="true"></Settings>
                                            <SettingsText ConfirmDelete="Are you sure to delete this record?" PopupEditFormCaption="Add/Modify Family Relationship"></SettingsText>
                                            <Styles>
                                                <Header SortingImageSpacing="5px" ImageSpacing="5px">
                                                </Header>
                                                <LoadingPanel ImageSpacing="10px">
                                                </LoadingPanel>
                                            </Styles>
                                        </dxe:ASPxGridView>
                                        <dxe:ASPxPopupControl runat="server" ClientInstanceName="popup" CloseAction="CloseButton"
                                            ContentUrl="frmAddDocuments.aspx" HeaderText="Add Document" Left="300" Top="120"
                                            Width="900px" Height="353px" ID="ASPXPopupControl" ResizingMode="Postponed" Modal="true">
                                            <ContentCollection>
                                                <dxe:PopupControlContentControl runat="server">
                                                </dxe:PopupControlContentControl>
                                            </ContentCollection>
                                        </dxe:ASPxPopupControl>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Family Members" Text="Family Members">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Registration" Text="Registration">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Group Member" Text="Group Member">
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
                        </TabPages>
                        <TabStyle Font-Size="12px">
                        </TabStyle>
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
	                                            
	                                            
	                                            }"></ClientSideEvents>
                        <LoadingPanelStyle ImageSpacing="6px">
                        </LoadingPanelStyle>
                    </dxe:ASPxPageControl>
                </td>
                <td>
                    <asp:TextBox ID="txtID" runat="server" Visible="false"></asp:TextBox>
                </td>
            </tr>
        </table>
        <%--<asp:SqlDataSource ID="EmployeeDocumentData" runat="server"
            SelectCommand="select tbl_master_document.doc_id AS Id, tbl_master_documentType.dty_documentType AS Type,tbl_master_document.doc_documentName AS FileName, tbl_master_document.doc_source AS Src,COALESCE ('Building- ' + tbl_master_building.bui_Name + CHAR(13) + CHAR(10), '') +COALESCE ('/ Floor No : ' + tbl_master_document.doc_Floor + CHAR(13) + CHAR(10), '')  + '/ Room No-' + tbl_master_document.doc_RoomNo + '/ Cabinet No-' + tbl_master_document.doc_CellNo + '/ File No-' + tbl_master_document.doc_FileNo AS FilePath from tbl_master_document INNER JOIN tbl_master_documentType ON tbl_master_document.doc_documentTypeId = tbl_master_documentType.dty_id INNER JOIN tbl_master_building ON tbl_master_document.doc_buildingId = tbl_master_building.bui_id where doc_contactId=@doc_contactId"
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
