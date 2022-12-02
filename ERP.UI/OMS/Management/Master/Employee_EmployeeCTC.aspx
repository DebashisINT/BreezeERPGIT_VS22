<%@ Page Title="CTC" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    Inherits="ERP.OMS.Management.Master.management_master_Employee_EmployeeCTC" CodeBehind="Employee_EmployeeCTC.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxe" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dxe" %>
<%@ Register Assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="System.Web.UI" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<title>CTC</title>--%>


    <script type="text/javascript">
        function Show() {
           <%-- var ActiveUser = '<%=Session["userid"]%>'
            if (ActiveUser != null) {
                $.ajax({
                    type: "POST",
                    url: "Employee_EmployeeCTC.aspx/IsUserLoggedIn",                  
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var status = msg.d;
                        var url = 'PurchaseInvoice.aspx?key=' + keyValue + '&Permission=' + status + '&type=PB';
                        window.location.href = url;
                    }
                });
            }--%>


            var url = "frmEmployeeCTC.aspx?link=Employee_EmployeeCTC.aspx&id=ADD&ContID=N";           
            window.location = url;
        }
        function EditFormShow(obj) {
            var intid = obj;
            // var url = "frmAddDocuments.aspx?id=Employee_Document.aspx&id1=Employee";
            //alert(intid);
            var url = "frmEmployeeCTC.aspx?link=Employee_EmployeeCTC.aspx&id=" + intid + "&ContID=N";
            //popup.SetContentUrl(url);
            //popup.Show();
            window.location = url;
        }

        function DeleteRow(keyValue) {
            doIt = confirm('Confirm delete?');
            if (doIt) {
                EmployeeCTC.PerformCallback('Delete~' + keyValue);
                height();
            }
            else {

            }


        }

        function GridEndCall() {
            if (EmployeeCTC.cpmsg != null) {
                jAlert(EmployeeCTC.cpmsg);
            }
        }
    </script>
    <style>
        .dxgvFooter_PlasticBlue {
            background-color: none !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">
        function disp_prompt(name) {
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
                document.location.href = "Employee_Employee.aspx";
                //alert(name);
                // document.location.href="Employee_DPDetails.aspx"; 
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
                //document.location.href = "Employee_GroupMember.aspx";
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
                // document.location.href = "Employee_Remarks.aspx";
            }
            else if (name == "tab11") {
                //alert(name);
                //  document.location.href = "Employee_Education.aspx";
            }

            else if (name == "tab12") {
                //alert(name);
                //   document.location.href="Employee_Subscription.aspx";
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



    </script>

    <div class="panel-heading">
        <div class="panel-title">
            <h3>Employee CTC</h3>
            <div class="crossBtn"><a href="employee.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td style="text-align: center">
                    <asp:Label ID="lblHeader" runat="server" Font-Bold="True" Font-Size="15px" ForeColor="Navy"
                        Width="819px" Height="18px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="7" ClientInstanceName="page">
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
                                        <div style="float: left">
                                            <%-- <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                                              { %>--%>
                                            <% if (rights.CanAdd)
                                               { %>
                                            <a href="javascript:void(0);" onclick="Show();" class="btn btn-primary pull-left"><span>Add New</span> </a><%} %>
                                            <%-- <%} %>--%>
                                        </div>
                                        <dxe:ASPxGridView ID="EmployeeCTC" runat="server" AutoGenerateColumns="False" DataSourceID="EployeeCTC"
                                            KeyFieldName="Id" Width="100%" ClientInstanceName="EmployeeCTC" Font-Size="12px"
                                            OnHtmlRowCreated="EmployeeCTC_HtmlRowCreated" OnInitNewRow="EmployeeCTC_InitNewRow"
                                            OnCellEditorInitialize="EmployeeCTC_CellEditorInitialize" OnHtmlEditFormCreated="EmployeeCTC_HtmlEditFormCreated" OnCustomCallback="EmployeeCTC_CustomCallback">
                                            <Styles>
                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                </Header>
                                                <LoadingPanel ImageSpacing="10px">
                                                </LoadingPanel>
                                                <FocusedRow BackColor="#FFC080">
                                                </FocusedRow>
                                                <FocusedGroupRow BackColor="#FFC080">
                                                </FocusedGroupRow>
                                            </Styles>
                                            <Settings ShowFooter="True" ShowTitlePanel="True" />
                                            <SettingsBehavior AllowFocusedRow="False" ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                                            <Columns>
                                                <dxe:GridViewDataTextColumn Caption="Applicable From" FieldName="EffectiveDate1"
                                                    VisibleIndex="0">
                                                    <EditFormSettings Visible="False" />
                                                    <EditCellStyle>
                                                        <Border BorderColor="#C0C0FF" BorderStyle="Ridge" BorderWidth="1px" />
                                                    </EditCellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn Caption="Applicable To" FieldName="EffectiveUntil1"
                                                    VisibleIndex="1">
                                                    <EditFormSettings Visible="False" />
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Id" ReadOnly="True" VisibleIndex="2" Visible="False">
                                                    <EditFormSettings Visible="False" />
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn Caption="Designation" FieldName="Designation" VisibleIndex="2">
                                                    <EditFormSettings Visible="False" />
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="Department" VisibleIndex="3">
                                                    <EditFormSettings Visible="False" />
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="CompanyName" VisibleIndex="4">
                                                    <EditFormSettings Visible="False" />
                                                    <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="BranchName" ReadOnly="True" VisibleIndex="5">
                                                    <EditFormSettings Visible="False" />
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="8" Width="60px" Caption="Details">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <DataItemTemplate>
                                                        <% if (rights.CanEdit)
                                                           { %>
                                                        <a href="javascript:void(0);" onclick="EditFormShow('<%# Container.KeyValue %>')" title="Edit" class="pad">
                                                            <img src="../../../assests/images/Edit.png" />
                                                        </a><%} %>
                                                        <%-- <% if (rights.CanDelete)
                                                               { %>
                                                            <a href="javascript:void(0);" onclick="DeleteRow('<%# Container.KeyValue %>')" title="Delete" class="pad">
                                                                <img src="../../../assests/images/Delete.png" />
                                                            </a><%} %>--%>
                                                    </DataItemTemplate>
                                                    <CellStyle Wrap="False">
                                                    </CellStyle>
                                                    <HeaderTemplate>
                                                        Actions
                                                         <%--   <a href="javascript:void(0);" onclick="javascript:OnAddButtonClick();"><span style="color: #000099;
                                                                text-decoration: underline">Add New</span> </a>--%>
                                                        <%--  <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                                              { %>
                                                            <a href="javascript:void(0);" onclick="Show();"><span>
                                                                Add New</span> </a>
                                                            <%} %>--%>
                                                    </HeaderTemplate>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>


                                                <%-- <dxe:GridViewCommandColumn Name="command" VisibleIndex="7">
                                                        <EditButton Visible="True">
                                                        </EditButton>
                                                        <DeleteButton Visible="True">
                                                        </DeleteButton>
                                                        <ClearFilterButton Visible="True">
                                                        </ClearFilterButton>
                                                        <HeaderTemplate>
                                                            <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                                              { %>
                                                            <a href="javascript:void(0);" onclick="EmployeeCTC.AddNewRow();"><span style="color: #000099;
                                                                text-decoration: underline">Add New</span> </a>
                                                            <%} %>
                                                        </HeaderTemplate>
                                                    </dxe:GridViewCommandColumn>--%>
                                                <dxe:GridViewDataTextColumn FieldName="emp_id" ReadOnly="True" Visible="False"
                                                    VisibleIndex="5">
                                                    <EditFormSettings Visible="False" />
                                                    <EditFormCaptionStyle Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="emp_cntId" ReadOnly="True" Visible="False"
                                                    VisibleIndex="6">
                                                    <EditFormSettings Visible="False" />
                                                    <EditFormCaptionStyle Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataDateColumn FieldName="emp_dateofJoining" Visible="False" VisibleIndex="6">
                                                    <PropertiesDateEdit DisplayFormatString="" EditFormat="Custom" EditFormatString="dd MMMM yyyy"
                                                        UseMaskBehavior="True">
                                                    </PropertiesDateEdit>
                                                    <EditFormSettings Visible="True" VisibleIndex="1" Caption="Date Of Joining" />
                                                    <EditCellStyle BackColor="#FFF2C8">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle BackColor="#FFF2C8" HorizontalAlign="Right" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataDateColumn>
                                                <dxe:GridViewDataComboBoxColumn FieldName="emp_Organization" Visible="False" VisibleIndex="6">
                                                    <PropertiesComboBox DataSourceID="Organization" EnableIncrementalFiltering="True"
                                                        TextField="cmp_name" ValueField="cmp_id" ValueType="System.Int32">
                                                    </PropertiesComboBox>
                                                    <EditFormSettings Visible="True" VisibleIndex="2" Caption="Organization" />
                                                    <EditCellStyle BackColor="#FFF2C8">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle BackColor="#FFF2C8" HorizontalAlign="Right" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataComboBoxColumn FieldName="emp_JobResponsibility" Visible="False"
                                                    VisibleIndex="6">
                                                    <PropertiesComboBox DataSourceID="Responsibility" EnableIncrementalFiltering="True"
                                                        TextField="job_responsibility" ValueField="job_id" ValueType="System.Int32">
                                                    </PropertiesComboBox>
                                                    <EditFormSettings Visible="True" VisibleIndex="3" Caption="Job Responsibility" />
                                                    <EditCellStyle BackColor="#FFF2C8">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle BackColor="#FFF2C8" HorizontalAlign="Right" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataComboBoxColumn FieldName="emp_Designation" Visible="False" VisibleIndex="6">
                                                    <PropertiesComboBox DataSourceID="Designation" EnableIncrementalFiltering="True"
                                                        TextField="deg_designation" ValueField="deg_id" ValueType="System.Int32">
                                                    </PropertiesComboBox>
                                                    <EditFormSettings Visible="True" VisibleIndex="4" Caption="Designation" />
                                                    <EditCellStyle BackColor="#FFF2C8">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle BackColor="#FFF2C8" HorizontalAlign="Right" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataComboBoxColumn FieldName="emp_type" Visible="False" VisibleIndex="6">
                                                    <PropertiesComboBox DataSourceID="EmployeeType" EnableIncrementalFiltering="True"
                                                        TextField="emptpy_type" ValueField="emptpy_id" ValueType="System.Int32">
                                                    </PropertiesComboBox>
                                                    <EditFormSettings Visible="True" VisibleIndex="5" Caption="Employee Type" />
                                                    <EditCellStyle BackColor="#FFF2C8">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle BackColor="#FFF2C8" HorizontalAlign="Right" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataComboBoxColumn FieldName="emp_Department" Visible="False" VisibleIndex="6">
                                                    <PropertiesComboBox DataSourceID="Department" EnableIncrementalFiltering="True" TextField="cost_description"
                                                        ValueField="cost_id" ValueType="System.Int32">
                                                    </PropertiesComboBox>
                                                    <EditFormSettings Visible="True" VisibleIndex="6" Caption="Department" />
                                                    <EditCellStyle BackColor="#FFF2C8">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle BackColor="#FFF2C8" HorizontalAlign="Right" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataComboBoxColumn FieldName="emp_reportTo" Visible="False" VisibleIndex="6">
                                                    <PropertiesComboBox DataSourceID="ReportTo_Collegue_EmployeeDeputy" EnableIncrementalFiltering="True"
                                                        TextField="Name" ValueField="ID" ValueType="System.Int32">
                                                        <ValidationSettings Display="Dynamic">
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </PropertiesComboBox>
                                                    <EditFormSettings Visible="True" VisibleIndex="7" Caption="Report To" />
                                                    <EditCellStyle BackColor="#FFF2C8">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle BackColor="#FFF2C8" HorizontalAlign="Right" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataComboBoxColumn FieldName="emp_deputy" Visible="False" VisibleIndex="6">
                                                    <PropertiesComboBox DataSourceID="ReportTo_Collegue_EmployeeDeputy" EnableIncrementalFiltering="True"
                                                        TextField="Name" ValueField="ID" ValueType="System.Int32">
                                                        <ValidationSettings Display="Dynamic">
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </PropertiesComboBox>
                                                    <EditFormSettings Visible="True" VisibleIndex="8" Caption="Additional Reporting Head" />
                                                    <EditCellStyle BackColor="#FFF2C8">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle BackColor="#FFF2C8" HorizontalAlign="Right" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataComboBoxColumn FieldName="emp_colleague" Visible="False" VisibleIndex="6">
                                                    <PropertiesComboBox DataSourceID="ReportTo_Collegue_EmployeeDeputy" EnableIncrementalFiltering="True"
                                                        TextField="Name" ValueField="ID" ValueType="System.Int32">
                                                        <ValidationSettings Display="Dynamic">
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </PropertiesComboBox>
                                                    <EditFormSettings Visible="True" VisibleIndex="9" Caption="Colleague" />
                                                    <EditCellStyle BackColor="#FFF2C8">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle BackColor="#FFF2C8" HorizontalAlign="Right" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataComboBoxColumn FieldName="emp_workinghours" Visible="False" VisibleIndex="6">
                                                    <PropertiesComboBox DataSourceID="workingHr" EnableIncrementalFiltering="True" TextField="wor_scheduleName"
                                                        ValueField="wor_id" ValueType="System.String">
                                                    </PropertiesComboBox>
                                                    <EditFormSettings Visible="True" VisibleIndex="23" Caption="Working Hour" />
                                                    <EditCellStyle BackColor="#FFF2C8">
                                                        <BorderBottom BorderColor="Blue" BorderStyle="Ridge" />
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle BackColor="#FFF2C8" HorizontalAlign="Right" Wrap="False">
                                                        <BorderBottom BorderColor="Blue" />
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataTextColumn FieldName="emp_currentCTC" Visible="False" VisibleIndex="6">
                                                    <EditFormSettings Visible="True" VisibleIndex="11" Caption="Current CTC" />
                                                    <EditCellStyle BackColor="#DDECFE">
                                                        <BorderTop BorderColor="#8080FF" BorderStyle="Ridge" BorderWidth="1px" />
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle BackColor="#DDECFE" HorizontalAlign="Right" Wrap="False">
                                                        <BorderTop BorderColor="#8080FF" BorderStyle="Ridge" BorderWidth="1px" />
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="emp_basic" Visible="False" VisibleIndex="6">
                                                    <EditFormSettings Visible="True" VisibleIndex="12" Caption="Basic" />
                                                    <EditCellStyle BackColor="#DDECFE">
                                                        <BorderTop BorderColor="#8080FF" BorderStyle="Ridge" BorderWidth="1px" />
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle BackColor="#DDECFE" HorizontalAlign="Right" Wrap="False">
                                                        <BorderLeft BorderColor="#8080FF" BorderWidth="1px" />
                                                        <BorderTop BorderColor="#8080FF" BorderStyle="Ridge" BorderWidth="1px" />
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="emp_HRA" Visible="False" VisibleIndex="6">
                                                    <EditFormSettings Visible="True" VisibleIndex="13" Caption="HRA" />
                                                    <EditCellStyle BackColor="#DDECFE">
                                                        <BorderRight BorderColor="Blue" BorderWidth="1px" />
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle BackColor="#DDECFE" HorizontalAlign="Right" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="emp_CCA" Visible="False" VisibleIndex="6">
                                                    <EditFormSettings Visible="True" VisibleIndex="14" Caption="CCA" />
                                                    <EditCellStyle BackColor="#DDECFE">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle BackColor="#DDECFE" HorizontalAlign="Right" Wrap="False">
                                                        <BorderLeft BorderColor="#8080FF" BorderWidth="1px" />
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="emp_spAllowance" Visible="False" VisibleIndex="6">
                                                    <EditFormSettings Visible="True" VisibleIndex="15" Caption="SP. Allowance" />
                                                    <EditCellStyle BackColor="#DDECFE">
                                                        <BorderRight BorderColor="Blue" BorderWidth="1px" />
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle BackColor="#DDECFE" HorizontalAlign="Right" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="emp_childrenAllowance" Visible="False" VisibleIndex="6">
                                                    <EditFormSettings Visible="True" VisibleIndex="16" Caption="Children Allowance" />
                                                    <EditCellStyle BackColor="#DDECFE">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle BackColor="#DDECFE" HorizontalAlign="Right" Wrap="False">
                                                        <BorderLeft BorderColor="#8080FF" BorderWidth="1px" />
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataComboBoxColumn FieldName="emp_totalLeavePA" Visible="False" VisibleIndex="6">
                                                    <PropertiesComboBox DataSourceID="LeaveScheme" EnableIncrementalFiltering="True"
                                                        TextField="ls_name" ValueField="ls_id" ValueType="System.String">
                                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText" SetFocusOnError="True">
                                                            <RequiredField ErrorText="Select leave Scheme!" IsRequired="True" />
                                                        </ValidationSettings>
                                                    </PropertiesComboBox>
                                                    <EditFormSettings Visible="True" VisibleIndex="24" Caption="Leave Policy" />
                                                    <EditCellStyle BackColor="#FFF2C8">
                                                        <BorderBottom BorderColor="Blue" BorderStyle="Ridge" BorderWidth="1px" />
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle BackColor="#FFF2C8" HorizontalAlign="Right" Wrap="False">
                                                        <BorderBottom BorderColor="Blue" BorderStyle="Ridge" BorderWidth="1px" />
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataTextColumn FieldName="emp_PF" Visible="False" VisibleIndex="6">
                                                    <EditFormSettings Visible="True" VisibleIndex="18" Caption="PF" />
                                                    <EditCellStyle BackColor="#DDECFE">
                                                        <BorderRight BorderColor="Blue" BorderWidth="1px" />
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle BackColor="#DDECFE" HorizontalAlign="Right" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                    <CellStyle BackColor="#DDECFE">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="emp_medicalAllowance" Visible="False" VisibleIndex="6">
                                                    <EditFormSettings Visible="True" VisibleIndex="19" Caption="Medical Allowance" />
                                                    <EditCellStyle BackColor="#DDECFE">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle BackColor="#DDECFE" HorizontalAlign="Right" Wrap="False">
                                                        <BorderLeft BorderColor="#8080FF" BorderWidth="1px" />
                                                    </EditFormCaptionStyle>
                                                    <CellStyle BackColor="#DDECFE">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="emp_LTA" Visible="False" VisibleIndex="6">
                                                    <EditFormSettings Visible="True" VisibleIndex="20" Caption="LTA" />
                                                    <EditCellStyle BackColor="#DDECFE">
                                                        <BorderRight BorderColor="Blue" BorderWidth="1px" />
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle BackColor="#DDECFE" HorizontalAlign="Right" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                    <CellStyle BackColor="#DDECFE">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="emp_convence" Visible="False" VisibleIndex="6">
                                                    <EditFormSettings Visible="True" VisibleIndex="21" Caption="Convence" />
                                                    <EditCellStyle BackColor="#DDECFE">
                                                        <BorderBottom BorderColor="Blue" BorderStyle="Ridge" BorderWidth="1px" />
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle BackColor="#DDECFE" HorizontalAlign="Right" Wrap="False">
                                                        <BorderLeft BorderColor="#8080FF" BorderWidth="1px" />
                                                        <BorderBottom BorderColor="Blue" BorderStyle="Ridge" BorderWidth="1px" />
                                                    </EditFormCaptionStyle>
                                                    <CellStyle BackColor="#DDECFE">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="emp_mobilePhoneExp" Visible="False" VisibleIndex="6">
                                                    <EditFormSettings Visible="True" VisibleIndex="22" Caption="Mbile Phone Exp." />
                                                    <EditCellStyle BackColor="#DDECFE">
                                                        <BorderBottom BorderColor="Blue" BorderStyle="Ridge" BorderWidth="1px" />
                                                        <BorderRight BorderColor="Blue" BorderStyle="Ridge" BorderWidth="1px" />
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle BackColor="#DDECFE" HorizontalAlign="Right" Wrap="False">
                                                        <BorderBottom BorderColor="Blue" BorderStyle="Ridge" BorderWidth="1px" />
                                                    </EditFormCaptionStyle>
                                                    <CellStyle BackColor="#DDECFE">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="emp_totalMedicalLeavePA" Visible="False"
                                                    VisibleIndex="6">
                                                    <EditFormSettings Visible="False" VisibleIndex="23" Caption="Total Medical Leave PA" />
                                                    <EditCellStyle BackColor="#DDECFE">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle BackColor="#DDECFE" HorizontalAlign="Right" Wrap="False">
                                                    </EditFormCaptionStyle>
                                                    <CellStyle BackColor="#DDECFE">
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataDateColumn Caption="Leave Effect From" Visible="False" VisibleIndex="8"
                                                    FieldName="emp_LeaveSchemeAppliedFrom">
                                                    <PropertiesDateEdit DisplayFormatString="" EditFormat="Custom" EditFormatString="dd MMMM yyyy"
                                                        UseMaskBehavior="True">
                                                    </PropertiesDateEdit>
                                                    <EditFormSettings Caption="Leave Effective from" Visible="True" VisibleIndex="25" />
                                                    <EditCellStyle BackColor="#FFF2C8" HorizontalAlign="Left">
                                                        <BorderBottom BorderColor="Blue" BorderStyle="Ridge" BorderWidth="1px" />
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle BackColor="#FFF2C8" HorizontalAlign="Right" Wrap="False">
                                                        <BorderBottom BorderColor="Blue" BorderStyle="Ridge" BorderWidth="1px" />
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataDateColumn>
                                                <dxe:GridViewDataComboBoxColumn Caption="Branch" Visible="False" FieldName="emp_branch">
                                                    <PropertiesComboBox DataSourceID="Branch" EnableIncrementalFiltering="True" TextField="branch_description"
                                                        ValueField="branch_id" ValueType="System.Int32">
                                                    </PropertiesComboBox>
                                                    <EditFormSettings Caption="Branch" Visible="True" VisibleIndex="3" />
                                                    <EditCellStyle BackColor="#FFF2C8" HorizontalAlign="Left">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle BackColor="#FFF2C8" HorizontalAlign="Right">
                                                    </EditFormCaptionStyle>
                                                </dxe:GridViewDataComboBoxColumn>
                                                <dxe:GridViewDataMemoColumn Caption="Remarks" VisibleIndex="6" FieldName="emp_Remarks">
                                                    <PropertiesMemoEdit Height="50px">
                                                    </PropertiesMemoEdit>
                                                    <EditFormSettings Caption="Remarks" ColumnSpan="2" Visible="True" VisibleIndex="26" />
                                                </dxe:GridViewDataMemoColumn>
                                            </Columns>
                                            <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                                                <FirstPageButton Visible="True">
                                                </FirstPageButton>
                                                <LastPageButton Visible="True">
                                                </LastPageButton>
                                            </SettingsPager>
                                            <ClientSideEvents EndCallback="GridEndCall" />
                                        </dxe:ASPxGridView>
                                        <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ContentUrl="frmAddDocuments.aspx"
                                            CloseAction="CloseButton" Top="100" Left="400" ClientInstanceName="popup" Height="530px"
                                            Width="930px" HeaderText="Employee CTC">
                                            <ContentCollection>
                                                <dxe:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                                                </dxe:PopupControlContentControl>
                                            </ContentCollection>
                                        </dxe:ASPxPopupControl>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Bank Details" Text="Bank">
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

                            <dxe:TabPage Name="Subscription" Text="Subscriptions" Visible="false">
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
                    <asp:TextBox ID="txtID" runat="server" Visible="false"></asp:TextBox></td>
            </tr>
        </table>
        <asp:SqlDataSource ID="EployeeCTC" runat="server"
            UpdateCommand="employeeCTCupdate" UpdateCommandType="StoredProcedure" DeleteCommand="employeeCTCdelete"
            DeleteCommandType="StoredProcedure" InsertCommand="EmployeeCTCInsert" InsertCommandType="StoredProcedure"
            SelectCommand="EmplyeeCTCSelect" SelectCommandType="StoredProcedure">
            <InsertParameters>
                <asp:SessionParameter Name="emp_cntId" SessionField="KeyVal_InternalID" Type="String" />
                <asp:Parameter Name="emp_dateofJoining" Type="DateTime" />
                <asp:Parameter Name="emp_organization" Type="Int32" />
                <asp:Parameter Name="emp_JobResponsibility" Type="Int32" />
                <asp:Parameter Name="emp_Designation" Type="Int32" />
                <asp:Parameter Name="emp_type" Type="Int32" />
                <asp:Parameter Name="emp_Department" Type="Int32" />
                <asp:Parameter Name="emp_reportTo" Type="Int32" />
                <asp:Parameter Name="emp_deputy" Type="Int32" />
                <asp:Parameter Name="emp_colleague" Type="Int32" />
                <asp:Parameter Name="emp_workinghours" Type="Int32" />
                <asp:Parameter Name="emp_currentCTC" Type="String" />
                <asp:Parameter Name="emp_basic" Type="String" />
                <asp:Parameter Name="emp_HRA" Type="String" />
                <asp:Parameter Name="emp_CCA" Type="String" />
                <asp:Parameter Name="emp_spAllowance" Type="String" />
                <asp:Parameter Name="emp_childrenAllowance" Type="String" />
                <asp:Parameter Name="emp_totalLeavePA" Type="String" />
                <asp:Parameter Name="emp_PF" Type="String" />
                <asp:Parameter Name="emp_medicalAllowance" Type="String" />
                <asp:Parameter Name="emp_LTA" Type="String" />
                <asp:Parameter Name="emp_convence" Type="String" />
                <asp:Parameter Name="emp_mobilePhoneExp" Type="String" />
                <asp:Parameter Name="emp_totalMedicalLeavePA" Type="String" />
                <asp:SessionParameter Name="userid" SessionField="userid" Type="Int32" />
                <asp:Parameter Name="emp_LeaveSchemeAppliedFrom" Type="DateTime" />
                <asp:Parameter Name="emp_branch" Type="int32" />
                <asp:Parameter Name="emp_Remarks" Type="string" />
            </InsertParameters>
            <DeleteParameters>
                <asp:Parameter Name="Id" Type="Int32" />
                <asp:SessionParameter Name="userid" SessionField="userid" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="emp_dateofJoining" Type="DateTime" />
                <asp:Parameter Name="emp_organization" Type="String" />
                <asp:Parameter Name="emp_JobResponsibility" Type="Int32" />
                <asp:Parameter Name="emp_Designation" Type="Int32" />
                <asp:Parameter Name="emp_type" Type="Int32" />
                <asp:Parameter Name="emp_Department" Type="Int32" />
                <asp:Parameter Name="emp_reportTo" Type="Int32" />
                <asp:Parameter Name="emp_deputy" Type="Int32" />
                <asp:Parameter Name="emp_colleague" Type="Int32" />
                <asp:Parameter Name="emp_workinghours" Type="Int32" />
                <asp:Parameter Name="emp_currentCTC" Type="String" />
                <asp:Parameter Name="emp_basic" Type="String" />
                <asp:Parameter Name="emp_HRA" Type="String" />
                <asp:Parameter Name="emp_CCA" Type="String" />
                <asp:Parameter Name="emp_spAllowance" Type="String" />
                <asp:Parameter Name="emp_childrenAllowance" Type="String" />
                <asp:Parameter Name="emp_totalLeavePA" Type="String" />
                <asp:Parameter Name="emp_PF" Type="String" />
                <asp:Parameter Name="emp_medicalAllowance" Type="String" />
                <asp:Parameter Name="emp_LTA" Type="String" />
                <asp:Parameter Name="emp_convence" Type="String" />
                <asp:Parameter Name="emp_mobilePhoneExp" Type="String" />
                <asp:Parameter Name="emp_totalMedicalLeavePA" Type="String" />
                <asp:SessionParameter Name="userid" SessionField="userid" Type="Int32" />
                <asp:Parameter Name="Id" Type="Int32" />
                <asp:Parameter Name="emp_LeaveSchemeAppliedFrom" Type="datetime" />
                <asp:Parameter Name="emp_branch" Type="int32" />
                <asp:Parameter Name="emp_cntId" Type="string" />
                <asp:Parameter Name="emp_Remarks" Type="string" />
            </UpdateParameters>
            <SelectParameters>
                <asp:SessionParameter Name="cnt_internalId" SessionField="KeyVal_InternalID" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="Organization" runat="server" ConflictDetection="CompareAllValues"
             SelectCommand="Select cmp_id, cmp_name from tbl_master_Company order by cmp_name"></asp:SqlDataSource>
        <asp:SqlDataSource ID="Responsibility" runat="server" ConflictDetection="CompareAllValues"
            SelectCommand="Select job_id, job_responsibility from tbl_master_jobresponsibility order by job_responsibility"></asp:SqlDataSource>
        <asp:SqlDataSource ID="Designation" runat="server" ConflictDetection="CompareAllValues"
            SelectCommand="Select deg_id, deg_designation from tbl_master_Designation order by deg_designation"></asp:SqlDataSource>
        <asp:SqlDataSource ID="EmployeeType" runat="server" ConflictDetection="CompareAllValues"
             SelectCommand="Select emptpy_id, emptpy_type from tbl_master_employeeType order by emptpy_type"></asp:SqlDataSource>
        <asp:SqlDataSource ID="Department" runat="server" ConflictDetection="CompareAllValues"
             SelectCommand="Select cost_id, cost_description from tbl_master_costCenter where cost_costCenterType = 'department' order by cost_description"></asp:SqlDataSource>
        <asp:SqlDataSource ID="LeaveScheme" runat="server" ConflictDetection="CompareAllValues"
             SelectCommand="Select ls_id, ls_name from tbl_master_LeaveScheme"></asp:SqlDataSource>
        <asp:SqlDataSource ID="ReportTo_Collegue_EmployeeDeputy" runat="server" ConflictDetection="CompareAllValues"
           >
            <SelectParameters>
                <asp:SessionParameter Name="ID" Type="string" SessionField="KeyVal_InternalID" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="workingHr" runat="server" ConflictDetection="CompareAllValues"
           SelectCommand="Select wor_scheduleName,wor_id from tbl_master_workingHours"></asp:SqlDataSource>
        <asp:SqlDataSource ID="Branch" runat="server" ConflictDetection="CompareAllValues"
            SelectCommand="Select [branch_id], [branch_description] from tbl_master_branch"></asp:SqlDataSource>
    </div>

    <asp:HiddenField ID="hdKeyVal" runat="server" />
    <%--<SettingsDetail AllowOnlyOneMasterRowExpanded="True" />--%>
    <%--<SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True"
                PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="600px" />--%>
    <%--  <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="250px" PopupEditFormHorizontalAlign="Center"
                PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="500px"
                EditFormColumnCount="1" />
          <%--  <SettingsText ConfirmDelete="Are you sure want to Delete this Record?" />
            <Templates>
                <EditForm>
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors"
                                   ColumnID="" ID="Editors">
                                </dxe:ASPxGridViewTemplateReplacement>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                    runat="server" ColumnID="">
                                </dxe:ASPxGridViewTemplateReplacement>
                                <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                    runat="server" ColumnID="">
                                </dxe:ASPxGridViewTemplateReplacement>
                            </td>
                        </tr>
                    </table>
                </EditForm>
            </Templates>--%>
</asp:Content>

