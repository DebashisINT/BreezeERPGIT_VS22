<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frm_DatewiseClientMarginReport" Codebehind="frm_DatewiseClientMarginReport.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
         <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />
        <script type="text/javascript" src="/assests/js/init.js"></script>
    <script type="text/javascript" src="/assests/js/ajaxList_rootfile.js"></script>
    
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
	  .grid_scroll
    {
        overflow-y: no;  
        overflow-x: scroll; 
        width:90%;
        scrollbar-base-color: #C0C0C0;
    
    }
	</style>
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
                   //* btn.click();
                }  
                if(j[0]=='Clients')
                {
                    document.getElementById('HiddenField_Client').value = j[1];
                } 
                if(j[0]=='BranchGroup')
                {
                    document.getElementById('HiddenField_BranchGroup').value = j[1];
                }
                if(j[0]=='Product')
                {
                    document.getElementById('HiddenField_Product').value = j[1];
                }
                 if(j[0]=='MAILEMPLOYEE')
                {
                    document.getElementById('HiddenField_emmail').value = j[1];
                }
        }
        </script>
	
     <script language="javascript" type="text/javascript">
     var FieldName=null;
    function Page_Load()
    {
//        document.getElementById('btn_show').style.display='inline';
//        document.getElementById('btngenerate').style.display='none';
//        document.getElementById('tr_grd').style.display='none';
        document.getElementById('tr_export').style.display='none';
         document.getElementById('showfilter').style.display='none';
        txtUnApproved.SetValue('100.00');
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
     function dateassign(s)
         {
        
                 var date1 = dtdate.GetDate();
                 if(date1 != null) 
                 {
                  var date2 =parseInt(date1.getMonth()+1)+'-'+date1.getDate()+'-'+ date1.getFullYear();
                  var prevdate2=parseInt(date1.getMonth()+1)+'-'+(date1.getDate()-1)+'-'+ date1.getFullYear();
                  
                   dtcdate.SetDate(new Date(prevdate2));
                   dthdate.SetDate(new Date(date2));
                   dttbdate.SetDate(new Date(prevdate2));
                   dtmcdate.SetDate(new Date(date2));
                 }
                document.getElementById('btnhide').click();
         }
     function norecord()
     {
        document.getElementById('tr_export').style.display='none';
        document.getElementById('showFilter').style.display='none';
        document.getElementById('tabResult').style.display='none';
        alert('No Record Found !');
        height();
        
     }
     function display()
     {
        document.getElementById('btn_show').style.display='none';
        document.getElementById('btngenerate').style.display='inline';
        document.getElementById('tr_export').style.display='inline';
        document.getElementById('tr_grd').style.display='inline';
       height();
        
     }
        function ChangeRowColor(rowID) 
        {
            var gridview = document.getElementById('grdmarginreporting'); 
            var rCount = gridview.rows.length; 
            var rowIndex=1; 
            for (rowIndex; rowIndex<=rCount-2; rowIndex++) 
            { 
                var rowElement = gridview.rows[rowIndex]; 
                rowElement.style.backgroundColor='#FFFFFF'
            }
            var color = document.getElementById(rowID).style.backgroundColor;
            if(color != '#ffe1ac') 
            {
                oldColor = color;
            }
            if(color == '#ffe1ac') 
            {
                document.getElementById(rowID).style.backgroundColor = oldColor;
            }
            else 
                document.getElementById(rowID).style.backgroundColor = '#ffe1ac'; 

            
        }
     function fnchkUnApprovedShares(obj)
      {
        if(obj.checked==true)
        {
            document.getElementById('txtUnApprovedShares_I').disabled=false;
        }
        else
        {
            document.getElementById('txtUnApprovedShares_I').disabled=true;
        }
      }
     function fnchktxtchk(obj)
     {
       if(obj>100.00)
       {
            txtUnApproved.SetValue('100.00');
       }
     }
 	function selecttion()
      {
         var combo=document.getElementById('ddlExport');
         combo.value='Ex';
      } 
      
       function  fnddlGroup(obj)
        {
        
            if(obj=="0" || obj=="2")
            {
                Hide('td_group');
                Show('td_branch');
                Hide('showfilter');
            }
            else
            {
                Show('td_group');
                Hide('td_branch');
                Hide('showfilter');
//                var btn = document.getElementById('btnhide');
//                btn.click();
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
        
        function Show(obj)
            {
                document.getElementById(obj).style.display='inline';
            
            }
        function Hide(obj)
            {
                document.getElementById(obj).style.display='none';
            
            }
         function filter()
            {
                 document.getElementById('tabFilter').style.display='inline';
                 document.getElementById('tr_export').style.display='none';       
                 document.getElementById('showfilter').style.display='none';         
                 document.getElementById('tabResult').style.display='none';
                 document.getElementById('rdbranchAll').checked=true;
                 selecttion();    
                 var ddlgroup= document.getElementById('ddlGroup');
                 ddlgroup.value='0';
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
                      Show('divselectbranches');
                      Hide('divselectclient');
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
                 Show('divselectbranches');
                 Hide('divselectclient');
             }
             selecttion();
             height();
      }
        
          function btnAddsubscriptionlist_click()
            {
            
                var cmb=document.getElementById('cmbsearchOption');
                        var userid;
                        var hiddenid;
                        if(cmb.value=='Clients')
                            {
                                userid = document.getElementById('txtSelectClients'); 
                                hiddenid='txtSelectClients_hidden';
                            }
                         else
                            {
                                userid = document.getElementById('txtSelectionID');
                                hiddenid='txtSelectionID_hidden';
                            }
                            
                        if(userid.value != '')
                        {
                            var ids = document.getElementById(hiddenid);
                            var listBox = document.getElementById('lstSlection');
                            var tLength = listBox.length;
                           
                            
                            var no = new Option();
                            no.value = ids.value;
                            no.text = userid.value;
                            listBox[tLength]=no;
                           // var recipient = document.getElementById(userid);
                            userid.value='';
                        }
                        else
                            alert('Please search name and then Add!')
                            
                        var s=document.getElementById(userid);
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
            
           function Clients(obj)
             {
            if(obj=="a")
                Hide('showFilter');
             else
             {
//                  if(document.getElementById('ddlGroup').value=="0")
//                   {
//                    document.getElementById('cmbsearchOption').value='Branch';
//                   }
//                   if(document.getElementById('ddlGroup').value=="2")
//                   {
//                    document.getElementById('cmbsearchOption').value='BranchGroup';
//                   }
                    document.getElementById('cmbsearchOption').value='Clients';
                  Show('showFilter');
                  Hide('divselectbranches');
                  Show('divselectclient');            
                  
             }
             selecttion();
                height();
              }
        
         function GetClients(obj1,obj2,obj3)
          {
            var segname= document.getElementById('hiddenSegmentName').value;
            var compname=document.getElementById('hiddenCompany').value;
           var strQuery_Table = "tbl_master_contact ,tbl_master_contactexchange,tbl_master_branch";
           var strQuery_FieldName = " top 10 rtrim(ltrim(isnull(cnt_firstname,'')))+ ' ' + rtrim(ltrim(isnull(cnt_middlename,'')))+' '+ rtrim(ltrim(isnull(cnt_lastname,''))) +\' [ \'+rtrim(ltrim(isnull(cnt_ucc,'')))+\' ]\' + ' [ ' + rtrim(ltrim(isnull(branch_description,''))) + ' ]' as Clientname,rtrim(ltrim(cast(cnt_internalid as varchar))) as Clientid";
           var strQuery_WhereClause = " cnt_internalid=crg_cntid and cnt_branchid=branch_id and (cnt_firstname like (\'%RequestLetter%\') or cnt_ucc like (\'%RequestLetter%\')) and  crg_company='" + compname + "' and crg_exchange='" + segname + "'" ;
           var strQuery_OrderBy='';
           var strQuery_GroupBy='';
           var CombinedQuery=new String(strQuery_Table+"$"+strQuery_FieldName+"$"+strQuery_WhereClause+"$"+strQuery_OrderBy+"$"+strQuery_GroupBy);
           
           ajax_showOptions(obj1,obj2,obj3,replaceChars(CombinedQuery),'Main'); 
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
        function SetValueforclient(val1,val2,val3)
            {
               
                document.getElementById('hiddenSegmentName').value=val1;
                document.getElementById('hiddenCompany').value=val2;           
                document.getElementById('hiddenUserbranchierarchy').value=val3;    
            
            }
            
        function getBranches(obj1,obj2,obj3)
            {
               
               if(document.getElementById('cmbsearchOption').value=='Branch')
                {
               
                   var userbranchhierarchy=document.getElementById('hiddenUserbranchierarchy').value;
                   var strQuery_Table = "tbl_master_branch";
                   var strQuery_FieldName = " top 10 rtrim(ltrim(isnull(branch_description,'')))+ ' ' + rtrim(ltrim(isnull(branch_code,''))) as BranchDesc,rtrim(ltrim(cast(branch_id as varchar))) as Branchid";
                   var strQuery_WhereClause = " (branch_description like (\'%RequestLetter%\') or branch_code like (\'%RequestLetter%\')) and branch_id in(" + userbranchhierarchy + ")" ;
                   var strQuery_OrderBy='';
                   var strQuery_GroupBy='';
                   var CombinedQuery=new String(strQuery_Table+"$"+strQuery_FieldName+"$"+strQuery_WhereClause+"$"+strQuery_OrderBy+"$"+strQuery_GroupBy);
                   
                   ajax_showOptions(obj1,obj2,obj3,replaceChars(CombinedQuery),'Main'); 
                }
               else if(document.getElementById('cmbsearchOption').value=='Group')
                {
                   var grouptype=document.getElementById('ddlgrouptype').value;
                   var strQuery_Table = "tbl_master_groupmaster";
                   var strQuery_FieldName = " top 10 rtrim(ltrim(isnull(gpm_description,'')))+ ' ' + rtrim(ltrim(isnull(gpm_code,''))) as Groupdesc,rtrim(ltrim(cast(gpm_id as varchar))) as Groupid";
                   var strQuery_WhereClause = " (gpm_description like (\'%RequestLetter%\') or gpm_code like (\'%RequestLetter%\')) and gpm_type='" + grouptype + "'" ;
                   var strQuery_OrderBy='';
                   var strQuery_GroupBy='';
                   var CombinedQuery=new String(strQuery_Table+"$"+strQuery_FieldName+"$"+strQuery_WhereClause+"$"+strQuery_OrderBy+"$"+strQuery_GroupBy);
                   
                   ajax_showOptions(obj1,obj2,obj3,replaceChars(CombinedQuery),'Main'); 
                }
                 else if(document.getElementById('cmbsearchOption').value=='BranchGroup')
                {
                   //var userbranchhierarchy=document.getElementById('hiddenUserbranchierarchy').value;
                   var strQuery_Table = "master_branchgroups";
                   var strQuery_FieldName = " top 10 rtrim(ltrim(isnull(BranchGroups_Name,'')))+ ' [' + rtrim(ltrim(isnull(BranchGroups_Code,''))) +']' as BranchGroupDesc,rtrim(ltrim(cast(BranchGroups_ID as varchar))) as BranchGroupid";
                   var strQuery_WhereClause = " (BranchGroups_Name like (\'%RequestLetter%\') or BranchGroups_Code like (\'%RequestLetter%\')) " ;
                   var strQuery_OrderBy='';
                   var strQuery_GroupBy='';
                   var CombinedQuery=new String(strQuery_Table+"$"+strQuery_FieldName+"$"+strQuery_WhereClause+"$"+strQuery_OrderBy+"$"+strQuery_GroupBy);
                   
                   ajax_showOptions(obj1,obj2,obj3,replaceChars(CombinedQuery),'Main'); 

                
                }
            
            }
        
        </script>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
            <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3600">
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
         document.getElementById('btn_show').disabled=true;
   } 

   function EndRequest(sender, args) 
   { 
        $get('UpdateProgress1').style.display = 'none'; 
          document.getElementById('btn_show').disabled=false;
   } 

  function FunClientScrip(objID,objListFun,objEvent)
        {
        
            var combo = document.getElementById("cmbsearchOption");
            if(combo.value=='Clients')
                {
                    
                
                
                }
             else
                {
            
                  var cmbVal;
                 
                  cmbVal=document.getElementById('cmbsearchOption').value;
                  cmbVal=cmbVal+'~'+document.getElementById('ddlgrouptype').value;
                  
                  
                  ajax_showOptions(objID,objListFun,objEvent,cmbVal);
                }
        }
        
      

   </script>

    <div>
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" style="text-align: center;" colspan="2">
                    <strong><span style="color: #000099">Client Margin Reporting Datewise</span></strong>
                </td>
                <td class="EHEADER" width="15%" id="tr_export" style="height: 22px">
                 <a href="javascript:void(0);" onclick="filter();"><span
                        style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight:bold ">Filter</span></a>
                <asp:DropDownList ID="ddlExport" Font-Size="Smaller"  runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged" >
                            <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                            <asp:ListItem Value="E">Excel</asp:ListItem>
                            <asp:ListItem Value="P">PDF</asp:ListItem>
                        </asp:DropDownList> 

             </td>
            </tr>
        </table>
             
        <table id="tabFilter">
            <tr>
                <td>
                    <table border="10" cellpadding="1" cellspacing="1">
                        <tr>
                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                From :</td>
                            <td>
                                <dxe:ASPxDateEdit ID="dtdate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Font-Size="12px" Width="108px" EditFormatString="dd-MM-yyyy" ClientInstanceName="dtdate">
                                    <DropDownButton Text="For">
                                    </DropDownButton>
                                    <ClientSideEvents ValueChanged="function(s, e) {dateassign(s);}" />
                                </dxe:ASPxDateEdit>
                            </td>
                            <td>
                                To :
                            </td>
                             <td>
                                <dxe:ASPxDateEdit ID="dtToDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Font-Size="12px" Width="108px" EditFormatString="dd-MM-yyyy" ClientInstanceName="dtToDate1">
                                    <DropDownButton Text="For">
                                    </DropDownButton>
                                    <ClientSideEvents ValueChanged="function(s, e) {dateassign(s);}" />
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>
                    </table>
                </td>
                <td rowspan="5">
                            <table id="showFilter" border="10" cellpadding="1" cellspacing="1">
                                      <tr>
                                                    <td class="gridcellleft" style="vertical-align: top; text-align: right; height: 21px;"
                                                        id="TdFilter">
                                                     
                                                      <div id="divselectbranches"> <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="getBranches(this,'GenericAjaxList',event)"></asp:TextBox></div>
                                                      <div id="divselectclient"> <asp:TextBox ID="txtSelectClients" runat="server" Font-Size="12px" Width="150px" onkeyup="GetClients(this,'GenericAjaxList',event)"></asp:TextBox></div>
                                                        <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                            Enabled="false">
                                                             <asp:ListItem>Branch</asp:ListItem>
                                                            <asp:ListItem>Clients</asp:ListItem>
                                                            <asp:ListItem>Group</asp:ListItem>
                                                            <asp:ListItem>Product</asp:ListItem>
                                                            <asp:ListItem>BranchGroup</asp:ListItem>
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
            
              <tr>
                <td>
                     <table border="10" cellpadding="1" cellspacing="1">
                                                <tr>
                                                    <td class="gridcellleft" bgcolor="#B7CEEC">
                                                        Selected By</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlGroup" runat="server" Width="100px" Font-Size="12px" onchange="fnddlGroup(this.value)">
                                                            <asp:ListItem Value="0">Branch</asp:ListItem>
                                                            <asp:ListItem Value="1">Group</asp:ListItem>
                                                            <asp:ListItem Value="2">Branch Group</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td id="td_branch">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbranchAll" runat="server" Checked="True" GroupName="a" onclick="Branch('a')" />
                                                                    All
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rdbranchSelected" runat="server" GroupName="a" onclick="Branch('b')" />Selected
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td id="td_group" style="display:none" >
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                            <asp:UpdatePanel ID="updatePanelGroup" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                            <asp:DropDownList ID="ddlgrouptype" runat="server" Font-Size="12px" onchange="fngrouptype(this.value)">
                                                                            </asp:DropDownList>
                                                                            </ContentTemplate>
                                                                           
                                                                            </asp:UpdatePanel>
                                                                        
                                                                </td>
                                                                <td id="td_allselect" style="display: none;">
                                                                    <asp:RadioButton ID="rdddlgrouptypeAll" runat="server" Checked="True" GroupName="b"
                                                                        onclick="Group('a')" />
                                                                    All
                                                                    <asp:RadioButton ID="rdddlgrouptypeSelected" runat="server" GroupName="b" onclick="Group('b')" />Selected
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                        </table>
                    
                </td>
                <td >
                    
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td bgcolor="#B7CEEC">
                                Group By :
                            </td>
                            <td id="tdBranchFilter">
                                <asp:DropDownList ID="drpBranchFilter" runat="server">
                                    <asp:ListItem Text="Date+Client" Value="dc"></asp:ListItem>
                                    <asp:ListItem Text="Client+Date" Value="cd"></asp:ListItem>
                                    <asp:ListItem Text="Datewise Consolidated" Value="d"></asp:ListItem>
                                    <asp:ListItem Text="Clientwise Consolidated" Value="c"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td id="tdGroupFilter">
                                
                            </td>
                            <td id="tdBranchGroupFilter">
                                
                            </td>
                        </tr>
                    </table>       
                </td>
                <td>
                </td>
                
            </tr>
             <tr>
                <td>
                    <table>
                        <tr>
                            <td bgcolor="#B7CEEC">
                                Select Client :
                            </td>
                           <td>
                                <asp:RadioButton ID="rbClientAll" Checked="true" GroupName="cl" onclick="Clients('a')" runat="Server" />All
                                <asp:RadioButton ID="rbClientSelected" GroupName="cl" onclick="Clients('b')" runat="Server" />Selected
                           </td>
                        </tr>
                    </table>   
                 </td>
                <td>
                
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <table border="10" cellpadding="1" cellspacing="1">
                        <tr>
                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                Collateral Valuation Day :</td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="drpCollatValueDay" runat="Server">
                                                 <asp:ListItem Text="T" Value="T"></asp:ListItem>
                                                 <asp:ListItem Text="T-1" Value="T-1"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div id="display" runat="server" style="border: solid 1px black">
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnhide" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td></td>
            </tr>
          
       <tr>
                <td colspan="2">
                   <table><tr><td><table border="10" cellpadding="1" cellspacing="1">
                        <tr>
                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                Collateral Holding Day :</td>
                            <td>
                                <asp:DropDownList ID="drpCollatHoldingDay" runat="Server">
                                    <asp:ListItem Text="T" Value="T"></asp:ListItem>
                                    <asp:ListItem Text="T-1" Value="T-1"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table></td><td> <table border="10" cellpadding="1" cellspacing="1">
                        <tr>
                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                Trading Account Balance Day :</td>
                            <td>
                                <asp:DropDownList ID="drpTradeAccBalanceDay" runat="server">
                                    <asp:ListItem Text="T" Value="T"></asp:ListItem>
                                    <asp:ListItem Text="T-1" Value="T-1"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table></td><td><table border="10" cellpadding="1" cellspacing="1">
                        <tr>
                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                Margin and FDR Account Balance Day :</td>
                            <td>
                                <asp:DropDownList ID="drpMarginFDRDay" runat="Server">
                                    <asp:ListItem Text="T" Value="T"></asp:ListItem>
                                    <asp:ListItem Text="T-1" Value="T-1"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table></td></tr></table>
                </td>
            </tr>
              <tr>
                <td colspan="2">
                    <table border="10" cellpadding="1" cellspacing="1">
                        <tr valign="top">
                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                <asp:CheckBox ID="chkhaicut"  runat="server" />
                                Do Not Apply Haircut
                            </td>
                             <td class="gridcellleft" bgcolor="#B7CEEC">
                                <asp:CheckBox ID="chkcmsegment" runat="server"  />
                                Consider CM Segment Trading A/c Balance
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            Initial Margin</td>
                                        <td>
                                            <asp:DropDownList ID="DdlmarginType" runat="server" Width="150px" Font-Size="12px">
                                                <asp:ListItem Value="SI">Span+Premium</asp:ListItem>
							     <asp:ListItem Value="S">Span</asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table  border="10" cellpadding="1" cellspacing="1">
                        <tr id="tr_UnApprovedShares">
                            <td class="gridcellleft" bgcolor="#B7CEEC">
                                <asp:CheckBox ID="chkUnApprovedShares"  Checked="true" runat="server" onclick="fnchkUnApprovedShares(this)" />
                                Haircut for Unapproved Shares</td>
                            <td>
                                <dxe:ASPxTextBox ID="txtUnApprovedShares" ClientInstanceName="txtUnApproved" runat="server"
                                    HorizontalAlign="Right" Width="100px">
                                    <MaskSettings Mask="&lt;0..199g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                    <ValidationSettings ErrorDisplayMode="None">
                                    </ValidationSettings>
                                    <ClientSideEvents LostFocus="function(s, e) {
	                            fnchktxtchk(s.GetValue())
                            }" />
                                </dxe:ASPxTextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btn_show" runat="server" CssClass="btnUpdate" Height="20px" Text="Show"
                                    Width="101px" OnClick="btn_show_Click" OnClientClick="selecttion()"/></td>
                            <td>
                                <asp:Button ID="btngenerate" runat="server" CssClass="btnUpdate" Height="20px" Text="Export To Excel"
                                    Width="101px" OnClick="btngenerate_Click"  OnClientClick="selecttion()"/></td>
                        </tr>
                        <tr>
                            <td>
                                <div style="display:none">
                                                                  <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                                                                  <asp:TextBox ID="txtSelectClients_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                                                                 <asp:HiddenField ID="HiddenField_Group" runat="server" />
                                                                <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                                                                 <asp:HiddenField ID="HiddenField_BranchGroup" runat="server" />
                                                                 <asp:HiddenField ID="HiddenField_Client" runat="Server" />
                                                                 <asp:HiddenField ID="hiddenSegmentName" runat="Server" />
                                                                 <asp:HiddenField ID="hiddenCompany" runat="server" />
                                                                  <asp:HiddenField ID="hiddenUserbranchierarchy" runat="server" />
                                                                 
                                                                 </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
         
           
        </table>
        
        <table>
            <tr>
                <td style="display: none;">
                 
                            <asp:Button ID="btnhide" runat="server" Text="Button"
                                OnClick="btnhide_Click" />
                              
</td> </tr></table>
            <table id="tabResult">
                <tr>
                    <td>
                       <asp:UpdatePanel ID="updatepanelShow" runat="Server">
                            <ContentTemplate>
                                <div id="divShow" runat="server"></div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
    
    </div>
</asp:Content>
