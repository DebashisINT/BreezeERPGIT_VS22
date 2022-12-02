<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_cdslHolding" Codebehind="frmReport_cdslHolding.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>

    <style type="text/css">
	
	/* Big box with list of options */
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
    groupvalue="";
      function ForFilterOff()
        {
            hide('filter');  
           show('btnfilter');
        
//            document.getElementById("TrAll").style.display='none';
//            document.getElementById("TdAll1").style.display='none';
//            document.getElementById("TrBtn").style.display='none';
//            document.getElementById('spanBtn').style.display='none';
            height();
        }
   function MailsendT()
    {
    alert("Mail Sent Successfully");
    }
   function MailsendF()
    {
    alert("Error on sending!Try again..");
    }
    function SignOff()
    {
        window.parent.SignOff();
    }
    
   function EndCall(obj)
    {
        height();
    }
   function SignOff()
    {
        window.parent.SignOff();
    }
    function height()
    {
        if(document.body.scrollHeight>=500)
            window.frameElement.height = document.body.scrollHeight;
        else
            window.frameElement.height = '500px';
        window.frameElement.Width = document.body.scrollWidth;
    }
    
    function PageLoad()
    {
        FieldName='SelectionList';
        document.getElementById('txtName_hidden').style.display="none";
        document.getElementById('txtISIN_hidden').style.display="none";
        document.getElementById('txtSettlement_hidden').style.display="none";
        ShowEmployeeFilterForm('A');
        ShowISINFilterForm('A');
        ShowSettlementFilterForm('A');
        hide('btnfilter');
         hide('ShowSelectUser');
       document.getElementById('showFilter1').style.display="none";
    } 
    function ShowISINFilterForm(obj)
    {
        document.getElementById('txtISIN_hidden').value="";
        if(obj=='A')
        {
            hide('tdisinValue');
            hide('tdisin');
            document.getElementById('txtISIN_hidden').style.display="none";
             document.getElementById('txtISIN').value="";
        }
        if(obj=='S')
        {
            show('tdisinValue');
            show('tdisin');
            document.getElementById('txtISIN_hidden').style.display="none";
        }
    }
    function ShowEmployeeFilterForm(obj)
    {
        document.getElementById('txtName_hidden').value="";
        if(obj=='A')
        {
            hide('txttdName');
            hide('tdname');
            document.getElementById('txtName_hidden').style.display="none";
            document.getElementById('txtName').value="";
        }
        if(obj=='S')
        {
           var cmb=document.getElementById('cmbsearch');
            cmb.value='Clients';
            show('txttdName');
            show('tdname');
            document.getElementById('txtName_hidden').style.display="none";
             document.getElementById('txtName').focus();
        }
    }
    function NoOfRows(obj)
    {
        //alert(obj);
        Noofrows=obj;
        document.getElementById('txtName_hidden').style.display="none";
    }
    function show(obj1)
    {
        //alert(obj1);
         document.getElementById(obj1).style.display='inline';
    }
    function hide(obj1)
    {
        //alert(obj1);
         document.getElementById(obj1).style.display='none';
    }
   FieldName='SelectionList';
   function CallAjax(obj1,obj2,obj3)
    {
        //var cmbTime=document.getElementById('cmbTime_VI');
        //var cmbType=document.getElementById('ASPxComboBox1_VI');
        var Client=document.getElementById('txtName_hidden');
        var isin=document.getElementById('txtISIN_hidden');
        var group=document.getElementById('ddlGroup');
        var obj4=dateCombo.GetValue()+'~'+ClientType.GetValue()+'~'+Client.value+'~'+isin.value+'~'+group.value;
        ajax_showOptions(obj1,obj2,obj3,obj4 );
    } 
    function OnDateChanged() 
        {   
            dateCombo.PerformCallback();
        }
        
   function OnClientTypeChanged(s,e)
    {
        document.getElementById('txtSettlement_hidden').value="";
        document.getElementById('txtSettlement').value=""; 
        var item=s.GetSelectedItem(); 
        if (item.text=='Clearing Member' || item.text=='All')
            {
                show('tdSettlementLabel');
                show('tdrbSettlement');
                ShowSettlementFilterForm('A');
            }
        else
            {               
                hide('tdSettlementLabel');
                hide('tdrbSettlement');
                hide('tdtxtSettlement');
               
            }
    } 
  function ShowSettlementFilterForm(obj)
    { 
       document.getElementById('txtSettlement_hidden').value="";
       show('tdSettlementLabel');
       if(obj=='A')
        {
            hide('tdtxtSettlement');
            document.getElementById('txtSettlement_hidden').style.display="none";
            document.getElementById('txtSettlement').value=""; 
        }
        if(obj=='S')
        {
            show('tdtxtSettlement');
           
            document.getElementById('txtSettlement_hidden').style.display="none";
        }
    } 
  function ShowHideFilter(obj)
    {
       grid.PerformCallback(obj);
    }
        



     function btnAddEmailtolist_click()
            {
            
                var cmb=document.getElementById('cmbsearch');
            
                    var userid = document.getElementById('txtName');
                    if(userid.value != '')
                    {
                        var ids = document.getElementById('txtName_hidden');
                        var listBox = document.getElementById('SelectionList');
                        var tLength = listBox.length;
                       
                        
                        var no = new Option();
                        no.value = ids.value;
                        no.text = userid.value;
                        listBox[tLength]=no;
                        var recipient = document.getElementById('txtName');
                        recipient.value='';
                    }
                    else
                        alert('Please search name and then Add!')
                    var s=document.getElementById('txtName');
                    s.focus();
                    s.select();

            }
    
        
   function callAjax1(obj1,obj2,obj3)
    {
     document.getElementById('SelectionList').style.display='none';
        var combo = document.getElementById("cmbsearch");
        var set_value = combo.value
        var obj4='Main';
       
        if (set_value=='16')
        {
            ajax_showOptions(obj1,'GetLeadId',obj3,set_value,obj4)
        }
        else
        {
         
            ajax_showOptions(obj1,obj2,obj3,set_value,obj4)	  
        }
        
    }
    
       function clientselection()
	        {
	           selecttion();
	            var listBoxSubs = document.getElementById('SelectionList');
	         
                var cmb=document.getElementById('cmbsearch');
              
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
                    
                    CallServer1(sendData,"");
                    
                 document.getElementById('txttdname').style.display='none';
                 
                }
                else
                {
                alert("Please select email from list.")
                }
               
	            var i;
                for(i=listBoxSubs.options.length-1;i>=0;i--)
                {
                    listBoxSubs.remove(i);
                 }
                   if(cmb.value=="User")
                 {
                 document.getElementById('showFilter1').style.display="inline";
                 document.getElementById('ShowSelectUser').style.display="none";
                 }
                 height();
           
                
	        }
	        
	   function btnRemoveEmailFromlist_click()
            {
                selecttion();
                var listBox = document.getElementById('SelectionList');
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
            
      function ReceiveSvrData(rValue)
        {            
            var Data=rValue.split('~');
            if(Data[0]=='Clients')
            {
                document.getElementById('hidClients').value = Data[1];
            }
            else if(Data[0]=='Branch')
            {
                document.getElementById('hidBranch').value = Data[1];
            }
            else if(Data[0]=='Group')
            {
                document.getElementById('hidGroup').value = Data[1];
            }
            else if(Data[0]=='User')
            {
                
            }
        }
           function selecttion()
        {            
            var combo=document.getElementById('ddlExport');
            combo.value='Ex';
        }
        
          //-------------Select Groupwise and branchwise
        
        
    function  fnddlGroup(obj)
   {
        if(obj=="0")
        {
        document.getElementById('td_group').style.display="none";
        document.getElementById('td_branch').style.display="inline";
        document.getElementById('ddlgrouptype').value="";

        }
        else
        {
            document.getElementById('td_group').style.display="inline";
            document.getElementById('td_branch').style.display="none";
            var btn = document.getElementById('btnhide');
            btn.click();
        }
   } 
   
   
     function Branch(obj)
      {
            if(obj=="a")
            {

                hide('txttdname');
                hide('tdname');
                document.getElementById('txtName_hidden').style.display="none";

            }
             else
             {
                  var cmb=document.getElementById('cmbsearch');
                  cmb.value='Branch';
                  show('txttdname');
                  show('tdname');
                  document.getElementById('txtName_hidden').style.display="none";
                  document.getElementById('txtName').focus();

             }
              
      }
   
     function Group(obj)
      {
      
            if(obj=="a")
            {

            hide('txttdname');
            hide('tdname');
            document.getElementById('txtName_hidden').style.display="none";

             }
             else
             {
             
                  var cmb=document.getElementById('cmbsearch');
                  cmb.value='Group';
                 document.getElementById('txttdname').style.display="inline";
                  document.getElementById('tdname').style.display="inline";
                  document.getElementById('txtName_hidden').style.display="none";
                  document.getElementById('txtName').focus();

             }
           
      }
    function fngrouptype(obj)
   {
       if(obj=="0")
       {
       document.getElementById('td_allselect').style.display="none";
       // Hide('td_allselect');
        alert('Please Select Group Type !');
       }
       else
       {
         document.getElementById('td_allselect').style.display="inline";
       // Show('td_allselect');
       }
   }
   
      
     function FunClientScrip(objID,objListFun,objEvent)
        {
          if(document.getElementById('cmbsearch').value=="User")
          { 
           ajax_showOptions(objID,'GetMailId',objEvent,'EM');
          }
          else
          {
          
//          var Client=document.getElementById('txtName_hidden');
//        var isin=document.getElementById('txtISIN_hidden');
//        var obj4=dateCombo.GetValue()+'~'+ClientType.GetValue()+'~'+Client.value+'~'+isin.value;
//        ajax_showOptions(obj1,obj2,obj3,obj4 );
//          
          
//                var cmbVal;
//                var cmbTime=document.getElementById('txtDate_I');
//                var cmbType=document.getElementById('ASPxComboBox1_VI');
//                var Client=document.getElementById('txtName_hidden');
//                var isin=document.getElementById('txtISIN_hidden');
//                var GtCL=cmbTime.value+'~'+cmbType.value+'~'+Client.value+'~'+isin.value

         
                var cmbVal;
                var cmbTime=document.getElementById('txtDate_I');
                var cmbType=document.getElementById('ASPxComboBox1_VI');
                var Client=document.getElementById('txtName_hidden');
                var isin=document.getElementById('txtISIN_hidden');
                var GtCL=dateCombo.GetValue()+'~'+ClientType.GetValue()+'~'+Client.value+'~'+isin.value
                
//                 cmbVal=document.getElementById('cmbsearch').value;
//                           cmbVal=cmbVal+'~'+ GtCL + document.getElementById('ddlgrouptype').value;
//                
               
                
                  if(groupvalue=="")
                  {
                           if(document.getElementById('cmbsearch').value=="Clients")
                           {
                           cmbVal=document.getElementById('cmbsearch').value;
                           cmbVal=cmbVal+'~'+ GtCL + document.getElementById('ddlgrouptype').value;
                                                     
                           }
                          else
                           {
                           cmbVal=document.getElementById('cmbsearch').value;
                           cmbVal=cmbVal+'~'+document.getElementById('ddlgrouptype').value;
                          
                           }
                  }
                  else
                  { 
                    if(document.getElementById('cmbsearch').value=="Clients")
                    {
                       if(document.getElementById('ddlGroup').value=="0")//////////////Group By  selected are branch
                        {
                           if(document.getElementById('rdbranchAll').checked==true)
                              {                    
                                  cmbVal=document.getElementById('cmbsearch').value+'Branch';
                                  cmbVal=cmbVal+'~'+ GtCL +'ALL'+'~'+document.getElementById('ddlgrouptype').value;
                              }
                              else
                              {
                                  cmbVal=document.getElementById('cmbsearch').value+'Branch';
                                  cmbVal=cmbVal+'~'+ GtCL +'Selected'+'~'+groupvalue;
                                          
                              }
                          }
                           else //////////////Group By selected are Group
                              {
                               if(document.getElementById('rdddlgrouptypeAll').checked==true)
                                  {
                                    cmbVal=document.getElementById('cmbsearch').value+'Group';
                                    cmbVal=cmbVal+'~'+ GtCL +'ALL'+'~'+document.getElementById('ddlgrouptype').value;
                                  }
                                   else
                                  {
                                    cmbVal=document.getElementById('cmbsearch').value+'Group';
                                    cmbVal=cmbVal+'~'+ GtCL +'Selected'+'~'+groupvalue; 
                                 }
                             }
                    }
                    else
                    {
                        cmbVal=document.getElementById('cmbsearch').value;
                        cmbVal=cmbVal+'~'+document.getElementById('ddlgrouptype').value;
                    }
                  }
                 
                
                 
                  ajax_showOptions(objID,objListFun,objEvent,cmbVal);
          }

    }  
        
      ///-----------Email New
      
       function SelectUserClient(obj)
         {
            if(obj=='Client')
            {
                      
                document.getElementById('ShowSelectUser').style.display='inline';
                document.getElementById('ShowTable').style.display='none';
                document.getElementById('showFilter1').style.display='inline';
                window.frameElement.height = document.body.scrollHeight;
            
            }
           else if(obj=='User')
            {
                  document.getElementById('ShowTable').style.display='inline';
                  document.getElementById('ShowSelectUser').style.display='inline';
                  document.getElementById('showFilter1').style.display='none';
                  window.frameElement.height = document.body.scrollHeight;
            }
        }          	
      
     function Sendmail() 
        {
        
       
              document.getElementById('ShowSelectUser').style.display='inline';
              if(document.getElementById('rbClientUser').checked==true)
                  {
                   document.getElementById('showFilter1').style.display="none";
                  }
                  else
                  {
                   document.getElementById('showFilter1').style.display="inline";
                  }
                if(document.getElementById('ddlGroup').value=="0")
                 {
                       if(document.getElementById('rdbranchAll').checked==true)
                       {
                       document.getElementById('tdBrGr').style.display="none";
                       document.getElementById('tdCl').style.display="inline";
                       document.getElementById('tdSU').style.display="inline";
                       
                       }
                       else
                       {
                       document.getElementById('tdBrGr').style.display="inline";
                       document.getElementById('tdCl').style.display="inline";
                       document.getElementById('tdSU').style.display="inline";
                        document.getElementById('lblSelectBrCl').value='Respective Branch';
                       }
                       
                       
        
                 }
                 else if(document.getElementById('ddlGroup').value=="1")
                 { 
                     if(document.getElementById('rdddlgrouptypeAll').checked==true)
                       {
                       document.getElementById('tdBrGr').style.display="none";
                       document.getElementById('tdCl').style.display="inline";
                       document.getElementById('tdSU').style.display="inline";                           
                       }
                       else
                       {
                       document.getElementById('tdBrGr').style.display="inline";
                       document.getElementById('tdCl').style.display="inline";
                       document.getElementById('tdSU').style.display="inline";
                       document.getElementById('lblSelectBrCl').value='Respective Group';                       
                       }
                  } 
            
        }  
        
        
      function User(obj)
      {
            if(obj=="User")
            {
                  var cmb=document.getElementById('cmbsearch');
                  cmb.value='User';
                  show('txttdname');
                  show('tdname');
                  document.getElementById('txtName_hidden').style.display="none";
                  document.getElementById('txtName').focus();
                  document.getElementById('showFilter1').style.display="none";
           }
          else
           {
                  document.getElementById('txttdname').style.display="none";
                  document.getElementById('tdname').style.display="none";
                  document.getElementById('showFilter1').style.display="inline";
           
           }
            
            height();
     }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>
         <asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server"
                AsyncPostBackTimeout="3600">
                </asp:ScriptManager>
            <table class="TableMain100">
                <tr>
                    <td class="EHEADER" colspan="0" style="text-align: center;">
                        <strong><span style="color: #000099">CDSL Holding Report</span></strong>
                    </td>
                    <td width="35%" id="btnfilter">
                        <asp:DropDownList ID="ddlExport" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged1">
                            <asp:ListItem Value="Ex" Selected="True">Export</asp:ListItem>
                            <asp:ListItem Value="E">Excel</asp:ListItem>
                            <asp:ListItem Value="P">PDF</asp:ListItem>
                            <asp:ListItem Value="PM">PDF DIFF PAGES</asp:ListItem>
                        </asp:DropDownList>||
                        <input id="Button1" type="button" value="Show Filter" class="btnUpdate" onclick="javascript: show('filter');hide('btnfilter');"
                            style="width: 66px; height: 19px" />
                        ||
                        <%--<asp:LinkButton ID="btnEmail" runat="server" Font-Bold="True" Font-Underline="True"
                            ForeColor="Blue" OnClick="btnEmail_Click">SendEmail</asp:LinkButton>--%>
                            <a href="javascript:void(0);" onclick="Sendmail();"><span style="color: Blue; text-decoration: underline;
                            font-size: 8pt; font-weight: bold">Send Email</span></a>
                    </td>
                </tr>
            </table>
                        <table id="ShowSelectUser">
                <tr>
                    <td id="tdBrGr">
                        <table>
                            <tr>
                                <td valign="top">
                                    <asp:RadioButton ID="rbOnlyClient" runat="server" Checked="true" GroupName="h" />
                                </td>
                                <td valign="top">
                                    <%-- <asp:Label ID="lblSelectBrCl" runat="server" Text=""></asp:Label>--%>
                                    <asp:TextBox ID="lblSelectBrCl" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td id="tdCl">
                        <table>
                            <tr>
                                <td valign="top">
                                    <asp:RadioButton ID="rbRspctvClient" runat="server" Checked="true" GroupName="h" />
                                </td>
                                <td valign="top">
                                    <%-- <asp:Label ID="lblSelectBrCl" runat="server" Text=""></asp:Label>--%>
                                    Respective Client
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td id="tdSU">
                        <table>
                            <tr>
                                <td valign="top">
                                    <asp:RadioButton ID="rbClientUser" runat="server" GroupName="h" />
                                </td>
                                <td valign="top">
                                    Selected User
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
             <table id="showFilter1">
                <tr>
                    <td>
                        <asp:Button ID="Button2" runat="server" OnClick="Button1_Click" CssClass="btnUpdate"
                            Text="Send" OnClientClick="javascript:selecttion();" />
                    </td>
                </tr>
            </table>
           
            <table border="0" class="TableMain" cellpadding="0" cellspacing="0" style="width: 100%;
                padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;">
                <tr>
                    <td style="vertical-align: top; text-align: left; width: 100%;">
                     <table><tr><td>
                        <table border="0" cellpadding="0" cellspacing="0" class="gridcellleft" id="filter">
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <dxe:ASPxDateEdit ID="txtDate" runat="server" ClientInstanceName="e1" Width="130px"
                                                    EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" AllowNull="False"
                                                    Font-Size="12px">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                    <ClientSideEvents DateChanged="function(s, e) { OnDateChanged(); }" />
                                                    <Paddings PaddingBottom="0px"></Paddings>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cmbTime" Width="120px" ClientInstanceName="dateCombo" runat="server"
                                                    Font-Size="12px" ValueType="System.String" Font-Bold="False" OnCallback="cmbTime_Callback">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                    <DropDownButton Text="Time">
                                                    </DropDownButton>
                                                    <Paddings PaddingBottom="0px"></Paddings>
                                                </dxe:ASPxComboBox>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="ASPxComboBox1" Width="160px" ClientInstanceName="ClientType"
                                                    runat="server" Font-Size="12px" ValueType="System.String" Font-Bold="False" SelectedIndex="0">
                                                    <Items>
                                                        <dxe:ListEditItem Text="All" Value="All"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="Clearing Member" Value="Clearing Member"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="Corporate" Value="Corporate"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="Corporate POA" Value="Corporate POA"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="Individual" Value="Individual"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="Individual POA" Value="Individual POA"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="NRI" Value="NRI"></dxe:ListEditItem>
                                                    </Items>
                                                    <ButtonStyle Width="60px">
                                                    </ButtonStyle>
                                                    <DropDownButton Text="Client Type">
                                                    </DropDownButton>
                                                    <ClientSideEvents SelectedIndexChanged="OnClientTypeChanged" />
                                                    <Paddings PaddingBottom="0px"></Paddings>
                                                </dxe:ASPxComboBox>
                                            </td>
                                          
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                            <td>
                             <table>
                                        <tr>
                                            <td>
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
                                                            <asp:RadioButton ID="rdbranchAll" runat="server" Checked="True" GroupName="cd" onclick="Branch('a')" />
                                                            All
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbranchSelected" runat="server" GroupName="cd" onclick="Branch('b')" />Selected
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td id="td_group" style="display: none;" colspan="2">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
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
                                                            <asp:RadioButton ID="rdddlgrouptypeAll" runat="server" Checked="True" GroupName="ef"
                                                                onclick="Group('a')" />
                                                            All
                                                            <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="ef" onclick="Group('b')" />Selected
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                            </td>
                            </tr>

                     
                            
                            
                            <tr>
                                <td>
                                    <table>
                                    <tr>                                      <td>
                                                <span class="Ecoheadtxt" style="color: blue"><strong>Client:</strong></span>
                                            </td>
                                            <td>
                                                <dxe:ASPxRadioButtonList ID="rbUser" runat="server" SelectedIndex="0" ItemSpacing="10px"
                                                    RepeatDirection="Horizontal" TextWrap="False" Font-Size="12px">
                                                    <Items>
                                                        <dxe:ListEditItem Text="All" Value="A" />
                                                        <dxe:ListEditItem Text="Selected" Value="S" />
                                                    </Items>
                                                    <ClientSideEvents ValueChanged="function(s, e) {ShowEmployeeFilterForm(s.GetValue());}" />
                                                    <Border BorderWidth="0px" />
                                                </dxe:ASPxRadioButtonList>
                                            </td>
                                            </tr>
                                    </table>
                                </td>
                            </tr>
                            
                            
                            
 
                           <tr>
                                <td>
                                    <table>
                                    <tr>
                                     <td>
                                                <span class="Ecoheadtxt" style="color: blue"><strong>ISIN:</strong></span>
                                            </td>
                                            <td>
                                                <dxe:ASPxRadioButtonList ID="rbISIN" runat="server" SelectedIndex="0" ItemSpacing="10px"
                                                    RepeatDirection="Horizontal" TextWrap="False" Font-Size="12px">
                                                    <Items>
                                                        <dxe:ListEditItem Text="All" Value="A" />
                                                        <dxe:ListEditItem Text="Specific" Value="S" />
                                                    </Items>
                                                    <ClientSideEvents ValueChanged="function(s, e) {ShowISINFilterForm(s.GetValue());}" />
                                                    <Border BorderWidth="0px" />
                                                </dxe:ASPxRadioButtonList>
                                            </td>
                                            <td id="tdisin">
                                                <span class="Ecoheadtxt" style="color: blue"><strong>Value:</strong></span>
                                            </td>
                                            <td id="tdisinValue">
                                                <asp:TextBox ID="txtISIN_hidden" runat="server" Width="14px"></asp:TextBox>
                                                <asp:TextBox ID="txtISIN" runat="server" Width="190px" Font-Size="11px" Height="20px"></asp:TextBox>
                                            </td>
                                            </tr>
                                    </table>
                                </td>
                               
                            </tr>
                            
                            
                            
                            
                            <tr>
                                <td>
                                    <table>
                                    <tr>
                                     <td id="tdSettlementLabel">
                                                <span class="Ecoheadtxt" style="color: blue"><strong>Settlement No.:</strong></span></td>
                                            <td id="tdrbSettlement">
                                                <dxe:ASPxRadioButtonList ID="rbSettlement" runat="server" SelectedIndex="0" ItemSpacing="10px"
                                                    RepeatDirection="Horizontal" TextWrap="False" Font-Size="12px" ClientInstanceName="SettClid">
                                                    <Items>
                                                        <dxe:ListEditItem Text="All" Value="A" />
                                                        <dxe:ListEditItem Text="Specific" Value="S" />
                                                    </Items>
                                                    <ClientSideEvents ValueChanged="function(s, e)  {ShowSettlementFilterForm(s.GetValue());}" />
                                                    <Border BorderWidth="0px" />
                                                </dxe:ASPxRadioButtonList>
                                            </td>
                                            <td id="tdtxtSettlement">
                                                <asp:TextBox ID="txtSettlement_hidden" runat="server" Width="14px"></asp:TextBox>
                                                <asp:TextBox ID="txtSettlement" runat="server" Width="190px" Font-Size="11px" Height="20px"></asp:TextBox></td>
                                                </tr>
                                    </table>
                                </td>
                            </tr>
                            
                            
                            
                            <tr>
                                <td>
                                    <table>
                                    <tr>
                                    <td>
                                                <span class="Ecoheadtxt" style="color: blue"><strong>Report Type:</strong></span></td>
                                            <td>
                                                <dxe:ASPxRadioButtonList ID="RBReportType" runat="server" CssPostfix="BlackGlass"
                                                    Height="2px" RepeatDirection="Horizontal" SelectedIndex="0" TextSpacing="3px">
                                                    <Paddings PaddingBottom="0px"></Paddings>
                                                    <ValidationSettings ErrorText="Error has occurred">
                                                        <ErrorImage Height="14px" Width="14px"></ErrorImage>
                                                    </ValidationSettings>
                                                    <Items>
                                                        <dxe:ListEditItem Text="Screen" Value="Screen"></dxe:ListEditItem>
                                                        <dxe:ListEditItem Text="Print" Value="Print"></dxe:ListEditItem>
                                                    </Items>
                                                </dxe:ASPxRadioButtonList>
                                            </td>
                                            <td>
                                                <dxe:ASPxButton ID="btnShow" runat="server" OnClick="btnShow_Click" Text="Show" ValidationGroup="a">
                                                    <ClientSideEvents Click="function(s, e) {
                                                                hide('filter');
                                                                  show('btnfilter');
                                                                }" />
                                                </dxe:ASPxButton>
                                            </td>
                                            </tr>
                                    </table>
                                </td>
                            </tr>
                            

                            
                       
                            
                            
                            <tr>
                                <td colspan="6" width="100%">
                                </td>
                            </tr>
                        </table>
                        </td><td valign="top">                                 <table id="txttdname">
                                                <%--<table id="txttdname">--%>
                        
                                        <tr>
                                            <td id="tdname" valign="top">
                                                <span class="Ecoheadtxt" style="color: blue"><strong></strong></span>
                                            </td>
                                            <%-- <td id="txttdname">
                                                            <asp:TextBox ID="txtName_hidden" runat="server" Width="14px"></asp:TextBox>
                                                            <asp:TextBox ID="txtName" runat="server" Width="250px" Font-Size="11px" Height="20px"></asp:TextBox>
                                                        </td>--%>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td class="gridcellleft" style="vertical-align: top; text-align: left" id="TdFilter1">
                                                            <span id="spanal2">
                                                              <%-- <asp:TextBox ID="txtName" runat="server" Font-Size="12px" Width="285px"></asp:TextBox></span>--%>
                                                            <span id="span2">
                                                    <asp:TextBox ID="txtName" runat="server" Font-Size="12px" Width="285px" onkeyup="FunClientScrip(this,'CDSLHoldingClientsGroups',event)"></asp:TextBox></span>
                                                            <span id="span1" visible="false">
                                                                <asp:DropDownList ID="cmbsearch" runat="server" Width="70px" Font-Size="12px" Enabled="false">
                                                        <asp:ListItem>Clients</asp:ListItem>
                                                        <asp:ListItem>Branch</asp:ListItem>
                                                        <asp:ListItem>Group</asp:ListItem>
                                                        <asp:ListItem>User</asp:ListItem>
                                                                </asp:DropDownList></span> <a id="A3" href="javascript:void(0);" onclick="btnAddEmailtolist_click()">
                                                                    <span style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                                                        style="color: #009900; font-size: 8pt;"> </span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: left; vertical-align: top; height: 134px;">
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;&nbsp;<asp:ListBox ID="SelectionList" runat="server" Font-Size="12px" Height="90px"
                                                                            Width="290px"></asp:ListBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: left">
                                                                        <table cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <a id="AA2" href="javascript:void(0);" onclick="clientselection()"><span style="color: #000099;
                                                                                        text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <a id="AA1" href="javascript:void(0);" onclick="btnRemoveEmailFromlist_click()"><span
                                                                                        style="color: #cc3300; text-decoration: underline; font-size: 8pt;">Remove</span></a>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr style="display: none">
                                                        <td width="70px" style="text-align: left;">
                                                        </td>
                                                        <td style="height: 23px">
                                                            <asp:TextBox ID="txtName_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                                            <asp:TextBox ID="dtFrom_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                                            <asp:TextBox ID="dtTo_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <%--  <tr>
                                    <td style="text-align:left;">
                                        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" CssClass="btnUpdate" Text="Send" /></td>
                                    </tr>--%>
                                                </table>
                                            </td>
                                        </tr>
                                            <tr style="display: none">
                                <td style="height: 23px">
                                    <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                    <asp:Button ID="btnhide" runat="server" Text="Button" OnClick="btnhide_Click" />
                                </td>
                            </tr>
                                    </table></td></tr></table>
                     <%--   <asp:ScriptManager runat="server" ID="s1" AsyncPostBackTimeout="360000">
                        </asp:ScriptManager>--%>

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
                          //if (postBackElement.id == 'btnShow') 
                             $get('UpdateProgress1').style.display = 'block'; 
                       } 


                       function EndRequest(sender, args) 
                       { 

                          // if (postBackElement.id == 'btnShow') 
                              $get('UpdateProgress1').style.display = 'none'; 
                       } 
                        </script>

                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="u1">
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
                        <asp:UpdatePanel runat="server" ID="u1">
                            <ContentTemplate>
                                <asp:Label ID="Label5" runat="server" Text="Label"></asp:Label>
                                <table id="t11" runat="server">
                                  <%--  <tbody>--%>
                                        <tr>
                                            <td>
                                                <asp:LinkButton ID="ASPxFirst1" OnClick="btnFirst" runat="server" Font-Bold="True"
                                                    ForeColor="Blue" OnClientClick="javascript:selecttion();">First</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="ASPxPrevious1" OnClick="btnPrevious" runat="server" Font-Bold="True"
                                                    ForeColor="Blue" OnClientClick="javascript:selecttion();">Previous</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="ASPxNext1" OnClick="btnNext" runat="server" Font-Bold="True"
                                                    ForeColor="Blue" OnClientClick="javascript:selecttion();">Next</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="ASPxLast1" OnClick="btnLast" runat="server" Font-Bold="True"
                                                    ForeColor="Blue" OnClientClick="javascript:selecttion();">Last</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label11" runat="server" Font-Bold="True"></asp:Label>
                                            </td>
                                        </tr>
                                  <%--  </tbody>--%>
                                </table>
                                <table id="head" cellspacing="0" bordercolordark="blue" cellpadding="6" width="100%"
                                    bgcolor="#ffffff" border="1" runat="server">
                                  <%--  <tbody>--%>
                                        <tr>
                                            <td width="20%" bgcolor="white">
                                                BO ID:<asp:Label ID="lblBoID" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                            </td>
                                            <td align="left" width="30%" bgcolor="white">
                                                BO Name:<asp:Label ID="lblBoName" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                            </td>
                                            <td width="25%" bgcolor="white">
                                                Second Holder:<asp:Label ID="lblSecondHolder" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                            </td>
                                            <td width="25%" bgcolor="white">
                                                Third Holder:<asp:Label ID="lblThirdHolder" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                            </td>
                                        </tr>
                                  <%--  </tbody>--%>
                                </table>
                                <div>
                                   <%-- <table>--%>
                                        <tbody>
                                            <tr>
                                                <td id="ShowFilter">
                                                    <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">
                                                        Show Filter</span></a>
                                                </td>
                                                <td id="Td1">
                                                    <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">
                                                        All Records</span></a>
                                                </td>
                                            </tr>
                                      <%--  </tbody>--%>
                                    </table>
                                </div>
                                <div style="background-color: white" id="griddiv" runat="server">
                                    <dxe:ASPxGridView ID="gridHolding" runat="server" Width="100%" ClientInstanceName="grid"
                                        AutoGenerateColumns="False" KeyFieldName="CdslHolding_ISIN" OnCustomCallback="gridHolding_CustomCallback"
                                        OnCustomJSProperties="gridHolding_CustomJSProperties">
                                        <SettingsBehavior AllowFocusedRow="True"></SettingsBehavior>
                                        <Styles>
                                            <Header SortingImageSpacing="5px" ImageSpacing="5px">
                                            </Header>
                                        </Styles>
                                        <SettingsPager PageSize="20">
                                        </SettingsPager>
                                        <ClientSideEvents EndCallback="function(s, e) {
	  EndCall(s.cpEND);
}"></ClientSideEvents>
                                        <Columns>
                                            <dxe:GridViewDataTextColumn FieldName="CdslHolding_ISIN" Caption="ISIN No:" VisibleIndex="0">
                                                <Settings FilterMode="DisplayText" AutoFilterCondition="Contains"></Settings>
                                                <CellStyle CssClass="gridcellleft">
                                                </CellStyle>
                                                <FooterTemplate>
                                                    Total Holding Value
                                                </FooterTemplate>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="CDSLISIN_ShortName" Caption="Instrument Name (Short)"
                                                VisibleIndex="1">
                                                <Settings FilterMode="DisplayText" AutoFilterCondition="Contains"></Settings>
                                                <CellStyle CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="CdslHolding_SettlementID" Caption="SettlementID"
                                                VisibleIndex="2">
                                                <Settings FilterMode="DisplayText" AutoFilterCondition="Contains"></Settings>
                                                <CellStyle CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="CdslHolding_CurrentBalance" UnboundType="Integer"
                                                Caption="Current Balance" VisibleIndex="3">
                                                <Settings AllowAutoFilter="False"></Settings>
                                                <CellStyle HorizontalAlign="Right" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="CdslHolding_FreeBalance" UnboundType="Integer"
                                                Caption="Free" VisibleIndex="4">
                                                <Settings AllowAutoFilter="False"></Settings>
                                                <CellStyle HorizontalAlign="Right" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="CdslHolding_PledgeBalance" UnboundType="Integer"
                                                Caption="Pledged" VisibleIndex="5">
                                                <Settings AllowAutoFilter="False"></Settings>
                                                <CellStyle HorizontalAlign="Right" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="CdslHolding_EarmarkedBalance" UnboundType="Integer"
                                                Caption="Blocked[Earmark]" VisibleIndex="6">
                                                <Settings AllowAutoFilter="False"></Settings>
                                                <CellStyle HorizontalAlign="Right" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="CdslHolding_PendingRematBalance" UnboundType="Integer"
                                                Caption="Pending Remat" VisibleIndex="7">
                                                <Settings AllowAutoFilter="False"></Settings>
                                                <CellStyle HorizontalAlign="Right" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="CdslHolding_PendingDematBalance" UnboundType="Integer"
                                                Caption="Pending Demat" VisibleIndex="8">
                                                <Settings AllowAutoFilter="False"></Settings>
                                                <CellStyle HorizontalAlign="Right" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="Rate" Caption="Rate" VisibleIndex="9">
                                                <Settings AllowAutoFilter="False"></Settings>
                                                <CellStyle HorizontalAlign="Right" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="ISINVAlue" Caption="Value" VisibleIndex="10">
                                                <Settings AllowAutoFilter="False"></Settings>
                                                <CellStyle HorizontalAlign="Right" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                        </Columns>
                                        <Settings ShowTitlePanel="True" ShowStatusBar="Visible" ShowFooter="True"></Settings>
                                        <StylesEditors>
                                            <ProgressBar Height="25px">
                                            </ProgressBar>
                                        </StylesEditors>
                                        <TotalSummary>
                                            <dxe:ASPxSummaryItem FieldName="ISINVAlue" ShowInColumn="Value" ShowInGroupFooterColumn="Value"
                                                SummaryType="Sum" Tag="Total Holding Value" DisplayFormat="#,##,###.00" />
                                        </TotalSummary>
                                    </dxe:ASPxGridView>
                                </div>
                                <div>
                                    <table id="t1" runat="server">
                                     <%--   <tbody>--%>
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="ASPxFirst" OnClick="btnFirst" runat="server" Font-Bold="True"
                                                        ForeColor="Blue" OnClientClick="javascript:selecttion();">First</asp:LinkButton></td>
                                                <td>
                                                    <asp:LinkButton ID="ASPxPrevious" OnClick="btnPrevious" runat="server" Font-Bold="True"
                                                        ForeColor="Blue" OnClientClick="javascript:selecttion();">Previous</asp:LinkButton></td>
                                                <td>
                                                    <asp:LinkButton ID="ASPxNext" OnClick="btnNext" runat="server" Font-Bold="True" ForeColor="Blue"
                                                        OnClientClick="javascript:selecttion();">Next</asp:LinkButton></td>
                                                <td>
                                                    <asp:LinkButton ID="ASPxLast" OnClick="btnLast" runat="server" Font-Bold="True" ForeColor="Blue"
                                                        OnClientClick="javascript:selecttion();">Last</asp:LinkButton></td>
                                                <td>
                                                    <asp:Label ID="Label1" runat="server" Font-Bold="True"></asp:Label>
                                                </td>
                                            </tr>
                                      <%--  </tbody>--%>
                                    </table>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click"></asp:AsyncPostBackTrigger>
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>            
            <input id="hidClients" type="hidden"  runat="server"  />
            <input id="hidBranch" type="hidden"  runat="server"  />
            <input id="hidGroup" type="hidden"  runat="server"  />
            
        </div>
</asp:Content>

