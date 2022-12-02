
        $(document).ready(function () {
            $('.js-example-basic-single').select2();

            $('#filterToggle').hide();

            $('.togglerSlide').click(function () {
                $('#filterToggle').slideDown({
                    duration: 200,
                    easing: "easeOutQuad"
                });
                $(this).hide()
            });
            $('.togglerSlidecut').click(function () {
                $('#filterToggle').slideUp({
                    duration: 200,
                    easing: "easeOutQuad"
                });
                $('.togglerSlide').show();
            });
            $('#dataTable').DataTable({
                scrollX: true,
                fixedColumns: {
                    rightColumns: 2
                }
            });
            $('#dataTable2').DataTable({
            });
            $('#dataTable3').DataTable({
            });
            $('[data-toggle="tooltip"]').tooltip();

            var date_diff = 1 + 'd';
            $(".date").datepicker({
                autoclose: true,
                todayHighlight: true,
                format: 'dd-mm-yyyy',
                minDate:0,
            }).datepicker('update', new Date());



            if ($("#hdnWarrantyForEdit").val() != "") {
                var dtser = $("#hdnWarrantyForEdit").val();
                $("#dtUpdateWarranty").val(dtser);
            }

            if ($("#txtWarrantyDate").val() != '') {
                $('.date').datepicker('update', $("#txtWarrantyDate").val());
            }

        });
  
                function Search_Click() {

                    $.ajax({
                        type: "POST",
                        url: "WarrantyEntry.aspx/SerialNoFetch",
                        data: JSON.stringify({ SerialNo: $("#txtSerialNo").val() }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var list = msg.d;
                            if (msg.d.DocumentNumber == "" || msg.d.DocumentNumber == null) {
                                jAlert("Please enter valid serial no.", "Alert", function () {
                                    setTimeout(function () {
                                        $("#txtSerialNo").focus();
                                    }, 200);
                                });
                            }
                            else {
                                if (msg.d.STATUS == "DE") {
                                    jAlert("Challan is already Delivered.", "Alert", function () {
                                        setTimeout(function () {
                                            $("#txtSerialNo").focus();
                                        }, 200);
                                    });
                                }
                                else {
                                    $("#txtReceiptChallanNo").val(msg.d.DocumentNumber);
                                    $("#txtDate").val(msg.d.DocumentDate);
                                    $("#txtEntityCode").val(msg.d.EntityCode);
                                    $("#txtNetworkName").val(msg.d.NetworkName);
                                    $("#txtdtlsSerialNo").val(msg.d.DeviceNumber);
                                    $("#txtNewSerialNo").val(msg.d.NewSerialNo);
                                    $("#txtWarrantyStatus").val(msg.d.WarrantyStatus);
                                    $("#txtProblemFound").val(msg.d.ProblemFound);
                                    if (msg.d.Warranty != "01-01-1900") {
                                        $("#txtWarrantyDate").val(msg.d.Warranty);
                                        if (msg.d.Warranty != "") {
                                            $('.date').datepicker('update', msg.d.Warranty);
                                        }
                                    }
                                    //Mantis Issue 24290
                                    $("#ddlWarrentyStatus").val(msg.d.WarrentyStatusID);
                                    dllval = msg.d.WarrentyStatusID;
                                    // End of Mantis Issue 24290
                                    $("#hdnReceiptChallanID").val(msg.d.ReceiptChallan_ID);
                                    $("#hdnEntryID").val(msg.d.Entry_Id);
                                    $("#hdnEntryDtlsId").val(msg.d.EntryDtls_Id);

                                }
                            }
                        }
                    });
                }

        function UpdateWarranty() {

            if ($("#hdnReceiptChallanID").val() == "") {
                jAlert("Please enter valid serial no.", "Alert", function () {
                    setTimeout(function () {
                        $("#txtSerialNo").focus();
                        return;
                    }, 200);
                });
                return
            }
            if ($("#hdnEntryID").val() == "") {
                jAlert("Please enter valid serial no.", "Alert", function () {
                    setTimeout(function () {
                        $("#txtSerialNo").focus();
                        return;
                    }, 200);
                });
                return
            }

            if ($("#hdnEntryDtlsId").val() == "") {
                jAlert("Please enter valid serial no.", "Alert", function () {
                    setTimeout(function () {
                        $("#txtSerialNo").focus();
                        return;
                    }, 200);
                });
                return
            }


            if ($("#dtUpdateWarranty").val() == "") {
                jAlert("Please select Update Warranty Date.", "Alert", function () {
                    setTimeout(function () {
                        $("#dtUpdateWarranty").focus();
                        return;
                    }, 200);
                });
                return
            }
            if ($("#txtRemarks").val() == "") {
                jAlert("Please enter Remarks.", "Alert", function () {
                    setTimeout(function () {
                        $("#txtRemarks").focus();
                        return;
                    }, 200);
                });
                return
            }



            var data = {
                ReceiptChallan_ID: $("#hdnReceiptChallanID").val(),
                SrvEntryID: $("#hdnEntryID").val(),
                SrvEntryDtlsId: $("#hdnEntryDtlsId").val(),
                ReceiptChallanNo: $("#txtReceiptChallanNo").val(),
                SerialNo: $("#txtdtlsSerialNo").val(),
                NewSerialNo: $("#txtNewSerialNo").val(),
                Old_WarrantyDate: $("#txtWarrantyDate").val(),
                UpdateWarrantyDate: $("#dtUpdateWarranty").val(),
                Remarks: $("#txtRemarks").val(),
                WarrantyUpdateId: $("#hdnWarrantyUpdateId").val(),
                AddEditAction: $("#hdnAddEditAction").val(),
                // Mantis Issue 24290
                WarrentyStatusID: $("#ddlWarrentyStatus").val()
            // End of Mantis Issue 24290
            }
            $.ajax({
                type: "POST",
                url: "WarrantyEntry.aspx/UpdateWarranty",
                data: JSON.stringify({ data: data }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    if (list == "Sucess") {
                        jAlert("Warranty Update successful.", "Alert", function () {
                            $("#txtRemarks").val("");
                            $("#txtReceiptChallanNo").val("");
                            $("#txtDate").val("");
                            $("#txtEntityCode").val("");
                            $("#txtNetworkName").val("");
                            $("#txtdtlsSerialNo").val("");
                            $("#txtNewSerialNo").val("");
                            $("#txtWarrantyStatus").val("");
                            $("#txtProblemFound").val("");
                            $("#txtWarrantyDate").val("");
                            $("#hdnReceiptChallanID").val("");
                            $("#hdnEntryID").val("");
                            $("#hdnEntryDtlsId").val("");
                            $("#hdnWarrantyUpdateId").val("");
                            $("#txtSerialNo").val("");
                            // Mantis Issue 24290
                            $("#ddlWarrentyStatus").val("");
                            // End of Mantis Issue 24290
                            window.location.href = "WarrantyList.aspx";
                        });

                    } else {
                        jAlert("Please try again later.");
                    }

                }
            });
        }

// Mantis Issue 24290
        var dllval = "";
        function WarrantyChange() {
            if ($("#dtUpdateWarranty").val() != "" ) {
                var today = new Date();

                today.setHours(0, 0, 0, 0);

                var dateSplit = document.getElementById("dtUpdateWarranty").value.split('-'),
                    dateTask = new Date(dateSplit[2], dateSplit[1] - 1, dateSplit[0], 0, 0, 0, 0);

                if (dateTask.getTime() < today.getTime()) {
                    $("#ddlWarrentyStatus").val(2);
                    document.getElementById("ddlWarrentyStatus").disabled = true;
                }
                else {
                    if (dllval == "")
                        $("#ddlWarrentyStatus").val(1);
                    else {
                        $("#ddlWarrentyStatus").val(dllval);
                        dllval = "";
                    }
                    document.getElementById("ddlWarrentyStatus").disabled = false;
                }
            }
            else {
                // $("#ddlWarrentyStatus").val(0).trigger('change');
                if (dllval == "")
                    $("#ddlWarrentyStatus").val(0);
                else {
                    $("#ddlWarrentyStatus").val(dllval);
                    dllval = "";
                }

                document.getElementById("ddlWarrentyStatus").disabled = false;
            }
        }
// End of Mantis Issue 24290