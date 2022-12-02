<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    EnableEventValidation="false" Inherits="ERP.OMS.Reports.Reports_DeliveryProcessing" Codebehind="DeliveryProcessing.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
        function HideOn()
        {
             document.getElementById('TrGrid').style.display='none';
             document.getElementById('TrPross').style.display='none';
        }
	    function FunSettNumber(objID,objListFun,objEvent)
        {
            ajax_showOptions(objID,objListFun,objEvent,'SettType','Sub');
        }
        function FunForSettNum(objID,objListFun,objEvent)
        {
            ajax_showOptions(objID,objListFun,objEvent);
        }
        function FunForSettType(objID,objListFun,objEvent)
        {            
            ajax_showOptions(objID,objListFun,objEvent,document.getElementById('txtSettNumberHoldBack').value);
        }
        function height()
        {
            if(document.body.scrollHeight>=500)
            {
                window.frameElement.height = document.body.scrollHeight;
            }
            else
            {
                window.frameElement.height = '500';
            }
            window.frameElement.width = document.body.scrollWidth;
        }
        function Page_Load()
        {
            document.getElementById('TrDeliveryShow').style.display='none';
            document.getElementById('TrPross').style.display='none';
            document.getElementById('TrGrid').style.display='none';
            document.getElementById('showFilter').style.display='none';
            document.getElementById('TrHoldBackSett').style.display='none';
            document.getElementById('TrHoldSettType').style.display='none';
            document.getElementById('TrPayInOblg').style.display='none'; 
            document.getElementById('TrExport').style.display='none';            
            TypeChange(document.getElementById('ddlType').value);
        }
        function DeliveryProcessButton()
        {
            document.getElementById('TrPross').style.display='inline';
            document.getElementById('TrGrid').style.display='inline';
            document.getElementById('TrExport').style.display='inline'; 
            height();
        }
        function CalcKeyCode(aChar) 
        {
          var character = aChar.substring(0,1);
          var code = aChar.charCodeAt(0);
          return code;
        }
        function checkNumber(val) 
        {          
          var strPass = val.value;
          var strLength = strPass.length;
          var lchar = val.value.charAt((strLength) - 1);
          var cCode = CalcKeyCode(lchar);

          /* Check if the keyed in character is a number
             do you want alphabetic UPPERCASE only ?
             or lower case only just check their respective
             codes and replace the 48 and 57 */

          if (cCode < 48 || cCode > 57 ) {
            var myNumber = val.value.substring(0, (strLength) - 1);
            val.value = myNumber;
          }
          return false;
        }
        function DeliverableValue(objVal,StockPrevVal)
        {
            if(StockPrevVal=='')
            {
                if(objVal.value!='')
                {
                    alert('You Can Not Deliver This Share');
                    objVal.value=StockPrevVal;
                }
            }
            else if(parseInt(StockPrevVal)<parseInt(objVal.value))
            {
                alert('You Can Deliver Maximum '+StockPrevVal+' Share');
                objVal.value=StockPrevVal;
            }
        }
          function fn_AddJustInterSettlement(textid,txtvalue,AddJustQty,Stock)
          {
            if(parseInt(txtvalue)>parseInt(Stock))
            {
               alert('You Can Deliver Maximum '+Stock+' Share');
               textid.value=Stock;
               textid.focus();
               textid.select();
            }
            else if(parseInt(txtvalue)>parseInt(AddJustQty))
            {
               alert('You Can Deliver Maximum '+AddJustQty+' Share');
               textid.value=AddJustQty;
               textid.focus();
               textid.select();
            }
            
          }
        function SelectAll(id) 
        {
            var frm = document.forms[0];
            var val;
            for (i=0;i<frm.elements.length;i++) 
            {
                if(frm.elements[i].type == "text")
                {
                    val='';
                    val=frm.elements[i].value
                }
                if (frm.elements[i].type == "checkbox") 
                {  
                    if(val!='')
                        frm.elements[i].checked = document.getElementById(id).checked;
                }
            }
        }  
        function SelectAllInterSegment(id)
        {
            var frm = document.forms[0];
            var val;
            for (i=0;i<frm.elements.length;i++) 
            {
                if(frm.elements[i].type == "text")
                {
                    val='';
                    val=frm.elements[i].value
                }
                if (frm.elements[i].type == "checkbox") 
                {  
                    if(val!='')
                        frm.elements[i].checked = document.getElementById(id).checked;
                }
            }
        } 
        function checkValForTextBox(obj)       
        {
            var ddlVal=document.getElementById('ddlType').value;
            var objText=obj.split('_');
            var objMainText;
            if(ddlVal=='PH')
                objMainText=objText[0]+'_'+objText[1]+'_'+'txtTransferable';
            else if(ddlVal=='IS')
                objMainText=objText[0]+'_'+objText[1]+'_'+'txtAdjst';
            else if(ddlVal=='CP')
                objMainText=objText[0]+'_'+objText[1]+'_'+'txtStock';
            else if(ddlVal=='MP')
                objMainText=objText[0]+'_'+objText[1]+'_'+'txtDeliverable';
            else if(ddlVal=='PO')
                objMainText=objText[0]+'_'+objText[1]+'_'+'txtTransferable';
            else if(ddlVal=='HR')
                objMainText=objText[0]+'_'+objText[1]+'_'+'txtStock';
            var TextStockVal=document.getElementById(objMainText).value;
            if(document.getElementById(obj).checked==true)
            {
                if(TextStockVal!='')
                     document.getElementById(obj).checked=true;
                else
                {                
                    document.getElementById(obj).checked=false;
                }
            }
            else
                 document.getElementById(obj).checked=false;
        }
//        function FunClientScrip(objID,objListFun,objEvent)
//        {
//            var cmbVal=document.getElementById('cmbsearchOption').value;
//            ajax_showOptions(objID,objListFun,objEvent,cmbVal,'Sub');
//        }
        function FunClientScrip(objID,objListFun,objEvent)
        {
          var cmbVal;
         
            if(document.getElementById('cmbsearchOption').value=="Clients")
            {
                if(document.getElementById('ddlGroup').value=="0")//////////////Group By  selected are branch
                {
                    if(document.getElementById('ddlGroup').value=="0")
                    {
                        if(document.getElementById('rdbranchAll').checked==true)
                           {
                               cmbVal='ClientsBranch'+'~'+'ALL';
                           }
                       else
                           {
                               cmbVal='ClientsBranch'+'~'+'Selected'+'~'+document.getElementById('HiddenField_Branch').value;
                           }
                    }
                   
                }
               else //////////////Group By selected are Group
                {
                   if(document.getElementById('rdddlgrouptypeAll').checked==true)
                       {
                           cmbVal='ClientsGroup'+'~'+'ALL'+'~'+document.getElementById('ddlgrouptype').value;
                       }
                   else
                       {
                           cmbVal='ClientsGroup'+'~'+'Selected'+'~'+document.getElementById('HiddenField_Group').value;
                       }
               }
            }
            else
            {
                cmbVal=document.getElementById('cmbsearchOption').value;
                cmbVal=cmbVal+'~'+document.getElementById('ddlgrouptype').value;
            }
          
          
          ajax_showOptions(objID,objListFun,objEvent,cmbVal,'Sub');
        }
         function Hide(obj)
            {
             document.getElementById(obj).style.display='none';
            }
        function Show(obj)
            {
             document.getElementById(obj).style.display='inline';
            }
    
         function  fnddlGroup(obj)
           {
                if(obj=="0")
                {
                    Hide('td_group');
                    Show('td_branch');
                }
                else
                {
                    Show('td_group');
                    Hide('td_branch');
                    var btn = document.getElementById('btnhide');
                    btn.click();
                }
                 height();
   }
   function fngrouptype(obj)
   {
       if(obj=="0")
       {
       Hide('td_allselect');
        alert('Please Select Group Type !');
       }
       else
       {
       Show('td_allselect');
       }
       height();
   }
    function fnBranch(obj)
      {
            if(obj=="a")
                Hide('showFilter');
             else
             {
                  var cmb=document.getElementById('cmbsearchOption');
                  cmb.value='Branch';
                  Show('showFilter');
             }
          height();
      }
     function fnGroup(obj)
      {
            if(obj=="a")
                Hide('showFilter');
             else
             {
                  var cmb=document.getElementById('cmbsearchOption');
                  cmb.value='Group';
                  Show('showFilter');
             }
             height();
      }
        function btnAddsubscriptionlist_click()
        {
            var userid = document.getElementById('txtsubscriptionID');
            if(userid.value != '')
            {
                var ids = document.getElementById('txtsubscriptionID_hidden');
                var listBox = document.getElementById('lstSuscriptions');
                var tLength = listBox.length;
                //alert(tLength);
                
                var no = new Option();
                no.value = ids.value;
                no.text = userid.value;
                listBox[tLength]=no;
                var recipient = document.getElementById('txtsubscriptionID');
                recipient.value='';
            }
            else
                alert('Please search name and then Add!')
            var s=document.getElementById('txtsubscriptionID');
            s.focus();
            s.select();
        }
        function btnRemovefromsubscriptionlist_click()
        {
            
            var listBox = document.getElementById('lstSuscriptions');
            var tLength = listBox.length;
            
            var arrTbox = new Array();
            var arrLookup = new Array();
            var i;
            var j = 0;
            for (i = 0; i < listBox.options.length; i++) 
            {
                if (listBox.options[i].selected && listBox.options[i].value != "") 
                {
                    
                }
                else 
                {
                    arrLookup[listBox.options[i].text] = listBox.options[i].value;
                    arrTbox[j] = listBox.options[i].text;
                    j++;
                }
            }
            listBox.length = 0;
            for (i = 0; i < j; i++) 
            {
                var no = new Option();
                no.value = arrLookup[arrTbox[i]];
                no.text = arrTbox[i];
                listBox[i]=no;
            }
        }
        function clientselectionfinal()
	    {
	        var listBoxSubs = document.getElementById('lstSuscriptions');
            var cmb=document.getElementById('cmbsearchOption');
            var listIDs='';
            var i;
            if(listBoxSubs.length > 0)
            {                
                for(i=0;i<listBoxSubs.length;i++)
                {
                    if(listIDs == '')
                        listIDs = listBoxSubs.options[i].value+';'+listBoxSubs.options[i].text;
                    else
                        listIDs += ',' + listBoxSubs.options[i].value+';'+listBoxSubs.options[i].text;
                }
                var sendData = cmb.value + '~' + listIDs;
                CallServer(sendData,"");
            }
	        var i;
            for(i=listBoxSubs.options.length-1;i>=0;i--)
            {
                listBoxSubs.remove(i);
            }
            document.getElementById('showFilter').style.display='none';
	    }
	    function Client(obj)
        {
            if(obj=="a")
            {
               document.getElementById('showFilter').style.display='none';
               document.getElementById('TrDeliveryShow').style.display='none';
            }
            else
            {
                var cmb=document.getElementById('cmbsearchOption');
                cmb.value='Clients';
                document.getElementById('showFilter').style.display='inline';
                document.getElementById('TrDeliveryShow').style.display='none';
            }
        }
        function Scrip(obj)
        {
            if(obj=="a")
            {
               document.getElementById('showFilter').style.display='none';
                document.getElementById('TrDeliveryShow').style.display='none';
            }
            else
            {
                var cmb=document.getElementById('cmbsearchOption');
                cmb.value='Scrips';
                document.getElementById('showFilter').style.display='inline';
                document.getElementById('TrDeliveryShow').style.display='none';
            }
        }
        function TypeChange(objTypeVal)
        {
            if(objTypeVal=="IS")
            {
                document.getElementById('TrCliPayout1').style.display='none';
                document.getElementById('TrCliPayout2').style.display='none';
                document.getElementById('TrCliPayout3').style.display='none';
                document.getElementById('TrCliPayoutBranchGroup').style.display='none';
                document.getElementById('TrClient').style.display='none';
                document.getElementById('TrInterSeg').style.display='inline';
                document.getElementById('TrIntSeg').style.display='inline';
                document.getElementById('TrMktPayIN').style.display='none';
                document.getElementById('TrPayInOwnAcc').style.display='none';
                document.getElementById('TrGrid').style.display='none';
                document.getElementById('TrPross').style.display='none';
                document.getElementById('TrHoldBackSett').style.display='inline';
                document.getElementById('TrHoldSettType').style.display='inline';
                document.getElementById('TrPayInOblg').style.display='inline';
                document.getElementById('TrPayInFromMarginHoldBack').style.display='none';
                document.getElementById('radAllHoldSettType').checked=true;
                document.getElementById('TdHoldSettType').style.display='none';
                document.getElementById('TrOffSetSettlement').style.display='none';
                document.getElementById('TrOffSetSettlementSource').style.display='none';
                document.getElementById('TrOffSetPosition').style.display='none';
                document.getElementById('TrMrktPayINPool').style.display='none';
                document.getElementById('TrMrktPayINPool1').style.display='none';
                document.getElementById('TrInterSettSourceAcc').style.display='inline';
                document.getElementById('TrInterSettTargetAcc').style.display='inline';
                document.getElementById('TdPA').style.display='none';
                document.getElementById('TdPA1').style.display='none';
                document.getElementById('TdPA2').style.display='none';
                document.getElementById('TdPA3').style.display='none';
                document.getElementById('TrMrgnHoldback').style.display='none';
                document.getElementById('TrOwnAccount').style.display='none';                
            }
            else if(objTypeVal=="CP")
            {
                document.getElementById('TrCliPayout1').style.display='inline';
                document.getElementById('TrCliPayout2').style.display='inline';
                document.getElementById('TrCliPayout3').style.display='inline';
                document.getElementById('TrCliPayoutBranchGroup').style.display='inline';
                document.getElementById('TrClient').style.display='inline';
                document.getElementById('TrInterSeg').style.display='none';
                document.getElementById('TrIntSeg').style.display='none';
                document.getElementById('TrMktPayIN').style.display='none';
                document.getElementById('TrPayInOwnAcc').style.display='none';
                document.getElementById('TrGrid').style.display='none';
                document.getElementById('TrPross').style.display='none';
                document.getElementById('TrHoldBackSett').style.display='none';
                document.getElementById('TrHoldSettType').style.display='none';
                document.getElementById('TrPayInOblg').style.display='none';
                document.getElementById('TrPayInFromMarginHoldBack').style.display='none';
                document.getElementById('TrOffSetSettlement').style.display='none';
                document.getElementById('TrOffSetSettlementSource').style.display='none';
                document.getElementById('TrOffSetPosition').style.display='none';
                document.getElementById('TrMrktPayINPool').style.display='none';
                document.getElementById('TrMrktPayINPool1').style.display='none';
                document.getElementById('TrInterSettSourceAcc').style.display='none';
                document.getElementById('TrInterSettTargetAcc').style.display='none';
                document.getElementById('TdPA').style.display='inline';
                document.getElementById('TdPA1').style.display='inline';
                document.getElementById('TdPA2').style.display='inline';
                document.getElementById('TdPA3').style.display='inline';
                document.getElementById('TrMrgnHoldback').style.display='none';
                document.getElementById('TrOwnAccount').style.display='none';
            }
            else if(objTypeVal=="MP")
            {
                document.getElementById('TrCliPayout1').style.display='none';
                document.getElementById('TrCliPayout2').style.display='none';
                document.getElementById('TrCliPayout3').style.display='inline';
                document.getElementById('TrCliPayoutBranchGroup').style.display='none';
                document.getElementById('TrClient').style.display='none';
                document.getElementById('TrInterSeg').style.display='none';
                document.getElementById('TrIntSeg').style.display='none';
                document.getElementById('TrMktPayIN').style.display='inline';
                document.getElementById('TrPayInOwnAcc').style.display='none';
                document.getElementById('TrGrid').style.display='none';
                document.getElementById('TrPross').style.display='none';
                document.getElementById('TrHoldBackSett').style.display='none';
                document.getElementById('TrHoldSettType').style.display='none';
                document.getElementById('TrPayInOblg').style.display='none';
                document.getElementById('TrPayInFromMarginHoldBack').style.display='none';
                document.getElementById('TrOffSetSettlement').style.display='none';
                document.getElementById('TrOffSetSettlementSource').style.display='none';
                document.getElementById('TrOffSetPosition').style.display='none';
                document.getElementById('TrMrktPayINPool').style.display='inline';
                document.getElementById('TrMrktPayINPool1').style.display='inline';
                document.getElementById('TrInterSettSourceAcc').style.display='none';
                document.getElementById('TrInterSettTargetAcc').style.display='none';
                document.getElementById('TdPA').style.display='none';
                document.getElementById('TdPA1').style.display='none';
                document.getElementById('TdPA2').style.display='none';
                document.getElementById('TdPA3').style.display='none';
                document.getElementById('TrMrgnHoldback').style.display='none';
                document.getElementById('TrOwnAccount').style.display='none';
            }
            else if(objTypeVal=="PO")
            {
                document.getElementById('TrCliPayout1').style.display='none';
                document.getElementById('TrCliPayout2').style.display='none';
                document.getElementById('TrCliPayout3').style.display='none';
                document.getElementById('TrCliPayoutBranchGroup').style.display='none';
                document.getElementById('TrClient').style.display='none';
                document.getElementById('TrInterSeg').style.display='inline';
                document.getElementById('TrIntSeg').style.display='none';
                document.getElementById('TrMktPayIN').style.display='none';
                document.getElementById('TrPayInOwnAcc').style.display='inline';
                document.getElementById('TrGrid').style.display='none';
                document.getElementById('TrPross').style.display='none';
                document.getElementById('TrHoldBackSett').style.display='none';
                document.getElementById('TrHoldSettType').style.display='none';
                document.getElementById('TrPayInOblg').style.display='none';
                document.getElementById('TrPayInFromMarginHoldBack').style.display='none';
                document.getElementById('TrOffSetSettlement').style.display='none';
                document.getElementById('TrOffSetSettlementSource').style.display='none';
                document.getElementById('TrOffSetPosition').style.display='none';
                document.getElementById('TrMrktPayINPool').style.display='none';
                document.getElementById('TrMrktPayINPool1').style.display='none';
                document.getElementById('TrInterSettSourceAcc').style.display='none';
                document.getElementById('TrInterSettTargetAcc').style.display='none';
                document.getElementById('TdPA').style.display='none';
                document.getElementById('TdPA1').style.display='none';
                document.getElementById('TdPA2').style.display='none';
                document.getElementById('TdPA3').style.display='none';
                document.getElementById('TrMrgnHoldback').style.display='none';
                document.getElementById('TrOwnAccount').style.display='inline';
            }
            else if(objTypeVal=="PH")
            {
                document.getElementById('TrCliPayout1').style.display='none';
                document.getElementById('TrCliPayout2').style.display='none';
                document.getElementById('TrCliPayout3').style.display='none';
                document.getElementById('TrCliPayoutBranchGroup').style.display='inline';
                document.getElementById('TrClient').style.display='none';
                document.getElementById('TrInterSeg').style.display='inline';
                document.getElementById('TrIntSeg').style.display='none';
                document.getElementById('TrMktPayIN').style.display='none';
                document.getElementById('TrPayInOwnAcc').style.display='none';
                document.getElementById('TrGrid').style.display='none';
                document.getElementById('TrPross').style.display='none';
                document.getElementById('TrHoldBackSett').style.display='inline';
                document.getElementById('TrHoldSettType').style.display='inline';
                document.getElementById('TrPayInOblg').style.display='inline';
                document.getElementById('TrPayInFromMarginHoldBack').style.display='inline';
                document.getElementById('radSelectedHoldSettType').checked=true;
                document.getElementById('TdHoldSettType').style.display='inline';
                document.getElementById('TrOffSetSettlement').style.display='none';
                document.getElementById('TrOffSetSettlementSource').style.display='none';
                document.getElementById('TrOffSetPosition').style.display='none';
                document.getElementById('TrMrktPayINPool').style.display='none';
                document.getElementById('TrMrktPayINPool1').style.display='none';
                document.getElementById('TrInterSettSourceAcc').style.display='none';
                document.getElementById('TrInterSettTargetAcc').style.display='none';
                document.getElementById('TdPA').style.display='none';
                document.getElementById('TdPA1').style.display='none';
                document.getElementById('TdPA2').style.display='none';
                document.getElementById('TdPA3').style.display='none';
                document.getElementById('TrMrgnHoldback').style.display='inline';
                document.getElementById('TrOwnAccount').style.display='none';
            }
            else if(objTypeVal=="HR")
            {
                document.getElementById('TrCliPayout1').style.display='inline';
                document.getElementById('TrCliPayout2').style.display='inline';
                document.getElementById('TrCliPayout3').style.display='none';
                document.getElementById('TrCliPayoutBranchGroup').style.display='inline';
                document.getElementById('TrClient').style.display='inline';
                document.getElementById('TrInterSeg').style.display='none';
                document.getElementById('TrIntSeg').style.display='none';
                document.getElementById('TrMktPayIN').style.display='none';
                document.getElementById('TrPayInOwnAcc').style.display='none';
                document.getElementById('TrGrid').style.display='none';
                document.getElementById('TrPross').style.display='none';
                document.getElementById('TrHoldBackSett').style.display='none';
                document.getElementById('TrHoldSettType').style.display='none';
                document.getElementById('TrPayInOblg').style.display='none';
                document.getElementById('TrPayInFromMarginHoldBack').style.display='none';                
                document.getElementById('TrOffSetSettlement').style.display='none';
                document.getElementById('TrOffSetSettlementSource').style.display='none';
                document.getElementById('TrOffSetPosition').style.display='none';
                document.getElementById('TrMrktPayINPool').style.display='none';
                document.getElementById('TrMrktPayINPool1').style.display='none';
                document.getElementById('TrInterSettSourceAcc').style.display='none';
                document.getElementById('TrInterSettTargetAcc').style.display='none';
                document.getElementById('TdPA').style.display='inline';
                document.getElementById('TdPA1').style.display='inline';
                document.getElementById('TdPA2').style.display='inline';
                document.getElementById('TdPA3').style.display='inline';
                document.getElementById('TrMrgnHoldback').style.display='none';
                document.getElementById('TrOwnAccount').style.display='none';
            }
            if(objTypeVal=="OF")
            {
                document.getElementById('TrCliPayout1').style.display='none';
                document.getElementById('TrCliPayout2').style.display='none';
                document.getElementById('TrCliPayout3').style.display='none';
                document.getElementById('TrCliPayoutBranchGroup').style.display='inline';
                document.getElementById('TrClient').style.display='none';
                document.getElementById('TrInterSeg').style.display='none';
                document.getElementById('TrIntSeg').style.display='none';
                document.getElementById('TrMktPayIN').style.display='none';
                document.getElementById('TrPayInOwnAcc').style.display='none';
                document.getElementById('TrGrid').style.display='none';
                document.getElementById('TrPross').style.display='none';
                document.getElementById('TrHoldBackSett').style.display='none';
                document.getElementById('TrHoldSettType').style.display='none';
                document.getElementById('TrPayInOblg').style.display='none';                
                document.getElementById('TrOffSetSettlement').style.display='inline';
                document.getElementById('TrOffSetSettlementSource').style.display='inline';
                document.getElementById('TrOffSetPosition').style.display='inline';
                document.getElementById('TrMrktPayINPool').style.display='none';
                document.getElementById('TrMrktPayINPool1').style.display='none';
                document.getElementById('TrInterSettSourceAcc').style.display='none';
                document.getElementById('TrInterSettTargetAcc').style.display='none';
                document.getElementById('TdPA').style.display='none';
                document.getElementById('TdPA1').style.display='none';
                document.getElementById('TdPA2').style.display='none';
                document.getElementById('TdPA3').style.display='none';
                document.getElementById('TrMrgnHoldback').style.display='none';
                document.getElementById('TrOwnAccount').style.display='none';
            }
        }
        function Visible(obj)
        {
            if(obj=='a')
            {
                alert('No Record Found !!');
                document.getElementById('TrGrid').style.display='none';
                document.getElementById('TrPross').style.display='none';
            }
            else
            {
                document.getElementById('TrGrid').style.display='inline';
                document.getElementById('TrPross').style.display='inline';
            }
        }
        function SettHold(obj)
        {
            if(obj=='a')
            {
                document.getElementById('TdHoldSett').style.display='none';
            }
            else
            {
                document.getElementById('TdHoldSett').style.display='inline';
            }
        }
        function SettHoldType(obj)
        {
            if(obj=='a')
            {
                document.getElementById('TdHoldSettType').style.display='none';
            }
            else
            {
                document.getElementById('TdHoldSettType').style.display='inline';
            }
        }
        function showPayInType(obj)
        {
            var objNsdlCdsl=obj.split('~')
            if(objNsdlCdsl[1]=="NSDL")
                document.getElementById('TrMrktPayINPool1').style.display='inline';
            else
                document.getElementById('TrMrktPayINPool1').style.display='none';
        }
        function selecttion()
        {
            var combo=document.getElementById('ddlExport');
            combo.value='Ex';
        }
        FieldName='lstSuscriptions';
        document.body.style.cursor = 'pointer'; 
        var oldColor = '';
	    function ChangeRowColor(rowID,rowNumber) 
        { 
//            var objval=document.getElementById("grdDematProcessing_ctl04_lblID");
//            alert(objval);
            var gridview = document.getElementById('grdDematProcessing'); 
            var rCount = gridview.rows.length; 
            var rowIndex=1;
            var rowCount=0;
            if(rCount==28)
                 rowCount=25;
            else    
               rowCount=rCount-2;
            if(rowNumber>25 && rCount<28)
                rowCount=rCount-3;
            for (rowIndex; rowIndex<=rowCount; rowIndex++) 
            { 
                var rowElement = gridview.rows[rowIndex]; 
                rowElement.style.backgroundColor='#FFFFFF'
            }
            var color = document.getElementById(rowID).style.backgroundColor;
            if(color != '#ffe1ac') 
            {
                oldColor = color;
            }
            if(color == '#ffe1ac') 
            {
                document.getElementById(rowID).style.backgroundColor = oldColor;
            }
            else 
                document.getElementById(rowID).style.backgroundColor = '#ffe1ac';            
          
            CallServer('DeliveryProfile~'+rowNumber+'',"");
        }         
    </script>

    <script type="text/ecmascript">
        function ReceiveServerData(rValue)
        {
            var DATA=rValue.split('~');
            if(DATA[0]=="DeliveryProfile")
            {
                if(DATA[1]=="NA")
                    document.getElementById('TrDeliveryShow').style.display='none';
                else
                {
                    document.getElementById('spanLedgerClear').innerText=DATA[1];
                    document.getElementById('spanMarginClear').innerText=DATA[2];
                    document.getElementById('spanLedgerClosing').innerText=DATA[3];
                    document.getElementById('spanMarginClosing').innerText=DATA[4];
                    document.getElementById('spanProfileName').innerText=DATA[5];
                    document.getElementById('spanTotalDelVal').innerText=DATA[6];
                    document.getElementById('TrDeliveryShow').style.display='inline';
                    height();
                }
            }
            else
            {
                var btn = document.getElementById('btnhide');

                if(DATA[0]=='Branch')
                {
                    document.getElementById('HiddenField_Branch').value = DATA[1];
                }
                if(DATA[0]=='Group')
                {
                    btn.click();
                    document.getElementById('HiddenField_Group').value = DATA[1];
                }  
                if(DATA[0]=='Clients')
                {
                    document.getElementById('HiddenField_Client').value = DATA[1];
                } 
                if(DATA[0]=='Scrips')
                {
                    document.getElementById('HiddenField_Scrip').value = DATA[1];
                }
            }
        }
    </script>
       <script language="javascript" type="text/javascript">
           var prm = Sys.WebForms.PageRequestManager.getInstance();
           prm.add_initializeRequest(InitializeRequest);
           prm.add_endRequest(EndRequest);
           var postBackElement;
           function InitializeRequest(sender, args) {
               if (prm.get_isInAsyncPostBack())

                   args.set_cancel(true);
               postBackElement = args.get_postBackElement();
               $get('UpdateProgress1').style.display = 'block';

           }
           function EndRequest(sender, args) {
               $get('UpdateProgress1').style.display = 'none';
           }
            </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>
         

         
            <table class="TableMain100">
                <tr>
                    <td class="EHEADER" style="text-align: center;">
                        <strong><span style="color: #000099">Delivery Processing</span></strong>
                    </td>
                </tr>
            </table>
            <table class="TableMain100">
                <tr id="TrAll">
                    <td style="text-align: left; vertical-align: top;">
                        <table border="0">
                            <tr>
                                <td class="gridcellleft">
                                    Type
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlType" Font-Size="12px" runat="server" onchange="TypeChange(this.value)"
                                        Width="210px">
                                        <asp:ListItem Value="IS">Inter Settlement</asp:ListItem>
                                       <%-- <asp:ListItem Value="PH">Payin From Margin/HoldBack</asp:ListItem>--%>
                                       <%-- <asp:ListItem Value="PO">Payin From Own Account</asp:ListItem>--%>
                                        <asp:ListItem Value="MP">Market PayIn</asp:ListItem>
                                       <%-- <asp:ListItem Value="CP">Client PayOut</asp:ListItem>--%>
                                       <%-- <asp:ListItem Value="MR">Margin Release</asp:ListItem>
                                        <asp:ListItem Value="HR">HoldBack Release</asp:ListItem>--%>
                                        <asp:ListItem Value="OF">Offset Position</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr id="TrCliPayoutBranchGroup">
                                <td class="gridcellleft">
                                    Group By</td>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlGroup" runat="server" Width="80px" Font-Size="12px" onchange="fnddlGroup(this.value)">
                                                    <asp:ListItem Value="0">Branch</asp:ListItem>
                                                    <asp:ListItem Value="1">Group</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td id="td_branch">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="rdbranchAll" runat="server" Checked="True" GroupName="ac" onclick="fnBranch('a')" />
                                                            All
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbranchSelected" runat="server" GroupName="ac" onclick="fnBranch('b')" />Selected
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td id="td_group" style="display: none;">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList ID="ddlgrouptype" runat="server" Font-Size="12px" onchange="fngrouptype(this.value)">
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnhide" EventName="Click"></asp:AsyncPostBackTrigger>
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td id="td_allselect" style="display: none;">
                                                            <asp:RadioButton ID="rdddlgrouptypeAll" runat="server" Checked="True" GroupName="bc"
                                                                onclick="fnGroup('a')" />
                                                            All
                                                            <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="bc" onclick="fnGroup('b')" />Selected
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="TrCliPayout1">
                                <td class="gridcellleft">
                                    Client
                                </td>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="radClientAll" runat="server" Checked="true" GroupName="a" onclick="Client('a')" />
                                            </td>
                                            <td>
                                                All</td>
                                            <td>
                                                <asp:RadioButton ID="radClientSelected" runat="server" GroupName="a" onclick="Client('b')" />
                                            </td>
                                            <td>
                                                Selected</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                            <tr id="TrCliPayout2">
                                <td class="gridcellleft">
                                    Scrip
                                </td>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="radScripAll" runat="server" Checked="true" GroupName="b" onclick="Scrip('a')" />
                                            </td>
                                            <td>
                                                All</td>
                                            <td>
                                                <asp:RadioButton ID="radScripSelected" runat="server" GroupName="b" onclick="Scrip('b')" />
                                            </td>
                                            <td>
                                                Selected</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="TrInterSeg">
                                <td class="gridcellleft">
                                    Payin Date
                                </td>
                                <td colspan="2">
                                    <dxe:ASPxDateEdit ID="dtPayindate" runat="server" Font-Size="12px" Width="206px"
                                        EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                    </dxe:ASPxDateEdit>
                                </td>
                            </tr>
                            <tr id="TrOffSetSettlement">
                                <td class="gridcellleft">
                                    Target Settlement
                                </td>
                                <td style="text-align: left;" colspan="2">
                                    <asp:TextBox ID="txtTargetSettOff" Font-Size="12px" runat="server" onkeyup="FunSettNumber(this,'ShowClientScrip',event)"
                                        Width="206px"></asp:TextBox>
                                    <asp:HiddenField ID="txtTargetSettOff_hidden" runat="server" />
                                </td>
                            </tr>
                            <tr id="TrOffSetSettlementSource">
                                <td class="gridcellleft">
                                    Source Settlement
                                </td>
                                <td style="text-align: left;" colspan="2">
                                    <asp:TextBox ID="txtSourceSettOff" Font-Size="12px" runat="server" onkeyup="FunSettNumber(this,'ShowClientScrip',event)"
                                        Width="206px"></asp:TextBox>
                                    <asp:HiddenField ID="txtSourceSettOff_hidden" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    Transfer Date
                                </td>
                                <td>
                                    <dxe:ASPxDateEdit ID="DtTransferDate" runat="server" Font-Size="12px" Width="206px"
                                        EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td>
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                        <ProgressTemplate>
                                            <div id='Div2' style='position: absolute; font-family: arial; font-size: 30; background-color: white;
                                                layer-background-color: white;'>
                                                <table border='1' cellpadding='0' cellspacing='0' bordercolor='#C0D6E4'>
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td height='25' align='center' bgcolor='#FFFFFF'>
                                                                        <img src='/assests/images/progress.gif' width='18' height='18'></td>
                                                                    <td height='10' width='100%' align='center' bgcolor='#FFFFFF'>
                                                                        <font size='2' face='Tahoma'><strong align='center'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Loading..</strong></font></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </td>
                            </tr>
                            <tr id="TrPayInOblg">
                                <td colspan="3" class="gridcellleft">
                                    <b>Consider Payin Obligation for :</b>
                                </td>
                            </tr>
                            <tr id="TrHoldBackSett">
                                <td class="gridcellleft">
                                    Settlement Number
                                </td>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="radAllHold" runat="server" GroupName="AS" onclick="SettHold('a')" />
                                            </td>
                                            <td>
                                                All</td>
                                            <td>
                                                <asp:RadioButton ID="radSelectedHold" runat="server" Checked="true" GroupName="AS"
                                                    onclick="SettHold('b')" />
                                            </td>
                                            <td>
                                                Selected</td>
                                            <td id="TdHoldSett">
                                                <asp:TextBox ID="txtSettNumberHoldBack" runat="server" onkeyup="FunForSettNum(this,'SearchSettlementNumber',event)"
                                                    Font-Size="12px"></asp:TextBox>
                                                <asp:HiddenField ID="txtSettNumberHoldBack_hidden" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="TrHoldSettType">
                                <td class="gridcellleft">
                                    Settlement Type
                                </td>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="radAllHoldSettType" runat="server" GroupName="AS1" onclick="SettHoldType('a')" />
                                            </td>
                                            <td>
                                                All</td>
                                            <td>
                                                <asp:RadioButton ID="radSelectedHoldSettType" runat="server" Checked="true" GroupName="AS1"
                                                    onclick="SettHoldType('b')" />
                                            </td>
                                            <td>
                                                Selected</td>
                                            <td id="TdHoldSettType">
                                                <asp:TextBox ID="txtSettTypeHoldBack" runat="server" onkeyup="FunForSettType(this,'SearchSettlementType',event)"
                                                    Font-Size="12px"></asp:TextBox>
                                                <asp:HiddenField ID="txtSettTypeHoldBack_hidden" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="TrInterSettSourceAcc">
                                <td class="gridcellleft">
                                    Source Account
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlSourceAccount" Font-Size="12px" runat="server" Width="310px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="TrInterSettTargetAcc">
                                <td class="gridcellleft">
                                    Target Account
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlTargetAccount" Font-Size="12px" runat="server" Width="310px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="TrMrktPayINPool">
                                <td class="gridcellleft">
                                    Pool A/C
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlPoolAC" Font-Size="12px" runat="server" onchange="showPayInType(this.value)"
                                        Width="210px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="TrMrktPayINPool1">
                                <td class="gridcellleft">
                                    PayIN Type
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlPayInType" Font-Size="12px" runat="server" Width="210px">
                                        <asp:ListItem Value="N">Normal</asp:ListItem>
                                        <asp:ListItem Value="EP">Early PayIN</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="TrCliPayout3">
                                <td class="gridcellleft">
                                    Settlement
                                </td>
                                <td style="text-align: left;" colspan="2">
                                    <asp:TextBox ID="txtSettlementNumber" Font-Size="12px" runat="server" onkeyup="FunSettNumber(this,'ShowClientScrip',event)"
                                        Width="206px"></asp:TextBox>
                                    <asp:HiddenField ID="txtSettlementNumber_hidden" runat="server" />
                                </td>
                            </tr>
                            
                             <tr id="TrMrgnHoldback">
                                <td class="gridcellleft">
                                    Account
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="drpMarginHoldbackAccount" Font-Size="12px" runat="server" Width="210px">                                        
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="TrOwnAccount">
                                <td class="gridcellleft">
                                    Account
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="drpOwnAccount" Font-Size="12px" runat="server" Width="210px">                                        
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="TrButton">
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="btnUpdate" Height="22px"
                                                    Width="106px" OnClientClick="javascript:selecttion();" OnClick="btnShow_Click" />
                                            </td>
                                            <td id="TdPA">
                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/green.bmp"/>
                                            </td>
                                            <td class="gridcellleft"  id="TdPA1">
                                                POA Client
                                            </td>
                                            <td id="TdPA2">
                                                <asp:Image ID="Image2" runat="server" ImageUrl="~/images/blue.bmp"/>
                                            </td>
                                            <td class="gridcellleft" id="TdPA3">
                                                Own DP Client
                                            </td>
                                            <td id="TrExport" style="text-align: right">
                                                <asp:DropDownList ID="ddlExport" runat="server" AutoPostBack="True" Font-Size="12px"
                                                    OnSelectedIndexChanged="ddlExport_SelectedIndexChanged">
                                                    <asp:ListItem Value="Ex" Selected="True">Export</asp:ListItem>
                                                    <asp:ListItem Value="E">Excel</asp:ListItem>
                                                    <asp:ListItem Value="P">PDF</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td id="TrDeliveryShow" style="vertical-align: top; text-align: left">
                        <table style="border: solid 1px blue;">
                            <tr>
                                <td>
                                    Profile Name :
                                </td>
                                <td>
                                    <span id="spanProfileName"></span>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    Ledger Account :
                                </td>
                                <td>
                                    Margin Account
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Clear Balance :
                                </td>
                                <td>
                                    <span id="spanLedgerClear"></span>
                                </td>
                                <td>
                                    <span id="spanMarginClear"></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Closing Balance :
                                </td>
                                <td>
                                    <span id="spanLedgerClosing"></span>
                                </td>
                                <td>
                                    <span id="spanMarginClosing"></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Total Value Of Delivery :
                                </td>
                                <td>
                                    <span id="spanTotalDelVal"></span>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="text-align: right; vertical-align: top;">
                        <table width="100%" id="showFilter">
                            <tr>
                                <td style="text-align: right; vertical-align: top">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td class="gridcellleft" style="vertical-align: top; text-align: left" id="TdFilter">
                                                            <asp:TextBox ID="txtsubscriptionID" runat="server" Font-Size="12px" Width="150px"
                                                                onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox>
                                                            <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                                Enabled="false">
                                                                <asp:ListItem>Clients</asp:ListItem>
                                                                <asp:ListItem>Scrips</asp:ListItem>
                                                                <asp:ListItem>Branch</asp:ListItem>
                                                                <asp:ListItem>Group</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                                                style="color: #009900; text-decoration: underline; font-size: 8pt;">Add List</span></a><span
                                                                    style="color: #009900; font-size: 8pt;"> </span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:ListBox ID="lstSuscriptions" runat="server" Font-Size="12px" Height="90px" Width="290px">
                                                </asp:ListBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="height: 14px">
                                                            <a id="A2" href="javascript:void(0);" onclick="clientselectionfinal()"><span style="color: #000099;
                                                                text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
                                                        </td>
                                                        <td style="height: 14px">
                                                            <a id="A1" href="javascript:void(0);" onclick="btnRemovefromsubscriptionlist_click()">
                                                                <span style="color: #cc3300; text-decoration: underline; font-size: 8pt;">Remove</span></a>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td>
                                    <asp:TextBox ID="txtsubscriptionID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                    <asp:HiddenField ID="HiddenField_Group" runat="server" />
                                    <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                                    <asp:HiddenField ID="HiddenField_Client" runat="server" />
                                    <asp:HiddenField ID="HiddenField_Scrip" runat="server" />
                                      <asp:Button ID="btnhide" runat="server" Text="btnhide" OnClick="btnhide_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="TrGrid">
                    <td colspan="2">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="Panel1" runat="server" Height="420px" ScrollBars="Vertical">
                                    <table width="100%">
                                        <tr id="TrClient">
                                            <td>
                                                <asp:GridView ID="grdDematProcessing" runat="server" Width="100%" BorderColor="CornflowerBlue"
                                                    ShowFooter="True" AllowSorting="true" AutoGenerateColumns="false" BorderStyle="Solid"
                                                    BorderWidth="2px" CellPadding="4" ForeColor="#0000C0" OnRowDataBound="grdDematProcessing_RowDataBound"
                                                    OnRowCreated="grdDematProcessing_RowCreated" OnSorting="grdDematProcessing_Sorting"
                                                    OnRowCommand="grdDematProcessing_RowCommand">
                                                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                                                    <Columns>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Center"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("CustomerID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Client" SortExpression="Client">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblClient" runat="server" Text='<%# Eval("Client")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="UCC" SortExpression="UCC">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUCC" runat="server" Text='<%# Eval("UCC")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Branch Code" SortExpression="BranchCode">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBranchCode" runat="server" Text='<%# Eval("BranchCode")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Scrip" SortExpression="Scrip">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblScrip" runat="server" Text='<%# Eval("Scrip")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ISIN">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblISIN" runat="server" Text='<%# Eval("ISIN")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="To Deliver">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQtyDeliver" runat="server" Text='<%# Eval("QtyToDeliver")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Deliverable">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtStock" Font-Size="12px" Width="60px" runat="server" Text='<%# Eval("Stock") %>'
                                                                    onKeyUp="javascript:checkNumber(this);"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="From">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFromAccount" runat="server" Text='<%# Eval("FromAccount")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Deliver To" Visible="false">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:DropDownList Font-Size="12px" ID="ddlDeliverTo" SelectedValue='<%# Eval("AccType")%>'
                                                                    runat="server">
                                                                    <asp:ListItem Value="CA">Client A/c</asp:ListItem>
                                                                    <asp:ListItem Value="MA">Margin A/c</asp:ListItem>
                                                                    <asp:ListItem Value="HA">HoldBack A/c</asp:ListItem>
                                                                    <asp:ListItem Value="OW">Own A/c</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <%--<asp:Button ID="btnDelv" runat="server" Text="Go" CommandName="DelvTo" CommandArgument='<%# Eval("CustomerID") %>'
                                                                     Width="20px" />--%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="AccountName">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:DropDownList Font-Size="12px" ID="ddlAccountName" runat="server" Width="250px">
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Deliverable" Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblStock" runat="server" Text='<%# Eval("Stock")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ISIN" Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProductID" runat="server" Text='<%# Eval("ProductID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDelvOutgoingDelivery" runat="server" Text='<%# Eval("DelvOutgoingDelivery")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDelvDesignatedBenAc" runat="server" Text='<%# Eval("DelvDesignatedBenAc")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDelvHoldBackEntireAC" runat="server" Text='<%# Eval("DelvHoldBackEntireAC")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDelvMarkUpOnDebitBalance" runat="server" Text='<%# Eval("DelvMarkUpOnDebitBalance")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDelvLedgerClearBalance" runat="server" Text='<%# Eval("DelvLedgerClearBalance")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDelvMarginClearBalance" runat="server" Text='<%# Eval("DelvMarginClearBalance")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDelvLedgerClosingBalance" runat="server" Text='<%# Eval("DelvLedgerClosingBalance")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDelvMarginClosingBalance" runat="server" Text='<%# Eval("DelvMarginClosingBalance")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTotalDeliveryValue" runat="server" Text='<%# Eval("TotalDeliveryValue")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAccType" runat="server" Text='<%# Eval("AccType")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAccName" runat="server" Text='<%# Eval("AccName")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbSourceAccID" runat="server" Text='<%# Eval("AccountID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSettNumber" runat="server" Text='<%# Eval("SettNumber")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSettType" runat="server" Text='<%# Eval("SettType")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblColourType" runat="server" Text='<%# Eval("ColourType")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ChkDelivery" runat="server" onclick="checkValForTextBox(this.id)"/>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="cbSelectAll" runat="server" />
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle BackColor="White" ForeColor="#330099" BorderColor="#BFD3EE" BorderStyle="Double"
                                                        BorderWidth="1px"></RowStyle>
                                                    <EditRowStyle BackColor="#E59930"></EditRowStyle>
                                                    <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
                                                    <PagerStyle ForeColor="White" HorizontalAlign="Center"></PagerStyle>
                                                    <HeaderStyle ForeColor="Black" BorderWidth="1px" CssClass="EHEADER" BorderColor="AliceBlue"
                                                        Font-Bold="False"></HeaderStyle>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr id="TrIntSeg">
                                            <td>
                                                <asp:GridView ID="GrdInterSegment" runat="server" Width="100%" BorderColor="CornflowerBlue"
                                                    ShowFooter="True" AllowSorting="true" AutoGenerateColumns="false" BorderStyle="Solid"
                                                    BorderWidth="2px" CellPadding="4" ForeColor="#0000C0" OnRowDataBound="GrdInterSegment_RowDataBound"
                                                    OnSorting="GrdInterSegment_Sorting">
                                                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                                                    <Columns>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Center"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCustID" runat="server" Text='<%# Eval("CustomerID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Client" SortExpression="CustomerName">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblClientName" runat="server" Text='<%# Eval("CustomerName")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Scrip" SortExpression="ProductName">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblScripName" runat="server" Text='<%# Eval("ProductName")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Settl.Source" SortExpression="SettlementS">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSettSource" runat="server" Text='<%# Eval("SettlementS")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Pending Outgoing">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPendOutgoing" runat="server" Text='<%# Eval("OutGoing")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Settl. Target">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSettTarget" runat="server" Text='<%# Eval("SettlementT")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Pending Incoming">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPendgIncoming" runat="server" Text='<%# Eval("Incoming")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="AdJustment">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtAdjst" Font-Size="12px" Width="60px" runat="server" Text='<%# Eval("AddJust") %>'
                                                                    onKeyUp="javascript:checkNumber(this);"></asp:TextBox>
										<asp:Label ID="lblStocksAdjustForCheck" Visible="false" runat="server" Text='<%# Eval("StocksAdjustForCheck")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Deliverable" Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAccountid" runat="server" Text='<%# Eval("AccountId")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ISIN" Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProductID" runat="server" Text='<%# Eval("ProductSeriesID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ISIN" Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblISIN" runat="server" Text='<%# Eval("ISIN")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ChkDelivery" runat="server" onclick="checkValForTextBox(this.id)" />
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="cbSelectAll" runat="server" />
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle BackColor="White" ForeColor="#330099" BorderColor="#BFD3EE" BorderStyle="Double"
                                                        BorderWidth="1px"></RowStyle>
                                                    <EditRowStyle BackColor="#E59930"></EditRowStyle>
                                                    <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
                                                    <PagerStyle ForeColor="White" HorizontalAlign="Center"></PagerStyle>
                                                    <HeaderStyle ForeColor="Black" BorderWidth="1px" CssClass="EHEADER" BorderColor="AliceBlue"
                                                        Font-Bold="False"></HeaderStyle>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr id="TrMktPayIN">
                                            <td>
                                                <asp:GridView ID="GrdMarketPayIN" runat="server" Width="100%" BorderColor="CornflowerBlue"
                                                    ShowFooter="True" AllowSorting="true" AutoGenerateColumns="false" BorderStyle="Solid"
                                                    BorderWidth="2px" CellPadding="4" ForeColor="#0000C0" OnRowDataBound="GrdMarketPayIN_RowDataBound"
                                                    OnSorting="GrdMarketPayIN_Sorting">
                                                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Exchange Name">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblClientName" runat="server" Text='<%# Eval("CustomerID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Scrip" SortExpression="Scrip">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblScripName" runat="server" Text='<%# Eval("Scrip")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ISIN" SortExpression="ISIN">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblISIN" runat="server" Text='<%# Eval("ISIN")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Qty To Deliver">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDeliver" runat="server" Text='<%# Eval("Deliver")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Deliverable">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtDeliverable" Font-Size="12px" Width="60px" runat="server" Text='<%# Eval("Deliverable") %>'
                                                                    onKeyUp="javascript:checkNumber(this);"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProductSeriesid" runat="server" Text='<%# Eval("ProductSeriesID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAccountID" runat="server" Text='<%# Eval("AccountID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ChkDelivery" runat="server" onclick="checkValForTextBox(this.id)" />
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="cbSelectAll" runat="server" />
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle BackColor="White" ForeColor="#330099" BorderColor="#BFD3EE" BorderStyle="Double"
                                                        BorderWidth="1px"></RowStyle>
                                                    <EditRowStyle BackColor="#E59930"></EditRowStyle>
                                                    <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
                                                    <PagerStyle ForeColor="White" HorizontalAlign="Center"></PagerStyle>
                                                    <HeaderStyle ForeColor="Black" BorderWidth="1px" CssClass="EHEADER" BorderColor="AliceBlue"
                                                        Font-Bold="False"></HeaderStyle>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr id="TrPayInOwnAcc">
                                            <td>
                                                <asp:GridView ID="grdPayInOwnAcc" runat="server" Width="100%" BorderColor="CornflowerBlue"
                                                    ShowFooter="True" AllowSorting="true" AutoGenerateColumns="false" BorderStyle="Solid"
                                                    BorderWidth="2px" CellPadding="4" ForeColor="#0000C0" OnRowDataBound="grdPayInOwnAcc_RowDataBound"
                                                    OnSorting="grdPayInOwnAcc_Sorting">
                                                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                                                    <Columns>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Center"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCliID" runat="server" Text='<%# Eval("CustomerID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Scrip" SortExpression="Scrip">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblScrip" runat="server" Text='<%# Eval("Scrip")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ISIN" SortExpression="ISIN">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblISIN" runat="server" Text='<%# Eval("ISIN")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Settl.Number" SortExpression="SettNumber">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSettNumber" runat="server" Text='<%# Eval("SettNumber")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="To Receivable">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblIncoming" runat="server" Text='<%# Eval("Incoming")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="To Transferable">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtTransferable" Font-Size="12px" Width="60px" runat="server" Text='<%# Eval("Transferable") %>'
                                                                    onKeyUp="javascript:checkNumber(this);"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="From A/C">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSourceAccName" runat="server" Text='<%# Eval("SourceAccName")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="To A/C">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:DropDownList Font-Size="12px" ID="ddlAccountName" runat="server" Width="250px">
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Deliverable" Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProductSeriesID" runat="server" Text='<%# Eval("ProductSeriesID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ISIN" Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAccountID" runat="server" Text='<%# Eval("AccountID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ChkDelivery" runat="server" onclick="checkValForTextBox(this.id)" />
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="cbSelectAll" runat="server" />
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="AlternateProductSeriesID" Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAlternateProductSeriesID" runat="server" Text='<%# Eval("AlternateProductSeriesID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="DammyAccID" Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDammyAccID" runat="server" Text='<%# Eval("DammyAccID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Type" Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblType" runat="server" Text='<%# Eval("Type")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle BackColor="White" ForeColor="#330099" BorderColor="#BFD3EE" BorderStyle="Double"
                                                        BorderWidth="1px"></RowStyle>
                                                    <EditRowStyle BackColor="#E59930"></EditRowStyle>
                                                    <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
                                                    <PagerStyle ForeColor="White" HorizontalAlign="Center"></PagerStyle>
                                                    <HeaderStyle ForeColor="Black" BorderWidth="1px" CssClass="EHEADER" BorderColor="AliceBlue"
                                                        Font-Bold="False"></HeaderStyle>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr id="TrPayInFromMarginHoldBack">
                                            <td>
                                                <asp:GridView ID="grdPayInFromMarginHoldBack" runat="server" Width="100%" BorderColor="CornflowerBlue"
                                                    ShowFooter="True" AllowSorting="true" AutoGenerateColumns="false" BorderStyle="Solid"
                                                    BorderWidth="2px" CellPadding="4" ForeColor="#0000C0" OnRowDataBound="grdPayInFromMarginHoldBack_RowDataBound"
                                                    OnSorting="grdPayInFromMarginHoldBack_Sorting">
                                                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                                                    <Columns>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Center"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCliID" runat="server" Text='<%# Eval("CustomerID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Client Name" SortExpression="CustomerName">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval("CustomerName")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Scrip" SortExpression="Scrip">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblScrip" runat="server" Text='<%# Eval("Scrip")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ISIN" SortExpression="ISIN">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblISIN" runat="server" Text='<%# Eval("ISIN")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Settl.Number" SortExpression="SettNumType">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSettNumType" runat="server" Text='<%# Eval("SettNumType")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Pending Incoming">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblIncoming" runat="server" Text='<%# Eval("Incoming")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Transfer Qty">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtTransferable" Font-Size="12px" Width="60px" runat="server" Text='<%# Eval("Transferable") %>'
                                                                    onKeyUp="javascript:checkNumber(this);"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="From A/C">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSourceAccName" runat="server" Text='<%# Eval("SourceAccName")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="To A/C">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:DropDownList Font-Size="12px" ID="ddlAccountName" runat="server" Width="250px">
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Deliverable" Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProductSeriesID" runat="server" Text='<%# Eval("ProductSeriesID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ISIN" Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAccountID" runat="server" Text='<%# Eval("AccountID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="SettNumberS" Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSettNumberS" runat="server" Text='<%# Eval("SettNumberS")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="SettTypeS" Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSettTypeS" runat="server" Text='<%# Eval("SettTypeS")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="AlternateProductSeriesID" Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAlternateProductSeriesID" runat="server" Text='<%# Eval("AlternateProductSeriesID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="DammyAccID" Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDammyAccID" runat="server" Text='<%# Eval("DammyAccID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Type" Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblType" runat="server" Text='<%# Eval("Type")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ChkDelivery" runat="server" onclick="checkValForTextBox(this.id)" />
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="cbSelectAll" runat="server" />
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle BackColor="White" ForeColor="#330099" BorderColor="#BFD3EE" BorderStyle="Double"
                                                        BorderWidth="1px"></RowStyle>
                                                    <EditRowStyle BackColor="#E59930"></EditRowStyle>
                                                    <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
                                                    <PagerStyle ForeColor="White" HorizontalAlign="Center"></PagerStyle>
                                                    <HeaderStyle ForeColor="Black" BorderWidth="1px" CssClass="EHEADER" BorderColor="AliceBlue"
                                                        Font-Bold="False"></HeaderStyle>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr id="TrOffSetPosition">
                                            <td>
                                                <asp:GridView ID="grdOffSetPosition" runat="server" Width="100%" BorderColor="CornflowerBlue"
                                                    ShowFooter="True" AllowSorting="true" AutoGenerateColumns="false" BorderStyle="Solid"
                                                    BorderWidth="2px" CellPadding="4" ForeColor="#0000C0" OnRowDataBound="grdOffSetPosition_RowDataBound"
                                                    OnSorting="grdOffSetPosition_Sorting">
                                                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                                                    <Columns>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Center"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCliID" runat="server" Text='<%# Eval("CustomerID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Client Name" SortExpression="CustomerName">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval("CustomerName")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Scrip" SortExpression="ProductName">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblScrip" runat="server" Text='<%# Eval("ProductName")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ISIN" SortExpression="ISIN">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblISIN" runat="server" Text='<%# Eval("ISIN")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Qty">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQty" runat="server" Text='<%# Eval("Qty")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Sell Position Sett.">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTargetSettNumber" runat="server" Text='<%# Eval("TargetSettNumber")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Buy Position Sett.">
                                                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSourceSettNumber" runat="server" Text='<%# Eval("SourceSettNumber")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Deliverable" Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBranchID" runat="server" Text='<%# Eval("BranchID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ISIN" Visible="False">
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProductSeriesID" runat="server" Text='<%# Eval("ProductSeriesID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ChkDelivery" runat="server"/>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="cbSelectAll" runat="server" />
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle BackColor="White" ForeColor="#330099" BorderColor="#BFD3EE" BorderStyle="Double"
                                                        BorderWidth="1px"></RowStyle>
                                                    <EditRowStyle BackColor="#E59930"></EditRowStyle>
                                                    <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
                                                    <PagerStyle ForeColor="White" HorizontalAlign="Center"></PagerStyle>
                                                    <HeaderStyle ForeColor="Black" BorderWidth="1px" CssClass="EHEADER" BorderColor="AliceBlue"
                                                        Font-Bold="False"></HeaderStyle>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click"></asp:AsyncPostBackTrigger>
                                <asp:AsyncPostBackTrigger ControlID="btnProcessing" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr id="TrPross">
                    <td colspan="2">
                        <asp:Button ID="btnProcessing" runat="server" Text="Generate Transfers" CssClass="btnUpdate"
                            Height="25px" OnClientClick="javascript:selecttion();" OnClick="btnProcessing_Click"
                            Width="147px" />
                    </td>
                </tr>
            </table>
        </div>
  </asp:Content>