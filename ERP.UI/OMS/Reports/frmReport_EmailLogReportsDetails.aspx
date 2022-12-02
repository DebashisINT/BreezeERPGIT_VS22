<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_EmailLogReportsDetails" Codebehind="frmReport_EmailLogReportsDetails.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>
            <table class="TableMain100" style="border: solid 1px white;" cellpadding="2" cellspacing="3">
                <tr>
                    <td align="center">
                        <table width="100%" style="border: solid 1px white;">
                            <tr>
                                <td style="font-size: 12px; font-weight: bold;" width="20px">
                                    <asp:Label ID="lblType" runat="server"></asp:Label>:</td>
                                <td align="left">
                                    <asp:Label ID="lblName" runat="server"></asp:Label><asp:Label ID="lblEmail" runat="server"></asp:Label><br />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table width="100%" style="border: solid 1px white;">
                            <tr>
                                <td style="font-size: 12px; font-weight: bold;" width="20px">
                                    Subject:</td>
                                <td align="left" >
                                    <asp:Label ID="lblSub" runat="server"></asp:Label><br />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table width="100%" style="border: solid 1px white;">
                            <tr>
                                <td style="font-size: 12px; font-weight: bold;" width="20px" valign="top">
                                    Content:</td>
                                <td align="left" style="background-color:White;">
                                    <asp:Label ID="lblContent" runat="server"></asp:Label></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="font-size: 12px; font-weight: bold;" valign="top">
                        Attachments:
                    </td>
                </tr>
                <tr>
                    <tr>
                        <td align="center">
                            <dxe:ASPxGridView ID="AttachGrid" runat="server" AutoGenerateColumns="False" KeyFieldName="EmailAttachment_ID"
                                Width="100%" ClientInstanceName="grid">
                                <Settings />
                                <Columns>
                                    <dxe:GridViewDataTextColumn FieldName="EmailAttachment_Path" Caption="Attachments"
                                        ReadOnly="True" VisibleIndex="0">
                                        <CellStyle HorizontalAlign="Left">
                                        </CellStyle>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="ADD/Modify " FieldName="formID" ReadOnly="True"
                                        VisibleIndex="1">
                                        <DataItemTemplate>
                                            <a href='ViewAttachment.aspx?id=<%#Eval("EmailAttachment_Path") %>' target="_blank">
                                                View</a>
                                        </DataItemTemplate>
                                        <EditFormSettings Visible="False" />
                                        <CellStyle HorizontalAlign="Left">
                                        </CellStyle>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <HeaderTemplate>
                                            View
                                        </HeaderTemplate>
                                    </dxe:GridViewDataTextColumn>
                                </Columns>
                                <SettingsBehavior AllowFocusedRow="True" ColumnResizeMode="NextColumn" />
                                <Styles>
                                    <LoadingPanel ImageSpacing="10px">
                                    </LoadingPanel>
                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                    </Header>
                                    <FocusedGroupRow CssClass="gridselectrow">
                                    </FocusedGroupRow>
                                    <FocusedRow CssClass="gridselectrow">
                                    </FocusedRow>
                                </Styles>
                                <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                                    <FirstPageButton Visible="True">
                                    </FirstPageButton>
                                    <LastPageButton Visible="True">
                                    </LastPageButton>
                                </SettingsPager>
                            </dxe:ASPxGridView>
                            <%-- <asp:Label ID="lblAtt" runat="server"></asp:Label>--%>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-size: 12px; font-weight: bold;" valign="top">
                            Email Delivery Log:
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="height: 20px">
                            <dxe:ASPxGridView ID="logGrid" runat="server" AutoGenerateColumns="False" KeyFieldName="EmailLog_ID"
                                Width="100%" ClientInstanceName="grid">
                                <Columns>
                                    <dxe:GridViewDataTextColumn FieldName="EmailLog_SentDateTime" Caption="Sent DateTime"
                                        ReadOnly="True" Width="110px" VisibleIndex="0">
                                        <CellStyle HorizontalAlign="Left">
                                        </CellStyle>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="EmailLog_DeliveryStatus" Caption="Status"
                                        Width="60px" ReadOnly="True" VisibleIndex="1">
                                        <CellStyle HorizontalAlign="Left">
                                        </CellStyle>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="EmailLog_Reason" Caption="Reason" VisibleIndex="2">
                                        <CellStyle HorizontalAlign="Left" Wrap="true">
                                        </CellStyle>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </dxe:GridViewDataTextColumn>
                                </Columns>
                                <SettingsBehavior AllowFocusedRow="True" ColumnResizeMode="NextColumn" />
                                <Styles>
                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                    </Header>
                                    <FocusedGroupRow CssClass="gridselectrow">
                                    </FocusedGroupRow>
                                    <FocusedRow CssClass="gridselectrow">
                                    </FocusedRow>
                                    <LoadingPanel ImageSpacing="10px">
                                    </LoadingPanel>
                                </Styles>
                                <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                                    <FirstPageButton Visible="True">
                                    </FirstPageButton>
                                    <LastPageButton Visible="True">
                                    </LastPageButton>
                                </SettingsPager>
                                <Settings />
                            </dxe:ASPxGridView>
                            <%-- <asp:Label ID="lblLog" runat="server"></asp:Label>--%>
                        </td>
                    </tr>
            </table>
        </div>
</asp:Content>
