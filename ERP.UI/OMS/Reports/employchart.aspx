<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_employchart" Codebehind="employchart.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="panel-heading">
       <div class="panel-title">
           <h3>organizational hierarchy</h3>
       </div>

   </div> 
      <div class="crossBtn"><a href="../Management/ProjectMainPage.aspx"><i class="fa fa-times"></i></a></div>
    <div class="form_main">
        <table class="TableMain100" cellpadding="opx" cellspacing="0px">
          
            <tr>
                <td>
                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr style="display:none;">
                <td style="vertical-align: top; text-align: left;">
                    <asp:Panel ID="pnl" BorderColor="blue" BorderWidth="1px" runat="server" Visible="false">
                        <table>
                            <tr>
                                <td class="gridcellleft" style="width: 83px">
                                    <asp:TreeView ID="TVOrgHirchy" runat="server" ExpandDepth="0" ShowLines="True" ForeColor="Blue" OnSelectedNodeChanged="TVOrgHirchy_SelectedNodeChanged">
                                        <ParentNodeStyle BackColor="Gainsboro" Font-Bold="True" ForeColor="Blue" />
                                        <LevelStyles>
                                            <asp:TreeNodeStyle Font-Underline="False" ForeColor="Blue" />
                                        </LevelStyles>
                                        <HoverNodeStyle ForeColor="Black" BackColor="WhiteSmoke" Font-Bold="True" />
                                        <SelectedNodeStyle ChildNodesPadding="1px" ForeColor="Blue" />
                                        <RootNodeStyle BackColor="Gainsboro" Font-Bold="True" ForeColor="HotTrack" />
                                        <LeafNodeStyle ForeColor="Blue" />
                                        <NodeStyle ForeColor="Blue" />
                                    </asp:TreeView>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <table class="TableMain100"  cellpadding="0px" cellspacing="0px">
                        <tr>
                            <td style="vertical-align: top;" class="gridcellcenter">
                                <table width="100%" cellpadding="0px" cellspacing="0px">
                                    <tr>
                                        <td class="gridcellleft" align="left">
                                            <table>
                                                <tr>
                                                    <td style="padding-right:15px">
                                                        <dxe:ASPxComboBox ID="cmbCompany" runat="server" ClientInstanceName="cmbCompany" 
                                                            EnableIncrementalFiltering="True" Font-Bold="False" Font-Size="12px" ValueType="System.String" >
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                            <DropDownButton Text="Company">
                                                            </DropDownButton>
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td style="padding-right:15px">
                                                        <dxe:ASPxComboBox ID="cmbBranch" runat="server" ClientInstanceName="cmbBranch" EnableIncrementalFiltering="True"
                                                            Font-Overline="False" Font-Size="12px"  ValueType="System.String"
                                                            >
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                            <DropDownButton Text="Branch">
                                                            </DropDownButton>
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td style="padding-right:15px">
                                                        <dxe:ASPxButton ID="btnSubmit" runat="server" AutoPostBack="False" Text="Show" EnableClientSideAPI="true"  CssClass="btn btn-xs btn-primary">
                                                            <ClientSideEvents Click="function(s, e) {
	                                                            List.PerformCallback('');
                                                            }" />
                                                        </dxe:ASPxButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td class="gridcellright" align="right">
                                            <table>
                                                <tr>
                                                    <%--<td>
                                                        <dxe:ASPxButton ID="btnExpandAll" runat="server" Text="Expand All" 
                                                            AutoPostBack="false" CssClass="btn btn-warning btn-xs">
                                                            <ClientSideEvents Click="function(s, e) {
                                                                                 List.ExpandAll();
                                                                             }" />
                                                        </dxe:ASPxButton>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxButton ID="btnCollapsAll" runat="server" Text="Collaps All" 
                                                            AutoPostBack="false" CssClass="btn btn-warning btn-xs">
                                                            <ClientSideEvents Click="function(s, e) {
                                                                                 List.CollapseAll();
                                                                             }" />
                                                        </dxe:ASPxButton>
                                                    </td>--%>
                                                    <td>
                                                        <dxe:ASPxComboBox ID="cmbExport" runat="server" ValueType="System.String" 
                                                            OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" SelectedIndex="0" AutoPostBack="true" Width="103px" >
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                            <Items>
                                                                <dxe:ListEditItem Text="Select" Value="" />
                                                                <dxe:ListEditItem Text="Pdf" Value="Pdf" />
                                                                <dxe:ListEditItem Text="Xls" Value="Xls" />
                                                                <dxe:ListEditItem Text="Rtf" Value="Rtf" />
                                                            </Items>
                                                            <DropDownButton Text="Export" ToolTip="Export File">
                                                            </DropDownButton>
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellcenter">
                                <dxeTreeList:ASPxTreeList ID="TVorgHir" ClientInstanceName="List" runat="server" KeyFieldName="ID"
                                    ParentFieldName="ParentID" OnCustomCallback="TVorgHir_CustomCallback" OnHtmlDataCellPrepared="TVorgHir_HtmlDataCellPrepared" AutoGenerateColumns="False" OnCustomJSProperties="TVorgHir_CustomJSProperties">
                                    <Settings SuppressOuterGridLines="True" GridLines="Both"></Settings>
                                    <Columns>
                                        <dxeTreeList:TreeListTextColumn Caption="Name[Company Name: Branch: Designation]" FieldName="Name"
                                            VisibleIndex="0">
                                            <CellStyle HorizontalAlign="Left">
                                            </CellStyle>
                                        </dxeTreeList:TreeListTextColumn>
                                    </Columns>
                                    <ClientSideEvents EndCallback="function(s, e) {
	LastCall(s.cpHeight);
}" />
                                    
                                </dxeTreeList:ASPxTreeList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <dxeTreeList:ASPxTreeListExporter ID="ASPxTreeListExporter1" runat="server" TreeListID="TVorgHir">
                    </dxeTreeList:ASPxTreeListExporter>
                </td>
            </tr>
        </table>
        </div>
    </asp:Content>
