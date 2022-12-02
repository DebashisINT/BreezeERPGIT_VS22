<%@ Page Title="Asset Details" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="AssetDetail.aspx.cs" Inherits="ERP.OMS.Management.Master.AssetDetail" %>
<%@ Register Src="../Headermain.ascx" TagName="Headermain" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
     <script type="text/javascript">
         function CallList(obj, obj1, obj2) {
             ajax_showOptions(obj, obj1, obj2);
         }
         FieldName = 'cmbSegment'; 

         function fn_PopOpen() {
             //alert('lets append a div');
             //document.getElementById('mylay').className = 'mylay';
             //$('#mylay').addClass('fff');
             cPopup_Empcitys.SetHeaderText('Add DP Details');
             document.getElementById('hiddenedit').value = ""; 
             var countv = $('#lstVendor option').length;
             if (countv > 0)
             {
                 var techGroups = document.getElementById("lstVendor");
                 techGroups.options[0].selected = true;
                 $('#lstVendor').trigger("chosen:updated");
             } 
             var counts = $('#lstServicepro option').length;
             if (counts > 0) {
                 var techGroups = document.getElementById("lstServicepro");
                 techGroups.options[0].selected = true;
                 $('#lstServicepro').trigger("chosen:updated");
             } 
             var counti = $('#lstinsurer option').length;
             if (counti > 0) {
                 var techGroups = document.getElementById("lstinsurer");
                 techGroups.options[0].selected = true;
                 $('#lstinsurer').trigger("chosen:updated");
             } 
             var countl = $('#lstlocation option').length;
             if (countl > 0) {
                 var techGroups = document.getElementById("lstlocation");
                 techGroups.options[0].selected = true;
                 $('#lstlocation').trigger("chosen:updated");
             } 
             var countu = $('#lstusedby option').length;
             if (countu > 0) {
                 var techGroups = document.getElementById("lstusedby");
                 techGroups.options[0].selected = true;
                 $('#lstusedby').trigger("chosen:updated");
             } 
             combAssetCategory.SetSelectedIndex(0); 
             combFinYear.SetSelectedIndex(0);
             if (cdtpPurchaseDate.GetDate() != null)
             { 
                 cdtpPurchaseDate.SetDate(null);
             } 
             if (cdtpPolicyExpiry.GetDate() != null) {
                 cdtpPolicyExpiry.SetDate(null);
             } 
             if (cdtpPremiumDue.GetDate() != null) {
                 cdtpPremiumDue.SetDate(null);
             } 
             if (cdtpExpiryDate.GetDate() != null) {
                 cdtpExpiryDate.SetDate(null);
             }
             ctxtCostPrice.SetText('');
             ctxtDepreciation.SetText('');
             ctxtBroughtForward.SetText('');
             ctxtDepreciationIT.SetText('');
             ctxtAddition.SetText('');
             ctxtDisposals.SetText('');
             cPopup_Empcitys.Show();  
         }
         $(document).ready(function () {
             $('#lstVendor').chosen();
             $('#lstServicepro').chosen();
             $('#lstinsurer').chosen(); 
             $('#lstlocation').chosen(); 
             $('#lstusedby').chosen();
              
         });
         function fn_btnCancel() {
             cPopup_Empcitys.Hide(); 
         }

         function btnSave_citys() {
             if (document.getElementById('hiddenedit').value == '') {
                 grid.PerformCallback('savecity~');
             }
             else {
                 grid.PerformCallback('updatecity~' + GetObjectID('hiddenedit').value);
             }

         }
         function fn_Editcity(keyValue) {
             cPopup_Empcitys.SetHeaderText('Modify DP Detail');
             grid.PerformCallback('Edit~' + keyValue);
         } 
         function fn_Deletecity(keyValue) {
             if (confirm("Confirm delete?")) {
                 grid.PerformCallback('Delete~' + keyValue);
             }
         }
          
         function grid_EndCallBack() {
             if (grid.cpinsert != null) {
                 if (grid.cpinsert == 'Success') {
                     alert('Saved Successfully'); 
                     cPopup_Empcitys.Hide();
                 }
                 else if (grid.cpinsert == 'fail') {
                     alert("Error On Insertion \n 'Please Try Again!!'")
                 }
                 else {
                     alert(grid.cpinsert); 
                 }
             }
             if (grid.cpEdit != null) { 
                 var Vendor = grid.cpEdit.split('~')[1];
                 if (Vendor != '')
                 {
                     var techGroups = document.getElementById("lstVendor");
                     for (var i = 0; i < techGroups.options.length; i++) {
                         if (techGroups.options[i].value == Vendor) { 
                             techGroups.options[i].selected = true;
                         }
                     }
                     $('#lstVendor').trigger("chosen:updated");
                 }
                 combAssetCategory.SetValue(grid.cpEdit.split('~')[2]);
                 var Servicepro = grid.cpEdit.split('~')[3];
                 if (Servicepro != '')
                 {
                     var techGroups = document.getElementById("lstServicepro");
                     for (var i = 0; i < techGroups.options.length; i++) {
                         if (techGroups.options[i].value == Servicepro) { 
                             techGroups.options[i].selected = true;
                         }
                     }
                     $('#lstServicepro').trigger("chosen:updated");
                 }
                 combFinYear.SetText(grid.cpEdit.split('~')[4]);  
                 var d = grid.cpEdit.split('~')[5];
                 if (d != '') {
                     var myDate = new Date(); 
                     var CDate = new Date(d);
                     myDate.setDate(CDate.getDate());                     
                     cdtpExpiryDate.SetDate(myDate);
                 } 
                 var PurchaseDate = grid.cpEdit.split('~')[6];
                     if (PurchaseDate != '') {
                         var myDate = new Date(); 
                         var CDate = new Date(PurchaseDate);
                         myDate.setDate(CDate.getDate());                     
                         cdtpPurchaseDate.SetDate(myDate);
                     }
                     var insurer = grid.cpEdit.split('~')[7];
                     if (insurer != '')
                     {
                         var techGroups = document.getElementById("lstinsurer");
                         for (var i = 0; i < techGroups.options.length; i++) {
                             if (techGroups.options[i].value == insurer) { 
                                 techGroups.options[i].selected = true;
                             }
                         }
                         $('#lstinsurer').trigger("chosen:updated");
                     }
                     ctxtCostPrice.SetText(grid.cpEdit.split('~')[8]);
                     var PolicyExpiry = grid.cpEdit.split('~')[9];
                     if (PolicyExpiry != '') {
                         var myDate = new Date();
                         var CDate = new Date(PolicyExpiry);
                         myDate.setDate(CDate.getDate());
                         cdtpPolicyExpiry.SetDate(myDate);
                     }
                     ctxtAddition.SetText(grid.cpEdit.split('~')[10]);
                     var PremiumDue = grid.cpEdit.split('~')[11];
                     if (PremiumDue != '') {
                         var myDate = new Date();
                         var CDate = new Date(PremiumDue);
                         myDate.setDate(CDate.getDate());
                         cdtpPremiumDue.SetDate(myDate);
                     }
                     ctxtDisposals.SetText(grid.cpEdit.split('~')[12]);
                     var location = grid.cpEdit.split('~')[13]
                     if (location != '')
                     { 
                      var techGroups = document.getElementById("lstlocation"); 
                         for (var i = 0; i < techGroups.options.length; i++) { 
                             if (techGroups.options[i].value == location) {
                                 techGroups.options[i].selected = true; 
                             }
                         }
                         $('#lstlocation').trigger("chosen:updated");
                      } 
                     ctxtDepreciation.SetText(grid.cpEdit.split('~')[14]);
                     ctxtBroughtForward.SetText(grid.cpEdit.split('~')[15]);
                     ctxtDepreciationIT.SetText(grid.cpEdit.split('~')[16]);
                     var usedby = grid.cpEdit.split('~')[18]
                     if (usedby != '')
                     {
                         var techGroups = document.getElementById("lstusedby");
                        
                         for (var i = 0; i < techGroups.options.length; i++) {
                             if (techGroups.options[i].value == usedby) {
                                 techGroups.options[i].selected = true;
                             }
                         }
                         $('#lstusedby').trigger("chosen:updated");
                     }
                     GetObjectID('hiddenedit').value = grid.cpEdit.split('~')[17];
                   cPopup_Empcitys.Show();
             }
             if (grid.cpUpdate != null) {
                 if (grid.cpUpdate == 'Success') {
                     alert('Saved Successfully');
                     cPopup_Empcitys.Hide();
                 }
                 else {
                     alert("Error on Updation\n'Please Try again!!'") 
                 }
             }
             if (grid.cpUpdateValid != null) {
                 if (grid.cpUpdateValid == "StateInvalid") {
                     alert("Please Select proper country state and city"); 
                }
            }
             if (grid.cpDelete != null) {
                 if (grid.cpDelete == 'Success')
                     alert('Deleted Successfully');
                 else
                     alert("Error on deletion\n'Please Try again!!'")
             }
             if (grid.cpExists != null) {
                 if (grid.cpExists == "Exists") {
                     alert('Record already Exists'); 
                 }
                 else {
                     alert("Error on operation \n 'Please Try again!!'") 
                 }
             }
         }
     </script>
     <style type="text/css">
          .chosen-container.chosen-container-single {
           width:100% !important;
       }
	.col-md-6 {
        width:49%;
        float:left;
	}
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
	
	/*form{
		display:inline;
	}*/
	#txtCostPrice_EC, #txtAddition_EC, #txtDisposals_EC, #txtDepreciation_EC, #txtBroughtForward_EC, #txtDepreciationIT_EC {
        position:absolute;
	}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
             
            <h3>
                Asset Details
            </h3> 
            <% if (Session["requesttype"] == "Account Heads")
               { %>
            <div class="crossBtn"><a href="MainAccountHead.aspx"><i class="fa fa-times"></i></a></div>
            <% } %>

             <% else if (Session["requesttype"] == "Sub Ledger")
               { %>
            <div class="crossBtn"><a href="<%= Convert.ToString(Session["redirct"]) %>"><i class="fa fa-times"></i></a></div>
            <% } %>

            <%--..........................Code Commented and Modified by sam on 03102016............................--%>
        </div>
    </div>
    <div>
            <table class="TableMain100">
                 <tr>
                <td>
                    <table width="">
                        <tr>
                            <td style="text-align: left; vertical-align: top">
                                <table>
                                    <tr>
                                        <td id="ShowFilter"> 
                                             <a class="btn btn-primary" href="javascript:void(0);" onclick="fn_PopOpen()"><span>Add New</span> </a> 
                                            <% if (rights.CanExport)
                                               { %>
                                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true"  >
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                                <asp:ListItem Value="2">XLS</asp:ListItem>
                                <asp:ListItem Value="3">RTF</asp:ListItem>
                                <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>
                                            <% } %>
                                        </td> 
                                    </tr>
                                </table>
                            </td>
                            <td>
                                </td>
                             
                        </tr>
                    </table>
                </td>
            </tr>
             <%-- DataSourceID="AssetDetaildata"--%>
                <tr>
                    <td style="height: 181px">
                        <dxe:ASPxGridView ID="AssetDetailGrid" runat="server" AutoGenerateColumns="False"
                             KeyFieldName="AssetDetail_ID" ClientInstanceName="grid"                             
                            Width="100%"   OnHtmlEditFormCreated="AssetDetailGrid_HtmlEditFormCreated"
                             OnHtmlRowCreated="AssetDetailGrid_HtmlRowCreated"
                            OnRowInserting="AssetDetailGrid_RowInserting" OnRowUpdating="AssetDetailGrid_RowUpdating"
                            OnRowValidating="AssetDetailGrid_RowValidating" OnStartRowEditing="AssetDetailGrid_StartRowEditing" 
                            OnCustomCallback="AssetDetailGrid_CustomCallback"> 
                            <SettingsText PopupEditFormCaption="Add/Modify DP Details" ConfirmDelete="Confirm delete?" />
                            <SettingsSearchPanel Visible="True" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <Styles  >
                                <LoadingPanel ImageSpacing="10px">
                                </LoadingPanel>
                                <Header ImageSpacing="5px" SortingImageSpacing="5px" >
                                </Header>
                                <%--<FocusedGroupRow CssClass="gridselectrow">
                                </FocusedGroupRow>
                                <FocusedRow CssClass="gridselectrow">
                                </FocusedRow>--%>
                            </Styles> 
                            
                            <SettingsPager NumericButtonCount="20" PageSize="20">
                            </SettingsPager>
                             <SettingsCommandButton> 
                                                <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                                                </EditButton>
                                                <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                </DeleteButton> 
                                                <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary"></UpdateButton>
                                                <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger"></CancelButton>
                                            </SettingsCommandButton>
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="AssetDetail ID" FieldName="AssetDetail_ID"
                                    Visible="False" VisibleIndex="0">
                                    
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataComboBoxColumn Caption="Company Name" FieldName="AssetDetail_CompanyID"
                                    VisibleIndex="0" Visible="False" ShowInCustomizationForm="True">
                                   <%-- <PropertiesComboBox DataSourceID="Company" EnableIncrementalFiltering="True" TextField="CompanyName"
                                        ValueField="ID" ValueType="System.String" >
                                    </PropertiesComboBox>--%>
                                    <Settings FilterMode="DisplayText" />
                                    <EditFormSettings Visible="False" VisibleIndex="5" />
                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                </dxe:GridViewDataComboBoxColumn>
                                <dxe:GridViewDataComboBoxColumn Caption="Asset Category" FieldName="AssetDetail_Category"
                                    VisibleIndex="0" Visible="False">
                                    <%--<PropertiesComboBox DataSourceID="AssetDetaildata" TextField="AssetDetail_Category"
                                        ValueField="AssetDetail_Category" ValueType="System.String">
                                        <Items>
                                            <dxe:ListEditItem Text="Movable" Value="M">
                                            </dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Immovable" Value="I">
                                            </dxe:ListEditItem>
                                            <dxe:ListEditItem Text="Work-In-Progress" Value="W">
                                            </dxe:ListEditItem>
                                        </Items>
                                    </PropertiesComboBox>--%>
                                </dxe:GridViewDataComboBoxColumn>
                                <dxe:GridViewDataTextColumn Caption="MainAccountCode" FieldName="AssetDetail_MainAccountCode"
                                    UnboundType="String" Visible="False">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="SubAccountCode" FieldName="AssetDetail_SubAccountCode"
                                    UnboundType="String" Visible="False">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataComboBoxColumn Caption="Financial Year" FieldName="AssetDetail_FinYear" 
                                    VisibleIndex="0">
                                   <%-- <PropertiesComboBox DataSourceID="FinYear" TextField="FinancialYear" ValueField="ID" 
                                        ValueType="System.String">
                                    </PropertiesComboBox>--%>
                                    <Settings FilterMode="DisplayText" />
                                    <%--<EditFormSettings Visible="False" VisibleIndex="6" />--%>
                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                </dxe:GridViewDataComboBoxColumn>
                                <dxe:GridViewDataTextColumn Caption="Brought Forward" FieldName="AssetDetail_BroughtForward"
                                    UnboundType="Decimal" VisibleIndex="1">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                    <%--<PropertiesTextEdit DisplayFormatString="{0:N2}" Width="90px">
                                        <MaskSettings Mask="0,00,000..9,99,999" />
                                    </PropertiesTextEdit>
                                    <EditFormSettings Visible="False" />--%>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataDateColumn Caption="Date Of Purchase" FieldName="AssetDetail_PurchaseDate"
                                    UnboundType="DateTime" VisibleIndex="1" Visible="False">
                                    <%--<PropertiesDateEdit DateOnError="Today" DisplayFormatString="dd MMMM yyyy" EditFormatString="dd-MM-yyyy"
                                        UseMaskBehavior="True">
                                    </PropertiesDateEdit>--%>
                                </dxe:GridViewDataDateColumn>
                                <dxe:GridViewDataTextColumn Caption="Cost Price" FieldName="AssetDetail_CostPrice"
                                    UnboundType="Decimal" VisibleIndex="2">
                                     <HeaderStyle HorizontalAlign="Center" />
                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                   <%-- <PropertiesTextEdit DisplayFormatString="{0:N2}" Width="90px">
                                        <MaskSettings Mask="0,00,000..9,99,999" />
                                    </PropertiesTextEdit>
                                    <EditFormSettings Visible="False" />--%>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Addition" FieldName="AssetDetail_Additions"
                                    UnboundType="Decimal" VisibleIndex="3">
                                     <HeaderStyle HorizontalAlign="Center" />
                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                    <%--<PropertiesTextEdit DisplayFormatString="{0:N2}" Width="90px">
                                        <MaskSettings Mask="0,00,000..9,99,999" />
                                    </PropertiesTextEdit>
                                    <EditFormSettings Visible="False" />--%>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Disposals" FieldName="AssetDetail_Disposals"
                                    UnboundType="Decimal" VisibleIndex="4">
                                     <HeaderStyle HorizontalAlign="Center" />
                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                    <%--<PropertiesTextEdit DisplayFormatString="{0:N2}" Width="90px">
                                        <MaskSettings Mask="0,00,000..9,99,999" />
                                    </PropertiesTextEdit>
                                    <EditFormSettings Visible="False" />--%>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Depreciation" FieldName="AssetDetail_Depreciation"
                                    UnboundType="Decimal" VisibleIndex="5">
                                     <HeaderStyle HorizontalAlign="Center" />
                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                   <%-- <PropertiesTextEdit DisplayFormatString="{0:N2}" Width="90px">
                                        <MaskSettings Mask="0,00,000..9,99,999" />
                                    </PropertiesTextEdit>
                                    <EditFormSettings ColumnSpan="2" Visible="False" />--%>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Net Value" FieldName="NetValue" VisibleIndex="6">
                                     <HeaderStyle HorizontalAlign="Center" />
                                <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                   <%-- <PropertiesTextEdit DisplayFormatString="{0:N2}" Width="90px">
                                        <MaskSettings Mask="0,00,000..9,99,999" />
                                    </PropertiesTextEdit>
                                    <EditFormSettings Visible="False" />--%>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Vendor" FieldName="AssetDetail_Vendor" UnboundType="Decimal"
                                    VisibleIndex="4" Visible="False">
                                   <%-- <EditFormSettings ColumnSpan="2" />--%>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Service Provider" FieldName="AssetDetail_ServiceProvider"
                                    VisibleIndex="4" Visible="False">
                                    <%--<EditFormSettings ColumnSpan="2" />--%>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataDateColumn Caption="AMC Expiry" FieldName="AssetDetail_AMCExpiryDate"
                                    UnboundType="DateTime" VisibleIndex="4" Visible="False">
                                   <%-- <PropertiesDateEdit DisplayFormatString="dd MMMM yyyy" UseMaskBehavior="True" DateOnError="Today"
                                        EditFormatString="dd-MM-yyyy">
                                    </PropertiesDateEdit>
                                    <EditFormSettings ColumnSpan="2" />--%>
                                </dxe:GridViewDataDateColumn>
                                <dxe:GridViewDataTextColumn Caption="Insurer" FieldName="AssetDetail_Insurer" VisibleIndex="4"
                                    Visible="False">
                                    <%--<EditFormSettings ColumnSpan="2" />--%>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataDateColumn Caption="Policy Expiry" FieldName="AssetDetail_PolicyExpiryDate"
                                    VisibleIndex="4" Visible="False" UnboundType="DateTime">
                                   <%-- <PropertiesDateEdit DisplayFormatString="dd MMMM yyyy" EditFormatString="dd-MM-yyyy"
                                        DateOnError="Today" UseMaskBehavior="True">
                                    </PropertiesDateEdit>
                                    <EditFormSettings ColumnSpan="2" />--%>
                                </dxe:GridViewDataDateColumn>
                                <dxe:GridViewDataDateColumn Caption="Premium Due" FieldName="AssetDetail_PremiumDueDate"
                                    VisibleIndex="4" Visible="False" UnboundType="DateTime">
                                    <%--<PropertiesDateEdit DisplayFormatString="dd MMMM yyyy" DateOnError="Today" EditFormatString="dd-MM-yyyy"
                                        UseMaskBehavior="True">
                                    </PropertiesDateEdit>
                                    <EditFormSettings ColumnSpan="2" />--%>
                                </dxe:GridViewDataDateColumn>
                                <%--<dxe:GridViewCommandColumn VisibleIndex="7" ShowEditButton="true" ShowDeleteButton="true">
                                  
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <HeaderTemplate>
                                        Actions
                                         
                                    </HeaderTemplate>
                                    
                                </dxe:GridViewCommandColumn>--%>
                                <dxe:GridViewDataTextColumn ReadOnly="True" Width="12%" CellStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" />
                        <HeaderTemplate>
                            Actions
                        </HeaderTemplate>
                        <DataItemTemplate> 
                            <a href="javascript:void(0);" onclick="fn_Editcity('<%# Container.KeyValue %>')" title="Edit" class="pad">
                                <img src="../../../assests/images/Edit.png" /></a>
                              
                             
                            <a href="javascript:void(0);" onclick="fn_Deletecity('<%# Container.KeyValue %>')" title="Delete"  class="pad">
                                <img src="../../../assests/images/Delete.png" /></a> 
                        </DataItemTemplate>
                    </dxe:GridViewDataTextColumn>
                            </Columns>
                            <%--<SettingsEditing PopupEditFormHeight="400px" PopupEditFormHorizontalAlign="Center" Mode="PopupEditForm" EditFormColumnCount="1"
                                PopupEditFormModal="True" PopupEditFormVerticalAlign="Center" PopupEditFormWidth="500px" />--%>
                             <ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack();}" />
                        </dxe:ASPxGridView>
                        <%--SelectCommand="select AssetDetail_ID,AssetDetailCompany_ID,case ltrim(rtrim(dpd_accountType)) when ltrim(rtrim('Primary')) then ltrim(rtrim('Default')) else ltrim(rtrim(dpd_accountType)) end AS Category,dpd_dpCode AS DPName,dpd_ClientId AS ClientId, CASE dpd_POA WHEN 1 THEN 'Yes' ELSE 'No' END AS POA,dpd_POAName AS POAName,CreateUser,(select DP_Name+' ['+DP_DepositoryID+']' from Master_DP where DP_DepositoryID=replace(tbl_master_contactDPDetails.dpd_dpCode,char(160),'')) as DP from tbl_master_contactDPDetails where dpd_cntId=@CntId"--%>
                        
                        
                        <%--<asp:SqlDataSource ID="AssetDetaildata" runat="server" 
                            SelectCommand="" 
                            InsertCommand="INSERT INTO Master_AssetDetail(AssetDetail_CompanyID, AssetDetail_FinYear,AssetDetail_MainAccountCode,AssetDetail_BroughtForward,AssetDetail_SubAccountCode,AssetDetail_PurchaseDate, AssetDetail_Category, AssetDetail_Vendor, AssetDetail_CostPrice, AssetDetail_Additions, AssetDetail_Disposals, AssetDetail_Depreciation,AssetDetail_DepreciationIT, AssetDetail_Location, AssetDetail_User, AssetDetail_Insurer, AssetDetail_Premium, AssetDetail_PolicyExpiryDate, AssetDetail_PremiumDueDate, AssetDetail_ServiceProvider, AssetDetail_AMCExpiryDate) VALUES (@AssetDetail_CompanyID,@AssetDetail_FinYear,@AssetDetail_MainAccountCode,@AssetDetail_BroughtForward,@AssetDetail_SubAccountCode,@AssetDetail_PurchaseDate,@AssetDetail_Category,@AssetDetail_Vendor,@AssetDetail_CostPrice,@AssetDetail_Additions,@AssetDetail_Disposals,@AssetDetail_Depreciation,@AssetDetail_DepreciationIT,@AssetDetail_Location,@AssetDetail_User,@AssetDetail_Insurer,@AssetDetail_Premium,@AssetDetail_PolicyExpiryDate,@AssetDetail_PremiumDueDate,@AssetDetail_ServiceProvider,@AssetDetail_AMCExpiryDate)"
                            UpdateCommand="UPDATE [Master_AssetDetail] SET AssetDetail_CompanyID=@AssetDetail_CompanyID,[AssetDetail_FinYear]=@AssetDetail_FinYear,[AssetDetail_MainAccountCode]=@AssetDetail_MainAccountCode,[AssetDetail_BroughtForward]=@AssetDetail_BroughtForward,[AssetDetail_Category]=@AssetDetail_Category,[AssetDetail_PurchaseDate]=@AssetDetail_PurchaseDate,[AssetDetail_Vendor]=@AssetDetail_Vendor,AssetDetail_CostPrice=@AssetDetail_CostPrice,[AssetDetail_Additions]=@AssetDetail_Additions,[AssetDetail_Disposals]=@AssetDetail_Disposals,[AssetDetail_Depreciation]=@AssetDetail_Depreciation,[AssetDetail_DepreciationIT]=@AssetDetail_DepreciationIT,[AssetDetail_Location]=@AssetDetail_Location,[AssetDetail_User]=@AssetDetail_User,[AssetDetail_Insurer]=@AssetDetail_Insurer,[AssetDetail_Premium]=@AssetDetail_Premium,[AssetDetail_PolicyExpiryDate]=@AssetDetail_PolicyExpiryDate,[AssetDetail_PremiumDueDate]=@AssetDetail_PremiumDueDate,[AssetDetail_ServiceProvider]=@AssetDetail_ServiceProvider,[AssetDetail_AMCExpiryDate]=@AssetDetail_AMCExpiryDate where AssetDetail_ID=@AssetDetail_ID"
                            DeleteCommand="DELETE FROM [Master_AssetDetail] WHERE AssetDetail_ID=@AssetDetail_ID">
                            <InsertParameters>
                                <asp:Parameter Name="AssetDetail_CompanyID" Type="string" />
                                <asp:Parameter Name="AssetDetail_FinYear" Type="string"/>
                                <asp:Parameter Name="AssetDetail_MainAccountCode" Type="string" />
                                <asp:Parameter Name="AssetDetail_BroughtForward" Type="string" />
                                <asp:Parameter Name="AssetDetail_SubAccountCode" Type="string" />
                                <asp:SessionParameter SessionField="KeyVal" Type="string" Name="AssetDetail_SubAccountCode" />
                                <asp:Parameter Name="AssetDetail_PurchaseDate" Type="datetime" />
                                <asp:Parameter Name="AssetDetail_Category" Type="string" />
                                <asp:Parameter Name="AssetDetail_Vendor" Type="string" />
                                <asp:Parameter Name="AssetDetail_CostPrice" Type="Decimal"/>
                                <asp:Parameter Name="AssetDetail_Additions" Type="Decimal" />
                                <asp:Parameter Name="AssetDetail_Disposals" Type="Decimal"/>
                                <asp:Parameter Name="AssetDetail_Depreciation" Type="Decimal"/>
                                <asp:Parameter Name="AssetDetail_DepreciationIT" Type="Decimal"/>
                                <asp:Parameter Name="AssetDetail_Location" Type="Int32" />
                                <asp:Parameter Name="AssetDetail_User" Type="String" />
                                <asp:Parameter Name="AssetDetail_Insurer" Type="string" />
                                <asp:Parameter Name="AssetDetail_Premium" Type="Decimal" />
                                <asp:Parameter Name="AssetDetail_PolicyExpiryDate" Type="datetime"/>
                                <asp:Parameter Name="AssetDetail_PremiumDueDate" Type="datetime" />
                                <asp:Parameter Name="AssetDetail_ServiceProvider" Type="string" />
                                <asp:Parameter Name="AssetDetail_AMCExpiryDate" Type="datetime"/>
                            </InsertParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="AssetDetail_ID" Type="Int32" /> 
                                <asp:Parameter Name="AssetDetail_CompanyID" Type="String"/>
                                <asp:Parameter Name="AssetDetail_FinYear"/>
                                <asp:Parameter Name="AssetDetail_MainAccountCode"/>
                                <asp:Parameter Name="AssetDetail_BroughtForward" Type="string" />
                                <asp:Parameter Name="AssetDetail_SubAccountCode"/>
                                <asp:Parameter Name="AssetDetail_Category"/>
                                <asp:Parameter Name="AssetDetail_PurchaseDate"/>
                                <asp:Parameter Name="AssetDetail_Vendor"/>
                                <asp:Parameter Name="AssetDetail_CostPrice" Type="Decimal"/>
                                <asp:Parameter Name="AssetDetail_Additions"/>
                                <asp:Parameter Name="AssetDetail_Disposals"/>
                                <asp:Parameter Name="AssetDetail_Depreciation"/>
                                <asp:Parameter Name="AssetDetail_DepreciationIT" Type="Decimal"/>
                                <asp:Parameter Name="AssetDetail_Location" Type="Int32"/>
                                <asp:Parameter Name="AssetDetail_User" Type="String"/>
                                <asp:Parameter Name="AssetDetail_Insurer"/>
                                <asp:Parameter Name="AssetDetail_Premium"/>
                                <asp:Parameter Name="AssetDetail_PolicyExpiryDate"/>
                                <asp:Parameter Name="AssetDetail_PremiumDueDate"/>
                                <asp:Parameter Name="AssetDetail_ServiceProvider"/>
                                <asp:Parameter Name="AssetDetail_AMCExpiryDate"/>
                            </UpdateParameters>
                        </asp:SqlDataSource>--%>
                        <%--<asp:SqlDataSource ID="Company" runat="server" 
                            SelectCommand="Select cmp_Name as CompanyName,cmp_internalid as ID from tbl_master_company Order By cmp_Name">
                        </asp:SqlDataSource>--%>
                        <asp:SqlDataSource ID="FinYear" runat="server" 
                            SelectCommand="Select FinYear_ID as ID,FinYear_Code as FinancialYear from Master_FinYear Order By FinYear_Code Desc">
                        </asp:SqlDataSource>
                       <asp:HiddenField ID="hdnMainAccountCode" runat="server"></asp:HiddenField>
                       <asp:HiddenField ID="hdnSubAccountCode" runat="server"></asp:HiddenField>
                    </td>
                </tr>
               <%--.......................... Code Added By Sam on 21112016..............................................--%>
                <tr>
                    <td>
                       
                    </td>
                </tr>
                <%--ContentStyle-CssClass="pad"--%>
              <%--.......................... Code Above Added By Sam on 21112016..............................................--%>

            </table>
         <div class="PopUpArea">
               <dxe:ASPxPopupControl ID="Popup_Empcitys" runat="server" ClientInstanceName="cPopup_Empcitys" Height="540px"
                Width="700px" HeaderText="Add/Modify DP Details" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
                PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                >
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">                         
                        <div class="Top">
                            <div class="col-md-6">
                                <div class="cityDiv" style="height: auto;margin-bottom:5px;"> 
                                    Company <span style="color:red;"> *</span>
                                </div>
                                <div class="Left_Content">
                                    <dxe:ASPxComboBox ID="cmbCompany" ClientInstanceName="combCompany" Enabled="false" EnableIncrementalFiltering="True"
                                        TextField="CompanyName" ValueField="ID" runat="server" ValueType="System.String" SelectedIndex="0"
                                        TabIndex="1" Width="100%">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>

                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="cityDiv" style="height: auto;margin-bottom:5px;">
                                    Vendor
                                    <%--<asp:Label ID="LblName" runat="server" Text="Name" CssClass="newLbl"></asp:Label>--%>
                                </div>
                                <div class="Left_Content">
                                    <%-- <asp:TextBox ID="txtVendor" CssClass="EcoheadCon" runat="server"  TabIndex="10" MaxLength="10" Width="100%"></asp:TextBox> data-placeholder="Select..."--%>
                                     <asp:ListBox ID="lstVendor"  runat="server" Font-Size="12px" ClientIDMode="Static"  Width="253px" ></asp:ListBox>
                                </div>
                            </div>
                            <div style="clear: both"></div>
                            <div class="col-md-6">
                                <div class="cityDiv" style="height: auto;">
                                    <%--Description--%>
                                   Asset Category
                                </div>
                                <div class="Left_Content">
                                    <dxe:ASPxComboBox ID="cmbAssetCategory" ClientInstanceName="combAssetCategory"  EnableIncrementalFiltering="True"
                                                    EnableSynchronization="False" ValueType="System.String" Value='<%#Bind("AssetDetail_Category") %>'
                                                    runat="server"  
                                                      TabIndex="2" Width="100%">
                                                    <Items>
                                                        <dxe:ListEditItem Text="Movable" Value="M" />
                                                        <dxe:ListEditItem Text="Immovable" Value="I" />
                                                        <dxe:ListEditItem Text="Work-In-Progress" Value="W" />
                                                    </Items>
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxComboBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="cityDiv" style="height: auto;">
                                    Service Provider
                                </div>
                                <div class="Left_Content">
                                    <%--<asp:TextBox ID="txtServiceProvider" CssClass="EcoheadCon" Text='<%#Bind("AssetDetail_ServiceProvider") %>'
                                                    runat="server"  TabIndex="11" MaxLength="10" Width="100%"></asp:TextBox>--%>
                                    <asp:ListBox ID="lstServicepro"   runat="server" Font-Size="12px" ClientIDMode="Static"  Width="253px" ></asp:ListBox>
                                </div>
                            </div>
                            <%--<div class="col-md-6">
                                <div class="cityDiv" style="height: auto;">
                                    
                                    <asp:Label ID="LblPCcode" runat="server" Text="Class Name" CssClass="newLbl"></asp:Label>
                                </div>
                                <div class="Left_Content">
                                    <dxe:ASPxComboBox ID="CmbProClassCode" ClientInstanceName="cCmbProClassCode" runat="server" SelectedIndex="0" TabIndex="5" ClearButton-DisplayMode="Always"
                                        ValueType="System.String" Width="226px" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>--%>

                            <div style="clear: both"></div>
                            <div class="col-md-6">
                                <div class="cityDiv" style="height: auto;"> 
                                    <%--<asp:Label ID="LblGlobalCode" runat="server" Text="Financial Year :"  CssClass="newLbl"  > <span style="color:red;"> *</span></asp:Label>--%>
                                    Financial Year <span style="color:red;"> *</span>
                                </div>
                                <div class="Left_Content">
                                     <dxe:ASPxComboBox ID="cmbFinYear" EnableIncrementalFiltering="True" EnableSynchronization="False" ClientInstanceName="combFinYear"
                                                    TextField="FinancialYear" ValueField="FinancialYear" 
                                                    runat="server" ValueType="System.String" 
                                                     SelectedIndex="0" 
                                                     TabIndex="3" Width="100%">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxComboBox>
                                    <%--<dxe:ASPxTextBox ID="txtGlobalCode" ClientInstanceName="ctxtGlobalCode" MaxLength="30" TabIndex="6"
                                        runat="server" Width="226px" Text='<%# Bind("txtMarkets_Description") %>'>
                                    </dxe:ASPxTextBox>--%>
                                </div>
                            </div>
                           <%-- Value='<%#Bind("AssetDetail_AMCExpiryDate") %>' --%>
                            <div class="col-md-6">
                                <div class="cityDiv" style="height: auto;">
                                    <%--Quote Currency--%>
                                    <asp:Label ID="LblQCurrency" runat="server" Text="AMC Expiry" CssClass="newLbl"></asp:Label>
                                </div>
                                <div class="Left_Content">
                                   <dxe:ASPxDateEdit ID="dtpExpiryDate" runat="server"  EditFormatString="dd-MM-yyyy" ClientInstanceName="cdtpExpiryDate"
                                                    EditFormat="Custom" 
                                                     
                                                     TabIndex="12" Width="100%">
                                                </dxe:ASPxDateEdit>
                                </div>
                            </div>

                            <div style="clear: both"></div>

                            <div class="col-md-6">
                                <div class="cityDiv" style="height: auto;margin-bottom:5px;">
                                   Date Of Purchase 
                                    <%--<asp:Label ID="LblQLot" runat="server" Text="Quote Lot" CssClass="newLbl"></asp:Label>--%>
                                </div>
                                <div class="Left_Content">
                                   <%-- Value='<%#Bind("AssetDetail_PurchaseDate") %>'--%>
                                    <dxe:ASPxDateEdit ID="dtpPurchaseDate" runat="server"  EditFormatString="dd-MM-yyyy" ClientInstanceName="cdtpPurchaseDate"
                                                    EditFormat="Custom"  
                                                     TabIndex="4" Width="100%">
                                                </dxe:ASPxDateEdit>
                                    <%--<dxe:ASPxTextBox ID="txtQuoteLot" ClientInstanceName="ctxtQuoteLot" MaxLength="8" TabIndex="8"
                                        runat="server" Width="226px" Text='<%# Bind("txtMarkets_Description") %>'>
                                        <ValidationSettings  Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="product"  ErrorTextPosition="Right"  ErrorImage-ToolTip="Mandatory" SetFocusOnError="True" >
                                           <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                           </ValidationSettings>
                                         <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" /> 
                                    </dxe:ASPxTextBox>--%>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="cityDiv" style="height: auto;">
                                    Insurer
                                </div>
                                <div class="Left_Content">
                                    <asp:ListBox ID="lstinsurer"   runat="server" Font-Size="12px" ClientIDMode="Static"  Width="253px" ></asp:ListBox>
                                     <%--<asp:TextBox ID="txtInsurer" CssClass="EcoheadCon" Text='<%#Bind("AssetDetail_Insurer") %>'
                                                    runat="server"  TabIndex="13" MaxLength="10" Width="100%"></asp:TextBox>--%>
                                    <%--<dxe:ASPxComboBox ID="CmbQuoteLotUnit" ClientInstanceName="cCmbQuoteLotUnit" runat="server" TabIndex="9" ClearButton-DisplayMode="Always"
                                        ValueType="System.String" Width="226px" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                        <ValidationSettings  Display="Dynamic" ErrorDisplayMode="ImageWithTooltip"  ValidationGroup="product"    ErrorTextPosition="Right"  ErrorImage-ToolTip="Mandatory" SetFocusOnError="True" >
                                           <RequiredField IsRequired="True"  />
                                           </ValidationSettings>
                                    </dxe:ASPxComboBox>--%>
                                </div>
                            </div>


                            <div style="clear: both"></div>

                            <div class="col-md-6">
                                <div class="cityDiv" style="height: auto;">
                                    Cost Price 
                                    <%--<asp:Label ID="LblTradingLot" runat="server" Text="Trading Lot" CssClass="newLbl"></asp:Label>--%>
                                </div>
                                <div class="Left_Content">
                                    <dxe:ASPxTextBox ID="txtCostPrice" ClientInstanceName="ctxtCostPrice" runat="server"   MaxLength="18"
                                                      TabIndex="5" Width="100%" DisplayFormatString="{0:N2}">
                                       
                                    <MaskSettings Mask="<0..999999999999g>.<0..99g>" />
                                        <%--<MaskSettings Mask="0,00,000..9,99,999" />--%>
                                         <%--<MaskSettings Mask="<0..999g>.<0..99g>" />--%>
                                                   <%-- <MaskSettings Mask="<0..999999999999999999999999999999g>.<00..99>" ErrorText="None"
                                                        IncludeLiterals="DecimalSymbol" />--%>
                                                </dxe:ASPxTextBox>
                                    <%--<dxe:ASPxTextBox ID="txtTradingLot" ClientInstanceName="ctxtTradingLot"  MaxLength="8" TabIndex="10"
                                        runat="server" Width="226px" Text='<%# Bind("txtMarkets_Description") %>'>
                                        <ValidationSettings  Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="product"  ErrorTextPosition="Right"  ErrorImage-ToolTip="Mandatory" SetFocusOnError="True" >
                                           <RequiredField IsRequired="True"  />
                                           </ValidationSettings>
                                        <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                    </dxe:ASPxTextBox>--%>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="cityDiv" style="height: auto;">
                                    <%--Trading Lot Units--%>
                                    <asp:Label ID="LblTLotUnit" runat="server" Text="Policy Expiry" CssClass="newLbl"></asp:Label>
                                </div>
                               <%--  Value='<%#Bind("AssetDetail_PolicyExpiryDate") %>' --%>
                                <div class="Left_Content">
                                    <dxe:ASPxDateEdit  ID="dtpPolicyExpiry" runat="server" ClientInstanceName="cdtpPolicyExpiry"
                                                     EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                                   
                                                     TabIndex="14" Width="100%">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>
                                    <%--<dxe:ASPxComboBox ID="CmbTradingLotUnits" ClientInstanceName="cCmbTradingLotUnits" runat="server" TabIndex="11" ClearButton-DisplayMode="Always"
                                        ValueType="System.String" Width="226px" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                    </dxe:ASPxComboBox>--%>
                                </div>
                            </div>

                            <div style="clear: both"></div>
                            <div class="col-md-6">
                                <div class="cityDiv" style="height: auto;">
                                    Additions
                                    <%--<asp:Label ID="LblDeliveryLot" runat="server" Text="Delivery Lot" CssClass="newLbl"></asp:Label>--%>
                                </div>
                                <div class="Left_Content">
                                     <dxe:ASPxTextBox ID="txtAddition" ClientInstanceName="ctxtAddition" runat="server"   MaxLength="18"
                                                      TabIndex="6" Width="100%" DisplayFormatString="{0:N2}">
                                        
                                           <MaskSettings Mask="<0..999999999999g>.<0..99g>" />
                                                    <%--<MaskSettings Mask="<0..999999999999999999999999999999g>.<00..99>" ErrorText="None"
                                                        IncludeLiterals="DecimalSymbol" />--%>
                                                </dxe:ASPxTextBox>
                                   <%-- <dxe:ASPxTextBox ID="txtDeliveryLot" ClientInstanceName="ctxtDeliveryLot" MaxLength="8" TabIndex="12"
                                        runat="server" Width="226px" Text='<%# Bind("txtMarkets_Description") %>'>
                                        <ValidationSettings  Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="product"  ErrorTextPosition="Right"  ErrorImage-ToolTip="Mandatory" SetFocusOnError="True" >
                                           <RequiredField IsRequired="True"  />
                                           </ValidationSettings>
                                        <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" /> 
                                    </dxe:ASPxTextBox>--%>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="cityDiv" style="height: auto;">
                                    <%--Delivery Lot Unit--%>
                                    <asp:Label ID="LblDeliveryLotUnit" runat="server" Text="Premium Due" CssClass="newLbl"></asp:Label>
                                </div>
                               <%-- Value='<%#Bind("AssetDetail_PremiumDueDate") %>'--%>
                                <div class="Left_Content">
                                    <dxe:ASPxDateEdit ID="dtpPremiumDue" runat="server"  EditFormat="Custom" ClientInstanceName="cdtpPremiumDue"
                                                    EditFormatString="dd-MM-yyyy"  
                                                     
                                                     TabIndex="15" Width="100%">
                                                </dxe:ASPxDateEdit>
                                    <%--<dxe:ASPxComboBox ID="CmbDeliveryLotUnit" ClientInstanceName="cCmbDeliveryLotUnit" runat="server" TabIndex="13" ClearButton-DisplayMode="Always"
                                        ValueType="System.String" Width="226px" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                    </dxe:ASPxComboBox>--%>
                                </div>
                            </div>



                            <div style="clear: both"></div>
                             

                            <%-- //......................... Code Commented and Updated  by Sam on 04-10-2014............................--%>
                            

                            <div class="col-md-6">
                                <div class="cityDiv" style="height: auto;">
                                     
                                    <asp:Label ID="LblProductColor" runat="server" Text="Disposals" CssClass="newLbl"></asp:Label>
                                </div>
                                <div class="Left_Content">
                                    <dxe:ASPxTextBox ID="txtDisposals" ClientInstanceName="ctxtDisposals" runat="server"   MaxLength="18"
                                                    Text='<%#Bind("AssetDetail_Disposals") %>'  TabIndex="7" Width="100%" DisplayFormatString="{0:N2}">
                                        <%-- <MaskSettings Mask="<0..999g>.<0..99g>" />--%>
                                         <MaskSettings Mask="<0..999999999999g>.<0..99g>" />
                                                    <%--<MaskSettings Mask="<0..999999999999999999999999999999g>.<00..99>" ErrorText="None"
                                                        IncludeLiterals="DecimalSymbol" />--%>
                                                </dxe:ASPxTextBox>
                                    <%--<dxe:ASPxComboBox ID="CmbProductColor" ClientInstanceName="cCmbProductColor" ClearButton-DisplayMode="Always" runat="server" TabIndex="14"
                                        ValueType="System.String" Width="226px" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                    </dxe:ASPxComboBox>--%>
                                </div>
                            </div>


                            <div class="col-md-6">
                                <div class="cityDiv" style="height: auto;">
                                    Location
                                </div>
                                <div class="Left_Content">
                                    
                                    <%--<asp:TextBox ID="txtLocation" CssClass="EcoheadCon" runat="server" 
                                                    Width="100%" TabIndex="16"></asp:TextBox>--%>
                                    <asp:ListBox ID="lstlocation"   runat="server" Font-Size="12px" Height="90px" Width="253px" ClientIDMode="Static"  ></asp:ListBox>
                                </div>
                            </div>

                           <%-- //......................... Code Commented and Updated  by Sam on 04-10-2014............................--%>
                            <div style="clear: both"></div>

                            <div class="col-md-6" >
                                <div class="cityDiv" style="height: auto;">
                                     
                                    <asp:Label ID="LblProductSize" runat="server" Text="Depreciation" CssClass="newLbl"></asp:Label>
                                </div>
                                <div class="Left_Content">
                                    <dxe:ASPxTextBox ID="txtDepreciation" ClientInstanceName="ctxtDepreciation" runat="server"  MaxLength="18"
                                                    Text='<%#Bind("AssetDetail_Depreciation") %>'  TabIndex="8" Width="100%" DisplayFormatString="{0:N2}">
                                        
                                                    <MaskSettings Mask="<0..999999999999g>.<0..99g>" />
                                                    <%--<MaskSettings Mask="<0..999999999999999999999999999999g>.<00..99>" ErrorText="None"
                                                        IncludeLiterals="DecimalSymbol" />--%>
                                                </dxe:ASPxTextBox>
                                    <%--<dxe:ASPxComboBox ID="CmbProductSize" ClientInstanceName="cCmbProductSize" ClearButton-DisplayMode="Always" runat="server" TabIndex="16"
                                        ValueType="System.String" Width="226px" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                    </dxe:ASPxComboBox>--%>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="cityDiv" style="height: auto;">
                                    Brought Forward
                                </div>
                                <div class="Left_Content">
                                     <dxe:ASPxTextBox ID="txtBroughtForward" ClientInstanceName="ctxtBroughtForward" runat="server"   MaxLength="18"
                                                    Text='<%#Bind("AssetDetail_BroughtForward") %>'  TabIndex="17" Width="100%" DisplayFormatString="{0:N2}">
                                          <MaskSettings Mask="<0..999999999999g>.<0..99g>" />
                                         <%--<MaskSettings Mask="<0..999g>.<0..99g>" />--%>
                                                   <%-- <MaskSettings Mask="<0..999999999999999999999999999999g>.<00..99>" ErrorText="None"
                                                        IncludeLiterals="DecimalSymbol" />--%>
                                                </dxe:ASPxTextBox>
                                    <%--<dxe:ASPxRadioButtonList ID="rdblapp" ClientInstanceName="Rrdblapp" runat="server" RepeatDirection="Horizontal" Width="226px" TabIndex="17">
                                        <Items>
                                            <dxe:ListEditItem Text="Applicable" Value="1" Selected="true" />
                                            <dxe:ListEditItem Text="Not Applicable" Value="0" />
                                             MaxLength="18"
                                                      TabIndex="5" Width="100%" DisplayFormatString="{0:N2}">
                                       
                                    <MaskSettings Mask="<0..999999999999g>.<0..99g>" />
                                        </Items>
                                    </dxe:ASPxRadioButtonList>--%>
                                </div>
                            </div>

                            

                             <div style="clear: both"></div>
                            <div class="col-md-6">
                                <div class="cityDiv" style="height: auto;">
                                   DepreciationIT :  
                                    <%--<asp:Label ID="LblDeliveryLot" runat="server" Text="Delivery Lot" CssClass="newLbl"></asp:Label>--%>
                                </div>
                                <div class="Left_Content">
                                    <dxe:ASPxTextBox ID="txtDepreciationIT" ClientInstanceName="ctxtDepreciationIT" runat="server"  MaxLength="18"
                                                    Text='<%#Bind("AssetDetail_DepreciationIT") %>'  TabIndex="9" Width="100%" DisplayFormatString="{0:N2}">
                                        
                                        <MaskSettings Mask="<0..999999999999g>.<0..99g>" />
                                        <%-- <MaskSettings Mask="<0..999g>.<0..99g>" />--%>
                                                   <%-- <MaskSettings Mask="<0..999999999999999999999999999999g>.<00..99>" ErrorText="None"
                                                        IncludeLiterals="DecimalSymbol" />--%>
                                                </dxe:ASPxTextBox>
                                                 
                                   <%-- <dxe:ASPxTextBox ID="txtDeliveryLot" ClientInstanceName="ctxtDeliveryLot" MaxLength="8" TabIndex="12"
                                        runat="server" Width="226px" Text='<%# Bind("txtMarkets_Description") %>'>
                                        <ValidationSettings  Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="product"  ErrorTextPosition="Right"  ErrorImage-ToolTip="Mandatory" SetFocusOnError="True" >
                                           <RequiredField IsRequired="True"  />
                                           </ValidationSettings>
                                        <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" /> 
                                    </dxe:ASPxTextBox>--%>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="cityDiv" style="height: auto;">
                                    <%--Delivery Lot Unit--%>
                                    <asp:Label ID="Label1" runat="server" Text="Used By :" CssClass="newLbl"></asp:Label>
                                </div>
                                <div class="Left_Content">
                                    <asp:ListBox ID="lstusedby"   runat="server" Font-Size="12px"  Width="253px" ClientIDMode="Static"></asp:ListBox>
                                    <%--<asp:TextBox ID="txtUsedBy" CssClass="EcoheadCon" runat="server" Width="100%"
                                                    TabIndex="18" MaxLength="10"></asp:TextBox>--%>
                                    <%--<dxe:ASPxComboBox ID="CmbDeliveryLotUnit" ClientInstanceName="cCmbDeliveryLotUnit" runat="server" TabIndex="13" ClearButton-DisplayMode="Always"
                                        ValueType="System.String" Width="226px" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                    </dxe:ASPxComboBox>--%>
                                </div>
                            </div>

                        </div>
                        <div class="ContentDiv" style="height: auto">
                             
                           
                            <br style="clear: both;" />
                             <div class="col-md-12"> <span style="color:red;"> * Denotes mandatory field</span></div>
                            <div class="Footer clearfix" style="padding-left: 16px">
                                

                                <dxe:ASPxButton ID="btnSave_citys" CausesValidation="true" ClientInstanceName="cbtnSave_citys" runat="server" ValidationGroup="product"
                                    AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                                    <ClientSideEvents Click="function (s, e) {btnSave_citys();}" />
                                </dxe:ASPxButton>


                                <dxe:ASPxButton ID="btnCancel_citys" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger">
                                    <ClientSideEvents Click="function (s, e) {fn_btnCancel();}" />
                                </dxe:ASPxButton>

                                <input type="button" value="Assing Values" style="display: none;" onclick="fetchLebel()" class="btn btn-primary">

                                 <div class="HiddenFieldArea" style="display: none;">
                                        <asp:HiddenField runat="server" ID="hiddenedit" />
                                </div>

                                <br style="clear: both;" />
                            </div>
                            <br style="clear: both;" />
                        </div>
                        <%-- </div>--%>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="LightGray" ForeColor="Black" />
            </dxe:ASPxPopupControl>
             <dxe:ASPxGridViewExporter ID="exporter" GridViewID="AssetDetailGrid" runat="server"  Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true" >
        </dxe:ASPxGridViewExporter>
        </div>
         </div>
</asp:Content>
