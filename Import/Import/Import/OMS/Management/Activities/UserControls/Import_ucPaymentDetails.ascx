<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Import_ucPaymentDetails.ascx.cs" Inherits=" Import.Import.Import.OMS.Management.Activities.UserControls.Import_ucPaymentDetails" %>

<style>
    .table-duplicate {
        width: 100%;
    }

        .table-duplicate td > label {
            background: #215b61;
            color: #fff;
            padding: 2px 5px;
        }

    .NotValid {
        border-color: #a43a3a !important;
    }

    #paymentDetails > tr > td {
        padding-right: 15px !important;
    }

    .backdrop {
        background: #f0f0f0;
        margin-top: 15px;
        padding: 0 15px 15px 15px;
        border: 1px solid #ccc;
    }

        .backdrop .hdmain {
            font-size: 14px;
            color: #333;
            margin-bottom: 8px !important;
            background: #425799;
            padding: 5px 10px;
            color: #fff;
        }
</style>
<script>
    function LedgerButnClick(s, e) {

        var txt = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th>Main Account Name </th><th>Main Account Code </th></tr><table>";
        document.getElementById("LedgerTable").innerHTML = txt;

        setTimeout(function () { $("#txtLedgerSearch").focus(); }, 500);
        $('#txtLedgerSearch').val('');
        $('#LedgerModel').modal('show');

    }
    function LedgerKeyDown(s, e) {
        if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
            s.OnButtonClick(0);
        }
    }
    function LedgerModalkeydown(e) {

        var OtherDetails = {}
        OtherDetails.SearchKey = $("#txtLedgerSearch").val();
        OtherDetails.BranchID = $('#ddl_Branch').val();

        if (e.code == "Enter" || e.code == "NumpadEnter") {
            var HeaderCaption = [];
            HeaderCaption.push("Main Account Name");
            HeaderCaption.push("Main Account Code");
            if ($("#txtLedgerSearch").val() != "") {
                callonServer("Services/Import_Master.asmx/GetLedger", OtherDetails, "LedgerTable", HeaderCaption, "AccountIndex", "SetAccount");
            }
        }
        else if (e.code == "ArrowDown") {
            if ($("input[AccountIndex=0]"))
                $("input[AccountIndex=0]").focus();
        }
    }
    function SetAccount(Id, Name) {
        if (Id) {
            $("#hdnLedgerAccountID").val(Id);
            $('#LedgerModel').modal('hide');
            ctxtLedger.SetText(Name);
            ctxtLedger.Focus();
        }
    }
    //function ValueSelected(e, indexName) {
    //    console.log(indexName);
    //    if (e.code == "Enter" || e.code == "NumpadEnter") {
    //        var Id = e.target.parentElement.parentElement.cells[0].innerText;
    //        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
    //        if (Id) {
    //            if (indexName == "AccountIndex") {
    //                SetAccount(Id, name);
    //            }
    //        }
    //    }
    //}
</script>



<asp:HiddenField ID="HdSelectedBranch" runat="server" />
<dxe:ASPxHiddenField runat="server" Id="ClientSaveData" ClientInstanceName="cClientSaveData"></dxe:ASPxHiddenField>
<asp:HiddenField ID="hdJsonMainAccountString" runat="server" />
<asp:HiddenField ID="HdJsonBranchMainAct" runat="server" />
<asp:HiddenField ID="hdnLedgerAccountID" runat="server" />

<section class="rds col-md-12">
    <div class="">
        <div class="clearfix">
            <h4 class="fieldsettype hdmain" id="HeaderTextforPaymentDetails">Import Duty</h4>
        </div>


        <div class="row">
            <div class="col-md-2">
                <label>Payment Type</label>
               <%-- <select class="form-control" id="ddl_type1" runat="server" onchange="paymentTypeChange(event)">--%>
                <select class="form-control" id="ddl_type1" runat="server" >
                    <option>-Select-</option>
                    <%--  <option>Cash</option>
                    <option>Bank</option>--%>
                    <option>Ledger</option>
                </select>
            </div>

            <div id="dvtype1" class="clearfix" style="display: none;">
                <div id="divbankgl" class="col-md-2">
                    <label id="lblbankledger">Bank Name</label>
                    <input id="txtbankgl" runat="server" />
                </div>

                <div id="divbank" class="col-md-4">
                    <div class="row">
                        <div class="col-md-6">
                            <label id="lblinstrument">Instrument Number</label>
                            <input id="txtinstrumentnumber" runat="server" />
                        </div>
                        <div class="col-md-6">
                            <label id="lblinstrumentdate">Instrument Date</label>
                            <%--  <input id="txtinstrumentdate" runat="server" />--%>
                            <dxe:ASPxDateEdit ID="dtinstrumentdate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" TabIndex="3"
                                ClientInstanceName="cdtinstrumentdate" Width="100%" UseMaskBehavior="True">
                                <buttonstyle width="13px">
                                </buttonstyle>
                                <clientsideevents datechanged="function(s, e) { TDateChange(e)}" gotfocus="function(s,e){cdtinstrumentdate.ShowDropDown();}" />
                            </dxe:ASPxDateEdit>
                        </div>
                    </div>

                </div>
            </div>

            <div class="col-md-3 " id="LedgerID">

                <lable>Select Ledger</lable>
                </label>
                           
                    <dxe:ASPxButtonEdit ID="txtLedger" runat="server" ReadOnly="true" ClientInstanceName="ctxtLedger" Width="100%">
                        <buttons>
                                <dxe:EditButton>
                                </dxe:EditButton>
                            </buttons>
                        <clientsideevents buttonclick="function(s,e){LedgerButnClick();}" keydown="function(s,e){LedgerKeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>

            </div>

            <div class="col-md-2">
                <label id="lblamount">Amount</label>
                <input id="txtamount" runat="server" />
            </div>


        </div>



        <div class="row" style="display: none;">
            <div class="col-md-2">
                <label>Payment Type</label>
                <select class="form-control" id="ddl_type2" runat="server" onchange="paymentTypeChange1(event)">
                    <option>-Select-</option>
                    <option>Cash</option>
                    <option>Bank</option>
                    <%-- <option>Ledger</option>--%>
                </select>
            </div>

            <div id="dvtype2" class="clearfix" style="display: none;">
                <div id="divbankgl1" class="col-md-2">
                    <label id="lblbankledger1">Bank Name</label>
                    <input id="txtbankgl1" runat="server" />
                </div>
                <div id="divbank1" class="col-md-4">
                    <div class="row">
                        <div class="col-md-6" id="instNumCus2">
                            <label id="lblinstrument1">Instrument Number</label>
                            <input id="txtinstrumentnumber1" runat="server" />
                        </div>
                        <div class="col-md-6" id="instDtCus2">
                            <label id="lblinstrumentdate1">Instrument Date</label>
                            <dxe:ASPxDateEdit ID="dtinstrumentdate1" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" TabIndex="3"
                                ClientInstanceName="cdtinstrumentdate1" Width="100%" UseMaskBehavior="True">
                                <buttonstyle width="13px">
                                </buttonstyle>
                                <clientsideevents datechanged="function(s, e) { TDateChange(e)}" gotfocus="function(s,e){cdtinstrumentdate1.ShowDropDown();}" />
                            </dxe:ASPxDateEdit>
                        </div>
                    </div>
                </div>
                <div class="col-md-2">
                    <label id="lblamount1">Amount</label>
                    <input id="txtamount1" runat="server" />
                </div>
            </div>
        </div>


        <div class="row" style="display: none;">
            <div class="col-md-2">
                <label>Payment Type</label>
                <select class="form-control" id="ddl_type3" runat="server" onchange="paymentTypeChange2(event)">
                    <option>-Select-</option>
                    <option>Cash</option>
                    <option>Bank</option>
                    <%--  <option>Ledger</option>--%>
                </select>
            </div>

            <div id="dvtype3" class="clearfix" style="display: none;">
                <div id="divbankgl2" class="col-md-2">
                    <label id="lblbankledger2">Bank Name</label>
                    <input id="txtbankgl2" runat="server" />
                </div>
                <div id="divbank2" class="col-md-4">
                    <div class="row">
                        <div class="col-md-6" id="instNumCus">
                            <label id="lblinstrument2">Instrument Number</label>
                            <input id="txtinstrumentnumber2" runat="server" />
                        </div>
                        <div class="col-md-6" id="instDtCus">
                            <label id="lblinstrumentdate2">Instrument Date</label>
                            <dxe:ASPxDateEdit ID="dtinstrumentdate2" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" TabIndex="3"
                                ClientInstanceName="cPdtinstrumentdate2" Width="100%" UseMaskBehavior="True">
                                <buttonstyle width="13px">
                                                        </buttonstyle>
                                <clientsideevents datechanged="function(s, e) { TDateChange(e)}" gotfocus="function(s,e){cPdtinstrumentdate2.ShowDropDown();}" />
                            </dxe:ASPxDateEdit>
                        </div>
                    </div>
                </div>
                <div class="col-md-2">
                    <label id="lblamount2">Amount</label>
                    <input id="txtamount2" runat="server" />
                </div>
            </div>
        </div>


        <div class="modal fade" id="LedgerModel" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Ledger Search</h4>
                    </div>
                    <div class="modal-body">

                        <input type="text" onkeydown="LedgerModalkeydown(event)" id="txtLedgerSearch" autofocus width="100%" placeholder="Search By Account Name" />

                        <div id="LedgerTable">

                            <table border='1' width="100%" class="dynamicPopupTbl">
                                <tr class="HeaderStyle">
                                    <th class="hide">id</th>
                                    <th>Main Account Name</th>
                                    <th>Main Account Code</th>
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
    </div>
</section>

