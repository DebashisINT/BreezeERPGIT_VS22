<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="FinYearList.aspx.cs" Inherits="CutOff.CutOff.Master.FinYearList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function EditFinYear(Key) {
            window.location.href = "/CutOff/Master/FinYear.aspx?Key=" + Key;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Financial Year</h3>
        </div>
    </div>
    <div class="form_main">
        <div class="SearchArea">
            <table class="TableMain100">
                <%--<tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">Financial Year</span></strong>
                </td>
            </tr>--%>

                <tr>
                    <td>
                        <dxe:ASPxGridView ID="gridStatus" ClientInstanceName="grid" Width="100%"
                            KeyFieldName="FinYear_ID" DataSourceID="gridStatusDataSource" runat="server"
                            AutoGenerateColumns="False" OnCustomCallback="gridStatus_CustomCallback" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
                            <settingsbehavior allowfocusedrow="True" confirmdelete="True" />

                            <styles>
                            <%--   <Header CssClass="gridheader">
                                </Header>--%>
                            <%-- <FocusedRow CssClass="gridselectrow" BackColor="#FCA977">
                            </FocusedRow>--%>
                            <FocusedGroupRow CssClass="gridselectrow" BackColor="#FCA977">
                            </FocusedGroupRow>
                        </styles>
                            <%--                        <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>--%>
                            <settingssearchpanel visible="True" delay="7000" />
                            <columns>
                            <dxe:GridViewDataTextColumn Visible="false" FieldName="FinYear_ID" Caption="ID">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />

                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="FinYear_Code"
                                Caption="Financial Year">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />

                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="FinYear_StartDate"
                                Caption="Start Date">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle><Settings AllowAutoFilterTextInputTimer="False" />

                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="FinYear_EndDate"
                                Caption="End Date" Visible="true">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />

                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="FinYear_Remarks"
                                Caption="Remarks">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />

                                <%--  <DataItemTemplate>
                                    <a href="javascript:void(0);" onclick="javascript:OnAddButtonClick();"><span style="text-decoration: underline">Edit</span> </a>
                                </DataItemTemplate>--%>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>


                           <dxe:GridViewDataTextColumn FieldName="Actions" VisibleIndex="4" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" Width="5%">
                                <Settings AllowAutoFilter="False"></Settings>
                                <DataItemTemplate>
                                    <a href="javascript:void(0);" onclick="EditFinYear('<%# Container.KeyValue %>')" title="Status"><img src="../../../assests/images/Edit.png" />                                                              
                                    </a>
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <HeaderStyle Wrap="False" />
                            </dxe:GridViewDataTextColumn>
                             </columns>
                            <settingscontextmenu enabled="true"></settingscontextmenu>
                            <%-- <dxe:GridViewDataTextColumn VisibleIndex="5" Width="60px" Caption="Details">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <HeaderTemplate>
                                    <a href="javascript:void(0);" onclick="javascript:OnAddButtonClick();"><span style="color: #000099; text-decoration: underline">Add New</span> </a>
                                </HeaderTemplate>
                                <DataItemTemplate>
                                    <a href="javascript:void(0);" onclick="DeleteRow('<%# Container.KeyValue %>')">
                                        <img src="../../../assests/images/Delete.png" /></a>
                                </DataItemTemplate>
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>--%>
                            <%--  <dxe:GridViewCommandColumn ShowEditButton="true" VisibleIndex="4" ShowDeleteButton="false" ButtonType="Button">
                                 <HeaderTemplate>
                                     <span>Action</span>
                                 </HeaderTemplate>
                             </dxe:GridViewCommandColumn>
                        </columns>
                        <settingscommandbutton>

                            <EditButton Image-ToolTip="Delete" ButtonType="Image" Image-Url="../../../assests/images/Edit.png" Text="Edit"></EditButton>
                            <DeleteButton Image-ToolTip="Delete"  ButtonType="Image" Image-Url="../../../assests/images/Delete.png" Text="Delete"></DeleteButton>
                            <UpdateButton Text="Save" ButtonType="Button" Image-Width="25px"></UpdateButton>
                            <CancelButton Text="Cancel" ButtonType="Button" Image-Width="25px"></CancelButton>
                        </settingscommandbutton>--%>
                            <settingstext confirmdelete="Confirm delete?" />
                            <settingssearchpanel visible="True" />
                            <settings showfilterrow="true" showgrouppanel="true" showfilterrowmenu="true" />
                            <settingsbehavior allowfocusedrow="false" columnresizemode="NextColumn" />
                            <styleseditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </styleseditors>
                        </dxe:ASPxGridView>
                        <asp:SqlDataSource ID="gridStatusDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:crmConnectionString %>"
                            SelectCommand="">
                            <SelectParameters>
                                <asp:SessionParameter Name="userlist" SessionField="userchildHierarchy" Type="string" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </div>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
</asp:Content>
