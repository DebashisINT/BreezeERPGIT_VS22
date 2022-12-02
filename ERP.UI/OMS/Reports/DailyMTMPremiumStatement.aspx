<%@ Page Language="C#" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_DailyMTMPremiumStatement" Codebehind="DailyMTMPremiumStatement.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script language="javascript" type="text/javascript">

  
    function Page_Load()///Call Into Page Load
            {
                 Hide('showFilter');
                 Hide('td_filter');
                 document.getElementById('hiddencount').value=0;
                 fn_ReportView('1');
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
            window.frameElement.width = document.body.scrollWidth;
           
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
                ajax_showOptions(objID,'ShowClientFORMarginStocks',objEvent,cmbVal);
            }
            else if(document.getElementById('cmbsearchOption').value=="Expiry")
            {
                 
                 var date2=null;
                 var date1 = DtFrom.GetDate();
                 if(date1 != null) 
                 {
                   date2 =parseInt(date1.getMonth()+1)+'-'+date1.getDate()+'-'+ date1.getFullYear();
                 }
                
                 ajax_showOptions(objID,'Searchproductandeffectuntil',objEvent,'Expiry'+'~'+date2);
            }
            else 
            {
                cmbVal=document.getElementById('cmbsearchOption').value;
                cmbVal=cmbVal+'~'+document.getElementById('ddlgrouptype').value;
                ajax_showOptions(objID,'ShowClientFORMarginStocks',objEvent,cmbVal);
            }
          
         

        }

        
        function fnClients(obj)
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
        }
        function fnAsset(obj)
        {
             if(obj=="a")
                Hide('showFilter');
             else
             {
                  var cmb=document.getElementById('cmbsearchOption');
                  cmb.value='Product';
                  Show('showFilter');
             }
             selecttion();
        }
        function fnExpiry(obj)
        {
             if(obj=="a")
                Hide('showFilter');
             else
             {
                  var cmb=document.getElementById('cmbsearchOption');
                  cmb.value='Expiry';
                  Show('showFilter');
             }
             selecttion();
        }
      function fnBranch(obj)
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
      }
     function fnGroup(obj)
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
                document.getElementById('btnScreen').disabled=false;
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
   }

    function RecordDisplay()
    {
        Hide('showFilter');
        Show('td_filter');
        Hide('tab1');
        Show('displayAll');
        document.getElementById('hiddencount').value=0;
        selecttion();
        height();
        
    }
    function fnNoRecord(obj)
    {
        
        Hide('td_filter');
        Show('tab1');
        Hide('displayAll');
        if(obj=='1')
            alert('No Record Found!!');
        if(obj=='3')
            alert('You Can Only Select Report View - '+'\n'+' [Branch/Group - Client] '+'\n'+' [Client / Group / Branch  + Month] '+'\n'+' [Month + Client / Group / Branch] '+'\n'+' [Instrument + Client / Group / Branch] ');
        if(obj=='4')
            alert("Mail Sent Successfully !!");
        if(obj=='5')
             alert("Error on sending!Try again.. !!");
        if(obj=='6')
             alert("You Have To Chcek Atleast Three Columns !!");
        document.getElementById('hiddencount').value=0;  
       
        fn_ReportView(document.getElementById('DLLRptView').value);  
         Hide('showFilter');
        height();
        selecttion();
    }
    function fn_ReportView(obj)
    {
       
        if(obj=='2' || obj=='3'  || obj=='5'  || obj=='6' || obj=='11')/// Product + Date
        {
            Show('Tr_Asset');
            Show('Tr_Expiry');
            if(obj=='5' ||  obj=='6')///////For Exchange
              Hide('Tr_GroupBy');
            else         ///////For Client
              Show('Tr_GroupBy');
            FilterColumnDisplay(obj);
            FnddlGeneration(document.getElementById('ddlGeneration').value);    
            
        }
        else if(obj=='12' || obj=='13')/// Product + Date[with open position]
        {
            Show('Tr_Asset');
            Show('Tr_Expiry');
            Show('Tr_detail');
            Show('Tr_GroupBy');
            FilterColumnDisplay(obj);
           FnddlGeneration('2'); 
            
        }
       else if(obj=='7' || obj=='8' || obj=='10')///Tot and Obligation
         {
            Hide('Tr_Asset');
            Hide('Tr_Expiry');
            Show('Tr_GroupBy');
            FilterColumnDisplay(obj);
            document.getElementById('ddlGeneration').value="2";
            FnddlGeneration('2'); 
         }
        else if(obj=='9')///Group + Client
         {
            Hide('Tr_Asset');
            Hide('Tr_Expiry');
            Show('Tr_GroupBy');
            FilterColumnDisplay(obj);
            document.getElementById('ddlGeneration').value="2";
            FnddlGeneration('2'); 
         }
        else 
        {
            Hide('Tr_Asset');
            Hide('Tr_Expiry');
            if(obj=='4')///////For Exchange
              Hide('Tr_GroupBy');
            else///////For Client
              Show('Tr_GroupBy');
            FilterColumnDisplay(obj);
            FnddlGeneration(document.getElementById('ddlGeneration').value);    
        }
      
    }
  function FilterColumnDisplay(Obj)
  {
    if(Obj=='1' || Obj=='4')//Summary View Date Wise
    {
        Hide('Tr_DetailProduct');
        Hide('Tr_detail');
        Show('Tr_Summary');
    }
    if(Obj=='2' || Obj=='5')//Detail View Product Wise
    {
        Show('Tr_DetailProduct');
        Hide('Tr_detail');
        Hide('Tr_Summary');
    }
    if(Obj=='3' || Obj=='6' || Obj=='11')//Detail View Product+Date Wise
    {
        Hide('Tr_DetailProduct');
        Show('Tr_detail');
        Hide('Tr_Summary');
    }
    if(Obj=='12' || Obj=='13')//Detail View Product+Date Wise
    {
        Hide('Tr_DetailProduct');
        Hide('Tr_detail');
        Hide('Tr_Summary');
    }
  }
  function FnddlGeneration(obj)
  {
       
    if(obj=="1")
    {
   
        Show('td_Screen');
        Hide('td_Export');
        Hide('td_Mail');
        Hide('tr_MailSendOption');
        Hide('showFilter');
      
    }
    if(obj=="2")
    {
    
        Hide('td_Screen');
        Show('td_Export');
        Hide('td_Mail');
        Hide('tr_MailSendOption');
        Hide('showFilter');
        
    }
    if(obj=="3")
    {
   
        Hide('td_Screen');
        Hide('td_Export');
        Show('td_Mail');
        Show('tr_MailSendOption');
        var cmb=document.getElementById('cmbsearchOption');
        cmb.value='MAILEMPLOYEE';
        //Show('showFilter');
      document.getElementById('Button1').click();
    
    }
   
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
  function selecttion()
  {
     var combo=document.getElementById('cmbExport');
     combo.value='Ex';
  } 
 
 function SelectAllCheckboxesSummary(chk) 
  {
    $('#<%=chktfilterSummary.ClientID %>').find("input:checkbox").each(function() 
    {
        if (this != chk) {
            this.checked = chk.checked;
        }
    });
    
 }
  function SelectAllCheckboxesDetail(chk) 
  {
    $('#<%=chktfilterDetail.ClientID %>').find("input:checkbox").each(function() 
    {
        if (this != chk) {
            this.checked = chk.checked;
        }
    });
    
   }
  function SelectAllChkDetailProduc(chk) 
  {
    $('#<%=ChkfilterDetailProduct.ClientID %>').find("input:checkbox").each(function() 
    {
        if (this != chk) {
            this.checked = chk.checked;
        }
    });
    
   }
    function tgentype()
        {
        var drpmail=document.getElementById('ddloptionformail');
            var op=new  Option();
            op.value='1';
            op.text='item1'
            drpmail[0]=op;
            
        
        }
     function mailoption(objval)
        {
           if(objval=='1')
                Show('showFilter');
           else
                Hide('showFilter');
        
        }
    FieldName='lstSlection';
     </script>
    
  
 
  <script type="text/ecmascript">   
       function ReceiveServerData(rValue)
        {
                
                var j=rValue.split('~');
                var btn = document.getElementById('btnhide');

                if(j[0]=='Branch')
                {
                    document.getElementById('HiddenField_Branch').value = j[1];
                }
                if(j[0]=='Group')
                {
                    document.getElementById('HiddenField_Group').value = j[1];
                }  
                if(j[0]=='Clients')
                {
                    document.getElementById('HiddenField_Client').value = j[1];
                } 
                if(j[0]=='Product')
                {
                    document.getElementById('HiddenField_Product').value = j[1];
                }
                if(j[0]=='Expiry')
                {
                    document.getElementById('HiddenField_Expiry').value = j[1];
                }
                if(j[0]=='BranchGroup')
                {
                    document.getElementById('HiddenField_BranchGroup').value = j[1];
                }
                if(j[0]=='MAILEMPLOYEE')
                {
                    document.getElementById('HiddenField_emmail').value = j[1];
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
                    <strong><span id="SpanHeader" style="color: #000099">Daily MTM/Premium Statement</span></strong></td>

              <td class="EHEADER" width="15%" id="td_filter" style="height: 22px">
                    <a href="javascript:void(0);" onclick="fnNoRecord(2);"><span style="color: Blue; text-decoration: underline;
                        font-size: 8pt; font-weight: bold">Filter</span></a>
                 <asp:DropDownList ID="cmbExport" runat="server" AutoPostBack="True" Font-Size="12px" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                        <asp:ListItem Value="E">Excel</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    
       <table id="tab1" border="0" cellpadding="0" cellspacing="0">
            <tr valign="top">
                <td>
                    <table>
                        <tr>
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            For A Period
                                        </td>
                                        <td class="gridcellleft">
                                            <dxe:ASPxDateEdit ID="DtFrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="108px" ClientInstanceName="DtFrom">
                                                <dropdownbutton text="From">
                                                </dropdownbutton>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td class="gridcellleft">
                                            <dxe:ASPxDateEdit ID="DtTo" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="108px" ClientInstanceName="DtTo">
                                                <dropdownbutton text="To">
                                                </dropdownbutton>
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
                                            Report View :</td>
                                        <td>
                                            <asp:DropDownList ID="DLLRptView" runat="server" Width="300px" Font-Size="12px" onchange="fn_ReportView(this.value)">
                                                <asp:ListItem Value="1">Client + Date</asp:ListItem>
                                                <asp:ListItem Value="9">Group + Client </asp:ListItem>
                                                <asp:ListItem Value="2">Client + Product </asp:ListItem>
                                                <asp:ListItem Value="3">Client + Product + Date</asp:ListItem>
                                                 <asp:ListItem Value="11">Client + Date + Product </asp:ListItem>
                                                <asp:ListItem Value="4">Exchange + Date</asp:ListItem>
                                                <asp:ListItem Value="5">Exchange + Product </asp:ListItem>
                                                <asp:ListItem Value="6">Exchange + Product + Date</asp:ListItem>
                                                <asp:ListItem Value="7">Group + Client To & Obligation </asp:ListItem>
                                                <asp:ListItem Value="8">Group + Client +Date To & Obligation </asp:ListItem>
                                                <asp:ListItem Value="10">Group + Date To & Obligation </asp:ListItem>
                                                <asp:ListItem Value="12">Client + Date + Product [Open Position With Exposure]</asp:ListItem>
                                                <asp:ListItem Value="13">Client + Date + Margin</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="Tr_GroupBy">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Group By</td>
                                        <td>
                                            <asp:DropDownList ID="ddlGroup" runat="server" Width="100px" Font-Size="12px" onchange="fnddlGroup(this.value)">
                                                <asp:ListItem Value="0">Branch</asp:ListItem>
                                                <asp:ListItem Value="1">Group</asp:ListItem>
                                                <asp:ListItem Value="2">Branch Group</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="2" id="td_branch">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rdbranchAll" runat="server" Checked="True" GroupName="a" onclick="fnBranch('a')" />
                                                        All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbranchSelected" runat="server" GroupName="a" onclick="fnBranch('b')" />Selected
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
                                                        <asp:RadioButton ID="rdddlgrouptypeAll" runat="server" Checked="True" GroupName="b"
                                                            onclick="fnGroup('a')" />
                                                        All
                                                        <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="b" onclick="fnGroup('b')" />Selected
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Clients :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbClientALL" runat="server" Checked="True" GroupName="c" onclick="fnClients('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdPOAClient" runat="server" GroupName="c" onclick="fnClients('a')" />POA
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbClientSelected" runat="server" GroupName="c" onclick="fnClients('b')" />
                                            Selected
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="Tr_Asset">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Asset :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbAssetAll" runat="server" Checked="True" GroupName="ee" onclick="fnAsset('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdAssetSelected" runat="server" GroupName="ee" onclick="fnAsset('b')" />Selected
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="Tr_Expiry">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Expiry :</td>
                                        <td>
                                            <asp:RadioButton ID="rdbExpiryAll" runat="server" Checked="True" GroupName="e" onclick="fnExpiry('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbExpirySelected" runat="server" GroupName="e" onclick="fnExpiry('b')" />Selected
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="Tr_detail">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Filter Columns :
                                        </td>
                                        <td>
                                           <table> <tr>
                                                    <td >
                                                        <asp:CheckBox ID="chkAllDetail" runat="server" onclick="javascript:SelectAllCheckboxesDetail(this);"
                                                            Checked="true" /><span style="font-family: Verdana; color: Teal; font-size: x-small;"><b>Check/UnCheck
                                                                ALL</b></span>
                                                    </td>
                                                </tr><tr><td> <div style="overflow: auto; border: 1px black solid; scrollbar-base-color: #C0C0C0">
                                                <asp:CheckBoxList ID="chktfilterDetail" runat="server" RepeatDirection="Horizontal" Width="500px"
                                                    RepeatColumns="6">
                                                    <asp:ListItem Value="B/f Qty" Selected="True">B/f Qty</asp:ListItem>
                                                    <asp:ListItem Value="B/F Price" Selected="True">B/F Price</asp:ListItem>
                                                    <asp:ListItem Value="Buy Qty" Selected="True">Buy Qty</asp:ListItem>
                                                    <asp:ListItem Value="Buy Value" Selected="True">Buy Value</asp:ListItem>
                                                    <asp:ListItem Value="Sell Qty" Selected="True">Sell Qty</asp:ListItem>
                                                    <asp:ListItem Value="Sell Value" Selected="True">Sell Value</asp:ListItem>
                                                    <asp:ListItem Value="C/f Qty" Selected="True">C/f Qty</asp:ListItem>
                                                    <asp:ListItem Value="C/f Price" Selected="True">C/f Price</asp:ListItem>
                                                    <asp:ListItem Value="MTM P/L" Selected="True">MTM P/L</asp:ListItem>
                                                    <asp:ListItem Value="Premium" Selected="True">Premium</asp:ListItem>
                                                    <asp:ListItem Value="Asn/Exc" Selected="True">Asn/Exc</asp:ListItem>
                                                    <asp:ListItem Value="Fin.Sett" Selected="True">Fin.Sett</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </div></td></tr></table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                              <tr id="Tr_DetailProduct">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Filter Columns :
                                        </td>
                                        <td>
                                           <table> <tr>
                                                    <td >
                                                        <asp:CheckBox ID="ChkDetailProducALL" runat="server" onclick="javascript:SelectAllChkDetailProduct(this);"
                                                            Checked="true" /><span style="font-family: Verdana; color: Teal; font-size: x-small;"><b>Check/UnCheck
                                                                ALL</b></span>
                                                    </td>
                                                </tr><tr><td> <div style="overflow: auto; border: 1px black solid; scrollbar-base-color: #C0C0C0">
                                                <asp:CheckBoxList ID="ChkfilterDetailProduct" runat="server" RepeatDirection="Horizontal" Width="200px"
                                                    RepeatColumns="2">
                                                  
                                                    <asp:ListItem Value="MTM P/L" Selected="True">MTM P/L</asp:ListItem>
                                                    <asp:ListItem Value="Premium" Selected="True">Premium</asp:ListItem>
                                                    <asp:ListItem Value="Asn/Exc" Selected="True">Asn/Exc</asp:ListItem>
                                                    <asp:ListItem Value="Fin.Sett" Selected="True">Fin.Sett</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </div></td></tr></table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                         <tr id="Tr_Summary">
                            <td class="gridcellleft">
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Filter Columns :
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td >
                                                        <asp:CheckBox ID="chkAllSummary" runat="server" onclick="javascript:SelectAllCheckboxesSummary(this);"
                                                            Checked="true" /><span style="font-family: Verdana; color: Teal; font-size: x-small;"><b>Check/UnCheck
                                                                ALL</b></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div style="overflow: auto; border: 1px black solid; scrollbar-base-color: #C0C0C0">
                                                            <asp:CheckBoxList ID="chktfilterSummary" runat="server" RepeatDirection="Horizontal"
                                                                Width="400px" RepeatColumns="6">
                                                                <asp:ListItem Value="MTM P/L" Selected="True">MTM P/L</asp:ListItem>
                                                                <asp:ListItem Value="Premium" Selected="True">Premium</asp:ListItem>
                                                                <asp:ListItem Value="Fin.Sett" Selected="True">Fin.Sett</asp:ListItem>
                                                                <asp:ListItem Value="Asn/Exc" Selected="True">Asn/Exc</asp:ListItem>
                                                                <asp:ListItem Value="TranCharge" Selected="True">TranCharge</asp:ListItem>
                                                                <asp:ListItem Value="StampDuty" Selected="True">StampDuty</asp:ListItem>
                                                                <asp:ListItem Value="OtherCharge" Selected="True">OtherCharge</asp:ListItem>
                                                                <asp:ListItem Value="TotalServTax" Selected="True">TotalServTax</asp:ListItem>
                                                                <asp:ListItem Value="SebiFee" Selected="True">SebiFee</asp:ListItem>
                                                                <asp:ListItem Value="STT" Selected="True">STT</asp:ListItem>
                                                                <asp:ListItem Value="NetObligation" Selected="True">NetObligation</asp:ListItem>
                                                            </asp:CheckBoxList>
                                                        </div>
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
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Generate Type :</td>
                                        <td>
                                            <asp:DropDownList ID="ddlGeneration" runat="server" Width="210px" Font-Size="12px"
                                                onchange="FnddlGeneration(this.value)" >
                                                <asp:ListItem Value="1">Screen</asp:ListItem>
                                                <asp:ListItem Value="2">Export</asp:ListItem>
                                                  <asp:ListItem Value="3">Send Mail</asp:ListItem>
                                            </asp:DropDownList>
                                           
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="tr_MailSendOption">
                            <td class="gridcellleft">
                               <asp:UpdatePanel ID="upanelRespective" runat="Server" UpdateMode="Conditional">
                               <ContentTemplate>
                                <table id="tabRespective" runat="Server" border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Respective :</td>
                                        <td>
                                            <asp:DropDownList ID="ddloptionformail" onchange="mailoption(this.value)" runat="server" Width="100px" Font-Size="12px"
                                                >
                                                
                                            </asp:DropDownList></td>
                                    </tr>
                                </table>
                                </ContentTemplate>
                                <Triggers> <asp:AsyncPostBackTrigger ControlID="Button1" /> </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <table>
                                    <tr>
                                        <td id="td_Screen">
                                            <asp:Button ID="btnScreen" runat="server" CssClass="btnUpdate" Height="20px" Text="Show"
                                                Width="101px" OnClientClick="selecttion()" OnClick="btnScreen_Click" />
                                        </td>
                                        <td id="td_Mail">
                                            <asp:Button ID="btnSendmail" runat="server" CssClass="btnUpdate" Height="20px" Text="Send Mail"
                                                Width="101px" OnClientClick="selecttion()" OnClick="btnSendmail_Click"  /></td>
                                        <td id="td_Export">
                                            <asp:Button ID="btnExcel" runat="server" CssClass="btnUpdate" Height="20px" Text="Export To Excel"
                                                Width="101px"  OnClientClick="selecttion()" OnClick="btnExcel_Click" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        
                    </table>
                </td>
                <td>
                    <table border="10" cellpadding="1" cellspacing="1" id="showFilter">
                        <tr>
                            <td id="TdFilter">
                                <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox>
                                <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                    Enabled="false">
                                    <asp:ListItem>Clients</asp:ListItem>
                                    <asp:ListItem>Branch</asp:ListItem>
                                    <asp:ListItem>Group</asp:ListItem>
                                    <asp:ListItem>Product</asp:ListItem>
                                    <asp:ListItem>BranchGroup</asp:ListItem>
                                    <asp:ListItem>Expiry</asp:ListItem>
                                    <asp:ListItem>MAILEMPLOYEE</asp:ListItem>
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
        <table>
            <tr>
                <td style="display: none;">
                    <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                    <asp:Button ID="btnhide" runat="server" Text="btnhide" OnClick="btnhide_Click" />
                    <asp:HiddenField ID="HiddenField_Group" runat="server" />
                    <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                    <asp:HiddenField ID="HiddenField_Client" runat="server" />
                    <asp:HiddenField ID="HiddenField_Product" runat="server" />
                    <asp:HiddenField ID="HiddenField_Expiry" runat="server" />
                    <asp:HiddenField ID="HiddenField_BranchGroup" runat="server" />
                    <asp:HiddenField ID="hiddencount" runat="server" />
                    <asp:HiddenField ID="HiddenField_emmail" runat="server" />
                     <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
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
       <div id="displayAll" style="display: none;" width="100%">
                 <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                     <ContentTemplate>
                         <table width="100%" border="1">
                          <tr style="display: none;">
                            <td>
                                <asp:DropDownList ID="cmbclient" runat="server" Font-Size="12px" Width="300px" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                           
                             <tr>
                                 <td>
                                     <div id="DivHeader" runat="server">
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
                                     <div id="Divdisplay" runat="server">
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
      
    </div>
</asp:Content>