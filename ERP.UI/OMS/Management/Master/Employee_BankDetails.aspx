<%@ Page Title="Bank" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    Inherits="ERP.OMS.Management.Master.management_master_Employee_BankDetails" CodeBehind="Employee_BankDetails.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dxe" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web.ASPxTabControl" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web.ASPxClasses" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--  <title>Bank Details</title>--%>

    <!--___________________________________________________________________________-->

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                document.location.href = "Employee_general.aspx?id=" + $("#hdKeyVal").val();
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
                //  document.location.href = "Employee_Employee.aspx";
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
            else if (name == "tab13") {
                //alert(name);
                var keyValue = $("#hdnlanguagespeak").val();
                document.location.href = 'frmLanguageProfi.aspx?id=' + keyValue + '&status=speak';
                //   document.location.href="Employee_Subscription.aspx"; 
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
                    <asp:Label ID="lblHeader" runat="server" Font-Bold="True" Font-Size="15px" ForeColor="Navy"
                        Width="819px" Height="18px"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="8" ClientInstanceName="page">
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
                                    <dxe:ContentControl runat="server">
                                        <div style="float: left">

                                            <% if (rights.CanAdd)
                                               { %>
                                            <a href="javascript:void(0);" onclick="gridBank.AddNewRow();" class="btn btn-primary pull-left"><span>Add New</span> </a><%} %>
                                            <%--<%} %>--%>
                                            <% if (rights.CanExport)
                                               { %>
                                            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" onchange="if(!AvailableExportOption()){ return false;}">
                                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                                <asp:ListItem Value="2">XLS</asp:ListItem>
                                                <asp:ListItem Value="3">RTF</asp:ListItem>
                                                <asp:ListItem Value="4">CSV</asp:ListItem>

                                            </asp:DropDownList>
                                            <% } %>
                                        </div>
                                        <asp:Label ID="lblmessage" runat="server" Text="" Font-Bold="True" ForeColor="Red"></asp:Label>
                                        <dxe:ASPxGridView ID="BankDetailsGrid" runat="server" ClientInstanceName="gridBank"
                                            DataSourceID="BankDetails" KeyFieldName="Id" AutoGenerateColumns="False" Width="100%"
                                            Font-Size="12px"
                                            OnRowInserting="BankDetailsGrid_RowInserting" OnRowValidating="BankDetailsGrid_RowValidating" OnRowUpdated="BankDetailsGrid_RowUpdated"
                                            OnRowUpdating="BankDetailsGrid_RowUpdating" OnCommandButtonInitialize="BankDetailsGrid_CommandButtonInitialize" OnStartRowEditing="BankDetailsGrid_StartRowEditing">
                                            <Columns>
                                                <dxe:GridViewDataTextColumn FieldName="Id" VisibleIndex="0" Visible="False" Caption="Type">
                                                    <EditFormSettings Caption="ID" Visible="False" />
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Category" Caption="Category" VisibleIndex="1"
                                                    Width="12%">
                                                    <EditFormSettings Caption="Category" Visible="true" />
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="AccountType" Caption="Account Type" VisibleIndex="5"
                                                    Width="12%">
                                                    <EditFormSettings Caption="AccountType" Visible="False" />
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="BankName" Caption="Bank Name" VisibleIndex="6"
                                                    Width="12%">
                                                    <EditFormSettings Caption="BankName" Visible="False" />
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="BankName1" Caption="BankName1" VisibleIndex="7"
                                                    Width="12%" Visible="false">
                                                    <EditFormSettings Caption="BankName1" Visible="False" />
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Branch" Caption="Branch" VisibleIndex="8"
                                                    Width="12%">
                                                    <EditFormSettings Caption="Branch" Visible="False" />
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="MICR" Caption="MICR" VisibleIndex="9" Width="12%">
                                                    <EditFormSettings Caption="MICR" Visible="False" />
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataComboBoxColumn Caption="Category" FieldName="Category" Visible="False"
                                                    VisibleIndex="2">
                                                    <PropertiesComboBox ValueType="System.String" EnableIncrementalFiltering="True">
                                                        <Items>

                                                            <dxe:ListEditItem Text="Default" Value="Default"></dxe:ListEditItem>

                                                            <dxe:ListEditItem Text="Secondary" Value="Secondary"></dxe:ListEditItem>

                                                        </Items>

                                                    </PropertiesComboBox>
                                                    <EditFormSettings Visible="True" />
                                                    <EditFormCaptionStyle VerticalAlign="Top" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataComboBoxColumn Caption="Account Type" FieldName="AccountType"
                                                    Visible="False" VisibleIndex="3">
                                                    <PropertiesComboBox ValueType="System.String" EnableIncrementalFiltering="True">
                                                        <Items>

                                                            <dxe:ListEditItem Text="Saving" Value="Saving"></dxe:ListEditItem>

                                                            <dxe:ListEditItem Text="Current" Value="Current"></dxe:ListEditItem>

                                                            <dxe:ListEditItem Text="Joint" Value="Joint"></dxe:ListEditItem>

                                                        </Items>

                                                    </PropertiesComboBox>
                                                    <EditFormSettings Visible="True" />
                                                    <EditFormCaptionStyle VerticalAlign="Top" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataTextColumn FieldName="AccountNumber" Caption="Account Number" VisibleIndex="10"
                                                    Width="12%">
                                                    <EditFormSettings Caption="AccountNumber" Visible="True" />
                                                    <EditFormCaptionStyle VerticalAlign="Top" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="AccountName" Caption="Account Name" VisibleIndex="11"
                                                    Width="12%">
                                                    <EditFormSettings Caption="AccountName" Visible="True" />
                                                    <EditFormCaptionStyle VerticalAlign="Top" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="BankName" Caption="Bank Name" VisibleIndex="4"
                                                    Visible="False">
                                                    <EditFormSettings Caption="BankName" Visible="True" />
                                                    <EditFormCaptionStyle VerticalAlign="Top" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewCommandColumn VisibleIndex="12" ShowEditButton="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="10%" ShowDeleteButton="True">
                                                    <%-- <DeleteButton Visible="True">
                                                        </DeleteButton>
                                                        <EditButton Visible="True">
                                                        </EditButton>--%>
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                    <CellStyle HorizontalAlign="Center"></CellStyle>
                                                    <HeaderTemplate>
                                                        Actions
                                                      <%--  <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                                          { %>
                                                        <a href="javascript:void(0);" onclick="gridBank.AddNewRow();"><span>Add New</span> </a>
                                                        <%} %>--%>
                                                    </HeaderTemplate>
                                                </dxe:GridViewCommandColumn>
                                            </Columns>
                                            <SettingsCommandButton>
                                                <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                                                    <Image AlternateText="Edit" Url="../../../assests/images/Edit.png"></Image>

                                                    <Styles>
                                                        <Style CssClass="pad"></Style>
                                                    </Styles>
                                                </EditButton>
                                                <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                    <Image AlternateText="Delete" Url="../../../assests/images/Delete.png"></Image>
                                                </DeleteButton>
                                                <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary">
                                                    <Styles>
                                                        <Style CssClass="btn btn-primary"></Style>
                                                    </Styles>
                                                </UpdateButton>
                                                <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger">
                                                    <Styles>
                                                        <Style CssClass="btn btn-danger"></Style>
                                                    </Styles>
                                                </CancelButton>
                                            </SettingsCommandButton>
                                            <%--<Settings ShowTitlePanel="True" />--%>
                                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" />
                                            <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="320px" PopupEditFormHorizontalAlign="WindowCenter"
                                                PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="400px"
                                                EditFormColumnCount="1" />
                                            <Styles>
                                                <LoadingPanel ImageSpacing="10px">
                                                </LoadingPanel>
                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                </Header>
                                            </Styles>
                                            <SettingsSearchPanel Visible="True" />
                                            <SettingsText PopupEditFormCaption="Add Bank Details" ConfirmDelete="Confirm delete?" />
                                            <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                                                <FirstPageButton Visible="True">
                                                </FirstPageButton>
                                                <LastPageButton Visible="True">
                                                </LastPageButton>
                                            </SettingsPager>
                                            <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                                            <Templates>
                                                <EditForm>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="text-align: center;">
                                                                <table class="padingTbl">
                                                                    <tr>
                                                                        <td align="left" style="padding-left: 30px">Category: <span style="font-size: 7.5pt; color: red"><strong>*</strong></span></td>
                                                                        <td style="text-align: left;" colspan="2">
                                                                            <dxe:ASPxComboBox ID="drpCategory" runat="server" ValueType="System.String" Width="203px"
                                                                                Value='<%#Bind("Category") %>' SelectedIndex="0" EnableIncrementalFiltering="true">
                                                                                <Items>
                                                                                    <dxe:ListEditItem Text="Default" Value="Default" />
                                                                                    <dxe:ListEditItem Text="Secondary" Value="Secondary" />
                                                                                </Items>
                                                                                <%--  <ValidationSettings CausesValidation="True" SetFocusOnError="True">
                                                                                    <RequiredField IsRequired="True" ErrorText="Select category" />
                                                                                </ValidationSettings>--%>

                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="editForm" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                    <RequiredField ErrorText="Mandatory" IsRequired="True" />
                                                                                </ValidationSettings>
                                                                            </dxe:ASPxComboBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" style="padding-left: 30px">Account Type:<span style="font-size: 7.5pt; color: red"><strong>*</strong></span></td>
                                                                        <td style="text-align: left;" colspan="2">
                                                                            <dxe:ASPxComboBox ID="drpAccountType" runat="server" ValueType="System.String" Value='<%#Bind("AccountType") %>'
                                                                                Width="203px" SelectedIndex="0" EnableIncrementalFiltering="true">
                                                                                <Items>
                                                                                    <dxe:ListEditItem Text="Saving" Value="Saving" />
                                                                                    <dxe:ListEditItem Text="Current" Value="Current" />
                                                                                    <dxe:ListEditItem Text="Joint" Value="Joint" />
                                                                                </Items>
                                                                                <%--    <ValidationSettings CausesValidation="True" SetFocusOnError="True">
                                                                                    <RequiredField IsRequired="True" ErrorText="Select Account Type" />
                                                                                </ValidationSettings>--%>

                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="editForm" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                    <RequiredField ErrorText="Mandatory" IsRequired="True" />
                                                                                </ValidationSettings>

                                                                            </dxe:ASPxComboBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="text-align: left; padding-left: 30px">Search By:</td>
                                                                        <td>
                                                                            <dxe:ASPxComboBox ID="drpSearchBank" runat="server" ValueType="System.String" SelectedIndex="0"
                                                                                ClientInstanceName="combo" Width="203px">
                                                                                <Items>
                                                                                    <dxe:ListEditItem Text="Bank Name" Value="bnk_bankName" />
                                                                                    <%--Not savebank in employee edit Tanmoy mantis 21887--%>

                                                                                  <%--  <dxe:ListEditItem Text="MICR No" Value="bnk_Micrno" />
                                                                                    <dxe:ListEditItem Text="Branch Name" Value="bnk_branchName" />--%>

                                                                                   <%--Not savebank in employee edit Tanmoy mantis 21887--%>
                                                                                </Items>
                                                                                <ClientSideEvents ValueChanged="function(s,e){
                                                                                                    var indexr = s.GetSelectedIndex();
                                                                                                    cAspxBankCombo.PerformCallback(indexr);
                                                                                                    setvaluetovariable(indexr)
                                                                                                    }"
                                                                                    Init="function(s, e) {
                                                                                            cAspxBankCombo.PerformCallback('0');
                                                                                    }" />
                                                                            </dxe:ASPxComboBox>
                                                                        </td>
                                                                    </tr>
                                                                    <%--  <tr>
                                                                        <td align="left" style="padding-left: 30px">Bank Name:<span style="font-size: 7.5pt; color: red"><strong>*</strong></span></td>
                                                                        <td style="text-align: left;">
                                                                            <asp:TextBox ID="txtbankname" runat="server" Width="200px" Text='<%#Bind("BankName1") %>'></asp:TextBox>
                                                                            <asp:TextBox ID="txtbankname_hidden" runat="server" Visible="false"></asp:TextBox>
                                                                            <span id="Mandatorytxtbankname" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red;display:none;right: 46px; top: 156px;position: absolute;" title="Mandatory"></span>
                                                                        </td>

                                                                    </tr>--%>

                                                                    <%--added by debjyoti 20-12-2016--%>
                                                                    <tr>
                                                                        <td align="left" style="padding-left: 30px">Bank Name:<span style="font-size: 7.5pt; color: red"><strong>*</strong></span></td>
                                                                        <td style="text-align: left;">
                                                                            <dxe:ASPxComboBox ID="AspxBankCombo" runat="server" ValueType="System.String" SelectedIndex="-1" ValueField="id" TextField="name"
                                                                                Value='<%#Bind("BankName1") %>' ClientInstanceName="cAspxBankCombo" Width="203px" OnCallback="AspxBankCombo_CustomCallback">

                                                                                <%--<ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                    <RequiredField ErrorText="Mandatory" IsRequired="True" />
                                                                                </ValidationSettings>--%>
                                                                                <ValidationSettings ValidationGroup="editForm" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right">
                                                                                    <RequiredField IsRequired="True" ErrorText="Mandatory." />
                                                                                </ValidationSettings>
                                                                            </dxe:ASPxComboBox>
                                                                        </td>
                                                                    </tr>
                                                                    <%--End debjyoti 20-12-2016--%>
                                                                    <tr>
                                                                        <td align="left" style="padding-left: 30px">Account Number:</td>
                                                                        <td style="text-align: left;" colspan="2">
                                                                            <asp:TextBox ID="txtAccountNo" runat="server" Text='<%#Bind("AccountNumber") %>'
                                                                                Width="200px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" style="padding-left: 30px">Account Name:</td>
                                                                        <td style="text-align: left;" colspan="2">
                                                                            <asp:TextBox ID="txtAnccountName" runat="server" Text='<%#Bind("AccountName") %>'
                                                                                Width="200px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2" style="padding-left: 74px">
                                                                            <dxe:ASPxButton ID="btnUpdate" runat="server" Text="Save" ToolTip="Update data"
                                                                                AutoPostBack="false" CssClass="btn btn-primary">
                                                                                <%--  <ClientSideEvents Click="function(s, e) {
                                                                                     gridBank.UpdateEdit(); 
                                                                                    }" />--%>
                                                                                <ClientSideEvents Click="function(s, e) {
                                                                                     OnUpdateClick(this); 
                                                                                    }" />
                                                                            </dxe:ASPxButton>
                                                                            <dxe:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel data"
                                                                                AutoPostBack="False" CssClass="btn btn-danger">
                                                                                <ClientSideEvents Click="function(s, e) {gridBank.CancelEdit();}" />
                                                                            </dxe:ASPxButton>
                                                                        </td>
                                                                        <td style="text-align: left;" colspan="2"></td>

                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EditForm>

                                            </Templates>
                                        </dxe:ASPxGridView>
                                        <br />
                                        <asp:Panel ID="BankDetailsPanel" runat="server" Width="100%" Visible="false">
                                            <table border="1" width="100%">
                                                <tr>
                                                    <td colspan="2" style="background-color: #A9D4FA; text-align: center; height: 18px;">
                                                        <span style="font-size: 7.5pt"><strong>Investment</strong></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 40%" valign="top">
                                                        <table width="100%">
                                                            <tr>
                                                                <td class="Ecoheadtxt" style="color: Blue; text-align: right; width: 154px;">
                                                                    <span style="font-size: 7.5pt">Gross Annual Salary </span>
                                                                </td>
                                                                <td>:</td>
                                                                <td class="Ecoheadtxt" style="color: Blue; text-align: left;">
                                                                    <span style="font-size: 7.5pt">Rs.</span><asp:TextBox ID='txtgrossannualsalary' runat="server"
                                                                        Width="50px" Font-Size="12px" ForeColor="Blue"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="Ecoheadtxt" style="color: Blue; width: 154px; text-align: right;">
                                                                    <span style="font-size: 7.5pt">Annual Trunover </span>
                                                                </td>
                                                                <td>:</td>
                                                                <td class="Ecoheadtxt" style="color: Blue">
                                                                    <span style="font-size: 7.5pt">Rs.</span><asp:TextBox ID='txtannualTrunover' runat="server"
                                                                        Width="50px" Font-Size="12px" ForeColor="Blue"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="Ecoheadtxt" style="color: Blue; width: 154px; text-align: right;">
                                                                    <span style="font-size: 7.5pt">Gross Profit </span>
                                                                </td>
                                                                <td>:</td>
                                                                <td class="Ecoheadtxt" style="color: Blue">
                                                                    <span style="font-size: 7.5pt">Rs.</span><asp:TextBox ID='txtGrossProfit' runat="server"
                                                                        Width="50px" Font-Size="12px" ForeColor="Blue"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="Ecoheadtxt" style="color: Blue; width: 154px; text-align: right;">
                                                                    <span style="font-size: 7.5pt">Approx. Expenses (PM) </span>
                                                                </td>
                                                                <td>:</td>
                                                                <td class="Ecoheadtxt" style="color: Blue">
                                                                    <span style="font-size: 7.5pt">Rs.</span><asp:TextBox ID='txtPMExpenses' runat="server"
                                                                        Width="50px" Font-Size="12px" ForeColor="Blue"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="Ecoheadtxt" style="color: Blue; width: 154px; text-align: right;">
                                                                    <span style="font-size: 7.5pt">Approx. Saving (PM) </span>
                                                                </td>
                                                                <td>:</td>
                                                                <td class="Ecoheadtxt" style="color: Blue">
                                                                    <span style="font-size: 7.5pt">Rs.</span><asp:TextBox ID='txtPMSaving' runat="server"
                                                                        Width="50px" Font-Size="12px" ForeColor="Blue"></asp:TextBox></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 60%">
                                                        <table width="100%">
                                                            <tr>
                                                                <td style="width: 225px">
                                                                    <table>
                                                                        <tr>
                                                                            <td class="Ecoheadtxt" style="color: Blue; width: 118px; text-align: right;">
                                                                                <span style="font-size: 7.5pt">Equity</span></td>
                                                                            <td>:</td>
                                                                            <td class="Ecoheadtxt" style="color: Blue">
                                                                                <span style="font-size: 7.5pt">Rs.</span><asp:TextBox ID="txtequity" runat="server"
                                                                                    Width="50px" Font-Size="12px" ForeColor="Blue"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="Ecoheadtxt" style="color: Blue; width: 118px; text-align: right;">
                                                                                <span style="font-size: 7.5pt">Mutal Fund</span></td>
                                                                            <td>:</td>
                                                                            <td class="Ecoheadtxt" style="color: Blue">
                                                                                <span style="font-size: 7.5pt">Rs.</span><asp:TextBox ID="txtMutalFund" runat="server"
                                                                                    Width="50px" Font-Size="12px" ForeColor="Blue"></asp:TextBox></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="Ecoheadtxt" style="color: Blue; width: 118px; text-align: right;">
                                                                                <span style="font-size: 7.5pt">Bank FD's</span></td>
                                                                            <td>:</td>
                                                                            <td class="Ecoheadtxt" style="color: Blue">
                                                                                <span style="font-size: 7.5pt">Rs.</span><asp:TextBox ID="txtBankFD" runat="server"
                                                                                    Width="50px" Font-Size="12px" ForeColor="Blue"></asp:TextBox></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="Ecoheadtxt" style="color: Blue; width: 118px; text-align: right;">
                                                                                <span style="font-size: 7.5pt">Debt's Instruments</span></td>
                                                                            <td>:</td>
                                                                            <td class="Ecoheadtxt" style="color: Blue">
                                                                                <span style="font-size: 7.5pt">Rs.</span><asp:TextBox ID="txtDebtsInstruments" runat="server"
                                                                                    Width="50px" Font-Size="12px" ForeColor="Blue"></asp:TextBox></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="Ecoheadtxt" style="color: Blue; width: 118px; text-align: right;">
                                                                                <span style="font-size: 7.5pt">NSS's</span></td>
                                                                            <td>:</td>
                                                                            <td class="Ecoheadtxt" style="color: Blue">
                                                                                <span style="font-size: 7.5pt">Rs.</span><asp:TextBox ID="txtNSS" runat="server"
                                                                                    Width="50px" Font-Size="12px" ForeColor="Blue"></asp:TextBox></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td class="Ecoheadtxt" style="color: Blue; text-align: right;">
                                                                                <span style="font-size: 7.5pt">Life Insurance</span></td>
                                                                            <td>:</td>
                                                                            <td class="Ecoheadtxt" style="color: Blue">
                                                                                <span style="font-size: 7.5pt">Rs.</span><asp:TextBox ID="txtLifeInsurance" runat="server"
                                                                                    Width="50px" Font-Size="12px" ForeColor="Blue"></asp:TextBox></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="Ecoheadtxt" style="color: Blue; text-align: right;">
                                                                                <span style="font-size: 7.5pt">Health Insurance</span></td>
                                                                            <td>:</td>
                                                                            <td class="Ecoheadtxt" style="color: Blue">
                                                                                <span style="font-size: 7.5pt">Rs.</span><asp:TextBox ID="txtHealthInsurance" runat="server"
                                                                                    Width="50px" Font-Size="12px" ForeColor="Blue"></asp:TextBox></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="Ecoheadtxt" style="color: Blue; text-align: right;">
                                                                                <span style="font-size: 7.5pt">Real Estate</span></td>
                                                                            <td>:</td>
                                                                            <td class="Ecoheadtxt" style="color: Blue">
                                                                                <span style="font-size: 7.5pt">Rs.</span><asp:TextBox ID="txtRealEstate" runat="server"
                                                                                    Width="50px" Font-Size="12px" ForeColor="Blue"></asp:TextBox></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="Ecoheadtxt" style="color: Blue; text-align: right;">
                                                                                <span style="font-size: 7.5pt">Precious Metals/Stones</span></td>
                                                                            <td>:</td>
                                                                            <td class="Ecoheadtxt" style="color: Blue">
                                                                                <span style="font-size: 7.5pt">Rs.</span><asp:TextBox ID="txtPreciousMetals" runat="server"
                                                                                    Width="50px" Font-Size="12px" ForeColor="Blue"></asp:TextBox></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="Ecoheadtxt" style="color: Blue; text-align: right;">
                                                                                <span style="font-size: 7.5pt">Other's</span></td>
                                                                            <td>:</td>
                                                                            <td class="Ecoheadtxt" style="color: Blue">
                                                                                <span style="font-size: 7.5pt">Rs.</span><asp:TextBox ID="txtOthers" runat="server"
                                                                                    Width="50px" Font-Size="12px" ForeColor="Blue"></asp:TextBox></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td class="Ecoheadtxt" style="color: Blue; text-align: right;">
                                                                    <span style="font-size: 7.5pt">Has Fund For Investment </span>
                                                                </td>
                                                                <td>:</td>
                                                                <td class="Ecoheadtxt" style="color: Blue">
                                                                    <asp:CheckBox ID="chkHasFundInvestment" runat="server" OnCheckedChanged="chkHasFundInvestment_CheckedChanged"
                                                                        Font-Size="12px" ForeColor="Blue" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="Ecoheadtxt" style="color: Blue; text-align: right;">
                                                                    <span style="font-size: 7.5pt">If Yes Then Availabe Funds </span>
                                                                </td>
                                                                <td>:</td>
                                                                <td class="Ecoheadtxt" style="color: Blue">
                                                                    <asp:TextBox ID="txtAvailableFund" runat="server" Width="50px" Font-Size="12px" ForeColor="Blue" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="Ecoheadtxt" style="color: Blue; text-align: right;">
                                                                    <span style="font-size: 7.5pt">If Yes Then Investment Horizon </span>
                                                                </td>
                                                                <td>:</td>
                                                                <td class="Ecoheadtxt" style="color: Blue">
                                                                    <asp:TextBox ID="txtInvestmentHorizon" runat="server" Width="50px" Font-Size="12px"
                                                                        ForeColor="Blue" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td class="Ecoheadtxt" style="color: Blue; width: 119px; text-align: right;">
                                                                                <span style="font-size: 7.5pt">Ready to Transfer Existing Portfoilio </span>
                                                                            </td>
                                                                            <td>:</td>
                                                                            <td class="Ecoheadtxt" style="color: Blue">
                                                                                <asp:CheckBox ID="chkPortFoilio" runat="server" OnCheckedChanged="chkPortFoilio_CheckedChanged"
                                                                                    Font-Size="12px" ForeColor="Blue" /></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="Ecoheadtxt" style="color: Blue; width: 119px; text-align: right;">
                                                                                <span style="font-size: 7.5pt">If Yes Then Amount </span>
                                                                            </td>
                                                                            <td>:</td>
                                                                            <td class="Ecoheadtxt" style="color: Blue">
                                                                                <asp:TextBox ID="TxtPortFoilioAmount" runat="server" Width="50px" Font-Size="12px"
                                                                                    ForeColor="Blue" /></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td class="Ecoheadtxt" style="color: Blue; width: 138px; text-align: right;">
                                                                                <span style="font-size: 7.5pt">&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; Own House </span>
                                                                            </td>
                                                                            <td style="width: 3px">:</td>
                                                                            <td class="Ecoheadtxt" style="color: Blue">
                                                                                <asp:CheckBox ID="chkhouse" runat="server" Font-Size="12px" ForeColor="Blue" /></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="Ecoheadtxt" style="color: Blue; width: 138px; text-align: right;">
                                                                                <span style="font-size: 7.5pt">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; Own Vehicle
                                                                                </span>
                                                                            </td>
                                                                            <td style="width: 3px">:</td>
                                                                            <td class="Ecoheadtxt" style="color: Blue">
                                                                                <asp:CheckBox ID="chkVehicle" runat="server" Font-Size="12px" ForeColor="Blue" /></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center" style="text-align: right">
                                                        <asp:Button ID="btn_Finance_Save" runat="server" Text="Save" OnClick="btn_Finance_Save_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </dxe:ContentControl>
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
                            <dxe:TabPage Name="Language" Text="Language">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
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
                                                var Tab14 = page.GetTab(14);
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
    <asp:HiddenField ID="hdKeyVal" runat="server" />
</asp:Content>

