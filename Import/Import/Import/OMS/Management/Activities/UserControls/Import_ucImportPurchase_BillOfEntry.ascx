<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Import_ucImportPurchase_BillOfEntry.ascx.cs" 
    Inherits="ERP.OMS.Management.Activities.UserControls.Import_ucImportPurchase_BillOfEntry" %>
 

<head>
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode == 46)
                return true;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        function callTCControl(docID, docType) {
            ccallBackuserControlPanelMainTC.PerformCallback('TCtagging~' + docID + '~' + docType);
        }
        function callTCspecefiFields_PO(add_cntId) {
            ccallBackuserControlPanelMainTC.PerformCallback('TCspecefiFields_PO~' + add_cntId);
        }
        function componentEndCallBack(s, e) {

            //SaveTermsConditionDataForTagging();
            //SaveTermsConditionData();
        }
        function SaveTermsConditionData() {


            if (cBillEntryDate.GetDate() == null) {
                jAlert("Bill Entry Date is mandatory.");
                ImportPurchaseShowHide(1);
                cBillEntryDate.SetFocus();
            }
            else if ($("#txtFreightRemarks").val() == null) {
                jAlert("Insurance Coverage is mandatory.");
                ImportPurchaseShowHide(1);
                ccmbInsuranceType.SetFocus();
            }
            else if ($("#txtFreightRemarks").val() == null) {
                jAlert("Freight Charges is mandatory.");
                ImportPurchaseShowHide(1);
                ccmbFreightCharges.SetFocus();
            }
            else {
                var jsDate = cBillEntryDate.GetDate();
                var myDate = new Date();
                if (jsDate != null) {
                    var year = jsDate.getFullYear(); // where getFullYear returns the year (four digits)
                    var month = jsDate.getMonth(); // where getMonth returns the month (from 0-11)
                    var day = jsDate.getDate();   // where getDate returns the day of the month (from 1-31)
                    myDate = new Date(year, month, day).toLocaleDateString('en-GB');
                }

                

                var data = $("#hfTermsConditionDocType").val() + '|'; //'SO|';1
                data += cBillEntryDate.GetDate() == null ? '@' + '|' : myDate + '|';//2
                data += $("#txtDelremarks").val() == null || $("#txtDelremarks").val() == '' ? '@' + '|' : $("#txtDelremarks").val() + '|';//3
                data += $("#txtDelremarks").val() == null || $("#txtDelremarks").val() == '' ? '@' + '|' : $("#txtDelremarks").val() + '|';//3
                
                $("#hfTermsConditionData").val(data);
                ImportPurchaseShowHide(0);
            }
        }

        
        function calcelbuttonclick() {
            ImportPurchaseShowHide(0);
            //clearTermsCondition();
        }
        function ImportPurchaseShowHide(param) {

            switch (param) {
                case 0:
                    $('#TermsConditionseModal').modal('toggle');
                    break;
                case 1:
                    $('#TermsConditionseModal').modal({
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


        }
        
        

    </script>
</head>

<body>

    <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#TermsConditionseModal" id="btn_PurchaseImport">Import Purchase</button>

    <div class="modal fade" id="TermsConditionseModal" role="dialog" aria-labelledby="TermsConditionsModalLabel" data-backdrop="static">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="TermsConditionsModalLabel">Import Purchase</h4>
                </div>
                <div class="modal-body">
                    <dxe:ASPxCallbackPanel runat="server" ID="callBackuserControlPanelMainTC" ClientInstanceName="ccallBackuserControlPanelMainTC" OnCallback="callBackuserControlPanelMainTC_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <asp:HiddenField ID="hfTCspecefiFieldsVisibilityCheck" runat="server" Value="1" />  <%--if PO then 1 otherwise 0--%>
                                <asp:Panel runat="server" ID="pnl_TCspecefiFields_Not_PO">
                                    
                                  
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">Bill Entry No</label>
                                            <asp:TextBox ID="txtBillEntryNo" runat="server" Width="100%" TextMode="MultiLine">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">Bill Entry Date</label>
                                            <dxe:ASPxDateEdit ID="dt_BillEntrydate" TabIndex="12" runat="server" Width="100%" EditFormat="Custom" ClientInstanceName="cBillEntryDate"
                                                   EditFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                            </dxe:ASPxDateEdit>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label for="recipient-name" class="control-label">Port Code</label>
                                            <asp:TextBox ID="txtPortCode" runat="server" Width="100%" TextMode="MultiLine">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                              
                                    <div class="clear"></div>

                                </asp:Panel>
                                
                                <div class="clear"></div>
                                <div class="text-center" style="text-align: center !important">
                                    <dxe:ASPxButton ID="btnTCsave" ClientInstanceName="cbtnTCsave" runat="server" AutoPostBack="False" Text="Save&#818;" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {SaveTermsConditionData();}" />
                                    </dxe:ASPxButton>
                                    <dxe:ASPxButton ID="btnTCcancel" ClientInstanceName="cbtnTCcancel" runat="server" AutoPostBack="False" Text="Cancel&#818;" CssClass="btn btn-danger" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {calcelbuttonclick();}" />
                                    </dxe:ASPxButton>
                                </div>
                            </dxe:PanelContent>
                        </PanelCollection>
                        <ClientSideEvents EndCallback="componentEndCallBack" />
                    </dxe:ASPxCallbackPanel>
                </div>
            </div>
        </div>
    </div>

</body>