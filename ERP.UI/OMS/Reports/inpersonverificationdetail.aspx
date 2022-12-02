<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_inpersonverificationdetail"  EnableEventValidation="false" Codebehind="inpersonverificationdetail.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />
        <script type="text/javascript" src="/assests/js/init.js"></script>
          
    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>
      <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>
    <script language="javascript" type="text/javascript">
   
    
   FieldName = 'txtSelectionID';
   function selecttion()
        {                   
            var combo=document.getElementById('cmbExport');         
             combo.value='0';
        }
   function SignOff()
    {
        window.parent.SignOff();
    }
    function height()
     {
  
        if(document.body.scrollHeight>=600)
            window.frameElement.height = document.body.scrollHeight;
        else
            window.frameElement.height = '700px';
        window.frameElement.Width = document.body.scrollWidth;
    }
    function ValidatePage()
     {

   selecttion();
//      if(document.getElementById("txtClientID").value=='')
//      {
//         alert('Please Type Client ID!..');
//         return false;
//      }

    }
    
    
     
   function ShowHideFilter(obj)
    {
           grid.PerformCallback(obj);
    } 

    
//    function SearchOpt(obj)
//    {
//   
//         var cmbt=document.getElementById('cmbDuplicate');
//         if(cmbt.value == 'None')
//         {
//             document.getElementById("txtClientID").style.display="inline";
//            document.getElementById("TrFilter").style.display="none"
//         }
//         else if (cmbt.value == 'PANEXEMPT') 
//         {
//             document.getElementById("txtClientID").style.display="none";
//             document.getElementById("TrFilter").style.display="inline";
//             
//         
//         }
//         else
//         {
//         document.getElementById("txtClientID").style.display="none";
//             document.getElementById("TrFilter").style.display="none";
//         }
//     
//    }
//    function ShowHideFilter(obj)
//    {
//         if(document.getElementById('TxtSeg').value=='N')
//           {
//             document.getElementById('TxtTCODE').style.display="none";
//           }
//           else
//           {
//            document.getElementById('TxtTCODE').style.display="inline";
//           }
//        InitialTextVal();
//        if(obj=="s")
//            document.getElementById('TrFilter').style.display="inline";
//        else
//        {
//            document.getElementById('TrFilter').style.display="none";
//            grid.PerformCallback(obj);
//        }
//    }
     function btnSearch_click()
   {
        document.getElementById('TrFilter').style.display="none";
        grid.PerformCallback('s');
   }  
//   function InitialTextVal()
//   {
//   

//        document.getElementById('txtName').value = "ADD1";
//        document.getElementById('txtBranchName').value = "ADD2";
//        document.getElementById('txtCode').value = "ADD3";
//        document.getElementById('txtRelationManager').value = "Landmark";
//        document.getElementById('txtReferedBy').value = "Country";
//        document.getElementById('txtPhNumber').value = "State";
//        document.getElementById('txtContactStatus').value = "City";
////        document.getElementById('txtStatus').value = "Status";
//        
//        document.getElementById('TxtTCODE').value = "Area";
//        //document.getElementById('txtPAN').value = "PAN No.";
//   }  
//    function ClearTextboxes()
//        {
//            document.getElementById('txtName').value = '';
//           
//            document.getElementById('txtBranchName').value = ''; 
//            document.getElementById('txtCode').value= '';
//            document.getElementById('TxtTCODE').value= '';
//            document.getElementById('txtPAN').value= '';
//            document.getElementById('txtRelationManager').value= '';
//            document.getElementById('txtReferedBy').value= '';
//            document.getElementById('txtPhNumber').value= '';           
        //}
        
         function Hide(obj)
            {
             document.getElementById(obj).style.display='none';
            }
    function Show(obj)
            {
             document.getElementById(obj).style.display='inline';
            }
            
   
    function  FnddlGroupBy(obj)
     {
        if(obj=='Group')
        {
            Hide('Td_OtherGroupBy');
            Show('Td_Group');
            document.getElementById('BtnGroup').click();
        }
        else
        {
            Show('Td_OtherGroupBy');
            Hide('Td_Group');
        }
     }
  function FnOtherGroupBy(obj)
    {
       if(obj=="a")
            Hide('Tab_showFilter');
         else
         {
             if(document.getElementById('ddlGroupBy').value=='Clients')
                 document.getElementById('cmbsearchOption').value='Clients';
             if(document.getElementById('ddlGroupBy').value=='Branch')
                 document.getElementById('cmbsearchOption').value='Branch';
             Show('Tab_showFilter');
         }
         
    }
 function FnGroup(obj)
  {
        if(obj=="a")
            Hide('Tab_showFilter');
         else
         {
              document.getElementById('cmbsearchOption').value='Group';
              Show('Tab_showFilter');
         }
        
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
            
                Hide('Tab_showFilter');
                document.getElementById('BtnGenerate').disabled=false;
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
            function btnShow_Click()
            {
            var cmbt=document.getElementById('cmbDuplicate');
         if(cmbt.value == 'Print')
                document.getElementById("tblBulk").style.display="inline";
            }
             function abcd(obj1,Obj2,obj3)
   {
       var strQuery_Table = '';
       var strQuery_FieldName = '';
       var strQuery_WhereClause = '';
       var strQuery_OrderBy='';
       var strQuery_GroupBy='';
       var CombinedQuery='';
       
         if(document.getElementById('cmbsearchOption').value=="Clients")
                    {
                       
                    //alert('456');
                    strQuery_Table = "tbl_master_contact,tbl_master_branch";
                       strQuery_FieldName = "distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_shortname),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(CNT_UCC),'')+'] ['+isnull(rtrim( BRANCH_DESCRIPTION),'')+']' ,cnt_internalID";
                        strQuery_WhereClause = "  branch_id=cnt_branchid  and (CNT_UCC Like (\'%RequestLetter%') or CNT_FIRSTNAME like (\'%RequestLetter%') OR CNT_SHORTNAME like (\'%RequestLetter%')) and cnt_branchid in (<%=Session["userbranchHierarchy"]%>)";
                    //}
                    
       }
       
       else if(document.getElementById('cmbsearchOption').value=="Branch")
                    {
                   
                       strQuery_Table = "tbl_master_branch";
                       strQuery_FieldName = "top 10 branch_description+'-'+branch_code,branch_id";
                       strQuery_WhereClause = " (branch_description Like (\'%RequestLetter%') or branch_code like (\'%RequestLetter%')) and branch_id in (<%=Session["userbranchHierarchy"]%>)";
                    }
                   
                    else if(document.getElementById('cmbsearchOption').value=="Group")
                    {
                    
                       strQuery_Table = "tbl_master_groupmaster";
                       strQuery_FieldName = "top 10 gpm_description+'-'+gpm_code ,gpm_id";
                       strQuery_WhereClause = " (gpm_description Like (\'%RequestLetter%') or gpm_code like (\'%RequestLetter%')) and gpm_type='"+document.getElementById('ddlGroup').value+"'";
                    }
                   
       
       CombinedQuery=new String(strQuery_Table+"$"+strQuery_FieldName+"$"+strQuery_WhereClause+"$"+strQuery_OrderBy+"$"+strQuery_GroupBy);
      
       ajax_showOptions(obj1,'GenericAjaxList',obj3,replaceChars(CombinedQuery));
    
   }
   function replaceChars(entry) 
   {
        out = "+"; 
        add = "--";
        temp = "" + entry; 

        while (temp.indexOf(out)>-1) {
        pos= temp.indexOf(out);
        temp = "" + (temp.substring(0, pos) + add + 
        temp.substring((pos + out.length), temp.length));
        }
        return temp;
    } 
    
     function SearchOpt(obj)
    {
  
         var cmbt=document.getElementById('cmbDuplicate');
         Hide('Tab_showFilter');
         Hide('Td_Group');
         
     }
     function Page_Load()///Call Into Page Load
            {
                 Hide('Tab_showFilter');
                 Hide('Td_Filter');
                 //document.getElementById('lblCurrentSettlementNo').innerText='<%=Session["LastSettNo"]%>';
                 FnSelection('1');
                 FnddlGeneration('1');
                 FnddlGroupBy(document.getElementById('ddlGroupBy').value);
                 height();
            }
            

    </script>
    
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
	<script type="text/ecmascript">   
       function ReceiveServerData(rValue)
        {
                var j=rValue.split('~');
                
                if(j[0]=='Branch')
                    document.getElementById('HiddenField_Branch').value = j[1];
                if(j[0]=='Group')
                    document.getElementById('HiddenField_Group').value = j[1];
                if(j[0]=='Clients')
                    document.getElementById('HiddenField_Client').value = j[1];
                
                
        }
        </script>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>
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
                         
                   } 
                   function EndRequest(sender, args) 
                   {
                     $get('UpdateProgress1').style.display = 'none';                         
                   } 
            </script>

            <table class="TableMain100">
                <tr>
                    <td class="EHEADER" style="text-align: center">
                        <strong><span style="color: #000099">Verification Details</span></strong></td>
                </tr>
                <tr>
                    <td>
                        <table cellspacing="0" cellpadding="0" style="background-color: #B7CEEC;border:solid 1px #ffffff"
                            border="0" width="60%">
                            <tr>
                                <td class="gridcellleft" width="150px">
                               <span style="font-weight:bold"> Inperson Verification </span>
                                </td>
                                <td class="gridcellleft" colspan="2">
                                    <asp:DropDownList ID="cmbDuplicate" runat="server"  onchange="SearchOpt(this.value)" OnSelectedIndexChanged="cmbDuplicate_SelectedIndexChanged" >
                                       <asp:ListItem Text="Veification Completed" Value="Y"></asp:ListItem>
                                    <asp:ListItem Text="Pending Verification" Value="N"></asp:ListItem>
                                    <asp:ListItem Text="Both" Value="B"></asp:ListItem>
                                    
                                    </asp:DropDownList>
                                                                 
                               
                                   
                                </td>
                                <td  class="gridcellright">
                                    <asp:Button ID="btnSave" runat="server" TabIndex="3" Text="Show" CssClass="btnUpdate"
                                        OnClick="btnSave_Click" />
                              
                                    <asp:Button ID="btnExport" runat="server" TabIndex="4" Text="Export to Excel" CssClass="btnUpdate"
                                        OnClick="btnExport_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                
                
               
               
       
               
                <tr>
                    <td class="gridcellleft">
                        <table border="10" cellpadding="1" cellspacing="1">
                            <tr id="tr_grp" runat="server">
                                <td class="gridcellleft" bgcolor="#B7CEEC">
                                    Group By</td>
                                <td>
                                    <asp:DropDownList ID="ddlGroupBy" runat="server" Width="180px" Font-Size="12px" onchange="FnddlGroupBy(this.value)">
                                            
                                          <asp:ListItem Value="Clients">Client</asp:ListItem>
                                         <asp:ListItem Value="Branch">Branch</asp:ListItem>
                                         <asp:ListItem Value="Group">Group</asp:ListItem>
                                         
                                    </asp:DropDownList>
                                </td>
                                <td class="gridcellleft" id="Td_OtherGroupBy">
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="RadioBtnOtherGroupByAll" runat="server" Checked="True" GroupName="a"
                                                    onclick="FnOtherGroupBy('a')" />
                                                All
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RadioBtnOtherGroupBySelected" runat="server" GroupName="a" onclick="FnOtherGroupBy('b')" />Selected
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RadioBtnOtherGroupByallbutSelected" runat="server" style="display:none" GroupName="a"
                                                    onclick="FnOtherGroupBy('c')" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="gridcellleft" id="Td_Group">
                                    <table border="10" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlGroup" runat="server" Font-Size="12px">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="BtnGroup" EventName="Click"></asp:AsyncPostBackTrigger>
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RadioBtnGroupAll" runat="server" Checked="True" GroupName="b"
                                                    onclick="FnGroup('a')" />
                                                All
                                                <asp:RadioButton ID="RadioBtnGroupSelected" runat="server" GroupName="b" onclick="FnGroup('b')" />Selected
                                                <asp:RadioButton ID="RadioBtnGroupallbutSelected" runat="server" Style="display:none" GroupName="b" onclick="FnGroup('c')" />
                                                
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                           
                            </tr>
                        </table>
                    </td>
                </tr>
                  <tr>
                    <td valign="top">
                        <table id="Tab_showFilter">
                            <tr>
                                <td valign="top">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="14px" Width="300px" onkeyup="abcd(this,'Other',event)"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                    Enabled="false">
                                                    <asp:ListItem>Clients</asp:ListItem>
                                                    <asp:ListItem>Branch</asp:ListItem>
                                                    <asp:ListItem>Group</asp:ListItem>
                                                    
                                                    
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <a id="A3" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                                    style="color: #2554C7; text-decoration: underline; font-size: 8pt;"><b>Add to List</b></span></a><span
                                                        style="color: #009900; font-size: 8pt;"> </span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ListBox ID="lstSlection" runat="server" Font-Size="12px" Height="100px" Width="400px">
                                    </asp:ListBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <a id="A5" href="javascript:void(0);" onclick="clientselectionfinal()"><span style="color: #000099;
                                                    text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
                                            </td>
                                            <td>
                                                <a id="A6" href="javascript:void(0);" onclick="btnRemovefromsubscriptionlist_click()">
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
                        <table>
                            <tr>
                                <td id="Td1" align="left">
                                    <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">
                                        Show Filter</span></a> || <a href="javascript:ShowHideFilter('All');"><span style="color: #000099;
                                            text-decoration: underline">All Records</span></a>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <div id='Div1' style='position: absolute; font-family: arial; font-size: 30; left: 50%;
                                    top: 8; background-color: white; layer-background-color: white; height: 80; width: 150;'>
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
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table class="TableMain100">
                                    <tr>
                                        <td>
                                            <dxe:ASPxGridView ID="gridContract" ClientInstanceName="grid" Width="100%" 
                                                runat="server" AutoGenerateColumns="False" OnCustomCallback="gridContract_CustomCallback">
                                                <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
                                                <Styles>
                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                    </Header>
                                                    <LoadingPanel ImageSpacing="10px">
                                                    </LoadingPanel>
                                                    <FocusedRow BackColor="#FEC6AB">
                                                    </FocusedRow>
                                                </Styles>
                                                <Columns>
                                                <dxe:GridViewDataTextColumn VisibleIndex="1"  Visible="false" FieldName="CustomerID" Caption="CustomerID">
                                                        <CellStyle Wrap="True" CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="ClientName" Caption="Name">
                                                        <CellStyle Wrap="True" CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                     <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="Ucc" Caption="Ucc">
                                                        <CellStyle Wrap="True" CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="branch" Caption="Branch">
                                                        <CellStyle Wrap="True" CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="PhoneNo" Caption="Phone">
                                                        <CellStyle Wrap="True" CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="Email" Caption="Email">
                                                        <CellStyle Wrap="True" CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="VerificationRemarks" Caption="Remarks">
                                                        <CellStyle Wrap="True" CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="VerificationDate" Caption="Verification Date">
                                                        <CellStyle Wrap="True" CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="VerificationName" Caption="Verification By">
                                                        <CellStyle Wrap="True" CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                   
                                                </Columns>
                                                <StylesEditors>
                                                    <ProgressBar Height="25px">
                                                    </ProgressBar>
                                                </StylesEditors>
                                                <SettingsBehavior AllowFocusedRow="True" AllowSort="False" AllowMultiSelection="True" />
                                                <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True"
                                                    PageSize="15">
                                                    <FirstPageButton Visible="True">
                                                    </FirstPageButton>
                                                    <LastPageButton Visible="True">
                                                    </LastPageButton>
                                                </SettingsPager>
                                                <SettingsText Title="Template" />
                                                <Settings ShowGroupedColumns="True" ShowGroupPanel="True" />
                                            </dxe:ASPxGridView>
                                            <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                                            </dxe:ASPxGridViewExporter>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                
            </table> 
            <table>   
            <tr style="display: none;">
                    <td>
                        <asp:Button ID="BtnGroup" runat="server" Text="BtnGroup" OnClick="BtnGroup_Click" />
                        <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                        <asp:TextBox ID="txtClientID1_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                        <asp:HiddenField ID="HiddenField_Group" runat="server" />
                        <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                        <asp:HiddenField ID="HiddenField_Client" runat="server" />
                    </td>
                </tr>
                </table>
        </div>
</asp:Content>
