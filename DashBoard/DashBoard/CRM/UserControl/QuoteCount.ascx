<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuoteCount.ascx.cs" Inherits="DashBoard.DashBoard.CRM.UserControl.QuoteCount" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<script>
    function QtCountGenerate(e) {
        cgridQt.Refresh();
        e.preventDefault();
    }

    $(document).ready(function () {
        $('a .white').click(function (e) {
            e.preventDefault();
        });

    });


</script>

<div>
    <aside class="colWraper">
        <div class="diverh">
            <table>
                <tr>
                    <td class="" colspan="5"><i class="fa fa-check-circle" style="font-size: 25px; color: #f7ffb7;"></i><span class="trSpan besidepart">Proforma Invoice/Quotation Salesman Wise</span></td>
                </tr>
                <tr>
                    <td class="pad5">


                        <dx:ASPxDateEdit ID="FromDateQt" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="cFormDateQt" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" data-toggle="tooltip" title="From Date"
                            Theme="PlasticBlue">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>


                    <td class="pad5">
                        <dx:ASPxDateEdit ID="TodateQt" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="cTodateQt" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" data-toggle="tooltip" title="To Date"
                            Theme="PlasticBlue">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>

                    <td class="pad5">

                        <dx:ASPxComboBox ID="quotecmbcalldispositions" runat="server" ValueType="System.String" data-toggle="tooltip" title="Activities Outcome"></dx:ASPxComboBox> 
                    </td>
                         
                    <td class="pad5">

                        <a href="#" data-toggle="tooltip" title="Generate" class="white">
                            <i class="fa fa-play-circle" onclick="QtCountGenerate(event)"></i>
                        </a>
                    </td>
                    <td class="pad5">
                        <asp:LinkButton ID="LinkButton1Qt" class="white" runat="server" OnClick="LinkButton1_Click" data-toggle="tooltip" title="Export to Excel">
                            <%--<i class="fa fa-file"></i>--%>
                             <img src="../../Dashboard/images/excel.png" class="excelIco" />
                        </asp:LinkButton>

                    </td>

                    <td class="pad5">
                        <span data-toggle="tooltip" title="Help">
                            <span data-toggle="popover" data-placement="left" data-html="true" id="formulapopover" data-content="This report shows the Count of activities by Salesmen with the outcome type defined as 'Pipeline/Sales Visits' in call disposition master. <br /><br /> <strong>Salesmen:</strong> Name of the user entered the Activities.<br /><br /> <strong>Count:</strong> Shows the Count  of activities by Salesmen with the outcome type defined as 'Pipeline/Sales Visits' in call disposition master. "><i class="fa fa-question-circle"></i></span>
                        </span>
                    </td>

                </tr>
            </table>


        </div>






        <dx:ASPxGridViewExporter ID="exporterQt" runat="server" Landscape="false" PaperKind="A4" GridViewID="gridQt"
            PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true" FileName="Proforma Invoice/Quotation Salesman Wise">
        </dx:ASPxGridViewExporter>

        <dx:ASPxGridView ID="gridQt" runat="server" ClientInstanceName="cgridQt" KeyFieldName="cnt_internalId"
            Width="100%" Settings-HorizontalScrollBarMode="Auto"
            SettingsBehavior-ColumnResizeMode="Control" OnDataBinding="gridPhone_DataBinding1"
            Settings-VerticalScrollableHeight="237" SettingsBehavior-AllowSelectByRowClick="true"
            Settings-VerticalScrollBarMode="Auto" Theme="PlasticBlue"
            Settings-ShowFilterRow="true" Settings-ShowFilterRowMenu="true">

            <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
            <SettingsBehavior EnableCustomizationWindow="true" />
            <Settings ShowFooter="true" />
            <SettingsContextMenu Enabled="true" />

            <Columns>

                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Salesmen" FieldName="SalesManName" Width="75%"
                     >
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Count" FieldName="quoteCount" Width="25%"
                    HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>

            </Columns>

            <SettingsPager PageSize="10" NumericButtonCount="4">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
            </SettingsPager>
        </dx:ASPxGridView>
    </aside>

</div>
