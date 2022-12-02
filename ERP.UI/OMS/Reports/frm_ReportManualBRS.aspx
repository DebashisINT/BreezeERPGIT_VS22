<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frm_ReportManualBRS" Codebehind="frm_ReportManualBRS.aspx.cs" %>

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
	
	</style>

    <script language="javascript" type="text/javascript">

  
    function Page_Load()///Call Into Page Load
            {
                 Hide('td_filter');
                 document.getElementById('hiddencount').value=0;
                 FnReportView('1');
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
    function RecordDisplay()
    {
        Hide('showFilter');
        Show('td_filter');
        Hide('tab1');
        Show('displayAll');
        document.getElementById('hiddencount').value=0;
        selecttion();
        height();
        
    }
    function fnNoRecord(obj)
    {
        Hide('showFilter');
        Hide('td_filter');
        Show('tab1');
        Hide('displayAll');
        document.getElementById('hiddencount').value=0;
        if(obj=='1')
            alert('No Record Found!!');
        if(obj=='3')
            alert('Please Select Bank Name!!');
        selecttion();
        FnReportView(document.getElementById('DdlReportView').value);
        
    }
    function FnReportView(obj)
    {
        if(obj=='2')///Summary
        {
            Hide('Tr_BankDetail');
            Show('Tr_BankSummary');
        }
        else
        {
            Show('Tr_BankDetail');
            Hide('Tr_BankSummary');
        }
        Hide('showFilter');
        FnddlGeneration(document.getElementById('ddlGeneration').value);     
        height();
    }
 
  function FnddlGeneration(obj)
  {
    if(obj=="1")
    {
        Show('td_Screen');
        Hide('td_Export');
    }
    if(obj=="2")
    {
        Hide('td_Screen');
        Show('td_Export');
    }
     Hide('showFilter');
  }
  function selecttion()
  {
     var combo=document.getElementById('cmbExport');
     combo.value='Ex';
  }
   function btnAddsubscriptionlist_click()
    {
    
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
                  
                    var sendData =listIDs;
                    CallServer(sendData,"");
                   
                }
	            var i;
                for(i=listBoxSubs.options.length-1;i>=0;i--)
                {
                    listBoxSubs.remove(i);
                }
            
                Hide('showFilter');
                document.getElementById('btnScreen').disabled=false;
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
      function fnBank(obj)
      {
            if(obj=="a")
                Hide('showFilter');
             else
                Show('showFilter');
         selecttion();
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
    FieldName='lstSlection';
    </script>

    <script type="text/ecmascript">   
       function ReceiveServerData(rValue)
        {
              
               document.getElementById('HiddenField_BRSAC').value = rValue;
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
                        <strong><span id="SpanHeader" style="color: #000099">Bank Reconciliation Statement</span></strong></td>
                    <td class="EHEADER" width="15%" id="td_filter" style="height: 22px">
                        <a href="javascript:void(0);" onclick="fnNoRecord(2);"><span style="color: Blue;
                            text-decoration: underline; font-size: 8pt; font-weight: bold">Filter</span></a>
                        <asp:DropDownList ID="cmbExport" runat="server" AutoPostBack="True" Font-Size="12px"
                            OnSelectedIndexChanged="cmbExport_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                            <asp:ListItem Value="E">Excel</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table>
                <tr valign="top">
                    <td>
                        <table id="tab1" cellspacing="1" cellpadding="2" style="background-color: #B7CEEC; border: solid 1px  #ffffff;"
                                        border="1">
                            <%--<tr valign="top">
                                <td>
                                    <table cellspacing="1" cellpadding="2" style="background-color: #B7CEEC; border: solid 1px  #ffffff"
                                        border="1">--%>
                                        <tr>
                                            <td class="gridcellleft">
                                                Report View :</td>
                                            <td>
                                                <asp:DropDownList ID="DdlReportView" runat="server" Width="100px" Font-Size="12px"
                                                    onchange="FnReportView(this.value)">
                                                    <asp:ListItem Value="1">Detail</asp:ListItem>
                                                    <asp:ListItem Value="2">Summary</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="Tr_BankDetail">
                                            <td class="gridcellleft">
                                                Bank Name :</td>
                                            <td>
                                                <asp:TextBox ID="txtBankName" runat="server" Width="300px" Font-Size="12px" onkeyup="ajax_showOptions(this,'SearchBankNameFromMainAccountForBrs',event,'Current')"></asp:TextBox>
                                            </td>
                                           
                                        </tr>
                                        <tr id="Tr_BankSummary">
                                            <td class="gridcellleft">
                                                Bank Name :</td>
                                            <td>
                                                <asp:RadioButton ID="rdbBankAll" runat="server" Checked="True" GroupName="d" onclick="fnBank('a')" />
                                                All
                                              <asp:RadioButton ID="rdBankSelected" runat="server" GroupName="d" onclick="fnBank('b')" />Selected
                                            </td>
                                        </tr>
                                    <%--</table>
                                </td>
                            </tr>--%>
                            <tr>
                                <td class="gridcellleft">
                                    As On Date
                                </td>
                                <td>
                                    <dxe:ASPxDateEdit ID="DtAsOn" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                        Font-Size="12px" Width="120px" ClientInstanceName="DtAsOn">
                                        <dropdownbutton text="Date">
                                                </dropdownbutton>
                                    </dxe:ASPxDateEdit>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    Consider :</td>
                                <td>
                                    <asp:DropDownList ID="DdlConsider" runat="server" Width="100px" Font-Size="12px">
                                        <asp:ListItem Value="1">Value Date</asp:ListItem>
                                        <asp:ListItem Value="2">Statement Date</asp:ListItem>
                                    </asp:DropDownList>
                                    </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft" bgcolor="#B7CEEC">
                                    Generate Type :</td>
                                <td>
                                    <asp:DropDownList ID="ddlGeneration" runat="server" Width="100px" Font-Size="12px"
                                        onchange="FnddlGeneration(this.value)">
                                        <asp:ListItem Value="1">Screen</asp:ListItem>
                                        <asp:ListItem Value="2">Export</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td id="td_Screen">
                                    <asp:Button ID="btnScreen" runat="server" CssClass="btnUpdate" Height="20px" Text="Show"
                                        Width="101px" OnClientClick="selecttion()" OnClick="btnScreen_Click" />
                                </td>
                                <td id="td_Export">
                                    <asp:Button ID="btnExcel" runat="server" CssClass="btnUpdate" Height="20px" Text="Export To Excel"
                                        Width="101px" OnClientClick="selecttion()" OnClick="btnExcel_Click" /></td>
                            </tr>
                            <tr>
                             <td style="display: none;" colspan="2">
                                                <asp:TextBox ID="txtBankName_hidden" runat="server" Width="5px"></asp:TextBox>
                                                <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                                                <asp:HiddenField ID="HiddenField_BRSAC" runat="server" />
                                            </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table  cellpadding="1" cellspacing="1" id="showFilter">
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="320px" onkeyup="ajax_showOptions(this,'SearchBankNameFromMainAccountForBrs',event)"></asp:TextBox>
                                    <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                        style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                            style="color: #009900; font-size: 8pt;"> </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ListBox ID="lstSlection" runat="server" Font-Size="12px" Height="120px" Width="340px">
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
            <table>
                <tr>
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
            <div id="displayAll" style="display: none;" width="100%">
                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                    <ContentTemplate>
                        <table width="100%" border="1">
                            <tr style="display: none;">
                                <td>
                                    <asp:HiddenField ID="hiddencount" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="DivHeader" runat="server">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="Divdisplay" runat="server">
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnScreen" EventName="Click"></asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
</asp:Content>
