<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/OMS/MasterPage/PopUp.Master"
    Inherits="ERP.OMS.Management.Master.management_master_Contact_DPDetails" Codebehind="Contact_DPDetails.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <script type="text/javascript">
    function disp_prompt(name)
    {
        //var ID = document.getElementById(txtID);
        if ( name == "tab0")
        {
        //alert(name);
        document.location.href="Contact_general.aspx"; 
        }
        if ( name == "tab1")
        {
        //alert(name);
        document.location.href="Contact_Correspondence.aspx"; 
        }
        else if ( name == "tab2")
        {
        //alert(name);
        document.location.href="Contact_BankDetails.aspx"; 
        }
        else if ( name == "tab3")
        {
        //alert(name);
        //document.location.href="Contact_DPDetails.aspx"; 
        }
        else if ( name == "tab4")
        {
        //alert(name);
        document.location.href="Contact_Document.aspx"; 
        }
        else if ( name == "tab12")
        {
        //alert(name);
        document.location.href="Contact_FamilyMembers.aspx"; 
        }
        else if ( name == "tab5")
        {
        //alert(name);
        document.location.href="Contact_Registration.aspx"; 
        }
        else if ( name == "tab7")
        {
        //alert(name);
        document.location.href="Contact_GroupMember.aspx"; 
        }
        else if ( name == "tab8")
        {
        //alert(name);
        document.location.href="Contact_Deposit.aspx"; 
        }
        else if ( name == "tab9")
        {
        //alert(name);
        document.location.href="Contact_Remarks.aspx"; 
        }
        else if ( name == "tab10")
        {
        //alert(name);
        document.location.href="Contact_Education.aspx"; 
        }
        else if ( name == "tab11")
        {
        //alert(name);
        document.location.href="contact_brokerage.aspx"; 
        }
        else if ( name == "tab6")
        {
        //alert(name);
            document.location.href="contact_other.aspx"; 
        }
         else if(name=="tab13")
        {
             document.location.href="contact_Subscription.aspx";
        }
       
    }
    function CallList(obj,obj1,obj2)
        {
            ajax_showOptions(obj,obj1,obj2);
        } 
        FieldName='ASPxPageControl1_DpDetailsGrid_DXSelInput';
    function setvaluetovariable(obj)
    {
        if(obj=='1')
        {
            document.getElementById("TrPoaName").style.display="none";
        }
        else    
        {
            document.getElementById("TrPoaName").style.display="inline";
        }
    }
    
    
      function DeleteRow(keyValue)
    {
         doIt=confirm('Confirm delete?');
            if(doIt)
                {
                   gridDp.PerformCallback('Delete~'+keyValue);
                   height();
                }
            else{
                  
                }

   
    }
    function Emailcheck(obj,obj2)
     {
        if(obj =='N')
        {
            if(obj !='B')
             {  
            alert("Transactions exists for this ClientID...Deletion disallowed!!");   
            obj='B';     
            }
        }
        if(obj2 !='Y')
        {
       
               INR =confirm('Warning!!.\n\nThis DP Id and ClientId already assigned to  '+ obj2 +'.\n\nClick OK to Accept,Otherwise Click Cancel');
                   if(INR)
                  {
              
                                    
                                     WAR2 =confirm('Warning!!.\n\nThis DP Id and ClientId already assigned to  '+ obj2 +'.\n\nClick OK to Accept,Otherwise Click Cancel');
                                       if(WAR2)
                                      {
                                                                          
                                                                          
                                                                  WAR3 =confirm('Warning!!.\n\nThis DP Id and ClientId already assigned to  '+ obj2 +'.\n\nClick OK to Accept,Otherwise Click Cancel');
                                                                  if(WAR3)
                                                                   {
                                                                   alert('Your DPID and ClientID has been accepted.')
                                                                                    
                                                                   }
                                                                   else
                                                                   {
                                                                    obj='DeleteCurrentID';
                                                                    gridDp.PerformCallback(obj);
                                                                   }  
                                       }
                                       else
                                       {
                                        obj='DeleteCurrentID';
                                        gridDp.PerformCallback(obj);
                                       }
                                    
                                    
                   }
                   else
                   {
                    obj='DeleteCurrentID';
                    gridDp.PerformCallback(obj);
                   }
        }
     
     
     }
     
     
     
     //----------Update Status 
     
     
     function btnSave_Click()
    {  
           var obj='SaveOld~'+RowID;
           popPanel.PerformCallback(obj);
        
    }
    
    function OnAddEditClick(e,obj)
    {     
      var data=obj.split('~');
        if(data.length>1)
            RowID=data[1];
        popup.Show();
        popPanel.PerformCallback(obj);
    }
   function EndCallBack(obj)
    {
        if(obj=='Y')
        {
         popup.Hide();        
         alert("Successfully Update!..");  
          gridDp.PerformCallback('GridBind');       
        }

     
    }
     function btnCancel_Click()
    {
        popup.Hide();
    }
    function isNumberKey(evt)
     {
        
        var charCode = (evt.which) ? evt.which : event.keyCode
             if (charCode > 31 && (charCode < 48 || charCode > 57))
                {
                alert('Please Enter only Numeric Value!');
                return false;
                }
             else
                
             return true;   
        
     }
   function isLength()
    {
      
        var len=document.getElementById('ASPxPageControl1_DpDetailsGrid_txtClientID').value;
        
        
    
    }  
     
     
    </script>
    <!--___________________These files are for List Items__________________________-->
   
    <!--___________________________________________________________________________-->
  
</asp:Content>

   
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>
            <table width="100%">
                <tr>
                    <td class="EHEADER" style="text-align: center">
                        <asp:Label ID="lblName" runat="server" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
            </table>
            <table class="TableMain100">
                 <tr>
                    <td colspan="2">
                        <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" Width="100%" ActiveTabIndex="3"
                            ClientInstanceName="page">
                            <TabPages>
                                <dxe:TabPage  Name="General">
                                <TabTemplate ><span style="font-size:x-small">General</span>&nbsp;<span style="color:Red;">*</span> </TabTemplate>
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage  Name="CorresPondence">
                                <TabTemplate ><span style="font-size:x-small">CorresPondence</span>&nbsp;<span style="color:Red;">*</span> </TabTemplate>
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Bank">
                                <TabTemplate ><span style="font-size:x-small">Bank</span>&nbsp;<span style="color:Red;">*</span> </TabTemplate>
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="DP" >
                                <TabTemplate ><span style="font-size:x-small">DP</span>&nbsp;<span style="color:Red;">*</span> </TabTemplate>
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                            <dxe:ASPxGridView ID="DpDetailsGrid" runat="server" AutoGenerateColumns="False"
                                                DataSourceID="DpDetailsdata" KeyFieldName="Id" ClientInstanceName="gridDp" Width="100%"
                                                Font-Size="12px" OnHtmlEditFormCreated="DpDetailsGrid_HtmlEditFormCreated" OnRowInserting="DpDetailsGrid_RowInserting"
                                                OnRowUpdating="DpDetailsGrid_RowUpdating" OnRowValidating="DpDetailsGrid_RowValidating" OnCustomJSProperties="DpDetailsGrid_CustomJSProperties" OnCustomCallback="DpDetailsGrid_CustomCallback">
                                                <Templates>
                                                    <EditForm>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="text-align: right; width: 30%">
                                                                    <span class="Ecoheadtxt" style="color: Black">Category :</span>
                                                                </td>
                                                                <td style="text-align: left">
                                                                    <dxe:ASPxComboBox ID="comboCategory" EnableIncrementalFiltering="True" EnableSynchronization="False"
                                                                        Value='<%#Bind("Category") %>' runat="server" ValueType="System.String" Width="285px">
                                                                        <Items>
                                                                            <dxe:ListEditItem Text="Default" Value="Default" />
                                                                            <dxe:ListEditItem Text="Secondary" Value="Secondary" />
                                                                            <dxe:ListEditItem Text="CommodityDP" Value="CommodityDP" />
                                                                             <dxe:ListEditItem Text="CommodityDP Sec" Value="CommodityDP Sec" />
                                                                        </Items>
                                                                        <ButtonStyle Width="13px">
                                                                        </ButtonStyle>
                                                                    </dxe:ASPxComboBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right; width: 30%">
                                                                    <span class="Ecoheadtxt" style="color: Black">DPName :</span>
                                                                </td>
                                                                <td style="text-align: left">
                                                                    <asp:TextBox ID="txtDPName" CssClass="EcoheadCon" Text='<%#Bind("DP") %>' runat="server"
                                                                        Width="279px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right; width: 30%">
                                                                    <span class="Ecoheadtxt" style="color: Black">ClientID :</span>
                                                                </td>
                                                                <td style="text-align: left">
                                                               <%-- <dxe:ASPxTextBox id="txtClientID" CssClass="EcoheadCon" Text='<%#Bind("ClientId") %>' ClientInstanceName="ctxtClientID" runat="server" Width="279px" MaxLength="8" 
                                                                onkeypress="return isNumberKey(event)" onblur="return isLength()" OnValidation="txtClientID_Validation"></dx:ASPxTextBox>--%>
                                                                    <asp:TextBox ID="txtClientID" CssClass="EcoheadCon" Text='<%#Bind("ClientId") %>'
                                                                        runat="server" Width="279px" MaxLength="8" onkeypress="return isNumberKey(event)" ></asp:TextBox>
                                                                        
                                                                      </td>
                                                            </tr>
                                                        

                                                          <tr>
                                                                <td style="text-align: right; width: 30%">
                                                                    <span class="Ecoheadtxt" style="color: Black">POA :</span>
                                                                </td>
                                                                <td style="text-align: left">
                                                                    <dxe:ASPxComboBox ID="comboPOA" EnableIncrementalFiltering="True" EnableSynchronization="False"
                                                                        Value='<%#Bind("POA") %>' AutoPostBack="true" runat="server" ValueType="System.String" Width="285px" 
                                                                        onselectedindexchanged="comboPOA_SelectedIndexChanged">
                                                                        <Items>
                                                                            <dxe:ListEditItem Text="Yes" Value="1" />
                                                                            <dxe:ListEditItem Text="No" Value="0" />
                                                                        </Items>
                                                                        <ClientSideEvents ValueChanged="function(s,e){
                                                                                                    var indexr = s.GetSelectedIndex();
                                                                                                    setvaluetovariable(indexr)
                                                                                                    }" />
                                                                        <ButtonStyle Width="13px">
                                                                        </ButtonStyle>
                                                                    </dxe:ASPxComboBox>
                                                                </td>
                                                            </tr>
                                                            
                                                            
                                                             <%--------------------------------%>
                                                                        
                                                                         <tr >
                                                                         
                                                                          <td style="text-align: right; width: 30%">
                                                                   <%-- <span class="Ecoheadtxt" style="color: Black">POA :</span>--%>
                                                                     <asp:Label ID="lblPOADate" Visible="false" runat="server"  Text="POA Date:"></asp:Label>
                                                                </td>
                                                                         
                                                                           
                                                                            <td style="text-align: left">
                                                                                <%--<asp:TextBox ID="TextBox1" runat="server" Text='<%#Bind("POAName") %>'   
                                                                                    width="200px" ></asp:TextBox>--%>
                                                                                    
                                                                                    <dxe:ASPxDateEdit ID="txtPOADate" Visible="false"  runat="server" Width="200px" 
                                                                                      EditFormat="Custom"   EditFormatString="dd-MM-yyyy"
                                                                        UseMaskBehavior="True" TabIndex="10"  Value='<%# Bind("POADate") %>' >
                                                                        <ButtonStyle Width="13px">
                                                                        </ButtonStyle>
                                                                    </dxe:ASPxDateEdit>
                                                                            </td>
                                                                        </tr>
                                                                        
                                                                        
                                                                        
                                                                       <%-- ------------------------------%>


                                                            <tr id="TrPoaName">
                                                                <td style="text-align: right; width: 30%">
                                                                   <%-- <span class="Ecoheadtxt" style="color: Black">POAName :</span>--%>
                                                                    <asp:Label ID="lblPOAName" Visible="false"  runat="server" Text="POAName:"></asp:Label>
                                                                </td>
                                                                <td style="text-align: left">
                                                                    <asp:TextBox ID="txtPoaName" CssClass="EcoheadCon" Text='<%#Bind("POAName") %>' runat="server"
                                                                        Width="279px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="text-align: center">
                                                                    <%--<controls>
                                                       <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors">
                                                       </dxe:ASPxGridViewTemplateReplacement>                                                           
                                                     </controls>--%>
                                                                    <div style="text-align: right; padding: 2px 2px 2px 2px">
                                                                        <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                                            runat="server">
                                                                        </dxe:ASPxGridViewTemplateReplacement>
                                                                        
                                                                        <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                                            runat="server">
                                                                        </dxe:ASPxGridViewTemplateReplacement>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="display: none">
                                                                    <asp:TextBox ID="txtDPName_hidden" CssClass="EcoheadCon" Text='<%#Bind("DPName") %>'
                                                                        runat="server" Width="279px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </EditForm>
                                                </Templates>
                                                <SettingsText PopupEditFormCaption="Add/Modify DP Details" ConfirmDelete="Confirm delete?" />
                                                <Styles>
                                                    <LoadingPanel ImageSpacing="10px">
                                                    </LoadingPanel>
                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                    </Header>
                                                </Styles>
                                                <SettingsBehavior ConfirmDelete="True" />
                                                <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                                                    <FirstPageButton Visible="True">
                                                    </FirstPageButton>
                                                    <LastPageButton Visible="True">
                                                    </LastPageButton>
                                                </SettingsPager>
                                                <Columns>
                                                    <dxe:GridViewDataTextColumn FieldName="Id" VisibleIndex="0" Visible="False">
                                                        <EditFormSettings Visible="False" />
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="CntId" VisibleIndex="0" Visible="False">
                                                        <EditFormSettings Visible="False" />
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataComboBoxColumn FieldName="Category" VisibleIndex="0">
                                                        <PropertiesComboBox ValueType="System.String">
                                                            <Items>
                                                                <dxe:ListEditItem Text="Default" Value="Default">
                                                                </dxe:ListEditItem>
                                                                <dxe:ListEditItem Text="Secondary" Value="Secondary">
                                                                </dxe:ListEditItem>
                                                            </Items>
                                                            <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                                <RequiredField ErrorText="Select Category" IsRequired="True" />
                                                            </ValidationSettings>
                                                        </PropertiesComboBox>
                                                        <EditFormSettings Visible="True" />
                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                        </EditFormCaptionStyle>
                                                    </dxe:GridViewDataComboBoxColumn>
                                                    <dxe:GridViewDataComboBoxColumn FieldName="DP" VisibleIndex="1">
                                                        <PropertiesComboBox DataSourceID="SelectDp" TextField="DP" ValueField="DP_DepositoryID"
                                                            ValueType="System.String" EnableIncrementalFiltering="True" EnableSynchronization="False">
                                                            <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                                <RequiredField ErrorText="Select DPName" IsRequired="True" />
                                                            </ValidationSettings>
                                                        </PropertiesComboBox>
                                                        <EditFormSettings Visible="True" />
                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                        </EditFormCaptionStyle>
                                                    </dxe:GridViewDataComboBoxColumn>
                                                    <dxe:GridViewDataComboBoxColumn FieldName="POA" VisibleIndex="3">
                                                        <PropertiesComboBox ValueType="System.String">
                                                            <Items>
                                                                <dxe:ListEditItem Text="Yes" Value="1">
                                                                </dxe:ListEditItem>
                                                                <dxe:ListEditItem Text="No" Value="0">
                                                                </dxe:ListEditItem>
                                                            </Items>
                                                            <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                                <RequiredField ErrorText="Select POA" IsRequired="True" />
                                                            </ValidationSettings>
                                                        </PropertiesComboBox>
                                                        <EditFormSettings Visible="True" />
                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                        </EditFormCaptionStyle>
                                                    </dxe:GridViewDataComboBoxColumn>


                                                     <%--  -----------------------------%>
                                                  
                                                  
                                                   <dxe:GridViewDataTextColumn FieldName="POADate" VisibleIndex="4">
                                                        <EditFormSettings Visible="True" />
                                                       
                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                        </EditFormCaptionStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    
                                                    <%----------------------------------------------------------------------------------%>
                                                    
                                                    <dxe:GridViewDataTextColumn FieldName="ClientId" VisibleIndex="2">
                                                        <EditFormSettings Visible="True" />
                                                        <PropertiesTextEdit>
                                                            <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" ErrorText="ClientID Must be 8 Character Long." SetFocusOnError="True">
                                                                <RequiredField ErrorText="ClientId Required" IsRequired="True" />                                                                
                                                            </ValidationSettings> 
                                                            <ClientSideEvents Validation="OnLengthValidation" />                                                          
                                                        </PropertiesTextEdit>
                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                        </EditFormCaptionStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    
                                                    <dxe:GridViewDataTextColumn FieldName="POAName" VisibleIndex="4">
                                                        <EditFormSettings Visible="True" />
                                                        <PropertiesTextEdit>
                                                            <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                                <RequiredField ErrorText="POAName Required" IsRequired="True" />
                                                            </ValidationSettings>
                                                        </PropertiesTextEdit>
                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                        </EditFormCaptionStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="CreateUser" VisibleIndex="5" Visible="False">
                                                        <EditFormSettings Visible="False" />
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="DPName" VisibleIndex="6" Visible="False">
                                                        <EditFormSettings Visible="False" />
                                                    </dxe:GridViewDataTextColumn>
                                                    
                                                     <dxe:GridViewDataTextColumn FieldName="status" VisibleIndex="7">
                                                    <DataItemTemplate>
                                                        <a href="javascript:void(0);" onclick="OnAddEditClick(this,'Edit~'+'<%# Container.KeyValue %>')">                                                            
                                                            <dxe:ASPxLabel ID="ASPxTextBox2"   runat="server"   Text='<%# Eval("status")%>' Width="100%" ToolTip="Click to Change Status">
                                                            </dxe:ASPxLabel>                                                            
                                                        </a>
                                                    </DataItemTemplate>
                                                    <EditFormSettings Visible="False" />
                                                    <CellStyle Wrap="False">
                                                    </CellStyle>
                                                    <HeaderTemplate>
                                                      Status                                                         
                                                    </HeaderTemplate>
                                                    <HeaderStyle Wrap="False" />
                                                </dxe:GridViewDataTextColumn>
                                                    
                                                    <dxe:GridViewCommandColumn VisibleIndex="8" ShowEditButton="true">
                                                      <%--  <DeleteButton Visible="True">
                                                        </DeleteButton>--%>
                                                       
                                                        <HeaderTemplate>
                                                            <a href="javascript:void(0);" onclick="gridDp.AddNewRow();"><span >Add New</span> </a>
                                                        </HeaderTemplate>
                                                    </dxe:GridViewCommandColumn>
                                                    
                                                    
                                                    
                                                    
                                                    <dxe:GridViewDataTextColumn VisibleIndex="9" Width="60px" Caption="Details">
<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
<DataItemTemplate>
    <a href="javascript:void(0);"  onclick="DeleteRow('<%# Container.KeyValue %>')">
                                                            Delete</a>                                                   
</DataItemTemplate>

<CellStyle Wrap="False"></CellStyle>
<HeaderTemplate>
         <span style="color: #000099;text-decoration: underline">Delete</span>
                                                    
</HeaderTemplate>

<EditFormSettings Visible="False"></EditFormSettings>
</dxe:GridViewDataTextColumn>
                                                    
                                                    
                                                    
                                                </Columns>
                                                <SettingsCommandButton>
                                                                    
                                                                        <EditButton  Text="Edit">
                                                                        </EditButton>
                                                                </SettingsCommandButton>
                                                <ClientSideEvents EndCallback="function(s, e) {
Emailcheck(s.cpHeight,s.cpWidth);
}"></ClientSideEvents>
                                            </dxe:ASPxGridView>
                                            
                                            
                                            
                                            
                                            
                                            
                                            <dxe:ASPxPopupControl ID="ASPxPopupControl1" ClientInstanceName="popup" runat="server"
                                    AllowDragging="True" PopupHorizontalAlign="WindowCenter" HeaderText="Set DP Status"
                                    EnableHotTrack="False" BackColor="#DDECFE" Width="400px" CloseAction="CloseButton">
                                    <ContentCollection>
                                        <dxe:PopupControlContentControl runat="server">
                                            <dxe:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="400px" ClientInstanceName="popPanel"
                                                OnCallback="ASPxCallbackPanel1_Callback" OnCustomJSProperties="ASPxCallbackPanel1_CustomJSProperties">
                                                <PanelCollection>
                                                    <dxe:PanelContent runat="server">
                                                    
                                                    <table>
                                                    <tr>
                                                    <td>
                                                    Status:
                                                    </td>
                                                    <td>  <asp:DropDownList ID="cmbStatus" runat="server" Width="100px">
                                                                        <asp:ListItem Text="Active" Value="Y"></asp:ListItem>
                                                                        <asp:ListItem Text="Deactive" Value="N"></asp:ListItem>                                                                        
                                                                       </asp:DropDownList>
                                                    
                                                    </td>
                                                    </tr>
                                                    <tr>
                                                    <td>
                                                     Date:
                                                    </td>
                                                    <td>    <dxe:ASPxDateEdit ID="StDate" runat="server" ClientInstanceName="StDate" EditFormat="Custom"
                                                                                                    UseMaskBehavior="True" Width="99px" Font-Size="12px" TabIndex="21">
                                                                                                    <ButtonStyle Width="13px">
                                                                                                    </ButtonStyle>
                                                                                                </dxe:ASPxDateEdit>
                                                    </td>
                                                    </tr>
                                                    <tr>
                                                    <td>
                                                    Reason:
                                                    </td>
                                                    <td>
                                                    <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Width="250px" ></asp:TextBox>
                                                    </td>
                                                    </tr>
                                                    
                                                     <tr>
                                                                <td>
                                                                </td>
                                                                <td colspan="2" class="gridcellleft">
                                                                    <input id="Button1" type="button" value="Save" class="btnUpdate" onclick="btnSave_Click()"
                                                                        style="width: 60px" tabindex="41" />
                                                                    <input id="Button2" type="button" value="Cancel" class="btnUpdate" onclick="btnCancel_Click()"
                                                                        style="width: 60px" tabindex="42" />
                                                                </td>
                                                            </tr>
                                                    </table>                                                  
                                                    
                                                     </dxe:PanelContent>
                                                </PanelCollection>
                                                <ClientSideEvents EndCallback="function(s, e) {
	                                                    EndCallBack(s.cpLast);
                                                    }" />
                                            </dxe:ASPxCallbackPanel>
                                        </dxe:PopupControlContentControl>
                                    </ContentCollection>
                                    <HeaderStyle HorizontalAlign="Left">
                                        <Paddings PaddingRight="6px" />
                                    </HeaderStyle>
                                    <SizeGripImage Height="16px" Width="16px" />
                                    <CloseButtonImage Height="12px" Width="13px" />
                                    <ClientSideEvents CloseButtonClick="function(s, e) {
	 popup.Hide();
}" />
                                </dxe:ASPxPopupControl>
                                
                                
                                            
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Documents">
                                <TabTemplate ><span style="font-size:x-small">Documents</span>&nbsp;<span style="color:Red;">*</span> </TabTemplate>
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                
                                <dxe:TabPage Name="Registration">
                                <TabTemplate ><span style="font-size:x-small">Registration</span>&nbsp;<span style="color:Red;">*</span> </TabTemplate>
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Other">
                                <TabTemplate ><span style="font-size:x-small">Other</span>&nbsp;<span style="color:Red;">*</span> </TabTemplate>
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Group" Text="Group">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Deposit" Text="Deposit">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Remarks" Text="Remarks">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Education" Text="Education">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Trad. Prof." Text="Trad.Prof">
                                <%--<TabTemplate ><span style="font-size:x-small">Trad.Prof</span>&nbsp;<span style="color:Red;">*</span> </TabTemplate>--%>
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="FamilyMembers" Text="Family">
                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                           
                             <dxe:TabPage Name="Subscription" Text="Subscription">
                        <ContentCollection>
                            <dxe:ContentControl runat="server">
                            </dxe:ContentControl>
                        </ContentCollection>
                    </dxe:TabPage>
                   
                     </TabPages>
                            <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
	                                            var Tab0 = page.GetTab(0);
	                                            var Tab1 = page.GetTab(1);
	                                            var Tab2 = page.GetTab(2);
	                                            var Tab3 = page.GetTab(3);
	                                            var Tab4 = page.GetTab(4);
	                                            var Tab5 = page.GetTab(5);
	                                            var Tab6 = page.GetTab(6);
	                                            var Tab7 = page.GetTab(7);
	                                            var Tab8 = page.GetTab(8);
	                                             var Tab9 = page.GetTab(9);
	                                             var Tab10 = page.GetTab(10);
	                                             var Tab11 = page.GetTab(11);
	                                            var Tab12 = page.GetTab(12);
	                                            var Tab13=page.GetTab(13);
	                                            
	                                            if(activeTab == Tab0)
	                                            {
	                                                disp_prompt('tab0');
	                                            }
	                                            if(activeTab == Tab1)
	                                            {
	                                                disp_prompt('tab1');
	                                            }
	                                            else if(activeTab == Tab2)
	                                            {
	                                                disp_prompt('tab2');
	                                            }
	                                            else if(activeTab == Tab3)
	                                            {
	                                                disp_prompt('tab3');
	                                            }
	                                            else if(activeTab == Tab4)
	                                            {
	                                                disp_prompt('tab4');
	                                            }
	                                            else if(activeTab == Tab5)
	                                            {
	                                                disp_prompt('tab5');
	                                            }
	                                            else if(activeTab == Tab6)
	                                            {
	                                                disp_prompt('tab6');
	                                            }
	                                            else if(activeTab == Tab7)
	                                            {
	                                                disp_prompt('tab7');
	                                            }
	                                            else if(activeTab == Tab8)
	                                            {
	                                                disp_prompt('tab8');
	                                            }
	                                            else if(activeTab == Tab9)
	                                            {
	                                                disp_prompt('tab9');
	                                            }
	                                            else if(activeTab == Tab10)
	                                            {
	                                                disp_prompt('tab10');
	                                            }
	                                            else if(activeTab == Tab11)
	                                            {
	                                                disp_prompt('tab11');
	                                            }
	                                            else if(activeTab == Tab12)
	                                            {
	                                                disp_prompt('tab12');
	                                            }
                                                else if(activeTab == Tab13)
	                                            {
	                                               disp_prompt('tab13');
	                                            }
	                                           
	                                            }"></ClientSideEvents>
                            <ContentStyle>
                                <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                            </ContentStyle>
                            <LoadingPanelStyle ImageSpacing="6px">
                            </LoadingPanelStyle>
                            <TabStyle Font-Size="12px">
                            </TabStyle>
                        </dxe:ASPxPageControl>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="display: none; visibility: hidden">
                        <asp:TextBox ID="txtID" runat="server" Visible="false"></asp:TextBox></td>
                </tr>
            </table>
        </div>
        <asp:SqlDataSource ID="DpDetailsdata" runat="server" 
            SelectCommand="select dpd_id AS Id,dpd_cntId as CntId,case ltrim(rtrim(dpd_accountType)) when ltrim(rtrim('Primary')) then ltrim(rtrim('Default')) else ltrim(rtrim(dpd_accountType)) end AS Category,dpd_dpCode AS DPName,dpd_ClientId AS ClientId, CASE dpd_POA WHEN 1 THEN 'Yes' ELSE 'No' END AS POA,dpd_POAName AS POAName,CreateUser,isnull((select rtrim(replace(dp_dpName,char(160),char(32))) from tbl_master_depositoryParticipants where substring(dp_dpId,1,8)=substring(ltrim(dpd_dpCode),1,8)),'')+'['+ltrim(rtrim(dpd_dpCode))+']' as DP,case when  dpd_Status='N' then 'Deactive' else 'Active' end as status,isnull(dpd_POADate,'') as [POADate]  from tbl_master_contactDPDetails where dpd_cntId=@CntId"
            InsertCommand="insert into tbl_master_contactDPDetails(dpd_cntId,dpd_accountType,dpd_dpCode,dpd_clientId,dpd_POA,dpd_POAName,CreateDate,CreateUser,dpd_POADate) values(@CntId,@Category,@DPName,@ClientId,@POA,@POAName,getdate(),@CreateUser,@POADate)"
            UpdateCommand="update tbl_master_contactDPDetails set dpd_accountType=@Category,dpd_dpCode=@DPName,dpd_clientId=@ClientId,dpd_POA=@POA,dpd_POAName=@POAName,LastModifyDate=getdate(),LastModifyUser=@CreateUser,dpd_POADate= @POADate  where dpd_id=@Id">
            <SelectParameters>
                <asp:SessionParameter Name="CntId" SessionField="KeyVal_InternalID_New" Type="String" />
            </SelectParameters>
            <InsertParameters>
                <asp:SessionParameter Name="CntId" SessionField="KeyVal_InternalID_New" Type="String" />
                <asp:Parameter Name="Category" />
                <asp:Parameter Name="DPName" />
                <asp:Parameter Name="ClientId" />
                <asp:Parameter Name="POA" />
                <asp:Parameter Name="POAName" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
                 <asp:Parameter Name="POADate"  />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="Category" />
                <asp:Parameter Name="DPName" />
                <asp:Parameter Name="ClientId" />
                <asp:Parameter Name="POA" />
                <asp:Parameter Name="POAName" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
                <asp:Parameter Name="Id" />
                 <asp:Parameter Name="POADate"  />
            </UpdateParameters>
          <%--  <DeleteParameters>
                <asp:Parameter Name="Id" />
            </DeleteParameters>--%>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SelectDp" runat="server" 
            SelectCommand="select DP_DepositoryID,DP_Name+' ['+DP_DepositoryID+']' as DP from Master_DP order by DP_Name">
        </asp:SqlDataSource>
 </asp:Content>
