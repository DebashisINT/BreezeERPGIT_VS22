<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_DEMATCHARGESSTATEMENT" Codebehind="DEMATCHARGESSTATEMENT.ASPX.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
        groupvalue="";
       function Page_Load()///Call Into Page Load
            {   
                Hide('showFilter');
                Hide('filter');
                Hide('DATADISPLAY');
                FnddlGeneration('SelectType');
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
            window.frameElement.width = document.body.scrollWidth;
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
              
                document.getElementById('showFilter').style.display='none';
                document.getElementById('btn_show').disabled=false;
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
            
 function Hide(obj)
            {
             document.getElementById(obj).style.display='none';
            }
  function Show(obj)
            {
             document.getElementById(obj).style.display='inline';
            }
    function selecttion()
        {
            var combo=document.getElementById('ddlExport');
            combo.value='Ex';
        } 
  
      function Clients(obj)
        {
             if(obj=="a")
             {
                Hide('showFilter');
             }
             else
             {
                  var cmb=document.getElementById('cmbsearchOption');
                  cmb.value='Clients';
                  Show('showFilter');
             }
             selecttion();
        }
      function Branch(obj)
      {
            if(obj=="a")
            {
                Hide('showFilter');
            }
             else
             {
                  var cmb=document.getElementById('cmbsearchOption');
                  cmb.value='Branch';
                  Show('showFilter');
             }
           selecttion();   
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
   }
     function Group(obj)
      {
            if(obj=="a")
            {
                Hide('showFilter');
            }
             else
             {
                  var cmb=document.getElementById('cmbsearchOption');
                  cmb.value='Group';
                  Show('showFilter');
             }
           selecttion();
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
   }
   function ReceiveServerData(rValue)
        {
                var j=rValue.split('~');
               
                if(j[0]=='Group')
                {
                   groupvalue=j[1];
                }
                if(j[0]=='Branch')
                {
                   groupvalue=j[1];
                }
                var btn = document.getElementById('btnhide');
                btn.click();
      
        } 
//        function ddlType1(obj)
//        {
//        if(obj=='1')
//        {
//        
//        FnddlGeneration('screen')
//        }
//        
//        }
        function FnddlGeneration(obj)
    {
   
    if(obj=='SelectType')
    {
    Hide('td_screen');
    Hide('td_email')
    }
    if(obj=='screen')
    {
    Show('td_screen');
    Hide('td_email')
    }
    if(obj=='email')
    {
    Hide('td_screen');
    Show('td_email')
    }
    
    
    
    }
    function Display(obj)
    {
     document.getElementById('spanshow2').innerText=obj;
     Show('filter');
     Hide('TrAll');
     Hide('TrBtn');
     Show('DATADISPLAY');
     height();
    }
   function Filterdispaly()
   {
     Hide('filter');
     Show('TrAll');
     Show('TrBtn');
     Hide('DATADISPLAY');
     height();
   }
  function NORECORD()
  {
     Hide('filter');
     Show('TrAll');
     Show('TrBtn');
     Hide('DATADISPLAY');
     alert('No Record Found !!');
     height();
  }
            
	       FieldName='lstSlection';

    
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
                    <td class="EHEADER" style="text-align: center;">
                        <strong><span style="color: #000099">Demat Charges Statement</span></strong>
                    </td>
                    <td class="EHEADER" width="15%" id="filter">
                        <a href="javascript:void(0);" onclick="Filterdispaly();"><span style="color: Blue;
                            text-decoration: underline; font-size: 8pt; font-weight: bold">Filter</span></a>
                        &nbsp; &nbsp;||&nbsp;
                        <asp:DropDownList ID="ddlExport" Font-Size="Smaller" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlExport_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                            <asp:ListItem Value="E">Excel</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table class="TableMain100">
                <tr id="TrAll">
                    <td style="text-align: left; vertical-align: top;">
                        <table border="0" id="td_search">
                            <tr>
                                <td>
                                    For a Date Range :</td>
                                <td>
                                    <dxe:ASPxDateEdit ID="dtFrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                        Font-Size="12px" Width="100px" ClientInstanceName="dtfrom">
                                        <dropdownbutton text="From">
                                    </dropdownbutton>
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td>
                                    <dxe:ASPxDateEdit ID="dtTo" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                        Font-Size="12px" Width="100px" ClientInstanceName="dtto">
                                        <dropdownbutton text="To">
                                    </dropdownbutton>
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td colspan="3">
                                </td>
                            </tr>
                            <tr id="tr_Group">
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
                            <tr id="tr_Clients">
                                <td>
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
                                <td colspan="2">
                                </td>
                            </tr>
                            <tr id="TR_REPORTTYPE">
                                <td>
                                    Report Type :</td>
                               <%-- <td>
                                </td>--%>
                                <td colspan="5">
                                    <asp:DropDownList ID="ddlType" runat="server" Width="200px" Font-Size="12px" >
                                        <asp:ListItem Value="0">Client Wise </asp:ListItem>
                                        <asp:ListItem Value="1">Branch/Group + Client Wise</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   
                                            Generation Type :</td>
                                            
                                        
                                        
                                    
                                
                                <%--<td></td>--%>
                                <td colspan="5"><asp:DropDownList ID="ddlgeneration" runat="server" Width="130px" Font-Size="12px"
                                                onchange="FnddlGeneration(this.value)">
                                                <asp:ListItem Value="SelectType">Select Type</asp:ListItem>
                                                <asp:ListItem Value="screen">Screen</asp:ListItem>
                                                <asp:ListItem Value="email">Send Email</asp:ListItem>
                                            </asp:DropDownList></td>
                            </tr>
                            <%--<tr id="tr_MailSendOption">
                            <td class="gridcellleft">
                               <asp:UpdatePanel ID="upanelRespective" runat="Server" UpdateMode="Conditional">
                               <ContentTemplate>
                                <table id="tabRespective" runat="Server" border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Respective :</td>
                                        <td>
                                            <asp:DropDownList ID="ddloptionformail" onchange="mailoption(this.value)" runat="server" Width="100px" Font-Size="12px"
                                                >
                                                
                                            </asp:DropDownList></td>
                                    </tr>
                                </table>
                                </ContentTemplate>
                                <Triggers> <asp:AsyncPostBackTrigger ControlID="Button1" /> </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>--%>
                        </table>
                    </td>
                    <td>
                        <table width="100%" id="showFilter">
                            <tr>
                                <td style="text-align: right; vertical-align: top; height: 134px;">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="gridcellleft">
                                                <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox>
                                                <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                    Enabled="false">
                                                    <asp:ListItem>Clients</asp:ListItem>
                                                    <asp:ListItem>Branch</asp:ListItem>
                                                    <asp:ListItem>Group</asp:ListItem>
                                                </asp:DropDownList>
                                                <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                                    style="color: #009900; text-decoration: underline; font-size: 8pt;">Add toList</span></a><span
                                                        style="color: #009900; font-size: 8pt;"> </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:ListBox ID="lstSlection" runat="server" Font-Size="12px" Height="90px" Width="300px">
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
                <tr id="TrBtn">
                    <td>
                        <table>
                            <td id="td_screen" runat="server">
                                <asp:Button ID="btn_show" runat="server" Text="Show" CssClass="btnUpdate" Height="23px"
                                    Width="101px" OnClientClick="selecttion()" OnClick="btn_show_Click" /></td>
                            <td id="td_email" runat="server">
                                <asp:Button ID="BtnEmail" runat="server" CssClass="btnUpdate" Height="20px" Text="Send Email"
                                    Width="120px" OnClick="BtnEmail_Click" OnClientClick="selecttion()" /></td>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table id="DATADISPLAY">
                        <tr>
                            <td style="display: none;">
                                <asp:DropDownList ID="cmbdistinct" runat="server" Font-Size="12px">
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>
                                <span id="spanshow1" style="color: Blue; font-weight: bold">Demat Charges Statement</span>
                                &nbsp;<span id="spanshow2"></span></td>
                        </tr>
                        <tr bordercolor="Blue">
                            <td align="center">
                                <table id="tblpage" cellspacing="0" cellpadding="0" runat="server" width="100%">
                                    <tr>
                                        <td width="20" style="padding: 5px">
                                            <asp:LinkButton ID="ASPxFirst" runat="server" Font-Bold="True" ForeColor="maroon"
                                                OnClientClick="javascript:showProgress();selecttion();" OnClick="ASPxFirst_Click">First</asp:LinkButton></td>
                                        <td width="25">
                                            <asp:LinkButton ID="ASPxPrevious" runat="server" Font-Bold="True" ForeColor="Blue"
                                                OnClientClick="javascript:showProgress();selecttion();" OnClick="ASPxPrevious_Click">Previous</asp:LinkButton></td>
                                        <td width="20" style="padding: 5px">
                                            <asp:LinkButton ID="ASPxNext" runat="server" Font-Bold="True" ForeColor="maroon"
                                                OnClientClick="javascript:showProgress();selecttion();" OnClick="ASPxNext_Click">Next</asp:LinkButton></td>
                                        <td width="20">
                                            <asp:LinkButton ID="ASPxLast" runat="server" Font-Bold="True" ForeColor="Blue" OnClientClick="javascript:showProgress();selecttion"
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
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btn_show" EventName="Click"></asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
            <table>
                <tr>
                    <td style="display: none;">
                        <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                        <asp:Button ID="btnhide" runat="server" Text="Button" OnClick="btnhide_Click" OnClientClick="selecttion()" /></td>
                </tr>
            </table>
            <table>
                <tr>
                    <td colspan="2" style="text-align: left; vertical-align: bottom;">
                        <table class="TableMain100">
                            <tr>
                                <td align="left" style="height: 93px">
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
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
  </asp:Content>
