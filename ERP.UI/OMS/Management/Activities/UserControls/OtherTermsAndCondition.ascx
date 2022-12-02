<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OtherTermsAndCondition.ascx.cs" Inherits="ERP.OMS.Management.Activities.UserControls.OtherTermsAndCondition" %>
<head>
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script type="text/javascript">
        var canTCCallBack = true;
        $(document).ready(function () {
            $('#txt_BEValue').bind('copy paste cut', function (e) {
                e.preventDefault(); //disable cut,copy,paste
                jAlert('cut,copy & paste options are disabled !!');
            });

        });


        function AllControlInitilizeOTC() {
            if (canTCCallBack) {
                OtherSaveTermsConditionDataWhileOpening();
                canTCCallBack = false;
            }
        }

    </script>
    <script type="text/javascript">


        function isNumeric(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode == 46) {
                var inputValue = $("#inputfield").val()
                if (inputValue.indexOf('.') < 1) {
                    return true;
                }
                return false;
            }
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
    </script>
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode == 46)
                return true;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

    

        function BinducTcBank() {
            ccallBackuserControlPanelMainTC.PerformCallback();
        }

        function callOTCControl(docID, docType) {
            ccallBackuserControlPanelMainOTC.PerformCallback('TCtagging~' + docID + '~' + docType);
        }
        function callTCspecefiFields_PO(add_cntId) {
            ccallBackuserControlPanelMainTC.PerformCallback('TCspecefiFields_PO~' + add_cntId);
        }
        function OTCcomponentEndCallBack(s, e) {

            SaveOtherTermsConditionDataForTagging();
            OtherSaveTermsConditionDataWhileOpening();
            //SaveTermsConditionData();
        }
        function SaveOtherTermsConditionData() {           
            

            var jsDate1 = cdtSecurityPeriodFrom.GetDate();
            var myDate1 = new Date();
            if (jsDate1 != null) {
                var year = jsDate1.getFullYear(); // where getFullYear returns the year (four digits)
                var month = jsDate1.getMonth(); // where getMonth returns the month (from 0-11)
                var day = jsDate1.getDate();   // where getDate returns the day of the month (from 1-31)
                myDate1 = new Date(year, month, day).toLocaleDateString('en-GB');
            }

            var jsDate2 = cdtSecurityPeriodTo.GetDate();
            var myDate2 = new Date();
            if (jsDate2 != null) {
                var year = jsDate2.getFullYear(); // where getFullYear returns the year (four digits)
                var month = jsDate2.getMonth(); // where getMonth returns the month (from 0-11)
                var day = jsDate2.getDate();   // where getDate returns the day of the month (from 1-31)
                myDate2 = new Date(year, month, day).toLocaleDateString('en-GB');
            }

           

            var data = $("#hfOtherTermsConditionDocType").val() + '|';

            data += ccmbTypPO.GetValue() == null || ccmbTypPO.GetValue() == '' ? '' + '|' : ccmbTypPO.GetValue() + '|';
            data += ctxtPORCNo.GetValue() == null || ctxtPORCNo.GetValue() == '' ? '' + '|' : ctxtPORCNo.GetValue() + '|';
            data += ccmbPaymentTerms.GetValue() == null || ccmbPaymentTerms.GetValue() == '' ? '' + '|' : ccmbPaymentTerms.GetValue() + '|';
            data += ctxtPerformanceSecurityAmount.GetValue() == null || ctxtPerformanceSecurityAmount.GetValue() == '' ? '' + '|' : ctxtPerformanceSecurityAmount.GetValue() + '|';
            data += ccmbTypePerformanceSecurity.GetValue() == null || ccmbTypePerformanceSecurity.GetValue() == '' ? '' + '|' : ccmbTypePerformanceSecurity.GetValue() + '|';
            data += cdtSecurityPeriodFrom.GetDate() == null ? '' + '|' : myDate1.toString("yyyy-dd-mm") + '|';
            data += cdtSecurityPeriodTo.GetDate() == null ? '' + '|' : myDate2.toString("yyyy-dd-mm") + '|';
            data += ccmbPriceBasis.GetValue() == null || ccmbPriceBasis.GetValue() == '' ? '' + '|' : ccmbPriceBasis.GetValue() + '|';
            data += ctxtBaseDate.GetValue() == null || ctxtBaseDate.GetValue() == '' ? '' + '|' : ctxtBaseDate.GetValue() + '|';          
            data += ccmbLDTerms.GetValue() == null || ccmbLDTerms.GetValue() == '' ? '' + '|' : ccmbLDTerms.GetValue() + '|';
            data += ctxtRateLD.GetValue() == null || ctxtRateLD.GetValue() == '' ? '' + '|' : ctxtRateLD.GetValue() + '|';
            data += ccmbLDApplicable.GetValue() == null || ccmbLDApplicable.GetValue() == '' ? '' + '|' : ccmbLDApplicable.GetValue() + '|';


            $("#hfOtherTermsConditionData").val(data);
            OtherTermsConditionmodalShowHide(0);
        }


        function OtherSaveTermsConditionDataWhileOpening() {
            var jsDate1 = cdtSecurityPeriodFrom.GetDate();
            var myDate1 = new Date();
            if (jsDate1 != null) {
                var year = jsDate1.getFullYear(); // where getFullYear returns the year (four digits)
                var month = jsDate1.getMonth(); // where getMonth returns the month (from 0-11)
                var day = jsDate1.getDate();   // where getDate returns the day of the month (from 1-31)
                myDate1 = new Date(year, month, day).toLocaleDateString('en-GB');
            }

            var jsDate2 = cdtSecurityPeriodTo.GetDate();
            var myDate2 = new Date();
            if (jsDate2 != null) {
                var year = jsDate2.getFullYear(); // where getFullYear returns the year (four digits)
                var month = jsDate2.getMonth(); // where getMonth returns the month (from 0-11)
                var day = jsDate2.getDate();   // where getDate returns the day of the month (from 1-31)
                myDate2 = new Date(year, month, day).toLocaleDateString('en-GB');
            }

            var data = $("#hfOtherTermsConditionDocType").val() + '|';
            data += ccmbTypPO.GetValue() == null || ccmbTypPO.GetValue() == '' ? '' + '|' : ccmbTypPO.GetValue() + '|';
            data += ctxtPORCNo.GetValue() == null || ctxtPORCNo.GetValue() == '' ? '' + '|' : ctxtPORCNo.GetValue() + '|';
            data += ccmbPaymentTerms.GetValue() == null || ccmbPaymentTerms.GetValue() == '' ? '' + '|' : ccmbPaymentTerms.GetValue() + '|';
            data += ctxtPerformanceSecurityAmount.GetValue() == null || ctxtPerformanceSecurityAmount.GetValue() == '' ? '' + '|' : ctxtPerformanceSecurityAmount.GetValue() + '|';
            data += ccmbTypePerformanceSecurity.GetValue() == null || ccmbTypePerformanceSecurity.GetValue() == '' ? '' + '|' : ccmbTypePerformanceSecurity.GetValue() + '|';
            data += cdtSecurityPeriodFrom.GetDate() == null ? '' + '|' : myDate1.toString("yyyy-dd-mm") + '|';
            data += cdtSecurityPeriodTo.GetDate() == null ? '' + '|' : myDate2.toString("yyyy-dd-mm") + '|';
            data += ccmbPriceBasis.GetValue() == null || ccmbPriceBasis.GetValue() == '' ? '' + '|' : ccmbPriceBasis.GetValue() + '|';
            data += ctxtBaseDate.GetValue() == null || ctxtBaseDate.GetValue() == '' ? '' + '|' : ctxtBaseDate.GetValue() + '|';
            data += ccmbLDTerms.GetValue() == null || ccmbLDTerms.GetValue() == '' ? '' + '|' : ccmbLDTerms.GetValue() + '|';
            data += ctxtRateLD.GetValue() == null || ctxtRateLD.GetValue() == '' ? '' + '|' : ctxtRateLD.GetValue() + '|';
            data += ccmbLDApplicable.GetValue() == null || ccmbLDApplicable.GetValue() == '' ? '' + '|' : ccmbLDApplicable.GetValue() + '|';

            $("#hfOtherTermsConditionData").val(data);        

        }

        function SaveOtherTermsConditionDataForTagging() {

            var jsDate1 = cdtSecurityPeriodFrom.GetDate();
            var myDate1 = new Date();
            if (jsDate1 != null) {
                var year = jsDate1.getFullYear(); // where getFullYear returns the year (four digits)
                var month = jsDate1.getMonth(); // where getMonth returns the month (from 0-11)
                var day = jsDate1.getDate();   // where getDate returns the day of the month (from 1-31)
                myDate1 = new Date(year, month, day).toLocaleDateString('en-GB');
            }

            var jsDate2 = cdtSecurityPeriodTo.GetDate();
            var myDate2 = new Date();
            if (jsDate2 != null) {
                var year = jsDate2.getFullYear(); // where getFullYear returns the year (four digits)
                var month = jsDate2.getMonth(); // where getMonth returns the month (from 0-11)
                var day = jsDate2.getDate();   // where getDate returns the day of the month (from 1-31)
                myDate2 = new Date(year, month, day).toLocaleDateString('en-GB');
            }

            var data = $("#hfOtherTermsConditionDocType").val() + '|';
            data += ccmbTypPO.GetValue() == null || ccmbTypPO.GetValue() == '' ? '' + '|' : ccmbTypPO.GetValue() + '|';
            data += ctxtPORCNo.GetValue() == null || ctxtPORCNo.GetValue() == '' ? '' + '|' : ctxtPORCNo.GetValue() + '|';
            data += ccmbPaymentTerms.GetValue() == null || ccmbPaymentTerms.GetValue() == '' ? '' + '|' : ccmbPaymentTerms.GetValue() + '|';
            data += ctxtPerformanceSecurityAmount.GetValue() == null || ctxtPerformanceSecurityAmount.GetValue() == '' ? '' + '|' : ctxtPerformanceSecurityAmount.GetValue() + '|';
            data += ccmbTypePerformanceSecurity.GetValue() == null || ccmbTypePerformanceSecurity.GetValue() == '' ? '' + '|' : ccmbTypePerformanceSecurity.GetValue() + '|';
            data += cdtSecurityPeriodFrom.GetDate() == null ? '' + '|' : myDate1.toString("yyyy-dd-mm") + '|';
            data += cdtSecurityPeriodTo.GetDate() == null ? '' + '|' : myDate2.toString("yyyy-dd-mm") + '|';
            data += ccmbPriceBasis.GetValue() == null || ccmbPriceBasis.GetValue() == '' ? '' + '|' : ccmbPriceBasis.GetValue() + '|';
            data += ctxtBaseDate.GetValue() == null || ctxtBaseDate.GetValue() == '' ? '' + '|' : ctxtBaseDate.GetValue() + '|';
            data += ccmbLDTerms.GetValue() == null || ccmbLDTerms.GetValue() == '' ? '' + '|' : ccmbLDTerms.GetValue() + '|';
            data += ctxtRateLD.GetValue() == null || ctxtRateLD.GetValue() == '' ? '' + '|' : ctxtRateLD.GetValue() + '|';
            data += ccmbLDApplicable.GetValue() == null || ccmbLDApplicable.GetValue() == '' ? '' + '|' : ccmbLDApplicable.GetValue() + '|';

            $("#hfOtherTermsConditionData").val(data);  
                   
        }
        function calcelbuttonclick() {
            OtherTermsConditionmodalShowHide(0);           
        }
        function OtherTermsConditionmodalShowHide(param) {

            switch (param) {
                case 0:
                    $('#OtherTermsConditionseModal').modal('toggle');
                    break;
                case 1:
                    $('#OtherTermsConditionseModal').modal({
                        show: 'true'
                    });
                    break;
            }

        }
        function clearTermsCondition() {

            cdtDeliveryDate.SetText('');
            $("#txtDelremarks").val('');
            ccmbInsuranceType.SetText('0');
            ccmbFreightCharges.SetText('0');
            $("#txtFreightRemarks").val('');
            ctxtPermitValue.SetText('');
            $("#txtRemarks").val('');
            ccmbDelDetails.SetText('');
            $("#txtotherlocation").val('');
            ccmbCertReq.SetText('');
            ccmbDiscntrcv.SetText('');
            $("#txtDiscntrcv").val('');
            ccmbCommissionRcv.SetText('');
            $("#txtCommissionRcv").val('');

            //New PO fields
            cddlTypeOfImport.SetText('');
            $("#txtPaymentTrmRemarks").val('');
            $("#txtPaymentTerms").val('');
            cddlIncoDVTerms.SetText('');
            $("#txtIncoDVTermsRemarks").val('');
            $("#txtShippmentSchedule").val('');
            $("#txtPortOfShippment").val('');

            // Code Added By Sam on 05012018 Section Start
            cddl_PortOfShippment.SetText('');
            $("#txt_BENumber").val('');
            cdt_BEDate.SetText('');
            $("#txt_BEValue").val('')

            // Code Added By Sam on 05012018 Section Start



            $("#txtPortOfDestination").val('');
            cddlPartialShippment.SetText('');
            cddlTransshipment.SetText('');
            $("#txtPackingSpec").val('');
            cdtValidityOfOrder.SetText('');
            $("#txtValidityOfOrderRemarks").val('');
            cddlCountryOfOrigin.SetText('');
            $("#txtFreeDetentionPeriod").val('');
            $("#txtFreeDetentionPeriodRemark").val('');


            //------#16920
            ctxtBankBranchName.SetText('');
            ctxtBankBranchAddress.SetText('');
            ctxtBankBranchLandmark.SetText('');
            ctxtBankBranchPin.SetText('');
            ctxtAccountNumber.SetText('');
            ctxtSwiftCode.SetText('');
            ctxtRTGS.SetText('');
            ctxtIFSC.SetText('');
            ctxtBankRemarks.SetText('');
            cddlBankName.SetValue('0');
            ///End-----

        }



    </script>
</head>

<body>

    <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#OtherTermsConditionseModal" id="btn_OtherTermsCondition" style="background-image: none !important; background-color: #428bca;">Other Term&#818;s and Conditions</button>
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilizeOTC" />
    </dxe:ASPxGlobalEvents>
    <div class="modal fade" id="OtherTermsConditionseModal" role="dialog" aria-labelledby="OtherTermsConditionseModal" data-backdrop="static">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="Other_TermsConditionsModalLabel">Other Terms and Conditions Details</h4>
                </div>
                <div class="modal-body" >
                    <dxe:ASPxCallbackPanel runat="server" ID="callBackuserControlPanelMainOTC" ClientInstanceName="ccallBackuserControlPanelMainOTC" OnCallback="callBackuserControlPanelMainOTC_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <asp:HiddenField ID="hfOTCspecefiFieldsVisibilityCheck" runat="server" Value="1" />

                                <asp:Panel runat="server" ID="pnl_OTCspecefiFields">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">Type of PO</label>
                                            <dxe:ASPxComboBox ID="cmbTypPO" runat="server" ClientInstanceName="ccmbTypPO" Width="100%">
                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                <Items>
                                                    <dxe:ListEditItem Text="Rate Contract" Value="RateContract" />
                                                    <dxe:ListEditItem Text="Supply Order" Value="SupplyOrder" />
                                                </Items>
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">PO  / RC No.</label>
                                            <dxe:ASPxTextBox ID="txtPORCNo" runat="server" ClientInstanceName="ctxtPORCNo" Width="100%" Text="" MaxLength="30">
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>

                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">Payment Terms</label>
                                            <dxe:ASPxComboBox ID="cmbPaymentTerms" runat="server" ClientInstanceName="ccmbPaymentTerms" Width="100%">
                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                <Items>
                                                    <dxe:ListEditItem Text="Credit" Value="Credit" />
                                                    <dxe:ListEditItem Text="LC" Value="LC" />
                                                    <dxe:ListEditItem Text="Advance" Value="Advance" />
                                                    <dxe:ListEditItem Text="Against PI" Value="Against PI" />
                                                </Items>
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>

                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">Performance Security Amount</label>
                                            <dxe:ASPxTextBox ID="txtPerformanceSecurityAmount" runat="server" ClientInstanceName="ctxtPerformanceSecurityAmount" Width="100%" Text="">
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">Type of Performance Security</label>
                                            <dxe:ASPxComboBox ID="cmbTypePerformanceSecurity" runat="server" ClientInstanceName="ccmbTypePerformanceSecurity" Width="100%">
                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                <Items>
                                                    <dxe:ListEditItem Text="BG" Value="BG" />
                                                    <dxe:ListEditItem Text="Cash" Value="Cash" />
                                                    <dxe:ListEditItem Text="FDs" Value="FDs" />
                                                    <dxe:ListEditItem Text="Others" Value="Others" />
                                                </Items>
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">Security Period</label>
                                            <label>From</label>
                                            <dxe:ASPxDateEdit ID="dtSecurityPeriodFrom" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdtSecurityPeriodFrom" Width="100%">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                            </dxe:ASPxDateEdit>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label>To</label>
                                            <dxe:ASPxDateEdit ID="dtSecurityPeriodTo" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdtSecurityPeriodTo" Width="100%">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                            </dxe:ASPxDateEdit>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">Price Basis</label>
                                            <dxe:ASPxComboBox ID="cmbPriceBasis" runat="server" ClientInstanceName="ccmbPriceBasis" Width="100%">
                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                <Items>
                                                    <dxe:ListEditItem Text="Firm" Value="Firm" />
                                                    <dxe:ListEditItem Text="Variable" Value="Variable" />
                                                </Items>
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>
                                      <div class="clear"></div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label for="Payment-name" class="control-label">Number</label>
                                            <dxe:ASPxTextBox ID="txtBaseDate" runat="server" Width="100%" ClientInstanceName="ctxtBaseDate" MaxLength="30">
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                   
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">LD Terms</label>
                                            <dxe:ASPxComboBox ID="cmbLDTerms" runat="server" ClientInstanceName="ccmbLDTerms" Width="100%">
                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                <Items>
                                                    <dxe:ListEditItem Text="Applicable" Value="Applicable" />
                                                    <dxe:ListEditItem Text="Not Applicable" Value="Not Applicable" />
                                                </Items>
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label for="Payment-name" class="control-label">Rate of LD (Per Week)</label>
                                            <dxe:ASPxTextBox ID="txtRateLD" runat="server" Width="100%" ClientInstanceName="ctxtRateLD">
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">LD Applicable on</label>
                                            <dxe:ASPxComboBox ID="cmbLDApplicable" runat="server" ClientInstanceName="ccmbLDApplicable" Width="100%">
                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                <Items>
                                                    <dxe:ListEditItem Text="Remaining Supply" Value="RemainingSupply" />
                                                    <dxe:ListEditItem Text="schedule/ RO" Value="scheduleRO" />
                                                </Items>
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>
                                    <div class="clear"></div>
                                </asp:Panel>


                                <div class="clear"></div>
                                <div class="text-center" style="text-align: center !important">
                                    <dxe:ASPxButton ID="btnOTCsave" ClientInstanceName="cbtnOTCsave" runat="server" AutoPostBack="False" Text="Save&#818;" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {SaveOtherTermsConditionData();}" />
                                    </dxe:ASPxButton>
                                    <dxe:ASPxButton ID="btnOTCcancel" ClientInstanceName="cbtnOTCcancel" runat="server" AutoPostBack="False" Text="Cancel&#818;" CssClass="btn btn-danger" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {calcelbuttonclick();}" />
                                    </dxe:ASPxButton>
                                </div>
                               


                            </dxe:PanelContent>
                        </PanelCollection>
                        <ClientSideEvents EndCallback="OTCcomponentEndCallBack" />
                    </dxe:ASPxCallbackPanel>
                </div>
            </div>
        </div>
    </div>

</body>
