<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SmenOrdCount.ascx.cs" Inherits="DashBoard.DashBoard.CRM.UserControl.SmenOrdCount" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<script>
    function OCountCountGenerate(e) {
        cgridOCount.Refresh();
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
                    <td class="" colspan="5"><i class="fa fa-check-circle" style="font-size: 25px; color: #f7ffb7;"></i><span class="trSpan besidepart">Salesman Wise Order Count</span></td>
                </tr>
                <tr>
                    <td class="pad5">


                        <dx:ASPxDateEdit ID="FromDateOCount" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="cFormDateOCount" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" data-toggle="tooltip" title="From Date"
                            Theme="PlasticBlue">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>


                    <td class="pad5">
                        <dx:ASPxDateEdit ID="TodateOCount" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="cTodateOCount" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" data-toggle="tooltip" title="To Date"
                            Theme="PlasticBlue">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>

                   


                    <td class="pad5">

                        <a href="#" data-toggle="tooltip" title="Generate" class="white">
                            <i class="fa fa-play-circle" onclick="OCountCountGenerate(event)"></i>
                        </a>
                    </td>
                    <td class="pad5">
                        <asp:LinkButton ID="LinkButton1sv" class="white" runat="server" OnClick="LinkButton1_Click" data-toggle="tooltip" title="Export to Excel">
                            <%--<i class="fa fa-file"></i>--%>
                             <img src="../../Dashboard/images/excel.png" class="excelIco" />
                        </asp:LinkButton>

                    </td>

                    <td class="pad5">
                        <span data-toggle="tooltip" title="Help">
                            <span data-toggle="popover" data-placement="left" data-html="true" data-content="This report shows the Count of Total Sale Order(s) which are Invoiced and is based on unique Customer in the given period. <br /><br /> <strong>Salesmen:</strong> Name of the person done the entries of Sales Order(s). <br /><br /><strong>Invoiced Order(s):</strong> Shows the count of total Sale Order(s) which are already Invoiced in the given period. <br /><br />"><i class="fa fa-question-circle"></i></span>
                        </span>
                    </td>


                </tr>
            </table>


        </div>






        <dx:ASPxGridViewExporter ID="exporterOCount" runat="server" Landscape="false" PaperKind="A4" GridViewID="gridOCount"
            PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true" FileName="Sales Visit Count">
        </dx:ASPxGridViewExporter>

        <dx:ASPxGridView ID="gridOCount" runat="server" ClientInstanceName="cgridOCount" KeyFieldName="cnt_internalId"
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

                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Salesmen" FieldName="SalesManName" Width="75%">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Invoiced Order(s)" FieldName="TotCount" Width="25%"
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
