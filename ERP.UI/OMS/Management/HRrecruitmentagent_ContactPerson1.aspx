<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" 
    Inherits="ERP.OMS.Management.management_HRrecruitmentagent_ContactPerson1" Codebehind="HRrecruitmentagent_ContactPerson1.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>
<%@ Register assembly="DevExpress.Web.v15.1" namespace="DevExpress.Web" tagprefix="dx" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>
            <table class="TableMain100">
                <tr>
                    <td style="text-align: center">
                        <asp:Label ID="lblHeader" runat="server" Font-Bold="True" Font-Size="15px" ForeColor="Navy"
                            Width="819px" Height="18px"></asp:Label></td>
                </tr>
                <tr>
                    <td class="gridcellcenter">
                        <dxe:ASPxGridView ID="GridContactPerson" ClientInstanceName="GridContactPerson"
                            KeyFieldName="ContactId" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1"
                            Width="100%" OnHtmlDataCellPrepared="GridContactPerson_HtmlDataCellPrepared">
                            <Styles>
                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                </Header>
                                <LoadingPanel ImageSpacing="10px">
                                </LoadingPanel>
                            </Styles>
                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="name" VisibleIndex="0">
                                    <EditCellStyle HorizontalAlign="Right">
                                    </EditCellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="phone" VisibleIndex="1">
                                    <EditCellStyle HorizontalAlign="Right">
                                    </EditCellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="email" VisibleIndex="2">
                                    <PropertiesTextEdit>
                                        <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom">
                                            <RegularExpression ErrorText="Invali E-mail ID!" ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$" />
                                        </ValidationSettings>
                                    </PropertiesTextEdit>
                                    <EditCellStyle HorizontalAlign="Right">
                                    </EditCellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="status" VisibleIndex="3" Width="10%">
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataComboBoxColumn FieldName="cp_status" Visible="False" VisibleIndex="4"
                                    Width="10%">
                                    <PropertiesComboBox ValueType="System.String" EnableIncrementalFiltering="True">
                                        <Items>
                                            <dxe:ListEditItem Text="Active" Value="Y">
                                            </dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Suspended" Value="N">
                                            </dxe:ListEditItem>
                                        </Items>
                                    </PropertiesComboBox>
                                    <EditFormSettings Visible="True" Caption="Status" />
                                    <EditCellStyle HorizontalAlign="Right">
                                    </EditCellStyle>
                                </dxe:GridViewDataComboBoxColumn>
                                <dxe:GridViewCommandColumn VisibleIndex="4" Width="10%" ShowClearFilterButton="True" ShowDeleteButton="True" ShowEditButton="True">
                                    <HeaderCaptionTemplate>
                                        <dxe:ASPxHyperLink ID="ASPxHyperLink1" runat="server" Text="Add New" ClientSideEvents-Click="function(s, e) {GridContactPerson.AddNewRow();}" Font-Size="12px" Font-Underline="true">
                                        </dxe:ASPxHyperLink>
                                    </HeaderCaptionTemplate>
                                    <HeaderStyle Font-Underline="True" HorizontalAlign="Center"/>
                                </dxe:GridViewCommandColumn>
                            </Columns>
                            <SettingsBehavior ConfirmDelete="True" />
                            <SettingsPager ShowSeparators="True">
                            </SettingsPager>
                        </dxe:ASPxGridView>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                            SelectCommand="" InsertCommand="ContactPersonInsert" InsertCommandType="StoredProcedure"
                            DeleteCommand="ContactPersonDelete" DeleteCommandType="StoredProcedure" UpdateCommand="ContactPersonUpdate"
                            UpdateCommandType="StoredProcedure">
                            <InsertParameters>
                                <asp:Parameter Name="name" Type="String" />
                                <asp:Parameter Name="phone" Type="String" />
                                <asp:Parameter Name="email" Type="String" />
                                <asp:Parameter Name="cp_status" Type="String" />
                                <asp:SessionParameter Name="userid" SessionField="userid" Type="Int32" />
                                <asp:SessionParameter Name="agentid" SessionField="KeyVal_InternalID" Type="String" />
                            </InsertParameters>
                            <DeleteParameters>
                                <asp:Parameter Name="ContactId" Type="String" />
                            </DeleteParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="name" Type="String" />
                                <asp:Parameter Name="phone" Type="String" />
                                <asp:Parameter Name="email" Type="String" />
                                <asp:Parameter Name="cp_status" Type="String" />
                                <asp:Parameter Name="ContactId" Type="String" />
                                <asp:SessionParameter Name="userid" SessionField="userid" Type="Int32" />
                            </UpdateParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </div>
   </asp:Content>
