<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PendingAct.ascx.cs" Inherits="DashBoard.DashBoard.CRM.UserControl.PendingAct" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<script src="../CRM/Js/PendingAct.js"></script>
<script>

    function GenerateChartPendingAct() {
        if (cgridpact.GetVisibleRowsOnPage() == 0) {
            showCustomNewNotify('You must generate the report first to view the chart.');
            return;
        }


        $('#pendActModel').modal('show');
        var OtherDetails = {}
        OtherDetails.toDate = moment(cFormDatePAct.GetDate()).format('YYYY-MM-DD');
        OtherDetails.ActType = $('#dpActivitylistPAct').val();


        $.ajax({
            type: "POST",
            url: "../Service/General.asmx/GetPendingAct",
            data: JSON.stringify(OtherDetails),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var retObj = msg.d;


                var PieObject = {
                    "theme": "light",
                    "type": "serial",
                    "startDuration": 2,
                    "dataProvider": [],
                    "valueAxes": [{
                        "position": "left",
                        "title": "Efficiency"
                    }],
                    "graphs": [{
                        "balloonText": "[[category]]: <b>[[value]]</b>",
                        "fillColorsField": "color",
                        "fillAlphas": 1,
                        "lineAlpha": 0.1,
                        //"labelText": "[[category]]", 
                        "type": "column",
                        "valueField": "EF"
                    }],
                    "depth3D": 20,
                    "angle": 30,
                    "chartCursor": {
                        "categoryBalloonEnabled": false,
                        "cursorAlpha": 0,
                        "zoomable": false
                    },
                    "categoryField": "SManName",
                    "categoryAxis": {
                        "gridPosition": "start",
                        "labelRotation": 45
                    },
                    "export": {
                        "enabled": true
                    }

                };
                PieObject.dataProvider = retObj;
                var chart = AmCharts.makeChart("pendActChart", PieObject);



            }
        });
    }
</script>

<div>
    <aside class="colWraper">
        <table style="width: 100%">
            <tr>
                <td class="ChartLblClass">Click to view Pending Activities Chart. <i class="fa fa-arrow-down shakeAlways"></i></td>
            </tr>
        </table>
        <div class="diverh col-md-10">

            <table>
                <tr>
                    <td class="" style="width: 190px;"><i class="fa fa-bell" style="font-size: 25px; color: #f7ffb7;"></i><span class="trSpan besidepart">(Pending Activities)</span></td>
                </tr>
                <tr>
                    <td class="pad5">


                        <dx:ASPxDateEdit ID="FromDatePAct" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="cFormDatePAct" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" data-toggle="tooltip" title="As on Date"
                            Theme="PlasticBlue">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>

                    <td class="pad5">
                        <asp:DropDownList ID="dpActivitylistPAct" runat="server" Width="100%" Style="margin-top: 5px;" data-toggle="tooltip" title="Activity Status">
                            <asp:ListItem Text="All" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Future Sale" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Clarification Required" Value="5"></asp:ListItem>
                            <asp:ListItem Text="Document Collection" Value="1"></asp:ListItem>

                        </asp:DropDownList>
                    </td>



                    <td class="pad5">

                        <a href="#" data-toggle="tooltip" title="Generate" class="white">
                            <i class="fa fa-play-circle" onclick="PactCountGenerate(event)"></i>
                        </a>
                    </td>
                    <td class="pad5">
                        <asp:LinkButton ID="LinkButton1pact" class="white" runat="server" OnClick="LinkButton1_Click" data-toggle="tooltip" title="Export to Excel">
                            <%--<i class="fa fa-file"></i>--%>
                             <img src="../../Dashboard/images/excel.png" class="excelIco" />
                        </asp:LinkButton>

                    </td>

                    <td class="pad5">
                        <span data-toggle="tooltip" title="Help">
                            <span data-toggle="popover" data-placement="left" data-html="true" data-content="This report shows the Count of Total Pending Activities. Pending Activities means those activities which are having 'Next Activity Date' <= 'As On Date'. <br /><br /> <strong>Salesmen:</strong> Shows the list of salesmen not completed task for next activity date.<br /><br /><strong>Activities:</strong> Shows the count of total pending task for the assigned salesmen in the given period."><i class="fa fa-question-circle"></i></span>
                        </span>
                    </td>

                </tr>
            </table>


        </div>
        <div class="col-md-2">
            <img src="../../Dashboard/images/chart.png" onclick="GenerateChartPendingAct()" style="border: .5px solid #675a5a;" class="zoomImg" width="100%" height="87px" />
            <%--<span > Click To view Chart</span>--%>
        </div>






        <dx:ASPxGridViewExporter ID="exporterpact" runat="server" Landscape="false" PaperKind="A4" GridViewID="gridpact"
            PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true" FileName="Sales Visit Count">
        </dx:ASPxGridViewExporter>

        <dx:ASPxGridView ID="gridpact" runat="server" ClientInstanceName="cgridpact" KeyFieldName="cnt_internalId"
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

                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Activities" FieldName="ActCount" Width="25%"
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

    <div class="modal fade" id="pendActModel" role="dialog">
        <div class="modal-dialog">

            <div class="modal-content">
                <%--<div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title">Efficiency Ratio Salesmen wise</h4>
            </div>--%>
                <div class="modal-body">
                    <div>
                        <table width="100%">
                            <tr>
                                <td width="90%" class="headerTdClass">Pending Activity Salesmen wise</td>
                                <td width="10%" style="text-align: right;"><span data-toggle="tooltip" title="Click to close the chart." data-dismiss="modal" style="cursor: pointer"><i class="fa fa-times closePopup"></i></span></td>
                            </tr>
                        </table>
                    </div>
                    <div id="pendActChart" style="height: 500px"></div>
                    <div class="FooterTdClass">Salesmen</div>
                </div>
                <%-- <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
            </div>--%>
            </div>

        </div>
    </div>





</div>
