<%@ Page Title="Productwise Stock Status" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" CodeBehind="ProductsWareHouseSerialReport.aspx.cs" Inherits="ERP.OMS.Management.Master.ProductsWareHouseSerialReport" %>

<%--<%@ Register assembly="DevExpress.Web.v15.1, Version=15.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dxe" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Opening Balances - Product(s)</title>
    <style>
        .dxgvEditFormDisplayRow_PlasticBlue td.dxgv, .dxgvDataRow_PlasticBlue td.dxgv, .dxgvDataRowAlt_PlasticBlue td.dxgv, .dxgvSelectedRow_PlasticBlue td.dxgv, .dxgvFocusedRow_PlasticBlue td.dxgv {
            overflow: visible !important;
        }

        .dxgvControl_PlasticBlue td.dxgvBatchEditModifiedCell_PlasticBlue {
            background: #fff !important;
        }

        #OpeningGrid_DXMainTable > tbody > tr > td:first-child {
            display: none !important;
        }

        #OpeningGrid_DXMainTable > tbody > tr > td:nth-child(2) {
            display: none !important;
        }

        #RequiredFieldValidatortxtbatchqntity {
            right: -2px !important;
            top: 19px !important;
        }

        #RequiredFieldValidatortxtwareqntity {
            right: -2px !important;
            top: 19px !important;
        }

        #content-5 {
            width: 100%;
        }
    </style>
    <script type="text/javascript">
        function SignOff() {
            window.parent.SignOff()
        }
        $(document).ready(function () {
            //cOpeningGrid.batchEditApi.StartEdit(-1, 1);
            // cCmblevel1.PerformCallback();
            //cOpeningGrid.batchEditApi.EndEdit();

            //  cbtnSaveRecords.SetVisible(false);

        })

        function OnKeyDown(s, e) {
            if (e.htmlEvent.keyCode == 40 || e.htmlEvent.keyCode == 38)
                return ASPxClientUtils.PreventEvent(e.htmlEvent);
        }

        function ShowError(obj) {


            //IntializeGlobalVariables(s);
            if (cOpeningGrid.cpupdatemssg != null) {

                jAlert(cOpeningGrid.cpupdatemssg);
                cOpeningGrid.cpupdatemssg = null;
            }
            if (cOpeningGrid.cpsaveenableornot != null) {
                if (cOpeningGrid.cpsaveenableornot == "enable") {
                    // cbtnSaveRecords.SetVisible(true);

                } if (cOpeningGrid.cpsaveenableornot == "disable") {
                    // cbtnSaveRecords.SetVisible(false);

                }

                cOpeningGrid.cpsaveenableornot = null;
            }

            if (cOpeningGrid.cpDelete != null) {
                if (cOpeningGrid.cpDelete == 'Success') {
                    jAlert('Deleted Successfully');
                    cOpeningGrid.cpDelete = null;
                }
                else {
                    jAlert('Used in other module.Can not delete');
                    cOpeningGrid.cpDelete = null;
                }

            }

        }
        function saveandupdate() {
            //alert("fire");

            var branchid = ccmbbranch.GetValue();

            if (branchid == null || branchid == "0") {
                jAlert("Please Select Branch.");
            } else {

                cOpeningGrid.UpdateEdit();
            }

            //cOpeningGrid.PerformCallback();
        }
        function abc(a) {
            a.SetEidth(1200)
        }

        function CheckProductStockdetails(indexno, stockids, productid, productname, uom, isactivebatch, isactiveserial, branchids, iswarehousactive, warehouseid, oldopeing, availablestock, outstock) {
            //alert(iswarehousactive + "/" + isactivebatch + "/" + isactiveserial);
            // var stockids = cOpeningGrid.GetEditor('Stock_ID').GetValue();

            $("#RequiredFieldValidatortxtbatch").css("display", "none");
            $("#RequiredFieldValidatortxtserial").css("display", "none");
            $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");

            $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
            $("#RequiredFieldValidatortxtwareqntity").css("display", "none");

            $(".blockone").css("display", "none");
            $(".blocktwo").css("display", "none");
            $(".blockthree").css("display", "none");

            ctxtqnty.SetText("0.0");
            ctxtqnty.SetEnabled(true);

            ctxtbatchqnty.SetText("0.0");
            ctxtserial.SetText("");
            ctxtbatchqnty.SetText("");

            ctxtbatch.SetEnabled(true);
            cCmbWarehouse.SetEnabled(true);

            $('#<%=hdnoutstock.ClientID %>').val("0");

            $('#<%=hdnisedited.ClientID %>').val("false");
            $('#<%=hdnisoldupdate.ClientID %>').val("false");
            $('#<%=hdnisnewupdate.ClientID %>').val("false");
            $('#<%=hdfProductID.ClientID %>').val(0);
            $('#<%=hdnisolddeleted.ClientID %>').val("false");

            $('#<%=hdntotalqnty.ClientID %>').val(0);
            $('#<%=hdnoldrowcount.ClientID %>').val(0);
            $('#<%=hdndeleteqnity.ClientID %>').val(0);
            $('#<%=hidencountforserial.ClientID %>').val("1");

            $('#<%=hdfstockid.ClientID %>').val(0);
            $('#<%=hdfopeningstock.ClientID %>').val(0);
            $('#<%=oldopeningqntity.ClientID %>').val(0);
            $('#<%=hdnnewenterqntity.ClientID %>').val(0);

            $('#<%=hdnenterdopenqnty.ClientID %>').val(0);
            $('#<%=hdbranchID.ClientID %>').val(0);

            $('#<%=hdnisviewqntityhas.ClientID %>').val("false");

            $('#<%=hdnisserial.ClientID %>').val("");
            $('#<%=hdnisbatch.ClientID %>').val("");
            $('#<%=hdniswarehouse.ClientID %>').val("");
            $('#<%=hdndefaultID.ClientID %>').val("");
            $('#<%=hdnbatchchanged.ClientID %>').val("0");
            $('#<%=hdnrate.ClientID %>').val("0");
            $('#<%=hdnvalue.ClientID %>').val("0");
            $('#<%=hdnstrUOM.ClientID %>').val(uom);
            var branchid = ccmbbranch.GetValue();
            $('#<%=hdnisreduing.ClientID %>').val("false");
            //alert(cOpeningGrid.GetEditor('OpeningStock').GetValue());
            var ProductID = (cOpeningGrid.GetEditor('ProductID').GetValue() != null) ? cOpeningGrid.GetEditor('ProductID').GetValue() : "";
            var StkQuantityValue = (cOpeningGrid.GetEditor('OpeningStock').GetValue() != null) ? cOpeningGrid.GetEditor('OpeningStock').GetValue() : "0.0000";


            var checkstockdecimalornot = StkQuantityValue.toString().split(".")[1]

            if (productid != "" && productid == ProductID) {

                //commnet for allowing reduce from main page
                //var isnegetiveoqnti = Number(StkQuantityValue) - Number(availablestock);
                //if (isnegetiveoqnti < 0) {
                //    jAlert("Opening quantity and Available quantity are mismatch.");
                //    return;
                //}
                if (outstock != "0") {
                    var isnegetiveoqnti = Number(StkQuantityValue) - Number(outstock);
                    $('#<%=hdnoutstock.ClientID %>').val(isnegetiveoqnti);
                    if (isnegetiveoqnti < 0) {
                        jAlert("Opening quantity and Available quantity are mismatch.");
                        return;
                    }

                } else {


                }

                if (outstock == "0" && Number(StkQuantityValue) == 0) {
                    var isnegetiveoqnti = Number(StkQuantityValue) - Number(oldopeing);
                    if (isnegetiveoqnti < 0) {
                        //jAlert("Opening quantity and Available quantity are mismatch.");
                        //return;
                        $('#<%=hdnisreduing.ClientID %>').val("true");
                    } else {
                        $('#<%=hdnisreduing.ClientID %>').val("false");
                    }

                }

                if (stockids == "none") {
                    jAlert("Please Update the Product stock, then click here.");

                } else if (branchid == null || branchid == "0" || branchid == "00") {
                    jAlert("Please Select Branch.");
                }
                else if ((StkQuantityValue == "0.0000" || StkQuantityValue == "0" || StkQuantityValue == "0.0" || StkQuantityValue == "000000000.0000") && (oldopeing == "0.0000" || oldopeing == "0.0000" || oldopeing == "0" || oldopeing == "0.0" || oldopeing == "000000000.0000")) {
                    jAlert("Quantity should not be zero.");
                }
                else if (isactivebatch == "false" && isactiveserial == "false" && iswarehousactive == "false") {
                    jAlert("Please configure Inventry before click here.");
                }
                else if (Number(checkstockdecimalornot) / 1 != 0 && isactiveserial == "true") {
                    jAlert("Serial number is activated, Quantity should not contain decimals.");
                }
                else {


                    //var ProductName = (cOpeningGrid.GetEditor('ProductName').GetValue() != null) ? cOpeningGrid.GetEditor('ProductID').GetValue() : "";
                    var ProductName = cOpeningGrid.batchEditApi.GetCellValue(indexno, "ProductName");

                    var ratevalue = (cOpeningGrid.GetEditor('itemsvalue').GetValue() != null) ? cOpeningGrid.GetEditor('itemsvalue').GetValue() : "0";
                    var rate = (cOpeningGrid.GetEditor('itemrate').GetValue() != null) ? cOpeningGrid.GetEditor('itemrate').GetValue() : "0";

                    // var branchid = (cOpeningGrid.GetEditor('branch').GetValue() != null) ? cOpeningGrid.GetEditor('branch').GetValue() : "0";
                    var branchid = ccmbbranch.GetValue();
                    //var BranchNames = (cOpeningGrid.GetEditor('branch').GetText() != null) ? cOpeningGrid.GetEditor('branch').GetText() : "0";
                    var BranchNames = ccmbbranch.GetText();

                    // alert(BranchNames)

                    //var SpliteDetails = ProductID.split("||@||");

                    productname = productname.replace('dquote', '"');


                    var strProductID = productid;
                    var strDescription = "";
                    var strUOM = (uom != null) ? uom : "0";

                    var strProductName = productname;

                    document.getElementById('<%=lblbranchName.ClientID %>').innerHTML = BranchNames;

                    $('#<%=hdndefaultID.ClientID %>').val(warehouseid);
                    $('#<%=hdfProductID.ClientID %>').val(productid);
                    $('#<%=hdfstockid.ClientID %>').val(stockids);
                    var calculateopein = Number(StkQuantityValue) - Number(availablestock);
                    var oldqnt = Number(oldopeing);
                    //alert(calculateopein);
                    //commnet for allowing reduce from main page
                    <%--$('#<%=hdfopeningstock.ClientID %>').val(calculateopein.toString().replace("-", ""));--%>
                    $('#<%=hdfopeningstock.ClientID %>').val(StkQuantityValue);
                    $('#<%=oldopeningqntity.ClientID %>').val(oldqnt);
                    $('#<%=hdnnewenterqntity.ClientID %>').val(StkQuantityValue);
                    $('#<%=hdnenterdopenqnty.ClientID %>').val(oldopeing);
                    $('#<%=hdbranchID.ClientID %>').val(branchid);
                    $('#<%=hdnselectedbranch.ClientID %>').val(branchid);

                    $('#<%=hdnrate.ClientID %>').val(rate);
                    $('#<%=hdnvalue.ClientID %>').val(ratevalue);
                    // $("#lblProductName").text(strProductName)
                    //$("#lblAvailableStk").text(StkQuantityValue + " " + strUOM);
                    var dtd = (Number(StkQuantityValue)).toFixed(4);

                    $("#lblopeningstock").text(dtd + " " + strUOM);

                    ctxtmkgdate.SetDate = null;
                    txtexpirdate.SetDate = null;
                    ctxtserial.SetValue("");
                    ctxtbatch.SetValue("");
                    ctxtqnty.SetValue("0.0");
                    ctxtbatchqnty.SetValue("0.0");

                    var hv = $('#hdnselectedbranch').val();
                    //alert(hv);

                    //cGrdWarehouse.PerformCallback('clreaAll~')

                    cCmbWarehouse.PerformCallback('BindWarehouse');

                    if (iswarehousactive == "true") {

                        cCmbWarehouse.SetVisible(true);
                        cCmbWarehouse.SetSelectedIndex(1);
                        cCmbWarehouse.Focus();
                        ctxtqnty.SetVisible(true);
                        $('#<%=hdniswarehouse.ClientID %>').val("true");

                        $(".blockone").css("display", "block");

                    } else {
                        cCmbWarehouse.SetVisible(false);
                        ctxtqnty.SetVisible(false);
                        $('#<%=hdniswarehouse.ClientID %>').val("false");

                        cCmbWarehouse.SetSelectedIndex(-1);
                        $(".blockone").css("display", "none");

                    }

                    if (isactivebatch == "true") {

                        ctxtbatch.SetVisible(true);
                        ctxtmkgdate.SetVisible(true);
                        ctxtexpirdate.SetVisible(true);
                        $('#<%=hdnisbatch.ClientID %>').val("true");

                        $(".blocktwo").css("display", "block");

                    } else {
                        ctxtbatch.SetVisible(false);
                        ctxtmkgdate.SetVisible(false);
                        ctxtexpirdate.SetVisible(false);
                        $('#<%=hdnisbatch.ClientID %>').val("false");

                        $(".blocktwo").css("display", "none");

                    }
                    if (isactiveserial == "true") {
                        ctxtserial.SetVisible(true);
                        $('#<%=hdnisserial.ClientID %>').val("true");


                        $(".blockthree").css("display", "block");
                    } else {
                        ctxtserial.SetVisible(false);
                        $('#<%=hdnisserial.ClientID %>').val("false");


                        $(".blockthree").css("display", "none");
                    }

                    if (iswarehousactive == "false" && isactivebatch == "true") {
                        ctxtbatchqnty.SetVisible(true);

                        $(".blocktwoqntity").css("display", "block");
                    } else {
                        ctxtbatchqnty.SetVisible(false);
                        $(".blocktwoqntity").css("display", "none");
                    }

                    if (iswarehousactive == "false" && isactivebatch == "true") {
                        ctxtbatch.Focus();
                    } else {
                        cCmbWarehouse.Focus();
                    }

                    cbtnWarehouse.SetVisible(true);
                    cGrdWarehouse.PerformCallback('checkdataexist~' + strProductID + '~' + stockids + '~' + branchid + '~' + strProductName);

                    cPopup_Warehouse.Show();
                }
} else if (branchid != "00" && branchid != "0") {

    //var ProductName = (cOpeningGrid.GetEditor('ProductName').GetValue() != null) ? cOpeningGrid.GetEditor('ProductID').GetValue() : "";
    var ProductName = cOpeningGrid.batchEditApi.GetCellValue(indexno, "ProductName");

    var ratevalue = (cOpeningGrid.GetEditor('itemsvalue').GetValue() != null) ? cOpeningGrid.GetEditor('itemsvalue').GetValue() : "0";
    var rate = (cOpeningGrid.GetEditor('itemrate').GetValue() != null) ? cOpeningGrid.GetEditor('itemrate').GetValue() : "0";

    // var branchid = (cOpeningGrid.GetEditor('branch').GetValue() != null) ? cOpeningGrid.GetEditor('branch').GetValue() : "0";
    var branchid = ccmbbranch.GetValue();
    //var BranchNames = (cOpeningGrid.GetEditor('branch').GetText() != null) ? cOpeningGrid.GetEditor('branch').GetText() : "0";
    var BranchNames = ccmbbranch.GetText();

    // alert(BranchNames)

    //var SpliteDetails = ProductID.split("||@||");

    if (ProductName == "") {
        return;
    }

    productname = productname.replace('dquote', '"');


    var strProductID = productid;
    var strDescription = "";
    var strUOM = (uom != null) ? uom : "0";

    var strProductName = productname;

    document.getElementById('<%=lblbranchName.ClientID %>').innerHTML = BranchNames;

    $('#<%=hdndefaultID.ClientID %>').val(warehouseid);
    $('#<%=hdfProductID.ClientID %>').val(productid);
    $('#<%=hdfstockid.ClientID %>').val(stockids);
                    <%-- var calculateopein = Number(StkQuantityValue) - Number(oldopeing);
                    var oldqnty = Number(oldopeing);
                    //alert(calculateopein);
                    $('#<%=hdfopeningstock.ClientID %>').val(calculateopein.toString().replace("-", ""));--%>
    $('#<%=hdfopeningstock.ClientID %>').val(StkQuantityValue);
    $('#<%=oldopeningqntity.ClientID %>').val(oldqnty);
    $('#<%=hdnnewenterqntity.ClientID %>').val(StkQuantityValue);
    $('#<%=hdnenterdopenqnty.ClientID %>').val(oldopeing);
    $('#<%=hdbranchID.ClientID %>').val(branchid);

    $('#<%=hdnrate.ClientID %>').val(rate);
    $('#<%=hdnvalue.ClientID %>').val(ratevalue);
    // $("#lblProductName").text(strProductName)
    //$("#lblAvailableStk").text(StkQuantityValue + " " + strUOM);
    var dtd = (Number(StkQuantityValue)).toFixed(4);
    $("#lblopeningstock").text(dtd + " " + strUOM);

    ctxtmkgdate.SetDate = null;
    txtexpirdate.SetDate = null;
    ctxtserial.SetValue("");
    ctxtbatch.SetValue("");
    ctxtqnty.SetValue("0.0");
    ctxtbatchqnty.SetValue("0.0");

    var hv = $('#hdnselectedbranch').val();
    //alert(hv);

    //cGrdWarehouse.PerformCallback('clreaAll~')

    cCmbWarehouse.PerformCallback('BindWarehouse');

    if (iswarehousactive == "true") {

        cCmbWarehouse.SetVisible(true);
        cCmbWarehouse.SetSelectedIndex(1);
        ctxtqnty.SetVisible(true);
        $('#<%=hdniswarehouse.ClientID %>').val("true");

        $(".blockone").css("display", "block");

    } else {
        cCmbWarehouse.SetVisible(false);
        ctxtqnty.SetVisible(false);
        $('#<%=hdniswarehouse.ClientID %>').val("false");

        cCmbWarehouse.SetSelectedIndex(-1);
        $(".blockone").css("display", "none");

    }

    if (isactivebatch == "true") {

        ctxtbatch.SetVisible(true);
        ctxtmkgdate.SetVisible(true);
        ctxtexpirdate.SetVisible(true);
        $('#<%=hdnisbatch.ClientID %>').val("true");

        $(".blocktwo").css("display", "block");

    } else {
        ctxtbatch.SetVisible(false);
        ctxtmkgdate.SetVisible(false);
        ctxtexpirdate.SetVisible(false);
        $('#<%=hdnisbatch.ClientID %>').val("false");

        $(".blocktwo").css("display", "none");

    }
    if (isactiveserial == "true") {
        ctxtserial.SetVisible(true);
        $('#<%=hdnisserial.ClientID %>').val("true");


        $(".blockthree").css("display", "block");
    } else {
        ctxtserial.SetVisible(false);
        $('#<%=hdnisserial.ClientID %>').val("false");


        $(".blockthree").css("display", "none");
    }

    if (iswarehousactive == "false" && isactivebatch == "true") {
        ctxtbatchqnty.SetVisible(true);

        $(".blocktwoqntity").css("display", "block");
    } else {
        ctxtbatchqnty.SetVisible(false);
        $(".blocktwoqntity").css("display", "none");
    }


    cbtnWarehouse.SetVisible(false);

    if (iswarehousactive == "false" && isactivebatch == "true") {
        ctxtbatch.Focus();
    } else {
        cCmbWarehouse.Focus();
    }
    cGrdWarehouse.PerformCallback('checkdataexist~' + strProductID + '~' + stockids + '~' + branchid + '~' + strProductName);

    cPopup_Warehouse.Show();

} else {
    jAlert("Please Select Branch.");
}


}

function MRPLostfocus(s, e) {
    var mrp = (s.GetValue() != null) ? s.GetValue() : "0.00";
    if (parseFloat(mrp) != 0) {
        var markMinVal = cOpeningGrid.GetEditor('OpeningStock').GetValue();
        var markPlusVal = cOpeningGrid.GetEditor('itemrate').GetValue();
        //alert(markMinVal * mrp);
        if (parseFloat(markMinVal) != 0) {
            cOpeningGrid.GetEditor('itemsvalue').SetValue(parseFloat(markMinVal) * parseFloat(mrp));
        }
        if (parseFloat(markPlusVal) != 0) {
            cOpeningGrid.GetEditor('itemsvalue').SetValue(parseFloat(markMinVal) * parseFloat(mrp));
        }
        if (parseFloat(markMinVal) == 0 && parseFloat(markPlusVal) == 0) {
            cOpeningGrid.GetEditor('itemsvalue').SetValue("0.00");
        }
    } else {

        cOpeningGrid.GetEditor('itemsvalue').SetValue("0.00");
    }

}

function chnagedcombo(s) {
    //alert(s.GetValue());
    $('#<%=hdnselectedbranch.ClientID %>').val(s.GetValue());

    document.getElementById('ddlExport').value = "0";
    document.getElementById('drdExport').value = "0";

    //cOpeningGrid.Enable = false;
    cOpeningGrid.PerformCallback("branchwise~" + s.GetValue());
}
function cleanqntity(s, e) {

    var quantity = Number(s.GetValue());
    //alert(quantity / 1);
    if (quantity / 1 == 0) {
        e.isValid = false;
    } else {
        e.isValid = true;
    }
    //s.SetText("");

}
function Settab(s, e) {

    cOpeningGrid.batchEditApi.EndEdit();


    var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    //alert(keyCode);
    if (keyCode === 9) {
        //cOpeningGrid.batchEditApi.EndEdit();
        // cbtnSaveRecords.Focus();
        cOpeningGrid.batchEditApi.EndEdit();
    }

}
function OnBatchStartEdit(s, e) {
    var branchid = ccmbbranch.GetValue();
    if (branchid == null || branchid == "0") {
        e.cancel = true;
    } else {

        e.cancel = false;
    }

}
function initopengrid() {
    //alert(cOpeningGrid.cpsaveenableornot);
    if (cOpeningGrid.cpsaveenableornot != null) {
        if (cOpeningGrid.cpsaveenableornot == "enable") {
            //cbtnSaveRecords.SetVisible(true);

        } if (cOpeningGrid.cpsaveenableornot == "disable") {
            // cbtnSaveRecords.SetVisible(false);

        }

        cOpeningGrid.cpsaveenableornot = null;
    }
}
    </script>
    <style>
        .manAb .dxeErrorFrameSys.dxeErrorCellSys {
            position: absolute;
        }

        .tp2 {
            position: absolute;
            right: -15px;
            top: 5px;
        }

        #OpeningGrid_DXMainTable.dxgvTable_PlasticBlue {
            overflow: visible !important;
        }

        .lblHolder {
            height: auto;
        }

        .horizontal-images.content li {
            margin-right: 8px;
            margin-bottom: 10px;
        }

        #btnWarehouse, #ASPxButton2 {
            float: left !important;
            display: inline-block;
        }

        #OpeningGrid_DXHeaderTable > tbody > tr:first-child td:first-child,
        #OpeningGrid_DXHeaderTable > tbody > tr:first-child td:last-child {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title" id="td_contact1" runat="server">

            <h3>
                <asp:Label ID="lblHeadTitle" runat="server" Text="Product Opening Stock Status"></asp:Label>
            </h3>
        </div>

    </div>
    <div class="form_main">
        <div class="SearchArea">
            <div class="FilterSide">
                <div style="width: 60px; float: left; padding-top: 5px">Branch: </div>
                <div class="col-sm-4">
                    <dxe:ASPxComboBox ID="cmbbranch" runat="server" ClientInstanceName="ccmbbranch" TextField="branchName" ValueField="branchID">
                        <ClientSideEvents SelectedIndexChanged="function(s,e) { chnagedcombo(s);}" />
                    </dxe:ASPxComboBox>
                </div>
            </div>
            <div class="FilterSide">
                <div class="pull-right">
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnChange="if(!AvailableExportOption()){return false;}" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="pull-right">
                    <asp:DropDownList ID="ddlExport" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged" CssClass="btn btn-sm btn-success">
                        <asp:ListItem Value="0">Export Data For Opening Import</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>

                    <%-- <asp:DropDownList ID="ddlOpeningExport" runat="server" CssClass="btn btn-sm btn-primary" OnChange="if(!AvailableExportOption()){return false;}"  AutoPostBack="true" OnSelectedIndexChanged="ddlOpeningExport_SelectedIndexChanged">
                        <asp:ListItem Value="0">Export For Opening</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>--%>
                </div>
            </div>
            <div class="clear">
                <br />
            </div>
        </div>
        <dxe:ASPxGridView ID="OpeningGrid" runat="server" KeyFieldName="Stock_ID" AutoGenerateColumns="False" EnableRowsCache="true"
            Width="100%" ClientInstanceName="cOpeningGrid" OnBatchUpdate="OpeningGrid_BatchUpdate" OnRowInserting="Grid_RowInserting" OnRowUpdating="Grid_RowUpdating" OnRowDeleting="Grid_RowDeleting"
            OnCustomCallback="OpeningGrid_CustomCallback" OnDataBinding="OpeningGrid_DataBinding">
            <ClientSideEvents EndCallback="function(s,e) { ShowError(s.cpInsertError);}" BatchEditStartEditing="OnBatchStartEdit" Init="function(s,e) { initopengrid();}" />
            <SettingsEditing Mode="Batch">
                <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
            </SettingsEditing>
            <Settings ShowGroupPanel="false" ShowStatusBar="Hidden" ShowTitlePanel="false" ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsDataSecurity AllowDelete="False" />
            <Columns>
                <dxe:GridViewDataTextColumn FieldName="UOMID" ReadOnly="true" VisibleIndex="0" Width="" CellStyle-CssClass="hide" HeaderStyle-CssClass="hide" FilterCellStyle-CssClass="hide">
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="ProductID" VisibleIndex="1" ReadOnly="true" Settings-ShowFilterRowMenu="False" Settings-AllowHeaderFilter="False" CellStyle-VerticalAlign="Top" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="hide" FilterCellStyle-CssClass="hide">
                    <Settings AllowHeaderFilter="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataColumn Caption="Branch" FieldName="branch_description" VisibleIndex="2" Width="100px" CellStyle-VerticalAlign="Top">
                    <EditFormSettings Visible="False" />
                </dxe:GridViewDataColumn>
                <dxe:GridViewDataColumn Caption="Product Code" FieldName="ProductCode" VisibleIndex="3" Width="100px" CellStyle-VerticalAlign="Top">
                    <EditFormSettings Visible="False" />
                </dxe:GridViewDataColumn>
                <dxe:GridViewDataColumn Caption="Product Name" FieldName="ProductName2" VisibleIndex="4" Width="200px" CellStyle-VerticalAlign="Top">
                    <EditFormSettings Visible="False" />
                </dxe:GridViewDataColumn>
                <dxe:GridViewDataColumn Caption="Product Class" FieldName="ProductClass_Name" VisibleIndex="5" Width="150px" CellStyle-VerticalAlign="Top">
                    <EditFormSettings Visible="False" />
                </dxe:GridViewDataColumn>
                <dxe:GridViewDataColumn Caption="Brand" FieldName="Brand_Name" VisibleIndex="6" Width="100px" CellStyle-VerticalAlign="Top">
                    <EditFormSettings Visible="False" />
                </dxe:GridViewDataColumn>
                <dxe:GridViewDataTextColumn Caption="WareHouse" VisibleIndex="7" Width="150px" FieldName="bui_Name" CellStyle-VerticalAlign="Top">
                    <EditFormSettings Visible="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Rate" VisibleIndex="9" Width="100px" FieldName="Rate" CellStyle-VerticalAlign="Top">
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    <CellStyle HorizontalAlign="Right"></CellStyle>
                        <HeaderStyle HorizontalAlign="Right" />
                    <EditFormSettings Visible="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Stock Qty" VisibleIndex="10" Width="100px" FieldName="StockBranchWarehouse_StockIn" CellStyle-VerticalAlign="Top">
                    <PropertiesTextEdit DisplayFormatString="0.0000"></PropertiesTextEdit>
                    <CellStyle HorizontalAlign="Right"></CellStyle>
                        <HeaderStyle HorizontalAlign="Right" />
                    <EditFormSettings Visible="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Value" VisibleIndex="11" Width="100px" FieldName="OPVALUE" CellStyle-VerticalAlign="Top">
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    <CellStyle HorizontalAlign="Right"></CellStyle>
                        <HeaderStyle HorizontalAlign="Right" />
                    <EditFormSettings Visible="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Batch" VisibleIndex="12" Width="15%" FieldName="Batch" CellStyle-VerticalAlign="Top" CellStyle-Wrap="True">
                    <EditFormSettings Visible="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Serial" VisibleIndex="13" Width="15%" FieldName="Serial" CellStyle-Wrap="True">
                    <EditFormSettings Visible="False" />
                </dxe:GridViewDataTextColumn>
            </Columns>
            <SettingsSearchPanel Visible="True" />
            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
            <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </SettingsPager>
            <TotalSummary>
                <dxe:ASPxSummaryItem FieldName="StockBranchWarehouse_StockIn" SummaryType="Sum" />
                <dxe:ASPxSummaryItem FieldName="OPVALUE" SummaryType="Sum" />
            </TotalSummary>
            <%--<ClientSideEvents CustomButtonClick="OnCustomButtonClick" />--%>
        </dxe:ASPxGridView>

    </div>
    <br />
    <div>
        <% if (rights.CanAdd && rights.CanEdit)
           { %>
        <dxe:ASPxButton ID="btnSaveRecords" ClientInstanceName="cbtnSaveRecords" runat="server" VisibleIndex="10" Visible="false"
            AccessKey="S" AutoPostBack="false" CssClass="btn btn-primary" Text="S&#818;ave"
            UseSubmitBehavior="False">
            <ClientSideEvents Click="function(s, e) {saveandupdate();}" />
        </dxe:ASPxButton>
        <% } %>
    </div>

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <dxe:ASPxGridViewExporter ID="Openingexporter" runat="server" Landscape="true" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

    <div style="display: none">
        <%--KeyFieldName="Stock_ID"--%>
        <dxe:ASPxGridView ID="openingGridExport" runat="server" AutoGenerateColumns="True"
            Width="100%" EnableRowsCache="true" SettingsBehavior-AllowFocusedRow="true"
            OnDataBinding="openingGridExport_DataBinding">
            <Columns>
                <dxe:GridViewDataTextColumn FieldName="Branch" Caption="Branch" VisibleIndex="0">
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="Product_Code" Caption="Product Code" VisibleIndex="1">
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="Product_Name" Caption="Product Name" VisibleIndex="2">
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="Warehouse" Caption="Warehouse" VisibleIndex="3">
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="Rate" Caption="Rate" VisibleIndex="4">
                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="Stock_Qty" Caption="Stock Qty" VisibleIndex="5">
                     <PropertiesTextEdit DisplayFormatString="0.0000"></PropertiesTextEdit>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Value" VisibleIndex="6" Width="100px" FieldName="OPVALUE">
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    <CellStyle HorizontalAlign="Right"></CellStyle>
                    <HeaderStyle HorizontalAlign="Right" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="Batch" Caption="Batch" VisibleIndex="7">
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="Serial" Caption="Serial" VisibleIndex="8">
                </dxe:GridViewDataTextColumn>
            </Columns>
        </dxe:ASPxGridView>
    </div>

    <%-- PopUp Section Start here--%>

    <dxe:ASPxPopupControl ID="Popup_Warehouse" runat="server" ClientInstanceName="cPopup_Warehouse"
        Width="900px" HeaderText="Stock Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="Top clearfix">
                    <div class="row">
                        <div class="col-md-3">
                            <div class="lblHolder">
                                <table>
                                    <tr>
                                        <td>Selected Branch</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblbranchName" runat="server"></asp:Label></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="lblHolder">
                                <table>
                                    <tr>
                                        <td>Selected Product</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblProductName" runat="server"></asp:Label></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="lblHolder">
                                <table>
                                    <tr>
                                        <td>Available Stock</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblAvailableStk" runat="server" Text="0.0"></asp:Label></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="lblHolder">
                                <table>
                                    <tr>
                                        <td>Entered Stock</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblopeningstock" runat="server" Text="0.0000"></asp:Label></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>


                    <div class="clear">
                        <br />
                    </div>
                    <div class="clearfix">
                        <div class="row manAb">
                            <div class="blockone">
                                <div class="col-md-3">
                                    <div>
                                        <span id="RequiredFieldValidatorCmbWarehousetxt">Warehouse</span>
                                    </div>
                                    <div class="Left_Content relative" style="">
                                        <%-- <dxe:ASPxTextBox ID="txtwarehousname" runat="server" Width="80%" ClientInstanceName="ctxtwarehousname" HorizontalAlign="Left" Font-Size="12px">
                                    </dxe:ASPxTextBox>--%>
                                        <dxe:ASPxComboBox ID="CmbWarehouse" EnableIncrementalFiltering="True" ClientInstanceName="cCmbWarehouse" SelectedIndex="0"
                                            TextField="WarehouseName" ValueField="WarehouseID" runat="server" Width="100%" OnCallback="CmbWarehouse_Callback">
                                            <ClientSideEvents ValueChanged="function(s,e){CmbWarehouse_ValueChange(s)}" EndCallback="function(s,e){endcallcmware(s)}"></ClientSideEvents>

                                        </dxe:ASPxComboBox>
                                        <span id="RequiredFieldValidatorCmbWarehouse" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div>
                                        <span id="RequiredFieldValidatorCmbWarehouseQuantity">Quantity</span>
                                    </div>
                                    <div class="Left_Content" style="">
                                        <dxe:ASPxTextBox ID="txtqnty" runat="server" Width="100%" ClientInstanceName="ctxtqnty" HorizontalAlign="Left" Font-Size="12px">
                                            <MaskSettings Mask="<0..999999999>.<0..9999>" IncludeLiterals="DecimalSymbol" />
                                            <ClientSideEvents TextChanged="function(s,e){changedqnty(s)}" LostFocus="function(s,e){Setenterfocuse(s)}" />
                                        </dxe:ASPxTextBox>
                                        <span id="RequiredFieldValidatortxtwareqntity" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                    </div>
                                </div>
                            </div>
                            <div class="clear"></div>
                            <div class="blocktwo">
                                <div class="col-md-3">
                                    <div>
                                        <span id="RequiredFieldValidatortxtbatchtxt">Batch</span>
                                    </div>
                                    <div class="Left_Content relative" style="">
                                        <dxe:ASPxTextBox ID="txtbatch" runat="server" Width="100%" ClientInstanceName="ctxtbatch" HorizontalAlign="Left" Font-Size="12px" MaxLength="49">
                                            <ClientSideEvents TextChanged="function(s,e){chnagedbtach(s)}" />
                                        </dxe:ASPxTextBox>
                                        <span id="RequiredFieldValidatortxtbatch" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                    </div>
                                </div>
                                <div class="col-md-3 blocktwoqntity">
                                    <div>
                                        <span id="RequiredFieldValidatorbatchQuantity">Quantity</span>
                                    </div>
                                    <div class="Left_Content" style="">
                                        <dxe:ASPxTextBox ID="batchqnty" runat="server" Width="100%" ClientInstanceName="ctxtbatchqnty" HorizontalAlign="Left" Font-Size="12px">
                                            <MaskSettings Mask="<0..999999999>.<0..9999>" IncludeLiterals="DecimalSymbol" />
                                            <ClientSideEvents TextChanged="function(s,e){changedqntybatch(s)}" />
                                        </dxe:ASPxTextBox>
                                        <span id="RequiredFieldValidatortxtbatchqntity" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div>
                                        <span id="RequiredFieldValidatortxtbatchtxtmkgdate">Manufacture Date</span>
                                    </div>
                                    <div class="Left_Content" style="">
                                        <%--<dxe:ASPxTextBox ID="txtmkgdate" runat="server" Width="80%" ClientInstanceName="ctxtmkgdate" HorizontalAlign="Left" Font-Size="12px">
                                    </dxe:ASPxTextBox>--%>
                                        <dxe:ASPxDateEdit ID="txtmkgdate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True" ClientInstanceName="ctxtmkgdate" AllowNull="true" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>

                                        </dxe:ASPxDateEdit>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div>
                                        <span id="RequiredFieldValidatortxtbatchtxtexpdate">Expiry Date</span>
                                    </div>
                                    <div class="Left_Content" style="">
                                        <%-- <dxe:ASPxTextBox ID="txtexpirdate" runat="server" Width="80%" ClientInstanceName="ctxtexpirdate" HorizontalAlign="Left" Font-Size="12px">
                                    </dxe:ASPxTextBox>--%>
                                        <dxe:ASPxDateEdit ID="txtexpirdate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True" ClientInstanceName="ctxtexpirdate" AllowNull="true" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>

                                        </dxe:ASPxDateEdit>
                                    </div>
                                </div>
                            </div>
                            <div class="clear"></div>
                            <div class="blockthree">
                                <div class="col-md-3">
                                    <div>
                                        <span id="RequiredFieldValidatortxtserialtxt">Serial No</span>
                                    </div>
                                    <div class="Left_Content relative" style="">
                                        <dxe:ASPxTextBox ID="txtserial" runat="server" Width="100%" ClientInstanceName="ctxtserial" HorizontalAlign="Left" Font-Size="12px" MaxLength="49">
                                        </dxe:ASPxTextBox>
                                        <span id="RequiredFieldValidatortxtserial" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div>
                                </div>
                                <div class=" clearfix" style="padding-top: 11px;">
                                    <dxe:ASPxButton ID="btnWarehouse" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary pull-left">
                                        <ClientSideEvents Click="function(s, e) {SaveWarehouse();}" />
                                    </dxe:ASPxButton>

                                    <dxe:ASPxButton ID="ASPxButton2" ClientInstanceName="cbtnrefreshWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="Clear Entries" CssClass="btn btn-primary pull-left">
                                        <ClientSideEvents Click="function(s, e) {Clraear();}" />
                                    </dxe:ASPxButton>

                                </div>
                            </div>

                        </div>
                        <br />


                        <div class="clearfix">
                            <dxe:ASPxGridView ID="GrdWarehouse" runat="server" KeyFieldName="SrlNo" AutoGenerateColumns="False" SettingsBehavior-AllowSort="false"
                                Width="100%" ClientInstanceName="cGrdWarehouse" OnCustomCallback="GrdWarehouse_CustomCallback" OnDataBinding="GrdWarehouse_DataBinding">
                                <Columns>
                                    <dxe:GridViewDataTextColumn Caption="Warehouse Name" FieldName="viewWarehouseName"
                                        VisibleIndex="0">
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Batch Number" FieldName="viewBatchNo"
                                        VisibleIndex="2">
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataDateColumn Caption="Manufacture Date" FieldName="viewMFGDate"
                                        VisibleIndex="2">
                                        <Settings AllowHeaderFilter="False" />
                                        <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy"></PropertiesDateEdit>
                                    </dxe:GridViewDataDateColumn>

                                    <dxe:GridViewDataDateColumn Caption="Expiry Date" FieldName="viewExpiryDate"
                                        VisibleIndex="2">
                                        <Settings AllowHeaderFilter="False" />
                                        <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy"></PropertiesDateEdit>
                                    </dxe:GridViewDataDateColumn>
                                    <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="viewQuantity"
                                        VisibleIndex="3">
                                        <Settings ShowInFilterControl="False" />
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity"
                                        VisibleIndex="5">
                                        <Settings ShowInFilterControl="False" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Serial Number" FieldName="viewSerialNo"
                                        VisibleIndex="4">
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Action" FieldName="SrlNo" CellStyle-VerticalAlign="Middle" VisibleIndex="6" CellStyle-HorizontalAlign="Center" Settings-ShowFilterRowMenu="False" Settings-AllowHeaderFilter="False" Settings-AllowAutoFilter="False" Width="100px">
                                        <EditFormSettings Visible="False" />
                                        <DataItemTemplate>
                                            <a href="javascript:void(0);" onclick="UpdateWarehousebatchserial(<%#Eval("SrlNo")%>,'<%#Eval("WarehouseID")%>','<%#Eval("BatchNo")%>','<%#Eval("SerialNo")%>','<%#Eval("isnew")%>','<%#Eval("viewQuantity")%>','<%#Eval("Quantity")%>')" title="update Details" class="pad">
                                                <img src="../../../assests/images/Edit.png" />
                                            </a>
                                            <a href="javascript:void(0);" onclick="DeleteWarehousebatchserial(<%#Eval("SrlNo")%>,'<%#Eval("BatchWarehouseID")%>','<%#Eval("viewQuantity")%>',<%#Eval("Quantity")%>,'<%#Eval("WarehouseID")%>','<%#Eval("BatchNo")%>')" title="delete Details" class="pad">
                                                <img src="../../../assests/images/crs.png" />
                                            </a>
                                        </DataItemTemplate>
                                    </dxe:GridViewDataTextColumn>
                                </Columns>
                                <ClientSideEvents EndCallback="function(s,e) { cGrdWarehouseShowError(s.cpInsertError);}" />
                                <%-- <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True">
                                    <FirstPageButton Visible="True">
                                    </FirstPageButton>
                                    <LastPageButton Visible="True">
                                    </LastPageButton>
                                </SettingsPager>--ShowFilterRow="true" ShowFilterRowMenu="true" --%>
                                <SettingsPager Mode="ShowAllRecords" />
                                <Settings ShowGroupPanel="false" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" VerticalScrollBarMode="Visible" VerticalScrollableHeight="190" />
                                <SettingsLoadingPanel Text="Please Wait..." />
                            </dxe:ASPxGridView>
                        </div>
                        <br />
                        <div class="Center_Content" style="">
                            <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="S&#818;ave & Exit" AccessKey="S" CssClass="btn btn-primary">
                                <ClientSideEvents Click="function(s, e) {SaveWarehouseAll();}" />
                            </dxe:ASPxButton>


                        </div>
                    </div>
                    <%-- <div class="text-center">
                        <table class="pull-right">
                            <tr>
                                <td style="padding-right: 15px"><strong>Total</strong></td>
                                <td>
                                    <dxe:ASPxTextBox ID="ASPxTextBox3" runat="server" Width="100%" ClientInstanceName="ctxtTotalAmount" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                    </dxe:ASPxTextBox>
                                </td>
                            </tr>
                        </table>
                    </div>--%>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>

    <script type="text/javascript">

        $(document).ready(function () {


            var isCtrl = false;
            document.onkeydown = function (e) {

                if ((event.keyCode == 120 || event.keyCode == 88) && event.ctrlKey == true) {

                    //run code for Ctrl+X -- ie, Save & Exit! 
                    document.getElementById('ASPxButton1_CD').click();
                    return false;
                }
                //if (event.keyCode == 116) {

                //    //run code for Ctrl+X -- ie, Save & Exit! 
                //    cPopup_Warehouse.Hide();
                //    return false;
                //}

            }
        });

        function DeleteWarehousebatchserial(SrlNo, BatchWarehouseID, viewQuantity, Quantity, WarehouseID, BatchNo) {
            //alert(viewQuantity);
            var IsSerial = $('#hdnisserial').val();
            if (IsSerial == "true" && viewQuantity != "1.0000" && viewQuantity != "1.0" && viewQuantity != "") {
                jAlert("Cannot Proceed. You have to delete subsequent data first before delete this data.");
            } else {
                if (BatchWarehouseID == "" || BatchWarehouseID == "0") {

                    $('#<%=hdnisolddeleted.ClientID %>').val("false");
                    if (SrlNo != "") {


                        cGrdWarehouse.PerformCallback('Delete~' + SrlNo + '~' + viewQuantity + '~' + Quantity + '~' + WarehouseID + '~' + BatchNo);
                    }

                } else {

                    $('#<%=hdnisolddeleted.ClientID %>').val("true");
                    if (SrlNo != "") {

                        cGrdWarehouse.PerformCallback('Delete~' + SrlNo + '~' + viewQuantity + '~' + Quantity + '~' + WarehouseID + '~' + BatchNo);
                    }
                }
            }



        }

        function Setenterfocuse(s) {

           <%-- var Isbatch = $('#hdnisbatch').val();
            var IsSerial = $('#hdnisserial').val();
            //alert(Isbatch);
            if (Isbatch == "true") {
                ctxtbatch.Focus();
                document.getElementById("<%=txtbatch.ClientID%>").focus();
            } else if (IsSerial == "true") {
                ctxtserial.Focus();
            }--%>
        }

        function UpdateWarehousebatchserial(SrlNo, WarehouseID, BatchNo, SerialNo, isnew, viewQuantity, Quantity) {

            var Isbatch = $('#hdnisbatch').val();

            if (isnew == "old" || isnew == "Updated") {

                $('#<%=hdnisoldupdate.ClientID %>').val("true");
                $('#<%=hdncurrentslno.ClientID %>').val("");
                cCmbWarehouse.SetValue(WarehouseID);
                if (Quantity != null && Quantity != "" && Isbatch != "true") {
                    ctxtqnty.SetText(Quantity);
                } else {
                    ctxtqnty.SetText(viewQuantity);
                }
                var IsSerial = $('#hdnisserial').val();

                if (IsSerial == "true") {

                    if (viewQuantity == "") {
                        ctxtbatch.SetEnabled(false);
                        cCmbWarehouse.SetEnabled(false);
                        ctxtqnty.SetEnabled(false);
                        ctxtserial.Focus();
                    } else {
                        ctxtbatch.SetEnabled(true);
                        cCmbWarehouse.SetEnabled(true);
                        ctxtqnty.SetEnabled(true);
                        ctxtserial.Focus();
                    }

                }
                else {
                    ctxtbatch.SetEnabled(true);
                    cCmbWarehouse.SetEnabled(true);
                    ctxtqnty.SetEnabled(true);
                    ctxtbatch.Focus();
                }
                // ctxtqnty.SetEnabled(false);

                ctxtbatchqnty.SetText(Quantity);
                //ctxtbatchqnty.SetEnabled(false);
                ctxtbatch.SetText(BatchNo);
                ctxtserial.SetText(SerialNo);

                if (viewQuantity == "") {
                    ctxtbatch.SetEnabled(false);
                    cCmbWarehouse.SetEnabled(false);
                    $('#<%=hdnisviewqntityhas.ClientID %>').val("true");
                } else {
                    ctxtbatch.SetEnabled(true);
                    cCmbWarehouse.SetEnabled(true);
                    $('#<%=hdnisviewqntityhas.ClientID %>').val("false");
                }

                var hdniswarehouse = $('#hdniswarehouse').val();


                if (hdniswarehouse != "true" && Isbatch == "true") {
                    ctxtbatchqnty.SetText(viewQuantity);
                    ctxtbatchqnty.Focus();

                } else {
                    ctxtqnty.Focus();
                }
                $('#<%=hdncurrentslno.ClientID %>').val(SrlNo);

            } else {

                $('#<%=hdnisoldupdate.ClientID %>').val("false");

                ctxtqnty.SetText("0.0");
                ctxtqnty.SetEnabled(true);

                ctxtbatchqnty.SetText("0.0");
                ctxtserial.SetText("");
                ctxtbatchqnty.SetText("");
                $('#<%=hdncurrentslno.ClientID %>').val("");

                $('#<%=hdnisnewupdate.ClientID %>').val("true");
                $('#<%=hdncurrentslno.ClientID %>').val("");
                cCmbWarehouse.SetValue(WarehouseID);
                if (Quantity != null && Quantity != "" && Isbatch != "true") {
                    ctxtqnty.SetText(Quantity);
                } else {
                    ctxtqnty.SetText(viewQuantity);
                }
                var IsSerial = $('#hdnisserial').val();
                if (IsSerial == "true") {

                    if (viewQuantity == "") {
                        ctxtbatch.SetEnabled(false);
                        cCmbWarehouse.SetEnabled(false);
                        ctxtqnty.SetEnabled(false);
                        $('#<%=hdnisviewqntityhas.ClientID %>').val("true");
                        ctxtserial.Focus();
                    } else {
                        ctxtbatch.SetEnabled(true);
                        cCmbWarehouse.SetEnabled(true);
                        ctxtqnty.SetEnabled(true);
                        $('#<%=hdnisviewqntityhas.ClientID %>').val("false");
                        ctxtserial.Focus();
                    }

                } else {
                    ctxtbatch.SetEnabled(true);
                    cCmbWarehouse.SetEnabled(true);
                    ctxtqnty.SetEnabled(true);
                    ctxtbatch.Focus();
                }
                // ctxtqnty.SetEnabled(false);

                ctxtbatchqnty.SetText(Quantity);
                //ctxtbatchqnty.SetEnabled(false);
                ctxtbatch.SetText(BatchNo);
                ctxtserial.SetText(SerialNo);

                if (viewQuantity == "") {
                    ctxtbatch.SetEnabled(false);
                    cCmbWarehouse.SetEnabled(false);
                } else {
                    ctxtbatch.SetEnabled(true);
                    cCmbWarehouse.SetEnabled(true);
                }

                var hdniswarehouse = $('#hdniswarehouse').val();


                if (hdniswarehouse != "true" && Isbatch == "true") {
                    ctxtbatchqnty.SetText(viewQuantity);
                } else {
                    ctxtqnty.Focus();
                }

                $('#<%=hdncurrentslno.ClientID %>').val(SrlNo);

                //jAlert("Sorry, This is new entry you can not update. please click on 'Clear Entries' and Add again.");
            }
        }

        function changedqnty(s) {

            var qnty = s.GetText();
            var sum = $('#hdntotalqnty').val();

            sum = Number(Number(sum) + Number(qnty));
            //alert(sum);
            $('#<%=hdntotalqnty.ClientID %>').val(sum);

        }

        function endcallcmware(s) {

            if (cCmbWarehouse.cpstock != null) {

                var ddd = cCmbWarehouse.cpstock + " " + $('#hdnstrUOM').val();
                document.getElementById('<%=lblAvailableStk.ClientID %>').innerHTML = ddd;
                cCmbWarehouse.cpstock = null;
            }
        }
        function changedqntybatch(s) {

            var qnty = s.GetText();
            var sum = $('#hdntotalqnty').val();
            sum = Number(Number(sum) + Number(qnty));
            //alert(sum);
            $('#<%=hdntotalqnty.ClientID %>').val(sum);
        }
        function chnagedbtach(s) {

            $('#<%=hdnoldbatchno.ClientID %>').val(s.GetText());
            $('#<%=hidencountforserial.ClientID %>').val(1);

            var sum = $('#hdnbatchchanged').val();
            sum = Number(Number(sum) + Number(1));

            $('#<%=hdnbatchchanged.ClientID %>').val(sum);

            ctxtexpirdate.SetText("");
            ctxtmkgdate.SetText("");
        }

        function CmbWarehouse_ValueChange(s) {

            var ISupdate = $('#hdnisoldupdate').val();
            var isnewupdate = $('#hdnisnewupdate').val();

            $('#<%=hdnoldwarehousname.ClientID %>').val(s.GetText());

            if (ISupdate == "true" || isnewupdate == "true") {


            } else {


                ctxtserial.SetValue("");


                ctxtbatch.SetEnabled(true);
                ctxtexpirdate.SetEnabled(true);
                ctxtmkgdate.SetEnabled(true);


            }


        }

        function Clraear() {
            ctxtbatch.SetValue("");

            ASPx.CalClearClick('txtmkgdate_DDD_C');
            ASPx.CalClearClick('txtexpirdate_DDD_C');
            $('#<%=hdnisoldupdate.ClientID %>').val("false");

            ctxtserial.SetValue("");
            ctxtqnty.SetValue("0.0000");
            ctxtbatchqnty.SetValue("0.0000");
            $('#<%=hdntotalqnty.ClientID %>').val(0);
            $('#<%=hidencountforserial.ClientID %>').val(1);
            $('#<%=hdnbatchchanged.ClientID %>').val("0");
            var strProductID = $('#hdfProductID').val();
            var stockids = $('#hdfstockid').val();
            var branchid = $('#hdbranchID').val();
            var strProductName = $('#lblProductName').text();
            $('#<%=hdnisnewupdate.ClientID %>').val("false");
            ctxtbatch.SetEnabled(true);
            ctxtexpirdate.SetEnabled(true);
            ctxtmkgdate.SetEnabled(true);
            ctxtbatch.SetEnabled(true);
            cCmbWarehouse.SetEnabled(true);
            $('#<%=hdnisviewqntityhas.ClientID %>').val("false");
            $('#<%=hdnisolddeleted.ClientID %>').val("false");
            ctxtqnty.SetEnabled(true);

            var existingqntity = $('#hdfopeningstock').val();
            var totaldeleteqnt = $('#hdndeleteqnity').val();

            var addqntity = Number(existingqntity) + Number(totaldeleteqnt);

            $('#<%=hdndeleteqnity.ClientID %>').val(0);
           <%-- $('#<%=hdfopeningstock.ClientID %>').val(addqntity);--%>



            cGrdWarehouse.PerformCallback('checkdataexist~' + strProductID + '~' + stockids + '~' + branchid + '~' + strProductName);

        }

        function SaveWarehouse() {

            var WarehouseID = cCmbWarehouse.GetValue();
            var WarehouseName = cCmbWarehouse.GetText();

            var qnty = ctxtqnty.GetText();
            var IsSerial = $('#hdnisserial').val();
            //alert(qnty);

            if (qnty == "0.0000") {
                qnty = ctxtbatchqnty.GetText();
            }

            if (Number(qnty) % 1 != 0 && IsSerial == "true") {
                jAlert("Serial number is activated, Quantity should not contain decimals. ");
                return;
            }

            //alert(qnty);
            var BatchName = ctxtbatch.GetText();
            var SerialName = ctxtserial.GetText();
            var Isbatch = $('#hdnisbatch').val();

            var enterdqntity = $('#hdfopeningstock').val();

            var hdniswarehouse = $('#hdniswarehouse').val();

            var ISupdate = $('#hdnisoldupdate').val();
            var isnewupdate = $('#hdnisnewupdate').val();
            //alert(Isbatch + "/" + IsSerial);
            //alert(hdniswarehouse+"/"+WarehouseID);
            if (Isbatch == "true" && hdniswarehouse == "false") {
                qnty = ctxtbatchqnty.GetText();
            }

            if (ISupdate == "true") {

                if (hdniswarehouse == "true" && WarehouseID == null) {

                    $("#RequiredFieldValidatorCmbWarehouse").css("display", "block");
                }
                else {
                    $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
                }
                if (qnty == "0.0") {

                    if (Isbatch != "false" || hdniswarehouse != "false") {
                        $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                        $("#RequiredFieldValidatortxtwareqntity").css("display", "block");
                        //jAlert("Quantity should not be 0.0");
                    } else if (Isbatch == "false" && hdniswarehouse == "false" && IsSerial == "true") {
                        qnty = "0.00"
                        $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                        $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
                    }
                } else {

                    qnty = "0.00"
                    $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                    $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
                }

                if (Isbatch == "true" && BatchName == "") {

                    $("#RequiredFieldValidatortxtbatch").css("display", "block");
                    ctxtbatch.Focus();
                } else {
                    $("#RequiredFieldValidatortxtbatch").css("display", "none");
                }
                if (IsSerial == "true" && SerialName == "") {
                    $("#RequiredFieldValidatortxtserial").css("display", "block");
                    ctxtserial.Focus();

                } else {
                    $("#RequiredFieldValidatortxtserial").css("display", "none");
                }
                var slno = $('#hdncurrentslno').val();



                if (slno != "") {

                    cGrdWarehouse.PerformCallback('Updatedata~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + SerialName + '~' + slno + '~' + qnty);

                    $('#<%=hdnisoldupdate.ClientID %>').val("false");
                    ctxtqnty.SetText("0.0");
                    ctxtbatch.SetText("");
                    ctxtbatch.SetEnabled(true);
                    cCmbWarehouse.SetEnabled(true);
                    ctxtqnty.SetEnabled(true);
                    return false;
                }


            } else if (isnewupdate == "true") {
                if (hdniswarehouse == "true" && WarehouseID == null) {

                    $("#RequiredFieldValidatorCmbWarehouse").css("display", "block");
                }
                else {
                    $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
                }
                if (qnty == "0.0") {

                    if (Isbatch != "false" || hdniswarehouse != "false") {
                        $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                        $("#RequiredFieldValidatortxtwareqntity").css("display", "block");
                        //jAlert("Quantity should not be 0.0");
                    } else if (Isbatch == "false" && hdniswarehouse == "false" && IsSerial == "true") {
                        qnty = "0.00"
                        $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                        $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
                    }
                } else {

                    qnty = "0.00"
                    $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                    $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
                }

                if (Isbatch == "true" && BatchName == "") {

                    $("#RequiredFieldValidatortxtbatch").css("display", "block");
                    ctxtbatch.Focus();
                }
                else {
                    $("#RequiredFieldValidatortxtbatch").css("display", "none");
                }
                if (IsSerial == "true" && SerialName == "") {


                    $("#RequiredFieldValidatortxtserial").css("display", "block");
                    ctxtserial.Focus();

                } else {
                    $("#RequiredFieldValidatortxtserial").css("display", "none");
                }
                var slno = $('#hdncurrentslno').val();

                if (slno != "") {

                    cGrdWarehouse.PerformCallback('NewUpdatedata~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + SerialName + '~' + slno + '~' + qnty);

                    $('#<%=hdnisviewqntityhas.ClientID %>').val("false");
                    $('#<%=hdnisnewupdate.ClientID %>').val("false");
                    ctxtbatch.SetEnabled(true);
                    cCmbWarehouse.SetEnabled(true);
                    ctxtqnty.SetEnabled(true);
                    ctxtqnty.SetText("0.0");
                    ctxtbatch.SetText("");
                    return false;
                }

            }
            else {

                var hdnisediteds = $('#hdnisedited').val();

                if (hdniswarehouse == "true" && WarehouseID == null) {

                    $("#RequiredFieldValidatorCmbWarehouse").css("display", "block");

                    return;
                } else {
                    $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
                }
                if (qnty == "0.0") {

                    if (Isbatch != "false" || hdniswarehouse != "false") {
                        $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                        $("#RequiredFieldValidatortxtwareqntity").css("display", "block");
                        //jAlert("Quantity should not be 0.0");
                    } else if (Isbatch == "false" && hdniswarehouse == "false" && IsSerial == "true") {
                        qnty = "0.00"
                        $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                        $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
                    }
                } else {

                    qnty = "0.00"
                    $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                    $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
                }
                if (Isbatch == "true" && BatchName == "") {

                    $("#RequiredFieldValidatortxtbatch").css("display", "block");
                    ctxtbatch.Focus();
                    return;

                } else {
                    $("#RequiredFieldValidatortxtbatch").css("display", "none");
                }
                if (IsSerial == "true" && SerialName == "") {


                    $("#RequiredFieldValidatortxtserial").css("display", "block");
                    ctxtserial.Focus();
                    return;

                } else {
                    $("#RequiredFieldValidatortxtserial").css("display", "none");
                }
                if (Isbatch == "true" && hdniswarehouse == "false") {

                    qnty = ctxtbatchqnty.GetText();

                    if (qnty == "0.0000") {
                        //alert("Enter" + ctxtbatchqnty.GetText());

                        ctxtbatchqnty.Focus();
                    }
                }

                if (qnty == "0.0") {

                    if (Isbatch != "false" || hdniswarehouse != "false") {
                        $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                        $("#RequiredFieldValidatortxtwareqntity").css("display", "block");
                        //jAlert("Quantity should not be 0.0");
                    } else if (Isbatch == "false" && hdniswarehouse == "false" && IsSerial == "true") {
                        qnty = "0.00"
                        $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                        $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
                    }
                }
                else if (((hdniswarehouse == "true" && WarehouseID != null) || hdniswarehouse == "false") && ((Isbatch == "true" && BatchName != "") || Isbatch == "false") && ((IsSerial == "true" && SerialName != "") || IsSerial == "false") && qnty != "0.0") {
                    $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
                    $("#RequiredFieldValidatortxtbatch").css("display", "none");
                    $("#RequiredFieldValidatortxtserial").css("display", "none");

                    $("#RequiredFieldValidatortxtwareqntity").removeAttr("style");
                    $("#RequiredFieldValidatortxtbatchqntity").removeAttr("style");
                    $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                    $("#RequiredFieldValidatortxtwareqntity").css("display", "none");


                    if (Isbatch == "true" && hdniswarehouse == "false") {

                        qnty = ctxtbatchqnty.GetText();

                        if (qnty = "0.0000") {
                            ctxtbatchqnty.Focus();
                        }
                    }


                    var oldenterqntity = $('#hdnenterdopenqnty').val();
                    var enterdqntityss = $('#hdnnewenterqntity').val();
                    var deletedquantity = $('#hdndeleteqnity').val();
                    //alert(deletedquantity);


                    if (Number(qnty) > (Number(enterdqntity) + Number(deletedquantity)) && hdnisediteds == "false") {
                        qnty = "0.00";
                        jAlert("You have entered Quantity greater than Opening Quantity. Cannot Proceed.");


                    }
                    else {


                        cGrdWarehouse.PerformCallback('Display~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + SerialName + '~' + qnty);



                        cCmbWarehouse.Focus();
                    }
                }

                return false;
            }
    }
    function SaveWarehouseAll() {

        cGrdWarehouse.PerformCallback('Saveall~');

    }


    function cGrdWarehouseShowError(obj) {

        if (cGrdWarehouse.cpdeletedata != null) {
            var existingqntity = $('#hdfopeningstock').val();
            var totaldeleteqnt = $('#hdndeleteqnity').val();

            var addqntity = Number(cGrdWarehouse.cpdeletedata) + Number(existingqntity);
            var adddeleteqnty = Number(cGrdWarehouse.cpdeletedata) + Number(totaldeleteqnt);

            $('#<%=hdndeleteqnity.ClientID %>').val(adddeleteqnty);
            <%--$('#<%=hdfopeningstock.ClientID %>').val(addqntity);--%>
            cGrdWarehouse.cpdeletedata = null;
        }

        if (cGrdWarehouse.cpdeletedatasubsequent != null) {
            jAlert(cGrdWarehouse.cpdeletedatasubsequent);
            cGrdWarehouse.cpdeletedatasubsequent = null;
        }
        if (cGrdWarehouse.cpbatchinsertmssg != null) {
            ctxtbatch.SetText("");

            ctxtqnty.SetValue("0.0000");
            ctxtbatchqnty.SetValue("0.0000");
            cGrdWarehouse.cpbatchinsertmssg = null;
        }
        if (cGrdWarehouse.cpupdateexistingdata != null) {

            $('#<%=hdnisedited.ClientID %>').val("true");
            cGrdWarehouse.cpupdateexistingdata = null;
        }
        if (cGrdWarehouse.cpupdatenewdata != null) {

            $('#<%=hdnisedited.ClientID %>').val("true");

            cGrdWarehouse.cpupdateexistingdata = null;
        }

        if (cGrdWarehouse.cpupdatemssgserialsetdisblebatch != null) {
            ctxtbatch.SetEnabled(false);
            ctxtexpirdate.SetEnabled(false);
            ctxtmkgdate.SetEnabled(false);
            cGrdWarehouse.cpupdatemssgserialsetdisblebatch = null;
        }
        if (cGrdWarehouse.cpupdatemssgserialsetenablebatch != null) {
            ctxtbatch.SetEnabled(true);
            ctxtexpirdate.SetEnabled(true);
            ctxtmkgdate.SetEnabled(true);
            $('#<%=hidencountforserial.ClientID %>').val(1);

            $('#<%=hdnbatchchanged.ClientID %>').val("0");
            $('#<%=hidencountforserial.ClientID %>').val("1");
            ctxtqnty.SetValue("0.0000");
            ctxtbatchqnty.SetValue("0.0000");
            ctxtbatch.SetText("");
            cGrdWarehouse.cpupdatemssgserialsetenablebatch = null;
        }


        if (cGrdWarehouse.cpproductname != null) {
            document.getElementById('<%=lblProductName.ClientID %>').innerHTML = cGrdWarehouse.cpproductname;
            cGrdWarehouse.cpproductname = null;
        }



        if (cGrdWarehouse.cpupdatemssg != null) {
            if (cGrdWarehouse.cpupdatemssg == "Saved Successfully.") {
                $('#<%=hdntotalqnty.ClientID %>').val("0");
                $('#<%=hdnbatchchanged.ClientID %>').val("0");
                $('#<%=hidencountforserial.ClientID %>').val("1");
                ctxtqnty.SetValue("0.0000");
                ctxtbatchqnty.SetValue("0.0000");

                parent.cPopup_Warehouse.Hide();
                var hdnselectedbranch = $('#hdnselectedbranch').val();

                //cOpeningGrid.Enable = false;
                parent.cOpeningGrid.PerformCallback("branchwise~" + hdnselectedbranch);
            }
            jAlert(cGrdWarehouse.cpupdatemssg);
            cGrdWarehouse.cpupdatemssg = null;


        }
        if (cGrdWarehouse.cpupdatemssgserial != null) {
            jAlert(cGrdWarehouse.cpupdatemssgserial);
            cGrdWarehouse.cpupdatemssgserial = null;
        }

        if (cGrdWarehouse.cpinsertmssg != null) {
            $('#<%=hidencountforserial.ClientID %>').val(2);
            ctxtserial.SetValue("");
            ctxtserial.Focus();
            cGrdWarehouse.cpinsertmssg = null;
        }
        if (cGrdWarehouse.cpinsertmssgserial != null) {

            ctxtserial.SetValue("");
            ctxtserial.Focus();
            cGrdWarehouse.cpinsertmssgserial = null;
        }


    }
    </script>

    <asp:HiddenField ID="hdfProductID" runat="server" />
    <asp:HiddenField ID="hdfstockid" runat="server" />
    <asp:HiddenField ID="hdfopeningstock" runat="server" />
    <asp:HiddenField ID="hdbranchID" runat="server" />
    <asp:HiddenField ID="hdnselectedbranch" runat="server" Value="0" />

    <asp:HiddenField ID="hdniswarehouse" runat="server" />
    <asp:HiddenField ID="hdnisbatch" runat="server" />
    <asp:HiddenField ID="hdnisserial" runat="server" />
    <asp:HiddenField ID="hdndefaultID" runat="server" />

    <asp:HiddenField ID="hdnoldrowcount" runat="server" Value="0" />

    <asp:HiddenField ID="hdntotalqnty" runat="server" Value="0" />

    <asp:HiddenField ID="hdnoldwarehousname" runat="server" />
    <asp:HiddenField ID="hdnoldbatchno" runat="server" />
    <asp:HiddenField ID="hidencountforserial" runat="server" />
    <asp:HiddenField ID="hdnbatchchanged" runat="server" Value="0" />

    <asp:HiddenField ID="hdnrate" runat="server" Value="0" />
    <asp:HiddenField ID="hdnvalue" runat="server" Value="0" />

    <asp:HiddenField ID="oldhdnoldwarehousname" runat="server" Value="0" />

    <asp:HiddenField ID="oldhidencountforserial" runat="server" Value="0" />
    <asp:HiddenField ID="oldhdnbatchchanged" runat="server" Value="0" />
    <asp:HiddenField ID="hdnstrUOM" runat="server" />
    <asp:HiddenField ID="hdnenterdopenqnty" runat="server" />
    <asp:HiddenField ID="hdnnewenterqntity" runat="server" />

    <asp:HiddenField ID="hdnisoldupdate" runat="server" />
    <asp:HiddenField ID="hdncurrentslno" runat="server" />
    <asp:HiddenField ID="oldopeningqntity" runat="server" Value="0" />
    <asp:HiddenField ID="hdnisedited" runat="server" />

    <asp:HiddenField ID="hdnisnewupdate" runat="server" />

    <asp:HiddenField ID="hdnisviewqntityhas" runat="server" />
    <asp:HiddenField ID="hdndeleteqnity" runat="server" Value="0" />
    <asp:HiddenField ID="hdnisolddeleted" runat="server" Value="false" />

    <asp:HiddenField ID="hdnisreduing" runat="server" Value="false" />
    <asp:HiddenField ID="hdnoutstock" runat="server" Value="0" />
    <%-- Popup End here--%>
</asp:Content>

