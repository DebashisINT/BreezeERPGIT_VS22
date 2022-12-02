
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
    $scope.document_dtls = [];

    var obj = {};
    obj.id = id;


    $http({

        method: "POST",
        url: "services/ViewTransportService.asmx/GetTransporterDetails",
        dataType: 'json',
        data: JSON.stringify(obj),
        //params: { date:"a"},

        headers: { "Content-Type": "application/json" }

    }).then(function (response) {
        if (response.data.d) {            
            if (response.data.d.msg == "ok") {
                $scope.lbl_cust_type = response.data.d.general.lgl_legalStatus;
                //$scope.lbl_id_type = response.data.d.general.cnt_IdType;
                if (response.data.d.general.cnt_IdType != null && response.data.d.general.cnt_IdType.trim() != "") {
                    $scope.lbl_id_type = response.data.d.general.cnt_IdType;
                }
                else {
                    $scope.lbl_id_type = "Unique Code";
                }
                $scope.lbl_unique_id = response.data.d.general.unique_id;
                $scope.lbl_full_nm = response.data.d.general.fulll_name;
                $scope.lbldtbrth = response.data.d.general.dob;
                $scope.lbl_cust_ntnlty = response.data.d.general.nationality;
                $scope.lbl_cust_anvrsry = response.data.d.general.anniver_dt;
                $scope.lbl_gender = response.data.d.general.gender;
                $scope.lbl_crdt_days = response.data.d.general.credit_days;
                $scope.lbl_crdt_limit = response.data.d.general.credit_limit;
                $scope.lbl_m_status = response.data.d.general.maritial_status;
                $scope.lblstatus = response.data.d.general.status_type;
                $scope.lbl_m_acnt = response.data.d.general.main_acnt;
                $scope.check_disabl = response.data.d.general.credit_hold;
                $scope.disable = response.data.d.general.credit_hold;
                $scope.lbl_GSTIN = response.data.d.general.gstin;
                $scope.lbl_TransEnId = response.data.d.general.TransEnId;
                $scope.check_register = response.data.d.general.register;
                // $scope.ng_register = response.data.d.general.register
                $scope.correspondence_dtls = response.data.d.corspndnc_dtls;
                $scope.phone_number = response.data.d.Phone_number;
                $scope.email = response.data.d.email_dtls;
                $scope.bank = response.data.d.bank_dtls;
                $scope.registration = response.data.d.reg_dtls;
                $scope.membership = response.data.d.membershp_dtls;
                $scope.group = response.data.d.Group_dtls;
                $scope.document_dtls = response.data.d.doc_details;
                //Rev work start 27.06.2022 mantise:-24992
                $scope.lbl_vehicleno = response.data.d.Vehicle_details.VehicleNos;
                //Rev work close 27.06.2022 mantise:-24992
                
                if ($scope.check_register == 1) {

                    $scope.class1 = "glyphicon-ok";
                }
                else {

                    $scope.class1 = "glyphicon-remove";
                }

            }

        }
        else {
            alert(response.data.d.msg);
        }




        console.log(response.data);
        //alert('');
        console.log($scope.check_register);

    });

    //$scope.show = function (id) {
    //    $scope.detailsid = id;
    //    $('#TimeUpdateModel').modal('show');
    //}

    //$scope.trms_show = function () {
    //    // alert(1)
    //    $('#TimeUpdateModel1').modal('show');
    //}


});