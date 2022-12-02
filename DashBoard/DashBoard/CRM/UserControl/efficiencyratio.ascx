<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="efficiencyratio.ascx.cs" Inherits="DashBoard.DashBoard.CRM.UserControl.efficiencyratio" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<script>
    function EFGenerate(e) {
        cgridEF.Refresh();
        e.preventDefault();
    }

    function GenerateChartEfficency() {
        if (cgridEF.GetVisibleRowsOnPage() == 0) {
            showCustomNewNotify('You must generate the report first to view the chart.');
            return;
        }


        $('#EfModel').modal('show');
        var OtherDetails = {}
        OtherDetails.FromDtae = moment(cFormDateEF.GetDate()).format('YYYY-MM-DD');
        OtherDetails.toDate = moment(cTodateEF.GetDate()).format('YYYY-MM-DD');
        $.ajax({
            type: "POST",
            url: "../Service/General.asmx/GetEfficency",
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
                var chart = AmCharts.makeChart("EFChart", PieObject);
                 


            }
        });
    }

    $(document).ready(function () {
        $('a .white').click(function (e) {
            e.preventDefault();
        });

    });


</script>
<style>
    .headerTdClass {
        text-align: center;
    font-size: 26px;
    font-weight: 700;
    color: #440303;
    }
    .closePopup {
    font-size: 24px;
    color: #da0bad; 
    }
    .FooterTdClass {
      text-align: center;
    font-size: 17px;
    font-weight: 600;
    color: #000000;
    }

    .ChartLblClass {
        text-align: right;
    font-size: 15px;
    color: maroon;
    font-weight: 700; 
     PADDING-RIGHT: 12PX;
    }

    .shakeAlways {
     animation: 2s shakeUP infinite alternate;
    }

    @keyframes shakeUP {
        0% {
            transform: translateY(-5px);
        }

        25% {
            transform: translateY(0px);
        }

        50% {
            transform: translateY(-5px);
        }

        75% {
            transform: translateY(0px);
        }
         
        100% {
            transform: translateY(-5px);
        }
    }
</style>

<div>
    <aside class="colWraper">
        <table style="width:100%">
                <tr>
                    <td class="ChartLblClass">Click to view Efficiency Ratio Chart. <i class="fa fa-arrow-down shakeAlways"></i> </td>
                </tr>
            </table>

        <div class="diverh col-md-10">
            
            <table>
                <tr >
                    <td  colspan="5"><i class="fa fa-check-circle" style="font-size: 25px; color: #f7ffb7;"></i><span class="trSpan besidepart">Efficiency Ratio Salesmen wise</span></td>
                </tr>
                <tr>
                    <td class="pad5">


                        <dx:ASPxDateEdit ID="FromDateEF" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="cFormDateEF" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" data-toggle="tooltip" title="From Date"
                            Theme="PlasticBlue">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>


                    <td class="pad5">
                        <dx:ASPxDateEdit ID="TodateEF" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="cTodateEF" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" data-toggle="tooltip" title="To Date"
                            Theme="PlasticBlue">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>

                    <td class="pad5">

                        <a href="#" data-toggle="tooltip" title="Generate" class="white">
                            <i class="fa fa-play-circle" onclick="EFGenerate(event)"></i>
                        </a>
                    </td>
                    <td class="pad5">
                        <asp:LinkButton ID="LinkButton1EF" class="white" runat="server" OnClick="LinkButton1_Click" data-toggle="tooltip" title="Export to Excel">
                            <%--<i class="fa fa-file"></i>--%>
                             <img src="../../Dashboard/images/excel.png" class="excelIco" />
                        </asp:LinkButton>

                    </td>

                    <td class="pad5">
                        <span data-toggle="tooltip" title="Help">
                            <span data-toggle="popover" data-placement="left" data-html="true"   data-content="This report shows the Efficiency Ration of each Salesmen based on the formula as:<strong style='color: maroon;'> (No. of Invoiced Sales Order/No. of activities done)*100 </strong>in the given period. <br /><br /> <strong>Efficiency:</strong> Shows the output using formula <strong style='color: maroon;'>(No. of Invoiced Sales Order/No. of activities done)*100 </strong> in the given period."><i class="fa fa-question-circle"></i></span>
                        </span>
                    </td>

                </tr>
            </table>


        </div>
        <div class="col-md-2">
            <img src="../../Dashboard/images/chart.png" onclick="GenerateChartEfficency()" style="    border: .5px solid #675a5a;" class="zoomImg" width="100%" height="87px"/>
            <%--<span > Click To view Chart</span>--%>
        </div>





        <dx:ASPxGridViewExporter ID="exporterEF" runat="server" Landscape="false" PaperKind="A4" GridViewID="gridEF"
            PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true" FileName="Proforma Invoice/Quotation Salesman Wise">
        </dx:ASPxGridViewExporter>

        <dx:ASPxGridView ID="gridEF" runat="server" ClientInstanceName="cgridEF" KeyFieldName="cnt_internalId"
            Width="100%" Settings-HorizontalScrollBarMode="Auto"
            SettingsBehavior-ColumnResizeMode="Control" OnDataBinding="gridPhone_DataBinding1"
            Settings-VerticalScrollableHeight="237" SettingsBehavior-AllowSelectByRowClick="true"
            Settings-VerticalScrollBarMode="Auto" Theme="PlasticBlue"
            Settings-ShowFilterRow="true" Settings-ShowFilterRowMenu="true">
               <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
            <SettingsBehavior EnableCustomizationWindow="true"   />
                            <Settings ShowFooter="true"  /> 
                            <SettingsContextMenu Enabled="true" />    
            <Columns>

                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Salesmen" FieldName="SalesManName" Width="75%">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Efficiency %" FieldName="EF" Width="25%"
                    HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>

            </Columns>

            <SettingsPager PageSize="10" NumericButtonCount="4">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
            </SettingsPager>
        </dx:ASPxGridView>

        <%--<div id="EFChart" style="height:500px"></div>--%>
    </aside>


<div class="modal fade" id="EfModel" role="dialog">
    <div class="modal-dialog">
         
        <div class="modal-content">
            <%--<div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title">Efficiency Ratio Salesmen wise</h4>
            </div>--%>
            <div class="modal-body">
                <div > 
                    <table width="100%">
                        <tr>
                            <td width="90%" class="headerTdClass" >Efficiency Ratio Salesmen wise</td>
                            <td width="10%" style="text-align: right;"><span data-toggle="tooltip" title="Click to close the chart." data-dismiss="modal" style="cursor:pointer"><i class="fa fa-times closePopup"></i></span></td>
                        </tr>
                       </table>
                </div>
                 <div id="EFChart" style="height:500px"></div>
                <div  class="FooterTdClass"> Salesmen</div>
            </div>
           <%-- <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
            </div>--%>
        </div>

    </div>
</div>





</div>
