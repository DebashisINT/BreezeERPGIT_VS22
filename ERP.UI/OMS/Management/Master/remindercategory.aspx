<%@ Page title="Reminder/Task Category" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Master.management_master_remindercategory" CodeBehind="remindercategory.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../../../Scripts/jquery-1.10.2.min.js"></script>
    <script type="text/javascript">

        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }


        function LastCall(obj) {
            if (Action == '')
                $("#divAddNew").hide();
           // cPopup_Empcitys.hide();
            //MakeRowInVisible();
        }

        function MakeRowVisible() {
            $("#divAddNew").show();
            cPopup_Empcitys.Show();
            Action = 'add';
            //document.getElementById("SaveRow").style.display = 'inline';
            Status = 'SAVE_NEW';
            ctxtcat_desc.SetText('');
            $("#<%=txt_description.ClientID%>").val("");

            GetObjectID('hiddenedit').value = '';
            //document.getElementById("txtcat_desc").value = '';

        }
        function MakeRowInVisible() {
            $('#MandatoryName').css({ 'display': 'none' });
            $("#divAddNew").hide();
            GetObjectID('hiddenedit').value = '';
            window.location.href = "ReminderCategory.aspx";
            // document.getElementById("SaveRow").style.display = 'none';
        }
        function Call_save() {

            if (validate()) {
                AddPanel.PerformCallback(Status);
                window.location.href = "ReminderCategory.aspx";
            }
        }
        function Call_edit() {
            AddPanel.PerformCallback('edit');
        }
        function PEndCallBack(obj) {
            if (obj == 'Y') {
                Action = '';
                ShowHideFilter('All');
            }
        }
        function OnEdit(obj) {
            Action = 'edit';
            //document.getElementById("SaveRow").style.display = 'inline';
            document.getElementById('hiddenedit').value = obj.split('~')[1];
            $("#divAddNew").show();
            cPopup_Empcitys.Show();
            Status = obj;
            AddPanel.PerformCallback('BEFORE_' + obj);
        }

        function validate() {
            var shortName = document.getElementById('<%=txt_description.ClientID %>').value;

            if (shortName.trim() == "") {
                $('#MandatoryName').css({ 'display': 'block' });
                // document.getElementById('<%=txt_description.ClientID %>').style.border = "1px solid red";
                //shortName.style.borderWidth = "1px";
                //shortName.style.borderStyle = "solid";
                //shortName.style.borderColor = "red";
                // alert("Enter short name");
                return false;
            }
            else {
                //  document.getElementById('<%=txt_description.ClientID %>').style.border = "none";
                return true;
            }


        }
        function uniqueCheck(sValue) {
            var code = 0;
            if (GetObjectID('hiddenedit').value != '') {
                code = GetObjectID('hiddenedit').value;
            }
            var reminderShortName = sValue;
            var CheckUniqueCode = false;
            $.ajax({
                type: "POST",
                url: "remindercategory.aspx/CheckUniqueCode",
                data: JSON.stringify({ CategoriesShortCode: reminderShortName, Code: code }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    CheckUniqueCode = msg.d;
                    if (CheckUniqueCode != true) {
                        jAlert('Please enter unique short name');
                        document.getElementById('<%=txt_description.ClientID %>').value = '';
                        document.getElementById('<%=txt_description.ClientID %>').Focus();
                    }
                }
            });

        }
        function removeValidation() {
            var shortValue = document.getElementById('<%=txt_description.ClientID %>').value;
            if (shortValue.trim() == "") {
                $('#MandatoryName').css({ 'display': 'block' });
                return false;
            }
            else {
                $('#MandatoryName').css({ 'display': 'none' });
            }
            uniqueCheck(shortValue);
            //document.getElementById('<%=txt_description.ClientID %>').style.border = "none";
        }
    </script>
    <style>
        .pullleftClass {
            position: absolute;
            right: 0;
            top: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Reminder/Task Category</h3>
            <asp:HiddenField runat="server" ID="hiddenedit" />
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">

            <tr>
                <td colspan="4">
                    <table class="TableMain100">
                        <tr>
                            <td colspan="4" style="text-align: left; vertical-align: top">
                                <table>
                                    <tr>
                                        <td id="ShowFilter">
                                            <% if (rights.CanAdd)
                                               { %>
                                            <asp:HyperLink ID="HyperLink2" runat="server"
                                                NavigateUrl="javascript:void(0)" onclick="javascript:MakeRowVisible()" class="btn btn-primary">Add New</asp:HyperLink>
                                            <% } %>
                                            <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-success"><span>Show Filter</span></a>--%>
                                        </td>
                                        <td id="Td1">
                                            <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="gridcellright"></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr runat="server" id="SaveRow">
                <td>
                    <div id="divAddNew">

                        <dxe:ASPxPopupControl ID="Popup_Empcitys" runat="server" ClientInstanceName="cPopup_Empcitys"
                            Width="400px" HeaderText="Add Reminder/Task Category" PopupHorizontalAlign="WindowCenter"
                            BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                            ContentStyle-CssClass="pad">
                            <ContentStyle VerticalAlign="Top" CssClass="pad">
                            </ContentStyle>
                            <ContentCollection>
                                <dxe:PopupControlContentControl runat="server">
                                    <dxe:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" ClientInstanceName="AddPanel"
                                        OnCallback="ASPxCallbackPanel1_Callback" OnCustomJSProperties="ASPxCallbackPanel1_CustomJSProperties">
                                        <PanelCollection>
                                            <dxe:PanelContent runat="server">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <label>Shortname <span style="color: red">*</span></label>
                                                        <div class="relative">
                                                            <asp:TextBox ID="txt_description" runat="server" Width="95%" MaxLength="50" onblur="javascript:removeValidation();"></asp:TextBox>
                                                            <span id="MandatoryName" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; display: none" title="Mandatory"></span>
                                                        </div>

                                                    </div>
                                                    <div class="col-md-12">
                                                        <label>Description :</label>
                                                        <div>
                                                            <%-- <asp:TextBox ID="txtcat_desc" runat="server" Width="520px"  Height="50px" TextMode="MultiLine"></asp:TextBox>--%>
                                                            <dxe:ASPxMemo ID="txtcat_desc" runat="server" MaxLength="4000" Width="95%" Height="100px" ClientInstanceName="ctxtcat_desc">
                                                            </dxe:ASPxMemo>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12" style="padding-top: 15px">
                                                        <input id="btnSave" class="btn btn-primary" onclick="Call_save(status)" type="button" value="Save" />
                                                        <input id="btnCancel" class="btn btn-danger" onclick="MakeRowInVisible()" type="button" value="Cancel" />
                                                    </div>
                                                </div>

                                            </dxe:PanelContent>
                                        </PanelCollection>
                                        <ClientSideEvents EndCallback="function(s, e) {
	                                                    PEndCallBack(s.cpPanel);
                                                    }" />
                                    </dxe:ASPxCallbackPanel>
                                </dxe:PopupControlContentControl>
                            </ContentCollection>
                        </dxe:ASPxPopupControl>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxGridView ID="gridCategory" runat="server" ClientInstanceName="grid" AutoGenerateColumns="False"
                        DataSourceID="SqlDataSource1"  SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"  KeyFieldName="id" Width="100%" OnHtmlEditFormCreated="gridCategory_HtmlEditFormCreated"
                        OnHtmlRowCreated="gridCategory_HtmlRowCreated" OnCustomCallback="gridCategory_CustomCallback"
                        OnCustomJSProperties="gridCategory_CustomJSProperties">
                        <SettingsSearchPanel Visible="true" Delay="6000" />
                        <Styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="id" ReadOnly="True" Visible="False" VisibleIndex="0">
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="cat_description" Caption="Description"
                                VisibleIndex="0" Width="70%">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="cat_applicablefor" Caption="Shortname"
                                VisibleIndex="1" Width="30%">

                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />

                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="" VisibleIndex="3" Width="6%">
                                <CellStyle HorizontalAlign="Center">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    Actions
                                    <%--<asp:HyperLink ID="HyperLink2" runat="server"
                                        NavigateUrl="javascript:void(0)" CssClass="myhypertext" onclick="javascript:MakeRowVisible()">Add New</asp:HyperLink>--%>
                                </HeaderTemplate>
                                <DataItemTemplate>
                                    <% if (rights.CanEdit)
                                       { %>
                                    <a href="javascript:void(0);" onclick="OnEdit('EDIT~'+'<%# Container.KeyValue %>')">
                                        <img src="/assests/images/Edit.png" alt="Edit">
                                    </a>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<% } %>
                                </DataItemTemplate>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <SettingsCommandButton>

                            <EditButton Image-Url="/assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit">
                            </EditButton>
                            <DeleteButton Image-Url="/assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                            </DeleteButton>
                            <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary" Image-Width></UpdateButton>
                            <CancelButton Text="Cancel" ButtonType="Button"></CancelButton>
                        </SettingsCommandButton>
                        <StylesEditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </StylesEditors>
                       
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="200px" PopupEditFormHorizontalAlign="Center"
                            PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="600px"
                            EditFormColumnCount="1" />
                        <SettingsText PopupEditFormCaption="Add/Modify Category" ConfirmDelete="Confirm delete?" />
                        <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <SettingsBehavior ConfirmDelete="True" />

                        <ClientSideEvents EndCallback="function(s, e) {
	LastCall(s.cpHeight);
}" />
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
            DeleteCommand="DELETE FROM [tbl_master_remarksCategory] WHERE [id] = @id" InsertCommand="INSERT INTO [tbl_master_remarksCategory] ([cat_description],[cat_applicablefor]) VALUES (@cat_description,@cat_applicablefor)"
            SelectCommand="SELECT id,cat_description,case cat_applicablefor when 'Em' then 'Employee' when 'Ld' then 'Lead' when 'Sb' then 'Sub Broker' when 'fr' then 'Franchisses' when 'DV' then 'Data Vendors' when 'Cus' then 'Customer' when 'RP' then 'Relationship Partner' when 'BP' then 'Business Partner' when 'BP' then 'Business Partner' else 'Recruitment Agents' end as cat_applicablefor FROM [tbl_master_remarksCategory]"
            UpdateCommand="UPDATE [tbl_master_remarksCategory] SET [cat_description] = @cat_description,cat_applicablefor=@cat_applicablefor WHERE [id] = @id">
            <DeleteParameters>
                <asp:Parameter Name="id" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="cat_description" Type="String" />
                <asp:Parameter Name="cat_applicablefor" Type="String" />
                <asp:Parameter Name="id" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="cat_description" Type="String" />
                <asp:Parameter Name="cat_applicablefor" Type="String" />
            </InsertParameters>
        </asp:SqlDataSource>
        <br />
    </div>
</asp:Content>
