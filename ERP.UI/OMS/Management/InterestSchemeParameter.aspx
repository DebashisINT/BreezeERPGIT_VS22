<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    Inherits="ERP.OMS.Management.Management_InterestSchemeParameter" CodeBehind="InterestSchemeParameter.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxpc" %>--%>
    <style type="text/css">
        .radioButtonList {
            list-style: none;
            margin: 0;
            padding: 4px;
        }

            .radioButtonList.horizontal li {
                display: inline;
                padding: 4px;
            }

            .radioButtonList label {
                display: inline;
                padding: 4px;
            }
        /* Big box with list of options */
        #ajax_listOfOptions {
            position: absolute; /* Never change this one */
            width: 50px; /* Width of box */
            height: auto; /* Height of box */
            overflow: auto; /* Scrolling features */
            border: 1px solid Blue; /* Blue border */
            background-color: #FFF; /* White background color */
            text-align: left;
            font-size: 0.9em;
            z-index: 32761;
        }

            #ajax_listOfOptions div { /* General rule for both .optionDiv and .optionDivSelected */
                margin: 1px;
                padding: 1px;
                cursor: pointer;
                font-size: 0.9em;
            }

            #ajax_listOfOptions .optionDiv { /* Div for each item in list */
            }

            #ajax_listOfOptions .optionDivSelected { /* Selected item in the list */
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

        #txtIssuingBank {
            z-index: 10000;
        }

        .bubblewrap {
            list-style-type: none;
            margin: 0;
            padding: 0;
        }

            .bubblewrap li {
                display: inline;
                width: 65px;
                height: 60px;
            }

                .bubblewrap li img {
                    width: 30px; /* width of each image.*/
                    height: 35px; /* height of each image.*/
                    border: 0;
                    margin-right: 12px; /*spacing between each image*/
                    -webkit-transition: -webkit-transform 0.1s ease-in; /*animate transform property */
                    -o-transition: -o-transform 0.1s ease-in; /*animate transform property in Opera */
                }

                    .bubblewrap li img:hover {
                        -moz-transform: scale(1.8); /*scale up image 1.8x*/
                        -webkit-transform: scale(1.8);
                        -o-transform: scale(1.8);
    </style>

    <script type="text/javascript" src="/assests/js/init.js"></script>
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />
    <script type="text/javascript" src="/assests/js/ajaxList_wofolder.js"></script>
    
    <script type="text/javascript" src="/assests/js/GenericJScript.js"></script>
    <%--   <script type="text/javascript">
	   
           function CallSubAccount1(obj1,obj2,obj3)
        {
           try{
         var   mainAcType =((GetValue("txtMainAc_hidden")).split('~')[1]).toString().toLowerCase();
          alert(mainAcType);
          if(mainAcType=="custom")
          {
           
           var strQuery_Table = "Master_SubAccount";
           var strQuery_FieldName="top 10 RTrim(LTrim(RTrim(LTrim(SubAccount_Name)) +'-['+Rtrim(Ltrim (SubAccount_Code))+']')) as Text, SubAccount_Code as Value" ;
           var strQuery_WhereClause = " (SubAccount_Name like (\'%RequestLetter%\') or SubAccount_Code like (\'%RequestLetter%\'))";
           strQuery_WhereClause += " AND SubAccount_MainAcReferenceID='BADC002'"
           var strQuery_OrderBy='';
           var strQuery_GroupBy='';
            var CombinedQuery=new String(strQuery_Table+"$"+strQuery_FieldName+"$"+strQuery_WhereClause+"$"+strQuery_OrderBy+"$"+strQuery_GroupBy);
           ajax_showOptions(obj1,obj2,obj3,replaceChars(CombinedQuery),'Main');
          }
          else if (mainAcType=="brokers" || mainAcType=="customers" || mainAcType=="employees" || mainAcType=="creditors" || mainAcType=="debtors" || mainAcType=="sub broker" || mainAcType=="franchisee" || mainAcType=="agent" || mainAcType=="data vendor" || mainAcType=="relationship partner" || mainAcType=="relationship manager" || mainAcType=="vendor")
             {
                 var strQuery_Table = "tbl_master_contact";
                 var strQuery_FieldName="cnt_firstName+' '+ISNULL(cnt_middleName,'')+ ' '+cnt_lastName as TEXT, cnt_internalId +"~"+SubAccount_ReferenceID as value"
                 var strQuery_WhereClause = " (cnt_firstName like (\'%RequestLetter%\') or cnt_middleName like (\'%RequestLetter%\') or cnt_lastName like (\'%RequestLetter%\'))";
                 var strQuery_OrderBy='';
                 var strQuery_GroupBy='';
                 var CombinedQuery=new String(strQuery_Table+"$"+strQuery_FieldName+"$"+strQuery_WhereClause+"$"+strQuery_OrderBy+"$"+strQuery_GroupBy);
                 ajax_showOptions(obj1,obj2,obj3,replaceChars(CombinedQuery),'Main');
             }
             }
             catch(ex)
             {}
        }
       
       
      
         function MainAcType(v)
        {
        // alert(v.value);
        }
	   </script>--%>
    <script type="text/javascript">
        var IsPostInAc = 0, IsPostSameHead = 1;
        var serviceTaxType = 'N';
        /*  $(document).ready(function(){
             $("#rlstServTax").change(function(){
             //  alert("xx");
             try{
             var rb = document.getElementById("rlstServTax");
              var inputs = rb.getElementsByTagName('input');
              var flags = false;
              var selecteds;
               
              for (var i = 0; i < inputs.length; i++) {
                  if (inputs[i].checked) {
                      selecteds = inputs[i];
                      flags = true;
                      break;
                  }
              }
                 serviceTaxType= selecteds.value;
               // alert(selecteds.value);
                }
                catch(ex)
                {
                  alert(ex);
                }
             })
          })  */


        function ShowPopUp(mode, types) {
            //   alert("0"+GetValue("txtInterestScheme_hidden"));

            try {
                if (mode != 'not') {
                    document.getElementById("hdnMode").value = mode;
                }
                document.getElementById("divOverlapping").style.display = 'block';
                if (types == 'M') {
                    document.getElementById("divPopUp").style.display = 'block';
                }
                else {
                    document.getElementById("divDetailsPopUp").style.display = 'block';
                }
            }
            catch (ex) {
                // alert(ex);
            }
            document.getElementById("lblInterestSchemeCode").innerHTML = document.getElementById("txtInterestScheme").value;
        }

        function ClosePopUp() {
            document.getElementById("divOverlapping").style.display = 'none';
            document.getElementById("divPopUp").style.display = 'none';
            document.getElementById("divDetailsPopUp").style.display = 'none';
            __doPostBack();
        }
        function DeleteConfirmation() {
            var i = window.confirm("Are you sure to delete this record?");
            if (i == true) {
                var j = window.confirm("Are you sure to delete this record?");
                if (j == true) {
                    var k = window.confirm("Are you sure to delete this record?");
                    if (k == true) {
                        return true;
                    }
                }
            }
            return false;
        }
        function EnableDisableControl(li) {

            if (li == 1) {
                // alert(li);
                document.getElementById("trPostHead").disabled = true;
            }
            else {
                // alert(li);
                document.getElementById("trPostHead").disabled = false;
            }
        }

        function replaceChars(entry) {
            out = "+"; // replace this
            add = "--"; // with this
            temp = "" + entry; // temporary holder

            while (temp.indexOf(out) > -1) {
                pos = temp.indexOf(out);
                temp = "" + (temp.substring(0, pos) + add +
                temp.substring((pos + out.length), temp.length));
            }
            return temp;

        }
        function height() {
            if (document.body.scrollHeight >= 500)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '600px';
            window.frameElement.Width = document.body.scrollWidth;
        }

        function keyVal(obj) {
            // alert("*******");
            var MainAccoutValue = GetValue("txtMainAc_hidden");
            if (((GetValue("txtMainAc_hidden")).split('~')[1]).toString().toLowerCase().trim() == "none" || ((GetValue("txtMainAc_hidden")).split('~')[1]).toString().toLowerCase().trim() == "") {
                document.getElementById("hdnMainAcType").value = "N";
            }
            else {
                document.getElementById("hdnMainAcType").value = "Y";
            }
            if (((GetValue("txtTDSMainAC_hidden")).split('~')[1]).toString().toLowerCase().trim() == "none" || ((GetValue("txtTDSMainAC_hidden")).split('~')[1]).toString().toLowerCase().trim() == "") {
                document.getElementById("hdnTDSMainAcType").value = "N";
            }
            else {
                document.getElementById("hdnTDSMainAcType").value = "Y";
            }
            if (((GetValue("txtSTMainAc_hidden")).split('~')[1]).toString().toLowerCase().trim() == "none" || ((GetValue("txtSTMainAc_hidden")).split('~')[1]).toString().toLowerCase().trim() == "") {
                document.getElementById("hdnSTMainAcType").value = "N";
            }
            else {
                document.getElementById("hdnSTMainAcType").value = "Y";
            }

            // alert(document.getElementById("hdnMainAcType").value);
            //  alert(document.getElementById("hdnSTMainAcType").value);
            // alert(document.getElementById("hdnTDSMainAcType").value);

        }
        function Schemeblur() {

            // alert(GetValue("txtInterestScheme_hidden"));
        }
        function CallInterestScheme(obj1, obj2, obj3) {
            var strQuery_Table = "Master_IntScheme";
            var strQuery_FieldName = "top 10 IntScheme_Name +' ['+IntScheme_Code+']' as Text ,IntScheme_Code as Value";
            var strQuery_WhereClause = " (IntScheme_Name like (\'%RequestLetter%\') or IntScheme_Code like (\'%RequestLetter%\'))";
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');


        }
        function CallMainAccount(obj1, obj2, obj3) {
            var strQuery_Table = "Master_MainAccount";
            var strQuery_FieldName = " top 10 MainAccount_Name+\' [ \'+rtrim(ltrim(MainAccount_AccountCode))+\' ]\' as MainAccount_Name1,cast(MainAccount_ReferenceID as varchar)+\'~\'+MainAccount_SubLedgerType+\'~MAINAC~\'+MainAccount_AccountType +'*'+MainAccount_AccountCode as MainAccount_ReferenceID";
            //  var strQuery_WhereClause = " (MainAccount_Name like (\'%RequestLetter%\') or MainAccount_AccountCode like (\'%RequestLetter%\')) and isnull(MainAccount_BankCashType,'abc') not in('Cash','Bank')";
            var strQuery_WhereClause = " (MainAccount_Name like (\'%RequestLetter%\') or MainAccount_AccountCode like (\'%RequestLetter%\'))";
            // strQuery_WhereClause +=" AND MainAccount_AccountType in ('Asset','Liability') and isnull(MainAccount_BankCashType,'') not in ('Bank','Cash') and left(isnull(MainAccount_SubLedgerType,''),4)<>'PROD' "
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');
            document.getElementById("txtSubAc").value = "";
            document.getElementById("txtSubAc_hidden").value = "";
        }
        function CallSTMainAccount(obj1, obj2, obj3) {
            var SelectedMainAcId = GetValue("txtMainAc_hidden").split('~')[0];
            // alert (SelectedMainAcId);
            var strQuery_Table = "Master_MainAccount";
            var strQuery_FieldName = " top 10 MainAccount_Name+\' [ \'+rtrim(ltrim(MainAccount_AccountCode))+\' ]\' as MainAccount_Name1,cast(MainAccount_ReferenceID as varchar)+\'~\'+MainAccount_SubLedgerType+\'~MAINAC~\'+MainAccount_AccountType +'*'+MainAccount_AccountCode as MainAccount_ReferenceID";
            //  var strQuery_WhereClause = " (MainAccount_Name like (\'%RequestLetter%\') or MainAccount_AccountCode like (\'%RequestLetter%\')) and isnull(MainAccount_BankCashType,'abc') not in('Cash','Bank')";
            var strQuery_WhereClause = " (MainAccount_Name like (\'%RequestLetter%\') or MainAccount_AccountCode like (\'%RequestLetter%\'))";
            // strQuery_WhereClause +=" AND MainAccount_AccountType in ('Asset','Liability') and isnull(MainAccount_BankCashType,'') not in ('Bank','Cash') and left(isnull(MainAccount_SubLedgerType,''),4)<>'PROD' ";
            //             if (SelectedMainAcId !="")
            //             {
            //               strQuery_WhereClause +=" AND mainAccount_ReferenceId <>"+SelectedMainAcId;
            //             }
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');
            document.getElementById("txtSTSubAc").value = "";
            document.getElementById("txtSTSubAc_hidden").value = "";
        }
        function CallTDSMainAccount(obj1, obj2, obj3) {
            var strQuery_Table = "Master_MainAccount";
            var strQuery_FieldName = " top 10 MainAccount_Name+\' [ \'+rtrim(ltrim(MainAccount_AccountCode))+\' ]\' as MainAccount_Name1,cast(MainAccount_ReferenceID as varchar)+\'~\'+MainAccount_SubLedgerType+\'~MAINAC~\'+MainAccount_AccountType +'*'+MainAccount_AccountCode as MainAccount_ReferenceID";
            //  var strQuery_WhereClause = " (MainAccount_Name like (\'%RequestLetter%\') or MainAccount_AccountCode like (\'%RequestLetter%\')) and isnull(MainAccount_BankCashType,'abc') not in('Cash','Bank')";
            var strQuery_WhereClause = " (MainAccount_Name like (\'%RequestLetter%\') or MainAccount_AccountCode like (\'%RequestLetter%\'))";
            // strQuery_WhereClause +=" AND MainAccount_AccountType in ('Asset','Liability') and isnull(MainAccount_BankCashType,'') not in ('Bank','Cash') and left(isnull(MainAccount_SubLedgerType,''),4)<>'PROD' "
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');
            document.getElementById("txtTDSSubAC").value = "";
            document.getElementById("txtTDSSubAC_hidden").value = "";
        }
        function CallSubAccount(obj1, obj2, obj3) {
            var mainAcTypes = ((GetValue("txtMainAc_hidden")).split('~')[1]).toString().toLowerCase();
            // alert(mainAcTypes) ;
            if (mainAcTypes == "custom") {
                var subreferenceAcId = GetValue("txtMainAc_hidden").split('*')[1];
                var strQuery_Table = "Master_SubAccount";
                var strQuery_FieldName = "top 10 RTrim(LTrim(RTrim(LTrim(SubAccount_Name)) +'-['+Rtrim(Ltrim (SubAccount_Code))+']')) as Text, SubAccount_Code +'*'+SubAccount_Code as Value";
                var strQuery_WhereClause = " (SubAccount_Name like (\'%RequestLetter%\') or SubAccount_Code like (\'%RequestLetter%\'))";
                // strQuery_WhereClause += " AND SubAccount_MainAcReferenceID='BADC002'"
                strQuery_WhereClause += " AND SubAccount_MainAcReferenceID=" + "'" + subreferenceAcId + "'"
                var strQuery_OrderBy = '';
                var strQuery_GroupBy = '';
                var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
                ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');
            }
            else if (mainAcTypes == "brokers" || mainAcTypes == "customers" || mainAcTypes == "employees" || mainAcTypes == "creditors" || mainAcTypes == "debtors" || mainAcTypes == "sub broker" || mainAcTypes == "franchisee" || mainAcTypes == "agent" || mainAcTypes == "data vendor" || mainAcTypes == "relationship partner" || mainAcTypes == "relationship manager" || mainAcTypes == "vendor") {
                var strQuery_Table = "tbl_master_contact";
                var strQuery_FieldName = "cnt_firstName+' '+ISNULL(cnt_middleName,'')+ ' '+cnt_lastName+' '+cnt_UCC as TEXT, cnt_internalId+'*'+cnt_internaliD as value"
                var strQuery_WhereClause = " (cnt_firstName like (\'%RequestLetter%\') or cnt_middleName like (\'%RequestLetter%\') or cnt_lastName like (\'%RequestLetter%\') or cnt_UCC like (\'%RequestLetter%\'))";
                var strQuery_OrderBy = '';
                var strQuery_GroupBy = '';
                var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
                ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');
            }
            else if (mainAcTypes == "cdsl clients" || mainAcTypes == "master_cdslclients") {
                var strQuery_Table = "Master_CdslClients";
                var strQuery_FieldName = "CdslClients_BenAccountNumber,CdslClients_FirstHolderName+CdslClients_FirstHolderMiddleName+CdslClients_FirstHolderLastName as Text" + "CdslClients_BenAccountNumber +'*'+CdslClients_BenAccountNumber"
                var strQuery_WhereClause = " (CdslClients_FirstHolderName like (\'%RequestLetter%\') or CdslClients_BenAccountNumber like (\'%RequestLetter%\') or CdslClients_FirstHolderLastName like (\'%RequestLetter%\'))";
                var strQuery_OrderBy = '';
                var strQuery_GroupBy = '';
                var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
                ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');
            }
            else if (mainAcTypes == "nsdl clients" || mainAcTypes == "master_nsdlclients") {
                var strQuery_Table = "Master_NsdlClients";
                var strQuery_FieldName = "NsdlClients_BenFirstHolderName+ '['  + CAST (NsdlClients_BenAccountID as Varchar(100))+']'as Text, 'aa*'+   CAST (NsdlClients_BenAccountID as Varchar(50)) as Value"
                var strQuery_WhereClause = " (NsdlClients_BenFirstHolderName like (\'%RequestLetter%\') or NsdlClients_BenFirstHolderName like (\'%RequestLetter%\') or NsdlClients_BenFirstHolderName like (\'%RequestLetter%\'))";
                var strQuery_OrderBy = '';
                var strQuery_GroupBy = '';
                var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
                ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');

            }

        }

        function CallSTSubAccount(obj1, obj2, obj3) {
            try {
                var mainAcType = ((GetValue("txtSTMainAc_hidden")).split('~')[1]).toString().toLowerCase();
                var mainAcSelectedId = GetValue("txtSubAc_hidden").split('*')[1];

                if (mainAcType == "custom") {
                    var subreferenceAcId = GetValue("txtSTMainAc_hidden").split('*')[1];
                    var strQuery_Table = "Master_SubAccount";
                    var strQuery_FieldName = "top 10 RTrim(LTrim(RTrim(LTrim(SubAccount_Name)) +'-['+Rtrim(Ltrim (SubAccount_Code))+']')) as Text, SubAccount_Code +'*'+SubAccount_Code as Value";
                    var strQuery_WhereClause = " (SubAccount_Name like (\'%RequestLetter%\') or SubAccount_Code like (\'%RequestLetter%\'))";
                    //  strQuery_WhereClause += " AND SubAccount_MainAcReferenceID='BADC002'"
                    strQuery_WhereClause += " AND SubAccount_MainAcReferenceID=" + "'" + subreferenceAcId + "'"
                    /*  if(mainAcSelectedId=="")
                      {
                        strQuery_WhereClause +=" AND SubAccount_Code<>"+mainAcSelectedId;
                      }  */
                    var strQuery_OrderBy = '';
                    var strQuery_GroupBy = '';
                    var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
                    ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');
                }
                else if (mainAcType == "brokers" || mainAcType == "customers" || mainAcType == "employees" || mainAcType == "creditors" || mainAcType == "debtors" || mainAcType == "sub broker" || mainAcType == "franchisee" || mainAcType == "agent" || mainAcType == "data vendor" || mainAcType == "relationship partner" || mainAcType == "relationship manager" || mainAcType == "vendor") {
                    var strQuery_Table = "tbl_master_contact";
                    var strQuery_FieldName = "cnt_firstName+' '+ISNULL(cnt_middleName,'')+ ' '+cnt_lastName+' '+cnt_UCC as TEXT, cnt_internalId+'*'+cnt_internaliD as value"
                    var strQuery_WhereClause = " (cnt_firstName like (\'%RequestLetter%\') or cnt_middleName like (\'%RequestLetter%\') or cnt_lastName like (\'%RequestLetter%\') or cnt_UCC like (\'%RequestLetter%\'))";
                    if (mainAcSelectedId == "") {
                        strQuery_WhereClause += "  AND cnt_internaliD<>" + mainAcSelectedId;
                    }
                    var strQuery_OrderBy = '';
                    var strQuery_GroupBy = '';
                    var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
                    ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');
                }
                else if (mainAcType == "cdsl clients" || mainAcType == "master_cdslclients") {
                    var strQuery_Table = "Master_CdslClients";
                    var strQuery_FieldName = "CdslClients_BenAccountNumber,CdslClients_FirstHolderName+CdslClients_FirstHolderMiddleName+CdslClients_FirstHolderLastName as Text" + "CdslClients_BenAccountNumber +'*'+CdslClients_BenAccountNumber"
                    var strQuery_WhereClause = " (CdslClients_FirstHolderName like (\'%RequestLetter%\') or CdslClients_BenAccountNumber like (\'%RequestLetter%\') or CdslClients_FirstHolderLastName like (\'%RequestLetter%\'))";
                    if (mainAcSelectedId == "") {
                        strQuery_WhereClause += "  AND CdslClients_BenAccountNumber<>" + mainAcSelectedId;
                    }
                    var strQuery_OrderBy = '';
                    var strQuery_GroupBy = '';
                    var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
                    ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');
                }
                else if (mainAcType == "nsdl clients" || mainAcType == "master_nsdlclients") {
                    var strQuery_Table = "Master_NsdlClients";
                    var strQuery_FieldName = "NsdlClients_BenFirstHolderName+ '['  + CAST (NsdlClients_BenAccountID as Varchar(100))+']'as Text, 'aa*'+   CAST (NsdlClients_BenAccountID as Varchar(50)) as Value"
                    var strQuery_WhereClause = " (NsdlClients_BenFirstHolderName like (\'%RequestLetter%\') or NsdlClients_BenFirstHolderName like (\'%RequestLetter%\') or NsdlClients_BenFirstHolderName like (\'%RequestLetter%\'))";
                    if (mainAcSelectedId == "") {
                        strQuery_WhereClause += "  AND NsdlClients_BenAccountID<>" + mainAcSelectedId;
                    }
                    var strQuery_OrderBy = '';
                    var strQuery_GroupBy = '';
                    var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
                    ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');

                }
            }
            catch (ex) {
                // alert(ex);
            }
        }
        function CallTDSSubAccount(obj1, obj2, obj3) {
            var mainAcTypes = ((GetValue("txtTDSMainAC_hidden")).split('~')[1]).toString().toLowerCase();
            // alert(mainAcTypes) ;
            if (mainAcTypes == "custom") {
                var subreferenceAcId = GetValue("txtMainAc_hidden").split('*')[1];
                var strQuery_Table = "Master_SubAccount";
                var strQuery_FieldName = "top 10 RTrim(LTrim(RTrim(LTrim(SubAccount_Name)) +'-['+Rtrim(Ltrim (SubAccount_Code))+']')) as Text, SubAccount_Code +'*'+SubAccount_Code as Value";
                var strQuery_WhereClause = " (SubAccount_Name like (\'%RequestLetter%\') or SubAccount_Code like (\'%RequestLetter%\'))";
                // strQuery_WhereClause += " AND SubAccount_MainAcReferenceID='BADC002'"
                strQuery_WhereClause += "  AND SubAccount_MainAcReferenceID=" + "'" + subreferenceAcId + "'"
                var strQuery_OrderBy = '';
                var strQuery_GroupBy = '';
                var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
                ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');
            }
            else if (mainAcTypes == "brokers" || mainAcTypes == "customers" || mainAcTypes == "employees" || mainAcTypes == "creditors" || mainAcTypes == "debtors" || mainAcTypes == "sub broker" || mainAcTypes == "franchisee" || mainAcTypes == "agent" || mainAcTypes == "data vendor" || mainAcTypes == "relationship partner" || mainAcTypes == "relationship manager" || mainAcTypes == "vendor") {
                var strQuery_Table = "tbl_master_contact";
                var strQuery_FieldName = "cnt_firstName+' '+ISNULL(cnt_middleName,'')+ ' '+cnt_lastName+' '+cnt_UCC as TEXT, cnt_internalId+'*'+cnt_internaliD as value"
                var strQuery_WhereClause = " (cnt_firstName like (\'%RequestLetter%\') or cnt_middleName like (\'%RequestLetter%\') or cnt_lastName like (\'%RequestLetter%\') or cnt_UCC like (\'%RequestLetter%\'))";
                var strQuery_OrderBy = '';
                var strQuery_GroupBy = '';
                var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
                ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');
            }
            else if (mainAcTypes == "cdsl clients" || mainAcTypes == "master_cdslclients") {
                var strQuery_Table = "Master_CdslClients";
                var strQuery_FieldName = "CdslClients_BenAccountNumber,CdslClients_FirstHolderName+CdslClients_FirstHolderMiddleName+CdslClients_FirstHolderLastName as Text" + "CdslClients_BenAccountNumber +'*'+CdslClients_BenAccountNumber"
                var strQuery_WhereClause = " (CdslClients_FirstHolderName like (\'%RequestLetter%\') or CdslClients_BenAccountNumber like (\'%RequestLetter%\') or CdslClients_FirstHolderLastName like (\'%RequestLetter%\'))";
                var strQuery_OrderBy = '';
                var strQuery_GroupBy = '';
                var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
                ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');
            }
            else if (mainAcTypes == "nsdl clients" || mainAcTypes == "master_nsdlclients") {
                var strQuery_Table = "Master_NsdlClients";
                var strQuery_FieldName = "NsdlClients_BenFirstHolderName+ '['  + CAST (NsdlClients_BenAccountID as Varchar(100))+']'as Text, 'aa*'+   CAST (NsdlClients_BenAccountID as Varchar(50)) as Value"
                var strQuery_WhereClause = " (NsdlClients_BenFirstHolderName like (\'%RequestLetter%\') or NsdlClients_BenFirstHolderName like (\'%RequestLetter%\') or NsdlClients_BenFirstHolderName like (\'%RequestLetter%\'))";
                var strQuery_OrderBy = '';
                var strQuery_GroupBy = '';
                var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
                ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');

            }

        }
        function ValidateFileds() {
            var ValMessage = "";
            //           if(document.getElementById("txtDateFromDetails").value=="")
            //           {
            //               ValMessage+='Enter Date From';
            //           }
            if (document.getElementById("txtMainAc_hidden").value == "" || document.getElementById("txtMainAc_hidden").value == null || document.getElementById("txtMainAc_hidden").value == "No Record Found") {
                ValMessage += '<br/>' + 'Enter Main Account Name';
            }
            if (document.getElementById("txtSubAc_hidden").value == "" || document.getElementById("txtSubAc_hidden").value == null || document.getElementById("txtSubAc_hidden").value == "No Record Found") {
                ValMessage += '<br/>' + 'Enter Sub Account Name';
            }

            if ((document.getElementById("txtSTMainAc_hidden").value == "" || document.getElementById("txtSTMainAc_hidden").value == null || document.getElementById("txtSTMainAc_hidden").value == "No Record Found") && serviceTaxType != 'N') {
                ValMessage += '<br/>' + 'Enter Service Tax Main Account Name';
            }
            if ((document.getElementById("txtSTSubAc_hidden").value == "" || document.getElementById("txtSTSubAc_hidden").value == null || document.getElementById("txtSTSubAc_hidden").value == "No Record Found") && serviceTaxType != 'N') {
                ValMessage += '<br/>' + 'Enter Service Tax Sub Account Name';
            }
            if ((document.getElementById("txtTDSMainAC_hidden").value == "" || document.getElementById("txtTDSMainAC_hidden").value == null || document.getElementById("txtTDSMainAC_hidden").value == "No Record Found") && document.getElementById("txtTDSRate").value != "") {
                ValMessage += '<br/>' + 'Enter TDS Main Account Name';
            }
            if ((document.getElementById("txtTDSSubAC_hidden").value == "" || document.getElementById("txtTDSSubAC_hidden").value == null || document.getElementById("txtTDSSubAC_hidden").value == "No Record Found") && document.getElementById("txtTDSRate").value != "") {
                ValMessage += '<br/>' + 'Enter TDS Sub Account Name';
            }
            if (ValMessage != "") {
                // alert(ValMessage);
                return false;
            }
            else {
                return true;
            }

        }
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            // alert(charCode);
            if (charCode == 46) {
                var inputValue = $("#inputfield").val()
                if (inputValue.indexOf('.') < 1) {
                    return true;
                }
                return false;
            }
            if (charCode == 44) {
                return true;
            }
            if ((charCode > 31 && (charCode < 48 || charCode > 57))) {
                return false;
            }
            return true;
        }
        function validate() {
            var vv = "0";
            // alert("ABCD-1234");
            try {
                if (document.getElementById("txtExAmount").value == "") {
                    document.getElementById("spnExAmount").innerHTML = "Enter Exemption Amount";
                    vv = "1";
                }
                else {
                    document.getElementById("spnExAmount").innerHTML = "";
                }

                if (document.getElementById("txtMainAc").value == "" || document.getElementById("txtMainAc_hidden").value == "") {
                    document.getElementById("spnMainAc").innerHTML = "Enter  Main Account Name";
                    vv = "1";
                }
                else {
                    document.getElementById("spnMainAc").innerHTML = "";
                }
                if ((document.getElementById("txtSubAc").value == "" || document.getElementById("txtSubAc_hidden").value == "") && document.getElementById("hdnMainAcType").value != "N") {
                    document.getElementById("spnSubAc").innerHTML = "Enter  Sub Account Name";
                    vv = "1";
                }
                else {
                    document.getElementById("spnSubAc").innerHTML = "";
                }
                if (serviceTaxType != 'N') {
                    //  alert("4444");
                    if (document.getElementById("txtSTMainAc").value == "" || document.getElementById("txtSTMainAc_hidden").value == "") {
                        document.getElementById("spnSTMainAc").innerHTML = "Enter Service Tax Main Account Name";
                        vv = "1";
                    }
                    else {
                        document.getElementById("spnSTMainAc").innerHTML = "";
                    }
                    if ((document.getElementById("txtSTSubAc").value == "" || document.getElementById("txtSTSubAc_hidden").value == "") && document.getElementById("hdnSTMainAcType").value != "N") {
                        document.getElementById("spnSTSubAc").innerHTML = "Enter Service Tax Sub Account Name";
                        vv = "1";
                    }
                    else {
                        document.getElementById("spnSTSubAc").innerHTML = "";
                    }
                }
                else {
                    document.getElementById("spnSTMainAc").innerHTML = "";
                    document.getElementById("spnSTSubAc").innerHTML = "";
                }
                if (document.getElementById("txtTDSRate").value != "") {
                    if (document.getElementById("txtTDSMainAC").value == "" || document.getElementById("txtTDSMainAc_hidden").value == "") {
                        document.getElementById("spnTDSMainAc").innerHTML = "Enter TDS Main Account Name";
                        vv = "1";
                    }
                    else {
                        document.getElementById("spnTDSMainAc").innerHTML = "";
                    }
                    if ((document.getElementById("txtTDSSubAC").value == "" || document.getElementById("txtTDSSubAC_hidden").value == "") && document.getElementById("hdnTDSMainAcType").value != "N") {
                        document.getElementById("spnTDSSubAc").innerHTML = "Enter TDS Sub Account Name";
                        vv = "1";
                    }
                    else {
                        document.getElementById("spnTDSSubAc").innerHTML = "";
                    }
                }
                else {
                    document.getElementById("spnTDSMainAc").innerHTML = "";
                    document.getElementById("spnTDSSubAc").innerHTML = "";
                }
                //  alert(vv);
                if (vv == "1") {
                    return false;
                }
                else if (vv == "0") {
                    //  alert("re");
                    return true;
                }
            }
            catch (ex) {
                //   alert(ex+'/////'+ex.Message);
                return true;
            }

        }
        function changeState(v) {
            if (v == "2") {
                serviceTaxType = 'N'
            }
            else {
                serviceTaxType = 'Y'
            }
        }
        function PostSaveMessage(v) {
            if (v == '0') {
                alert('Data saved successfully');
            }
            else {
                alert('Already a configuration exists between this company and scheme code on or before this date');
                ShowPopUp('not', 'M');
            }
        }

    </script>
    <script type="text/javascript" language="javascript">
        FieldName = 'BtnSave';
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
        <col width="67%">
        <col width="33%">
        <tr>
            <td colspan="2" align="center" style="font-size: medium; font-weight: bold; padding: 5px 0 5px 0;">Interest Scheme Configuration
            </td>
        </tr>
        <tr>
            <td align="left">
                <b>Company :</b>
                <asp:Label ID="lblCompany" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <b>Interest Scheme :</b>
                <asp:TextBox ID="txtInterestScheme" runat="server" Width="270" onkeyup="CallInterestScheme(this,'GenericAjaxList',event)" onblur="Schemeblur();"></asp:TextBox>
                <asp:HiddenField ID="txtInterestScheme_hidden" runat="server" />

            </td>
            <td align="left" style="padding: 0 10px 0 -10px;">
                <asp:Label ID="lblBlankMessage" runat="server" ForeColor="red" Font-Bold="true"></asp:Label>
                <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" />
            </td>


        </tr>
        <tr>
            <td colspan="2">

                <asp:GridView ID="gvInterestSchemeParameter" runat="server" AllowPaging="true" AutoGenerateColumns="false" OnPageIndexChanged="gvInterestSchemeParameter_PageIndexChanged" OnPageIndexChanging="gvInterestSchemeParameter_PageIndexChanging" OnRowCommand="gvInterestSchemeParameter_RowCommand" Width="98%" EmptyDataText="No Record Founds" EmptyDataRowStyle-Font-Bold="true" EmptyDataRowStyle-VerticalAlign="Middle" EmptyDataRowStyle-HorizontalAlign="Center" HeaderStyle-BorderColor="Black" HeaderStyle-BackColor="#B7CEEC" RowStyle-BorderColor="Black">
                    <Columns>
                        <asp:TemplateField HeaderText="Date From">
                            <ItemTemplate>
                                <%--  <%#Eval("DateFrom")%>--%>
                                <asp:Label ID="lblFrom" runat="server" Text='<%#GetDateFormat(Eval("DateFrom"))%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date To">
                            <ItemTemplate>
                                <%--  <%#Eval("DateTo")%>--%>
                                <asp:Label ID="lblTo" runat="server" Text='<%#GetDateFormat(Eval("DateToVal"))%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lbtn" runat="server" Text="Add New" CommandName="newAdd" ForeColor="blue" Font-Underline="true"></asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnMore" runat="server" Text="Main Parameter Details......" CommandName="Info" CommandArgument='<%#Eval("IntRates_ID") %>'>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnDetails" runat="server" Text="Slab and rate details......" CommandName="details" CommandArgument='<%#Eval("IDAndCycle") %>'>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnDelete" runat="server" Text="Delete" OnClientClick="return DeleteConfirmation();" CommandName="Del" CommandArgument='<%#Eval("IdAndDateto") %>'>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                    <EmptyDataRowStyle HorizontalAlign="Left" VerticalAlign="Middle"></EmptyDataRowStyle>
                    <EmptyDataTemplate>
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:Panel ID="pnlEmpty" runat="server">
                    <div style="font-size: small; font-weight: bold; padding: 5px 0 0 10px;">
                        No&nbsp;Record&nbsp;Found&nbsp;<asp:LinkButton ID="lbtn1" ForeColor="Blue" runat="server" Text="Click" CommandName="newadd" OnClick="Add_empty"></asp:LinkButton>&nbsp;here to add new details 
                    </div>
                </asp:Panel>
            </td>
        </tr>
    </table>

    <div id="divOverlapping" style="position: fixed; height: 100%; width: 100%; background-color: #000; top: 0px; left: 0px; opacity: 0.4; filter: alpha(opacity=40); z-index: 50; display: none;">
    </div>
    <div id="divPopUp" style="background-color: #B7CEEC; height: 320px; left: 11px; position: absolute; top: 10px; width: 970px; z-index: 75; display: none; padding-bottom: 5px;">
        <div style="background-color: #B7CEEC; height: 12px; font-size: small; color: White; padding-bottom: 27px; padding-left: 0px;">
            <div style="background-color: Black; color: White; font-weight: bold; font-size: medium; padding: 5px 0 5px 0;">
                Interest Scheme Parameter Form
                
                <img src="../windowfiles/close.gif" height="16px" alt="CLOSE" onclick="ClosePopUp();"
                    style="padding-left: 98%; margin: 0px;" />
            </div>
            <div style="background-color: #B7CEEC; color: Black; padding-left: 0px; background-color: White;">
                <table border="1" style="border-collapse: collapse; background-color: #B7CEEC;" width="100%">
                    <col width="12%">
                    <col width="38%">
                    <col width="12%">
                    <col width="38%">
                    <tr>
                        <td align="right" style="font-weight: bold;">Interest&nbsp;Scheme&nbsp;Code
                        </td>
                        <td align="left">
                            <asp:Label ID="lblInterestSchemeCode" runat="server"></asp:Label>
                        </td>
                        <td align="right" style="font-weight: bold;">Company 
                        </td>
                        <td align="left">
                            <asp:Label ID="lblCompanyIn" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="font-weight: bold;">Date&nbsp;From
                        </td>
                        <td align="left">
                            <dxe:ASPxDateEdit ID="txtDateFrom" runat="server" Width="320px" EditFormat="Custom"
                                EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" TabIndex="0" ClientInstanceName="ctxtDateFrom">
                                <ButtonStyle>
                                </ButtonStyle>

                            </dxe:ASPxDateEdit>
                        </td>
                        <td align="right" style="font-weight: bold;">Calculate&nbsp;On
                        </td>
                        <td align="left">
                            <asp:RadioButtonList ID="rbtnCalculateOn" runat="server" RepeatDirection="Horizontal" TabIndex="0" CssClass="radioButtonList">
                                <asp:ListItem Text="Opening" Value="O" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Closing" Value="C"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="font-weight: bold;">Calculation&nbsp;Cycle
                        </td>
                        <td align="left">
                            <asp:RadioButtonList ID="rlstCycle" runat="server" RepeatDirection="Horizontal" TabIndex="0" CssClass="radioButtonList">
                                <asp:ListItem Text="Weekly" Value="W"></asp:ListItem>
                                <asp:ListItem Text="Monthly" Value="M" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td align="right" style="font-weight: bold;">Days&nbsp;In&nbsp;Year
                        </td>
                        <td align="left">
                            <asp:RadioButtonList ID="rlstDays" runat="server" RepeatDirection="Horizontal" TabIndex="0" CssClass="radioButtonList">
                                <asp:ListItem Text="365" Value="365" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="366" Value="366"></asp:ListItem>
                                <asp:ListItem Text="360" Value="360"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="font-weight: bold;">Exemption&nbsp;Amount
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtExAmount" runat="server" TabIndex="0" onkeypress="return isNumber(event)" MaxLength="15"></asp:TextBox>
                            <span id="spnExAmount" style="color: Red;"></span>
                        </td>
                        <td align="right" style="font-weight: bold;">Balance&nbsp;Type
                        </td>
                        <td align="left">
                            <asp:RadioButtonList ID="rlstBalanceType" runat="server" RepeatDirection="Horizontal" TabIndex="0" CssClass="radioButtonList">
                                <asp:ListItem Text="Debit" Value="D" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Credit" Value="C"></asp:ListItem>
                                <asp:ListItem Text="Debit & Credit" Value="B"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>

                    <tr>
                        <td align="right" style="font-weight: bold;">Main&nbsp;Account
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtMainAc" runat="server" Width="320px" TabIndex="0" onkeyup="CallMainAccount(this,'GenericAjaxList',event);">
                            </asp:TextBox><br />
                            <span id="spnMainAc" style="color: Red;"></span>
                            <asp:HiddenField ID="txtMainAc_hidden" runat="server" />
                            <asp:HiddenField ID="hdnMainAcType" runat="server" />
                        </td>
                        <td align="right" style="font-weight: bold;">Sub&nbsp;Account
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtSubAc" runat="server" TabIndex="0" Width="320px" onkeyup="CallSubAccount(this,'GenericAjaxList',event)"></asp:TextBox>
                            <br />
                            <span id="spnSubAc" style="color: Red;"></span>
                            <asp:HiddenField ID="txtSubAc_hidden" runat="server" />

                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="font-weight: bold;">Service&nbsp;Tax
                        </td>
                        <td colspan="0">
                            <asp:RadioButtonList ID="rlstServTax" runat="server" RepeatDirection="Horizontal"
                                TabIndex="0" CssClass="radioButtonList">
                                <asp:ListItem Text="Inclusive" Value="I" onclick="changeState('0');"></asp:ListItem>
                                <asp:ListItem Text="Exclusive" Value="E" onclick="changeState('1');"></asp:ListItem>
                                <asp:ListItem Text="Not Applicable" Value="N" Selected="True" onclick="changeState('2');"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td align="right" style="font-weight: bold;">Narration
                        </td>
                        <td align="left" colspan="0">
                            <asp:TextBox ID="txtNarration" runat="server" TextMode="MultiLine" Width="316px" Rows="2" TabIndex="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="font-weight: bold;">Service&nbsp;Tax&nbsp;Main&nbsp;Account
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtSTMainAc" runat="server" TabIndex="0" Width="320px" onkeyup="CallSTMainAccount(this,'GenericAjaxList',event)"></asp:TextBox>
                            <br />
                            <span id="spnSTMainAc" style="color: Red;"></span>
                            <asp:HiddenField ID="txtSTMainAc_hidden" runat="server" />
                            <asp:HiddenField ID="hdnSTMainAcType" runat="server" />
                        </td>
                        <td align="right" style="font-weight: bold;">Service&nbsp;Tax&nbsp;Sub&nbsp;Account
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtSTSubAc" runat="server" TabIndex="0" Width="320px" onkeyup="CallSTSubAccount(this,'GenericAjaxList',event)"></asp:TextBox>
                            <span id="spnSTSubAc" style="color: Red;"></span>
                            <asp:HiddenField ID="txtSTSubAc_hidden" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="font-weight: bold;">TDS&nbsp;Rate
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtTDSRate" runat="server" TabIndex="0" MaxLength="12" onkeypress="return isNumber(event)"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="font-weight: bold;">TDS&nbsp;Main&nbsp;Account
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtTDSMainAC" runat="server" TabIndex="0" Width="320px" onkeyup="CallTDSMainAccount(this,'GenericAjaxList',event)"></asp:TextBox>
                            <br />
                            <span id="spnTDSMainAc" style="color: Red;"></span>
                            <asp:HiddenField ID="txtTDSMainAC_hidden" runat="server" />
                            <asp:HiddenField ID="hdnTDSMainAcType" runat="server" />
                        </td>
                        <td align="right" style="font-weight: bold;">TDS&nbsp;Sub&nbsp;Account
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtTDSSubAC" runat="server" TabIndex="0" Width="320px" onkeyup="CallTDSSubAccount(this,'GenericAjaxList',event)"></asp:TextBox>
                            <br />
                            <span id="spnTDSSubAc" style="color: Red;"></span>
                            <asp:HiddenField ID="txtTDSSubAC_hidden" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:HiddenField ID="hdnMode" runat="server" />
                            <asp:Button ID="btnSave" Text="Save" runat="server" TabIndex="0" OnClick="btnSave_Click" OnClientClick="return validate();" />
                        </td>
                    </tr>

                </table>
            </div>
        </div>
    </div>
    <div id="divDetailsPopUp" style="background-color: #B7CEEC; height: 230px; left: 481px; position: absolute; top: 25px; width: 800px; left: 90px; z-index: 75; display: none; padding-bottom: 10px;">
        <div style="background-color: Black; height: 12px; font-size: small; color: White; font-weight: bold; padding-bottom: 27px; padding-left: 2px;">
            Interest Scheme Details Parameter Form
                <img src="../windowfiles/close.gif" height="16px" alt="CLOSE" onclick="ClosePopUp();"
                    style="padding-left: 98%; margin: 0px;" />
            <div style="background-color: #B7CEEC; color: Black; padding-left: 0px;">
                <asp:Panel ID="pnlSlab" runat="server" BackColor="#B7CEEC">
                    <table border="5" style="border-collapse: collapse; background-color: #B7CEEC;" width="97%">
                        <tr>
                            <td align="left" style="font-weight: bold;">Day&nbsp;From
                            </td>
                            <td>
                                <div>
                                    <div style="float: left;">
                                        <asp:TextBox ID="txtDateFromDetails" runat="server" ValidationGroup="details" TabIndex="0" Enabled="false" onkeypress="return isNumber(event)"></asp:TextBox>
                                    </div>
                                    <div style="float: left;">
                                        <asp:RequiredFieldValidator ID="reqDaysFromDetails" runat="server" ControlToValidate="txtDateFromDetails" ErrorMessage="Enter&nbsp;Days&nbsp;From" ValidationGroup="details" ForeColor="red" Font-Bold="true">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </td>
                            <td align="left" style="font-weight: bold;">Day&nbsp;To
                            </td>
                            <td>
                                <div>
                                    <div style="float: left;">
                                        <asp:TextBox ID="txtDateToDetails" runat="server" ValidationGroup="details" TabIndex="0" MaxLength="2" onkeypress="return isNumber(event)"></asp:TextBox>
                                    </div>
                                    <div style="float: left;">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDateToDetails" ErrorMessage="Enter&nbsp;Days&nbsp;To" ValidationGroup="details" Display="Dynamic" ForeColor="red" Font-Bold="true">
                                        </asp:RequiredFieldValidator>
                                        <%-- <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtDateToDetails" ControlToCompare="txtDateFromDetails" ErrorMessage="Day to should be greater than day from" Operator="LessThan" Display="Dynamic" ValidationGroup="details">
                          </asp:CompareValidator>--%>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="font-weight: bold;">Amount&nbsp;From
                            </td>
                            <td colspan="1">
                                <div>
                                    <div style="float: left;">
                                        <asp:TextBox ID="txtAmountFrom" runat="server" ValidationGroup="details" TabIndex="0" Enabled="false" onkeypress="return isNumber(event)"></asp:TextBox>
                                    </div>
                                    <div style="float: left;">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtAmountFrom" ErrorMessage="Enter&nbsp;Amount&nbsp;From" ValidationGroup="details" ForeColor="yellow" Font-Bold="true">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </td>
                            <td align="left" style="font-weight: bold;">Amount&nbsp;To
                            </td>
                            <td colspan="1">
                                <div>
                                    <div style="float: left;">
                                        <asp:TextBox ID="txtAmountTo" runat="server" ValidationGroup="details" TabIndex="0" MaxLength="15" onkeypress="return isNumber(event)"></asp:TextBox>
                                    </div>
                                    <div style="float: left;">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtAmountTo" ErrorMessage="Enter&nbsp;Amount&nbsp;To" ValidationGroup="details" Display="Dynamic" ForeColor="red" Font-Bold="true">
                                        </asp:RequiredFieldValidator>
                                        <%--<asp:CompareValidator ID="cmp" runat="server" ControlToValidate="txtAmountTo" ControlToCompare="txtAmountFrom" ErrorMessage="Amount to should be greater than amount from" Operator="GreaterThan" Display="Dynamic" ValidationGroup="details">
                          </asp:CompareValidator>--%>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="font-weight: bold;">Rate 
                            </td>
                            <td colspan="1">
                                <div>
                                    <div style="float: left;">
                                        <asp:TextBox ID="txtRate" runat="server" ValidationGroup="details" TabIndex="0" MaxLength="9" onkeypress="return isNumber(event)"></asp:TextBox>
                                    </div>
                                    <div style="float: left;">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtRate" ErrorMessage="Enter&nbsp;Rate" ValidationGroup="details" ForeColor="red" Font-Bold="true">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </td>
                            <td colspan="2">
                                <asp:Button ID="btnAdd" runat="server" Text="ADD" ValidationGroup="details" OnClick="btnAdd_Click" />
                            </td>

                        </tr>
                    </table>
                </asp:Panel>
                <%--<asp:Label ID="lblBlankDetailMessage" runat="server" Text="" EnableViewState="false"></asp:Label>--%>
                <div style="height: 100px; padding-top: 10px; overflow-y: scroll; background-color: #B7CEEC; background-color: #B7CEEC;">
                    <asp:Repeater ID="rptDetails1" runat="server" OnItemCommand="rptDetails1_ItemCommand">
                        <HeaderTemplate>
                            <table width="98%" border="5" style="border-collapse: collapse; background-color: #B7CEEC;">
                                <tr style="background-color: #B7CEEC; font-weight: bold; color: White;">
                                    <td style="font-weight: bold;">SL No</td>
                                    <td style="font-weight: bold;">Day From</td>
                                    <td style="font-weight: bold;">Day To</td>
                                    <td style="font-weight: bold;">Amount From</td>
                                    <td style="font-weight: bold;">Amount To</td>
                                    <td style="font-weight: bold;">Rate</td>

                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><%#(((RepeaterItem)Container).ItemIndex+1).ToString()%>
                                </td>
                                <td>
                                    <asp:Label ID="lblDayFrom" runat="server" Text='<%#Eval("DayFrom")%>'></asp:Label>

                                </td>
                                <td>
                                    <asp:Label ID="lblDayTo" runat="server" Text='<%#Eval("DayTo")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblAmountFrom" runat="server" Text='<%#Eval("AmountFrom")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblAmountTo" runat="server" Text='<%#Eval("AmountTo")%>'></asp:Label>
                                </td>
                                <td style="color: Black;">
                                    <asp:Label ID="lblRate" runat="server" Text='<%#Eval("Rate")%>'></asp:Label>
                                </td>

                            </tr>
                        </ItemTemplate>

                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                    <br />
                    <div style="padding: 0 0 0 10px;">
                        <asp:Label ID="lblBlank" runat="server" Text="No Record Found" Font-Bold="true" Font-Size="Medium" />
                    </div>
                </div>
                <table>

                    <tr>
                        <td colspan="1">
                            <asp:Button ID="btnDeleteDetails" runat="server" Text="Delete Details" OnClick="btnDeleteDetails_Click" OnClientClick="return DeleteConfirmation();" />
                        </td>
                        <td>
                            <asp:Button ID="btnSaveDetails" runat="server" Text="Save" OnClick="btnSaveDetails_Click" OnClientClick="alert('Detailed parameters saved successfully');" />
                        </td>
                        <td>
                            <asp:Button ID="btnRemove" runat="server" Text="Remove All" Visible="false" OnClick="btnRemove_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>

