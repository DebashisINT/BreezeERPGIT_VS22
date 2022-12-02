<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="ERP.OMS.Reports.Reports_MultiCurrencyPortfolioAnalysis" EnableEventValidation="false" Codebehind="MultiCurrencyPortfolioAnalysis.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxCallbackPanel"
    TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Across Exchange Multi-Currency Portfolio Performance & Hedge Analysis</title>

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" src="/assests/js/ajaxList_rootfile.js"></script>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/GenericJScript.js"></script>

    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />
    <link type="text/css" href="../CSS/AjaxStyle.css" rel="Stylesheet" />
    <!--Page Common Script-->

    <script type="text/javascript" language="javascript">
    function SignOff()
    {
      window.parent.SignOff();
    }
    function PageLoad()///Call Into Page Load
    {         
       Hide('showFilter');
       Hide('divSendMail');
       cdtFrom.Focus();      
       height();         
    }
    function Hide(obj)
    {
      document.getElementById(obj).style.display='none';
    }
    function Show(obj)
    {
       document.getElementById(obj).style.display='inline';
    }
    function height()
    {        
       if(document.body.scrollHeight>=300)
        window.frameElement.height = document.body.scrollHeight;
       else
        window.frameElement.height = '350px';
        window.frameElement.Width = document.body.scrollWidth;
    }
    function ChangeDateFormat_CalToDB(obj)
    {
        var SelectedDate = new Date(obj);            
        var monthnumber = SelectedDate.getMonth() + 1;
        var monthday    = SelectedDate.getDate();
        var year        = SelectedDate.getYear();            
        var changedDateValue=year+'-'+monthnumber+'-'+monthday;
        return changedDateValue;
    }
    function ChangeDateFormat_SetCalenderValue(obj)
    {       
        var SelectedDate = new Date(obj);
        var monthnumber = SelectedDate.getMonth();
        var monthday    = SelectedDate.getDate();
        var year        = SelectedDate.getYear();            
        var changeDateValue=new Date(year, monthnumber, monthday);
        return changeDateValue;            
    }   
    function replaceChars(entry)
    {
        out = "+"; // replace this
        add = "--"; // with this
        temp = "" + entry; // temporary holder
        while (temp.indexOf(out)>-1) 
        {
            pos= temp.indexOf(out);
            temp = "" + (temp.substring(0, pos) + add + 
            temp.substring((pos + out.length), temp.length));
        }
        return temp;
    }
    function DateChange(positionDate)
    {
        var FYS='<%=Session["FinYearStart"]%>';
        var FYE='<%=Session["FinYearEnd"]%>';
        var LFY='<%=Session["LastFinYear"]%>';
        DevE_CheckForFinYear(positionDate,FYS,FYE,LFY);
    }     
    </script>

    <!--CallAjax,CallServer Script-->

    <script type="text/javascript" language="javascript">       
       FieldName="none";
       AjaxComQuery='';
       function CallGenericAjaxJS(e)
       {
            var AjaxList_TextBox=document.getElementById('<%=txtSelectionID.ClientID%>');
            AjaxList_TextBox.focus();
            AjaxComQuery=AjaxComQuery.replace("\'","'");
            ajax_showOptions(AjaxList_TextBox,'GenericAjaxList',e,replaceChars(AjaxComQuery),'Main');
       }       
       function ReceiveServerData(rValue)
       {            
            var Data=rValue.split('!');
            if(Data[1]!="undefined")
            {
                if(Data[0]=='Branch')
                {
                    document.getElementById('HiddenField_Branch').value = Data[1];                    
                }
                if(Data[0]=='Group')
                { 
                    document.getElementById('HiddenField_Group').value = Data[1];
                }
                if(Data[0]=='Client')
                {
                    document.getElementById('HiddenField_Client').value = Data[1];
                }
                if(Data[0]=='Asset')
                { 
                    document.getElementById('HiddenField_Asset').value = Data[1];
                }
                if(Data[0]=='UserEmail')
                { 
                    document.getElementById('HiddenField_UserEmail').value = Data[1];
                }
            } 
            if(Data[0]=='AjaxQuery')
            {                
                AjaxComQuery = Data[1];
                var AjaxList_TextBox=document.getElementById('<%=txtSelectionID.ClientID%>');                          
                AjaxList_TextBox.value='';
                AjaxList_TextBox.attachEvent('onkeyup',CallGenericAjaxJS);
             }              
        }
        function btnAddsubscriptionlist_click()
        {            
            var txtName = document.getElementById('txtSelectionID');
            if(txtName != '')
            {
                var txtId = document.getElementById('txtSelectionID_hidden').value;
                var listBox = document.getElementById('<%=lstSelection.ClientID%>');
                var listLength = listBox.length;               
                var opt = new Option();
                opt.value = txtId;
                opt.text = txtName.value;
                listBox[listLength]=opt;
                txtName.value='';
            }
            else
                alert('Please search name and then Add!');
            txtName.focus();
            txtName.select();            
        }
        function lnkBtnAddFinalSelection(obj)
        {
            var listBox = document.getElementById('<%=lstSelection.ClientID%>');	          
            var listID='';
            var i;
            if(listBox.length > 0)
            {                             
                for(i=0;i<listBox.length;i++)
                {
                    if(listID == '')
                        listID = listBox.options[i].value+'|'+listBox.options[i].text;
                    else
                        listID += '^' + listBox.options[i].value+'|'+listBox.options[i].text;
                }                
                CallServer(listID,"");  
                var j;
                for(j=listBox.options.length-1;j>=0;j--)
                {
                    listBox.remove(j);
                } 
                Hide('showFilter');
                document.getElementById('divExport').style.display='inline';                    
                if(document.getElementById('divSendMail').style.display=='inline')
                        cbtnResult.SetEnabled(true);                
            }
        }	        
        function lnkBtnRemoveFromSelection()
        {                
            var listBox = document.getElementById('<%=lstSelection.ClientID%>');
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
    </script>

    <!--Control Reset and Validation Script-->

    <script type="text/javascript">
      function Reset()
      {
           cdtFrom.SetDate(new Date());
           cdtTo.SetDate(new Date()); 
           document.getElementById('<%=RdoAssetAll.ClientID%>').checked=true;      
           document.getElementById('HiddenField_Asset').value='';           
           cCmbReportType.SetSelectedIndex(0);
           SetControl_Initialize("ReportType");
      }
      function SetControl_Initialize(obj)
      {
            Hide('divSendMail');          
            Show('divExport');
            cbtnResult.SetEnabled(false); 
            if(obj=="ReportType")
            {
                 cCmbReportStyle.SetEnabled(false);
                 cCmbReportStyle.SetSelectedIndex(0);
                 cCmbReport.SetEnabled(false);
                 cCmbReport.SetSelectedIndex(0);                
                 cCmbReportBy.SetEnabled(false);
                 cCmbReportBy.SetSelectedIndex(0);
                 if(document.getElementById('divGroupBy').style.display=="none")
                        Show('divGroupBy');
                 Hide('divGroupByOpt');       
                 cCmbGroupBy.SetEnabled(false);
                 cCmbGroupBy.SetSelectedIndex(0);
                 document.getElementById('HiddenField_Branch').value=='';
                 document.getElementById('HiddenField_Group').value=='';
                 document.getElementById('HiddenField_Client').value=='';
                 document.getElementById('HiddenField_UserEmail').value=='';                                  
                 height();
            }
            else if(obj=="ReportStyle")
            {
                 cCmbReport.SetEnabled(false);
                 cCmbReport.SetSelectedIndex(0);                  
                 cCmbReportBy.SetEnabled(false);
                 cCmbReportBy.SetSelectedIndex(0);                    
                 if(document.getElementById('divGroupBy').style.display=="none")
                        Show('divGroupBy');
                 Hide('divGroupByOpt');       
                 cCmbGroupBy.SetEnabled(false);
                 cCmbGroupBy.SetSelectedIndex(0);                
                 document.getElementById('HiddenField_Branch').value=='';
                 document.getElementById('HiddenField_Group').value=='';
                 document.getElementById('HiddenField_Client').value=='';
                 document.getElementById('HiddenField_UserEmail').value=='';                                 
                 height(); 
            }
            else if(obj=="Report")
            {                 
                 cCmbReportBy.SetEnabled(false);
                 cCmbReportBy.SetSelectedIndex(0);                    
                 if(document.getElementById('divGroupBy').style.display=="none")
                        Show('divGroupBy');
                 Hide('divGroupByOpt');       
                 cCmbGroupBy.SetEnabled(false);
                 cCmbGroupBy.SetSelectedIndex(0);                
                 document.getElementById('HiddenField_Branch').value=='';
                 document.getElementById('HiddenField_Group').value=='';
                 document.getElementById('HiddenField_Client').value=='';
                 document.getElementById('HiddenField_UserEmail').value=='';                                  
                 height();
            }
             else if(obj=="ReportBy")
            {                 
                 if(document.getElementById('divGroupBy').style.display=="none")
                        Show('divGroupBy');
                 Hide('divGroupByOpt');
                 cCmbGroupBy.SetEnabled(false);
                 cCmbGroupBy.SetSelectedIndex(0);                
                 document.getElementById('HiddenField_Branch').value=='';
                 document.getElementById('HiddenField_Group').value=='';
                 document.getElementById('HiddenField_Client').value=='';
                 document.getElementById('HiddenField_UserEmail').value=='';                                  
                 height();
            }
            else if(obj=="GroupBy")
            {                 
                 Hide('divGroupType');   
                 Hide('divGroupByOpt');
                 document.getElementById('HiddenField_Branch').value=='';
                 document.getElementById('HiddenField_Group').value=='';
                 document.getElementById('HiddenField_Client').value=='';
                 document.getElementById('HiddenField_UserEmail').value=='';                                  
                 height();
            }
            else if(obj=="GroupType")
            {                 
                 document.getElementById('HiddenField_Group').value=='';
                 height();
            }
            else if(obj=="Respective")
            {
                 Show('divSendMail');
                 Hide('showFilter');
                 document.getElementById('HiddenField_UserEmail').value=='';
                 height();           
            }
      }
      function Validate(obj)
      {
          if(obj=="1")
          {
              alert("Please Select From Date.");
              cdtFrom.Focus();                               
          }
          if(obj=="2") 
          {
              alert("Please Select To Date.");
              cdtTo.Focus();        
          }            
          if(obj=="3") 
          {
              alert("Please Select A Report Type.");
              cCmbReportType.SetFocus();                                
          }
          if(obj=="4") 
          {
              alert("Please Select A Report Style.");
              cCmbReportStyle.SetFocus();                             
          }
          if(obj=="5") 
          {
              alert("Please Select Report.");
              cCmbReport.SetFocus();                                
          }
          if(obj=="6") 
          {
              alert("Please Select Report Output.");
              cCmbReportBy.SetFocus();                                
          }
          if(obj=="7") 
          {
              alert("Please Select Group By.");
              cCmbGroupBy.SetFocus();                                
          }
          if(obj=="8") 
          {
              alert("Please Select Group Type.");
              cCmbGroupType.SetFocus();                                
          }  
          if(obj=="9") 
          {
              alert("Please Select Mail Recipient.");
              cCmbMailSendTo.SetFocus();                                
          }              
      }      
      //SetToAllobj RadioButtons
      //SettoZeroIndex Combo
      function SelectionChecking(ValidateObj,SetToAllobj)
      {
            var hdnAsset='';
            var hdnBranch='';
            var hdnGroup='';
            var hdnClient='';            
            var MailToUser="0";
            var hdnMailToUser='';            
            
            if(document.getElementById('HiddenField_Asset').value!=undefined)
                        hdnAsset=document.getElementById('HiddenField_Asset').value;              
            if(document.getElementById('HiddenField_Branch').value!=undefined)
                        hdnBranch=document.getElementById('HiddenField_Branch').value;              
            if(document.getElementById('HiddenField_Group').value!=undefined)
                        hdnGroup=document.getElementById('HiddenField_Group').value;              
            if(document.getElementById('HiddenField_Client').value!=undefined)
                        hdnClient=document.getElementById('HiddenField_Client').value;              
            
            if(document.getElementById('divSendMail').style.display=="inline")
            {
                MailToUser=cCmbMailSendTo.GetValue();
                if(document.getElementById('HiddenField_UserEmail').value!=undefined)
                        hdnMailToUser=document.getElementById('HiddenField_UserEmail').value;              
                if((MailToUser=="1")||(MailToUser=="2")||(MailToUser=="3"))
                {
                    Hide('showFilter');  
                    document.getElementById('divExport').style.display='inline';
                    cbtnResult.SetEnabled(true);                                                  
                }        
            }            
            if (ValidateObj=="RdoAssetSelected")
            { 
                if(((document.getElementById("RdoGroupBySelected").checked==true)&&(hdnBranch==''||hdnGroup==''||hdnClient=='')) || (MailToUser=="4" && hdnMailToUser==''))
                {
                    if(document.getElementById("RdoGroupBySelected").checked==true)
                    {
                        if(cCmbGroupBy.GetValue()=="B" && hdnBranch=='')
                        {
                            alert("Please Select Atleast One Branch Item !!!");
                            document.getElementById(SetToAllobj).checked=true;
                            return false;
                        }
                        else if(cCmbGroupBy.GetValue()=="G" && hdnGroup=='')
                        {
                            alert("Please Select Atleast One Group Item !!!");
                            document.getElementById(SetToAllobj).checked=true;
                            return false;
                        }
                        else if(cCmbGroupBy.GetValue()=="C" && hdnClient=='')
                        {
                            alert("Please Select Atleast One Client Item !!!");
                            document.getElementById(SetToAllobj).checked=true;
                            return false;
                        }                        
                    }
                    else if(MailToUser=="4" && hdnMailToUser=='')
                    {
                         alert("Please Select Atleast One User Item !!!");
                         document.getElementById(SetToAllobj).checked=true;
                         return false;
                    }
                }
                Show('showFilter');                       
                document.getElementById('divExport').style.display='none';
                CallServer("CallAjax-Asset");
                document.getElementById('<%=txtSelectionID.ClientID%>').focus();                
            }
            else if (ValidateObj=="RdoGroupBySelected")
            {                 
                 if(((document.getElementById("RdoAssetSelected").checked==true) && (hdnAsset=='')) || (MailToUser=="4" && hdnMailToUser==''))
                   {
                        if((document.getElementById("RdoAssetSelected").checked==true) && (hdnAsset==''))
                        {
                             alert("Please Select Atleast One Asset Item !!!");
                             document.getElementById(SetToAllobj).checked=true;
                             return false;
                        }
                        else if(MailToUser=="4" && hdnMailToUser=='')
                        {
                             alert("Please Select Atleast One User Item !!!");
                             document.getElementById(SetToAllobj).checked=true;
                             return false;
                        }        
                   }
                   Show('showFilter');
                   if(cCmbGroupBy.GetValue()=="B")
                   {
                       document.getElementById('divExport').style.display='none';
                       CallServer("CallAjax-Branch");
                       document.getElementById('<%=txtSelectionID.ClientID%>').focus();
                   }
                   else if(cCmbGroupBy.GetValue()=="G")
                   {
                       document.getElementById('divExport').style.display='none';
                       CallServer("CallAjax-Group~"+cCmbGroupType.GetText(),"");
                       document.getElementById('<%=txtSelectionID.ClientID%>').focus();
                   }
                   else if(cCmbGroupBy.GetValue()=="C")
                   {
                       document.getElementById('divExport').style.display='none';
                       CallServer("CallAjax-Client");        
                       document.getElementById('<%=txtSelectionID.ClientID%>').focus();
                   }
            }
            else if((ValidateObj=="CmbMailSendTo") && (SetToAllobj=="4"))
            {
               if(((document.getElementById("RdoAssetSelected").checked==true) && (hdnAsset=='')) ||((document.getElementById("RdoGroupBySelected").checked==true)&&(hdnBranch==''||hdnGroup==''||hdnClient=='')))
                {
                    if((document.getElementById("RdoAssetSelected").checked==true) && (hdnAsset==''))
                    {
                         alert("Please Select Atleast One Asset Item !!!");
                         cCmbMailSendTo.SetSelectedIndex(SetToAllobj);//Test Zero
                         return false;
                    }
                    else if(document.getElementById("RdoGroupBySelected").checked==true)
                    {
                        if(cCmbGroupBy.GetValue()=="B" && hdnBranch=='')
                        {
                            alert("Please Select Atleast One Branch Item !!!");
                            cCmbMailSendTo.SetSelectedIndex(SetToAllobj);//Test Zero
                            return false;
                        }
                        else if(cCmbGroupBy.GetValue()=="G" && hdnGroup=='')
                        {
                            alert("Please Select Atleast One Group Item !!!");
                            cCmbMailSendTo.SetSelectedIndex(SetToAllobj);//Test Zero
                            return false;
                        }
                        else if(cCmbGroupBy.GetValue()=="C" && hdnClient=='')
                        {
                            alert("Please Select Atleast One Client Item !!!");
                           cCmbMailSendTo.SetSelectedIndex(SetToAllobj);//Test Zero
                            return false;
                        }                        
                    }                                                     
                }               
                Show('showFilter');          
                document.getElementById('divExport').style.display='none';
                CallServer("CallAjax-UserEmail");
                document.getElementById('<%=txtSelectionID.ClientID%>').focus();                                   
            }        
      }
    </script>

    <!--Control Populate and Selection Script-->

    <script type="text/javascript" language="javascript">
       function fn_RdoAssetAll()
       {
           Hide('showFilter');
           document.getElementById('divExport').style.display='inline';                                          
       }
       function fn_CmbReportType(obj)
       {
            if(obj=="0")
                SetControl_Initialize("ReportType");
            else
                cCmbReportStyle.PerformCallback("ReportStyle~"+obj);
       }
       function CmbReportStyle_EndCallback()
       {
           cCmbReportStyle.SetEnabled(true);
           SetControl_Initialize("ReportStyle");            
       }
       function fn_CmbReportStyle(obj)
       {
            if(obj=="0")
                SetControl_Initialize("ReportStyle");
            else
                cCmbReport.PerformCallback("Report~"+cCmbReportType.GetValue()+"~"+obj);
       }
       function CmbReport_EndCallback()
       {
             cCmbReport.SetEnabled(true);
             SetControl_Initialize("Report"); 
       }
       function fn_CmbReport(obj)
       {
            if(obj=="0")            
                SetControl_Initialize("Report");
             else
                cCmbReportBy.PerformCallback("ReportBy~"+obj);   
       }
       function CmbReportBy_EndCallback()
       {
            cCmbReportBy.SetEnabled(true);
            SetControl_Initialize("ReportBy");           
       }
       function fn_CmbReportBy(obj)
       {
           if(obj=="0")
           {
                SetControl_Initialize("ReportBy");
           }
           else
           {
                if(obj=="1")
                {
                    cbtnResult.SetText("Export"); 
                    cbtnResult.SetEnabled(false);                    
                }
                else if(obj=="2")
                {
                    cbtnResult.SetText("Send Mail"); 
                }
                cCmbGroupBy.PerformCallback("GroupBy~"+cCmbReport.GetValue()+"~"+cCmbReportBy.GetValue());                              
            }
       }
       function CmbGroupBy_EndCallback()
       {
            SetControl_Initialize("GroupBy");
            if(document.getElementById('divGroupType').style.display=="inline")
                   Hide('divGroupType'); 
            if(document.getElementById('divGroupByOpt').style.display=="inline")
                   Hide('divGroupByOpt');  
            if(document.getElementById('showFilter').style.display=="inline")
                   Hide('showFilter');                                                 
            if(cCmbGroupBy.cpShowGroupBy!=undefined)
            {
                if(cCmbGroupBy.cpShowGroupBy=="Y")
                    cCmbGroupBy.SetEnabled(true);
                else
                    Hide('divGroupBy');              
            }    
            if(cCmbGroupBy.cpBindUserOnly!=undefined)
            {
               if(cCmbGroupBy.cpBindUserOnly=="Y")
               {
                    cCmbMailSendTo.PerformCallback("MailToOnlyUser~0");
               }
               else if(cCmbGroupBy.cpBindUserOnly=="N")
               {
                    if(document.getElementById('divSendMail').style.display=='inline')
                            Hide('divSendMail');
               }                                         
            }
            if(cCmbGroupBy.cpEnablebtnResult!=undefined)
            {
               if(cCmbGroupBy.cpEnablebtnResult=="Y")
                    cbtnResult.SetEnabled(true);
                else if(cCmbGroupBy.cpEnablebtnResult=="N")
                    cbtnResult.SetEnabled(false);                    
            }
       } 
       function  fn_GroupBy(obj)
       {
           if(obj=="0")
           {
                SetControl_Initialize("GroupBy");
           }
           else
           { 
               document.getElementById('<%=lstSelection.ClientID%>').length=0;
                Hide('divGroupType'); 
                if(obj=="B")
                {
                      document.getElementById('RdoGroupByAll').checked=true;
                      Show('divGroupByOpt');               
                      CallServer("CallAjax-Branch",""); 
                      if(cCmbReportBy.GetValue()=="1")
                          cbtnResult.SetEnabled(true);           
                      else if(cCmbReportBy.GetValue()=="2")
                          cCmbMailSendTo.PerformCallback("MailToBranch~"+cCmbReport.GetValue());                   
                }           
                if(obj=="G")                
                {  
                    Show('divGroupType');
                    Hide('divGroupByOpt');                                      
                    cbtnResult.SetEnabled(false);              
                    if(cCmbReportBy.GetValue()=="2")
                        cCmbGroupType.PerformCallback("GroupTypeWithSendMail~");
                    else
                        cCmbGroupType.PerformCallback("GroupType~");
                }            
                if(obj=="C")
                { 
                    document.getElementById('RdoGroupByAll').checked=true;
                    Hide('divGroupType');
                    Show('divGroupByOpt');               
                    CallServer("CallAjax-Client","");
                    if(cCmbReportBy.GetValue()=="1")
                          cbtnResult.SetEnabled(true);           
                    else if(cCmbReportBy.GetValue()=="2")
                          cCmbMailSendTo.PerformCallback("MailToClient~"+cCmbReport.GetValue());              
                }                
                height();
           }    
       }
       
       function fn_RdoGroupByAll()
       {
           Hide('showFilter');
           document.getElementById('divExport').style.display='inline';                                          
       }       
       function CmbGroupType_EndCallback()
       {
          if(cCmbGroupType.cpBindGroupType!=undefined)
          {
              if(cCmbGroupType.cpBindGroupType=="Y")
              {
                    cCmbGroupType.SetEnabled(true);
                    SetControl_Initialize("GroupType");                            
              }
              else if(cCmbGroupType.cpBindGroupType=="N")
              {
                    cCmbGroupType.SetEnabled(false);                   
              }
          }
          if(cCmbGroupType.cpSendMailTo!= undefined)
          {
              if(cCmbGroupType.cpSendMailTo=="Y")
                    cCmbMailSendTo.PerformCallback("MailToGroup~"+cCmbReport.GetValue()); 
          }
          height();
       }
       function fn_CmbGroupType(obj)
       {
            if(obj=="0")
            {
                SetControl_Initialize("GroupType");            
                Hide('divGroupByOpt'); 
                cbtnResult.SetEnabled(false);   
            }
            else
            {
                 document.getElementById('RdoGroupByAll').checked=true;                    
                 Show('divGroupByOpt');
                 cbtnResult.SetEnabled(true);                                   
            }               
       }
       function CmbMailSendTo_EndCallback()
       { 
            Show('divSendMail');   
            cCmbMailSendTo.SetEnabled(true);             
            height();
       }
       function fn_CmbMailSendTo(obj)
       {
            if(obj=="0")
            {
                SetControl_Initialize("Respective");
                cbtnResult.SetEnabled(false);
            }
            else
            {
                SelectionChecking('CmbMailSendTo',obj);     
            }            
       }      
    </script>

</head>
<body style="margin: 0px 0px 0px 0px; background-color: #DDECFE;" onload="clearPreloadPage();">
    <form id="form1" runat="server" autocomplete="off">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <div class="TableMain100">
                <div class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">Across Exchange Multi-Currency Portfolio Performance
                        & Hedge Analysis</span></strong>
                </div>
                <div id="divReset" style="text-align: right; margin-right: 50px; margin-top:-18px;">
                   <a href="javascript:void(0);" style="height:19px; border:.1pt solid #555; background:#ddd; padding:2px 15px; text-decoration:none; color:#222;" onclick="Reset()" >Reset</a>                       
                </div> 
            </div>
            <br class="clear" />
            <div class="pageContent">
                <div id="divPageFilter" style="width: 995px;">
                    <div id="showFilter" class="right" style="width: 452px; background-color: #B7CEEC;
                        border: solid 2px  #ccc; display: none;">
                        <div style="width: 100%">
                            <div class="frmleftContent">
                                <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Height="20px" Width="350px"
                                    TabIndex="0"></asp:TextBox>
                            </div>
                            <div class="frmleftContent">
                                <a id="A4" href="javascript:void(0);" tabindex="0" onclick="btnAddsubscriptionlist_click()">
                                    <span style="color: #009900; text-decoration: underline; font-size: 10pt;">Add to List</span></a>
                            </div>
                        </div>
                        <span class="clear" style="background-color: #B7CEEC;"></span>
                        <div class="frmleftContent" style="height: 105px; margin-top: 5px">
                            <asp:ListBox ID="lstSelection" runat="server" Font-Size="12px" Height="100px" Width="440px"
                                TabIndex="0"></asp:ListBox>
                        </div>
                        <span class="clear" style="background-color: #B7CEEC;"></span>
                        <div class="frmleftContent" style="text-align: center">
                            <a id="A2" href="javascript:void(0);" tabindex="0" onclick="lnkBtnAddFinalSelection()">
                                <span style="color: #000099; text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
                            <a id="A1" href="javascript:void(0);" tabindex="0" onclick="lnkBtnRemoveFromSelection()">
                                <span style="color: #cc3300; text-decoration: underline; font-size: 10pt;">Remove</span></a>
                        </div>
                    </div>
                    <div id="dvMainFilter" class="frmContent" style="width: 528px">
                        <div id="forDate">
                            <div class="frmleftContent" style="width: 110px; line-height: 20px">
                                <asp:Label ID="lblDate" runat="server" Text="Date Range : "></asp:Label>
                            </div>
                            <div class="left">
                                <div class="frmleftContent">
                                    <dxe:ASPxDateEdit ID="dtFrom" runat="server" ClientInstanceName="cdtFrom" DateOnError="Today"
                                        EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" Width="125px"
                                        Height="25px" Font-Size="11px" TabIndex="0">
                                        <DropDownButton Text="From">
                                        </DropDownButton>
                                        <ClientSideEvents DateChanged="function(s,e){DateChange(cdtFrom);}"></ClientSideEvents>
                                    </dxe:ASPxDateEdit>
                                </div>
                                <div class="frmleftContent">
                                    <dxe:ASPxDateEdit ID="dtTo" runat="server" ClientInstanceName="cdtTo" DateOnError="Today"
                                        EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" Width="125px"
                                        Height="25px" Font-Size="11px" TabIndex="0">
                                        <DropDownButton Text="To">
                                        </DropDownButton>
                                        <ClientSideEvents DateChanged="function(s,e){DateChange(cdtTo);}"></ClientSideEvents>
                                    </dxe:ASPxDateEdit>
                                </div>
                            </div>
                        </div>
                        <span class="clear"></span>
                        <div id="divAsset">
                            <div class="frmleftContent" style="width: 110px; line-height: 20px">
                                <asp:Label ID="lblAsset" runat="server" Text="Asset : "></asp:Label>
                            </div>
                            <div class="frmleftContent" style="width: 125px; padding-top: 3px; font-size: 12px;">
                                <asp:RadioButton ID="RdoAssetAll" runat="server" Checked="True" GroupName="asset"
                                    onclick="fn_RdoAssetAll()" TabIndex="0" />
                                All
                                <asp:RadioButton ID="RdoAssetSelected" runat="server" GroupName="asset" onclick="SelectionChecking('RdoAssetSelected','RdoAssetAll')"
                                    TabIndex="0" />Selected
                            </div>
                        </div>
                        <span class="clear"></span>
                        <div id="divRptType">
                            <div class="frmleftContent" style="width: 110px; line-height: 20px">
                                <asp:Label ID="lblRptType" runat="server" Text="Report Type : "></asp:Label>
                            </div>
                            <div class="frmleftContent">
                                <dxe:ASPxComboBox ID="CmbReportType" runat="server" Width="240px" Font-Size="13px"
                                    ClientInstanceName="cCmbReportType" TabIndex="0">
                                    <Items>
                                        <dxe:ListEditItem Value="0" Text="Select Report Type" />
                                        <dxe:ListEditItem Value="Obligation" Text="Obligation & Exposure" />
                                        <dxe:ListEditItem Value="OpenPosition" Text="Only Open Position & Exposure" />
                                    </Items>
                                    <ClientSideEvents ValueChanged="function(s, e) {fn_CmbReportType(s.GetValue());}" />
                                </dxe:ASPxComboBox>
                            </div>
                        </div>
                        <span class="clear"></span>
                        <div id="divReportStyle">
                            <div class="frmleftContent" style="width: 110px; line-height: 20px">
                                <asp:Label ID="lblReportStyle" runat="server" Text="Report Style : "></asp:Label>
                            </div>
                            <div class="frmleftContent">
                                <dxe:ASPxComboBox ID="CmbReportStyle" runat="server" Width="240px" Font-Size="13px"
                                    ClientInstanceName="cCmbReportStyle" TabIndex="0" OnCallback="CmbReportStyle_Callback" >
                                    <ClientSideEvents ValueChanged="function(s, e) {fn_CmbReportStyle(s.GetValue());}"
                                        EndCallback="CmbReportStyle_EndCallback" />
                                </dxe:ASPxComboBox>
                            </div>
                        </div>
                        <span class="clear"></span>
                        <div id="divReport" runat="server">
                            <div class="frmleftContent" style="width: 110px; line-height: 20px">
                                <asp:Label ID="lblReport" runat="server" Text="Report : "></asp:Label>
                            </div>
                            <div class="frmleftContent">
                                <dxe:ASPxComboBox ID="CmbReport" runat="server" Width="240px" Font-Size="13px" ClientInstanceName="cCmbReport"
                                    TabIndex="0" OnCallback="CmbReport_Callback">
                                    <ClientSideEvents ValueChanged="function(s, e) {fn_CmbReport(s.GetValue());}" EndCallback="CmbReport_EndCallback" />
                                </dxe:ASPxComboBox>
                            </div>
                        </div>
                        <span class="clear"></span>
                        <div id="divReportBy" class="Row">
                            <div class="frmleftContent" style="width: 110px; line-height: 20px">
                                Report Output :
                            </div>
                            <div class="frmleftContent">
                                <dxe:ASPxComboBox ID="CmbReportBy" runat="server" ValueType="System.String" ClientInstanceName="cCmbReportBy"
                                    Width="170px" SelectedIndex="0" TabIndex="0" OnCallback="CmbReportBy_Callback">
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) {fn_CmbReportBy(s.GetValue());}"
                                        EndCallback="CmbReportBy_EndCallback" />
                                </dxe:ASPxComboBox>
                            </div>
                        </div>
                        <span class="clear"></span>
                        <div id="divGroupBy">
                            <div class="frmleftContent" style="width: 110px; line-height: 20px">
                                <asp:Label ID="lblGroupBy" runat="server" Text="Group By : "></asp:Label>
                            </div>
                            <div class="left">
                                <div class="frmleftContent" style="padding-top: 3px">
                                    <dxe:ASPxComboBox ID="CmbGroupBy" runat="server" Width="125px" Font-Size="13px" ClientInstanceName="cCmbGroupBy"
                                        TabIndex="0" OnCallback="CmbGroupBy_Callback">
                                        <ClientSideEvents ValueChanged="function(s, e) {fn_GroupBy(s.GetValue());}" EndCallback="CmbGroupBy_EndCallback" />
                                    </dxe:ASPxComboBox>
                                </div>
                                <div id="divGroupType" class="left" style="display: none;">
                                    <div class="frmleftContent">
                                        <dxe:ASPxComboBox ID="CmbGroupType" ClientInstanceName="cCmbGroupType" runat="server"
                                            Width="125px" Font-Size="13px" TabIndex="0" OnCallback="CmbGroupType_Callback">
                                            <ClientSideEvents ValueChanged="function(s, e) {fn_CmbGroupType(s.GetValue());}"
                                                EndCallback="CmbGroupType_EndCallback" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                </div>
                                <div id="divGroupByOpt" class="frmleftContent" style="display: none; width: 125px;
                                    padding-top: 3px; font-size: 12px;">
                                    <asp:RadioButton ID="RdoGroupByAll" runat="server" Checked="True" GroupName="a" onclick="fn_RdoGroupByAll()"
                                        TabIndex="0" />
                                    All
                                    <asp:RadioButton ID="RdoGroupBySelected" runat="server" GroupName="a" onclick="SelectionChecking('RdoGroupBySelected','RdoGroupByAll')"
                                        TabIndex="0" />Selected
                                </div>
                            </div>
                        </div>
                        <span class="clear"></span>
                        <div id="divSendMail">
                            <div class="frmleftContent" style="width: 110px; line-height: 20px">
                                <asp:Label ID="Label1" runat="server" Text="Respective : "></asp:Label>
                            </div>
                            <div class="frmleftContent">
                                <dxe:ASPxComboBox ID="CmbMailSendTo" ClientInstanceName="cCmbMailSendTo" runat="server"
                                    Width="170px" Font-Size="13px" TabIndex="0" OnCallback="CmbMailSendTo_Callback">
                                    <ClientSideEvents ValueChanged="function(s, e) {fn_CmbMailSendTo(s.GetValue());}"
                                        EndCallback="CmbMailSendTo_EndCallback" />
                                </dxe:ASPxComboBox>
                            </div>
                        </div>
                        <span class="clear"></span>
                        <div id="divExport" style="float: right; margin-right: 20px;">
                            <dxe:ASPxButton ID="btnResult" runat="server" AutoPostBack="true" ClientInstanceName="cbtnResult"
                                Text="Export" OnClick="btnResult_Click" TabIndex="0" Width="100px">
                            </dxe:ASPxButton>
                        </div>
                        <br class="clear" />
                        <br class="clear" />
                    </div>
                </div>
                <br class="clear" />
                <div style="display: none">
                    <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                    <asp:HiddenField ID="HiddenField_Asset" runat="server" />
                    <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                    <asp:HiddenField ID="HiddenField_Group" runat="server" />
                    <asp:HiddenField ID="HiddenField_Client" runat="server" />
                    <asp:HiddenField ID="HiddenField_UserEmail" runat="server" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
