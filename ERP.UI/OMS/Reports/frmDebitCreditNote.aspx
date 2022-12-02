<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_frmDebitCreditNote" Codebehind="frmDebitCreditNote.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="../CSS/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>

    
     <script type="text/javascript" src="/assests/js/GenericJScript.js"></script>

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
    <%--<script type="text/javascript" src="/assests/js/loaddata1.js"></script>--%>
      <script language="javascript" type="text/javascript">
      function height()
        {
            if(document.body.scrollHeight>=300)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '300px';
            window.frameElement.Width = document.body.scrollWidth;
        }
        
          function Show(obj)
        {
            document.getElementById(obj).style.display='inline';
        }
        function Hide(obj)
        {
            document.getElementById(obj).style.display='none';
        }
        function Page_Load()
        {
            Hide('TdSpecific');
            height();
        }
         function OnAllCheckedChanged(s, e) {
            if (s.GetChecked())
                cgridJournalVouchar.SelectRows();
            else
                cgridJournalVouchar.UnselectRows();
        }
        function ForSpecific(obj)
        {
            if(obj=='A')
                Hide('TdSpecific');
            else
                Show('TdSpecific');
        }
        
        function DateChange(DateObj)
{
    //var Lck ='<%=Session["LCKBNK"] %>';
    var FYS ='<%=Session["FinYearStart"]%>';
    var FYE ='<%=Session["FinYearEnd"]%>'; 
    var LFY ='<%=Session["LastFinYear"]%>';
    //DevE_CheckForLockDate(DateObj,Lck);
    DevE_CheckForFinYear(DateObj,FYS,FYE,LFY);
//    DevE_CompareDateAndAddDay(DateObj,cDatepayinedit,2)
//    DevE_CompareDateAndAddDay(DateObj,cDatepayoutedit,2)
//    DevE_CompareDateAndAddDay(DateObj,cDatedelpayinedit,2)
//    DevE_CompareDateAndAddDay(DateObj,cDatedelpayoutedit,2)
//    DevE_CompareDateAndAddDay(DateObj,cDateconfedit,2)
//    DevE_CompareDateAndAddDay(DateObj,cDateendedit,0)
}
      
      function DateCompare(DateobjFrm,DateobjTo)
{
    var Msg="To Date Can Not Be Less Than From Date!!!";
    DevE_CompareDateForMin(DateobjFrm,DateobjTo,Msg);
}
 function updateEditorText() 
         {
            var code=txtAccountCode.GetText().toUpperCase();
            if(code=='X' || code=='Y' || code=='Z' || code=='V' || code=='U' || code=='T')
            {
                alert('{T,U,V,W,X,Y,Z} are Reserved Key');
                txtAccountCode.SetText('JV');
            }
         }
      </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>
            <table class="TableMain100">
             <tr>
                    <td class="EHEADER" colspan="2" style="text-align: center;">
                        <strong><span style="color: #000099">Debit / Credit Notes</span></strong>
                    </td>
                </tr>
                <tr>
                    <td class="gridcellleft" style="width: 77px">
                        Date :
                    </td>
                    <td>
                        <table>
                        
                            <tr>
                                <td style="width: 196px">
                         
                                    <dxe:ASPxDateEdit ID="dtDate" runat="server" EditFormat="Custom" ClientInstanceName="dtDate"
                                        UseMaskBehavior="True">
                                        <DropDownButton Text="From ">
                                        </DropDownButton>
                                        
                                        <clientsideevents datechanged="function(s,e){DateChange(dtDate);}"></clientsideevents>
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td>
                                    <dxe:ASPxDateEdit ID="dtToDate" runat="server" EditFormat="Custom" ClientInstanceName="dtToDate"
                                        UseMaskBehavior="True">
                                        <DropDownButton Text="To">
                                        </DropDownButton>
                                        <clientsideevents datechanged="function(s,e){DateCompare(dtDate,dtToDate);}"></clientsideevents>
                                    </dxe:ASPxDateEdit>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="gridcellleft" style="width: 77px">
                        Voucher Type : 
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="radAll" Checked="true" runat="server" GroupName="a" onclick="ForSpecific('A')" />
                                </td>
                                <td>
                                    All
                                </td>
                                <td>
                                    <asp:RadioButton ID="radSpecific" runat="server" GroupName="a" onclick="ForSpecific('B')" />
                                </td>
                                <td>
                                    Specific
                                </td>
                                <td id="TdSpecific">
                                    <dxe:ASPxTextBox ID="txtAccountCode" ClientInstanceName="txtAccountCode"
                                        runat="server" Width="50px" MaxLength="2">
                                        <ValidationSettings>
                                            <RequiredField IsRequired="True" ErrorText="Select Account Code" />
                                        </ValidationSettings>
                                        <%--<ClientSideEvents KeyPress="function(s,e){window.setTimeout('updateEditorText()', 10);}" />--%>
                                    </dxe:ASPxTextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                 <tr>
                    <td class="gridcellleft" style="width: 89px">
                        Print Option :
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td id="td_user">
                                    <asp:CheckBox ID="user" runat="server"   />
                                </td>
                                <td>
                                    Print Entered By User
                                </td>
                                <td id="Td1">
                                    <asp:CheckBox ID="time" runat="server"  />
                                </td>
                                <td>
                                    Print Entry Date Time
                                </td>
                                
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="BtnSave" runat="server" Text="Show Notes" CssClass="btn" OnClick="BtnSave_Click"
                         Height="26px" Width="105px" />
                            
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                          <dxe:ASPxGridView ID="gridJournalVouchar" runat="server" width="100%" AutoGenerateColumns="False"
                            ClientInstanceName="cgridJournalVouchar" Font-Size="12px"  KeyFieldName="JournalVoucherDetail_ID"
                            >
                          
                          
                            <Columns>
                            
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="50px" VisibleIndex="0">
                                                <HeaderTemplate>
                                                    <dxe:ASPxCheckBox ID="cbAll" runat="server" ClientInstanceName="cbAll" ToolTip="Select all rows"
                                                        BackColor="White"  >
                                                         <ClientSideEvents CheckedChanged="OnAllCheckedChanged" />
                                                    </dxe:ASPxCheckBox>
                                                 <%--<dxe:ASPxCheckBox ID="cbPage" runat="server" ClientInstanceName="cbPage" ToolTip="Select all rows within the page"
                                                        OnInit="cbPage_Init">
                                                        <ClientSideEvents CheckedChanged="OnPageCheckedChanged" />
                                                    </dxe:ASPxCheckBox>--%>
                                                </HeaderTemplate>
                                            </dxe:GridViewCommandColumn>
                                            
                                             <dxe:GridViewDataDateColumn VisibleIndex="1" PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" FieldName="JournalVoucherDetail_TransactionDate"   
                                    Caption="Voucher Date" >
                                   
                                </dxe:GridViewDataDateColumn>
                            
                             <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="JournalVoucherDetail_VoucherNumber"
                                    Caption="Voucher No.">
                                  
                                </dxe:GridViewDataTextColumn>
                               
                                
                                 
                                <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="JournalVoucher_Narration"
                                     Caption="Main Narration">
                                   
                                </dxe:GridViewDataTextColumn>
                              
                                <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="JournalVoucherDetail_Narration" 
                                    Caption="Sub Narration">
                                  
                                </dxe:GridViewDataTextColumn>
                               <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="JournalVoucherDetail_AmountDr" PropertiesTextEdit-DisplayFormatString="0.00"
                                    Caption="Debit Note">
                                   
                                </dxe:GridViewDataTextColumn>
                                
                                 <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="JournalVoucherDetail_AmountCr" PropertiesTextEdit-DisplayFormatString="0.00"
                                    Caption="Credit Note">
                                   
                                </dxe:GridViewDataTextColumn>
                                                            
                            </Columns>
                            <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
                            <SettingsPager NumericButtonCount="30" ShowSeparators="True" Mode="ShowAllRecords"
                                PageSize="20">
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </SettingsPager>
                            
                            <Styles>
                                <FocusedRow BackColor="#FFC080" Font-Bold="False">
                                </FocusedRow>
                                <Header BackColor="ControlLight" Font-Bold="True" ForeColor="Black" HorizontalAlign="Center">
                                </Header>
                            </Styles>
                        </dxe:ASPxGridView>
                    </td>
                </tr>
                <tr>
                   <td colspan="2" id="td_print" runat="server">
                         <asp:Button ID="btnPrint" runat="server" Text="Print Notes" CssClass="btn" Height="26px"
                            Width="105px" OnClick="btnPrint_Click" />
                         </td>   
                </tr>
            </table>
        </div>
</asp:Content>
