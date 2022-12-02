 
       $(document).ready(function () {
           debugger;
           $('#cddlSegmentMandatory1').multiselect({
               includeSelectAllOption: true,
               buttonWidth: 'auto',
               enableFiltering: true,

               maxHeight: 400,

               //dropUp: true,
               enableCaseInsensitiveFiltering: true,
               //onDropdownHide: function (event) {
               //    //console.log(event)
               //},
               onChange: function() {
                   var selected = this.$select.val();
                   // ...
                   $('#hdnSegmentMandatory1').val(selected);
                 
                    
                   console.log("selected", selected);
               },
               buttonText: function (options, select) {
                   if (options.length === 0) {
                       return 'No option selected ...';
                   }
                   else if (options.length > 2) {
                       return 'More than 2 options selected!';
                   } else {
                       var labels = [];
                       options.each(function () {
                           if ($(this).attr('label') !== undefined) {
                               labels.push($(this).attr('label'));
                           }
                           else {
                               labels.push($(this).html());
                           }
                       });
                       return labels.join(', ') + '';
                   }
               }
           }).multiselect('selectAll', false).multiselect('updateButtonText');


           $('#cddlSegmentMandatory2').multiselect({
               includeSelectAllOption: true,
               buttonWidth: '100%',
               enableFiltering: true,
               maxHeight: 200,
               //dropUp: true,
               enableCaseInsensitiveFiltering: true,
               //onDropdownHide: function (event) {
               //    //console.log(event)
               //},
               onChange: function () {
                   var selected = this.$select.val();
                   // ...
                   $('#hdnSegmentMandatory2').val(selected);

                   console.log("selected", selected);
               },
               buttonText: function (options, select) {
                   if (options.length === 0) {
                       return 'No option selected ...';
                   }
                   else if (options.length > 2) {
                       return 'More than 2 options selected!';
                   } else {
                       var labels = [];
                       options.each(function () {
                           if ($(this).attr('label') !== undefined) {
                               labels.push($(this).attr('label'));
                           }
                           else {
                               labels.push($(this).html());
                           }
                       });
                       return labels.join(', ') + '';
                   }
               }
           }).multiselect('selectAll', false).multiselect('updateButtonText');

           $('#cddlSegmentMandatory3').multiselect({
               includeSelectAllOption: true,
               buttonWidth: '100%',
               enableFiltering: true,
               maxHeight: 200,
               //dropUp: true,
               enableCaseInsensitiveFiltering: true,
               //onDropdownHide: function (event) {
               //    //console.log(event)
               //},
               onChange: function () {
                   var selected = this.$select.val();
                   // ...
                   $('#hdnSegmentMandatory3').val(selected);

                   console.log("selected", selected);
               },
               buttonText: function (options, select) {
                   if (options.length === 0) {
                       return 'No option selected ...';
                   }
                   else if (options.length > 2) {
                       return 'More than 2 options selected!';
                   } else {
                       var labels = [];
                       options.each(function () {
                           if ($(this).attr('label') !== undefined) {
                               labels.push($(this).attr('label'));
                           }
                           else {
                               labels.push($(this).html());
                           }
                       });
                       return labels.join(', ') + '';
                   }
               }
           }).multiselect('selectAll', false).multiselect('updateButtonText');

           $('#cddlSegmentMandatory4').multiselect({
               includeSelectAllOption: true,
               buttonWidth: '100%',
               enableFiltering: true,
               maxHeight: 200,               
               enableCaseInsensitiveFiltering: true,                
               onChange: function () {
                   var selected = this.$select.val();                   
                   $('#hdnSegmentMandatory4').val(selected);
                   console.log("selected", selected);
               },
               buttonText: function (options, select) {
                   if (options.length === 0) {
                       return 'No option selected ...';
                   }
                   else if (options.length > 2) {
                       return 'More than 2 options selected!';
                   } else {
                       var labels = [];
                       options.each(function () {
                           if ($(this).attr('label') !== undefined) {
                               labels.push($(this).attr('label'));
                           }
                           else {
                               labels.push($(this).html());
                           }
                       });
                       return labels.join(', ') + '';
                   }
               }
           }).multiselect('selectAll', false).multiselect('updateButtonText');
       
$('#cddlSegmentMandatory5').multiselect({
    includeSelectAllOption: true,
    buttonWidth: '100%',
    enableFiltering: true,
    maxHeight: 200,               
    enableCaseInsensitiveFiltering: true,                
    onChange: function () {
        var selected = this.$select.val();                   
        $('#hdnSegmentMandatory5').val(selected);
        console.log("selected", selected);
    },
    buttonText: function (options, select) {
        if (options.length === 0) {
            return 'No option selected ...';
        }
        else if (options.length > 2) {
            return 'More than 2 options selected!';
        } else {
            var labels = [];
            options.each(function () {
                if ($(this).attr('label') !== undefined) {
                    labels.push($(this).attr('label'));
                }
                else {
                    labels.push($(this).html());
                }
            });
            return labels.join(', ') + '';
        }
    }
}).multiselect('selectAll', false).multiselect('updateButtonText');
})
   
function CheckSegZero() {
    var Segment1 = $('#txtSegment1').val();
    if (Segment1 == "0") {
        $('#txtSegment1').focus();
        $('#txtSegment2').val("0");
        alert("Zero Value is not allowed for Segment1");
        return false;
    }
}
function CheckSegZero1() {
    var Segment2 = $('#txtSegment2').val();
    if (Segment2 == "0") {
        $('#txtSegment2').focus();
        $('#txtSegment3').val("0");
        alert("Zero Value is not allowed for Segment2");
        return false;
    }
}
function CheckSegZero2() {
    var Segment3 = $('#txtSegment3').val();
    if (Segment3 == "0") {
        $('#txtSegment3').focus();
        $('#txtSegment4').val("0");
        alert("Zero Value is not allowed for Segment3");
        return false;
    }
}
function CheckSegZero3() {
    var Segment4 = $("#txtSegment4").val();
    if (Segment4 == "0") {
        $("#txtSegment4").focus();
        $("#txtSegment5").val("0");
        alert("Zero Value is not allowed for Segment4");
        return false;
    }
}
function SaveCustomer() {
    document.getElementById("btnSaveCust").disabled = true;

    $("#uniqueId").removeClass('alert-danger');
    $("#Name").removeClass('alert-danger');
    $("#BillingAddress1").removeClass('alert-danger');
    $("#pinCode").removeClass('alert-danger');
    $("#shippingAddress1").removeClass('alert-danger');
    $("#shippingpinCode").removeClass('alert-danger');

    $("#GSTIN1").removeClass('alert-danger');
    $("#GSTIN2").removeClass('alert-danger');
    $("#GSTIN3").removeClass('alert-danger');
    var uniqueID = "";
    var NumberingId = "0";
    var NumberingVal=$("#NumScheme").val()
    if (dataval != "1") {
        uniqueID = $("#uniqueId").val();
        NumberingId="0"
    }
    else
    {
        uniqueID = $("#DocNum").val();
        NumberingId=NumberingVal.split("~")[0]
    }

    var Name = $("#Name").val();
    var BillingAddress1 = $("#BillingAddress1").val();
    var BillingAddress2 = $("#BillingAddress2").val();
    var pinCode = $("#pinCode").val();
    var Country = $("#Country").val();

    var shippingAddress1 = $("#shippingAddress1").val();
    var shippingAddress2 = $("#shippingAddress2").val();
    var shippingpinCode = $("#shippingpinCode").val();
    var shippingCountry = $("#shippingCountry").val();

    var GSTIN = $("#GSTIN1").val().trim() + $("#GSTIN2").val().trim() + $("#GSTIN3").val().trim();

    var BillingPhone = $("#BillingPhone").val().trim();
    var ShippingPhone = $("#ShippingPhone").val().trim();
    var contactperson = $("#contactperson").val().trim();

    var GrpCust = $("#GroupCust option:selected").val();
    //Add rev for Transcation Category Tanmoy
    var TransactionCategory = $("#TransactionCategory option:selected").val();
    //Add rev for Transcation Category Tanmoy

    var IdType = document.getElementById("IdType");
    var IdTypeval = IdType.options[IdType.selectedIndex].value;
    var TCSApplicable = false;
    if (document.getElementById("chkTCSAppli").checked == true) {
        TCSApplicable = true;
    }

    if (dataval != "1")
    {
        if (IdTypeval == 1) {
            if (isNaN(uniqueID)) {
                jAlert("Please type valid Phone No.");
                document.getElementById("btnSaveCust").disabled = false;
                return;
            }
        } else if (IdTypeval == 2) {

            var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
            var code = /([C,P,H,F,A,T,B,L,J,G])/;
            var code_chk = uniqueID.substring(3, 4);
            if (uniqueID.search(panPat) == -1) {
                jAlert("Please type valid PAN No.");
                document.getElementById("btnSaveCust").disabled = false;
                return;
            }
            if (code.test(code_chk) == false) {
                jAlert("Please type valid PAN No.");
                document.getElementById("btnSaveCust").disabled = false;
                return;
            }

        } else if (IdTypeval == 3) {
            if (isNaN(uniqueID) || uniqueID.length != 12) {
                document.getElementById("btnSaveCust").disabled = false;
                jAlert("Please type valid Aadhar No.");
                return;
            }
        }

        if (uniqueID.trim() == "") {
            document.getElementById("btnSaveCust").disabled = false;
            $("#uniqueId").addClass('alert-danger');
            $("#uniqueId").focus();
            return;
        }

    }
   
    if (dataval == "1")
    {
        var SchemeVal = $("#NumScheme").val();
        var uccName = $("#DocNum").val();
        if(SchemeVal=="0")
        {
            document.getElementById("btnSaveCust").disabled = false;
            jAlert('Please Select Numbering Scheme');
            return;
        }
        if(uccName=="")
        {
            document.getElementById("btnSaveCust").disabled = false;
            jAlert('Please Enter Unique Id.');
            $("#DocNum").focus();
            return;
        }
    }


    if (Name.trim() == "") {
        document.getElementById("btnSaveCust").disabled = false;
        $("#Name").addClass('alert-danger');
        $("#Name").focus();

        return;
    }
    else if (BillingAddress1.trim() == "") {
        document.getElementById("btnSaveCust").disabled = false;
        $("#BillingAddress1").addClass('alert-danger');
        $("#BillingAddress1").focus();
        return;
    } else if (pinCode.trim() == "") {
        document.getElementById("btnSaveCust").disabled = false;
        $("#pinCode").addClass('alert-danger');
        $("#pinCode").focus();
        return;
    } else if (Country.trim() == "") {
        document.getElementById("btnSaveCust").disabled = false;
        $("#pinCode").addClass('alert-danger');
        $("#pinCode").focus();
        return;
    }

    else if (shippingAddress1.trim() == "") {
        document.getElementById("btnSaveCust").disabled = false;
        $("#shippingAddress1").addClass('alert-danger');
        $("#shippingAddress1").focus();
        return;
    } else if (shippingpinCode.trim() == "") {
        document.getElementById("btnSaveCust").disabled = false;
        $("#shippingpinCode").addClass('alert-danger');
        $("#shippingpinCode").focus();
        return;
    }
    else if (shippingCountry.trim() == "") {
        document.getElementById("btnSaveCust").disabled = false;
        $("#shippingpinCode").addClass('alert-danger');
        $("#shippingpinCode").focus();
        return;
    } else if (!validGstin()) {
        document.getElementById("btnSaveCust").disabled = false;
        $("#GSTIN1").addClass('alert-danger');
        $("#GSTIN2").addClass('alert-danger');
        $("#GSTIN3").addClass('alert-danger');

        return;
    }
    
    var PAN = $("#PANNum").val();
    
    if (PAN.trim() != "") {
        var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
        var code = /([C,P,H,F,A,T,B,L,J,G])/;
        var code_chk = PAN.substring(3, 4);
        if (PAN.search(panPat) == -1) {
            document.getElementById("btnSaveCust").disabled = false;
            jAlert("Please enter valid PAN.");
           
            return;
        }
        if (code.test(code_chk) == false) {
            document.getElementById("btnSaveCust").disabled = false;
            jAlert("Please enter valid PAN.");
           
            return;
        }
    }

    var CrgNumber = $("#PANNum").val();
    var procode = "0";
    if (CrgNumber.trim() != "") {
        $.ajax({
            type: "POST",
            //url: "Contact_Registration.aspx/CheckUniqueNumber",     
            url: "/OMS/management/Master/Contact_Registration.aspx/SettingsWisecheckCustomerPanNameExists",
            data: JSON.stringify({ CrgNumber: CrgNumber, procode: procode }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                var data = msg.d;

                if (data != "") {
                    document.getElementById("btnSaveCust").disabled = false;
                    jAlert("Entered value is already exist for Customer " + data + ". Cannot Proceed.");
                    ctxtNumber.Focus();
                    return;
                }
            }

        });
    }
    if ($("#hdnEinvoiceActive").val() == "1")
    {
        if (TransactionCategory.trim() == "0") {
            document.getElementById("btnSaveCust").disabled = false;
            $("#TransactionCategory").addClass('alert-danger');
            $("#TransactionCategory").focus();
            return;
        }

    }

    if ($("#hdnDocumentSegmentActive").val() == "1") {
        var table = document.getElementById("TblDocumentSegment");
        document.getElementById('HdnDocumentSegment').value = '';
        //var data = '';
        //var row = table.rows[1];
        //for (var j = 0, col; col = row.cells[j]; j++) {
        //    if (data == '') {
        //        if (col.children[0].value == '') {
        //        }
        //        else {
        //            data = col.children[0].value;
        //        }
        //    }
        //    else {
        //        data = data + '~' + col.children[0].value;
        //    }
        //}
        //if (document.getElementById('HdnDocumentSegment').value == '') {
        //    document.getElementById('HdnDocumentSegment').value = data.toUpperCase();
        //    data = '';
        //}
        //else {
        //    document.getElementById('HdnDocumentSegment').value = document.getElementById('HdnDocumentSegment').value + ',' + data.toUpperCase();
        //    data = '';
        //}

        document.getElementById('HdnDocumentSegment').value = $('#txtSegment1').val() + '~' + $('#txtSegment2').val() + '~' + $('#txtSegment3').val() + '~' + $('#txtSegment4').val() + '~' + $('#txtSegment5').val();

        var Document_Segments = document.getElementById('HdnDocumentSegment').value;

        var TotalDocument_Segments = 0;
        var DocumentSegment = Document_Segments.split("~");
        for (i = 0; i < DocumentSegment.length; i++) {
            TotalDocument_Segments = parseFloat(TotalDocument_Segments) + parseFloat(DocumentSegment[i]);
        }
        if (parseFloat(TotalDocument_Segments) > 15) {
            jAlert("Document Segments value greater than 15.");
            ret = false;
        }
    }

    var Segment1 = $("#txtSegment1").val();
    var Segment2 = $("#txtSegment2").val();
    var Segment3 = $("#txtSegment3").val();
    var Segment4 = $("#txtSegment4").val();
    var Segment5 = $("#txtSegment5").val();

    if (Segment1 == "")
    {
        Segment1 = "0";
    }
    if (Segment2 == "") {
        Segment2 = "0";
    }
    if (Segment3 == "") {
        Segment3 = "0";
    }
    if (Segment4 == "") {
        Segment4 = "0";
    }
    if (Segment5 == "") {
        Segment5 = "0";
    }

    var SegmentUsedFor1 = $("#txtUsedFor1").val();
    var SegmentUsedFor2 = $("#txtUsedFor2").val();
    var SegmentUsedFor3 = $("#txtUsedFor3").val();
    var SegmentUsedFor4 = $("#txtUsedFor4").val();
    var SegmentUsedFor5 = $("#txtUsedFor5").val();

    if (Segment1 != "0")
    {
        if (SegmentUsedFor1 == "")
        {
            document.getElementById("btnSaveCust").disabled = false;
            $("#txtUsedFor1").addClass('alert-danger');
            $("#txtUsedFor1").focus();
            return;
        }
    }
    if (Segment2 != "0") {
        if (SegmentUsedFor2 == "") {
            document.getElementById("btnSaveCust").disabled = false;
            $("#txtUsedFor2").addClass('alert-danger');
            $("#txtUsedFor2").focus();
            return;
        }
    }

    if (Segment3 != "0") {
        if (SegmentUsedFor3 == "") {
            document.getElementById("btnSaveCust").disabled = false;
            $("#txtUsedFor3").addClass('alert-danger');
            $("#txtUsedFor3").focus();
            return;
        }
    }

    if (Segment4 != "0") {
        if (SegmentUsedFor4 == "") {
            document.getElementById("btnSaveCust").disabled = false;
            $("#txtUsedFor4").addClass('alert-danger');
            $("#txtUsedFor4").focus();
            return;
        }
    }

    if (Segment5 != "0") {
        if (SegmentUsedFor5 == "") {
            document.getElementById("btnSaveCust").disabled = false;
            $("#txtUsedFor5").addClass('alert-danger');
            $("#txtUsedFor5").focus();
            return;
        }
    }



    var Segment_Mandatory1 = $("#hdnSegmentMandatory1").val();
    var Segment_Mandatory2 = $("#hdnSegmentMandatory2").val();
    var Segment_Mandatory3 = $("#hdnSegmentMandatory3").val();
    var Segment_Mandatory4 = $("#hdnSegmentMandatory4").val();
    var Segment_Mandatory5 = $("#hdnSegmentMandatory5").val();


    var DocumentSegments = document.getElementById('HdnDocumentSegment').value;
    if (DocumentSegments == undefined)
    {
        DocumentSegments="";
    }

    var SegmentMandatory1 = "";

    //if (Segment_Mandatory1 == "")
    //{
    //    for (i in Segment_Mandatory1)
    //    {
    //        if (SegmentMandatory1 == '') {
    //            SegmentMandatory1 = Segment_Mandatory1[i];
    //        }
    //        else
    //        {
    //            SegmentMandatory1 = SegmentMandatory1+','+Segment_Mandatory1[i];
    //        }
            
    //    }
    //}

    if (Segment_Mandatory1 != "")
    {
        var SegmentMandatory1 = Segment_Mandatory1.join();
    }
    if (Segment_Mandatory2 != "") {
        var SegmentMandatory2 = Segment_Mandatory2.join();
    }
    if (Segment_Mandatory3 != "") {
        var SegmentMandatory3 = Segment_Mandatory3.join();
    }
    if (Segment_Mandatory4 != "") {
        var SegmentMandatory4 = Segment_Mandatory4.join();
    }
    if (Segment_Mandatory5 != "") {
        var SegmentMandatory5 = Segment_Mandatory5.join();
    }



    if (SegmentMandatory1 == undefined) {
        SegmentMandatory1 = "";
    }
    if (SegmentMandatory2 == undefined) {
        SegmentMandatory2 = "";
    }
    if (SegmentMandatory3 == undefined) {
        SegmentMandatory3 = "";
    }
    if (SegmentMandatory4 == undefined) {
        SegmentMandatory4 = "";
    }
    if (SegmentMandatory5 == undefined)
    {
        SegmentMandatory5 = "";
    }
    
    var ServiceBranch = $("#ServiceBranchCust option:selected").val();
    
    var CustomerDetails = {}
    debugger;
    //var 1.2
    CustomerDetails.UniqueID = uniqueID;
    CustomerDetails.Name = Name;
    CustomerDetails.BillingAddress1 = BillingAddress1;
    CustomerDetails.BillingAddress2 = BillingAddress2;
    CustomerDetails.BillingPin = pinCode;
    CustomerDetails.shippingAddress1 = shippingAddress1;
    CustomerDetails.shippingAddress2 = shippingAddress2;
    CustomerDetails.shippingPin = shippingpinCode;
    CustomerDetails.GSTIN = GSTIN;
    CustomerDetails.BillingPhone = BillingPhone;
    CustomerDetails.ShippingPhone = ShippingPhone;
    CustomerDetails.contactperson = contactperson;
    CustomerDetails.IdTypeval = IdTypeval;
    CustomerDetails.GrpCust = GrpCust;
    
    if (dataval == "1") {
        CustomerDetails.NumberingId = NumberingId;
    }
    else
    {
        CustomerDetails.NumberingId = NumberingId;
    }
    CustomerDetails.TCSApplicable = TCSApplicable;
    CustomerDetails.PANValue = $("#PANNum").val();
    CustomerDetails.TransactionCategory = TransactionCategory;
    CustomerDetails.DocumentSegments = DocumentSegments;

    CustomerDetails.Segment1 = Segment1;
    CustomerDetails.Segment2 = Segment2;
    CustomerDetails.Segment3 = Segment3;
    CustomerDetails.Segment4 = Segment4;
    CustomerDetails.Segment5 = Segment5;


    CustomerDetails.SegmentUsedFor1 = SegmentUsedFor1;
    CustomerDetails.SegmentUsedFor2 = SegmentUsedFor2;
    CustomerDetails.SegmentUsedFor3 = SegmentUsedFor3;
    CustomerDetails.SegmentUsedFor4 = SegmentUsedFor4;
    CustomerDetails.SegmentUsedFor5 = SegmentUsedFor5;


    CustomerDetails.SegmentMandatory1 = SegmentMandatory1;
    CustomerDetails.SegmentMandatory2 = SegmentMandatory2;
    CustomerDetails.SegmentMandatory3 = SegmentMandatory3;
    CustomerDetails.SegmentMandatory4 = SegmentMandatory4;
    CustomerDetails.SegmentMandatory5 = SegmentMandatory5;

    CustomerDetails.ServiceBranch = ServiceBranch;



    //data: "{ UniqueID: '" + uniqueID + "',Name: '" + Name + "',BillingAddress1: '" + BillingAddress1 + "',BillingAddress2: '" + BillingAddress2 + "',BillingPin: '" + pinCode + "',shippingAddress1: '" + shippingAddress1 + "',shippingAddress2: '" + shippingAddress2 + "',shippingPin: '" + shippingpinCode + "', GSTIN:'" + GSTIN + "',BillingPhone:'" + BillingPhone + "',ShippingPhone:'" + ShippingPhone + "',contactperson:'" + contactperson + "' }",

    $.ajax({
        type: "POST",
        url: "/OMS/management/Master/Customer_general.aspx/SaveCustomer",
        data: JSON.stringify(CustomerDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: "true",
        cache: "false",
        success: function (msg) {
            var obj = msg.d;
            if (obj.status == "Ok") {

                //parent.ParentCustomerOnClose(obj.InternalId, $("#Name").val(), $("#uniqueId").val(), obj.BillingStateText, obj.BillingStateCode, obj.ShippingStateText, obj.ShippingStateCode);
                parent.ParentCustomerOnClose(obj.InternalId, $("#Name").val(), uniqueID, obj.BillingStateText, obj.BillingStateCode, obj.ShippingStateText, obj.ShippingStateCode);
                
            }
            else if (obj.status == "DuplicateGSTIN") {
                document.getElementById("btnSaveCust").disabled = false;
                jAlert('Duplicate GSTIN. ');

            }
            else if (obj.status == "AUTOError") {
                document.getElementById("btnSaveCust").disabled = false;
                jAlert('Either Unique ID Exists OR Unique ID Exhausted.');
                return;
            }
            else if (obj.status == "BannedPAN") {
                document.getElementById("btnSaveCust").disabled = false;
                jAlert('This PAN is banned by SEBI.');
                return;
            }
            else if (obj.status == "DuplicatePAN") {
                document.getElementById("btnSaveCust").disabled = false;
                jAlert('Duplicate PAN number is not allowed.');
                return;
            }
            else {
                document.getElementById("btnSaveCust").disabled = false;
                console.log(obj.Msg);
            }
        },
        Error: function (x, e) {
            // On Error
        }
    });
}




function UniqueVendorCodeCheck() {
    debugger;
    var SchemeVal = $("#NumSchemeVendor").val();

    var NoSchemeId = SchemeVal.toString().split('~')[0];
    if (SchemeVal == "0") {
        jAlert('Please Select Numbering Scheme');
        //ctxt_SlOrderNo.SetValue('');
        //ctxt_SlOrderNo.Focus();
    }

        //if (NoSchemeId == "0") 
    else {
        var CheckUniqueCode = false;
        var uccName = "";
        var Type = "";

        uccName = $("#DocNum").val();
        Type = "MasterVendor";


        $.ajax({
            type: "POST",
            url: "/OMS/management/Master/HRrecruitmentagent_general.aspx/CheckUniqueNumberingCode",
            data: JSON.stringify({ uccName: uccName, Type: Type }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                CheckUniqueCode = msg.d;
                if (CheckUniqueCode == true) {
                    $("#DocNum").val('');
                    $("#DocNum").addClass('alert-danger');
                    $("#DocNum").focus();
                    // console.log('Please enter unique document number');
                    //jAlert('Please enter unique Sales Order No');

                }

            }

        });
    }
}

function NameForSave()
{
    document.getElementById("btnSaveCust").disabled = false;
}


function SetPanNumber()
{
    
    $("#PANNum").val($("#GSTIN2").val());
    $("#TransactionCategory").val("B2B")
}

function UniquePanNumberCheck()
{
    var PAN = $("#PANNum").val();
   // $("#hdnflag").val("0");
    if (PAN.trim() != "") {
        var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
        var code = /([C,P,H,F,A,T,B,L,J,G])/;
        var code_chk = PAN.substring(3, 4);
        if (PAN.search(panPat) == -1) {
            jAlert("Please enter valid PAN.");
           // $("#hdnflag").val("1");
            //alert('hi');
            return;
        }
        if (code.test(code_chk) == false) {
            jAlert("Please enter valid PAN.");
            //$("#hdnflag").val("1");
            return;
        }
    }
    var CrgNumber = $("#PANNum").val();
    var procode = "0";
    if (CrgNumber.trim() != "") {
        $.ajax({
            type: "POST",
            //url: "Contact_Registration.aspx/CheckUniqueNumber",     
            url: "/OMS/management/Master/Contact_Registration.aspx/SettingsWisecheckCustomerPanNameExists",
            data: JSON.stringify({ CrgNumber: CrgNumber, procode: procode }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                var data = msg.d;
               
                if (data != "") {
                    jAlert("Entered value is already exist for Customer " + data + ". Cannot Proceed.");
                    ctxtNumber.Focus();
                    return ;
                }
            }

        });
    }


}

function UniqueCodeCheck() {
    debugger;
    var SchemeVal = $("#NumScheme").val();

    var NoSchemeId = SchemeVal.toString().split('~')[0];
    if (SchemeVal == "0") {
        jAlert('Please Select Numbering Scheme');
        //ctxt_SlOrderNo.SetValue('');
        //ctxt_SlOrderNo.Focus();
    }

        //if (NoSchemeId == "0") 
    else {
        var CheckUniqueCode = false;
        var uccName = "";
        var Type = "";
       
        uccName = $("#DocNum").val();
        Type = "Mastercustomerclient";
       

        $.ajax({
            type: "POST",
            url: "/OMS/management/Master/Contact_general.aspx/CheckUniqueNumberingCode",
            data: JSON.stringify({ uccName: uccName, Type: Type }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                CheckUniqueCode = msg.d;
                if (CheckUniqueCode == true) {
                    $("#DocNum").val('');
                    $("#DocNum").addClass('alert-danger');
                    //$("#DocNum").focus();
                   // console.log('Please enter unique document number');
                    jAlert('Duplicate Unique Id. Cannot Proceed.');
                    
                }
                else
                {
                    $("#DocNum").removeClass('alert-danger');
                }

            }

        });
    }
}



function SaveVendor() {
    debugger;
  
  
    $("#VenuniqueId").removeClass('alert-danger');
    $("#VenName").removeClass('alert-danger');
    $("#VenBillingAddress1").removeClass('alert-danger');
    $("#VenpinCode").removeClass('alert-danger');
    $("#VenshippingAddress1").removeClass('alert-danger');
    $("#VenshippingpinCode").removeClass('alert-danger');

    $("#VenGSTIN1").removeClass('alert-danger');
    $("#VenGSTIN2").removeClass('alert-danger');
    $("#VenGSTIN3").removeClass('alert-danger');

    var uniqueID = "";
    //rev srijeeta  mantis issue 0024515
    var Alternative_Code = $("#Docaltcodee").val();;
    //end of rev srijeeta  mantis issue 0024515
    var NumberingId = "0";
    var NumberingVal = $("#NumSchemeVendor").val()
    if (datavalVendor != "1") {
        uniqueID = $("#VenuniqueId").val();
        NumberingId = "0"
    }
    else {
        uniqueID = $("#DocNum").val();
        NumberingId = NumberingVal.split("~")[0]
    }


    //var uniqueID = $("#VenuniqueId").val();
    var Name = $("#VenName").val();
    var BillingAddress1 = $("#VenBillingAddress1").val();
    var BillingAddress2 = $("#VenBillingAddress2").val();
    var pinCode = $("#VenpinCode").val();
    var Country = $("#VenCountry").val();

    var shippingAddress1 = $("#VenshippingAddress1").val();
    var shippingAddress2 = $("#VenshippingAddress2").val();
    var shippingpinCode = $("#VenshippingpinCode").val();
    var shippingCountry = $("#VenshippingCountry").val();

    var GSTIN = $("#VenGSTIN1").val().trim() + $("#VenGSTIN2").val().trim() + $("#VenGSTIN3").val().trim();

    var BillingPhone = $("#VenBillingPhone").val().trim();
    var ShippingPhone = $("#VenShippingPhone").val().trim();
    var contactperson = $("#Vencontactperson").val().trim();

    var GrpCust = $("#VenGroupCust option:selected").val();
    ///rev Bapi
    var hiddenproduct = $("#hdnSegmentMandatory1").val().toString();
    

    //end rev Bapi
    //Add rev for Transcation Category Tanmoy
    var TransactionCategory = "0";//$("#TransactionCategory option:selected").val();
    //Add rev for Transcation Category Tanmoy
   // var IdType = document.getElementById("IdType");
    var IdTypeval =0;


    //if (IdTypeval == 1) {
    //    if (isNaN(uniqueID)) {
    //        alert("Please type valid Phone No.");
    //        return;
    //    }
    //} else if (IdTypeval == 2) {

    //    var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
    //    var code = /([C,P,H,F,A,T,B,L,J,G])/;
    //    var code_chk = uniqueID.substring(3, 4);
    //    if (uniqueID.search(panPat) == -1) {
    //        alert("Please type valid PAN No.");
    //        return;
    //    }
    //    if (code.test(code_chk) == false) {
    //        alert("Please type valid PAN No.");
    //        return;
    //    }

    //} else if (IdTypeval == 3) {
    //    if (isNaN(uniqueID) || uniqueID.length != 12) {
    //        alert("Please type valid Aadhar No.");
    //        return;
    //    }
    //}
    if (datavalVendor != "1") {
        if (uniqueID.trim() == "") {
            $("#VenuniqueId").addClass('alert-danger');
            $("#VenuniqueId").focus();
            return;
        }
    }

    if (datavalVendor == "1") {
        var SchemeVal = $("#NumSchemeVendor").val();
        var uccName = $("#DocNum").val();
        if (SchemeVal == "0") {
            jAlert('Please Select Numbering Scheme');
            return;
        }
        if (uccName == "") {
            jAlert('Please Enter Unique Id.');
            return;
        }
    }

    if (Name.trim() == "") {
        $("#VenName").addClass('alert-danger');
        $("#VenName").focus();
        return;
    }


    else if (BillingAddress1.trim() == "") {
        $("#VenBillingAddress1").addClass('alert-danger');
        $("#VenBillingAddress1").focus();
        return;
    } else if (pinCode.trim() == "") {
        $("#VenpinCode").addClass('alert-danger');
        $("#VenpinCode").focus();
        return;
    } else if (Country.trim() == "") {
        $("#VenpinCode").addClass('alert-danger');
        $("#VenpinCode").focus();
        return;
    }

    else if (shippingAddress1.trim() == "") {
        $("#VenshippingAddress1").addClass('alert-danger');
        $("#VenshippingAddress1").focus();
        return;
    } else if (shippingpinCode.trim() == "") {
        $("#VenshippingpinCode").addClass('alert-danger');
        $("#VenshippingpinCode").focus();
        return;
    }
    else if (shippingCountry.trim() == "") {
        $("#VenshippingpinCode").addClass('alert-danger');
        $("#VenshippingpinCode").focus();
        return;
    } else if (!validVendorGstin()) {
        $("#VenGSTIN1").addClass('alert-danger');
        $("#VenGSTIN2").addClass('alert-danger');
        $("#VenGSTIN3").addClass('alert-danger');

        return;
    }

    var CustomerDetails = {}
   
    //var 1.2
    CustomerDetails.UniqueID = uniqueID;
    //rev srijeeta  mantis issue 0024515
    CustomerDetails.Alternative_Code = Alternative_Code;
    //end of rev srijeeta  mantis issue 0024515
    CustomerDetails.Name = Name;
    CustomerDetails.BillingAddress1 = BillingAddress1;
    CustomerDetails.BillingAddress2 = BillingAddress2;
    CustomerDetails.BillingPin = pinCode;
    CustomerDetails.shippingAddress1 = shippingAddress1;
    CustomerDetails.shippingAddress2 = shippingAddress2;
    CustomerDetails.shippingPin = shippingpinCode;
    CustomerDetails.GSTIN = GSTIN;
    CustomerDetails.BillingPhone = BillingPhone;
    CustomerDetails.ShippingPhone = ShippingPhone;
    CustomerDetails.contactperson = contactperson;
    CustomerDetails.IdTypeval = IdTypeval;
    CustomerDetails.GrpCust = GrpCust;
    if (datavalVendor == "1") {
        CustomerDetails.NumberingId = NumberingId;
    }
    else {
        CustomerDetails.NumberingId = NumberingId;
    }
    CustomerDetails.TransactionCategory = TransactionCategory;
    CustomerDetails.ProductIDs = hiddenproduct;

    //data: "{ UniqueID: '" + uniqueID + "',Name: '" + Name + "',BillingAddress1: '" + BillingAddress1 + "',BillingAddress2: '" + BillingAddress2 + "',BillingPin: '" + pinCode + "',shippingAddress1: '" + shippingAddress1 + "',shippingAddress2: '" + shippingAddress2 + "',shippingPin: '" + shippingpinCode + "', GSTIN:'" + GSTIN + "',BillingPhone:'" + BillingPhone + "',ShippingPhone:'" + ShippingPhone + "',contactperson:'" + contactperson + "' }",


    $.ajax({
        type: "POST",
        url: "/OMS/management/Master/Customer_general.aspx/SaveVendorLightwet",
        data: JSON.stringify(CustomerDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: "true",
        cache: "false",
        success: function (msg) {
            var obj = msg.d;
            if (obj.status == "Ok") {

                parent.ParentCustomerOnClose(obj.InternalId, $("#VenName").val(), $("#VenuniqueId").val(), obj.BillingStateText, obj.BillingStateCode, obj.ShippingStateText, obj.ShippingStateCode);

            }
            else if (obj.status == "DuplicateGSTIN") {                
                    jAlert('Duplicate GSTIN. ');                 
            }
            else if (obj.status == "AUTOError") {
                jAlert('Either Unique ID Exists OR Unique ID Exhausted.');
                return;
            }
            else {
                console.log(obj.Msg);
            }
        },
        Error: function (x, e) {
            // On Error
        }
    });
}

function PinChange() {
    var pincode = $("#pinCode").val();

    $.ajax({
        type: "POST",
        url: "/OMS/management/Master/Customer_general.aspx/GetAddressdetails",
        data: "{ pinCode: '" + pincode + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: "true",
        cache: "false",
        success: function (msg) {
            var obj = msg.d;
            if (obj.status == "Ok") {
                $("#Country").val(obj.Country);
                $("#State").val(obj.state);
                $("#City").val(obj.city);
                $("#pinCode").removeClass('alert-danger');
            } else {
                $("#Country").val('');
                $("#State").val('');
                $("#City").val('');
                $("#pinCode").addClass('alert-danger');
            }
        },
        Error: function (x, e) {
            // On Error
        }
    });


}

function VendorPinChange() {
    var pincode = $("#VenpinCode").val();

    $.ajax({
        type: "POST",
        url: "/OMS/management/Master/Customer_general.aspx/GetAddressdetails",
        data: "{ pinCode: '" + pincode + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: "true",
        cache: "false",
        success: function (msg) {
            var obj = msg.d;
            if (obj.status == "Ok") {
                $("#VenCountry").val(obj.Country);
                $("#VenState").val(obj.state);
                $("#VenCity").val(obj.city);
                $("#VenpinCode").removeClass('alert-danger');
            } else {
                $("#VenCountry").val('');
                $("#VenState").val('');
                $("#VenCity").val('');
                $("#VenpinCode").addClass('alert-danger');
            }
        },
        Error: function (x, e) {
            // On Error
        }
    });


}



function shippingPinChange() {
    var pincode = $("#shippingpinCode").val();

    $.ajax({
        type: "POST",
        url: "/OMS/management/Master/Customer_general.aspx/GetAddressdetails",
        data: "{ pinCode: '" + pincode + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: "true",
        cache: "false",
        success: function (msg) {
            var obj = msg.d;
            if (obj.status == "Ok") {
                $("#shippingCountry").val(obj.Country);
                $("#shippingState").val(obj.state);
                $("#shippingCity").val(obj.city);
                $("#shippingpinCode").removeClass('alert-danger');
            } else {
                $("#shippingCountry").val('');
                $("#shippingState").val('');
                $("#shippingCity").val('');
                $("#shippingpinCode").addClass('alert-danger');
            }
        },
        Error: function (x, e) {
            // On Error
        }
    });


}


function VendorshippingPinChange() {
    var pincode = $("#VenshippingpinCode").val();

    $.ajax({
        type: "POST",
        url: "/OMS/management/Master/Customer_general.aspx/GetAddressdetails",
        data: "{ pinCode: '" + pincode + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: "true",
        cache: "false",
        success: function (msg) {
            var obj = msg.d;
            if (obj.status == "Ok") {
                $("#VenshippingCountry").val(obj.Country);
                $("#VenshippingState").val(obj.state);
                $("#VenshippingCity").val(obj.city);
                $("#VenshippingpinCode").removeClass('alert-danger');
            } else {
                $("#VenshippingCountry").val('');
                $("#VenshippingState").val('');
                $("#VenshippingCity").val('');
                $("#VenshippingpinCode").addClass('alert-danger');
            }
        },
        Error: function (x, e) {
            // On Error
        }
    });


}


function CheckUnique() {
    var uniqueId = $("#uniqueId").val();
    if (uniqueId.trim() != '') {
        $.ajax({
            type: "POST",
            url: "/OMS/management/Master/Customer_general.aspx/CheckuniqueId",
            data: "{ uniqueId: '" + uniqueId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: "true",
            cache: "false",
            success: function (msg) {
                debugger;
                var obj = msg.d;
                if (obj.IsPresent) {
                    $("#uniqueId").addClass('alert-danger');
                    $("#uniqueId").val('');
                    //Rev Rajdip
                    jAlert("Duplicate Unique Id.Cannot Proceed.");
                    //End Rev Rajdip
                    $("#uniqueId").focus();
                } else {
                    $("#uniqueId").removeClass('alert-danger');
                }
            },
            Error: function (x, e) {
                // On Error
            }
        });
    }

}


function CheckVendorUnique() {
    var uniqueId = $("#VenuniqueId").val();
    if (uniqueId.trim() != '') {
        $.ajax({
            type: "POST",
            url: "/OMS/management/Master/Customer_general.aspx/VendorCheckuniqueId",
            data: "{ uniqueId: '" + uniqueId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: "true",
            cache: "false",
            success: function (msg) {
                debugger;
                var obj = msg.d;
                if (obj.IsPresent) {
                    $("#VenuniqueId").addClass('alert-danger');
                    $("#VenuniqueId").focus();
                   
                   
                    document.getElementById("uniqueIdMessage").style.display = "block";
                    $("#VenuniqueId").val("");
                } else {
                    $("#VenuniqueId").removeClass('alert-danger');
                    document.getElementById("uniqueIdMessage").style.display = "none";
                }
            },
            Error: function (x, e) {
                // On Error
            }
        });
    }

}


function billingCopyToshipping() {
    
        $("#shippingAddress1").val($("#BillingAddress1").val());
        $("#shippingAddress2").val($("#BillingAddress2").val());
        $("#shippingpinCode").val($("#pinCode").val());
        $("#shippingCountry").val($("#Country").val());
        $("#shippingState").val($("#State").val());
        $("#shippingCity").val($("#City").val());
        $("#ShippingPhone").val($("#BillingPhone").val());
     
}

function VendorbillingCopyToshipping() {

    $("#VenshippingAddress1").val($("#VenBillingAddress1").val());
    $("#VenshippingAddress2").val($("#VenBillingAddress2").val());
    $("#VenshippingpinCode").val($("#VenpinCode").val());
    $("#VenshippingCountry").val($("#VenCountry").val());
    $("#VenshippingState").val($("#VenState").val());
    $("#VenshippingCity").val($("#VenCity").val());
    $("#VenShippingPhone").val($("#VenBillingPhone").val());

}


function shippingCopyTobilling() {
    $("#BillingAddress1").val($("#shippingAddress1").val());
    $("#BillingAddress2").val($("#shippingAddress2").val());
    $("#pinCode").val($("#shippingpinCode").val());
    $("#Country").val($("#shippingCountry").val());
    $("#State").val($("#shippingState").val());
    $("#City").val($("#shippingCity").val());
    $("#BillingPhone").val($("#ShippingPhone").val());
}

function VendorshippingCopyTobilling() {
    $("#VenBillingAddress1").val($("#VenshippingAddress1").val());
    $("#VenBillingAddress2").val($("#VenshippingAddress2").val());
    $("#VenpinCode").val($("#VenshippingpinCode").val());
    $("#VenCountry").val($("#VenshippingCountry").val());
    $("#VenState").val($("#VenshippingState").val());
    $("#VenCity").val($("#VenshippingCity").val());
    $("#VenBillingPhone").val($("#VenShippingPhone").val());
}


function ClearShipping() {
    $("#shippingAddress1").val('');
    $("#shippingAddress2").val('');
    $("#shippingpinCode").val('');
    $("#shippingCountry").val('');
    $("#shippingState").val('');
    $("#shippingCity").val('');
    $("#ShippingPhone").val('');

}

function VendorClearShipping() {
    $("#VenshippingAddress1").val('');
    $("#VenshippingAddress2").val('');
    $("#VenshippingpinCode").val('');
    $("#VenshippingCountry").val('');
    $("#VenshippingState").val('');
    $("#VenshippingCity").val('');
    $("#VenShippingPhone").val('');

}

function ClearBilling() {
    $("#BillingAddress1").val('');
    $("#BillingAddress2").val('');
    $("#pinCode").val('');
    $("#Country").val('');
    $("#State").val('');
    $("#City").val('');
    $("#BillingPhone").val('');
}
function VendorClearBilling() {
    $("#VenBillingAddress1").val('');
    $("#VenBillingAddress2").val('');
    $("#VenpinCode").val('');
    $("#VenCountry").val('');
    $("#VenState").val('');
    $("#VenCity").val('');
    $("#VenBillingPhone").val('');
}
function validGstin() {
    var GSTIN1 = $("#GSTIN1").val().trim();
    var GSTIN2 = $("#GSTIN2").val().trim();
    var GSTIN3 = $("#GSTIN3").val().trim();

    var returnval = true;

    if (GSTIN1.length == 0 && GSTIN2.length == 0 && GSTIN3.length == 0) {
    }
    else {
        if (GSTIN1.length != 2 || GSTIN2.length != 10 || GSTIN3.length != 3) {
            $('#invalidGst').css({ 'display': 'block' });
            returnval = false;
        }


        var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
        var code = /([C,P,H,F,A,T,B,L,J,G])/;
        var code_chk = GSTIN2.substring(3, 4);
        if (GSTIN2.search(panPat) == -1) { 
            returnval = false;
        }
        if (code.test(code_chk) == false) { 
            returnval = false;
        }
    }

    return returnval;

}

function validVendorGstin() {
    var GSTIN1 = $("#VenGSTIN1").val().trim();
    var GSTIN2 = $("#VenGSTIN2").val().trim();
    var GSTIN3 = $("#VenGSTIN3").val().trim();

    var returnval = true;

    if (GSTIN1.length == 0 && GSTIN2.length == 0 && GSTIN3.length == 0) {
    }
    else {
        if (GSTIN1.length != 2 || GSTIN2.length != 10 || GSTIN3.length != 3) {
            $('#invalidGst').css({ 'display': 'block' });
            returnval = false;
        }


        var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
        var code = /([C,P,H,F,A,T,B,L,J,G])/;
        var code_chk = GSTIN2.substring(3, 4);
        if (GSTIN2.search(panPat) == -1) {
            returnval = false;
        }
        if (code.test(code_chk) == false) {
            returnval = false;
        }
    }

    return returnval;

}



$(document).ready(function () {
    debugger;
    $("#uniqueId").focus();
    $("#VenuniqueId").focus();
  
    $('#NumScheme').change(function () {
        //

        var NoSchemeTypedtl = $("#NumScheme").val();
        var NoSchemeId = NoSchemeTypedtl.toString().split('~')[0];
        var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];

        var schemeLength = NoSchemeTypedtl.toString().split('~')[2];

        if (NoSchemeType == '1') {
            $("#DocNum").val('Auto');
            $('#DocNum').prop('disabled', true);
            $('#DocNum input').attr('maxlength', schemeLength);


        }
        else if (NoSchemeType == '0') {
            schemeLength = 50;
            $("#DocNum").val('');
            $('#DocNum').prop('disabled', false);
            $('#DocNum input').attr('maxlength', schemeLength);
        }
        else if ($('#NumScheme').val() == "0") {
            $("#DocNum").val('');
            $('#DocNum').prop('disabled', false);
        }

    });


    $('#NumSchemeVendor').change(function () {
        //
        debugger;
        var NoSchemeTypedtl = $("#NumSchemeVendor").val();
        var NoSchemeId = NoSchemeTypedtl.toString().split('~')[0];
        var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];

        var schemeLength = NoSchemeTypedtl.toString().split('~')[2];

        if (NoSchemeType == '1') {
            $("#DocNum").val('Auto');
            $('#DocNum').prop('disabled', true);
            $('#DocNum input').attr('maxlength', schemeLength);


        }
        else if (NoSchemeType == '0') {
            schemeLength = 80;
            $("#DocNum").val('');
            $('#DocNum').prop('disabled', false);
            $('#DocNum input').attr('maxlength', schemeLength);
        }
        else if ($('#NumSchemeVendor').val() == "0") {
            $("#DocNum").val('');
            $('#DocNum').prop('disabled', false);
        }

    });

});



function BillingCheckChange() {
    if (document.getElementById('fancy-checkbox-success').checked) {
        document.getElementById('fancy-checkbox-success').checked = false;
        ClearShipping();
    }
    else {
        document.getElementById('fancy-checkbox-success').checked = true;
        billingCopyToshipping();
    }

}


function VendorBillingCheckChange() {
    if (document.getElementById('fancy-checkbox-success').checked) {
        document.getElementById('fancy-checkbox-success').checked = false;
        VendorClearShipping();
    }
    else {
        document.getElementById('fancy-checkbox-success').checked = true;
        VendorbillingCopyToshipping();
    }

}


function ShippingCheckChange() {
    if (document.getElementById('fancy-checkbox-successShipping').checked) {
        document.getElementById('fancy-checkbox-successShipping').checked = false;
        ClearBilling();
    }
    else {
        document.getElementById('fancy-checkbox-successShipping').checked = true;
        shippingCopyTobilling();
    }

}


function VendorShippingCheckChange() {
    if (document.getElementById('fancy-checkbox-successShipping').checked) {
        document.getElementById('fancy-checkbox-successShipping').checked = false;
        VendorClearBilling();
    }
    else {
        document.getElementById('fancy-checkbox-successShipping').checked = true;
        VendorshippingCopyTobilling();
    }

}

$(document).ready(function () {
    $.ajax({
        url: "/OMS/management/Master/View/Services/ViewCustomerService.asmx/getGroupmasterDetails",
        contentType: "application/json; charset=utf-8",
        datatype: "JSON",
        type: "POST",
        success: function (data) {

            $("#GroupCust").empty();
            var grpdetl = data.d;

            debugger;

            var opts = "";
            opts = "<option value='0'>Select</option>";
            for (i in grpdetl)
                //if (opts == "") {
                //    opts = "<option value='0'>Select</option>"
                //}
                opts += "<option value='" + grpdetl[i].Id + "'>" + grpdetl[i].Name + "</option>";

            $("#GroupCust").empty().append(opts);
            //$.each(grpdetl, function (Id, Name) {
            //    $('#GroupCust')
            //        .append($("<option></option>")
            //                   .attr("value", Id)
            //                   .text(Name));
            //});
            // for (var i = 0; i < grpdetl.length; i++) {
            //     var opt = new Option(grpdetl[i].Name);

            //    $("#GroupCust").append(opt);

            //     debugger;
            //}
        },
        error: function (data) {
            debugger;
            jAlert("Please try again later");
        }
    });


    $.ajax({
        url: "/OMS/management/Master/View/Services/ViewCustomerService.asmx/getGroupmasterDetails",
        contentType: "application/json; charset=utf-8",
        datatype: "JSON",
        type: "POST",
        success: function (data) {

            $("#GroupCust").empty();
            var grpdetl = data.d;

            debugger;

            var opts = "";
            opts = "<option value='0'>Select</option>";
            for (i in grpdetl)
                
                opts += "<option value='" + grpdetl[i].Id + "'>" + grpdetl[i].Name + "</option>";

            $("#GroupCust").empty().append(opts);
            
        },
        error: function (data) {
            debugger;
            jAlert("Please try again later");
        }
    });

    


});

var dataval;
var datavalVendor;
$(document).ready(function () {
    debugger;
    $.ajax({
        url: "/OMS/management/Master/View/Services/ViewCustomerService.asmx/getVendorGroupmasterDetails",
        contentType: "application/json; charset=utf-8",
        datatype: "JSON",
        type: "POST",
        success: function (data) {

            $("#VenGroupCust").empty();
            var grpdetl = data.d;

          

            var opts = "";
            opts = "<option value='0'>Select</option>";
            for (i in grpdetl)
                //if (opts == "") {
                //    opts = "<option value='0'>Select</option>"
                //}
                opts += "<option value='" + grpdetl[i].Id + "'>" + grpdetl[i].Name + "</option>";

            $("#VenGroupCust").empty().append(opts);
          
        },
        error: function (data) {
           
            jAlert("Please try again later");
        }
    });

    $.ajax({
        url: "/OMS/management/Master/View/Services/ViewCustomerService.asmx/getCustomerNumberSchemeSettings",
        contentType: "application/json; charset=utf-8",
        datatype: "JSON",
        type: "POST",
        async:false,
        success: function (data) {
           // debugger;
             dataval = data.d;
            if(dataval=="1")
            {
                $("#dvIdType").hide();
                $("#dvUniqueId").hide();
                $("#dvdocCust").show();
                $("#dvNumberingSchemeCust").show();
            }
             else
            {
                $("#dvIdType").show();
                $("#dvUniqueId").show();
                $("#dvdocCust").hide();
                $("#dvNumberingSchemeCust").hide();
            }

        },
        error: function (data) {

            jAlert("Please try again later");
        }
    });

    $.ajax({
        url: "/OMS/management/Master/View/Services/ViewVendorService.asmx/getVendorNumberSchemeSettings",
        contentType: "application/json; charset=utf-8",
        datatype: "JSON",
        type: "POST",
        async: false,
        success: function (data) {
          debugger;
            datavalVendor = data.d;
            if (datavalVendor == "1") {
              
                $("#dvUniqueVendorId").hide();
                $("#dvdoc").show();
                $("#dvNumberingScheme").show();
            }
            else
            {
                $("#dvUniqueVendorId").show();
                $("#dvdoc").hide();
                $("#dvNumberingScheme").hide();
            }

        },
        error: function (data) {

            jAlert("Please try again later");
        }
    });



    $.ajax({
        url: "/OMS/management/Master/View/Services/ViewCustomerService.asmx/NumberingSchemeBind",
        contentType: "application/json; charset=utf-8",
        datatype: "JSON",
        type: "POST",
        success: function (data) {

            $("#NumScheme").empty();
            var grpdetl = data.d;

          //  debugger;

            var opts = "";
            //opts = "<option value='0'>Select</option>";
            for (i in grpdetl)
                //if (opts == "") {
                //    opts = "<option value='0'>Select</option>"
                //}
                opts += "<option value='" + grpdetl[i].Id + "'>" + grpdetl[i].SchemaName + "</option>";

            $("#NumScheme").empty().append(opts);

        },
        error: function (data) {

            jAlert("Please try again later");
        }
    });

    $.ajax({
        url: "/OMS/management/Master/View/Services/ViewVendorService.asmx/NumberingSchemeBind",
        contentType: "application/json; charset=utf-8",
        datatype: "JSON",
        type: "POST",
        success: function (data) {

            $("#NumSchemeVendor").empty();
            var grpdetl = data.d;

           // debugger;

            var opts = "";
            //opts = "<option value='0'>Select</option>";
            for (i in grpdetl)
                //if (opts == "") {
                //    opts = "<option value='0'>Select</option>"
                //}
                opts += "<option value='" + grpdetl[i].Id + "'>" + grpdetl[i].SchemaName + "</option>";

            $("#NumSchemeVendor").empty().append(opts);

        },
        error: function (data) {

            jAlert("Please try again later");
        }
    });


     $.ajax({
        url: "/OMS/management/Master/View/Services/ViewCustomerService.asmx/getEinvoiceActiveSettings",
        contentType: "application/json; charset=utf-8",
        datatype: "JSON",
        type: "POST",
        async:false,
        success: function (data) {
          
          var EinvoiceActive = data.d;
            if(EinvoiceActive=="1")
            {
                
                $("#hdnEinvoiceActive").val("1");

                $("#divTransactionCategory").show();
            }
             else
            {
                $("#hdnEinvoiceActive").val("0");
                $("#divTransactionCategory").hide();
            }
        },
        error: function (data) {

            jAlert("Please try again later");
        }
     });

    //rev Bapi
     $.ajax({
         url: "/OMS/management/Master/View/Services/ViewCustomerService.asmx/getVendorNumberSchemeSettings",
         contentType: "application/json; charset=utf-8",
         datatype: "JSON",
         type: "POST",
         async: false,
         success: function (data) {
             debugger;
             datavalProduct = data.d;
             if (datavalProduct == "1") {

                
                 $("#Table1").show();
             }
             else {
                
                 $("#Table1").hide();
             }

         },
         error: function (data) {

             jAlert("Please try again later");
         }
     });



    //End rev bapi

     $.ajax({
         url: "/OMS/management/Master/View/Services/ViewCustomerService.asmx/PopulateAllProject",
         contentType: "application/json; charset=utf-8",
         datatype: "JSON",
         type: "POST",
         success: function (data) {

            
             $("#cddlSegmentMandatory1").empty();
             $("#cddlSegmentMandatory2").empty();
             $("#cddlSegmentMandatory3").empty();
             $("#cddlSegmentMandatory4").empty();
             $("#cddlSegmentMandatory5").empty();
             var grpdetl = data.d;

             //  debugger;

             var opts = "";
             //opts = "<option value='0'>Select</option>";
             for (i in grpdetl)
                 //if (opts == "") {
                 //    opts = "<option value='0'>Select</option>"
                 //}
                 opts += "<option value='" + grpdetl[i].Id + "'>" + grpdetl[i].Name + "</option>";

             $("#cddlSegmentMandatory1").empty().append(opts);
             
             $("#cddlSegmentMandatory2").empty().append(opts);
             $("#cddlSegmentMandatory3").empty().append(opts);
             $("#cddlSegmentMandatory4").empty().append(opts);
             $("#cddlSegmentMandatory5").empty().append(opts);
             setTimeout(function () {
                 //$('#cddlSegmentMandatory1, #cddlSegmentMandatory2, #cddlSegmentMandatory3, #cddlSegmentMandatory4, #cddlSegmentMandatory5').multiselect('rebuild');
                 $('#cddlSegmentMandatory1').multiselect('rebuild');
                 $('#cddlSegmentMandatory2').multiselect('rebuild');
                 $('#cddlSegmentMandatory3').multiselect('rebuild');
                 $('#cddlSegmentMandatory4').multiselect('rebuild');
                 $('#cddlSegmentMandatory5').multiselect('rebuild');
             }, 200)
         },
         error: function (data) {

             jAlert("Please try again later");
         }
     });

     $.ajax({
         url: "/OMS/management/Master/View/Services/ViewCustomerService.asmx/getDocumentSegmentSettings",
         contentType: "application/json; charset=utf-8",
         datatype: "JSON",
         type: "POST",
         async: false,
         success: function (data) {

             var DocumentSegmentActive = data.d;
             if (DocumentSegmentActive == "1") {

                 $("#hdnDocumentSegmentActive").val("1");

                 $("#PanelDocumentSegment").show();
                 $("#DivServiceBranch").show();
                 
             }
             else {
                 $("#hdnDocumentSegmentActive").val("0");
                 $("#PanelDocumentSegment").hide();
                 $("#DivServiceBranch").hide();

             }
         },
         error: function (data) {

             jAlert("Please try again later");
         }
     });

     if ($("#hdnDocumentSegmentActive").val() == "1")
     {
         $.ajax({
             url: "/OMS/management/Master/View/Services/ViewCustomerService.asmx/getAllServiceBranch",
             contentType: "application/json; charset=utf-8",
             datatype: "JSON",
             type: "POST",
             success: function (data) {

                 $("#ServiceBranchCust").empty();
                 var grpdetl = data.d;



                 var opts = "";
                 opts = "<option value='0'>Select</option>";
                 for (i in grpdetl)

                     opts += "<option value='" + grpdetl[i].Id + "'>" + grpdetl[i].Name + "</option>";

                 $("#ServiceBranchCust").empty().append(opts);

             },
             error: function (data) {

                 jAlert("Please try again later");
             }
         });
     }

     

     //GetDocumentSegment();

});
function RemoveSegDisabled(textbox) {
    //if (textbox != 1) {
    //    if ($("#txtSeg" + textbox).val() != 0) {
    //        var id = textbox + 1;
    //        $("#txtSeg" + id).prop("disabled", false);
    //    }
    //    else {
    //        var id = textbox - 1;
    //        $("#txtSeg" + id).prop("disabled", true);
    //    }

    //}

    $("#txtSeg" + textbox).prop("disabled", false);
}

function GetDocumentSegment() {
    $.ajax({
        type: "POST",
        url: "/OMS/management/Master/View/Services/ViewCustomerService.asmx/GetDocumentSegment",
        //data: JSON.stringify({ reqStr: fname }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var list = msg.d;
            var listItems = [];
            if (list.length > 0) {

                var table = document.getElementById("TblDocumentSegment");

                var row = table.insertRow(0);
                for (var i = 0; i < list.length; i++) {
                    var id = '';
                    var name = '';
                    
                    id = list[i].Id;
                    name = list[i].Name;

                    var cell2 = row.insertCell(0);
                    if (list.length != id) {
                        var segid = parseFloat(id) + parseFloat(1);
                        cell2.innerHTML = "<input type='number' id=txtSeg" + id + " maxlength = '2' class='upper' value='0' Onblur='RemoveSegDisabled(" + segid + ")' />";

                    }
                    else {
                        cell2.innerHTML = "<input type='number' id=txtSeg" + id + " maxlength = '2' class='upper' value='0' Onblur='RemoveSegDisabled(" + id + ")' />";

                    }          

                    if (('txtSeg' + id) != "txtSeg1") {
                        $('#txtSeg' + id).attr('disabled', 'disabled');
                    }
                }


                var row = table.insertRow(0);
                for (var i = 0; i < list.length; i++) {
                    var id = '';
                    var name = '';

                    id = list[i].Id;
                    name = list[i].Name;


                    var cell1 = row.insertCell(0);
                    // var cell2 = row.insertCell(1);
                    cell1.innerHTML = "<label >" + name + "</label>";
                    //cell2.innerHTML = "<input type='text' id=txtSeg'" + id + "' maxlength = '18' class='upper' />";

                }
            }            
        }
    });
}

function ChangeCustomer()
{
    $.ajax({
        url: "/OMS/management/Master/View/Services/ViewCustomerService.asmx/getGroupmasterDetails",
        contentType: "application/json; charset=utf-8",
        datatype: "JSON",
        type: "POST",
        success: function (data) {
           
            $("#GroupCust").empty();
            var grpdetl = data.d;

            debugger;

            var opts = "";
            for (i in grpdetl)
                opts += "<option value='" + grpdetl[i].Id + "'>" + grpdetl[i].Name + "</option>";

            $("#GroupCust").empty().append(opts);
            //$.each(grpdetl, function (Id, Name) {
            //    $('#GroupCust')
            //        .append($("<option></option>")
            //                   .attr("value", Id)
            //                   .text(Name));
            //});
           // for (var i = 0; i < grpdetl.length; i++) {
           //     var opt = new Option(grpdetl[i].Name);
              
           //    $("#GroupCust").append(opt);
               
           //     debugger;
           //}
        },
        error: function (data) {
            debugger;
            jAlert("Please try again later");
        }
    });
}
