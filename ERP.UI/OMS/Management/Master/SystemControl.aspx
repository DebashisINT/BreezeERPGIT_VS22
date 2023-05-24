<%--================================================== Revision History =============================================
1.0   Pallab    V2.0.38      24-05-2023          0026218: System Control module design modification & check in small device
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="SystemControl.aspx.cs" Inherits="ERP.OMS.Management.Master.SystemControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />
    <script src="../Activities/JS/SearchPopup.js"></script>

    <script>
        //document.getElementById("txt_noofcopies").onkeyup = function () {
        //    var input = parseInt(this.value);
        //    if (input < 1 || input > 3)
        //        alert("Value should be between 1 - 3");
        //    return;
        //}

        $("input[name='num']").change(function () {
            number = $("input[name='num']").val()
            if (number <= 0 || number >= 4) {
                $("input[name='num']").val("");
                alert("0 - 100");
            }
        });

    </script>


    <style>
        #txtFollowupDb_EC {
            visibility: visible;
            display: none;
        }

        .dxgvControl_PlasticBlue td.dxgvBatchEditModifiedCell_PlasticBlue {
            background: #fff !important;
        }
    </style>




    <script>

        var globalRowIndex;
        function gridFocusedRowChanged(s, e) {
            globalRowIndex = e.visibleIndex;
        }

        function OnInit(s, e) {

        }
        function OnBatchStartEdit(s, e) {

        }

        function DaysTextChange(s, e) {
            var DaysId = (cBackEntries.GetEditor('DaysId').GetValue() != null) ? cBackEntries.GetEditor('DaysId').GetValue() : "0";
            if (parseInt(DaysId) > 30) {
                jAlert("Please enter below or equal 30 days.");
                cBackEntries.GetEditor('DaysId').SetValue(0);
                //cBackEntries.batchEditApi.StartEdit(globalRowIndex, 3);
                //return;
            }

        }
        function ShowError(obj) {

            //IntializeGlobalVariables(s);
            if (cBackEntries.cpupdatemssg != null) {
                jAlert(cBackEntries.cpupdatemssg);
                cBackEntries.cpupdatemssg = null;
            }

        }

        function saveandupdate() {

            cBackEntries.UpdateEdit();
            cBackEntries.PerformCallback();
        }
        //***Sales Promotion***//
        function SalesPromotionButnClick(s, e) {

            $("#SalesPromotionTable").empty();
            var html = "<table border='1' width='100%' class='dynamicPopupTbl'><tr class='HeaderStyle'><th>Main Account Name</th><th>Short Name</th></tr></table>"
            $("#SalesPromotionTable").html(html);
            setTimeout(function () { $("#txtSalesPromotionSearch").focus(); }, 500);
            $('#txtSalesPromotionSearch').val('');
            //shouldCheck = 1;
            $('#mainActMsg').hide();
            $('#SalesPromotionModel').modal('show');

        }

        function SalesPromotionKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumpadEnter") {
                shouldCheck = 0;
                s.OnButtonClick(0);
            }
        }
        function SalesPromotionNewkeydown(e) {
            var OtherDetails = {}
            if ($.trim($("#txtSalesPromotionSearch").val()) == "" || $.trim($("#txtSalesPromotionSearch").val()) == null) {
                return false;
            }

            OtherDetails.SearchKey = $("#txtSalesPromotionSearch").val();
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                if ($("#txtSalesPromotionSearch").val() == "")
                    return;
                var HeaderCaption = [];
                HeaderCaption.push("Main Account Name");
                HeaderCaption.push("Short Name");

                //callonServer("/OMS/Management/Activities/Services/Master.asmx/GetSalesPromotion", OtherDetails, "SalesPromotionTable", HeaderCaption, "SalesPromotionIndex", "SetSalesPromotion");
                callonServer("/OMS/Management/Master/SystemControl.aspx/GetSalesPromotion", OtherDetails, "SalesPromotionTable", HeaderCaption, "SalesPromotionIndex", "SetSalesPromotion");
            }
            else if (e.code == "ArrowDown") {
                if ($("input[SalesPromotionIndex=0]"))
                    $("input[SalesPromotionIndex=0]").focus();
            }
            else if (e.code == "Escape") {
                //  
                $('#SalesPromotionModel').modal('hide');
            }
        }
        function SetSalesPromotion(id, name, e) {
            $('#SalesPromotionModel').modal('hide');
            SetSalesPromotiontxt(id, name);
        }
        function ValueSelected(e, indexName) {
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var Id = e.target.parentElement.parentElement.cells[0].innerText;
                var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                if (Id) {
                    if (indexName == "SalesPromotionIndex") {
                        $('#SalesPromotionModel').modal('hide');
                        SetSalesPromotiontxt(Id, name);
                    }
                    else if (indexName == "DeliveryChargesIndex") {
                        $('#DeliveryChargesModel').modal('hide');
                        SetDeliveryChargestxt(Id, name);
                    }

                }

            }
            else if (e.code == "ArrowDown") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex++;
                if (thisindex < 10)
                    $("input[" + indexName + "=" + thisindex + "]").focus();
            }
            else if (e.code == "ArrowUp") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex--;
                if (thisindex > -1)
                    $("input[" + indexName + "=" + thisindex + "]").focus();
                else {
                    if (indexName == "SalesPromotionIndex")
                        $('#txtSalesPromotionSearch').focus();
                    else if (indexName == "DeliveryChargesIndex") {
                        $('#txtDeliveryChargesSearch').focus();
                    }


                }
            }
            else if (e.code == "Escape") {
                if (indexName == "SalesPromotionIndex") {
                    $('#SalesPromotionModel').modal('hide');
                }
                else if (indexName == "DeliveryChargesIndex") {
                    $('#DeliveryChargesModel').modal('hide');
                }
            }
        }
        function SetSalesPromotiontxt(id, name) {
            ctxtSalesPromotion.SetText(name);
            document.getElementById('hdnSalesPromotionId').value = id;
            ctxtDeliveryCharges.Focus();
        }
    </script>
    <script>
        //***Delivary Charges ***//
        function DeliveryChargesButnClick(s, e) {
            $("#DeliveryChargesTable").empty();
            var html = "<table border='1' width='100%' class='dynamicPopupTbl'><tr class='HeaderStyle'><th>Main Account Name</th><th>Short Name</th></tr></table>"
            $("#DeliveryChargesTable").html(html);
            setTimeout(function () { $("#txtDeliveryChargesSearch").focus(); }, 500);
            $('#txtDeliveryChargesSearch').val('');
            //shouldCheck = 1;
            $('#mainActMsg').hide();
            $('#DeliveryChargesModel').modal('show');

        }
        function DeliveryChargesKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                shouldCheck = 0;
                s.OnButtonClick(0);
            }
        }
        function DeliveryChargesNewkeydown(e) {
            var OtherDetails = {}
            if ($.trim($("#txtDeliveryChargesSearch").val()) == "" || $.trim($("#txtDeliveryChargesSearch").val()) == null) {
                return false;
            }

            OtherDetails.SearchKey = $("#txtDeliveryChargesSearch").val();
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                if ($("#txtDeliveryChargesSearch").val() == "")
                    return;
                var HeaderCaption = [];
                HeaderCaption.push("Main Account Name");
                HeaderCaption.push("Short Name");

                //callonServer("/OMS/Management/Activities/Services/Master.asmx/GetSalesPromotion", OtherDetails, "SalesPromotionTable", HeaderCaption, "SalesPromotionIndex", "SetSalesPromotion");
                callonServer("/OMS/Management/Master/SystemControl.aspx/GetSalesPromotion", OtherDetails, "DeliveryChargesTable", HeaderCaption, "DeliveryChargesIndex", "SetDeliveryCharges");
            }
            else if (e.code == "ArrowDown") {
                if ($("input[DeliveryChargesIndex=0]"))
                    $("input[DeliveryChargesIndex=0]").focus();
            }
            else if (e.code == "Escape") {
                //  
                $('#DeliveryChargesModel').modal('hide');
            }
        }
        function SetDeliveryCharges(Id, name, e) {
            $('#DeliveryChargesModel').modal('hide');
            SetDeliveryChargestxt(Id, name);
        }

        function SetDeliveryChargestxt(id, name) {
            ctxtDeliveryCharges.SetText(name);
            document.getElementById('hdnDeliveryChargesId').value = id;
            $("#chkBarcodeGen").focus();

        }
    </script>
    <script type="text/javascript">
        /****************Save and ready************/
        $(document).ready(function () {
            if ($("#hdnActiveEInvoice").val() == "1") {
                dvTurnover.style.display = 'block';
            }
            else if ($("#hdnActiveEInvoice").val() == "0") {
                dvTurnover.style.display = 'none';
            }

            ctxtSalesPromotion.Focus()
        });

        function SaveButtonClick(flag) {
            var chlk;
            $("#MandatorysSalesPromotion").hide();
            $("#MandatorysDeliveryCharges").hide();

            <%--  if (ctxtSalesPromotion.GetText() == "") {
                $("#MandatorysSalesPromotion").show();
                document.getElementById('<%= txtSalesPromotion.ClientID %>').focus();
                return false;
            }--%>
           <%-- if (ctxtDeliveryCharges.GetText() == "") {
                $("#MandatorysDeliveryCharges").show();
                document.getElementById('<%= txtDeliveryCharges.ClientID %>').focus();
                return false;
            }--%>
            if ($("#chkBarcodeGen").prop("checked") == true) {
                chlk = 1;
            }
            else {
                chlk = 0;
            }
            var ActualWarehouseID = ccmbSourceWarehouse.GetValue();
            var ReplaceableWarehouseID = ccmbDestWarehouse.GetValue();
            var SehemaID = ccmbAdjustmentNumbering.GetValue();
            var DefectWarehouseID = ccmbDefectWarehouse.GetValue();
            var DefectSehemaID = ccmbAutoNumberingforDefect.GetValue();
            var mainAccountpartyjournal = ccmbMainAccount.GetValue();
            var Turnover = ctxtTurnover.GetValue();
            var ReturnTransferNumbering = ccmbReturnTNumbering.GetValue();
            // Mantis Issue 24818
            var OpprSupprDay = ccmbOpprSupprDay.GetValue();
            // End of Mantis Issue 24818
            $.ajax({
                type: "POST",
                url: "/OMS/Management/Master/SystemControl.aspx/addSystemControl",
                // Mantis Issue 24818 [OpprSupprDay  added]
                data: JSON.stringify({ "SalesPromotion": $.trim($("#hdnSalesPromotionId").val()), "DeliveryCharges": $.trim($("#hdnDeliveryChargesId").val()), "BarcodeGeneration": chlk, "action": flag, "followDaylmt": ctxtFollowupDb.GetText(), "LastDBName": lastdbname.GetText(), "ActualWarehouseID": ActualWarehouseID, "ReplaceableWarehouseID": ReplaceableWarehouseID, "SehemaID": SehemaID, "DefectWarehouseID": DefectWarehouseID, "DefectSehemaID": DefectSehemaID, "mainAccountPartyjournal": mainAccountpartyjournal, "TurnOver": Turnover, "RtnTransferNum": ReturnTransferNumbering, "OpprSupprDay": OpprSupprDay }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                global: false,
                async: false,
                success: function (msg) {
                    if (msg.d) {
                        if (flag == "GetSystemControl") {
                            setValue()
                        }
                        else if (flag == "Insert") {
                            alert("added Successfully");
                            return false;
                        }

                    }
                },
                error: function (response) {
                    console.log(response);
                }
            });

        }



        function SaveButtonClickImagecompresssettig(flag) {
            var chlk;

            if ($("#txtwidth").val() != '') {
                $.ajax({
                    type: "POST",
                    url: "/OMS/Management/Master/SystemControl.aspx/addSystemProductControl",
                    data: JSON.stringify({ "width": $.trim($("#txtwidth").val()), "height": $.trim($("#txtheight").val()) }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    global: false,
                    async: false,
                    success: function (msg) {
                        if (msg.d) {

                            jAlert('Updated Successfully');

                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });

            }
            else {

                jAlert('Width is mandatory');
            }



        }

        function SaveButtonClickNoofCopies(flag) {
            var chlk;

            if ($("#ddlnoofcopies").val() != '') {
                $.ajax({
                    type: "POST",
                    url: "/OMS/Management/Master/SystemControl.aspx/updateSystemControlCopies",
                    data: JSON.stringify({ "No_Of_Copies": $("#ddlnoofcopies").val() }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    global: false,
                    async: false,
                    success: function (msg) {
                        if (msg.d) {

                            jAlert('Updated Successfully');

                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });

            }
            else {

                jAlert('Width is mandatory');
            }



        }
        function SaveButtonClickInfluencer(flag) {
            var chlk;

            if (parseFloat(ctxtInfLimt.GetText()) > 0) {
                $.ajax({
                    type: "POST",
                    url: "/OMS/Management/Master/SystemControl.aspx/updateSystemControlInfluencer",
                    data: JSON.stringify({ "type": $("#DropDownList1").val(), "amount": ctxtInfLimt.GetText() }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    global: false,
                    async: false,
                    success: function (msg) {
                        if (msg.d) {

                            jAlert('Updated Successfully');

                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });

            }
            else {

                jAlert('Limit is mandatory');
            }



        }


    </script>

    <script>
        function SaveButtonClickHoliday(flag) {
            if ($("#ddlHoliday").val() > 0) {
                $.ajax({
                    type: "POST",
                    url: "/OMS/Management/Master/SystemControl.aspx/updateSystemControlHoliday",
                    data: JSON.stringify({ "holiday_id": $("#ddlHoliday").val() }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    global: false,
                    async: false,
                    success: function (msg) {
                        if (msg.d) {
                            jAlert('Updated Successfully');
                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
            }
            else {

                jAlert('Please select holiday.');
            }
        }
    </script>

    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 1;
            margin-bottom: 0;
            -webkit-appearance: none;
        }

        /*#grid {
            max-width: 98% !important;
        }*/
        #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_partyInvDt
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_partyInvDt_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
        #dt_partyInvDt_B-1 #dt_partyInvDt_B-1Img
        {
            display: none;
        }

        /*select
        {
            -webkit-appearance: auto;
        }*/

        .calendar-icon
        {
                right: 18px;
                bottom: 6px;
        }
        .padTabtype2 > tbody > tr > td
        {
            vertical-align: bottom;
        }
        #rdl_Salesquotation
        {
            margin-top: 0px;
        }

        .lblmTop8>span, .lblmTop8>label
        {
            margin-top: 8px !important;
        }

        .col-md-2, .col-md-4 {
    margin-bottom: 5px;
}

        .simple-select::after
        {
                top: 34px;
            right: 13px;
        }

        .dxeErrorFrameWithoutError_PlasticBlue.dxeControlsCell_PlasticBlue
        {
            padding: 0;
        }

        .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .backSelect {
    background: #42b39e !important;
}

        #ddlInventory
        {
                -webkit-appearance: auto;
        }

        /*.wid-90
        {
            width: 100%;
        }
        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content
        {
            width: 97%;
        }*/
        .newLbl
        {
                margin: 3px 0 !important;
        }

        .lblBot > span, .lblBot > label
        {
                margin-bottom: 3px !important;
        }

        .col-md-2 > label, .col-md-2 > span, .col-md-1 > label, .col-md-1 > span
        {
            margin-top: 8px !important;
            font-size: 14px;
        }

        .col-md-6 span
        {
            font-size: 14px;
        }

        #gridDEstination
        {
            width:99% !important;
        }

        #txtEntity , #txtCustName
        {
            width: 100%;
        }
        .col-md-6 span
        {
            margin-top: 8px !important;
        }

        .rds
        {
            margin-top: 10px !important;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , select
        {
            height: 30px !important;
            
        }
        select
        {
            background-color: transparent;
                padding: 0 20px 0 5px !important;
        }

        .newLbl
        {
            font-size: 14px;
            margin: 3px 0 !important;
            font-weight: 500 !important;
            margin-bottom: 0 !important;
            line-height: 20px;
        }

        .crossBtn {
            top: 25px !important;
            right: 25px !important;
        }

        .wrapHolder
        {
            height: 60px;
        }
        #rdl_SaleInvoice
        {
            margin-top: 12px;
        }

        .dxeRoot_PlasticBlue
        {
            width: 100% !important;
        }

        #ShowFilter {
            padding-bottom: 0px !important;
        }

        @media only screen and (max-width: 1380px) and (min-width: 1300px)
        {
            .col-xs-1, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9, .col-xs-10, .col-xs-11, .col-xs-12, .col-sm-1, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-sm-10, .col-sm-11, .col-sm-12, .col-md-1, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-md-10, .col-md-11, .col-md-12, .col-lg-1, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-lg-10, .col-lg-11, .col-lg-12
            {
                 padding-right: 10px;
                 padding-left: 10px;
            }
            .simple-select::after
        {
                top: 34px;
            right: 8px;
        }
            .calendar-icon
        {
                right: 14px;
                bottom: 6px;
        }
        }

    </style>
    <%--Rev end 1.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">System Control
            </h3>
        </div>
    </div>


        <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="2" ClientInstanceName="page"
        Font-Size="12px" Width="100%">
        <TabPages>
            <dxe:TabPage Name="General" Text="General">
                <ContentCollection>
                    <dxe:ContentControl runat="server">
                        <div class="row">
                            <div class="col-md-3">
                                <label>Sales Promotion</label>
                                <dxe:ASPxButtonEdit ID="txtSalesPromotion" TabIndex="1" runat="server" ReadOnly="true" ClientInstanceName="ctxtSalesPromotion" Width="100%">
                                    <Buttons>
                                        <dxe:EditButton>
                                        </dxe:EditButton>
                                    </Buttons>
                                    <ClientSideEvents ButtonClick="function(s,e){SalesPromotionButnClick();}" KeyDown="function(s,e){SalesPromotionKeyDown(s,e);}" />
                                </dxe:ASPxButtonEdit>
                                <span id="MandatorysSalesPromotion" class="fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none; right: -3px; top: 24px;"
                                    title="Mandatory"></span>
                            </div>
                            <div class="col-md-3">
                                <label>Delivery Charges</label>
                                <dxe:ASPxButtonEdit ID="txtDeliveryCharges" TabIndex="2" runat="server" ReadOnly="true" ClientInstanceName="ctxtDeliveryCharges" Width="100%">
                                    <Buttons>
                                        <dxe:EditButton>
                                        </dxe:EditButton>
                                    </Buttons>
                                    <ClientSideEvents ButtonClick="function(s,e){DeliveryChargesButnClick();}" KeyDown="function(s,e){DeliveryChargesKeyDown(s,e);}" />
                                </dxe:ASPxButtonEdit>
                                <span id="MandatorysDeliveryCharges" class="fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none; right: -3px; top: 24px;"
                                    title="Mandatory"></span>
                            </div>


                            <div class="col-md-2" style="margin-top: 30px">
                                <asp:CheckBox ID="chkBarcodeGen" ClientIDMode="Static" runat="server" TabIndex="3" />
                                Barcode Generation
                            </div>

                            <div class="col-md-3">
                                <label>Next Followup Date restriction in Followup module</label>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 25%">
                                            <dxe:ASPxTextBox ID="txtFollowupDb" ClientInstanceName="ctxtFollowupDb" runat="server" Width="100%">
                                                <MaskSettings Mask="&lt;0..365&gt;" AllowMouseWheel="false" />
                                            </dxe:ASPxTextBox>
                                        </td>
                                        <td>
                                            <span style="padding-left: 10px">Day(s)</span>
                                        </td>
                                    </tr>
                                </table>




                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <label>Last Database Name</label>
                                <div class="relative">
                                    <dxe:ASPxTextBox ID="lastdbname" runat="server" MaxLength="200">
                                    </dxe:ASPxTextBox>
                                </div>
                                <%-- <table style="width: 100%">
                                    <tr>
                                        <td style="width: 25%">
                                            <dxe:ASPxTextBox ID="lastdbname" runat="server" MaxLength="200">
                                            </dxe:ASPxTextBox>
                                        </td>

                                    </tr>
                                </table>--%>
                            </div>

                            <div class="col-md-2 " id="DivActualWarehouse" runat="server">
                                <label class="mTop5">Actual Warehouse</label>
                                <div class="relative">
                                    <dxe:ASPxComboBox ID="cmbSourceWarehouse" runat="server" ClientInstanceName="ccmbSourceWarehouse"
                                        Width="100%">
                                        <%--    <ClientSideEvents ValueChanged="SourceWH_ValueChange"></ClientSideEvents>--%>
                                    </dxe:ASPxComboBox>
                                    <%-- <span id="MandatoryWarehouse" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                <dxe:ASPxLabel runat="server" ID="lblSourceWHAddress" ClientInstanceName="clblSourceWHAddress" Text="" CssClass="text-muted lblWarhouse"></dxe:ASPxLabel>--%>
                                </div>
                            </div>

                            <div class="col-md-2 " id="DivReplaceablehouse" runat="server">
                                <label class="mTop5">Replaceable Warehouse</label>
                                <div class="relative">
                                    <dxe:ASPxComboBox ID="cmbDestWarehouse" runat="server" ClientInstanceName="ccmbDestWarehouse"
                                        Width="100%">
                                        <%--  <ClientSideEvents ValueChanged="DestWH_ValueChange"></ClientSideEvents>--%>
                                    </dxe:ASPxComboBox>
                                    <%--   <span id="MandatorycmbDestWarehouse" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                <dxe:ASPxLabel runat="server" ID="lblDestWHAddress" ClientInstanceName="clblDestWHAddress" Text="" CssClass="text-muted lblWarhouse"></dxe:ASPxLabel>--%>
                                </div>
                            </div>
                            <div class="col-md-2 " id="DivDefectWarehouse" runat="server">
                                <label class="mTop5">Defect Warehouse</label>
                                <div class="relative">
                                    <dxe:ASPxComboBox ID="cmbDefectWarehouse" runat="server" ClientInstanceName="ccmbDefectWarehouse"
                                        Width="100%">
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>



                        </div>
                        <div class="row">
                            <div class="col-md-2 " id="DivAutoAdjustmentNumbering" runat="server">
                                <label class="mTop5">Auto Adjustment Numbering</label>
                                <div class="relative">
                                    <dxe:ASPxComboBox ID="cmbAdjustmentNumbering" runat="server" ClientInstanceName="ccmbAdjustmentNumbering"
                                        Width="100%">
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                            <div class="col-md-2 " id="DivAutoNumberingforDefect" runat="server">
                                <label class="mTop5">Auto Numbering for Defect</label>
                                <div class="relative">
                                    <dxe:ASPxComboBox ID="cmbAutoNumberingforDefect" runat="server" ClientInstanceName="ccmbAutoNumberingforDefect"
                                        Width="100%">
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                            <div class="col-md-2 " id="DivReturnTNumbering" runat="server">
                                <label class="mTop5">Return transfer Numbering</label>
                                <div class="relative">
                                    <dxe:ASPxComboBox ID="cmbReturnTNumbering" runat="server" ClientInstanceName="ccmbReturnTNumbering"
                                        Width="100%">
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                            <div class="col-md-2 ">
                                <label class="mTop5">Main Account for Party Journal</label>
                                <div class="relative">
                                    <dxe:ASPxComboBox ID="cmbMainAccount" runat="server" ClientInstanceName="ccmbMainAccount"
                                        Width="100%">
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                            <div class="col-md-2 " runat="server" id="dvTurnover">
                                <label class="mTop5">Turnover</label>
                                <div class="relative">
                                    <dxe:ASPxTextBox ID="txtTurnover" ClientInstanceName="ctxtTurnover"
                                        runat="server" Width="100%">
                                        <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                                        <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                            <%--Mantis Issue 24818--%>
                            <div class="col-md-3 " runat="server" id="Div1">
                                 <label class="mTop5">Opportunity Close - the Suppression Day of Month</label>
                                <div class="relative">
                                    <dxe:ASPxComboBox ID="cmbOpprSupprDay" runat="server" ClientInstanceName="ccmbOpprSupprDay"
                                        Width="30%">
                                         <Items>
                                            <dxe:ListEditItem Text="SELECT" Value="0"  />
                                            <dxe:ListEditItem Text="1" Value="1"  />
                                            <dxe:ListEditItem Text="2" Value="2" />
                                            <dxe:ListEditItem Text="3" Value="3" />
                                            <dxe:ListEditItem Text="4" Value="4" />
                                            <dxe:ListEditItem Text="5" Value="5" />
                                            <dxe:ListEditItem Text="6" Value="6" />
                                            <dxe:ListEditItem Text="7" Value="7" />
                                            <dxe:ListEditItem Text="8" Value="8" />
                                            <dxe:ListEditItem Text="9" Value="9" />
                                            <dxe:ListEditItem Text="10" Value="10" />
                                            <dxe:ListEditItem Text="11" Value="11" />
                                            <dxe:ListEditItem Text="12" Value="12" />
                                            <dxe:ListEditItem Text="13" Value="13" />
                                            <dxe:ListEditItem Text="14" Value="14" />
                                            <dxe:ListEditItem Text="15" Value="15" />
                                            <dxe:ListEditItem Text="16" Value="16" />
                                            <dxe:ListEditItem Text="17" Value="17" />
                                            <dxe:ListEditItem Text="18" Value="18" />
                                            <dxe:ListEditItem Text="19" Value="19" />
                                            <dxe:ListEditItem Text="20" Value="20" />
                                            <dxe:ListEditItem Text="21" Value="21" />
                                            <dxe:ListEditItem Text="22" Value="22" />
                                            <dxe:ListEditItem Text="23" Value="23" />
                                            <dxe:ListEditItem Text="24" Value="24" />
                                            <dxe:ListEditItem Text="25" Value="25" />
                                            <dxe:ListEditItem Text="26" Value="26" />
                                            <dxe:ListEditItem Text="27" Value="27" />
                                            <dxe:ListEditItem Text="28" Value="28" />
                                        </Items>
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                            <%--End of Mantis Issue 24818--%>
                        </div>


                        <div class="clearfix"></div>
                        <div style="padding: 20px 10px 10px 10px;">
                            <dxe:ASPxButton ID="btnSaveRecords" TabIndex="4" ClientInstanceName="cbtnSaveRecords" runat="server" AutoPostBack="False" Text="S&#818;ave" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {SaveButtonClick('Insert');}" />
                            </dxe:ASPxButton>

                        </div>

                    </dxe:ContentControl>
                </ContentCollection>
            </dxe:TabPage>



            <dxe:TabPage Name="Image" Text="Image Compressed">
                <ContentCollection>
                    <dxe:ContentControl runat="server">

                        <div class="col-md-1">

                            <label>Width: </label>
                            <asp:TextBox ID="txtwidth" runat="server" Width="80px"></asp:TextBox>

                        </div>

                        <div class="col-md-1">

                            <label>Height: </label>
                            <asp:TextBox ID="txtheight" runat="server" Width="80px"></asp:TextBox>

                        </div>
                        <div class="col-md-2"></div>
                        <div style="padding: 28px 10px 10px 10px;">
                            <dxe:ASPxButton ID="btn_imagecompressed" TabIndex="4" ClientInstanceName="btn_imagecompressed" runat="server" AutoPostBack="False" Text="S&#818;ave" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {SaveButtonClickImagecompresssettig('Insert');}" />
                            </dxe:ASPxButton>
                        </div>


                    </dxe:ContentControl>
                </ContentCollection>
            </dxe:TabPage>

            <dxe:TabPage Name="Image" Text="Print Copies" Visible="false">
                <ContentCollection>
                    <dxe:ContentControl runat="server">

                        <div class="col-md-1">

                            <label>No. of Copies: </label>
                            <%--<asp:TextBox ID="TextBox1" runat="server" Width="100%" TextMode="Number"></asp:TextBox>--%>
                            <asp:DropDownList ID="ddlnoofcopies" runat="server" Width="100%">
                                <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                <asp:ListItem Value="3" Text="3"></asp:ListItem>
                            </asp:DropDownList>

                        </div>


                        <div style="padding: 15px 10px 10px 10px;">
                            <dxe:ASPxButton ID="ASPxButton1" TabIndex="4" ClientInstanceName="btn_imagecompressed" runat="server" AutoPostBack="False" Text="S&#818;ave" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {SaveButtonClickNoofCopies('Insert');}" />
                            </dxe:ASPxButton>
                        </div>


                    </dxe:ContentControl>
                </ContentCollection>
            </dxe:TabPage>


            <dxe:TabPage Name="Image" Text="Influencer Limit">
                <ContentCollection>
                    <dxe:ContentControl runat="server">
                        <div class="col-md-3">

                            <label>Limit Amount </label>
                            <%--<asp:TextBox ID="TextBox1" runat="server" Width="100%" TextMode="Number"></asp:TextBox>--%>
                            <dxe:ASPxTextBox ID="txtInfLimt" ClientInstanceName="ctxtInfLimt" runat="server" Width="100%" DisplayFormatString="0.00">
                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                            </dxe:ASPxTextBox>

                        </div>
                        <%--Rev 1.0: "simple-select" class add --%>
                        <div class="col-md-3 simple-select">

                            <label>Restriction Type </label>
                            <%--<asp:TextBox ID="TextBox1" runat="server" Width="100%" TextMode="Number"></asp:TextBox>--%>
                            <asp:DropDownList ID="DropDownList1" runat="server" Width="100%">
                                <asp:ListItem Value="w" Text="Warn"></asp:ListItem>
                                <asp:ListItem Value="i" Text="Ignore"></asp:ListItem>
                                <asp:ListItem Value="b" Text="Block"></asp:ListItem>
                            </asp:DropDownList>

                        </div>


                        <div style="padding: 28px 10px 10px 10px;">
                            <dxe:ASPxButton ID="ASPxButton2" TabIndex="4" ClientInstanceName="btn_inf" runat="server" AutoPostBack="False" Text="S&#818;ave" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {SaveButtonClickInfluencer('Insert');}" />
                            </dxe:ASPxButton>
                        </div>


                    </dxe:ContentControl>
                </ContentCollection>
            </dxe:TabPage>

            <dxe:TabPage Name="BackdatedEntries" Text="Allow Backdated Entries">
                <ContentCollection>
                    <dxe:ContentControl runat="server">

                        <dxe:ASPxGridView ID="BackEntries" runat="server" KeyFieldName="moduleids" AutoGenerateColumns="False" SettingsBehavior-AllowSort="false"
                            Width="100%" ClientInstanceName="cBackEntries" OnRowInserting="Grid_RowInserting" OnRowUpdating="Grid_RowUpdating" OnRowDeleting="Grid_RowDeleting"
                            OnCustomCallback="BackEntries_CustomCallback" OnDataBinding="BackEntries_DataBinding" OnCellEditorInitialize="BackEntries_CellEditorInitialize" OnBatchUpdate="BackEntries_BatchUpdate">

                            <ClientSideEvents EndCallback="function(s,e) { ShowError(s.cpInsertError);}" BatchEditStartEditing="OnBatchStartEdit" Init="OnInit" />
                            <SettingsEditing Mode="Batch">
                                <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                            </SettingsEditing>
                            <Settings ShowGroupPanel="false" ShowStatusBar="Hidden" ShowTitlePanel="false" />
                            <SettingsDataSecurity AllowDelete="False" />
                            <Columns>

                                <dxe:GridViewDataTextColumn FieldName="moduleids" VisibleIndex="1" Visible="false" Caption="ModuleId">
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn FieldName="modulenames" Visible="true" VisibleIndex="2" Caption="Module">
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Days" FieldName="DaysId" VisibleIndex="3" Width="220px">
                                    <%--     <PropertiesComboBox ValueField="DaysId" TextField="Value" ClearButton-DisplayMode="Always">
                       <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                            <RequiredField ErrorText="Mandetary" />
                        </ValidationSettings>
                 
                   </PropertiesComboBox>--%>
                                    <PropertiesTextEdit DisplayFormatString="0" MaxLength="2">
                                        <ClientSideEvents LostFocus="DaysTextChange"></ClientSideEvents>
                                        <MaskSettings AllowMouseWheel="False" Mask="&lt;0..99&gt;"></MaskSettings>
                                        <ValidationSettings Display="None"></ValidationSettings>
                                        <Style HorizontalAlign="Right"></Style>
                                    </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                            </Columns>
                            <SettingsDataSecurity AllowEdit="true" />
                            <SettingsEditing Mode="Batch">
                                <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="Row" />
                            </SettingsEditing>
                            <ClientSideEvents BatchEditStartEditing="gridFocusedRowChanged" />
                        </dxe:ASPxGridView>
                        <div></div>
                        <div style="padding-top: 10px">

                            <dxe:ASPxButton ID="ASPxButton3" ClientInstanceName="cbtnSaveRecords" runat="server"
                                AccessKey="S" AutoPostBack="false" CssClass="btn btn-primary" TabIndex="0" Text="Save & Update"
                                UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {saveandupdate();}" />
                            </dxe:ASPxButton>

                        </div>



                    </dxe:ContentControl>
                </ContentCollection>
            </dxe:TabPage>

            <dxe:TabPage Name="Holiday" Text="Auto Invoice - Holiday">
                <ContentCollection>
                    <dxe:ContentControl runat="server">
                        <div class="col-md-3">
                            <label>Holiday </label>
                            <asp:DropDownList ID="ddlHoliday" runat="server" Width="100%">
                            </asp:DropDownList>
                        </div>
                        <div style="padding: 28px 10px 10px 10px;">
                            <dxe:ASPxButton ID="btn_holiday" TabIndex="4" ClientInstanceName="btn_holiday" runat="server" AutoPostBack="False" Text="S&#818;ave" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {SaveButtonClickHoliday('Insert');}" />
                            </dxe:ASPxButton>
                        </div>
                    </dxe:ContentControl>
                </ContentCollection>
            </dxe:TabPage>
        </TabPages>

        <%--  <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
	                                            var Tab0 = page.GetTab(0);
	                                            var Tab1 = page.GetTab(1);
	                                            var Tab2 = page.GetTab(2);
	                                            if(activeTab == Tab0)
	                                            {
	                                                disp_prompt('tab0');
	                                            }
	                                            if(activeTab == Tab1)
	                                            {
	                                                disp_prompt('tab1');
	                                            }
                                                if(activeTab == Tab2)
	                                            {
	                                                disp_prompt('tab2');
	                                            }
	                                            }"></ClientSideEvents>--%>
    </dxe:ASPxPageControl>
    </div>




    <asp:HiddenField ID="hdnSalesPromotionId" runat="server" />
    <asp:HiddenField ID="hdnDeliveryChargesId" runat="server" />
    <div class="modal fade" id="SalesPromotionModel" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Main Account Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="SalesPromotionNewkeydown(event)" id="txtSalesPromotionSearch" autofocus width="100%" placeholder="Search by Sales Promotion Name or Short Name" />

                    <div id="SalesPromotionTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th>Main Account Name</th>
                                <th>Short Name</th>
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
    <div class="modal fade" id="DeliveryChargesModel" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Main Account Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="DeliveryChargesNewkeydown(event)" id="txtDeliveryChargesSearch" autofocus width="100%" placeholder="Search by delivery charges or Short Name" />

                    <div id="DeliveryChargesTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th>Main Account Name</th>
                                <th>Short Name</th>
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
    <asp:HiddenField ID="hdnActiveEInvoice" runat="server" />

</asp:Content>
