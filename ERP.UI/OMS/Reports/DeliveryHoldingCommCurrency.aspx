<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_DeliveryHoldingCommCurrency" Codebehind="DeliveryHoldingCommCurrency.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    	<script language="javascript" type="text/javascript">
    function Page_Load()///Call Into Page Load
            {
                 Hide('showFilter');
                 Hide('tr_filter');
                 Fn_ddlGeneration('1');
                 document.getElementById('hiddencount').value=0;
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
    function Fn_ddlGeneration(obj)
    {
        if(obj=='1')/////Screen
        {
            Show('td_Screen');
            Hide('td_Export');
        }
        if(obj=='2')/////Export
        {
            Hide('td_Screen');
            Show('td_Export');
            
       }
       document.getElementById('btndpac').click();
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
           
            
            function Fn_Display()
            {
                Show('tr_filter');
                Hide('tab1');
                Show('displayAll');
                document.getElementById('hiddencount').value=0;
                height();
            } 
           function NORECORD(obj)
            {
                Hide('tr_filter');
                Show('tab1');
                Hide('displayAll');
                Hide('showFilter');
                if(obj=='1')
                    alert('No Record Found');
                document.getElementById('hiddencount').value=0;
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
         FieldName='lstSlection';
            </script>

    <script type="text/ecmascript">   
       function ReceiveServerData(rValue)
        {
               
                var j=rValue.split('~');
               
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
                        <strong><span id="SpanHeader" style="color: #000099">Holding Report  </span></strong>
                    </td>
                    <td class="EHEADER" width="15%" id="tr_filter" style="height: 22px">
                        <a href="javascript:void(0);" onclick="NORECORD(6);"><span style="color: Blue; text-decoration: underline;
                            font-size: 8pt; font-weight: bold">Filter</span></a>
                              <asp:DropDownList ID="cmbExport" runat="server" AutoPostBack="True" Font-Size="12px" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                        <asp:ListItem Value="E">Excel</asp:ListItem>
                    </asp:DropDownList>
                    </td>
                </tr>
            </table>
          <table id="tab1"><tr valign="top"><td>  <table>
                <tr>
                    <td class="gridcellleft">
                        <table border="10" cellpadding="1" cellspacing="1">
                            <tr>
                                <td class="gridcellleft" bgcolor="#B7CEEC">
                                    Generate Type :</td>
                                <td>
                                    <asp:DropDownList ID="ddlGeneration" runat="server" Width="130px" Font-Size="12px"
                                        onchange="Fn_ddlGeneration(this.value)">
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
                                <td class="gridcellleft" bgcolor="#B7CEEC">
                                    Delivery Mode:</td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdDeliveryModeBoth" runat="server"  GroupName="a" Enabled="false"/>
                                                Both
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdDeliveryModeDemat" runat="server" GroupName="a" Checked="True" />Demat
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdDeliveryModePhysical" runat="server" GroupName="a" Enabled="false"/>Physical
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
                      <table border="10" cellpadding="1" cellspacing="1">
                          <tr>
                              <td class="gridcellleft" bgcolor="#B7CEEC" >
                              DP A/C :
                              </td>
                              <td>
                                  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                      <ContentTemplate>
                                          <asp:DropDownList ID="ddlDPAc" TabIndex="0" runat="server" Font-Size="12px" Width="300px">
                                          </asp:DropDownList>
                                      </ContentTemplate>
                                      <Triggers>
                                         
                                           <asp:AsyncPostBackTrigger ControlID="btndpac" EventName="Click"></asp:AsyncPostBackTrigger>
                                      </Triggers>
                                  </asp:UpdatePanel>
                              </td>
                          </tr>
                      </table>
                  </td>
              </tr>
                <tr>
                    <td class="gridcellleft">
                        <table border="10" cellpadding="1" cellspacing="1">
                            <tr valign="top">
                                <td class="gridcellleft" bgcolor="#B7CEEC">
                                    As On Date:
                                </td>
                               
                                <td class="gridcellleft">
                                    <dxe:ASPxDateEdit ID="dtasondate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                        Font-Size="12px" Width="150px" ClientInstanceName="dtto">
                                        <DropDownButton Text="As On Date">
                                        </DropDownButton>
                                         
                                    </dxe:ASPxDateEdit>
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
                                  <asp:RadioButton ID="rdbProductAll" runat="server" Checked="True" GroupName="b" onclick="Fn_Product('a')" />
                              </td>
                              <td>
                                  All
                              </td>
                              <td>
                                  <asp:RadioButton ID="rdbProductSelected" runat="server" GroupName="b" onclick="Fn_Product('b')" />
                              </td>
                              <td>
                                  Selected
                              </td>
                          </tr>
                      </table>
                  </td>
              </tr>
           
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td id="td_Screen">
                                    <asp:Button ID="btnScreen" runat="server" CssClass="btnUpdate" Height="20px" Text="Screen"
                                        Width="101px" OnClientClick="selecttion()" OnClick="btnScreen_Click" /></td>
                                <td id="td_Export">
                                    <asp:Button ID="btnExport" runat="server" CssClass="btnUpdate" Height="20px" Text="Export To Excel"
                                        Width="101px" OnClientClick="selecttion()" OnClick="btnExport_Click" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
          </table>
          </td>
              <td >
                  <table cellpadding="0" cellspacing="0" id="showFilter">
                      <tr>
                          <td>
                              <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox>
                              <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                  Enabled="false">
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
                   
                    <asp:HiddenField ID="HiddenField_ScripCriteria" runat="server" />
                    <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                <asp:Button ID="btndpac" runat="server" CssClass="btnUpdate" Height="20px" Text="btndpac"
                    Width="101px" OnClick="btndpac_Click" /></td>
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
        </div>
            <div id="displayAll" style="display: none;">
            <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                <ContentTemplate>
                    <table width="100%" border="1">
                        <tr style="display: none;">
                            <td>
                              <asp:HiddenField ID="hiddencount" runat="server" />
                            </td>
                        </tr>
                        <tr id="tr_DIVdisplayPERIOD">
                            <td>
                                <div id="DIVdisplayPERIOD" runat="server">
                                </div>
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
                    <asp:AsyncPostBackTrigger ControlID="btnScreen" EventName="Click"></asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
        </div>
   </asp:Content>

