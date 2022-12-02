<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_finalsettlementreport" Codebehind="finalsettlementreport.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />

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
	  .grid_scroll
    {
        overflow-y: no;  
        overflow-x: scroll; 
        width:90%;
        scrollbar-base-color: #C0C0C0;
    
    }
	</style>
 <script language="javascript" type="text/javascript">
  groupvalue="";
  
    function Page_Load()///Call Into Page Load
            {
                 Hide('td_dtfrom');
                 Hide('td_dtto');
                 Hide('showFilter');
                 Show('td_btnshow');
                 Hide('td_btnprint');
                 Hide('tr_filter');
                 Hide('cmbgroup');
                 Hide('cmbclient');
                 document.getElementById('hiddencount').value=0;
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
          if(groupvalue=="")
          {
               cmbVal=document.getElementById('cmbsearchOption').value;
               cmbVal=cmbVal+'~'+document.getElementById('ddlgrouptype').value;
          }
          else
          {
            if(document.getElementById('cmbsearchOption').value=="Clients")
            {
                if(document.getElementById('ddlGroup').value=="0")//////////////Group By  selected are branch
                {
                    if(document.getElementById('rdbranchAll').checked==true)
                       {
                           cmbVal=document.getElementById('cmbsearchOption').value+'Branch';
                           cmbVal=cmbVal+'~'+'ALL'+'~'+document.getElementById('ddlgrouptype').value;
                       }
                   else
                       {
                           cmbVal=document.getElementById('cmbsearchOption').value+'Branch';
                           cmbVal=cmbVal+'~'+'Selected'+'~'+groupvalue;
                       }
                }
               else //////////////Group By selected are Group
                {
                   if(document.getElementById('rdddlgrouptypeAll').checked==true)
                       {
                           cmbVal=document.getElementById('cmbsearchOption').value+'Group';
                           cmbVal=cmbVal+'~'+'ALL'+'~'+document.getElementById('ddlgrouptype').value;
                       }
                   else
                       {
                           cmbVal=document.getElementById('cmbsearchOption').value+'Group';
                           cmbVal=cmbVal+'~'+'Selected'+'~'+groupvalue;
                       }
               }
            }
            else
            {
                cmbVal=document.getElementById('cmbsearchOption').value;
                cmbVal=cmbVal+'~'+document.getElementById('ddlgrouptype').value;
            }
          }
         
          ajax_showOptions(objID,'ShowClientFORMarginStocks',objEvent,cmbVal);
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
                     Show('td_ChkDISTRIBUTION');
                 }
             else
                 {
                     Show('td_btnshow');
                     Hide('td_btnprint');
                     Hide('td_ChkDISTRIBUTION');
                 }
                 height();
                 selecttion();
        } 
        
   function fnunderlying(obj)
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
             height();
   }
   
  function fnExpiry(obj)
   {
            if(obj=="a")
                Hide('showFilter');
             else
             {
                  var cmb=document.getElementById('cmbsearchOption');
                  cmb.value='Expiry';
                  Show('showFilter');
             }
             selecttion();
             height();
   
   }
  function NORECORD(obj)
  {
      Hide('tr_filter');
      Hide('displayAll');
      Show('tab2');
      Hide('showFilter');
      if(obj=='1')
          alert('No Record Found!!');
      else if (obj=='2')
          alert('Please Select Type!!');
           document.getElementById('hiddencount').value=0;
      height();
  }
  function Display()
  {
      Show('tr_filter');
      Show('displayAll');
      Hide('tab2');
      Hide('showFilter');
      document.getElementById('hiddencount').value=0;
      selecttion();
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
     function fnexpirtycallajax(objID,objListFun,objEvent)
     {
         var date;
         if(document.getElementById('ddldate').value=='0')
         {
            date=new Date(dtfor.GetDate());
            date=parseInt(date.getMonth()+1)+'-'+date.getDate()+'-'+ date.getFullYear();
         }
         else
         {
            date=new Date(dtfrom.GetDate());
            date=parseInt(date.getMonth()+1)+'-'+date.getDate()+'-'+ date.getFullYear();
         }
         ajax_showOptions(objID,'Searchproductandeffectuntil',objEvent,'Expiry'+'~'+date);
     }
    
     function ajaxcall(objID,objListFun,objEvent)
     {
        
        if(document.getElementById('cmbsearchOption').value=="Expiry")
        {
        
            fnexpirtycallajax(objID,objListFun,objEvent);
        }
        else if(document.getElementById('cmbsearchOption').value=="Product")
        {
        
            ajax_showOptions(objID,'Searchproductandeffectuntil',objEvent,'Product');
        }
        else
        {
            FunClientScrip(objID,objListFun,objEvent);
        }
     }
    
   FieldName='lstSlection';
 </script>	
  <script type="text/ecmascript">   
       function ReceiveServerData(rValue)
        {
               
                var j=rValue.split('~');
                var btn = document.getElementById('btnhide');

                if(j[0]=='Group')
                {
                   groupvalue=j[1];
                    btn.click();
                }
                if(j[0]=='Branch')
                {
                   groupvalue=j[1];
                   btn.click();
                }
            if(j[0]=='Clients')
            {
                document.getElementById('HiddenField_Client').value = j[1];
            }
            else if(j[0]=='Expiry')
            {
                document.getElementById('HiddenField_Expiry').value = j[1];
            }
            else if(j[0]=='Product')
            {
                document.getElementById('HiddenField_Product').value = j[1];
            }
            else if(j[0]=='Branch')
            {
                document.getElementById('HiddenField_Branch').value = j[1];
            }
            else if(j[0]=='Group')
            {
                document.getElementById('HiddenField_Group').value = j[1];
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
                    <strong><span id="SpanHeader" style="color: #000099">Final Settlement Report</span></strong></td>
                <td class="EHEADER" width="15%" id="tr_filter" style="height: 22px">
                    <a href="javascript:void(0);" onclick="Filter();"><span style="color: Blue; text-decoration: underline;
                        font-size: 8pt; font-weight: bold">Filter</span></a>
                  <asp:DropDownList ID="cmbExport" runat="server" AutoPostBack="True" Font-Size="12px" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged">
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
                <td class="gridcellleft">
                    <asp:DropDownList ID="ddldate" runat="server" Width="80px" Font-Size="12px" onchange="fn_ddldateChange(this.value)">
                        <asp:ListItem Value="0">For a Date</asp:ListItem>
                        <asp:ListItem Value="1">For a Period</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td id="td_dtfor" class="gridcellleft">
                    <dxe:ASPxDateEdit ID="dtfor" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                        Font-Size="12px" Width="108px" ClientInstanceName="dtfor">
                        <dropdownbutton text="For">
                                        </dropdownbutton>
                    </dxe:ASPxDateEdit>
                </td>
                <td id="td_dtfrom"  class="gridcellleft">
                    <dxe:ASPxDateEdit ID="dtfrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                        Font-Size="12px" Width="108px" ClientInstanceName="dtfrom">
                        <dropdownbutton text="From">
                                        </dropdownbutton>
                    </dxe:ASPxDateEdit>
                </td>
                <td id="td_dtto"  class="gridcellleft">
                    <dxe:ASPxDateEdit ID="dtto" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                        Font-Size="12px" Width="108px" ClientInstanceName="dtto">
                        <dropdownbutton text="To">
                                        </dropdownbutton>
                    </dxe:ASPxDateEdit>
                </td>
                <td colspan="3"></td>
            </tr>
            <tr>
            
                            <td colspan="5" class="gridcellleft">
                             <table border="10" cellpadding="1" cellspacing="1">
                             <tr>
                            <td class="gridcellleft" BGCOLOR="#B7CEEC">
                                Type :</td>
                            <td colspan="3">
                <asp:CheckBoxList ID="chktradetype" runat="server" RepeatDirection="Horizontal" Width="200px">
                                    <asp:ListItem Value="Exc" Selected="True">Exercise</asp:ListItem>
                                    <asp:ListItem Value="Asn" Selected="True">Assignment</asp:ListItem>
                                    <asp:ListItem Value="Exp" Selected="True">Expiry</asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft" BGCOLOR="#B7CEEC">
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
                            <td class="gridcellleft" BGCOLOR="#B7CEEC">
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
                        </table></td></tr>                        <tr>
                            <td colspan="5" class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" style="width:100px;" BGCOLOR="#B7CEEC">
                                           Asset:</td>
                                        <td>
                                            <asp:RadioButton ID="rdbunderlyingall" runat="server" Checked="True" GroupName="d"
                                                onclick="fnunderlying('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbunderlyingselected" runat="server" GroupName="d" onclick="fnunderlying('b')" />Selected
                                            Asset
                                        </td>
                                        <td>
                                           
                                            </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" style="width: 100px;" bgcolor="#B7CEEC">
                                            Expiry :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbExpiryAll" runat="server" Checked="True" GroupName="e" onclick="fnExpiry('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbExpirySelected" runat="server" GroupName="e" onclick="fnExpiry('b')" />Selected
                                            Expiry
                                        </td>
                                        <td>
                                          
                                            </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                           
                        
                        
                        <tr>
                            <td colspan="5">
                                <table>
                                    <tr>
                                        <td id="Td3" class="gridcellleft">
                                            <asp:RadioButton ID="rbScreen" runat="server" GroupName="f" Checked="True" onclick="RBShowHide(this.value)" />Screen
                                        </td>
                                        <td id="Td4" class="gridcellleft">
                                            <asp:RadioButton ID="rbPrint" runat="server" GroupName="f" onclick="RBShowHide(this.value)" />Print
                                        </td>
                                        <td style="display: none;" id="td_ChkDISTRIBUTION">
                                            <table>
                                                <tr>
                                                    <td id="tr_printlogo">
                                                        <asp:CheckBox ID="CHKLOGOPRINT" runat="server" Checked="true" />
                                                        Do Not Print Logo</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="ChkDISTRIBUTION" runat="server" />
                                                        Distribution Copy</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 22px" id="td_btnshow" class="gridcellleft">
                                <asp:Button ID="btnshow" runat="server" CssClass="btnUpdate" Height="20px" Text="Show"
                                    Width="101px" OnClick="btnshow_Click" />
                            </td>
                            <td style="height: 22px" id="td_btnprint">
                                <asp:Button ID="btnprint" runat="server" CssClass="btnUpdate" Height="20px" Text="Print"
                                    OnClientClick="selecttion()" Width="101px" />
                            </td>
                           
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
                                            <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="ajaxcall(this,'chkfn',event)"></asp:TextBox>
                                            <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                Enabled="false">
                                                <asp:ListItem>Clients</asp:ListItem>
                                                <asp:ListItem>Branch</asp:ListItem>
                                                <asp:ListItem>Group</asp:ListItem>
                                                <asp:ListItem>Expiry</asp:ListItem>
                                                <asp:ListItem>Product</asp:ListItem>
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
        <table>
            <tr>
                <td style="display: none;">
                    <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                    <asp:Button ID="btnhide" runat="server" Text="Button" OnClick="btnhide_Click" />
                    <asp:HiddenField ID="HiddenField_Client" runat="server" />
                    <asp:HiddenField ID="HiddenField_Expiry" runat="server" />
                    <asp:HiddenField ID="HiddenField_Product" runat="server" />
                    <asp:HiddenField ID="HiddenField_Group" runat="server" />
                    <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                    <asp:HiddenField ID="hiddencount" runat="server" />
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
                        
                        <tr id="tr_DIVdisplayPERIOD" style="display: none;"> 
                            <td>
                                <div id="DIVdisplayPERIOD" runat="server">
                                </div>
                            </td>
                             <td style="height: 44px">
                                                    <asp:DropDownList ID="cmbgroup" runat="server" Font-Size="12px" Width="300px" AutoPostBack="True"
                                                        OnSelectedIndexChanged="cmbgroup_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                <asp:DropDownList ID="cmbclient" runat="server" Font-Size="12px" Width="300px" AutoPostBack="True">
                                </asp:DropDownList><asp:HiddenField ID="HiddenField1" runat="server" />
                            </td>
                        </tr>
                      
                       
                        <tr>
                            <td>
                                <div id="display" runat="server" >
                                </div>
                            </td>
                        </tr>
                       
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnshow" EventName="Click"></asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>