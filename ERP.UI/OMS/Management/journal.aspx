<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.management_journal" CodeBehind="journal.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>--%>
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script language="javascript" type="text/javascript">
        function EndCall(obj) {
            if (obj == "3")
                alert('Delete Successfully');
            height();
        }
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
        function OnMoreInfoClick(KeyValue, exch, date) {
            <%Session["cashJournal"] = "2";%>
            var url = "journalPopup.aspx?id=" + KeyValue + "&exch=" + exch + "&date=" + date;
            OnMoreInfoClick(url, "Modify Journal Entries", '940px', '450px', "Y");

        }
        function OnAddButtonClick() {
            <%Session["cashJournal"] = "2";%>
            var url = 'journalPopup.aspx';
            OnMoreInfoClick(url, "Add Journal Entries", '985px', '450px', "Y");

        }
        function RefreshGrid() {
            editwin.close();
        }
        FieldName = 'ctl00_ContentPlaceHolder1_Headermain1_Menumain_SkipLink';
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function callback() {
            grid.PerformCallback();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="TableMain100">
        <tr>
            <td style="text-align: left; vertical-align: top">
                <table>
                    <tr>
                        <td id="ShowFilter">
                            <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">Show Filter</span></a>
                        </td>
                        <td id="Td1">
                            <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">All Records</span></a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <dxe:ASPxGridView ID="gridJournal" ClientInstanceName="grid" KeyFieldName="JournalVoucher_ID" DataSourceID="SqlJournal" Width="100%" runat="server" OnCustomCallback="gridJournal_CustomCallback" AutoGenerateColumns="False" OnRowDeleting="gridJournal_RowDeleting" OnCustomJSProperties="gridJournal_CustomJSProperties">
                    <Styles>
                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                        </Header>
                        <LoadingPanel ImageSpacing="10px">
                        </LoadingPanel>
                    </Styles>
                    <StylesEditors>
                        <ProgressBar Height="25px">
                        </ProgressBar>
                    </StylesEditors>
                    <Columns>
                        <dxe:GridViewDataTextColumn Caption="Voucher Number" Visible="False" FieldName="JournalVoucher_ID"
                            VisibleIndex="0">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" VisibleIndex="0" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Voucher Number" FieldName="VoucherNumber"
                            VisibleIndex="0">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" VisibleIndex="0" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Transaction Date" FieldName="TDate"
                            VisibleIndex="1">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" VisibleIndex="0" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Segment" FieldName="Segment"
                            VisibleIndex="2">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" VisibleIndex="0" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Credit" FieldName="JournalVoucher_ExchangeSegmentID" Visible="False" VisibleIndex="4">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" VisibleIndex="0" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Credit" FieldName="JournalVoucher_TransactionDate" Visible="False" VisibleIndex="5">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" VisibleIndex="0" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="BID" FieldName="JournalVoucher_BranchID" Visible="False" VisibleIndex="6">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" VisibleIndex="0" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="CompID" FieldName="JournalVoucher_CompanyID" Visible="False" VisibleIndex="7">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right" Wrap="False">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" VisibleIndex="0" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Width="5%" Caption="Details" VisibleIndex="3">
                            <DataItemTemplate>
                                <a href="#" onclick="OnMoreInfoClick('<%#Eval("VoucherNumber") %>','<%#Eval("JournalVoucher_ExchangeSegmentID") %>','<%#Eval("JournalVoucher_TransactionDate") %>')">Edit</a>
                            </DataItemTemplate>
                            <HeaderTemplate>
                                <a href="javascript:void(0);" onclick="OnAddButtonClick()"><span style="color: #000099; text-decoration: underline">Add New</span></a>
                            </HeaderTemplate>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewCommandColumn VisibleIndex="4" Width="5%" ShowDeleteButton="True"/>
                    </Columns>
                    <Settings ShowGroupPanel="True" />
                    <SettingsText PopupEditFormCaption="Add/Modify Cash/Bank" ConfirmDelete="Are you sure to Delete this Record!" />
                    <SettingsPager NumericButtonCount="20" Visible="False" ShowSeparators="True">
                        <FirstPageButton Visible="True">
                        </FirstPageButton>
                        <LastPageButton Visible="True">
                        </LastPageButton>
                    </SettingsPager>
                    <SettingsBehavior ConfirmDelete="True" AllowFocusedRow="True" />
                    <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center" PopupEditFormHeight="600px"
                        PopupEditFormVerticalAlign="TopSides" PopupEditFormWidth="900px" />
                    <ClientSideEvents EndCallback="function(s, e) {
	 EndCall(s.cpEND);
}" />
                </dxe:ASPxGridView>
            </td>
        </tr>
    </table>
    <asp:SqlDataSource ID="SqlJournal" runat="server" ConflictDetection="CompareAllValues" 
        SelectCommand=""></asp:SqlDataSource>
</asp:Content>
