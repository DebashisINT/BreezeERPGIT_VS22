<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_DStatChart" CodeBehind="DStatChart.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.XtraCharts.v10.2, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraCharts.v10.2.Web, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td>
                <dxeWebCharts:WebChartControl ID="chrtDalyStat" Height="275px" Width="500px" runat="server" ClientInstanceName="chart">
                    <seriesserializable>
                            <dxeCharts:Series  Name="Closing Rates" >
                                <ViewSerializable>
<dxeCharts:LineSeriesView hiddenserializablestring="to be serialized">
                                <LineStyle DashStyle="Dot"></LineStyle>

                                <LineMarkerOptions Visible="False"></LineMarkerOptions>
                                </dxeCharts:LineSeriesView>
</ViewSerializable>
                                <LabelSerializable>
<dxeCharts:PointSeriesLabel hiddenserializablestring="to be serialized" linevisible="False"><%-- overlappingoptionstypename="PointOverlappingOptions"--%>
                                    <border visible="False"></border>
                                    <fillstyle fillmode="Empty"></fillstyle>
                                </dxeCharts:PointSeriesLabel>
</LabelSerializable>
                                <PointOptionsSerializable>
<dxeCharts:PointOptions hiddenserializablestring="to be serialized"></dxeCharts:PointOptions>
</PointOptionsSerializable>
                                <LegendPointOptionsSerializable>
<dxeCharts:PointOptions hiddenserializablestring="to be serialized"></dxeCharts:PointOptions>
</LegendPointOptionsSerializable>
                            </dxeCharts:Series>
                        </seriesserializable>
                    <fillstyle>
                            <OptionsSerializable>
<dxeCharts:SolidFillOptions HiddenSerializableString="to be serialized" />
</OptionsSerializable>
                        </fillstyle>
                    <borderoptions visible="False" />
                    <seriestemplate>
                            <ViewSerializable>
<dxeCharts:SideBySideBarSeriesView HiddenSerializableString="to be serialized">
                            </dxeCharts:SideBySideBarSeriesView>
</ViewSerializable>
                            <LabelSerializable>
<dxeCharts:SideBySideBarSeriesLabel HiddenSerializableString="to be serialized" LineVisible="True" > <%--OverlappingOptionsTypeName="OverlappingOptions"--%>
                                <FillStyle >
                                    <OptionsSerializable>
<dxeCharts:SolidFillOptions HiddenSerializableString="to be serialized" />
</OptionsSerializable>
                                </FillStyle>
                            </dxeCharts:SideBySideBarSeriesLabel>
</LabelSerializable>
                            <PointOptionsSerializable>
<dxeCharts:PointOptions HiddenSerializableString="to be serialized">
                            </dxeCharts:PointOptions>
</PointOptionsSerializable>
                            <LegendPointOptionsSerializable>
<dxeCharts:PointOptions HiddenSerializableString="to be serialized">
                            </dxeCharts:PointOptions>
</LegendPointOptionsSerializable>
                        </seriestemplate>

                </dxeWebCharts:WebChartControl>


            </td>
        </tr>
    </table>
</asp:Content>

