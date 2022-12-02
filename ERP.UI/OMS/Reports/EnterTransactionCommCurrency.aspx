<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_EnterTransactionCommCurrency" Codebehind="EnterTransactionCommCurrency.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script language="javascript" type="text/javascript">
    function Page_Load(obj)///Call Into Page Load
            {
                 if(obj=='client')
                    Fn_TransactionType('CI');
                 if(obj=='exchange')
                 {
                    document.getElementById('DDlTranType').value='MI';
                    Fn_TransactionType('MI');
                  } 
                  document.getElementById('DDlTranType').focus(); 
            }
  
    function Hide(obj)
            {
             document.getElementById(obj).style.display='none';
            }
    function Show(obj)
            {
             document.getElementById(obj).style.display='inline';
            }
    function Fn_TransactionType(obj)
    {
        if(obj=='CI')////////Client Incoming
        {
             Show('tr_Client');  
             Hide('tr_Market');
             Show('tr_ClientIncoming');  
             Hide('tr_ClientOutgoing');  
             Hide('tr_MarketIncoming');
             Hide('tr_MarketOutgoing');
        }
        if(obj=='CO')////////Client Outgoing
        {
             Show('tr_Client');  
             Hide('tr_Market');
             Hide('tr_ClientIncoming');  
             Show('tr_ClientOutgoing');  
             Hide('tr_MarketIncoming');
             Hide('tr_MarketOutgoing');
        
        } 
        if(obj=='MI')////////Market Payin
        {
             Hide('tr_Client');  
             Show('tr_Market');
             Hide('tr_ClientIncoming');  
             Hide('tr_ClientOutgoing');  
             Show('tr_MarketIncoming');
             Hide('tr_MarketOutgoing');
        
        } 
        if(obj=='MO')////////Market Payout
        {
             Hide('tr_Client');  
             Show('tr_Market');
             Hide('tr_ClientIncoming');  
             Hide('tr_ClientOutgoing');  
             Hide('tr_MarketIncoming');
             Show('tr_MarketOutgoing');
        
        } 
    }
      function isNumberKey(e)      
        {         
            var keynum
            var keychar
            var numcheck
            if(window.event)//IE
            {
                keynum = e.keyCode 
                if(keynum>=48 && keynum<=57 || keynum==46)
                   {
                      return true;
                   }
                else
                    {
                     alert("Please Insert Numeric Only");
                     return false;
                    }
             } 
         
         else if(e.which) // Netscape/Firefox/Opera
           {
               keynum = e.which  
               if(keynum>=48 && keynum<=57 || keynum==46)
                     {
                      return true;
                     }
                     else
                     {
                     alert("Please Insert Numeric Only");
                     return false;
                     }     
                }
        }
         function dateassign(s)
         {
                var date1 = dtTransaction.GetDate();
                if(date1 != null) 
                  {
                      var date3 =parseInt(date1.getMonth()+1)+'-'+date1.getDate()+'-'+ date1.getFullYear();
                  } 
                var date2 = dtExecution.GetDate();
                if(date2 != null) 
                  {
                      var date4 =parseInt(date2.getMonth()+1)+'-'+date2.getDate()+'-'+ date2.getFullYear();
                  }
                var tranDate = Date.parse(date3);
                var excDate = Date.parse(date4);
                
                if(excDate<tranDate)
                {   
                    alert('Excecution Date Cant Be Less Than Transaction Date !');
                    dtExecution.SetValue(date1);
                }
                
        }
       function Fn_ISIN(objID,objListFun,objEvent)
       {
            if(document.getElementById('txtProduct').value!='')
            {
                 var criterria='ICIN~'+document.getElementById('txtProduct_hidden').value;
                 ajax_showOptions(objID,objListFun,objEvent,criterria);
            }
            else
            {
               alert('Please Select Product!');
            }
       }
       function AlertStatus(obj,objAlert)
       {    
           Page_Load(obj)
           alert(objAlert);
            
       }
       function AlertResult(obj)
       {    
          if(obj!=='0')
          {
            alert('Inserted Successfully !! ');
            parent.editwin.close();
          }
          else
          {
             alert('Insertion Failed !! ');
          }
            
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
     

        
        <table border="10" cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <table>
                        <tr>
                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                Transaction Type :
                            </td>
                            <td>
                                <asp:DropDownList ID="DDlTranType" runat="server" onchange="Fn_TransactionType(this.value)"
                                    Width="184px" Font-Size="11px" TabIndex="0">
                                    <asp:ListItem Value="CI">Client InComing</asp:ListItem>
                                    <asp:ListItem Value="CO">Client OutGoing</asp:ListItem>
                                    <asp:ListItem Value="MI">Market Payin</asp:ListItem> <%---Exchange OutGoing--%>
                                    <asp:ListItem Value="MO">Market Payout</asp:ListItem><%---Exchange InComing---%>
                                     <%-- <asp:ListItem Value="IS">Inter Settlement</asp:ListItem>
                                    <asp:ListItem Value="ICT">Inter-Account Transfer</asp:ListItem>--%>
                                </asp:DropDownList>
                            </td>
                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                Transaction Date :
                            </td>
                            <td>
                                <dxe:ASPxDateEdit ID="dtTransaction" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Font-Size="12px" Width="108px" ClientInstanceName="dtTransaction" TabIndex="0">
                                    <DropDownButton Text="For">
                                    </DropDownButton>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                Execution Date :
                            </td>
                            <td>
                                <dxe:ASPxDateEdit ID="dtExecution" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Font-Size="12px" Width="108px" ClientInstanceName="dtExecution" TabIndex="0">
                                    <DropDownButton Text="For">
                                    </DropDownButton>
                                    <ClientSideEvents valuechanged="function(s, e) {dateassign(s.GetValue());}" />
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table border="10" cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <table style="border: solid 1px blue">
                        <tr id="tr_Client">
                            <td>
                                <table>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Client :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtClientName" TabIndex="0" runat="server" Font-Size="12px" Width="250px" onkeyup="ajax_showOptions(this,'ShowClientFORMarginStocks',event,'Clients')"></asp:TextBox>
                                            <asp:TextBox ID="txtClientName_hidden" runat="server" Width="14px" Style="display: none;"> </asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="tr_Market">
                            <td>
                                <table>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Market :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtExchange" TabIndex="0" runat="server" Font-Size="12px" Width="250px" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Product :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProduct" TabIndex="0" runat="server" Font-Size="12px" Width="155px"
                                                onkeyup="ajax_showOptions(this,'ShowClientFORMarginStocks',event,'ScripCriteria')"></asp:TextBox>
                                                <asp:TextBox ID="txtProduct_hidden" runat="server" Width="14px" Style="display: none;"> </asp:TextBox>
                                        </td>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            ICIN :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtICIN" runat="server" TabIndex="0" Font-Size="12px" Width="155px" onkeyup="Fn_ISIN(this,'ShowClientFORMarginStocks',event)"></asp:TextBox>
                                        </td>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Delivery Mode :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddldeliverymode" TabIndex="0" runat="server" Width="100px" Font-Size="11px">
                                                <asp:ListItem Value="D">Demat</asp:ListItem>
                                                <%---<asp:ListItem Value="P">Physical</asp:ListItem>--%>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="tr_ClientIncoming">
                            <td>
                                <table>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Source Account :
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList ID="ddlClientSourceIncoming" TabIndex="0" runat="server" Font-Size="12px" Width="250px" OnSelectedIndexChanged="ddlClientSourceIncoming_SelectedIndexChanged"  AutoPostBack="true">
                                                            <%---<asp:ListItem Value="MH">Margin/Holdback A/C</asp:ListItem>--%>
                                                            <asp:ListItem Value="C">Client A/C</asp:ListItem>
                                                            <%---<asp:ListItem Value="Own">Own A/C</asp:ListItem>--%>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlSAccount" TabIndex="0" runat="server" Font-Size="12px" Width="300px">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlClientSourceIncoming" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Target Account :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlClientTargetIncoming" TabIndex="0" runat="server" Font-Size="12px" Width="250px">
                                            </asp:DropDownList></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="tr_ClientOutgoing">
                            <td>
                                <table>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Source Account :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlClientSourceOutgoing" TabIndex="0" runat="server" Font-Size="12px" Width="250px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Target Account :
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList ID="ddlClientTargetOutgoing" TabIndex="0" runat="server" Font-Size="12px" Width="250px"  AutoPostBack="true" OnSelectedIndexChanged="ddlClientTargetOutgoing_SelectedIndexChanged">
                                                            <%---<asp:ListItem Value="MH">Margin/Holdback A/C</asp:ListItem>--%>
                                                            <asp:ListItem Value="C">Client A/C</asp:ListItem>
                                                            <%---<asp:ListItem Value="Own">Own A/C</asp:ListItem>--%>
                                                        </asp:DropDownList></td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlTAccount" TabIndex="0" runat="server" Font-Size="12px" Width="300px">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlClientTargetOutgoing" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="tr_MarketIncoming">
                            <td>
                                <table>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Source Account :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlMarketIncoming" TabIndex="0" runat="server" Font-Size="12px"
                                                Width="250px">
                                            </asp:DropDownList></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="tr_MarketOutgoing">
                            <td>
                                <table>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Target Account :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlMarketOutgoing" TabIndex="0" runat="server" Font-Size="12px"
                                                Width="250px">
                                            </asp:DropDownList></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Settlement Number :</td>
                                        <td id="TrSett2">
                                            <asp:TextBox ID="txtSettNumber" TabIndex="0" runat="server" Font-Size="12px" Width="155px" onkeyup="InterSettlementFunc(this,'InterSettlementForDeliveryPosition',event,'SettSource')"></asp:TextBox>
                                            
                                        </td>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Slip Number :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSlipNumber" TabIndex="0" runat="server" Font-Size="12px" Width="155px"></asp:TextBox>
                                        </td>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Quantity :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtQuantity" TabIndex="0" runat="server" Onkeypress="return isNumberKey(event)" Font-Size="12px" Width="155px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Remarks :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRemarks" runat="server" TabIndex="0" Font-Size="12px" Width="700px" Height="90px"
                                                TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td >
                                            <asp:Button ID="btnInsert" TabIndex="0" runat="server" Text="Save" CssClass="btnUpdate" Height="25px"
                                                Width="94px" OnClick="btnInsert_Click" /></td>
                                        
                                    </tr>
                                </table>
                            </td>
                        </tr>
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
                </td>
            </tr>
        </table>
    </div>
  </asp:Content>