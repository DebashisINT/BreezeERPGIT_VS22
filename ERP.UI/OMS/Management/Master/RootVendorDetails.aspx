<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="RootVendorDetails.aspx.cs" Inherits="ERP.OMS.Management.Master.RootVendorDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script type="text/javascript">

        FieldName = 'test';
        /*Code  Added  By Priti on 20122016 to use jquery Choosen*/
        $(document).ready(function () {
            ListBind();
            ChangeSource();

        });
        function ListBind() {

            var config = {
                '.chsn': {},
                '.chsn-deselect': { allow_single_deselect: true },
                '.chsn-no-single': { disable_search_threshold: 10 },
                '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
                '.chsn-width': { width: "100%" }
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }

        }
        function lstAssociatedEmployee() {

            $('#lstAssociatedEmployee').fadeIn();

        }
        function setvalue() {
            document.getElementById("txtReportTo_hidden").value = document.getElementById("lstAssociatedEmployee").value;

        }
        function Changeselectedvalue() {
            //alert(document.getElementById("txtReportTo_hidden").value);
            var lstAssociatedEmployee = document.getElementById("lstAssociatedEmployee");
            if (document.getElementById("txtReportTo_hidden").value != '') {
                for (var i = 0; i < lstAssociatedEmployee.options.length; i++) {
                    if (lstAssociatedEmployee.options[i].value == document.getElementById("txtReportTo_hidden").value) {
                        lstAssociatedEmployee.options[i].selected = true;
                    }
                }
                $('#lstAssociatedEmployee').trigger("chosen:updated");
            }

        }
        function ChangeSource() {
            var fname = "%";
            var lAssociatedEmployee = $('select[id$=lstAssociatedEmployee]');
            lAssociatedEmployee.empty();


            $.ajax({
                type: "POST",
                url: "RootVendorDetails.aspx/ALLEmployee",
                data: JSON.stringify({ reqStr: fname }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {
                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];

                            $('#lstAssociatedEmployee').append($('<option>').text(name).val(id));

                        }

                        $(lAssociatedEmployee).append(listItems.join(''));

                        lstAssociatedEmployee();
                        $('#lstAssociatedEmployee').trigger("chosen:updated");

                        Changeselectedvalue();

                    }
                    else {
                        //   alert("No records found");
                        //lstReferedBy();
                        $('#lstAssociatedEmployee').trigger("chosen:updated");

                    }
                }
            });
            // }
        }
        //....end......
        function CallList(obj1, obj2, obj3) {
            //var obj5 = '';
            //if (obj5 != '18') {
            //    ajax_showOptions(obj1, obj2, obj3, obj5);
            //}
            if (obj1.value == "") {
                obj1.value = "%";
            }
            var obj5 = '';
            if (obj5 != '18') {
                ajax_showOptionsTEST(obj1, obj2, obj3, obj5);
                if (obj1.value == "%") {
                    obj1.value = "";
                }
            }
        }
        function Close() {
            parent.editwin.close();
        }
        function Cancel_Click() {
            //parent.editwin.close();
            location.href = '/OMS/Management/Master/root_user.aspx';
        }
    </script>

    <script type="text/javascript" language="javascript">
        // WRITE THE VALIDATION SCRIPT IN THE HEAD TAG.
        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
                return false;

            return true;
        }
    </script>
    <style>
        .reltv {
            position:relative;
        }
        .spl {
            position:absolute;
            right: -17px;
            top: 5px;
        }
        .inb {
            display:inline-block !important;
            width:62px !important;
        }
         /*Code  Added  By Priti on 20122016 to use jquery Choosen*/
         .chosen-container.chosen-container-single {
            width:100% !important;
        }
        .chosen-choices {
            width:100% !important;
        }
        #lstAssociatedEmployee {
            width:100%;
            display:none !important;
        }
       
        #display:none !important;_chosen{
            width:100% !important;
        }
        .dxtcLite_PlasticBlue > .dxtc-content {
            overflow:visible !important
        }
        /*...end....*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Add/Edit Vendor </h3>
        </div>
        <div class="crossBtn"><a href="root_VendorUser.aspx"><i class="fa fa-times"></i></a></div>
    </div>
    <div class="form_main">
        <div class="row">
            </div>
        <div class="row">
            <div class="col-md-3">
                <label>User Name&nbsp;<em style="color: red">*</em> :</label>
                <div Class="reltv">
                <asp:TextBox ID='txtusername' runat="server" Width="100%" ValidationGroup="a" MaxLength="50" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvuserName" runat="server" ControlToValidate="txtusername" CssClass="spl fa fa-exclamation-circle iconRed"
                    ErrorMessage="" ValidationGroup="a" ToolTip="Mandatory."></asp:RequiredFieldValidator>

                </div>
            </div>
            <div class="col-md-3">
                <label>Login Id&nbsp;<em style="color: red">*</em> :</label>
                <div Class="reltv">
                <asp:TextBox ID='txtloginid' runat="server" Width="100%" ValidationGroup="a" value=" " MaxLength="50" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvLiginId" runat="server" ControlToValidate="txtloginid" CssClass="spl fa fa-exclamation-circle iconRed"
                    ErrorMessage="" ToolTip="Mandatory." ValidationGroup="a"></asp:RequiredFieldValidator></div>
            </div>
            <div class="col-md-3" id="user_password" runat="server" visible="true">
                <label>Password&nbsp;<em style="color: red">*</em> :</label>
                <div Class="reltv">
                    <asp:TextBox ID='txtpassword' runat="server" Width="100%" TextMode="Password" ValidationGroup="a"  MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPass" runat="server" ControlToValidate="txtpassword" CssClass="spl fa fa-exclamation-circle iconRed"
                        ErrorMessage="" ToolTip="Mandatory." ValidationGroup="a"></asp:RequiredFieldValidator></div>
                <div ></div>
            </div>
            <div class="col-md-3">
                 <label>Associated Vendor <em style="color: red">*</em> :</label>
                <div Class="reltv"> 
                  
                    <asp:ListBox ID="lstAssociatedEmployee" CssClass="chsn"   runat="server"  Width="100%"   data-placeholder="Select..."></asp:ListBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="lstAssociatedEmployee" CssClass="spl fa fa-exclamation-circle iconRed"
                        ErrorMessage="" ToolTip="Mandatory." ValidationGroup="a"></asp:RequiredFieldValidator>

                    <asp:HiddenField ID="txtReportTo_hidden" runat="server" />
                </div>
            </div>
            <div class="clear"></div>
           
         
            <div class="col-md-3">
                <label>Inactive?</label>
                <div>
                    <dxe:ASPxCheckBox ID="chkIsActive" runat="server" Text="">
                    </dxe:ASPxCheckBox>
                </div>
            </div>
            <div class="clear"></div>
            <div class="col-md-12">
                <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnUpdate_Click" OnClientClick="setvalue()"
                    ValidationGroup="a" />
                <input id="btnCancel" type="button" class="btn btn-danger" value="Cancel" onclick="Cancel_Click()" />
            </div>
        </div>
  
    </div>
</asp:Content>
