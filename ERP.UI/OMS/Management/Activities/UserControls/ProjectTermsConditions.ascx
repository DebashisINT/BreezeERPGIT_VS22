<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectTermsConditions.ascx.cs" Inherits="ERP.OMS.Management.Activities.UserControls.ProjectTermsConditions" %>


<script src="JS/ProjectTerms.js?v1.0.0.0161"></script>

 <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#ProjectTermsConditionseModal" id="Projectbtn_TermsCondition" onclick="ClickTermsconditions();">P&#818;roject Terms and Conditions</button>
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
       <%-- <ClientSideEvents ControlsInitialized="AllControlInitilizeTC" />--%>
    </dxe:ASPxGlobalEvents>
    
    


<!-- Modal -->
<div class="modal fade pmsModal w90" id="ProjectTermsConditionseModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="exampleModalLabel">Terms and Conditions</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        <div>
        <div id="DvProjTerms">
             
            <div class="row">
         
             <div class="col-md-3 col-lg-2">
                 <asp:Label ID="lbldftLiaPeriod" runat="server" Text="Defect Liability From Date"></asp:Label>

                   <dxe:ASPxDateEdit ID="dtDefectPerid" runat="server"  EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdtDefectPerid" Width="100%">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        <ClientSideEvents GotFocus="function(s,e){cdtDefectPerid.ShowDropDown();}" />
                   </dxe:ASPxDateEdit>
                   <br />  
             </div>
                 <div class="col-md-3 col-lg-2">
                 <asp:Label ID="Label1" runat="server" Text="Defect Liability To Date"></asp:Label>

                   <dxe:ASPxDateEdit ID="dtDefectPeridToDate" runat="server"  EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdtDefectPeridToDate" Width="100%">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        <ClientSideEvents GotFocus="function(s,e){cdtDefectPeridToDate.ShowDropDown();}" />
                   </dxe:ASPxDateEdit>
                     <br />  
             </div>
                <div class="col-md-3 col-lg-3">
                    <span>Defect Liability Remarks</span>
                    <div> <dxe:ASPxTextBox runat="server" id="txtDefectPerid" ClientInstanceName="ctxtDefectPerid" Width="100%"></dxe:ASPxTextBox></div>
                </div>
              <div class="col-md-3 col-lg-2">
                    <asp:Label ID="lblLiquiDamage" runat="server" Text="Liquidated Damage %"></asp:Label>

                <dxe:ASPxTextBox runat="server" id="txtLiquiDamage" Width="100%"  ClientInstanceName="ctxtLiquiDamage">
                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                </dxe:ASPxTextBox>
             </div>
             <div class="col-md-3 col-lg-2">
                 <asp:Label ID="lblLiqDmgAppliDt" runat="server" Text="Liq. Damage Applicable Dt."></asp:Label>

                 <dxe:ASPxDateEdit ID="dtLiqDmgAppliDt" runat="server"  EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdtLiqDmgAppliDt" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                       <ClientSideEvents GotFocus="function(s,e){cdtLiqDmgAppliDt.ShowDropDown();}" />
               </dxe:ASPxDateEdit>
             </div>
                
            </div>
            <div class="row">
                  <div class="col-md-5">
                    <asp:Label ID="lblPaymentTerms" runat="server" Text="Payment Terms"></asp:Label>
                    <dxe:ASPxTextBox runat="server" id="txtPaymentTerms" ClientInstanceName="ctxtPaymentTerms" Width="100%"></dxe:ASPxTextBox>
                  </div>
                  <div class="col-md-2">
                     <asp:Label ID="lblOrderType" runat="server" Text="Order Type"></asp:Label>
                     <dxe:ASPxTextBox runat="server" id="txtOrderType" ClientInstanceName="ctxtOrderType" Width="100%"></dxe:ASPxTextBox>
                  </div>
             
                  <div class="col-md-2">
                       <asp:Label ID="lblNatureWork" runat="server" Text="Nature of Work"></asp:Label>
                     <dxe:ASPxTextBox runat="server" id="txtNatureWork" ClientInstanceName="ctxtNatureWork" Width="100%"></dxe:ASPxTextBox>
                 </div>
            </div>
            
            <div class="clearfix col-md-12" style="background: #f3f2f0;padding-top: 11px;padding-bottom: 10px;margin-top:8px;">
              <div class="row">
                   <div class="col-md-2">
                           <div class="form-group ">
                                <label for="" class=" col-form-label">BG Group</label>
                                  <div class="">
                                    <div class="relative">
                                        <dxe:ASPxTextBox ID="txtBGGroup" runat="server" ClientInstanceName="ctxtBGGroup" Width="100%" MaxLength="50" CssClass="">
                                        </dxe:ASPxTextBox>
                                    </div>
                               </div>
                             </div>
                    </div>
                    <div class="col-md-2">
                           <div class="form-group ">
                                <label for="" class=" col-form-label">BG Type</label>
                                  <div class="">
                                    <div class="relative">
                                        <dxe:ASPxTextBox ID="txtBGType" runat="server" ClientInstanceName="ctxtBGType" Width="100%" MaxLength="50" CssClass="">
                                        </dxe:ASPxTextBox>
                                    </div>
                               </div>
                             </div>
                    </div>

                  <div class="col-md-2">
                           <div class="form-group ">
                                <label for="" class=" col-form-label">Percentage</label>
                                  <div class="">
                                    <div class="relative">
                                        <dxe:ASPxTextBox ID="txtPercentage" runat="server" ClientInstanceName="ctxtPercentage" Width="100%" MaxLength="50" CssClass="">
                                               <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                        </dxe:ASPxTextBox>
                                    </div>
                               </div>
                             </div>
                    </div>

                   <div class="col-md-2">
                           <div class="form-group ">
                                <label for="" class=" col-form-label">Value </label>
                                  <div class="">
                                    <div class="relative">
                                        <dxe:ASPxTextBox ID="txtValue" runat="server" ClientInstanceName="ctxtValue" Width="100%" MaxLength="50" CssClass="">
                                               <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                        </dxe:ASPxTextBox>
                                    </div>
                               </div>
                             </div>
                    </div>
                  
                  <div class="col-md-2">
                           <div class="form-group ">
                                <label for="" class=" col-form-label">Status</label>
                                  <div class="">
                                    <div class="relative">
                                        <dxe:ASPxTextBox ID="txtStatus" runat="server" ClientInstanceName="ctxtStatus" Width="100%" MaxLength="50" CssClass="">
                                        </dxe:ASPxTextBox>
                                    </div>
                               </div>
                             </div>
                    </div>

                  <div class="col-md-2">
                           <div class="form-group ">
                                <label for="" class=" col-form-label">Validity From Date</label>
                                  <div class="">
                                    <div class="relative">
                                        <dxe:ASPxDateEdit ID="dtBGValidFromD" runat="server"  EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdtBGValidFromD" Width="100%">
                                           <ButtonStyle Width="13px">
                                           </ButtonStyle>
                                             <ClientSideEvents GotFocus="function(s,e){cdtBGValidFromD.ShowDropDown();}" />
                                       </dxe:ASPxDateEdit>
                                    </div>
                               </div>
                             </div>
                    </div>


                   <div class="col-md-2">
                           <div class="form-group ">
                                <label for="" class=" col-form-label">Validity To Date</label>
                                  <div class="">
                                    <div class="relative">
                                        <dxe:ASPxDateEdit ID="dtBGValidToD" runat="server"  EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdtBGValidToD" Width="100%">
                                           <ButtonStyle Width="13px">
                                           </ButtonStyle>
                                             <ClientSideEvents GotFocus="function(s,e){cdtBGValidToD.ShowDropDown();}" />
                                       </dxe:ASPxDateEdit>
                                    </div>
                               </div>
                             </div>
                    </div>
                      <div class="col-md-3 pdTop5">
                          <div class="mTop5"><button type="button" class="btn btn-success btn-radius mTop5" id="btnSave" onclick="AddTermsDetails()">Add BG</button></div>
                     </div>
                </div>
            </div>
              
 </div>

        <div class="clearfix mTop5"></div>
        <div class="form-group">
            <dxe:ASPxGridView ID="GrdBGDetails" runat="server" KeyFieldName="Terms_BankGuaranteeSL" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
                SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto"  OnDataBinding="GrdBGDetails_DataBinding" OnCustomCallback="GrdBGDetails_CustomCallback"
                Width="100%" Settings-VerticalScrollableHeight="100" ClientInstanceName="cGrdBGDetails">
                <SettingsSearchPanel Visible="True" Delay="5000" />
                <Columns>


                    <dxe:GridViewDataTextColumn Caption="Sl" FieldName="Terms_BankGuaranteeSL" ReadOnly="true"  Width="4%" VisibleIndex="1" Visible="false">
                        <PropertiesTextEdit>
                        </PropertiesTextEdit>
                 </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="BG Group" FieldName="BGGroup"
                        VisibleIndex="2" FixedStyle="Left" Width="150px" Visible="true">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="BG Type" FieldName="BGType"
                        VisibleIndex="3" Width="150px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Percentage" FieldName="Percentage"
                        VisibleIndex="3" Width="90px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>


                     <dxe:GridViewDataTextColumn Caption="Value" FieldName="Value"
                        VisibleIndex="3" Width="100px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                     <dxe:GridViewDataTextColumn Caption="Status" FieldName="Status"
                        VisibleIndex="3" Width="120px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="BG Validity From Date" Width="150px" FieldName="ValidityFromDate" VisibleIndex="3">
                        <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="BG Validity TO Date" FieldName="ValidityToDate" Width="150px" VisibleIndex="4">
                        <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="7" Width="60px">
                        <DataItemTemplate>

                          <%--  <a href="javascript:void(0);" onclick="ClickOnEdit('<%# Container.KeyValue %>')" id="a_editInvoice" class="pad" title="Edit">
                                <img src="../../../assests/images/info.png" /></a>--%>

                            <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="pad" title="Delete" id="a_delete">
                                <img src="../../../assests/images/Delete.png" /></a>

                            
                        </DataItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <CellStyle HorizontalAlign="Center"></CellStyle>
                        <HeaderTemplate><span>Actions</span></HeaderTemplate>
                        <EditFormSettings Visible="False"></EditFormSettings>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                </Columns>
                <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                <ClientSideEvents EndCallback="GrdBGDetails_EndCallBack" />
                <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                    <FirstPageButton Visible="True">
                    </FirstPageButton>
                    <LastPageButton Visible="True">
                    </LastPageButton>
                </SettingsPager>

                <Settings ShowGroupPanel="false" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                <SettingsLoadingPanel Text="Please Wait..." />
            </dxe:ASPxGridView>
            <%--    <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                        ContextTypeName="ERPDataClassesDataContext" TableName="ERP_TEAMVIEW" />--%>
            <asp:HiddenField runat="server" ID="hdnGuid" />
        </div>
        <div class="clear"></div>
    </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-primary" onclick="TermsConditionsSave();">Save</button>
        <button type="button" class="btn btn-danger" onclick="TermsConditionscancel();" data-dismis="modal">Cancel</button>
      </div>
    </div>
  </div>
</div>

