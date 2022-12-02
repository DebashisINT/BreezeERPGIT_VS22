
var app = angular.module("myApp",[]);

app.controller("myCtrl", function ($scope, $http) {
    var full_url = document.URL;
    var url_array = full_url.split('id=');
    var id = url_array[1];
    var obj = {};
    obj.id = id;
    $scope.Product_dtls = [];
    $scope.document_dtls = [];




    $http({

        method: "POST",
        url: "services/ViewProductService.asmx/GetProductDetails",
        dataType: 'json',
        data: JSON.stringify(obj),
        //params: { date:"a"},

        headers: { "Content-Type": "application/json" }

    }).then(function (response) {
        if (response.data.d) {
            if (response.data.d.msg == "ok") {
                $scope.ng_short_name = response.data.d._header.sProducts_Code;
                $scope.ng_name = response.data.d._header.sProducts_Name;
                $scope.ng_desc = response.data.d._header.sProducts_Description;
                $scope.ng_inventory = response.data.d._header.Inventory_item;
                $scope.ng_service_item = response.data.d._header.Service_item;
                $scope.ng_type = response.data.d._header.sProducts_Type;
                $scope.ng_stck_value = response.data.d._header.stck_val_tech;
                $scope.ng_class_nm = response.data.d._header.class_name;
                $scope.ng_status = response.data.d._header.status;
                $scope.ng_hsn_code = response.data.d._header.sProducts_HsnCode;
                $scope.check_disabl = response.data.d._header.furtherence_to_buisness;
                $scope.disable = response.data.d._header.furtherence_to_buisness;
                $scope.ng_quote_currency = response.data.d._header.Quote_currency;
                $scope.ng_uom_factor = response.data.d._header.uom_factor;
                $scope.ng_quote_uom = response.data.d._header.Quote_uom;
                $scope.ng_to_buisness = response.data.d._header.Capital_goods;
                $scope.ng_sale_uom_factor = response.data.d._header.Sale_Uom_Factor;
                $scope.ng_sale_uom = response.data.d._header.sale_uom;
                $scope.ng_sale_price = response.data.d._header.sale_price;
                $scope.ng_min_sale_price = response.data.d._header.min_sale_price;
                $scope.ng_purchase_uom_factor = response.data.d._header.Purchase_uom_factor;
                $scope.ng_purchase_uom = response.data.d._header.purchase_uom;
                $scope.ng_purchase_price = response.data.d._header.purchase_price;
                $scope.ng_mrp = response.data.d._header.mrp;
                $scope.ng_stock_uom = response.data.d._header.stock_uom
                $scope.ng_min_levl = response.data.d._header.min_levl;
                $scope.ng_reordr_lvl = response.data.d._header.reordr_levl;
                $scope.ng_negative_stock = response.data.d._header.negative_stock;
                $scope.ng_sale_invoice = response.data.d._header.sInv_MainAccount_Name;
                $scope.ng_sale_return = response.data.d._header.sRet_MainAccount_Name;
                $scope.ng_purchase_invoice = response.data.d._header.pInv_MainAccount_Name;
                $scope.ng_purchase_return = response.data.d._header.pRet_MainAccount_Name;

                $scope.ng_bar_type = response.data.d._header.sProducts_barCodeType;
                $scope.ng_bar_no = response.data.d._header.sProducts_barCode;
                $scope.ng_global_code = response.data.d._header.sProducts_GlobalCode;

                $scope.ng_tax_code_scheme_sales = response.data.d._header.tax_code_scheme_sales;
                $scope.ng_tax_code_scheme_purchase = response.data.d._header.tax_code_scheme_purchase;
                $scope.ng_service_category=response.data.d._header.service_category;


                $scope.ng_tds_sec = response.data.d._header.tdsdescription;

                $scope.ng_product_color = response.data.d._header.Color_Name;
                $scope.check_applicable = response.data.d._header.color_applicable;
                $scope.ng_product_size = response.data.d._header.Size_Name;
                $scope.check_applicable1 = response.data.d._header.size_applicable;
                $scope.ng_install_req = response.data.d._header.install_req;
                $scope.ng_brand = response.data.d._header.Brand_Name;
                $scope.ng_old_unit = response.data.d._header.is_old_unt;
                $scope.ng_quantity = response.data.d._header.product_quantity;
                $scope.ng_packing = response.data.d._header.packing_quantity;
                $scope.ng_sel_uom = response.data.d._header.select_uom;
                $scope.ng_reordr_quantity = response.data.d._header.Reorder_Quantity;

                $scope.Product_dtls = response.data.d.Components_dtls;
                $scope.document_dtls = response.data.d.doc_details;

                
             
                if ($scope.check_disabl) {

                    $scope.class2 = "glyphicon-ok";
                }
                else {

                    $scope.class2 = "glyphicon-remove";
                }

                if ($scope.check_applicable) {

                    $scope.class1 = "glyphicon-ok";
                }
                else {

                    $scope.class1 = "glyphicon-remove";
                }

                if ($scope.check_applicable1) {

                    $scope.class3 = "glyphicon-ok";
                }
                else {

                    $scope.class3 = "glyphicon-remove";
                }

            }
            else {
                alert(response.data.d.msg);
            }
        }
       




       // console.log(response.data);
        //alert('');
        //console.log($scope.check_disabl);

    });



    $scope.ShowProductAttribute= function ()
    {
        //alert(1);
        $("#header_main").slideUp();
        $("#c_prod_attr").slideDown();
    }
    $scope.close_prod_attr= function () { 
        $("#c_prod_attr").slideToggle();
        $("#header_main").slideToggle();
       
    }

    $scope.ShowBarCode=function()
    {
        $("#header_main").slideUp();
        $("#c_bar_code").slideDown();
    }
    $scope.close_barcode=function()
    {
        $("#c_bar_code").slideToggle();
        $("#header_main").slideToggle();
    }

    $scope.ShowTaxCode=function()
    {
        $("#header_main").slideUp();
        $("#c_tax").slideDown();
    }
    $scope.close_tax=function()
    {
        $("#c_tax").slideToggle();
        $("#header_main").slideToggle();
    }
    $scope.ShowServiceTax=function()
    {
        $("#header_main").slideUp();
        $("#c_service_catgory").slideDown();
    }
    $scope.close_service_category=function()
    {
        $("#c_service_catgory").slideToggle();
        $("#header_main").slideToggle();
    }
    $scope.ShowPackingDetails=function()
    {
        $("#header_main").slideUp();
        $("#c_packing_details").slideDown();
    }
    $scope.close_packing_dtls=function()
    {
        $("#c_packing_details").slideToggle();
        $("#header_main").slideToggle();
    }
    $scope.ShowTdsSection=function()
    {
        $("#header_main").slideUp();
        $("#c_tds_section").slideDown();
    }
    $scope.close_tds=function()
    {
        $("#c_tds_section").slideToggle();
        $("#header_main").slideToggle();
    }
    $scope.ShowDocuments= function ()
    {
        $("#header_main").slideUp();
        $("#c_documents").slideDown();
    }
    $scope.close_documents = function ()
    {
        
        $("#c_documents").slideToggle();
        $("#header_main").slideToggle();
    }
});