<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="OrderDeliveryScheduleAdd.aspx.cs" Inherits="ERP.OMS.Management.Activities.OrderDeliveryScheduleAdd" %>

<%@ Register Src="~/OMS/Management/Activities/UserControls/UOMConversion.ascx" TagPrefix="uc1" TagName="UOMConversionControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchPopup.js?v=0.02"></script>
    <script src="JS/OrderDelivarySchdule.js"></script>
    <style type="text/css">
        .inline {
            display: inline !important;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content {
            overflow: visible !important;
        }

        .abs {
            position: absolute;
            right: -20px;
            top: 10px;
        }

        .fa.fa-exclamation-circle:before {
            font-family: FontAwesome;
        }

        .tp2 {
            right: -18px;
            top: 7px;
            position: absolute;
        }

        #txtCreditLimit_EC {
            position: absolute;
        }

        #gridReplacement_DXStatus span > a {
            display: none;
        }

        #aspxGridTax_DXEditingErrorRow0 {
            display: none;
        }

        .horizontal-images.content li {
            float: left;
        }

        #rdl_SaleInvoice {
            margin-top: 3px;
        }

            #rdl_SaleInvoice > tbody > tr > td {
                padding-right: 5px;
            }

        #DivBilling [class^="col-md"], #DivShipping [class^="col-md"] {
            padding-top: 5px;
            padding-bottom: 5px;
        }

        #tbldescripion > tbody > tr > td, #tbldescripion2 > tbody > tr > td {
            padding-right: 15px;
        }


        .dxeErrorFrameSys.dxeErrorCellSys {
            position: absolute;
        }

        .eqTble > tbody > tr > td {
            padding: 0 7px;
            vertical-align: top;
        }

        .mlableWh {
            padding-top: 22px;
            display: inline-block;
        }

            .mlableWh > input + span {
                white-space: nowrap;
            }
    </style>

    <script>
        function closeMultiUOM(s, e) {
            e.cancel = false;
            // cPopup_MultiUOM.Hide();
        }

        var Uomlength = 0;
        function UomLenthCalculation() {


            var SLNo = 1;

            $.ajax({
                type: "POST",
                url: "OrderDeliveryScheduleAdd.aspx/GetQuantityfromSL",
                data: JSON.stringify({ SLNo: SLNo }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {

                    Uomlength = msg.d;

                }
            });
        }
        function FinalMultiUOM() {
            UomLenthCalculation();
            if (Uomlength == 0 || Uomlength < 0) {
                jAlert("Please add atleast one Alt. Quantity with Update Row as checked.");
                return;
            }
            else {
                cPopup_MultiUOM.Hide();
                var SLNo = 1;
                cgrid_MultiUOM.PerformCallback('SetBaseQtyRateInGrid~' + SLNo);

            }
        }


        function PopulateMultiUomAltQuantity() {

            var otherdet = {};
            var Quantity = $("#UOMQuantity").val();
            otherdet.Quantity = Quantity;
            var UomId = ccmbUOM.GetValue();
            otherdet.UomId = UomId;
            var ProductID = $("#hdnProductId").val();
            otherdet.ProductID = ProductID;
            var AltUomId = ccmbSecondUOM.GetValue();
            otherdet.AltUomId = AltUomId;

            $.ajax({
                type: "POST",
                url: "OrderDeliveryScheduleAdd.aspx/GetPackingQuantity",
                data: JSON.stringify(otherdet),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    if (msg.d.length != 0) {
                        var packingQuantity = msg.d[0].packing_quantity;
                        var sProduct_quantity = msg.d[0].sProduct_quantity;
                    }
                    else {
                        var packingQuantity = 0;
                        var sProduct_quantity = 0;
                    }
                    var uomfactor = 0
                    if (sProduct_quantity != 0 && packingQuantity != 0) {
                        uomfactor = parseFloat(packingQuantity / sProduct_quantity).toFixed(4);
                        $('#hddnuomFactor').val(parseFloat(packingQuantity / sProduct_quantity));
                    }
                    else {
                        $('#hddnuomFactor').val(0);
                    }

                    var uomfac_Qty_to_stock = $('#hddnuomFactor').val();
                    var Qty = $("#UOMQuantity").val();
                    var calcQuantity = parseFloat(Qty * uomfac_Qty_to_stock).toFixed(4);

                    //$("#AltUOMQuantity").val(calcQuantity);
                    cAltUOMQuantity.SetValue(calcQuantity);

                }
            });
        }

        function OnAddClick() {
            window.location.href = 'OrderDeliveryScheduleList.aspx?key=' + $("#hdOrderId").val() + '&type=SO';
        }
        function DeliveryQuantityGotFocus() {

            // var adjustqty = parseFloat(ctxtEnterAdjustQty.GetValue());
            var ProductID = GetObjectID('hdnProductId').value.trim();
            var IsInventory = '';
            var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
            var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
            //  var AllowQty = $('#hdnAllowQty').val();
            var type = 'add';

            var actionQry = '';
            var AdjustmentId = '';
            if ($('#hdnDeliveryScheduleDetailsId').val() == '') {
                actionQry = 'DeliveryScheduleProduct';
            }
            else {
                actionQry = 'GetDeliveryScheduleProduct';
                AdjustmentId = $('#hdnDeliveryScheduleDetailsId').val();
            }
            var GetserviceURL = "../Activities/Services/Master.asmx/GetMultiUOMDetails";

            $.ajax({
                type: "POST",
                url: GetserviceURL,
                data: JSON.stringify({ orderid: ProductID, action: actionQry, module: 'DeliverySchedule', strKey: AdjustmentId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    var SpliteDetails = msg.d.split("||@||");
                    var IsInventory = '';
                    if (SpliteDetails[5] == "1") {
                        IsInventory = 'Yes';
                    }
                    var gridprodqty = '';
                    var gridPackingQty = '';
                    var slno = '1';
                    var strProductID = ProductID;
                    var isOverideConvertion = SpliteDetails[4];
                    //if (AllowQty == "1") {
                    //    isOverideConvertion = "0";
                    //}
                    //else {
                    //    isOverideConvertion = SpliteDetails[4];
                    //}

                    var isOverideConvertion = SpliteDetails[4];

                    var packing_saleUOM = SpliteDetails[2];
                    var sProduct_SaleUom = SpliteDetails[3];
                    var sProduct_quantity = SpliteDetails[0];
                    var packing_quantity = SpliteDetails[1];

                    //Rev 24428
                    hdnUOMId.value = sProduct_SaleUom;
                    //End rev 24428

                    if ($('#hdnDeliveryScheduleDetailsId').val() != '') {
                        gridPackingQty = SpliteDetails[6];

                        gridprodqty = SpliteDetails[7];
                    }
                    if ($('#hdnDeliveryScheduleDetailsId').val() == '') {
                        if (parseFloat(ctxtDeliveryQuantity.GetValue()) != 0) {
                            gridprodqty = parseFloat(ctxtDeliveryQuantity.GetValue());
                            if ($('#hdnUOMpacking').val() != null && $('#hdnUOMpacking').val() != '') {
                                gridPackingQty = $('#hdnUOMpacking').val();
                            }
                        }
                    }
                    if ($("#hddnMultiUOMSelection").val() == "0") {
                        if (ShowUOMConversionInEntry == "1" && SpliteDetails.length > 5) {



                            //if (AllowQty == "1") {
                            //    ShowUOMExtraValue(type, "Stock Adjustment", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty, AllowQty);
                            //}
                            //else {
                            //    ShowUOM(type, "Stock Adjustment", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);

                            //}
                            ShowUOM(type, "Delivery Schedule", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);

                        }
                    }

                }
            });

        }

        var aarrUOM = [];

        function SetDataToGrid(Quantity, packing, PackingUom, PackingSelectUom, productid, slno) {
            ctxtDeliveryQuantity.SetValue(Quantity);
            $('#hdnUOMQuantity').val(Quantity);
            $('#hdnUOMpacking').val(packing);
            $('#hdnUOMPackingUom').val(PackingSelectUom);
            $('#hdnUOMPackingSelectUom').val(PackingUom);
            dueLostFocus();
        }




        function dueLostFocus() {
            var hdnTotalDeliveryQty = $('#hdnTotalDeliveryQty').val();
            var TotalDelValue = parseFloat(ctxtDeliveryQuantity.GetValue()) + parseFloat(hdnTotalDeliveryQty);
            var hdnDeliveryQty = $('#hdnDeliveryQty').val();
            if ($("#hdnDeliveryScheduleDetailsId").val() != "") {
                var Total_DelValue = parseFloat(ctxtDeliveryQuantity.GetValue()) + parseFloat(hdnTotalDeliveryQty) - parseFloat(hdnDeliveryQty);
                if (parseFloat(ctxtProQty.GetText()) < parseFloat(Total_DelValue)) {
                    jAlert('Delivery Quantity can not be greater than Product Quantity', 'Alert', function () {
                        ctxtDeliveryQuantity.SetValue("0.00");
                    });

                }
            }
            else {
                jAlert('Delivery Quantity can not be greater than Product Quantity', 'Alert', function () {
                    ctxtDeliveryQuantity.SetValue("0.00");
                });
            }
        }









        function TypeCheck() {

            if ($("#ddltype").val() == "SB" || $("#ddltype").val() == "RET") {
                $("#tbldescripion").removeAttr('style');

                $("#tbldescripion2").attr('style', 'display:none');

            }

            else if ($("#ddltype").val() == "CDB" || $("#ddltype").val() == "CCR") {

                $("#tbldescripion").attr('style', 'display:none');

                $("#tbldescripion2").attr('style', 'display:none');
                $("#tblconsolidatevendor").removeAttr('style');

            }


            else {
                $("#tbldescripion").attr('style', 'display:none');
                $("#tbldescripion2").removeAttr('style');
            }


            if ($("#ddltype").val() == "SB" && $("#hdnProject").val() == "Yes") {
                $("#tblretention").removeAttr('style');
            }
            else {
                $("#tblretention").attr('style', 'display:none');
            }
        }

        function saveClientClick(s, e) {
            //debugger;
            if (ctxtWarranty.GetText() == 0) {
                flag = false;
                jAlert("Please enter Warranty Day(s).");
                ctxtWarranty.Focus();
                return false;
            }
            else if (tdt_DeliveryDate.GetText() == '') {
                jAlert('Delivery Date is mandatory', "Alert", function () {
                    tdt_DeliveryDate.Focus();
                });
            }
            else if (ctxtDeliveryQuantity.GetText() == 0.00) {
                flag = false;
                jAlert("Please Enter Delivery Quantity.");
                ctxtDeliveryQuantity.Focus();
                return false;
            }
                //Rev 24428

                //End Rev 24428


            else {

                var hdnTotalDeliveryQty = $('#hdnTotalDeliveryQty').val();
                var TotalDelValue = parseFloat(ctxtDeliveryQuantity.GetValue()) + parseFloat(hdnTotalDeliveryQty);
                //var hdnDeliveryQty = $('#hdnDeliveryQty').val();
                var Total_DelValue = "";
                if ($("#hdnDeliveryScheduleDetailsId").val() != "") {

                     Total_DelValue = parseFloat(ctxtDeliveryQuantity.GetValue());
                }
                else
                {
                    Total_DelValue = parseFloat(ctxtDeliveryQuantity.GetValue()) + parseFloat(hdnTotalDeliveryQty);
                }
               
                if (parseFloat(ctxtProQty.GetText()) < parseFloat(Total_DelValue)) {
                    jAlert('Delivery Quantity can not be greater than Product Quantity', 'Alert', function () {
                        ctxtDeliveryQuantity.SetValue("0.00");
                        return false;
                    });
                }
                else {
                    //dueLostFocus();
                    submitFunc();
                    return true;
                }








            }
        }
        function submitFunc() {

            if ($("#hdnDeliveryScheduleDetailsId").val() == "") {
                cOpeningGrid.PerformCallback('TemporaryData~' + 0);
            }
            else {
                var mod = $("#hdnDeliveryScheduleDetailsId").val();
                cOpeningGrid.PerformCallback('ModifyData~' + mod);
            }
        }

        function OnClickDelete(keyValue) {
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {


                    cOpeningGrid.PerformCallback('Delete~' + keyValue);
                    cOpeningGrid.PerformCallback('Display~' + 0);

                }
            });
        }

        function ClearData(s, e) {

            if (cOpeningGrid.cpSaveSuccessOrFail == "Success") {
                cOpeningGrid.cpSaveSuccessOrFail = null;
                ctxtWarranty.SetText('');
                tdt_DeliveryDate.SetText('');
                ctxtDeliveryQuantity.SetText('');
                //Rev 24428
                cAltertxtQty1.SetText('');
                ccmbPackingUom2.SetValue('');
                //Rev 24428
                $("#hdnTotalDeliveryQty").val(cOpeningGrid.cphdnTotalDeliveryQty);
                //End Rev 24428  z
                //End Rev 24428

                jAlert('Saved Successfully');

            }
            else if (cOpeningGrid.cpSaveSuccessOrFail == "Duplicate") {
                cOpeningGrid.cpSaveSuccessOrFail = null;
                ctxt_doccno.SetText('');
                ctxt_doccno2.SetText('');
                jAlert('Duplicate Document Number');
                TypeCheck();
            }
            else if (cOpeningGrid.cpSaveSuccessOrFail == "Mentatory") {
                cOpeningGrid.cpSaveSuccessOrFail = null;
                jAlert("Please Select Project.");
                TypeCheck();
            }
            else if (cOpeningGrid.cpSaveSuccessOrFail == "Delete") {
                cOpeningGrid.cpSaveSuccessOrFail = null;
                //Rev 24428
                $("#hdnTotalDeliveryQty").val(cOpeningGrid.cphdnTotalDeliveryQty);
                //End Rev 24428
                jAlert('Delete Successfully');

            }
        }


        function fn_AllowonlyNumeric(s, e) {
            var theEvent = e.htmlEvent || window.event;
            var key = theEvent.keyCode || theEvent.which;

            var txt = s.GetText();

            if (key != 8 || key != 13) txt += String.fromCharCode(key);

            var regex = /^[0-9]*\.?[0-9]*$/;
            if (!regex.test(txt)) {
                theEvent.returnValue = false;
                if (theEvent.preventDefault) theEvent.preventDefault();
            }

        }
        //Rev Mantis 24428 add two new parameter DeliverySchedule_AltQty, DeliverySchedule_AltUOM
        function onOpeningEdit(DeliveryScheduleDetails_Id, DeliverySchedule_DeliveryDate, DeliverySchedule_DeliveryQty, WarrantyDay, DeliverySchedule_AltQty, DeliverySchedule_AltUOM, SerialNumber) {
            //debugger;
            $("#hdnDeliveryScheduleDetailsId").val(DeliveryScheduleDetails_Id);
            ctxtWarranty.SetText(WarrantyDay);

            tdt_DeliveryDate.SetDate(new Date(DeliverySchedule_DeliveryDate));

            ctxtDeliveryQuantity.SetText(DeliverySchedule_DeliveryQty);
            //Rev Mantis 24428
            cAltertxtQty1.SetText(DeliverySchedule_AltQty);
            ccmbPackingUom2.SetText(DeliverySchedule_AltUOM);



            ctxtSerialNumber.SetText(SerialNumber);

            var otherdet = {};

            var Uom = DeliverySchedule_AltUOM;
            otherdet.Uom = Uom;

            $.ajax({
                type: "POST",
                url: "OrderDeliveryScheduleAdd.aspx/GetUOMID",
                data: JSON.stringify(otherdet),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    $('#hdnUOMpacking').val(DeliverySchedule_AltQty);
                    $('#hdnUOMPackingSelectUom').val(msg.d);

                }
            });




            //End Rev Mantis 24428
            cOpeningGrid.PerformCallback('MultiUOMData');


        }
        //End Rev MAntis 24428 






        function Callback_EndCallback() {
            // alert('');
            $("#ddldetails").val(0);
        }


        //Rev Bapi
        $(document).ready(function () {

            $("#UOMQuantity").on('blur', function () {
                var currentObj = $(this);
                var currentVal = currentObj.val();
                if (!isNaN(currentVal)) {
                    var updatedVal = parseFloat(currentVal).toFixed(4);
                    currentObj.val(updatedVal);
                }
                else {
                    currentObj.val("");
                }
            })


        })
        //End Rev Bapi



    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input type="hidden" value="0" id="hdnUOMQuantity" runat="server" />
    <input type="hidden" value="0" id="hdnUOMpacking" runat="server" />
    <input type="hidden" value="0" id="hdnUOMPackingUom" runat="server" />
    <input type="hidden" value="0" id="hdnUOMPackingSelectUom" runat="server" />

    <uc1:UOMConversionControl runat="server" ID="UOMConversionControl" />
    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">Delivery Schedule </h3>
        </div>
        <div id="ApprovalCross" runat="server" class="crossBtn"><a onclick="OnAddClick()" href="javascript:void(0);"><i class="fa fa-times"></i></a></div>
    </div>
    <div class="form_main">
        <div class="clearfix" style="background: #f7f5f5; padding: 10px; padding-bottom: 20px;">
            <div class="row">

                <div class="col-md-2 lblmTop8">
                    <label>Order Number </label>
                    <div>
                        <dxe:ASPxTextBox ID="txt_OrderNumber" runat="server" ClientInstanceName="ctxt_OrderNumber" Width="100%" ReadOnly="true">
                        </dxe:ASPxTextBox>
                    </div>
                </div>
                <div class="col-md-2 lblmTop8">
                    <label>Date</label>
                    <div>
                        <dxe:ASPxDateEdit ID="dt_date" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="tsdate" Width="100%" ReadOnly="true">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dxe:ASPxDateEdit>
                    </div>
                </div>
                <div class="col-md-3 lblmTop8">
                    <label>Product Code </label>
                    <div>
                        <dxe:ASPxTextBox ID="txt_ProductCode" runat="server" ClientInstanceName="ctxt_ProductCode" Width="100%" ReadOnly="true">
                        </dxe:ASPxTextBox>
                    </div>
                </div>
                <div class="col-md-3 lblmTop8">
                    <label>Product Name </label>
                    <div>
                        <dxe:ASPxTextBox ID="txt_ProductName" runat="server" ClientInstanceName="ctxt_ProductName" Width="100%" ReadOnly="true">
                        </dxe:ASPxTextBox>
                    </div>
                </div>
                <div class="col-md-2 lblmTop8">
                    <label>Product Quantity</label>
                    <div>
                        <dxe:ASPxTextBox ID="txtProQty" runat="server" ClientInstanceName="ctxtProQty" Width="100%" ReadOnly="true">
                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                        </dxe:ASPxTextBox>
                    </div>
                </div>
            </div>


            <div class="row">
                <div class="col-md-2 lblmTop8">
                    <label>Warranty Day(S) </label>
                    <div>
                        <dxe:ASPxTextBox ID="txtWarranty" runat="server" ClientInstanceName="ctxtWarranty" Width="100%">
                            <MaskSettings Mask="&lt;0..999999999&gt;" />
                        </dxe:ASPxTextBox>
                    </div>
                </div>
                <div class="col-md-2 lblmTop8">
                    <label>Schedule Date</label>
                    <div>
                        <dxe:ASPxDateEdit ID="dt_DeliveryDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="tdt_DeliveryDate" Width="100%">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dxe:ASPxDateEdit>
                    </div>
                </div>
                <div class="col-md-2 lblmTop8">
                    <label>Schedule Quantity </label>
                    <div>
                        <dxe:ASPxTextBox ID="txtDeliveryQuantity" runat="server" ClientInstanceName="ctxtDeliveryQuantity" Width="100%" ReadOnly="true">
                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" />
                            <%-- Rev 24428--%>
                            <%-- <ClientSideEvents LostFocus="dueLostFocus" GotFocus="DeliveryQuantityGotFocus" />--%>
                            <%--   End Rev 24428--%>
                            <ClientSideEvents GotFocus="DeliveryQuantityGotFocus" />
                        </dxe:ASPxTextBox>
                    </div>
                </div>
                <div class="col-md-1" id="multiuom" runat="server">
                    <div>
                        <label class="col-md-12" style="padding: 0px;"><b>Multi UOM</b></label>
                        <label class="clearfix"></label>
                        <a aria-hidden="true" onclick="MultiUom();">
                            <img src="/assests/images/MultiUomIcon.png" /></a>
                    </div>
                </div>

                <div class="col-md-2 lblmTop8" id="_div_AlterQuantity">
                    <label id="lblaltqty">Alt. Qty</label>
                    <div>
                        <dxe:ASPxTextBox ID="txtAlterQty1" runat="server" ClientSideEvents-GotFocus="QuantityGotFocus" ClientInstanceName="cAltertxtQty1" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px" ReadOnly="true">
                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" />
                            <%--ClientSideEvents-GotFocus="QuantityGotFocus"--%>
                            <ValidationSettings Display="None"></ValidationSettings>
                            <ClientSideEvents TextChanged="function(s,e) { ChangeQuantityByPacking1();}" />
                        </dxe:ASPxTextBox>

                        <span id="rfvAlterQuantity" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>

                    </div>
                </div>

                <div class="col-md-2 lblmTop8" id="_div_Uom">
                    <label id="lblaltuom">Alt. UOM</label>
                    <div>
                        <dxe:ASPxTextBox ID="cmbPackingUom2" runat="server" ClientInstanceName="ccmbPackingUom2" Width="100%" ReadOnly="true">
                        </dxe:ASPxTextBox>
                    </div>
                </div>

                <div class="clear"></div>
                <div class="col-md-2 lblmTop8">
                    <label>UOM </label>
                    <div>
                        <dxe:ASPxTextBox ID="txtUOM" runat="server" ClientInstanceName="ctxtUOM" Width="100%" ReadOnly="true">
                        </dxe:ASPxTextBox>
                    </div>
                </div>
                <div class="col-md-3 lblmTop8">
                    <label>Serial Number </label>
                    <div>
                        <dxe:ASPxTextBox ID="txtSerialNumber" runat="server" ClientInstanceName="ctxtSerialNumber" Width="100%" >
                        </dxe:ASPxTextBox>
                    </div>
                </div>
            </div>

        </div>

        <div style="padding-top: 15px;">
            <dxe:ASPxButton ID="FinalSave" runat="server" AutoPostBack="false" TabIndex="19" CssClass="btn btn-primary" UseSubmitBehavior="False"
                Text="<u>S</u>ave" ClientInstanceName="cFinalSave" VerticalAlign="Bottom" EncodeHtml="false">
                <ClientSideEvents Click="saveClientClick" />
            </dxe:ASPxButton>
        </div>
        <div class="GridViewArea" id="divgrid">

            <asp:DropDownList ID="ddldetails" runat="server" CssClass="btn btn-sm btn-primary" Style="float: right; margin-right: 2px !important;"
                OnSelectedIndexChanged="cmbExport2_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>

            <dxe:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" ClientInstanceName="cOpeningGrid"
                OnDataBinding="grid_DataBinding" KeyField="DeliveryScheduleDetails_Id"
                SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight="350"
                OnCustomCallback="grid_CustomCallback"
                Width="100%" Settings-HorizontalScrollBarMode="Visible">

                <Settings ShowGroupPanel="false" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu="True" />
                <Columns>

                    <dxe:GridViewDataTextColumn Caption="Order Number" FieldName="OrderNumber" ReadOnly="True" Visible="True" VisibleIndex="0" Width="200">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Order Date" FieldName="Order_Date" ReadOnly="True" Visible="True" VisibleIndex="1" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" Width="100">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Product Code" FieldName="sProducts_Code" ReadOnly="True" Visible="True" VisibleIndex="2" Width="200">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Product Name" FieldName="sProducts_Name" ReadOnly="True" Visible="True" VisibleIndex="3" Width="200">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Product Qty" FieldName="DeliverySchedule_Quantity" ReadOnly="True" Visible="True" VisibleIndex="4" Width="100">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Delivery Date" FieldName="DeliverySchedule_DeliveryDate" ReadOnly="True" Visible="True" VisibleIndex="5"
                        Width="100" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Delivery Qty" FieldName="DeliverySchedule_DeliveryQty" ReadOnly="True" Visible="True" VisibleIndex="6" Width="100">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="UOM" FieldName="UOM_Name" ReadOnly="True" Visible="False" VisibleIndex="7" Width="200">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Warranty Day(S)" FieldName="WarrantyDay" ReadOnly="True" Visible="True" VisibleIndex="8" Width="100">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Alternative Qty" FieldName="OrderDetails_PackingQty" ReadOnly="True" Visible="True" VisibleIndex="9" Width="100">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Alternative UOM" FieldName="PackingUOM" ReadOnly="True" Visible="True" VisibleIndex="10" Width="100">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                     <dxe:GridViewDataTextColumn Caption="Serial Number" FieldName="SerialNumber" ReadOnly="True" Visible="True" VisibleIndex="11" Width="100">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn ReadOnly="True" CellStyle-HorizontalAlign="Center" VisibleIndex="18" Width="200">
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Center"></CellStyle>
                        <HeaderTemplate>
                            Actions
                        </HeaderTemplate>
                        <DataItemTemplate>
                            <% if (rights.CanEdit)
                               { %>
                            <a href="javascript:void(0);" onclick="onOpeningEdit('<%#Eval("DeliveryScheduleDetails_Id")%>',
                                '<%#Eval("DeliverySchedule_DeliveryDate")%>','<%#Eval("DeliverySchedule_DeliveryQty")%>','<%#Eval("WarrantyDay")%>','<%#Eval("OrderDetails_PackingQty")%>','<%#Eval("PackingUOM")%>','<%#Eval("SerialNumber")%>')"
                                title="Edit" class="pad">
                                <img src="/assests/images/Edit.png" /></a>
                            <% } %>
                            <%-- <% if (rights.CanEdit)
                               { %>
                            <a href="javascript:void(0);" onclick="OnClickRetention('<%#Eval("ModId")%>','<%#Eval("DocNumber")%>'
                                ,'<%#Eval("DocAmount")%>','<%#Eval("Cus_ret_amount")%>','<%#Eval("Cus_ret_adjusted")%>'
                                ,'<%#Eval("Cus_unpaid_ret_amount")%>')"
                                title="Retention" class="pad">
                                <img src="/assests/images/changeIcon.png" /></a>

                            <% } %>--%>


                            <%--  <% } %>--%>
                            <% if (rights.CanDelete)
                               { %>
                            <a href="javascript:void(0);" onclick="OnClickDelete('<%#Eval("DeliveryScheduleDetails_Id")%>')" title="Edit" class="pad">
                                <img src="/assests/images/Delete.png" /></a>

                            <% } %>
                        </DataItemTemplate>
                    </dxe:GridViewDataTextColumn>

                </Columns>
                <SettingsPager PageSize="10">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                </SettingsPager>
                <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                <SettingsBehavior ColumnResizeMode="NextColumn" />
                <ClientSideEvents EndCallback="ClearData" />
            </dxe:ASPxGridView>
        </div>
        <div class="clear"></div>
        <div style="padding-top: 10px;">
        </div>


        <div class="clear"></div>
        <div class="">
        </div>
    </div>

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

    <asp:HiddenField runat="server" ID="hiddnmodid" />
    <asp:HiddenField runat="server" ID="hdnDeliveryScheduleDetailsId" />
    <asp:HiddenField runat="server" ID="hdnConvertionOverideVisible" />
    <asp:HiddenField runat="server" ID="hdnShowUOMConversionInEntry" />
    <asp:HiddenField ID="hdnProductId" runat="server" />
    <asp:HiddenField ID="hdnTotalDeliveryQty" runat="server" />
    <asp:HiddenField ID="hdOrderId" runat="server" />
    <asp:HiddenField ID="hdOrderdetailsID" runat="server" />
    <asp:HiddenField ID="hddnuomFactor" runat="server" />
    <asp:HiddenField ID="hddnMultiUOMSelection" runat="server" />
    <asp:HiddenField ID="hdnDeliveryQty" runat="server" />
    <asp:HiddenField ID="hdnUOMId" runat="server" />

    <%--Multi UOM POPUP--%>
    <dxe:ASPxPopupControl ID="Popup_MultiUOM" runat="server" ClientInstanceName="cPopup_MultiUOM"
        Width="900px" HeaderText="Multi UOM Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ClientSideEvents Closing="function(s, e) {
	    closeMultiUOM(s, e);}" />
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="Top clearfix">
                    <div class="clearfix col-md-12" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;">
                        <table class="eqTble">
                            <tr>
                                <td>
                                    <div>
                                        <div style="margin-bottom: 5px;">
                                            <div>
                                                <label>Base Quantity</label>
                                            </div>
                                            <div>
                                                <%--Rev Sanchita--%>
                                                <%--<input type="text" id="UOMQuantity" style="text-align: right;" maxlength="18"  class="allownumericwithdecimal" />--%>
                                                <input type="text" id="UOMQuantity" style="text-align: right;" maxlength="18" class="allownumericwithdecimal" onchange="CalcBaseRate()" />
                                                <%--End of Rev Sanchita--%>
                                            </div>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="Left_Content" style="margin-bottom: 5px;">
                                        <div>
                                            <label style="text-align: right;">Base UOM</label>
                                        </div>
                                        <div>
                                            <dxe:ASPxComboBox ID="cmbUOM" ClientInstanceName="ccmbUOM" runat="server" SelectedIndex="0" DataSourceID="UomSelect"
                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" ValueField="UOM_ID" TextField="UOM_Name">
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label>Base Rate </label>
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="cmbBaseRate" runat="server" Width="80px" ClientInstanceName="ccmbBaseRate" DisplayFormatString="0.000" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right" ReadOnly="true"></dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <span style="font-size: 22px; padding-top: 15px; display: inline-block;">=</span>
                                </td>
                                <td>
                                    <div>
                                        <div>
                                            <label style="text-align: right;">Alt. UOM</label>
                                        </div>
                                        <div class="Left_Content" style="">
                                            <dxe:ASPxComboBox ID="cmbSecondUOM" ClientInstanceName="ccmbSecondUOM" runat="server" SelectedIndex="0" DataSourceID="AltUomSelect"
                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" ValueField="UOM_ID" TextField="UOM_Name">
                                                <ClientSideEvents TextChanged="function(s,e) { PopulateMultiUomAltQuantity();}" />
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label style="white-space: nowrap">Alt. Quantity </label>
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox Width="80px" ID="AltUOMQuantity" runat="server" ClientInstanceName="cAltUOMQuantity" DisplayFormatString="0.0000" MaskSettings-Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right">
                                                <ClientSideEvents TextChanged="function(s,e) { CalcBaseQty();}" />
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label>Alt Rate </label>
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="cmbAltRate" Width="80px" runat="server" ClientInstanceName="ccmbAltRate" DisplayFormatString="0.000" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right">
                                                <ClientSideEvents TextChanged="function(s,e) { CalcBaseRate();}" />
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                        </div>
                                        <div>
                                            <%--  Mantis 24428--%>
                                            <%--  <label class="checkbox-inline mlableWh">
                                                <asp:CheckBox ID="chkUpdateRow" Checked="false" runat="server"></asp:CheckBox>
                                                <span style="margin: 0px 0; display: block">
                                                    <dxe:ASPxLabel ID="ASPxLabel18" runat="server" Text="Update Row">
                                                    </dxe:ASPxLabel>
                                                </span>
                                            </label>--%>
                                            <label class="checkbox-inline mlableWh">
                                                <input type="checkbox" id="chkUpdateRow" />
                                                <span style="margin: 0px 0; display: block">
                                                    <dxe:ASPxLabel ID="ASPxLabel18" runat="server" Text="Update Row">
                                                    </dxe:ASPxLabel>
                                                </span>
                                            </label>
                                            <%-- End Mantis 24428--%>
                                        </div>
                                    </div>


                                </td>
                                <%--End of Rev Sanchita--%>
                                <td style="padding-top: 14px;">
                                    <dxe:ASPxButton ID="btnMUltiUOM" ClientInstanceName="cbtnMUltiUOM" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                        <ClientSideEvents Click="function(s, e) { SaveMultiUOM();}" />
                                    </dxe:ASPxButton>
                                </td>
                            </tr>
                        </table>

                    </div>
                    <div class="clearfix">
                        <dxe:ASPxGridView ID="grid_MultiUOM" runat="server" KeyFieldName="AltUomId;AltQuantity" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="cgrid_MultiUOM" OnCustomCallback="MultiUOM_CustomCallback" OnDataBinding="MultiUOM_DataBinding"
                            SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200" SettingsBehavior-AllowSort="false">
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity"
                                    VisibleIndex="0" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="UOM" FieldName="UOM"
                                    VisibleIndex="1">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="BaseRate"
                                    VisibleIndex="2" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Alt. UOM" FieldName="AltUOM"
                                    VisibleIndex="3">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Alt. Quantity" FieldName="AltQuantity"
                                    VisibleIndex="4" HeaderStyle-HorizontalAlign="Right">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="" FieldName="UomId" Width="0px"
                                    VisibleIndex="5">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="" FieldName="AltUomId" Width="0px"
                                    VisibleIndex="6">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="AltRate"
                                    VisibleIndex="7" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Update Row" FieldName="UpdateRow"
                                    VisibleIndex="8" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="10" Width="80px" Caption="Action">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" onclick="Delete_MultiUom('<%# Container.KeyValue %>','<%#Eval("SrlNo") %>')" title="Delete">
                                            <img src="/assests/images/crs.png" /></a>

                                        <%--Mantis Issue 24428 --%>

                                        <a href="javascript:void(0);" onclick="Edit_MultiUom('<%# Container.KeyValue %>','<%#Eval("MultiUOMSR") %>')" title="Edit">
                                            <img src="/assests/images/Edit.png" /></a>
                                        <%--End of Mantis Issue 24428 --%>
                                    </DataItemTemplate>
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <ClientSideEvents EndCallback="OnMultiUOMEndCallback" />
                            <SettingsPager Visible="false"></SettingsPager>
                            <SettingsLoadingPanel Text="Please Wait..." />
                        </dxe:ASPxGridView>
                    </div>
                    <div class="clearfix">
                        <br />
                        <div style="align-content: center">
                            <dxe:ASPxButton ID="ASPxButton7" ClientInstanceName="cbtnfinalUomSave" Width="50px" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                <ClientSideEvents Click="function(s, e) {FinalMultiUOM();}" />
                            </dxe:ASPxButton>
                        </div>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>
    <%--Multi UOM POPUP End--%>


    <asp:SqlDataSource ID="UomSelect" runat="server"
        SelectCommand="select UOM_ID,UOM_Name from Master_UOM "></asp:SqlDataSource>

    <asp:SqlDataSource ID="AltUomSelect" runat="server"
        SelectCommand="select UOM_ID,UOM_Name from Master_UOM "></asp:SqlDataSource>
</asp:Content>
