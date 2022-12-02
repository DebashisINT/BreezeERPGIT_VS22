function RefreshAll() {
    HeaderReferesh();
    LoadTopSaleman();
    LoadCustomer();
}

function HeaderReferesh() {

    var OtherDetails = {}
    OtherDetails.FromDtae = moment(cFormDate.GetDate()).format('YYYY-MM-DD');
    OtherDetails.toDate = moment(ctoDate.GetDate()).format('YYYY-MM-DD');
    OtherDetails.branchid = cddlBranch.GetValue();
    OtherDetails.ProdClass = $('#hdnClassId').val();
    OtherDetails.Prodid = $('#hdnProductId').val();
    $.ajax({
        type: "POST",
        url: "PurchaseDb.aspx/GetPurchaseBalance",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var retObj = msg.d;

            $("#totSale").text(retObj.TotalSale);
            $("#totDue").text(retObj.TotDue);
            $("#totAdvance").text(retObj.totAdvance);
            $("#totOrder").text(retObj.totOrder);
        }
    });
}


function LoadTopSaleman() {

    var OtherDetails = {}
    OtherDetails.FromDtae = moment(cFormDate.GetDate()).format('YYYY-MM-DD');
    OtherDetails.toDate = moment(ctoDate.GetDate()).format('YYYY-MM-DD');
    OtherDetails.branchid = cddlBranch.GetValue();
    OtherDetails.ProdClass = $('#hdnClassId').val();
    OtherDetails.Prodid = $('#hdnProductId').val();
    $.ajax({
        type: "POST",
        url: "PurchaseDb.aspx/GetTopNPurchaseMan",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var retObj = msg.d;

            console.log(retObj);
            $('#PurchaseManPanel').removeClass('chartboxes');
            var PieObject = {
                "type": "pie",
                "theme": "light",
                "innerRadius": "40%",
                "gradientRatio": [-0.4, -0.4, -0.4, -0.4, -0.4, -0.4, 0, 0.1, 0.2, 0.1, 0, -0.2, -0.5],
                "dataProvider": [],
                "balloonText": "[[value]]",
                "valueField": "AmtVal",
                "titleField": "Name",
                "balloon": {
                    "drop": true,
                    "adjustBorderColor": false,
                    "color": "#FFFFFF",
                    "fontSize": 16
                },
                "export": {
                    "enabled": true
                }
            };
            PieObject.dataProvider = msg.d;
            var chart = AmCharts.makeChart("PurchaseManPanel", PieObject);
        }
    });
}


function LoadCustomer() {
    var OtherDetails = {}
    OtherDetails.FromDtae = moment(cFormDate.GetDate()).format('YYYY-MM-DD');
    OtherDetails.toDate = moment(ctoDate.GetDate()).format('YYYY-MM-DD');
    OtherDetails.branchid = cddlBranch.GetValue();
    OtherDetails.ProdClass = $('#hdnClassId').val();
    OtherDetails.Prodid = $('#hdnProductId').val();
    $.ajax({
        type: "POST",
        url: "PurchaseDb.aspx/GetTopNCustomer",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            console.log();
            $('#topNCustomer').removeClass('chartboxes');
            var chartobj = {
                "theme": "light",
                "type": "serial",
                "dataProvider": [],
                "valueAxes": [{
                    "title": "Amount in Rs"
                }],
                "graphs": [{
                    "balloonText": "Amount in [[category]]:[[value]]",
                    "fillAlphas": 1,
                    "lineAlpha": 0.2,
                    "title": "Amount",
                    "type": "column",
                    "valueField": "AmtVal"
                }],
                "depth3D": 20,
                "angle": 30,
                "rotate": true,
                "categoryField": "Name",
                "categoryAxis": {
                    "gridPosition": "start",
                    "fillAlpha": 0.05,
                    "position": "left"
                },
                "export": {
                    "enabled": true
                }
            };

            chartobj.dataProvider = msg.d;
            var chart = AmCharts.makeChart("topNCustomer", chartobj);
        }
    });
}



function changeViewTopProd() {
    if (document.getElementById('PurchaseManPanel').style.height != "100%") {
        $('#topProd').addClass("fullscreen");
        document.getElementById('PurchaseManPanel').style.height = '100%';
    } else {
        $('#topProd').removeClass("fullscreen");
        document.getElementById('PurchaseManPanel').style.height = '350px';
    }
}


function changeViewTopVend() {
    if (document.getElementById('PurchaseManPanel').style.height != "100%") {
        $('#topVend').addClass("fullscreen");
        document.getElementById('PurchaseManPanel').style.height = '100%';
    } else {
        $('#topVend').removeClass("fullscreen");
        document.getElementById('PurchaseManPanel').style.height = '350px';
    }
}

// week work WTD
Date.prototype.getWeek = function () {
    var target = new Date(this.valueOf());
    var dayNr = (this.getDay() + 6) % 7;
    target.setDate(target.getDate() - dayNr + 3);
    var firstThursday = target.valueOf();
    target.setMonth(0, 1);
    if (target.getDay() != 4) {
        target.setMonth(0, 1 + ((4 - target.getDay()) + 7) % 7);
    }
    return 1 + Math.ceil((firstThursday - target) / 604800000);
}

function getDateRangeOfWeek(weekNo) {
    var d1 = new Date();
    numOfdaysPastSinceLastMonday = eval(d1.getDay() - 1);
    d1.setDate(d1.getDate() - numOfdaysPastSinceLastMonday);
    var weekNoToday = d1.getWeek();
    var weeksInTheFuture = eval(weekNo - weekNoToday);
    d1.setDate(d1.getDate() + eval(7 * weeksInTheFuture));
    var rangeIsFrom = eval(d1.getMonth() + 1) + "/" + d1.getDate() + "/" + d1.getFullYear();
    d1.setDate(d1.getDate() + 6);
    var rangeIsTo = eval(d1.getMonth() + 1) + "/" + d1.getDate() + "/" + d1.getFullYear();
    return rangeIsFrom + " to " + rangeIsTo;
};

Date.prototype.whatWeek = function () {
    var onejan = new Date(this.getFullYear(), 0, 1);
    var today = new Date(this.getFullYear(), this.getMonth(), this.getDate());
    var dayOfYear = ((today - onejan + 86400000) / 86400000);
    return Math.ceil(dayOfYear / 7)
};
// QTD
function quarter_of_the_year(date) {
    var month = date.getMonth() + 1;
    return (Math.ceil(month / 3));
}





$(document).ready(function () {
    $("#dynamiDate").change(function () {
        var value = $(this).val();
        
        if (value == "0") {
            cFormDate.SetEnabled(true);
            ctoDate.SetEnabled(true);
        } else if (value == "WTD") {
            var today = new Date();
            var currentWeekNumber = today.whatWeek();
            var wTD = getDateRangeOfWeek(currentWeekNumber);
            var slicd = wTD.split(" ");
            var fst = slicd[0];
            var lst = slicd[2];
            cFormDate.SetDate(new Date(fst));
            ctoDate.SetDate(new Date(lst));
            cFormDate.SetEnabled(false);
            ctoDate.SetEnabled(false);
        } else if (value == "MTD") {
            var date = new Date();
            var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
            cFormDate.SetDate(new Date(firstDay));
            ctoDate.SetDate(new Date());
            cFormDate.SetEnabled(false);
            ctoDate.SetEnabled(false);
        } else if (value == "YTD") {
            var date = new Date();
            var firstDay = new Date(date.getFullYear(), 3, 1);
            cFormDate.SetDate(new Date(firstDay));
            ctoDate.SetDate(new Date());
            cFormDate.SetEnabled(false);
            ctoDate.SetEnabled(false);
        } else if (value == "QTD") {
            var yr = new Date().getFullYear();
            //1st QTD

            var dateFrom1 = new Date("04/01/" + yr);
            var dateTo1 = new Date("06/30/" + yr);
            // 2nd QTD
            var dateFrom2 = new Date("07/01/" + yr);
            var dateTo2 = new Date("09/30/" + yr);
            // 3rd 
            var dateFrom3 = new Date("10/01/" + yr);
            var dateTo3 = new Date("12/31/" + yr);
            //4th
            var dateFrom4 = new Date("01/01/" + yr);
            var dateTo4 = new Date("03/31/" + yr);

           var Today = new Date()
            

           if (Today > dateFrom1 && Today < dateTo1) {
                cFormDate.SetDate(new Date(dateFrom1));
                ctoDate.SetDate(new Date());
           } else if (Today > dateFrom2 && Today < dateTo2) {
                cFormDate.SetDate(new Date(dateFrom2));
                ctoDate.SetDate(new Date());
           } else if (Today > dateFrom3 && Today < dateTo3) {
                cFormDate.SetDate(new Date(dateFrom3));
                ctoDate.SetDate(new Date());
           } else if (Today > dateFrom4 && Today < dateTo4) {
                cFormDate.SetDate(new Date(dateFrom4));
                ctoDate.SetDate(new Date());
            } else {

            }
            cFormDate.SetEnabled(false);
            ctoDate.SetEnabled(false);
            
        }else {
            cFormDate.SetEnabled(false);
            ctoDate.SetEnabled(false);
        }
    })
});

function ddCheck(dfrm, dto) {
    var dateCheck = new Date().toLocaleDateString();
    var d1 = dfrm.split("/");
    var d2 = dto.split("/");
    var c = dateCheck.split("/");
    var from = new Date(d1[2], parseInt(d1[1]) - 1, d1[0]);  // -1 because months are from 0 to 11
    var to = new Date(d2[2], parseInt(d2[1]) - 1, d2[0]);
    var check = new Date(c[2], parseInt(c[1]) - 1, c[0]);
    if (check > dfrm && check < dto) {
        return 1;
    }   
}


//<%-- For Class multiselection--%>
 
 
     $(document).ready(function () {
         $('#ClassModel').on('shown.bs.modal', function () {
             $('#txtClassSearch').focus();
         })

     })
var ClassArr = new Array();
$(document).ready(function () {
    var ClassObj = new Object();
    ClassObj.Name = "ClassSource";
    ClassObj.ArraySource = ClassArr;
    arrMultiPopup.push(ClassObj);
})
function ClassButnClick(s, e) {
    $('#ClassModel').modal('show');
}

function Class_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#ClassModel').modal('show');
    }
}

function Classkeydown(e) {
    var OtherDetails = {}

    if ($.trim($("#txtClassSearch").val()) == "" || $.trim($("#txtClassSearch").val()) == null) {
        return false;
    }
    OtherDetails.SearchKey = $("#txtClassSearch").val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {

        var HeaderCaption = [];
        HeaderCaption.push("Class Name");


        if ($("#txtClassSearch").val() != "") {
            callonServerM("../Service/Mastered.asmx/GetClass", OtherDetails, "ClassTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ClassSource");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[dPropertyIndex=0]"))
            $("input[dPropertyIndex=0]").focus();
    }
}

function SetfocusOnseach(indexName) {
    if (indexName == "dPropertyIndex")
        $('#txtClassSearch').focus();
    else
        $('#txtClassSearch').focus();
}

function SetSelectedValues(Id, Name, ArrName) {
    if (ArrName == 'ClassSource') {
        var key = Id;
        if (key != null && key != '') {
            $('#ClassModel').modal('hide');
            ctxtClass.SetText(Name);
            $('#hdnClassId').val(key);


            ctxtProdName.SetText('');
            $('#txtProdSearch').val('')

            var OtherDetailsProd = {}
            OtherDetailsProd.SearchKey = 'undefined text';
            OtherDetailsProd.ClassID = '';
            var HeaderCaption = [];
            HeaderCaption.push("Code");
            HeaderCaption.push("Name");
            HeaderCaption.push("Hsn");

            callonServerM("../Service/Mastered.asmx/GetClassWiseProduct", OtherDetailsProd, "ProductTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ProductSource");

        }
        else {
            ctxtClass.SetText('');
            //GetObjectID('hdnClassId').value = '';
            $('#hdnClassId').val('');
        }
    }
    else if (ArrName == 'ProductSource') {
        var key = Id;
        if (key != null && key != '') {
            $('#ProdModel').modal('hide');
            ctxtProdName.SetText(Name);
            //GetObjectID('').value = key;
            $('#hdnProductId').val(key);
        }
        else {
            ctxtProdName.SetText('');
            //GetObjectID('hdnProductId').value = '';
            $('#hdnProductId').val("");
        }
    }
}


//<%-- For Class multiselection--%>

var ProdArr = new Array();
$(document).ready(function () {
    var ProdObj = new Object();
    ProdObj.Name = "ProductSource";
    ProdObj.ArraySource = ProdArr;
    arrMultiPopup.push(ProdObj);
})

function ProductButnClick(s, e) {
    $('#ProdModel').modal('show');
}

function Product_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#ProdModel').modal('show');
    }
}
$(document).ready(function () {
    $('#ProdModel').on('shown.bs.modal', function () {
        $('#txtProdSearch').focus();
    })
})
function Productkeydown(e) {
    var OtherDetails = {}

    if ($.trim($("#txtProdSearch").val()) == "" || $.trim($("#txtProdSearch").val()) == null) {
        return false;
    }
    OtherDetails.SearchKey = $("#txtProdSearch").val();
    OtherDetails.ClassID = "";

    if (e.code == "Enter" || e.code == "NumpadEnter") {

        var HeaderCaption = [];
        HeaderCaption.push("Code");
        HeaderCaption.push("Name");
        HeaderCaption.push("Hsn");

        if ($("#txtProdSearch").val() != "") {
            callonServerM("../Service/Mastered.asmx/GetClassWiseProduct", OtherDetails, "ProductTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ProductSource");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[dPropertyIndex=0]"))
            $("input[dPropertyIndex=0]").focus();
    }
}

function SetfocusOnseach(indexName) {
    if (indexName == "dPropertyIndex")
        $('#txtProdSearch').focus();
    else
        $('#txtProdSearch').focus();
}
   