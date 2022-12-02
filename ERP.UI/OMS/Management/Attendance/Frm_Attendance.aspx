<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Frm_Attendance.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Attendance.Frm_Attendance" EnableEventValidation="false"  EnableViewStateMac="false"   %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"> 
    <link href="Css/Attendance.css" rel="stylesheet" />
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Js/Attendance.js?v=0.4"></script>
    <script src="../Activities/JS/SearchPopup.js"></script>
      <link href="../Activities/CSS/SearchPopup.css?v=0.01" rel="stylesheet" />

    <div class="timeWraper"> 
        <div class="timeBox">
            <div class="line"></div>
            <div class="dateMnth" id="Day_span"></div>
            <div id="time_span"></div>

            <div class="btnWrap"> 
                <button type="button" class="btn btn-success" id="BtnShowEmployee" onclick="EmployeeSelect()"> <i class="fa fa-search"></i>Select Employee</button>
                <button type="button" class="btn btn-primary" id="BtnSubmitRequest" onclick="AttendanceSubmit()"><i class="fa fa-clock-o"></i> Time In/Out</button>
            </div>

            <div class="text-center" ><label class="label label-default employeeNameClass"><i class="fa fa-user"></i> <span id="EmployeeNameSpan"></span> </label></div>

        </div>
    </div>


    
    <span style="color: red;font-size: 13px;">* Date/Time of the Local PC. Attendance Time In/Out will be according to the Server Date/Time</span>


    <asp:HiddenField runat="server" id="EmpId"/>
    <asp:HiddenField ID="hdEmpName" runat="server" />










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
           <input type="text" onkeydown="Employeekeydown(event)"  id="txtEmpSearch" autofocus width="100%" placeholder="Search By Employee Name or Unique Id"
               
               />
             
            <div id="EmployeeTable">
                <table border='1' width="100%" class="dynamicPopupTbl">
                    <tr class="HeaderStyle">
                <th class="hide">id</th> <th>Employee Name</th><th>Employee Id</th>
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
