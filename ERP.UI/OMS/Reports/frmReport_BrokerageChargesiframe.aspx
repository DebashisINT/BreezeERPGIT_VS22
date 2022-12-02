<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_BrokerageChargesiframe" EnableEventValidation="false" Codebehind="frmReport_BrokerageChargesiframe.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>

    

    <style type="text/css">
		  .tableClass {
    /* border: 0px; */
    border: 1px solid #aaa !important;
    border-collapse: collapse !important;
}
.tableBorderClass {
    /* border: 0px; */
    border: 1px solid #aaa !important;

}	/* Big box with list of options */
	#ajax_listOfOptions{
		position:absolute;	/* Never change this one */
		width:50px;	/* Width of box */
		height:auto;	/* Height of box */
		overflow:auto;	/* Scrolling features */
		border:1px solid Blue;	/* Blue border */
		background-color:#FFF;	/* White background color */
		text-align:left;
		font-size:0.9em;
		z-index:32767;
	}
	#ajax_listOfOptions div{	/* General rule for both .optionDiv and .optionDivSelected */
		margin:1px;		
		padding:1px;
		cursor:pointer;
		font-size:0.9em;
	}
	#ajax_listOfOptions .optionDiv{	/* Div for each item in list */
		
	}
	#ajax_listOfOptions .optionDivSelected{ /* Selected item in the list */
		background-color:#DDECFE;
		color:Blue;
	}
	#ajax_listOfOptions_iframe{
		background-color:#F00;
		position:absolute;
		z-index:3000;
	}
	
	form{
		display:inline;
	}
	

	</style>

    <script language="javascript" type="text/javascript">

  
    function Page_Load()///Call Into Page Load
            {
                 Hide('showFilter');
                 Hide('td_filter');
                 Hide('Tr_Broker');
		         Show('Td_Specific');
                 document.getElementById('hiddencount').value=0;
                 fn_ReportView('1');
                 FnddlGeneration('1');
                 //Hide('Tab_showFilter');
                 height();
            }
   function height()
        {
            if(document.body.scrollHeight>=350)
            {
                window.frameElement.height = document.body.scrollHeight;
            }
            else
            {
                window.frameElement.height = '350px';
            }
            window.frameElement.width = document.body.scrollwidth;
        }
    function Hide(obj)
            {
             document.getElementById(obj).style.display='none';
            }
    function Show(obj)
            {
             document.getElementById(obj).style.display='inline';
            }
    
    function FunClientScrip(objID,objListFun,objEvent)
        {
          var cmbVal;
         
            if(document.getElementById('cmbsearchOption').value=="Clients")
            {
                if(document.getElementById('ddlGroup').value=="0" || document.getElementById('ddlGroup').value=="2")//////////////Group By  selected are branch
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
                    if(document.getElementById('ddlGroup').value=="2")
                    {
                       if(document.getElementById('rdbranchAll').checked==true)
                           {
                               cmbVal='ClientsBranchGroup'+'~'+'ALL';
                           }
                       else
                           {
                               cmbVal='ClientsBranchGroup'+'~'+'Selected'+'~'+document.getElementById('HiddenField_BranchGroup').value;
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
            else if(document.getElementById('cmbsearchOption').value=="UserID")
            {
                 var exchangesegmnet="<%=Session["ExchangeSegmentID"]%>";
                 
                 cmbVal=document.getElementById('cmbsearchOption').value;
                 var date2=null;
                 var date4=null;
                 var date1 = DtFrom.GetDate();
                 var date3 = DtTo.GetDate();
                 if(date1 != null) 
                 {
                   date2 =parseInt(date1.getMonth()+1)+'-'+date1.getDate()+'-'+ date1.getFullYear();
                 }
                 if(date3 != null) 
                 {
                   date4 =parseInt(date3.getMonth()+1)+'-'+date3.getDate()+'-'+ date3.getFullYear();
                 }
                 var criteritype='B';
                 if(exchangesegmnet=="1" || exchangesegmnet=="2" || exchangesegmnet=="4" || exchangesegmnet=="5" || exchangesegmnet=="15")
                 {
                     criteritype='  ExchangeTrades_TRADEDATE between "'+date2+'" AND ';
                 }
                 else
                 {
                    criteritype='  ComExchangeTrades_TRADEDATE between "'+date2+'" AND ';
                 }
                 criteritype=criteritype.replace('"',"'");
                 criteritype=criteritype.replace('"',"'");
                 criteritype=criteritype+ ' "'+date4+'"';
                 criteritype=criteritype.replace('"',"'");
                 criteritype=criteritype.replace('"',"'");
                 
                 if(document.getElementById('rdbSegmentAll').checked==false)
                 {
                    if(exchangesegmnet=="1" || exchangesegmnet=="2" || exchangesegmnet=="4" || exchangesegmnet=="5" || exchangesegmnet=="15")
                    {
                        criteritype=criteritype+ ' And EXCHANGETRADES_Segment in ("'+document.getElementById('HiddenField_Segment').value+'")';
                     }
                     else
                     {
                        criteritype=criteritype+ ' And ComEXCHANGETRADES_Segment in ("'+document.getElementById('HiddenField_Segment').value+'")';
                     }
                     criteritype=criteritype.replace('"',"'");
                     criteritype=criteritype.replace('"',"'");
                    
                 }
                  cmbVal=cmbVal+'~'+criteritype;
            }
            else  if(document.getElementById('cmbsearchOption').value=="Company")
            {
                 ajax_showOptions(objID,"Company",objEvent);
            }
            else 
            {
                cmbVal=document.getElementById('cmbsearchOption').value;
                cmbVal=cmbVal+'~'+document.getElementById('ddlgrouptype').value;
            }
          
          ajax_showOptions(objID,objListFun,objEvent,cmbVal);

        }

        function fnbroker(obj)
        {
             if(obj=="a")
                Hide('showFilter');
             else
             {
                  var cmb=document.getElementById('cmbsearchOption');
                  cmb.value='Broker';
                  Show('showFilter');
             }
             selecttion();
        }
        
        
        function fnClients(obj)
        {
             if(obj=="a")
                Hide('showFilter');
             else
             {
                  var cmb=document.getElementById('cmbsearchOption');
                  cmb.value='Clients';
                  Show('showFilter');
             }
             selecttion();
        }
        function fnAsset(obj)
        {
             if(obj=="a")
                Hide('showFilter');
             else
             {
                  var cmb=document.getElementById('cmbsearchOption');
                  cmb.value='Product';
                  Show('showFilter');
             }
             selecttion();
        }
         function fnCompany(obj)
          {
            if(obj=="a")
                Hide('showFilter');
             else
             {
                Show('showFilter');
                document.getElementById('cmbsearchOption').value='Company';
             }
             selecttion();
          }
        function fnSegment(obj)
        {
             if(obj=="a")
                Hide('showFilter');
		     else if(obj=="c")
             {
                Hide('showFilter');
                Show('Td_Specific');
             }
             else
             {
                  var cmb=document.getElementById('cmbsearchOption');
                  cmb.value='Segment';
                  Show('showFilter');
             }
             selecttion();
        }
        function fnUserID(obj)
        {
             if(obj=="a")
                Hide('showFilter');
             else
             {
                  var cmb=document.getElementById('cmbsearchOption');
                  cmb.value='UserID';
                  Show('showFilter');
             }
             selecttion();
        }
      function fnBranch(obj)
      {
            if(obj=="a")
                Hide('showFilter');
             else
             {
                   if(document.getElementById('ddlGroup').value=="0")
                   {
                    document.getElementById('cmbsearchOption').value='Branch';
                   }
                   if(document.getElementById('ddlGroup').value=="2")
                   {
                    document.getElementById('cmbsearchOption').value='BranchGroup';
                   }

                  Show('showFilter');
             }
         selecttion();
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
           selecttion();
      }
    function btnAddsubscriptionlist_click()
            {
            
                var cmb=document.getElementById('cmbsearchOption');
                        var userid = document.getElementById('txtSelectionID');
                        if(userid.value != '')
                        {
                            var ids = document.getElementById('txtSelectionID_hidden');
                            var listBox = document.getElementById('lstSlection');
                            var tLength = listBox.length;
                           
                            
                            var no = new Option();
                            no.value = ids.value;
                            no.text = userid.value;
                            listBox[tLength]=no;
                            var recipient = document.getElementById('txtSelectionID');
                            recipient.value='';
                        }
                        else
                            alert('Please search name and then Add!')
                        var s=document.getElementById('txtSelectionID');
                        s.focus();
                        s.select();
                   
            }
        
      function clientselectionfinal()
	        {
	            var listBoxSubs = document.getElementById('lstSlection');
	          
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
            
                Hide('showFilter');
                document.getElementById('btnScreen').disabled=false;
	        }
	     
	        
	   function btnRemovefromsubscriptionlist_click()
            {
                
                var listBox = document.getElementById('lstSlection');
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
    
   
   function  fnddlGroup(obj)
   {
        if(obj=="0" || obj=="2")
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
       selecttion();
   }
   function  fnddlview(obj)
   {
   if(obj=="1")
   {
   Show('Tr_Clients');
   Hide('Tr_Broker');
   }
   else
   {
   Hide('Tr_Clients');
   Show('Tr_Broker');
   
   }
   selecttion();
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
      selecttion();
   }

    function RecordDisplay()
    {
        Hide('showFilter');
        Show('td_filter');
        Hide('tab1');
        Show('displayAll');
        document.getElementById('hiddencount').value=0;
        selecttion();
        height();
        
    }
    function fnNoRecord(obj)
    {
        Hide('showFilter');
        Hide('td_filter');
        Show('tab1');
        Hide('displayAll');
        if(obj=='1')
            alert('No Record Found!!');
        if(obj=='3')
            alert('You Can Only Select Report View - '+'\n'+' [Branch/Group - Client] '+'\n'+' [Client / Group / Branch  + Month] '+'\n'+' [Month + Client / Group / Branch] '+'\n'+' [Instrument + Client / Group / Branch] ');
        if(obj=='4')
            alert("Mail Sent Successfully !!");
        if(obj=='5')
             alert("Error on sending!Try again.. !!");
       
        document.getElementById('hiddencount').value=0;  
        fn_ReportView(document.getElementById('DLLRptView').value);  
        height();
        selecttion();
    }
    function fn_ReportView(obj)
    {
        
        if(obj=='4' || obj=='6' || obj=='7')///Client + Instrument or Intrument Wise
        {
            Hide('Tr_Consolidate');
            Hide('Td_ConsolidatedAcrossSegment');
            document.getElementById('ChkConsolidate').checked=false;
            Hide('Tr_GroupBy');
            Hide('Tr_Company');
            Show('Tr_Clients');
            Show('Tr_viewby');
            Show('tr_ConsiderTopRecord');
            Show('Tr_FilterColumn');
            Show('Tr_SortOrder');
            if(obj=='6' || obj=='7')
            {
                Show('tr_UserID');
                Hide('Tr_Asset');
                Hide('tr_ConsolidateSegmentScrip');
                document.getElementById('ChkConsolidateSegmentScrip').checked=false;
            }
            else
            {
             
                Hide('tr_UserID'); 
                Show('Tr_Asset');
                Show('tr_ConsolidateSegmentScrip');
            }
             
        }
        else if(obj=='5')
        {
            Hide('tr_UserID'); 
            Show('Tr_Asset');
            Show('Tr_Consolidate');
            Hide('Td_ConsolidatedAcrossSegment');
            Show('Tr_GroupBy');
            Show('Tr_Clients');
            Show('Tr_viewby');
            Show('tr_ConsiderTopRecord');
            Show('Tr_FilterColumn');
            Show('Tr_SortOrder');
            Hide('Tr_Company');
            Show('tr_ConsolidateSegmentScrip');
            
        }
        else if(obj=='8')
        {
            Hide('Tr_Consolidate');
            Hide('Td_ConsolidatedAcrossSegment');
            Hide('tr_UserID'); 
            Hide('Tr_Asset');
            Show('Tr_GroupBy');
            Show('Tr_Clients');
            Show('Tr_viewby');
            Show('tr_ConsiderTopRecord');
            Show('Tr_FilterColumn');
            Show('Tr_SortOrder');
            Hide('Tr_Company');
            Hide('tr_ConsolidateSegmentScrip');
            document.getElementById('ChkConsolidateSegmentScrip').checked=false;
        }
        else if(obj=='9')
        {
            Hide('Tr_Consolidate');
            Hide('Td_ConsolidatedAcrossSegment');
            Show('tr_UserID'); 
            Hide('Tr_Asset');
            Show('Tr_GroupBy');
            Show('Tr_Clients');
            Show('Tr_viewby');
            Show('tr_ConsiderTopRecord');
            Show('Tr_FilterColumn');
            Show('Tr_SortOrder');
            Hide('Tr_Company');
            Hide('tr_ConsolidateSegmentScrip');
            document.getElementById('ChkConsolidateSegmentScrip').checked=false;
        }
        else if(obj=='10' || obj=='11')
        {
            Hide('Tr_Consolidate');
            Show('Td_ConsolidatedAcrossSegment');
            Hide('tr_UserID'); 
            Hide('Tr_Asset');
            Show('Tr_GroupBy');
            Hide('Tr_Clients');
            Hide('Tr_viewby');
            Hide('tr_ConsolidateSegmentScrip');
            Hide('tr_ConsiderTopRecord');
            Hide('Tr_FilterColumn');
            Hide('Tr_SortOrder');
            Show('Tr_Company');
            document.getElementById('ChkConsolidateSegmentScrip').checked=false;
        }
        else if(obj=='12')
        {
            Hide('Tr_Consolidate');
            Show('Td_ConsolidatedAcrossSegment');
            Hide('tr_UserID'); 
            Hide('Tr_Asset');
            Show('Tr_GroupBy');
            Show('Tr_Clients');
            Show('Tr_viewby');
            Hide('tr_ConsolidateSegmentScrip');
            Hide('tr_ConsiderTopRecord');
            Hide('Tr_FilterColumn');
            Hide('Tr_SortOrder');
            Show('Tr_Company');
            document.getElementById('ChkConsolidateSegmentScrip').checked=false;
        }
        else if(obj=='13')
        {
            Hide('Tr_Consolidate');
            Hide('Td_ConsolidatedAcrossSegment');
            Hide('tr_UserID'); 
            Hide('Tr_Asset');
            Show('Tr_GroupBy');
            Show('Tr_Clients');
            Show('Tr_viewby');
            Hide('tr_ConsolidateSegmentScrip');
            Hide('tr_ConsiderTopRecord');
            Hide('Tr_FilterColumn');
            Hide('Tr_SortOrder');
            Show('Tr_Company');
            document.getElementById('ChkConsolidateSegmentScrip').checked=false;
        }
        
        else 
        {
            Show('Tr_Consolidate');
            Hide('Td_ConsolidatedAcrossSegment');
            Hide('tr_UserID'); 
            Hide('Tr_Asset');
            Show('Tr_GroupBy');
            Hide('tr_ConsolidateSegmentScrip');
            Show('Tr_Clients');
            Show('Tr_viewby');
            Show('tr_ConsiderTopRecord');
            Show('Tr_FilterColumn');
            Show('Tr_SortOrder');
            Hide('Tr_Company');
            document.getElementById('ChkConsolidateSegmentScrip').checked=false;
        }
        
        FnddlGeneration(document.getElementById('ddlGeneration').value);     
        
       
    }
  function FnddlEmail(obj)
    {
        if(obj=="1")
        {
       // alert ('1234')
          //Show('Tab_showFilter'); 
          Show('showFilter');
          var cmb=document.getElementById('cmbsearchOption');
          cmb.value='MAILEMPLOYEE';  
          
        }
        else
        {
        //alert ('12346789')
           // Hide('Tab_showFilter');
            Hide('showFilter');
          
           // alert ('12346789')
        }
    }
  
  function FnddlGeneration(obj)
  {
    if(obj=="1")
    {
   
        Show('td_Screen');
        Hide('td_Export');
        Hide('td_Mail');
        Hide('tr_MailSendOption');
    }
    if(obj=="2")
    {
        Hide('td_Screen');
        Show('td_Export');
        Hide('td_Mail');
        Hide('tr_MailSendOption');
    }
    if(obj=="3")
    {
     //alert ('fhhsdhjfgvhjadgfh')
        Hide('td_Screen');
        Hide('td_Export');
        Show('td_Mail');
        Show('tr_MailSendOption');
      
        var cmb=document.getElementById('cmbsearchOption');
      cmb.value='MAILEMPLOYEE';
      Show('showFilter');
    
    }
      height();
  }
    function heightlight(obj)
   {
      
        var colorcode=obj.split('&');
        
         if((document.getElementById('hiddencount').value)==0)
         {
            prevobj='';
            prevcolor='';
            document.getElementById('hiddencount').value=1;
         }
          document.getElementById(obj).style.backgroundColor='#ffe1ac';
         
          if(prevobj!='')
          {
            document.getElementById(prevobj).style.backgroundColor=prevcolor;
          }
          prevobj=obj;
          prevcolor=colorcode[1];

    } 
  function selecttion()
  {
     var combo=document.getElementById('cmbExport');
     combo.value='Ex';
  } 
   function FnSettlementType(obj)
      {
             if((obj=="a") || (obj=="b"))
                Hide('showFilter');
             else
             {
                  var cmb=document.getElementById('cmbsearchOption');
                  cmb.value='SettlementType';
                  Show('showFilter');
             }
             selecttion();
             height();
      }
      function ShowFilters()
            {
            window.location='../reports/frmReport_BrokerageChargesiframe.aspx';
            }
 

    FieldName='lstSlection';
    </script>

    <script type="text/ecmascript">   
       function ReceiveServerData(rValue)
        {
                
                var j=rValue.split('~');
                var btn = document.getElementById('btnhide');

                if(j[0]=='Branch')
                {
                    document.getElementById('HiddenField_Branch').value = j[1];
                }
                if(j[0]=='Group')
                {
                    document.getElementById('HiddenField_Group').value = j[1];
                }  
                if(j[0]=='Clients')
                {
                    document.getElementById('HiddenField_Client').value = j[1];
                } 
                if(j[0]=='Broker')
                {
                    document.getElementById('HiddenField_Broker').value = j[1];
                } 
                if(j[0]=='Product')
                {
                    document.getElementById('HiddenField_Product').value = j[1];
                }
                if(j[0]=='Segment')
                {
                    document.getElementById('HiddenField_Segment').value = j[1];
                }
                if(j[0]=='UserID')
                {
                    document.getElementById('HiddenField_UserID').value = j[1];
                }
                if(j[0]=='BranchGroup')
                {
                    document.getElementById('HiddenField_BranchGroup').value = j[1];
                }
                if(j[0]=='MAILEMPLOYEE')
                {
                    document.getElementById('HiddenField_emmail').value = j[1];
                }
                if(j[0]=='Company')
                {
                    document.getElementById('HiddenField_Company').value = j[1];
                }
                 if(j[0]=='SettlementType')
            {
                document.getElementById('HiddenField_Setttype').value = j[1];
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3600">
        </asp:ScriptManager>

        <script language="javascript" type="text/javascript"> 
   var prm = Sys.WebForms.PageRequestManager.getInstance(); 
   prm.add_initializeRequest(InitializeRequest); 
   prm.add_endRequest(EndRequest); 
   var postBackElement; 
   function InitializeRequest(sender, args) 
   { 
      if (prm.get_isInAsyncPostBack()) 
         args.set_cancel(true); 
            postBackElement = args.get_postBackElement(); 
         $get('UpdateProgress1').style.display = 'block'; 
   } 
   function EndRequest(sender, args) 
   { 
          $get('UpdateProgress1').style.display = 'none'; 
 
   } 
        </script>

        <div>
            <table class="TableMain100">
                <tr>
                    <td class="EHEADER" colspan="0" style="text-align: center; height: 20px;">
                        <strong><span id="SpanHeader" style="color: #000099">Brokerage Statement</span></strong></td>
                    <td class="EHEADER" width="15%" id="td_filter" style="height: 22px">
                        <a href="javascript:void(0);" onclick="ShowFilters();"><span style="color: Blue;
                            text-decoration: underline; font-size: 8pt; font-weight: bold">Filter</span></a>
                        <asp:DropDownList ID="cmbExport" runat="server" AutoPostBack="True" Font-Size="12px"
                            OnSelectedIndexChanged="cmbExport_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                            <asp:ListItem Value="E">Excel</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table id="tab1" border="0" cellpadding="0" cellspacing="0">
                <tr valign="top">
                    <td>
                        <table>
                            <tr>
                                <td class="gridcellleft">
                                    <table  cellpadding="1" cellspacing="1" Class="tableClass">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                For A Period
                                            </td>
                                            <td class="gridcellleft">
                                                <dxe:ASPxDateEdit ID="DtFrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                    Font-Size="12px" Width="108px" ClientInstanceName="DtFrom">
                                                    <dropdownbutton text="From">
                                                </dropdownbutton>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                            <td class="gridcellleft">
                                                <dxe:ASPxDateEdit ID="DtTo" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                    Font-Size="12px" Width="108px" ClientInstanceName="DtTo">
                                                    <dropdownbutton text="To">
                                                </dropdownbutton>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <table  cellpadding="1" cellspacing="1" Class="tableClass">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Report View :</td>
                                            <td>
                                                <asp:DropDownList ID="DLLRptView" runat="server" Width="250px" Font-Size="12px" onchange="fn_ReportView(this.value)">
                                                    <asp:ListItem Value="1">Branch/Group - Client</asp:ListItem>
                                                    <asp:ListItem Value="2">Client / Group / Branch  + Month</asp:ListItem>
                                                    <asp:ListItem Value="3">Month + Client / Group / Branch</asp:ListItem>
                                                    <asp:ListItem Value="4">Instrument Wise</asp:ListItem>
                                                    <asp:ListItem Value="5">Instrument + Client / Group / Branch</asp:ListItem>
                                                    <asp:ListItem Value="6">UserID + Month</asp:ListItem>
                                                    <asp:ListItem Value="7">Month + UserID</asp:ListItem>
                                                    <asp:ListItem Value="8">Client Wise</asp:ListItem>
                                                    <asp:ListItem Value="9">UserID + Client </asp:ListItem>
                                                    <asp:ListItem Value="10">Month Wise -Across Segment</asp:ListItem>
                                                    <asp:ListItem Value="13">Month Wise -Across Segment +Branch/Group </asp:ListItem>
                                                    <asp:ListItem Value="12">Month Wise -Across Segment +Client </asp:ListItem>
                                                    <asp:ListItem Value="11">Year Wise -Across Segment</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="Tr_GroupBy">
                                <td class="gridcellleft">
                                    <table  cellpadding="1" cellspacing="1" class="tableBorderClass">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Group By</td>
                                            <td>
                                                <asp:DropDownList ID="ddlGroup" runat="server" Width="100px" Font-Size="12px" onchange="fnddlGroup(this.value)">
                                                    <asp:ListItem Value="0">Branch</asp:ListItem>
                                                    <asp:ListItem Value="1">Group</asp:ListItem>
                                                    <asp:ListItem Value="2">Branch Group</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td colspan="2" id="td_branch">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="rdbranchAll" runat="server" Checked="True" GroupName="a" onclick="fnBranch('a')" />
                                                            All
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbranchSelected" runat="server" GroupName="a" onclick="fnBranch('b')" />Selected
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td id="td_group" style="display: none;" colspan="2">
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
                                                            <asp:RadioButton ID="rdddlgrouptypeAll" runat="server" Checked="True" GroupName="b"
                                                                onclick="fnGroup('a')" />
                                                            All
                                                            <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="b" onclick="fnGroup('b')" />Selected
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="Tr_viewby">
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                                View By :</td>
                                            <td>
                                                <asp:DropDownList ID="ddlviewby" runat="server" Width="100px" Font-Size="12px" onchange="fnddlview(this.value)">
                                                    <asp:ListItem Value="1">Client</asp:ListItem>
                                                    <asp:ListItem Value="2">Broker</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="Tr_Clients">
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Clients :</td>
                                            <td>
                                                <asp:RadioButton ID="rdbClientALL" runat="server" Checked="True" GroupName="c" onclick="fnClients('a')" />
                                                All
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdPOAClient" runat="server" GroupName="c" onclick="fnClients('a')" />POA
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdbClientSelected" runat="server" GroupName="c" onclick="fnClients('b')" />
                                                Selected
                                            </td>
                                        </tr>
                                        <tr id="Tr_Broker">
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Broker :</td>
                                            <td>
                                                <asp:RadioButton ID="rdbbrokerall" runat="server" Checked="True" GroupName="M" onclick="fnbroker('a')" />
                                                All
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdbbrokerselected" runat="server" GroupName="M" onclick="fnbroker('b')" />
                                                Selected  
                                            </td>
                                            <%--<td>
                                                &nbsp;
                                            </td>--%>
                                            
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="Tr_Asset">
                                <td class="gridcellleft">
                                    <table  cellpadding="1" cellspacing="1" Class="tableClass">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Asset :</td>
                                            <td>
                                                <asp:RadioButton ID="rdbAssetAll" runat="server" Checked="True" GroupName="ee" onclick="fnAsset('a')" />
                                                All
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdAssetSelected" runat="server" GroupName="ee" onclick="fnAsset('b')" />Selected
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="Tr_Company">
                                <td class="gridcellleft">
                                    <table  cellpadding="1" cellspacing="1" Class="tableClass">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Company :</td>
                                            <td>
                                                <asp:RadioButton ID="RdbAllCompany" runat="server" GroupName="dd" onclick="fnCompany('a')" />
                                                All
                                                <asp:RadioButton ID="RdbCurrentCompany" runat="server" Checked="True" GroupName="dd"
                                                    onclick="fnCompany('a')" />
                                                Current
                                                <asp:RadioButton ID="RdbSelectedCompany" runat="server" GroupName="dd" onclick="fnCompany('b')" />Selected
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <table  cellpadding="1" cellspacing="1" Class="tableClass">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Segment:</td>
                                            <td>
                                                <asp:RadioButton ID="rdbSegmentAll" runat="server" GroupName="d" onclick="fnSegment('a')" />
                                                All
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdbSegmentSpecific" runat="server" Checked="True" GroupName="d"
                                                    onclick="fnSegment('c')" />Current
                                            </td>
                                            <td id="Td_Specific">
                                                [ <span id="litSegmentMain" runat="server" style="color: Maroon"></span>]
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdSegmentSelected" runat="server" GroupName="d" onclick="fnSegment('b')" />Selected
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="tr_sett" runat="server">
                                <td class="gridcellleft">
                                    <table  cellpadding="1" cellspacing="1" Class="tableClass">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Sett Type :</td>
                                            <td>
                                                <asp:RadioButton ID="rdbSettlementTypeSelected" runat="server" Checked="True" GroupName="z"
                                                    onclick="FnSettlementType('b')" />
                                                Current
                                            </td>
                                            <td>
                                                [ <span id="litSettlementType" runat="server" style="color: Maroon"></span>]
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RadbSettlementTypeselection" runat="server" GroupName="z" onclick="FnSettlementType('c')" />
                                                Selected
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RadbSettlementTypeAll" runat="server" GroupName="z" onclick="FnSettlementType('a')" />
                                                All
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="display: none;">
                                <td colspan="4">
                                    <table cellpadding="1" cellspacing="1" Class="tableClass">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Sett Type :</td>
                                            <td>
                                                <asp:RadioButton ID="RadioButton1" runat="server" GroupName="g" onclick="FnSettlementType('a')" />
                                                All
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RadioButton2" runat="server" Checked="True" GroupName="g" onclick="FnSettlementType('b')" />
                                                Selected
                                            </td>
                                            <td>
                                                [ <span id="litSettlementNo" runat="server" style="color: Maroon"></span>]
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="tr_UserID">
                                <td class="gridcellleft">
                                    <table cellpadding="1" cellspacing="1" Class="tableClass">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                User-ID :</td>
                                            <td>
                                                <asp:RadioButton ID="rdbUserIDAll" runat="server" Checked="True" GroupName="e" onclick="fnUserID('a')" />
                                                All
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdbUserIDSelected" runat="server" GroupName="e" onclick="fnUserID('b')" />Selected
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <table  cellpadding="1" cellspacing="1" Class="tableClass">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC" id="Tr_Consolidate">
                                                <asp:CheckBox ID="ChkConsolidate" runat="server" />
                                                Consolidate Client Group/Branch Wise</td>
                                            <td class="gridcellleft" bgcolor="#B7CEEC" id="Td_ConsolidatedAcrossSegment">
                                                <asp:CheckBox ID="ChkCOnsolidatedAcrossSegment" runat="server" />
                                                Show Group/Branch BreakUp</td>
                                            <td class="gridcellleft" bgcolor="#B7CEEC" id="tr_ConsolidateSegmentScrip">
                                                <asp:CheckBox ID="ChkConsolidateSegmentScrip" runat="server" />
                                                Consolidate Segment [Cash/FO] Wise
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="tr_ConsiderTopRecord">
                                <td class="gridcellleft">
                                    <table  cellpadding="1" cellspacing="1" Class="tableClass">
                                        <tr>
                                            <td bgcolor="#B7CEEC">
                                                Consider Top
                                            </td>
                                            <td bgcolor="#B7CEEC">
                                                <dxe:ASPxTextBox ID="txtTopRecord" runat="server" HorizontalAlign="Right" Width="100px">
                                                    <masksettings mask="&lt;0..9999999999g&gt;" includeliterals="DecimalSymbol" />
                                                    <validationsettings errordisplaymode="None"></validationsettings>
                                                </dxe:ASPxTextBox>
                                            </td>
                                            <td bgcolor="#B7CEEC">
                                                Entities</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="Tr_FilterColumn">
                                <td class="gridcellleft">
                                    <table cellpadding="1" cellspacing="1" Class="tableClass">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Filter Columns :
                                            </td>
                                            <td>
                                                <div style="overflow: auto; border: 1px black solid; scrollbar-base-color: #C0C0C0">
                                                    <asp:CheckBoxList ID="chktfilter" runat="server" RepeatDirection="Horizontal" Width="200px"
                                                        RepeatColumns="0">
                                                        <asp:ListItem Value="Turnover" Selected="True">Turnover</asp:ListItem>
                                                        <asp:ListItem Value="Brokerage" Selected="True">Brokerage</asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="Tr_SortOrder">
                                <td class="gridcellleft">
                                    <table cellpadding="1" cellspacing="1" Class="tableClass">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Sort Order :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DDlSortOrder" runat="server" Width="180px" Font-Size="12px">
                                                    <asp:ListItem Value="1">Client/Group/Branch Name</asp:ListItem>
                                                    <asp:ListItem Value="2">Total Brokerage (Descending)</asp:ListItem>
                                                    <asp:ListItem Value="3">Total TOT (Descending)</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <table  cellpadding="1" cellspacing="1" Class="tableClass">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Generate Type :</td>
                                            <td>
                                                <asp:DropDownList ID="ddlGeneration" runat="server" Width="210px" Font-Size="12px"
                                                    onchange="FnddlGeneration(this.value)">
                                                    <asp:ListItem Value="1">Screen</asp:ListItem>
                                                    <asp:ListItem Value="2">Export</asp:ListItem>
                                                    <asp:ListItem Value="3">Send Mail</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="tr_MailSendOption">
                                <td class="gridcellleft">
                                    <table  cellpadding="1" cellspacing="1" Class="tableClass">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Respective :</td>
                                            <td>
                                                <asp:DropDownList ID="ddloptionformail" runat="server" Width="100px" Font-Size="12px"
                                                    onchange="FnddlEmail(this.value)">
                                                    <asp:ListItem Value="1">User</asp:ListItem>
                                                    <asp:ListItem Value="2">Branch</asp:ListItem>
                                                    <asp:ListItem Value="3">Group</asp:ListItem>
                                                </asp:DropDownList></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <table>
                                        <tr>
                                            <td id="td_Screen">
                                                <asp:Button ID="btnScreen" runat="server" CssClass="btnUpdate" Height="20px" Text="Show"
                                                    Width="101px" OnClick="btnScreen_Click" OnClientClick="selecttion()" />
                                            </td>
                                            <td id="td_Mail">
                                                <asp:Button ID="btnSendmail" runat="server" CssClass="btnUpdate" Height="20px" Text="Send Mail"
                                                    Width="101px" OnClientClick="selecttion()" OnClick="btnSendmail_Click" /></td>
                                            <td id="td_Export">
                                                <asp:Button ID="btnExcel" runat="server" CssClass="btnUpdate" Height="20px" Text="Export To Excel"
                                                    Width="101px" OnClick="btnExcel_Click" OnClientClick="selecttion()" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table  cellpadding="1" cellspacing="1" id="showFilter"
                            <tr>
                                <td class="gridcellleft" style="vertical-align: top; text-align: right; height: 21px;"
                                    id="TdFilter">
                                    <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox>
                                    <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                        Enabled="false">
                                        <asp:ListItem>Clients</asp:ListItem>
                                        <asp:ListItem>Broker</asp:ListItem>
                                        <asp:ListItem>Branch</asp:ListItem>
                                        <asp:ListItem>Group</asp:ListItem>
                                        <asp:ListItem>Product</asp:ListItem>
                                        <asp:ListItem>Segment</asp:ListItem>
                                        <asp:ListItem>Company</asp:ListItem>
                                        <asp:ListItem>BranchGroup</asp:ListItem>
                                        <asp:ListItem>UserID</asp:ListItem>
                                        <asp:ListItem>MAILEMPLOYEE</asp:ListItem>
                                        <asp:ListItem>SettlementType</asp:ListItem>
                                    </asp:DropDownList>
                                    <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                        style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                            style="color: #009900; font-size: 8pt;"> </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ListBox ID="lstSlection" runat="server" Font-Size="12px" Height="90px" Width="290px">
                                    </asp:ListBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <a id="A2" href="javascript:void(0);" onclick="clientselectionfinal()"><span style="color: #000099;
                                                    text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
                                            </td>
                                            <td>
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
            </table>
            <table>
                <tr>
                    <td style="display: none;">
                        <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                        <asp:Button ID="btnhide" runat="server" Text="btnhide" OnClick="btnhide_Click" />
                        <asp:HiddenField ID="HiddenField_Group" runat="server" />
                        <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                        <asp:HiddenField ID="HiddenField_Client" runat="server" />
                        <asp:HiddenField ID="HiddenField_Broker" runat="server" />
                        <asp:HiddenField ID="HiddenField_Product" runat="server" />
                        <asp:HiddenField ID="HiddenField_Segment" runat="server" />
                        <asp:HiddenField ID="HiddenField_UserID" runat="server" />
                        <asp:HiddenField ID="HiddenField_BranchGroup" runat="server" />
                        <asp:HiddenField ID="hiddencount" runat="server" />
                        <asp:HiddenField ID="HiddenField_emmail" runat="server" />
                        <asp:HiddenField ID="HiddenField_Company" runat="server" />
                        <asp:HiddenField ID="HiddenField_Setttype" runat="server" />
                    </td>
                    <td>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <div id='Div1' style='position: absolute; font-family: arial; font-size: 30; left: 50%;
                                    top: 50%; background-color: white; layer-background-color: white; height: 80;
                                    width: 150;'>
                                    <table width='100' height='35' border='1' cellpadding='0' cellspacing='0' bordercolor='#C0D6E4'>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td height='25' align='center' bgcolor='#FFFFFF'>
                                                            <img src='/assests/images/progress.gif' width='18' height='18'></td>
                                                        <td height='10' width='100%' align='center' bgcolor='#FFFFFF'>
                                                            <font size='1' face='Tahoma'><strong align='center'>Please Wait..</strong></font></td>
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
            </table>
            <div id="displayAll" style="display: none;" width="100%">
                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                    <ContentTemplate>
                        <table width="100%" border="1">
                            <tr>
                                <td>
                                    <div id="DivHeader" runat="server">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="Divdisplay" runat="server">
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnScreen" EventName="Click"></asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
</asp:Content>
