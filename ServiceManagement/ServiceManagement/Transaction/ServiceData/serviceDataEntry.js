
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
                    rightColumns: 1
                }
            });
            $('#dataTable2').DataTable({
            });
            $('#dataTable3').DataTable({
            });
            $('[data-toggle="tooltip"]').tooltip();

            $(".date").datepicker({
                autoclose: true,
                todayHighlight: true,
                format: 'dd-mm-yyyy',
                onSelect: function (dateText) {
                    console.log("Selected date: " + dateText + "; input's current value: " + this.value);
                }



            }).datepicker('update', new Date());

            $(".date").focusout(function () {
                WarrantyChange();
            });



            setTimeout(function () {
                $('.multi').multiselect({
                    //  includeSelectAllOption: true,
                    // enableFiltering:true,
                    maxHeight: 200,
                    buttonText: function (options) {
                        if (options.length == 0) {
                            return 'None selected';
                        } else {
                            var selected = 0;
                            options.each(function () {
                                selected += 1;
                            });
                            return selected + ' Selected ';
                        }
                    }
                });
            }, 1000);

            gridComponentLookup.SetEnabled(false);
        });

        
                $(document).ready(function () {
                    //if ($("#hdnParentStatus").val() == "Yes") {
                    //    $("#divcross").addClass("hide");
                    //}
                    //else {
                    //    $("#divcross").removeClass("hide");
                    //}
                    document.getElementById("ddlStockEntry").disabled = true;
                    document.getElementById("ddlModel").disabled = true;
                    document.getElementById("txtNewSerialNo").disabled = true;
                    cddlReturnReason.SetEnabled(false);
                    //  document.getElementById("ddlComponent").disabled = true;
                    gridComponentLookup.SetEnabled(false);

                    cddlServiceAction.SetEnabled(false);

                    document.getElementById("ddlWarrentyStatus").disabled = true;
                    document.getElementById("chkBillable").disabled = true;
                    document.getElementById("txtRemarks").disabled = true;
                    document.getElementById("ddlProblemFound").disabled = true;
                    document.getElementById("dtWarrenty").disabled = true;
                    // Mantis Issue 25172
                    document.getElementById("ddlLevel").disabled = true;
                    // End of Mantis Issue 25172

                    $('#dtWarrenty').prop('disabled', true);

                    BindJobDetails();

                    $('#detailsModalComponent').on('shown.bs.modal', function (e) {
                        // do something...

                    })
                });

        function BindJobDetails() {
            $.ajax({
                type: "POST",
                url: "serviceDataEntry.aspx/JobDetails",
                data: JSON.stringify({ model: $("#hdnReceiptChalanID").val() }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    //
                    var status = "<table id='dataTableList' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                    status = status + " <thead><tr>";
                    status = status + " <th style='width:205px'>Model No &nbsp; &nbsp; </th><th>Serial No</th><th>Problem Reported </th><th>Service Action</th><th>Component</th>";
                    status = status + " <th>Warranty </th><th>Stock Entry </th><th>New Model</th><th>New Serial No</th><th>Problem Found</th>";
                    status = status + " <th>Remarks</th><th>Warranty Status</th><th>Return Reason</th><th>Billable</th><th>Reason</th><th>Level &nbsp; &nbsp; &nbsp;</th><th>Action &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</th>";
                    status = status + " </tr></thead></table>";
                    $('#divListData').html(status);


                    $('#dataTableList').DataTable({

                        data: msg.d,
                        "ordering": false,
                        columns: [
                           { 'data': 'Rcpt_Model' },
                           { 'data': 'SerialNo' },
                           { 'data': 'ProblemReported' },
                           { 'data': 'ServiceAction' },
                           { 'data': 'Component' },
                           { 'data': 'Warrenty' },
                           { 'data': 'StockEntry' },
                           { 'data': 'Entry_Model' },
                           { 'data': 'NewSerialNo' },
                           { 'data': 'ProblemFound' },
                           { 'data': 'Remarks' },
                           { 'data': 'WarrentyStatus' },
                           { 'data': 'ReturnReason' },
                           { 'data': 'Billable' },
                           { 'data': 'Reason' },
                           // Mantis Issue 25172
                           { 'data': 'LevelDesc' },
                           // End of Mantis Issue 25172
                           { 'data': 'Action' },
                        ],
                        scrollX: true,
                        fixedColumns: {
                            leftColumns: 1,
                            rightColumns: 1
                        },
                        error: function (error) {
                            alert(error);
                        }
                    });
                }
            });
        }
        var dllval = "";
        function ddlServiceAction_change() {
            cddlReturnReason.SetEnabled(false);
            // document.getElementById("ddlComponent").disabled = true;
            gridComponentLookup.SetEnabled(false);
            var ServiceAction = cddlServiceAction.GetText();
            if (ServiceAction == "Exchanged") {
                document.getElementById("ddlStockEntry").disabled = false;
                document.getElementById("ddlModel").disabled = true;
                $("#ddlModel").val('0');
                document.getElementById("txtNewSerialNo").disabled = false;
            }
            else if (ServiceAction == "Upgradation") {
                document.getElementById("ddlStockEntry").disabled = false;
                document.getElementById("ddlModel").disabled = false;
                document.getElementById("txtNewSerialNo").disabled = false;
            }
            else if (ServiceAction == "Degradation") {
                document.getElementById("ddlStockEntry").disabled = false;
                document.getElementById("ddlModel").disabled = false;
                document.getElementById("txtNewSerialNo").disabled = false;
            }
            //Mantis Issue 24495
            else if (ServiceAction == "PNF") {
                

                document.getElementById("dtWarrenty").disabled = false;
                document.getElementById("ddlProblemFound").disabled = false;
                document.getElementById("txtRemarks").disabled = false;
                document.getElementById("ddlWarrentyStatus").disabled = false;

                //document.getElementById("ddlComponent").disabled = true;
                $("#ddlComponent").attr("disabled", true);
                //document.getElementById("ddlStockEntry").disabled = true;
                $("#ddlStockEntry").attr("disabled", true);
                //document.getElementById("ddlModel").disabled = true;
                $("#ddlModel").attr("disabled", true);
                //document.getElementById("txtNewSerialNo").disabled = true;
                $("#txtNewSerialNo").attr("disabled", true);
                //document.getElementById("ddlReturnReason").disabled = true;
                $("#ddlReturnReason").attr("disabled", true);
                //document.getElementById("chkBillable").disabled = true;
                $("#chkBillable").attr("disabled", true);
                $("#txtReason").attr("disabled", true);

                // Mantis Issue 25172
                document.getElementById("ddlLevel").disabled = false;
                // End of Mantis Issue 25172

                //document.getElementById("dtWarrenty").removeAttribute("disabled");
                //document.getElementById("ddlProblemFound").removeAttribute("disabled");
                //document.getElementById("txtRemarks").removeAttribute("disabled");
                //document.getElementById("ddlWarrentyStatus").removeAttribute("disabled");
            }
                //End of Mantis Issue 24495 
            else {
                document.getElementById("ddlStockEntry").disabled = true;
                document.getElementById("ddlModel").disabled = true;
                document.getElementById("txtNewSerialNo").disabled = true;

                $("#ddlStockEntry").val('0');
                $("#ddlModel").val('0');
                $("#txtNewSerialNo").val('');
            }

            if (ServiceAction == "Returned") {
                cddlReturnReason.SetEnabled(true);

            }
            if (ServiceAction == "Repaired") {
                // document.getElementById("ddlComponent").disabled = false;
                gridComponentLookup.SetEnabled(true);
                cComponentPanel.PerformCallback('BindComponentModelGrid' + '~' + $("#tdModel").text());
            }
            else {
                gridComponentLookup.gridView.UnselectRows();
            }

            WarrantyChanges(dllval);
        }

        function WarrantyChanges(val) {
            if ($("#dtWarrenty").val() != "" && cddlServiceAction.GetText() != "Returned") {
                var today = new Date();

                today.setHours(0, 0, 0, 0);

                var dateSplit = document.getElementById("dtWarrenty").value.split('-'),
                    dateTask = new Date(dateSplit[2], dateSplit[1] - 1, dateSplit[0], 0, 0, 0, 0);

                if (dateTask.getTime() < today.getTime()) {
                    $("#ddlWarrentyStatus").val(2).trigger('change');
                    document.getElementById("ddlWarrentyStatus").disabled = true;
                    document.getElementById("txtReason").disabled = false;
                }
                else {
                    $("#ddlWarrentyStatus").val(1).trigger('change');
                    document.getElementById("ddlWarrentyStatus").disabled = false;
                    document.getElementById("txtReason").disabled = true;
                }
            }
            else {
                if (val == "")
                    $("#ddlWarrentyStatus").val(0).trigger('change');
                else {
                    $("#ddlWarrentyStatus").val(val).trigger('change');
                    dllval = "";
                }
                document.getElementById("ddlWarrentyStatus").disabled = false;
            }
            var ServiceAction = cddlServiceAction.GetText();
            //Mantis Issue 24495
            if (ServiceAction == "PNF") {
                document.getElementById("chkBillable").disabled = true;
            }
            //End of Mantis Issue 24495
        }

        function AssignServiceEntry(value) {
            unselectAllProduct();
            $("#hdnReceiptChalanDtlsID").val(value);
            $.ajax({
                type: "POST",
                url: "serviceDataEntry.aspx/ServiceEntry",
                data: JSON.stringify({ ReceptDtlsID: value, ReceptID: $("#hdnReceiptChalanID").val() }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var list = msg.d;

                    // cComponentPanel.PerformCallback('BindComponentModelGrid' + '~' + msg.d.Rcpt_Model);
                    cddlServiceAction.SetValue(msg.d.ServiceAction);
                    cddlServiceAction.SetEnabled(true);

                    gridComponentLookup.SetEnabled(false);
                    document.getElementById("ddlWarrentyStatus").disabled = false;
                    document.getElementById("chkBillable").disabled = false;
                    document.getElementById("txtRemarks").disabled = false;
                    document.getElementById("ddlProblemFound").disabled = false;
                    document.getElementById("dtWarrenty").disabled = false;
                    // Mantis Issue 25172
                    document.getElementById("ddlLevel").disabled = false;
                    // End of Mantis Issue 25172

                    var ServiceAction = msg.d.ServiceActionText;
                    if (ServiceAction == "Exchanged") {
                        document.getElementById("ddlStockEntry").disabled = false;
                        document.getElementById("ddlModel").disabled = true;
                        document.getElementById("txtNewSerialNo").disabled = false;
                    }
                    else if (ServiceAction == "Upgradation") {
                        document.getElementById("ddlStockEntry").disabled = false;
                        document.getElementById("ddlModel").disabled = false;
                        document.getElementById("txtNewSerialNo").disabled = false;
                    }
                    else if (ServiceAction == "Degradation") {
                        document.getElementById("ddlStockEntry").disabled = false;
                        document.getElementById("ddlModel").disabled = false;
                        document.getElementById("txtNewSerialNo").disabled = false;
                    }
                        //Mantis Issue 24495
                    else if (ServiceAction == "PNF") {
                        document.getElementById("dtWarrenty").disabled = false;
                        document.getElementById("ddlProblemFound").disabled = false;
                        document.getElementById("txtRemarks").disabled = false;
                        document.getElementById("ddlWarrentyStatus").disabled = false;
                        // Mantis Issue 25172
                        document.getElementById("ddlLevel").disabled = false;
                        // End of Mantis Issue 25172

                        //document.getElementById("ddlComponent").disabled = true;
                        $("#ddlComponent").attr("disabled", true);
                        //document.getElementById("ddlStockEntry").disabled = true;
                        $("#ddlStockEntry").attr("disabled", true);
                        //document.getElementById("ddlModel").disabled = true;
                        $("#ddlModel").attr("disabled", true);
                        //document.getElementById("txtNewSerialNo").disabled = true;
                        $("#txtNewSerialNo").attr("disabled", true);
                        //document.getElementById("ddlReturnReason").disabled = true;
                        $("#ddlReturnReason").attr("disabled", true);
                        //document.getElementById("chkBillable").disabled = true;
                        $("#chkBillable").attr("disabled", true);
                        $("#txtReason").attr("disabled", true);
                    }
                        //End of Mantis Issue 24495
                    else {
                        document.getElementById("ddlStockEntry").disabled = true;
                        document.getElementById("ddlModel").disabled = true;
                        document.getElementById("txtNewSerialNo").disabled = true;
                    }

                    if (ServiceAction == "Returned") {
                        cddlReturnReason.SetEnabled(true);
                    }
                    if (ServiceAction == "Repaired") {
                        // document.getElementById("ddlComponent").disabled = false;
                        //gridComponentLookup.gridView.UnselectRows();
                        gridComponentLookup.SetEnabled(true);
                        cComponentPanel.PerformCallback('SetComponentGrid' + '~' + value + '~' + msg.d.Rcpt_Model);
                    }
                    else {

                    }

                    $("#tdModel").text(msg.d.Rcpt_Model);
                    $("#tdSerialNo").text(msg.d.SerialNo);
                    $("#tdProblemReported").text(msg.d.ProblemReported);

                    //$("#ddlComponent").val(msg.d.Component).trigger('change');
                    $("#hdncWiseProductId").val(msg.d.Component);
                    // ctxtProdName.SetText("");
                    //  unselectAllProduct();

                    $("#ddlStockEntry").val(msg.d.StockEntry).trigger('change');
                    $("#ddlModel").val(msg.d.Entry_Model).trigger('change');
                    $("#ddlProblemFound").val(msg.d.ProblemFound).trigger('change');
                    $("#ddlWarrentyStatus").val(msg.d.WarrentyStatus);
                    // Mantis Issue 25172
                   // $("#ddlLevel").val(msg.d.LevelID);
                    $("#ddlLevel").val(msg.d.LevelID).trigger('change');
                    // End of Mantis Issue 25172
                    dllval = msg.d.WarrentyStatus;
                    $("#txtNewSerialNo").val(msg.d.NewSerialNo);
                    $("#txtRemarks").val(msg.d.Remarks);
                    cddlReturnReason.SetValue(msg.d.ReturnReasonID);
                    if (msg.d.IsBillable == "1") {
                        $('#chkBillable').prop('checked', true);
                    }
                    if (msg.d.Warrenty != "01-01-1900") {
                        $("#dtWarrenty").val(msg.d.Warrenty);
                        $('.date').datepicker('update', msg.d.Warrenty);
                    }

                    $("#txtReason").val(msg.d.Reason);


                    $("#btnAdd").removeClass('hide');
                    $("#btnSave").addClass('hide');
                }
            });
        }

        function AddServiceEntry() {


            LoadingPanel.Show();
            var Component = "";
            var StockEntry = "";
            var Entry_Model = "";
            var ProblemFound = "";
            var WarrentyStatus = "";

            var Warrenty = null;

            var ComponentIDs = ""//gridComponentLookup.gridView.GetSelectedKeysOnPage();
            var Components = "";
            // Mantis Issue 25172
            var LevelDesc = "";
            // End of Mantis Issue 25172

            //for (var i = 0; i < ComponentIDs.length; i++) {
            //    if (Components == "") {
            //        Components = ComponentIDs[i];
            //    }
            //    else {
            //        Components += ',' + ComponentIDs[i];
            //    }
            //}

            //gridComponentLookup.gridView.GetSelectedFieldValues("ID", function (val) {
            //    Components = val;
            //});



            Component = gridComponentLookup.GetText();
            //if ($("#ddlComponent").find("option:selected").text() != "Select") {
            //    Component = $("#ddlComponent").find("option:selected").text();
            //}

            if ($("#ddlStockEntry").find("option:selected").text() != "Select") {
                StockEntry = $("#ddlStockEntry").find("option:selected").text();
            }
            if ($("#ddlModel").find("option:selected").text() != "Select") {
                Entry_Model = $("#ddlModel").find("option:selected").text();
            }
            if ($("#ddlProblemFound").find("option:selected").text() != "Select") {
                ProblemFound = $("#ddlProblemFound").find("option:selected").text();
            }
            if ($("#ddlWarrentyStatus").find("option:selected").text() != "Select") {
                WarrentyStatus = $("#ddlWarrentyStatus").find("option:selected").text();
            }

            if ($("#dtWarrenty").val() != "") {
                var warsplit = $("#dtWarrenty").val().split('-');
                Warrenty = warsplit[2] + '-' + warsplit[1] + '-' + warsplit[0];
            }

            // Mantis Issue 25172
            if ($("#ddlLevel").find("option:selected").text() != "Select") {
                LevelDesc = $("#ddlLevel").find("option:selected").text();
            }
            // End of Mantis Issue 25172

            if (cddlServiceAction.GetValue() == "0") {
                jAlert("Please select service action.");
                cddlServiceAction.Focus();
                LoadingPanel.Hide();
                suc = false;
                return
            }

            if (cddlServiceAction.GetText() == "Repaired") {
                //if ($("#ddlComponent").val() == "0") {
                //    jAlert("Please select component.");
                //    $("#ddlComponent").focus();
                //    suc = false;
                //    return
                //}

                if (Component == "") {
                    jAlert("Please select component.");
                    gridComponentLookup.Focus();
                    LoadingPanel.Hide();
                    suc = false;
                    return
                }
            }

            var ServiceAction = cddlServiceAction.GetText();
            if (ServiceAction == "Exchanged") {
                if ($("#ddlStockEntry").val() == "0") {
                    jAlert("Please select Stock Entry.");
                    $("#ddlStockEntry").focus();
                    LoadingPanel.Hide();
                    suc = false;
                    return
                }

                if ($("#txtNewSerialNo").val() == "") {
                    jAlert("Please enter new serial no.");
                    $("#txtNewSerialNo").focus();
                    LoadingPanel.Hide();
                    suc = false;
                    return
                }
            }
            else if (ServiceAction == "Upgradation") {

                if ($("#ddlStockEntry").val() == "0") {
                    jAlert("Please select Stock Entry.");
                    LoadingPanel.Hide();
                    $("#ddlStockEntry").focus();
                    suc = false;
                    return
                }

                if ($("#ddlModel").val() == "0") {
                    jAlert("Please select new Model.");
                    LoadingPanel.Hide();
                    $("#ddlModel").focus();
                    suc = false;
                    return
                }

                if ($("#txtNewSerialNo").val() == "") {
                    jAlert("Please enter new serial no.");
                    LoadingPanel.Hide();
                    $("#txtNewSerialNo").focus();
                    suc = false;
                    return
                }
            }
            else if (ServiceAction == "Degradation") {

                if ($("#ddlStockEntry").val() == "0") {
                    jAlert("Please select Stock Entry.");
                    LoadingPanel.Hide();
                    $("#ddlStockEntry").focus();
                    suc = false;
                    return
                }

                if ($("#ddlModel").val() == "0") {
                    jAlert("Please select new Model.");
                    LoadingPanel.Hide();
                    $("#ddlModel").focus();
                    suc = false;
                    return
                }

                if ($("#txtNewSerialNo").val() == "") {
                    jAlert("Please enter new serial no.");
                    LoadingPanel.Hide();
                    $("#txtNewSerialNo").focus();
                    suc = false;
                    return
                }
            }

            if ($("#ddlProblemFound").val() == "0") {
                jAlert("Please select problem found.");
                LoadingPanel.Hide();
                $("#ddlProblemFound").focus();
                suc = false;
                return
            }

            if (cddlServiceAction.GetText() == "Returned") {
                if (cddlReturnReason.GetValue() == "0") {
                    jAlert("Please Select Return Reason.");
                    LoadingPanel.Hide();
                    cddlReturnReason.Focus();
                    suc = false;
                    return
                }
            }



            if ($("#dtWarrenty").val() != "" && cddlServiceAction.GetText() != "Returned") {
                var today = new Date();
                var dd = String(today.getDate()).padStart(2, '0');
                var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
                var yyyy = today.getFullYear();

                today = dd + '-' + mm + '' + yyyy;
                if ($("#ddlWarrentyStatus").val() == "2") {
                    if ($('#chkBillable').is(':checked') == false) {
                        jAlert("Billable Mandatory.");
                        LoadingPanel.Hide();
                        $("#chkBillable").focus();
                        suc = false;
                        return
                    }
                }
            }
            else {
                if ($("#ddlWarrentyStatus").val() == "0") {
                    jAlert("Please select Warranty status.");
                    LoadingPanel.Hide();
                    $("#ddlWarrentyStatus").focus();
                    suc = false;
                    return
                }
            }

            var IsBillable = "0";
            var Billable = "No";
            if ($('#chkBillable').is(':checked') == true) {
                IsBillable = "1";
                Billable = "Yes";
            }
            var com_qty = [];

            gridComponentLookup.gridView.GetSelectedFieldValues("ID", function (val) {
                ComponentIDs = val

                for (var i = 0; i < ComponentIDs.length; i++) {
                    if (Components == "") {
                        Components = ComponentIDs[i];
                    }
                    else {
                        Components += ',' + ComponentIDs[i];
                    }
                }

                if (ComponentidwithQty != "") {
                    for (var i = 0; i < ComponentidwithQty.length; i++) {
                        if ($('#' + ComponentidwithQty[i].Productid).val() != "" && $('#' + ComponentidwithQty[i].Productid).val()>0) {
                            com_qty.push({ id: ComponentidwithQty[i].Productid, Value: $('#' + ComponentidwithQty[i].Productid).val() });
                        }
                        else {
                            jAlert("Quantity must be entered.", "Alart" ,function() {
                                LoadingPanel.Hide();
                                $("#detailsModalComponent").modal('show');
                            });
                           
                            suc = false;
                            return
                        }                        
                    }
                }
                else {
                    for (var i = 0; i < ComponentIDs.length; i++) {
                        com_qty.push({ id: ComponentIDs[i], Value: '0' });
                    }
                }

                var Apply = {
                    RcptChallan_ID: $("#hdnReceiptChalanID").val(),
                    RcptChallanDtls_ID: $("#hdnReceiptChalanDtlsID").val(),
                    ServiceAction: cddlServiceAction.GetText(),
                    Component: Component,
                    Warrenty: Warrenty,
                    StockEntry: StockEntry,
                    Entry_Model: Entry_Model,
                    NewSerialNo: $("#txtNewSerialNo").val(),
                    ProblemFound: ProblemFound,
                    Remarks: $("#txtRemarks").val(),
                    WarrentyStatus: WarrentyStatus,
                    ServiceActionID: cddlServiceAction.GetValue(),
                    //ComponentID: $("#ddlComponent").val(),
                    ComponentID: Components,
                    StockEntryID: $("#ddlStockEntry").val(),
                    Entry_ModelID: $("#ddlModel").val(),
                    ProblemFound_ID: $("#ddlProblemFound").val(),
                    WarrentyStatusID: $("#ddlWarrentyStatus").val(),
                    ReturnReasonID: cddlReturnReason.GetValue(),
                    IsBillable: IsBillable,
                    ReturnReason: cddlReturnReason.GetText(),
                    Billable: Billable,
                    Reason: $("#txtReason").val(),
                    com_qty: com_qty
                    // Mantis Issue 25172
                    , LevelID: $("#ddlLevel").val(),
                    LevelDesc: LevelDesc
                    // End of Mantis Issue 25172
                }

                $.ajax({
                    type: "POST",
                    url: "serviceDataEntry.aspx/AddNewServiceEntry",
                    data: "{model:" + JSON.stringify(Apply) + "}",
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        console.log(response);
                        if (response.d) {

                            if (response.d.split('~')[1] == "Success") {
                                LoadingPanel.Hide();
                                jAlert(response.d.split('~')[0], "Alert", function () {
                                    $("#txtRemarks").val(''),
                                    $("#ddlWarrentyStatus").val('0').trigger('change'),
                                    cddlServiceAction.SetValue(0),
                                    //  $("#ddlComponent").val('0').trigger('change'),
                                    $("#hdncWiseProductId").val("");
                                    // ctxtProdName.SetText("");
                                    unselectAllProduct();
                                    $("#ddlStockEntry").val('0').trigger('change');
                                    $("#ddlModel").val('0').trigger('change');
                                    $("#ddlProblemFound").val('0').trigger('change');
                                    $("#ddlWarrentyStatus").val('0').trigger('change');
                                    $("#txtNewSerialNo").val('');
                                    $("#tdModel").text('');
                                    $("#tdSerialNo").text('');
                                    $("#tdProblemReported").text('');
                                    $("#hdnReceiptChalanDtlsID").val('');
                                    cddlReturnReason.SetValue(0);
                                    $('#chkBillable').prop('checked', false);
                                    $(".date").datepicker('update', new Date());
                                    $("#dtWarrenty").val('');
                                    // Mantis Issue 25172
                                    $("#ddlLevel").val('').trigger('change');
                                    // End of Mantis Issue 25172

                                    document.getElementById("ddlStockEntry").disabled = true;
                                    document.getElementById("ddlModel").disabled = true;
                                    document.getElementById("txtNewSerialNo").disabled = true;
                                    cddlReturnReason.SetEnabled(false);
                                    //document.getElementById("ddlComponent").disabled = true;
                                    gridComponentLookup.SetEnabled(false);
                                    cddlServiceAction.SetEnabled(false);
                                    document.getElementById("ddlWarrentyStatus").disabled = true;
                                    document.getElementById("chkBillable").disabled = true;
                                    document.getElementById("txtRemarks").disabled = true;
                                    document.getElementById("ddlProblemFound").disabled = true;
                                    document.getElementById("dtWarrenty").disabled = true;
                                    $('#dtWarrenty').prop('disabled', true);
                                    BindJobDetails();
                                    $("#btnSave").removeClass('hide');
                                    $("#btnAdd").addClass('hide');
                                    ComponentidwithQty = "";
                                    com_qty = [];
                                    // Mantis Issue 25172
                                    document.getElementById("ddlLevel").disabled = true;
                                    // End of Mantis Issue 25172
                                });
                            }
                            else {
                                if (response.d.split('~')[0]=='Stock will be negative. Can not proceed.') {
                                    ComponentidwithQty = "";
                                    com_qty = [];
                                }
                                jAlert(response.d.split('~')[0]);
                                LoadingPanel.Hide();
                                return
                            }
                        }
                    },
                    error: function (response) {
                        LoadingPanel.Hide();
                        console.log(response);
                    }
                });
            });
        }

        function SaveServiceEntry() {
            LoadingPanel.Show();
            var sendSMS = "No";
            if ($('#chkSendSMS').is(':checked') == true) {
                sendSMS = "Yes";
            }
            $.ajax({
                type: "POST",
                url: "serviceDataEntry.aspx/SaveServiceEntry",
                data: JSON.stringify({ ReceptID: $("#hdnReceiptChalanID").val(), EntryMode: $("#hdnEntryMode").val(), sendSMS: sendSMS }),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    console.log(response);
                    if (response.d) {
                        if (response.d == "Saved Successfully.") {
                            LoadingPanel.Hide();
                            if ($("#hdnParentStatus").val() == "Yes") {

                                jAlert("Saved Successfully.", "Alert", function () {
                                    parent.MethodAssignJob();
                                });
                            }
                            else {
                                jAlert("Saved Successfully.", "Alert", function () {
                                    window.location.href = "serviceDataList.aspx";
                                });
                            }
                        } else if (response.d == "Update Successfully.") {
                            LoadingPanel.Hide();
                            jAlert("Update Successfully.", "Alert", function () {
                                window.location.href = "serviceDataList.aspx";
                            });
                        }
                        else {
                            jAlert(response.d);
                            LoadingPanel.Hide();
                            return
                        }
                    }
                },
                error: function (response) {
                    console.log(response);
                    LoadingPanel.Hide();
                }
            });
        }

        function BindServiceEntryHistory() {
            var status = "<table class='table table-striped table-bordered tableStyle' id='dataTable'>";
            status = status + " <thead><tr>";
            status = status + " <th>Entity Code</th><th>Ref. Receipt No.</th>";
            status = status + " <th>Service Action</th><th>Remarks</th><th>Billable</th></tr></thead>";
            status = status + " </table>";
            $('#DivHistoryTable').html(status);
            setTimeout(function () {
                $.ajax({
                    type: "POST",
                    url: "serviceDataEntry.aspx/ServiceEntryHistory",
                    data: JSON.stringify({ model: $("#tdModel").text(), DeviceNumber: $("#tdSerialNo").text() }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {

                        $('#dataTable').DataTable({
                            //scrollX: true,
                            //fixedColumns: {
                            //    rightColumns: 2
                            //},
                            data: msg.d,
                            columns: [
                               { 'data': 'EntityCode' },
                               { 'data': 'ReceiptNo' },
                               { 'data': 'ServiceAction' },
                               { 'data': 'Remarks' },
                               { 'data': 'Billable' },
                            ],

                            error: function (error) {
                                alert(error);
                            }
                        });
                    }
                });
            }, 1000);
        }

        function Cancel() {
            if ($("#hdnParentStatus").val() == "Yes") {
                parent.MethodAssignJob();
            }
            else {
                window.location.href = "serviceDataList.aspx";
            }
        }
        
            function Delete(values) {
                var receptId = $("#hdnReceiptChalanID").val();
                jConfirm('Confirm Delete?', 'Alert', function (r) {
                    if (r) {
                        $.ajax({
                            type: "POST",
                            url: "serviceDataEntry.aspx/DeleteLineServiceEntry",
                            data: JSON.stringify({ ReceptDtlsID: values, ReceptID: $("#hdnReceiptChalanID").val() }),
                            async: false,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                console.log(response);
                                if (response.d.split('~')[1] == "Success") {
                                    jAlert(response.d.split('~')[0], "Alert", function () {
                                        BindJobDetails();
                                    });
                                }
                                else {
                                    jAlert(response.d.split('~')[0]);
                                    return
                                }
                            },
                            error: function (response) {
                                console.log(response);
                            }
                        });
                    }
                });
            }
        
            $(function () {
                //cComponentPanel.PerformCallback('BindComponentGrid' + '~' + "All");
            });

        function selectAllProduct() {
            gridComponentLookup.gridView.SelectRows();
        }
        function unselectAllProduct() {
            gridComponentLookup.gridView.UnselectRows();
        }
        function CloseProductLookup() {
            gridComponentLookup.ConfirmCurrentSelection();
            gridComponentLookup.HideDropDown();
            gridComponentLookup.Focus();
        }

        function WarrantyChange() {
            if ($("#dtWarrenty").val() != "" && cddlServiceAction.GetText() != "Returned") {
                var today = new Date();

                today.setHours(0, 0, 0, 0);

                var dateSplit = document.getElementById("dtWarrenty").value.split('-'),
                    dateTask = new Date(dateSplit[2], dateSplit[1] - 1, dateSplit[0], 0, 0, 0, 0);

                if (dateTask.getTime() < today.getTime()) {
                    $("#ddlWarrentyStatus").val(2).trigger('change');
                    document.getElementById("ddlWarrentyStatus").disabled = true;
                }
                else {
                    if (dllval == "")
                        $("#ddlWarrentyStatus").val(1).trigger('change');
                    else {
                        $("#ddlWarrentyStatus").val(dllval).trigger('change');
                        dllval = "";
                    }
                    document.getElementById("ddlWarrentyStatus").disabled = false;
                }
            }
            else {
                // $("#ddlWarrentyStatus").val(0).trigger('change');
                if (dllval == "")
                    $("#ddlWarrentyStatus").val(0).trigger('change');
                else {
                    $("#ddlWarrentyStatus").val(dllval).trigger('change');
                    dllval = "";
                }

                document.getElementById("ddlWarrentyStatus").disabled = false;
            }
        }

        function ddlWarrentyStatus_Change() {
            if ($("#ddlWarrentyStatus").val() == "2") {
                document.getElementById("txtReason").disabled = false;
            }
            else {
                document.getElementById("txtReason").disabled = true;
                $("#txtReason").val('');
            }
        }
        
            var ComponentidwithQty = "";
        function ServiceEntryAdd() {
            var Component = "";
            var StockEntry = "";
            var Entry_Model = "";
            var ProblemFound = "";
            var WarrentyStatus = "";

            var Warrenty = null;

            Component = gridComponentLookup.GetText();

            if ($("#dtWarrenty").val() != "") {
                var warsplit = $("#dtWarrenty").val().split('-');
                Warrenty = warsplit[2] + '-' + warsplit[1] + '-' + warsplit[0];
            }

            if (cddlServiceAction.GetValue() == "0") {
                jAlert("Please select service action.");
                cddlServiceAction.Focus();
                LoadingPanel.Hide();
                suc = false;
                return
            }

            if (cddlServiceAction.GetText() == "Repaired") {

                if (Component == "") {
                    jAlert("Please select component.");
                    gridComponentLookup.Focus();
                    LoadingPanel.Hide();
                    suc = false;
                    return
                }
            }

            var ServiceAction = cddlServiceAction.GetText();
            if (ServiceAction == "Exchanged") {
                if ($("#ddlStockEntry").val() == "0") {
                    jAlert("Please select Stock Entry.");
                    $("#ddlStockEntry").focus();
                    LoadingPanel.Hide();
                    suc = false;
                    return
                }

                if ($("#txtNewSerialNo").val() == "") {
                    jAlert("Please enter new serial no.");
                    $("#txtNewSerialNo").focus();
                    LoadingPanel.Hide();
                    suc = false;
                    return
                }
            }
            else if (ServiceAction == "Upgradation") {

                if ($("#ddlStockEntry").val() == "0") {
                    jAlert("Please select Stock Entry.");
                    LoadingPanel.Hide();
                    $("#ddlStockEntry").focus();
                    suc = false;
                    return
                }

                if ($("#ddlModel").val() == "0") {
                    jAlert("Please select new Model.");
                    LoadingPanel.Hide();
                    $("#ddlModel").focus();
                    suc = false;
                    return
                }

                if ($("#txtNewSerialNo").val() == "") {
                    jAlert("Please enter new serial no.");
                    LoadingPanel.Hide();
                    $("#txtNewSerialNo").focus();
                    suc = false;
                    return
                }
            }
            else if (ServiceAction == "Degradation") {

                if ($("#ddlStockEntry").val() == "0") {
                    jAlert("Please select Stock Entry.");
                    LoadingPanel.Hide();
                    $("#ddlStockEntry").focus();
                    suc = false;
                    return
                }

                if ($("#ddlModel").val() == "0") {
                    jAlert("Please select new Model.");
                    LoadingPanel.Hide();
                    $("#ddlModel").focus();
                    suc = false;
                    return
                }

                if ($("#txtNewSerialNo").val() == "") {
                    jAlert("Please enter new serial no.");
                    LoadingPanel.Hide();
                    $("#txtNewSerialNo").focus();
                    suc = false;
                    return
                }
            }

            if ($("#ddlProblemFound").val() == "0") {
                jAlert("Please select problem found.");
                LoadingPanel.Hide();
                $("#ddlProblemFound").focus();
                suc = false;
                return
            }

            if (cddlServiceAction.GetText() == "Returned") {
                if (cddlReturnReason.GetValue() == "0") {
                    jAlert("Please Select Return Reason.");
                    LoadingPanel.Hide();
                    cddlReturnReason.Focus();
                    suc = false;
                    return
                }
            }



            if ($("#dtWarrenty").val() != "" && cddlServiceAction.GetText() != "Returned") {
                var today = new Date();
                var dd = String(today.getDate()).padStart(2, '0');
                var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
                var yyyy = today.getFullYear();

                today = dd + '-' + mm + '' + yyyy;
                if ($("#ddlWarrentyStatus").val() == "2") {
                    if ($('#chkBillable').is(':checked') == false) {
                        jAlert("Billable Mandatory.");
                        LoadingPanel.Hide();
                        $("#chkBillable").focus();
                        suc = false;
                        return
                    }
                }
            }
            else {
                if ($("#ddlWarrentyStatus").val() == "0") {
                    jAlert("Please select Warranty status.");
                    LoadingPanel.Hide();
                    $("#ddlWarrentyStatus").focus();
                    suc = false;
                    return
                }
            }

            var ComponentIDs = "";
            var Components = "";
            if ($("#hdnComponentQty").val() == "Yes") {
                gridComponentLookup.gridView.GetSelectedFieldValues("ID", function (val) {
                    ComponentIDs = val
                    if (ComponentIDs != "") {
                        for (var i = 0; i < ComponentIDs.length; i++) {
                            if (Components == "") {
                                Components = ComponentIDs[i];
                            }
                            else {
                                Components += ',' + ComponentIDs[i];
                            }
                        }
                        $('#dataTableComponent').DataTable().destroy();
                        $('#dataTableComponent').DataTable().destroy();

                        $.ajax({
                            type: "POST",
                            url: "serviceDataEntry.aspx/ShowComponentQty",
                            data: JSON.stringify({ ComponentID: Components, RcptChallan_ID: $("#hdnReceiptChalanID").val(), RcptChallanDtls_ID: $("#hdnReceiptChalanDtlsID").val(), EntryMode: $("#hdnEntryMode").val() }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                if (msg.d != "") {
                                    ComponentidwithQty = msg.d;
                                    $("#detailsModalComponent").modal('show');
                                    $('#dataTableComponent').DataTable().destroy();
                                    //$('#dataTableComponent').DataTable().fnDestroy();
                                    var list = msg.d;
                                    var status = " <table id='dataTableComponent' class='table tableStyle table-striped table-bordered display nowrap' style='width: 100%'>";
                                    status = status + " <thead><tr>";
                                    status = status + " <th class='hide'>Product id</th><th>Product Code</th><th>Product Name</th><th>Replaceable</th><th>Quantity</th></tr></thead>";
                                    status = status + " </table>";
                                    $('#ComponentTable').html(status);

                                    $('#dataTableComponent').DataTable({
                                        retrieve: true,
                                        "ordering": false,
                                        fixedColumns: {
                                            rightColumns: 1
                                        },
                                        data: msg.d,
                                        columns: [
                                            { 'data': 'Productid' },
                                            { 'data': 'ProductCode' },
                                            { 'data': 'ProductName' },
                                            { 'data': 'Replaceable' },
                                            { 'data': 'TEXTBOX' },
                                        ],
                                        "columnDefs": [
                                        {
                                            "visible": false,
                                            "targets": 0
                                        }
                                        ]
                                    });
                                }
                                else {
                                    AddServiceEntry();
                                }
                            }
                        });
                    }
                    else {
                        AddServiceEntry();
                    }
                });
            }
            else {
                AddServiceEntry();
            }
        }


        function ComponentQty_Submit() {
            AddServiceEntry();
        }

        function validateFloatKeyPress(el, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            var number = el.value.split('.');
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            if (number.length > 1 && charCode == 46) {
                return false;
            }
            return true;
        }
       
