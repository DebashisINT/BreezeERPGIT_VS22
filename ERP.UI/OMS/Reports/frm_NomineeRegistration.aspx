<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" 
    Inherits="ERP.OMS.Reports.Reports_frm_NomineeRegistration" Codebehind="frm_NomineeRegistration.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>

    <style type="text/css">
	
	/* Big box with list of options */
	#ajax_listOfOptions{
		position:absolute;	    /* Never change this one */
		width:50px;	        /* Width of box */
		height:auto;	        /* Height of box */
		overflow:auto;	        /* Scrolling features */
		border:1px solid Blue;	/* Blue border */
		background-color:#FFF;	/* White background color */
		text-align:left;
		font-size:0.9em;
		z-index:100;
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
		z-index:900;
	}
	#ajax_listOfOptions_iframe{
		background-color:#F00;
		position:absolute;
		z-index:10;
	}
	
	form{
		display:inline;
	}
	
	</style>
	<script language="javascript" type="text/javascript">
	FieldName='none';
   function SignOff()
    {
        window.parent.SignOff();
    }
    function height()
    {
        if(document.body.scrollHeight>=450)
            window.frameElement.height = document.body.scrollHeight;
        else
            window.frameElement.height = '450px';
        window.frameElement.Width = document.body.scrollWidth;
    }
    
    
 function ShowError(obj)
    {
   
        if(obj!='')
            {
              var msgvalue=obj.split('~');
              if(msgvalue[0]=='D')
                {
                 if(msgvalue[1]=='no')
                    alert('Not Deleted, There May be Some Problem');
                                
                }
               else if(msgvalue[0]=='edit')   
                    {
                        if(msgvalue[1]=='Y')
                            {
                             combGuardianCountry.SetEnabled(true);
                             combGuardianState.SetEnabled(true);
                             combGuardianCity.SetEnabled(true);
                             dtDOBMinor.SetEnabled(true);
                             txtGuardianAddress.SetEnabled(true);
                             txtGuardianPin.SetEnabled(true);
                            
                            
                            }
                         else
                            {
                             combGuardianCountry.SetEnabled(false);
                             combGuardianState.SetEnabled(false);
                             combGuardianCity.SetEnabled(false);
                             dtDOBMinor.SetEnabled(false);
                             txtGuardianAddress.SetEnabled(false);
                             txtGuardianPin.SetEnabled(false);
                            
                            
                            }
                    
                    }
                  else if(msgvalue[0]=='new')   
                    {
                         combGuardianCountry.SetEnabled(false);
                         combGuardianState.SetEnabled(false);
                         combGuardianCity.SetEnabled(false);
                         dtDOBMinor.SetEnabled(false);
                         txtGuardianAddress.SetEnabled(false);
                         txtGuardianPin.SetEnabled(false);
                            
                    }
            
            }
            height();
    }
    
    function OnCountryChanged(cmbCountry) {
        var cmbState = grid.GetEditor("combState");
        var cmbCountryValue = cmbCountry.GetValue();
        combState.PerformCallback(cmbCountryValue);
        if(cmbCountryValue==null)
            {
               
                var Statevalue='';
                combCity.PerformCallback(Statevalue);
            }
        //cmbState.PerformCallback();
        
    //     cmbState.PerformCallback(cmbCountry.GetValue().toString());
    //    if (cmbCountryValue == 'UK')
    //        cmbCity.SetEnabled(false);
    //    else
    //        cmbCity.PerformCallback(cmbCountry.GetValue().toString());
    }
    function OnStateChanged(cmbState)
    {
        var cmbStatevalue=cmbState.GetValue();
        combCity.PerformCallback(cmbStatevalue);
    
    
    }
    
     function OnCountryChangedGuardian(combGuardianCountry) {
//        var cmbState = grid.GetEditor("combGuardianState");
        var cmbGuardianCountryValue = combGuardianCountry.GetValue();
        combGuardianState.PerformCallback(cmbGuardianCountryValue);
        if(cmbGuardianCountryValue==null)
            {
               
                var StatevalueGuardian='';
                combGuardianCity.PerformCallback(StatevalueGuardian);
            }
        
    }
    function OnStateChangedGuardian(cmbStateGuardian)
    {
        var cmbStatevalueGuardian=cmbStateGuardian.GetValue();
        combGuardianCity.PerformCallback(cmbStatevalueGuardian);
    
    
    }
    
    function FetchBenIdNSDL(obj1,obj2,obj3)
        {
          
          var strQuery_Table = "master_nsdlclients";
           var strQuery_FieldName = " top 10 cast(NsdlClients_BenAccountID as varchar)+' ['+rtrim(ltrim(NsdlClients_BenFirstHolderName))+']' as BenId,NsdlClients_BenAccountID as BenAccId";
           var strQuery_WhereClause = " NsdlClients_BenAccountID like (\'%RequestLetter%\') or  (NsdlClients_BenFirstHolderName like (\'%RequestLetter%\')) ";;
           var strQuery_OrderBy='';
           var strQuery_GroupBy='';
           var CombinedQuery=new String(strQuery_Table+"$"+strQuery_FieldName+"$"+strQuery_WhereClause+"$"+strQuery_OrderBy+"$"+strQuery_GroupBy);
           ajax_showOptions(obj1,obj2,obj3,replaceChars(CombinedQuery));   
        
        
        }
        
         function FetchBenIdCDSL(obj1,obj2,obj3)
        {
          
          var strQuery_Table = "master_cdslclients";
           var strQuery_FieldName = " top 10 cast(CdslClients_BenAccountNumber as varchar)+' ['+rtrim(ltrim(CdslClients_FirstHolderName))+']' as BenId,CdslClients_BenAccountNumber as BenAccId";
           var strQuery_WhereClause = " CdslClients_BenAccountNumber like (\'%RequestLetter%\') or  (CdslClients_FirstHolderName like (\'%RequestLetter%\')) ";;
           var strQuery_OrderBy='';
           var strQuery_GroupBy='';
           var CombinedQuery=new String(strQuery_Table+"$"+strQuery_FieldName+"$"+strQuery_WhereClause+"$"+strQuery_OrderBy+"$"+strQuery_GroupBy);
           ajax_showOptions(obj1,obj2,obj3,replaceChars(CombinedQuery));   
        
        
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
    function TMinor(objminorvalue)
        {
//            alert('aa');
            var ab=document.getElementById('grdNominees_efnew_trGuardianAddress');
            alert(ab);
           if(objminorvalue=='Y')
                document.getElementById('grdNominees_efnew_trGuardianAddress').style.display='inline';
           else
            document.getElementById('grdNominees_efnew_trGuardianAddress').style.display='none';
        
        
        }
        
        function ShowHideMinorDetails(objminorvalue)
        {
           

           if(objminorvalue=='Y')
                {
                 combGuardianCountry.SetEnabled(true);
                 combGuardianState.SetEnabled(true);
                 combGuardianCity.SetEnabled(true);
                 dtDOBMinor.SetEnabled(true);
                 txtGuardianAddress.SetEnabled(true);
                 txtGuardianPin.SetEnabled(true);
                 
                }
           else
                {
                 combGuardianCountry.SetEnabled(false);
                 combGuardianState.SetEnabled(false);
                 combGuardianCity.SetEnabled(false);
                 dtDOBMinor.SetEnabled(false);
                 txtGuardianAddress.SetEnabled(false);
                 txtGuardianPin.SetEnabled(false);
                 
                }
        
        
        }
        function ShowHideMinorDetails1(objminorvalue)
        {
//            alert('aa');

           if(objminorvalue=='Y')
                {
                    
//                    combIsMinor.PerformCallback(objminorvalue);
                    var aa='';
                }
           else
               {
                   
//                    combIsMinor.PerformCallback(objminorvalue);
                    var bb='';
               }
        
        
        }      
        
        
       function TMinor1(objMinor)
            {
//               alert(objMinor);
//               alert(objMinor.id); 
//               alert(objMinor.Id); 
                combGuardianCountry.SetEnabled(false);
            
            
            }
            
     function NomineeDelete(obj1,obj2)
        {
           var conf=confirm('Are you sure to delete?'); 
            if(conf)
                {
                var obj3=obj2 +'~' + obj1;
                 grid.PerformCallback(obj3);   
                               
                }
        
        }
    function ShowHideFilter(obj)
        {
          grid.PerformCallback(obj);
        }
    function Callbackcity()
        {
            var Statevalue='';
             combCity.PerformCallback(Statevalue);
            
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table  width="100%">
                    <tr>
                            <td colspan="2" class="EHEADER" align="center"  >
                                    <strong><span id="SpanHeader" style="color: #000099">Nominee Registration</span></strong>
                            </td>
                                    
                                
                        </tr>
                        <tr>
                            <td>
                              <table>
                                <tr>
                                    <td id="ShowFilter">
                                        <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">
                                            Show Filter</span></a>
                                    </td>
                                    <td id="Td1">
                                        <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">
                                            All Records</span></a>
                                    </td>
                                </tr>
                            </table>  
                                
                            </td>
                            <td align="right" style="width:200px">
                               <asp:DropDownList ID="drpExport" AutoPostBack="true" OnSelectedIndexChanged="drpExport_SelectIndexChanged"  runat="Server">
                                    <asp:ListItem Text="Export" Value="Export"></asp:ListItem>
                                    <asp:ListItem Text="Excel" Value="E"></asp:ListItem>
                                   </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
      <table width="100%">
        <tr>
            <td align="center">
                    <dxe:ASPxGridView ID="grdNominees" runat="server" AutoGenerateColumns="False"
                        KeyFieldName="NomineeRegisterID" ClientInstanceName="grid" DataSourceID="sqlDsNominees"
                        Width="100%" OnRowUpdating="grdNominees_OnRowUpdating" OnCustomCallback="grdNominees_CustomCallback"
                        OnRowValidating="grdNominees_OnRowValidating" OnHtmlDataCellPrepared="grdNominees_OnHtmlDataCellPrepared"
                        OnRowInserting="grdNominees_OnRowInserting" OnHtmlEditFormCreated="grdNominees_HtmlEditFormCreated"
                        OnStartRowEditing="grdNominees_StartRowEditing" OnHtmlRowCreated="grdNominees_HtmlRowCreated"
                        OnCustomJSProperties="grdNominees_CustomJSProperties" OnProcessColumnAutoFilter="grdNominees_ProcessColumnAutoFilter" >
                        <ClientSideEvents EndCallback="function(s,e) { ShowError(s.cpmsg);

                                        }
" /> 
                        <Templates>
                            <EditForm>
                                 <table style="background-color: #ffdead;"   border="0">
                                  <tr>
                <td valign="top" align="left" colspan="4" style="font-weight:bold; height:30px">Nominee Details</td>
            </tr>
            <tr>
                <td style="width:130px" align="left">
                    Registration Number :
                </td>
                <td style="width:265px" align="left">
                    <asp:TextBox ID="txtRegistrationNo" MaxLength="20" Text='<%#Bind("RegNo") %>' Width="210px" runat="server"></asp:TextBox>
                                       
                </td>
                <td style="width:130px" align="left">
                    Registration Date :
                </td>
                <td style="width:265px" align="left">
                       <dxe:ASPxDateEdit ID="dtRegistrationDate" runat="server" EditFormat="Custom" UseMaskBehavior="true"  Width="214px" EditFormatString="dd-MM-yyyy">
                                                                <ButtonStyle Width="13px">
                                                                </ButtonStyle>
                                                            </dxe:ASPxDateEdit>
                         <asp:Label ID="lblRegDate" Text='<%#Bind("RegDate") %>' runat="Server" Visible="false"></asp:Label>                                     
                </td>
            </tr>
             <tr>
                <td align="left">
                    BenID :
                </td>
                <td align="left">
                    <asp:TextBox ID="txtBenId" Text='<%#Bind("BenAcc") %>'  Width="210px" runat="server"></asp:TextBox>
                </td>
                <td align="left">
                    Name :
                </td>
                <td align="left">
                    <asp:TextBox ID="txtName" Text='<%#Bind("Name") %>' Width="210px" MaxLength="150"  runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left">
                    Address :
                </td>
                <td align="left">
                    <asp:TextBox ID="txtAddress" Text='<%#Bind("address") %>' MaxLength="150"  Width="210px" runat="server"></asp:TextBox>
                </td>
                <td align="left">
                    Country :
                </td>
                <td align="left">
                    <dxe:ASPxComboBox ID="combCountry" ClientInstanceName="combCountry" runat="server"
                           DataSourceID="SqlDSCountry" EnableIncrementalFiltering="true" Width="215px" TextField="Country_ShortName" ValueField="Country_ID"  
                        
                                               >
                                               <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCountryChanged(s); }" />
                                            </dxe:ASPxComboBox>
                         <asp:Label ID="lblCountry" Text='<%#DataBinder.Eval(Container.DataItem,"country")%>' runat="Server" Visible="false"></asp:Label>  
               
                   
                </td>
            </tr>
            
            <tr>
                <td align="left">
                    State :
                </td>
                <td align="left">
                   <dxe:ASPxComboBox ID="combState" ClientInstanceName="combState" runat="server"
                                                EnableIncrementalFiltering="true" Width="215px"   
                                                TextField="State_ShortName" ValueField="State_ID" 
                                             OnCallback="combState_Callback"  >
                                             <ClientSideEvents SelectedIndexChanged="function(s, e) { OnStateChanged(s); }" />
                                            </dxe:ASPxComboBox>
                                            <asp:Label ID="lblState" Text='<%#DataBinder.Eval(Container.DataItem,"state")%>' runat="Server" Visible="false"></asp:Label>  
                </td>
                <td align="left">
                    City :
                </td>
                <td align="left">
                    <dxe:ASPxComboBox ID="combCity" ClientInstanceName="combCity" runat="server"
                                                EnableIncrementalFiltering="true" Width="215px"  OnCallback="combCity_Callback"
                                                TextField="City_ShortName" ValueField="City_ID" 
                                               >
                                            </dxe:ASPxComboBox>
                            <asp:Label ID="lblCity" Text='<%#DataBinder.Eval(Container.DataItem,"city")%>' runat="Server" Visible="false"></asp:Label>  
                    
                </td>
            </tr>
            
                       
            
            <tr>
                <td align="left">
                    PinCode :
                </td>
                <td align="left">
                    <asp:TextBox ID="txtPin" Text='<%#Bind("pincode") %>' MaxLength="10"  Width="210px" runat="server"></asp:TextBox>
                </td>
                <td align="left">
                     Residence Phone :
                </td>
                <td align="left">
                     
                       <asp:TextBox ID="txtResidencePhone" Text='<%#Bind("resphone") %>' MaxLength="20" Width="210px" runat="server"></asp:TextBox>                   
                                        
                </td>
            </tr>
          
            
            
            <tr>
                <td align="left">
                  Mobile :
                </td>
                <td align="left">
                    <asp:TextBox ID="txtMobile" Text='<%#Bind("mob") %>' MaxLength="10" Width="210px" runat="server"></asp:TextBox>
                    
                </td>
                <td align="left">
                    Email :
                </td>
                <td align="left">
                    <asp:TextBox ID="txtEmail" Text='<%#Bind("email") %>' MaxLength="120" Width="210px" runat="server"></asp:TextBox>
                                                          
                </td>
            </tr>
            
            <tr>
                <td align="left">
                   Nomination : 
                </td>
                <td align="left">
                    <dxe:ASPxComboBox ID="combNomination" ClientInstanceName="combNomination" runat="server"
                      SelectedIndex="0"  ValueType="System.String" EnableIncrementalFiltering="true" Width="215px"    >
                                   <Items>
                                        <dxe:ListEditItem Text="No" Value="N" />
                                        <dxe:ListEditItem Text="Yes" Value="Y" />
                                        
                                    </Items>
                    
                                            </dxe:ASPxComboBox>
                   <asp:Label ID="lblNomination" Text='<%#DataBinder.Eval(Container.DataItem,"nomination")%>' Visible="false" runat="Server"></asp:Label>
                </td>
                <td align="left">
                  IsMinor :
                </td>
                <td align="left">
                    
 <dxe:ASPxComboBox ID="combIsMinor" ClientInstanceName="combIsMinor" runat="server"
                      SelectedIndex="0"   ValueType="System.String" EnableIncrementalFiltering="true" Width="215px" >
                                   <Items>
                                        <dxe:ListEditItem Text="No" Value="N" />
                                        <dxe:ListEditItem Text="Yes" Value="Y" />
                                        
                                    </Items>
                                <ClientSideEvents SelectedIndexChanged="function(s, e){ ShowHideMinorDetails(s.GetValue());}" />
                                            </dxe:ASPxComboBox>
                              <asp:Label ID="lblMinor" Text='<%#DataBinder.Eval(Container.DataItem,"minor")%>' runat="Server" Visible="false"></asp:Label>
                                                          
                </td>
            </tr>
            <tr>
                <td align="left">
                   Remark :
                </td>
                <td align="left">
                    <asp:TextBox ID="txtRemark" MaxLength="150" Text='<%#Bind("remarks") %>'  Width="210px" TextMode="MultiLine" runat="server"></asp:TextBox>
                </td>
                <td align="left">
                   
                </td>
                <td align="left">
                   
                                                          
                </td>
            </tr>
            
        </table>
                
         <table style="background-color: #E7C2AB;"   border="0">
            <tr>
                <td valign="top" align="left" colspan="4" style="font-weight:bold;height:30px">Guardian Details (If Nominee Is Minor)</td>
            </tr>
              <tr id="trGuardianAddress" runat="server">
                <td style="width:130px" align="left">
                    Address :
                </td>
                <td style="width:265px" align="left">
                  
                    <dxe:ASPxTextBox ID="txtGuardianAddress" Text='<%#Bind("gaddress") %>' ClientInstanceName="txtGuardianAddress"  runat="server" MaxLength="150" Width="215px"></dxe:ASPxTextBox>
                </td>
                <td style="width:130px" align="left">
                    Country :
                </td>
                <td style="width:265px" align="left">
                        <dxe:ASPxComboBox ID="combGuardianCountry" ClientInstanceName="combGuardianCountry" runat="server"
                           DataSourceID="SqlDSCountry"   EnableIncrementalFiltering="true" Width="215px" TextField="Country_ShortName" ValueField="Country_ID" 
                        
                                               >
                                               <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCountryChangedGuardian(s); }" />
                                            </dxe:ASPxComboBox>
                                            <asp:Label ID="lblGuardianCountry" Text='<%#DataBinder.Eval(Container.DataItem,"gcountry")%>' Visible="false" runat="Server"></asp:Label>
                
                     
                </td>
            </tr>
            <tr>
                <td align="left">
                    State :
                </td>
                <td align="left">
                    <dxe:ASPxComboBox ID="combGuardianState" ClientInstanceName="combGuardianState" runat="server"
                                                  EnableIncrementalFiltering="true" Width="215px"   
                                                TextField="State_ShortName" ValueField="State_ID" 
                                             OnCallback="combGuardianState_Callback"  >
                                             <ClientSideEvents SelectedIndexChanged="function(s, e) { OnStateChangedGuardian(s); }" />
                                            </dxe:ASPxComboBox>
                                            <asp:Label ID="lblGuardianState" Text='<%#DataBinder.Eval(Container.DataItem,"gstate")%>'  runat="Server" Visible="false"></asp:Label>
                </td>
                <td align="left">
                    City :
                </td>
                <td align="left">
                     <dxe:ASPxComboBox ID="combGuardianCity" ClientInstanceName="combGuardianCity" runat="server"
                                                EnableIncrementalFiltering="true" Width="215px"  OnCallback="combGuardianCity_Callback"
                                                TextField="City_ShortName" ValueField="City_ID" 
                                               >
                                            </dxe:ASPxComboBox>
                                            <asp:Label ID="lblGuardianCity" Text='<%#DataBinder.Eval(Container.DataItem,"gcity")%>' runat="Server" Visible="false"></asp:Label>
                </td>
            </tr>
                        
            <tr>
                <td align="left">
                    Pin Code :
                </td>
                <td align="left">
                    
                 <dxe:ASPxTextBox ID="txtGuardianPin" Text='<%#Bind("gpin") %>' ClientInstanceName="txtGuardianPin"  runat="server" MaxLength="10" Width="215px"></dxe:ASPxTextBox>
                </td>
                <td align="left">
                     Date of Birth of Minor :
                </td>
                <td align="left">
               
                    
                    
                    <dxe:ASPxDateEdit ID="dtDOBMinor" ClientInstanceName="dtDOBMinor" runat="server" EditFormat="Custom" UseMaskBehavior="true" EditFormatString="dd-MM-yyyy"
                                                                Width="214px">
                                                                <ButtonStyle Width="13px">
                                                                </ButtonStyle>
                                                            </dxe:ASPxDateEdit>
                       <asp:Label ID="lblDOBMinor" Text='<%#Bind("dobminor") %>' runat="Server" Visible="false"></asp:Label>
                    
                </td>
            </tr>
         </table>               
                               
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <controls></controls>
                                            <dxe:ASPxGridViewTemplateReplacement ID="Editors" runat="server" ColumnID="" ReplacementType="EditFormEditors">
                                            </dxe:ASPxGridViewTemplateReplacement>
                                            <div style="text-align: center; padding: 2px 2px 2px 2px; font-weight: bold;">
                                                <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                    runat="server">
                                                </dxe:ASPxGridViewTemplateReplacement>
                                                <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                    runat="server">
                                                </dxe:ASPxGridViewTemplateReplacement>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </EditForm>
                        </Templates>
                        <SettingsBehavior ConfirmDelete="True" AllowFocusedRow="True" />
                        <Styles>
                            <Header CssClass="gridheader" SortingImageSpacing="5px" ImageSpacing="5px">
                            </Header>
                            <FocusedRow CssClass="gridselectrow">
                            </FocusedRow>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <FocusedGroupRow CssClass="gridselectrow">
                            </FocusedGroupRow>
                        </Styles>
                        <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                        </SettingsPager>
                        <Columns>
                        
                            <dxe:GridViewDataTextColumn Width="150px" Caption="Nominee Name" FieldName="Name"  VisibleIndex="1">
                               
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False" VisibleIndex="7"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Width="150px" Caption="Registration No"  FieldName="RegNo" VisibleIndex="2">
                                
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False" VisibleIndex="9"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Width="150px" Caption="Ben ID" FieldName="BenID" VisibleIndex="3">
                               
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False" VisibleIndex="11"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Width="150px" Caption="Name" FieldName="BenAccName"  VisibleIndex="4">
                               
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                               <EditFormSettings Visible="false" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Width="150px" Caption="Registration Date" FieldName="RegDateShow" VisibleIndex="5">                                
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False" VisibleIndex="4"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            
                           
                            <dxe:GridViewCommandColumn Width="100px" VisibleIndex="6" Caption="Edit" ShowEditButton="True">
                                <HeaderStyle HorizontalAlign="Center">
                                </HeaderStyle>
                                <HeaderTemplate>
                                    <a href="javascript:void(0);" onclick="grid.AddNewRow()">
                                        <span style="color: #000099;
                                                                                text-decoration: underline">Add New</span>
                                    </a>
                                </HeaderTemplate>
                            </dxe:GridViewCommandColumn>
                            
                            <dxe:GridViewDataTextColumn Width="100px" VisibleIndex="7">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <DataItemTemplate>
                                    <a href="javascript:void(0);" onclick="NomineeDelete('<%# Container.KeyValue %>','D')" >
                                        <span style="color: #000099; text-decoration: underline">Delete</span></a>
                                </DataItemTemplate>
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            
                        </Columns>
                        <SettingsEditing PopupEditFormHeight="400px" PopupEditFormHorizontalAlign="Center"
                            PopupEditFormModal="True" Mode="EditForm" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="500px" />
                        <SettingsText PopupEditFormCaption="Add/Modify Main Account" ConfirmDelete="Are you sure You Want To Delete ?" />
                        <Settings ShowGroupedColumns="True" ShowGroupPanel="True" />
                    </dxe:ASPxGridView>
            
            </td>
        </tr>
        <tr>
            <td>
              
            
            
             </td>
        </tr>
    </table>
    <asp:SqlDataSource ID="sqlDsNominees" runat="server" ConflictDetection="CompareAllValues"
          
             SelectCommand="">
          </asp:SqlDataSource>
          
            <asp:SqlDataSource ID="SqlDSCountry" runat="server" ConflictDetection="CompareAllValues"
           
             SelectCommand="select Country_ShortName,Country_ID from Master_Countries">
          </asp:SqlDataSource>
    </div>
</asp:Content>
