<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.DailyTask.management_DailyTask_frm_AccountTransfer_PopUp" CodeBehind="frm_AccountTransfer_PopUp.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>--%>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        &nbsp;<dxe:ASPxGridView ID="gridHolding" runat="server" AutoGenerateColumns="False"
            ClientInstanceName="grid" KeyFieldName="NsdlHolding_ISIN" OnCustomCallback="gridHolding_CustomCallback"
            Width="100%">
            <SettingsBehavior AllowFocusedRow="True" />
            <Styles>
                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                </Header>
                <LoadingPanel ImageSpacing="10px">
                </LoadingPanel>
            </Styles>
            <SettingsPager PageSize="20">
            </SettingsPager>
            <Columns>
                <dxe:GridViewDataTextColumn Caption="ISIN No." FieldName="NsdlHolding_ISIN" VisibleIndex="0">
                    <Settings AutoFilterCondition="Contains" FilterMode="DisplayText" />
                    <CellStyle CssClass="gridcellleft">
                    </CellStyle>
                    <FooterTemplate>
                        Total Holding Value
                    </FooterTemplate>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="ISIN Name" FieldName="CmpName" VisibleIndex="1">
                    <Settings AutoFilterCondition="Contains" FilterMode="DisplayText" />
                    <CellStyle CssClass="gridcellleft">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="SettlementID" FieldName="NsdlHolding_SettlementNumber"
                    VisibleIndex="2">
                    <Settings AutoFilterCondition="Contains" FilterMode="DisplayText" />
                    <CellStyle CssClass="gridcellleft">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Type" FieldName="Type" VisibleIndex="3">
                    <Settings AutoFilterCondition="Contains" FilterMode="DisplayText" />
                    <CellStyle CssClass="gridcellleft">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Current Balance" FieldName="Total" UnboundType="Integer"
                    VisibleIndex="4">
                    <Settings AllowAutoFilter="False" />
                    <CellStyle CssClass="gridcellleft" HorizontalAlign="Right">
                    </CellStyle>
                    <PropertiesTextEdit DisplayFormatString="#.###">
                    </PropertiesTextEdit>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Free" FieldName="Free" UnboundType="Integer"
                    VisibleIndex="5">
                    <Settings AllowAutoFilter="False" />
                    <CellStyle CssClass="gridcellleft" HorizontalAlign="Right">
                    </CellStyle>
                    <PropertiesTextEdit DisplayFormatString="#.###">
                    </PropertiesTextEdit>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Pledged" FieldName="Pledged" UnboundType="Integer"
                    VisibleIndex="6">
                    <PropertiesTextEdit DisplayFormatString="#.###">
                    </PropertiesTextEdit>
                    <Settings AllowAutoFilter="False" />
                    <CellStyle CssClass="gridcellleft" HorizontalAlign="Right">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Pending Remat" FieldName="Remat" UnboundType="Integer"
                    VisibleIndex="7">
                    <Settings AllowAutoFilter="False" />
                    <CellStyle CssClass="gridcellleft" HorizontalAlign="Right">
                    </CellStyle>
                    <PropertiesTextEdit DisplayFormatString="#.###">
                    </PropertiesTextEdit>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Pending Demat" FieldName="Demat" UnboundType="Integer"
                    VisibleIndex="8">
                    <Settings AllowAutoFilter="False" />
                    <CellStyle CssClass="gridcellleft" HorizontalAlign="Right">
                    </CellStyle>
                    <PropertiesTextEdit DisplayFormatString="#.###">
                    </PropertiesTextEdit>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="Rate" VisibleIndex="9">
                    <Settings AllowAutoFilter="False" />
                    <CellStyle CssClass="gridcellleft" HorizontalAlign="Right">
                    </CellStyle>
                    <PropertiesTextEdit DisplayFormatString="0.0000">
                    </PropertiesTextEdit>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Value" FieldName="ISINValue" VisibleIndex="10">
                    <Settings AllowAutoFilter="False" />
                    <CellStyle CssClass="gridcellleft" HorizontalAlign="Right">
                    </CellStyle>
                    <PropertiesTextEdit DisplayFormatString="0.00">
                    </PropertiesTextEdit>
                </dxe:GridViewDataTextColumn>
            </Columns>
            <Settings ShowFooter="True" ShowStatusBar="Visible" ShowTitlePanel="True" />
            <StylesEditors>
                <ProgressBar Height="25px">
                </ProgressBar>
            </StylesEditors>
            <TotalSummary>
                <dxe:ASPxSummaryItem DisplayFormat="#,##,###.00" FieldName="ISINValue" ShowInColumn="Value"
                    ShowInGroupFooterColumn="Value" SummaryType="Sum" Tag="Total Holding Value" />
            </TotalSummary>
        </dxe:ASPxGridView>
    </div>
</asp:Content>
