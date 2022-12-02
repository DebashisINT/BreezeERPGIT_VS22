<%@ Page title="JournalVoucher_ExcelImport.aspx" Language="C#" AutoEventWireup="true"  MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.DailyTask.JournalVoucher_ExcelImport" Codebehind="JournalVoucher_ExcelImport.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxpc" %>

<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxp" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajaxList.js"></script>

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
		position:absolute ;
		z-index:5;
	}
	
	form{
		display:inline;
	}
</style>

    <script language="javascript" type="text/javascript">
    //ProtoType
            String.prototype.trim = function() {
	            return this.replace(/^\s+|\s+$/g,"");
            }
            String.prototype.ltrim = function() {
	            return this.replace(/^\s+/,"");
            }
            String.prototype.rtrim = function() {
	            return this.replace(/\s+$/,"");
            }
        //
     //Global Variable
        FieldName = 'txtVoucherNo';
     //
    function SignOff()
    {
        window.parent.SignOff();
    }
    function PageLoad()
    {
        document.getElementById("txtProfTaxS").style.display="none";
        document.getElementById("txtTdsS").style.display="none";
    }
    function height()
    {
        if(document.body.scrollHeight>=350)
            window.frameElement.height = document.body.scrollHeight;
        else
            window.frameElement.height = '350px';
        window.frameElement.Width = document.body.scrollWidth;
    }
    /////////////////Ajax List Methods//////////////////////////
    function keyVal(obj)
    {
       //alert(obj);
       if(obj=="No Record Found") return;
       var LedgerType=obj.split("~")[1];
       var MethodCalledBy=obj.split("~")[4];
       var MainAcOrSubAc=obj.split("~")[2];
       if(MainAcOrSubAc=="MAINAC" && LedgerType!="NONE")
       {
            if(MethodCalledBy=="PROFTAX")
            {
              document.getElementById("txtProfTaxS").value="";
            }
            if(MethodCalledBy=="TDS")
            {
              document.getElementById("txtTdsS").value="";
            }
       }
       if(LedgerType!="NONE")
       {
            if(MethodCalledBy=="PROFTAX")
            {
              document.getElementById("txtProfTaxS").style.display="inline";
              document.getElementById("HtmBProfTax").style.display="none";
            }
            if(MethodCalledBy=="TDS")
            {
              document.getElementById("txtTdsS").style.display="inline";
              document.getElementById("HtmBTDS").style.display="none";
            }
       }
       else
       {
            if(MethodCalledBy=="PROFTAX")
            {
              document.getElementById("txtProfTaxS").style.display="none";
              document.getElementById("HtmBProfTax").style.display="inline";
            }
            if(MethodCalledBy=="TDS")
            {
              document.getElementById("txtTdsS").style.display="none";
              document.getElementById("HtmBTDS").style.display="inline";
            }
       }
       
    }
    function replaceChars(entry) {
            out = "+"; // replace this
            add = "--"; // with this
            temp = "" + entry; // temporary holder

            while (temp.indexOf(out)>-1) {
            pos= temp.indexOf(out);
            temp = "" + (temp.substring(0, pos) + add + 
            temp.substring((pos + out.length), temp.length));
            }
            return temp;
    }
    function CallMainAccount(obj1,obj2,obj3,obj4,obj5)
    {
       var SubLedgerType;
       if(obj4=="E")
        SubLedgerType="'Employees'";
       if(obj4=="ECN")
        SubLedgerType="'Employees','None','Custom'";
       var strQuery_Table = "Master_MainAccount";
       var strQuery_FieldName = " top 10 MainAccount_Name+\' [ \'+rtrim(ltrim(MainAccount_AccountCode))+\' ]\' as MainAccount_Name1,cast(MainAccount_ReferenceID as varchar)+\'~\'+MainAccount_SubLedgerType+\'~MAINAC~\'+MainAccount_AccountType+\'~\'+\'"+obj5+"\' as MainAccount_ReferenceID";
       var strQuery_WhereClause = " (MainAccount_Name like (\'%RequestLetter%\') or MainAccount_AccountCode like (\'%RequestLetter%\')) and isnull(MainAccount_BankCashType,'abc') not in('Cash','Bank') and MainAccount_SubLedgerType in (" + SubLedgerType + ")";
       var strQuery_OrderBy='';
       var strQuery_GroupBy='';
       var CombinedQuery=new String(strQuery_Table+"$"+strQuery_FieldName+"$"+strQuery_WhereClause+"$"+strQuery_OrderBy+"$"+strQuery_GroupBy);
       ajax_showOptions(obj1,obj2,obj3,replaceChars(CombinedQuery),'Main'); 
    }
    function CallSubAccount(obj1,obj2,obj3,obj4)
    {
       var WhichMainAccount=obj4;
       var MainAccountCode;  
       
       if(WhichMainAccount=="ProfTax") MainAccountCode=document.getElementById("txtProfTaxM_hidden").value;
       if(WhichMainAccount=="TDS") MainAccountCode=document.getElementById("txtTdsM_hidden").value;
       
       var SegID=document.getElementById("hdn_SegID_SegmentName").value.split('~')[0];
       var SegmentName=document.getElementById("hdn_SegID_SegmentName").value.split('~')[1];
       var ProcedureName = "SubAccountSelect_New";
       var InputName = "CashBank_MainAccountID#clause#branch#exchSegment#SegmentN";
       var InputType = "V#V#V#V#V";
       var InputValue = MainAccountCode.split('~')[0]+"#RequestLetter#"+'<%=Session["userbranchHierarchy"] %>'+"#'"+'<%=Session["ExchangeSegmentID"] %>'+"'#'"+SegmentName+"'";
       var SplitChar="#";
       var CombinedQuery=ProcedureName+"$"+InputName+"$"+InputType+"$"+InputValue+"$"+SplitChar;
       ajax_showOptions(obj1,obj2,obj3,CombinedQuery,'Main');
    }
    function ImportFileButtonClick()
    {
       //Validate Page
           
            if(document.getElementById("txtSalaryAcc_hidden").value=="" || document.getElementById("txtSalaryAcc").value.trim()=="")
            {
                alert("Please Choose Salary Account !");
                document.getElementById("txtSalaryAcc").focus();
                return;
            }
            if(document.getElementById("txtSalaryPayable_hidden").value=="" || document.getElementById("txtSalaryPayable").value.trim()=="")
            {
                alert("Please Choose Salary Payable !");
                document.getElementById("txtSalaryPayable").focus();
                return;
            }
            if(document.getElementById("txtLoanNAdvance_hidden").value=="" || document.getElementById("txtLoanNAdvance").value.trim()=="")
            {
                alert("Please Choose Loan and Advance !");
                document.getElementById("txtLoanNAdvance").focus();
                return;
            }
            if(document.getElementById("txtProfTaxM_hidden").value=="" || document.getElementById("txtProfTaxM").value.trim()=="")
            {
                alert("Please Choose Prof. Tax !");
                document.getElementById("txtProfTaxM").focus();
                return;
            }
            if(document.getElementById("txtProfTaxM_hidden").value.split("~")[1]!="NONE")
            {
                if(document.getElementById("txtProfTaxS_hidden").value=="" || document.getElementById("txtProfTaxS").value.trim()=="")
                {
                    alert("Please Choose Prof. Tax SubAccount !");
                    document.getElementById("txtProfTaxS").focus();
                    return;
                }
            }
            if(document.getElementById("txtTdsM_hidden").value=="" || document.getElementById("txtTdsM").value.trim()=="")
            {
                alert("Please Choose TDS !");
                document.getElementById("txtTdsM").focus();
                return;
            }
            if(document.getElementById("txtTdsM_hidden").value.split("~")[1]!="NONE")
            {
                if(document.getElementById("txtTdsS_hidden").value=="" || document.getElementById("txtTdsS").value.trim()=="")
                {
                    alert("Please Choose TDS SubAccount !");
                    document.getElementById("txtTdsS").focus();
                    return;
                }
            }
        ///////////////
        var  file = document.getElementById('FU_ImportN').value;
        cCbpImportFile.PerformCallback(file);
        height();
    }
    function OnComboSelectedIndexChange()
    {
        
    }
    function DateChange()
    {
        var SelectedDate = new Date(tDate.GetDate());
        var monthnumber = SelectedDate.getMonth();
        var monthday    = SelectedDate.getDate();
        var year        = SelectedDate.getYear();
        
        var SelectedDateValue=new Date(year, monthnumber, monthday);
        ///Checking of Transaction Date For MaxLockDate
        var MaxLockDate=new Date('<%=Session["LCKJV"]%>');
        monthnumber = MaxLockDate.getMonth();
        monthday    = MaxLockDate.getDate();
        year        = MaxLockDate.getYear();
        var MaxLockDateNumeric=new Date(year, monthnumber, monthday).getTime();
//                alert('TransactionDate='+TransactionDate+'\n'+'MaxLockDate= '+MaxLockDate);
        //alert(ValueDate+'~'+ValueDateNumeric+'~'+VisibleIndexE);
        if(SelectedDateValue<=MaxLockDateNumeric)
        {
            alert('This Entry Date has been Locked.');
            MaxLockDate.setDate(MaxLockDate.getDate() + 1);
            tDate.SetDate(MaxLockDate);
            return;
        }
    
    
        var FYS ="<%=Session["FinYearStart"]%>";
        var FYE ="<%=Session["FinYearEnd"]%>"; 
        var LFY ="<%=Session["LastFinYear"]%>";
        var SelectedDate = new Date(tDate.GetDate());
        var FinYearStartDate = new Date(FYS);
        var FinYearEndDate = new Date(FYE);
        var LastFinYearDate=new Date(LFY);
        
        var monthnumber = SelectedDate.getMonth();
        var monthday    = SelectedDate.getDate();
        var year        = SelectedDate.getYear();
        
        var SelectedDateValue=new Date(year, monthnumber, monthday);
        
        monthnumber = FinYearStartDate.getMonth();
        monthday    = FinYearStartDate.getDate();
        year        = FinYearStartDate.getYear();
        var FinYearStartDateValue=new Date(year, monthnumber, monthday);
        
      
        monthnumber = FinYearEndDate.getMonth();
        monthday    = FinYearEndDate.getDate();
        year        = FinYearEndDate.getYear();
        var FinYearEndDateValue=new Date(year, monthnumber, monthday);
        
    //                alert('SelectedDateValue :'+SelectedDateValue.getTime()+
    //                '\nFinYearStartDateValue :'+FinYearStartDateValue.getTime()+
    //                '\nFinYearEndDateValue :'+FinYearEndDateValue.getTime());
        
        var SelectedDateNumericValue=SelectedDateValue.getTime();
        var FinYearStartDateNumericValue=FinYearStartDateValue.getTime();
        var FinYearEndDatNumbericValue=FinYearEndDateValue.getTime();
        if(SelectedDateNumericValue>=FinYearStartDateNumericValue && SelectedDateNumericValue<=FinYearEndDatNumbericValue)
        {
    //                   alert('Between');
        }
        else
        {
           alert('Enter Date Is Outside Of Financial Year !!');
           if(SelectedDateNumericValue<FinYearStartDateNumericValue)
           {
                tDate.SetDate(new Date(FinYearStartDate));
           }
           if(SelectedDateNumericValue>FinYearEndDatNumbericValue)
           {
                tDate.SetDate(new Date(FinYearEndDate));
           }
        }

                
    } 
    function CbpImportFile_EndCallBack()
    {
        if(cCbpImportFile.cpImportStatus!=undefined)
        {
            alert(cCbpImportFile.cpImportStatus);
            window.location.reload();
        }
    }  

    </script>
    <style type="text/css">
        .tb2tbl td {
            padding:3px 10px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="panel-heading">
        <div class="panel-title">
            <h3>IMPORT SALARY JVS FROM PAYROLL FILE</h3>
        </div>

    </div>
        <div class="form_main">
            <table border="1" class="tb2tbl" style="border-color:#ccc;">
                
                <tr>
                    <td style="width: 134px; font-weight: bold;  text-align: left;
                        text-decoration: underline;">
                        Select Account For :</td>
                    <td style="width: 33%">
                    </td>
                    <td style="width: 40%">
                    </td>
                    <td style="width: 40%">
                    </td>
                </tr>
                <tr>
                    <td style="width: 18%">
                    </td>
                    <td style="font-weight: bold; vertical-align: top; height: 11px; 
                        text-align: left;">
                        Main Accounts</td>
                    <td style="font-weight: bold; vertical-align: top; height: 11px; 
                        text-align: left; width: 40%;">
                        Sub Accounts</td>
                    <td style="font-weight: bold; vertical-align: top; height: 11px; 
                        text-align: left;">
                        Amount Column</td>
                </tr>
                <tr>
                    <td style="font-weight: bold; vertical-align: top; height: 11px; 
                        text-align: left;">
                        Salary Account</td>
                    <td style="width: 33%">
                        <asp:TextBox ID="txtSalaryAcc" runat="server" Width="95%" onkeyup="CallMainAccount(this,'GenericAjaxList',event,'E','SalaryAccount')"
                            onFocus="this.select()"></asp:TextBox>
                        <asp:HiddenField ID="txtSalaryAcc_hidden" runat="server" />
                    </td>
                    <td style="width: 40%">
                        XX</td>
                    <td style="width: 40%">
                        <dxe:aspxcombobox id="ComboSalaryAccount" runat="server" clientinstancename="cComboSalaryAccount"
                            Font-Size="12px" SelectedIndex="0" ValueType="System.String" EnableIncrementalFiltering="True"
                            Width="65px">
                            <clientsideevents selectedindexchanged="OnComboSelectedIndexChange" />
                        </dxe:aspxcombobox>
                    </td>
                </tr>
                <tr>
                    <td style="font-weight: bold; vertical-align: top; height: 11px; 
                        text-align: left;">
                        Salary Payable</td>
                    <td style="width: 33%">
                        <asp:TextBox ID="txtSalaryPayable" runat="server" Width="95%" onkeyup="CallMainAccount(this,'GenericAjaxList',event,'E','SalaryPayable')"
                            onFocus="this.select()"></asp:TextBox>
                        <asp:HiddenField ID="txtSalaryPayable_hidden" runat="server" />
                    </td>
                    <td style="width: 40%">
                        XX</td>
                    <td style="width: 40%">
                        <dxe:aspxcombobox id="ComboSalaryPayable" runat="server" clientinstancename="cComboSalaryPayable"
                            Font-Size="12px" SelectedIndex="0" ValueType="System.String" EnableIncrementalFiltering="True"
                            Width="65px">
                            <clientsideevents selectedindexchanged="OnComboSelectedIndexChange" />
                        </dxe:aspxcombobox></td>
                </tr>
                <tr>
                    <td style="font-weight: bold; vertical-align: top; height: 11px; 
                        text-align: left;">
                        Loan &amp; Advance</td>
                    <td style="width: 33%">
                        <asp:TextBox ID="txtLoanNAdvance" runat="server" Width="95%" onkeyup="CallMainAccount(this,'GenericAjaxList',event,'E','LoanAdvance')"
                            onFocus="this.select()"></asp:TextBox>
                        <asp:HiddenField ID="txtLoanNAdvance_hidden" runat="server" />
                    </td>
                    <td style="width: 40%">
                        XX</td>
                    <td style="width: 40%">
                        <dxe:aspxcombobox id="ComboLoanNAdvance" runat="server" clientinstancename="cComboLoanNAdvance"
                            Font-Size="12px" SelectedIndex="0" ValueType="System.String" EnableIncrementalFiltering="True"
                            Width="65px">
                            <clientsideevents selectedindexchanged="OnComboSelectedIndexChange" />
                        </dxe:aspxcombobox></td>
                </tr>
                <tr>
                    <td style="font-weight: bold; vertical-align: top; height: 11px; 
                        text-align: left;">
                        Proff. Tax</td>
                    <td style="width: 33%">
                        <asp:TextBox ID="txtProfTaxM" runat="server" Width="95%" onkeyup="CallMainAccount(this,'GenericAjaxList',event,'ECN','ProfTax')"
                            onFocus="this.select()"></asp:TextBox>
                        <asp:HiddenField ID="txtProfTaxM_hidden" runat="server" />
                    </td>
                    <td style="width: 40%">
                        <asp:TextBox ID="txtProfTaxS" runat="server" Width="95%" onkeyup="CallSubAccount(this,'GenericAjaxListSP',event,'ProfTax')"></asp:TextBox>
                        <b style="text-align: right" id="HtmBProfTax">XX</b>
                        <asp:HiddenField ID="txtProfTaxS_hidden" runat="server" />
                    </td>
                    <td style="width: 40%">
                        <dxe:aspxcombobox id="ComboProfTaxM" runat="server" clientinstancename="cComboProfTaxM"
                            Font-Size="12px" SelectedIndex="0" ValueType="System.String" EnableIncrementalFiltering="True"
                            Width="65px">
                            <clientsideevents selectedindexchanged="OnComboSelectedIndexChange" />
                        </dxe:aspxcombobox></td>
                </tr>
                <tr>
                    <td style="font-weight: bold; vertical-align: top; height: 11px; 
                        text-align: left;">
                        TDS</td>
                    <td style="width: 33%">
                        <asp:TextBox ID="txtTdsM" runat="server" Width="95%" onkeyup="CallMainAccount(this,'GenericAjaxList',event,'ECN','TDS')"
                            onFocus="this.select()"></asp:TextBox>
                        <asp:HiddenField ID="txtTdsM_hidden" runat="server" />
                    </td>
                    <td style="width: 40%">
                        <asp:TextBox ID="txtTdsS" runat="server" Width="95%" onkeyup="CallSubAccount(this,'GenericAjaxListSP',event,'TDS')"></asp:TextBox>
                        <b style="text-align: right" id="HtmBTDS">XX</b>
                        <asp:HiddenField ID="txtTdsS_hidden" runat="server" />
                    </td>
                    <td style="width: 40%">
                        <dxe:aspxcombobox id="ComboTds" runat="server" clientinstancename="cComboTds" Font-Size="12px"
                            SelectedIndex="0" ValueType="System.String" EnableIncrementalFiltering="True"
                            Width="65px">
                            <clientsideevents selectedindexchanged="OnComboSelectedIndexChange" />
                        </dxe:aspxcombobox></td>
                </tr>
                <tr>
                    <td style="font-weight: bold; vertical-align: top; height: 11px;  text-align: left;">
                        Transaction Date</td>
                    <td style="width: 33%">
                        <dxe:ASPxDateEdit ID="tDate" runat="server" EditFormat="Custom" ClientInstanceName="tDate"
                            UseMaskBehavior="True" Font-Size="13px" Width="40%" EditFormatString="dd-MM-yyyy">
                            <clientsideevents datechanged="function(s,e){DateChange()}" />
                        </dxe:ASPxDateEdit>
                    </td>
                    <td style="width: 40%">
                    </td>
                    <td style="width: 326%">
                    </td>
                </tr>
                <tr>
                    <td style="font-weight: bold; vertical-align: top; height: 11px;  text-align: left;">
                        Comman Narration</td>
                    <td style="width: 33%">
                        <asp:TextBox ID="txtCommonNarration" runat="server" TextMode="MultiLine" Width="95%"></asp:TextBox></td>
                    <td style="width: 40%">
                    </td>
                    <td style="width: 326%">
                    </td>
                </tr>
                <tr>
                    <td style="width: 18%">
                    </td>
                    <td style="width: 33%">
                    </td>
                    <td style="width: 40%">
                    </td>
                    <td style="width: 326%">
                    </td>
                </tr>
                <tr>
                    <td style="font-weight: bold; vertical-align: top; height: 11px; 
                        text-align: left;">
                        File Path</td>
                    <td style="width: 33%">
                         <input id="FU_Import" name="FU_ImportN" type="file" style="width:95%" /></td>
                    <td style="width: 40%">
                        <dxe:ASPxButton ID="btnImportFile" CssClass="btn btn-primary" runat="server" AccessKey="X" AutoPostBack="false"
                            TabIndex="0" Text="Import File" UseSubmitBehavior="False">
                            <clientsideevents click="function(s, e) {ImportFileButtonClick();}"></clientsideevents>
                        </dxe:ASPxButton>
                    </td>
                    <td style="width: 326%">
                        <dxe:ASPxCallbackPanel ID="CbpImportFile" runat="server" ClientInstanceName="cCbpImportFile"
                            BackColor="White" OnCallback="CbpImportFile_Callback">
                            <PanelCollection>
                                <dxe:panelcontent runat="server">
                                </dxe:panelcontent>
                            </PanelCollection>
                            <ClientSideEvents EndCallback="function(s, e) {
	                                                    CbpImportFile_EndCallBack(); }" />
                        </dxe:ASPxCallbackPanel>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdn_CurrentSegment" runat="server" />
            <asp:HiddenField ID="hdn_SegID_SegmentName" runat="server" />
        </div>
  </asp:Content>