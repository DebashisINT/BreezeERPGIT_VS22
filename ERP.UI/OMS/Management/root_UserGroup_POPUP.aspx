<%@ Page Title="CRM" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.root_UserGroup_POPUP" CodeBehind="root_UserGroup_POPUP.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table style="padding-left: 0px; padding-top: 0px;">
            <tr>
                <td colspan="2">
                    <strong><span style="font-size: 10pt; color: #3300cc">Please Don't Uncheck Any check Box. For No Access Use 'Non' Option!</span></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <dxeTreeList:ASPxTreeList ID="TLgrid" runat="server" AutoGenerateColumns="False" KeyFieldName="smu_id" ParentFieldName="menuParentID" CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css" CssPostfix="Office2003_Blue" OnHtmlRowPrepared="TLgrid_HtmlRowPrepared">
                        <columns>
                                        
                                        <dxeTreeList:TreeListTextColumn Caption="name" FieldName="smu_name" VisibleIndex="1">
                                        </dxeTreeList:TreeListTextColumn>
                                        <dxeTreeList:TreeListTextColumn Caption="Option" VisibleIndex="2">
                                        <DataCellTemplate>
                                                    <dxe:ASPxTextBox ID="ASPxTextBox1" runat="server" Width="77px" Text='<%# Eval("Mode") %>'><MaskSettings ShowHints="True" Mask="&lt;All|Add|*View|DelAdd|Modify|Delete|Non&gt;" /></dxe:ASPxTextBox>
                                        </DataCellTemplate>
                                            <HeaderStyle Wrap="False" />
                                            <CellStyle Wrap="False">
                                            </CellStyle>
                                        </dxeTreeList:TreeListTextColumn>
                                        
                                    </columns>
                        <styles cssfilepath="~/App_Themes/Office2003 Blue/{0}/styles.css" csspostfix="Office2003_Blue">
                                    </styles>
                        <settings gridlines="Both" />
                        <settingsbehavior allowsort="False" autoexpandallnodes="True" />
                        <images imagefolder="~/App_Themes/Office2003 Blue/{0}/">
                                        <ExpandedButton Height="11px" Url="~/App_Themes/Office2003 Blue/TreeList/ExpandedButton.png"
                                            Width="11px" />
                                        <CustomizationWindowClose Height="12px" Width="13px" />
                                        <CollapsedButton Height="11px" Url="~/App_Themes/Office2003 Blue/TreeList/CollapsedButton.png"
                                            Width="11px" />
                                    </images>
                        <settingsselection enabled="True" />
                    </dxeTreeList:ASPxTreeList>
                </td>
                <td style="text-align: left; vertical-align: top;">
                    <dxe:ASPxButton ID="btnSave" runat="server" Text="Save" CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css" CssPostfix="Office2003_Blue" OnClick="btnSave_Click">
                    </dxe:ASPxButton>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

