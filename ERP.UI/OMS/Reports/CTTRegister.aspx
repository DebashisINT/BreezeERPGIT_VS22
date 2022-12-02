<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.CTTRegister" CodeBehind="CTTRegister.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function ShowHeight(obj) {

            height();
            nodisplay();
        }
        function ShowHeight1(obj) {

            height();
            nodisplay1();
        }
        function height() {
            if (document.body.scrollHeight + 50 > 300)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = 300;
            window.frameElement.widht = document.body.scrollWidht;

        }
        function nodisplay() {
            document.getElementById('CTT1').style.display = 'none';
            document.getElementById('CTT').style.display = 'inline';
            dllchange();
            height();
        }
        function nodisplay1() {
            document.getElementById('CTT1').style.display = 'inline';
            document.getElementById('CTT').style.display = 'none';
            dllchange();
            height();
        }
        function dllchange() {
            var obj = document.getElementById('cmbtype').value;
            if (obj == "Reconciliation") {

                document.getElementById('tdshow').style.display = 'inline';

            }
            else {

                document.getElementById('tdshow').style.display = 'none';

            }

        }
        function gridcallback() {

            var obj = document.getElementById('cmbtype').value;
            if (obj == "Reconciliation") {
                grid1.PerformCallback();

            }
            else {
                grid.PerformCallback();

            }
            dllchange();
        }
        function ShowHideFilter(obj) {

            var objtype = document.getElementById('cmbtype').value;
            if (objtype == "Reconciliation") {
                grid1.PerformCallback(obj);
            }
            else {
                grid.PerformCallback(obj);
            }
            dllchange();
            height();
        }
        function onchangecheckbox(obj) {
            if (obj == 'on') {
                grid1.PerformCallback(obj);
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="text-align: center; vertical-align: top">
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">CTT Register</span></strong>
                </td>
            </tr>
        </table>
        <table class="TableMain100">

            <tr>

                <td class="gridcellright" style="width: 150px" id="tdsettlement" valign="top">
                    <span class="Ecoheadtxt" style="color: Blue"><strong>Date:</strong></span>
                </td>
                <td style="width: 81px; text-align: left" valign="top">
                    <dxe:ASPxDateEdit ID="cmbDate" runat="server" UseMaskBehavior="True"
                        DateOnError="Today" EditFormat="Custom" EditFormatString="dd-MM-yyyy">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                        <DropDownButton ImagePosition="Top" Text="Date" Width="30px">
                        </DropDownButton>
                    </dxe:ASPxDateEdit>
                </td>
                <td class="gridcellright" style="width: 80px" valign="top">
                    <span class="Ecoheadtxt" style="color: Blue"><strong>Type:</strong></span>
                </td>
                <td class="gridcellleft" style="width: 154px" valign="top">
                    <asp:DropDownList runat="server" Width="130px" ID="cmbtype" onchange="javascript:dllchange();gridcallback();">
                        <asp:ListItem Value="Exchange">Exchange</asp:ListItem>
                        <asp:ListItem Value="Provisional">Provisional</asp:ListItem>
                        <asp:ListItem Value="Reconciliation">Reconciliation</asp:ListItem>

                    </asp:DropDownList>
                </td>
                <td class="gridcellleft" style="width: 154px" id="tdshow" runat="server" valign="top">
                    <asp:CheckBox ID="Chkdiff" runat="server" Font-Size="X-Small" Checked="True" onClick="onchangecheckbox(this.value)" />
                    <asp:Label ID="lblcheck" runat="server" Text="Show Only Differences"></asp:Label>

                </td>
                <td>
                    <dxe:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" Text="Show">

                        <ClientSideEvents Click="function(s, e) {
                               gridcallback();
	
}"></ClientSideEvents>
                    </dxe:ASPxButton>
                </td>
            </tr>
            <tr>
                <td class="gridcellleft" style="width: 150px" valign="top" id="ShowFilter">

                    <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">Show Filter</span></a>
                    <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">All Records</span></a>

                </td>
                <td style="width: 81px; text-align: left" valign="top" id="Td1" class="gridcellleft"></td>
                <td class="gridcellright" style="width: 80px" valign="top"></td>
                <td class="gridcellleft" style="width: 154px" valign="top"></td>
                <td id="Td2" runat="server" class="gridcellleft" style="width: 154px"></td>
                <td class="gridcellleft"></td>
            </tr>
            <tr id="CTT">
                <td colspan="6">
                    <dxe:ASPxGridView ID="CTTRegisterGrid" ClientInstanceName="grid" Width="100%" runat="server"
                        KeyFieldName="CTTaxSummary_ID"
                        AutoGenerateColumns="False" DataSourceID="CTTRegisterDataSource" OnCustomCallback="CTTRegisterGrid_CustomCallback" OnCustomJSProperties="CTTRegisterGrid_CustomJSProperties" OnSummaryDisplayText="CTTRegisterGrid_SummaryDisplayText">
                        <Columns>
                            <dxe:GridViewDataTextColumn Width="40%" FieldName="name" VisibleIndex="0" Caption="Name">
                                <EditFormSettings Visible="False" />
                                <PropertiesTextEdit Width="90px">
                                </PropertiesTextEdit>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Width="15%" FieldName="CTTaxSummary_CustomerUcc" VisibleIndex="1"
                                Caption="Code">
                                <EditFormSettings Visible="False" />
                                <PropertiesTextEdit Width="90px">
                                </PropertiesTextEdit>
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Width="15%" FieldName="CTTaxSummary_TotalCTT" VisibleIndex="2"
                                Caption="Total">
                                <EditFormSettings Visible="False" />
                                <PropertiesTextEdit Width="90px" DisplayFormatString="{0:N2}">
                                </PropertiesTextEdit>
                                <CellStyle CssClass="gridcellright">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Width="15%" FieldName="CTTaxSummary_NetCTT" VisibleIndex="4"
                                Caption="Net">
                                <EditFormSettings Visible="False" />
                                <PropertiesTextEdit Width="90px" DisplayFormatString="{0:N2}">
                                </PropertiesTextEdit>
                                <CellStyle CssClass="gridcellright">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Width="15%" FieldName="CTTaxSummary_RoundOffAmount"
                                VisibleIndex="3" Caption="RoundOff">
                                <EditFormSettings Visible="False" />
                                <PropertiesTextEdit Width="90px" DisplayFormatString="{0:N2}">
                                </PropertiesTextEdit>
                                <CellStyle CssClass="gridcellright">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowFooter="True" />
                        <TotalSummary>
                            <dxe:ASPxSummaryItem FieldName="CTTaxSummary_NetCTT" ShowInColumn="Net" ShowInGroupFooterColumn="Net"
                                SummaryType="Sum" Tag="net" />
                        </TotalSummary>
                        <Templates>
                            <DetailRow>
                                <br />
                                <dxe:ASPxGridView ID="CTTDetailRegisterGrid" runat="server"
                                    Width="100%" KeyFieldName="CTTax_ID" DataSourceID="DataSourcedetailCTTGrid"
                                    OnBeforePerformDataSelect="CTTDetailRegisterGrid_DataSelect">

                                    <Columns>

                                        <dxe:GridViewDataTextColumn FieldName="CTTax_SecuritySymbol" Caption="Symbol" VisibleIndex="1">
                                            <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn FieldName="CTTax_SecuritySeries" Caption="Series" VisibleIndex="2">
                                            <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataDateColumn FieldName="CTTax_ExpiryDate" Caption="Exp.Date" VisibleIndex="3">
                                            <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                        </dxe:GridViewDataDateColumn>
                                        <dxe:GridViewDataTextColumn FieldName="CTTax_StrikePrice" Caption="Strike Price"
                                            VisibleIndex="4">
                                            <CellStyle CssClass="gridcellright">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn FieldName="CTTax_FutureSaleValue" Caption="FutureVal"
                                            VisibleIndex="5">
                                            <CellStyle CssClass="gridcellright">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn FieldName="CTTax_FutureCTT" Caption="Future CTT"
                                            VisibleIndex="6">
                                            <CellStyle CssClass="gridcellright">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn FieldName="CTTax_OptionSaleValue" Caption="Opt.Value"
                                            VisibleIndex="7">
                                            <CellStyle CssClass="gridcellright">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn FieldName="CTTax_OptionCTT" Caption="Opt.CTT"
                                            VisibleIndex="8">
                                            <CellStyle CssClass="gridcellright">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="CTTax_OptionExerciseValue" Caption="Exer.Val"
                                            VisibleIndex="10">
                                            <CellStyle CssClass="gridcellright">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn FieldName="CTTax_OptionExerciseCTT" Caption="Exer.CTT"
                                            VisibleIndex="11">
                                            <CellStyle CssClass="gridcellright">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn FieldName="CTTax_TotalCTT" Caption="Total"
                                            VisibleIndex="12">
                                            <CellStyle CssClass="gridcellright">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                    </Columns>

                                </dxe:ASPxGridView>
                            </DetailRow>
                        </Templates>
                        <SettingsDetail ShowDetailRow="True" />
                        <SettingsBehavior ConfirmDelete="True" AllowFocusedRow="True" />
                        <ClientSideEvents EndCallback="function(s, e) {
	ShowHeight(s.cpHeight);
}" />


                    </dxe:ASPxGridView>

                </td>
            </tr>
            <tr id="CTT1">
                <td colspan="6">

                    <dxe:ASPxGridView ID="grdCTTRegister" Width="100%" runat="server" KeyFieldName="provclient"
                        AutoGenerateColumns="False" ClientInstanceName="grid1" DataSourceID="CTTDataSource" OnSummaryDisplayText="CTTRegister_SummaryDisplayText" OnCustomCallback="CTTRegister_CustomCallback" OnCustomJSProperties="CTTRegister_CustomJSProperties">
                        <ClientSideEvents EndCallback="function(s, e) {
	                            ShowHeight1(s.cpHeight1);
                            }" />
                        <SettingsBehavior AllowFocusedRow="True"></SettingsBehavior>
                        <Columns>
                            <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="Name" Width="40%" Caption="Name">
                                <CellStyle CssClass="gridcellleft"></CellStyle>

                                <PropertiesTextEdit Width="90px"></PropertiesTextEdit>

                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="UCC" Width="15%" Caption="Code">
                                <CellStyle CssClass="gridcellleft"></CellStyle>

                                <PropertiesTextEdit Width="90px"></PropertiesTextEdit>

                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="ExchCTT" Width="15%" Caption="Exch CTT">
                                <CellStyle CssClass="gridcellright"></CellStyle>

                                <PropertiesTextEdit DisplayFormatString="{0:N2}" Width="90px"></PropertiesTextEdit>

                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="ProvCTT" Width="15%" Caption="Prov CTT">
                                <CellStyle CssClass="gridcellright"></CellStyle>

                                <PropertiesTextEdit DisplayFormatString="{0:N2}" Width="90px"></PropertiesTextEdit>

                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="Diffr" Width="15%" Caption="Diff">
                                <CellStyle CssClass="gridcellright"></CellStyle>

                                <PropertiesTextEdit DisplayFormatString="{0:N2}" Width="90px"></PropertiesTextEdit>

                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                        </Columns>

                        <TotalSummary>
                            <dxe:ASPxSummaryItem FieldName="ExchCTT" Tag="net" ShowInGroupFooterColumn="Exch CTT" ShowInColumn="Exch CTT" SummaryType="Sum"></dxe:ASPxSummaryItem>
                            <dxe:ASPxSummaryItem FieldName="ProvCTT" Tag="net" ShowInGroupFooterColumn="Prov CTT" ShowInColumn="Prov CTT" SummaryType="Sum"></dxe:ASPxSummaryItem>
                            <dxe:ASPxSummaryItem FieldName="Diffr" Tag="net" ShowInGroupFooterColumn="Diff" ShowInColumn="Diff" SummaryType="Sum"></dxe:ASPxSummaryItem>
                        </TotalSummary>
                        <Settings ShowFooter="True" />

                    </dxe:ASPxGridView>
                </td>
            </tr>
            <tr>
                <td><%--  <asp:SqlDataSource ID="CTTRegisterDataSource" runat="server">
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="DataSourcedetailCTTGrid" runat="server" 
                            SelectCommand="SELECT CTTax_SecuritySymbol,CTTax_SecuritySeries,CTTax_ExpiryDate,
                        (case when CTTax_StrikePrice=0.0000 then '' else convert( varchar,cast(CTTax_StrikePrice as money),1)end )as CTTax_StrikePrice,
                        (case when CTTax_FutureSaleValue=0.0000 then '' else convert(varchar,cast(CTTax_FutureSaleValue as money),1)end)as CTTax_FutureSaleValue,
                        (case when CTTax_FutureCTT=0.0000 then '' else convert(varchar,cast(CTTax_FutureCTT as money),1)end)as CTTax_FutureCTT,
                        (case when CTTax_OptionSaleValue=0.0000 then '' else convert(varchar,cast(CTTax_OptionSaleValue as money),1)end )as CTTax_OptionSaleValue,
                        (case when CTTax_OptionCTT=0.0000 then '' else convert(varchar,cast(CTTax_OptionCTT  as money),1)end )as CTTax_OptionCTT,
                        (case when CTTax_OptionExerciseValue=0.0000 then '' else convert(varchar,cast(CTTax_OptionExerciseValue as money),1) end)as CTTax_OptionExerciseValue,
                        (case when CTTax_OptionExerciseCTT=0.0000 then '' else convert(varchar,cast(CTTax_OptionExerciseCTT as money),1)end )as CTTax_OptionExerciseCTT,
                       (case when CTTax_TotalCTT=0.0000 then '' else convert(varchar,cast(CTTax_TotalCTT as money ),1)end )as CTTax_TotalCTT
                         FROM [Trans_CTTax] Where CTTax_CustomerID =(select CTTaxSummary_CustomerID from Trans_CTTaxSummary where CTTaxSummary_ID=@Id )">
                            <SelectParameters>
                                <asp:SessionParameter Name="Id" SessionField="KeyVal" Type="Int32" />
                            </SelectParameters>
                        </asp:SqlDataSource>--%>
                    <asp:SqlDataSource ID="CTTRegisterDataSource" runat="server" ></asp:SqlDataSource>
                    <asp:SqlDataSource ID="DataSourcedetailCTTGrid" runat="server" ></asp:SqlDataSource>
                    <asp:SqlDataSource ID="CTTDataSource" ConflictDetection="CompareAllValues" runat="server" ></asp:SqlDataSource>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

