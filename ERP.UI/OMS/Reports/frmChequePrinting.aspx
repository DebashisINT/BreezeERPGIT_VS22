<%@ Page Title="Cheque Printing" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmChequePrinting" enableEventValidation="false"  Codebehind="frmChequePrinting.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />
    <style type="text/css">
	   .tableClass {
    /* border: 0px; */
    border: 1px solid #aaa !important;
    border-collapse: collapse !important;
}
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
	    
	    
	    
	    function callback()
	    {   
	       var btn = document.getElementById('Button1');
           btn.click(); 
	    }
	    function SelectSingle() 
            {
           // alert('Hiiiiiii');
                var gridview = document.getElementById('gridCheque'); 
                var rCount = gridview.rows.length; 
                var SumAmt=0;
                var SumChq=0;
                var chqNo1;
                var j=0;
                var chequeID=0;
                var chequenumber=0;
                var bank=0;
                
                for (i=2;i<rCount;i++) 
                {
                    var leni;                    
                    var a=new String(i)
                    j=i-1;
                    var b=new String(j)
                    if(a.length==1)
                        leni="0"+i;        
                    else
                        leni=i;
                    if(b.length==1)
                        j="0"+j;
                     var obj='gridCheque'+'_ctl'+leni+'_'+'ChkDelivery';
                     var chqNo='gridCheque'+'_ctl'+leni+'_'+'txtChequeNumber';
                     var chqid='gridCheque'+'_ctl'+leni+'_'+'lblCashBankDetailID';
                     var bnkid='gridCheque'+'_ctl'+leni+'_'+'ddlClientAccount';
                     if(i!=2)
                         chqNo1='gridCheque'+'_ctl'+j+'_'+'txtChequeNumber';
                     if(document.getElementById(obj).checked==true)
                     {
                       
                        if(document.getElementById(chqid).value!='')
                        {
                            if(chequeID==0)
                            {
                                chequeID=document.getElementById(chqid).innerText;
                                bank=document.getElementById(bnkid).value;
                            }
                            else
                            {
                                chequeID=chequeID+','+document.getElementById(chqid).innerText;
                                bank=bank+','+document.getElementById(bnkid).value;
                            }
                        }
                        SumAmt = parseFloat(SumAmt);
                       	if(i!=2)
                            {
                                if(document.getElementById(chqNo1).value=='')
                                {
                                    if(SumChq!=0)
                                    {
                                        SumChq = 1+parseFloat(SumChq); 
                                        var Chq=new String(SumChq)                                       
                                        if(Chq.length==1)                     
                                            SumChq="00000"+SumChq;   
                                        else if(Chq.length==2)                     
                                            SumChq="0000"+SumChq;    
                                        else if(Chq.length==3)                     
                                            SumChq="000"+SumChq;      
                                        else if(Chq.length==4)                     
                                            SumChq="00"+SumChq;        
                                        else if(Chq.length==5)                     
                                            SumChq="0"+SumChq;     
                                        document.getElementById(chqNo).innerText=SumChq;
                                        chequenumber=SumChq; 
                                           
                                     } 
                                     
                                }
                                else  
                                {  
                                    SumChq = 1+parseFloat(document.getElementById(chqNo1).value);
                                    var Chq=new String(SumChq)                                       
                                    if(Chq.length==1)                     
                                         SumChq="00000"+SumChq;   
                                    else if(Chq.length==2)                     
                                         SumChq="0000"+SumChq;    
                                    else if(Chq.length==3)                     
                                         SumChq="000"+SumChq;      
                                    else if(Chq.length==4)                     
                                         SumChq="00"+SumChq;        
                                    else if(Chq.length==5)                     
                                         SumChq="0"+SumChq;    
                                    document.getElementById(chqNo).innerText=SumChq; 
                                    chequenumber=chequenumber+','+SumChq;    
   
                                }
                                
                                  
                            }
                            else
                            {
                                SumChq = document.getElementById(chqNo).value; 
                        
                                chequenumber=SumChq;
                           }
                     }               
                }
                document.getElementById('hdnID').value=chequeID;
                document.getElementById('hdnChequeNumber').value=chequenumber;
                document.getElementById('hdnbank').value=document.getElementById('ddlBank').value;
                document.getElementById('hdnstrBank').value=bank;
                  
          
            }
            
            
             function SelectSingle1() 
            {
            //alert('byyy');
                var gridview = document.getElementById('gridother'); 
                var rCount = gridview.rows.length; 
                var SumAmt=0;
                var SumChq=0;
                var chqNo1;
                var j=0;
                var chequeID=0;
                var chequenumber=0;
                var bank=0;
                
                for (i=2;i<rCount;i++) 
                {
                    var leni;                    
                    var a=new String(i)
                    j=i-1;
                    var b=new String(j)
                    if(a.length==1)
                        leni="0"+i;        
                    else
                        leni=i;
                    if(b.length==1)
                        j="0"+j;
                     var obj='gridother'+'_ctl'+leni+'_'+'ChkDelivery1';
                     var chqNo='gridother'+'_ctl'+leni+'_'+'txtChequeNumber1';
                     var chqid='gridother'+'_ctl'+leni+'_'+'lblCashBankDetailID1';
                     //var bnkid='gridCheque'+'_ctl'+leni+'_'+'ddlClientAccount';
                     if(i!=2)
                         chqNo1='gridother'+'_ctl'+j+'_'+'txtChequeNumber1';
                     if(document.getElementById(obj).checked==true)
                     {
                       
                        if(document.getElementById(chqid).value!='')
                        {
                            if(chequeID==0)
                            {
                                chequeID=document.getElementById(chqid).innerText;
                                //bank=document.getElementById(bnkid).value;
                            }
                            else
                            {
                                chequeID=chequeID+','+document.getElementById(chqid).innerText;
                                //bank=bank+','+document.getElementById(bnkid).value;
                            }
                        }
                        SumAmt = parseFloat(SumAmt);
                       	if(i!=2)
                            {
                                if(document.getElementById(chqNo1).value=='')
                                {
                                    if(SumChq!=0)
                                    {
                                        SumChq = 1+parseFloat(SumChq); 
                                        var Chq=new String(SumChq)                                       
                                        if(Chq.length==1)                     
                                            SumChq="00000"+SumChq;   
                                        else if(Chq.length==2)                     
                                            SumChq="0000"+SumChq;    
                                        else if(Chq.length==3)                     
                                            SumChq="000"+SumChq;      
                                        else if(Chq.length==4)                     
                                            SumChq="00"+SumChq;        
                                        else if(Chq.length==5)                     
                                            SumChq="0"+SumChq;     
                                        document.getElementById(chqNo).innerText=SumChq;
                                        chequenumber=SumChq; 
                                           
                                     } 
                                     
                                }
                                else  
                                {  
                                    SumChq = 1+parseFloat(document.getElementById(chqNo1).value);
                                    var Chq=new String(SumChq)                                       
                                    if(Chq.length==1)                     
                                         SumChq="00000"+SumChq;   
                                    else if(Chq.length==2)                     
                                         SumChq="0000"+SumChq;    
                                    else if(Chq.length==3)                     
                                         SumChq="000"+SumChq;      
                                    else if(Chq.length==4)                     
                                         SumChq="00"+SumChq;        
                                    else if(Chq.length==5)                     
                                         SumChq="0"+SumChq;    
                                    document.getElementById(chqNo).innerText=SumChq; 
                                    chequenumber=chequenumber+','+SumChq;    
   
                                }
                                
                                  
                            }
                            else
                            {
                                SumChq = document.getElementById(chqNo).value; 
                        
                                chequenumber=SumChq;
                           }
                     }               
                }
                document.getElementById('hdnID1').value=chequeID;
                document.getElementById('hdnChequeNumber1').value=chequenumber;
                document.getElementById('hdnbank').value=document.getElementById('ddlBank').value;
                //document.getElementById('hdnstrBank1').value=bank;
                  
          
            }


        function HideOn()
        {
            document.getElementById('btnCrystalPrint').style.display='inline';
            document.getElementById('btnUpdateCheque').style.display='none';                
        }
        
        function OnMoreInfoClick()
        {
        

            var ID=document.getElementById('hdnID').value;
            var bank=document.getElementById('hdnbank').value;
            var FromDate=document.getElementById('hdnFromdate').value;
            var ToDate=document.getElementById('hdnTodate').value;
            var strBank=document.getElementById('hdnstrBank').value;
            var chequenumber=document.getElementById('hdnChequeNumber').value;
            var BankType=document.getElementById('hdnBankType').value;
            var chkUcc=document.getElementById('hdnUcc').value;
            var chkAccount=document.getElementById('hdnAccount').value;
           
            if(chkAccount=='Yes')
            {
                chkAccount="true";
            }
            else
            {
                chkAccount="false";
            }
            
            if(chkUcc=='Yes')
            {
                chkUcc="true";
            }
            else
            {
                chkUcc="false";
            }
           
            
            var url="ChequePrintingintermediate.aspx?ID="+ID+"&bank="+bank+"&Fromdate="+FromDate+"&Todate="+ToDate+"&strBank="+strBank+"&ChequeNumber="+chequenumber+"&BankType="+BankType+"&Account="+chkAccount+"&Ucc="+chkUcc;
           // alert(url);
            OnMoreInfoClick(url,"Cheque Printing",'940px','450px',"Y");
            
       
        }
          function OnMoreInfoClick1()
        {
        
//alert('ani');
            var ID1=document.getElementById('hdnID1').value;
            var bank=document.getElementById('hdnbank').value;
            var FromDate=document.getElementById('hdnFromdate').value;
            var ToDate=document.getElementById('hdnTodate').value;
           // var strBank1=document.getElementById('hdnstrBank1').value;
            //alert (strBank1);
            var chequenumber1=document.getElementById('hdnChequeNumber1').value;
            var BankType=document.getElementById('hdnBankType').value;
            var chkUcc=document.getElementById('hdnUcc').value;
            var chkAccount=document.getElementById('hdnAccount').value;
            
            var url="ChequePrintingintermediateother.aspx?ID="+ID1+"&bank="+bank+"&Fromdate="+FromDate+"&Todate="+ToDate+"&ChequeNumber="+chequenumber1+"&BankType="+BankType+"&Account="+chkAccount+"&Ucc="+chkUcc;
//alert(url);
            OnMoreInfoClick(url,"Cheque Printing",'940px','450px',"Y");
            
    }
	    function FunSettNumber(objID,objListFun,objEvent)
        {
            ajax_showOptions(objID,objListFun,objEvent,'SettType','Sub');
        }
        function FunForSettNum(objID,objListFun,objEvent)
        {
            ajax_showOptions(objID,objListFun,objEvent);
        }
        function FunForSettType(objID,objListFun,objEvent)
        {            
            ajax_showOptions(objID,objListFun,objEvent,document.getElementById('txtSettNumberHoldBack').value);
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
        function PageLoad()
        {
            
            document.getElementById('btnCrystalPrint').style.display='none';
            document.getElementById('btnUpdateCheque').style.display='none';            
            height();

        }
        function hideprint()
        {
            
            document.getElementById('btnCrystalPrint').style.display='none';
           

        }
        function CalcKeyCode(aChar) 
        {
          var character = aChar.substring(0,1);
          var code = aChar.charCodeAt(0);
          return code;
        }
        function checkNumber(val) 
        {          
          var strPass = val.value;
          var strLength = strPass.length;
          var lchar = val.value.charAt((strLength) - 1);
          var cCode = CalcKeyCode(lchar);


          if (cCode < 48 || cCode > 57 ) {
            var myNumber = val.value.substring(0, (strLength) - 1);
            val.value = myNumber;
          }
          return false;
        }
      
        function SelectAllInterSegment(id)
        {
            var frm = document.forms[0];
            for (i=0;i<frm.elements.length;i++) 
            {
                if (frm.elements[i].type == "checkbox") 
                { 
                    frm.elements[i].checked = document.getElementById(id).checked;
                }
            }
            SelectSingle();
        } 
 function SelectAllInterSegment1(id)
        {
        alert(id);
            var frm = document.forms[0];
            for (i=0;i<frm.elements.length;i++) 
            {
                if (frm.elements[i].type == "checkbox") 
                { 
                    frm.elements[i].checked = document.getElementById(id).checked;
                }
            }
            SelectSingle1();
        } 
        FieldName='lstSuscriptions';
        document.body.style.cursor = 'pointer'; 
        var oldColor = '';
        function showdiv()
        {
           document.getElementById('Div1').style.display='inline';
        }
        function hidediv()
        {
           document.getElementById('Div1').style.display='none';
        }        
    </script>

    <script type="text/ecmascript">
    </script>

    <link href="/aspnet_client/system_web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/system_web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="panel-heading">
        <div class="panel-title">
            <h3>Cheque Printing </h3>
        </div>

    </div>
        <div class="form_main">
            <%--<table class="TableMain100">
                <tr>
                    <td class="EHEADER">
                        <strong><span style="color: #000099">Cheque Printing</span></strong>&nbsp;</td>
                </tr>
            </table>--%>
            <table  class="tableClass">
                <tr>
                    <td>
                        <%--<asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="36000">
                        </asp:ScriptManager>--%>
                        <table class="TableMain100">
                            
                            <tr>
                                <td style="padding:0 5px">
                                    Select Bank :
                                </td>
                                <td style="padding:4px 5px">
                                    <asp:DropDownList ID="ddlBank" runat="server" Width="345px" OnSelectedIndexChanged="ddlBank_SelectedIndexChanged1"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                                    
                        </table>
                        <table id="tbl_customer" runat="server">
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                               Select Cheque Type :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DropDownList1" runat="server" Width="107px" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"
                                                    AutoPostBack="True">
                                                    
                                                    <asp:ListItem Value="1">Customer</asp:ListItem>
                                                    <asp:ListItem Value="2">Others</asp:ListItem>
                                                </asp:DropDownList></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table class="TableMain100" id="tbl_Other" runat="server">
                            <tr id="Trfilter">
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                Voucher Date :</td>
                                            <td>
                                                <dxe:ASPxDateEdit ID="dtpfromDate" runat="server" ClientInstanceName="dtpDate" Width="157px"
                                                    EditFormat="Custom" UseMaskBehavior="True" EditFormatString="dd-MM-yyyy">
                                                    <dropdownbutton text="From">
                                                    </dropdownbutton>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                            <td>
                                                <dxe:ASPxDateEdit ID="dtptoDate" runat="server" ClientInstanceName="dtpDate" Width="157px"
                                                    EditFormat="Custom" UseMaskBehavior="True" EditFormatString="dd-MM-yyyy">
                                                    <dropdownbutton text="To">
                                                    </dropdownbutton>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                Cheque Type:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlChequeType" runat="server" Width="345px" AutoPostBack="false">
                                                    <asp:ListItem Value="HC">HDFC</asp:ListItem>
                                                    <asp:ListItem Value="UTI">AXIS</asp:ListItem>
                                                    <asp:ListItem Value="IC">ICICI</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                    </table>
                                </td>
                            </tr>
                            <tr id="TrDate">
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table id="tbl_ucc" runat="server">
                                        <tr>
                                            <td>
                                                Want to Print Account Number:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlAccount" runat="server">
                                                    <asp:ListItem>Yes</asp:ListItem>
                                                    <asp:ListItem>No</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                Want To Print Ucc:</td>
                                            <td>
                                                <asp:DropDownList ID="ddlUcc" runat="server">
                                                    <asp:ListItem>No</asp:ListItem>
                                                    <asp:ListItem>Yes</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <td>
                                            <asp:Button ID="btnShow" runat="server" CssClass="btn" OnClick="btnShow_Click" Text="Show" />
                                        </td>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnSave" CssClass="btn" OnClick="btnSave_Click" runat="server" Width="200px"
                                                    Text="PRINT FOR CUSTOMER"></asp:Button>
                                                <asp:Button ID="btnsave1" CssClass="btn" OnClick="btnSave1_Click" runat="server"
                                                    Width="200px" Text="PRINT FOR OTHERS"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table id="tbl_InstrumentDate" runat="server">
                            <tr>
                                <td>
                                    Instrument Date:</td>
                                <td>
                                    <dxe:ASPxDateEdit ID="dtpupdateinstrumentdate" runat="server" ClientInstanceName="dtpDate"
                                        Width="157px" EditFormat="Custom" UseMaskBehavior="True" EditFormatString="dd-MM-yyyy">
                                        <dropdownbutton text="Date">
                                                    </dropdownbutton>
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td>
                                    <asp:Button ID="btnAllocate" runat="server" CssClass="btn" OnClick="btnAllocate_Click"
                                        Text="Allocate Date" Font-Size="Smaller" Width="106px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                
                 <tr>
                    <td>
                        <table id="tbl_InstrumentDate1" runat="server">
                            <tr>
                                <td>
                                    Instrument Date:</td>
                                <td>
                                    <dxe:ASPxDateEdit ID="dtpupdateinstrumentdate1" runat="server" ClientInstanceName="dtpDate"
                                        Width="157px" EditFormat="Custom" UseMaskBehavior="True" EditFormatString="dd-MM-yyyy">
                                        <dropdownbutton text="Date">
                                                    </dropdownbutton>
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td>
                                    <asp:Button ID="btnAllocate1" runat="server" CssClass="btn" OnClick="btnAllocate1_Click"
                                        Text="Date Allocate" Font-Size="Smaller" Width="106px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table class="TableMain100">
                <tr id="TrGrid">
                    <td colspan="2">
                        <asp:Panel ID="Panel1" runat="server" Height="420px">
                            <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server" ChildrenAsTriggers>
                                <ContentTemplate>
                                    <table class="TableMain100">
                                        <tr id="trbutton">
                                            <td id="button" style="text-align: left; width: 112px; display: none" colspan="20">
                                                <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" /></td>
                                            <td>
                                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="width: 100%">
                                        <tr id="TrOffSetPosition">
                                            <td>
                                                <div style="width: 900px;">
                                                    <asp:GridView ID="gridCheque" runat="server" Width="100%" ShowFooter="True" AllowSorting="True"
                                                        AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" OnRowDataBound="gridCheque_RowDataBound"
                                                        DataMember="CashBankdetail_ID" GridLines="None" Font-Size="Smaller">
                                                        <FooterStyle BackColor="#507CD1" ForeColor="Black" Font-Bold="True"></FooterStyle>
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemStyle BorderWidth="2px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="ChkDelivery" runat="server" onclick="SelectSingle();" />
                                                                </ItemTemplate>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="cbSelectAll" runat="server" />
                                                                </HeaderTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ID">
                                                                <ItemStyle Font-Size="Small" BorderWidth="2px" HorizontalAlign="Center"></ItemStyle>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="False" Font-Size="Small" ForeColor="Black">
                                                                </HeaderStyle>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCashBankDetailID" runat="server" Text='<%# Eval("CashBankDetail_ID")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ControlStyle Font-Size="Small" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Account Name">
                                                                <ItemStyle Font-Size="Small" BorderWidth="2px" HorizontalAlign="Center"></ItemStyle>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="False" Width="130px" Font-Size="Small"
                                                                    ForeColor="Black"></HeaderStyle>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMainAccount" runat="server" Text='<%# Eval("MainAccount_Name") %>'
                                                                        Width="224px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ControlStyle Font-Size="Small" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Voucher Number">
                                                                <ItemStyle Font-Size="Small" BorderWidth="2px" HorizontalAlign="Center"></ItemStyle>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="False" Font-Size="Small" ForeColor="Black">
                                                                </HeaderStyle>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVocherNumber" runat="server" Text='<%# Eval("cashbank_vouchernumber") %>'
                                                                        Width="220px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ControlStyle Font-Size="Small" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Cheque Number">
                                                                <ItemStyle Font-Size="Small" BorderWidth="2px" HorizontalAlign="Center"></ItemStyle>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="False" Font-Size="Small" ForeColor="Black">
                                                                </HeaderStyle>
                                                                <ItemTemplate>
                                                                    <%--<asp:Label ID="lblChequeNumber" runat="server" Text='<%# Eval("CashBankDetail_InstrumentNumber")%>'></asp:Label>--%>
                                                                    <asp:TextBox ID="txtChequeNumber" runat="server" Text='<%# Eval("CashBankDetail_InstrumentNumber")%>'></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ControlStyle Font-Size="Small" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Transaction Date">
                                                                <ItemStyle Font-Size="Small" BorderWidth="2px" HorizontalAlign="Center"></ItemStyle>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="False" Font-Size="Small" ForeColor="Black">
                                                                </HeaderStyle>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBankDate" runat="server" Text='<%# Eval("CashBank_TransactionDate") %>'
                                                                        Width="151px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ControlStyle Font-Size="Small" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Payment">
                                                                <ItemStyle Font-Size="Small" BorderWidth="2px" HorizontalAlign="Center"></ItemStyle>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="False" Font-Size="Small" ForeColor="Black">
                                                                </HeaderStyle>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPaymentAmount" runat="server" Text='<%# Eval("Payment") %>' Width="213px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ControlStyle Font-Size="Small" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ClientID" Visible="False">
                                                                <ItemStyle Font-Size="Small" BorderWidth="2px" HorizontalAlign="Center"></ItemStyle>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="False" Font-Size="Small" ForeColor="Black">
                                                                </HeaderStyle>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblClientID" runat="server" Text='<%# Eval("CashBankDetail_SubAccountID")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ControlStyle Font-Size="Small" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="MainClientID" Visible="False">
                                                                <ItemStyle Font-Size="Small" BorderWidth="2px" HorizontalAlign="Center"></ItemStyle>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="False" Font-Size="Small" ForeColor="Black">
                                                                </HeaderStyle>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMainClientID" runat="server" Text='<%# Eval("CashBankDetail_MainAccountID")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ControlStyle Font-Size="Small" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Account Number">
                                                                <ItemStyle Font-Size="Small" BorderWidth="2px" HorizontalAlign="Center"></ItemStyle>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="False" Font-Size="Small" ForeColor="Black">
                                                                </HeaderStyle>
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlClientAccount" runat="server" Width="345px">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                                <ControlStyle Font-Size="Small" />
                                                            </asp:TemplateField>
                                                            <%--    <asp:TemplateField HeaderText="GRPID" Visible="False">
                                                            <ItemStyle Font-Size="Small" BorderWidth="2px" HorizontalAlign="Center"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="False" Font-Size="Small" ForeColor="Black"></HeaderStyle>
                                                            <ItemTemplate>
                                                             <asp:Label ID="lblGRPID" runat="server" Text='<%# Eval("grp_id")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ControlStyle Font-Size="Small" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Group Master" Visible="False">
                                                            <ItemStyle Font-Size="Small" BorderWidth="2px" HorizontalAlign="Center"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="False" Font-Size="Small" ForeColor="Black"></HeaderStyle>
                                                            <ItemTemplate>
                                                             <asp:Label ID="lblgroupMaster" runat="server" Text='<%# Eval("grp_groupMaster")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ControlStyle Font-Size="Small" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Group Description" Visible="False">
                                                            <ItemStyle Font-Size="Small" BorderWidth="2px" HorizontalAlign="Center"></ItemStyle>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="False" Font-Size="Small" ForeColor="Black"></HeaderStyle>
                                                            <ItemTemplate>
                                                             <asp:Label ID="lblgroupDescription" runat="server" Text='<%# Eval("GroupDescription")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ControlStyle Font-Size="Small" />
                                                        </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="Branch ID" Visible="False">
                                                                <ItemStyle Font-Size="Small" BorderWidth="2px" HorizontalAlign="Center"></ItemStyle>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="False" Font-Size="Small" ForeColor="Black">
                                                                </HeaderStyle>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBranchID" runat="server" Text='<%# Eval("cnt_branchID")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ControlStyle Font-Size="Small" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Branch Description" Visible="False">
                                                                <ItemStyle Font-Size="Small" BorderWidth="2px" HorizontalAlign="Center"></ItemStyle>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="False" Font-Size="Small" ForeColor="Black">
                                                                </HeaderStyle>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBranchDescription" runat="server" Text='<%# Eval("BranchDescription")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ControlStyle Font-Size="Small" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <RowStyle BackColor="#EFF3FB" BorderColor="#BFD3EE" BorderStyle="Double" BorderWidth="2px">
                                                        </RowStyle>
                                                        <EditRowStyle BackColor="#2461BF"></EditRowStyle>
                                                        <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
                                                        <PagerStyle ForeColor="White" HorizontalAlign="Center" BackColor="#2461BF"></PagerStyle>
                                                        <HeaderStyle ForeColor="White" BorderWidth="2px" CssClass="EHEADER" BorderColor="AliceBlue"
                                                            Font-Bold="True" BackColor="#507CD1"></HeaderStyle>
                                                        <AlternatingRowStyle BackColor="White" />
                                                    </asp:GridView>
                                                    <asp:GridView ID="gridother" runat="server" Width="100%" ShowFooter="True" AllowSorting="True"
                                                        AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" OnRowDataBound="gridother_RowDataBound"
                                                        DataMember="CashBankdetail_ID" GridLines="None" Font-Size="Smaller">
                                                        <FooterStyle BackColor="#507CD1" ForeColor="Black" Font-Bold="True"></FooterStyle>
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemStyle BorderWidth="2px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="ChkDelivery1" runat="server" onclick="SelectSingle1();" />
                                                                </ItemTemplate>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="cbSelectAll1"  Visible="False" runat="server"/>
                                                                </HeaderTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ID">
                                                                <ItemStyle Font-Size="Small" BorderWidth="2px" HorizontalAlign="Center"></ItemStyle>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="False" Font-Size="Small" ForeColor="Black">
                                                                </HeaderStyle>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCashBankDetailID1" runat="server" Text='<%# Eval("CashBankDetail_ID")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ControlStyle Font-Size="Small" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Account Name">
                                                                <ItemStyle Font-Size="Small" BorderWidth="2px" HorizontalAlign="Center"></ItemStyle>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="False" Width="130px" Font-Size="Small"
                                                                    ForeColor="Black"></HeaderStyle>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMainAccount1" runat="server" Text='<%# Eval("MainAccount_Name") %>'
                                                                        Width="224px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ControlStyle Font-Size="Small" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Voucher Number">
                                                                <ItemStyle Font-Size="Small" BorderWidth="2px" HorizontalAlign="Center"></ItemStyle>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="False" Font-Size="Small" ForeColor="Black">
                                                                </HeaderStyle>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVocherNumber1" runat="server" Text='<%# Eval("cashbank_vouchernumber") %>'
                                                                        Width="220px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ControlStyle Font-Size="Small" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Cheque Number">
                                                                <ItemStyle Font-Size="Small" BorderWidth="2px" HorizontalAlign="Center"></ItemStyle>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="False" Font-Size="Small" ForeColor="Black">
                                                                </HeaderStyle>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtChequeNumber1" runat="server" Text='<%# Eval("CashBankDetail_InstrumentNumber")%>'></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ControlStyle Font-Size="Small" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Transaction Date">
                                                                <ItemStyle Font-Size="Small" BorderWidth="2px" HorizontalAlign="Center"></ItemStyle>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="False" Font-Size="Small" ForeColor="Black">
                                                                </HeaderStyle>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBankDate1" runat="server" Text='<%# Eval("CashBank_TransactionDate") %>'
                                                                        Width="151px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ControlStyle Font-Size="Small" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Payment">
                                                                <ItemStyle Font-Size="Small" BorderWidth="2px" HorizontalAlign="Center"></ItemStyle>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="False" Font-Size="Small" ForeColor="Black">
                                                                </HeaderStyle>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPaymentAmount1" runat="server" Text='<%# Eval("Payment") %>' Width="213px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ControlStyle Font-Size="Small" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ClientID" Visible="False">
                                                                <ItemStyle Font-Size="Small" BorderWidth="2px" HorizontalAlign="Center"></ItemStyle>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="False" Font-Size="Small" ForeColor="Black">
                                                                </HeaderStyle>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblClientID1" runat="server" Text='<%# Eval("CashBankDetail_SubAccountID")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ControlStyle Font-Size="Small" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="MainClientID" Visible="False">
                                                                <ItemStyle Font-Size="Small" BorderWidth="2px" HorizontalAlign="Center"></ItemStyle>
                                                                <HeaderStyle HorizontalAlign="Center" Font-Bold="False" Font-Size="Small" ForeColor="Black">
                                                                </HeaderStyle>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMainClientID1" runat="server" Text='<%# Eval("CashBankDetail_MainAccountID")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ControlStyle Font-Size="Small" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <RowStyle BackColor="#EFF3FB" BorderColor="#BFD3EE" BorderStyle="Double" BorderWidth="2px">
                                                        </RowStyle>
                                                        <EditRowStyle BackColor="#2461BF"></EditRowStyle>
                                                        <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
                                                        <PagerStyle ForeColor="White" HorizontalAlign="Center" BackColor="#2461BF"></PagerStyle>
                                                        <HeaderStyle ForeColor="White" BorderWidth="2px" CssClass="EHEADER" BorderColor="AliceBlue"
                                                            Font-Bold="True" BackColor="#507CD1"></HeaderStyle>
                                                        <AlternatingRowStyle BackColor="White" />
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger EventName="Click" ControlID="btnAllocate" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <contenttemplate>
                                
                                    <table>
                                        <tr>
                                        <td style="display:none;">
                                          <asp:HiddenField ID="hdnID" runat="server" />
                                        <asp:HiddenField ID="hdnbank" runat="server" />
                                        <asp:HiddenField ID="hdnFromdate" runat="server" />
                                        <asp:HiddenField ID="hdnTodate" runat="server" />
                                        <asp:HiddenField ID="hdnstrBank" runat="server" />
                                        <asp:HiddenField ID="hdnChequeNumber" runat="server" />
                                        <asp:HiddenField ID="hdnBankType" runat="server" />
                                        <asp:HiddenField ID="hdnType" runat="server" />
                                        <asp:HiddenField ID="hdnAccount" runat="server" />
                                        <asp:HiddenField ID="hdnUcc" runat="server" />
                                        
                                        <asp:HiddenField ID="hdnID1" runat="server" />
                                        <asp:HiddenField ID="hdnstrBank1" runat="server" />
                                        <asp:HiddenField ID="hdnChequeNumber1" runat="server" />
                                       
                                 
                                        </td>
                                      </tr>
                                    </table>
                                </contenttemplate>
        </div>
</asp:Content>
