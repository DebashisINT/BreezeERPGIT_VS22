<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_frmperformanceReport_salesvisit" Codebehind="frmperformanceReport_salesvisit.aspx.cs" %>


<%--<%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Performance Report Sales</h3>
        </div>

    </div>
     <div class="crossBtn"><a href="../Management/ProjectMainPage.aspx"><i class="fa fa-times"></i></a></div>
    <div class="form_main">
     <asp:Panel ID="pnl" BorderColor="white" BorderWidth="1px" runat="server">
        <table>
            <tr>
                <td style="padding-right: 15px;">
                    <dxe:ASPxDateEdit ID="txtFromDate" runat="server" UseMaskBehavior="True" EditFormat="Custom">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                        <DropDownButton Text="From Date" Position="right">
                        </DropDownButton>
                                            </dxe:ASPxDateEdit>
                            
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFromDate"
                        Display="Dynamic" EnableTheming="True" ErrorMessage="Date required!"></asp:RequiredFieldValidator>
                </td>
                <td style="padding-right: 15px;">
                    <dxe:ASPxDateEdit ID="txtToDate" runat="server" UseMaskBehavior="True" EditFormat="Custom">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                        <DropDownButton Text="To Date" Position="right">
                        </DropDownButton>
                                            </dxe:ASPxDateEdit>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtToDate"
                        Display="Dynamic" EnableTheming="True" ErrorMessage="Date required!"></asp:RequiredFieldValidator></td>
                <td class="gridcellleft">
                    <asp:Button ID="btnGetReport" runat="server" Text="Get Report"  CssClass="btn btn-success"  OnClick="btnGetReport_Click"/></td>
                <td class="gridcellleft">
                    <dxe:ASPxComboBox ID="ASPxComboBox1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ASPxComboBox1_SelectedIndexChanged" ValueType="System.String" CaptionCellStyle-CssClass="pull-right">
                        <Items>
                    <dxe:ListEditItem Text="Select" Value="0" />
                    <dxe:ListEditItem Text="PDF" Value="1" />
                    <dxe:ListEditItem Text="XLS" Value="2" />
                    <dxe:ListEditItem Text="RTF" Value="3" />
                    <dxe:ListEditItem Text="CSV" Value="4" />
                        </Items>
                        <DropDownButton Text="Export">
                        </DropDownButton>
                    </dxe:ASPxComboBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <table class="TableMain100">
        <%--<tr>
            <td class="EHEADER" colspan="4" style="text-align:center;">
                <strong><span style="color: #000099">Performance Report Sales</span></strong>
            </td>
        </tr>--%>
        
        <tr>
            <td colspan="4">
                <dxeTreeList:ASPxTreeList ID="TLLead" runat="server" AutoGenerateColumns="False" KeyFieldName="User_ID" ParentFieldName="User_ParentId"
                    Font-Size="12px" Width="100%">
                    <Columns>
                        <dxeTreeList:TreeListTextColumn Caption="User" FieldName="User" VisibleIndex="0">
                        </dxeTreeList:TreeListTextColumn>
                        <dxeTreeList:TreeListTextColumn Caption="Alotd. (New)" FieldName="Alotd" VisibleIndex="1">
                        </dxeTreeList:TreeListTextColumn>
                        <dxeTreeList:TreeListTextColumn Caption="Contctd" FieldName="Contctd" VisibleIndex="2">
                        </dxeTreeList:TreeListTextColumn>
                        <dxeTreeList:TreeListTextColumn Caption="Visits(T)" FieldName="VisitsT" VisibleIndex="3">
                        </dxeTreeList:TreeListTextColumn>
                        <dxeTreeList:TreeListTextColumn Caption="Visits(U)" FieldName="VisitsU" VisibleIndex="4">
                        </dxeTreeList:TreeListTextColumn>
                        <dxeTreeList:TreeListTextColumn Caption="Calls(T)" FieldName="CallsT" VisibleIndex="5">
                        </dxeTreeList:TreeListTextColumn>
                        <dxeTreeList:TreeListTextColumn Caption="Calls(U)" FieldName="CallsU" VisibleIndex="6">
                        </dxeTreeList:TreeListTextColumn>
                        <dxeTreeList:TreeListTextColumn Caption="Pending" FieldName="Pending" VisibleIndex="7">
                        </dxeTreeList:TreeListTextColumn>
                        <dxeTreeList:TreeListTextColumn Caption="Refix" FieldName="Refix" VisibleIndex="8">
                        </dxeTreeList:TreeListTextColumn>
                        <dxeTreeList:TreeListTextColumn Caption="Call Back" FieldName="CallBack" VisibleIndex="9">
                        </dxeTreeList:TreeListTextColumn>
                        <dxeTreeList:TreeListTextColumn Caption="Convrtd" FieldName="Convrtd" VisibleIndex="10">
                        </dxeTreeList:TreeListTextColumn>
                        <dxeTreeList:TreeListTextColumn Caption="Lost" FieldName="Lost" VisibleIndex="11">
                        </dxeTreeList:TreeListTextColumn>
                        <dxeTreeList:TreeListTextColumn Caption="Non Contctble" FieldName="NonContactble" VisibleIndex="12">
                        </dxeTreeList:TreeListTextColumn>
                        <dxeTreeList:TreeListTextColumn Caption="Non -usable / Fake" FieldName="Nonusable/Fake" VisibleIndex="13">
                        </dxeTreeList:TreeListTextColumn>
                        <dxeTreeList:TreeListTextColumn Caption="Contblty. (%)" FieldName="Contblty" VisibleIndex="14">
                        </dxeTreeList:TreeListTextColumn>
                        <dxeTreeList:TreeListTextColumn Caption="Conv. (%)" FieldName="Conv" VisibleIndex="15">
                        </dxeTreeList:TreeListTextColumn>
                    </Columns>
                    <Summary>
                        <dxeTreeList:TreeListSummaryItem FieldName="Alotd" ShowInColumn="Alotd" SummaryType="Sum" DisplayFormat="{0:n0}" />
                        <dxeTreeList:TreeListSummaryItem FieldName="Contctd" ShowInColumn="Contctd" SummaryType="Sum" DisplayFormat="{0:n0}" />
                        <dxeTreeList:TreeListSummaryItem FieldName="VisitsT" ShowInColumn="VisitsT" SummaryType="Sum" DisplayFormat="{0:n0}" />
                        <dxeTreeList:TreeListSummaryItem FieldName="VisitsU" ShowInColumn="VisitsU" SummaryType="Sum" DisplayFormat="{0:n0}" />
                        <dxeTreeList:TreeListSummaryItem FieldName="CallsT" ShowInColumn="CallsT" SummaryType="Sum" DisplayFormat="{0:n0}" />
                        <dxeTreeList:TreeListSummaryItem FieldName="CallsU" ShowInColumn="CallsU" SummaryType="Sum" DisplayFormat="{0:n0}" />
                        <dxeTreeList:TreeListSummaryItem FieldName="Pending" ShowInColumn="Pending" SummaryType="Sum" DisplayFormat="{0:n0}" />
                        <dxeTreeList:TreeListSummaryItem FieldName="Refix" ShowInColumn="Refix" SummaryType="Sum" DisplayFormat="{0:n0}" />
                        <dxeTreeList:TreeListSummaryItem FieldName="CallBack" ShowInColumn="CallBack" SummaryType="Sum" DisplayFormat="{0:n0}" />
                        <dxeTreeList:TreeListSummaryItem FieldName="Convrtd" ShowInColumn="Convrtd" SummaryType="Sum" DisplayFormat="{0:n0}" />
                        <dxeTreeList:TreeListSummaryItem FieldName="Lost" ShowInColumn="Lost" SummaryType="Sum" DisplayFormat="{0:n0}" />
                        <dxeTreeList:TreeListSummaryItem FieldName="NonContactble" ShowInColumn="NonContactble" SummaryType="Sum" DisplayFormat="{0:n0}" />
                        <dxeTreeList:TreeListSummaryItem FieldName="Nonusable/Fake" ShowInColumn="Nonusable/Fake" SummaryType="Sum" DisplayFormat="{0:n0}" />
                    </Summary>
                    <Styles>
                        <FooterCell HorizontalAlign="Center">
                        </FooterCell>
                        <GroupFooterCell HorizontalAlign="Center"></GroupFooterCell>
                    </Styles>
                    <Settings SuppressOuterGridLines="True" GridLines="Both" ShowGroupFooter="True" />
                    <Images>
                        <ExpandedButton Height="11px"
                            Width="11px" />
                        <CustomizationWindowClose Height="12px" Width="13px" />
                        <CollapsedButton Height="11px" Width="11px" />
                    </Images>
                    <SettingsPager>
                        <FirstPageButton Visible="True">
                        </FirstPageButton>
                        <LastPageButton Visible="True">
                        </LastPageButton>
                    </SettingsPager>
                </dxeTreeList:ASPxTreeList>
                
            </td>
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
