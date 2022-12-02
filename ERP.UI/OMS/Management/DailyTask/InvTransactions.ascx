<%@ Control Language="C#" AutoEventWireup="True"
    Inherits="ERP.OMS.Management.DailyTask.Management_DailyTask_InvTransactions" Codebehind="InvTransactions.ascx.cs" %>
<%--<%@ Register Assembly="DevExpress.Web.v9.1, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v9.1.Export, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.Export" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.1" Namespace="DevExpress.Web"
    TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.v9.1, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>--%>

  <%--  <%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>--%>






<style type="text/css">
    .dxeMemoEditArea
    {
        height: 80px;
    }
    #Trans_Popup_PWST-1
    {
        width: 93% !important;
        left: 45px !important;
    }
    #ucTrans_Popup_Trans_Popup_PWST-1
    {
        width: 92% !important;
        left: 34px !important;
    }
</style>
<style type="text/css">
    #ucTrans_Popup_Conversion_Popup_PW-1
    {
        left: 35% !important;
        top: 19% !important;
        width: 26% !important;
    }
    #ucTrans_Popup_Conversion_Popup_CLW-1
    {
        width: 100% !important;
    }
    #ucTrans_Popup_AssignValuePopup_PW-1
    {
        position: absolute;
        left: 424px;
        top: 187px;
        z-index: 12004;
        overflow: visible;
        width: 560px;
        height: 60px;
    }
    #ucTrans_Popup_AssignValuePopup_CLW-1
    {
        width: 553px !important;
    }
</style>
<style>
    /* Big box with list of options */#ajax_listOfOptions
    {
        position: absolute; /* Never change this one */
        width: 50px; /* Width of box */
        height: auto; /* Height of box */
        overflow: auto; /* Scrolling features */
        border: 1px solid Blue; /* Blue border */
        background-color: #FFF; /* White background color */
        text-align: left;
        font-size: 0.9em;
        z-index: 32767;
    }
    #ajax_listOfOptions div
    {
        /* General rule for both .optionDiv and .optionDivSelected */
        margin: 1px;
        padding: 1px;
        cursor: pointer;
        font-size: 0.9em;
    }
    #ajax_listOfOptions .optionDiv
    {
        /* Div for each item in list */
    }
    #ajax_listOfOptions .optionDivSelected
    {
        /* Selected item in the list */
        background-color: #DDECFE;
        color: Blue;
    }
    #ajax_listOfOptions_iframe
    {
        background-color: #F00;
        position: absolute;
        z-index: 3000;
    }
    form
    {
        display: inline;
    }
</style>

<script type="text/javascript" src="../../CentralData/JSScript/GenericJScript.js"></script>

<%--<script type="text/javascript" src="/assests/js/jquery-1.3.2.min.js"></script>--%>
<%--<script type="text/javascript" src="/assests/js/ajaxList_inner.js"></script>--%>
<%--<script type="text/javascript" src="/assests/js/init.js"></script>--%>

<script type="text/javascript">
    function FunCallAjaxList_Customer(objID, objEvent, ObjType) {
        var strQuery_Table = '';
        var strQuery_FieldName = '';
        var strQuery_WhereClause = '';
        var strQuery_OrderBy = '';
        var strQuery_GroupBy = '';
        var CombinedQuery = '';
        if (ObjType == 'Digital') {
            var CustomerName = document.getElementById("ucTrans_Popup_Trans_Popup_txtCustomer").value;
            strQuery_Table = "tbl_master_contact";
            strQuery_FieldName = "cnt_firstname+' '+cnt_middlename+' '+cnt_lastname +' ('+cnt_internalId+')' as name , cnt_internalId";
            strQuery_WhereClause = "cnt_firstname like '" + CustomerName + "%'";
            CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars(CombinedQuery), "Main");
        }
    }

    function FunCallAjaxList_Product(objID, objEvent, ObjType) {
        var strQuery_Table = '';
        var strQuery_FieldName = '';
        var strQuery_WhereClause = '';
        var strQuery_OrderBy = '';
        var strQuery_GroupBy = '';
        var CombinedQuery = '';
        if (ObjType == 'Digital') {
            var ProductName = document.getElementById("ucTrans_Popup_Trans_Popup_txtProduct").value;
            strQuery_Table = "Master_sProducts";
            strQuery_FieldName = "sProducts_Name + ' ('+sProducts_Code+')' as Product_Name , sProducts_ID";
            strQuery_WhereClause = "sProducts_Name like '" + ProductName + "%'";
            CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars(CombinedQuery), "Main");
        }
    }
    function FunCallAjaxList_QuantityUnit(objID, objEvent, ObjType) {
        var strQuery_Table = '';
        var strQuery_FieldName = '';
        var strQuery_WhereClause = '';
        var strQuery_OrderBy = '';
        var strQuery_GroupBy = '';
        var CombinedQuery = '';
        if (ObjType == 'Digital') {
            var UOM_Name = document.getElementById("ucTrans_Popup_Trans_Popup_txtQuantityUnit").value;
            strQuery_Table = "Master_UOM";
            strQuery_FieldName = "UOM_Name, UOM_ID";
            strQuery_WhereClause = "UOM_Name like '" + UOM_Name + "%'";
            CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars(CombinedQuery), "Main");
        }
    }

    function FunCallAjaxList_Curency(objID, objEvent, ObjType) {
        var strQuery_Table = '';
        var strQuery_FieldName = '';
        var strQuery_WhereClause = '';
        var strQuery_OrderBy = '';
        var strQuery_GroupBy = '';
        var CombinedQuery = '';
        if (ObjType == 'Digital') {
            var CurrencyName = document.getElementById("ucTrans_Popup_Trans_Popup_txtCurrency").value;
            strQuery_Table = "Master_Currency";
            strQuery_FieldName = "Currency_AlphaCode + '('+ Currency_Name +')' AS Currency_Name, Currency_ID";
            strQuery_WhereClause = "Currency_Name like '" + CurrencyName + "%'";
            CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars(CombinedQuery), "Main");
        }
    }
    function FunCallAjaxList_PriceUnit(objID, objEvent, ObjType) {
        var strQuery_Table = '';
        var strQuery_FieldName = '';
        var strQuery_WhereClause = '';
        var strQuery_OrderBy = '';
        var strQuery_GroupBy = '';
        var CombinedQuery = '';
        if (ObjType == 'Digital') {
            var CurrencyName = document.getElementById("ucTrans_Popup_Trans_Popup_txtPriceUnit").value;
            strQuery_Table = "Master_Currency";
            strQuery_FieldName = "Currency_AlphaCode + '('+ Currency_Name +')' AS Currency_Name, Currency_ID";
            strQuery_WhereClause = "Currency_Name like '" + CurrencyName + "%'";
            CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars(CombinedQuery), "Main");
        }
    }

    function TransactionPopup() {
        var OrderpositopnId = "";
        OrderpositopnId = document.getElementById("hdnOrderPositon").value;
        var mode = document.getElementById("hdnTransactionEdit").value;
        $.ajax({
            type: "POST",
            url: "InvControlCentre.aspx/GetTransactionDetailsData",

            data: "{'id':'" + OrderpositopnId + "','mode':'" + mode + "','WareHouseId':'" + Get_CurrentWareHouseId() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function(msg) {
                var data = msg.d;
                // alert(data);
                var SetVal = data.split(',');
                Position_cmbType.SetValue(SetVal[0]);
                Position_cmbType.SetEnabled(false);

                var dt = new Date(SetVal[1]);
                Position_dtDate.SetDate(dt);


                Position_dtOrderDate.SetDate(dt);

                Position_txtOrderNo_hidden.SetValue(SetVal[2]);
                Position_txtOrderNo.SetValue(SetVal[31]);

                document.getElementById("ucTrans_Popup_Trans_Popup_txtPieceNo_I").value = SetVal[32];
                
                document.getElementById("ucTrans_Popup_Trans_Popup_txtCustomer").value = SetVal[3];
                document.getElementById("ucTrans_Popup_Trans_Popup_txtCustomer").readOnly = true;

                document.getElementById("ucTrans_Popup_Trans_Popup_txtCustomer_hidden").value = SetVal[4];

                document.getElementById("ucTrans_Popup_Trans_Popup_txtProduct").value = SetVal[5];
                document.getElementById("ucTrans_Popup_Trans_Popup_txtProduct").readOnly = true;

                document.getElementById("ucTrans_Popup_Trans_Popup_txtProduct_hidden").value = SetVal[6];
                Position_txtBrand.SetValue(SetVal[7]);

                Position_cmbSize.SetValue(SetVal[8]);
                Position_cmbSize.SetEnabled(false);

                Position_cmbColor.SetValue(SetVal[9]);
                Position_cmbColor.SetEnabled(false);

                Position_txtQuantity.SetValue(SetVal[10]);

                document.getElementById("ucTrans_Popup_Trans_Popup_txtQuantityUnit").value = SetVal[11];
                //document.getElementById("ucTrans_Popup_Conversion_Popup_lbUnit").innerHTML = SetVal[11];



                document.getElementById("ucTrans_Popup_Trans_Popup_txtQuantityUnit").readOnly = true;

                document.getElementById("ucTrans_Popup_Trans_Popup_txtQuantityUnit_hidden").value = SetVal[12];

                document.getElementById("ucTrans_Popup_Trans_Popup_txtCurrency").value = SetVal[13];
                document.getElementById("ucTrans_Popup_Trans_Popup_txtCurrency").readOnly = true;

                document.getElementById("ucTrans_Popup_Trans_Popup_txtCurrency_hidden").value = SetVal[14];

                Position_txtPrice.SetValue(SetVal[15]);

                Position_txtPerPrice.SetValue(SetVal[16]);

                document.getElementById("ucTrans_Popup_Trans_Popup_txtPriceUnit").value = SetVal[17];
                document.getElementById("ucTrans_Popup_Trans_Popup_txtPriceUnit").readOnly = true;

                document.getElementById("ucTrans_Popup_Trans_Popup_txtPriceUnit_hidden").value = SetVal[18];

                Position_memoDescription.SetText(SetVal[19]);

                if (SetVal[20] == "A") {
                    document.getElementById("dvOtherLocation").style.display = "none";
                    document.getElementById("dvCustomerLocation").style.display = "block";

                    if (SetVal[0] == "P" || SetVal[0] == "J") {
                        document.getElementById("WarehouseFrom").style.display = "none";
                        document.getElementById("WarehouseTo").style.display = "block";
                        document.getElementById("CustomerTo").style.display = "none";
                        document.getElementById("CustomerFrom").style.display = "block";
                        ClDeliveryFrom.SetValue(SetVal[24])
                    }
                    else if (SetVal[0] == "R") {
                        document.getElementById("dvCustomerLocation").style.display = "none";
                        document.getElementById("dvWareHouse").style.display = "block";
                        document.getElementById("WarehouseFrom").style.display = "block";
                        document.getElementById("WarehouseTo").style.display = "block";

                    }
                    else {
                        document.getElementById("WarehouseFrom").style.display = "block";
                        document.getElementById("WarehouseTo").style.display = "none";
                        document.getElementById("CustomerFrom").style.display = "none";
                        document.getElementById("CustomerTo").style.display = "block";
                        ClDeliveryTo.SetValue(SetVal[24])
                    }
                }
                if (SetVal[20] == "B") {
                    document.getElementById("dvCustomerLocation").style.display = "none";
                    document.getElementById("dvOtherLocation").style.display = "block";

                    if (SetVal[0] == "P" || SetVal[0] == "J") {
                        document.getElementById("WarehouseFrom").style.display = "none";
                        document.getElementById("WarehouseTo").style.display = "block";
                        document.getElementById("OtherLocationTo").style.display = "none";
                        document.getElementById("OtherLocationFrom").style.display = "block";
                        document.getElementById("ucTrans_Popup_Trans_Popup_txtOlDeliveryFrom").value = SetVal[21];
                        document.getElementById("ucTrans_Popup_Trans_Popup_hidBranch").value = SetVal[22];
                        document.getElementById("ucTrans_Popup_Trans_Popup_txtOlDeliveryFrom").readOnly = true;
                    }
                    else if (SetVal[0] == "R") {
                        document.getElementById("dvOtherLocation").style.display = "none";
                        document.getElementById("dvWareHouse").style.display = "block";
                        document.getElementById("WarehouseFrom").style.display = "block";
                        document.getElementById("WarehouseTo").style.display = "block";

                    }
                    else {
                        document.getElementById("WarehouseFrom").style.display = "block";
                        document.getElementById("WarehouseTo").style.display = "none";
                        document.getElementById("OtherLocationFrom").style.display = "none";
                        document.getElementById("OtherLocationTo").style.display = "block";
                        document.getElementById("ucTrans_Popup_Trans_Popup_txtOlDeliveryTo").value = SetVal[21];
                        document.getElementById("ucTrans_Popup_Trans_Popup_hidBranch").value = SetVal[22];
                        document.getElementById("ucTrans_Popup_Trans_Popup_txtOlDeliveryTo").readOnly = true;
                    }
                }
                if (SetVal[20] == "O") {
                    document.getElementById("dvCustomerLocation").style.display = "none";
                    document.getElementById("dvOtherLocation").style.display = "block";

                    if (SetVal[0] == "P" || SetVal[0] == "J") {
                        document.getElementById("WarehouseFrom").style.display = "none";
                        document.getElementById("WarehouseTo").style.display = "block";
                        document.getElementById("OtherLocationTo").style.display = "none";
                        document.getElementById("OtherLocationFrom").style.display = "block";
                        document.getElementById("ucTrans_Popup_Trans_Popup_txtOlDeliveryFrom").value = SetVal[25];
                        document.getElementById("ucTrans_Popup_Trans_Popup_txtOlDeliveryFrom").readOnly = true;

                    }
                    else if (SetVal[0] == "R") {
                        document.getElementById("dvOtherLocation").style.display = "none";
                        document.getElementById("dvWareHouse").style.display = "block";
                        document.getElementById("WarehouseFrom").style.display = "block";
                        document.getElementById("WarehouseTo").style.display = "block";

                    }
                    else {
                        document.getElementById("WarehouseFrom").style.display = "block";
                        document.getElementById("WarehouseTo").style.display = "none";
                        document.getElementById("OtherLocationFrom").style.display = "none";
                        document.getElementById("OtherLocationTo").style.display = "block";
                        document.getElementById("ucTrans_Popup_Trans_Popup_txtOlDeliveryTo").value = SetVal[25];
                        document.getElementById("ucTrans_Popup_Trans_Popup_txtOlDeliveryTo").readOnly = true;
                    }
                }
                if (SetVal[20] == "W") {
                    document.getElementById("dvCustomerLocation").style.display = "none";
                    document.getElementById("dvOtherLocation").style.display = "none";
                    document.getElementById("dvWareHouse").style.display = "block";

                    if (SetVal[0] == "P" || SetVal[0] == "J") {
                        document.getElementById("WarehouseFrom").style.display = "none";
                        document.getElementById("WarehouseTo").style.display = "block";
                        //document.getElementById("WarehouseFrom").style.display = "block";
                        WHDeliveryTo.SetValue(SetVal[23])
                    }
                    else if (SetVal[0] == "R") {
                        document.getElementById("dvWareHouse").style.display = "block";
                        document.getElementById("WarehouseFrom").style.display = "block";
                        document.getElementById("WarehouseTo").style.display = "block";

                    }
                    else {
                        document.getElementById("WarehouseFrom").style.display = "block";
                        document.getElementById("WarehouseTo").style.display = "none";
                        document.getElementById("WarehouseTo").style.display = "block";
                        WHDeliveryTo.SetValue(SetVal[23])
                    }
                }
                document.getElementById("ucTrans_Popup_Trans_Popup_hdnDeliveryAt").value = SetVal[20];
                BestBeforeMonth.SetValue(SetVal[26]);
                BestBeforeMonth.SetEnabled(false);
                //alert(SetVal[20]);
                BestBeforeYear.SetValue(SetVal[27]);
                BestBeforeYear.SetEnabled(false);
                //set batch no
                if (SetVal[29] != "" || SetVal[29] != null) {
                    document.getElementById("ucTrans_Popup_Trans_Popup_txtBatchNo_I").value = SetVal[29];
                    //ctxtBatchNo.SetEnabled(false);
                    //document.getElementById("ucTrans_Popup_Trans_Popup_txtBatchNo_I").readOnly = true;
                }
                else {
                    alert(SetVal[29]);
                    //ctxtBatchNo.SetEnabled(true);
                }

                //Conversion section show hide 
                if (SetVal[0] == "P" || SetVal[0] == "J") {
                    document.getElementById("dvConversionSelection").style.display = "none";
                    document.getElementById("dvConversionLinkButton").style.display = "none";
                }
                else {
                    document.getElementById("dvConversionSelection").style.display = "block";
                    document.getElementById("dvConversionLinkButton").style.display = "block";
                }

                var dt = new Date(SetVal[30]);
                Position_dtRecvDate.SetDate(dt);

                WarehouseStockCheck();

            }
        });
    }

    function CheckSubmitValidation(s, e) {
        var flag = true;
        var BatchNo = ctxtBatchNo.GetValue();
        var Quantity = Position_txtQuantity.GetValue();
        if (BatchNo == null || BatchNo == "") {
            alert("Please enter value for Batch no.");
            document.getElementById("ucTrans_Popup_Trans_Popup_txtBatchNo_I").focus();
            flag = false;
            return flag;
        }
        if (Quantity == null || Quantity == "") {
            alert("Please enter value for Quantity");
            document.getElementById("ucTrans_Popup_Trans_Popup_txtQuantity_I").focus();
            flag = false;
            return flag;
        }
        return flag;
    }
    function WarehouseStockCheck() {
        var WarehouseId = WHDeliveryFrom.GetValue();
        var ProductId = document.getElementById("ucTrans_Popup_Trans_Popup_txtProduct_hidden").value;
        var ColorId = Position_cmbColor.GetValue();
        var SizeId = Position_cmbSize.GetValue();
        var QuantityUnitId = document.getElementById("ucTrans_Popup_Trans_Popup_txtQuantityUnit_hidden").value;
        $.ajax({
            type: "POST",
            url: "InvControlCentre.aspx/GetWarehouseStock",
            data: "{'WarehouseId':'" + WarehouseId + "','ProductId':'" + ProductId + "','ColorId':'" + ColorId + "','SizeId':'" + SizeId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function(msg) {
                $("#ucTrans_Popup_Trans_Popup_DrpLocStock").empty();
                $.each(msg.d, function(key, value) {
                    $("#ucTrans_Popup_Trans_Popup_DrpLocStock").append($("<option></option>").val(value.Stock_Id).html(value.Text));
                });
                ConversionShowHide();
            }
        });


    }

    function ConversionShowHide() {
        var QuantityUnit = document.getElementById("ucTrans_Popup_Trans_Popup_txtQuantityUnit").value;
        var SelectStockValue = $("#ucTrans_Popup_Trans_Popup_DrpLocStock").text();
        var retVal = SelectStockValue.split(':');
        if (QuantityUnit.toLowerCase() == retVal[1].toLowerCase()) {
            document.getElementById("dvlnkConvertion").style.display = "none";

        }
        else {
            document.getElementById("dvlnkConvertion").style.display = "block";
            document.getElementById("ucTrans_Popup_Trans_Popup_btnSave").disabled = true;
            //document.getElementById("ucTrans_Popup_Trans_Popup_btnSave").disabled = true;
        }
    }
</script>

<script type="text/javascript">

    $(document).ready(function() {
        ConvertionClick();
    });

    function ConvertionClick() {
        $("#lnkConvertion").click(function() {
            document.getElementById("ucTrans_Popup_Conversion_Popup_txtQuantity_toconvert").value = "";
            Conversion_Popup.Show();
        });

    }
    function ConvertDataset() {
        var WarehouseFromname = document.getElementById("ucTrans_Popup_Trans_Popup_cmbWHDeliveryFrom_I").value;
        var StockId = $('#ucTrans_Popup_Trans_Popup_DrpLocStock').find('option:selected').val();
        var OrderQuantityId = document.getElementById("ucTrans_Popup_Trans_Popup_txtQuantityUnit_hidden").value;
        var OrderQuantityName = document.getElementById("ucTrans_Popup_Trans_Popup_txtQuantityUnit").value;
        WHDeliveryFrom_Convert.SetValue(WarehouseFromname);
        WHDeliveryFrom_Convert.SetEnabled(false);
        $.ajax({
            type: "POST",
            url: "InvControlCentre.aspx/SetConvertForm",
            data: "{'StockId':'" + StockId + "','OrderQuantityId':'" + OrderQuantityId + "','OrderQuantityName':'" + OrderQuantityName + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function(msg) {
                var Message = msg.d;
                var strArry = Message.split(',');
                document.getElementById("ucTrans_Popup_Conversion_Popup_txtWarehouseQuantity").value = strArry[0];
                document.getElementById("ucTrans_Popup_Conversion_Popup_txtWarehouseQuantityUnit").innerHTML = strArry[2];
                document.getElementById("ucTrans_Popup_Conversion_Popup_hdnWarehouseQuantityUnit").value = strArry[1];
                document.getElementById("ucTrans_Popup_Conversion_Popup_lblQuantity_toconvertUnit").innerHTML = strArry[4];
                document.getElementById("ucTrans_Popup_Conversion_Popup_hdnQuantity_toconvertUnit").value = strArry[3];
            }
        });
    }

    function Get_CurrentWareHouseId() {
        var WarehouseFromId = WHDeliveryFrom.GetValue();
        return WarehouseFromId;
    }
    function ConvertOperation() {
        var WarehouseQuantity = document.getElementById("ucTrans_Popup_Conversion_Popup_txtQuantity_toconvert").value;
        var WarehouseQuantityUnit = document.getElementById("ucTrans_Popup_Conversion_Popup_hdnWarehouseQuantityUnit").value;
        var ToConvertQuantityUnit = document.getElementById("ucTrans_Popup_Conversion_Popup_hdnQuantity_toconvertUnit").value;
        $.ajax({
            type: "POST",
            url: "InvControlCentre.aspx/ConvertOperation",
            data: "{'WarehouseQuantity':'" + WarehouseQuantity + "','WarehouseQuantityUnit':'" + WarehouseQuantityUnit + "','ToConvertQuantityUnit':'" + ToConvertQuantityUnit + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function(msg) {
                var Message = msg.d;
                if (Message != "") {
                    document.getElementById("ucTrans_Popup_Conversion_Popup_txtWarehouseQuantity").value = Message;
                }
                else {
                    alert("Conversion can not possible!");
                }
            }
        });
    }
    function TransactionButtonCheck() {
        var SelectStockValue = $("#ucTrans_Popup_Trans_Popup_DrpLocStock").text();
        var retVal = SelectStockValue.split(':');
        var ConversionResult = document.getElementById("ucTrans_Popup_Conversion_Popup_txtWarehouseQuantity").value;
        alert(ConversionResult + "    " + retVal[0])
        if (parseFloat(ConversionResult) > parseFloat(retVal[0])) {
            alert("Converted value should be less than or equal of warehouse value!")
            return false;
        }
        else {
            return true;
        }

    }
</script>

<%--new script added on 3 december 2015 by monojit--%>

<script type="text/javascript">

    var counter = 0;


    function fetchLebel() {

        $("#generatedForm").html("");
        counter = 0;


        $(".newLbl").each(function() {

            var newField = "<div style='width:500px; margin-left:5px; float:left; margin-bottom:5px;'><label id='LblKey" + counter + "' style='width:110px; float:left;'>" + $(this).text() + "</label>";
            newField += "<input type='text' id='TxtKey" + counter + "' value='" + $(this).text() + "' style='margin-left:41px; width:250px;' />";

            //alert($(this).attr("id").split('_')[4]);

            if (String($(this).attr("id").split('_')[4]) != "undefined") {
                newField += "<input type='text' id='HddnKey" + counter + "' value='" + $(this).attr("id").split('_')[4] + "' style='display:none; margin-left:41px; width:250px;' />";
            }
            else {
                //alert($(this).attr("id"));
                newField += "<input type='text' id='HddnKey" + counter + "' value='" + $(this).attr("id") + "' style='display:none; margin-left:41px; width:250px;' />";
            }
            newField += "</div>";

            $("#generatedForm").append(newField);

            counter++;

        });

        AssignValuePopup.Show();

    }




    function SaveDataToResource() {

        var key = "";
        var value = "";

        for (var i = 0; i < counter; i++) {

            if (key == "") {

                key = $("#HddnKey" + i).val();
                value = $("#TxtKey" + i).val();

            }
            else {

                key += "," + $("#HddnKey" + i).val();
                value += "," + $("#TxtKey" + i).val();

            }

        }

        $("#ucTrans_Popup_AssignValuePopup_KeyField").val(key);
        $("#ucTrans_Popup_AssignValuePopup_ValueField").val(value);
        $("#ucTrans_Popup_AssignValuePopup_RexPageName").val("InventoryControlCentre");



        //           $.ajax({ url: "SetResData.aspx?key=" + key + "&value=" + value + "&RexPageName=InventoryControlCentre",
        //                   success: function(result) {
        //                   AssignValuePopup.Hide();
        //                   
        //                   window.location="";
        //                   
        //                   }
        //               });


        return true;

    }
        
    
</script>

  <script type="text/javascript">
      function InventoryPieceNoCheck1() {          
          var pcno = document.getElementById("ucTrans_Popup_Trans_Popup_txtPieceNo_I").value;
          
          if (pcno != "") {

              $.ajax({
                  type: "POST",
                  url: "InvControlCentre.aspx/CheckUniqueInventiryPieceNo",
                  data: "{'pcno':'" + pcno + "'}",
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  success: function(msg) {
                      var data = msg.d;
                      if (data == 1) {
                          alert("Please enter unique piece no");
                          return false;
                      }
                  }

              });
          }
         else {
             alert('Piece No can not be blank');
         }
      }
    </script>


<%--new script added on 3 december 2015 by monojit--%>
<dxe:ASPxPopupControl ID="Trans_Popup" runat="server" ClientInstanceName="Trans_Popup"
    Width="407px" HeaderText="Add / Edit Transaction" PopupHorizontalAlign="WindowCenter"
    BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
    Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
    ContentStyle-CssClass="pad">
    <ClientSideEvents PopUp="function(s,e){TransactionPopup();}" />
    <ContentStyle VerticalAlign="Top" CssClass="pad">
    </ContentStyle>
    <ContentCollection>
        <dxe:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
            <div style="background-color: #d3ddeb; overflow: hidden; width: 1223px; height: 760px;
                overflow-y: auto;">
                <div style="background-color: #4d74a8">
                    <h3 style="color: #fff; margin: 0; font-size: 12px; text-transform: uppercase; font-family: Arial, Helvetica, sans-serif;
                        padding: 10px 0; text-align: center">
                        ENTER TRANSACTION
                    </h3>
                </div>
                <div class="field" style="border: 2px solid #fff; margin: 5px; overflow: hidden">
                    <div style="float: left; width: 55%; border-right: 2px solid #fff">
                        <div style="padding: 10px 20px">
                            <div style="margin: 10px 0; overflow: hidden">
                                <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                                    font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                    <%-- <label>
                                        Type
                                    </label>--%>
                                    <asp:Label ID="LblType" runat="server" Text="Type" CssClass="newLbl"></asp:Label>
                                </div>
                                <div style="display: inline-block; vertical-align: middle; width: 64%; background-color: #d3ddeb;
                                    border: 1px solid #fff; padding: 3px 5px">
                                    <dxe:ASPxComboBox ClientInstanceName="Position_cmbType" ID="cmbType" runat="server"
                                        Style="width: 100%">
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                            <div style="margin: 10px 0; overflow: hidden">
                                <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                                    font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                    <%--<label>
                                        Date
                                    </label>--%>
                                    <asp:Label ID="LblDate" runat="server" Text="Date" CssClass="newLbl"></asp:Label>
                                </div>
                                <div style="display: inline-block; vertical-align: middle; width: 25%; background-color: #d3ddeb;
                                    border: 1px solid #fff; padding: 3px 5px">
                                    <dxe:ASPxDateEdit EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="Position_dtDate"
                                        ReadOnly="true" ID="dtDate" runat="server" Style="width: 100%">
                                    </dxe:ASPxDateEdit>
                                </div>
                            </div>
                            <div style="margin: 10px 0; overflow: hidden">
                                <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                                    font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                    <%--<label>
                                        Order Number
                                    </label>--%>
                                    <asp:Label ID="LblOrderNumber" runat="server" Text="Order Number" CssClass="newLbl"></asp:Label>
                                </div>
                                <div style="display: inline-block; vertical-align: middle; width: 65%; background-color: #d3ddeb;
                                    border: 1px solid #fff; padding: 3px 5px">
                                    <div style="display:none;">
                                        <dxe:ASPxTextBox ClientInstanceName="Position_txtOrderNo_hidden" ID="txtOrderNo"
                                            runat="server" ReadOnly="true" Style="width: 100%;">
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <dxe:ASPxTextBox ClientInstanceName="Position_txtOrderNo" ID="txtOrderNo1" runat="server"
                                        ReadOnly="true" Style="width: 100%;">
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                            <div style="margin: 10px 0; overflow: hidden">
                                <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                                    font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                    <%--<label>
                                        Order Date
                                    </label>--%>
                                    <asp:Label ID="LblOrderDate" runat="server" Text="Order Date" CssClass="newLbl"></asp:Label>
                                </div>
                                <div style="display: inline-block; vertical-align: middle; width: 25%; background-color: #d3ddeb;
                                    border: 1px solid #fff; padding: 3px 5px">
                                    <dxe:ASPxDateEdit ClientInstanceName="Position_dtOrderDate" EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                        ReadOnly="true" ID="dtOrderDate" runat="server" Style="width: 100%">
                                    </dxe:ASPxDateEdit>
                                </div>
                            </div>
                            <div style="margin: 10px 0; overflow: hidden">
                                <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                                    font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                    <%--<label>
                                        Customer/Vendor
                                    </label>--%>
                                    <asp:Label ID="LblCustomer" runat="server" Text="Customer/Vendor" CssClass="newLbl"></asp:Label>
                                </div>
                                <div style="display: inline-block; vertical-align: middle; width: 65%; background-color: #d3ddeb;
                                    border: 1px solid #fff; padding: 3px 5px">
                                    <asp:TextBox ID="txtCustomer" runat="server" autocomplete="off" onkeyup="FunCallAjaxList_Customer(this,event,'Digital');"
                                        Style="width: 100%"></asp:TextBox>
                                    <asp:TextBox ID="txtCustomer_hidden" runat="server" Style="display: none;"></asp:TextBox>
                                </div>
                            </div>
                            <div style="margin: 10px 0; overflow: hidden">
                                <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                                    font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                    <%--<label>
                                        Customer/Vendor
                                    </label>--%>
                                    <asp:Label ID="LblRecvDate" runat="server" Text="Received Date" CssClass="newLbl"></asp:Label>
                                </div>
                                <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                                    font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                    <dxe:ASPxDateEdit EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="Position_dtRecvDate"
                                        ID="txtRecvDate" runat="server" Style="width: 100%">
                                    </dxe:ASPxDateEdit>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div style="float: right; width: 40%; overflow: hidden">
                        <div style="padding: 10px 20px">
                            <div style="margin: 10px 0; overflow: hidden">
                                <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                                    font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                    <%--<label>
                                        Delivery Reference
                                    </label>--%>
                                    <asp:Label ID="LblDeliveryReference" runat="server" Text="Delivery Reference" CssClass="newLbl"></asp:Label>
                                </div>
                                <div style="display: inline-block; vertical-align: middle; width: 65%; background-color: #d3ddeb;
                                    border: 1px solid #fff; padding: 3px 5px" rows="3">
                                    <dxe:ASPxMemo ID="memoDeliveryRef" runat="server" Style="width: 100%; height: 80px;">
                                    </dxe:ASPxMemo>
                                </div>
                            </div>
                            <%--<div style="margin: 10px 0; overflow: hidden">
                                <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                                    font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                    
                                    <asp:Label ID="LblWeight" runat="server" Text="Weight" CssClass="newLbl"></asp:Label>
                                    
                                </div>
                                <div style="display: inline-block; vertical-align: middle; width: 65%; background-color: #d3ddeb;
                                    border: 1px solid #fff; padding: 3px 5px" rows="3">
                                    <dxe:ASPxMemo ID="memoRemarks" runat="server" Style="width: 100%">
                                    </dxe:ASPxMemo>
                                </div>
                            </div>--%>
                        </div>
                    </div>
                </div>
                <div style="clear: both">
                </div>
                <div style="border: 2px solid #fff; margin: 5px; overflow: hidden; float: left; padding: 5px;
                    width: 65%">
                    <div style="background-color: #4d74a8">
                        <h3 style="color: #fff; margin: 0; font-size: 12px; text-transform: uppercase; font-family: Arial, Helvetica, sans-serif;
                            padding: 10px 0; text-align: center">
                            Product Detail
                        </h3>
                    </div>
                    <div style="float: left; width: 55%;">
                        <div style="padding: 10px 20px">
                            <div style="margin: 10px 0; overflow: hidden">
                                <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                                    font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                    <%--  <label>
                                        Product
                                    </label>--%>
                                    <asp:Label ID="LblProduct" runat="server" Text="Product" CssClass="newLbl"></asp:Label>
                                </div>
                                <div style="display: inline-block; vertical-align: middle; width: 65%; background-color: #d3ddeb;
                                    border: 1px solid #fff; padding: 3px 5px">
                                    <asp:TextBox ID="txtProduct" runat="server" autocomplete="off" onkeyup="FunCallAjaxList_Product(this,event,'Digital');"
                                        Style="width: 100%"></asp:TextBox>
                                    <asp:TextBox ID="txtProduct_hidden" runat="server" Style="display: none;"></asp:TextBox>
                                </div>
                            </div>
                            <div style="margin: 10px 0; overflow: hidden">
                                <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                                    font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                    <%--<label>
                                        Brand
                                    </label>--%>
                                    <asp:Label ID="LblBrand" runat="server" Text="Brand" CssClass="newLbl"></asp:Label>
                                </div>
                                <div style="display: inline-block; vertical-align: middle; width: 65%; background-color: #d3ddeb;
                                    border: 1px solid #fff; padding: 3px 5px">
                                    <dxe:ASPxTextBox ClientInstanceName="Position_txtBrand" ID="txtBrand" runat="server"
                                        ReadOnly="true" Style="width: 100%">
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                            <div style="margin: 10px 0; overflow: hidden">
                                <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                                    font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                    <%--<label>
                                        Size/Strength
                                    </label>--%>
                                    <asp:Label ID="LblSize" runat="server" Text="Size" CssClass="newLbl"></asp:Label>
                                </div>
                                <div style="display: inline-block; vertical-align: middle; width: 65%; background-color: #d3ddeb;
                                    border: 1px solid #fff; padding: 3px 5px">
                                    <dxe:ASPxComboBox ClientInstanceName="Position_cmbSize" ID="cmbSize" runat="server"
                                        Style="width: 100%">
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                            <div style="margin: 10px 0; overflow: hidden">
                                <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                                    font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                    <%--<label>
                                        Color
                                    </label>--%>
                                    <asp:Label ID="LblColor" runat="server" Text="Color" CssClass="newLbl"></asp:Label>
                                </div>
                                <div style="display: inline-block; vertical-align: middle; width: 65%; background-color: #d3ddeb;
                                    border: 1px solid #fff; padding: 3px 5px">
                                    <dxe:ASPxComboBox ClientInstanceName="Position_cmbColor" ID="cmbColor" runat="server"
                                        Style="width: 100%">
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                            <div style="margin: 10px 0; overflow: hidden">
                                <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                                    font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                    <%--<label>
                                        Quantity
                                    </label>--%>
                                    <asp:Label ID="LblQuantity" runat="server" Text="Quantity" CssClass="newLbl"></asp:Label>
                                </div>
                                <div style="display: inline-block; vertical-align: middle; width: 20%; background-color: #d3ddeb;
                                    border: 1px solid #fff; padding: 3px 5px; margin-right: 5px">
                                    <dxe:ASPxTextBox ClientInstanceName="Position_txtQuantity" ID="txtQuantity" runat="server"
                                        Style="width: 100%">
                                    </dxe:ASPxTextBox>
                                </div>
                                <div style="display: inline-block; vertical-align: middle; width: 15%; background-color: #d3ddeb;
                                    border: 1px solid #fff; padding: 3px 5px">
                                    <asp:TextBox ID="txtQuantityUnit" runat="server" autocomplete="off" onkeyup="FunCallAjaxList_QuantityUnit(this,event,'Digital');"
                                        Style="width: 100%"></asp:TextBox>
                                    <asp:TextBox ID="txtQuantityUnit_hidden" runat="server" Style="display: none"></asp:TextBox>
                                </div>
                            </div>
                            <div style="margin: 10px 0; overflow: hidden">
                                <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                                    font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                    <%--<label>
                                        Currency
                                    </label>--%>
                                    <asp:Label ID="LblCurrency" runat="server" Text="Currency" CssClass="newLbl"></asp:Label>
                                </div>
                                <div style="display: inline-block; vertical-align: middle; width: 25%; background-color: #d3ddeb;
                                    border: 1px solid #fff; padding: 3px 5px">
                                    <asp:TextBox ID="txtCurrency" runat="server" autocomplete="off" onkeyup="FunCallAjaxList_Curency(this,event,'Digital');"
                                        Style="width: 100%"></asp:TextBox>
                                    <asp:TextBox ID="txtCurrency_hidden" runat="server" Style="display: none;"></asp:TextBox>
                                </div>
                            </div>
                            <%--new position for weight start--%>
                            <div style="margin: 10px 0; overflow: hidden">
                                <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                                    font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                    <asp:Label ID="LblWeight" runat="server" Text="Weight" CssClass="newLbl"></asp:Label>
                                </div>
                                <div style="display: inline-block; vertical-align: middle; width: 65%; background-color: #d3ddeb;
                                    border: 1px solid #fff; padding: 3px 5px" rows="3">
                                    <%--<dxe:ASPxMemo ID="memoRemarks" runat="server" Style="width: 100%">
                                    </dxe:ASPxMemo>--%>
                                    <asp:TextBox ID="memoRemarks" runat="server" autocomplete="off" Style="width: 100%"></asp:TextBox>
                                </div>
                            </div>
                            <%--new position for weight end--%>
                            <div style="margin: 10px 0; overflow: hidden">
                                <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                                    font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                    <%--<label>
                                        Price
                                    </label>--%>
                                    <asp:Label ID="LblPrice" runat="server" Text="Price" CssClass="newLbl"></asp:Label>
                                </div>
                                <div style="display: inline-block; width: 20%; background-color: #d3ddeb; border: 1px solid #fff;
                                    padding: 3px 5px; margin-right: 5px">
                                    <dxe:ASPxTextBox ClientInstanceName="Position_txtPrice" ID="txtPrice" runat="server"
                                        ReadOnly="true">
                                    </dxe:ASPxTextBox>
                                </div>
                                <div style="display: inline-block; width: 15%; background-color: #d3ddeb; border: 1px solid #fff;
                                    padding: 3px 5px; margin-right: 5px">
                                    <dxe:ASPxTextBox ClientInstanceName="Position_txtPerPrice" ID="txtPerPrice" runat="server"
                                        ReadOnly="true">
                                    </dxe:ASPxTextBox>
                                </div>
                                <div style="display: inline-block; width: 15%; background-color: #d3ddeb; border: 1px solid #fff;
                                    padding: 3px 5px">
                                    <asp:TextBox ID="txtPriceUnit" runat="server" autocomplete="off" onkeyup="FunCallAjaxList_PriceUnit(this,event,'Digital');"
                                        Style="width: 100%"></asp:TextBox>
                                    <asp:TextBox ID="txtPriceUnit_hidden" runat="server" Style="display: none;"></asp:TextBox>
                                </div>
                            </div>
                            <div style="margin: 10px 0; overflow: hidden">
                                <div style="display: inline-block; vertical-align: middle; width: 30%; font-size: 12px;
                                    font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                    <%--<label>
                                        Batch Number
                                    </label>--%>
                                    <asp:Label ID="LblBatchNumber" runat="server" Text="Batch Number" CssClass="newLbl"></asp:Label>
                                </div>
                                <div style="display: inline-block; vertical-align: middle; width: 65%; background-color: #d3ddeb;
                                    border: 1px solid #fff; padding: 3px 5px">
                                    <dxe:ASPxTextBox ID="txtBatchNo" runat="server" ClientInstanceName="ctxtBatchNo"
                                        Style="width: 100%">
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div style="float: right; width: 45%; overflow: hidden">
                        <div style="padding: 10px 20px">
                            <div style="margin: 10px 0; overflow: hidden">
                                <div style="display: inline-block; vertical-align: middle; width: 25%; font-size: 12px;
                                    font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                    <%--<label>
                                        Description
                                    </label>--%>
                                    <asp:Label ID="LblDescription" runat="server" Text="Description" CssClass="newLbl"></asp:Label>
                                </div>
                                <div style="display: inline-block; vertical-align: middle; width: 68%; background-color: #d3ddeb;
                                    border: 1px solid #fff; padding: 3px 5px" rows="16">
                                    <dxe:ASPxMemo ClientInstanceName="Position_memoDescription" ID="memoDescription"
                                        ReadOnly="true" runat="server" Style="width: 100%">
                                    </dxe:ASPxMemo>
                                </div>
                            </div>
                        </div>
                        <div style="padding: 10px 20px" id="dvConversionSelection">
                            <div style="margin: 10px 0; overflow: hidden">
                                <div style="display: inline-block; vertical-align: middle; width: 25%; font-size: 12px;
                                    font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                    <%--<label>
                                        Select Stock
                                    </label>--%>
                                    <asp:Label ID="LblSelectStock" runat="server" Text="Select Stock" CssClass="newLbl"></asp:Label>
                                </div>
                                <div style="display: inline-block; vertical-align: middle; width: 68%; background-color: #d3ddeb;
                                    border: 1px solid #fff; padding: 3px 5px" rows="16">
                                    <asp:DropDownList runat="server" ID="DrpLocStock" onchange="ConversionShowHide();">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        
                           <div style="padding: 10px 20px" id="dvPcNo">
                            <div style="margin: 10px 0; overflow: hidden">
                                <div style="display: inline-block; vertical-align: middle; width: 25%; font-size: 12px;
                                    font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                    
                                    <asp:Label ID="Label1" runat="server" Text="Piece No" CssClass="newLbl"></asp:Label>
                                </div>
                                <div style="display: inline-block; vertical-align: middle; width: 68%; background-color: #d3ddeb;
                                    border: 1px solid #fff; padding: 3px 5px" rows="16">
                                    
                                    <dxe:ASPxTextBox ID="txtPieceNo" Width="180px" runat="server">
                            <ClientSideEvents TextChanged="function(s,e){InventoryPieceNoCheck1()}" />
                        </dxe:ASPxTextBox>
                        
                                </div>
                            </div>
                        </div>
                        
                        
                        
                        <div id="dvConversionLinkButton">
                            <div style="padding: 10px 20px">
                                <div style="margin: 10px 0; overflow: hidden">
                                    <div id="dvlnkConvertion" style="display: inline-block; vertical-align: middle; width: 25%;
                                        font-size: 12px; font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                        <a href="#" id="lnkConvertion">Unit Convertion</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div style="overflow: hidden; clear: both">
                        <div style="padding: 0px 20px">
                            <div style="margin: 10px 0; overflow: hidden; float: left; width: 30%">
                                <div style="float: left; width: 46%; font-size: 12px; font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                    <%--<label>
                                        Manufacture Date
                                    </label>--%>
                                    <asp:Label ID="LblManufactureDate" runat="server" Text="Manufacture Date" CssClass="newLbl"></asp:Label>
                                </div>
                                <div style="float: left; width: 30%; background-color: #d3ddeb; border: 1px solid #fff;
                                    padding: 3px 5px; margin-left: 20px">
                                    <dxe:ASPxDateEdit ID="dtManufactureDate" runat="server" Style="width: 100%">
                                    </dxe:ASPxDateEdit>
                                </div>
                            </div>
                            <div style="margin: 10px 0; overflow: hidden; float: left; width: 25%">
                                <div style="float: left; width: 35%; font-size: 12px; font-weight: bold; font-family: Arial, Helvetica, sans-serif;
                                    margin-left: 20px">
                                    <%--<label>
                                        <%--Expiry Date--%>
                                    <%--Best Befor Month
                                    </label>--%>
                                    <asp:Label ID="LblBestBeforMonth" runat="server" Text="Best Befor Month" CssClass="newLbl"></asp:Label>
                                </div>
                                <div style="float: left; width: 30%; background-color: #d3ddeb; border: 1px solid #fff;
                                    padding: 3px 5px; margin-left: 20px">
                                    <%--<dxe:ASPxDateEdit EditFormat="Custom" EditFormatString="dd-MM-yyyy" ID="dtExpiryDate"
                                        runat="server" Style="width: 100%">
                                    </dxe:ASPxDateEdit>--%>
                                    <dxe:ASPxComboBox ClientInstanceName="BestBeforeMonth" ID="cmbBestBeforeMonth" runat="server"
                                        Width="100%">
                                        <Items>
                                            <dxe:ListEditItem Text="" Value="0" />
                                            <dxe:ListEditItem Text="Jan" Value="1" />
                                            <dxe:ListEditItem Text="Feb" Value="2" />
                                            <dxe:ListEditItem Text="MAR" Value="3" />
                                            <dxe:ListEditItem Text="APR" Value="4" />
                                            <dxe:ListEditItem Text="MAY" Value="5" />
                                            <dxe:ListEditItem Text="JUN" Value="6" />
                                            <dxe:ListEditItem Text="JUL" Value="7" />
                                            <dxe:ListEditItem Text="AUG" Value="8" />
                                            <dxe:ListEditItem Text="SEP" Value="9" />
                                            <dxe:ListEditItem Text="OCT" Value="10" />
                                            <dxe:ListEditItem Text="NOV" Value="11" />
                                            <dxe:ListEditItem Text="DEC" Value="12" />
                                        </Items>
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                            <div style="margin: 10px 0; overflow: hidden; float: left; width: 35%">
                                <div style="float: left; width: 50%; font-size: 12px; font-weight: bold; font-family: Arial, Helvetica, sans-serif;
                                    margin-left: 20px">
                                    <%--<label>--%>
                                    <%--Best Before [Use]--%>
                                    <%--Bset Before Year
                                    </label>--%>
                                    <asp:Label ID="LblBsetBeforeYear" runat="server" Text="Bset Before Year" CssClass="newLbl"></asp:Label>
                                </div>
                                <div style="float: left; width: 30%; background-color: #d3ddeb; border: 1px solid #fff;
                                    padding: 3px 5px; margin-left: 20px">
                                    <%--<dxe:ASPxDateEdit ID="dtBestBefore" runat="server" Style="width: 100%">
                                    </dxe:ASPxDateEdit>--%>
                                    <dxe:ASPxComboBox ClientInstanceName="BestBeforeYear" ID="cmbBestBeforeYear" runat="server"
                                        Width="100%">
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div style="border: 2px solid #fff; margin: 5px; overflow: hidden; float: right;
                    padding: 5px; width: 30%">
                    <div style="background-color: #4d74a8">
                        <h3 style="color: #fff; margin: 0; font-size: 12px; text-transform: uppercase; font-family: Arial, Helvetica, sans-serif;
                            padding: 10px 0; text-align: center">
                            delivery location
                        </h3>
                    </div>
                    <div id="dvWareHouse" style="padding: 3px; border: 1px solid #fff; margin: 5px 0">
                        <h3 style="font-size: 12px; font-family: Arial, Helvetica, sans-serif; text-align: center">
                            Warehouse
                        </h3>
                        <div id="WarehouseFrom" style="margin: 10px 0; overflow: hidden; padding: 0 3px">
                            <div style="display: inline-block; vertical-align: top; width: 30%; font-size: 12px;
                                font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                <%--<label>
                                    Delivered from
                                </label>--%>
                                <asp:Label ID="LblDeliveredfrom" runat="server" Text="Delivered from" CssClass="newLbl"></asp:Label>
                            </div>
                            <div style="display: inline-block; width: 64%; background-color: #d3ddeb; border: 1px solid #fff;
                                padding: 3px 5px">
                                <dxe:ASPxComboBox ClientInstanceName="WHDeliveryFrom" ID="cmbWHDeliveryFrom" runat="server"
                                    Style="width: 100%">
                                    <ClientSideEvents SelectedIndexChanged="function(s,e){WarehouseStockCheck();}" />
                                </dxe:ASPxComboBox>
                            </div>
                        </div>
                        <div id="WarehouseTo" style="margin: 10px 0; overflow: hidden; padding: 0 3px">
                            <div style="display: inline-block; vertical-align: top; width: 30%; font-size: 12px;
                                font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                <%--<label>
                                    Delivered To
                                </label>--%>
                                <asp:Label ID="LblDeliveredTo" runat="server" Text="Delivered To" CssClass="newLbl"></asp:Label>
                            </div>
                            <div style="display: inline-block; width: 64%; background-color: #d3ddeb; border: 1px solid #fff;
                                padding: 3px 5px">
                                <dxe:ASPxComboBox ClientInstanceName="WHDeliveryTo" ID="cmbWHDeliveryTo" runat="server"
                                    Style="width: 100%">
                                </dxe:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                    <div id="dvCustomerLocation" style="padding: 3px; border: 1px solid #fff; margin: 5px 0">
                        <h3 style="font-size: 12px; font-family: Arial, Helvetica, sans-serif; text-align: center">
                            Customer/Vendor's Location
                        </h3>
                        <div id="CustomerFrom" style="margin: 10px 0; overflow: hidden; padding: 0 3px">
                            <div style="display: inline-block; vertical-align: top; width: 30%; font-size: 12px;
                                font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                <label>
                                    Delivered from
                                </label>
                            </div>
                            <div style="display: inline-block; width: 64%; background-color: #d3ddeb; border: 1px solid #fff;
                                padding: 3px 5px">
                                <dxe:ASPxComboBox ClientInstanceName="ClDeliveryFrom" ID="cmbClDeliveryFrom" runat="server"
                                    Style="width: 100%" ReadOnly="true">
                                </dxe:ASPxComboBox>
                            </div>
                        </div>
                        <div id="CustomerTo" style="margin: 10px 0; overflow: hidden; padding: 0 3px">
                            <div style="display: inline-block; vertical-align: top; width: 30%; font-size: 12px;
                                font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                <label>
                                    Delivered To
                                </label>
                            </div>
                            <div style="display: inline-block; width: 64%; background-color: #d3ddeb; border: 1px solid #fff;
                                padding: 3px 5px">
                                <dxe:ASPxComboBox ClientInstanceName="ClDeliveryTo" ID="cmbClDeliveryTo" runat="server"
                                    Style="width: 100%" ReadOnly="true">
                                </dxe:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                    <div id="dvOtherLocation" style="padding: 3px; border: 1px solid #fff; margin: 5px 0">
                        <h3 style="font-size: 12px; font-family: Arial, Helvetica, sans-serif; text-align: center">
                            Other Location
                        </h3>
                        <div id="OtherLocationFrom" style="margin: 10px 0; overflow: hidden; padding: 0 3px">
                            <div style="display: inline-block; vertical-align: top; width: 30%; font-size: 12px;
                                font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                <label>
                                    Delivered from
                                </label>
                            </div>
                            <div style="display: inline-block; width: 64%; background-color: #d3ddeb; border: 1px solid #fff;
                                padding: 3px 5px">
                                <asp:TextBox ID="txtOlDeliveryFrom" runat="server" Style="width: 100%">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div id="OtherLocationTo" style="margin: 10px 0; overflow: hidden; padding: 0 3px">
                            <div style="display: inline-block; vertical-align: top; width: 30%; font-size: 12px;
                                font-weight: bold; font-family: Arial, Helvetica, sans-serif;">
                                <label>
                                    Delivered To
                                </label>
                            </div>
                            <div style="display: inline-block; width: 64%; background-color: #d3ddeb; border: 1px solid #fff;
                                padding: 3px 5px">
                                <asp:TextBox ID="txtOlDeliveryTo" runat="server" Style="width: 100%">
                                </asp:TextBox>
                            </div>
                        </div>
                        <asp:HiddenField ID="hidBranch" runat="server" />
                    </div>
                </div>
                <div style="clear: both">
                </div>
                <div class="fieldBtns" style="background-color: #4d74a8; margin: 0 auto; text-align: center;
                    padding: 10px 0">
                    <div class="saveBtn" style="display: inline-block; margin: 0 10px">
                        <div style="background: #4d74a8; color: #fff; border: 2px solid #fff; padding: 5px 10px;
                            text-transform: uppercase; font-weight: bold; font-size: 12px; letter-spacing: 0.8px">
                            <%--<dxe:ASPxButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Onclick">
                                <ClientSideEvents Click="function(s,e){return CheckSubmitValidation();}" />
                            </dxe:ASPxButton>--%>
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="return CheckSubmitValidation();"
                                OnClick="btnSave_Onclick" />
                        </div>
                    </div>
                    <div class="cancelBtn" style="display: inline-block; margin: 0 10px">
                        <div style="background: #4d74a8; color: #fff; border: 2px solid #fff; padding: 5px 10px;
                            text-transform: uppercase; font-weight: bold; font-size: 12px; letter-spacing: 0.8px">
                            <dxe:ASPxButton Visible="false" ID="btnCancel" runat="server" Text="Cancel">
                            </dxe:ASPxButton>
                            <asp:Button ID="btnCan" OnClientClick="jq132(document).ready(function() { Trans_Popup.Hide(); }); return false;"
                                runat="server" Text="Cancel" />
                            <input type="button" value="Assing Values" onclick="fetchLebel()" />
                        </div>
                    </div>
                    <asp:HiddenField ID="hdnDeliveryAt" runat="server" />
                </div>
            </div>
        </dxe:PopupControlContentControl>
    </ContentCollection>
    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
</dxe:ASPxPopupControl>
&nbsp;<dxe:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server">
</dxe:ASPxGridViewExporter>
<dxe:ASPxPopupControl ID="Conversion_Popup" runat="server" ClientInstanceName="Conversion_Popup">
    <ContentStyle VerticalAlign="Top" CssClass="pad">
    </ContentStyle>
    <ClientSideEvents PopUp="function (s,e){ ConvertDataset(); }" />
    <ContentCollection>
        <dxe:PopupControlContentControl ID="Convertion_PopupControlContentControl" runat="server">
            <div>
                <div>
                    <div>
                        Warehouse:
                    </div>
                    <div>
                        <dxe:ASPxTextBox ClientInstanceName="WHDeliveryFrom_Convert" ID="cmbWarehouses" runat="server"
                            Style="width: 100%">
                        </dxe:ASPxTextBox>
                        <asp:HiddenField ID="WHDeliveryFrom_Convert_hidden" runat="server" />
                    </div>
                </div>
                <div>
                    <div>
                        Quantity to convert:
                    </div>
                    <div>
                        <asp:TextBox ID="txtQuantity_toconvert" runat="server" onkeyup="ConvertOperation();"></asp:TextBox>
                        <asp:Label ID="lblQuantity_toconvertUnit" runat="server"></asp:Label>
                        <asp:HiddenField ID="hdnQuantity_toconvertUnit" runat="server" />
                    </div>
                </div>
                <div>
                    <div>
                        Warehouse Quantity:
                    </div>
                    <div>
                        <asp:TextBox ID="txtWarehouseQuantity" runat="server"></asp:TextBox>
                        <asp:Label ID="txtWarehouseQuantityUnit" runat="server"></asp:Label>
                        <asp:HiddenField ID="hdnWarehouseQuantityUnit" runat="server" />
                    </div>
                </div>
                <%--.--%>
                <%-- <div>
                    <div>
                        Stock left for this Warehouse:
                    </div>
                    <div>
                  
                       <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox> 
                    </div>
                </div>--%>
                <div>
                    <div>
                        <asp:Button ID="btnConversionSubmit" runat="server" Text="Submit" OnClick="btnConversionSubmit_OnClick"
                            OnClientClick="return TransactionButtonCheck();" />
                    </div>
                    <div>
                        <asp:Button ID="btnConversionCancel" runat="server" Text="Cancel" />
                    </div>
                </div>
            </div>
        </dxe:PopupControlContentControl>
    </ContentCollection>
</dxe:ASPxPopupControl>
<dxe:ASPxPopupControl ID="AssignValuePopup" runat="server" ClientInstanceName="AssignValuePopup"
    Width="200px" HeaderText="Add / Edit Key Value" PopupHorizontalAlign="WindowCenter"
    BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
    Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
    ContentStyle-CssClass="pad">
    <ContentStyle VerticalAlign="Top" CssClass="pad">
    </ContentStyle>
    <ContentCollection>
        <dxe:PopupControlContentControl ID="AssignValuePopupContent" runat="server">
            <div id="generatedForm">
            </div>
            <div id="SubmitFrm">
                <asp:TextBox ID="KeyField" runat="server" Style="display: none;"></asp:TextBox>
                <asp:TextBox ID="ValueField" runat="server" Style="display: none;"></asp:TextBox>
                <asp:TextBox ID="RexPageName" runat="server" Style="display: none;"></asp:TextBox>
                <asp:Button ID="Button1" runat="server" Text="Save" OnClientClick="return SaveDataToResource()"
                    OnClick="BTNSave_clicked" Style="margin-left: 155px;" />
            </div>
        </dxe:PopupControlContentControl>
    </ContentCollection>
    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
</dxe:ASPxPopupControl>
