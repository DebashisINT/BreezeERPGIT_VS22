<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Management_InterestSchemeMapping" CodeBehind="InterestSchemeMapping.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>


<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
 <%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web" TagPrefix="dxpc" %>--%>

    <style type="text/css">
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
    <style type="text/css">
        .radioButtonList {
            list-style: none;
            margin: 0;
            padding: 2px;
        }

            .radioButtonList.horizontal li {
                display: inline;
            }

            .radioButtonList label {
                display: inline;
            }
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
        var mainAcTypes = "";
        var mainAcType = "";
        /*  $(document).ready(function(){
             //  var count=0; 
              $("#rbtnPostInAc").change(function(){
                var rb=document.getElementById("rbtnPostInAc");
                var inputs = rb.getElementsByTagName('input');
               var flag = false;
               var selected;
               for (var i = 0; i < inputs.length; i++) {
                   if (inputs[i].checked) {
                       selected = inputs[i];
                       flag = true;
                       break;
                   }
               }
               if (flag){
                 // alert(selected.value);
                   IsPostInAc=selected.value;
                 if(selected.value==0){
                    document.getElementById("rbtnPostInSameHead").disabled = true;
                      document.getElementById("txtPostMainAc").disabled = true;
                        document.getElementById("txtPostSubAc").disabled = true;
                           document.getElementById("reqPostMainAc").disabled = true;
                        document.getElementById("reqPostSubAc").disabled = true;
                 }
                 else if(selected.value==1){
                          document.getElementById("rbtnPostInSameHead").disabled = false;
                          if(IsPostSameHead==0 )
                          {
                            document.getElementById("txtPostMainAc").disabled = false;
                            document.getElementById("txtPostSubAc").disabled = false;
                            document.getElementById("reqPostMainAc").disabled = false;
                            document.getElementById("reqPostSubAc").disabled = false;
                           }
                         
                       }
                 // count=1;
               }
                 
              }
              )
          }
           
          )
           $(document).ready(function(){
              $("#rbtnPostInSameHead").change(function(){
              //  alert("xx");
              try{
              var rb = document.getElementById("rbtnPostInSameHead");
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
                  IsPostSameHead=selecteds.value;
                  if(selecteds.value==1){
                       document.getElementById("txtPostMainAc").disabled = true;
                        document.getElementById("txtPostSubAc").disabled = true;
                         document.getElementById("reqPostMainAc").disabled = true;
                        document.getElementById("reqPostSubAc").disabled = true;
                  }
                  else if(selecteds.value==0 && IsPostInAc==1){
                      document.getElementById("txtPostMainAc").disabled = false;
                        document.getElementById("txtPostSubAc").disabled = false;
                        document.getElementById("reqPostMainAc").disabled = false;
                        document.getElementById("reqPostSubAc").disabled = false;
                  }
                 }
                 catch(ex)
                 {
                  // alert(ex);
                 }
              })
           }) */
        function PostDeleteMessage() {
            alert('Data deleted Successfully');

        }
        function PostSaveMessage(v) {
            if (v == '0') {
                alert('Data saved successfully');
            }
            else if (v == '1') {
                alert('Already this interest scheme is mapped with this account and subaccount under current company');
                ShowPopUp();
            }
            else if (v == '2') {
                alert('Already this account and subaccount is mapped on this date under current company');
                ShowPopUp();
            }
            else if (v == '3') {
                alert('Already this account and subaccount is mapped on this date under current company');
                ShowPopUp();
            }
        }
        function MakeEnable() {
            document.getElementById("rbtnPostInSameHead").disabled = false;
        }
        function ShowPopUp() {
            document.getElementById("divOverlapping").style.display = 'block';
            document.getElementById("divPopUp").style.display = 'block';
            // EnableDisableControl(0);
            // document.getElementById("rbtnPostInSameHead").disabled = true;
            document.getElementById("rbtnPostInSameHead").disabled = true;
        }
        function ClosePopUp() {
            document.getElementById("divOverlapping").style.display = 'none';
            document.getElementById("divPopUp").style.display = 'none';
            //  __doPostBack();

        }
        function EnableDisableControl(li) {

            if (li == 1) {
                //alert(li);
                document.getElementById("trPostHead").disabled = true;
            }
            else {
                //  alert(li);
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
        function Check() {
            //  alert(GetValue("txtSubAc_hidden")+'--'+GetValue("txtMainAc_hidden")+'--'+GetValue("txtPostSubAc_hidden")+'--'+GetValue("txtPostMainAc_hidden"));
        }
        function keyVal(obj) {

            var MainAccoutValue = GetValue("txtSubAc_hidden");
            mainAcTypes = ((GetValue("txtMainAc_hidden")).split('~')[1]).toString().toLowerCase().trim();
            if (mainAcTypes.trim() == "none" || mainAcTypes.trim() == "") {
                // alert("1");
                document.getElementById("hdnMainAcType").value = "N";
            }
            else {
                //  alert("2");
                document.getElementById("hdnMainAcType").value = "Y";
            }
            mainAcType = ((GetValue("txtPostMainAc_hidden")).split('~')[1]).toString().toLowerCase().trim();
            if (mainAcType.trim() == "none" || mainAcType.trim() == "") {
                document.getElementById("hdnPostMainAcType").value = "N";
            }
            else {
                document.getElementById("hdnPostMainAcType").value = "Y";
            }

            //alert(MainAccoutValue);
            // alert(GetValue("txtSubAc_hidden")+'--'+GetValue("txtMainAc_hidden")+'--'+GetValue("txtPostSubAc_hidden")+'--'+GetValue("txtPostMainAc_hidden"));
            // alert(GetValue("txtMainAc_hidden").split('*')[1]);
            // alert(GetValue("txtPostMainAc_hidden").split('*')[1]);

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
            try {
                var strQuery_Table = "Master_MainAccount";
                var strQuery_FieldName = " top 10 MainAccount_Name+\' [ \'+rtrim(ltrim(MainAccount_AccountCode))+\' ]\' as MainAccount_Name1,cast(MainAccount_ReferenceID as varchar)+\'~\'+MainAccount_SubLedgerType+\'~MAINAC~\'+MainAccount_AccountType +'*'+MainAccount_AccountCode as MainAccount_ReferenceID";
                //  var strQuery_WhereClause = " (MainAccount_Name like (\'%RequestLetter%\') or MainAccount_AccountCode like (\'%RequestLetter%\')) and isnull(MainAccount_BankCashType,'abc') not in('Cash','Bank')";
                var strQuery_WhereClause = " (MainAccount_Name like (\'%RequestLetter%\') or MainAccount_AccountCode like (\'%RequestLetter%\'))";
                strQuery_WhereClause += "  and isnull(MainAccount_BankCashType,'') not in ('Bank','Cash') and left(isnull(MainAccount_SubLedgerType,''),4)<>'PROD' "
                var strQuery_OrderBy = '';
                var strQuery_GroupBy = '';
                var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
                ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');
            }
            catch (ex) {
                //  alert(ex);
            }
        }
        function CallPostMainAccount(obj1, obj2, obj3) {
            //  alert(GetValue("txtMainAc_hidden"));
            try {

                var SelectedMainAcId = GetValue("txtMainAc_hidden").split('~')[0];

                var strQuery_Table = "Master_MainAccount";
                var strQuery_FieldName = " top 10 MainAccount_Name+\' [ \'+rtrim(ltrim(MainAccount_AccountCode))+\' ]\' as MainAccount_Name1,cast(MainAccount_ReferenceID as varchar)+\'~\'+MainAccount_SubLedgerType+\'~MAINAC~\'+MainAccount_AccountType +'*'+MainAccount_AccountCode as MainAccount_ReferenceID";
                //  var strQuery_WhereClause = " (MainAccount_Name like (\'%RequestLetter%\') or MainAccount_AccountCode like (\'%RequestLetter%\')) and isnull(MainAccount_BankCashType,'abc') not in('Cash','Bank')";
                var strQuery_WhereClause = " (MainAccount_Name like (\'%RequestLetter%\') or MainAccount_AccountCode like (\'%RequestLetter%\'))";
                strQuery_WhereClause += "  and isnull(MainAccount_BankCashType,'') not in ('Bank','Cash') and left(isnull(MainAccount_SubLedgerType,''),4)<>'PROD' ";
                if (SelectedMainAcId != "") {
                    strQuery_WhereClause += " AND mainAccount_ReferenceId <>" + SelectedMainAcId;
                }
                var strQuery_OrderBy = '';
                var strQuery_GroupBy = '';
                var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);

                ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');
            }
            catch (ex) {
                // alert(ex);
            }
        }
        function CallSubAccount(obj1, obj2, obj3) {

            try {
                //  alert(GetValue("txtMainAc_hidden"));
                mainAcTypes = ((GetValue("txtMainAc_hidden")).split('~')[1]).toString().toLowerCase().trim();
                //alert(((GetValue("txtMainAc_hidden")).split('*')[1]).toString().toLowerCase().trim());
                // var  mainAcTypes = (document.getElementById("txtMainAc_hidden").value).split('~')[1]).toString().toLowerCase();

                if (mainAcTypes == "custom") {
                    var subreferenceAcId = GetValue("txtMainAc_hidden").split('*')[1];
                    // alert(subreferenceAcId) ;
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
                    var strQuery_FieldName = "cnt_firstName+' '+ISNULL(cnt_middleName,'')+ ' '+cnt_lastName+' ' + cnt_UCC as TEXT, cnt_internalId+'*'+cnt_internaliD as value"
                    var strQuery_WhereClause = " (cnt_firstName like (\'%RequestLetter%\') or cnt_middleName like (\'%RequestLetter%\') or cnt_shortName like (\'%RequestLetter%\') or cnt_lastName like (\'%RequestLetter%\') or cnt_UCC like (\'%RequestLetter%\'))";
                    var strQuery_OrderBy = '';
                    var strQuery_GroupBy = '';
                    var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
                    ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');
                }
                else if (mainAcTypes == "cdsl clients" || mainAcTypes == "master_cdslclients") {
                    var strQuery_Table = "Master_CdslClients";
                    var strQuery_FieldName = "CdslClients_BenAccountNumber,CdslClients_FirstHolderName+CdslClients_FirstHolderMiddleName+CdslClients_FirstHolderLastName as Text " + ", CdslClients_BenAccountNumber +'*'+CdslClients_BenAccountNumber"
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
            catch (ex) {
                // alert(ex);
            }

        }

        function CallPostSubAccount(obj1, obj2, obj3) {
            // alert('1');
            try {
                mainAcType = ((GetValue("txtPostMainAc_hidden")).split('~')[1]).toString().toLowerCase().trim();
                //  var  mainAcType = (document.getElementById("txtPostMainAc_hidden").value).split('~')[1]).toString().toLowerCase();
                // var   mainAcSelectedId=GetValue("txtSubAc_hidden").split('*')[1];
                //  alert(mainAcSelectedId);
                //  var  mainAcSelectedId=(document.getElementById("txtSubAc_hidden").value).split('*')[1];

                if (mainAcType == "custom") {
                    var subreferenceAcIdpOST = GetValue("txtPostMainAc_hidden").split('*')[1];
                    // alert(subreferenceAcIdpOST) ;
                    var strQuery_Table = "Master_SubAccount";
                    var strQuery_FieldName = "top 10 RTrim(LTrim(RTrim(LTrim(SubAccount_Name)) +'-['+Rtrim(Ltrim (SubAccount_Code))+']')) as Text, SubAccount_Code +'*'+SubAccount_Code as Value";
                    var strQuery_WhereClause = " (SubAccount_Name like (\'%RequestLetter%\') or SubAccount_Code like (\'%RequestLetter%\'))";
                    //  strQuery_WhereClause += " AND SubAccount_MainAcReferenceID='BADC002'"

                    strQuery_WhereClause += " AND SubAccount_MainAcReferenceID=" + "'" + subreferenceAcIdpOST + "'"
                    //           if(mainAcSelectedId=="")
                    //           {
                    //             strQuery_WhereClause +=" AND SubAccount_Code<>"+mainAcSelectedId;
                    //           }
                    var strQuery_OrderBy = '';
                    var strQuery_GroupBy = '';
                    var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
                    ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');
                }
                else if (mainAcType == "brokers" || mainAcType == "customers" || mainAcType == "employees" || mainAcType == "creditors" || mainAcType == "debtors" || mainAcType == "sub broker" || mainAcType == "franchisee" || mainAcType == "agent" || mainAcType == "data vendor" || mainAcType == "relationship partner" || mainAcType == "relationship manager" || mainAcType == "vendor") {
                    var strQuery_Table = "tbl_master_contact";
                    var strQuery_FieldName = "cnt_firstName+' '+ISNULL(cnt_middleName,'')+ ' '+cnt_lastName+' ' + cnt_UCC as TEXT, cnt_internalId+'*'+cnt_internaliD as value"
                    var strQuery_WhereClause = " (cnt_firstName like (\'%RequestLetter%\') or cnt_shortName like (\'%RequestLetter%\') or cnt_middleName like (\'%RequestLetter%\') or cnt_lastName like (\'%RequestLetter%\') or cnt_UCC like (\'%RequestLetter%\'))";
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
                    var strQuery_FieldName = "CdslClients_BenAccountNumber,CdslClients_FirstHolderName+CdslClients_FirstHolderMiddleName+CdslClients_FirstHolderLastName as Text " + ", CdslClients_BenAccountNumber +'*'+CdslClients_BenAccountNumber"
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
        function ValidateFields() {

            try {
                if (Control_Empty("txtMainAc", "Please Select Main Account"))
                    if (Control_Empty("txtSubAc", "Please Select Sub Account"))
                        if (Control_Empty("txtIntSchemeCode", "Please Select Interest Scheme Code"))
                            if (document.getElementById("txtPostMainAc").value = "") {
                                // alert("AAA");
                            }



                return false;
            }
            catch (ex) {
                // alert(ex);
                return false;
            }

        }
        function validate() {
            var vv = "0";

            // alert("ABCD-1234");
            try {
                if (document.getElementById("txtMainAc").value == "" || document.getElementById("txtMainAc_hidden").value == "") {
                    document.getElementById("tdMainAc").innerHTML = "Enter  Main Account Name";
                    vv = "1";
                }
                else {
                    document.getElementById("tdMainAc").innerHTML = "";
                }

                if ((document.getElementById("txtSubAc").value == "" || document.getElementById("txtSubAc_hidden").value == "") && (document.getElementById("hdnMainAcType").value == "Y")) {

                    document.getElementById("tdSubAc").innerHTML = "Enter  Sub Account Name";
                    vv = "1";
                }
                else {
                    document.getElementById("tdSubAc").innerHTML = "";
                }
                //              if(document.getElementById("txtIntSchemeCode").value=="" || document.getElementById("txtIntSchemeCode_hidden").value=="")
                //            {
                //                document.getElementById("tdIntSchemeCode").innerHTML ="Enter Interest Scheme Code" ;
                //                vv="1";
                //            }
                //            else {
                //                 document.getElementById("tdIntSchemeCode").innerHTML ="" ;
                //            }
                if (IsPostInAc == "1" && IsPostSameHead == "0") {
                    if (document.getElementById("txtPostMainAc").value == "" || document.getElementById("txtPostMainAc_hidden").value == "") {
                        document.getElementById("tdPostMainAc").innerHTML = "Enter Post in Main Account Name";
                        vv = "1";
                    }
                    else {
                        document.getElementById("tdPostMainAc").innerHTML = "";
                    }
                    if ((document.getElementById("txtPostSubAc").value == "" || document.getElementById("txtPostSubAc_hidden").value == "") && (document.getElementById("hdnPostMainAcType").value == "Y")) {
                        document.getElementById("tdPostSubAc").innerHTML = "Enter Post in Sub Account Name";
                        vv = "1";
                    }
                    else {
                        document.getElementById("tdPostSubAc").innerHTML = "";
                    }
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
                // alert(ex);
                return false;
            }

        }
        function DeleteConfirmation() {
            var i = confirm("Are you sure delete this record?");
            if (i == true) {
                var j = confirm("Are you sure delete this record?");
                if (j == true) {
                    var k = confirm("Are you sure delete this record?");
                    if (k == true) {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            }
            return false;
        }
        function InitialValidator() {
            var vv = 0;
            if (document.getElementById("txtIntSchemeCode").value == "" || document.getElementById("txtIntSchemeCode_hidden").value == "") {
                document.getElementById("lblBlankMessage").innerHTML = "Enter Interest Scheme Code";
                vv = "1";
            }
            else {
                document.getElementById("lblBlankMessage").innerHTML = "";
            }
            // alert(vv);
            if (vv == "0") {
                return true;
            }
            else {
                return false;
            }
        }
        function preventBackspace(e) {
            KeyPress();
            var evt = e || window.event;
            if (evt) {
                var keyCode = evt.charCode || evt.keyCode;
                if (keyCode === 8) {

                    if (evt.preventDefault) {
                        evt.preventDefault();
                        evt.stopPropagation();
                    }
                    else {
                        evt.returnValue = false;
                    }
                }
            }
        }
        function changeState1(v) {
            IsPostInAc = v;

            try {
                if (IsPostInAc == 1 && IsPostSameHead == 0) {
                    document.getElementById("txtPostMainAc").disabled = false;
                    document.getElementById("txtPostSubAc").disabled = false;
                    document.getElementById("rbtnPostInSameHead").disabled = false;
                }

                else if (IsPostInAc == 0) {
                    // document.getElementById("rbtnPostInSameHead").disabled = true;
                    document.getElementById("txtPostMainAc").disabled = true;
                    document.getElementById("txtPostSubAc").disabled = true;
                }
                if (v == 1) {
                    // alert("R");
                    document.getElementById("rbtnPostInSameHead").disabled = false;
                }
                else {
                    // alert("M");
                    document.getElementById("rbtnPostInSameHead").disabled = true;
                }
            }
            catch (ex) {
                alert(ex);
            }
        }
        function changeState2(v) {
            IsPostSameHead = v;
            if (IsPostInAc == 1 && IsPostSameHead == 0) {
                document.getElementById("txtPostMainAc").disabled = false;
                document.getElementById("txtPostSubAc").disabled = false;
            }
            else if (IsPostSameHead == 1) {
                document.getElementById("txtPostMainAc").disabled = true;
                document.getElementById("txtPostSubAc").disabled = true;
            }
        }
    </script>
    <script type="text/javascript" language="javascript">
         FieldName = 'BtnSave';
                            </script>
                        </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table width="100%" cellspacing="2">
            <col width="75%" />
            <col width="25%" />
            <tr style="padding-bottom: 20px;">

                <td style="font-size: large; font-weight: bold; padding-bottom: 5px;" colspan="2" align="center">Interest Scheme Mapping
                </td>

            </tr>
            <tr>
                <td colspan="2">
                    <br />
                </td>
            </tr>
            <tr style="padding-top: 15px;">
                <td align="left" colspan="2">
                    <div>
                        <div style="float: left;">
                            <b>Company :</b>
                            <asp:Label ID="lblCompany" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                
                      <b>Interest Scheme :</b>

                            <asp:TextBox ID="txtIntSchemeCode" runat="server" Width="240" EnableViewState="true" onkeyup="CallInterestScheme(this,'GenericAjaxList',event)"></asp:TextBox>&nbsp;&nbsp;
                   <asp:HiddenField ID="txtIntSchemeCode_hidden" runat="server" />
                        </div>
                        <div style="float: left;">
                            &nbsp;<asp:Label ID="lblBlankMessage" runat="server" ForeColor="red"></asp:Label>
                            &nbsp;<asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" OnClientClick="return InitialValidator();" />
                        </div>
                    </div>
                </td>
            </tr>
        </table>

        <%-- <asp:LinkButton  ID="lbtnAdds" Text="Add New" runat="server" OnClientClick="ShowPopUp();return false;"></asp:LinkButton>--%>
        <asp:GridView ID="gvInterestSchemeMapping" runat="server" AutoGenerateColumns="false" Width="99%" AllowPaging="true" PageSize="10" HeaderStyle-BackColor="#B7CEEC" OnPageIndexChanged="gvInterestSchemeMapping_PageIndexChanged" OnPageIndexChanging="gvInterestSchemeMapping_PageIndexChanging" OnRowCommand="gvInterestSchemeMapping_RowCommand" PagerSettings-Position="Bottom" PagerSettings-Mode="NumericFirstLast" EmptyDataRowStyle-Font-Size="X-Large" EmptyDataRowStyle-Font-Bold="true" EmptyDataRowStyle-HorizontalAlign="Justify" EditRowStyle-VerticalAlign="Middle">
            <Columns>
                <asp:TemplateField HeaderText="Main Account" ItemStyle-Width="240px">
                    <ItemTemplate>
                        <%#Eval("MainAcName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sub Account" ItemStyle-Width="240px">
                    <ItemTemplate>
                        <%#Eval("SubAcName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Date From" ItemStyle-Width="140px">
                    <ItemTemplate>
                        <%-- <%#Eval("IntMembers_DateFrom").ToString()%>--%>
                        <asp:Label ID="lblFrom" runat="server" Text='<%#GetDateFormat(Eval("IntMembers_DateFrom"))%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Width="130px">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lbtnAdd" Text="Add New" runat="server" CommandName="AddNew" OnClientClick="return InitialValidator();" ForeColor="blue" Font-Underline="true"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnInfo" Text="More Info......." runat="server" CommandArgument='<%#Eval("IntMembers_ID") %>' CommandName="info"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Width="120px">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnDelete" runat="server" Text="DELETE" CommandName="dele" CommandArgument='<%#Eval("IDandDateTo") %>' OnClientClick="return DeleteConfirmation();" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataRowStyle Font-Size="700px" Font-Bold="true" />
            <EmptyDataTemplate>
                <div style="font-size: small; font-weight: bold; padding: 5px 0 0 10px;">
                    No Data Found. Click
                    <asp:LinkButton ID="lbtnAddEmpty" Font-Size="Small" Text="Here" runat="server" ForeColor="Blue" CommandName="AddNew" OnClientClick="return InitialValidator();"></asp:LinkButton>
                    to add data 
                </div>
            </EmptyDataTemplate>
        </asp:GridView>

    </div>
    <div id="divOverlapping" style="position: fixed; height: 100%; width: 100%; background-color: #000; top: 0px; left: 0px; opacity: 0.4; filter: alpha(opacity=40); z-index: 50; display: none;">
    </div>
    <div id="divPopUp" class="container" style="background-color: #B7CEEC; height: 300px; left: 90px; position: absolute; top: 10px; width: 790px; z-index: 75; display: none; padding-bottom: 10px;">
        <div style="background-color: #B7CEEC; height: 7px; font-size: small; color: White; padding-bottom: 0px; padding-left: 0px;">
            <div style="background-color: Black; color: White; font-weight: bold; font-size: medium;">
                Interest Scheme Mapping Form
              <img src="../windowfiles/close.gif" height="19px" alt="CLOSE" onclick="ClosePopUp();" style="padding-left: 96%; padding-top: -10px; margin: 0px; top: -7px;" />
            </div>
            <div style="background-color: #B7CEEC; color: Black; padding-left: 10px; padding-top: 0px;">
                <table cellpadding="2" cellspacing="5" class="container" style="background-color: #B7CEEC;">

                    <tr>
                        <td align="right" style="font-weight: bold;">Main Account
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtMainAc" runat="server" Width="430" onkeyup="CallMainAccount(this,'GenericAjaxList',event);" onKeyDown="preventBackspace();">
                            </asp:TextBox>
                            <asp:HiddenField ID="txtMainAc_hidden" runat="server" />
                        </td>
                        <td id="tdMainAc" style="color: Yellow; font-weight: bold;">

                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtMainAc" ErrorMessage="Select Main Account" ValidationGroup="vg"></asp:RequiredFieldValidator>--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="font-weight: bold;">Sub Account
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtSubAc" runat="server" Width="430" onkeyup="CallSubAccount(this,'GenericAjaxList',event)"></asp:TextBox>
                            <asp:HiddenField ID="txtSubAc_hidden" runat="server" />
                        </td>
                        <td id="tdSubAc" style="color: Yellow; font-weight: bold;">

                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtSubAc" ErrorMessage="Select Sub Account" ValidationGroup="vg"></asp:RequiredFieldValidator>--%>
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td align="right" style="font-weight: bold;">Interest Scheme Code
                        </td>
                        <td align="left">
                            <%-- <asp:TextBox ID="txtIntSchemeCode" runat="server" onkeyup="CallInterestScheme(this,'GenericAjaxList',event)"></asp:TextBox>
                            <asp:HiddenField ID="txtIntSchemeCode_hidden" runat="server" />--%>
                        </td>
                        <td id="tdIntSchemeCode" style="color: Yellow; font-weight: bold;">
                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtIntSchemeCode" ErrorMessage="Select Interest Scheme Code" ValidationGroup="vg"></asp:RequiredFieldValidator>--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" id="trSameAc" style="font-weight: bold;">Is Post In Account ?
                        </td>
                        <td align="left">
                            <asp:RadioButtonList ID="rbtnPostInAc" runat="server" RepeatDirection="Horizontal" CssClass="radioButtonList">
                                <asp:ListItem Text="YES" Value="1" onclick="changeState1('1');"></asp:ListItem>
                                <asp:ListItem Selected="True" Text="No" Value="0" onclick="changeState1('0');"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr id="trPostHead">
                        <td align="right" style="font-weight: bold;">Is Post In Same Head ?
                        </td>
                        <td align="left">
                            <asp:RadioButtonList ID="rbtnPostInSameHead" runat="server" RepeatDirection="Horizontal" CssClass="radioButtonList">
                                <asp:ListItem Selected="True" Text="YES" Value="1" onclick="changeState2('1');"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0" onclick="changeState2('0');"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="font-weight: bold;">Post Main Account 
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtPostMainAc" runat="server" Width="430" onkeyup="CallPostMainAccount(this,'GenericAjaxList',event)" Enabled="false"></asp:TextBox>
                            <asp:HiddenField ID="txtPostMainAc_hidden" runat="server" />
                        </td>
                        <td id="tdPostMainAc" style="color: Yellow; font-weight: bold;">
                            <%--<asp:RequiredFieldValidator ID="reqPostMainAc" runat="server" ControlToValidate="txtPostMainAc" ErrorMessage="Select Post Main Account" ValidationGroup="vg" Enabled="false"></asp:RequiredFieldValidator>--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="font-weight: bold;">Post Sub Account
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtPostSubAc" runat="server" Width="430" onkeyup="CallPostSubAccount(this,'GenericAjaxList',event)" Enabled="false"></asp:TextBox>
                            <asp:HiddenField ID="txtPostSubAc_hidden" runat="server" />
                        </td>
                        <td id="tdPostSubAc" style="color: Yellow; font-weight: bold;">
                            <%--  <asp:RequiredFieldValidator ID="reqPostSubAc" runat="server" ControlToValidate="txtPostSubAc" ErrorMessage="Select Post Sub Account" ValidationGroup="vg" Enabled="false"></asp:RequiredFieldValidator>--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="font-weight: bold;">Date From 
                        </td>
                        <td>
                            <dxe:ASPxDateEdit ID="txtDateFrom" runat="server" Width="430px" EditFormat="Custom"
                                EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" TabIndex="8" ClientInstanceName="ctxtDateFrom">
                                <ButtonStyle>
                                </ButtonStyle>
                                <ClientSideEvents DateChanged="function(s,e){DateChange(ctxtKYCModDate);}"></ClientSideEvents>
                            </dxe:ASPxDateEdit>
                        </td>
                        <td>
                            <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtDateFrom" ErrorMessage="Enter Date From" ValidationGroup="vg"></asp:RequiredFieldValidator>--%>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td colspan="2">
                            <asp:Button ID="btnSave" Text="Save" runat="server" ValidationGroup="vg21" OnClick="btnSave_Click" OnClientClick="return validate();" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:HiddenField ID="hdnMainAcType" runat="server" Value="Y" />
        <asp:HiddenField ID="hdnPostMainAcType" runat="server" Value="Y" />
    </div>
</asp:Content>


