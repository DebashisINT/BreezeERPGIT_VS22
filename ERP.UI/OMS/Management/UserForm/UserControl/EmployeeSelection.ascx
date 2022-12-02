<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmployeeSelection.ascx.cs" Inherits="ERP.OMS.Management.UserForm.UserControl.EmployeeSelection" %>
<script>
    function EmployeeButnClick(s, e) { 
        $('#EmployeeModel').modal('show');
    }
    function EmployeeKeyDown(s, e) {
        if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
            $('#EmployeeModel').modal('show');
        }
    }

    function Employeekeydown(e) {

        var OtherDetails = {}
        OtherDetails.SearchKey = $("#txtEmployeeSearch").val();

        if (e.code == "Enter" || e.code == "NumpadEnter") {
            var HeaderCaption = [];
            HeaderCaption.push("Employee Name");
            HeaderCaption.push("Unique Id"); 
            if ($("#txtEmployeeSearch").val() != '') {
                callonServer("../Activities/Services/Master.asmx/GetEmployee", OtherDetails, "EmployeeTable", HeaderCaption, "EmployeeIndex", "SetEmployee");
            }
        }
        else if (e.code == "ArrowDown") {
            if ($("input[Employeeindex=0]"))
                $("input[Employeeindex=0]").focus();
        }

    }


     


    function SetEmployee(Id, Name) {
        if (Id) {
            $('#EmployeeModel').modal('hide');
            ctxtEmployeeName.SetText(Name);
            $('#HdEmpId').val(Id);
        }
    }
</script>
<style>
    .EmployeespanHide + span {
    display:none;
    }
</style>

<asp:HiddenField ID="HdEmpId" runat="server" />
<dxe:ASPxButtonEdit ID="txtEmployeeName" runat="server" ReadOnly="true" ClientInstanceName="ctxtEmployeeName"
   Width="100%" >
                                                             
    <Buttons>
        <dxe:EditButton>
        </dxe:EditButton>
                                                                 
    </Buttons>
    <ClientSideEvents ButtonClick="function(s,e){EmployeeButnClick();}" KeyDown="function(s,e){EmployeeKeyDown(s,e);}"/>
</dxe:ASPxButtonEdit>



 <!--Customer Modal -->
  <div class="modal fade EmployeespanHide" id="EmployeeModel" role="dialog">
    <div class="modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title">Employee Search</h4>
        </div>
        <div class="modal-body">
           <input type="text" onkeydown="Employeekeydown(event)"  id="txtEmployeeSearch" autofocus width="100%" placeholder="Search By Employee Name or Unique Id"/>
             
            <div id="EmployeeTable">
                <table border='1' width="100%" class="dynamicPopupTbl">
                    <tr class="HeaderStyle">
                <th class="hide">id</th> <th>Employee Name</th><th>Unique Id</th> 
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

