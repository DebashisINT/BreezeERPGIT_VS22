<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_ConsumerComp" CodeBehind="ConsumerComp.aspx.cs" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    
    <script language="javascript" type="text/javascript">
        function OnContactInfoClick(keyValue, name) {
            var url = 'insurance_contactPerson.aspx?id=' + keyValue;
            OnMoreInfoClick(url, "Company Name :" + name + "", '940px', '450px', "Y");
        }
        function NewPgae(cnt_id) {
            alert('cnt_id');
        }
        function ClickOnMoreInfo(keyValue) {
            var url = 'ConsumerComp_general.aspx?id=' + keyValue;
            OnMoreInfoClick(url, "Modify Contact Details", '940px', '450px', "Y");
        }
        function OnAddButtonClick() {
            var url = 'ConsumerComp_general.aspx?id=' + 'ADD';
            OnMoreInfoClick(url, "Add Contact Details", '940px', '450px', "Y");

        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }     
        //-->
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">Consumer Finance Companies List</span></strong></td>
            </tr>
            <tr>
                <td>

                    <table width="100%">
                        <tr>
                            <td style="text-align: left; vertical-align: top">
                                <table>
                                    <tr>
                                        <td id="ShowFilter">
                                            <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">Show Filter</span></a>
                                        </td>
                                        <td id="Td1">
                                            <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">All Records</span></a>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td></td>
                            <td class="gridcellright">
                                <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" BackColor="Navy" Font-Bold="False" ForeColor="White" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" ValueType="System.Int32" Width="130px">
                                    <Items>
                                        <dxe:ListEditItem Text="Select" Value="0" />
                                        <dxe:ListEditItem Text="PDF" Value="1" />
                                        <dxe:ListEditItem Text="XLS" Value="2" />
                                        <dxe:ListEditItem Text="RTF" Value="3" />
                                        <dxe:ListEditItem Text="CSV" Value="4" />
                                    </Items>
                                    <ButtonStyle BackColor="#C0C0FF" ForeColor="Black">
                                    </ButtonStyle>
                                    <ItemStyle BackColor="Navy" ForeColor="White">
                                        <HoverStyle BackColor="#8080FF" ForeColor="White">
                                        </HoverStyle>
                                    </ItemStyle>
                                    <Border BorderColor="White" />
                                    <DropDownButton Text="Export">
                                    </DropDownButton>
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxGridView ID="EmployeeGrid" runat="server" KeyFieldName="cnt_Id"
                        AutoGenerateColumns="False" DataSourceID="EmployeeDataSource" Width="100%" ClientInstanceName="grid" OnCustomCallback="EmployeeGrid_CustomCallback">
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="Name" ReadOnly="True" VisibleIndex="0" Width="40%">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="BranchName" VisibleIndex="1">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Phone" FieldName="phone" VisibleIndex="2">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Details" VisibleIndex="3" Width="5%">
                                <DataItemTemplate>
                                    <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Container.KeyValue %>')">More Info...</a>
                                </DataItemTemplate>
                                <CellStyle HorizontalAlign="Left" Wrap="False">
                                </CellStyle>
                                <EditFormSettings Visible="False" />
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    <a href="javascript:void(0);" onclick="OnAddButtonClick()">
                                        <span style="color: #000099; text-decoration: underline">Add New</span>
                                    </a>
                                </HeaderTemplate>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Cont.Person" VisibleIndex="4" Width="5%">
                                <DataItemTemplate>
                                    <a href="javascript:void(0);" onclick="OnContactInfoClick('<%#Eval("Id") %>','<%#Eval("Name") %>')">Show</a>
                                </DataItemTemplate>
                                <CellStyle HorizontalAlign="Left" Wrap="False">
                                </CellStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Created User" FieldName="user_name" Visible="False"
                                VisibleIndex="6">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <Styles>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                        </Styles>
                        <Settings ShowStatusBar="Visible" />
                        <SettingsText PopupEditFormCaption="Add/ Modify Employee" />
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True"
                            PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="900px" EditFormColumnCount="3" />
                        <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
                        <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="EmployeeDataSource" runat="server" 
            SelectCommand="">
            <%--<SelectParameters>
            <asp:SessionParameter Name="BranchId" SessionField="userbranchID" Type="Int32" />
            <asp:SessionParameter Name="contactType" SessionField="userContactType" Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="cnt_id" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="cnt_id" Type="Int32" />
            <asp:Parameter Name="cnt_ucc" Type="String" />
            <asp:Parameter Name="cnt_internalId" Type="String" />
            <asp:Parameter Name="cnt_salutation" Type="Int32" />
            <asp:Parameter Name="cnt_firstName" Type="String" />
            <asp:Parameter Name="cnt_middleName" Type="String" />
            <asp:Parameter Name="cnt_lastName" Type="String" />
            <asp:Parameter Name="cnt_shortName" Type="String" />
            <asp:Parameter Name="cnt_branchId" Type="Int32" />
            <asp:Parameter Name="cnt_sex" Type="Int32" />
            <asp:Parameter Name="cnt_maritalStatus" Type="Int32" />
            <asp:Parameter Name="cnt_DOB" Type="DateTime" />
            <asp:Parameter Name="cnt_anniversaryDate" Type="DateTime" />
            <asp:Parameter Name="cnt_legalStatus" Type="Int32" />
            <asp:Parameter Name="cnt_education" Type="Int32" />
            <asp:Parameter Name="cnt_profession" Type="Int32" />
            <asp:Parameter Name="cnt_organization" Type="String" />
            <asp:Parameter Name="cnt_jobResponsibility" Type="Int32" />
            <asp:Parameter Name="cnt_designation" Type="Int32" />
            <asp:Parameter Name="cnt_industry" Type="Int32" />
            <asp:Parameter Name="cnt_contactSource" Type="Int32" />
            <asp:Parameter Name="cnt_referedBy" Type="String" />
            <asp:Parameter Name="cnt_relation" Type="Int32" />
            <asp:Parameter Name="cnt_contactType" Type="Int32" />
            <asp:Parameter Name="cnt_contactStatus" Type="Int32" />
            <asp:Parameter Name="cnt_LeadId" Type="String" />
            <asp:SessionParameter Name="lastModifyUser" SessionField="userid" Type="String" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="cnt_id" Type="Int32" />
            <asp:Parameter Name="cnt_ucc" Type="String" />
            <asp:Parameter Name="cnt_salutation" Type="Int32" />
            <asp:Parameter Name="cnt_firstName" Type="String" />
            <asp:Parameter Name="cnt_middleName" Type="String" />
            <asp:Parameter Name="cnt_lastName" Type="String" />
            <asp:Parameter Name="cnt_shortName" Type="String" />
            <asp:Parameter Name="cnt_branchId" Type="Int32" />
            <asp:Parameter Name="cnt_sex" Type="Int32" />
            <asp:Parameter Name="cnt_maritalStatus" Type="Int32" />
            <asp:Parameter Name="cnt_DOB" Type="DateTime" />
            <asp:Parameter Name="cnt_anniversaryDate" Type="DateTime" />
            <asp:Parameter Name="cnt_legalStatus" Type="Int32" />
            <asp:Parameter Name="cnt_education" Type="Int32" />
            <asp:Parameter Name="cnt_profession" Type="Int32" />
            <asp:Parameter Name="cnt_organization" Type="String" />
            <asp:Parameter Name="cnt_jobResponsibility" Type="Int32" />
            <asp:Parameter Name="cnt_designation" Type="Int32" />
            <asp:Parameter Name="cnt_industry" Type="Int32" />
            <asp:Parameter Name="cnt_contactSource" Type="Int32" />
            <asp:Parameter Name="cnt_referedBy" Type="String" />
            <asp:Parameter Name="cnt_relation" Type="Int32" />
            <asp:Parameter Name="cnt_contactType" Type="Int32" />
            <asp:Parameter Name="cnt_contactStatus" Type="Int32" />
            <asp:Parameter Name="cnt_LeadId" Type="String" />
            <asp:Parameter Name="lastModifyUser" Type="String" />
        </InsertParameters>--%>
        </asp:SqlDataSource>

        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>
    </div>
</asp:Content>

