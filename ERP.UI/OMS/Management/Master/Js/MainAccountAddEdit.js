$(document).ready(function () {

            if ((cdrp_asset_type.GetValue() == "Asset") || (cdrp_asset_type.GetValue() == "Liability"))
            {
                
                $("#dvIsparty").css("display", "block");
            }
            if(cdrp_sub_ledger_type.GetValue()=="None")
            {
                cIsparty.SetEnabled(false);
            }

            if(cdrp_asset_type.GetValue() == "Liability")
            {
                if (cdrpassettype.GetItemCount()==4) {
                    cdrpassettype.RemoveItem(3);
                    cdrpassettype.SetValue('Bank');
                }
            }
            else
            {
                if (cdrpassettype.GetItemCount() == 3) {
                    cdrpassettype.AddItem("Fixed Asset", "Fixed Asset");

                }
            }


        });

function UdfPopupClick(s, e) {

    var keyVal = document.getElementById('Keyval_internalId').value;
    var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=AH&&KeyVal_InternalID=' + keyVal;
    popup.SetContentUrl(url);
    popup.Show();
    e.processOnServer = false;
    return true;

}

var canCallBack = true;
function AllControlInitilize() {
    if (canCallBack) {

        if (cdrp_asset_type.GetValue() != "Asset" && cdrp_asset_type.GetValue() != "Liability") {
            $("#asset_type").hide();
            $("#sub_ledger").show();

            //chinmoy added for Isparty
            if ($("#hdMainActId").val() == "")
                cdrp_sub_ledger_type.SetSelectedIndex(0);
            if (cdrp_asset_type.GetValue() == "Asset" || cdrp_asset_type.GetValue() == "Liability") {
                if (cdrp_sub_ledger_type.GetValue() == "None") {
                    cIsparty.SetEnabled(false);
                    cIsparty.SetValue(0);
                }
                else {
                    cIsparty.SetEnabled(true);
                    // cIsparty.SetValue(1);
                    //cIsparty.SetValue(0);
                }
            }
            //End
            $("#bnk_acnt").hide();
            $("#tds").show();
            $("#div_NegStock").hide();
            $("#div_BalLimit").hide();


            ctxtBalanceLimit.SetValue("0.00");
            ccmbNegativeStk.SetValue("");

                    


            $("#div_DailyLimit").show();
            $("#div_DailyLimitExceed").show();

        }
        else if (cdrp_asset_type.GetValue() == "Asset") {
            $("#sub_ledger").hide();


            if ($("#hdMainActId").val() == "")
                cdrp_sub_ledger_type.SetSelectedIndex(0);
            //chinmoy added for Isparty
            if (cdrp_asset_type.GetValue() == "Asset" || cdrp_asset_type.GetValue() == "Liability") {
                if (cdrp_sub_ledger_type.GetValue() == "None") {
                    cIsparty.SetEnabled(false);
                    cIsparty.SetValue(0);
                }
                else {
                    cIsparty.SetEnabled(true);
                    // cIsparty.SetValue(1);
                    //cIsparty.SetValue(0);
                }
            }
            //End


            $("#bnk_acnt").show();
            $("#tds").hide();
            $("#asset_type").show();
            $("#AssLiaType").text('Asset Type');
        }
        else if (cdrp_asset_type.GetValue() == "Liability")
        {
            $("#asset_type").show();
            $("#bnk_acnt").show();
            $("#AssLiaType").text('Liability Type');

        }

        if (cdrp_asset_type.GetValue() == "Asset"||cdrp_asset_type.GetValue() == "Liability") {
            if (cdrpassettype.GetValue() == "Bank") {
                //$("#roi").show();
                $("#bnk_acnt").show();
                $("#tds").hide();

                if ($("#hdnSubledgerCashBankType").val() == "1") {
                    $("#sub_ledger").show();
                }
                else {
                    $("#sub_ledger").hide();
                }

                //$("#sub_ledger").hide();
                if ($("#hdMainActId").val()=="")
                    cdrp_sub_ledger_type.SetSelectedIndex(0);
                //chinmoy added for Isparty
                if (cdrp_asset_type.GetValue() == "Asset" || cdrp_asset_type.GetValue() == "Liability") {
                    if (cdrp_sub_ledger_type.GetValue() == "None") {
                        cIsparty.SetEnabled(false);
                        cIsparty.SetValue(0);
                    }
                    else {
                        cIsparty.SetEnabled(true);
                        // cIsparty.SetValue(1);
                        //cIsparty.SetValue(0);
                    }
                }
                //End


                $("#depr").css("display", "none");
                $("#div_NegStock").show();
                $("#div_BalLimit").show();

                $("#div_DailyLimit").hide();
                $("#div_DailyLimitExceed").hide();
                        
                $("#div_DailyLimit").val("0.00");
                $("#div_DailyLimitExceed").val("S");
                        
            }
            else if (cdrpassettype.GetValue() == "Cash") {

                if ($("#hdnSubledgerCashBankType").val() == "1") {
                    $("#sub_ledger").show();
                }
                else {
                    $("#sub_ledger").hide();
                }

                //$("#sub_ledger").hide();
                if ($("#hdMainActId").val() == "")
                    cdrp_sub_ledger_type.SetSelectedIndex(0);
                //chinmoy added for Isparty
                if (cdrp_asset_type.GetValue() == "Asset" || cdrp_asset_type.GetValue() == "Liability") {
                    if (cdrp_sub_ledger_type.GetValue() == "None") {
                        cIsparty.SetEnabled(false);
                        cIsparty.SetValue(0);
                    }
                    else {
                        cIsparty.SetEnabled(true);
                        // cIsparty.SetValue(1);
                        //cIsparty.SetValue(0);
                    }
                }
                //End


                $("#bnk_acnt").hide();
                // $("#roi").hide();
                $("#tds").hide();
                $("#depr").css("display", "none");
                $("#div_NegStock").show();
                $("#div_BalLimit").show();
                $("#div_DailyLimit").hide();
                $("#div_DailyLimitExceed").hide();

                ctxtDailyLimit.SetValue("0.00");
                ccmbDailyLimitExceed.SetValue("");

            }
            else if (cdrpassettype.GetValue() == "Fixed Asset") {
                $("#sub_ledger").show();
                if ($("#hdMainActId").val() == "")
                    cdrp_sub_ledger_type.SetSelectedIndex(0);
                //chinmoy added for Isparty
                if (cdrp_asset_type.GetValue() == "Asset" || cdrp_asset_type.GetValue() == "Liability") {
                    if (cdrp_sub_ledger_type.GetValue() == "None") {
                        cIsparty.SetEnabled(false);
                        cIsparty.SetValue(0);
                    }
                    else {
                        cIsparty.SetEnabled(true);
                        // cIsparty.SetValue(1);
                        //cIsparty.SetValue(0);
                    }
                }
                //End


                $("#bnk_acnt").hide();
                // $("#roi").hide();
                $("#tds").hide();
                $("#depr").css("display", "block");
                $("#div_NegStock").hide();
                $("#div_BalLimit").hide();

                ctxtBalanceLimit.SetValue("0.00");
                ccmbNegativeStk.SetValue("");

                $("#div_DailyLimit").show();
                $("#div_DailyLimitExceed").show();
            }
            else {
                $("#bnk_acnt").hide();
                // $("#roi").show();
                $("#depr").css("display", "none");
                $("#sub_ledger").show();
                if ($("#hdMainActId").val() == "")
                    cdrp_sub_ledger_type.SetSelectedIndex(0);
                //chinmoy added for Isparty
                if (cdrp_asset_type.GetValue() == "Asset" || cdrp_asset_type.GetValue() == "Liability") {
                    if (cdrp_sub_ledger_type.GetValue() == "None") {
                        cIsparty.SetEnabled(false);
                        cIsparty.SetValue(0);
                    }
                    else {
                        cIsparty.SetEnabled(true);
                        // cIsparty.SetValue(1);
                        //cIsparty.SetValue(0);
                    }
                }
                //End


                $("#tds").show();
                $("#div_NegStock").hide();
                $("#div_BalLimit").hide();

                ctxtBalanceLimit.SetValue("0.00");
                ccmbNegativeStk.SetValue("");

                $("#div_DailyLimit").show();
                $("#div_DailyLimitExceed").show();
            }

        }

        if (cdrp_asset_type.GetValue() == "Liability") {
            if (cdrpassettype.GetItemCount() == 4) {
                cdrpassettype.RemoveItem(3);
                cdrpassettype.SetValue('Bank');
            }
        }
        else {
            if (cdrpassettype.GetItemCount() == 3) {
                cdrpassettype.AddItem("Fixed Asset", "Fixed Asset");
                        
            }
        }



        canCallBack = false;
    }

}
function getaccounttype() {
    //debugger;
    cdrp_acnt_grp.PerformCallback();
    if (cdrp_asset_type.GetValue() != "Asset") {
        $("#asset_type").hide();
        $("#sub_ledger").show();
        if ($("#hdMainActId").val() == "")
            cdrp_sub_ledger_type.SetSelectedIndex(0);
        //chinmoy added for Isparty
        if (cdrp_asset_type.GetValue() == "Asset" || cdrp_asset_type.GetValue() == "Liability") {
            if (cdrp_sub_ledger_type.GetValue() == "None") {
                cIsparty.SetEnabled(false);
                cIsparty.SetValue(0);
            }
            else {
                cIsparty.SetEnabled(true);
                // cIsparty.SetValue(1);
                //cIsparty.SetValue(0);
            }
        }
        //End



        $("#bnk_acnt").hide();
        $("#tds").show();
        $("#depr").css("display", "none");
                
        $("#div_NegStock").hide();
        $("#div_BalLimit").hide();

        ctxtBalanceLimit.SetValue("0.00");
        ccmbNegativeStk.SetValue("");


        $("#div_DailyLimit").show();
        $("#div_DailyLimitExceed").show();

        if (cdrp_asset_type.GetValue() == "Liability") {
            $("#asset_type").show();
            $("#bnk_acnt").show();
            $("#AssLiaType").text('Liability Type');

            //chinmoy added for liability start

            if (cdrpassettype.GetValue() == "Cash") {

                if ($("#hdnSubledgerCashBankType").val() == "1") {
                    $("#sub_ledger").show();
                }
                else {
                    $("#sub_ledger").hide();
                }

                //$("#sub_ledger").hide();
                if ($("#hdMainActId").val() == "")
                    cdrp_sub_ledger_type.SetSelectedIndex(0);
                //chinmoy added for Isparty
                if (cdrp_asset_type.GetValue() == "Asset" || cdrp_asset_type.GetValue() == "Liability") {
                    if (cdrp_sub_ledger_type.GetValue() == "None") {
                        cIsparty.SetEnabled(false);
                        cIsparty.SetValue(0);
                    }
                    else {
                        cIsparty.SetEnabled(true);
                        // cIsparty.SetValue(1);
                        //cIsparty.SetValue(0);
                    }
                }
                //End



                $("#tds").hide();
                $("#depr").css("display", "none");
                $("#bnk_acnt").hide();
                $("#div_NegStock").show();
                $("#div_BalLimit").show();

                $("#div_DailyLimit").hide();
                $("#div_DailyLimitExceed").hide();


                ctxtDailyLimit.SetValue("0.00");
                ccmbDailyLimitExceed.SetValue("");
            }
            else if (cdrpassettype.GetValue() == "Bank") {

                if ($("#hdnSubledgerCashBankType").val() == "1") {
                    $("#sub_ledger").show();
                }
                else {
                    $("#sub_ledger").hide();
                }

                //$("#sub_ledger").hide();
                if ($("#hdMainActId").val() == "")
                    cdrp_sub_ledger_type.SetSelectedIndex(0);
                //chinmoy added for Isparty
                if (cdrp_asset_type.GetValue() == "Asset" || cdrp_asset_type.GetValue() == "Liability") {
                    if (cdrp_sub_ledger_type.GetValue() == "None") {
                        cIsparty.SetEnabled(false);
                        cIsparty.SetValue(0);
                    }
                    else {
                        cIsparty.SetEnabled(true);
                        // cIsparty.SetValue(1);
                        //cIsparty.SetValue(0);
                    }
                }
                //End


                $("#tds").hide();
                $("#depr").css("display", "none");
                $("#bnk_acnt").show();
                $("#div_NegStock").show();
                $("#div_BalLimit").show();

                $("#div_DailyLimit").hide();
                $("#div_DailyLimitExceed").hide();

                ctxtDailyLimit.SetValue("0.00");
                ccmbDailyLimitExceed.SetValue("");
            }
            else if (cdrpassettype.GetValue() == "Fixed Asset") {
                $("#depr").css("display", "block");
                $("#tds").hide();
                $("#div_NegStock").hide();
                $("#div_BalLimit").hide();

                ctxtBalanceLimit.SetValue("0.00");
                ccmbNegativeStk.SetValue("");

                $("#div_DailyLimit").show();
                $("#div_DailyLimitExceed").show();
            }



            //End
        }
    }
    else if (cdrp_asset_type.GetValue() == "Asset"||(cdrp_asset_type.GetValue() == "Liability")) {
        $("#asset_type").show();
        $("#AssLiaType").text('Asset Type');
        if (cdrpassettype.GetValue() == "Cash") {
            $("#sub_ledger").hide();
            if ($("#hdMainActId").val() == "")
                cdrp_sub_ledger_type.SetSelectedIndex(0);
            //chinmoy added for Isparty
            if (cdrp_asset_type.GetValue() == "Asset" || cdrp_asset_type.GetValue() == "Liability") {
                if (cdrp_sub_ledger_type.GetValue() == "None") {
                    cIsparty.SetEnabled(false);
                    cIsparty.SetValue(0);
                }
                else {
                    cIsparty.SetEnabled(true);
                    // cIsparty.SetValue(1);
                    //cIsparty.SetValue(0);
                }
            }
            //End



            $("#tds").hide();
            $("#depr").css("display", "none");
            $("#bnk_acnt").hide();
            $("#div_NegStock").show();
            $("#div_BalLimit").show();

            $("#div_DailyLimit").hide();
            $("#div_DailyLimitExceed").hide();


            ctxtDailyLimit.SetValue("0.00");
            ccmbDailyLimitExceed.SetValue("");
        }
        else if (cdrpassettype.GetValue() == "Bank") {
            $("#sub_ledger").hide();
            if ($("#hdMainActId").val() == "")
                cdrp_sub_ledger_type.SetSelectedIndex(0);
            //chinmoy added for Isparty
            if (cdrp_asset_type.GetValue() == "Asset" || cdrp_asset_type.GetValue() == "Liability") {
                if (cdrp_sub_ledger_type.GetValue() == "None") {
                    cIsparty.SetEnabled(false);
                    cIsparty.SetValue(0);
                }
                else {
                    cIsparty.SetEnabled(true);
                    // cIsparty.SetValue(1);
                    //cIsparty.SetValue(0);
                }
            }
            //End


            $("#tds").hide();
            $("#depr").css("display", "none");
            $("#bnk_acnt").show();
            $("#div_NegStock").show();
            $("#div_BalLimit").show();

            $("#div_DailyLimit").hide();
            $("#div_DailyLimitExceed").hide();

            ctxtDailyLimit.SetValue("0.00");
            ccmbDailyLimitExceed.SetValue("");
        }
        else if (cdrpassettype.GetValue() == "Fixed Asset") {
            $("#depr").css("display", "block");
            $("#tds").hide();
            $("#div_NegStock").hide();
            $("#div_BalLimit").hide();

            ctxtBalanceLimit.SetValue("0.00");
            ccmbNegativeStk.SetValue("");

            $("#div_DailyLimit").show();
            $("#div_DailyLimitExceed").show();
        }

        //$("#bnk_acnt").show();
        //$("#tds").hide();

    }


           
    SelectPostingType();
    //getasset_type();

    if ((cdrp_asset_type.GetValue() == "Asset") || (cdrp_asset_type.GetValue() == "Liability")) {

        //cIsparty.SetValue(0);
        $("#dvIsparty").css("display", "block");
    }
    else
    {
        $("#dvIsparty").css("display", "none");
        cIsparty.SetValue(0);
    }


    if (cdrp_asset_type.GetValue() == "Liability") {
        if (cdrpassettype.GetItemCount() == 4) {
            cdrpassettype.RemoveItem(3);
            cdrpassettype.SetValue('Bank');
        }
    }
    else {
        if (cdrpassettype.GetItemCount() == 3) {
            cdrpassettype.AddItem("Fixed Asset", "Fixed Asset");
                    
        }
    }


}

function get_SubLedger_type()
{
    if ((cdrp_asset_type.GetValue() == "Asset") || (cdrp_asset_type.GetValue() == "Liability")) {
        if (cdrp_sub_ledger_type.GetValue() == "None") {
            cIsparty.SetEnabled(false);
            cIsparty.SetValue(0);
        }
        else {
            cIsparty.SetEnabled(true);
            // cIsparty.SetValue(1);
            //cIsparty.SetValue(0);
        }
    }
}
             

function getasset_type() {
    if (cdrp_asset_type.GetValue() == "Asset"|| cdrp_asset_type.GetValue() == "Liability") {
        if (cdrpassettype.GetValue() == "Bank") {
            $("#DivSetAsDefault").show();
            $("#bnk_acnt").show();
            $("#tds").hide();
            if ($("#hdnSubledgerCashBankType").val() == "1") {
                $("#sub_ledger").show();
            }
            else {
                $("#sub_ledger").hide();
            }
            if ($("#hdMainActId").val() == "")
                cdrp_sub_ledger_type.SetSelectedIndex(0);
            //chinmoy added for Isparty
            if (cdrp_asset_type.GetValue() == "Asset" || cdrp_asset_type.GetValue() == "Liability") {
                if (cdrp_sub_ledger_type.GetValue() == "None") {
                    cIsparty.SetEnabled(false);
                    cIsparty.SetValue(0);
                }
                else {
                    cIsparty.SetEnabled(true);
                    // cIsparty.SetValue(1);
                    //cIsparty.SetValue(0);
                }
            }
            //End



            $("#depr").css("display", "none");

            $("#div_NegStock").show();
            $("#div_BalLimit").show();

            $("#div_DailyLimit").hide();
            $("#div_DailyLimitExceed").hide();

            ctxtDailyLimit.SetValue("0.00");
            ccmbDailyLimitExceed.SetValue("");


        }
        else if (cdrpassettype.GetValue() == "Cash") {

            if ($("#hdnSubledgerCashBankType").val() == "1") {
                $("#sub_ledger").show();
            }
            else {
                $("#sub_ledger").hide();
            }
            $("#DivSetAsDefault").hide();
            //$("#sub_ledger").hide();
            if ($("#hdMainActId").val() == "")
                cdrp_sub_ledger_type.SetSelectedIndex(0);
            //chinmoy added for Isparty
            if (cdrp_asset_type.GetValue() == "Asset" || cdrp_asset_type.GetValue() == "Liability") {
                if (cdrp_sub_ledger_type.GetValue() == "None") {
                    cIsparty.SetEnabled(false);
                    cIsparty.SetValue(0);
                }
                else {
                    cIsparty.SetEnabled(true);
                    // cIsparty.SetValue(1);
                    //cIsparty.SetValue(0);
                }
            }
            //End


            $("#bnk_acnt").hide();
            // $("#roi").hide();
            $("#tds").hide();
            $("#depr").css("display", "none");

            $("#div_NegStock").show();
            $("#div_BalLimit").show();

            $("#div_DailyLimit").hide();
            $("#div_DailyLimitExceed").hide();

            ctxtDailyLimit.SetValue("0.00");
            ccmbDailyLimitExceed.SetValue("");
        }
        else if (cdrpassettype.GetValue() == "Fixed Asset") {
            $("#DivSetAsDefault").hide();
            $("#sub_ledger").show();
            if ($("#hdMainActId").val() == "")
                cdrp_sub_ledger_type.SetSelectedIndex(0);
            //chinmoy added for Isparty
            if (cdrp_asset_type.GetValue() == "Asset" || cdrp_asset_type.GetValue() == "Liability") {
                if (cdrp_sub_ledger_type.GetValue() == "None") {
                    cIsparty.SetEnabled(false);
                    cIsparty.SetValue(0);
                }
                else {
                    cIsparty.SetEnabled(true);
                    // cIsparty.SetValue(1);
                    //cIsparty.SetValue(0);
                }
            }
            //End



            $("#bnk_acnt").hide();
            // $("#roi").hide();
            $("#tds").hide();
            $("#depr").css("display", "block");

            $("#div_NegStock").hide();
            $("#div_BalLimit").hide();

            ctxtBalanceLimit.SetValue("0.00");
            ccmbNegativeStk.SetValue("");

            $("#div_DailyLimit").show();
            $("#div_DailyLimitExceed").show();
        }
        else {
            $("#DivSetAsDefault").hide();
            $("#bnk_acnt").hide();
            // $("#roi").show();
            $("#depr").css("display", "none");
            $("#sub_ledger").show();
            if ($("#hdMainActId").val() == "")
                cdrp_sub_ledger_type.SetSelectedIndex(0);
            //chinmoy added for Isparty
            if (cdrp_asset_type.GetValue() == "Asset" || cdrp_asset_type.GetValue() == "Liability") {
                if (cdrp_sub_ledger_type.GetValue() == "None") {
                    cIsparty.SetEnabled(false);
                    cIsparty.SetValue(0);
                }
                else {
                    cIsparty.SetEnabled(true);
                    // cIsparty.SetValue(1);
                    //cIsparty.SetValue(0);
                }
            }
            //End

            $("#tds").show();

            ctxtBalanceLimit.SetValue("0.00");
            ccmbNegativeStk.SetValue("");

            $("#div_NegStock").hide();
            $("#div_BalLimit").hide();


            $("#div_DailyLimit").show();
            $("#div_DailyLimitExceed").show();
        }
    }

    SelectPostingType();
}

function drp_acnt_grpendcallback() {
    if (cdrp_acnt_grp.cperrormsg && cdrp_acnt_grp.cperrormsg != "") {
        jAlert(cdrp_acnt_grp.cperrormsg);
        cdrp_acnt_grp.cperrormsg = null;
    }
}

function PartyOnFocus()
{

}


function selectAll() {
    cBranchGridLookup.gridView.SelectRows();
}
function unselectAll() {
    cBranchGridLookup.gridView.UnselectRows();
}
function CloseGridLookup() {
    cBranchGridLookup.ConfirmCurrentSelection();
    cBranchGridLookup.HideDropDown();
}
function SelectPostingType() {
    if (cdrp_asset_type.GetValue() == "Asset" && cdrpassettype.GetValue() == "Bank") {
        cPaymenttype.ClearItems();
        cPaymenttype.AddItem("None", "None");
        cPaymenttype.AddItem("Card", "Card");
        cPaymenttype.AddItem("Coupon", "Coupon");
        cPaymenttype.AddItem("Etransfer", "Etransfer");


    }


    else {
        cPaymenttype.ClearItems();
        cPaymenttype.AddItem("None", "None");
        cPaymenttype.AddItem("Ledger for Interstate Stk-Out", "LedgOut");
        cPaymenttype.AddItem("Ledger for Interstate Stk-In", "LedgIn");
        cPaymenttype.AddItem("Finance Processing Fee", "PrcFee");
        cPaymenttype.AddItem("Finance Other Charges Emi", "EmiCharge");
        cPaymenttype.AddItem("Goods in Transit", "TrnstGoods");

    }
    cPaymenttype.SetValue("None");
}




function submitvalidate(s, e) {
    var x = true;
    var acnt_name = ctxt_acnt_nm.GetText();
    var short_nm = ctxt_short_nm.GetText();
    var acnt_type = cdrp_asset_type.GetValue();
    var asset_type = cdrpassettype.GetValue();
    var cmp_name = cdrp_cmp_nm.GetValue();
    var branch = cBranchGridLookup.GetText();
    if (acnt_name == "") {
        $("#acnt_nm").show();
        x = false;
    }
    else {
        $("#acnt_nm").hide();
    }
    if (short_nm == "") {
        $("#short_nm").show();
        x = false;
    }
    else {
        $("#short_nm").hide();
    }
    if (acnt_type == "" || acnt_type == null) {
        $("#spn_acnt_type").show();
        x = false;
    }
    else {
        $("#spn_acnt_type").hide();
    }
    if (acnt_type == "Asset") {
        if (asset_type == "" || asset_type == null) {
            $("#spn_asset_type").show();
            x = false;
        }
        else {
            $("#spn_asset_type").hide();
        }
    }

    if (cmp_name == "" || cmp_name == null) {
        $("#cmp_nm").show();
        x = false;
    }
    else {
        $("#cmp_nm").hide();
    }

    if (branch == "") {
        $("#branch").show();
        x = false;
    }
    else {
        $("#branch").hide();
    }



    e.processOnServer = x;
}

function cancel_click() {
    window.location.href = 'MainAccountHead.aspx';
}
