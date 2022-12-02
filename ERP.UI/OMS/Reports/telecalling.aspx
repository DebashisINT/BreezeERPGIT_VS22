<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_telecalling" CodeBehind="telecalling.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v10.2, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>--%>

    <%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
        Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

    <%--<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>--%>
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />
    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>
    <script language="javascript" type="text/javascript">
        function SignOff() {
            window.parent.SignOff()
        }
        function height() {

            if (document.body.scrollHeight >= 500) {
                window.frameElement.height = document.body.scrollHeight;
            }
            else {
                window.frameElement.height = '500px';
            }
            window.frameElement.widht = document.body.scrollWidht;

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="TableMain100">
        <tr>
            <td>
                <asp:Panel ID="panel" BorderColor="blue" BorderWidth="0" runat="server" Width="100%">
                    <table class="TableMain100">
                        <tr>
                            <td class="EHEADER" style="text-align: center;">
                                <strong><span style="color: #000099">Call Outcome Wise Report</span></strong>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left">
                                <table>
                                    <tr>

                                        <td style="text-align: right">
                                            <dxe:ASPxComboBox ID="cmbCallType" runat="server" ValueType="System.String" Font-Size="12px" Width="170px" SelectedIndex="0" OnSelectedIndexChanged="ASPxComboBox1_SelectedIndexChanged" Height="1px" AutoPostBack="True">
                                                <Items>
                                                    <dxe:ListEditItem Text="Call Back" Value="1" />
                                                    <dxe:ListEditItem Text="Not Reachable" Value="2" />
                                                    <dxe:ListEditItem Text="No Response" Value="3" />
                                                </Items>
                                                <ButtonStyle Width="13px" Wrap="False">
                                                </ButtonStyle>
                                                <DropDownButton Text="Call Type" Width="60px">
                                                </DropDownButton>
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td>
                                            <dxe:ASPxRadioButtonList ID="RBReportType" runat="server" ItemSpacing="20px" RepeatDirection="Horizontal" Font-Size="12px" Height="2px" SelectedIndex="0" CssPostfix="BlackGlass">
                                                <Items>
                                                    <dxe:ListEditItem Text="Screen" Value="Screen" />
                                                    <dxe:ListEditItem Text="Print" Value="Print" />
                                                </Items>
                                                <DisabledStyle>
                                                    <Border BorderWidth="0px" />
                                                    <BorderBottom BorderWidth="0px" />
                                                </DisabledStyle>
                                            </dxe:ASPxRadioButtonList>
                                        </td>
                                        <td>
                                            <dxe:ASPxButton ID="btnShowReport" runat="server" Text="Show" Height="5px" OnClick="btnShowReport_Click" Width="82px">
                                            </dxe:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>

                        </tr>
                        <tr>
                            <td>
                                <dxe:aspxgridview id="gridCallOutcome" runat="server" font-size="10px" width="100%" autogeneratecolumns="False">
                                    <Styles>
                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                        </Header>
                                        <LoadingPanel ImageSpacing="10px">
                                        </LoadingPanel>
                                    </Styles>
                                       <SettingsBehavior ColumnResizeMode="NextColumn" />
                                    <SettingsPager AlwaysShowPager="True" PageSize="20" ShowSeparators="True">
                                        <FirstPageButton Visible="True">
                                        </FirstPageButton>
                                        <LastPageButton Visible="True">
                                        </LastPageButton>
                                    </SettingsPager>
                                    <Settings ShowGroupPanel="True" />
                                    <Columns>
                                        <dxe:GridViewDataTextColumn Caption="Lead  Name" FieldName="Lead  Name" VisibleIndex="0">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="First Attempt" FieldName="First Attempt" VisibleIndex="1">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Second Attempt" FieldName="Second Attempt" VisibleIndex="2">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Third Attempt" FieldName="Third Attempt" VisibleIndex="3">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Forth Attempt" FieldName="Forth Attempt" VisibleIndex="4">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Fifth Attempt" FieldName="Fifth Attempt" VisibleIndex="5">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Sixth Attempt" FieldName="Sixth Attempt" VisibleIndex="6">
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                </dxe:aspxgridview>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
