<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_ExportDematTransactioncommCurrency" Codebehind="ExportDematTransactioncommCurrency.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


 <script language="javascript" type="text/javascript">

  
    function Page_Load()///Call Into Page Load
            {
            
                 FnAccount(document.getElementById('ddlAccount').value);
                 Hide('trSummary');
                 Hide('Tr_Batch');
                 Hide('Tr_ExportBtn');
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
    function FnAccount(obj)
    {
        
        var j=obj.split('~');
       
        if(j[1].substring(0,2)!='IN')
        {
            Show('Tr_CDSLTransactionType');
        }
        else
        {
            Hide('Tr_CDSLTransactionType');
        }
        Hide('trSummary');
        Hide('Tr_Batch');
        Hide('Tr_ExportBtn');
      
    }
    function RecordDisplay(obj)
    {
        if(obj=='1')
        {
           alert('No Record Found !!');
           Hide('trSummary');
           Hide('Tr_Batch');
           Hide('Tr_ExportBtn');
        }
        else
         {
            Show('trSummary');
            Show('Tr_Batch');
            Show('Tr_ExportBtn');
         }
    }
    function DisableTextBox()
    {
        
        var GridView = document.getElementById('<%= gridSummary.ClientID %>') 
        if (GridView.rows.length > 0)
            {
                //loop starts from 1. rows[0] points to the header.
                for (i=1; i<GridView.rows.length; i++)
                {
                    //get the reference of template column
                    cell = GridView.rows[i].cells[0];
                    //loop according to the number of childNodes in the cell
                    for (j=0; j<cell.childNodes.length; j++)
                    {   
                        if (cell.childNodes[j].innerText == '')
                        {
                            if (cell.childNodes[j].innerHTML.indexOf('CHECKED')>=0)
                            {
                                 GridView.rows[i].getElementsByTagName("input")[1].disabled=false;
                            }
                            else
                            {
                                GridView.rows[i].getElementsByTagName("input")[1].disabled=true;
                            }                           
                            
                        }
                        
                    }
                }
               
                
                  
            }
            

    }
   function  FnParam()
   {
       var GridView = document.getElementById('<%= gridSummary.ClientID %>') 
        var param = '';
       if (GridView.rows.length > 0)
            {
                //loop starts from 1. rows[0] points to the header.
                for (i=1; i<GridView.rows.length; i++)
                {
                    //get the reference of template column
                    cell = GridView.rows[i].cells[0];
                    //loop according to the number of childNodes in the cell
                    for (j=0; j<cell.childNodes.length; j++)
                    {   
                        if (cell.childNodes[j].innerText == '')
                        {
                            if (cell.childNodes[j].innerHTML.indexOf('CHECKED')>=0)
                            {
                               /////////For Batch
                                if (GridView.rows[i].cells[1].innerText == ' ')
                                    paramBatch = 'NA';                                
                                else 
                                    paramBatch = GridView.rows[i].cells[1].innerText;
                               
                               /////////For Slip 
                                if (GridView.rows[i].getElementsByTagName("input")[1].value=='')
                                    paramSlip = 'NA';                                
                                else 
                                    paramSlip =GridView.rows[i].getElementsByTagName("input")[1].value;

                                /////////For Transaction
                                if (GridView.rows[i].cells[3].innerText == ' ')
                                    paramTransaction = 'NA';                                
                                else 
                                    paramTransaction = GridView.rows[i].cells[3].innerText;
                                   
                                /////////For Transfer Type  
                                if (GridView.rows[i].cells[4].innerText == ' ')
                                    paramTransfer = 'NA';                                
                                else 
                                    paramTransfer = GridView.rows[i].cells[4].innerText;
                                    
                                  
                                /////////For Old Slip 
                               
                                if (GridView.rows[i].getElementsByTagName("input")[2].value=='')
                                    paramOldSlip = 'NA';                                
                                else 
                                    paramOldSlip = GridView.rows[i].getElementsByTagName("input")[2].value;
                                    
                               
                                param = param + paramBatch + '~' + paramSlip + '~' + paramTransaction + '~' + paramTransfer + '~' + paramOldSlip + ',';
                               
                            }                            
                            
                        }
                        
                    }
                }
               
                
                  
            }
           
            
            if (param == '')
            {
                alert('Invalid. Please select at least one batch to generate.');
                return false;
            }  
            else
            {
                document.getElementById('<%= txtparam.ClientID %>').value=param;
            }
            
   }
   function Download()
    {
        document.getElementById("<%= btnDownload.ClientID %>").click();        
    }
    function BatchExists()
    {
        alert('Invalid. New BatchNumber Already Exists.');
        document.getElementById('<%=txtNextBatch.ClientID %>').focus();
        
    }
    function AlertRecord()
    {
        alert('No Record Found !!');
    }
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <%--   <script language="javascript" type="text/javascript"> 
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
            </script>--%>
    <div>
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" colspan="0" style="text-align: center; height: 20px;">
                    <strong><span id="SpanHeader" style="color: #000099">Export Demat Transactions </span></strong></td>

             
            </tr>
        </table>
        <table id="tab1" border="0" cellpadding="0" cellspacing="0">
            <tr valign="top">
                <td>
                    <table>
                        <tr valign="top">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Select Account :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlAccount" Font-Size="12px" runat="server" Width="250px" onchange="FnAccount(this.value)">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr valign="top">
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Transaction Date :</td>
                                        <td class="gridcellleft">
                                            <dxe:ASPxDateEdit ID="DtTransactionDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="108px" ClientInstanceName="DtTransactionDate">
                                                <dropdownbutton text="For">
                                    </dropdownbutton>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr valign="top">
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Execution Date :</td>
                                        <td class="gridcellleft">
                                            <dxe:ASPxDateEdit ID="DtExecutionDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="108px" ClientInstanceName="DtExecutionDate">
                                                <dropdownbutton text="For">
                                    </dropdownbutton>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="Tr_CDSLTransactionType">
                            <td colspan="3" class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr> <td class="gridcellleft" bgcolor="#B7CEEC">
                                            TransferType Type :</td>
                                        <td>
                                            <asp:RadioButton ID="RdbCDSLTransferType_InterDepository" runat="server" Checked="True" GroupName="a"/>
                                           Inter Depository
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="RdbCDSLTransferType_OffMarket" runat="server" GroupName="a" />Off Market
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr><td class="gridcellleft" colspan="4" >
                                                        <asp:Button ID="BtnScreen" runat="server" CssClass="btnUpdate" Height="20px" Text="Show"
                                                            Width="101px" OnClick="BtnScreen_Click" />
                                                    </td></tr>
                       
                    </table>
                </td>
            </tr>
        </table>
       <%-- <table>
            <tr>
               
                <td id="tdupdatePrgress" style="display:none">
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
        </table>--%>
        <table>
            <tr id="trSummary">
                <td><table border="10" cellpadding="1" cellspacing="1"><tr><td class="gridcellleft" bgcolor="#B7CEEC">
                    Select Batch to Generate :
                </td>
                <td >
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gridSummary" runat="server" AutoGenerateColumns="False" DataKeyNames="CommDeliveryTransactions_BatchNumber"
                                BackColor="PeachPuff" BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px"
                                EmptyDataText="No Record Found." OnRowCommand="gridSummary_RowCommand" >
                                <Columns>
                                    <asp:TemplateField HeaderText="Select Batch To Generate" ControlStyle-Width="10%"
                                        ItemStyle-Width="10%" ItemStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelected" runat="server" onclick="Javascript:DisableTextBox();" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Batch Number" DataField="CommDeliveryTransactions_BatchNumber" />
                                    <asp:TemplateField HeaderText="Slip Number" ControlStyle-Width="70%" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <input type="text" value='<%# Eval("CommDeliveryTransactions_SlipNumber")%>' disabled="disabled" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Transactions Type" DataField="CommDeliveryTransactions_Type" />
                                    <asp:BoundField HeaderText="Transfer Type" DataField="CommDeliveryTransactions_TransferType" />
                                    <asp:BoundField HeaderText="Number of Instructions" DataField="count_batch" />
                                    <asp:BoundField HeaderText="Batch Export DateTime" DataField="CommDeliveryTransactions_BatchExportDate" />
                                    <asp:ButtonField CommandName="Select" Text="Print" ControlStyle-Font-Underline="true"
                                        ControlStyle-ForeColor="blue" />
                                    <asp:TemplateField InsertVisible="false">
                                        <ItemTemplate>
                                            <input type="hidden" name="Hidden" value='<%# Eval("CommDeliveryTransactions_SlipNumber")%>'>
                                            <asp:TextBox ID="txtslip" runat="server" Text='<%# Eval("CommDeliveryTransactions_SlipNumber")%>' Visible="false"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                      <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="BtnScreen" />
                            <asp:AsyncPostBackTrigger ControlID="btnExport" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td></tr></table></td>
            </tr>
            <tr id="Tr_Batch">
                <td>
                    <table border="10" cellpadding="1" cellspacing="1">
                        <tr>
                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                Next Available Batch Number :
                            </td>
                            <td><asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                              <asp:TextBox ID="txtNextBatch" runat="server" MaxLength="7" Width="250px" Font-Size="12px"
                                    BackColor="peachPuff" ForeColor="green" Font-Bold="true"></asp:TextBox>
                                     </ContentTemplate>
                      <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="BtnScreen" />
                            <asp:AsyncPostBackTrigger ControlID="btnExport" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="Tr_ExportBtn">
                <td align="center">
                    <asp:Button ID="btnExport" runat="server" Text="Export"  OnClientClick="return FnParam();" OnClick="btnExport_Click"/>
                </td>
            </tr>
            <tr><td style="display:none;">  <asp:Button ID="btnDownload" runat="server" Text="" OnClick="btnDownload_Click" /><asp:TextBox ID="txtparam" runat="server"></asp:TextBox></td></tr>
        </table>
    </div>
</asp:Content>