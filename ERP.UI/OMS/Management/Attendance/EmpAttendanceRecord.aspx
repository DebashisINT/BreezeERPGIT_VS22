<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="EmpAttendanceRecord.aspx.cs" Inherits="ERP.OMS.Management.Attendance.EmpAttendanceRecord" %>

<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxScheduler" TagPrefix="dxwschs" %>
<%@ Register assembly="DevExpress.XtraScheduler.v15.1.Core, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraScheduler" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>

        .Rsquare {
	width: 10px;
	height: 10px;
	background: red;
    }
         .Gsquare {
	width: 10px;
	height: 10px;
	background: #66CC66;
    }
    .Wosquare {
	width: 10px;
	height: 10px;
	background: #CC6633;
    }

         .nuM.MakeRound {
    -webkit-transform: rotateY(150deg); /* Safari */
    transform: rotateY(360deg); /* Standard syntax */
    -webkit-transition:all 1s ease;
    transition:all 1s ease;
}
        .cddlYear {
        margin-bottom:0px !important;
        }
        .topTableri {
            float:right;
        }
        .topTableri>tbody>tr>td {
            padding-left:15px;
            text-align:right;
            padding-bottom:8px;
        }
        .setWtd {
            width:75px;
        }
        .setWtd> .btn {
            margin-right:0 !important;
        }
         .hPtop {
             /*padding-top: 19px;*/
                 font-size: 19px;
                 color:#006ac5;
         }
         .boxesNum {
             min-height:120px;
             border-left:1px solid #ccc;
             text-align:center;
             padding-top: 20px;
             margin-top: 10px;
         }
         .bxeNumber {
               display: inline-block;
                width: 100px;
                background: #263d90;
                padding: 6px 0;
                margin: 8px 0;
                color: #fff;
                font-size: 32px;
         }
         .boxesNum.two .bxeNumber {
             background: #2c6eb1;
             color:#fff
         }
         .boxesNum.three .bxeNumber {
             background: #38af9e;
             color:#fff
         }
         .boxesNum.four .bxeNumber {
                 background: #dde01c;
                color: #4c4949;
         }
         .boxesNum.five .bxeNumber {
             background: #651bb5;
             color:#fff
         }
         .boxesNum.six .bxeNumber {
             background: #f95a40;
             color:#fff
         }
         .statHead {
             text-transform:uppercase;
             margin:10px 0 8px 0; 
             font-size: 17px;
             color: #006ac5;
         }
         .roundedLi {
             width:100%;
             padding:0;
             margin-top:5px;
             text-align:center;
         }
         .roundedLi .nuM {
             width:80px;
             height:80px;
             text-align:center;
             background:#263d90;
             color:#fff;
             display: inline-block;
            font-size: 26px;
            font-weight: 700;
            line-height: 79px;
            border-radius: 50%;
            margin: 8px 0;
            
         }
         .roundedLi li {
             list-style-type:none;
             text-align:center;
                 margin-right: 13px;
                 margin-bottom:30px;
            display: inline-block;
         }
         .roundedLi li:nth-child(1) .nuM {
             background:#18B81C;
             color:#fff;
         }
         .roundedLi li:nth-child(2) .nuM {
             background:#FF3600;
             color:#fff;
         }
         .roundedLi li:nth-child(3) .nuM {
             background:#FF9000;
             color:#fff;
         }
         .roundedLi li:nth-child(4) .nuM {
             background:#00BAFF;
             color:#fff;
         }
         .roundedLi li:nth-child(5) .nuM {
             background:#B13173;
             color:#fff;
         }
         .roundedLi li:nth-child(6) .nuM {
             background:#234B0D;
             color:#fff;
         }
         .roundedLi li:nth-child(7) .nuM {
             background:#8A340A;
             color:#fff;
         }
         .roundedLi li:nth-child(8) .nuM {
             background:#5C0F00;
             color:#fff;
         }
         .roundedLi li:nth-child(9) .nuM {
             background:#8A610A;
             color:#fff;
         }
         .roundedLi li:nth-child(10) .nuM {
             background:#267D90;
             color:#fff;
         }
         /*.roundedLi li:nth-child(11) .nuM {
             background:#263d90;
             color:#fff;
         }
         .roundedLi li:nth-child(12) .nuM {
             background:#263d90;
             color:#fff;
         }*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Js/EmpAttendanceRecord.js?v=0.02"></script>
    <script src="../Activities/JS/SearchPopup.js"></script>
    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />

      <div class="panel-heading">
        <div class="panel-title">            
            
        </div>
     </div>

     <div class="">
         <div class="clearfix">
             <span class="pull-left hPtop">Attendance Statistics</span> 
             <table class="topTableri">
                 <tr>
                      <td><div class="Wosquare"></div></td>
                     <td style="padding-right: 20px;"> Week Off</td>
                     <td><div class="Gsquare"></div></td>
                     <td style="padding-right: 20px;"> Present</td>
                     <td><div class="Rsquare"></div></td>
                     <td style="padding-right: 20px;"> Absent</td>
                     <td style="color:#006ac5">Employee</td>
                     <td colspan="2">
                         <dxe:ASPxButtonEdit ID="empButtonEdit" ReadOnly="true" runat="server" ClientInstanceName="cempButtonEdit">
                            <Buttons>
                                <dxe:EditButton>
                                </dxe:EditButton>
                            </Buttons>
                            <ClientSideEvents ButtonClick="function(s,e){EmployeeSelect();}" KeyDown="function(s,e){EmployeeKeyDown(s,e);}" />
                        </dxe:ASPxButtonEdit>
                     </td>
                 <%--</tr>
                 <tr>--%>
                     <td style="color:#006ac5">Year</td>
                     <td>
                          <asp:DropDownList ID="ddlYear" runat="server" Width="100%" CssClass="cddlYear"> 
                              <asp:ListItem Text="2018" Value="2018"></asp:ListItem>
                              <asp:ListItem Text="2019" Value="2019"></asp:ListItem>
                               <asp:ListItem Text="2020" Value="2020"></asp:ListItem>
                          </asp:DropDownList>
                     </td>
                     <td class="setWtd"><button type="button" class="btn btn-success" id="BtnShow" onclick="ShowAttendance()">Show</button></td>
                 </tr>
             </table>
         </div>
          

          


    <dxe:ASPxGridView ID="GridFullYear" runat="server" ClientInstanceName="cGrid" KeyFieldName="YearMonth"
        OnCustomCallback="GridMonth_CustomCallback" OnDataBinding="GridFullYear_DataBinding" Width="100%"
        OnHtmlDataCellPrepared="GridFullYear_HtmlDataCellPrepared"  Settings-VerticalScrollBarMode="auto"
        ClientSideEvents-EndCallback="gridEndCallback"  Settings-VerticalScrollableHeight="250">



        <Columns>
            <dxe:GridViewDataTextColumn Caption="Month" Width="50" FieldName="YearMonthName"> 
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn Caption="S" Width="25" FieldName="Week1_1"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="M" Width="25" FieldName="Week2_1"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="T" Width="25" FieldName="Week3_1"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="W" Width="25" FieldName="Week4_1"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="T" Width="25" FieldName="Week5_1"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="F" Width="25" FieldName="Week6_1"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="S" Width="25" FieldName="Week7_1"> 
            </dxe:GridViewDataTextColumn>
            
            <%--WEEK 2--%>
            <dxe:GridViewDataTextColumn Caption="S" Width="25" FieldName="Week1_2"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="M" Width="25" FieldName="Week2_2"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="T" Width="25" FieldName="Week3_2"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="W" Width="25" FieldName="Week4_2"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="T" Width="25" FieldName="Week5_2"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="F" Width="25" FieldName="Week6_2"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="S" Width="25" FieldName="Week7_2"> 
            </dxe:GridViewDataTextColumn>
            
            <%--WEEK 3--%>
            <dxe:GridViewDataTextColumn Caption="S" Width="25" FieldName="Week1_3"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="M" Width="25" FieldName="Week2_3"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="T" Width="25" FieldName="Week3_3"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="W" Width="25" FieldName="Week4_3"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="T" Width="25" FieldName="Week5_3"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="F" Width="25" FieldName="Week6_3"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="S" Width="25" FieldName="Week7_3"> 
            </dxe:GridViewDataTextColumn>
            
            <%--WEEK 4--%>
             <dxe:GridViewDataTextColumn Caption="S" Width="25" FieldName="Week1_4"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="M" Width="25" FieldName="Week2_4"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="T" Width="25" FieldName="Week3_4"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="W" Width="25" FieldName="Week4_4"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="T" Width="25" FieldName="Week5_4"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="F" Width="25" FieldName="Week6_4"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="S" Width="25" FieldName="Week7_4"> 
            </dxe:GridViewDataTextColumn>

            <%--WEEK 5--%>
             <dxe:GridViewDataTextColumn Caption="S" Width="25" FieldName="Week1_5"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="M" Width="25" FieldName="Week2_5"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="T" Width="25" FieldName="Week3_5"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="W" Width="25" FieldName="Week4_5"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="T" Width="25" FieldName="Week5_5"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="F" Width="25" FieldName="Week6_5"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="S" Width="25" FieldName="Week7_5"> 
            </dxe:GridViewDataTextColumn>

            <%--WEEK 6--%>
             <dxe:GridViewDataTextColumn Caption="S" Width="25" FieldName="Week1_6"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="M" Width="20" FieldName="Week2_6"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="T" Width="20" FieldName="Week3_6"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="W" Width="20" FieldName="Week4_6"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="T" Width="20" FieldName="Week5_6"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="F" Width="20" FieldName="Week6_6"> 
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="S" Width="20" FieldName="Week7_6"> 
            </dxe:GridViewDataTextColumn>

          </Columns>
    </dxe:ASPxGridView>


         <asp:HiddenField ID="EmpId" runat="server" />

         <div class="clearfix">
             <h3 class="statHead">Key Statistics</h3>
             <div class="clearfix pull-left">
                 <ul class="roundedLi">
                     <li id="mainWorkingdays">
                         <div>Working Days</div> 
                       <span class="nuM" id="Workingdays">0</span>
                         <div >Last Year:<span id="lastWorkingdays">0</span></div>
                     </li>
                     <li id="mainco">
                         <div>Compensatory Off</div> 
                         <span class="nuM" id="co">0</span>
                         <div>Last Year:<span id="lastco">0</span></div>
                     </li>
                     <li id="mainhc">
                         <div>Half Day(Casual)</div> 
                         <span class="nuM" id ="hc">0</span>
                         <div>Last Year:<span id="lasthc">0</span></div>
                     </li>
                     <li id="mainHS">
                         <div>Half Day(Sick)</div> 
                         <span class="nuM" id="HS">0</span>
                         <div>Last Year:<span id="lastHS">0</span></div>
                     </li>
                     <li id="mainOV">
                         <div>Official Visit</div> 
                         <span class="nuM" id="OV">0</span>
                         <div>Last Year:<span id="lastOV">0</span></div>
                     </li>
                     <li id="mainPD">
                         <div>Personal Delay</div> 
                          <span class="nuM" id="PD">0</span>
                         <div>Last Year:<span id="lastPD">0</span></div>
                     </li>
                     <li id="mainPH">
                         <div>Paid holiday</div> 
                          <span class="nuM" id="PH">0</span>
                         <div>Last Year:<span id="lastPH">0</span></div>
                     </li>
                     <li id="mainPL">
                         <div>Privilege Leave</div> 
                         <span class="nuM" id="PL">0</span>
                         <div>Last Year:<span id="lastPL">0</span></div>
                     </li>
                     <li id="mainSL">
                         <div>Sick Leave</div> 
                        <span class="nuM" id="SL">0</span>
                         <div>Last Year:<span id="lastSL">0</span></div>
                     </li>
                     
                    <li id="mainOVOT">
                        <div>Official Visit -Outstation</div> 
                    <span class="nuM" id="OVOT">0</span>
                        <div>Last Year:<span id="lastOVOT">0</span></div>
                    </li>
                 </ul>
             </div>

            
         </div>
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
