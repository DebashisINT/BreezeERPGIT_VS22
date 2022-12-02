<%@ Page Title="Document" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" CodeBehind="Driver_document.aspx.cs" Inherits="ERP.OMS.Management.Master.Driver_document" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     

    <script language="javascript" type="text/javascript">

        function Changestatus(obj) {
            var URL = "../verify_documentremarks.aspx?id=" + obj;
            window.location.href = URL;

        }
        FieldName = 'btnCancel';

        function disp_prompt(name) {
            if (name == "tab0") {
                document.location.href = "DriverAddEdit.aspx?id=" + <%=Convert.ToString(Session["KeyVal"])%> +"";
            }
            if (name == "tab1") {
                document.location.href = "Driver_Correspondence.aspx";
            }

        }

        function OnAuthSigInfoClick(keyValue) {

            var url = 'AuthoSignatory.aspx?id=' + keyValue;
            var HeaderText = 'Authorised Signatory';
            editwin1 = dhtmlwindow_inner.open("Editbox", "iframe", url, HeaderText, "width=840px,height=350px,center=1,resize=1,scrolling=2,top=500", "recal")
            editwin1.onclose = function () {

            }

        }

        function CallList(obj1, obj2, obj3) {
            var obj5 = '';
            ajax_showOptions(obj1, obj2, obj3, obj5);
        }

        function Show() {
            var url = "frmAddDocuments.aspx?id=rootComp_document.aspx&id1=Companies&AcType=Add";
            popup.SetContentUrl(url);

            popup.Show();
        }

        function ShowEditForm(KeyValue) {
            var url = "frmAddDocuments.aspx?id=rootComp_document.aspx&id1=Companies&AcType=Edit&docid=" + KeyValue;
            popup.SetContentUrl(url);
            popup.Show();
        }

        function DeleteRow(keyValue) {
            doIt = confirm('Are you sure to delete this record?');
            if (doIt) {
                gridDocument.PerformCallback('Delete~' + keyValue);

            }
            else {

            }


        }

        function FilterOff(obj) {

            if (obj == 'EXA0000001') {
                document.getElementById("trSegment").style.display = "none";
                document.getElementById("trMemType").style.display = "none";
                document.getElementById("TmCode").style.display = "none";
                document.getElementById("CmCode").style.display = "none";
                document.getElementById("trSEBI").style.display = "none";
                document.getElementById("trExp").style.display = "none";
                document.getElementById("TrFMC").style.display = "none";
                document.getElementById("TrFmcEX").style.display = "none";
                document.getElementById("TrCompOf").style.display = "none";
                document.getElementById("Cmbpid").style.display = "none";
                document.getElementById("Trbroker").style.display = "none";
                document.getElementById("Trexchange").style.display = "none";

            }
            else {

                document.getElementById("trSegment").style.display = "inline";
                document.getElementById("trMemType").style.display = "inline";
                document.getElementById("TmCode").style.display = "inline";
                document.getElementById("CmCode").style.display = "inline";
                document.getElementById("trSEBI").style.display = "inline";
                document.getElementById("trExp").style.display = "inline";
                document.getElementById("TrFMC").style.display = "inline";
                document.getElementById("TrFmcEX").style.display = "inline";
                document.getElementById("TrCompOf").style.display = "inline";
                document.getElementById("Cmbpid").style.display = "inline";
                document.getElementById("Trbroker").style.display = "inline";
                document.getElementById("Trexchange").style.display = "inline";

            }

        }


        function ShowHideFilter(obj) {

            grid.PerformCallback(obj);
        }

        function OnAddEditClick(e, obj) {

            // FieldName='ASPxPopupControl1_ASPxCallbackPanel1_drpBranch';
            Filter = 'N';
            RowID = '';
            var data = obj.split('~');
            if (data.length > 1)
                RowID = data[1];
            popup.Show();
            popPanel.PerformCallback(obj);
        }

        function callback() {
            var applicant = document.getElementById("ASPxPopupControl1_ASPxCallbackPanel1_txtApplicant_hidden").value;
            AppBank.PerformCallback(applicant);
            cmbSubSequentBank.PerformCallback(applicant);
        }


        function OnDeleteClick(e, obj) {
            if (confirm('Are You Sure you want to Delete This Transaction?') == true) {
                grid.PerformCallback(obj);
            }
        }
        function btnSave_Click() {

            Filter = 'Y';

            if (RowID == '') {
                var obj = 'SaveNew';
                popPanel.PerformCallback(obj);

            }
            else {
                var obj = 'SaveOld~' + RowID;
                popPanel.PerformCallback(obj);
            }

        }
        function EndCallBack(obj, obj1) {


            //Validate();
            if (obj1 == 'Y') {
                alert("Already Exists!..");
                return false;
            }

            if (obj1 == 'S') {
                FilterOff('EXA0000001')
            }


            if (obj != '') {
                var data = obj.split('~');
                if (data[0] == 'Edit') {

                }
            }
            if (Filter == 'Y') {
                popup.Hide();
                grid.PerformCallback();
            }

        }

        //   function calldispose()
        //    {
        //      var  type=document.getElementById("ASPxPopupControl1_ASPxCallbackPanel1_cmbType").value;
        //            var y=(screen.availHeight-450)/2;
        //        var x=(screen.availWidth-900)/2;
        //        var str = 'frm_TemplateReservedWord.aspx?Type='+type;
        //        window.open(str,"Search_Conformation_Box","height=350,width=700,top="+ y +",left="+ x +",location=no,directories=no,menubar=no,toolbar=no,status=yes,scrollbars=no,resizable=no,dependent=no");       
        //      

        //    }


        function btnCancel_Click() {
            popup.Hide();
        }
        function EndCall() {
            if (grid.cpDelmsg != null)
                alert(grid.cpDelmsg);
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
            <h3>Driver Documents</h3>
            <div class="crossBtn"><a href="frm_drivers_master.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td style="text-align: center" class="EHEADER">
                    <asp:Literal ID="LitCompName" runat="server"></asp:Literal>
                    <div class="crossBtn"><a href="frm_drivers_master.aspx"><i class="fa fa-times"></i></a></div>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="2" ClientInstanceName="page"
                        Width="100%">
                        <TabPages>
                            <dxe:TabPage Text="General" Name="General">
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
                                        <div class="pull-left">
                                            <% if (rights.CanAdd)
                                               { %>
                                            <a href="javascript:void(0);" onclick="Show();" class="btn btn-primary"><span>Add New</span> </a>
                                            <%} %> 
                                        </div>
                                        <dxe:ASPxGridView ID="EmployeeDocumentGrid" runat="server" AutoGenerateColumns="False"
                                            ClientInstanceName="gridDocument" KeyFieldName="Id" Width="100%"
                                            OnCustomCallback="EmployeeDocumentGrid_CustomCallback" OnHtmlRowCreated="EmployeeDocumentGrid_HtmlRowCreated">
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
                                                           <% if (rights.CanView)
                                                                 { %>
                                                        <a onclick="OnDocumentView('<%#Eval("doc") %>','<%#Eval("Src") %>')" style="text-decoration: none; cursor: pointer;" title="View" class="pad">
                                                            <img src="/assests/images/viewIcon.png" />
                                                        </a>  
                                                         <% } %>
                                                             <% if (rights.CanEdit)
                                                                 { %>
                                                        <a href="javascript:void(0);" onclick="ShowEditForm('<%# Container.KeyValue %>');" style="text-decoration: none;" title="Edit" class="pad">
                                                            <img src="/assests/images/Edit.png" />
                                                        </a> 
                                                           <% } %>
                                                         <% if (rights.CanDelete)
                                                                 { %>
                                                        <a href="javascript:void(0);" onclick="DeleteRow('<%# Container.KeyValue %>')" style="text-decoration: none;" title="Delete">
                                                            <img src="/assests/images/Delete.png" />
                                                        </a> 
                                                          <% } %>
                                                    </DataItemTemplate>
                                                    <HeaderTemplate>
                                                        Actions
                                                    </HeaderTemplate>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                               
                                            </Columns>
                                            <SettingsSearchPanel Visible="True" />
                                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu ="true" />
                                            <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="WindowCenter"
                                                PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="700px"
                                                EditFormColumnCount="1" />
                                            <Styles>
                                                
                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                </Header>
                                            </Styles>
                                            <SettingsText PopupEditFormCaption="Add/Modify Documents" ConfirmDelete="Are you sure to delete this record?" />
                                            <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                                                <FirstPageButton Visible="True">
                                                </FirstPageButton>
                                                <LastPageButton Visible="True">
                                                </LastPageButton>
                                            </SettingsPager>
                                            <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" AllowFocusedRow="false" />
                                            
                                        </dxe:ASPxGridView>
                                        <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ContentUrl="frmAddDocuments.aspx"
                                            CloseAction="CloseButton" ClientInstanceName="popup" Height="466px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                            Width="900px" HeaderText="Add / Modify Document" AllowResize="true" ResizingMode="Postponed" Modal="true">
                                            <ContentCollection>
                                                <dxe:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                                                </dxe:PopupControlContentControl>
                                            </ContentCollection>
                                            <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
                                        </dxe:ASPxPopupControl>
                                    </dxe:ContentControl>
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
                        <TabStyle Font-Size="12px">
                        </TabStyle>
                    </dxe:ASPxPageControl>
                </td>
            </tr>
        </table>
    </div>

</asp:Content>