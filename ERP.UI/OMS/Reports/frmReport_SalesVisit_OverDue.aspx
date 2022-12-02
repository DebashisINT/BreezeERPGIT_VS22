<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_SalesVisit_OverDue" CodeBehind="frmReport_SalesVisit_OverDue.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <script language="javascript" type="text/javascript">
        FieldName = 'ctl00_ContentPlaceHolder3_TxtStartDate';
    </script>
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Overdue Report</h3>
        </div>

    </div>
    <div class="crossBtn"><a href="../Management/ProjectMainPage.aspx"><i class="fa fa-times"></i></a></div>
    <div class="form_main">
        <table class="TableMain100">
            <%--<tr>
                    <td class="EHEADER" style="text-align: center;" colspan="4">
                        <strong><span style="color: #000099">Overdue Report</span></strong>
                    </td>
                </tr>--%>
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Panel ID="Panel2" BorderColor="white" BorderWidth="1px" runat="server" Width="100%">
                                    <table style="width: 100%">
                                        <tr>
                                            <td>
                                                <table class="TableMain100">
                                                    <tr>
                                                        <td class="gridcellright" valign="top" style="width: 66px; height: 25px;">
                                                            <span style="color: #000099">Report Type:</span></td>
                                                        <td class="gridcellleft" style="width: 125px; height: 25px;" valign="top">
                                                            <dxe:ASPxRadioButtonList ID="RBReportType" runat="server" RepeatDirection="Horizontal"
                                                                Height="2px" SelectedIndex="0" TextSpacing="3px" CssPostfix="BlackGlass">
                                                                <Items>
                                                                    <dxe:ListEditItem Text="Screen" Value="Screen" />
                                                                    <dxe:ListEditItem Text="Print" Value="Print" />
                                                                </Items>
                                                                <ValidationSettings ErrorText="Error has occurred">
                                                                    <ErrorImage Height="14px" Width="14px" />
                                                                </ValidationSettings>
                                                            </dxe:ASPxRadioButtonList>
                                                        </td>
                                                        <td style="height: 25px; text-align: left;" valign="top">
                                                            <dxe:ASPxButton ID="btnShowReport" runat="server" Text="Show" CssClass="btn btn-success"
                                                                OnClick="btnShowReport_Click">
                                                            </dxe:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="right">
                                                <table id="Table1" runat="server">
                                                    <tr>
                                                        <td style="text-align: right; vertical-align: bottom">
                                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" Text="Expand All" Height="20px" AutoPostBack="false"
                                                                Font-Size="12px" CssClass="btn btn-warning">
                                                                <ClientSideEvents Click="function(s, e) {
                                                                                      List.ExpandAll();
                                                                                      }" />
                                                            </dxe:ASPxButton>
                                                        </td>
                                                        <td style="text-align: center vertical-align:bottom">
                                                            <dxe:ASPxButton ID="ASPxButton2" runat="server" Text="Collaps All" Height="11px" CssClass="btn btn-warning"
                                                                AutoPostBack="false" Font-Size="12px">
                                                                <ClientSideEvents Click="function(s, e) {
                                                                                      List.CollapseAll();
                                                                                      }" />
                                                            </dxe:ASPxButton>
                                                        </td>
                                                        <td style="vertical-align: bottom">
                                                            <dxe:ASPxComboBox ID="cmbExport" runat="server" ValueType="System.String" Height="17px"
                                                                OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" SelectedIndex="0" AutoPostBack="true"
                                                                Font-Overline="False" Font-Size="12px">
                                                                <ButtonStyle Width="13px">
                                                                </ButtonStyle>
                                                                <Items>
                                                                    <dxe:ListEditItem Text="Select" Value="0" />
                                                                    <dxe:ListEditItem Text="PDF" Value="1" />
                                                                    <dxe:ListEditItem Text="XLS" Value="2" />
                                                                    <dxe:ListEditItem Text="RTF" Value="3" />
                                                                    <dxe:ListEditItem Text="CSV" Value="4" />

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
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="Panel1" runat="server" Width="100%">
                                    <dxeTreeList:ASPxTreeList ID="TvoOverDue" runat="server" ClientInstanceName="List" Width="100%"
                                        KeyFieldName="ID" ParentFieldName="ParentID" AutoGenerateColumns="False">
                                        <Styles>
                                        </Styles>
                                        <Settings SuppressOuterGridLines="True" GridLines="None"/>
                                        <Images>
                                            <ExpandedButton Height="11px" Width="11px" />
                                            <CustomizationWindowClose Height="12px" Width="13px" />
                                            <CollapsedButton Height="11px" Width="11px" />
                                        </Images>
                                        <Columns>
                                            <dxeTreeList:TreeListTextColumn Caption="Name" FieldName="Name1" VisibleIndex="0">
                                                <CellStyle CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxeTreeList:TreeListTextColumn>
                                            <dxeTreeList:TreeListTextColumn Caption="Visit DateTime" FieldName="Visit DateTime" VisibleIndex="1">
                                                <CellStyle CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxeTreeList:TreeListTextColumn>
                                            <dxeTreeList:TreeListTextColumn Caption="ActivityType" FieldName="ActivityType" VisibleIndex="2">
                                                <CellStyle CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxeTreeList:TreeListTextColumn>
                                            <dxeTreeList:TreeListTextColumn Caption="Address" FieldName="Address" VisibleIndex="3"
                                                Width="30%">
                                                <CellStyle Wrap="True" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxeTreeList:TreeListTextColumn>
                                            <dxeTreeList:TreeListTextColumn Caption="PhoneNo" FieldName="PhoneNo" VisibleIndex="4">
                                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                                </CellStyle>
                                            </dxeTreeList:TreeListTextColumn>
                                            <dxeTreeList:TreeListTextColumn Caption="LastOutCome" FieldName="LastOutCome" VisibleIndex="5">
                                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                                </CellStyle>
                                            </dxeTreeList:TreeListTextColumn>
                                            <dxeTreeList:TreeListTextColumn Caption="Last Visit" FieldName="Last Visit" VisibleIndex="6">
                                                <CellStyle CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxeTreeList:TreeListTextColumn>
                                        </Columns>
                                    </dxeTreeList:ASPxTreeList>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <dxeTreeList:ASPxTreeListExporter ID="ASPxTreeListExporter1" runat="server">
                    </dxeTreeList:ASPxTreeListExporter>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
