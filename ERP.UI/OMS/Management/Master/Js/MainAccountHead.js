//====================================================Revision History =========================================================================
//1.0   V2.0.41 	Priti	09-11-2023	0026978:The mapping/unmapping of the HSN/SAC code to be restricted in Account Head, once there is transaction exists
//====================================================End Revision History=====================================================================

function OnMoreInfoClick(id) {
    location.href = "MainAccountAddEdit.aspx?id=" + id;
}
//Rev work start 21.06.2022 Mantise no:0024974: Copy in Account Head Master module with Rights based on 'ADD' rights
function OnCopyClick(id) {
    location.href = "MainAccountAddEdit.aspx?key=Copy&id=" + id;
}

//Rev work close 21.06.2022 Mantise no:0024974: Copy in Account Head Master module with Rights based on 'ADD' rights
function selectbranch(id)
{
    cbranchGrid.PerformCallback(id);
    cBranchSelectPopup.Show(); 
}

function OnClickDelete(id) {

    jConfirm("Confirm Delete?", "Title", function (ret) {

        if (ret == true)
        {
            $.ajax({
                type: "POST",
                url: "MainAccountHead.aspx/DeleteMainAccount",
                data: JSON.stringify({ 'acnt_id': id }),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    console.log(response);
                    if (response.d) {
                        if (response.d == "true") {
                            jAlert("Deleted Successfully");
                            grid.Refresh();
                        }
                        else {
                            jAlert(response.d);
                        }

                    }

                },
                error: function (response) {

                    console.log(response);
                }
            });

        }

    });

            
}

// Mantis Issue 24953
function OnPostingTypeClick(id)
{
    $("#hdnAccountTypeID").val(id);
    
    $.ajax({
        type: "POST",
        url: "MainAccountHead.aspx/GetSelectedPostingType",
        data: JSON.stringify({ 'AccountTypeID': id }),
        async: false,
        contentType: "application/json; charset=utf-8",
        // dataType: "json",
        success: function (response) {
            ccmbPostingType.SetValue(response.d);
            cPostingTypePopup.Show();

        },
        error: function (response) {

            console.log(response);
        }
    });

    

}

function PostingTypeSaveClick() {

    var AccountTypeID = $("#hdnAccountTypeID").val();
    var PostingTypeID = ccmbPostingType.GetValue();

    $.ajax({
        type: "POST",
        url: "MainAccountHead.aspx/SavePostingType",
        data: JSON.stringify({ 'AccountTypeID': AccountTypeID, 'PostingTypeID': PostingTypeID }),
        async: false,
        contentType: "application/json; charset=utf-8",
       // dataType: "json",
        success: function (response) {
            console.log(response);
            if (response.d) {
                if (response.d == "true") {
                    jAlert("Posting Type Updated Successfully");
                    grid.Refresh();
                }
                else {
                    jAlert(response.d);
                }
                cPostingTypePopup.Hide();
            }

        },
        error: function (response) {

            console.log(response);
        }
    });


}

function ClosePostingTypePopup() {
    cPostingTypePopup.Hide();
}
// End of Mantis Issue 24953

function fn_AllowonlyNumeric(s, e) {
    var theEvent = e.htmlEvent || window.event;
    var key = theEvent.keyCode || theEvent.which;
    var keychar = String.fromCharCode(key);
    if (key == 9 || key == 37 || key == 38 || key == 39 || key == 40 || key == 8) { //tab/ Left / Up / Right / Down Arrow, Backspace, Delete keys
        return;
    }
    var regex = /[0-9]/;

    if (!regex.test(keychar)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault)
            theEvent.preventDefault();
    }
}

function DeducteStatusBasedOnPan()
{
    if(ctxtNumber.GetText()!="")
    {
        ctxtNameAsPerPan.SetEnabled(true);
    }
    else
    {
        ctxtNameAsPerPan.SetEnabled(false);
    }
}



function Gstin2TextChanged(s, e) {

    if (!e.htmlEvent.ctrlKey) {
        if (e.htmlEvent.key != 'Control') {
            s.SetText(s.GetText().toUpperCase());
        }
    }

}





    //Code for UDF Control 
    function unSelectAllBranch() {
        cbranchGrid.UnselectRows();
    }

function SelectAllBranch() {
    cbranchGrid.SelectRows();
}

function OpenUdf() {
    if (document.getElementById('IsUdfpresent').value == '0') {
        jAlert("UDF not define.");
    }
    else {
        var keyVal = document.getElementById('Keyval_internalId').value;
        var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=AH&&KeyVal_InternalID=' + keyVal;
        popup.SetContentUrl(url);
        popup.Show();
    }
    return true;
}

// End Udf Code
function branchGridEndCallBack() {
    if (cbranchGrid.cpReceviedString) {
        if (cbranchGrid.cpReceviedString == 'SetAllRecordToDataTable') {
            cBranchSelectPopup.Hide();
        }
    }

}

function CmbBranchChanged() {
    var branchCode = CmbBranch.GetValue();
    if (branchCode == 0) {
        $('#MultiBranchButton').show();
    }
    else {
        $('#MultiBranchButton').hide();
    }
}

function MultiBranchClick() {
    cbranchGrid.PerformCallback('SetAllSelectedRecord');
    cBranchSelectPopup.Show();
}

function SaveSelectedBranch() {
    cbranchGrid.PerformCallback('SetAllRecordToDataTable');
}

//$(document).ready(function () {
//    var acval = $('#ASPxComboBox1').val();
//    alert(acval);

//});
//function SignOff()
// {
//     window.parent.SignOff();
// }
// function height()
// {
//     if(document.body.scrollHeight>=900)
//         window.frameElement.height = document.body.scrollHeight+200;
//     else
//         window.frameElement.height = '900px';
//     window.frameElement.Width = document.body.scrollWidth;
// }

function DisablePaymentType(s, e) {
    if (cPaymenttype.GetValue() == null) {
        cPaymenttype.SetValue('None');
    }
    cPaymenttype.SetEnabled(false);
}


function changeControlState(state) {

    txtAccountCode.SetEnabled(state);
    ASPxComboBox1.SetEnabled(state);
    //    combAccountGroup.SetEnabled(state);
    ASPxComboBox2.SetEnabled(state);
    txtBankAccountNo.SetEnabled(state);
    comboCompanyName.SetEnabled(state);
    CmbBranch.SetEnabled(state);
    CmbSubLedgerType.SetEnabled(state);
    txtRateofIntrest.SetEnabled(state);
    cmb_tdstcs.SetEnabled(state);

}

function OntxtAccountCodeInit(s, e) {



    if (txtAccountCode.GetText().length > 0) {
        if (s.GetText().toLowerCase().indexOf("systm") >= 0) {
            changeControlState(false);
        } else {
            changeControlState(true);
            txtAccountCode.SetEnabled(false);
        }
    } else {
        changeControlState(true);
    }
}
$('#MainAccountGrid_tccell1_18').attr('style', 'text-align: center !important');
function EndCall(obj) {

}
function CallList(obj1, obj2, obj3) {
    FieldName = 'Label1';
    ajax_showOptions(obj1, obj2, obj3);
}

function Load(obj) {

    document.getElementById("tdsrate").style.display = 'none';
    document.getElementById("tdsrate1").style.display = 'none';
    document.getElementById("fbtrate").style.display = 'none';
    document.getElementById("fbtrate1").style.display = 'none';
}

function LoadSubledger(obj, obj1, obj2, obj3) {
    if (obj1 != 'None') {
        var aaaa = obj;
        url1 = "frm_Subledger.aspx?id=" + aaaa + "&name=" + obj1 + "&accountType=" + obj2 + "&accountcode=" + obj3;
        window.location.href = url1;
        //OnMoreInfoClick(url1, "Modify Sub Ledger", '990px', '520px', "Y");
    }

}

function AccopuntType(obj) {
    //if (obj != "-1") {

    //    combAccountGroup.PerformCallback(obj);
    //}
    if (obj == '0') {
        //ASPxComboBox2.SetSelectedIndex(0);

        document.getElementById("trBankCashType").style.display = 'block';
        //ASPxComboBox2.SetSelectedIndex(0);
        //document.getElementById("tdBankCashType").style.display = 'inline';
        //document.getElementById("tdBankCashType").style.display = 'block';

        //document.getElementById("tdBankCashType1").style.display = 'inline';
        //document.getElementById("tdBankCashType1").style.display = 'block';

        //document.getElementById("tdBankAccountNo").style.display = 'inline';
        document.getElementById("tdBankAccountNo").style.display = 'block';
        document.getElementById("clsPaymentType").style.display = 'block';

        //document.getElementById("tdBankAccountNo1").style.display = 'inline';
        //document.getElementById("tdBankAccountNo1").style.display = 'block';

        document.getElementById("tdBankAccountType").style.display = 'inline';
        document.getElementById("tdBankAccountType").style.display = 'none';// modified by atish from table-cell to none for not showing for asset account type

        document.getElementById("tdBankAccountType1").style.display = 'inline';
        document.getElementById("tdBankAccountType1").style.display = 'none';// modified by atish from table-cell to none for not showing for asset account type

        document.getElementById("tdSubledgertype").style.display = 'none';
        //document.getElementById("tdSubledgertype1").style.display = 'none';
        //document.getElementById("tdExchangeSeg").style.display = 'inline';
        //document.getElementById("tdExchangeSeg").style.display = 'table-cell';

        //document.getElementById("tdExchangeSeg1").style.display = 'inline';
        //document.getElementById("tdExchangeSeg1").style.display = 'table-cell';
        document.getElementById("tddepretion").style.display = 'none';
        document.getElementById("tdroi").style.display = 'none';
        document.getElementById("tdroi1").style.display = 'none';
        document.getElementById("tdtdsapprate").style.display = 'none';
        document.getElementById("tdtdsapprate1").style.display = 'none';
        var asettypeindex = ASPxComboBox2.GetSelectedIndex();
        BankCashType(asettypeindex);
        combAccountGroup.PerformCallback(obj);
    }
    else {

        //ASPxComboBox2.SetSelectedIndex(0);
        //document.getElementById("tdBankCashType").style.display = 'none';
        //document.getElementById("tdBankCashType1").style.display = 'none';
        document.getElementById("trBankCashType").style.display = 'none';
        document.getElementById("tdBankAccountNo").style.display = 'none';
        // document.getElementById("clsPaymentType").style.display = 'none';
        //document.getElementById("tdBankAccountNo1").style.display = 'none';
        document.getElementById("tdBankAccountType").style.display = 'none';
        document.getElementById("tdBankAccountType1").style.display = 'none';
        document.getElementById("tdSubledgertype").style.display = 'inline';
        document.getElementById("tdSubledgertype").style.display = 'table-cell';

        //document.getElementById("tdSubledgertype1").style.display = 'block';
        //document.getElementById("tdSubledgertype1").style.display = 'block';

        //document.getElementById("tdExchangeSeg").style.display = 'none';
        //document.getElementById("tdExchangeSeg1").style.display = 'none';
        document.getElementById("tddepretion").style.display = 'none';
        $("#trBankCashType").addClass("classname");
        document.getElementById("tdroi").style.display = 'inline';
        document.getElementById("tdroi1").style.display = 'inline';
        document.getElementById("tdtdsapprate").style.display = 'block';
        document.getElementById("tdtdsapprate1").style.display = 'block';
        var asettypeindex = ASPxComboBox2.GetSelectedIndex();
        BankCashType(asettypeindex);
        combAccountGroup.PerformCallback(obj);

        // document.getElementById("clsPaymentType").style.display = 'block';
        ItemaOther();
    }
}
function ItemsBank() {

    var comboitem = cPaymenttype.FindItemByValue('None');
    if (comboitem != undefined && comboitem != null) {
        cPaymenttype.RemoveItem(comboitem.index);
    }
    var comboitem = cPaymenttype.FindItemByValue('Card');
    if (comboitem != undefined && comboitem != null) {
        cPaymenttype.RemoveItem(comboitem.index);
    }
    var comboitem = cPaymenttype.FindItemByValue('Coupon');
    if (comboitem != undefined && comboitem != null) {
        cPaymenttype.RemoveItem(comboitem.index);
    }
    var comboitem = cPaymenttype.FindItemByValue('Etransfer');
    if (comboitem != undefined && comboitem != null) {
        cPaymenttype.RemoveItem(comboitem.index);
    }
    var comboitem = cPaymenttype.FindItemByValue('LedgOut');
    if (comboitem != undefined && comboitem != null) {
        cPaymenttype.RemoveItem(comboitem.index);
    }
    var comboitem = cPaymenttype.FindItemByValue('LedgIn');
    if (comboitem != undefined && comboitem != null) {
        cPaymenttype.RemoveItem(comboitem.index);
    }
    var comboitem = cPaymenttype.FindItemByValue('PrcFee');
    if (comboitem != undefined && comboitem != null) {
        cPaymenttype.RemoveItem(comboitem.index);
    }
    var comboitem = cPaymenttype.FindItemByValue('EmiCharge');
    if (comboitem != undefined && comboitem != null) {
        cPaymenttype.RemoveItem(comboitem.index);
    }
    cPaymenttype.AddItem("None", "None");
    cPaymenttype.AddItem("Card", "Card");
    cPaymenttype.AddItem("Coupon", "Coupon");
    cPaymenttype.AddItem("Etransfer", "Etransfer");
}
function ItemaOther() {
    //<dxe:ListEditItem Text="None" Value="None" Selected="true" />
    //<dxe:ListEditItem Text="Card" Value="Card" />
    //<dxe:ListEditItem Text="Coupon" Value="Coupon" />
    //<dxe:ListEditItem Text="Etransfer" Value="Etransfer" />
    //<dxe:ListEditItem Text="Ledger for Interstate Stk-Out" Value="LedgOut" />
    //<dxe:ListEditItem Text="Ledger for Interstate Stk-In" Value="LedgIn" />

    //<dxe:ListEditItem Text="Finance Processing Fee" Value="PrcFee" />
    //<dxe:ListEditItem Text="Finance Other Charges Emi" Value="EmiCharge" />

    var comboitem = cPaymenttype.FindItemByValue('None');
    if (comboitem != undefined && comboitem != null) {
        cPaymenttype.RemoveItem(comboitem.index);
    }
    var comboitem = cPaymenttype.FindItemByValue('Card');
    if (comboitem != undefined && comboitem != null) {
        cPaymenttype.RemoveItem(comboitem.index);
    }
    var comboitem = cPaymenttype.FindItemByValue('Coupon');
    if (comboitem != undefined && comboitem != null) {
        cPaymenttype.RemoveItem(comboitem.index);
    }
    var comboitem = cPaymenttype.FindItemByValue('Etransfer');
    if (comboitem != undefined && comboitem != null) {
        cPaymenttype.RemoveItem(comboitem.index);
    }
    var comboitem = cPaymenttype.FindItemByValue('Etransfer');
    if (comboitem != undefined && comboitem != null) {
        cPaymenttype.RemoveItem(comboitem.index);
    }
    var comboitem = cPaymenttype.FindItemByValue('LedgOut');
    if (comboitem != undefined && comboitem != null) {
        cPaymenttype.RemoveItem(comboitem.index);
    }
    var comboitem = cPaymenttype.FindItemByValue('LedgIn');
    if (comboitem != undefined && comboitem != null) {
        cPaymenttype.RemoveItem(comboitem.index);
    }
    var comboitem = cPaymenttype.FindItemByValue('PrcFee');
    if (comboitem != undefined && comboitem != null) {
        cPaymenttype.RemoveItem(comboitem.index);
    }
    var comboitem = cPaymenttype.FindItemByValue('EmiCharge');
    if (comboitem != undefined && comboitem != null) {
        cPaymenttype.RemoveItem(comboitem.index);
    }
    cPaymenttype.AddItem("Ledger for Interstate Stk-Out", "LedgOut");
    cPaymenttype.AddItem("Ledger for Interstate Stk-In", "LedgIn");
    cPaymenttype.AddItem("Finance Processing Fee", "PrcFee");
    cPaymenttype.AddItem("Finance Other Charges Emi", "EmiCharge");
}





function BankAccountType(obj) {
    if (obj == '2') {

        document.getElementById("tdExchangeSeg").style.display = 'none';
        document.getElementById("tdExchangeSeg1").style.display = 'none';

    }
    else if (obj == 'A') {
        document.getElementById("tdExchangeSeg").style.display = 'none';
        document.getElementById("tdExchangeSeg1").style.display = 'none';
    }
    else {

        document.getElementById("tdExchangeSeg").style.display = 'block';


        document.getElementById("tdExchangeSeg1").style.display = 'block';

    }
}

//function ExchangeSegment(obj) {

//    if (obj == 'A') {
//        document.getElementById("trExchange").style.display = 'none';
//        document.getElementById("trExchange1").style.display = 'none';
//    }
//    else if (obj == 'S') {
//        document.getElementById("trExchange").style.display = 'block';


//        document.getElementById("trExchange1").style.display = 'block';

//        comboSegment.PerformCallback();

//    }
//    else {

//        document.getElementById("trExchange").style.display = 'block';


//        document.getElementById("trExchange1").style.display = 'block';

//        comboSegment.SetValue(obj);
//        document.getElementById('hdSegment').value = obj
//    }
//}

function TDSApplicableFun(obj) {
    if (obj == 1) {
        document.getElementById("tdsrate").style.display = 'block';

        //document.getElementById("tdsrate1").style.display = 'inline'; 
    }
    else {
        document.getElementById("tdsrate").style.display = 'none';
        //document.getElementById("tdsrate1").style.display = 'none'; 
    }
}

function FBTApplicableFun(obj) {
    if (obj == 1) {
        document.getElementById("fbtrate").style.display = 'block';
        document.getElementById("fbtrate1").style.display = 'block';

    }
    else {
        document.getElementById("fbtrate").style.display = 'none';
        document.getElementById("fbtrate1").style.display = 'none';
    }
}

//function SubLedgerTypeFun(obj) {
//    if (obj == '11') {
//        document.getElementById("addCustomLedger").style.display = 'inline';
//    }
//    else {
//        document.getElementById("addCustomLedger").style.display = 'none';
//    }
//}
function aaa(obj) {
    document.getElementById("Subledger").style.display = 'inline';
    document.getElementById("main").style.display = 'none';
}
function ShowHideFilter(obj) {
    var chk = document.getElementById("chkSysAccount");
    if (chk.checked == true)
        grid.PerformCallback(obj + '~T');
    else
        grid.PerformCallback(obj + '~F');
}

function Show(Keyvalue) {

    var url = "frm_OpeningBalance.aspx?id=" + Keyvalue + "";
    window.location.href = url;
    //popup.SetContentUrl(url);
    //popup.Show();
}
function showhistory(obj) {
    //var URL = 'Account_Document.aspx?idbldng=' + obj;

    //editwin = dhtmlmodal.open("Editbox", "iframe", URL, "Document", "width=1000px,height=400px,center=0,resize=1,top=-1", "recal");
    //editwin.onclose = function () {
    //    grid.PerformCallback();
    //}

    //var url = 'Contact_Document.aspx?idbldng=' + obj;
    //popup.SetContentUrl(url);
    //popup.Show();
    // .............................Code Commented and Added by Sam on 02122016. to use page instead of popup ..................................... 

    //var URL = "Account_Document.aspx?idbldng=" + obj + ""; 
    //popupdoc.SetContentUrl(URL); 
    //popupdoc.Show();
    //popupdoc.SetHeaderText('Add Document');

    var URL = "Contact_Document.aspx?idbldng=" + obj + "";
    window.location.href = URL;

    // .............................Code Above Commented and Added by Sam on 29112016...................................... 
}
function ShowAssetDetail(KeyVal, Val) {


    // .............................Code Commented and Added by Sam on 02122016. to use page instead of popup due to generate iframe ..................................... 
    var url = "AssetDetail.aspx?id=" + Val + "";
    window.location.href = url;
    //popup.SetContentUrl(url);
    //popup.Show();
    //popup.SetHeaderText('Asset Details');

    // .............................Code Above Commented and Added by Sam on 02122016...................................... 

    //............................. Previous old Comment ......................
    //var url = "AssetDetail.aspx?id=" + Val + "";
    //OnMoreInfoClick(url, "Asset Details", '990px', '510px', "Y"); 
    //        editwin=dhtmlmodal.open("Editbox", "iframe", url,"Add/Modify AssetDetail" , "width=900px,height=500px,center=1,resize=1,scrolling=2,top=500", "recal")
    //        document.getElementById('ctl00_ContentPlaceHolder1_Headermain1_cmbSegment').style.visibility='hidden';
    //        editwin.onclose=function()
    //         {
    //         document.getElementById('ctl00_ContentPlaceHolder1_Headermain1_cmbSegment').style.visibility='visible';
    //         }
    //         return false;
    //.............................Above Previous old Comment ......................
}
function Validations(obj) {
    if (obj == "SubLedger Type Can Only Custom or None") {
        var k = $('#valid');
        //$('#valid').css({ 'display': 'block' });
        $('#valid').removeClass('hide');

        return false;
    }
    else if (obj == "Mandatory Account name") {
        //var k1 = $('#valid');
        $('#valid').removeClass('hide');
        $('.dxgvEditingErrorRow_PlasticBlue').attr('style', 'background-color:#EDF3F4 !important');
        $('#txtAccountCode').focus();
        //$('#valid').css({ 'display': 'block' });
        return;
    }
    else if (obj == "Mandatory Account Code") {
        $('#validaccode').removeClass('hide');
        $('.dxgvEditingErrorRow_PlasticBlue').attr('style', 'background-color:#EDF3F4 !important');
        //$('#txtAccountCode').focus();

    }
    else if (obj == "This AccountCode Already Exists") {
        $('#validaccode').removeClass('hide');
        $('.dxgvEditingErrorRow_PlasticBlue').attr('style', 'background-color:#EDF3F4 !important');
        //$('#txtAccountCode').focus();

    }
    else if (obj == "Bank Account Number Already Exists.") {
        $('#validbankaccno').removeClass('hide');
        $('.dxgvEditingErrorRow_PlasticBlue').attr('style', 'background-color:#EDF3F4 !important');
    }
    else if (obj == "Sub Ledger Type Can Not Be Blank") {
        $('#validsubledgtyp').removeClass('hide');
        $('.dxgvEditingErrorRow_PlasticBlue').attr('style', 'background-color:#EDF3F4 !important');
    }
    else if (obj == "SubLedger Type Can Only Custom or None") {
        $('#validsubledgtyp').removeClass('hide');
        $('.dxgvEditingErrorRow_PlasticBlue').attr('style', 'background-color:#EDF3F4 !important');
    }
}
function ShowError(obj) {
    if (!Validations(obj)) {
        return false;
    }


    if (obj != "a" && obj != "b") {
        var objVal = obj.split('~')
        AccopuntType(objVal[0]);
        BankCashType(objVal[1]);
        BankAccountType(objVal[2]);
        var code = objVal[3].toUpperCase();
        if (code == 'SYSTM') {
            // txtAccountNo.SetEnabled(false);
            txtAccountCode.SetEnabled(false);
            ASPxComboBox1.SetEnabled(false);
            ASPxComboBox2.SetEnabled(false);
            txtBankAccountNo.SetEnabled(false);
            ASPxComboBox3.SetEnabled(false);
            CmbSubLedgerType.SetEnabled(false);
            rbSegment.SetEnabled(false);
            //txtRateofIntrest.SetEnabled(false);

            FBTApplicable.SetEnabled(false);
            txtFBTRate.SetEnabled(false);
            TDSApplicable.SetEnabled(false);
            txtTDSRate.SetEnabled(false);
        }
        txtAccountCode.SetEnabled(false);
        if (objVal[4] != "" && objVal[4] != "0") {
            rbSegment.SetValue('S');
            ExchangeSegment(objVal[4]);
        }
    }

}
function updateEditorText() {
    var code = txtAccountCode.GetText().toUpperCase();
    if (code == 'SYSTM') {
        alert('You Can not Enter This Code,This is Reserve Code ');
        txtAccountCode.SetText('');
    }
}
//function GridDelete(obj1, obj2,obj3,obj4) {
//   if (obj4=='Delete')
//   {
//        if (confirm("Are You Sure You Want To Delete ?")) {
//        var obj5 = obj1 + '~' + obj2+ '~' +obj3+ '~' +obj4;
//        combo.PerformCallback(obj5);

//    }
//    else {
//        return false;
//    }

//   }
//    else if(obj4=='Edit')
//    {
//         var obj5 = obj1 + '~' + obj2+ '~' +obj3+ '~' +obj4;
//        combo.PerformCallback(obj5);
//    }

//}
function GridDelete(obj1, obj2) {
    if (confirm("Confirm delete ?")) {
        var obj3 = obj1 + '~' + obj2;
        combo.PerformCallback(obj3);

    }
    else {
        return false;
    }

}
function ShowError1(obj) {
    if (obj == "b") {
        alert('Transaction Exists for this Code. Deletion Not Allowed !!');
        return false;
    }
    else {
        var chk = document.getElementById("chkSysAccount");
        if (chk.checked == true)
            grid.PerformCallback('T');
        else
            grid.PerformCallback('F');
    }
}
//function CompanyExchange(obj) {
//  comboSegment.PerformCallback();
//}
function SegmentID1(obj) {

    document.getElementById("hdSegment").value = obj;
}
function CallTdsAccount(objid, objfunc, objevant) {
    FieldName = 'Label1';
    // alert(objid);
    ajax_showOptions(objid, objfunc, objevant);
}
function checkChange(obj) {
    if (obj == true)
        grid.PerformCallback('T');
    else
        grid.PerformCallback('F');
}
function SetSegValue(segval) {

    document.getElementById("hdSegment").value = segval;

}
function lost() {
    alert('lost focus');
}

function OnGridEndCallback(s, e) {
    if (grid.cpValidating != null) {

        jAlert(grid.cpValidating);

    }
    if (grid.cpUDF != null) {
        jAlert("UDF is set as Mandatory. Please enter values.", "Alert", function () { OpenUdf(); });

        grid.cpUDF = null;
    }
    if (grid.cpUDFKey != null) {
        document.getElementById("Keyval_internalId").value = grid.cpUDFKey;
        grid.cpUDFKey == null
    }
    //debugger;
    if (grid.cpDelete != null) {
        if (grid.cpDelete == 's') {
            jAlert('Deleted successfully');
            grid.cpDelete = null;
        }
        else if (grid.cpDelete == 'f') {
            jAlert('Used in other module.Cannot delete.')
            grid.cpDelete = null;
        }
        else if (grid.cpDelete == 'syscode') {
            jAlert('System generated code. Cannot Delete.')
            grid.cpDelete = null;
        }
    }
    else if (grid.cpValidating != null) {
        //alert(grid.cpValidating);
        if (grid.cpValidating == 'Bank Account Number already exists.') {
            txtBankAccountNo.Focus();
            txtBankAccountNo.SetText();
        }

        //jAlert(grid.cpValidating);
        grid.cpValidating = null;
    }
    else if (grid.cpUpdate != null) {
        jAlert(grid.cpUpdate);
        grid.cpUpdate = null;
    }
    else if (grid.cpinsert != null) {
        jAlert(grid.cpinsert);
        grid.cpinsert = null;
    }
    //MainAccountGrid.JSProperties["cpUpdate"] = "Saved successfully";

}

    $(document).ready(function () {
        $('#chkSysAccount + label').addClass('emph');
        $('#chkSysAccount').click(function () {
            if ($('#chkSysAccount').prop('checked')) {
                $('#chkSysAccount + label').addClass('emph');

            } else {
                $('#chkSysAccount + label').removeClass('emph');

            }
        });
        //$("#chkSysAccount").bootstrapSwitch();
        //Rev Rajdip
        BindDeducteeType();
        //END Rev Rajdip

        if (ctxtNumber.GetText() != "") {
            ctxtNameAsPerPan.SetEnabled(true);
        }
        else {
            ctxtNameAsPerPan.SetEnabled(false);
        }


    });
function ValidatePanno() {
    debugger;
    var PAN = ctxtNumber.GetText().toUpperCase();
    $("#hdnflag").val("0");
    if (PAN != "") {
        var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
        var code = /([C,P,H,F,A,T,B,L,J,G])/;
        var code_chk = PAN.substring(3, 4);
        if (PAN.search(panPat) == -1) {
            //jAlert("Please Enter Valid Pan No");
            $("#hdnflag").val("1");
            //alert('hi');
            return;
        }
        if (code.test(code_chk) == false) {
            //jAlert("Invaild PAN Card No or Use Capital Letter");
            // $("#hdnflag").val("1");
            //alert('hi');
            return;
        }
    }
}
//Rev Rajdip
function BindDeducteeType()
{
    $.ajax({
        url: "MainAccountHead.Aspx/Binddeducteetype",
        contentType: "application/json; charset=utf-8",
        datatype: "JSON",
        type: "POST",
        success: function (data) {

            $("#cmbDeducteeType").empty();
            var deducteetype = data.d;

            debugger;

            var opts = "";
            opts = "<option value='0'>Select</option>";
            for (i in deducteetype)
                      
                opts += "<option value='" + deducteetype[i].ID + "'>" + deducteetype[i].Type_Name + "</option>";

            $("#cmbDeducteeType").empty().append(opts);
                  
        },
        error: function (data) {
            debugger;
            //jAlert("Please try again later");
        }
    });
}
//END Rev Rajdip

function AddButtonClick() {
    //grid.AddNewRow();
    //document.getElementById("Keyval_internalId").value = "Add";
    location.href = "MainAccountAddEdit.aspx?id=ADD";
}

function VerifyButtonClick() {
    var url = '/OMS/management/Master/ApprovalPopup.aspx?ModuleName=AccountHead';
    cVerifyPopup.SetContentUrl(url);
    cVerifyPopup.Show();
}


    function OpenMappingLedgerPopup(s, e) {
        var keyValue = grid.GetRowKey(e.visibleIndex);
        $("#hfLedgerID").val(keyValue);
        GetMappedHSNSCA(keyValue);
        cMappingLedgerPopup.Show();
    }

function CloseMappingPopup() {
    cMappingLedgerPopup.Hide();
}

function GetMappedHSNSCA(keyValue) {
    $.ajax({
        type: "POST",
        url: "MainAccountHead.aspx/GetMappedHSNSCAData",
        data: JSON.stringify({ LedgerID: $("#hfLedgerID").val() }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            debugger;
            data = msg.d;
            if (data.length > 0) {
                if (data[1] == "HSN") {
                    cHsnLookUp.SetText(data[3]);
                    cHsnLookUp.SetValue(data[3]);
                }
                else if (data[1] == "SAC") {
                    cScaLookUp.SetText(data[3]);
                    cScaLookUp.SetValue(data[3]);
                }
                cchbFurtherenceOfBusiness.SetValue(data[2]);
                      
                if (data[4] != null && data[4] != '') {
                    var gstin = data[4];
                    if (gstin.length > 0) {
                        ctxtGSTIN111.SetText(gstin.substring(0, 2));
                        ctxtGSTIN222.SetText(gstin.substring(2, 12));
                        ctxtGSTIN333.SetText(gstin.substring(12, 15));
                    }
                }
                else {
                    ctxtGSTIN111.SetText('');
                    ctxtGSTIN222.SetText('');
                    ctxtGSTIN333.SetText('');
                }
                //REV RAJDIP FOR PAN & DEDUCTEE TYPE
                if (data[5] != null && data[5] != '') {
                    var Panno = data[5];
                    ctxtNumber.SetText(Panno);
                }
                else {
                    ctxtNumber.SetText('');
                } //
                if (data[6] != null && data[6] != '') {
                    var deducteetype = data[6];
                    $("#cmbDeducteeType").val(deducteetype);
                }
                else {
                    $("#cmbDeducteeType").val('0');
                }
                //END REV RAJDIP 
                if (data[7] != null && data[7] != '') {
                    var NamePan = data[7];
                    ctxtNameAsPerPan.SetText(NamePan);
                }
                else {
                    ctxtNameAsPerPan.SetText('');
                }
            }
            else {
                cScaLookUp.SetValue('');
                cScaLookUp.SetText('');
                cHsnLookUp.SetValue('');
                cHsnLookUp.SetText('');
                cchbFurtherenceOfBusiness.SetValue(false);
                ctxtGSTIN111.SetText('');
                ctxtGSTIN222.SetText('');
                ctxtGSTIN333.SetText('');
                //Rev Rajdip
                ctxtNumber.SetText('');
                $("#cmbDeducteeType").val('0');
                //End Rev Rajdip
                ctxtNameAsPerPan.SetText('');
            }




        }
    });
}

function HsnLookUp_SelectedChange(e) {
    var hsnkey = cHsnLookUp.GetGridView().GetRowKey(cHsnLookUp.GetGridView().GetFocusedRowIndex());
    $('#hfHSNSCAkey').val(hsnkey);
    $('#hfHSNSCAType').val('HSN');
    if (hsnkey != null && hsnkey != '') {
        cScaLookUp.SetValue('');
        cScaLookUp.SetText('');
    }
}

function ScaLookUp_SelectedChange(e) {
    var scakey = cScaLookUp.GetGridView().GetRowKey(cScaLookUp.GetGridView().GetFocusedRowIndex());
    $('#hfHSNSCAkey').val(scakey);
    $('#hfHSNSCAType').val('SAC');
    if (scakey != null && scakey != '') {
        cHsnLookUp.SetValue('');
        cHsnLookUp.SetText('');
    }
}

function MappingLedgerSaveClick() {
    var flag = true;
    debugger;
    //Rev Rajdip
    var PAN = ctxtNumber.GetText().toUpperCase();
    $("#hdnflag").val("0");
    if (PAN != "") {
        var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
        var code = /([C,P,H,F,A,T,B,L,J,G])/;
        var code_chk = PAN.substring(3, 4);
        if (PAN.search(panPat) == -1) {
            jAlert("Please enter valid PAN.");
            $("#hdnflag").val("1");
            //alert('hi');
            return;
        }
        if (code.test(code_chk) == false) {
            jAlert("Please enter valid PAN.");
            $("#hdnflag").val("1");
            return;
        }
    }
    //var flag= $("#hdnflag").val();
    //if (flag == "1")
    //{
    //    jAlert("Please Insert A Valid Pan");
    //}
    //End Rev Rajdip
    $('#invalidGst').css({ 'display': 'none' });
    var gst1 = ctxtGSTIN111.GetText().trim();
    var gst2 = ctxtGSTIN222.GetText().trim();
    var gst3 = ctxtGSTIN333.GetText().trim();
    //Rev Rajdip 
    var PAN = ctxtNumber.GetText().toUpperCase();
    var Deductee_Type = $('#cmbDeducteeType').val();

    //End Rev Rajdip
    var NameAsPerPan = ctxtNameAsPerPan.GetText();
    if (gst1.length == 0 && gst2.length == 0 && gst3.length == 0) {
       // <%-- var isregistered = $('#<%=radioregistercheck.ClientID %> input:checked').val();
       // if (isregistered == 1) {
        //    jAlert('GSTIN is mandatory.');
         //   retValue = false;--%>
        //}
        flag = true;
        }
        else {
            if (gst1.length != 2 || gst2.length != 10 || gst3.length != 3) {
                $('#invalidGst').css({ 'display': 'block' });
                flag = false;
            }
            var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
            var code = /([C,P,H,F,A,T,B,L,J,G])/;
            var code_chk = gst2.substring(3, 4);
            if (gst2.search(panPat) == -1) {
                $('#invalidGst').css({ 'display': 'block' });
                flag = false;
            }
            if (code.test(code_chk) == false) {
                $('#invalidGst').css({ 'display': 'block' });
                flag = false;
            }

        }
        if ($('#hfHSNSCAkey').val() == '' && cchbFurtherenceOfBusiness.GetChecked() == false && gst1 == '') {
            //Rev Rajdip as Per Instruction to block mandatory HSN
            //Commented By Rajdip as Per Instruction to block mandatory HSN
            //jAlert('Values not entered. Can not saved.')
            //return;
            //flag == false;
            //ENd Comment
            flag == true;
            //End Rev rajdip
        }
        if (flag == true) {
            var GSTIN = gst1 + gst2 + gst3;
            $.ajax({
                type: "POST",
                url: "MainAccountHead.aspx/MapLedgerToHSNSCA",
                //data: JSON.stringify({ LedgerID: $("#hfLedgerID").val() }),
                //Rev Rajdip
                //data: JSON.stringify({ LedgerID: $("#hfLedgerID").val(), HSNSCACode: $('#hfHSNSCAkey').val(), HSNSCAType: $('#hfHSNSCAType').val(), FOBFlag: cchbFurtherenceOfBusiness.GetChecked(), GSTIN: GSTIN }),                   
                data: JSON.stringify({ LedgerID: $("#hfLedgerID").val(), HSNSCACode: $('#hfHSNSCAkey').val(), HSNSCAType: $('#hfHSNSCAType').val(), FOBFlag: cchbFurtherenceOfBusiness.GetChecked(), GSTIN: GSTIN, PAN: PAN, Deductee_Type: Deductee_Type, NameAsPerPan: NameAsPerPan }),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    CheckFlag = msg.d;
                    if (CheckFlag == true) {
                        CloseMappingPopup();
                        ctxtNumber.SetText("");
                        ctxtNameAsPerPan.SetText("");
                        $('#cmbDeducteeType').val("0");
                        jAlert("Saved Successfully.", "Alert");
                        //jAlert('Please enter unique short name');
                    }
                    //Rev 1.0
                    else {
                        jAlert("Account Head already used in transaction.The mapping/unmapping of the HSN/SAC code is not allowed.", "Alert");
                    }
                    //Rev 1.0 end
                }
            });
        }
    }
    function gridRowclick(s, e) {
        $('#MainAccountGrid').find('tr').removeClass('rowActive');
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
 