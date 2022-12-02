
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

            $(".date").datepicker({
                autoclose: true,
                todayHighlight: true,
                format: 'dd-mm-yyyy'
            }).datepicker('update', new Date());
        });
$(function () {

});

    $(document).ready(function () {
        if ($("#hdnDocumentType").val() == "Add") {
            ViewDetails();
        }
        else {
            ViewDetailsEdit();
        }
            
        //$('.navbar-minimalize').click(function () {
        //    consoloe.log('togg')
        //    $('#dataTable').DataTable().draw();
        //})

            
    });

function ViewDetailsEdit() {
    $.ajax({
        type: "POST",
        url: "DeliveryEntry.aspx/ViewDetailsEdit",
        data: JSON.stringify({ ReceiptID: $("#hdnReceiptChallanID").val() }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var list = msg.d;
            $("#tdChallanNo").text(msg.d.DocumentNumber);
            $("#tdEntityCode").text(msg.d.EntityCode);
            $("#tdNetworkName").text(msg.d.NetworkName);
            $("#tdContactPerson").text(msg.d.ContactPerson);
            $("#tdReceivedOn").text(msg.d.ReceivedOn);
            $("#tdReceivedBy").text(msg.d.ReceivedBy);
            $("#tdAssignedTo").text(msg.d.AssignedTo);
            $("#tdAssignedBy").text(msg.d.AssignedBy);
            $("#tdAssignedOn").text(msg.d.AssignedOn);

            $("#txtDeliveredTo").val(msg.d.DeliveryTo);
            $("#txtPhoneNo").val(msg.d.ContactNo);
            $("#txtRemarks").val(msg.d.Remarks);

            $("#hdnAttachmentFile").val(msg.d.Attachment);
            if (msg.d.Attachment!="") {
                $("#btnView").removeClass('hide');
            }

            if (msg.d.isRcptChallanNotReceived == "1") {
                $('#chkReceiptChallan').prop('checked', true);
            }
                   
            $("#txtChallanRemarks").val(msg.d.ReceiptRemarks);
            $("#hdnDeliveryId").val(msg.d.DeliveryID);

            var status = "<div class=''>";
            status = status + "<table class=' display' id='dataTable' >";
            status = status + " <thead><tr>";
            status = status + " <th>Device Type</th><th>Model No</th><th>Serial No</th><th>Problem Found</th><th>Service Action</th>";
            status = status + " <th>New Serial No</th><th>Warranty</th><th>AC Cord / Adapter</th><th>Remote</th></tr></thead>";
            status = status + " </table></div>";

            $('#DivDetailsTable').html(status);
            $('#dataTable').DataTable({
                        
                //"scrollY": "170px",
                //"scrollCollapse": true,
                "jQueryUI": true,
                "bSort" : false,
                //"scrollCollapse": true,
                data: msg.d.DetailsList,
                columns: [
                   { 'data': 'DeviceType' },
                   { 'data': 'Model' },
                   { 'data': 'DeviceNumber' },
                   { 'data': 'ProblemDesc' },
                   { 'data': 'SrvActionDesc' },
                   { 'data': 'NewSerialNo' },
                   { 'data': 'Warrenty' },
                   { 'data': 'CordAdaptor_Status' },
                   { 'data': 'Remote_Status' },
                ],
            });

            $('#dataTable').DataTable().draw();
        }
    });
}

function ViewDetails() {
    $.ajax({
        type: "POST",
        url: "DeliveryEntry.aspx/ReceptDetails",
        data: JSON.stringify({ ReceiptID: $("#hdnReceiptChallanID").val() }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var list = msg.d;
            $("#tdChallanNo").text(msg.d.DocumentNumber);
            $("#tdEntityCode").text(msg.d.EntityCode);
            $("#tdNetworkName").text(msg.d.NetworkName);
            $("#tdContactPerson").text(msg.d.ContactPerson);
            $("#tdReceivedOn").text(msg.d.ReceivedOn);
            $("#tdReceivedBy").text(msg.d.ReceivedBy);
            $("#tdAssignedTo").text(msg.d.AssignedTo);
            $("#tdAssignedBy").text(msg.d.AssignedBy);
            $("#tdAssignedOn").text(msg.d.AssignedOn);

            var status = "<div class=''>";
            status = status + "<table class=' display' id='dataTable'>";
            status = status + " <thead><tr>";
            status = status + " <th>Device Type</th><th>Model No</th><th>Serial No</th><th style='300px'>Problem Found</th><th>Service Action</th>";
            status = status + " <th>New Serial No</th><th>Warranty</th><th>AC Cord / Adapter</th><th>Remote</th></tr></thead>";
            status = status + "</table></div>";

            $('#DivDetailsTable').html(status);
            $('#dataTable').DataTable({
                        
                //"scrollY": "170px",
                //"scrollCollapse": true,
                "bSort": false,
                "jQueryUI":       true,
                data: msg.d.DetailsList,
                columns: [
                   { 'data': 'DeviceType' },
                   { 'data': 'Model' },
                   { 'data': 'DeviceNumber' },
                   { 'data': 'ProblemDesc' },
                   { 'data': 'SrvActionDesc' },
                   { 'data': 'NewSerialNo' },
                   { 'data': 'Warrenty' },
                   { 'data': 'CordAdaptor_Status' },
                   { 'data': 'Remote_Status' },
                ],
            });
        }
    });
}

function OnSubmit() {
    LoadingPanel.Show();
    var chkReceiptChallan = "0";
    if ($('#chkReceiptChallan').is(':checked') == true) {
        chkReceiptChallan = "1";
    }

    if ($("#txtDeliveredTo").val() == "") {
        jAlert("Please enter Delivery To.", "Alert", function () {
            setTimeout(function () {
                LoadingPanel.Hide();

                $("#txtDeliveredTo").focus();
                return
            }, 200);
        });
        return
    }

    if ($("#txtPhoneNo").val() == "") {
        jAlert("Please enter phone no.", "Alert", function () {
            setTimeout(function () {
                LoadingPanel.Hide();

                $("#txtPhoneNo").focus();
                return
            }, 200);
        });
        return
    }

    //if ($("#txtRemarks").val() == "") {
    //    jAlert("Please enter remarks.", "Alert", function () {
    //        setTimeout(function () {
    //            $("#txtRemarks").focus();
    //            return
    //        }, 200);
    //    });
    //    return
    //}

    if (chkReceiptChallan == "1") {
        if ($("#txtChallanRemarks").val() == "") {
            jAlert("Please Enter Receipt Challan Remarks.", "Alert", function () {
                setTimeout(function () {
                    LoadingPanel.Hide();

                    $("#txtChallanRemarks").focus();
                    return
                }, 200);
            });
            return
        }
    }

    var file=""
    if (document.querySelector('#ReceiptChallanDoc').files.length > 0) {
        file = document.querySelector('#ReceiptChallanDoc').files[0];
               
    }

    var AttachDocument = ""
    if (file != "") {
        SaveAttachment();
        AttachDocument = "/Documents/DeliveryFile/"+file.name;
    }

    var hdnDeliveryId= $("#hdnDeliveryId").val();
    var DocumentType = $("#hdnDocumentType").val();
    var txtDeliveredTo = $("#txtDeliveredTo").val();
    var txtPhoneNo = $("#txtPhoneNo").val();
    var txtRemarks = $("#txtRemarks").val();
    var txtChallanRemarks = $("#txtChallanRemarks").val();
           
    var recept_id = $("#hdnReceiptChallanID").val();
    $.ajax({
        type: "POST",
        url: "DeliveryEntry.aspx/SaveDelivery",
        data: JSON.stringify({ DeliveredTo: txtDeliveredTo, PhoneNo: txtPhoneNo, Remarks: txtRemarks, chkReceiptChallan: chkReceiptChallan, ChallanRemarks: txtChallanRemarks, ReceiptChallanDoc: AttachDocument, recept_id: recept_id, DocumentType: DocumentType, DeliveryId: hdnDeliveryId }),
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            LoadingPanel.Hide();

            //console.log(response);
            if (response.d) {
                if (response.d.split('~')[1] == "Sucess") {
                    jAlert(response.d.split('~')[0], "Alert", function () {
                        if (DocumentType == "Add") {
                            if ($("#hdnOnlinePrint").val() == "Yes") {
                                window.open("../../../OMS/Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=DeliveryChallan~D&modulename=DELIVERYCHALLAN&id=" + response.d.split('~')[2], '_blank')
                            }
                        }
                        window.location.href = "DeliveryList.aspx";
                    });
                }
                else {
                    jAlert(response.d.split('~')[0]);
                    return
                }
            }
        },
        error: function (response) {
            console.log(response);
        }
    });
}

var filenames = [];
function SaveAttachment() {
    var process = 0;

    var fileUploadCheck = $("#ReceiptChallanDoc").get(0);
    var docFileName = "Delivery";
    var txtDeliveredTo = $("#txtDeliveredTo").val();
    var txtPhoneNo = $("#txtPhoneNo").val();

    for (var i = 0; i < $('#ReceiptChallanDoc').get(0).files.length; ++i) {
        filenames.push($('#ReceiptChallanDoc').get(0).files[i].name);
    }

           
    if (process == 0) {
        if (txtDeliveredTo != null && txtDeliveredTo != "" && txtPhoneNo != null && txtPhoneNo != "") {
            if (window.FormData !== undefined) {
                var fileUpload = $("#ReceiptChallanDoc").get(0);
                var files = fileUpload.files;
                var fileData = new FormData();

                for (var i = 0; i < files.length; i++) {
                    fileData.append(files[i].name, files[i]);
                }
                       
                fileData.append('docFileName', docFileName);

                $.ajax({
                    type: "POST",
                    url: '/SRVFileuploadDelivery/AttachmentDocumentAddUpdate',
                    contentType: false,
                    processData: false,
                    data: fileData,
                    success: function (response) {

                        if (response) {
                            // jAlert("Documents Added Successfully!");
                        }
                        else {
                            jAlert("Upload failed!");
                            return
                        }
                    }
                });
            } else {
                jAlert("Upload failed Please try again later!");
                return
            }
        }
        else {
            jAlert("Upload failed Please try again later!");
            return
        }
    }

}


function Cancel() {
    window.location.href = "DeliveryList.aspx";
}

function ViewAttachment(url) {
    $('#AttachmentFilesModal').modal('show');

    var extension = url.substr((url.lastIndexOf('.') + 1)).toLowerCase();
    switch (extension) {
        case 'pdf':
            $('#pdfview').attr('data', url);
            $('#imageview').hide();
            $('#pdfview').show();
            break;
        case 'jpg':
        case 'png':
        case 'gif':
        case 'jpeg':
            $('#imageview').attr('src', url);
            $('#pdfview').hide();
            $('#imageview').show();
            break;
        default:
            $('#AttachmentFilesModal').modal('hide');
            window.open(
               url,
              '_blank'
            );
    }


}

function AttachmentView() {
    ViewAttachment($("#hdnAttachmentFile").val());
}
