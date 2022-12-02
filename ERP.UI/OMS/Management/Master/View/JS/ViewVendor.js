var app = angular.module("myApp", []);
app.controller("myCtrl", function ($scope, $http) {
    var full_url = document.URL;
    var url_array = full_url.split('?id=');
    var id = url_array[1];
    $scope.correspondence_dtls = [];
    $scope.phone_number = [];
    $scope.email = [];
    $scope.bank = [];
    $scope.registration = [];
    $scope.membership = [];
    $scope.group = [];
    $scope.branch_dtls = [];
    $scope.contact_list = [];
    $scope.document_dtls = [];
    //Rev Bapi
    $scope.product_dtls = [];
    //End Rev Bapi

    var obj = {};
    obj.id = id;


    $http({

        method: "POST",
        url: "services/ViewVendorService.asmx/GetVendorDetails",
        dataType: 'json',
        data: JSON.stringify(obj),
        //params: { date:"a"},

        headers: { "Content-Type": "application/json" }

    }).then(function (response) {
        if (response.data.d) {
            if (response.data.d.msg == "ok") {
                $scope.lbl_legal_status = response.data.d.general.legal_status;
                $scope.lbl_unique_id = response.data.d.general.unique_id;
                $scope.lbl_vndor_name = response.data.d.general.vendor_name;
                $scope.lbl_rffr_by = response.data.d.general.reffered_by;
                $scope.lbl_status = response.data.d.general.status;
                $scope.lbldt_incrp = response.data.d.general.dt_incorp;
                $scope.lbl_m_acnt = response.data.d.general.main_acnt;
                $scope.lbl_GSTIN = response.data.d.general.gstin;
                $scope.check_register = response.data.d.general.register;
                $scope.lbl_acnt_grp = response.data.d.general.acnts_grp;
                $scope.lbl_vndr_type = response.data.d.general.vendor_type;
                $scope.lbl_print_cheque = response.data.d.general.cheque_print;
                $scope.branch_dtls = response.data.d.branch_dtls;
                $scope.lbl_deductee_type = response.data.d.general.TDS_Deductees;

                $scope.correspondence_dtls = response.data.d.corspndnc_dtls;
                $scope.phone_number = response.data.d.Phone_number;
                $scope.email = response.data.d.email_dtls;
                $scope.bank = response.data.d.bank_dtls;
                $scope.registration = response.data.d.reg_dtls;
                $scope.membership = response.data.d.membershp_dtls;
                $scope.group = response.data.d.Group_dtls;
                $scope.contact_list = response.data.d.contact_person;
                $scope.document_dtls = response.data.d.doc_details;
                $scope.product_dtls = response.data.d.product_dtls;

                if ($scope.check_register) {

                    $scope.class = "glyphicon-ok";
                }
                else {

                    $scope.class = "glyphicon-remove";
                }

            }

        }
        else {
            alert(response.data.d.msg);
        }




        console.log(response.data);
        //alert('');
        //console.log($scope.check_disabl);

    });

   

    $scope.btn_show = function () {
        // alert(1)
        $('#TimeUpdateModel').modal('show');
    }

    //Rev Bapi
    $scope.btn_showP = function () {
       
        $('#ProductUpdateModel').modal('show');
    }
    //End Rev Bapi


});