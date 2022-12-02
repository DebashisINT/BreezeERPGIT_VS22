<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GSTReturn.aspx.cs" Inherits="ERP.OMS.Reports.GSTReturn" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableViewStateMac="false" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>

        function ShowReportClick() {

            cTaxGridPanel.PerformCallback();
        }

        function Callback_EndCallback() {
 
            $("#drdExport").val(0);
            $("#DropDownList1").val(0);
            $("#cmbDetailITC").val(0);
        }


    </script>
    
    
     <div class="panel-heading">
        <div class="panel-title">
            <h3>GST Return</h3>
        </div>
    </div>

     



    <div class="form_main">
        <div class="SearchArea">
              <div class="clearfix row">
                    <div class="col-md-2">
                        <label>GSTIN: </label>
                        <div > 
                        
                        <dxe:ASPxComboBox ID="cmbGstinlist" ClientInstanceName="ccmbGstinlist" runat="server" SelectedIndex="0"
                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                        </dxe:ASPxComboBox>
                        </div> 
                    </div> 

                   <%-- <div class="col-md-3">

                                <label>Month: </label>
                                <dxe:ASPxComboBox ID="ccmbMonth" ClientInstanceName="cccmbMonth" runat="server" SelectedIndex="0"
                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                </dxe:ASPxComboBox>
                    </div>  --%>


                    <div class="col-md-2">

                                <label>From Date: </label>
                                <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                    </div>  


                    <div class="col-md-2">

                                <label>To Date: </label>
                        
                                 <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                  </dxe:ASPxDateEdit>
                    </div>  

                    <div class="col-md-3" style="padding-top:15px;">
                         
                        <dxe:ASPxCheckBox ID="chkByPartyinvdate" runat="server" Checked="true"></dxe:ASPxCheckBox>
                        <%--<label>Show by Purchase Entry Date </label>--%>
                        <label>Search by Party Invoice Date</label>
                    </div>

                   <div class="col-md-2" style="padding-top:19px;padding-left:0">
                    <input type="button" class="btn btn-primary " onclick="ShowReportClick()" value="Show"/>
                    </div>  
         
                </div>
                   
         
            <br />
         
                <div class="FilterSide clearfix">
                    <h4 class="pull-left">Details of outward supplies and inward supplies liable to reverse charge</h4>

                    <div style="float: right; padding-right: 5px;"> 
                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" OnChange="if(!AvailableExportOption()){return false;}" AutoPostBack="true">
                            <asp:ListItem Value="0">Export to</asp:ListItem> 
                            <asp:ListItem Value="2">XLS</asp:ListItem> 
                        </asp:DropDownList>
                        
                    </div>
                  
                </div>
              
            </div>
            <dxe:ASPxCallbackPanel runat="server" ID="TaxGridPanel" ClientInstanceName="cTaxGridPanel" OnCallback="Component_Callback" ClientSideEvents-BeginCallback="Callback_EndCallback">
                                        <PanelCollection>
                                            <dxe:PanelContent runat="server">

            <div class="GridViewArea">
                <dxe:ASPxGridView ID="GSTGrid3_1" runat="server" AutoGenerateColumns="False" ClientInstanceName="cGSTGrid3_1"
                     OnDataBinding="cGSTGrid3_1_DataBinding" SettingsPager-Mode="ShowAllRecords" 
                      Width="100%"   CssClass="pull-left"  >
                    <SettingsSearchPanel Visible="True" Delay="5000" />
                    <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu="True" />
                    <Columns>

                        <dxe:GridViewDataTextColumn Caption="Nature of Supplies" FieldName="NATURE" ReadOnly="True" Width="50%"
                            Visible="True" FixedStyle="Left" VisibleIndex="0">
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                        
                          <dxe:GridViewDataTextColumn Caption="Total Taxable Value" FieldName="TaxableAmt" ReadOnly="True" Width="10%"
                            Visible="True" FixedStyle="Left" VisibleIndex="0">
                              <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Integrated Tax" FieldName="IgstAmt" ReadOnly="True" Width="10%" 
                            Visible="True" FixedStyle="Left" VisibleIndex="0">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>


                           <dxe:GridViewDataTextColumn Caption="Central Tax" FieldName="CgstAmt" ReadOnly="True" Width="10%"
                            Visible="True" FixedStyle="Left" VisibleIndex="0">
                               <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                          <dxe:GridViewDataTextColumn Caption="State/UT Tax" FieldName="StateTaxAmt" ReadOnly="True" Width="10%"
                            Visible="True" FixedStyle="Left" VisibleIndex="0">
                              <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                              <dxe:GridViewDataTextColumn Caption="Cess" FieldName="cess" ReadOnly="True" Width="10%"
                            Visible="True" FixedStyle="Left" VisibleIndex="0">
                                  <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                    </Columns>
                    
                    <SettingsBehavior ColumnResizeMode="NextColumn" /> 
                </dxe:ASPxGridView>

                 <dxe:ASPxGridViewExporter ID="exporter3_1" runat="server" Landscape="true" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                </dxe:ASPxGridViewExporter>


            </div>
                
            



            <br />
            <br />
            <br />

              <div class="FilterSide clearfix">
                    <h4 class="pull-left"> Details of inter-State supplies made to unregistered persons, composition dealer and UIN holders</h4>

                    <div style="float: right; padding-right: 5px;padding-top:5px"> 
                        <asp:DropDownList ID="DropDownList1" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged3_2"   AutoPostBack="true">
                            <asp:ListItem Value="0">Export to</asp:ListItem> 
                            <asp:ListItem Value="2">XLS</asp:ListItem> 
                        </asp:DropDownList>
                        
                    </div>
                  
                </div>



               <div class="GridViewArea">
                <dxe:ASPxGridView ID="GSTGrid3_2" runat="server" AutoGenerateColumns="False" ClientInstanceName="cGSTGrid3_2"
                     OnDataBinding="cGSTGrid3_2_DataBinding" SettingsPager-Mode="ShowAllRecords"
                      Width="100%"   CssClass="pull-left"  >
                    <SettingsSearchPanel Visible="True" Delay="5000" />
                    <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu="True" />
                    <Columns>

                        <dxe:GridViewDataTextColumn Caption="Nature of Supplies" FieldName="NATURE" ReadOnly="True" Width="40%"
                            Visible="True" FixedStyle="Left" VisibleIndex="0">
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                        
                          <dxe:GridViewDataTextColumn Caption="Place of Supply (State/UT) " FieldName="PlaceOfSupply" ReadOnly="True" Width="10%"
                            Visible="True" FixedStyle="Left" VisibleIndex="0"> 
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Total Taxable Value" FieldName="TaxableAmt" ReadOnly="True" Width="20%" 
                            Visible="True" FixedStyle="Left" VisibleIndex="0">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>


                           <dxe:GridViewDataTextColumn Caption="Amount of Integrated Tax" FieldName="IgstAmt" ReadOnly="True" Width="10%"
                            Visible="True" FixedStyle="Left" VisibleIndex="0">
                               <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                         
                    </Columns>
                    
                    <SettingsBehavior ColumnResizeMode="NextColumn" /> 
                </dxe:ASPxGridView>

                 <dxe:ASPxGridViewExporter ID="exporter3_2" runat="server" Landscape="true" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                </dxe:ASPxGridViewExporter>


            </div>


         <br />
            <br />
            <br />

              <div class="FilterSide clearfix">
                    <h4 class="pull-left"> Details of eligible Input Tax Credit</h4>

                    <div style="float: right; padding-right: 5px;padding-top:5px"> 
                        <asp:DropDownList ID="cmbDetailITC" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="DetailITC_SelectedIndexChanged3_2"   AutoPostBack="true">
                            <asp:ListItem Value="0">Export to</asp:ListItem> 
                            <asp:ListItem Value="2">XLS</asp:ListItem> 
                        </asp:DropDownList>
                        
                    </div>
                  
                </div>



               <div class="GridViewArea">
                <dxe:ASPxGridView ID="DetailITC" runat="server" AutoGenerateColumns="False" ClientInstanceName="cDetailITC"
                     OnDataBinding="DetailITC_DataBinding" SettingsPager-Mode="ShowAllRecords"
                      Width="100%"   CssClass="pull-left"  >
                    <SettingsSearchPanel Visible="True" Delay="5000" />
                    <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu="True" />
                    <Columns>

                        <dxe:GridViewDataTextColumn Caption="Details" FieldName="DETAILS" ReadOnly="True" Width="40%"
                            Visible="True" FixedStyle="Left" VisibleIndex="0">
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                          

                        <dxe:GridViewDataTextColumn Caption="Integrated Tax" FieldName="IgstAmt" ReadOnly="True" Width="20%" 
                            Visible="True" FixedStyle="Left" VisibleIndex="0">
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>


                           <dxe:GridViewDataTextColumn Caption="Central Tax" FieldName="CgstAmt" ReadOnly="True" Width="10%"
                            Visible="True" FixedStyle="Left" VisibleIndex="0">
                               <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                              <dxe:GridViewDataTextColumn Caption="State/UT Tax" FieldName="StateTaxAmt" ReadOnly="True" Width="10%"
                            Visible="True" FixedStyle="Left" VisibleIndex="0">
                               <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                          <dxe:GridViewDataTextColumn Caption="Cess" FieldName="cess" ReadOnly="True" Width="10%"
                            Visible="True" FixedStyle="Left" VisibleIndex="0">
                               <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <EditFormSettings Visible="True" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>


                         
                    </Columns>
                    
                    <SettingsBehavior ColumnResizeMode="NextColumn" /> 
                </dxe:ASPxGridView>

                 <dxe:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" Landscape="true" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                </dxe:ASPxGridViewExporter>


            </div>





                    </dxe:PanelContent>
                </PanelCollection>
            </dxe:ASPxCallbackPanel>
        </div>
    
</asp:Content>