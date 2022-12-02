<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_CommissionReceivableReport_internal" Codebehind="frmReport_CommissionReceivableReport_internal.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" language="javascript">
        function OnDetailsClick(CompID, CompName) {
            var Fdate = document.getElementById("hdFdate").value;
            var Tdate = document.getElementById("hdTdate").value;
            var CompIDL = document.getElementById("hdCompany").value;
            var url = "frmReport_CommissionReceivableReport_internal.aspx?ProdID=" + CompID + "&CompID=" + CompIDL + "&CompName=" + CompName + "&Fdate=" + Fdate + "&Tdate=" + Tdate + "&Type=Client";
            //alert(url);
            popup.SetContentUrl(url);
            popup.Show();
        }
        function height() {
            if (document.body.scrollHeight <= 500)
                window.frameElement.height = '650px';
            else
                window.frameElement.height = document.body.scrollHeight;
            window.frameElement.Width = document.body.scrollWidth;
        }
        function FireHeight(obj) {
            height();
        }
        function btnShow_Click() {
            grid.PerformCallback();
        }
        function CloseWindow() {
            parent.popup.Hide();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <table class="TableMain10">
            <tr>
                <td>
                    <table class="TableMain10">
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCompany" runat="server" ForeColor="#0000C0"></asp:Label>
                                        </td>
                                        <td class="gridcellleft">
                                            From date:&nbsp;<asp:Label ID="lblFromDate" runat="server" ForeColor="#0000C0"></asp:Label>
                                        </td>
                                        <td class="gridcellleft">
                                            To date:&nbsp;<asp:Label ID="lblToDate" runat="server" ForeColor="#0000C0"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="text-align: right">
                                <table>
                                    <tr>
                                        <td>
                                            <a id="refresh" href="javascript:location.reload(true)"><span style="color: #3300cc; text-decoration: underline;
                                                cursor: pointer;">Refresh</span></a>&nbsp;|&nbsp;
                                        </td>
                                        <td>
                                            <a id="WinClose" onclick="CloseWindow();"><span style="color: #3300cc; text-decoration: underline;
                                                cursor: pointer;">Close Window</span></a>
                                        </td>
                                    </tr>
                                </table>
                                
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxGridView ID="grdInsuranceCommission" runat="server" ClientInstanceName="grid"
                        KeyFieldName="ID" AutoGenerateColumns="False" OnCustomCallback="grdInsuranceCommission_CustomCallback">
                        <Styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <FocusedRow BackColor="#FEC6AB">
                            </FocusedRow>
                        </Styles>
                        <Columns>
                            <dxe:GridViewDataTextColumn Caption="Company" FieldName="company" VisibleIndex="0">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Total Premium" FieldName="PayedAmt" VisibleIndex="1">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <PropertiesTextEdit DisplayFormatString="{0:n2}">
                                </PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Base Commission" FieldName="BaseCommission"
                                VisibleIndex="2">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <PropertiesTextEdit DisplayFormatString="{0:n2}">
                                </PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="ORC Commission" FieldName="ORCCommission"
                                VisibleIndex="3">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <PropertiesTextEdit DisplayFormatString="{0:n2}">
                                </PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="TopUp Commission" FieldName="TopUpCommission"
                                VisibleIndex="4">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <PropertiesTextEdit DisplayFormatString="{0:n2}">
                                </PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Trail Commission" FieldName="TrailCommission"
                                VisibleIndex="5">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <PropertiesTextEdit DisplayFormatString="{0:n2}">
                                </PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            
                            <dxe:GridViewDataTextColumn Caption="Received Commission" FieldName="RecCommission"
                                VisibleIndex="6">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <PropertiesTextEdit DisplayFormatString="{0:n2}">
                                </PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="7">
                                <DataItemTemplate>
                                    <a href="javascript:void(0);" onclick="OnDetailsClick('<%# Container.KeyValue %>','<%#Eval("company") %>')">
                                        <u>Details</u> </a>
                                </DataItemTemplate>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <StylesEditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </StylesEditors>
                        <SettingsBehavior AllowFocusedRow="True" AllowSort="False" AllowMultiSelection="True" />
                        <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <SettingsText Title="Commission Receivable" />
                        <Settings ShowTitlePanel="false" />
                    </dxe:ASPxGridView>
                    <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ContentUrl="" CloseAction="CloseButton"
                        Left="0" ClientInstanceName="popup" Height="300px" Width="800px" HeaderText="Commission Details"
                        AllowDragging="True" AllowResize="True" EnableViewState="False" DragElement="Window">
                        <ContentCollection>
                            <dxe:PopupControlContentControl runat="server">
                            </dxe:PopupControlContentControl>
                        </ContentCollection>
                    </dxe:ASPxPopupControl>
                    <asp:HiddenField ID="hdFdate" runat="server" />
                    <asp:HiddenField ID="hdTdate" runat="server" />
                    <asp:HiddenField ID="hdCompany" runat="server" />
                    <asp:HiddenField ID="hdType" runat="server" />
                </td>
            </tr>
        </table>
</asp:Content>
