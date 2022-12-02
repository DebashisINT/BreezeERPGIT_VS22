import React, {Component } from 'react';
import * as am4core from '@amcharts/amcharts4/core';
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";
const EmptyComponent = (props) =>{
    return (
        <div style={{
            display: "flex", 
            height:"300px",
            justifyContent: "center",
            alignItems: "center",
            fontWeight: "bold"
        }}> No Data Found</div>
    )
    
}
class CostBreakup extends Component {
    constructor(props) {
        super(props);
        this.state = { 
            costInsideLoading: false,
        }
    }
    render() { 
        const data1 = this.props.data1;
        const data2 = this.props.data2;
        
        if(data1 != null){
            var result = Object.keys(data1).map(function (key) {
                var itemK;     
                if (key == 'Initial_Material_Cost') {
                    itemK = 'Initial Material Cost';
                } else if (key == 'Initial_Service_Cost') {
                    itemK = 'Initial Service Cost';
                }
                return {
                    "project": itemK,
                    "cost": data1[key]
                };
            });
            console.log('result', result)
            am4core.ready(function () {       
                am4core.useTheme(am4themes_animated); 
                var chart = am4core.create("chartcosting1", am4charts.PieChart);
                var pieSeries = chart.series.push(new am4charts.PieSeries());
                pieSeries.dataFields.value = "cost";
                pieSeries.dataFields.category = "project";
                chart.innerRadius = am4core.percent(0);
                pieSeries.slices.template.stroke = am4core.color("#fff");
                pieSeries.slices.template.strokeWidth = 2;
                pieSeries.slices.template.strokeOpacity = 1;
                pieSeries.slices.template
                  // change the cursor on hover to make it apparent the object can be interacted with
                  .cursorOverStyle = [
                    {
                        "property": "cursor",
                        "value": "pointer"
                    }
                  ];
                pieSeries.alignLabels = false;
                //pieSeries.labels.template.bent = true;
                pieSeries.labels.template.radius = 6;
                pieSeries.labels.template.padding(0, 0, 0, 0);
                pieSeries.ticks.template.disabled = true;        
                var shadow = pieSeries.slices.template.filters.push(new am4core.DropShadowFilter);
                shadow.opacity = 0;
                var hoverState = pieSeries.slices.template.states.getKey("hover"); // normally we have to create the hover state, in this case it already exists
                // Slightly shift the shadow and make it more prominent on hover
                var hoverShadow = hoverState.filters.push(new am4core.DropShadowFilter);
                hoverShadow.opacity = 0.7;
                hoverShadow.blur = 5;
                // Add a legend
                chart.legend = new am4charts.Legend();        
                chart.data = result;
            }); // end am4core.ready()

        }
        if(data2 != null){
            var result2 = Object.keys(data2).map(function (key) {
                var itemK;
                if (key == 'Revised_Material_Cost') {
                    itemK = 'Revised Material Cost';
                } else if (key == 'Revised_Service_Cost') {
                    itemK = 'Revised Service Cost';
                }
                return {
                    "project": itemK,
                    "cost": data2[key]
                };
            });
            console.log('result', result2)
            am4core.ready(function () {
                // Themes begin
                am4core.useTheme(am4themes_animated);
                // Themes end   
                // Create chart instance
                var chart = am4core.create("chartcosting2", am4charts.PieChart);
                // Add and configure Series
                var pieSeries = chart.series.push(new am4charts.PieSeries());
                pieSeries.dataFields.value = "cost";
                pieSeries.dataFields.category = "project";
                // Let's cut a hole in our Pie chart the size of 30% the radius
                chart.innerRadius = am4core.percent(0);
                // Put a thick white border around each Slice
                pieSeries.slices.template.stroke = am4core.color("#fff");
                pieSeries.slices.template.strokeWidth = 2;
                pieSeries.slices.template.strokeOpacity = 1;
                pieSeries.slices.template
                  // change the cursor on hover to make it apparent the object can be interacted with
                  .cursorOverStyle = [
                    {
                        "property": "cursor",
                        "value": "pointer"
                    }
                  ];
                pieSeries.alignLabels = false;
                //pieSeries.labels.template.bent = true;
                pieSeries.labels.template.radius = 6;
                pieSeries.labels.template.padding(0, 0, 0, 0);
                //pieSeries.ticks.template.disabled = true;
                // Create a base filter effect (as if it's not there) for the hover to return to
                var shadow = pieSeries.slices.template.filters.push(new am4core.DropShadowFilter);
                shadow.opacity = 0;
                // Create hover state
                var hoverState = pieSeries.slices.template.states.getKey("hover"); // normally we have to create the hover state, in this case it already exists
                // Slightly shift the shadow and make it more prominent on hover
                var hoverShadow = hoverState.filters.push(new am4core.DropShadowFilter);
                hoverShadow.opacity = 0.7;
                hoverShadow.blur = 5;
                // Add a legend
                chart.legend = new am4charts.Legend();
                chart.data = result2;
            }); // end am4core.ready()
        }
        let chart1= <EmptyComponent />;
        let chart2= <EmptyComponent />;
        if(data1 != null) {
            if(data1.Initial_Material_Cost == "0.00" && data1.Initial_Service_Cost == "0.00"){
                chart1 = <EmptyComponent />
            }else{
                chart1 = <div id="chartcosting1" style={{height: "300px"}}></div>
            }
        }
        if(data2 != null) {
            if(data2.Revised_Material_Cost == "0.00" && data2.Revised_Service_Cost == "0.00"){
                chart2 = <EmptyComponent />
            }else{
                chart2 = <div id="chartcosting2" style={{height: "300px"}}></div>
            }
        }
        return ( 
            <React.Fragment>
                <div className="row">
                    <div className="col-md-6">
                        <div className="chartBox relative">
                            {/* <div className="pull-right " onClick={(e) => {this.makeFullScreen(e)}}>
                                 <i className="fa fa-arrows-alt"></i>
                            </div>  */}
                            <div className="hader">Initial Cost Breakup</div>  
                            {chart1}  
                        </div>
                    </div>
                    <div className="col-md-6">
                        <div className="chartBox relative">
                            {/* <div className="pull-right tt">
                                 <i className="fa fa-arrows-alt"></i>
                            </div> */}
                            <div className="hader">Revised Cost Breakup</div>
                            {chart2}
                        </div>
                    </div>
                </div>
            </React.Fragment>
         );
        
    }
    makeFullScreen = (e) =>{
        console.log(e)
        //e.parentElement.querySelector('.chartBox').classList.toggle("fullScreen");
    }
    
}
export default CostBreakup;

