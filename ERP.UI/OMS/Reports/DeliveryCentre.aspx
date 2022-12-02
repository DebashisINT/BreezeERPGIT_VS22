<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_DeliveryCentre" Codebehind="DeliveryCentre.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
       function selecttion()
        {            
            var combo=document.getElementById('ddlExport');
            combo.value='Ex';
        }
        function AfterShow(obj)
        {
        selecttion();
             height();	
             Hide('TrAll');
	         Show('TrFilter');
	         Hide('TrButton');
	         Show('TrSpanSettlement');
	         Show('TdPoa');
	         document.getElementById('SpanDCenter').innerText=obj;        
        }
	    function Page_Load()
	    {
	     selecttion();
	        Hide('showFilter');
	        //Hide('TdClient');
	        Hide('TrFilter');
	        Hide('TrAccountType1');
	        Hide('TrAccountType2');
	        Hide('TrShow');
	        Hide('TdSettlementType');
	        Hide('TdPoa');
	        height();	        
	    }
	    function ForFilter()
	    {
	     selecttion();
	        Show('TrAll');
	        Hide('TrFilter');
	        Show('TrButton'); 
	        Hide('TrSpanSettlement');   
	    }
	    function height()
        {
            if(document.body.scrollHeight>=500)
            {
                window.frameElement.height = document.body.scrollHeight;
            }
            else
            {
                window.frameElement.height = '500';
            }
            window.frameElement.width = document.body.scrollWidth;
        }
        function Show(objID)
        {
            document.getElementById(objID).style.display='inline';
        }
        function Hide(objID)
        {
            document.getElementById(objID).style.display='none';
        }
        function FunSettNumber(objID,objListFun,objEvent)
        {
            var type;
            var Seg=document.getElementById('radSegAll');            
            if(Seg.checked==true)
            {
                type='A';
            }
            else
                type='S';
            ajax_showOptions(objID,objListFun,objEvent,type,'Sub');
        }
        function FunSettType(objID,objListFun,objEvent)
        {
            ajax_showOptions(objID,objListFun,objEvent,document.getElementById('txtSettlementNumber').value,'Sub');
        }
        function FunClientScrip(objID,objListFun,objEvent)
        {
            var cmbVal=document.getElementById('cmbsearchOption').value;
            var Seg=document.getElementById('radSegAll');            
            if(Seg.checked==true)
            {
                if(cmbVal=='Scrips')
                    cmbVal='ScripsAllSeg'
            }
            ajax_showOptions(objID,objListFun,objEvent,cmbVal,'Sub');
        }
        function DPAccount(objID,objListFun,objEvent)
        {
            var cmbVal=document.getElementById('ddlAccountType').value;
            ajax_showOptions(objID,objListFun,objEvent,cmbVal,'Sub');
        }
        function Settlement(obj)
        {
            if(obj=="a")
                Hide('TdSettlement');
            else
                Show('TdSettlement');
        }
        function SettlementType(obj)
        {
            if(obj=="a")
                Hide('TdSettlementType');
            else
                Show('TdSettlementType');
        }
        function forClient(obj)
        {
            if(obj=="a")
                Show('TdClient');
            else
                Hide('TdClient');
        }
        function Client(obj)
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
        function Scrip(obj)
        {
             if(obj=="a")
                Hide('showFilter');
             else
             {
                  var cmb=document.getElementById('cmbsearchOption');
                  cmb.value='Scrips';
                  Show('showFilter');
             }
        }
        function btnAddsubscriptionlist_click()
        {
            var userid = document.getElementById('txtsubscriptionID');
            if(userid.value != '')
            {
                var ids = document.getElementById('txtsubscriptionID_hidden');
                var listBox = document.getElementById('lstSuscriptions');
                var tLength = listBox.length;
                //alert(tLength);
                
                var no = new Option();
                no.value = ids.value;
                no.text = userid.value;
                listBox[tLength]=no;
                var recipient = document.getElementById('txtsubscriptionID');
                recipient.value='';
            }
            else
                alert('Please search name and then Add!')
            var s=document.getElementById('txtsubscriptionID');
            s.focus();
            s.select();
        }
        function btnRemovefromsubscriptionlist_click()
        {
            
            var listBox = document.getElementById('lstSuscriptions');
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
        function clientselectionfinal()
	    {
	        var listBoxSubs = document.getElementById('lstSuscriptions');
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
	    FieldName='lstSuscriptions'; 
	    function Transaction(objID,objScrip,objSettNumType,objISIN,objClient)
	    {
	        var url="Transaction.aspx?id="+objID;
            OnMoreInfoClick(url,"Transaction For : "+objScrip+" ["+objISIN+"],"+objSettNumType+" For : "+objClient+"",'940px','450px',"N");
	    } 
	    function Stock(objScrip,objProductID)
	    {
	        var url="Stock.aspx?id="+objProductID;
            OnMoreInfoClick(url,"Stock Position For : "+objScrip+"",'940px','450px',"N");
	    }
	    function TransactionStock(objSettNumType,objScrip,objISIN)
	    {
	        var DpAccount=document.getElementById('TxtAccount_hidden').value;
	        var aCCType=document.getElementById('ddlAccountType').value;
	        var url="Transaction.aspx?id="+DpAccount+" &ISIN="+objISIN+" &Type="+aCCType+" &SettType="+objSettNumType;
	        OnMoreInfoClick(url,"Transaction For : "+objScrip+" ["+objISIN+"]",'940px','450px',"N");
	    }
	    function TypeChange(objTypeVal)
	    {
	        if(objTypeVal=='M')
	        {
	            Hide('TrSettlement');
	            Hide('TrSettlementType');
	            Hide('TrAccountType1');
	            Hide('TrAccountType2');
	            Show('TrFor');
	            Show('TrClients');
	            Show('TrMovement');
	            Show('trGroupBy');
	            Hide('TrShow');
	            Hide('TrStocks');
	            Show('TrDematCentre');
	            Hide('TrClientFor');
	            Show('TdClient');
	            document.getElementById('rdbSettTypeSelected').checked = true;
                document.getElementById('rdbSettTypeAll').checked = false;
                Show('TdSettlement');
	        }
	        else if(objTypeVal=='O')
	        {
	            Show('TrSettlement');
	            Show('TrSettlementType');
	            Hide('TrAccountType1');
	            Hide('TrAccountType2');
	            Show('TrFor');
	            Show('TrClients');
	            Show('TrMovement');
	            Show('trGroupBy');
	            Hide('TrShow');
	            Hide('TrStocks');
	            Show('TrDematCentre');
	            Show('TrClientFor');
	            Hide('TdClient');
	            document.getElementById('rdbSettTypeSelected').checked = true;
                document.getElementById('rdbSettTypeAll').checked = false;
                Show('TdSettlement');
	        }
	        else if(objTypeVal=='S')
	        {
	            Show('TrSettlement');
	            Show('TrSettlementType');
	            Show('TrAccountType1');
	            Show('TrAccountType2');
	            Hide('TrFor');
	            Show('TrClients');
	            Hide('TrMovement');
	            Hide('trGroupBy');
	            Show('TrShow');
	            Show('TrStocks');
	            Hide('TrDematCentre');
	            document.getElementById('rdbSettTypeSelected').checked = false;
                document.getElementById('rdbSettTypeAll').checked = true;
                Hide('TdSettlement');
	        }
	    }
//	    function ForTransaction()
//	    {
//	         var url='../management/InterSettlementNSECM.aspx?ID='+objVal;
//             OnMoreInfoClick(url,"Add Transaction Entries",'985px','450px',"Y");
//	    }
	    function EnterTransaction(Client,Ucc,SettNo,Scrip,Isin,Incoming,OutGoing,prdID,CustID)
	    {
	        var Quantity;
            var mode;
            var radBoth = document.getElementById('radMovBoth');
            if(radBoth.checked==true)
            {
                var Incoming=Incoming;
                if(Incoming!='')
                {
                    Quantity=Incoming;
                    mode="I";
                }
                var OutGoing=OutGoing;
                if(OutGoing!='')
                {
                    Quantity=OutGoing;
                    mode="O";
                }
            }
            var radIncoming = document.getElementById('radIncoming');
            if(radIncoming.checked==true)
            {
                Quantity=Incoming;
                mode="I";
            }
            var radOutgoing = document.getElementById('radOutgoing');
            if(radOutgoing.checked==true)
            {
                Quantity=OutGoing;
                mode="O";
            }
            var cli=Client.replace("&", "-");
            var Script=Scrip.replace("&", "-");
            
            objVal='INOUT'+'~'+cli+'~'+Ucc+'~'+Script+'~'+Isin+'~'+Quantity+'~'+mode+'~'+SettNo+'~'+prdID+'~'+CustID
            //alert(objVal);
            var url='../management/InterSettlementNSECM.aspx?ID='+objVal;
            OnMoreInfoClick(url,"Add Transaction Entries",'985px','450px',"Y");
	    }
	    function MarGinHoldBack(objClient,objBranch,objClientName,UCC)
	    {
	        var url='../management/MarginHoldback.aspx?Client='+objClient+' &Branch='+objBranch;
             OnMoreInfoClick(url,"Show Margin/Holdback Stocks For : "+objClientName+" [ "+UCC+" ]",'985px','450px',"Y");
	    }
	    function callback()
        {
            document.getElementById('Button1').click();
        }
	    function AccountTypeChange(objValForACType)
	    {
	        if(objValForACType=='P')
	        {
	            Show('TrSettlement');
	            Show('TrAccountType2');
	            Show('TrSettlementType');
	        }
	        else if(objValForACType=='A')
	        {
	            Hide('TrAccountType2');
	            Hide('TrSettlement');
	            Hide('TrSettlementType');
	        }
	        else
	        {
	            Hide('TrSettlement');
	            Hide('TrSettlementType');
	            Show('TrAccountType2');
	        }
	    }	    
	    document.body.style.cursor = 'pointer'; 
         var oldColor = '';
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
        function ChangeRowColor1(rowID,rowNumber,obj) 
        {             
            
            var gridview = document.getElementById('grdDematStocks'); 
            objVal='STOCK'+'~'+1;
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
        function ShowMarketTraker(TickerCode,ExchID,ProductName)
        {
            parent.MarketTracker(ProductName,ExchID,TickerCode);
        }
        function TradeRegister(objCustomer,objProduct,objSettNumber,objSettType,objSegment,objDID,objClientName,objUCC)
        {
             var url="frmReport_TradeRegisteriframe.aspx?Custid="+objCustomer+" &ProdID="+objProduct+" &SettNo="+objSettNumber+" &SettType="+objSettType+" &Segment="+objSegment+" &DematID="+objDID;
            OnMoreInfoClick(url,"Show Trade Register : "+objClientName+" ["+objUCC+"]",'985px','450px',"N");
        }
    </script>

    <script type="text/ecmascript">
        
	    function ReceiveServerData(rValue)
        {
            var Data=rValue.split('~');                     
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
                    <td class="EHEADER" style="text-align: center;">
                        <strong><span style="color: #000099">Delivery Centre</span></strong>
                    </td>
                </tr>
            </table>
            <table class="TableMain100">
                <tr id="TrAll">
                    <td style="text-align: left; vertical-align: top;" class="gridcellleft">
                        <table border="10" cellpadding="1" cellspacing="1">
                            <tr>
                                <td class="gridcellleft" bgcolor="#B7CEEC">
                                    Type
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlType" runat="server" onchange="TypeChange(this.value)" Width="128px">
                                        <asp:ListItem Value="O">Obligation</asp:ListItem>
                                        <asp:ListItem Value="M">Margin</asp:ListItem>
                                        <asp:ListItem Value="L">Loan</asp:ListItem>
                                        <asp:ListItem Value="S">Stocks</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr id="TrAccountType1">
                                <td class="gridcellleft" bgcolor="#B7CEEC">
                                    A/C Type :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlAccountType" runat="server" onchange="AccountTypeChange(this.value)"
                                        Width="128px">
                                        <asp:ListItem Value="P">Pool A/C</asp:ListItem>
                                        <asp:ListItem Value="M">Margin A/C</asp:ListItem>
                                        <asp:ListItem Value="O">Own A/C</asp:ListItem>
                                        <asp:ListItem Value="A">All A/C</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr id="TrAccountType2">
                                <td class="gridcellleft" bgcolor="#B7CEEC">
                                    Account :
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtAccount" runat="server" onkeyup="DPAccount(this,'SearchDpAccount',event)"
                                        Width="274px"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft" bgcolor="#B7CEEC">
                                    Segment
                                </td>
                                <td style="text-align: left;" colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="radSegAll" runat="server" GroupName="Segment" />
                                            </td>
                                            <td>
                                                All
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="radSegSpecific" runat="server" Checked="True" GroupName="Segment" />
                                            </td>
                                            <td>
                                                Specific
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="TrSettlement">
                                <td class="gridcellleft" bgcolor="#B7CEEC">
                                    Settlement Number
                                </td>
                                <td style="text-align: left;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdbSettTypeAll" runat="server" GroupName="a" onclick="Settlement('a')" />
                                            </td>
                                            <td>
                                                All
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdbSettTypeSelected" runat="server" Checked="True" GroupName="a"
                                                    onclick="Settlement('b')" />
                                            </td>
                                            <td>
                                                Specific
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td id="TdSettlement">
                                    <asp:TextBox ID="txtSettlementNumber" runat="server" onkeyup="FunSettNumber(this,'SearchSettlementNumberAll',event)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="TrSettlementType">
                                <td class="gridcellleft" bgcolor="#B7CEEC">
                                    Settlement Type
                                </td>
                                <td style="text-align: left;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdbSettTypeAll1" runat="server" GroupName="1a" Checked="True"
                                                    onclick="SettlementType('a')" />
                                            </td>
                                            <td>
                                                All
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdbSettTypeSelected1" runat="server" GroupName="1a" onclick="SettlementType('b')" />
                                            </td>
                                            <td>
                                                Specific
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td id="TdSettlementType">
                                    <asp:TextBox ID="txtSettlementType" runat="server" onkeyup="FunSettType(this,'SearchSettlementType',event)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="TrFor">
                                <td class="gridcellleft" bgcolor="#B7CEEC">
                                    For
                                </td>
                                <td style="text-align: left;" id="TrClientFor">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="radClient" runat="server" Checked="True" GroupName="a1" onclick="forClient('a')" />
                                            </td>
                                            <td>
                                                Client
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="radExchange" runat="server" GroupName="a1" onclick="forClient('b')" />
                                            </td>
                                            <td>
                                                Exchange
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="radBoth" runat="server" GroupName="a1" onclick="forClient('b')" />
                                            </td>
                                            <td>
                                                Both
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="text-align: left;" id="TdClient">
                                    <asp:Panel ID="Panel1" runat="server">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:RadioButton ID="radAll" runat="server" Checked="True" GroupName="a2" onclick="Client('a')" />
                                                </td>
                                                <td>
                                                    All Client
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="radPOAClient" runat="server" GroupName="a2" onclick="Client('a')" />
                                                </td>
                                                <td>
                                                    POA Client
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="radSelected" runat="server" GroupName="a2" onclick="Client('b')" />
                                                </td>
                                                <td>
                                                    Selected Client
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr id="TrClients">
                                <td class="gridcellleft" bgcolor="#B7CEEC">
                                    Scrips
                                </td>
                                <td style="text-align: left;" colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdbScripsAll" runat="server" Checked="True" GroupName="b" onclick="Scrip('a')" />
                                            </td>
                                            <td>
                                                All
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdbScripsSelected" runat="server" GroupName="b" onclick="Scrip('b')" />
                                            </td>
                                            <td>
                                                Selected
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="TrMovement">
                                <td class="gridcellleft" bgcolor="#B7CEEC">
                                    Movement
                                </td>
                                <td style="text-align: left;" colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="radIncoming" runat="server" Checked="True" GroupName="b1" />
                                            </td>
                                            <td>
                                                Incoming
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="radOutgoing" runat="server" GroupName="b1" />
                                            </td>
                                            <td>
                                                Outgoing
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="radMovBoth" runat="server" GroupName="b1" />
                                            </td>
                                            <td>
                                                Both
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trGroupBy">
                                <td class="gridcellleft" bgcolor="#B7CEEC">
                                    Nature
                                </td>
                                <td style="text-align: left;" colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="radTransfered" runat="server" GroupName="b2" />
                                            </td>
                                            <td>
                                                Transfered
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="radUntransfered" runat="server" Checked="True" GroupName="b2" />
                                            </td>
                                            <td>
                                                UnTransfered
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="radNatBoth" runat="server" GroupName="b2" />
                                            </td>
                                            <td>
                                                Both
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="TrShow">
                                <td class="gridcellleft">
                                    Show
                                </td>
                                <td style="text-align: left;" colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="radShowNonZero" runat="server" Checked="true" GroupName="b7" />
                                            </td>
                                            <td>
                                                Only Non Zero Stocks
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="radShowAll" runat="server" GroupName="b7" />
                                            </td>
                                            <td>
                                                All
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="text-align: right; vertical-align: top;">
                        <table width="100%" id="showFilter">
                            <tr>
                                <td style="text-align: right; vertical-align: top">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td class="gridcellleft" style="vertical-align: top; text-align: left" id="TdFilter">
                                                            <asp:TextBox ID="txtsubscriptionID" runat="server" Font-Size="12px" Width="150px"
                                                                onkeyup="FunClientScrip(this,'ShowClientScrip',event)"></asp:TextBox>
                                                            <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                                Enabled="false">
                                                                <asp:ListItem>Clients</asp:ListItem>
                                                                <asp:ListItem>Scrips</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                                                style="color: #009900; text-decoration: underline; font-size: 8pt;">Add List</span></a><span
                                                                    style="color: #009900; font-size: 8pt;"> </span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:ListBox ID="lstSuscriptions" runat="server" Font-Size="12px" Height="90px" Width="290px">
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
                            <tr style="display: none">
                                <td>
                                    <asp:TextBox ID="txtsubscriptionID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                    <asp:HiddenField ID="txtSettlementNumber_hidden" runat="server" />
                                    <asp:HiddenField ID="TxtAccount_hidden" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <div id='Div2' style='position: absolute; font-family: arial; font-size: 30; background-color: white;
                                    layer-background-color: white;'>
                                    <table border='1' cellpadding='0' cellspacing='0' bordercolor='#C0D6E4'>
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
                <tr id="TdPoa">
                    <td colspan="2">
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
                    <td id="TrSpanSettlement">
                        <strong><span id="SpanDCenter" style="color: #000099"></span></strong>
                    </td>
                    <td style="text-align: right;" id="TrFilter">
                        <%--<span style="font-weight: bold; color: Blue; cursor: pointer" onclick="javascript:ForTransaction();">
                            Enter Transaction</span> ||--%> 
                            <span style="font-weight: bold; color: Blue; cursor: pointer"
                                onclick="javascript:ForFilter();">Filter</span>
                        <%--<asp:DropDownList ID="ddlExport" runat="server" AutoPostBack="True">
                            <asp:ListItem Value="Ex" Selected="True">Export</asp:ListItem>
                            <asp:ListItem Value="E">Excel</asp:ListItem>
                            <asp:ListItem Value="P">PDF</asp:ListItem>
                        </asp:DropDownList>--%>
                        
                           <asp:DropDownList ID="ddlExport" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged1">
                            <asp:ListItem Value="Ex" Selected="True">Export</asp:ListItem>
                            <asp:ListItem Value="E">Excel</asp:ListItem>                           
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="TrButton">
                    <td colspan="2">
                        <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="btnUpdate" Height="22px"
                            Width="106px" OnClick="btnShow_Click" />
                    </td>
                </tr>
                <tr id="TrDematCentre">
                    <td colspan="2">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="grdDematCentre" runat="server" Width="100%" BorderColor="CornflowerBlue"
                                    ShowFooter="True" AllowSorting="true" AutoGenerateColumns="false" BorderStyle="Solid"
                                    BorderWidth="2px" CellPadding="4" ForeColor="#0000C0" OnRowCreated="grdDematCentre_RowCreated"
                                    OnSorting="grdDematCentre_Sorting" OnRowDataBound="grdDematCentre_RowDataBound">
                                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                                    <Columns>
                                        <asp:TemplateField Visible="false">
                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Center"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("dematPosition_ID")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Segment" SortExpression="DematPosition_SegmentID">
                                            <ItemStyle BorderWidth="1px" Wrap="false" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDematPosition_SegmentID" runat="server" Text='<%# Eval("DematPosition_SegmentID")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Client" SortExpression="Client">
                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblClient" runat="server" Text='<%# Eval("Client")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="UCC" SortExpression="UCC">
                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblUCC" runat="server" Text='<%# Eval("UCC")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Branch" SortExpression="Branch">
                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblBranch" runat="server" Text='<%# Eval("Branch")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Settlement" SortExpression="Settlement">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSettlement" runat="server" Text='<%# Eval("Settlement")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Scrip" SortExpression="Scrip">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblScrip" runat="server" Text='<%# Eval("Scrip")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ISIN" SortExpression="DematPosition_ISIN">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblISIN" runat="server" Text='<%# Eval("DematPosition_ISIN")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty To Recv">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblQtyRec" runat="server" Text='<%# Eval("DematPosition_QtyToReceive")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty Recvd">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblQtyRecvd" runat="server" Text='<%# Eval("DematPosition_QtyReceived")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty To Delv">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblQtyDeliver" runat="server" Text='<%# Eval("DematPosition_QtyToDeliver")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty Delvrd">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblQtyDelivered" runat="server" Text='<%# Eval("DematPosition_QtyDelivered")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pndg Incoming">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblIncomingPending" runat="server" Text='<%# Eval("IncomingPending")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pndg Outgoing">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblOutgoingPending" runat="server" Text='<%# Eval("OutgoingPending")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblProID" runat="server" Text='<%# Eval("DematPosition_ProductSeriesID")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDematPosition_BranchID" runat="server" Text='<%# Eval("DematPosition_BranchID")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDematPosition_CustomerID" runat="server" Text='<%# Eval("DematPosition_CustomerID")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Transaction">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <a href="javascript:Transaction('<%# Eval("dematPosition_ID")%>','<%# Eval("Scrip")%>','<%# Eval("Settlement")%>','<%# Eval("DematPosition_ISIN")%>','<%# Eval("Client")%>')">
                                                    Show</a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Stocks">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <a href="javascript:Stock('<%# Eval("Scrip")%>','<%# Eval("DematPosition_ProductSeriesID")%>')">
                                                    Show</a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Margn Stocks">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <a href="javascript:MarGinHoldBack('<%# Eval("DematPosition_CustomerID")%>','<%# Eval("DematPosition_BranchID")%>','<%# Eval("Client")%>','<%# Eval("UCC")%>')">
                                                    Show</a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Enter Transaction">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <a href="javascript:EnterTransaction('<%# Eval("Client")%>','<%# Eval("UCC")%>','<%# Eval("Settlement")%>','<%# Eval("Scrip")%>','<%# Eval("DematPosition_ISIN")%>','<%# Eval("IncomingPending")%>','<%# Eval("OutgoingPending")%>','<%# Eval("DematPosition_ProductSeriesID")%>','<%# Eval("DematPosition_CustomerID")%>')">
                                                    Show</a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblColourType" runat="server" Text='<%# Eval("ColourType")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblTickerCode" runat="server" Text='<%# Eval("Equity_TickerCode")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblExchSegmentID" runat="server" Text='<%# Eval("Equity_ExchSegmentID")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblTickerSymbol" runat="server" Text='<%# Eval("Equity_TickerSymbol")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField Visible="false">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblPhoneNumber" runat="server" Text='<%# Eval("PhoneNumber")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle BackColor="#FFFFFF" ForeColor="#330099" BorderColor="#BFD3EE" BorderStyle="Double"
                                        BorderWidth="1px"></RowStyle>
                                    <EditRowStyle BackColor="#E59930"></EditRowStyle>
                                    <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
                                    <PagerStyle ForeColor="White" HorizontalAlign="Center"></PagerStyle>
                                    <HeaderStyle ForeColor="Black" BorderWidth="1px" CssClass="EHEADER" BorderColor="AliceBlue"
                                        Font-Bold="False"></HeaderStyle>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click"></asp:AsyncPostBackTrigger>
                                <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr id="TrStocks">
                    <td colspan="2">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="grdDematStocks" runat="server" Width="100%" BorderColor="CornflowerBlue"
                                    ShowFooter="True" AllowSorting="true" AutoGenerateColumns="false" BorderStyle="Solid"
                                    BorderWidth="2px" CellPadding="4" ForeColor="#0000C0" OnRowCreated="grdDematStocks_RowCreated" OnRowDataBound="grdDematStocks_RowDataBound">
                                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                                    <Columns>
                                        <asp:TemplateField HeaderText="A/C Name">
                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblACName" runat="server" Text='<%# Eval("ACName")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Scrip">
                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblScrip" runat="server" Text='<%# Eval("Scrip")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ISIN">
                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblISIN" runat="server" Text='<%# Eval("DematStocks_ISIN")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Settlement">
                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSettlement" runat="server" Text='<%# Eval("Settlement")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Opening Qty">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblOpeningQty" runat="server" Text='<%# Eval("OpeningQty")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="In Qty">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblInQty" runat="server" Text='<%# Eval("InQty")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Out Qty">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblQtyRec" runat="server" Text='<%# Eval("OutQty")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pledge Qty">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblPledgeQty" runat="server" Text='<%# Eval("PledgeQty")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Blocked">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblBlocked" runat="server" Text='<%# Eval("Blocked")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Free Balance">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblFreeBalance" runat="server" Text='<%# Eval("FreeBalance")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Transaction">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <a href="javascript:TransactionStock('<%# Eval("Settlement")%>','<%# Eval("Scrip")%>','<%# Eval("DematStocks_ISIN")%>')">
                                                    Show</a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblETickerCode" runat="server" Text='<%# Eval("Equity_TickerCode")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblEExchSegmentID" runat="server" Text='<%# Eval("Equity_ExchSegmentID")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblETickerSymbol" runat="server" Text='<%# Eval("Equity_TickerSymbol")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle BackColor="#FFFFFF" ForeColor="#330099" BorderColor="#BFD3EE" BorderStyle="Double"
                                        BorderWidth="1px"></RowStyle>
                                    <EditRowStyle BackColor="#E59930"></EditRowStyle>
                                    <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
                                    <PagerStyle ForeColor="White" HorizontalAlign="Center"></PagerStyle>
                                    <HeaderStyle ForeColor="Black" BorderWidth="1px" CssClass="EHEADER" BorderColor="AliceBlue"
                                        Font-Bold="False"></HeaderStyle>
                                    <%--<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>--%>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click"></asp:AsyncPostBackTrigger>
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr style="display: none">
                    <td colspan="2">
                        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
                    </td>
                </tr>
            </table>
        </div>
  </asp:Content>
