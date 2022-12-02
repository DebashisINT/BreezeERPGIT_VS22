<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.management_UploadDigitalSignature_AuthUser" CodeBehind="UploadDigitalSignature_AuthUser.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script lang="javascript" type="text/javascript">
        function SignOff() {
            window.parent.SignOff();
        }
        function PageLoad() {
            FieldName = 'A4';

            document.getElementById('txtValidUser_hidden').style.display = "none";

        }
        function CallAjax(obj1, obj2, obj3) {
            ajax_showOptions(obj1, obj2, obj3);
        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function closeWin() {
            parent.editwin.close();
            // parent.DhtmlClose();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="tblSummary" border="0" cellpadding="0" cellspacing="0" class="TableMain100"
        style="width: 100%; padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;">
        <tr>
            <td colspan="2" style="text-align: left;"></td>
            <td class="gridcellright" colspan="4" valign="top"></td>
        </tr>
        <tr>
            <td colspan="6" style="text-align: left">
                <dxe:ASPxGridView ID="gridAuthUser" runat="server" AutoGenerateColumns="False"
                    ClientInstanceName="grid" KeyFieldName="col" Width="100%" OnRowDeleting="gridAuthUser_RowDeleting"
                    OnRowInserting="gridAuthUser_RowInserting">
                    <Templates>
                        <EditForm>
                            <table>
                                <tr>
                                    <td>Name:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValidUser" runat="server" onkeyup="CallAjax(this,'SearchEmployeesForDigitalSignatureUser',event)"
                                            Width="238px"></asp:TextBox>
                                        <asp:HiddenField ID="txtValidUser_hidden" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxButton ID="btnAdd" runat="server" AutoPostBack="False" Text="Add" ValidationGroup="a">
                                                        <ClientSideEvents Click="function(s, e) {grid.UpdateEdit();}" />
                                                    </dxe:ASPxButton>
                                                </td>
                                                <td>
                                                    <dxe:ASPxButton ID="btnCancel" runat="server" AutoPostBack="False" Text="Cancel"
                                                        ValidationGroup="a">
                                                        <ClientSideEvents Click="function(s, e) {grid.CancelEdit();}" />
                                                    </dxe:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>

                        </EditForm>
                    </Templates>
                    <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
                    <Styles>
                        <Header CssClass="gridheader"></Header>

                        <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>

                        <FocusedRow CssClass="gridselectrow"></FocusedRow>
                    </Styles>
                    <SettingsPager AlwaysShowPager="True" ShowSeparators="True">
                        <FirstPageButton Visible="True"></FirstPageButton>

                        <LastPageButton Visible="True"></LastPageButton>
                    </SettingsPager>
                    <Columns>
                        <dxe:GridViewDataTextColumn FieldName="name" Caption="Name" VisibleIndex="0">
                            <Settings FilterMode="DisplayText" AutoFilterCondition="Contains"></Settings>

                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewCommandColumn Width="10%" VisibleIndex="1" ShowDeleteButton="True">
                            <HeaderTemplate>
                                <a href="javascript:void(0);" onclick="grid.AddNewRow()">
                                    <span style="color: #000099; text-decoration: underline">Add New</span>
                                </a>
                            </HeaderTemplate>
                        </dxe:GridViewCommandColumn>
                    </Columns>
                    <Settings ShowStatusBar="Visible" />
                    <StylesEditors>
                        <ProgressBar Height="25px"></ProgressBar>
                    </StylesEditors>
                </dxe:ASPxGridView>
            </td>
        </tr>
        <tr>
            <td colspan="6" style="text-align: left; padding-left: 10px">
                <br />
                <asp:Button ID="btnClose" runat="server" CssClass="btnUpdate" Height="23px" OnClientClick="javascript:closeWin();"
                    Text="Close" Width="101px" />
            </td>
        </tr>
    </table>
</asp:Content>
