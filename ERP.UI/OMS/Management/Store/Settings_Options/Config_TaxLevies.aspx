<%@ Page Title="Tax Scheme" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false"
    Inherits="ERP.OMS.Management.Store.Settings_Options.Management_Accounts_Master_Config_TaxLevies" CodeBehind="Config_TaxLevies.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <script type="text/javascript" src="/assests/js/init.js"></script>
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <%--<script type="text/javascript" src="/assests/js/ajaxList_inner.js"></script>--%>

    <%--<script type="text/javascript" src="/assests/js/jquery-1.3.2.js"></script>--%>

    <script language="javascript" type="text/javascript">
        function SignOff() {
            window.parent.SignOff();
        }
        function height() {
            if (document.body.scrollHeight >= 500)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '500px';
            window.frameElement.Width = document.body.scrollWidth;
        }
    </script>

    <script type="text/javascript">
        //function is called on changing country
        //        function OnCountryChanged(cmbCountry) 
        //        {
        //            grid.GetEditor("cou_country").PerformCallback(cmbCountry.GetValue().toString());
        //        }


        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
    </script>

    <script type="text/javascript">
        function visibleValidation() {
            $("#MandatoryTaxCode").hide();
            $("#MandatoryState").hide();

            $("#MandatoryCountry").hide()

            $("#MandatoryCity").hide();
            $("#MandatoryDateFrom").hide();
            $("#MandatoryRateOrSlab").hide();
            $("#MandatoryRate").hide();
            $("#MandatorySlabCode").hide();
            $("#MandatorySurchargeApplicable").hide();
            $("#MandatorySurchargeCriteria").hide();
            $("#MandatorySurchargeAbove").hide();
            $("#MandatorySurchargeOn").hide();
            $("#MandatorySurchargeRate").hide();
            $("#MandatorylstTaxRates_MainAccount").hide();
            $("#Mandatorytxt_schemename").hide();
        }
        function fn_PopOpen() {
            //Debjyoti Now Add new redirect to new page
            window.location.href = '/OMS/management/store/Settings_Options/TaxSchemeAddEdit.aspx?id=ADD'
            //Bellow line commented by debjyoti 


            //visibleValidation();
            //document.getElementById('hiddenedit').value = "";

            //var val = cCmbTaxRates_SurchargeApplicable.GetValue();
            //if (val == 'Y') {

            //    document.getElementById('divTaxRates_SurchargeCriteria').style.display = 'block';
            //    document.getElementById('divTaxRates_SurchargeAbove').style.display = 'block';
            //    document.getElementById('divtxtTaxes_SurchargeRate').style.display = 'block';
            //    document.getElementById('divCmbTaxRates_SurchargeOn').style.display = 'block';
            //}
            //else {
            //    document.getElementById('divtxtTaxes_SurchargeRate').style.display = 'none';
            //    document.getElementById('divCmbTaxRates_SurchargeOn').style.display = 'none';
            //    document.getElementById('divTaxRates_SurchargeCriteria').style.display = 'none';
            //    document.getElementById('divTaxRates_SurchargeAbove').style.display = 'none';
            //}

            //var val1 = cCmbTaxRates_RateOrSlab.GetValue();

            //if (val1 == 'R') {
            //    ctxtTaxRates_Rate.SetText('');
            //    ctxtTaxRates_MinAmount.SetText('');
            //    //ctxtTaxRates_SlabCode.SetText('');
            //    cCmbTaxSlab_Code.SetSelectedIndex(0);

            //    document.getElementById('divtxtTaxRates_Rate').style.display = 'block';
            //    document.getElementById('divtxtTaxRates_MinAmount').style.display = 'block';
            //    document.getElementById('divtxtTaxRates_SlabCode').style.display = 'none';

            //}
            //else {
            //    ctxtTaxRates_Rate.SetText('');
            //    ctxtTaxRates_MinAmount.SetText('');
            //    //ctxtTaxRates_SlabCode.SetText('');
            //    cCmbTaxSlab_Code.SetSelectedIndex(0);

            //    document.getElementById('divtxtTaxRates_Rate').style.display = 'none';
            //    document.getElementById('divtxtTaxRates_MinAmount').style.display = 'none';
            //    document.getElementById('divtxtTaxRates_SlabCode').style.display = 'block';
            //}

            //cCmbTaxRates_TaxCode.Focus();
            //cCmbCountryName.SetSelectedIndex(0);
            //cCmbTaxSlab_Code.SetSelectedIndex(0);
            //cCmbState.SetSelectedIndex(0);
            //cCmbCity.SetSelectedIndex(0);
            //cCmbTaxRates_TaxCode.SetSelectedIndex(0);
            //cCmbTaxRates_TaxCode.SetSelectedIndex(0);
            //cCmbTaxRates_ProductClass.SetSelectedIndex(0);
            //ctxt_schemename.SetText('');

            //var date = new Date();
            //ctxtTaxRates_DateFrom.SetDate(date);
            ////ctxtTaxRates_DateFrom.SetText('');
            ////ctxtTaxRates_DateFrom.SetDate(null);

            //cCmbTaxRates_RateOrSlab.SetSelectedIndex(0);
            //ctxtTaxRates_Rate.SetText('');
            //ctxtTaxRates_MinAmount.SetText('');
            //////ctxtTaxRates_SlabCode.SetText('');
            //cCmbTaxRates_SurchargeApplicable.SetSelectedIndex(0);
            //cCmbtxtTaxRates_SurchargeCriteria.SetSelectedIndex(0);
            //ctxtTaxRates_SurchargeAbove.SetText('');
            //cCmbTaxRates_SurchargeOn.SetSelectedIndex(0);
            //ctxtTaxes_SurchargeRate.SetText('');
            //crdpExempted.SetSelectedIndex(1);
            //ChangeSource();

            ////document.getElementById('Popup_Empcitys_txtTaxRates_MainAccount').value = "";
            ////document.getElementById('Popup_Empcitys_txtTaxRates_MainAccount_hidden').value = "";
            ////document.getElementById('Popup_Empcitys_txtTaxRates_SubAccount').value = "";
            ////document.getElementById('Popup_Empcitys_txtTaxRates_SubAccount_hidden').value = "";

            //cPopup_Empcitys.Show();


        }
        function btnSave_citys() {

            //if (ctxtMarkets_Code.GetText() != '' && ctxtMarkets_Name.GetText() != '') {

            visibleValidation();

            var val = cCmbTaxRates_RateOrSlab.GetValue();

         
               
            if (cCmbTaxRates_TaxCode.GetValue() == '0') {
                //alert('Please Enter TaxCode');
                $("#MandatoryTaxCode").show();
                return false;
            }
            else if (ctxt_schemename.GetText() == '') {
                $("#Mandatorytxt_schemename").show();
                return false;

            }
             
            //else if (cCmbCountryName.GetValue() == '0') {

            //    $("#MandatoryCountry").show();
            //    return false;
            //}
            //else if (cCmbState.GetValue() == '0') {

            //    $("#MandatoryState").show();
            //    return false;
            //}
            //else if (cCmbCity.GetValue() == '0') {

            //    $("#MandatoryCity").show();
            //    return false;
            //}
            else if (val == '0') {
                //alert('Please Enter Rate Or Slab');
                $("#MandatoryRateOrSlab").show();
                return false;
            }
            

            //Bellow line commented by debjyoti 
            //Reason : Rate should be allow zero 
            //else if (val == 'R' && ctxtTaxRates_Rate.GetText() == "0.0") {
            //    //alert('Please Enter TaxRates Rate');
            //    $("#MandatoryRate").show();
            //    return false;
            //}



            else if (cCmbTaxRates_SurchargeApplicable.GetValue() == '0') {
                //alert('Please Enter Surcharge Applicable');
                $("#MandatorySurchargeApplicable").show();
                return false;
            }
            else if (ctxtTaxRates_DateFrom.GetValue() == null) {
                //alert('Please Enter Surcharge Applicable');
                $("#MandatoryDateFrom").show();
                return false;
            }
                
            else if (val == 'R' && ctxtTaxRates_Rate.GetText() == "") {
                //alert('Please Enter TaxRates Rate');
                $("#MandatoryRate").show();
                return false;
            }
            //comment by sanjib due to logic has been chnaged 412017

            //else if (val == 'S' && cCmbTaxSlab_Code.GetValue() == "0") {
            //    //alert('Please Enter TaxRates SlabCode');
            //    $("#MandatorySlabCode").show();
            //    return false;
            //}
            //else if (cCmbTaxRates_SurchargeApplicable.GetValue() == 'Y' && cCmbtxtTaxRates_SurchargeCriteria.GetValue() == '0') {
            //    //alert('Please Enter TaxRates SurchargeCriteria');
            //    $("#MandatorySurchargeCriteria").show();
            //    return false;
            //}
            //else if (cCmbTaxRates_SurchargeApplicable.GetValue() == 'Y' && ctxtTaxRates_SurchargeAbove.GetText() == "") {
            //        //alert('Please Enter TaxRates SurchargeAbove');
            //        $("#MandatorySurchargeAbove").show();
            //        return false;
            //    }
            
            //else if (cCmbTaxRates_SurchargeApplicable.GetValue() == 'Y' && cCmbTaxRates_SurchargeOn.GetValue() == "0") {
            //    //alert('Please Enter TaxRates SurchargeOn');
            //    $("#MandatorySurchargeOn").show();
            //    return false;
            //}
            //else if (cCmbTaxRates_SurchargeApplicable.GetValue() == 'Y' && ctxtTaxes_SurchargeRate.GetText() == "") {
            //    //alert('Please Enter Taxes SurchargeRate');
            //    $("#MandatorySurchargeRate").show();
            //    return false;
            //}

            //else if (document.getElementById('Popup_Empcitys_txtTaxRates_MainAccount_hidden').value == "") {
            //    alert('Please Enter Proper TaxRates MainAccount');
            //}
            //else if (document.getElementById('Popup_Empcitys_txtTaxRates_MainAccount_hidden').value == "Customers" && document.getElementById('Popup_Empcitys_txtTaxRates_SubAccount_hidden').value == "") {
            //    alert('Please Enter Proper TaxRates SubAccount');
            //}
            //end
            var maincountval = document.getElementById("hndTaxRates_MainAccount_hidden").value;
            if (maincountval == null || maincountval == "" || maincountval == 0) {

                $("#MandatorylstTaxRates_MainAccount").show();
                return false;
            }
            
            if (document.getElementById('hiddenedit').value == '') {

                grid.PerformCallback('savecity~');


                //--------------- Comments By Sudip 14122016

                //document.getElementById('hiddenedit').value = "";

                //var val = cCmbTaxRates_SurchargeApplicable.GetValue();
                //if (val == 'Y') {
                //    document.getElementById('divTaxRates_SurchargeCriteria').style.display = 'block';
                //    document.getElementById('divTaxRates_SurchargeAbove').style.display = 'block';
                //    document.getElementById('divtxtTaxes_SurchargeRate').style.display = 'block';
                //    document.getElementById('divCmbTaxRates_SurchargeOn').style.display = 'block';
                //}
                //else {
                //    document.getElementById('divtxtTaxes_SurchargeRate').style.display = 'none';
                //    document.getElementById('divCmbTaxRates_SurchargeOn').style.display = 'none';
                //    document.getElementById('divTaxRates_SurchargeCriteria').style.display = 'none';
                //    document.getElementById('divTaxRates_SurchargeAbove').style.display = 'none';
                //}

                //var val1 = cCmbTaxRates_RateOrSlab.GetValue();

                //if (val1 == 'R') {
                //    ctxtTaxRates_Rate.SetText('');
                //    ctxtTaxRates_MinAmount.SetText('');
                //    //ctxtTaxRates_SlabCode.SetText('');
                //    cCmbTaxSlab_Code.SetSelectedIndex(0);

                //    document.getElementById('divtxtTaxRates_Rate').style.display = 'block';
                //    document.getElementById('divtxtTaxRates_MinAmount').style.display = 'block';
                //    document.getElementById('divtxtTaxRates_SlabCode').style.display = 'none';

                //}
                //else {
                //    ctxtTaxRates_Rate.SetText('');
                //    ctxtTaxRates_MinAmount.SetText('');
                //    //ctxtTaxRates_SlabCode.SetText('');
                //    cCmbTaxSlab_Code.SetSelectedIndex(0);

                //    document.getElementById('divtxtTaxRates_Rate').style.display = 'none';
                //    document.getElementById('divtxtTaxRates_MinAmount').style.display = 'none';
                //    document.getElementById('divtxtTaxRates_SlabCode').style.display = 'block';
                //}

                //cCmbTaxRates_TaxCode.Focus();
                //cCmbCountryName.SetSelectedIndex(0);
                //cCmbTaxSlab_Code.SetSelectedIndex(0);
                //cCmbState.SetSelectedIndex(0);
                //cCmbCity.SetSelectedIndex(0);
                //cCmbTaxRates_TaxCode.SetSelectedIndex(0);
                //cCmbTaxRates_TaxCode.SetSelectedIndex(0);
                //cCmbTaxRates_ProductClass.SetSelectedIndex(0);

                ////ctxtTaxRates_DateFrom.SetText('');
                //var date = new Date();
                //ctxtTaxRates_DateFrom.SetDate(date);

                //cCmbTaxRates_RateOrSlab.SetSelectedIndex(0);
                //ctxtTaxRates_Rate.SetText('');
                //ctxtTaxRates_MinAmount.SetText('');
                //////ctxtTaxRates_SlabCode.SetText('');
                //cCmbTaxRates_SurchargeApplicable.SetSelectedIndex(0);
                //cCmbtxtTaxRates_SurchargeCriteria.SetSelectedIndex(0);
                //ctxtTaxRates_SurchargeAbove.SetText('');
                //cCmbTaxRates_SurchargeOn.SetSelectedIndex(0);
                //ctxtTaxes_SurchargeRate.SetText('');
                ////document.getElementById('Popup_Empcitys_txtTaxRates_MainAccount').value = "";
                ////document.getElementById('Popup_Empcitys_txtTaxRates_MainAccount_hidden').value = "";
                ////document.getElementById('Popup_Empcitys_txtTaxRates_SubAccount').value = "";
                ////document.getElementById('Popup_Empcitys_txtTaxRates_SubAccount_hidden').value = "";

            }
            else {
                grid.PerformCallback('updatecity~' + GetObjectID('hiddenedit').value);
            }

        }
        //function CheckDecimal(objId, evt) {
        //    var obj = document.getElementById(objId);
        //    var dotcontainer = obj.value.split('.');
        //    var precision = 0;
        //    precision = obj.value.split('.')[1];

        //    var charCode = (evt.which) ? evt.which : evt.keyCode;
        //    if (charCode != 8) {
        //        if (!(dotcontainer.length == 1 && charCode == 46) && charCode > 31 && (charCode < 48 || charCode > 57)) {
        //            alert('Please enter numeric value');
        //            evt.preventDefault();
        //            return false;
        //        }
        //        if (precision != null && precision.length > 7) {
        //            evt.preventDefault();
        //            return false;
        //        }
        //    }
        //}

        function fn_btnCancel() {
            cPopup_Empcitys.Hide();
        }
        function fn_Editcity(keyValue) {
         window.location.href = '/OMS/management/store/Settings_Options/TaxSchemeAddEdit.aspx?id='+keyValue;
        //    visibleValidation();
         //   grid.PerformCallback('Edit~' + keyValue);
        }
        function fn_Deletecity(keyValue) {

            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {

                    grid.PerformCallback('Delete~' + keyValue);
                }
            });
            
        }
        function grid_EndCallBack() {
            if (grid.cpinsert != null) {
                if (grid.cpinsert == 'Success') {
                    jAlert('Saved Successfully');
                    cPopup_Empcitys.Hide();
                }
                else if (grid.cpinsert == 'Invalid') {
                    jAlert("Should be compulsorily greater than the datefrom value of the earlier instance of rate-configuration of this 'TaxCode+ProductClass+Country+State+City' combination \n 'Please Try Another date!!'");
                    cPopup_Empcitys.Hide();
                }
                else {
                    jAlert("Error On Insertion \n 'Please Try Again!!'");
                    cPopup_Empcitys.Hide();
                }
            }
            if (grid.cpEdit != null) {

                cCmbTaxRates_TaxCode.SetValue(grid.cpEdit.split('~')[0]);
                cCmbTaxRates_ProductClass.SetValue(grid.cpEdit.split('~')[1]);
                ctxtTaxRates_DateFrom.SetText(grid.cpEdit.split('~')[2]);
                cCmbTaxRates_RateOrSlab.SetValue(grid.cpEdit.split('~')[4]);
                ctxtTaxRates_Rate.SetText(grid.cpEdit.split('~')[5]);
                ctxtTaxRates_MinAmount.SetText(grid.cpEdit.split('~')[6]);
                cCmbTaxSlab_Code.SetValue(grid.cpEdit.split('~')[7]);
                cCmbTaxRates_SurchargeApplicable.SetValue(grid.cpEdit.split('~')[8]);
                cCmbtxtTaxRates_SurchargeCriteria.SetValue(grid.cpEdit.split('~')[9]);
                ctxtTaxRates_SurchargeAbove.SetText(grid.cpEdit.split('~')[10]);
                cCmbTaxRates_SurchargeOn.SetValue(grid.cpEdit.split('~')[11]);
                ctxtTaxes_SurchargeRate.SetText(grid.cpEdit.split('~')[12]);

                cCmbCountryName.SetValue(grid.cpEdit.split('~')[15]);
                cCmbState.SetValue(grid.cpEdit.split('~')[13]);
                cCmbCity.PerformCallback("BindCity~" + cCmbState.GetValue());

                
                if (grid.cpEdit.split('~')[14] != "0") {
                   
                    cCmbCity.SetValue(grid.cpEdit.split('~')[14]);
                } else {
                    cCmbCity.SetText("Any");
                }
                

                /*Code  Added  By Sudip on 14122016 for Edit function*/


                document.getElementById('hiddenedit').value = grid.cpEdit.split('~')[18];
                document.getElementById('hndTaxRates_MainAccount_hidden').value = grid.cpEdit.split('~')[16];
                document.getElementById('hndTaxRates_SubAccount_hidden').value = grid.cpEdit.split('~')[17];

                ctxt_schemename.SetText(grid.cpEdit.split('~')[19]);
               
                
                if (typeof (grid.cpEdit.split('~')[20]) != 'undefined') {
                    crdpExempted.SetSelectedIndex(grid.cpEdit.split('~')[20]);
                } else {

                    crdpExempted.SetSelectedIndex(1);
                }
                
               
                var valuenew = document.getElementById('hndTaxRates_MainAccount_hidden').value;
                var valuenew1 = document.getElementById('hndTaxRates_SubAccount_hidden').value;

                ChangeSource();
                //changeFunc();
                //ChangeSubSource();

                if (valuenew == "NONE") {

                    document.getElementById('divtxtTaxRates_SubAccount').style.display = 'none';
                }
                if (valuenew == "") {

                    document.getElementById('divtxtTaxRates_SubAccount').style.display = 'none';
                }
                else {
                    document.getElementById('divtxtTaxRates_SubAccount').style.display = 'block';
                    ChangeSubSource();
                }

                ChangeSubSource();

                var val = cCmbTaxRates_SurchargeApplicable.GetValue();
                if (val == 'Y') {

                    document.getElementById('divTaxRates_SurchargeCriteria').style.display = 'block';
                    document.getElementById('divTaxRates_SurchargeAbove').style.display = 'block';
                    document.getElementById('divtxtTaxes_SurchargeRate').style.display = 'block';
                    document.getElementById('divCmbTaxRates_SurchargeOn').style.display = 'block';
                }
                else {
                    document.getElementById('divtxtTaxes_SurchargeRate').style.display = 'none';
                    document.getElementById('divCmbTaxRates_SurchargeOn').style.display = 'none';
                    document.getElementById('divTaxRates_SurchargeCriteria').style.display = 'none';
                    document.getElementById('divTaxRates_SurchargeAbove').style.display = 'none';
                }

                var val1 = cCmbTaxRates_RateOrSlab.GetValue();

                if (val1 == 'R') {
                    document.getElementById('divtxtTaxRates_Rate').style.display = 'block';
                    document.getElementById('divtxtTaxRates_MinAmount').style.display = 'block';
                    document.getElementById('divtxtTaxRates_SlabCode').style.display = 'none';

                }
                else {
                    document.getElementById('divtxtTaxRates_Rate').style.display = 'none';
                    document.getElementById('divtxtTaxRates_MinAmount').style.display = 'none';
                    document.getElementById('divtxtTaxRates_SlabCode').style.display = 'block';
                }

                //.................code end.......

                cPopup_Empcitys.Show();
            }
            if (grid.cpUpdate != null) {
                if (grid.cpUpdate == 'Success') {
                    jAlert('Updated Successfully');
                    cPopup_Empcitys.Hide();
                }
                else {
                    jAlert("Error on Updation\n'Please Try again!!'")
                    cPopup_Empcitys.Hide();
                }
            }
            if (grid.cpUpdateValid != null) {
                if (grid.cpUpdateValid == "StateInvalid") {
                    //$("#MandatoryState").show();
                    //$("#MandatoryCity").show();
                    //alert("Please Select proper country state and city");
                    return true;
                }
                else if (grid.cpUpdateValid == "TaxCodeInvalid") {
                    jAlert("Please Select proper TaxCode");
                    return false;
                }
                else if (grid.cpUpdateValid == "dateInvalid") {
                    $("#MandatoryDateFrom").show();
                    //alert("Please Select proper date");
                    return false;
                }
            }


            if (grid.cpDelete != null) {
                if (grid.cpDelete == 'Success')
                    jAlert('Deleted Successfully');
                else if (grid.cpDelete == 'inUse') 
                    jAlert('Transaction exists. Cannot Delete.');
                else
                    jAlert("Error on deletion\n'Please Try again!!'")

                grid.cpDelete = null;
            }
            if (grid.cpExists != null) {
                if (grid.cpExists == "Exists") {
                    jAlert('Record already Exists');
                    cPopup_Empcitys.Hide();
                }
                else {
                    jAlert("Error on operation \n 'Please Try again!!'")
                    cPopup_Empcitys.Hide();
                }
            }
        }

        /*Code  Comment By Sudip on 14122016*/

        //function FunCallAjaxList(objID, objEvent, ObjType) {
        //    var strQuery_Table = '';
        //    var strQuery_FieldName = '';
        //    var strQuery_WhereClause = '';
        //    var strQuery_OrderBy = '';
        //    var strQuery_GroupBy = '';
        //    var CombinedQuery = '';
        //    if (ObjType == 'Digital') {
        //        var alert1 = document.getElementById('HiddenField1').value;

        //        var txtTaxRates_MainAccount = document.getElementById('Popup_Empcitys_txtTaxRates_MainAccount').value;


        //        strQuery_Table = "Master_MainAccount";
        //        strQuery_FieldName = "MainAccount_Name,MainAccount_AccountCode+'-'+MainAccount_SubLedgerType as MainAccount_AccountCode";
        //        strQuery_WhereClause = "MainAccount_Name like '" + txtTaxRates_MainAccount + "%'";

        //        // strQuery_WhereClause = " user_group not IN (\%52%') and ( USER_NAME like (\'%RequestLetter%') or user_loginId like (\'%RequestLetter%'))";

        //    }
        //    CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);

        //    //ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars(CombinedQuery));
        //    ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars(CombinedQuery), "Main");

        //}
        //function onBlrTXT() {
        //    document.getElementById('divtxtTaxRates_SubAccount').style.display = 'none';
        //    var MainAccount_val = document.getElementById('Popup_Empcitys_txtTaxRates_MainAccount_hidden').value.split("-")[0];
        //    var MainAccount_val2 = "";

        //    if (MainAccount_val != null && MainAccount_val != "") {

        //        document.getElementById('divtxtTaxRates_SubAccount').style.display = 'none';
        //        MainAccount_val2 = document.getElementById('Popup_Empcitys_txtTaxRates_MainAccount_hidden').value.split("-")[1];
        //        //alert(MainAccount_val2);

        //        if (MainAccount_val2 == "NONE") {

        //            document.getElementById('divtxtTaxRates_SubAccount').style.display = 'none';
        //        }
        //        else {
        //            // alert(MainAccount_val2);
        //            document.getElementById('divtxtTaxRates_SubAccount').style.display = 'block';
        //        }
        //    }
        //    else {

        //        // document.getElementById('divtxtTaxRates_SubAccount').style.display = 'none';
        //    }


        //}
        //function replaceChars(entry) {
        //    out = "+"; // replace this
        //    add = "--"; // with this
        //    temp = "" + entry; // temporary holder

        //    while (temp.indexOf(out) > -1) {
        //        pos = temp.indexOf(out);
        //        temp = "" + (temp.substring(0, pos) + add +
        //    temp.substring((pos + out.length), temp.length));
        //    }

        //    return temp;
        //}
        //function FunCallAjaxList2(objID, objEvent, ObjType) {

        //    var strQuery_Table = '';
        //    var strQuery_FieldName = '';
        //    var strQuery_WhereClause = '';
        //    var strQuery_OrderBy = '';
        //    var strQuery_GroupBy = '';
        //    var CombinedQuery = '';
        //    var MainAccount_val = document.getElementById('Popup_Empcitys_txtTaxRates_MainAccount_hidden').value.split("-")[0];
        //    //alert(Popup_Empcitys_txtTaxRates_MainAccount_hidden);
        //    if (ObjType == 'Digital') {
        //        var alert1 = document.getElementById('HiddenField1').value;

        //        var txtTaxRates_SubAccount = document.getElementById('Popup_Empcitys_txtTaxRates_SubAccount').value;

        //        //alert(alert1);
        //        strQuery_Table = "Master_SubAccount";
        //        strQuery_FieldName = "SubAccount_Name,SubAccount_ReferenceID";
        //        strQuery_WhereClause = " SubAccount_MainAcReferenceID = '" + MainAccount_val + "' and SubAccount_Name like '" + txtTaxRates_SubAccount + "%'";


        //    }
        //    CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);

        //    ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars2(CombinedQuery));
        //    //alert (CombinedQuery);
        //}
        //function replaceChars2(entry) {
        //    out = "+"; // replace this
        //    add = "--"; // with this
        //    temp = "" + entry; // temporary holder

        //    while (temp.indexOf(out) > -1) {
        //        pos = temp.indexOf(out);
        //        temp = "" + (temp.substring(0, pos) + add +
        //    temp.substring((pos + out.length), temp.length));
        //    }

        //    return temp;
        //}

        //----------------------End

        function OnCmbCountryName_ValueChange() {
            cCmbState.PerformCallback("BindState~" + cCmbCountryName.GetValue());
        }
        function CmbState_EndCallback() {
            cCmbState.SetSelectedIndex(0);
            cCmbState.Focus();
        }
        function OnCmbStateName_ValueChange() {
            cCmbCity.PerformCallback("BindCity~" + cCmbState.GetValue());
        }
        function CmbCity_EndCallback() {
            cCmbCity.SetSelectedIndex(0);
            cCmbCity.Focus();
        }
        function CmbCity_ValuChanged(s, e) {
            $('#hdnCityId').val(s.GetValue());
        }
        function CmbTaxRates_RateOrSlab_ValueChange() {
            var val = cCmbTaxRates_RateOrSlab.GetValue();

            if (val == 'R') {
                ctxtTaxRates_Rate.SetText('');
                ctxtTaxRates_MinAmount.SetText('');
                //ctxtTaxRates_SlabCode.SetText('0');
                cCmbTaxSlab_Code.SetSelectedIndex(0);

                document.getElementById('divtxtTaxRates_Rate').style.display = 'block';
                document.getElementById('divtxtTaxRates_MinAmount').style.display = 'block';
                document.getElementById('divtxtTaxRates_SlabCode').style.display = 'none';

            }
            else {
                ctxtTaxRates_Rate.SetText('0');
                ctxtTaxRates_MinAmount.SetText('0');
                //ctxtTaxRates_SlabCode.SetText('');
                cCmbTaxSlab_Code.SetSelectedIndex(0);

                document.getElementById('divtxtTaxRates_Rate').style.display = 'none';
                document.getElementById('divtxtTaxRates_MinAmount').style.display = 'none';
                document.getElementById('divtxtTaxRates_SlabCode').style.display = 'block';
            }
        }
        function CmbTaxRates_SurchargeApplicable_ValueChange() {
            var val = cCmbTaxRates_SurchargeApplicable.GetValue();
            if (val == 'Y') {

                document.getElementById('divTaxRates_SurchargeCriteria').style.display = 'block';
                document.getElementById('divTaxRates_SurchargeAbove').style.display = 'block';
                document.getElementById('divtxtTaxes_SurchargeRate').style.display = 'block';
                document.getElementById('divCmbTaxRates_SurchargeOn').style.display = 'block';
            }
            else {
                ctxtTaxes_SurchargeRate.SetText('0');
                cCmbtxtTaxRates_SurchargeCriteria.SetSelectedIndex(0);
                ctxtTaxRates_SurchargeAbove.SetText('0');

                document.getElementById('divtxtTaxes_SurchargeRate').style.display = 'none';
                document.getElementById('divCmbTaxRates_SurchargeOn').style.display = 'none';
                document.getElementById('divTaxRates_SurchargeCriteria').style.display = 'none';
                document.getElementById('divTaxRates_SurchargeAbove').style.display = 'none';
            }
        }
    </script>
    <script type="text/javascript">
        /*Code  Added  By Sudip on 14122016 to use jquery Choosen*/

        $(document).ready(function () {
            ListBind();
            //ChangeSource();
        });
        function ListBind() {

            var config = {
                '.chsn': {},
                '.chsn-deselect': { allow_single_deselect: true },
                '.chsn-no-single': { disable_search_threshold: 10 },
                '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
                '.chsn-width': { width: "100%" }
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }

        }
        function lstTaxRates_MainAccount() {
            $('#lstTaxRates_MainAccount').fadeIn();
        }
        function Changeselectedvalue() {
            var lstTaxRates_MainAccount = document.getElementById("lstTaxRates_MainAccount");
            if (document.getElementById("hndTaxRates_MainAccount_hidden").value != '') {
                for (var i = 0; i < lstTaxRates_MainAccount.options.length; i++) {
                    if (lstTaxRates_MainAccount.options[i].value == document.getElementById("hndTaxRates_MainAccount_hidden").value) {
                        lstTaxRates_MainAccount.options[i].selected = true;
                    }
                }
                $('#lstTaxRates_MainAccount').trigger("chosen:updated");
            }
        }
        function ChangeSource() {
            var fname = "%";
            var lReportTo = $('select[id$=lstTaxRates_MainAccount]');
            lReportTo.empty();

            $.ajax({
                type: "POST",
                url: "Config_TaxLevies.aspx/GetMainAccountList",
                data: JSON.stringify({ reqStr: fname }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {
                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];

                            $('#lstTaxRates_MainAccount').append($('<option>').text(name).val(id));
                        }

                        $(lReportTo).append(listItems.join(''));
                        lstTaxRates_MainAccount();
                        $('#lstTaxRates_MainAccount').trigger("chosen:updated");
                        Changeselectedvalue();
                    }
                    else {
                        $('#lstTaxRates_MainAccount').trigger("chosen:updated");
                    }
                }
            });
        }
        function lstTaxRates_SubAccount() {
            $('#lstTaxRates_SubAccount').fadeIn();
        }
        function ChangeSubselectedvalue() {
            var lstTaxRates_SubAccount = document.getElementById("lstTaxRates_SubAccount");
            if (document.getElementById("hndTaxRates_SubAccount_hidden").value != '') {
                for (var i = 0; i < lstTaxRates_SubAccount.options.length; i++) {
                    if (lstTaxRates_SubAccount.options[i].value == document.getElementById("hndTaxRates_SubAccount_hidden").value) {
                        lstTaxRates_SubAccount.options[i].selected = true;
                    }
                }
                $('#lstTaxRates_SubAccount').trigger("chosen:updated");
            }
        }
        function ChangeSubSource() {
            var fname = "%";
            //var mainAccount = document.getElementById("lstTaxRates_MainAccount").value;;
            var mainAccount = document.getElementById("hndTaxRates_MainAccount_hidden").value;;
            var lReportTo = $('select[id$=lstTaxRates_SubAccount]');
            lReportTo.empty();

            $.ajax({
                type: "POST",
                url: "Config_TaxLevies.aspx/GetSubAccountList",
                data: JSON.stringify({ reqStr: fname, mainreqStr: mainAccount }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {
                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];

                            $('#lstTaxRates_SubAccount').append($('<option>').text(name).val(id));
                        }

                        $(lReportTo).append(listItems.join(''));
                        ListBind();
                        lstTaxRates_SubAccount();
                        $('#lstTaxRates_SubAccount').trigger("chosen:updated");
                        ChangeSubselectedvalue();
                        $('#lstTaxRates_SubAccount').trigger("chosen:updated");
                    }
                    else {
                        $('#lstTaxRates_SubAccount').trigger("chosen:updated");
                    }
                }
            });
        }
        function changeFunc() {
            var MainAccount_val2 = document.getElementById("lstTaxRates_MainAccount").value;
            document.getElementById("hndTaxRates_MainAccount_hidden").value = document.getElementById("lstTaxRates_MainAccount").value;

            if (MainAccount_val2 == "NONE") {

                document.getElementById('divtxtTaxRates_SubAccount').style.display = 'none';
            }
            if (MainAccount_val2 == "") {

                document.getElementById('divtxtTaxRates_SubAccount').style.display = 'none';
            }
            else {
                document.getElementById('divtxtTaxRates_SubAccount').style.display = 'block';
                ChangeSubSource();
            }
        }
        function changeSubFunc() {
            document.getElementById("hndTaxRates_SubAccount_hidden").value = document.getElementById("lstTaxRates_SubAccount").value;
        }
    </script>
    <style type="text/css">
        #lstTaxRates_MainAccount {
            width: 100%;
        }

        #lstTaxRates_MainAccount {
            display: none !important;
        }

        #lstTaxRates_MainAccount_chosen {
            width: 100% !important;
        }

        #lstTaxRates_SubAccount {
            width: 100%;
        }

        #lstTaxRates_SubAccount {
            display: none !important;
        }

        #lstTaxRates_SubAccount_chosen {
            width: 100% !important;
        }

        #txtTaxRates_Rate_EC, #txtTaxRates_SurchargeAbove_EC, #txtTaxes_SurchargeRate_EC, #txtTaxRates_MinAmount_EC {
            position: absolute;
        }
    </style>
    <style type="text/css">
        .cityDiv {
            height: 25px;
            width: 155px;
            float: left;
            margin-left: 70px;
        }

        .cityTextbox {
            height: 25px;
            width: 50px;
        }

        .Top {
            padding-top: 5px;
            valign: top;
        }

        .Footer {
            height: 30px;
            width: 400px;
            padding-top: 10px;
        }

        .ScrollDiv {
            height: 250px;
            width: 400px;
            overflow-x: hidden;
            overflow-y: scroll;
        }

        .ContentDiv {
            width: 400px;
            border: 2px;
        }

       

        .TitleArea {
            height: 20px;
            padding-left: 10px;
            padding-right: 3px;
            background-image: url( '../images/EHeaderBack.gif' );
            background-repeat: repeat-x;
            background-position: bottom;
            text-align: center;
        }

        .FilterSide {
            float: left;
            padding-left: 15px;
            width: 50%;
        }

        .SearchArea {
            width: 100%;
            height: 30px;
            padding-top: 5px;
        }
        /* Big box with list of options */ #ajax_listOfOptions {
            position: absolute; /* Never change this one */
            width: 50px; /* Width of box */
            height: auto; /* Height of box */
            overflow: auto; /* Scrolling features */
            border: 1px solid Blue; /* Blue border */
            background-color: #FFF; /* White background color */
            text-align: left;
            font-size: 0.9em;
            z-index: 32767;
        }

            #ajax_listOfOptions div {
                /* General rule for both .optionDiv and .optionDivSelected */
                margin: 1px;
                padding: 1px;
                cursor: pointer;
                font-size: 0.9em;
            }

            #ajax_listOfOptions .optionDiv {
                /* Div for each item in list */
            }

            #ajax_listOfOptions .optionDivSelected {
                /* Selected item in the list */
                background-color: #DDECFE;
                color: Blue;
            }

        #ajax_listOfOptions_iframe {
            background-color: #F00;
            position: absolute;
            z-index: 3000;
        }

        form {
            display: inline;
        }
    </style>
    <style type="text/css">
        .inline {
            display: inline !important;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content {
            overflow: visible !important;
        }

        .pdl8 {
            padding-left: 8px;
        }

        .abs {
            position: absolute;
            right: -20px;
            top: 10px;
        }

        .fa.fa-exclamation-circle:before {
            font-family: FontAwesome;
        }

        .tp2 {
            right: -18px;
            top: 9px;
            position: absolute;
        }

        .Left_Content {
            position: relative;
        }

        .col-md-6 {
            position: relative;
        }

        .pullleftClass {
            position: absolute;
            right: -17px;
            top: 5px;
        }
    </style>
    <script>
        function gridRowclick(s, e) {
            $('#cityGrid').find('tr').removeClass('rowActive');
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
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                grid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                grid.SetWidth(cntWidth);
            }
            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    grid.SetWidth(cntWidth);
                }

            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading">
        <div class="panel-title">
            <h3>Tax Components Scheme
            </h3>
        </div>
    </div>
    <div class="form_main">

        <div class="">
            <div class="clearfix">
                <a href="javascript:void(0);" onclick="fn_PopOpen()" class="btn btn-success btn-radius">
                    <span class="btn-icon"><i class="fa fa-plus" ></i></span>
                    <span>Add New</span> 
                </a>
               
				<% if (rights.CanExport)
                                               { %>
                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">PDF</asp:ListItem>
                    <asp:ListItem Value="2">XLS</asp:ListItem>
                    <asp:ListItem Value="3">RTF</asp:ListItem>
                    <asp:ListItem Value="4">CSV</asp:ListItem>
                </asp:DropDownList>
                 <% } %>
            </div>

            <%--<div class="ExportSide">
                    <div style="margin-left: 90%;">
                        <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" BackColor="Navy"
                            Font-Bold="False" ForeColor="White" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                            ValueType="System.Int32" Width="130px">
                            <Items>
                                <dxe:ListEditItem Text="Select" Value="0" />
                                <dxe:ListEditItem Text="PDF" Value="1" />
                                <dxe:ListEditItem Text="XLS" Value="2" />
                                <dxe:ListEditItem Text="RTF" Value="3" />
                                <dxe:ListEditItem Text="CSV" Value="4" />
                            </Items>
                            <ButtonStyle BackColor="#C0C0FF" ForeColor="Black">
                            </ButtonStyle>
                            <ItemStyle BackColor="Navy" ForeColor="White">
                                <HoverStyle BackColor="#8080FF" ForeColor="White">
                                </HoverStyle>
                            </ItemStyle>
                            <Border BorderColor="White" />
                            <DropDownButton Text="Export">
                            </DropDownButton>
                        </dxe:ASPxComboBox>
                    </div>
                </div>--%>
        </div>
    </div>
    <div class="GridViewArea relative">
        <dxe:ASPxGridView ID="cityGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
            KeyFieldName="TaxRates_ID" Width="100%" OnHtmlRowCreated="cityGrid_HtmlRowCreated" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
            OnHtmlEditFormCreated="cityGrid_HtmlEditFormCreated" OnCustomCallback="cityGrid_CustomCallback" SettingsBehavior-AllowFocusedRow="true"  SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"  >
            <SettingsSearchPanel Visible="True"  Delay="5000"/>
            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true"  />
            <Columns>
                <dxe:GridViewDataTextColumn Caption="ID" FieldName="TaxRates_ID" ReadOnly="True"
                    Visible="False" FixedStyle="Left" VisibleIndex="0">
                    <EditFormSettings Visible="False" />
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                 <dxe:GridViewDataTextColumn Caption="Tax Component ID" FieldName="TaxRatesSchemeName" ReadOnly="True"
                    Visible="true" FixedStyle="Left" VisibleIndex="1">
                    <EditFormSettings Visible="False" />
                     <Settings AllowAutoFilterTextInputTimer="False" />
                     <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Country" FieldName="TaxRates_Country" ReadOnly="True"
                    Visible="False" FixedStyle="Left" VisibleIndex="2">
                    <EditFormSettings Visible="False" />
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="State" FieldName="TaxRates_State" ReadOnly="True"
                    Visible="False" FixedStyle="Left" VisibleIndex="3">
                    <EditFormSettings Visible="False" />
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="City" FieldName="TaxRates_City" ReadOnly="True"
                    Visible="False" FixedStyle="Left" VisibleIndex="4">
                    <EditFormSettings Visible="False" />
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Tax Component Name" FieldName="Taxes_Code" Width="12%"
                    FixedStyle="Left" Visible="True" VisibleIndex="5">
                    <EditFormSettings Visible="True" />
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                 <dxe:GridViewDataTextColumn Caption="Rate" FieldName="TaxRates_Rate" Width="12%"
                    FixedStyle="Left" Visible="True" VisibleIndex="6" HeaderStyle-HorizontalAlign="Right">
                   <PropertiesTextEdit DisplayFormatString="0.000"></PropertiesTextEdit>
                    <EditFormSettings Visible="True" />
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                  <dxe:GridViewDataTextColumn Caption="Product Class" FieldName="ProductClass_Code"
                    Width="12%" FixedStyle="Left" Visible="True" VisibleIndex="7">
                    <EditFormSettings Visible="True" />
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Country" FieldName="cou_country" Visible="True"
                    Width="7%" VisibleIndex="8">
                    <Settings AutoFilterCondition="Contains" />
                    <%--  <CellStyle CssClass="gridcellleft" Wrap="False">
                        </CellStyle>--%>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="State" FieldName="state" Width="12%" FixedStyle="Left"
                    Visible="True" VisibleIndex="9">
                    <EditFormSettings Visible="True" />
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="City Name" FieldName="city_name" Width="12%"
                    FixedStyle="Left" Visible="True" VisibleIndex="10">
                    <EditFormSettings Visible="True" />
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Date From" FieldName="TaxRates_DateFrom" Width="12%"
                    FixedStyle="Left" Visible="True" VisibleIndex="11">
                    <EditFormSettings Visible="True" />
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn ReadOnly="True" Width="0"  VisibleIndex="11" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <%-- <a href="javascript:void(0);" onclick="fn_PopOpen()"><span style="color: #000099;
                                text-decoration: underline">Add New</span> </a>--%>
                            
                    </HeaderTemplate>
                    <%--<DataItemTemplate>
                        <a href="javascript:void(0);" onclick="fn_Editcity('<%# Container.KeyValue %>')">Edit</a>
                        <a href="javascript:void(0);" onclick="fn_Deletecity('<%# Container.KeyValue %>')">Delete</a>
                    </DataItemTemplate>--%>

                    <DataItemTemplate>
                        <div class='floatedBtnArea'>
                        <% if (rights.CanEdit)
                           { %>
                        <a href="javascript:void(0);" onclick="fn_Editcity('<%# Container.KeyValue %>')" class="" title="">
                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a><%} %>
                        <% if (rights.CanDelete)
                           { %>
                        <a href="javascript:void(0);" onclick="fn_Deletecity('<%# Container.KeyValue %>')" title="">
                            <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a><%} %>
                        </div>
                    </DataItemTemplate>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
            </Columns>
            <SettingsContextMenu Enabled="true"></SettingsContextMenu>
            <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </SettingsPager>
            <ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack();}" RowClick="gridRowclick" />
        </dxe:ASPxGridView>
    </div>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="Popup_Empcitys" runat="server" ClientInstanceName="cPopup_Empcitys"
            Width="600px" HeaderText="Add/Edit Tax Components Scheme" PopupHorizontalAlign="WindowCenter"
            BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentStyle VerticalAlign="Top" CssClass="pad">
            </ContentStyle>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                    <div class="Top clearfix">
                        <div class="col-md-6">
                            <div class="padBot5" style="display: block;">
                                <span>Tax Component Name</span><span style="text-align: left; float: left; font-size: medium; color: Red; width: 8px;">*</span>
                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxComboBox ID="CmbTaxRates_TaxCode" EnableIncrementalFiltering="true" ClientInstanceName="cCmbTaxRates_TaxCode"
                                    runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                </dxe:ASPxComboBox>
                                <span id="MandatoryTaxCode" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="padBot5" style="display: block;">
                                Product Class
                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxComboBox ID="CmbTaxRates_ProductClass" EnableIncrementalFiltering="true"
                                    ClientInstanceName="cCmbTaxRates_ProductClass" runat="server" ValueType="System.String"
                                    Width="100%" EnableSynchronization="True">
                                </dxe:ASPxComboBox>
                            </div>
                        </div>
                         <div class="col-md-6">
                            <div class="padBot5" style="display: block;">
                                Tax Component ID<span style="text-align: left; float: left; font-size: medium; color: Red; width: 8px;">*</span>
                            </div>
                            <div class="Left_Content">
                              <dxe:ASPxTextBox ID="txt_schemename" ClientInstanceName="ctxt_schemename" runat="server"
                                    Width="100%">
                                 
                                    <%-- <ClientSideEvents KeyPress="function(s,e){ return CheckDecimal('Popup_Empcitys_txtTaxRates_Rate_I',event);}" />--%>
                                </dxe:ASPxTextBox>

                                <span id="Mandatorytxt_schemename" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="padBot5" style="display: block;">
                                Country
                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxComboBox ID="CmbCountryName" ClientInstanceName="cCmbCountryName" runat="server"
                                    ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                    <ClientSideEvents ValueChanged="function(s,e){OnCmbCountryName_ValueChange()}"></ClientSideEvents>
                                </dxe:ASPxComboBox>
                                <span id="MandatoryCountry" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="padBot5" style="display: block;">
                                State
                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxComboBox ID="CmbState" ClientInstanceName="cCmbState" runat="server" ValueType="System.String"
                                    Width="100%" EnableSynchronization="True" OnCallback="CmbState_Callback" EnableIncrementalFiltering="True">
                                    <ClientSideEvents EndCallback="CmbState_EndCallback" ValueChanged="function(s,e){OnCmbStateName_ValueChange()}"></ClientSideEvents>
                                </dxe:ASPxComboBox>

                                <span id="MandatoryState" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="padBot5" style="display: block;">
                                <span>City</span>
                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxComboBox ID="CmbCity" ClientInstanceName="cCmbCity" runat="server" ValueType="System.String"
                                    Width="100%" EnableSynchronization="True" OnCallback="CmbCity_Callback" EnableIncrementalFiltering="True">
                                    <ClientSideEvents ValueChanged="CmbCity_ValuChanged"></ClientSideEvents>
                                </dxe:ASPxComboBox>

                                <span id="MandatoryCity" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="padBot5" style="display: block;">
                                <span>Date From</span><span style="text-align: left; float: left; font-size: medium; color: Red; width: 8px;">*</span>
                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxDateEdit ID="txtTaxRates_DateFrom" runat="server" Width="100%" ClientInstanceName="ctxtTaxRates_DateFrom"
                                    EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="true" />
                                <span id="MandatoryDateFrom" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="padBot5" style="display: block;">
                                <span>Rate</span>  <span style="text-align: left; float: left; font-size: medium; color: Red; width: 8px;">*</span>
                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxComboBox ID="CmbTaxRates_RateOrSlab" EnableIncrementalFiltering="True" ClientInstanceName="cCmbTaxRates_RateOrSlab"
                                    runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                    <ClientSideEvents ValueChanged="function(s,e){CmbTaxRates_RateOrSlab_ValueChange()}"></ClientSideEvents>
                                </dxe:ASPxComboBox>
                                <span id="MandatoryRateOrSlab" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            </div>
                        </div>
                        <div class="col-md-6" id="divtxtTaxRates_Rate" style="display: block">
                            <div class="padBot5" style="display: block;">
                                <span>Rate</span> <%--<span style="text-align: left; float: left; font-size: medium; color: Red; width: 8px;">*</span>--%>
                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxTextBox ID="txtTaxRates_Rate" ClientInstanceName="ctxtTaxRates_Rate" runat="server"
                                    Width="100%">
                                    <MaskSettings Mask="<0..99999g>.<0..99g>" />
                                    <%-- <ClientSideEvents KeyPress="function(s,e){ return CheckDecimal('Popup_Empcitys_txtTaxRates_Rate_I',event);}" />--%>
                                </dxe:ASPxTextBox>

                                <span id="MandatoryRate" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            </div>
                        </div>
                        
                        <div class="col-md-6" id="divtxtTaxRates_MinAmount" style="display: block">
                            <div class="padBot5" style="display: block;">
                                Min Amount
                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxTextBox ID="txtTaxRates_MinAmount" ClientInstanceName="ctxtTaxRates_MinAmount"
                                    runat="server" Width="100%">
                                    <%--<ClientSideEvents KeyPress="function(s,e){ return CheckDecimal('Popup_Empcitys_txtTaxRates_MinAmount_I',event);}" />--%>
                                    <MaskSettings Mask="<0..99999g>.<0..99g>" />
                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                        <div class="clear"></div>
                        <div class="col-md-6" id="divtxtTaxRates_SlabCode" style="display: none">
                            <div class="padBot5" style="display: block;">
                                Slab Code
                            </div>
                            <div class="Left_Content">
                                <%-- <dxe:ASPxTextBox ID="txtTaxRates_SlabCode" ClientInstanceName="ctxtTaxRates_SlabCode"
                                        runat="server" Width="100%">
                                    </dxe:ASPxTextBox>--%>
                                <dxe:ASPxComboBox ID="CmbTaxSlab_Code" ClientInstanceName="cCmbTaxSlab_Code" runat="server"
                                    ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                    <%-- <ClientSideEvents ValueChanged="function(s,e){OnCmbCountryName_ValueChange()}"></ClientSideEvents>--%>
                                </dxe:ASPxComboBox>

                                <span id="MandatorySlabCode" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="padBot5" style="display: block;">
                                Surcharge Applicable <span style="text-align: left; float: left; font-size: medium; color: Red; width: 8px;">*</span>
                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxComboBox ID="CmbTaxRates_SurchargeApplicable" EnableIncrementalFiltering="True"
                                    ClientInstanceName="cCmbTaxRates_SurchargeApplicable" runat="server" ValueType="System.String"
                                    Width="100%" EnableSynchronization="True">
                                    <ClientSideEvents ValueChanged="function(s,e){CmbTaxRates_SurchargeApplicable_ValueChange()}"></ClientSideEvents>
                                </dxe:ASPxComboBox>

                                <span id="MandatorySurchargeApplicable" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            </div>
                        </div>
                        <div class="col-md-6" id="divTaxRates_SurchargeCriteria" >
                            <div class="padBot5" style="display: block;">
                                Surcharge Criteria
                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxComboBox ID="CmbtxtTaxRates_SurchargeCriteria" EnableIncrementalFiltering="True"
                                    ClientInstanceName="cCmbtxtTaxRates_SurchargeCriteria" runat="server" ValueType="System.String"
                                    Width="100%" EnableSynchronization="True">
                                </dxe:ASPxComboBox>
                                <%--<dxe:ASPxTextBox ID="txtTaxRates_SurchargeCriteria" ReadOnly="true" ClientInstanceName="ctxtTaxRates_SurchargeCriteria"
                                    runat="server" Width="100%">
                                </dxe:ASPxTextBox>--%>

                                <span id="MandatorySurchargeCriteria" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            </div>
                        </div>

                        <div class="col-md-6" id="divTaxRates_SurchargeAbove">
                            <div class="padBot5" style="display: block;">
                                Surcharge Above
                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxTextBox ID="txtTaxRates_SurchargeAbove" ClientInstanceName="ctxtTaxRates_SurchargeAbove"
                                    runat="server" Width="100%">
                                    <%--  <ClientSideEvents KeyPress="function(s,e){ return CheckDecimal('Popup_Empcitys_txtTaxRates_SurchargeAbove_I',event);}" />--%>
                                    <MaskSettings Mask="<0..99999g>.<0..99g>" />
                                </dxe:ASPxTextBox>

                                <span id="MandatorySurchargeAbove" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            </div>
                        </div>
                        
                        <div class="col-md-6" id="divCmbTaxRates_SurchargeOn">
                            <div class="padBot5" style="display: block;">
                                Surcharge On
                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxComboBox ID="CmbTaxRates_SurchargeOn" EnableIncrementalFiltering="True"
                                    ClientInstanceName="cCmbTaxRates_SurchargeOn" runat="server" ValueType="System.String"
                                    Width="100%" EnableSynchronization="True">
                                </dxe:ASPxComboBox>

                                <span id="MandatorySurchargeOn" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            </div>
                        </div>
                        <div class="clear"></div>
                        <div class="col-md-6" id="divtxtTaxes_SurchargeRate">
                            <div class="padBot5" style="display: block;">
                                Surcharge Rate
                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxTextBox ID="txtTaxes_SurchargeRate" ClientInstanceName="ctxtTaxes_SurchargeRate"
                                    runat="server" Width="100%">
                                    <%--<ClientSideEvents KeyPress="function(s,e){ return CheckDecimal('Popup_Empcitys_txtTaxes_SurchargeRate_I',event);}" />--%>
                                    <MaskSettings Mask="<0..99999g>.<0..99g>" />
                                </dxe:ASPxTextBox>

                                <span id="MandatorySurchargeRate" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                            </div>
                        </div>
                        
                        <div class="col-md-6">
                            <div class="padBot5" style="display: block;">
                                <span>Main Account</span><span style="text-align: left; float: left; font-size: medium; color: Red; width: 8px;">*</span>
                            </div>
                            <div class="Left_Content">
                                <%--<asp:TextBox ID="txtTaxRates_MainAccount" Width="100%" runat="server" onkeyup="FunCallAjaxList(this,event,'Digital');"></asp:TextBox>
                                <asp:TextBox ID="txtTaxRates_MainAccount_hidden" runat="server" Width="100px" Style="display: none"></asp:TextBox>--%>
                                <%--onmouseout="onBlrTXT();"   --%>

                                <asp:ListBox ID="lstTaxRates_MainAccount" CssClass="chsn" runat="server" Font-Size="12px" Width="100%" data-placeholder="Select..." onchange="changeFunc();"></asp:ListBox>
                                <span id="MandatorylstTaxRates_MainAccount" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                <asp:HiddenField ID="hndTaxRates_MainAccount_hidden" runat="server" />
                            </div>
                        </div><div class="clear"></div>
                        <div class="col-md-6" id="divtxtTaxRates_SubAccount" style="display: none;">
                            <div class="padBot5" style="display: block; height: auto;">
                                Sub Account
                            </div>
                            <div class="Left_Content">
                                <%-- <asp:TextBox ID="txtTaxRates_SubAccount" Width="176px" runat="server" onkeyup="FunCallAjaxList2(this,event,'Digital')"></asp:TextBox>
                                <asp:TextBox ID="txtTaxRates_SubAccount_hidden" runat="server" Width="100px" Style="display: none"></asp:TextBox>--%>

                                <asp:ListBox ID="lstTaxRates_SubAccount" CssClass="chsn" runat="server" Font-Size="12px" Width="100%" data-placeholder="Select..." onchange="changeSubFunc();"></asp:ListBox>
                                <asp:HiddenField ID="hndTaxRates_SubAccount_hidden" runat="server" />
                            </div>
                        </div>
                         <div class="col-md-6" id="divtxtTaxRates_Exempted" >
                            <div class="padBot5" style="display: block; height: auto;">
                                Exempted
                            </div>
                            <div class="Left_Content">
                                <%-- <asp:TextBox ID="txtTaxRates_SubAccount" Width="176px" runat="server" onkeyup="FunCallAjaxList2(this,event,'Digital')"></asp:TextBox>
                                <asp:TextBox ID="txtTaxRates_SubAccount_hidden" runat="server" Width="100px" Style="display: none"></asp:TextBox>--%>

                                <dxe:ASPxRadioButtonList ID="rdpExempted" runat="server" ClientInstanceName="crdpExempted" ValueType="System.String" RepeatDirection="Horizontal">
                                    <Items>
                                        <dxe:ListEditItem Text="Yes" Value="1"/>
                                         <dxe:ListEditItem Text="No" Value="0"/>
                                        
                                        
                                    </Items>
                                </dxe:ASPxRadioButtonList>
                            </div>
                        </div>
                        <div style="clear: both"></div>
                    </div>

                    <div class="ContentDiv clearfix">
                        <div style="display: none">
                            <div style="height: 20px; width: 280px; background-color: Gray; padding-left: 120px;">
                                <h5>Static Code</h5>
                            </div>
                            <div style="height: 20px; width: 130px; padding-left: 70px; background-color: Gray; float: left;">
                                Exchange
                            </div>
                            <div style="height: 20px; width: 200px; background-color: Gray; text-align: left;">
                                Value
                            </div>
                            <div class="ScrollDiv">
                                <div class="cityDiv" style="padding-top: 5px;">
                                    NSE Code
                                </div>
                                <div style="padding-top: 5px;">
                                    <dxe:ASPxTextBox ID="txtNseCode" ClientInstanceName="ctxtNseCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    BSE Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtBseCode" ClientInstanceName="ctxtBseCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    MCX Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtMcxCode" ClientInstanceName="ctxtMcxCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    MCXSX Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtMcsxCode" ClientInstanceName="ctxtMcsxCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    NCDEX Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtNcdexCode" ClientInstanceName="ctxtNcdexCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    CDSL Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtCdslCode" ClientInstanceName="ctxtCdslCode" CssClass="cityTextbox"
                                        runat="server">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    NSDL Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtNsdlCode" ClientInstanceName="ctxtNsdlCode" CssClass="cityTextbox"
                                        runat="server">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    NDML Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtNdmlCode" ClientInstanceName="ctxtNdmlCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    CVL Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtCvlCode" ClientInstanceName="ctxtCvlCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                                <br style="clear: both;" />
                                <div class="cityDiv">
                                    DOTEX Code
                                </div>
                                <div>
                                    <dxe:ASPxTextBox ID="txtDotexCode" ClientInstanceName="ctxtDotexCode" runat="server"
                                        CssClass="cityTextbox">
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                        </div>
                        <div class="clearfix" style="padding-left: 15px; padding-top: 15px">

                            <dxe:ASPxButton ID="btnSave_citys" ClientInstanceName="cbtnSave_citys" runat="server" AutoPostBack="false"
                                Text="Save" CssClass="btn btn-primary">
                                <ClientSideEvents Click="function (s, e) {btnSave_citys();}" />
                            </dxe:ASPxButton>


                            <dxe:ASPxButton ID="btnCancel_citys" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger">
                                <ClientSideEvents Click="function (s, e) {fn_btnCancel();}" />
                            </dxe:ASPxButton>


                        </div>
                    </div>

                    <%-- </div>--%>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
    <div class="HiddenFieldArea" style="display: none;">
        <asp:HiddenField runat="server" ID="hiddenedit" />
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:HiddenField ID="hdnCityId" runat="server" />
    </div>
</asp:Content>

