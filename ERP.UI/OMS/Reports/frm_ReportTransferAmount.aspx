<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frm_ReportTransferAmount" Codebehind="frm_ReportTransferAmount.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>

    
    
    <script type="text/javascript" src="../CentralData/JSScript/GenericJScript.js"></script>
    

    


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

    <script language='JavaScript' type="text/javascript">
    groupvalue='';
      function height()
    {
    
        if(document.body.scrollHeight>=500)
            window.frameElement.height = document.body.scrollHeight;
        else
            window.frameElement.height = '500px';
       
      window.frameElement.Width = document.body.scrollWidth;
    }
    function Page_Load()
    {
        document.getElementById('TrFilter').style.display='none';
        document.getElementById('TrTransfer').style.display='none';
    }
    function AllSelct(obj,obj1)
          {
            var FilTer=document.getElementById('cmbsearchOption');
            if(obj!='a')
            {
                if(obj1=='C')
                    FilTer.value='Clients';
                else if(obj1=='B')    
                    FilTer.value='Branch';
                else if(obj1=='G')    
                    FilTer.value='Group';
                document.getElementById('TrFilter').style.display='inline';
                document.getElementById('TdFilter').style.display='inline';
            }
            else
            {
                document.getElementById('TrFilter').style.display='none';
                document.getElementById('TdFilter').style.display='none';
            }
          }
       function fngrouptype(obj)
           {
               if(obj=="0")
               {
                document.getElementById('td_allselect').style.display='none';
                alert('Please Select Group Type !');
               }
               else
               {
                    document.getElementById('td_allselect').style.display='inline';
               }
           }
       function  fnddlGroup(obj)
       {
            if(obj=="0")
            {
                document.getElementById('td_group').style.display='none';
                document.getElementById('td_branch').style.display='inline';
            }
            else
            {
                document.getElementById('td_group').style.display='inline';
                document.getElementById('td_branch').style.display='none';
                var btn = document.getElementById('btnhide');
                btn.click();
            }
       }
      function showOptionsforSunAc(obj1,obj2,obj3,obj4)
         {
         
             var cmb=document.getElementById('cmbsearchOption');
             var cmb1=document.getElementById('litSegment1');
             var SpanVal=document.getElementById('litSegment1').innerText;
             if(cmb.value=='Branch')
             
             {
                 ajax_showOptions(obj1,obj2,obj3,'Branch');
             }
             else
             {
                
                ajax_showOptions(obj1,obj2,obj3,obj4+'~'+SpanVal,'Sub');
             }
             
            
         }
        FieldName='lstSuscriptions';
        
        function btnAddsubscriptionlist_click()
        {
            var userid = document.getElementById('txtsegselected');
            
            if(userid.value != '')
            {
                //NSDL/CDSL
                var Arr=document.getElementById('txtsegselected_hidden').value.split("~");
                var WhichCall=Arr[Arr.length-1];
                if(WhichCall=="NSDLCLIENT" || WhichCall=="CDSLCLIENT")
                    document.getElementById('txtsegselected_hidden').value=Arr[0];
                
                var ids = document.getElementById('txtsegselected_hidden');
                //End NSDL/CDSL
                var listBox = document.getElementById('lstSuscriptions');
                var tLength = listBox.length;
                var no = new Option();
                no.value = ids.value;
                no.text = userid.value;
                listBox[tLength]=no;
                var recipient = document.getElementById('txtsegselected');
                recipient.value='';
            }
            else
                alert('Please search name and then Add!')
            var s=document.getElementById('txtsegselected');
            s.focus();
            s.select();
        } 
         function clientselectionfinal()
	    {
	        var listBoxSubs = document.getElementById('lstSuscriptions');
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
//	        var i;
            for(i=listBoxSubs.options.length-1;i>=0;i--)
            {
                listBoxSubs.remove(i);
            }
           
            document.getElementById('TdFilter').style.display='none';
            document.getElementById('TdFilter1').style.display='none';
	        
//	        document.getElementById('rdAll').checked='true';
	    }
	    function MainAll(obj1,obj2)
        {
            document.getElementById('cmbsearchOption').value=obj1;
            if(obj2=='all')
            {
                
                document.getElementById('TdFilter').style.display='none';
                document.getElementById('TdFilter1').style.display='none';
                if(document.getElementById('rdAllBranch').checked==true)
                {
                    document.getElementById('litSegment1').innerText="";
                    document.getElementById('litSegment12').innerText="";
                }
                if(document.getElementById('rdAll').checked==true)
                {
                   
                    document.getElementById('litSegment').innerText="";
                    document.getElementById('litSegment112').innerText="";
                }
            }
            else    
            {
                document.getElementById('TdFilter').style.display='inline';
                document.getElementById('TdFilter1').style.display='inline';
               
            }
        }
    
    function ImagevisibleOff()
    {
         document.getElementById("Div1").style.visibility='hidden';//style.display='none';
    }
    function ImageAndHeight()
    {
        height();
        ImagevisibleOff();
    }
    function FunClientScrip(objID,objListFun,objEvent)
        {
           
            var cmbVal;
            if(document.getElementById('cmbsearchOption').value=="Clients")
            {
                if(document.getElementById('ddlGroup').value=="0")//////////////Group By  selected are branch
                {
                    if(document.getElementById('rdbranchAll').checked==true)
                       {
                           cmbVal=document.getElementById('cmbsearchOption').value+'Branch';
                           cmbVal=cmbVal+'~'+'ALL'+'~'+document.getElementById('ddlgrouptype').value;
                           cmbVal=cmbVal+"SelectedSegmentID,"+'<%=Session["UserSegID"]%>';
                       }
                   else
                       {
                           cmbVal=document.getElementById('cmbsearchOption').value+'Branch';
                           cmbVal=cmbVal+'~'+'Selected'+'~'+groupvalue;
                       }
                }
               else //////////////Group By selected are Group
                {
                   if(document.getElementById('rdddlgrouptypeAll').checked==true)
                       {
                           cmbVal=document.getElementById('cmbsearchOption').value+'Group';
                           cmbVal=cmbVal+'~'+'ALL'+'~'+document.getElementById('ddlgrouptype').value;
                           cmbVal=cmbVal+"SelectedSegmentID,"+'<%=Session["UserSegID"]%>';
                       }
                   else
                       {
                           cmbVal=document.getElementById('cmbsearchOption').value+'Group';
                           cmbVal=cmbVal+'~'+'Selected'+'~'+document.getElementById("hdnGroupID").value;
                       }
               }
            }
            else
            {
                cmbVal=document.getElementById('cmbsearchOption').value;
                cmbVal=cmbVal+'~'+document.getElementById('ddlgrouptype').value;
            }
//          }         
          ajax_showOptions(objID,objListFun,objEvent,cmbVal);
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
            function DeliverableValue(objVal,StockPrevVal)
            {
                if(parseFloat(StockPrevVal)<parseFloat(objVal.value))
                {
                    alert('You Can Transfer Maximum '+StockPrevVal+' Amount');
                    objVal.value=StockPrevVal;
                }
            }
            function SelectAll(id) 
            {
                var frm = document.forms[0];
                var val;
                for (i=0;i<frm.elements.length;i++) 
                {
                    if(frm.elements[i].type == "text")
                    {
                        val='';
                        val=frm.elements[i].value
                    }
                    if (frm.elements[i].type == "checkbox") 
                    {  
                        if(val!='')
                            frm.elements[i].checked = document.getElementById(id).checked;
                    }
                }
            }
        function AfterShow(obj)
        {  
            if(obj=='a')
            {
                document.getElementById('TrAll').style.display='none';
                document.getElementById('TrFilter').style.display='inline';
                document.getElementById('TrTransfer').style.display='inline';
            }
            else
            {
                alert('No Record Found !!');
                document.getElementById('TrAll').style.display='inline';
                document.getElementById('TrFilter').style.display='none';
                document.getElementById('TrTransfer').style.display='none';
            }
            height();
        }
        function ForFilter()
        {
            document.getElementById('TrAll').style.display='inline';
            height();
        }
        
       
    
    </script>

    <script type="text/ecmascript">
        
	    function ReceiveServerData(rValue)
        {
            document.getElementById("hdnGroupID").value=rValue.split('~')[1];
            groupvalue=rValue.split(':')[1];
        } 
        
        
        function DateChange(positionDate)
        {
        
        var FYS='<%=Session["FinYearStart"]%>';
        var FYE='<%=Session["FinYearEnd"]%>';
        var LFY='<%=Session["LastFinYear"]%>';
        DevE_CheckForFinYear(positionDate,FYS,FYE,LFY);
        }  
        
        function PageLoad()
        {
        
         var FYE='<%=Session["FinYearEnd"]%>';
          var FYS='<%=Session["FinYearStart"]%>';
         cdtDate.SetDate(new Date(FYE));
         cdtTransaction.SetDate(new Date());
       var FYS='<%=Session["FinYearStart"]%>';
//       DevE_CompareDateToMax(cdtTransaction,FYS,'Transfer Date Is Outside Of Financial Year !!');
       DevE_CompareDateToMax(cdtTransaction,FYS);
        } 
   
    </script>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="36000">
        </asp:ScriptManager>

        <script language="javascript" type="text/javascript">
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
        </script>
        <table class="TableMain100">
                <tr>
                    <td class="EHEADER" style="text-align: center;">
                        <strong><span style="color: #000099">InterSegment Transfer</span></strong>
                    </td>
                </tr>
            </table>
        <table class="TableMain100">
            <tr id="TrAll">
                <td>
                    <table border="1">
                        <tr>
                            <td style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                &nbsp;Source Segment</td>
                            <td>
                                <asp:DropDownList ID="cmbSourceSeg" runat="server" Width="220px" OnSelectedIndexChanged="cmbSourceSeg_SelectedIndexChanged"
                                    AutoPostBack="true" Enabled="false">
                                </asp:DropDownList>
                            </td>
                            <td style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left;">
                                &nbsp;Target Segment</td>
                            <td style="width: 218px">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="cmbTargetSeg" runat="server" Width="220px">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbSourceSeg" EventName="SelectedIndexChanged">
                                        </asp:AsyncPostBackTrigger>
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                &nbsp;Source A/c</td>
                            <td>
                                <asp:DropDownList ID="cmbSourceAc" runat="server" Width="220px">
                                </asp:DropDownList>
                            </td>
                            <td style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left;">
                                &nbsp;Target A/c</td>
                            <td style="width: 218px">
                                <asp:DropDownList ID="cmbTargetAc" runat="server" Width="220px">
                                    <asp:ListItem Value="SYSTM00001">Clients - Trading A/c</asp:ListItem>
                                    <asp:ListItem Value="SYSTM00002">Clients - Margin Deposit A/c</asp:ListItem>
                                    <asp:ListItem Value="SYSTM00003">Clients - FDR A/c</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                &nbsp;Balance As On</td>
                            <td colspan="3">
                                <dxe:ASPxDateEdit ID="dtDate" runat="server" ClientInstanceName="cdtDate" DateOnError="ToDay"
                                 UseMaskBehavior="True" EditFormat="Custom" EditFormatString="dd-MM-yyyy">
                                 <ClientSideEvents DateChanged="function(s,e){DateChange(cdtDate);}"></ClientSideEvents>
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                &nbsp;Transfer Date</td>
                            <td colspan="3">
                                <dxe:ASPxDateEdit ID="dtTransaction" runat="server" ClientInstanceName="cdtTransaction" DateOnError="Today"
                                 UseMaskBehavior="True" EditFormat="Custom" EditFormatString="dd-MM-yyyy">
                                 <ClientSideEvents DateChanged="function(s,e){DateChange(cdtTransaction);}"></ClientSideEvents>
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <table border="1">
                                    <tr>
                                        <td class="gridcellleft" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                            Group By</td>
                                        <td>
                                            <asp:DropDownList ID="ddlGroup" runat="server" Width="80px" Font-Size="12px" onchange="fnddlGroup(this.value)">
                                                <asp:ListItem Value="0">Branch</asp:ListItem>
                                                <asp:ListItem Value="1">Group</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="2" id="td_branch">
                                            <table border="1">
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rdbranchAll" runat="server" Checked="True" GroupName="a" onclick="AllSelct('a','B')" />
                                                        All
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbranchSelected" runat="server" GroupName="a" onclick="AllSelct('b','B')" />Selected
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
                                                            onclick="AllSelct('a','G')" />
                                                        All
                                                        <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="b" onclick="AllSelct('b','G')" />Selected
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                Client :</td>
                            <td>
                                <table border="1">
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdbClientALL" runat="server" Checked="True" GroupName="c" onclick="AllSelct('a','C')" /></td>
                                        <td>
                                            All Client</td>
                                        <td>
                                            <asp:RadioButton ID="radPOAClient" runat="server" GroupName="c" onclick="AllSelct('a','C')" />
                                        </td>
                                        <td>
                                            POA Client</td>
                                        <td>
                                            <asp:RadioButton ID="rdbClientSelected" runat="server" GroupName="c" onclick="AllSelct('b','C')" /></td>
                                        <td>
                                            Selected Client</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <table border="1">
                                    <tr>
                                        <td class="gridcellleft" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                            &nbsp;Consider Amout Greater Than Or Equal To</td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtStartAmount" Font-Size="12px" Width="130px" runat="server" Onkeypress="javascript:return isNumberKey(event);"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <table>
                                    <tr>
                                        <td class="gridcellleft" style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                            &nbsp;Naration</td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtNarration" Font-Size="12px" Width="432px" runat="server" TextMode="MultiLine" Height="50px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                Transfer</td>
                            <td colspan="3">
                                <table border="1">
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdAllDebit" runat="server" GroupName="f" Checked="true" /></td>
                                        <td style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                            All Debit</td>
                                        <td>
                                            <asp:RadioButton ID="rdAllCredit" runat="server" GroupName="f" />
                                        </td>
                                        <td style="vertical-align: top; height: 11px; background-color: #b7ceec; text-align: left">
                                            All Credit</td>
                                        <td>
                                            <asp:RadioButton ID="rdOffBal" runat="server" GroupName="f" Enabled="false" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label14" runat="server" Text="Offsetting Balance" CssClass="mylabel1"
                                                Enabled="false"></asp:Label></td>
                                        <td>
                                            <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="btnUpdate" OnClick="btnShow_Click" />
                                        </td>
                                        <td>
                                            
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top" id="TdFilter" style="display:none">
                    <table border=1>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtsegselected" runat="server" Width="128px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox>
                                <asp:HiddenField ID="txtsegselected_hidden" runat="server" />
                                <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                    style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                        style="color: #009900; font-size: 8pt;">&nbsp;</span>
                            </td>
                            <td id="TdFilter1" style="height: 23px">
                                <asp:DropDownList ID="cmbsearchOption" runat="server" Width="85px" Font-Size="11px"
                                    Enabled="false">
                                    <asp:ListItem>Clients</asp:ListItem>
                                    <asp:ListItem>Branch</asp:ListItem>
                                    <asp:ListItem>Group</asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>
                    </table>
                    <table cellpadding="0" cellspacing="0" id="TdSelect" border=1>
                        <tr>
                            <td>
                                <asp:ListBox ID="lstSuscriptions" runat="server" Font-Size="12px" Height="90px" Width="253px">
                                </asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center">
                                <table cellpadding="0" cellspacing="0" border=1>
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
                        <tr>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="text-align: left">
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                        <ProgressTemplate>
                            <div id='Div2' style='position: absolute; font-family: arial; font-size: 30; background-color: white;
                                layer-background-color: white;'>
                                <table border='1' cellpadding='0' cellspacing='0' bordercolor='#C0D6E4'>
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
                <td id="TrFilter" colspan="4" style="text-align: right">
                    <span style="font-weight: bold; color: Blue; cursor: pointer" onclick="javascript:ForFilter();">
                        Filter </span>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="grdInterSegmentTransfer" runat="server" Width="100%" BorderColor="CornflowerBlue"
                                ShowFooter="True" AllowSorting="true" AutoGenerateColumns="false" BorderStyle="Solid"
                                BorderWidth="2px" CellPadding="4" ForeColor="#0000C0" OnRowCreated="grdInterSegmentTransfer_RowCreated"
                                OnRowDataBound="grdInterSegmentTransfer_RowDataBound" OnSorting="grdInterSegmentTransfer_Sorting">
                                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                                <Columns>
                                    <asp:TemplateField Visible="False">
                                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Center"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubAccID" runat="server" Text='<%# Eval("SubAccID")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Branch Name" SortExpression="BranchName">
                                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblBranchName" runat="server" Text='<%# Eval("BranchName")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Client Name" SortExpression="ClientName">
                                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblClientName" runat="server" Text='<%# Eval("ClientName")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="BenID">
                                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblBenID" runat="server" Text='<%# Eval("BenID")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCredit1" runat="server" Text='<%# Eval("Credit1")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDebit" runat="server" Text='<%# Eval("Debit1")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Transfer">
                                        <ItemStyle Font-Size="12px" BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtStock" Font-Size="12px" Width="130px" runat="server" Text='<%# Eval("Credit") %>'
                                                Onkeypress="javascript:return isNumberKey(event);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="False">
                                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Center"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblBranchID" runat="server" Text='<%# Eval("BranchID")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="False">
                                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCredit" runat="server" Text='<%# Eval("Credit")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                   
                                    <asp:TemplateField>
                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkDelivery" runat="server" />
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="cbSelectAll" runat="server" />
                                        </HeaderTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle BackColor="White" ForeColor="#330099" BorderColor="#BFD3EE" BorderStyle="Double"
                                    BorderWidth="1px"></RowStyle>
                                <EditRowStyle BackColor="#E59930"></EditRowStyle>
                                <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
                                <PagerStyle ForeColor="White" HorizontalAlign="Center"></PagerStyle>
                                <HeaderStyle ForeColor="Black" BorderWidth="1px" CssClass="EHEADER" BorderColor="AliceBlue"
                                    Font-Bold="False"></HeaderStyle>
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr id="TrTransfer">
                <td colspan="2">
                    <asp:Button ID="btnSave" runat="server" Text="Transfer" CssClass="btnUpdate" OnClick="btnSave_Click" />
                </td>
            </tr>
            <tr>
                <td  style="display: none">
                    <asp:Button ID="btnhide" runat="server" Text="Button" OnClick="btnhide_Click"/>
                    <asp:HiddenField ID="hdnGroupID" runat="Server" />
                </td>
            </tr>
        </table>
</asp:Content>
