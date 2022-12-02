
<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.management_journalvoucherledgeredit" Codebehind="~/Reports/journalvoucherledgeredit.aspx.cs" %>
<%--<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.v10.2.Export" Namespace="DevExpress.Web.Export" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxCallbackPanel"
    TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
        function PageLoad()
        {
            cMsgPopUp.Show();
        }
        function SignOff()
        {
         window.parent.SignOff();
        }
        var isCtrl = false;
        document.onkeyup=function(e)
        {
	        if(event.keyCode == 17) 
	        {
	            isCtrl=false;
	        }
	        if(event.keyCode == 27)
	        {
	            btnCancel_Click();
	        }
        }
        document.onkeydown=function(e)
        {
	        if(event.keyCode == 17) isCtrl=true;
	        if(event.keyCode == 83 && isCtrl == true) 
	        {
		        //run code for CTRL+S -- ie, save!
		        var debit=document.getElementById('txtTDebit').value;
		        var credit=document.getElementById('txtTCredit').value;
		        if(debit==credit)
		        {
		            document.getElementById('btnSave').click();    
		            return false;
		        }
		        else
		        {
		            alert('Credit And Debit Must Be Same');
		            return false;
		        }
	        }
        } 
        function height()
        {
            if(document.body.scrollHeight>=300)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '300px';
            window.frameElement.Width = document.body.scrollWidth;
        }      
        function CallList(obj1,obj2,obj3)
         {
            ajax_showOptions(obj1,obj2,obj3);    
         }  
         function updateEditorText() 
         {
            var code=txtAccountCode.GetText().toUpperCase();
            if(code=='X' || code=='Y' || code=='Z' || code=='W' || code=='V' || code=='U' || code=='T')
            {
                alert('{T,U,V,W,X,Y,Z} are Reserved Key');
                txtAccountCode.SetText('JV');
            }
         }
         function CallMainAccount(obj1,obj2,obj3)
         {
            ajax_showOptions(obj1,obj2,obj3,null,'Main');
         }
         function CallSubAccount(obj1,obj2,obj3)
         {
            var valSub;
            var HdVal=document.getElementById("hddnEdit").value;
            if(HdVal=='Edit')
            {
               var BranchID=document.getElementById("ddlBranch").value;
               valSub=val+'~'+BranchID;
            }
            else
               valSub=val+'~'+'N'; 
            ajax_showOptions(obj1,obj2,obj3,valSub,'Main');
         } 
         function CallMainAccountE(obj1,obj2,obj3)
         {
           ajax_showOptions(obj1,obj2,obj3,null,'Main');
         }
         function CallSubAccountE(obj1,obj2,obj3)
         {
            var valSub;
            valSub=valE+'~'+'N'; 
            ajax_showOptions(obj1,obj2,obj3,valSub,'Main');
         } 
         function keyVal(obj)
         {
            //alert(obj);
            var spObj=obj.split('~');
            var WhichQuery=spObj[2];
            
            //alert(WhichQuery);
            if(WhichQuery=='MAINAC')
            {
                if(spObj[3]!='Edit')
                {
                    val=spObj[0];
                    valLedgerType=spObj[1];                
                    document.getElementById("txtMainAccount_hidden").value=val;  
                }
                else
                {
                    valE=spObj[0];
                    valLedgerTypeE=spObj[1]; 
                    document.getElementById("txtMainAccountE_hidden").value=val;  
                }
                if(valLedgerType.toUpperCase()=='NONE')
                {
                    document.getElementById('tdSubAccount').value='';
                    document.getElementById('tdSubAccount').style.display='none';
                    document.getElementById('tdlblSubAccount').style.display='none';
                    document.getElementById('hdn_MainAcc_Type').value='None';
                    document.getElementById('hdn_Brch_NonBrch').value='NAB';
                    document.getElementById('txtSubAccount_hidden').value='';
                    
                }
                else
                {
                    document.getElementById('tdSubAccount').style.display='inline';
                    document.getElementById('tdlblSubAccount').style.display='inline';
                    document.getElementById('hdn_MainAcc_Type').value='NotNone';
                   
                }
            }
            else if(WhichQuery=="SUBAC")
            {
                var Branch=spObj[1];
                document.getElementById('hdn_Brch_NonBrch').value=Branch;
                var MainAcCode=document.getElementById('txtMainAccount_hidden').value;
                var SubAc=document.getElementById('txtSubAccount_hidden').value;
                cCbpAcBalance.PerformCallback('AcBalance~'+MainAcCode+'~'+SubAc);
                //alert(Branch);
            }
            else
            {
                document.getElementById('hdn_Brch_NonBrch').value='NAB';
            }
         }
         function SubAccountCheck()
         {
            if(valLedgerType.toUpperCase()!='NONE')
            {
                var testSubAcc1=document.getElementById("txtSubAccount");
                if(testSubAcc1.value=='' || testSubAcc1.value=='No Record Found')
                {
                    alert('SubAccount Name Required !!!');
                    testSubAcc1.focus();
                    testSubAcc1.select();
                    return false;    
                }
             }
             var Withdraw=ctxtdebit.GetValue();
             var Receipt=ctxtcredit.GetValue();
             var WithReceipt=parseFloat(Withdraw)+parseFloat(Receipt);
             if(WithReceipt=="0")
             {
                alert('Debit/Credit  Required !!!');
                ctxtdebit.Focus();
                return false;    
             }
             tdadd.style.display='none'
             tdnew.style.display='inline'
             document.getElementById('btnnew').focus();
             var SubAccountBranch=document.getElementById('hdn_Brch_NonBrch').value;
             cDetailsGrid.PerformCallback('Add~'+SubAccountBranch);
         }
         function SubAccountCheckUpdate(obj)
         {
            var obj1=obj.split('_');
            if(valLedgerType.toUpperCase()!='NONE')
            {
                var obj2='grdAdd'+'_'+obj1[1]+'_'+'txtEditSubAccount';
                var testSubAcc1=document.getElementById(obj2);
                if(testSubAcc1.value=='')
                {
                    alert('SubAccount Name Required !!!');
                    testSubAcc1.focus();
                    testSubAcc1.select();
                    return false;    
                }
             }
             var Withdraw=txtEditWithdraw.GetValue();
             var Receipt=txtEditRecpt.GetValue();
             var WithReceipt=parseFloat(Withdraw)+parseFloat(Receipt);
             if(WithReceipt=="0")
             {
                alert('Debit/Credit  Required !!!');
                return false;    
             }
         }
         function btnCancel_Click()
         {
            var answer = confirm ("Do you Want To Close This Window?");
            if (answer)
                parent.editwin.close();
         }
         function Page_Load()
         {
            document.getElementById("btnSave").disabled=true;
            document.getElementById("btnInsert").disabled=true;
         }
         function Page_Load1()
         {
            document.getElementById("tdSeg1").style.display="none";
            document.getElementById("tdSeg2").style.display="none";
         }
         function Button_Click()
         {
            document.getElementById("btnSave").disabled=false;
            document.getElementById("btnInsert").disabled=false;
         }
         function SetSubAcc1(obj)
         {
            var s=document.getElementById('txtSubAccount');
            s.focus();
            s.select();
         }
//         function keyVal1(obj)
//         {s
//            alert(obj);
//            val=obj
//         }
         function PopulateData()
            {
                parent.RefreshGrid();
            }
         function Narration(obj)
         {
            document.getElementById("txtNarration1").value=obj;
         }
         function Narration1()
         {
            document.getElementById("txtNarration1").value='';
         }
         function overChange(obj)
         {
            obj.style.backgroundColor = "#FFD497";
         }
         function OutChange(obj)
         {
            obj.style.backgroundColor = "#DDECFE";
         }
         function focusval(obj)
         {
            if(obj!='0.00')
            {
               ctxtcredit.SetEnabled(false);  
               ctxtcredit.SetText('0000000000.00');            
               OnlyPayment(obj,'Dr');
            }
            else
            {
                 ctxtcredit.SetEnabled(true);
            }
         }
         function focusval1(obj)
         {
            if(obj!='0.00')
            {
                ctxtdebit.SetEnabled(false);
                ctxtdebit.SetText('0000000000.00');
                OnlyPayment(obj,'Cr');
            }
            else    
            {
                ctxtdebit.SetEnabled(true);
            }
         }
         
         function Efocusval(obj)
         {
            if(obj!='0.00')
            {
                ctxtcreditE.SetEnabled(false);  
                ctxtcreditE.SetText('0.00');              
            }
            else
            {
                 ctxtcreditE.SetEnabled(true);
            }
         }
         function Efocusval1(obj)
         {
            if(obj!='0.00')
            {
                ctxtdebitE.SetEnabled(false);
                ctxtdebitE.SetText('0.00');
            }
            else    
            {
                ctxtdebitE.SetEnabled(true);
            }
         }
         function SelectSegment()
         {
            var obj=document.getElementById('ddlIntExchange').value;
            if(obj=="0")
            {
                document.getElementById("tdSeg1").style.display="none";
                document.getElementById("tdSeg2").style.display="none";
                txtAccountCode.SetText('JV');
                txtAccountCode.SetEnabled(true);
            }
            else    
            {
                document.getElementById("tdSeg1").style.display="inline";
                document.getElementById("tdSeg2").style.display="inline";
                txtAccountCode.SetText('YF');
                txtAccountCode.SetEnabled(false);
            }
         }
         function SegmentName()
         {
            var obj=document.getElementById('ddlSegment').value;
            var obj1=document.getElementById('ddlTntraExchange').value;
            if(obj==obj1)
            {
                alert('Segment And Segment2 Must Be Different');
                document.getElementById('ddlTntraExchange').selectedIndex='0';
                return false;
            }
         }
         function Narration_Off()
         {
             document.getElementById('TrNarration').style.display='none';
         }
         function Narration_Val(obj)
         {
            document.getElementById('TrNarration').style.display='inline';
            document.getElementById('txtNarration1').value=obj;
         }
           ////This Method is Used For Checking Lock Date and Financial Year and Alert User For That if Date OutSide
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
          function AcBalance(obj)
          {
                var comp=document.getElementById('hdnCompanyid').value;
                var segmnt=document.getElementById('hdnSegmentid').value;
                var date=document.getElementById('tDate_I').value; 
                var dest=document.getElementById('CbpAcBalance'); 
                var Suba=obj+"_hidden";
                var SubAcc=document.getElementById(Suba).value;
                var param=comp+'~'+segmnt+'~'+date+'~'+val+'~'+SubAcc;
                //alert(param);
                PageMethods.GetContactName(param, CallSuccess, CallFailed, dest);
          }
          function CallSuccess(res, destCtrl)
          { 
              //destCtrl.innerText=res;
              var cc=res.substr(0, 1);
              if(cc=='-')
              {
                 //cc=res * (-1);
                 cc=res +' (DR)';
                 lbltype='DR';
                 destCtrl.innerText=cc;
                 destCtrl.style.color='red';
              }
              else
              {
                 cc=res+' (CR)';
                 lbltype='CR';
                 destCtrl.innerText=cc;
                 destCtrl.style.color='blue';
              }
              lblval=res;
          }
          function CallFailed(res, destCtrl)
          {
              alert(res.get_message());
          }
          function alertMessage()
          {
                alert('This Voucher has multi branch enters.\n Please Provide a single account for counter entry !!')
          }
          function btnInsert_Click()
          {
               document.getElementById('Div1').style.display='inline';
               document.getElementById('btnInsert').disabled=true;
               document.getElementById('btnSave').click();
          }
          function AlertAfterInsert()
            {
                var answer = confirm ("Do You Want To Print this page??");
                if (answer)
                {
                   document.getElementById('btnPrint').click();
                } 
            }
          function OnlyPayment(obj,objType)
            {
                if(lbltype=='DR' && objType=="Cr")
                {
                    var str=lblval;
                    str=str.replace(",", "");
                    str=Math.abs(str);
                    if(parseFloat(str)<parseFloat(obj))
                    {
                        document.getElementById('bDrCrStatus').innerHTML = '(Credit Is Greater Than Debit)';
                        document.getElementById('bDrCrStatus').style.font = "red";
                    }
                    else
                    {
                         document.getElementById('bDrCrStatus').innerHTML = '';
                    }
                }
                if(lbltype=='CR' && objType=="Dr")
                {
                    var str=lblval;
                    str=str.replace(",", "");
                    str=Math.abs(str);
                    if(parseFloat(str)<parseFloat(obj))
                    {
                        document.getElementById('bDrCrStatus').innerHTML = '(Debit Is Greater Than Credit)';
                        document.getElementById('bDrCrStatus').style.font = "blue";
                    }
                    else
                    {
                         document.getElementById('bDrCrStatus').innerHTML = '';
                    }
                }
            }
            function OnlyNarration(obj1,obj2,obj3)
             {
                ajax_showOptions(obj1,obj2,obj3);    
             }
         FieldName='txtPrefix';
         function AddButtonClick()
         {
            cDetailsGrid.PerformCallback('Add');
         }
         function CancelButtonClick()
         {
            Asp_SetSpace('txtMainAccount');
            Asp_SetSpace('txtSubAccount');
            document.getElementById('txtMainAccount').focus();
         }
         function SaveButtonClick()
         {
            if(cDetailsGrid.GetVisibleRowsOnPage() != '0' )
            {
               cDetailsGrid.PerformCallback('Save');
               
            }
            else
            {
                alert('Please Add Atleast Single Record First');
                document.getElementById('txtMainAccount').focus();
            }
            
         }
         function DiscardButtonClick()
         {
           var data='Are You Sure.It will Discard All Data You Entered';
           var answer = confirm(data)
	       if (answer)
	       {
             cDetailsGrid.PerformCallback('Discard');
           }
         }
         function CloseButtonClick()
         {
            var answer = confirm ("Do you Want To Close This Window WithOut Saving?\n Close Window Using Close Button(Cross Button Upper Right) to Automatically Save.");
            if (answer)
                cGvJvSearch.PerformCallback('PCB_EditEnd~');
                setTimeout(100);
                parent.editwin.close();
                
            
         }
         function NewButtonClick()
         {
            if(parseFloat(ctxtTDebit.GetValue())>parseFloat(ctxtTCredit.GetValue()))
            {
             ctxtdebit.SetText('0000000000.00');
            }
            else
            {
              ctxtcredit.SetText('0000000000.00');  
            }
            tdadd.style.display='inline'
            tdnew.style.display='none'
            ctxtcredit.SetEnabled(true); 
            ctxtdebit.SetEnabled(true);
            if(document.getElementById('tdSubAccount').style.display=='none')
            {
                document.getElementById('txtMainAccount').focus();
            }
            else
            {
                document.getElementById('txtSubAccount').value='';
                document.getElementById('txtSubAccount').focus();
            }
            document.getElementById('CbpAcBalance').innerHTML = '';
            document.getElementById('bDrCrStatus').innerHTML = '';
         }
         
         function SetDebitCreditValue(obj)
         {
            var TotalDebit=(obj.split('~')[0]);
            var TotalCredit=(obj.split('~')[1]);
            var RemainingDebit=(obj.split('~')[2]);
            var RemainingCredit=(obj.split('~')[3]);
            //alert(TotalDebit+' '+TotalDebit+' '+RemainingDebit+' '+RemainingCredit);
            ctxtdebit.SetText('0000000000.00');
            ctxtcredit.SetText('0000000000.00');
            ctxtdebit.SetText(RemainingDebit.toString());
            ctxtcredit.SetText(RemainingCredit.toString());
            ctxtTDebit.SetText('0000000000.00');
            ctxtTCredit.SetText('0000000000.00');
            ctxtTDebit.SetText(TotalDebit.toString());
            ctxtTCredit.SetText(TotalCredit.toString());
            
            if(TotalDebit==TotalCredit)
            {
                tdSaveButton.style.display="inline"
                ctxtdebit.SetText('0000000000.00');
                ctxtcredit.SetText('0000000000.00');  
            }
            else
            {   
                if(parseFloat(ctxtTDebit.GetValue())>parseFloat(ctxtTCredit.GetValue()))
                {
                 ctxtdebit.SetText('0000000000.00');
                }
                else
                {
                  ctxtcredit.SetText('0000000000.00');  
                }
                tdSaveButton.style.display="none"
            }
            if(cDetailsGrid.cpEntryNotAllow!="undefined")
            {
                if(cDetailsGrid.cpEntryNotAllow!="Empty")
                {
                    alert(cDetailsGrid.cpEntryNotAllow);
                    cDetailsGrid.cpEntryNotAllow="undefined";
                }
            }
            if(cDetailsGrid.cpSaveSuccessOrFail!="undefined")
            {
                if(cDetailsGrid.cpSaveSuccessOrFail=="Problem")
                {
                    alert("There is Some Problem. Sry for InConvenience");
                    cDetailsGrid.cpSaveSuccessOrFail="undefined";
                }
                else if(cDetailsGrid.cpSaveSuccessOrFail=="Success")
                {
                    alert("Records Successfully Saved");
                    cDetailsGrid.cpSaveSuccessOrFail="undefined";
                    var answer = confirm ("Do You Want To Print Saved JV/JVs?");
                    if (answer)
                    {
                        document.getElementById('btnPrint').click();
                        TblMainEntryForm.style.display="none";
                    }
                    else
                    {
                        parent.editwin.close();
                    } 
                     tdAcBal.style.display="none";
                }
                else
                {
                }
            }
            if(cDetailsGrid.cpSuccessDiscard!="undefined")
            {
                if(cDetailsGrid.cpSuccessDiscard=="Problem")
                {
                    alert('There is Some Problem. Sry for InConvenience');
                    cDetailsGrid.cpSuccessDiscard="undefined";
                }
                else if(cDetailsGrid.cpSuccessDiscard=="SuccessDiscard")
                {
                    ResetPageOnDiscard();
                    alert('Records Successfully Discard');
                    cDetailsGrid.cpSuccessDiscard="undefined";
                }
                else
                {
                }
            }
            if(cDetailsGrid.cpEntryData!="undefined")
            {
            
                if(cDetailsGrid.cpBillNo!="EmptyString")document.getElementById('txtBillNo').value=cDetailsGrid.cpBillNo;
                else document.getElementById('txtBillNo').value='';
                if(cDetailsGrid.cpJvNarration!="EmptyString")document.getElementById('txtNarration').value=cDetailsGrid.cpJvNarration;
                else document.getElementById('txtNarration').value='';
                if(cDetailsGrid.cpPrefix!="EmptyString")txtAccountCode.SetText(cDetailsGrid.cpPrefix);
                else txtAccountCode.SetText('');
                var ddlBranch= document.getElementById("<%=ddlBranch.ClientID%>");
                if(cDetailsGrid.cpBranchSelectedValue!="EmptyString")ddlBranch.options[cDetailsGrid.cpBranchSelectedValue].selected=true;
                else ddlBranch.options[0].selected=true;
            }
            if(cDetailsGrid.cpHideAddBtnOnLock=="true")
            {
                cbtnadd.SetEnabled(true);
                cbtnDelVoucher.SetEnabled(true);
                cbtnSaveRecords.SetEnabled(true);
                cDetailsGrid.cpHideAddBtnOnLock=null;
                document.getElementById('bLockIdicator').innerHTML="";
            }
            if(cDetailsGrid.cpHideAddBtnOnLock=="false")
            {
                cbtnadd.SetEnabled(false);
                cbtnDelVoucher.SetEnabled(false);
                cbtnSaveRecords.SetEnabled(false);
                document.getElementById('bLockIdicator').innerHTML=" Data Has Been Locked !!! ";
                cDetailsGrid.cpHideAddBtnOnLock=null;
            } 
             
            //Currency Setting
            if(cDetailsGrid.cpSetCurrencyNameSymbol != null)
            {
                var ChoosenCurrencyName=cDetailsGrid.cpSetCurrencyNameSymbol.split('~')[0];
                var ChoosenCurrencySymbol=cDetailsGrid.cpSetCurrencyNameSymbol.split('~')[1];
                document.getElementById('<%=B_ChoosenCurrency.ClientID %>').innerHTML="Voucher Currency : "+ChoosenCurrencyName+"["+ChoosenCurrencySymbol+"]";
            }
           height();
         }
         function EntryButtonClick()
         {
             TblMainEntryForm.style.display="inline";
             document.getElementById('txtMainAccount').focus();
             cDetailsGrid.PerformCallback('Entry');
             cDetailsGrid.cpEntryNotAllow='';
         }
         function SearchButtonClick()
         {
           cSearchPopUp.Show();
         }
         function ResetPageOnDiscard()
         {
            Asp_SetSpace('txtMainAccount');
            Asp_SetSpace('txtSubAccount');
            ctxtcredit.SetText('0000000000.00');
            ctxtdebit.SetText('0000000000.00');
            ctxtTCredit.SetText('0000000000.00');
            ctxtTDebit.SetText('0000000000.00');
            tdadd.style.display='inline';
            tdnew.style.display='none';
            tdSaveButton.style.display='inline';
            if(document.getElementById('txtMainAccount').style.display!='none')
            {
                document.getElementById('txtMainAccount').focus();
            }
            else
            {
                tDate.focus();
            }
            document.getElementById('CbpAcBalance').innerHTML = '';
            document.getElementById('txtNarration1').value = '';
         }
         function Asp_SetSpace(obj)
         {
            document.getElementById(obj).value='';
         }
         function focustxtMainAccountOnUpdateCancel()
         {
            document.getElementById('txtMainAccount').focus();
         }
        function OnComboModeSelectedIndexChanged()
        {
            TblMainEntryForm.style.display="none";
            TblSearch.style.display="none";
            document.getElementById('txtBillNo').value='';
            document.getElementById('txtNarration').value='';
            Asp_SetSpace('txtMainAccount');
            Asp_SetSpace('txtSubAccount');
            Asp_SetSpace('txtNarration1');
            ctxtdebit.SetEnabled(true);
            ctxtcredit.SetEnabled(true);
            ctxtcredit.SetText('0000000000.00');
            ctxtdebit.SetText('0000000000.00');
            ctxtTCredit.SetText('0000000000.00');
            ctxtTDebit.SetText('0000000000.00');
            tdadd.style.display='inline'
            tdnew.style.display='none'
            var ddlBranch = document.getElementById("<%=ddlBranch.ClientID%>");
            ddlBranch.options[0].selected=true;
            tDate.Focus();
            var SelectedValue=cComboMode.GetValue();
            
        }
        function btnShowClick()
        {
            cGvJvSearch.PerformCallback('PCB_BtnShow~');
            cSearchPopUp.Hide();
//            if(cChkBranch.GetChecked()) cChkBranch.SetChecked(false);
//            if(cChkBillNo.GetChecked()) cChkBillNo.SetChecked(false);
//            if(cChkPrefix.GetChecked()) cChkPrefix.SetChecked(false);
//            if(cChkNarration.GetChecked()) cChkNarration.SetChecked(false);
            TblSearch.style.display="inline";
        }
        function CustomButtonClick(s, e) {
            var TransactionDate=new Date(tDate.GetDate());
            monthnumber = TransactionDate.getMonth();
            monthday    = TransactionDate.getDate();
            year        = TransactionDate.getYear();
            var TransactionDateNumeric=new Date(year, monthnumber, monthday).getTime();
            
            var MaxLockDate=new Date('<%=Session["LCKJV"]%>');
            monthnumber = MaxLockDate.getMonth();
            monthday    = MaxLockDate.getDate();
            year        = MaxLockDate.getYear();
            var MaxLockDateNumeric=new Date(year, monthnumber, monthday).getTime();
//                alert('TransactionDate='+TransactionDate+'\n'+'MaxLockDate= '+MaxLockDate);
            //alert(ValueDate+'~'+ValueDateNumeric+'~'+VisibleIndexE);
            if(TransactionDateNumeric<=MaxLockDateNumeric)
            {
                alert('This Entry has been Locked.You Can Only View The Detail');
                VisibleIndexE=e.visibleIndex;
                cGvJvSearch.PerformCallback('PCB_BtnOkE~'+e.visibleIndex);
                return;
            }
		      if(e.buttonID == 'CustomBtnEdit') {
		            cMsgPopUp.Show();
		            VisibleIndexE=e.visibleIndex;
		            
		      }
		      if(e.buttonID == 'CustomBtnDelete') {
		            cDeleteMsgPopUp.Show();
		            VisibleIndexE=e.visibleIndex;
		      }
            }
         
         function btnOkClick()
         {
            cGvJvSearch.PerformCallback('PCB_BtnOkE');
            cSearchPopUp.Hide();
         }
         function DeletebtnOkClick()
         {
            cGvJvSearch.PerformCallback('PCB_DeleteBtnOkE');
         }
         function btnContinueClick()
         {
            cGvJvSearch.PerformCallback('PCB_ContinueWith');
         }
         function btnFreshEntryClick()
         {
            cGvJvSearch.PerformCallback('PCB_FreshEntry');
         }
         function btnCloseClick()
         {
            cGvJvSearch.PerformCallback('PCB_CloseEntry');
         }
         
         function GvJvSearch_EndCallBack()
         {
            if(cGvJvSearch.cpJVE_FileAlreadyUsedBy!=undefined)
            {
                var obj=cGvJvSearch.cpJVE_FileAlreadyUsedBy;
                var WhichUser=(obj.split('~')[0]);
                if(WhichUser=="Other")
                {
                    alert('This File Being Used By '+obj.split('~')[1]);
                }
                else
                {
                    cFileUsedByPopUp.Show();
                }
               
            }
            if(cGvJvSearch.cpEntryEventFire!=undefined)
            {
                TblSearch.style.display="none";
                EntryButtonClick();
            }
            if(cGvJvSearch.cpJVDelete!=undefined)
            {
                alert(cGvJvSearch.cpJVDelete);
                parent.editwin.close();
            }
            if(cGvJvSearch.cpJVClose!=undefined)
            {
                alert(cGvJvSearch.cpJVClose);
                parent.editwin.close();
            }
            height();
         }
         function ExitButtonClick()
         {
            cGvJvSearch.PerformCallback('PCB_CloseEntry');
         }
         function DelVoucherButtonClick()
         {
            cDeleteMsgPopUp.Show();
         }
          function blinkIt() 
        {
            if (!document.all) return;
            else 
            {
              for(i=0;i<document.all.tags('blink').length;i++)
              {
                s=document.all.tags('blink')[i];
                s.style.visibility=(s.style.visibility=='visible') ?'hidden':'visible';
              }
            }
        }
          //Currency Setting
        function PageLoad_ForCurrency()
        {
            var ActiveCurrency = '<%=Session["ActiveCurrency"]%>'
            ActiveCurrencyID=ActiveCurrency.split('~')[0];
            ActiveCurrencyName=ActiveCurrency.split('~')[1];
            ActiveCurrencySymbol=ActiveCurrency.split('~')[2];
        }
        function CurrencySetting(CParam)
        {
            var ActiveCurrency = CParam;
            ActiveCurrencyID=ActiveCurrency.split('~')[0];
            ActiveCurrencyName=ActiveCurrency.split('~')[1];
            ActiveCurrencySymbol=ActiveCurrency.split('~')[2];
            document.getElementById('<%=B_ChoosenCurrency.ClientID %>').innerHTML="Voucher Currency : "+ActiveCurrencyName+"["+ActiveCurrencySymbol+"]";
        }
        function ChangeCurrency()
        {
            cCbpChoosenCurrency.PerformCallback("ChangeCurrency");
        }
        function CbpChoosenCurrency_EndCallBack()
        {
//            alert(cCbpChoosenCurrency.cpChangeCurrencyParam);
            if(cCbpChoosenCurrency.cpChangeCurrencyParam != null)
            {
                ActiveCurrencyName = cCbpChoosenCurrency.cpChangeCurrencyParam.split('~')[0];
                ActiveCurrencySymbol=cCbpChoosenCurrency.cpChangeCurrencyParam.split('~')[1];
                document.getElementById('<%=B_ChoosenCurrency.ClientID %>').innerHTML="Voucher Currency : "+ActiveCurrencyName+"["+ActiveCurrencySymbol+"]";
            }
        } 
        function CbpAcBalance_EndCallBack()
         {
            var strUndefined=new String(cCbpAcBalance.cpAcBalance);
            if(strUndefined != "undefined")
            {
                document.getElementById('<%=B_AcBalance.ClientID %>').innerHTML = strUndefined.split('~')[0];
                document.getElementById('<%=B_AcBalance.ClientID %>').style.color = strUndefined.split('~')[1];
            }
         }  
        //////////
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>
          
            <b id="bSegmentName" runat="server" style="text-decoration:underline;width:100%;color:Blue;"></b>&nbsp;&nbsp;&nbsp;
            <blink><b id="bLockIdicator" style="text-decoration:underline;width:100%;color:Red;"></b></blink>
             <!--Currency Setting-->
            <dxe:ASPxCallbackPanel ID="CbpChoosenCurrency" runat="server" ClientInstanceName="cCbpChoosenCurrency" BackColor="Transparent" OnCallback="CbpChoosenCurrency_Callback">
            <PanelCollection>
                <dxe:panelcontent runat="server">
                 <b title="Switch To Active Currency" id="B_ChoosenCurrency" runat="server" style="text-decoration: underline; width: 100%;
                        font-style:italic;color: Blue; float: left;"></b>
                </dxe:panelcontent>
            </PanelCollection>
            <ClientSideEvents EndCallback="function(s, e) {
                                CbpChoosenCurrency_EndCallBack(); }" />
        </dxe:ASPxCallbackPanel>  
            <table class="TableMain100" style="width: 99%;">
                <tr>
                    <td>
                         <table class="TableMain100" style="width: 91%; height: 72px; margin-left:20px" border="1">
                            <tr>
                                <td valign="top" style="background-color: #b7ceec; height: 11px; width: 52px;">
                                    TranDate</td>
                                <td style="width: 63px; height: 11px;" valign="top">
                                    <dxe:ASPxDateEdit ID="tDate" runat="server" EditFormat="Custom" ClientInstanceName="tDate"
                                        UseMaskBehavior="True" Font-Size="13px" Width="157px">
                                        <ClientSideEvents DateChanged="function(s,e){DateChange() }" />
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td valign="top" style="background-color: #b7ceec; width: 45px; height: 11px;">
                                    Branch</td>
                                <td style="width: 119px; height: 11px;" valign="top">
                                    <asp:DropDownList ID="ddlBranch" runat="server" DataSourceID="dsBranch"
                                        DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="204px"
                                        Font-Size="13px">
                                    </asp:DropDownList></td>
                                <td valign="top">
                                    <asp:TextBox ID="txtNarration" Font-Names="Arial" runat="server" TextMode="MultiLine" 
                                      Width="97%" Font-Size="10pt" Height="42px" onkeyup="OnlyNarration(this,'Narration',event)"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="height: 3px; background-color: #b7ceec; width: 52px;" valign="top">
                                    Bill No</td>
                                <td style="width: 63px; height: 3px" valign="top">
                                    <asp:TextBox ID="txtBillNo" runat="server" Width="149px" Font-Size="13px"></asp:TextBox></td>
                                <td style="width: 45px; height: 3px; background-color: #b7ceec" valign="top">
                                    Prefix</td>
                                <td style="width: 119px; height: 3px" valign="top">
                                    <dxe:ASPxTextBox ID="txtAccountCode" ClientInstanceName="txtAccountCode"
                                        runat="server" Width="44px" MaxLength="2" Font-Size="13px">
                                        
                                        <ClientSideEvents KeyPress="function(s,e){window.setTimeout('updateEditorText()', 10);}" />
                                    </dxe:ASPxTextBox></td>
                                
                               
                                 <td style="font-weight:bold;font-size:10px;width: 40%;" id="tdAcBal" colspan="2">
                                    
                                A/C Balance : <dxe:ASPxCallbackPanel ID="CbpAcBalance" runat="server" ClientInstanceName="cCbpAcBalance"
                                        OnCallback="CbpAcBalance_Callback" BackColor="White">
                                        <PanelCollection>
                                            <dxe:panelcontent runat="server">
                                                <div style="width: 100%; text-align: right;">
                                                    <b style="text-align: right" id="B_AcBalance" runat="server"></b>
                                                </div>
                                            </dxe:panelcontent>
                                        </PanelCollection>
                                        <ClientSideEvents EndCallback="function(s, e) {
	                                                    CbpAcBalance_EndCallBack(); }" />
                                    </dxe:ASPxCallbackPanel>
                                <blink><b style="color:Blue;font-size:10px;" id="bDrCrStatus" runat="server"></b></blink>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 91%;display:none;margin-left:20px" id="TblMainEntryForm" border="1">
                            <tr>
                                <td style="font-weight: bold; text-align: left; text-decoration: underline; background-color: #b7ceec; width: 134px;" >
                                    Main Account</td>
                                <td id="tdlblSubAccount" style="font-weight: bold; text-align: left; text-decoration: underline; background-color: #b7ceec; width: 54px;" >
                                    SubAccount</td>
                                <td style="font-weight: bold; text-align: left; text-decoration: underline; background-color: #b7ceec;" >
                                    Debit</td>
                                <td style="font-weight: bold; text-align: left; text-decoration: underline; background-color: #b7ceec; width: 177px;" >
                                    Credit</td>
                            </tr>
                            <tr>
                                <td style="width: 134px" >
                                    <asp:TextBox ID="txtMainAccount" runat="server" Width="200px" onkeyup="CallMainAccount(this,'MainAccountJournal_New',event)" onFocus="this.select()" Font-Size="13px"></asp:TextBox></td>
                                <td id="tdSubAccount" style="width: 54px">
                                    <asp:TextBox ID="txtSubAccount" runat="server" Width="200px" onkeyup="CallSubAccount(this,'SubAccountMod_New',event)" onFocus="this.select()" onblur="AcBalance(this.id)" Font-Size="13px"></asp:TextBox></td>
                                <td >
                                    <%--<dxe:ASPxSpinEdit ID="txtdebit" ClientInstanceName="ctxtdebit" runat="server" NumberType="Float" HorizontalAlign="Right" 
                                    DecimalPlaces="2"  TabIndex="0" NullText="0.00" Number="0.00">
                                        <Paddings PaddingRight="5px" />
                                        <ClientSideEvents LostFocus="function(s,e){focusval(s.GetValue());}" />
                                        <SpinButtons ShowIncrementButtons="False" ></SpinButtons>
                                     </dxe:ASPxSpinEdit>--%>
                                    <dxe:ASPxTextBox ID="txtdebit" runat="server" Width="140px" ClientInstanceName="ctxtdebit" HorizontalAlign="Right" Font-Size="13px" >
                                        <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;"  IncludeLiterals="DecimalSymbol" />
                                        <ValidationSettings ErrorDisplayMode="None">
                                        </ValidationSettings>
                                        <ClientSideEvents KeyUp="function(s,e){focusval(s.GetValue());}" />
                                    </dxe:ASPxTextBox>
                                </td>
                                <td style="width: 177px">
                                 <%--   <dxe:ASPxSpinEdit ID="txtcredit" ClientInstanceName="ctxtcredit" runat="server" NumberType="Float" HorizontalAlign="Right" DecimalPlaces="2" TabIndex="0" NullText="0.00" Number="0.00">
                                        <Paddings PaddingRight="5px" />
                                        <ClientSideEvents LostFocus="function(s,e){focusval1(s.GetValue());}" />
                                        <SpinButtons ShowIncrementButtons="False" ></SpinButtons>
                                     </dxe:ASPxSpinEdit>--%>

                                  <dxe:ASPxTextBox ID="txtcredit" runat="server" Width="145px" ClientInstanceName="ctxtcredit" HorizontalAlign="Right" Font-Size="13px">
                                        <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;"  IncludeLiterals="DecimalSymbol" />
                                        <ValidationSettings ErrorDisplayMode="None">
                                        </ValidationSettings>
                                        <ClientSideEvents KeyUp="function(s,e){focusval1(s.GetValue());}" />
                                  </dxe:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="font-weight: bold; text-align: left; text-decoration: underline; background-color: #b7ceec; width: 134px;" valign="top">
                                    Line Narration</td>
                                    <td style="font-weight: bold; text-align: left; text-decoration: underline;" valign="top" colspan="3">
                                    <asp:TextBox ID="txtNarration1" Font-Names="Arial" runat="server" TextMode="MultiLine"
                                        Width="99%" Font-Size="12px" Height="16px" TabIndex="0"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="height: 16px" colspan="4" valign="top">
                                    <table>
                                        <tr>
                                            <td id="tdadd" style="width: 100px">
                                                <dxe:ASPxButton ID="btnadd" ClientInstanceName="cbtnadd" runat="server" AccessKey="L" AutoPostBack="false" TabIndex="0"
                                                    Text="Add Entry To [L]ist" Width="120px">
                                                    <ClientSideEvents Click="function(s, e) {SubAccountCheck();}" />
                                                </dxe:ASPxButton>
                                            </td>
                                            <td id="tdnew" style="width: 100px; height: 16px; display: none">
                                                <dxe:ASPxButton ID="btnnew" ClientInstanceName="cbtnnew" runat="server" AutoPostBack="false" TabIndex="0" Text="[N]ew Entry"
                                                    Width="150px" AccessKey="N" Font-Bold="False" Font-Underline="False" BackColor="Tan">
                                                    <ClientSideEvents Click="function(s, e) {NewButtonClick();}" />
                                                </dxe:ASPxButton>
                                            </td>
                                            <td style="width: 100px">
                                                <dxe:ASPxButton ID="btnCancelEntry" runat="server" AccessKey="C" AutoPostBack="false"
                                                    TabIndex="0" Text="[C]ancel Entry" Width="120px">
                                                    <ClientSideEvents Click="function(s, e) {CancelButtonClick();}" />
                                                </dxe:ASPxButton>
                                            </td>
                                            <td style="width: 100px;display:none" id="tdSaveButton" runat="Server">
                                                <dxe:ASPxButton ID="btnSaveRecords" ClientInstanceName="cbtnSaveRecords" runat="server" AccessKey="S" AutoPostBack="false"
                                                    TabIndex="0" Text="[S]ave Entered Records" Width="150px">
                                                    <ClientSideEvents Click="function(s, e) {SaveButtonClick();}" />
                                                </dxe:ASPxButton>
                                            </td>
                                            <td style="width: 100px">
                                                <dxe:ASPxButton ID="btnDiscardEntry" runat="server" AccessKey="D" AllowFocus="False"
                                                    AutoPostBack="false" TabIndex="0" Text="[D]iscard Entered Records" Width="150px" Visible="False">
                                                    <ClientSideEvents Click="function(s, e) {DiscardButtonClick();}" />
                                                </dxe:ASPxButton>
                                            </td>
                                            <td style="width: 100px">
                                                <dxe:ASPxButton ID="btnDelVoucher" ClientInstanceName="cbtnDelVoucher" runat="server" AccessKey="V" AllowFocus="False"
                                                    AutoPostBack="false" TabIndex="0" Text="Delete [V]oucher" Width="120px">
                                                    <ClientSideEvents Click="function(s, e) {DelVoucherButtonClick();}" />
                                                </dxe:ASPxButton>
                                            </td>
                                            <td style="width: 100px">
                                                <dxe:ASPxButton ID="btnUnsaveData" runat="server" AccessKey="X" AutoPostBack="false"
                                                    TabIndex="0" Text="E[x]it" Width="120px">
                                                    <ClientSideEvents Click="function(s, e) {ExitButtonClick();}" />
                                                </dxe:ASPxButton></td>
                                            <td style="width: 100px">
                                                <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" BackColor="Navy"
                                                    ClientInstanceName="exp" Font-Bold="False" ForeColor="White" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                                    SelectedIndex="0" ValueType="System.Int32" Width="50px">
                                                    <items>
<dxe:ListEditItem Value="0" Text="Select"></dxe:ListEditItem>
<dxe:ListEditItem Value="1" Text="PDF"></dxe:ListEditItem>
<dxe:ListEditItem Value="2" Text="XLS"></dxe:ListEditItem>
<dxe:ListEditItem Value="3" Text="RTF"></dxe:ListEditItem>
<dxe:ListEditItem Value="4" Text="CSV"></dxe:ListEditItem>
</items>
                                                    <clientsideevents selectedindexchanged="OncmbExportSelectedIndexChanged" />
                                                    <buttonstyle backcolor="#C0C0FF" forecolor="Black"></buttonstyle>
                                                    <itemstyle backcolor="Navy" forecolor="White">
<HoverStyle BackColor="#8080FF" ForeColor="White"></HoverStyle>
</itemstyle>
                                                    <border bordercolor="White" />
                                                    <dropdownbutton text="Export"></dropdownbutton>
                                                </dxe:ASPxComboBox></td>
                                        </tr>
                                    </table>
                                   </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="height: 16px" valign="top">
                                    <table width="100%" border="1">
                                        <tr id="Tdebit" runat="server">
                                            <td style="width: 13017px; text-align: left; height: 14px;" valign="top">
                                            </td>
                                            <td style="width: 75px; text-align: left; height: 14px; background-color: #b7ceec;" valign="top">
                                                Total&nbsp;
                                            </td>
                                            <td style="width: 79px; text-align: left; height: 14px;" valign="top">
                                                <dxe:ASPxTextBox ID="txtTDebit" runat="server" Width="105px" ClientInstanceName="ctxtTDebit" HorizontalAlign="Right" Font-Size="12px">
                                                    <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;"  IncludeLiterals="DecimalSymbol" />
                                                    <ValidationSettings ErrorDisplayMode="None">
                                                    </ValidationSettings>
                                                </dxe:ASPxTextBox>
                                            </td>
                                            <td style="width: 75px; height: 14px; text-align: left; background-color: #b7ceec;" valign="top">
                                                Dr.</td>
                                            <td style="text-align: left; height: 14px;" valign="top">
                                                <dxe:ASPxTextBox ID="txtTCredit" runat="server" Width="105px" ClientInstanceName="ctxtTCredit" HorizontalAlign="Right" Font-Size="12px">
                                                    <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;"  IncludeLiterals="DecimalSymbol" />
                                                    <ValidationSettings ErrorDisplayMode="None">
                                                    </ValidationSettings>
                                                </dxe:ASPxTextBox>
                                            </td>
                                            <td style="height: 14px; text-align: left; width: 75px; background-color: #b7ceec;" valign="top">
                                                Cr.</td>
                                        </tr>
                                    </table>
                        <dxe:ASPxGridView ID="DetailsGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="cDetailsGrid"
                            KeyFieldName="CashReportID" Width="100%" OnCustomCallback="DetailsGrid_CustomCallback" 
                            OnCustomJSProperties="DetailsGrid_CustomJSProperties" OnHtmlRowCreated="DetailsGrid_HtmlRowCreated" 
                            OnRowDeleting="DetailsGrid_RowDeleting" OnHtmlEditFormCreated="DetailsGrid_HtmlEditFormCreated" OnRowUpdating="DetailsGrid_RowUpdating" OnCancelRowEditing="DetailsGrid_CancelRowEditing" 
                            OnHtmlRowPrepared="DetailsGrid_HtmlRowPrepared" Font-Size="12px" OnCommandButtonInitialize="DetailsGrid_CommandButtonInitialize">
                            <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True"></SettingsBehavior>
                               <%-- <Settings ShowVerticalScrollBar="True" />--%>
                            <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" Mode="ShowAllRecords">
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </SettingsPager>
                            <ClientSideEvents EndCallback="function(s, e) {SetDebitCreditValue(s.cpTotalDebitCredit);}"></ClientSideEvents>
                            
                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="MainAccount1" Caption="Main Account" VisibleIndex="0" Width="25%">
                                    <CellStyle Wrap="False" CssClass="gridcellleft">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="SubAccount1" Caption="Sub Account" VisibleIndex="1" Width="40%">
                                    <CellStyle Wrap="False" CssClass="gridcellleft">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="WithDrawl" Caption="Debit" VisibleIndex="2" Width="10%">
                                    <CellStyle Wrap="False" CssClass="gridcellleft" HorizontalAlign="Right">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                 <dxe:GridViewDataTextColumn FieldName="Receipt" Caption="Credit" VisibleIndex="3" Width="10%">
                                    <CellStyle Wrap="False" CssClass="gridcellleft" HorizontalAlign="Right">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="CashReportID" Visible="False">
                                  </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="SubNarration" Visible="False">
                                </dxe:GridViewDataTextColumn>
                             
                                <dxe:GridViewCommandColumn VisibleIndex="4" Width="15%" ShowDeleteButton="True" ShowEditButton="True">
                                    <CellStyle ForeColor="White">
                                        <%-- <HoverStyle BackColor="#000040">
                                                                                </HoverStyle>--%>
                                    </CellStyle>
                                </dxe:GridViewCommandColumn>
                            </Columns>
                            <Templates>
                                <EditForm>
                                    <table>
                                        <tr>
                                            <td style="font-weight: bold;font-size:10px; text-decoration: underline">
                                                Main Account</td>
                                            <td id="tdlblSubAccount" style="font-weight: bold;font-size:10px; text-decoration: underline">
                                                Sub Account</td>
                                            <td style="font-weight: bold;font-size:10px;  text-decoration: underline">
                                                Debit</td>
                                            <td style="font-weight: bold;font-size:10px;  text-decoration: underline">
                                                Credit</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtMainAccountE" runat="server" Font-Size="13px" Width="200px" Text='<%# Bind("MainAccount1") %>' onkeyup="CallMainAccountE(this,'MainAccountJournalE',event)"></asp:TextBox>
                                                <asp:HiddenField ID="txtMainAccountE_hidden" runat="server" />
                                            </td>
                                            <td id="tdSubAccount">
                                                <asp:TextBox ID="txtSubAccountE" runat="server" Width="200px" Font-Size="13px" Text='<%# Bind("SubAccount1") %>' onkeyup="CallSubAccountE(this,'SubAccountModE',event)"></asp:TextBox>
                                                <asp:HiddenField ID="txtSubAccountE_hidden" runat="server" />
                                            </td>
                                            <td>
                                                <dxe:ASPxTextBox ID="ASPxTextBox1" runat="server" Font-Size="13px" Width="140px" ClientInstanceName="ctxtdebitE" Text='<%# Bind("WithDrawl") %>' 
                                                    HorizontalAlign="Right">
                                                    <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                    <ValidationSettings ErrorDisplayMode="None">
                                                    </ValidationSettings>
                                                    <ClientSideEvents LostFocus="function(s,e){Efocusval(s.GetValue());}" />
                                                </dxe:ASPxTextBox>
                                            </td>
                                            <td>
                                                <dxe:ASPxTextBox ID="ASPxTextBox2" runat="server" Width="145px" Font-Size="13px" ClientInstanceName="ctxtcreditE"
                                                    HorizontalAlign="Right" Text='<%# Bind("Receipt") %>' >
                                                    <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                    <ValidationSettings ErrorDisplayMode="None">
                                                    </ValidationSettings>
                                                    <ClientSideEvents LostFocus="function(s,e){Efocusval1(s.GetValue());}" />
                                                </dxe:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="font-weight: bold;font-size:10px;text-decoration: underline; height: 16px;" valign="top">
                                                Line Narration</td>
                                            <td  colspan="3">
                                                <asp:TextBox Font-Size="13px" Font-Names="Arial" ID="TextBox1" runat="server" Text='<%# Bind("SubNarration") %>'
                                                    TextMode="MultiLine" Width="99%" Height="16px" TabIndex="0"></asp:TextBox></td>
                                            <td>
                                                <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                    runat="server">
                                                </dxe:ASPxGridViewTemplateReplacement>
                                            </td>
                                            <td>
                                                <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                    runat="server">
                                                </dxe:ASPxGridViewTemplateReplacement>
                                            </td>
                                        </tr>
                                        
                                    </table>
                                </EditForm>
                            </Templates>
                            <SettingsEditing Mode="EditForm" />
                            <Styles>
                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                </Header>
                                <FocusedGroupRow CssClass="gridselectrow">
                                </FocusedGroupRow>
                                <FocusedRow CssClass="gridselectrow" HorizontalAlign="Left" VerticalAlign="Top">
                                </FocusedRow>
                                <Footer CssClass="gridfooter">
                                </Footer>
                                <LoadingPanel ImageSpacing="10px">
                                </LoadingPanel>
                            </Styles>

                        </dxe:ASPxGridView>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 850px;display:none" id="TblSearch">
                            <tr>
                                <td style="width: 100px; height: 178px">
                                    <dxe:ASPxGridView ID="GvJvSearch" runat="server" AutoGenerateColumns="False"
                                        ClientInstanceName="cGvJvSearch" KeyFieldName="JvID" Width="859px" OnCustomCallback="GvJvSearch_CustomCallback" 
                                        OnCustomJSProperties="GvJvSearch_CustomJSProperties" Font-Size="12px" >
                                        <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
                                        <Settings VerticalScrollableHeight="500" ShowGroupPanel="True" ShowHorizontalScrollBar="True"  />
                                        <SettingsPager NumericButtonCount="20" ShowSeparators="True">
                                            <FirstPageButton Visible="True">
                                            </FirstPageButton>
                                            <LastPageButton Visible="True">
                                            </LastPageButton>
                                        </SettingsPager>
                                        <ClientSideEvents CustomButtonClick="CustomButtonClick" EndCallback="function(s, e) {GvJvSearch_EndCallBack();}" />
                                        <Columns>
                                            <dxe:GridViewDataTextColumn FieldName="JvID" Caption="Main Account" VisibleIndex="0" Visible="False">
                                                <CellStyle Wrap="False" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="VoucherNumber" Caption="Jv Number" VisibleIndex="0"
                                                Width="20%">
                                                <CellStyle Wrap="False" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="BranchNameCode" Caption="Branch" VisibleIndex="1"
                                                Width="20%">
                                                <CellStyle Wrap="False" CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="Narration" Caption="Narration" VisibleIndex="2"
                                                Width="60%">
                                                <CellStyle CssClass="gridcellleft">
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="IBRef" Visible="False" VisibleIndex="4">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="WhichTypeItem" Visible="False" VisibleIndex="5">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewCommandColumn VisibleIndex="3" Width="15%">
                                                <CustomButtons>
                                                    <dxe:GridViewCommandColumnCustomButton ID="CustomBtnEdit" Text="Edit">
                                                    </dxe:GridViewCommandColumnCustomButton>
                                                    <dxe:GridViewCommandColumnCustomButton ID="CustomBtnDelete" Text="Delete">
                                                    </dxe:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                            </dxe:GridViewCommandColumn>
                                        </Columns>
                                       
                                        <Styles>
                                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                            </Header>
                                            <FocusedGroupRow CssClass="gridselectrow">
                                            </FocusedGroupRow>
                                            <FocusedRow CssClass="gridselectrow" HorizontalAlign="Left" VerticalAlign="Top">
                                            </FocusedRow>
                                            <Footer CssClass="gridfooter">
                                            </Footer>
                                            <LoadingPanel ImageSpacing="10px">
                                            </LoadingPanel>
                                        </Styles>
                                    </dxe:ASPxGridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td id='Div1' style="height: 20px; display:none">
                        <div  style='position: absolute; font-family: arial; font-size: 30; left: 50%;
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
                                                            <font size='2' face='Tahoma'><strong align='center'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Loading..</strong></font></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                    </td>
                </tr>
              
            </table>
             <dxe:ASPxPopupControl ID="SearchPopUp" runat="server" HeaderText="Filter" ClientInstanceName="cSearchPopUp">
                            <ContentCollection>
                                <dxe:PopupControlContentControl runat="server">
                                    <table class="style1">
                                        <tr>
                                            <td style="width: 3px">
                                                <dxe:aspxcheckbox  id="ChkTranDate" runat="server" text="Tran.Date"  
                                            ClientInstanceName="cChkTranDate" Checked="True" ReadOnly="True">
                                                </dxe:aspxcheckbox>
                                            </td>
                                            <td>
                                            <dxe:aspxcheckbox  id="ChkBranch" runat="server" text="Branch" 
                                            ClientInstanceName="cChkBranch" ></dxe:aspxcheckbox >
                                             </td>
                                            <td><dxe:aspxcheckbox  id="ChkBillNo" runat="server" text="BillNo" 
                                            ClientInstanceName="cChkBillNo">
                                            </dxe:aspxcheckbox>
                                            </td>
                                            <td style="width: 3px">
                                            <dxe:aspxcheckbox  id="ChkPrefix" runat="server" text="Prefix" 
                                            ClientInstanceName="cChkPrefix">
                                             </dxe:aspxcheckbox>
                                            </td>
                                            <td style="width: 3px">
                                                <dxe:aspxcheckbox  id="ChkNarration" runat="server" text="Narration" 
                                            ClientInstanceName="cChkNarration">
                                                </dxe:aspxcheckbox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 3px">
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td style="width: 3px">
                                                <dxe:ASPxButton ID="btnShow" runat="server" AutoPostBack="False" Text="Show" Width="65px">
                                                    <ClientSideEvents Click="function (s, e) { btnShowClick(); ExcludePopUp.Hide(); }" />
                                                </dxe:ASPxButton>
                                            </td>
                                            <td style="width: 3px">
                                                <dxe:ASPxButton ID="btnCancel" runat="server" AutoPostBack="False" Text="Cancel" Width="62px">
                                                    <ClientSideEvents Click="function (s, e) { cSearchPopUp.Hide(); }" />
                                                </dxe:ASPxButton>
                                            </td>
                                        </tr>
                                    </table>
                                </dxe:PopupControlContentControl>
                            </ContentCollection>
                        </dxe:ASPxPopupControl>
              
               <dxe:ASPxPopupControl ID="MsgPopUp" runat="server" HeaderText="Notice" ClientInstanceName="cMsgPopUp" Width="387px" Left="200" ShowSizeGrip="False" Top="200" CloseAction="None" Modal="True">
                            <ContentCollection>
                                <dxe:PopupControlContentControl runat="server">
                                    <table class="style1">
                                        <tr>
                                            <td colspan="5">
                                                &nbsp;Do You Want To Edit This&nbsp; Entry?</td>
                                        </tr>
                                        <tr>
                                            <td style="width: 3px">
                                            </td>
                                            <td>
                                            </td>
                                            <td style="width: 164px">
                                            </td>
                                            <td style="width: 3px">
                                                <dxe:ASPxButton ID="abtnOk" runat="server" AutoPostBack="False" Text="Ok">
                                                    <ClientSideEvents Click="function (s, e) { btnOkClick(); cMsgPopUp.Hide(); }" />
                                                </dxe:ASPxButton>
                                            </td>
                                            <td style="width: 3px">
                                                <dxe:ASPxButton ID="abtnCancel" runat="server" AutoPostBack="False" Text="Cancel">
                                                    <ClientSideEvents Click="function (s, e) { cMsgPopUp.Hide();parent.editwin.close(); }" />
                                                </dxe:ASPxButton>
                                            </td>
                                        </tr>
                                    </table>
                                </dxe:PopupControlContentControl>
                            </ContentCollection>
                        </dxe:ASPxPopupControl>
                
                <dxe:ASPxPopupControl ID="DeleteMsgPopUp" runat="server" HeaderText="Notice" ClientInstanceName="cDeleteMsgPopUp" Width="387px" Left="200" ShowSizeGrip="False" Top="200">
                            <ContentCollection>
                                <dxe:PopupControlContentControl runat="server">
                                    <table class="style1">
                                        <tr>
                                            <td colspan="5">
                                                Are u Sure? Do You Want To Delete This JV?</td>
                                        </tr>
                                        <tr>
                                            <td style="width: 3px">
                                            </td>
                                            <td>
                                            </td>
                                            <td style="width: 164px">
                                            </td>
                                            <td style="width: 3px">
                                                <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" Text="Ok">
                                                    <ClientSideEvents Click="function (s, e) { DeletebtnOkClick(); cDeleteMsgPopUp.Hide(); }" />
                                                </dxe:ASPxButton>
                                            </td>
                                            <td style="width: 3px">
                                                <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" Text="Cancel">
                                                    <ClientSideEvents Click="function (s, e) { cDeleteMsgPopUp.Hide(); }" />
                                                </dxe:ASPxButton>
                                            </td>
                                        </tr>
                                    </table>
                                </dxe:PopupControlContentControl>
                            </ContentCollection>
                        </dxe:ASPxPopupControl>
                        
                         <dxe:ASPxPopupControl ID="FileUsedByPopUp" runat="server" HeaderText="Notice" ClientInstanceName="cFileUsedByPopUp" Width="387px" Left="200" ShowSizeGrip="False" Top="200">
                            <ContentCollection>
                                <dxe:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                                    <table class="style1">
                                        <tr>
                                            <td colspan="5">
                                                This Entry Was Already Being Edited By You.</td>
                                        </tr>
                                        <tr>
                                           
                                            <td>
                                            </td>
                                            <td style="width: 3px">
                                                <dxe:ASPxButton ID="btnContinue" runat="server" AutoPostBack="False" Text="Continue Previous Edit" Width="140px">
                                                    <ClientSideEvents Click="function (s, e) { btnContinueClick(); cFileUsedByPopUp.Hide(); }" />
                                                </dxe:ASPxButton>
                                            </td>
                                            <td style="width: 3px">
                                                <dxe:ASPxButton ID="btnNewEntry" runat="server" AutoPostBack="False" Text="Fresh Edit" Width="140px">
                                                    <ClientSideEvents Click="function (s, e) { btnFreshEntryClick();cFileUsedByPopUp.Hide(); }" />
                                                </dxe:ASPxButton>
                                            </td>
                                            <td style="width: 3px">
                                                <dxe:ASPxButton ID="btnClose" runat="server" AutoPostBack="False" Text="Discard Previous Edit" Width="140px">
                                                    <ClientSideEvents Click="function (s, e) {btnCloseClick(); cFileUsedByPopUp.Hide(); }" />
                                                </dxe:ASPxButton>
                                            </td>
                                             <td style="width: 3px">
                                                <dxe:ASPxButton ID="btnCancle" runat="server" AutoPostBack="False" Text="Cancel" Width="140px">
                                                    <ClientSideEvents Click="function (s, e) { cFileUsedByPopUp.Hide(); }" />
                                                </dxe:ASPxButton>
                                            </td>
                                        </tr>
                                    </table>
                                </dxe:PopupControlContentControl>
                            </ContentCollection>
                        </dxe:ASPxPopupControl>
            <asp:SqlDataSource ID="dsCompany" runat="server" 
                ConflictDetection="CompareAllValues" SelectCommand="select cmp_internalId,cmp_Name from tbl_master_company where cmp_internalId in(select exch_compId from tbl_master_companyExchange)">
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="dsBranch" runat="server" 
                ConflictDetection="CompareAllValues" SelectCommand="SELECT BRANCH_id AS BANKBRANCH_ID , RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH">
            </asp:SqlDataSource>
                        <asp:HiddenField ID="hddnEdit" runat="server" />
                         <asp:HiddenField ID="hdnSegmentid" runat="server" />
                         <asp:HiddenField ID="hdnCompanyid" runat="server" />
                        <asp:HiddenField ID="txtMainAccount_hidden" runat="server" />
                        <asp:HiddenField ID="txtSubAccount_hidden" runat="server" />
            <asp:HiddenField ID="hdn_Brch_NonBrch" runat="server" />
            <asp:HiddenField ID="hdn_MainAcc_Type" runat="server" />
            <div style="display: none">
                <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" />
                <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                </dxe:ASPxGridViewExporter>
            </div>
        </div>
</asp:Content>