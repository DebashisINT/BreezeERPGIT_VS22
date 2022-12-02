<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="setTopBoxAdd.aspx.cs" Inherits="ServiceManagement.STBManagement.STB.setTopBoxAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../ServiceManagement/Transaction/CSS/ReceiptChallan.css" rel="stylesheet" />
    <style>
        .mtop8 {
            margin-top: 8px;
        }

        .ptTbl > tbody > tr > td {
            padding-right: 10px;
            padding-bottom: 8px;
        }

        .headerPy {
            background: #66b1c7;
            padding: 4px 10px;
            border-radius: 5px 5px 0 0;
            font-weight: 500;
            color: #f1f1f1;
            margin-top: 5px;
        }
    </style>

    <script>
        $(document).ready(function () {
            if ($("#hdAddEdit").val() == "view") {
                $("#btnSave").addClass('hide');
            }
        });

        function Save_click() {

            LoadingPanel.Show();
            var model = cddlModel.GetValue();
            var STBType = cddlSTBType.GetValue();
            var Manufacturer = cddlManufacturer.GetValue();

            var STBmodel = cddlSTBModel.GetValue();

            var PriceDAS1 = ctxtPriceDAS1.GetValue();
            var PriceDAS2 = ctxtPriceDAS2.GetValue();
            var PriceDAS3 = ctxtPriceDAS3.GetValue();
            var PriceDAS4 = ctxtPriceDAS4.GetValue();
            var isActive = 0;
            if (model == "0" && STBmodel == "0") {
                jAlert("Please Select Model.", "Alert", function () {
                    setTimeout(function () {
                        LoadingPanel.Hide();
                        cddlModel.Focus();
                        return
                    }, 200);
                });
                return
            }

            if (STBType == "0") {
                jAlert("Please Select STB Type.", "Alert", function () {
                    setTimeout(function () {
                        LoadingPanel.Hide();
                        cddlSTBType.Focus();
                        return
                    }, 200);
                });
                return
            }

            if ($('#chkIsActive').is(':checked') == true) {
                isActive = 1;
            }

            var Apply = {
                Action: $("#hdAddEdit").val(),
                SetTopBoxID: $("#hdnSetTopBoxID").val(),
                model: model,
                STBType: STBType,
                Manufacturer: Manufacturer,
                PriceDAS1: PriceDAS1,
                PriceDAS2: PriceDAS2,
                PriceDAS3: PriceDAS3,
                PriceDAS4: PriceDAS4,
                IsActive: isActive,
                STBmodel: STBmodel
            }

            $.ajax({
                type: "POST",
                url: "setTopBoxAdd.aspx/save",
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
                            if ($("#hdAddEdit").val() == "Insert") {
                                msgs = "Set Top Box save successfully.";
                            }
                            else {
                                msgs = "Set Top Box update successfully.";
                            }

                            jAlert(msgs, "Alert", function () {
                                LoadingPanel.Hide();
                                window.location.href = "setTopBox.aspx";
                            });
                        }
                        else {
                            jAlert(response.d.split('~')[0]);
                            LoadingPanel.Hide();
                            return
                        }
                    }
                },
                error: function (response) {
                    console.log(response);
                }
            });
        }

        function cancel() {
            window.location.href = "setTopBox.aspx";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>
                <span id="HeaderName" runat="server"></span>
            </h3>
        </div>
        <div id="divcross" class="crossBtn"><a href="setTopBox.aspx"><i class="fa fa-times"></i></a></div>
    </div>
    <div class="form_main clearfix">
        <div class="pmsForm slick boxModel clearfix">
            <div class="row" style="margin-top: 5px">
                <div class="col-md-3">
                    <label>Model-STB-01 <span class="red">*</span></label>
                    <div class="dropDev">
                        <dxe:ASPxComboBox ID="ddlModel" runat="server" ClientInstanceName="cddlModel" Width="100%">
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div class="col-md-3">
                    <label>Model-STB-02 <span class="red">*</span></label>
                    <div class="dropDev">
                        <dxe:ASPxComboBox ID="ddlSTBModel" runat="server" ClientInstanceName="cddlSTBModel" Width="100%">
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div class="col-md-3">
                    <label>STB Type <span class="red">*</span></label>
                    <div class="dropDev">
                        <%-- <select class="form-control">
                            <option>Select</option>
                        </select>--%>
                        <dxe:ASPxComboBox ID="ddlSTBType" runat="server" ClientInstanceName="cddlSTBType" Width="100%">
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div class="col-md-3">
                    <label>Manufacturer</label>
                    <div class="dropDev">
                        <%-- <select class="form-control">
                            <option>Select</option>
                        </select>--%>
                        <dxe:ASPxComboBox ID="ddlManufacturer" runat="server" ClientInstanceName="cddlManufacturer" Width="100%">
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div class="clear"></div>
                <div class="col-md-3">
                    <label>Price DAS - I</label>
                    <div class="dropDev">
                        <%--<input type="text" class="form-control " />--%>
                        <dxe:ASPxTextBox ID="txtPriceDAS1" ClientInstanceName="ctxtPriceDAS1" runat="server" Width="100%" DisplayFormatString="{0:0.00}">
                            <masksettings mask="<0..999999999>.<0..99>" allowmousewheel="false" />
                            <validationsettings requiredfield-isrequired="false" errordisplaymode="None"></validationsettings>
                        </dxe:ASPxTextBox>
                    </div>
                </div>
                <div class="col-md-3">
                    <label>Price DAS - II</label>
                    <div class="dropDev">
                        <%--<input type="text" class="form-control " />--%>
                        <dxe:ASPxTextBox ID="txtPriceDAS2" ClientInstanceName="ctxtPriceDAS2" runat="server" Width="100%" DisplayFormatString="{0:0.00}">
                            <masksettings mask="<0..999999999>.<0..99>" allowmousewheel="false" />
                            <validationsettings requiredfield-isrequired="false" errordisplaymode="None"></validationsettings>
                        </dxe:ASPxTextBox>
                    </div>
                </div>
                <div class="col-md-3">
                    <label>Price DAS - III</label>
                    <div class="dropDev">
                        <%--<input type="text" class="form-control " />--%>
                        <dxe:ASPxTextBox ID="txtPriceDAS3" ClientInstanceName="ctxtPriceDAS3" runat="server" Width="100%" DisplayFormatString="{0:0.00}">
                            <masksettings mask="<0..999999999>.<0..99>" allowmousewheel="false" />
                            <validationsettings requiredfield-isrequired="false" errordisplaymode="None"></validationsettings>
                        </dxe:ASPxTextBox>
                    </div>
                </div>
                <div class="col-md-3">
                    <label>Price DAS - IV</label>
                    <div class="dropDev">
                        <%--<input type="text" class="form-control " />--%>
                        <dxe:ASPxTextBox ID="txtPriceDAS4" ClientInstanceName="ctxtPriceDAS4" runat="server" Width="100%" DisplayFormatString="{0:0.00}">
                            <masksettings mask="<0..999999999>.<0..99>" allowmousewheel="false" />
                            <validationsettings requiredfield-isrequired="false" errordisplaymode="None"></validationsettings>
                        </dxe:ASPxTextBox>
                    </div>
                </div>
                <div class="clear"></div>
                <div class="col-md-3">
                    <div class="checkbox">
                        <label>
                            <input type="checkbox" id="chkIsActive" runat="server" value="" />Is Active</label>
                    </div>
                </div>
            </div>
            <div id="divsave">
                <button type="button" id="btnSave" class="btn btn-primary" onclick="Save_click();">Save</button>
                <button type="button" id="btnCancel" class="btn btn-danger" onclick="cancel();">Cancel</button>
            </div>
        </div>
    </div>
    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divsave"
        Modal="True">
    </dxe:ASPxLoadingPanel>

    <asp:HiddenField ID="hdAddEdit" runat="server" />
    <asp:HiddenField runat="server" ID="hdnSetTopBoxID" />
</asp:Content>
