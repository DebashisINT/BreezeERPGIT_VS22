
<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="AttendanceChartReport.aspx.cs" Inherits="ERP.OMS.Management.Attendance.AttendanceChartReport" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Js/AttandanceChartReport.js?v=0.2"></script>
    <script src="../Activities/JS/SearchPopup.js"></script>
    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />
    <link href="Css/Attendance.css" rel="stylesheet" />

    <div class="panel-heading">
        <div class="panel-title">
            
            <h3>Monthwise Attendance Chart</h3>
        </div>
    </div>
    <div class="form_main">
        <div class="row">
            <div class="col-md-2">
                <label>Month / Year</label>
                <dxe:ASPxComboBox ID="cmbMonthYear" ClientInstanceName="ccmbMonthYear"
                    runat="server" ValueType="System.String">
                </dxe:ASPxComboBox>
            </div>
            <div class="col-md-2">
                <label>Employee </label>
                <dxe:ASPxButtonEdit ID="empButtonEdit" ReadOnly="true" runat="server" ClientInstanceName="cempButtonEdit">
                    <Buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </Buttons>
                    <ClientSideEvents ButtonClick="function(s,e){EmployeeSelect();}" KeyDown="function(s,e){EmployeeKeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>
            </div>
            <div class="col-md-2 butnlaymargin">
                <button type="button" class="btn btn-success" id="BtnShow" onclick="ShowAttendance()">Show</button>
            </div>
        </div>
        

        <dxe:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="100%"
            ClientInstanceName="updateChart" OnCallback="ASPxCallbackPanel1_Callback"
            >
            <PanelCollection>
                <dxe:PanelContent>
                        <asp:Panel ID="MainPanel" runat="server" Width="100%">
                            <dxeWebCharts:WebChartControl ID="WebChartControl1" runat="server" Width="1000px" Height="350px"
                                OnCustomDrawSeriesPoint="WebChartControl1_CustomDrawSeriesPoint">
                                
                            </dxeWebCharts:WebChartControl>
                        </asp:Panel> 
                    </dxe:PanelContent>
                </PanelCollection>
        </dxe:ASPxCallbackPanel>

        
         
        <asp:HiddenField runat="server" ID="EmpId" />
        <asp:HiddenField runat="server" ID="KeyValue" />
        <asp:HiddenField runat="server" ID="hdAttDate" />



    </div>






    <!--Employee Modal -->
    <div class="modal fade" id="EmployeeModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Employee Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Employeekeydown(event)" id="txtEmpSearch" autofocus width="100%" placeholder="Search By Employee Name or Unique Id" />

                    <div id="EmployeeTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Employee Name</th>
                                <th>Employee Id</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>


   



</asp:Content>
