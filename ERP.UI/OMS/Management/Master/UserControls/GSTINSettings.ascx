<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GSTINSettings.ascx.cs" Inherits="ERP.OMS.Management.Master.UserControls.GSTINSettings" %>


<style>
     .nestedinput {
            padding: 0;
            margin: 0;
        }

            .nestedinput li {
                list-style-type: none;
                display: inline-block;
                float: left;
            }

                .nestedinput li.dash {
                    width: 26px;
                    text-align: center;
                    padding: 6px;
                }

                .nestedinput li .iconRed {
                    position: absolute;
                    right: -10px;
                    top: 5px;
                }
  .mTop16 {
        margin-top: 16px;
        }
</style>



<script>
    var currentMode = "Add";
    var EditData = "";
    function openGstin() {
        cGstinPopUp.Show();
    }
    function onCountryChange(s, e) {
        ccmbStateGstin.PerformCallback(s.GetValue());
    }

    function gstinAddOnclick(s, e) {
        if (isValidGstinDetails()) {
            cGstinGrid.PerformCallback(currentMode + "~" + EditData);
        }


        return false;
    }

    function isValidGstinDetails() {
        $('#invalidGstpopup').hide();
        $('#invalidGstStatepopup').hide();
        var returnvalue = true;
        if (ccmbStateGstin.GetText().trim() == '') {
            returnvalue = false;
            $('#invalidGstStatepopup').show();
        }

        if (ctxtGSTIN1.GetText() == '' || ctxtGSTIN2.GetText() == '' || ctxtGSTIN3.GetText() == '') {
            $('#invalidGstpopup').show();
            returnvalue = false;
        }

        var gst1 = ctxtGSTIN1.GetText().trim();
        var gst2 = ctxtGSTIN2.GetText().trim();
        var gst3 = ctxtGSTIN3.GetText().trim();



        if (gst1.length != 2 || gst2.length != 10 || gst3.length != 3) {
            $('#invalidGstpopup').show();
            returnvalue = false;
        }


        var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
        var code = /([C,P,H,F,A,T,B,L,J,G])/;
        var code_chk = gst2.substring(3, 4);
        if (gst2.search(panPat) == -1) {
            $('#invalidGstpopup').show();
            returnvalue = false;
        }
        if (code.test(code_chk) == false) {
            $('#invalidGstpopup').show();
            returnvalue = false;
        }


        return returnvalue;
    }


    function ClearClick() {
        ClearAllControl();
        //   document.getElementById('cmbgstinBranch').focus();
        ccmbCountryGstin.Focus();
    }

    function ClearAllControl() {
        document.getElementById('cmbgstinBranch').value = document.getElementById('cmbgstinBranch').options[0].value;
        ccmbCountryGstin.SetSelectedIndex(0);
        onCountryChange(ccmbCountryGstin);
        ctxtGSTIN1.SetText('');
        ctxtGSTIN2.SetText('');
        ctxtGSTIN3.SetText('');
        currentMode = "Add";

    }

    function fn_Deletecity(id) {
        cGstinGrid.PerformCallback("Delete~" + id);
    }


    function fn_Editcity(obj) {
        currentMode = "Edit";
        EditData = obj;
        cGstinPanel.PerformCallback(obj);
    }

    function GstinPanelEndCallBack(s, e) {
        //document.getElementById('cmbgstinBranch').focus();
        ccmbCountryGstin.Focus();
    }

    function GstinGridEndCallBack(s, e) {
        if (cGstinGrid.cpDelete) {
            if (cGstinGrid.cpDelete == 'True') {
                jAlert("Deleted Successfully.", "Alert", function () {
                    //document.getElementById('cmbgstinBranch').focus();
                    ccmbCountryGstin.Focus();
                });
                currentMode = "Add";
                cGstinGrid.cpDelete = null;
            }
        }

        if (cGstinGrid.cpInsert) {
            if (cGstinGrid.cpInsert == 'True') {
                jAlert("GSTIN Added Successfully.", "Alert", function () {
                    //document.getElementById('cmbgstinBranch').focus();
                    ccmbCountryGstin.Focus();
                });
                ClearAllControl();
                currentMode = "Add";
                cGstinGrid.cpInsert = null;
            }
        }

        if (cGstinGrid.cpUpdate) {
            if (cGstinGrid.cpUpdate == 'True') {
                jAlert("GSTIN Updated Successfully.", "Alert", function () {
                    //document.getElementById('cmbgstinBranch').focus();
                    ccmbCountryGstin.Focus();
                });
                ClearAllControl();
                currentMode = "Add";
                cGstinGrid.cpUpdate = null;
            }
        }

        if (cGstinGrid.cpBranchExists) {
            if (cGstinGrid.cpBranchExists == 'True') {
                jAlert("GSTIN already entered for the selected branch. Cannot proceed.", "Alert", function () {
                    //document.getElementById('cmbgstinBranch').focus();
                    ccmbCountryGstin.Focus();
                });
                cGstinGrid.cpBranchExists = null;
            }
        }

    }

</script>









  <asp:Button ID="GstinSettingsButton" runat="server" Text="GSTIN Settings Branchwise"  CssClass="btn btn-primary dxbButton"  OnClientClick="openGstin();return false;"/>



   <dxe:ASPxPopupControl ID="GstinPopUp" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cGstinPopUp" Height="630px"
            Width="900" HeaderText="Add/Modify GSTIN" Modal="true" AllowResize="true" ResizingMode="Postponed">
            <contentcollection>
                                                <dxe:PopupControlContentControl runat="server">
                

                    <dxe:ASPxCallbackPanel runat="server" id="GstinPanel" ClientInstanceName="cGstinPanel" OnCallback="GstinPanel_Callback">
                                                  <PanelCollection>
                                                       <dxe:PanelContent runat="server">
                                                        <div class="row">
                                                            
                                                                  <div class="col-md-3"> 
                                                                 <dxe:ASPxLabel ID="lblgstinCountry" runat="server" Text="Country" Width="59px">
                                                                    </dxe:ASPxLabel>
                                                 
                                                                <div>
                                                                     <dxe:ASPxComboBox ID="cmbCountryGstin" runat="server" ClientInstanceName="ccmbCountryGstin"  Width="100%">
                                                                           <ClientSideEvents SelectedIndexChanged="onCountryChange" />
                                                                     </dxe:ASPxComboBox>
                                                                </div>
                                                            </div>
                                                                  <div class="col-md-3"> 
                                                                 <dxe:ASPxLabel ID="lblgstinstate" runat="server" Text="State" Width="59px">
                                                                    </dxe:ASPxLabel>
                                                 
                                                                <div>
                                                                     <dxe:ASPxComboBox ID="cmbStateGstin" runat="server" ClientInstanceName="ccmbStateGstin" OnCallback="cmbStateGstin_Callback"   Width="100%">
                                                   
                                                                </dxe:ASPxComboBox>
                                                                    
                                                                </div>
                                                            </div>

                                                             <div class="col-md-3"> 
                                                                 <dxe:ASPxLabel ID="lblbranch" runat="server" Text="Branch" Width="59px">
                                                                    </dxe:ASPxLabel>
                                                 
                                                                <div>
                                                                    <asp:DropDownList ID="cmbgstinBranch" runat="server" Width="100%"  >
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>


                                                              <span id="invalidGstStatepopup" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red;display:none;padding-top: 24px;" title="Invalid GSTIN"></span>
                                                                    <div class="clear"></div>
                                                         <div class="col-md-6">
                                                                <label >GSTIN</label>
                                                                <div class="relative"> 
                                                                      <ul class="nestedinput">
                                                                        <li>
                                                                            <dxe:ASPxTextBox ID="gstinpopup1" ClientInstanceName="ctxtGSTIN1"  MaxLength="2"    runat="server" Width="50px">
                                                                              <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                                                            </dxe:ASPxTextBox>
                                                                        </li>
                                                                        <li class="dash"> - </li>
                                                                        <li>
                                                                            <dxe:ASPxTextBox ID="gstinpopup2" ClientInstanceName="ctxtGSTIN2"  MaxLength="10"    runat="server" Width="150px"> 
                                                                          <ClientSideEvents KeyUp="Gstin2TextChanged" />
                                                                                   </dxe:ASPxTextBox>
                                                                        </li>
                                                                        <li class="dash"> - </li>
                                                                        <li>
                                                                            <dxe:ASPxTextBox ID="gstinpopup3" ClientInstanceName="ctxtGSTIN3"  MaxLength="3"   runat="server" Width="50px"> 
                                                                            </dxe:ASPxTextBox>
                                                                            <span id="invalidGstpopup" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red;display:none;padding-left: 9px;left:304px" title="Invalid GSTIN"></span>
                                                                        </li>
                                                                    </ul>   
                                                                 </div>
                                                                 </div>

                           



                                                                    <dxe:ASPxButton ID="ASPxButton1" runat="server" Text="Add" AutoPostBack="false" CssClass="btn btn-primary mTop16" >
                                                                    <clientsideevents Click="gstinAddOnclick" />
                                                                </dxe:ASPxButton>

                                                             <dxe:ASPxButton ID="ASPxButton2" runat="server" Text="Clear" AutoPostBack="false" CssClass="btn btn-danger mTop16" >
                                                                    <clientsideevents Click="ClearClick" />
                                                                </dxe:ASPxButton>

                                                        </div>

                                      </dxe:PanelContent>
                            </PanelCollection>  
                         <ClientSideEvents EndCallback="GstinPanelEndCallBack" />
                        </dxe:ASPxCallbackPanel>


      <div class="GridViewArea">
            <dxe:ASPxGridView ID="GstinGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="cGstinGrid"
                 Width="100%"   OnCustomCallback="GstinGrid_CustomCallback" CssClass="pull-left" KeyFieldName="gstin_id"> 
                <Columns>

                    <dxe:GridViewDataTextColumn Caption="Branch" FieldName="branch_description" ReadOnly="True"
                        Visible="True" FixedStyle="Left" VisibleIndex="0">
                        <EditFormSettings Visible="True" />
                          <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Country" FieldName="cou_country" ReadOnly="True"
                        Visible="True" FixedStyle="Left" VisibleIndex="1">
                        <EditFormSettings Visible="True" />
                          <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="State" FieldName="state" ReadOnly="True"
                        Visible="True" FixedStyle="Left" VisibleIndex="2">
                        <EditFormSettings Visible="True" />
                          <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="GSTIN" FieldName="GSTIN" ReadOnly="True"
                        Visible="True" FixedStyle="Left" VisibleIndex="3">
                          <Settings AutoFilterCondition="Contains" />
                        <EditFormSettings Visible="True" />
                    </dxe:GridViewDataTextColumn>
                   
                     <dxe:GridViewDataTextColumn ReadOnly="True" Width="12%" CellStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" />
                   <CellStyle HorizontalAlign="Center"></CellStyle>
                        <HeaderTemplate>
                            Actions
                        </HeaderTemplate>
                        <DataItemTemplate> 
                           
                            <a href="javascript:void(0);" onclick="fn_Editcity('<%# Container.KeyValue %>')" title="Edit" class="pad">
                                <img src="/assests/images/Edit.png" /></a>
                            
                            <a href="javascript:void(0);" onclick="fn_Deletecity('<%# Container.KeyValue %>')" title="Delete"  class="pad">
                                <img src="/assests/images/Delete.png" /></a>
                        
                        </DataItemTemplate>  
                       </dxe:GridViewDataTextColumn>
                </Columns>
                   <ClientSideEvents EndCallback="GstinGridEndCallBack" />
            </dxe:ASPxGridView>
        </div>

         </dxe:PopupControlContentControl>
        </contentcollection>
        </dxe:ASPxPopupControl>