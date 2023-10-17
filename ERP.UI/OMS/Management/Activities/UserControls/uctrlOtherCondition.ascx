<%-------------------------------------------------------------------------------------------------------------------------------------------------
    Written by Sanchita on 28-09-2023 for V2.0.40
    Few Fields required in the Quotation Entry Module for the Purpose of Quotation Print from ERP
    New button "Other Condiion" to show instead of "Terms & Condition" Button if the settings "Show Other Condition" is set as "Yes"
    Mantis: 26868
-------------------------------------------------------------------------------------------------------------------------------------------------%>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uctrlOtherCondition.ascx.cs" Inherits="ERP.OMS.Management.Activities.UserControls.WebUserControl1" %>

<head>
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script type="text/javascript">
        var canOCCallBack = true;
        $(document).ready(function () {

            $('#txt_BEValue').bind('copy paste cut', function (e) {

                e.preventDefault(); //disable cut,copy,paste
                jAlert('cut,copy & paste options are disabled !!');
            });

        });

        function AllControlInitilizeOC() {
            if (canOCCallBack) {


                SaveOtherConditionDataWhileOpening();

                canOCCallBack = false;
            }
        }
        function callOCControl(docID, docType) {
            ccallBackuserControlPanelMainOC.PerformCallback('OCtagging~' + docID + '~' + docType);
        }
        function componentOCEndCallBack(s, e) {
            SaveOtherConditionDataForTagging();
            SaveOtherConditionDataWhileOpening();
        }

        function BuildString() {
            var jsOfferValidDate = cdtOfferValidUpto.GetDate();
            var OfferValidDate = new Date();
            if (jsOfferValidDate != null) {
                var year = jsOfferValidDate.getFullYear(); // where getFullYear returns the year (four digits)
                var month = jsOfferValidDate.getMonth(); // where getMonth returns the month (from 0-11)
                var day = jsOfferValidDate.getDate();   // where getDate returns the day of the month (from 1-31)
                OfferValidDate = new Date(year, month, day).toLocaleDateString('en-GB');
            }

            var data = $("#hfOtherConditionDocType").val() + '|'; //'SO|';1
            data += $("#txtPriceBasis").val() == null || $("#txtPriceBasis").val() == '' ? '@' + '|' : $("#txtPriceBasis").val() + '|';//2
            data += $("#txtLoadingCharges").val() == null || $("#txtLoadingCharges").val() == '' ? '@' + '|' : $("#txtLoadingCharges").val() + '|';//3
            data += $("#txtDetentionCharges").val() == null || $("#txtDetentionCharges").val() == '' ? '@' + '|' : $("#txtDetentionCharges").val() + '|';//4
            data += $("#txtDeliveryPeriod").val() == null || $("#txtDeliveryPeriod").val() == '' ? '@' + '|' : $("#txtDeliveryPeriod").val() + '|';//5
            data += $("#txtInspection").val() == null || $("#txtInspection").val() == '' ? '@' + '|' : $("#txtInspection").val() + '|';//6
            data += $("#txtPaymentTermsOther").val() == null || $("#txtPaymentTermsOther").val() == '' ? '@' + '|' : $("#txtPaymentTermsOther").val() + '|';//7
            data += cdtOfferValidUpto.GetDate() == null ? '@' + '|' : OfferValidDate + '|';//2
            data += $("#txtQuantityTol").val() == null || $("#txtQuantityTol").val() == '' ? '@' + '|' : $("#txtQuantityTol").val() + '|';//9
            data += $("#txtDimensionalTol").val() == null || $("#txtDimensionalTol").val() == '' ? '@' + '|' : $("#txtDimensionalTol").val() + '|';//10
            data += $("#txtThicknessTol").val() == null || $("#txtThicknessTol").val() == '' ? '@' + '|' : $("#txtThicknessTol").val() + '|';//11
            data += $("#txtWarranty").val() == null || $("#txtWarranty").val() == '' ? '@' + '|' : $("#txtWarranty").val() + '|';//12
            data += $("#txtDeviation").val() == null || $("#txtDeviation").val() == '' ? '@' + '|' : $("#txtDeviation").val() + '|';//13
            data += $("#txtLDClause").val() == null || $("#txtLDClause").val() == '' ? '@' + '|' : $("#txtLDClause").val() + '|';//14
            data += $("#txtInterestClause").val() == null || $("#txtInterestClause").val() == '' ? '@' + '|' : $("#txtInterestClause").val() + '|';//15
            data += $("#txtPriceEscalationClause").val() == null || $("#txtPriceEscalationClause").val() == '' ? '@' + '|' : $("#txtPriceEscalationClause").val() + '|';//16
            data += $("#txtInternalCoating").val() == null || $("#txtInternalCoating").val() == '' ? '@' + '|' : $("#txtInternalCoating").val() + '|';//17
            data += $("#txtExternalCoating").val() == null || $("#txtExternalCoating").val() == '' ? '@' + '|' : $("#txtExternalCoating").val() + '|';//18
            data += $("#txtSpecialNote").val() == null || $("#txtSpecialNote").val() == '' ? '@' + '|' : $("#txtSpecialNote").val() + '|';//19

            return data;
        }

        function SaveOtherConditionData() {
            var data = BuildString();
            $("#hfOtherConditionData").val(data);
            OtherConditionmodalShowHide(0);
            
        }

        function SaveOtherConditionDataWhileOpening() {
            var data = BuildString();
            $("#hfOtherConditionData").val(data);
        }

        function SaveOtherConditionDataForTagging() {
            var data = BuildString();
            $("#hfOtherConditionData").val(data);
        }
        function cancelOCbuttonclick() {
            OtherConditionmodalShowHide(0);
            //clearTermsCondition();
        }
        function OtherConditionmodalShowHide(param) {

            switch (param) {
                case 0:
                    $('#OtherConditionseModal').modal('toggle');
                    break;
                case 1:
                    $('#OtherConditionseModal').modal({
                        show: 'true'
                    });
                    break;
            }

        }
        function clearOtherCondition() {
            $("#txtPriceBasis").val('');
            $("#txtLoadingCharges").val('');
            $("#txtDetentionCharges").val('');
            $("#txtDeliveryPeriod").val('');
            $("#txtInspection").val('');
            $("#txtPaymentTermsOther").val('');
            cdtOfferValidUpto.SetText('');
            $("#txtQuantityTol").val('');
            $("#txtDimensionalTol").val('');
            $("#txtThicknessTol").val('');
            $("#txtWarranty").val('');
            $("#txtDeviation").val('');
            $("#txtLDClause").val('');
            $("#txtInterestClause").val('');
            $("#txtPriceEscalationClause").val('');
            $("#txtInternalCoating").val('');
            $("#txtExternalCoating").val('');
            $("#txtSpecialNote").val('');
        }

    </script>
</head>

<body>
    <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#OtherConditionseModal" id="btn_OtherCondition" 
            style="background-image:none !important;background-color: #428bca;">Othe&#818;r Conditions</button>

    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilizeOC" />
    </dxe:ASPxGlobalEvents>
    <div class="modal fade" id="OtherConditionseModal" role="dialog" aria-labelledby="OtherConditionsModalLabel" data-backdrop="static">
        <div class="modal-dialog" role="document" >
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="OtherConditionsModalLabel">Other Conditions Details</h4>
                </div>
                <div class="modal-body" style="height:550px;overflow-y:auto">
                    <dxe:ASPxCallbackPanel runat="server" ID="callBackuserControlPanelMainOC" ClientInstanceName="ccallBackuserControlPanelMainOC" OnCallback="callBackuserControlPanelMainOC_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <asp:HiddenField ID="hfOCspecefiFieldsVisibilityCheck" runat="server" Value="1" />  <%-- e.g. if SO then 1 otherwise 0--%>
                                <asp:Panel runat="server" ID="pnl_OCspecefiFields_SALEQ">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">Price Basis</label>
                                            <asp:TextBox runat="server" Width="100%" TextMode="MultiLine" ID="txtPriceBasis">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">Loading Charges</label>
                                            <asp:TextBox runat="server" Width="100%" TextMode="MultiLine" ID="txtLoadingCharges">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">Detention Charges</label>
                                            <asp:TextBox runat="server" Width="100%" TextMode="MultiLine" ID="txtDetentionCharges">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">Delivery Period</label>
                                            <asp:TextBox runat="server" Width="100%" TextMode="MultiLine" ID="txtDeliveryPeriod">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">Inspection</label>
                                            <asp:TextBox runat="server" Width="100%" TextMode="MultiLine" ID="txtInspection">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">Payment Terms</label>
                                            <asp:TextBox runat="server" Width="100%" TextMode="MultiLine" ID="txtPaymentTermsOther">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">Offer Valid Upto </label>
                                            <span style="color: red;">*</span>
                                            <dxe:ASPxDateEdit ID="dtOfferValidUpto" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdtOfferValidUpto" Width="100%">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                            </dxe:ASPxDateEdit>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">Quantity Tol </label>
                                            <asp:TextBox runat="server" Width="100%" TextMode="MultiLine" ID="txtQuantityTol">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">Dimensional Tol </label>
                                            <asp:TextBox runat="server" Width="100%" TextMode="MultiLine" ID="txtDimensionalTol">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">Thickness Tol</label>
                                            <asp:TextBox runat="server" Width="100%" TextMode="MultiLine" ID="txtThicknessTol">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">Warranty </label>
                                            <asp:TextBox runat="server" Width="100%" TextMode="MultiLine" ID="txtWarranty">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">Deviation </label>
                                            <asp:TextBox runat="server" Width="100%" TextMode="MultiLine" ID="txtDeviation">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">LD Clause </label>
                                            <asp:TextBox runat="server" Width="100%" TextMode="MultiLine" ID="txtLDClause">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">Interest Clause </label>
                                            <asp:TextBox runat="server" Width="100%" TextMode="MultiLine" ID="txtInterestClause">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">Price Escalation Clause </label>
                                            <asp:TextBox runat="server" Width="100%" TextMode="MultiLine" ID="txtPriceEscalationClause">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">Internal Coating </label>
                                            <asp:TextBox runat="server" Width="100%" TextMode="MultiLine" ID="txtInternalCoating">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">External Coating </label>
                                            <asp:TextBox runat="server" Width="100%" TextMode="MultiLine" ID="txtExternalCoating">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">Special Note </label>
                                            <asp:TextBox runat="server" Width="100%" TextMode="MultiLine" ID="txtSpecialNote">
                                            </asp:TextBox>
                                        </div>
                                    </div>

                                     <div class="clear"></div>
                                    <div class="text-center" style="text-align: center !important">
                                        <dxe:ASPxButton ID="btnOCsave" ClientInstanceName="cbtnOCsave" runat="server" AutoPostBack="False" Text="Save&#818;" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                            <ClientSideEvents Click="function(s, e) {SaveOtherConditionData();}" />
                                        </dxe:ASPxButton>
                                        <dxe:ASPxButton ID="btnOCcancel" ClientInstanceName="cbtnOCcancel" runat="server" AutoPostBack="False" Text="Cancel&#818;" CssClass="btn btn-danger" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                            <ClientSideEvents Click="function(s, e) {cancelOCbuttonclick();}" />
                                        </dxe:ASPxButton>
                                    </div>

                                </asp:Panel>
                            </dxe:PanelContent>
                        </PanelCollection>
                        <ClientSideEvents EndCallback="componentOCEndCallBack" />
                    </dxe:ASPxCallbackPanel>
                </div>
            </div>
        </div>
    </div>

</body>