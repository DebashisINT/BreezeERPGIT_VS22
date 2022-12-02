<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_dailymrgnrprttoclient" Codebehind="dailymrgnrprttoclient.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxCallbackPanel"
    TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>--%>


   
	 
	
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
    .frmleftCont{float:left; margin:2px; padding:2px; height:26px; border: solid 1px  #D1E0F3; font-size:12px;}
        </style>
    <script language="javascript" type="text/javascript">  
    
    if (typeof String.prototype.startsWith != 'function') {
      // see below for better implementation!
      String.prototype.startsWith = function (str){
        return this.indexOf(str) == 0;
      };
    }
    
    function Page_Load()///Call Into Page Load
    {
         Hide('showFilter');
         Show('td_btnshow');
         Hide('td_btnprint');
         Hide('Tr_DigitalSign');
         Hide('td_btnemail');
         Hide('tr_filter');
         //Hide('Tr_DigitalSign');
         Hide('trshow');
         Hide('ecndetail');
         document.getElementById('hiddencount').value=0;
         Hide('tr_printlogo');
         height();
    }
    function Reload()
    {        
         Hide('showFilter');
         Hide('td_btnshow');
         Hide('td_btnprint');
         Hide('Tr_DigitalSign');
         Hide('td_btnemail');
         Hide('tr_filter');
         Hide('tr_printlogo');
         alert('Please Select Signature !!');
         window.location = "../reports/dailymrgnrprttoclient.aspx"
    }
    function keyVal(obj)
     {
       var WhichCall=obj.split("~")[4];
       if(WhichCall=="DIGISIGN")
       {
        var isEtoken=obj.split("~")[2];
        if(isEtoken=="E")
        {
            HideShow("tr_openpopup","H");
            HideShow("Tr_GeneratePdf","S");
        }
        else
        {
            HideShow("tr_openpopup","S");
            HideShow("Tr_GeneratePdf","H");
        }
       }
     } 
     function fn_GenearatePDF()
       {
        cBtnGeneratePdf.SetEnabled(false);
        cBtnGeneratePdf.SetText("Please Wait..");
        cCbpSuggestISIN.PerformCallback('GeneratePDF');
       }           
    function mailsendwithreload(obj)
    {
         Hide('showFilter');
         Hide('td_btnshow');
         Hide('td_btnprint');
         Hide('Tr_DigitalSign');
         Show('td_btnemail');
         Hide('tr_filter');
         Hide('tr_printlogo');
         alert(obj);
         if(obj=='1')
               alert('Mail Sent Successfully!!');
         if(obj=='2')
               alert('Mail Sent Successfully!!!'+'\n'+'Certain Email id could not found!!!');
         if(obj=='3')
               alert('Error on sending!Try again..');
         if(obj=='4')
               alert('PDF Genrated Sucessfully.Please Signed PDF Now..');
               
         height();   
    }        
    function height()
    {
        if(document.body.scrollHeight>=500)
        {
            window.frameElement.height = document.body.scrollHeight;
        }
        else
        {
            window.frameElement.height = '500px';
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
    function FunClientScrip(objID,objListFun,objEvent)
    {
        var cmbVal;         
        if(document.getElementById('cmbsearchOption').value=="Clients")
        {
            if(document.getElementById('ddlGroup').value=="0" || document.getElementById('ddlGroup').value=="2")//////////////Group By  selected are branch
            {
                if(document.getElementById('ddlGroup').value=="0")
                {
                   if(document.getElementById('rdbranchAll').checked==true)
                   {
                       cmbVal='ClientsBranch'+'~'+'ALL';
                   }
                   else
                   {
                       cmbVal='ClientsBranch'+'~'+'Selected'+'~'+document.getElementById('HiddenField_Branch').value;
                   }
                }
                if(document.getElementById('ddlGroup').value=="2")
                {
                   if(document.getElementById('rdbranchAll').checked==true)
                   {
                       cmbVal='ClientsBranchGroup'+'~'+'ALL';
                   }
                   else
                   {
                       cmbVal='ClientsBranchGroup'+'~'+'Selected'+'~'+document.getElementById('HiddenField_BranchGroup').value;
                   }
                }
            }
            else //////////////Group By selected are Group
            {
               if(document.getElementById('rdddlgrouptypeAll').checked==true)
               {
                   cmbVal='ClientsGroup'+'~'+'ALL'+'~'+document.getElementById('ddlgrouptype').value;
               }
               else
               {
                   cmbVal='ClientsGroup'+'~'+'Selected'+'~'+document.getElementById('HiddenField_Group').value;
               }
            }
        }
        else
        {
            cmbVal=document.getElementById('cmbsearchOption').value;
            cmbVal=cmbVal+'~'+document.getElementById('ddlgrouptype').value;
        }          
        ajax_showOptions(objID,objListFun,objEvent,cmbVal);
    }
    function Segment(obj)
    {
         if(obj=="a")
            Hide('showFilter');
         else if(obj=="b")
            Hide('showFilter');                
         else
         {
            var cmb=document.getElementById('cmbsearchOption');
            cmb.value='Segment';
            Show('showFilter');
         }
         selecttion();
         height();
    }
    function Clients(obj)
    {
         if(obj=="a")
            Hide('showFilter');
         else
         {
              var cmb=document.getElementById('cmbsearchOption');
              cmb.value='Clients';
              Show('showFilter');
         }
         selecttion();
          height();
    }
    function Branch(obj)
    {
          if(obj=="a")
              Hide('showFilter');
           else
           {
                if(document.getElementById('ddlGroup').value=="0")
                 {
                  document.getElementById('cmbsearchOption').value='Branch';
                 }
                 if(document.getElementById('ddlGroup').value=="2")
                 {
                  document.getElementById('cmbsearchOption').value='BranchGroup';
                 }
                Show('showFilter');
           }
           selecttion();
              height();
    }
    function Group(obj)
    {
        if(obj=="a")
            Hide('showFilter');
         else
         {
              var cmb=document.getElementById('cmbsearchOption');
              cmb.value='Group';
              Show('showFilter');
         }
         selecttion();
         height();
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
    function  fnddlGroup(obj)
    {
        if(obj=="0" || obj=="2")
        {
            Hide('td_group');
            Show('td_branch');
        }
        else
        {
            Show('td_group');
            Hide('td_branch');
            var btn = document.getElementById('btnhide');
            btn.click();
        }
        selecttion();
         height();
    }
    function fngrouptype(obj)
    {
       if(obj=="0")
       {
       Hide('td_allselect');
        alert('Please Select Group Type !');
       }
       else
       {
       Show('td_allselect');
       }
       selecttion();
       height();
    }   
    function RBShowHide(obj)
    {
        if(obj=='rbPrint')
            {
                Hide('td_btnshow');
                Show('td_btnprint');
                Show('tr_printlogo');
                Hide('td_btnemail');
                Hide('Tr_DigitalSign');
                Hide('trshow');
                Hide('ecndetail');
            }
//        else
           if(obj=='rbScreen')
            {
                Show('td_btnshow');
                Hide('td_btnprint');
                Hide('tr_printlogo');
                Hide('td_btnemail');
                Hide('Tr_DigitalSign');
                Hide('trshow');
                Hide('ecndetail');
            }
            if(obj=='rdbmail')
            {
                Hide('td_btnshow');
                Hide('td_btnprint');
                Hide('tr_printlogo');
//                Show('td_btnemail');trshow
                Hide('Tr_DigitalSign');
                Show('trshow');
                Hide('ecndetail');
            }
            height();
            selecttion();
    } 
        
 
  function NORECORD()
  {
      Hide('tr_filter');
      Hide('displayAll');
      Show('tab2');
      Hide('showFilter');
      alert('No Record Found!!');
      height();
  }
  function MAILSEND(obj)
  {
      Hide('tr_filter');
      Hide('displayAll');
      Show('tab2');
      Hide('showFilter');
      if(obj=='1')
        alert('Mail Sent Successfully!!');
      if(obj=='2')
        alert('Mail Sent Successfully!!!'+'\n'+'Certain Email id could not found!!!');
      if(obj=='3')
        alert('Error on sending!Try again..');
      height();
  }
  function Display()
  {
      Show('tr_filter');
      Show('displayAll');
      Hide('tab2');
      Hide('showFilter');
      height();
  }
  function selecttion()
  {
     var combo=document.getElementById('cmbExport');
     combo.value='Ex';
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
     function Filter()
     {
          Hide('tr_filter');
          Hide('displayAll');
          Show('tab2');
          Hide('showFilter');          
          height();
     } 
//     function CallAjax(obj1,obj2,obj3)
//     { 
//        ajax_showOptions(obj1,obj2,obj3);
//     }
    
   FieldName='lstSlection';
   ////////newly develop through devexpress for ecn
   function fn_show()
   {
    //Show('Tr_DigitalSign');
    cCbptxtSelectionID.PerformCallback('v4');     
   }
    function CbptxtSelectionID_EndCallBack()
     {
         //Hide('Tr_DigitalSign');
          if (cCbptxtSelectionID.cpproperties=="Exportall")
                document.getElementById('BtnForExportEvent').click();
         if (cCbptxtSelectionID.cpproperties=="Export")
                document.getElementById('BtnForExportEvent1').click();
          if (cCbptxtSelectionID.cpproperties=="Exportdelivery")
                document.getElementById('BtnForExportEvent2').click();
         
        if((cCbptxtSelectionID.cpallcontract != null) && (cCbptxtSelectionID.cpecnenable != null) && (cCbptxtSelectionID.cpdeliveryrpt != null))
            {
                document.getElementById('<%=B_allcontract.ClientID %>').innerHTML=cCbptxtSelectionID.cpallcontract;
                document.getElementById('<%=B_ecnenable.ClientID %>').innerHTML=cCbptxtSelectionID.cpecnenable;
                document.getElementById('<%=B_deliveryrpt.ClientID %>').innerHTML=cCbptxtSelectionID.cpdeliveryrpt;;
                
                Show('ecndetail');
                if(cCbptxtSelectionID.cpecnenable=='0')
                    {
                        Hide('Tr_DigitalSign');
                        btnopenpopup.SetEnabled(false);
                    }
                else
                    {
                        Show('Tr_DigitalSign');
                        btnopenpopup.SetEnabled(true);
                    }
                    
            }
        if ((cCbptxtSelectionID.cpallcontractpop != null) && (cCbptxtSelectionID.cpecnenablepop != null))
            {
                document.getElementById('<%=B_allcontractpop.ClientID %>').innerHTML=cCbptxtSelectionID.cpallcontractpop;
                document.getElementById('<%=B_ecnenablepop.ClientID %>').innerHTML=cCbptxtSelectionID.cpecnenablepop;
                var remn=document.getElementById('<%=B_allcontractpop.ClientID %>').innerHTML=cCbptxtSelectionID.cpallcontractpop;
                var remn1=document.getElementById('<%=B_ecnenablepop.ClientID %>').innerHTML=cCbptxtSelectionID.cpecnenablepop;
                if (remn=='0' || remn1=='0')
                {
                    
                    Hide('btnremain');
                }
                else
                {
                    Show('btnremain');
                }
                Hide('btnokdiv');
                Hide('div_fail');
                Hide('div_success');
                cPopUp_ScripAlert.Show();
            
            }
            if ((cCbptxtSelectionID.cpvisibletrue =="yes")|| (cCbptxtSelectionID.cpecnenable=='0'))
            btnopenpopup.SetEnabled(false);
            if ((cCbptxtSelectionID.cpvisibletrue =="no")&& (cCbptxtSelectionID.cpecnenable!='0'))
            btnopenpopup.SetEnabled(true);
//            if (cCbptxtSelectionID.cpallcontractpop != null)
//                alert('You dont have any record to Export !!');
     }
      function fn_showpopup()
        {
            if(document.getElementById('txtdigitalName_hidden').value!="")
                cCbptxtSelectionID.PerformCallback('v5');
             else
                 alert('Please select Signature !!');
        
        }
      function btnsendall_Click()
        {
            cCbpSuggestISIN.PerformCallback('all');
        }
        function btnsendremaining_Click()
        {
            cCbpSuggestISIN.PerformCallback('remain');
        }
    function CbpSuggestISIN_EndCallBack()
    {
        if ((cCbpSuggestISIN.cpallcontractpops != null) && (cCbpSuggestISIN.cpecnenablepops != null))
            {
                
                document.getElementById('<%=B_allcontractpop.ClientID %>').innerHTML=cCbpSuggestISIN.cpallcontractpops;
                document.getElementById('<%=B_ecnenablepop.ClientID %>').innerHTML=cCbpSuggestISIN.cpecnenablepops;
                var remn2=cCbpSuggestISIN.cpallcontractpops;
                var remn3=cCbpSuggestISIN.cpecnenablepops;
                if (remn2=='0' || remn3=='0')
               {                   
                    Hide('btnremain');
                    Hide('btnall');
                    Hide('cancel');

                    
                }
                else
                {
                Show('btnremain');
                Hide('btnall');
                Hide('cancel');

                   
                }
                if (remn3!='0')
                //if('<%=Session["Error"]%>'=='err')
                {
                    Show('div_fail');
                    Hide('div_success');
               
                }
                else
                {
                    Hide('div_fail');
                    Show('div_success');
                      
                }
                 Show('btnokdiv');
                 
                
            }
            if(cCbpSuggestISIN.cpNoPDFGenerated != null)
            {
                alert("No of PDF Successfully Generated : "+cCbpSuggestISIN.cpNoPDFGenerated);
                cCbpSuggestISIN.cpNoPDFGenerated=null;
                cBtnGeneratePdf.SetEnabled(true);
                cBtnGeneratePdf.SetText("Generate PDF");
            }
    }
    function btnok_Click()  
    {
        window.location='../reports/dailymrgnrprttoclient.aspx';
    }
    function btncancel_Click()
    {

        cPopUp_ScripAlert.Hide();
    }      
    function ShowHideFilter(obj)
     {
        //alert('1111');
//         document.getElementById('<%=B_allcontract.ClientID %>').innerHTML=cCbptxtSelectionID.cpallcontract;
//                document.getElementById('<%=B_ecnenable.ClientID %>').innerHTML=cCbptxtSelectionID.cpecnenable;
//                document.getElementById('<%=B_deliveryrpt.ClientID %>').innerHTML=cCbptxtSelectionID.cpdeliveryrpt;;
                
//        alert('<%=Session["allcontract_excel"]%>');
        if (obj=='v1')
            {
            
            
                if(document.getElementById('<%=B_allcontract.ClientID %>').innerHTML=='0')
                 alert('No Record To Export !!');
                else
                 cCbptxtSelectionID.PerformCallback(obj);
            }
        if (obj=='v2')
            {
                if(document.getElementById('<%=B_ecnenable.ClientID %>').innerHTML=='0')
                 alert('No Record To Export !!');
                else
                 cCbptxtSelectionID.PerformCallback(obj);
            }
        if (obj=='v3')
            {
                
                if(document.getElementById('<%=B_deliveryrpt.ClientID %>').innerHTML=='0')
                 alert('No Record To Export !!');
                else
                 cCbptxtSelectionID.PerformCallback(obj);
            }
//   
     }
   /////////////////////////////////////
    </script>

    <script type="text/ecmascript">   
       function ReceiveServerData(rValue)
        {               
                var j=rValue.split('~');
                var btn = document.getElementById('btnhide');
                if(j[0]=='Segment')
                {
                    document.getElementById('HiddenField_Segment').value = j[1];
                    //alert(j[1]);
                }               
                if(j[0]=='Branch')
                {
                    document.getElementById('HiddenField_Branch').value = j[1];
                }
                if(j[0]=='Group')
                {
                    document.getElementById('HiddenField_Group').value = j[1];
                    btn.click();
                }  
                if(j[0]=='Clients')
                {
                    document.getElementById('HiddenField_Client').value = j[1];
                } 
                if(j[0]=='BranchGroup')
                {
                    document.getElementById('HiddenField_BranchGroup').value = j[1];
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
                        <strong><span id="SpanHeader" style="color: #000099">Daily Margin Reporting To Clients</span></strong>
                    </td>
                    <td class="EHEADER" width="15%" id="tr_filter" style="height: 22px">
                        <a href="javascript:void(0);" onclick="Filter();"><span style="color: Blue; text-decoration: underline;
                            font-size: 8pt; font-weight: bold">Filter</span></a> ||
                        <asp:Button ID="btnmailsend" runat="server" Text="Send Mail" CssClass="btnUpdate"
                            Height="20px" OnClick="btnmailsend_Click" />
                    </td>
                </tr>
            </table>
            <table id="tab2" border="0" cellpadding="0" cellspacing="0">
                <tr valign="top">
                    <td>
                        <table>
                            <tr>
                                <td colspan="5" class="gridcellleft" style="height: 154px">
                                    <table  cellpadding="1" cellspacing="1" class="tableClass">
                                        <tr>
                                            <td bgcolor="#b7ceec" class="gridcellleft">
                                                Date :
                                            </td>
                                            <td>
                                                <dxe:ASPxDateEdit ID="dtfor" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                    Font-Size="12px" Width="108px" ClientInstanceName="dtfor">
                                                    <DropDownButton Text="For">
                                                    </DropDownButton>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                            <td colspan="3">
                                            </td>
                                            <td colspan="2" style="display: none">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Segment :</td>
                                            <td>
                                                <asp:RadioButton ID="rdbSegmentAll" runat="server" GroupName="a" onclick="Segment('a')" />All
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdbSegmentSpecific" runat="server" Checked="True" GroupName="a"
                                                    onclick="Segment('b')" />Current [<span id="litSegmentMain" runat="server" style="color: Maroon">
                                                    </span>]
                                            </td>
                                            <td colspan="3">
                                                <asp:RadioButton ID="rdbSegmentSelect" runat="server" GroupName="a" onclick="Segment('c')" />Select
                                            </td>                                            
                                        </tr>
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Group By</td>
                                            <td>
                                                <asp:DropDownList ID="ddlGroup" runat="server" Width="120px" Font-Size="12px" onchange="fnddlGroup(this.value)">
                                                    <asp:ListItem Value="0">Branch</asp:ListItem>
                                                    <asp:ListItem Value="1">Group</asp:ListItem>
                                                    <asp:ListItem Value="2">Branch Group</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td colspan="2" id="td_branch">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="rdbranchAll" runat="server" Checked="True" GroupName="b" onclick="Branch('a')" />
                                                            All
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbranchSelected" runat="server" GroupName="b" onclick="Branch('b')" />Selected
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td id="td_group" style="display: none;" colspan="2">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList ID="ddlgrouptype" runat="server" Font-Size="12px" onchange="fngrouptype(this.value)">
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnhide" EventName="Click"></asp:AsyncPostBackTrigger>
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td id="td_allselect" style="display: none;">
                                                            <asp:RadioButton ID="rdddlgrouptypeAll" runat="server" Checked="True" GroupName="c"
                                                                onclick="Group('a')" />
                                                            All
                                                            <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="c" onclick="Group('b')" />Selected
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                                Clients :</td>
                                            <td>
                                                <asp:RadioButton ID="rdbClientALL" runat="server" Checked="True" GroupName="d" onclick="Clients('a')" />
                                                All Client
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="radPOAClient" runat="server" GroupName="d" onclick="Clients('a')" />POA
                                                Client
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdbClientSelected" runat="server" GroupName="d" onclick="Clients('b')" />
                                                Selected Client
                                            </td>
                                        </tr>
                                        <tr>
                                             <td  class="gridcellleft" bgcolor="#B7CEEC">                               
                                                Applicable Haircut Rate
                                             </td>
                                             <td>
                                                <asp:RadioButton ID="rdbVarmarginElm" runat="server" Checked="True" GroupName="elm"/>
                                                Var Margin+ELM
                                             </td>
                                             <td>
                                                <asp:RadioButton ID="rdbVarmargin" runat="server" GroupName="elm" />Var Margin
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="gridcellleft" colspan="4">
                                                <table>
                                                    <tr>
                                                        <td class="gridcellleft">
                                                            <asp:CheckBox ID="CHKAPPMRGN" runat="server" Checked="true" />
                                                            Consider Only Clients Having Applicabel Margin As On T/T-1 Day</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="gridcellleft">
                                                            <asp:CheckBox ID="CHKDETAIL" runat="server" Checked="true" />
                                                            Show Holding Details</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5">
                                                <table>
                                                    <tr>
                                                        <td id="Td3" class="gridcellleft" bgcolor="#B7CEEC">
                                                            <asp:RadioButton ID="rbScreen" runat="server" GroupName="f" Checked="True" onclick="RBShowHide(this.value)" />Screen
                                                        </td>
                                                        <td id="Td4" class="gridcellleft" bgcolor="#B7CEEC">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:RadioButton ID="rbPrint" runat="server" GroupName="f" onclick="RBShowHide(this.value)" />Print</td>
                                                                    <td id="tr_printlogo">
                                                                        <asp:CheckBox ID="CHKLOGOPRINT" runat="server" Checked="true" />
                                                                        Print Logo</td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td id="td_mail" class="gridcellleft" bgcolor="#B7CEEC">
                                                            <asp:RadioButton ID="rdbmail" runat="Server" GroupName="f" onclick="RBShowHide(this.value)" />Send
                                                            Email
                                                        </td>
                                                    </tr>
                                                    <%--<tr id="Tr_DigitalSign">
                                                        <td colspan="2">
                                                            <table border="10" cellpadding="1" cellspacing="1">
                                                                <tr>
                                                                    <td class="gridcellleft" bgcolor="#B7CEEC">
                                                                        Select Signature :
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtdigitalName" runat="server" Width="250px" onkeyup="FunCallAjaxList(this,event,'Digital')"></asp:TextBox>
                                                                        <asp:TextBox ID="txtdigitalName_hidden" runat="server" TabIndex="11" Width="100px"
                                                                            Style="display: none"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="Validatetosignature" runat="server" ControlToValidate="txtdigitalName"
                                                                            ErrorMessage="Required." ValidationGroup="a"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td id="td_msg" runat="server">
                                                                        <asp:Label ID="Location" runat="server" Text="You Dont have Permission.Please Contact to Administrator"
                                                                            ForeColor="red" Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>--%>
                                                    <tr>
                                                        <td style="height: 22px" id="td_btnshow" class="gridcellleft">
                                                            <asp:Button ID="btnshow" runat="server" CssClass="btnUpdate" Height="20px" Text="Show"
                                                                OnClientClick="selecttion()" Width="101px" OnClick="btnshow_Click" />
                                                        </td>
                                                        <td style="height: 22px" id="td_btnprint">
                                                            <asp:Button ID="btnprint" runat="server" CssClass="btnUpdate" Height="20px" Text="Print"
                                                                OnClientClick="selecttion()" Width="101px" OnClick="btnprint_Click" />
                                                        </td>
                                                        <td style="height: 22px" id="td_btnemail" class="gridcellleft">
                                                            <asp:Button ID="btnemail" runat="server" CssClass="btnUpdate" Height="20px" Text="Send Email"
                                                                ValidationGroup="a" Width="101px" OnClick="btnemail_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr id="trshow">
                                            <td class="gridcellleft">
                                                <dxe:ASPxButton ID="btnshowecn" runat="server" CssClass="btnUpdate" AutoPostBack="False"
                                                    Height="5px" Text="Show" Width="101px">
                                                    <ClientSideEvents Click="function (s, e) {fn_show();}" />
                                                </dxe:ASPxButton>
                                            </td>
                                             <td>
                                                <span id="spanall">
                                                    <dxe:ASPxCallbackPanel ID="Cexcelexportpanel" runat="server" ClientInstanceName="cCbptxtSelectionID"
                                                        OnCallback="Cexcelexportpanel_Callback" Width="206px">
                                                        <PanelCollection>
                                                            <dxe:panelcontent runat="server">
                                                            </dxe:panelcontent>
                                                        </PanelCollection>
                                                        <ClientSideEvents EndCallback="CbptxtSelectionID_EndCallBack" />
                                                    </dxe:ASPxCallbackPanel>
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                    
                                    <div style="margin-left: 10px;">
                                        <table class="gridcellleft" cellpadding="0" cellspacing="0"  id="ecndetail"
                                            style="padding: 2px;">
                                            <tr style="background-color: lavender; text-align: left">
                                                <td colspan="5">
                                                    <b>Daily Margin Related Detail </b>
                                                </td>
                                            </tr>
                                            <tr style="background-color: #DBEEF3;">
                                                <%--<td colspan="2">
                                                    <b>Total Client</b>
                                                </td>--%>
                                                <td colspan="2">
                                                    <b>Report Generating Client</b>
                                                </td>
                                                <td colspan="2">
                                                    <b>Email Enable Report Generating Client</b>
                                                </td>
                                                <td colspan="2">
                                                    <b>Delivered Client</b>
                                                </td>
                                            </tr>
                                            <tr style="background-color: white;">
                                                <td>
                                                    <%--<b>
                                        <%= allcontract%>
                                    </b>--%>
                                                    <b style="text-align: right" id="B_allcontract" runat="server"></b>
                                                </td>
                                                <td>
                                                    <a href="javascript:ShowHideFilter('v1');"><span style="color: Blue; text-decoration: underline;
                                                        vertical-align: bottom; font-size: 12px">View Detail</span></a>
                                                </td>
                                                <td>
                                                    <%-- <b>
                                        <%=ecnenable %>
                                    </b>--%>
                                                    <b style="text-align: right" id="B_ecnenable" runat="server"></b>
                                                </td>
                                                <td>
                                                    <a href="javascript:ShowHideFilter('v2');"><span style="color: Blue; text-decoration: underline;
                                                        vertical-align: bottom; font-size: 12px">View Detail</span></a>
                                                </td>
                                                <td>
                                                    <%--<b>
                                        <%=deliveryrpt %>
                                    </b>--%>
                                                    <b style="text-align: right" id="B_deliveryrpt" runat="server"></b>
                                                </td>
                                                <td>
                                                    <a href="javascript:ShowHideFilter('v3');"><span style="color: Blue; text-decoration: underline;
                                                        vertical-align: bottom; font-size: 12px">View Detail</span></a>
                                                </td>
                                            </tr>
                                            <%--<tr id="tr_openpopup">
                                <td class="gridcellleft" colspan="5">
                                    <dxe:ASPxButton ID="btnopenpopup" runat="server" CssClass="btnUpdate" AutoPostBack="False"
                                        Height="5px" Text="Generate" Width="101px">
                                        <clientsideevents click="function (s, e) {fn_showpopup();}" />
                                    </dxe:ASPxButton>
                                </td>
                            </tr>--%>
                                        </table>
                                    </div>
                                    <table>
                                        <tr id="Tr_DigitalSign">
                                            <td colspan="2">
                                                <table  cellpadding="1" cellspacing="1" >
                                                    <tr>
                                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                                            Select Signature :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtdigitalName" runat="server" Width="250px"></asp:TextBox>
                                                            <asp:TextBox ID="txtdigitalName_hidden" runat="server" TabIndex="11" Width="100px"
                                                                Style="display: none"></asp:TextBox>
                                                        </td>
                                                        <td id="td_msg" runat="server">
                                                            <asp:Label ID="Location" runat="server" Text="You Dont have Permission to send/ Contact to Administrator"
                                                                ForeColor="red" Font-Bold="true"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr id="tr_openpopup">
                                                        <td class="gridcellleft" style="text-align: left;">
                                                            <dxe:ASPxButton ID="btnopenpopup" runat="server" CssClass="btnUpdate" AutoPostBack="False" Height="5px" Text="Generate" Width="101px">
                                                                <clientsideevents click="function (s, e) {fn_showpopup();}" />
                                                            </dxe:ASPxButton>
                                                        </td>
                                                    </tr>
                                                    <tr id="Tr_GeneratePdf" style="display:none">
                                                        <td>
                                                            <dxe:ASPxButton ID="BtnGeneratePdf" ClientInstanceName="cBtnGeneratePdf" runat="server" AutoPostBack="False" CssClass="btnUpdate" Height="5px" Text="Generate PDF" Width="101px">
                                                                <ClientSideEvents Click="function (s, e) {fn_GenearatePDF();}"></ClientSideEvents>
                                                            </dxe:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <%-- <tr id="tr_pdf" runat="server">
                    <td id="td1">
                        <asp:Button ID="Button1" runat="server" Text="Print" CssClass="btnUpdate" Width="165px"
                            OnClick="BtnPrint_Click" />
                    </td>
                    <td align="center">
                        <a id="A3" style="cursor: pointer;" onclick="btnShow_Click()">
                            <div style="height: 12px; padding: 3px 2px 3px 2px; width: 90px; border: 1px solid blue;
                                line-height: 14px; font-size: 11px; font-weight: bold; background: url(../images/EHeaderBack.gif) repeat-x 0px 0px;">
                                <span>Generate</span>
                            </div>
                        </a>
                    </td>
                </tr>
                <tr id="tr_email" runat="server">
                   
                </tr>--%>
                                        <%-- <tr>
                    <td>
                    </td>
                </tr>--%>
                                    </table>
                                </td>
                            </tr>
                            <%-- <tr>
                                <td class="gridcellleft" colspan="4">
                                    <table>
                                        <tr>
                                            <td class="gridcellleft">
                                                <asp:CheckBox ID="CHKAPPMRGN" runat="server" Checked="true" />
                                                Consider Only Clients Having Applicabel Margin As On T/T-1 Day</td>
                                        </tr>
                                        <tr>
                                            <td class="gridcellleft">
                                                <asp:CheckBox ID="CHKDETAIL" runat="server" Checked="true" />
                                                Show Holding Details</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>--%>
                            <%-- <tr>
                                <td colspan="5">
                                    <table>
                                        <tr>
                                            <td id="Td3" class="gridcellleft">
                                                <asp:RadioButton ID="rbScreen" runat="server" GroupName="f" Checked="True" onclick="RBShowHide(this.value)" />Screen
                                            </td>
                                            <td id="Td4" class="gridcellleft">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="rbPrint" runat="server" GroupName="f" onclick="RBShowHide(this.value)" />Print</td>
                                                        <td id="tr_printlogo">
                                                            <asp:CheckBox ID="CHKLOGOPRINT" runat="server" Checked="true" />
                                                            Do Not Print Logo</td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td id="td_mail" class="gridcellleft">
                                                <asp:RadioButton ID="rdbmail" runat="Server" GroupName="f" onclick="RBShowHide(this.value)" />Send
                                                Email
                                            </td>
                                        </tr>
                                        <tr id="Tr_DigitalSign">
                                            <td colspan="2">
                                                <table border="10" cellpadding="1" cellspacing="1">
                                                    <tr>
                                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                                            Select Signature :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtdigitalName" runat="server" Width="250px" onkeyup="FunCallAjaxList(this,event,'Digital')"></asp:TextBox>
                                                            <asp:TextBox ID="txtdigitalName_hidden" runat="server" TabIndex="11" Width="100px"
                                                                Style="display: none"></asp:TextBox>
                                                        </td>
                                                        <td id="td_msg" runat="server">
                                                            <asp:Label ID="Location" runat="server" Text="You Dont have Permission.Please Contact to Administrator"
                                                                ForeColor="red" Font-Bold="true"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 22px" id="td_btnshow" class="gridcellleft">
                                                <asp:Button ID="btnshow" runat="server" CssClass="btnUpdate" Height="20px" Text="Show"
                                                    OnClientClick="selecttion()" Width="101px" OnClick="btnshow_Click" />
                                            </td>
                                            <td style="height: 22px" id="td_btnprint">
                                                <asp:Button ID="btnprint" runat="server" CssClass="btnUpdate" Height="20px" Text="Print"
                                                    OnClientClick="selecttion()" Width="101px" OnClick="btnprint_Click" />
                                            </td>
                                            <td style="height: 22px" id="td_btnemail" class="gridcellleft">
                                                <asp:Button ID="btnemail" runat="server" CssClass="btnUpdate" Height="20px" Text="Send Email"
                                                    OnClientClick="selecttion()" Width="101px" OnClick="btnemail_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>--%>
                        </table>
                    </td>
                    <td style="height: 353px">
                        <table width="100%" id="showFilter">
                            <tr>
                                <td style="text-align: right; vertical-align: top; height: 134px;">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="gridcellleft" style="vertical-align: top; text-align: right; height: 21px;"
                                                id="TdFilter">
                                                <span id="spanunder"></span><span id="spanclient"></span>
                                                <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox>
                                                <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                    Enabled="false">
                                                    <asp:ListItem>Segment</asp:ListItem>
                                                    <asp:ListItem>Clients</asp:ListItem>
                                                    <asp:ListItem>Branch</asp:ListItem>
                                                    <asp:ListItem>Group</asp:ListItem>
                                                    <asp:ListItem>BranchGroup</asp:ListItem>
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
                        </table>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="display: none;">
                        <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                        <asp:Button ID="btnhide" runat="server" Text="Button" OnClick="btnhide_Click" />
                    </td>
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
                </tr>
            </table>
            <dxe:ASPxPopupControl ID="PopUp_ScripAlert" runat="server" ClientInstanceName="cPopUp_ScripAlert"
                Width="340px" HeaderText="ECN Detail Information" PopupHorizontalAlign="Center"
                PopupVerticalAlign="Middle" CloseAction="None" Modal="True" ShowCloseButton="False">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <%--<div style="font-weight: bold; color: black; background-color: gainsboro; border-right: silver thin solid;
                            border-top: silver thin solid; border-left: silver thin solid; border-bottom: silver thin solid;">
                            No of ECN Sent : <b style="text-align:right" id="B_allcontractpop" runat="server"></b>
                            <br />
                            <br />
                            Remaining ECN (To Be Send) : <b style="text-align:right" id="B_ecnenablepop" runat="server"></b>
                        </div>
                        <br />
                        <br />--%>
                        <dxe:ASPxCallbackPanel ID="CbpSuggestISIN" runat="server" ClientInstanceName="cCbpSuggestISIN"
                            BackColor="White" OnCallback="CbpSuggestISIN_Callback" LoadingPanelText="Sending....Please Wait !!"
                            LoadingPanelStyle-Font-Bold="true" LoadingPanelStyle-Cursor="wait" LoadingPanelStyle-ForeColor="gray"
                            LoadingPanelImage-Url='../images/Animated_Email.gif'>
                            <ClientSideEvents EndCallback="CbpSuggestISIN_EndCallBack" />
                            <PanelCollection>
                                <dxe:panelcontent runat="server">
                                    <div id="div_fail" style="color: Red; font-weight: bold; font-size: 12px;">
                                        An Internal Error Occured during sending some ECNs.Please send Remaing.
                                    </div>
                                    <div id="div_success" style="color: Green; font-weight: bold; font-size: 12px;">
                                        Successfully send ECNs.
                                    </div>
                                    <%--<asp:Label runat="server" ID="div_success" Text="efgh"></asp:Label>--%>
                                    <br />
                                    <br />
                                    <div style="font-weight: bold; color: black; background-color: gainsboro; border-right: silver thin solid;
                                        border-top: silver thin solid; border-left: silver thin solid; border-bottom: silver thin solid;">
                                        No of ECN Sent : <b style="text-align: right" id="B_allcontractpop" runat="server"></b>
                                        <br />
                                        <%--<asp:Image src='../Documents/Animated_Email.gif' runat="server" />--%>
                                        <br />
                                        Remaining ECN (To Be Sent) : <b style="text-align: right" id="B_ecnenablepop" runat="server">
                                        </b>
                                    </div>
                                    <br />
                                    <br />
                                    <div class="frmleftCont" id="btnall">
                                        <dxe:ASPxButton ID="btnsendall" runat="server" AutoPostBack="False" Text="Send All">
                                            <ClientSideEvents Click="function (s, e) {btnsendall_Click();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                    <div class="frmleftCont" id="btnremain">
                                        <dxe:ASPxButton ID="btnsendremaining" runat="server" AutoPostBack="False" Text="Send Remaining">
                                            <ClientSideEvents Click="function (s, e) {btnsendremaining_Click();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                    <div class="frmleftCont" id="btnokdiv">
                                        <dxe:ASPxButton ID="btnok" runat="server" AutoPostBack="False" Text="Close">
                                            <ClientSideEvents Click="function (s, e) {btnok_Click();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                    <div class="frmleftCont" id="cancel">
                                        <dxe:ASPxButton ID="btncancel" runat="server" AutoPostBack="False" Text="Cancel">
                                            <ClientSideEvents Click="function (s, e) {btncancel_Click();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                </dxe:panelcontent>
                            </PanelCollection>
                        </dxe:ASPxCallbackPanel>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="LightGray" ForeColor="Black" />
            </dxe:ASPxPopupControl>
            <table>
                <tr>
                    <td style="display: none;">
                        <asp:HiddenField ID="HiddenField_Group" runat="server" />
                        <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                        <asp:HiddenField ID="HiddenField_Client" runat="server" />
                        <asp:HiddenField ID="HiddenField_Segment" runat="server" />
                        <asp:HiddenField ID="HiddenField_BranchGroup" runat="server" />
                        <asp:Button ID="BtnForExportEvent" runat="server" OnClick="cmbExport_SelectedIndexChanged"
                            BackColor="#DDECFE" BorderStyle="None" />
                        <asp:Button ID="BtnForExportEvent1" runat="server" OnClick="cmbExport1_SelectedIndexChanged"
                            BackColor="#DDECFE" BorderStyle="None" />
                        <asp:Button ID="BtnForExportEvent2" runat="server" OnClick="cmbExport2_SelectedIndexChanged"
                            BackColor="#DDECFE" BorderStyle="None" />
                    </td>
                </tr>
            </table>
            <div id="displayAll" style="display: none;">
                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                    <ContentTemplate>
                        <table width="100%" border="1">
                            <tr style="display: none;">
                                <td>
                                    <asp:DropDownList ID="cmbclient" runat="server" Font-Size="12px" Width="300px" AutoPostBack="True">
                                    </asp:DropDownList><asp:HiddenField ID="hiddencount" runat="server" />
                                </td>
                            </tr>
                            <tr id="tr_DIVdisplayPERIOD">
                                <td>
                                    <span style="text-align: center;"></span>
                                    <div id="DIVdisplayPERIOD" runat="server">
                                    </div>
                                </td>
                            </tr>
                            <tr id="tr_DIVdisplayCompliance">
                                <td>
                                    <div id="DIVdisplayCompliance" runat="server">
                                    </div>
                                </td>
                            </tr>
                            <tr bordercolor="Blue" id="tr_prvnxt">
                                <td align="center">
                                    <table id="tblpage" cellspacing="0" cellpadding="0" runat="server" width="100%">
                                        <tr>
                                            <td width="20" style="padding: 5px">
                                                <asp:LinkButton ID="ASPxFirst" runat="server" Font-Bold="True" ForeColor="maroon"
                                                    OnClientClick="javascript:selecttion();;" OnClick="ASPxFirst_Click">First</asp:LinkButton></td>
                                            <td width="25">
                                                <asp:LinkButton ID="ASPxPrevious" runat="server" Font-Bold="True" ForeColor="Blue"
                                                    OnClientClick="javascript:selecttion();" OnClick="ASPxPrevious_Click">Previous</asp:LinkButton></td>
                                            <td width="20" style="padding: 5px">
                                                <asp:LinkButton ID="ASPxNext" runat="server" Font-Bold="True" ForeColor="maroon"
                                                    OnClientClick="javascript:selecttion();" OnClick="ASPxNext_Click">Next</asp:LinkButton></td>
                                            <td width="20">
                                                <asp:LinkButton ID="ASPxLast" runat="server" Font-Bold="True" ForeColor="Blue" OnClientClick="javascript:selecttion();"
                                                    OnClick="ASPxLast_Click">Last</asp:LinkButton></td>
                                            <td align="right">
                                                <asp:Label ID="listRecord" runat="server" Font-Bold="True"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="display" runat="server">
                                    </div>
                                </td>
                            </tr>
                            <asp:HiddenField ID="Totalbroker" runat="server" />
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnshow" EventName="Click"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="btnmailsend" EventName="Click"></asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </asp:Content>