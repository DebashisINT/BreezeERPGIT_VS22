<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmLanguageProfi.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Master.frmLanguageProfi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function AddSelectedItems() {
            MoveSelectedItems(lbAvailable, lbChoosen);
            UpdateButtonState();
        }
        function AddAllItems() {
            MoveAllItems(lbAvailable, lbChoosen);
            UpdateButtonState();
        }
        function RemoveSelectedItems() {
            MoveSelectedItems(lbChoosen, lbAvailable);
            UpdateButtonState();
        }
        function RemoveAllItems() {
            MoveAllItems(lbChoosen, lbAvailable);
            UpdateButtonState();
        }
        function MoveSelectedItems(srcListBox, dstListBox) {
            srcListBox.BeginUpdate();
            dstListBox.BeginUpdate();
            var items = srcListBox.GetSelectedItems();
            for (var i = items.length - 1; i >= 0; i = i - 1) {
                dstListBox.AddItem(items[i].text, items[i].value);
                srcListBox.RemoveItem(items[i].index);
            }
            srcListBox.EndUpdate();
            dstListBox.EndUpdate();
        }
        function MoveAllItems(srcListBox, dstListBox) {
            srcListBox.BeginUpdate();
            var count = srcListBox.GetItemCount();
            for (var i = 0; i < count; i++) {
                var item = srcListBox.GetItem(i);
                dstListBox.AddItem(item.text, item.value);
            }
            srcListBox.EndUpdate();
            srcListBox.ClearItems();
        }

     
        function UpdateButtonState() {
          //  debugger;

            var item = [];
            //for (var i = 0; i < lbAvailable.GetItemCount() ; i++) {
               
            //    item.push(lbAvailable.GetItem(i).text);
            //}

            var items = lbAvailable.GetSelectedItems();
            for (var i = 0 ;i <items.length; i ++) {
                item.push(items[i].text);
            }
         //   alert(item);
            $("#lblspeaklanguages").text(item.toString());
            //var s = lbAvailable.GetItem() +',';
            //s += s;
            //alert(lbAvailable)
            //btnMoveAllItemsToRight.SetEnabled(lbAvailable.GetItemCount() > 0);
            //btnMoveAllItemsToLeft.SetEnabled(lbChoosen.GetItemCount() > 0);
            //btnMoveSelectedItemsToRight.SetEnabled(lbAvailable.GetSelectedItems().length > 0);
            //btnMoveSelectedItemsToLeft.SetEnabled(lbChoosen.GetSelectedItems().length > 0);
        }


        function UpdateButtonWrite() {
    
            var item = [];
 

            var items = lbChoosen.GetSelectedItems();
            for (var i = 0 ; i < items.length; i++) {
                item.push(items[i].text);
            }
         
            $("#LitWrittenLanguage").text(item.toString());
         
        }



        //function Error() {
        //    alert('Please Select a name');
        //    return false;

        //}
    </script>
    <style>
        .lbsd {
            float: left;
            margin-right: 20px;
        }
    </style>
</asp:Content>

<%--<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Language Proficiency of 
                <asp:Label ID="lblHeader" runat="server"
                    Width="819px" Height="18px"></asp:Label></h3>
              <div class="crossBtn"> <a href="employee_general.aspx"><i class="fa fa-times"></i></a></div>
            <div class="crossBtn">
                <asp:LinkButton ID="goBackCrossBtn" runat="server" OnClick="goBackCrossBtn_Click"><i class="fa fa-times"></i></asp:LinkButton>
            </div>
        </div>
    </div>
    <div class="form_main" style="border: 1px solid #ccc; padding: 5px 15px">
        <table class="TableMain100">
            
            <tr>


                <td style="width:30%">
                    <span style="margin-bottom:8px;display:inline-block">Name:</span>
                    <dxe:ASPxListBox ID="lbAvailable" runat="server" ClientInstanceName="lbAvailable"
                        Width="350px" Height="240px" SelectionMode="CheckColumn" Caption="">
                        <clientsideevents selectedindexchanged="function(s, e) { UpdateButtonState(); }" />
                    </dxe:ASPxListBox>

                </td>
                <td style="width:300px; text-align:center">
                    <div>
                        <dxe:ASPxButton ID="btnMoveSelectedItemsToRight" runat="server" ClientInstanceName="btnMoveSelectedItemsToRight"
                            AutoPostBack="False" Text="Add >" Width="130px" cssClass="btn btn-primary" ClientEnabled="False"
                            ToolTip="Add selected items">
                            <clientsideevents click="function(s, e) { AddSelectedItems(); }" />
                        </dxe:ASPxButton>
                    </div>
                    <div class="TopPadding">
                        <dxe:ASPxButton ID="btnMoveAllItemsToRight" runat="server" ClientInstanceName="btnMoveAllItemsToRight"
                            AutoPostBack="False" Text="Add All >>" Width="130px" cssClass="btn btn-primary" ToolTip="Add all items">
                            <clientsideevents click="function(s, e) { AddAllItems(); }" />
                        </dxe:ASPxButton>
                    </div>
                    <div style="height: 32px">
                    </div>
                    <div>
                        <dxe:ASPxButton ID="btnMoveSelectedItemsToLeft" runat="server" ClientInstanceName="btnMoveSelectedItemsToLeft"
                            AutoPostBack="False" Text="< Remove" Width="130px" cssClass="btn btn-danger" ClientEnabled="False"
                            ToolTip="Remove selected items">
                            <clientsideevents click="function(s, e) { RemoveSelectedItems(); }" />
                        </dxe:ASPxButton>
                    </div>
                    <div class="TopPadding">
                        <dxe:ASPxButton ID="btnMoveAllItemsToLeft" runat="server" ClientInstanceName="btnMoveAllItemsToLeft"
                            AutoPostBack="False" Text="<< Remove All" Width="130px" cssClass="btn btn-danger" ClientEnabled="False"
                            ToolTip="Remove all items">
                            <clientsideevents click="function(s, e) { RemoveAllItems(); }" />
                        </dxe:ASPxButton>
                    </div>
                </td>
                <td style="width:45%">
                    <dxe:ASPxListBox ID="lbChoosen" runat="server" ClientInstanceName="lbChoosen"
                        Width="350px" Height="240px" SelectionMode="CheckColumn" Caption="Selected Name">
                        <captionsettings position="Top" />
                        <clientsideevents selectedindexchanged="function(s, e) { UpdateButtonState(); }">
                    </clientsideevents>
                    </dxe:ASPxListBox>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                   
                    <asp:Button ID="BtnAdd" runat="server" Text="Save" OnClick="BtnAdd_Click" CssClass="btn btn-primary" />
                    <asp:Button ID="btnCancel" runat="server" OnClick="btncancel_click" Text="Cancel" CssClass="btn btn-danger" />
                </td>
            </tr>


        </table>
    </div>
</asp:Content>--%>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .dxgvControl_PlasticBlue a {
            color: #fff !important;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function OnUpdateClick(editor) {
            if (ASPxClientEdit.ValidateGroup("editForm"))
                gridBank.UpdateEdit();
        }

        function isValid() {
            var retvalue = true;

            if (document.getElementById('BankDetailsGrid_DXPEForm_efnew_txtbankname').value.trim() = '') {
                retvalue = false;
            }
            return retvalue;
        }
        function callAjax(obj, obj1, obj2, obj3) {
            var o = document.getElementById("SearchCombo")
            ajax_showOptions(obj, obj1, obj2, o.value)
        }
        function chkAct(str12, str) {
            var str = document.getElementById(str)
            str.value = str12;
        }

        function disp_prompt(name) {
            //var ID = document.getElementById(txtID);
            if (name == "tab0") {
                //alert(name);
                document.location.href = "Employee_general.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "Employee_Correspondence.aspx";
            }
            else if (name == "tab2") {
                //alert(name);
                document.location.href = "Employee_Education.aspx";
            }
            else if (name == "tab3") {
                //alert(name);
                document.location.href = "Employee_Employee.aspx";
            }
            else if (name == "tab4") {
                //alert(name);
                document.location.href = "Employee_Document.aspx";
            }
            else if (name == "tab5") {
                //alert(name);
                document.location.href = "Employee_FamilyMembers.aspx";
            }
            else if (name == "tab6") {
                //alert(name);
                document.location.href = "Employee_GroupMember.aspx";
            }
            else if (name == "tab7") {
                //alert(name);
                document.location.href = "Employee_EmployeeCTC.aspx";
            }
            else if (name == "tab8") {
                //alert(name);
                document.location.href = "Employee_BankDetails.aspx";
            }
            else if (name == "tab9") {
                //alert(name);
                document.location.href = "Employee_Remarks.aspx";
            }
            else if (name == "tab10") {
                //alert(name);
                //  document.location.href = "Employee_Remarks.aspx";
            }
            else if (name == "tab11") {
                //alert(name);
                //  document.location.href = "Employee_Education.aspx";
            }
            else if (name == "tab12") {
                //alert(name);
                // document.location.href="Employee_Subscription.aspx";
            }
            else if (name == "tab14") {
                //alert(name);
                document.location.href = "StatutoryDocumnentDtls.aspx";
            }
            else if (name == "tab15") {
                //alert(name);
                document.location.href = "PFESIDtls.aspx";
            }
            else if (name == "tab16") {
                //alert(name);
                document.location.href = "OtherDetails.aspx";
            }
            else if (name == "tab17") {
                //alert(name);
                document.location.href = "Resignation.aspx";
            }
        }
        function CallList(obj1, obj2, obj3) {
            var obj4 = '';
            //alert(valuse);
            if (valuse == 0)
                obj4 = 'bnk_bankName';
            if (valuse == 1)
                obj4 = 'bnk_Micrno';
            if (valuse == 2)
                obj4 = 'bnk_branchName';
            //alert(obj4);
            ajax_showOptions(obj1, obj2, obj3, obj4);
        }
        function setvaluetovariable(obj1) {
            valuse = obj1;
        }
        valuse = '0';
        FieldName = 'ASPxPageControl1_txtequity';
    </script>
    <style>
        .padingTbl > tbody > tr > td {
            padding: 5px 5px;
        }
    </style>
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Employee Bank</h3>
            <div class="crossBtn"><a href="employee.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>

    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td style="text-align: center">
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="15px" ForeColor="Navy"
                        Width="819px" Height="18px"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="13" ClientInstanceName="page">
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
                            <dxe:TabPage Name="Education" Text="Education">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Employee" Text="Employment">
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
                            <dxe:TabPage Name="Family Members" Text="Family">
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

                            <dxe:TabPage Name="Employee CTC" Text="CTC">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>

                            <dxe:TabPage Name="Bank Details" Text="Bank">
                                <ContentCollection>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Remarks" Text="UDF" Visible="false">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="DP Details" Text="DP" Visible="false">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>


                            <dxe:TabPage Name="Registration" Text="Registration" Visible="false">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>


                            <dxe:TabPage Name="Subscription" Text="Subscription" Visible="false">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>


                            <dxe:TabPage Name="Language"  Text="Language" >
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <div class="panel-heading">
                                            <div class="panel-title">
                                                <h3>Language Proficiency of 
                <asp:Label ID="lblHeader" runat="server"
                    Width="819px" Height="18px"></asp:Label></h3>
                                                <div class="crossBtn"><a href="employee_general.aspx"><i class="fa fa-times"></i></a></div>
                                                <div class="crossBtn">
                                                    <asp:LinkButton ID="goBackCrossBtn" runat="server" OnClick="goBackCrossBtn_Click"><i class="fa fa-times"></i></asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form_main" style="border: 1px solid #ccc; padding: 5px 15px">
                                            <table class="TableMain100">

                                                <tr>



                                                    <td style="width: 30%">
                                                       
                                                        <span style="margin-bottom: 8px; display: inline-block">Language Speak:</span>
                                                       <br />
                                                         <asp:Label runat="server" ID="lblspeaklanguages" ></asp:Label>
                                                        
                                                        <dxe:ASPxListBox ID="lbAvailable" runat="server" ClientInstanceName="lbAvailable"
                                                            Width="350px" Height="240px" SelectionMode="CheckColumn" Caption="">
                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonState(); }" />
                                                        </dxe:ASPxListBox>

                                                    </td>
                                                    <td style="width: 300px; text-align: center">
                                                        <div>
                                                            <dxe:ASPxButton ID="btnMoveSelectedItemsToRight" runat="server" ClientInstanceName="btnMoveSelectedItemsToRight"
                                                                AutoPostBack="False" Text="Add >" Width="130px" CssClass="btn btn-primary" ClientEnabled="False" Visible="false"
                                                                ToolTip="Add selected items">
                                                                <ClientSideEvents Click="function(s, e) { AddSelectedItems(); }" />
                                                            </dxe:ASPxButton>
                                                        </div>
                                                        <div class="TopPadding">
                                                            <dxe:ASPxButton ID="btnMoveAllItemsToRight" runat="server" ClientInstanceName="btnMoveAllItemsToRight"
                                                                AutoPostBack="False" Text="Add All >>" Width="130px" CssClass="btn btn-primary" ToolTip="Add all items" Visible="false">
                                                                <ClientSideEvents Click="function(s, e) { AddAllItems(); }" />
                                                            </dxe:ASPxButton>
                                                        </div>
                                                        <div style="height: 32px">
                                                        </div>
                                                        <div>
                                                            <dxe:ASPxButton ID="btnMoveSelectedItemsToLeft" runat="server" ClientInstanceName="btnMoveSelectedItemsToLeft"
                                                                AutoPostBack="False" Text="< Remove" Width="130px" CssClass="btn btn-danger" ClientEnabled="False"
                                                                ToolTip="Remove selected items" Visible="false">
                                                                <ClientSideEvents Click="function(s, e) { RemoveSelectedItems(); }" />
                                                            </dxe:ASPxButton>
                                                        </div>
                                                        <div class="TopPadding">
                                                            <dxe:ASPxButton ID="btnMoveAllItemsToLeft" runat="server" ClientInstanceName="btnMoveAllItemsToLeft"
                                                                AutoPostBack="False" Text="<< Remove All" Width="130px" CssClass="btn btn-danger" ClientEnabled="False"
                                                                ToolTip="Remove all items" Visible="false">
                                                                <ClientSideEvents Click="function(s, e) { RemoveAllItems(); }" />
                                                            </dxe:ASPxButton>
                                                        </div>
                                                    </td>
                                                    <td style="width: 45%">
                                                         <span style="margin-bottom: 8px; display: inline-block">Language Write:</span>
                                                       <br />
                                                           <asp:Label runat="server" ID="LitWrittenLanguage" ></asp:Label>
                                                        <br />
                                                        <dxe:ASPxListBox ID="lbChoosen" runat="server" ClientInstanceName="lbChoosen"
                                                            Width="350px" Height="240px" SelectionMode="CheckColumn" Caption="">
                                                            <CaptionSettings Position="Top" />
                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonWrite(); }"></ClientSideEvents>
                                                        </dxe:ASPxListBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <%--    <asp:Button ID="btnsubmit" runat="server" OnClick="btnsubmit_click" Text="Save" CssClass="btn btn-primary" />--%>
                                                        <asp:Button ID="BtnAdd" runat="server" Text="Save" OnClick="BtnAdd_Click" CssClass="btn btn-primary" />
                                                        <asp:Button ID="btnCancel" runat="server" OnClick="btncancel_click" Text="Cancel" CssClass="btn btn-danger" />
                                                    </td>
                                                </tr>


                                            </table>
                                        </div>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="StautoryDocumentDetails" Text="Statutory">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                             <dxe:TabPage Name="PFESI" Text="PF/ESI">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="othrdtls" Text="Other Details">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="RESIGNATION" Text="Resignation">
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
                                                var Tab13 = page.GetTab(13);
                                                var Tab14=page.GetTab(14)
                                                var Tab15 = page.GetTab(15);
                                                 var Tab16 = page.GetTab(16);
                                                 var Tab17 = page.GetTab(17);
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

                                                else if(activeTab == Tab16)
	                                            {
	                                                disp_prompt('tab16');
	                                            }
                            else if(activeTab == Tab17)
	                                            {
	                                                disp_prompt('tab17');
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
            <tr>
                <td>
                    <asp:TextBox ID="txtID" runat="server" Visible="false"></asp:TextBox>
                </td>
            </tr>
        </table>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
    <asp:SqlDataSource ID="SqlBank" runat="server" ></asp:SqlDataSource>
    <asp:SqlDataSource ID="BankDetails" runat="server" 
        SelectCommand="BankDetailsSelect" InsertCommand="BankDetailsInsert" InsertCommandType="StoredProcedure"
        SelectCommandType="StoredProcedure" UpdateCommand="BankDetailsUpdate" UpdateCommandType="StoredProcedure"
        DeleteCommand="INSERT INTO tbl_trans_contactBankDetails_Log(cbd_id, cbd_accountCategory, cbd_cntId, cbd_contactType, cbd_bankCode, cbd_accountNumber, cbd_accountType, cbd_accountName, CreateDate, CreateUser, LastModifyDate, LastModifyUser, LogModifyDate, LogModifyUser, LogStatus) SELECT cbd_id,cbd_accountCategory,cbd_cntId,cbd_contactType,cbd_bankCode,cbd_accountNumber,cbd_accountType, cbd_accountName,CreateDate, CreateUser, LastModifyDate, LastModifyUser,getdate(),@CreateUser,'D' FROM tbl_trans_contactBankDetails where cbd_id=@Id delete from tbl_trans_contactBankDetails where cbd_id=@Id">
        <SelectParameters>
            <asp:SessionParameter Name="insuId" SessionField="KeyVal_InternalID" Type="String" />
        </SelectParameters>
        <InsertParameters>
            <asp:Parameter Name="Category" Type="String" />
            <asp:SessionParameter Name="insuId" SessionField="KeyVal_InternalID" Type="String" />
            <asp:Parameter Name="BankName1" Type="String" />
            <asp:Parameter Name="AccountNumber" Type="String" />
            <asp:Parameter Name="AccountType" Type="String" />
            <asp:Parameter Name="AccountName" Type="String" />
            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="Category" Type="String" />
            <asp:Parameter Name="BankName1" Type="String" />
            <asp:Parameter Name="AccountNumber" Type="String" />
            <asp:Parameter Name="AccountType" Type="String" />
            <asp:Parameter Name="AccountName" Type="String" />
            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="String" />
            <asp:Parameter Name="Id" Type="String" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="int32" />
            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="String" />
        </DeleteParameters>
    </asp:SqlDataSource>
</asp:Content>
