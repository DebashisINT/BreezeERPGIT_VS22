<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_RegionwiseBranch" CodeBehind="frmReport_RegionwiseBranch.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Regionwise Branch Distribution Report</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain" style="width: 100%;">
            <%--        <tr>
            <td class="EHEADER" colspan="4">
                <strong><span style="color: #000099">Regionwise Branch Distribution Report</span></strong>
            </td>
        </tr>--%>
            <tr>
                <td>
                    <dxeTreeList:ASPxTreeList ID="TVRegionBranchHir" ClientInstanceName="List" runat="server"
                        CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css" CssPostfix="Office2003_Blue"
                        KeyFieldName="ID" ParentFieldName="ParentID" Width="100%">
                        <Styles CssPostfix="Office2003_Blue" CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css">
                            <FooterCell HorizontalAlign="Right">
                            </FooterCell>
                        </Styles>
                        <Settings SuppressOuterGridLines="True" GridLines="Both"></Settings>
                        <Images ImageFolder="~/App_Themes/Office2003 Blue/{0}/">
                            <ExpandedButton Height="11px" Width="11px" Url="~/App_Themes/Office2003 Blue/TreeList/ExpandedButton.png">
                            </ExpandedButton>
                            <CustomizationWindowClose Height="12px" Width="13px">
                            </CustomizationWindowClose>
                            <CollapsedButton Height="11px" Width="11px" Url="~/App_Themes/Office2003 Blue/TreeList/CollapsedButton.png">
                            </CollapsedButton>
                        </Images>
                        <Columns>
                            <dxeTreeList:TreeListTextColumn Caption="Country" FieldName="Name" VisibleIndex="0">
                                <CellStyle HorizontalAlign="Left">
                                </CellStyle>
                            </dxeTreeList:TreeListTextColumn>
                        </Columns>
                        <SettingsBehavior AllowFocusedNode="True" />
                    </dxeTreeList:ASPxTreeList>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

