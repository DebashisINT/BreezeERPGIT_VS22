<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" 
    Inherits="ERP.OMS.Management.management_DailyTask_frm_BankDepositSlip" Codebehind="frm_BankDepositSlip.aspx.cs" %>


<%--<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe000001" %>


<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%----%>

    <script type="text/javascript" src="/assests/js/init.js"></script>

<%--    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>--%>

    <script type="text/javascript" src="/assests/js/ajaxList_wofolder.js"></script>

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
        FieldName = 'btnGenerate';
        function keyVal(obj) {
            if (obj != '') {

                if (e2.GetDate() < e1.GetDate()) {
                    alert('Execution Date cannot be earlier than Transaction Date.');
                    e2.SetDate(e1.GetDate());
                }
                document.getElementById('<%=Button1.ClientID %>').click();
      trSummary.style.display = 'inline';
  }

}

//function SignOff() {
//    window.parent.SignOff();
//}
//function height() {

//    if (document.body.scrollHeight >= 600)
//        window.frameElement.height = document.body.scrollHeight + 600;
//    else
//        window.frameElement.height = '600px';
//    window.frameElement.Width = document.body.scrollWidth + 100;
//}
function PageLoad() {
    trSummary.style.display = 'none';
    MainFilter.style.display = 'inline';
    txtSelect.style.display = 'none';

}
function ShowBankName(obj1, obj2, obj3) {
    ajax_showOptions(obj1, obj2, obj3, 'k', 'Main');
}
function OnDateChanged() {
    if (e2.GetDate() < e1.GetDate()) {
        alert('Execution Date cannot be earlier than Transaction Date.');
        e2.SetDate(e1.GetDate());
    }
    document.getElementById('<%=Button1.ClientID %>').click();
          trSummary.style.display = 'inline';
      }
      function OnExecDateChanged() {
          if (e2.GetDate() < e1.GetDate()) {
              alert('Invalid. Execution Date cannot be earlier than Transaction Date.');
              e2.SetDate(e1.GetDate());
          }

      }
      function CheckAccid() {
          trSummary.style.display = 'inline';
          return true;

      }
      function RegenBatch(obj) {
          var selRow = eval(obj.id.substr(15, (obj.id.substr(15).indexOf('_'))));
          var grid = document.getElementById("<%= gridSummary.ClientID %>");
     var transaction = grid.rows[selRow - 1].cells[1].innerText;
     var transfer = grid.rows[selRow - 1].cells[2].innerText;
 }

 function GenereteSlip(obj) {
     var selRow = eval(obj.id.substr(15, (obj.id.substr(15).indexOf('_'))));
     var grid = document.getElementById("<%= GridSelect.ClientID %>");
     var transaction = grid.rows[selRow - 1].cells[1].innerText;
     var transfer = grid.rows[selRow - 1].cells[2].innerText;
 }

 function ForFilterOff() {
     //     document.getElementById('spanfltr').style.display='inline';  
     //     document.getElementById('MainFilter').style.display="none";
     //     document.getElementById('spanBtn').style.display='none';
     //     document.getElementById('spanShow').style.display='none';
     trSummary.style.display = 'none';
     MainFilter.style.display = 'none';
     txtSelect.style.display = 'inline';
     spanfltr.style.display = 'inline';
     SpanPrint.style.display = 'inline';

     height();
 }
 function ShowHide() {

     trSummary.style.display = 'none';
     MainFilter.style.display = 'none';
     txtSelect.style.display = 'inline';
     spanfltr.style.display = 'inline';
     SpanPrint.style.display = 'inline';
 }
 function ForFilterOn() {
     trSummary.style.display = 'inline';
     MainFilter.style.display = 'inline';
     txtSelect.style.display = 'none';
     spanfltr.style.display = 'none';
     SpanPrint.style.display = 'none';
     spanBtn.style.display = 'inline';

 }

 function CallBankAccount(obj1, obj2, obj3) {
     var CurrentSegment = '<%=Session["usersegid"]%>'
         var strPutSegment = " and (MainAccount_ExchangeSegment=" + CurrentSegment + " or MainAccount_ExchangeSegment=0)";
         var strQuery_Table = "(Select MainAccount_AccountCode+\'-\'+MainAccount_Name+\' [ \'+MainAccount_BankAcNumber+\' ]\' as IntegrateMainAccount,MainAccount_AccountCode as MainAccount_AccountCode,MainAccount_Name,MainAccount_BankAcNumber from Master_MainAccount where (MainAccount_BankCashType=\'Bank\' or MainAccount_BankCashType=\'Cash\') and (MainAccount_BankCompany=\'" + '<%=Session["LastCompany"] %>' + "\' Or IsNull(MainAccount_BankCompany,'')='')" + strPutSegment + ") as t1";
       var strQuery_FieldName = " Top 10 * ";
       var strQuery_WhereClause = "MainAccount_AccountCode like (\'%RequestLetter%\') or MainAccount_Name like (\'%RequestLetter%\') or MainAccount_BankAcNumber like (\'%RequestLetter%\')";
       var strQuery_OrderBy = '';
       var strQuery_GroupBy = '';
       var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
       ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');
   }
   function replaceChars(entry) {
       out = "+"; // replace this
       add = "--"; // with this
       temp = "" + entry; // temporary holder

       while (temp.indexOf(out) > -1) {
           pos = temp.indexOf(out);
           temp = "" + (temp.substring(0, pos) + add +
           temp.substring((pos + out.length), temp.length));
       }
       return temp;

   }


    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Bank Deposit Slips</h3>
        </div>

    </div>
    <div class="form_main">
            <table class="TableMain100">
               <%-- <tr>
                    <td class="EHEADER" style="text-align: center;">
                        <strong><span style="color: #000099">Bank Deposit Slips</span></strong>
                    </td>
                </tr>--%>
                <tr id="MainFilter" runat="server">
                    <td align="Center">
                        <table width="500px">
                            <tr>
                                <td class="gridcellleft">
                                    Select Bank
                                </td>
                                <td class="gridcellleft">
                                    <asp:TextBox ID="txtBankName" runat="server" Font-Size="12px" Width="250px" TabIndex="1"
                                    onkeyup ="CallBankAccount(this,'GenericAjaxList',event)"></asp:TextBox>
                                    <asp:HiddenField ID="txtBankName_hidden" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    Transaction Date
                                </td>
                                <td class="gridcellleft">
                                    <dxe:ASPxDateEdit ID="txtTranDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                        TabIndex="2" Width="250px" ClientInstanceName="e1">
                                        <buttonstyle width="13px">
                                                    </buttonstyle>
                                        <clientsideevents valuechanged="function(s, e) {OnDateChanged();}" />
                                    </dxe:ASPxDateEdit>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    Deposit Date
                                </td>
                                <td class="gridcellleft">
                                    <dxe:ASPxDateEdit ID="txtDipDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                        TabIndex="3" Width="250px" ClientInstanceName="e2">
                                        <buttonstyle width="13px">
                                                    </buttonstyle>
                                        <clientsideevents valuechanged="function(s, e) {OnExecDateChanged();}" />
                                    </dxe:ASPxDateEdit>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    Format
                                </td>
                                <td class="gridcellleft">
                                    <asp:DropDownList ID="ddlChequeType" runat="server" Width="250px" AutoPostBack="false"
                                        TabIndex="4">
                                        <asp:ListItem Value="HC">HDFC</asp:ListItem>
                                        <asp:ListItem Value="UTI">AXIS</asp:ListItem>
                                        <asp:ListItem Value="IC">ICICI</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="trSummary">
                                <td>
                                    Select Transaction To Print :
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="gridSummary" runat="server" AutoGenerateColumns="False" DataKeyNames="PrintDate"
                                                BackColor="PeachPuff" BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px"
                                                EmptyDataText="No Record Found." OnRowDataBound="gridSummary_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select" ControlStyle-Width="10%" ItemStyle-Width="10%"
                                                        ItemStyle-HorizontalAlign="center">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chb1" runat="server" onclick="RegenBatch(this);" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Transaction">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCount" class="Ecoheadtxt" runat="server" Text='<%# Eval("TotalTran") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle ForeColor="Blue" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Print DateTime">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDate" class="Ecoheadtxt" runat="server" Text='<%# Eval("PrintDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle ForeColor="Blue" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="Button1" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td class="gridcellleft">
                                    <span id="spanShow">
                                        <asp:Button ID="btnGenerate" Text="Show" runat="server" CssClass="btn btn-primary" OnClick="btnGenerate_Click"
                                            TabIndex="6" />
                                    </span><span id="spanBtn" style="display: none"><a href="#" style="font-weight: bold;
                                        color: Blue" onclick="javascript:ForFilterOff();">Cancel</a></span>
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td colspan="2">
                                    <asp:Button ID="Button1" runat="server" Text="" OnClientClick="return CheckAccid();"
                                        OnClick="Button1_Click" />
                                    <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <table width="100%">
                            <tr>
                              <td align="left">     <span id="SpanPrint" style="display: none">
                    <asp:Button ID="btnShow" Text="Print" runat="server" CssClass="btnUpdate" OnClick="btnShow_Click"
                        TabIndex="6" /></span>
                                </td>
                                <td align="right"> <span id="spanfltr" style="display: none"><a href="#" style="font-weight: bold; color: Blue"
                onclick="javascript:ForFilterOn();">Filter</a></span> 
                                </td>
                              
                            </tr>
                        </table>
       </td> </tr>
            <tr id="txtSelect">
                <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="GridSelect" runat="server" AutoGenerateColumns="False" DataKeyNames="SrNo"
                                BackColor="PeachPuff" BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px"
                                EmptyDataText="No Record Found." OnRowDataBound="GridSelect_RowDataBound" Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="Select" ControlStyle-Width="10%" ItemStyle-Width="10%"
                                        ItemStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chb2" runat="server" onclick="GenereteSlip(this);" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cheque No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInstrument" class="Ecoheadtxt" runat="server" Text='<%# Eval("CASHBANKDETAIL_INSTRUMENTNUMBER") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle ForeColor="Blue" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cheque Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPrintDate" class="Ecoheadtxt" runat="server" Text='<%# Eval("CASHBANKDETAIL_INSTRUMENTDATE") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle ForeColor="Blue" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bank & Branch Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBankName" class="Ecoheadtxt" runat="server" Text='<%# Eval("BankName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle ForeColor="Blue" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmonut" class="Ecoheadtxt" runat="server" Text='<%# Eval("CASHBANKDETAIL_RECEIPTAMOUNT") %>'></asp:Label>
                                        </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"
 />
                                        <HeaderStyle ForeColor="Blue" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="A/C Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCode" class="Ecoheadtxt" runat="server" Text='<%# Eval("CNT_UCC") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle ForeColor="Blue" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Account Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" class="Ecoheadtxt" runat="server" Text='<%# Eval("CilentName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle ForeColor="Blue" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Account Name" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPrintDatetime" class="Ecoheadtxt" runat="server" Text='<%# Eval("CashBankDetail_SlipPrintDateTime") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle ForeColor="Blue" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CashBankDetailid" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCashBankID" class="Ecoheadtxt" runat="server" Text='<%# Eval("CashBankDetail_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle ForeColor="Blue" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="Button1" />                            
                        </Triggers>
                    </asp:UpdatePanel>
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
 </asp:Content>