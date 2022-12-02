<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/PopUp.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_AddAddressForContactPerson" CodeBehind="AddAddressForContactPerson.aspx.cs" enableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
         
        .pdtbl td {
            padding: 2px 10px;
            position:relative;
        }
        .red {
            color:red;
        }
        #ajax_listOfOptions {
            position: absolute;
            width: 230px !important;
            height: auto;
            overflow: auto;
            border: 1px solid #c2c2c5 !important;
            background-color: #FFF;
            text-align: left;
            font-size: 0.9em;
            z-index: 100;
        }
        #ajax_listOfOptions .optionDiv, #ajax_listOfOptions .optionDivSelected {
            padding:8px;
        }
    </style>
    <script>
        function goBack() {
            window.history.back();
        }
    </script>
     <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            ListBind();
            if (document.getElementById('txtCountry_hidden')) {
                var cntry = document.getElementById('txtCountry_hidden').value;
                document.getElementById('txtCountry_hidden').value = "";
                setCountry(cntry);
            }
        });

        function ClientSaveClick() {
            if (!isValid()) {
                return false;
            }
            document.getElementById('txtCountry_hidden').value = document.getElementById('lstCountry').value;
            document.getElementById('txtState_hidden').value = document.getElementById('lstState').value;
            document.getElementById('txtCity_hidden').value = document.getElementById('lstCity').value;
            document.getElementById('txtArea_hidden').value = document.getElementById('lstArea').value;
            document.getElementById('txtPincode_hidden').value = document.getElementById('lstPin').value;
            return true;
        }
        function setCountry(obj) {
            if (obj) {
                var lstCntry = document.getElementById("lstCountry");

                for (var i = 0; i < lstCntry.options.length; i++) {
                    if (lstCntry.options[i].value == obj) {
                        lstCntry.options[i].selected = true;
                    }
                }
                $('#lstCountry').trigger("chosen:updated");
                onCountryChange();
            }
        }
        function setState(obj) {
            if (obj) {
                var lstStae = document.getElementById("lstState");

                for (var i = 0; i < lstStae.options.length; i++) {
                    if (lstStae.options[i].value == obj) {
                        lstStae.options[i].selected = true;
                    }
                }
                $('#lstState').trigger("chosen:updated");
               onStateChange();
            }
        }

        function setCity(obj) {
            if (obj) {
                var lstCity = document.getElementById("lstCity");

                for (var i = 0; i < lstCity.options.length; i++) {
                    if (lstCity.options[i].value == obj) {
                        lstCity.options[i].selected = true;
                    }
                }
                $('#lstCity').trigger("chosen:updated");
                onCityChange();
            }
        }
        function setArea(obj) {
            if (obj) {
                var lstArea = document.getElementById("lstArea");

                for (var i = 0; i < lstArea.options.length; i++) {
                    if (lstArea.options[i].value == obj) {
                        lstArea.options[i].selected = true;
                    }
                }
                $('#lstArea').trigger("chosen:updated");

            }

        }

        function setPin(obj) {
            if (obj) {
                var lstPin = document.getElementById("lstPin");

                for (var i = 0; i < lstPin.options.length; i++) {
                    if (lstPin.options[i].value == obj) {
                        lstPin.options[i].selected = true;
                    }
                }
                $('#lstPin').trigger("chosen:updated");

            }
        }
        function onCountryChange() {
            var CountryId = "";
            if (document.getElementById('lstCountry').value) {
                CountryId = document.getElementById('lstCountry').value;
            } else {
                return;
            }
            var lState = $('select[id$=lstState]');
            var lCity = $('select[id$=lstCity]');
            var lArea = $('select[id$=lstArea]');
            var lPin = $('select[id$=lstPin]');
            lState.empty();
            lCity.empty();
            lArea.empty();
            lPin.empty();
            $('#lstCity').trigger("chosen:updated");
            $('#lstArea').trigger("chosen:updated");
            $('#lstPin').trigger("chosen:updated");
            $.ajax({
                type: "POST",
                url: "BranchAddEdit.aspx/GetStates",
                data: JSON.stringify({ CountryCode: CountryId }),
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

                        $(lState).append(listItems.join(''));

                        $('#lstState').fadeIn();
                        $('#lstState').trigger("chosen:updated");
                        if (document.getElementById('txtState_hidden').value) {
                            var stateVal = document.getElementById('txtState_hidden').value;
                            document.getElementById('txtState_hidden').value = "";
                            setState(stateVal);
                        }
                    }
                    else {
                        $('#lstState').fadeIn();
                        $('#lstState').trigger("chosen:updated");
                    }
                }
            });
        }
        function onStateChange() {
            var StateId = "";
            if (document.getElementById('lstState').value) {
                StateId = document.getElementById('lstState').value;
            }
            else {
                return;
            }
            var lCity = $('select[id$=lstCity]');
            var lArea = $('select[id$=lstArea]');
            var lPin = $('select[id$=lstPin]');
            lArea.empty();
            lCity.empty();
            lPin.empty();
            $('#lstArea').trigger("chosen:updated");
            $('#lstPin').trigger("chosen:updated");
            $.ajax({
                type: "POST",
                url: "BranchAddEdit.aspx/GetCities",
                data: JSON.stringify({ StateCode: StateId }),
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

                        $(lCity).append(listItems.join(''));

                        $('#lstCity').fadeIn();
                        $('#lstCity').trigger("chosen:updated");
                        if (document.getElementById('txtCity_hidden').value) {
                            var cityVal = document.getElementById('txtCity_hidden').value;
                            document.getElementById('txtCity_hidden').value = "";
                            setCity(cityVal);
                        }
                    }
                    else {
                        $('#lstCity').fadeIn();
                        $('#lstCity').trigger("chosen:updated");
                    }
                }
            });
        }
        function onCityChange() {
            getPinList();

            var CityId = "";
            
            if (document.getElementById('lstCity').value) {
                CityId = document.getElementById('lstCity').value;
            }
            else {
                return;
            }
            var lArea = $('select[id$=lstArea]');
            lArea.empty();
            pinCodeWithAreaId = [];
            $.ajax({
                type: "POST",
                url: "BranchAddEdit.aspx/GetArea",
                data: JSON.stringify({ CityCode: CityId }),
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
                            pin = list[i].split('|')[2];
                            listItems.push('<option value="' +
                            id + '">' + name
                            + '</option>');
                            pinCodeWithAreaId[i] = id + '~' + pin;
                        }

                        $(lArea).append(listItems.join(''));

                        $('#lstArea').fadeIn();
                        $('#lstArea').trigger("chosen:updated");
                        if (document.getElementById('txtArea_hidden').value) {
                            var areaVal = document.getElementById('txtArea_hidden').value;
                            document.getElementById('txtArea_hidden').value = "";
                            setArea(areaVal);
                        }
                    }
                    else {
                        $('#lstArea').fadeIn();
                        $('#lstArea').trigger("chosen:updated");
                    }
                }
            });
        }
        function getPinList() {
            var CityId = "";
            if (document.getElementById('lstCity').value) {
                CityId = document.getElementById('lstCity').value;
            }
            else {
                return;
            }
            var lPin = $('select[id$=lstPin]');
            lPin.empty();
            $.ajax({
                type: "POST",
                url: "BranchAddEdit.aspx/GetPin",
                data: JSON.stringify({ CityCode: CityId }),
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

                        $(lPin).append(listItems.join(''));

                        $('#lstPin').fadeIn();
                        $('#lstPin').trigger("chosen:updated");
                        if (document.getElementById('txtPincode_hidden').value) {
                            var pin = document.getElementById('txtPincode_hidden').value;
                            document.getElementById('txtPincode_hidden').value = '';
                            setPin(pin);
                        }

                    }
                    else {
                        $('#lstPin').fadeIn();
                        $('#lstPin').trigger("chosen:updated");
                    }
                }
            });
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
        function Page_Load() {
            //document.getElementById('trState').style.display = 'inline';
            //document.getElementById('trCity').style.display = 'inline';
            //document.getElementById('trArea').style.display = 'inline';
        }

        function isValid() {
            var retvalue = true;
            

            if (document.getElementById('lstCountry').value.trim() == '') {
                $('#MandatorylstCountry').css({ 'display': 'block' });
                retvalue = false;
            } else {
                $('#MandatorylstCountry').css({ 'display': 'none' });
            }
            if (document.getElementById('lstState').value.trim() == '') {
                $('#MandatorylstState').css({ 'display': 'block' });
                retvalue = false;
            } else {
                $('#MandatorylstState').css({ 'display': 'none' });
            }
            if (document.getElementById('lstCity').value.trim() == '') {
                $('#MandatorylstCity').css({ 'display': 'block' });
                retvalue = false;
            } else {
                $('#MandatorylstCity').css({ 'display': 'none' });
            }
            if (document.getElementById('lstPin').value.trim() == '') {
                $('#MandatorylstPin').css({ 'display': 'block' });
                retvalue = false;
            } else {
                $('#MandatorylstPin').css({ 'display': 'none' });
            }
            return retvalue;
        }

        function CallAjaxForCountry(objid, objfuncname, objevent) {
            ajax_showOptions(objid, objfuncname, objevent);

            //document.getElementById('trState').style.display = 'inline';
        }
        function CallAjaxForState(objid, objfuncname, objevent) {
            var stateid = document.getElementById('txtCountry_hidden').value;
            ajax_showOptions(objid, objfuncname, objevent, stateid);
            return;
            //document.getElementById('trCity').style.display = 'inline';
        }
        function CallAjaxForCity(objid, objfuncname, objevent) {
            var cityid = document.getElementById('txtState_hidden').value;
            ajax_showOptions(objid, objfuncname, objevent, cityid);
            //document.getElementById('trArea').style.display = 'inline';
        }
        function CallAjaxForArea(objid, objfuncname, objevent) {
            var areaid = document.getElementById('txtCity_hidden').value;
            ajax_showOptions(objid, objfuncname, objevent, areaid);
        }
        function btnCalcel_Click() {

            //comment by sanjib 19122016 due to popup mode chnaged.
            //window.close();
            // window.location = 'insurance_contactPerson.aspx';
            //var parentWindow = window.parent;
            //parentWindow.popup.Hide();
            //changes parentwindow to window because the parentwindow does't working 12-12-2016
            //window.close();
            //end
           
            //var path = document.referrer;
            var path = window.parent.location.href
            var page = path.split("/").pop();
            if (page == 'HRrecruitmentagent_ContactPerson.aspx')
            {
                window.parent.GridContactPerson.PerformCallback();
                window.parent.popup.Hide();
            }
            else{
            window.parent.GridContactPerson.PerformCallback();
            window.parent.Popup_AddAddress.Hide();
            }
        }
        FieldName = 'btnCancel';
    </script>



     <style type="text/css">
       
          .chosen-container.chosen-container-single {
            width:100% !important;
        }
        .chosen-choices {
            width:100% !important;
        }
        #lstCountry,#lstState,#lstCity,#lstArea,#lstPin,#lstBranchHead {
            width:220px;
        }
        #lstCountry,#lstState,#lstCity,#lstArea,#lstPin,#lstBranchHead {
            display:none !important;
            
        }
        #lstCountry_chosen,#lstState_chosen,#lstCity_chosen,#lstArea_chosen,#lstPin_chosen,#lstBranchHead_chosen{
            width:100% !important;
        }
        #PageControl1_CC {
        overflow:visible !important;
        }
        #lstState_chosen, #lstCountry_chosen, #lstCity_chosen,#lstPin_chosen,#lstBranchHead_chosen {
            margin-bottom:5px;
        }
        #lstCountry_chosen .chosen-drop,#lstState_chosen .chosen-drop, #lstCity_chosen .chosen-drop, #lstPin_chosen .chosen-drop, #lstArea_chosen .chosen-drop  {
                bottom: 20px;
                top: auto !important;
                border-top: 1px solid #ccc;
        }
        .chosen-container .chosen-results {
            max-height:180px;
        }
        .pdtbl td {
            padding-bottom:8px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
      <%--  <div class="panel-title">
            <h3><span id="lblHeadTitle">Add/Edit Address</span></h3>

            <div class="crossBtn">--%>

                <%--.................Code Commented By Sam on 08112016 due to redirection of pages based on previous url......................--%>
<%--                <asp:HyperLink ID="goBackCrossBtn" NavigateUrl="#" runat="server">
             <i class="fa fa-times" style="margin-top:6px"></i>
                </asp:HyperLink>--%>

                <%-- <a href="insurance_contactPerson.aspx">
                      <i class="fa fa-times"></i></a>--%>
          <%--  </div>--%>
            <%--.................Code Above Commented By Sam on 08112016......................--%>
        <%--</div>--%>
        <div >
            <table class="TableMain100  pdtbl" style="width: 410PX;">
                <tr>
                    <td class="EcoheadCon_" align="">Address Type:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlAddressType" runat="server" CssClass="form-control" TabIndex="1">
                            <asp:ListItem>Residence</asp:ListItem>
                            <asp:ListItem>Office</asp:ListItem>
                            <asp:ListItem>Correspondence</asp:ListItem>
                            <asp:ListItem>Registered</asp:ListItem>
                            <asp:ListItem>Permanent</asp:ListItem>
                        </asp:DropDownList>
                        
                    </td>
                </tr>
                <tr>
                    <td class="EcoheadCon_" align="">Address1 :
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddress1" runat="server" CssClass="form-control" TabIndex="2" MaxLength="500"></asp:TextBox>
                     
                    </td>
                </tr>
                <tr>
                    <td class="EcoheadCon_" align="">Address2 :
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddress2" runat="server" CssClass="form-control" TabIndex="3" MaxLength="500"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="EcoheadCon_" align="">Address3 :
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddress3" runat="server" CssClass="form-control" TabIndex="4" MaxLength="500"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="EcoheadCon_" align="">Landmark :
                    </td>
                    <td>
                        <asp:TextBox ID="txtLandmark" runat="server" CssClass="form-control" TabIndex="5" MaxLength="500"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="EcoheadCon_" align="">Country<span style="color: red">*</span> :
                    </td>
                    <td>
                        <%--<asp:TextBox ID="txtCountry" runat="server" onkeyup="CallAjaxForCountry(this,'Country',event)" CssClass="form-control" TabIndex="6"></asp:TextBox>--%>
                        <asp:ListBox ID="lstCountry" CssClass="chsn"   runat="server" Font-Size="12px" Width="253px"   data-placeholder="Select..." onchange="onCountryChange()" TabIndex="6"></asp:ListBox> 
                        <span id="MandatorylstCountry" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute;right:-4px;top:6px;display:none" title="Mandatory"></span>
                        <asp:HiddenField ID="txtCountry_hidden" runat="server" />
                        
                    </td>
                </tr>
                <tr id="trState">
                    <td class="EcoheadCon_" align="">State<span style="color: red">*</span> :
                    </td>
                    <td style="height: 21px">
                        <%--<asp:TextBox ID="txtState" runat="server" onkeyup="CallAjaxForState(this,'State',event)" CssClass="form-control" TabIndex="7"></asp:TextBox>--%>
                       <asp:ListBox ID="lstState" CssClass="chsn"   runat="server" Font-Size="12px" Width="253px"   data-placeholder="Select State.."  onchange="onStateChange()" TabIndex="7"></asp:ListBox>
                        <span id="MandatorylstState" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute;right:-4px;top:6px;display:none" title="Mandatory"></span>
                        <asp:HiddenField ID="txtState_hidden" runat="server" />
                       
                    </td>
                </tr>
                <tr id="trCity">
                    <td class="EcoheadCon_" align="">City<span style="color: red">*</span> :
                    </td>
                    <td>
                        <%--<asp:TextBox ID="txtCity" runat="server" onkeyup="CallAjaxForCity(this,'City',event)" CssClass="form-control" TabIndex="8"></asp:TextBox>--%>
                        <asp:ListBox ID="lstCity" CssClass="chsn"   runat="server" Font-Size="12px" Width="253px"   data-placeholder="Select City.." onchange="onCityChange()" TabIndex="8"></asp:ListBox>
                        <span id="MandatorylstCity" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute;right:-4px;top:6px;display:none" title="Mandatory"></span>
                        <asp:HiddenField ID="txtCity_hidden" runat="server" />
                      
                    </td>
                </tr>
               
                <tr>
                    <td class="EcoheadCon_" align="">Pincode<span style="color: red">*</span> :
                    </td>
                    <td>
                        <%--<asp:TextBox ID="txtPincode" runat="server" CssClass="form-control" TabIndex="10"></asp:TextBox>--%>
                         <asp:ListBox ID="lstPin" CssClass="chsn"   runat="server" Font-Size="12px" Width="253px"   data-placeholder="Select..."  TabIndex="9"></asp:ListBox>
                        <span id="MandatorylstPin" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute;right:-4px;top:6px;display:none" title="Mandatory"></span>
                        <asp:HiddenField ID="txtPincode_hidden" runat="server" />
                       
                    </td>
                </tr>
                 <tr id="trArea">
                    <td class="EcoheadCon_" align="">Area :
                    </td>
                    <td>
                        <%--<asp:TextBox ID="txtArea" runat="server" onkeyup="CallAjaxForArea(this,'Area',event)" CssClass="form-control" TabIndex="9"></asp:TextBox>--%>
                        <asp:ListBox ID="lstArea" CssClass="chsn"   runat="server" Font-Size="12px" Width="253px"   data-placeholder="Select area.." TabIndex="10" ></asp:ListBox>
                        <asp:HiddenField ID="txtArea_hidden" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding-left: 158px;" align="left">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" OnClientClick="if(!ClientSaveClick()){return false}" TabIndex="11" />
                        <input id="btnCalcel" type="button" class="btn btn-danger" value="Cancel" onclick="btnCalcel_Click()" tabindex="12" />
                    </td>
                </tr>
            </table>

        </div>
    </div>
</asp:Content>
