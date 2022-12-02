<%@ Page Language="C#" AutoEventWireup="true" Title="Document"
    Inherits="ERP.OMS.Management.Master.management_Master_Contact_Document" CodeBehind="Contact_Document.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        //function SignOff() {
        //window.parent.SignOff()
        //}
        function Show() {
            var url = "";
            var isRedirect = document.getElementById("IsredirectedBranch").value;

            if (isRedirect == "1") {
                url = "frmAddDocuments.aspx?id=Contact_Document.aspx&id1=<%=Session["requesttype"]%>&AcType=Add&Page=branch";
            }
            else {
                url = "frmAddDocuments.aspx?id=Contact_Document.aspx&id1=<%=Session["requesttype"]%>&AcType=Add";
            }

            popup.SetContentUrl(url);
            //alert (url);
            popup.Show();

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
            var isRedirect = document.getElementById("IsredirectedBranch").value;
            var url = "";
            if (isRedirect == "1") {
                url = 'frmAddDocuments.aspx?id=Contact_Document.aspx&id1=<%=Session["requesttype"]%>&AcType=Edit&Page=branch&docid=' + KeyValue;
            }
            else {
                url = 'frmAddDocuments.aspx?id=Contact_Document.aspx&id1=<%=Session["requesttype"]%>&AcType=Edit&docid=' + KeyValue;
            }

            popup.SetContentUrl(url);

            popup.Show();
        }
        function disp_prompt(name) {

            //var ID = document.getElementById(txtID);
            if (name == "tab0") {
                //alert(name);
                //document.location.href = "rootcompany_general.aspx";//--//"Contact_general.aspx"; 
                if ("<%=Session["requesttype"]%>" == "Branches") {
                    document.location.href = "BranchAddEdit.aspx?id=<%=Session["con_branch"]%>";
                }
                else if ("<%=Session["requesttype"]%>" == "Building/Warehouses") {
                    var qString = window.location.href.split("=")[1];
                    document.location.href = "RootBuildingInsertUpdate.aspx?id=" + qString;
                }
                else {
                    document.location.href = "Contact_general.aspx";
                }
            }
            if (name == "tab1") {

                var isRedirect = document.getElementById("IsredirectedBranch").value;

                if (isRedirect == "1") {
                    document.location.href = "Branch_Correspondance.aspx?Page=branch";
                }
                else {
                    document.location.href = "Contact_Correspondence.aspx";
                }
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
              <% if (Session["requesttype"] == "Branches")
                 { %>
                document.location.href = "frm_BranchUdf.aspx";
                <%    }
                 else
                 {%>
                document.location.href = "Contact_Remarks.aspx";
            <% }%>

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
            else if (name == "tab14") {
                document.location.href = "Contact_tds.aspx";
            }
            else if (name == "tab15") {
                document.location.href = "Contact_Person.aspx";
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
        //var n = d.getFullYear();
        //var url = '\\OMS\\Management\\Documents\\' + docid + '\\' + n + '\\' + filename;
        //window.open(url, '_blank');
        var url = '\\OMS\\Management\\Documents\\' + obj2;
        var seturl = '\\OMS\\Management\\DailyTask\\viewImage.aspx?id=' + url;
        popup.contentUrl = url;
        popup.Show();
    }
    else {
        jAlert('File not found.')
    }



}

function DeleteRow(keyValue) {

    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
        if (r == true) {
            gridDocument.PerformCallback('Delete~' + keyValue);
            height();
        }
    });


}

function crossbtn_click() {
    if ("<%=Session["requesttype"]%>" == "Building/Warehouses") {
                document.location.href = "RootBuilding.aspx";
            }
            else {
                document.location.href = "Branch.aspx";
            }

        }


        $(document).ready(function () {
            var mod = '<%= Session["Contactrequesttype"] %>';
            if (mod == 'customer') {
                document.getElementById("lnkClose").href = 'CustomerMasterList.aspx';
            }
            else if (mod == 'Transporter') {
                document.getElementById("lnkClose").href = 'TransporterMasterList.aspx?requesttype=<%= Session["Contactrequesttype"] %>';
             }
             else {
                 document.getElementById("lnkClose").href = 'frmContactMain.aspx?requesttype=<%= Session["Contactrequesttype"] %>';
             }

        });
    </script>
    <style>
        .mainWraper > div {
            padding-bottom: 22px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">

            <h3>
                <asp:Label ID="lblHeadTitle" runat="server"></asp:Label>
            </h3>
            <%--<div class="crossBtn"><a href="<%=Session["PrePageRedirect"] %>"><i class="fa fa-times"></i></a></div>--%>

            <%--..........................Code Commented and Modified by sam on 03102016............................--%>
            <%-- <div class="crossBtn"><a href="frmContactMain.aspx?requesttype=<%= Session["Contactrequesttype"] %>""><i class="fa fa-times"></i></a></div>--%>

            <%--    <div class="crossBtn"><a href="Branch.aspx"><i class="fa fa-times"></i></a></div>--%>
            <%--   <div class="crossBtn"><a id="crossbtn" onclick="crossbtn_click()"><i class="fa fa-times"></i></a></div>--%>


            <% if (Session["requesttype"] == "Branches")
               { %>
            <div class="crossBtn"><a href="Branch.aspx?requesttype=Branches"><i class="fa fa-times"></i></a></div>
            <% }
               else if (Session["requesttype"] == "Building/Warehouses")
               { %>

            <div class="crossBtn"><a href="RootBuilding.aspx"><i class="fa fa-times"></i></a></div>
            <% }
               else if (Session["requesttype"] == "Account Heads")
               { %>
            <div class="crossBtn"><a href="MainAccountHead.aspx"><i class="fa fa-times"></i></a></div>
            <% }

               else if (Session["requesttype"] == "Sub Ledger")
               { %>

            <div class="crossBtn"><a href="<%= Convert.ToString(Session["redirct"]) %>"><i class="fa fa-times"></i></a></div>
            <% }


               else
               { %>
            <%--  <div class="crossBtn"><a href="frmContactMain.aspx?requesttype=<%= Session["Contactrequesttype"] %>"><i class="fa fa-times"></i></a></div>--%>
            <div class="crossBtn"><a id="lnkClose"><i class="fa fa-times"></i></a></div>
            <% }
            %>
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

                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>

                            <dxe:TabPage Name="CorresPondence" Text="Correspondence">

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

                                <ContentCollection>
                                    <dxe:ContentControl runat="server">

                                        <div style="float: left">
                                            <% if (rights.CanAdd)
                                               { %><a href="javascript:void(0);" class="btn btn-primary" onclick="Show();"><span>Add New</span> </a><% } %>
                                        </div>
                                        <dxe:ASPxGridView ID="EmployeeDocumentGrid" runat="server" AutoGenerateColumns="False"
                                            ClientInstanceName="gridDocument" KeyFieldName="Id" Width="100%" Font-Size="12px"
                                            OnCustomCallback="EmployeeDocumentGrid_CustomCallback" OnHtmlRowCreated="EmployeeDocumentGrid_HtmlRowCreated" OnRowCommand="EmployeeDocumentGrid_RowCommand">
                                            <SettingsSearchPanel Visible="True" />
                                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" ShowGroupPanel="true" ShowStatusBar="Visible" />
                                            <SettingsPager>
                                                <PageSizeItemSettings Visible="true" />
                                            </SettingsPager>
                                            <Columns>
                                                <dxe:GridViewDataTextColumn FieldName="Id" ReadOnly="True" VisibleIndex="0" Visible="False">
                                                    <EditFormSettings Visible="False" />
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Type" VisibleIndex="0" Caption="Doc. Type">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="FileName" VisibleIndex="1" Caption="Doc. Name">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Src" VisibleIndex="2" Visible="False">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Note1" VisibleIndex="3" Visible="true" Caption="Note1">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Note2" VisibleIndex="4" Visible="true" Caption="Note2">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Fileno" VisibleIndex="5" Visible="true" Caption="Number">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Bldng" VisibleIndex="6" Visible="true" Caption="Building">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="FilePath" ReadOnly="True" VisibleIndex="7"
                                                    Caption="Location">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="createuser" ReadOnly="True" VisibleIndex="8"
                                                    Caption="Upload By">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn Caption="Verified By" FieldName="vrfy" ReadOnly="True"
                                                    VisibleIndex="9">
                                                    <EditFormSettings Visible="False" />
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="ReceiveDate" VisibleIndex="10" Visible="true">
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="RenewDate" VisibleIndex="11" Visible="true">
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn VisibleIndex="13" Width="8%" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>Action</HeaderTemplate>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <DataItemTemplate>
                                                        <% if (rights.CanView)
                                                           { %>
                                                        <a onclick="OnDocumentView('<%#Eval("doc") %>','<%#Eval("Src") %>')" style="text-decoration: none; cursor: pointer;" title="View" class="pad">
                                                            <img src="../../../assests/images/viewIcon.png" />
                                                        </a><% } %>

                                                        <% if (rights.CanEdit)
                                                           { %>
                                                        <a href="javascript:void(0);" onclick="ShowEditForm('<%# Container.KeyValue %>');" style="text-decoration: none;" title="Edit" class="pad">
                                                            <img src="../../../assests/images/Edit.png" />
                                                        </a><% } %>
                                                        <% if (rights.CanDelete)
                                                           { %>
                                                        <a href="javascript:void(0);" onclick="DeleteRow('<%# Container.KeyValue %>')" style="text-decoration: none;" title="Delete">
                                                            <img src="../../../assests/images/Delete.png" />
                                                        </a><% } %>
                                                    </DataItemTemplate>

                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>

                                            </Columns>
                                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" />
                                            <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="2" PopupEditFormHorizontalAlign="Center"
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

                                        </dxe:ASPxGridView>
                                        <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ContentUrl="frmAddDocuments.aspx"
                                            CloseAction="CloseButton" Top="120" Left="300" ClientInstanceName="popup" Height="466px"
                                            Width="900px" HeaderText="Add/Modify Document" AllowResize="true" ResizingMode="Postponed" Modal="true">
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

                            <dxe:TabPage Name="Remarks" Text="UDF" Visible="false">
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
                             <dxe:TabPage Name="TDS" Visible="false" Text="TDS">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="Contact Person" Name="ContactPreson">
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
	                                              var Tab14 = page.GetTab(14);
	                                                var Tab15=page.GetTab(15);
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
	                                            else if(activeTab == Tab14)
	                                            {
	                                                disp_prompt('tab14');
	                                            }
	                                            else if(activeTab == Tab15)
	                                            {
	                                                disp_prompt('tab15');
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
        <asp:HiddenField ID="IsredirectedBranch" runat="server" />
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
