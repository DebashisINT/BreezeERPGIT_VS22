function TermsConditionsSave()
{
    //debugger;
    //var _returnStatus = true;
    //DefLiaPer = cdtDefectPerid.GetDate();
    //DefLiaPerRemarks = ctxtDefectPerid.GetText();
    //Liqdamageper = ctxtLiquiDamage.GetText();
    //LiqDamageApplicableDt = cdtLiqDmgAppliDt.GetDate();
    //PaymentTerms = ctxtPaymentTerms.GetText();
    //OrderType = ctxtOrderType.GetText();
    //NatureofWork = ctxtNatureWork.GetText();
    ////otherDet.BGGroup = ctxtBGGroup.GetText();
    ////otherDet.BGType = ctxtBGType.GetText();
    ////otherDet.Percentage = ctxtPercentage.GetText();
    ////otherDet.Value = ctxtValue.GetText();
    ////otherDet.Status = ctxtStatus.GetText();
    ////otherDet.ValidityFromDate = cdtBGValidFromD.GetDate();
    ////otherDet.ValidityToDate = cdtBGValidToD.GetDate();

    //if(DefLiaPer=="")
    //{
    //    _returnStatus = false;
    //    cdtDefectPerid.SetFocus();
    //}
    //if(DefLiaPerRemarks=="")
    //{
    //    _returnStatus = false;
    //    ctxtDefectPerid.SetFocus();
    //}

    //if(Liqdamageper=="")
    //{
    //    _returnStatus = false;
    //    ctxtLiquiDamage.SetFocus();
    //}
  
    //if(PaymentTerms=="")
    //{
    //    _returnStatus = false;
    //    ctxtPaymentTerms.SetFocus();
    //}
    //if(OrderType=="")
    //{
    //    _returnStatus = false;
    //    ctxtOrderType.SetFocus();
    //}
    //if(NatureofWork=="")
    //{
    //    ctxtNatureWork.SetFocus();
    //}
    //if(cGrdBGDetails.GetVisibleRowsOnPage()==0)
    //{
    //    _returnStatus = false;
    //}


    $("#ProjectTermsConditionseModal").hide();

}


//document.onkeydown = function (e) {
//    if (event.keyCode == 80 && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
//        TermsConditionsSave();
//        $("#ProjectTermsConditionseModal").hide();
//    }
//}


function ClickTermsconditions()
{
    setTimeout(function () {
        cdtDefectPerid.Focus();
    }, 500);
   
}

function GrdBGDetails_EndCallBack()
{
    //cGrdBGDetails.cpTermsBind
    if (cGrdBGDetails.cpTermsBind != "") {
        cdtDefectPerid.SetText(cGrdBGDetails.cpTermsBind.split('~')[0]);
        ctxtDefectPerid.SetText(cGrdBGDetails.cpTermsBind.split('~')[1]);
        ctxtLiquiDamage.SetText(cGrdBGDetails.cpTermsBind.split('~')[2]);
        cdtLiqDmgAppliDt.SetText(cGrdBGDetails.cpTermsBind.split('~')[3]);
        ctxtPaymentTerms.SetText(cGrdBGDetails.cpTermsBind.split('~')[4]);
        ctxtOrderType.SetText(cGrdBGDetails.cpTermsBind.split('~')[5]);
        ctxtNatureWork.SetText(cGrdBGDetails.cpTermsBind.split('~')[6]);
        cdtDefectPeridToDate.SetText(cGrdBGDetails.cpTermsBind.split('~')[7])
    }
}


function CallTaggedOrderTermsCondition(id,type)
{
    cGrdBGDetails.PerformCallback('BindTermsAndCondition' + '~' + id + '~' + type);
}

function TermsConditionscancel()
{
    $("#ProjectTermsConditionseModal").hide();
}


function ClickOnEdit(val) {
}

//function OnClickDelete(val) {
//}



function grid_EndCallBack(s, e) {


}

function GetDateFormat(today) {
    if (today != "") {
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!

        var yyyy = today.getFullYear();
        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }
        //  today = dd + '-' + mm + '-' + yyyy;
        today = yyyy + '-' + mm + '-' + dd;
    }

    return today;
}
function AddTermsDetails()
{
    debugger;
   
    var BGGroup = ctxtBGGroup.GetText();

    if (BGGroup == "")
    {
        ctxtBGGroup.SetFocus();
        return;
    }

  var BGType = ctxtBGType.GetText();
  var Percentage = ctxtPercentage.GetText();
  var Value = ctxtValue.GetText();
  var Status = ctxtStatus.GetText();

  var ValidityFromDate = cdtBGValidFromD.GetValue();
  var ValidityToDate = cdtBGValidToD.GetValue();
  if (ValidityFromDate != null) {
      ValidityFromDate = GetDateFormat(ValidityFromDate);
  }
  if (ValidityToDate != null) {
      ValidityToDate = GetDateFormat(ValidityToDate);
  }
  var Terms_BankGuaranteeSL = $('#hdnGuid').val();

    $.ajax({
        type: "POST",
        url: "Services/ProjectTermsService.asmx/AddData",
        data: JSON.stringify({ Terms_BankGuaranteeSL:Terms_BankGuaranteeSL, BGGroup: BGGroup, BGType: BGType, Percentage: Percentage, Value: Value, Status: Status, ValidityFromDate: ValidityFromDate, ValidityToDate: ValidityToDate }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var list = msg.d;
           
            jAlert(msg.d);
            ctxtBGGroup.SetText('');
            ctxtBGType.SetText('');
            ctxtPercentage.SetText('');
            ctxtValue.SetText('');
            ctxtStatus.SetText('');
            cdtBGValidFromD.Clear();
            cdtBGValidToD.Clear();
            cGrdBGDetails.PerformCallback();
            

        }
    });
}



function ClickOnEdit(val) {
    $.ajax({
        type: "POST",
        url: "Services/ProjectTermsService.asmx/EditData",
        data: JSON.stringify({ Terms_BankGuaranteeSL: val }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var list = msg.d;

            $('#hdnGuid').val(val);
            ctxtBGGroup.SetText(msg.d.BGGroup);
            ctxtBGType.SetText(msg.d.BGType);

            cGrdBGDetails.PerformCallback();

        }
    });
}


function OnClickDelete(val) {
    jConfirm('Confirm Delete?', 'Alert', function (r) {
        if (r) {
            $.ajax({
                type: "POST",
                url: "Services/ProjectTermsService.asmx/DeleteData",
                data: JSON.stringify({ Terms_BankGuaranteeSL: val }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                   
                    jAlert(msg.d);
                  
                    cGrdBGDetails.PerformCallback();

                }
            });
        }
        else {

        }
    });
}


//function ProjectTermsType()
//{

//    GetTermsConditions(Docid,Doctype);

//}