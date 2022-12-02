<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_ContractNote" Codebehind="ContractNote.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxCallbackPanel"
    TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>--%>

<%--<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">    
    form {
		    display:inline;	
		    	
	    }
	 .frmleftCont{float:left; margin:2px; padding:2px; height:26px; border: solid 1px  #D1E0F3; font-size:12px;}
   
	</style>

    <script language="javascript" type="text/javascript">

  FieldName='lstSlection';
  var listBox="";
    function Page_Load()///Call Into Page Load
            {
//               if('<%=Session["ExchangeSegmentID"]%>'=='1' || '<%=Session["ExchangeSegmentID"]%>'=='4' || '<%=Session["ExchangeSegmentID"]%>'=='15' || '<%=Session["ExchangeSegmentID"]%>'=='19')
//                 {
//                     Hide('Tr_netforcommcdxfo');
//                 }
//             else
//                {
//                     Show('Tr_netforcommcdxfo');
//                }
                 Hide('Tab_showFilter');
                 //Hide('Td_Filter');
                 Hide('Td_Employee');
                 SessionCheck();
                 FnddlGeneration('1');
                 FnddlGroupBy(document.getElementById('ddlGroupBy').value);
                 height();
                 
                 
               
                 
                
            }
            
    function setDate()
        {
           var lastdate='<%=Session["StartdateFundsPayindate"] %>';
           var array=lastdate.split(',');
           var firstpart=array[0];
           var secondpart=array[1];
           var displayfirtstdate=new Date(firstpart);
           var displayseconddate=new Date(secondpart);
            cdtFromDate.SetDate(displayfirtstdate); 
            cdtToDate.SetDate(displayfirtstdate);
        }
     function DateChange(DateObj)
       {
            var FYS ='<%=Session["FinYearStart"]%>';
            var FYE ='<%=Session["FinYearEnd"]%>'; 
            var LFY ='<%=Session["LastFinYear"]%>';
            DevE_CheckForFinYear(DateObj,FYS,FYE,LFY);
            DevE_CompareDateFromMax(cdtFromDate,cdtToDate,'FromDate cannot be greater than ToDate!!!.');
       }
       
     function setDateExceptCM()
        {
             var settodate=new Date();
             var curyr=settodate.getFullYear();
             
             var LFY ='<%=Session["LastFinYear"]%>';
             var arrfinyr=LFY.split('-');
             var lastyr=arrfinyr[1];
            
             var lastdate='<%=Session["StartdateFundsPayindate"] %>';
             var array=lastdate.split(',');
             var secondpart=array[1];
             var displayseconddate=new Date(secondpart);
            
             cdtFromDate.SetDate(displayseconddate); 
             cdtToDate.SetDate(displayseconddate);
             if(settodate<displayseconddate)
             {
                cdtFromDate.SetDate(settodate);
                cdtToDate.SetDate(settodate);
             }
        }     
       
          
    function DateChangeforExpiryRangeFrom(DateObj)  
       {
            var targetdate=DateObj.GetDate();
            cdtFromDate.SetDate(targetdate);
       }
   
   function DateChangeforExpiryRangeTo(DateObj)
       {
            var targetdate=DateObj.GetDate();
           cdtToDate.SetDate(targetdate);
       
       }           
            
    function alertcall(value)
        {
            
          if(value=='true')
            alert('Mail Send Successfully');
          else
            alert('Error on sending');
        
        }
   
   function SessionCheck()
   {
     if('<%=Session["ExchangeSegmentID"]%>'=='1' || '<%=Session["ExchangeSegmentID"]%>'=='4' || '<%=Session["ExchangeSegmentID"]%>'=='15' || '<%=Session["ExchangeSegmentID"]%>'=='19')
     {
        document.getElementById('lblCurrentSettlementNo').innerText='<%=Session["LastSettNo"]%>';
        
       
            FnSelection('1');
           
            
        
        
     }
     else
     {
        FnSelection('2');
        

     }
   }
   function height()
        {
            if(document.body.scrollHeight>=380)
            {
                window.frameElement.height = document.body.scrollHeight;
            }
            else
            {
                window.frameElement.height = '380px';
            }
            window.frameElement.width = document.body.scrollWidth;
        }
    function Hide(obj)
            {
             document.getElementById(obj).style.display='none';
            }
    function Show(obj)
            {
             document.getElementById(obj).style.display='inline';
            }
            
   
    function  FnddlGroupBy(obj)
     {
        if(obj=='Group')
        {
            Hide('Td_OtherGroupBy');
            Show('Td_Group');
            document.getElementById('BtnGroup').click();
        }
        else
        {
            Show('Td_OtherGroupBy');
            Hide('Td_Group');
        }
     }
  function FnOtherGroupBy(obj)
    {
        if(document.getElementById('DdlGeneRationType').value!='4')
            {
               if(obj=="a")
                    Hide('Tab_showFilter');
                 else
                 {
                     if(document.getElementById('ddlGroupBy').value=='Clients')
                         document.getElementById('cmbsearchOption').value='Clients';
                     if(document.getElementById('ddlGroupBy').value=='Branch')
                         document.getElementById('cmbsearchOption').value='Branch';
                     Show('Tab_showFilter');
                 }
            }
        else
            {
                
                if(obj=="a")
                {
                    Hide('Tab_showFilter');
                    Hide('Tr_Date');
                         Hide('td_fordate');
                         Hide('td_from');
                         Hide('td_date');
                         Show('tr_selectiontype');
                         Hide('td_to');
                }
                else
                    {
                         if(document.getElementById('ddlGroupBy').value=='Clients')
                         {
                            document.getElementById('cmbsearchOption').value='Clients';
                       
                    
                        if(obj=="b")
                         {
                            
                                 Show('Tr_Date');
                                 Hide('td_fordate');
                                 Show('td_from');
                                 Show('td_date');
                                 Hide('tr_selectiontype');
                                 Show('td_to');
                         }
                        if(obj=="c")
                         {
                                
                                Hide('Tr_Date');
                                 Hide('td_fordate');
                                 Hide('td_from');
                                 Hide('td_date');
                                 Show('tr_selectiontype');
                                 Hide('td_to');
                         }
                         Show('Tab_showFilter');
                 }
                
                  if(document.getElementById('ddlGroupBy').value=='Branch')
                  {
                    document.getElementById('cmbsearchOption').value='Branch';
                     Hide('Tr_Date');
                         Hide('td_fordate');
                         Hide('td_from');
                         Hide('td_date');
                         Show('tr_selectiontype');
                         Hide('td_to');
                    }
                     }
            }
         
    }
 function FnGroup(obj)
  {
        if(obj=="a")
            Hide('Tab_showFilter');
         else
         {
              document.getElementById('cmbsearchOption').value='Group';
              Show('Tab_showFilter');
         }
        
  }
  function ajaxFunction()
    {
           
            
            var aa="";
            var path="";
            path=document.getElementById('hdnpath').value;
            var xmlhttp;
            if (window.XMLHttpRequest)
            {
                xmlhttp=new XMLHttpRequest();
            }
            else
            {
                xmlhttp=new ActiveXObject("Microsoft.XMLHTTP");
                
            }
            xmlhttp.onreadystatechange=function()
            {
                 if(xmlhttp.readyState==4)
                 {       
                     var a=xmlhttp.responseText;
                     if(a!="")
                     {
                         document.getElementById('Div1').style.display='inline';           
                         CreateFile(a);
                     }
                 }
            }
            //xmlhttp.open("GET","Defaultnew.aspx?path="+path+"",true); 
            xmlhttp.open("GET","Defaultnew.aspx?path="+path+"&reportformat=",true); 
            xmlhttp.send(null);
            
    }
     function CreateFile(value)
     {
           var rand_no = Math.random();        
            var now = new Date();
            var then = now.getFullYear()+'_'+(now.getMonth()+1)+'_'+now.getDate();
                then += '_'+now.getHours()+'_'+now.getMinutes()+'_'+now.getSeconds(); 
            var destination=document.getElementById('hdnLocationPath').value;
            var fso ;
            try
            {
                fso = new ActiveXObject("Scripting.FileSystemObject");
            }
            catch(err)
            {
                alert("Please Enable The option\n'Initialize and Script Active X controls not marked as safe for scripting' \n Under 'Active X Controls & Piug-Ins' \n from Internet options -> Security Settings");

            }
            varFileObject = fso.OpenTextFile(destination, 2, true,0);
            varFileObject.write(value);
            varFileObject.close();
            alert("Print Send To Printer");
            document.getElementById('Div1').style.display='none'; 
    }
    function showdiv()
   {
         show('Div1');
   }
   function hidediv()
  {
  
        hide('Div1');
  }
  function CallAjax(obj1,obj2,obj3)
         { 
            ajax_showOptions(obj1,obj2,obj3);
         }
  Fieldname = 'none'
      function FunCallAjaxList(objID,objEvent,ObjType)
        {
               
               var strQuery_Table = '';
               var strQuery_FieldName = '';
               var strQuery_WhereClause = '';
               var strQuery_OrderBy='';
               var strQuery_GroupBy='';
               var CombinedQuery='';
               
            
                 if(ObjType=='Sign')
                {
                    strQuery_Table = "tbl_master_contact contact,tbl_master_employee e,tbl_master_document";
                    strQuery_FieldName = " top 10 (contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName +'['+contact.cnt_shortName+']') as Name ,e.emp_contactid";
                    strQuery_WhereClause = " contact.cnt_firstName Like (\'%RequestLetter%') and e.emp_contactId=contact.cnt_internalid and doc_contactId=e.emp_contactid and doc_documentTypeId=(select top 1 dty_id from tbl_master_documentType where dty_documentType='Signature' and dty_applicableFor='Employee') ";
                   
                }
                else
                {   
                   
                  if(document.getElementById('cmbsearchOption').value=="Branch")
                   {
                    if('<%=Session["ExchangeSegmentID"]%>'=='1' || '<%=Session["ExchangeSegmentID"]%>'=='4' || '<%=Session["ExchangeSegmentID"]%>'=='15' || '<%=Session["ExchangeSegmentID"]%>'=='19')
                         {
                              if(document.getElementById('DdlGeneRationType').value!="2")
                              {
                                   strQuery_Table = "tbl_master_branch";
                                   strQuery_FieldName = "top 10 branch_description+'-'+branch_code,branch_id";
                                   strQuery_WhereClause = " (branch_description Like (\'%RequestLetter%') or branch_code like (\'%RequestLetter%')) and branch_id in (<%=Session["userbranchHierarchy"]%>)";
                              }
                             else
                              {
                                    
//                                    var settnobranch='<%=Session["LastSettNo"]%>'
//                                    var finalsettbranch=settnobranch.substr(0,7);
//                                    var setttypebranch=settnobranch.substr(7,1);
//                                    
//                                    strQuery_Table = "tbl_master_branch,trans_contractnotes,tbl_master_email,tbl_master_contact";
//                                    strQuery_FieldName = "Distinct top 10 branch_description+'-'+branch_code,branch_id";
//                                    strQuery_WhereClause = " (branch_description Like (\'%RequestLetter%') or branch_code like (\'%RequestLetter%')) and branch_id in (<%=Session["userbranchHierarchy"]%>) and branch_id=contractnotes_branchid and contractnotes_companyid in ('<%=Session["LastCompany"]%>') and contractnotes_finyear in ('<%=Session["LastFinYear"]%>') and contractnotes_segmentid in (<%=Session["usersegid"]%>) and ContractNotes_SettlementNumber='"+ finalsettbranch +"' and ContractNotes_SettlementType='"+ setttypebranch +"' and cnt_internalId=ContractNotes_CustomerID and cnt_internalId=eml_cntId and eml_cntId=ContractNotes_CustomerID and isnull(cnt_ContractDeliveryMode,'P')<>'P' and eml_type='Official'";
                                        strQuery_Table = "tbl_master_branch";
                                        strQuery_FieldName = "top 10 branch_description+'-'+branch_code,branch_id";
                                        strQuery_WhereClause = " (branch_description Like (\'%RequestLetter%') or branch_code like (\'%RequestLetter%')) and branch_id in (<%=Session["userbranchHierarchy"]%>)";
                              }
                         }
                    else
                      {
                           strQuery_Table = "tbl_master_branch";
                           strQuery_FieldName = "top 10 branch_description+'-'+branch_code,branch_id";
                           strQuery_WhereClause = " (branch_description Like (\'%RequestLetter%') or branch_code like (\'%RequestLetter%')) and branch_id in (<%=Session["userbranchHierarchy"]%>)";
                      
                      }
                   }
                    else if(document.getElementById('cmbsearchOption').value=="EMail")
                    {
                       strQuery_Table = "tbl_master_email INNER JOIN tbl_master_contact ON tbl_master_email.eml_cntId = tbl_master_contact.cnt_internalId";
                       strQuery_FieldName = "top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'') + '< '+ tbl_master_email.eml_email + '>' as name,cnt_internalId as contactid";
                       strQuery_WhereClause = " (tbl_master_email.eml_email <> '') AND cnt_contacttype ='EM'  and (cnt_firstName Like (\'%RequestLetter%') or cnt_ucc like (\'%RequestLetter%') or tbl_master_email.eml_email like (\'%RequestLetter%') )";
                    }
                    else if(document.getElementById('cmbsearchOption').value=="Group")
                    {
                         if('<%=Session["ExchangeSegmentID"]%>'=='1' || '<%=Session["ExchangeSegmentID"]%>'=='4' || '<%=Session["ExchangeSegmentID"]%>'=='15' || '<%=Session["ExchangeSegmentID"]%>'=='19')
                         {
                              if(document.getElementById('DdlGeneRationType').value!="2")
                              {
                                   strQuery_Table = "tbl_master_groupmaster";
                                   strQuery_FieldName = "Distinct top 10 gpm_description+'-'+gpm_code ,gpm_id";
                                   strQuery_WhereClause = " (gpm_description Like (\'%RequestLetter%') or gpm_code like (\'%RequestLetter%')) and gpm_type='"+document.getElementById('ddlGroup').value+"'";
                              }
                              
                              else
                              {
                              
//                                    var settnogroup='<%=Session["LastSettNo"]%>'
//                                    var finalsettgroup=settnogroup.substr(0,7);
//                                    var setttypegroup=settnogroup.substr(7,1);
//                                   strQuery_Table = "tbl_master_groupmaster,tbl_trans_group,tbl_master_email,trans_contractnotes,tbl_master_contact";
//                                   strQuery_FieldName = "Distinct top 10 gpm_description+'-'+gpm_code ,gpm_id";
//                                   strQuery_WhereClause = " (gpm_description Like (\'%RequestLetter%') or gpm_code like (\'%RequestLetter%')) and gpm_type='"+document.getElementById('ddlGroup').value+"' and gpm_id=grp_groupMaster and contractnotes_companyid in ('<%=Session["LastCompany"]%>') and contractnotes_finyear in ('<%=Session["LastFinYear"]%>') and contractnotes_segmentid in (<%=Session["usersegid"]%>) and ContractNotes_SettlementNumber='"+ finalsettgroup +"' and ContractNotes_SettlementType='"+ setttypegroup +"' and ContractNotes_CustomerID=grp_contactId and grp_contactId=eml_cntId and ContractNotes_CustomerID=eml_cntId and eml_type='Official'  and grp_contactId=cnt_internalId and ContractNotes_CustomerID=cnt_internalId and eml_cntId=cnt_internalId and isnull(cnt_ContractDeliveryMode,'P')<>'P'";
                                   strQuery_Table = "tbl_master_groupmaster";
                                   strQuery_FieldName = "Distinct top 10 gpm_description+'-'+gpm_code ,gpm_id";
                                   strQuery_WhereClause = " (gpm_description Like (\'%RequestLetter%') or gpm_code like (\'%RequestLetter%')) and gpm_type='"+document.getElementById('ddlGroup').value+"'";
                              
                              }
                         }
                         
                        else
                         {
                         
                                   strQuery_Table = "tbl_master_groupmaster";
                                   strQuery_FieldName = "Distinct top 10 gpm_description+'-'+gpm_code ,gpm_id";
                                   strQuery_WhereClause = " (gpm_description Like (\'%RequestLetter%') or gpm_code like (\'%RequestLetter%')) and gpm_type='"+document.getElementById('ddlGroup').value+"'";
                         }
                    }
                 else if(document.getElementById('cmbsearchOption').value=="Clients")
                    {
                        if(document.getElementById('DdlGeneRationType').value!="4")
                        {
                               if('<%=Session["ExchangeSegmentID"]%>'=='1' || '<%=Session["ExchangeSegmentID"]%>'=='4' || '<%=Session["ExchangeSegmentID"]%>'=='15' || '<%=Session["ExchangeSegmentID"]%>'=='19')
                                 {
                                      if(document.getElementById('DdlGeneRationType').value!="2")
                                      {
                                           strQuery_Table = "tbl_master_contact,tbl_master_contactexchange,tbl_master_branch";
                                           strQuery_FieldName = "distinct top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID";
                                           strQuery_WhereClause = "  branch_id=cnt_branchid and crg_exchange='"+document.getElementById('HiddenField_SegmentName').value+"' and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' and (crg_tcode Like (\'RequestLetter%') or CNT_FIRSTNAME like (\'RequestLetter%')) and cnt_branchid in (<%=Session["userbranchHierarchy"]%>)";
                                      }
                                      else
                                      {
                                           strQuery_Table = "tbl_master_contact,tbl_master_contactexchange,tbl_master_branch";
                                           strQuery_FieldName = "distinct top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID";
                                           strQuery_WhereClause = "  branch_id=cnt_branchid and crg_exchange='"+document.getElementById('HiddenField_SegmentName').value+"' and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' and (crg_tcode Like (\'RequestLetter%') or CNT_FIRSTNAME like (\'RequestLetter%')) and cnt_branchid in (<%=Session["userbranchHierarchy"]%>)";
                                      }
                                 }
                                 
                                 else
                                   {
                                        strQuery_Table = "tbl_master_contact,tbl_master_contactexchange,tbl_master_branch";
                                        strQuery_FieldName = "distinct top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID";
                                        strQuery_WhereClause = "  branch_id=cnt_branchid and crg_exchange='"+document.getElementById('HiddenField_SegmentName').value+"' and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' and (crg_tcode Like (\'RequestLetter%') or CNT_FIRSTNAME like (\'RequestLetter%')) and cnt_branchid in (<%=Session["userbranchHierarchy"]%>)";
                                    
                                   
                                   }
                           }
                        else
                            {
                                if(listBox.length>0)
                                {
                                    strQuery_Table = "tbl_master_contact";
                                    strQuery_FieldName = "distinct top 1 'Can not select more than one client in date range for HTML',12345678987654321 ";
                                }
                                else
                                {
                                    if('<%=Session["ExchangeSegmentID"]%>'=='1' || '<%=Session["ExchangeSegmentID"]%>'=='4' || '<%=Session["ExchangeSegmentID"]%>'=='15' || '<%=Session["ExchangeSegmentID"]%>'=='19')
                                     {
                                          if(document.getElementById('DdlGeneRationType').value!="2")
                                          {
                                               strQuery_Table = "tbl_master_contact,tbl_master_contactexchange,tbl_master_branch";
                                               strQuery_FieldName = "distinct top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID";
                                               strQuery_WhereClause = "  branch_id=cnt_branchid and crg_exchange='"+document.getElementById('HiddenField_SegmentName').value+"' and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' and (crg_tcode Like (\'RequestLetter%') or CNT_FIRSTNAME like (\'RequestLetter%')) and cnt_branchid in (<%=Session["userbranchHierarchy"]%>)";
                                          }
                                          else
                                          {
                                               strQuery_Table = "tbl_master_contact,tbl_master_contactexchange,tbl_master_branch";
                                               strQuery_FieldName = "distinct top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID";
                                               strQuery_WhereClause = "  branch_id=cnt_branchid and crg_exchange='"+document.getElementById('HiddenField_SegmentName').value+"' and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' and (crg_tcode Like (\'RequestLetter%') or CNT_FIRSTNAME like (\'RequestLetter%')) and cnt_branchid in (<%=Session["userbranchHierarchy"]%>)";
                                          }
                                     }
                                     
                                     else
                                       {
                                            strQuery_Table = "tbl_master_contact,tbl_master_contactexchange,tbl_master_branch";
                                            strQuery_FieldName = "distinct top 10 isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(crg_tcode),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID";
                                            strQuery_WhereClause = "  branch_id=cnt_branchid and crg_exchange='"+document.getElementById('HiddenField_SegmentName').value+"' and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' and (crg_tcode Like (\'RequestLetter%') or CNT_FIRSTNAME like (\'RequestLetter%')) and cnt_branchid in (<%=Session["userbranchHierarchy"]%>)";
                                        
                                       
                                       }
                                    }
                            
                            }
                    }
                    else
                    {
                        
                       if(document.getElementById('ddlGroupBy').value=="Clients")
                       {
                           strQuery_Table = "trans_contractnotes ";
                           strQuery_FieldName = " Distinct top 10 contractnotes_number,contractnotes_number";
                           
                           if(document.getElementById('RadioBtnOtherGroupBySelected').checked)
                               strQuery_WhereClause = " contractnotes_number like (\'%RequestLetter%') and contractnotes_companyid in ('<%=Session["LastCompany"]%>') and contractnotes_finyear in ('<%=Session["LastFinYear"]%>') and contractnotes_segmentid in (<%=Session["usersegid"]%>) and contractnotes_customerid  in ("+document.getElementById('HiddenField_Client').value+")";
                           else if(document.getElementById('RadioBtnOtherGroupByallbutSelected').checked)
                               strQuery_WhereClause = " contractnotes_number like (\'%RequestLetter%') and contractnotes_companyid in ('<%=Session["LastCompany"]%>') and contractnotes_finyear in ('<%=Session["LastFinYear"]%>') and contractnotes_segmentid in (<%=Session["usersegid"]%>) and contractnotes_customerid not in ("+document.getElementById('HiddenField_Client').value+")";
                           else
                               strQuery_WhereClause = " contractnotes_number like (\'%RequestLetter%') and contractnotes_companyid in ('<%=Session["LastCompany"]%>') and contractnotes_finyear in ('<%=Session["LastFinYear"]%>') and contractnotes_segmentid in (<%=Session["usersegid"]%>)";
                       }
                       if(document.getElementById('ddlGroupBy').value=="Branch")
                       {
                           strQuery_Table = "trans_contractnotes ";
                           strQuery_FieldName = " Distinct top 10 contractnotes_number,contractnotes_number";
                           
                           if(document.getElementById('RadioBtnOtherGroupBySelected').checked)
                               strQuery_WhereClause = " contractnotes_number like (\'%RequestLetter%') and contractnotes_companyid in ('<%=Session["LastCompany"]%>') and contractnotes_finyear in ('<%=Session["LastFinYear"]%>') and contractnotes_segmentid in (<%=Session["usersegid"]%>) and contractnotes_customerid  in (select cnt_internalid from tbl_master_contact where cnt_branchid in("+document.getElementById('HiddenField_Branch').value+"))";
                           else if(document.getElementById('RadioBtnOtherGroupByallbutSelected').checked)
                               strQuery_WhereClause = " contractnotes_number like (\'%RequestLetter%') and contractnotes_companyid in ('<%=Session["LastCompany"]%>') and contractnotes_finyear in ('<%=Session["LastFinYear"]%>') and contractnotes_segmentid in (<%=Session["usersegid"]%>) and contractnotes_customerid not in (select cnt_internalid from tbl_master_contact where cnt_branchid in("+document.getElementById('HiddenField_Branch').value+"))";
                           else
                               strQuery_WhereClause = " contractnotes_number like (\'%RequestLetter%') and contractnotes_companyid in ('<%=Session["LastCompany"]%>') and contractnotes_finyear in ('<%=Session["LastFinYear"]%>') and contractnotes_segmentid in (<%=Session["usersegid"]%>)";
                              
                       }
                       if(document.getElementById('ddlGroupBy').value=="Group")
                       {
                           strQuery_Table = "trans_contractnotes ";
                           strQuery_FieldName = " Distinct top 10 contractnotes_number,contractnotes_number";
                           
                           if(document.getElementById('RadioBtnGroupSelected').checked)
                               strQuery_WhereClause = " contractnotes_number like (\'%RequestLetter%') and contractnotes_companyid in ('<%=Session["LastCompany"]%>') and contractnotes_finyear in ('<%=Session["LastFinYear"]%>') and contractnotes_segmentid in (<%=Session["usersegid"]%>) and contractnotes_customerid  in (select grp_contactid from tbl_trans_group where grp_groupmaster in("+document.getElementById('HiddenField_Group').value+"))";
                           else if(document.getElementById('RadioBtnGroupallbutSelected').checked)
                               strQuery_WhereClause = " contractnotes_number like (\'%RequestLetter%') and contractnotes_companyid in ('<%=Session["LastCompany"]%>') and contractnotes_finyear in ('<%=Session["LastFinYear"]%>') and contractnotes_segmentid in (<%=Session["usersegid"]%>) and contractnotes_customerid not in (select grp_contactid from tbl_trans_group where grp_groupmaster in("+document.getElementById('HiddenField_Group').value+"))";
                           else
                               strQuery_WhereClause = " contractnotes_number like (\'%RequestLetter%') and contractnotes_companyid in ('<%=Session["LastCompany"]%>') and contractnotes_finyear in ('<%=Session["LastFinYear"]%>') and contractnotes_segmentid in (<%=Session["usersegid"]%>)";
                       }
                    }
                   
                }
                 CombinedQuery=new String(strQuery_Table+"$"+strQuery_FieldName+"$"+strQuery_WhereClause+"$"+strQuery_OrderBy+"$"+strQuery_GroupBy);
                 ajax_showOptions(objID,'GenericAjaxList',objEvent,replaceChars(CombinedQuery));
        }

       function replaceChars(entry) 
       {
            out = "+"; // replace this
            add = "--"; // with this
            temp = "" + entry; // temporary holder

            while (temp.indexOf(out)>-1) {
            pos= temp.indexOf(out);
            temp = "" + (temp.substring(0, pos) + add + 
            temp.substring((pos + out.length), temp.length));
            }
            return temp;
        } 
    
    function btnAddsubscriptionlist_click()
            {
            
                var cmb=document.getElementById('cmbsearchOption');
                        var userid = document.getElementById('txtSelectionID');
                        //alert(userid.value);
                        if(userid.value != '')
                        {
                            if(userid.value != 'Can not select more than one client in date range for HTML')
                            {
                                 var ids = document.getElementById('txtSelectionID_hidden');
    //                            var listBox = document.getElementById('lstSlection');
                                listBox = document.getElementById('lstSlection');
                                var tLength = listBox.length;
                               
                                
                                var no = new Option();
                                no.value = ids.value;
                                no.text = userid.value;
                                listBox[tLength]=no;
                                var recipient = document.getElementById('txtSelectionID');
                                recipient.value='';
                            }
                            else
                            {
                               alert('Please Remove Selected Client then select another one !!');
                               document.getElementById('txtSelectionID').value='';
                            }
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
            
                Hide('Tab_showFilter');
                document.getElementById('BtnGenerate').disabled=false;
	        }
	     
	        
	   function btnRemovefromsubscriptionlist_click()
            {
                
//                var listBox = document.getElementById('lstSlection');
                listBox = document.getElementById('lstSlection');
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
    
   function FnSelection(obj)
   {
        if(obj=="1")
        {
             Show('Td_CurrentSettlementNo');
             Hide('Tr_Date');
             Hide('Tr_ContractNo');
             Hide('Tab_showFilter');
        }
        else if(obj=="2")
        {
             
             Hide('Td_CurrentSettlementNo');
             Show('Tr_Date');
             Hide('Tr_ContractNo');
             Hide('Tab_showFilter');
             if('<%=Session["ExchangeSegmentID"]%>'=='1' || '<%=Session["ExchangeSegmentID"]%>'=='4' || '<%=Session["ExchangeSegmentID"]%>'=='15' || '<%=Session["ExchangeSegmentID"]%>'=='19')
                 {
                     setDate();
                 }
                 else
                 {
                    setDateExceptCM();
                 
                 }
             
            
        }
        else if(obj=="3")
        {
             Hide('Td_CurrentSettlementNo');
             Hide('Tr_Date');
             Show('Tr_ContractNo');
             FnCntrNo('S');
            
        }
   }
   function FnCntrNo(obj)
   {                                                                          
        if(obj=="S")
        {
            Hide('Td_Range');
            Show('Tab_showFilter');
            document.getElementById('cmbsearchOption').value='Contractnoteno';
        }
       if (obj=="R")
        {
            Show('Td_Range');
            Hide('Tab_showFilter');
        }
       
   }
    function ShowHideFilter(obj)
     {
        cCbptxtSelectionID.PerformCallback(obj);
   
     }
     function fn_html()
        {
            //alert(document.getElementById('ddlOutputtype').value);
            if(document.getElementById('ddlOutputtype').value=='1')
                {
                    document.getElementById('img_id').src="/assests/images/contractimages.gif";
                    document.getElementById('S1').innerHTML="Please Wait..";
                }
            else
                {
                    document.getElementById('img_id').src="../images/Animated_Email.gif";
                    document.getElementById('img_id').width=64;
                    document.getElementById('img_id').height=64;
                    document.getElementById('S1').innerHTML="";
                }
            Show('td_image');
            cCbpSuggestISIN.PerformCallback('html');
        }
     
     function CbptxtSelectionID_EndCallBack()
     {
         if (cCbptxtSelectionID.cpproperties=="Export")
          {
     
                document.getElementById('BtnForExportEvent').click();
          }
          if (cCbptxtSelectionID.cpproperties=="Exportall")
          {
     
                document.getElementById('BtnForExportEvent1').click();
          }
          if (cCbptxtSelectionID.cpproperties=="Exportdelivery")
          {
     
                document.getElementById('BtnForExportEvent2').click();
          }
           if((cCbptxtSelectionID.cpallcontract != null) && (cCbptxtSelectionID.cpecnenable != null) && (cCbptxtSelectionID.cpdeliveryrpt != null))
            {
                document.getElementById('<%=B_allcontract.ClientID %>').innerHTML=cCbptxtSelectionID.cpallcontract;
                document.getElementById('<%=B_ecnenable.ClientID %>').innerHTML=cCbptxtSelectionID.cpecnenable;
                document.getElementById('<%=B_deliveryrpt.ClientID %>').innerHTML=cCbptxtSelectionID.cpdeliveryrpt;
                
                Show('ecndetail');
                if(cCbptxtSelectionID.cpecnenable=='0')
                  btnopenpopup.SetEnabled(false);
                else
                  btnopenpopup.SetEnabled(true);
            }
            if ((cCbptxtSelectionID.cpallcontractpop != null) && (cCbptxtSelectionID.cpecnenablepop != null))
            {
                document.getElementById('<%=B_allcontractpop.ClientID %>').innerHTML=cCbptxtSelectionID.cpallcontractpop;
                document.getElementById('<%=B_ecnenablepop.ClientID %>').innerHTML=cCbptxtSelectionID.cpecnenablepop;
                var remn=document.getElementById('<%=B_allcontractpop.ClientID %>').innerHTML=cCbptxtSelectionID.cpallcontractpop;
                var remn1=document.getElementById('<%=B_ecnenablepop.ClientID %>').innerHTML=cCbptxtSelectionID.cpecnenablepop;
                if (remn=='0' || remn1=='0')
                {
                    
                    Hide('btnremain');
                }
                else
                {
                    Show('btnremain');
                }
                Hide('btnokdiv');
                Hide('div_fail');
                Hide('div_success');
                cPopUp_ScripAlert.Show();
            
            }
            if ((cCbptxtSelectionID.cpvisibletrue =="yes")|| (cCbptxtSelectionID.cpecnenable=='0'))
            btnopenpopup.SetEnabled(false);
            if ((cCbptxtSelectionID.cpvisibletrue =="no")&& (cCbptxtSelectionID.cpecnenable!='0'))
            btnopenpopup.SetEnabled(true);
      }
    function CbpSuggestISIN_EndCallBack()
    {
//        alert(cCbpSuggestISIN.cpdownloadcomplete11);
//        alert(cCbpSuggestISIN.cpdownloadcomplete12);
        if (cCbpSuggestISIN.cpdownloadcomplete != null)
            {
            
                if (cCbpSuggestISIN.cpdownloadcomplete='yes')
                    {
                        Hide('td_image');
                        document.getElementById('BtnForExportEvent3').click();
                    }
            }
            
             if (cCbpSuggestISIN.cpsuccessandfailmsg != null)
              {
                if (cCbpSuggestISIN.cpsuccessandfailmsg=='totalsuccess')
                    {
                        
                        alert('All Mail Send Successfully');
                        FnddlGeneration('4');
                       
                        
                    }
                     if (cCbpSuggestISIN.cpsuccessandfailmsg=='totalfail')
                    {
                        
                        alert('Official Emailid not found for all sending mail');
                        FnddlGeneration('4');
                       
                        
                    }
                     if (cCbpSuggestISIN.cpsuccessandfailmsg=='fewsuccessandfewfail')
                    {
                        
                        alert('Some Official Emailid not found \n Rest emails has been Sent.');
                        FnddlGeneration('4');
                       
                        
                    }
            }
            
             if (cCbpSuggestISIN.cpnorecord != null)
                {
                if (cCbpSuggestISIN.cpnorecord='norecord')
                    {
                        
                        alert('No Record Found.');
                        FnddlGeneration('4');
                       
                        
                    }
                }
            
        if ((cCbpSuggestISIN.cpallcontractpops != null) && (cCbpSuggestISIN.cpecnenablepops != null))
            {
                
                
                document.getElementById('<%=B_allcontractpop.ClientID %>').innerHTML=cCbpSuggestISIN.cpallcontractpops;
                document.getElementById('<%=B_ecnenablepop.ClientID %>').innerHTML=cCbpSuggestISIN.cpecnenablepops;
                var remn2=cCbpSuggestISIN.cpallcontractpops;
                var remn3=cCbpSuggestISIN.cpecnenablepops;
                if (remn2=='0' || remn3=='0')
               {                   
                    Hide('btnremain');
                    Hide('btnall');
                    Hide('cancel');

                    
                }
                else
                {
                Show('btnremain');
                Hide('btnall');
                Hide('cancel');

                   
                }
                if (remn3!='0')
                //if('<%=Session["Error"]%>'=='err')
                {
                    Show('div_fail');
                    Hide('div_success');
               
                }
                else
                {
                    Hide('div_fail');
                      Show('div_success');
                      
                }
                 Show('btnokdiv');
                 
                
            }
    }
    function btnok_Click()  
    {
        window.location='../reports/ContractNote.aspx';
    }
    function fnRecord()
        {
            cPopup_ContractNoteDetails.Show();
        }
    function btncancel_Click()
    {

        cPopUp_ScripAlert.Hide();
    }
    function btnSubmit_Click()
    {
        var single=crdbSingle.GetValue();
       
        var combined=crdbCombined.GetValue();
        
        if(combined==true)
       
        window.location='../Reports/ContractNote_CombinedSegment.aspx';
        
        else
        cPopup_ContractNoteDetails.Hide();
       
    }      
    function fn_showpopup()
    {
    

if(document.getElementById('txtdigitalName_hidden').value!="")
{
       
        cCbptxtSelectionID.PerformCallback('v5');
 }
 else
 {
     alert('Please select Signature !!');
 }
        
    }
    function btnsendall_Click()
    {
        cCbpSuggestISIN.PerformCallback('all');
    }
    function btnsendremaining_Click()
    {
        cCbpSuggestISIN.PerformCallback('remain');
    }
         
   function fn_show()
   {



 Show('Tr_DigitalSign');
cCbptxtSelectionID.PerformCallback('v4');
   
        
   }
   
   
   
   function Remove()
   {
        if('<%=Session["ExchangeSegmentID"]%>'!='1' && '<%=Session["ExchangeSegmentID"]%>'!='4' && '<%=Session["ExchangeSegmentID"]%>'!='15' && '<%=Session["ExchangeSegmentID"]%>'!='19')
        {
          
//       }
//       else
//        {
           var DropDownList=document.getElementById('DdlGeneRationType');

           for(i=DropDownList.length-1; i>=0; i--) 
           {
               if(DropDownList.options[i].value=='4')
                  DropDownList.remove(i);         
               
           }
       }
   }
function fnSetDocuType(obj)
  {
  
   if(obj=="2")
        document.getElementById('docu_type').style.visibility = "visible";
   else
        document.getElementById('docu_type').style.visibility = "hidden";
  
  }

   
   
   function FnddlGeneration(obj)
   {
//   functioninvisible(obj);
   Remove();
   Hide('ddlGroup');
   Hide('Tab_showFilter');
        if(obj=="1")
        {
             Hide('ecndetail');
             Hide('trshow');
             Show('trshow_generate');
             Show('display');
             Show('td_cnt1');
             Hide('td_cnt2');
             Hide('td_cnt3');
             Show('Tr_Sign');
             Show('Tr_Sign1');
             Hide('Tr_DigitalSign');
             Hide('Tr_Location');
             Hide('Tr_printtype');
             Hide('td_date');
             Hide('td_from');
             Hide('td_to');
             Show('td_fordate');
             Show('tr_selectiontype');
             Show('Td_CurrentSettlementNo');
             Hide('out_type');
             Hide('docu_type');
               Show('ptype');
                Hide('trhtml_generate')
             if('<%=Session["ExchangeSegmentID"]%>'=='1' || '<%=Session["ExchangeSegmentID"]%>'=='4' || '<%=Session["ExchangeSegmentID"]%>'=='15' || '<%=Session["ExchangeSegmentID"]%>'=='19')
           {
                
             Hide('Tr_Date');
             Hide('Tr_netforcommcdxfo');
            }
            else
                {
                    
                Show('Tr_Date');
                
                Show('Tr_netforcommcdxfo');
                
                }
             //FnSelection('1');
             document.getElementById('ddlselection').disabled=true;
//             if('<%=Session["Segmentname"]%>'=='NSE-FO')
//               {
////                var h1;
////                h1=document.getElementById('DdlPrintType');
////                var listItem1 = new Option("Contract Note Only (Exchange)", "5");

////                h1.options[4] = listItem1;
//                
////                var target=document.getElementById('DdlPrintType');    
////                var optionName = new Option('Contract Note Only (Exchange)', '5');    
////                var targetlength = target.length;    
////                target.options[targetlength] = optionName;

//                    var DropdownBox =document.getElementById("DdlPrintType");
//                    var optn = document.createElement("OPTION");
//                    optn.text="Contract Note Only (Exchange)";
//                    optn.value="5";
//                    DropdownBox.options.add(optn);
//               }

             
        }
        
        
        else if(obj=="2")
        {
           if('<%=Session["ExchangeSegmentID"]%>'=='1' || '<%=Session["ExchangeSegmentID"]%>'=='4' || '<%=Session["ExchangeSegmentID"]%>'=='15' || '<%=Session["ExchangeSegmentID"]%>'=='19')
           {
            Show('trshow');
            Hide('trshow_generate');
            
            Hide('Tr_netforcommcdxfo');
           }
           else
           {
            Hide('trshow');
            Show('trshow_generate');
            Show('Tr_netforcommcdxfo');
           }
             
              Show('trshow');
            Hide('trshow_generate');
             Hide('display');
             Hide('td_cnt1');
             Show('td_cnt2');
             Hide('td_cnt3');
             Hide('Tr_Sign');
             Show('Tr_Sign1');
             Hide('Tr_DigitalSign');
             Hide('Tr_Location');
             Hide('Tr_printtype');
             Hide('td_date');
             Hide('td_from');
             Hide('td_to');
             Show('td_fordate');
             Hide('tr_selectiontype');
              Hide('out_type');
             Hide('docu_type');
               Show('ptype');
                Hide('trhtml_generate')
             //Show('td_check');
             FnSelection('2');
             document.getElementById('ddlselection').disabled=true;
//             if('<%=Session["Segmentname"]%>'=='NSE-FO')
//               {
//                var h;
//                h=document.getElementById('DdlPrintType1');
//                var listItem = new Option("Contract Note Only (Exchange)", "4", false, false);

//                h.options[3] = listItem;
//               }
             

             
        }
        else if(obj=="3")
        {
            Hide('ecndetail');
            Show('display');
             Hide('trshow');
             Show('trshow_generate');
             Hide('td_cnt1');
             Hide('td_cnt2');
             Show('td_cnt3'); 
             Hide('Tr_Sign');
             Hide('Tr_Sign1');
             Hide('Tr_DigitalSign');
             Show('Tr_Location');
             Show('Tr_printtype');
             Show('td_date');
             Show('td_from');
             Show('td_to');
             Hide('td_fordate');
             Show('tr_selectiontype');
             
             
              Show('Td_CurrentSettlementNo');
             Hide('Tr_Date');
             Hide('Tr_ContractNo');
             Hide('Tab_showFilter');
              Hide('out_type');
             Hide('docu_type');
              Show('ptype');
              Hide('trhtml_generate')
             if('<%=Session["ExchangeSegmentID"]%>'=='1' || '<%=Session["ExchangeSegmentID"]%>'=='4' || '<%=Session["ExchangeSegmentID"]%>'=='15' || '<%=Session["ExchangeSegmentID"]%>'=='19')
           {
             Hide('Tr_Date');
            }
            else
            {
                Show('Tr_Date');
               
             Hide('Tr_netforcommcdxfo');
             }
             document.getElementById('ddlselection').disabled=false;
        }
        else if(obj=="4")
        {
             Hide('ecndetail');
             Hide('trshow');
             Hide('trshow_generate');
             Show('display');
             Show('td_cnt1');
             Hide('td_cnt2');
             Hide('td_cnt3');
             Hide('Tr_Sign');
             Hide('Tr_Sign1');
             Hide('Tr_DigitalSign');
             Hide('Tr_Location');
             Hide('Tr_printtype');
             Hide('td_date');
             Hide('td_from');
             Hide('td_to');
             Show('td_fordate');
             Show('tr_selectiontype');
             Show('Td_CurrentSettlementNo');
             Show('out_type');
             Show('docu_type');
             Hide('ptype');
             if('<%=Session["ExchangeSegmentID"]%>'=='1' || '<%=Session["ExchangeSegmentID"]%>'=='4' || '<%=Session["ExchangeSegmentID"]%>'=='15' || '<%=Session["ExchangeSegmentID"]%>'=='19')
           {
             Hide('Tr_Date');
             Show('out_type');
             Show('docu_type');
             Show('trhtml_generate')
            }
            else
            {
                Show('Tr_Date');
                 FnddlGeneration('1');
                 }
             //FnSelection('1');
             document.getElementById('ddlselection').disabled=true;
              Hide('Tr_netforcommcdxfo');
             }
             Hide('td_image');
             
            
//      
   }
    function ChkSignature()
    {
          var checkbox=document.getElementById('chkSignature')
          if(checkbox.checked)
            {
             Show('Td_Employee');
             document.getElementById('txtEmpName').focus();
            }
          else
            {
             Hide('Td_Employee');
             document.getElementById('txtEmpName_hidden').value="";
             document.getElementById('txtEmpName').value="";
            }
       }
     function isNumberKey(e)      
        {         
            var keynum
            var keychar
            var numcheck
            if(window.event)//IE
            {
                keynum = e.keyCode 
                if(keynum>=48 && keynum<=57 || keynum==46)
                   {
                      return true;
                   }
                else
                    {
                     alert("Please Insert Numeric Only");
                     return false;
                    }
             } 
         
         else if(e.which) // Netscape/Firefox/Opera
           {
               keynum = e.which  
               if(keynum>=48 && keynum<=57 || keynum==46)
                     {
                      return true;
                     }
                     else
                     {
                     alert("Please Insert Numeric Only");
                     return false;
                     }     
                }
        }
          
    function FnAlert(obj)
    {
      
        height();
    }
    function FnPrint()
    {
         Hide('Tab_showFilter');
         Hide('Td_Filter');
         document.getElementById('lblCurrentSettlementNo').innerText='<%=Session["LastSettNo"]%>';
         FnSelection('1');
         FnddlGeneration('3');
         FnddlGroupBy(document.getElementById('ddlGroupBy').value);
         alert("File Send To Printer");
         height();
    }
    </script>

    <script type="text/ecmascript">   
       function ReceiveServerData(rValue)
        {
                var j=rValue.split('~');
                
                if(j[0]=='Branch')
                    document.getElementById('HiddenField_Branch').value = j[1];
                if(j[0]=='Group')
                    document.getElementById('HiddenField_Group').value = j[1];
                if(j[0]=='Clients')
                    document.getElementById('HiddenField_Client').value = j[1];
                if(j[0]=='Contractnoteno')
                    document.getElementById('HiddenField_Contractnoteno').value = j[1];
                
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
                    <td class="EHEADER" colspan="0" style="text-align: center; height: 20px;">
                        <strong><span id="SpanHeader" style="color: #000099">Contract Notes/Annexure Printing</span></strong></td>
                    <td class="EHEADER" width="15%" id="Td_Filter" style="height: 22px">
                        <%-- <a href="javascript:void(0);" onclick="fnRecord();"><span style="color: Blue; text-decoration: underline;
                            font-size: 8pt; font-weight: bold">Go To Main Selection</span></a>--%>
                        <dxe:ASPxButton ID="btnGoToMain" runat="server" AutoPostBack="False" ClientInstanceName="cGoToMain"
                            Text="Go To Main Selection" Font-Size="7" TabIndex="0" CssClass="btnRight">
                            <ClientSideEvents Click="function(s,e){fnRecord();}" />
                            <Paddings Padding="0" PaddingBottom="0" PaddingLeft="0" PaddingRight="0" />
                        </dxe:ASPxButton>
                    </td>
                </tr>
            </table>
            <table id="tab1" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top">
                        <table>
                            <tr>
                                <td class="gridcellleft">
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Generation Type :</td>
                                            <td>
                                                <asp:DropDownList ID="DdlGeneRationType" runat="server" Width="200px" Font-Size="12px"
                                                    onchange="FnddlGeneration(this.value)">
                                                    <asp:ListItem Value="1">PDF Document</asp:ListItem>
                                                    <asp:ListItem Value="2">ECN</asp:ListItem>
                                                    <asp:ListItem Value="3">Dos Print (Pre-Printed)</asp:ListItem>
                                                    <asp:ListItem Value="4">HTML Document</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Group By</td>
                                            <td>
                                                <asp:DropDownList ID="ddlGroupBy" runat="server" Width="120px" Font-Size="12px" onchange="FnddlGroupBy(this.value)">
                                                    <asp:ListItem Value="Clients">Clients</asp:ListItem>
                                                    <asp:ListItem Value="Branch">Branch</asp:ListItem>
                                                    <asp:ListItem Value="Group">Group</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td class="gridcellleft" id="Td_OtherGroupBy">
                                                <table border="10" cellpadding="1" cellspacing="1">
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="RadioBtnOtherGroupByAll" runat="server" Checked="True" GroupName="a"
                                                                onclick="FnOtherGroupBy('a')" />
                                                            All
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="RadioBtnOtherGroupBySelected" runat="server" GroupName="a" onclick="FnOtherGroupBy('b')" />Selected
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="RadioBtnOtherGroupByallbutSelected" runat="server" GroupName="a"
                                                                onclick="FnOtherGroupBy('c')" />All But Selected
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td class="gridcellleft" id="Td_Group">
                                                <table border="10" cellpadding="1" cellspacing="1">
                                                    <tr>
                                                        <td>
                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList ID="ddlGroup" runat="server" Font-Size="12px">
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="BtnGroup" EventName="Click"></asp:AsyncPostBackTrigger>
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="RadioBtnGroupAll" runat="server" Checked="True" GroupName="b"
                                                                onclick="FnGroup('a')" />
                                                            All
                                                            <asp:RadioButton ID="RadioBtnGroupSelected" runat="server" GroupName="b" onclick="FnGroup('b')" />Selected
                                                            <asp:RadioButton ID="RadioBtnGroupallbutSelected" runat="server" GroupName="b" onclick="FnGroup('c')" />All
                                                            But Selected
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="tr_selectiontype">
                                <td class="gridcellleft">
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Selection Type :
                                            </td>
                                            <td class="gridcellleft">
                                                <asp:DropDownList ID="ddlselection" runat="server" Width="200px" Font-Size="12px"
                                                    onchange="FnSelection(this.value)">
                                                    <asp:ListItem Value="2">Date Range</asp:ListItem>
                                                    <asp:ListItem Value="3">Contract No Wise</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td id="Td_CurrentSettlementNo">
                                                <asp:Label ID="lblCurrentSettlementNo" runat="server" ForeColor="red" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="Tr_Date">
                                <td class="gridcellleft">
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC" id="td_fordate">
                                                For Date :
                                            </td>
                                            <td id="td_from" runat="server" class="gridcellleft" bgcolor="#B7CEEC">
                                                From :
                                            </td>
                                            <td>
                                                <dxe:ASPxDateEdit ID="dtFromDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True"
                                                    Font-Size="12px" Width="108px" ClientInstanceName="cdtFromDate">
                                                    <DropDownButton>
                                                    </DropDownButton>
                                                    <ClientSideEvents DateChanged="function(s,e){DateChange(cdtFromDate);}"/>

                                                </dxe:ASPxDateEdit>
                                            </td>
                                            <td id="td_to" runat="server" class="gridcellleft" bgcolor="#B7CEEC">
                                                To :
                                            </td>
                                            <td id="td_date" runat="server">
                                                <dxe:ASPxDateEdit ID="dtToDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                    Font-Size="12px" Width="108px" ClientInstanceName="cdtToDate">
                                                    <DropDownButton>
                                                    </DropDownButton>
                                                    <ClientSideEvents DateChanged="function(s,e){DateChange(cdtToDate);}"/>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="Tr_ContractNo">
                                <td class="gridcellleft">
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                ContractNote No :
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdbCntrNoSelected" runat="server" Checked="True" GroupName="bb"
                                                    onclick="FnCntrNo('S')" />
                                                Selected
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdbCntrNoRange" runat="server" onclick="FnCntrNo('R')" GroupName="bb" />
                                                Range
                                            </td>
                                            <td id="Td_Range" style="display: none;">
                                                <table>
                                                    <tr>
                                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                                            From No.
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtFromNo" runat="server" Font-Size="12px" Onkeypress="return isNumberKey(event)"
                                                                Height="12px" Width="100px"></asp:TextBox>
                                                        </td>
                                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                                            To No.
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtToNo" runat="server" Font-Size="12px" Height="12px" Onkeypress="return isNumberKey(event)"
                                                                Width="100px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr id="ptype">
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Print Type :</td>
                                            <td id="td_cnt1">
                                                <asp:DropDownList ID="DdlPrintType" runat="server" Width="220px" Font-Size="12px"
                                                    onchange="FnPrintType(this.value)">
                                                    <asp:ListItem Value="1">Contractnotes With Annexure</asp:ListItem>
                                                    <asp:ListItem Value="2">Contractnotes Only</asp:ListItem>
                                                    <asp:ListItem Value="3">Trade Annexure</asp:ListItem>
                                                    <asp:ListItem Value="4">Sttax Annexures</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td id="td_cnt2">
                                                <asp:DropDownList ID="DdlPrintType1" runat="server" Width="250px" Font-Size="12px"
                                                    onchange="FnPrintType(this.value)">
                                                    <asp:ListItem Value="1">Contractnotes With Trade and STT Annexure</asp:ListItem>
                                                    <asp:ListItem Value="2">Contractnotes With STT Annexure</asp:ListItem>
                                                    <asp:ListItem Value="3">Contractnotes Only</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td id="td_cnt3">
                                                <asp:DropDownList ID="DdlPrintType2" runat="server" Width="220px" Font-Size="12px"
                                                    onchange="FnPrintType(this.value)">
                                                    <asp:ListItem Value="2">Contract Notes only</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="out_type">
                                <td class="gridcellleft">
                                    <table border="10" cellspacing="1" cellpadding="1">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Output Type:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlOutputtype" runat="server" Width="250px" Font-Size="12px"
                                                    onchange="fnSetDocuType(this.value)">
                                                    <asp:ListItem Value="1">Download Html</asp:ListItem>
                                                    <asp:ListItem Value="2">E-Mail Attachment Html</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="docu_type">
                                <td class="gridcellleft">
                                    <table border="10" cellspacing="1" cellpadding="1">
                                        <%--<tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Document Type:
                                            </td>
                                            <td style="width: 450px;">
                                                <asp:RadioButtonList ID="dtype" runat="server" RepeatDirection="Horizontal" Width="450px">
                                                    <asp:ListItem Selected="True">Multiple Document</asp:ListItem>
                                                    <asp:ListItem Selected="True">Merged File Download</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>--%>
                                    </table>
                                </td>
                            </tr>
                            <tr id="Tr_Sign">
                                <td class="gridcellleft">
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Add Signatory:
                                                <input id="chkSignature" type="checkbox" onclick="ChkSignature()" runat="server" /></td>
                                            <td id="Td_Employee">
                                                Employee:
                                                <asp:TextBox ID="txtEmpName" runat="server" Width="200px" onkeyup="FunCallAjaxList(this,event,'Sign')"></asp:TextBox>
                                                <asp:TextBox ID="txtEmpName_hidden" runat="server" TabIndex="11" Width="100px" Style="display: none"></asp:TextBox>
                                            </td>
                                            <td>
                                                <%--<input id="chkBrokerage" type="checkbox" onclick="ChkSignature()" runat="server"
                                                    tabindex="8" checked="CHECKED" />&nbsp;Print Total Brokerage<br />--%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="Tr_Sign1">
                                <td class="gridcellleft">
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                <input id="chkBrokerage" type="checkbox" onclick="ChkSignature()" runat="server"
                                                    tabindex="8" checked="CHECKED" />&nbsp;Print Total Brokerage<br />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="Tr_netforcommcdxfo">
                                <td class="gridcellleft">
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                <input id="chknet" type="checkbox" onclick="ChkSignature()" runat="server" />&nbsp;Print
                                                Net Obligation For the Day [ Provided Billing is done for the day. ]<br />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <%--<tr id="Tr_DigitalSign">
                                <td class="gridcellleft">
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Select Signature :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtdigitalName" runat="server" Width="250px" onkeyup="FunCallAjaxList(this,event,'Digital')"></asp:TextBox>
                                                <asp:TextBox ID="txtdigitalName_hidden" runat="server" TabIndex="11" Width="100px"
                                                    Style="display: none"></asp:TextBox>
                                            </td>
                                            <td id="td_msg" runat="server">
                                                <asp:Label ID="Location" runat="server" Text="You Dont have Permission to sent ECN/ Contact to Administrator"
                                                    ForeColor="red" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>--%>
                            <tr id="Tr_printtype">
                                <td class="gridcellleft">
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Print Mode :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlfordos" runat="server" Width="220px" Font-Size="12px">
                                                    <asp:ListItem Value="B">Both (ECN & Print)</asp:ListItem>
                                                    <asp:ListItem Value="P">Only Print</asp:ListItem>
                                                    <asp:ListItem Value="E">Only ECN</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="Tr_Location">
                                <td class="gridcellleft">
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Location :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlLocation" Font-Size="12px" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trshow_generate">
                                <td class="gridcellleft">
                                    <asp:Button ID="btnshow" runat="server" CssClass="btnUpdate" Height="20px" Text="Generate"
                                        Width="101px" OnClick="btnshow_Click" />
                                </td>
                            </tr>
                            <tr id="trshow">
                                <td class="gridcellleft">
                                    <dxe:ASPxButton ID="btnshowecn" runat="server" CssClass="btnUpdate" AutoPostBack="False"
                                        Height="5px" Text="Show" Width="101px">
                                        <ClientSideEvents Click="function (s, e) {fn_show();}" />
                                    </dxe:ASPxButton>
                                </td>
                            </tr>
                            <tr id="trhtml_generate">
                                <td class="gridcellleft">
                                    <dxe:ASPxButton ID="btnHtmlgenerate" runat="server" CssClass="btnUpdate" AutoPostBack="False"
                                        Height="5px" Text="Generate" Width="101px">
                                        <ClientSideEvents Click="function (s, e) {fn_html();}" />
                                    </dxe:ASPxButton>
                                </td>
                                <td id='td_image'>
                                    <img id="img_id" alt="" title="Please Wait.." />
                                    <span style="color: #333" id="S1"></span>
                                </td>
                            </tr>
                        </table>
                        <div style="margin-left: 10px;">
                            <table class="gridcellleft" cellpadding="0" cellspacing="0" border="1" id="ecndetail"
                                style="padding: 2px;">
                                <tr style="background-color: lavender; text-align: left">
                                    <td colspan="5">
                                        <b>ECN Related Detail </b>
                                    </td>
                                </tr>
                                <tr style="background-color: #DBEEF3;">
                                    <td colspan="2">
                                        <b>Total Contract</b>
                                    </td>
                                    <td colspan="2">
                                        <b>ECN Enabled</b>
                                    </td>
                                    <td colspan="2">
                                        <b>ECN Delivered</b>
                                    </td>
                                </tr>
                                <tr style="background-color: white;">
                                    <td>
                                        <%--<b>
                                        <%= allcontract%>
                                    </b>--%>
                                        <b style="text-align: right" id="B_allcontract" runat="server"></b>
                                    </td>
                                    <td>
                                        <a href="javascript:ShowHideFilter('v1');"><span style="color: Blue; text-decoration: underline;
                                            vertical-align: bottom; font-size: 12px">View Detail</span></a>
                                    </td>
                                    <td>
                                        <%-- <b>
                                        <%=ecnenable %>
                                    </b>--%>
                                        <b style="text-align: right" id="B_ecnenable" runat="server"></b>
                                    </td>
                                    <td>
                                        <a href="javascript:ShowHideFilter('v2');"><span style="color: Blue; text-decoration: underline;
                                            vertical-align: bottom; font-size: 12px">View Detail</span></a>
                                    </td>
                                    <td>
                                        <%--<b>
                                        <%=deliveryrpt %>
                                    </b>--%>
                                        <b style="text-align: right" id="B_deliveryrpt" runat="server"></b>
                                    </td>
                                    <td>
                                        <a href="javascript:ShowHideFilter('v3');"><span style="color: Blue; text-decoration: underline;
                                            vertical-align: bottom; font-size: 12px">View Detail</span></a>
                                    </td>
                                </tr>
                                <%--<tr id="tr_openpopup">
                                <td class="gridcellleft" colspan="5">
                                    <dxe:ASPxButton ID="btnopenpopup" runat="server" CssClass="btnUpdate" AutoPostBack="False"
                                        Height="5px" Text="Generate" Width="101px">
                                        <clientsideevents click="function (s, e) {fn_showpopup();}" />
                                    </dxe:ASPxButton>
                                </td>
                            </tr>--%>
                            </table>
                        </div>
                        <table>
                            <tr id="Tr_DigitalSign">
                                <td class="gridcellleft">
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Select Signature :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtdigitalName" runat="server" Width="250px"></asp:TextBox>
                                                <asp:TextBox ID="txtdigitalName_hidden" runat="server" TabIndex="11" Width="100px"
                                                    Style="display: none"></asp:TextBox>
                                            </td>
                                            <td id="td_msg" runat="server">
                                                <asp:Label ID="Location" runat="server" Text="You Dont have Permission to sent ECN/ Contact to Administrator"
                                                    ForeColor="red" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="tr_openpopup">
                                            <td class="gridcellleft" style="text-align: left;">
                                                <dxe:ASPxButton ID="btnopenpopup" runat="server" CssClass="btnUpdate" AutoPostBack="False"
                                                    Height="5px" Text="Generate" Width="101px">
                                                    <ClientSideEvents Click="function (s, e) {fn_showpopup();}" />
                                                </dxe:ASPxButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top">
                        <table id="Tab_showFilter">
                            <tr>
                                <td valign="top">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="300px" onkeyup="FunCallAjaxList(this,event,'Other')"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                    Enabled="false">
                                                    <asp:ListItem>Clients</asp:ListItem>
                                                    <asp:ListItem>Group</asp:ListItem>
                                                    <asp:ListItem>Branch</asp:ListItem>
                                                    <asp:ListItem>Contractnoteno</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <a id="A3" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                                    style="color: #2554C7; text-decoration: underline; font-size: 8pt;"><b>Add to List</b></span></a><span
                                                        style="color: #009900; font-size: 8pt;"> </span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ListBox ID="lstSlection" runat="server" Font-Size="12px" Height="100px" Width="400px">
                                    </asp:ListBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <a id="A5" href="javascript:void(0);" onclick="clientselectionfinal()"><span style="color: #000099;
                                                    text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
                                            </td>
                                            <td>
                                                <a id="A6" href="javascript:void(0);" onclick="btnRemovefromsubscriptionlist_click()">
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
            <%--<table>
                <tr>
                    <td>--%>
                <dxe:ASPxPopupControl ID="PopUp_ScripAlert" runat="server" ClientInstanceName="cPopUp_ScripAlert"
                Width="340px" HeaderText="ECN Detail Information" PopupHorizontalAlign="Center"
                PopupVerticalAlign="Middle" CloseAction="None" Modal="True" ShowCloseButton="False">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <%--<div style="font-weight: bold; color: black; background-color: gainsboro; border-right: silver thin solid;
                            border-top: silver thin solid; border-left: silver thin solid; border-bottom: silver thin solid;">
                            No of ECN Sent : <b style="text-align:right" id="B_allcontractpop" runat="server"></b>
                            <br />
                            <br />
                            Remaining ECN (To Be Send) : <b style="text-align:right" id="B_ecnenablepop" runat="server"></b>
                        </div>
                        <br />
                        <br />--%>
                        <dxe:ASPxCallbackPanel ID="CbpSuggestISIN" runat="server" ClientInstanceName="cCbpSuggestISIN"
                            BackColor="White" OnCallback="CbpSuggestISIN_Callback" LoadingPanelText="Sending....Please Wait !!"
                            LoadingPanelStyle-Font-Bold="true" LoadingPanelStyle-Cursor="wait" LoadingPanelStyle-ForeColor="gray"
                            LoadingPanelImage-Url='../images/Animated_Email.gif'>
                            <ClientSideEvents EndCallback="CbpSuggestISIN_EndCallBack" />
                            <PanelCollection>
                                <dxe:panelcontent runat="server">
                                     <div id="div_fail" style="color: Red; font-weight: bold; font-size: 12px;">
                                        An Internal Error Occured during generating file for ECN.
                                    </div>
                                    <div id="div_success" style="color: Green; font-weight: bold; font-size: 12px;">
                                       File generated successfully for ECN .
                                    </div>

                                    <%--<asp:Label runat="server" ID="div_success" Text="efgh"></asp:Label>--%>
                                    <br />
                                    <br />
                                  
                                   <div style="font-weight: bold; color: black; background-color: gainsboro; border-right: silver thin solid;
                                        border-top: silver thin solid; border-left: silver thin solid; border-bottom: silver thin solid;">
                                        No of ECN Generated : <b style="text-align: right" id="B_allcontractpop" runat="server"></b>
                                        <br />
                                        <%--<asp:Image src='../Documents/Animated_Email.gif' runat="server" />--%>
                                        <br />
                                        Remaining ECN (To Be Generated) : <b style="text-align: right" id="B_ecnenablepop" runat="server">
                                        </b>
                                    </div>

                                    <br />
                                    <br />

                                    <div class="frmleftCont" id="btnall">
                                        <%--<dxe:ASPxButton ID="btnsendall" runat="server" AutoPostBack="False" Text="Send All">--%>
                                        <dxe:ASPxButton ID="btnsendall" runat="server" AutoPostBack="False" Text="Generate All">
                                            <ClientSideEvents Click="function (s, e) {btnsendall_Click();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                    <div class="frmleftCont" id="btnremain">
                                        <dxe:ASPxButton ID="btnsendremaining" runat="server" AutoPostBack="False" Text="Send Remaining">
                                            <ClientSideEvents Click="function (s, e) {btnsendremaining_Click();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                    <div class="frmleftCont" id="btnokdiv">
                                        <dxe:ASPxButton ID="btnok" runat="server" AutoPostBack="False" Text="Close">
                                            <ClientSideEvents Click="function (s, e) {btnok_Click();}" />
                                        </dxe:ASPxButton>
                                    </div>

                                    <div class="frmleftCont" id="cancel">
                                        <dxe:ASPxButton ID="btncancel" runat="server" AutoPostBack="False" Text="Cancel">
                                            <ClientSideEvents Click="function (s, e) {btncancel_Click();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                </dxe:panelcontent>
                            </PanelCollection>
                        </dxe:ASPxCallbackPanel>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="LightGray" ForeColor="Black" />
            </dxe:ASPxPopupControl>

            
            <%--</td>
                </tr>
            </table>--%>
            <table>
                <tr>
                    <td style="display: none;">
                        <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                        <asp:Button ID="BtnGroup" runat="server" Text="BtnGroup" OnClick="BtnGroup_Click" />
                        <asp:HiddenField ID="HiddenField_Group" runat="server" />
                        <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                        <asp:HiddenField ID="HiddenField_Contractnoteno" runat="server" />
                        <asp:HiddenField ID="HiddenField_Client" runat="server" />
                        <asp:HiddenField ID="HiddenField_SegmentName" runat="server" />
                        <asp:HiddenField ID="Hddndate" runat="server" />
                        <asp:Button ID="BtnForExportEvent" runat="server" OnClick="cmbExport_SelectedIndexChanged"
                            BackColor="#DDECFE" BorderStyle="None" />
                        <asp:Button ID="BtnForExportEvent1" runat="server" OnClick="cmbExport1_SelectedIndexChanged"
                            BackColor="#DDECFE" BorderStyle="None" />
                        <asp:Button ID="BtnForExportEvent2" runat="server" OnClick="cmbExport2_SelectedIndexChanged"
                            BackColor="#DDECFE" BorderStyle="None" />
                        <asp:Button ID="BtnForExportEvent3" runat="server" OnClick="cmbExport3_SelectedIndexChanged"
                            BackColor="#DDECFE" BorderStyle="None" />
                    </td>
                    <td>
                        <span id="spanall">
                            <dxe:ASPxCallbackPanel ID="Cexcelexportpanel" runat="server" ClientInstanceName="cCbptxtSelectionID"
                                OnCallback="Cexcelexportpanel_Callback" Width="206px">
                                <PanelCollection>
                                    <dxe:panelcontent runat="server">
                                    </dxe:panelcontent>
                                </PanelCollection>
                                <ClientSideEvents EndCallback="CbptxtSelectionID_EndCallBack" />
                            </dxe:ASPxCallbackPanel>
                        </span>
                    </td>
                    <td>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <div id='Div1' style='position: absolute; font-family: arial; font-size: 30; left: 50%;
                                    top: 50%; background-color: white; layer-background-color: white; height: 80;
                                    width: 150;'>
                                    <table width='100' height='35' border='1' cellpadding='0' cellspacing='0' bordercolor='#c0d6e4'>
                                        <tr>
                                            <td>
                                                <table style="width: 245px">
                                                    <tr>
                                                        <td height='25' align='center' bgcolor='#ffffff'>
                                                            <img src='/assests/images/progress.gif' width='18' height='18'></td>
                                                        <td height='10' width='100%' align='center' bgcolor='#ffffff'>
                                                            <font size='1' face='Tahoma'><strong align='center'>Generating Output Files.Please Wait..</strong></font></td>
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
        </div>
        <table>
            <tr>
                <td>
                    <div id="display" runat="server">
                    </div>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnpath" runat="server" />
        <asp:HiddenField ID="hdnLocationPath" runat="server" />
        <dxe:ASPxPopupControl ID="Popup_ContractNoteDetails" runat="server" ClientInstanceName="cPopup_ContractNoteDetails"
            HeaderText="ContractNote Details" ShowCloseButton="false" Modal="true" CloseAction="None"
            PopupHorizontalAlign="leftsides" PopupVerticalAlign="topsides" ShowOnPageLoad="true"
            Width="400px">
            <ContentCollection>
                <dxe:PopupControlContentControl ID="popupcontentctrl" runat="server">
                    <dxe:ASPxCallbackPanel ID="cbpPopupContractnotePane" runat="server" ClientInstanceName="ccbpPopupContractnotePane"
                        BackColor="White" OnCallback="CbpSuggestISIN_Callback">
                        <PanelCollection>
                            <dxe:panelcontent ID="panelcontentbegin" runat="server">
                                <br />
                                <br />
                                <div style="font-weight: bold; color: black; background-color: gainsboro; border-right: silver thin solid;
                                    border-top: silver thin solid; border-left: silver thin solid; border-bottom: silver thin solid;">
                                    ContractNote Cum Bill for Combined Exchange-Segment : <b style="text-align: right"
                                        id="B2" runat="server">
                                        <dxe:ASPxRadioButton ID="rdbCombined" runat="server" Checked="true" ClientInstanceName="crdbCombined"
                                            GroupName="o" Layout="Flow">
                                            <ClientSideEvents CheckedChanged="function(s,e){FnShowNew()}"></ClientSideEvents>
                                        </dxe:ASPxRadioButton>
                                    </b>
                                    <br />
                                    <%--<asp:Image src='../Documents/Animated_Email.gif' runat="server" />--%>
                                    <br />
                                    Single Exchange-Segment (Old Format):  <b style="text-align: right" id="B1"
                                        runat="server"></b>
                                    <dxe:ASPxRadioButton ID="rdbSingle" runat="server" Checked="false" ClientInstanceName="crdbSingle"
                                        GroupName="o" Layout="Flow">
                                        <ClientSideEvents CheckedChanged="function(s,e){FnShowNew()}"></ClientSideEvents>
                                    </dxe:ASPxRadioButton>
                                   
                                    
                                </div>
                                <br />
                                <br />
                                <div class="frmleftCont" id="Div2">
                                    <dxe:ASPxButton ID="btnSubmit" runat="server" AutoPostBack="False" Text="Submit">
                                        <ClientSideEvents Click="function (s, e) {btnSubmit_Click();}" />
                                    </dxe:ASPxButton>
                                </div>
                            </dxe:panelcontent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
</asp:Content>
