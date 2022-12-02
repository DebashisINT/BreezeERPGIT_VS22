<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalesmanPerformance.aspx.cs" Inherits="DashBoard.DashBoard.SalesmanPerformance.SalesmanPerformance" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title></title>
    <link href="~/Scripts/pluggins/multiselect/bootstrap-multiselect.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.js"></script>
    <script src="../Js/bootstrap.min.js"></script>
    <script src="../Js/moment.min.js"></script>
    <script src="../Js/PurchaseDb.js"></script>
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
<link href="/Scripts/Charts/styles.css" rel="stylesheet" />

<link href="/Content/media.css" rel="stylesheet" />
<script src="/Scripts/Charts/apexcharts.min.js"></script>
<link href="/assests/pluggins/LightBox/lightbox.css" rel="stylesheet" />
<script src="/assests/pluggins/LightBox/lightbox.js"></script>
<script src="/Scripts/pluggins/multiselect/bootstrap-multiselect.js"></script>
    <link href="SalesmanPerformance.css" rel="stylesheet" />
    <script src="SalesmanPerformance.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <%--salesmanPerformance--%>
        <div class="form_main ">
            <div  class="col-md-12" id="salesmanPerformance">
                <h1 class="leadH">Salesman Performance
                <div class="pull-right">
                    <table>
                        <tbody><tr>
                            <td style="font-size:13px;font-weight:400">Select Salesman</td>
                            <td><select>
                                    <option>Mohit Roy</option>
                                    <option>Gurucharan Mahato</option>
                                    <option>Anand Sharma</option>
                                    <option>Rohit Thakur</option>

                                </select></td>
                        </tr>
                    </tbody></table>
                </div>
                    </h1>

                        <div class="clearfix">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="card">
                                    <div class="leaderboardHeader">LEADS, VISITS AND CLOSED</div>
                                        <div id="chartpie" style="min-height: 364px;"><div id="apexchartsx7dw7wlt" class="apexcharts-canvas apexchartsx7dw7wlt" style="width: 480px; height: 350px;"><svg id="SvgjsSvg2044" width="480" height="350" xmlns="http://www.w3.org/2000/svg" version="1.1" xmlns:xlink="http://www.w3.org/1999/xlink" xmlns:svgjs="http://svgjs.com/svgjs" class="apexcharts-svg" xmlns:data="ApexChartsNS" transform="translate(0, 0)" style="background: transparent;"><g id="SvgjsG2047" class="apexcharts-legend" transform="translate(0, 6)"><text id="SvgjsText2050" font-family="Helvetica, Arial, sans-serif" x="417.3125" y="142" text-anchor="start" dominate-baseline="central" font-size="12px" fill="#373d3f" class="apexcharts-legend-text apexcharts-legend-series" rel="1" data:collapsed="false" style="font-family: Helvetica, Arial, sans-serif;">LEADS</text><circle id="SvgjsCircle2049" r="6" cx="-4" cy="9" class="apexcharts-legend-point" fill="#01b8aa" fill-opacity="1" stroke-width="0" stroke-opacity="1" rel="1" data:collapsed="false" transform="translate(411.3125,128.5)"></circle><text id="SvgjsText2053" font-family="Helvetica, Arial, sans-serif" x="417.3125" y="162" text-anchor="start" dominate-baseline="central" font-size="12px" fill="#373d3f" class="apexcharts-legend-text apexcharts-legend-series" rel="2" data:collapsed="false" style="font-family: Helvetica, Arial, sans-serif;">VISITS</text><circle id="SvgjsCircle2052" r="6" cx="-4" cy="29.5" class="apexcharts-legend-point" fill="#374649" fill-opacity="1" stroke-width="0" stroke-opacity="1" rel="2" data:collapsed="false" transform="translate(411.3125,128.5)"></circle><text id="SvgjsText2056" font-family="Helvetica, Arial, sans-serif" x="417.3125" y="183" text-anchor="start" dominate-baseline="central" font-size="12px" fill="#373d3f" class="apexcharts-legend-text apexcharts-legend-series" rel="3" data:collapsed="false" style="font-family: Helvetica, Arial, sans-serif;">CLOSED</text><circle id="SvgjsCircle2055" r="6" cx="-4" cy="50" class="apexcharts-legend-point" fill="#fd625e" fill-opacity="1" stroke-width="0" stroke-opacity="1" rel="3" data:collapsed="false" transform="translate(411.3125,128.5)"></circle></g><g id="SvgjsG2046" class="apexcharts-inner apexcharts-graphical" transform="translate(5, 30)"><defs id="SvgjsDefs2045"><clipPath id="gridRectMaskx7dw7wlt"><rect id="SvgjsRect2057" width="350.3125" height="330.3125" x="0" y="0" rx="0" ry="0" fill="#ffffff" opacity="1" stroke-width="0" stroke="none" stroke-dasharray="0"></rect></clipPath><clipPath id="gridRectMarkerMaskx7dw7wlt"><rect id="SvgjsRect2058" width="358.3125" height="338.3125" x="-4" y="-4" rx="0" ry="0" fill="#ffffff" opacity="1" stroke-width="0" stroke="none" stroke-dasharray="0"></rect></clipPath><filter id="SvgjsFilter2066" filterUnits="userSpaceOnUse"><feFlood id="SvgjsFeFlood2067" flood-color="black" flood-opacity="0.45" result="SvgjsFeFlood2067Out" in="SourceGraphic"></feFlood><feComposite id="SvgjsFeComposite2068" in="SvgjsFeFlood2067Out" in2="SourceAlpha" operator="in" result="SvgjsFeComposite2068Out"></feComposite><feOffset id="SvgjsFeOffset2069" dx="1" dy="1" result="SvgjsFeOffset2069Out" in="SvgjsFeComposite2068Out"></feOffset><feGaussianBlur id="SvgjsFeGaussianBlur2070" stdDeviation="1 " result="SvgjsFeGaussianBlur2070Out" in="SvgjsFeOffset2069Out"></feGaussianBlur><feMerge id="SvgjsFeMerge2071" result="SvgjsFeMerge2071Out" in="SourceGraphic"><feMergeNode id="SvgjsFeMergeNode2072" in="SvgjsFeGaussianBlur2070Out"></feMergeNode><feMergeNode id="SvgjsFeMergeNode2073" in="[object Arguments]"></feMergeNode></feMerge><feBlend id="SvgjsFeBlend2074" in="SourceGraphic" in2="SvgjsFeMerge2071Out" mode="normal" result="SvgjsFeBlend2074Out"></feBlend></filter><filter id="SvgjsFilter2079" filterUnits="userSpaceOnUse"><feFlood id="SvgjsFeFlood2080" flood-color="black" flood-opacity="0.45" result="SvgjsFeFlood2080Out" in="SourceGraphic"></feFlood><feComposite id="SvgjsFeComposite2081" in="SvgjsFeFlood2080Out" in2="SourceAlpha" operator="in" result="SvgjsFeComposite2081Out"></feComposite><feOffset id="SvgjsFeOffset2082" dx="1" dy="1" result="SvgjsFeOffset2082Out" in="SvgjsFeComposite2081Out"></feOffset><feGaussianBlur id="SvgjsFeGaussianBlur2083" stdDeviation="1 " result="SvgjsFeGaussianBlur2083Out" in="SvgjsFeOffset2082Out"></feGaussianBlur><feMerge id="SvgjsFeMerge2084" result="SvgjsFeMerge2084Out" in="SourceGraphic"><feMergeNode id="SvgjsFeMergeNode2085" in="SvgjsFeGaussianBlur2083Out"></feMergeNode><feMergeNode id="SvgjsFeMergeNode2086" in="[object Arguments]"></feMergeNode></feMerge><feBlend id="SvgjsFeBlend2087" in="SourceGraphic" in2="SvgjsFeMerge2084Out" mode="normal" result="SvgjsFeBlend2087Out"></feBlend></filter><filter id="SvgjsFilter2092" filterUnits="userSpaceOnUse"><feFlood id="SvgjsFeFlood2093" flood-color="black" flood-opacity="0.45" result="SvgjsFeFlood2093Out" in="SourceGraphic"></feFlood><feComposite id="SvgjsFeComposite2094" in="SvgjsFeFlood2093Out" in2="SourceAlpha" operator="in" result="SvgjsFeComposite2094Out"></feComposite><feOffset id="SvgjsFeOffset2095" dx="1" dy="1" result="SvgjsFeOffset2095Out" in="SvgjsFeComposite2094Out"></feOffset><feGaussianBlur id="SvgjsFeGaussianBlur2096" stdDeviation="1 " result="SvgjsFeGaussianBlur2096Out" in="SvgjsFeOffset2095Out"></feGaussianBlur><feMerge id="SvgjsFeMerge2097" result="SvgjsFeMerge2097Out" in="SourceGraphic"><feMergeNode id="SvgjsFeMergeNode2098" in="SvgjsFeGaussianBlur2096Out"></feMergeNode><feMergeNode id="SvgjsFeMergeNode2099" in="[object Arguments]"></feMergeNode></feMerge><feBlend id="SvgjsFeBlend2100" in="SourceGraphic" in2="SvgjsFeMerge2097Out" mode="normal" result="SvgjsFeBlend2100Out"></feBlend></filter></defs><g id="SvgjsG2059" class="apexcharts-pie" data:innerTranslateX="0" data:innerTranslateY="-25"><g id="SvgjsG2060" transform="translate(0, -25) scale(1)"><g id="SvgjsG2061"><g id="apexcharts-series-0" class="apexcharts-series apexcharts-pie-series LEADS" rel="1"><path id="apexcharts-pieSlice-0" d="M 175.15625 9.84146341463412 A 147.65853658536588 147.65853658536588 0 1 1 74.88337447887622 265.8900081246286 L 175.15625 157.5 L 175.15625 9.84146341463412" fill="rgba(1,184,170,1)" fill-opacity="1" stroke="#ffffff" stroke-opacity="1" stroke-linecap="butt" stroke-width="2" stroke-dasharray="0" class="apexcharts-pie-area" index="0" j="0" data:angle="222.77227722772278" data:startAngle="0" data:strokeWidth="2" data:value="250" data:pathOrig="M 175.15625 9.84146341463412 A 147.65853658536588 147.65853658536588 0 1 1 74.88337447887622 265.8900081246286 L 175.15625 157.5 L 175.15625 9.84146341463412" data:pieClicked="false"></path><text id="SvgjsText2064" font-family="Helvetica, Arial, sans-serif" x="285.14934564759477" y="200.57512858761356" text-anchor="middle" dominate-baseline="central" font-size="12px" fill="#ffffff" filter="url(#SvgjsFilter2066)" class="apexcharts-pie-label" style="font-family: Helvetica, Arial, sans-serif;">61.9%</text></g><g id="apexcharts-series-1" class="apexcharts-series apexcharts-pie-series VISITS" rel="2"><path id="apexcharts-pieSlice-1" d="M 74.88337447887622 265.8900081246286 A 147.65853658536588 147.65853658536588 0 0 1 121.31233926548313 20.008630440506494 L 175.15625 157.5 L 74.88337447887622 265.8900081246286" fill="rgba(55,70,73,1)" fill-opacity="1" stroke="#ffffff" stroke-opacity="1" stroke-linecap="butt" stroke-width="2" stroke-dasharray="0" class="apexcharts-pie-area" index="0" j="1" data:angle="115.84158415841586" data:startAngle="222.77227722772278" data:strokeWidth="2" data:value="130" data:pathOrig="M 74.88337447887622 265.8900081246286 A 147.65853658536588 147.65853658536588 0 0 1 121.31233926548313 20.008630440506494 L 175.15625 157.5 L 74.88337447887622 265.8900081246286" data:pieClicked="false"></path><text id="SvgjsText2077" font-family="Helvetica, Arial, sans-serif" x="59.080663800242235" y="135.5818321622682" text-anchor="middle" dominate-baseline="central" font-size="12px" fill="#ffffff" filter="url(#SvgjsFilter2079)" class="apexcharts-pie-label" style="font-family: Helvetica, Arial, sans-serif;">32.2%</text></g><g id="apexcharts-series-2" class="apexcharts-series apexcharts-pie-series CLOSED" rel="3"><path id="apexcharts-pieSlice-2" d="M 121.31233926548313 20.008630440506494 A 147.65853658536588 147.65853658536588 0 0 1 175.15624999999997 9.84146341463412 L 175.15625 157.5 L 121.31233926548313 20.008630440506494" fill="rgba(253,98,94,1)" fill-opacity="1" stroke="#ffffff" stroke-opacity="1" stroke-linecap="butt" stroke-width="2" stroke-dasharray="0" class="apexcharts-pie-area" index="0" j="2" data:angle="21.386138613861363" data:startAngle="338.61386138613864" data:strokeWidth="2" data:value="24" data:pathOrig="M 121.31233926548313 20.008630440506494 A 147.65853658536588 147.65853658536588 0 0 1 175.15624999999997 9.84146341463412 L 175.15625 157.5 L 121.31233926548313 20.008630440506494" data:pieClicked="false"></path><text id="SvgjsText2090" font-family="Helvetica, Arial, sans-serif" x="153.2380821622683" y="41.42441380024222" text-anchor="middle" dominate-baseline="central" font-size="12px" fill="#ffffff" filter="url(#SvgjsFilter2092)" class="apexcharts-pie-label" style="font-family: Helvetica, Arial, sans-serif;">5.9%</text></g></g></g></g><line id="SvgjsLine2101" x1="0" y1="0" x2="350.3125" y2="0" stroke="#b6b6b6" stroke-dasharray="0" stroke-width="1" class="apexcharts-ycrosshairs"></line><line id="SvgjsLine2102" x1="0" y1="0" x2="350.3125" y2="0" stroke-dasharray="0" stroke-width="0" class="apexcharts-ycrosshairs-hidden"></line></g></svg><div class="apexcharts-tooltip dark"><div class="apexcharts-tooltip-series-group"><span class="apexcharts-tooltip-marker" style="background-color: rgb(1, 184, 170);"></span><div class="apexcharts-tooltip-text"><div class="apexcharts-tooltip-y-group"><span class="apexcharts-tooltip-text-label"></span><span class="apexcharts-tooltip-text-value"></span></div><div class="apexcharts-tooltip-z-group"><span class="apexcharts-tooltip-text-z-label"></span><span class="apexcharts-tooltip-text-z-value"></span></div></div></div><div class="apexcharts-tooltip-series-group"><span class="apexcharts-tooltip-marker" style="background-color: rgb(55, 70, 73);"></span><div class="apexcharts-tooltip-text"><div class="apexcharts-tooltip-y-group"><span class="apexcharts-tooltip-text-label"></span><span class="apexcharts-tooltip-text-value"></span></div><div class="apexcharts-tooltip-z-group"><span class="apexcharts-tooltip-text-z-label"></span><span class="apexcharts-tooltip-text-z-value"></span></div></div></div><div class="apexcharts-tooltip-series-group"><span class="apexcharts-tooltip-marker" style="background-color: rgb(253, 98, 94);"></span><div class="apexcharts-tooltip-text"><div class="apexcharts-tooltip-y-group"><span class="apexcharts-tooltip-text-label"></span><span class="apexcharts-tooltip-text-value"></span></div><div class="apexcharts-tooltip-z-group"><span class="apexcharts-tooltip-text-z-label"></span><span class="apexcharts-tooltip-text-z-value"></span></div></div></div></div></div></div>
                                    </div>
                            </div>
                            <div class="col-md-6">
                                <div class="card">
                                    <div class="leaderboardHeader">QUALIFY LEAD, DEVELOP, PROPOSE AND CLOSED</div>
                                    <div id="chartpie2" style="min-height: 364px;"><div id="apexchartssnet6fv5" class="apexcharts-canvas apexchartssnet6fv5" style="width: 480px; height: 350px;"><svg id="SvgjsSvg2105" width="480" height="350" xmlns="http://www.w3.org/2000/svg" version="1.1" xmlns:xlink="http://www.w3.org/1999/xlink" xmlns:svgjs="http://svgjs.com/svgjs" class="apexcharts-svg" xmlns:data="ApexChartsNS" transform="translate(0, 0)" style="background: transparent;"><g id="SvgjsG2108" class="apexcharts-legend" transform="translate(0, 6)"><text id="SvgjsText2111" font-family="Helvetica, Arial, sans-serif" x="399.296875" y="132" text-anchor="start" dominate-baseline="central" font-size="12px" fill="#373d3f" class="apexcharts-legend-text apexcharts-legend-series" rel="1" data:collapsed="false" style="font-family: Helvetica, Arial, sans-serif;">Qualify Lead</text><circle id="SvgjsCircle2110" r="6" cx="-4" cy="9" class="apexcharts-legend-point" fill="#01b8aa" fill-opacity="1" stroke-width="0" stroke-opacity="1" rel="1" data:collapsed="false" transform="translate(393.296875,118.25)"></circle><text id="SvgjsText2114" font-family="Helvetica, Arial, sans-serif" x="399.296875" y="152" text-anchor="start" dominate-baseline="central" font-size="12px" fill="#373d3f" class="apexcharts-legend-text apexcharts-legend-series" rel="2" data:collapsed="false" style="font-family: Helvetica, Arial, sans-serif;">Develop</text><circle id="SvgjsCircle2113" r="6" cx="-4" cy="29.5" class="apexcharts-legend-point" fill="#374649" fill-opacity="1" stroke-width="0" stroke-opacity="1" rel="2" data:collapsed="false" transform="translate(393.296875,118.25)"></circle><text id="SvgjsText2117" font-family="Helvetica, Arial, sans-serif" x="399.296875" y="173" text-anchor="start" dominate-baseline="central" font-size="12px" fill="#373d3f" class="apexcharts-legend-text apexcharts-legend-series" rel="3" data:collapsed="false" style="font-family: Helvetica, Arial, sans-serif;">Propose</text><circle id="SvgjsCircle2116" r="6" cx="-4" cy="50" class="apexcharts-legend-point" fill="#f2c80f" fill-opacity="1" stroke-width="0" stroke-opacity="1" rel="3" data:collapsed="false" transform="translate(393.296875,118.25)"></circle><text id="SvgjsText2120" font-family="Helvetica, Arial, sans-serif" x="399.296875" y="193" text-anchor="start" dominate-baseline="central" font-size="12px" fill="#373d3f" class="apexcharts-legend-text apexcharts-legend-series" rel="4" data:collapsed="false" style="font-family: Helvetica, Arial, sans-serif;">Close</text><circle id="SvgjsCircle2119" r="6" cx="-4" cy="70.5" class="apexcharts-legend-point" fill="#fd625e" fill-opacity="1" stroke-width="0" stroke-opacity="1" rel="4" data:collapsed="false" transform="translate(393.296875,118.25)"></circle></g><g id="SvgjsG2107" class="apexcharts-inner apexcharts-graphical" transform="translate(5, 30)"><defs id="SvgjsDefs2106"><clipPath id="gridRectMasksnet6fv5"><rect id="SvgjsRect2121" width="332.296875" height="312.296875" x="0" y="0" rx="0" ry="0" fill="#ffffff" opacity="1" stroke-width="0" stroke="none" stroke-dasharray="0"></rect></clipPath><clipPath id="gridRectMarkerMasksnet6fv5"><rect id="SvgjsRect2122" width="340.296875" height="320.296875" x="-4" y="-4" rx="0" ry="0" fill="#ffffff" opacity="1" stroke-width="0" stroke="none" stroke-dasharray="0"></rect></clipPath><filter id="SvgjsFilter2134" filterUnits="userSpaceOnUse"><feFlood id="SvgjsFeFlood2135" flood-color="black" flood-opacity="0.45" result="SvgjsFeFlood2135Out" in="SourceGraphic"></feFlood><feComposite id="SvgjsFeComposite2136" in="SvgjsFeFlood2135Out" in2="SourceAlpha" operator="in" result="SvgjsFeComposite2136Out"></feComposite><feOffset id="SvgjsFeOffset2137" dx="1" dy="1" result="SvgjsFeOffset2137Out" in="SvgjsFeComposite2136Out"></feOffset><feGaussianBlur id="SvgjsFeGaussianBlur2138" stdDeviation="1 " result="SvgjsFeGaussianBlur2138Out" in="SvgjsFeOffset2137Out"></feGaussianBlur><feMerge id="SvgjsFeMerge2139" result="SvgjsFeMerge2139Out" in="SourceGraphic"><feMergeNode id="SvgjsFeMergeNode2140" in="SvgjsFeGaussianBlur2138Out"></feMergeNode><feMergeNode id="SvgjsFeMergeNode2141" in="[object Arguments]"></feMergeNode></feMerge><feBlend id="SvgjsFeBlend2142" in="SourceGraphic" in2="SvgjsFeMerge2139Out" mode="normal" result="SvgjsFeBlend2142Out"></feBlend></filter><filter id="SvgjsFilter2147" filterUnits="userSpaceOnUse"><feFlood id="SvgjsFeFlood2148" flood-color="black" flood-opacity="0.45" result="SvgjsFeFlood2148Out" in="SourceGraphic"></feFlood><feComposite id="SvgjsFeComposite2149" in="SvgjsFeFlood2148Out" in2="SourceAlpha" operator="in" result="SvgjsFeComposite2149Out"></feComposite><feOffset id="SvgjsFeOffset2150" dx="1" dy="1" result="SvgjsFeOffset2150Out" in="SvgjsFeComposite2149Out"></feOffset><feGaussianBlur id="SvgjsFeGaussianBlur2151" stdDeviation="1 " result="SvgjsFeGaussianBlur2151Out" in="SvgjsFeOffset2150Out"></feGaussianBlur><feMerge id="SvgjsFeMerge2152" result="SvgjsFeMerge2152Out" in="SourceGraphic"><feMergeNode id="SvgjsFeMergeNode2153" in="SvgjsFeGaussianBlur2151Out"></feMergeNode><feMergeNode id="SvgjsFeMergeNode2154" in="[object Arguments]"></feMergeNode></feMerge><feBlend id="SvgjsFeBlend2155" in="SourceGraphic" in2="SvgjsFeMerge2152Out" mode="normal" result="SvgjsFeBlend2155Out"></feBlend></filter><filter id="SvgjsFilter2160" filterUnits="userSpaceOnUse"><feFlood id="SvgjsFeFlood2161" flood-color="black" flood-opacity="0.45" result="SvgjsFeFlood2161Out" in="SourceGraphic"></feFlood><feComposite id="SvgjsFeComposite2162" in="SvgjsFeFlood2161Out" in2="SourceAlpha" operator="in" result="SvgjsFeComposite2162Out"></feComposite><feOffset id="SvgjsFeOffset2163" dx="1" dy="1" result="SvgjsFeOffset2163Out" in="SvgjsFeComposite2162Out"></feOffset><feGaussianBlur id="SvgjsFeGaussianBlur2164" stdDeviation="1 " result="SvgjsFeGaussianBlur2164Out" in="SvgjsFeOffset2163Out"></feGaussianBlur><feMerge id="SvgjsFeMerge2165" result="SvgjsFeMerge2165Out" in="SourceGraphic"><feMergeNode id="SvgjsFeMergeNode2166" in="SvgjsFeGaussianBlur2164Out"></feMergeNode><feMergeNode id="SvgjsFeMergeNode2167" in="[object Arguments]"></feMergeNode></feMerge><feBlend id="SvgjsFeBlend2168" in="SourceGraphic" in2="SvgjsFeMerge2165Out" mode="normal" result="SvgjsFeBlend2168Out"></feBlend></filter><filter id="SvgjsFilter2173" filterUnits="userSpaceOnUse"><feFlood id="SvgjsFeFlood2174" flood-color="black" flood-opacity="0.45" result="SvgjsFeFlood2174Out" in="SourceGraphic"></feFlood><feComposite id="SvgjsFeComposite2175" in="SvgjsFeFlood2174Out" in2="SourceAlpha" operator="in" result="SvgjsFeComposite2175Out"></feComposite><feOffset id="SvgjsFeOffset2176" dx="1" dy="1" result="SvgjsFeOffset2176Out" in="SvgjsFeComposite2175Out"></feOffset><feGaussianBlur id="SvgjsFeGaussianBlur2177" stdDeviation="1 " result="SvgjsFeGaussianBlur2177Out" in="SvgjsFeOffset2176Out"></feGaussianBlur><feMerge id="SvgjsFeMerge2178" result="SvgjsFeMerge2178Out" in="SourceGraphic"><feMergeNode id="SvgjsFeMergeNode2179" in="SvgjsFeGaussianBlur2177Out"></feMergeNode><feMergeNode id="SvgjsFeMergeNode2180" in="[object Arguments]"></feMergeNode></feMerge><feBlend id="SvgjsFeBlend2181" in="SourceGraphic" in2="SvgjsFeMerge2178Out" mode="normal" result="SvgjsFeBlend2181Out"></feBlend></filter></defs><g id="SvgjsG2123" class="apexcharts-pie" data:innerTranslateX="0" data:innerTranslateY="-25"><g id="SvgjsG2125" class="apexcharts-datalabels-group" style="opacity: 1;"><text id="SvgjsText2126" font-family="Helvetica, Arial, sans-serif" x="166.1484375" y="122.5" text-anchor="middle" dominate-baseline="central" font-size="16px" fill="#373d3f" class="apexcharts-datalabel-label" style="font-family: Helvetica, Arial, sans-serif; fill: rgb(55, 61, 63);">Total</text><text id="SvgjsText2127" font-family="Helvetica, Arial, sans-serif" x="166.1484375" y="158.5" text-anchor="middle" dominate-baseline="central" font-size="20px" fill="#373d3f" class="apexcharts-datalabel-value" style="font-family: Helvetica, Arial, sans-serif;">404</text></g><g id="SvgjsG2124" transform="translate(0, -25) scale(1)"><circle id="SvgjsCircle2128" r="95.97804878048782" cx="166.1484375" cy="157.5" fill="transparent"></circle><g id="SvgjsG2129"><g id="apexcharts-series-0" class="apexcharts-series apexcharts-pie-series Qualify-Lead" rel="1"><path id="apexcharts-pieSlice-0" d="M 166.1484375 9.84146341463412 A 147.65853658536588 147.65853658536588 0 1 1 65.87556197887622 265.8900081246286 L 100.97106841126954 227.95350528100857 A 95.97804878048782 95.97804878048782 0 1 0 166.1484375 61.521951219512175 L 166.1484375 9.84146341463412 z" fill="rgba(1,184,170,1)" fill-opacity="1" stroke="#ffffff" stroke-opacity="1" stroke-linecap="butt" stroke-width="2" stroke-dasharray="0" class="apexcharts-pie-area" index="0" j="0" data:angle="222.77227722772278" data:startAngle="0" data:strokeWidth="2" data:value="250" data:pathOrig="M 166.1484375 9.84146341463412 A 147.65853658536588 147.65853658536588 0 1 1 65.87556197887622 265.8900081246286 L 100.97106841126954 227.95350528100857 A 95.97804878048782 95.97804878048782 0 1 0 166.1484375 61.521951219512175 L 166.1484375 9.84146341463412 z"></path><text id="SvgjsText2132" font-family="Helvetica, Arial, sans-serif" x="279.5788173865821" y="201.9212263559765" text-anchor="middle" dominate-baseline="central" font-size="12px" fill="#ffffff" filter="url(#SvgjsFilter2134)" class="apexcharts-pie-label" style="font-family: Helvetica, Arial, sans-serif;">61.9%</text></g><g id="apexcharts-series-1" class="apexcharts-series apexcharts-pie-series Develop" rel="2"><path id="apexcharts-pieSlice-1" d="M 65.87556197887622 265.8900081246286 A 147.65853658536588 147.65853658536588 0 0 1 56.212112808769135 58.924912173936676 L 94.68982645069994 93.42619291305884 A 95.97804878048782 95.97804878048782 0 0 0 100.97106841126954 227.95350528100857 L 65.87556197887622 265.8900081246286 z" fill="rgba(55,70,73,1)" fill-opacity="1" stroke="#ffffff" stroke-opacity="1" stroke-linecap="butt" stroke-width="2" stroke-dasharray="0" class="apexcharts-pie-area" index="0" j="1" data:angle="89.10891089108912" data:startAngle="222.77227722772278" data:strokeWidth="2" data:value="100" data:pathOrig="M 65.87556197887622 265.8900081246286 A 147.65853658536588 147.65853658536588 0 0 1 56.212112808769135 58.924912173936676 L 94.68982645069994 93.42619291305884 A 95.97804878048782 95.97804878048782 0 0 0 100.97106841126954 227.95350528100857 L 65.87556197887622 265.8900081246286 z"></path><text id="SvgjsText2145" font-family="Helvetica, Arial, sans-serif" x="44.46271420234969" y="163.18165272706045" text-anchor="middle" dominate-baseline="central" font-size="12px" fill="#ffffff" filter="url(#SvgjsFilter2147)" class="apexcharts-pie-label" style="font-family: Helvetica, Arial, sans-serif;">24.8%</text></g><g id="apexcharts-series-2" class="apexcharts-series apexcharts-pie-series Propose" rel="3"><path id="apexcharts-pieSlice-2" d="M 56.212112808769135 58.924912173936676 A 147.65853658536588 147.65853658536588 0 0 1 112.30452676548313 20.008630440506494 L 131.14989552256404 68.13060978632923 A 95.97804878048782 95.97804878048782 0 0 0 94.68982645069994 93.42619291305884 L 56.212112808769135 58.924912173936676 z" fill="rgba(242,200,15,1)" fill-opacity="1" stroke="#ffffff" stroke-opacity="1" stroke-linecap="butt" stroke-width="2" stroke-dasharray="0" class="apexcharts-pie-area" index="0" j="2" data:angle="26.73267326732673" data:startAngle="311.8811881188119" data:strokeWidth="2" data:value="30" data:pathOrig="M 56.212112808769135 58.924912173936676 A 147.65853658536588 147.65853658536588 0 0 1 112.30452676548313 20.008630440506494 L 131.14989552256404 68.13060978632923 A 95.97804878048782 95.97804878048782 0 0 0 94.68982645069994 93.42619291305884 L 56.212112808769135 58.924912173936676 z"></path><text id="SvgjsText2158" font-family="Helvetica, Arial, sans-serif" x="96.70808103020372" y="57.41137264638478" text-anchor="middle" dominate-baseline="central" font-size="12px" fill="#ffffff" filter="url(#SvgjsFilter2160)" class="apexcharts-pie-label" style="font-family: Helvetica, Arial, sans-serif;">7.4%</text></g><g id="apexcharts-series-3" class="apexcharts-series apexcharts-pie-series Close" rel="4"><path id="apexcharts-pieSlice-3" d="M 112.30452676548313 20.008630440506494 A 147.65853658536588 147.65853658536588 0 0 1 166.14843749999997 9.84146341463412 L 166.14843749999997 61.521951219512175 A 95.97804878048782 95.97804878048782 0 0 0 131.14989552256404 68.13060978632923 L 112.30452676548313 20.008630440506494 z" fill="rgba(253,98,94,1)" fill-opacity="1" stroke="#ffffff" stroke-opacity="1" stroke-linecap="butt" stroke-width="2" stroke-dasharray="0" class="apexcharts-pie-area" index="0" j="3" data:angle="21.386138613861363" data:startAngle="338.61386138613864" data:strokeWidth="2" data:value="24" data:pathOrig="M 112.30452676548313 20.008630440506494 A 147.65853658536588 147.65853658536588 0 0 1 166.14843749999997 9.84146341463412 L 166.14843749999997 61.521951219512175 A 95.97804878048782 95.97804878048782 0 0 0 131.14989552256404 68.13060978632923 L 112.30452676548313 20.008630440506494 z"></path><text id="SvgjsText2171" font-family="Helvetica, Arial, sans-serif" x="143.5453269173392" y="37.79705173149979" text-anchor="middle" dominate-baseline="central" font-size="12px" fill="#ffffff" filter="url(#SvgjsFilter2173)" class="apexcharts-pie-label" style="font-family: Helvetica, Arial, sans-serif;">5.9%</text></g></g></g></g><line id="SvgjsLine2182" x1="0" y1="0" x2="332.296875" y2="0" stroke="#b6b6b6" stroke-dasharray="0" stroke-width="1" class="apexcharts-ycrosshairs"></line><line id="SvgjsLine2183" x1="0" y1="0" x2="332.296875" y2="0" stroke-dasharray="0" stroke-width="0" class="apexcharts-ycrosshairs-hidden"></line></g></svg></div></div>
                                </div>
                            </div>

                        </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="card">
                                        <div class="leaderboardHeader">TARGET AND ACHIVEMENT</div>
                                        <div id=""><img src="/assests/images/HDF3.jpg" class="resposive-image"></div>
                                    </div>
                                </div>


                            </div>
                        </div>
            </div>
        </div>
        <%--salesmanPerformance--%>
        <script>

            function NoFileAlert() {

                jAlert('No data for export');
            }

            function mapcloseclick() {
                $("#map").addClass("hide");
            }



            var objData = {};
            $(document).ready(function () {

                //Surojit

                $('#cmbState').multiselect({
                    includeSelectAllOption: true,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true,
                    onDropdownHide: function (event) {
                        GetSateValue();
                    }
                }).multiselect('selectAll', false).multiselect('updateButtonText');
                stateids = $('#cmbState').val();

                var statelistcount = $('#hdnStateListCount').val();
                if (statelistcount > 0) {
                    var GroupBy = $('#hdnGridStatewiseSummaryGroupBy').val();
                    for (var i = objsettings.length - 1; i >= 0; i--) {
                        if (objsettings[i].ID == settingsid) {
                            objsettings.splice(i, 1);
                        }
                    }

                    if (settingsid == "1") {
                        var obj = {};
                        obj.ID = "1";
                        obj.action = "AT_WORK";
                        obj.rptype = "Summary";
                        obj.empid = "";
                        obj.stateid = stateids.join(',');// cmbState.GetValue();
                        obj.designid = "";
                        objsettings.push(obj);
                    }

                    WindowSize = $(window).width();


                    $("#lblAtWork").html("<img src='/assests/images/Spinner.gif' />");
                    $("#lblOnLeave").html("<img src='/assests/images/Spinner.gif' />");
                    $("#lblNotLoggedIn").html("<img src='/assests/images/Spinner.gif' />");
                    $("#lblTotal").html("<img src='/assests/images/Spinner.gif' />");
                    stateid = stateids.join(',');// cmbState.GetValue();
                    $("#salesmanheader").html("State wise Summary");
                    stateid = stateids.join(',');// cmbState.GetValue();
                    //GetAddress(stateid);
                    objData = {};

                    var hdnTotalEmployees = $('#hdnTotalEmployees').val();
                    var hdnAtWork = $('#hdnAtWork').val();
                    var hdnOnLeave = $('#hdnOnLeave').val();
                    var hdnNotLoggedIn = $('#hdnNotLoggedIn').val();
                    var hdnStatewiseSummary = $('#hdnStatewiseSummary').val();

                    var obj = {};
                    obj.stateid = stateid;

                    if (hdnTotalEmployees > 0 || hdnAtWork > 0 || hdnOnLeave > 0 || hdnNotLoggedIn > 0) {
                        $.ajax({
                            type: "POST",
                            url: "/DashboardMenu/GetDashboardData",
                            data: JSON.stringify(obj),
                            async: true,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                if (hdnAtWork > 0) {
                                    $("#lblAtWork").html(response.lblAtWork);
                                }
                                if (hdnOnLeave > 0) {
                                    $("#lblOnLeave").html(response.lblOnLeave);
                                }
                                if (hdnNotLoggedIn > 0) {
                                    $("#lblNotLoggedIn").html(response.lblNotLoggedIn);
                                }
                                if (hdnTotalEmployees > 0) {
                                    $("#lblTotal").html(response.lblTotal);
                                }


                                //$("#lbltotalshop").text(response.TotalVisit);
                                //$("#lblnewvisit").text(response.NewVisit);
                                //$("#lblrevisit").text(response.ReVisit);

                                //$("#lblavgvisits").text(response.AvgPerDay);
                                //$("#lblavgduration").text(response.AvgDurationPerShop);

                                //$("#lbltodaysale").text(response.TODAYSALES);
                                //$("#lblavgsale").text(response.AVGSALES);
                                //$("#lbltotalsale").text(response.TOTALSALES);




                            },
                            error: function (response) {
                                jAlert("Please try again later");
                            }
                        });
                    }
                    if (hdnStatewiseSummary > 0) {
                        gridsalesman.ClearFilter();
                        gridsalesman.Refresh();
                    }
                    //gridsalesman.Refresh();
                    // $("#gridsalesman_DXMainTable > tbody > tr > td.dxgvHeader_PlasticBlue").css({ 'background': divBGcolor });
                    //setTimeout(function () { gridsalesman.GroupBy(GroupBy); }, 2000);
                }
                else {
                    $('.bodymain_areastatewise').hide();
                }

            });

            function gridsummarydashboardExport() {
                var url = '@Url.Action("ExportDashboardSummaryGridView", "DashboardMenu", new { type = "_type_" })'

                window.location.href = url.replace("_type_", 3);


            }

            function gridsalesmanExport() {
                var url = '@Url.Action("ExportDashboardGridViewSalesmanDetail", "DashboardMenu", new { type = "_type_" })'
                window.location.href = url.replace("_type_", 3);
            }

            function griddashboardgridviewexport() {
                var url = '@Url.Action("ExportDashboardGridView", "DashboardMenu", new { type = "_type_" })'
                window.location.href = url.replace("_type_", 3);
            }

            function griddashboardgridviewdetailsexport() {
                var url = '@Url.Action("ExportDashboardGridViewDetails", "DashboardMenu", new { type = "_type_" })'
                window.location.href = url.replace("_type_", 3);
            }





                </script>
    </form>
</body>
</html>
