<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="allEmpAttRecords.aspx.cs" Inherits="ERP.OMS.Management.Attendance.allEmpAttRecords" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="../../../assests/js/jquery.min.js"></script>


    <script>

        $(document).ready(function () {

            if (document.referrer == "") {
                $('#mainDivattReport').hide();
                return;
            }
            document.getElementById('hdEmpId').value = localStorage.getItem('attRptEmpId');
            document.getElementById('hdBranchId').value = localStorage.getItem('attRptBranch');
            document.getElementById('HdBarcnhName').value = localStorage.getItem('attRptBranchName');
            document.getElementById('hdShowInactive').value = localStorage.getItem('showInactive');

            document.getElementById('HdFromDate').value = localStorage.getItem('attRptFromDate');
            document.getElementById('HdToDate').value = localStorage.getItem('attRptToDate');
            document.getElementById('ConsiderPayBranch').value = localStorage.getItem('attConsiderPayBranch');
            cGrid.PerformCallback();
        })

        function gridEndCallback() {
            if (cGrid.cpProcExecuted) {
                if (cGrid.cpProcExecuted == "1") {
                    cGrid.Refresh();
                }
            }
        }

    </script>
    <style>
        .bandedHeader {
            background: #54749D;
            font-size: medium;
        }

        .gridHeader {
            background: #54749D;
        }

        .dxgvSelectedRow_PlasticBlue {
            background: #54749D;
        }

        .exprtClass {
            background: #54749D;
            font-size: 16px;
            margin-top: 10px;
            color: white;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="mainDivattReport">
            <div style="background: #54749D; display: none">
                <span>Employeewise Attendance Record</span>

            </div>
            <dxe:ASPxGridViewExporter ID="exporter" GridViewID="GridFullYear" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
            </dxe:ASPxGridViewExporter>

            <dxe:ASPxGridView ID="GridFullYear" runat="server" ClientInstanceName="cGrid" KeyFieldName="EmpId"
                OnCustomCallback="GridFullYear_CustomCallback" Width="100%" Settings-HorizontalScrollBarMode="Auto"
                SettingsBehavior-ColumnResizeMode="Control" DataSourceID="EntityServerModeDataSource"
                Settings-VerticalScrollableHeight="450" SettingsBehavior-AllowSelectByRowClick="true"
                Settings-VerticalScrollBarMode="Auto">

                <Columns>
                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="Employee Details" Name="MainBandedHeader" FixedStyle="Left">
                        <Columns>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Emp Code" Width="150" FieldName="EmpCode">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Name" Width="200" FieldName="EmpName">
                            </dxe:GridViewDataTextColumn>

                        </Columns>
                    </dxe:GridViewBandColumn>

                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="01" Name="Band1">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day1In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day1Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day1WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day1Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day1">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks1">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>

                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="02" Name="Band2">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day2In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day2Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day2WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day2Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day2">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks2">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>

                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="03" Name="Band3">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day3In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day3Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day3WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day3Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day3">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks3">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>

                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="04" Name="Band4">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day4In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day4Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day4WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day4Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day4">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks4">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>

                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="05" Name="Band5">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day5In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day5Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day5WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day5Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day5">
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks5" Width="100" FieldName="Remarks4">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>

                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="06" Name="Band6">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day6In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day6Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day6WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day6Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day6">
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks6">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>


                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="07" Name="Band7">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day7In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day7Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day7WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day7Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day7">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks7">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>

                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="08" Name="Band8">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day8In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day8Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day8WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day8Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day8">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks8">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>

                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="09" Name="Band9">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day9In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day9Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day9WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day9Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day9">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks9">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>



                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="10" Name="Band10">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day10In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day10Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day10WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day10Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day10">
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks10">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>

                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="11" Name="Band11">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day11In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day11Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day11WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day11Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day11">
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks11">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>

                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="12" Name="Band12">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day12In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day12Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day12WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day12Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day12">
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks12">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>

                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="13" Name="Band13">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day13In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day13Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day13WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day13Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day13">
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks13">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>

                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="14" Name="Band14">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day14In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day14Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day14WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day14Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day14">
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks14">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>

                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="15" Name="Band15">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day15In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day15Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day15WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day15Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day15">
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks15">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>

                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="16" Name="Band16">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day16In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day16Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day16WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day16Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day16">
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks16">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>


                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="17" Name="Band17">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day17In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day17out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day17WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day17Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day17">
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks17">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>

                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="18" Name="Band18">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day18In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day18Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day18WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day18Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day18">
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks18">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>

                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="19" Name="Band19">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day19In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day19Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day19WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day19Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day19">
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks19">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>

                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="20" Name="Band20">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day20In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day20Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day20WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day20Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day20">
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks20">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>

                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="21" Name="Band21">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day21In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day21Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day21WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day21Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day21">
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks21">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>

                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="22" Name="Band22">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day22In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day22Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day22WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day22Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day22">
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks22">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>

                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="23" Name="Band23">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day23In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day23Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day23WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day23Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day23">
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks23">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>

                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="24" Name="Band24">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day24In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day24Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day24WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day24Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day24">
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks24">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>

                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="25" Name="Band25">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day25In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day25Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day25WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day25Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day25">
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks25">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>

                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="26" Name="Band26">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day26In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day26Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day26WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day26Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day26">
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks26">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>


                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="27" Name="Band27">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day27In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day27Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day27WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day27Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day27">
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks27">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>

                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="28" Name="Band28">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day28In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day28Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day28WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day28Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day28">
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks28">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>

                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="29" Name="Band29">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day29In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day29Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day29WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day29Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day29">
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks29">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>


                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="30" Name="Band30">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day30In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day30Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day30WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day30Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day30">
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks30">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>

                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="31" Name="Band31">
                        <Columns>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day31In" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day31Out" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                            </dxe:GridViewDataTimeEditColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Duration" Width="70" CellStyle-HorizontalAlign="Center" FieldName="Day31WO">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late Hour(s)" HeaderStyle-HorizontalAlign="Center" Width="85" FieldName="Day31Late">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" Width="100" FieldName="Day31">
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" Width="100" FieldName="Remarks31">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>


                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="Summary">
                        <Columns>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Late(s)" Width="70" FieldName="LateCount" HeaderStyle-HorizontalAlign="Center">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Present(Full)" Width="80" FieldName="PresentFull" HeaderStyle-HorizontalAlign="Center">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Present(Half)" Width="80" FieldName="PresentHalf" HeaderStyle-HorizontalAlign="Center">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Weekly Off" Width="75" FieldName="WeeklyOff" HeaderStyle-HorizontalAlign="Center">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Holiday" Width="75" FieldName="HolidayCount" HeaderStyle-HorizontalAlign="Center">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Absent" Width="75" FieldName="TotAbsent" HeaderStyle-HorizontalAlign="Center">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>

                        </Columns>
                    </dxe:GridViewBandColumn>


                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="Working Hour">
                        <Columns>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Estimated " Width="80" FieldName="WorkingHourExpected" HeaderStyle-HorizontalAlign="Center">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Actual " Width="76" FieldName="WorkingHourActual" HeaderStyle-HorizontalAlign="Center">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remaining" Width="80" FieldName="WorkingHourRemaining" HeaderStyle-HorizontalAlign="Center">
                                <CellStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                    </dxe:GridViewBandColumn>
                </Columns>
                <ClientSideEvents EndCallback="gridEndCallback" />
                <SettingsPager PageSize="20">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="20,50,100,150,200" />
                </SettingsPager>
            </dxe:ASPxGridView>
        </div>

        <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="tbl_EmpAttendanceRecord_reports" />

        <asp:HiddenField ID="hdEmpId" runat="server" />
        <asp:HiddenField ID="hdBranchId" runat="server" />
        <asp:HiddenField ID="HdBarcnhName" runat="server" />
        <asp:HiddenField ID="HdFromDate" runat="server" />
        <asp:HiddenField ID="HdToDate" runat="server" />
        <asp:HiddenField ID="hdShowInactive" runat="server" />
        <asp:HiddenField ID="ConsiderPayBranch" runat="server" />

        <% if (rights.CanExport)
           { %>
        <asp:DropDownList ID="drdExport" runat="server" CssClass="exprtClass" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
            <asp:ListItem Value="0">Export to</asp:ListItem>
            <asp:ListItem Value="2">XLSX</asp:ListItem>
        </asp:DropDownList>
        <%} %>
    </form>
</body>
</html>
