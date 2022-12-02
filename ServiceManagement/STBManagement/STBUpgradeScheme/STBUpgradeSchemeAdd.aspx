<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="STBUpgradeSchemeAdd.aspx.cs" Inherits="ServiceManagement.STBManagement.STBUpgradeScheme.STBUpgradeSchemeAdd" %>

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
                $("#btnSaveNew").addClass('hide');
                $("#btnSaveExit").addClass('hide');
            }
            else if ($("#hdAddEdit").val() == "Update") {
                $("#btnSaveNew").addClass('hide');
            }
        });

        function Save_click(value) {

            LoadingPanel.Show();
            var model = cddlModel.GetValue();
            var txtSTBPrice = ctxtSTBPrice.GetValue();
            var STBRemotePrice = ctxtSTBRemotePrice.GetValue();
            var STBCordAdapterPrice = ctxtSTBCordAdapterPrice.GetValue();
            var isActive = 0;
            if (model == "0") {
                jAlert("Please Select Model.", "Alert", function () {
                    setTimeout(function () {
                        LoadingPanel.Hide();
                        cddlModel.Focus();
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
                STBUpgradeSchemeID: $("#hdnSTBUpgradeSchemeID").val(),
                model: model,
                STBPrice: txtSTBPrice,
                STBRemotePrice: STBRemotePrice,
                STBCordAdapterPrice: STBCordAdapterPrice,
                IsActive: isActive
            }

            $.ajax({
                type: "POST",
                url: "STBUpgradeSchemeAdd.aspx/save",
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
                                msgs = "STB - Upgrade Scheme save successfully.";
                            }
                            else {
                                msgs = "STB - Upgrade Scheme update successfully.";
                            }

                            jAlert(msgs, "Alert", function () {
                                LoadingPanel.Hide();
                                if (value == "Exit") {
                                    window.location.href = "STBUpgradeSchemeList.aspx";
                                }
                                else {
                                    window.location.href = "STBUpgradeSchemeAdd.aspx?Key=Add";
                                }
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
            window.location.href = "STBUpgradeSchemeList.aspx";
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
        <div id="divcross" class="crossBtn"><a href="STBUpgradeSchemeList.aspx"><i class="fa fa-times"></i></a></div>
    </div>
    <div class="form_main clearfix">
        <div class="pmsForm slick boxModel clearfix">
            <div class="row" style="margin-top: 5px">
                <div class="col-md-3">
                    <label>Model <span class="red">*</span></label>
                    <div class="dropDev">
                        <dxe:ASPxComboBox ID="ddlModel" runat="server" ClientInstanceName="cddlModel" Width="100%">
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div class="clear"></div>
                <div class="col-md-3">
                    <label>STB Price </label>
                    <div class="dropDev">
                        <%--<input type="text" class="form-control " />--%>
                        <dxe:ASPxTextBox ID="txtSTBPrice" ClientInstanceName="ctxtSTBPrice" runat="server" Width="100%" DisplayFormatString="{0:0.00}">
                            <masksettings mask="<0..999999999>.<0..99>" allowmousewheel="false" />
                            <validationsettings requiredfield-isrequired="false" errordisplaymode="None"></validationsettings>
                        </dxe:ASPxTextBox>
                    </div>
                </div>
                <div class="col-md-3">
                    <label>STB + Remote Price </label>
                    <div class="dropDev">
                        <%--<input type="text" class="form-control " />--%>
                        <dxe:ASPxTextBox ID="txtSTBRemotePrice" ClientInstanceName="ctxtSTBRemotePrice" runat="server" Width="100%" DisplayFormatString="{0:0.00}">
                            <masksettings mask="<0..999999999>.<0..99>" allowmousewheel="false" />
                            <validationsettings requiredfield-isrequired="false" errordisplaymode="None"></validationsettings>
                        </dxe:ASPxTextBox>
                    </div>
                </div>
                <div class="col-md-3">
                    <label>STB + Cord/Adapter Price </label>
                    <div class="dropDev">
                        <%--<input type="text" class="form-control " />--%>
                        <dxe:ASPxTextBox ID="txtSTBCordAdapterPrice" ClientInstanceName="ctxtSTBCordAdapterPrice" runat="server" Width="100%" DisplayFormatString="{0:0.00}">
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
                <%--<button type="button" id="btnSave" class="btn btn-primary" onclick="Save_click();">Save</button>--%>
                 <button type="button" onclick="Save_click('new');" id="btnSaveNew" class="btn btn-success">Save & New</button>
                <button type="button" onclick="Save_click('Exit');" id="btnSaveExit" class="btn btn-primary">Save & Exit</button>
                <button type="button" id="btnCancel" class="btn btn-danger" onclick="cancel();">Cancel</button>
            </div>
        </div>
    </div>
    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divsave"
        Modal="True">
    </dxe:ASPxLoadingPanel>

    <asp:HiddenField ID="hdAddEdit" runat="server" />
    <asp:HiddenField runat="server" ID="hdnSTBUpgradeSchemeID" />
</asp:Content>
