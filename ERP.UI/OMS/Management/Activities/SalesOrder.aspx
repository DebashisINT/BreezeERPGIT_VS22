<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="SalesOrder.aspx.cs" Inherits="ERP.OMS.Management.Activities.SalesOrder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     
    <style type="text/css">
        .inline {
            display: inline !important;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content {
            overflow: visible !important;
        }



        .abs {
            position: absolute;
            right: -20px;
            top: 10px;
        }

        .fa.fa-exclamation-circle:before {
            font-family: FontAwesome;
        }

        .tp2 {
            right: -18px;
            top: 7px;
            position: absolute;
        }

        #txtCreditLimit_EC {
            position: absolute;
        }

        #grid_DXStatus span>a {
            display:none;
        }

    </style>
    <script type="text/javascript">
        window.onload = function () {
            grid.AddNewRow();
        };
        function CloseGridLookup() {
            gridLookup.ConfirmCurrentSelection();
            gridLookup.HideDropDown();
            gridLookup.Focus();
        }
    </script>
    <script type="text/javascript">
        function GetContactPerson(e) {
            var internalid = e.Get
        }

        function SetDifference() {
            var diff = CheckDifference();
            if (diff > 0) {
                clientResult.SetText(diff.toString());
            }

        }

        function CheckDifference() {
            var startDate = new Date();
            var endDate = new Date();
            var difference = -1;
            startDate = cPLQuoteDate.GetDate();
            if (startDate != null) {
                endDate = cExpiryDate.GetDate();
                var startTime = startDate.getTime();
                var endTime = endDate.getTime();
                difference = (endTime - startTime) / 86400000;

            }
            return difference;

        }

        $(document).ready(function () {
            $('#ddl_Customer').change(function () {
                var customerid = $(this).val();
                cContactPerson.PerformCallback('BindContactPerson~' + customerid);

            })
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-title">
             
            <h3>
                <%--<asp:Label ID="lblHeadTitle" Text="" runat="server"></asp:Label>--%>
                <label>Add Sales Order</label>
            </h3>
             
        </div> 
    <div class="form_main">
        <div class="row">
        <div class="col-md-2">
            

            <dxe:ASPxLabel ID="lbl_NumberingScheme" Width="120px" runat="server" Text="Numbering Scheme">
            </dxe:ASPxLabel>

            <asp:DropDownList ID="ddl_numberingScheme"  runat="server" Width="100%" TabIndex="1">
            </asp:DropDownList>


        </div>

        <div class="col-md-2">

            <dxe:ASPxLabel ID="lbl_SaleOrderNo" runat="server" Text="Sale Order Number" Width="120px">
            </dxe:ASPxLabel>
           

            <dxe:ASPxTextBox ID="txt_SaleOrderNo"  runat="server" TabIndex="2" Width="100%">
            </dxe:ASPxTextBox>
        </div>

        <div class="col-md-2">
            <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Date" Width="120px" CssClass="inline">
            </dxe:ASPxLabel>

            <dxe:ASPxDateEdit ID="dt_SaleOrder" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy hh:mm tt" ClientInstanceName="cPLQuoteDate"  TabIndex="3" Width="100%">
                <ButtonStyle Width="13px">
                </ButtonStyle>
            </dxe:ASPxDateEdit>
        </div>


        <div class="col-md-3">

            <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="PI/Quotation"  Width="61px">
            </dxe:ASPxLabel> 
            <dxe:ASPxGridLookup ID="lookup_PIQuotation" runat="server" TabIndex="5"   ClientInstanceName="cLookupPIQuotation" KeyFieldName="cnt_internalid" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" >

                <Columns>
                    <%--<dxe:GridViewCommandColumn ShowSelectCheckbox="True" />--%>
            
                    <dxe:GridViewDataColumn FieldName="shortname" Visible="true" VisibleIndex="0" Caption="Short Name" />
                    <dxe:GridViewDataColumn FieldName="Name"  Visible="true" VisibleIndex="1" Caption="Name" Settings-AllowAutoFilter="False">
        <Settings AllowAutoFilter="False"></Settings>
                    </dxe:GridViewDataColumn>
                    <dxe:GridViewDataColumn FieldName="cnt_internalid"  Visible="false" VisibleIndex="2" Settings-AllowAutoFilter="False" >
        <Settings AllowAutoFilter="False"></Settings>
                    </dxe:GridViewDataColumn>
                </Columns>
                <GridViewProperties>
            <Templates>
                <StatusBar>
                    <table class="OptionsTable" style="float: right">
                        <tr>
                            <td>
                                <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />
                            </td>
                        </tr>
                    </table>
                </StatusBar>
            </Templates>

<SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>

            <Settings ShowFilterRow="True" ShowStatusBar="Visible" />
        </GridViewProperties> 
                <ClearButton DisplayMode="Always">
                </ClearButton>
              </dxe:ASPxGridLookup>
             </div>

        <div class="col-md-3">
            <dxe:ASPxLabel ID="lbl_OANo" runat="server" Text="PI/Quotation Date"  Width="120px" CssClass="inline">
            </dxe:ASPxLabel>
            <div style="width: 100%;height: 23px;border: 1px solid #e6e6e6;">
                <dxe:ASPxLabel ID="lbl_OADate" runat="server" Text="" Enabled="false" Width="120px" CssClass="inline">
                </dxe:ASPxLabel>
            </div>
             
        </div>
            <%--<dxe:ASPxDateEdit ID="dt_PlQuoteExpiry" runat="server" ClientInstanceName="cExpiryDate" EditFormat="Custom" EditFormatString="dd-MM-yyyy hh:mm tt"  TabIndex="4" Width="100%">
                 
                <validationsettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right"  ErrorText="Expiry date can not be shorter than Pl/Quote date.">
                    <RequiredField IsRequired="true" />
                    </validationsettings>
                 
                    <clientsideevents DateChanged="function(s,e){SetDifference();}" 
                        Validation="function(s,e){e.isValid = (CheckDifference()>=0)}"/> 
            </dxe:ASPxDateEdit>--%>

       


        <div style="clear: both">
        </div>

        <div class="col-md-2">

            <dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Customer" >
            </dxe:ASPxLabel>
            <dxe:ASPxGridLookup ID="lookup_Customer" runat="server" TabIndex="5"   ClientInstanceName="gridLookup"
        KeyFieldName="cnt_internalid" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" >

        <Columns>
            <%--<dxe:GridViewCommandColumn ShowSelectCheckbox="True" />--%>
            
            <dxe:GridViewDataColumn FieldName="shortname" Visible="true" VisibleIndex="0" Caption="Short Name" />
            <dxe:GridViewDataColumn FieldName="Name"  Visible="true" VisibleIndex="1" Caption="Name" Settings-AllowAutoFilter="False">
<Settings AllowAutoFilter="False"></Settings>
            </dxe:GridViewDataColumn>
            <dxe:GridViewDataColumn FieldName="cnt_internalid"  Visible="false" VisibleIndex="2" Settings-AllowAutoFilter="False" >
<Settings AllowAutoFilter="False"></Settings>
            </dxe:GridViewDataColumn>
        </Columns>
        <GridViewProperties>
            <Templates>
                <StatusBar>
                    <table class="OptionsTable" style="float: right">
                        <tr>
                            <td>
                                <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />
                            </td>
                        </tr>
                    </table>
                </StatusBar>
            </Templates>

<SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>

            <Settings ShowFilterRow="True" ShowStatusBar="Visible" />
        </GridViewProperties>
                <ClientSideEvents TextChanged="function(s, e) { GetContactPerson(e)
	
}" />
                <ClearButton DisplayMode="Always">
                </ClearButton>
    </dxe:ASPxGridLookup>
            <%--<asp:DropDownList ID="ddl_Customer" runat="server" TabIndex="5" Width="100%">

            </asp:DropDownList>--%>

        </div>


        <div class="col-md-2">

            <dxe:ASPxLabel ID="lbl_ContactPerson" runat="server" Text="Contact Person" CssClass="inline">
            </dxe:ASPxLabel>
             <dxe:ASPxComboBox ID="cmbContactPerson" runat="server" OnCallback="cmbContactPerson_Callback" TabIndex="6" Width="100%" ClientInstanceName="cContactPerson" Font-Size="12px">
                                     
                                    
                                     
             </dxe:ASPxComboBox>
            <%--<asp:DropDownList ID="ddl_ContactPerson" runat="server" TabIndex="6" Width="100%"></asp:DropDownList>--%>

        </div>
        <div class="col-md-2">

            

                <dxe:ASPxLabel ID="lbl_Refference" runat="server" Text="Reference">
                </dxe:ASPxLabel>
 

                <dxe:ASPxTextBox ID="txt_Refference" runat="server" TabIndex="7" Width="100%" >
                </dxe:ASPxTextBox>

            
        </div>
        <div class="col-md-3">
            <dxe:ASPxLabel ID="lbl_Branch"  runat="server" Text="Branch">
            </dxe:ASPxLabel>
            <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%" TabIndex="8">
            </asp:DropDownList>
        </div>
        
        <div class="col-md-3">
            <dxe:ASPxLabel ID="ASPxLabel3"   runat="server" Text="Salesman/Agents">
            </dxe:ASPxLabel>
            <asp:DropDownList ID="ddl_SalesAgent" runat="server" Width="100%" TabIndex="9">
            </asp:DropDownList>
        </div>
        <div style="clear: both;"></div>

        <div class="col-md-2">

            <dxe:ASPxLabel ID="lbl_Currency" runat="server" Text="Currency">
            </dxe:ASPxLabel>
            <asp:DropDownList ID="ddl_Currency" runat="server" Width="100%" TabIndex="10">
            </asp:DropDownList>


        </div>
        <div class="col-md-2">

            <dxe:ASPxLabel ID="lbl_Rate" runat="server" Text="Rate">
            </dxe:ASPxLabel>


            <dxe:ASPxTextBox ID="txt_Rate" runat="server" TabIndex="11" Width="100%" >
            </dxe:ASPxTextBox>

        </div>
        <div class="col-md-2">

            <dxe:ASPxLabel ID="lbl_AmountAre" runat="server" Text="Amounts are">
            </dxe:ASPxLabel>

            <asp:DropDownList ID="ddl_AmountAre" runat="server" TabIndex="12" Width="100%">
            </asp:DropDownList>

        </div>
        <div class="col-md-3">

            <dxe:ASPxLabel ID="lblVatGstCst" runat="server" Text="Select VAT/GST/CST">
            </dxe:ASPxLabel>
            <dxe:ASPxComboBox ID="ddl_VatGstCst"  runat="server"  TabIndex="13" Width="100%"></dxe:ASPxComboBox>

        </div>
        <div style="clear: both;"></div>
        <div class="col-md-12">

                <div style="display: none;">
                    <a href="javascript:void(0);" onclick="OnAddNewClick()" class="btn btn-primary"><span>Add New</span> </a>
                </div>
                <div>
                    <br />
                </div>
                 
                <dxe:ASPxGridView runat="server" OnBatchUpdate="grid_BatchUpdate" KeyFieldName="CashReportID" ClientInstanceName="grid" ID="grid"
                    Width="100%"  SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                    Settings-ShowFooter="false" OnCustomCallback="grid_CustomCallback" OnDataBinding="grid_DataBinding">
                    <SettingsPager Visible="false"></SettingsPager>
                    <Columns>
                        <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="true"  Width="50" VisibleIndex="0" Caption="Action">
                            <%--<HeaderTemplate>
                                <img src="../../../assests/images/Add.png" />
                            </HeaderTemplate>--%>
                            <CustomButtons>
                                <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                </dxe:GridViewCommandColumnCustomButton>
                            </CustomButtons>
                        </dxe:GridViewCommandColumn>
                         <dxe:GridViewDataTextColumn FieldName="CashReportID" Caption="Sl" ReadOnly="true" VisibleIndex="1" >
                             <PropertiesTextEdit>

                             </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>



                        <dxe:GridViewDataComboBoxColumn Caption="Product" FieldName="MainAccount1" VisibleIndex="2" Width="220px">
                            <PropertiesComboBox ValueField="CountryID" ClientInstanceName="CountryID" TextField="CountryName">
                                 
                                 
                            </PropertiesComboBox>
                        </dxe:GridViewDataComboBoxColumn>
                         <dxe:GridViewDataTextColumn FieldName="CashReportID" Caption="Description"  VisibleIndex="3" >
                             <PropertiesTextEdit>

                             </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="CashReportID" Caption="Quantity" VisibleIndex="4" >
                            <PropertiesTextEdit>

                             </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>
                         <dxe:GridViewDataTextColumn FieldName="CashReportID" Caption="UOM(Sale)" VisibleIndex="5">
                             <PropertiesTextEdit>

                             </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>
                         <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="openingBalance" Caption="Warehouse" ShowInCustomizationForm="True">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <EditCellStyle HorizontalAlign="Center">
                                </EditCellStyle>
                                <Settings ShowFilterRowMenu="False"  AllowAutoFilter="False"  />
                                <DataItemTemplate>
                                    <%-- <a href="javascript:void(0);" id="aaa" style="color:#000099;" runat="server">Add/Edit </a>--%>
                                    <dxe:ASPxHyperLink ID="AviewLink" runat="server" Text="Select...">
                                    </dxe:ASPxHyperLink>
                                </DataItemTemplate>
                                <CellStyle Wrap="False" CssClass="gridcellright" HorizontalAlign="Center">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                         <dxe:GridViewDataTextColumn FieldName="CashReportID" Caption="Stock Qty" VisibleIndex="7" >
                             <PropertiesTextEdit>

                             </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>
                         <dxe:GridViewDataTextColumn FieldName="CashReportID" Caption="Stock UOM" VisibleIndex="8" >
                             <PropertiesTextEdit>

                             </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>
                          <dxe:GridViewDataTextColumn FieldName="CashReportID" Caption="Sale Price" VisibleIndex="9" >
                              <PropertiesTextEdit>

                             </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="CashReportID" Caption="Discount" VisibleIndex="10" >
                            <PropertiesTextEdit>

                             </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="CashReportID" Caption="Amount" VisibleIndex="11" >
                            <PropertiesTextEdit>

                             </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataComboBoxColumn Caption="Tax Amount" FieldName="MainAccount1" VisibleIndex="12" Width="220px">
                            <PropertiesComboBox ValueField="CountryID" ClientInstanceName="CountryID" TextField="CountryName">
                                 
                                 
                            </PropertiesComboBox>
                        </dxe:GridViewDataComboBoxColumn>
                        <dxe:GridViewDataTextColumn FieldName="CashReportID" Caption="Total Amount in INR" VisibleIndex="13">
                            <PropertiesTextEdit>

                             </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>
                        
                         
                       
                    </Columns>
               <%--      Init="OnInit"BatchEditStartEditing="OnBatchEditStartEditing" BatchEditEndEditing="OnBatchEditEndEditing"
                        CustomButtonClick="OnCustomButtonClick" EndCallback="OnEndCallback" --%>
                    <ClientSideEvents   />
                    <SettingsDataSecurity AllowEdit="true" />
                    <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                        <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                    </SettingsEditing>
                </dxe:ASPxGridView>
            </div>
         <div style="clear: both;"></div>
        <br />
        <div class="col-md-12">
        <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="Save & [N]ew" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                            <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                        </dxe:ASPxButton>
             <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtn_SaveRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="Save & E[X]it" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                            <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                        </dxe:ASPxButton>
             <dxe:ASPxButton ID="ASPxButton2" ClientInstanceName="cbtn_SaveRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="[U]DF" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                            <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                        </dxe:ASPxButton>
             <dxe:ASPxButton ID="ASPxButton3" ClientInstanceName="cbtn_SaveRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="[T]axes" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                            <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                        </dxe:ASPxButton>
             <dxe:ASPxButton ID="ASPxButton4" ClientInstanceName="cbtn_SaveRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="[A]ttachment(s)" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                            <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                        </dxe:ASPxButton>
             <dxe:ASPxButton ID="ASPxButton5" ClientInstanceName="cbtn_SaveRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="[B]illing/Shipping" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                            <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                        </dxe:ASPxButton>
            </div>
    </div>
   </div>
</asp:Content>
