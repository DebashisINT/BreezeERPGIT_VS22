<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_GroupMemberList" Codebehind="GroupMemberList.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajaxList_rootfile.js"></script>

    <%--    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>--%>
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript">
       function Page_Load()///Call Into Page Load
            {  
            
                 document.getElementById('ShowTable').style.display='none';
                height();
               
            }
   function callheight(obj)
    {
        height();
       //// parent.CallMessage();
    }
    function SignOff()
    {
        window.parent.SignOff();
    }
    function height()
    {
        if(document.body.scrollHeight>=500)
            window.frameElement.height = document.body.scrollHeight;
        else
            window.frameElement.height = '500px';
        window.frameElement.Width = document.body.scrollWidth;
    }

    function ShowHideFilter(obj)
    {
    selecttion();
           grid.PerformCallback(obj);
    } 
    
                    
                     //THIS IS FOR EMAIL
        
          function btnAddEmailtolist_click()
            {
            
            selecttion();
                var cmb=document.getElementById('cmbsearch');
            
                    var userid = document.getElementById('txtSelectID');
                    if(userid.value != '')
                    {
                        var ids = document.getElementById('txtSelectID_hidden');
                        var listBox = document.getElementById('SelectionList');
                        var tLength = listBox.length;
                       
                        
                        var no = new Option();
                        no.value = ids.value;
                        no.text = userid.value;
                        listBox[tLength]=no;
                        var recipient = document.getElementById('txtSelectID');
                        recipient.value='';
                    }
                    else
                        alert('Please search name and then Add!')
                    var s=document.getElementById('txtSelectID');
                    s.focus();
                    s.select();

            }
    
        
   function callAjax1(obj1,obj2,obj3)
    {
     document.getElementById('SelectionList').style.display='none';
        var combo = document.getElementById("cmbGroup");
        var set_value = combo.value;
        var obj4='Main';
        ajax_showOptions(obj1,obj2,obj3,set_value,obj4)	  
       
//        if (set_value=='16')
//        {
//            ajax_showOptions(obj1,'GetLeadId',obj3,set_value,obj4)
//        }
//        else
//        {
//         
//            ajax_showOptions(obj1,obj2,obj3,set_value,obj4)	  
//        }
        
    }
    
       function clientselection()
	        {
	            var listBoxSubs = document.getElementById('SelectionList');
	          
                var cmb=document.getElementById('cmbsearch');
              
                var listIDs='';
                var i;
                selecttion();
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
                    
                    CallServer1(sendData,"");
           
                 document.getElementById('ShowTable').style.display='none';
                 document.getElementById('showFilter1').style.display='inline';
                }
                else
                {
                alert("Please select email from list.")
                }
               
	            var i;
                for(i=listBoxSubs.options.length-1;i>=0;i--)
                {
                    listBoxSubs.remove(i);
                 }
           
                window.frameElement.height = document.body.scrollHeight;
	        }
	        
	   function btnRemoveEmailFromlist_click()
            {
                
                var listBox = document.getElementById('SelectionList');
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
            
      function ReceiveSvrData(rValue)
        {
            var Data=rValue.split('~');
            if(Data[0]=='Clients')
            {
            }
        }
        
    
                
        function keyVal(obj)
         {
           document.getElementById('SelectionList').style.display='inline';
           
         }   
         
           function SelectUserClient(obj)
             {
            if(obj=='Client')
            {
                      
           
            document.getElementById('ShowTable').style.display='none';
            document.getElementById('showFilter1').style.display='inline';
            
            }
           else if(obj=='User')
            {
                  document.getElementById('ShowTable').style.display='inline';
                
                  document.getElementById('showFilter1').style.display='none';
            }
            
 
 } 
   function selecttion()
        {
       
            var combo=document.getElementById('cmbExport');
            combo.value='Ex';
        }  
 FieldName='SelectionList';
              
    </script>

    <script type="text/ecmascript">   
       function ReceiveServerData(rValue)
        {
               
                var j=rValue.split('~');
                var btn = document.getElementById('btnhide');
                if(j[0]=='Group')
                {
                   groupvalue=j[1];
                   btn.click();
                }
                if(j[0]=='Branch')
                {
                   groupvalue=j[1];
                   btn.click();
                }
        }
        
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>
            <table class="TableMain100">
                <tr>
                    <td class="EHEADER" style="text-align: center; height: 20px;">
                        <strong><span style="color: #000099">Group Member List</span></strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table class="TableMain100">
                            <tr>
                                <td valign="top">
                                    <table cellspacing="3">
                                        <tr>
                                            <td>
                                                Group Type:
                                                <asp:DropDownList ID="cmbGroup" runat="server">
                                                    <asp:ListItem>ALL</asp:ListItem>
                                                    <asp:ListItem Value="Broker">Broker</asp:ListItem>
                                                    <asp:ListItem Value="Sub Broker">Sub Broker</asp:ListItem>
                                                    <asp:ListItem Value="Relationship Partner">Relationship Partner</asp:ListItem>
                                                    <asp:ListItem Value="Franchisee">Franchisee</asp:ListItem>
                                                    <asp:ListItem Value="Relationship Manager">Relationship Manager</asp:ListItem>
                                                    <asp:ListItem Value="Relationship Officer">Relationship Officer</asp:ListItem>
                                                    <asp:ListItem Value="Dealers">Dealers</asp:ListItem>
                                                    <asp:ListItem Value="Family">Family</asp:ListItem>
                                                    <asp:ListItem Value="Business Partner">Business Partner</asp:ListItem>
                                                    <asp:ListItem Value="Agents">Agents</asp:ListItem>
                                                    <asp:ListItem Value="Virtual DP">Virtual DP</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table id="ShowSelectUser">
                                                    <tr>
                                                        <td valign="top">
                                                            Groups:
                                                        </td>
                                                        <td valign="top">
                                                            <asp:RadioButton ID="rbOnlyClient" runat="server" Checked="true" GroupName="h" />
                                                        </td>
                                                        <td valign="top">
                                                            ALL
                                                        </td>
                                                        <td valign="top">
                                                            <asp:RadioButton ID="rbClientUser" runat="server" GroupName="h" /><span style="font-size: 8pt;
                                                                color: #009900"> </span>
                                                        </td>
                                                        <td valign="top">
                                                            Selected
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="Button1" runat="server" CssClass="btnUpdate" OnClick="Button1_Click"
                                                    OnClientClick="javascript:selecttion();" Text="Show" /></td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <table id="ShowTable">
                                        <tr style="display: none;">
                                            <td width="70px" style="text-align: left;">
                                                Type:</td>
                                            <td class="gridcellleft" style="vertical-align: top; text-align: left" id="Td1">
                                                <span id="span1">
                                                    <asp:DropDownList ID="cmbsearch" runat="server" Width="150px" Font-Size="12px">
                                                    </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="70px" style="text-align: left;">
                                                Select Groups:</td>
                                            <td class="gridcellleft" style="vertical-align: top; text-align: left" id="TdFilter1">
                                                <span id="spanal2">
                                                    <asp:TextBox ID="txtSelectID" runat="server" Font-Size="12px" Width="285px"></asp:TextBox></span>
                                                <a id="A3" href="javascript:void(0);" onclick="btnAddEmailtolist_click()"><span style="color: #009900;
                                                    text-decoration: underline; font-size: 8pt;">Add to List</span></a><span style="color: #009900;
                                                        font-size: 8pt;"> </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="70px" style="text-align: left;">
                                            </td>
                                            <td style="text-align: left; vertical-align: top; height: 134px;">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            &nbsp;&nbsp;<asp:ListBox ID="SelectionList" runat="server" Font-Size="12px" Height="90px"
                                                                Width="290px"></asp:ListBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: left">
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <a id="AA2" href="javascript:void(0);" onclick="clientselection()"><span style="color: #000099;
                                                                            text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <a id="AA1" href="javascript:void(0);" onclick="btnRemoveEmailFromlist_click()"><span
                                                                            style="color: #cc3300; text-decoration: underline; font-size: 8pt;">Remove</span></a>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="display: none">
                                            <td width="70px" style="text-align: left;">
                                            </td>
                                            <td style="height: 23px">
                                                <asp:TextBox ID="txtSelectID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                                <asp:TextBox ID="dtFrom_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                                <asp:TextBox ID="dtTo_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <table  class="TableMain100">
                            
                                    <tr>
                                        <td align="left">
                                            <table>
                                                <tr>
                                                    <td id="ShowFilter">
                                                        <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">
                                                            Show Filter</span></a>
                                                    </td>
                                                    <td id="Td2">
                                                        <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">
                                                            All Records</span></a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td align="right">
                                            <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" BackColor="Navy"
                                                Font-Bold="False" ForeColor="White" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                                ValueType="System.Int32" Width="130px">
                                                <Items>
                                                    <dxe:ListEditItem Text="Select" Value="0" />
                                                    <dxe:ListEditItem Text="PDF" Value="1" />
                                                    <dxe:ListEditItem Text="XLS" Value="2" />
                                                    <dxe:ListEditItem Text="RTF" Value="3" />
                                                    <dxe:ListEditItem Text="CSV" Value="4" />
                                                </Items>
                                                <ButtonStyle BackColor="#C0C0FF" ForeColor="Black">
                                                </ButtonStyle>
                                                <ItemStyle BackColor="Navy" ForeColor="White">
                                                    <HoverStyle BackColor="#8080FF" ForeColor="White">
                                                    </HoverStyle>
                                                </ItemStyle>
                                                <Border BorderColor="White" />
                                                <DropDownButton Text="Export">
                                                </DropDownButton>
                                            </dxe:ASPxComboBox>
                                        </td>
                                    </tr>
                                </table>
                         
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <dxe:ASPxGridView ID="GroupMembers" runat="server" AutoGenerateColumns="False"
                            KeyFieldName="grp_id" Width="100%" ClientInstanceName="grid" OnCustomJSProperties="GroupMembers_CustomJSProperties"
                            OnCustomCallback="GroupMembers_CustomCallback">
                            <settings showgrouppanel="True" />
                            <columns>
                                <dxe:GridViewDataTextColumn FieldName="grp_id" ReadOnly="True" VisibleIndex="0"
                                    Visible="False">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxe:GridViewDataTextColumn>
                                 <dxe:GridViewDataTextColumn FieldName="GroupName" caption="Group Name" ReadOnly="True" VisibleIndex="0">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxe:GridViewDataTextColumn>
                                
                                  <dxe:GridViewDataTextColumn FieldName="grp_groupType" caption="group Type" ReadOnly="True" VisibleIndex="0" width="100px">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxe:GridViewDataTextColumn>
                                
                                <dxe:GridViewDataTextColumn FieldName="MembersName" caption="Members Name" ReadOnly="True" VisibleIndex="0">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxe:GridViewDataTextColumn>
                                 <dxe:GridViewDataTextColumn FieldName="Cnt_UCC" caption="Code" ReadOnly="True" VisibleIndex="0" width="70px">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="branch_desc"  caption="Branch Name" VisibleIndex="1">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="branch_Code" caption="Branch Code" ReadOnly="True" VisibleIndex="2" width="60px">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxe:GridViewDataTextColumn>
                               
                            </columns>
                            <settingsbehavior allowfocusedrow="True" columnresizemode="NextColumn" />
                            <styles>
                                <LoadingPanel ImageSpacing="10px">
                                </LoadingPanel>
                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                </Header>
                                <FocusedGroupRow CssClass="gridselectrow">
                                </FocusedGroupRow>
                                <FocusedRow CssClass="gridselectrow">
                                </FocusedRow>
                            </styles>
                            <settingspager numericbuttoncount="20" pagesize="20" showseparators="True" alwaysshowpager="True">
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </settingspager>
                            <clientsideevents endcallback="function(s, e) {
	callheight(s.cpHeight);
}" />
                        </dxe:ASPxGridView>
                        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                        </dxe:ASPxGridViewExporter>
                    </td>
                </tr>
            </table>
        </div>
</asp:Content>
