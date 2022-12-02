<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_Report_teleCaller_salesVisit" CodeBehind="Report_teleCaller_salesVisit.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v10.2.Export, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList.Export" TagPrefix="dxeTreeList" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v10.2, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dxeTreeList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>--%>
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>
    <script type="text/javascript">
        function SignOff() {
            window.parent.SignOff()
        }
        function height() {
            if (document.body.scrollHeight >= 350) {
                window.frameElement.height = document.body.scrollHeight;
            }
            else {
                window.frameElement.height = '350px';
            }
            window.frameElement.widht = document.body.scrollWidht;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" colspan="4" style="text-align: center;">
                    <strong><span style="color: #000099">Sales Visit Report</span></strong>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top; text-align: left;">
                    <asp:Panel ID="pnl" BorderColor="blue" BorderWidth="0px" runat="server" Width="100%">
                        <table class="TableMain100">
                            <tr>
                                <td>
                                    <asp:Panel ID="Panel1" BorderColor="white" BorderWidth="1px" runat="server" Width="100%">
                                        <table width="50%">
                                            <tr>
                                                <td class="gridcellleft">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFromDate"
                                                        Display="Dynamic" EnableTheming="True" ErrorMessage="Date required!"></asp:RequiredFieldValidator></td>
                                                <td class="gridcellleft"></td>
                                                <td class="gridcellright"></td>
                                                <td class="gridcellleft"></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td class="gridcellleft" valign="middle">
                                                    <dxe:ASPxDateEdit ID="txtFromDate" runat="server" EditFormat="Custom" EditFormatString="dd MMMM yyyy" UseMaskBehavior="True">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                        <DropDownButton Text="From Date">
                                                        </DropDownButton>
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                                <td class="gridcellleft" valign="middle">
                                                    <dxe:ASPxDateEdit ID="txtToDate" runat="server" EditFormat="Custom" EditFormatString="dd MMMM yyyy" UseMaskBehavior="True">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                        <DropDownButton Text="To Date">
                                                        </DropDownButton>
                                                    </dxe:ASPxDateEdit>

                                                </td>
                                                <td class="gridcellright">
                                                    <span style="color: #000099">Report Type:</span></td>
                                                <td class="gridcellleft">
                                                    <dxe:ASPxRadioButtonList ID="RBReportType" runat="server" CssPostfix="BlackGlass" RepeatDirection="Horizontal" Height="2px" SelectedIndex="0" TextSpacing="3px">
                                                        <Items>
                                                            <dxe:ListEditItem Text="Screen" Value="Screen" />
                                                            <dxe:ListEditItem Text="Print" Value="Print" />
                                                        </Items>
                                                        <ValidationSettings ErrorText="Error has occurred">
                                                            <ErrorImage Height="14px" Width="14px" />
                                                        </ValidationSettings>
                                                    </dxe:ASPxRadioButtonList>
                                                </td>
                                                <td style="height: 25px">
                                                    <dxe:ASPxButton ID="btnShowReport" runat="server" Text="Show" Height="5px" Width="82px" OnClick="btnShowReport_Click">
                                                    </dxe:ASPxButton>
                                                </td>
                                            </tr>

                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; vertical-align: bottom">
                                    <table id="tblExport" runat="server">
                                        <tr>
                                            <td style="text-align: right; vertical-align: bottom">
                                                <dxe:ASPxButton ID="btnExpandAll" runat="server" Text="Expand All" Height="20px" AutoPostBack="false" Font-Size="12px">
                                                    <ClientSideEvents Click="function(s, e) {
                                                                                 List.ExpandAll();
                                                                             }" />

                                                </dxe:ASPxButton>
                                            </td>
                                            <td style="text-align: center vertical-align:bottom">
                                                <dxe:ASPxButton ID="btnCollapsAll" runat="server" Text="Collaps All" Height="11px" AutoPostBack="false" Font-Size="12px">
                                                    <ClientSideEvents Click="function(s, e) {
                                                                                 List.CollapseAll();
                                                                             }" />
                                                </dxe:ASPxButton>
                                            </td>
                                            <td style="vertical-align: bottom">
                                                <dxe:ASPxComboBox ID="cmbExport" runat="server" ValueType="System.String" Height="17px" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" SelectedIndex="0" AutoPostBack="true" Font-Overline="False" Font-Size="12px">
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
                            <tr>
                                <td>
                                    <dxeTreeList:aspxtreelist id="TLTeleSalesvisit" clientinstancename="List" runat="server" keyfieldname="User_ID" parentfieldname="User_ParentId" width="100%" autogeneratecolumns="False">
                                        <columns>
                                    <dxeTreeList:TreeListTextColumn Caption="User" FieldName="User" VisibleIndex="0">
                                    </dxeTreeList:TreeListTextColumn>
                                    <dxeTreeList:TreeListTextColumn Caption="total Lead/ Name" FieldName="LeadName" VisibleIndex="1">
                                    </dxeTreeList:TreeListTextColumn>
                                    <dxeTreeList:TreeListTextColumn Caption="Visit Date" FieldName="VisitDate" VisibleIndex="2">
                                    </dxeTreeList:TreeListTextColumn>
                                    <dxeTreeList:TreeListTextColumn Caption="Visit Place" FieldName="VisitPlace" VisibleIndex="3">
                                        <CellStyle Wrap="True">
                                        </CellStyle>
                                    </dxeTreeList:TreeListTextColumn>
                                    <dxeTreeList:TreeListTextColumn Caption="Status" FieldName="Status" VisibleIndex="4">
                                        <CellStyle Wrap="True">
                                        </CellStyle>
                                    </dxeTreeList:TreeListTextColumn>
                                </columns>
                                        <settings gridlines="Both" suppressoutergridlines="True" showgroupfooter="True" />
                                        <images>
                                    <ExpandedButton Height="11px"
                                        Width="11px" />
                                    <CustomizationWindowClose Height="12px" Width="13px" />
                                    <CollapsedButton Height="11px"
                                        Width="11px" />
                                </images>
                                        <settingspager showseparators="True">
                                    <FirstPageButton Visible="True">
                                    </FirstPageButton>
                                    <LastPageButton Visible="True">
                                    </LastPageButton>
                                </settingspager>
                                    </dxeTreeList:aspxtreelist>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <dxeTreeList:aspxtreelistexporter id="ASPxTreeListExporter1" runat="server">
                                    </dxeTreeList:aspxtreelistexporter>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
</asp:Content>
