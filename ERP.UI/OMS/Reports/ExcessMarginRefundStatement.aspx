<%@ Page Language="C#" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_ExcessMarginRefundStatement" MasterPageFile="~/OMS/MasterPage/ERP.Master" Codebehind="ExcessMarginRefundStatement.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
 <script language="javascript" type="text/javascript">
 
    function Page_Load()///Call Into Page Load
            {
                 Hide('showFilter');
                 Show('td_btnshow');
                 Hide('td_btnprint');
                 Hide('tr_filter');
                 txtmarkupappmrgn.SetValue('175.00');
                 txtmaintaincashcomp.SetValue('50.00');
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
        
     function fn_txtmarkupappmrgn(obj)
     {
       if(obj>9999.99)
       {
            txtmarkupappmrgn.SetValue('100.00');
       }
       selecttion();
     }
     function fn_txtmaintaincashcomp(obj)
     {
       if(obj>100.00)
       {
            txtmaintaincashcomp.SetValue('100.00');
       }
       selecttion();
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
               else //////////////Group By selected are Group
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
        
  
 
  function NORECORD(obj)
  {
      Hide('tr_filter');
      Hide('displayAll');
      Hide('showFilter');
      Show('tab2');
      if(obj=='1')
          alert('No Record Found!!');
      document.getElementById('hiddencount').value=0;
      height();
  }
  function Display()
  {
  
      Show('tr_filter');
      Show('displayAll');
      Hide('showFilter');
      Hide('tab2');
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
     
     function FnSegment(Obj)
      {
        if(Obj=="b")
        {
            Show('Td_SpecificSegment');
        }
        else
        {
            Hide('Td_SpecificSegment');
        }
      } 
   FieldName='lstSlection';
 </script>	
  <script type="text/ecmascript">   
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
            }  
            if(j[0]=='Clients')
            {
                document.getElementById('HiddenField_Client').value = j[1];
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
                    <strong><span id="SpanHeader" style="color: #000099">Excess Margin Refund Statement</span></strong></td>
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
                               <table border="10" cellpadding="0" cellspacing="0">
                                   <tr valign="top">
                                       <td class="gridcellleft" bgcolor="#B7CEEC">
                                           Settlement Date :
                                       </td>
                                       <td id="td_dtto" class="gridcellleft" colspan="4">
                                           <dxe:ASPxDateEdit ID="dtSettlementDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                               Font-Size="12px" Width="108px" ClientInstanceName="cdtSettlementDate" EditFormatString="dd-MM-yyyy">
                                           </dxe:ASPxDateEdit>
                                       </td>
                                   </tr>
                                   <tr valign="top">
                                       <td colspan="4" bgcolor="#B7CEEC">
                                           <table>
                                               <tr>
                                                   <td>
                                                       Applicable Margin is Average Daily Margin For The last
                                                   </td>
                                                   <td>
                                                       <dxe:ASPxTextBox ID="txtWorkingdays" runat="server" HorizontalAlign="Right" Width="50px">
                                                           <masksettings mask="&lt;1..99g&gt;" includeliterals="DecimalSymbol" />
                                                           <validationsettings errordisplaymode="None">
                                                    </validationsettings>
                                                       </dxe:ASPxTextBox></td>
                                                   <td>
                                                       Working Days With a MarkUp Of</td>
                                                   <td>
                                                       <dxe:ASPxTextBox ID="txtmarkupappmrgn" runat="server" HorizontalAlign="Right" Width="100px">
                                                           <masksettings mask="&lt;0..9999g&gt;.&lt;00..99&gt;%" includeliterals="DecimalSymbol" />
                                                           <validationsettings errordisplaymode="None">
                                               </validationsettings>
                                                           <clientsideevents lostfocus="function(s, e) {
	                                        fn_txtmarkupappmrgn(s.GetValue())
                                        }" />
                                                       </dxe:ASPxTextBox>
                                                   </td>
                                               </tr>
                                           </table>
                                       </td>
                                   </tr>
                                    <tr valign="top">
                                       <td class="gridcellleft" bgcolor="#B7CEEC">
                                          Maintain Cash Component at:
                                       </td>
                                       <td  class="gridcellleft" colspan="4">
                                           <dxe:ASPxTextBox ID="txtmaintaincashcomp" runat="server" HorizontalAlign="Right"
                                               Width="100px">
                                               <MaskSettings Mask="&lt;0..100g&gt;.&lt;00..99&gt;%" IncludeLiterals="DecimalSymbol" />
                                               <ValidationSettings ErrorDisplayMode="None">
                                               </ValidationSettings>
                                               <ClientSideEvents LostFocus="function(s, e) {
	                                        fn_txtmaintaincashcomp(s.GetValue())
                                        }" />
                                           </dxe:ASPxTextBox>
                                       </td>
                                   </tr>
                                     <tr>
                                            <td class="gridcellleft" BGCOLOR="#B7CEEC" colspan="5" >
                                                <asp:CheckBox ID="chkpartialrelease" runat="server"/>
                                                    No Partial Release of Stocks
                                            </td>
                                        </tr>
                                   <tr id="tr_grp">
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
                                                       <asp:RadioButton ID="rdbranchAll" runat="server" Checked="True" GroupName="b" onclick="Branch('a')" />
                                                       All
                                                   </td>
                                                   <td>
                                                       <asp:RadioButton ID="rdbranchSelected" runat="server" GroupName="b" onclick="Branch('b')" />Selected
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
                                                       <asp:RadioButton ID="rdddlgrouptypeAll" runat="server" Checked="True" GroupName="c"
                                                           onclick="Group('a')" />
                                                       All
                                                       <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="c" onclick="Group('b')" />Selected
                                                   </td>
                                               </tr>
                                           </table>
                                       </td>
                                   </tr>
                                   <tr id="tr_client">
                                       <td class="gridcellleft" bgcolor="#B7CEEC">
                                           Clients :</td>
                                       <td>
                                           <asp:RadioButton ID="rdbClientALL" runat="server" Checked="True" GroupName="d" onclick="Clients('a')" />
                                           All Client
                                       </td>
                                       <td>
                                           <asp:RadioButton ID="radPOAClient" runat="server" GroupName="d" onclick="Clients('a')" />POA
                                           Client
                                       </td>
                                       <td>
                                           <asp:RadioButton ID="rdbClientSelected" runat="server" GroupName="d" onclick="Clients('b')" />
                                           Selected Client
                                       </td>
                                   </tr>
                                     <tr>
                                        <td colspan="5">
                                            <table>
                                                <tr>
                                                    <td class="gridcellleft" bgcolor="#B7CEEC">
                                                        Segment :</td>
                                                    <td>
                                                        <asp:RadioButton ID="rdSegmentALLCM" runat="server" Checked="True" GroupName="dd" onclick="FnSegment('a')" />
                                                        All CM
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdSegmentALLCMFO" runat="server" GroupName="dd" onclick="FnSegment('a')" />
                                                       All CM & FO
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdSpecificSegment" runat="server" GroupName="dd" onclick="FnSegment('b')" />
                                                        Specific
                                                    </td>
                                                    <td id="Td_SpecificSegment" style="display:none;">
                                                        <asp:TextBox ID="txtSegment" runat="server" Width="100px" Font-Size="12px" onkeyup="ajax_showOptions(this,'ShowClientFORMarginStocks',event,'Segment')"></asp:TextBox></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                    <td class="gridcellleft" BGCOLOR="#B7CEEC">Stock Release Order</td>
                                            <td colspan="4" >
                                                 <asp:DropDownList ID="ddlStock" runat="server" Width="200px" Font-Size="12px">
                                               <asp:ListItem Value="0">Margin then Holdback [Default]</asp:ListItem>
                                               <asp:ListItem Value="1">Hlodback then Margin</asp:ListItem>
                                           </asp:DropDownList>
                                            </td>
                                        </tr>
                               </table>
                           </td>
                       </tr>
                      
                      
                       <tr>
                           <td colspan="5" >
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
                                                       <asp:CheckBox ID="ChkDISTRIBUTION" runat="server" onclick="fn_charge(this)" />
                                                       Distribution Copy</td>
                                               </tr>
                                           </table>
                                       </td>
                                   </tr>
                               </table>
                           </td>
                       </tr>
                       <tr>
                           <td colspan="5">
                               <table>
                                   <tr>
                                       <td id="td_btnshow" class="gridcellleft">
                                           <asp:Button ID="btnshow" runat="server" CssClass="btnUpdate" Height="20px" Text="Show"
                                               Width="101px" OnClick="btnshow_Click" />
                                       </td>
                                       <td id="td_btnprint">
                                           <asp:Button ID="btnprint" runat="server" CssClass="btnUpdate" Height="20px" Text="Print"
                                               OnClientClick="selecttion()" Width="101px" OnClick="btnprint_Click" />
                                       </td>
                                   </tr>
                               </table>
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
                                            <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="FunClientScrip(this,'chkfn',event)"></asp:TextBox>
                                            <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                Enabled="false">
                                                <asp:ListItem>Clients</asp:ListItem>
                                                <asp:ListItem>Branch</asp:ListItem>
                                                <asp:ListItem>Group</asp:ListItem>
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
                    <asp:Button ID="btnhide" runat="server" Text="btnhide" OnClick="btnhide_Click" />
                    <asp:HiddenField ID="HiddenField_Group" runat="server" />
                    <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                    <asp:HiddenField ID="HiddenField_Client" runat="server" />
                    <asp:HiddenField ID="HiddenField_Product" runat="server" />
                    <asp:TextBox ID="txtSegment_hidden" runat="server" Width="5px"></asp:TextBox>
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
                                                        <font size='1' face='Tahoma'><strong align='center'>Please Wait..</strong></font></td>
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
                    
   <div id="displayAll" style="display: none;">
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                   <table width="100%" border="1">
                        <tr style="display: none;">
                            <td>
                                <asp:DropDownList ID="cmbclient" runat="server" Font-Size="12px" Width="300px" AutoPostBack="True">
                                </asp:DropDownList><asp:HiddenField ID="hiddencount" runat="server" />
                            </td>
                        </tr>
                        <tr id="tr_DIVdisplayPERIOD">
                            <td>
                                <div id="DIVdisplayPERIOD" runat="server">
                                </div>
                            </td>
                        </tr>
                        <tr id="tr_group">
                            <td>
                                <asp:UpdatePanel ID="updatepanel_trprevnext" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" runat="server">
                                           <tr valign="top">
                                                <td style="height: 44px">
                                                    <asp:LinkButton ID="lnkPrev" runat="server" CommandName="Prev" Text="[Prev]"
                                                        OnCommand="NavigationLinkC_Click" OnClientClick="javascript:selecttion();"> </asp:LinkButton>
                                                </td>
                                                <td style="height: 44px">
                                                    <asp:DropDownList ID="cmbgroup" runat="server" Font-Size="12px" Width="300px"
                                                        AutoPostBack="True" OnSelectedIndexChanged="cmbgroup_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="height: 44px">
                                                    <asp:LinkButton ID="lnkNext" runat="server" CommandName="Next" Text="[Next]"
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
                                <div id="display" runat="server" >
                                </div>
                            </td>
                        </tr>
                        <asp:HiddenField ID="TotalGrp" runat="server" />
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnshow" EventName="Click"></asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
  </asp:Content>
