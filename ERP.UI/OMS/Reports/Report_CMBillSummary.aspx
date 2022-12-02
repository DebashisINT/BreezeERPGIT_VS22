<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="Reports_Report_CMBillSummary" Codebehind="Report_CMBillSummary.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxe" %>
<%--<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxCallbackPanel"
    TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>CM Bill Summary Page</title>
    <!--External Style-->
    <!--Internal Style-->
    <link type="text/css" href="../CentralData/CSS/GenericCss.css" rel="Stylesheet" />
    <!--External Javascript-->

    <script type="text/javascript" src="../CentralData/JSScript/GenericJScript.js"></script>

    <!--Start For Ajax-->

    <script type="text/javascript" src="../CentralData/JSScript/init.js"></script>

    <script type="text/javascript" src="../CentralData/JSScript/GenericAjaxList.js"></script>

    <link type="text/css" href="../CentralData/CSS/GenericAjaxStyle.css" rel="Stylesheet" />
    <!--End For Ajax-->
    <!--Internal Javascript-->

    <script language="javascript" type="text/javascript">
        function PageLoad()///Call Into Page Load
        { 
           HideShow('btnOpen','H');      
           HideShow('btnClose','H');       
           HideShow('divTotalCMBillSummary','H'); 
           HideShow('divShowFilter','H');                    
           HideShow('C1_Row2_Col4','H');
           HideShow('C1_Row2_Col5','H');
           HideShow('Container2','H'); 
           HideShow('Row1','H');
           HideShow('ShowMore','H');
           HideShow('loader','H');
           Height('260','260');                     
        } 
        function DateChange(positionDate)
        {
            var FYS='<%=Session["FinYearStart"]%>';
            var FYE='<%=Session["FinYearEnd"]%>';
            var LFY='<%=Session["LastFinYear"]%>';
            DevE_CheckForFinYear(positionDate,FYS,FYE,LFY);
            if(cDtFrom.GetDate()==positionDate.GetDate())
            {
                var setFromDate='<%=currentFromDate%>'; 
                CompareDate(cDtFrom.GetDate(),cDtTo.GetDate(),'LE','From Date Can Not Be Greater Than To Date',cDtFrom,setFromDate);
            }
            else if(cDtTo.GetDate()==positionDate.GetDate())            
            {
                var setToDate='<%=currentToDate%>'; 
                CompareDate(cDtFrom.GetDate(),cDtTo.GetDate(),'LE','To Date Can Not Be Less Than From Date',cDtTo,setToDate);
            }
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
        function BtnShowFilter_Click()
        {        
            window.location = "../reports/Report_CMBillSummary.aspx";
        }       
        function fn_GroupBy(obj)
        {
            GetObjectID('<%=lstSelection.ClientID%>').length=0;             
            if(obj=="C")
            { 
                cRblClient.SetSelectedIndex(0);     
                fn_Client('A');                
                HideShow('C1_Row2_Col4','H'); 
                HideShow('C1_Row2_Col5','H');
                HideShow('C1_Row2_Col3','S');
                CallServer("CallAjax-Client","");              
            }
            if(obj=="B")
            {
                cRblBranch.SetSelectedIndex(0);     
                fn_Branch('A'); 
                HideShow('C1_Row2_Col3','H');  
                HideShow('C1_Row2_Col5','H');               
                HideShow('C1_Row2_Col4','S');               
                CallServer("CallAjax-Branch",""); 
            }           
            if(obj=="G")                
            { 
                HideShow('C1_Row2_Col3','H'); 
                HideShow('C1_Row2_Col4','H');
                HideShow('Container2','H');
                HideShow('C1_Row2_Col5','S');
                HideShow('C1_Row2_Col6','H');
                cCmbGroupType.PerformCallback("GroupType~");
             }
        }      
        function fn_Client(obj)
        {  
            if(obj=="A")
            {
                SetValue('HiddenField_ClientBranchGroup','');                                          
                GetObjectID('<%=lstSelection.ClientID%>').length=0;
                HideShow('Container2','H');
                HideShow('C1_Row6','S');
                CallServer("CallAjax-Client",""); 
            }
            else if(obj=="S")
            {               
                if(GetObjectID('Container2').style.display=="inline")
                {                
                   cRblClient.SetSelectedIndex(0);     
                   lnkBtnAddFinalSelection();                        	
                }
                else
                {
                   HideShow('Container2','S');
                   HideShow('C1_Row6','H');
                   CallServer("CallAjax-Client",""); 
                   //GetObjectID('txtSelectionID').focus();
                } 
            }                         
        }        
        function fn_Branch(obj)
        {  
            if(obj=="A")
            {
                SetValue('HiddenField_ClientBranchGroup','');
                GetObjectID('<%=lstSelection.ClientID%>').length=0;
                HideShow('Container2','H');
                HideShow('C1_Row6','S');
            }
            else if(obj=="S")
            {               
                if(GetObjectID('Container2').style.display=="inline")
                {                
                   cRblBranch.SetSelectedIndex(0);     
                   lnkBtnAddFinalSelection();                        	
                }
                else
                {
                   HideShow('Container2','S');
                   HideShow('C1_Row6','H');
                   CallServer("CallAjax-Branch",""); 
                   //GetObjectID('txtSelectionID').focus();
                } 
            }                         
        }
        function CmbGroupType_EndCallback()
        {
           if(cCmbGroupType.cpBindGroupType!=undefined)
           {
               if(cCmbGroupType.cpBindGroupType=="Y")
               {
                    cCmbGroupType.SetSelectedIndex(0);  
                    SetValue('HiddenField_ClientBranchGroup','');
               }
               else if(cCmbGroupType.cpBindGroupType=="N")
               {
                    cCmbGroupType.SetEnabled(false);                   
               }
           }          
           Height('260','260');
        }
        function fn_CmbGroupType(obj)
        {
            if(obj=="0")
            {
               HideShow('C1_Row2_Col6','H');
               alert('Please Select Group Type !');
               cbtnExport.SetEnabled(false);
            }
            else
            {
               cRblGroup.SetSelectedIndex(0);     
               HideShow('C1_Row2_Col6','S');
               cbtnExport.SetEnabled(true);
            }
           Height('260','260');
        }      
        function fn_Group(obj)
        {  
            if(obj=="A")
            {
                SetValue('HiddenField_ClientBranchGroup','');                                          
                GetObjectID('<%=lstSelection.ClientID%>').length=0;
                HideShow('Container2','H');
                HideShow('C1_Row6','S');
            }
            else if(obj=="S")
            {               
                if(GetObjectID('Container2').style.display=="inline")
                {                
                   cRblGroup.SetSelectedIndex(0);     
                   lnkBtnAddFinalSelection();                        	
                }
                else
                {
                   HideShow('Container2','S');
                   HideShow('C1_Row6','H');
                   CallServer("CallAjax-Group~"+cCmbGroupType.GetText(),"");
                   //GetObjectID('txtSelectionID').focus();
                } 
            }                         
        }
        function fn_ReportBy(obj)
        {        
            if(obj=='S')
            {
                cbtnExport.SetText('Show Screen');
            }
            if(obj=='E')
            {
                cbtnExport.SetText('Export Excel');
            }
        }
        function NORECORD()
        {
             alert('No Record Found !!!');  
             Reset();                    
        }
        function ErrorMsg(msg)
        {
            if(msg=="ClientErr")
                alert("There is No Proper Client Selection!!!");
            else if(msg=="BranchErr")
                alert("There is No Proper Branch Selection!!!");
            else if(msg=="GroupErr")
                alert("There is No Proper Group Selection!!!");             
        }
       function Reset()
        {
           cCmbGroupBy.SetSelectedIndex(0); 
           cCmbGroupBy.SetEnabled(true); 
           cRblClient.SetSelectedIndex(0);
           cRblClient.SetEnabled(true); 
           cRblBranch.SetSelectedIndex(0);
           cRblGroup.SetSelectedIndex(0);
           cCmbGroupType.SetSelectedIndex(0);
           HideShow('C1_Row2_Col4','H');       
           HideShow('C1_Row2_Col5','H');
           GetObjectID('<%=lstSelection.ClientID%>').length=0;
           SetValue('HiddenField_ClientBranchGroup',''); 
           HideShow('Container2','H');                                                     
           Height('260','260');           
       } 
       function fn_SearchFilter_Hide()
       {
           HideShow('Row0','H');
           HideShow('divShowFilter','S'); 
           HideShow('Container2','S'); 
           HideShow('btnClose','H');       
           HideShow('divTotalCMBillSummary','H'); 
           var filterColVal=GetObjectID('HDNFilterCol').value;
           if (filterColVal.indexOf("Y") != -1) 
           {                
               filterColVal=filterColVal.substring(1,filterColVal.length);
               if (filterColVal.search("Y") == 1) 
                   HideShow('btnOpen','S');       
               else
                   HideShow('btnOpen','H');                
           }
           else 
               HideShow('btnOpen','H');            
           HideShow('Row1','S');
           HideShow('ShowMore','S'); 
           HideShow('loader','H');  
           Height('260','260');
       }
       function btnCloseTotal_click()
       {
          HideShow('btnClose','H');
          HideShow('divTotalCMBillSummary','H');
          HideShow('btnOpen','S');
       } 
       function btnOpenTotal_click()
       {
          HideShow('btnOpen','H');
          HideShow('btnClose','S');
          HideShow('divTotalCMBillSummary','S');
       }             
    </script>

    <!-- CallAjax and Receive Server Script-->

    <script language="javascript" type="text/javascript">       
        FieldName='none';
        function btnAddToList_click()
        {
            var txtName = GetObjectID('txtSelectionID');
            if(txtName != '')
            {
                var txtId = GetValue('txtSelectionID_hidden');
                var listBox = GetObjectID('lstSelection');
                var listLength = listBox.length;               
                var opt = new Option();
                opt.value = txtId;
                opt.text = txtName.value;
                listBox[listLength]=opt;
                txtName.value='';
            }
            else
                alert('Please Search Name And Then Add!');
            txtName.focus();
            txtName.select();   
        }
        function lnkBtnAddFinalSelection()
	    {
	        var listBox = GetObjectID('lstSelection');         
            var listID='';
            var i;
            if(listBox.length > 0)
            {                             
                for(i=0;i<listBox.length;i++)
                {
                    if(listID == '')
                        listID = listBox.options[i].value+'!'+listBox.options[i].text;
                    else
                        listID += '^' + listBox.options[i].value+'!'+listBox.options[i].text;
                }
                CallServer(listID,"");  
                var j;
                for(j=listBox.options.length-1;j>=0;j--)
                {
                    listBox.remove(j);
                } 
                HideShow('Container2','H');
                HideShow('C1_Row6','S');                    
            }
            else if((GetObjectID('Container2').style.display=="inline") && (listBox.length == 0))
            { 
                if((cCmbGroupBy.GetSelectedIndex()==0) && (cRblClient.GetSelectedIndex()==1))
                {             
                     alert("Please Select Atleast One Client Item!!!");
                }   
                else if((cCmbGroupBy.GetSelectedIndex()==1) && (cRblBranch.GetSelectedIndex()==1))
                {             
                    alert("Please Select Atleast One Branch Item!!!");
                }
                else if((cCmbGroupBy.GetSelectedIndex()==2) && (cRblGroup.GetSelectedIndex()==1))
                {             
                     alert("Please Select Atleast One Group Item!!!");
                }                                   
            }
	    }
	    function lnkBtnRemoveFromSelection()
        {   
            var listBox = GetObjectID('lstSelection');
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
        function string_contains(containerString, matchBySubString)
        {
            if(containerString.indexOf(matchBySubString) == -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }          
        function ReceiveServerData(rValue)
        {            
            var Data=rValue.split('@');           
            if(Data[1]!="undefined")
            {
                 if((Data[0]=='Branch')||(Data[0]=='Group')||(Data[0]=='Client'))
                    SetValue('HiddenField_ClientBranchGroup',Data[1]);                                                     
            } 
            if(Data[0]=='AjaxQuery')
            {                
                AjaxComQuery = Data[1];               
                var AjaxList_TextBox=GetObjectID('txtSelectionID');
                AjaxList_TextBox.value='';
                AjaxList_TextBox.attachEvent('onkeyup',CallGenericAjaxJS);
             }              
        }
        function CallGenericAjaxJS(e)
        {
            var AjaxList_TextBox=GetObjectID('txtSelectionID');
            AjaxList_TextBox.focus();
            AjaxComQuery=AjaxComQuery.replace("\'","'");
            var GenericAjaxListAspxPath = '../CentralData/Pages/GenericAjaxList.aspx';            
            ajax_showOptions(AjaxList_TextBox,'GenericAjaxList',e,replaceChars(AjaxComQuery),'Main',GenericAjaxListAspxPath);      
        }
        function CallAjax(obj1,obj2,obj3,Query)
        {
            var CombinedQuery=new String(Query);
            var GenericAjaxListAspxPath = '../CentralData/Pages/GenericAjaxList.aspx';
            ajax_showOptions(obj1,obj2,obj3,replaceChars(CombinedQuery),'Main',GenericAjaxListAspxPath);
        }        
    </script>

    <!--Infinite Scrolling Script-->

    <script type="text/javascript" src="/assests/js/jquery.min.js"></script>

    <script language="javascript" type="text/javascript">
               var pageNumber = 1;
               var pageCount; 
               function btnShowMOre_click()
               {
                    GetRecords();
               }             
               $(parent.window).scroll(function () 
               {
                  if(GetObjectID('Row1').style.display=='inline')
                  {
                      if ($(parent.window).scrollTop() == $(parent.document).height() - $(parent.window).height()) 
                      {                        
                         GetRecords();
                      }
                  }
                });                
                function GetRecords() 
                {
                
                    pageNumber++;                                             
                    if (pageNumber == 2 || pageNumber <= pageCount) 
                    {                        
                        $("#ShowMore").hide();
                        $("#loader").show();                      

                        $.ajax({
                            type: "POST",
                            url: "Report_CMBillSummary.aspx/GetCMBillSummary",
                            data: '{pageNumber: ' + pageNumber + '}',         
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",                          
                            success: OnSuccess,
                            failure: function (response) 
                            {
                                alert(response.d);                                    
                            },
                            error: function (response) 
                            {
                               alert(response.d);                   
                            }
                        });                                     
                    }
                    else if(pageNumber > pageCount) 
                    {                       
                        $("#ShowMore").hide();
                        $("#loader").hide(); 
                        alert('No More Record Available!!!');  
                    }                    
                }
                function OnSuccess(response) {
                    var xmlDoc = $.parseXML(response);
                    var xml = $(xmlDoc);                   
                    pageCount = parseInt(xml.find("Table1").eq(0).find("PageCount").text());
                    var CMBillSummary = xml.find("Table");                  
                    CMBillSummary.each(function () 
                    {
                        var i=0;
                        var filterData=GetObjectID('<%=HDNFilterCol.ClientID%>').value.split('~');  
                                            
                        var cCMBillSummary = $(this);
                        var row = $("[id*=gvwCMBillSummary] tr:last-child").clone(true);                    
                            $("td", row).eq(0).html((cCMBillSummary.find("Client").text()).length==0?'&nbsp;':(cCMBillSummary.find("Client").text()).search("\\*") != -1 ? '<a href="javascript:void(0);" title="'+cCMBillSummary.find("Client").text()+'"><b>'+(cCMBillSummary.find("Client").text()).substring(0,40) +'</b></a>' : '<a href="javascript:void(0);" title="'+cCMBillSummary.find("Client").text()+'">'+(cCMBillSummary.find("Client").text()).substring(0,40)+'</a>');
                            $("td", row).eq(1).html((cCMBillSummary.find("SettNum").text()).length==0?'&nbsp;':cCMBillSummary.find("SettNum").text());
                            $("td", row).eq(2).html((cCMBillSummary.find("BillDate").text()).length==0?'&nbsp;':cCMBillSummary.find("BillDate").text());
                            i=3;
                            if(filterData[2]=='N')
                            {                                
                                $("td", row).eq(i).html('&nbsp;');                           
                            }
                            else 
                            {    
                                $("td", row).eq(i).html((cCMBillSummary.find("NetObligation").text()).length==0?'&nbsp;':(cCMBillSummary.find("NetObligation").text()).search("-") != -1 ? '<span style="color:red">('+(cCMBillSummary.find("NetObligation").text()).substr(1,((cCMBillSummary.find("NetObligation").text()).length-1))+')</span>' : cCMBillSummary.find("NetObligation").text());
                                i=i+1;                                
                            }
                            
                            if(filterData[3]=='N')
                            {                                
                                 $("td", row).eq(i).html('&nbsp;');                           
                            }
                            else 
                            {    
                                $("td", row).eq(i).html((cCMBillSummary.find("STT").text()).length==0?'&nbsp;':(cCMBillSummary.find("STT").text()).search("-") != -1 ? '<span style="color:red">('+(cCMBillSummary.find("STT").text()).substr(1,((cCMBillSummary.find("STT").text()).length-1))+')</span>' : cCMBillSummary.find("STT").text());
                                i=i+1;                                
                            }
                            if(filterData[4]=='N')
                            {                                
                                $("td", row).eq(i).html('&nbsp;');                          
                            }
                            else 
                            {    
                                $("td", row).eq(i).html((cCMBillSummary.find("TranCharge").text()).length==0?'&nbsp;':(cCMBillSummary.find("TranCharge").text()).search("-") != -1 ? '<span style="color:red">('+(cCMBillSummary.find("TranCharge").text()).substr(1,((cCMBillSummary.find("TranCharge").text()).length-1))+')</span>' : cCMBillSummary.find("TranCharge").text());
                                i=i+1;                                
                            }
                            if(filterData[5]=='N')
                            {                                
                                 $("td", row).eq(i).html('&nbsp;');                            
                            }
                            else 
                            {    
                                $("td", row).eq(i).html((cCMBillSummary.find("StampDuty").text()).length==0?'&nbsp;':(cCMBillSummary.find("StampDuty").text()).search("-") != -1 ? '<span style="color:red">('+(cCMBillSummary.find("StampDuty").text()).substr(1,((cCMBillSummary.find("StampDuty").text()).length-1))+')</span>' : cCMBillSummary.find("StampDuty").text());
                                i=i+1;
                            }
                            if(filterData[6]=='N')
                            {                                
                                 $("td", row).eq(i).html('&nbsp;');                 
                            }
                            else 
                            {    
                                $("td", row).eq(i).html((cCMBillSummary.find("DlvCharge").text()).length==0?'&nbsp;':(cCMBillSummary.find("DlvCharge").text()).search("-") != -1 ? '<span style="color:red">('+(cCMBillSummary.find("DlvCharge").text()).substr(1,((cCMBillSummary.find("DlvCharge").text()).length-1))+')</span>' : cCMBillSummary.find("DlvCharge").text());
                                i=i+1;
                            }
                            if(filterData[7]=='N')
                            {                                
                                $("td", row).eq(i).html('&nbsp;');                     
                            }
                            else 
                            {    
                                $("td", row).eq(i).html((cCMBillSummary.find("SebiFee").text()).length==0?'&nbsp;':(cCMBillSummary.find("SebiFee").text()).search("-") != -1 ? '<span style="color:red">('+(cCMBillSummary.find("SebiFee").text()).substr(1,((cCMBillSummary.find("SebiFee").text()).length-1))+')</span>' : cCMBillSummary.find("SebiFee").text());
                                i=i+1;
                            }
                            if(filterData[8]=='N')
                            {                                
                                $("td", row).eq(i).html('&nbsp;');                        
                            }
                            else 
                            {    
                                $("td", row).eq(i).html((cCMBillSummary.find("OtherCharge").text()).length==0?'&nbsp;':(cCMBillSummary.find("OtherCharge").text()).search("-") != -1 ? '<span style="color:red">('+(cCMBillSummary.find("OtherCharge").text()).substr(1,((cCMBillSummary.find("OtherCharge").text()).length-1))+')</span>' : cCMBillSummary.find("OtherCharge").text());
                                i=i+1;
                            }
                            if(filterData[9]=='N')
                            {                                
                                 $("td", row).eq(i).html('&nbsp;');                         
                            }
                            else 
                            {    
                                $("td", row).eq(i).html((cCMBillSummary.find("ServTax").text()).length==0?'&nbsp;':(cCMBillSummary.find("ServTax").text()).search("-") != -1 ? '<span style="color:red">('+(cCMBillSummary.find("ServTax").text()).substr(1,((cCMBillSummary.find("ServTax").text()).length-1))+')</span>' : cCMBillSummary.find("ServTax").text());
                                i=i+1;
                            }
                            if(filterData[10]=='N')
                            {                                
                                 $("td", row).eq(i).html('&nbsp;');                     
                            }
                            else 
                            {    
                                $("td", row).eq(i).html((cCMBillSummary.find("NetBillAmount").text()).length==0?'&nbsp;':(cCMBillSummary.find("NetBillAmount").text()).search("-") != -1 ? '<span style="color:red">('+(cCMBillSummary.find("NetBillAmount").text()).substr(1,((cCMBillSummary.find("NetBillAmount").text()).length-1))+')</span>' : cCMBillSummary.find("NetBillAmount").text());
                                i=i+1;
                            }
                            if(filterData[11]=='N')
                            {                                
                                 $("td", row).eq(i).html('&nbsp;');                       
                            }
                            else 
                            {    
                                $("td", row).eq(i).html((cCMBillSummary.find("Turnover").text()).length==0?'&nbsp;':(cCMBillSummary.find("Turnover").text()).search("-") != -1 ? '<span style="color:red">('+(cCMBillSummary.find("Turnover").text()).substr(1,((cCMBillSummary.find("Turnover").text()).length-1))+')</span>' : cCMBillSummary.find("Turnover").text());
                                i=i+1;
                            }
                            if(filterData[12]=='N')
                            {                                
                                 $("td", row).eq(i).html('&nbsp;');                       
                            }
                            else 
                            {    
                                $("td", row).eq(i).html((cCMBillSummary.find("TotalBrokerage").text()).length==0?'&nbsp;':(cCMBillSummary.find("TotalBrokerage").text()).search("-") != -1 ? '<span style="color:red">('+(cCMBillSummary.find("TotalBrokerage").text()).substr(1,((cCMBillSummary.find("TotalBrokerage").text()).length-1))+')</span>' : cCMBillSummary.find("TotalBrokerage").text());
                                i=i+1;
                            }
                            if(filterData[13]=='N')
                            {                                
                                $("td", row).eq(i).html('&nbsp;');                           
                            }
                            else 
                            {    
                                $("td", row).eq(i).html((cCMBillSummary.find("UnrealizedServTax").text()).length==0?'&nbsp;':(cCMBillSummary.find("UnrealizedServTax").text()).search("-") != -1 ? '<span style="color:red">('+(cCMBillSummary.find("UnrealizedServTax").text()).substr(1,((cCMBillSummary.find("UnrealizedServTax").text()).length-1))+')</span>' : cCMBillSummary.find("UnrealizedServTax").text());
                                i=i+1;
                            }
                            if(filterData[14]=='N')
                            {                                
                                 $("td", row).eq(i).html('&nbsp;');                            
                            }
                            else 
                            {    
                                $("td", row).eq(i).html((cCMBillSummary.find("UnrealizedSTT").text()).length==0?'&nbsp;':(cCMBillSummary.find("UnrealizedSTT").text()).search("-") != -1 ? '<span style="color:red">('+(cCMBillSummary.find("UnrealizedSTT").text()).substr(1,((cCMBillSummary.find("UnrealizedSTT").text()).length-1))+')</span>' : cCMBillSummary.find("UnrealizedSTT").text());
                                i=i+1;
                            }                           
                            $("[id*=gvwCMBillSummary]").append(row);                                                                                            
                    });                    
                    $("#loader").hide();                                                   
                    $("#ShowMore").show();
                    Height('260','260');        
                }  
    </script>

    <!--  For Floating Div with snap-->

    <script type="text/javascript" src="/assests/js/floating-1.12.js"></script>

    <script type="text/javascript">  
//    floatingMenu.add('divTotalCMBillSummary',  
//        {  
//            // Represents distance from left or right browser window  
//            // border depending upon property used. Only one should be  
//            // specified.  
//             targetLeft: 6,  
//            //targetRight: 10,  
//  
//            // Represents distance from top or bottom browser window  
//            // border depending upon property used. Only one should be  
//            // specified.  
//            //targetTop: 30,  
//             targetBottom: 123,  
//  
//            // Uncomment one of those if you need centering on  
//            // X- or Y- axis.  
//            // centerX: true,  
//            // centerY: true,  
//  
//            // Remove this one if you don't want snap effect  
//             snap: true  
//        }); 
     floatingMenu.add('divTotalCMBillSummary',{targetLeft: 6,targetBottom: 123,snap: true});    
     floatingMenu.add('btnClose',targetLeft: 20, targetBottom: 190,snap: true});      
     floatingMenu.add('btnOpen',targetLeft: 20, targetBottom: 190,snap: true});   
    </script>

    <!--Page Style-->
    <style type="text/css">       
        #MainFull { padding:5px;}        
        #Container1 { width: 435px; padding:5px}
        .LableWidth { width:110px;}
        .ContentWidth { width:125px; height:21px;}       
        .labelCont { font-size:13px; margin-top:7px;}      
        .btnRight { margin-right:18px; float:right;} 
        .txt_left { text-align:left;}
        .txt_right { text-align:right;}	    
       
        .Box_filter { width:170px; height:120px;} 
        .Box_filterCont { width:169px; height:109px;}
        .Box_filter_HeaderCont { width:167px; text-align:center; color: #000099; font-size:12px; border: 1px solid #316AC5; 
                                    background-color: #97B9E9; }
        .Box_filter_HeaderNotCont { width:167px; text-align:center; color: #333; font-size:12px; border: 1px solid #aaa;
                                        background-color: #DDD; }
        .Box_filterBtn { width: 25px; padding-top: 10px;}
        .Btn_filterAddAll { background: url(../images/pLast.png) no-repeat; border:none; width: 22px; cursor:pointer;
                                margin-bottom:3px;}
        .Btn_filterAdd { background: url(../images/pNext.png) no-repeat; border:none; width: 22px; cursor:pointer;
                            margin-bottom:3px;}
        .Btn_filterRemove { background: url(../images/pPrev.png) no-repeat; border:none; width: 22px; cursor:pointer;
                                margin-bottom:3px;}
        .Btn_filterRemoveAll { background: url(../images/pFirst.png) no-repeat; border:none; width: 22px; cursor:pointer;
                                margin-bottom:3px;}                        
        .loaderShowMore { text-align:center; color: #000099; font-size:14px; border: 1px solid #316AC5;
                            background-color: #C1D2EE; padding:10px 5px 10px 5px; margin: 5px; width:450px;}
        .loading { bottom:5px; z-index:60; text-align:center; border: 1px solid #316AC5; background-color: #C1D2EE;
                    padding:10px 5px 10px 5px; margin: 5px; width:460px;}
        .txt_left { text-align:left;}
        .txt_right { text-align:right;}
        .Box_Detail { width: 985px; overflow-x: auto; overflow-y: hidden; padding-bottom: 18px; border:1px solid #aaa;}
        .cellHeader { padding:2px; height: 25px; border: solid .1pt #aaa;font-size: 13px; background-color: #DDD;
                        font-weight:bold; text-align:center;}                
        .cellRecord { padding:2px; border: solid .1pt  #aaa;font-size: 12px; background-color: #eee;}                
        .Box_TotalRecord { margin-left: 5px; bottom: 123px; z-index: 50; position: absolute; overflow-x: auto; overflow-y: hidden; 
                           width: 1080px; height: 65px; padding: 2px 20px 2px 2px; background-color: #aaa; opacity: 0.9; filter: alpha(opacity=90);}
        .cellTotHeader { padding:2px; height: 22px; border: solid .1pt #F4A460;font-size: 11px; background-color: #DDD;
                         font-weight:bold; text-align:center; width:110px;}                
        .cellTotal { padding:2px; border: solid .1pt #F4A460;font-size: 11px; background-color: #DDD; color:maroon;
                     font-weight:bold; text-align:right; width:110px;}                        
        .Box_closeTotal { margin-left:20px; bottom:190px; z-index: 55; position: fixed; width:16px; height:20px;
                      text-align:center; color: #000;  font-weight:bold; background: #F4A460; border:solid 2px #ccc; border-bottom:none; }
        .buttonMore { cursor:pointer;}                                             
   </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="btnOpen" class="Box_closeTotal">
            <a href="javascript:void(0);" onclick="btnOpenTotal_click()" title="Maximize Total">
                +</a></div>
        <div id="btnClose" class="Box_closeTotal">
            <a href="javascript:void(0);" onclick="btnCloseTotal_click()" title="Minimize Total">
                -</a></div>
        <div id="divTotalCMBillSummary" class="Box_TotalRecord">
            <div id="divTotHeaderCMBillSummary" style="width: 1800px;">
                <div style="width: 100%">
                    <div class="left cellTotHeader">
                        &nbsp;
                    </div>
                    <div id="totHeaderNetObligation" runat="server" class="left cellTotHeader">
                        <b>Tot NetObligation</b>
                    </div>
                    <div id="totHeaderSTT" runat="server" class="left cellTotHeader">
                        <b>Tot STT</b>
                    </div>
                    <div id="totHeaderTranCharge" runat="server" class="left cellTotHeader">
                        <b>Tot TranCharge</b>
                    </div>
                    <div id="totHeaderStampDuty" runat="server" class="left cellTotHeader">
                        <b>Tot StampDuty</b>
                    </div>
                    <div id="totHeaderDlvCharge" runat="server" class="left cellTotHeader">
                        <b>Tot DlvCharge</b>
                    </div>
                    <div id="totHeaderSebiFee" runat="server" class="left cellTotHeader">
                        <b>Tot SebiFee</b>
                    </div>
                    <div id="totHeaderOtherCharge" runat="server" class="left cellTotHeader">
                        <b>Tot OtherCharge</b>
                    </div>
                    <div id="totHeaderServTax" runat="server" class="left cellTotHeader">
                        <b>Tot ServTax</b>
                    </div>
                    <div id="totHeaderNetBillAmount" runat="server" class="left cellTotHeader">
                        <b>Tot NetBillAmount</b>
                    </div>
                    <div id="totHeaderTurnover" runat="server" class="left cellTotHeader">
                        <b>Tot Turnover</b>
                    </div>
                    <div id="totHeaderTotalBrokerage" runat="server" class="left cellTotHeader">
                        <b>Tot TotalBrokerage</b>
                    </div>
                    <div id="totHeaderUnrealizedServTax" runat="server" class="left cellTotHeader">
                        <b>Tot UnrealizedServTax</b>
                    </div>
                    <div id="totHeaderUnrealizedSTT" runat="server" class="left cellTotHeader">
                        <b>Tot UnrealizedSTT</b>
                    </div>
                    <span class="clear"></span>
                </div>
                <span class="clear"></span>
                <div style="width: 100%">
                    <div class="left cellTotal">
                        <span class="left"><b>Grand Total : </b></span>
                    </div>
                    <div id="footerTotNetObligation" runat="server" class="left cellTotal">
                        <span class="right">
                            <%=TotNetObligation%>
                        </span>
                    </div>
                    <div id="footerTotSTT" runat="server" class="left cellTotal">
                        <span class="right">
                            <%=TotSTT%>
                        </span>
                    </div>
                    <div id="footerTotTranCharge" runat="server" class="left cellTotal">
                        <span class="right">
                            <%=TotTranCharge%>
                        </span>
                    </div>
                    <div id="footerTotStampDuty" runat="server" class="left cellTotal">
                        <span class="right">
                            <%=TotStampDuty%>
                        </span>
                    </div>
                    <div id="footerTotDlvCharge" runat="server" class="left cellTotal">
                        <span class="right">
                            <%=TotDlvCharge%>
                        </span>
                    </div>
                    <div id="footerTotSebiFee" runat="server" class="left cellTotal">
                        <span class="right">
                            <%=TotSebiFee%>
                        </span>
                    </div>
                    <div id="footerTotOtherCharge" runat="server" class="left cellTotal">
                        <span class="right">
                            <%=TotOtherCharge%>
                        </span>
                    </div>
                    <div id="footerTotServTax" runat="server" class="left cellTotal">
                        <span class="right">
                            <%=TotServTax%>
                        </span>
                    </div>
                    <div id="footerTotNetBillAmount" runat="server" class="left cellTotal">
                        <span class="right">
                            <%=TotNetBillAmount%>
                        </span>
                    </div>
                    <div id="footerTotTurnover" runat="server" class="left cellTotal">
                        <span class="right">
                            <%=TotTurnover%>
                        </span>
                    </div>
                    <div id="footerTotTotalBrokerage" runat="server" class="left cellTotal">
                        <span class="right">
                            <%=TotTotalBrokerage%>
                        </span>
                    </div>
                    <div id="footerTotUnrealizedServTax" runat="server" class="left cellTotal">
                        <span class="right">
                            <%=TotUnrealizedServTax%>
                        </span>
                    </div>
                    <div id="footerTotUnrealizedSTT" runat="server" class="left cellTotal">
                        <span class="right">
                            <%=TotUnrealizedSTT%>
                        </span>
                    </div>
                    <span class="clear" style="height: 0px"></span>
                </div>
                <span class="clear"></span>
            </div>
        </div>
        <div id="MainFull">
            <div id="Header" class="Header">
                <div id="divShowFilter" style="text-align: right;">
                    <dxe:ASPxButton ID="btnShowFilter" runat="server" AutoPostBack="False" ClientInstanceName="cbtnShowFilter"
                        Text="Show Filter" Font-Size="7" TabIndex="0" CssClass="btnRight">
                        <ClientSideEvents Click="function(s,e){BtnShowFilter_Click();}" />
                        <Paddings Padding="0" PaddingBottom="0" PaddingLeft="0" PaddingRight="0" />
                    </dxe:ASPxButton>
                </div>
                Report For CM Bill Summary <span class="clear"></span>
            </div>
            <div id="Row0" class="Row">
                <div id="Container1" class="container">
                    <div id="C1_Row1" class="Row">
                        <div id="C1_Row1_Col1" class="LFloat_Lable LableWidth">
                            <asp:Label ID="lblDate" runat="server" Text="Date Range : "></asp:Label>
                        </div>
                        <div id="C1_Row1_Col2" class="LFloat_Content ContentWidth">
                            <dxe:ASPxDateEdit ID="DtFrom" runat="server" ClientInstanceName="cDtFrom" DateOnError="Today"
                                EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" Width="125px"
                                Font-Size="11px" TabIndex="0">
                                <DropDownButton Text="From">
                                </DropDownButton>
                                <ClientSideEvents DateChanged="function(s,e){DateChange(cDtFrom);}"></ClientSideEvents>
                            </dxe:ASPxDateEdit>
                        </div>
                        <div id="C1_Row1_Col3" class="LFloat_Content ContentWidth">
                            <dxe:ASPxDateEdit ID="DtTo" runat="server" ClientInstanceName="cDtTo" DateOnError="Today"
                                EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" Width="125px"
                                Font-Size="11px" TabIndex="0">
                                <DropDownButton Text="To">
                                </DropDownButton>
                                <ClientSideEvents DateChanged="function(s,e){DateChange(cDtTo);}"></ClientSideEvents>
                            </dxe:ASPxDateEdit>
                        </div>
                        <span class="clear"></span>
                    </div>
                    <span class="clear"></span>
                    <div id="C1_Row2" class="Row">
                        <div id="C1_Row2_Col1" class="LFloat_Lable LableWidth">
                            <asp:Label ID="lblGroupBy" runat="server" Text="Group By : "></asp:Label>
                        </div>
                        <div class="left">
                            <div id="C1_Row2_Col2" class="LFloat_Content ContentWidth">
                                <dxe:ASPxComboBox ID="CmbGroupBy" runat="server" ValueType="System.String" ClientInstanceName="cCmbGroupBy"
                                    SelectedIndex="0" TabIndex="0" Width="125px">
                                    <Items>
                                        <dxe:ListEditItem Text="Client" Value="C"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Branch" Value="B"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Group" Value="G"></dxe:ListEditItem>
                                    </Items>
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) {fn_GroupBy(s.GetValue());}" />
                                </dxe:ASPxComboBox>
                            </div>
                            <div class="left">
                                <div>
                                    <div id="C1_Row2_Col3" class="LFloat_Content ContentWidth">
                                        <dxe:ASPxRadioButtonList ID="RblClient" runat="server" SelectedIndex="0" ItemSpacing="15px"
                                            Paddings-PaddingTop="1px" RepeatDirection="Horizontal" TextWrap="False" TabIndex="0"
                                            ClientInstanceName="cRblClient">
                                            <Items>
                                                <dxe:ListEditItem Text="All" Value="A" />
                                                <dxe:ListEditItem Text="Selected" Value="S" />
                                            </Items>
                                            <ClientSideEvents ValueChanged="function(s, e) {fn_Client(s.GetValue());}" />
                                            <Border BorderWidth="0px" />
                                        </dxe:ASPxRadioButtonList>
                                    </div>
                                </div>
                                <div>
                                    <div id="C1_Row2_Col4" class="LFloat_Content ContentWidth">
                                        <dxe:ASPxRadioButtonList ID="RblBranch" runat="server" SelectedIndex="0" ItemSpacing="15px"
                                            Paddings-PaddingTop="1px" RepeatDirection="Horizontal" TextWrap="False" TabIndex="0"
                                            ClientInstanceName="cRblBranch">
                                            <Items>
                                                <dxe:ListEditItem Text="All" Value="A" />
                                                <dxe:ListEditItem Text="Selected" Value="S" />
                                            </Items>
                                            <ClientSideEvents ValueChanged="function(s, e) {fn_Branch(s.GetValue());}" />
                                            <Border BorderWidth="0px" />
                                        </dxe:ASPxRadioButtonList>
                                    </div>
                                    <span class="clear"></span>
                                </div>
                                <div id="C1_Row2_Col5">
                                    <div class="LFloat_Content ContentWidth">
                                        <dxe:ASPxComboBox ID="CmbGroupType" ClientInstanceName="cCmbGroupType" runat="server"
                                            Font-Size="11px" TabIndex="0" Width="125px" OnCallback="CmbGroupType_Callback">
                                            <ClientSideEvents ValueChanged="function(s, e) {fn_CmbGroupType(s.GetValue());}"
                                                EndCallback="CmbGroupType_EndCallback" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div id="C1_Row2_Col6" class="LFloat_Content ContentWidth" style="display: none;
                                        margin-top: 3px;">
                                        <dxe:ASPxRadioButtonList ID="RblGroup" runat="server" SelectedIndex="0" ItemSpacing="15px"
                                            Paddings-PaddingTop="1px" RepeatDirection="Horizontal" TextWrap="False" TabIndex="0"
                                            ClientInstanceName="cRblGroup">
                                            <Items>
                                                <dxe:ListEditItem Text="All" Value="A" />
                                                <dxe:ListEditItem Text="Selected" Value="S" />
                                            </Items>
                                            <ClientSideEvents ValueChanged="function(s, e) {fn_Group(s.GetValue());}" />
                                            <Border BorderWidth="0px" />
                                        </dxe:ASPxRadioButtonList>
                                    </div>
                                    <span class="clear"></span>
                                </div>
                                <span class="clear"></span>
                            </div>
                            <span class="clear"></span>
                        </div>
                        <span class="clear"></span>
                    </div>
                    <span class="clear"></span>
                    <div id="C1_Row3" class="Row">
                        <div id="C1_Row3_Col1" class="LFloat_Lable LableWidth">
                            <asp:Label ID="Label1" runat="server" Text="Order By : "></asp:Label>
                        </div>
                        <div id="C1_Row3_Col2" class="LFloat_Content">
                            <dxe:ASPxComboBox ID="CmbOrderBy" runat="server" ValueType="System.String" ClientInstanceName="cCmbOrderBy"
                                SelectedIndex="0" TabIndex="0" Width="260px">
                                <Items>
                                    <dxe:ListEditItem Text="Customer+TradeDate" Value="1"></dxe:ListEditItem>
                                    <dxe:ListEditItem Text="TradeDate+Customer" Value="2"></dxe:ListEditItem>
                                </Items>
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                    <span class="clear"></span>
                    <div id="C1_Row4" class="Row">
                        <div id="C1_Row4_Col1" class="LFloat_Lable LableWidth">
                            <asp:Label ID="Label2" runat="server" Text="Report Type : "></asp:Label>
                        </div>
                        <div id="C1_Row4_Col2" class="LFloat_Content">
                            <dxe:ASPxComboBox ID="CmbReportType" runat="server" ValueType="System.String" ClientInstanceName="cCmbReportType"
                                SelectedIndex="0" TabIndex="0" Width="260px">
                                <Items>
                                    <dxe:ListEditItem Text="Settlement Number Wise Detail" Value="1"></dxe:ListEditItem>
                                </Items>
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                    <span class="clear"></span>
                    <div id="C1_Row5" class="Row">
                        <div id="C1_Row5_Col1" class="LFloat_Lable LableWidth">
                            <asp:Label ID="Label3" runat="server" Text="Report By : "></asp:Label>
                        </div>
                        <div id="C1_Row5_Col2" class="LFloat_Content ContentWidth">
                            <dxe:ASPxComboBox ID="CmbReportBy" runat="server" ValueType="System.String" ClientInstanceName="cCmbReportBy"
                                SelectedIndex="0" TabIndex="0" Width="125px">
                                <Items>
                                    <dxe:ListEditItem Text="Screen" Value="S"></dxe:ListEditItem>
                                    <dxe:ListEditItem Text="Excel export" Value="E"></dxe:ListEditItem>
                                </Items>
                                <ClientSideEvents ValueChanged="function(s, e) {fn_ReportBy(s.GetValue());}" />
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                    <span class="clear"></span>
                    <br class="clear" />
                    <div id="C1_Row6" class="Row">
                        <dxe:ASPxButton ID="btnExport" runat="server" ClientInstanceName="cbtnExport" CssClass="btnUpdate"
                            AutoPostBack="False" Height="5px" Text="Show Screen" Width="101px" OnClick="btnExport_Click">
                        </dxe:ASPxButton>
                    </div>
                </div>
                <div id="Container2" class="container">
                    <div id="C2_Row0" class="Row">
                        <div id="C2_Row0_Col1" class="LFloat_Content">
                            <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="320px" TabIndex="0"></asp:TextBox>
                        </div>
                        <div id="C2_Row0_Col2" class="LFloat_Lable">
                            <a href="javascript:void(0);" tabindex="0" onclick="btnAddToList_click()"><span class="lnkBtnAjax green">
                                Add to List</span></a>
                        </div>
                    </div>
                    <div id="C2_Row1" class="Row">
                        <div id="C2_Row1_Col1" class="LFloat_Content finalSelectedBox">
                            <asp:ListBox ID="lstSelection" runat="server" Font-Size="12px" Height="100px" Width="400px"
                                TabIndex="0"></asp:ListBox>
                        </div>
                    </div>
                    <div id="C2_Row2" class="Row">
                        <div id="C2_Row2_Col1" class="LFloat_Lable">
                            <a href="javascript:void(0);" tabindex="0" onclick="lnkBtnAddFinalSelection()"><span
                                class="lnkBtnAjax blue">Done</span></a>&nbsp;&nbsp; <a href="javascript:void(0);"
                                    tabindex="0" onclick="lnkBtnRemoveFromSelection()"><span class="lnkBtnAjax red">Remove</span></a>
                        </div>
                    </div>
                </div>
            </div>
            <div id="Row1" class="Row">
                <div class="Box_Detail">
                    <asp:GridView ID="gvwCMBillSummary" AutoGenerateColumns="false" Width="100%" runat="server"
                        DataKeyNames="AutoID" TabIndex="0" ShowFooter="false" CellPadding="2" OnRowDataBound="gvwCMBillSummary_RowDataBound">
                        <HeaderStyle CssClass="cellHeader" />
                        <Columns>
                            <asp:TemplateField HeaderText="Srl" Visible="false">
                                <ItemTemplate>
                                    <%#Eval("AutoID")%>
                                </ItemTemplate>
                                <ItemStyle CssClass="cellRecord" Wrap="false" />
                                <HeaderStyle CssClass="cellHeader" Width="40px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Client" ControlStyle-CssClass="client">
                                <ItemTemplate>
                                    <a href="javascript:void(0);" title="<%#Eval("Client")%>">
                                        <%#(string)ConvertToShortString((string)(String.IsNullOrEmpty(Eval("Client").ToString()) ? "&nbsp;" : Eval("Client")))%>
                                    </a>
                                </ItemTemplate>
                                <ItemStyle CssClass="cellRecord" Wrap="false" />
                                <HeaderStyle CssClass="cellHeader" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="SettNum" ControlStyle-CssClass="settNum">
                                <ItemTemplate>
                                    <%#Eval("SettNum")%>
                                </ItemTemplate>
                                <ItemStyle CssClass="cellRecord" Wrap="false" />
                                <HeaderStyle CssClass="cellHeader" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="BillDate" ControlStyle-CssClass="billDate">
                                <ItemTemplate>
                                    <%#Eval("BillDate")%>
                                </ItemTemplate>
                                <ItemStyle CssClass="cellRecord" Wrap="false" />
                                <HeaderStyle CssClass="cellHeader" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="NetObligation" HeaderText="NetObligation" />
                            <%-- <asp:TemplateField HeaderText="NetObligation" ControlStyle-CssClass="netObligation">
                                <ItemTemplate>
                                    <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty((Eval("NetObligation")).ToString()) ? "" : Eval("NetObligation")))%>
                                </ItemTemplate>
                                <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                                <HeaderStyle CssClass="cellHeader" />
                            </asp:TemplateField>--%>
                            <asp:BoundField DataField="STT" HeaderText="STT" />
                            <%--<asp:TemplateField HeaderText="STT" ControlStyle-CssClass="sTT">
                                <ItemTemplate>
                                    <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("STT").ToString()) ? "" : Eval("STT")))%>
                                </ItemTemplate>
                                <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                                <HeaderStyle CssClass="cellHeader" />
                            </asp:TemplateField>--%>
                            <asp:BoundField DataField="TranCharge" HeaderText="TranCharge" />
                            <%--<asp:TemplateField HeaderText="TranCharge" ControlStyle-CssClass="tranCharge">
                                <ItemTemplate>
                                    <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("TranCharge").ToString()) ? "" : Eval("TranCharge")))%>
                                </ItemTemplate>
                                <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                                <HeaderStyle CssClass="cellHeader" />
                            </asp:TemplateField>--%>
                            <asp:BoundField DataField="StampDuty" HeaderText="StampDuty" />
                            <%--<asp:TemplateField HeaderText="StampDuty" ControlStyle-CssClass="stampDuty">
                                <ItemTemplate>
                                    <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("StampDuty").ToString()) ? "" : Eval("StampDuty")))%>
                                </ItemTemplate>
                                <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                                <HeaderStyle CssClass="cellHeader" />
                            </asp:TemplateField>--%>
                            <asp:BoundField DataField="DlvCharge" HeaderText="DlvCharge" />
                            <%-- <asp:TemplateField HeaderText="DlvCharge" ControlStyle-CssClass="dlvCharge">
                                <ItemTemplate>
                                    <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("DlvCharge").ToString()) ? "" : Eval("DlvCharge")))%>
                                </ItemTemplate>
                                <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                                <HeaderStyle CssClass="cellHeader" />
                            </asp:TemplateField>--%>
                            <asp:BoundField DataField="SebiFee" HeaderText="SebiFee" />
                            <%--<asp:TemplateField HeaderText="SebiFee" ControlStyle-CssClass="sebiFee">
                                <ItemTemplate>
                                    <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("SebiFee").ToString()) ? "" : Eval("SebiFee")))%>
                                </ItemTemplate>
                                <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                                <HeaderStyle CssClass="cellHeader" />
                            </asp:TemplateField>--%>
                            <asp:BoundField DataField="OtherCharge" HeaderText="OtherCharge" />
                            <%-- <asp:TemplateField HeaderText="OtherCharge" ControlStyle-CssClass="otherCharge">
                                <ItemTemplate>
                                    <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("OtherCharge").ToString()) ? "" : Eval("OtherCharge")))%>
                                </ItemTemplate>
                                <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                                <HeaderStyle CssClass="cellHeader" />
                            </asp:TemplateField>--%>
                            <asp:BoundField DataField="ServTax" HeaderText="ServTax" />
                            <%--<asp:TemplateField HeaderText="ServTax" ControlStyle-CssClass="servTax">
                                <ItemTemplate>
                                    <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("ServTax").ToString()) ? "" : Eval("ServTax")))%>
                                </ItemTemplate>
                                <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                                <HeaderStyle CssClass="cellHeader" />
                            </asp:TemplateField>--%>
                            <asp:BoundField DataField="NetBillAmount" HeaderText="NetBillAmount" />
                            <%-- <asp:TemplateField HeaderText="NetBillAmount" ControlStyle-CssClass="netBillAmount">
                                <ItemTemplate>
                                    <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("NetBillAmount").ToString()) ? "" : Eval("NetBillAmount")))%>
                                </ItemTemplate>
                                <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                                <HeaderStyle CssClass="cellHeader" />
                            </asp:TemplateField>--%>
                            <asp:BoundField DataField="Turnover" HeaderText="Turnover" />
                            <%--<asp:TemplateField HeaderText="Turnover" ControlStyle-CssClass="turnover">
                                <ItemTemplate>
                                    <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("Turnover").ToString()) ? "" : Eval("Turnover")))%>
                                </ItemTemplate>
                                <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                                <HeaderStyle CssClass="cellHeader" />
                            </asp:TemplateField>--%>
                            <asp:BoundField DataField="TotalBrokerage" HeaderText="TotalBrokerage" />
                            <%--<asp:TemplateField HeaderText="TotalBrokerage" ControlStyle-CssClass="totalBrokerage">
                                <ItemTemplate>
                                    <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("TotalBrokerage").ToString()) ? "" : Eval("TotalBrokerage")))%>
                                </ItemTemplate>
                                <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                                <HeaderStyle CssClass="cellHeader" />
                            </asp:TemplateField>--%>
                            <asp:BoundField DataField="UnrealizedServTax" HeaderText="UnrealizedServTax" />
                            <%--<asp:TemplateField HeaderText="UnrealizedServTax" ControlStyle-CssClass="unrealizedServTax">
                                <ItemTemplate>
                                    <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("UnrealizedServTax").ToString()) ? "" :Eval("UnrealizedServTax")))%>
                                </ItemTemplate>
                                <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                                <HeaderStyle CssClass="cellHeader" />
                            </asp:TemplateField>--%>
                            <asp:BoundField DataField="UnrealizedSTT" HeaderText="UnrealizedSTT" />
                            <%--<asp:TemplateField HeaderText="UnrealizedSTT" ControlStyle-CssClass="unrealizedSTT">
                                <ItemTemplate>
                                    <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("UnrealizedSTT").ToString()) ? "" :Eval("UnrealizedSTT")))%>
                                </ItemTemplate>
                                <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                                <HeaderStyle CssClass="cellHeader" />
                            </asp:TemplateField>--%>
                        </Columns>
                    </asp:GridView>
                    <span class="clear"></span>
                </div>
            </div>
            <br class="clear" />
            <center>
                <div id="ShowMore" class="loaderShowMore">
                    <a href="javascript:void(0);" id="BtnShowMore" onclick="btnShowMOre_click()" title="Click To Show More Records"
                        class="buttonMore">Click To Show More Record(s)</a>
                </div>
                <br class="clear" style="height: 0px" />
                <div id="loader" class="loading">
                    <img alt="Loading.." title="See More Record.." src="../images/endlessLoading.gif"
                        width="32px" height="100px" />
                </div>
                <br class="clear" style="height: 0px" />
            </center>
            <br class="clear" style="height: 0px" />
            <div style="display: none">
                <asp:TextBox ID="txtSelectionID_hidden" runat="server"></asp:TextBox>
                <asp:HiddenField ID="HiddenField_ClientBranchGroup" runat="server" />
                <asp:HiddenField ID="HDNFilterCol" runat="server" />
            </div>
        </div>
    </form>
</body>
</html>
