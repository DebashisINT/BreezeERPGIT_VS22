<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="eInvoice-configuration.aspx.cs" Inherits="ERP.OMS.Management.Master.eInvoice_configuration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://cdn3.devexpress.com/jslib/20.1.6/css/dx.common.css" />
    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/20.1.6/css/dx.light.compact.css" />

    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.2/jszip.min.js"></script>
    <script src="https://cdn3.devexpress.com/jslib/20.1.6/js/dx.all.js"></script>
    <link href="/assests/pluggins/Transfer/icon_font/css/icon_font.css" rel="stylesheet" />
    <link href="/assests/pluggins/Transfer/css/jquery.transfer.css" rel="stylesheet" />
    <script src="/assests/pluggins/Transfer/jquery.transfer.js"></script>
    <script type="text/javascript">

        function hideotherstatus() {
            jAlert("E-Invoice feature is not activated. Talk to BreezeERP support team. Thanks.", 'Alert', function () {
                window.location.href = '../ProjectMainPage.aspx';
            });
           
        }

        function validGstin(VALUES) {
            var GSTIN1 = VALUES.trim();

            var returnval = true;

            if (GSTIN1.length == 0) {
            }
            else {
                if (GSTIN1.length != 2) {
                    // $('#invalidGst').css({ 'display': 'block' });
                    returnval = false;
                }


                var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
                var code = /([C,P,H,F,A,T,B,L,J,G])/;
                var code_chk = GSTIN1.substring(3, 4);
                if (GSTIN1.search(panPat) == -1) {
                    returnval = false;
                }
                if (code.test(code_chk) == false) {
                    returnval = false;
                }
            }

            return returnval;

        }

        var selecteduser = [];


        function updateGridByDate() {
            if (cFormDate.GetDate() == null) {
                jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
            }
            else if (ctoDate.GetDate() == null) {
                jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
            }
            else if (ccmbBranchfilter.GetValue() == null) {
                jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
            }
            else {

                //cGrdQuotation.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
                $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                $("#hfBranchID").val(ccmbBranchfilter.GetValue());
                $("#hfIsFilter").val("Y");
                cGrdQuotation.Refresh();

            }
        }

        function grdEndcallback(s, e) {
            if (s.cpJson == "Yes") {
                $.ajax({
                    type: "POST",
                    url: "einvoice.aspx/generateMultiEinvoiceJSON",
                    //data: "{'ProductName':'" + ProductName + "'}",
                    //data: JSON.stringify(otherdet),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        console.log(msg.d);
                        downloadObjectAsJson(msg.d, "Invoice_JSON");
                        var json = JSON.stringify(msg.d);
                        console.log(json);
                        // WriteToFile(json);
                    }
                });
                s.cpJson = null;
            }
        }

        function DoownLoadJson(id) {
            var otherdet = {};
            otherdet.id = id;
            $.ajax({
                type: "POST",
                url: "einvoice.aspx/generateEinvoiceJSON",
                //data: "{'ProductName':'" + ProductName + "'}",
                data: JSON.stringify(otherdet),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    console.log(msg.d);
                    downloadObjectAsJson(msg.d, id);
                    var json = JSON.stringify(msg.d);
                    console.log(json);
                    //  WriteToFile(json);
                }
            });

        }


        function gridcrmCampaignclick(s, e) {
            //alert('hi');
            //IconChange();
            $('#gridcrmCampaign').find('tr').removeClass('rowActive');
            $('.floatedBtnArea').removeClass('insideGrid');
            //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
            $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).addClass('rowActive');
            setTimeout(function () {
                //alert('delay');
                var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
                //    setTimeout(function () {
                //        $(this).fadeIn();
                //    }, 100);
                //});    
                $.each(lists, function (index, value) {
                    //console.log(index);
                    //console.log(value);
                    setTimeout(function () {
                        console.log(value);
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }

        function downloadObjectAsJson(exportObj, exportName) {
            var dataStr = "data:text/json;charset=utf-8," + encodeURIComponent(JSON.stringify(exportObj, 0, 4));
            var downloadAnchorNode = document.createElement('a');
            downloadAnchorNode.setAttribute("href", dataStr);
            downloadAnchorNode.setAttribute("download", exportName + ".json");
            document.body.appendChild(downloadAnchorNode); // required for firefox
            downloadAnchorNode.click();
            downloadAnchorNode.remove();
        }

        function UploadExcel() {



            var formData = new FormData();
            formData.append('file', $('#flEcel')[0].files[0]);
            $.ajax({
                type: 'post',
                url: 'fileUploader.ashx',
                data: formData,
                success: function (status) {
                    if (status != 'error') {

                    }
                },
                processData: false,
                contentType: false,
                error: function (msg) {
                    alert("Whoops something went wrong!");
                }
            });
        }


        $(document).ready(function () {

            $("#trCompany").find("input:not('#chkCompany')").attr("disabled", true)
            $("#trCompany").find("select").attr("disabled", true)
            $("#trCompany").find("button").attr("disabled", true)
            $("#trBranch").find("input:not('#chkBranch')").attr("disabled", true)
            $("#trBranch").find("select").attr("disabled", true)
            $("#trBranch").find("button").attr("disabled", true)

            if ($("#DefaultEinvoiceSellerAddress").val() == "Yes") {
                $("#trCompany").find("input:not('#chkCompany')").attr("disabled", false)
                $("#trCompany").find("select").attr("disabled", false);
                $("#trCompany").find("button").attr("disabled", true);
                $("#trBranch").find("input").attr("disabled", true);
                $("#trBranch").find("select").attr("disabled", true);
                $("#trBranch").find("button").attr("disabled", true);
                $('#chkCompany').prop('checked', true);
                chkCompany_Change();

            }
            else {
                $("#trCompany").find("input").attr("disabled", true);
                $("#trCompany").find("select").attr("disabled", true);
                $("#trCompany").find("button").attr("disabled", true)
                $("#trBranch").find("input:not('#chkBranch')").attr("disabled", false);
                $("#trBranch").find("select").attr("disabled", false);
                $("#trBranch").find("button").attr("disabled", true);
                $('#chkBranch').prop('checked', true);
                chkBranch_Change();
            }




            $('#chkCompany').change(function () {
                if (this.checked) {
                    $("#trBranch").find("input:not('#chkBranch')").attr("disabled", true)
                    $("#trBranch").find("select").attr("disabled", true)
                    $("#trBranch").find("button").attr("disabled", true)
                    $("#trCompany").find("input:not('#chkBranch')").attr("disabled", false)
                    $("#trCompany").find("select").attr("disabled", false)
                    //$("#trCompany").find("button").attr("disabled", false)
                    $('#chkBranch').prop('checked', false);
                    chkCompany_Change();
                }
                //  $('#textbox1').val(this.checked);        
            });

            $('#chkBranch').change(function () {
                if (this.checked) {
                    $("#trCompany").find("input:not('#chkCompany')").attr("disabled", true)
                    $("#trCompany").find("select").attr("disabled", true);
                    $("#trCompany").find("button").attr("disabled", true)
                    $("#trBranch").find("input:not('#chkCompany')").attr("disabled", false)
                    $("#trBranch").find("select").attr("disabled", false)
                    //$("#trBranch").find("button").attr("disabled", false)
                    $('#chkCompany').prop('checked', false);
                    chkBranch_Change()
                }
            });

            $('#chkeInvoiceCompany').change(function () {
                if (this.checked) {
                    $("#trCompany").find("button").attr("disabled", false);
                    set_GSPonBoarding('Company');
                }
                else {
                    jConfirm('Wish to remove Onboarding & login details?', 'Confirmation Dialog', function (r) {
                        if (r == true) {
                             $("#trCompany").find("button").attr("disabled", true)
                            var companyBranchID = "";
                            var companyBranchID = "";

                            companyBranchType = "Company";
                            companyBranchID = $("#ddlCompany").val();

                            $.ajax({
                                type: "POST",
                                url: "eInvoice-configuration.aspx/DeleteGSPOnBoarding",
                                data: JSON.stringify({ Action: 'DeleteGSPOnBoarding', companyBranchType: companyBranchType, companyBranchID: companyBranchID }),
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                async: false,
                                success: function (msg) {
                                    var list = msg.d;
                                    if (list == "OK") {
                                        jAlert("Remove Successfully");
                                    }

                                    if (msg.d == "Logout") {
                                        location.href = "../../OMS/SignOff.aspx";
                                    }
                                }
                            });
                        }
                        else {
                               $("#chkeInvoiceCompany").prop('checked',true);
                        }
                    });
                }
            });

            $('#chkeInvoiceBranch').change(function () {
                if (this.checked) {
                    $("#trBranch").find("button").attr("disabled", false);
                    set_GSPonBoarding('Branch');
                }
                else {
                    jConfirm('The GPS onboarding records remove?', 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            $("#trBranch").find("button").attr("disabled", true)
                            var companyBranchType = "";
                            var companyBranchID = "";

                            companyBranchType = "Branch";
                            companyBranchID = $("#ddlBranch").val();
                            $.ajax({
                                type: "POST",
                                url: "eInvoice-configuration.aspx/DeleteGSPOnBoarding",
                                data: JSON.stringify({ Action: 'DeleteGSPOnBoarding', companyBranchType: companyBranchType, companyBranchID: companyBranchID }),
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                async: false,
                                success: function (msg) {
                                    var list = msg.d;
                                    if (list == "OK") {
                                        jAlert("Remove Successfully");
                                    }

                                    if (msg.d == "Logout") {
                                        location.href = "../../OMS/SignOff.aspx";
                                    }
                                }
                            });
                        }
                        else {
                               $("#chkeInvoiceBranch").prop('checked',true);
                        }
                    });
                }
            });

            $('#chkAllUser').change(function () {
                if (this.checked) {
                    $("#ddlUserGroup").attr("disabled", true);
                    $("#ddlUserGroup").val(0);
                    $.ajax({
                        type: "POST",
                        url: "eInvoice-configuration.aspx/UserGroupChange",
                        data: JSON.stringify({ UserGroupID: "0" }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {
                            $(".transfer").html("");
                            var list = msg.d;
                            var settings = {
                                dataArray: list,
                                tabNameText: "Available Users",
                                rightTabNameText: "Selected Users",
                                callable: function (items) {
                                    //console.log("item", items)
                                    selecteduser = items;
                                }
                            };
                            var transfer = $(".transfer").transfer(settings);
                        }
                    });
                }
                else {
                    $("#ddlUserGroup").attr("disabled", false);
                    $(".transfer").html("");
                    var list = [];
                    var settings = {
                        dataArray: list,
                        tabNameText: "Available Users",
                        rightTabNameText: "Selected Users",
                        callable: function (items) {
                            //console.log("item", items)
                            selecteduser = [];
                        }
                    };
                    selecteduser = [];
                    var transfer = $(".transfer").transfer(settings);
                }
            });
        });


        function chkCompany_Change() {
            $.ajax({
                type: "POST",
                url: "eInvoice-configuration.aspx/changesbind",
                data: JSON.stringify({ Type: 'Company' }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var list = msg.d;
                    cGrdDevice.PerformCallback();
                    if (msg.d == "Logout") {
                        location.href = "../../OMS/SignOff.aspx";
                    }
                }
            });
        }

        function chkBranch_Change() {

            $.ajax({
                type: "POST",
                url: "eInvoice-configuration.aspx/changesbind",
                data: JSON.stringify({ Type: 'Branch' }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var list = msg.d;
                    cGrdDevice.PerformCallback();
                    if (msg.d == "Logout") {
                        location.href = "../../OMS/SignOff.aspx";
                    }
                }
            });
        }


    </script>
    <style>
        .transfer-double {
            height: auto !important;
        }

        .tabs-left, .tabs-right {
            border-bottom: none;
            padding-top: 2px;
        }

        .tabs-left {
            border-right: 1px solid #ddd;
        }

        .tabs-right {
            border-left: 1px solid #ddd;
        }

            .tabs-left > li, .tabs-right > li {
                float: none;
                margin-bottom: 2px;
            }

        .tabs-left > li {
            margin-right: -1px;
        }

        .tabs-right > li {
            margin-left: -1px;
        }

        .tabs-left > li.active > a,
        .tabs-left > li.active > a:hover,
        .tabs-left > li.active > a:focus {
            border-bottom-color: #ddd;
            border-right-color: transparent;
        }

        .tabs-right > li.active > a,
        .tabs-right > li.active > a:hover,
        .tabs-right > li.active > a:focus {
            border-bottom: 1px solid #ddd;
            border-left-color: transparent;
        }

        .tabs-left > li > a {
            border-radius: 21px 0 0 21px;
            margin-right: 0;
            display: block !important;
        }

        .tabs-right > li > a {
            border-radius: 0 4px 4px 0;
            margin-right: 0;
        }

        .vertical-text {
            margin-top: 50px;
            border: none;
            position: relative;
        }

            .vertical-text > li {
                height: 20px;
                width: 120px;
                margin-bottom: 100px;
            }

                .vertical-text > li > a {
                    border-bottom: 1px solid #ddd;
                    border-right-color: transparent;
                    text-align: center;
                    border-radius: 4px 4px 0px 0px;
                }

                .vertical-text > li.active > a,
                .vertical-text > li.active > a:hover,
                .vertical-text > li.active > a:focus {
                    border-bottom-color: transparent;
                    border-right-color: #ddd;
                    border-left-color: #ddd;
                }

            .vertical-text.tabs-left {
                left: -50px;
            }

            .vertical-text.tabs-right {
                right: -50px;
            }

                .vertical-text.tabs-right > li {
                    -webkit-transform: rotate(90deg);
                    -moz-transform: rotate(90deg);
                    -ms-transform: rotate(90deg);
                    -o-transform: rotate(90deg);
                    transform: rotate(90deg);
                }

            .vertical-text.tabs-left > li {
                -webkit-transform: rotate(-90deg);
                -moz-transform: rotate(-90deg);
                -ms-transform: rotate(-90deg);
                -o-transform: rotate(-90deg);
                transform: rotate(-90deg);
            }

        /*.tabs-left > li > a, .fontPp {
            font-family: Poppins !important;
        }*/

        .tabs-left > li > a {
            background-color: #f1f1f1 !important;
            margin-bottom: 10px;
        }

            .tabs-left > li > a:hover {
                background-color: #efefff !important;
                border-color: #e1e5ff;
            }

        .tabs-left > li.active > a, .tabs-left > li.active > a:hover, .tabs-left > li.active > a:focus {
            background: #432ADB !important;
            color: #fff !important;
            font-size: 14px !important;
            border-color: #432adb;
        }

        .no-gutters {
            padding: 0;
        }

        .ttCont {
            background: #ffffff;
            padding: 10px;
            border-radius: 18px;
            min-height: 300px;
        }

        .holderBox {
            background: #ff5808;
            border-radius: 10px;
            padding: 1px 15px 8px 15px;
            color: #ffffff;
        }

            .holderBox.c1 {
                background: #0f78fb;
            }

            .holderBox.c2 {
                background: #7208ff;
            }

            .holderBox.c3 {
                background: #fb0f87;
            }

        .bDashed-right {
            border-right: 1px dashed;
        }
        /*horizontal tab*/
        .horiTab .nav-tabs>li>a {
            padding: 7px 7px;
            height: auto;
            white-space:nowrap !important;
        }
        .horiTab .nav-tabs>li>a:before {
            display:none;
        }
        .horiTab {
            margin-top: 15px;
        }

            .horiTab .nav-tabs > li > a {
                background: transparent;
                border: none;
            }

                .horiTab .nav-tabs > li > a:hover {
                    border: none;
                    background-color: transparent !important;
                }

            .horiTab .nav-tabs > li.active > a, .horiTab .nav-tabs > li.active > a:hover, .horiTab .nav-tabs > li.active > a:focus {
                border: none;
                background: none !important;
                border-bottom: 3px solid #0f78fb;
                font-size: 14px;
                font-weight: 500;
                color: #565353;
            }

        .dx-header-row {
            background: #545dc1 !important;
            color: #fff;
        }

            .dx-header-row > td {
                border-color: #545dc1 !important;
            }

        .dx-texteditor-input-container input {
            border: transparent;
            margin: 0;
        }

        .dropzone {
            background: white;
            border-radius: 5px;
            border: 2px dashed rgb(0, 135, 247);
            border-image: none;
            max-width: 500px;
            margin-left: auto;
            margin-right: auto;
        }

        .tblVerti {
            margin-left: 25px;
        }

            .tblVerti > tbody > tr > td {
                vertical-align: middle;
                padding-right: 25px;
                padding-bottom: 10px;
            }

        .minSelect {
            min-width: 140px;
        }

        .tblVerti .form-control {
            margin-bottom: 0px !important;
            border-radius: 5px;
            border: 1px solid rgba(0,0,0,0.12);
            min-height: 32px;
        }

        /*Swiitch Slider*/
        .switch {
            position: relative;
            display: inline-block;
            width: 56px;
            height: 28px;
        }

            .switch input {
                opacity: 0;
                width: 0;
                height: 0;
            }

        .slider {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #ccc;
            -webkit-transition: .4s;
            transition: .4s;
        }

            .slider:before {
                position: absolute;
                content: "";
                height: 22px;
                width: 22px;
                left: 4px;
                bottom: 3px;
                background-color: white;
                -webkit-transition: .4s;
                transition: .4s;
            }

        input:checked + .slider {
            background-color: #38af5c;
        }

        input:focus + .slider {
            box-shadow: 0 0 1px #38af5c;
        }

        input:checked + .slider:before {
            -webkit-transform: translateX(26px);
            -ms-transform: translateX(26px);
            transform: translateX(26px);
        }

        /* Rounded sliders */
        .slider.round {
            border-radius: 34px;
        }

            .slider.round:before {
                border-radius: 50%;
            }

        .mainColorTbl {
            width: 100%;
            border: 1px solid #e4e4e4;
        }

            .mainColorTbl > thead > tr > th {
                background: #545dc1 !important;
                color: #fff;
                padding: 5px 10px;
            }

            .mainColorTbl > tbody > tr > td {
                padding: 5px 10px;
            }

            .mainColorTbl > tbody > tr:nth-child(even) > td {
                background: #F2F2F2;
            }
    </style>
    <script>

        $(function () {

        });

         $(document).ready(function () {
            cPanelSalesRoundoff.PerformCallback('BindSalesRoundOffGrid');
            //clookup_SalesRoundoff.gridView.Refresh();

            cPanelSalesDiscount.PerformCallback('BindSalesDiscountGrid');
            //clookup_SalesDiscount.gridView.Refresh();



            cPanelSalesTCS.PerformCallback('BindSalesTCSGrid');
            //clookup_SalesTCS.gridView.Refresh();

            cPanelPurchaseDiscount.PerformCallback('BindPurchaseDiscountGrid');
            // clookup_PurchaseDiscount.gridView.Refresh();

            cPanelPurchaseRoundoff.PerformCallback('BindPurchaseRoundoffGrid');
            //clookup_PurchaseRoundoff.gridView.Refresh();

            cPanelPurchaseTCS.PerformCallback('BindPurchaseTCSGrid');
            // clookup_PurchaseTCS.gridView.Refresh();
         });

        function taxcharges() {
            cPanelSalesRoundoff.PerformCallback('SetSalesRoundOffGrid');
            ////clookup_SalesRoundoff.gridView.Refresh();

            cPanelSalesDiscount.PerformCallback('SetSalesDiscountGrid');
            ////clookup_SalesDiscount.gridView.Refresh();



            cPanelSalesTCS.PerformCallback('SetSalesTCSGrid');
            //clookup_SalesTCS.gridView.Refresh();

            cPanelPurchaseDiscount.PerformCallback('SetPurchaseDiscountGrid');
            // clookup_PurchaseDiscount.gridView.Refresh();

            cPanelPurchaseRoundoff.PerformCallback('SetPurchaseRoundoffGrid');
            //clookup_PurchaseRoundoff.gridView.Refresh();

            cPanelPurchaseTCS.PerformCallback('SetPurchaseTCSGrid');
            // clookup_PurchaseTCS.gridView.Refresh();
        }



        function Save_TaxCharges() {

            //var SalesDiscount = clookup_SalesDiscount.GetGridView().GetRowKey(clookup_SalesDiscount.GetGridView().GetFocusedRowIndex());
            //var SalesRoundOff = clookup_SalesRoundOff.GetGridView().GetRowKey(clookup_SalesRoundOff.GetGridView().GetFocusedRowIndex());
            //var SalesTCS = clookup_SalesTCS.GetGridView().GetRowKey(clookup_SalesTCS.GetGridView().GetFocusedRowIndex());

            //var purchaseDiscount = clookup_PurchaseDiscount.GetGridView().GetRowKey(clookup_PurchaseDiscount.GetGridView().GetFocusedRowIndex());
            //var purchaseRoundOff = clookup_PurchaseRoundoff.GetGridView().GetRowKey(clookup_PurchaseRoundoff.GetGridView().GetFocusedRowIndex());
            //var purchaseTCS = clookup_PurchaseTCS.GetGridView().GetRowKey(clookup_PurchaseTCS.GetGridView().GetFocusedRowIndex());

            var SalesDiscount = clookup_SalesDiscount.GetGridView().GetSelectedKeysOnPage();
            var SalesRoundOff = clookup_SalesRoundOff.GetGridView().GetSelectedKeysOnPage();
            var SalesTCS = clookup_SalesTCS.GetGridView().GetSelectedKeysOnPage();

            var purchaseDiscount = clookup_PurchaseDiscount.GetGridView().GetSelectedKeysOnPage();
            var purchaseRoundOff = clookup_PurchaseRoundoff.GetGridView().GetSelectedKeysOnPage();
            var purchaseTCS = clookup_PurchaseTCS.GetGridView().GetSelectedKeysOnPage();

            

            if (SalesDiscount.length>0 && SalesRoundOff.length>0) {
            if (SalesDiscount.every(val => SalesRoundOff.includes(val))) {
                jAlert("Sales Round off and Discount selected Item same.");
                return
            }
            }

            if (SalesDiscount.length>0 && SalesTCS.length>0) {
            //if (SalesDiscount == SalesTCS) {
             if (SalesDiscount.every(val => SalesTCS.includes(val))) {
                jAlert("Sales Discount and TCS selected Item same.");
                return
                }
            }

            if (SalesRoundOff.length>0 && SalesTCS.length>0) {
                // if (SalesRoundOff == SalesTCS) {
                if (SalesRoundOff.every(val => SalesTCS.includes(val))) {
                jAlert("Sales Round off and TCS selected Item same.");
                return
                 }
            }
            
             if (purchaseDiscount.length>0 && purchaseRoundOff.length>0) {
            //if (purchaseDiscount == purchaseRoundOff) {
            if (purchaseDiscount.every(val => purchaseRoundOff.includes(val))) {
                jAlert("Purchase Round off and Discount selected Item same.");
                return
            }
            }

             if (purchaseDiscount.length>0 && purchaseTCS.length>0) {
            //if (purchaseDiscount == purchaseTCS) {
            if (purchaseDiscount.every(val => purchaseTCS.includes(val))) {
                jAlert("Purchase Discount and TCS selected Item same.");
                return
            }
            }

             if (purchaseRoundOff.length>0 && purchaseTCS.length>0) {
            //if (purchaseRoundOff == purchaseTCS) {
            if (purchaseRoundOff.every(val => purchaseTCS.includes(val))) {
                jAlert("Purchase Round off and TCS selected Item same.");
                return
            }
            }

            $.ajax({
                type: "POST",
                url: "eInvoice-configuration.aspx/SaveTaxCharges",
                data: JSON.stringify({
                    SalesDiscount: SalesDiscount, SalesRoundOff: SalesRoundOff, SalesTCS: SalesTCS, purchaseDiscount: purchaseDiscount,
                    purchaseRoundOff: purchaseRoundOff, purchaseTCS: purchaseTCS
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var list = msg.d;
                    if (list == "OK") {
                        jAlert("Save Successfully.");
                    }
                    if (msg.d == "Logout") {
                        location.href = "../../OMS/SignOff.aspx";
                    }
                }
            });
        }

        function ddlUserGroup_Change() {
            $.ajax({
                type: "POST",
                url: "eInvoice-configuration.aspx/UserGroupChange",
                data: JSON.stringify({ UserGroupID: $("#ddlUserGroup").val() }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    $(".transfer").html("");
                    var list = msg.d;
                    var settings = {
                        dataArray: list,
                        tabNameText: "Available Users",
                        rightTabNameText: "Selected Users",
                        callable: function (items) {
                            // your code
                            // console.log("item", items)
                            selecteduser = items;
                        }
                    };
                    var transfer = $(".transfer").transfer(settings);
                }
            });
        }


        function Save_UserGroup() {
            if (selecteduser.length <= 0) {
                jAlert("Please select user");
                return
            }


            var UserGroup = "0";//$("#ddlUserGroup").val();

            if ($('#chkCompany').is(':checked')) {
                UserGroup = "0";
            }
            else {
               UserGroup = $("#ddlUserGroup").val();
            }

            $.ajax({
                type: "POST",
                url: "eInvoice-configuration.aspx/SaveGroupUser",
                data: JSON.stringify({ selecteduser: selecteduser, User_Group: UserGroup }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var list = msg.d;
                    if (list == "OK") {
                        jAlert("Save Successfully.");
                    }
                    if (msg.d == "Logout") {
                        location.href = "../../OMS/SignOff.aspx";
                    }
                }
            });
        }




        function eInvoiceActivation_Update() {


            var companyBranchID = "";
            var GSTIN = "";
            var eInvoice = "";
            if ($('#chkCompany').is(':checked')) {
                companyBranchID = $("#ddlCompany").val();
                GSTIN = $("#txtCompanyGSTIN").val();
                if ($('#chkeInvoiceCompany').is(':checked')) {
                    eInvoice = "Yes";
                }
                else {
                    eInvoice = "No"
                }
            }

            if ($('#chkBranch').is(':checked')) {
                companyBranchID = $("#ddlBranch").val();
                GSTIN = $("#txtBranchGTSTIN").val();
                if ($('#chkeInvoiceBranch').is(':checked')) {
                    eInvoice = "Yes";
                }
                else {
                    eInvoice = "No"
                }
            }


            var gsp = $("#ddlGSP").val();
            var MobileNo = $("#txtMobileNo").val();
            var Email = $("#txtEmail").val();
            var Password = $("#txtPassword").val();
               var ApiType = "";
            if ($('#rdbSandbox').is(':checked')) { ApiType = "Sandbox"; }

            if (gsp == "0") {
                jAlert("Please select GSP.");
                $("#ddlGSP").focus();
                return
            }

            if (MobileNo == "") {
                jAlert("Please enter Mobile No.");
                $("#txtMobileNo").focus();
                return
            }

            if (Email == "") {
                jAlert("Please enter Email.");
                $("#txtEmail").focus();
                return
            }

            if (Password == "") {
                jAlert("Please enter password.");
                $("#txtPassword").focus();
                return
            }

            if (ApiType != "Sandbox") {
                if (!validateEmail(Email)) {
                    return;
                }
            }

            //if (!validGstin(GSTIN)) {
            //    jAlert("Invalid GSTIN");
            //    return;
            //}
            Save_Record();


            $.ajax({
                type: "POST",
                url: "eInvoice-configuration.aspx/UpdateeInvoiceActivation",
                data: JSON.stringify({ companyBranchID: companyBranchID, GSTIN: GSTIN, eInvoice: eInvoice }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var list = msg.d;
                    if (list == "OK") {
                        cGrdDevice.Refresh();
                        $("#ModalOnboarding").modal('hide');
                    }
                    if (msg.d == "Logout") {
                        location.href = "../../OMS/SignOff.aspx";
                    }
                }
            });
        }

        function Save_eInvoiceActivation() {
            $.ajax({
                type: "POST",
                url: "eInvoice-configuration.aspx/SaveEInvoiceActivation",
                data: JSON.stringify({ Action: 'Insert' }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var list = msg.d;
                    if (list == "OK") {
                        jAlert("Save Successfully");
                    }
                    else if (list == "GridBlank") {
                        jAlert("Please add Company/Branch.");
                    }
                    if (msg.d == "Logout") {
                        location.href = "../../OMS/SignOff.aspx";
                    }
                }
            });
        }

        function validateEmail(emailField) {
            //var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;

            //if (reg.test(emailField.value) == false) {
            //    jAlert('Invalid Email Address');
            //    return false;
            //}

            const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            if (!re.test(String(emailField).toLowerCase())) {
                jAlert('Invalid Email Address');
                return false;
            }

            return true;

        }

        function set_GSPonBoarding(VALUES) {

            var companyBranchID = "";
            var companyBranchType = "";
            if (VALUES == "Company") {
                companyBranchID = $("#ddlCompany").val();
                companyBranchType = "Company";
            }
            else if (VALUES == "Branch") {
                companyBranchID = $("#ddlBranch").val();
                companyBranchType = "Branch";
            }

            $.ajax({
                type: "POST",
                url: "eInvoice-configuration.aspx/GSPOnBoardingSetValues",
                data: JSON.stringify({ Action: 'GSPOnBoardingSet', companyBranchType: companyBranchType, companyBranchID: companyBranchID }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var list = msg.d;
                    if (list.length > 0) {
                        $("#ddlGSP").val(msg.d[0].GSP_CODE);
                        $("#txtFirsName").val(msg.d[0].FIRST_NAME);
                        $("#txtLastName").val(msg.d[0].LAST_NAME);
                        $("#txtMobileNo").val(msg.d[0].CONTACT_NO);
                        $("#txtEmail").val(msg.d[0].EMAIL);
                        $("#txtPassword").val(msg.d[0].PASSWORD);
                        $("#txtOTP").val(msg.d[0].OTP);
                        var ApiType = msg.d[0].ApiType;
                        if (ApiType == "Sandbox") {
                            //$('#rdbSandbox').is(':checked')
                            document.getElementById("rdbSandbox").checked = true;
                        } else if (ApiType == "Production") {
                            //$('#rdbProduction').is(':checked')
                            document.getElementById("rdbProduction").checked = true;
                        }
                       
                    }
                    else {
                        $("#ddlGSP").val(0);
                        $("#txtFirsName").val('');
                        $("#txtLastName").val('');
                        $("#txtMobileNo").val('');
                        $("#txtEmail").val('');
                        $("#txtPassword").val('');
                        $("#txtOTP").val('');
                        document.getElementById("rdbSandbox").checked = true;
                    }
                     $("#ModalOnboarding").modal('toggle');
                    if (msg.d == "Logout") {
                        location.href = "../../OMS/SignOff.aspx";
                    }
                }
            });
        }

        function Save_Record() {

            var companyBranchType = "";
            var companyBranchID = "";
            var GSTIN = "";
            var eInvoice = "";
            if ($('#chkCompany').is(':checked')) {
                companyBranchType = "Company";
                companyBranchID = $("#ddlCompany").val();
                GSTIN = $("#txtCompanyGSTIN").val();
                if ($('#chkeInvoiceCompany').is(':checked')) {
                    eInvoice = "1";
                }
                else {
                    eInvoice = "0"
                }
            }
            else if ($('#chkBranch').is(':checked')) {
                companyBranchType = "Branch";
                companyBranchID = $("#ddlBranch").val();
                GSTIN = $("#txtBranchGTSTIN").val();
                if ($('#chkeInvoiceBranch').is(':checked')) {
                    eInvoice = "1";
                }
                else {
                    eInvoice = "0"
                }
            }


            var gsp = $("#ddlGSP").val();
            var FirsName = $("#txtFirsName").val();
            var LastName = $("#txtLastName").val();
            var MobileNo = $("#txtMobileNo").val();
            var Email = $("#txtEmail").val();
            var Password = $("#txtPassword").val();
            var onfirmPassword = "";
            var OTP = "";
            var Sandbox = $("#rdbSandbox").val();
            var Production = $("#rdbProduction").val();
            var ApiType = "";
            if ($('#rdbSandbox').is(':checked')) { ApiType = "Sandbox"; }
            if ($('#rdbProduction').is(':checked')) { ApiType = "Production" }

            //if (FirsName == "") {
            //    jAlert("Please enter first name.");
            //    $("#txtFirsName").focus();
            //    return
            //}

            //if (MobileNo == "") {
            //    jAlert("Please enter Mobile No.");
            //    $("#txtMobileNo").focus();
            //    return
            //}

            //if (Email == "") {
            //    jAlert("Please enter Email.");
            //    $("#txtEmail").focus();
            //    return
            //}
            //if (ApiType != "Sandbox") {
            //    if (!validateEmail(Email)) {
            //        return;
            //    }
            //}


            //if (Password == "") {
            //    jAlert("Please enter password.");
            //    $("#txtPassword").focus();
            //    return
            //}

            //if (onfirmPassword == "") {
            //    jAlert("Please enter confirm password.");
            //    $("#txtConfirmPassword").focus();
            //    return
            //}

            //if (Password != onfirmPassword) {
            //    jAlert("Password and confirm password are not match.");
            //    $("#txtConfirmPassword").focus();
            //    return
            //}

            //if (ApiType == "") {
            //    jAlert("Please select Sandbox or Production");
            //    return
            //}


            $.ajax({
                type: "POST",
                url: "eInvoice-configuration.aspx/GSPOnBoardingSave",
                data: JSON.stringify({
                    gsp: gsp, FirsName: FirsName, LastName: LastName, MobileNo: MobileNo, Email: Email,
                    Password: Password, OTP: OTP, ApiType: ApiType, BasicBaseURL: '', EnrichedBaseURL: '', IRP_API_Version: '', IRP_Name: '', GSP_API_Version: '', Organization_Id: '',
                    companyBranchID: companyBranchID, eInvoice: eInvoice, GSTIN: GSTIN, companyBranchType: companyBranchType
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var data = msg.d;

                    if (data == 'OK') {
                        //jAlert("Save successfully.");
                        return true;
                    }
                }
            });
        }


        function UserRights_Click() {

            $.ajax({
                type: "POST",
                url: "eInvoice-configuration.aspx/BindUserGroup",
                data: JSON.stringify({ UserGroupID: "" }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    if (msg.d.length > 0) {
                        $("#ddlUserGroup").val(msg.d[0].User_Group);
                        $(".transfer").html("");
                       // ddlUserGroup_Change();
                        var list = msg.d;
                        var settings = {
                            dataArray: list,
                            tabNameText: "Available Users",
                            rightTabNameText: "Selected Users",
                            callable: function (list) {
                                // your code
                                // console.log("item", list)
                                selecteduser = list;
                            }
                        };
                        var transfer = $(".transfer").transfer(settings);
                    }
                }

            });
        }

        function ddlCompany_Change() {
            var companyBranchID = "";
            var companyBranchType = "";
                companyBranchID = $("#ddlCompany").val();
                companyBranchType = "Company";
            
            $.ajax({
                type: "POST",
                url: "eInvoice-configuration.aspx/GSTINSetValues",
                data: JSON.stringify({ Action: 'GSTINSetValues', companyBranchType: companyBranchType, companyBranchID: companyBranchID }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var list = msg.d;
                        $("#txtCompanyGSTIN").val(msg.d.split('~')[0]);
                         if(msg.d.split('~')[0]!=""){
                            $("#txtCompanyGSTIN").attr("disabled", true);
                        }
                        else {
                            $("#txtCompanyGSTIN").attr("disabled", false);
                        }

                        if (msg.d.split('~')[1]=="Yes") {
                            $("#chkeInvoiceCompany").prop('checked',true);
                            $("#trCompany").find("button").attr("disabled", false);
                        }
                        else {
                            $("#chkeInvoiceCompany").prop('checked',false);
                            $("#trCompany").find("button").attr("disabled", true);
                        }
                    if (msg.d == "Logout") {
                        location.href = "../../OMS/SignOff.aspx";
                    }
                }
            });
        }

        function ddlBranch_Change() {
            var companyBranchID = "";
            var companyBranchType = "";
                companyBranchID = $("#ddlBranch").val();
                companyBranchType = "Branch";
            
            $.ajax({
                type: "POST",
                url: "eInvoice-configuration.aspx/GSTINSetValues",
                data: JSON.stringify({ Action: 'BindGSTINbyBranchCompany', companyBranchType: companyBranchType, companyBranchID: companyBranchID }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var list = msg.d;
                        $("#txtBranchGTSTIN").val(msg.d.split('~')[0]);
                        if(msg.d.split('~')[0]!=""){
                            $("#txtBranchGTSTIN").attr("disabled", true);
                        }
                        else {
                            $("#txtBranchGTSTIN").attr("disabled", false);
                        }
                        if (msg.d.split('~')[1]=="Yes") {
                            $("#chkeInvoiceBranch").prop('checked',true);
                            $("#trBranch").find("button").attr("disabled", false);
                        }
                        else {
                            $("#chkeInvoiceBranch").prop('checked',false);
                             $("#trBranch").find("button").attr("disabled", true);
                        }
                    if (msg.d == "Logout") {
                        location.href = "../../OMS/SignOff.aspx";
                    }
                }
            });
        }

        function componentEndCallBack(s, e) {
           if (cPanelSalesDiscount.cpEdit!=null && cPanelSalesDiscount.cpEdit!='') {
               // clookup_SalesDiscount.gridView.SelectItemsByKey(cPanelSalesDiscount.cpEdit);
            } 
        }

         function SalesRoundOffEndCallBack(s, e) {
           if (cPanelSalesRoundoff.cpEdit!=null && cPanelSalesRoundoff.cpEdit!='') {
                // clookup_SalesRoundOff.gridView.SelectItemsByKey(cPanelSalesRoundoff.cpEdit);
            } 
        }

         function SalesTCSEndCallBack(s, e) {
           if (cPanelSalesTCS.cpEdit!=null && cPanelSalesTCS.cpEdit!='') {
              //  clookup_SalesTCS.gridView.SelectItemsByKey(cPanelSalesTCS.cpEdit);
            } 
        }

         function PurchaseDiscountEndCallBack(s, e) {
           if (cPanelPurchaseDiscount.cpEdit!=null && cPanelPurchaseDiscount.cpEdit!='') {
           for(var i=0;i<cPanelPurchaseDiscount.cpEdit.split(',').length;i++){
           // clookup_PurchaseDiscount.gridView.SelectItemsByKey(cPanelPurchaseDiscount.cpEdit.split(',')[i]);
            }
               
            } 
        }

         function PurchaseRoundoffEndCallBack(s, e) {
           if (cPanelPurchaseRoundoff.cpEdit!=null && cPanelPurchaseRoundoff.cpEdit!='') {
             // clookup_PurchaseRoundoff.gridView.SelectItemsByKey(cPanelPurchaseRoundoff.cpEdit);
            } 
        }

         function PurchaseTCSEndCallBack(s, e) {
           if (cPanelPurchaseTCS.cpEdit!=null && cPanelPurchaseTCS.cpEdit!='') {
               //  clookup_PurchaseTCS.gridView.SelectItemsByKey(cPanelPurchaseTCS.cpEdit);
            } 
        }

        function Discard_Click() {
          $("#ModalOnboarding").modal('hide');
       }
    </script>
    <style>
        .Vtabs .tab-pane {
            margin-left: 0;
            border-left: none;
            padding-left: 0;
        }
        .Vtabs .ttCont>.tab-pane {
            margin-left: 15px;
            border-left: 1px solid #e8e8e8;
            padding-left: 11px;
        }
        .rightSide  {
            padding:0;
        }
        .Vtabs {
            border-top:none;
            padding-top:0;
        }
        .Vtabs li a {
            padding-left: 5px;
            padding-right: 5px;
        }
        .floatedBtnArea.insideGrid>a {
            padding: 0 !important;
            font-size:13px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-title clearfix">
        <h3 class="pull-left">
            <%--<asp:Label ID="lblHeadTitle" Text="" runat="server"></asp:Label>--%>
            <label>
                E-Invoice 
            </label>
        </h3>
    </div>
    <div class="form_main">
        <div class="clearfix Vtabs">
            <div class="row">
            <div class="col-sm-2 col-md-2 col-lg-1">
                <!-- Nav tabs -->
                <ul class="nav nav-pills nav-stacked flex-column" style="margin-top: 20px">
                    <li class="active"><a href="#home" data-toggle="tab">Activation</a></li>
                    <li><a href="#tax" data-toggle="tab" onclick="taxcharges()">Tax/Charges </a></li>
                    <li><a href="#userRights" data-toggle="tab" onclick="UserRights_Click()">User Rights</a></li>
                </ul>
            </div>
            <div class="col-sm-10 col-md-10 col-lg-11">
                <!-- Tab panes -->
                <div class="tab-content ttCont">
                    <div class="tab-pane active" id="home">
                        <div class="row">
                            <div style=" margin: 15px">
                                
                                <div class="crmTAbhd">eInvoice Activation <span class="bulet"></span></div>
                                <table class="tblVerti fontPp">
                                    <tr id="trCompany">
                                        <td>
                                            <div class="checkbox">
                                                <label>
                                                    <input type="checkbox" value="checkboxEInvoice" id="chkCompany" />
                                                    Company</label>
                                            </div>
                                        </td>
                                        <td>
                                            <div>
                                                <%--<select class="form-control minSelect">
                                                    <option>Select</option>
                                                </select>--%>
                                                <asp:DropDownList runat="server" ID="ddlCompany" CssClass="form-control minSelect" onchange="ddlCompany_Change()">
                                                </asp:DropDownList>
                                            </div>
                                        </td>
                                        <td>
                                            <div><b>GSTIN</b></div>
                                        </td>
                                        <td>

                                            <div>
                                                <input type="text" class="form-control" id="txtCompanyGSTIN" />
                                            </div>
                                        </td>
                                        <td>
                                            <div><b>eInvoice</b></div>
                                        </td>
                                        <td>
                                            <label class="switch">
                                                <input type="checkbox" id="chkeInvoiceCompany" />
                                                <span class="slider round"></span>
                                            </label>
                                        </td>
                                        <td>
                                            <button type="button" class="btn btn-SeaGreen fontPp" data-toggle="modal" data-target="#srrHist" onclick="set_GSPonBoarding('Company');">Onboarding & login</button>
                                        </td>
                                    </tr>
                                    <tr id="trBranch">
                                        <td>
                                            <div class="checkbox">
                                                <label>
                                                    <input type="checkbox" value="checkboxEInvoice" id="chkBranch" />
                                                    Branch</label>
                                            </div>
                                        </td>
                                        <td>

                                            <div>
                                                <%-- <select class="form-control minSelect">
                                                    <option>Select</option>
                                                </select>--%>
                                                <asp:DropDownList runat="server" ID="ddlBranch" CssClass="form-control minSelect" onchange="ddlBranch_Change()">
                                                </asp:DropDownList>
                                            </div>
                                        </td>
                                        <td>
                                            <div><b>GSTIN</b></div>
                                        </td>
                                        <td>

                                            <div>
                                                <input type="text" class="form-control" id="txtBranchGTSTIN" />
                                            </div>
                                        </td>
                                        <td>
                                            <div><b>eInvoice</b></div>
                                        </td>
                                        <td>
                                            <label class="switch">
                                                <input type="checkbox" id="chkeInvoiceBranch" />
                                                <span class="slider round"></span>
                                            </label>
                                        </td>
                                        <td>
                                            <button type="button" class="btn btn-SeaGreen fontPp" data-toggle="modal" data-target="#srrHist" onclick="set_GSPonBoarding('Branch');">Onboarding & login</button>
                                        </td>
                                    </tr>
                                </table>
                                <dxe:ASPxGridView ID="GrdDevice" runat="server" KeyFieldName="CompanyBranch_Id" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
                                    SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" OnDataBinding="grid_DataBinding" OnCustomCallback="GrdDevice_CustomCallback"
                                    Width="100%" ClientInstanceName="cGrdDevice" Settings-HorizontalScrollBarMode="auto">
                                    <SettingsSearchPanel Visible="false" Delay="5000" />
                                    <Columns>

                                        <dxe:GridViewDataTextColumn Caption="CompanyBranch_Id" FieldName="CompanyBranch_Id"
                                            VisibleIndex="1" FixedStyle="Left" Width="200px" Visible="false">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Entity" FieldName="Entity" VisibleIndex="2" Width="50%">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="GSTIN" FieldName="GSTIN" VisibleIndex="4" Width="30%">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="eInvoice" FieldName="eInvoice" VisibleIndex="5" Width="10%">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>


                                        <%--<dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="15" Width="10%">
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
                                        </dxe:GridViewDataTextColumn>--%>
                                    </Columns>
                                    <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                                    <%--<clientsideevents endcallback="grid_EndCallBack" />--%>
                                    <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                        <FirstPageButton Visible="True">
                                        </FirstPageButton>
                                        <LastPageButton Visible="True">
                                        </LastPageButton>
                                    </SettingsPager>

                                    <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                    <SettingsLoadingPanel Text="Please Wait..." />
                                </dxe:ASPxGridView>

                                <%--<button type="button" id="btneInvoiceActivationSave" class="btn btn-success" onclick="Save_eInvoiceActivation();">Save</button>--%>
                            </div>
                        </div>
                    </div>
                    <div class="tab-pane " id="tax">
                        <div style="padding: 15px;">
                            
                            <div class="crmTAbhd">Sales <span class="bulet"></span></div>
                            <div class="demo-container" style="margin-top: 10px">
                                <table class="mainColorTbl fontPp">
                                    <thead>
                                        <tr>
                                            <th>eInvoice Schema
                                            </th>
                                            <th>ERP Schema
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>Discount
                                            </td>
                                            <td>
                                                <dxe:ASPxCallbackPanel runat="server" ID="PanelSalesDiscount" ClientInstanceName="cPanelSalesDiscount" OnCallback="PanelSalesDiscount_Callback">
                                                    <PanelCollection>
                                                        <dxe:PanelContent runat="server">
                                                            <dxe:ASPxGridLookup ID="lookup_SalesDiscount" SelectionMode="Multiple" AllowUserInput="false" runat="server" ClientInstanceName="clookup_SalesDiscount"
                                                                KeyFieldName="Taxes_ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", "
                                                                OnDataBinding="lookup_SalesDiscount_DataBinding">
                                                                <Columns>
                                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                                                    <dxe:GridViewDataColumn FieldName="Taxes_Code" Visible="true" VisibleIndex="1" Caption="Tax Code" Settings-AutoFilterCondition="Contains" Width="150" />
                                                                    <dxe:GridViewDataColumn FieldName="Taxes_Description" Visible="true" VisibleIndex="4" Caption="Tax" Settings-AutoFilterCondition="Contains" Width="120" />
                                                                </Columns>
                                                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                                    <Templates>
                                                                        <StatusBar>
                                                                            <table class="OptionsTable" style="float: right">
                                                                                <tr>
                                                                                    <td>
                                                                                        <%--<dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" />--%>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </StatusBar>
                                                                    </Templates>
                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                                                    <SettingsPager Mode="ShowPager">
                                                                        <%-- <PageSizeItemSettings Items="10,20,30"></PageSizeItemSettings>--%>
                                                                    </SettingsPager>
                                                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                                </GridViewProperties>
                                                                <ClearButton DisplayMode="Always">
                                                                </ClearButton>
                                                                <%--<ClientSideEvents LostFocus="function(s,e){SalesDiscount_lostfocus();  }" />--%>
                                                            </dxe:ASPxGridLookup>
                                                            <%--GotFocus="function(s,e){gridquotationLookup.ShowDropDown();}"--%>
                                                        </dxe:PanelContent>
                                                    </PanelCollection>
                                                    <ClientSideEvents EndCallback="componentEndCallBack" />
                                                </dxe:ASPxCallbackPanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Round Off
                                            </td>
                                            <td>
                                                <dxe:ASPxCallbackPanel runat="server" ID="PanelSalesRoundOff" ClientInstanceName="cPanelSalesRoundoff" OnCallback="PanelSalesRoundOff_Callback">
                                                    <PanelCollection>
                                                        <dxe:PanelContent runat="server">
                                                            <dxe:ASPxGridLookup ID="lookup_SalesRoundOff" SelectionMode="Multiple" AllowUserInput="false" runat="server" ClientInstanceName="clookup_SalesRoundOff"
                                                                KeyFieldName="Taxes_ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", "
                                                                OnDataBinding="lookup_SalesRoundOff_DataBinding">
                                                                <%--DataSourceID="EntityServerModeDataOverheadCost"--%>
                                                                <Columns>
                                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                                                    <dxe:GridViewDataColumn FieldName="Taxes_Code" Visible="true" VisibleIndex="1" Caption="Tax Code" Settings-AutoFilterCondition="Contains" Width="150" />
                                                                    <dxe:GridViewDataColumn FieldName="Taxes_Description" Visible="true" VisibleIndex="4" Caption="Tax" Settings-AutoFilterCondition="Contains" Width="120" />
                                                                </Columns>
                                                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                                    <Templates>
                                                                        <StatusBar>
                                                                            <table class="OptionsTable" style="float: right">
                                                                                <tr>
                                                                                    <td>
                                                                                        <%--<dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" />--%>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </StatusBar>
                                                                    </Templates>
                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                                                    <SettingsPager Mode="ShowPager">
                                                                        <%-- <PageSizeItemSettings Items="10,20,30"></PageSizeItemSettings>--%>
                                                                    </SettingsPager>
                                                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                                </GridViewProperties>
                                                                <ClearButton DisplayMode="Always">
                                                                </ClearButton>
                                                                <%--<ClientSideEvents LostFocus="function(s,e){SalesRoundOff_lostfocus();  }" />--%>
                                                            </dxe:ASPxGridLookup>
                                                        </dxe:PanelContent>
                                                    </PanelCollection>
                                                    <ClientSideEvents EndCallback="SalesRoundOffEndCallBack" />
                                                </dxe:ASPxCallbackPanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Other Charges
                                            </td>
                                            <td>
                                                <dxe:ASPxCallbackPanel runat="server" ID="PanelSalesTCS" ClientInstanceName="cPanelSalesTCS" OnCallback="PanelSalesTCS_Callback">
                                                    <PanelCollection>
                                                        <dxe:PanelContent runat="server">
                                                            <dxe:ASPxGridLookup ID="lookup_SalesTCS" SelectionMode="Multiple" AllowUserInput="false" runat="server" ClientInstanceName="clookup_SalesTCS"
                                                                KeyFieldName="Taxes_ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", "
                                                                OnDataBinding="lookup_SalesTCS_DataBinding">
                                                                <%--DataSourceID="EntityServerModeDataOverheadCost"--%>
                                                                <Columns>
                                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                                                    <dxe:GridViewDataColumn FieldName="Taxes_Code" Visible="true" VisibleIndex="1" Caption="Tax Code" Settings-AutoFilterCondition="Contains" Width="150" />
                                                                    <dxe:GridViewDataColumn FieldName="Taxes_Description" Visible="true" VisibleIndex="4" Caption="Tax" Settings-AutoFilterCondition="Contains" Width="120" />
                                                                </Columns>
                                                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                                    <Templates>
                                                                        <StatusBar>
                                                                            <table class="OptionsTable" style="float: right">
                                                                                <tr>
                                                                                    <td>
                                                                                        <%--<dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" />--%>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </StatusBar>
                                                                    </Templates>
                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                                                    <SettingsPager Mode="ShowPager">
                                                                        <%-- <PageSizeItemSettings Items="10,20,30"></PageSizeItemSettings>--%>
                                                                    </SettingsPager>
                                                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                                </GridViewProperties>
                                                                <ClearButton DisplayMode="Always">
                                                                </ClearButton>
                                                                <%--<ClientSideEvents LostFocus="function(s,e){SalesTCS_lostfocus();  }" />--%>
                                                            </dxe:ASPxGridLookup>
                                                            <%--GotFocus="function(s,e){gridquotationLookup.ShowDropDown();}"--%>
                                                        </dxe:PanelContent>
                                                    </PanelCollection>
                                                    <ClientSideEvents EndCallback="SalesTCSEndCallBack" />
                                                </dxe:ASPxCallbackPanel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>

                        <div style="padding: 15px; margin-top: 15px">
                           
                            <div class="crmTAbhd">Purchase <span class="bulet"></span></div>
                            <div class="demo-container" style="margin-top: 10px">
                                <table class="mainColorTbl fontPp">
                                    <thead>
                                        <tr>
                                            <th>eInvoice Schema
                                            </th>
                                            <th>ERP Schema
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>Discount
                                            </td>
                                            <td>
                                                <dxe:ASPxCallbackPanel runat="server" ID="PanelPurchaseDiscount" ClientInstanceName="cPanelPurchaseDiscount" OnCallback="PanelPurchaseDiscount_Callback">
                                                    <PanelCollection>
                                                        <dxe:PanelContent runat="server">
                                                            <dxe:ASPxGridLookup ID="lookup_PurchaseDiscount" SelectionMode="Multiple" AllowUserInput="false" runat="server" ClientInstanceName="clookup_PurchaseDiscount"
                                                                KeyFieldName="Taxes_ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", "
                                                                OnDataBinding="lookup_PurchaseDiscount_DataBinding">
                                                                <%--DataSourceID="EntityServerModeDataOverheadCost"--%>
                                                                <Columns>
                                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                                                    <dxe:GridViewDataColumn FieldName="Taxes_Code" Visible="true" VisibleIndex="1" Caption="Tax Code" Settings-AutoFilterCondition="Contains" Width="150" />
                                                                    <dxe:GridViewDataColumn FieldName="Taxes_Description" Visible="true" VisibleIndex="4" Caption="Tax" Settings-AutoFilterCondition="Contains" Width="120" />
                                                                </Columns>
                                                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                                    <Templates>
                                                                        <StatusBar>
                                                                            <table class="OptionsTable" style="float: right">
                                                                                <tr>
                                                                                    <td>

                                                                                        <%--<dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" />--%>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </StatusBar>
                                                                    </Templates>
                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                                                    <SettingsPager Mode="ShowPager">
                                                                        <%-- <PageSizeItemSettings Items="10,20,30"></PageSizeItemSettings>--%>
                                                                    </SettingsPager>
                                                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                                </GridViewProperties>
                                                                <ClearButton DisplayMode="Always">
                                                                </ClearButton>
                                                            </dxe:ASPxGridLookup>
                                                        </dxe:PanelContent>
                                                    </PanelCollection>
                                                    <ClientSideEvents EndCallback="PurchaseDiscountEndCallBack" />
                                                </dxe:ASPxCallbackPanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Round Off
                                            </td>
                                            <td>
                                                <dxe:ASPxCallbackPanel runat="server" ID="PanelPurchaseRoundoff" ClientInstanceName="cPanelPurchaseRoundoff" OnCallback="PanelPurchaseRoundoff_Callback">
                                                    <PanelCollection>
                                                        <dxe:PanelContent runat="server">
                                                            <dxe:ASPxGridLookup ID="lookup_PurchaseRoundoff" SelectionMode="Multiple" AllowUserInput="false" runat="server" ClientInstanceName="clookup_PurchaseRoundoff"
                                                                KeyFieldName="Taxes_ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", "
                                                                OnDataBinding="lookup_PurchaseRoundoff_DataBinding">
                                                                <%--DataSourceID="EntityServerModeDataOverheadCost"--%>
                                                                <Columns>
                                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                                                    <dxe:GridViewDataColumn FieldName="Taxes_Code" Visible="true" VisibleIndex="1" Caption="Tax Code" Settings-AutoFilterCondition="Contains" Width="150" />
                                                                    <dxe:GridViewDataColumn FieldName="Taxes_Description" Visible="true" VisibleIndex="4" Caption="Tax" Settings-AutoFilterCondition="Contains" Width="120" />
                                                                </Columns>
                                                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                                    <Templates>
                                                                        <StatusBar>
                                                                            <table class="OptionsTable" style="float: right">
                                                                                <tr>
                                                                                    <td>

                                                                                        <%--<dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" />--%>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </StatusBar>
                                                                    </Templates>
                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                                                    <SettingsPager Mode="ShowPager">
                                                                        <%-- <PageSizeItemSettings Items="10,20,30"></PageSizeItemSettings>--%>
                                                                    </SettingsPager>

                                                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                                </GridViewProperties>
                                                                <ClearButton DisplayMode="Always">
                                                                </ClearButton>
                                                            </dxe:ASPxGridLookup>
                                                        </dxe:PanelContent>
                                                    </PanelCollection>
                                                    <ClientSideEvents EndCallback="PurchaseRoundoffEndCallBack" />
                                                </dxe:ASPxCallbackPanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Other Charges
                                            </td>
                                            <td>
                                                <dxe:ASPxCallbackPanel runat="server" ID="PanelPurchaseTCS" ClientInstanceName="cPanelPurchaseTCS" OnCallback="PanelPurchaseTCS_Callback">
                                                    <PanelCollection>
                                                        <dxe:PanelContent runat="server">
                                                            <dxe:ASPxGridLookup ID="lookup_PurchaseTCS" SelectionMode="Multiple" AllowUserInput="false" runat="server" ClientInstanceName="clookup_PurchaseTCS"
                                                                KeyFieldName="Taxes_ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", "
                                                                OnDataBinding="lookup_PurchaseTCS_DataBinding">
                                                                <%--DataSourceID="EntityServerModeDataOverheadCost"--%>
                                                                <Columns>
                                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                                                    <dxe:GridViewDataColumn FieldName="Taxes_Code" Visible="true" VisibleIndex="1" Caption="Tax Code" Settings-AutoFilterCondition="Contains" Width="150" />
                                                                    <dxe:GridViewDataColumn FieldName="Taxes_Description" Visible="true" VisibleIndex="4" Caption="Tax" Settings-AutoFilterCondition="Contains" Width="120" />
                                                                </Columns>
                                                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                                    <Templates>
                                                                        <StatusBar>
                                                                            <table class="OptionsTable" style="float: right">
                                                                                <tr>
                                                                                    <td>

                                                                                        <%--<dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" />--%>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </StatusBar>
                                                                    </Templates>
                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                                                    <SettingsPager Mode="ShowPager">
                                                                        <%-- <PageSizeItemSettings Items="10,20,30"></PageSizeItemSettings>--%>
                                                                    </SettingsPager>
                                                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                                </GridViewProperties>
                                                                <ClearButton DisplayMode="Always">
                                                                </ClearButton>
                                                            </dxe:ASPxGridLookup>
                                                            <%--GotFocus="function(s,e){gridquotationLookup.ShowDropDown();}"--%>
                                                        </dxe:PanelContent>
                                                    </PanelCollection>
                                                    <ClientSideEvents EndCallback="PurchaseTCSEndCallBack" />
                                                </dxe:ASPxCallbackPanel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div class="mTop5">
                            <button type="button" id="btnTaxChargesSave" class="btn btn-success" onclick="Save_TaxCharges();">Save</button></div>

                    </div>
                    <div class="tab-pane" id="userRights">
                        <div class="col-md-12 hide">
                            <table class="tblVerti fontPp">
                                <tr>
                                    <td><b>User Groups</b></td>
                                    <td>
                                        <%-- <select class="form-control minSelect">
                                            <option>Select</option>
                                        </select>--%>
                                        <asp:DropDownList runat="server" ID="ddlUserGroup" onchange="ddlUserGroup_Change();" CssClass="form-control minSelect">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <div class="checkbox">
                                            <label>
                                                <input type="checkbox" value="" id="chkAllUser" />
                                                All Users</label>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="col-md-12">
                            <div class="transfer"></div>
                        </div>
                        <div class="col-md-12 mTop5">
                            <button type="button" id="btnUserGroupSave" class="btn btn-success mTop5" onclick="Save_UserGroup();">Save</button></div>
                    </div>
                </div>
            </div>
            </div>
        </div>
    </div>

    <div class="modal fade pmsModal w50" id="ModalOnboarding" tabindex="-1" role="dialog" aria-labelledby="srrHist" aria-hidden="true" data-keyboard="false" data-backdrop="static">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Onboarding & login</h5>
                    <%-- <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>--%>
                </div>
                <div class="modal-body">
                    <div class="row ">
                        <div class="col-md-12">
                            <div>
                                <div class="form_main">
                                    <div class="boxedMatt">
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <label>Select GSP <span class="red">*</span></label>
                                                        <div>
                                                            <%--<input type="text" class="form-control" />--%>
                                                            <asp:DropDownList runat="server" ID="ddlGSP" CssClass="form-control">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <label>First Name</label>
                                                        <div>
                                                            <input type="text" class="form-control" runat="server" id="txtFirsName" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <label>Mobile No <span class="red">*</span></label>
                                                        <div>
                                                            <input type="text" class="form-control" runat="server" id="txtMobileNo" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <label>Email(User ID) <span class="red">*</span></label>
                                                        <div>
                                                            <input type="text" class="form-control" runat="server" id="txtEmail" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <label>Password <span class="red">*</span></label>
                                                        <div class="">
                                                            <input type="password" value="" placeholder="Enter Password" class="password" runat="server" id="txtPassword" />
                                                            <%--<button class="unmask" type="button"></button>--%>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row hide">
                                                    <div class="col-md-12">
                                                        <label>Confirm Password</label>
                                                        <div>
                                                            <input type="password" class="password" runat="server" id="txtConfirmPassword" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div id="strong"><span></span></div>
                                                        <div id="valid"></div>
                                                        <small>Must be 6+ characters long and contain at least 1 upper case letter, 1 number, 1 special character</small>
                                                    </div>
                                                </div>
                                                <div class="row hide">
                                                    <div class="col-md-12">
                                                        <label>OTP</label>
                                                        <div>
                                                            <input type="text" class="form-control" runat="server" id="txtOTP" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row hide">
                                                    <div class="col-md-12 " style="padding: 10px 15px">
                                                        <button class="btn btn-default" type="button" id="btnEmailOTP">Request for Email OTP</button>
                                                        <button class="btn btn-success" type="button" id="btnSubmit" onclick="Save_Record();">Save</button>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div style="padding-top: 27px;">
                                                    <table>
                                                        <tr>
                                                            <td style="padding-right: 30px">
                                                                <div class="form-check form-check-inline">
                                                                    <input class="form-check-input" type="radio" name="inlineRadioOptions" id="rdbSandbox" value="option1" />
                                                                    <label class="form-check-label" for="rdbSandbox">Sandbox</label>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="form-check form-check-inline">
                                                                    <input class="form-check-input" type="radio" name="inlineRadioOptions" id="rdbProduction" value="option2" />
                                                                    <label class="form-check-label" for="rdbProduction">Production</label>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <label>Last Name</label>
                                                        <div>
                                                            <input type="text" class="form-control" runat="server" id="txtLastName" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <%-- <div class="row">
                                                    <div class="col-md-12">
                                                        <label>Mobile No</label>
                                                        <div>
                                                            <input type="text" class="form-control" runat="server" id="Text1" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <label>Mobile No</label>
                                                        <div>
                                                            <input type="text" class="form-control" runat="server" id="Text2" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <label>Mobile No</label>
                                                        <div>
                                                            <input type="text" class="form-control" runat="server" id="Text3" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <label>Mobile No</label>
                                                        <div>
                                                            <input type="text" class="form-control" runat="server" id="Text4" />
                                                        </div>
                                                    </div>
                                                </div>--%>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" onclick="Discard_Click()">Discard</button>
                    <button type="button" class="btn btn-success" id="btnActivationSave" onclick="eInvoiceActivation_Update();">Save</button>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnActiveEInvoice" />
    <asp:HiddenField runat="server" ID="DefaultEinvoiceSellerAddress" />
    <script>

        $(document).ready(function () {
            var dataArray = [
                //{
                //    item: "Beijing",
                //    value: 1
                //},
                //{
                //    item: "Shanghai",
                //    value: 2
                //},
                //{
                //    item: "Tokyo",
                //    value: 6
                //}
            ];
            var settings = {
                dataArray: dataArray,
                tabNameText: "Available Users",
                rightTabNameText: "Selected Users",
                callable: function (items) {
                    // your code
                    selecteduser = [];
                }
            };
            selecteduser = [];
            var transfer = $(".transfer").transfer(settings);

        });
    </script>
</asp:Content>
