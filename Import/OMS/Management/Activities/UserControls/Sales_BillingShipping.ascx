<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Sales_BillingShipping.ascx.cs" Inherits="ERP.OMS.Management.Activities.UserControls.Sales_BillingShipping" %>

<script src="JS/SalesBillingShipping.js"></script>


            <div>
                <table>
                    <tr>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
                <div class="row">
                    <div class="col-md-6 mbot5" id="DivBilling">
                        <div class="clearfix" style="background: #fff; border: 1px solid #ccc; padding: 0px 0 10px 0;">
                            <h5 class="headText" style="margin: 0; padding: 10px 15px 10px; background: #ececec; margin-bottom: 10px">Billing Address</h5>
                            <div style="padding-right: 8px">
                                <div class="col-md-4" style="height: auto;">
                                    <asp:Label ID="LblType" runat="server" Text="Select Address:" CssClass="newLbl"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <dxe:ASPxButtonEdit ID="ASPxButtonEdit1" runat="server" ReadOnly="true" ClientInstanceName="txtSelectBillingAdd" Width="100%">
                                        <Buttons>
                                            <dxe:EditButton>
                                            </dxe:EditButton>
                                        </Buttons>
                                        <ClientSideEvents ButtonClick="function(s,e) {SelectBillingAddClick(s,e);}" KeyDown="function(s,e){SelectBillingKeyDown(s,e);}"/>
                                    </dxe:ASPxButtonEdit>
                                </div>
                                <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                    Address1: <span style="color: red;">*</span>
                                </div>
                                <div class="col-md-8">

                                    <div class="Left_Content relative">
                                        <dxe:ASPxTextBox ID="txtAddress1" MaxLength="80" ClientInstanceName="ctxtAddress1"
                                            runat="server" Width="100%">
                                            
                                        </dxe:ASPxTextBox>
                                        <span id="badd1" style="display: none" class="mandt">
                                            <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                        </span>
                                    </div>
                                </div>
                                <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                    <%--Code--%>
                                                                            Address2:
                                                                           

                                </div>
                                <%--Start of Address2 --%>
                                <div class="col-md-8">

                                    <div class="Left_Content relative">
                                        <dxe:ASPxTextBox ID="txtAddress2" MaxLength="80" ClientInstanceName="ctxtAddress2"
                                            runat="server" Width="100%">
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>
                                <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                    Address3: 
                                </div>
                                <%--Start of Address3 --%>

                                <div class="col-md-8">

                                    <div class="Left_Content relative">
                                        <dxe:ASPxTextBox ID="txtAddress3" MaxLength="80" ClientInstanceName="ctxtAddress3"
                                            runat="server" Width="100%">
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>
                                <%--Start of Landmark --%>
                                <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                    <%--Code--%>
                                                                            Landmark:
                                                                             

                                </div>
                                <div class="col-md-8">

                                    <div class="Left_Content relative">
                                        <dxe:ASPxTextBox ID="txtlandmark" MaxLength="80" ClientInstanceName="ctxtlandmark"
                                            runat="server" Width="100%">
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>

                                <%--start of Pin/Zip.--%>
                                <div class="col-md-4" style="height: auto;">
                                    <%--Type--%>
                                    <asp:Label ID="Label8" runat="server" Text="Pin/Zip (6 Characters):" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                </div>
                                <div class="col-md-8">

                                    <div class="Left_Content relative">

                                        <dxe:ASPxTextBox ID="txtbillingPin"  ClientInstanceName="ctxtbillingPin"
                                            runat="server" Width="100%">
                                            <MaskSettings Mask="&lt;-0..999999&gt;"  AllowMouseWheel="false" />
                                            <ClientSideEvents LostFocus="BillingPinChange" />
                                        </dxe:ASPxTextBox>
                                        <asp:HiddenField ID="hdBillingPin" runat="server"></asp:HiddenField>

                                        <span id="bpin" style="display: none" class="mandt">
                                            <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                        </span>

                                    </div>
                                </div>

                                <div class="col-md-4" style="height: auto;">
                                    <%--Type--%>
                                    <asp:Label ID="Label2" runat="server" Text="Country:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                </div>
                                <div class="col-md-8">

                                   <div class="Left_Content relative">
                                        <dxe:ASPxTextBox ID="txtbillingCountry" ClientEnabled="false"  MaxLength="80" ClientInstanceName="ctxtbillingCountry"
                                            runat="server" Width="100%">
                                        </dxe:ASPxTextBox>
                                        <span id="billingcountry" style="display: none" class="mandt">
                                            <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                        </span>
                                    </div> 
                                </div>
                                <%--End of State--%>
                                <div class="col-md-4" style="height: auto;">
                                    <%--Type--%>
                                    <asp:Label ID="Label4" runat="server" Text="State:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                </div>
                                <div class="col-md-8">
                                     <div class="Left_Content relative">
                                        <dxe:ASPxTextBox ID="txtbillingState"  ClientEnabled="false"  MaxLength="80" ClientInstanceName="ctxtbillingState"
                                            runat="server" Width="100%">
                                        </dxe:ASPxTextBox>
                                        <span id="billingstate" style="display: none" class="mandt">
                                            <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                        </span>
                                    </div> 
                                    
                                </div>

                                <%--start of City/district.--%>
                                <div class="col-md-4" style="height: auto;">
                                    <asp:Label ID="Label6" runat="server" Text="City/District:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                </div>
                                <div class="col-md-8">

                                    <div class="Left_Content relative">
                                        <dxe:ASPxTextBox ID="txtbillingCity"  ClientEnabled="false"  MaxLength="80" ClientInstanceName="ctxtbillingCity"
                                            runat="server" Width="100%">
                                        </dxe:ASPxTextBox>
                                        <span id="billingcity" style="display: none" class="mandt">
                                            <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                        </span>
                                    </div> 
                                      
                                </div>

                                <%--start of Area--%>
                                <div class="col-md-4" style="height: auto;">
                                    <%--Type--%>
                                    <asp:Label ID="Label10" runat="server" Text="Area:" CssClass="newLbl"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <dxe:ASPxButtonEdit ID="txtSelectBillingArea" runat="server" ReadOnly="true" ClientInstanceName="ctxtSelectBillingArea" Width="100%">
                                        <Buttons>
                                            <dxe:EditButton>
                                            </dxe:EditButton>
                                        </Buttons>
                                        <ClientSideEvents ButtonClick="function(s,e){SelectBillingAreaClick();}" KeyDown="function(s,e){SelectBillingAreaKeyDown(s,e);}" />
                                    </dxe:ASPxButtonEdit>
                                </div>
                                  <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                    
                                                                <Label>Distance(Km):</Label>    
                                </div>
                                <div class="col-md-8">

                                    <div class="Left_Content relative">
                                        <dxe:ASPxTextBox ID="txtDistance" MaxLength="80" ClientInstanceName="ctxtDistance"
                                            runat="server" Width="100%">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;"/>
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>

                                 <div id="BillingGstDiv">
                                                        <div class="col-md-4" style="height: auto;">
                                                            <asp:Label ID="LabelBillingGst" runat="server" Text="GSTIN:" CssClass="newLbl"></asp:Label>
                                                        </div>
                                                        <div class="col-md-8">
                                                        <div class="Left_Content">
                                                             <ul class="nestedinput">
                                                                <li>
                                                                    <dxe:ASPxTextBox ID="txtBillingGSTIN1" ClientInstanceName="ctxtBillingGSTIN1" MaxLength="2" runat="server" Width="33px" readonly ="true">
                                                                    </dxe:ASPxTextBox>
                                                                </li>
                                                                 <li class="dash">-</li>
                                                                 <li>
                                                                     <dxe:ASPxTextBox ID="txtBillingGSTIN2" ClientInstanceName="ctxtBillingGSTIN2" MaxLength="10" runat="server" Width="90px" readonly ="true">
                                                                     </dxe:ASPxTextBox>
                                                                 </li>
                                                                 <li class="dash">-</li>
                                                                 <li>
                                                                     <dxe:ASPxTextBox ID="txtBillingGSTIN3" ClientInstanceName="ctxtBillingGSTIN3" MaxLength="3" runat="server" Width="50px" readonly ="true">
                                                                     </dxe:ASPxTextBox>
                                                                 </li>
                                                             </ul>
                                                        </div>
                                                    </div>
                                                        <div class="clear"></div>
                                                    </div>



                                       <div class="Left_Content relative">
                                       <asp:HiddenField ID="hdStateIdBilling"  runat="server"  />

                                   </div> 
                             
                                
                                
                                 
                                   <div class="Left_Content relative">
                                       <asp:HiddenField ID="hdStateCodeBilling"  runat="server"  />

                                    </div> 
                               


                               
                                         <div class="Left_Content relative">
                                       <asp:HiddenField ID="hdCountryIdBilling"  runat="server"  />

                                </div> 
                             
                                         <div class="Left_Content relative">
                                       <asp:HiddenField ID="hdCityIdBilling"  runat="server"  />

                                </div>
                                     <div class="Left_Content relative">
                                       <asp:HiddenField ID="hdAreaIdBilling"  runat="server"  />

                                </div>
                                <div class="clear"></div>
                                <div class="col-md-12" style="height: auto;">

                                    <a class="[ form-group ]" id="shiptosame" href="#" onclick="javascript: BillingCheckChange()">
                                        <div class="[ btn-group ]">
                                            <label for="fancy-checkbox-success" class="[ btn btn-default active ]">
                                                Ship To Same Address >>
                                            </label>
                                        </div>
                                    </a>

                                </div>


                            

                                    
                            </div>
                        </div>
                    </div>


                    <div class="col-md-6 mbot5" id="DivShipping">
                        <div class="clearfix" style="background: #fff; border: 1px solid #ccc; padding: 0px 0 10px 0;">

                            <h5 class="headText" style="margin: 0; padding: 10px 15px 10px; background: #ececec; margin-bottom: 10px">Shipping Address</h5>
                            <div style="padding-right: 8px">

                                 <div class="col-md-4" style="height: auto;">
                                    <label>Ship to party:</label>
                                </div>
                                  <div class="col-md-8">
                                    <dxe:ASPxButtonEdit ID="txtShipToPartyShippingAdd" runat="server" ReadOnly="true" ClientInstanceName="ctxtShipToPartyShippingAdd" Width="100%">
                                        <Buttons>
                                            <dxe:EditButton>
                                            </dxe:EditButton>
                                        </Buttons>
                                        <ClientSideEvents ButtonClick="function(s,e){ShipToPartyAddClick();}" KeyDown="function(s,e){ShipToPartyAddkeydown(s,e);}" />
                                    </dxe:ASPxButtonEdit>
                                </div>

                                  <asp:HiddenField ID="hdShipToParty" runat="server" />



                                <div class="col-md-4" style="height: auto;">
                                    <label>Select Address:</label>
                                </div>
                                <div class="col-md-8">
                                    <dxe:ASPxButtonEdit ID="txtSelectShippingAdd" runat="server" ReadOnly="true" ClientInstanceName="ctxtSelectShippingAdd" Width="100%">
                                        <Buttons>
                                            <dxe:EditButton>
                                            </dxe:EditButton>
                                        </Buttons>
                                        <ClientSideEvents ButtonClick="function(s,e){SelectShippingAddClick(s,e);}" KeyDown="function(s,e){SelectShippingAddKeyDown(s,e);}" />
                                    </dxe:ASPxButtonEdit>
                                </div>

                                

                                <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                    Address1: <span style="color: red;">*</span>

                                </div>
                                <div class="col-md-8">

                                    <div class="Left_Content relative">
                                        <dxe:ASPxTextBox ID="txtsAddress1" MaxLength="80" ClientInstanceName="ctxtsAddress1"
                                            runat="server" Width="100%">
                                        </dxe:ASPxTextBox>
                                        <span id="sadd1" style="display: none" class="mandt">
                                            <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                        </span>
                                    </div>
                                </div>
                                <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                    Address2:
                                                                           
                                </div>
                                <div class="col-md-8">

                                    <div class="Left_Content">
                                        <dxe:ASPxTextBox ID="txtsAddress2" MaxLength="80" ClientInstanceName="ctxtsAddress2"
                                            runat="server" Width="100%">
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>
                                <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                    Address3: 
                                </div>
                                <div class="col-md-8">

                                    <div class="Left_Content">
                                        <dxe:ASPxTextBox ID="txtsAddress3" MaxLength="80" ClientInstanceName="ctxtsAddress3"
                                            runat="server" Width="100%">
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>
                                <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                    Landmark: 
                                </div>
                                <div class="col-md-8">

                                    <div class="Left_Content">
                                        <dxe:ASPxTextBox ID="txtslandmark" MaxLength="80" ClientInstanceName="ctxtslandmark"
                                            runat="server" Width="100%">
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>



                                <div class="col-md-4" style="height: auto;">
                                    <%--Type--%>
                                    <asp:Label ID="Label9" runat="server" Text="Pin/Zip (6 Characters):" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                </div>
                                <div class="col-md-8">

                                    <div class="Left_Content relative">

                                        <dxe:ASPxTextBox ID="txtShippingPin" MaxLength="6" ClientInstanceName="ctxtShippingPin"
                                            runat="server" Width="100%">
                                              <MaskSettings Mask="&lt;-0..999999&gt;"  AllowMouseWheel="false" />
                                            <ClientSideEvents LostFocus="ShippingPinChange" />
                                        </dxe:ASPxTextBox>
                                        <asp:HiddenField ID="hdShippingPin" runat="server"></asp:HiddenField>


                                        <span id="spin" style="display: none" class="mandt">
                                            <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                        </span>
                                    </div>
                                </div>
                                <%--End of Pin/Zip.--%>




                                <div class="col-md-4" style="height: auto;">

                                    <asp:Label ID="Label3" runat="server" Text="Country:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                </div>
                                 <div class="col-md-8">

                                   <div class="Left_Content relative">
                                        <dxe:ASPxTextBox ID="txtshippingCountry" ClientEnabled="false" MaxLength="80" ClientInstanceName="ctxtshippingCountry"
                                            runat="server" Width="100%">
                                        </dxe:ASPxTextBox>
                                        <span id="shippingcountry" style="display: none" class="mandt">
                                            <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                        </span>
                                    </div> 
                                </div>
                                <%--End of Country--%>
                                <div class="col-md-4" style="height: auto;">
                                    <%--Type--%>
                                    <asp:Label ID="Label5" runat="server" Text="State:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                </div>
                                <div class="col-md-8">

                                   <div class="Left_Content relative">
                                        <dxe:ASPxTextBox ID="txtshippingState"  ClientEnabled="false"   MaxLength="80" ClientInstanceName="ctxtshippingState"
                                            runat="server" Width="100%">
                                        </dxe:ASPxTextBox>
                                        <span id="shippingstate" style="display: none" class="mandt">
                                            <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                        </span>
                                    </div> 
                                </div>


                                <%--End of State--%>
                                <div class="col-md-4" style="height: auto;">
                                    <%--Type--%>
                                    <asp:Label ID="Label7" runat="server" Text="City/District:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                </div>
                                 <div class="col-md-8">

                                   <div class="Left_Content relative">
                                        <dxe:ASPxTextBox ID="txtshippingCity" ClientEnabled="false"  MaxLength="80" ClientInstanceName="ctxtshippingCity"
                                            runat="server" Width="100%">
                                        </dxe:ASPxTextBox>
                                        <span id="shippingcity" style="display: none" class="mandt">
                                            <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                        </span>
                                    </div> 
                                </div>
                                <%--End of City/District--%>

                                <div class="col-md-4" style="height: auto;">
                                    <%--Type--%>
                                    <asp:Label ID="Label11" runat="server" Text="Area:" CssClass="newLbl"></asp:Label>
                                </div>
                                     <div class="col-md-8">
                                    <dxe:ASPxButtonEdit ID="txtSelectShippingArea" runat="server" ReadOnly="true" ClientInstanceName="ctxtSelectShippingArea" Width="100%">
                                        <Buttons>
                                            <dxe:EditButton>
                                            </dxe:EditButton>
                                        </Buttons>
                                        <ClientSideEvents ButtonClick="function(s,e){SelectShippingAreaClick();}" KeyDown="function(s,e){SelectShippingAreaKeyDown(s,e);}" />
                                    </dxe:ASPxButtonEdit>
                                </div>
  

                                 <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                    
                                                                <Label>Distance(Km):</Label>    
                                </div>
                                <div class="col-md-8">

                                    <div class="Left_Content relative">
                                        <dxe:ASPxTextBox ID="txtDistanceShipping" MaxLength="80" ClientInstanceName="ctxtDistanceShipping"
                                            runat="server" Width="100%">
                                     <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;"/>
                                                                           
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>
                                  <div id="ShippingGstDiv">
                                                        <div class="col-md-4" style="height: auto;">
                                                            <asp:Label ID="LabelGST" runat="server" Text="GSTIN:" CssClass="newLbl"></asp:Label>
                                                        </div>
                                                        <div class="col-md-8">
                                                        <div class="Left_Content">
                                                             <ul class="nestedinput">
                                                                <li>
                                                                    <dxe:ASPxTextBox ID="txtShippingGSTIN1" ClientInstanceName="ctxtShippingGSTIN1" MaxLength="2" runat="server" Width="33px" readonly ="true">
                                                                    </dxe:ASPxTextBox>
                                                                </li>
                                                                 <li class="dash">-</li>
                                                                 <li>
                                                                     <dxe:ASPxTextBox ID="txtShippingGSTIN2" ClientInstanceName="ctxtShippingGSTIN2" MaxLength="10" runat="server" Width="90px" readonly ="true">
                                                                     </dxe:ASPxTextBox>
                                                                 </li>
                                                                 <li class="dash">-</li>
                                                                 <li>
                                                                     <dxe:ASPxTextBox ID="txtShippingGSTIN3" ClientInstanceName="ctxtShippingGSTIN3" MaxLength="3" runat="server" Width="50px" readonly ="true">
                                                                     </dxe:ASPxTextBox>
                                                                 </li>
                                                             </ul>
                                                        </div>
                                                    </div>
                                                        <div class="clear"></div>
                                                    </div>

                                   <div class="Left_Content relative">
                                       <asp:HiddenField ID="hdStateCodeShipping"  runat="server"  />
                                      </div>
                              
                                 

                                   <div class="Left_Content relative">
                                       <asp:HiddenField ID="hdCountryIdShipping"  runat="server"  />

                                   </div> 
                                
                            
                                 
                                     

                                   <div class="Left_Content relative">
                                       <asp:HiddenField ID="hdStateIdShipping"  runat="server"  />

                                </div> 
                                    <div class="Left_Content relative">
                                       <asp:HiddenField ID="hdCityIdShipping"  runat="server"  />

                                </div> 
                                  <div class="Left_Content relative">
                                       <asp:HiddenField ID="hdAreaIdShipping"  runat="server"  />

                                </div>
                                


                                <div class="clear"></div>
                                <div class="col-md-12" style="height: auto;">


                                    <a class="[ form-group ]" id="billtoSame" href="#" onclick="javascript: ShippingCheckChange()">
                                        <div class="[ btn-group ]">

                                            <label for="fancy-checkbox-successShipping" class="[ btn btn-default active ]">
                                                << Bill To Same Address
                                            </label>
                                        </div>
                                    </a>


                                </div>




                            </div>

                        </div>
                    </div>
                </div>
                <%--End of Address Type--%>

            
                 

                <%--End of Area--%>


                <div class="clear"></div>
                <div class="col-md-12 pdLeft0" style="padding-top: 10px">
                      <%-- <button class="btn btn-primary">OK</button> ValidationGroup="Address"--%>

                               <dxe:ASPxButton ID="btnSave_SalesBillingShiping"  ClientInstanceName="cbtnSave_SalesBillingShiping"  runat="server"
                        AutoPostBack="False" Text="OK" CssClass="btn btn-primary" UseSubmitBehavior="false">
                        <ClientSideEvents Click="function (s, e) {ValidationBillingShipping();}" />
                    </dxe:ASPxButton>
                       
             </div>
      
           
                           
            </div>
  
     <div class="modal fade" id="billingAddressModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Select Address</h4>
                </div>
                <div class="modal-body">
                    <div id="AddressTableQuotation">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">Id</th>
                                <th>Address</th>
                                <th>Address Type</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>
       



    <!--Product Modal -->
  <div class="modal fade" id="billingAreaModel" role="dialog">
    <div class="modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title">Select Area</h4>
        </div>
        <div class="modal-body">
           <input type="text" onkeydown="areakeydown(event)"  id="txtbillingArea" autofocus width="100%" placeholder="Search By Area Name."/>
             
            <div id="billingAreatable">
                <table border='1' width="100%" class="dynamicPopupTbl">
                    <tr class="HeaderStyle">
                  <th class="hide">id</th> <th>Area</th> 
                    </tr>
                </table>
            </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        </div>
      </div>
      
    </div>
  </div>

 <div class="modal fade" id="shippingAreaModel" role="dialog">
    <div class="modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title">Select Area</h4>
        </div>
        <div class="modal-body">
           <input type="text" onkeydown="areakeydownshipping(event)"  id="txtshippingArea" autofocus width="100%" placeholder="Search By Area Name."/>
             
            <div id="shippingAreatable">
                <table border='1' width="100%" class="dynamicPopupTbl">
                    <tr class="HeaderStyle">
                  <th class="hide">id</th> <th>Area</th> 
                    </tr>
                </table>
            </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        </div>
      </div>
      
    </div>
  </div>


   <!--Product Modal -->
  <div class="modal fade" id="ShippingShipToPartyModel" role="dialog">
    <div class="modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title">Ship to Party</h4>
        </div>
        <div class="modal-body">
           <input type="text" onkeydown="ShipToPartykeydown(event)"  id="txtshippingShipToParty" autofocus width="100%" placeholder="Search By Customer Name."/>
             
            <div id="ShippingShipToPartytable">
                <table border='1' width="100%" class="dynamicPopupTbl">
                    <tr class="HeaderStyle">
          <th class="hide">Id</th> <th >Customer Name</th> <th>Unique Id</th> <th>Address</th>
                    </tr>
                </table>
            </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        </div>
      </div>
      
    </div>
  </div>
     


<div class="modal fade" id="ShiptoPartyModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Select Address</h4>
                </div>
                <div class="modal-body">
                    <div id="ShiptoPartyModelTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">Id</th>
                                <th>Address</th>
                                <th>Address Type</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>








