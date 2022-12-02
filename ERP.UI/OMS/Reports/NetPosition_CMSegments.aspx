<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_NetPosition_CMSegments" EnableEventValidation="false" CodeBehind="NetPosition_CMSegments.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.v10.2.Export" Namespace="DevExpress.Web.Export"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxCallbackPanel"
    TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>--%>
    <link type="text/css" href="../CSS/GenericCss.css" rel="Stylesheet" />
    <!--External Scripts file-->
    <!-- Ajax List Requierd-->
    <link type="text/css" href="../CSS/AjaxStyle.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/ajaxList_rootfile.js"></script>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <!--Other Script-->

    <script type="text/javascript" src="/assests/js/GenericJScript.js"></script>

    <!--JS Inline Method-->
    <!--Page and Filter Script -->

    <script type="text/javascript" language="javascript">
        function Page_Load()
        { 
            HideShow('btnOpen','H');      
            HideShow('btnClose','H');       
            HideShow('divTotalCMNetPosition','H');                     
            HideShow('divShowFilter','H');
            HideShow('HideFilterColumn','H');
            HideShow('Container2','H'); 
            HideShow('Row1','H');
            HideShow('ShowMore','H');
            HideShow('loader','H'); 
            HideShow('Row2','H');
            Height('200','200');                              
        }      
        function SignOff()
        {
            window.parent.SignOff();
        }
        function Reset()
        {       
            cRblSegment.SetSelectedIndex(0);     
            cRblProduct.SetSelectedIndex(0);     
            cCmbGroupBy.SetSelectedIndex(0); 
            cRblBranch.SetSelectedIndex(0);
            cRblClient.SetSelectedIndex(0);
            cRblGroup.SetSelectedIndex(0);
            HideShow('C1_Row2_Col4','H');
            HideShow('C1_Row2_Col5','H');
            HideShow('C1_Row2_Col3','S');
            SetValue('HDNSegment','');
            SetValue('HDNProduct','');
            SetValue('HDNBranch','');
            SetValue('HDNGroup','');   
            SetValue('HDNClient','');            
            Height('200','200');
            //alert('Reset1');                         
        }
        function CompareFromDate()
        {
            var setFromDate='<%=currentFromDate%>'; 
            CompareDate(cDtFrom.GetDate(),cDtTo.GetDate(),'LE','From Date Can Not Be Greater Than To Date',cDtFrom,setFromDate);
        }  
        function CompareToDate()
        {
            var setToDate='<%=currentToDate%>'; 
            CompareDate(cDtFrom.GetDate(),cDtTo.GetDate(),'LE','To Date Can Not Be Less Than From Date',cDtTo,setToDate);
        }
        //        function DateChange(positionDate)
        //        {
        //            var FYS='<%=Session["FinYearStart"]%>';
        //            var FYE='<%=Session["FinYearEnd"]%>';
        //            var LFY='<%=Session["LastFinYear"]%>';
        //            alert("DateChange--"+positionDate+"~"+FYS+"~"+FYE+"~"+LFY);
        //            DevE_CheckForFinYear(positionDate,FYS,FYE,LFY);
        //            alert('Fin Yr Test');
        //        }     
        function fn_Segment(obj)
        {  
            if(obj=="A")
            {
                HideShow('Container2','H');
                HideShow('C1_Row6','S');
            }
            else if(obj=="S")
            {               
                if(GetObjectID('Container2').style.display=="inline")
                {                
                    cRblSegment.SetSelectedIndex(0);     
                    lnkBtnAddFinalSelection();                        	
                }
                else
                {
                    HideShow('C1_Row6','H');
                    HideShow('Container2','S');
                    CallServer("CallAjax-Segment",""); 
                    //GetObjectID('txtSelectionID').focus();
                } 
            }                         
        }      
        function fn_Product(obj)
        {  
            if(obj=="A")
            {
                HideShow('Container2','H');
                HideShow('C1_Row6','S');                
            }
            else if(obj=="S")
            {               
                if(GetObjectID('Container2').style.display=="inline")
                {                
                    cRblProduct.SetSelectedIndex(0);     
                    lnkBtnAddFinalSelection();                        	
                }
                else
                {
                    HideShow('Container2','S');
                    HideShow('C1_Row6','H');
                    CallServer("CallAjax-Product","");
                }  
            }                         
        }      
        function fn_GroupBy(obj)
        {
            GetObjectID('<%=lstSelection.ClientID%>').length=0;
            if(obj=="B")
            {
                cRblBranch.SetSelectedIndex(0);     
                fn_Branch('A'); 
                HideShow('C1_Row2_Col4','H');  
                HideShow('C1_Row2_Col5','H');               
                HideShow('C1_Row2_Col3','S');               
                CallServer("CallAjax-Branch",""); 
            }           
            if(obj=="G")                
            {  
                HideShow('C1_Row2_Col3','H');
                HideShow('C1_Row2_Col4','H');  
                HideShow('Container2','H');
                HideShow('C1_Row2_Col5','S');
                HideShow('C1_Row2_Col6','H');              
                GetObjectID('btnhide').click();            
            }            
            if(obj=="C")
            { 
                cRblClient.SetSelectedIndex(0);     
                fn_Client('A');                
                HideShow('C1_Row2_Col5','H');
                HideShow('C1_Row2_Col3','H'); 
                HideShow('C1_Row2_Col4','S');
                CallServer("CallAjax-Client","");              
            }
        }      
        function fn_Branch(obj)
        {  
            if(obj=="A")
            {
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
        function fn_Group(obj)
        {  
            if(obj=="A")
            {
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
                    CallServer("CallAjax-Group",""); 
                    //GetObjectID('txtSelectionID').focus();
                } 
            }                         
        }
        function fn_GroupType(obj)
        {
            if(obj=="0")
            {
                HideShow('C1_Row2_Col6','H');
                alert('Please Select Group Type !');
            }
            else
            {
                cRblGroup.SetSelectedIndex(0);     
                HideShow('C1_Row2_Col6','S');
            }
        }      
        function fn_Client(obj)
        {  
            if(obj=="A")
            {
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
        function fn_ReportBy(obj)
        {
            if(obj=="S")
            {
                cbtnShow.SetText("Show Result");  
            }
            else if(obj=="E")
            {
                cbtnShow.SetText("Export");
            }              
        }
        function fn_ShowFilterColumn()
        {
            HideShow('ExpandFilterColumn','H'); 
            HideShow('C1_Row4_Col2','H'); 
            HideShow('HideFilterColumn','S');
            HideShow('C1_Row4_Col3','S'); 
            Height('200','200');        
        }
        function fn_HideFilterColumn()
        {
            GetObjectID('HideFilterColumn').style.display='none';
            HideShow('ExpandFilterColumn','S');
            HideShow('C1_Row4_Col3','H'); 
            HideShow('C1_Row4_Col2','S');
            Height('200','200');                  
        }
        function fn_SearchFilter_Hide()
        {
            HideShow('Row0','H');
            HideShow('divShowFilter','S'); 
            HideShow('btnClose','H');       
            HideShow('divTotalCMNetPosition','H'); 
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
            Height('200','200');                 
        }        
        function NORECORD()
        {      
            HideShow('Container2','H');
            HideShow('C1_Row6','S'); 
            Page_Load(); 
            Reset();          
            alert('No Record Found !! ');            
        }
        function BtnShowFilter_Click()
        {        
            window.location = "../reports/NetPosition_CMSegments.aspx";
        } 
        function ListValidate(ele)
        {
            if(ele=="1")
                alert("Please Select An Item Then Add!!!");
            if(ele=="2")
                alert("Please Select An Item Then Remove!!!");    
        }
        function btnCloseTotal_click()
        {
            HideShow('btnClose','H');
            HideShow('divTotalCMNetPosition','H');
            HideShow('btnOpen','S');
        } 
        function btnOpenTotal_click()
        {
            HideShow('btnOpen','H');
            HideShow('btnClose','S');
            HideShow('divTotalCMNetPosition','S');
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
                if(cRblSegment.GetSelectedIndex()==1)
                {
                    alert("Please Select Atleast One Segment Item!!!"); 
                }
                else if(cRblProduct.GetSelectedIndex()==1)
                {             
                    alert("Please Select Atleast One Product Item!!!");
                }
                else if((cCmbGroupBy.GetSelectedIndex()==0) && (cRblBranch.GetSelectedIndex()==1) && (cRblSegment.GetSelectedIndex()==0) && (cRblProduct.GetSelectedIndex()==0))
                {             
                    alert("Please Select Atleast One Branch Item!!!");
                }
                else if((cCmbGroupBy.GetSelectedIndex()==1) && (cRblGroup.GetSelectedIndex()==1) && (cRblSegment.GetSelectedIndex()==0) && (cRblProduct.GetSelectedIndex()==0))
                {             
                    alert("Please Select Atleast One Group Item!!!");
                }
                else if((cCmbGroupBy.GetSelectedIndex()==2) && (cRblClient.GetSelectedIndex()==1) && (cRblSegment.GetSelectedIndex()==0) && (cRblProduct.GetSelectedIndex()==0))
                {             
                    alert("Please Select Atleast One Client Type Item!!!");
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
        function ReceiveServerData(rValue)
        {            
            var Data=rValue.split('@');           
            if(Data[1]!="undefined")
            {
                if(Data[0]=='Segment')
                {                   
                    SetValue('HDNSegment',Data[1]);                   
                }
                else if(Data[0]=='Product')
                { 
                    SetValue('HDNProduct',Data[1]);
                }
                else if(Data[0]=='Branch')
                {
                    SetValue('HDNBranch',Data[1]);
                }
                else if(Data[0]=='Group')
                { 
                    SetValue('HDNGroup',Data[1]);
                }
                else if(Data[0]=='Client')
                { 
                    SetValue('HDNClient',Data[1]);
                }               
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
            ajax_showOptions(AjaxList_TextBox,'GenericAjaxList',e,replaceChars(AjaxComQuery),'Main');
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
                    url: "NetPosition_CMSegments.aspx/GetCMNetPositions",
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
            var CMNetPositions = xml.find("Table");                  
            CMNetPositions.each(function () 
            {
                var i=0;
                var filterData=GetObjectID('HDNFilterCol').value.split('~');  
                var reportType=GetObjectID('HDNReportType').value;                                              
                var cMNetPosition = $(this);
                var row = $("[id*=gvwNetPosition] tr:last-child").clone(true);                    
                $("td", row).eq(0).html((cMNetPosition.find("Client").text()).length==0?'&nbsp;':(cMNetPosition.find("Client").text()).search("\\*") != -1 ? '<a href="javascript:void(0);" title="'+cMNetPosition.find("Client").text()+'"><b>'+(cMNetPosition.find("Client").text()).substring(0,25) +'</b></a>' : '<a href="javascript:void(0);" title="'+cMNetPosition.find("Client").text()+'">'+(cMNetPosition.find("Client").text()).substring(0,25)+'</a>');
                $("td", row).eq(1).html((cMNetPosition.find("Segment").text()).length==0?'&nbsp;':cMNetPosition.find("Segment").text());
                if(reportType=="D")
                {
                    $("td", row).eq(2).html((cMNetPosition.find("Date").text()).length==0?'&nbsp;':cMNetPosition.find("Date").text());
                    $("td", row).eq(3).html((cMNetPosition.find("SettNum").text()).length==0?'&nbsp;':cMNetPosition.find("SettNum").text());
                    $("td", row).eq(4).html((cMNetPosition.find("Symbol").text()).length==0?'&nbsp;':cMNetPosition.find("Symbol").text());
                    i=5;
                }
                else
                {
                    $("td", row).eq(2).html((cMNetPosition.find("Symbol").text()).length==0?'&nbsp;':cMNetPosition.find("Symbol").text());
                    i=3;
                }
                if(filterData[0]=='N')
                {                                
                    $("td", row).eq(i).html('&nbsp;');                     
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("ISIN").text()).length==0?'&nbsp;':cMNetPosition.find("ISIN").text());
                    i=i+1;                                
                }
                if(filterData[1]=='N')
                {                                
                    $("td", row).eq(i).html('&nbsp;');                           
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("Pur").text()).length==0?'&nbsp;':(cMNetPosition.find("Pur").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("Pur").text()).substr(1,((cMNetPosition.find("Pur").text()).length-1))+')</span>' : cMNetPosition.find("Pur").text());
                    i=i+1;                                
                }
                            
                if(filterData[2]=='N')
                {                                
                    $("td", row).eq(i).html('&nbsp;');                           
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("PurMkt").text()).length==0?'&nbsp;':(cMNetPosition.find("PurMkt").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("PurMkt").text()).substr(1,((cMNetPosition.find("PurMkt").text()).length-1))+')</span>' : cMNetPosition.find("PurMkt").text());
                    i=i+1;                                
                }
                if(filterData[3]=='N')
                {                                
                    $("td", row).eq(i).html('&nbsp;');                          
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("PurAvg").text()).length==0?'&nbsp;':(cMNetPosition.find("PurAvg").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("PurAvg").text()).substr(1,((cMNetPosition.find("PurAvg").text()).length-1))+')</span>' : cMNetPosition.find("PurAvg").text());
                    i=i+1;                                
                }
                if(filterData[4]=='N')
                {                                
                    $("td", row).eq(i).html('&nbsp;');                            
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("PurNet").text()).length==0?'&nbsp;':(cMNetPosition.find("PurNet").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("PurNet").text()).substr(1,((cMNetPosition.find("PurNet").text()).length-1))+')</span>' : cMNetPosition.find("PurNet").text());
                    i=i+1;
                }
                if(filterData[5]=='N')
                {                                
                    $("td", row).eq(i).html('&nbsp;');                 
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("PurNetAvg").text()).length==0?'&nbsp;':(cMNetPosition.find("PurNetAvg").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("PurNetAvg").text()).substr(1,((cMNetPosition.find("PurNetAvg").text()).length-1))+')</span>' : cMNetPosition.find("PurNetAvg").text());
                    i=i+1;
                }
                if(filterData[6]=='N')
                {                                
                    $("td", row).eq(i).html('&nbsp;');                     
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("Sale").text()).length==0?'&nbsp;':(cMNetPosition.find("Sale").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("Sale").text()).substr(1,((cMNetPosition.find("Sale").text()).length-1))+')</span>' : cMNetPosition.find("Sale").text());
                    i=i+1;
                }
                if(filterData[7]=='N')
                {                                
                    $("td", row).eq(i).html('&nbsp;');                        
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("SaleMkt").text()).length==0?'&nbsp;':(cMNetPosition.find("SaleMkt").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("SaleMkt").text()).substr(1,((cMNetPosition.find("SaleMkt").text()).length-1))+')</span>' : cMNetPosition.find("SaleMkt").text());
                    i=i+1;
                }
                if(filterData[8]=='N')
                {                                
                    $("td", row).eq(i).html('&nbsp;');                         
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("SaleAvg").text()).length==0?'&nbsp;':(cMNetPosition.find("SaleAvg").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("SaleAvg").text()).substr(1,((cMNetPosition.find("SaleAvg").text()).length-1))+')</span>' : cMNetPosition.find("SaleAvg").text());
                    i=i+1;
                }
                if(filterData[9]=='N')
                {                                
                    $("td", row).eq(i).html('&nbsp;');                     
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("SaleNet").text()).length==0?'&nbsp;':(cMNetPosition.find("SaleNet").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("SaleNet").text()).substr(1,((cMNetPosition.find("SaleNet").text()).length-1))+')</span>' : cMNetPosition.find("SaleNet").text());
                    i=i+1;
                }
                if(filterData[10]=='N')
                {                                
                    $("td", row).eq(i).html('&nbsp;');                       
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("SaleNetAvg").text()).length==0?'&nbsp;':(cMNetPosition.find("SaleNetAvg").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("SaleNetAvg").text()).substr(1,((cMNetPosition.find("SaleNetAvg").text()).length-1))+')</span>' : cMNetPosition.find("SaleNetAvg").text());
                    i=i+1;
                }
                if(filterData[11]=='N')
                {                                
                    $("td", row).eq(i).html('&nbsp;');                       
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("SqrQty").text()).length==0?'&nbsp;':(cMNetPosition.find("SqrQty").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("SqrQty").text()).substr(1,((cMNetPosition.find("SqrQty").text()).length-1))+')</span>' : cMNetPosition.find("SqrQty").text());
                    i=i+1;
                }
                if(filterData[12]=='N')
                {                                
                    $("td", row).eq(i).html('&nbsp;');                           
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("SqrPL").text()).length==0?'&nbsp;':(cMNetPosition.find("SqrPL").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("SqrPL").text()).substr(1,((cMNetPosition.find("SqrPL").text()).length-1))+')</span>' : cMNetPosition.find("SqrPL").text());
                    i=i+1;
                }
                if(filterData[13]=='N')
                {                                
                    $("td", row).eq(i).html('&nbsp;');                            
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("DlvPur").text()).length==0?'&nbsp;':(cMNetPosition.find("DlvPur").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("DlvPur").text()).substr(1,((cMNetPosition.find("DlvPur").text()).length-1))+')</span>' : cMNetPosition.find("DlvPur").text());
                    i=i+1;
                }
                if(filterData[14]=='N')
                {                                
                    $("td", row).eq(i).html('&nbsp;');                   
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("DlvPurMkt").text()).length==0?'&nbsp;':(cMNetPosition.find("DlvPurMkt").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("DlvPurMkt").text()).substr(1,((cMNetPosition.find("DlvPurMkt").text()).length-1))+')</span>' : cMNetPosition.find("DlvPurMkt").text());
                    i=i+1;
                }
                if(filterData[15]=='N')
                {                                
                    $("td", row).eq(i).html('&nbsp;');                        
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("DlvPurAvg").text()).length==0?'&nbsp;':(cMNetPosition.find("DlvPurAvg").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("DlvPurAvg").text()).substr(1,((cMNetPosition.find("DlvPurAvg").text()).length-1))+')</span>' : cMNetPosition.find("DlvPurAvg").text());
                    i=i+1;
                }
                if(filterData[16]=='N')
                {                                
                    $("td", row).eq(i).html('&nbsp;');                  
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("DlvPurNet").text()).length==0?'&nbsp;':(cMNetPosition.find("DlvPurNet").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("DlvPurNet").text()).substr(1,((cMNetPosition.find("DlvPurNet").text()).length-1))+')</span>' : cMNetPosition.find("DlvPurNet").text());
                    i=i+1;
                }
                if(filterData[17]=='N')
                {                               
                    $("td", row).eq(i).html('&nbsp;');                        
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("DlvPurNetAvg").text()).length==0?'&nbsp;':(cMNetPosition.find("DlvPurNetAvg").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("DlvPurNetAvg").text()).substr(1,((cMNetPosition.find("DlvPurNetAvg").text()).length-1))+')</span>' : cMNetPosition.find("DlvPurNetAvg").text());
                    i=i+1;
                }
                if(filterData[18]=='N')
                {
                    $("td", row).eq(i).html('&nbsp;');                  
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("DlvSale").text()).length==0?'&nbsp;':(cMNetPosition.find("DlvSale").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("DlvSale").text()).substr(1,((cMNetPosition.find("DlvSale").text()).length-1))+')</span>' : cMNetPosition.find("DlvSale").text());
                    i=i+1;
                }
                if(filterData[19]=='N')
                {
                    $("td", row).eq(i).html('&nbsp;');                         
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("DlvSaleMkt").text()).length==0?'&nbsp;':(cMNetPosition.find("DlvSaleMkt").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("DlvSaleMkt").text()).substr(1,((cMNetPosition.find("DlvSaleMkt").text()).length-1))+')</span>' : cMNetPosition.find("DlvSaleMkt").text());
                    i=i+1;
                }
                if(filterData[20]=='N')
                {
                    $("td", row).eq(i).html('&nbsp;');                     
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("DlvSaleAvg").text()).length==0?'&nbsp;':(cMNetPosition.find("DlvSaleAvg").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("DlvSaleAvg").text()).substr(1,((cMNetPosition.find("DlvSaleAvg").text()).length-1))+')</span>' : cMNetPosition.find("DlvSaleAvg").text());
                    i=i+1;
                }
                if(filterData[21]=='N')
                {
                    $("td", row).eq(i).html('&nbsp;');                       
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("DlvSaleNet").text()).length==0?'&nbsp;':(cMNetPosition.find("DlvSaleNet").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("DlvSaleNet").text()).substr(1,((cMNetPosition.find("DlvSaleNet").text()).length-1))+')</span>' : cMNetPosition.find("DlvSaleNet").text());
                    i=i+1;
                }
                if(filterData[22]=='N')
                {
                    $("td", row).eq(i).html('&nbsp;');                    
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("DlvSaleNetAvg").text()).length==0?'&nbsp;':(cMNetPosition.find("DlvSaleNetAvg").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("DlvSaleNetAvg").text()).substr(1,((cMNetPosition.find("DlvSaleNetAvg").text()).length-1))+')</span>' : cMNetPosition.find("DlvSaleNetAvg").text());
                    i=i+1;
                }
                if(filterData[23]=='N')
                {
                    $("td", row).eq(i).html('&nbsp;');                          
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("NetDlv").text()).length==0?'&nbsp;':(cMNetPosition.find("NetDlv").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("NetDlv").text()).substr(1,((cMNetPosition.find("NetDlv").text()).length-1))+')</span>' : cMNetPosition.find("NetDlv").text());
                    i=i+1;
                }
                if(filterData[24]=='N')
                {
                    $("td", row).eq(i).html('&nbsp;');                    
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("ClPrice").text()).length==0?'&nbsp;':(cMNetPosition.find("ClPrice").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("ClPrice").text()).substr(1,((cMNetPosition.find("ClPrice").text()).length-1))+')</span>' : cMNetPosition.find("ClPrice").text());
                    i=i+1;
                }
                if(filterData[25]=='N')
                {
                    $("td", row).eq(i).html('&nbsp;');                     
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("MtmPL").text()).length==0?'&nbsp;':(cMNetPosition.find("MtmPL").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("MtmPL").text()).substr(1,((cMNetPosition.find("MtmPL").text()).length-1))+')</span>' : cMNetPosition.find("MtmPL").text());
                    i=i+1;
                }
                if(filterData[26]=='N')
                {
                    $("td", row).eq(i).html('&nbsp;');                      
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("TotalPL").text()).length==0?'&nbsp;':(cMNetPosition.find("TotalPL").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("TotalPL").text()).substr(1,((cMNetPosition.find("TotalPL").text()).length-1))+')</span>' : cMNetPosition.find("TotalPL").text());
                    i=i+1;
                }
                if(filterData[27]=='N')
                {
                    $("td", row).eq(i).html('&nbsp;');                 
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("Exposure").text()).length==0?'&nbsp;':(cMNetPosition.find("Exposure").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("Exposure").text()).substr(1,((cMNetPosition.find("Exposure").text()).length-1))+')</span>' : cMNetPosition.find("Exposure").text());
                    i=i+1;
                }
                if(filterData[28]=='N')
                {
                    $("td", row).eq(i).html('&nbsp;');                           
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("DlvTurnover").text()).length==0?'&nbsp;':(cMNetPosition.find("DlvTurnover").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("DlvTurnover").text()).substr(1,((cMNetPosition.find("DlvTurnover").text()).length-1))+')</span>' : cMNetPosition.find("DlvTurnover").text());
                    i=i+1;
                }
                if(filterData[29]=='N')
                {
                    $("td", row).eq(i).html('&nbsp;');                                         
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("DlvBrokerage").text()).length==0?'&nbsp;':(cMNetPosition.find("DlvBrokerage").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("DlvBrokerage").text()).substr(1,((cMNetPosition.find("DlvBrokerage").text()).length-1))+')</span>' : cMNetPosition.find("DlvBrokerage").text());                               
                    i=i+1;
                }
                if(filterData[30]=='N')
                {
                    $("td", row).eq(i).html('&nbsp;');
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("SqrTurnover").text()).length==0?'&nbsp;':(cMNetPosition.find("SqrTurnover").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("SqrTurnover").text()).substr(1,((cMNetPosition.find("SqrTurnover").text()).length-1))+')</span>' : cMNetPosition.find("SqrTurnover").text());
                    i=i+1;
                }
                if(filterData[31]=='N')
                {
                    $("td", row).eq(i).html('&nbsp;');                                   
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("SqrBrokerage").text()).length==0?'&nbsp;':(cMNetPosition.find("SqrBrokerage").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("SqrBrokerage").text()).substr(1,((cMNetPosition.find("SqrBrokerage").text()).length-1))+')</span>' : cMNetPosition.find("SqrBrokerage").text());
                    i=i+1;                                 
                }
                if(filterData[32]=='N')//37--TurnOver
                {
                    $("td", row).eq(i).html('&nbsp;');                               
                }
                else 
                {                              
                    $("td", row).eq(i).html((cMNetPosition.find("Turnover").text()).length==0?'&nbsp;':(cMNetPosition.find("Turnover").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("Turnover").text()).substr(1,((cMNetPosition.find("Turnover").text()).length-1))+')</span>' : cMNetPosition.find("Turnover").text());
                    i=i+1;
                }                                
                if(filterData[33]=='N')//[38(Total Brokerage) out of 39 Col] / [With AutoID -(0- 40)]
                {                                
                    $("td", row).eq(i).html('&nbsp;');      
                }
                else 
                {    
                    $("td", row).eq(i).html((cMNetPosition.find("TotalBrokerage").text()).length==0?'&nbsp;':(cMNetPosition.find("TotalBrokerage").text()).search("-") != -1 ? '<span style="color:red">('+(cMNetPosition.find("TotalBrokerage").text()).substr(1,((cMNetPosition.find("TotalBrokerage").text()).length-1))+')</span>' : cMNetPosition.find("TotalBrokerage").text());                                                              
                }
                $("[id*=gvwNetPosition]").append(row);                                                                                            
            });                    
            $("#loader").hide();                                                   
            $("#ShowMore").show();
            Height('200','200');        
        }  
    </script>

    <!--  For Floating Div with snap-->

    <script type="text/javascript" src="/assests/js/floating-1.12.js"></script>

    <script type="text/javascript">  
        floatingMenu.add('divTotalCMNetPosition',  
            {  
                // Represents distance from left or right browser window  
                // border depending upon property used. Only one should be  
                // specified.  
                targetLeft: 6,  
                //targetRight: 10,  
  
                // Represents distance from top or bottom browser window  
                // border depending upon property used. Only one should be  
                // specified.  
                //targetTop: 30,  
                targetBottom: 123,  
  
                // Uncomment one of those if you need centering on  
                // X- or Y- axis.  
                // centerX: true,  
                // centerY: true,  
  
                // Remove this one if you don't want snap effect  
                snap: true  
            }); 
        floatingMenu.add('btnClose',targetLeft: 20, targetBottom: 190,snap: true});      
        floatingMenu.add('btnOpen',targetLeft: 20, targetBottom: 190,snap: true});   
    </script>

    <!--Page Style-->
    <style type="text/css">
        #MainFull {
            padding: 5px;
        }

        #Container1 {
            width: 520px;
        }

        #Container2 {
            width: 420px;
            display: none;
        }

        .LableWidth {
            width: 110px;
        }

        .ContentWidth {
            width: 170px;
            height: 21px;
        }

        .labelCont {
            font-size: 13px;
            margin-top: 7px;
        }

        .btnRight {
            margin-right: 10px;
            float: right;
        }

        .Box_filter {
            width: 170px;
            height: 120px;
        }

        .Box_filterCont {
            width: 169px;
            height: 109px;
        }

        .Box_filter_HeaderCont {
            width: 167px;
            text-align: center;
            color: #000099;
            font-size: 12px;
            border: 1px solid #316AC5;
            background-color: #97B9E9;
        }

        .Box_filter_HeaderNotCont {
            width: 167px;
            text-align: center;
            color: #333;
            font-size: 12px;
            border: 1px solid #aaa;
            background-color: #DDD;
        }

        .Box_filterBtn {
            width: 25px;
            padding-top: 10px;
        }

        .Btn_filterAddAll {
            background: url(../images/pLast.png) no-repeat;
            border: none;
            width: 22px;
            cursor: pointer;
            margin-bottom: 3px;
        }

        .Btn_filterAdd {
            background: url(../images/pNext.png) no-repeat;
            border: none;
            width: 22px;
            cursor: pointer;
            margin-bottom: 3px;
        }

        .Btn_filterRemove {
            background: url(../images/pPrev.png) no-repeat;
            border: none;
            width: 22px;
            cursor: pointer;
            margin-bottom: 3px;
        }

        .Btn_filterRemoveAll {
            background: url(../images/pFirst.png) no-repeat;
            border: none;
            width: 22px;
            cursor: pointer;
            margin-bottom: 3px;
        }

        .loaderShowMore {
            text-align: center;
            color: #000099;
            font-size: 14px;
            border: 1px solid #316AC5;
            background-color: #C1D2EE;
            padding: 10px 5px 10px 5px;
            margin: 5px;
            width: 450px;
        }

        .loading {
            bottom: 5px;
            z-index: 60;
            text-align: center;
            border: 1px solid #316AC5;
            background-color: #C1D2EE;
            padding: 10px 5px 10px 5px;
            margin: 5px;
            width: 460px;
        }

        .txt_left {
            text-align: left;
        }

        .txt_right {
            text-align: right;
        }

        .Box_Detail {
            width: 1100px;
            overflow-x: auto;
            overflow-y: hidden;
            padding-bottom: 18px;
            border: 1px solid #aaa;
        }

        .cellHeader {
            padding: 2px;
            height: 25px;
            border: solid .1pt #aaa;
            font-size: 13px;
            background-color: #DDD;
            font-weight: bold;
            text-align: center;
        }

        .cellRecord {
            padding: 2px;
            border: solid .1pt #aaa;
            font-size: 12px;
            background-color: #eee;
        }

        .Box_TotalRecord {
            margin-left: 5px;
            bottom: 123px;
            z-index: 50;
            position: absolute;
            overflow-x: auto;
            overflow-y: hidden;
            width: 1080px;
            height: 65px;
            padding: 2px 20px 2px 2px;
            background-color: #aaa;
            opacity: 0.9;
            filter: alpha(opacity=90);
        }

        .cellTotHeader {
            padding: 2px;
            height: 22px;
            border: solid .1pt #F4A460;
            font-size: 11px;
            background-color: #DDD;
            font-weight: bold;
            text-align: center;
        }

        .cellTotal {
            padding: 2px;
            border: solid .1pt #F4A460;
            font-size: 11px;
            background-color: #DDD;
            color: maroon;
            font-weight: bold;
            text-align: right;
        }

        .Box_closeTotal {
            margin-left: 20px;
            bottom: 190px;
            z-index: 55;
            position: fixed;
            width: 16px;
            height: 20px;
            text-align: center;
            color: #000;
            font-weight: bold;
            background: #F4A460;
            border: solid 2px #ccc;
            border-bottom: none;
        }

        .buttonMore {
            cursor: pointer;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="btnOpen" class="Box_closeTotal">
        <a href="javascript:void(0);" onclick="btnOpenTotal_click()" title="Maximize Total">+</a>
    </div>
    <div id="btnClose" class="Box_closeTotal">
        <a href="javascript:void(0);" onclick="btnCloseTotal_click()" title="Minimize Total">-</a>
    </div>
    <div id="divTotalCMNetPosition" class="Box_TotalRecord">
        <div id="divTotHeaderCMNetPosition" style="width: 4024px;">
            <div style="width: 100%">
                <div class="left cellTotHeader" style="width: 90px;">
                    &nbsp;
                </div>
                <div id="totHeaderPur" runat="server" class="left cellTotHeader" style="width: 100px;">
                    <b>Tot Pur</b>
                </div>
                <div id="totHeaderPurMkt" runat="server" class="left cellTotHeader" style="width: 110px;">
                    <b>Tot PurMkt</b>
                </div>
                <div id="totHeaderPurAvg" runat="server" class="left cellTotHeader" style="width: 110px;">
                    <b>Tot PurAvg</b>
                </div>
                <div id="totHeaderPurNet" runat="server" class="left cellTotHeader" style="width: 110px;">
                    <b>Tot PurNet</b>
                </div>
                <div id="totHeaderPurNetAvg" runat="server" class="left cellTotHeader" style="width: 110px;">
                    <b>Tot PurNetAvg</b>
                </div>
                <div id="totHeaderSale" runat="server" class="left cellTotHeader" style="width: 110px;">
                    <b>Tot Sale</b>
                </div>
                <div id="totHeaderSaleMkt" runat="server" class="left cellTotHeader" style="width: 110px;">
                    <b>Tot SaleMkt</b>
                </div>
                <div id="totHeaderSaleAvg" runat="server" class="left cellTotHeader" style="width: 110px;">
                    <b>Tot SaleAvg</b>
                </div>
                <div id="totHeaderSaleNet" runat="server" class="left cellTotHeader" style="width: 110px;">
                    <b>Tot SaleNet</b>
                </div>
                <div id="totHeaderSaleNetAvg" runat="server" class="left cellTotHeader" style="width: 110px;">
                    <b>Tot SaleNetAvg</b>
                </div>
                <div id="totHeaderSqrQty" runat="server" class="left cellTotHeader" style="width: 110px;">
                    <b>Tot SqrQty</b>
                </div>
                <div id="totHeaderSqrPL" runat="server" class="left cellTotHeader" style="width: 110px;">
                    <b>Tot SqrPL</b>
                </div>
                <div id="totHeaderDlvPur" runat="server" class="left cellTotHeader" style="width: 110px;">
                    <b>Tot DlvPur</b>
                </div>
                <div id="totHeaderDlvPurMkt" runat="server" class="left cellTotHeader" style="width: 110px;">
                    <b>Tot DlvPurMkt</b>
                </div>
                <div id="totHeaderDlvPurAvg" runat="server" class="left cellTotHeader" style="width: 110px;">
                    <b>Tot DlvPurAvg</b>
                </div>
                <div id="totHeaderDlvPurNet" runat="server" class="left cellTotHeader" style="width: 110px;">
                    <b>Tot DlvPurNet</b>
                </div>
                <div id="totHeaderDlvPurNetAvg" runat="server" class="left cellTotHeader" style="width: 110px;">
                    <b>Tot DlvPurNetAvg</b>
                </div>
                <div id="totHeaderDlvSale" runat="server" class="left cellTotHeader" style="width: 110px;">
                    <b>Tot DlvSale</b>
                </div>
                <div id="totHeaderDlvSaleMkt" runat="server" class="left cellTotHeader" style="width: 110px;">
                    <b>Tot DlvSaleMkt</b>
                </div>
                <div id="totHeaderDlvSaleAvg" runat="server" class="left cellTotHeader" style="width: 110px;">
                    <b>Tot DlvSaleAvg</b>
                </div>
                <div id="totHeaderDlvSaleNet" runat="server" class="left cellTotHeader" style="width: 110px;">
                    <b>Tot DlvSaleNet</b>
                </div>
                <div id="totHeaderDlvSaleNetAvg" runat="server" class="left cellTotHeader" style="width: 110px;">
                    <b>Tot DlvSaleNetAvg</b>
                </div>
                <div id="totHeaderNetDlv" runat="server" class="left cellTotHeader" style="width: 120px;">
                    <b>Tot NetDlv</b>
                </div>
                <div id="totHeaderClPrice" runat="server" class="left cellTotHeader" style="width: 120px;">
                    <b>Tot ClPrice</b>
                </div>
                <div id="totHeaderMtmPL" runat="server" class="left cellTotHeader" style="width: 120px;">
                    <b>Tot MtmPL</b>
                </div>
                <div id="totHeaderTotalPL" runat="server" class="left cellTotHeader" style="width: 120px;">
                    <b>Tot TotalPL</b>
                </div>
                <div id="totHeaderExposure" runat="server" class="left cellTotHeader" style="width: 120px;">
                    <b>Tot Exposure</b>
                </div>
                <div id="totHeaderDlvTurnover" runat="server" class="left cellTotHeader" style="width: 120px;">
                    <b>Tot DlvTurnover</b>
                </div>
                <div id="totHeaderDlvBrokerage" runat="server" class="left cellTotHeader" style="width: 120px;">
                    <b>Tot DlvBrokerage</b>
                </div>
                <div id="totHeaderSqrTurnover" runat="server" class="left cellTotHeader" style="width: 120px;">
                    <b>Tot SqrTurnover</b>
                </div>
                <div id="totHeaderSqrBrokerage" runat="server" class="left cellTotHeader" style="width: 120px;">
                    <b>Tot SqrBrokerage</b>
                </div>
                <div id="totHeaderTurnover" runat="server" class="left cellTotHeader" style="width: 120px;">
                    <b>Tot Turnover</b>
                </div>
                <div id="totHeaderTotalBrokerage" runat="server" class="left cellTotHeader" style="width: 120px;">
                    <b>TotalBrokerage</b>
                </div>
                <span class="clear"></span>
            </div>
            <span class="clear"></span>
            <div style="width: 100%">
                <div class="left cellTotal" style="width: 90px;">
                    <span class="left"><b>Grand Total : </b></span>
                </div>
                <div id="footerTotPur" runat="server" class="left cellTotal" style="width: 100px;">
                    <span class="right">
                        <%=TotPur%>
                    </span>
                </div>
                <div id="footerTotPurMkt" runat="server" class="left cellTotal" style="width: 110px;">
                    <span class="right">
                        <%=TotPurMkt%>
                    </span>
                </div>
                <div id="footerTotPurAvg" runat="server" class="left cellTotal" style="width: 110px;">
                    <span class="right">
                        <%=TotPurAvg%>
                    </span>
                </div>
                <div id="footerTotPurNet" runat="server" class="left cellTotal" style="width: 110px;">
                    <span class="right">
                        <%=TotPurNet%>
                    </span>
                </div>
                <div id="footerTotPurNetAvg" runat="server" class="left cellTotal" style="width: 110px;">
                    <span class="right">
                        <%=TotPurNetAvg%>
                    </span>
                </div>
                <div id="footerTotSale" runat="server" class="left cellTotal" style="width: 110px;">
                    <span class="right">
                        <%=TotSale%>
                    </span>
                </div>
                <div id="footerTotSaleMkt" runat="server" class="left cellTotal" style="width: 110px;">
                    <span class="right">
                        <%=TotSaleMkt%>
                    </span>
                </div>
                <div id="footerTotSaleAvg" runat="server" class="left cellTotal" style="width: 110px;">
                    <span class="right">
                        <%=TotSaleAvg%>
                    </span>
                </div>
                <div id="footerTotSaleNet" runat="server" class="left cellTotal" style="width: 110px;">
                    <span class="right">
                        <%=TotSaleNet%>
                    </span>
                </div>
                <div id="footerTotSaleNetAvg" runat="server" class="left cellTotal" style="width: 110px;">
                    <span class="right">
                        <%=TotSaleNetAvg%>
                    </span>
                </div>
                <div id="footerTotSqrQty" runat="server" class="left cellTotal" style="width: 110px;">
                    <span class="right">
                        <%=TotSqrQty%>
                    </span>
                </div>
                <div id="footerTotSqrPL" runat="server" class="left cellTotal" style="width: 110px;">
                    <span class="right">
                        <%=TotSqrPL%>
                    </span>
                </div>
                <div id="footerTotDlvPur" runat="server" class="left cellTotal" style="width: 110px;">
                    <span class="right">
                        <%=TotDlvPur%>
                    </span>
                </div>
                <div id="footerTotDlvPurMkt" runat="server" class="left cellTotal" style="width: 110px;">
                    <span class="right">
                        <%=TotDlvPurMkt%>
                    </span>
                </div>
                <div id="footerTotDlvPurAvg" runat="server" class="left cellTotal" style="width: 110px;">
                    <span class="right">
                        <%=TotDlvPurAvg%>
                    </span>
                </div>
                <div id="footerTotDlvPurNet" runat="server" class="left cellTotal" style="width: 110px;">
                    <span class="right">
                        <%=TotDlvPurNet%>
                    </span>
                </div>
                <div id="footerTotDlvPurNetAvg" runat="server" class="left cellTotal" style="width: 110px;">
                    <span class="right">
                        <%=TotDlvPurNetAvg%>
                    </span>
                </div>
                <div id="footerTotDlvSale" runat="server" class="left cellTotal" style="width: 110px;">
                    <span class="right">
                        <%=TotDlvSale%>
                    </span>
                </div>
                <div id="footerTotDlvSaleMkt" runat="server" class="left cellTotal" style="width: 110px;">
                    <span class="right">
                        <%=TotDlvSaleMkt%>
                    </span>
                </div>
                <div id="footerTotDlvSaleAvg" runat="server" class="left cellTotal" style="width: 110px;">
                    <span class="right">
                        <%=TotDlvSaleAvg%>
                    </span>
                </div>
                <div id="footerTotDlvSaleNet" runat="server" class="left cellTotal" style="width: 110px;">
                    <span class="right">
                        <%=TotDlvSaleNet%>
                    </span>
                </div>
                <div id="footerTotDlvSaleNetAvg" runat="server" class="left cellTotal" style="width: 110px;">
                    <span class="right">
                        <%=TotDlvSaleNetAvg%>
                    </span>
                </div>
                <div id="footerTotNetDlv" runat="server" class="left cellTotal" style="width: 120px;">
                    <span class="right">
                        <%=TotNetDlv%>
                    </span>
                </div>
                <div id="footerTotClPrice" runat="server" class="left cellTotal" style="width: 120px;">
                    <span class="right">
                        <%=TotClPrice%>
                    </span>
                </div>
                <div id="footerTotMtmPL" runat="server" class="left cellTotal" style="width: 120px;">
                    <span class="right">
                        <%=TotMtmPL%>
                    </span>
                </div>
                <div id="footerTotTotalPL" runat="server" class="left cellTotal" style="width: 120px;">
                    <span class="right">
                        <%=TotTotalPL%>
                    </span>
                </div>
                <div id="footerTotExposure" runat="server" class="left cellTotal" style="width: 120px;">
                    <span class="right">
                        <%=TotExposure%>
                    </span>
                </div>
                <div id="footerTotDlvTurnover" runat="server" class="left cellTotal" style="width: 120px;">
                    <span class="right">
                        <%=TotDlvTurnover%>
                    </span>
                </div>
                <div id="footerTotDlvBrokerage" runat="server" class="left cellTotal" style="width: 120px;">
                    <span class="right">
                        <%=TotDlvBrokerage%>
                    </span>
                </div>
                <div id="footerTotSqrTurnover" runat="server" class="left cellTotal" style="width: 120px;">
                    <span class="right">
                        <%=TotSqrTurnover%>
                    </span>
                </div>
                <div id="footerTotSqrBrokerage" runat="server" class="left cellTotal" style="width: 120px;">
                    <span class="right">
                        <%=TotSqrBrokerage%>
                    </span>
                </div>
                <div id="footerTotTurnover" runat="server" class="left cellTotal" style="width: 120px;">
                    <span class="right">
                        <%=TotTurnover%>
                    </span>
                </div>
                <div id="footerTotTotalBrokerage" runat="server" class="left cellTotal" style="width: 120px;">
                    <span class="right">
                        <%=TotTotalBrokerage%>
                    </span>
                </div>
                <span class="clear" style="height: 0px"></span>
            </div>
            <span class="clear"></span>
        </div>
    </div>
    <div id="MainFull">
        <div id="header" class="Header">
            <div id="divShowFilter" style="text-align: right;">
                <dxe:ASPxButton ID="btnShowFilter" runat="server" AutoPostBack="False" ClientInstanceName="cbtnShowFilter"
                    Text="Show Filter" Font-Size="7" TabIndex="0" CssClass="btnRight">
                    <ClientSideEvents Click="function(s,e){BtnShowFilter_Click();}" />
                    <Paddings Padding="0" PaddingBottom="0" PaddingLeft="0" PaddingRight="0" />
                </dxe:ASPxButton>
            </div>
            Net Position CM Segments
        </div>
        <div id="Row0" class="Row">
            <div id="Container1" class="container">
                <div id="C1_Row0" class="Row">
                    <div id="C1_Row0_Col1" class="LFloat_Lable LableWidth">
                        Date Range :
                    </div>
                    <div id="C1_Row0_Col2" class="LFloat_Content ContentWidth">
                        <dxe:ASPxDateEdit ID="DtFrom" runat="server" ClientInstanceName="cDtFrom" DateOnError="Today"
                            EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" TabIndex="0">
                            <ClientSideEvents DateChanged="function(s,e){CompareFromDate();}"></ClientSideEvents>
                            <DropDownButton Text="From">
                            </DropDownButton>
                        </dxe:ASPxDateEdit>
                    </div>
                    <div id="C1_Row0_Col3" class="LFloat_Content ContentWidth">
                        <dxe:ASPxDateEdit ID="DtTo" runat="server" ClientInstanceName="cDtTo" DateOnError="Today"
                            EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" TabIndex="0">
                            <ClientSideEvents DateChanged="function(s,e){CompareToDate();}"></ClientSideEvents>
                            <DropDownButton Text="To">
                            </DropDownButton>
                        </dxe:ASPxDateEdit>
                    </div>
                    <span class="clear"></span>
                </div>
                <div id="C1_Row1" class="Row">
                    <div id="C1_Row1_Col1" class="LFloat_Lable LableWidth">
                        Segment :
                    </div>
                    <div id="C1_Row1_Col2" class="LFloat_Content ContentWidth">
                        <dxe:ASPxRadioButtonList ID="RblSegment" runat="server" SelectedIndex="0" ItemSpacing="20px"
                            Paddings-PaddingTop="1px" RepeatDirection="Horizontal" TextWrap="False" TabIndex="0"
                            ClientInstanceName="cRblSegment">
                            <Items>
                                <dxe:ListEditItem Text="All" Value="A" />
                                <dxe:ListEditItem Text="Specific" Value="S" />
                            </Items>
                            <ClientSideEvents ValueChanged="function(s, e) {fn_Segment(s.GetValue());}" />
                            <Border BorderWidth="0px" />
                        </dxe:ASPxRadioButtonList>
                    </div>
                </div>
                <div id="C1_Row2" class="Row">
                    <div id="C1_Row2_Col1" class="LFloat_Lable LableWidth">
                        <asp:Label ID="lblGroupBy" runat="server" Text="Group By : "></asp:Label>
                    </div>
                    <div class="left">
                        <div id="C1_Row2_Col2" class="LFloat_Content ContentWidth">
                            <dxe:ASPxComboBox ID="CmbGroupBy" runat="server" ValueType="System.String" ClientInstanceName="cCmbGroupBy"
                                SelectedIndex="0" TabIndex="0">
                                <Items>
                                    <dxe:ListEditItem Text="Branch" Value="B"></dxe:ListEditItem>
                                    <dxe:ListEditItem Text="Group" Value="G"></dxe:ListEditItem>
                                    <dxe:ListEditItem Text="Client" Value="C"></dxe:ListEditItem>
                                </Items>
                                <ClientSideEvents SelectedIndexChanged="function(s, e) {fn_GroupBy(s.GetValue());}" />
                            </dxe:ASPxComboBox>
                        </div>
                        <div class="left">
                            <div>
                                <div id="C1_Row2_Col3" class="LFloat_Content ContentWidth">
                                    <dxe:ASPxRadioButtonList ID="RblBranch" runat="server" SelectedIndex="0" ItemSpacing="20px"
                                        Paddings-PaddingTop="1px" RepeatDirection="Horizontal" TextWrap="False" TabIndex="0"
                                        ClientInstanceName="cRblBranch">
                                        <Items>
                                            <dxe:ListEditItem Text="All" Value="A" />
                                            <dxe:ListEditItem Text="Specific" Value="S" />
                                        </Items>
                                        <ClientSideEvents ValueChanged="function(s, e) {fn_Branch(s.GetValue());}" />
                                        <Border BorderWidth="0px" />
                                    </dxe:ASPxRadioButtonList>
                                </div>
                            </div>
                            <div>
                                <div id="C1_Row2_Col4" class="LFloat_Content ContentWidth">
                                    <dxe:ASPxRadioButtonList ID="RblClient" runat="server" SelectedIndex="0" ItemSpacing="20px"
                                        Paddings-PaddingTop="1px" RepeatDirection="Horizontal" TextWrap="False" TabIndex="0"
                                        ClientInstanceName="cRblClient">
                                        <Items>
                                            <dxe:ListEditItem Text="All" Value="A" />
                                            <dxe:ListEditItem Text="Specific" Value="S" />
                                        </Items>
                                        <ClientSideEvents ValueChanged="function(s, e) {fn_Client(s.GetValue());}" />
                                        <Border BorderWidth="0px" />
                                    </dxe:ASPxRadioButtonList>
                                </div>
                            </div>
                            <div id="C1_Row2_Col5">
                                <div class="left">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="LFloat_Content ContentWidth">
                                                <asp:DropDownList ID="ddlGrouptype" AutoPostBack="true" runat="server" Font-Size="13px"
                                                    onchange="fn_GroupType(this.value)" TabIndex="0" Width="170px">
                                                </asp:DropDownList>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnhide" EventName="Click"></asp:AsyncPostBackTrigger>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <div id="C1_Row2_Col6" class="LFloat_Content ContentWidth" style="display: none; margin-top: 3px;">
                                    <dxe:ASPxRadioButtonList ID="RblGroup" runat="server" SelectedIndex="0" ItemSpacing="20px"
                                        Paddings-PaddingTop="1px" RepeatDirection="Horizontal" TextWrap="False" TabIndex="0"
                                        ClientInstanceName="cRblGroup">
                                        <Items>
                                            <dxe:ListEditItem Text="All" Value="A" />
                                            <dxe:ListEditItem Text="Specific" Value="S" />
                                        </Items>
                                        <ClientSideEvents ValueChanged="function(s, e) {fn_Group(s.GetValue());}" />
                                        <Border BorderWidth="0px" />
                                    </dxe:ASPxRadioButtonList>
                                </div>
                            </div>
                            <span class="clear"></span>
                        </div>
                    </div>
                </div>
                <span class="clear"></span>
                <div id="C1_Row3" class="Row">
                    <div id="C1_Row3_Col1" class="LFloat_Lable LableWidth">
                        Product :
                    </div>
                    <div id="C1_Row3_Col2" class="LFloat_Content ContentWidth">
                        <dxe:ASPxRadioButtonList ID="RblProduct" runat="server" SelectedIndex="0" ItemSpacing="20px"
                            Paddings-PaddingTop="1px" RepeatDirection="Horizontal" TextWrap="False" TabIndex="0"
                            ClientInstanceName="cRblProduct">
                            <Items>
                                <dxe:ListEditItem Text="All" Value="A" />
                                <dxe:ListEditItem Text="Specific" Value="S" />
                            </Items>
                            <ClientSideEvents ValueChanged="function(s, e) {fn_Product(s.GetValue());}" />
                            <Border BorderWidth="0px" />
                        </dxe:ASPxRadioButtonList>
                    </div>
                </div>
                <div id="C1_Row4" class="Row">
                    <div id="C1_Row4_Col1" class="LFloat_Lable LableWidth">
                        Filter Column :<a id="HideFilterColumn" href="javascript:void(0)" onclick="fn_HideFilterColumn()"
                            title="Hide Filter Columns">[-]</a><a id="ExpandFilterColumn" href="javascript:void(0)"
                                onclick="fn_ShowFilterColumn()" title="Expand Columns For Select">[+]</a>
                    </div>
                    <div class="left">
                        <div id="C1_Row4_Col2">
                            <span style="font-size: 12px; line-height: 2;">Please Click To [+] Expand.</span>
                        </div>
                        <div id="C1_Row4_Col3" style="display: none;">
                            <asp:UpdatePanel ID="UpdateAllList" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div id="C1_Row4_Col4" class="LFloat_Content Box_filter">
                                        <div class="Box_filter_HeaderCont">
                                            Columns Showing :
                                        </div>
                                        <span class="clear"></span>
                                        <asp:ListBox ID="lstAllListBox" class="Box_filterCont" runat="Server" SelectionMode="Multiple"></asp:ListBox>
                                    </div>
                                    <div id="C1_Row4_Col5" class="left Box_filterBtn">
                                        <asp:Button ID="btnAddAllItem" runat="server" CssClass="Btn_filterAddAll" ToolTip="Add All Items"
                                            OnClick="btnAddAllItem_Click" />
                                        <asp:Button ID="btnAddItem" runat="server" CssClass="Btn_filterAdd" ToolTip="Add One Item"
                                            OnClick="btnAddItem_Click" />
                                        <asp:Button ID="btnRemoveItem" runat="server" CssClass="Btn_filterRemove" ToolTip="Remove One Item"
                                            OnClick="btnRemoveItem_Click" />
                                        <asp:Button ID="btnRemoveAllItem" runat="server" CssClass="Btn_filterRemoveAll" ToolTip="Remove All Items"
                                            OnClick="btnRemoveAllItem_Click" />
                                    </div>
                                    <div id="C1_Row4_Col6" class="LFloat_Content Box_filter">
                                        <div class="Box_filter_HeaderNotCont">
                                            Columns Not Showing :
                                        </div>
                                        <span class="clear"></span>
                                        <asp:ListBox ID="lstFilteredListBox" runat="Server" class="Box_filterCont" SelectionMode="Multiple"></asp:ListBox>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnAddItem" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnRemoveItem" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnAddAllItem" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnRemoveAllItem" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <span class="clear"></span>
                        </div>
                    </div>
                </div>
                <div id="C1_Row5" class="Row">
                    <div id="C1_Row5_Col1" class="LFloat_Lable LableWidth">
                        Report Type :
                    </div>
                    <div id="C1_Row5_Col2" class="LFloat_Content ContentWidth">
                        <dxe:ASPxRadioButtonList ID="RblReportType" runat="server" SelectedIndex="0" ItemSpacing="5px"
                            Paddings-PaddingTop="1px" RepeatDirection="Horizontal" TextWrap="False" TabIndex="0"
                            ClientInstanceName="cRblReportType">
                            <Items>
                                <dxe:ListEditItem Text="By Date" Value="D" />
                                <dxe:ListEditItem Text="Consolidated" Value="C" />
                            </Items>
                            <Border BorderWidth="0px" />
                        </dxe:ASPxRadioButtonList>
                    </div>
                </div>
                <div id="C1_Row7" class="Row">
                    <div id="C1_Row7_Col1" class="LFloat_Lable LableWidth">
                        Report By :
                    </div>
                    <div id="C1_Row7_Col2" class="LFloat_Content ContentWidth">
                        <dxe:ASPxComboBox ID="CmbReportBy" runat="server" ValueType="System.String" ClientInstanceName="cCmbReportBy"
                            SelectedIndex="0" TabIndex="0">
                            <ClientSideEvents SelectedIndexChanged="function(s, e) {fn_ReportBy(s.GetValue());}" />
                            <Items>
                                <dxe:ListEditItem Text="Screen" Value="S"></dxe:ListEditItem>
                                <dxe:ListEditItem Text="Excel" Value="E"></dxe:ListEditItem>
                            </Items>
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div id="C1_Row6" class="Row btnRight">
                    <dxe:ASPxButton ID="btnShow" runat="server" AutoPostBack="true" ClientInstanceName="cbtnShow"
                        Text="Show Result" OnClick="BtnShow_Click" Width="100px">
                    </dxe:ASPxButton>
                </div>
            </div>
            <div id="Container2" class="container">
                <div id="C2_Row0" class="Row">
                    <div id="C2_Row0_Col1" class="LFloat_Content">
                        <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="323px" TabIndex="0"></asp:TextBox>
                    </div>
                    <div id="C2_Row0_Col2" class="LFloat_Lable">
                        <a href="javascript:void(0);" tabindex="0" onclick="btnAddToList_click()"><span class="lnkBtnAjax green">Add to List</span></a>
                    </div>
                </div>
                <div id="C2_Row1" class="Row">
                    <div id="C2_Row1_Col1" class="LFloat_Content finalSelectedBox">
                        <asp:ListBox ID="lstSelection" runat="server" Font-Size="12px" Height="100px" Width="410px"
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
                <asp:GridView ID="gvwNetPosition" AutoGenerateColumns="false" Width="100%" runat="server"
                    DataKeyNames="AutoID" TabIndex="0" ShowFooter="false" OnRowDataBound="gvwNetPosition_RowDataBound"
                    CellPadding="2">
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
                        <asp:TemplateField HeaderText="Segment" ControlStyle-CssClass="segment">
                            <ItemTemplate>
                                <%#Eval("Segment")%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" Wrap="false" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date" ControlStyle-CssClass="date">
                            <ItemTemplate>
                                <%#Eval("Date")%>
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
                        <asp:TemplateField HeaderText="Symbol" ControlStyle-CssClass="symbol">
                            <ItemTemplate>
                                <%#Eval("Symbol")%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ISIN" ControlStyle-CssClass="iSIN">
                            <ItemTemplate>
                                <%#Eval("ISIN")%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" Width="100px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pur" ControlStyle-CssClass="pur">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("Pur").ToString()) ? "" : Eval("Pur")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PurMkt" ControlStyle-CssClass="purMkt">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("PurMkt").ToString()) ? "" : Eval("PurMkt")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PurAvg" ControlStyle-CssClass="purAvg">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("PurAvg").ToString()) ? "" : Eval("PurAvg")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PurNet" ControlStyle-CssClass="purNet">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("PurNet").ToString()) ? "" : Eval("PurNet")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PurNetAvg" ControlStyle-CssClass="purNetAvg">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("PurNetAvg").ToString()) ? "" : Eval("PurNetAvg")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sale" ControlStyle-CssClass="sale">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("Sale").ToString()) ? "" : Eval("Sale")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SaleMkt" ControlStyle-CssClass="saleMkt">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("SaleMkt").ToString()) ? "" : Eval("SaleMkt")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SaleAvg" ControlStyle-CssClass="saleAvg">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("SaleAvg").ToString()) ? "" : Eval("SaleAvg")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SaleNet" ControlStyle-CssClass="saleNet">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("SaleNet").ToString()) ? "" : Eval("SaleNet")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SaleNetAvg" ControlStyle-CssClass="saleNetAvg">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("SaleNetAvg").ToString()) ? "" : Eval("SaleNetAvg")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SqrQty" ControlStyle-CssClass="sqrQty">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("SqrQty").ToString()) ? "" : Eval("SqrQty")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SqrPL" ControlStyle-CssClass="sqrPL">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("SqrPL").ToString()) ? "" :Eval("SqrPL")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DlvPur" ControlStyle-CssClass="dlvPur">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("DlvPur").ToString()) ? "" :Eval("DlvPur")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DlvPurMkt" ControlStyle-CssClass="dlvPurMkt">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("DlvPurMkt").ToString()) ? "" :Eval("DlvPurMkt")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DlvPurAvg" ControlStyle-CssClass="dlvPurAvg">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("DlvPurAvg").ToString()) ? "" :Eval("DlvPurAvg")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DlvPurNet" ControlStyle-CssClass="dlvPurNet">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("DlvPurNet").ToString()) ? "" : Eval("DlvPurNet")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DlvPurNetAvg" ControlStyle-CssClass="dlvPurNetAvg">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("DlvPurNetAvg").ToString()) ? "" : Eval("DlvPurNetAvg")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DlvSale" ControlStyle-CssClass="dlvSale">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("DlvSale").ToString()) ? "" : Eval("DlvSale")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DlvSaleMkt" ControlStyle-CssClass="dlvSaleMkt">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("DlvSaleMkt").ToString()) ? "" : Eval("DlvSaleMkt")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DlvSaleAvg" ControlStyle-CssClass="dlvSaleAvg">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("DlvSaleAvg").ToString()) ? "" : Eval("DlvSaleAvg")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DlvSaleNet" ControlStyle-CssClass="dlvSaleNet">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("DlvSaleNet").ToString()) ? "" : Eval("DlvSaleNet")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DlvSaleNetAvg" ControlStyle-CssClass="dlvSaleNetAvg">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("DlvSaleNetAvg").ToString()) ? "" : Eval("DlvSaleNetAvg")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="NetDlv" ControlStyle-CssClass="netDlv">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("NetDlv").ToString()) ? "" : Eval("NetDlv")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ClPrice" ControlStyle-CssClass="clPrice">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("ClPrice").ToString()) ? "" : Eval("ClPrice")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="MtmPL" ControlStyle-CssClass="mtmPL">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("MtmPL").ToString()) ? "" : Eval("MtmPL")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TotalPL" ControlStyle-CssClass="totalPL">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("TotalPL").ToString()) ? "" : Eval("TotalPL")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Exposure" ControlStyle-CssClass="exposure">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("Exposure").ToString()) ? "" : Eval("Exposure")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DlvTurnover" ControlStyle-CssClass="dlvTurnover">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("DlvTurnover").ToString()) ? "" : Eval("DlvTurnover")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DlvBrokerage" ControlStyle-CssClass="dlvBrokerage">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("DlvBrokerage").ToString()) ? "" : Eval("DlvBrokerage")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SqrTurnover" ControlStyle-CssClass="sqrTurnover">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("SqrTurnover").ToString()) ? "" : Eval("SqrTurnover")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SqrBrokerage" ControlStyle-CssClass="sqrBrokerage">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("SqrBrokerage").ToString()) ? "" :Eval("SqrBrokerage")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Turnover" ControlStyle-CssClass="turnover">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("Turnover").ToString()) ? "" :Eval("Turnover")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TotalBrokerage" ControlStyle-CssClass="totalBrokerage">
                            <ItemTemplate>
                                <%#(string)ConvertToNegetive((string)(String.IsNullOrEmpty(Eval("TotalBrokerage").ToString()) ? "" :Eval("TotalBrokerage")))%>
                            </ItemTemplate>
                            <ItemStyle CssClass="cellRecord txt_right" Wrap="false" />
                            <HeaderStyle CssClass="cellHeader" />
                        </asp:TemplateField>
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
                        width="32px" height="32px" />
                </div>
                <br class="clear" style="height: 0px" />
            </center>
        <br class="clear" style="height: 0px" />
        <div id="Row2">
            <asp:TextBox ID="txtSelectionID_hidden" runat="server"></asp:TextBox>
            <asp:HiddenField ID="HDNSegment" runat="server" />
            <asp:HiddenField ID="HDNProduct" runat="server" />
            <asp:HiddenField ID="HDNBranch" runat="server" />
            <asp:HiddenField ID="HDNGroup" runat="server" />
            <asp:HiddenField ID="HDNClient" runat="server" />
            <asp:Button ID="btnhide" runat="server" OnClick="btnhide_Click" />
            <asp:HiddenField ID="HDNFilterCol" runat="server" />
            <asp:HiddenField ID="HDNReportType" runat="server" />
        </div>
        <span class="clear" style="height: 0px"></span>
    </div>
</asp:Content>
