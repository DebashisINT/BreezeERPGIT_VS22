<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="Contact_Person.aspx.cs" Inherits="ERP.OMS.Management.Master.Contact_Person" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .dxeErrorFrameSys.dxeErrorCellSys, .dxeErrorFrameSys.dxeErrorCellSys {
            position: absolute !important;
        }

        .dxgvEditingErrorRow_PlasticBlue {
            background-color: #0820b7 !important;
            color: #f4f4f4 !important;
        }
    </style>
    <script>
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
        function DateValidateBirth() {
            var DOB = new Date(GridContactPerson.GetEditor("DateofBirth").GetDate());
            var monthnumber = DOB.getMonth();
            var monthday = DOB.getDate();
            var year = DOB.getYear();
            var SelectedDateValue = new Date(year, monthnumber, monthday);

            var Anniversary = new Date(GridContactPerson.GetEditor("AnniversaryDate").GetDate());
            var monthnumber = Anniversary.getMonth();
            var monthday = Anniversary.getDate();
            var year = Anniversary.getYear();
            var SelectedAnniversary = new Date(year, monthnumber, monthday);

            if (DOB != "") {
                if (Anniversary != "") {
                    if (SelectedDateValue.getTime() >= SelectedAnniversary.getTime()) {
                        GridContactPerson.GetEditor("AnniversaryDate").SetText("");
                        GridContactPerson.GetEditor("AnniversaryDate").Focus();
                    }
                }
            }
        }
        function disp_prompt(name) {
            if (name == "tab0") {

                document.location.href = "Contact_general.aspx";
            }
            if (name == "tab1") {

                document.location.href = "Contact_Correspondence.aspx";
            }
            else if (name == "tab2") {

                document.location.href = "Contact_BankDetails.aspx";
            }
            else if (name == "tab3") {

                document.location.href = "Contact_DPDetails.aspx";
            }
            else if (name == "tab4") {

                document.location.href = "Contact_Document.aspx";
            }
            else if (name == "tab12") {

                document.location.href = "Contact_FamilyMembers.aspx";
            }
            else if (name == "tab5") {

                document.location.href = "Contact_Registration.aspx";
            }
            else if (name == "tab7") {

                document.location.href = "Contact_GroupMember.aspx";
            }
            else if (name == "tab8") {

                document.location.href = "Contact_Deposit.aspx";
            }
            else if (name == "tab9") {

                document.location.href = "Contact_Remarks.aspx";
            }
            else if (name == "tab10") {

                document.location.href = "Contact_Education.aspx";
            }
            else if (name == "tab11") {

                document.location.href = "contact_brokerage.aspx";
            }
            else if (name == "tab6") {

                document.location.href = "contact_other.aspx";
            }
            else if (name == "tab13") {
                document.location.href = "contact_Subscription.aspx";
            }

            else if (name == "tab14") {
                document.location.href = "Contact_tds.aspx";
            }
            else if (name == "tab15") {
                //document.location.href = "Contact_Person.aspx";
            }

        }
        function AddAddress(KeyVal) {
            var url = 'AddAddressForContactPerson.aspx?id=' + KeyVal;
            popup.SetContentUrl(url);
            popup.Show();          
        }
    </script>
    <style type="text/css">
        .dxeValidStEditorTable td.dxeErrorFrameSys.dxeErrorCellSys {
            position: absolute !important;
        }

        .dxeValidStEditorTable[errorframe="errorFrame"] {
            width: 100% !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="panel-heading">
        <div class="panel-title">           
            <h3>
                <asp:Label ID="lblHeadTitle" runat="server"></asp:Label>
            </h3>          
            <div class="crossBtn"><a id="lnkClose"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
    <div class="form_main">
        <div>
            <table width="100%">
                <tr>
                    <td class="EHEADER" style="text-align: center">
                        <asp:Label ID="lblName" runat="server" Font-Size="12px" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
            </table>
            <table class="TableMain100">
                <tr>
                    <td>
                        <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="15" ClientInstanceName="page"
                            OnActiveTabChanged="ASPxPageControl1_ActiveTabChanged" >
                            <TabPages>
                                <dxe:TabPage Name="General" Text="General">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Correspondence" Text="Correspondence">
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
                                <dxe:TabPage Name="Documents" Text="Documents">

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
                                <dxe:TabPage Name="Other" Visible="false" Text="Other">

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
                                            <div class="pull-left">
                                                  <% if (rights.CanAdd)
                                               { %>
                                                <dxe:ASPxHyperLink ID="ASPxHyperLink1" runat="server" Text="Add New" ClientSideEvents-Click="function(s, e) {GridContactPerson.AddNewRow();}"
                                                    CssClass="btn btn-primary">
                                                    <ClientSideEvents Click="function(s, e) {GridContactPerson.AddNewRow();}"></ClientSideEvents>
                                                </dxe:ASPxHyperLink>

                                                 <% } %>

                                            <% if (rights.CanExport)
                                               { %>
                                                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" AutoPostBack="true" OnSelectedIndexChanged="drdExport_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                                    <asp:ListItem Value="2">XLS</asp:ListItem>
                                                    <asp:ListItem Value="3">RTF</asp:ListItem>
                                                    <asp:ListItem Value="4">CSV</asp:ListItem>

                                                </asp:DropDownList>
                                                    <% } %>
                                            </div>

                                            <table class="TableMain100">
                                                <tr>
                                                    <td style="width: 933px">
                                                        <dxe:ASPxGridView ID="GridContactPerson" ClientInstanceName="GridContactPerson"
                                                            KeyFieldName="ContactId" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1"
                                                            Width="100%" OnHtmlDataCellPrepared="GridContactPerson_HtmlDataCellPrepared" OnCellEditorInitialize="GridContactPerson_CellEditorInitialize" CssClass="wordWrap">
                                                            <Styles>
                                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                </Header>
                                                                <LoadingPanel ImageSpacing="10px">
                                                                </LoadingPanel>
                                                            </Styles>
                                                            <Columns>
                                                                <dxe:GridViewDataTextColumn FieldName="name" Caption="Name" VisibleIndex="0" Width="20%">
                                                                    <EditFormSettings Visible="True" VisibleIndex="0" />
                                                                    <PropertiesTextEdit MaxLength="2000" Width="100%">
                                                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True" ErrorTextPosition="Right">
                                                                            <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                                                        </ValidationSettings>
                                                                    </PropertiesTextEdit>
                                                                    <EditCellStyle HorizontalAlign="Right">
                                                                    </EditCellStyle>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Phone" VisibleIndex="6" CellStyle-Wrap="True" Width="15%">
                                                                    <EditFormSettings Visible="False" />
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Officephone" Visible="false" VisibleIndex="1">
                                                                    <EditCellStyle HorizontalAlign="Right">
                                                                    </EditCellStyle>
                                                                    <EditFormSettings Visible="True" Caption="Office Phone" VisibleIndex="1" />
                                                                    <PropertiesTextEdit MaxLength="500" Width="100%">
                                                                    </PropertiesTextEdit>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Residencephone" Visible="false" VisibleIndex="2">
                                                                    <EditCellStyle HorizontalAlign="Right">
                                                                    </EditCellStyle>
                                                                    <EditFormSettings Visible="True" Caption="Residence Phone" VisibleIndex="2" />
                                                                    <PropertiesTextEdit MaxLength="500" Width="100%">
                                                                    </PropertiesTextEdit>
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Mobilephone" Visible="false" VisibleIndex="3">
                                                                    <PropertiesTextEdit MaxLength="500" Width="100%">
                                                                    </PropertiesTextEdit>
                                                                    <EditCellStyle HorizontalAlign="Right">
                                                                    </EditCellStyle>
                                                                    <EditFormSettings Visible="True" Caption="Mobile Phone" VisibleIndex="3" />
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="email" Caption="Email" VisibleIndex="7">
                                                                    <PropertiesTextEdit MaxLength="2000" Width="100%">
                                                                        <ValidationSettings Display="Dynamic" ErrorTextPosition="right" SetFocusOnError="True">
                                                                            <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                                                        </ValidationSettings>
                                                                    </PropertiesTextEdit>
                                                                    <EditCellStyle HorizontalAlign="Right">
                                                                    </EditCellStyle>
                                                                    <EditFormSettings Visible="True" VisibleIndex="4" />
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataDateColumn FieldName="DateofBirth" Caption="Date of Birth" Visible="false">
                                                                    <EditFormSettings Visible="True" VisibleIndex="5" />
                                                                    <CellStyle CssClass="">
                                                                    </CellStyle>
                                                                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                    </EditFormCaptionStyle>
                                                                    <PropertiesDateEdit Width="100%" EditFormatString="dd-MM-yyyy">
                                                                        <ClientSideEvents DateChanged="DateValidateBirth" />
                                                                    </PropertiesDateEdit>
                                                                </dxe:GridViewDataDateColumn>

                                                                <dxe:GridViewDataDateColumn FieldName="AnniversaryDate" Caption="Anniversary date" Visible="false">
                                                                    <EditFormSettings Visible="True" VisibleIndex="6" />
                                                                    <CellStyle CssClass="gridcellleft">
                                                                    </CellStyle>
                                                                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                    </EditFormCaptionStyle>
                                                                    <PropertiesDateEdit Width="100%" EditFormatString="dd-MM-yyyy">
                                                                        <ClientSideEvents DateChanged="DateValidateBirth" />
                                                                    </PropertiesDateEdit>
                                                                </dxe:GridViewDataDateColumn>

                                                                <dxe:GridViewDataComboBoxColumn FieldName="cp_designation" Caption="Designation"
                                                                    VisibleIndex="5" Width="10%">
                                                                    <PropertiesComboBox DataSourceID="SqlDesignation" TextField="deg_designation" ValueField="deg_id"
                                                                        ValueType="System.String" EnableSynchronization="False" EnableIncrementalFiltering="True" Width="100%">
                                                                    </PropertiesComboBox>
                                                                    <EditFormSettings Visible="True" VisibleIndex="7" />
                                                                    <EditCellStyle HorizontalAlign="Right">
                                                                    </EditCellStyle>
                                                                </dxe:GridViewDataComboBoxColumn>
                                                                <dxe:GridViewDataComboBoxColumn FieldName="cp_relationShip" Caption="Relationship"
                                                                    VisibleIndex="4" Width="10%">
                                                                    <PropertiesComboBox DataSourceID="SqlFamRelationShip" TextField="fam_familyRelationship"
                                                                        ValueField="fam_id" ValueType="System.String" EnableSynchronization="False" EnableIncrementalFiltering="True" Width="100%">


                                                                        <ClientSideEvents Init="function(s, e) {
	                                                                        var value = s.GetText().toUpperCase();	
                                                                            if(value == &quot;EMPLOYEE&quot;)
                                                                            {
                                                                                 GridContactPerson.GetEditor(&quot;cp_designation&quot;).SetEnabled(true);
                                                                            }
                                                                            else
                                                                            {
                                                                                 GridContactPerson.GetEditor(&quot;cp_designation&quot;).SetEnabled(false);
                                                                            }
                                                                        }" />
                                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) {
	                                                                        var value = s.GetText().toUpperCase();
                                                                            if(value == &quot;EMPLOYEE&quot;)
                                                                            {
                                                                                 GridContactPerson.GetEditor(&quot;cp_designation&quot;).SetEnabled(true);
                                                                            }
                                                                            else
                                                                            {
                                                                                 GridContactPerson.GetEditor(&quot;cp_designation&quot;).SetEnabled(false);
                                                                            }
                                                                        }" />


                                                                    </PropertiesComboBox>
                                                                    <EditFormSettings Visible="True" VisibleIndex="8" />
                                                                    <EditCellStyle HorizontalAlign="Right">
                                                                    </EditCellStyle>
                                                                </dxe:GridViewDataComboBoxColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="status" Caption="Status" VisibleIndex="9"
                                                                    Width="10%">
                                                                    <EditFormSettings Visible="False" />
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataComboBoxColumn FieldName="cp_status" Visible="False" VisibleIndex="8"
                                                                    Width="10%">
                                                                    <PropertiesComboBox ValueType="System.Char" EnableIncrementalFiltering="True" Width="100%">
                                                                        <Items>
                                                                            <dxe:ListEditItem Text="Active" Value="Y" Selected="true"></dxe:ListEditItem>
                                                                            <dxe:ListEditItem Text="Suspended" Value="N"></dxe:ListEditItem>
                                                                        </Items>


                                                                    </PropertiesComboBox>
                                                                    <EditFormSettings Visible="True" Caption="Status" VisibleIndex="9" />
                                                                    <EditCellStyle HorizontalAlign="Right">
                                                                    </EditCellStyle>
                                                                </dxe:GridViewDataComboBoxColumn>
                                                                <dxe:GridViewCommandColumn VisibleIndex="10" Width="10%" ShowClearFilterButton="true" ShowDeleteButton="true" ShowEditButton="true">
                                                                    <HeaderCaptionTemplate>
                                                                        Actions
                                                                    </HeaderCaptionTemplate>
                                                                    <HeaderStyle Font-Underline="false" HorizontalAlign="Center" />
                                                                    <CustomButtons>
                                                                        <dxe:GridViewCommandColumnCustomButton Image-Url="../../../assests/images/add-adress.png" Image-ToolTip="Contact Person's Address">
                                                                        </dxe:GridViewCommandColumnCustomButton>

                                                                    </CustomButtons>
                                                                </dxe:GridViewCommandColumn>
                                                            </Columns>
                                                            <ClientSideEvents CustomButtonClick="function(s, e) {
                                                          var key = s.GetRowKey(e.visibleIndex);
                                                          AddAddress(key);
                            
                                                           }" />
                                                            <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="400px" PopupEditFormHorizontalAlign="WindowCenter"
                                                                PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="430px"
                                                                EditFormColumnCount="1" />
                                                            <SettingsText PopupEditFormCaption="Add/Modify contact person" ConfirmDelete="Are You Sure To Delete This Record ???" />
                                                            <SettingsBehavior ConfirmDelete="True" />
                                                            <SettingsCommandButton>
                                                                <ClearFilterButton Text="ClearFilter">
                                                                </ClearFilterButton>

                                                                <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                                                                    <Image AlternateText="Edit" Url="../../../assests/images/Edit.png"></Image>
                                                                </EditButton>
                                                                <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete" Styles-Style-CssClass="pad">
                                                                    <Image AlternateText="Delete" Url="../../../assests/images/Delete.png"></Image>
                                                                </DeleteButton>
                                                                <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary">
                                                                    <Styles>
                                                                        <Style CssClass="btn btn-primary pull-left"></Style>
                                                                    </Styles>
                                                                </UpdateButton>
                                                                <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger">
                                                                    <Styles>
                                                                        <Style CssClass="btn btn-danger pull-left"></Style>
                                                                    </Styles>
                                                                </CancelButton>
                                                            </SettingsCommandButton>
                                                            <SettingsBehavior ConfirmDelete="True" />
                                                            <SettingsPager ShowSeparators="True">
                                                            </SettingsPager>
                                                        </dxe:ASPxGridView>
                                                        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                                            SelectCommand="select A.cp_contactId as ContactId, A.cp_name as name,(select phf_phonenumber from tbl_master_phonefax 
                                                        where phf_cntId=A.cp_contactId and phf_type='Office') as Officephone
                                                        ,(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Residence') as Residencephone
                                                        ,(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Mobile') as Mobilephone
                                                        ,isnull(('(O)'+(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Office')),'')+isnull(('(R)'+(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Residence')),'')+isnull(('(M)'+(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Mobile')),'') as Phone,(select eml_email from tbl_master_email where eml_cntId=A.cp_contactId) as email, (case when cp_status = 'Y' then 'Active' when cp_status = 'N' then 'Suspended' else 'N/A'  end) as status, ltrim(rtrim(cp_status)) as cp_status
                                                        ,ltrim(rtrim(cp_designation)) as cp_designation,ltrim(rtrim(cp_relationShip)) as cp_relationShip 
                                                        ,CAST(DateofBirth AS DATE) AS DateofBirth 
                                                        ,CAST(AnniversaryDate AS DATE) AS AnniversaryDate     
                                                        from tbl_master_CustomerContactPerson A  
                                                        where cp_agentInternalId=@insuId ORDER BY cp_status desc"
                                                            InsertCommand="ContactPersonInsertforCustomer" InsertCommandType="StoredProcedure"
                                                            DeleteCommand="CustomerContactPersonDelete" DeleteCommandType="StoredProcedure" UpdateCommand="ContactPersonUpdateforCustomer"
                                                            UpdateCommandType="StoredProcedure">
                                                            <SelectParameters>
                                                                <asp:SessionParameter Name="insuId" SessionField="KeyVal_InternalID_New" Type="String" />
                                                            </SelectParameters>
                                                            <InsertParameters>
                                                                <asp:Parameter Name="name" Type="String" />
                                                                <asp:Parameter Name="Officephone" Type="String" />
                                                                <asp:Parameter Name="Residencephone" Type="String" />
                                                                <asp:Parameter Name="Mobilephone" Type="String" />
                                                                <asp:Parameter Name="email" Type="String" />
                                                                <asp:Parameter Name="cp_designation" Type="String" />
                                                                <asp:Parameter Name="cp_relationShip" Type="String" />
                                                                <asp:Parameter Name="cp_status" Type="String" />
                                                                <asp:SessionParameter Name="userid" SessionField="userid" Type="Int32" />
                                                                <asp:SessionParameter Name="agentid" SessionField="KeyVal_InternalID_New" Type="String" />
                                                                <asp:Parameter Name="DateofBirth" Type="datetime" />
                                                                <asp:Parameter Name="AnniversaryDate" Type="datetime" />
                                                            </InsertParameters>
                                                            <DeleteParameters>
                                                                <asp:Parameter Name="ContactId" Type="String" />
                                                            </DeleteParameters>
                                                            <UpdateParameters>
                                                                <asp:Parameter Name="name" Type="String" />
                                                                <asp:Parameter Name="Officephone" Type="String" />
                                                                <asp:Parameter Name="Residencephone" Type="String" />
                                                                <asp:Parameter Name="Mobilephone" Type="String" />
                                                                <asp:Parameter Name="email" Type="String" />
                                                                <asp:Parameter Name="cp_designation" Type="String" />
                                                                <asp:Parameter Name="cp_relationShip" Type="String" />
                                                                <asp:Parameter Name="cp_status" Type="String" />
                                                                <asp:Parameter Name="ContactId" Type="String" />
                                                                <asp:SessionParameter Name="userid" SessionField="userid" Type="Int32" />
                                                                <asp:Parameter Name="DateofBirth" Type="datetime" />
                                                                <asp:Parameter Name="AnniversaryDate" Type="datetime" />
                                                            </UpdateParameters>
                                                        </asp:SqlDataSource>
                                                        <asp:SqlDataSource ID="SqlDesignation" runat="server"
                                                            SelectCommand="select deg_id,deg_designation from tbl_master_designation"></asp:SqlDataSource>
                                                        <asp:SqlDataSource ID="SqlFamRelationShip" runat="server"
                                                            SelectCommand="select fam_id,fam_familyRelationship from tbl_master_familyrelationship"></asp:SqlDataSource>

                                                    </td>
                                                </tr>
                                            </table>
                                            <dxe:ASPxPopupControl ID="ASPXPopupControl1" runat="server"
                                                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="540px"
                                                Width="450px" HeaderText="Add/Modify Address" Modal="true" AllowResize="true" ResizingMode="Postponed">
                                                <ContentCollection>
                                                    <dxe:PopupControlContentControl runat="server">
                                                    </dxe:PopupControlContentControl>
                                                </ContentCollection>
                                            </dxe:ASPxPopupControl>
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
	                                                var Tab14=page.GetTab(14);
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
                        <asp:TextBox ID="txtID" runat="server" Visible="false"></asp:TextBox></td>
                    <td></td>
                </tr>
            </table>
            <dxe:ASPxGridViewExporter ID="exporter" runat="server">
            </dxe:ASPxGridViewExporter>
        </div>
    </div>
</asp:Content>
