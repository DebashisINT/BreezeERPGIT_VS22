<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Import_ucPaymentDetails.ascx.cs" Inherits="ERP.OMS.Management.Activities.UserControls.Import_ucPaymentDetails" %>
                <%--<link rel="stylesheet" href="https://unpkg.com/flatpickr/dist/flatpickr.min.css">--%>
  <%--    <link href="../../../assests/bootstrap/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <script src="../../../assests/bootstrap/js/bootstrap-datetimepicker.min.js"></script>--%>

    <%--<link href="Css/bootstrap-datepicker.min.css" rel="stylesheet" />
    <script src="Js/bootstrap-datepicker.min.js" type="text/javascript"></script>--%>

<%--    <script src="https://unpkg.com/flatpickr"></script>  --%> 
<style>
      .table-duplicate {
            width:100%;
        }
        .table-duplicate td>label {
            background:#215b61;
            color:#fff;
            
            padding:2px 5px;
        }
         .NotValid {
             border-color: #a43a3a !important;
         }
         #paymentDetails>tr>td {
             padding-right:15px !important;
         }
</style>
  


            <asp:HiddenField ID="HdSelectedBranch" runat="server" />
            <dxe:ASPxHiddenField runat="server" Id="ClientSaveData" ClientInstanceName="cClientSaveData"></dxe:ASPxHiddenField>
            <asp:HiddenField ID="hdJsonMainAccountString" runat="server" />
            <asp:HiddenField ID="HdJsonBranchMainAct" runat="server" />
       <section class="rds col-md-12">
                     <div class="clearfix">
                         <span class="fieldsettype" id="HeaderTextforPaymentDetails">Cash Invoice Payment Details</span>
                         <table class="pull-right pad">
                             <tbody><tr>
                                 <td><span style="background: #215b61;color: #fff;padding: 2px 5px;">Select Ledger(Cash)</span></td>
                                 <td>
                                      <dxe:ASPxComboBox ID="cmbUcpaymentCashLedger" runat="server" ClientIDMode="Static" ClientInstanceName="ccmbUcpaymentCashLedger" SelectedIndex="0"  Width="100%">
                                                  <ClientSideEvents SelectedIndexChanged="function(){cmbUcpaymentCashLedgerChanged();}" />
                                       </dxe:ASPxComboBox>
                                 </td>
                                 <td class="hd"><span style="background: #215b61;color: #fff;padding: 2px 5px;">Amount</span></td>
                                 <td><input type="text" id="cmbUcpaymentCashLedgerAmt" value="0.00" onkeypress="return isNumberKey(event)" onblur="CallRunningBalance()" /></td>
                             </tr>
                         </tbody></table>
                     </div>


                     <table class="table-duplicate" id="PaymentTable">
                         <tbody id="paymentDetails">
                             <tr>
                             <td width="10%">
                                 <label>Payment Type</label>
                                 <select class="form-control" onchange="paymentTypeChange(event)">
                                      <option>-Select-</option>
                                     <option>Card</option>
                                     <option>Cheque</option>
                                     <option>Coupon</option>
                                     <option>E Transfer</option>
                                 </select>
                             </td>
                             
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                         </tr>

                         
                         
                     </tbody></table>
                 </section>