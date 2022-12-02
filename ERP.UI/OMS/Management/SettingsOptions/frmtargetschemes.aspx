<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.SettingsOptions.management_SettingsOptions_frmtargetschemes" Codebehind="frmtargetschemes.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
 <%--   
    <script type="text/javascript" src="/assests/js/init.js"></script>
    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>
    <script language="javascript" type="text/javascript">

    </script>--%>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <script language="javascript" type="text/javascript">
    FieldName='txt_schemename';
     function RowValue(e)
          {
            grid.GetRowValues(e.visibleIndex,'tgt_masterid', GetValues);
          }
     function GetValues(values) 
         {
            document.getElementById('TdgrdTarget').style.display = 'none';
            document.getElementById('TdgrdTargetDetails').style.display = 'inline';
            document.getElementById('TdNew').style.display = 'inline';
            document.getElementById('TdDetails').style.display = 'none';
            RowId1=values;
            grid1.PerformCallback(values);
         } 
         
         function RowValue1(e)
          {
            grid1.GetRowValues(e.visibleIndex,'tgt_id', GetValues1);
          }
     function GetValues1(values) 
         {
            document.getElementById('TdgrdTarget').style.display = 'none';
            document.getElementById('TdgrdTargetDetails').style.display = 'none';
            document.getElementById('TdNew').style.display = 'none';
            document.getElementById('TdDetails').style.display = 'inline';
            RowID = values;
            BtnEdit_Click();
         } 
    function sum_value(obj)
 {
     if ((obj.id == 'txt_retails') || (obj.id == 'txt_hni') || (obj.id == 'txt_institution'))
     {
     var main_obj
     main_obj=document.getElementById('txt_newclients')
     var fisrt
     var second
     var third
     first=document.getElementById('txt_retails').value
     second=document.getElementById('txt_hni').value
     third=document.getElementById('txt_institution').value
     if (first=='')
     { first=0}
     if (second=='')
     {second=0}
     if (third=='')
     {third=0}
     main_obj.value= Number(first) + Number(second) + Number(third);
    
     }
     
     if ((obj.id == 'txt_derivatives') || (obj.id == 'txt_equities') || (obj.id == 'txt_commodities'))
     {
     var main_obj
     main_obj=document.getElementById('txt_grossearning')
     var fisrt
     var second
     var third
     first=document.getElementById('txt_derivatives').value
     second=document.getElementById('txt_equities').value
     third=document.getElementById('txt_commodities').value
     if (first=='')
     { first=0}
     if (second=='')
     {second=0}
     if (third=='')
     {third=0}
     main_obj.value= Number(first) + Number(second) + Number(third);
     }
     
     
      if ((obj.id == 'txt_newcalls') || (obj.id == 'txt_coldcalls'))
     {
     var main_obj
     main_obj=document.getElementById('txt_totalcalls')
     var first
     var second
     var third
     first=document.getElementById('txt_newcalls').value
     second=document.getElementById('txt_coldcalls').value     
     if (first=='')
     {first=0}
     if (second=='')
     {second=0}     
     main_obj.value= Number(first) + Number(second);
     }
     
     
     if ((obj.id == 'txt_selfsalesvisit') || (obj.id == 'txt_newsalesvisit'))
     {
     var main_obj
     main_obj=document.getElementById('txt_salesvisit')
     var first
     var second   
     first=document.getElementById('txt_selfsalesvisit').value
     second=document.getElementById('txt_newsalesvisit').value     
     if (first=='')
     { first=0}
     if (second=='')
     {second=0}     
     main_obj.value= Number(first) + Number(second);
     }    
     
     
     if ((obj.id == 'txt_unitmonthly') || (obj.id == 'txt_acpmonthly'))
     {
     var first
     var second
     var third
     first=document.getElementById('txt_unitmonthly').value
     second=document.getElementById('txt_acpmonthly').value     
     document.getElementById('txt_tcpmonthly').value=Number(first)*Number(second)
     document.getElementById('txt_aapmonthly').value=12*Number(second)
     document.getElementById('txt_tapmonthly').value=Number(first)*document.getElementById('txt_aapmonthly').value     
     document.getElementById('txt_grandtap').value=Number(document.getElementById('txt_taponce').value)+Number(document.getElementById('txt_tapann').value)+Number(document.getElementById('txt_tapsemiann').value)+Number(document.getElementById('txt_tapquarterly').value)+Number(document.getElementById('txt_tapmonthly').value)
     document.getElementById('txt_grandtcp').value=Number(document.getElementById('txt_tcponce').value)+Number(document.getElementById('txt_tcpann').value)+Number(document.getElementById('txt_tcpsemiann').value)+Number(document.getElementById('txt_tcpquaterly').value)+Number(document.getElementById('txt_tcpmonthly').value)
     }
     
     
     if ((obj.id == 'txt_unitquarterly') || (obj.id == 'txt_acpquartly'))
     {
     var first
     var second
     var third
     first=document.getElementById('txt_unitquarterly').value
     second=document.getElementById('txt_acpquartly').value     
     document.getElementById('txt_tcpquaterly').value=Number(first)*Number(second)
     document.getElementById('txt_aapquaterly').value=4*Number(second)
     document.getElementById('txt_tapquarterly').value=Number(first)*Number(document.getElementById('txt_aapquaterly').value)     
     document.getElementById('txt_grandtap').value=Number(document.getElementById('txt_taponce').value)+Number(document.getElementById('txt_tapann').value)+Number(document.getElementById('txt_tapsemiann').value)+Number(document.getElementById('txt_tapquarterly').value)+Number(document.getElementById('txt_tapmonthly').value)
     document.getElementById('txt_grandtcp').value=Number(document.getElementById('txt_tcponce').value)+Number(document.getElementById('txt_tcpann').value)+Number(document.getElementById('txt_tcpsemiann').value)+Number(document.getElementById('txt_tcpquaterly').value)+Number(document.getElementById('txt_tcpmonthly').value)
     }
     
     
    if ((obj.id == 'txt_unitsemiannual') || (obj.id == 'txt_acpsemiann'))
     {
     var first
     var second
     var third
     first=document.getElementById('txt_unitsemiannual').value
     second=document.getElementById('txt_acpsemiann').value     
     document.getElementById('txt_tcpsemiann').value=Number(first)*Number(second)
     document.getElementById('txt_aapsemiann').value=2*Number(second)
     document.getElementById('txt_tapsemiann').value=Number(first)*document.getElementById('txt_aapsemiann').value     
     document.getElementById('txt_grandtap').value=Number(document.getElementById('txt_taponce').value)+Number(document.getElementById('txt_tapann').value)+Number(document.getElementById('txt_tapsemiann').value)+Number(document.getElementById('txt_tapquarterly').value)+Number(document.getElementById('txt_tapmonthly').value)
     document.getElementById('txt_grandtcp').value=Number(document.getElementById('txt_tcponce').value)+Number(document.getElementById('txt_tcpann').value)+Number(document.getElementById('txt_tcpsemiann').value)+Number(document.getElementById('txt_tcpquaterly').value)+Number(document.getElementById('txt_tcpmonthly').value)
     }
     
     
     if ((obj.id == 'txt_unitannual') || (obj.id == 'txt_acpann'))
     {
     var first
     var second
     var third
     first=document.getElementById('txt_unitannual').value
     second=document.getElementById('txt_acpann').value     
     document.getElementById('txt_tcpann').value=Number(first)*Number(second)
     document.getElementById('txt_aapann').value=1*Number(second)
     document.getElementById('txt_tapann').value=Number(first)*document.getElementById('txt_aapann').value     
     document.getElementById('txt_grandtap').value=Number(document.getElementById('txt_taponce').value)+Number(document.getElementById('txt_tapann').value)+Number(document.getElementById('txt_tapsemiann').value)+Number(document.getElementById('txt_tapquarterly').value)+Number(document.getElementById('txt_tapmonthly').value)
     document.getElementById('txt_grandtcp').value=Number(document.getElementById('txt_tcponce').value)+Number(document.getElementById('txt_tcpann').value)+Number(document.getElementById('txt_tcpsemiann').value)+Number(document.getElementById('txt_tcpquaterly').value)+Number(document.getElementById('txt_tcpmonthly').value)
     }
     
     
     if ((obj.id == 'txt_unitonetime') || (obj.id == 'txt_acponce'))
     {
     var first
     var second
     var third
     first=document.getElementById('txt_unitonetime').value
     second=document.getElementById('txt_acponce').value     
     document.getElementById('txt_tcponce').value=Number(first)*Number(second)
     document.getElementById('txt_aaponce').value=1*Number(second)
     document.getElementById('txt_taponce').value=Number(first)*document.getElementById('txt_acponce').value     
     document.getElementById('txt_grandtap').value=Number(document.getElementById('txt_taponce').value)+Number(document.getElementById('txt_tapann').value)+Number(document.getElementById('txt_tapsemiann').value)+Number(document.getElementById('txt_tapquarterly').value)+Number(document.getElementById('txt_tapmonthly').value)
     document.getElementById('txt_grandtcp').value=Number(document.getElementById('txt_tcponce').value)+Number(document.getElementById('txt_tcpann').value)+Number(document.getElementById('txt_tcpsemiann').value)+Number(document.getElementById('txt_tcpquaterly').value)+Number(document.getElementById('txt_tcpmonthly').value)
     }
     
     
     
     if ((obj.id == 'txt_unitsip') || (obj.id == 'txt_aisip'))
     {
     var first
     var second
     var third
     first=document.getElementById('txt_unitsip').value
     second=document.getElementById('txt_aisip').value     
     document.getElementById('txt_tasip').value=Number(first)*Number(second)    
     document.getElementById('txt_grandta').value=Number(document.getElementById('txt_tachurned').value)+Number(document.getElementById('txt_tafresh').value)+Number(document.getElementById('txt_tasip').value)
    }
     
     
     
     
      if ((obj.id == 'txt_unitfresh') || (obj.id == 'txt_aifresh'))
     {
     var first
     var second
     var third
     first=document.getElementById('txt_unitfresh').value
     second=document.getElementById('txt_aifresh').value     
     document.getElementById('txt_tafresh').value=Number(first)*Number(second)
     document.getElementById('txt_grandta').value=Number(document.getElementById('txt_tachurned').value)+Number(document.getElementById('txt_tafresh').value)+Number(document.getElementById('txt_tasip').value)
     }
     
     
     
      if ((obj.id == 'txt_unitchurned') || (obj.id == 'txt_aichurned'))
     {
     var first
     var second
     var third
     first=document.getElementById('txt_unitchurned').value
     second=document.getElementById('txt_aichurned').value     
     document.getElementById('txt_tachurned').value=Number(first)*Number(second)
     document.getElementById('txt_grandta').value=Number(document.getElementById('txt_tachurned').value)+Number(document.getElementById('txt_tafresh').value)+Number(document.getElementById('txt_tasip').value)
     }
 }
 function  validate_gen(obj,obj_type,err_msg,null_allow,sp_cahr_allow)
   {
     var i      
     var str
     if (err_msg=="")
     {
     err_msg="Value is Invalid";
     }
         str=obj.value;                        
         
       // *------------for null value validation----------------------          
         if (null_allow==1)
         {
         if (str.length==0)
         {
          alert('Null Value Not Allowed');          
          obj.focus();
          return false;
         }                  
         }
         
      // *----------------for string validation------------------                   
         if (obj_type==0)
         {
        for(i=0;i<str.length;i++)
        {    
          if (isNaN(str.charAt(i))==false)
           {
             alert(err_msg);        
             obj.focus();
             return false;
           }                   
           else
           {           
           if (sp_cahr_allow==1) 
           {
           if(isSplChar(str.charAt(i))== true)
           {
           alert('Plz Not Use Special Character');
           obj.focus();
           return false;
           }
           }                       
           }
        }
          }
      // *---------------for integer value validation-------------------          
      if (obj_type==1)
      {
      if (IsNumeric(str)==false)
      {
      alert(err_msg);
      obj.value=0;
      obj.focus();
      return ;
      }
      }  
      
      //*---------for Email value check
      
      if (obj_type==2)
      {
      	if (echeck(str)==false)
      	{
      	alert("Invalid E-mail ID")
		obj.value=""
		obj.focus()
		return false
     	}
      }	              
        return true;
    } 
    function echeck(str) 
    {
		var at="@"
		var dot="."
		var lat=str.indexOf(at)
		var lstr=str.length
		var ldot=str.indexOf(dot)
		if (str.indexOf(at)==-1){
		   alert("Invalid E-mail ID")
		   return false
		}

		if (str.indexOf(at)==-1 || str.indexOf(at)==0 || str.indexOf(at)==lstr)
		{		   
		   return false
		}

		if (str.indexOf(dot)==-1 || str.indexOf(dot)==0 || str.indexOf(dot)==lstr)
		{		
		    return false
		}

		 if (str.indexOf(at,(lat+1))!=-1)
		 {		 
		    return false
		 }

		 if (str.substring(lat-1,lat)==dot || str.substring(lat+1,lat+2)==dot)
		 {		 
		    return false
		 }

		 if (str.indexOf(dot,(lat+2))==-1)
		 {		 
		    return false
		 }
		
		 if (str.indexOf(" ")!=-1)
		 {		 
		    return false
		 }

 		 return true;					
	}   
    
    
   // Check for Numeric Function 
   
    
    function IsNumeric(sText)
      {
        var ValidChars = "0123456789.";
        var IsNumber=true;
        var Char; 
        for (i = 0; i < sText.length && IsNumber == true; i++) 
        { 
         Char = sText.charAt(i); 
         if (ValidChars.indexOf(Char) == -1) 
         {
         IsNumber = false;
         }
        }
        return IsNumber;
      }
    
    
    /*Check for a special character'***/
function isSplChar(str)
{	
	var spchar, getChar, SpecialChar,j;	
	spchar="`()(\\~!@^&*+\"|%:=,<>";
	getChar='Empty';
	SpecialChar='No';
	var spchars =" ` ( )  \\ ~ ! @ ^ & * + \" | : =  , < > "; 
	//for(var i=0; i
	//{
		for(j=0;j<(spchar.length)-1;j++)
		{			
			if(str == spchar.charAt(j))
			{			
				SpecialChar='Yes';
				//alert(SpecialChar);			
				break;			
			}
			else
			{
				if (str.charAt(i)!=' ')
				getChar='Normal';
			}
		}		
	//}
	if (SpecialChar == 'Yes')
	{
		//alert('Please do not enter any of the following characters: \n ' + spchars);	
		return true;
	}
	else if (SpecialChar == 'No')
	{
		//alert('Please do not enter any of the following characters: \n ' + spchars);			
		return false;
	}
}
function TdShow()
{
    document.getElementById("TdgrdTarget").style.display = 'inline';
    document.getElementById("TdgrdTargetDetails").style.display = 'none';
    document.getElementById("TdNew").style.display = 'inline';
    document.getElementById("TdDetails").style.display = 'none';
}
function btn_New()
{
    document.getElementById("TdgrdTarget").style.display = 'none';
    document.getElementById("TdgrdTargetDetails").style.display = 'none';
    document.getElementById("TdNew").style.display = 'none';
    document.getElementById("TdDetails").style.display = 'inline';
    var TextBoxId=document.getElementById('txt_schemename');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_newclients');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_grossearning');
        TextBoxId.readOnly = false;
        TextBoxId.value='';        
        TextBoxId=document.getElementById('txt_equities');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_equities');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_derivatives');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_commodities');
        TextBoxId.readOnly = false;
        TextBoxId.value='';        
        TextBoxId=document.getElementById('txt_totalcalls');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_newcalls');
        TextBoxId.readOnly = false;
        TextBoxId.value='';        
        TextBoxId=document.getElementById('txt_coldcalls');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_salesvisitratio');
        TextBoxId.readOnly = false;
        TextBoxId.value='';        
        TextBoxId=document.getElementById('txt_salesratio');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_salesvisit');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_newsalesvisit');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_selfsalesvisit');
        TextBoxId.readOnly = false;
        TextBoxId.value='';        
        TextBoxId=document.getElementById('txt_sales_salesratio');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_retails');
        TextBoxId.readOnly = false;
        TextBoxId.value='';        
        TextBoxId=document.getElementById('txt_hni');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_institution');
        TextBoxId.readOnly = false;
        TextBoxId.value='';        
        TextBoxId=document.getElementById('txt_unitmonthly');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_acpmonthly');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_tcpmonthly');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_aapmonthly');
        TextBoxId.readOnly = false;
        TextBoxId.value='';        
        TextBoxId=document.getElementById('txt_tapmonthly');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_unitquarterly');
        TextBoxId.readOnly = false;
        TextBoxId.value='';        
        TextBoxId=document.getElementById('txt_acpquartly');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_tcpquaterly');
        TextBoxId.readOnly = false;
        TextBoxId.value='';        
        TextBoxId=document.getElementById('txt_aapquaterly');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_tapquarterly');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_unitsemiannual');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_acpsemiann');
        TextBoxId.readOnly = false;
        TextBoxId.value='';        
        TextBoxId=document.getElementById('txt_tcpsemiann');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_aapsemiann');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_tapsemiann');
        TextBoxId.readOnly = false;
        TextBoxId.value='';        
        TextBoxId=document.getElementById('txt_unitannual');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_acpann');
        TextBoxId.readOnly = false;
        TextBoxId.value='';        
        TextBoxId=document.getElementById('txt_tcpann');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_aapann');
        TextBoxId.readOnly = false;
        TextBoxId.value='';        
        TextBoxId=document.getElementById('txt_tapann');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_unitonetime');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_acponce');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_tcponce');
        TextBoxId.readOnly = false;
        TextBoxId.value='';        
        TextBoxId=document.getElementById('txt_aaponce');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_taponce');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_grandtcp');
        TextBoxId.readOnly = false;
        TextBoxId.value='';        
        TextBoxId=document.getElementById('txt_grandtap');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_unitsip');
        TextBoxId.readOnly = false;
        TextBoxId.value='';        
        TextBoxId=document.getElementById('txt_aisip');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_tasip');
        TextBoxId.readOnly = false;
        TextBoxId.value='';        
        TextBoxId=document.getElementById('txt_unitfresh');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_aifresh');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_tafresh');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_unitchurned');
        TextBoxId.readOnly = false;
        TextBoxId.value='';        
        TextBoxId=document.getElementById('txt_aichurned');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_tachurned');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('txt_grandta');
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById("TxtEndDate"); 
        TextBoxId.readOnly = false;
        TextBoxId.value='';
        TextBoxId=document.getElementById('lst_periodicity');
        TextBoxId.disabled = false;
        TextBoxId=document.getElementById('btn_target_save');
        TextBoxId.disabled = false;
        TextBoxId=document.getElementById("hdID");
        TextBoxId.value='';
}
function BtnEdit()
{
    var TextBoxId=document.getElementById('txt_schemename');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_newclients');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_grossearning');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_equities');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_equities');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_derivatives');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_commodities');
        TextBoxId.readOnly = true; 
        TextBoxId=document.getElementById('txt_totalcalls');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_newcalls');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_coldcalls');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_salesvisitratio');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_salesratio');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_salesvisit');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_newsalesvisit');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_selfsalesvisit');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_sales_salesratio');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_retails');
        TextBoxId.readOnly = true; 
        TextBoxId=document.getElementById('txt_hni');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_institution');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_unitmonthly');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_acpmonthly');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_tcpmonthly');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_aapmonthly');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_tapmonthly');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_unitquarterly');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_acpquartly');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_tcpquaterly');
        TextBoxId.readOnly = true; 
        TextBoxId=document.getElementById('txt_aapquaterly');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_tapquarterly');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_unitsemiannual');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_acpsemiann');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_tcpsemiann');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_aapsemiann');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_tapsemiann');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_unitannual');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_acpann');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_tcpann');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_aapann');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_tapann');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_unitonetime');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_acponce');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_tcponce');
        TextBoxId.readOnly = true; 
        TextBoxId=document.getElementById('txt_aaponce');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_taponce');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_grandtcp');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_grandtap');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_unitsip');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_aisip');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_tasip');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_unitfresh');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_aifresh');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_tafresh');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_unitchurned');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_aichurned');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_tachurned');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('txt_grandta');
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById("TxtEndDate"); 
        TextBoxId.readOnly = true;
        TextBoxId=document.getElementById('lst_periodicity');
        TextBoxId.disabled = true;
        TextBoxId=document.getElementById('btn_target_save');
        TextBoxId.disabled = true;
}
function TABLE1_onclick() {

}

        </script>

        <script type="text/ecmascript">
   function Btn_Save()
   {
        var data='Save';
        var TextId=document.getElementById("txt_schemename");
                    if(TextId.value!="")
                    {
                        data+='~'+TextId.value;
                    }
                    else
                    {
                        alert('Scheme Name is Required !');
                        return false;
                    }
           TextId=document.getElementById("lst_periodicity"); 
           data+='~'+TextId.value;
           TextId=document.getElementById("txt_newclients"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_grossearning"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_equities"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_derivatives"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_commodities"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_totalcalls"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_newcalls"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_coldcalls"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_salesvisitratio"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_salesratio"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_salesvisit"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_newsalesvisit"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_selfsalesvisit"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_sales_salesratio"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_retails"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_hni"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_institution"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_unitmonthly"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_acpmonthly"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_tcpmonthly"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_aapmonthly"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_tapmonthly"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_unitquarterly"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_acpquartly"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_tcpquaterly"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_aapquaterly"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_tapquarterly"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_unitsemiannual"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_acpsemiann"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_tcpsemiann"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_aapsemiann"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_tapsemiann"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_unitannual"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_acpann"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_tcpann"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_aapann"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_tapann"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_unitonetime"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_acponce"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_tcponce"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_aaponce"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_taponce"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_grandtcp"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_grandtap"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_unitsip"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_aisip"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_tasip"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_unitfresh"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_aifresh"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_tafresh"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_unitchurned"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_aichurned"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_tachurned"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("txt_grandta"); 
           if(TextId.value!="")
           {
                data+='~'+TextId.value;
           }
           else
           {
                data+='~'+'0';
           }
           TextId=document.getElementById("TxtEffectiveDate"); 
           data+='~'+TextId.value;
           var dataFromCombo=TxtEndDate.GetDate();
          TextId=(dataFromCombo.getMonth()+1)+'/'+dataFromCombo.getDate()+'/'+dataFromCombo.getFullYear();
          //alert(TextId);
          // alert((dataFromCombo.getMonth()+1)+'/'+dataFromCombo.getDate()+'/'+dataFromCombo.getFullYear());
          //alert(TextId);
           if(TextId!="")
           {
                data+='~'+TextId;
           }
           else
           {
                alert('End Date Required !');
                return false;
           } 
           var Edate=new Date(document.getElementById("TxtEffectiveDate").value);  
           var Tilldate=new Date(document.getElementById("TxtEndDate").value); 
           if(Tilldate<= Edate)
           {
                alert('End Date Must Be Greater Than Effective Date');
                return false;
           }   
           TextId=document.getElementById("hdID");
           data+='~'+TextId.value;
           TextId=document.getElementById("HD");
           data+='~'+TextId.value;
          CallServer(data, ""); 
           btn_New();
           document.getElementById('TdgrdTarget').style.display = 'inline';
           document.getElementById('TdgrdTargetDetails').style.display = 'none';
           document.getElementById('TdNew').style.display = 'inline';
           document.getElementById('TdDetails').style.display = 'none';
   } 
   function BtnEdit_Click()
    {
        BtnEdit();
        CallServer("Edit" + '~' + RowID,"");
    }
    function BtnRevised_Click()
    {
        document.getElementById('TdgrdTarget').style.display = 'none';
        document.getElementById('TdgrdTargetDetails').style.display = 'none';
        document.getElementById('TdNew').style.display = 'none';
        document.getElementById('TdDetails').style.display = 'inline';
        CallServer("Revise" + '~' + RowId1,"");
    }
   function ReceiveServerData(rValue)
    {
        var DATA=rValue.split('~');
        if(DATA[0]=="Save")
        {
            if(DATA[1]=="Y")
            {
                alert('Update Successfully !!');
                grid.PerformCallback();
            }
        }
       if(DATA[0]=="Edit")
        {
            if(DATA[1]!="N")
            {
                var TextId=document.getElementById("txt_schemename");
                    TextId.value=DATA[1];
                   TextId=document.getElementById("lst_periodicity"); 
                   TextId.value=DATA[2];
                   TextId=document.getElementById("txt_newclients"); 
                   TextId.value=DATA[3];
                   TextId=document.getElementById("txt_grossearning"); 
                   TextId.value=DATA[4];
                   TextId=document.getElementById("txt_equities"); 
                   TextId.value=DATA[5];
                   TextId=document.getElementById("txt_derivatives"); 
                   TextId.value=DATA[6];
                   TextId=document.getElementById("txt_commodities"); 
                   TextId.value=DATA[7];
                   TextId=document.getElementById("txt_totalcalls"); 
                   TextId.value=DATA[8];
                   TextId=document.getElementById("txt_newcalls"); 
                   TextId.value=DATA[9];
                   TextId=document.getElementById("txt_coldcalls"); 
                   TextId.value=DATA[10];
                   TextId=document.getElementById("txt_salesvisitratio"); 
                   TextId.value=DATA[11];
                   TextId=document.getElementById("txt_salesratio"); 
                   TextId.value=DATA[12];
                   TextId=document.getElementById("txt_salesvisit"); 
                   TextId.value=DATA[13];
                   TextId=document.getElementById("txt_newsalesvisit"); 
                   TextId.value=DATA[14];
                   TextId=document.getElementById("txt_selfsalesvisit"); 
                   TextId.value=DATA[15];
                   TextId=document.getElementById("txt_sales_salesratio"); 
                   TextId.value=DATA[16];
                   TextId=document.getElementById("txt_retails"); 
                   TextId.value=DATA[17];
                   TextId=document.getElementById("txt_hni"); 
                   TextId.value=DATA[18];
                   TextId=document.getElementById("txt_institution"); 
                   TextId.value=DATA[19];
                   TextId=document.getElementById("txt_unitmonthly"); 
                   TextId.value=DATA[20];
                   TextId=document.getElementById("txt_acpmonthly"); 
                   TextId.value=DATA[21];
                   TextId=document.getElementById("txt_tcpmonthly"); 
                   TextId.value=DATA[22];
                   TextId=document.getElementById("txt_aapmonthly"); 
                   TextId.value=DATA[23];
                   TextId=document.getElementById("txt_tapmonthly"); 
                   TextId.value=DATA[24];
                   TextId=document.getElementById("txt_unitquarterly"); 
                   TextId.value=DATA[25];
                   TextId=document.getElementById("txt_acpquartly"); 
                   TextId.value=DATA[26];
                   TextId=document.getElementById("txt_tcpquaterly"); 
                   TextId.value=DATA[27];
                   TextId=document.getElementById("txt_aapquaterly"); 
                   TextId.value=DATA[28];
                   TextId=document.getElementById("txt_tapquarterly"); 
                   TextId.value=DATA[29];
                   TextId=document.getElementById("txt_unitsemiannual"); 
                   TextId.value=DATA[30];
                   TextId=document.getElementById("txt_acpsemiann"); 
                   TextId.value=DATA[31];
                   TextId=document.getElementById("txt_tcpsemiann"); 
                   TextId.value=DATA[32];
                   TextId=document.getElementById("txt_aapsemiann"); 
                   TextId.value=DATA[33];
                   TextId=document.getElementById("txt_tapsemiann"); 
                   TextId.value=DATA[34];
                   TextId=document.getElementById("txt_unitannual"); 
                   TextId.value=DATA[35];
                   TextId=document.getElementById("txt_acpann"); 
                   TextId.value=DATA[36];
                   TextId=document.getElementById("txt_tcpann"); 
                   TextId.value=DATA[37];
                   TextId=document.getElementById("txt_aapann"); 
                   TextId.value=DATA[38];
                   TextId=document.getElementById("txt_tapann"); 
                   TextId.value=DATA[39];
                   TextId=document.getElementById("txt_unitonetime"); 
                   TextId.value=DATA[40];
                   TextId=document.getElementById("txt_acponce"); 
                   TextId.value=DATA[41];
                   TextId=document.getElementById("txt_tcponce"); 
                   TextId.value=DATA[42];
                   TextId=document.getElementById("txt_aaponce"); 
                   TextId.value=DATA[43];
                   TextId=document.getElementById("txt_taponce"); 
                   TextId.value=DATA[44];
                   TextId=document.getElementById("txt_grandtcp"); 
                   TextId.value=DATA[45];
                   TextId=document.getElementById("txt_grandtap"); 
                   TextId.value=DATA[46];
                   TextId=document.getElementById("txt_unitsip"); 
                   TextId.value=DATA[47];
                   TextId=document.getElementById("txt_aisip"); 
                   TextId.value=DATA[48];
                   TextId=document.getElementById("txt_tasip"); 
                   TextId.value=DATA[49];
                   TextId=document.getElementById("txt_unitfresh"); 
                   TextId.value=DATA[50];
                   TextId=document.getElementById("txt_aifresh"); 
                   TextId.value=DATA[51];
                   TextId=document.getElementById("txt_tafresh"); 
                   TextId.value=DATA[52];
                   TextId=document.getElementById("txt_unitchurned"); 
                   TextId.value=DATA[53];
                   TextId=document.getElementById("txt_aichurned"); 
                   TextId.value=DATA[54];
                   TextId=document.getElementById("txt_tachurned"); 
                   TextId.value=DATA[55];
                   TextId=document.getElementById("txt_grandta"); 
                   TextId.value=DATA[56];
                   TextId=document.getElementById("TxtEffectiveDate"); 
                   TextId.value=DATA[57];
                   TextId=document.getElementById("TxtEndDate"); 
                   TextId.value=DATA[58];
                   TextId=document.getElementById("hdID");
                   TextId.value=DATA[59];
            }
        }
        if(DATA[0]=="Revise")
        {
            if(DATA[1]!="N")
            {
                var TextId=document.getElementById("txt_schemename");
                    TextId.value=DATA[1];
                    TextId.readOnly = false;
                   TextId=document.getElementById("lst_periodicity"); 
                   TextId.value=DATA[2];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_newclients"); 
                   TextId.value=DATA[3];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_grossearning"); 
                   TextId.value=DATA[4];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_equities"); 
                   TextId.value=DATA[5];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_derivatives"); 
                   TextId.value=DATA[6];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_commodities"); 
                   TextId.value=DATA[7];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_totalcalls"); 
                   TextId.value=DATA[8];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_newcalls"); 
                   TextId.value=DATA[9];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_coldcalls"); 
                   TextId.value=DATA[10];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_salesvisitratio"); 
                   TextId.value=DATA[11];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_salesratio"); 
                   TextId.value=DATA[12];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_salesvisit"); 
                   TextId.value=DATA[13];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_newsalesvisit"); 
                   TextId.value=DATA[14];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_selfsalesvisit"); 
                   TextId.value=DATA[15];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_sales_salesratio"); 
                   TextId.value=DATA[16];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_retails"); 
                   TextId.value=DATA[17];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_hni"); 
                   TextId.value=DATA[18];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_institution"); 
                   TextId.value=DATA[19];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_unitmonthly"); 
                   TextId.value=DATA[20];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_acpmonthly"); 
                   TextId.value=DATA[21];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_tcpmonthly"); 
                   TextId.value=DATA[22];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_aapmonthly"); 
                   TextId.value=DATA[23];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_tapmonthly"); 
                   TextId.value=DATA[24];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_unitquarterly"); 
                   TextId.value=DATA[25];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_acpquartly"); 
                   TextId.value=DATA[26];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_tcpquaterly"); 
                   TextId.value=DATA[27];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_aapquaterly"); 
                   TextId.value=DATA[28];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_tapquarterly"); 
                   TextId.value=DATA[29];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_unitsemiannual"); 
                   TextId.value=DATA[30];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_acpsemiann"); 
                   TextId.value=DATA[31];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_tcpsemiann"); 
                   TextId.value=DATA[32];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_aapsemiann"); 
                   TextId.value=DATA[33];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_tapsemiann"); 
                   TextId.value=DATA[34];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_unitannual"); 
                   TextId.value=DATA[35];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_acpann"); 
                   TextId.value=DATA[36];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_tcpann"); 
                   TextId.value=DATA[37];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_aapann"); 
                   TextId.value=DATA[38];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_tapann"); 
                   TextId.value=DATA[39];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_unitonetime"); 
                   TextId.value=DATA[40];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_acponce"); 
                   TextId.value=DATA[41];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_tcponce"); 
                   TextId.value=DATA[42];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_aaponce"); 
                   TextId.value=DATA[43];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_taponce"); 
                   TextId.value=DATA[44];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_grandtcp"); 
                   TextId.value=DATA[45];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_grandtap"); 
                   TextId.value=DATA[46];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_unitsip"); 
                   TextId.value=DATA[47];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_aisip"); 
                   TextId.value=DATA[48];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_tasip"); 
                   TextId.value=DATA[49];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_unitfresh"); 
                   TextId.value=DATA[50];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_aifresh"); 
                   TextId.value=DATA[51];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_tafresh"); 
                   TextId.value=DATA[52];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_unitchurned"); 
                   TextId.value=DATA[53];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_aichurned"); 
                   TextId.value=DATA[54];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_tachurned"); 
                   TextId.value=DATA[55];
                   TextId.readOnly = false;
                   TextId=document.getElementById("txt_grandta"); 
                   TextId.value=DATA[56];
                   TextId.readOnly = false;
                   TextId=document.getElementById("TxtEffectiveDate"); 
                   TextId.value=DATA[57];
                   TextId.readOnly = true;
                   TextId=document.getElementById("TxtEndDate"); 
                   TextId.value='';
                   TextId=document.getElementById("HD");
                   TextId.value=DATA[58];
                    var TextBoxId=document.getElementById('txt_schemename');
                    TextBoxId.readOnly = true;
                    TextBoxId=document.getElementById('lst_periodicity');
                    TextBoxId.disabled = true;
                   
            }
        }
    }
        </script>
        <style>
            .headText {
                    text-align: center;
                    background: #ececec;
                    padding: 4px;
                    border-radius: 2px;
                   
            }
        </style>
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Target Schemes</h3>
        </div>

    </div>
     <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td style="text-align: left" id="TdNew">
                    <input id="BtnNew" type="button" value="New" class="btnUpdate btn btn-primary" style="width: 81px"
                        onclick="btn_New()" />
                </td>
            </tr>
            <tr>
                <td id="TdgrdTarget">
                    <dxe:ASPxGridView ID="grd_target" runat="server" KeyFieldName="tgt_masterid" ClientInstanceName="grid"
                        Width="100%" OnCustomCallback="grd_target_CustomCallback">
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="tgt_masterid" VisibleIndex="0" Visible="False">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="tgt_descripition" Caption="Name" VisibleIndex="0">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="tgt_periodicity" Caption="Periodicity" VisibleIndex="1">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Effective date" Caption="Effective Date"
                                VisibleIndex="2">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="End Date" Caption="End Date" VisibleIndex="3">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <Styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>
                        <SettingsBehavior AllowFocusedRow="True" AllowSort="False" />
                        <ClientSideEvents RowClick="function(s, e) { RowValue(e); }" />
                        <SettingsPager ShowSeparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                    </dxe:ASPxGridView>
                </td>
            </tr>
            <tr>
                <td id="TdgrdTargetDetails" style="text-align: left">
                    <dxe:ASPxGridView ID="grdtargetdetails" runat="server" Width="100%" KeyFieldName="tgt_id"
                        ClientInstanceName="grid1" OnCustomCallback="grdtargetdetails_CustomCallback">
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="tgt_id" VisibleIndex="0" Visible="false">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="tgt_descripition" Caption="Name" VisibleIndex="1">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="tgt_periodicity" Caption="Periodicity" VisibleIndex="2">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Effective date" Caption="Effective Date"
                                VisibleIndex="3">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="End Date" Caption="End Date" VisibleIndex="4">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <Styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>
                        <SettingsBehavior AllowFocusedRow="True" AllowSort="False" AllowMultiSelection="false" />
                        <ClientSideEvents RowClick="function(s, e) { RowValue1(e); }" />
                    </dxe:ASPxGridView>
                    <input id="BtnRevised" type="button" value="Revise" class="btnUpdate" onclick="BtnRevised_Click()" />
                </td>
            </tr>
            
            <tr>
                <td align="left" id="TdDetails">
                    <asp:Panel ID="Panel2" runat="server" Width="100%" >
                       
                        <table class="TableMain100">
                        
                            <tr>
                                <td colspan="2">
                                    <asp:Panel ID="Panel1" runat="server" Width="100%" BorderColor="white" BorderWidth="1px">
                                        <div class="TableMain100" id="TABLE1" onclick="return TABLE1_onclick()">
                                            <div class="clearfix">
                                                    <div class="headText">Target Scheme Details</div>
                                                <div class="row">
                                                    <div class="col-md-3">
                                                        <label>Scheme Name </label>
                                                        <div>
                                                            <asp:TextBox ID="txt_schemename" runat="server" Width="100%"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required!"
                                                                ControlToValidate="txt_schemename" ValidationGroup="a"></asp:RequiredFieldValidator></div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label>Periodicity </label>
                                                        <div>
                                                            <asp:DropDownList ID="lst_periodicity" runat="server" Width="100%">
                                                                <asp:ListItem>Monthly</asp:ListItem>
                                                                <asp:ListItem>Daily</asp:ListItem>
                                                                <asp:ListItem>Quarterly</asp:ListItem>
                                                                <asp:ListItem>Hale-Yearly</asp:ListItem>
                                                                <asp:ListItem>Yearly</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label>
                                                            Effective Date :
                                                        </label>
                                                        <div>
                                                            <asp:TextBox ID="TxtEffectiveDate" runat="server" Text="01/01/2007" Width="100%"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label>
                                                                            End Date :
                                                        </label>
                                                        <div>
                                                            <%-- <asp:TextBox id="TxtEndDate" tabIndex="19" runat="server" Width="100px" Font-Size="12px"></asp:TextBox>&nbsp;<asp:Image id="ImgStartDate" runat="server" ImageUrl="~/images/calendar.jpg"></asp:Image>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtEndDate" ValidationGroup="a"
                                                                    Display="Dynamic" EnableTheming="True" ErrorMessage="Date required!"></asp:RequiredFieldValidator>--%>
                                              
                                                                <dxe:ASPxDateEdit ID="TxtEndDate" ClientInstanceName="TxtEndDate" runat="server" EditFormat="Custom" UseMaskBehavior="true" Width="100%">
                                                                    <ButtonStyle Width="13px">
                                                                    </ButtonStyle>
                                                                </dxe:ASPxDateEdit>
                                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtEndDate"
                                                                            Display="Dynamic" EnableTheming="True" ErrorMessage="Date required!" ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                                                        </div>
                                                    </div>
                                                </div>
                                                </div>
                                                <div class="clearfix">
                                                    <div class="headText">Broking & Dp Operations</div>
                                                    <div class="row">
                                                    <div class="col-md-3">
                                                        <label>
                                                            Retails :
                                                        </label>
                                                        <div>
                                                            <asp:TextBox ID="txt_retails" runat="server"></asp:TextBox>
                                                            <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="*" ControlToValidate="txt_retails"
                                                                MaximumValue="999999" MinimumValue="0" Type="Integer"></asp:RangeValidator></div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label>
                                                            Derivatives :
                                                        </label>
                                                        <div>
                                                            <asp:TextBox ID="txt_derivatives" runat="server"></asp:TextBox>
                                                            <asp:RangeValidator ID="RangeValidator2" runat="server" ErrorMessage="*" ControlToValidate="txt_derivatives"
                                                                MaximumValue="999999" MinimumValue="0" Type="Integer"></asp:RangeValidator>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label>
                                                            HNI :
                                                        </label>
                                                        <div>
                                                            <asp:TextBox ID="txt_hni" runat="server"></asp:TextBox>
                                                            <asp:RangeValidator ID="RangeValidator3" runat="server" ErrorMessage="*" ControlToValidate="txt_hni"
                                                                MaximumValue="999999" MinimumValue="0" Type="Integer"></asp:RangeValidator></div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label>
                                                            Commodities :
                                                        </label>
                                                        <div>
                                                            <asp:TextBox ID="txt_commodities" runat="server"></asp:TextBox>
                                                            <asp:RangeValidator ID="RangeValidator4" runat="server" ErrorMessage="*" ControlToValidate="txt_commodities"
                                                                MaximumValue="999999" MinimumValue="0" Type="Integer"></asp:RangeValidator>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label>
                                                            Institution :
                                                        </label>
                                                        <div>
                                                            <asp:TextBox ID="txt_institution" runat="server"></asp:TextBox>
                                                            <asp:RangeValidator ID="RangeValidator5" runat="server" ErrorMessage="*" ControlToValidate="txt_institution"
                                                                MaximumValue="999999" MinimumValue="0" Type="Integer"></asp:RangeValidator></div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label>
                                                            Equities :
                                                        </label>
                                                        <div>
                                                            <asp:TextBox ID="txt_equities" runat="server" ></asp:TextBox>
                                                            <asp:RangeValidator ID="RangeValidator6" runat="server" ErrorMessage="*" ControlToValidate="txt_equities"
                                                                MaximumValue="999999" MinimumValue="0" Type="Integer"></asp:RangeValidator>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label>
                                                            New Clients :
                                                        </label>
                                                        <div>
                                                            <asp:TextBox ID="txt_newclients" runat="server"></asp:TextBox>
                                                            <asp:RangeValidator ID="RangeValidator7" runat="server" ErrorMessage="*" ControlToValidate="txt_newclients"
                                                                MaximumValue="999999" MinimumValue="0" Type="Integer"></asp:RangeValidator></div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label>
                                                            Gross Brokrage arning :
                                                        </label>
                                                        <div>
                                                            <asp:TextBox ID="txt_grossearning" runat="server"></asp:TextBox>
                                                            <asp:RangeValidator ID="RangeValidator8" runat="server" ErrorMessage="*" ControlToValidate="txt_grossearning"
                                                                MaximumValue="999999" MinimumValue="0" Type="Integer"></asp:RangeValidator>
                                                        </div>
                                                    </div>
                                                    </div>>
                                                </div>
                                                <div class="clearfix">
                                                    <div class="headText">Tele Sales & DST</div>
                                                    <div class="row">
                                                    <div class="col-md-3">
                                                        <label>
                                                            New Calls :
                                                        </label>
                                                        <div>
                                                            <asp:TextBox ID="txt_newcalls" runat="server"></asp:TextBox>
                                                            <asp:RangeValidator ID="RangeValidator9" runat="server" ErrorMessage="*" ControlToValidate="txt_newcalls"
                                                                MaximumValue="999999" MinimumValue="0" Type="Integer"></asp:RangeValidator></div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label>
                                                            New Sales Visit :
                                                        </label>
                                                        <div>
                                                            <asp:TextBox ID="txt_newsalesvisit" runat="server"></asp:TextBox>
                                                            <asp:RangeValidator ID="RangeValidator10" runat="server" ErrorMessage="*" ControlToValidate="txt_newsalesvisit"
                                                                MaximumValue="999999" MinimumValue="0" Type="Integer"></asp:RangeValidator>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label>
                                                            Cold Calls :
                                                        </label>
                                                        <div>
                                                            <asp:TextBox ID="txt_coldcalls" runat="server"></asp:TextBox>
                                                            <asp:RangeValidator ID="RangeValidator11" runat="server" ErrorMessage="*" ControlToValidate="txt_coldcalls"
                                                                MaximumValue="999999" MinimumValue="0" Type="Integer"></asp:RangeValidator></div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label>
                                                            Self Generated Sales Visits :
                                                        </label>
                                                        <div>
                                                            <asp:TextBox ID="txt_selfsalesvisit" runat="server"></asp:TextBox>
                                                            <asp:RangeValidator ID="RangeValidator12" runat="server" ErrorMessage="*" ControlToValidate="txt_selfsalesvisit"
                                                                MaximumValue="999999" MinimumValue="0" Type="Integer"></asp:RangeValidator>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label>
                                                            Total Calls :
                                                        </label>
                                                        <div>
                                                            <asp:TextBox ID="txt_totalcalls" runat="server"></asp:TextBox>
                                                            <asp:RangeValidator ID="RangeValidator13" runat="server" ErrorMessage="*" ControlToValidate="txt_totalcalls"
                                                                MaximumValue="999999" MinimumValue="0" Type="Integer"></asp:RangeValidator></div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label>
                                                            Sales Visit :
                                                        </label>
                                                        <div>
                                                            <asp:TextBox ID="txt_salesvisit" runat="server"></asp:TextBox>
                                                            <asp:RangeValidator ID="RangeValidator14" runat="server" ErrorMessage="*" ControlToValidate="txt_salesvisit"
                                                                MaximumValue="999999" MinimumValue="0" Type="Integer"></asp:RangeValidator>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label>
                                                            Calls To Salse Visit Ratio (%) :
                                                        </label>
                                                        <div>
                                                            <asp:TextBox ID="txt_salesvisitratio" runat="server"></asp:TextBox>
                                                            <asp:RangeValidator ID="RangeValidator15" runat="server" ErrorMessage="*" ControlToValidate="txt_salesvisitratio"
                                                                MaximumValue="999999" MinimumValue="0" Type="Integer"></asp:RangeValidator></div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label>
                                                            Visits To sales Ratio (%) :
                                                        </label>
                                                        <div>
                                                            <asp:TextBox ID="txt_sales_salesratio" runat="server"></asp:TextBox>
                                                            <asp:RangeValidator ID="RangeValidator16" runat="server" ErrorMessage="*" ControlToValidate="txt_sales_salesratio"
                                                                MaximumValue="999999" MinimumValue="0" Type="Integer"></asp:RangeValidator>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <label>
                                                            Calls To Sales Ratio (%) :
                                                        </label>
                                                        <div>
                                                            <asp:TextBox ID="txt_salesratio" runat="server"></asp:TextBox>
                                                            <asp:RangeValidator ID="RangeValidator17" runat="server" ErrorMessage="*" ControlToValidate="txt_salesratio"
                                                                MaximumValue="999999" MinimumValue="0" Type="Integer"></asp:RangeValidator></div>
                                                    </div>
                                                    </div>
                                                </div>
                                           
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="headText">Distribution</div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%; vertical-align: top">
                                    <asp:Panel ID="Panel3" runat="server" Width="100%" BorderColor="white" BorderWidth="1px">
                                        <table class="TableMain100">
                                            <tr>
                                                <td colspan="6" style="text-align: center">
                                                    <span style="color: #3300cc"><strong>Insurance</strong></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span style="font-size: 8pt; color: blue">PP Mode </span>
                                                </td>
                                                <td>
                                                    <span style="font-size: 8pt; color: blue">Units</span>
                                                </td>
                                                <td>
                                                    <span style="font-size: 8pt; color: blue">Avg. Collected Prm.</span>
                                                </td>
                                                <td>
                                                    <span style="font-size: 8pt; color: blue">Total. Collected Prm.</span>
                                                </td>
                                                <td>
                                                    <span style="font-size: 8pt; color: blue">Avg. Anualised Prm.</span>
                                                </td>
                                                <td>
                                                    <span style="font-size: 8pt; color: blue">Total Anualised Prm.</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span style="color: blue">Monthly </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_unitmonthly" runat="server" Width="42px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_acpmonthly" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_tcpmonthly" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_aapmonthly" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_tapmonthly" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span style="color: blue">Quarterly </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_unitquarterly" runat="server" Width="42px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_acpquartly" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_tcpquaterly" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_aapquaterly" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_tapquarterly" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span style="color: blue">Semi Annualy </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_unitsemiannual" runat="server" Width="42px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_acpsemiann" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_tcpsemiann" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_aapsemiann" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_tapsemiann" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span style="color: blue">Annualy </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_unitannual" runat="server" Width="42px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_acpann" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_tcpann" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_aapann" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_tapann" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span style="color: blue">One Time </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_unitonetime" runat="server" Width="42px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_acponce" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_tcponce" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_aaponce" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_taponce" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <span style="color: blue">Grand total : </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_grandtcp" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TextBox31" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_grandtap" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:Panel ID="Panel4" runat="server" Width="60%" BorderColor="white" BorderWidth="1px">
                                        <table class="TableMain100">
                                            <tr>
                                                <td colspan="4" style="text-align: center">
                                                    <span style="color: #3300cc"><strong>Mutual Funds</strong></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span style="font-size: 8pt; color: blue">Types</span>
                                                </td>
                                                <td>
                                                    <span style="font-size: 8pt; color: blue">Units</span>
                                                </td>
                                                <td>
                                                    <span style="font-size: 8pt; color: blue">Avg. Investment</span>
                                                </td>
                                                <td>
                                                    <span style="font-size: 8pt; color: blue">Total Amount</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span style="color: blue">SIPs </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_unitsip" runat="server" Width="42px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_aisip" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_tasip" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span style="color: blue">Fresh </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_unitfresh" runat="server" Width="42px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_aifresh" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_tafresh" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span style="color: blue">Churned </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_unitchurned" runat="server" Width="42px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_aichurned" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_tachurned" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <span style="color: blue">Grand Total : </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_grandta" runat="server" Width="87px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: center">
                                    <input id="btn_target_save"  type="button" value="Save" class="btnUpdate btn btn-primary" style="width: 64px"
                                        onclick="Btn_Save()" />
                                    <input id="BtnCancel" type="button" value="Cancel" class="btnUpdate btn btn-danger" style="width: 64px"
                                        onclick="TdShow()" />
                                    <input style="width: 151px; height: 17px" id="hdID" type="hidden" />
                                    <input style="width: 151px; height: 17px" id="HD" type="hidden" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
     </div>
</asp:Content>