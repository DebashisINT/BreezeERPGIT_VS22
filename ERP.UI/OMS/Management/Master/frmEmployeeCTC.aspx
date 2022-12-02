<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Master.management_master_frmEmployeeCTC"
    CodeBehind="frmEmployeeCTC.aspx.cs" EnableEventValidation="false" %>

<%--<%@ register assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--___________________These files are for List Items__________________________-->
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script language="javascript" type="text/javascript">
        /*Code  Added  By Priti on 06122016 to use jquery Choosen*/
        $(document).ready(function () {
            ListBind();
            ChangeSource();

            $("#btnSave").click(function () {
                if (document.getElementById("txtReportTo_hidden").value != '') {
                    $('#redlstReportTo').attr('style', 'display:none');
                    return true;
                }
                else
                {
                    $('#redlstReportTo').attr('style', 'display:block');
                    return false;
                }

            });
            
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
        function lstReportTo() {

            $('#lstReportTo').fadeIn();
            $('#lstColleague').fadeIn();
            $('#lstReportHead').fadeIn();
        }
        function setvalue() {
            document.getElementById("txtReportTo_hidden").value = document.getElementById("lstReportTo").value;
            document.getElementById("txtColleague_hidden").value = document.getElementById("lstColleague").value;
            document.getElementById("txtAReportHead_hidden").value = document.getElementById("lstReportHead").value;

            if($("#txtCTC").val()=="")
            {
                $("#txtCTC").val(0);
            }

            if ($("#txtBasic").val() == "") {
                $("#txtBasic").val(0);
            }

            if ($("#txtHRA").val() == "") {
                $("#txtHRA").val(0);
            }

            if ($("#txtCCA").val() == "") {
                $("#txtCCA").val(0);
            }

            if ($("#txtSpAl").val() == "") {
                $("#txtSpAl").val(0);
            }

            if ($("#txtChAL").val() == "") {
                $("#txtChAL").val(0);
            }

            if ($("#txtPf").val() == "") {
                $("#txtPf").val(0);
            }

            if ($("#txtMedAl").val() == "") {
                $("#txtMedAl").val(0);
            }
            if ($("#txtLTA").val() == "") {
                $("#txtLTA").val(0);
            }
            if ($("#txtConvence").val() == "") {
                $("#txtConvence").val(0);
            }
            if ($("#txtMbAl").val() == "") {
                $("#txtMbAl").val(0);
            }

        }
        function Changeselectedvalue() {
            var lstReportTo = document.getElementById("lstReportTo");
            if (document.getElementById("txtReportTo_hidden").value != '') {
                for (var i = 0; i < lstReportTo.options.length; i++) {
                    if (lstReportTo.options[i].value == document.getElementById("txtReportTo_hidden").value) {
                        lstReportTo.options[i].selected = true;
                    }
                }
                $('#lstReportTo').trigger("chosen:updated");
            }

        }
        function ChangeselectedvalueColleague() {
            var lstColleague = document.getElementById("lstColleague");
            if (document.getElementById("txtColleague_hidden").value != '') {
                for (var i = 0; i < lstColleague.options.length; i++) {
                    if (lstColleague.options[i].value == document.getElementById("txtColleague_hidden").value) {
                        lstColleague.options[i].selected = true;
                    }
                }
                $('#lstColleague').trigger("chosen:updated");
            }

        }
        function ChangeselectedvalueReportHead() {
            var lstReportHead = document.getElementById("lstReportHead");
            if (document.getElementById("txtAReportHead_hidden").value != '') {
                for (var i = 0; i < lstReportHead.options.length; i++) {
                    if (lstReportHead.options[i].value == document.getElementById("txtAReportHead_hidden").value) {
                        lstReportHead.options[i].selected = true;
                    }
                }
                $('#lstReportHead').trigger("chosen:updated");
            }

        }
        function ChangeSource() {
            var fname = "%";
            var lReportTo = $('select[id$=lstReportTo]');
            lReportTo.empty();

            var lColleague = $('select[id$=lstColleague]');
            lColleague.empty();

            var lReportHead = $('select[id$=lstReportHead]');
            lReportHead.empty();

            $.ajax({
                type: "POST",
                url: "frmEmployeeCTC.aspx/GetEmpCTC",
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

                            $('#lstReportTo').append($('<option>').text(name).val(id));
                            $('#lstColleague').append($('<option>').text(name).val(id));
                            $('#lstReportHead').append($('<option>').text(name).val(id));
                        }

                        $(lReportTo).append(listItems.join(''));
                        $(lColleague).append(listItems.join(''));
                        $(lReportHead).append(listItems.join(''));
                        lstReportTo();
                        $('#lstReportTo').trigger("chosen:updated");
                        $('#lstColleague').trigger("chosen:updated");
                        $('#lstReportHead').trigger("chosen:updated");
                        Changeselectedvalue();
                        ChangeselectedvalueColleague();
                        ChangeselectedvalueReportHead();
                    }
                    else {
                        //   alert("No records found");
                        //lstReferedBy();
                        $('#lstReportTo').trigger("chosen:updated");
                        $('#lstColleague').trigger("chosen:updated");
                        $('#lstReportHead').trigger("chosen:updated");
                    }
                }
            });
            // }
        }

        //.................code end.......
        FieldName = 'btnSave';
        function CallList(obj1, obj2, obj3) {
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

        function OnCloseButtonClick(s, e) {

            var parentWindow = window.parent;

            parentWindow.popup.Hide();

        }


        //Validation For CTC
        function ValidateCTC() {
            //  alert('hhhh');

            if (document.getElementById("cmbOrganization").value == "0") {
                alert('Please Select  Organization.');
                return false;
            }

            else if (document.getElementById("cmbJobResponse").value == "0") {
                alert('Please Select Job Responsibility.');
                return false;
            }
            else if (document.getElementById("cmbBranch").value == "0") {
                alert('Please Select Branch.');
                return false;
            }
            else if (document.getElementById("cmbDesg").value == "0") {
                alert('Please Select Designation.');
                return false;
            }
            else if (document.getElementById("EmpType").value == "0") {
                alert('Please Select Employee Type.');
                return false;
            }
            else if (document.getElementById("cmbDept").value == "0") {
                alert('Please Select Employee Dept.');
                return false;
            }
            else if (document.getElementById("txtReportTo_hidden").value == '') {
                alert('Please Select Reporting Head.');
                return false;
            }

            else if (JoiningDate.GetText() == '01-01-0100' || cmbLeaveEff.GetText() == '01-01-1900' || cmbLeaveEff.GetText() == '' || cmbLeaveEff.GetText() == '01010100') {
                alert('Joining Date is Required!.');
                return false;
            }


        }
        //


        function MaskMoney(evt) {
            if (!(evt.keyCode == 46 || (evt.keyCode >= 48 && evt.keyCode <= 57))) return false;
            var parts = evt.srcElement.value.split('.');

            if (parts.length > 2) return false;
            if (evt.keyCode == 46) return (parts.length == 1);
            if (parts[0].length >= 14) return false;
            if (parts.length == 2 && parts[1].length >= 2) return false;
        }

        function MaskMoneyDecimal(evt) {
            if (!(evt.keyCode == 46 || (evt.keyCode >= 48 && evt.keyCode <= 57))) return false;
            var parts = evt.srcElement.value.split('.');

            if (parts.length > 2) return false;
            if (evt.keyCode == 46) return (parts.length == 1);
            if (parts[0].length >= 9) return false;
            if (parts.length == 2 && parts[1].length >= 2) return false;
        }
        function OncmbLeaveEffChange() {
            var DOJ = document.getElementById("Hidden_DOJ").value;
            var AlreadyAssignedDate = document.getElementById("Hidden_LEF").value;
            var NewDate = cCmbLeaveEff.GetDate();
            CompareDate(DOJ, NewDate, "LE", "Selected Date Can not Be Less Than DOJ Date!!!", cCmbLeaveEff, AlreadyAssignedDate);
        }
        function OnJoiningDateChange() {
            var DOJ = document.getElementById("Hidden_DOJ").value;
            var AlreadyAssignedDate = document.getElementById("Hidden_CTCAppFrom").value;
            var NewDate = cJoiningDate.GetDate();
            CompareDate(DOJ, NewDate, "LE", "Selected Date Can not Be Less Than DOJ Date!!!", cJoiningDate, AlreadyAssignedDate);
        }

    </script>
    <style>
        .pullrightClass {
            position: absolute;
            right: 47px;
            width: 15px;
            height: 15px;
            top: 64px;
            color: #DF3636;
            font-size: 15px;
        }

        .r59 {
            top: 144px;
            right: 77px;
            font-size: 12px;
        }

        .r591 {
            top: 75px;
            right: 77px;
            font-size: 12px;
        }

        .ctcclass {
            position: absolute;
            right: -7px;
            top: 34px;
        }


        /*Code  Added  By Priti on 06122016 to use jquery Choosen*/
        .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }

        #lstReportTo {
            width: 200px;
        }

        #lstReportTo {
            display: none !important;
        }

        #lstReportTo_chosen {
            width: 100% !important;
        }

        .dxtcLite_PlasticBlue > .dxtc-content {
            overflow: visible !important;
        }

        .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }

        #lstColleague {
            width: 200px;
        }

        #lstColleague {
            display: none !important;
        }

        #lstColleague_chosen {
            width: 100% !important;
        }

        .dxtcLite_PlasticBlue > .dxtc-content {
            overflow: visible !important;
        }

        #lstReportHead {
            width: 200px;
        }

        #lstReportHead {
            display: none !important;
        }

        #lstReportHead_chosen {
            width: 100% !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Employee CTC</h3>
            <div class="crossBtn"><a href="Employee_EmployeeCTC.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
    <div class="form_main">
        <div>
            <div class="col-md-3 relative">
                <label>CTC Applicable From:<span style="color: red">*</span></label>
                <div style="position: relative">
                    <dxe:ASPxDateEdit ID="JoiningDate" ClientInstanceName="cJoiningDate" runat="server" DateOnError="Today" EditFormat="Custom"
                        TabIndex="0" Width="100%">
                        <clientsideevents datechanged="OnJoiningDateChange" />

                    </dxe:ASPxDateEdit>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ValidationGroup="a" runat="server" ControlToValidate="JoiningDate"
                        CssClass="fa fa-exclamation-circle ctcclass " ToolTip="Mandatory."
                        ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                </div>
            </div>
            <div class="col-md-3 relative">
                <label>Organization<span style="color: red">*</span></label>
                <div>
                    <asp:DropDownList ID="cmbOrganization" runat="server" Width="100%" TabIndex="1" Enabled="false">
                    </asp:DropDownList>

                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" InitialValue="0" ValidationGroup="a" runat="server" ToolTip="Mandatory."
                        CssClass=" fa fa-exclamation-circle ctcclass" SetFocusOnError="true" ControlToValidate="cmbOrganization" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="col-md-3 relative">
                <label>Job Responsibility<span style="color: red">*</span></label>
                <div>
                    <asp:DropDownList ID="cmbJobResponse" runat="server" Width="100%" TabIndex="2">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" InitialValue="0" ValidationGroup="a" runat="server" SetFocusOnError="true" ToolTip="Mandatory."
                        ControlToValidate="cmbJobResponse" ForeColor="Red" CssClass=" fa fa-exclamation-circle ctcclass "></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="col-md-3 relative">
                <label>Branch<span style="color: red">*</span></label>
                <div>
                    <asp:DropDownList ID="cmbBranch" runat="server" Width="100%" TabIndex="3">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" InitialValue="0" ValidationGroup="a" runat="server" SetFocusOnError="true" ToolTip="Mandatory."
                        CssClass=" fa fa-exclamation-circle ctcclass" ControlToValidate="cmbBranch" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="clear"></div>
            <div class="col-md-3 relative">
                <label>Designation<span style="color: red">*</span></label>
                <div>
                    <asp:DropDownList ID="cmbDesg" runat="server" Width="100%" TabIndex="4">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" InitialValue="0" CssClass=" fa fa-exclamation-circle ctcclass" ValidationGroup="a" ToolTip="Mandatory."
                        runat="server" SetFocusOnError="true" ControlToValidate="cmbDesg" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="col-md-3 relative">
                <label>Employee Type<span style="color: red">*</span></label>
                <div>
                    <asp:DropDownList ID="EmpType" runat="server" Width="100%" TabIndex="5" Enabled="false">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorEType1" CssClass=" fa fa-exclamation-circle ctcclass" ToolTip="Mandatory."
                        InitialValue="0" ValidationGroup="a" runat="server" SetFocusOnError="true" ControlToValidate="EmpType" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div id="divContactType" class="col-md-3 relative" runat="server" visible="false">
                <asp:Label ID="lblContactType" runat="server"></asp:Label>
                <div>
                    <asp:DropDownList ID="ddlContactType" runat="server" Width="100%" TabIndex="5">
                        <asp:ListItem Value="" Text="--Select--"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-3 relative">
                <label>Department<span style="color: red">*</span></label>
                <div>
                    <asp:DropDownList ID="cmbDept" runat="server" Width="100%" TabIndex="6">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" CssClass=" fa fa-exclamation-circle ctcclass" InitialValue="0" ToolTip="Mandatory."
                        ValidationGroup="a" runat="server" SetFocusOnError="true" ControlToValidate="cmbDept" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="col-md-3">
                <label>Basic</label>
                <div>
                    <asp:TextBox ID="txtBasic" runat="server" Width="100%" TabIndex="7"></asp:TextBox>
                </div>
            </div>
            <div class="clear"></div>
            <div class="col-md-3 relative">
                <label>Report To<span style="color: red">*</span></label>
                <div>
                    <asp:ListBox ID="lstReportTo" CssClass="chsn" runat="server" Width="100%" TabIndex="8" data-placeholder="Select..."></asp:ListBox>
                    <%--<asp:TextBox ID="txtReportTo" runat="server" Width="300px" TabIndex="8"></asp:TextBox>--%>
                  <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator1123" ValidationGroup="a" runat="server" ControlToValidate="lstReportTo"
                        CssClass=" fa fa-exclamation-circle ctcclass" ToolTip="Mandatory."
                        ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                  <span id="redlstReportTo" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute; top:118px; left: 400px; display:none" title="Mandatory"></span>

                    <asp:HiddenField ID="txtReportTo_hidden" runat="server" />
                </div>
            </div>
            <div class="col-md-3">
                <label>CCA</label>
                <div>
                    <asp:TextBox ID="txtCCA" runat="server" Width="100%" TabIndex="9"></asp:TextBox>
                </div>
            </div>

            <div class="col-md-3">
                <label>Additional Reporting Head</label>
                <div>
                    <asp:ListBox ID="lstReportHead" CssClass="chsn" runat="server" Width="100%" TabIndex="10" data-placeholder="Select..."></asp:ListBox>
                    <%--  <asp:TextBox ID="txtAReportHead" runat="server" Width="300px" TabIndex="10"></asp:TextBox>--%>
                    <asp:HiddenField ID="txtAReportHead_hidden" runat="server" />
                </div>
            </div>
            <div class="col-md-3">
                <label>Colleague</label>
                <div>
                    <asp:ListBox ID="lstColleague" CssClass="chsn" runat="server" Width="100%" TabIndex="11" data-placeholder="Select..."></asp:ListBox>
                    <%--<asp:TextBox ID="txtColleague" runat="server" Width="300px" TabIndex="11"></asp:TextBox>--%>
                    <asp:HiddenField ID="txtColleague_hidden" runat="server" />
                </div>
            </div>
            <div class="clear"></div>
            <div class="col-md-3">
                <label>Current CTC</label>
                <div>
                    <asp:TextBox ID="txtCTC" runat="server" Width="100%" TabIndex="12"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-3">
                <label>HRA</label>
                <div>
                    <asp:TextBox ID="txtHRA" runat="server" Width="100%" TabIndex="13"></asp:TextBox></div>
            </div>
            <div class="col-md-3">
                <label>SP. Allowance</label>
                <div>
                    <asp:TextBox ID="txtSpAl" runat="server" Width="100%" TabIndex="14"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-3">
                <label>Children Allowance</label>
                <div>
                    <asp:TextBox ID="txtChAL" runat="server" Width="100%" TabIndex="15"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-3">
                <label>PF</label>
                <div>
                    <asp:TextBox ID="txtPf" runat="server" Width="100%" TabIndex="16"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-3">
                <label>Medical Allowance</label>
                <div>
                    <asp:TextBox ID="txtMedAl" runat="server" Width="100%" TabIndex="17"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-3">
                <label>LTA</label>
                <div>
                    <asp:TextBox ID="txtLTA" runat="server" Width="100%" TabIndex="18"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-3">
                <label>Conveyance</label>
                <div>
                    <asp:TextBox ID="txtConvence" runat="server" Width="100%" TabIndex="19"></asp:TextBox>
                </div>
            </div>

            <div class="col-md-3">
                <label>
                    Mobile Phone Expenses
                </label>
                <div>
                    <asp:TextBox ID="txtMbAl" runat="server" Width="100%" TabIndex="20"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-3">
                <label>
                    Car Allowance
                </label>
                <div>
                    <asp:TextBox ID="txtCarAl" runat="server" Width="100%" TabIndex="21"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-3">
                <label>
                    Uniform Allowance
                </label>
                <div>
                    <asp:TextBox ID="txtUniform" runat="server" Width="100%" TabIndex="22"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-3">
                <label>
                    Books Periodicals Allowance
                </label>
                <div>
                    <asp:TextBox ID="txtBook" runat="server" Width="100%" TabIndex="23"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-3">
                <label>
                    Seminar Allowance
                </label>
                <div>
                    <asp:TextBox ID="txtSeminar" runat="server" Width="100%" TabIndex="24"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-3">
                <label>
                    Other Allowance
                </label>
                <div>
                    <asp:TextBox ID="txtOther" runat="server" Width="100%" TabIndex="25"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-3 relative">
                <label>
                    Working Hour<span style="color: red">*</span>
                </label>
                <div>
                    <asp:DropDownList ID="cmbWorkingHr" runat="server" Width="100%" TabIndex="26">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass=" fa fa-exclamation-circle ctcclass" ForeColor="Red" runat="server" ToolTip="Mandatory."
                        ErrorMessage="" InitialValue="0" SetFocusOnError="true" ControlToValidate="cmbWorkingHr" ValidationGroup="a"></asp:RequiredFieldValidator>

                </div>
            </div>
            <div class="col-md-3 relative">
                <label>Leave Policy<span style="color: red">*</span></label>
                <div>
                    <asp:DropDownList ID="cmbLeaveP" runat="server" Width="100%" TabIndex="27">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass=" fa fa-exclamation-circle ctcclass" ForeColor="Red" runat="server" ToolTip="Mandatory."
                        ErrorMessage="" InitialValue="0" ControlToValidate="cmbLeaveP" SetFocusOnError="true" ValidationGroup="a"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="col-md-3">
                <label>Leave Effective From</label>
                <div>
                    <dxe:ASPxDateEdit ID="cmbLeaveEff" ClientInstanceName="cCmbLeaveEff" runat="server" DateOnError="Today" EditFormat="Custom"
                        TabIndex="28" Width="100%">
                        <clientsideevents datechanged="OncmbLeaveEffChange" />
                    </dxe:ASPxDateEdit>
                </div>
                <div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="cmbLeaveP"
                        ErrorMessage="Required." Width="100%"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="col-md-3">
                <label>Remarks</label>
                <div>
                    <asp:TextBox ID="txtRemarks" runat="server" Width="100%" Height="60px" TabIndex="29" MaxLength="4000"></asp:TextBox>
                </div>
            </div>
            <div style="clear: both"></div>
            <div style="padding-left: 15px;">
                <dxe:ASPxButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="a"
                    TabIndex="30" CssClass="btn btn-primary">
                    <clientsideevents click="function(s,e){
                                                    setvalue()}" />
                </dxe:ASPxButton>
                <dxe:ASPxButton ID="btnCancel" runat="server" AutoPostBack="false" Text="Cancel" VerticalAlign="Bottom" OnClick="btnCancel_Click"
                    CssClass="btn btn-danger" TabIndex="31">
                    <%--  <ClientSideEvents Click="OnCloseButtonClick" />--%>
                </dxe:ASPxButton>
            </div>
        </div>

        <asp:HiddenField ID="Hidden_DOJ" runat="server" />
        <asp:HiddenField ID="Hidden_LEF" runat="server" />
        <asp:HiddenField ID="Hidden_CTCAppFrom" runat="server" />
    </div>
</asp:Content>
