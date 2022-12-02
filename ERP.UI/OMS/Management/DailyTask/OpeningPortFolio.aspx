<%@ Page Title="Carry Forward of Stocks" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.DailyTask.management_DailyTask_OpeningPortFolio" Codebehind="OpeningPortFolio.aspx.cs" %>
<%--<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe000001" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../CSS/style.css" rel="stylesheet" type="text/css" />
  
     <script type="text/javascript" src="/assests/js/init.js"></script>
    <script type="text/javascript" src="/assests/js/ajaxList.js"></script>
    <script type="text/javascript" src="/assests/js/jquery-1.3.2.js"></script>
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
 function Page_Load()///Call Into Page Load
            {
                 FnDdlType("1");
                 height();
            }
   function height()
        {
            if(document.body.scrollHeight>=450)
            {
                window.frameElement.height = document.body.scrollHeight;
            }
            else
            {
                window.frameElement.height = '450px';
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
   function FnDdlType(obj)
   {
    if(obj=="1")
    {
        Show('Tr_CloseStock');
        Show('Tr_Client');
        Hide('Tr_SelectedClient');
        document.getElementById('rdbClientALL').checked=true;
        Hide('showFilter');
    }
    else
    {   
         Hide('Tr_Client');
         Hide('Tr_CloseStock');
         Show('Tr_SelectedClient');
         Hide('showFilter');
    }
     FnValuationMathod(document.getElementById('ddlclosmethod').value);
   } 
   
   function FnGenerate(obj)
   {
        FnDdlType(document.getElementById('ddlType').value);
        if(obj=="1")
           alert('Generate Successfully !!');
         if(obj=="2")
           alert('Please Select Client !!');
        height();
   } 
    function FnClients(obj)
        {
             if(obj=="a")
                Hide('showFilter');
             else
             {
                  var cmb=document.getElementById('cmbsearchOption');
                  cmb.value='Clients';
                  Show('showFilter');
             }
            
          
        }
    function FunClientScrip(objID,objListFun,objEvent)
    {
      ajax_showOptions(objID,objListFun,objEvent,"Clients");
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
            
             function FnValuationMathod(obj)
                {
                    if(obj=="0")
                      Hide('Tr_ValuationDate');
                    else
                      Show('Tr_ValuationDate');
                     height();
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

        }
        </script>	
    </asp:Content>
<%--<body style="margin: 0px 0px 0px 0px; background-color: #DDECFE">--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <%--<asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3600">
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
         $get('UpdateProgress1').style.display = 'block'; 
   } 
   function EndRequest(sender, args) 
   { 
          $get('UpdateProgress1').style.display = 'none'; 
 
   } 
            </script>
     <div class="panel-heading">
        <div class="panel-title">
            <h3>Carry Forward of Stocks</h3>
        </div>

    </div>
        <div class="form_main">
      <%--  <table class="TableMain100">
            <tr>
                <td class="EHEADER" colspan="0" style="text-align: center; height: 20px;">
                    <strong><span id="SpanHeader" style="color: #000099">Carry Forward of Stocks</span></strong></td>
            </tr>
        </table>--%>
        <table><tr><td class="gridcellleft"> <table border="10" cellpadding="0" cellspacing="0">
            <tr>
                <td  class="gridcellleft">
                    Type :</td>
                <td>
                    <table>
                        <tr>
                            <td style="width: 100px">
                    <asp:DropDownList ID="ddlType" runat="server" Width="150px" Font-Size="12px" onchange="FnDdlType(this.value)">
                        <asp:ListItem Value="1">Pro Stocks</asp:ListItem>
                        <asp:ListItem Value="2">Client Stock</asp:ListItem>
                       
                       
                    </asp:DropDownList></td>
                            <td style="width: 100px">
                                <asp:CheckBox id="chkConsBillDate" runat="server" Width="101px" Text="Consider Bill Date"></asp:CheckBox></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="Tr_CloseStock">
                <td class="gridcellleft" >
                    Closing Stock
                    Valuation Method :</td>
                <td>
                    <asp:DropDownList ID="ddlclosmethod" runat="server" Font-Size="11px" Width="150px"
                        Enabled="true" onchange="FnValuationMathod(this.value)">
                        <asp:ListItem Value="0">At Cost</asp:ListItem>
                        <asp:ListItem Value="1">At Market</asp:ListItem>
                        <asp:ListItem Value="2">Cost/Market Lower</asp:ListItem>
                        <asp:ListItem Value="3">Avg Cost/Market Lower</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="Tr_ValuationDate">
                <td class="gridcellleft" >
                    Valuation Date :</td>
                <td class="gridcellleft">
                <dxe:ASPxDateEdit ID="dtValuationDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                        Font-Size="12px" Width="108px" ClientInstanceName="dtValuationDate">
                        <dropdownbutton text="For">
                                        </dropdownbutton>
                    </dxe:ASPxDateEdit>&nbsp;
                    
                </td>
            </tr>
            <tr id="Tr_Client">
                <td class="gridcellleft" >
                    Select A Pro-Account :
                </td>
                <td>
                    <asp:TextBox ID="txtClient" runat="server" Width="200px" Font-Size="12px" onkeyup="ajax_showOptions(this,'ShowClientFORMarginStocks',event,'ProClients')"></asp:TextBox></td>
 
            </tr>
            <tr valign="top">
                <td colspan="2">
                    <table>
                        <tr valign="top">
                            <td>
                                <table>
                                    <tr id="Tr_SelectedClient">
                                        <td class="gridcellleft" >
                                            Clients :</td>
                                             <td>
                                            <asp:RadioButton ID="rdbClientALL" runat="server" GroupName="d" Checked="true" onclick="FnClients('a')" />
                                            ALL
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbClientSelected" runat="server" GroupName="d"  onclick="FnClients('b')"/>
                                            Selected
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table border="10" cellpadding="0" cellspacing="0" id="showFilter">
                                    <tr>
                                        <td id="TdFilter">
                                            <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox>
                                            <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                Enabled="false">
                                                <asp:ListItem>Clients</asp:ListItem>
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
               
            <tr>
                <td class="gridcellleft">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnshow" runat="server" CssClass="btnUpdate" Height="20px" Text="Generate"
                                Width="101px" OnClick="btnshow_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td> <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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
                    </asp:UpdateProgress></td>
            </tr>
            <tr>
                <td colspan="2" style="display: none;">
                    <asp:HiddenField ID="HiddenField_Client" runat="server" /><asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                    <asp:TextBox ID="txtClient_hidden" runat="server" Width="5px"></asp:TextBox></td>
            </tr>
           
        </table>
        </td>
        </tr>
        </table>
       
    </div>
   
<%--</body>--%>
    </asp:Content>