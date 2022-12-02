<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_NewfrmperformanceReport_telecaller" CodeBehind="frmperformanceReport_telecaller.aspx.cs" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script language="javascript" type="text/javascript">
        function SignOff() {
            window.parent.SignOff();
        }
        function height() {
            if (document.body.scrollHeight >= 500)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '500px';
            window.frameElement.Width = document.body.scrollWidth;
        }
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" colspan="4" style="text-align: center;">
                    <strong><span style="color: #000099">Performance Report Sales</span></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pnl" BorderColor="white" BorderWidth="1px" runat="server">
                        <table>
                            <tr>
                                <td class="gridcellleft">
                                    <dxe:ASPxDateEdit ID="txtFromDate" runat="server" EditFormat="Custom" UseMaskBehavior="True">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                        <DropDownButton Text="From Date">
                                        </DropDownButton>
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td class="gridcellleft">
                                    <dxe:ASPxDateEdit ID="txtToDate" runat="server" EditFormat="Custom" UseMaskBehavior="True">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                        <DropDownButton Text="To Date">
                                        </DropDownButton>
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td class="gridcellleft">
                                    <asp:Button ID="btnGetReport" runat="server" Text="Get Report" CssClass="btnUpdate"
                                        Height="19px" Font-Size="12px" OnClick="btnGetReport_Click" /></td>
                                <td class="gridcellleft">
                                    <table id="tblExportV" runat="server">
                                        <tr>
                                            <td colspan="4" class="gridcellright">
                                                <dxe:ASPxComboBox ID="ASPxComboBox1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ASPxComboBox1_SelectedIndexChanged"
                                                    ValueType="System.String">
                                                    <Items>
                                                        <dxe:ListEditItem Text="Select" Value="" />
                                                        <dxe:ListEditItem Text="Pdf" Value="Pdf" />
                                                        <dxe:ListEditItem Text="Xls" Value="Xls" />
                                                        <dxe:ListEditItem Text="Rtf" Value="Rtf" />
                                                    </Items>
                                                    <DropDownButton Text="Export">
                                                    </DropDownButton>
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFromDate"
                                        Display="Dynamic" EnableTheming="True" ErrorMessage="Date required!"></asp:RequiredFieldValidator></td>
                                <td class="gridcellleft">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtToDate"
                                        Display="Dynamic" EnableTheming="True" ErrorMessage="Date required!"></asp:RequiredFieldValidator></td>
                                <td class="gridcellleft"></td>
                                <td class="gridcellleft"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <dxeTreeList:ASPxTreeList ID="TLLead" runat="server" AutoGenerateColumns="False" KeyFieldName="User_ID"
                        ParentFieldName="User_ParentId" OnHtmlRowPrepared="TLLead_HtmlRowPrepared" Width="100%">
                        <columns>
                                <dxeTreeList:TreeListTextColumn Caption="Caller" FieldName="Caller" VisibleIndex="0">
                                </dxeTreeList:TreeListTextColumn>
                                <dxeTreeList:TreeListTextColumn Caption="Total Calls" FieldName="TotalCalls" VisibleIndex="1">
                                </dxeTreeList:TreeListTextColumn>
                                <dxeTreeList:TreeListTextColumn Caption="Call Back" FieldName="CallBack" VisibleIndex="2">
                                </dxeTreeList:TreeListTextColumn>
                                <dxeTreeList:TreeListTextColumn Caption="Sales Visit" FieldName="SalesVisit" VisibleIndex="3">
                                </dxeTreeList:TreeListTextColumn>
                                <dxeTreeList:TreeListTextColumn Caption="Confirm Sales" FieldName="ConfirmSales" VisibleIndex="4">
                                </dxeTreeList:TreeListTextColumn>
                                <dxeTreeList:TreeListTextColumn Caption="Not Interested" FieldName="NotInterested" VisibleIndex="5">
                                </dxeTreeList:TreeListTextColumn>
                                <dxeTreeList:TreeListTextColumn Caption="Non Contactable" FieldName="NonContactable" VisibleIndex="6">
                                </dxeTreeList:TreeListTextColumn>
                                <dxeTreeList:TreeListTextColumn Caption="Non Usabled" FieldName="NonUsabled" VisibleIndex="7">
                                </dxeTreeList:TreeListTextColumn>
                                <dxeTreeList:TreeListTextColumn Caption="Contactiblity(%)" FieldName="Contactiblity" VisibleIndex="8">
                                </dxeTreeList:TreeListTextColumn>
                                <dxeTreeList:TreeListTextColumn Caption="Effectiveness" FieldName="Effectiveness" VisibleIndex="9">
                                </dxeTreeList:TreeListTextColumn>
                                <dxeTreeList:TreeListTextColumn Caption="Avg. Times(min)" FieldName="Avg" VisibleIndex="10">
                                </dxeTreeList:TreeListTextColumn>
                            </columns>
                        <summary>
                            <dxeTreeList:TreeListSummaryItem FieldName="TotalCalls" ShowInColumn="TotalCalls" SummaryType="Sum"
                                DisplayFormat="{0:n0}" />
                            <dxeTreeList:TreeListSummaryItem FieldName="CallBack" ShowInColumn="CallBack" SummaryType="Sum"
                                DisplayFormat="{0:n0}" />
                            <dxeTreeList:TreeListSummaryItem FieldName="SalesVisit" ShowInColumn="SalesVisit" SummaryType="Sum"
                                DisplayFormat="{0:n0}" />
                            <dxeTreeList:TreeListSummaryItem FieldName="ConfirmSales" ShowInColumn="ConfirmSales" SummaryType="Sum"
                                DisplayFormat="{0:n0}" />
                            <dxeTreeList:TreeListSummaryItem FieldName="NotInterested" ShowInColumn="NotInterested"
                                SummaryType="Sum" DisplayFormat="{0:n0}" />
                            <dxeTreeList:TreeListSummaryItem FieldName="NonContactable" ShowInColumn="NonContactable"
                                SummaryType="Sum" DisplayFormat="{0:n0}" />
                            <dxeTreeList:TreeListSummaryItem FieldName="NonUsabled" ShowInColumn="NonUsabled" SummaryType="Sum"
                                DisplayFormat="{0:n0}" />
                        </summary>
                        <styles>
                                <GroupFooterCell HorizontalAlign="Center" VerticalAlign="Middle">
                                </GroupFooterCell>
                                <FooterCell HorizontalAlign="Center">
                                </FooterCell>
                            </styles>
                        <settings gridlines="Both" showgroupfooter="True" />
                        <images>
                                <ExpandedButton Height="11px" Width="11px" />
                                <CustomizationWindowClose Height="12px" Width="13px" />
                                <CollapsedButton Height="11px" Width="11px" />
                            </images>
                        <settingspager showseparators="True">
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </settingspager>
                    </dxeTreeList:ASPxTreeList>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="4">
                    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label></td>
            </tr>
            <tr>
                <td colspan="4">
                    <dxeTreeList:ASPxTreeListExporter ID="ASPxTreeListExporter1" runat="server" TreeListID="TLLead">
                    </dxeTreeList:ASPxTreeListExporter>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
