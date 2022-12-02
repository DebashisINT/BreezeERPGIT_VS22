<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_INMXBrokerageTOAnalysis" Codebehind="INMXBrokerageTOAnalysis.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" src="/assests/js/ajaxList_rootfile.js"></script>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/GenericJScript.js"></script>

    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />
    <link type="text/css" href="../CSS/AjaxStyle.css" rel="Stylesheet" />
    <style type="text/css">    
    form {
		display:inline;		
	}	
    </style>

    <script type="text/javascript" language="javascript">
    function SignOff()
    {
      window.parent.SignOff()
    }
    function Page_Load()///Call Into Page Load
    {         
         Hide('showFilter');
         Hide('tr_filter'); 
        // document.getElementById('btnExport').disabled=true;
         Reset();      
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
        window.frameElement.height = '300px';
        window.frameElement.Width = document.body.scrollWidth;
    }
    FieldName="none";
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
    function DateCompare(DateobjFrm,DateobjTo)
    {
        var Msg="To Date Can Not Be Less Than From Date!!!";
        DevE_CompareDateForMin(DateobjFrm,DateobjTo,Msg);
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
       function  fnddlGroup(obj)
       {  
            var clist=document.getElementById('lstSlection').options.length;
            var sType=document.getElementById('cmbsearchOption').value;                  
            if(obj=="0")
            {
                document.getElementById('rdbranchclientAll').checked=true;
                fn_Branch('a'); 
                Hide('td_group');
                Show('td_branch');
                Hide('td_allselect');
            }           
            if(obj=="1")                
            {  
                Show('td_group');
                Hide('td_branch');
                Hide('showFilter');
                document.getElementById('<%=ddlGenerate.ClientID %>').style.display='inline';
                var btn = document.getElementById('btnhide');
                btn.click();            
             }            
             if(obj=="2")
             { 
                document.getElementById('rdbranchclientSelected').checked=true;
                fn_Clients('b');
                Hide('td_group');
                Show('td_branch');
                Hide('td_allselect');               
              }     
            height();
       }
       function fn_Branch(obj)
       {     
            if(obj=="a")
              {
                Hide('showFilter');
                document.getElementById('<%=ddlGenerate.ClientID %>').style.display='inline';
              }
             else
             {
              var cmbBranchOpt=document.getElementById('cmbsearchOption');
                  if(document.getElementById('ddlGroup').value=="0")
                   {
                    cmbBranchOpt.value='Branch';
                   }                   
                  Show('showFilter');
                  document.getElementById('<%=ddlGenerate.ClientID %>').style.display='none';
             }                      
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
               document.getElementById('rdddlgrouptypeAll').checked=true;
               Show('td_allselect');
           }      
           height();
       }
       function fn_Group(obj)
       {      
            if(obj=="a")
             {
                Hide('showFilter');
                document.getElementById('<%=ddlGenerate.ClientID %>').style.display='inline';
             }
             else
             {
                  var cmb=document.getElementById('cmbsearchOption');
                  cmb.value='Group';
                  Show('showFilter');
                  document.getElementById('<%=ddlGenerate.ClientID %>').style.display='none';
             }                        
       }  
        function fn_Clients(obj)
        {        
             if(obj=="a")
             {
                Hide('showFilter');
                document.getElementById('<%=ddlGenerate.ClientID %>').style.display='inline';
             }   
             else
             {
                  var cmbClient=document.getElementById('cmbsearchOption');
                  if(document.getElementById('ddlGroup').value=="2")
                   {
                      cmbClient.value='Clients';
                   }
                  Show('showFilter');
                  document.getElementById('<%=ddlGenerate.ClientID %>').style.display='none';
             }             
        }
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
                btn.click();
            }  
            if(j[0]=='Clients')
            {
                document.getElementById('HiddenField_Client').value = j[1];
            }               
        }
        function FunClientScrip(objID,objListFun,objEvent)
        {
          var cmbVal; 
          if(document.getElementById('cmbsearchOption').value=="Clients")
            {
                if(document.getElementById('ddlGroup').value=="0")//////////////Group By  selected are branch
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
               else if(document.getElementById('ddlGroup').value=="1")//////////////Group By selected are Group
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
               else if(document.getElementById('ddlGroup').value=="2")///////client wise
                {
                       cmbVal='ClientsBranch'+'~'+'ALL';
                }
            }
            else
            {
                cmbVal=document.getElementById('cmbsearchOption').value;
                cmbVal=cmbVal+'~'+document.getElementById('ddlgrouptype').value;
            }          
          ajax_showOptions(objID,objListFun,objEvent,cmbVal);
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
                alert('Please search name and then Add!');
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
            document.getElementById('<%=ddlGenerate.ClientID %>').style.display='inline';
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
      function NORECORD(obj)
      {      
          Hide('showFilter');
          document.getElementById('<%=ddlGenerate.ClientID %>').style.display='inline';
          var grpBy=document.getElementById('ddlGroup').value;         
            if(obj=='1')
            {
                 if(grpBy=='0' && document.getElementById('HiddenField_Branch').value=='')
                 {
                    alert('You Have To Select Atleast One Branch !!');
                    Reset();  
                 }
                 if(grpBy=='1' && document.getElementById('HiddenField_Group').value=='')
                 {
                    alert('You Have To Select Atleast One Group !!');
                    Reset();  
                 }
                 if(grpBy=='2' && document.getElementById('HiddenField_Client').value=='')
                 {
                    alert('You Have To Select Atleast One Client !!');  
                    Reset();               
                 }
             }  
            if(obj=='2')
            {
                alert('No Record Found !! ');
                Reset();
            }
          height();
      }
      function Reset()
      {  
           Hide('showFilter');  
           document.getElementById('<%=ddlGenerate.ClientID %>').style.display='inline';        
           document.getElementById('HiddenField_Branch').value=='';
           document.getElementById('HiddenField_Group').value=='';
           document.getElementById('HiddenField_Client').value=='';   
           dtfrom.SetDate(new Date());
           dtto.SetDate(new Date()); 
           document.getElementById('<%=ddlRptType.ClientID%>').selectedIndex=0;
           document.getElementById('<%=ddlConCurrencyType.ClientID%>').selectedIndex=0;                       
           document.getElementById('<%=ddlGroup.ClientID%>').selectedIndex=0;
           document.getElementById('rdbranchclientAll').checked=true;
           document.getElementById('<%=cmbsearchOption.ClientID%>').selectedIndex=0;
           document.getElementById('<%=ddlgrouptype.ClientID%>').selectedIndex=0;  
           document.getElementById('<%=ddlgrouptype.ClientID%>').style.display='none';
           document.getElementById('td_group').style.display='none';   
           document.getElementById('td_branch').style.display='inline';                                    
           height();
      }
      function IsValidateSearch()
      {
            var sFromDate= dtfrom.GetDate();     
            var sToDate=dtto.GetValue();
            var sConCurrencyType=document.getElementById('<%=ddlConCurrencyType.ClientID%>').value.trim();  
            var sReportType = document.getElementById('<%=ddlRptType.ClientID%>').value.trim(); 
                if(sFromDate==null)
                {
                    alert("Please Select From Date");
                    dtfrom.Focus();
                    return false;                       
                }
                if(sToDate==null)
                {
                    alert("Please Select To Date");
                    dtto.Focus();
                    return false;                       
                }
            if(sReportType=="0")
            {
                alert("Please Select A Report Type");
               document.getElementById('<%=ddlRptType.ClientID%>').focus();
                return false;
            }
            if(sConCurrencyType=="0")
            {
                alert("Please Select A Currency");
                document.getElementById('<%=ddlConCurrencyType.ClientID%>').focus();
                return false;
            }
            return true;
      }
      function ddlGenerate_OnChange(ele)
       {
          if(IsValidateSearch())
          {
            if(ele=="G")
              {
                var ddlShowGrid=document.getElementById("<%=ddlGenerate.ClientID%>");  
                 document.getElementById('btnShowGrid').click();       
                 ddlShowGrid.options[0].selected=true; 
              }
              else if(ele=="E")
              {          
                 var ddlExport=document.getElementById("<%=ddlGenerate.ClientID%>");  
                 document.getElementById('btnExcelExport').click();                                
                 ddlExport.options[0].selected=true; 
              }
          }
       }
    </script>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <div class="TableMain100">
                <div class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">Client/Product wise Turnover cum Net Earning Analysis</span></strong>
                </div>
            </div>
            <br class="clear" />
            <div class="pageContent">
                <asp:Label ID="lblStatus" runat="server" Font-Size="XX-Small" Font-Names="Arial"
                    Font-Bold="True" ForeColor="Red" />
                <div id="divPageFilter">
                    <div id="showFilter" class="right" style="width: 472px; background-color: #B7CEEC;
                        border: solid 2px  #ccc; display: none;">
                        <div style="width: 100%">
                            <div class="frmleftContent">
                                <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Height="20px" Width="260px"
                                    onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)" TabIndex="11"></asp:TextBox>
                            </div>
                            <div class="frmleftContent" style="padding-top: 3px">
                                <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="13px" Width="110px"
                                    Enabled="false">
                                    <asp:ListItem>Branch</asp:ListItem>
                                    <asp:ListItem>Group</asp:ListItem>
                                    <asp:ListItem>Clients</asp:ListItem>
                                </asp:DropDownList></div>
                            <div class="frmleftContent">
                                <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                    style="color: #009900; text-decoration: underline; font-size: 10pt;">Add to List</span></a>
                            </div>
                        </div>
                        <span class="clear" style="background-color: #B7CEEC;"></span>
                        <div class="frmleftContent" style="height: 105px; margin-top: 5px">
                            <asp:ListBox ID="lstSlection" runat="server" Font-Size="12px" Height="100px" Width="460px"
                                TabIndex="13"></asp:ListBox>
                        </div>
                        <span class="clear" style="background-color: #B7CEEC;"></span>
                        <div class="frmleftContent" style="text-align: center">
                            <a id="A2" href="javascript:void(0);" onclick="clientselectionfinal()"><span style="color: #000099;
                                text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp; <a id="A1"
                                    href="javascript:void(0);" onclick="btnRemovefromsubscriptionlist_click()"><span
                                        style="color: #cc3300; text-decoration: underline; font-size: 10pt;">Remove</span></a>
                        </div>
                    </div>                   
                    <div id="dvMainFilter" class="frmContent" style="width: 508px">
                        <div id="forDate">
                            <div class="frmleftContent" style="width: 90px; line-height: 20px">
                                <asp:Label ID="lblDate" runat="server" Text="Date : "></asp:Label>
                            </div>
                            <div class="left">
                                <div class="frmleftContent">
                                    <dxe:ASPxDateEdit ID="dtfrom" runat="server" ClientInstanceName="dtfrom" DateOnError="Today"
                                        EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" Width="125px"
                                        Height="25px" Font-Size="11px" TabIndex="1">
                                        <DropDownButton Text="From">
                                        </DropDownButton>
                                        <ClientSideEvents DateChanged="function(s,e){DateCompare(dtfrom,dtto);}"></ClientSideEvents>
                                    </dxe:ASPxDateEdit>
                                </div>
                                <div class="frmleftContent">
                                    <dxe:ASPxDateEdit ID="dtto" runat="server" ClientInstanceName="dtto" DateOnError="Today"
                                        EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" Width="125px"
                                        Height="25px" Font-Size="11px" TabIndex="2">
                                        <DropDownButton Text="To">
                                        </DropDownButton>
                                        <ClientSideEvents DateChanged="function(s,e){DateCompare(dtfrom,dtto);}"></ClientSideEvents>
                                    </dxe:ASPxDateEdit>
                                </div>
                            </div>
                        </div>
                        <span class="clear"></span>
                        <div id="divRptType">
                            <div class="frmleftContent" style="width: 90px; line-height: 20px">
                                <asp:Label ID="lblRptType" runat="server" Text="Report Type : "></asp:Label>
                            </div>
                            <div class="frmleftContent">
                                <asp:DropDownList ID="ddlRptType" runat="server" Width="340px" Font-Size="13px" TabIndex="3">
                                    <asp:ListItem Value="0" Selected="True">Select Report Type</asp:ListItem>
                                    <asp:ListItem Value="1">Client+Exchange Brokerage & TO Summary</asp:ListItem>
                                    <asp:ListItem Value="2">Client+Instrument Brokerage & TO</asp:ListItem>
                                    <asp:ListItem Value="3">Client+Product Brokerage & TO Summary</asp:ListItem>
                                    <asp:ListItem Value="4">Date+Client+Instrument Brokerage & TO</asp:ListItem>
                                    <asp:ListItem Value="5">Client wise Spread-Offset Charge Vs Expiry Cost</asp:ListItem>
                                    <asp:ListItem Value="6">Client+Product wise Spread-Offset Charge Vs Expiry Cost</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <span class="clear"></span>
                        <div id="divCurrency">
                            <div class="frmleftContent" style="width: 90px; line-height: 20px">
                                <asp:Label ID="lblCurrency" runat="server" Text="Currency : "></asp:Label>
                            </div>
                            <div class="frmleftContent">
                                <asp:DropDownList ID="ddlConCurrencyType" runat="server" Width="150px" Font-Size="13px"
                                    TabIndex="4">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <span class="clear"></span>
                        <div id="tr_grpselection">
                            <div class="frmleftContent" style="width: 90px; line-height: 20px">
                                <asp:Label ID="lblGroupBy" runat="server" Text="Group By : "></asp:Label>
                            </div>
                            <div class="left">
                                <div class="frmleftContent" style="padding-top: 3px">
                                    <asp:DropDownList ID="ddlGroup" runat="server" Width="125px" Font-Size="13px" onchange="fnddlGroup(this.value)"
                                        TabIndex="5">
                                        <asp:ListItem Value="0">Branch</asp:ListItem>
                                        <asp:ListItem Value="1">Group</asp:ListItem>
                                        <asp:ListItem Value="2">Clients</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div id="td_branch" class="frmleftContent" style="width: 125px; padding-top: 3px;
                                    font-size: 12px;">
                                    <asp:RadioButton ID="rdbranchclientAll" runat="server" Checked="True" GroupName="a"
                                        onclick="fn_Branch('a')" TabIndex="6" />
                                    All
                                    <asp:RadioButton ID="rdbranchclientSelected" runat="server" GroupName="a" onclick="fn_Branch('b')"
                                        TabIndex="7" />Selected
                                </div>
                                <div id="td_group" class="left" style="display: none;">
                                    <div class="left">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="frmleftContent">
                                                    <asp:DropDownList ID="ddlgrouptype" runat="server" Width="125px" Font-Size="13px"
                                                        onchange="fngrouptype(this.value)" TabIndex="8">
                                                    </asp:DropDownList>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnhide" EventName="Click"></asp:AsyncPostBackTrigger>
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div id="td_allselect" class="frmleftContent" style="display: none; width: 125px;
                                        font-size: 12px">
                                        <asp:RadioButton ID="rdddlgrouptypeAll" runat="server" Checked="True" GroupName="b"
                                            onclick="fn_Group('a')" TabIndex="9" />
                                        All
                                        <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="b" onclick="fn_Group('b')"
                                            TabIndex="10" />Selected
                                    </div>
                                </div>
                            </div>
                        </div>
                        <span class="clear"></span>
                        <div id="td_show" class="right" style="padding: 5px">
                            <asp:DropDownList ID="ddlGenerate" runat="server" Onchange="ddlGenerate_OnChange(this.value)">
                                <asp:ListItem Value="A" Selected="True">Generate</asp:ListItem>
                                <asp:ListItem Value="G">Show Grid</asp:ListItem>
                                <asp:ListItem Value="E">Export</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <br class="clear" />
                <div id="dvGvw">
                    <dxe:ASPxGridView ID="GridVWINMX" KeyFieldName="Client" Width="990px" runat="Server"
                        AutoGenerateColumns="False" ClientInstanceName="cGridVWINMX" OnCustomUnboundColumnData="GridVWINMX_CustomUnboundColumnData">
                        <Settings ShowHorizontalScrollBar="True" />
                        <SettingsBehavior AllowFocusedRow="True" />
                        <Styles>
                            <FocusedGroupRow CssClass="gridselectrow">
                            </FocusedGroupRow>
                            <FocusedRow CssClass="gridselectrow" BackColor="#FCA977">
                            </FocusedRow>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>
                        <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="Srl" VisibleIndex="0" Width="40px">
                                <CellStyle CssClass="gridcellright" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataDateColumn FieldName="Trade Date" VisibleIndex="1" Width="100px">
                                <PropertiesDateEdit DisplayFormatString="dd MMM yyyy">
                                </PropertiesDateEdit>
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataDateColumn>
                            <dxe:GridViewDataTextColumn FieldName="Client" VisibleIndex="2" Width="140px">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="TradeCode" VisibleIndex="3" Width="80px">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Product" VisibleIndex="4" Width="200px">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Instrument" VisibleIndex="5" Width="160px">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Exchange" VisibleIndex="6" Width="80px">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataDateColumn FieldName="Exp.Date" VisibleIndex="7" Width="100px">
                                <PropertiesDateEdit DisplayFormatString="dd MMM yyyy">
                                </PropertiesDateEdit>
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataDateColumn>
                            <dxe:GridViewDataTextColumn FieldName="Trd.Cat" VisibleIndex="8" Width="80px">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Trd.Cur" VisibleIndex="9" Width="80px">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Conv.Cur" VisibleIndex="10" Width="80px">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Lots" VisibleIndex="11" Width="120px">
                                <CellStyle CssClass="gridcellright" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="LotSize" VisibleIndex="12" Width="120px">
                                <CellStyle CssClass="gridcellright" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Total Lots" VisibleIndex="13" Width="120px">
                                <CellStyle CssClass="gridcellright" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Trd.Qty" VisibleIndex="14" Width="120px">
                                <CellStyle CssClass="gridcellright" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Trd.Unit" VisibleIndex="15" Width="60px">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Conv.Rate" VisibleIndex="16" Width="100px">
                                <CellStyle CssClass="gridcellright" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Conv.Qty" VisibleIndex="17" Width="120px">
                                <CellStyle CssClass="gridcellright" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Conv.Unit" VisibleIndex="18" Width="70px">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="TurnOver" VisibleIndex="19" Width="120px">
                                <CellStyle CssClass="gridcellright" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Exch.Charge" VisibleIndex="20" Width="120px">
                                <CellStyle CssClass="gridcellright" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Brokerage" VisibleIndex="21" Width="120px">
                                <CellStyle CssClass="gridcellright" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Net Earning" VisibleIndex="22" Width="120px">
                                <CellStyle CssClass="gridcellright" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="FxRate" VisibleIndex="23" Width="60px">
                                <CellStyle CssClass="gridcellright" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Conv.To" VisibleIndex="24" Width="120px">
                                <CellStyle CssClass="gridcellright" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Conv.Exch.Chrg" VisibleIndex="25" Width="120px">
                                <CellStyle CssClass="gridcellright" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Conv.Brkg" VisibleIndex="26" Width="100px">
                                <CellStyle CssClass="gridcellright" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Conv.Net.Earning" VisibleIndex="27" Width="100px">
                                <CellStyle CssClass="gridcellright" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Spread Charge" VisibleIndex="28" Width="100px">
                                <CellStyle CssClass="gridcellright" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Expiry Cost" VisibleIndex="29" Width="100px">
                                <CellStyle CssClass="gridcellright" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Symbol" VisibleIndex="30" Width="200px">
                                <CellStyle CssClass="gridcellright" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="SOTLots" VisibleIndex="31" Width="80px">
                                <CellStyle CssClass="gridcellright" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="SOTAmount" VisibleIndex="32" Width="80px">
                                <CellStyle CssClass="gridcellright" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="EXPLots" VisibleIndex="33" Width="80px">
                                <CellStyle CssClass="gridcellright" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="EXPAmount" VisibleIndex="34" Width="100px">
                                <CellStyle CssClass="gridcellright" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                         <Settings ShowFooter="true"  />
                        <TotalSummary>
                            <dxe:ASPxSummaryItem FieldName="Srl" ShowInColumn="Srl" SummaryType="Count" />
                            <dxe:ASPxSummaryItem FieldName="Lots" ShowInColumn="Lots" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Total Lots" ShowInColumn="Total Lots" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="TurnOver" ShowInColumn="TurnOver" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Exch.Charge" ShowInColumn="Exch.Charge" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Brokerage" ShowInColumn="Brokerage" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Net Earning" ShowInColumn="Net Earning" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Conv.To" ShowInColumn="Conv.To" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Conv.Exch.Chrg" ShowInColumn="Conv.Exch.Chrg" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Conv.Brkg" ShowInColumn="Conv.Brkg" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Conv.Net.Earning" ShowInColumn="Conv.Net.Earning"
                                SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Spread Charge" ShowInColumn="Spread Charge" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Expiry Cost" ShowInColumn="Expiry Cost" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="SOTLots" ShowInColumn="SOTLots" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="SOTAmount" ShowInColumn="SOTAmount" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="EXPLots" ShowInColumn="EXPLots" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="EXPAmount" ShowInColumn="EXPAmount" SummaryType="Sum" />
                        </TotalSummary>                       
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" />
                        <SettingsText EmptyDataRow="No Data To Display" />
                    </dxe:ASPxGridView>
                </div>
                <br class="clear" />
                <div style="display: none">
                    <asp:HiddenField ID="hdnDPSessionValue" runat="server" />
                    <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                    <asp:Button ID="btnhide" runat="server" Text="Button" OnClick="btnhide_Click" />
                    <asp:HiddenField ID="HiddenField_Group" runat="server" />
                    <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                    <asp:HiddenField ID="HiddenField_Client" runat="server" />
                    <asp:Button ID="btnExcelExport" runat="server" BackColor="#DDECFE" BorderStyle="None"
                        OnClick="btnExcelExport_Click" />
                    <asp:Button ID="btnShowGrid" runat="server" BackColor="#DDECFE" BorderStyle="None"
                        OnClick="btnShowGrid_Click" />
                </div>
            </div>
        </div>
</asp:Content>
