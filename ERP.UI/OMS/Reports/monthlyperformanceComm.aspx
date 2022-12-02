<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_monthlyperformanceComm" Codebehind="monthlyperformanceComm.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script language="javascript" type="text/javascript">
  
    function Page_Load()///Call Into Page Load
            {
                 Hide('td_dtfrom');
                 Hide('td_dtto');
                 Hide('showFilter');
                 Show('td_btnshow');
                 Hide('td_btnprint');
                 Hide('tr_filter');
                 document.getElementById('hiddencount').value=0;
//                 DateChange(cdtfor);
                DateChangeatPageLoad(cdtfor);
                DateChangeatPageLoad(cdtfrom);
                DateChangeatPageLoad(cdtto);
                if(document.getElementById('ddldate').value=0)
                {
                   DevE_SetFirstDayOfMonth(cdtfor,cdtrangefrom); 
                   DevE_SetLastDayOfMonth(cdtfor,cdtrangeto); 
                }
                else
                {   
                   DateChangeforExpiryRangeFrom(cdtfrom);
                   DateChangeforExpiryRangeTo(cdtto);
                }
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
     function  fn_ddldateChange(obj)
       {
            if(obj=='0')
            {
                Show('td_dtfor');
                Hide('td_dtfrom');
                Hide('td_dtto');

            }
            else
            {
                Hide('td_dtfor');
                Show('td_dtfrom');
                Show('td_dtto');
            }
            selecttion();
       }
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
              
            }
            else
            {
                cmbVal=document.getElementById('cmbsearchOption').value;
                cmbVal=cmbVal+'~'+document.getElementById('ddlgrouptype').value;
            }
          
          ajax_showOptions(objID,objListFun,objEvent,cmbVal);

        }
        
        function Clients(obj)
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
              height();
        }
      function Branch(obj)
      {
            if(obj=="a")
                Hide('showFilter');
             else
             {
                  var cmb=document.getElementById('cmbsearchOption');
                  cmb.value='Branch';
                  Show('showFilter');
             }
             selecttion();
                height();
      }
     function Group(obj)
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
             height();
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
                document.getElementById('btnshow').disabled=false;
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
        selecttion();
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
       selecttion();
       height();
   } 
   
   function RBShowHide(obj)
        {
             if(obj=='rbPrint')
                 {
                     Hide('td_btnshow');
                     Show('td_btnprint');
                 }
             else
                 {
                     Show('td_btnshow');
                     Hide('td_btnprint');
                 }
                 height();
                 selecttion();
        } 
        
   function fnunderlying(obj)
   {
    if(obj=='a')
    {
        Hide('td_underlying')
    }
    else
    {
        Show('td_underlying')
    }
    Hide('showFilter');
    selecttion();
   }
   
  function fnExpiry(obj)
   {
    if(obj=='a')
    {
        Hide('td_Expiry');
         Hide('td_RangeFrom');
        Hide('td_RangeTo');
    }
    else if(obj=='b')
    {
        Show('td_Expiry');
         Hide('td_RangeFrom');
        Hide('td_RangeTo');
    }
    else
    {
        Hide('td_Expiry');
        Show('td_RangeFrom');
        Show('td_RangeTo');
//        Raisedate();
    
    }
    Hide('showFilter');
    selecttion();
   }
  function NORECORD()
  {
      Hide('tr_filter');
      Hide('displayAll');
      Show('tab2');
      Hide('showFilter');
      alert('No Record Found!!');
      height();
  }
  function Display()
  {
      Show('tr_filter');
      Show('displayAll');
      Hide('tab2');
      Hide('showFilter');
      height();
  }
  function selecttion()
  {
     var combo=document.getElementById('cmbExport');
     combo.value='Ex';
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
     function Filter()
      {
          Hide('tr_filter');
          Hide('displayAll');
          Show('tab2');
          Hide('showFilter');
          selecttion();
          height();
      }

     function Raisedate()
        {      
            var dt=new Date(cdtfor.GetDate());             
            cdtrangefrom.SetDate(dt);
            var FinYearEndDate='<%=Session["FinYearEnd"]%>';
            var SelectedDate = new Date(FinYearEndDate);            
            cdtrangeto.SetDate(SelectedDate);           
//            DevE_CompareDateForMin(cdtrangefrom.GetDate(),dt,'ss');

        }   
          
     function fnexpirtycallajax(objID,objListFun,objEvent)
     {
         var date;
         if(document.getElementById('ddldate').value=='0')
         {
            date=new Date(cdtfor.GetDate());
            date=parseInt(date.getMonth()+1)+'-'+date.getDate()+'-'+ date.getFullYear();
            
         }
         else
         {
            date=new Date(cdtfrom.GetDate());
            date=parseInt(date.getMonth()+1)+'-'+date.getDate()+'-'+ date.getFullYear();
         }
         ajax_showOptions(objID,objListFun,objEvent,'Expiry'+'~'+date);
     }
   FieldName='lstSlection';
 </script>	
  <script type="text/ecmascript">   
       function ReceiveServerData(rValue)
        {
               
                var j=rValue.split('~');
               
                if(j[0]=='Clients')
                {
                    document.getElementById('HiddenField_Client').value = j[1];
                } 
                if(j[0]=='Branch')
                {
                    document.getElementById('HiddenField_Branch').value = j[1];
                }
                if(j[0]=='Group')
                {
                    document.getElementById('HiddenField_Group').value = j[1];
                    btn.click();
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

          function DateChangeatPageLoad(DateObj) {
              var FYS = '<%=Session["FinYearStart"]%>';
       var FYE = '<%=Session["FinYearEnd"]%>';
       var LFY = '<%=Session["LastFinYear"]%>';
       DevE_CheckForFinYearWithoutAlert(DateObj, FYS, FYE, LFY);


   }
   function DateChange(DateObj) {
       //var Lck ='<%=Session["LCKBNK"] %>';
       var FYS = '<%=Session["FinYearStart"]%>';
       var FYE = '<%=Session["FinYearEnd"]%>';
       var LFY = '<%=Session["LastFinYear"]%>';
       //DevE_CheckForLockDate(DateObj,Lck);

       DevE_CheckForFinYear(DateObj, FYS, FYE, LFY);
   }
   function DateChangeUnlimited(DateObj) {
       var FYS = '<%=Session["FinYearStart"]%>';
       //        var FYE ='<%=Session["FinYearEnd"]%>';
       var FYE = '3/31/9999';
       var LFY = '<%=Session["LastFinYear"]%>';
       //DevE_CheckForLockDate(DateObj,Lck);        
        DevE_CheckForFinYear(DateObj, FYS, FYE, LFY);
    }
    function DateChangeforExpiryRangeFrom(DateObj) {
        var targetdate = DateObj.GetDate();
        cdtrangefrom.SetDate(targetdate);
    }

    function DateChangeforExpiryRangeTo(DateObj) {
        var targetdate = DateObj.GetDate();
        cdtrangeto.SetDate(targetdate);

    }
            </script>
</asp:Content>
          <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" colspan="0" style="text-align: center; height: 20px;">
                    <strong><span id="SpanHeader" style="color: #000099">Monthly Performance Report</span></strong>
                </td>
                <td class="EHEADER" width="15%" id="tr_filter" style="height: 22px">
                    <a href="javascript:void(0);" onclick="Filter();"><span style="color: Blue; text-decoration: underline;
                        font-size: 8pt; font-weight: bold">Filter</span></a>
                    <asp:DropDownList ID="cmbExport" runat="server" AutoPostBack="True" Font-Size="12px" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" >
                        <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                        <asp:ListItem Value="E">Excel</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table id="tab2" border="0" cellpadding="0" cellspacing="0">
            <tr valign="top">
                <td>
                    <table>
                        <tr valign="top">
                            <td>
                                <table>
                                    <tr>
                                        <td class="gridcellleft">
                                            <asp:DropDownList ID="ddldate" runat="server" Width="100px" Font-Size="12px" onchange="fn_ddldateChange(this.value)">
                                                <asp:ListItem Value="0">As On Date</asp:ListItem>
                                                <asp:ListItem Value="1">For a Period</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td id="td_dtfor" class="gridcellleft">
                                            <dxe:ASPxDateEdit ID="dtfor" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="108px" ClientInstanceName="cdtfor">
                                                <dropdownbutton text="For">
                                        </dropdownbutton>
                                            <ClientSideEvents DateChanged="function(s,e){DateChange(cdtfor);DateChangeatPageLoad(cdtfor);DevE_SetFirstDayOfMonth(cdtfor,cdtrangefrom); DevE_SetLastDayOfMonth(cdtfor,cdtrangeto);}"/>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td id="td_dtfrom" class="gridcellleft">
                                            <dxe:ASPxDateEdit ID="dtfrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="108px" ClientInstanceName="cdtfrom">
                                                <dropdownbutton text="From">
                                        </dropdownbutton>
                                        <ClientSideEvents DateChanged="function(s,e){DateChange(cdtfrom);DateChangeatPageLoad(cdtfrom);DateChangeforExpiryRangeFrom(cdtfrom);}"/>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td id="td_dtto" class="gridcellleft">
                                            <dxe:ASPxDateEdit ID="dtto" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="108px" ClientInstanceName="cdtto">
                                                <dropdownbutton text="To">
                                        </dropdownbutton>
                                          <ClientSideEvents DateChanged="function(s,e){DateChange(cdtto);DateChangeatPageLoad(cdtto);DevE_CompareDateForMin(cdtfrom,cdtto,'Todate Can Not Be Less than FromDate!!!');DateChangeforExpiryRangeTo(cdtto);}"/>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td  class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Group By</td>
                                        <td>
                                            <asp:DropDownList ID="ddlGroup" runat="server" Width="80px" Font-Size="12px" onchange="fnddlGroup(this.value)">
                                                <asp:ListItem Value="0">Branch</asp:ListItem>
                                                <asp:ListItem Value="1">Group</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="2" id="td_branch">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rdbranchAll" runat="server" Checked="True" GroupName="a" onclick="Branch('a')" />
                                                        All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbranchSelected" runat="server" GroupName="a" onclick="Branch('b')" />Selected
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
                                                            onclick="Group('a')" />
                                                        All
                                                        <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="b" onclick="Group('b')" />Selected
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Clients :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbClientALL" runat="server" Checked="True" GroupName="c" onclick="Clients('a')" />
                                            All Client
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="radPOAClient" runat="server" GroupName="c" onclick="Clients('a')" />POA
                                            Client
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbClientSelected" runat="server" GroupName="c" onclick="Clients('b')" />
                                            Selected Client
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" style="width: 100px;" bgcolor="#B7CEEC">
                                            Asset:</td>
                                        <td>
                                            <asp:RadioButton ID="rdbunderlyingall" runat="server" Checked="True" GroupName="d"
                                                onclick="fnunderlying('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbunderlyingselected" runat="server" GroupName="d" onclick="fnunderlying('b')" />Specific
                                            Asset
                                        </td>
                                        <td style="display: none;" id="td_underlying">
                                            <asp:TextBox runat="server" Width="250px" Font-Size="12px" ID="txtunderlying" onkeyup="ajax_showOptions(this,'Searchproductandeffectuntil',event,'Product')"></asp:TextBox>
                                            <asp:TextBox ID="txtunderlying_hidden" runat="server" Width="14px" Style="display: none;"> </asp:TextBox></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td  class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" style="width: 100px;" bgcolor="#B7CEEC">
                                            Expiry :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbExpiryAll" runat="server" Checked="True" GroupName="e" onclick="fnExpiry('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbExpirySelected" runat="server" GroupName="e" onclick="fnExpiry('b')" />Specific
                                            Expiry
                                        </td>
                                        <td style="display: none;" id="td_Expiry">
                                            <asp:TextBox runat="server" Width="250px" Font-Size="12px" ID="txtExpiry" onkeyup="fnexpirtycallajax(this,'Searchproductandeffectuntil',event)"></asp:TextBox>
                                            <asp:TextBox ID="txtExpiry_hidden" runat="server" Width="14px" Style="display: none;"> </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbExpiryRange" runat="server" GroupName="e" onclick="fnExpiry('c')" />Specific
                                            Expiry Range
                                        </td>
                                       
                                        <td style="display: none;" id="td_RangeFrom" class="gridcellleft">
                                            <dxe:ASPxDateEdit ID="dtrangefrom" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True"
                                                Font-Size="12px" Width="108px" ClientInstanceName="cdtrangefrom">
                                                <dropdownbutton text="From">
                                                </dropdownbutton>
                                                <ClientSideEvents DateChanged="function(s,e){DateChange(cdtrangefrom);}"/>                                             
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td style="display: none;" id="td_RangeTo" class="gridcellleft">
                                            <dxe:ASPxDateEdit ID="dtrangeto" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True"
                                                Font-Size="12px" Width="108px" ClientInstanceName="cdtrangeto">
                                                <dropdownbutton text="To">
                                        </dropdownbutton>
                                        <ClientSideEvents DateChanged="function(s,e){DateChangeUnlimited(cdtrangeto); DevE_CompareDateForMin(cdtrangefrom,cdtrangeto,'Expiry RangeTo Can Not Be Less Than Expiry Range From!!!')}"/>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft" >
                                <asp:CheckBox ID="chkoptmtm" runat="server" />
                                Consider MTM On Option Contracts
                            </td>
                        </tr>
                        <tr>
                            <td >
                                <table>
                                    <tr>
                                        <td id="Td3" class="gridcellleft">
                                            <asp:RadioButton ID="rbScreen" runat="server" GroupName="f" Checked="True" onclick="RBShowHide(this.value)" />Screen
                                        </td>
                                        <td id="Td4" class="gridcellleft">
                                            <asp:RadioButton ID="rbPrint" runat="server" GroupName="f" onclick="RBShowHide(this.value)" />Print
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                          <td><table><tr>  <td  id="td_btnshow" class="gridcellleft">
                                <asp:Button ID="btnshow" runat="server" CssClass="btnUpdate" Height="20px" Text="Show"
                                    OnClientClick="selecttion()" Width="101px" OnClick="btnshow_Click" />
                            </td>
                            <td id="td_btnprint">
                                <asp:Button ID="btnprint" runat="server" CssClass="btnUpdate" Height="20px" Text="Print"
                                    OnClientClick="selecttion()" Width="101px" OnClick="btnprint_Click" />
                            </td></tr></table></td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table width="100%" id="showFilter">
                        <tr>
                            <td style="text-align: right; vertical-align: top; height: 134px;">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="gridcellleft" style="vertical-align: top; text-align: right; height: 21px;"
                                            id="TdFilter">
                                            <span id="spanunder"></span><span id="spanclient"></span>
                                            <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox>
                                            <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                Enabled="false">
                                                <asp:ListItem>Clients</asp:ListItem>
                                                <asp:ListItem>Branch</asp:ListItem>
                                                <asp:ListItem>Group</asp:ListItem>
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
                </td>
            </tr>
        </table>
       
                                       <table><tr><td style="display: none;">
                                                               <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>

                        <asp:Button ID="btnhide" runat="server" Text="Button"  OnClick="btnhide_Click"/>

                    </td><td>
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
                                                            <font size='2' face='Tahoma'><strong align='center'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Loading..</strong></font></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td></tr></table>
                    
        <div id="displayAll" style="display: none;">
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <table width="100%" border="1">
                        <tr style="display: none;">
                            <td>
                                <asp:DropDownList ID="cmbclient" runat="server" Font-Size="12px" Width="300px" AutoPostBack="True">
                                </asp:DropDownList><asp:HiddenField ID="hiddencount" runat="server" />
                                 <asp:HiddenField ID="HiddenField_Group" runat="server" />
                    <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                    <asp:HiddenField ID="HiddenField_Client" runat="server" />
                            </td>
                        </tr>
                        <tr id="tr_DIVdisplayPERIOD">
                            <td>
                                <div id="DIVdisplayPERIOD" runat="server">
                                </div>
                            </td>
                        </tr>
                        <tr id="tr_cmbsubbroker">
                            <td>
                                <asp:UpdatePanel ID="updatepanel_trprevnext" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="brokerFilter" runat="server">
                                            <tr valign="top">
                                                <td style="height: 44px">
                                                    <asp:LinkButton ID="lnkPrevClient" runat="server" CommandName="Prev" Text="[Prev]"
                                                        OnCommand="NavigationLinkC_Click" OnClientClick="javascript:selecttion();"> </asp:LinkButton>
                                                </td>
                                                <td style="height: 44px">
                                                    <asp:DropDownList ID="cmbsubbroker" runat="server" Font-Size="12px" Width="300px"
                                                        AutoPostBack="True" OnSelectedIndexChanged="cmbsubbroker_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="height: 44px">
                                                    <asp:LinkButton ID="lnkNextClient" runat="server" CommandName="Next" Text="[Next]"
                                                        OnCommand="NavigationLinkC_Click" OnClientClick="javascript:selecttion();"> </asp:LinkButton>&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnshow" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr bordercolor="Blue" id="tr_prvnxt">
                            <td align="center">
                                <table id="tblpage" cellspacing="0" cellpadding="0" runat="server" width="100%">
                                    <tr>
                                        <td width="20" style="padding: 5px">
                                            <asp:LinkButton ID="ASPxFirst" runat="server" Font-Bold="True" ForeColor="maroon"
                                                OnClientClick="javascript:selecttion();;" OnClick="ASPxFirst_Click">First</asp:LinkButton></td>
                                        <td width="25">
                                            <asp:LinkButton ID="ASPxPrevious" runat="server" Font-Bold="True" ForeColor="Blue"
                                                OnClientClick="javascript:selecttion();" OnClick="ASPxPrevious_Click">Previous</asp:LinkButton></td>
                                        <td width="20" style="padding: 5px">
                                            <asp:LinkButton ID="ASPxNext" runat="server" Font-Bold="True" ForeColor="maroon"
                                                OnClientClick="javascript:selecttion();" OnClick="ASPxNext_Click">Next</asp:LinkButton></td>
                                        <td width="20">
                                            <asp:LinkButton ID="ASPxLast" runat="server" Font-Bold="True" ForeColor="Blue" OnClientClick="javascript:selecttion();"
                                                OnClick="ASPxLast_Click">Last</asp:LinkButton></td>
                                        <td align="right">
                                            <asp:Label ID="listRecord" runat="server" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="display" runat="server">
                                </div>
                            </td>
                        </tr>
                        <asp:HiddenField ID="Totalbroker" runat="server" />
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnshow" EventName="Click"></asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
  </asp:Content>