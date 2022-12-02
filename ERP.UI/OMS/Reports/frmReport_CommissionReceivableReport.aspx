<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_CommissionReceivableReport" Codebehind="frmReport_CommissionReceivableReport.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" language="javascript">
        function OnDetailsClick(CompID, CompName) {
            var Fdate = (dtFromDate.GetDate().getMonth() + 1) + '/' + dtFromDate.GetDate().getDate() + '/' + dtFromDate.GetDate().getYear();
            var Tdate = (dtToDate.GetDate().getMonth() + 1) + '/' + dtToDate.GetDate().getDate() + '/' + dtToDate.GetDate().getYear();

            var url = "frmReport_CommissionReceivableReport_internal.aspx?CompID=" + CompID + "&CompName=" + CompName + "&Fdate=" + Fdate + "&Tdate=" + Tdate + "&Type=Prod";
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
        function OpenPrintPage() {
            if (dtFromDate.GetDate() == null)
                alert('Please Select from date!');
            else {
                if (dtToDate.GetDate() == null)
                    alert('Please Select To date!');
                else {
                    var Fdate = (dtFromDate.GetDate().getMonth() + 1) + '/' + dtFromDate.GetDate().getDate() + '/' + dtFromDate.GetDate().getYear();
                    var Tdate = (dtToDate.GetDate().getMonth() + 1) + '/' + dtToDate.GetDate().getDate() + '/' + dtToDate.GetDate().getYear();

                    var url = "frmReport_CommissionReceivableReport_print.aspx?Fdate=" + Fdate + "&Tdate=" + Tdate;
                    popup.SetContentUrl(url);
                    popup.Show();
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <table class="TableMain100">
            <tr>
                <td class="gridcellleft">
                    <table class="TableMain100">
                        <tr>
                            <td>
                                <table cellspacing="0px">
                                    <tr>
                                        <td class="gridcellleft">
                                            <dxe:ASPxDateEdit ID="dtFromDate" ClientInstanceName="dtFromDate" runat="server"
                                                EditFormat="Custom" UseMaskBehavior="True" TabIndex="1" Width="135px">
                                                <DropDownButton Text="From ">
                                                </DropDownButton>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td class="gridcellleft">
                                            <dxe:ASPxDateEdit ID="dtToDate" ClientInstanceName="dtToDate" runat="server" EditFormat="Custom"
                                                UseMaskBehavior="True" TabIndex="2" Width="135px">
                                                <DropDownButton Text="To">
                                                </DropDownButton>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td class="gridcellright">
                                            <input id="btnShow" type="button" value="Show" onclick="btnShow_Click()" class="btnUpdate" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <a id="Print" href="javascript:OpenPrintPage()"><span style="color: #3300cc; text-decoration: underline;
                                    cursor: pointer;">Print Preview</span></a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="gridcellcenter">
                    <dxe:ASPxGridView ID="grdInsuranceCommission" ClientInstanceName="grid" runat="server"
                        KeyFieldName="companiID" AutoGenerateColumns="False" OnCustomCallback="grdInsuranceCommission_CustomCallback"
                        OnCustomJSProperties="grdInsuranceCommission_CustomJSProperties">
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
                                VisibleIndex="3">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <PropertiesTextEdit DisplayFormatString="{0:n2}">
                                </PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Trail Commission" FieldName="TrailCommission"
                                VisibleIndex="3">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <PropertiesTextEdit DisplayFormatString="{0:n2}">
                                </PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Received Commission" FieldName="RecCommission"
                                VisibleIndex="9">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <PropertiesTextEdit DisplayFormatString="{0:n2}">
                                </PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="10">
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
                        <Settings ShowTitlePanel="True" />
                        <ClientSideEvents EndCallback="function(s, e) {
	FireHeight(grid.cpHeight);
}" />
                    </dxe:ASPxGridView>
                    <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ContentUrl="" CloseAction="CloseButton"
                        Left="10" ClientInstanceName="popup" Height="400px" Width="950px" HeaderText="Commission Details"
                        AllowDragging="True" AllowResize="True" AutoUpdatePosition="True" EnableViewState="False"
                        RenderIFrameForPopupElements="True">
                        <ContentCollection>
                            <dxe:PopupControlContentControl runat="server">
                            </dxe:PopupControlContentControl>
                        </ContentCollection>
                    </dxe:ASPxPopupControl>
                </td>
            </tr>
        </table>
</asp:Content>
