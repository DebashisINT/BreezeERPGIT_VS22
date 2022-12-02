import React from 'react';
import('../kpiDB.css');
import('../../CRMdash/CRMdash.css');
import * as am4core from '@amcharts/amcharts4/core';
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";
import * as helpers from '../../helpers/helperFunction';
class Performance extends React.Component {

    componentDidMount(){
        console.log('section Mounted', this.props);
        //chart one
        if(this.props.leadStatus.length){
            this.generateLeadStatus(this.props.leadStatus)
        }
        //chart two
        if(this.props.InquiryAr.length){
            this.generateChart1(this.props.InquiryAr)
        }
         //chart three
        if(this.props.QuotationBreakdown.length){
            this.generateChart2(this.props.QuotationBreakdown)
        }
        //chart four
        if(this.props.OrderBreakdown.length){
            this.generateChart3(this.props.OrderBreakdown)
        }
       //chart five
        if(this.props.InvoiceBreakdown.length){
            this.generateChart4(this.props.InvoiceBreakdown)
        }      
    }
    componentDidUpdate(){
        if(this.props.leadStatus.length){
            this.generateLeadStatus(this.props.leadStatus)
        }
        if(this.props.InquiryAr.length){
            this.generateChart1(this.props.InquiryAr)
        }      
        //chart two
        if(this.props.QuotationBreakdown.length){
            this.generateChart2(this.props.QuotationBreakdown)
        }
        //chart three
        if(this.props.OrderBreakdown.length){
            this.generateChart3(this.props.OrderBreakdown)
        }
        //chart four
        if(this.props.InvoiceBreakdown.length){
            this.generateChart4(this.props.InvoiceBreakdown)
        }
    }
    generateLeadStatus = (data) =>{
        am4core.ready(function () {
            am4core.useTheme(am4themes_animated);
            var chart = am4core.create("chartdivDonut", am4charts.PieChart);
            chart.data = data;

            // Add label
            chart.innerRadius = 20;
            var label = chart.seriesContainer.createChild(am4core.Label);
            //label.text = "15K";
            label.horizontalCenter = "middle";
            label.verticalCenter = "middle";
            label.fontSize = 50;

            // Add and configure Series
            var pieSeries = chart.series.push(new am4charts.PieSeries());
            pieSeries.dataFields.value = "LDCNT";
            pieSeries.dataFields.category = "LDSTATUS";
            pieSeries.ticks.template.disabled = true;
            pieSeries.labels.template.disabled = true;
        }); // end am4core.ready()
    }
    generateChart1 = (data) =>{
        var d = data[0];
        var result = Object.keys(d).map(function (key) {
            return {
                "name": key,
                "value": d[key]
            };
        });
        am4core.ready(function () {
            am4core.useTheme(am4themes_animated);
            var chart = am4core.create("chartdivL1", am4charts.XYChart);
            chart.data = result;
            var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
            categoryAxis.dataFields.category = "name";
            categoryAxis.renderer.grid.template.location = 0;
            categoryAxis.renderer.minGridDistance = 30;
            var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
            valueAxis.numberFormatter = new am4core.NumberFormatter();
            valueAxis.numberFormatter.numberFormat = "#,##,###.##";
            var series = chart.series.push(new am4charts.ColumnSeries());
            series.dataFields.valueY = "value";
            series.dataFields.categoryX = "name";
            series.name = "name";
            series.columns.template.tooltipText = "{categoryX}: [bold]{valueY}[/]";
            series.columns.template.fillOpacity = .8;
            var columnTemplate = series.columns.template;
            columnTemplate.strokeWidth = 2;
            columnTemplate.strokeOpacity = 1;
        }); // end am4core.ready()
        
    }
    render() {
        let LDTOTAMT =  (this.props.top.LDTOTAMT == '' || undefined) ? 0 : this.props.top.LDTOTAMT;
        let INQTOTAMT =  (this.props.top.INQTOTAMT == '' || undefined) ? 0 : this.props.top.INQTOTAMT;
        let QOTOTAMT =  (this.props.top.QOTOTAMT == '' || undefined) ? 0 : this.props.top.QOTOTAMT;
        let SOTOTAMT =  (this.props.top.SOTOTAMT == '' || undefined) ? 0 : this.props.top.SOTOTAMT;
        let SITOTAMT =  (this.props.top.SITOTAMT == '' || undefined) ? 0 : this.props.top.SITOTAMT;
        let CRPTOTAMT =  (this.props.top.CRPTOTAMT == '' || undefined) ? 0 : this.props.top.CRPTOTAMT;
        console.log(LDTOTAMT, INQTOTAMT, QOTOTAMT, SOTOTAMT, SITOTAMT, CRPTOTAMT )
        return (
            <div>
                <div className="backgroundedBoxes">
                    <div className="clearfix col-md-12">
                        <div className="flex-row space-between align-items-center">
                            <div className="flex-item itemType relative">
                                <div className="">
                                    <div className="valRound" >
                                        <div class="showFullInfo">{helpers.numberWithCommas(LDTOTAMT)}</div>
                                        <span>{helpers.numFormatterLocal(LDTOTAMT)}</span>
                                    </div>
                                    <div className="smallmuted">Total</div>
                                    <div className="hdTag">LEADS</div>
                                    <div class=" vSm">Count : <span>{this.props.top.LDCNT}</span></div>
                                </div>
                            </div>
                            <div className="flex-item itemType relative">
                                <div className="">
                                    <div className="valRound " style={{background: "#f76d6d"}}>
                                        <div class="showFullInfo">{helpers.numberWithCommas(INQTOTAMT)}</div>
                                        <span id="emailValue">{helpers.numFormatterLocal(INQTOTAMT)}</span>
                                    </div>
                                    <div className="smallmuted">Total</div>
                                    <div className="hdTag">INQUIRY</div>
                                    <div class=" vSm">Count : <span id="ldcnt">{this.props.top.INQCNT}</span></div>
                                </div>
                            </div>
                            <div className="flex-item itemType relative">
                                <div className="">
                                    
                                    <div className="valRound" style={{background: "#6e98f5"}}>
                                        <div class="showFullInfo">{helpers.numberWithCommas(QOTOTAMT)}</div>
                                        <span id="callsmsValue">{helpers.numFormatterLocal(QOTOTAMT)}</span>
                                    </div>
                                    <div className="smallmuted">Total</div>
                                    <div className="hdTag">QUOTATION</div>  
                                    <div class=" vSm">Count : <span id="ldcnt">{this.props.top.QOCNT}</span></div>
                                </div>
                            </div>
                            <div className="flex-item itemType relative">
                                <div className="">
                                   
                                    <div className="valRound" style={{background: "#44c8cd"}}>
                                        <div class="showFullInfo">{helpers.numberWithCommas(SOTOTAMT)}</div>
                                        <span id="visitValue">{helpers.numFormatterLocal(SOTOTAMT)}</span>
                                    </div>
                                    <div className="smallmuted">Total</div>
                                    <div className="hdTag">ORDER</div> 
                                    <div class=" vSm">Count : <span id="ldcnt">{this.props.top.SOCNT}</span></div>
                                </div>
                            </div>
                            <div className="flex-item itemType relative">
                                <div className="">
                                  
                                    <div className="valRound" style={{background: "#9db53c"}}>
                                        <div class="showFullInfo">{helpers.numberWithCommas(SITOTAMT)}</div>
                                        <span id="socialValue">{helpers.numFormatterLocal(SITOTAMT)}</span>
                                    </div>
                                    <div className="smallmuted ">Total</div>
                                    <div className="hdTag">SALES</div> 
                                    <div class=" vSm">Count : <span id="ldcnt">{this.props.top.SICNT}</span></div>
                                </div>
                            </div>
                            <div className="flex-item itemType relative">
                                <div className="">
                                    <div className="valRound" style={{background: "#8846c5"}}>
                                        <div class="showFullInfo">{helpers.numberWithCommas(CRPTOTAMT)}</div>
                                        <span id="otherValue">{helpers.numFormatterLocal(CRPTOTAMT)}</span>
                                    </div>
                                    <div className="smallmuted">Total</div>
                                    <div className="hdTag">COLLECTION</div>  
                                    <div class=" vSm">Count : <span id="ldcnt">{this.props.top.CRPCNT}</span></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="row" style={{marginTop: "15px"}}>
                    <div className="col-md-3">
                        <div className="chartBox">
                             <div className="hader">Lead Status</div>
                             <div style={{height:"250px"}} id="chartdivDonut"></div>
                        </div>
                    </div>
                    <div className="col-md-3">
                        <div className="chartBox">
                             <div className="hader">Inquiry Breakdown</div>
                             <div style={{height:"250px"}} id="chartdivL1"></div>
                        </div>
                    </div>
                    <div className="col-md-3">
                        <div className="chartBox">
                             <div className="hader">Quotation Breakdown</div>
                             <div style={{height:"250px"}} id="chartdivL2"></div>
                        </div>
                    </div>
                    <div className="col-md-3">
                        <div className="chartBox">
                             <div className="hader">Order Breakdown</div>
                             <div style={{height:"250px"}} id="chartdivL3"></div>
                        </div>
                    </div>
                </div>
                <div className="row" style={{marginTop: "15px"}}>
                    <div className="col-md-3">
                        <div className="chartBox">
                             <div className="hader">Invoice Breakdown</div>
                             <div style={{height:"250px"}} id="chartdivL4"></div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
    generateChart2 = (data) => {
        var d = data[0];
            var result = Object.keys(d).map(function (key) {
                return {
                    "name": key,
                    "value": d[key]
                };
            });
            am4core.ready(function () {
                am4core.useTheme(am4themes_animated);
                // Themes end
                // Create chart instance
                var chart = am4core.create("chartdivL2", am4charts.XYChart);
                // Add data
                chart.data = result;
                // Create axes
                var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
                categoryAxis.dataFields.category = "name";
                categoryAxis.renderer.grid.template.location = 0;
                categoryAxis.renderer.minGridDistance = 30;
                var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
                // Create series
                var series = chart.series.push(new am4charts.ColumnSeries());
                series.dataFields.valueY = "value";
                series.dataFields.categoryX = "name";
                series.name = "name";
                series.columns.template.tooltipText = "{categoryX}: [bold]{valueY}[/]";
                series.columns.template.fillOpacity = .8;
                var columnTemplate = series.columns.template;
                columnTemplate.strokeWidth = 2;
                columnTemplate.strokeOpacity = 1;

            }); // end am4core.ready()
    }
    generateChart3 = (data) => {
        var d = data[0];
        var result = Object.keys(d).map(function (key) {
            return {
                "name": key,
                "value": d[key]
            };
        });
        am4core.ready(function () {
            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end
            // Create chart instance
            var chart = am4core.create("chartdivL3", am4charts.XYChart);
            // Add data
            chart.data = result;
            // Create axes
            var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
            categoryAxis.dataFields.category = "name";
            categoryAxis.renderer.grid.template.location = 0;
            categoryAxis.renderer.minGridDistance = 30;
            var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
            // Create series
            var series = chart.series.push(new am4charts.ColumnSeries());
            series.dataFields.valueY = "value";
            series.dataFields.categoryX = "name";
            series.name = "name";
            series.columns.template.tooltipText = "{categoryX}: [bold]{valueY}[/]";
            series.columns.template.fillOpacity = .8;
            var columnTemplate = series.columns.template;
            columnTemplate.strokeWidth = 2;
            columnTemplate.strokeOpacity = 1;
        }); // end am4core.ready()
    }
    generateChart4 = (data) => {
        var d = data[0];
            var result = Object.keys(d).map(function (key) {
                return {
                    "name": key,
                    "value": d[key]
                };
            });
            am4core.ready(function () {
                // Themes begin
                am4core.useTheme(am4themes_animated);
                // Themes end
                // Create chart instance
                var chart = am4core.create("chartdivL4", am4charts.XYChart);
                // Add data
                chart.data = result;
                // Create axes
                var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
                categoryAxis.dataFields.category = "name";
                categoryAxis.renderer.grid.template.location = 0;
                categoryAxis.renderer.minGridDistance = 30;                
                categoryAxis.renderer.labels.template.adapter.add("dy", function (dy, target) {
                    console.log('rnTempl', dy)
                    return dy;
                });
                var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
                // Create series
                var series = chart.series.push(new am4charts.ColumnSeries());
                series.dataFields.valueY = "value";
                series.dataFields.categoryX = "name";
                series.name = "name";
                series.columns.template.tooltipText = "{categoryX}: [bold]{valueY}[/]";
                series.columns.template.fillOpacity = .8;
                var columnTemplate = series.columns.template;
                columnTemplate.strokeWidth = 2;
                columnTemplate.strokeOpacity = 1;
            }); // end am4core.ready()
    }

}
export default Performance;