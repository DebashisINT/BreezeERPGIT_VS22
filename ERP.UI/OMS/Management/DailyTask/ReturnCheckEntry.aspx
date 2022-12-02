<%@ Page Title="Return Check Entry" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.DailyTask.management_DailyTask_ReturnCheckEntry" Codebehind="ReturnCheckEntry.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <%-- --%>
    <script type="text/javascript" src="../../CentralData/JSScript/GenericJScript.js"></script>
    <script type="text/javascript" src="/assests/js/init.js"></script>
    <script src="../../JSFUNCTION/choosen.min.js"></script>
    <script type="text/javascript" src="/assests/js/ajaxList.js"></script>
    
   <%--  <script type="text/javascript" src="/assests/js/GenericJscript.js"></script>--%>
   
         <link type="text/css" href="../../CSS/AjaxStyle.css" rel="Stylesheet" />

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
		z-index:100;
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
		z-index:5;
	}
	
	form{
		display:inline;
	}
	</style>

    <script language="javascript" type="text/javascript">
        function Page_Load()
        {
            DateChange(cTransactionDt);
            document.getElementById('TrEntry').style.display='none';
           
        }
        function ShowBankName(obj1,obj2,obj3)
        {
           ajax_showOptions(obj1,obj2,obj3);
        }

        function ListBind() {
            debugger;
            var config = {
                '.chsn': {},
                '.chsn-deselect': { allow_single_deselect: true },
                '.chsn-no-single': { disable_search_threshold: 10 },
                '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
                '.chsn-width': { width: "100%" }
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }

        }
        function height()
        {
           //if(document.body.scrollHeight>=500)
           //   window.frameElement.height = document.body.scrollHeight;
           //else
           //   window.frameElement.height = '500px';
           //   window.frameElement.Width = document.body.scrollWidth;
        }
        function Show()
        {
            document.getElementById('TrReturnCheque').style.display='inline';
            document.getElementById('TrEntry').style.display='inline';
        }
        function alertMessage()
        {
            alert('Return Entry Generated Successfully !!');
            document.getElementById('TrEntry').style.display='none';
            document.getElementById('TrReturnCheque').style.display='none';
        }
        FieldName='btnEntry';    
    </script>
     <script language="javascript" type="text/javascript">
    

         $(document).ready(function () {
             debugger;
             ListBind();
             $('#txtBankName').trigger("chosen:updated");
             var cntry = document.getElementById('txtBankName_hidden').value;
             document.getElementById('txtBankName_hidden').value = "";
             //SetBankNames(cntry);
         });
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
     


         function IsProductExpired(CompareToDate, SessionExpireDate) {
             //alert(SessionExpireDate+'~'+CompareToDate.GetText());
             ///Date Should Between Current Fin Year StartDate and EndDate
             var ExpireDate = new Date(SessionExpireDate);
             var CompareDate = new Date(CompareToDate.GetText());

             monthnumber = ExpireDate.getMonth();
             monthday = ExpireDate.getDate();
             year = ExpireDate.getYear();
             var ExpireDateValue = new Date(year, monthnumber, monthday);


             monthnumber = CompareDate.getMonth();
             monthday = CompareDate.getDate();
             year = CompareDate.getYear();
             var CompareDateValue = new Date(year, monthnumber, monthday);

             //alert('ExpireDateValue :'+ExpireDateValue.getTime()+' CompareDateValue :'+CompareDateValue.getTime());

             var ExpireDateNumeric = ExpireDateValue.getTime();
             var CompareDateNumeric = CompareDateValue.getTime();

             if (ExpireDateNumeric <= CompareDateNumeric) {
                 alert('License Expired.Please Renew Your License for No Further Interruption!!!.Sorry For InConvenience.');
                 CompareToDate.Focus();
                 return 'T';
             }
             else
                 return 'F';

         }
         function DateChange(DateObj) {
             var FYS = '<%=Session["FinYearStart"]%>';
                        var FYE = '<%=Session["FinYearEnd"]%>';
                        var LFY = '<%=Session["LastFinYear"]%>';
                        DevE_CheckForFinYear(DateObj, FYS, FYE, LFY);
                    }

                    function CallBankAccount(obj1, obj2, obj3) {
                        var CurrentSegment = document.getElementById('hdn_CurrentSegment').value;
                        var strPutSegment = " and (MainAccount_ExchangeSegment=" + CurrentSegment + " or MainAccount_ExchangeSegment=0)";
                        var strQuery_Table = "(Select MainAccount_AccountCode+\'-\'+MainAccount_Name+\' [ \'+MainAccount_BankAcNumber+\' ]\'+\' ~ \'+MainAccount_BankCashType as IntegrateMainAccount,MainAccount_AccountCode+\'~\'+MainAccount_BankCashType+\'~CASHBANK\' as MainAccount_AccountCode,MainAccount_Name,MainAccount_BankAcNumber from Master_MainAccount where (MainAccount_BankCashType=\'Bank\' or MainAccount_BankCashType=\'Cash\') and (MainAccount_BankCompany=\'" + '<%=Session["LastCompany"] %>' + "\' Or IsNull(MainAccount_BankCompany,'')='')" + strPutSegment + ") as t1";
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
<%--<body style="margin: 0px 0px 0px 0px; background-color: #DDECFE">--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>          
           
            <table class="TableMain100">
                <tr>
                    <td align="">
                        <table>
                            <tr>
                                <td style="text-align: right; width: 120px;">
                                    Purpose :
                                </td>
                                <td style="text-align: left;">
                                    <asp:DropDownList ID="ddlPurpose" Font-Size="11px" runat="server" Width="327px">
                                        <asp:ListItem Value="I">Issued By Us</asp:ListItem>
                                        <asp:ListItem Value="R">Received By Us</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 120px;">
                                    Bank Name:
                                </td>
                                <td style="text-align: left;">
                                    <%--<asp:TextBox ID="txtBankName" Font-Size="11px" runat="server" Width="327px"
                                    onkeyup="CallBankAccount(this,'GenericAjaxList',event)"></asp:TextBox>--%>
                                    <asp:ListBox ID="txtBankName" CssClass="chsn"   runat="server" Width="100%"   
                                        data-placeholder="Select..." ></asp:ListBox>
                                    <asp:HiddenField ID="txtBankName_hidden" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 120px;">
                                    Cheque Number:
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtChequeNumber" Font-Size="11px" runat="server" Width="327px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 120px;">
                                    Amount:
                                </td>
                                <td style="text-align: left;">
                                    <dxe:ASPxTextBox ID="txtAmount" runat="server" HorizontalAlign="Right" Font-Size="11px"
                                        Width="325px">
                                        <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                        <ValidationSettings ErrorDisplayMode="None">
                                        </ValidationSettings>
                                    </dxe:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 120px;">
                                    Transaction Date:
                                </td>
                                <td style="text-align: left;">
                                    <dxe:ASPxDateEdit ID="dtDate" runat="server" ClientInstanceName="cTransactionDt" EditFormat="Custom" UseMaskBehavior="True"
                                        Font-Size="12px" Width="327px">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                         <ClientSideEvents DateChanged="function(s,e){DateChange(cTransactionDt);}"/>
                                    </dxe:ASPxDateEdit>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 120px;">
                                    Narration:
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtNarration" runat="server" Height="54px" Font-Size="12px" TextMode="MultiLine" Width="327px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td style="text-align: left">
                                    <asp:Button ID="btnShow" runat="server" Text="Show Original Entry" CssClass="btn btn-primary" OnClick="btnShow_Click" />
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
                                                                        <img src='../../../assests/images/progress.gif' width='18' height='18'></td>
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
                                <td></td>
                            </tr>
                            <tr id="TrReturnCheque">
                                <td colspan="2">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="grdReturnCheque" runat="server" CssClass="gridcellleft" CellPadding="4"
                                                ForeColor="#333333" GridLines="None" BorderWidth="1px" BorderColor="#507CD1"
                                                Width="100%" AutoGenerateColumns="False">
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                                                <EditRowStyle BackColor="#E59930" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <HeaderStyle Font-Bold="False" ForeColor="Black" CssClass="EHEADER" BorderColor="AliceBlue"
                                                    BorderWidth="1px" />
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Bank Name">
                                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBankName" runat="server" Text='<%# Eval("BankName")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Account Name">
                                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMainSub" runat="server" Text='<%# Eval("MainSub")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cheque Number">
                                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblChequeNumber" runat="server" Text='<%# Eval("ChequeNumber")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tr. Date">
                                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTransactionDate" runat="server" Text='<%# Eval("TransactionDate")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Payment Amount">
                                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPayAmount" runat="server" Text='<%# Eval("PayAmount")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnEntry" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr id="TrEntry">
                                <td>
                                </td>
                                <td style="text-align: left">
                                    <asp:Button ID="btnEntry" runat="server" Text="Generate Return Entry" CssClass="btn btn-primary" OnClick="btnEntry_Click1" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="hdn_CurrentSegment" runat="server" />
</asp:Content>
<%--</body>--%>

