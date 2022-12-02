<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.management_frmDPNetworthStatement" Codebehind="frmDPNetworthStatement.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />
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
      var FieldName;
    
       function PageLoad()
          {
            document.getElementById('txttdname').style.display='none';
            document.getElementById('btnfilter').style.display='none';
            HidObj('trheadline');
    
          }
      function ShowAccounts()
        {
            
          document.getElementById('txttdname').style.display='inline';
      
        }
       function HideAccounts()
        {
            
          document.getElementById('txttdname').style.display='none';
      
        }
        function HidFilter()
        {
             document.getElementById('tabFilter').style.display='none';
        
        }
        function ShowFilter()
        {
             document.getElementById('tabFilter').style.display='inline';
            HidObj('btnfilter');
        }
         function ShowObj(obj)
        {
             document.getElementById(obj).style.display='inline';
        
        }
         function HidObj(obj)
        {
             document.getElementById(obj).style.display='none';
        
        }
     function height()
    {
        if(document.body.scrollHeight>=500)
            window.frameElement.height = document.body.scrollHeight;
        else
            window.frameElement.height = '500px';
        window.frameElement.Width = document.body.scrollWidth;
    }
    
        function AllowNumericOnly(e)
    {
        var keycode;
        if (window.event) keycode = window.event.keyCode;
        else if (event) keycode = event.keyCode;
        else if(e) keycode = e.which;
        else return true;
        if( (keycode > 47 && keycode <= 57) )
        {
          return true;
        }
        else
        {
          return false;
        }
      return true;  
    }
    
    function FetchAccountTypes(objID,objListFun,objEvent)
        {
            
             ajax_showOptions(objID,objListFun,objEvent,'client','aa');
        
        }
        
      function btnAddEmailtolist_click()
            {
            
               // var cmb=document.getElementById('cmbsearch');
            
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
                        alert('Please search account types and then Add!')
                    var s=document.getElementById('txtName');
                    s.focus();
                    s.select();

            }
            
            
       function clientselection()
	        {
	           
	            var listBoxSubs = document.getElementById('SelectionList');
	            //var cmb=document.getElementById('cmbsearch');
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
                  
                    var sendData = 'Client' + '~' + listIDs;
                  
                    CallServer1(sendData,"");
                    document.getElementById('txttdname').style.display='none';
                 
                }
                else
                {
                alert("Please select account types from list.")
                }
             
	            var i;
                for(i=listBoxSubs.options.length-1;i>=0;i--)
                {
                    listBoxSubs.remove(i);
                 }
              
                 height();
           
                
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
        
        function btnRemoveEmailFromlist_click()
            {
                //selecttion();
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
           
           function EndCallBack()
            {
                grid.PerformCallback();
            }
            
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
    
    <table class="TableMain100">
                <tr>
                    <td class="EHEADER" colspan="0" style="text-align: center;">
                        <strong><span style="color: #000099">DP Networth Statement</span></strong>
                    </td>
                    <td width="35%" id="btnfilter">
                        <asp:DropDownList ID="ddlExport" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged1">
                            <asp:ListItem Value="Ex" Selected="True">Export</asp:ListItem>
                            <asp:ListItem Value="E">Excel</asp:ListItem>
                            <asp:ListItem Value="P">PDF</asp:ListItem>
                            </asp:DropDownList>||
                            <input id="Button1" type="button" value="Show Filter" class="btnUpdate" onclick="javascript: ShowFilter();"
                            style="width: 66px; height: 19px" />
                        
                       
                           
                    </td>
                </tr>
                <tr id="trheadline">
                    <td>
                        DP Networth Statement for <asp:Label ID="lblDate" runat="Server"></asp:Label>
                    </td>
                </tr>
            </table>
            
        <table id="tabFilter" border="0">
            <tr style="height:30px">
                <td style="width:97px">
                    Date
                
                </td>
                <td>
                    <dxe:ASPxDateEdit id="dtFrom" runat="server" ClientInstanceName="dtFrom" EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                    Font-Size="12px" UseMaskBehavior="True" Width="108px">
                                    <dropdownbutton text="Date">
                                        </dropdownbutton>
                                </dxe:ASPxDateEdit>
                
                </td>
                <td>
                   Select top <asp:TextBox ID="txtRecordNo" Width="50px" onkeypress="return AllowNumericOnly(this);" runat="Server"></asp:TextBox> accounts.
                </td>
            </tr>
            <tr style="height:30px">
                <td >
                    Account Types
                </td>
                <td >
                     <asp:RadioButton ID="rbAll" GroupName="acType" onclick="HideAccounts()" runat="Server" />All
                    &nbsp;&nbsp;&nbsp;
                    <asp:RadioButton ID="rbSpecific" GroupName="acType" onclick="ShowAccounts()" runat="server" />Specific
                </td>
                <td >
                
                </td>
            </tr>
            <tr>
               <td></td>
                <td colspan="2">
                         <table border="0" id="txttdname">
                                                <%--<table id="txttdname">--%>
                        
                                        <tr>
                                           
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
                                                    <asp:TextBox ID="txtName" runat="server" Font-Size="12px" Width="285px" onkeyup="FetchAccountTypes(this,'CDSLAccountType',event)"></asp:TextBox></span>
                                                             <a id="A3" href="javascript:void(0);" onclick="btnAddEmailtolist_click()">
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
                                            
                                    </table>
                </td>
            </tr>
            <tr style="height:30px">
                <td>
                    Show
                </td>
                <td colspan="2">
                    <asp:CheckBox ID="chkTradingCode" runat="server" />Trading Code 
                    &nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkPhoneNo" runat="server" />Phone Number
                </td>
                
            </tr>
            <tr style="height:30px">
                <td colspan="3">
                    <asp:CheckBox ID="chkConsiderAccounts" runat="server" />Consider Accounts whose networth is more than <asp:TextBox ID="txtNetworthPercentage" Width="50px"  onkeypress="return AllowNumericOnly(this);" runat="server"></asp:TextBox> % of Total Networth of all Accounts.
                </td>
            </tr>
            <tr style="height:50px">
                <td colspan="3">
                     <asp:Button ID="btnShow" runat="server" OnClick="btnShow_Click" CssClass="btnUpdate"
                            Text="Show" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    
                
                </td>
            </tr>
         </table>
         <table width="100%">
            <tr>
                <td>
                    <dxe:ASPxGridView ID="gridStatement" runat="server" Width="100%" ClientInstanceName="grid"
                                        AutoGenerateColumns="False" KeyFieldName="BenAcc" 
                                        >
                                        <SettingsBehavior AllowFocusedRow="True"></SettingsBehavior>
                                        <Styles>
                                            <Header SortingImageSpacing="5px" ImageSpacing="5px">
                                            </Header>
                                        </Styles>
                                        <SettingsPager PageSize="20">
                                        </SettingsPager>
                                        <ClientSideEvents SelectionChanged="function(s,e) {EndCallBack();}" />
                                        <Columns>
                                            <dxe:GridViewDataTextColumn FieldName="HolderName" Caption="Name" VisibleIndex="0">
                                                <Settings FilterMode="DisplayText" AutoFilterCondition="Contains"></Settings>
                                                <CellStyle CssClass="gridcellleft">
                                                </CellStyle>
                                                <FooterTemplate>
                                                    Total Holding Value
                                                </FooterTemplate>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="BenAcc" Caption="Account No"
                                                VisibleIndex="1">
                                                <Settings FilterMode="DisplayText" AutoFilterCondition="Contains"></Settings>
                                                <CellStyle CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="TRADINGUCC" Caption="Trading Ucc"
                                                VisibleIndex="2">
                                                <Settings FilterMode="DisplayText" AutoFilterCondition="Contains"></Settings>
                                                <CellStyle CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="AccountType" 
                                                Caption="Account Type" VisibleIndex="3">
                                                <Settings AllowAutoFilter="False"></Settings>
                                                <CellStyle HorizontalAlign="Left" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="Phone"
                                                Caption="Phone No" VisibleIndex="4">
                                                <Settings AllowAutoFilter="False"></Settings>
                                                <CellStyle HorizontalAlign="Right" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn PropertiesTextEdit-DisplayFormatString="#,##,###.000" FieldName="CURRENTBALANCE"
                                                Caption="Current Balance" VisibleIndex="5">
                                                <Settings AllowAutoFilter="False"></Settings>
                                                <CellStyle HorizontalAlign="Right" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn PropertiesTextEdit-DisplayFormatString="#,##,###.00" FieldName="VALUE"
                                                Caption="Value" VisibleIndex="6">
                                                <Settings AllowAutoFilter="False"></Settings>
                                                <CellStyle HorizontalAlign="Right" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                             <dxe:GridViewDataTextColumn FieldName="branch"
                                                Caption="Branch" VisibleIndex="7">
                                                <Settings AllowAutoFilter="False"></Settings>
                                                <CellStyle HorizontalAlign="Left" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                             <dxe:GridViewDataTextColumn FieldName="group"
                                                Caption="Group" VisibleIndex="8">
                                                <Settings AllowAutoFilter="False"></Settings>
                                                <CellStyle HorizontalAlign="Left" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                        </Columns>
                                        <Settings ShowTitlePanel="True" ShowStatusBar="Visible" ShowFooter="True"></Settings>
                                        <StylesEditors>
                                            <ProgressBar Height="25px">
                                            </ProgressBar>
                                        </StylesEditors>
                                        <TotalSummary>
                                            <dxe:ASPxSummaryItem FieldName="VALUE" ShowInColumn="Value" ShowInGroupFooterColumn="Value"
                                                SummaryType="Sum" Tag="Total Holding Value" DisplayFormat="#,##,###.00" />
                                        </TotalSummary>
                                    </dxe:ASPxGridView>
                </td>
            </tr>
         </table>
          <input id="hidClients" type="hidden"  runat="server"  />
          <input id="hidBranch" type="hidden"  runat="server"  />
          <input id="hidGroup" type="hidden"  runat="server"  />
    </div>
</asp:Content>
