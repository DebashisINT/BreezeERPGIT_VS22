<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.MarginReportNSEFO" Codebehind="MarginReportNSEFO.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script language="javascript" type="text/javascript">
  groupvalue="";
  
    function Page_Load()///Call Into Page Load
            {
            
                 Hide('td_dtfrom');
                 Hide('td_dtto');
                 Hide('showFilter');
                 Hide('tr_filter');
                 document.getElementById('hiddencount').value=0;
                 height();
                 Hide('trType');
              
            }
            
 function DisableC(obj)
 {
 var hideobj=obj;
 if(hideobj=='N')
 {
 document.getElementById('tr_group').style.display="inline";
 
 }
 else if(hideobj=='N')
 {
  document.getElementById('tr_group').style.display="none";
 }
 
 }           
  function Filter()
      {
          Hide('tr_filter');
          Hide('displayAll');
          Hide('trType');
          Show('tab2');
          Hide('showFilter');
          selecttion();
          var obj=document.getElementById("ddldate").value;
          fn_ddldateChange(obj)
          height();

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

    function height()
        {
            if(document.body.scrollHeight>=1000)
            {
                window.frameElement.height = document.body.scrollHeight;
            }
            else
            {
                window.frameElement.height = '1500px';
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
  function fnrpttype(obj)
   {
      
      if(obj=='1')
      {
       Show('trType');
       }
       else
       {
       Hide('trType');
       }
       selecttion();
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
       height();
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
             height();
   }
   
       
 function NORECORD(obj)
  {
      Hide('displayAll');
      Hide('tr_filter');
      Show('tab2');
      Hide('showFilter');
      if(obj=='1')
          alert('No Record Found!!');
      else if (obj=='2')
          alert('Please Select Type!!');
           document.getElementById('hiddencount').value=0;
              var obj=document.getElementById("ddldate").value;
      fn_ddldateChange(obj)
   if(document.getElementById('ddlrpttype').value=='0')
        {
            document.getElementById('trType').style.display='none';
        } 
        if(document.getElementById('ddlrpttype').value=='1')
        {
            document.getElementById('trType').style.display='inline';
        } 
      height();
      
  }
  function Display()
  {
     
      Show('displayAll');
      Hide('tab2');
      Hide('showFilter');
      Hide('trType');
      document.getElementById('hiddencount').value=0;
   
//       if(document.getElementById('ddlrpttype').value=='0')
//        {
//            document.getElementById('tr_prvnxt').style.display='inline';
//        }
//       if(document.getElementById('ddlrpttype').value=='1')
//        {
//            document.getElementById('tr_prvnxt').style.display='none';
//        } 
      Show('tr_filter');
      selecttion();
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
    
     function ajaxcall(objID,objListFun,objEvent)
     {
        
            FunClientScrip(objID,objListFun,objEvent);
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
                    <strong><span id="SpanHeader" style="color: #000099">Margin Report For NSE FO</span></strong></td>
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
                   
            <tr>
                         <td colspan="5" class="gridcellleft">
                             <table class="tableBorderClass"cellpadding="1" cellspacing="1">
                        <tr valign="top">
                            <td BGCOLOR="#B7CEEC">
                                <asp:DropDownList ID="ddldate" runat="server" Width="100px" Font-Size="12px" onchange="fn_ddldateChange(this.value)">
                                    <asp:ListItem Value="0">For a Date</asp:ListItem>
                                    <asp:ListItem Value="1">Date Range</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td id="td_dtfor">
                                <dxe:ASPxDateEdit ID="dtfor" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Font-Size="12px" Width="108px" ClientInstanceName="dtfor">
                                    <DropDownButton Text="For">
                                    </DropDownButton>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td id="td_dtfrom">
                                <dxe:ASPxDateEdit ID="dtfrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Font-Size="12px" Width="108px" ClientInstanceName="dtfrom">
                                    <DropDownButton Text="From">
                                    </DropDownButton>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td id="td_dtto">
                                <dxe:ASPxDateEdit ID="dtto" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Font-Size="12px" Width="108px" ClientInstanceName="dtto">
                                    <DropDownButton Text="To">
                                    </DropDownButton>
                                </dxe:ASPxDateEdit>
                            </td>
                       
                        </tr>
                        <tr>
                           <td BGCOLOR="#B7CEEC">
                               <strong>Group By : </strong>
                           </td>
                            <td>
                                <asp:DropDownList ID="ddlGroup" runat="server" Width="80px" Font-Size="12px" onchange="fnddlGroup(this.value)">
                                    <asp:ListItem Value="0">Branch</asp:ListItem>
                                    <asp:ListItem Value="1">Group</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td colspan="2" id="td_branch">
                                <table>
                                    <tr>
                                        <td style="width: 79px">
                                            <asp:RadioButton ID="rdbranchAll" runat="server" Checked="True" GroupName="a" onclick="Branch('a')" />
                                            All
                                        </td>
                                        <td style="width: 136px">
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
                            <td BGCOLOR="#B7CEEC">
                                <strong>Clients :</strong></td>
                            <td>
                                <asp:RadioButton ID="rdbClientALL" runat="server" Checked="True" GroupName="c" onclick="Clients('a')" />
                                All Client
                            </td>
                         
                            <td>
                                <asp:RadioButton ID="rdbClientSelected" runat="server" GroupName="c" onclick="Clients('b')" />
                                Selected Client
                            </td>
                        </tr>
                        <tr>
                        <td BGCOLOR="#B7CEEC">
                            <strong>Report View :</strong></td>
                            <td>   <asp:DropDownList ID="ddlrpttype" runat="server" Width="105px" onchange="fnrpttype(this.value)">
                                    <asp:ListItem Value="0">Details</asp:ListItem>
                                    <asp:ListItem Value="1">Summary</asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>
                          <tr  id="trType">
                        <td BGCOLOR="#B7CEEC">
                            <strong>Calculation Type :</strong></td>
                            <td>   <asp:DropDownList ID="ddlCalType" runat="server" Width="105px">
                                    <asp:ListItem Value="0">Exchange</asp:ListItem>
                                    <asp:ListItem Value="1">Calculated</asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>
                        </table></td></tr>                      
                        <tr>
                             <td style="height: 22px" id="td_btnprint" align="center">
                                <asp:Button ID="btnGenerate" runat="server" CssClass="btnUpdate" Height="20px" Text="Generate"
                                      Width="101px" OnClick="btnGenerate_Click" />
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
                                                    <td style="height: 14px">
                                                        <a id="A2" href="javascript:void(0);" onclick="clientselectionfinal()"><span style="color: #000099;
                                                            text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
                                                    </td>
                                                    <td style="height: 14px">
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
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        </asp:UpdatePanel>
       <div id="displayAll" style="display: none;">
       <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <table border="1" width="100%">
                    <tr style="display: none">
                        <td style="height: 62px">
                            <asp:DropDownList ID="cmbclient" runat="server" AutoPostBack="True" Font-Size="12px"
                                Width="300px">
                            </asp:DropDownList><asp:HiddenField ID="hiddencount1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                    <td style="font-weight: bold" align="center">
                        Margin Report For :
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="True"></asp:Label></td>
                    </tr>
                    <tr id="tr_DIVdisplayPERIOD">
                        <td>
                            <div id="DIVdisplayPERIOD" runat="server">
                            </div>
                        </td>
                    </tr>
                    <tr id="tr_group">
                        <td style="height: 117px">
                            <asp:UpdatePanel ID="updatepanel_trprevnext" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table runat="server">
                                        <tr valign="top">
                                            <td style="height: 44px">
                                                <asp:LinkButton ID="lnkPrev" runat="server" CommandName="Prev" OnClientClick="javascript:selecttion();"
                                                    OnCommand="NavigationLinkC_Click" Text="[Prev]"> </asp:LinkButton>
                                            </td>
                                            <td style="height: 44px">
                                                <asp:DropDownList ID="cmbgroup" runat="server" AutoPostBack="True" Font-Size="12px"
                                                    OnSelectedIndexChanged="cmbgroup_SelectedIndexChanged" Width="300px">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="height: 44px">
                                                <asp:LinkButton ID="lnkNext" runat="server" CommandName="Next" OnClientClick="javascript:selecttion();"
                                                 OnCommand="NavigationLinkC_Click" Text="[Next]"> </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
           
                    <tr>
                        <td>
                            <div id="display" runat="server">
                            </div>
                        </td>
                    </tr>
                    <asp:HiddenField ID="TotalGrp" runat="server" />
                </table>
            </ContentTemplate>
         
        </asp:UpdatePanel>
     </div>
   </asp:Content>
