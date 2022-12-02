<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerSelection.ascx.cs" Inherits="ERP.OMS.Management.UserForm.UserControl.CustomerSelection" %>
<script>
    function CustomerButnClick(s, e) {
        $('#CustModel').modal('show');
    }
    function CustomerKeyDown(s, e) {
        if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
            $('#CustModel').modal('show');
        }
    }

    function Customerkeydown(e) {

        var OtherDetails = {}
        OtherDetails.SearchKey = $("#txtCustSearch").val();

        if (e.code == "Enter" || e.code == "NumpadEnter") {
            var HeaderCaption = [];
            HeaderCaption.push("Customer Name");
            HeaderCaption.push("Unique Id");
            HeaderCaption.push("Address");
            if ($("#txtCustSearch").val() != '') {
                callonServer("../Activities/Services/Master.asmx/GetAllCustomer", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
            }
        }
        else if (e.code == "ArrowDown") {
            if ($("input[customerindex=0]"))
                $("input[customerindex=0]").focus();
        }

    }


   


    function SetCustomer(Id, Name) {
        if (Id) {
            $('#CustModel').modal('hide');
            ctxtCustName.SetText(Name);
            $('#HdCustId').val(Id);
        }
    }
</script>
<style>
    .CustomerspanHide + span {
    display:none;
    }
</style>

<asp:HiddenField ID="HdCustId" runat="server" />
<dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName"
   Width="100%" >
                                                             
    <Buttons>
        <dxe:EditButton>
        </dxe:EditButton>
                                                                 
    </Buttons>
    <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){CustomerKeyDown(s,e);}"/>
</dxe:ASPxButtonEdit>



 <!--Customer Modal -->
  <div class="modal fade CustomerspanHide" id="CustModel" role="dialog">
    <div class="modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title">Customer Search</h4>
        </div>
        <div class="modal-body">
           <input type="text" onkeydown="Customerkeydown(event)"  id="txtCustSearch" autofocus width="100%" placeholder="Search By Customer Name or Unique Id"/>
             
            <div id="CustomerTable">
                <table border='1' width="100%" class="dynamicPopupTbl">
                    <tr class="HeaderStyle">
                <th class="hide">id</th> <th>Customer Name</th><th>Unique Id</th><th>Address</th>
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


