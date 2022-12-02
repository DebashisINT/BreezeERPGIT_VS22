<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UOMConversion.ascx.cs" Inherits="ERP.OMS.Management.Activities.UserControls.UOMConversion" %>

<script>
    var aarr = [];
    var uomfactor = 0;
    var moduleNameSTk = "";

    function ShowUOM(type, module, docid, prodquantity, saleuom, packingqty, selectuom, productid, slno, isOverideConvertion, gridprodqty, gridPackingQty) {
       // debugger;

        moduleNameSTk = module;
        var htmlfactor = "";
        var iscalcQty = false;
        //UOM FACTOR
        var uomfactor = 0
        if (prodquantity != 0 && packingqty != 0) {
            uomfactor = parseFloat(packingqty / prodquantity).toFixed(4);
            $('#hdnuomFactor').val(parseFloat(packingqty / prodquantity));
        }
        else {
            $('#hdnuomFactor').val(0);
        }

        htmlfactor = parseFloat(prodquantity).toFixed(4) + " ";
        //

        //"Show Conversion In entry" option is activated in ERP Settings. Now create a product with same UOM and doesnot select any UOM Conversion value. 
        if (selectuom == null || selectuom=="") {
            return false;
        }
        else {
            var extobj = {};
            console.log(slno);
            var calcpackingqty = packingqty;
           
                for (var i = 0; i < aarr.length; i++) {
                    extobj = aarr[i];
                    if (extobj.slno == slno && extobj.productid == productid) {
                        prodquantity = extobj.Quantity;
                        calcpackingqty = extobj.packing;
                        //Rev Subhra 25-03-2019 because after click on ok button unit and 2 nd unit was changes
                        //saleuom = extobj.PackingUom;
                        //selectuom = extobj.PackingSelectUom;
                        saleuom = extobj.PackingSelectUom;
                        selectuom = extobj.PackingUom;
                        //End of Rev Subhra 25-03-2019
                        iscalcQty = true;
                    }
                    extobj = {};
                }
           


            $('#hdnsaleuom').val(saleuom);
            $('#hdnSLNO').val(slno);
            $('#hdnselectuom').val(selectuom);

            $('#hdnisOverideConvertion').val(isOverideConvertion);
            if (prodquantity == null || prodquantity == "") {
                prodquantity = parseFloat(0).toFixed(4);
            }
            if (packingqty == null || packingqty == "") {
                packingqty = parseFloat(0).toFixed(4);
            }

            $('#txtQuantity').val(prodquantity);
            $('#hdnprodquantity').val(prodquantity);
            $('#hdnpackingqty').val(packingqty);
            //$('#txtSaleUOM').val(saleuom);
   
            $('#txtPacking').val(calcpackingqty);
            

            //$('#txtSelectUOM').val(selectuom);
            $('#hdnProductID').val(productid);
            // $('#hdnSLNo').val(slno);
            $('#hdnDocID').val(docid);


            $('#UOMModal').modal('show');

            if (saleuom != '0' && saleuom != null) {
                ccmbPackingSelectUom.SetValue(saleuom);
            }
            if (selectuom != '0' && selectuom != null) {
                ccmbPackingUom.SetValue(selectuom);
            }
            else {
                if (saleuom != '0' && saleuom != null) {
                    ccmbPackingUom.SetValue(saleuom);
                }
            }

            ccmbPackingSelectUom.SetEnabled(false);
            ccmbPackingUom.SetEnabled(false);

            //if ((type == 'edit' && iscalcQty == false && parseFloat(gridprodqty) > 0 && parseFloat(gridPackingQty) > 0) || (parseFloat(gridprodqty) > 0 && parseFloat(gridPackingQty) > 0 && iscalcQty == false)) {
           

            htmlfactor = htmlfactor + " " + ccmbPackingSelectUom.GetText() + " = " + parseFloat(packingqty).toFixed(4) + " " + ccmbPackingUom.GetText();
             
            //$('#lbluomfactor').text(uomfactor);
            $('#lbluomfactor').text(htmlfactor);

            $('#lblUom').text(module);
            $('#lblQuantity').text(module);


            //According discussion when add product it's show 0 in both quantity

            if (gridPackingQty == "" || gridPackingQty == null) {
                gridPackingQty = "0.0000";
            }

            //According discussion when add product it's show 0 in both quantity

            if ((iscalcQty == false && parseFloat(gridprodqty) > -1 && parseFloat(gridPackingQty) > -1) || (parseFloat(gridprodqty) > -1 && parseFloat(gridPackingQty) > -1 && iscalcQty == false)) {
                $('#txtQuantity').val(gridprodqty);
                //ChangePackingByQuantity($('#txtQuantity'));
                //if (calcpackingqty != "" && calcpackingqty != null &&( gridPackingQty == "" || gridPackingQty==null )) {
                //    gridPackingQty = calcpackingqty;
                //}
                $('#txtPacking').val(gridPackingQty);

            }

            //Surojit 25-07-2019 0020188: Note
            if (module == "Opening Balances" && iscalcQty) {
                $('#txtQuantity').val("0.0000");
                $('#txtPacking').val("0.0000");
            }
            //Surojit 25-07-2019 0020188: Note


            ChkDataDigitCount($('#txtPacking'));
            ChkDataDigitCount($('#txtQuantity'));



            $('#txtQuantity').select();
        }
    }

    function ShowUOMExtraValue(type, module, docid, prodquantity, saleuom, packingqty, selectuom, productid, slno, isOverideConvertion, gridprodqty, gridPackingQty,Query) {
      
        moduleNameSTk = module;
        var htmlfactor = "";
        var iscalcQty = false;       
        var uomfactor = 0
        if (prodquantity != 0 && packingqty != 0) {
            uomfactor = parseFloat(packingqty / prodquantity).toFixed(4);
            $('#hdnuomFactor').val(parseFloat(packingqty / prodquantity));
        }
        else {
            $('#hdnuomFactor').val(0);
        }
        htmlfactor = parseFloat(prodquantity).toFixed(4) + " ";      
        if (selectuom == null || selectuom == "") {
            return false;
        }
        else {
            var extobj = {};
            console.log(slno);
            var calcpackingqty = packingqty;
            for (var i = 0; i < aarr.length; i++) {
                extobj = aarr[i];
                if (extobj.slno == slno && extobj.productid == productid) {
                    prodquantity = extobj.Quantity;
                    calcpackingqty = extobj.packing;                  
                    saleuom = extobj.PackingSelectUom;
                    selectuom = extobj.PackingUom;                  
                    iscalcQty = true;
                }
                extobj = {};
            }
            $('#hdnsaleuom').val(saleuom);
            $('#hdnSLNO').val(slno);
            $('#hdnselectuom').val(selectuom);
            $('#hdnisOverideConvertion').val(isOverideConvertion);
            if (prodquantity == null || prodquantity == "") {
                prodquantity = parseFloat(0).toFixed(4);
            }
            if (packingqty == null || packingqty == "") {
                packingqty = parseFloat(0).toFixed(4);
            }
            $('#txtQuantity').val(prodquantity);
            $('#hdnprodquantity').val(prodquantity);
            $('#hdnpackingqty').val(packingqty);        
            $('#hdnExtraQuery').val(Query);
            $('#txtPacking').val(calcpackingqty);          
            $('#hdnProductID').val(productid);          
            $('#hdnDocID').val(docid);
            $('#UOMModal').modal('show');
            if (saleuom != '0' && saleuom != null) {
                ccmbPackingSelectUom.SetValue(saleuom);
            }
            if (selectuom != '0' && selectuom != null) {
                ccmbPackingUom.SetValue(selectuom);
            }
            else {
                if (saleuom != '0' && saleuom != null) {
                    ccmbPackingUom.SetValue(saleuom);
                }
            }
            ccmbPackingSelectUom.SetEnabled(false);
            ccmbPackingUom.SetEnabled(false);     

            htmlfactor = htmlfactor + " " + ccmbPackingSelectUom.GetText() + " = " + parseFloat(packingqty).toFixed(4) + " " + ccmbPackingUom.GetText();         
            $('#lbluomfactor').text(htmlfactor);
            $('#lblUom').text(module);
            $('#lblQuantity').text(module);
            //According discussion when add product it's show 0 in both quantity
            if (gridPackingQty == "" || gridPackingQty == null) {
                gridPackingQty = "0.0000";
            }
            //According discussion when add product it's show 0 in both quantity
            if ((iscalcQty == false && parseFloat(gridprodqty) > -1 && parseFloat(gridPackingQty) > -1) || (parseFloat(gridprodqty) > -1 && parseFloat(gridPackingQty) > -1 && iscalcQty == false)) {
                $('#txtQuantity').val(gridprodqty);              
                $('#txtPacking').val(gridPackingQty);
            }
            //Surojit 25-07-2019 0020188: Note
            if (module == "Opening Balances" && iscalcQty) {
                $('#txtQuantity').val("0.0000");
                $('#txtPacking').val("0.0000");
            }
            //Surojit 25-07-2019 0020188: Note
            ChkDataDigitCount($('#txtPacking'));
            ChkDataDigitCount($('#txtQuantity'));
            $('#txtQuantity').select();
        }
    }


    //Rev Subhra 04-03-2019
    $(function () {
        $('#UOMModal').on('shown.bs.modal', function () {

            $('#txtQuantity').trigger('focus')
            //  setTimeout(function () {
            //        $('#txtQuantity').trigger('focus')
            //    }, 1600)

            //})

        });
    })
  
    //End of Rev
    function ChangePackingByQuantity(e) {
       
        var Quantity = $(e).val();
        var packing = $('#txtPacking').val();
        if (packing == null || packing == '') {
            $('#txtPacking').val(parseFloat(0).toFixed(4));
            packing = $('#txtPacking').val();
        }
        if (Quantity == null || Quantity == '') {
            $(e).val(parseFloat(0).toFixed(4));
            Quantity = $(e).val();
        }
        var packingqty = parseFloat($('#hdnpackingqty').val()).toFixed(4);
        var uomfac_Qty_to_stock = $('#hdnuomFactor').val();    
        var calcQuantity = parseFloat(Quantity * uomfac_Qty_to_stock).toFixed(4);   
        var AllowsetPacking = $('#hdnExtraQuery').val();
        if (AllowsetPacking == "1")
        {
            $('#txtPacking').val();
        }
        else
        {
            $('#txtPacking').val(calcQuantity);
        }
        

        ChkDataDigitCount(e);
    }

    function ChangeQuantityByPacking(e) {
       // debugger;
        var isOverideConvertion = $('#hdnisOverideConvertion').val();
        if (isOverideConvertion == '1') {
            var packing = $(e).val();
            var Quantity = $('#txtQuantity').val();
            if (packing == null || packing == '') {
                $(e).val(parseFloat(0).toFixed(4));
                packing = $(e).val();
            }

            if (Quantity == null || Quantity == '') {
                $('#txtQuantity').val(parseFloat(0).toFixed(4));

                Quantity = $('#txtQuantity').val();
            }
            var packingqty = parseFloat($('#hdnpackingqty').val()).toFixed(4);

           
            //Rev Subhra 06-03-2019
            // var calcQuantity = parseFloat(packing / packingqty).toFixed(4);
            var uomfac_stock_to_qty = $('#hdnuomFactor').val();
            //var uomfac_stock_to_qty = $('#hdnpackingqty').val();
            //Rev Surojit 21-05-2019
           // debugger;
            var calcQuantity = 0;
            if (parseFloat(uomfac_stock_to_qty) != 0) {
                calcQuantity = parseFloat(packing / uomfac_stock_to_qty).toFixed(4);
            }
            //End of Rev Surojit 21-05-2019
           // debugger;
            //End of Rev Subhra 06-03-2019
            $('#txtQuantity').val(calcQuantity);
        }
        ChkDataDigitCount(e);
    }

    function SaveSetQuantity() {
        // debugger;
        var OverideConvertion = $('#hdnisOverideConvertion').val();
        var productidget = $('#hdnProductID').val();
        var slnoget = $('#hdnSLNO').val();
        var Quantity = $('#txtQuantity').val();
        var packing = $('#txtPacking').val();
        if (Quantity == null || Quantity == '') {
            Quantity = '0.0000';

        }
        if (packing == null || packing == '') {
            packing = '0.0000';
        }
        if (moduleNameSTk != "Stock Adjustment") {
            if ((parseFloat(Quantity) != 0 && parseFloat(packing) != 0) || (parseFloat(Quantity) == 0 && parseFloat(packing) == 0)) {


                if (moduleNameSTk == "Stock Adjustment") {

                    if (parseFloat(Quantity) < 0) {
                        if (parseFloat(packing) >= 0) {
                            jAlert(moduleNameSTk + "Quantity & Stock quantity are must be negative!");
                        }
                        else {
                            var extobj = {};
                            var PackingUom = ccmbPackingUom.GetValue();
                            var PackingSelectUom = ccmbPackingSelectUom.GetValue();

                            for (i = 0; i < aarr.length; i++) {
                                extobj = aarr[i];
                                if (extobj.slno == slnoget && extobj.productid == productidget) {
                                    aarr.splice(i, 1);
                                }
                                extobj = {};
                            }
                            var arrobj = {};
                            arrobj.productid = productidget;
                            arrobj.slno = slnoget;
                            arrobj.Quantity = Quantity;
                            arrobj.packing = packing;
                            arrobj.PackingUom = PackingUom;
                            arrobj.PackingSelectUom = PackingSelectUom;
                            aarr.push(arrobj);
                            SetDataToGrid(Quantity, packing, PackingUom, PackingSelectUom, productidget, slnoget);
                            $('#UOMModal').modal('hide');
                        }

                    }
                    else if (parseFloat(Quantity) > 0) {
                        if (parseFloat(packing) < 0) {
                            if (parseFloat(Quantity) >= 0) {
                                jAlert(moduleNameSTk + "Quantity & Stock quantity are must be negative!");
                            }
                        }
                        else {

                            var extobj = {};
                            var PackingUom = ccmbPackingUom.GetValue();
                            var PackingSelectUom = ccmbPackingSelectUom.GetValue();

                            for (i = 0; i < aarr.length; i++) {
                                extobj = aarr[i];
                                if (extobj.slno == slnoget && extobj.productid == productidget) {
                                    aarr.splice(i, 1);
                                }
                                extobj = {};
                            }
                            var arrobj = {};
                            arrobj.productid = productidget;
                            arrobj.slno = slnoget;
                            arrobj.Quantity = Quantity;
                            arrobj.packing = packing;
                            arrobj.PackingUom = PackingUom;
                            arrobj.PackingSelectUom = PackingSelectUom;
                            aarr.push(arrobj);
                            SetDataToGrid(Quantity, packing, PackingUom, PackingSelectUom, productidget, slnoget);
                            $('#UOMModal').modal('hide');
                        }
                    }
                }
                else {
                    var extobj = {};
                    var PackingUom = ccmbPackingUom.GetValue();
                    var PackingSelectUom = ccmbPackingSelectUom.GetValue();

                    for (i = 0; i < aarr.length; i++) {
                        extobj = aarr[i];
                        console.log(extobj);
                        if (extobj.slno == slnoget && extobj.productid == productidget) {
                            //aarr.pop(extobj);
                            aarr.splice(i, 1);
                        }
                        extobj = {};
                    }
                    var arrobj = {};
                    arrobj.productid = productidget;
                    arrobj.slno = slnoget;
                    arrobj.Quantity = Quantity;
                    arrobj.packing = packing;
                    arrobj.PackingUom = PackingUom;
                    arrobj.PackingSelectUom = PackingSelectUom;
                    aarr.push(arrobj);
                    SetDataToGrid(Quantity, packing, PackingUom, PackingSelectUom, productidget, slnoget);
                    $('#UOMModal').modal('hide');
                }
            }
            else {
                jAlert(moduleNameSTk + "Quantity & Stock quantity cannot be zero!");
            }
        }
        else {
            if ((parseFloat(Quantity) != 0 && parseFloat(packing) != 0) || (parseFloat(Quantity) == 0 && parseFloat(packing) == 0) || (parseFloat(Quantity) != 0 && parseFloat(packing) == 0) || (parseFloat(Quantity) == 0 && parseFloat(packing) != 0)) {


                if (moduleNameSTk == "Stock Adjustment") {
                    var extobj = {};
                    var PackingUom = ccmbPackingUom.GetValue();
                    var PackingSelectUom = ccmbPackingSelectUom.GetValue();

                    for (i = 0; i < aarr.length; i++) {
                        extobj = aarr[i];
                        if (extobj.slno == slnoget && extobj.productid == productidget) {
                            aarr.splice(i, 1);
                        }
                        extobj = {};
                    }
                    var arrobj = {};
                    arrobj.productid = productidget;
                    arrobj.slno = slnoget;
                    arrobj.Quantity = Quantity;
                    arrobj.packing = packing;
                    arrobj.PackingUom = PackingUom;
                    arrobj.PackingSelectUom = PackingSelectUom;
                    aarr.push(arrobj);
                    SetDataToGrid(Quantity, packing, PackingUom, PackingSelectUom, productidget, slnoget);
                    $('#UOMModal').modal('hide');                          
                }
                else {
                    jAlert(moduleNameSTk + "Quantity & Stock quantity cannot be zero!");
                }
            }
        }
    }

    
    function ChkDataDigitCount(e) {
        var data = $(e).val();
        $(e).val(parseFloat(data).toFixed(4));
    }

    function SetFoucs() {
        ////debugger;
    }

    function validateFloatKeyPress(el, evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        var number = el.value.split('.');

        if ((moduleNameSTk == "Stock Adjustment" && charCode == 45)) {
            return true;
        }
        else {
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            //just one dot (thanks ddlab)
            if (number.length > 1 && charCode == 46) {
                return false;
            }
            //get the carat position
            //var caratPos = getSelectionStart(el);
            //var dotPos = el.value.indexOf(".");
            //if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
            //    return false;
            //}
            return true;
        }
    }
</script>
<style>
    .eqtTbl {
        width: 100%;
    }

        .eqtTbl > tbody > tr > td {
            padding: 0 15px;
            vertical-align: middle;
        }

    .eqT {
        font-size: 22px;
        padding-top: 15px;
        display: inline-block;
    }

    .white {
        background-color: #fff !important;
    }

    .mBot0 {
        margin-bottom: 0 !important;
    }
    .labler {
        padding: 10px;
        text-align: center;
        background: #e6a04c;
        margin-bottom: 23px;
        color: #fff;
        font-size: 14px;
    }
    .w50 {
        width:52% !important;
    }
</style>
<div class="container">
    <!-- Modal -->

    <input type="hidden" id="hdnProductID" />
    <input type="hidden" id="hdnSLNo" />
    <input type="hidden" id="hdnDocID" />

    <input type="hidden" id="hdnSLNO" />
    <input type="hidden" id="hdnprodquantity" />
    <input type="hidden" id="hdnsaleuom" />
    <input type="hidden" id="hdnpackingqty" />
    <input type="hidden" id="hdnselectuom" />
    <input type="hidden" id="hdnisOverideConvertion" />
     <input type="hidden" id="hdnExtraQuery" />
    <%--Rev -2019----Subhra----%>
    <input type="hidden" id="hdnuomFactor" />
    <%--End of Rev--%>

    <div class="modal fade" id="UOMModal" role="dialog">
        <div class="modal-dialog w50">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" onclick="SetFoucs();">&times;</button>
                    <h4 class="modal-title">UOM Conversion</h4>
                </div>
               
                <div class="modal-body white">
                    <div class="">
                        <table class="eqtTbl">
                            <tr>
                                <td colspan="5">
                                    <div class="labler">
                                         <span><b>UOM Conversion: </b></span>
                                         <span id="lbluomfactor"></span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label style="text-align:right;"> Quantity</label>
                                    <div>
                                        <input type="text" id="txtQuantity" style="text-align:right;"  maxlength="18" class="allownumericwithdecimal" onchange="ChangePackingByQuantity(this);" onkeypress="return validateFloatKeyPress(this,event);" /></div>
                                </td>
                                <td>
                                    <label style="margin-bottom: 2px;transform: translateY(-3px);"> UOM</label>
                                    <div>
                                        <dxe:ASPxComboBox ID="cmbPackingSelectUom" ClientInstanceName="ccmbPackingSelectUom" runat="server" SelectedIndex="0"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                        </dxe:ASPxComboBox>
                                    </div>
                                </td>
                                <td><span class="eqT">=</span></td>
                                <td>
                                    <label class="mMargin" style="text-align:right;">2nd Quantity</label>
                                    <div>
                                        <input type="text" id="txtPacking" maxlength="18" style="text-align:right;" class="allownumericwithdecimal" onchange="ChangeQuantityByPacking(this);" onkeypress="return validateFloatKeyPress(this,event);" /></div>
                                </td>
                                <td>
                                    <label style="margin-bottom: 2px;transform: translateY(-3px);">2nd UOM</label>
                                    <div>
                                        <dxe:ASPxComboBox ID="cmbPackingUom" ClientInstanceName="ccmbPackingUom" runat="server" SelectedIndex="0"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                        </dxe:ASPxComboBox>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5"></td>
                            </tr>
                        </table>
                    </div>
                    
                    <%--<div class="row">
                        <div class="col-lg-3">
                            
                        </div>

                        <div class="col-lg-3">
                           <input type="text" id="txtSaleUOM" />
                            
                        </div>
                        <div class="col-lg-3">
                            
                        </div>
                        <div class="col-lg-3">
                            <input type="text" id="txtSelectUOM" />
                             
                        </div>
                    </div>--%>
                </div>
                
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary mBot0" id="btnSetQuantity" onclick="SaveSetQuantity();">Confirm</button>
                    <button type="button" class="btn btn-danger" data-dismiss="modal" onclick="SetFoucs();">Cancel</button>
                </div>
            </div>

        </div>
    </div>

</div>

