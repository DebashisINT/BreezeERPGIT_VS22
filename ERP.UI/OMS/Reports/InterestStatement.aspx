<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Management_PledgeEntry" Codebehind="InterestStatement.aspx.cs" %>

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
		z-index:32761;
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
    #txtIssuingBank
    {
        z-index:10000;
        
    }
    
    .bubblewrap{
    list-style-type:none;
    margin:0;
    padding:0;
    }

    .bubblewrap li{
    display:inline;
    width: 65px;
    height:60px;
    }

    .bubblewrap li img{
    width: 30px; /* width of each image.*/
    height: 35px; /* height of each image.*/
    border:0;
    margin-right: 12px; /*spacing between each image*/
    -webkit-transition:-webkit-transform 0.1s ease-in; /*animate transform property */
    -o-transition:-o-transform 0.1s ease-in; /*animate transform property in Opera */
    }

    .bubblewrap li img:hover{
    -moz-transform:scale(1.8); /*scale up image 1.8x*/
    -webkit-transform:scale(1.8);
    -o-transform:scale(1.8);
    
	</style>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />
    <script type="text/javascript" src="/assests/js/ajaxList_rootfile.js"></script>

    

    <script type="text/javascript" src="/assests/js/GenericJScript.js"></script>
     
    <link type="text/css" href="../CSS/AjaxStyle.css" rel="Stylesheet" />
    <style type="text/css">
    .radioButtonList { list-style:none; margin: 0; padding: 0;}
    .radioButtonList.horizontal li { display: inline;}

    .radioButtonList label{
      display:inline;
     } 
     .hidingButton {
       display :none;
     }
    </style>
    <script type="text/javascript">
       var state=0;
      
        function changeState(v){
           // alert(v);
              if(v=="1"){
                document.getElementById("tblSubAc").style.display="none";
                state=0;
              }
              else {
                  document.getElementById("tblSubAc").style.display="block";
                   state=1;
              }
        }
        
         function height(){
            if(document.body.scrollHeight>=500)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '600px';
            window.frameElement.Width = document.body.scrollWidth;
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
        function keyVal(obj)
        {
         try{
         //alert(GetValue("txtSubAc_hidden"));
         }
         catch(ex)
         {
          // alert(ex);
         }
        
        }
        function ShowHide(v)
        {
          if (v=='1') {
             document.getElementById("divSearchPanel").style.display = 'block';
             document.getElementById("btnFilter").style.display = 'none';
             document.getElementById("btnExportSummary").style.display = 'none';
          }
          else {
                 document.getElementById("divSearchPanel").style.display = 'none';
                 document.getElementById("btnFilter").style.display = 'block';
                 document.getElementById("btnExportSummary").style.display = 'block';
          }
          
        }
        function ShowPopUp()
        {
         //alert("11ex");
           try {
           document.getElementById("divOverlapping").style.display = 'block';
           document.getElementById("divPopUp").style.display = 'block';
           }
           catch(ex){
             alert(ex);
           }
        }
        function ClosePopUp()
        {
          
          document.getElementById("divOverlapping").style.display = 'none';
          document.getElementById("divPopUp").style.display = 'none';
        }
        function CallMainAccount(obj1,obj2,obj3)
        {
          try
          {
           var strQuery_Table = "Master_MainAccount";
           var strQuery_FieldName = " top 10 MainAccount_Name+\' [ \'+rtrim(ltrim(MainAccount_AccountCode))+\' ]\' as MainAccount_Name1,cast(MainAccount_ReferenceID as varchar)+\'~\'+MainAccount_SubLedgerType+\'~MAINAC~\'+MainAccount_AccountType +'*'+MainAccount_AccountCode as MainAccount_ReferenceID";
         //  var strQuery_WhereClause = " (MainAccount_Name like (\'%RequestLetter%\') or MainAccount_AccountCode like (\'%RequestLetter%\')) and isnull(MainAccount_BankCashType,'abc') not in('Cash','Bank')";
            var strQuery_WhereClause = " (MainAccount_Name like (\'%RequestLetter%\') or MainAccount_AccountCode like (\'%RequestLetter%\'))";
           // strQuery_WhereClause +=" AND MainAccount_AccountType in ('Asset','Liability') and isnull(MainAccount_BankCashType,'') not in ('Bank','Cash') and left(isnull(MainAccount_SubLedgerType,''),4)<>'PROD' "
           strQuery_WhereClause += "  AND MainAccount_SubLedgerType in ('brokers','customers','employees','creditors','debtors','sub broker','franchisee','agent','data vendor','relationship partner','relationship manager','vendor')"
           var strQuery_OrderBy='';
           var strQuery_GroupBy='';
           var CombinedQuery=new String(strQuery_Table+"$"+strQuery_FieldName+"$"+strQuery_WhereClause+"$"+strQuery_OrderBy+"$"+strQuery_GroupBy);
           ajax_showOptions(obj1,obj2,obj3,replaceChars(CombinedQuery),'Main'); 
           
           }
           catch(ex){
           //  alert(ex);
           }
         }
       function CallSubAccount(obj1,obj2,obj3){
           var   mainAcTypes =((GetValue("txtMainAc_hidden")).split('~')[1]).toString().toLowerCase();
            // alert(mainAcTypes) ;
          if(mainAcTypes=="custom")
          {
            var subreferenceAcId=GetValue("txtMainAc_hidden").split('*')[1];
           var strQuery_Table = "Master_SubAccount";
           var strQuery_FieldName="top 10 RTrim(LTrim(RTrim(LTrim(SubAccount_Name)) +'-['+Rtrim(Ltrim (SubAccount_Code))+']')) as Text, SubAccount_Code +'*'+SubAccount_Code as Value" ;
           var strQuery_WhereClause = " (SubAccount_Name like (\'%RequestLetter%\') or SubAccount_Code like (\'%RequestLetter%\'))";
          // strQuery_WhereClause += " AND SubAccount_MainAcReferenceID='BADC002'"
           strQuery_WhereClause += " AND SubAccount_MainAcReferenceID=" + "'"+subreferenceAcId +"'"
           var strQuery_OrderBy='';
           var strQuery_GroupBy='';
            var CombinedQuery=new String(strQuery_Table+"$"+strQuery_FieldName+"$"+strQuery_WhereClause+"$"+strQuery_OrderBy+"$"+strQuery_GroupBy);
           ajax_showOptions(obj1,obj2,obj3,replaceChars(CombinedQuery),'Main');
          }
          else if (mainAcTypes=="brokers" || mainAcTypes=="customers" || mainAcTypes=="employees" || mainAcTypes=="creditors" || mainAcTypes=="debtors" || mainAcTypes=="sub broker" || mainAcTypes=="franchisee" || mainAcTypes=="agent" || mainAcTypes=="data vendor" || mainAcTypes=="relationship partner" || mainAcTypes=="relationship manager" || mainAcTypes=="vendor")
             {
                 var strQuery_Table = "tbl_master_contact";
                 var strQuery_FieldName="cnt_firstName+' '+ISNULL(cnt_middleName,'')+ ' '+cnt_lastName+' '+cnt_UCC as TEXT, cnt_internalId+'*'+cnt_internaliD as value"
                 var strQuery_WhereClause = " (cnt_firstName like (\'%RequestLetter%\') or cnt_middleName like (\'%RequestLetter%\') or cnt_lastName like (\'%RequestLetter%\') or cnt_UCC like (\'%RequestLetter%\'))";
                 var strQuery_OrderBy='';
                 var strQuery_GroupBy='';
                 var CombinedQuery=new String(strQuery_Table+"$"+strQuery_FieldName+"$"+strQuery_WhereClause+"$"+strQuery_OrderBy+"$"+strQuery_GroupBy);
                 ajax_showOptions(obj1,obj2,obj3,replaceChars(CombinedQuery),'Main');
             }
           else if(mainAcTypes=="cdsl clients" || mainAcTypes=="master_cdslclients"){
              var strQuery_Table = "Master_CdslClients";
              var strQuery_FieldName="CdslClients_BenAccountNumber,CdslClients_FirstHolderName+CdslClients_FirstHolderMiddleName+CdslClients_FirstHolderLastName as Text"+"CdslClients_BenAccountNumber +'*'+CdslClients_BenAccountNumber"
               var strQuery_WhereClause = " (CdslClients_FirstHolderName like (\'%RequestLetter%\') or CdslClients_BenAccountNumber like (\'%RequestLetter%\') or CdslClients_FirstHolderLastName like (\'%RequestLetter%\'))";
                var strQuery_OrderBy='';
                 var strQuery_GroupBy='';
                 var CombinedQuery=new String(strQuery_Table+"$"+strQuery_FieldName+"$"+strQuery_WhereClause+"$"+strQuery_OrderBy+"$"+strQuery_GroupBy);
                 ajax_showOptions(obj1,obj2,obj3,replaceChars(CombinedQuery),'Main');
           }
           else if(mainAcTypes=="nsdl clients" || mainAcTypes=="master_nsdlclients"){
                  var strQuery_Table = "Master_NsdlClients";
              var strQuery_FieldName="NsdlClients_BenFirstHolderName+ '['  + CAST (NsdlClients_BenAccountID as Varchar(100))+']'as Text, 'aa*'+   CAST (NsdlClients_BenAccountID as Varchar(50)) as Value"
               var strQuery_WhereClause = " (NsdlClients_BenFirstHolderName like (\'%RequestLetter%\') or NsdlClients_BenFirstHolderName like (\'%RequestLetter%\') or NsdlClients_BenFirstHolderName like (\'%RequestLetter%\'))";
                var strQuery_OrderBy='';
                 var strQuery_GroupBy='';
                 var CombinedQuery=new String(strQuery_Table+"$"+strQuery_FieldName+"$"+strQuery_WhereClause+"$"+strQuery_OrderBy+"$"+strQuery_GroupBy);
                 ajax_showOptions(obj1,obj2,obj3,replaceChars(CombinedQuery),'Main');
           
           }
        
        }
        function test()
        {
          alert("ABCD");
        }
        function Validate()
        {
        
         if (document.getElementById("txtMainAc").value !="" && document.getElementById("txtMainAc_hidden").value !="" && (state==0 || document.getElementById("hdnSubAcNo").value =="1") ) 
         {
           /// alert("ss");
            document.getElementById("divMessage").style.display="none";
            return true;
         }
         else if (document.getElementById("txtMainAc").value =="" || document.getElementById("txtMainAc_hidden").value =="")
         {
           //document.getElementById("divMessage").style.display="block";
           alert("Select main account");
           return false;
         }
         else if ((state==1 && document.getElementById("lbSubAc").value =="") && (document.getElementById("txtMainAc").value =="" && document.getElementById("hdnSubAcNo").value =="0"))
         {
            document.getElementById("divMessage").style.display="none";
            alert("Select atleast one sub account");
           return false;
         }
         else {
           
            document.getElementById("divMessage").style.display="none";
            alert("Select atleast one sub account");
           return false;
         }
        }
        function check()
        {
          alert("Select one sub account");
          
        }
        

    </script> 

    <script type="text/javascript" language="javascript">
        FieldName='BtnSave';
        
    </script>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript" language="javascript">
function DateChange(positionDate)
        {
          var FYS='<%=Session["FinYearStart"]%>';
          var FYE='<%=Session["FinYearEnd"]%>';
          var LFY='<%=Session["LastFinYear"]%>';
          DevE_CheckForFinYear(positionDate,FYS,FYE,LFY);
        }
         </script>
  
    <table width="100%;">
    <tr>
          <td colspan="2" align="center" style="font-weight:bold; padding:3px 5px  5px 45px; font-size:medium; border-bottom-style:none;">
             Interest&nbsp;Summary&nbsp;Form
          </td>
    </tr>
      <tr align="left">
      <td style="width:100%;" colspan="2">
          <div id="divSearchPanel"  visible="true">
              <div style="float: left; background-color: #b7ceec; width:450px;height:180px;margin:2px;">
                  <table cellpadding="2" cellspacing="2" width="100%" border="0" style="border-color: White;">
                     
                      <tr align="left">
                          <td colspan="4">
                              <div style="padding-left: 15px;">
                                  <div style="float: left; margin-right: 5px;">
                                      <div style="float: left;">
                                          Date&nbsp;From&nbsp;:&nbsp;
                                      </div>
                                      <div style="float: left;">
                                          <dxe:ASPxDateEdit ID="txtDateFrom" runat="server" Width="100px" EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" TabIndex="0" ClientInstanceName="ctxtDateFrom1">
                                              <buttonstyle>
                                              </buttonstyle>
                                              <clientsideevents datechanged="function(s,e){DateChange(ctxtDateFrom1);}">
                                              </clientsideevents>
                                          </dxe:ASPxDateEdit>
                                      </div>
                                  </div>
                                  <div style="float: left;">
                                      <div style="float: left;">
                                          Date&nbsp;To&nbsp;:&nbsp;
                                      </div>
                                      <div style="float: left;">
                                          <dxe:ASPxDateEdit ID="txtDateTo" runat="server" Width="100px" EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="True" TabIndex="0" ClientInstanceName="ctxtDateTo1">
                                              <buttonstyle>
                            </buttonstyle>
                                              <clientsideevents datechanged="function(s,e){DateChange(ctxtDateTo1);}">
                            </clientsideevents>
                                          </dxe:ASPxDateEdit>
                                      </div>
                                  </div>
                              </div>
                          </td>
                      </tr>
                      <tr align="left">
                          <td colspan="4">
                              <div>
                                  <div style="float: left;">
                                      Main&nbsp;Account&nbsp;:&nbsp;
                                  </div>
                                  <div style="float: left;">
                                      <asp:TextBox ID="txtMainAc" runat="server" Width="246" TabIndex="0" onkeyup="CallMainAccount(this,'GenericAjaxList',event);">
                                      </asp:TextBox>
                                      <asp:HiddenField ID="txtMainAc_hidden" runat="server" />
                                      <div id="divMessage" style="display: none; color: Red; font-weight: bold;">
                                          Enter&nbsp;Main&nbsp;Account&nbsp;Name
                                      </div>
                                  </div>
                              </div>
                          </td>
                      </tr>
                      <tr align="left">
                          <td align="left" valign="middle" colspan="4">
                              <div>
                                  <div style="float: left;">
                                      Sub&nbsp;Account&nbsp;Type&nbsp;:
                                  </div>
                                  <div style="float: left;">
                                      <asp:RadioButtonList ID="rlstSubAcType" runat="server" Width="240" AutoPostBack="false" RepeatDirection="Horizontal" Visible="true" CssClass="radioButtonList" TabIndex="0">
                                          <asp:ListItem Selected="True" Value="1" Text="All" onclick="changeState('1');"></asp:ListItem>
                                          <asp:ListItem Value="0" Text="Selected" onclick="changeState('0');"></asp:ListItem>
                                      </asp:RadioButtonList>
                                  </div>
                              </div>
                          </td>
                      </tr>
                      <tr>
                          <td align="left" colspan="4">
                              <div>
                                  <div style="float: left;">
                                      Interest&nbsp;Type&nbsp;:
                                  </div>
                                  <div style="float: left;">
                                      <asp:RadioButtonList ID="rbtnInterestType" runat="server" AutoPostBack="false" RepeatDirection="Horizontal" CssClass="radioButtonList" TabIndex="0">
                                          <asp:ListItem Selected="True" Value="1" Text="All Entries"></asp:ListItem>
                                          <asp:ListItem Value="0" Text="Non Zero"></asp:ListItem>
                                      </asp:RadioButtonList>
                                  </div>
                              </div>
                          </td>
                      </tr>
                      <tr>
                          <td>
                          </td>
                          <td align="left">
                              <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" OnClientClick="return Validate();" TabIndex="0" />
                          </td>
                          <td colspan="2">
                          </td>
                      </tr>
                  </table>
              </div>
              <div id="tblSubAc" style="float: left; display: none; background-color:#b7ceec; height:180px;">
                  <table cellspacing="0" style="background-color: #b7ceec;">
                      <tr align="left">
                          <td align="left">
                              Sub Account :<br />
                              <asp:TextBox ID="txtSubAc" runat="server" Width="240" TabIndex="0" onkeyup="CallSubAccount(this,'GenericAjaxList',event);">
                              </asp:TextBox>
                              <asp:HiddenField ID="txtSubAc_hidden" runat="server" />
                          </td>
                          <td>
                              <br />
                              <asp:LinkButton ID="lbtnAddSubAc" runat="server" Text="Add" ForeColor="blue" Font-Underline="true" TabIndex="0" OnClick="lbtnAddSubAc_Click"></asp:LinkButton>
                          </td>
                          <td align="left">
                          </td>
                      </tr>
                      <tr align="left">
                          <td colspan="2" align="left">
                              <asp:ListBox ID="lbSubAc" runat="server" Width="242" TabIndex="0" Height="100"></asp:ListBox>
                          </td>
                          <td>
                          </td>
                      </tr>
                      <tr align="left">
                          <td>
                              <asp:LinkButton ID="lbtnDone" runat="server" Text="Done" TabIndex="0" ForeColor="green" Font-Underline="true" OnClientClick="changeState('1');return false;">
                              </asp:LinkButton>
                              <asp:LinkButton ID="lbtnRemove" runat="server" Text="Remove" TabIndex="0" ForeColor="red" Font-Underline="true" OnClick="btnRemove_Click">
                              </asp:LinkButton>
                          </td>
                          <td colspan="2">
                          </td>
                      </tr>
                  </table>
              </div>
          </div>
         </td>
      </tr>
      <tr>
        <td colspan="2" align="left">
        <div>
          <div style="float:left;">
          <asp:Button ID="btnFilter" runat="server" Text="Filter"  CssClass="hidingButton" OnClick="btnFilter_Click" OnClientClick="ShowHide('1'); return false;"/>
           </div>
           <div style="float:left; padding: 0 0 0 5px;">
            <asp:Button ID="btnExportSummary" runat="server" CssClass="hidingButton" Text="Export to excel" OnClick="btnExport_Click"  />
           </div>
           </div>
        </td>
      </tr>
      <tr>
        <td  colspan="2" align="right" style="width: 100%;" pagersettings-position="TopAndBottom">
              <asp:GridView ID="gvInterestSummary" runat="server" AllowSorting="true" AutoGenerateColumns="false"
                        Width="100%" OnPageIndexChanged="gvInterestSummary_PageIndexChanged" OnPageIndexChanging="gvInterestSummary_PageIndexChanging"
                        OnRowCommand="gvInterestSummary_RowCommand" OnRowDataBound="gvInterestSummary_RowDataBound"
                        OnSorted="gvInterestSummary_Sorted" OnSorting="gvInterestSummary_Sorting" AllowPaging="True" HeaderStyle-BackColor="#B7CEEC" EmptyDataRowStyle-Font-Size="Larger" EmptyDataText="No Record Found" EmptyDataRowStyle-HorizontalAlign="Left" EmptyDataRowStyle-Font-Bold="true" PagerSettings-Position="TopAndBottom">
                        <Columns>
                            <asp:TemplateField HeaderText="Client Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("ClientName")%>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" ForeColor="Blue" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Branch" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" >
                                <ItemTemplate>
                                    <%#Eval("Branch")%>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" ForeColor="Blue" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Segments" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" >
                                <ItemTemplate>
                                    <%#Eval("Segments")%>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" ForeColor="Blue" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Post&nbsp;Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                      <asp:Label ID="lblPostDate" runat="server" Text='<%#GetDateFormat(Eval("CreateTime"))%>' ></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" ForeColor="Blue" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Interest&nbsp;Rate" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("Interest")%>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" ForeColor="Blue" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Service&nbsp;Tax" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("SrvTax")%>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" ForeColor="Blue" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Creation&nbsp;Time" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                     <asp:Label ID="lblCreationTime" runat="server" Text='<%#GetDateTimeFormat(Eval("CreateTime"))%>' ></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" ForeColor="Blue" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnDetails" runat="server" Text="Details" ForeColor="blue" CommandName="det" OnClientClick="ShowHide('0');" 
                                        CommandArgument='<%#Eval("Reference")%>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                  <EmptyDataRowStyle Font-Bold="True" Font-Size="Large" />
                  <HeaderStyle BackColor="#B7CEEC" />
                    </asp:GridView> <br />
                    <asp:HiddenField ID="hdnSubAcNo" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnSelectedSubAcs" runat="server" Value="'0'" />
        </td>
      </tr>
    </table>
    
       <div id="divOverlapping" style="position: fixed; height: 100%; width: 100%; background-color: #000;
            top: 0px; left: 0px; opacity: 0.5; filter: alpha(opacity=50); z-index: 60; display: none;">
        </div>
        <div id="divPopUp" style="background-color: #B7CEEC; height: 200px; left: 81px;
            position: absolute; top: 25px; width: 966px; left: 20px; z-index: 100; display: none;
            padding-bottom: 10px;">
            <div style="background-color: #B7CEEC; height: 7px; font-size: small; color: White;
                padding-bottom: 7px; padding-left: 0px;">
                <div style="background-color: Black; font-weight: bold; font-size: medium;color:White;">
                    Interest Details
                    <%-- <img  src="../windowfiles/close.gif" height="16px" alt="CLOSE" onclick="ClosePopUp();" style="padding-left:98%; margin: 0px;"  />--%>
                    <asp:ImageButton ID="ibtnClosing" runat="server" ImageUrl="../windowfiles/close.gif"
                        OnClick="ibtn_Close" OnClientClick="ClosePopUp();ShowHide('0');return false;" Width="20px" Style="padding-left: 97%; margin: 0px; padding-top: -25px;
                        top: -10px;" />
                </div>
                <div style="background-color: #B7CEEC; color: Black;">
                     <table style="background-color:#DDECFE;" >
                       <tr>
                          <td align="right">
                                Export Type : <asp:DropDownList ID="ddlExportType" runat="server" AutoPostBack="false" OnSelectedIndexChanged="ddlExportType_SelectedIndexChanged">
                                   <asp:ListItem Selected="True" Value="0" Text="Select" onclick="changeState('1');"></asp:ListItem>
                                   <asp:ListItem  Value="1" Text="Excel"></asp:ListItem>
                                   <asp:ListItem  Value="2" Text="PDF"></asp:ListItem>
                                </asp:DropDownList>
                               &nbsp;&nbsp;<asp:Button ID="btnGenerateReport" runat="server" Text="Generate Report" OnClick="btnGenerateReport_Click" />
                          </td>
                       </tr>
                      
                       <tr>
                          <td align="left">
                           
                         
                    <asp:GridView ID="gvInterestDetails" runat="server" AllowPaging="true" PageSize="10"
                        OnPageIndexChanged="gvInterestDetails_PageIndexChanged" OnPageIndexChanging="gvInterestDetails_PageIndexChanging" AutoGenerateColumns="false" HeaderStyle-BackColor="#B7CEEC" EmptyDataRowStyle-Font-Size="Large">
                        <Columns>
                            <asp:TemplateField HeaderText="Client Name">
                                <ItemTemplate>
                                    <%#Eval("ClientName")%>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" ForeColor="Blue" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Branch">
                                <ItemTemplate>
                                    <%#Eval("Branch")%>
                                   
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" ForeColor="Blue" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Segments">
                                <ItemTemplate>
                                    <%#Eval("Segments")%>
                                    
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" ForeColor="Blue" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Txndate">
                                <ItemTemplate>
                                    <%#Eval("Txndate")%>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" ForeColor="Blue" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Opening Balance" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%#Eval("OpeningBalance")%>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" ForeColor="Blue" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Day Net" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%#Eval("DayNet")%>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" ForeColor="Blue" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Closing Balance" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%#Eval("ClosingBalance")%>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" ForeColor="Blue" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Balance Type">
                                <ItemTemplate>
                                    <%#Eval("BalanceType")%>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" ForeColor="Blue" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Charged On">
                                <ItemTemplate>
                                    <%#Eval("ChargedOn")%>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" ForeColor="Blue" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Interest Rate" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%#Eval("InterestRate")%>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" ForeColor="Blue" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Interest" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%#Eval("Interest")%>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" ForeColor="Blue" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    
                     </td>
                       </tr>
                     </table>
                </div>
            </div>
        </div>
</asp:Content>
