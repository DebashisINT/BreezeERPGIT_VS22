<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_DeliveryCentreCommCurrency" Codebehind="DeliveryCentreCommCurrency.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script language="javascript" type="text/javascript">
    function Page_Load()///Call Into Page Load
            {
                 Hide('showFilter');
                 Fn_TypeChange('O');
                 fnddlGeneration('1');
                 NORECORD('3');
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
    function Fn_TypeChange(obj)
    {
        if(obj=='O')
        {
            Show('Tr_SearchByDate');
            Hide('tr_StocksZero');
            Hide('Tr_AccountType');
            Hide('Tr_Account');
            Show('td_For');
            Show('td_ForClient');
            Show('td_ForALLClient');
            Show('tr_Movement');
            Show('tr_Nature');
            Show('tr_SettNo');
            Show('tr_SettType');
            Fn_SearchBy(document.getElementById('ddlsearchBy').value);
        }
        if(obj=='M')
        {
            Hide('Tr_SearchByDate');
            Hide('tr_StocksZero');
            Hide('Tr_AccountType');
            Hide('Tr_Account');
            Show('td_For');
            Hide('td_ForClient');
            Show('td_ForALLClient');
            Show('tr_Movement');
            Show('tr_Nature');
            Hide('tr_SettNo');
            Hide('tr_SettType');
        }
        if(obj=='S')
        {
            Hide('Tr_SearchByDate');
            Show('tr_StocksZero');
            Show('Tr_AccountType');
            Show('Tr_Account');
            Hide('td_For');
            Hide('td_ForClient');
            Hide('td_ForALLClient');
            Hide('tr_Movement');
            Hide('tr_Nature');
            Show('tr_SettNo');
            Show('tr_SettType');
        }
         Hide('showFilter');
    }
     function fnddlGeneration(obj)
      {
        if(obj=='1')
        {
            Show('td_show');
            Hide('td_export');
        }
        if(obj=='2')
        {
            Hide('td_show');
            Show('td_export');
        }
         Hide('showFilter');
      }
   function Fn_AccountTypeChange(obj)
   {
        if(obj=='P')
        {
            Show('tr_SettNo');
            Show('tr_SettType');
            Show('tr_StocksZero');
            Show('Tr_Account');
        }
        if(obj=='M')
        {
            Hide('tr_SettNo');
            Hide('tr_SettType');
            Show('tr_StocksZero');
            Show('Tr_Account');
        }
        if(obj=='O')
        {
            Hide('tr_SettNo');
            Hide('tr_SettType');
            Show('tr_StocksZero');
            Show('Tr_Account');
        }
        if(obj=='A')
        {
            Hide('tr_SettNo');
            Hide('tr_SettType');
            Show('tr_StocksZero');
            Hide('Tr_Account');
        }
         Hide('showFilter');
   }
   function Fn_SearchBy(obj)
   {
        if(obj=='TDATE')
        {
            Show('tr_TradeDate');
            Hide('tr_PayOutDate');
        }
        else
        {
            Hide('tr_TradeDate');
            Show('tr_PayOutDate');
        }
   }
    function ForClient(obj)
        {
            if(obj=="a")
                Show('td_ForALLClient');
            else
                Hide('td_ForALLClient');
                
             Hide('showFilter');
        }
    function Fn_SettType(obj)
        {
            if(obj=="a")
                Hide('Td_SettType');
            else
                Show('Td_SettType');
                
            Hide('showFilter');
        }
     function Fn_SettNo(obj)
        {
            if(obj=="a")
                Hide('Td_SettNo');
            else
                Show('Td_SettNo');
           
            Hide('showFilter');
        }
    function Fn_Product(obj)
    {
          if(obj=="a")
                Hide('showFilter');
            else
            {
                var cmb=document.getElementById('cmbsearchOption');
                cmb.value='ScripCriteria';
                Show('showFilter');
            }
    }
    function Fn_Client(obj)
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
        ajax_showOptions(objID,objListFun,objEvent,document.getElementById('cmbsearchOption').value);
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
    
      function Fn_DPAccount(objID,objListFun,objEvent)
        {
            ajax_showOptions(objID,objListFun,objEvent,document.getElementById('ddlAccountType').value);
        }
      function NORECORD(obj)
      {
        if(obj=='1')
        {
            alert('No Record Found !!');
            Page_Load();
        
        }
        if(obj=='2')
            Page_Load();
                
            Show('Table_Selection');
            Hide('TABLE_SHOWRESULT');
            Hide('tr_filter');
            
            height();
      }
     function displaygrid(obj)
      {
        Hide('Table_Selection');
        Show('TABLE_SHOWRESULT');
        Show('tr_filter');
        document.getElementById(obj).focus();
        height();
      }
     function ChangeRowColor(rowID,rowNumber,obj) 
        { 
            var gridview = document.getElementById('grdDematCentre'); 
                       
            var rCount = gridview.rows.length; 
            var rowIndex=1;
            var rowCount=0;
            if(rCount==28)
                 rowCount=25;
            else    
               rowCount=rCount-2;
            if(rowNumber>25 && rCount<28)
                rowCount=rCount-3;
            for (rowIndex; rowIndex<=rowCount; rowIndex++) 
            { 
                var rowElement = gridview.rows[rowIndex]; 
                rowElement.style.backgroundColor='#FFFFFF'
            }
            var color = document.getElementById(rowID).style.backgroundColor;
            if(color != '#ffe1ac') 
            {
                oldColor = color;
            }
            if(color == '#ffe1ac') 
            {
                document.getElementById(rowID).style.backgroundColor = oldColor;
            }
            else 
                document.getElementById(rowID).style.backgroundColor = '#ffe1ac'; 

        }
   function Fn_ddlSelectaType(selectiontype,clientid,productid,settno,qty) 
   {
        if(selectiontype=='E')
        {
            var url='EnterTransactionCommCurrency.aspx?clientid='+clientid +'&productid='+productid +'&settno='+settno +'&qty='+qty ;
            OnMoreInfoClick(url,"Add Transaction Entries",'950px','450px',"Y");
        }
        if(selectiontype=='T')
        {
            var url='ShowTransactionCommCurrency.aspx?clientid='+clientid +'&productid='+productid +'&settno='+settno;
            OnMoreInfoClick(url,"Transactions",'950px','450px',"Y");
        }
        if(selectiontype=='S')
        {
            var url='ShowStocksCommCurrency.aspx?productid='+productid;
            OnMoreInfoClick(url,"Stocks",'950px','450px',"Y");
        }
   }
  function callback()
  {
     document.getElementById('btncallback').click();
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
                if(j[0]=='ScripCriteria')
                {
                    document.getElementById('HiddenField_ScripCriteria').value = j[1];
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
                    <strong><span id="SpanHeader" style="color: #000099">Delivery Centre</span></strong></td>
                <td class="EHEADER" width="10%" id="tr_filter" style="height: 22px">
                    <a href="javascript:void(0);" onclick="NORECORD(2);"><span style="color: Blue; text-decoration: underline;
                        font-size: 8pt; font-weight: bold">Filter</span></a>
                 
                </td>
            </tr>
        </table>
        <table id="Table_Selection">
            <tr valign="top">
                <td>
                    <table>
                        <tr>
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Type
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlType" runat="server" onchange="Fn_TypeChange(this.value)"
                                                Width="100px" Font-Size="11px">
                                                <asp:ListItem Value="O">Obligation</asp:ListItem>
                                                <asp:ListItem Value="M">Margin</asp:ListItem>
                                                <%-- <asp:ListItem Value="L">Loan</asp:ListItem>--%>
                                                <asp:ListItem Value="S">Stocks</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="Tr_AccountType">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            A/C Type :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlAccountType" runat="server" onchange="Fn_AccountTypeChange(this.value)"
                                                Width="100px" Font-Size="11px">
                                                <asp:ListItem Value="P">Pool A/C</asp:ListItem>
                                                <asp:ListItem Value="M">Margin A/C</asp:ListItem>
                                                <asp:ListItem Value="O">Own A/C</asp:ListItem>
                                                <asp:ListItem Value="A">All A/C</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="Tr_Account">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Account :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtAccount" runat="server" onkeyup="Fn_DPAccount(this,'SearchDpAccount',event)"
                                                Width="274px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="Tr_SearchByDate">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                           Search By :</td>
                                           <td> <asp:DropDownList ID="ddlsearchBy" runat="server" onchange="Fn_SearchBy(this.value)"
                                                Width="100px" Font-Size="11px">
                                                <asp:ListItem Value="TDATE">Trade Date</asp:ListItem>
                                                <asp:ListItem Value="PDATE">PayOut Date</asp:ListItem>
                                              
                                            </asp:DropDownList></td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr id="tr_TradeDate">
                                                                <td>
                                                                    <dxe:ASPxDateEdit ID="DtTradeFrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                                        Font-Size="12px" Width="108px" ClientInstanceName="dtfrom">
                                                                        <dropdownbutton text="From">
                                                            </dropdownbutton>
                                                                    </dxe:ASPxDateEdit>
                                                                </td>
                                                                <td>
                                                                    <dxe:ASPxDateEdit ID="DtTradeTo" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                                        Font-Size="12px" Width="108px" ClientInstanceName="dtto">
                                                                        <dropdownbutton text="To">
                                                            </dropdownbutton>
                                                                    </dxe:ASPxDateEdit>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr id="tr_PayOutDate">
                                                                <td>
                                                                    <dxe:ASPxDateEdit ID="DtPayoutFrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                                        Font-Size="12px" Width="108px" ClientInstanceName="dtfrom">
                                                                        <dropdownbutton text="From">
                                                            </dropdownbutton>
                                                                    </dxe:ASPxDateEdit>
                                                                </td>
                                                                <td>
                                                                    <dxe:ASPxDateEdit ID="DtPayoutTo" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                                        Font-Size="12px" Width="108px" ClientInstanceName="dtto">
                                                                        <dropdownbutton text="To">
                                                            </dropdownbutton>
                                                                    </dxe:ASPxDateEdit>
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
                       
                        <tr valign="top">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr valign="top">
                                        <td class="gridcellleft" bgcolor="#B7CEEC" id="td_For">
                                            For :</td>
                                        <td id="td_ForClient">
                                            <table border="10" cellpadding="1" cellspacing="1">
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="radClient" runat="server" Checked="True" GroupName="a" onclick="ForClient('a')" />
                                                    </td>
                                                    <td>
                                                        Client
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="radExchange" runat="server" GroupName="a" onclick="ForClient('b')" />
                                                    </td>
                                                    <td>
                                                        Exchange
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="radBoth" runat="server" GroupName="a" onclick="ForClient('b')" />
                                                    </td>
                                                    <td>
                                                        Both
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td id="td_ForALLClient">
                                            <table border="10" cellpadding="1" cellspacing="1">
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="Panel1" BorderColor="white" BorderWidth="1px" runat="server">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:RadioButton ID="radAll" runat="server" Checked="True" GroupName="b" onclick="Fn_Client('a')" />
                                                                    </td>
                                                                    <td>
                                                                        All Client
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="radPOAClient" runat="server" GroupName="b" onclick="Fn_Client('a')" />
                                                                    </td>
                                                                    <td>
                                                                        POA Client
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="radSelected" runat="server" GroupName="b" onclick="Fn_Client('b')" />
                                                                    </td>
                                                                    <td>
                                                                        Selected Client
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="tr_SettNo">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Settlement Number :
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdSettNoALL" runat="server" Checked="True" GroupName="cc" onclick="Fn_SettNo('a')" />
                                        </td>
                                        <td>
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdSettNoSpecific" runat="server" GroupName="cc" onclick="Fn_SettNo('b')" />
                                        </td>
                                        <td>
                                            Specific
                                        </td>
                                         <td style="display: none;" id="Td_SettNo">
                                            <asp:TextBox runat="server" Width="200px" Font-Size="12px" ID="txtSettNo" onkeyup="ajax_showOptions(this,'ShowClientFORMarginStocks',event,'CommDeliveryPositionSettlementNumber')"></asp:TextBox>
                                            <asp:TextBox ID="txtSettNo_hidden" runat="server" Width="14px" Style="display: none;"> </asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="tr_SettType">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Settlement Type :
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdSettNoTypeALL" runat="server" Checked="True" GroupName="cd"
                                                onclick="Fn_SettType('a')" />
                                        </td>
                                        <td>
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdSettNoTypeSpecific" runat="server" GroupName="cd" onclick="Fn_SettType('b')" />
                                        </td>
                                        <td>
                                            Specific
                                        </td>
                                        <td style="display: none;" id="Td_SettType">
                                            <asp:TextBox runat="server" Width="200px" Font-Size="12px" ID="txtSettType" onkeyup="ajax_showOptions(this,'ShowClientFORMarginStocks',event,'FetchSettlementTypeSpot')"></asp:TextBox>
                                            <asp:TextBox ID="txtSettType_hidden" runat="server" Width="14px" Style="display: none;"> </asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Product :
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbProductAll" runat="server" Checked="True" GroupName="c" onclick="Fn_Product('a')" />
                                        </td>
                                        <td>
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbProductSelected" runat="server" GroupName="c" onclick="Fn_Product('b')" />
                                        </td>
                                        <td>
                                            Selected
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="tr_Movement">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Movement :
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="radIncoming" runat="server" Checked="True" GroupName="d" />
                                        </td>
                                        <td>
                                            Incoming
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="radOutgoing" runat="server" GroupName="d" />
                                        </td>
                                        <td>
                                            Outgoing
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="radMovBoth" runat="server" GroupName="d" />
                                        </td>
                                        <td>
                                            Both
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="tr_Nature">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Nature :
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="radTransfered" runat="server" GroupName="e" />
                                        </td>
                                        <td>
                                            Transfered
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="radUntransfered" runat="server" Checked="True" GroupName="e" />
                                        </td>
                                        <td>
                                            UnTransfered
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="radNatBoth" runat="server" GroupName="e" />
                                        </td>
                                        <td>
                                            Both
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="tr_StocksZero">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Show :
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdStocksZero" runat="server" GroupName="f" Checked="True"/>
                                        </td>
                                        <td>
                                            Only Non Zero Stocks
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdStocksAll" runat="server" GroupName="f" />
                                        </td>
                                        <td>
                                            ALL
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr >
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Order By :
                                        </td>
                                      <td><table><tr>  <td>
                                            <asp:RadioButton ID="rdOrderBy1ST" runat="server" GroupName="ff" Checked="True"/>
                                        </td>
                                        <td>
                                            Client+Product+PayOut Date
                                        </td>
                                      </tr>
                                      <tr>  <td>
                                            <asp:RadioButton ID="rdOrderBy2ND" runat="server" GroupName="ff" />
                                        </td>
                                        <td>
                                             Product+PayOut Date+Client
                                        </td></tr></table></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Generate Type :</td>
                                        <td>
                                            <asp:DropDownList ID="ddlGeneration" runat="server" Width="130px" Font-Size="12px"
                                                onchange="fnddlGeneration(this.value)">
                                                <asp:ListItem Value="1">Screen</asp:ListItem>
                                                <asp:ListItem Value="2">Export</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td id="td_show">
                                            <asp:Button ID="btnshow" runat="server" CssClass="btnUpdate" Height="20px" Text="Screen"
                                                Width="101px" OnClientClick="selecttion()" OnClick="btnshow_Click" /></td>
                                        <td id="td_export">
                                           <asp:Button ID="btnexport" runat="server" CssClass="btnUpdate" Height="20px" Text="Export To Excel"
                                                Width="101px" OnClientClick="selecttion()" OnClick="btnexport_Click" />
                                                </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
              <td>
                       <table cellpadding="0" cellspacing="0" id="showFilter">
                                        <tr>
                                            <td >
                                               
                                                <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox>
                                                <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                    Enabled="false">
                                                    <asp:ListItem>Clients</asp:ListItem>
                                                    <asp:ListItem>ScripCriteria</asp:ListItem>
                                                   
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
            <tr>
                <td style="display: none;">
                    <asp:HiddenField ID="HiddenField_Client" runat="server" />
                    <asp:HiddenField ID="HiddenField_ScripCriteria" runat="server" />
                    <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                <asp:Button ID="btncallback" runat="server" CssClass="btnUpdate" Height="20px" Text="callback"
                    Width="101px" OnClick="btncallback_Click" /></td>
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
        <table id="TABLE_SHOWRESULT">
            <tr>
                <td>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                        <ContentTemplate>
                            <div id="display" runat="server">
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnshow" EventName="Click"></asp:AsyncPostBackTrigger>
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/green.bmp" />
                            </td>
                            <td class="gridcellleft">
                                POA Client
                            </td>
                            <td>
                                <asp:Image ID="Image2" runat="server" ImageUrl="~/images/blue.bmp" />
                            </td>
                            <td class="gridcellleft">
                                Own DP Client
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 226px">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="grdDematCentre" runat="server" Width="100%" BorderColor="CornflowerBlue"
                                AutoGenerateColumns="false" BorderStyle="Solid" BorderWidth="2px" CellPadding="4" 
                                ForeColor="#0000C0" OnRowCreated="grdDematCentre_RowCreated" OnRowDataBound="grdDematCentre_RowDataBound">
                                 <HeaderStyle ForeColor="Black" BorderWidth="1px" CssClass="EHEADER" BorderColor="AliceBlue"
                                        Font-Bold="False"></HeaderStyle>
                                <Columns>
                                    <asp:TemplateField HeaderText="Client">
                                        <ItemStyle Wrap="False" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCLIENTNAME" runat="server" Text='<%# Eval("CLIENTNAME")%>'></asp:Label>
                                            <asp:Label ID="lblColourType" Visible="false" runat="server" Text='<%# Eval("CLIENTTYPE")%>'></asp:Label>
                                            <asp:Label ID="lblclientid" Visible="false" runat="server" Text='<%# Eval("CLIENTID")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="UCC">
                                        <ItemStyle Wrap="False" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblUCC" runat="server" Text='<%# Eval("UCC")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Branch">
                                        <ItemStyle Wrap="False" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblBranch" runat="server" Text='<%# Eval("BRANCHCODE")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Settlement">
                                        <ItemStyle Wrap="False" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSETTNO" runat="server" Text='<%# Eval("SETTNO")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Product">
                                        <ItemStyle Wrap="False" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblScrip" runat="server" Text='<%# Eval("PRODUCTNAME")%>'></asp:Label>
                                             <asp:Label ID="lblproductid" Visible="false" runat="server" Text='<%# Eval("PRODUCTID")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Trade Date">
                                        <ItemStyle Wrap="False" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTradedate" runat="server" Text='<%# Eval("Tradedate")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PayOut">
                                        <ItemStyle Wrap="False" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDATE" runat="server" Text='<%# Eval("DATE")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Qty To Recv">
                                              <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblQtyToReceive" runat="server" Text='<%# Eval("QtyToReceive")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty Recvd">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblQtyReceived" runat="server" Text='<%# Eval("QtyReceived")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty To Delv">
                                              <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblQtyToDeliver" runat="server" Text='<%# Eval("QtyToDeliver")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty Delvrd">
                                               <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblQtyDelivered" runat="server" Text='<%# Eval("QtyDelivered")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pndg Incoming">
                                              <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblIPendingIncoming" runat="server" Text='<%# Eval("PendingIncoming")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pndg Outgoing">
                                              <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblPendingOutgoing" runat="server" Text='<%# Eval("PendingOutgoing")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Select">
                                              <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlSelectaType" runat="server" Width="130px" Font-Size="12px" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlSelectaType_SelectedIndexChanged">
                                                    <asp:ListItem Value="O">Select Type</asp:ListItem>
                                                    <asp:ListItem Value="T">Transaction</asp:ListItem>
                                                    <asp:ListItem Value="S">Stocks</asp:ListItem>
                                                    <asp:ListItem Value="M">Margin Stocks</asp:ListItem>
                                                    <asp:ListItem Value="E">Enter Transaction</asp:ListItem>
                                                </asp:DropDownList>
                                                  <asp:Label ID="lblQUANTITY" runat="server" Text='<%# Eval("QUANTITY")%>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                </Columns>
                                  <RowStyle BackColor="#FFFFFF" ForeColor="#330099" BorderColor="#BFD3EE" BorderStyle="Double"
                                        BorderWidth="1px"></RowStyle>
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnshow" EventName="Click"></asp:AsyncPostBackTrigger>
                            <asp:AsyncPostBackTrigger ControlID="btncallback" EventName="Click"></asp:AsyncPostBackTrigger>
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        </div>
   </asp:Content>