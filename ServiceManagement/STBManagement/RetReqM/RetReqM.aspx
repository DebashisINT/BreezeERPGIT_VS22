<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/PopUp.Master" AutoEventWireup="true" CodeBehind="RetReqM.aspx.cs" Inherits="ServiceManagement.STBManagement.RetReqM.RetReqM" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .typeHeader {
            background: #2a8cb1;
            padding: 4px 5px;
            border-radius: 4px;
            color: #fff;
            margin: 5px 15px;
        }

        .hdM {
            font-size: 16px;
            margin: 0;
            padding: 4px 0 7px 4px;
        }

        .lbInput {
            display: block;
            background: #eaeaea;
            padding: 5px;
            border-radius: 3px;
            border: 1px solid #ccc;
            min-height: 28px;
        }


        .approved{
            text-align: center;
            padding: 15px;
            margin-top: 35px;
            background: #66ce90;
            border-radius: 8px;
            margin: auto 10px auto 10px;
            border: 1px solid #238dbf;
            box-shadow: 0px 2px 8px rgb(0 0 0 / 2%);
            color: #fff;
            word-spacing: 2px;
            top: 50%;
            position: absolute;
            left: 50%;
            transform: translate(-53%, -50%);
            width: 89%;
        }
    </style>

    <script>

        $(document).ready(function () {

            //document.getElementById('txtDocumentNumber').disabled = true;

            $("#btnAdd").removeClass('hide');
            ctxtDetails2Quantity.SetEnabled(false);
        });

        function Details2Hide(val) {
            //if (cddlRequisitionFor.GetText() == "Fresh Issue") {
            // ctxtUnitPrice.SetEnabled(true);
            $("#divDetails2").removeClass('hide');
            //}
            //else {
            //    ctxtUnitPrice.SetEnabled(true);
            $("#spnDetails2HeaderName").text(val + " Details");
            //    $("#divDetails2").removeClass('hide');
            //}

            if (val == "OSTB Exchange") {
                $("#ostbVendor").removeClass('hide');
            }

        }

        function EditDevice() {
            ctxtUnitPrice.SetEnabled(true);
            ctxtQuantity.SetEnabled(true);

            $("#btnAdd").addClass('hide');
            $("#btnSave").removeClass('hide');
        }

        function SaveDevice() {

            var a = parseInt(ctxtUnitPrice.GetText());
            var b = parseInt(ctxtQuantity.GetText());
            var c = a * b;
            ctxtAmount.SetValue(c);

            if ($("#lblRequisitionFor").html() != "Fresh Issue") {
                ctxtDetails2Quantity.SetValue(b);
                ctxtDetails2UnitPrice.SetValue(a);
                ctxtDetails2Amount.SetValue(c);
            }

            ctxtUnitPrice.SetEnabled(false);
            ctxtQuantity.SetEnabled(false);

            $("#btnAdd").removeClass('hide');
            $("#btnSave").addClass('hide');

        }

        function grid_EndCallBack(s, e) {
            //var url = 'erp_addHoliday.aspx';
            //window.location.href = url;
        }

        function OnUnitPriceLostFocus(s, e) {
            var a = parseInt(ctxtUnitPrice.GetText());
            var b = parseInt(ctxtQuantity.GetText());
            var c = a * b;
            ctxtAmount.SetValue(c);

            if ($("#lblRequisitionFor").html() != "Fresh Issue") {
                ctxtDetails2Quantity.SetValue(b);
                ctxtDetails2UnitPrice.SetValue(a);
                ctxtDetails2Amount.SetValue(c);
            }
        }

        function OnQuantityLostFocus(s, e) {
            var a = parseInt(ctxtUnitPrice.GetText());
            var b = parseInt(ctxtQuantity.GetText());
            var c = a * b;
            ctxtAmount.SetValue(c);

            if ($("#lblRequisitionFor").html() != "Fresh Issue") {
                ctxtDetails2Quantity.SetValue(b);
                ctxtDetails2UnitPrice.SetValue(a);
                ctxtDetails2Amount.SetValue(c);
            }
        }

        function AddDevice() {

            var STBRequisitionID = $("#hdnSTBRequisitionID").val();
            var Model = cddlModel.GetText();
            var Model_ID = cddlModel.GetValue();
            var Amount = ctxtAmount.GetText();
            var UnitPrice = ctxtUnitPrice.GetValue();
            var Quantity = ctxtQuantity.GetValue();
            var Remarks = $("#txtRemarks").val();
            var Gu_id = $('#hdnGuid').val();


            var suc = true;

            //if (cCmbScheme.GetValue() == "0") {
            //    jAlert("Please select Numbering Scheme.");
            //    cCmbScheme.Focus();
            //    suc = false;
            //    return
            //}

            if ($("#txtEntityCode").val() == "") {
                jAlert("Please enter Entity Code.")
                $("#txtEntityCode").focus();
                suc = false;
                return
            }

            if (cddlModel.GetValue() == "0") {
                jAlert("Please select Model.")
                cddlModel.Focus();
                suc = false;
                return
            }

            if (ctxtUnitPrice.GetValue() == 0) {
                jAlert("Please enter Unit Price.")
                ctxtUnitPrice.Focus();
                suc = false;
                return
            }

            if (ctxtQuantity.GetValue() == 0) {
                jAlert("Please enter Quantity.")
                ctxtQuantity.Focus();
                suc = false;
                return
            }


            $.ajax({
                type: "POST",
                url: "RetReqM.aspx/AddData",
                data: JSON.stringify({
                    Model: Model,
                    UnitPrice: UnitPrice,
                    Quantity: Quantity,
                    Amount: Amount,
                    Remarks: Remarks,
                    Model_ID: Model_ID,
                    Guids: Gu_id
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    if (msg.d != "") {
                        cddlModel.SetValue(0);
                        ctxtUnitPrice.SetText(0);
                        ctxtQuantity.SetText(0);
                        ctxtAmount.SetText(0);
                        $("#txtRemarks").val("");
                        $('#hdnGuid').val('');
                        cGrdDevice.PerformCallback();
                    }

                    if (msg.d == "Logout") {
                        location.href = "../../OMS/SignOff.aspx";
                    }

                    jAlert(msg.d, "Alert", function () {
                        setTimeout(function () {
                            cddlModel.Focus();
                        }, 200);

                    });
                }
            });
        }

        function ClickOnEdit(val) {

            $("#divDetails1EntryLeven").removeClass('hide');

            $.ajax({
                type: "POST",
                url: "RetReqM.aspx/EditData",
                data: JSON.stringify({ HiddenID: val }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    $('#hdnGuid').val(val);
                    cddlModel.SetValue(msg.d.Model_ID);
                    ctxtUnitPrice.SetText(msg.d.UnitPrice);
                    ctxtQuantity.SetText(msg.d.Quantity);
                    ctxtAmount.SetText(msg.d.Amount);
                    $("#txtRemarks").val(msg.d.Remarks);

                    cGrdDevice.PerformCallback();

                }
            });
        }

        function OnClickDelete(val) {
            jConfirm('Confirm Delete?', 'Alert', function (r) {
                if (r) {
                    $.ajax({
                        type: "POST",
                        url: "RetReqM.aspx/DeleteData",
                        data: JSON.stringify({ HiddenID: val }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var list = msg.d;
                            $("#divDetails1EntryLeven").removeClass('hide');
                            jAlert(msg.d);
                            cGrdDevice.PerformCallback();
                        }
                    });
                }
                else {

                }
            });
        }

        function GetDateFormat(today) {
            if (today != "") {
                var dd = today.getDate();
                var mm = today.getMonth() + 1; //January is 0!

                var yyyy = today.getFullYear();
                if (dd < 10) {
                    dd = '0' + dd;
                }
                if (mm < 10) {
                    mm = '0' + mm;
                }
                //  today = dd + '-' + mm + '-' + yyyy;
                today = yyyy + '-' + mm + '-' + dd;
            }

            return today;
        }




        //Rev Details 2 section

        function AddDetails2Device() {

            var STBRequisitionID = $("#hdnSTBRequisitionID").val();
            var Model = cddlDetails2Model.GetText();
            var Model_ID = cddlDetails2Model.GetValue();
            var Amount = ctxtDetails2Amount.GetText();
            var UnitPrice = ctxtDetails2UnitPrice.GetValue();
            var Quantity = ctxtDetails2Quantity.GetValue();
            var Remarks = $("#txtDetails2Remarks").val();
            var Gu_id = $('#hdnGuid2').val();
            var ostbVendor = cddlOSTBVendor.GetText();
            var ostbVendorID = cddlOSTBVendor.GetValue();

            if (ostbVendorID == 0) {
                ostbVendor = '';
            }

            var suc = true;

            //if (cCmbScheme.GetValue() == "0") {
            //    jAlert("Please select Numbering Scheme.");
            //    cCmbScheme.Focus();
            //    suc = false;
            //    return
            //}

            if ($("#txtEntityCode").val() == "") {
                jAlert("Please enter Entity Code.")
                $("#txtEntityCode").focus();
                suc = false;
                return
            }

            if (cddlDetails2Model.GetValue() == "0") {
                jAlert("Please select Model.")
                cddlDetails2Model.Focus();
                suc = false;
                return
            }

            if (ctxtDetails2UnitPrice.GetValue() == 0) {
                jAlert("Please enter Unit Price.")
                ctxtDetails2UnitPrice.Focus();
                suc = false;
                return
            }

            if (ctxtDetails2Quantity.GetValue() == 0) {
                jAlert("Please enter Quantity.")
                ctxtDetails2Quantity.Focus();
                suc = false;
                return
            }


            $.ajax({
                type: "POST",
                url: "RetReqM.aspx/AddDetails2Data",
                data: JSON.stringify({
                    Model: Model,
                    UnitPrice: UnitPrice,
                    Quantity: Quantity,
                    Amount: Amount,
                    Remarks: Remarks,
                    Model_ID: Model_ID,
                    Guids: Gu_id,
                    ostbVendor: ostbVendor,
                    ostbVendorID: ostbVendorID
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    if (msg.d != "") {
                        cddlDetails2Model.SetValue(0);
                        ctxtDetails2UnitPrice.SetText(0);
                        ctxtDetails2Quantity.SetText(0);
                        ctxtDetails2Amount.SetText(0);
                        cddlOSTBVendor.SetValue(0);
                        $("#txtDetails2Remarks").val("");
                        $('#hdnGuid2').val('');
                        cgridDeviceDetails2.PerformCallback();
                    }

                    if (msg.d == "Logout") {
                        location.href = "../../OMS/SignOff.aspx";
                    }

                    jAlert(msg.d, "Alert", function () {
                        setTimeout(function () {
                            cddlDetails2Model.Focus();
                        }, 200);

                    });
                }
            });
        }

        function ClickOnDetails2Edit(val) {
            $("#divDetails2EntryLeven").removeClass('hide');

            $.ajax({
                type: "POST",
                url: "RetReqM.aspx/EditDetails2Data",
                data: JSON.stringify({ HiddenID: val }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    $('#hdnGuid2').val(val);
                    cddlDetails2Model.SetValue(msg.d.Model_ID);
                    ctxtDetails2UnitPrice.SetText(msg.d.UnitPrice);
                    ctxtDetails2Quantity.SetText(msg.d.Quantity);
                    ctxtDetails2Amount.SetText(msg.d.Amount);
                    $("#txtDetails2Remarks").val(msg.d.Remarks);
                    cddlOSTBVendor.SetValue(msg.d.ostbVendorID);

                    cgridDeviceDetails2.PerformCallback();


                }
            });
        }

        function OnClickDetails2Delete(val) {
            jConfirm('Confirm Delete?', 'Alert', function (r) {
                if (r) {
                    $.ajax({
                        type: "POST",
                        url: "RetReqM.aspx/DeleteDetails2Data",
                        data: JSON.stringify({ HiddenID: val }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var list = msg.d;
                            $("#divDetails2EntryLeven").removeClass('hide');
                            jAlert(msg.d);
                            cgridDeviceDetails2.PerformCallback();
                        }
                    });
                }
                else {

                }
            });
        }

        function OnDetails2UnitPriceLostFocus(s, e) {
            var a = parseInt(ctxtDetails2UnitPrice.GetText());
            var b = parseInt(ctxtDetails2Quantity.GetText());
            var c = a * b;
            ctxtDetails2Amount.SetValue(c);
        }

        function OnDetails2QuantityLostFocus(s, e) {
            var a = parseInt(ctxtDetails2UnitPrice.GetText());
            var b = parseInt(ctxtDetails2Quantity.GetText());
            var c = a * b;
            ctxtDetails2Amount.SetValue(c);
        }

        function ddlDetails2Model_ValueChange() {

            $.ajax({
                type: "POST",
                url: "RetReqM.aspx/GetUnitPrice",
                data: JSON.stringify({
                    ModelId: cddlDetails2Model.GetValue(),
                    DAS: $("#txtDAS").val()
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    if (msg.d != "") {
                        ctxtDetails2UnitPrice.SetText(msg.d);
                    }
                }
            });
        }

        function ddlModel_ValueChange() {
            $.ajax({
                type: "POST",
                url: "RetReqM.aspx/GetUnitPrice",
                data: JSON.stringify({
                    ModelId: cddlModel.GetValue(),
                    DAS: $("#txtDAS").val()
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    if (msg.d != "") {
                        ctxtUnitPrice.SetText(msg.d);
                    }
                }
            });
        }

        //End of Rev Details 2 section


        function SaveButtonClick(values) {
            //WorkingRoster();
            //if (rosterstatus) {
            // LoadingPanel.Show();

            if ($("#hdAddEdit").val() == "Add") {
                //if (cCmbScheme.GetValue() == "0") {
                //    LoadingPanel.Hide();
                //    jAlert("Please select Numbering Scheme.");
                //    cCmbScheme.Focus();
                //    return
                //}
            }
            //if ($("#txtDocumentNumber").val() == "") {
            //    LoadingPanel.Hide();
            //    $("#MandatoryBillNo").show();
            //    return
            //}
            //else {
            //    $("#MandatoryBillNo").hide();
            //}

            //if (cddlRequisitionType.GetValue() == "0") {
            //    jAlert("Please Select Requisition Type.", "Alert", function () {
            //        setTimeout(function () {
            //            LoadingPanel.Hide();
            //            cddlRequisitionType.Focus();
            //            return
            //        }, 200);
            //    });
            //    return
            //}

            //if (cddlRequisitionFor.GetValue() == "0") {
            //    jAlert("Please Select Requisition For.", "Alert", function () {
            //        setTimeout(function () {
            //            LoadingPanel.Hide();
            //            cddlRequisitionFor.Focus();
            //            return
            //        }, 200);
            //    });
            //    return
            //}

            //if ($("#txtEntityCode").val() == "") {
            //    jAlert("Please enter Entity Code.", "Alert", function () {
            //        setTimeout(function () {
            //            LoadingPanel.Hide();
            //            $("#txtEntityCode").focus();
            //            return
            //        }, 200);
            //    });
            //    return
            //}

            //if ($("#txtNetworkName").val() == "") {
            //    jAlert("Please Enter Network Name.", "Alert", function () {
            //        setTimeout(function () {
            //            LoadingPanel.Hide();
            //            $("#txtNetworkName").focus();
            //            return
            //        }, 200);
            //    });
            //    return
            //}

            //var IsNoPayment = "0";

            //if ($('#chkNoPayment').is(':checked') == true) {
            //    IsNoPayment = "1";
            //}

            //var IsPayUsingOnAcountBalance = "0";

            //if ($('#chkPayUsingOnAcountBalance').is(':checked') == true) {
            //    IsPayUsingOnAcountBalance = "1";
            //}

            var ApprovalAction = "";
            var DirectorApprovalRequired = "";
            var ApprovalEmployee = "";
            var txtApprovalRemarks = "";
            var IsApproval = "";
            if ($("#hdnIsAproval").val() == "Yes") {

                if ($("#ddlApprovalAction").val() == "0") {
                    jAlert("Please Select Approval Action.", "Alert", function () {
                        setTimeout(function () {
                            LoadingPanel.Hide();
                            $("#ddlApprovalAction").focus();
                            return
                        }, 200);
                    });
                    return
                }
                if ($("#txtApprovalRemarks").val() == "") {
                    jAlert("Please Enter Approval Remarks.", "Alert", function () {
                        setTimeout(function () {
                            LoadingPanel.Hide();
                            $("#txtApprovalRemarks").focus();
                            return
                        }, 200);
                    });
                    return
                }

                ApprovalAction = $("#ddlApprovalAction").val();
                txtApprovalRemarks = $("#txtApprovalRemarks").val();
                IsApproval = "Yes";

            }

            // var frmDate = GetDateFormat(cFormDate.GetValue());

            var Apply = {
                Action: $("#hdAddEdit").val(),
                STBRequisitionID: $("#hdnSTBRequisitionID").val(),
                //DocumentNumber: $("#txtDocumentNumber").val(),
                //branch: cddlBranch.GetValue(),
                //date: frmDate,
                //RequisitionType: cddlRequisitionType.GetValue(),
                //RequisitionFor: cddlRequisitionFor.GetValue(),
                //EntityCode: $("#txtEntityCode").val(),
                //NetworkName: $("#txtNetworkName").val(),
                //IsNoPayment: IsNoPayment,
                //PaymentRelatedRemarks: $("#txtPaymentRelatedRemarks").val(),
                //IsPayUsingOnAcountBalance: IsPayUsingOnAcountBalance,

                unitPrice: ctxtUnitPrice.GetText(),
                UnitQty: ctxtQuantity.GetText(),
                UnitAmount: ctxtAmount.GetText(),

                ApprovalAction: ApprovalAction,
                ApprovalRemarks: txtApprovalRemarks,
                IsApproval: IsApproval,
                IsInventory: $("#hdnIsInventory").val()
            }

            $.ajax({
                type: "POST",
                url: "RetReqM.aspx/save",
                data: "{apply:" + JSON.stringify(Apply) + "}",
                async: true,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    if (response.d) {
                        if (response.d == "Logout") {
                            location.href = "../../OMS/SignOff.aspx";
                        }

                        if (response.d.split('~')[0] == "true") {
                            var msgs = "";
                            if ($("#hdAddEdit").val() == "Add") {
                                msgs = "STB Return Requisition " + response.d.split('~')[2] + " generated successfully.";
                            }
                            else {
                                msgs = "STB Return Requisition " + response.d.split('~')[2] + " approved successfully.";
                            }

                            jAlert(msgs, "Alert", function () {
                                //window.close();
                                $("#hMSG").html("STB Return Requisition Update Successfully.");
                                $("#divApproved").removeClass('hide');
                                $("#divApprovalSection").addClass('hide');
                            });
                        }
                        else {
                            jAlert(response.d.split('~')[0]);
                            //  $("#txtDeviceNumber").focus();
                            LoadingPanel.Hide();
                            return
                        }
                    }
                },
                error: function (response) {
                    console.log(response);
                }
            });
            //}
            //else {
            //    //jAlert("Working period is over. Try in next working period.", "Alert", function () {
            //    //    window.location.href = "STBRequisition.aspx";
            //    //});
            //    $("#divPopHead").removeClass('hide');
            //}
        }

        function CloseWindow() {
            //jAlert("STB Requisition already approved.", "Alert", function () {
            //    window.close();
            //});

            $("#hMSG").html("STB Return Requisition Already Updated.");

            $("#divApproved").removeClass('hide');
            $("#divApprovalSection").addClass('hide');
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="divApproved" runat="server" class="approved hide">
        <h3 class="hdM" id="hMSG">STB Ret Requisition Update Successfull. </h3>
    </div>
    <div style="padding: 4px 8px" id="divApprovalSection" runat="server" class="">
        <h3 class="hdM">STB Return Requisition -- Approval </h3>
        <div class="form_main">
            <div class="clearfix">
                <div class="row">
                    <div class="col-md-3">
                        <label>Document Number </label>
                        <div>
                            <label id="lblDocumentNumber" class="lbInput" runat="server"></label>
                            <%--<asp:TextBox ID="txtDocumentNumber" runat="server" Width="95%" MaxLength="30" CssClass="form-control">                             
                            </asp:TextBox>--%>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label>Location </label>
                        <div class="dropDev">
                            <label id="lblBranch" class="lbInput" runat="server"></label>
                            <%--<dxe:ASPxComboBox ID="ddlBranch" runat="server" ClientInstanceName="cddlBranch" Width="100%">
                            </dxe:ASPxComboBox>--%>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label>Date </label>
                        <div class="dropDev">
                            <label id="lblDate" class="lbInput" runat="server"></label>
                            <%--  <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                <buttonstyle width="13px"></buttonstyle>
                            </dxe:ASPxDateEdit>--%>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label>Requisition type </label>
                        <div class="dropDev">
                            <label id="lblRequisitionType" class="lbInput" runat="server"></label>
                            <%-- <dxe:ASPxComboBox ID="ddlRequisitionType" runat="server" ClientInstanceName="cddlRequisitionType" Width="100%">
                            </dxe:ASPxComboBox>--%>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <label>Requisition for </label>
                        <div class="dropDev">
                            <label id="lblRequisitionFor" class="lbInput" runat="server"></label>
                            <%-- <dxe:ASPxComboBox ID="ddlRequisitionFor" runat="server" ClientInstanceName="cddlRequisitionFor" Width="100%">
                                <clientsideevents selectedindexchanged="ddlRequisitionFor_ValueChange" />
                            </dxe:ASPxComboBox>--%>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label>Entity code </label>
                        <div>
                            <label id="lblEntityCode" class="lbInput" runat="server"></label>
                            <%--<input type="text" class="form-control" id="txtEntityCode" maxlength="300" runat="server" disabled />--%>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label>Network Name </label>
                        <div>
                            <label id="lblNetworkName" class="lbInput" runat="server"></label>
                            <%--<input type="text" class="form-control" id="txtNetworkName" maxlength="300" runat="server" disabled />--%>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label>DAS </label>
                        <div>
                            <label id="lblDAS" class="lbInput" runat="server"></label>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div>
                        <div class="typeHeader">STB Details</div>
                        <div class="col-md-3">
                            <label>Model </label>
                            <div class="dropDev">
                                <%--  <dxe:ASPxComboBox ID="ddlModel" runat="server" ClientInstanceName="cddlModel" Width="100%">
                                    <clientsideevents selectedindexchanged="ddlModel_ValueChange" />
                                </dxe:ASPxComboBox>--%>
                                <label id="lblModel1" class="lbInput" runat="server"></label>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Unit Price <span style="color: red;">*</span></label>
                            <div class="dropDev">
                                <dxe:ASPxTextBox ID="txtUnitPrice" ClientInstanceName="ctxtUnitPrice" runat="server" Width="100%" DisplayFormatString="{0:0.00}">
                                    <masksettings mask="<0..999999999>.<0..99>" allowmousewheel="false" />
                                    <validationsettings requiredfield-isrequired="false" errordisplaymode="None"></validationsettings>
                                    <clientsideevents lostfocus="OnUnitPriceLostFocus" />
                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Quantity <span style="color: red;">*</span></label>
                            <div class="dropDev">
                                <dxe:ASPxTextBox ID="txtQuantity" ClientInstanceName="ctxtQuantity" runat="server" Width="100%" DisplayFormatString="{0:0}">
                                    <masksettings mask="<0..999999999>" allowmousewheel="false" />
                                    <validationsettings requiredfield-isrequired="false" errordisplaymode="None"></validationsettings>
                                    <clientsideevents lostfocus="OnQuantityLostFocus" />
                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Amount <span style="color: red;">*</span></label>
                            <div class="dropDev">
                                <dxe:ASPxTextBox ID="txtAmount" ClientInstanceName="ctxtAmount" runat="server" Width="100%" DisplayFormatString="{0:0.00}">
                                    <masksettings mask="<0..999999999>.<0..99>" allowmousewheel="false" />
                                    <validationsettings requiredfield-isrequired="false" errordisplaymode="None"></validationsettings>
                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label> OSTB Vendor </label>
                            <div class="dropDev">
                                <label id="lblOSTBVendors" class="lbInput" runat="server"></label>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <label>Remarks </label>
                            <div>
                                <label id="lblRemarks" class="lbInput" runat="server"></label>
                            </div>
                        </div>
                        <div class="col-md-6" style="margin-top: 23px;">
                            <button type="button" id="btnAdd" class="btn btn-success" onclick="EditDevice()"><i class="fa fa-edit mr-2"></i>Edit</button>

                            <button type="button" id="btnSave" class="btn btn-success hide" onclick="SaveDevice()"><i class="fa fa-save mr-2"></i>Save</button>
                        </div>

                        <%--<div style="margin-top: 2px;">
                            <div class="row">
                                <div class="col-md-12" style="padding-left: 15px;">
                                    <dxe:ASPxGridView ID="GrdDevice" runat="server" KeyFieldName="HIddenID" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
                                        SettingsPager-Mode="ShowAllRecords" OnDataBinding="grid_DataBinding" OnCustomCallback="GrdDevice_CustomCallback"
                                        Width="100%" ClientInstanceName="cGrdDevice">
                                        
                                        <settingssearchpanel visible="false" delay="5000" />
                                        <columns>
                    <dxe:GridViewDataTextColumn Caption="ID" FieldName="HIddenID"
                        VisibleIndex="0" FixedStyle="Left" Width="0%" Visible="false">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                     <dxe:GridViewDataTextColumn Caption="Model" FieldName="Model"
                        VisibleIndex="1" FixedStyle="Left" Width="20%" >
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Unit Price" FieldName="UnitPrice"
                        VisibleIndex="2" FixedStyle="Left" Width="15%" >
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                     <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity"
                        VisibleIndex="3" FixedStyle="Left" Width="10%" >
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                     <dxe:GridViewDataTextColumn Caption="Amount " FieldName="Amount"
                        VisibleIndex="4" FixedStyle="Left" Width="15%" >
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Remarks" FieldName="Remarks"
                        VisibleIndex="5" FixedStyle="Left" Width="20%" >
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Model_ID" FieldName="Model_ID"
                        VisibleIndex="12" FixedStyle="Left" Width="0%" Visible="false">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>                        
                    </dxe:GridViewDataTextColumn>

                   <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="9" Width="20%">
                        <DataItemTemplate>

                            <a href="javascript:void(0);" onclick="ClickOnEdit('<%# Container.KeyValue %>')" id="a_editInvoice" class="pad" title="Edit">
                                <img src="../../../assests/images/info.png" /></a>

                            <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="pad" title="Delete" id="a_delete">
                                <img src="../../../assests/images/Delete.png" /></a>
                        </DataItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <CellStyle HorizontalAlign="Center"></CellStyle>
                        <HeaderTemplate><span>Actions</span></HeaderTemplate>
                        <EditFormSettings Visible="False"></EditFormSettings>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                </columns>
                                        <settingscontextmenu enabled="true"></settingscontextmenu>
                                        <clientsideevents endcallback="grid_EndCallBack" />
                                        <settingspager numericbuttoncount="10" pagesize="10" showseparators="True" mode="ShowPager">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                    <FirstPageButton Visible="True">
                    </FirstPageButton>
                    <LastPageButton Visible="True">
                    </LastPageButton>
                </settingspager>

                                        <settings showgrouppanel="False" showfooter="false" showstatusbar="Hidden" showhorizontalscrollbar="False" showfilterrow="true" showfilterrowmenu="true" />
                                        <settingsloadingpanel text="Please Wait..." />
                                        <totalsummary>
          
            </totalsummary>
                                    </dxe:ASPxGridView>
                                </div>
                            </div>
                        </div>--%>
                    </div>
                </div>
                <div class="row">
                    <div id="divDetails2" class="hide">
                        <div class="typeHeader"><span id="spnDetails2HeaderName"></span></div>
                        <div class="col-md-2">
                            <label>Model </label>
                            <div class="dropDev">
                                <%--    <dxe:ASPxComboBox ID="ddlDetails2Model" runat="server" ClientInstanceName="cddlDetails2Model" Width="100%">
                                    <clientsideevents selectedindexchanged="ddlDetails2Model_ValueChange" />
                                </dxe:ASPxComboBox>--%>
                                <label id="lblModel2" class="lbInput" runat="server"></label>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Unit Price <span style="color: red;">*</span></label>
                            <div class="dropDev">
                                <dxe:ASPxTextBox ID="txtDetails2UnitPrice" ClientInstanceName="ctxtDetails2UnitPrice" runat="server" Width="100%" DisplayFormatString="{0:0.00}">
                                    <masksettings mask="<0..999999999>.<0..99>" allowmousewheel="false" />
                                    <validationsettings requiredfield-isrequired="false" errordisplaymode="None"></validationsettings>
                                    <%--<clientsideevents lostfocus="OnDetails2UnitPriceLostFocus" />--%>
                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Quantity <span style="color: red;">*</span></label>
                            <div class="dropDev">
                                <dxe:ASPxTextBox ID="txtDetails2Quantity" ClientInstanceName="ctxtDetails2Quantity" runat="server" Width="100%" DisplayFormatString="{0:0}">
                                    <masksettings mask="<0..999999999>" allowmousewheel="false" />
                                    <validationsettings requiredfield-isrequired="false" errordisplaymode="None"></validationsettings>
                                    <%--<clientsideevents lostfocus="OnDetails2QuantityLostFocus" />--%>
                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Amount <span style="color: red;">*</span></label>
                            <div class="dropDev">
                                <dxe:ASPxTextBox ID="txtDetails2Amount" ClientInstanceName="ctxtDetails2Amount" runat="server" Width="100%" DisplayFormatString="{0:0.00}">
                                    <masksettings mask="<0..999999999>.<0..99>" allowmousewheel="false" />
                                    <validationsettings requiredfield-isrequired="false" errordisplaymode="None"></validationsettings>
                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                        <div class="col-md-2 hide" id="ostbVendor">
                            <label>Vendor</label>
                            <div class="dropDev">
                                <%--  <dxe:ASPxComboBox ID="ddlOSTBVendor" runat="server" ClientInstanceName="cddlOSTBVendor" Width="100%">
                                </dxe:ASPxComboBox>--%>
                                <label id="lblOSTBVendor" class="lbInput" runat="server"></label>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <label>Remarks </label>
                            <div>
                                <%--<textarea class="form-control textareaBig" id="txtDetails2Remarks" maxlength="500"></textarea>--%>
                                <label id="lblDetails2Remarks" class="lbInput" runat="server"></label>
                            </div>
                        </div>
                        <%--  <div class="col-md-6" style="margin-top: 23px;">
                            <button type="button" id="btnDetails2Add" class="btn btn-success" onclick="AddDetails2Device()"><i class="fa fa-plus-circle mr-2"></i>Add</button>
                        </div>--%>
                        <%--  <div style="margin-top: 2px;">
                            <div class="row">
                                <div class="col-md-12" style="padding-left: 15px;">
                                    <dxe:ASPxGridView ID="gridDeviceDetails2" runat="server" KeyFieldName="HIddenID" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
                                        SettingsPager-Mode="ShowAllRecords" OnDataBinding="gridDeviceDetails2_DataBinding" OnCustomCallback="gridDeviceDetails2_CustomCallback"
                                        Width="100%" ClientInstanceName="cgridDeviceDetails2">
                                       
                                        <settingssearchpanel visible="false" delay="5000" />
                                        <columns>
                                <dxe:GridViewDataTextColumn Caption="ID" FieldName="HIddenID"
                                    VisibleIndex="0" FixedStyle="Left" Width="0" Visible="false">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn Caption="Model" FieldName="Model"
                                    VisibleIndex="1" FixedStyle="Left" Width="20%" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Unit Price" FieldName="UnitPrice"
                                    VisibleIndex="2" FixedStyle="Left" Width="15%" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity"
                                    VisibleIndex="3" FixedStyle="Left" Width="10%" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn Caption="Amount " FieldName="Amount"
                                    VisibleIndex="4" FixedStyle="Left" Width="10%" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Vendor" FieldName="ostbVendor"
                                    VisibleIndex="5" FixedStyle="Left" Width="10%" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Remarks" FieldName="Remarks"
                                    VisibleIndex="6" FixedStyle="Left" Width="15%" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Model_ID" FieldName="Model_ID"
                                    VisibleIndex="12" FixedStyle="Left" Width="0" Visible="false">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>                        
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn Caption="ostbVendorID" FieldName="ostbVendorID"
                                    VisibleIndex="13" FixedStyle="Left" Width="0" Visible="false">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>                        
                                </dxe:GridViewDataTextColumn>

                               <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="9" Width="20%">
                                    <DataItemTemplate>

                                        <a href="javascript:void(0);" onclick="ClickOnDetails2Edit('<%# Container.KeyValue %>')" id="a_editInvoice" class="pad" title="Edit">
                                            <img src="../../../assests/images/info.png" /></a>

                                        <a href="javascript:void(0);" onclick="OnClickDetails2Delete('<%# Container.KeyValue %>')" class="pad" title="Delete" id="a_delete">
                                            <img src="../../../assests/images/Delete.png" /></a>
                                    </DataItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <CellStyle HorizontalAlign="Center"></CellStyle>
                                    <HeaderTemplate><span>Actions</span></HeaderTemplate>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>
                         </columns>
                                        <settingscontextmenu enabled="true"></settingscontextmenu>
                                        <clientsideevents endcallback="grid_EndCallBack" />
                                        <settingspager numericbuttoncount="10" pagesize="10" showseparators="True" mode="ShowPager">
                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                        <FirstPageButton Visible="True">
                        </FirstPageButton>
                        <LastPageButton Visible="True">
                        </LastPageButton>
                        </settingspager>

                                        <settings showgrouppanel="False" showfooter="false" showstatusbar="Hidden" showhorizontalscrollbar="False" showfilterrow="true" showfilterrowmenu="true" />
                                        <settingsloadingpanel text="Please Wait..." />
                                        <totalsummary>
                        </totalsummary>
                                    </dxe:ASPxGridView>
                                </div>
                            </div>
                        </div>--%>
                    </div>
                </div>
                <div class="row">
                    <div>
                        <div class="typeHeader">Payment Details</div>
                        <div class="col-md-3">
                            <div class="checkbox">
                                <%--<label>
                                    <input type="checkbox" value="" />Pay Using on Account Balance</label>--%>
                                <input type="checkbox" id="chkPayUsingOnAcountBalance" runat="server" disabled />
                                Pay using on Acount Balance
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="checkbox">
                                <%-- <label>
                                    <input type="checkbox" value="" />No Payment (Due)</label>--%>
                                <input type="checkbox" id="chkNoPayment" runat="server" disabled />
                                No Payment (Due)
                            </div>
                        </div>

                        <div class="col-md-6">
                            <label>Remarks/Notes (payment related) </label>
                            <div>
                                <%--<textarea class="form-control" cols="3" rows="3"></textarea>--%>
                                <%--<asp:TextBox ID="txtPaymentRelatedRemarks" runat="server" CssClass="form-control" MaxLength="500" disabled></asp:TextBox>--%>
                                <label id="lblPaymentRelatedRemarks" class="lbInput" runat="server"></label>
                            </div>
                        </div>
                    </div>
                </div>
                
                <div class="row">
                    <div>
                        <div class="typeHeader">Approval Details</div>
                        <div class="col-md-3">
                            <label>Approval Action <span style="color: red;">*</span></label>
                            <div>
                                <select id="ddlApprovalAction" class="form-control">
                                    <option value="0">Select</option>
                                    <option value="1">Approve</option>
                                    <option value="2">Reject</option>
                                    <option value="3">Hold</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <label>Remarks <span style="color: red;">*</span></label>
                            <div>
                                <%--<textarea class="form-control" cols="3" rows="3"></textarea>--%>
                                <input type="text" class="form-control" id="txtApprovalRemarks" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div>


                        <div class="col-md-12" id="divsave">
                            <%--<button class="btn btn-success">Approve</button>--%>
                            <button type="button" onclick="SaveButtonClick('exit');" id="btnSaveNew" class="btn btn-success">Approve</button>
                            <button class="btn btn-danger">Cancel</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divsave" Modal="True">
    </dxe:ASPxLoadingPanel>

    <asp:HiddenField ID="hdAddEdit" runat="server" />
    <asp:HiddenField runat="server" ID="hdnSTBRequisitionID" />
    <asp:HiddenField runat="server" ID="hdnGuid" />
    <asp:HiddenField runat="server" ID="hdnGuid2" />
    <asp:HiddenField runat="server" ID="hdnIsAproval" />
</asp:Content>
