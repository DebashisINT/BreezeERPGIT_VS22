<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmClosingRatelist" Codebehind="frmClosingRatelist.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajaxList_rootfile.js"></script>

    <script type="text/javascript" src="/assests/js/GenericJscript.js"></script>

    <link type="text/css" href="../CSS/AjaxStyle.css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript">
    
    
     function ShowHide(obj,obj1)
        {
         document.getElementById('spanshow3').innerText=obj;
         document.getElementById('spanshow2').innerText=obj1;
         
          height();
        
        }
    
    function DeleteRow(keyValue)
    {
             doIt=confirm('Confirm delete?');
            if(doIt)
                {
                   gridClosing.PerformCallback(keyValue);
                   height();
                }
            else{
                  
                }

   
    }
    function ForFilterOn()
    {   
    document.getElementById('MainFilter').style.display="inline";
    document.getElementById('spanBtn').style.display='inline';
    document.getElementById('spanfltr').style.display='none';
    document.getElementById('spanShow').style.display='inline';
    document.getElementById('showADD').style.display='inline';
    
          var chk=document.getElementById("rdInstrumentAll");
           if(chk.checked == true)
           {
              document.getElementById('ShowAll').style.display="inline";
              document.getElementById('ShowInst').style.display="none";
              document.getElementById('ShowSelect').style.display="none";
           }
           else
           {           
              document.getElementById('ShowAll').style.display="none";
              document.getElementById('ShowInst').style.display="inline";
              document.getElementById('ShowSelect').style.display="inline";
           }
  
        height();
    
    }
    function ForFilterOff()
    { 
     document.getElementById('spanfltr').style.display='inline';  
     document.getElementById('MainFilter').style.display="none";
     document.getElementById('spanBtn').style.display='none';
     document.getElementById('spanShow').style.display='none';
     document.getElementById('trFilterVar').style.display='none';
     document.getElementById('trGridVar').style.display='none';
     
    height();
    }
    function SignOff()
    {
        window.parent.SignOff();
    }
    function height()
    {
       
        if(document.body.scrollHeight>=700)
            window.frameElement.height = document.body.scrollHeight;
        else
            window.frameElement.height = '700px';
        window.frameElement.Width = document.body.scrollWidth;
    }
    function OnAddButtonClick() 
    {
     //var dat=document.getElementById('dtTo_hidden').value;
     // var dat=document.getElementById('cmbForDate').value;
          var dat = cmbForDate.GetText();
          var url='../Management/Master/frmClosingRatesAdd.aspx?id='+'ADD'+'&dtfor='+ dat;
         OnMoreInfoClick(url,"Add New Closing Rates",'700px','350px',"Y");
    }
   function OnMoreInfoClick(keyValue)
    {
      var dat=document.getElementById('dtTo_hidden').value;
       var url='../Management/Master/frmClosingRatesAdd.aspx?id='+ keyValue +'&dtfor='+ dat;
       OnMoreInfoClick(url,"Edit Closing Rates",'700px','350px',"Y");
   
    }
  
   function PageLoad()
    {
     
      //ShowEmployeeFilterForm('A');
      document.getElementById('ShowAll').style.display="inline";
      document.getElementById('ShowSelect').style.display="none";
         document.getElementById('ShowInst').style.display="none";
      document.getElementById('spanfltr').style.display='none';
       document.getElementById('spanshow3').style.display='none';
         document.getElementById('spanshow2').style.display='none';
         
         document.getElementById('spanfltr1').style.display='none';
          document.getElementById('trFilterClosing').style.display='none';
          document.getElementById('trGridClosing').style.display='none';
       height();
        
    }
    function ShowHideFilter(obj)
    {
        gridClosing.PerformCallback(obj);
          height();
    } 
    function callback()
    {
       gridClosing.PerformCallback();
       gridVarRate.PerformCallback();
        height();
    }  
    
       function ReceiveServerData(rValue)
        {
          var Data=rValue.split('~');
          if(Data[0]=='Instruments')
            {
                var combo = document.getElementById('litInstrumentMain');
                var NoItems=Data[1].split(',');
                var i;
                var val='';
                for(i=0;i<NoItems.length;i++)
                {
                
                    var items = NoItems[i].split(';');
                   
                    if(val=='')
                    {
                        val='('+items[1];
                    }
                    else
                    {
                        val+=','+items[1];
                    }
                }
                val=val+')';
                combo.innerText=val;
            }
        }
        function CallAjax(obj1,obj2,obj3,Query)
  {
  //alert(Query);
        var CombinedQuery=new String(Query);
        ajax_showOptions(obj1,obj2,obj3,replaceChars(CombinedQuery),'Main');
       // alert(CombinedQuery);
  }
  
  
function replaceChars(entry)
 {
        out = "+"; // replace this
        add = "--"; // with this
        temp = "" + entry; // temporary holder

        while (temp.indexOf(out)>-1)
         {
        pos= temp.indexOf(out);
        temp = "" + (temp.substring(0, pos) + add + 
        temp.substring((pos + out.length), temp.length));
         }
     return temp;
}
//     function showOptions(obj1,obj2,obj3)
//         {
//               var obj4 = txtFromDate.GetText();
//               ajax_showOptions(obj1,obj2,obj3,obj4);
//         }
   
     function btnAddsubscriptionlist_click()
        {
        
            var cmb=document.getElementById('cmbsearchOption');
                var userid = document.getElementById('CbptxtSelectionID_txtSelectionID');
                if(Control_CompareText("CbptxtSelectionID_txtSelectionID","No Record Found","Please Select Product Properly!!!"))
                    if(Control_Empty("CbptxtSelectionID_txtSelectionID","Please Select Product Properly!!!"))
                    {
                        var ids = document.getElementById('CbptxtSelectionID_txtSelectionID_hidden');
                        var listBox = document.getElementById('lstSlection');
                        var tLength = listBox.length;
                       
                        
                        var no = new Option();
                        no.value = ids.value;
                        no.text = userid.value;
                        listBox[tLength]=no;
                        var recipient = document.getElementById('CbptxtSelectionID_txtSelectionID');
                        recipient.value='';
                    }
//                document.getElementById('CbptxtSelectionID_txtSelectionID').focus()
                document.getElementById('CbptxtSelectionID_txtSelectionID').select();
                

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
              
//                document.getElementById('showFilter').style.display='none';
//                document.getElementById('TdFilter').style.display='none';
 
                document.getElementById('btn_show').disabled=false;
                document.getElementById('showADD').style.display='none';
               document.getElementById('ShowInst').style.display="none";
                 height();
                
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
            
       function rdbtnSegAll(obj)
        {
            document.getElementById('ShowAll').style.display="inline";
            document.getElementById('ShowSelect').style.display="none";
              document.getElementById('ShowInst').style.display="none";
             height();

        }
        
        function rdbtnSelected(obj)
        {
           //For Binding Ajax List Dynamically
           FromDateChange();
          
          
        }
        
        FieldName='lstSlection';
        
         function ForVarFilterOff()
            { 
             document.getElementById('spanfltr1').style.display='inline';  
             document.getElementById('MainFilter').style.display="none";
             document.getElementById('spanBtn').style.display='none';
             document.getElementById('spanShow').style.display='none';
             document.getElementById('trFilterClosing').style.display='none';
             document.getElementById('trGridClosing').style.display='none';
             
            height();
            }
            
            function ShowHideVar(obj,obj1)
            {
                document.getElementById('spanshow33').innerText=obj;
                document.getElementById('spanshow22').innerText=obj1;
                height();
            }
            function ShowHideVarFilter(obj)
            {
                gridVarRate.PerformCallback(obj);
            } 
            
        function OnVarAddButtonClick() 
        {
            var dat = cmbForDate.GetText();
            var url='frmVarRateAdd.aspx?id='+'ADD'+'&dtfor='+ dat;
            OnMoreInfoClick(url,"Add New Var Rates",'700px','350px',"Y");
        }
       function OnVarMoreInfoClick(keyValue)
        {
          var dat=document.getElementById('dtTo_hidden').value;
           var url='frmVarRateAdd.aspx?id='+ keyValue +'&dtfor='+ dat;
           OnMoreInfoClick(url,"Edit Var Rates",'700px','350px',"Y");
       
        }
       function CancelFilter()
        { 
         document.getElementById('MainFilter').style.display="none";
         document.getElementById('spanBtn').style.display='none';
          document.getElementById('spanShow').style.display='none';
         if(document.getElementById('rbClosingRate').checked==true)
            {
            
             document.getElementById('spanfltr').style.display='inline';                                    
             document.getElementById('trFilterVar').style.display='none';
             document.getElementById('trGridVar').style.display='none';
             document.getElementById('trFilterClosing').style.display='inline';
             document.getElementById('trGridClosing').style.display='inline';
            }
         else
            {
                document.getElementById('spanfltr1').style.display='inline'; 
                document.getElementById('trFilterVar').style.display='inline';
                document.getElementById('trGridVar').style.display='inline';
                document.getElementById('trFilterClosing').style.display='none';
                document.getElementById('trGridClosing').style.display='none';
            
            }
        height();
        }
        
         function DeleteVarRow(keyValue)
            {
                     conf=confirm('Confirm delete?');
                    if(conf)
                        {
                           gridVarRate.PerformCallback(keyValue);
                           height();
                        }
                    else{
                          
                        }

           
            }
        function FromDateChange()
        {
            cCbptxtSelectionID.PerformCallback('BindProduct'+'~'+ctxtFromDate.GetDate());
        }
        //Global Variable
        var WhichTextBox;
        var CombinedQuery;
        function CbptxtSelectionID_EndCallBack()
        {
            if(cCbptxtSelectionID.cpBindProduct != null)
            { 
                document.getElementById('ShowAll').style.display="none";
                document.getElementById('ShowSelect').style.display="inline";
                document.getElementById('ShowInst').style.display="inline";
                WhichTextBox=document.getElementById('CbptxtSelectionID_txtSelectionID');
                CombinedQuery=cCbptxtSelectionID.cpBindProduct;
                WhichTextBox.attachEvent('onkeyup',CallGenericAjaxJS);
            }
        }
        function CallGenericAjaxJS(e)
        {
            CombinedQuery=CombinedQuery.replace("\'","'");
            ajax_showOptions(WhichTextBox,'GenericAjaxList',e,replaceChars(CombinedQuery),'Main');
        }
        function gridVarRate_EndCallBack(e)
        {
            height();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3600">
            </asp:ScriptManager>
            <table class="TableMain100">
                <tr>
                    <td class="EHEADER" style="text-align: center; height: 20px;">
                        <strong><span style="color: #000099">Closing Rates / Var Rate</span></strong>
                    </td>
                </tr>
                <tr id="MainFilter">
                    <td>
                        <table cellspacing="1" cellpadding="2" style="background-color: #B7CEEC; border: solid 1px  #ffffff"
                            border="1">
                            <tr>
                                <td>
                                    Select Rate
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="rbClosingRate" runat="server" Checked="True" GroupName="Rate" />
                                                        </td>
                                                        <td>
                                                            Closing Rate
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rbVarRate" runat="server" GroupName="Rate" />
                                                        </td>
                                                        <td>
                                                            Var Rate
                                                        </td>
                                                        <td>
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
                                    Instruments
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="rdInstrumentAll" runat="server" Checked="True" GroupName="b" />
                                                        </td>
                                                        <td>
                                                            All
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdInstrumentSelected" runat="server" GroupName="b" />
                                                        </td>
                                                        <td>
                                                            Selected
                                                        </td>
                                                        <td>
                                                            <span id="litInstrumentMain" runat="server" style="color: Maroon"></span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="ShowAll">
                                <td>
                                    For Date:
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <dxe:ASPxDateEdit ID="cmbForDate" runat="server" EditFormat="Custom" UseMaskBehavior="True">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                    <DropDownButton Text="From Date">
                                                    </DropDownButton>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="ShowSelect">
                                <td valign="top">
                                    For Period:
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td class="gridcellleft" valign="top">
                                                <dxe:ASPxDateEdit ID="txtFromDate" ClientInstanceName="ctxtFromDate" runat="server" EditFormat="Custom" UseMaskBehavior="True">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                    <DropDownButton Text="From Date">
                                                    </DropDownButton>
                                                    <ClientSideEvents DateChanged="function(s,e){FromDateChange();}" />
                                                </dxe:ASPxDateEdit>
                                            </td>
                                            <td class="gridcellleft" valign="top">
                                                <dxe:ASPxDateEdit ID="txtToDate" runat="server" EditFormat="Custom" UseMaskBehavior="True">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                    <DropDownButton Text="To Date">
                                                    </DropDownButton>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="ShowInst">
                                <td valign="top">
                                    Select Instrument:
                                </td>
                                <td valign="top" id="showADD" align="left">
                                    <table id="showFilter">
                                        <tr>
                                            <td class="gridcellleft" style="vertical-align: top; text-align: left" id="TdFilter">
                                                <span id="spanall">
                                                    <dxe:ASPxCallbackPanel ID="CbptxtSelectionID" runat="server" ClientInstanceName="cCbptxtSelectionID" 
                                                    OnCallback="CbptxtSelectionID_Callback" Width="206px">
                                                        <PanelCollection>
                                                            <dxe:panelcontent runat="server">
                                                                <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="210px"></asp:TextBox>
                                                                <div style="display: none">
                                                                    <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                                                </div>
                                                            </dxe:panelcontent>
                                                        </PanelCollection>
                                                        <ClientSideEvents EndCallback="CbptxtSelectionID_EndCallBack" />
                                                    </dxe:ASPxCallbackPanel>
                                                </span>
                                                <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                                Enabled="false">
                                                                <asp:ListItem>Instruments</asp:ListItem>
                                                                
                                                            </asp:DropDownList>
                                                <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                                    style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                                        style="color: #009900; font-size: 8pt;"> </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; vertical-align: top; height: 146px;">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:ListBox ID="lstSlection" runat="server" Font-Size="12px" Height="90px" Width="300px">
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
                                        <%--  <tr>
                                    <td style="text-align:left;">
                                        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" CssClass="btnUpdate" Text="Send" /></td>
                                    </tr>--%>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="updatepanel_trprevnext">
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
                <tr>
                    <td>
                        <asp:UpdatePanel ID="updatepanel_trprevnext" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table width="100%">
                                    <tr id="TrBtn">
                                        <td colspan="2">
                                            <table width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <span id="spanShow">
                                                            <asp:Button ID="btn_show" runat="server" Text="Show" CssClass="btnUpdate" Height="23px"
                                                                Width="101px" OnClick="btn_show_Click" /></span> <span id="spanBtn" style="display: none">
                                                                    <a href="#" style="font-weight: bold; color: Blue" onclick="javascript:CancelFilter();">
                                                                        Cancel</a></span></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="display: none">
                                            <asp:TextBox ID="dtTo_hidden" runat="server" Font-Size="12px" Width="78px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trFilterClosing">
                                        <td style="text-align: left; vertical-align: top">
                                            <table width="100%">
                                                <tr>
                                                    <td id="Td1" align="left">
                                                        <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">
                                                            Show Filter</span></a> || <a href="javascript:ShowHideFilter('All');"><span style="color: #000099;
                                                                text-decoration: underline">All Records</span></a>
                                                    </td>
                                                    <td>
                                                        <span id="spanshow2" style="color: Blue; font-weight: bold"></span>&nbsp;&nbsp;<span
                                                            id="spanshow3"></span>
                                                    </td>
                                                    <td align="right" style="text-align: left">
                                                        <span id="spanfltr" style="display: none"><a href="#" style="font-weight: bold; color: Blue"
                                                            onclick="javascript:ForFilterOn();">Filter</a></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="trGridClosing">
                                        <td align="center">
                                            <dxe:ASPxGridView ID="gridClosing" ClientInstanceName="gridClosing" Width="100%"
                                                KeyFieldName="DailyStat_ID" DataSourceID="gridClosingDataSource" runat="server"
                                                AutoGenerateColumns="False" OnCustomCallback="gridClosing_CustomCallback">
                                                <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
                                                <Styles>
                                                    <Header CssClass="gridheader">
                                                    </Header>
                                                    <FocusedRow CssClass="gridselectrow" BackColor="#FCA977">
                                                    </FocusedRow>
                                                    <FocusedGroupRow CssClass="gridselectrow" BackColor="#FCA977">
                                                    </FocusedGroupRow>
                                                </Styles>
                                                <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                                                    <FirstPageButton Visible="True">
                                                    </FirstPageButton>
                                                    <LastPageButton Visible="True">
                                                    </LastPageButton>
                                                </SettingsPager>
                                                <Columns>
                                                    <dxe:GridViewDataTextColumn Visible="False" FieldName="DailyStat_ID" Caption="ID">
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="Dailystat_DateTime1" Width="50px"
                                                        Caption="Date For">
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="instruments" Width="100px"
                                                        Caption="Instruments">
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="DailyStat_Open" Width="50px"
                                                        Caption="Open">
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="DailyStat_High" Width="50px"
                                                        Caption="High">
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="DailyStat_Low" Width="50px"
                                                        Caption="Low">
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="DailyStat_Close" Width="50px"
                                                        Caption="Close">
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="DailyStat_SettlementPrice"
                                                        Width="50px" Caption="Settlement Price">
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="DailyStat_AssetPrice" Width="50px"
                                                        Caption="Underlying">
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="8" Width="60px" Caption="Details">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <DataItemTemplate>
                                                            <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')">
                                                                Edit</a> &nbsp;&nbsp;<a href="javascript:void(0);" onclick="DeleteRow('<%# Container.KeyValue %>')">
                                                                    Delete</a>
                                                        </DataItemTemplate>
                                                        <CellStyle Wrap="False">
                                                        </CellStyle>
                                                        <HeaderTemplate>
                                                            <a href="javascript:void(0);" onclick="javascript:OnAddButtonClick();"><span style="color: #000099;
                                                                text-decoration: underline">Add New</span> </a>
                                                        </HeaderTemplate>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                </Columns>
                                                <SettingsText ConfirmDelete="Confirm delete?" />
                                                <StylesEditors>
                                                    <ProgressBar Height="25px">
                                                    </ProgressBar>
                                                </StylesEditors>
                                            </dxe:ASPxGridView>
                                            <asp:SqlDataSource ID="gridClosingDataSource" runat="server">
                                                <%-- SelectCommand="select DematTransactions_ID,DematTransactions_FinYear,DematTransactions_Date,DematTransactions_CompanyID,DematTransactions_SegmentID,DematTransactions_BranchID,DematTransactions_CustomerID,DematTransactions_ProductSeriesID,DematTransactions_ISIN,DematTransactions_Type,DematTransactions_SettlementNumberS,DematTransactions_SettlementTypeS,DematTransactions_Quantity,DematTransactions_OwnAccountS,DematTransactions_Remarks,DematTransactions_GenerationType,DematTransactions_GenerateUser,DematTransactions_IsAuthorized from  Trans_DematTransactions"
                       InsertCommand=" ">--%>
                                            </asp:SqlDataSource>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%">
                                    <tr id="trFilterVar">
                                        <td style="text-align: left; vertical-align: top">
                                            <table width="100%">
                                                <tr>
                                                    <td id="tdVarFilter" align="left">
                                                        <a href="javascript:ShowHideVarFilter('s');"><span style="color: #000099; text-decoration: underline">
                                                            Show Filter</span></a> || <a href="javascript:ShowHideVarFilter('All');"><span style="color: #000099;
                                                                text-decoration: underline">All Records</span></a>
                                                    </td>
                                                    <td>
                                                        <span id="spanshow22" style="color: Blue; font-weight: bold"></span>&nbsp;&nbsp;<span
                                                            id="spanshow33"></span>
                                                    </td>
                                                    <td align="right" style="text-align: left">
                                                        <span id="spanfltr1" style="display: none"><a href="#" style="font-weight: bold;
                                                            color: Blue" onclick="javascript:ForFilterOn();">Filter</a></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="trGridVar">
                                        <td align="center">
                                            <dxe:ASPxGridView ID="gridVarRate" ClientInstanceName="gridVarRate" Width="100%"
                                                KeyFieldName="DailyVar_ID" DataSourceID="gridVarDataSource" runat="server" AutoGenerateColumns="False"
                                                OnCustomCallback="gridVarRate_CustomCallback">
                                                <ClientSideEvents EndCallback="function(s, e) {gridVarRate_EndCallBack();}" />
                                                <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
                                                <Styles>
                                                    <Header CssClass="gridheader">
                                                    </Header>
                                                    <FocusedRow CssClass="gridselectrow" BackColor="#FCA977">
                                                    </FocusedRow>
                                                    <FocusedGroupRow CssClass="gridselectrow" BackColor="#FCA977">
                                                    </FocusedGroupRow>
                                                </Styles>
                                                <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                                                    <FirstPageButton Visible="True">
                                                    </FirstPageButton>
                                                    <LastPageButton Visible="True">
                                                    </LastPageButton>
                                                </SettingsPager>
                                                <Columns>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="DailyVar_Date" Width="50px"
                                                        Caption="Date For">
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="Equity_TickerSymbol" Width="50px"
                                                        Caption="Instruments">
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="DailyVar_SecurityVar" Width="50px"
                                                        Caption="Security Var">
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="DailyVar_IndexVar" Width="50px"
                                                        Caption="IndexVar">
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="DailyVar_VarMargin" Width="50px"
                                                        Caption="VarMargin">
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="DailyVar_ExtremeLossRate"
                                                        Width="50px" Caption="ExtremeLossRate">
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="DailyVar_AdhocMargin" Width="50px"
                                                        Caption="AdhocMargin">
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="DailyVar_SpecialMargin"
                                                        Width="50px" Caption="SpecialMargin">
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="DailyVar_AppMargin" Width="50px"
                                                        Caption="AppMargin">
                                                        <CellStyle CssClass="gridcellleft">
                                                        </CellStyle>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="9" Width="60px" Caption="Details">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <DataItemTemplate>
                                                            <a href="javascript:void(0);" onclick="OnVarMoreInfoClick('<%# Container.KeyValue %>')">
                                                                Edit</a> &nbsp;&nbsp;<a href="javascript:void(0);" onclick="DeleteVarRow('<%# Container.KeyValue %>')">
                                                                    Delete</a>
                                                        </DataItemTemplate>
                                                        <CellStyle Wrap="False">
                                                        </CellStyle>
                                                        <HeaderTemplate>
                                                            <a href="javascript:void(0);" onclick="javascript:OnVarAddButtonClick();"><span style="color: #000099;
                                                                text-decoration: underline">Add New</span> </a>
                                                        </HeaderTemplate>
                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                    </dxe:GridViewDataTextColumn>
                                                </Columns>
                                                <SettingsText ConfirmDelete="Confirm delete?" />
                                                <StylesEditors>
                                                    <ProgressBar Height="25px">
                                                    </ProgressBar>
                                                </StylesEditors>
                                            </dxe:ASPxGridView>
                                            <asp:SqlDataSource ID="gridVarDataSource" runat="server">
                                                <%-- SelectCommand="select DematTransactions_ID,DematTransactions_FinYear,DematTransactions_Date,DematTransactions_CompanyID,DematTransactions_SegmentID,DematTransactions_BranchID,DematTransactions_CustomerID,DematTransactions_ProductSeriesID,DematTransactions_ISIN,DematTransactions_Type,DematTransactions_SettlementNumberS,DematTransactions_SettlementTypeS,DematTransactions_Quantity,DematTransactions_OwnAccountS,DematTransactions_Remarks,DematTransactions_GenerationType,DematTransactions_GenerateUser,DematTransactions_IsAuthorized from  Trans_DematTransactions"
                       InsertCommand=" ">--%>
                                            </asp:SqlDataSource>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
</asp:Content>
