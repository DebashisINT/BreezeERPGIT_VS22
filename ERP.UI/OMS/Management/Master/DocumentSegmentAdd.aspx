<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="DocumentSegmentAdd.aspx.cs" Inherits="ERP.OMS.Management.Master.DocumentSegmentAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/OMS/management/Master/Js/DocumentSegment.js?v=2.2"></script>
    <style>
        #PANNum {
            text-transform: uppercase;
        }

        .input-sm {
            margin-bottom: 5px;
        }

        .nogap {
            padding: 0px;
        }

        .ui-widget-header {
            border: 1px solid #272b3c !important;
            background: #272b3c !important;
            text-align: center;
            color: #fff;
        }

        .ui-widget.ui-widget-content + .ui-widget-overlay {
            z-index: 999998 !important;
            background: #040404;
            opacity: 0.7;
        }

        .ui-widget.ui-widget-content {
            min-width: 300px !important;
            min-width: 300px !important;
            box-shadow: 0px 0px 0px 4px rgba(255,255,255,0.7);
            border: none;
            z-index: 999999 !important;
        }

        .ui-dialog-buttonset {
            float: none !important;
            text-align: center;
            padding: 0px;
        }

        ui-dialog-buttonset > button {
            margin: 0 !important;
            margin-top: 3px;
        }
    </style>
    <style>
        .form-group input[type="checkbox"] {
            display: none;
        }

            .form-group input[type="checkbox"] + .btn-group > label span {
                width: 20px;
            }

                .form-group input[type="checkbox"] + .btn-group > label span:first-child {
                    display: none;
                }

                .form-group input[type="checkbox"] + .btn-group > label span:last-child {
                    display: inline-block;
                }

            .form-group input[type="checkbox"]:checked + .btn-group > label span:first-child {
                display: inline-block;
            }

            .form-group input[type="checkbox"]:checked + .btn-group > label span:last-child {
                display: none;
            }

        .clear {
            clear: both;
        }

        label {
            display: inline-block;
            margin-bottom: 5px;
            font-weight: bold !important;
        }
    </style>
    <style>
        #divModalBody {
            /* Remove default list styling */
            list-style-type: none;
            padding: 0;
            margin: 0;
            margin-bottom: 8px;
            background: #f5ece1;
            padding: 5px 0 0;
            border-radius: 5px;
        }

            #divModalBody li {
                padding: 5px 10px;
                display: inline-block;
            }

                #divModalBody li a {
                    margin-top: -1px; /* Prevent double borders */
                    padding: 0 12px; /* Add some padding */
                    text-decoration: none; /* Remove default text underline */
                    font-size: 14px; /* Increase the font-size */
                    color: black; /* Add a black text color */
                    display: inline-block; /* Make it into a block element to fill the whole list */
                    cursor: pointer;
                }

        .boxed {
            border: 1px solid #ccc;
            padding: 5px 10px;
        }
    </style>
    <script>
        function OnAddClick() {
            window.location.href = 'DocumentSegmentList.aspx?id=' + $("#hdnCustomerID").val();
        }
        function fn_AllowonlyNumeric(s, e) {
            var theEvent = e.htmlEvent || window.event;
            var key = theEvent.keyCode || theEvent.which;
            var keychar = String.fromCharCode(key);
            if (key == 9 || key == 37 || key == 38 || key == 39 || key == 40 || key == 8) { //tab/ Left / Up / Right / Down Arrow, Backspace, Delete keys
                return;
            }
            var regex = /[0-9]/;

            if (!regex.test(keychar)) {
                theEvent.returnValue = false;
                if (theEvent.preventDefault)
                    theEvent.preventDefault();
            }

            <%--if ($("#<%=hddnApplicationMode.ClientID%>").val() === "E") {

                td_Applicablefrom.style.display = "block";
            }
            else {
                td_Applicablefrom.style.display = "block";
            }--%>
        }
        function Gstin2TextChanged(s, e) {

            if (!e.htmlEvent.ctrlKey) {
                if (e.htmlEvent.key != 'Control') {
                    s.SetText(s.GetText().toUpperCase());
                }
            }
            <%--if ($("#<%=hddnApplicationMode.ClientID%>").val() === "E") {

                td_Applicablefrom.style.display = "block";
            }
            else {
                td_Applicablefrom.style.display = "block";
            }--%>
        }
        function Gstin3TextChanged(s, e) {

            <%--if ($("#<%=hddnApplicationMode.ClientID%>").val() === "E") {
                 td_Applicablefrom.style.display = "block";
             }
             else {
                 td_Applicablefrom.style.display = "block";
             }--%>
        }
        function PageLoad() {
            window.location.href = 'DocumentSegmentList.aspx?id=' + $("#hdnCustomerID").val();
        }
        function CheckParticular(v) {
            if (v == false) {
                $(".statecheckall").prop('checked', false);
            }
        }
        function validateGSTIN() {
            $("#myInputGSTIN").removeClass("hide");
            $("#myInputGSTIN").val($("#txtGSTIN1_I").val().trim() + $("#txtGSTIN2_I").val().trim() + $("#txtGSTIN3_I").val().trim());
            CopyFunction();


            window.open('https://services.gst.gov.in/services/searchtp');
        }

        function CopyFunction() {
            var copyText = document.getElementById("myInputGSTIN");
            copyText.select();
            document.execCommand("copy");
            $("#myInputGSTIN").addClass("hide");
        }
        function fn_btnValidate(s, e) {
            let a = [];

            $(".statecheckall:checked").each(function () {
                a.push(this.value);
            });

            $(".statecheck:checked").each(function () {
                a.push(this.value);
            });

            $("#hdnModuleList").val(a);

        }
        //$(document).ready(function () {
        //    fn_ModuleMap();
        //});
        //function myFunction() {
        //    // Declare variables
        //    var input, filter, ul, li, a, i, txtValue;
        //    input = document.getElementById('myInput');
        //    filter = input.value.toUpperCase();
        //    ul = document.getElementById("divModalBody");
        //    li = ul.getElementsByTagName('li');

        //    // Loop through all list items, and hide those who don't match the search query
        //    for (i = 0; i < li.length; i++) {
        //        a = li[i].getElementsByTagName("a")[0];
        //        txtValue = a.textContent || a.innerText;

        //        if (txtValue.toUpperCase().indexOf(filter) > -1) {
        //            li[i].style.display = "";
        //        } else {
        //            li[i].style.display = "none";
        //        }
        //    }
        //}
        //function fn_ModuleMap() {
        //    var DocumentSegmentId = $("#hdDocumentSegmentId").val();
        //    var str
        //    str = { Segment_Map_ID: DocumentSegmentId }

        //    var html = "";
        //    $.ajax({
        //        type: "POST",
        //        url: "DocumentSegmentAdd.aspx/GetModuleList",
        //        data: JSON.stringify(str),
        //        contentType: "application/json; charset=utf-8",
        //        datatype: "json",
        //        success: function (responseFromServer) {
        //            for (i = 0; i < responseFromServer.d.length; i++) {
        //                if (responseFromServer.d[i].IsChecked == true) {
        //                    html += "<li><input type='checkbox' id=" + responseFromServer.d[i].Module_Id + "  class='statecheck' onclick=CheckParticular($(this).is(':checked')) value=" + responseFromServer.d[i].Module_Id + " checked  /><a href='#'><label id='lblstatename' class='lblstate' for=" + responseFromServer.d[i].Module_Id + " >" + responseFromServer.d[i].Module_Name + "</label></a></li>";
        //                }
        //                else {
        //                    html += "<li><input type='checkbox' id=" + responseFromServer.d[i].Module_Id + " class='statecheck'  onclick=CheckParticular($(this).is(':checked')) value=" + responseFromServer.d[i].Module_Id + "   /><a href='#'><label id='lblstatename' class='lblstate' for=" + responseFromServer.d[i].Module_Id + ">" + responseFromServer.d[i].Module_Name + "</label></a></li>";
        //                }
        //            }
        //            $("#divModalBody").html(html);
        //            //  $("#myModal").modal('show');
        //        }
        //    });
        //}
    </script>
    <script>

        function OnCountryChanged(ccmbCountry) {
            var CountryID = ccmbCountry.GetValue();
            if (CountryID != "") {
                ccmbState.PerformCallback(CountryID);
            }
        }
        function OnStateChanged(ccmbState) {
            var State = ccmbState.GetValue();
            if (State != "") {
                ccmbDistrict.PerformCallback(State);
            }
        }
        function OnDistrictChanged(ccmbDistrict) {
            var District = ccmbDistrict.GetValue();
            if (District != "") {
                ccmbPincode.PerformCallback(District);
            }
        }


        function ChangeSegment() {
            var SegmentID = cddlSegment.GetValue();
            //$("#txtcode").prop('maxlength', '3');
            document.getElementById("txtcode").maxLength = "4";
            ccmbParent.PerformCallback(SegmentID);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-title clearfix hide" id="myDiv">
        <h3 class="pull-left">
            <asp:Label ID="lblHeading" runat="server" Text="Add Document Segment"></asp:Label>
        </h3>
    </div>
    <div id="ApprovalCross" runat="server" class="crossBtn hide"><a onclick="OnAddClick()" href="javascript:void(0);"><i class="fa fa-times"></i></a></div>
    <div class="panel panel-default">
        <div class="panel-body">
            <div class="row">
                <div class="col-sm-4">
                    <label>Segment <span class="red">*</span></label>
                    <div>
                        <select class="form-control input-sm" id="ddlSegment">
                            <option>Select</option>
                        </select>
                    </div>
                </div>
                <div class="col-sm-4">
                    <label>Unique ID <span class="red">*</span></label>
                    <div>
                        <input class="form-control input-sm" placeholder="Unique ID" maxlength="50" id="uniqueId" onblur="CheckUnique()" type="text" data-toggle="tooltip" title="Unique ID">
                    </div>
                </div>
                <div class="col-sm-4">
                    <label>Name <span class="red">*</span></label>
                    <div>
                        <input type="text" class="form-control input-sm" id="Name" />
                    </div>
                </div>
            </div>
            <div class="clear"></div>
            <div class="row">
                <div class="col-sm-4">
                    <label>Parent </label>
                    <div>
                        <select class="form-control input-sm" id="ddlParentSegment">
                            <option>Select</option>
                        </select>
                    </div>
                </div>
                <div class="col-sm-5">
                    <div class="row">
                        <div class="col-md-12">
                            <label>GSTIN</label>
                        </div>
                        <div class="clear">
                            <div class="col-sm-3">
                                <input class="form-control input-sm" id="GSTIN1" type="text" maxlength="2" data-toggle="tooltip" title="GSTIN" />
                            </div>
                            <div class="col-sm-5">
                                <input class="form-control input-sm" id="GSTIN2" type="text" maxlength="10" data-toggle="tooltip" title="GSTIN" />
                            </div>
                            <div class="col-sm-4">
                                <input class="form-control input-sm" id="GSTIN3" type="text" maxlength="3" data-toggle="tooltip" title="GSTIN" onblur="SetPanNumber()" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3" id="dvPAN">
                    <label>PAN</label>
                    <input class="form-control input-sm" placeholder="PAN" maxlength="10" id="PANNum" type="text" data-toggle="tooltip" title="Unique ID" onblur="UniquePanNumberCheck()">
                </div>
                <div class="clear"></div>
                <div class="col-sm-4" id="DivServiceBranch" runat="server">
                    <label>
                      Service Branch
                    </label>
                    <asp:DropDownList ID="ddlServiceBranch" runat="server" Width="100%">
                    </asp:DropDownList>
                </div>

            </div>
            <div class="clear"></div>
            <div class="row">
                <div class="col-sm-12">
                    <div class="row">
                        <div class="col-sm-6">
                            <div class="panel panel-default">
                                <div class="panel-header">
                                    <label style="padding-left: 10px; padding-top: 6px;">Billing Details</label>
                                </div>
                                <div class="panel-body" style="padding-top: 0">
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <label>Pin <span class="red">*</span></label>
                                            <div>
                                                <input class="form-control input-sm" placeholder="Pin Code" id="pinCode" maxlength="6" onblur="PinChange()"
                                                    type="text" data-toggle="tooltip" title="Pin Code">
                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <label>Alternative Phone </label>
                                            <div>
                                                <input class="form-control input-sm" placeholder="Alternative Phone" id="BillingPhone" maxlength="12" type="text"
                                                    data-toggle="tooltip" title="Alternative Phone">
                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                        <div class="col-sm-12">
                                            <label>Address <span class="red">*</span></label>
                                            <div>
                                                <input class="form-control input-sm" placeholder="Address 1" id="BillingAddress1" maxlength="50" type="text" data-toggle="tooltip" title="Address 1">
                                            </div>
                                            <div>
                                                <input class="form-control input-sm" placeholder="Address 2" id="BillingAddress2" maxlength="50" type="text" data-toggle="tooltip" title="Address 2">
                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                        <div class="col-sm-4">
                                            <label>Country</label>
                                            <div>
                                                <input class="form-control input-sm " placeholder="Country" disabled id="Country" type="text" data-toggle="tooltip" title="Country">
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <label>State</label>
                                            <div>
                                                <input class="form-control input-sm" placeholder="State" disabled id="State" type="text" data-toggle="tooltip" title="State">
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <label>District</label>
                                            <div>
                                                <input class="form-control input-sm" placeholder="District" disabled id="City" type="text" data-toggle="tooltip" title="District">
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <label>Latitude</label>
                                            <div>
                                                <input class="form-control input-sm" placeholder="Latitude" id="BillingLatitude" type="text" data-toggle="tooltip" title="Latitude">
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <label>Longitude</label>
                                            <div>
                                                <input class="form-control input-sm" placeholder="Longitude" id="BillingLongitude" type="text" data-toggle="tooltip" title="Longitude">
                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                        <a class="[ form-group ]" href="#" onclick="javascript: BillingCheckChange()">
                                            <input type="checkbox" name="fancy-checkbox-success" id="fancy-checkbox-success" autocomplete="off" />
                                            <div class="[ btn-group ]">
                                                <label for="fancy-checkbox-success" class="[ btn btn-success ]">
                                                    <span class="[ glyphicon glyphicon-ok ]"></span>
                                                    <span></span>
                                                </label>
                                                <label for="fancy-checkbox-success" class="[ btn btn-default active ]">
                                                    Ship To Same Address
                                                </label>
                                            </div>
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-sm-6">
                            <div class="panel panel-default">
                                <div class="panel-header">
                                    <label style="padding-left: 10px; padding-top: 6px;">Service Details</label>
                                </div>
                                <div class="panel-body" style="padding-top: 0">
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <label>Contact person </label>
                                            <div>
                                                <input class="form-control input-sm" placeholder="Contact Person" id="contactperson" maxlength="40" type="text" data-toggle="tooltip" title="Contact Person">
                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                        <div class="col-sm-6">
                                            <label>Pin <span class="red">*</span></label>
                                            <div>
                                                <input class="form-control input-sm" placeholder="Pin Code" id="shippingpinCode" maxlength="6" onblur="shippingPinChange()" id="Text4" type="text" data-toggle="tooltip" title="Pin Code">
                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <label>Alternative Phone </label>
                                            <div>
                                                <input class="form-control input-sm" placeholder="Alternative Phone" id="ShippingPhone" maxlength="12" type="text" data-toggle="tooltip" title="Alternative Phone">
                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                        <div class="col-sm-12">
                                            <label>Address <span class="red">*</span></label>
                                            <div>
                                                <input class="form-control input-sm" placeholder="Address 1" id="shippingAddress1" maxlength="50" type="text" data-toggle="tooltip" title="Address 1">
                                            </div>
                                            <div>
                                                <input class="form-control input-sm" placeholder="Address 2" id="shippingAddress2" maxlength="50" type="text" data-toggle="tooltip" title="Address 2">
                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                        <div class="col-sm-4">
                                            <label>Country</label>
                                            <div>
                                                <input class="form-control input-sm " placeholder="Country" disabled id="shippingCountry" type="text" data-toggle="tooltip" title="Country">
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <label>State</label>
                                            <div>
                                                <input class="form-control input-sm" placeholder="State" disabled id="shippingState" type="text" data-toggle="tooltip" title="State">
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <label>District</label>
                                            <div>
                                                <input class="form-control input-sm" placeholder="District" disabled id="shippingCity" type="text" data-toggle="tooltip" title="District">
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <label>Latitude</label>
                                            <div>
                                                <input class="form-control input-sm" placeholder="Latitude" id="ServiceLatitude" type="text" data-toggle="tooltip" title="Latitude">
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <label>Longitude</label>
                                            <div>
                                                <input class="form-control input-sm" placeholder="Longitude" id="ServiceLongitude" type="text" data-toggle="tooltip" title="Longitude">
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <label>Treatment Area</label>
                                            <div>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <input class="form-control input-sm" placeholder="TreatmentArea" id="TreatmentArea" type="text" data-toggle="tooltip" title="TreatmentArea" /></td>
                                                        <td style="width: 40px; font-size: 11px;">Sq Ft</td>
                                                    </tr>
                                                </table>

                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                        <a class="[ form-group ]" href="#" onclick="javascript: ShippingCheckChange()">
                                            <input type="checkbox" name="fancy-checkbox-successShipping" id="fancy-checkbox-successShipping" autocomplete="off" />
                                            <div class="[ btn-group ]">
                                                <label for="fancy-checkbox-successShipping" class="[ btn btn-success ]">
                                                    <span class="[ glyphicon glyphicon-ok ]"></span>
                                                    <span></span>
                                                </label>
                                                <label for="fancy-checkbox-successShipping" class="[ btn btn-default active ]">
                                                    Bill To Same Address
                                                </label>
                                            </div>
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clear"></div>
                    <div>
                        <input type="button" id="btnSaveCust" class="btn btn-primary" value="Save" onclick="SaveCustomer()" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnCustomerID" runat="server" />
    <%--  <div id="myModal" class="modal fade" data-backdrop="static" role="dialog">
        <div class="modal-dialog" style="width: 450px;">

            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Module List</h4>
                </div>
                <div class="modal-body">
                    <div>

                        <input type="text" id="myInput" onkeyup="myFunction()" placeholder="Search for Module.">

                        <ul id="divModalBody" class="listStyle">
                            <input type="checkbox" id="idstate" class="statecheck" /><label id="lblstatename" class="lblstate"></label>
                        </ul>
                    </div>
                     <input type="button" id="btnsatesubmit" title="SUBMIT" value="SUBMIT" class="btn btn-primary" onclick="BranchPushPop()" />
                    <input type="hidden" id="hdnstatelist" class="btn btn-primary" />
                    <input type="hidden" id="hdnEMPID" class="btn btn-primary" />
                </div>
            </div>

        </div>
    </div>--%>
    <asp:HiddenField ID="hdnModuleList" runat="server" />
    <asp:HiddenField ID="hdAddEdit" runat="server" />
    <asp:HiddenField ID="hdDocumentSegmentId" runat="server" />
    <asp:HiddenField ID="hddnApplicationMode" runat="server" />


    <asp:HiddenField ID="hdnBillingCountryID" runat="server" />
    <asp:HiddenField ID="hdnBillingStateID" runat="server" />
    <asp:HiddenField ID="hdnBillingCityID" runat="server" />
    <asp:HiddenField ID="hdnBillingPinID" runat="server" />


    <asp:HiddenField ID="hdnServiceCountryID" runat="server" />
    <asp:HiddenField ID="hdnServiceStateID" runat="server" />
    <asp:HiddenField ID="hdnServiceCityID" runat="server" />
    <asp:HiddenField ID="hdnServicePinID" runat="server" />

     <asp:HiddenField ID="hdnSyncCustomertoFSMWhileCreating" runat="server" />
</asp:Content>

