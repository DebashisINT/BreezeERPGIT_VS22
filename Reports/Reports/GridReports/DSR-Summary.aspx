<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="DSR-Summary.aspx.cs"
    Inherits="Reports.Reports.GridReports.DSR_Summary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <script>

        function updateGridByDate() {

            if (cFormDate.GetDate() == null) {
                jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
            }
            else if (ctoDate.GetDate() == null) {
                jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
            }

            else {

                Grid.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd'));

                Grid.Refresh();


            }
        }

    </script>

    <style>
        .padTab>tbody>tr>td {
            padding-right:15px;
        }
        .padTab>tbody>tr>td:last-child {
            padding-right:0px;
        }
        .padTab{
            margin-bottom:5px;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="panel-heading">
        <div class="panel-title">
            <h3>DSR Summary</h3>
        </div>




        <table class="padTab pull-right" style="margin-top: 7px">
            <tr>
                <td>From 
                </td>
                <td style="width: 150px">
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <buttonstyle width="13px">
                        </buttonstyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>To 
                </td>
                <td style="width: 150px">
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                        <buttonstyle width="13px">
                        </buttonstyle>
                    </dxe:ASPxDateEdit>
                </td>

                <td>
                    <input type="button" value="Show" class="btn btn-success" onclick="updateGridByDate()" />

                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                        OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLSX</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>

                </td>

            </tr>

        </table>
    </div>
    <div class="form_main">


        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />

        <div class="pull-right">
        </div>
        <table class="TableMain100">


            <tr>

                <td colspan="2">
                    <div onkeypress="OnWaitingGridKeyPress(event)">
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                            OnDataBinding="ShowGrid_DataBinding" OnCustomCallback="Grid_CustomCallback" OnHtmlFooterCellPrepared="ShowGridCustOut_HtmlFooterCellPrepared"
                            OnHtmlDataCellPrepared="ShowGridCustOut_HtmlDataCellPrepared">

                            <Columns>




                                <dxe:GridViewDataTextColumn FieldName="Salesmanname" Caption="Salesman" Width="20%" VisibleIndex="0" />
                                <dxe:GridViewDataTextColumn FieldName="Outcome" Caption="Outcome" Width="60%" VisibleIndex="1" />
                                <dxe:GridViewDataTextColumn FieldName="Recordno" Caption="No. of records for selected Date" Width="20%" VisibleIndex="2" />
<%--                            <dxe:GridViewDataTextColumn FieldName="totalrecord" Caption="Total records" Width="10%" VisibleIndex="3" />               --%>



                            </Columns>

                            <settingsbehavior confirmdelete="true" enablecustomizationwindow="true" enablerowhottrack="true" />
                            <settings showfooter="true" showgrouppanel="true" showgroupfooter="VisibleIfExpanded" />
                            <settingsediting mode="EditForm" />
                            <settingscontextmenu enabled="true" />
                            <settingsbehavior autoexpandallgroups="true" columnresizemode="Control" />
                            <settings showgrouppanel="True" showstatusbar="Visible" showfilterrow="true" />
                            <settingssearchpanel visible="false" />
                            <settingspager pagesize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                            </settingspager>
                            <settings showgrouppanel="True" showstatusbar="Visible" showfilterrow="true" showfilterrowmenu="true" />

                            <totalsummary>
                            </totalsummary>
                        </dxe:ASPxGridView>

                    </div>
                </td>
            </tr>
        </table>
    </div>

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupbudget" Height="500px"
        Width="1200px" HeaderText="Details" Modal="true" AllowResize="true">
        <contentcollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </contentcollection>

        <clientsideevents closeup="BudgetAfterHide" />
    </dxe:ASPxPopupControl>



</asp:Content>


