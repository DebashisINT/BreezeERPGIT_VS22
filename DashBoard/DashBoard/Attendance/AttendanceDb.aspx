<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AttendanceDb.aspx.cs" Inherits="DashBoard.DashBoard.Attendance.AttendanceDb" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <title>Today's Attendance</title>
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/main.css" rel="stylesheet" />
    <link href="../css/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <link href="../css/SearchPopup.css" rel="stylesheet" />
    <script src="../Js/jquery.3.3.1.js"></script>
    
    <script src="../Js/bootstrap.min.js"></script>
    <link href="../css/newtheme.css" rel="stylesheet" />
    <script src="Js/attendancemain.js?v=0.2"></script>
    <link href="Css/attendancemain.css?v=0.2" rel="stylesheet" />

    <link href="../css/dashboard.css" rel="stylesheet" />
    <script>
        function reloadParent() {
            parent.document.location.href = '/oms/management/projectmainpage.aspx'
        }
    </script>
    <script src="../Js/amC3/amcharts.js"></script>
    <script src="../Js/amC3/serial.js"></script>
    <script src="../Js/amC3/pie.js"></script>
    <script src="../Js/amC3/export.min.js"></script>
    <link href="../Js/amC3/export.css" rel="stylesheet" />
    <script src="../Js/amC3/light.js"></script>
</head>
<body > 

    <form id="form1" runat="server">
    <div>
         <div class="clearfix">
            <div class="col-md-12 clearfix padding bdBot hdBoder ">
                <h3 class="pull-left fontPop">Today's Attendance</h3>
                <span class="pull-right">  <a href="#" onclick="reloadParent()" class="pageClose"><i class="fa fa-times"></i></a></span>
            </div>
        </div>

        <div class="col-md-12 clearfix effWidgetCnt">
            <div class="flexConatiner">
                <div class="flexItem" runat="server" id="EmpCount">
                    <div class="top">
                        <span class="icon c3"><i class="fa fa-users"></i></span>
                        <div class="">
                            <div class="f14">Employees</div>
                            <div class="number"><div id="TotalEmp"></div></div>
                        </div>
                    </div>
                </div>
                <div class="flexItem " runat="server" id="PresentCount">
                    <div class="top">
                        <span class="icon c2"><i class="fa fa-user"></i></span>
                        <div class="">
                            <div class="f14">Present</div>
                            <div class="number"><div id="Present"></div></div>
                        </div>
                    </div>
                </div>
                <div class="flexItem " runat="server" id="AbsentCount">
                    <div class="top">
                        <span class="icon "><i class="fa fa-info"></i></span>
                        <div class="">
                            <div class="f14">Absent</div>
                            <div class="number"><div id="todaysAbsent"></div></div>
                        </div>
                    </div>
                </div>
                <div class="flexItem" runat="server" id="LateComersCount" onclick="loadLateComers()" style="cursor:pointer">
                    <div class="top">
                        <span class="icon c4"><i class="fa fa-twitter"></i></span>
                        <div class="">
                            <div class="f14">Late Comers</div>
                            <div class="number"><div id="LateCount"></div></div>
                        </div>
                    </div>
                </div>
                <div class="flexItem " runat="server" id="OnLeaveCount">
                    <div class="top">
                        <span class="icon c5"><i class="fa fa-plane"></i></span>
                        <div class="">
                            <div class="f14">On Leave</div>
                            <div class="number"><div id="LeaveCount"></div></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-12">
            <%--<div class="col-md-6">
                <div class="clearfix">--%>
                <div  class="col-md-4" runat="server" id="InOutCount">

                    <div class="PanelMainDiv">
                     <div class="PopupDiv">
                        <dx:ASPxGridView ID="ASPxGridView1" runat="server" ClientInstanceName="cgridInoutCount" KeyFieldName="Emp_InternalId"
                        Width="100%" Settings-HorizontalScrollBarMode="Auto"
                        SettingsBehavior-ColumnResizeMode="Control" DataSourceID="LinqServerModeDataSource1Count"
                        Settings-VerticalScrollableHeight="237" SettingsBehavior-AllowSelectByRowClick="true"
                        Settings-VerticalScrollBarMode="Auto" Theme="PlasticBlue"
                        Settings-ShowFilterRow="true" Settings-ShowFilterRowMenu="true">
                           <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
                        <SettingsBehavior EnableCustomizationWindow="true"   />
                                        <Settings ShowFooter="true"  /> 
                                        <SettingsContextMenu Enabled="true" />    
                          <ClientSideEvents Init="grid_Initcount" BeginCallback="grid_BeginCallbackcount" EndCallback="grid_EndCallbackcount" />
       
                            <Columns>

                                  <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Employee Name" FieldName="Name" Width="70%">
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>


                                   <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Punch Count" FieldName="count" Width="30%">
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>

                  

                            </Columns>

            
                               <Settings GridLines="Vertical" />
                                <SettingsBehavior AllowDragDrop="false" />
                                <SettingsPager Mode="ShowAllRecords" />
                              <SettingsLoadingPanel Mode="ShowOnStatusBar" />
          </dx:ASPxGridView>
                    </div>                        
                    <div class="h2">
                        In-Out Count
                    </div>
                    </div>

                </div>
               <%-- </div>

                <div class="clearfix">--%>
                <div class="col-md-4" runat="server" id="AttendanceDet">
                 <div class="PanelMainDiv">
                     <div class="PopupDivDown">

                         <table style="width:100%">
                             <tr>
                                 <td style="width:36%">
                                   Attendance For Last
                                 </td>
                                 <td style="width:20%;padding-left: 10px;">
                                      <dx:ASPxTextBox ID="txtUpdateDays" ClientInstanceName="ctxtUpdateDays" runat="server" 
                                          Width="100%" ValidationSettings-Display="Dynamic" HorizontalAlign="Right">
                                                                    <MaskSettings Mask="&lt;0..365&gt;" AllowMouseWheel="false" />
                                                                    </dx:ASPxTextBox>
                                 </td>
                                 <td style="width:55%;padding-left: 10px;">
                                     Day(s)
                                 </td>
                                 <td style="width:20%">
                                     <button type="button" class="btn btn-primary btn-sm" onclick="GenerateLastdaysReport()" >Generate</button>
                                 </td>
                             </tr>
                         </table>
                        <div id="performanceChrtDiv" style="min-height:305px">

                        </div>


                    </div>
                 </div>
                </div>
               <%-- </div>--%>


           <%-- </div>--%>
            <div class="col-md-4 mBot10" runat="server" id="RecentAtt">
                <div class="PanelMainDiv">
                     <div class="PopupDiv">
                        <dx:ASPxGridView ID="gridActHis" runat="server" ClientInstanceName="cgridActHis" KeyFieldName="Emp_InternalId"
            Width="100%" Settings-HorizontalScrollBarMode="Auto"
            SettingsBehavior-ColumnResizeMode="Control" DataSourceID="EntityServerModeActHis"
            Settings-VerticalScrollableHeight="237" SettingsBehavior-AllowSelectByRowClick="true"
            Settings-VerticalScrollBarMode="Auto" Theme="PlasticBlue"
            Settings-ShowFilterRow="true" Settings-ShowFilterRowMenu="true">
               <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
            <SettingsBehavior EnableCustomizationWindow="true"   />
                            <Settings ShowFooter="true"  /> 
                            <SettingsContextMenu Enabled="true" />    
              <ClientSideEvents Init="grid_Init" BeginCallback="grid_BeginCallback" EndCallback="grid_EndCallback" />
       
            <Columns>

                  <dx:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Employee Name" FieldName="Name" Width="70%">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>


                  <dx:GridViewDataTimeEditColumn Caption="First Punch" FieldName="In_Time" Width="30%" SortOrder="Descending"
                     PropertiesTimeEdit-DisplayFormatString="h:mm:ss tt" 
                    PropertiesTimeEdit-EditFormatString="h:mm:ss tt">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTimeEditColumn>

                  

            </Columns>

            
               <Settings GridLines="Vertical" />
                <SettingsBehavior AllowDragDrop="false" />
                <SettingsPager Mode="ShowAllRecords" />
              <SettingsLoadingPanel Mode="ShowOnStatusBar" />
          </dx:ASPxGridView>
                    </div>                        
                    <div class="h2">
                        Recent Attendance
                    </div>
                    </div>
            </div>
        </div>
 
        
        
        
        
        

         


          



           <dx:LinqServerModeDataSource ID="EntityServerModeActHis" runat="server" OnSelecting="EntityServerModeActHis_Selecting" />
        <dx:LinqServerModeDataSource ID="LinqServerModeDataSource1Count" runat="server" OnSelecting="LinqServerModeDataSource1Count_Selecting" />









    </div>






        <div class="modal fade" id="latecomers" role="dialog">
    <div class="modal-dialog">

        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title">Late Comers</h4>
            </div>
            <div class="modal-body">

                <div style="width:100%" id="latecomeerDiv">
                    

                </div>
             


            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
            </div>
        </div>

    </div>
</div>



















    </form>
</body>
</html>
