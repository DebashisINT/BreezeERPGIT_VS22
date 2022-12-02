<%@ Page Title="Financer" Language="C#" AutoEventWireup="true" Inherits="ERP.OMS.Managemnent.Master.management_Master_FinancerAddEdit" CodeBehind="FinancerAddEdit.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
   <%-- changes by subhra 13-02-2017--%>
       <style>
        #pageControl, .dxtc-content {
            overflow: visible !important;
        }

        #MandatoryAssign {
            position: absolute;
            right: -6px;
            top: 4px;
        }
        .chosen-container.chosen-container-multi, 
        .chosen-container.chosen-container-single {
            width:100% !important;
        }
        .chosen-choices {
            width:100% !important;
        }
        #lstAssignTo{
            width:200px;
        }
        .hide {
            display:none;
        }
        .dxtc-activeTab .dxtc-link  {
            color:#fff !important;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function changeFunc() {
            if (document.getElementById('hdIsMainAccountInUse').value == "IsInUse") {
                jAlert("Transaction exists for the selected Ledger. Cannot proceed.");
                ChangeselectedMainActvalue();
            } else {
                var MainAccount_val2 = document.getElementById("lstTaxRates_MainAccount").value;
                document.getElementById("hndTaxRates_MainAccount_hidden").value = document.getElementById("lstTaxRates_MainAccount").value;
            }
        }


        function ChangeselectedMainActvalue() {
            var lstTaxRates_MainAccount = document.getElementById("lstTaxRates_MainAccount");
            if (document.getElementById("hndTaxRates_MainAccount_hidden").value != '') {
                for (var i = 0; i < lstTaxRates_MainAccount.options.length; i++) {
                    if (lstTaxRates_MainAccount.options[i].value == document.getElementById("hndTaxRates_MainAccount_hidden").value) {
                        lstTaxRates_MainAccount.options[i].selected = true;
                    }
                }
                $('#lstTaxRates_MainAccount').trigger("chosen:updated");
            }
        }

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

        function ChangeSourceMainAccount() {
            var fname = "%";
            var lReportTo = $('select[id$=lstTaxRates_MainAccount]');
            lReportTo.empty();

            $.ajax({
                type: "POST",
                url: "FinancerAddEdit.aspx/GetMainAccountList",
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

                            $('#lstTaxRates_MainAccount').append($('<option>').text(name).val(id));
                        }

                        $(lReportTo).append(listItems.join(''));

                        $('#lstTaxRates_MainAccount').trigger("chosen:updated");
                        ChangeselectedMainActvalue();
                    }
                    else {
                        $('#lstTaxRates_MainAccount').trigger("chosen:updated");
                    }
                }
            });
        }




        function ListAssignTo() {

            $('#lstAssignTo').chosen();
            $('#lstAssignTo').fadeIn();
        }
        $(function () {
            debugger;
            //  BindAssign(branchid);
            if ($('#<%=hdbBranch.ClientID %>').val() != "") {

                BindAssign($('#<%=hdbBranch.ClientID %>').val());
                $('#<%=hdbBranch.ClientID %>').val('');
            }
            //BindAssign();
        })

        function BindAssign(branchid) {

            var lAssignTo = $('select[id$=lstAssignTo]');
            //  var lSupervisor = $('select[id$=lstSupervisor]');

            lAssignTo.empty();
            //   lSupervisor.empty();

            var lAddEdit = document.getElementById("hdnstorequrystring").value;
            $.ajax({
                type: "POST",
                data: JSON.stringify({ id: lAddEdit, branchid: branchid }),
                url: 'FinancerAddEdit.aspx/GetAllUserListBeforeSelect',
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

                            listItems.push('<option value="' +
                            id + '">' + name
                            + '</option>');
                        }

                        $(lAssignTo).append(listItems.join(''));
                        //   $(lSupervisor).append(listItems.join(''));


                        //   ListSupervisor();
                        ListAssignTo();
                        setWithFromAssign();


                        $('#lstAssignTo').trigger("chosen:updated");
                    }
                    else {
                        //$('#lstSupervisor').trigger("chosen:updated");
                        //$('#lstSupervisor').prop('disabled', true).trigger("chosen:updated");
                    }

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                }
            });


        }
        function setWithFromAssign() {
            debugger;
            var lstAssignTo = document.getElementById("lstAssignTo");
            var listValue = document.getElementById("hdnEditAssignTo").value;
            var str_array = listValue.split(',');

            for (var i = 0; i < lstAssignTo.options.length; i++) {
                var selectedValue = lstAssignTo.options[i].value;

                if (str_array.indexOf(selectedValue) > -1) {
                    lstAssignTo.options[i].selected = true;
                }
                else {
                    lstAssignTo.options[i].selected = false;
                }
            }
            $("#lstAssignTo").chosen().change();
            //if (str_array != "") {

            //    $('#lstAssignTo').prop('disabled', true).trigger("chosen:updated");
            //}
        }

        $(document).ready(function () {
            ListBind();
            ListAssignTo();
            ChangeSourceMainAccount();

            debugger;
            $("#lstAssignTo").chosen().change(function () {
                var assignId = $(this).val();
                $('#<%=hdnAssign.ClientID %>').val(assignId);
                $('#MandatoryAssign').attr('style', 'display:none');
            })



            //$('#lblSalesActivity').text('Assign Sales Activity');
        })

        function ClientSaveClick() {
            debugger;
            var flag = true;
            if (Page_ClientValidate()) {
                if (isValid()) {
                    var assignID = $("#lstAssignTo").find("option:selected").text();
                    //if (assignID == "") {
                    //    flag = false;
                    //    $('#MandatoryAssign').attr('style', 'display:block');
                    //}
                    //else { $('#MandatoryAssign').attr('style', 'display:none'); flag = true; }

                    return flag;
                }
            }
            return false;
        }
    </script>
    <script language="javascript" type="text/javascript">

        function isValidChar(evt) {
            var theEvent = evt || window.event;
            var key = theEvent.keyCode || theEvent.which;
            if (theEvent.key == "|" || theEvent.key == "~") {
                theEvent.returnValue = false;
            } else {
                theEvent.returnValue = true;
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
            alert('Saved successfully');
            window.location.href = obj;
            //function (message, title, callback) {
            //    $.alerts.alert(message, title, callback);
            //}
            //jAlert('Saved successfully', 'Saved', doAlert());


        }

        //function doAlert() {
        //    window.location.href = obj;
        //}
        $(document).ready(function () {
            loadExecutiveNameFromField();

            $('#cmbBranch').change(function () {

              //  alert();

                $('#lstAssignTo').val('').trigger('chosen:updated');
                $('#lstAssignTo').val('');
                $('#lstAssignTo').val('').trigger('chosen:updated');
                $('#lstAssignTo').val('').trigger('chosen:updated');

                BindAssign($(this).val());


            });
        });


        function loadExecutiveNameFromField() {
            var table = document.getElementById("executiveTable");
            var exeName = document.getElementById('executiveName_hidden').value;
            for (var i = 0 ; i < exeName.split('~').length; i++) {
                if (exeName.split('~')[i].trim() != '') {
                    if (table.rows[0].cells[0].children[0].value.trim() != '') {
                        var row = table.insertRow(0);
                        var cell0 = row.insertCell(0);
                        var cell1 = row.insertCell(1);
                        var cell2 = row.insertCell(2);
                        var cell3 = row.insertCell(3);
                        var cell4 = row.insertCell(4);

                        cell0.innerHTML = "<input type='text'  onkeypress='return isValidChar(event)'  maxlength='50' value='" + exeName.split('~')[i].split('|')[0] + "'/>";
                        //check Duplicate or not
                        var indx = document.getElementById('ErrorExecutive').value.indexOf(exeName.split('~')[i].split('|')[1]);
                        if (indx != -1) {
                            cell1.setAttribute("Class", "relative");
                            cell1.innerHTML = "<input type='text'  onkeypress='return isValidChar(event)' class='gsajdfgvasd'  maxlength='50' value='" + exeName.split('~')[i].split('|')[1] + "'/> <span class='flMan fa fa-exclamation-circle iconRed' title='Already Exist'></span>";
                        }
                        else
                            cell1.innerHTML = "<input type='text'  onkeypress='return isValidChar(event)' maxlength='50' value='" + exeName.split('~')[i].split('|')[1] + "'/>";

                        cell2.innerHTML = "<input type='text'  onkeypress='return isValidChar(event)' maxlength='50' value='" + exeName.split('~')[i].split('|')[2] + "'/>";
                        if (exeName.split('~')[i].split('|')[3] == '1')
                            cell3.innerHTML = "<input type='checkbox'  checked='true'/>";
                        else
                            cell3.innerHTML = "<input type='checkbox'  />";
                        cell4.innerHTML = "<button type='button' value='' onclick='AddNewexecutive()' class='green'><i class='fa fa-plus-circle'></i></button> <button type='button' value='' class='red' onclick='removeExecutive(this.parentNode.parentNode)'><i class='fa fa-times-circle'></i></button>";
                    }
                    else {
                        table.rows[0].cells[0].children[0].value = exeName.split('~')[i].split('|')[0];
                        table.rows[0].cells[1].children[0].value = exeName.split('~')[i].split('|')[1];
                        var indx = document.getElementById('ErrorExecutive').value.indexOf(exeName.split('~')[i].split('|')[1]);
                        if (indx != -1)
                            table.rows[0].cells[1].children[1].setAttribute("Class", "clsExists");


                        table.rows[0].cells[2].children[0].value = exeName.split('~')[i].split('|')[2];
                        if (exeName.split('~')[i].split('|')[3] == '1')
                            table.rows[0].cells[3].children[0].checked = true;
                        else
                            table.rows[0].cells[3].children[0].checked = false;

                    }
                }
            }

        }


        function AddNewexecutive() {
            var table = document.getElementById("executiveTable");

            var row = table.insertRow(0);
            var cell0 = row.insertCell(0);
            var cell1 = row.insertCell(1);
            var cell2 = row.insertCell(2);
            var cell3 = row.insertCell(3);
            var cell4 = row.insertCell(4);
            cell0.innerHTML = "<input type='text' maxlength='50'  onkeypress='return isValidChar(event)'/>";
            cell1.innerHTML = "<input type='text' maxlength='50'  onkeypress='return isValidChar(event)'/>";
            cell2.innerHTML = "<input type='text' maxlength='50'  onkeypress='return isValidChar(event)'/>";
            cell3.innerHTML = "<input type='checkbox'/>";
            cell4.innerHTML = "<button type='button' value='' onclick='AddNewexecutive()' class='green'><i class='fa fa-plus-circle'></i></button> <button type='button' value='' class='red' onclick='removeExecutive(this.parentNode.parentNode)'><i class='fa fa-times-circle'></i></button>";
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


        function OnEndCallback(s, e) {

        }

        function disp_prompt(name) {
            if (name == "tab1") {

                document.location.href = "Financer_Correspondence.aspx";
            }
            if (name == "tab2") {

                document.location.href = "Financer_document.aspx";
            }
        }

        function onFinancerChange() {

            var finCd = 0;
            if (GetObjectID('hiddenedit').value != '') {
                finCd = GetObjectID('hiddenedit').value;
            }
            var ShortName = document.getElementById("txtFinancerId").value.trim();
            $.ajax({
                type: "POST",
                url: "FinancerAddEdit.aspx/CheckUniqueName",
                data: JSON.stringify({ ShortName: ShortName, FinCode: finCd }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;

                    if (data == true) {
                        jAlert("Please enter unique Short Name");
                        document.getElementById("txtFinancerId").value = '';
                        document.getElementById("txtFinancerId").focus();
                        return false;
                    }
                }
            });


        }


    </script>
   
    <style>
        
          #lstAssignTo {
            width:200px;
        }
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
        .clsExists {
        border-color:red !important
        }
        .padR10 {
            padding-right:10px;
        }
        .clsExists {
            border-color:red;
        }
        /*.clsExists:before {
            content:'Error';
            position:absolute;
            right:0;
        }*/
        .flMan {
            position: absolute;
            right: 15px;
            top: 7px;
        }
        #executiveTable tr td:nth-child(2),
        #executiveTable tr td:nth-child(3),
        #executiveTable tr td:nth-child(1) {
            padding-right:10px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Add/Edit Financer</h3>
            <div class="crossBtn"><a href="Financer.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>

    <div class="form_main">
        <dxe:ASPxCallbackPanel runat="server" ID="ExecutiveCallbackPanel" ClientInstanceName="CallbackPanel" RenderMode="Table" OnCallback="ExecutiveCallbackPanel_Callback">
            <clientsideevents endcallback="OnEndCallback"></clientsideevents>
            <panelcollection>
                            <dxe:PanelContent ID="PanelContent3" runat="server">

                            </dxe:PanelContent>
                </panelcollection>
        </dxe:ASPxCallbackPanel>
        <asp:HiddenField runat="server" ID="hiddenedit" />
        <%--debjyoti 22-12-2016--%>
        <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
            Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed">
            <contentcollection>
                                                <dxe:PopupControlContentControl runat="server">
                                                </dxe:PopupControlContentControl>
                                            </contentcollection>
        </dxe:ASPxPopupControl>

        <asp:HiddenField runat="server" ID="IsUdfpresent" />
        <asp:HiddenField ID="hdIsMainAccountInUse" runat="server" />
        <%--End debjyoti 22-12-2016--%>
        <asp:HiddenField ID="hdnstorequrystring" runat="server" />

        <table style="width: 100%">
            <tr>
                <td style="width: 100%">
                    <dxe:ASPxPageControl ID="PageControl1" runat="server" Width="100%" ActiveTabIndex="0"
                        ClientInstanceName="page">
                        <tabpages>
                            <dxe:TabPage Text="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <div class="totalWrap">
                                            <table >
                                                <tr>
                                                    <td width="150px"><label>Financer ID <span style="color:red">*</span></label></td>
                                                    <td width="300px" style="position:relative;" >
                                                        <asp:TextBox ID="txtFinancerId" runat="server"   Width="100%"  onblur="onFinancerChange()"
                                                        MaxLength="80" />
                                                       
                                                        <asp:RequiredFieldValidator Display="Dynamic" runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtFinancerId"
                                                        SetFocusOnError="true" ErrorMessage="" class="pullrightClass fa fa-exclamation-circle abs iconRed" ToolTip="Mandatory" ValidationGroup="fingroup" >                                                        
                                                    </asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="150px"><label>Financer Name <span style="color:red">*</span></label></td>
                                                    <td  style="position:relative;">
                                                        <asp:TextBox ID="txtFinancerName" runat="server"  Width="100%" 
                                                        MaxLength="80" />
                                                        <asp:RequiredFieldValidator Display="Dynamic" runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtFinancerName"
                                                        SetFocusOnError="true" ErrorMessage="" class="pullrightClass fa fa-exclamation-circle abs iconRed" ToolTip="Mandatory" ValidationGroup="fingroup" >                                                        
                                                    </asp:RequiredFieldValidator>

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="150px"><label>Branch </label></td>
                                                    <td><asp:DropDownList ID="cmbBranch" runat="server" Width="100%" 
                                                        >
                                                    </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div class="padBot5" style="display: block;">
                                                            <span>Main Account</span> 
                                                        </div>
                                                     </td>
                                                    <td>
                                                        <div class="Left_Content"> 
                                                                <asp:ListBox ID="lstTaxRates_MainAccount" CssClass="chsn" runat="server" Font-Size="12px" Width="100%" data-placeholder="Select..." TabIndex="10"  onchange="changeFunc();"></asp:ListBox>
                                                            <asp:HiddenField ID="hndTaxRates_MainAccount_hidden" runat="server" />
                                                        </div>
                                                      </td>  


                                                </tr>
                                                <tr>
                                                    <td><label>Executive Name</label></td>
                                                    <td width="300px" class="relative" style="padding-top: 5px;padding-bottom:5px">
                                                      <div class="auto-style2  ">
                                                      <asp:ListBox ID="lstAssignTo"  SelectionMode="Multiple" CssClass="hide" runat="server" Font-Size="12px"   Height="90px" Width="100%"  data-placeholder="Select..."></asp:ListBox>
                                                      <asp:Label ID="lblAssignTo" runat="server" Text=""></asp:Label>
                                                      <span id="MandatoryAssign" style="display:none"><span id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="pullrightClass fa fa-exclamation-circle abs iconRed"  title="Mandatory"></span></span>
                                                      <asp:HiddenField ID="hdnAssign" runat="server" />
                                                      <asp:HiddenField ID="hdnAssignText" runat="server" />
                                                            <asp:HiddenField ID="hdnEditAssignTo" runat="server" />
                                                         <%-- <asp:ListBox ID="lstAssignTo"   runat="server" SelectionMode="Multiple"  Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="Select ..."></asp:ListBox>
                                                          <asp:HiddenField ID="hdnAssign" runat="server" />
                                                          <asp:HiddenField ID="hdnAssignText" runat="server" />   
                                                          <span id="MandatoryAssign" style="display:none"><img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc"  title="Mandatory"></span>--%>
                                               </div>

                                                    </td>
                                                </tr>
                                                <tr style="display:none">
                                                    <td width="150px" style="vertical-align: center"><label>Executive Details </label></td>
                                                    <td>
                                                        <table width="100%">
                                                            <tr>
                                                                <td style="width: 142px;">Name</td>
                                                                <td style="width: 144px;">User Login Id</td>
                                                                <td>Password</td>
                                                            </tr>
                                                        </table>
                                                        <table id="executiveTable" style="width: 100%;" class="nbackBtn">
                                                        <tr >
                                                            <td class="padR10">
                                                                <input type="text" maxlength="50"  onkeypress='return isValidChar(event)'/>
                                                            </td>
                                                            <td class="padR10">
                                                                <input type="text" maxlength="50"  onkeypress='return isValidChar(event)'/>
                                                                <span class=""></span>
                                                            </td>
                                                            <td class="padR10">
                                                                <input type="text" maxlength="50"  onkeypress='return isValidChar(event)'/>
                                                            </td>
                                                             <td>
                                                                <input type="checkbox"/>
                                                            </td>
                                                            <td>
                                                                <button type="button" class="green" onclick="AddNewexecutive()" ><i class="fa fa-plus-circle"></i></button>
                                                                <button type="button" class="red" onclick="removeExecutive(this.parentNode.parentNode)" ><i class="fa fa-times-circle"></i></button>
                                                            </td>

                                                        </tr>
                                                    </table>
                                                    <asp:HiddenField ID="executiveName_hidden" runat="server" />
                                                        <asp:HiddenField ID="ErrorExecutive" runat="server" />
                                                    </td>
                                                </tr>
                                                 <tr>
                                                    <td width="150px"><label>Is Active? </label></td>
                                                    <td>
                                                        <asp:CheckBox ID="chkActive" runat="server"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div class="clear"></div>
                                            <div class="col-md-12" style="padding-top: 10px;padding-left: 151px;">
                                                
                                                <%--<asp:Button ID="btnSave" runat="server" Text="Save"  ValidationGroup="fingroup" CssClass="btn btn-primary dxbButton" OnClick="btnSave_Click" OnClientClick="return ClientSaveClick()"/>--%>
                                                <asp:Button ID="btnSave" runat="server" Text="Save"  ValidationGroup="fingroup" CssClass="btn btn-primary dxbButton" OnClick="btnSave_Click" OnClientClick="return ClientSaveClick()"/>
                                                 <asp:Button ID="Button2" runat="server" Text="Cancel" CssClass="btn btn-danger dxbButton" OnClick="Button2_Click"
                                               />
                                                 <asp:Button ID="btnUdf" runat="server" Text="UDF"  CssClass="btn btn-primary dxbButton"  OnClientClick="if(OpenUdf()){ return false;}"
                                                     />
                                            </div>
                                        </div>

                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            
                            <dxe:TabPage Name="Correspondence" Text="Correspondence">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">

                                        <%--details goes here--%>

                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>

                              <dxe:TabPage Name="Documents" Text="Documents">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">

                                        <%--details goes here--%>

                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>

                        </tabpages>
                        <clientsideevents activetabchanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
	                                            
	                                            var Tab1 = page.GetTab(1);
	                                            var Tab2 = page.GetTab(2);
	                                           
	                                            
	                                            if(activeTab == Tab1)
	                                            {
	                                                disp_prompt('tab1');
	                                            }
	                                            if(activeTab == Tab2)
	                                            {
	                                                disp_prompt('tab2');
	                                            }
	                                            }"></clientsideevents>
                    </dxe:ASPxPageControl>


                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hdbBranch" runat="server" />
</asp:Content>

