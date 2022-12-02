<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frm_Report_ContactDetails" Codebehind="frm_Report_ContactDetails.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>

    

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

    <script language="javascript" type="text/javascript">
    function MainAll(obj1,obj2)
        {
            document.getElementById('cmbsearchOption').value=obj1;
            if(obj2=='all')
            {
                
                document.getElementById('TdFilter').style.visibility='hidden';
                document.getElementById('TdFilter1').style.visibility='hidden';
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
                document.getElementById('TdFilter').style.visibility='visible';
                document.getElementById('TdFilter1').style.visibility='visible';
               
            }
        }
        function height()
    {
    
        if(document.body.scrollHeight>=500)
            window.frameElement.height = document.body.scrollHeight;
        else
            window.frameElement.height = '850px';
        window.frameElement.Width = document.body.scrollWidth;
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
                var ids = document.getElementById('txtsegselected_hidden');
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
           
            document.getElementById('TdFilter').style.visibility='hidden';
            document.getElementById('TdFilter1').style.visibility='hidden';
	        
//	        document.getElementById('rdAll').checked='true';
	    }
//	    function OnGenerateClick()
//    {
//       // document.getElementById("Div1").style.visibility='visible';//style.display='block';
//        dpHitDataBase.PerformCallback();
//    }
    </script>

    <script type="text/ecmascript">
        
	    function ReceiveServerData(rValue)
        {
            
            var Data=rValue.split('~');
            var NoItems=Data[1].split(';');
            var a='';
            if(NoItems.length>1)
            {
                
                var NoItemsDis=Data[1].split(',');
                for(i=0;i<NoItemsDis.length;i++)
                {
                    if(i==0)
                    {
//                        var a=NoItemsDis[i];
                          var Dis=NoItemsDis[i].split(';');
                           a=Dis[1];
                        
                    }
                    else
                    {
                        var Dis=NoItemsDis[i].split(';');
                         a=a+','+Dis[1];
                       
                    }
                     
                }
            }
            var val=''; 
            var j=0;
           
//            alert(NoItems[1])
            for(i=0;i<NoItems.length;i++)
            {
//                i=0;
//                while(i<NoItems.length/2)
//                {
//                    alert(NoItems[i]);
                    if(val=='')
                    {
                        val='('+NoItems[i];
                         
                    }
                    
                    else
                    {
                        val+=','+NoItems[i];
                        
                    }
//                    i=i+2;
//                }
            }
            
            val=val+')';
            
            if(Data[0]=='Client')
            {
                var combo = document.getElementById('litSegment');
                combo.innerText=val;
                
                var combo12 = document.getElementById('litSegment112');
                combo12.innerText=a;
            }
            else if(Data[0]=='Branch')
            {
                var combo12 = document.getElementById('litSegment12');
                combo12.innerText=a;

                var combo = document.getElementById('litSegment1');
                var BranchId=val.split(',');
                var valBID='';
                var i=0;
                for(i=0;i<BranchId.length;i++)
                {
//                    alert(i)
//                    alert(BranchId[0,i]);
                    if(i%2==0)
                    {
                        if(i==0)
                        {
                            valBID=BranchId[0,i];
                        }
                        else
                        {
                            valBID=valBID+','+BranchId[0,i];
                        }
                    }
                    
                }
                valBID=valBID+')';
                val=valBID;
                combo.innerText=val;
//              alert(combo.innerText)
            }
        } 
   
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <%-- <div id='Div1' style="position: absolute; left: 650px; top: 150px; visibility: hidden">
            <table style="width: 100; height: 35; border: 1;" cellpadding="0" cellspacing="0">
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
        </div>--%>
        <table class="TableMain100" style="z-index: 100; left: 0px; position: absolute; top: 0px">
            <tr class="EHEADER" align="center">
                <td colspan="2">
                    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="36000">
                    </asp:ScriptManager>
                    CONTACT DETAILS REPORT
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                Branch</td>
                            <%-- <td>
                                <asp:DropDownList ID="ddlGroup" runat="server" Width="80px" Font-Size="12px" onchange="fnddlGroup(this.value)">
                                    <asp:ListItem Value="0">Branch</asp:ListItem>
                                    <asp:ListItem Value="1">Group</asp:ListItem>
                                </asp:DropDownList>
                            </td>--%>
                            <td id="td_branch">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdAllBranch" runat="server" Checked="True" GroupName="cd" onclick="Branch('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdSelBrnh" runat="server" GroupName="cd" onclick="Branch('b')" />Selected
                                        </td>
                                        <td style="display: none">
                                            <span id="litSegment1" runat="server" style="color: Maroon" visible="true"></span>
                                        </td>
                                        <td style="width: 100px">
                                            <span id="litSegment12" runat="server" style="color: Maroon" visible="true"></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft" id="TdAc">
                                Clients
                            </td>
                            <td style="text-align: left;" id="TdAc1" colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdAll" runat="server" Checked="True" GroupName="a" />
                                        </td>
                                        <td>
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdSelected" runat="server" GroupName="a" />
                                        </td>
                                        <td>
                                            Selected
                                        </td>
                                        <td style="display: none">
                                            <span id="litSegment" runat="server" style="color: Maroon" visible="true"></span>
                                        </td>
                                        <td style="width: 100px">
                                            <span id="litSegment112" runat="server" style="color: Maroon" visible="true"></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%--<input id="btnSubmit" type="button" value="Generate" class="btnUpdate" onclick="Javascript:OnGenerateClick()" />--%>
                                <asp:Button ID="Button1" runat="server" Text="Show" OnClick="Button1_Click" CssClass="btnUpdate" />
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:HiddenField ID="HdPageNo" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnReportShow" runat="server" Text="Print" CssClass="btnUpdate" OnClick="btnReportShow_Click"/></td>
                                    </tr>
                                </table>
                            </td>
                            <%--<td style="display: none">
                                <dxe:ASPxComboBox ID="dpHitDataBase" runat="server" ValueField="col" TextField="Notification"
                                    ClientInstanceName="dpHitDataBase" Width="100%" Visible="true" OnCallback="dpHitDataBase_Callback">
                                </dxe:ASPxComboBox>
                            </td>--%>
                        </tr>
                        <tr runat="server" id="trButton">
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="btnFirst" runat="server" ImageUrl='../images/pFirst.png' OnClick="btnFirst_Click" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="btnPrevious" runat="server" ImageUrl='../images/pPrev.png' OnClick="btnPrevious_Click1" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="btnNext" runat="server" ImageUrl='../images/pNext.png' OnClick="btnNext_Click1" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="btnLast" runat="server" ImageUrl='../images/pLast.png' OnClick="btnLast_Click1" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top" id="TdFilter" style="visibility: hidden">
                    <table>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtsegselected" runat="server" Width="128px"></asp:TextBox><asp:HiddenField
                                    ID="txtsegselected_hidden" runat="server" />
                                <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                    style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                        style="color: #009900; font-size: 8pt;">&nbsp;</span>
                            </td>
                            <td id="TdFilter1" style="height: 23px">
                                <asp:DropDownList ID="cmbsearchOption" runat="server" Width="85px" Font-Size="11px"
                                    Enabled="false">
                                    <asp:ListItem>Branch</asp:ListItem>
                                    <asp:ListItem>Client</asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>
                    </table>
                    <table cellpadding="0" cellspacing="0" id="TdSelect">
                        <tr>
                            <td>
                                <asp:ListBox ID="lstSuscriptions" runat="server" Font-Size="12px" Height="90px" Width="253px">
                                </asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="height: 14px">
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
                <td colspan="2">
                    <div id="MainContainer" runat="server" class="TableMain100" style="border: solid 1px blue;
                        text-align: center">
                    </div>
                </td>
            </tr>
        </table>
</asp:Content>

