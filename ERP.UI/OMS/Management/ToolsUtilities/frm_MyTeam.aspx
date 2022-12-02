<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_utilities_frm_MyTeam" CodeBehind="frm_MyTeam.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <%--    <script type="text/javascript">
     function LastCall(obj)
        {
            height();
        }
    function height()
    {
        if(document.body.scrollHeight>300)
            window.frameElement.height = document.body.scrollHeight;
        else
            window.frameElement.height = '300px';
        window.frameElement.widht = document.body.scrollWidht;
    }
    </script>--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Team Details</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td style="text-align: left; vertical-align: top">
                    <table>
                        <tr>
                            <td>
                                <dxe:ASPxButton ID="btnExpandAll" runat="server" Text="Expand All" AutoPostBack="false" CssClass="btn btn-warning">
                                    <ClientSideEvents Click="function(s, e) {
                                                                                 List.ExpandAll();
                                                                             }" />
                                </dxe:ASPxButton>
                            </td>
                            <td>
                                <dxe:ASPxButton ID="btnCollapsAll" runat="server" Text="Collaps All" AutoPostBack="false" CssClass="btn btn-warning">
                                    <ClientSideEvents Click="function(s, e) {
                                                                                 List.CollapseAll();
                                                                             }" />
                                </dxe:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="gridcellright" style="float: right; vertical-align: top">
                    <dxe:ASPxComboBox ID="cmbExport" runat="server" ValueType="System.String" SelectedIndex="0"
                        AutoPostBack="true" Width="130px" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged">
                        <Items>
                            <dxe:ListEditItem Text="Select" Value="" />
                            <dxe:ListEditItem Text="Pdf" Value="Pdf" />
                            <dxe:ListEditItem Text="Xls" Value="Xls" />
                            <dxe:ListEditItem Text="Rtf" Value="Rtf" />
                        </Items>
                        <Border BorderColor="black" />
                        <DropDownButton Text="Export" ToolTip="Export File">
                        </DropDownButton>
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <dxeTreeList:ASPxTreeListExporter ID="ASPxTreeListExporter1" runat="server" TreeListID="TVorgHir">
                    </dxeTreeList:ASPxTreeListExporter>
                </td>
            </tr>

            <tr>
                <td colspan="2">
                    <dxeTreeList:ASPxTreeList ID="TVorgHir" runat="server" ClientInstanceName="List" AutoGenerateColumns="False"
                        ParentFieldName="ParentID" KeyFieldName="ID">
                        <Settings SuppressOuterGridLines="True" GridLines="Both"></Settings>
                        <Columns>
                            <dxeTreeList:TreeListTextColumn Caption="Name[Company Name: Branch: Designation:Mobile No:Email Id]"
                                FieldName="Name" VisibleIndex="0">
                                <CellStyle HorizontalAlign="Left">
                                </CellStyle>
                            </dxeTreeList:TreeListTextColumn>
                        </Columns>
                        <ClientSideEvents EndCallback="function(s, e) {
	LastCall(s.cpHeight);
}" />
                        <Border BorderColor="Navy" BorderStyle="Solid" BorderWidth="1px" />
                        <Styles>
                            <Header CssClass="EHEADER">
                            </Header>
                        </Styles>
                    </dxeTreeList:ASPxTreeList>
                </td>
            </tr>
        </table>
    </div>

</asp:Content>
