<%@ Page Title="Currency Master" Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="frm_currency_master.aspx.cs"  MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Master.frm_currency_master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script language="javascript" type="text/javascript">

        function maxLengthCheck(object) {
            if (object.value.length > object.maxLength)
                object.value = object.value.slice(0, object.maxLength)
        }

        function isNumeric(evt) {
            var theEvent = evt || window.event;
            var key = theEvent.keyCode || theEvent.which;
            key = String.fromCharCode(key);
            var regex = /[0-9]|\./;
            if (!regex.test(key)) {
                theEvent.returnValue = false;
                if (theEvent.preventDefault) theEvent.preventDefault();
            }
        }
        //Code for UDF Control 
        function OpenUdf() {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                var url = 'frm_BranchUdfPopUp.aspx?Type=FI';
                popup.SetContentUrl(url);
                popup.Show();
            }
            return true;
        }
        // End Udf Code

        function popUpRedirect(obj) {
            jAlert('Saved successfully');
            window.location.href = obj;


        }

        function Submited(obj) {
            jAlert('Saved successfully');
            var url = "frm_currency_master.aspx?id=" + obj;
             window.location.href = url;
        }

        $(document).ready(function () {
           
            cell1.innerHTML = "<select style='width:150px'>  " + document.getElementById("CurrencyHD").value + " </select>";
            loadExecutiveNameFromField();
        });


        function loadExecutiveNameFromField() {

            var table = document.getElementById("executiveTable");
            var exeName = document.getElementById('executiveName_hidden').value;
            for (var i = 0 ; i < exeName.split(',').length; i++) {
                var j = 0;
                if (exeName.split(',')[i].trim() != '') {
                    var values = exeName.split(',');
                    if (table.rows[0].cells[2].children[0].value.trim() != '') {
                        for (j = 0; j < values[i].toString().split('~').length; j++) {
                            var row = table.insertRow(0);
                            var cell1 = row.insertCell(0);
                            var cell2 = row.insertCell(1);
                            var cell3 = row.insertCell(2);
                            var cell4 = row.insertCell(3);
                            var cell5 = row.insertCell(4);                           
                            var replce = document.getElementById("CurrencyHD").value;
                           // replce = replce.replace("value='" + values[i].toString().split('~')[j] + "'", "value='" + values[i].toString().split('~')[j] + "' selected='selected'");
                            replce = replce.replace("value=" + values[i].toString().split('~')[j] + "", "value=" + values[i].toString().split('~')[j] + " selected='selected'");
                            cell1.innerHTML = "<select style='width:150px'>  " + replce + " </select>";                        

                            j++;
                            cell2.innerHTML = "<input class='flatpickr' 'type='text'  value='" + values[i].toString().split('~')[j] + "'/>";
                            //cell2.innerHTML = "<input type='date'  value='" + values[i].toString().split('~')[j] + "'/>";
                            j++;
                            cell3.innerHTML = "<input type='number' step='0.00001' maxlength = '18' onkeypress='return isNumeric(event)' oninput='maxLengthCheck(this)' value='" + values[i].toString().split('~')[j] + "'/>";
                            j++;
                            cell4.innerHTML = "<input type='number' step='0.00001' maxlength = '18' onkeypress='return isNumeric(event)' oninput='maxLengthCheck(this)' value='" + values[i].toString().split('~')[j] + "'/>";
                            j++;
                            if ('<%= edit %>' == 'True') {
                                cell5.innerHTML = "<button type='button' value='' onclick='AddNewexecutive()' class='green' style='display:none'><i class='fa fa-plus-circle'></i></button> <button type='button' value='' class='red' onclick='removeExecutive(this.parentNode.parentNode)' style='display:none'><i class='fa fa-times-circle'></i></button>";
                            }
                            else {
                                cell5.innerHTML = "<button type='button' value='' onclick='AddNewexecutive()' class='green'><i class='fa fa-plus-circle'></i></button> <button type='button' value='' class='red' onclick='removeExecutive(this.parentNode.parentNode)'><i class='fa fa-times-circle'></i></button>";
                            }

                            cell2.children[0].flatpickr({
                                enableTime: true,
                                weekNumbers: true

                            });
                            //cell1.innerHTML = "<input type='text' maxlength='50' value='" + exeName.split('~')[i] + "'/>";

                        }
                    }
                    else {
                        for (j = 0; j < values[i].toString().split('~').length; j++) {
                            table.rows[0].cells[0].children[0].value = exeName.split('~')[i];

                            var replce = document.getElementById("CurrencyHD").value;
                           // replce = replce.replace("value='" + values[i].toString().split('~')[j] + "'", "value='" + values[i].toString().split('~')[j] + "' selected='selected'");
                            replce = replce.replace("value=" + values[i].toString().split('~')[j] + "", "value=" + values[i].toString().split('~')[j] + " selected='selected'");
                            table.rows[0].cells[0].innerHTML = "<select style='width:150px'>  " + replce + " </select>";

                            j++;
                            //var date = values[i].toString().split('~')[j];
                            //date = date.split('-');


                            table.rows[0].cells[1].innerHTML = "<input class='flatpickr' 'type='text'  value='" + values[i].toString().split('~')[j] + "'/>";
                            j++;
                            table.rows[0].cells[2].innerHTML = "<input type='number' step='0.00001' maxlength = '18' onkeypress='return isNumeric(event)' oninput='maxLengthCheck(this)' value='" + values[i].toString().split('~')[j] + "' novalidate />";
                            j++;
                            table.rows[0].cells[3].innerHTML = "<input type='number' step='0.00001' maxlength = '18' onkeypress='return isNumeric(event)' oninput='maxLengthCheck(this)' value='" + values[i].toString().split('~')[j] + "'/>";
                            j++;
                            if ('<%= edit %>' == 'True') {
                                table.rows[0].cells[4].innerHTML = "<button type='button' value='' onclick='AddNewexecutive()' class='green' style='display:none'><i class='fa fa-plus-circle'></i></button> <button type='button' value='' class='red' onclick='removeExecutive(this.parentNode.parentNode)' style='display:none'><i class='fa fa-times-circle'></i></button>";
                            }
                            else {
                                table.rows[0].cells[4].innerHTML = "<button type='button' value='' onclick='AddNewexecutive()' class='green'><i class='fa fa-plus-circle'></i></button> <button type='button' value='' class='red' onclick='removeExecutive(this.parentNode.parentNode)'><i class='fa fa-times-circle'></i></button>";
                            }
                            //cell1.innerHTML = "<input type='text' maxlength='50' value='" + exeName.split('~')[i] + "'/>";
                            //cell1.children[0].flatpickr({
                            //    enableTime: true,
                            //    weekNumbers: true

                            //});
                        }
                    }


                }
            }

        }



        function AddNewexecutive() {
         
         
             var table = document.getElementById("executiveTable");
            var row = table.insertRow(0);
            var cell1 = row.insertCell(0);
            var cell2 = row.insertCell(1);
            var cell3 = row.insertCell(2);
            var cell4 = row.insertCell(3);
            var cell5 = row.insertCell(4);
            cell1.innerHTML = "<select style='width:150px'>  " + document.getElementById("CurrencyHD").value + " </select>";
            cell2.innerHTML = "<input class='flatpickr' type='text' placeholder='Select Date..'/>";
            cell3.innerHTML = "<input type='number' step='0.00001' maxlength = '18' onkeypress='return isNumeric(event)' oninput='maxLengthCheck(this)'/>";
            cell4.innerHTML = "<input type='number' step='0.00001' maxlength = '18' onkeypress='return isNumeric(event)' oninput='maxLengthCheck(this)'/>";
            cell5.innerHTML = "<button type='button' value='' onclick='AddNewexecutive()' class='green'><i class='fa fa-plus-circle'></i></button> <button type='button' value='' class='red' onclick='removeExecutive(this.parentNode.parentNode)'><i class='fa fa-times-circle'></i></button>";
            cell2.children[0].flatpickr({
                enableTime: true,
                weekNumbers: true

            });

        }
        function removeExecutive(obj) {
            var rowIndex = obj.rowIndex;
            var table = document.getElementById("executiveTable");
            if (table.rows.length > 1) {
                table.deleteRow(rowIndex);
            } else {
                jAlert('Cannot delete all Executive.');
            }
        }
        function isValid() {
            if (document.getElementById('txtFinancerId').value.trim() == '') {
                return false;
            }
            if (document.getElementById('txtFinancerName').value.trim() == '') {
                return false;
            }
            return true;
        }

        function ClientSaveClick() {
            ////if (isValid()) {
            if (document.getElementById("DrpCompany").value=='0')
            {
                 jAlert("Company Required");
                 return false;
            }
            var table = document.getElementById("executiveTable");
            document.getElementById('executiveName_hidden').value = '';
            var data = '';
            //var arr[] = new Array();
            for (var i = 0, row; row = table.rows[i]; i++) {
                for (var j = 0, col; col = row.cells[j]; j++) {
                    if (col.children[0].type != 'button') {
                        if (data == '') {
                            if (col.children[0].value == '') {
                                jAlert("Base Currency Required");
                                return false;
                            }
                            else {
                                data = col.children[0].value;
                            }
                        }
                        else {
                            if (col.children[0].value == '') {
                                jAlert("Required");
                                return false;
                            }
                            else {
                                data = data + '~' + col.children[0].value;
                            }
                        }
                    }
                }
                //arr.push(data);
                if (document.getElementById('executiveName_hidden').value == '') {
                    document.getElementById('executiveName_hidden').value = data;
                    data = '';
                }
                else {
                    document.getElementById('executiveName_hidden').value = document.getElementById('executiveName_hidden').value + ',' + data;
                    data = '';
                }

            }

            return true;

        }

        function OnEndCallback(s, e) {

        }




    </script>
    <style>
        .nbackBtn button {
            background: transparent !important;
            border: none !important;
            font-size: 21px;
        }

            .nbackBtn button.green {
                color: #2db52d;
            }

            .nbackBtn button.red {
                color: #f53434;
            }

        .nbackBtn tr td:last-child {
            position: absolute;
        }

        .abs {
            position: absolute;
            top: 8px;
            right: -20px;
        }
        
        #executiveTable td, .pdri td {
            padding-right: 20px;
        }
    </style>
    <link rel="stylesheet" href="https://unpkg.com/flatpickr/dist/flatpickr.min.css">
    <link href="../../../assests/bootstrap/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <script src="../../../assests/bootstrap/js/bootstrap-datetimepicker.min.js"></script>

    <script src="https://unpkg.com/flatpickr"></script>

    <script>
        $(document).ready(function () {
            $(".flatpickr").flatpickr({
                enableTime: true,
                weekNumbers: true
                
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Add/Edit Currency</h3>
            <div class="crossBtn"><a href="CurrencyMaster.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>

    <div class="form_main clearfix">
        <dxe:ASPxCallbackPanel runat="server" ID="ExecutiveCallbackPanel" ClientInstanceName="CallbackPanel" RenderMode="Table" OnCallback="ExecutiveCallbackPanel_Callback">
            <ClientSideEvents EndCallback="OnEndCallback"></ClientSideEvents>
            <PanelCollection>
                <dxe:PanelContent ID="PanelContent3" runat="server">
                </dxe:PanelContent>
            </PanelCollection>
        </dxe:ASPxCallbackPanel>

        <%--debjyoti 22-12-2016--%>
        <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
            Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>

        <asp:HiddenField runat="server" ID="IsUdfpresent" />
        <%--End debjyoti 22-12-2016--%>


        <table style="width: 100%">
            <tr>
                <td style="width: 100%">
                    
                                    <dxe:ContentControl runat="server">
                                        <div class="totalWrap">
                                            <table>
                                                <tr>

                                                    <td width="300px" style="position: relative;">
                                                        <label>Company<span style="color: red">*</span></label>
                                                        <%-- <asp:TextBox ID="txtFinancerId" runat="server"   Width="100%"  onblur="onFinancerChange()"
                                                        MaxLength="80" />
                                                       
                                                        <asp:RequiredFieldValidator Display="Dynamic" runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtFinancerId"
                                                        SetFocusOnError="true" ErrorMessage="" class="pullrightClass fa fa-exclamation-circle abs iconRed" ToolTip="Mandatory" ValidationGroup="fingroup" >                                                        
                                                    </asp:RequiredFieldValidator>--%>
                                                        <asp:DropDownList ValidateRequestMode="Disabled" ID="DrpCompany" runat="server" Width="100%" OnSelectedIndexChanged="DrpCompany_SelectedIndexChanged1" AutoPostBack="true"></asp:DropDownList>

                                                    </td>
                                                </tr>
                                                <tr>

                                                    <td style="position: relative;">
                                                        <label>Base Currency</label>

                                                        <%-- <asp:DropDownList ID="DrpCurrency" runat="server" Width="100%"></asp:DropDownList>--%>
                                                        <%--<label id="lblcurrency" runat="server"></label>--%>

                                                        <asp:TextBox ID="txtcurrency" runat="server" Enabled="false"></asp:TextBox>
                                                        <asp:HiddenField runat="server" ID="CurrencyHD" />
                                                    </td>
                                                </tr>
                                                <%--<tr>

                                                    <td>
                                                        <asp:Button ID="Button1" runat="server" Text="Save" ValidationGroup="fingroup" CssClass="btn btn-primary dxbButton" OnClick="Button1_Click" />
                                                    </td>
                                                </tr>--%>
                                            </table>
                                            <table>


                                                <tr>

                                                    <td style="border-top:1px solid #ccc;padding-top:15px;">
                                                        <table style="width: 100%;" class="pdri">
                                                            <tr>
                                                                <td style="width: 180px">
                                                                    <label style="display: block;">Currency<span style="color: red">*</span> </label>
                                                                </td>
                                                                <td style="width: 163px">
                                                                    <label style="display: block;">Date<span style="color: red">*</span></label></td>
                                                                <td style="width: 170px">
                                                                    <label style="display: block;">Sale Rate<span style="color: red">*</span></label></td>
                                                                <td style="width: 180px">
                                                                    <label style="display: block;">Purchase Rate<span style="color: red">*</span></label></td>
                                                                <td></td>
                                                            </tr>
                                                        </table>
                                                        <table id="executiveTable" style="width: 100%;" class="nbackBtn">

                                                            <tr>

                                                                <td id="cell1" style="width: 150px">
                                                                    <%--  <label style=" display: block;">Currency </label>--%>
                                                                    <%-- <select id="Select1" runat="server" style="width:150px"> 
                                                                     <option></option>  
                                                                 </select>--%>


                                                                </td>

                                                                <td style="width: 150px">
                                                                    <%-- <label style=" display: block;">Date</label>--%>
                                                                    <input class="flatpickr" type="text" placeholder="Select Date.."/>
                                                                      <%--<input  type="date"/>--%>
                                                               
                                                                </td>

                                                                <td style="width: 150px">
                                                                    <%-- <label style=" display: block;">Sale Rate</label> --%>
                                                                    <input type="number" step='0.00001'  maxlength = "18" onkeypress="return isNumeric(event)" oninput="maxLengthCheck(this)"/>

                                                                </td>

                                                                <td style="width: 150px">
                                                                    <%-- <label style=" display: block;">Purchase Rate</label>--%>
                                                                    <input type="number" step='0.00001' maxlength = "18" onkeypress="return isNumeric(event)" oninput="maxLengthCheck(this)"/>

                                                                </td>

                                                                <td>
                                                                    <%-- <label style=" display: block;">&nbsp</label>--%>
                                                                    <button type="button" class="green" onclick="AddNewexecutive()"><i class="fa fa-plus-circle"></i></button>
                                                                    <button type="button" class="red" onclick="removeExecutive(this.parentNode.parentNode)"><i class="fa fa-times-circle"></i></button>
                                                                </td>

                                                            </tr>
                                                        </table>
                                                        <asp:HiddenField ID="executiveName_hidden" runat="server" />
                                                        <asp:HiddenField ID="currid_hidden" runat="server" />
                                                    </td>
                                                </tr>

                                            </table>
                                            <div class="clear"></div>
                                            <div class="" style="padding-top: 10px;">

                                                <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="fingroup" CssClass="btn btn-primary dxbButton" OnClick="btnSave_Click" OnClientClick="return ClientSaveClick()" />
                                                <asp:Button ID="Button2" runat="server" Text="Cancel" CssClass="btn btn-danger dxbButton" OnClick="Button2_Click" />
                                                <asp:Button ID="btnUdf" runat="server" Text="UDF" CssClass="btn btn-primary dxbButton hide" OnClientClick="if(OpenUdf()){ return false;}" />
                                            </div>
                                        </div>

                                    </dxe:ContentControl>
                            


                </td>
            </tr>
        </table>
    </div>
</asp:Content>

